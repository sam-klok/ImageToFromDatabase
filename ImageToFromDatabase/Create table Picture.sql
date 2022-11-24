USE [LifelongLearning]
GO

CREATE TABLE [dbo].[Picture](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [varchar](255) NOT NULL,
	[Type] [varchar](255) NOT NULL DEFAULT 'PNG',
	[Hash] [varchar](255) NULL, -- let's have for encryption
	[PicData] [varbinary](max) NULL 
)
GO


