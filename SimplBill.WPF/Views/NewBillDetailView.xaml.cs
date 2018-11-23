using SimplBill.Models;
using SimplBill.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimplBill.WPF.Views
{
    /// <summary>
    /// Interaction logic for NewBillDetailView.xaml
    /// </summary>
    public partial class NewBillDetailView : UserControl
    {
        NewBillDetailViewModel newBillDetailViewModel;

        public NewBillDetailView()
        {
            InitializeComponent();
            
            newBillDetailViewModel = new NewBillDetailViewModel();
            SellingProductsDataGrid.ItemsSource = new ListCollectionView(newBillDetailViewModel.SellingProducs);
            PurchasingProductsDataGrid.ItemsSource = new ListCollectionView(newBillDetailViewModel.PurchasingProducts);
            SellingProductsDataGrid.AddingNewItem += SellingProductsDataGrid_AddingNewItem;
            SellingProductsDataGrid.InitializingNewItem += SellingProductsDataGrid_InitializingNewItem;

            this.DataContext = newBillDetailViewModel;
        }

        private void SellingProductsDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            ProductDetailModel productDetailModel = e.NewItem as ProductDetailModel;
            productDetailModel.MeasureType = MeasureType.InKgs;

        }

        private void SellingProductsDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new ProductDetailModel();
        }

    }
}
