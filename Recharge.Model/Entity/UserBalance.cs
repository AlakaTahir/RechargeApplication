using System;
using System.Collections.Generic;
using System.Text;

namespace Recharge.Model.Entity
{
    public class UserBalance
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public double Balance { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
