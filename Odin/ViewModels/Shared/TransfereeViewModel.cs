using System;

namespace Odin.ViewModels.Shared
{
    public class TransfereeViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        private string _phoneNumber;
        public string PhoneNumber {
            get { return _phoneNumber ?? String.Empty; }
            set { _phoneNumber = value; }
        }

        public string FullName => $"{FirstName} {LastName}";
    }
}