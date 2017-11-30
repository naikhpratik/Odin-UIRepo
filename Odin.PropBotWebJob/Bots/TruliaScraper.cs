using AutoMapper;
using BatchJobPropBot.Dtos;
using BatchJobPropBot.Dtos.Trulia;
using BatchJobPropBot.Extensions;
using BatchJobPropBot.interfaces;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using USAddress;

namespace BatchJobPropBot.Scrapers
{
    public class TruliaScraper : IScraper
    {
        private const string BREAK_TAG = "<br />";
        private string _url;
        private string _scrapeType;
        private HtmlDocument _doc;

        public TruliaScraper(string url)
        {
            _url = url;
            _scrapeType = GetScrapeType();
            HtmlWeb web = new HtmlWeb();
            _doc = web.Load(_url);
        }

        public HousingPropertyDto Scrape(string orderId)
        {
            HousingPropertyDto retVal = null;

            //Try to scrape rent pages first.
            if (_scrapeType == "rental" || _scrapeType == "rental-community")
            {
                retVal = ScrapeRentalJson(orderId);

                if (retVal == null)
                {
                    retVal = ScrapeRental(orderId);
                }

                if (retVal != null)
                {
                    retVal.PropertyDescription = ScrapeRentalDescription();
                }
            }
            else if (_scrapeType == "property")
            {
                //Try to scrape buy pages.
                retVal = ScrapePropertyJson(orderId);

                if (retVal == null)
                {
                    retVal = ScrapeProperty(orderId);
                }

                if (retVal != null)
                {
                    retVal.PropertyDescription = ScrapePropertyDescription();
                }
            }
            else if (_scrapeType == "builder-community")
            {
                retVal = ScrapeBuilderCommunity(orderId);

                if (retVal != null)
                {
                    retVal.PropertyDescription = ScrapeBuilderCommunityDescription();
                }
            }

            return retVal;
        }

        public HousingPropertyImagesDto ScrapeImages(string hpId)
        {
            HousingPropertyImagesDto retVal = null;

            if (_scrapeType == "rental")
            {
                retVal = ScrapeRentImageJson(hpId);
            }
            else if(_scrapeType == "property" || _scrapeType == "builder-community")
            {
                retVal = ScrapeBuyImageJson(hpId);
            }

            return retVal;

        }

        private HousingPropertyDto ScrapeRentalJson(string orderId)
        {
            try
            {
                //Should blow up if can't find script tag.
                var scriptText = _doc.DocumentNode.SelectSingleNode("//script[contains(text(), 'trulia.propertyData')]").InnerText;
                var result = scriptText.StringBetween("trulia.propertyData.set(", ");");

                //Start by trying to map page json to dto
                TruliaRentDto tDto = JsonConvert.DeserializeObject<TruliaRentDto>(result);
                var hpDto = Mapper.Map<TruliaRentDto, HousingPropertyDto>(tDto);
                hpDto.OrderId = orderId;
                hpDto.SourceUrl = _url;

                return hpDto;
            }
            catch (Exception e)
            {
                return null;
            }
           
        }

        private HousingPropertyDto ScrapePropertyJson(string orderId)
        {
            try
            {
                //Should blow up if can't find script tag.
                var scriptText = _doc.DocumentNode.SelectSingleNode("//script[contains(text(), 'window.propertyWeb = {')]").InnerText;

                var hpDto = new HousingPropertyDto();

                var featStr = "{" + scriptText.StringBetween("features: {", "},") + "}";
                var featDto = JsonConvert.DeserializeObject<TruliaBuyFeatureDto>(featStr);
                Mapper.Map<TruliaBuyFeatureDto, HousingPropertyDto>(featDto, hpDto);

                var locStr = "{" + scriptText.StringBetween("location: {", "},") + "}";
                var locDto = JsonConvert.DeserializeObject<TruliaBuyLocationDto>(locStr);
                Mapper.Map<TruliaBuyLocationDto, HousingPropertyDto>(locDto, hpDto);

                //Get rid of obj because only 1 field?
                var priceStr = "{" + scriptText.StringBetween("listingPrice: {", "},") + "}";
                var priceDto = JsonConvert.DeserializeObject<TruliaBuyPriceDto>(priceStr);
                Mapper.Map<TruliaBuyPriceDto, HousingPropertyDto>(priceDto, hpDto);

                //One off.  Didn't bother to make object.
                int temp;
                string test = scriptText.StringBetween("\"sqft\":", ",").CleanNumeric();
                if (Int32.TryParse(test, out temp))
                {
                    hpDto.PropertySquareFootage = temp;
                }

                hpDto.OrderId = orderId;
                hpDto.SourceUrl = _url;
                return hpDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Back up in case there is no JSON on page.
        public HousingPropertyDto ScrapeRental(string orderId)
        {
            try
            {
                HousingPropertyDto hpDto = new HousingPropertyDto();
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

                hpDto.PropertyStreet1 = addr.StreetLine;
                hpDto.PropertyStreet2 = addr.SecondaryUnit + addr.SecondaryNumber;
                hpDto.PropertyCity = addr.City;
                hpDto.PropertyState = addr.State;
                hpDto.PropertyPostalCode = addr.Zip;

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
                            hpDto.PropertyNumberOfBedrooms = temp;
                        }
                    }
                    else if(text.Contains("BATH"))
                    {
                        decimal temp;
                        if (Decimal.TryParse(numReg.Match(text).Value, out temp))
                        {
                            hpDto.PropertyNumberOfBathrooms = temp;
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
                        hpDto.PropertyAmount = tempDec;
                    }
                }

                hpDto.OrderId = orderId;
                hpDto.SourceUrl = _url;
                return hpDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Back up in case there is no JSON on page.
        public HousingPropertyDto ScrapeProperty(string orderId)
        {
            try
            {
                HousingPropertyDto hpDto = new HousingPropertyDto();
                AddressParser addrParser = new AddressParser();
                var summaryContent = _doc.QuerySelector(".summaryContent");

                //Parse Address
                var address = summaryContent.QuerySelector("div.pan").InnerText;
                var cityStateZip = summaryContent.QuerySelector("div.mts").InnerText;

                var addr = addrParser.ParseAddress(address + ", " + cityStateZip);
                hpDto.PropertyStreet1 = addr.StreetLine;
                hpDto.PropertyStreet2 = addr.SecondaryUnit + addr.SecondaryNumber;
                hpDto.PropertyCity = addr.City;
                hpDto.PropertyState = addr.State;
                hpDto.PropertyPostalCode = addr.Zip;

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
                            hpDto.PropertyNumberOfBedrooms = temp;
                        }
                    }
                    else if (text.Contains("BATH"))
                    {
                        decimal temp;
                        if (Decimal.TryParse(numReg.Match(text).Value, out temp))
                        {
                            hpDto.PropertyNumberOfBathrooms = temp;
                        }
                    }
                    else if (text.Contains("SQFT") && !text.Contains("$"))
                    {
                        int temp;
                        if (Int32.TryParse(numReg.Match(text).Value, out temp))
                        {
                            hpDto.PropertySquareFootage = temp;
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
                        hpDto.PropertyAmount = temp;
                    }
                }

                hpDto.OrderId = orderId;
                hpDto.SourceUrl = _url;
                return hpDto;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        private HousingPropertyDto ScrapeBuilderCommunity(string orderId)
        {
            try
            {
                //Should blow up if can't find script tag.
                var scriptText = _doc.DocumentNode.SelectSingleNode("//script[contains(text(), 'window.propertyWeb = {')]").InnerText;

                var hpDto = new HousingPropertyDto();

                var locStr = "{" + scriptText.StringBetween("location: {", "},") + "}";
                var locDto = JsonConvert.DeserializeObject<TruliaBuyLocationDto>(locStr);
                Mapper.Map<TruliaBuyLocationDto, HousingPropertyDto>(locDto, hpDto);

                Regex numReg = new Regex(@"[+-]?(\d*\.)?\d+");
                var bathsText = scriptText.StringBetween("\"bathrooms\":\"", "\"").CleanNumeric();
                decimal numBath;
                if (Decimal.TryParse(numReg.Match(bathsText).Value, out numBath))
                {
                    hpDto.PropertyNumberOfBathrooms = numBath;
                }

                var bedsText = scriptText.StringBetween("\"bedrooms\":\"", "\"").CleanNumeric();
                int numBeds;
                if (int.TryParse(numReg.Match(bedsText).Value, out numBeds))
                {
                    hpDto.PropertyNumberOfBedrooms = numBeds;
                }

                var priceText = scriptText.StringBetween("listingPrice: \"", "\"").CleanNumeric();
                decimal price;
                if (Decimal.TryParse(numReg.Match(priceText).Value, out price))
                {
                    hpDto.PropertyAmount = price;
                }

                hpDto.OrderId = orderId;
                hpDto.SourceUrl = _url;
               
                return hpDto;
            }
            catch (Exception e)
            {
                
            }

            return null;
        }

        private HousingPropertyImagesDto ScrapeRentImageJson(string hpId)
        {
            try
            {
                var rawPhotoJson = _doc.QuerySelector("#photoPlayerSlideshow").Attributes["data-photos"].Value;
                var tDto = JsonConvert.DeserializeObject<TruliaRentImagesDto>(rawPhotoJson);

                HousingPropertyImagesDto hpiDto = null;
                if (tDto.Images != null && tDto.Images.Count > 0)
                {
                    hpiDto = new HousingPropertyImagesDto() {Id = hpId};
                    foreach (var image in tDto.Images)
                    {
                        var currImage = "http:" + image.Url;
                        hpiDto.Images.Add(currImage);
                    }
                }

                return hpiDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private HousingPropertyImagesDto ScrapeBuyImageJson(string hpId)
        {
            try
            {
                var scriptText = _doc.DocumentNode
                    .SelectSingleNode("//script[contains(text(), 'window.propertyWeb = {')]").InnerText;

                var mediaStr = "[" + scriptText.StringBetween("\"mediaCollection\":[", "],") + "]";
                var tDtos = JsonConvert.DeserializeObject<List<TruliaBuyImageDto>>(mediaStr);

                HousingPropertyImagesDto hpiDto = null;
                if (tDtos != null && tDtos.Count > 0)
                {
                    hpiDto = new HousingPropertyImagesDto() {Id = hpId};
                    foreach (var image in tDtos)
                    {
                        var currImage = "http:" + image.Url;
                        hpiDto.Images.Add(currImage);
                    }
                }

                return hpiDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string ScrapeRentalDescription()
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

        private string ScrapePropertyDescription()
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

        private string ScrapeBuilderCommunityDescription()
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

        private string GetScrapeType()
        {
            var route = _url.Split(new string[] {"https://www.trulia.com/"}, StringSplitOptions.None)[1];
            return route.Split('/')[0].ToLower().Trim();
        }

    }
}
