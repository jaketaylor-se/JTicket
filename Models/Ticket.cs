using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace JTicket.Models
{
    public enum Severity
    {
        VeryLow = 1,
        Low = 2,
        Medium = 3,
        High = 4,
        VeryHigh = 5
    }

    public class Ticket
    {
        public string Description { get; set; }
        public string Comments { get; set; }
        public bool isOpen { get; set; }
        public Severity severity { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime lastModified { get; set; }
        public byte Id { get; set; }
        public string Title { get; set; }

    }
}