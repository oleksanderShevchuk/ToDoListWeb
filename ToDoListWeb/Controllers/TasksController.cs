using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ToDoListWeb.Data;
using ToDoListWeb.Models;

namespace ToDoListWeb.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<IdentityUser> _userManager;
        public TasksController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            //IEnumerable<Tasks> CatagoreList = db.Tasks;
            List<Tasks> CatagoreList = new List<Tasks>();
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            foreach (var item in db.Tasks)
            {
                if (item.UserId == user.Id)
                {
                    CatagoreList.Add(item);
                }
            }
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
        public async Task<IActionResult> Create(Tasks category)
        {
            //var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var user = await _userManager.GetUserAsync(User);
            category.UserId = user.Id.ToString();

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
        public async Task<IActionResult> Edit(Tasks category)
        {
            var user = await _userManager.GetUserAsync(User);
            category.UserId = user.Id.ToString();
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
