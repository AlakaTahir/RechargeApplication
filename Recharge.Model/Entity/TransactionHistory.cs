using System;
using System.Collections.Generic;
using System.Text;

namespace Recharge.Model.Entity
{
    public class TransactionHistory
    {
     public Guid Id { get; set; }
     public Guid UserId { get; set; }   
     public string TransactionDescription { get; set; }
     public DateTime Date { get; set; }
     public double Amount { get; set; }
     public string TypeofTransaction { get; set; }
     public bool IsSuccessful { get; set; }
    }
}
