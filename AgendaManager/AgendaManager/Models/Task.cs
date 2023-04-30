using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace AgendaManager.Models
{
    [Serializable]
    public class Task : INotifyPropertyChanged
    {
        public Task() { }

        public Task(string name, string description, string priority, string category, DateTime deadline)
        {
            Name = name;
            Description = description;
            TaskPriority = priority;
            TaskCategory = category;
            Deadline = deadline;
        }

        private string name;
        private string description;
        private bool taskStatus;
        private string taskPath;
        private string taskPriority;
        private string taskCategory;
        private DateTime deadline;
        private DateTime completionDate;

        [XmlAttribute]
        public string Name
        {
            set
            {
                name = value;
                NotifyPropertyChanged(nameof(Name));
            }

            get { return name; }
        }

        [XmlAttribute]
        public string Description
        {
            set
            {
                description = value;
                NotifyPropertyChanged(nameof(Description));
            }

            get { return description; }
        }

        [XmlAttribute]
        public bool TaskStatus
        {
            set
            {
                taskStatus = value;
                completionDate = DateTime.Now;
                NotifyPropertyChanged(nameof(TaskStatus));
            }

            get { return taskStatus; }
        }

        public string TaskPath
        {
            set
            {
                taskPath = value;
                NotifyPropertyChanged(nameof(TaskPath));
            }

            get { return taskPath; }
        }

        [XmlAttribute]
        public string TaskPriority
        {
            set
            {
                taskPriority = value;
                NotifyPropertyChanged(nameof(TaskPriority));
            }

            get { return taskPriority; }
        }

        [XmlAttribute]
        public string TaskCategory
        {
            set
            {
                taskCategory = value;
                NotifyPropertyChanged(nameof(TaskCategory));
            }

            get { return taskCategory; }
        }

        [XmlAttribute]
        public DateTime Deadline
        {
            set
            {
                deadline = value;
                NotifyPropertyChanged(nameof(Deadline));
            }

            get { return deadline; }
        }

        [XmlAttribute]
        public DateTime CompletionDate
        {
            set
            {
                completionDate = value;
                NotifyPropertyChanged(nameof(CompletionDate));
            }

            get { return completionDate; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}