namespace Odin.ViewModels.Order.Index
{
    public class OrderIndexManagerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}