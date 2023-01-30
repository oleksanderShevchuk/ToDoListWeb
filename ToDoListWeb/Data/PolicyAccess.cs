namespace ToDoListWeb.Data
{
    /// <summary>
    /// class for policy name
    /// </summary>
    public static class PolicyAccess
    {
        /// <summary>
        /// Available to users who have rights to edit users or claims,
        /// block and unblock, delete or a user with the Admin role
        /// </summary>
        public const string UserClaimOrAdmin = "CreateOrEditOrDeleteAccessOrAdmin";
    }
}
