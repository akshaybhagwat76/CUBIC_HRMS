USE [TEMP_CUBIC_HRMS]
GO
/****** Object:  Table [dbo].[CalendarColor]    Script Date: 07/20/2023 11:36:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CalendarColor](
	[CalendarColorId] [int] IDENTITY(1,1) NOT NULL,
	[CalendarDescription] [nvarchar](150) NULL,
	[Color] [nvarchar](50) NULL,
 CONSTRAINT [PK_CalendarColor] PRIMARY KEY CLUSTERED 
(
	[CalendarColorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CalendarColor] ON
INSERT [dbo].[CalendarColor] ([CalendarColorId], [CalendarDescription], [Color]) VALUES (1, N'PublicHoliday', N'Purple')
INSERT [dbo].[CalendarColor] ([CalendarColorId], [CalendarDescription], [Color]) VALUES (2, N'Leave', N'Blue')
SET IDENTITY_INSERT [dbo].[CalendarColor] OFF
/****** Object:  StoredProcedure [dbo].[USP_VP_GET_LEAVE_DATA]    Script Date: 07/20/2023 11:36:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[USP_VP_GET_LEAVE_DATA]

AS
BEGIN
	SELECT 
	A.[LEAVEH_NO],
	[LEAVED_DATE_FROM], 
	[LEAVED_DATE_TO],
	LEAVE_NAME ,
	 ISNULL((SELECT Color FROM CalendarColor where CalendarDescription='Leave'),'BLACK') AS COLOR
FROM 
	[dbo].[T_EMP_LEAVE_HDR] A,[dbo].[T_EMP_LEAVE_DET] B, [M_LEAVE_TYPE] C 
WHERE A.[LEAVEH_NO] = B.[LEAVED_NO] AND A.[LEAVEH_CODE]=c.[LEAVE_CODE]

UNION

SELECT 
	'' as [LEAVEH_NO],
	PUBLIC_HOLIDAYDATE AS [LEAVED_DATE_FROM]
	,PUBLIC_HOLIDAYDATE AS [LEAVED_DATE_TO]
	,PUBLIC_DESCRIPTION AS  LEAVE_NAME 
	,ISNULL((SELECT Color FROM CalendarColor where CalendarDescription='PublicHoliday'),'BLACK') AS COLOR
FROM M_PUBHOLIDAY

ORDER BY LEAVED_DATE_FROM;
END
GO
