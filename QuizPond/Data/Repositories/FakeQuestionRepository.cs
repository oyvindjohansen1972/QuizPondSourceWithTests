using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizPond.Data.Interfaces;
using QuizPond.Data.Models;
using QuizPond.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace QuizPond.Data.Repositories
{
    //public class FakeQuestionRepository : IQuestionRepository
    //{
    //    public IQueryable<Question> Questions => throw new NotImplementedException();

    //    public void SaveQuestions(List<NewQuestionVM> list)
    //    {
    //        using (var context = GetContextFromInMemoryDatabase())           
    //        {
    //            context.Questions.Add(list);
    //            context.SaveChanges();
    //        }
    //    }


        
    //    public ApplicationDbContext GetContextFromInMemoryDatabase()
    //    {
    //        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    //                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
    //                            .Options;
    //        var context = new ApplicationDbContext(options);

    //        return context;
    //    }
       
    //}
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using QuizPond.Data.Interfaces;
//using QuizPond.Data.Models;
//using QuizPond.ViewModels.CreateQuestions;

//namespace QuizPond.Data.Repositories
//{
//    public class FakeQuestionRepositoryForStoringData : IQuestionRepository
//    {
//        

//        public IQueryable<Question> Questions => questionsList.AsQueryable<Question>(); //Er dette ok?

//        public void SaveQuestions(List<NewQuestionVM> list)
//        {
//            Question q = new Question();

//            foreach (var l in list)
//            {
//                q.QuizzOwner = 1; // Må rette dette senere!
//                q.QuestionNr = l.QuestionNr;
//                q.QuestionString = l.QuestionString;
//                q.RadioButtonAlternativeAnswer = l.RadioButtonAlternativeAnswer;
//                q.AnswersList = new List<string>();

//                if (!(string.IsNullOrEmpty(l.Answer1) || string.IsNullOrWhiteSpace(l.Answer1)))
//                {
//                    q.AnswersList.Add(l.Answer1);
//                }
//                if (!(string.IsNullOrEmpty(l.Answer2) || string.IsNullOrWhiteSpace(l.Answer2)))
//                {
//                    q.AnswersList.Add(l.Answer2);
//                }
//                if (!(string.IsNullOrEmpty(l.Answer3) || string.IsNullOrWhiteSpace(l.Answer3)))
//                {
//                    q.AnswersList.Add(l.Answer3);
//                }
//                if (!(string.IsNullOrEmpty(l.Answer4) || string.IsNullOrWhiteSpace(l.Answer4)))
//                {
//                    q.AnswersList.Add(l.Answer4);
//                }

//                questionsList.Add(q);
//            }
//        }
//    }
//}

