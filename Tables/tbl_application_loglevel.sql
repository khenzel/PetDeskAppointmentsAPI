USE [SolutionsWeb]
GO

/****** Object:  Table [dbo].[ApplicationLogLevel]    Script Date: 03/17/2016 11:38:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ApplicationLogLevel](
	[level] [nvarchar](10) NOT NULL,
	[description] [nvarchar](50) NULL
) ON [PRIMARY]

GO


