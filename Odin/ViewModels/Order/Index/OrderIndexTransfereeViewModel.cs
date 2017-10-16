namespace Odin.ViewModels.Order.Index
{
    public class OrderIndexTransfereeViewModel
    {        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}