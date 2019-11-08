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
using System.Net.Http;
using HtmlAgilityPack;

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

        public async Task SaveGiftAsync(AddGiftWithDataRequest addGiftWithDataRequest)
        {
            var gift = mapper.Map<AddGiftWithDataRequest, Gift>(addGiftWithDataRequest);
            gift.Event = await applicationDbContext.Events.Include(e => e.Gifts).Include(e => e.Invitations)
                .FirstOrDefaultAsync(e => e.EventId == addGiftWithDataRequest.EventId);
            await applicationDbContext.Gifts.AddAsync(gift);
            await applicationDbContext.SaveChangesAsync();
            if (addGiftWithDataRequest.Image == null)
            {
                gift.PhotoUrl = "https://dotnetpincerstorage.blob.core.windows.net/mealimages/default/default.png";
            }
            else
            {
                CloudBlockBlob blob = await blobStorageService.MakeBlobFolderAndSaveImageAsync("gift", gift.GiftId, addGiftWithDataRequest.Image);
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

        public async Task SelectGiftByUserAsync(Gift gift, ClaimsPrincipal user)
        {
            var appUser = await userService.FindUserByNameOrEmailAsync(user.Identity.Name);
            var userGift = new UserGift()
            {
                Gift = gift,
                BuyerUser = appUser
            };
            await applicationDbContext.UserGifts.AddAsync(userGift);
            await applicationDbContext.SaveChangesAsync();
        }
        public async Task SaveGiftFromArukeresoAsync(AddGiftWithUrlRequest addGiftWithUrlRequest)
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(addGiftWithUrlRequest.GiftUrl);
                var pageContents = await response.Content.ReadAsStringAsync();
                HtmlDocument pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);
                var gift = new Gift();
                gift.Name = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@class,'product-details')]//h1)").InnerText.Trim();
                var priceString = pageDocument.DocumentNode.SelectSingleNode("(//span[contains(@itemprop,'lowPrice')])").Attributes["content"].Value;
                gift.Price = Int32.Parse(priceString.Substring(0, priceString.IndexOf(".")));
                gift.PhotoUrl = pageDocument.DocumentNode.SelectSingleNode("(/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/a[1]/img[1])").Attributes["src"].Value;
                gift.Quantity = addGiftWithUrlRequest.Quantity;
                gift.Event = await applicationDbContext.Events.Include(e => e.Gifts).Include(e => e.Invitations)
                            .FirstOrDefaultAsync(e => e.EventId == addGiftWithUrlRequest.EventId);
                gift.GiftUrl = addGiftWithUrlRequest.GiftUrl;
                await applicationDbContext.Gifts.AddAsync(gift);
                await applicationDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
            }
        }
    }
}
