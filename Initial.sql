CREATE DATABASE test123
USE test123

drop table building

CREATE TABLE [building] (
    [BuildingId] uniqueidentifier NOT NULL,
    [City] nvarchar(85) NOT NULL,
    [StreetAddress] nvarchar(255) NOT NULL,
    [Neighbourhood] nvarchar(100) NOT NULL,
    [Floors] int NOT NULL
);

ALTER TABLE [building] ADD	CONSTRAINT PK_building PRIMARY KEY([BuildingId]);
