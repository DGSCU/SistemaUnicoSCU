USE [Registrazione]
GO
/****** Object:  UserDefinedFunction [dbo].[f_BinaryToBase64]    Script Date: 12/08/2022 12:35:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_BinaryToBase64](@bin VARBINARY(MAX))
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Base64 VARCHAR(MAX)
   
    /*
        SELECT dbo.f_BinaryToBase64(CONVERT(VARBINARY(MAX), 'Converting this text to Base64...'))
    */
   
    SET @Base64 = CAST(N'' AS XML).value('xs:base64Binary(xs:hexBinary(sql:variable("@bin")))','VARCHAR(MAX)')
   
    RETURN @Base64
END
GO