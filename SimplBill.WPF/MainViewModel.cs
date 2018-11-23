using SimplBill.WPF.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplBill.WPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand ChangeViewCommand { get; set; }

        public MainViewModel()
        {
            ChangeViewCommand = new RelayCommand(ChangeView);
        }

        public void ChangeView(object viewName)
        {
            if(viewName.ToString() == "NewBill")
            {
                MainContentView = new Views.NewBillDetailView();
            }
        }

        private object mainContetView;

        public object MainContentView
        {
            get { return mainContetView; }
            set { mainContetView = value; OnPropertyChanged("MainContentView"); }
        }


        ///public object MainContentView { get

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
