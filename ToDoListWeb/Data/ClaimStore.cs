using System.Security.Claims;

namespace ToDoListWeb.Data
{
    public static class ClaimStore
    {
        public static List<Claim> claimsList = new List<Claim>()
        {
            new Claim("Create", "Create"),
            new Claim("Edit", "Edit"),
            new Claim("Delete", "Delete"),
            new Claim("Index", "Index"),
            new Claim("LockUnlock", "LockUnlock"),
            new Claim("ManageUserClaims", "ManageUserClaims"),
            new Claim("Upsert", "Upsert"),
        };
    }
}
