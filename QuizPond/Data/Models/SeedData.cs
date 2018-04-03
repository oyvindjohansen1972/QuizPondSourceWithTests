
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace QuizPond.Data.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();

            //context.Database.Migrate();

            if(!context.Questions.Any())
            {
                var quiz1 = new Quiz { QuizName = "TV Quiz", UserId = "18e8eeec-c795-42af-bd61-1c192b5c6e6c" };
                context.Add(quiz1);
                context.SaveChanges();

                var question1 = new Question
                {
                    QuizId = quiz1.QuizId,
                    QuestionNr = 1,
                    QuestionString = "Når begynte de første norske prøvesendingene med fjernsyn?",
                    RadioButtonAlternativeAnswer = 2,
                    Answers = new List<Answer> {new  Answer { AnswerText="1949" },
                                                new  Answer { AnswerText="1954" },
                                                new  Answer { AnswerText="1958" } }
                };

                var question2 = new Question
                {
                    QuizId = quiz1.QuizId,
                    QuestionNr = 2,
                    QuestionString = "Når startet TV2 sine sendinger?",
                    RadioButtonAlternativeAnswer = 1,
                    Answers = new List<Answer> {new  Answer { AnswerText="1992" },
                                                new  Answer { AnswerText="1994" },
                                                new  Answer { AnswerText="1995" } }
                };


                var question3 = new Question
                {
                    QuizId = quiz1.QuizId,
                    QuestionNr = 3,
                    QuestionString = "Hvem spilte Derrick i TV-serien med samme navn?",
                    RadioButtonAlternativeAnswer = 3,
                    Answers = new List<Answer> {new  Answer { AnswerText="Peter Falk" },
                                                new  Answer { AnswerText="James Hed" },
                                                new  Answer { AnswerText="Horst Tappert" } }
                };

                var question4 = new Question
                {
                    QuizId = quiz1.QuizId,
                    QuestionNr =4,
                    QuestionString = "Hva het kaféverten i 'Allo 'allo?",
                    RadioButtonAlternativeAnswer = 1,
                    Answers = new List<Answer> {new  Answer { AnswerText="Renè" },
                                                new  Answer { AnswerText="Manuel" },
                                                new  Answer { AnswerText="Hercules" } }
                };

                var question5 = new Question
                {
                    QuizId = quiz1.QuizId,
                    QuestionNr = 5,
                    QuestionString = "Hva het Fleksnes til fornavn?",
                    RadioButtonAlternativeAnswer = 1,
                    Answers = new List<Answer> {new  Answer { AnswerText="Marve" },
                                                new  Answer { AnswerText="Anton" },
                                                new  Answer { AnswerText="Kurt" } }
                };


                context.AddRange(question1, question2,  question3,  question4, question5);                
                context.SaveChanges();

                //var newGame = new NewGame { GameCodeID = "ABCD", QuizId = 1 };
                //context.Add(newGame);
                //context.SaveChanges();

                //var player1 = new Player { GameCodeID = "ABCD", Name = "Julie" };
                //var player2 = new Player { GameCodeID = "ABCD", Name = "Øyvind" };
                //var player3 = new Player { GameCodeID = "ABCD", Name = "Geir" };
                //var player4 = new Player { GameCodeID = "ABCD", Name = "Ola Nordmann" };
                //context.AddRange(player1, player2, player3, player4);
                //context.SaveChanges();


                //-------------------------QUIZ 2

                var quiz2 = new Quiz { QuizName = "Påske Quiz", UserId = "18e8eeec-c795-42af-bd61-1c192b5c6e6c" };
                context.Add(quiz2);
                context.SaveChanges();

                question1 = new Question
                {
                    QuizId = quiz2.QuizId,
                    QuestionNr = 1,
                    QuestionString = "Fra hvilken frukt får vi svisker?",
                    RadioButtonAlternativeAnswer = 2,
                    Answers = new List<Answer> {new  Answer { AnswerText="Pære" },
                                                new  Answer { AnswerText="Plomme" },
                                                new  Answer { AnswerText="Rosin" } }
                };

                question2 = new Question
                {
                    QuizId = quiz2.QuizId,
                    QuestionNr = 2,
                    QuestionString = "Hva heter fargestoffet i gulrøtter?",
                    RadioButtonAlternativeAnswer = 1,
                    Answers = new List<Answer> {new  Answer { AnswerText="Betakaroten" },
                                                new  Answer { AnswerText="Gluten" }}
                };


                question3 = new Question
                {
                    QuizId = quiz2.QuizId,
                    QuestionNr = 3,
                    QuestionString = "Er sopp grønnsaker?",
                    RadioButtonAlternativeAnswer = 2,
                    Answers = new List<Answer> {new  Answer { AnswerText="Ja" },
                                                new  Answer { AnswerText="Nei" }}
                };

                question4 = new Question
                {
                    QuizId = quiz2.QuizId,
                    QuestionNr = 4,
                    QuestionString = "Hvilken grønnsak kaller man jordeple?",
                    RadioButtonAlternativeAnswer = 1,
                    Answers = new List<Answer> {new  Answer { AnswerText="Potet" },
                                                new  Answer { AnswerText="Kålrabi" },
                                                new  Answer { AnswerText="Løk" } }
                };

                question5 = new Question
                {
                    QuizId = quiz1.QuizId,
                    QuestionNr = 5,
                    QuestionString = "Hvilken frukt ble tidligere kaldt kinesiske stikkelsbær?",
                    RadioButtonAlternativeAnswer = 3,
                    Answers = new List<Answer> {new  Answer { AnswerText="Dverg-stikkelsbær" },
                                                new  Answer { AnswerText="fuchia" },
                                                new  Answer { AnswerText="Kiwi" } }
                };


                context.AddRange(question1, question2, question3, question4, question5);
                context.SaveChanges();

                //newGame = new NewGame { QuizId = 2 };
                //context.Add(newGame);
                //context.SaveChanges();

                //var player1 = new Player { GameCodeID = "ABCD", Name = "Julie" };
                //var player2 = new Player { GameCodeID = "ABCD", Name = "Øyvind" };
                //var player3 = new Player { GameCodeID = "ABCD", Name = "Geir" };
                //var player4 = new Player { GameCodeID = "ABCD", Name = "Ola Nordmann" };
                //context.AddRange(player1, player2, player3, player4);
                //context.SaveChanges();



                //-------------------------QUIZ 3

                var quiz3 = new Quiz { QuizName = "Sommer Quiz" };
                context.Add(quiz3);
                context.SaveChanges();

                question1 = new Question
                {
                    QuizId = quiz2.QuizId,
                    QuestionNr = 1,
                    QuestionString = "Hva heter hovedstaden i Frankrike?",
                    RadioButtonAlternativeAnswer = 2,
                    Answers = new List<Answer> {new  Answer { AnswerText="Bruxelles" },
                                                new  Answer { AnswerText="Paris" },
                                                new  Answer { AnswerText="Monin" } }
                };

                question2 = new Question
                {
                    QuizId = quiz2.QuizId,
                    QuestionNr = 2,
                    QuestionString = "Hvilket land grenser USA til i sør?",
                    RadioButtonAlternativeAnswer = 1,
                    Answers = new List<Answer> {new  Answer { AnswerText="Mexico" },
                                                new  Answer { AnswerText="Venesuela" }}
                };


                question3 = new Question
                {
                    QuizId = quiz2.QuizId,
                    QuestionNr = 3,
                    QuestionString = "Hva heter norges største innsjø?",
                    RadioButtonAlternativeAnswer = 2,
                    Answers = new List<Answer> {new  Answer { AnswerText="Tyrifjorden" },
                                                new  Answer { AnswerText="Mjøsa" }}
                };

                question4 = new Question
                {
                    QuizId = quiz2.QuizId,
                    QuestionNr = 4,
                    QuestionString = "Hvilket dyr kalles skogens konge?",
                    RadioButtonAlternativeAnswer = 3,
                    Answers = new List<Answer> {new  Answer { AnswerText="Ulv" },
                                                new  Answer { AnswerText="Hjort" },
                                                new  Answer { AnswerText="Elg" } }
                };

                question5 = new Question
                {
                    QuizId = quiz1.QuizId,
                    QuestionNr = 5,
                    QuestionString = "Hvem var president i USA da Donald Trumph tok over?",
                    RadioButtonAlternativeAnswer = 2,
                    Answers = new List<Answer> {new  Answer { AnswerText="George W Bush" },
                                                new  Answer { AnswerText="Obama" },
                                                new  Answer { AnswerText="Reagan" } }
                };


                //context.AddRange(question1, question2, question3, question4, question5);
                //context.SaveChanges();

                //newGame = new NewGame { QuizId = 3 };
                //context.Add(newGame);
                //context.SaveChanges();

                //var player1 = new Player { GameCodeID = "ABCD", Name = "Julie" };
                //var player2 = new Player { GameCodeID = "ABCD", Name = "Øyvind" };
                //var player3 = new Player { GameCodeID = "ABCD", Name = "Geir" };
                //var player4 = new Player { GameCodeID = "ABCD", Name = "Ola Nordmann" };
                //context.AddRange(player1, player2, player3, player4);
                //context.SaveChanges();

            }
        }

    }
}


// // Create a list of parts.
//        List<Part> parts = new List<Part>();

//// Add parts to the list.
//parts.Add(new Part() { PartName = "crank arm", PartId = 1234});
//        parts.Add(new Part() { PartName = "chain ring", PartId = 1334 });