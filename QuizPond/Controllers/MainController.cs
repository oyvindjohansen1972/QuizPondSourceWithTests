using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizPond.Data.Interfaces;
using QuizPond.ViewModels;

namespace QuizPond.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class MainController : Controller
    {

        private IQuestionRepository repository;
        public IHttpContextAccessor MyContextAccessor;

        public MainController(IQuestionRepository repo, IHttpContextAccessor _contextAccessor)
        {
            repository = repo;
            MyContextAccessor = _contextAccessor;
        }


        [HttpGet]
        public ViewResult Index() => View();


        [HttpPost]
        public IActionResult Index(PlayerInfoVM gameCodeVM)
        {
            string quizName = repository.getQuizName(gameCodeVM?.GameCode?.ToUpper());
            if(quizName == null)
            {
                ModelState.Clear();
                ModelState.AddModelError("", "This code does not exist!");
                return View(gameCodeVM);
            }
            else              
            {
                gameCodeVM.QuizName = quizName;
                TempData["GameCode"] = gameCodeVM.GameCode;
                TempData["QuizName"] = quizName;
                return RedirectToAction("CreateNickname");
            }           
        }
         

        [HttpGet]
        public ViewResult CreateNickname()
        {
            PlayerInfoVM gameCodeVM = new PlayerInfoVM { GameCode = TempData["GameCode"] as string, QuizName = TempData["QuizName"] as string };
            return View(gameCodeVM);
        }

        [HttpPost]
        public IActionResult CreateNickname(PlayerInfoVM gameCodeVM)
        {
            if(!ModelState.IsValid)
            {
                return View(gameCodeVM);
            }
            bool result = repository.TryToSaveNewPlayer(gameCodeVM);
            if(result)
            {
                return RedirectToAction("Instructions", gameCodeVM);
            }
            else
            {
                ModelState.Clear();
                ModelState.AddModelError("", "This nickname is used! Please pick another nickname!");               
            }
            return View(gameCodeVM);
        }


        [HttpGet]
        public ViewResult Instructions(PlayerInfoVM gameCodeVM)
        {            
            return View(gameCodeVM);
        }
                      

        [HttpGet]
        public ViewResult ShowNewQuizStored() => View();
    }
}
