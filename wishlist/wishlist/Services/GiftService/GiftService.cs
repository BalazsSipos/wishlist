using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Models.RequestModels.Event;
using wishlist.Services.BlobService;
using Microsoft.Azure.Storage.Blob;
using wishlist.Models.Identity;
using wishlist.Services.User;

namespace wishlist.Services.GiftService
{
    public class GiftService : IGiftService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        IBlobStorageService blobStorageService;
        private readonly IUserService userService;

        public GiftService(ApplicationDbContext applicationDbContext, IMapper mapper, IBlobStorageService blobStorageService, IUserService userService)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.blobStorageService = blobStorageService;
            this.userService = userService;
        }

        public async Task SaveGiftAsync(AddGiftRequest addGiftRequest)
        {
            var gift = mapper.Map<AddGiftRequest, Gift>(addGiftRequest);
            gift.Event = await applicationDbContext.Events.Include(e => e.Gifts).Include(e => e.Invitations)
                .FirstOrDefaultAsync(e => e.EventId == addGiftRequest.EventId);
            await applicationDbContext.Gifts.AddAsync(gift);
            await applicationDbContext.SaveChangesAsync();
            if (addGiftRequest.Image == null)
            {
                gift.PhotoUrl = "https://dotnetpincerstorage.blob.core.windows.net/mealimages/default/default.png";
            }
            else
            {
                CloudBlockBlob blob = await blobStorageService.MakeBlobFolderAndSaveImageAsync("gift", gift.GiftId, addGiftRequest.Image);
                await AddImageUriToGiftAsync(gift.GiftId, blob);
            }
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task AddImageUriToGiftAsync(long giftId, CloudBlockBlob blob)
        {
            var gift = await GetGiftByIdAsync(giftId);
            gift.PhotoUrl = blob.SnapshotQualifiedStorageUri.PrimaryUri.ToString();
        }

        public async Task<Gift> GetGiftByIdAsync(long giftId)
        {
            var gift = await applicationDbContext.Gifts.Include(g => g.Event).FirstOrDefaultAsync(g => g.GiftId == giftId);
            return gift;
        }

        public async Task SelectGiftByUserAsync(long giftId, ClaimsPrincipal user)
        {
            var appUser = await userService.FindUserByNameOrEmailAsync(user.Identity.Name);
            var gift = await GetGiftByIdAsync(giftId);
            var userGift = new UserGift()
            {
                Gift = gift,
                BuyerUser = appUser
            };
            await applicationDbContext.UserGifts.AddAsync(userGift);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
