﻿namespace Odin.ViewModels.Shared
{
    public class ServiceTypeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ActionLabel { get; set; }
        public int Category { get; set; }

        public int SortOrder { get; set; }
    }
}