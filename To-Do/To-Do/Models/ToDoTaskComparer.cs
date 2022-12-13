using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using To_Do.Models;

namespace To_Do.Models
{
    public class ToDoTaskComparer : IComparer<ToDoTask>
    {
        public int Compare(ToDoTask x, ToDoTask y)
        {
            return y.IsImportant.CompareTo(x.IsImportant);
        }
    }
}
