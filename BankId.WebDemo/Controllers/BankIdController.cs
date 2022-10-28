using BankId.ServiceClient;
using BankId.ServiceClient.Models;
using BankId.ServiceClient.Types;
using BankId.WebDemo.Extensions;
using BankId.WebDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankId.WebDemo.Controllers
{
    [Route("api/bankid")]
    public class BankIdController : ControllerBase
    {
        private readonly IBankIdService _bankIdService;

        public BankIdController(IBankIdService bankIdClient)
        {
            _bankIdService = bankIdClient;
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody]AuthenticateModel contract)
        {
            try
            {
                if (contract.EndUserIp == null)
                {
                    contract.EndUserIp = await Request.HttpContext.GetClientIpAsync();
                }

                var authenticationAttempt = await _bankIdService.AuthenticateAsync(contract.EndUserIp, contract.UserVisibleData,
                    contract.UserNonVisibleData, EncodeType.Undecoded, DataFormat.Regular,
                    contract.PersonalNumber, contract.Requirement);

                return Ok(authenticationAttempt);
            }
            catch (Exception e)
            {
                return StatusCode(500, _bankIdService.GetUserMessage(e));
            }
        }

        [HttpPost]
        [Route("sign")]
        public async Task<IActionResult> SignAsync(SignModel contract)
        {
            try
            {
                if (contract.EndUserIp == null)
                {
                    contract.EndUserIp = await Request.HttpContext.GetClientIpAsync();
                }

                var signAttempt = await _bankIdService.SignAsync(contract.EndUserIp, contract.UserVisibleData,
                    contract.UserNonVisibleData, EncodeType.Undecoded, DataFormat.Regular,
                    contract.PersonalNumber, contract.Requirement);

                return Ok(signAttempt);
            }
            catch (Exception e)
            {
                return StatusCode(500, _bankIdService.GetUserMessage(e));
            }
        }

        [HttpPost]
        [Route("cancel")]
        public async Task<IActionResult> CancelAsync(string orderRef)
        {
            try
            {
                var cancellationAttempt = await _bankIdService.CancelAsync(orderRef);
                return Ok(cancellationAttempt);
            }
            catch (Exception e)
            {
                return StatusCode(500, _bankIdService.GetUserMessage(e));
            }
        }

        [HttpGet]
        [Route("collect")]
        public async Task<IActionResult> CollectAsync(string orderRef, bool isAutomaticStart, bool isQrCodeUsed)
        {
            try
            {
                var collectResponse = await _bankIdService.CollectAsync(orderRef);

                var status = _bankIdService.GetStatus(collectResponse);
                var userMessage = _bankIdService.GetUserMessage(collectResponse, isAutomaticStart, isQrCodeUsed);

                var statusModel = new StatusModel(collectResponse, status.ToString(), userMessage);

                return Ok(statusModel);
            }
            catch (Exception e)
            {
                return StatusCode(500, _bankIdService.GetUserMessage(e));
            }
        }
    }
}
