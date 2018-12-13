using SimplBill.WPF.Printing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplBill.WPF.PrintContentProviders
{
    public class BillPrintContentProvider : BasePrintContentProvider
    {
        private readonly string BlankString = "                                                  ";
        private CustomerBillPrintData CustomerBillPrintData;

        public BillPrintContentProvider(CustomerBillPrintData customerBillPrintData)
        {
            CustomerBillPrintData = customerBillPrintData;
        }

        public override string GetContent()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"       Name : {CustomerBillPrintData.CustomerName}   ");

            stringBuilder.AppendLine($"       {CustomerBillPrintData.BillDate.ToString()}  ");

            stringBuilder.AppendLine($"       Sold Items  ");

            if (CustomerBillPrintData.SoldItemsBillData != null)
            {
                stringBuilder.AppendLine("_________________________________________________");
                stringBuilder.AppendLine("Quantity | Product Name       | Cost   | Amount |");
                stringBuilder.AppendLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯");

                foreach (ProductPrintData prdouct in CustomerBillPrintData.SoldItemsBillData.ProductPrintDatas)
                {
                    StringBuilder productStringBuilder = new StringBuilder(BlankString);

                    string quanCal = prdouct.IsInKgs ? "Kg" : "N";
                    productStringBuilder.Insert(0, $"{prdouct.Quantity}{quanCal}");

                    productStringBuilder.Insert(9, $"| {prdouct.ProductName}");

                    productStringBuilder.Insert(30, $"| {prdouct.Cost}");

                    productStringBuilder.Insert(39, $"| {prdouct.Amount}");

                    productStringBuilder.Insert(48, "|");

                    stringBuilder.AppendLine(productStringBuilder.ToString());
                }
                stringBuilder.AppendLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯");
                StringBuilder totalCostBuilder = new StringBuilder(BlankString);
                totalCostBuilder.Insert(30, $"| Total Cost ");
                totalCostBuilder.Insert(39, $"| {CustomerBillPrintData.SoldItemsBillData.TotalCost}");

                stringBuilder.AppendLine(totalCostBuilder.ToString());
            }



            stringBuilder.AppendLine($"       Purchased Items  ");

            if (CustomerBillPrintData.PurchasedItemsBillData != null)
            {
                stringBuilder.AppendLine("_________________________________________________");
                stringBuilder.AppendLine("Quantity | Product Name       | Cost   | Amount |");
                stringBuilder.AppendLine("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯");
                foreach (ProductPrintData prdouct in CustomerBillPrintData.PurchasedItemsBillData.ProductPrintDatas)
                {
                    StringBuilder productStringBuilder = new StringBuilder(BlankString);

                    string quanCal = prdouct.IsInKgs ? "Kg" : "N";
                    productStringBuilder.Insert(0, $"{prdouct.Quantity}{quanCal}");

                    productStringBuilder.Insert(9, $"| {prdouct.ProductName}");

                    productStringBuilder.Insert(30, $"| {prdouct.Cost}");

                    productStringBuilder.Insert(39, $"| {prdouct.Amount}");

                    productStringBuilder.Insert(48, "|");

                    stringBuilder.AppendLine(productStringBuilder.ToString());
                }

                StringBuilder totalCostBuilder = new StringBuilder(BlankString);
                totalCostBuilder.Insert(30, $"| Total Cost ");
                totalCostBuilder.Insert(39, $"| {CustomerBillPrintData.PurchasedItemsBillData.TotalCost}");

                stringBuilder.AppendLine(totalCostBuilder.ToString());
            }


            return stringBuilder.ToString();
        }
    }
}