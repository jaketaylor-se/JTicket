using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using JTicket.Models;
using JTicket.Dtos;

namespace JTicket.App_Start
{

    public class MappingProfile : Profile
    {
        public MappingProfile()    // Constructor
        {
            Mapper.CreateMap<Ticket, TicketDto>();
            Mapper.CreateMap<TicketDto, Ticket>();
        }

        
    
    }
}