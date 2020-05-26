namespace ToDoList.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using ToDoListLogic;

    public class ToDoListDbContext : DbContext
    {
        public ToDoListDbContext()
            : base("name=ToDoListDbContext")
        {
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}