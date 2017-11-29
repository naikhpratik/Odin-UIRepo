namespace Odin.Data.Core.Models
{
    public class ServiceType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ActionLabel { get; set; }
        public int SortOrder { get; set; }

        public int Default { get; set; }

        public ServiceCategory Category { get; set; }
    }
}
