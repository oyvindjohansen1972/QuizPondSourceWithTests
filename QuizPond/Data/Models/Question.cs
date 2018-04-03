using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QuizPond.Data.Models
{
    public class Question
    {
        public int QuestionId { get; set; }                 
        public int QuestionNr { get; set; }
        [MaxLength(255, ErrorMessage = "The question can only contain 255 characters.")]
        public string QuestionString { get; set; }            
        public ICollection<Answer> Answers { get; set; }
        public int RadioButtonAlternativeAnswer { get; set; }        
        public Quiz Quiz { get; set; }
        public int QuizId { get; set; }
    }
}
