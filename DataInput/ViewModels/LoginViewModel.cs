using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Classes.Models;
using DataInput.Views;
using Ultilities.Database;

namespace DataInput.ViewModels
{
    public class LoginViewModel
    {
        private LoginView loginView;

        public LoginViewModel(Frame contentFrame)
        {
            loginView = new LoginView();
            loginView.DataContext = this;
            loginView.LoginBtn.Click += LoginBtn_OnClick;
            contentFrame.Content = loginView;
        }

        private void LoginBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string login = loginView.Login.Text;
            string password = loginView.Password.Text;

            UserManager userManager = new UserManager();
            User user = userManager.GetByLogin(login, password);

            if (user.Id != 0)
            {
                new DataViewModel(user);
            }
        }
    }
}
