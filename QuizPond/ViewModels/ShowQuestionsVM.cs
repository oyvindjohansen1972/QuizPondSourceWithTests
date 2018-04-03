using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizPond.Data.Models;

namespace QuizPond.ViewModels
{
    public class ShowQuestionsVM
    {
        public bool AllQuestionsAnswered { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int QuestionId { get; set; }
        public string QuestionString { get; set; }        
        public int? RadioButtonChosenAlternativeAnswer { get; set; }
        public ICollection<Answer> ListOfAnswers = new List<Answer>();             
    }
}
