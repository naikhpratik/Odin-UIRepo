using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IDepositTypesRepository
    {
        IEnumerable<DepositType> GetDepositTypesList();
        DepositType GetDepositType(string seValue);
    }
}