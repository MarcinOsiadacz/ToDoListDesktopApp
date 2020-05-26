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
                    ItemName = "Test 1",
                    DueDate = null,
                    Priority = Priority.Medium,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    Id = 2,
                    ItemName = "Test 2",
                    DueDate = null,
                    Priority = Priority.Low,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    Id = 3,
                    ItemName = "Test 3",
                    DueDate = DateTime.Now,
                    Priority = Priority.High,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    Id = 4,
                    ItemName = "Test 3",
                    DueDate = DateTime.Now,
                    Priority = Priority.High,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    Id = 5,
                    ItemName = "Test 3",
                    DueDate = DateTime.Now,
                    Priority = Priority.High,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    //Id = 6,
                    ItemName = "Test 3",
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

        public int Commit()
        {
            return 0;
        }

        public int GetCountOfIncompleteItems()
        {
            return (from i in toDoItems 
                    where !i.IsCompleted
                    select i)
                    .Count();
        }

        public IEnumerable<ToDoItem> GetIncompleteItemsByName(string name = null, bool state = false)
        {
            return from i in toDoItems
                   where (string.IsNullOrEmpty(name) || i.ItemName.Contains(name)) &&
                   i.IsCompleted == state
                   orderby i.ItemName
                   select i;
        }

        public void Update(ToDoItem updatedItem)
        {
            var item = toDoItems.SingleOrDefault(i => i.Id == updatedItem.Id);
            
            if(item != null)
            {
                item.ItemName = updatedItem.ItemName;
                item.DueDate = updatedItem.DueDate;
                item.Priority = updatedItem.Priority;
                item.IsCompleted = updatedItem.IsCompleted;
            }
        }
    }
}
