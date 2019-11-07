using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Models.RequestModels.Event;
using wishlist.Services.BlobService;
using Microsoft.Azure.Storage.Blob;

namespace wishlist.Services.GiftService
{
    public class GiftService : IGiftService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        IBlobStorageService blobStorageService;

        public GiftService(ApplicationDbContext applicationDbContext, IMapper mapper, IBlobStorageService blobStorageService)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.blobStorageService = blobStorageService;
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
    }
}
