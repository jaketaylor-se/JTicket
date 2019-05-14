using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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

        [Required]
        [Range(1, 5)]
        public Severity Severity { get; set; }
        public DateTime? creationDate { get; set; }
        public DateTime? lastModified { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

    }
}