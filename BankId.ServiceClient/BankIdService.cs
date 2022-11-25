using BankId.ServiceClient.Exceptions;
using BankId.ServiceClient.Models;
using BankId.ServiceClient.Types;
using RestSharp;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DataFormat = BankId.ServiceClient.Types.DataFormat;

namespace BankId.ServiceClient
{
    public class BankIdService : IBankIdService
    {
        private readonly string _productionUrl = "https://appapi2.bankid.com/rp/v5.1";
        private readonly string _testUrl = "https://appapi2.test.bankid.com/rp/v5.1";

        private readonly bool _isProduction;

        private readonly X509Certificate2 _certificate;

        private readonly RestClient _restClient;

        public BankIdService(bool isProduction, string certificateThumbprint)
        {
            // We use TLS1.2 which is the TLS version that BankID recommends
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // We increase the connection limit as the default value is quite low (10)
            ServicePointManager.DefaultConnectionLimit = 9999;

            _isProduction = isProduction;
            _certificate = GetCertificateFromStore(certificateThumbprint);
            _restClient = GetRestClient();
        }

        public async Task<AuthorizeResponse> AuthenticateAsync(string endUserIp, string userVisibleData = null, string userNonVisibleData = null,
            EncodeType userDataEncodingType = EncodeType.Undecoded, DataFormat userVisibleDataFormat = DataFormat.Regular,
            string personalNumber = null, string requirement = null)
        {
            if (string.IsNullOrEmpty(endUserIp))
            {
                throw new ArgumentException("The \"endUserIp\" argument is required.");
            }

            if (personalNumber != null && personalNumber.Length != 12)
            {
                throw new ArgumentException("The \"personalNumber\" argument must be assigned with 12 digits.");
            }

            if (userDataEncodingType == EncodeType.Undecoded && !string.IsNullOrEmpty(userVisibleData))
            {
                userVisibleData = EncodeStringToBase64(userVisibleData);

                if (userNonVisibleData != null)
                    userNonVisibleData = EncodeStringToBase64(userNonVisibleData);
            }

            if (userVisibleData != null && userVisibleData.Length > 1500)
            {
                throw new ArgumentException("The \"userVisibleData\" argument is too long. " +
                    "The Base64-encoded message must be less than 1 500 characters.");
            }

            if (userNonVisibleData != null && userNonVisibleData.Length > 1500)
            {
                throw new ArgumentException("The \"userNonVisibleData\" argument is too long. " +
                    "The Base64-encoded message must be less than 1 500 characters.");
            }

            var userVisibleDataFormatText = userVisibleDataFormat == DataFormat.Markdown ? "simpleMarkdownV1" : null;

            return await GetDeserializedObjectResponseAsync<AuthorizeResponse>("auth", new AuthorizeRequest
            {
                EndUserIp = endUserIp,
                UserVisibleData = userVisibleData,
                UserVisibleDataFormat = userVisibleDataFormatText,
                UserNonVisibleData = userNonVisibleData,
                PersonalNumber = personalNumber,
                Requirement = requirement
            });
        }

        public async Task<AuthorizeResponse> SignAsync(string endUserIp, string userVisibleData, string userNonVisibleData = null,
            EncodeType userDataEncodingType = EncodeType.Undecoded, DataFormat userVisibleDataFormat = DataFormat.Regular,
            string personalNumber = null, string requirement = null)
        {
            if (string.IsNullOrEmpty(endUserIp) || string.IsNullOrEmpty(userVisibleData))
            {
                throw new ArgumentException("The \"endUserIp\" and \"userVisibleData}\" arguments are required.");
            }

            if (personalNumber != null && personalNumber.Length != 12)
            {
                throw new ArgumentException("The \"personalNumber\" argument must be assigned with 12 digits.");
            }

            if (userDataEncodingType == EncodeType.Undecoded)
            {
                userVisibleData = EncodeStringToBase64(userVisibleData);

                if (userNonVisibleData != null)
                    userNonVisibleData = EncodeStringToBase64(userNonVisibleData);
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

            var userVisibleDataFormatText = userVisibleDataFormat == DataFormat.Markdown ? "simpleMarkdownV1" : null;

            return await GetDeserializedObjectResponseAsync<AuthorizeResponse>("sign", new AuthorizeRequest
            {
                EndUserIp = endUserIp,
                UserVisibleData = userVisibleData,
                UserVisibleDataFormat = userVisibleDataFormatText,
                UserNonVisibleData = userNonVisibleData,
                PersonalNumber = personalNumber,
                Requirement = requirement
            });
        }

        public async Task<CollectResponse> CollectAsync(string orderRef)
        {
            if (string.IsNullOrEmpty(orderRef))
            {
                throw new ArgumentException("The \"orderRef\" argument is required.");
            }

            return await GetDeserializedObjectResponseAsync<CollectResponse>("collect", new CollectRequest
            {
                OrderReference = orderRef
            });
        }

        public async Task<bool> CancelAsync(string orderRef)
        {
            if (string.IsNullOrEmpty(orderRef))
            {
                throw new ArgumentException("The \"orderRef\" argument is required.");
            }

            var response = await GetResponseAsync("cancel", new CancelRequest
            {
                OrderReference = orderRef
            });

            return response.StatusCode == HttpStatusCode.OK; // The BankId services returns an empty JSON object on success
        }

        public StatusType GetStatus(CollectResponse collectResponse)
        {
            if (collectResponse == null)
            {
                throw new ArgumentException("The \"collectResponse\" argument is required.");
            }

            if (collectResponse.IsComplete())
                return StatusType.Complete;

            if (collectResponse.IsFailed())
                return StatusType.Failed;

            if (collectResponse.IsPending())
                return StatusType.Pending;

            throw new InvalidOperationException($"Unknown status '{collectResponse?.Status?.ToLower()}'.");
        }

        public string GetUserMessage(CollectResponse collectResponse, bool isAutomaticStart = false, bool isQrCodeUsed = false)
        {
            if (collectResponse == null)
            {
                return UserMessages.UNKNOWN_STATUS;
            }

            if (collectResponse.IsPending())
            {
                if (collectResponse.HintCode == "outstandingTransaction" && !isAutomaticStart || collectResponse.HintCode == "noClient")
                    return UserMessages.RFA1;

                if (collectResponse.HintCode == "userSign")
                    return UserMessages.RFA9;

                if (collectResponse.HintCode == "outstandingTransaction" && isAutomaticStart)
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

        private async Task<RestResponse> GetResponseAsync(string resource, object body)
        {
            var request = GetRestRequest(resource, body);

            var response = await _restClient.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                if (string.IsNullOrEmpty(response.Content) && response.ErrorException != null)
                {
                    throw response.ErrorException;
                }

                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response.Content);

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

            return JsonSerializer.Deserialize<T>(response.Content);
        }

        private RestClient GetRestClient()
        {
            var options = new RestClientOptions()
            {
                BaseUrl = new Uri(_isProduction ? _productionUrl : _testUrl),
                ClientCertificates = new X509CertificateCollection
                {
                    _certificate
                },
                RemoteCertificateValidationCallback = ServerCertificateValidation
            };

            return new RestClient(options);
        }

        private RestRequest GetRestRequest(string resource, object body)
        {
            var request = new RestRequest(resource, Method.Post);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");

            // It's important to ignore null properties as the API calls otherwise fail due to "invalidParameters"
            JsonSerializerOptions serializeOptions = new(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var serializedBody = JsonSerializer.Serialize(body, serializeOptions);
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

        private X509Certificate2 GetCertificateFromStore(string certificateThumbprint)
        {
            if (string.IsNullOrEmpty(certificateThumbprint))
            {
                throw new ArgumentException("The \"certificateThumbprint\" argument is not valid.");
            }

            var storeNames = new[] { StoreName.Root, StoreName.My, StoreName.AuthRoot, StoreName.CertificateAuthority };

            var certStores = new List<X509Store>();
            foreach (var storeName in storeNames)
            {
                certStores.Add(new X509Store(storeName, StoreLocation.CurrentUser));
            }

            var certCollections = new List<X509Certificate2Collection>();
            foreach (var certStore in certStores)
            {
                try
                {
                    certStore.Open(OpenFlags.ReadOnly);

                    var certCollection = certStore.Certificates;

                    if (certCollection != null && certCollection.Count > 0)
                    {
                        certCollections.Add(certCollection);

                        certCollection = certCollection.Find(X509FindType.FindByThumbprint, certificateThumbprint, false);
                        if (certCollection.Count > 0)
                            return certCollection[0];
                    }
                }
                finally
                {
                    certStore.Close();
                }
            }

            var message = $"BankID certificate thumbprint \"{certificateThumbprint}\" was not found in the certificate stores." +
                $"{Environment.NewLine}{Environment.NewLine}" +
                $"Found certificates:";

            for (int i = 0; certStores.Count > i; i++)
            {
                message += $"{Environment.NewLine}{certStores[i].Location}/{certStores[i].Name}: {certCollections[i].Count}";
            }

            throw new Exception(message);
        }

        // We use basic custom validation since API calls can fail without this due to incomplete chains, when missing the validation certificate.
        // Complete validation is not required by us, since BankID validates the server certificate on their end.
        private bool ServerCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (!certificate.Issuer.Contains("CN=Test BankID SSL Root CA") && !certificate.Issuer.Contains("CN=BankID SSL Root CA") ||
                !certificate.Issuer.Contains("OU=Infrastructure CA") || !certificate.Issuer.Contains("O=Finansiell ID-Teknik BID AB"))
            {
                throw new Exception($"Failed to verify BankID server certificate by issuer '{certificate.Issuer}'.");
            }

            return true;
        }
    }
}
