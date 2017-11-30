using HtmlAgilityPack;
using Odin.PropBotWebJob.Dtos;
using Odin.PropBotWebJob.Interfaces;
using System;
using System.Text.RegularExpressions;

namespace BatchJobPropBot.Bots
{
    public class ApartmentsBot : IBot
    {
        private const string BREAK_TAG = "<br />";
        private string _url;
        private HtmlDocument _doc;

        public ApartmentsBot(string url)
        {
            _url = url;
            HtmlWeb web = new HtmlWeb();
            _doc = web.Load(_url);
        }

        public HousingPropertyDto Bot(string orderId)
        {
            try
            {
                HousingPropertyDto hpDto = new HousingPropertyDto();

                var addTag = _doc.QuerySelector("[itemprop='address']");

                var streetTag = addTag.QuerySelector("[itemprop='streetAddress']");
                hpDto.PropertyStreet1 = !String.IsNullOrWhiteSpace(streetTag.InnerText)
                    ? streetTag.InnerText.CleanText()
                    : streetTag.Attributes["content"].Value.CleanText();

                var cityTag = addTag.QuerySelector("[itemprop='addressLocality']");
                hpDto.PropertyCity = !String.IsNullOrWhiteSpace(cityTag.InnerText)
                    ? cityTag.InnerText.CleanText()
                    : cityTag.Attributes["content"].Value.CleanText();


                var stateTag = addTag.QuerySelector("[itemprop='addressRegion']");
                hpDto.PropertyState = !String.IsNullOrWhiteSpace(stateTag.InnerText)
                    ? stateTag.InnerText.CleanText()
                    : stateTag.Attributes["content"].Value.CleanText();

                var zipTag = addTag.QuerySelector("[itemprop='postalCode']");
                hpDto.PropertyPostalCode = !String.IsNullOrWhiteSpace(zipTag.InnerText)
                    ? zipTag.InnerText.CleanText()
                    : zipTag.Attributes["content"].Value.CleanText();

                var latTag = _doc.QuerySelector("meta[property='place:location:latitude']");
                var lngTag = _doc.QuerySelector("meta[property='place:location:longitude']");

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

                var bedBathFtTag = _doc.QuerySelector(".profileContent .availabilityTable > tbody > .rentalGridRow");
                if (bedBathFtTag != null)
                {
                    var rentAt = bedBathFtTag.Attributes["data-maxrent"];
                    if (rentAt != null)
                    {
                        decimal rent;
                        if (decimal.TryParse(rentAt.Value.CleanNumeric(), out rent))
                        {
                            hpDto.PropertyAmount = rent;
                        }
                    }

                    var bedAt = bedBathFtTag.Attributes["data-beds"];
                    if (bedAt != null)
                    {
                        int beds;
                        if (int.TryParse(bedAt.Value.CleanNumeric(), out beds))
                        {
                            hpDto.PropertyNumberOfBedrooms = beds;
                        }
                    }

                    var bathAt = bedBathFtTag.Attributes["data-baths"];
                    if (bathAt != null)
                    {
                        decimal baths;
                        if (decimal.TryParse(bathAt.Value.CleanNumeric(), out baths))
                        {
                            hpDto.PropertyNumberOfBathrooms = baths;
                        }
                    }

                    var unitAt = bedBathFtTag.Attributes["data-unit"];
                    if (unitAt != null)
                    {
                        string unit = unitAt.Value.CleanText();
                        if (!String.IsNullOrWhiteSpace(unit))
                        {
                            hpDto.PropertyStreet2 = unit;
                        }
                    }

                    var sqftTag = bedBathFtTag.QuerySelector(".sqft");
                    if (sqftTag != null)
                    {
                        Regex numReg = new Regex(@"[+-]?(\d*\.)?\d+");
                        int sqft;
                        if (Int32.TryParse(numReg.Match(sqftTag.InnerText.CleanNumeric()).Value, out sqft))
                        {
                            hpDto.PropertySquareFootage = sqft;
                        }
                    }
                }

                hpDto.PropertyDescription = BotDescription();
                hpDto.OrderId = orderId;
                hpDto.SourceUrl = _url;

                return hpDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public HousingPropertyImagesDto BotImages(string hpId)
        {
            try
            {
                var hpiDto = new HousingPropertyImagesDto();

                var galleryImgTags = _doc.QuerySelectorAll("#gallery0 img");
                if (galleryImgTags != null)
                {
                    foreach (var img in galleryImgTags)
                    {
                        if (img.Attributes["src"] != null)
                        {
                            var srcAtt = img.Attributes["src"];
                            if (!hpiDto.Images.Contains(srcAtt.Value))
                            {
                                hpiDto.Images.Add(srcAtt.Value);
                            }
                        }
                    }
                }

                var carouselImgTags = _doc.QuerySelectorAll("#carouselSection img");
                if (carouselImgTags != null)
                {
                    foreach (var img in carouselImgTags)
                    {
                        if (img.Attributes["src"] != null)
                        {
                            var srcAtt = img.Attributes["src"];
                            if (!hpiDto.Images.Contains(srcAtt.Value))
                            {
                                hpiDto.Images.Add(srcAtt.Value);
                            }
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

        public string BotDescription()
        {
            var result = String.Empty;
            try
            {

                var feesTag = _doc.QuerySelector("#feesSection");
                if (feesTag != null)
                {
                    result += "-----ADDITIONAL EXPENSES-----" + BREAK_TAG + BREAK_TAG;

                    var monthlyTags = feesTag.QuerySelectorAll(".monthlyFees .fee");
                    if (monthlyTags != null && monthlyTags.Count > 0)
                    {
                        foreach (var tag in monthlyTags)
                        {
                            result += "Monthly -- " + tag.QuerySelector(".priceWrapper").InnerText.CleanText() +
                                      " -- " +
                                      tag.QuerySelector(".descriptionWrapper").InnerText.CleanText() + BREAK_TAG;
                        }
                        result += BREAK_TAG;
                    }

                    var oneTimeTags = feesTag.QuerySelectorAll(".oneTimeFees .fee");
                    if (oneTimeTags != null && oneTimeTags.Count > 0)
                    {
                        foreach (var tag in oneTimeTags)
                        {
                            result += "One Time -- " + tag.QuerySelector(".priceWrapper").InnerText.CleanText() +
                                      " -- " +
                                      tag.QuerySelector(".descriptionWrapper").InnerText.CleanText() + BREAK_TAG;
                        }
                        result += BREAK_TAG;
                    }
                }

                var descTag = _doc.QuerySelector("#descriptionSection");
                if(descTag != null)
                {
                    result += "-----OVERVIEW-----" + BREAK_TAG + BREAK_TAG;
                    result += descTag.QuerySelector("p").InnerText.CleanText() + BREAK_TAG + BREAK_TAG;
                }

                var specialsTag = _doc.QuerySelector("#rentSpecialsSection");
                if (specialsTag != null)
                {
                    result += "-----RENT SPECIALS-----" + BREAK_TAG + BREAK_TAG;
                    result += specialsTag.QuerySelector("p").InnerText.CleanText() + BREAK_TAG + BREAK_TAG;
                }

                var amenTag = _doc.QuerySelector("#amenitiesSection");
                if (amenTag != null)
                {
                    result += "-----FEATURES-----" + BREAK_TAG + BREAK_TAG;

                    var divTags = amenTag.QuerySelectorAll(".specList");
                    foreach (var divTag in divTags)
                    {
                        result += divTag.QuerySelector("h3").InnerText.CleanText() + BREAK_TAG;

                        var h4Tag = divTag.QuerySelector("h4");
                        if (h4Tag != null)
                        {
                            result += h4Tag.InnerText.CleanText() + BREAK_TAG;
                        }

                        var pTags = divTag.QuerySelectorAll("p");
                        if (pTags != null)
                        {
                            foreach (var pTag in pTags)
                            {
                                result += pTag.InnerText.CleanText() + BREAK_TAG;
                            }
                        }

                        var liTags = divTag.QuerySelectorAll("li");
                        if (liTags != null && liTags.Count > 0)
                        {
                            foreach (var liTag in liTags)
                            {
                                result += liTag.InnerText.CleanText() + BREAK_TAG;
                            }
                            result += BREAK_TAG;
                        }
                    }
                }

                var officeHoursTag = _doc.QuerySelector("#officeHoursSection");
                if (officeHoursTag != null)
                {

                    result += "-----OFFICE HOURS-----" + BREAK_TAG + BREAK_TAG;

                    var langTag = officeHoursTag.QuerySelector(".languages");
                    if (langTag != null)
                    {
                        result += langTag.InnerText.CleanText() + BREAK_TAG;
                    }

                    var hoursTag = officeHoursTag.QuerySelector(".officeHoursContainer");
                    if (hoursTag != null)
                    {
                        result += hoursTag.InnerText.CleanText() + BREAK_TAG;
                    }

                    result += BREAK_TAG;
                }

                var contTag = _doc.QuerySelector("#contactSection");
                if (contTag != null)
                {
                    result += "-----CONTACT-----" + BREAK_TAG + BREAK_TAG;

                    var agentTag = contTag.QuerySelector(".agentFullName");
                    if (agentTag != null)
                    {
                        result += agentTag.InnerText.CleanText() + BREAK_TAG;
                    }

                    var phoneTag = contTag.QuerySelector(".phoneNumber");
                    if (phoneTag != null)
                    {
                        result += phoneTag.InnerText.CleanText() + BREAK_TAG;
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
