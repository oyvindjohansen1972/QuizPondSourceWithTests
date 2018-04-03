using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPond.ViewModels
{
    public class PreviewAllQuestionsWithAnswersVM   
    {
        public PreviewAllQuestionsWithAnswersVM()
        {
            questionsWithAnswers = new List<QuestionsWithAnswersVM>();
        }

        public string QuizName { get; set; }       
        public ICollection<QuestionsWithAnswersVM> questionsWithAnswers { get; set; }
    }   
}

