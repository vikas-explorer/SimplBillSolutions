using SimplBill.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplBill.Models
{
    public class ProductDetailModel : BaseModel
    {
        public uint Id { get; private set; }

        private decimal quantity;

        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(Amount));
            }
        }


        private decimal costPrice;

        public decimal CostPrice
        {
            get { return costPrice; }
            set
            {
                costPrice = value;
                OnPropertyChanged(nameof(CostPrice));
                OnPropertyChanged(nameof(Amount));
            }
        }
        private string name = "Aluminium";

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        public uint? ProductInfoId { get; set; }

        private MeasureType measureType;

        public MeasureType MeasureType
        {
            get { return measureType; }
            set
            {
                measureType = value;
                OnPropertyChanged(nameof(MeasureType));
            }
        }

        public BillDetailModel BillDetail { get; set; }

        public decimal Amount { get { return CostPrice * Quantity; } }
    }

    public enum MeasureType
    {
        InKgs = 1,

        InNumbers = 2
    }
}
