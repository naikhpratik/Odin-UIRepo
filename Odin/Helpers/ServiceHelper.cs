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

        public static string ToServiceCategoryString(ServiceCategory cat)
        {
            switch (cat)
            {
                    case ServiceCategory.InitialConsultation:
                        return "Initial Consultation";
                    case ServiceCategory.WelcomePacket:
                        return "Welcome Packet";
                    case ServiceCategory.SettlingIn:
                        return "Settling In";
                    case ServiceCategory.AreaOrientation:
                        return "Area Orientation";
                    case ServiceCategory.SchoolFinding:
                        return "School Finding";
                    case ServiceCategory.AccompaniedHomeFinding:
                        return "Accompanied Home Finding";
                    case ServiceCategory.UnAccompaniedHomeFinding:
                        return "Unaccompanied Home Finding";
                    case ServiceCategory.LeaseRenewal:
                        return "Lease Renewal";
                    case ServiceCategory.Departure:
                        return "Departure";
                    default:
                        return String.Empty;
            }
        }
    }
}