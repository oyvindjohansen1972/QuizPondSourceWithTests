using System.ComponentModel.DataAnnotations;

namespace QuizPond.ViewModels
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Keep me logged in!")]
        public bool KeepMeLoggedIn { get; set; }
    }
}
