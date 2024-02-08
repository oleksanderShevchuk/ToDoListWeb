namespace ToDoListWeb.Data
{
    /// <summary>
    /// class for policy name
    /// </summary>
    public static class PolicyAccess
    {
        /// <summary>
        /// Available to users who have rights to create
        /// </summary>
        public const string CreateClaim = "CreateAccess";
        /// <summary>
        /// Available to users who have rights to delete
        /// </summary>
        public const string DeleteClaim = "DeleteAccess";
        /// <summary>
        /// Available to users who have rights to edit
        /// </summary>
        public const string EditClaim = "EditAccess";
        /// <summary>
        /// Available to users who have rights to LockUnlock User
        /// </summary>
        public const string LockUnlockClaim = "LockUnlockAccess";
        /// <summary>
        /// Available to users who have rights to edit users or claims,
        /// block and unblock, delete or a user with the Admin role
        /// </summary>
        public const string UserClaimOrAdmin = "CreateOrEditOrDeleteAccessOrAdmin";
    }
}
