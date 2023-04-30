using System;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace AgendaManager.Models
{
    [Serializable]
    public class Database
    {
        public static string Name { get; set; }

        public static string Path { get; set; }

        [XmlArray]
        public ObservableCollection<TDL> ToDoLists { get; set; }

        [XmlArray]
        public ObservableCollection<string> Categories { get; set; }
    }
}