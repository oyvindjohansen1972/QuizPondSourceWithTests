//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using QuizPond.Data.Interfaces;
//using QuizPond.Data.Models;
//using QuizPond.ViewModels;

//namespace QuizPond.Data.Repositories
//{
//    public class FakeQuestionRepositoryForStoringData : IQuestionRepository
//    {
//        private List<Question> questionsList = new List<Question>();

//        public ICollection<Question> Questions => questionsList;

//        public ICollection<Player> GetPlayers(string GameCode)
//        {
//            throw new NotImplementedException();
//        }

//        public string getQuizName(string GameCode)
//        {
//            throw new NotImplementedException();
//        }

//        public void SaveQuestions(List<NewQuestionVM> list)
//        {
//            Question q = new Question();

//            foreach(var l in list)
//            {
//                q.QuizId = 1; 
//                q.QuestionNr = l.QuestionNr;
//                q.QuestionString = l.QuestionString;
//                q.RadioButtonAlternativeAnswer = l.RadioButtonAlternativeAnswer;
//                q.Answers = new List<Answer>();
              
//                if ( !(string.IsNullOrEmpty(l.Answer1) || string.IsNullOrWhiteSpace(l.Answer1)) )
//                {
//                    q.Answers.Add(new Answer{ AnswerText = l.Answer1 });
//                }
//                if (!(string.IsNullOrEmpty(l.Answer2) || string.IsNullOrWhiteSpace(l.Answer2)))
//                {
//                    q.Answers.Add(new Answer{ AnswerText = l.Answer2 });
//                }
//                if (!(string.IsNullOrEmpty(l.Answer3) || string.IsNullOrWhiteSpace(l.Answer3)))
//                {
//                    q.Answers.Add(new Answer{ AnswerText = l.Answer3 });
//                }
//                if (!(string.IsNullOrEmpty(l.Answer4) || string.IsNullOrWhiteSpace(l.Answer4)))
//                {
//                    q.Answers.Add(new Answer{ AnswerText = l.Answer4 });
//                }

//                questionsList.Add(q);
//            }
//        }
//    }
//}
