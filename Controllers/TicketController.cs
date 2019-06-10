/*
 *  Controller for the Ticket requests. Base class for the Controller
 *  class ResolvedTicketController and OpenTicketController.
 */

using System.Linq;
using System.Web.Mvc;
using System;
using System.Collections.Generic;

using JTicket.Models;
using JTicket.Views.ViewModels;


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


        public ActionResult ViewOpenTickets()
        {
            if (User.IsInRole(RoleName.CanManageTickets))    // If admin
                return View("FullPermissionsViewOpenTickets");

            return View("CreateOnlyViewOpenTickets");    // If not admin, create only
        }

        /// <summary>
        /// Method 
        /// <c>New</c> 
        /// Return the View for the new ticket form.
        /// </summary>
        [Authorize]
        public ActionResult New()
        {
            DateTime CurrentTime = DateTime.UtcNow;    // This is not exact creation time...
            var ViewModel = new TicketFormViewModel    // View Model for form
            {
                Severities = getSeverityList(),    // Severities for display
                Ticket = Ticket.CreateEmptyOpenTicket(CreationDateTime: CurrentTime,
                                                      LastModifiedDateTime: CurrentTime)
            };

            return View("TicketForm", ViewModel);
        }


        /// <summary>
        /// Method 
        /// <c>SaveNew</c> 
        /// Save a NEW ticket to the database if valid.
        /// If invalid, redirect back to ticket form.
        /// </summary>
        [HttpPost]  
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult SaveNew(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                var ViewModel = new TicketFormViewModel
                {
                    Ticket = ticket,   // The erroneous ticket
                    Severities = getSeverityList()
                };

                return View("TicketForm", ViewModel);  // Send back form
            }

            _context.Tickets.Add(ticket);   // Add change to transaction
            _context.SaveChanges();        // Save changes wrapped in transaction

            return RedirectToAction("ViewOpenTickets", "Ticket");
        }

        /// <summary>
        /// Method 
        /// <c>Save</c> 
        /// Save the changes to an existing ticket to 
        /// the database if valid.
        /// If invalid, redirect back to ticket form.
        /// </summary>
        [HttpPost]   
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageTickets)]
        public ActionResult SaveChanges(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                var ViewModel = new TicketFormViewModel
                {
                    Ticket = ticket,   // The erroneous ticket
                    Severities = getSeverityList()
                };
                return View("TicketForm", ViewModel);  // Send back form
            }

            // NOTE: Future implement automapper for this
            var ticketInDB = _context
                            .Tickets
                            .Single(t => t.Id == ticket.Id);

            ticketInDB.Title = ticket.Title;    // Swap attribute values
            ticketInDB.Description = ticket.Description;
            ticketInDB.Comments = ticket.Comments;
            ticketInDB.Severity = ticket.Severity;
            ticketInDB.IsOpen = ticket.IsOpen;

            _context.SaveChanges();    // Save changes wrapped in transaction

            return RedirectToAction("ViewOpenTickets", "Ticket");
        }

        /// <summary>
        /// Method 
        /// <c>Edit</c> 
        /// Edit an existing ticket from the database.
        /// </summary>
        [Authorize(Roles = RoleName.CanManageTickets)]
        public ActionResult Edit(int id)
        {
            var ticket = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticket == null)    // No matching ticket in database
                return HttpNotFound();

            var ViewModel = new TicketFormViewModel    // Create view model
            {
                Ticket = ticket,
                Severities = getSeverityList()
            };
            return View("TicketForm", ViewModel);
        }

        /// <summary>
        /// Method 
        /// <c>Resolve</c> 
        /// Close an active ticket.
        /// </summary>
        [Authorize(Roles = RoleName.CanManageTickets)]
        public ActionResult Resolve(int id)
        {
            var ticketInDB = _context
                             .Tickets
                             .SingleOrDefault(t => t.Id == id);

            if (ticketInDB == null)    // No matching ticket
                return HttpNotFound();

            ticketInDB.IsOpen = false;    // Close ticket
            _context.SaveChanges();       // Save changes

            return View("ViewOpenTickets", "Ticket");
        }

        
        /// <summary>
        /// Method 
        /// <c>Index</c> 
        /// Return the View for displaying resolved tickets.
        /// </summary>
        public ActionResult ViewResolvedTickets()
        {
            if (User.IsInRole(RoleName.CanManageTickets))    // if admin
                return View("FullPermissionsViewResolvedTickets");

            return View("ReadOnlyViewResolvedTickets");    // if not admin, read only
        }

        /// <summary>
        /// Method 
        /// <c>Save</c> 
        /// Helper method to turn Severity enum into List for displays.
        /// </summary>
        private IEnumerable<Severity> getSeverityList()
        {
            return Enum.GetValues(typeof(Severity)).Cast<Severity>().ToList();
        }
    }

}
