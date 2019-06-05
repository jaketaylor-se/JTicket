/*
 *  Controller for Resolved ticket requests.
 */

using System.Web.Mvc;

using JTicket.Controllers;
using JTicket.Models;

namespace JTicket.Views
{
    /// <summary>
    /// Class 
    /// <c>ResolvedTicketController</c> 
    /// Controller for Resolved Ticket requests.
    /// </summary>
    public class ResolvedTicketController : TicketController
    {
        /// <summary>
        /// Method 
        /// <c>Index</c> 
        /// Return the View for displaying resolved tickets.
        /// </summary>
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.CanManageTickets))    // if admin
                return View("Index");

            return View("ReadOnlyIndex");    // if not admin, read only
        }
    }
}