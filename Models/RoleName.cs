/*
 *  Domain Model for RoleNames. Intent is to avoid string or 
 *  numeric literals, and instead incapsulate the values in a
 *  static class.
 */

namespace JTicket.Models
{
    /// <summary>
    /// Class 
    /// <c>RoleName</c> 
    /// Static class for encapsulating different roles for permissions.
    /// </summary>
    public static class RoleName
    {
        // admin permissions
        public const string CanManageTickets = "CanManageResolvedTickets";
    }
}
