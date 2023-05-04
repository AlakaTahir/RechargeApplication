using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recharge.Model.ViewModel;
using Recharge.Service.Interface;
using System;
using System.Threading.Tasks;

namespace Recharge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RechargeInfoController : ControllerBase
    {
        private readonly IRechargeInfoService _rechargeInfoService;
        public RechargeInfoController(IRechargeInfoService rechargeInfoService)
        {
            _rechargeInfoService = rechargeInfoService;

        }

        [HttpPost("CreateUser")]

        public async Task<IActionResult> CreateUser(RechargeInformationRequestModel model)
        {
            var response = await _rechargeInfoService.CreateUser(model);
            return Ok(response);
        }
       
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(Guid id, RechargeInformationRequestModel model)
        {
            var response = await _rechargeInfoService.UpdateUser(id, model);
            return Ok(response);
        }
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUserDeleteUser(Guid id) 
        {
            var response = await _rechargeInfoService.DeleteUser(id);
            return Ok(response);
        }
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(Guid id) 
        {
         var response = await _rechargeInfoService.GetUserById(id);
            return Ok(response);

        }
        [HttpGet("BalanceByPhone")]
        public async Task<IActionResult> RetrieveBalanceByPhoneNumber(string phoneNumber) 
        {
            var response = await _rechargeInfoService.RetrieveBalanceByPhoneNumber(phoneNumber);
            return Ok(response);
        }
        [HttpPost("AirtimeTopUp")]
        public async Task<IActionResult> AirtimeTopup(string phoneNumber, double amount) 
        {
             var response = await _rechargeInfoService.AirtimeTopup(phoneNumber, amount);
             return Ok(response);
        }
        [HttpPost("TransferAirtimeTopUp")]
        public async Task<IActionResult> TransferAirtimeTopup(string phoneNumber, double amount) 
        {
             var response = await _rechargeInfoService.TransferAirtimeTopup(phoneNumber,amount);
             return Ok(response);
        }
        [HttpPost ("MakeACall")]
        public async Task<IActionResult> MakeACall(string initiatorPhonenumber, double minutes) 
        { 
             var response = await _rechargeInfoService.MakeACall(initiatorPhonenumber, minutes);
            return Ok(response);

        }
    }
        
}
