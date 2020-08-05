--Creating database only if database is not created yet
IF DB_ID('Zadatak_50') IS NULL
CREATE DATABASE Zadatak_50
GO
USE Zadatak_50;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblSongs')
drop table tblSongs;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblUser')
drop table tblUser;


create table tblSongs (
SongID int identity(1,1) primary key,
Title nvarchar (50) not null ,
Author nvarchar (50) not null,
Duration_s int ,
Logged bit
)

create table tblUsers(
UserID int identity(1,1) primary key,
Username nvarchar(50) unique not null,
Pasword nvarchar (50) not null
)








