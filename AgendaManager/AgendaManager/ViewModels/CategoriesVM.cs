using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Messaging;

using AgendaManager.Models;
using AgendaManager.Windows;
using AgendaManager.Commands;

namespace AgendaManager.ViewModels
{
    public class CategoriesVM : INotifyPropertyChanged
    {
        public CategoriesVM()
        {
            Messenger.Default.Register<string>(this, RenameCategory);
        }

        private ObservableCollection<string> categoryList = Category.CategoriesList;

        public ObservableCollection<string> CategoryCollection
        {
            set
            {
                categoryList = value;
                Category.CategoriesList = categoryList;
                NotifyPropertyChanged("CategoryCollection");
            }

            get { return categoryList; }
        }

        public string selectedCategory;

        private string newCategory;

        public string SelectedCategory
        {
            set
            {
                selectedCategory = value;
                NotifyPropertyChanged("SelectedCategory");
            }

            get { return selectedCategory; }
        }
        public void AddCategory(object param)
        {
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow();
            addCategoryWindow.ShowDialog();
            CategoryCollection.Add(newCategory);
        }

        public void EditCategory(object param)
        {
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow();
            addCategoryWindow.ShowDialog();
            CategoryCollection[CategoryCollection.IndexOf(SelectedCategory)] = newCategory;
        }

        public void RenameCategory(string name)
        {
            newCategory = name;
        }

        public void DeleteCategory(object param)
        {
            CategoryCollection.Remove(SelectedCategory);
        }

        private ICommand addCategoryCommand;

        public ICommand AddCategoryCommand
        {
            get
            {
                if (addCategoryCommand == null)
                {
                    addCategoryCommand = new RelayCommand(AddCategory);
                }

                return addCategoryCommand;
            }
        }

        private ICommand editCategoryCommand;

        public ICommand EditCategoryCommand
        {
            set
            {
                editCategoryCommand = value;
            }

            get
            {
                if (editCategoryCommand == null)
                {
                    editCategoryCommand = new RelayCommand(EditCategory);
                }

                return editCategoryCommand;
            }
        }

        private ICommand deleteCategoryCommand;

        public ICommand DeleteCategoryCommand
        {
            set
            {
                deleteCategoryCommand = value;
            }

            get
            {
                if (deleteCategoryCommand == null)
                {
                    deleteCategoryCommand = new RelayCommand(DeleteCategory);
                }

                return deleteCategoryCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
