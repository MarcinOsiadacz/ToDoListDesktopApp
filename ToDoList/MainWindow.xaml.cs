using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ToDoList.Data;
using ToDoList.Logic;
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
        public bool StateSelected {get; private set;}

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "To-Do List";
            this.toDoItemData = new SqlToDoItemData(new ToDoListDbContext());
            PriorityListBox.ItemsSource = Priority.GetPriorities();
            StateComboBox.ItemsSource = State.GetStates();

            CurrentPage = 1;  

            SaveItemCommand = new RelayCommand(obj => SaveItem(), obj =>
                !string.IsNullOrEmpty(NameTextBox.Text) &&
                PriorityListBox.SelectedItem != null
            );
            SaveButton.Command = SaveItemCommand;

            MarkCompleteCommand = new RelayCommand(obj => MarkComplete(), obj => 
                ToDoItemsDataGrid.SelectedItem != null &&
                !StateSelected
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

            EditTaskCommand = new RelayCommand(obj => EditTask(), obj =>
                ToDoItemsDataGrid.SelectedItem != null &&
                !StateSelected
            );
            EditTaskButton.Command = EditTaskCommand;

            NewTaskCommand = new RelayCommand(obj => NewTask());
            NewTaskButton.Command = NewTaskCommand;

            RestoreActiveCommand = new RelayCommand(obj => RestoreActive(), obj =>
            ToDoItemsDataGrid.SelectedItem != null &&
            StateSelected
            );
            RestoreAsActiveButton.Command = RestoreActiveCommand;

            HelpCommand = new RelayCommand(obj => Help());
            HelpButton.Command = HelpCommand;
        }

        RelayCommand SaveItemCommand;
        private async void SaveItem()
        {
            if (ToDoItemsDataGrid.SelectedItem != null)
            {
                var editedItem = (ToDoItem)ToDoItemsDataGrid.SelectedItem;

                editedItem.ItemName = NameTextBox.Text;
                editedItem.Priority = PriorityListBox.SelectedItem.ToString();
                editedItem.DueDate = DueDatePicker.SelectedDate;

                await Task.Run(() => toDoItemData.Update(editedItem));
            }
            else
            {
                var newItem = new ToDoItem
                {
                    ItemName = NameTextBox.Text,
                    Priority = PriorityListBox.SelectedItem.ToString(),
                    IsCompleted = false,
                    DueDate = DueDatePicker.SelectedDate
                };

                await Task.Run(() => toDoItemData.Add(newItem));
            }

            ClearInputValues();
            TasksViewRefresh();
        }

        private void ClearInputValues()
        {
            NameTextBox.Text = string.Empty;
            PriorityListBox.SelectedItem = null;
            DueDatePicker.SelectedDate = null;
        }

        RelayCommand MarkCompleteCommand;
        private async void MarkComplete()
        {
            var itemToComplete = (ToDoItem)ToDoItemsDataGrid.SelectedItem;
            itemToComplete.IsCompleted = true;

            await Task.Run(() => toDoItemData.Update(itemToComplete));
            TasksViewRefresh();

            // CompletionScreen.Visibility = Visibility.Visible;
        }

        RelayCommand PreviousPageCommand;
        private void PreviousPage()
        {
            CurrentPage--;
            TasksViewRefresh();
        }

        RelayCommand NextPageCommand;
        private void NextPage()
        {
            CurrentPage++;
            TasksViewRefresh();
        }

        private int CalculateLastPage()
        {
            return (int)Math.Ceiling(toDoItemData.GetCountOfItems(state: StateSelected) / (double)_maxItemsPerPage);
        }

        private async void TasksViewRefresh()
        {
            LastPage = await Task.Run(() => CalculateLastPage());

            if (CurrentPage > LastPage &&
                CurrentPage > 1)
            {
                CurrentPage--;
            }

            Items = await Task.Run(() => toDoItemData.GetItemsByNameAndState(state: StateSelected)              
                    .Skip((CurrentPage - 1) * _maxItemsPerPage)
                    .Take(_maxItemsPerPage));

            RefreshPagecount();
            ToDoItemsDataGrid.ItemsSource = Items;        
        }

        private void RefreshPagecount()
        {
            if (LastPage == 0)
            {
                PagecountTextBlock.Text = "No Tasks";
            }
            else
            {
                PagecountTextBlock.Text = $"{CurrentPage} of {LastPage}";
            }
        }
        /*
        private void CompletionScreen_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ((Storyboard)CompletionScreen.Resources["MarkCompleteStoryboard"]).Begin();
            }
        }
        */
        private void StateComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Select active items by default
            StateComboBox.SelectedIndex = 0;
        }

        private void StateComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (StateComboBox.SelectedItem.ToString() == State.Completed)
            {
                StateSelected = true;
            }
            else
            {
                StateSelected = false;
            }

            CurrentPage = 1;
            TasksViewRefresh();
        }

        RelayCommand EditTaskCommand;
        private void EditTask()
        {
            var editedItem = (ToDoItem)ToDoItemsDataGrid.SelectedItem;

            NameTextBox.Text = editedItem.ItemName;
            PriorityListBox.SelectedItem = editedItem.Priority;
            DueDatePicker.SelectedDate = editedItem.DueDate;
        }

        RelayCommand NewTaskCommand;
        private void NewTask()
        {
            ToDoItemsDataGrid.SelectedItem = null;
        }

        private void ToDoItemsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ClearInputValues();
        }

        RelayCommand RestoreActiveCommand;
        private async void RestoreActive()
        {
            var itemToRestore = (ToDoItem)ToDoItemsDataGrid.SelectedItem;
            itemToRestore.IsCompleted = false;

            await Task.Run(() => toDoItemData.Update(itemToRestore));
            TasksViewRefresh();
        }

        RelayCommand HelpCommand;
        private void Help()
        {
            MessageBox.Show("Help Test!");
        }
    }
}
