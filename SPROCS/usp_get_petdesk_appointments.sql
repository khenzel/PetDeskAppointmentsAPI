USE [SolutionsWeb]
GO

/****** Object:  StoredProcedure [dbo].[usp_get_petdesk_appointments]    Script Date: 09/10/2019 12:11:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_get_petdesk_appointments]'))
	DROP PROCEDURE [dbo].[usp_get_petdesk_appointments]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- ==========================================================================================
-- Description:
--		Returns results from the tbl_PetDesk_Appointments_Frequency table for use with PetDeskAPI
-- ==========================================================================================
-- History:
--		09/10/2019		Kevin Henzel			Created sproc.
-- ==========================================================================================

CREATE PROCEDURE [dbo].[usp_get_petdesk_appointments]  

	@TypeFilter TINYINT = 0 -- 0 = all, 1 = Request, 2 = Type
	
AS  
SELECT [Type]
	   , [Value]
	   , [Frequency]
FROM dbo.[tbl_PetDesk_Appointments_Frequency] 
WHERE 
	(@TypeFilter = 1) AND [Type] = 'apptRequest'
	OR (@TypeFilter = 2) AND [Type] = 'apptType'
	OR @TypeFilter = 0
