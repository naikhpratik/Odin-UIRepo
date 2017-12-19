using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using HtmlAgilityPack;
using Odin.Data.Core.Models;
using Odin.PropBotWebJob.Extensions;
using Odin.PropBotWebJob.Interfaces;

namespace Odin.PropBotWebJob.Bots
{
    public class CraigslistBot : IBot
    {
        private const string BREAK_TAG = "<br />";
        private string _url;
        private HtmlDocument _doc;
        private IMapper _mapper;

        public CraigslistBot(string url, IMapper mapper, string html = null)
        {
            _url = url;
            _mapper = mapper;

            if (String.IsNullOrEmpty(html))
            {
                HtmlWeb web = new HtmlWeb();
                _doc = web.Load(_url);
            }
            else
            {
                _doc = new HtmlDocument();
                _doc.LoadHtml(html);
            }
        }

        public Property Bot()
        {
            var prop = new Property();

            var body = _doc.QuerySelector(".body");

            var address = body.QuerySelectorAll(".userbody .mapAndAttrs .mapbox .mapaddress");
            //prop.Street1 = address.InnerText.CleanText();

            var map = body.QuerySelector(".userbody .mapAndAttrs .mapbox .map");

            var latAttribute = map.Attributes["data-latitude"];
            if (latAttribute != null)
            {
                if (decimal.TryParse(latAttribute.Value.CleanNumeric(), out var lat))
                {
                    prop.Latitude = lat;
                }
            }

            var longAttribute = map.Attributes["data-longitude"];
            if (longAttribute != null)
            {
                if (decimal.TryParse(longAttribute.Value.CleanNumeric(), out var lon))
                {
                    prop.Longitude = lon;
                }
            }

            var priceTag = body.QuerySelector(".price");
            if (priceTag != null)
            {
                if (decimal.TryParse(priceTag.InnerText.CleanNumeric(), out var price))
                {
                    prop.Amount = price;
                }
            }

            var descriptionTag = body.QuerySelector(".userbody .postingbody");
            if (descriptionTag != null)
            {
                prop.Description = descriptionTag.InnerText.CleanText();
            }

            var attrGroups = body.QuerySelectorAll(".userbody .mapAndAttrs .attrgroup");
            if (attrGroups != null)
            {
                foreach (var group in attrGroups)
                {
                    if (group.QuerySelector(".property_date") != null)
                    {
                        var dateTag = group.QuerySelector(".property_date");
                        var dateProperty = dateTag?.Attributes["data-date"];
                        if (dateProperty != null && DateTime.TryParse(dateProperty.Value, out var availabilityDate))
                        {
                            prop.AvailabilityDate = availabilityDate;
                        }
                    }
                    if (group.QuerySelectorAll(".shared-line-bubble") != null) // Craigslist has important information in a list of bubbles
                    {
                        foreach (var bubble in group.QuerySelectorAll(".shared-line-bubble"))
                        {
                            if (bubble != null)
                            {
                                if (bubble.InnerText.Contains("ft")) // Square footage bubble
                                {
                                    var feetTag = bubble.QuerySelector("b");
                                    if (feetTag != null)
                                    {
                                        if(int.TryParse(feetTag.InnerText, out var feet))
                                        {
                                            prop.SquareFootage = feet;
                                        }
                                    }
                                }
                                else if (bubble.InnerText.ToLower().Contains("br") ||
                                         (bubble.InnerText.ToLower().Contains("ba"))) // Bed or bath bubble
                                {
                                    var bubbles = bubble.QuerySelectorAll("b");
                                    foreach (var bub in bubbles)
                                    {
                                        if (bub.InnerText.ToLower().Contains("br"))
                                        {
                                            var bedsLower = bub.InnerText.LowerReplace("br", "");
                                            if (int.TryParse(bedsLower, out var beds))
                                                prop.NumberOfBedrooms = beds;
                                        }
                                        else if (bub.InnerText.ToLower().Contains("ba"))
                                        {
                                            var bathsLower = bub.InnerText.LowerReplace("ba", "");
                                            if (decimal.TryParse(bathsLower, out var baths))
                                                prop.NumberOfBathrooms = baths;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return prop;
        }

        public IEnumerable<string> BotImages()
        {
            var imageUrls = new List<string>();

            var images = _doc.QuerySelectorAll(".userbody #thumbs .thumb");
            foreach (var image in images)
            {
                var imageHref = image.Attributes["href"];
                if (imageHref != null)
                {
                    imageUrls.Add(imageHref.Value);
                }
            }

            return imageUrls;
        }
    }
}
