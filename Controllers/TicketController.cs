/*
 *  Controller for the Ticket requests. Base class for the Controller
 *  class ResolvedTicketController and OpenTicketController.
 */

using System.Linq;
using System.Web.Mvc;

using JTicket.Models;


namespace JTicket.Controllers
{
    /// <summary>
    /// Class 
    /// <c>TicketController</c> 
    /// Base class for Controllers that handle ticket-related requests.
    /// </summary>
    public class TicketController : Controller
    {

        protected ApplicationDbContext _context;    // DB context

        /// <summary>
        /// Method 
        /// <c>TicketController</c> 
        /// Constructor for the TicketController class.
        /// </summary>
        public TicketController()
        {
            _context = new ApplicationDbContext();    // Initialize context
        }

        /// <summary>
        /// Method 
        /// <c>Dispose</c> 
        /// Dispose the context and close database connection.
        /// </summary>
        protected override void Dispose(bool disposing)  // Override 
        {
            _context.Dispose();
        }

        /// <summary>
        /// Method 
        /// <c>Details</c> 
        /// Return the View for displaying ticket comments and descriptions.
        /// </summary>
        public ActionResult Details(int id)
        {
            var ticket = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticket == null)    // No ticket matching in database
                return HttpNotFound();

            return View(ticket);
        }

        /// <summary>
        /// Method 
        /// <c>ViewTicket</c> 
        /// Return the View for displaying full ticket details.
        /// </summary>
        public ActionResult ViewTicket(int id)
        {
            var ticket = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticket == null)    // No matching ticket in database
                return HttpNotFound();

            return View("ViewTicket", ticket);   
        }

    }
}