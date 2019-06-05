/*
 *  Mapping profiles for Automapper. The Mappers will map between data
 *  transfer objects and the Domain Model classes.
 *  
 *  NOTE: This application currently uses a Deprecated version of 
 *  Automapper that implements static classes. When Automapper is
 *  updated, the Mapping profile will need to be updated to use
 *  non-static classes.
 */

using AutoMapper;    // Automapper package
using JTicket.Models;
using JTicket.Dtos;

namespace JTicket.App_Start
{
    /// <summary>
    /// Class 
    /// <c>MappingProfile</c> 
    /// Mapping Profile between JTicket DTOs and Domain Model classes.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Method 
        /// <c>MappingProfile</c> 
        /// Constructor for the MappingProfile class. Creates maps
        /// for static Mapper class.
        /// </summary>
        public MappingProfile()    
        {
            Mapper.CreateMap<Ticket, TicketDto>();    // Ticket --> TicketDto map
            Mapper.CreateMap<TicketDto, Ticket>();    // TicketDto --> Ticket map
        }
    }
}
