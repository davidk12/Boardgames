using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Boardgames_webproject.Models;
using Boardgames_webproject.Models.ViewModels;

using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Boardgames_webproject.Filters;
using System.Data.SqlClient;


namespace Boardgames_webproject.Controllers
{
    public class GameController : Controller
    {
        BoardGameRepository repository = BoardGameRepository.getRepository;
        ErrorLogger error_logger = ErrorLogger.getLogger;

        #region Game Operations
        /// <summary>
        /// Loads the profile for a specific game. 
        /// </summary>
        /// <param name="game_id"></param>
        /// <returns></returns>
        public ActionResult Profile(int? game_id)
        {

            try
            {
                if (game_id == null) { return View("Error"); }                                      // if null is passed in, an error occurs
                User current_user = repository.getUserByName(User.Identity.Name);                   // getting the current user
                GameInstance user_game_instance = repository.userIsInGame(current_user.id);         // checking whether the user is in game already
                if (user_game_instance == null)                                                     // if the user isn't already active in a game
                {
                    UserAndGameVM model = new UserAndGameVM();                                      // preparing a new view model
                    int id = Convert.ToInt32(game_id);                                              // converting from nullable

                    model.game_list = repository.getAllGames();                                     // populating the games list
                    model.specific_game = repository.getGameById(id);
                    model.highest_rated_game = repository.getHighestRatedGame();                    // preparing the view model    
                    model.game_achievements = repository.getGameAchievements(id);
                    model.current_user_id = repository.getUserIdByName(User.Identity.Name);
                    model.active_games_list = repository.getActiveGamesById(id);
                    model.user_score_list = repository.getUserRankings(id);


                    if (model.specific_game == null)
                    {
                        return View("Error");
                    }
                    return View(model);
                }
                else
                {
                    //return RedirectToAction("Lobby", "Game", new { game_instance_id = user_game_instance.id });
                    return Lobby(user_game_instance.id);                                         // if the user IS already active in a game, he's redirected to its lobby
                }
            }
            catch (Exception general_exception)                                                  // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/Profile");                // logging the exception into the database via the error logger
                return View("Error");                                                            // returning an error view
            }

        }


        /// <summary>
        /// Attempts to create a new instance of a game if the user isn't active in one already
        /// If that is the case, he is redirected to the active game
        /// </summary>
        /// <param name="game_id">The id of the game to be created</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(int? game_id)
        {
            try
            {
                if (game_id != null)
                {
                    int converted_game_id = Convert.ToInt32(game_id);
                    User current_user = repository.getUserByName(User.Identity.Name);                   // fetching the user's ID by name
                    GameInstance user_game_instance = new GameInstance();
                    user_game_instance = repository.userIsInGame(current_user.id);                      // checking whether the user is active in a different game

                    if (user_game_instance == null)                                                     // if the user isn't active in any game, he can create a new instance
                    {
                        string session_name = current_user.username + "-Session";                       // setting the name of the session, which is prefixed with the owner's username
                        repository.createNewGame(current_user.id, converted_game_id, session_name);     // creating a new active game
                        user_game_instance = repository.userIsInGame(current_user.id);                  // fetching the game instance the user just created

                        return Lobby(user_game_instance.id);                                            // redirecting the user to the lobby of his game
                    }
                    else                                                                                // otherwise, if the user already had an active game
                    {
                        return Lobby(user_game_instance.id);                                            // he is redirected to the lobby of his game      
                    }
                }
                return RedirectToAction("index", "Home");                                               // if null is passed in, redirect to the index page
            }
            catch (Exception general_exception)                                                  // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/Create");      // logging the exception into the database via the error logger
                return View("Error");                                                            // returning an error view
            }
        }

        /// <summary>
        /// Redirects the user into the game lobby that was passed in. If the user is in a different lobby, 
        /// he's automatically redirected to the right one
        /// </summary>
        /// <param name="game_instance_id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Lobby(int? game_instance_id)
        {

            if (game_instance_id == null) { return View("Error"); }                             // if null is passed in, return an error view                   

            int id = Convert.ToInt32(game_instance_id);                                         // converting the id from a nullable integer

            User current_user = repository.getUserByName(User.Identity.Name);                   // getting the current user's info
            UserAndActiveGameVM model = new UserAndActiveGameVM();                              // preparing a new view model
            GameInstance current_game_instance = null;
            GameInstance user_game_instance = repository.userIsInGame(current_user.id);         // checking whether a user is active in a game

            if (user_game_instance == null) { return View("Error"); }                           // if the user isn't currently in any active game
            else
            {
                current_game_instance = repository.getActiveGameById(id);
                if (user_game_instance.id == id)
                {

                    model.specific_game_instance = repository.getActiveGameById(user_game_instance.id);                             // filling the view model with data
                    model.specific_user = current_user;
                    model.owner = repository.getOwnerByGameInstanceId(user_game_instance.id);
                    model.user_list = repository.getUsersByActiveGameId(user_game_instance.id);
                    model.user_rating_list = repository.getUserRankings(Convert.ToInt32(model.specific_game_instance.game_id));
                    model.current_user_id = repository.getUserIdByName(User.Identity.Name);

                    if (user_game_instance.is_in_lobby == true)                                                                     // if the game is in the lobby
                    {

                        foreach (var user in model.user_list)
                        {
                            if (user.id == model.owner.id)
                            {
                                model.user_list.Remove(user);                                                                       // removing the owner from the user list
                                break;
                            }
                        }
                        model.game_achievement_list = repository.getGameAchievements(Convert.ToInt32(model.specific_game_instance.game_id));


                        /*model.user_rating_list = (from r in model.user_rating_list                                                  // sorting the user rating list
                                                  orderby (r.total_wins * 100 / r.total_games * 100) descending
                                                  select r).ToList();*/
                        return View("Lobby", model);                                                                                // sending the view model into the lobby
                    }
                    else
                    {
                        return Board(model.specific_game_instance.id);                                                              // otherwise, the game is active and the user is send to its board
                    }

                }
                else                                                                                    // if the user isn't active in a game, he can't access any lobby                                                 
                {
                    return Lobby(user_game_instance.id);                                                // calling the Lobby action again recursively with the id of the game the user is in
                }
            }

        }


        /// <summary>
        /// Accepts the id of an active game and returns the appropriate board for the game type in a view
        /// </summary>
        /// <param name="game_instance"> The ID of the game instance that should be rendered</param>
        /// <returns>Returns an action result. Either by calling itself again or returning another view</returns>
        [Authorize]
        public ActionResult Board(int? game_instance)
        {
            try
            {
                if (game_instance == null)                                                                      // if null is passed in
                {
                    return View("Error");                                                                       // returns an error
                }
                User current_user = repository.getUserByName(User.Identity.Name);                               // getting the current user
                GameInstance user_game_instance = repository.userIsInGame(current_user.id);                     // checking whether the current user is active in a game

                if (user_game_instance == null)                                                                 // if the user isn't active in any game
                {
                    return RedirectToAction("index", "Home");
                }
                else                                                                                            // if the user IS active in a game
                {
                    if (user_game_instance.id == game_instance)                                                 // checking whether his active game is the same as the one requested                                           
                    {

                        if (user_game_instance.is_active == true)                                               // if the game is active and ongoing
                        {

                            UserAndActiveGameVM board_model = new UserAndActiveGameVM();                        // inserting all data into the view model
                            board_model.user_list = repository.getUsersByActiveGameId(user_game_instance.id);
                            board_model.specific_user = current_user;
                            board_model.owner = repository.getOwnerByGameInstanceId(user_game_instance.id);
                            board_model.specific_game_instance = user_game_instance;
                            board_model.current_user_id = current_user.id;
                            Game game_type = repository.getGameById(user_game_instance.game_id);

                            return View("~/Views/" + game_type.name + "/Board.cshtml", board_model);            // returning the appropriate game board in a vew

                        }
                        else if (user_game_instance.is_in_lobby == true)                                         // if the game is in lobby, the user is redirected there
                        {
                            return Lobby(user_game_instance.id);                                                // calling the lobby action
                        }
                        else                                                                                    // if the game is neither in lobby or active, the user is redirected to the index
                        {

                            return RedirectToAction("index", "Home");
                        }
                    }
                    else                                                                                        // if the user doesn't have access to the sent in game instance
                    {
                        return Board(user_game_instance.id);                                                    // calling the board action again with HIS game's ID
                    }
                }
            }
            catch (Exception general_exception)                                               // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/Board");    // logging the exception into the database via the error logger
                return View("Error");                                                         // returning an error view
            }

        }


        /// <summary>
        /// Removes a user from the game he's in and redirects him back to the game profile
        /// </summary>
        /// <param name="user_id">The user id</param>
        /// <returns>Returns an actionresult for the game profile view</returns>
        [Authorize]
        public ActionResult Leave(int? game_id)
        {
            try
            {
                if (game_id == null) { return View("Error"); }
                int converted_id = Convert.ToInt32(game_id);                                                                // converting the game id from nullable

                User current_user = repository.getUserByName(User.Identity.Name);                                           // getting the identity of the current user
                GameInstance user_game_instance = repository.userIsInGame(current_user.id);

                if (user_game_instance != null)                                                                             // if the user is active in a game
                {

                    if (current_user.id == user_game_instance.owner.id)                                                     // if the user is the owner of the game
                    {
                        repository.endGame(user_game_instance.id);                                                          // the game ends since it can't continue without the owner
                        return RedirectToAction("Profile", "Game", new { game_id = user_game_instance.game_id });

                    }
                    else
                    {
                        if (user_game_instance.is_active == true && user_game_instance.is_in_lobby == false)                // if the game is active and a single user leaves
                        {
                            repository.removeUserFromGame(current_user.id, user_game_instance.id);                          // the player is simply removed from the lobby
                            repository.sendGameToLobby(user_game_instance.id);                                              // sending the game back to the lobby
                            return Board(user_game_instance.id);
                        }
                        else                                                                                                // in the case that the game hasn't left the lobby
                        {
                            repository.removeUserFromGame(current_user.id, user_game_instance.id);                          // the player is simply removed from the lobby
                            return RedirectToAction("Profile", "Game", new { game_id = user_game_instance.game_id });       // redirecting the player back to the game profile
                        }
                    }

                }
                else
                {
                    return View("Error");                                                                                   // if the user isn't active in any game, return an error view
                }
            }
            catch (Exception general_exception)                                             // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/Leave");  // logging the exception into the database via the error logger
                return View("Error");                                                       // returning an error view
            }


        }


        /// <summary>
        /// Attempts to join a specific game instance with the current user
        /// </summary>
        /// <param name="game_id">The game instance id</param>
        /// <returns>
        /// Either redirects to the lobby/board of the requested game or 
        /// the lobby/board of the game the user is currently in
        /// </returns>
        [Authorize]
        public ActionResult Join(int? game_id)
        {
            try
            {
                if (game_id == null) { return View("Error"); }                                                  // if null is passed in, return an error view
                User current_user = repository.getUserByName(User.Identity.Name);                               // getting the identity of the current user
                GameInstance current_user_game = repository.userIsInGame(current_user.id);                      // checking if the user is currently active in any game
                int converted_id = Convert.ToInt32(game_id);                                                    // converting the game id from nullable
                int open_slots = repository.getAvailableSlots(converted_id);                                    // getting the current number of available slots

                if (current_user_game == null)                                                                  // if the current user isn't active in any game
                {
                    GameInstance current_game = repository.getActiveGameById(converted_id);                     // fetching the game instance requested

                    if (current_game.is_in_lobby != true || open_slots == 0)                                    // if the game has left the lobby, or has no remaining slots
                    {
                        return RedirectToAction("Profile", "Game", new { game_id = current_game.game_id });     // the user is redirected back to the profile of the game
                    }
                    else                                                                                        // otherwise, the game is in the lobby and has open slots
                    {
                        repository.joinGame(current_user.id, converted_id);                                     // calling the database and marking the user as part of the session                           
                        //current_user_game = repository.userIsInGame(current_user.id);                           // 
                        if (open_slots == 1) { repository.startGame(converted_id); };                           // if the number of slots available was 1, the game starts
                        return Lobby(converted_id);                                                             // redirecting the user to the lobby of the game

                    }
                }
                else                                                                                            // in the instance that the user is already active in a game
                {
                    return Lobby(current_user_game.id);                                                         // he is redirected to his game
                }
            }
            catch (Exception general_exception)                                             // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/Join");   // logging the exception into the database via the error logger
                return View("Error");                                                       // returning an error view
            }

        }
        #endregion

        #region Json Operations



        /// <summary>
        /// Rates a single user up within a game instance
        /// </summary>
        /// <param name="user_to_rate">The user to rate up</param>
        [Authorize]
        [HttpPost]
        public void rateUserUp(int user_to_rate)
        {
            try
            {
                User current_user = repository.getUserByName(User.Identity.Name);               // getting the current user info
                GameInstance user_game_instance = repository.userIsInGame(current_user.id);     // getting the current game instance's infi
                repository.rateUserUp(user_to_rate, current_user.id, user_game_instance.id);    // rating the desired user up
            }
            catch (Exception general_exception)                                                  // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/rateUserUp");  // logging the exception into the database via the error logger
            }

        }

        /// <summary>
        /// Rates a single user down within a game instance
        /// </summary>
        /// <param name="user_to_rate">The user to be rated</param>
        [Authorize]
        [HttpPost]
        public void rateUserDown(int user_to_rate)
        {
            try
            {
                User current_user = repository.getUserByName(User.Identity.Name);                    // getting the current user info
                GameInstance user_game_instance = repository.userIsInGame(current_user.id);          // getting the current game instance's infi
                repository.rateUserDown(user_to_rate, current_user.id, user_game_instance.id);       // rating the desired user up
            }
            catch (Exception general_exception)                                                      // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/rateUserDown");               // logging the exception into the database via the error logger
            }
        }


        /// <summary>
        /// Fetches a list of all active users in a game and checks whether the game is full 
        /// and ready to be started
        /// </summary>
        /// <returns>Returns a Json result </returns>
        [Authorize]
        [HttpGet]
        public JsonResult getActivePlayerList(int game_id)
        {
            try
            {
                ActiveUserListVM list_model = new ActiveUserListVM();
                list_model.is_game_full = repository.getAvailableSlots(game_id) == 0;                     // checking whether the game is full
                list_model.user_list = repository.getUsersByActiveGameId(game_id);                        // fetching all users in the active game
                //list_model.user_list.Insert(0, repository.getOwnerByGameInstanceId(game_id));
                return Json(list_model, JsonRequestBehavior.AllowGet);                                    // sending the user list by json
            }
            catch (Exception general_exception)                                                           // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/getActivePlayerList");  // logging the exception into the database via the error logger              
                return Json(null);                     // returning an error view
            }

        }


        /// <summary>
        /// Checks whether a game is active and returns a boolean value via Json based on the check
        /// </summary>
        /// <param name="game_id">The game's ID</param>
        /// <returns>Returns a json result</returns>
        [Authorize]
        [HttpGet]
        public JsonResult isGameActive(int game_id)
        {
            try
            {
                GameInstance current_game = repository.getActiveGameById(game_id);                          // getting the game instance
                bool is_closed = current_game.is_active == false && current_game.is_in_lobby == false;      // the instance that the game is closed

                if (is_closed || current_game.is_in_lobby == true)                                          // if the game is closed or is in the lobby
                {
                    return Json(false, JsonRequestBehavior.AllowGet);                                       // returning false (the game isn't active)
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);                                        // returning true (the game IS active)
                }
            }
            catch (Exception general_exception)                                                             // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/isGameActive");           // logging the exception into the database via the error logger              
                return Json(null);                     // returning an error view
            }
        }

        /// <summary>
        /// Adds 1 to the win count of the current player
        /// </summary>
        /// <param name="game_id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public void addWin(int game_id)
        {
            try
            {
                User current_user = repository.getUserByName(User.Identity.Name);
                repository.userWonGame(current_user.id, game_id, true);
            }
            catch (Exception general_exception)                                               // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/addWin");   // logging the exception into the database via the error logger              
            }

        }

        /// <summary>
        /// Adds 1 to the loss count of the current player via Json
        /// </summary>
        /// <param name="game_id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public void addLoss(int game_id)
        {
            try
            {
                User current_user = repository.getUserByName(User.Identity.Name);
                repository.userWonGame(current_user.id, game_id, false);
            }
            catch (Exception general_exception)                                                 // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/addLoss");    // logging the exception into the database via the error logger              
            }
        }

        /// <summary>
        /// Adds 1 to the tie count of the current player
        /// </summary>
        /// <param name="game_id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public void addTie(int game_id)
        {
            try
            {
                User current_user = repository.getUserByName(User.Identity.Name);
                repository.userTiedGame(current_user.id, game_id);
            }
            catch (Exception general_exception)                                                 // a general exception for all other cases
            {
                error_logger.logIntoDatabase(general_exception, "Game/addTie");     // logging the exception into the database via the error logger              
            }
        }

        #endregion


    }
}
