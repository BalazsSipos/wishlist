using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.models;
using wishlist.models.Identity;

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
    }
}
