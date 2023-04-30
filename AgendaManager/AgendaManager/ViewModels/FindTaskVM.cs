using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

using AgendaManager.Models;
using AgendaManager.Commands;

using Task = AgendaManager.Models.Task;

namespace AgendaManager.ViewModels
{
    public class FindTaskVM : INotifyPropertyChanged
    {
        public ObservableCollection<TDL> ToDoLists { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }
        public string TaskToFind { get; set; }
        public DateTime DateToFind { get; set; } = DateTime.Now;
        public bool IsInCurrentView { get; set; }

        private bool isSearchingDeadline;

        public bool IsSearchingDeadline
        {
            set
            {
                isSearchingDeadline = value;
                NotifyPropertyChanged(nameof(IsSearchingDeadline));
            }

            get { return isSearchingDeadline; }
        }

        public FindTaskVM()
        {
            Tasks = new ObservableCollection<Task>();
            ToDoLists = new ObservableCollection<TDL>();
        }

        public FindTaskVM(ObservableCollection<TDL> toDoLists)
        {
            ToDoLists = toDoLists;
        }

        private void Find(object param)
        {
            Tasks.Clear();

            if (!IsSearchingDeadline)
            {
                RecursiveSearch(ToDoLists, "", false);
            }
            else
            {
                RecursiveSearch(ToDoLists, "", true);
            }
        }

        private void Close(object param)
        {
            if (param is Window window)
            {
                window.Close();
            }
        }

        private ICommand findCommand;

        public ICommand FindCommand
        {
            set { findCommand = value; }

            get
            {
                if (findCommand == null)
                {
                    findCommand = new RelayCommand(Find);
                }

                return findCommand;
            }
        }

        private ICommand closeCommand;

        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null)
                {
                    closeCommand = new RelayCommand(Close);
                }

                return closeCommand;
            }
        }

        private void RecursiveSearch(ObservableCollection<TDL> listTDL, string path, bool isDeadline)
        {
            foreach (TDL item in listTDL)
            {
                path += item.Name;

                foreach (Task task in item.Tasks)
                {
                    if (task.Name == TaskToFind)
                    {
                        string finalPath = path;

                        if (path.EndsWith(" -> "))
                        {
                            finalPath = path.Remove(path.Length - 3);
                        }

                        task.TaskPath = finalPath;
                        Tasks.Add(task);
                    }

                    if (task.Deadline == DateToFind && isDeadline)
                    {
                        string finalPath = path;

                        if (path.EndsWith(" -> "))
                        {
                            finalPath = path.Remove(path.Length - 3);
                        }

                        task.TaskPath = finalPath;
                        Tasks.Add(task);
                    }
                }

                if (!IsInCurrentView)
                {
                    path += " -> ";
                    RecursiveSearch(item.SubToDoLists, path, isDeadline);
                    path = path.Remove(path.Length - 4);
                }

                path = path.Remove(path.Length - item.Name.Length);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
