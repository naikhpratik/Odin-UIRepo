﻿using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IPetsRepository
    {
        Pet GetChildById(string id);
    }
}