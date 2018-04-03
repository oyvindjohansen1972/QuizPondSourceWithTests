using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPond.ViewModels
{
    public class AccountVM
    {
        [Required, EmailAddress, MaxLength(256), Display(Name="Email Address")]
        public string Email { get; set; }

        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name ="Password")]
        public string Password { get; set; }

        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage ="The passwords does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
