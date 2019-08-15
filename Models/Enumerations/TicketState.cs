using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JTicket.Models.Enumerations
{
    public enum TicketState
    {
        open = 0,
        analysis = 1,
        debugging = 2,
        testing = 3,
        resolved = 4
    }
}