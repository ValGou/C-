using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Classes.Models;
using DataInput.ViewModels.Notify;
using DataInput.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utilities.Database;

namespace DataInput.ViewModels
{
    public class DataViewModel
    {
        private DataView dataView;

        public User CurrentUser { get; set; }
        public Data Data { get; set; }

        public ObservableCollection<Data> Datas { get; set; }

        public DataViewModel(User currentUser)
        {
            dataView = new DataView();
            Data = new Data();
            CurrentUser = currentUser;
            dataView.DataContext = this;
            dataView.DataList.ItemsSource = Datas;
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Content = dataView;
            Datas = new ObservableCollection<Data>(LoadData(CurrentUser));
            dataView.InsertJsonBtn.Click += InsertJsontn_OnClick;
        }

        public IEnumerable<Data> LoadData(User currentUser)
        {
            Manager<Data> dataManager = new Manager<Data>(ConnectionResource.LocalMySQL);
            return dataManager.Get().Result.Where(d => d.UserId == currentUser.Id);
        }

        private async void InsertJsontn_OnClick(object sender, RoutedEventArgs e)
        {
            Data.UserId = CurrentUser.Id;
            if (IsValidJson(Data.DataValue))
            {
                await new Manager<Data>(ConnectionResource.LocalMySQL).Insert(Data);
                Datas.Add(Data);
            }
        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
