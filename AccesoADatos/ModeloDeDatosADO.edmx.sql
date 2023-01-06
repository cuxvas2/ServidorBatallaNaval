
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/05/2023 21:41:38
-- Generated from EDMX file: C:\Users\Victor\Documents\Trabajos UV\5 semestre\Juego\ServidorBatallaNaval\AccesoADatos\ModeloDeDatosADO.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BatallaNavalDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Jugadores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Jugadores];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Jugadores'
CREATE TABLE [dbo].[Jugadores] (
    [IdJugador] int IDENTITY(1,1) NOT NULL,
    [CorreoElectronico] nvarchar(max)  NOT NULL,
    [Apodo] nvarchar(max)  NOT NULL,
    [Contraseña] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'JugadoresJugadores'
CREATE TABLE [dbo].[JugadoresJugadores] (
    [JugadoresAmigos_Jugadores1_IdJugador] int  NOT NULL,
    [Amigos_IdJugador] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdJugador] in table 'Jugadores'
ALTER TABLE [dbo].[Jugadores]
ADD CONSTRAINT [PK_Jugadores]
    PRIMARY KEY CLUSTERED ([IdJugador] ASC);
GO

-- Creating primary key on [JugadoresAmigos_Jugadores1_IdJugador], [Amigos_IdJugador] in table 'JugadoresJugadores'
ALTER TABLE [dbo].[JugadoresJugadores]
ADD CONSTRAINT [PK_JugadoresJugadores]
    PRIMARY KEY CLUSTERED ([JugadoresAmigos_Jugadores1_IdJugador], [Amigos_IdJugador] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [JugadoresAmigos_Jugadores1_IdJugador] in table 'JugadoresJugadores'
ALTER TABLE [dbo].[JugadoresJugadores]
ADD CONSTRAINT [FK_JugadoresAmigos_Jugadores]
    FOREIGN KEY ([JugadoresAmigos_Jugadores1_IdJugador])
    REFERENCES [dbo].[Jugadores]
        ([IdJugador])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Amigos_IdJugador] in table 'JugadoresJugadores'
ALTER TABLE [dbo].[JugadoresJugadores]
ADD CONSTRAINT [FK_JugadoresAmigos_Jugadores1]
    FOREIGN KEY ([Amigos_IdJugador])
    REFERENCES [dbo].[Jugadores]
        ([IdJugador])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JugadoresAmigos_Jugadores1'
CREATE INDEX [IX_FK_JugadoresAmigos_Jugadores1]
ON [dbo].[JugadoresJugadores]
    ([Amigos_IdJugador]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------