using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace testmvc.Models
{
    public class User 
    {
       
       
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string UserName { get; set; }

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

        [DisplayName("Active")]
        public bool IsActive { get; set; }




    }


}
