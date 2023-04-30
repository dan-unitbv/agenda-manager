using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Messaging;

using AgendaManager.Models;
using AgendaManager.Commands;

namespace AgendaManager.ViewModels
{
    public class ChangeRootVM : INotifyPropertyChanged
    {
        public ChangeRootVM()
        {
            ToDoListItems = new ObservableCollection<TDL>();

            Messenger.Default.Register<string>(this, ReceiveCurrentToDoList);
            Messenger.Default.Register<ObservableCollection<TDL>>(this, ReceiveToDoList);
        }

        private bool isRoot = true;

        public bool IsRoot
        {
            set
            {
                isRoot = value;
                NotifyPropertyChanged("IsRoot");
            }

            get { return isRoot; }
        }

        public string ToDoListName { get; set; }
        public TDL SelectedRoot { get; set; }
        public ObservableCollection<TDL> ToDoListItems { get; set; }

        public void Confirm(object param)
        {
            Messenger.Default.Send(SelectedRoot);

            if (param is Window window)
            {
                window.Close();
            }
        }

        private ICommand confirmCommand;

        public ICommand ConfirmCommand
        {
            get
            {
                if (confirmCommand == null)
                {
                    confirmCommand = new RelayCommand(Confirm);
                }

                return confirmCommand;
            }
        }

        public void ReceiveCurrentToDoList(string toDoList)
        {
            ToDoListName = toDoList;
            NotifyPropertyChanged("ToDoListName");
        }

        public void ReceiveToDoList(ObservableCollection<TDL> TDLCollection)
        {
            GetAllToDoLists(TDLCollection);
            NotifyPropertyChanged("TDLItems");
        }

        public void GetAllToDoLists(ObservableCollection<TDL> TDLCollection)
        {
            foreach (TDL toDoList in TDLCollection)
            {
                if (toDoList.Name != ToDoListName)
                {
                    ToDoListItems.Add(toDoList);
                }

                GetAllToDoLists(toDoList.SubToDoLists);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
