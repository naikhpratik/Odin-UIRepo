using System;
using System.ComponentModel;
using Odin.Helpers;
using System.ComponentModel.DataAnnotations;

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
            get
            {
                return DateHelper.GetViewFormat(_phoneNumber);                
            }
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
        
        [DisplayName("Invite Status: ")]
        public string InviteStatus { get; set; }
    }
}