using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoListLogic
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; }
        public bool IsCompleted { get; set; }
    }
}
