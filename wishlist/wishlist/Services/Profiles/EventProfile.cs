using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Models.RequestModels.Event;

namespace wishlist.Services.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<AddEventRequest, Event>();
            CreateMap<Event, AddEventRequest>();
        }
    }
}
