using Newtonsoft.Json;
using Odin.PropBotWebJob.Extensions;
using System;
using System.Text.RegularExpressions;

namespace Odin.PropBotWebJob.Dtos.Trulia
{
    public class TruliaRentDto
    {
        [JsonProperty("streetNumber")]
        public string StreetNumber { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        public string Street1
        {
            get { return StreetNumber + ' ' + Street; }
        }

        [JsonProperty("apartmentNumber")]
        public string Street2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("stateCode")]
        public string State { get; set; }

        [JsonProperty("zipCode")]
        public string PostalCode { get; set; }

        [JsonProperty("numBedrooms")]
        public int? NumberOfBedrooms { get; set; }

        [JsonProperty("numBathrooms")]
        public decimal? NumberOfBathrooms { get; set; }

        [JsonProperty("sqft")]
        public int? Sqft { get; set; }

        [JsonProperty("formattedSqft")]
        public string FormattedSqft { get; set; }

        public int? SquareFootage
        {
            get
            {
                if (Sqft.HasValue)
                {
                    return Sqft;
                }
                else
                {
                    Regex numReg = new Regex(@"[+-]?(\d*\.)?\d+");
                    int sqftOut;
                    if (Int32.TryParse(numReg.Match(FormattedSqft).Value.CleanNumeric(), out sqftOut))
                    {
                        return sqftOut;
                    }
                }

                return null;
            }
        }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("formattedPrice")]
        public string FormattedPrice { get; set; }

        public decimal? Amount
        {
            get
            {
                if (Price.HasValue)
                {
                    return Price;
                }
                else
                {
                    Regex numReg = new Regex(@"[+-]?(\d*\.)?\d+");
                    decimal priceOut;
                    if (decimal.TryParse(numReg.Match(FormattedPrice).Value.CleanNumeric(), out priceOut))
                    {
                        return priceOut;
                    }
                }

                return null;
            }
        }

        [JsonProperty("latitude")]
        public decimal? Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal? Longitude { get; set; }
    }
}
