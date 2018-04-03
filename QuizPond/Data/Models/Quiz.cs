using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPond.Data.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public string UserId { get; set; }
        public ICollection<NewGame> NewGames { get; set; }        
    }
}
