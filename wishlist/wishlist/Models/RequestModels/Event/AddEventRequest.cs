using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace wishlist.Models.RequestModels.Event
{
    public class AddEventRequest
    {
        public List<EventType> EventTypes { get; set; }

        [Required]
        public long SelectedEventTypeId { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string Message { get; set; }

        public IFormFile Image { get; set; }
    }
}
