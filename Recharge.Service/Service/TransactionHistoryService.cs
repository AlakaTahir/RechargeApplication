using Arch.EntityFrameworkCore.UnitOfWork;
using Recharge.Model.Entity;
using Recharge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recharge.Service.Service
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionHistoryService(IUnitOfWork unitOfWork)

        {
            _unitOfWork = unitOfWork;
        }
          public async Task<bool> CreateTransactionHistory(Guid userId,double amount,string transactionDescription,string typeOfTransction) 
          {
                var newTransactionHistory = new TransactionHistory();
                newTransactionHistory.Id = Guid.NewGuid();
                newTransactionHistory.UserId = userId;
                newTransactionHistory.Date = DateTime.Now;
                newTransactionHistory.Amount = amount;
                newTransactionHistory.TransactionDescription = transactionDescription;
                newTransactionHistory.TypeofTransaction = typeOfTransction;

                _unitOfWork.GetRepository<TransactionHistory>().Insert(newTransactionHistory);
                await _unitOfWork.SaveChangesAsync();

                return true;
          }


         public async Task<List<TransactionHistory>> GetTransactionHistoryByUserId(Guid userId) 
         {
                var transactionHistory = _unitOfWork.GetRepository<TransactionHistory>().GetAll().Where(x => x.UserId == userId).ToList();
                return transactionHistory;
         }

        public async Task<List<TransactionHistory>> GetTodayCreditTransactionHistoryByUserId(Guid userId)
        {
             var transactionHistory = _unitOfWork.GetRepository<TransactionHistory>().GetAll().Where(x => x.UserId == userId && x.TypeofTransaction == "Credit" && x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).ToList();
            
                return transactionHistory;
        }

    }
}
