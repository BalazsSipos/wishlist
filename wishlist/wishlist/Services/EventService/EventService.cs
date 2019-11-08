using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using wishlist;
using wishlist.Models;
using wishlist.Models.Identity;
using wishlist.Models.RequestModels.Event;
using wishlist.Services.BlobService;
using wishlist.Services.User;

namespace wishlist.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        IBlobStorageService blobStorageService;
        IUserService userService;

        public EventService(ApplicationDbContext applicationDbContext, IMapper mapper, IBlobStorageService blobStorageService, IUserService userService)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.blobStorageService = blobStorageService;
            this.userService = userService;
        }

        public async Task<Event> GetEventByIdAsync(long eventId)
        {
            var eventItem = await applicationDbContext.Events.Include(e => e.Gifts).Include(e => e.Invitations).Include(e => e.EventType).FirstOrDefaultAsync(e => e.EventId == eventId);
            if (eventItem == null)
            {
                return null;
            }
            return eventItem;
        }

        public async Task<bool> ValidateAccessAsync(long eventId, ClaimsPrincipal user)
        {
            List<Event> ownedEvents = await FindEventsByManagerNameOrEmailAsync(user.Identity.Name);
            Event currentEvent = await GetEventByIdAsync(eventId);
            return ownedEvents.Contains(currentEvent);
        }

        public async Task<List<Event>> FindEventsByManagerNameOrEmailAsync(string managerName)
        {
            var eventList = await applicationDbContext.Events.Include(e => e.Gifts).AsQueryable().Where(e => e.AppUser.UserName == managerName).OrderBy(e => e.Name).ToListAsync();
            return eventList;
        }
        
        public async Task<Event> FindEventByGiftId(long id)
        {
            var eventItem = await applicationDbContext.Events.Include(e => e.Gifts).AsQueryable()
                .FirstAsync(e => e.Gifts[0].GiftId == id);
            return eventItem;
        }

        public async Task<AddEventRequest> BuildEmptyAddEventRequestAsync(AddEventRequest addEventRequest)
        {
            List<EventType> eventTypes = await applicationDbContext.EventTypes.ToListAsync();
            if(addEventRequest == null)
            {
                AddEventRequest newRequest = new AddEventRequest()
                {
                    EventTypes = eventTypes
                };
                return newRequest;
            }
            else
            {
                addEventRequest.EventTypes = eventTypes;
                return addEventRequest;
            }
        }
        
        public async Task SaveEventAsync(AddEventRequest addEventRequest, ClaimsPrincipal user)
        {
            var eventItem = mapper.Map<AddEventRequest, Event>(addEventRequest);
            eventItem.AppUser = await userService.FindUserByNameOrEmailAsync(user.Identity.Name);
            eventItem.EventType = await GetEventTypeByIdAsync(addEventRequest.SelectedEventTypeId);
            await applicationDbContext.Events.AddAsync(eventItem);
            await applicationDbContext.SaveChangesAsync();
            if (addEventRequest.Image == null)
            {
                eventItem.PhotoUrl = "https://dotnetpincerstorage.blob.core.windows.net/mealimages/default/default.png";
            }
            else
            {
                CloudBlockBlob blob = await blobStorageService.MakeBlobFolderAndSaveImageAsync("event", eventItem.EventId, addEventRequest.Image);
                await AddImageUriToEventAsync(eventItem.EventId, blob);
            }
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task AddImageUriToEventAsync(long eventId, CloudBlockBlob blob)
        {
            var eventItem = await GetEventByIdAsync(eventId);
            eventItem.PhotoUrl = blob.SnapshotQualifiedStorageUri.PrimaryUri.ToString();
        }

        public async Task<EventType> GetEventTypeByIdAsync(long eventTypeId)
        {
            var eventType = await applicationDbContext.EventTypes.FirstOrDefaultAsync(et => et.EventTypeId == eventTypeId);
            return eventType;
        }
    }
}