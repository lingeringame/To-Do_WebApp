using System.Collections.Generic;
using System.Linq;
using To_Do.Models;

namespace To_Do.Data
{
    public interface IApplicationRepository
    {
        void AddNewToDo(ToDoTask todo);
        void SaveChanges();
        IEnumerable<ToDoTask> GetTodos();
        IEnumerable<ToDoTask> GetTodosByUserId(int id);
        void DeleteTodo(ToDoTask todo);
    }
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationRepository()
        {

        }
        public void AddNewToDo(ToDoTask todo)
        {
            _context.TodoTasks.Add(todo);
            throw new System.NotImplementedException();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void DeleteTodo(ToDoTask todo)
        {
            _context.TodoTasks.Remove(todo);
            throw new System.NotImplementedException();
        }

        public IEnumerable<ToDoTask> GetTodos()
        {
            return _context.TodoTasks.ToList();
        }

        public IEnumerable<ToDoTask> GetTodosByUserId(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
