using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace wishlist.Models.RequestModels.Event
{
    public class AddGiftWithDataRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public long Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public long EventId { get; set; }

        public IFormFile Image { get; set; }
    }
}
