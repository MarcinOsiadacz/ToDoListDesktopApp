using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoList.Data;
using ToDoListLogic;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InMemoryToDoListData data = new InMemoryToDoListData();

        private static readonly int _maxRecordsPerPage = 10;
        public int CurrentPage { get; private set; }
        public int LastPage { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            ToDoItemsDataGrid.ItemsSource = data.GetIncompleteItemsByName();        
            PriorityListBox.ItemsSource = Enum.GetValues(typeof(Priority));

            CurrentPage = 1;

            AddItemCommand = new RelayCommand(obj => AddItem(), obj =>
                !string.IsNullOrEmpty(NameTextBox.Text) &&
                PriorityListBox.SelectedItem != null
            );
            AddItemButton.Command = AddItemCommand;

            MarkCompleteCommand = new RelayCommand(obj => MarkComplete(), obj =>
               ToDoItemsDataGrid.SelectedItem != null
            );
            MarkAsCompletedButton.Command = MarkCompleteCommand;
        }

        RelayCommand AddItemCommand;
        private void AddItem()
        {
            data.Add(new ToDoItem
            {
                Name = NameTextBox.Text,
                Priority = (Priority)PriorityListBox.SelectedItem,
                IsCompleted = false,
                DueDate = DueDatePicker.SelectedDate
            });

            ToDoItemsDataGrid.ItemsSource = data.GetIncompleteItemsByName();
        }

        RelayCommand MarkCompleteCommand;
        private void MarkComplete()
        {
            var item = (ToDoItem)ToDoItemsDataGrid.SelectedItem;
            item.IsCompleted = true;
            data.Update(item);

            ToDoItemsDataGrid.ItemsSource = data.GetIncompleteItemsByName();
        }
    }
}
