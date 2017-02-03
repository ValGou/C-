using System.Linq;
using System.Windows;
using System.Windows.Input;
using Classes.Models;
using DataInput.Helpers;
using DataInput.Views;

namespace DataInput.ViewModels
{
    public class LayoutViewModel
    {
        private LayoutView layoutView;
        
        public User LayoutUser { get; set; }
        public ICommand AddUserCommand { get; private set; }

        public LayoutViewModel()
        {
            layoutView = new LayoutView();
            layoutView.DataContext = this;
            LayoutUser = new User();
            new LoginViewModel(layoutView.ContentFrame);
            AddUserCommand = new RelayCommand(ExecAddUser, CanAddUser);

            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Content = layoutView;
        }

        private bool CanAddUser(object obj)
        {
            return true;
        }

        private void ExecAddUser(object obj)
        {
            new CreateUserViewModel(layoutView.ContentFrame);
        }
    }
}
