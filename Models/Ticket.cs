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
        [StringLength(500)]
        public string Description { get; set; }    // strings are nullable

        [StringLength(500)]
        public string Comments{ get; set; }

        [Required]
        public bool isOpen { get; set; }

        [Required]
        public Severity Severity { get; set; }

        [Required]
        public DateTime creationDate { get; set; }

        [Required]
        public DateTime lastModified { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

    }
}