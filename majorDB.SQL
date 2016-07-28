USE Master
GO
CREATE DATABASE [majorDB]
GO
USE [majorDB]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Major]') AND type in (N'U'))
    DROP TABLE [dbo].[Major]
GO
CREATE TABLE [dbo].[Major] (
[MajorID] INTEGER  NOT NULL  
, [MajorAbbr] VARCHAR(10)  NOT NULL  
, [totalStudentsApplied] CHAR(10)  NULL  
, [totalStudentsAdmitted] CHAR(10)  NULL  
, [ gpaRangeMin] DECIMAL(10,2)  NULL  
, [ gpaRangeMax] DECIMAL(10,2)  NULL  
, [ gpaAverage] DECIMAL(10,2)  NULL  
, [ gpaMinimum] DECIMAL(10,2)  NULL  
, [applicationOpenDate(fall)] DATETIME  NULL  
, [applicationDueDate(fall)] DATETIME  NULL  
, [applicationOpenDate(winter)] DATETIME  NULL  
, [applicationDueDate(winter)] DATETIME  NULL  
, [applicationOpenDate(spring)] DATETIME  NULL  
, [applicationDueDate(spring)] DATETIME  NULL  
, [admittionDecisionDate] DATETIME  NULL  
, [advisorEmail] VARCHAR(100)  NULL  
, [majorType] VARCHAR(25)  NOT NULL  
)
GO

ALTER TABLE [dbo].[Major] ADD CONSTRAINT [Major_PK] PRIMARY KEY CLUSTERED (
[MajorID]
)
GO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Course]') AND type in (N'U'))
    DROP TABLE [dbo].[Course]
GO
CREATE TABLE [dbo].[Course] (
[CourseID] INTEGER  NOT NULL  
, [CourseNumber] INTEGER  NOT NULL  
, [credit] INTEGER  NOT NULL  
, [GeneralEducationRequirementDesignator] VARCHAR(10)  NULL  
, [Desc] VARCHAR(1000)  NULL  
, [QuartersOffered] VARCHAR(10)  NULL  
)
GO

ALTER TABLE [dbo].[Course] ADD CONSTRAINT [Course_PK] PRIMARY KEY CLUSTERED (
[CourseID]
)
GO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MajorCourseRelationship]') AND type in (N'U'))
    DROP TABLE [dbo].[MajorCourseRelationship]
GO
CREATE TABLE [dbo].[MajorCourseRelationship] (
[MajorCourseRelationshipID] INTEGER  NOT NULL  
, [MajorID] INTEGER  NOT NULL  
, [CourseID] INTEGER  NOT NULL  
, [IsPrereq] BIT  NULL  
, [IsGradreq] BIT  NULL  
)
GO

ALTER TABLE [dbo].[MajorCourseRelationship] ADD CONSTRAINT [MajorCourseRelationship_PK] PRIMARY KEY CLUSTERED (
[MajorCourseRelationshipID]
)
GO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PrereqRelationship]') AND type in (N'U'))
    DROP TABLE [dbo].[PrereqRelationship]
GO
CREATE TABLE [dbo].[PrereqRelationship] (
[PrereqRelationshipID] CHAR(10)  NOT NULL  
, [DominateCourse] INTEGER  NULL  
, [RecessiveCourse] INTEGER  NULL  
)
GO

ALTER TABLE [dbo].[PrereqRelationship] ADD CONSTRAINT [PrereqRelationship_PK] PRIMARY KEY CLUSTERED (
[PrereqRelationshipID]
)
GO
GO

GO

GO

ALTER TABLE [dbo].[MajorCourseRelationship] WITH CHECK ADD CONSTRAINT [Course_MajorCourseRelationship_FK1] FOREIGN KEY (
[CourseID]
)
REFERENCES [dbo].[Course] (
[CourseID]
)
ALTER TABLE [dbo].[MajorCourseRelationship] WITH CHECK ADD CONSTRAINT [Major_MajorCourseRelationship_FK1] FOREIGN KEY (
[MajorID]
)
REFERENCES [dbo].[Major] (
[MajorID]
)
GO

ALTER TABLE [dbo].[PrereqRelationship] WITH CHECK ADD CONSTRAINT [Course_PrereqRelationship_FK2] FOREIGN KEY (
[RecessiveCourse]
)
REFERENCES [dbo].[Course] (
[CourseID]
)
ALTER TABLE [dbo].[PrereqRelationship] WITH CHECK ADD CONSTRAINT [Course_PrereqRelationship_FK1] FOREIGN KEY (
[DominateCourse]
)
REFERENCES [dbo].[Course] (
[CourseID]
)
GO

