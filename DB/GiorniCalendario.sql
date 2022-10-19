declare @start_dt as date = '1/1/2009';		-- Date from which the calendar table will be created.
declare @end_dt as date = '1/1/2030';		-- Calendar table will be created up to this date (not including).

declare @dates as table (
 Data date primary key,
 Anno smallint,
 Mese tinyint,
 Giorno tinyint,
 GiornoSettimana tinyint,
 GiornoSettimanaTesto varchar(10),
 MeseTesto varchar(10)
)

while @start_dt < @end_dt
begin
	insert into @dates
	values(
		@start_dt, year(@start_dt), month(@start_dt), day(@start_dt), 
		datepart(weekday, @start_dt), datename(weekday, @start_dt), datename(month, @start_dt)
	)
	set @start_dt = dateadd(day, 1, @start_dt)
end
select * INTO Giorno from @dates