using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using wishlist;
using wishlist.Services.BlobService;
using wishlist.Services.EventService;
using wishlist.Services.User;
using wishlistTests.TestUtils;
using Xunit;

namespace wishlistTests.Services
{
    [Collection("Database collection")]
    public class EventServiceTest
    {
        private readonly DbContextOptions<ApplicationDbContext> options;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IUserService> mockUserService;
        private readonly Mock<IBlobStorageService> mockBlobStorageService;

        public EventServiceTest()
        {
            options = TestDbOptions.Get();
            mockMapper = new Mock<IMapper>();
            mockUserService = new Mock<IUserService>();
            mockBlobStorageService = new Mock<IBlobStorageService>();
        }

        [Fact]
        public async Task Event_Is_Queried_By_Id()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var eventService = new EventService(context, mockMapper.Object, mockBlobStorageService.Object, mockUserService.Object);
                var length = await context.Events.CountAsync();
                var eventItem = await eventService.GetEventByIdAsync(2L);

                Assert.Equal(2, await context.Events.CountAsync());
                Assert.Equal("Test Birthday 2020", eventItem.Name);
            }
        }
    }
}
