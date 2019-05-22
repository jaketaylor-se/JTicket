﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTicket.Controllers;
using JTicket.Models;
using JTicket.Views.ViewModels;


namespace JTicket.Views
{
    public class OpenTicketController : TicketController
    {
       
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.CanManageTickets))
                return View("Index");

            return View("CreateOnlyIndex");
        }

        [Authorize(Roles = RoleName.CanManageTickets)]
        public ActionResult New()
        {
            var ViewModel = new TicketFormViewModel
            {
                severities = getSeverityList()
            };

            return View("TicketForm", ViewModel);
        }

        [HttpPost]   // Only callable by HTTP Post, not HTTP Get
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageTickets)]
        public ActionResult Save(Ticket ticket)
        {

            if (!ModelState.IsValid)    // Validation
            {
                var ViewModel = new TicketFormViewModel
                {
                    Ticket = ticket,   // The erroneous ticket
                    severities = getSeverityList()
                };
                return View("TicketForm", ViewModel);
            }

            if (ticket.Id == 0)    // This is a new ticket
            { 
                _context.Tickets.Add(ticket);   // Add change to transaction
            }
            else    // Ticket is in database
            {

                var ticketInDB = _context.Tickets.Single(t => t.Id == ticket.Id);

                /* I don't like TryUpdateModel(). It seems to introduce security
                 * issues because a malicious user could edit fields that maybe
                 * should not be edited. */

                ticketInDB.Title = ticket.Title;
                ticketInDB.Description = ticket.Description;
                ticketInDB.Comments = ticket.Comments;
                ticketInDB.Severity = ticket.Severity;
                ticketInDB.isOpen = ticket.isOpen;
            }

            _context.SaveChanges();    // Save changes wrapped in transaction

            return RedirectToAction("Index", "OpenTicket");
        }

        [Authorize(Roles = RoleName.CanManageTickets)]
        public ActionResult Edit(int id)
        {
            var ticket = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticket == null)
                return HttpNotFound();

            var ViewModel = new TicketFormViewModel
            {
                Ticket = ticket,
                severities = getSeverityList()
            };
            return View("TicketForm", ViewModel);    // Call the View to return the form
        }

        [Authorize(Roles = RoleName.CanManageTickets)]
        public ActionResult Resolve(int id)
        {
            var ticketInDB = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticketInDB == null)
                return HttpNotFound();

            ticketInDB.isOpen = false;
            _context.SaveChanges();
            
            return View("Index", "OpenTicket");    // Call the View to return the form
        }

        private IEnumerable<Severity> getSeverityList()
        {
            return Enum.GetValues(typeof(Severity)).Cast<Severity>().ToList();
        }
    }
}