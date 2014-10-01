-- =============================================
-- Script Template
-- =============================================


USE [SJPConfiguration] 
GO

IF NOT EXISTS (SELECT * FROM ReferenceNum)
INSERT INTO ReferenceNum VALUES (0)
