using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp
{
    public class Task
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public int Completion { get; set; }
        public string Description { get; set; }

        public Task(DateTime date,string title, int completion, string description)
        {
            Date = date;
            Title = title;
            Completion = completion;
            Description = description;
        }
    }
}
