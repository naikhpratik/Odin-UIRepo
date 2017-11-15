using Odin.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Odin.Helpers
{
    public static class ServiceHelper
    {
        public static IEnumerable<ServiceCategory> GetCategoriesForServiceFlag(int serviceFlag)
        {
            var catEnums = Enum.GetValues(typeof(ServiceCategory)).Cast<ServiceCategory>();
            var cats = new List<ServiceCategory>();
            foreach (var cat in catEnums)
            {
                //Use service bit flag on order to determine what categories have been selected in SE.
                if ((serviceFlag & (int)cat) > 0)
                {
                    cats.Add(cat);
                }
            }

            return cats;
        }
    }
}