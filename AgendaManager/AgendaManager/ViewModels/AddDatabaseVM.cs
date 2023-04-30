using System.IO;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

using AgendaManager.Models;
using AgendaManager.Commands;

namespace AgendaManager.ViewModels
{
    public class AddDatabaseVM : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public void CreateDatabase(object parameter)
        {
            Database.Path = "C:\\Users\\WIN10\\source\\repos\\AgendaManager\\AgendaManager\\Assets\\Databases" + "\\" + Name + ".xml";
            Database.Name = Name;

            if (!File.Exists(Database.Path))
            {
                if (parameter is Window window)
                {
                    window.Close();
                }
                else
                {
                    MessageBox.Show("The database exists already!", "Error");
                }
            }
        }

        private ICommand createDatabaseCommand;

        public ICommand CreateDatabaseCommand
        {
            get
            {
                if (createDatabaseCommand == null)
                {
                    createDatabaseCommand = new RelayCommand(CreateDatabase);
                }

                return createDatabaseCommand;
            }
        }
        public AddDatabaseVM()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}