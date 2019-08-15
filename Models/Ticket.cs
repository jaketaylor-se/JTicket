/*
 *  Domain Model for the base class Ticket. Note: This class 
 *  will be the only Domain Model for tickets in future
 *  versions. 
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using JTicket.Models.Enumerations;

namespace JTicket.Models
{
    public class Ticket
    {
        [StringLength(500)]
        public string Description { get; set; }  // descriptions are optional

        [StringLength(500)]
        public string Comments{ get; set; }  // comments are optional

        [Required]
        public bool IsOpen { get; set; }  // resolved/active state

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public TicketSeverity? Severity { get; set; } 

        [Required]
        public DateTime CreationDate { get; set; }  // Ticket created time

        [Required]
        public DateTime LastModified { get; set; }  // Last modified stamp

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }  // Primary key

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public TicketState State { get; set; }
    }
}
