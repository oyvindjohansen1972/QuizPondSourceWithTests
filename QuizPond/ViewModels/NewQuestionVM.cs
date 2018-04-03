using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuizPond.Data.Models;

namespace QuizPond.ViewModels
{
    public class NewQuestionVM
    {
        public int QuestionNr { get; set; } 
        [Required(ErrorMessage = "Please enter a question!")]
        public string QuestionString { get; set; }        
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }        
        public string Answer4 { get; set; }
        public int? RadioButtonAlternativeAnswer { get; set; }


        public void  ConvertFromDataModel(Question question)
        {
            this.QuestionNr = question.QuestionNr;
            this.QuestionString = question.QuestionString;
            this.Answer1 = question.Answers.ElementAt(0).ToString();
            this.Answer2 = question.Answers.ElementAt(1).ToString();
            this.Answer3 = question.Answers.ElementAt(2).ToString();
            this.Answer4 = question.Answers.ElementAt(3).ToString();
            this.RadioButtonAlternativeAnswer = question.RadioButtonAlternativeAnswer;
        }


        public override bool Equals(Object obj)
        {
            if (!(obj is NewQuestionVM))
                return false;

            var other = obj as NewQuestionVM;

            if ( this.Answer1 != other.Answer1 ||
                 this.Answer2 != other.Answer2 ||
                 this.Answer3 != other.Answer3 ||
                 this.Answer4 != other.Answer4 ||
                 this.QuestionNr != other.QuestionNr ||
                 this.QuestionString != other.QuestionString ||
                 this.RadioButtonAlternativeAnswer != other.RadioButtonAlternativeAnswer)
            {
                return false;
            }                

            return true;
        }
    }
}
