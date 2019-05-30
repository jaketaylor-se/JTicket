using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTicket.Models;


namespace JTicket.Controllers
{
    public class TicketController : Controller
    {

        protected ApplicationDbContext _context;    // DB Context for DB access

        public TicketController()
        {
            _context = new ApplicationDbContext();    // Initialize DB context
        }

        protected override void Dispose(bool disposing)  // Override base class 
        {
            _context.Dispose();    // Disposable object
        }

        public ActionResult Details(int id)
        {
            var customer = _context.Tickets.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        public ActionResult ViewTicket(int id)
        {
            var ticket = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticket == null)
                return HttpNotFound();

            return View("ViewTicket", ticket);    // Call the View to return the form
        }

    }
}