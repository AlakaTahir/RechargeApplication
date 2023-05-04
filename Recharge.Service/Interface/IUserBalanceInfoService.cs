using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recharge.Service.Interface
{
    public interface IUserBalanceInfoService
    {
        Task<double> RetrieveUserBalance(Guid userId);
        Task<bool> InitiateBalance(Guid userId);
        Task<bool> TopUpBalance(Guid userId, double amount);
        Task<bool> ChargingCustomer(Guid userId, double amount);



    }
}
