using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
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
        private readonly IToDoItemData toDoItemData;

        private static readonly int _maxItemsPerPage = 10;
        public int CurrentPage { get; private set; }
        public int LastPage { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            this.toDoItemData = new SqlToDoListData(new ToDoListDbContext());

            PriorityListBox.ItemsSource = Priority.GetPriorities();
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
            var newItem = new ToDoItem
            {
                ItemName = NameTextBox.Text,
                Priority = PriorityListBox.SelectedItem.ToString(),
                IsCompleted = false,
                DueDate = DueDatePicker.SelectedDate
            };

            toDoItemData.Add(newItem);
            toDoItemData.Commit();

            RefreshToDoItemsDataGrid();
        }

        RelayCommand MarkCompleteCommand;
        private void MarkComplete()
        {
            var updatedItem = (ToDoItem)ToDoItemsDataGrid.SelectedItem;
            updatedItem.IsCompleted = true;
            toDoItemData.Update(updatedItem);
            toDoItemData.Commit();

            RefreshToDoItemsDataGrid();

            // CompletionScreen.Visibility = Visibility.Visible;
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
            return (int)Math.Ceiling(toDoItemData.GetCountOfIncompleteItems() / (double)_maxItemsPerPage);
        }

        private void RefreshToDoItemsDataGrid()
        {
            LastPage = CalculateLastPage();

            ToDoItemsDataGrid.ItemsSource = toDoItemData.GetIncompleteItemsByName()
                    .Skip((CurrentPage - 1) * _maxItemsPerPage)
                    .Take(_maxItemsPerPage);
        }

        private void CompletionScreen_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ((Storyboard)CompletionScreen.Resources["MarkCompleteStoryboard"]).Begin();
            }
        }
    }
}
