USE unscproduzione
CREATE USER ScheduleAgent FROM LOGIN ScheduleAgent
GRANT SELECT, INSERT, DELETE, UPDATE ON unscproduzione.dbo.DOL_DomandePresentate TO ScheduleAgent
GRANT SELECT, INSERT ON unscproduzione.dbo.LogScheduleAgent TO ScheduleAgent
GRANT SELECT, INSERT, UPDATE ON unscproduzione.dbo.Job TO ScheduleAgent
GRANT SELECT, INSERT, UPDATE ON unscproduzione.dbo.JobExecution TO ScheduleAgent
GRANT SELECT, INSERT, UPDATE ON unscproduzione.dbo.JobSchedule TO ScheduleAgent
GRANT SELECT ON unscproduzione.dbo.VW_LogScheduleAgent TO ScheduleAgent
GRANT EXECUTE ON unscproduzione.dbo.SP_DOL_IMPORTAZIONE_DOMANDE TO ScheduleAgent
