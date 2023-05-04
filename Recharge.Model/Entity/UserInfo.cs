using System;
using System.Collections.Generic;
using System.Text;

namespace Recharge.Model.Entity
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
