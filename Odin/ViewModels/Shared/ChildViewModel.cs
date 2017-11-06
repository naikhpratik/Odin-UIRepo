using System.ComponentModel;

namespace Odin.ViewModels.Shared
{
    public class ChildViewModel
    {
        public string Id { get; set; }

        [DisplayName("Name:")]
        public string Name { get; set; }

        [DisplayName("Age:")]
        public int? Age { get; set; }

        [DisplayName("Grade:")]
        public int? Grade { get; set; }

        public bool Deleted { get; set; }
    }
}