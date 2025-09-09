USE RUBNS_Auth
GO
--DeleteTokensExpiredResetPassword
BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @DeletedRows INT = 0;

    -- 1. Eliminar registros antiguos (por tiempo de vida)
    DELETE FROM ResetPasswords
    WHERE Registed <= DATEADD(MINUTE, -16, GETDATE());

    SET @DeletedRows = @DeletedRows + @@ROWCOUNT;

    -- Registrar ejecución
    INSERT INTO JobLogs (JobName, DeleteRows)
    VALUES ('DeleteTokensExpiredResetPassword', @DeletedRows);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;

    -- Registrar error
    INSERT INTO JobLogs (JobName, MessageError)
    VALUES ('DeleteTokensExpiredResetPassword', ERROR_MESSAGE());
END CATCH;
GO
--DeleteRefreskTokensExpired
BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @DeletedRows INT = 0;

    -- 1. Eliminar registros antiguos (por tiempo de vida)
    DELETE FROM SessionUsers
    WHERE Expiration < GETDATE();

    SET @DeletedRows = @DeletedRows + @@ROWCOUNT;

    -- Registrar ejecución
    INSERT INTO JobLogs (JobName, DeleteRows)
    VALUES ('DeleteRefreskTokensExpired', @DeletedRows);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;

    -- Registrar error
    INSERT INTO JobLogs (JobName, MessageError)
    VALUES ('DeleteRefreskTokensExpired', ERROR_MESSAGE());
END CATCH;
GO