using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPond.ViewModels
{
    public class ShowQuestionsWithAnswersFromQuizVM
    {
        public ShowQuestionsWithAnswersFromQuizVM()
        {
            questionsWithAnswers = new List<QuestionsWithAnswersVM>();
        }

        public string  QuizName { get; set; }
        public bool ShowAnswersToPlayer { get; set; }
        public ICollection<QuestionsWithAnswersVM> questionsWithAnswers { get; set; } 
    }   
}
