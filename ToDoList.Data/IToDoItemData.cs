using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoListLogic;

namespace ToDoList.Data
{
    public interface IToDoItemData
    {
        IEnumerable<ToDoItem> GetIncompleteItemsByName(string name = null, bool state = false);
        void Add(ToDoItem newItem);
        void Update(ToDoItem updatedItem);
        int GetCountOfIncompleteItems();
    }
}
