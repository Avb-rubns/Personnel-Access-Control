USE[RUBNS_AUTH]
GO
INSERT INTO ROLS(Name,Value,LevelPermission,Status)
VALUES('Root','root',0,1)
GO
INSERT INTO ROLS(Name,Value,LevelPermission,Status)
VALUES('Admin','Administrador',1,1)
GO
INSERT INTO ROLS(Name,Value,LevelPermission,Status)
VALUES('Edit','Editar',2,1)
GO
INSERT INTO ROLS(Name,Value,LevelPermission,Status)
VALUES('User','usuario',2,1)
GO
INSERT INTO Users(Name,Email,Password,Phone,RolId,Status) VALUES('Rubén','rubns@test.com','41a59384d89b017f47a7edd702baee5696dbfad3b2aeaf1676df040e85c1d731','2211538571',1,1)
GO
INSERT INTO Templates(Name,Value) VALUES('RegisterMail','<!DOCTYPE html>
<html lang="es">

<head>
    <meta charset="UTF-8" />
    <title>Confirmación de Registro</title>
    <style>
        body {
            font-family: ''Roboto'', sans-serif;
            background-color: #f2f2f2;
            margin: 0;
            padding: 0;
            color: #333333;
        }

        .container {
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            padding: 24px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        }

        h1 {
            font-size: 24px;
            font-weight: 700;
            margin-bottom: 12px;
            color: #004aad;
        }

        p {
            font-size: 16px;
            line-height: 1.6;
            margin: 8px 0;
        }

        .highlight-box {
            background-color: #f5f5f5;
            padding: 16px;
            border-radius: 6px;
            margin-top: 20px;
        }

        .footer {
            text-align: center;
            font-size: 12px;
            color: #999999;
            margin-top: 30px;
        }
    </style>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet" />
</head>

<body>
    <div class="container">
        <h1>¡Hola @user!</h1>
        <p>Tu registro fue exitoso.</p>
        <p><strong>Fecha de registro:</strong> @day, @date</p>

        <div class="highlight-box">
            <p><strong>Accesos de usuario:</strong></p>
            <p><strong>Correo:</strong> @mail</p>
            <p><strong>Contraseña:</strong> @password</p>
        </div>

        <div class="footer">
            <p>Este mensaje fue enviado automáticamente. Por favor, no respondas a este correo.</p>
        </div>
    </div>
</body>

</html>')


INSERT INTO Templates(Name,Value) VALUES('ForgotPasswordMail','<!DOCTYPE html>
<html lang="es">

<head>
    <meta charset="UTF-8" />
    <title>Confirmación de Registro</title>
    <style>
        body {
            font-family: ''Roboto'', sans-serif;
            background-color: #f2f2f2;
            margin: 0;
            padding: 0;
            color: #333333;
        }

        .container {
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            padding: 24px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        }

        h1 {
            font-size: 24px;
            font-weight: 700;
            margin-bottom: 12px;
            color: #004aad;
        }

        p {
            font-size: 16px;
            line-height: 1.6;
            margin: 8px 0;
        }

        .highlight-box {
            background-color: #f5f5f5;
            padding: 16px;
            border-radius: 6px;
            margin-top: 20px;
        }

        .footer {
            text-align: center;
            font-size: 12px;
            color: #999999;
            margin-top: 30px;
        }
    </style>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet" />
</head>

<body>
    <div class="container">
        <h1>¡Hola @user!</h1>
        <div class="highlight-box">
            <p>Recibimos tu solicitud para crear una nueva contraseña.</p>
            <p>Restablécela <a href="@link">Aquí</a></p>
            <p>Este correo es valido por 15 minutos</p>
        </div>

        <div class="footer">
            <p>Este mensaje fue enviado automáticamente. Por favor, no respondas a este correo.</p>
        </div>
    </div>
</body>

</html>')

GO
SET IDENTITY_INSERT dbo.CheckPersonal ON
INSERT INTO dbo.CheckPersonal(CheckPersonalID,UserID,LatitudeCheckIn,LongitudeCheckIn,LatitudeCheckOut,LongitudeCheckOut,IP)VALUES(0,0,19.0568929,-98.2146622,19.0568929,-98.2146622,'::1')
SET IDENTITY_INSERT dbo.CheckPersonal  OFF