using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplBill.WPF.PrintContentProviders
{
    public class CustomerBillPrintData
    {
        public DateTime BillDate { get; set; }

        public string CustomerName { get; set; }

        public BillPrintData SoldItemsBillData { get; set; }

        public BillPrintData PurchasedItemsBillData { get; set; }
        
    }

    public class BillPrintData
    {
        public List<ProductPrintData> ProductPrintDatas { get; set; }

        public decimal TotalCost { get; set; }
    }

    public class ProductPrintData
    {
        public decimal Quantity { get; set; }

        public string ProductName { get; set; }

        public string ProductShortName { get; set; }

        public decimal Cost { get; set; }

        public decimal Amount { get; set; }

        public bool IsInKgs { get; set; }
    }
}
