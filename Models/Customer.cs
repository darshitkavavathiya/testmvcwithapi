using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace testmvc.Models
{
    public class Customer
    {

        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,14}$", ErrorMessage = "Please enter Stronge Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Phone No")]
        [StringLength(10, ErrorMessage = "Please Enter Valid Phone No")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please Enter Valid Phone No")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Please Enter Address")]
        public string Address { get; set; }
        [DisplayName("Country")]
        public string CountryName { get; set; }
        [DisplayName ("State")]
        public string StateName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [DisplayName("Country Name")]
        public int? Countryid{ get; set; }
        [DisplayName("State Name")]
        public int? Stateid { get; set; }
        
     
    }
}
