using System;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankID.Client.Exceptions;
using BankID.Client.Models;
using BankID.Client.Types;
using Newtonsoft.Json;
using RestSharp;

namespace BankID.Client
{
    public class BankIDClient
    {
        private readonly string _productionUrl = "https://appapi2.bankid.com/rp/v5";
        private readonly string _testUrl = "https://appapi2.test.bankid.com/rp/v5";

        private readonly bool _isProduction;

        private readonly X509Certificate2 _certificate;

        public BankIDClient(bool isProductionEnvironment, string certificateName)
        {
            this._isProduction = isProductionEnvironment;
            this._certificate = GetCertificateFromStore(certificateName);
        }

        public async Task<AuthorizeResponseDTO> AuthenticateAsync(string endUserIp, string personalNumber = null, string requirement = null)
        {
            if (string.IsNullOrEmpty(endUserIp))
            {
                throw new ArgumentException("The \"endUserIp\" argument is required.");
            }

            return await GetDeserializedObjectResponseAsync<AuthorizeResponseDTO>("auth", new AuthRequestDTO
            {
                EndUserIp = endUserIp,
                PersonalNumber = personalNumber,
                Requirement = requirement
            });
        }

        public async Task<AuthorizeResponseDTO> SignAsync(string endUserIp, EncodeType inputEncodingType, string userVisibleData, 
            string userNonVisibleData = null, string personalNumber = null, string requirement = null)
        {
            if (string.IsNullOrEmpty(endUserIp) || string.IsNullOrEmpty(userVisibleData))
            {
                throw new ArgumentException("The \"endUserIp\" and \"userVisibleData}\" arguments are required.");
            }

            if (inputEncodingType == EncodeType.Undecoded)
            {
                userVisibleData = EncodeStringToBase64(userVisibleData);

                if (userNonVisibleData != null)
                {
                    userNonVisibleData = EncodeStringToBase64(userNonVisibleData);
                }
            }

            if (userVisibleData.Length > 40000)
            {
                throw new ArgumentException("The \"userVisibleData\" argument is too long. " +
                    "The Base64-encoded message must be less than 40 000 characters.");
            }

            if (userNonVisibleData != null && userNonVisibleData.Length > 200000)
            {
                throw new ArgumentException("The \"userNonVisibleData\" argument is too long. " +
                    "The Base64-encoded message must be less than 200 000 characters.");
            }

            return await GetDeserializedObjectResponseAsync<AuthorizeResponseDTO>("sign", new SignRequestDTO
            {
                EndUserIp = endUserIp,
                UserVisibleData = userVisibleData,
                UserNonVisibleData = userNonVisibleData,
                PersonalNumber = personalNumber,
                Requirement = requirement
            });
        }

        public async Task<CollectResponseDTO> CollectAsync(string orderRef)
        {
            if (string.IsNullOrEmpty(orderRef))
            {
                throw new ArgumentException("The \"orderRef\" argument is required.");
            }

            return await GetDeserializedObjectResponseAsync<CollectResponseDTO>("collect", new CollectRequestDTO
            {
                OrderRef = orderRef
            });
        }

        public async Task<bool> CancelAsync(string orderRef)
        {
            if (string.IsNullOrEmpty(orderRef))
            {
                throw new ArgumentException("The \"orderRef\" argument is required.");
            }

            var response = await GetResponseAsync("cancel", new CancelRequestDTO
            {
                OrderRef = orderRef
            });

            return response.StatusCode == HttpStatusCode.OK; // empty JSON object returned on success
        }

        public string GetUserMessage(CollectResponseDTO collectResponse, bool automaticStart = false, bool isQrCodeUsed = false)
        {
            if (collectResponse == null)
            {
                return UserMessages.UNKNOWN_STATUS;
            }

            if (collectResponse.IsPending())
            {
                if (!automaticStart && collectResponse.HintCode == "outstandingTransaction" || collectResponse.HintCode == "noClient")
                    return UserMessages.RFA1;

                if (collectResponse.HintCode == "userSign")
                    return UserMessages.RFA9;

                if (automaticStart && collectResponse.HintCode == "outstandingTransaction")
                    return UserMessages.RFA13;

                if (collectResponse.HintCode == "started")
                    return UserMessages.RFA14AB15AB;

                return UserMessages.RFA21;
            }

            if (collectResponse.IsFailed())
            {
                if (collectResponse.HintCode == "cancelled")
                    return UserMessages.RFA3;

                if (collectResponse.HintCode == "userCancel")
                    return UserMessages.RFA6;

                if (collectResponse.HintCode == "expiredTransaction")
                    return UserMessages.RFA8;

                if (collectResponse.HintCode == "certificateErr")
                    return UserMessages.RFA16;

                if (collectResponse.HintCode == "startFailed")
                    return isQrCodeUsed ? UserMessages.RFA17B : UserMessages.RFA17A;

                return UserMessages.RFA22;
            }

            if (collectResponse.IsComplete())
            {
                return UserMessages.SUCCESSFUL_AUTHENTICATION;
            }

            return UserMessages.UNKNOWN_STATUS;
        }

        public string GetUserMessage(Exception exception)
        {
            if (exception is AlreadyInProgressException)
                return UserMessages.RFA4;

            if (exception is RequestTimeoutException || exception is InternalErrorException || exception is MaintenanceException)
                return UserMessages.RFA5;

            return UserMessages.RFA22;
        }

        private async Task<IRestResponse> GetResponseAsync(string resource, object body)
        {
            var client = GetRestClient();
            var request = GetRestRequest(resource, body);

            var response = await client.ExecuteTaskAsync(request);

            if (!response.IsSuccessful)
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDTO>(response.Content);

                if (errorResponse?.ErrorCode == "alreadyInProgress")
                    throw new AlreadyInProgressException();

                if (errorResponse?.ErrorCode == "requestTimeout")
                    throw new RequestTimeoutException();

                if (errorResponse?.ErrorCode == "internalError")
                    throw new InternalErrorException();

                if (errorResponse?.ErrorCode == "Maintenance")
                    throw new MaintenanceException();

                if (!string.IsNullOrEmpty(errorResponse?.ErrorCode))
                    throw new Exception(errorResponse.ErrorCode + ": " + errorResponse.Details);
            }

            return response;
        }

        private async Task<T> GetDeserializedObjectResponseAsync<T>(string resource, object body)
        {
            var response = await GetResponseAsync(resource, body);

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        private RestClient GetRestClient()
        {
            var client = new RestClient(_isProduction ? _productionUrl : _testUrl);
            client.ClientCertificates = new X509CertificateCollection
            {
                _certificate
            };

            return client;
        }

        private RestRequest GetRestRequest(string resource, object body)
        {
            var request = new RestRequest(resource, Method.POST);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            // It's important to ignore null properties as the API calls otherwise fail due to "invalidParameters"
            string serializedBody = JsonConvert.SerializeObject(body, new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
            request.AddJsonBody(serializedBody);

            return request;
        }

        private string EncodeStringToBase64(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("The \"input\" argument is not valid.");
            }

            var bytes = Encoding.Default.GetBytes(input);
            input = Encoding.UTF8.GetString(bytes);

            bytes = Encoding.UTF8.GetBytes(input);
            var base64String = Convert.ToBase64String(bytes);

            if (string.IsNullOrEmpty(base64String))
            {
                throw new Exception("Failed to encode the input.");
            }

            return base64String;
        }

        private X509Certificate2 GetCertificateFromStore(string certName)
        {
            if (string.IsNullOrEmpty(certName))
            {
                throw new ArgumentException("The \"certName\" argument is not valid.");
            }

            var certStores = new X509Store[]
            {
                new X509Store(StoreName.Root),
                new X509Store(StoreName.My)
            };
            foreach (var certStore in certStores)
            {
                try
                {
                    certStore.Open(OpenFlags.ReadOnly);

                    var certCollection = certStore.Certificates;
                    certCollection = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

                    var targetCert = certCollection.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);
                    if (targetCert.Count == 0)
                    {
                        targetCert = certCollection.Find(X509FindType.FindBySubjectName, certName, false);
                    }
                    if (targetCert.Count > 0)
                    {
                        return targetCert[0];
                    }
                }
                finally
                {
                    certStore.Close();
                }
            }

            throw new Exception($"Certificate \"{certName}\" was not found in the certificate stores.");
        }
    }
}
