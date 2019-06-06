/*
 *  Controller for handling Open Tickets related requests.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using JTicket.Controllers;
using JTicket.Models;
using JTicket.Views.ViewModels;


namespace JTicket.Views
{
    /// <summary>
    /// Class 
    /// <c>OpenTicketController</c> 
    /// Controller for Open Ticket requests.
    /// </summary>
    public class OpenTicketController : TicketController
    {
        /// <summary>
        /// Method 
        /// <c>Index</c> 
        /// Return the View for displaying open tickets.
        /// </summary>
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.CanManageTickets))    // If admin
                return View("Index");

            return View("CreateOnlyIndex");    // If not admin, create only
        }


        /// <summary>
        /// Method 
        /// <c>New</c> 
        /// Return the View for the new ticket form.
        /// </summary>
        [Authorize(Roles = RoleName.CanManageTickets)]
        public ActionResult New()
        {
            var ViewModel = new TicketFormViewModel    // View Model for form
            {
                Severities = getSeverityList(),    // Severities for display
                Ticket = new Ticket()
            };

            return View("TicketForm", ViewModel);
        }

        /// <summary>
        /// Method 
        /// <c>Save</c> 
        /// Save the ticket to the database if valid.
        /// If invalid, redirect back to ticket form.
        /// </summary>
        [HttpPost]   // Only callable by HTTP Post, not HTTP Get
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageTickets)]
        public ActionResult Save(Ticket ticket)
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

            if (ticket.Id == 0)                 // This is a new ticket
            { 
                _context.Tickets.Add(ticket);   // Add change to transaction
            }
            else    // Ticket is in database
            {
                var ticketInDB = _context
                                .Tickets
                                .Single(t => t.Id == ticket.Id);

                ticketInDB.Title = ticket.Title;    // Swap attribute values
                ticketInDB.Description = ticket.Description;
                ticketInDB.Comments = ticket.Comments;
                ticketInDB.Severity = ticket.Severity;
                ticketInDB.IsOpen = ticket.IsOpen;
            }

            _context.SaveChanges();    // Save changes wrapped in transaction

            return RedirectToAction("Index", "OpenTicket");
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
            
            return View("Index", "OpenTicket"); 
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
