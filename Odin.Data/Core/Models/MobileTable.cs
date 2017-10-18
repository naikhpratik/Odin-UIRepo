using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;

namespace Odin.Data.Core.Models
{
    /// <inheritdoc />
    /// <summary>
    /// When inheriting from mobile table if you call a constructor make sure to call : base() after the constructor
    /// </summary>
    public abstract class MobileTable : EntityData
    {
        protected MobileTable()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
