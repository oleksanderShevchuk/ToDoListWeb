using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index(string? currentFilter, string? searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var CatagoreList = from c in db.Tasks.Where(t => t.UserId == user.Id)
                           select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                CatagoreList = CatagoreList.Where(s => s.Name.Contains(searchString));
            }

            int pageSize = 3;
            return View(await PaginatedList<Tasks>.CreateAsync(CatagoreList.AsNoTracking(), pageNumber ?? 1, pageSize));
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

        public async Task<IActionResult> NoteIndex(int? id)
        {
            var task = await db.Tasks.FindAsync(id);
            return View(task);
        }

        // GET
        public IActionResult NoteEdit(int? id)
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
        public async Task<IActionResult> NoteEditAsync(Tasks category)
        {
            var user = await _userManager.GetUserAsync(User);
            category.UserId = user.Id.ToString();
            db.Tasks.Update(category);
            db.SaveChanges();
            return View(category);
        }
        // GET
        public IActionResult NoteDelete(int? id)
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
        [HttpPost, ActionName("NoteDelete")]
        [ValidateAntiForgeryToken]
        public IActionResult NoteDeletePost(int? id)
        {

            var item = db.Tasks.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            item.NoteName = "None";
            db.Tasks.Update(item);
            db.SaveChanges();
            return RedirectToAction("NoteIndex");
        }
    }
}
