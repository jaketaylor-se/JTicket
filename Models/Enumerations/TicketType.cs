using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JTicket.Models.Enumerations
{
    public enum TicketType
    {
        [Display(Name = "Bug")]
        bug = 0,

        [Display(Name = "Story")]
        story = 1
    }
}