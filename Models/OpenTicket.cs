/*
 *  OpenTicket Domain Model. Likely deprecated in future 
 *  versions.
 */

using System.ComponentModel.DataAnnotations.Schema;

using JTicket.Models;

namespace JTicket.Models
{
    /// <summary>
    /// Class 
    /// <c>OpenTicket</c> 
    /// Extension of the base Ticket class, for Open Tickets.
    /// </summary>
    [NotMapped]
    public class OpenTicket : Ticket
    {}
}
