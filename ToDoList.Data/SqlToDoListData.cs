﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListLogic;

namespace ToDoList.Data
{
    public class SqlToDoListData : IToDoItemData
    {
        private readonly ToDoListDbContext db;

        public SqlToDoListData(ToDoListDbContext db)
        {
            this.db = db;
        }

        public void Add(ToDoItem newItem)
        {
            db.ToDoItems.Add(newItem);
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public int GetCountOfIncompleteItems()
        {
            return db.ToDoItems.Count();
        }

        public IEnumerable<ToDoItem> GetIncompleteItemsByName(string name = null, bool state = false)
        {
            var query = from i in db.ToDoItems
                        where (string.IsNullOrEmpty(name) || i.ItemName.Contains(name)) &&
                        i.IsCompleted == state
                        orderby i.ItemName
                        select i;
            return query;
        }

        public void Update(ToDoItem updatedItem)
        {
            var result = db.ToDoItems.SingleOrDefault(i => i.Id == updatedItem.Id);
            
            if (result != null)
            {
                if (result != null)
                {
                    result.ItemName = updatedItem.ItemName;
                    result.DueDate = updatedItem.DueDate;
                    result.Priority = updatedItem.Priority;
                    result.IsCompleted = updatedItem.IsCompleted;
                }
            }       
        }
    }
}
