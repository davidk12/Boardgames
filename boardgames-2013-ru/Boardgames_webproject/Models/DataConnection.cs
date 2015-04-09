using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Boardgames_webproject.Models.EntityModels;            // for Web.config

namespace Boardgames_webproject.Models
{
    /// <summary>
    /// DataConnection class handles all communication with the
    /// applications database server through its member functions
    /// </summary>
    class DataConnection
    {
        string connection_string = ConfigurationManager.ConnectionStrings["AppDataContext"].ConnectionString;
        SqlDataReader reader = null;

        private static DataConnection data_connection;
        private DataConnection() { }
        public static DataConnection get_data_connection
        {
            get
            {
                if (data_connection == null)
                {
                    data_connection = new DataConnection();
                }
                return data_connection;
            }
        }

        #region User related operations

        /// <summary>
        ///     Finds all achievements that a user has earned
        /// </summary>
        /// <param name="user_id">
        ///     id of the user
        /// </param>
        /// <returns>
        ///     A list containing instances of Achievement
        /// </returns>
        public List<Achievement> selectUserAchievements(int user_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectUserAchievements", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_userId", user_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<Achievement> result = new List<Achievement>();
                    Achievement achievement = new Achievement();
                    while (reader.Read())
                    {
                        achievement.achievement_id = (int)reader["ID"];
                        achievement.name = reader["Achievement"].ToString();
                        achievement.description = reader["description"].ToString();
                        achievement.game_id = (int)reader["game_FK"];

                        result.Add(achievement);
                        achievement = new Achievement();
                    }
                    reader.Close();
                    return result;
                }

                return null;
            }
        }

        /// <summary>
        ///     Finds a particular user
        /// </summary>
        /// <returns>
        ///    Instance of class User
        /// </returns>
        public User selectUser(int user_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectUser", connection);
                cmd.Parameters.Add(new SqlParameter("@_userId", user_id));
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    User result = new User();
                    result.id = (int)reader["UserId"];
                    result.username = reader["UserName"].ToString();
                    result.bio = reader["bio"].ToString();
                    result.rating = (int)reader["rating"];
                    reader.Close();

                    return result;
                }

                return null;
            }
        }

        /// <summary>
        ///     Finds all users in the database
        /// </summary>
        /// <returns>
        ///    A list containing instances of User
        /// </returns>
        public List<User> selectAllUsers()
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();
                List<User> result = new List<User>();
                User user = new User();

                SqlCommand cmd = new SqlCommand("SelectAllUsers", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user.id = (int)reader["UserId"];
                        user.username = reader["UserName"].ToString();
                        user.bio = reader["bio"].ToString();
                        user.rating = (int)reader["rating"];

                        result.Add(user);
                        user = new User();
                    }
                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Finds all users currently in an active game
        /// </summary>
        /// <param name="game_id">
        ///     Id of the game being played
        /// </param>
        /// <returns>
        ///     A list containing instances of User
        /// </returns>
        public List<User> selectUsersInGame(int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectUsersInGame", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_gameId", game_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<User> result = new List<User>();
                    User user = new User();
                    while (reader.Read())
                    {
                        user.id = (int)reader["UserId"];
                        user.username = reader["UserName"].ToString();
                        user.bio = reader["bio"].ToString();
                        user.rating = (int)reader["rating"];

                        result.Add(user);
                        user = new User();
                    }
                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Finds what game a user is currently playing
        /// </summary>
        /// <param name="user_id">
        ///     Id of the user
        /// </param>
        /// <returns>
        ///     Instance of GameInstance class
        /// </returns>
        public GameInstance selectUsersGame(int user_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectUsersGame", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_userId", user_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    GameInstance result = new GameInstance();

                    result.id = (int)reader["ID"];
                    result.name = reader["name"].ToString();
                    result.is_active = (bool)reader["is_active"];
                    result.is_in_lobby = (bool)reader["is_in_lobby"];
                    result.is_locked = (bool)reader["is_locked"];
                    result.start_time = (DateTime)reader["start_time"];
                    result.game_id = (int)reader["game_FK"];
                    result.owner_id = (int)reader["owner_FK"];

                    reader.Close();
                    return result;
                }

                return null;
            }
        }

        /// <summary>
        ///     Updates a specific users bio
        /// </summary>
        /// <param name="user_id">
        ///     Id of the user
        /// </param>
        /// <param name="new_bio">
        ///     The replacement bio
        /// </param>
        public void updateUserBio(int user_id, string new_bio)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateUserBio", connection))
                {
                    cmd.Parameters.Add("@_userId", SqlDbType.Int).Value = user_id;
                    cmd.Parameters.Add("@_newBio", SqlDbType.Text).Value = new_bio;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Updates a specific users score in a specific game
        /// </summary>
        /// <param name="user_id">
        ///     Id of the user
        /// </param>
        /// <param name="game_id">
        ///     Id of the game
        /// </param>
        /// <param name="user_won">
        ///     A boolean variable that determines if the user won the game or not
        /// </param>
        public void updateUserScore(int user_id, int game_id, bool user_won)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateUserScore", connection))
                {
                    cmd.Parameters.Add("@_userId", SqlDbType.Int).Value = user_id;
                    cmd.Parameters.Add("@_gameId", SqlDbType.Int).Value = game_id;
                    cmd.Parameters.Add("@_userWon", SqlDbType.Bit).Value = user_won;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Updates the UserScores table when there is a tie
        /// </summary>
        /// <param name="user_id">The User's ID</param>
        /// <param name="game_id">The game's ID</param>
        public void updateUserScoreTied(int user_id, int game_id)
        {

            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateUserScoreTied", connection))
                {
                    cmd.Parameters.Add("@_userId", SqlDbType.Int).Value = user_id;
                    cmd.Parameters.Add("@_gameId", SqlDbType.Int).Value = game_id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Finds all of a certain users scores
        /// </summary>
        /// <param name="user_id">
        ///     Id of the user
        /// </param>
        /// <returns>
        ///     A list containing instances of UserRanking
        /// </returns>
        public List<UserRanking> selectUserScores(int user_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectUserScores", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_userId", user_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<UserRanking> result = new List<UserRanking>();
                    UserRanking user_score = new UserRanking();
                    while (reader.Read())
                    {
                        user_score.username = reader["UserName"].ToString();
                        user_score.total_games = (int)reader["TotalPlayed"];
                        user_score.total_wins = (int)reader["TotalWins"];
                        user_score.total_losses = (int)reader["TotalLosses"];

                        result.Add(user_score);
                        user_score = new UserRanking();
                    }
                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Finds all users score in a particular game
        /// </summary>
        /// <param name="game_id">
        ///     Id of the game
        /// </param>
        /// <returns>
        ///     A list containing instances of UserRanking
        /// </returns>
        public List<UserRanking> selectGameScores(int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectGameScores", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_gameId", game_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<UserRanking> result = new List<UserRanking>();
                    UserRanking user_score = new UserRanking();
                    while (reader.Read())
                    {
                        user_score.username = reader["UserName"].ToString(); ;
                        user_score.total_games = (int)reader["TotalPlayed"];
                        user_score.total_wins = (int)reader["TotalWins"];
                        user_score.total_losses = (int)reader["TotalLosses"];

                        result.Add(user_score);
                        user_score = new UserRanking();
                    }
                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Finds all of a certain users scores in a particular game
        /// </summary>
        /// <param name="user_id">
        ///     Id of the user
        /// </param>
        /// <param name="game_id">
        ///     Id of the game
        /// </param>
        /// <returns>
        ///     A list containing instances of UserRanking
        /// </returns>
        public UserRanking selectUserScoresInGame(int user_id, int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectUserScoresInGame", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_userId", user_id));
                cmd.Parameters.Add(new SqlParameter("@_gameId", game_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    UserRanking result = new UserRanking();

                    result.username = reader["UserName"].ToString(); ;
                    result.total_games = (int)reader["TotalPlayed"];
                    result.total_wins = (int)reader["TotalWins"];
                    result.total_losses = (int)reader["TotalLosses"];

                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Grants a user an added achievement
        /// </summary>
        /// <param name="user_id">
        ///     Id of the user
        /// </param>
        /// <param name="achievement_id">
        ///     Id of the achievement
        /// </param>
        public void addUserAchievement(int user_id, int achievement_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("AddUserAchievement", connection))
                {
                    cmd.Parameters.Add("@_userId", SqlDbType.Int).Value = user_id;
                    cmd.Parameters.Add("@_achievement", SqlDbType.Int).Value = achievement_id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Changes a specific users rating
        /// </summary>
        /// <param name="user_id">
        ///     Id of the user to be rated
        /// </param>
        /// <param name="game_id">
        ///     Id of the game instance that was played
        /// </param>
        /// <param name="rater_id">
        ///     Id of the user that did the rating
        /// </param>
        /// <param name="positive_rating">
        ///     A boolean variable determining if the rating change is positive or not
        /// </param>
        public void updateUserRating(int user_id, int rater_id, int game_id, bool positive_rating)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateUserRating", connection))             //Stored procedure UpdateRating called
                {
                    cmd.Parameters.Add("@_userId", SqlDbType.Int).Value = user_id;                  //User's id passed to the procedure
                    cmd.Parameters.Add("@_positiveRating", SqlDbType.Bit).Value = positive_rating;  //Boolean
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand("UpdateRatingLog", connection))
                {
                    cmd.Parameters.Add("@_userId", SqlDbType.Int).Value = user_id;
                    cmd.Parameters.Add("@_raterId", SqlDbType.Int).Value = rater_id;
                    cmd.Parameters.Add("@_gameId", SqlDbType.Int).Value = game_id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Game related operations

        /// <summary>
        ///     Finds all games to be found in the database
        /// </summary>
        /// <returns>
        ///     A list containing instances of Game
        /// </returns>
        public List<Game> selectAllGames()
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectAllGames", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<Game> result = new List<Game>();
                    Game game = new Game();
                    while (reader.Read())
                    {
                        game.id = (int)reader["ID"];
                        game.name = reader["name"].ToString();
                        game.description = reader["game_description"].ToString();
                        game.slots = (int)reader["slots"];
                        game.rating = (int)reader["rating"];

                        result.Add(game);
                        game = new Game();
                    }
                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Finds information regarding a single game
        /// </summary>
        /// <param name="game_id">
        ///     Id of the game
        /// </param>
        /// <returns>
        ///     A single instances of Game
        /// </returns>
        public Game selectGame(int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectGame", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_gameId", game_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Game result = new Game();

                    result.id = (int)reader["ID"];
                    result.name = reader["name"].ToString();
                    result.description = reader["game_description"].ToString();
                    result.slots = (int)reader["slots"];
                    result.rating = (int)reader["rating"];

                    reader.Close();
                    return result;
                }
                return null;
            }
        }


        /// <summary>
        ///     Finds all possible achievements for a specific game in the database
        /// </summary>
        /// <returns>
        ///     A list containing instances of Achievement
        /// </returns>
        public List<Achievement> selectGameAchievements(int id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectGameAchievements", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_gameId", id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<Achievement> result = new List<Achievement>();
                    Achievement achievement = new Achievement();
                    while (reader.Read())
                    {
                        achievement.achievement_id = (int)reader["ID"];
                        achievement.name = reader["Achievement"].ToString();
                        achievement.description = reader["description"].ToString();
                        achievement.game_id = (int)reader["game_FK"];

                        result.Add(achievement);
                        achievement = new Achievement();
                    }
                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Finds all announcements of a given game
        /// </summary>
        /// <param name="game_id">
        ///     Id of the game
        /// </param>
        /// <returns>
        ///     A list containing instances of GameAnnouncement
        /// </returns>
        public List<GameAnnouncement> selectGameAnnouncements(int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectGameAnnouncements", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_gameId", game_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<GameAnnouncement> result = new List<GameAnnouncement>();
                    GameAnnouncement announcement = new GameAnnouncement();
                    while (reader.Read())
                    {
                        announcement.id = (int)reader["ID"];
                        announcement.title = reader["title"].ToString();
                        announcement.body = reader["body"].ToString();
                        announcement.time_stamp = (DateTime)reader["time_stamp"];
                        announcement.game_id = (int)reader["game_FK"];

                        result.Add(announcement);
                        announcement = new GameAnnouncement();
                    }
                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Adds a game announcement
        /// </summary>
        /// <param name="title">
        ///     Title of the announcement
        /// </param>
        /// <param name="body">
        ///     The announcements main body
        /// </param>
        /// <param name="game_id">
        ///     Id of the game that the announcement is for
        /// </param>
        public void addGameAnnouncement(string title, string body, int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("AddGameAnnouncement", connection))
                {
                    cmd.Parameters.Add("@_title", SqlDbType.VarChar).Value = title;
                    cmd.Parameters.Add("@_body", SqlDbType.Text).Value = body;
                    cmd.Parameters.Add("@_gameId", SqlDbType.Int).Value = game_id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Changes a specific games rating
        /// </summary>
        /// <param name="game_id">
        ///     Id of the game to be rated
        /// </param>
        /// <param name="positive_rating">
        ///     A boolean variable determining if the rating change is positive or not
        /// </param>
        public void updateGameRating(int game_id, bool positive_rating)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateGameRating", connection))
                {
                    cmd.Parameters.Add("@_gameId", SqlDbType.Int).Value = game_id;
                    cmd.Parameters.Add("@_positiveRating", SqlDbType.Bit).Value = positive_rating;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Finds all possible achievements for any game in the database
        /// </summary>
        /// <returns>
        ///     A list containing instances of Achievement
        /// </returns>
        public List<Achievement> selectAllAchievements()
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectAllAchievements", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<Achievement> result = new List<Achievement>();
                    Achievement achievement = new Achievement();
                    while (reader.Read())
                    {
                        achievement.achievement_id = (int)reader["ID"];
                        achievement.name = reader["Achievement"].ToString();
                        achievement.description = reader["description"].ToString();
                        achievement.game_id = (int)reader["game_FK"];

                        result.Add(achievement);
                        achievement = new Achievement();
                    }
                    reader.Close();
                    return result;
                }
                return null;
            }
        }
        #region Active game operations

        /// <summary>
        ///     Adds a user to an active game
        /// </summary>
        /// <param name="user_id">
        ///     Id of the user
        /// </param>
        /// <param name="game_id">
        ///     Id of the active game
        /// </param>
        public void addUserToActiveGame(int user_id, int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("AddUserToActiveGame", connection))
                {
                    cmd.Parameters.Add("@_userId", SqlDbType.Int).Value = user_id;
                    cmd.Parameters.Add("@_gameId", SqlDbType.Int).Value = game_id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Removes a user from an active game
        /// </summary>
        /// <param name="user_id">
        ///     Id of the user
        /// </param>
        /// <param name="game_id">
        ///     Id of the active game
        /// </param>
        public void removeUserFromActiveGame(int user_id, int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("RemoveUserFromActiveGame", connection))
                {
                    cmd.Parameters.Add("@_userId", SqlDbType.Int).Value = user_id;
                    cmd.Parameters.Add("@_gameId", SqlDbType.Int).Value = game_id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Finds all active games regardless of the game being played
        /// </summary>
        /// <returns>
        ///     A list containing instances of GameInstance
        /// </returns>
        public List<GameInstance> selectAllActiveGames()
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectAllActiveGames", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<GameInstance> result = new List<GameInstance>();
                    GameInstance active_game = new GameInstance();
                    while (reader.Read())
                    {
                        active_game.id = (int)reader["ID"];
                        active_game.name = reader["name"].ToString();
                        active_game.is_locked = (bool)reader["is_locked"];
                        active_game.start_time = (DateTime)reader["start_time"];
                        active_game.game_id = (int)reader["game_FK"];
                        active_game.owner_id = (int)reader["owner_FK"];

                        result.Add(active_game);
                        active_game = new GameInstance();
                    }
                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Finds all instances of a certain game being played
        /// </summary>
        /// <param name="game_id">
        ///     Id of the game
        /// </param>
        /// <returns>
        ///     A list containing instances of GameInstance
        /// </returns>
        public List<GameInstance> selectActiveGames(int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectActiveGames", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_gameId", game_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    reader = cmd.ExecuteReader();

                    List<GameInstance> result = new List<GameInstance>();
                    GameInstance active_game = new GameInstance();
                    while (reader.Read())
                    {
                        active_game.id = (int)reader["ID"];
                        active_game.name = reader["name"].ToString();
                        active_game.is_active = (bool)reader["is_active"];
                        active_game.is_in_lobby = (bool)reader["is_in_lobby"];
                        active_game.is_locked = (bool)reader["is_locked"];
                        active_game.start_time = (DateTime)reader["start_time"];
                        active_game.game_id = (int)reader["game_FK"];
                        active_game.owner_id = (int)reader["owner_FK"];

                        result.Add(active_game);
                        active_game = new GameInstance();
                    }

                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Finds the number of available slots a game has
        /// </summary>
        /// <param name="game_id">
        ///     Id of the game
        /// </param>
        /// <returns>
        ///     The number of available slots
        /// </returns>
        public int selectAvailableSlots(int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();
                int result = 0;

                SqlCommand cmd = new SqlCommand("SelectAvailableSlots", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_activeGameId", game_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    result = (int)reader["availableSlots"];
                    reader.Close();
                }
                return result;
            }
        }

        /// <summary>
        ///     Finds a single instance of an active game being played
        /// </summary>
        /// <param name="game_id">
        ///     Id of the active game
        /// </param>
        /// <returns>
        ///     A single instances of GameInstance
        /// </returns>
        public GameInstance selectActiveGame(int active_game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("SelectActiveGame", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_activeGameId", active_game_id));
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    GameInstance result = new GameInstance();

                    result.id = (int)reader["ID"];
                    result.name = reader["name"].ToString();
                    result.is_active = (bool)reader["is_active"];
                    result.is_in_lobby = (bool)reader["is_in_lobby"];
                    result.is_locked = (bool)reader["is_locked"];
                    result.start_time = (DateTime)reader["start_time"];
                    result.game_id = (int)reader["game_FK"];
                    result.owner_id = (int)reader["owner_FK"];

                    reader.Close();
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///     Creates an instance of an active game
        /// </summary>
        /// <param name="owner_id">
        ///     The player that started the game
        /// </param>
        /// <param name="game_id">
        ///     Id of the game being played
        /// </param>
        /// <param name="game_name">
        ///     Name of the active game
        /// </param>
        public void addActiveGame(int owner_id, int game_id, string game_name)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("AddActiveGame", connection))
                {
                    cmd.Parameters.Add("@_ownerId", SqlDbType.Int).Value = owner_id;
                    cmd.Parameters.Add("@_gameId", SqlDbType.Int).Value = game_id;
                    cmd.Parameters.Add("@_gameName", SqlDbType.VarChar).Value = game_name;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Sets the status of the game to active
        /// </summary>
        /// <param name="game_id">The game's ID</param>
        public void playActiveGame(int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("PlayActiveGame", connection))
                {
                    cmd.Parameters.Add("@_activeGameId", SqlDbType.Int).Value = game_id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Sets the game to inactive and not in lobby, as well as removing all players from the game itself
        /// </summary>
        /// <param name="game_id">The game's ID</param>
        public void gameOver(int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("GameOver", connection))
                {
                    cmd.Parameters.Add("@_activeGameId", SqlDbType.Int).Value = game_id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Sends the active game back to the lobby
        /// </summary>
        /// <param name="game_id">The game's ID</param>
        public void backToLobby(int game_id)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SendGameToLobby", connection))
                {
                    cmd.Parameters.Add("@_game_id", SqlDbType.Int).Value = game_id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #endregion

        /// <summary>
        ///     Logs an exception
        /// </summary>
        /// <param name="type">Type of exception</param>
        /// <param name="message">Exception message</param>
        public void addLogException(string type, string message, string function)
        {
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("AddExceptionLog", connection))
                {
                    cmd.Parameters.Add("@_exception_type", SqlDbType.VarChar).Value = type;
                    cmd.Parameters.Add("@_exception_message", SqlDbType.Text).Value = message;
                    cmd.Parameters.Add("@_name_of_function", SqlDbType.Text).Value = function;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}