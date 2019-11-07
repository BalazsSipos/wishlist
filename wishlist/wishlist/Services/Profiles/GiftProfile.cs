using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Models.RequestModels.Event;

namespace wishlist.Services.Profiles
{
    public class GiftProfile : Profile
    {
        public GiftProfile()
        {
            CreateMap<AddGiftWithDataRequest, Gift>();
            CreateMap<Gift, AddGiftWithDataRequest>();
        }
    }
}
