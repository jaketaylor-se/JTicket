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

namespace JTicket.Models
{
    /// <summary>
    /// Enum 
    /// <c>Severity</c> 
    /// Enumeration for ticket severities. In future versions, this 
    /// enumeration will replaced by a static class.
    /// </summary>
    public enum Severity
    {
        [Display(Name ="Very Low")]
        VeryLow = 1,

        [Display(Name = "Low")]
        Low = 2,

        [Display(Name = "Medium")]
        Medium = 3,

        [Display(Name = "High")]
        High = 4,

        [Display(Name = "Very High")]
        VeryHigh = 5
    }

    /// <summary>
    /// Class 
    /// <c>Ticket</c> 
    /// Base Ticket class. In future versions, this will be the only domain
    /// model for tickets.
    /// </summary>
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
        public Severity? Severity { get; set; } 

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
    }
}
