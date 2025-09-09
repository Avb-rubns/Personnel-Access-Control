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
	WHERE link.Status in(SELECT Value FROM @Statuses)
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

	if(@page = 0)
	BEGIN
		SET @page = 1;
	END

	SELECT 
		COUNT(link.ID)
	FROM Links as link
	LEFT JOIN Clicks as clic on link.ID = clic.LinkId
	WHERE link.Status in(SELECT Value FROM @Statuses)
END

