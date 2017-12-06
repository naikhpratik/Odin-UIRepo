using AutoMapper;
using Odin.Data.Core.Models;
using Odin.PropBotWebJob.Bots;
using Odin.PropBotWebJob.Interfaces;
using System.IO;
using System.Net;

namespace Odin.PropBotWebJob.Helpers
{
    public class BotHelper : IBotHelper
    {
        private readonly IMapper _mapper;
        private readonly IImageStore _imageStore;

        public BotHelper(IMapper mapper, IImageStore imageStore)
        {
            _mapper = mapper;
            _imageStore = imageStore;
        }

        public IBot GetBot(string rawUrl)
        {
            string url = rawUrl.ToLower();
            if (url.Contains("www.trulia.com"))
            {
                return new TruliaBot(url, _mapper);
            }
            else if (url.Contains("www.realtor.com"))
            {
                return new RealtorBot(url, _mapper);
            }
            else if (url.Contains("www.apartments.com"))
            {
                return new ApartmentsBot(url, _mapper);
            }

            return null;
        }

        public Photo SaveImageToStore(string imgUrl)
        {
            using (WebClient client = new WebClient())
            {
                using (Stream stream = client.OpenRead(imgUrl))
                {
                    var storageId = _imageStore.SaveImage(stream);
                    var urlStr = _imageStore.UriFor(storageId).AbsoluteUri;
                    return new Photo(storageId, urlStr); 
                }
            }
        }
    }
}
