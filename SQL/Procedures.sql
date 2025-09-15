USE RUBNS_Auth
GO
IF OBJECT_ID(N'p_GetLinks', N'P') IS NOT NULL
    DROP PROCEDURE p_GetLinks;
GO
CREATE PROCEDURE p_GetLinks @page int, @rows int, @filter NVARCHAR(10)
AS
BEGIN
	SET LANGUAGE 'SPANISH';
	DECLARE @Statuses TABLE (Value BIT);
	IF @filter = 'all'
	BEGIN
		INSERT INTO @Statuses (Value)
		VALUES (0), (1);
	END
	ELSE IF @filter = 'false'
	BEGIN
		INSERT INTO @Statuses (Value)
		VALUES (0);
	END
	ELSE IF @filter = 'true'
	BEGIN
		INSERT INTO @Statuses (Value)
		VALUES (1);
	END

	if(@page = 0)
	BEGIN
		SET @page = 1;
	END

	SELECT 
		link.ID,
		link.Name,
		link.Slug,
		link.Content,
		link.Url,
		qr.ColorDark,
		qr.ColorLight,
		qr.DotScale,
		qr.QuietZone,
		link.Status,
		usrRegisted.Name as 'userRegisted',
		usrmodified.Name as 'userModified',
		count(clic.id) as 'Clicks',
		link.Registered,
		link.LastModificated
	FROM Links as link
	LEFT JOIN Clicks as clic on link.ID = clic.LinkId
	INNER JOIN QR as qr on qr.LinkId = link.ID
	INNER JOIN  Users as usrRegisted on usrRegisted.UserID = link.UserID
	INNER JOIN  Users as usrmodified on usrmodified.UserID = link.LastUserID
	WHERE link.Status in(SELECT Value FROM @Statuses)
	group by 
		link.ID,
		link.Name,
		link.Slug,
		link.Content,
		link.Url,
		qr.ColorDark,
		qr.ColorLight,
		qr.DotScale,
		qr.QuietZone,
		link.Status,
		link.Registered,
		link.LastModificated,
		usrmodified.Name,
		usrRegisted.Name
	order by link.Registered desc
	OFFSET(@page - 1 ) * @rows ROWS
	FETCH NEXT @rows ROWS ONLY
END
GO
GO
IF OBJECT_ID(N'p_CountLinks', N'P') IS NOT NULL
    DROP PROCEDURE p_CountLinks;
GO
CREATE PROCEDURE p_CountLinks @filter NVARCHAR(10)
AS
BEGIN
	SET LANGUAGE 'SPANISH';
	DECLARE @Statuses TABLE (Value BIT);
	IF @filter = 'all'
	BEGIN
		INSERT INTO @Statuses (Value)
		VALUES (0), (1);
	END
	ELSE IF @filter = 'false'
	BEGIN
		INSERT INTO @Statuses (Value)
		VALUES (0);
	END
	ELSE IF @filter = 'true'
	BEGIN
		INSERT INTO @Statuses (Value)
		VALUES (1);
	END

	SELECT 
		COUNT(link.ID)
	FROM Links as link
	LEFT JOIN Clicks as clic on link.ID = clic.LinkId
	WHERE link.Status in(SELECT Value FROM @Statuses)
END
GO
IF OBJECT_ID(N'p_GetLink', N'P') IS NOT NULL
    DROP PROCEDURE p_GetLink;
GO
CREATE PROCEDURE p_GetLink @slug NVARCHAR(250)
AS
BEGIN
	SET LANGUAGE 'SPANISH';
	
	SELECT 
		link.ID,
		link.Name,
		link.Slug,
		link.Content,
		link.Url,
		link.ColorDark,
		link.ColorLight,
		link.DotScale,
		link.QuietZone,
		link.Status,
		usrRegisted.Name as 'userRegisted',
		usrmodified.Name as 'userModified',
		count(clic.id) as 'Clicks'
	FROM Links as link
	LEFT JOIN Clicks as clic on link.ID = clic.LinkId
	INNER JOIN  Users as usrRegisted on usrRegisted.UserID = link.UserID
	INNER JOIN  Users as usrmodified on usrmodified.UserID = link.LastUserID
	WHERE link.Slug = @slug
	group by 
		link.ID,
		link.Name,
		link.Slug,
		link.Content,
		link.Url,
		link.ColorDark,
		link.ColorLight,
		link.DotScale,
		link.QuietZone,
		link.Status,
		link.Registered,
		link.LastModificated,
		usrmodified.Name,
		usrRegisted.Name;
END
GO
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
goGO
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