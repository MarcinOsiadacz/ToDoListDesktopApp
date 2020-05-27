using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using ToDoList.Data;
using ToDoListLogic;

namespace ToDoList
{
    public partial class MainWindow : Window
    {
        private readonly IToDoItemData toDoItemData;
        private static readonly int _maxItemsPerPage = 10;

        public int CurrentPage { get; private set; }
        public int LastPage { get; private set; }
        public IEnumerable<ToDoItem> Items { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "To-Do List";
            this.toDoItemData = new SqlToDoListData(new ToDoListDbContext());
            PriorityListBox.ItemsSource = Priority.GetPriorities();

            CurrentPage = 1;

            AllTasksRefresh();     

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
        private async void AddItem()
        {
            var newItem = new ToDoItem
            {
                ItemName = NameTextBox.Text,
                Priority = PriorityListBox.SelectedItem.ToString(),
                IsCompleted = false,
                DueDate = DueDatePicker.SelectedDate
            };

            await Task.Run(() => toDoItemData.Add(newItem));

            AllTasksRefresh();
        }

        RelayCommand MarkCompleteCommand;
        private async void MarkComplete()
        {
            var updatedItem = (ToDoItem)ToDoItemsDataGrid.SelectedItem;
            updatedItem.IsCompleted = true;
            
            await Task.Run(() => toDoItemData.Update(updatedItem));

            AllTasksRefresh();

            CompletionScreen.Visibility = Visibility.Visible;
        }

        RelayCommand PreviousPageCommand;
        private void PreviousPage()
        {
            CurrentPage--;
            AllTasksRefresh();
        }

        RelayCommand NextPageCommand;
        private void NextPage()
        {
            CurrentPage++;
            AllTasksRefresh();
        }

        private int CalculateLastPage()
        {
            return (int)Math.Ceiling(toDoItemData.GetCountOfIncompleteItems() / (double)_maxItemsPerPage);
        }

        private async void AllTasksRefresh()
        {
            await Task.Run(() => LastPage = CalculateLastPage());

            await Task.Run(() => Items = toDoItemData.GetIncompleteItemsByName()
            .Skip((CurrentPage - 1) * _maxItemsPerPage)
            .Take(_maxItemsPerPage));

            Dispatcher.Invoke(() =>
            {
                ToDoItemsDataGrid.ItemsSource = Items;
                PagecountTextBlock.Text = $"{CurrentPage} of {LastPage}";
            });
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
