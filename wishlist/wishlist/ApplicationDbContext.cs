using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Models.Identity;

namespace whishlist
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<UserGift> UserGifts { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<EventType>().HasData(
                new EventType { Name = "Christmas", EventTypeId = 1 },
                new EventType { Name = "Wedding", EventTypeId = 2 },
                new EventType { Name = "Birthday", EventTypeId = 3 });
        }
    }
}
