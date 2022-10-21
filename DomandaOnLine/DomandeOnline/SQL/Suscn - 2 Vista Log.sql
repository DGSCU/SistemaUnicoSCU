USE unscproduzione
IF EXISTS(select * from sys.views where name='VW_LogScheduleAgent' and type='v')
	drop view VW_LogScheduleAgent;
GO
CREATE VIEW VW_LogScheduleAgent
AS
SELECT
	Id,
	Message,
	MessageTemplate,
	Level,
	Timestamp,
	Exception,
	Properties,
	SUBSTRING(
		Exception,
		CHARINDEX(': ',Exception)+2,
		CASE WHEN 
			CHARINDEX(' --->',Exception)>0 THEN
				CHARINDEX(' --->',Exception)-2
		WHEN 
			CHARINDEX('   in',Exception)>0 THEN
				CHARINDEX('   in',Exception)-4
		ELSE
			LEN (Exception)
		END
		-CHARINDEX(': ',Exception)
		) Errore
FROM
	LogScheduleAgent