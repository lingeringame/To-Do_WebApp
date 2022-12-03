using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
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
                    Body = viewModel.Body
                };
                _repo.AddNewToDo(newTask);
                _repo.SaveChanges();
                return Redirect("/todotask");
            }
            return Redirect("/todotask/add");
        }
    }
}
