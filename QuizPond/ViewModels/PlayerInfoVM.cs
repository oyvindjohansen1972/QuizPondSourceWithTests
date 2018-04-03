using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QuizPond.ViewModels
{
    public class PlayerInfoVM
    {
        public string QuizName { get; set; }    
        public string GameCode { get; set; }
        [Required(ErrorMessage ="You need to enter a nickname!")]
        public string NickName { get; set; }
        public int Score { get; set; }
    }
}
