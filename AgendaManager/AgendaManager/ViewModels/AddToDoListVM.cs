using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Messaging;

using AgendaManager.Models;
using AgendaManager.Commands;

namespace AgendaManager.ViewModels
{
    public class AddToDoListVM : INotifyPropertyChanged
    {

        public AddToDoListVM()
        {
            SymbolsList = new List<string>()
            {
                "C:\\Users\\WIN10\\source\\repos\\AgendaManager\\AgendaManager\\Assets\\Symbols\\edit.png",
                "C:\\Users\\WIN10\\source\\repos\\AgendaManager\\AgendaManager\\Assets\\Symbols\\star.png",
                "C:\\Users\\WIN10\\source\\repos\\AgendaManager\\AgendaManager\\Assets\\Symbols\\network.png",
                "C:\\Users\\WIN10\\source\\repos\\AgendaManager\\AgendaManager\\Assets\\Symbols\\diamond.png",
                "C:\\Users\\WIN10\\source\\repos\\AgendaManager\\AgendaManager\\Assets\\Symbols\\eyeglasses.png",
                "C:\\Users\\WIN10\\source\\repos\\AgendaManager\\AgendaManager\\Assets\\Symbols\\attachment.png",
                "C:\\Users\\WIN10\\source\\repos\\AgendaManager\\AgendaManager\\Assets\\Symbols\\paper-plane.png",
            };

            SymbolIndex = 0;

            ImagePath = SymbolsList[SymbolIndex];

            Messenger.Default.Register<ObservableCollection<TDL>>(this, ReceiveSet);
        }

        HashSet<string> ToDoListsNamesSet { get; set; } = new HashSet<string>();

        private void ReceiveSet(ObservableCollection<TDL> obj)
        {
            foreach (TDL item in obj)
            {
                ToDoListsNamesSet.Add(item.Name);

                if (item.SubToDoLists != null)
                {
                    ReceiveSet(item.SubToDoLists);
                }
            }
        }

        public int SymbolIndex;

        public List<string> SymbolsList { get; set; }

        public TDL ToDoList { get; set; }

        private string name;
        private string imagePath;

        public string Name
        {
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }

            get { return name; }
        }

        public string ImagePath
        {
            set
            {
                imagePath = value;
                NotifyPropertyChanged("ImagePath");
            }

            get { return imagePath; }
        }

        public void PreviousSymbol(object param)
        {
            SymbolIndex--;

            if (SymbolIndex < 0)
            {
                SymbolIndex = SymbolsList.Count - 1;
            }

            ImagePath = SymbolsList[SymbolIndex];
        }

        public void NextSymbol(object param)
        {
            SymbolIndex++;

            if (SymbolIndex >= SymbolsList.Count)
            {
                SymbolIndex = 0;
            }

            ImagePath = SymbolsList[SymbolIndex];
        }

        public void Confirm(object param)
        {
            if (Name == null)
            {
                MessageBox.Show("Please enter a name!", "Error");
                return;
            }

            if (ToDoListsNamesSet.Contains(Name))
            {
                MessageBox.Show("This to do list exists already!", "Error");
                return;
            }

            ToDoList = new TDL(Name, ImagePath);

            Messenger.Default.Send(ToDoList);

            if (param is Window window)
            {
                window.Close();
            }
        }

        private ICommand previousSymbolCommand;

        public ICommand PreviousSymbolCommand
        {
            set
            {
                previousSymbolCommand = value;
            }

            get
            {
                if (previousSymbolCommand == null)
                {
                    previousSymbolCommand = new RelayCommand(PreviousSymbol);
                }
                return previousSymbolCommand;
            }
        }

        private ICommand nextSymbolCommand;

        public ICommand NextSymbolCommand
        {
            set
            {
                nextSymbolCommand = value;
            }

            get
            {
                if (nextSymbolCommand == null)
                {
                    nextSymbolCommand = new RelayCommand(NextSymbol);
                }
                return nextSymbolCommand;
            }
        }

        private ICommand confirmCommand;

        public ICommand ConfirmCommand
        {
            set
            {
                confirmCommand = value;
            }

            get
            {
                if (confirmCommand == null)
                {
                    confirmCommand = new RelayCommand(Confirm);
                }
                return confirmCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}