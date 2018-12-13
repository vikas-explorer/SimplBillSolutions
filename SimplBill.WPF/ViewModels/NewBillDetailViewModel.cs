using SimplBill.Models;
using SimplBill.WPF.Core;
using SimplBill.WPF.PrintContentProviders;
using SimplBill.WPF.Printing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SimplBill.WPF.ViewModels
{
    public class NewBillDetailViewModel : BaseViewModel
    {
        public ObservableCollection<ProductDetailModel> purchasingProducts;

        private ObservableCollection<ProductDetailModel> sellingProducts;

        private string customerName;

        private List<MeasureType> measureTypes;



        public ObservableCollection<ProductDetailModel> SellingProducs

        {
            get { return sellingProducts; }
            set
            {
                sellingProducts = value;
                sellingProducts.CollectionChanged += SellingProducts_CollectionChanged;
                OnPropertyChanged(nameof(SellingProducs));
            }
        }

        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        public decimal TotalSellingKgs
        {
            get { return SellingProducs.Where(product => product.MeasureType == MeasureType.InKgs).Sum(product => product.Quantity); }
        }

        public decimal TotalPurchasingKgs
        {
            get { return PurchasingProducts.Where(product => product.MeasureType == MeasureType.InKgs).Sum(product => product.Quantity); }
        }

        public decimal TotalSellingProductCost
        {
            get { return SellingProducs.Sum(product => product.Amount); }
        }

        public decimal TotalPurchasingPorductCost
        {
            get { return PurchasingProducts.Sum(product => product.Amount); }
        }

        public decimal TotalCost
        {
            get { return TotalSellingProductCost - TotalPurchasingPorductCost; }
        }

        public ReadOnlyCollection<MeasureType> MeasureTypes
        {
            get { if (measureTypes == null) { measureTypes = new List<MeasureType>() { MeasureType.InKgs, MeasureType.InNumbers }; } return measureTypes.AsReadOnly(); }
        }

        private decimal amountPaid = 0;

        public decimal AmountPaid
        {
            get { return amountPaid; }
            set
            {
                amountPaid = value;
                CalculateCreditAmount();
                OnPropertyChanged(nameof(AmountPaid));
            }
        }

        private void CalculateCreditAmount()
        {
            decimal remainingAmount = TotalCost - AmountPaid;
            if (remainingAmount > 1 || remainingAmount < -1)
            {
                CreditEnabled = true;
                CreditAmount = remainingAmount;
            }
            else
            {
                CreditEnabled = false;
                CreditAmount = 0;
            }
        }

        private bool creditEnabled = false;

        public bool CreditEnabled
        {
            get { return creditEnabled; }
            set
            {
                creditEnabled = value;
                OnPropertyChanged(nameof(CreditEnabled));
            }
        }

        public bool IsCredited { get; set; } = true;

        private decimal creditAmount = 0;

        public decimal CreditAmount
        {
            get { return creditAmount; }
            set
            {
                creditAmount = value;
                OnPropertyChanged(nameof(CreditAmount));
            }
        }


        public ObservableCollection<ProductInfoModel> ProductInfoModels
        {
            get { return new ObservableCollection<ProductInfoModel>() { new ProductInfoModel() { Name = "Aluminium" }, new ProductInfoModel() { Name = "Copper" } }; }
        }

        public ObservableCollection<CustomerModel> CustomerModels
        {
            get { return new ObservableCollection<CustomerModel> { new CustomerModel() { Name = "Vikas" }, new CustomerModel() { Name = "Lalit" }, new CustomerModel() { Name = "Himmat" } }; }
        }


        public ObservableCollection<ProductDetailModel> PurchasingProducts
        {
            get { return purchasingProducts; }
            set
            {
                purchasingProducts = value;
                purchasingProducts.CollectionChanged += PurchasingProducts_CollectionChanged;
                OnPropertyChanged(nameof(purchasingProducts));
            }
        }


        public NewBillDetailViewModel()
        {
            SellingProducs = new ObservableCollection<ProductDetailModel>();
            PurchasingProducts = new ObservableCollection<ProductDetailModel>();
        }

        private void PurchasingProducts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    ((ProductDetailModel)item).PropertyChanged -= ProductDetailModel_PropertyChanged;
                    ((ProductDetailModel)item).PropertyChanged += ProductDetailModel_PropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    ((ProductDetailModel)item).PropertyChanged -= ProductDetailModel_PropertyChanged;
                }
            }
            OnPropertyChanged(nameof(TotalPurchasingKgs));
            OnPropertyChanged(nameof(TotalPurchasingPorductCost));
            OnPropertyChanged(nameof(TotalCost));
            CalculateCreditAmount();
        }

        private void SellingProducts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    ((ProductDetailModel)item).PropertyChanged -= ProductDetailModel_PropertyChanged;
                    ((ProductDetailModel)item).PropertyChanged += ProductDetailModel_PropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    ((ProductDetailModel)item).PropertyChanged -= ProductDetailModel_PropertyChanged;
                }
            }
            OnPropertyChanged(nameof(TotalSellingKgs));
            OnPropertyChanged(nameof(TotalSellingProductCost));
            OnPropertyChanged(nameof(TotalCost));
            CalculateCreditAmount();
        }

        private void ProductDetailModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseCostPropertyChange();
        }


        private void RaiseCostPropertyChange()
        {
            OnPropertyChanged(nameof(TotalSellingProductCost));
            OnPropertyChanged(nameof(TotalSellingKgs));
            OnPropertyChanged(nameof(TotalPurchasingPorductCost));
            OnPropertyChanged(nameof(TotalPurchasingKgs));
            OnPropertyChanged(nameof(TotalCost));
            CalculateCreditAmount();
        }

        private CustomerBillPrintData GetCustomerBillPrintData()
        {
            CustomerBillPrintData customerBillPrintData = new CustomerBillPrintData()
            {
                CustomerName = CustomerName,

                BillDate = DateTime.Now,

                PurchasedItemsBillData = GetPurchasedBillPrintData(),

                SoldItemsBillData = GetSoldBillPrintData()
            };

            return customerBillPrintData;
        }

        private BillPrintData GetPurchasedBillPrintData()
        {
            BillPrintData billPrintData = new BillPrintData()
            {
                ProductPrintDatas = new List<ProductPrintData>(),
                TotalCost = TotalPurchasingPorductCost
            };
            foreach (ProductDetailModel product in PurchasingProducts)
            {
                billPrintData.ProductPrintDatas.Add(new ProductPrintData() { Quantity = product.Quantity, Amount = product.Amount, Cost = product.CostPrice, ProductName = product.Name, IsInKgs = product.MeasureType == MeasureType.InKgs });
            }
            return billPrintData;
        }

        private BillPrintData GetSoldBillPrintData()
        {
            BillPrintData billPrintData = new BillPrintData()
            {
                ProductPrintDatas = new List<ProductPrintData>(),
                TotalCost = TotalSellingProductCost
            };
            foreach (ProductDetailModel product in SellingProducs)
            {
                billPrintData.ProductPrintDatas.Add(new ProductPrintData() { Quantity = product.Quantity, Amount = product.Amount, Cost = product.CostPrice, ProductName = product.Name, IsInKgs = product.MeasureType == MeasureType.InKgs });
            }
            return billPrintData;
        }

        public override void Print(object parameter)
        {
            ReceiptPrintController receiptPrintController = new ReceiptPrintController(new BillPrintContentProvider(GetCustomerBillPrintData()));
            receiptPrintController.Print();
        }
    }
}
