USE[RUBNS_AUTH]
GO
IF NOT EXISTS (
	SELECT 1
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Users')
BEGIN
CREATE TABLE Users
(
	UserID INT IDENTITY (1,1) NOT NULL CONSTRAINT PK_USERS_ID PRIMARY KEY CLUSTERED (UserID)
	,Name NVARCHAR(100) NOT NULL COLLATE Latin1_General_CI_AI
	,Email NVARCHAR(100) NOT NULL COLLATE Latin1_General_CI_AI
	,Password NVARCHAR(MAX) NOT NULL
	,Phone NVARCHAR(50) NOT NULL COLLATE Latin1_General_CI_AI
	,RolID INT NULL DEFAULT (3)
	,Status bit null
	,Registed DATETIME NOT NULL DEFAULT (SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)')
)
END
GO
IF NOT EXISTS (
	SELECT 1
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'CheckPersonal')
BEGIN
CREATE TABLE CheckPersonal
(
	CheckPersonalID INT IDENTITY (1,1) NOT NULL CONSTRAINT PK_CheckPersonal_ID PRIMARY KEY CLUSTERED (CheckPersonalID)
	,UserID INT NOT NULL
	,LatitudeCheckIn DECIMAL(11,8) NULL
	,LongitudeCheckIn DECIMAL(11,8) NULL
	,LatitudeCheckOut DECIMAL(11,8) NULL
	,LongitudeCheckOut DECIMAL(11,8) NULL
	,IP NVARCHAR(100) NULL
	,RegistedCheckIn DATETIME NOT NULL DEFAULT (SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)')
	,RegistedCheckOut DATETIME 
)
END
GO
IF NOT EXISTS (
	SELECT 1
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Rols')
BEGIN
CREATE TABLE Rols
(
	RolID INT IDENTITY (1,1) NOT NULL CONSTRAINT PK_Rols_ID PRIMARY KEY CLUSTERED (RolID)
	,Name NVARCHAR(100) NOT NULL COLLATE Latin1_General_CI_AI
	,Value NVARCHAR(100) NOT NULL
	,LevelPermission INT NOT NULL
	,Status BIT 
	,Registed DATETIME NOT NULL DEFAULT (SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)')
)
END
GO
IF NOT EXISTS (
	SELECT 1
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Templates')
BEGIN
CREATE TABLE Templates
(
	TemplateID INT IDENTITY (1,1) NOT NULL CONSTRAINT PK_TEMPLATE_ID PRIMARY KEY CLUSTERED (TemplateID)
	,Name NVARCHAR(100) NOT NULL COLLATE Latin1_General_CI_AI
	,Value NVARCHAR(MAX) NOT NULL COLLATE Latin1_General_CI_AI
	,Registed DATETIME NOT NULL DEFAULT (SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)')
)
END
IF OBJECT_ID(N'p_UserByEmail', N'P') IS NOT NULL
    DROP PROCEDURE p_UserByEmail;
GO
CREATE PROCEDURE p_UserByEmail @email NVARCHAR(100)
AS
BEGIN
    SELECT
		usr.UserID
		,usr.Name as UserName
		,Email
		,Password
		,Value
		,LevelPermission
		,usr.Status
		,Phone
    FROM Users as usr
	INNER JOIN Rols as rol on usr.RolID = rol.RolID
    WHERE 
	usr.Email = @email
END
GO
IF OBJECT_ID(N'p_UserByPhone', N'P') IS NOT NULL
    DROP PROCEDURE p_UserByPhone;
GO
CREATE PROCEDURE p_UserByPhone @Phone NVARCHAR(100)
AS
BEGIN
    SELECT
		usr.UserID
		,usr.Name as UserName
		,Email
		,Password
		,Value
		,LevelPermission
		,usr.Status
		,Phone
    FROM Users as usr
	INNER JOIN Rols as rol on usr.RolID = rol.RolID
    WHERE 
	usr.Phone = @Phone
END
GO
IF NOT EXISTS (
	SELECT 1
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'SessionUsers')
BEGIN
CREATE TABLE SessionUsers
(
	 ID INT IDENTITY (1,1) NOT NULL CONSTRAINT PK_SessionUsers_ID PRIMARY KEY CLUSTERED (ID)
	,UserID INT NOT NULL
	,Token NVARCHAR(MAX) NOT NULL
	,Expiration  DATETIME NOT NULL
	,Registed DATETIME NOT NULL DEFAULT (SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)')
)
END
GO
IF OBJECT_ID(N'p_DeleteSessionUser', N'P') IS NOT NULL
    DROP PROCEDURE p_DeleteSessionUser;
GO
CREATE PROCEDURE p_DeleteSessionUser @Token NVARCHAR(100)
AS
BEGIN
    BEGIN TRY
	    
		SET XACT_ABORT ON;
		BEGIN TRANSACTION;
		DECLARE @Result INT = 0;

		DELETE FROM SessionUsers WHERE Token = @Token;
		COMMIT TRANSACTION;

		 SET @Result = 1;

	END TRY
	 BEGIN CATCH
        ROLLBACK TRANSACTION;
		SET @Result = -3;
    END CATCH

	SELECT @Result AS Result;
END
GO
IF OBJECT_ID(N'p_UserByID', N'P') IS NOT NULL
    DROP PROCEDURE p_UserByID;
GO
CREATE PROCEDURE p_UserByID @ID INT
AS
BEGIN
    SELECT
		usr.UserID
		,usr.Name as UserName
		,Email
		,Password
		,Value
		,LevelPermission
		,usr.Status
    FROM Users as usr
	INNER JOIN Rols as rol on usr.RolID = rol.RolID
    WHERE 
	usr.UserID = @ID;
END
GO
IF OBJECT_ID(N'p_InsertCheckInPersonal', N'P') IS NOT NULL
    DROP PROCEDURE p_InsertCheckInPersonal;
GO
CREATE PROCEDURE [dbo].[p_InsertCheckInPersonal]
    @UserID INT,
    @Latitude FLOAT,
    @Longitude FLOAT,
    @IP NVARCHAR(200)
AS
BEGIN
    BEGIN TRY
        SET XACT_ABORT ON;
        BEGIN TRANSACTION;
		DECLARE @Result INT = 0;
        INSERT INTO CheckPersonal(UserID, LatitudeCheckIN, LongitudeCheckIn, IP)
        VALUES (@UserID, @Latitude, @Longitude, @Ip);

        COMMIT TRANSACTION;
        SET @Result = 1;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Result = -3;
    END CATCH

    SELECT @Result AS Result;
END
GO
IF OBJECT_ID(N'p_CheckUserToday', N'P') IS NOT NULL
    DROP PROCEDURE p_CheckUserToday;
GO
CREATE PROCEDURE [dbo].[p_CheckUserToday]
AS
BEGIN
	SELECT 
		checkUser.RegistedCheckIn as 'HourCheckIn'
		,usr.Name as 'Name' 
		,rol.Name as 'Rol'
		,CAST(ROUND(dbo.DistanceMts(checkUser.LatitudeCheckIn,checkUser.LongitudeCheckIn),2) as nvarchar(50)) as 'DistanceCheckIn',
		CASE 
			WHEN (dbo.DistanceMts(checkUser.LatitudeCheckIn,checkUser.LongitudeCheckIn)) <= 100 THEN 'Está dentro del área permitida.'
			ELSE 'Fuera del área permitida.' 
		END as 'AccessIn'
		,ISNULL(checkUser.RegistedCheckOut, '1900-01-01') as 'HourCheckOut'
		,CASE
			WHEN
				checkUser.LatitudeCheckOut is not null 
			THEN 
				CAST(ROUND(dbo.DistanceMts(checkUser.LatitudeCheckOut,checkUser.LongitudeCheckOut),2) as nvarchar(50)) 
			ELSE 'Sin realizar registro'
		END as 'DistanceCheckOut'
		,CASE 
			WHEN checkUser.LatitudeCheckOut IS NOT NULL 
			THEN 
				CASE WHEN (dbo.DistanceMts(checkUser.LatitudeCheckOut,checkUser.LongitudeCheckOut)) <= 100 
				THEN 'Está dentro del área permitida.'
				ELSE 'Fuera del área permitida.' 
				END
			ELSE 'Sin realizar registro' 
		END as 'AccessOut'
	FROM  [dbo].[CheckPersonal]  as checkUser
	INNER JOIN Users as usr on checkUser.UserID = usr.UserID
	INNER JOIN Rols as rol on rol.RolID = usr.RolID
	WHERE CAST(checkUser.RegistedCheckIn as DATE) = CAST(GETDATE() as DATE) AND checkUser.CheckPersonalID > 0;
END


GO
IF NOT EXISTS(SELECT 1
FROM sys.database_principals dp
LEFT JOIN sys.database_permissions dpr ON dp.principal_id = dpr.grantee_principal_id
WHERE dp.name = 'auth')
BEGIN
	CREATE LOGIN [auth] 
	WITH PASSWORD = 'Temporal1',
		 CHECK_POLICY = OFF;  

	CREATE USER [auth] FOR LOGIN [auth];

	-- Otorgar roles básicos (lectura y escritura)
	ALTER ROLE db_datareader ADD MEMBER [auth];
	ALTER ROLE db_datawriter ADD MEMBER [auth];
	ALTER ROLE db_ddladmin ADD MEMBER [auth];
	GRANT EXECUTE TO [auth];

	-- (Opcional) Si quieres que tenga control total dentro de la base de datos:
	-- ALTER ROLE db_owner ADD MEMBER [auth];

END

GO
IF OBJECT_ID(N'p_CreateMailRegister', N'P') IS NOT NULL
    DROP PROCEDURE p_CreateMailRegister;
GO
CREATE PROCEDURE p_CreateMailRegister @Name NVARCHAR(20), @Mail NVARCHAR(20), @Password NVARCHAR(20)
AS
BEGIN
	SET LANGUAGE 'SPANISH';
    DECLARE @template NVARCHAR(MAX);
	SELECT @template = Value FROM Templates WHERE Name = 'RegisterMail';

	SELECT FinalResult
	FROM (
		SELECT REPLACE(@template, '@user', @Name) AS Step1) AS s1
		CROSS APPLY (SELECT REPLACE(Step1, '@mail', @mail) AS Step2) AS s2
		CROSS APPLY (SELECT REPLACE(Step2, '@password', @Password) AS Step3) AS s3
		CROSS APPLY (
			SELECT REPLACE(
				Step3,
				'@date',
				CONCAT(
					DAY(GETDATE()), ' de ',
					DATENAME(MONTH, GETDATE()), ' de ',
					YEAR(GETDATE()), ' a las ',
					FORMAT(GETDATE(), 'hh:mm tt', 'es-MX')
				)
			) AS Step4
		) AS s4
		CROSS APPLY( SELECT REPLACE(step4, '@day',DATENAME(WEEKDAY, GETDATE())) as FinalResult) as s5;
END
GO
IF NOT EXISTS (
	SELECT 1
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'ResetPasswords')
BEGIN
CREATE TABLE ResetPasswords
(
	ResetPasswordID INT IDENTITY (1,1) NOT NULL CONSTRAINT PK_RESETPASSWORD_ID PRIMARY KEY CLUSTERED (ResetPasswordID)
	,UserID INT 
	,Token NVARCHAR(100)
	,Registed DATETIME NOT NULL DEFAULT (SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)')
	CONSTRAINT FK_USERS FOREIGN KEY (UserID)
    REFERENCES Users(UserID)
)
END
GO
IF OBJECT_ID(N'p_CreateMailForgotPassword', N'P') IS NOT NULL
    DROP PROCEDURE p_CreateMailForgotPassword;
GO
CREATE PROCEDURE p_CreateMailForgotPassword @Name NVARCHAR(20), @Link NVARCHAR(256)
AS
BEGIN
	SET LANGUAGE 'SPANISH';
    DECLARE @template NVARCHAR(MAX);
	SELECT @template = Value FROM Templates WHERE Name = 'ForgotPasswordMail';

	SELECT FinalResult
	FROM (
		SELECT REPLACE(@template, '@user', @Name) AS Step1) AS s1
		CROSS APPLY (SELECT REPLACE(Step1, '@link', @Link) AS FinalResult) AS s2

END
GO
IF OBJECT_ID(N'p_DeleteSessionUserforUserID', N'P') IS NOT NULL
    DROP PROCEDURE p_DeleteSessionUserforUserID;
GO
CREATE PROCEDURE p_DeleteSessionUserforUserID @UserID INT
AS
BEGIN
    BEGIN TRY
	    
		SET XACT_ABORT ON;
		BEGIN TRANSACTION;
		DECLARE @Result INT = 0;

		DELETE FROM SessionUsers WHERE UserID = @UserID;
		COMMIT TRANSACTION;

		 SET @Result = 1;

	END TRY
	 BEGIN CATCH
        ROLLBACK TRANSACTION;
		SET @Result = -3;
    END CATCH

	SELECT @Result AS Result;
END
GO
IF OBJECT_ID(N'p_UpdateUserPasswordforUserID', N'P') IS NOT NULL
    DROP PROCEDURE p_UpdateUserPasswordforUserID;
GO
CREATE PROCEDURE p_UpdateUserPasswordforUserID @UserID INT, @NewPassword NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
	    
		SET XACT_ABORT ON;
		BEGIN TRANSACTION;
		DECLARE @Result INT = 0;

		UPDATE Users SET Password = @NewPassword WHERE UserID = @UserID;
		COMMIT TRANSACTION;

		 SET @Result = 1;

	END TRY
	 BEGIN CATCH
        ROLLBACK TRANSACTION;
		SET @Result = -3;
    END CATCH

	SELECT @Result AS Result;
END
GO
CREATE or ALTER FUNCTION dbo.DistanceMts (@latCheck FLOAT, @lonCheck FLOAT)
RETURNS FLOAT
AS
BEGIN
	DECLARE @RadioTierra FLOAT = 6371000; -- en metros
DECLARE @latHomeRad FLOAT, @lonHomeRad FLOAT;
DECLARE @latCheckRad FLOAT, @lonCheckRad FLOAT;
DECLARE @deltaLat FLOAT, @deltaLon FLOAT;
DECLARE @a FLOAT, @c FLOAT, @distanciaMetros FLOAT;

-- Coordenadas del punto base
SELECT 
	@latHomeRad = RADIANS(ch.LatitudeCheckIn),
	@lonHomeRad = RADIANS(ch.LongitudeCheckIn)
FROM dbo.CheckPersonal AS ch
WHERE ch.CheckPersonalID = 0;

-- Coordenadas del punto a comprobar

SET	@latCheckRad = RADIANS(@latCheck);
SET	@lonCheckRad = RADIANS(@lonCheck)


-- Diferencias
SET @deltaLat = @latCheckRad - @latHomeRad;
SET @deltaLon = @lonCheckRad - @lonHomeRad;

-- Fórmula de Haversine
SET @a = POWER(SIN(@deltaLat / 2), 2) + COS(@latHomeRad) * COS(@latCheckRad) * POWER(SIN(@deltaLon / 2), 2);
SET @c = 2 * ASIN(SQRT(@a));
SET @distanciaMetros = @RadioTierra * @c;

-- Resultado
RETURN @distanciaMetros
END
GO
IF OBJECT_ID(N'p_InsertCheckOutPersonal', N'P') IS NOT NULL
    DROP PROCEDURE p_InsertCheckOutPersonal;
GO
CREATE PROCEDURE [dbo].[p_InsertCheckOutPersonal]
    @UserID INT,
    @Latitude FLOAT,
    @Longitude FLOAT
AS
BEGIN
    BEGIN TRY
        SET XACT_ABORT ON;
		DECLARE @LastCheckID  INT;
		WITH LastCheckIn AS (
		SELECT 
		*,
		ROW_NUMBER() OVER (ORDER BY RegistedCheckIn DESC) as RowNum
		FROM CheckPersonal WHERE UserID = @UserID)
		SELECT 
		@LastCheckID = LastCheckIn.CheckPersonalID
		FROM LastCheckIn WHERE  RowNum = 1 

        BEGIN TRANSACTION;
		DECLARE @Result INT = 0;
        UPDATE dbo.CheckPersonal 
			SET LatitudeCheckOut = @Latitude,
				LongitudeCheckOut = @Longitude,
				RegistedCheckOut =  (SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)')
		WHERE CheckPersonalID = @LastCheckID

        COMMIT TRANSACTION;
        SET @Result = 1;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Result = -3;
    END CATCH

    SELECT @Result AS Result;
END