USE [SolutionsWeb]
GO

/****** Object:  Table [dbo].[ApplicationLog]    Script Date: 03/17/2016 11:38:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ApplicationLog](
	[log_id] [int] IDENTITY(1,1) NOT NULL,
	[datetime] [datetime] NOT NULL,
	[level] [nvarchar](50) NOT NULL,
	[application] [nvarchar](50) NOT NULL,
	[message] [nvarchar](max) NOT NULL,
	[exception] [nvarchar](max) NOT NULL,
	[user] [nvarchar](50) NOT NULL,
	[machine] [nvarchar](50) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


