using BankID.Client;
using BankID.Client.Models;
using BankID.Client.Types;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BankID.ConsoleDemo
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("BankID-Implementation console demo has started.");

            var bankIdIsProduction = bool.Parse(ConfigurationManager.AppSettings["BankID.IsProduction"]);
            var bankIdCertificateThumbprint = ConfigurationManager.AppSettings["BankID.CertificateThumbprint"];
            var bankIdSignMessage = ConfigurationManager.AppSettings["BankID.SignMessage"];

            Console.WriteLine($"Initializing BankID client with the {(bankIdIsProduction ? "production" : "test")} environment.");

            var bankIdClient = new BankIdClient(bankIdIsProduction, bankIdCertificateThumbprint);

            var networkIp = await GetNetworkIPAsync();
            // Console.WriteLine($"Local network IP \"{networkIp}\" will be assigned as end user IP.");

            var personalNumber = GetPersonalNumberInput();

            try
            {
                Console.WriteLine("Attempting to authenticate user");

                var authenticateResponse = await bankIdClient.AuthenticateAsync(networkIp, personalNumber);
                var collectResponse = await WaitForCompletionAsync(bankIdClient, authenticateResponse.OrderRef);
            }
            catch (Exception e)
            {
                Console.WriteLine(bankIdClient.GetUserMessage(e));
            }

            try
            {
                Console.WriteLine("Attempting to sign user");

                var signResponse = await bankIdClient.SignAsync(networkIp, EncodeType.Undecoded, bankIdSignMessage, null, personalNumber);
                var collectResponse = await WaitForCompletionAsync(bankIdClient, signResponse.OrderRef);
            }
            catch (Exception e)
            {
                Console.WriteLine(bankIdClient.GetUserMessage(e));
            }

            Console.WriteLine("Press any key to end the demo.");
            Console.ReadKey();
        }

        private static async Task<CollectResponseDTO> WaitForCompletionAsync(BankIdClient bankIdClient, string orderRef)
        {
            var result = await bankIdClient.CollectAsync(orderRef);

            while (result.IsPending())
            {
                Console.WriteLine(bankIdClient.GetUserMessage(result, false, false));

                Thread.Sleep(1000);
                result = await bankIdClient.CollectAsync(orderRef);
            }

            Console.WriteLine(bankIdClient.GetUserMessage(result, false, false));

            if (result.IsComplete())
            {
                Console.WriteLine($"The authorized user's name is \"{result.CompletionData?.User?.Name}\".");
            }

            return result;
        }

        private static async Task<string> GetNetworkIPAsync()
        {
            var httpClient = new HttpClient();
            var ip = await httpClient.GetStringAsync("https://api.ipify.org");

            return ip;
        }

        private static string GetPersonalNumberInput()
        {
            string personalNumber = null;

            for (int inputAttempts = 0; !IsValidPersonalNumber(personalNumber); inputAttempts++)
            {
                if (inputAttempts > 0)
                {
                    Console.WriteLine("The social security number was invalid.");
                }

                Console.Write("Assign your social security number (12 numbers): ");
                personalNumber = GetCensoredInput();
            }

            return personalNumber;
        }

        private static string GetCensoredInput()
        {
            // https://stackoverflow.com/a/3404522/13516445
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

        private static bool IsValidPersonalNumber(string personalNumber)
        {
            // TODO: validate personal number (with pattern) for production use
            return personalNumber != null && personalNumber.Length == 12 && personalNumber.All(x => char.IsDigit(x));
        }
    }
}
