using AutoMapper;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.PropBotWebJob.Bots;
using Odin.PropBotWebJob.Helpers;
using Odin.PropBotWebJob.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TraceFilter = Microsoft.Azure.WebJobs.Extensions.TraceFilter;

namespace Odin.PropBotWebJob
{
    public class Functions
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageStore _imageStore;

        public Functions(IUnitOfWork unitOfWork, IMapper mapper, IImageStore imageStore)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageStore = imageStore;
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called propbotqueue.
        public void ProcessQueueMessage([QueueTrigger("propbotqueue")] string message, int dequeueCount, TextWriter log)
        {
            //Azure bug causes poison messages to stay in regular queue and keep processing.
            //This will mark them as successful after 5th time so they stop reprocessing after being put in poison queue.
            if (dequeueCount > ConfigHelper.GetMaxDequeueCount())
            {
                return;
            }

            var queueEntry = JsonConvert.DeserializeObject<PropBotJobQueueEntry>(message);
            var order = _unitOfWork.Orders.GetOrderById(queueEntry.OrderId);

            var bot = GetBot(queueEntry.PropertyUrl);
            var property = bot.Bot();

            try
            {
                //If exception happens on image retrieval, still save property.
                IEnumerable<string> images = bot.BotImages();
                foreach (var image in images)
                {
                    try
                    {
                        using (WebClient client = new WebClient())
                        {
                            using (Stream stream = client.OpenRead(image))
                            {
                                var storageId = _imageStore.SaveImage(stream);
                                var urlStr = _imageStore.UriFor(storageId).AbsoluteUri;
                                var photo = new Photo(storageId, urlStr);
                                property.Photos.Add(photo);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //Swallow image save exception.
                    }
                }

            }
            catch (Exception e)
            {
                
            }

            order.HomeFinding.HomeFindingProperties.Add(new HomeFindingProperty()
            {
                Property = property
            });
            
            _unitOfWork.Complete();

            //HousingPropertyDto hpDto = bot.Bot(queueEntry.OrderId);
            //HomeFindingProperty hfp = _mapper.Map<HousingPropertyDto, HomeFindingProperty>(hpDto);
           

            log.WriteLine(queueEntry.PropertyUrl);
        }

        private IBot GetBot(string rawUrl)
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

        //If 25 individual exceptions (basically if 5 properties fail) within an hour. Send Error Alert.
        //Throttle means one alert per hour.
        //Couldn't get SendGrid attribute to work, so send manually instead.
        public static void ErrorMonitor(
            [ErrorTrigger("01:00:00", 25, Throttle = "1:00:00")] TraceFilter filter, TextWriter log)

        {
            JObject errorJson = new JObject();
            errorJson["Title"] = "10 errors occured withing the past 30 minutes.";
            errorJson["Messages"] = filter.GetDetailedMessage(10);

            EmailHelper.SendErrorEmail("Odin.PropBotWebJob Error Alert",
                errorJson.ToString());
        }

    }
}
