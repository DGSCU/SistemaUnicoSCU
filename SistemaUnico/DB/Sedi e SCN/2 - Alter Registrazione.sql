USE Registrazione

ALTER TABLE Registrazione
ADD Albo varchar(3)
GO
UPDATE Registrazione
SET
Albo ='SCU'