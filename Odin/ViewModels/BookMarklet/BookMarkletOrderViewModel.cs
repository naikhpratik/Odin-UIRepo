namespace Odin.ViewModels.BookMarklet
{
    public class BookMarkletOrderViewModel
    {
        public string Id { get; set; }
        public string TransfereeFirstName { get; set; }
        public string TransfereeLastName { get; set; }
        public string TransfereeFullName
        {
            get
            {
                return $"{TransfereeFirstName} {TransfereeLastName}";
            }
        }
    }
}