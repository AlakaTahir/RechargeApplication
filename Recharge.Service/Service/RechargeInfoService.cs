using Arch.EntityFrameworkCore.UnitOfWork;
using Recharge.Model.Entity;
using Recharge.Model.ViewModel;
using Recharge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recharge.Service.Service
{
    public class RechargeInfoService : IRechargeInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserBalanceInfoService _userBalanceInfoService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        public RechargeInfoService(IUnitOfWork unitOfWork, IUserBalanceInfoService userBalanceInfoService, ITransactionHistoryService transactionHistoryService)
        {
            _unitOfWork = unitOfWork;
            _userBalanceInfoService = userBalanceInfoService;
            _transactionHistoryService = transactionHistoryService;
        }

        public  async Task<BaseResponseModel> CreateUser(RechargeInformationRequestModel model)
        {
            try
            {
                //Check if user exist
                var user = _unitOfWork.GetRepository<UserInfo>().GetFirstOrDefault(predicate: x => x.Email == model.Email);
                Console.WriteLine("User check complete");
                //If it doesnt exist 
                if (user == null)
                {
                    var newUser = new UserInfo();
                    newUser.Id = Guid.NewGuid();
                    newUser.Name = model.Name;
                    newUser.Email = model.Email;
                    newUser.PhoneNumber = model.PhoneNumber;
                    newUser.CreatedDate = DateTime.Now;

                    _unitOfWork.GetRepository<UserInfo>().Insert(newUser);
                    await _unitOfWork.SaveChangesAsync();

                    await _userBalanceInfoService.InitiateBalance(newUser.Id);

                    return new BaseResponseModel
                    {
                        Message = "User Created Successfully",
                        Status = true
                    };

                }
                return new BaseResponseModel
                {
                    Message = "User Already Exist",
                    Status = false
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Message = $"Error Occur due to {ex.Message}",
                    Status = false
                };
            }
            
        }

        public async Task<BaseResponseModel> UpdateUser(Guid id,RechargeInformationRequestModel model) 
        {
            var user = _unitOfWork.GetRepository<UserInfo>().GetFirstOrDefault(predicate: x=> x.Id == id);
            if (user != null)
            {
                user.Name = model.Name;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.UpdatedDate = DateTime.Now;    

                _unitOfWork.GetRepository<UserInfo>().Update(user);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponseModel
                {
                    Message = "Updated Successfully",
                    Status = true
                };
            }
            return new BaseResponseModel
            { 
             Message = "Unsuccessful",
             Status = false

            };
        }

        public async Task<BaseResponseModel> DeleteUser(Guid id) 
        {
            var user = _unitOfWork.GetRepository<UserInfo>().GetFirstOrDefault(predicate: x => x.Id == id); 
            if (user != null) 
            {
             _unitOfWork.GetRepository<UserInfo>().Delete(user);
             await _unitOfWork.SaveChangesAsync();
               
                return new BaseResponseModel 
                { 
                 Message = "Deleted Successfully",
                 Status = true
                };
            }
            return new BaseResponseModel 
            { 
             Message = "Doesnt exist",
             Status = false
            };
        }

        public async Task<RechargeInformationResponseModel> GetUserById (Guid id) 
        {
            var user = _unitOfWork.GetRepository<UserInfo>().GetFirstOrDefault(predicate: x=> x.Id == id); 
            if (user != null) 
            {
                return new RechargeInformationResponseModel 
                { 
                 Name = user.Name,
                 PhoneNumber = user.PhoneNumber,    
                 Email = user.Email
                };
            }
            return null;
         
        }

        public async Task<string> RetrieveBalanceByPhoneNumber(string phoneNumber)
        {
            var user = _unitOfWork.GetRepository<UserInfo>().GetFirstOrDefault(predicate: x => x.PhoneNumber == phoneNumber);
            if (user != null)
            {
                var userBalance = await _userBalanceInfoService.RetrieveUserBalance(user.Id);

                return "NGN VTU " + $"{userBalance}";
            }
            return "NGN VTU 0";
        }

        public async Task<string> AirtimeTopup(string phoneNumber, double amount)
        {
            //get the user
            var user = _unitOfWork.GetRepository<UserInfo>().GetFirstOrDefault(predicate: x => x.PhoneNumber == phoneNumber);
            //topup balance
            if(user != null)
            {

                //check transaction history
                var historyRecord = await _transactionHistoryService.GetTodayCreditTransactionHistoryByUserId(user.Id);
                //prevent customer from recharging more than 5 times daily
                if (historyRecord.Count > 5)
                    return "You have exhauseted your limit";

                //prevent customer from recharging more than NGN2000 airtime daily
                var cummulativeCredit = 0.0;
                foreach (TransactionHistory item in historyRecord)
                {
                    cummulativeCredit = cummulativeCredit + item.Amount;
                }

                if (cummulativeCredit + amount >= 2000)
                {
                    return "You have exhauseted your limit";
                }

                var isTopupSuccessful = await _userBalanceInfoService.TopUpBalance(user.Id, amount);

                //insert in transaction history
                await _transactionHistoryService.CreateTransactionHistory(user.Id, amount, "Airtime Topup", "Credit");

                return "Topup Successful";
            }
            return "User Does not exist";
        }
        public async Task<string> TransferAirtimeTopup(string phoneNumber, double amount) 
        {
            //get the user
            var user = _unitOfWork.GetRepository<UserInfo>().GetFirstOrDefault(predicate: x => x.PhoneNumber == phoneNumber);
            //topup balance
            if (user != null) 
            {
                //check transaction history
                var historyRecord = await _transactionHistoryService.GetTodayCreditTransactionHistoryByUserId(user.Id);
                //prevent customer from recharging more than 5 times daily
                if (historyRecord.Count > 5) 
                    return "You have exhausted your limit";

                //prevent customer from recharging more than NGN2000 airtime daily
                var cummulativeCredit = 0.0;
                foreach(TransactionHistory item in historyRecord)
                {
                    cummulativeCredit = cummulativeCredit + item.Amount;
                }

                if(cummulativeCredit + amount >= 2000)
                    return "You have exhausted your limit";

                //topup balance
                var isTopupSuccessful = await _userBalanceInfoService.TopUpBalance(user.Id, amount);

                await _transactionHistoryService.CreateTransactionHistory(user.Id, amount, "Transfer Airtime Topup", "Credit");


                return "Transfer Topup Successful";

            }
            return "Not successful";
        }

        public async Task<string> MakeACall(string initiatorPhonenumber, double minutes)
        {
            //get the user
            var user = _unitOfWork.GetRepository<UserInfo>().GetFirstOrDefault(predicate: x => x.PhoneNumber == initiatorPhonenumber);

            if(user != null)
            {
                var deductibleAmount = minutes * 10;

                var isDeducted = await _userBalanceInfoService.ChargingCustomer(user.Id, deductibleAmount);
                if (isDeducted == true)
                {
                    return "Operation Successful";
                }
                return "Error occured while making call";
            }
            return "User not registered";

        }
    }
}
