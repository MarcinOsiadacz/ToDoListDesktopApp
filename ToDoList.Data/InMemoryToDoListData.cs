using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ToDoListLogic;

namespace ToDoList.Data
{
    public class InMemoryToDoListData : IToDoItemData
    {
        List<ToDoItem> toDoItems;

        public InMemoryToDoListData()
        {
            toDoItems = new List<ToDoItem>()
            {
                new ToDoItem
                {
                    Id = 1,
                    Name = "Test 1",
                    DueDate = null,
                    Priority = Priority.Medium,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    Id = 2,
                    Name = "Test 2",
                    DueDate = null,
                    Priority = Priority.Low,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    Id = 3,
                    Name = "Test 3",
                    DueDate = DateTime.Now,
                    Priority = Priority.High,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    Id = 4,
                    Name = "Test 3",
                    DueDate = DateTime.Now,
                    Priority = Priority.High,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    Id = 5,
                    Name = "Test 3",
                    DueDate = DateTime.Now,
                    Priority = Priority.High,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    Id = 6,
                    Name = "Test 3",
                    DueDate = DateTime.Now,
                    Priority = Priority.High,
                    IsCompleted = false,
                },
            };
        }

        public void Add(ToDoItem newItem)
        {
            toDoItems.Add(newItem);
            newItem.Id = toDoItems.Max(i => i.Id) + 1;
        }

        public int GetCountOfIncompleteItems()
        {
            return (from i in toDoItems 
                    where !i.IsCompleted
                    select i)
                    .Count();
        }

        public IEnumerable<ToDoItem> GetIncompleteItemsByName(string name = null)
        {
            return from i in toDoItems
                   where (string.IsNullOrEmpty(name) || i.Name.Contains(name)) &&
                   !(i.IsCompleted)
                   orderby i.Name
                   select i;
        }

        public void Update(ToDoItem updatedItem)
        {
            var item = toDoItems.SingleOrDefault(i => i.Id == updatedItem.Id);
            
            if(item != null)
            {
                item.Name = updatedItem.Name;
                item.DueDate = updatedItem.DueDate;
                item.Priority = updatedItem.Priority;
                item.IsCompleted = updatedItem.IsCompleted;
            }
        }
    }
}
