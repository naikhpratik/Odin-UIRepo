using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IBrokerFeeTypesRepository
    {
        IEnumerable<BrokerFeeType> GetBrokerFeeTypes();
        BrokerFeeType GetBrokerFeeType(string seValue);
    }
}