using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JTicket.Models;
using JTicket.App_Start;
using JTicket.Dtos;
using AutoMapper;



/*
 *  Web API for Ticket CRUD operations.
 * 
 * 
    Status Code for Resource Creation in RESTful convention: 201
    
     
     
     
*/

namespace JTicket.Controllers.Api
{
    public class TicketsController : ApiController    // Note Superclass name
    {

        private ApplicationDbContext _context;

        public TicketsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/tickets
        public IHttpActionResult GetTickets(string filter="all")
        {
            // Pass Mapper as Delegate
            // Select Method: Projects each element of a sequence into a new form 
            // by incorporating the element's index.

            if (filter.Equals("all"))
                return Ok(_context.Tickets
                          .ToList()
                          .Select(Mapper.Map<Ticket, TicketDto>));
            else if (filter.Equals("open"))
                return Ok(_context.Tickets
                          .Where(t => t.isOpen == true)
                          .ToList()
                          .Select(Mapper.Map<Ticket, TicketDto>));
            else if (filter.Equals("resolved"))
                return Ok(_context.Tickets
                          .Where(t => t.isOpen == false)
                          .ToList()
                          .Select(Mapper.Map<Ticket, TicketDto>));
            else
                return BadRequest();
        }


        // GET /api/tickets/1
        public IHttpActionResult GetTicket(int id)
        {

            var ticket = _context.Tickets.SingleOrDefault(t => t.Id == id);

            // REST convention if not found -> Return HTTP Response
            if (ticket == null)
                return NotFound();

            return Ok(Mapper.Map<Ticket, TicketDto>(ticket));
        }



        // POST /api/tickets/1
        [HttpPost]
        [Authorize(Roles = RoleName.CanManageTickets)]
        public IHttpActionResult CreateTicket(TicketDto ticketDto)    // ticket sent in request body
        {
            /* The return type is IHttpActionResult to give more control over
              the status code sent back to the client. */
            if (!ModelState.IsValid)
                BadRequest();    // returns bad request result which implements the IHttpActionResultInterface

            var ticket = Mapper.Map<TicketDto, Ticket>(ticketDto);

            ticket.creationDate = DateTime.Now;
            ticket.lastModified = ticket.creationDate;
            ticket.isOpen = true;

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            ticketDto.Id = ticket.Id;

            return Created(new Uri(Request.RequestUri + "/" + ticket.Id), ticketDto);    // URI = Unified Resource Identifier
        }


        // PUT /api/tickets/1
        [HttpPut]
        [Authorize(Roles = RoleName.CanManageTickets)]
        public IHttpActionResult UpdateTicket(int id, TicketDto ticketDto, bool resolve=false)  // id from URL
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var ticketInDB = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticketInDB == null)
                return NotFound();

            // No return type now
            Mapper.Map(ticketDto, ticketInDB);    // Compiler infers types
            ticketInDB.lastModified = DateTime.Now;

            if (resolve)
                ticketInDB.isOpen = false;

            _context.SaveChanges();

            return Ok();
        }


        // DELETE /api/ticket/1
        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageTickets)]
        public IHttpActionResult DeleteTicket(int id)
        {

            var ticketInDB = _context.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticketInDB == null)
                return NotFound();

            _context.Tickets.Remove(ticketInDB);
            _context.SaveChanges();

            return Ok();
        }






    }
}
