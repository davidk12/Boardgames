/*SelectAllAchievements*/
GO
CREATE PROCEDURE SelectAllAchievements
AS
BEGIN
	SELECT Achievements.ID as ID, Achievements.name as Achievement, Achievements.description, Achievements.game_FK
		FROM Achievements
END

/*SelectGameAchievements*/
GO
CREATE PROCEDURE SelectGameAchievements(@_gameId INT)
AS
BEGIN
	SELECT Achievements.ID as ID, Achievements.name as Achievement, Achievements.description, Achievements.game_FK
		FROM Achievements
		WHERE Achievements.game_FK = @_gameId
END

/*SelectUserAchievements*/
GO
CREATE PROCEDURE SelectUserAchievements(@_userId INT)
AS
BEGIN
	SELECT Achievements.ID as ID, Achievements.name as Achievement, Achievements.description, Achievements.game_FK
		FROM UserAchievements
		INNER JOIN Achievements
		ON
		UserAchievements.achievement_FK = Achievements.ID
		AND
		UserAchievements.user_FK = @_userId
		ORDER BY Achievements.ID
END

/*SelectAllGames*/
GO
CREATE PROCEDURE SelectAllGames
AS
BEGIN
	SELECT*
		FROM Games
END

/*SelectGame*/
GO
CREATE PROCEDURE SelectGame(@_gameId INT)
AS
BEGIN
	SELECT*
		FROM Games
		WHERE ID = @_gameId
END

/*SelectAllActiveGames*/
GO 
CREATE PROCEDURE SelectAllActiveGames
AS
BEGIN
	SELECT ActiveGames.ID, ActiveGames.name, ActiveGames.is_active, ActiveGames.is_in_lobby, ActiveGames.is_locked, ActiveGames.start_time, ActiveGames.game_FK, ActiveGames.owner_FK
		FROM UserProfile
		INNER JOIN ActiveGames
		ON
		UserProfile.UserId = ActiveGames.owner_FK
		INNER JOIN Games
		ON
		ActiveGames.game_FK = Games.ID
END

/*SelectActiveGames*/
GO
CREATE PROCEDURE SelectActiveGames(@_gameId INT)
AS
BEGIN
	SELECT ActiveGames.ID, ActiveGames.name, ActiveGames.is_active, ActiveGames.is_in_lobby, ActiveGames.is_locked, ActiveGames.start_time, ActiveGames.game_FK, ActiveGames.owner_FK
		FROM UserProfile
		INNER JOIN ActiveGames
		ON
		UserProfile.UserId = ActiveGames.owner_FK
		INNER JOIN Games
		ON
		ActiveGames.game_FK = Games.ID
		AND
		Games.ID = @_gameId
END

select*
	from ActiveGames

/*SelectActiveGame*/
GO
CREATE PROCEDURE SelectActiveGame(@_activeGameId INT)
AS
BEGIN
	SELECT ActiveGames.ID, ActiveGames.name, ActiveGames.is_active, ActiveGames.is_in_lobby, ActiveGames.is_locked, ActiveGames.start_time, ActiveGames.game_FK, ActiveGames.owner_FK
		FROM UserProfile
		INNER JOIN ActiveGames
		ON
		UserProfile.UserId = ActiveGames.owner_FK
		INNER JOIN Games
		ON
		ActiveGames.game_FK = Games.ID
		AND
		ActiveGames.ID = @_activeGameId
END

/*SelectAvailableSlots*/
GO
CREATE PROCEDURE SelectAvailableSlots(@_activeGameId INT)
AS
BEGIN
	SELECT (SELECT slots FROM Games WHERE ID = (SELECT game_FK FROM ActiveGames WHERE ID = @_activeGameId)) - COUNT(*) AS availableSlots
		FROM UsersInGame WHERE game_FK = @_activeGameId
END

/*SelectGameAnnouncements*/
GO
CREATE PROCEDURE SelectGameAnnouncements(@_gameId INT)
AS
BEGIN
	SELECT*
		FROM GameAnnouncements
		WHERE game_FK = @_gameId
END
/*SelectUser*/
GO
CREATE PROCEDURE SelectUser(@_userId INT)
AS
BEGIN
	SELECT *
		FROM UserProfile
		WHERE UserId = @_userId
END

/*SelectAllUsers*/
GO
CREATE PROCEDURE SelectAllUsers
AS
BEGIN
	SELECT*
		FROM UserProfile
END

/*SelectUsersInGame*/
GO
CREATE PROCEDURE SelectUsersInGame(@_gameId INT)
AS
BEGIN
	SELECT UserProfile.UserId, UserProfile.UserName, UserProfile.rating, UserProfile.bio
		FROM UserProfile
		INNER JOIN UsersInGame
		ON
		UserProfile.UserId = UsersInGame.user_FK
		AND
		UsersInGame.game_FK = @_gameId
END

/*SelectGameScores*/
GO
CREATE PROCEDURE SelectGameScores(@_gameId INT)
AS
BEGIN
	SELECT UserProfile.UserName, Games.name AS GameName, UserScores.total_games AS TotalPlayed, UserScores.total_wins AS TotalWins, UserScores.total_losses AS TotalLosses
	FROM UserProfile
	INNER JOIN UserScores
	ON
	UserProfile.UserId = UserScores.user_FK
	INNER JOIN Games
	ON
	UserScores.game_FK = Games.ID
	AND
	UserScores.game_FK = @_gameId
END

/*SelectUserScores*/
GO
CREATE PROCEDURE SelectUserScores(@_userId INT)
AS
BEGIN
	SELECT UserProfile.UserName, Games.name AS GameName, UserScores.total_games AS TotalPlayed, UserScores.total_wins AS TotalWins, UserScores.total_losses AS TotalLosses
	FROM UserProfile
	INNER JOIN UserScores
	ON
	UserProfile.UserId = UserScores.user_FK
	INNER JOIN Games
	ON
	UserScores.game_FK = Games.ID
	AND
	UserScores.user_FK = @_userId
END

/*SelectUserScoresInGame*/
GO
CREATE PROCEDURE SelectUserScoresInGame(@_userId INT, @_gameId INT)
AS
BEGIN
	SELECT UserProfile.UserName, Games.name AS GameName, UserScores.total_games AS TotalPlayed, UserScores.total_wins AS TotalWins, UserScores.total_losses AS TotalLosses
	FROM UserProfile
	INNER JOIN UserScores
	ON
	UserProfile.UserId = UserScores.user_FK
	INNER JOIN Games
	ON
	UserScores.game_FK = Games.ID
	AND
	UserScores.user_FK = @_userId
	AND
	UserScores.game_FK = @_gameId
END

/*SelectUsersGame*/
GO
CREATE PROCEDURE SelectUsersGame(@_userId INT)
AS
BEGIN
	SELECT ActiveGames.ID, ActiveGames.name, ActiveGames.is_active, ActiveGames.is_in_lobby, ActiveGames.is_locked, ActiveGames.start_time, ActiveGames.game_FK, ActiveGames.owner_FK
		FROM ActiveGames
		INNER JOIN UsersInGame
		ON
		ActiveGames.ID = UsersInGame.game_FK
		AND
		UsersInGame.user_FK = @_userId
END