using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPond.ViewModels
{  
    public class QuestionsWithAnswersVM
    {
        public int QuestionNr { get; set; }
        public string QuestionString { get; set; }
        public string AnswerGivenByPlayer { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
