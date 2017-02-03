using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Classes.Models;
using DataInput.Views;
using Ultilities.Database;
using Utilities.Database;

namespace DataInput.ViewModels
{
    public class CreateUserViewModel
    {
        private CreateUserView createUserView;

        public ObservableCollection<User> Users { get; set; }
        public User User { get; set; }

        public CreateUserViewModel(Frame contentFrame)
        {
            createUserView = new CreateUserView();
            createUserView.DataContext = this;
            User = new User();
            createUserView.CreateUserBtn.Click += CreateUserBtn_OnClick;
            contentFrame.Content = createUserView;
            LoadRoles();
            //LoadUsers();
        }

        private async void LoadRoles()
        {
            List<Role> roles = await new Manager<Role>(ConnectionResource.LocalMySQL).Get() as List<Role>;
            int i = 3;
            foreach (Role role in roles)
            {
                Label roleLabel = new Label();
                roleLabel.Name = role.Name + "_label";
                roleLabel.Content = role.Name;
                roleLabel.FontSize = 14;
                roleLabel.HorizontalAlignment = HorizontalAlignment.Right;
                roleLabel.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(roleLabel, i);
                Grid.SetColumn(roleLabel, 0);
                createUserView.UserGrid.Children.Add(roleLabel);

                CheckBox roleCheckBox = new CheckBox();
                roleCheckBox.Name = role.Name + "_checkbox";
                roleCheckBox.HorizontalAlignment = HorizontalAlignment.Left;
                roleCheckBox.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(roleCheckBox, i);
                Grid.SetColumn(roleCheckBox, 1);
                createUserView.UserGrid.Children.Add(roleCheckBox);

                i++;
            }
        }

        //public async void LoadUsers()
        //{
        //    //Users = await new Manager<User>(ConnectionResource.LocalMySQL).Get();
        //}

        private async void CreateUserBtn_OnClick(object sender, RoutedEventArgs e)
        {
            User.Login = string.Format($"{User.Firstname}.{User.Lastname}");
            Users.Add(User);
            await new UserManager().Insert(User);
        }
    }
}
