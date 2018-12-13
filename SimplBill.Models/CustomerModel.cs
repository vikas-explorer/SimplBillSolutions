using System;
using System.Collections.Generic;
using System.Text;

namespace SimplBill.Models
{
    public class CustomerModel
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<BillDetailModel> BillDetails { get; set; }
    }
}
