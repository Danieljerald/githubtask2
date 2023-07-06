using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace Digitalrestaurantorderplatform.Models
{
    public class SignUpModel
    {
        private string Name;
        private string Email;
        private string Password;
        private string Cpassword;


        [Key]
        [Required]
        [RegularExpression(@"^[a-zA-Z]((?!(\.|))|\.(?!(_|\.))|[a-zA-Z0-9]){6,18}[a-zA-Z0-9]$",ErrorMessage ="Username should contains letters,numbers and Name should not start with number")]
        public string name{
            get{return Name;}
            set{Name=value;}
            }
        [Required]
        public string email
        {
            get{return Email;}
            set{Email=value;}
        }
        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$",ErrorMessage ="Must contain at least one number and one uppercase and lowercase letter, and at least 8 or more characters")]
        public string password {
            get{return Password;}
            set{Password=value;}
        }

        [NotMapped]
        public string cpassword {
            get{return Cpassword;}
            set{Cpassword=value;}
        }
    }
}