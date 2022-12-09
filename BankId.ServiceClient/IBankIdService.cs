using BankId.ServiceClient.Models;
using BankId.ServiceClient.Types;

namespace BankId.ServiceClient
{
    public interface IBankIdService
    {
        Task<AuthorizeResponse> AuthenticateAsync(string endUserIp, string userVisibleData, string userNonVisibleData,
            EncodeType userDataEncodingType, DataFormat userVisibleDataFormat, string personalNumber, string requirement);

        Task<AuthorizeResponse> SignAsync(string endUserIp, string userVisibleData, string userNonVisibleData,
            EncodeType userDataEncodingType, DataFormat userVisibleDataFormat, string personalNumber, string requirement);

        Task<CollectResponse> CollectAsync(string orderRef);

        Task<bool> CancelAsync(string orderRef);

        StatusType GetStatus(CollectResponse collectResponse);

        string GetUserMessage(CollectResponse collectResponse, bool isAutomaticStart, bool isQrCodeUsed);

        string GetUserMessage(Exception exception);

        string GetQrCode(string qrStartToken, string qrStartSecret, int secondsPassed);
    }
}
