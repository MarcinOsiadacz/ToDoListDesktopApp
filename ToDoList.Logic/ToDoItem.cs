using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoListLogic
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }
    }
}
