/*
 *  Data Transfer Class for Tickets
 */


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using JTicket.Models;
using JTicket.Models.Enumerations;

namespace JTicket.Dtos
{
    /// <summary>
    /// Class 
    /// <c>TicketDto</c> 
    /// Data Transfer Class for Ticket Domain Model objects.
    /// </summary>
    public class TicketDto
    {
        [StringLength(500)]
        public string Description { get; set; }    // strings are nullable

        [StringLength(500)]
        public string Comments { get; set; }

        [Required]
        public bool isOpen { get; set; }

        [Required]
        public DateTime creationDate { get; set; }

        [Required]
        public DateTime lastModified { get; set; }

        [Required]
        [Range(1, 5)]
        public TicketSeverity Severity { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [Range(0, 4)]
        public TicketState State { get; set; }

    }
}
