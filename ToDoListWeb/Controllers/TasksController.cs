using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListWeb.Data;
using ToDoListWeb.Models;

namespace ToDoListWeb.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext db;
        public TasksController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Tasks> CatagoreList = db.Tasks;
            return View(CatagoreList);
        }
        // GET
        public IActionResult Create()
        {
            return View();
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tasks category)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(category);
                db.SaveChanges();
                TempData["success"] = "Task created successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFormDb = db.Tasks.Find(id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tasks category)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Update(category);
                db.SaveChanges();
                TempData["success"] = "Task update successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        // GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFormDb = db.Tasks.Find(id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }
        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var item = db.Tasks.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            db.Tasks.Remove(item);
            db.SaveChanges();
            TempData["success"] = "Task deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
