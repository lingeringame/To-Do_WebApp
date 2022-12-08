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
        public IActionResult Index()
        {
            ViewBag.title = "Your tasks";
            List<ToDoTask> todos = _repo.GetTodos().ToList();
            IComparer<ToDoTask> comparer = new ToDoTaskComparer();
            todos.Sort(comparer);
            return View(todos);
        }

        //GET /<controller>/Add
        public IActionResult Add()
        {
            return View(new AddToDoTaskViewModel());
        }

        //POST /<controller>/Add
        [HttpPost]
        public IActionResult Add(AddToDoTaskViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                ToDoTask newTask = new ToDoTask
                {
                    Title = viewModel.Title,
                    Body = viewModel.Body,
                    isImportant = viewModel.isImportant
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
            AddToDoTaskViewModel viewModel = new AddToDoTaskViewModel();
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
                    Body = viewModel.Body
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
    }
}
