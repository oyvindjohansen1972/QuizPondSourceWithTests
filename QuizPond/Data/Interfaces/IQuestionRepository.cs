using System.Collections.Generic;
using System.Linq;
using QuizPond.Data.Models;
using QuizPond.ViewModels;

namespace QuizPond.Data.Interfaces

{
    public interface IQuestionRepository
    {
        void CheckIfNewGameIsMoreThan24HoursOldAndDeleteIfTrue(ICollection<Quiz> ListOfAllQuizzes);

        AddNewQuizVM GetNewQuiz(int QuizId);

        int SaveNewQuiz(AddNewQuizVM addNewQuizVM);

        bool ShowAnswersToPlayer(string gameCode);

        ICollection<Question> Questions(string gameCode);

        ICollection<int> GetCorrectAnswersForQuestions(string gameCode);

        ICollection<Player> GetPlayers(string GameCode);

        ICollection<Quiz> Quizzes(string userId);

        ICollection<string> getGameCodeIdsForUser(string userId, int quizId);

        bool TryToSaveNewPlayer(PlayerInfoVM gameCodeVM);

        bool TryToUpdatePlayer(PlayerInfoVM gameCodeVM, List<int> ChosenAlternativeAnswer);

        void SaveNewGame(NewGame newGame);

        void SaveQuestions(List<NewQuestionVM> list, int QuizId);        

        bool CheckIfPlayerExistsInGame(string Name, string GameCodeID);

        string getQuizName(string GameCode);

        bool TryDeleteQuiz(int QuizId, string NameOfQuiz); 
    }
}