using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizPond.Data.Repositories;
using QuizPond.Data.Interfaces;
using QuizPond.Data.Models;
using QuizPond.ViewModels;
using QuizPond.Infrastructure;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace QuizPond.Controllers
{
    public class GameController : Controller
    {         
        private IQuestionRepository repository;
        public IHttpContextAccessor MyContextAccessor;

        public GameController(IQuestionRepository repo, IHttpContextAccessor _contextAccessor)
        {
            repository = repo;
            MyContextAccessor = _contextAccessor;
        }    

        [HttpPost]
        public IActionResult InitGame(PlayerInfoVM gameCodeVM)
        {
            MyContextAccessor.HttpContext.Session.SetJson("PlayerInfo", gameCodeVM);
            int page = 1;
            var questionsWithAlternativeAnswers = repository.Questions(gameCodeVM.GameCode);
            int numberOfQuestions = questionsWithAlternativeAnswers.Count();
            List<ShowQuestionsVM> ListVM = new List<ShowQuestionsVM>();

            foreach (Question q in questionsWithAlternativeAnswers)
            {

                var result = q.Answers.OrderBy(a => a.AnswerNr).ToList<Answer>();

                ListVM.Add(new ShowQuestionsVM()
                {
                    Page = page++,
                    QuestionId = q.QuestionNr,
                    TotalPages = numberOfQuestions,
                    RadioButtonChosenAlternativeAnswer = null,
                    QuestionString = q.QuestionString,
                    ListOfAnswers = result
                });
            }
            
            MyContextAccessor.HttpContext.Session.SetJson("ListWithAllQuestionsAndAnswers", ListVM); //storing all the questions for this quiz in the session object  
            List<int?> ChosenAlternativeAnswer = new List<int?>(new int?[ListVM.Count]);         
            MyContextAccessor.HttpContext.Session.SetJson("ListWithChosenAlternativeAnswersForAllQuestions", ChosenAlternativeAnswer);
            if(ListVM.Count == 0)
            {
                removeInfoFromSessionObject();
                return RedirectToAction("Index", "Main");
            }            
            return View("Run", ListVM.ElementAt(0));
        }

        [HttpPost]
        public IActionResult Run(int page, int? radioButton, bool showMessagebox, string buttonBack, string buttonNext, string buttonFinished)
        {            
            ValidateInput(page, radioButton, buttonBack, buttonNext, buttonFinished);
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                var AllQuestions = MyContextAccessor.HttpContext.Session.GetJson<List<ShowQuestionsVM>>("ListWithAllQuestionsAndAnswers");
                if (radioButton != null)
                {
                    var ChosenAlternativeAnswer = MyContextAccessor.HttpContext.Session.GetJson<List<int?>>("ListWithChosenAlternativeAnswersForAllQuestions");
                    ChosenAlternativeAnswer[page-1] = radioButton;
                    MyContextAccessor.HttpContext.Session.SetJson("ListWithChosenAlternativeAnswersForAllQuestions", ChosenAlternativeAnswer);
                    AllQuestions.ElementAt(page-1).RadioButtonChosenAlternativeAnswer = radioButton;
                    MyContextAccessor.HttpContext.Session.SetJson("ListWithAllQuestionsAndAnswers", AllQuestions);
                }              

                if (!string.IsNullOrEmpty(buttonBack))
                {
                    if(page > 1)
                    {
                        page--;
                        return View(AllQuestions.Single(x=>x.Page == page));
                    }                    
                }
                else if (!string.IsNullOrEmpty(buttonNext))
                {                    
                    if(page < AllQuestions.Count)
                    {
                        page++;
                        return View(AllQuestions.Single(x => x.Page == page));
                    }                    
                }
                else if (!string.IsNullOrEmpty(buttonFinished))
                {
                    var AnswerChoosenByPlayer = MyContextAccessor.HttpContext.Session.GetJson<List<int?>>("ListWithChosenAlternativeAnswersForAllQuestions");

                    List<int> validationResult = (ValidateIfAllQuestionsAreAnswered(AnswerChoosenByPlayer)).ToList();
                    if(validationResult.Count != 0)
                    {                      
                        string unansweredQuestions = string.Join(",", validationResult.ToArray());
                        ModelState.AddModelError("", $"You need to answer all of the questions. Question nr {unansweredQuestions} has no answer!");
                    }
                    else //all questions have answers
                    {
                        var score = CalculatePlayersScore();
                        var PlayerId = MyContextAccessor.HttpContext.Session.GetJson<PlayerInfoVM>("PlayerInfo");
                        PlayerId.Score = score;
                        var NonNullableListOfPlayersAnswers = convertListToNonNullable(AnswerChoosenByPlayer);
                        if (repository.TryToUpdatePlayer(PlayerId, NonNullableListOfPlayersAnswers.ToList<int>()))
                        {
                            //Go to new view
                            return RedirectToAction("ShowQuestionsWithAnswersFromQuiz");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Main");
                        }
                    }                 
                }
                return View(AllQuestions.ElementAt(page-1));
            }
            else // input data was tampered with
            {
                removeInfoFromSessionObject();
                return RedirectToAction("Index", "Main");
            }
        }        

        [HttpGet]
        public IActionResult ShowQuestionsWithAnswersFromQuiz()
        {
            var AllQuestions = MyContextAccessor.HttpContext.Session.GetJson<List<ShowQuestionsVM>>("ListWithAllQuestionsAndAnswers");
            var AnswerChoosenByPlayer = MyContextAccessor.HttpContext.Session.GetJson<List<int?>>("ListWithChosenAlternativeAnswersForAllQuestions");
            var NonNullableListOfPlayersAnswers = convertListToNonNullable(AnswerChoosenByPlayer);

            ShowQuestionsWithAnswersFromQuizVM showQuestionsWithAnswersFromQuizVM = new ShowQuestionsWithAnswersFromQuizVM();
            var PlayerId = MyContextAccessor.HttpContext.Session.GetJson<PlayerInfoVM>("PlayerInfo");
            var CorrectAnswers = repository.GetCorrectAnswersForQuestions(PlayerId.GameCode);
            showQuestionsWithAnswersFromQuizVM.ShowAnswersToPlayer = repository.ShowAnswersToPlayer(PlayerId.GameCode);
            showQuestionsWithAnswersFromQuizVM.QuizName = PlayerId.QuizName;

            int counter = 0;
            foreach (var question in AllQuestions)
            {
                QuestionsWithAnswersVM questionsWithAnswersVM = new QuestionsWithAnswersVM();
                questionsWithAnswersVM.QuestionNr = question.Page;
                questionsWithAnswersVM.QuestionString = question.QuestionString;
                questionsWithAnswersVM.AnswerGivenByPlayer = question.ListOfAnswers.SingleOrDefault(q => q.AnswerNr == question.RadioButtonChosenAlternativeAnswer).AnswerText;
                questionsWithAnswersVM.CorrectAnswer = question.ListOfAnswers.ElementAt(CorrectAnswers.ElementAt(counter)).AnswerText;
                counter++;
                showQuestionsWithAnswersFromQuizVM.questionsWithAnswers.Add(questionsWithAnswersVM);            
            }                    
            return View(showQuestionsWithAnswersFromQuizVM);
        }

        private void removeInfoFromSessionObject()
        {
            //deleting all the info for this game stored in the session object   
            MyContextAccessor.HttpContext.Session.SetJson("ListWithAllQuestionsAndAnswers", null);
            MyContextAccessor.HttpContext.Session.SetJson("ListWithChosenAlternativeAnswersForAllQuestions", null);
        }

        public int CalculatePlayersScore()
        {
            var PlayerId = MyContextAccessor.HttpContext.Session.GetJson<PlayerInfoVM>("PlayerInfo");
            var CorrectAnswers = repository.GetCorrectAnswersForQuestions(PlayerId.GameCode);
            var AnswerChoosenByPlayer = MyContextAccessor.HttpContext.Session.GetJson<List<int?>>("ListWithChosenAlternativeAnswersForAllQuestions");
            int score = 0;
            int counter = 0;
            foreach (var answer in CorrectAnswers)
            {
                if(answer == AnswerChoosenByPlayer.ElementAt(counter++) )
                {
                    score++;
                }
            }
            return score;
        }

        public IEnumerable<int> ValidateIfAllQuestionsAreAnswered(List<int?> listOfAnswers)
        {            
            for (int i = 0; i < listOfAnswers.Count; i++)
            {
                if (listOfAnswers.ElementAt(i) == null)
                {
                    yield return i+1;
                }
            }            
        }

        public IEnumerable<int> convertListToNonNullable(List<int?> listOfAnswers)
        {
            foreach(var item in listOfAnswers)
            {
                yield return (int)item;                
            }
        }
        
        public void ValidateInput(int? page, int? radiobutton, string buttonBack, string buttonNext, string buttonFinished)
        {
            if (string.IsNullOrEmpty(buttonBack + buttonNext + buttonFinished))
            {
                ModelState.AddModelError("", "");
            }
            else
            {
                var AllQuestions = MyContextAccessor.HttpContext.Session.GetJson<List<ShowQuestionsVM>>("ListWithAllQuestionsAndAnswers");
                int numberOfQuestions = AllQuestions.Count;
                if (page == null || page < 1 || page > numberOfQuestions)
                {
                    ModelState.AddModelError("", "");
                }
                else
                {
                    var numberOfPossibleAnswersForCurrentQuestion = AllQuestions.SingleOrDefault(x => x.Page == page).ListOfAnswers.Count;
                    if (radiobutton < 0 || radiobutton > numberOfPossibleAnswersForCurrentQuestion)
                    {
                        ModelState.AddModelError("", "");
                    }
                }
            }
        }
    }
}


