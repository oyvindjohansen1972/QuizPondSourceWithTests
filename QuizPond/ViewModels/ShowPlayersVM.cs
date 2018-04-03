using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizPond.Data.Models;

namespace QuizPond.ViewModels
{
    public class ShowPlayersVM
    {
        public string QuizName { get; set; }
        public List<GameCodeAndAllPlayersListVM> PlayersAndGameCode { get; set; }
    }


    public class GameCodeAndAllPlayersListVM
    {
        public string GameCode { get; set; }
        public List<PlayerListVM> Players { get; set; }        
    }


    public class PlayerListVM
    {        
        public string Name { get; set; }       
        public int GameScore { get; set; }
    }
}
