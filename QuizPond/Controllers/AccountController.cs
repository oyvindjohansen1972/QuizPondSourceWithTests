using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizPond.Data.Interfaces;
using QuizPond.Data.Models;
using QuizPond.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPond.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IQuestionRepository repository;          

        public AccountController(   UserManager<ApplicationUser> userManager, 
                                    SignInManager<ApplicationUser> signInManager,
                                    IQuestionRepository repo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            repository = repo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.KeepMeLoggedIn, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("MyPage");
                }
                ModelState.AddModelError("", "Login failed!");
            }
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Main");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountVM vm)
        {
            if (ModelState.IsValid)
            {           
                var user = new ApplicationUser { UserName = vm.Email, Email = vm.Email };
                var result = await _userManager.CreateAsync(user, vm.Password);

                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.TryAddModelError("", error.Description);
                    }                
                }
            }
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public IActionResult MyPage()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            
            MyPageVM myPageVM = new MyPageVM();
            var allMyQuizzes = repository.Quizzes(userId);
            var allMyQuizzesVM = new List<QuizToStartVM>();

            repository.CheckIfNewGameIsMoreThan24HoursOldAndDeleteIfTrue(allMyQuizzes);

            allMyQuizzes = repository.Quizzes(userId);

            foreach (var item in allMyQuizzes)
            {
                var r = item.NewGames.Count;
                allMyQuizzesVM.Add(new QuizToStartVM { QuizId = item.QuizId, QuizName=item.QuizName, HasStartedNewGames = item.NewGames.Count > 0 ? true : false });
            }
            myPageVM.MyQuizzes = allMyQuizzesVM;
            return View(myPageVM);
        }

        [Authorize]
        [HttpGet]
        public IActionResult VerifyDeleteQuiz(int QuizId, string NameOfQuiz)
        {
            return View(new VerifyDeleteQuizVM { NameOfQuiz = NameOfQuiz, QuizId = QuizId }    );
        }

        [Authorize]
        [HttpPost]
        public IActionResult VerifyDeleteQuiz(int QuizId, string NameOfQuiz, string YesNo)
        {
            if(YesNo == "Yes")
            {
                repository.TryDeleteQuiz(QuizId, NameOfQuiz);               
            }
            return RedirectToAction(nameof(MyPage));
        }

        [Authorize]
        [HttpGet]
        public IActionResult ShowAnswersInSummary(int id, string name)
        {
            ShowCodeForNewGameVM showCodeForNewGameVM = new ViewModels.ShowCodeForNewGameVM() { QuizId = id, NameOfQuiz = name, ShowAnswers = false };
            return View(showCodeForNewGameVM);
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult ShowCodeForNewGame(ShowCodeForNewGameVM showCodeForNewGameVM)
        {           
            var guid = Guid.NewGuid().ToString().ToUpper();
            guid = guid.Substring(0, 4);
            showCodeForNewGameVM.GameCode = guid;
            repository.SaveNewGame(new NewGame
            {
                GameCodeId = showCodeForNewGameVM.GameCode,
                QuizId = showCodeForNewGameVM.QuizId,
                ShowAnswersToPlayer = showCodeForNewGameVM.ShowAnswers,
                TimeStamp = DateTime.Now
            });
            TempData["NameOfQuiz"] = showCodeForNewGameVM.NameOfQuiz;
            TempData["GameCode"] = showCodeForNewGameVM.GameCode;
            return RedirectToAction(nameof(ShowGameCode));         
        }
        
        [HttpGet]
        public IActionResult ShowGameCode()
        {
            ShowCodeForNewGameVM showCodeForNewGameVM = new ShowCodeForNewGameVM
            {
                NameOfQuiz = TempData["NameOfQuiz"] as string,
                GameCode = TempData["GameCode"] as string
            };
            return View(nameof(ShowCodeForNewGame), showCodeForNewGameVM);
        }
                
        [Authorize]
        [HttpGet]
        public IActionResult ShowPlayers(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var GameCodeIds = repository.getGameCodeIdsForUser(userId, id);            
            var quizName = repository.getQuizName(GameCodeIds.ElementAt(0));

            List<GameCodeAndAllPlayersListVM> AllPlayers = new List<GameCodeAndAllPlayersListVM>();

            foreach (var gameCodeId in GameCodeIds)
            {                
                var players = repository.GetPlayers(gameCodeId).ToList();
                List<PlayerListVM> playersVM = new List<PlayerListVM>();
                foreach (var player in players)
                {
                    playersVM.Add(new PlayerListVM { Name = player.Name, GameScore = player.Score});
                }
                var sortedResult = playersVM.OrderByDescending(g => g.GameScore).ToList();

                AllPlayers.Add(new GameCodeAndAllPlayersListVM { GameCode = gameCodeId, Players = sortedResult });
            }
            return View( new ShowPlayersVM { QuizName = quizName, PlayersAndGameCode = AllPlayers });
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult PreviewAllQuestionsWithAnswers(string gameCode, string quizName)
        {
            var questionsWithAlternativeAnswers = repository.Questions(gameCode);
            var correctAnswers = repository.GetCorrectAnswersForQuestions(gameCode);

            List<QuestionsWithAnswersVM> questionsWithAlternateiveAnswersList = new List<QuestionsWithAnswersVM>();
           
            foreach(Question q in questionsWithAlternativeAnswers)
            {         
                questionsWithAlternateiveAnswersList.Add(new QuestionsWithAnswersVM { QuestionNr = q.QuestionNr, QuestionString = q.QuestionString, CorrectAnswer = q.Answers.SingleOrDefault(a => a.AnswerNr == q.RadioButtonAlternativeAnswer).AnswerText });
            }
            PreviewAllQuestionsWithAnswersVM previewAllQuestionsWithAnswersVM = new PreviewAllQuestionsWithAnswersVM { QuizName = quizName, questionsWithAnswers = questionsWithAlternateiveAnswersList };            
            return View(previewAllQuestionsWithAnswersVM);
        }
    }
}

