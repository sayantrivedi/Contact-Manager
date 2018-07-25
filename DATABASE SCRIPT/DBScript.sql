USE [master]

GO


CREATE DATABASE [ContactsDB] 
GO


USE [ContactsDB]

GO


CREATE TABLE [dbo].[Contact](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[PostalCode] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Status] [varchar](20) DEFAULT ('Active'),
	CONSTRAINT [chk_Status]  CHECK  (([Status]='Inactive' OR [Status]='Active')),
	
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[Desc] [nvarchar](max) NOT NULL,
	[Inner Exception] [nvarchar](max) NULL,
	[StackTrace] [nvarchar](1) NOT NULL,
	[Date] dateTime NOT NULL,
	) 

GO