using System;
using System.Collections.Generic;

namespace To_Do.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<ToDoTask> tasks { get; set; }   
        public Folder()
        {
            tasks = new List<ToDoTask>();
        }

        public override bool Equals(object obj)
        {
            return obj is Folder folder &&
                   Id == folder.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
