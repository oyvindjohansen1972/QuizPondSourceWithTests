using System;
using System.Collections.Generic;
using System.Text;
using QuizPond.Controllers;
using QuizPond.Data.Interfaces;
using QuizPond.Data.Models;
using QuizPond.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Collections;
using Newtonsoft.Json;
using QuizPond.Infrastructure;
using QuizPond.Data.Repositories;
using System.Linq;

namespace QuizPond.Tests
{
    public class GameControllerTests
    {        
        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 1)]
        [InlineData(5, 3)]
        public void Run_InputNextButtonPressed_NrStoredInListWithChosenAlternativeAnswersForAllQuestionsAtIndexEqualsPageNr(int page, int? radiobutton)
        {
            //Arrange
            GameController gameController = setupBeforeTests();

            //Act            
            gameController.Run(page, radiobutton, false, "", "buttonNext", ""); 
            var result = gameController.MyContextAccessor.HttpContext.Session.GetJson<int?[]>("ListWithChosenAlternativeAnswersForAllQuestions");

            //Assert
            Assert.Equal(radiobutton, result[page-1]);
        }


        [Theory]       
        [InlineData(6, 1)] // I only got 5 questions should not be able to write to question nr 6
        [InlineData(2, 5)] // Max number of answer alternatives are 4 radiobutton can never be 5
        public void Run_InputNextButtonPressed_NrISNOTStoredInListWithChosenAlternativeAnswersForAllQuestionsAtIndexEqualsPageNr(int page, int? radiobutton)
        {
            //Arrange
            GameController gameController = setupBeforeTests();

            //Act            
            gameController.Run(page, radiobutton, false, "", "buttonNext", "");
            var result = gameController.MyContextAccessor.HttpContext.Session.GetJson<int?[]>("ListWithChosenAlternativeAnswersForAllQuestions");

            //Assert
            Assert.NotEqual(radiobutton, result?[page-1]);
        }


        //-------------------------------------------------------------------------------------------


        public void ValidateInput_InputNoButtonPressed_ModelstateIsNotValid()
        {
            //Arrange
            GameController gameController = setupBeforeTests();

            //Act 
            gameController.ValidateInput(1, 2, "", "", "");

            //Assert
            Assert.False(gameController.ModelState.IsValid);
        }



        [Theory]
        [InlineData(null, null)] // page is null should not work
        [InlineData(1, 6)] // I have a max of have 4 questionalternatives this should fail
        [InlineData(7, null)] // I have 5 questions this should fail
        public void ValidateInput_Input_ModelstateIsNotValid(int? page, int? radiobutton)
        {
            //Arrange
            GameController gameController = setupBeforeTests();

            //Act
            gameController.ValidateInput(page, radiobutton, "", "buttonNext", "");

            //Assert
            Assert.False(gameController.ModelState.IsValid);
        }


        [Theory]
        [InlineData(1, null)]
        [InlineData(4, null)] // I have 5 questions this should work
        [InlineData(5, null)] // I have 5 questions this should work
        public void ValidateInput_Input_ModelstateIsValid(int? page, int? radiobutton)
        {
            //Arrange
            GameController gameController = setupBeforeTests();

            //Act
            gameController.ValidateInput(page, radiobutton, "", "buttonNext", "");

            //Assert
            Assert.True(gameController.ModelState.IsValid);
        }


        public GameController setupBeforeTests()
        {
            //Arrange

            var mockHttpContext = new Mock<IHttpContextAccessor>();
            mockHttpContext.Setup(x => x.HttpContext.Session).Returns(new MyTestSession() { Id = "abba1", IsAvailable = true });
            var inMemoryDatabase = InMemoryDatabase.GetContextFromInMemoryDatabase();
            GameController gameController = new GameController(new EFQuestionRepository(inMemoryDatabase), mockHttpContext.Object);

            if (!inMemoryDatabase.Questions.Any())
            {
                var question1 = new Question
                {
                    QuestionNr = 1,
                    QuestionString = "(inMemoryDatabase)Når begynte de første norske prøvesendingene med fjernsyn?",
                    RadioButtonAlternativeAnswer = 2,
                    Answers = new List<Answer> {new  Answer { AnswerText="1949" },
                                                new  Answer { AnswerText="1954" },
                                                new  Answer { AnswerText="1958" } }
                };

                var question2 = new Question
                {
                    QuestionNr = 2,
                    QuestionString = "(inMemoryDatabase)Når startet TV2 sine sendinger?",
                    RadioButtonAlternativeAnswer = 1,
                    Answers = new List<Answer> {new  Answer { AnswerText="1992" },
                                                new  Answer { AnswerText="1994" },
                                                new  Answer { AnswerText="1995" } }
                };


                var question3 = new Question
                {
                    QuestionNr = 3,
                    QuestionString = "(inMemoryDatabase)Hvem spilte Derrick i TV-serien med samme navn?",
                    RadioButtonAlternativeAnswer = 3,
                    Answers = new List<Answer> {new  Answer { AnswerText="Peter Falk" },
                                                new  Answer { AnswerText="James Hed" },
                                                new  Answer { AnswerText="Horst Tappert" } }
                };

                var question4 = new Question
                {
                    QuestionNr = 4,
                    QuestionString = "(inMemoryDatabase)Hva het kaféverten i 'Allo 'allo?",
                    RadioButtonAlternativeAnswer = 1,
                    Answers = new List<Answer> {new  Answer { AnswerText="Renè" },
                                                new  Answer { AnswerText="Manuel" },
                                                new  Answer { AnswerText="Hercules" } }
                };

                var question5 = new Question
                {
                    QuestionNr = 5,
                    QuestionString = "(inMemoryDatabase)Hva het Fleksnes til fornavn?",
                    RadioButtonAlternativeAnswer = 1,
                    Answers = new List<Answer> {new  Answer { AnswerText="Marve" },
                                                new  Answer { AnswerText="Anton" },
                                                new  Answer { AnswerText="Kurt" } }
                };


                inMemoryDatabase.AddRange(question1, question2, question3, question4, question5);
                inMemoryDatabase.SaveChanges();
            }

            int page_nr = 1;
            int numberOfQuestions = inMemoryDatabase.Questions.Count();
            List<ShowQuestionsVM> ListVM = new List<ShowQuestionsVM>();

            foreach (Question q in inMemoryDatabase.Questions)
            {
                ListVM.Add(new ShowQuestionsVM()
                {
                    Page = page_nr++,
                    QuestionId = q.QuestionNr,
                    TotalPages = numberOfQuestions,
                    RadioButtonChosenAlternativeAnswer = null,
                    QuestionString = q.QuestionString,
                    ListOfAnswers = q.Answers
                });
            }
            gameController.MyContextAccessor.HttpContext.Session.SetJson("ListWithAllQuestionsAndAnswers", ListVM);
            int?[] ChosenAlternativeAnswer = new int?[numberOfQuestions + 1];
            gameController.MyContextAccessor.HttpContext.Session.SetJson("ListWithChosenAlternativeAnswersForAllQuestions", ChosenAlternativeAnswer);

            return gameController;
        }


        //--------------------------------------------------------------

        [Fact]
        public void ValidateAllQuestionsAnswered_CompleteArray_EmptyArrayReturned()
        {
            //Arrange
            GameController gameController = setupBeforeTests();
            List<int?> testList = new List<int?> { 1, 2, 3, 4, 5, 6 };

            //Act
            var result = (gameController.ValidateIfAllQuestionsAreAnswered(testList)).ToList();
           
            //Assert
            Assert.Equal(0, result.Count);
        }


        [Fact]
        public void ValidateAllQuestionsAnswered_CompleteArray_ArrayWith1ItemReturned()
        {
            //Arrange
            GameController gameController = setupBeforeTests();
            List<int?> testList = new List<int?> { 1, null, 3, 4, 5, 6 };

            //Act
            var result = (gameController.ValidateIfAllQuestionsAreAnswered(testList)).ToList();

            //Assert
            Assert.Equal(1, result.Count);
        }


    }
}
