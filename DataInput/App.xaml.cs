using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using DataInput.ViewModels;
using DataInput.Views;

namespace DataInput
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            NavigationWindow window = new NavigationWindow();
            window.Content = new LayoutView();
            window.ShowsNavigationUI = false;
            window.Loaded += Window_loaded;
            window.Show();
        }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            new LayoutViewModel();
        }
    }
}
