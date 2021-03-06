﻿using System.Collections.Generic;
using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface ITransportationTypesRepository
    {
        IEnumerable<TransportationType> GetTransportationTypes();
    }
}