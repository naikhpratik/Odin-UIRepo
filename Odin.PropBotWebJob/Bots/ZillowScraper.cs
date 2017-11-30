using BatchJobPropBot.Dtos;
using BatchJobPropBot.interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using USAddress;

namespace BatchJobPropBot.Scrapers
{
    public class ZillowScraper : IScraper
    {
        private string _url;

        public ZillowScraper(string url)
        {
            _url = url;
        }

        public HousingPropertyDto Scrape(string orderId)
        {
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(_url);
            var docTest = doc.DocumentNode.OuterHtml;
            AddressParser addressParser = new AddressParser();

            //Try json first. Has a lot of info including lat/lng
            var buildingJson = doc.QuerySelector("#bdpState");
            if (buildingJson != null)
            {
                var test = buildingJson.InnerText;
            }
            else
            {
                //Try add json. Also has lat lng.
                var adJson = doc.QuerySelector("#factsFeaturesTelecomAd");
                if (adJson != null)
                {

                }
                else
                {
                    //Try to get address from property title
                    var propAddrHeader = doc.QuerySelector(".zsg-content-header > h1");
                    if (propAddrHeader != null)
                    {
                        
                        var adResult = addressParser.ParseAddress(propAddrHeader.InnerText);
                    }
                    else
                    {
                        //Try to get address from building header.
                        var buildingAddrHeader = doc.QuerySelector(".zsg-media-bd");
                        if (buildingAddrHeader != null)
                        {
                            //Weird nameless building: https://www.zillow.com/homes/for_rent/125-Edgemere-Rd,-Boston,-MA-02132_rb/?fromHomePage=true&shouldFireSellPageImplicitClaimGA=false&fromHomePageTab=rent
                            if (buildingAddrHeader.QuerySelector("span") != null)
                            {
                                var addrResult = addressParser.ParseAddress(
                                    buildingAddrHeader.QuerySelector("h1").InnerText + ", " +
                                    buildingAddrHeader.QuerySelector("h2").InnerText);

                            }
                            //Named property
                            else
                            {
                                var addrResult =
                                    addressParser.ParseAddress(buildingAddrHeader.QuerySelector("h2").InnerText);
                            }
                        }
                        else
                        {
                            //Worst Case:  Get address from page title.
                            var addrResult = addressParser.ParseAddress(
                                buildingAddrHeader.QuerySelector("title").InnerText);
                        }
                    }
                }
            }

            return new HousingPropertyDto(){OrderId = orderId};
        }

        public HousingPropertyImagesDto ScrapeImages(string hpId)
        {
            return null;
        }
    }
}
