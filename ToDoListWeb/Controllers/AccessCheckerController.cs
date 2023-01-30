using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListWeb.Data;

namespace ToDoListWeb.Controllers
{
    [Authorize]
    public class AccessCheckerController : Controller
    {
        [AllowAnonymous]
        //Accessible by everyone, even if users are not logged in.
        public IActionResult AllAccess()
        {
            return View();
        }

        [Authorize(Roles = RoleAccess.User)]
        //Accessible by users who have user role
        public IActionResult UserAccess()
        {
            return View();
        }

        [Authorize(Roles = RoleAccess.Admin)]
        //Accessible by users who have admin role
        public IActionResult AdminAccess()
        {
            return View();
        }

        // Available to users who have rights to edit users or claims,
        // block and unblock, delete or a user with the Admin role
        [Authorize(Policy = PolicyAccess.UserClaimOrAdmin)]
        public IActionResult CreateOrEditOrDeleteAccessOrAdmin()
        {
            return View();
        }
    }
}
