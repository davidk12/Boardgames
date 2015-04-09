using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;             // for Web.config

namespace Boardgames_webproject.Models
{
    /// <summary>
    /// A collection of operations to retrieve various data from the database
    /// </summary>
    public class BoardGameRepository
    {
        DataConnection db = DataConnection.get_data_connection;

        private static BoardGameRepository repository;
        private BoardGameRepository() { }
        public static BoardGameRepository getRepository
        {
            get
            {
                if (repository == null)
                {
                    repository = new BoardGameRepository();
                }
                return repository;
            }
        }

        #region General User Operations

        /// <summary>
        /// Finds all registered users
        /// </summary>
        /// <returns>List of User instances</returns>
        public List<User> getAllUsers()
        {
            List<User> user_list = db.selectAllUsers();
            return user_list;
        }



        /// <summary>
        /// Finds a specific user
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>An instance of class User</returns>
        public User getUserById(int user_id)
        {
            User result = db.selectUser((int)user_id);
            return result;
        }


        /// <summary>
        /// Finds the id of the user with the greatest id number
        /// </summary>
        /// <returns>Highest user id, an integer</returns>
        public int getMaxUserId()
        {
            List<User> user_list = db.selectAllUsers();
            int max_id = -1;
            foreach (var user in user_list)
            {
                if (user.id > max_id)
                {
                    max_id = user.id;
                }
            }
            return max_id;
        }

        /// <summary>
        /// Finds the highest rated user
        /// </summary>
        /// <returns>An instance of class User</returns>
        public User getHighestRatedUser()
        {
            List<User> user_list = db.selectAllUsers();
            User result = new User();

            foreach (var user in user_list)
            {
                if (user.rating > result.rating)
                {
                    result = user;
                }
            }
            return result;
        }

        /// <summary>
        /// Finds a users id using his username
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <returns>The user's id number</returns>
        public int getUserIdByName(string username)
        {
            List<User> user_list = db.selectAllUsers();
            int result = -1;

            foreach (var user in user_list)
            {
                if (user.username == username)
                {
                    result = user.id;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Finds user information based on his name
        /// </summary>
        /// <param name="name">The name of the user to fetch</param>
        /// <returns>A single instance of user</returns>
        public User getUserByName(string name)
        {
            List<User> user_list = db.selectAllUsers();

            foreach (var user in user_list)
            {
                if (user.username == name)
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds all users in an active game
        /// </summary>
        /// <param name="game_id">The id of the game instance</param>
        /// <returns>List of User class instances</returns>
        public List<User> getUsersByActiveGameId(int game_id)
        {
            return db.selectUsersInGame(game_id);
        }

        /// <summary>
        /// Finds the owner of an active game
        /// </summary>
        /// <param name="game_instance_id">The id of the game instance</param>
        /// <returns>Single instance of class User</returns>
        public User getOwnerByGameInstanceId(int game_instance_id)
        {
            GameInstance active_game = db.selectActiveGame(game_instance_id);
            active_game.owner = db.selectUser(active_game.owner_id);
            return active_game.owner;
        }

        /// <summary>
        /// Increment a user's rating
        /// </summary>
        /// <param name="user_id">The ID of the user being rated</param>
        /// <param name="rater_id">The ID of the user doing the rating</param>
        /// <param name="game_id">The ID of the game instance</param>
        public void rateUserUp(int user_id, int rater_id, int game_instance_id)
        {
            db.updateUserRating(user_id, rater_id, game_instance_id, true);
        }

        /// <summary>
        /// Updates a users score in a particular game
        /// </summary>
        /// <param name="user_id">The user's ID</param>
        /// <param name="game_id">The game's ID</param>
        /// <param name="user_won">True if user won, otherwise false</param>
        public void userWonGame(int user_id, int game_id, bool user_won)
        {
            db.updateUserScore(user_id, game_id, user_won);
        }

        /// <summary>
        /// Updates the users score when there is a tie
        /// </summary>
        /// <param name="user_id">The User's ID</param>
        /// <param name="game_id">The game's ID</param>
        public void userTiedGame(int user_id, int game_id)
        {
            db.updateUserScoreTied(user_id, game_id);
        }

        /// <summary>
        /// Decrement a user's rating
        /// </summary>
        /// <param name="user_id">The ID of the user being rated</param>
        /// <param name="rater_id">The ID of the user doing the rating</param>
        /// <param name="game_id">The ID of the game instance</param>
        public void rateUserDown(int user_id, int rater_id, int game_instance_id)
        {
            db.updateUserRating(user_id, rater_id, game_instance_id, false);
        }

        #endregion

        #region Miscellaneous User Operations

        /// <summary>
        /// Finds all user rankings relating to a certain game
        /// </summary>
        /// <param name="game_id">The id of the game to fetch statistics for</param>
        /// <returns>A list of UserRanking instances</returns>
        public List<UserRanking> getUserRankings(int game_id)
        {
            return db.selectGameScores(game_id);
        }

        #endregion
        #region Statistics Operations

        /// <summary>
        /// Finds how often a player has won a game of any type
        /// </summary>
        /// <param name="user_id">The player's ID</param>
        /// <returns>The player's number of wins</returns>
        public int getTotalPlayerWins(int user_id)
        {
            int total_wins = 0;
            List<UserRanking> user_scores_list = db.selectUserScores(user_id);
            foreach (var user_scores in user_scores_list)
            {
                total_wins += user_scores.total_wins;
            }
            return total_wins;
        }

        /// <summary>
        /// Finds how often a player has lost a game of any type
        /// </summary>
        /// <param name="user_id">The player's ID</param>
        /// <returns>The player's number of losses</returns>
        public int getTotalPlayerLosses(int user_id)
        {
            int total_losses = 0;
            List<UserRanking> user_scores_list = db.selectUserScores(user_id);
            foreach (var user_scores in user_scores_list)
            {
                total_losses += user_scores.total_losses;
            }
            return total_losses;
        }

        /// <summary>
        /// Finds how often a player has played a game of any type
        /// </summary>
        /// <param name="user_id">The player's ID</param>
        /// <returns>The total number of games played by a player</returns>
        public int getTotalGamesPlayed(int user_id)
        {
            int total_played = 0;
            List<UserRanking> user_scores_list = db.selectUserScores(user_id);
            foreach (var user_scores in user_scores_list)
            {
                total_played += user_scores.total_games;
            }
            return total_played;
        }

        /// <summary>
        /// Finds a players sum of wins in a given game
        /// </summary>
        /// <param name="user_id">The player's ID</param>
        /// <param name="game_id">The game's ID</param>
        /// <returns>A players number of wins</returns>
        public int getTotalWinsInGame(int user_id, int game_id)
        {
            UserRanking user_score = db.selectUserScoresInGame(user_id, game_id);
            return user_score.total_wins;
        }

        /// <summary>
        /// Finds the sum of losses a players has in a given type of game
        /// </summary>
        /// <param name="user_id">The player's ID</param>
        /// <param name="game_id">The game's ID</param>
        /// <returns>A players number of losses</returns>
        public int getTotalLossesInGame(int user_id, int game_id)
        {
            UserRanking user_score = db.selectUserScoresInGame(user_id, game_id);
            return user_score.total_losses;
        }

        #endregion

        #region Game Operations

        /// <summary>
        /// Finds all possible games
        /// </summary>
        /// <returns>List of Game instances</returns>
        public List<Game> getAllGames()
        {
            return db.selectAllGames();
        }

        /// <summary>
        /// Finds the id of the game with the greatest id number
        /// </summary>
        /// <returns>Highest game id, an integer</returns>
        public int getMaxGameId()
        {
            List<Game> game_list = db.selectAllGames();
            int max_id = -1;
            if (game_list != null)
            {
                foreach (var game in game_list)
                {
                    if (game.id > max_id)
                    {
                        max_id = game.id;
                    }
                }
            }
            return max_id;
        }

        /// <summary>
        /// Finds a specific game
        /// </summary>
        /// <param name="game_id">The id of the game</param>
        /// <returns>Instance of class Game</returns>
        public Game getGameById(int game_id)
        {
            return db.selectGame(game_id);
        }

        /// <summary>
        /// Finds the highest rated game
        /// </summary>
        /// <returns>An instance of class Game</returns>
        public Game getHighestRatedGame()
        {
            Game result = new Game();
            List<Game> games = db.selectAllGames();

            foreach (var game in games)
            {
                if (game.rating > result.rating)
                {
                    result = game;
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a new active game
        /// </summary>
        /// <param name="owner_id">The game owner's ID</param>
        /// <param name="game_id">The game's ID</param>
        /// <param name="game_name">Name of the game</param>
        public void createNewGame(int owner_id, int game_id, string game_name)
        {
            db.addActiveGame(owner_id, game_id, game_name);
        }

        /// <summary>
        /// Adds a user to an active game
        /// </summary>
        /// <param name="user_id">The user id</param>
        /// <param name="game_id"></param>
        public void joinGame(int user_id, int game_id)
        {
            db.addUserToActiveGame(user_id, game_id);
        }

        /// <summary>
        /// Finds all active games regardless of type
        /// </summary>
        /// <returns>List of GameInstance instances</returns>
        public List<GameInstance> getAllActiveGames()
        {
            List<GameInstance> result = db.selectAllActiveGames();

            if (result != null)
            {
                foreach (var game in result)
                {
                    game.owner = db.selectUser(game.owner_id);
                }
            }
            return result;
        }

        /// <summary>
        /// Finds the id of the active game with the greatest id number
        /// </summary>
        /// <returns>Highest active game id, an integer</returns>
        public int getMaxActiveGameId()
        {
            List<GameInstance> active_game_list = db.selectAllActiveGames();
            int max_id = -1;

            if (active_game_list != null)
            {
                foreach (var active_game in active_game_list)
                {
                    if (active_game.id > max_id)
                    {
                        max_id = active_game.id;
                    }
                }
            }
            return max_id;
        }

        /// <summary>
        /// Checks if a user is an owner of an active game
        /// </summary>
        /// <param name="user_id">The user's ID</param>
        /// <returns>True if user owns an active game, otherwise false</returns>
        public GameInstance userIsInGame(int user_id)
        {
            GameInstance game = db.selectUsersGame(user_id);

            if (game != null)
            {
                game.owner = db.selectUser(game.owner_id);
                return game;
            }
            return null;
        }

        /// <summary>
        /// Finds all active games of a specific type
        /// </summary>
        /// <param name="game_id">Id of the game type</param>
        /// <returns>List of GameInstance instances</returns>
        public List<GameInstance> getActiveGamesById(int game_id)
        {
            List<GameInstance> result = db.selectActiveGames(game_id);

            if (result != null)
            {
                foreach (var game in result)
                {
                    game.owner = db.selectUser(game.owner_id);
                }
            }

            return result;
        }

        /// <summary>
        /// Finds a single instance of an active game
        /// </summary>
        /// <param name="game_id">Id of the game</param>
        /// <returns>Instance of class GameInstances</returns>
        public GameInstance getActiveGameById(int game_id)
        {
            GameInstance result = db.selectActiveGame(game_id);

            if (result != null)
            {
                result.owner = db.selectUser(result.owner_id);
            }
            return result;
        }

        /// <summary>
        /// Find how many slots an active game has open for new users
        /// </summary>
        /// <param name="game_id">The game's ID</param>
        /// <returns>The number of available slots</returns>
        public int getAvailableSlots(int game_id)
        {
            return db.selectAvailableSlots(game_id);
        }

        /// <summary>
        /// Removes a single user from an active game
        /// </summary>
        /// <param name="user_id">The user's ID</param>
        /// <param name="game_id">The game's ID</param>
        public void removeUserFromGame(int user_id, int game_id)
        {
            db.removeUserFromActiveGame(user_id, game_id);
        }

        /// <summary>
        /// Starts the selected game
        /// </summary>
        /// <param name="game_id">The game's ID</param>
        public void startGame(int game_id)
        {
            db.playActiveGame(game_id);
        }

        /// <summary>
        /// Ends the selected game
        /// </summary>
        /// <param name="game_id">The game's ID</param>
        public void endGame(int game_id)
        {
            db.gameOver(game_id);
        }

        /// <summary>
        /// Sends the selected game back to the lobby
        /// </summary>
        /// <param name="game_id">The game's ID</param>
        public void sendGameToLobby(int game_id)
        {
            db.backToLobby(game_id);
        }

        #endregion

        #region Form Operations

        #endregion

        #region Achievement Operations

        /// <summary>
        /// Finds all achievements associated with a single game
        /// </summary>
        /// <param name="game_id">The id of the game</param>
        /// <returns>Returns a List of Achievement instances</returns>
        public List<Achievement> getGameAchievements(int game_id)
        {
            return db.selectGameAchievements(game_id);
        }


        /// <summary>
        /// Finds all achievements earned by a single players
        /// </summary>
        /// <param name="user_id">The id of the players</param>
        /// <returns>Returns a list of Achievements instances</returns>
        public List<Achievement> getUserAchievements(int user_id)
        {
            return db.selectUserAchievements(user_id);
        }

        /// <summary>
        /// Finds a single achievement
        /// </summary>
        /// <param name="achievement_id">The ID of the Achievement to be found</param>
        /// <returns>Instance of class Achievement</returns>
        public Achievement getAchievement(int achievement_id)
        {
            List<Achievement> achievement_list = db.selectAllAchievements();

            if (achievement_list != null)
            {
                Achievement result = new Achievement();
                foreach (var achievement in achievement_list)
                {
                    if (achievement.achievement_id == achievement_id)
                    {
                        result = achievement;
                        break;
                    }
                }
                return result;
            }
            return null;
        }
        #endregion

        #region Exception handling

        /// <summary>
        /// Logs an exception in the database
        /// </summary>
        /// <param name="type">Exception type</param>
        /// <param name="message">Exception message</param>
        public void logException(string type, string message, string function)
        {
            db.addLogException(type, message, function);
        }

        #endregion
    }
}