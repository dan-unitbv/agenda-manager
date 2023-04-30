using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace AgendaManager.Models
{
    [Serializable]
    public class TDL : INotifyPropertyChanged
    {
        public TDL() { }

        public TDL(string name, string imagePath)
        {
            Name = name;
            ImagePath = imagePath;
            SubToDoLists = new ObservableCollection<TDL>();
            Tasks = new ObservableCollection<Task>();
        }

        private string name;
        private string imagePath;
        private ObservableCollection<TDL> subToDoLists;
        private ObservableCollection<Task> tasks;

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
        public string ImagePath
        {
            set
            {
                imagePath = value;
                NotifyPropertyChanged(nameof(ImagePath));
            }

            get { return imagePath; }
        }

        [XmlArray]
        public ObservableCollection<TDL> SubToDoLists
        {
            set
            {
                subToDoLists = value;
            }

            get { return subToDoLists; }
        }

        [XmlArray]
        public ObservableCollection<Task> Tasks
        {
            set
            {
                tasks = value;
            }

            get { return tasks; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}