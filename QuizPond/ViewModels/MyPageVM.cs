using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPond.ViewModels
{
    public class MyPageVM
    {
        public List<QuizToStartVM> MyQuizzes { get; set; }
    }

    public class QuizToStartVM
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; }   
        public bool HasStartedNewGames { get; set; }
    }
}
