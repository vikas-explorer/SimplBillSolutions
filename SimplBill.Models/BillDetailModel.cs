using System;
using System.Collections.Generic;

namespace SimplBill.Models
{
    public class BillDetailModel
    {
        public List<ProductDetailModel> ProductDetails { get; set; }

        public CustomerModel Customer { get; set; }

        public int TotalAmount { get; set; }
        

    }
}
