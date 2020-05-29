using BankID.Client.Models;
using BankID.Client.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankID.Client
{
    public interface IBankIdClient
    {
        Task<AuthorizeResponseDTO> AuthenticateAsync(string endUserIp, string personalNumber, string requirement);

        Task<AuthorizeResponseDTO> SignAsync(string endUserIp, EncodeType inputEncodingType, string userVisibleData,
            string userNonVisibleData, string personalNumber, string requirement);

        Task<CollectResponseDTO> CollectAsync(string orderRef);

        Task<bool> CancelAsync(string orderRef);

        StatusType GetStatus(CollectResponseDTO collectResponse);

        string GetUserMessage(CollectResponseDTO collectResponse, bool isAutomaticStart, bool isQrCodeUsed);

        string GetUserMessage(Exception exception);
    }
}
