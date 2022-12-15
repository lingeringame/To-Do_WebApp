using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using To_Do.Data;
using To_Do.Models;
using To_Do.ViewModels;

namespace To_Do.Controllers
{
    public class ToDoTaskController : Controller
    {
        private ApplicationRepository _repo;
        public ToDoTaskController(ApplicationDbContext dbContext)
        {
            _repo = new ApplicationRepository(dbContext);
        }

        //GET /<controller>/
        public IActionResult Index(List<ToDoTask> searchSet = null)
        {
            ViewBag.title = "Tasks";
            IComparer<ToDoTask> comparer = new ToDoTaskComparer();
            //we can't set default value to _repo.Getodos, so if its null, we set the default value
            if(!searchSet.Any())
            {
                List<ToDoTask> todos = _repo.GetTodos().ToList();
                todos.Sort(comparer);
                return View(todos);
            }
            else
            {
                ViewBag.userInput = "default";
                searchSet.Sort(comparer);
                return View(searchSet);
            }
        }

        //GET /<controller>/Add
        public IActionResult Add()
        {
            List<Folder> folders = _repo.GetFolders().ToList();
            return View(new AddToDoTaskViewModel(folders));
        }

        //POST /<controller>/Add
        [HttpPost]
        public IActionResult Add(AddToDoTaskViewModel viewModel)
        {
            Folder folder = _repo.GetFolderById(viewModel.FolderId);
            if(ModelState.IsValid)
            {
                ToDoTask newTask = new ToDoTask
                {
                    Title = viewModel.Title,
                    Body = viewModel.Body,
                    IsImportant = viewModel.IsImportant,
                    Folder= folder
                };
                _repo.AddNewToDo(newTask);
                _repo.SaveChanges();
                return Redirect("/todotask");
            }
            return Redirect("/todotask/add");
        }
        //GET /<controller>/EditTask
        public IActionResult EditTask(int id)
        {
            ViewBag.taskToEdit = _repo.GetTodoById(id); 
            //I pass in ViewBag.taskToEdit.Body because the body text is stored in a textarea and due to ASP.NET rules, I have to instantiate the viewModel with the
            //Body description so that it properly shows, instead of just putting a temporary placeholder attribute. 
            AddToDoTaskViewModel viewModel = new AddToDoTaskViewModel(ViewBag.taskToEdit.Body);
            return View(viewModel);
        }

        //POST /<controller>/EditTask
        [HttpPost]
        public IActionResult EditTask(AddToDoTaskViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                ToDoTask editedTask = new ToDoTask
                {
                    Id = viewModel.Id,
                    Title = viewModel.Title,
                    Body = viewModel.Body,
                    IsImportant = viewModel.IsImportant,
                    CreatedOn = _repo.GetTodoById(viewModel.Id).CreatedOn,
                    IsCompleted = viewModel.IsCompleted,
                    Folder = _repo.GetFolderById(viewModel.FolderId)
                };
                _repo.UpdateTask(editedTask);
                _repo.SaveChanges();
                return Redirect("/todotask");
            }
            return Redirect("/todotask/edit/" + viewModel.Id);
        }
        public IActionResult DeleteTask(int id)
        {
            _repo.DeleteTodo(id);
            _repo.SaveChanges();
            return Redirect("/todotask");
        }
        //GET /<controller>/BulkDelete
        public IActionResult BulkDelete()
        {
            List<ToDoTask> tasks = _repo.GetTodos().ToList();
            return View(tasks);
        }

        //POST /<controller>/BulkDelete
        [HttpPost]
        public IActionResult BulkDelete(int[] idsToRemove) //ids is empty. Need to debug this. checked checkboxes won't append their value to the array. 
        {
            foreach(int id in idsToRemove)
            {
                _repo.DeleteTodo(id);
            }
            _repo.SaveChanges();
            return Redirect("/todotask");
        }

        //GET /<controller>/Search
        [HttpPost]
        public IActionResult Search(string userInput)
        {
            List<ToDoTask> todos = _repo.GetTodos().ToList();
            List<ToDoTask> resultSet = new List<ToDoTask>();
            if(string.IsNullOrEmpty(userInput) || userInput.ToLower() == "all")
            {
                resultSet = todos;
            } else
            {
                foreach (ToDoTask todo in todos)
                {
                    if (todo.Title.Contains(userInput))
                    {
                        resultSet.Add(todo);
                    }
                }
            }
            return View("Index",resultSet);
        }
    }
}
