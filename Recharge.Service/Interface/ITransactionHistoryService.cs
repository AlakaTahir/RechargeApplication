using Recharge.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recharge.Service.Interface
{
    public interface ITransactionHistoryService
    {
        Task<bool> CreateTransactionHistory(Guid userId, double amount, string transactionDescription, string typeOfTransction);
        Task<List<TransactionHistory>> GetTransactionHistoryByUserId(Guid userId);
        Task<List<TransactionHistory>> GetTodayCreditTransactionHistoryByUserId(Guid userId);
    }
}
