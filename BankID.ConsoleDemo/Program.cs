using BankID.Client;
using BankID.Client.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankID.ConsoleDemo
{
    class Program
    {
        // The distinguished/subject name of the installed RP certificate to use.
        // For example "FP Testcert 2" for the RP test certificate.
        private static readonly string _certificateName = null; // TODO assign

        // Used to decide if the bank ID client should use the production or test endpoint URL.
        private static readonly bool _isProduction = true; // TODO confirm environment, use false if using test certificate

        // The personal number to use for the BankID authentication attempt.
        // Has to include century, for example "19850101xxxx".
        private static readonly string _personalNumber = null; // TODO assign

        // The message that displays to the user when signing.
        private static readonly string _signMessage = "This is a demo signing." + 
            Environment.NewLine + Environment.NewLine +
            "\"Environment.NewLine\" can be used in this message.";

        static async Task Main(string[] args)
        {
            // Set the program to use the recommended TLS version, TLS1.2.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Bumps up the connection limit as the default value is quite low (10).
            ServicePointManager.DefaultConnectionLimit = 9999;

            Console.WriteLine($"Initializing BankID client " +
                $"using {(_isProduction ? "production" : "test")} environment " +
                $"with certificate \"{_certificateName}\".");
            var bankIdClient = new BankIDClient(_isProduction, _certificateName);

            // Note: The passed IP should be the IP of the end user.
            // Grabbing and using our own local network IP as we're the end-user in this case.
            var networkIp = await GetNetworkIPAsync();
            Console.WriteLine($"Local network IP \"{networkIp}\" will be used as end user IP.");

            try
            {
                Console.WriteLine($"Attempting to authenticate user \"{_personalNumber}\" on IP \"{networkIp}\"");
                var authenticateResponse = await bankIdClient.AuthenticateAsync(networkIp, _personalNumber);
                await SleepUntilCompletionAsync(bankIdClient, authenticateResponse.OrderRef);
            }
            catch (Exception e)
            {
                Console.WriteLine(bankIdClient.GetUserMessage(e));
            }

            try
            {
                Console.WriteLine($"Attempting to sign user \"{_personalNumber}\" on IP \"{networkIp}\"");
                var signResponse = await bankIdClient.SignAsync(networkIp, EncodeType.Undecoded, _signMessage, null, _personalNumber);
                await SleepUntilCompletionAsync(bankIdClient, signResponse.OrderRef);
            }
            catch (Exception e)
            {
                Console.WriteLine(bankIdClient.GetUserMessage(e));
            }

            Console.WriteLine("Press any key to end the demo.");
            Console.ReadKey();
        }

        private static async Task<bool> SleepUntilCompletionAsync(BankIDClient bankIdClient, string orderRef)
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
                return true;
            }

            return false;
        }

        private static async Task<string> GetNetworkIPAsync()
        {
            var httpClient = new HttpClient();
            return await httpClient.GetStringAsync("https://api.ipify.org");
        }
    }
}
