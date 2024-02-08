using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoListWeb.Data;
using ToDoListWeb.Filters;
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
        public IActionResult Index(string? id)
        {
            var filters = new FiltersTasks(id);
            ViewBag.Filters = filters;
            ViewBag.Categories = db.Categories;
            ViewBag.Statuses = db.Statuses;
            ViewBag.DueFilters = FiltersTasks.DueFilterValues;

            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            IQueryable<Tasks> query = db.Tasks
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Where(t => t.UserId == user.Id);

            if (filters.HasCategory)
            {
                query = query.Where(t => t.CategoryId == filters.CategoryId);
            }
            if (filters.HasStatus)
            {
                query = query.Where(t => t.StatusId == filters.StatusId);
            }
            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                {
                    query = query.Where(t => t.DueDate < today);
                }
                else if (filters.IsToday)
                {
                    query = query.Where(t => t.DueDate == today);
                }
                else if (filters.IsFuture)
                {
                    query = query.Where(t => t.DueDate > today);
                }
            }
            var tasks = query.OrderBy(t => t.DueDate).ToList();

            return View(tasks);
        }
        // GET
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ViewBag.Categories = db.Categories;
            ViewBag.Statuses = db.Statuses;
            var categoryFormDb = db.Tasks.Find(id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }
        // GET
        [Authorize(Policy = PolicyAccess.CreateClaim)]
        public IActionResult Create()
        {
            ViewBag.Categories = db.Categories;
            ViewBag.Statuses = db.Statuses;
            return View();
        }
        // POST
        [HttpPost]
        [ClaimRequirements(Claims.Create)]
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
        [Authorize(Policy = PolicyAccess.EditClaim)]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ViewBag.Categories = db.Categories;
            ViewBag.Statuses = db.Statuses;
            var categoryFormDb = db.Tasks.Find(id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }
        // POST
        [HttpPost]
        [ClaimRequirements(Claims.Edit)]
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
        [Authorize(Policy = PolicyAccess.DeleteClaim)]
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
        [ClaimRequirements(Claims.Delete)]
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
        // POST
        [HttpPost]
        public IActionResult IsDone(int? id)
        {
            var item = db.Tasks.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            if (item.StatusId == "open")
            {
                // task's status changed by completed
                item.StatusId = "closed";
                TempData["success"] = "Task completed successfully.";
            }
            else if (item.StatusId == "closed")
            {
                // task's status changed by open
                item.StatusId = "open";
                TempData["success"] = "Task open successfully.";
            }
            db.Tasks.Update(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction("Index", new { ID = id });
        }
    }
}
