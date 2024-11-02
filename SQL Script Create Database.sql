USE [master]
GO
/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Book')
BEGIN
	ALTER DATABASE Book SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE Book SET ONLINE;
	DROP DATABASE Book;
END

GO
CREATE DATABASE Book
GO
USE Book
GO
/*******************************************************************************
	Drop tables if exists
*******************************************************************************/
DECLARE @sql nvarchar(MAX) 
SET @sql = N'' 

SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(KCU1.TABLE_SCHEMA) 
    + N'.' + QUOTENAME(KCU1.TABLE_NAME) 
    + N' DROP CONSTRAINT ' -- + QUOTENAME(rc.CONSTRAINT_SCHEMA)  + N'.'  -- not in MS-SQL
    + QUOTENAME(rc.CONSTRAINT_NAME) + N'; ' + CHAR(13) + CHAR(10) 
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 

INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1 
    ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
    AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
    AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME 

EXECUTE(@sql) 

GO
DECLARE @sql2 NVARCHAR(max)=''

SELECT @sql2 += ' Drop table ' + QUOTENAME(TABLE_SCHEMA) + '.'+ QUOTENAME(TABLE_NAME) + '; '
FROM   INFORMATION_SCHEMA.TABLES
WHERE  TABLE_TYPE = 'BASE TABLE'

Exec Sp_executesql @sql2 
GO 

CREATE TABLE Category(
CatId int primary key,
CatName nvarchar(255)
)
GO

Create table Book(
BookId int identity(1,1) primary key,
title nvarchar(255),
author nvarchar(255),
bookStatus int,
categoryId int REFERENCES Category(CatId),
publicDate date
)
GO

INSERT INTO Category(catID,catName) VALUES
(1,'13+'),
(2,'15+'),
(3,'17+')
GO

INSERT INTO Book(title,author, bookStatus,categoryId, publicDate) VALUES
('Khanh O Ban Ben', 'Huynh Hoang Ty', 1,3,'2024-11-02'),
('Ty O Ban Ben', 'Nguyen Le Khac Vu', 0,2,'2024-11-02'),
('Huy O Ban Ben', 'Tran Le Gia Huy', 1,1,'2024-11-02')
GO
-- Status: 1 Active, 0 Inactive