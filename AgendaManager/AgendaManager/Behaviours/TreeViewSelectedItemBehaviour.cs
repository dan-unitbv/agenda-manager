using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace AgendaManager.Behaviours
{
    public static class TreeViewSelectedItemBehaviour
    {
        public static readonly DependencyProperty SelectedItemChangedCommandProperty = DependencyProperty.RegisterAttached("SelectedItemChangedCommand", typeof(ICommand),
            typeof(TreeViewSelectedItemBehaviour), new PropertyMetadata(null, OnSelectedItemChangedCommandPropertyChanged));

        public static void SetSelectedItemChangedCommand(TreeView treeView, ICommand value)
        {
            treeView.SetValue(SelectedItemChangedCommandProperty, value);
        }

        public static ICommand GetSelectedItemChangedCommand(TreeView treeView)
        {
            return (ICommand)treeView.GetValue(SelectedItemChangedCommandProperty);
        }

        private static void OnSelectedItemChangedCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TreeView treeView)
            {
                treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;

                if (e.NewValue is ICommand command)
                {
                    treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
                }
            }
        }

        private static void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is TreeView treeView)
            {
                ICommand command = GetSelectedItemChangedCommand(treeView);

                if (command != null && command.CanExecute(e.NewValue))
                {
                    command.Execute(e.NewValue);
                }
            }
        }
    }
}