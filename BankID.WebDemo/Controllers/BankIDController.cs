using BankID.Client;
using BankID.Client.Models;
using BankID.WebDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BankID.WebDemo.Controllers
{
    public class BankIDController : ApiController
    {
        // The distinguished/subject name of the installed RP certificate to use.
        // For example "FP Testcert 2" for the RP test certificate.
        private static readonly string CertificateName = null; // TODO assign

        // Used to decide if the bank ID client should use the production or test endpoint URL.
        private static readonly bool IsProduction = true; // TODO confirm environment, use false if using test certificate

        private static readonly BankIDClient BankIdClient = new BankIDClient(IsProduction, CertificateName);

        [HttpPost]
        [Route("api/bankid/authenticate")]
        public async Task<AuthorizeResponseDTO> AuthenticateAsync([FromBody]string personalNumber)
        {
            try
            {
                var ipAddress = await Request.GetClientIpStringAsync();

                return await BankIdClient.AuthenticateAsync(ipAddress, personalNumber);
            }
            catch (Exception e)
            {
                var errorMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(BankIdClient.GetUserMessage(e))
                };

                throw new HttpResponseException(errorMessage);
            }
        }

        [HttpGet]
        [Route("api/bankid/collect")]
        public async Task<StatusModel> CollectAsync(string orderRef)
        {
            try
            {
                var collectResponse = await BankIdClient.CollectAsync(orderRef);
                var status = BankIdClient.GetStatus(collectResponse);
                var userMessage = BankIdClient.GetUserMessage(collectResponse);

                return new StatusModel(collectResponse, status.ToString(), userMessage);
            }
            catch (Exception e)
            {
                var errorMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(BankIdClient.GetUserMessage(e))
                };

                throw new HttpResponseException(errorMessage);
            }
        }
    }

    // https://stackoverflow.com/a/38096572
    static class HttpRequestMessageExtensions
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext = "MS_OwinContext";

        private static string LocalIp { get; set; }

        public static async Task<string> GetClientIpStringAsync(this HttpRequestMessage request)
        {
            //Local
            if (request.IsLocal())
            {
                if (LocalIp == null)
                {
                    var httpClient = new HttpClient();
                    LocalIp = await httpClient.GetStringAsync("https://api.ipify.org");
                }
                return LocalIp;
            }

            //Web-hosting
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }
            //Self-hosting
            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }
            //Owin-hosting
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic ctx = request.Properties[OwinContext];
                if (ctx != null)
                {
                    return ctx.Request.RemoteIpAddress;
                }
            }
            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.Request.UserHostAddress;
            }

            // Always return all zeroes for any failure
            return "0.0.0.0";
        }

        public static async Task<IPAddress> GetClientIpAddressAsync(this HttpRequestMessage request)
        {
            var ipString = await request.GetClientIpStringAsync();
            IPAddress ipAddress = new IPAddress(0);
            if (IPAddress.TryParse(ipString, out ipAddress))
            {
                return ipAddress;
            }

            return ipAddress;
        }

    }
}
