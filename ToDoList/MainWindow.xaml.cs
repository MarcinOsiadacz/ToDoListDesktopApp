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

        private static readonly int _maxItemsPerPage = 10;
        public int CurrentPage { get; private set; }
        public int LastPage { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            PriorityListBox.ItemsSource = Enum.GetValues(typeof(Priority));
            RefreshToDoItemsDataGrid();
            
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

            PreviousPageCommand = new RelayCommand(obj => PreviousPage(), obj => 
                CurrentPage - 1 > 0
            );
            PreviousButton.Command = PreviousPageCommand;

            NextPageCommand = new RelayCommand(obj => NextPage(), obj =>
                CurrentPage < LastPage
            );
            NextButton.Command = NextPageCommand;
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

            RefreshToDoItemsDataGrid();
        }

        RelayCommand MarkCompleteCommand;
        private void MarkComplete()
        {
            var item = (ToDoItem)ToDoItemsDataGrid.SelectedItem;
            item.IsCompleted = true;
            data.Update(item);

            RefreshToDoItemsDataGrid();
        }

        RelayCommand PreviousPageCommand;
        private void PreviousPage()
        {
            CurrentPage--;
            RefreshToDoItemsDataGrid();
        }

        RelayCommand NextPageCommand;
        private void NextPage()
        {
            CurrentPage++;
            RefreshToDoItemsDataGrid();
        }

        private int CalculateLastPage()
        {
            return (int)Math.Ceiling(data.GetCountOfIncompleteItems() / (double)_maxItemsPerPage);
        }

        private void RefreshToDoItemsDataGrid()
        {
            LastPage = CalculateLastPage();

            ToDoItemsDataGrid.ItemsSource = data.GetIncompleteItemsByName()
                    .Skip((CurrentPage - 1) * _maxItemsPerPage)
                    .Take(_maxItemsPerPage);
        }
    }
}
