USE [SolutionsWeb]
GO

/****** Object:  StoredProcedure [dbo].usp_petdesk_insert_appointments    Script Date: 09/10/2019 12:19:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].usp_petdesk_insert_appointments'))
DROP PROCEDURE [dbo].usp_petdesk_insert_appointments
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- ==========================================================================================
-- Description:
--		Update or insert bulk records to the tbl_PetDesk_Appointments_Frequency temp table
--			This is called by petDeskGetAppointments service to store frequency 
--			values for:
--				AppointmentTypeFrequency API
--				AppointmentRequestFrequency API
-- ==========================================================================================
-- History:
--		09/10/2019		Kevin Henzel			Created
-- ==========================================================================================

-- Create our datatable type. Drop if exists.

IF TYPE_ID(N'PetDeskAppointmentsDataTable') IS NOT NULL
	BEGIN
		DROP TYPE dbo.PetDeskAppointmentsDataTable
	END

	CREATE TYPE dbo.PetDeskAppointmentsDataTable AS TABLE
		(
			[Type] [varchar](15) NOT NULL,
			[Value] [varchar](50) NOT NULL,
			[Frequency] INT NOT NULL
		)
GO

CREATE PROCEDURE [dbo].usp_petdesk_insert_appointments
	
	@appointmentsDataTable dbo.PetDeskAppointmentsDataTable READONLY

AS
	
	SET NOCOUNT ON
	
	-- Drop temp table if exists
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_PetDesk_Appointments_Frequency]'))
	DROP TABLE [dbo].[tbl_PetDesk_Appointments_Frequency]
	
	-- Create our temp table with the fill from our service API load
	SELECT *
	INTO tbl_PetDesk_Appointments_Frequency
	FROM @appointmentsDataTable adt
	ORDER BY adt.Type ASC, adt.Frequency DESC

	SELECT @@ROWCOUNT as RowsInserted
	
GO
