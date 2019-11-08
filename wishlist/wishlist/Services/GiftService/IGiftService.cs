using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using wishlist.Models;
using wishlist.Models.Identity;
using wishlist.Models.RequestModels.Event;

namespace wishlist.Services.GiftService
{
    public interface IGiftService
    {
        Task SaveGiftAsync(AddGiftWithDataRequest addGiftWithDataRequest);
        Task AddImageUriToGiftAsync(long giftId, CloudBlockBlob blob);
        Task<Gift> GetGiftByIdAsync(long giftId);
        Task SelectGiftByUserAsync(Gift gift, ClaimsPrincipal user);
        Task SaveGiftFromArukeresoAsync(AddGiftWithUrlRequest addGiftWithUrlRequest);
    }
}
