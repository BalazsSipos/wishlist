using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Models.RequestModels.Event;

namespace wishlist.Services.GiftService
{
    public interface IGiftService
    {
        Task SaveGiftAsync(AddGiftRequest addGiftRequest);
        Task AddImageUriToGiftAsync(long giftId, CloudBlockBlob blob);
        Task<Gift> GetGiftByIdAsync(long giftId);
    }
}
