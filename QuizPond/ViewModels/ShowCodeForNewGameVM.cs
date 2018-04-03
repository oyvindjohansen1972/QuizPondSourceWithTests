using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPond.ViewModels
{
    public class ShowCodeForNewGameVM
    {
        public int QuizId { get; set; }
        public string GameCode { get; set; }
        public string NameOfQuiz { get; set; }
        public bool ShowAnswers { get; set; }
    }
}
