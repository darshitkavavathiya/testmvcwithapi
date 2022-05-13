using System.ComponentModel.DataAnnotations;
namespace testmvc.Models
{
    public class Authentication
    {

        public int Id { get; set; }

        //[Required(ErrorMessage = "Please Enter Last Name")]
        public string UserName { get; set; }


        //[Required(ErrorMessage = "Please Enter Password")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,14}$", ErrorMessage = "Please enter Stronge Password")]
        public string Password { get; set; }
    }
}
