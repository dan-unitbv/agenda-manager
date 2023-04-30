using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Messaging;

using AgendaManager.Models;
using AgendaManager.Commands;

using Task = AgendaManager.Models.Task;

namespace AgendaManager.ViewModels
{
    public class AddTaskVM : INotifyPropertyChanged
    {
        private string name;
        private string description;
        private string taskPriority;
        private string taskCategory;

        public DateTime Deadline { get; set; }
        public ObservableCollection<string> PriorityList { get; set; } = new ObservableCollection<string>()
        {
            "Low", "Medium", "High"
        };

        public ObservableCollection<string> CategoriesList { get; set; } = Category.CategoriesList;

        public string Name
        {
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }

            get { return name; }
        }

        public string Description
        {
            set
            {
                description = value;
                NotifyPropertyChanged("Description");
            }

            get { return description; }
        }

        public string TaskPriority
        {
            set
            {
                taskPriority = value;
                NotifyPropertyChanged("TaskPriority");
            }

            get { return taskPriority; }
        }

        public string TaskCategory
        {
            set
            {
                taskCategory = value;
                NotifyPropertyChanged("TaskCategory");
            }

            get { return taskCategory; }
        }

        public AddTaskVM()
        {
            Name = "";
            Description = "";
            TaskPriority = "";
            TaskCategory = "";
            Deadline = DateTime.Now;
        }

        private void CreateTask(object param)
        {
            Task newTask = new Task(Name, Description, TaskPriority, TaskCategory, Deadline);

            Messenger.Default.Send(newTask);

            if (param is Window window)
            {
                window.Close();
            }
        }

        private ICommand createTaskCommand;

        public ICommand CreateTaskCommand
        {
            set
            {
                createTaskCommand = value;
            }

            get
            {
                if (createTaskCommand == null)
                {
                    createTaskCommand = new RelayCommand(CreateTask);
                }

                return createTaskCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}