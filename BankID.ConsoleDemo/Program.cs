using BankId.ServiceClient;
using BankId.ServiceClient.Models;
using BankId.ServiceClient.Types;
using Microsoft.Extensions.Configuration;

Console.WriteLine("BankID-Implementation console demo has started.");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json");

var config = configuration.Build();

var bankIdIsProduction = bool.Parse(config["BankID:IsProduction"]);
var bankIdCertificateThumbprint = config["BankID:CertificateThumbprint"];
var bankIdSignMessage = config["BankID:SignMessage"];

Console.WriteLine($"Initializing BankID client with the {(bankIdIsProduction ? "production" : "test")} environment.");

var bankIdClient = (IBankIdService)new BankIdService(bankIdIsProduction, bankIdCertificateThumbprint);

var networkIp = await GetNetworkIpAsync();

var personalNumber = GetPersonalNumberInput();

try
{
    Console.WriteLine("Attempting to authenticate user");

    var authenticateResponse = await bankIdClient.AuthenticateAsync(networkIp, null, null, 
        EncodeType.Undecoded, DataFormat.Regular, personalNumber, null);

    var collectResponse = await WaitForCompletionAsync(bankIdClient, authenticateResponse.OrderReference);
}
catch (Exception e)
{
    Console.WriteLine(bankIdClient.GetUserMessage(e));
}

try
{
    Console.WriteLine("Attempting to sign user");

    var signResponse = await bankIdClient.SignAsync(networkIp, bankIdSignMessage, null, 
        EncodeType.Undecoded, DataFormat.Regular, personalNumber, null);

    var collectResponse = await WaitForCompletionAsync(bankIdClient, signResponse.OrderReference);
}
catch (Exception e)
{
    Console.WriteLine(bankIdClient.GetUserMessage(e));
}

Console.WriteLine("Press any key to end the demo.");
Console.ReadKey();


async Task<CollectResponse> WaitForCompletionAsync(IBankIdService bankIdClient, string orderRef)
{
    CollectResponse collectResponse = null;

    while (collectResponse == null || collectResponse.IsPending())
    {
        Thread.Sleep(1000);
        collectResponse = await bankIdClient.CollectAsync(orderRef);
        Console.WriteLine(bankIdClient.GetUserMessage(collectResponse, false, false));
    }

    if (collectResponse.IsComplete())
    {
        Console.WriteLine($"The authorized user's name is \"{collectResponse.CompletionData?.User?.Name}\".");
    }

    return collectResponse;
}

async Task<string> GetNetworkIpAsync()
{
    var httpClient = new HttpClient();
    var ip = await httpClient.GetStringAsync("https://api.ipify.org");

    return ip;
}

string GetPersonalNumberInput()
{
    string personalNumber = null;
    bool hasReceivedFirstInput = false;

    while (!IsValidPersonalNumber(personalNumber))
    {
        if (hasReceivedFirstInput)
        {
            Console.WriteLine("The social security number was invalid.");
        }

        Console.Write("Assign your social security number (12 numbers): ");
        personalNumber = GetCensoredInput();
        hasReceivedFirstInput = true;
    }

    return personalNumber;
}

// Obtained from https://stackoverflow.com/a/3404522/13516445
string GetCensoredInput()
{
    string input = "";
    do
    {
        ConsoleKeyInfo key = Console.ReadKey(true);
        // Backspace Should Not Work
        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
        {
            input += key.KeyChar;
            Console.Write("*");
        }
        else
        {
            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input.Substring(0, (input.Length - 1));
                Console.Write("\b \b");
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
        }
    } while (true);

    return input;
}

bool IsValidPersonalNumber(string personalNumber)
{
    // TODO: validate personal number (with pattern) for production use
    return personalNumber != null && personalNumber.Length == 12 && personalNumber.All(x => char.IsDigit(x));
}