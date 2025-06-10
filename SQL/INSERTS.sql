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
INSERT INTO Users VALUES('Rubén','rubns@test.com','41a59384d89b017f47a7edd702baee5696dbfad3b2aeaf1676df040e85c1d731',1)