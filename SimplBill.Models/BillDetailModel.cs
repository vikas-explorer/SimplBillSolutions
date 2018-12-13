using System;
using System.Collections.Generic;

namespace SimplBill.Models
{
    public class BillDetailModel
    {
        public uint Id { get; set; }

        public ICollection<ProductDetailModel> ProductDetails { get; set; }

        public CustomerModel Customer { get; set; }

        public int TotalAmount { get; set; }
        
        public DateTime BillDate { get; set; }

        public BillDetailType BillDetailType { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.InverseProperty("OwningBill")]
        public ICollection<BillPaymentDetailModel> BillPaymentDetails { get; set; }
    }

    public enum BillDetailType
    {
        Sell = 1,

        Purchase = 2
    }
}
