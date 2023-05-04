using Recharge.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recharge.Service.Interface
{
    public interface IRechargeInfoService
    {
        Task<BaseResponseModel> CreateUser(RechargeInformationRequestModel model);
        Task<BaseResponseModel> UpdateUser(Guid id, RechargeInformationRequestModel model);
        Task<BaseResponseModel> DeleteUser(Guid id);
        Task<RechargeInformationResponseModel> GetUserById(Guid id);
        Task<string> RetrieveBalanceByPhoneNumber(string phoneNumber);
        Task<string> AirtimeTopup(string phoneNumber, double amount);

        Task<string> TransferAirtimeTopup(string phoneNumber, double amount);
        Task<string> MakeACall(string initiatorPhonenumber, double minutes);

    }
}
