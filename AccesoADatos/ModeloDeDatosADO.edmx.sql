
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/30/2022 01:33:11
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

IF OBJECT_ID(N'[dbo].[FK_InvitacionJugador]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invitaciones] DROP CONSTRAINT [FK_InvitacionJugador];
GO
-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Invitaciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invitaciones];
GO
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

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdJugador] in table 'Jugadores'
ALTER TABLE [dbo].[Jugadores]
ADD CONSTRAINT [PK_Jugadores]
    PRIMARY KEY CLUSTERED ([IdJugador] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------