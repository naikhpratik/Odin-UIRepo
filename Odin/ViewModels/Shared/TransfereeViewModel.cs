using System;
using System.ComponentModel;

namespace Odin.ViewModels.Shared
{
    public class TransfereeViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DisplayName("Email:")]
        public string Email { get; set; }

        private string _phoneNumber;
        [DisplayName("Phone:")]
        public string PhoneNumber {
            get { return _phoneNumber ?? String.Empty; }
            set { _phoneNumber = value; }
        }

        [DisplayName("Name:")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        } 
    }
}