--DDL Section--
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MPstorage')
	CREATE DATABASE MPstorage
GO
	
USE MPstorage
GO

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE name = 'orders' and xtype = 'U')
	CREATE TABLE Orders (
		Id INT PRIMARY KEY IDENTITY,
		Price SMALLMONEY NOT NULL CHECK (Price >= 0),
		Date DATE NOT NULL
	)
GO

--DML Section--
BEGIN TRANSACTION
DBCC CHECKIDENT ('Orders', RESEED, 0)
DELETE Orders
COMMIT
GO

BEGIN TRANSACTION
	USE MPstorage
	GO

	DECLARE @from INT, @to INT  
	SET @from = 0
	SET @to = 25

	WHILE @from < @to
	BEGIN
		INSERT INTO Orders (Price, Date) VALUES (RAND() * 1000, 
		DATEFROMPARTS(1950 + ROUND(RAND() * 70, 0), 1 + ROUND(RAND() * 11, 0), 1 + ROUND(RAND() * 27, 0)))
		SET @from += 1
	END
COMMIT
GO

--Testing--
SELECT * FROM Orders