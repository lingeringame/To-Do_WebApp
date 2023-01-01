using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using To_Do.Areas.Identity.Data;
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
        protected UserManager<To_DoUser> UserManager { get; }
        public ToDoTaskController(ApplicationDbContext dbContext, IAuthorizationService authorizationService, UserManager<To_DoUser> userManager)
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
                ViewBag.title = "All tasks " + "(" + todos.Count() + ")";
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
        public IActionResult Add(int? folderId = null)
        {
            ViewBag.folderId = folderId;
            List<Folder> folders = _repo.GetFoldersByUserId(UserManager.GetUserId(User)).ToList();
            return View(new AddToDoTaskViewModel(folders));
        }

        //POST /<controller>/Add
        [HttpPost]
        public async Task<IActionResult> Add(AddToDoTaskViewModel viewModel, int? folderId = null)
        {
            Folder folder;
            int? folderIdVal = null;
            if(folderId != null)
            {
                //User created task while in a folder, use that folder by default
                folder = _repo.GetFolderById(folderId);  
                folderIdVal = folder.Id;
            }

            if (ModelState.IsValid)
            {
                ToDoTask newTask = new ToDoTask
                {
                    Title = viewModel.Title,
                    Body = viewModel.Body,
                    IsImportant = viewModel.IsImportant,
                    FolderId= folderIdVal, 
                    OwnerID = UserManager.GetUserId(User)
                };
                
                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, newTask, TodoTaskOperations.Create);
                if(!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                _repo.AddNewToDo(newTask);
                await _repo.SaveChanges();
                return RedirectToAction("Results", "Folder", new {id = folderIdVal});
            }
            return Redirect("/todotask/add");
        }
        //GET /<controller>/EditTask
        public async Task<IActionResult> EditTask(int id, int? folderId = null)
        {
            ToDoTask task = _repo.GetTodoById(id);
            ViewBag.folderId_e = null;
            if(folderId != null)
            {
                ViewBag.folderId_e = task.FolderId;
            }
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
            List<Folder> folders = _repo.GetFoldersByUserId(UserManager.GetUserId(User)).ToList();
            AddToDoTaskViewModel viewModel = new AddToDoTaskViewModel(ViewBag.taskToEdit.Body, folders);
            return View(viewModel);
        }

        //POST /<controller>/EditTask
        [HttpPost]
        public async Task<IActionResult> EditTask(AddToDoTaskViewModel viewModel, int? folderId_e = null)
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
                    FolderId = viewModel.FolderId,
                    OwnerID = editedTaskOnDB.OwnerID
                };

                _repo.UpdateTask(editedTask);
                
                await _repo.SaveChanges();
                return RedirectToAction("Results","Folder", new {id = folderId_e});
            }
            return Redirect("/todotask/edit/" + viewModel.Id);
        }
        public async Task<IActionResult> DeleteTask(int id, int? folderId = null)
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
            return RedirectToAction("Results", "Folder", new {id = folderId});
        }
        //GET /<controller>/BulkDelete
        public async Task<IActionResult> BulkDelete(int? folderId = null)
        {
            var currentUserId = string.Empty;
            List<ToDoTask> tasks;
            if(folderId == null)
            {
                currentUserId = UserManager.GetUserId(User);
                tasks = (List<ToDoTask>)await _repo.GetTodos(currentUserId);
            } else
            {
                int folderIdNotNull = (int)folderId;
                tasks = _repo.GetTodosByFolderId(folderIdNotNull).ToList();
            }
            ViewBag.folderId_bd = folderId;
            return View(tasks);
        }

        //POST /<controller>/BulkDelete
        [HttpPost]
        public async Task<IActionResult> BulkDelete(int[] idsToRemove, int? folderId_bd = null) //ids is empty. Need to debug this. checked checkboxes won't append their value to the array. 
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
            return RedirectToAction("Results", "Folder", new {id = folderId_bd});
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

        [HttpPost]
        public async Task<IActionResult> EditComplete([Bind("Id,IsCompleted")]ToDoTask task)
        {
            int taskId = task.Id;
            bool isCompleted = task.IsCompleted;
            ToDoTask theTask = _repo.GetTodoById(taskId);
            if(theTask == null)
            {
                return NotFound();
            }
            theTask.IsCompleted = isCompleted;
            _repo.UpdateTask(theTask);
            await _repo.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}
