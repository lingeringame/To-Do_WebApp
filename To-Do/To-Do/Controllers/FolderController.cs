using Microsoft.AspNetCore.Mvc;
using To_Do.Data;
using To_Do.ViewModels;
using To_Do.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace To_Do.Controllers
{
    public class FolderController : Controller
    {
        private ApplicationRepository _repo;
        public FolderController(ApplicationDbContext dbContext)
        {
            _repo= new ApplicationRepository(dbContext);
        }
        public IActionResult Index()
        {
            List<Folder> folders = _repo.GetFolders().ToList();
            ViewBag.title = "Folders";
            return View(folders);
        }
        //GET /<controller>/Add
        public IActionResult Add()
        {
            return View(new AddFolderViewModel());
        }

        [HttpPost]
        public IActionResult Add(AddFolderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Folder newFolder = new Folder
                {
                    Name = viewModel.Name,
                    CreatedOn = DateTime.Now
                };
                _repo.AddFolder(newFolder);
                _repo.SaveChanges();
                return Redirect("/folder");
            }
            return Redirect("/folder/add");
        }
        //GET /<controller>/Results
        public IActionResult Results(int id)
        {
            if (ModelState.IsValid)
            {
                List<ToDoTask> folderTasks = _repo.GetTodosByFolderId(id).ToList();
                return View("../ToDoTask/Index", folderTasks);
            }
            return View();
        }
    }
}
