﻿using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.ViewModel;
using TradeHubGui.Views;


namespace TradeHubGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new BaseViewModel();
            AppDomain.CurrentDomain.UnhandledException +=
                 new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MetroWindow mw = new SettingsWindow();
            mw.ShowTitleBar = true;
            mw.ShowIconOnTitleBar = true;
            mw.ShowInTaskbar = false;
            mw.Owner = this;
            mw.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            mw.ShowDialog();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("About", "Something will be displayed here!", MessageDialogStyle.Affirmative);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            (passwordBox.DataContext as ProviderCredential).CredentialValue = passwordBox.Password;
        }

        private void PasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.Password = (passwordBox.DataContext as ProviderCredential).CredentialValue;
        }

        private void OnApplicationClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EventSystem.Publish<string>("Close");

            // Close all open windows
            foreach (Window window in Application.Current.Windows)
            {
                // Because Main Window is already in closing state
                if (window != Application.Current.MainWindow)
                {
                    window.Close();
                }
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(ex.Message, "Uncaught Thread Exception",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
