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
using QuizPond.Tests.Fakes;

namespace QuizPond.Tests
{
    public class CreateQuestionsControllerTests
    {
        private CreateQuestionsController setupBeforeTests()
        {
            var mock = new Mock<IQuestionRepository>();
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            IUserManagerWrapper fakeUserManagerRepository = new FakeUserManagerRepository();
            mockHttpContext.Setup(x => x.HttpContext.Session).Returns(new MyTestSession() { Id = "abba1", IsAvailable = true });
            return new CreateQuestionsController(fakeUserManagerRepository, mock.Object, mockHttpContext.Object);
        }

        private ApplicationDbContext inMemoryDatabase { get; set; }

        private CreateQuestionsController setupBeforeTestWithFakeDb()
        {
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            mockHttpContext.Setup(x => x.HttpContext.Session).Returns(new MyTestSession() { Id = "abba1", IsAvailable = true });
            IUserManagerWrapper fakeUserManagerRepository = new FakeUserManagerRepository();
            inMemoryDatabase = InMemoryDatabase.GetContextFromInMemoryDatabase();
            return new CreateQuestionsController(fakeUserManagerRepository, new EFQuestionRepository(inMemoryDatabase), mockHttpContext.Object);
        }

        [Fact]
        public void AddNewQuiz_InputNewAddNewQuizVM_StoreToInMemoryDb()
        {
            //Arrange            
            CreateQuestionsController createQuestionsController = setupBeforeTestWithFakeDb();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object

            //Act
            AddNewQuizVM addNewQuizVM = new AddNewQuizVM { QuizName = "TestQuiz", UserId = "1" };
            createQuestionsController.AddNewQuiz(addNewQuizVM);
            var result = inMemoryDatabase.Quizzes.FirstOrDefault();
       
            //Assert
            Assert.Equal(addNewQuizVM.QuizName, result.QuizName);
            Assert.Equal(addNewQuizVM.UserId, result.UserId);
        }

        // ----------------------FINISHED---------------------------------

        [Fact]
        public void Add_XXXXXXFinishedPressedWhenOnPage1TwoQuestionsAddedInSessionAndFormFilledWithValidData_StoreToInMemoryDb()
        {
            //Arrange            
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            mockHttpContext.Setup(x => x.HttpContext.Session).Returns(new MyTestSession() { Id = "abba1", IsAvailable = true });
            IUserManagerWrapper fakeUserManagerRepository = new FakeUserManagerRepository();
            var inMemoryDatabase = InMemoryDatabase.GetContextFromInMemoryDatabase();
            CreateQuestionsController createQuestionsController = new CreateQuestionsController(fakeUserManagerRepository, new EFQuestionRepository(inMemoryDatabase), mockHttpContext.Object);

            AddNewQuizVM addNewQuizVM = new AddNewQuizVM { QuizId = 1, QuizName = "TestQuiz", UserId = "UserId-GUID" };
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuizId", addNewQuizVM);
            inMemoryDatabase.Quizzes.Add(new Quiz { QuizId = addNewQuizVM.QuizId, QuizName = addNewQuizVM.QuizName, UserId = addNewQuizVM.UserId });
            inMemoryDatabase.SaveChanges();

            List<NewQuestionVM> list = new List<NewQuestionVM>();
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object


            NewQuestionVM question = new NewQuestionVM() // filling the list
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 3,
                QuestionString = "Test"
            };
            list.Add(question);

            question = new NewQuestionVM() // filling the list
            {
                QuestionNr = 2,
                Answer1 = "A1-2",
                Answer2 = "A2-2",
                Answer3 = "A3-2",
                Answer4 = "A4-2",
                RadioButtonAlternativeAnswer = 2,
                QuestionString = "Test2"
            };
            list.Add(question);

            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object

            NewQuestionVM question2 = new NewQuestionVM() // filling the list
            {
                QuestionNr = 1,
                Answer1 = "A1-1",
                Answer2 = "A2-1",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 3,
                QuestionString = "Test"
            };

            //Act
            NewQuestionVM newQuestion = new NewQuestionVM();
            createQuestionsController.ButtonFinished(question2, newQuestion);
            var result = inMemoryDatabase.Questions.Count();

            //Assert           
            Assert.Equal(2, result);
        }



        [Fact]
        public void Add_FinishedPressedWhenOnPage1OneQuestionAddedInSessionAndFormFilledWithValidData_StoreToInMemoryDb()
        {
            //Arrange            
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            mockHttpContext.Setup(x => x.HttpContext.Session).Returns(new MyTestSession() { Id = "abba1", IsAvailable = true });
            IUserManagerWrapper fakeUserManagerRepository = new FakeUserManagerRepository();
            var inMemoryDatabase = InMemoryDatabase.GetContextFromInMemoryDatabase();
            CreateQuestionsController createQuestionsController = new CreateQuestionsController(fakeUserManagerRepository, new EFQuestionRepository(inMemoryDatabase), mockHttpContext.Object);

            AddNewQuizVM addNewQuizVM = new AddNewQuizVM { QuizId = 1, QuizName = "TestQuiz", UserId = "UserId-GUID" };
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuizId", addNewQuizVM);
            inMemoryDatabase.Quizzes.Add(new Quiz { QuizId = addNewQuizVM.QuizId, QuizName = addNewQuizVM.QuizName, UserId = addNewQuizVM.UserId });
            inMemoryDatabase.SaveChanges();

            List<NewQuestionVM> list = new List<NewQuestionVM>();
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object


            NewQuestionVM question = new NewQuestionVM() // filling the list
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 3,
                QuestionString = "Test"
            };
            list.Add(question);
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object

            NewQuestionVM question2 = new NewQuestionVM() // filling the list
            {
                QuestionNr = 1,
                Answer1 = "A1-1",
                Answer2 = "A2-1",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 3,
                QuestionString = "Test"
            };              

            //Act
            NewQuestionVM newQuestion = new NewQuestionVM();
            createQuestionsController.ButtonFinished(question2, newQuestion);
            var result = inMemoryDatabase.Questions.FirstOrDefault();

            //Assert           
            Assert.Equal(question2.QuestionNr, result.QuestionNr);
            Assert.Equal(question2.Answer1, result.Answers.ElementAt(0).AnswerText);
            Assert.Equal(question2.Answer2, result.Answers.ElementAt(1).AnswerText);
            Assert.Equal(question2.Answer3, result.Answers.ElementAt(2).AnswerText);
            Assert.Equal(question2.Answer4, result.Answers.ElementAt(3).AnswerText);
            Assert.Equal(question2.RadioButtonAlternativeAnswer, result.RadioButtonAlternativeAnswer);
            Assert.Equal(question2.QuestionString, result.QuestionString);
        }

        [Fact]
        public void Add_FinishedPressedWhenOnPage1NoQuestionAdded_GoTo()
        {
            //Arrange            
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            mockHttpContext.Setup(x => x.HttpContext.Session).Returns(new MyTestSession() { Id = "abba1", IsAvailable = true });
            IUserManagerWrapper fakeUserManagerRepository = new FakeUserManagerRepository();
            var inMemoryDatabase = InMemoryDatabase.GetContextFromInMemoryDatabase();
            CreateQuestionsController createQuestionsController = new CreateQuestionsController(fakeUserManagerRepository, new EFQuestionRepository(inMemoryDatabase), mockHttpContext.Object);

            List<NewQuestionVM> list = new List<NewQuestionVM>();
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object
            NewQuestionVM question = new NewQuestionVM()          
            {
                QuestionNr = 1,
                Answer1 = "",
                Answer2 = "",
                Answer3 = "A3",
                Answer4 = "",
                RadioButtonAlternativeAnswer = 1,
                QuestionString = "Test"
            };

            //Act
            NewQuestionVM newQuestion = new NewQuestionVM();
            var result = createQuestionsController.ButtonFinished(question, newQuestion) as ViewResult;
                                 
            //Assert
            Assert.Equal("Add", result.ViewName);
        }
        
        [Fact]
        public void Add_FinishedPressedWhenOnPage1NoQuestionAdded_ReturnInvalidModelstate()
        {
            //Arrange            
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            mockHttpContext.Setup(x => x.HttpContext.Session).Returns(new MyTestSession() { Id = "abba1", IsAvailable = true });
            IUserManagerWrapper fakeUserManagerRepository = new FakeUserManagerRepository();
            var inMemoryDatabase = InMemoryDatabase.GetContextFromInMemoryDatabase();
            CreateQuestionsController createQuestionsController = new CreateQuestionsController(fakeUserManagerRepository, new EFQuestionRepository(inMemoryDatabase), mockHttpContext.Object);

            List<NewQuestionVM> list = new List<NewQuestionVM>();
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object
            NewQuestionVM question = new NewQuestionVM();

            //Act
            NewQuestionVM newQuestion = new NewQuestionVM();
            createQuestionsController.ButtonFinished(question, newQuestion);

            //Assert           
            Assert.False(createQuestionsController.ModelState.IsValid);
        }               

        [Fact]
        public void Add_FinishedPressedWhenOnPage1AndOnlyQuestion1IsEnteredCorrectly_StoreToInMemoryDb()
        {
            //Arrange            
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            mockHttpContext.Setup(x => x.HttpContext.Session).Returns(new MyTestSession() { Id = "abba1", IsAvailable = true });
            IUserManagerWrapper fakeUserManagerRepository = new FakeUserManagerRepository();
            var inMemoryDatabase = InMemoryDatabase.GetContextFromInMemoryDatabase();
            CreateQuestionsController createQuestionsController = new CreateQuestionsController(fakeUserManagerRepository, new EFQuestionRepository(inMemoryDatabase), mockHttpContext.Object);

            AddNewQuizVM addNewQuizVM = new AddNewQuizVM { QuizId = 1, QuizName = "TestQuiz", UserId = "UserId-GUID" };
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuizId", addNewQuizVM);
            inMemoryDatabase.Quizzes.Add(new Quiz { QuizId = addNewQuizVM.QuizId, QuizName = addNewQuizVM.QuizName, UserId = addNewQuizVM.UserId });
            inMemoryDatabase.SaveChanges();


            List<NewQuestionVM> list = new List<NewQuestionVM>();
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object
            NewQuestionVM question = new NewQuestionVM() // filling the list
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 3,
                QuestionString = "Test"
            };
            list.Add(question);

            //Act
            NewQuestionVM newQuestion = new NewQuestionVM();
            createQuestionsController.ButtonFinished(question, newQuestion);
            var result = inMemoryDatabase.Questions.FirstOrDefault();
              
            //Assert           
            Assert.Equal(question.QuestionNr, result.QuestionNr);
            Assert.Equal(question.Answer1, result.Answers.ElementAt(0).AnswerText);
            Assert.Equal(question.Answer2, result.Answers.ElementAt(1).AnswerText);
            Assert.Equal(question.Answer3, result.Answers.ElementAt(2).AnswerText);
            Assert.Equal(question.Answer4, result.Answers.ElementAt(3).AnswerText);
            Assert.Equal(question.RadioButtonAlternativeAnswer, result.RadioButtonAlternativeAnswer);
            Assert.Equal(question.QuestionString, result.QuestionString);
        }

        //if someone is trying to manipulate the input 
        [Fact]
        public void Add_FinishedPressedWhenQuestionNrEqualTo3AndOnlyQuestion1IsStored_RedirectedToShowIndexOnMainPage()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            NewQuestionVM question = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 3,
                QuestionString = "Test"
            };
            list.Add(question);
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list);
            NewQuestionVM questionSendIn = new NewQuestionVM()
            {
                QuestionNr = 3,
                Answer1 = "B1",
                Answer2 = "B2",
                Answer3 = "B3",
                Answer4 = "B4",
                RadioButtonAlternativeAnswer = 2,
                QuestionString = "Test2"
            };

            //Act        

            NewQuestionVM newQuestion = new NewQuestionVM();
            RedirectToActionResult result = (RedirectToActionResult)createQuestionsController.ButtonFinished(questionSendIn, newQuestion);
            var sessionResult = createQuestionsController.MyContextAccessor.HttpContext.Session.GetJson<List<NewQuestionVM>>("CreateNewQuestionsList");

            //Assert
            Assert.Equal(null, sessionResult);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Main", result.ControllerName);
        }

        [Fact]
        public void Add_FinishedPressedQuestionAdded_RedirectedToShowNewQuizStored()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTestWithFakeDb();
            AddNewQuizVM addNewQuizVM = new AddNewQuizVM { QuizId = 1, QuizName = "TestQuiz", UserId = "UserId-GUID" };
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuizId", addNewQuizVM);
            inMemoryDatabase.Quizzes.Add(new Quiz { QuizId = addNewQuizVM.QuizId, QuizName = addNewQuizVM.QuizName, UserId = addNewQuizVM.UserId });
            inMemoryDatabase.SaveChanges();

            List<NewQuestionVM> list = new List<NewQuestionVM>();                   
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object
            NewQuestionVM question = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 1,
                QuestionString = "Test"
            };

            //Act
            NewQuestionVM newQuestion = new NewQuestionVM();
            RedirectToActionResult result = (RedirectToActionResult)createQuestionsController.ButtonFinished(question, newQuestion);
           
            //Assert
            Assert.Equal("ShowNewQuizStored", result.ActionName);
            Assert.Equal("Main", result.ControllerName);
        }


        // ----------------------NEXT---------------------------------


        [Fact]
        public void Add_NextPressedValidQuestionAddedToList()
        {
            //Arrange            
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object
            NewQuestionVM question = new NewQuestionVM
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 3,
                QuestionString = "Test"
            };            

            //Act        
            createQuestionsController.Add(question, "", "buttonNext", "");
            var result = createQuestionsController.MyContextAccessor.HttpContext.Session.GetJson<List<NewQuestionVM>>("CreateNewQuestionsList").ElementAt(0);

            //Assert           
            Assert.Equal(question.QuestionNr, result.QuestionNr);
            Assert.Equal(question.Answer1, result.Answer1);
            Assert.Equal(question.Answer2, result.Answer2);
            Assert.Equal(question.Answer3, result.Answer3);
            Assert.Equal(question.Answer4, result.Answer4);
            Assert.Equal(question.RadioButtonAlternativeAnswer, result.RadioButtonAlternativeAnswer);
            Assert.Equal(question.QuestionString, result.QuestionString);
        }

        [Fact]
        public void Add_NextPressedValidQuestionAdded_OpenAddView()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object
            NewQuestionVM question = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 1,
                QuestionString = "Test"
            };

            //Act   
            NewQuestionVM newQuestionVM = new NewQuestionVM();
            var result = createQuestionsController.ButtonNext(question, newQuestionVM) as ViewResult;        

            //Assert
            Assert.Equal("Add", result.ViewName);
        }


        [Fact]
        public void Add_NextPressedQuestionAdded_ReturnedNewEmptyQuestionObjectWithQuestionNrEqualtTo2()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object
            NewQuestionVM question = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 1,
                QuestionString = "Test"
            };

            //Act
            NewQuestionVM newQuestionVM = new NewQuestionVM();
            var model = (NewQuestionVM)(createQuestionsController.ButtonNext(question, newQuestionVM) as ViewResult)?.ViewData.Model;

            //Assert
            Assert.Equal(model.QuestionNr, 2);
        }

        //if someone is trying to manipulate the input 
        [Fact]
        public void Add_NextPressedWhenQuestionNrEqualTo3AndOnlyQuestion1IsStored_RedirectedToShowIndexOnMainPage() 
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            NewQuestionVM question = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 3,
                QuestionString = "Test"
            };
            list.Add(question);
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list);
            NewQuestionVM questionSendIn = new NewQuestionVM()
            {
                QuestionNr = 3,
                Answer1 = "B1",
                Answer2 = "B2",
                Answer3 = "B3",
                Answer4 = "B4",
                RadioButtonAlternativeAnswer = 2,
                QuestionString = "Test2"
            };

            //Act  
            NewQuestionVM newQuestionVM = new NewQuestionVM();
            RedirectToActionResult result = (RedirectToActionResult)createQuestionsController.ButtonNext(questionSendIn, newQuestionVM);
            var sessionResult = createQuestionsController.MyContextAccessor.HttpContext.Session.GetJson<List<NewQuestionVM>>("CreateNewQuestionsList");

            //Assert
            Assert.Equal(null, sessionResult);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Main", result.ControllerName);
        }


        //---------------------------BACK-------------------------------------------------



        //if someone is trying to manipulate the input 
        [Fact] 
        public void Add_BackPressedWhenOnPage1AndQuestion1IsAreadyStoredInSession_ReturnViewModelWithQuestionNr1()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            NewQuestionVM question = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 1,
                QuestionString = "Test"
            };
            list.Add(question);
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list);
            NewQuestionVM questionSendIn = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "B1",
                Answer2 = "B2",
                Answer3 = "B3",
                Answer4 = "B4",
                RadioButtonAlternativeAnswer = 2,
                QuestionString = "Test2"
            };

            //Act
            NewQuestionVM newQuestionVM = new NewQuestionVM();
            var model = (NewQuestionVM)(createQuestionsController.ButtonBack(questionSendIn, newQuestionVM) as ViewResult)?.ViewData.Model;

            //Assert
            Assert.Equal(model.QuestionNr, 1);
        }

        [Fact]
        public void Add_BackPressedWhenOnPage2WithValidInputAndOnlyQuestion1IsStoredInSession_ReturnViewModelWithQuestionNr1()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            NewQuestionVM question = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 1,
                QuestionString = "Test"
            };
            list.Add(question);
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list);
            NewQuestionVM questionSendIn = new NewQuestionVM()
            {
                QuestionNr = 2,
                Answer1 = "B1",
                Answer2 = "B2",
                Answer3 = "B3",
                Answer4 = "B4",
                RadioButtonAlternativeAnswer = 2,
                QuestionString = "Test2"
            };

            //Act
            NewQuestionVM newQuestionVM = new NewQuestionVM();
            var model = (NewQuestionVM)(createQuestionsController.ButtonBack(questionSendIn, newQuestionVM) as ViewResult)?.ViewData.Model;

            //Assert
            Assert.Equal(model.QuestionNr, 1);
        }


        [Fact]
        public void Add_BackPressedWhenOnPage2WithInvalidInputAndOnlyQuestion1IsStoredInSession_ReturnViewModelWithQuestionNr2AndModelStateInvalid()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            NewQuestionVM question = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 1,
                QuestionString = "Test"
            };
            list.Add(question);
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list);
            NewQuestionVM questionSendIn = new NewQuestionVM()
            {
                QuestionNr = 2,
                Answer1 = "B1",
                Answer2 = "B2",
                Answer3 = "B3",
                Answer4 = "B4",
                RadioButtonAlternativeAnswer = 2,
                QuestionString = ""
            };

            //Act
            NewQuestionVM newQuestionVM = new NewQuestionVM();
            var model = (NewQuestionVM)(createQuestionsController.ButtonBack(questionSendIn, newQuestionVM) as ViewResult)?.ViewData.Model;

            //Assert
            Assert.Equal(model.QuestionNr, 2);
            Assert.False(createQuestionsController.ModelState.IsValid);
        }
                
        [Fact]
        public void Add_BackPressedWhenQuestionNrEqualtTo3AndOnlyQuestion1IsStored_RedirectToIndexOnMainPageAndDeleteCreateNewQuestionsList()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            NewQuestionVM question = new NewQuestionVM()
            {
                QuestionNr = 1,
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 3,
                QuestionString = "Test"
            };
            list.Add(question);
            createQuestionsController.MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list);
            NewQuestionVM questionSendIn = new NewQuestionVM()
            {
                QuestionNr = 3,
                Answer1 = "B1",
                Answer2 = "B2",
                Answer3 = "B3",
                Answer4 = "B4",
                RadioButtonAlternativeAnswer = 2,
                QuestionString = "Test2"
            };

            //Act        
            NewQuestionVM newQuestionVM = new NewQuestionVM();
            RedirectToActionResult result = (RedirectToActionResult)createQuestionsController.ButtonBack(questionSendIn, newQuestionVM);

            var sessionResult = createQuestionsController.MyContextAccessor.HttpContext.Session.GetJson<List<NewQuestionVM>>("CreateNewQuestionsList");

            //Assert
            Assert.Equal(null, sessionResult);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Main", result.ControllerName);
        }





        ////-------------------ValidateInput---------------------------------------------------

            

        [Fact]
        public void ValidateInput_InputMissingCheckboxChecked_ModelstateIsNotValid()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            NewQuestionVM question = new NewQuestionVM()
            {
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 0,
                QuestionString = ""
            };

            //Act
            createQuestionsController.ValidateInput(question);

            //Assert
            Assert.False(createQuestionsController.ModelState.IsValid);
        }

        [Fact]
        public void ValidateInput_InputMissingValueForCheckboxCheckedAnswer_ModelstateIsNotValid()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            NewQuestionVM question = new NewQuestionVM()
            {
                Answer1 = "A1",
                Answer2 = "",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 2,
                QuestionString = ""
            };

            //Act
            createQuestionsController.ValidateInput(question);

            //Assert
            Assert.False(createQuestionsController.ModelState.IsValid);
        }

        [Fact]
        public void ValidateInput_InputMissingValueForQuestionString_ModelstateIsNotValid()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            NewQuestionVM question = new NewQuestionVM()
            {
                Answer1 = "A1",
                Answer2 = "A2",
                Answer3 = "A3",
                Answer4 = "A4",
                RadioButtonAlternativeAnswer = 2,
                QuestionString = ""
            };

            //Act
            createQuestionsController.ValidateInput(question);

            //Assert
            Assert.False(createQuestionsController.ModelState.IsValid);
        }


        [Fact]
        public void ValidateInput_InputMissingValueForAllAnswersRadioButtonNotChecked_ModelstateIsNotValid()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            NewQuestionVM question = new NewQuestionVM()
            {
                Answer1 = "",
                Answer2 = "",
                Answer3 = "",
                Answer4 = "",
                RadioButtonAlternativeAnswer = 0,
                QuestionString = "Test"
            };

            //Act
            createQuestionsController.ValidateInput(question);

            //Assert
            Assert.False(createQuestionsController.ModelState.IsValid);
        }

        [Fact]
        public void ValidateInput_InputMissingValueForAllAnswersRadioButtonChecked_ModelstateIsNotValid()
        {
            //Arrange
            CreateQuestionsController createQuestionsController = setupBeforeTests();
            NewQuestionVM question = new NewQuestionVM()
            {
                Answer1 = "",
                Answer2 = "",
                Answer3 = "",
                Answer4 = "",
                RadioButtonAlternativeAnswer = 1,
                QuestionString = "Test"
            };

            //Act
            createQuestionsController.ValidateInput(question);

            //Assert
            Assert.False(createQuestionsController.ModelState.IsValid);
        }
    }
}
