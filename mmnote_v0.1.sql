CREATE DATABASE mmnote_db
GO

/*
TABELE
*/

CREATE TABLE users (
u_id int NOT NULL PRIMARY KEY IDENTITY,
login varchar(50) NOT NULL,
first_name varchar(50) NOT NULL,
last_name varchar(50) NOT NULL, 
password varchar(64) NOT NULL,
email varchar(100) NOT NULL,
join_date DATE NOT NULL DEFAULT GETDATE(),
   
);

CREATE TABLE files (
f_id int NOT NULL PRIMARY KEY IDENTITY,
u_id int NOT NULL FOREIGN KEY REFERENCES users(u_id),
path varchar(100) NOT NULL,
add_time DATETIME NOT NULL DEFAULT GETDATE()

);

CREATE TABLE friends (
friendship_id int NOT NULL PRIMARY KEY IDENTITY,
u1_id int NOT NULL FOREIGN KEY REFERENCES users(u_id),
u2_id int NOT NULL FOREIGN KEY REFERENCES users(u_id),
add_time DATETIME NOT NULL DEFAULT GETDATE()

);


CREATE TABLE actions (
a_id int NOT NULL PRIMARY KEY IDENTITY,
u_id int NOT NULL FOREIGN KEY REFERENCES users(u_id),
type varchar(50) NOT NULL
f_id int FOREIGN KEY REFERENCES files(f_id),
action_time DATETIME NOT NULL DEFAULT GETDATE()

);

GO


DROP TABLE files
DROP TABLE actions
DROP TABLE friends
DROP TABLE users
GO


/*
TRIGGERS
*/

/*
CREATE TRIGGER tr_users_INSERT
ON users
FOR INSERT
AS
IF EXISTS (SELECT login FROM users = SELECT login FROM inserted)
 BEGIN
  PRINT 'Istnieje ju¿ u¿ytkownik o takim loginie'
 END
*/

CREATE TRIGGER tr_users_INSERT ON users
FOR INSERT 
AS
IF EXISTS
    (SELECT U.login
    FROM users AS U
        JOIN inserted as I
            ON U.login = I.login)
    BEGIN
        PRINT 'Istnieje ju¿ u¿ytkownik o takim loginie.';
        ROLLBACK TRANSACTION;
    END;
GO

DROP TRIGGER tr_users_INSERT
GO


INSERT INTO users (login, first_name, last_name, password, email)
VALUES (12334334,123,123,123,123)
GO

select * from users
go
