﻿using SimplBill.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace SimplBill.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer unityContainer = new UnityContainer();

            unityContainer.RegisterSingleton(typeof(BillingContext));

            unityContainer.RegisterType(typeof(MainViewModel));

            unityContainer.RegisterType(typeof(MainView));

            unityContainer.RegisterType(typeof(MainWindow));
            MainWindow window = unityContainer.Resolve<MainWindow>();
            window.Show();
        }
    }
}
