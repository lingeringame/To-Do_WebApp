﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using To_Do.Authorization;
using To_Do.Data;
using To_Do.Models;
using To_Do.ViewModels;

namespace To_Do.Controllers
{
    public class ToDoTaskController : Controller
    {
        private ApplicationRepository _repo;
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        public ToDoTaskController(ApplicationDbContext dbContext, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _repo = new ApplicationRepository(dbContext);
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }

        //GET /<controller>/

        public async Task<IActionResult> Index(List<ToDoTask> searchSet = null)
        {
            ViewBag.title = "Tasks";
            IComparer<ToDoTask> comparer = new ToDoTaskComparer();
            //we can't set default value to _repo.Getodos, so if its null, we set the default value
            if(!searchSet.Any())
            {
                var currentUserId = UserManager.GetUserId(User);
                List<ToDoTask> todos = (List<ToDoTask>)await _repo.GetTodos(currentUserId);
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
        public async Task<IActionResult> Add(AddToDoTaskViewModel viewModel)
        {
            Folder folder = _repo.GetFolderById(viewModel.FolderId);
            if(ModelState.IsValid)
            {
                ToDoTask newTask = new ToDoTask
                {
                    Title = viewModel.Title,
                    Body = viewModel.Body,
                    IsImportant = viewModel.IsImportant,
                    Folder= folder,
                    OwnerID = UserManager.GetUserId(User)
                };
                
                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, newTask, TodoTaskOperations.Create);
                if(!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                _repo.AddNewToDo(newTask);
                await _repo.SaveChanges();
                return Redirect("/todotask/index");
            }
            return Redirect("/todotask/add");
        }
        //GET /<controller>/EditTask
        public async Task<IActionResult> EditTask(int id)
        {
            ToDoTask task = _repo.GetTodoById(id);

            if(task == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, task, TodoTaskOperations.Update);

            if(!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            ViewBag.taskToEdit = task;
            //I pass in ViewBag.taskToEdit.Body because the body text is stored in a textarea and due to ASP.NET rules, I have to instantiate the viewModel with the
            //Body description so that it properly shows, instead of just putting a temporary placeholder attribute. 
            List<Folder> folders = _repo.GetFolders().ToList();
            AddToDoTaskViewModel viewModel = new AddToDoTaskViewModel(ViewBag.taskToEdit.Body, folders);
            return View(viewModel);
        }

        //POST /<controller>/EditTask
        [HttpPost]
        public async Task<IActionResult> EditTask(AddToDoTaskViewModel viewModel)
        {
            if(ModelState.IsValid)
            {

                ToDoTask editedTaskOnDB = _repo.GetTodoById(viewModel.Id);
                if(editedTaskOnDB == null)
                {
                    return NotFound();
                }

                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, editedTaskOnDB, TodoTaskOperations.Update);

                if(!isAuthorized.Succeeded)
                {
                    return Forbid();
                }

                ToDoTask editedTask = new ToDoTask
                {
                    Id = viewModel.Id,
                    Title = viewModel.Title,
                    Body = viewModel.Body,
                    IsImportant = viewModel.IsImportant,
                    CreatedOn = _repo.GetTodoById(viewModel.Id).CreatedOn,
                    IsCompleted = viewModel.IsCompleted,
                    Folder = _repo.GetFolderById(viewModel.FolderId),
                    OwnerID = editedTaskOnDB.OwnerID
                };

                _repo.UpdateTask(editedTask);
                
                await _repo.SaveChanges();
                return Redirect("/todotask/index");
            }
            return Redirect("/todotask/edit/" + viewModel.Id);
        }
        public async Task<IActionResult> DeleteTask(int id)
        {
            ToDoTask todoToRemove = _repo.GetTodoById(id);
            if(todoToRemove == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, todoToRemove, TodoTaskOperations.Delete);

            if(!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            _repo.DeleteTodo(todoToRemove);
            await _repo.SaveChanges();
            return Redirect("/todotask/index");
        }
        //GET /<controller>/BulkDelete
        public async Task<IActionResult> BulkDelete()
        {
            var currentUserId = UserManager.GetUserId(User);
            List<ToDoTask> tasks = (List<ToDoTask>)await _repo.GetTodos(currentUserId);
            return View(tasks);
        }

        //POST /<controller>/BulkDelete
        [HttpPost]
        public async Task<IActionResult> BulkDelete(int[] idsToRemove) //ids is empty. Need to debug this. checked checkboxes won't append their value to the array. 
        {
            foreach(int id in idsToRemove)
            {
                ToDoTask todoToDelete = _repo.GetTodoById(id);
                if (todoToDelete == null)
                {
                    return NotFound();
                }

                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, todoToDelete, TodoTaskOperations.Delete);

                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                _repo.DeleteTodo(todoToDelete);
            }
            await _repo.SaveChanges();
            return Redirect("/todotask/index");
        }

        //GET /<controller>/Search
        [HttpPost]
        public async Task<IActionResult> Search(string userInput)
        {
            var currentUserId = UserManager.GetUserId(User);
            List<ToDoTask> todos = (List<ToDoTask>)await _repo.GetTodos(currentUserId);
            List<ToDoTask> resultSet = new List<ToDoTask>();
            if(string.IsNullOrEmpty(userInput) || userInput.ToLower() == "all")
            {
                resultSet = todos;
            } else
            {
                foreach (ToDoTask todo in todos)
                {
                    if (todo.Title.ToLower().Contains(userInput))
                    {
                        resultSet.Add(todo);
                    }
                }
            }
            return View("Index",resultSet);
        }
    }
}
