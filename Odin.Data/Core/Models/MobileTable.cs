using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Server.Tables;

namespace Odin.Data.Core.Models
{
    public abstract class MobileTable : ITableData
    {
        [Key]
        [TableColumn(TableColumnType.Id)]
        public string Id { get; set; }
        [TableColumn(TableColumnType.Version)]
        [Timestamp]
        public byte[] Version { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index(IsClustered = true)]
        [TableColumn(TableColumnType.CreatedAt)]
        public DateTimeOffset? CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [TableColumn(TableColumnType.UpdatedAt)]
        public DateTimeOffset? UpdatedAt { get; set; }
        [TableColumn(TableColumnType.Deleted)]
        public bool Deleted { get; set; }
    }
}
