using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Rent
    {
        public int Id { get; set; }

        public decimal? HousingBudget { get; set; }
        //TODO: The rest of the Housing Preferences
        
        public Order Order { get; set; }
    }
}
