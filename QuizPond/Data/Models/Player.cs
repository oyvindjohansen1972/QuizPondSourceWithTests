using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPond.Data.Models
{
    public class Player
    {       
        public int  PlayerId { get; set; }  //PK
        public string Name { get; set; }
        public string Answers { get; set; }
        public int Score { get; set; }
        public NewGame NewGame { get; set; }        
        public string GameCodeId { get; set; } //FK
    }
}
