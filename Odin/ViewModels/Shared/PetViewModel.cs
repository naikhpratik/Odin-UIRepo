using System.ComponentModel;

namespace Odin.ViewModels.Shared
{
    public class PetViewModel
    {
        public string Id { get; set; }

        [DisplayName("Pet Type")]
        public string Type { get; set; }

        [DisplayName("Breed")]
        public string Breed { get; set; }

        [DisplayName("Weight/Size")]
        public string Size { get; set; }

        public bool Deleted { get; set; }
    }
}