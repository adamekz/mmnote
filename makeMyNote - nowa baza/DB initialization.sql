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


----------------------------------------------------------------------------------------------------
-- TABLE

DROP TABLE files
DROP TABLE friends
DROP TABLE users
go



CREATE TABLE users (
u_id int NOT NULL PRIMARY KEY IDENTITY,
login string NOT NULL,
first_name string NOT NULL,
last_name string NOT NULL, 
email string NOT NULL,
join_date DATE NOT NULL DEFAULT GETDATE(),
   
);

CREATE TABLE files (
f_id int NOT NULL PRIMARY KEY IDENTITY,
u_id int NOT NULL FOREIGN KEY REFERENCES users(u_id),
name string NOT NULL,
path varchar(max) NOT NULL,
type varchar(10) NOT NULL check(type in ('AUDIO','IMAGE')),
content varchar(max),
add_time DATETIME NOT NULL DEFAULT GETDATE()

);

CREATE TABLE friends (
friendship_id int NOT NULL PRIMARY KEY IDENTITY,
u1_id int NOT NULL FOREIGN KEY REFERENCES users(u_id),
u2_id int NOT NULL FOREIGN KEY REFERENCES users(u_id),
add_time DATETIME NOT NULL DEFAULT GETDATE()
);


GO



---------------------------------------------------------------------------------------------
--TRIGGERS

--sprawdzanie przy insercie czy u¿ytkownik o danym loginie ju¿ istnieje

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
VALUES (6,123,123,123,123)
GO

select * from users
go


--sprawdzanie czy dany u¿ytkownik ma ju¿ plik o danej nazwie
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
VALUES (1, 'nazwa', 'C://wd//w')
go


--sprawdzanie czy dana znajomoœæ ju¿ istnieje
CREATE TRIGGER tr_friends_INSERT ON friends
FOR INSERT 
AS
IF (SELECT count(*)
    FROM friends AS F, inserted as I
        WHERE (F.u1_id = I.u1_id AND F.u2_id = I.u2_id) OR (F.u2_id = I.u1_id AND F.u1_id = I.u2_id)) > 1
    BEGIN
        PRINT 'Istnieje ju¿ taka przyjaŸñ';
        ROLLBACK TRANSACTION;
    END;
GO

DROP TRIGGER tr_friends_INSERT
GO

--test
INSERT into friends(u1_id, u2_id)
VALUES (3,6)
go

select * from friends
go


--Blokada dodania samego siebie do listy przyjació³
CREATE TRIGGER tr_self_friend_INSERT ON friends
FOR INSERT 
AS
IF exists (select * from Inserted where u1_id = u2_id)
    BEGIN
        PRINT 'Nie mo¿esz zawrzeæ znajomoœci ze sob¹.';
        ROLLBACK TRANSACTION;
    END;
GO

DROP TRIGGER tr_self_friend_INSERT
GO


--test

INSERT into friends(u1_id, u2_id)
VALUES (3,3)
go





-----------------------------------------------------------------------
--PROCEDURY


delete from files where f_id=10



-----------------------------------------------------------------------
--reset zawartoœci bazy

delete from actions
go
delete from friends
go
delete from files
go
delete from users
go

