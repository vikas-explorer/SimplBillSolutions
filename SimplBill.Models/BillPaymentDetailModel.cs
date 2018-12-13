using System;
using System.Collections.Generic;
using System.Text;

namespace SimplBill.Models
{
    public class BillPaymentDetailModel
    {
        public uint Id { get; set; }

        public decimal Cash { get; set; }

        public BillPaymentType PaymentType { get; set; }

        public BillDetailModel SettlingBill { get; set; }

        public uint? SettlingBillId { get; set; }

        public BillDetailModel OwningBill { get; set; }

        public uint OwningBillId { get; set; }
    }

    public enum BillPaymentType
    {
        Cash = 1,

        Credit = 2,

        BillSettlement = 4,

        SquaredOff = 8
    }

}
