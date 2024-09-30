-- Creates table for Users and CaseWorker
--What type of user is determened by the UserType ('Normal', 'Caseworker' or 'Prioritized')
CREATE TABLE User (
    UserId INTEGER PRIMARY KEY,
    UserName TEXT,
    UserPassword TEXT,
    UserType TEXT NOT NULL,
    email TEXT,
    FirstName TEXT,
    LastName TEXT,
    TLFnumber INTEGER NOT NULL,
    Gender TEXT,
    RegistrationDate DATE NOT NULL,
    UserStatus TEXT NOT NULL
) ;

-- Creates Table for Cases
--Case is connected to a caseworker trough a foreign key ('UserName')
CREATE TABLE Cases (
    CaseId INTEGER PRIMARY KEY,
    CaseDate DATE NOT NULL,
    CaseInfo TEXT NOT NULL,
    CaseStatus TEXT NOT NULL,
    GeoLocation INTEGER NOT NULL,
    UserName TEXT,
    constraint fk_user FOREIGN KEY (UserName) REFERENCES User(UserName)
) ;

-- Selects all info from User Table
SELECT * FROM User ;


-- Selects all info from Cases Table
SELECT * FROM Cases ;

--Select all info on Caseworkers
Select * 
FROM User
Where UserType = 'CaseWorker';

--Select all info on Normal users
Select * 
FROM User
Where UserType = 'Normal';


--Select all info on prioritized users
Select * 
FROM User
Where UserType = 'Prioritized';

-- Updates casestatus of individual cases
UPDATE Cases
SET CaseStatus = 'Active' 
WHERE CaseId = 1;