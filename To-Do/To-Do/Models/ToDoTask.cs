using System;

namespace To_Do.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        //public bool IsCompleted { get; set; }
        public ToDoTask()
        {

        }

        public override bool Equals(object obj)
        {
            return obj is ToDoTask @do &&
                   Id == @do.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
        public override string ToString()
        {
            return Title;
        }
    }
}
