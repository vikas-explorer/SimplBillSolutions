using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplBill.WPF.Core
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public RelayCommand PrintCommnad { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        public BaseViewModel()
        {
            ConfigureCommnads();
        }

        public virtual void ConfigureCommnads()
        {
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel, CanCancel);
            PrintCommnad = new RelayCommand(Print, CanPrint);
        }

        public virtual void Save(object parameter)
        {

        }

        public virtual bool CanSave(object parameter)
        {
            return true;
        }

        public virtual void Cancel(object parameter)
        {

        }

        public virtual bool CanCancel(object parameter)
        {
            return true;
        }

        public virtual void Print(object parameter)
        {
            PrintBill print = new PrintBill();
            print.Print();
        }

        public virtual bool CanPrint(object parameter)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
