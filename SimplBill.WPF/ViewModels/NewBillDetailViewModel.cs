using SimplBill.Models;
using SimplBill.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplBill.WPF.ViewModels
{
    public class NewBillDetailViewModel : BaseViewModel
    {
        public ObservableCollection<ProductDetailModel> purchasingProducts;

        private ObservableCollection<ProductDetailModel> sellingProducts;

        private string customerName;

        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        


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
        }

        private void ProductDetailModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaiseCostPropertyChange();
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

        public RelayCommand AddProductDetailCommand { get; set; }

        public NewBillDetailViewModel()
            : base()
        {
            SellingProducs = new ObservableCollection<ProductDetailModel>();
            PurchasingProducts = new ObservableCollection<ProductDetailModel>();
        }

        private List<MeasureType> measureTypes;

        public ReadOnlyCollection<MeasureType> MeasureTypes
        {
            get { if (measureTypes == null) { measureTypes = new List<MeasureType>() { MeasureType.InKgs, MeasureType.InNumbers }; } return measureTypes.AsReadOnly(); }
        }


        public ObservableCollection<ProductInfoModel> ProductInfoModels
        {
            get { return new ObservableCollection<ProductInfoModel>() { new ProductInfoModel() { Name = "Aluminium" }, new ProductInfoModel() { Name = "Copper" } }; }
        }

        public ObservableCollection<CustomerModel> CustomerModels
        {
            get { return new ObservableCollection<CustomerModel> { new CustomerModel() { Name = "Vikas" }, new CustomerModel() { Name = "Lalit" }, new CustomerModel() { Name = "Himmat" } }; }
        }

        public override void ConfigureCommnads()
        {
            base.ConfigureCommnads();

            AddProductDetailCommand = new RelayCommand(AddProductDetail);
        }

        public void RaiseCostPropertyChange()
        {
            OnPropertyChanged(nameof(TotalSellingProductCost));
            OnPropertyChanged(nameof(TotalSellingKgs));
            OnPropertyChanged(nameof(TotalPurchasingPorductCost));
            OnPropertyChanged(nameof(TotalPurchasingKgs));
        }

        public void AddProductDetail(object parameter)
        {

        }
    }
}
