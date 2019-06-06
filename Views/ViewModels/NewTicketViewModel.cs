using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JTicket.Models;

namespace JTicket.Views.ViewModels
{
    public class TicketFormViewModel
    {
        public IEnumerable<Severity> Severities { get; set; }
        public Ticket Ticket { get; set; }
    }
}