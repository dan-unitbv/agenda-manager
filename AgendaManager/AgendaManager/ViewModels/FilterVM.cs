using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight.Messaging;

using AgendaManager.Models;
using AgendaManager.Commands;

namespace AgendaManager.ViewModels
{
    public class FilterVM
    {
        public string TaskFilter { get; set; }
        public ObservableCollection<string> CategoriesList { get; set; } = Category.CategoriesList;
        public bool IsCategoryChecked { get; set; }
        public bool IsDoneChecked { get; set; }
        public bool IsOverdueChecked { get; set; }
        public bool IsUnfinishedDueChecked { get; set; }
        public bool IsUnfinishedOverdueChecked { get; set; }

        private void Confirm(object param)
        {
            if (IsDoneChecked)
            {
                TaskFilter = "Done";
            }
            else if (IsOverdueChecked)
            {
                TaskFilter = "Overdue";
            }
            else if (IsUnfinishedOverdueChecked)
            {
                TaskFilter = "Unfinished overdue";
            }
            else if (IsUnfinishedDueChecked)
            {
                TaskFilter = "Unfinished due";
            }

            Messenger.Default.Send(TaskFilter);

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
    }
}
