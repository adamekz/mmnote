----------------------------------------------------------------------------------------------------
-- DATABASE

DROP DATABASE mmnote_db
go

CREATE DATABASE mmnote_db
go

----------------------------------------------------------------------------------------------------
--TYPY DANYCH
sp_addtype string, 'varchar(50)'
go

sp_addtype longstring, 'varchar(100)'
go


----------------------------------------------------------------------------------------------------
-- TABLE

DROP TABLE files
DROP TABLE actions
DROP TABLE friends
DROP TABLE users
go



CREATE TABLE users (
u_id int NOT NULL PRIMARY KEY IDENTITY,
login string NOT NULL,
first_name string NOT NULL,
last_name string NOT NULL, 
password string NOT NULL,
email longstring NOT NULL,
join_date DATE NOT NULL DEFAULT GETDATE(),
   
);

CREATE TABLE files (
f_id int NOT NULL PRIMARY KEY IDENTITY,
u_id int NOT NULL FOREIGN KEY REFERENCES users(u_id),
name string NOT NULL,
path longstring NOT NULL,
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
type string NOT NULL,
f_id int FOREIGN KEY REFERENCES files(f_id),
action_time DATETIME NOT NULL DEFAULT GETDATE()

);

GO



---------------------------------------------------------------------------------------------
--TRIGGERS

--sprawdzanie przy insercie czy urzytkownik o danym loginie ju¿ istnieje

CREATE TRIGGER tr_users_INSERT ON users
FOR INSERT 
AS
IF (SELECT count(*)
    FROM users AS U,  inserted as I
        WHERE I.login = U.login) > 1
    BEGIN
        PRINT 'Istnieje ju¿ u¿ytkownik o takim loginie.';
        ROLLBACK TRANSACTION;
    END;
GO

DROP TRIGGER tr_users_INSERT
GO

--test
INSERT INTO users (login, first_name, last_name, password, email)
VALUES (3,123,123,123,123)
GO

select * from users
go


--sprawdzanie czy dany urzytkownik ma ju¿ plik o danej nazwie
CREATE TRIGGER tr_file_INSERT ON files
FOR INSERT 
AS
IF (SELECT count(*)
    FROM files AS F, inserted as I
        WHERE I.name = F.name AND F.u_id = I.u_id) > 1
    BEGIN
        PRINT 'Istnieje ju¿ plik o tej nazwie.';
        ROLLBACK TRANSACTION;
    END;
GO

DROP TRIGGER tr_file_INSERT
GO

select * from files
go

--test
INSERT into files(u_id, name, path)
VALUES (3, 'nazwa', 'C://wd//w')
go

