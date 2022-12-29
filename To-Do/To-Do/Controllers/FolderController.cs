﻿using Microsoft.AspNetCore.Mvc;
using To_Do.Data;
using To_Do.ViewModels;
using To_Do.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using To_Do.Authorization;
using To_Do.Areas.Identity.Data;

namespace To_Do.Controllers
{
    public class FolderController : Controller
    {
        private ApplicationRepository _repo;
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<To_DoUser> UserManager { get; }
        public FolderController(ApplicationDbContext dbContext, IAuthorizationService authorizationService, UserManager<To_DoUser> userManager)
        {
            _repo= new ApplicationRepository(dbContext);
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var uid = UserManager.GetUserId(User);
            List<Folder> folders = _repo.GetFoldersByUserId(uid).ToList();
            Dictionary<Folder,int> FolderCountPairs = new Dictionary<Folder,int>();
            ViewBag.TotalTasks = ((List<ToDoTask>)await _repo.GetTodosByUserId(uid)).Count();
            foreach(Folder folder in folders)
            {
                FolderCountPairs.Add(folder, _repo.GetTodosByFolderId(folder.Id).ToList().Count());
            }
            ViewBag.title = "Folders";
            return View(FolderCountPairs);
        }
        //GET /<controller>/Add
        public IActionResult Add()
        {
            return View(new AddFolderViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddFolderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Folder newFolder = new Folder
                {
                    Name = viewModel.Name,
                    CreatedOn = DateTime.Now,
                    OwnerID = UserManager.GetUserId(User)
                };
                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, newFolder, TodoTaskOperations.Create);
                if(!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                _repo.AddFolder(newFolder);
                await _repo.SaveChanges();
                return Redirect("/folder/index");
            }
            return Redirect("/folder/add");
        }
        //GET /<controller>/Results
        [HttpGet]
        public IActionResult Results(int id)
        {
            if (ModelState.IsValid)
            {
                //should i add user id as well for extra security? or is it redundant?
                List<ToDoTask> folderTasks = _repo.GetTodosByFolderId(id).ToList();
                ViewBag.folderId = id;
                ViewBag.title = _repo.GetFolderById(id).Name + " (" + folderTasks.Count() + ")";
                return View("../ToDoTask/Index", folderTasks);
            }
            return View();
        }
    }
}
