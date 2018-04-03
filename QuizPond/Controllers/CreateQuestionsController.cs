using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizPond.ViewModels;
using QuizPond.Infrastructure;
using QuizPond.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuizPond.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace QuizPond.Controllers
{


    [Authorize]
    public class CreateQuestionsController : Controller
    {
        private IUserManagerWrapper userManagerRepository;
        private IQuestionRepository repository;
        public IHttpContextAccessor MyContextAccessor;

        public CreateQuestionsController(IUserManagerWrapper _userManagerRepository, IQuestionRepository repo, IHttpContextAccessor _contextAccessor)
        {
            userManagerRepository = _userManagerRepository;
            repository = repo;
            MyContextAccessor = _contextAccessor;
        }

        [HttpGet]
        public IActionResult AddNewQuiz()
        {
            return View(new AddNewQuizVM());
        }

        [HttpPost]
        public IActionResult AddNewQuiz(AddNewQuizVM addNewQuizVM)
        {
            var UserId = userManagerRepository.GetUserIdFromIdentity(HttpContext?.User);
            if (UserId == null)
            {
                deleteQuestionsAndAnswersStoredInSession();
                return RedirectToAction("Index", "Main");
            }
            addNewQuizVM.UserId = UserId;
            int QuizId = repository.SaveNewQuiz(addNewQuizVM);
            addNewQuizVM.QuizId = QuizId;
            MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuizId", addNewQuizVM);
            List<NewQuestionVM> list = new List<NewQuestionVM>();
            MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", list); //storing a list of empty NewQuestionVM objects in teh session object
            NewQuestionVM question = new NewQuestionVM { QuestionNr = 1 };
            return View(nameof(Add), question);
        }

        [HttpPost]
        public IActionResult Add(NewQuestionVM question, string buttonBack, string buttonNext, string buttonFinished)
        {
            NewQuestionVM newQuestion = new NewQuestionVM();
            IActionResult result = View(newQuestion);

            if (!string.IsNullOrEmpty(buttonNext))
            {
                result = ButtonNext(question, newQuestion);                
            }
            else if (!string.IsNullOrEmpty(buttonBack))
            {
                result = ButtonBack(question, newQuestion);
            }
            else if (!string.IsNullOrEmpty(buttonFinished))
            {
                result = ButtonFinished(question, newQuestion);
            }
            return result;
        }

        public IActionResult ButtonNext(NewQuestionVM question, NewQuestionVM newQuestion)
        {
            ValidateInput(question);
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                var sessionResult = MyContextAccessor.HttpContext.Session.GetJson<List<NewQuestionVM>>("CreateNewQuestionsList");
                if (sessionResult.Count == 0) //executes when we input the first question
                {
                    question.QuestionNr = 1;
                }
                else if (question.QuestionNr > 100 || question.QuestionNr < 1)
                {
                    deleteQuestionsAndAnswersStoredInSession();
                    return RedirectToAction("Index", "Main"); //////////////////////////////
                }
                bool QuestionExists = sessionResult.Any(x => x.QuestionNr == question.QuestionNr);
                if (!QuestionExists)
                {
                    if ((sessionResult.Count + 1) == question.QuestionNr)
                    {
                        sessionResult.Add(question);
                        MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", sessionResult); //add a new question to the session object                        
                        newQuestion.QuestionNr = (question.QuestionNr) + 1;
                        return View(nameof(Add), newQuestion); //////////////////////////////
                    }
                    else //if questionNr is messed with by user
                    {
                        deleteQuestionsAndAnswersStoredInSession();
                        return RedirectToAction("Index", "Main"); //////////////////////////////
                    }
                }
                else if (QuestionExists)
                {
                    var oldQuestion = sessionResult.Single(x => x.QuestionNr == question.QuestionNr);
                    sessionResult[sessionResult.IndexOf(oldQuestion)] = question;
                    MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", sessionResult); //add a new question to the session object                                                     
                    if (question.QuestionNr < sessionResult.Count())
                    {
                        return View(sessionResult.ElementAt(question.QuestionNr)); //////////////////////////////
                    }
                    else
                    {
                        newQuestion.QuestionNr = ++(question.QuestionNr);
                        return View(newQuestion); //////////////////////////////
                    }
                }
            }
            return View(question);
        }


        public IActionResult ButtonBack(NewQuestionVM question, NewQuestionVM newQuestion)
        {
            ValidateInput(question);
            if (ModelState.IsValid) //no validation error
            {
                ModelState.Clear();
                var sessionResult = MyContextAccessor.HttpContext.Session.GetJson<List<NewQuestionVM>>("CreateNewQuestionsList");
                if (sessionResult.Count == 0) //executes when we input the first question
                {
                    question.QuestionNr = 1;
                }
                else if (question.QuestionNr > 100 || question.QuestionNr < 1)
                {
                    deleteQuestionsAndAnswersStoredInSession();
                    return RedirectToAction("Index", "Main"); //////////////////////////////
                }
                bool QuestionExists = sessionResult.Any(x => x.QuestionNr == question.QuestionNr);
                if (!QuestionExists)
                {
                    if ((sessionResult.Count + 1) == question.QuestionNr)
                    {
                        sessionResult.Add(question);
                        MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", sessionResult); //add a new question to the session object                        
                        newQuestion = sessionResult[(question.QuestionNr) - 2];
                        return View(newQuestion); //////////////////////////////
                    }
                    else //if questionNr is messed with by user
                    {
                        deleteQuestionsAndAnswersStoredInSession();
                        return RedirectToAction("Index", "Main"); //////////////////////////////
                    }
                }
                else if (QuestionExists)
                {
                    var oldQuestion = sessionResult.Single(x => x.QuestionNr == question.QuestionNr);
                    sessionResult[sessionResult.IndexOf(oldQuestion)] = question;
                    MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", sessionResult); //add a new question to the session object   
                    if(question.QuestionNr == 1)
                    {
                        return View(sessionResult.ElementAt(question.QuestionNr - 1)); //////////////////////////////
                    }
                    else
                    {
                        return View(sessionResult.ElementAt(question.QuestionNr - 2)); //////////////////////////////
                    }
                    
                }
            }
            else
            {
                if (string.IsNullOrEmpty(question.QuestionString) && string.IsNullOrEmpty(question.Answer1)) //we have not input any question save questions for quiz
                {
                    if (question.QuestionNr > 1 && question.QuestionNr < 100)
                    {
                        var sessionResult = MyContextAccessor.HttpContext.Session.GetJson<List<NewQuestionVM>>("CreateNewQuestionsList");
                        ModelState.Clear();
                        return View(sessionResult.ElementAt(question.QuestionNr - 2)); //////////////////////////////
                    }
                }
                else
                {
                    return View(question); //////////////////////////////
                }
            }
            return View(question); //////////////////////////////
        }


        public IActionResult ButtonFinished(NewQuestionVM question, NewQuestionVM newQuestion)
        {
            ValidateInput(question);
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                var sessionResult = MyContextAccessor.HttpContext.Session.GetJson<List<NewQuestionVM>>("CreateNewQuestionsList");
                if (sessionResult.Count == 0) //executes when we input the first question
                {
                    question.QuestionNr = 1;
                }
                else if (question.QuestionNr > 100 || question.QuestionNr < 1)
                {
                    deleteQuestionsAndAnswersStoredInSession();
                    return RedirectToAction("Index", "Main"); //////////////////////////////
                }
                bool QuestionExists = sessionResult.Any(x => x.QuestionNr == question.QuestionNr);
                if (!QuestionExists) //--
                {
                    if ((sessionResult.Count + 1) == question.QuestionNr)
                    {
                        sessionResult.Add(question);
                        var sessionResultAddNewQuizVM = MyContextAccessor.HttpContext.Session.GetJson<AddNewQuizVM>("CreateNewQuizId");
                        var AddNewQuizVMFromDb = repository.GetNewQuiz(sessionResultAddNewQuizVM.QuizId);
                        if (sessionResultAddNewQuizVM.QuizName == AddNewQuizVMFromDb.QuizName &&
                            sessionResultAddNewQuizVM.UserId == AddNewQuizVMFromDb.UserId)
                        {
                            repository.SaveQuestions(sessionResult, sessionResultAddNewQuizVM.QuizId);
                            return RedirectToAction("ShowNewQuizStored", "Main"); //////////////////////////////
                        }
                        else
                        {
                            deleteQuestionsAndAnswersStoredInSession();
                            return RedirectToAction("Index", "Main"); //////////////////////////////
                        }
                    }
                    else //if questionNr is messed with by user
                    {
                        deleteQuestionsAndAnswersStoredInSession();
                        return RedirectToAction("Index", "Main"); //////////////////////////////
                    }
                }
                else if (QuestionExists)
                {
                    var oldQuestion = sessionResult.Single(x => x.QuestionNr == question.QuestionNr);
                    sessionResult[sessionResult.IndexOf(oldQuestion)] = question;
                    MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", sessionResult); //add a new question to the session object       

                    var sessionResultAddNewQuizVM = MyContextAccessor.HttpContext.Session.GetJson<AddNewQuizVM>("CreateNewQuizId");

                    var AddNewQuizVMFromDb = repository.GetNewQuiz(sessionResultAddNewQuizVM.QuizId);

                    if (sessionResultAddNewQuizVM.QuizName == AddNewQuizVMFromDb.QuizName &&
                        sessionResultAddNewQuizVM.UserId == AddNewQuizVMFromDb.UserId)
                    {
                        repository.SaveQuestions(sessionResult, sessionResultAddNewQuizVM.QuizId);
                        return RedirectToAction("ShowNewQuizStored", "Main"); //////////////////////////////
                    }
                    else
                    {
                        deleteQuestionsAndAnswersStoredInSession();
                        return RedirectToAction("Index", "Main"); //////////////////////////////
                    }
                }
            }
            else
            {
                //if (ValidateIfFormIsEmpty(question))
                //{
                //    return View(question);
                //}
                if (ValidateIfFormIsEmpty(question)) //we have not input any question save questions for quiz
                {
                    var sessionResult = MyContextAccessor.HttpContext.Session.GetJson<List<NewQuestionVM>>("CreateNewQuestionsList");
                    var sessionResultAddNewQuizVM = MyContextAccessor.HttpContext.Session.GetJson<AddNewQuizVM>("CreateNewQuizId");
                    if(sessionResultAddNewQuizVM != null)
                    {
                        var AddNewQuizVMFromDb = repository.GetNewQuiz(sessionResultAddNewQuizVM.QuizId);
                        if (sessionResultAddNewQuizVM.QuizName == AddNewQuizVMFromDb.QuizName &&
                            sessionResultAddNewQuizVM.UserId == AddNewQuizVMFromDb.UserId)
                        {
                            repository.SaveQuestions(sessionResult, sessionResultAddNewQuizVM.QuizId);
                            return RedirectToAction("ShowNewQuizStored", "Main"); //////////////////////////////
                        }
                        else
                        {
                            deleteQuestionsAndAnswersStoredInSession();
                            return RedirectToAction("Index", "Main"); //////////////////////////////
                        }
                    }
                    else
                    {
                        deleteQuestionsAndAnswersStoredInSession();
                        return RedirectToAction("Index", "Main"); //////////////////////////////
                    }
                }
                else
                {
                    return View("Add", question); //////////////////////////////
                }                
            }
            return View(nameof(Add), question); //////////////////////////////
        }
    



        public bool ValidateIfFormIsEmpty(NewQuestionVM newQuestionVM)
        {
            var result = String.IsNullOrEmpty(newQuestionVM.QuestionString + 
                                                     newQuestionVM.Answer1 +
                                                     newQuestionVM.Answer2 +
                                                     newQuestionVM.Answer3 +
                                                     newQuestionVM.Answer4);
            return result;
        }

        public void ValidateInput(NewQuestionVM question)
        {
            if (string.IsNullOrEmpty(question.QuestionString) || string.IsNullOrWhiteSpace(question.QuestionString) )
            {
                ModelState.AddModelError(nameof(question.QuestionString), "Please check the correct answer!");
            }
            if (question.RadioButtonAlternativeAnswer == null || question.RadioButtonAlternativeAnswer < 0 || question.RadioButtonAlternativeAnswer > 4)
            {
                ModelState.AddModelError(nameof(question.RadioButtonAlternativeAnswer), "Please check the correct answer!");
            }
            else
            {
                switch (question.RadioButtonAlternativeAnswer)
                {
                    case 0:
                        {
                            if (string.IsNullOrEmpty(question.Answer1))
                            {
                                ModelState.AddModelError("", "You need to check a valid question!");
                            }
                            break;
                        }
                    case 1:
                        {
                            if (string.IsNullOrEmpty(question.Answer2)) ModelState.AddModelError("", "You need to check a valid question!");
                            break;
                        }
                    case 2:
                        {
                            if (string.IsNullOrEmpty(question.Answer3)) ModelState.AddModelError("", "You need to check a valid question!");
                            break;
                        }
                    case 3:
                        {
                            if (string.IsNullOrEmpty(question.Answer4)) ModelState.AddModelError("", "You need to check a valid question!");
                            break;
                        }
                }
            }
        }  
            
        private void deleteQuestionsAndAnswersStoredInSession()
        {           
            MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuestionsList", null); 
            MyContextAccessor.HttpContext.Session.SetJson("CreateNewQuizId", null);
        }
    }
}
