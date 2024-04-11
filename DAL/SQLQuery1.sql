
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/01/2024 17:42:39
-- Generated from EDMX file: E:\CDB dotnet\ProjetoBancoDiagramas\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ProjetoBanco];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TB_CLIENTETB_CONTA]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TB_CLIENTE] DROP CONSTRAINT [FK_TB_CLIENTETB_CONTA];
GO
IF OBJECT_ID(N'[dbo].[FK_TB_TIPO_CONTATB_CONTA]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TB_CONTA] DROP CONSTRAINT [FK_TB_TIPO_CONTATB_CONTA];
GO
IF OBJECT_ID(N'[dbo].[FK_TB_TIPO_CLIENTETB_CLIENTE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TB_CLIENTE] DROP CONSTRAINT [FK_TB_TIPO_CLIENTETB_CLIENTE];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TB_CLIENTE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TB_CLIENTE];
GO
IF OBJECT_ID(N'[dbo].[TB_CONTA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TB_CONTA];
GO
IF OBJECT_ID(N'[dbo].[TB_TIPO_CONTA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TB_TIPO_CONTA];
GO
IF OBJECT_ID(N'[dbo].[TB_TIPO_CLIENTE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TB_TIPO_CLIENTE];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TB_CLIENTE'
CREATE TABLE [dbo].[TB_CLIENTE] (
    [ID_CLIENTE] int IDENTITY(1,1) NOT NULL,
    [TB_TIPO_CONTA_tp_conta] int  NOT NULL,
    [TB_TIPO_CLIENTEId] int  NOT NULL,
    [rg_cliente] nvarchar(max)  NOT NULL,
    [nm_cliente] nvarchar(max)  NOT NULL,
    [dn_cliente] datetime  NOT NULL,
    [pw_cliente] nvarchar(max)  NOT NULL,
    [tp_cliente] nvarchar(max)  NOT NULL,
    [nm_conta] nvarchar(max)  NOT NULL,
    [TB_CONTA_ID_CONTA] int  NOT NULL
);
GO

-- Creating table 'TB_CONTA'
CREATE TABLE [dbo].[TB_CONTA] (
    [ID_CONTA] int IDENTITY(1,1) NOT NULL,
    [TB_TIPO_CONTA_tp_conta] int  NOT NULL,
    [TB_CORRENTEId] int  NOT NULL,
    [nm_conta] nvarchar(max)  NOT NULL,
    [vl_conta] decimal(18,0)  NOT NULL,
    [tp_conta] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TB_TIPO_CONTA'
CREATE TABLE [dbo].[TB_TIPO_CONTA] (
    [tp_conta] int IDENTITY(1,1) NOT NULL,
    [tp_corrente] nvarchar(max)  NOT NULL,
    [tp_poupanca] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TB_TIPO_CLIENTE'
CREATE TABLE [dbo].[TB_TIPO_CLIENTE] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [tp_comum] nvarchar(max)  NOT NULL,
    [tp_super] nvarchar(max)  NOT NULL,
    [tp_premium] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID_CLIENTE] in table 'TB_CLIENTE'
ALTER TABLE [dbo].[TB_CLIENTE]
ADD CONSTRAINT [PK_TB_CLIENTE]
    PRIMARY KEY CLUSTERED ([ID_CLIENTE] ASC);
GO

-- Creating primary key on [ID_CONTA] in table 'TB_CONTA'
ALTER TABLE [dbo].[TB_CONTA]
ADD CONSTRAINT [PK_TB_CONTA]
    PRIMARY KEY CLUSTERED ([ID_CONTA] ASC);
GO

-- Creating primary key on [tp_conta] in table 'TB_TIPO_CONTA'
ALTER TABLE [dbo].[TB_TIPO_CONTA]
ADD CONSTRAINT [PK_TB_TIPO_CONTA]
    PRIMARY KEY CLUSTERED ([tp_conta] ASC);
GO

-- Creating primary key on [Id] in table 'TB_TIPO_CLIENTE'
ALTER TABLE [dbo].[TB_TIPO_CLIENTE]
ADD CONSTRAINT [PK_TB_TIPO_CLIENTE]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TB_CONTA_ID_CONTA] in table 'TB_CLIENTE'
ALTER TABLE [dbo].[TB_CLIENTE]
ADD CONSTRAINT [FK_TB_CLIENTETB_CONTA]
    FOREIGN KEY ([TB_CONTA_ID_CONTA])
    REFERENCES [dbo].[TB_CONTA]
        ([ID_CONTA])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TB_CLIENTETB_CONTA'
CREATE INDEX [IX_FK_TB_CLIENTETB_CONTA]
ON [dbo].[TB_CLIENTE]
    ([TB_CONTA_ID_CONTA]);
GO

-- Creating foreign key on [TB_TIPO_CONTA_tp_conta] in table 'TB_CONTA'
ALTER TABLE [dbo].[TB_CONTA]
ADD CONSTRAINT [FK_TB_TIPO_CONTATB_CONTA]
    FOREIGN KEY ([TB_TIPO_CONTA_tp_conta])
    REFERENCES [dbo].[TB_TIPO_CONTA]
        ([tp_conta])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TB_TIPO_CONTATB_CONTA'
CREATE INDEX [IX_FK_TB_TIPO_CONTATB_CONTA]
ON [dbo].[TB_CONTA]
    ([TB_TIPO_CONTA_tp_conta]);
GO

-- Creating foreign key on [TB_TIPO_CLIENTEId] in table 'TB_CLIENTE'
ALTER TABLE [dbo].[TB_CLIENTE]
ADD CONSTRAINT [FK_TB_TIPO_CLIENTETB_CLIENTE]
    FOREIGN KEY ([TB_TIPO_CLIENTEId])
    REFERENCES [dbo].[TB_TIPO_CLIENTE]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TB_TIPO_CLIENTETB_CLIENTE'
CREATE INDEX [IX_FK_TB_TIPO_CLIENTETB_CLIENTE]
ON [dbo].[TB_CLIENTE]
    ([TB_TIPO_CLIENTEId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------