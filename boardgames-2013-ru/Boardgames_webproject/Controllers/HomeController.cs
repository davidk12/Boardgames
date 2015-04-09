using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Boardgames_webproject.Models;
using Microsoft.CSharp.RuntimeBinder;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace Boardgames_webproject.Controllers
{
    public class HomeController : Controller
    {
        BoardGameRepository repository = BoardGameRepository.getRepository;
        public ActionResult index()
        {
            GameVM model = new GameVM();
     
            model.game_list = repository.getAllGames();
            model.highest_rated_game = repository.getHighestRatedGame();
            model.current_user_id = repository.getUserIdByName(User.Identity.Name);

            return View(model);
        }

        public ActionResult PlayerList()
        {
            UserAndGameVM model = new UserAndGameVM();

            model.game_list = repository.getAllGames();
            model.highest_rated_game = repository.getHighestRatedGame();
            model.user_list = repository.getAllUsers();
            model.highest_rated_user = repository.getHighestRatedUser();
            model.current_user_id = repository.getUserIdByName(User.Identity.Name);
            return View(model);
        }

 
        /// <summary>
        /// Redirects to a specific user profile page if one exists with the id 
        /// that is sent into the action
        /// </summary>
        /// <param name="user_id">The id of the request user</param>
        /// <returns>Returns a view with a user's profile</returns>
        public ActionResult Profile(int? user_id)
        {
            UserAndGameVM model = new UserAndGameVM();                          // instancing a new view model
            model.user_list = repository.getAllUsers();                         // fetching a list of all users
            model.highest_rated_game = repository.getHighestRatedGame();        // fetching the highest rated game
            model.current_user_id = repository.getUserIdByName(User.Identity.Name);

            
            int id = Convert.ToInt32(user_id);                                  // converting from a nullable type to integer     
            model.game_achievements = repository.getUserAchievements(id);
            int max_id = 0;

            if (user_id == null)                                                // if a null user id is sent in
            {
                
                return View("PlayerList", model);                               // redirects to the playerlist view
            }
            else                                                                // otherwise, if the id contains something
            {
                max_id = repository.getMaxUserId();                            // getting the highest user id in the database
                          

                if (id < 1 || id > max_id)                                      // checking if the user id within bounds
                {
                    model.error_message = "Player doesn't exist";               // setting an error message to be sent into the view
                    return View("PlayerList", model);                           // redirecting to the playerlist with the error message
                }
                else                                                            // otherwise, if the user id IS within bounds
                {
                    model.specific_user = repository.getUserById(id);           // fetching the requested user from the database
                    //model.user_score_list = repository.get
                    model.specific_user_scores.total_games = repository.getTotalGamesPlayed(id);
                    model.specific_user_scores.total_wins = repository.getTotalPlayerWins(id);
                    model.specific_user_scores.total_losses = repository.getTotalPlayerLosses(id);
                    model.specific_user_scores.total_ties = model.specific_user_scores.total_games - model.specific_user_scores.total_wins - model.specific_user_scores.total_losses;
                    return View(model);                                         // sending the view model into the view with the user data
                }
            }
        }

        

        
    }
}
