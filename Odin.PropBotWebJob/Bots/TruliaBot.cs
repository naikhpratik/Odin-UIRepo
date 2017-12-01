using AutoMapper;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Odin.Data.Core.Models;
using Odin.PropBotWebJob.Dtos.Trulia;
using Odin.PropBotWebJob.Extensions;
using Odin.PropBotWebJob.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using USAddress;

namespace Odin.PropBotWebJob.Bots
{
    public class TruliaBot : IBot
    {
        private const string BREAK_TAG = "<br />";
        private string _url;
        private string _botType;
        private HtmlDocument _doc;
        private IMapper _mapper;

        public TruliaBot(string url, IMapper mapper)
        {
            _url = url;
            _botType = GetBotType();
            HtmlWeb web = new HtmlWeb();
            _doc = web.Load(_url);
            _mapper = mapper;
        }

        public Property Bot()
        {
            Property retVal = null;

            //Try to scrape rent pages first.
            if (_botType == "rental" || _botType == "rental-community")
            {
                try
                {
                    retVal = BotRentalJson();
                }
                catch (Exception e)
                {
                    retVal = BotRental();
                }
                retVal.Description = BotRentalDescription();
            }
            else if (_botType == "property")
            {
                //Try to scrape buy pages.
                try
                {
                    retVal = BotPropertyJson();
                }
                catch (Exception e)
                {
                    retVal = BotProperty();
                }
                retVal.Description = BotPropertyDescription();
            }
            else if (_botType == "builder-community")
            {
                retVal = BotBuilderCommunity();
                retVal.Description = BotBuilderCommunityDescription();
            }
            else
            {
                throw new Exception("Unknown bot type!");
            }

            return retVal;
        }

        public IEnumerable<string> BotImages()
        {
            IEnumerable<string> images = null;

            if (_botType == "rental")
            {
                images = BotRentImageJson();
            }
            else if (_botType == "property" || _botType == "builder-community")
            {
                images = BotBuyImageJson();
            }
            else
            {
                throw new Exception("Unknown bot type!");
            }

            return images;
        }

        private Property BotRentalJson()
        {
            //Should blow up if can't find script tag.
            var scriptText = _doc.DocumentNode.SelectSingleNode("//script[contains(text(), 'trulia.propertyData')]").InnerText;
            var result = scriptText.StringBetween("trulia.propertyData.set(", ");");

            //Start by trying to map page json to dto
            TruliaRentDto tDto = JsonConvert.DeserializeObject<TruliaRentDto>(result);
            var prop = _mapper.Map<TruliaRentDto, Property>(tDto);
            
            prop.SourceUrl = _url;

            return prop;
        }

        private Property BotPropertyJson()
        {
            //Should blow up if can't find script tag.
            var scriptText = _doc.DocumentNode.SelectSingleNode("//script[contains(text(), 'window.propertyWeb = {')]").InnerText;

            var prop = new Property();

            var featStr = "{" + scriptText.StringBetween("features: {", "},") + "}";
            var featDto = JsonConvert.DeserializeObject<TruliaBuyFeatureDto>(featStr);
            _mapper.Map<TruliaBuyFeatureDto, Property>(featDto, prop);

            var locStr = "{" + scriptText.StringBetween("location: {", "},") + "}";
            var locDto = JsonConvert.DeserializeObject<TruliaBuyLocationDto>(locStr);
            _mapper.Map<TruliaBuyLocationDto, Property>(locDto, prop);

            //Get rid of obj because only 1 field?
            var priceStr = "{" + scriptText.StringBetween("listingPrice: {", "},") + "}";
            var priceDto = JsonConvert.DeserializeObject<TruliaBuyPriceDto>(priceStr);
            _mapper.Map<TruliaBuyPriceDto, Property>(priceDto,prop);

            //One off.  Didn't bother to make object.
            int temp;
            string test = scriptText.StringBetween("\"sqft\":", ",").CleanNumeric();
            if (Int32.TryParse(test, out temp))
            {
                prop.SquareFootage = temp;
            }

            prop.SourceUrl = _url;
            return prop;
        }

        //Back up in case there is no JSON on page.
        public Property BotRental()
        {
            Property prop = new Property();
            AddressParser addrParser = new AddressParser();
            var propDetail = _doc.QuerySelector("#propertyDetails");

            //Parse Address
            AddressParseResult addr;
            var streetTag = propDetail.QuerySelector("span[itemprop='streetAddress']");
            if (streetTag != null)
            {
                var street = streetTag.InnerText;
                var city = propDetail.QuerySelector("span[itemprop='addressLocality']").InnerText;
                var state = propDetail.QuerySelector("span[itemprop='addressRegion']").InnerText;
                var zip = propDetail.QuerySelector("span[itemprop='postalCode']").InnerText;

                var fullAddress = street + ", " + city + ", " + state + " " + zip;
                addr = addrParser.ParseAddress(fullAddress);
            }
            else
            {
                Regex zipReg = new Regex(@"\w{2}\s+([0-9]{5})");
                var headlineStr = propDetail.QuerySelector(".headlineDoubleSub").InnerText.Replace("\n", "");
                var zip = zipReg.Match(headlineStr).Groups[1].Value;
                var addrStr = headlineStr.StringEndingAt(zip);

                addr = addrParser.ParseAddress(addrStr);
            }

            prop.Street1 = addr.StreetLine;
            prop.Street2 = addr.SecondaryUnit + addr.SecondaryNumber;
            prop.City = addr.City;
            prop.State = addr.State;
            prop.PostalCode = addr.Zip;

            //Parse Bed/Bath
            Regex numReg = new Regex(@"[+-]?(\d*\.)?\d+");
            var bedBathList = propDetail.QuerySelectorAll(".listBulleted > li");
            foreach (var bedBath in bedBathList)
            {
                var text = bedBath.InnerText.ToUpper().CleanNumeric();
                if (text.Contains("BED"))
                {
                    int temp;
                    if (Int32.TryParse(numReg.Match(text).Value, out temp))
                    {
                        prop.NumberOfBedrooms = temp;
                    }
                }
                else if(text.Contains("BATH"))
                {
                    decimal temp;
                    if (Decimal.TryParse(numReg.Match(text).Value, out temp))
                    {
                        prop.NumberOfBathrooms = temp;
                    }
                }
            }

            //Parse Price
            var priceTag = _doc.QuerySelector(".priceModule");
            if (priceTag != null)
            {
                var priceText = priceTag.InnerText;
                decimal tempDec;
                if (Decimal.TryParse(priceText.Split('/')[0].CleanNumeric(), out tempDec))
                {
                    prop.Amount = tempDec;
                }
            }

            prop.SourceUrl = _url;
            return prop;
        }

        //Back up in case there is no JSON on page.
        public Property BotProperty()
        {
            Property prop = new Property();
            AddressParser addrParser = new AddressParser();
            var summaryContent = _doc.QuerySelector(".summaryContent");

            //Parse Address
            var address = summaryContent.QuerySelector("div.pan").InnerText;
            var cityStateZip = summaryContent.QuerySelector("div.mts").InnerText;

            var addr = addrParser.ParseAddress(address + ", " + cityStateZip);
            prop.Street1 = addr.StreetLine;
            prop.Street2 = addr.SecondaryUnit + addr.SecondaryNumber;
            prop.City = addr.City;
            prop.State = addr.State;
            prop.PostalCode = addr.Zip;

            //Parse Bed/Bad/Sqft
            Regex numReg = new Regex(@"[+-]?(\d*\.)?\d+");
            var detailsList = _doc.QuerySelectorAll("div[data-auto-test-id='home-details-overview'] li");
            foreach (var detail in detailsList)
            {
                var text = detail.InnerText.ToUpper().CleanNumeric();
                if (text.Contains("BED"))
                {
                    int temp;
                    if (Int32.TryParse(numReg.Match(text).Value, out temp))
                    {
                        prop.NumberOfBedrooms = temp;
                    }
                }
                else if (text.Contains("BATH"))
                {
                    decimal temp;
                    if (Decimal.TryParse(numReg.Match(text).Value, out temp))
                    {
                        prop.NumberOfBathrooms = temp;
                    }
                }
                else if (text.Contains("SQFT") && !text.Contains("$"))
                {
                    int temp;
                    if (Int32.TryParse(numReg.Match(text).Value, out temp))
                    {
                        prop.SquareFootage = temp;
                    }
                }
            }

            //Parse Price
            var priceTag = summaryContent.QuerySelector("span.h3");
            if (priceTag != null)
            {
                decimal temp;
                if (Decimal.TryParse(priceTag.InnerText.CleanNumeric(), out temp))
                {
                    prop.Amount = temp;
                }
            }
            prop.SourceUrl = _url;
            return prop;
        }

        private Property BotBuilderCommunity()
        {
            //Should blow up if can't find script tag.
            var scriptText = _doc.DocumentNode.SelectSingleNode("//script[contains(text(), 'window.propertyWeb = {')]").InnerText;

            var prop = new Property();

            var locStr = "{" + scriptText.StringBetween("location: {", "},") + "}";
            var locDto = JsonConvert.DeserializeObject<TruliaBuyLocationDto>(locStr);
            _mapper.Map<TruliaBuyLocationDto, Property>(locDto, prop);

            Regex numReg = new Regex(@"[+-]?(\d*\.)?\d+");
            var bathsText = scriptText.StringBetween("\"bathrooms\":\"", "\"").CleanNumeric();
            decimal numBath;
            if (Decimal.TryParse(numReg.Match(bathsText).Value, out numBath))
            {
                prop.NumberOfBathrooms = numBath;
            }

            var bedsText = scriptText.StringBetween("\"bedrooms\":\"", "\"").CleanNumeric();
            int numBeds;
            if (int.TryParse(numReg.Match(bedsText).Value, out numBeds))
            {
                prop.NumberOfBedrooms = numBeds;
            }

            var priceText = scriptText.StringBetween("listingPrice: \"", "\"").CleanNumeric();
            decimal price;
            if (Decimal.TryParse(numReg.Match(priceText).Value, out price))
            {
                prop.Amount = price;
            }

            prop.SourceUrl = _url;
               
            return prop;
        }

        private List<string> BotRentImageJson()
        {
            var rawPhotoJson = _doc.QuerySelector("#photoPlayerSlideshow").Attributes["data-photos"].Value;
            var tDto = JsonConvert.DeserializeObject<TruliaRentImagesDto>(rawPhotoJson);

            List<string> images = new List<string>();
            if (tDto.Images != null && tDto.Images.Count > 0)
            {
                foreach (var dtoImage in tDto.Images)
                {
                    var currImage = "http:" + dtoImage.Url;
                    if (!images.Contains(currImage))
                    {
                        images.Add(currImage);
                    }
                }
            }

            return images;
        }

        private IEnumerable<string> BotBuyImageJson()
        {
            var scriptText = _doc.DocumentNode
                .SelectSingleNode("//script[contains(text(), 'window.propertyWeb = {')]").InnerText;

            var mediaStr = "[" + scriptText.StringBetween("\"mediaCollection\":[", "],") + "]";
            var tDtos = JsonConvert.DeserializeObject<List<TruliaBuyImageDto>>(mediaStr);

            List<string> images = new List<string>();
            if (tDtos != null && tDtos.Count > 0)
            {
                foreach (var dtoImage in tDtos)
                {
                    var currImage = "http:" + dtoImage.Url;
                    if (!images.Contains(currImage))
                    {
                        images.Add(currImage);
                    }
                }
            }

            return images;
        }

        private string BotRentalDescription()
        {
            var result = String.Empty;

            try
            {
                var propDetail = _doc.QuerySelector("#propertyDetails");
                if (propDetail != null)
                {
                    result += "-----OVERVIEW-----" + BREAK_TAG + BREAK_TAG;
                    var factListTags = propDetail.QuerySelectorAll(".listBulleted > li");
                    foreach (var tag in factListTags)
                    {
                        result += tag.InnerText.CleanText() + BREAK_TAG;
                    }
                    result += BREAK_TAG;
                }

                var homeDetailsTag = _doc.QuerySelector("#listingHomeDetailsContainer");
                var summaryTag = _doc.QuerySelector(".mbm + .line");

                var detailsTag = summaryTag.QuerySelector(".cols14");
                if (detailsTag != null)
                {
                    result += "-----DESCRIPTION-----" + BREAK_TAG + BREAK_TAG;

                    foreach (var tag in detailsTag.GetChildElements())
                    {
                        var text = tag.InnerText.CleanText().Replace("\r\r", BREAK_TAG + BREAK_TAG);
                        if (!String.IsNullOrWhiteSpace(text))
                        {
                            result += text + BREAK_TAG + BREAK_TAG;
                        }
                    }

                    var schoolCrimeTag = summaryTag.QuerySelector(".lastCol");
                    var schoolTag = schoolCrimeTag.QuerySelector(".boxBody");
                    var schoolLevelTags = schoolTag.QuerySelectorAll(".h7");

                    foreach (var tag in schoolLevelTags)
                    {
                        var level = tag.InnerText.CleanText();
                        var rating = tag.NextSiblingElement().InnerText.CleanText();
                        result += level + " School Rating - " + rating + BREAK_TAG + BREAK_TAG;
                    }

                    var crimeTag = schoolCrimeTag.QuerySelector(".crime_glance_details");
                    var crimeLevel = crimeTag.QuerySelector(".boxBody > .h5").InnerText.CleanText();
                    if (!String.IsNullOrWhiteSpace(crimeLevel))
                    {
                        result += "Crime - " + crimeLevel + BREAK_TAG + BREAK_TAG;
                    }
                }

                var featuresTag = homeDetailsTag.QuerySelector(".mvl");
                if (featuresTag != null)
                {
                    result += "-----FEATURES-----" + BREAK_TAG + BREAK_TAG;
                    var featuresListTag = featuresTag.QuerySelector("ul");
                    foreach (var tag in featuresListTag.GetChildElements())
                    {
                        result += tag.InnerText.CleanText() + BREAK_TAG;
                    }

                    var otherFeaturesTags = featuresTag.QuerySelectorAll(".mtm");
                    foreach (var tag in otherFeaturesTags)
                    {
                        result += tag.InnerText.CleanText();
                    }

                    result += BREAK_TAG + BREAK_TAG;
                }

                var listingInfoTag = homeDetailsTag.QuerySelector(".mtl");
                if (listingInfoTag != null)
                {
                    result += "-----LISTING INFO-----" + BREAK_TAG + BREAK_TAG;

                    var updatedTag = listingInfoTag.QuerySelector(".mtn + span");
                    if (updatedTag != null)
                    {
                        result += updatedTag.InnerText.CleanText() + BREAK_TAG + BREAK_TAG;
                    }

                    var listingInfoListTags = listingInfoTag.QuerySelectorAll(".listBulleted > li");
                    foreach (var tag in listingInfoListTags)
                    {
                        result += tag.InnerText.CleanText() + BREAK_TAG;
                    }

                }
            }
            catch (Exception e)
            {
                
            }

            return result;
        }

        private string BotPropertyDescription()
        {
            var result = String.Empty;

            try
            {
                var overviewTags = _doc.QuerySelectorAll("div[data-auto-test-id='home-details-overview'] li");
                if (overviewTags.Count > 0)
                {
                    result += "-----OVERVIEW-----" + BREAK_TAG + BREAK_TAG;
                }
                foreach (var tag in overviewTags)
                {
                    result += tag.InnerText.CleanText() + BREAK_TAG;
                }
                result += BREAK_TAG;

                var descriptionTag = _doc.QuerySelector("#descriptionContainer");
                if (descriptionTag != null)
                {
                    result += "-----DESCRIPTION-----" + BREAK_TAG + BREAK_TAG;
                    result += descriptionTag.InnerText.CleanText() + BREAK_TAG + BREAK_TAG;
                }

                var featuresTag = _doc.QuerySelector("div[data-auto-test-id='home-details-features']");
                if (featuresTag != null)
                {
                    result += "-----FEATURES-----" + BREAK_TAG + BREAK_TAG;

                    var featureListTags = featuresTag.QuerySelectorAll(".mbl");
                    foreach (var tag in featureListTags)
                    {
                        result += tag.QuerySelector(".mbm").InnerText.CleanText() + BREAK_TAG;
                        var liTags = tag.QuerySelectorAll(".man > li");
                        foreach (var liTag in liTags)
                        {
                            result += liTag.InnerText.CleanText() + BREAK_TAG;
                        }
                        result += BREAK_TAG;
                    }
                }
            }
            catch (Exception e)
            {

            }
            
            return result;
        }

        private string BotBuilderCommunityDescription()
        {
            var result = String.Empty;

            try
            {
                var detailsBlockTag = _doc.QuerySelector(".maxWidth");
                var detailsListTags = detailsBlockTag.QuerySelectorAll(".mbm");
                foreach(var tag in detailsListTags)
                {
                    var prevTag = tag.PreviousSiblingElement();
                    var title = prevTag.QuerySelector(".h4").InnerText.CleanText().ToUpper();
                    result += "-----"+ title + "-----" + BREAK_TAG + BREAK_TAG;

                    var listTags = tag.QuerySelectorAll("ul > li");
                    foreach (var liTag in listTags)
                    {
                        result += liTag.InnerText.CleanText() + BREAK_TAG;
                    }
                    result += BREAK_TAG;
                }

                var descTag = detailsBlockTag.QuerySelector("#propertyDescription");
                if (descTag != null)
                {
                    result += "-----DESCRIPTION-----" + BREAK_TAG + BREAK_TAG;
                    result += descTag.InnerText.CleanText() + BREAK_TAG + BREAK_TAG;
                }
            }
            catch (Exception e)
            {
                
            }

            return result;
        }

        private string GetBotType()
        {
            var route = _url.Split(new string[] {"https://www.trulia.com/"}, StringSplitOptions.None)[1];
            return route.Split('/')[0].ToLower().Trim();
        }

    }
}
