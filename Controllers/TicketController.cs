/*
 *  Controller for the Ticket requests. Base class for the Controller
 *  class ResolvedTicketController and OpenTicketController.
 */

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;

using JTicket.Models;
using JTicket.Views.ViewModels;
using JTicket.Models.Enumerations;


namespace JTicket.Controllers
{

    public class TicketController : Controller
    {

        protected ApplicationDbContext _context;    // DB context


        public TicketController()
        {
            _context = new ApplicationDbContext();    // Initialize context
        }


        protected override void Dispose(bool disposing)  // Override 
        {
            _context.Dispose();
        }


        public ActionResult Details(int id)
        {
            var ticket = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticket == null)    // No ticket matching in database
                return HttpNotFound();

            return View(ticket);
        }


        public ActionResult ViewTicket(int id)
        {
            var ticket = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticket == null)    // No matching ticket in database
                return HttpNotFound();

            return View("ViewTicket", ticket);
        }


        public ActionResult ViewOpenTickets()
        {
            if (User.IsInRole(RoleName.HasFullPermissions))    // If admin
                return View("FullPermissionsOpenTickets");

            return View("CreateOnlyOpenTickets");    // If not admin, create only
        }


        public ActionResult ViewResolvedTickets()
        {
            if (User.IsInRole(RoleName.HasFullPermissions))    // If admin
                return View("FullPermissionsResolvedTickets");

            return View("ReadOnlyResolvedTickets");    // If not admin, create only
        }


 
        public ActionResult New()
        {
            var ViewModel = new TicketFormViewModel    // View Model for form
            {
                Severities = getSeverityList(),    // Severities for display
                Ticket = new Ticket()
            };

            return View("TicketForm", ViewModel);
        }


        [HttpPost]  
        [ValidateAntiForgeryToken]
        public ActionResult SaveNewTicket(Ticket ticket)
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

            DateTime theTime = DateTime.Now;
            ticket.CreationDate = theTime;
            ticket.LastModified = theTime;
            ticket.IsOpen = true;

            _context.Tickets.Add(ticket);   // Add change to transaction
            _context.SaveChanges();    // Save changes wrapped in transaction

            if (User.IsInRole(RoleName.HasFullPermissions))    // If admin
                return View("FullPermissionsOpenTickets");

            return View("CreateOnlyOpenTickets");    // If not admin, create only
        }


        [HttpPost]   // Only callable by HTTP Post, not HTTP Get
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.HasFullPermissions)]
        public ActionResult SaveExistingTicket(Ticket ticket)
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

            var ticketInDB = _context
                            .Tickets
                            .Single(t => t.Id == ticket.Id);

            ticketInDB.Title = ticket.Title;    // Swap attribute values
            ticketInDB.Description = ticket.Description;
            ticketInDB.Comments = ticket.Comments;
            ticketInDB.Severity = ticket.Severity;
            ticketInDB.IsOpen = ticket.IsOpen;
            ticketInDB.LastModified = DateTime.Now;
            
            _context.SaveChanges();    // Save changes wrapped in transaction

            return View("FullPermissionsOpenTickets");
        }


        [Authorize(Roles = RoleName.HasFullPermissions)]
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


        [Authorize(Roles = RoleName.HasFullPermissions)]
        public ActionResult Resolve(int id)
        {
            var ticketInDB = _context
                             .Tickets
                             .SingleOrDefault(t => t.Id == id);

            if (ticketInDB == null)    // No matching ticket
                return HttpNotFound();

            ticketInDB.IsOpen = false;    // Close ticket
            _context.SaveChanges();       // Save changes

            return View("FullPermissionsOpenTickets", "Ticket");
        }

        public ActionResult ViewKanbanBoard()
        {
            return View("KanbanBoard");
        }


        private IEnumerable<TicketSeverity> getSeverityList()
        {
            return System.Enum.GetValues(typeof(TicketSeverity)).Cast<TicketSeverity>().ToList();
        }

    }
}