using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.Types;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace To_Do.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string OwnerID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsImportant { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime LastEditedOn { get; set; } = DateTime.Now;
        public Folder Folder { get; set; }
        public int? FolderId { get; set; }

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
