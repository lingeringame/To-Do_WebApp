using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using To_Do.Models;

namespace To_Do.Data
{
    public interface IApplicationRepository
    {
        Task SaveChanges();
        void AddNewToDo(ToDoTask todo);
        void UpdateTask(ToDoTask todo);
        void DeleteTodo(ToDoTask todo);
        ToDoTask GetTodoById(int id);
        Task<IEnumerable<ToDoTask>> GetTodos(string cuid);
        IEnumerable<ToDoTask> GetTodosByFolderId(int id);
        Task<IEnumerable<ToDoTask>> GetTodosByUserId(string uid);
        void AddFolder(Folder folder);
        IEnumerable<Folder> GetFoldersByUserId(string uid);
        Folder GetFolderById(int? id);
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

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        public void DeleteTodo(ToDoTask todo)
        {
            _context.TodoTasks.Remove(todo);
        }
        public ToDoTask GetTodoById(int id)
        {
            ToDoTask task = _context.TodoTasks
                .AsNoTracking().FirstOrDefault(x => x.Id == id);
            return task;
            //return _context.TodoTasks.Find(id);
        }

        public async Task<IEnumerable<ToDoTask>> GetTodos(string cuid)
        {
            return await _context.TodoTasks.Where(t => t.OwnerID == cuid).ToListAsync();
        }

        public async Task<IEnumerable<ToDoTask>> GetTodosByUserId(string uid)
        {
            return await _context.TodoTasks.Where(task => task.OwnerID == uid).ToListAsync();
        }

        public void UpdateTask(ToDoTask todo)
        {
            _context.Attach(todo).State= EntityState.Modified;
            //_context.TodoTasks.Update(todo);
        }
        public void AddFolder(Folder folder)
        {
            _context.Folders.Add(folder);
        }
        public IEnumerable<Folder> GetFoldersByUserId(string uid)
        {
            return _context.Folders.Where(f => f.OwnerID == uid).ToList();
        }

        public Folder GetFolderById(int? id)
        {
            return _context.Folders.FirstOrDefault(x => x.Id==id);
        }

        public IEnumerable<ToDoTask> GetTodosByFolderId(int id)
        {
            IEnumerable<ToDoTask> todos = _context.TodoTasks.Where(x => x.FolderId == id).ToList();
            return todos;
        }

    }
}
