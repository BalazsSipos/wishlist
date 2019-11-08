using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using wishlist;
using wishlist.Models;
using wishlist.Models.Identity;

namespace wishlistTests.TestUtils
{
    public class DatabaseFixture : IDisposable
    {
        private readonly DbContextOptions<ApplicationDbContext> options;
        public DatabaseFixture()
        {
            options = TestDbOptions.Get();
            using (var dbContext = new ApplicationDbContext(options))
            {
                SeedEvents(dbContext);
                dbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Events.RemoveRange(dbContext.Events);
                dbContext.SaveChanges();
            }
        }

        private void SeedEvents(ApplicationDbContext dbContext)
        {
            AppUser user = new AppUser
            {
                UserName = "Test Gabriel"
            };

            Event testEvent = new Event
            {
                Name = "Test Christmas 2019",
                EventType =
                {
                    Name = "Christmas"
                }
            };
            Event testEvent2 = new Event
            {
                Name = "Test Birthday 2020",
                EventType =
                {
                    Name = "Birthday"
                }
            };

            dbContext.Events.AddRange(new List<Event>
            {
                testEvent, testEvent2
            });

            dbContext.Gifts.AddRange(new List<Gift>
            {
                new Gift {
                    Name = "Terran Medic figure",
                    Price = 20000
                },
                new Gift {
                    Name = "Starcraft Battlechest",
                    Price = 40100
                },
                new Gift {
                    Name = "Diablo IV card game",
                    Price = 6500
                }
            });
        }
    }
}
