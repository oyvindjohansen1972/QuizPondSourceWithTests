using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizPond.Data.Interfaces;
using QuizPond.Data.Models;
using QuizPond.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace QuizPond.Data.Repositories
{
    public class EFQuestionRepository : IQuestionRepository
    {
        private ApplicationDbContext context;

        public void  CheckIfNewGameIsMoreThan24HoursOldAndDeleteIfTrue(ICollection<Quiz> ListOfAllQuizzes)
        {
            foreach(var items in ListOfAllQuizzes)
            {
                foreach(var games in items.NewGames)
                {
                    var result = context.NewGames.FirstOrDefault(g => g.GameCodeId == games.GameCodeId);
                    if (result != null && result.TimeStamp <= DateTime.Now.AddDays(-1))
                    {
                        context.Remove(result);
                    }
                }              
            }
            context.SaveChanges();
        }

        public EFQuestionRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public bool ShowAnswersToPlayer(string gameCode)
        {
            var result = context.NewGames.Where(n => n.GameCodeId == gameCode).FirstOrDefault().ShowAnswersToPlayer;                         
            return result; 
        }
        
        public int SaveNewQuiz(AddNewQuizVM addNewQuizVM)
        {
            Quiz quiz = new Quiz { QuizName = addNewQuizVM.QuizName, UserId = addNewQuizVM.UserId };
            context.Add(quiz);
            context.SaveChanges();
            return quiz.QuizId;
        }
        
        public AddNewQuizVM GetNewQuiz(int QuizId)
        {
            var result = context.Quizzes.SingleOrDefault(q => q.QuizId == QuizId);
            if (result != null)
            {
                AddNewQuizVM quiz = new AddNewQuizVM { QuizId = result.QuizId, QuizName = result.QuizName, UserId = result.UserId };
                return quiz;
            }
            else
            {
                return null;
            }                           
        }

        public void SaveQuestions(List<NewQuestionVM> list, int QuizId)
        {
            Question q = null;
            foreach (var l in list)
            {
                q = new Question();
                q.QuizId = QuizId;
                q.QuestionNr = l.QuestionNr;
                q.QuestionString = l.QuestionString;
                q.RadioButtonAlternativeAnswer = (int)l.RadioButtonAlternativeAnswer;
                q.Answers = new List<Answer>();

                if (!(string.IsNullOrEmpty(l.Answer1) || string.IsNullOrWhiteSpace(l.Answer1)))
                {
                    q.Answers.Add(new Answer { AnswerText = l.Answer1, AnswerNr = 0 });
                }
                if (!(string.IsNullOrEmpty(l.Answer2) || string.IsNullOrWhiteSpace(l.Answer2)))
                {
                    q.Answers.Add(new Answer { AnswerText = l.Answer2, AnswerNr = 1 });
                }
                if (!(string.IsNullOrEmpty(l.Answer3) || string.IsNullOrWhiteSpace(l.Answer3)))
                {
                    q.Answers.Add(new Answer { AnswerText = l.Answer3, AnswerNr = 2 });
                }
                if (!(string.IsNullOrEmpty(l.Answer4) || string.IsNullOrWhiteSpace(l.Answer4)))
                {
                    q.Answers.Add(new Answer { AnswerText = l.Answer4, AnswerNr = 3 });
                }                
                context.Questions.Add(q);                
            }
            context.SaveChanges();
            //var result = context.Questions.Where(Q=>Q.QuizId == QuizId).ToList();
        }

        public ICollection<Question> Questions(string gameCode)
        {
            var result = (from questions in context.Questions
                          join quizzes in context.Quizzes on questions.QuizId equals quizzes.QuizId
                          join newGame in context.NewGames on quizzes.QuizId equals newGame.QuizId
                          where newGame.GameCodeId == gameCode
                          select questions).Include(q => q.Answers).OrderBy(q => q.QuestionNr).ToList<Question>();                             

            return result;
        }

        public ICollection<int> GetCorrectAnswersForQuestions(string gameCode)
        {
            var result = (from questions in context.Questions
                          join quizzes in context.Quizzes on questions.QuizId equals quizzes.QuizId
                          join newGame in context.NewGames on quizzes.QuizId equals newGame.QuizId
                          where newGame.GameCodeId == gameCode
                          select questions.RadioButtonAlternativeAnswer).ToList();
            return result;
        }

        public ICollection<Player> GetPlayers(string GameCode)
        {
            var result = context.Players.Where(p => p.GameCodeId == GameCode).ToList();
            return result;
        }
        
        public ICollection<Quiz> Quizzes(string userId) => context.Quizzes.Where(x => x.UserId == userId).Include(q => q.NewGames).ToList();
        
        public string getQuizName(string GameCode) 
        {
            var result =    (from n in context.NewGames
                            join q in context.Quizzes
                            on n.QuizId equals q.QuizId 
                            where (n.GameCodeId == GameCode)
                            select q.QuizName).SingleOrDefault();       
            return result;
        }
        
        public ICollection<string> getGameCodeIdsForUser(string userId, int quizId)
        {
            var result = (from n in context.NewGames
                       join q in context.Quizzes on n.QuizId equals q.QuizId
                       where (q.UserId == userId && q.QuizId == quizId)
                       select n.GameCodeId).ToList(); 
            return result;
        }
        
        public bool TryToSaveNewPlayer(PlayerInfoVM gameCodeVM)
        {
            int playersWithSameNicknameForThisQuiz = context.Players.Where(p => p.GameCodeId == gameCodeVM.GameCode && p.Name == gameCodeVM.NickName).Count();            
            if(playersWithSameNicknameForThisQuiz == 0)
            {
                context.Add(new Player { GameCodeId = gameCodeVM.GameCode, Name = gameCodeVM.NickName });
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }       
        }
                       
        public bool TryToUpdatePlayer(PlayerInfoVM gameCodeVM, List<int> ChosenAlternativeAnswer)
        {
            var player = context.Players.Where(p => p.GameCodeId == gameCodeVM.GameCode && p.Name == gameCodeVM.NickName).FirstOrDefault();

            if(!string.IsNullOrEmpty(player.Answers))
            {
                return false;
            }
            string AnswerAlternatives = JsonConvert.SerializeObject(ChosenAlternativeAnswer);

            if(player != null)
            {
                player.Answers = AnswerAlternatives;
                player.Score = gameCodeVM.Score;
                context.Update(player);
                context.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }

        public void SaveNewGame(NewGame newGame)
        {
            context.Add(newGame);
            context.SaveChanges();
        }

        public bool CheckIfPlayerExistsInGame(string Name, string GameCodeID) => context.Players.Where(p => p.Name == Name && p.GameCodeId == GameCodeID).Count() > 0 ? true : false;             
        
        public bool TryDeleteQuiz(int QuizId, string NameOfQuiz)
        {
            var result = context.Quizzes.SingleOrDefault(q => q.QuizId == QuizId && q.QuizName == NameOfQuiz);
            if(result != null)
            {
                context.Remove(result);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}



