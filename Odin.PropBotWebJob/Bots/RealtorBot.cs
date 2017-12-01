using AutoMapper;
using HtmlAgilityPack;
using Odin.Data.Core.Models;
using Odin.Data.Helpers;
using Odin.PropBotWebJob.Extensions;
using Odin.PropBotWebJob.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Odin.PropBotWebJob.Bots
{
    public class RealtorBot : IBot
    {
        private const string BREAK_TAG = "<br />";
        private string _url;
        private HtmlDocument _doc;
        private IMapper _mapper;

        public RealtorBot(string url, IMapper mapper)
        {
            _url = url;
            HtmlWeb web = new HtmlWeb();
            _doc = web.Load(_url);
            _mapper = mapper;
        }

        public Property Bot()
        {
            Property prop = new Property();

            var head = _doc.QuerySelector("head");

            //Get address - Blow up if missing any of these pieces.
            prop.Street1 =
                head.QuerySelector("meta[property='og:street-address']").Attributes["content"].Value.CleanText();
            prop.City = head.QuerySelector("meta[property='og:locality']").Attributes["content"].Value.CleanText();
            prop.State = head.QuerySelector("meta[property='og:region']").Attributes["content"].Value.CleanText();
            prop.PostalCode =
                head.QuerySelector("meta[property='og:postal-code']").Attributes["content"].Value.CleanText();

            var latTag = head.QuerySelector("meta[property='place:location:latitude']");
            var lngTag = head.QuerySelector("meta[property='place:location:longitude']");

            if (latTag != null && lngTag != null)
            {
                var lat = latTag.Attributes["content"].Value.CleanNumeric();
                var lng = lngTag.Attributes["content"].Value.CleanNumeric();
                if (!String.IsNullOrEmpty(lat) && !String.IsNullOrEmpty(lng))
                {
                    prop.Coordinates = GeographyHelper.CreateCoordinate(lat, lng);
                }
            }

            Regex numReg = new Regex(@"[+-]?(\d*\.)?\d+");

            var bedTag = _doc.QuerySelector("li[data-label='property-meta-beds'] > span");
            if (bedTag != null)
            {
                int beds;
                if (Int32.TryParse(numReg.Match(bedTag.InnerText.CleanNumeric()).Value, out beds))
                {
                    prop.NumberOfBedrooms = beds;
                }
            }

            var bathTag = _doc.QuerySelector("li[data-label='property-meta-bath'] > span");
            if (bathTag != null)
            {
                int baths;
                if (Int32.TryParse(numReg.Match(bathTag.InnerText.CleanNumeric()).Value, out baths))
                {
                    prop.NumberOfBathrooms = baths;
                }
            }

            var sqftTag = _doc.QuerySelector("li[data-label='property-meta-sqft'] > span");
            if (sqftTag != null)
            {
                int sqft;
                if (Int32.TryParse(numReg.Match(sqftTag.InnerText.CleanNumeric()).Value, out sqft))
                {
                    prop.SquareFootage = sqft;
                }
            }

            var priceTag = _doc.QuerySelector("span[itemprop='lowPrice'] > span") ?? _doc.QuerySelector("span[itemprop='price'] > span");
            if (priceTag != null)
            {
                decimal price;
                if (decimal.TryParse(priceTag.InnerText.CleanNumeric(), out price))
                {
                    prop.Amount = price;
                }
            }

            prop.Description = BotDescription();
            prop.SourceUrl = _url;

            return prop;
        }

        public IEnumerable<string> BotImages()
        {           
            var images = new List<string>();

            var imgs = _doc.QuerySelectorAll("#ldpHeroCarousel > div > img");
            foreach (var img in imgs)
            {
                if (img.Attributes["data-src"] != null || img.Attributes["src"] != null)
                {
                    var srcAtt = img.Attributes["data-src"] ?? img.Attributes["src"];
                    if (!images.Contains(srcAtt.Value))
                    {
                        images.Add(srcAtt.Value);
                    }
                }

            }
            
            return images;
        }

        public string BotDescription()
        {
            var result = String.Empty;
            try
            {
                var overviewListTags = _doc.QuerySelectorAll("#ldp-property-meta li");
                if (overviewListTags != null)
                {
                    result += "-----OVERVIEW-----" + BREAK_TAG + BREAK_TAG;
                    foreach (var tag in overviewListTags)
                    {
                        result += tag.InnerText.CleanText() + BREAK_TAG;
                    }
                    result += BREAK_TAG;
                }

                var descriptionTag = _doc.QuerySelector("#ldp-detail-overview");
                if (descriptionTag != null)
                {
                    result += "-----DESCRIPTION-----" + BREAK_TAG + BREAK_TAG;
                    var factTags = descriptionTag.QuerySelectorAll("#key-fact-carousel li");
                    foreach (var tag in factTags)
                    {
                        result += tag.InnerText.CleanText() + BREAK_TAG;
                    }
                    result += BREAK_TAG;

                    var descriptionPTag = descriptionTag.QuerySelector("#ldp-detail-romance");
                    if (descriptionPTag != null)
                    {
                        result += descriptionPTag.InnerText.CleanText() + BREAK_TAG + BREAK_TAG;
                    }
                }

                var featuresTag = _doc.QuerySelector("#ldp-detail-features");
                if (featuresTag != null)
                {
                    result += "-----FEATURES-----" + BREAK_TAG + BREAK_TAG;

                    var featureListTags = featuresTag.QuerySelectorAll(".list-default > li");
                    foreach (var tag in featureListTags)
                    {
                        result += tag.InnerText.CleanText() + BREAK_TAG;
                    }
                    result += BREAK_TAG;
                }
            }
            catch (Exception e)
            {
                
            }

            return result;
        }


    }
}
