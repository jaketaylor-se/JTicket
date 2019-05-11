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

    }
}