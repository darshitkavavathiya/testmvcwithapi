
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace testmvc.Models
{
    public class Resetpassword
    {


        public int UserId { get; set; }


        [Required(ErrorMessage = "Please Enter new Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,14}$", ErrorMessage = "Please enter Stronge Password")]
        // [DisplayName("New Password")]
        public string NewPassword { get; set; }



        [NotMapped] // Does not effect with your database
        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Compare("NewPassword")]
        // [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
