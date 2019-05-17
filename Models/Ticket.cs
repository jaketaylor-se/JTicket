using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace JTicket.Models
{
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

    public class Ticket
    {
        [StringLength(500)]
        public string Description { get; set; }    // strings are nullable

        [StringLength(500)]
        public string Comments{ get; set; }

        [Required]
        public bool isOpen { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
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