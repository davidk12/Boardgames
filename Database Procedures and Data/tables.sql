/*
	DROP TABLE UsersInGame
	DROP TABLE UserScores
	DROP TABLE ActiveGames
	DROP TABLE ExceptionLogger
	DROP TABLE GameAnnouncements
	DROP TABLE RatingLog	
	DROP TABLE UserAchievements
	DROP TABLE UsersInGroups
	DROP TABLE UserGroups
	DROP TABLE Achievements
	DROP TABLE Games
*/

CREATE TABLE UserProfile
(
	UserId INT NOT NULL PRIMARY KEY IDENTITY(0,1),
	UserName nvarchar(max) NULL
)

ALTER TABLE UserProfile
ADD bio text

ALTER TABLE UserProfile
ADD rating int

ALTER TABLE UserProfile
ADD CONSTRAINT UserProfile_Unique UNIQUE(UserName)

CREATE TABLE Games
(
	ID INT NOT NULL PRIMARY KEY IDENTITY(0,1),
	name VARCHAR(20),
	game_description TEXT,
	rating INT DEFAULT 0,
	slots INT
)

CREATE TABLE UserScores
(
	ID INT NOT NULL PRIMARY KEY IDENTITY(0,1),
	total_games INT,
	total_wins INT,
	total_losses INT,
	game_FK INT FOREIGN KEY REFERENCES Games(ID),
	user_FK INT FOREIGN KEY REFERENCES UserProfile(UserId),
	CONSTRAINT UserScores_Unique UNIQUE(game_FK, user_FK)
)

CREATE TABLE ActiveGames
(
	ID INT NOT NULL PRIMARY KEY IDENTITY(0,1),
	name varchar(20),
	game_FK INT FOREIGN KEY REFERENCES Games(ID),
	owner_FK INT FOREIGN KEY REFERENCES UserProfile(UserId),
	is_active BIT,
	is_in_lobby BIT,
	is_locked BIT,
	start_time DATETIME
)

CREATE TABLE UsersInGame
(
	user_FK INT FOREIGN KEY REFERENCES UserProfile(UserId),
	game_FK INT FOREIGN KEY REFERENCES ActiveGames(ID),
	CONSTRAINT UserInGame_Unique UNIQUE(user_FK, game_FK)
)

CREATE TABLE RatingLog
(
	ID INT NOT NULL PRIMARY KEY IDENTITY(0,1),
	user_FK INT FOREIGN KEY REFERENCES UserProfile(UserId),
	rater_FK INT FOREIGN KEY REFERENCES UserProfile(UserId),
	game_FK INT FOREIGN KEY REFERENCES Games(ID)
)

CREATE TABLE Achievements
(
	ID INT NOT NULL PRIMARY KEY IDENTITY(0,1),
	name VARCHAR(20),
	description TEXT,
	game_FK INT FOREIGN KEY REFERENCES Games(ID)
)

CREATE TABLE UserAchievements
(
	user_FK INT FOREIGN KEY REFERENCES UserProfile(UserId),
	achievement_FK INT FOREIGN KEY REFERENCES Achievements(ID)
	CONSTRAINT UserAchievements_Unique UNIQUE(user_FK, achievement_FK)
)

CREATE TABLE UserGroups
(
	ID INT NOT NULL PRIMARY KEY IDENTITY(0,1),
	name VARCHAR(20)
)

CREATE TABLE UsersInGroups
(
	group_FK INT FOREIGN KEY REFERENCES UserGroups(ID),
	user_FK INT FOREIGN KEY REFERENCES UserProfile(UserId),
	CONSTRAINT UsersInGroups_Unique UNIQUE(group_fk, user_FK)
)

CREATE TABLE GameAnnouncements
(
	ID INT NOT NULL PRIMARY KEY IDENTITY(0,1),
	title VARCHAR(50),
	body TEXT,
	time_stamp DATETIME,
	game_FK INT FOREIGN KEY REFERENCES Games(ID)
)

CREATE TABLE ExceptionLogger
(
	ID INT NOT NULL PRIMARY KEY IDENTITY(0,1),
	exception_type VARCHAR(30),
	exception_message TEXT,
	name_of_function TEXT,
	time_of_exception DATETIME
)