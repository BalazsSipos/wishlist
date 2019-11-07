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
using System.Net.Http;
using HtmlAgilityPack;

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
                CloudBlockBlob blob = await blobStorageService.MakeBlobFolderAndSaveImageAsync(gift.GiftId, addGiftRequest.Image);
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

        public async Task SaveGiftFromArukeresoAsync(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var pageContents = await response.Content.ReadAsStringAsync();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);
            var gift = new Gift();
            try
            {
                gift.Name = pageDocument.DocumentNode.SelectSingleNode("(//div[contains(@class,'product-details')]//h1)").InnerText.Trim();
                gift.Price = Int32.Parse(pageDocument.DocumentNode.SelectSingleNode("(//span[contains(@itemprop,'lowPrice')])").Attributes["content"].Value);
                gift.PhotoUrl = pageDocument.DocumentNode.SelectSingleNode("(/html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/a[1]/img[1])").Attributes["src"].Value;
                await applicationDbContext.Gifts.AddAsync(gift);
                await applicationDbContext.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("Cannot parse url or cannot save to DB. Is that a url from arukereso.hu?");
            }
        }
    }
}
