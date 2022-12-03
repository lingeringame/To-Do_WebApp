using Microsoft.AspNetCore.Mvc;
using To_Do.Data;

namespace To_Do.Controllers
{
    public class DeleteController : Controller
    {
        private ApplicationRepository _repo;
        public DeleteController(ApplicationDbContext dbContext)
        {
            _repo = new ApplicationRepository(dbContext);
        }
        public IActionResult Index()
        {
            return View();
        }
        //GET /<controller>/DeleteTask
        public IActionResult DeleteTask(int id)
        {
            _repo.DeleteTodo(id);
            _repo.SaveChanges();
            return Redirect("/todotask");
        }
    }
}
