using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QuizPond.Data.Models
{
    public class NewGame
    {        
        public string GameCodeId { get; set; }
        public DateTime TimeStamp { get; set; }
        public Quiz Quiz { get; set; }
        public int QuizId { get; set; }
        public ICollection<Player> Players { get; set; }
        public bool ShowAnswersToPlayer { get; set; }
    }
}
