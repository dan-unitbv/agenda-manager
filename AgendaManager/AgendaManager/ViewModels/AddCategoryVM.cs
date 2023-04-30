using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

using GalaSoft.MvvmLight.Messaging;

using AgendaManager.Commands;

namespace AgendaManager.ViewModels
{
    public class AddCategoryVM : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            set
            {
                name = value;
                NotifyPropertyChanged(nameof(Name));
            }

            get { return name; }
        }

        public void ConfirmAction(object param)
        {
            Messenger.Default.Send(Name);

            if (param is Window window)
            {
                window.Close();
            }
        }

        private ICommand confirmActionCommand;

        public ICommand ConfirmActionCommand
        {
            get
            {
                if (confirmActionCommand == null)
                {
                    confirmActionCommand = new RelayCommand(ConfirmAction);
                }

                return confirmActionCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}