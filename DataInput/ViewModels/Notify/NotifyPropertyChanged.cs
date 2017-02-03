using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataInput.ViewModels.Notify
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
