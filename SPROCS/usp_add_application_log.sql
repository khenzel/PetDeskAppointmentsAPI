USE [SolutionsWeb]
GO
/****** Object:  StoredProcedure [dbo].[usp_add_application_log]    Script Date: 9/11/2019 11:03:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kevin Henzel
-- Create date: 3/20/2016
-- Description: Add record to Application log
-- exec usp_add_ApplicationLog '3/20/2013','1','DMV','Message','Exception','Test User','Test Machine'
-- truncate table SolutionsWeb.dbo.ApplicationLog
-- =============================================
ALTER PROCEDURE [dbo].[usp_add_application_log]
(
	@datetime as datetime,
	@level as nvarchar(50),
	@application as nvarchar(50),
	@message as nvarchar(max),
	@exception as nvarchar(max),
	@user as nvarchar(50),
	@machine as nvarchar(50)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into SolutionsWeb.dbo.ApplicationLog
	Values (@datetime, @level, @application, @message, @exception, @user, @machine)

END
