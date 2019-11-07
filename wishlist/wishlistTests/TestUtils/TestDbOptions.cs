using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using wishlist;

namespace wishlistTests.TestUtils
{
    class TestDbOptions
    {
        public static DbContextOptions<ApplicationDbContext> Get()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "wishlist-testdb")
                .Options;
        }
    }
}
