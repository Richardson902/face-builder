DROP TABLE IF EXISTS [dbo].Person;

CREATE TABLE [dbo].Person
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY,
	[firstName] NVARCHAR(15) NOT NULL,
	[lastName] NVARCHAR(15) NOT NULL,
	[city] NCHAR(15) NOT NULL,
	[hair] INT NOT NULL,
	[eyes] INT NOT NULL,
	[nose] INT NOT NULL,
	[mouth] INT NOT NULL
	
);

INSERT INTO Person (firstName, lastName, city, hair, eyes, nose, mouth) VALUES ('John', 'Doe', 'Halifax', 0, 0, 0, 0);