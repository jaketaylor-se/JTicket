using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JTicket.Controllers;

namespace JTicket.Views
{
    public class ResolvedTicketController : TicketController
    {

        public ActionResult Index()
        {
            var tickets = _context.Tickets;   // Deferred execution unless ToList()

            return View(tickets.Where(d => d.isOpen == false));
        }


    }
}