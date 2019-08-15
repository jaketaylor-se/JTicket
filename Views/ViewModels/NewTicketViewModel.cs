using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JTicket.Models;
using JTicket.Models.Enumerations;

namespace JTicket.Views.ViewModels
{
    public class TicketFormViewModel
    {
        public IEnumerable<TicketSeverity> Severities { get; set; }
        public Ticket Ticket { get; set; }
    }
}