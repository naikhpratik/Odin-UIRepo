﻿using Odin.Data.Core.Models;
using Odin.Helpers;
using System;

namespace Odin.ViewModels.Shared
{
    public class ServiceViewModel
    {
        public string Id { get; set; }

        public DateTime? ScheduledDate { get; set; }        
        public string ScheduledDateDisplay => DateHelper.GetViewFormat(ScheduledDate, false);

        public string ScheduledTimeDisplay => DateHelper.GetViewFormat(ScheduledDate, true);

        public DateTime? CompletedDate { get; set; }
        public string CompletedDateDisplay => DateHelper.GetViewFormat(CompletedDate, false);

        public bool Selected { get; set; }

        public string Notes { get; set; }

        public string Name { get; set; }

        public string ActionLabel { get; set; }
        public int ServiceTypeSortOrder { get; set; }

        public ServiceCategory Category { get; set; }
    }
}