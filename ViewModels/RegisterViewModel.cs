using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Username can't be blank")]
        [RegularExpression(@"^[A-Za-z ]*$", ErrorMessage = "Alphabets only")]
        [MaxLength(40, ErrorMessage = "Username can be maximum 40 characters long")]
        [MinLength(2, ErrorMessage = "Username should contain at least 2 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email {  get; set; }

        [Required(ErrorMessage ="Mobile Number can't be blank")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only Numbers Required")]
        public string Mobile { get; set; }

        public string ProfileImage { get; set; }

        public bool RemoveImage { get; set; }

    }
}