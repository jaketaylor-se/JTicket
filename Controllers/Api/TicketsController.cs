/*
 *  Controller for RESTful web api. 
 */

using System;
using System.Linq;
using System.Web.Http;

using JTicket.Models;
using JTicket.Models.Enumerations;
using JTicket.Dtos;
using AutoMapper;


namespace JTicket.Controllers.Api
{
    /// <summary>
    /// Class 
    /// <c>TicketsController</c> 
    /// Controller for handling request to web api.
    /// </summary>
    public class TicketsController : ApiController    
    {

        private ApplicationDbContext _context;    // Context to database

        /// <summary>
        /// Method 
        /// <c>TicketsController</c> 
        /// Constructor for the TicketsController class.
        /// </summary>
        public TicketsController()
        {
            _context = new ApplicationDbContext();    // Initialize context
        }

        [HttpGet, Route("api/tickets/{filter=all}")]
        public IHttpActionResult GetTickets(string filter="all")
        {
            // Pass Mapper as Delegate
            // Select Method: Projects each element of a sequence into a new 
            // form by incorporating the element's index.



            if (filter.Equals("all"))    // Request is for all tickets
                return Ok(_context.Tickets
                          .ToList()
                          .Select(Mapper.Map<Ticket, TicketDto>));

            else if (filter.Equals("open"))
                return Ok(_context.Tickets
                          .Where(t => t.State != TicketState.resolved)
                          .ToList()
                          .Select(Mapper.Map<Ticket, TicketDto>));

            else if (Enum.IsDefined(typeof(TicketState), filter))
            {
                TicketState theState = (TicketState)Enum.Parse(typeof(TicketState), filter);
                return Ok(_context.Tickets
                          .Where(t => t.State == theState)
                          .ToList()
                          .Select(Mapper.Map<Ticket, TicketDto>));
            }
            else
                return BadRequest();
        }

        [HttpGet, Route("api/tickets/{id}")]
        public IHttpActionResult GetTicket(int id)
        {

            var ticket = _context.Tickets.SingleOrDefault(t => t.Id == id);

            // REST convention if not found -> Return HTTP Response
            if (ticket == null)
                return NotFound();

            return Ok(Mapper.Map<Ticket, TicketDto>(ticket));
        }

        // POST /api/tickets/1
        [HttpPost, Route("api/tickets")]
        public IHttpActionResult CreateTicket([FromBody] TicketDto ticketDto) 
        {
            /* The return type is IHttpActionResult to give more control over
              the status code sent back to the client. */
            if (!ModelState.IsValid)
                BadRequest();    // implements the IHttpActionResultInterface

            // Map to dto to domain model
            var ticket = Mapper.Map<TicketDto, Ticket>(ticketDto); 

            // Update internal state data
            ticket.CreationDate = DateTime.Now;
            ticket.LastModified = ticket.CreationDate;
            ticket.IsOpen = true;
            ticket.State = TicketState.open;

            // Add the new object to context and save changes
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            ticketDto.Id = ticket.Id;

            // Return Unified Resource Identifier
            return Created(new Uri(Request.RequestUri + "/" + ticket.Id), 
                                   ticketDto);    
        }

        [HttpPut, Route("api/tickets/{id}")]
        public IHttpActionResult PutTicket(int id, int newState,
                                             [FromBody] TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var ticketInDB = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticketInDB == null)    // No ticket found for this id
                return NotFound();

            // No return type now
            Mapper.Map(ticketDto, ticketInDB);    // Compiler infers types
            ticketInDB.LastModified = DateTime.Now;  // Update modified stamp


            if (Enum.IsDefined(typeof(TicketState), newState))
            {
                ticketInDB.State = (TicketState)newState;
            }
            else
                return BadRequest();

            if (ticketInDB.State != TicketState.resolved)
                ticketInDB.IsOpen = true;
            else
                ticketInDB.IsOpen = false;

            _context.SaveChanges();    // Persist changes

            return Ok();
        }

        // DELETE /api/ticket/1
        [HttpDelete]
        [Authorize(Roles = RoleName.HasFullPermissions)]
        public IHttpActionResult DeleteTicket(int id)
        {
            var ticketInDB = _context    // Ticket in database
                             .Tickets
                             .SingleOrDefault(t => t.Id == id);

            if (ticketInDB == null)    // No match in database
                return NotFound();

            _context.Tickets.Remove(ticketInDB);    // nuke ticket
            _context.SaveChanges();                 // save changes

            return Ok();
        }
    }
}
