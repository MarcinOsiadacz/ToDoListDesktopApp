using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace ToDoListLogic
{
    public static class Priority
    {
        public const string High = "High";
        public const string Medium = "Medium";
        public const string Low = "Low";

        public static IEnumerable<string> GetPriorities()
        {
            List<string> priorities = new List<string>()
            {
                High,
                Medium,
                Low,
            };
            return priorities;
        }
    }
}
