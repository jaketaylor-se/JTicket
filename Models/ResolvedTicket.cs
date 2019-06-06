/*
 *  Resolved Ticket Domain Model. Likely deprecated in future 
 *  versions.
 */


using System.ComponentModel.DataAnnotations.Schema;

using JTicket.Models;

namespace JTicket.Models
{
    /// <summary>
    /// Class 
    /// <c>ResolvedTicket</c> 
    /// Extension of the base Ticket class, for Resolved Tickets.
    /// </summary>
    [NotMapped]
    public class ResolvedTicket : Ticket
    {}
}
