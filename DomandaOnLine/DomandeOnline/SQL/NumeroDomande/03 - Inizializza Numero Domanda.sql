
update SUSCN_DOL_PROGETTI_DISPONIBILI
SET
	NumeroDomande = (
		SELECT
			COUNT(*)
		FROM 
			DomandaPartecipazione D
		WHERE
			D.DataAnnullamento IS NULL AND
			D.DataPresentazione IS NOT NULL AND
			CodiceProgettoSelezionato =CodiceProgetto AND
			CodiceSedeSelezionata= CodiceSede
	)

