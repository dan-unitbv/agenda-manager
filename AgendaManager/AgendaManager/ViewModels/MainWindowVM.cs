using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

using Microsoft.Win32;
using GalaSoft.MvvmLight.Messaging;

using AgendaManager.Models;
using AgendaManager.Commands;

using Task = AgendaManager.Models.Task;
using AgendaManager.Windows;
using System.Threading.Tasks;

namespace AgendaManager.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public Database database { get; set; }

        private ObservableCollection<TDL> itemsToDoList;
        private ObservableCollection<Task> itemsTasks;
        private TDL selectedToDoList;
        public TDL RootToDoList { get; set; }
        public string CurrentFilter { get; set; }

        private Task selectedTask;

        public ObservableCollection<TDL> ItemsToDoList
        {
            set
            {
                itemsToDoList = value;
                NotifyPropertyChanged(nameof(ItemsToDoList));
            }

            get { return itemsToDoList; }
        }

        public ObservableCollection<Task> ItemsTasks
        {
            set
            {
                itemsTasks = value;
                NotifyPropertyChanged(nameof(ItemsTasks));
            }

            get { return itemsTasks; }
        }

        public TDL SelectedToDoList
        {
            set
            {
                if (selectedToDoList != value)
                {
                    selectedToDoList = value;
                    NotifyPropertyChanged(nameof(SelectedToDoList));
                }
            }

            get { return selectedToDoList; }
        }

        private TaskStatus taskStatus;

        public TaskStatus TaskStatus
        {
            get { return taskStatus; }
            set
            {
                taskStatus = value;
                NotifyPropertyChanged("TaskStatus");
            }
        }

        public Task SelectedTask
        {
            set
            {
                selectedTask = value;
                NotifyPropertyChanged("TaskDescription");
            }

            get { return selectedTask; }
        }

        public bool CheckStatus
        {
            set
            {
                selectedTask.TaskStatus = value;
            }

            get
            {
                return (bool)(selectedTask?.TaskStatus);
            }
        }

        private ICommand changeToDoListCommand;

        public ICommand ChangeToDoListCommand
        {
            set
            {
                changeToDoListCommand = value;
            }

            get
            {
                if (changeToDoListCommand == null)
                {
                    changeToDoListCommand = new RelayCommand(UpdateToDoListSelection);
                }

                return changeToDoListCommand;
            }
        }

        private ICommand newDatabaseCreatedCommand;

        public ICommand NewDatabaseCreatedCommand
        {
            set
            {
                newDatabaseCreatedCommand = value;
            }

            get
            {
                if (newDatabaseCreatedCommand == null)
                {
                    newDatabaseCreatedCommand = new RelayCommand(CreateDatabase);
                }

                return newDatabaseCreatedCommand;
            }
        }

        private ICommand databaseSavedCommand;

        public ICommand DatabaseSavedCommand
        {
            set
            {
                databaseSavedCommand = value;
            }

            get
            {
                if (databaseSavedCommand == null)
                {
                    databaseSavedCommand = new RelayCommand(SaveDatabase);
                }

                return databaseSavedCommand;
            }
        }

        private ICommand databaseLoadedCommand;
        public ICommand DatabaseLoadedCommand
        {
            set
            {
                databaseLoadedCommand = value;
            }

            get
            {
                if (databaseLoadedCommand == null)
                {
                    databaseLoadedCommand = new RelayCommand(LoadDatabase);
                }

                return databaseLoadedCommand;
            }
        }

        private ICommand addRootToDoListCommand;

        public ICommand AddRootToDoListCommand
        {
            set
            {
                addRootToDoListCommand = value;
            }

            get
            {
                if (addRootToDoListCommand == null)
                {
                    addRootToDoListCommand = new RelayCommand(AddRootToDoList);
                }

                return addRootToDoListCommand;
            }
        }

        private ICommand addSubToDoListCommand;

        public ICommand AddSubToDoListCommand
        {
            set
            {
                addSubToDoListCommand = value;
            }

            get
            {
                if (addSubToDoListCommand == null)
                {
                    addSubToDoListCommand = new RelayCommand(AddSubToDoList);
                }

                return addSubToDoListCommand;
            }
        }

        private ICommand editToDoListCommand;

        public ICommand EditToDoListCommand
        {
            set
            {
                editToDoListCommand = value;
            }

            get
            {
                if (editToDoListCommand == null)
                {
                    editToDoListCommand = new RelayCommand(EditToDoList);
                }

                return editToDoListCommand;
            }
        }

        private ICommand deleteToDoListCommand;

        public ICommand DeleteToDoListCommand
        {
            set
            {
                deleteToDoListCommand = value;
            }

            get
            {
                if (deleteToDoListCommand == null)
                {
                    deleteToDoListCommand = new RelayCommand(DeleteToDoList);
                }

                return deleteToDoListCommand;
            }
        }

        private ICommand moveUpToDoListCommand;

        public ICommand MoveUpToDoListCommand
        {
            set
            {
                moveUpToDoListCommand = value;
            }

            get
            {
                if (moveUpToDoListCommand == null)
                {
                    moveUpToDoListCommand = new RelayCommand(MoveUpToDoList);
                }

                return moveUpToDoListCommand;
            }
        }

        private ICommand moveDownToDoListCommand;

        public ICommand MoveDownToDoListCommand
        {
            set
            {
                moveDownToDoListCommand = value;
            }

            get
            {
                if (moveDownToDoListCommand == null)
                {
                    moveDownToDoListCommand = new RelayCommand(MoveDownToDoList);
                }

                return moveDownToDoListCommand;
            }
        }

        private ICommand changePathCommand;

        public ICommand ChangePathCommand
        {
            set
            {
                changePathCommand = value;
            }

            get
            {
                if (changePathCommand == null)
                {
                    changePathCommand = new RelayCommand(ChangePath);
                }

                return changePathCommand;
            }
        }

        private ICommand addTaskCommand;

        public ICommand AddTaskCommand
        {
            set
            {
                addTaskCommand = value;
            }

            get
            {
                if (addTaskCommand == null)
                {
                    addTaskCommand = new RelayCommand(AddTask);
                }

                return addTaskCommand;
            }
        }
        private ICommand editTaskCommand;

        public ICommand EditTaskCommand
        {
            set
            {
                editTaskCommand = value;
            }

            get
            {
                if (editTaskCommand == null)
                {
                    editTaskCommand = new RelayCommand(EditTask);
                }

                return editTaskCommand;
            }
        }


        private ICommand deleteTaskCommand;

        public ICommand DeleteTaskCommand
        {
            set
            {
                deleteTaskCommand = value;
            }

            get
            {
                if (deleteTaskCommand == null)
                {
                    deleteTaskCommand = new RelayCommand(DeleteTask);
                }

                return deleteTaskCommand;
            }
        }

        private ICommand setDoneTaskCommand;

        public ICommand SetDoneTaskCommand
        {
            set
            {
                setDoneTaskCommand = value;
            }

            get
            {
                if (setDoneTaskCommand == null)
                {
                    setDoneTaskCommand = new RelayCommand(SetDoneTask);
                }

                return setDoneTaskCommand;
            }
        }

        private ICommand moveUpTaskCommand;

        public ICommand MoveUpTaskCommand
        {
            set
            {
                moveUpTaskCommand = value;
            }

            get
            {
                if (moveUpTaskCommand == null)
                {
                    moveUpTaskCommand = new RelayCommand(MoveUpTask);
                }

                return moveUpTaskCommand;
            }
        }

        private ICommand moveDownTaskCommand;

        public ICommand MoveDownTaskCommand
        {
            set
            {
                moveDownTaskCommand = value;
            }

            get
            {
                if (moveDownTaskCommand == null)
                {
                    moveDownTaskCommand = new RelayCommand(MoveDownTask);
                }

                return moveDownTaskCommand;
            }
        }

        private ICommand manageCategoryCommand;

        public ICommand ManageCategoryCommand
        {
            set
            {
                manageCategoryCommand = value;
            }

            get
            {
                if (manageCategoryCommand == null)
                {
                    manageCategoryCommand = new RelayCommand(ManageCategory);
                }

                return manageCategoryCommand;
            }
        }

        private ICommand filterCommand;

        public ICommand FilterCommand
        {
            set
            {
                filterCommand = value;
            }

            get
            {
                if (filterCommand == null)
                {
                    filterCommand = new RelayCommand(Filter);
                }

                return filterCommand;
            }
        }

        private ICommand findTaskCommand;

        public ICommand FindTaskCommand
        {
            set
            {
                findTaskCommand = value;
            }

            get
            {
                if (findTaskCommand == null)
                {
                    findTaskCommand = new RelayCommand(FindTask);
                }

                return findTaskCommand;
            }
        }

        private ICommand aboutCommand;

        public ICommand AboutCommand
        {
            set
            {
                aboutCommand = value;
            }

            get
            {
                if (aboutCommand == null)
                {
                    aboutCommand = new RelayCommand(About);
                }

                return aboutCommand;
            }
        }

        private ICommand exitCommand;

        public ICommand ExitCommand
        {
            set
            {
                exitCommand = value;
            }

            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommand(Exit);
                }

                return exitCommand;
            }
        }

        public string TaskDescription
        {
            get
            {
                return SelectedTask?.Description;
            }
        }

        private bool isButtonEnabled;
        private bool isToDoListSelected;
        public bool IsEditing { get; set; } = false;

        public bool IsButtonEnabled
        {
            set
            {
                isButtonEnabled = value;
                NotifyPropertyChanged("IsButtonEnabled");
            }

            get
            {
                return isButtonEnabled;
            }
        }

        public bool IsToDoListSelected
        {
            set
            {
                isToDoListSelected = value;
                NotifyPropertyChanged("IsToDoListSelected");
            }

            get
            {
                return isToDoListSelected;
            }
        }

        public MainWindowVM()
        {
            ItemsToDoList = new ObservableCollection<TDL>();
            database = new Database();

            Category.CategoriesList = new ObservableCollection<string>()
            {
                "Work", "School", "Home", "Workout"
            };
        }

        public void UpdateToDoListSelection(object param)
        {
            SelectedToDoList = param as TDL;

            if (SelectedToDoList != null)
            {
                ItemsTasks = SelectedToDoList.Tasks;
            }

            IsToDoListSelected = true;
        }

        public void CreateDatabase(object param)
        {
            AddDatabaseWindow addDatabaseWindow = new AddDatabaseWindow();
            addDatabaseWindow.ShowDialog();

            IsButtonEnabled = true;
            ItemsToDoList = new ObservableCollection<TDL>();
            ItemsTasks = new ObservableCollection<Task>();
        }

        public void SaveDatabase(object param)
        {
            database.ToDoLists = new ObservableCollection<TDL>(ItemsToDoList);
            database.Categories = Category.CategoriesList;

            if (!File.Exists(Database.Path))
            {
                FileStream fileStream = File.Create(Database.Path);
                fileStream.Close();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Database));

            using (TextWriter writer = new StreamWriter(Database.Path))
            {
                serializer.Serialize(writer, database);
            }

            MessageBox.Show("Database successfully saved!", "Congratulations!");
        }

        public void LoadDatabase(object param)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\Users\\WIN10\\source\\repos\\AgendaManager\\AgendaManager\\Assets\\Databases",

                Title = "Open Database",

                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Database.Path = openFileDialog.FileName;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Database));

            using (TextReader reader = new StreamReader(Database.Path))
            {
                database = (Database)serializer.Deserialize(reader);
            }

            if (database.Categories != null)
            {
                Category.CategoriesList = database.Categories;
            }

            ItemsToDoList = database.ToDoLists;
            IsButtonEnabled = true;

            MessageBox.Show("Database successfully loaded!", "Congratulations!");
        }

        public void AddTask(object param)
        {
            AddTaskWindow addTaskWindow = new AddTaskWindow();
            Messenger.Default.Register<Task>(this, ReceiveTask);
            addTaskWindow.ShowDialog();
        }

        public void EditTask(object param)
        {
            AddTaskWindow addTaskWindow = new AddTaskWindow();
            IsEditing = true;
            Messenger.Default.Register<Task>(this, ReceiveTask);
            addTaskWindow.ShowDialog();
        }

        public void ReceiveTask(Task task)
        {
            if (IsEditing)
            {
                int replacedIndex = SelectedToDoList.Tasks.IndexOf(SelectedTask);

                SelectedToDoList.Tasks[replacedIndex] = task;
                IsEditing = false;
            }
            else
            {
                SelectedToDoList.Tasks.Add(task);
            }

            Messenger.Default.Unregister<Task>(this, ReceiveTask);

        }

        public void DeleteTask(object param)
        {
            SelectedToDoList.Tasks.Remove(SelectedTask);
        }

        public void SetDoneTask(object param)
        {
            if (SelectedTask != null)
            {
                SelectedTask.TaskStatus = true;
            }
        }

        public void MoveUpTask(object param)
        {
            int index = SelectedToDoList.Tasks.IndexOf(SelectedTask);

            if (index > 0)
            {
                (SelectedToDoList.Tasks[index], SelectedToDoList.Tasks[index - 1]) = (SelectedToDoList.Tasks[index - 1], SelectedToDoList.Tasks[index]);
            }
        }

        public void MoveDownTask(object param)
        {
            int index = SelectedToDoList.Tasks.IndexOf(SelectedTask);

            if (index < SelectedToDoList.Tasks.Count - 1)
            {
                (SelectedToDoList.Tasks[index], SelectedToDoList.Tasks[index + 1]) = (SelectedToDoList.Tasks[index + 1], SelectedToDoList.Tasks[index]);
            }
        }

        public void ManageCategory(object param)
        {
            CategoriesWindow categoriesWindow = new CategoriesWindow();
            categoriesWindow.ShowDialog();
        }

        void CountTasks(ObservableCollection<Task> tasks, ObservableCollection<TDL> currentTDL)
        {
            foreach (TDL tdl in currentTDL)
            {
                foreach (Task task in tdl.Tasks)
                {
                    tasks.Add(task);
                }

                CountTasks(tasks, tdl.SubToDoLists);
            }
        }

        public void AddRootToDoList(object param)
        {
            AddToDoListWindow addToDoListWindow = new AddToDoListWindow();
            Messenger.Default.Send(ItemsToDoList);
            Messenger.Default.Register<TDL>(this, ReceiveObject);
            addToDoListWindow.ShowDialog();
        }

        public void AddSubToDoList(object param)
        {
            RootToDoList = param as TDL;
            AddToDoListWindow addToDoListWindow = new AddToDoListWindow();
            Messenger.Default.Send(ItemsToDoList);
            Messenger.Default.Register<TDL>(this, ReceiveObject);
            addToDoListWindow.ShowDialog();
        }

        public void EditToDoList(object param)
        {
            AddToDoListWindow editToDoListWindow = new AddToDoListWindow();
            IsEditing = true;
            Messenger.Default.Send(ItemsToDoList);
            Messenger.Default.Register<TDL>(this, ReceiveObject);
            editToDoListWindow.ShowDialog();
        }

        public void DeleteToDoList(object param)
        {
            TDL tdl = SelectedToDoList;
            FindAndEditToDoList(tdl, ItemsToDoList, true);
        }

        public void MoveUpToDoList(object param)
        {
            FindAndSwapToDoList(ItemsToDoList, -1);
        }

        public void MoveDownToDoList(object param)
        {
            FindAndSwapToDoList(ItemsToDoList, 1);
        }

        public void ChangePath(object param)
        {
            ChangeRootWindow changeRootWindow = new ChangeRootWindow();
            string toDoListName = SelectedToDoList.Name;
            Messenger.Default.Send(toDoListName);
            Messenger.Default.Send(ItemsToDoList);
            Messenger.Default.Register<TDL>(this, ReceiveParent);
            changeRootWindow.ShowDialog();
        }

        public void ReceiveParent(TDL rootToDoList)
        {
            TDL temp = SelectedToDoList;
            ObservableCollection<TDL> formerRootList = FindRoot(SelectedToDoList, ItemsToDoList);
            if (formerRootList != ItemsToDoList || rootToDoList != null)
            {
                formerRootList.Remove(temp);
            }
            else
            {
                ItemsToDoList.Remove(temp);
            }

            if (rootToDoList != null)
            {
                rootToDoList.SubToDoLists.Add(temp);
            }
            else
            {
                ItemsToDoList.Add(temp);
            }
            Messenger.Default.Unregister<TDL>(this, ReceiveParent);
        }

        private ObservableCollection<TDL> FindRoot(TDL descendentToDoList, ObservableCollection<TDL> sourceToDoList)
        {
            for (int i = 0; i < sourceToDoList.Count; ++i)
            {
                if (sourceToDoList[i] == descendentToDoList)
                {
                    return sourceToDoList;
                }
                if (sourceToDoList[i] is TDL)
                {
                    ObservableCollection<TDL> rootTDL = FindRoot(descendentToDoList, sourceToDoList[i].SubToDoLists);
                    if (rootTDL != null)
                    {
                        return rootTDL;
                    }
                }
            }

            return null;
        }

        private void FindAndSwapToDoList(ObservableCollection<TDL> sourceToDoList, int direction)
        {
            for (int i = 0; i < sourceToDoList.Count; ++i)
            {
                if (sourceToDoList[i] == SelectedToDoList)
                {
                    if ((i > 0 && direction == -1) || (i < sourceToDoList.Count - 1 && direction == 1))
                    {
                        (sourceToDoList[i], sourceToDoList[i + direction]) = (sourceToDoList[i + direction], sourceToDoList[i]);
                        return;
                    }
                }

                if (sourceToDoList[i] is TDL)
                {
                    FindAndSwapToDoList(sourceToDoList[i].SubToDoLists, direction);
                }
            }
        }

        private void FindAndEditToDoList(TDL toDoList, ObservableCollection<TDL> sourceToDoList, bool delete = false)
        {
            for (int i = 0; i < sourceToDoList.Count; ++i)
            {
                if (sourceToDoList[i] == SelectedToDoList)
                {
                    if (delete)
                    {
                        SelectedToDoList = null;
                        sourceToDoList.RemoveAt(i);
                    }
                    else
                    {
                        SelectedToDoList.Name = toDoList.Name;
                        SelectedToDoList.ImagePath = toDoList.ImagePath;
                        sourceToDoList[i] = SelectedToDoList;
                    }
                    return;
                }
                if (sourceToDoList[i] is TDL)
                {
                    FindAndEditToDoList(toDoList, sourceToDoList[i].SubToDoLists, delete);
                }
            }
        }

        private void ReceiveObject(TDL toDoList)
        {
            if (IsEditing)
            {
                FindAndEditToDoList(toDoList, ItemsToDoList);
                IsEditing = false;
            }
            else
            {
                if (RootToDoList == null)
                {
                    ItemsToDoList.Add(toDoList);
                }
                else
                {
                    RootToDoList.SubToDoLists.Add(toDoList);
                }
            }

            RootToDoList = null;

            Messenger.Default.Unregister<TDL>(this, ReceiveObject);
        }

        private void Filter(object param)
        {
            FilterWindow filterWindow = new FilterWindow();
            Messenger.Default.Register<string>(this, ReceivedFilter);
            filterWindow.ShowDialog();

            if (CurrentFilter == "Done")
            {
                ItemsTasks = new ObservableCollection<Task>(ItemsTasks.Where(x => x.TaskStatus == true));
            }
            else if (CurrentFilter == "Overdue")
            {
                ItemsTasks = new ObservableCollection<Task>(ItemsTasks.Where(x => x.TaskStatus == true && x.CompletionDate > x.Deadline));
            }
            else if (CurrentFilter == "Unfinished overdue")
            {
                ItemsTasks = new ObservableCollection<Task>(ItemsTasks.Where(x => x.TaskStatus == false && x.Deadline < DateTime.Now));
            }
            else if (CurrentFilter == "Unfinished due")
            {
                ItemsTasks = new ObservableCollection<Task>(ItemsTasks.Where(x => x.TaskStatus == false && x.Deadline > DateTime.Now));
            }
            else
            {
                foreach (string category in Category.CategoriesList)
                {
                    if (category == CurrentFilter)
                    {
                        ItemsTasks = new ObservableCollection<Task>(ItemsTasks.Where(x => x.TaskCategory == category));
                        break;
                    }
                }
            }
        }

        private void ReceivedFilter(string filter)
        {
            CurrentFilter = filter;
            Messenger.Default.Unregister<string>(this, ReceivedFilter);
        }

        private void FindTask(object param)
        {
            FindTaskWindow findTaskWindow = new FindTaskWindow();
            FindTaskVM findTaskVM = findTaskWindow.DataContext as FindTaskVM;
            findTaskVM.ToDoLists = ItemsToDoList;
            findTaskWindow.ShowDialog();
        }

        private void About(object param)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void Exit(object param)
        {
            if (param is Window window)
            {
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
