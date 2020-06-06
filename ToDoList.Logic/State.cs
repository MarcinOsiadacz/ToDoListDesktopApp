using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Logic
{
    public static class State
    {
        public const string Active = "Show Active";
        public const string Completed = "Show Completed";

        public static IEnumerable<string> GetStates()
        {
            List<string> states = new List<string>()
            {
                Active,
                Completed,    
            };
            return states;
        }
    }
}
