/*AddActiveGametest*/
GO
CREATE PROCEDURE AddActiveGametest(@_ownerId INT, @_gameId INT, @_gameName varchar(32))
AS
BEGIN
	INSERT INTO ActiveGames(name, game_FK, owner_FK, is_active, is_in_lobby, is_locked, start_time)
	VALUES(@_gameName, @_gameId, @_ownerId, 0, 1, 0, CURRENT_TIMESTAMP)

	DECLARE @_activeGameId INT;
	SET @_activeGameId = (SELECT MAX(ID) FROM ActiveGames WHERE owner_FK = @_ownerId);

	EXEC AddUserToActiveGame @_ownerId, @_activeGameId
END

/*PlayActiveGame*/
GO
CREATE PROCEDURE PlayActiveGame(@_activeGameId INT)
AS
BEGIN
	UPDATE ActiveGames
		SET is_active = 1, is_in_lobby = 0
		WHERE ID = @_activeGameId
END

/*GameOver*/
GO
CREATE PROCEDURE GameOver(@_activeGameId INT)
AS
BEGIN
	UPDATE ActiveGames
		SET is_active = 0, is_in_lobby = 0
		WHERE ID = @_activeGameId

	DELETE FROM UsersInGame
		WHERE game_FK = @_activeGameId
END

/*AddUserToActiveGame*/
GO
CREATE PROCEDURE AddUserToActiveGame(@_userId INT, @_gameId INT)
AS
BEGIN
	BEGIN TRANSACTION
	INSERT INTO UsersInGame(user_FK, game_FK)
		VALUES(@_userId, @_gameId)

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*RemoveUserFromActiveGame*/
GO
CREATE PROCEDURE RemoveUserFromActiveGame(@_userId INT, @_gameId INT)
AS
BEGIN
	BEGIN TRANSACTION
	DELETE FROM UsersInGame
		WHERE user_FK = @_userId
		AND
		game_FK = @_gameId

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*AddUserAchievement*/
GO
CREATE PROCEDURE AddUserAchievement(@_userId INT,  @_achievement INT)
AS
BEGIN
	BEGIN TRANSACTION
	INSERT INTO UserAchievements(user_FK, achievement_FK)
	VALUES(@_userId, @_achievement)

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*AddGameAnnouncement*/
GO
CREATE PROCEDURE AddGameAnnouncement(@_title VARCHAR(50),  @_body TEXT, @_gameId INT)
AS
BEGIN
	BEGIN TRANSACTION
	INSERT INTO GameAnnouncements(title, body, time_stamp, game_FK)
	VALUES(@_title, @_body, CURRENT_TIMESTAMP, @_gameId)

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*UpdateRatingLog*/
GO
CREATE PROCEDURE UpdateRatingLog(@_userId INT, @_raterId INT, @_gameId INT)
AS
BEGIN
	BEGIN TRANSACTION

	INSERT INTO RatingLog(user_FK, rater_FK, game_FK)
		VALUES(@_userId, @_raterId, @_gameId)

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*UpdateUserRating*/
GO
CREATE PROCEDURE UpdateUserRating(@_userId INT, @_positiveRating BIT)
AS
BEGIN
	BEGIN TRANSACTION

	DECLARE @_newRating INT;

	IF(@_positiveRating > 0)
		BEGIN
			SET @_newRating = (SELECT rating FROM UserProfile WHERE UserId = @_userId) + 1
		END
	ELSE
		BEGIN
			SET @_newRating = (SELECT rating FROM UserProfile WHERE UserId = @_userId) - 1
		END

	UPDATE UserProfile
		SET rating = @_newRating
		WHERE UserId = @_userId

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*UpdateGameRating*/
GO
CREATE PROCEDURE UpdateGameRating(@_gameId INT, @_positiveRating BIT)
AS
BEGIN
	BEGIN TRANSACTION

	DECLARE @_newRating INT;

	IF(@_positiveRating > 0)
		BEGIN
			SET @_newRating = (SELECT rating FROM Games WHERE ID = @_gameId) + 1
		END
	ELSE
		BEGIN
			SET @_newRating = (SELECT rating FROM Games WHERE ID = @_gameId) - 1
		END

	UPDATE Games
		SET rating = @_newRating
		WHERE ID = @_gameId

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*UpdateUserScoreTied*/
GO
CREATE PROCEDURE UpdateUserScoreTied(@_userId INT, @_gameId INT)
AS
BEGIN
	BEGIN TRANSACTION

	IF NOT EXISTS(SELECT* FROM UserScores WHERE game_FK = @_gameId AND user_FK = @_userId)
		BEGIN
			INSERT INTO UserScores(total_games, total_wins, total_losses, game_FK, user_FK)
				VALUES(1, 0, 0, @_gameId, @_userId)
		END
	ELSE
		BEGIN
			UPDATE UserScores
				SET total_games = (SELECT total_games FROM UserScores WHERE game_FK = @_gameID AND user_FK = @_userId) + 1
				WHERE game_FK = @_gameId AND user_FK = @_userId
		END

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*UpdateUserScore*/
GO
CREATE PROCEDURE UpdateUserScore(@_userId INT, @_gameId INT, @_userWon BIT)
AS
BEGIN
	BEGIN TRANSACTION

	IF NOT EXISTS(SELECT* FROM UserScores WHERE game_FK = @_gameId AND user_FK = @_userId)
		BEGIN
			INSERT INTO UserScores(total_games, total_wins, total_losses, game_FK, user_FK)
				VALUES(0, 0, 0, @_gameId, @_userId)
		END

	IF(@_userWon > 0)
		BEGIN
			UPDATE UserScores
				SET total_games = (SELECT total_games FROM UserScores WHERE game_FK = @_gameID AND user_FK = @_userId) + 1,
					 total_wins = (SELECT total_wins FROM UserScores WHERE game_FK = @_gameID AND user_FK = @_userId) + 1
				WHERE game_FK = @_gameId AND user_FK = @_userId
		END
	ELSE
		BEGIN
			UPDATE UserScores
				SET total_games = (SELECT total_games FROM UserScores WHERE game_FK = @_gameID AND user_FK = @_userId) + 1,
					 total_losses = (SELECT total_losses FROM UserScores WHERE game_FK = @_gameID AND user_FK = @_userId) + 1
				WHERE game_FK = @_gameID AND user_FK = @_userId
		END

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*UpdateUserBio*/
GO
CREATE PROCEDURE UpdateUserBio(@_userId INT, @_newBio TEXT)
AS
BEGIN
	BEGIN TRANSACTION

	UPDATE UserProfile
		SET bio = @_newBio
		WHERE UserId = @_userId

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END

/*AddExceptionLog*/
GO
CREATE PROCEDURE AddExceptionLog(@_exception_type VARCHAR(30), @_exception_message TEXT, @_name_of_function TEXT)
AS
BEGIN
	BEGIN TRANSACTION
	INSERT INTO ExceptionLogger(exception_type, exception_message, name_of_function, time_of_exception)
		VALUES(@_exception_type, @_exception_message, @_name_of_function, CURRENT_TIMESTAMP)

	IF @@ERROR <> 0
		BEGIN
			ROLLBACK
			RETURN
		END
	COMMIT
END