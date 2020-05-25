using System;
using System.Collections.Generic;
using System.Text;
using ToDoListLogic;

namespace ToDoList.Data
{
    public interface IToDoItemData
    {
        IEnumerable<ToDoItem> GetIncompleteItemsByName(string name = null);
        void Add(ToDoItem newItem);
        void Update(ToDoItem updatedItem);
        int GetCountOfIncompleteItems();
    }
}
