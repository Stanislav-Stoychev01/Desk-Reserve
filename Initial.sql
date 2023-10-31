CREATE DATABASE test123
USE test123

drop table building

CREATE TABLE [building] (
    [BuildingId] uniqueidentifier NOT NULL PRIMARY KEY,
    [City] nvarchar(30) NOT NULL,
    [StreetAddress] nvarchar(100) NOT NULL,
    [Neighbourhood] nvarchar(50) NOT NULL,
    [Floors] int NOT NULL
);