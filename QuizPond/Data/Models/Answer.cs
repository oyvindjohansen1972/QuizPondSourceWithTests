using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QuizPond.Data.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int AnswerNr { get; set; }
        [MaxLength(255, ErrorMessage = "The answer can only contain 255 characters.")]
        public string AnswerText { get; set; }
        public Question Question { get; set; }
        public int QuestionId { get; set; }
    }
}
