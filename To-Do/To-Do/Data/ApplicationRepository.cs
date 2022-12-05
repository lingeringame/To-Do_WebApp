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
        ToDoTask GetTodoById(int id);
        void UpdateTask(ToDoTask todo);
        IEnumerable<ToDoTask> GetTodosByUserId(int id);
        void DeleteTodo(int id);
    }
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationRepository()
        {
        }
        public ApplicationRepository(ApplicationDbContext dbContext)
        {
            _context= dbContext;
        }
        public void AddNewToDo(ToDoTask todo)
        {
            _context.TodoTasks.Add(todo);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void DeleteTodo(int id)
        {
            ToDoTask todoToRemove= _context.TodoTasks.FirstOrDefault(td => td.Id==id);
            _context.TodoTasks.Remove(todoToRemove);
        }
        public ToDoTask GetTodoById(int id)
        {
            return _context.TodoTasks.Find(id);
        }

        public IEnumerable<ToDoTask> GetTodos()
        {
            return _context.TodoTasks.ToList();
        }

        public IEnumerable<ToDoTask> GetTodosByUserId(int id)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateTask(ToDoTask todo)
        {
            _context.TodoTasks.Update(todo);
        }
    }
}
