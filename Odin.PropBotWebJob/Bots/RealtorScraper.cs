using BatchJobPropBot.Dtos;
using BatchJobPropBot.Extensions;
using BatchJobPropBot.interfaces;
using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;

namespace BatchJobPropBot.Scrapers
{
    public class RealtorScraper : IScraper
    {
        private const string BREAK_TAG = "<br />";
        private string _url;
        private HtmlDocument _doc;

        public RealtorScraper(string url)
        {
            _url = url;
            HtmlWeb web = new HtmlWeb();
            _doc = web.Load(_url);
        }

        public HousingPropertyDto Scrape(string orderId)
        {
            try
            {
                HousingPropertyDto hpDto = new HousingPropertyDto();

                var head = _doc.QuerySelector("head");

                //Get address - Blow up if missing any of these pieces.
                hpDto.PropertyStreet1 =
                    head.QuerySelector("meta[property='og:street-address']").Attributes["content"].Value.CleanText();
                hpDto.PropertyCity = head.QuerySelector("meta[property='og:locality']").Attributes["content"].Value.CleanText();
                hpDto.PropertyState = head.QuerySelector("meta[property='og:region']").Attributes["content"].Value.CleanText();
                hpDto.PropertyPostalCode =
                    head.QuerySelector("meta[property='og:postal-code']").Attributes["content"].Value.CleanText();

                var latTag = head.QuerySelector("meta[property='place:location:latitude']");
                var lngTag = head.QuerySelector("meta[property='place:location:longitude']");

                if (latTag != null && lngTag != null)
                {
                    decimal lat;
                    if (Decimal.TryParse(latTag.Attributes["content"].Value.CleanNumeric(), out lat))
                    {
                        hpDto.PropertyLatitude = lat;
                    }

                    decimal lng;
                    if (Decimal.TryParse(lngTag.Attributes["content"].Value.CleanNumeric(), out lng))
                    {
                        hpDto.PropertyLongitude = lng;
                    }
                }

                Regex numReg = new Regex(@"[+-]?(\d*\.)?\d+");

                var bedTag = _doc.QuerySelector("li[data-label='property-meta-beds'] > span");
                if (bedTag != null)
                {
                    int beds;
                    if (Int32.TryParse(numReg.Match(bedTag.InnerText.CleanNumeric()).Value, out beds))
                    {
                        hpDto.PropertyNumberOfBedrooms = beds;
                    }
                }

                var bathTag = _doc.QuerySelector("li[data-label='property-meta-bath'] > span");
                if (bathTag != null)
                {
                    int baths;
                    if (Int32.TryParse(numReg.Match(bathTag.InnerText.CleanNumeric()).Value, out baths))
                    {
                        hpDto.PropertyNumberOfBathrooms = baths;
                    }
                }

                var sqftTag = _doc.QuerySelector("li[data-label='property-meta-sqft'] > span");
                if (sqftTag != null)
                {
                    int sqft;
                    if (Int32.TryParse(numReg.Match(sqftTag.InnerText.CleanNumeric()).Value, out sqft))
                    {
                        hpDto.PropertySquareFootage = sqft;
                    }
                }

                var priceTag = _doc.QuerySelector("span[itemprop='lowPrice'] > span") ?? _doc.QuerySelector("span[itemprop='price'] > span");
                if (priceTag != null)
                {
                    decimal price;
                    if (decimal.TryParse(priceTag.InnerText.CleanNumeric(), out price))
                    {
                        hpDto.PropertyAmount = price;
                    }
                }

                hpDto.PropertyDescription = ScrapeDescription();
                hpDto.OrderId = orderId;
                hpDto.SourceUrl = _url;

                return hpDto;
            }
            catch (Exception e)
            {
                
            }

            return null;
        }

        public HousingPropertyImagesDto ScrapeImages(string hpId)
        {

            try
            {
                var hpiDto = new HousingPropertyImagesDto();

                var imgs = _doc.QuerySelectorAll("#ldpHeroCarousel > div > img");
                foreach (var img in imgs)
                {
                    if (img.Attributes["data-src"] != null || img.Attributes["src"] != null)
                    {
                        var srcAtt = img.Attributes["data-src"] ?? img.Attributes["src"];
                        if (!hpiDto.Images.Contains(srcAtt.Value))
                        {
                            hpiDto.Images.Add(srcAtt.Value);
                        }
                    }

                }

                hpiDto.Id = hpId;

                return hpiDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ScrapeDescription()
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
