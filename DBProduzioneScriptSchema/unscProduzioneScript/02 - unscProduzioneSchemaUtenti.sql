USE [unscproduzione]
GO
/****** Object:  User [BO_USER]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [BO_USER] FOR LOGIN [BO_USER] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [DOLusr]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [DOLusr] FOR LOGIN [DOLusr] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [ENTIBANCHE]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [ENTIBANCHE] FOR LOGIN [ENTIBANCHE] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [eurekaUsr]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [eurekaUsr] FOR LOGIN [eurekaUsr] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [OBIETTORI\cmonaco]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [OBIETTORI\cmonaco] FOR LOGIN [OBIETTORI\cmonaco] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [obiettori\cproia]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [obiettori\cproia] FOR LOGIN [OBIETTORI\cproia] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [OBIETTORI\INTRANET01$]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [OBIETTORI\INTRANET01$] FOR LOGIN [OBIETTORI\INTRANET01$] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [OBIETTORI\INTRANET02$]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [OBIETTORI\INTRANET02$] FOR LOGIN [OBIETTORI\INTRANET02$] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [OBIETTORI\WWW2$]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [OBIETTORI\WWW2$] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [OBIETTORI\wwwGestioneBanche]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [OBIETTORI\wwwGestioneBanche] FOR LOGIN [OBIETTORI\wwwGestioneBanche]
GO
/****** Object:  User [paghe]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [paghe] FOR LOGIN [paghe] WITH DEFAULT_SCHEMA=[paghe]
GO
/****** Object:  User [PAGHE_NM]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [PAGHE_NM] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[PAGHE_NM]
GO
/****** Object:  User [ScheduleAgent]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [ScheduleAgent] FOR LOGIN [ScheduleAgent] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [SIGED]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [SIGED] FOR LOGIN [SIGED] WITH DEFAULT_SCHEMA=[SIGED]
GO
/****** Object:  User [UNSC_DW]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [UNSC_DW] FOR LOGIN [UNSC_DW] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [UNSC_LETTURA]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [UNSC_LETTURA] FOR LOGIN [UNSC_LETTURA] WITH DEFAULT_SCHEMA=[UNSC_LETTURA]
GO
/****** Object:  User [USER_CUD]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [USER_CUD] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [WELODGE]    Script Date: 12/08/2022 11:17:27 ******/
CREATE USER [WELODGE] FOR LOGIN [WELODGE] WITH DEFAULT_SCHEMA=[dbo]
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'BO_USER'
GO
sys.sp_addrolemember @rolename = N'db_datareader', @membername = N'DOLusr'
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'OBIETTORI\cmonaco'
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'OBIETTORI\INTRANET01$'
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'OBIETTORI\INTRANET02$'
GO
sys.sp_addrolemember @rolename = N'db_datareader', @membername = N'UNSC_DW'
GO
sys.sp_addrolemember @rolename = N'db_datareader', @membername = N'UNSC_LETTURA'
GO
/****** Object:  Schema [paghe]    Script Date: 12/08/2022 11:17:27 ******/
CREATE SCHEMA [paghe]
GO
/****** Object:  Schema [PAGHE_NM]    Script Date: 12/08/2022 11:17:27 ******/
CREATE SCHEMA [PAGHE_NM]
GO
/****** Object:  Schema [SIGED]    Script Date: 12/08/2022 11:17:27 ******/
CREATE SCHEMA [SIGED]
GO
/****** Object:  Schema [UNSC_LETTURA]    Script Date: 12/08/2022 11:17:27 ******/
CREATE SCHEMA [UNSC_LETTURA]
GO
