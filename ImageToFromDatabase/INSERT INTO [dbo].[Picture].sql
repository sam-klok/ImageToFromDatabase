USE [LifelongLearning]
GO

INSERT INTO [dbo].[Picture]
           ([Name]
           ,[Type]
           ,[Hash]
           ,[PicData])
     VALUES
           (<Name, varchar(255),>
           ,<Type, varchar(255),>
           ,<Hash, varchar(255),>
           ,<PicData, varbinary(max),>)
GO


