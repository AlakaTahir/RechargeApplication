using Arch.EntityFrameworkCore.UnitOfWork;
using Recharge.Model.Entity;
using Recharge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recharge.Service.Service
{
    public class UserBalanceInfoService : IUserBalanceInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserBalanceInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> InitiateBalance(Guid userId) 
        {
            var newUserBalance = new UserBalance();
            newUserBalance.Id  = Guid.NewGuid();
            newUserBalance.UserId = userId;
            newUserBalance.Balance = 0;
            newUserBalance.Createddate = DateTime.Now;

            _unitOfWork.GetRepository<UserBalance>().Insert(newUserBalance);
            await _unitOfWork.SaveChangesAsync();

            return true;

        }

        public async Task<bool> TopUpBalance (Guid userId, double amount) 
        {
            var userBalance = _unitOfWork.GetRepository<UserBalance>().GetFirstOrDefault(predicate: x=> x.UserId == userId);
            if (userBalance != null) 
            { 
             userBalance.Balance = userBalance.Balance+amount;
             userBalance.UpdatedDate = DateTime.Now;

             _unitOfWork.GetRepository<UserBalance>().Update(userBalance);
             await _unitOfWork.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> ChargingCustomer(Guid userId, double amount) 
        {
            var userBalance = _unitOfWork.GetRepository<UserBalance>().GetFirstOrDefault(predicate: x=> x.UserId == userId);
            if (userBalance != null)
            {
             userBalance.Balance = userBalance.Balance - amount;
             userBalance.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<UserBalance>().Update(userBalance);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<double> RetrieveUserBalance(Guid userId) 
        {
            var userBalance = _unitOfWork.GetRepository<UserBalance>().GetFirstOrDefault(predicate: x=> x.UserId == userId);
            if (userBalance != null) 
            {
                return userBalance.Balance;
            }
            return 0;

        } 
    }
}
