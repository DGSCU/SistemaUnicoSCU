USE [DomandaOnline]
GO
/****** Object:  View [dbo].[VW_DGSCU_RiepilogoDomande]    Script Date: 12/08/2022 15:41:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VW_DGSCU_RiepilogoDomande]
AS

select ins.GruppoBando,ins.data,domandeInserite,isnull(domandePresentate,0)domandePresentate,isnull(domandeAnnullate,0) domandeAnnullate
--,ISNULL(domandeInseriteGG,0) domandeInseriteGG,isnull(domandePresentateGG,0) domandePresentateGG,isnull(domandeAnnullateGG,0) domandeAnnullateGG
,ISNULL(domandeInseriteAmb,0) domandeInseriteAmb,isnull(domandePresentateAmb,0) domandePresentateAmb,isnull(domandeAnnullateAmb,0) domandeAnnullateAmb
,ISNULL(domandeInseriteAfin,0) domandeInseriteAfin,isnull(domandePresentateAfin,0) domandePresentateAfin,isnull(domandeAnnullateAfin,0) domandeAnnullateAfin
,ISNULL(domandeInseriteDGT,0) domandeInseriteDGT ,ISNULL(domandePresentateDGT,0) domandePresentateDGT ,ISNULL(domandeAnnullateDGT,0) domandeAnnullateDGT from 
(select GruppoBando, convert(date,DataInserimento) data, count(*) domandeInserite from DomandaPartecipazione
where  GruppoBando=63 --and CodiceProgettoSelezionato is not null
group by convert(date,DataInserimento),GruppoBando) ins 
left join (select GruppoBando, convert(date,DataPresentazione) data, count(*) domandePresentate from DomandaPartecipazione
where not DataPresentazione is null group by convert(date,DataPresentazione),GruppoBando) pres on  ins.GruppoBando=pres.GruppoBando AND ins.data=pres.data
left join (select GruppoBando, convert(date,DataRichiestaAnnullamento) data, count(*) domandeAnnullate from DomandaPartecipazione
where not DataRichiestaAnnullamento is null group by convert(date,DataRichiestaAnnullamento),GruppoBando) ann on  ins.GruppoBando=ann.GruppoBando AND ins.data=ann.data
-- Progetti GG
left join (select GruppoBando, convert(date,DataInserimento) data, count(*) domandeInseriteGG from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IdTipoGG is not null group by convert(date,DataInserimento),GruppoBando) insGG on ins.GruppoBando=insGG.GruppoBando AND ins.data=insGG.data
left join (select GruppoBando, convert(date,DataPresentazione) data, count(*) domandePresentateGG from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IdTipoGG is not null AND not DataPresentazione is null group by convert(date,DataPresentazione),GruppoBando) presGG on  ins.GruppoBando=presGG.GruppoBando AND ins.data=presGG.data
left join (select GruppoBando, convert(date,DataRichiestaAnnullamento) data, count(*) domandeAnnullateGG from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IdTipoGG is not null AND not DataRichiestaAnnullamento is null group by convert(date,DataRichiestaAnnullamento),GruppoBando) annGG on  ins.GruppoBando=annGG.GruppoBando AND ins.data=annGG.data
-- Progetti DGT
left join (select GruppoBando, convert(date,DataInserimento) data, count(*) domandeInseriteDGT from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IsDigitale=1 group by convert(date,DataInserimento),GruppoBando) insDGT on ins.GruppoBando=insDGT.GruppoBando AND ins.data=insDGT.data
left join (select GruppoBando, convert(date,DataPresentazione) data, count(*) domandePresentateDGT from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IsDigitale=1 AND not DataPresentazione is null group by convert(date,DataPresentazione),GruppoBando) presDGT on  ins.GruppoBando=presDGT.GruppoBando AND ins.data=presDGT.data
left join (select GruppoBando, convert(date,DataRichiestaAnnullamento) data, count(*) domandeAnnullateDGT from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IsDigitale=1 AND not DataRichiestaAnnullamento is null group by convert(date,DataRichiestaAnnullamento),GruppoBando) annDGT on  ins.GruppoBando=annDGT.GruppoBando AND ins.data=annDGT.data
-- Progetti Ambientali
left join (select GruppoBando, convert(date,DataInserimento) data, count(*) domandeInseriteAmb from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IsAmbientale=1 group by convert(date,DataInserimento),GruppoBando) insAmb on ins.GruppoBando=insAmb.GruppoBando AND ins.data=insAmb.data
left join (select GruppoBando, convert(date,DataPresentazione) data, count(*) domandePresentateAmb from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IsAmbientale=1 AND not DataPresentazione is null group by convert(date,DataPresentazione),GruppoBando) presAmb on  ins.GruppoBando=presAmb.GruppoBando AND ins.data=presAmb.data
left join (select GruppoBando, convert(date,DataRichiestaAnnullamento) data, count(*) domandeAnnullateAmb from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IsAmbientale=1 AND not DataRichiestaAnnullamento is null group by convert(date,DataRichiestaAnnullamento),GruppoBando) annAmb on  ins.GruppoBando=annAmb.GruppoBando AND ins.data=annAmb.data
--
-- Progetti Autofin
left join (select GruppoBando, convert(date,DataInserimento) data, count(*) domandeInseriteAfin from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IsDigitale<>1 and c.IsAmbientale<>1 group by convert(date,DataInserimento),GruppoBando) insAfin on ins.GruppoBando=insAfin.GruppoBando AND ins.data=insAfin.data
left join (select GruppoBando, convert(date,DataPresentazione) data, count(*) domandePresentateAfin from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IsDigitale<>1 AND c.IsAmbientale<>1 And not DataPresentazione is null group by convert(date,DataPresentazione),GruppoBando) presAfin on  ins.GruppoBando=presAfin.GruppoBando AND ins.data=presAfin.data
left join (select GruppoBando, convert(date,DataRichiestaAnnullamento) data, count(*) domandeAnnullateAfin from DomandaPartecipazione a 
join (select distinct CodiceProgetto,IdProgramma from SUSCN_DOL_PROGETTI_DISPONIBILI) b on a.CodiceProgettoSelezionato=b.CodiceProgetto 
join SUSCN_DOL_PROGRAMMA c on b.IdProgramma=c.IdProgramma
where c.IsDigitale<>1 AND c.IsAmbientale<>1 AND not DataRichiestaAnnullamento is null group by convert(date,DataRichiestaAnnullamento),GruppoBando) annAfin on  ins.GruppoBando=annAfin.GruppoBando AND ins.data=annAfin.data

GO
/****** Object:  View [dbo].[VW_DomandaPresentata]    Script Date: 12/08/2022 15:41:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VW_DomandaPresentata] AS
SELECT
	D.Id,
	D.CodiceFiscale,
	D.GruppoBando,
	D.DataInserimento,
	D.UserIdModifica,
	D.DataModifica,
	D.CodiceProgettoSelezionato,
	D.CodiceSedeSelezionata,
	D.DataPresentazione,
	D.DataAnnullamento,
	D.NomeFileCV,
	D.Nome,
	D.Cognome,
	D.Genere,
	D.DataNascita,
	D.LuogoNascita,
	D.NazioneNascita,
	D.Cittadinanza,
	D.Telefono,
	D.Email,
	D.ResidenzaEstera,
	D.IndirizzoCompletoResidenza,
	D.NazioneResidenza,
	D.ComuneResidenza,
	D.ProvinciaResidenza,
	D.ViaResidenza,
	D.CivicoResidenza,
	D.CapResidenza,
	CASE WHEN D.ResidenzaEstera=1 THEN 
		D.IndirizzoCompletoResidenza 
	ELSE
		CONCAT(D.ViaResidenza,' ',D.CivicoResidenza,' ',D.CapResidenza,' ',D.ComuneResidenza,' ',D.ProvinciaResidenza)
	END IndirizoCompleto,
	D.ComuneRecapito,
	D.ProvinciaRecapito,
	D.ViaRecapito,
	D.CivicoRecapito,
	D.CapRecapito,
	D.CodiceMinoriOpportunita,
	D.IdMotivazione,
	M.Descrizione Motivazione,
	D.CodiceDichiarazioneCittadinanza,
	D.NonCondanneOk,
	D.TrasferimentoSedeOk,
	D.TrasferimentoProgettoOk,
	D.AltreDichiarazioniOk,
	D.IdTitoloStudio,
	TA.Descrizione TitoloStudio,
	D.PrivacyPresaVisione,
	D.PrivacyConsenso,
	D.PrecedentiEnte,
	D.PrecedentiEnteDescrizione,
	D.PrecedentiAltriEnti,
	D.PrecedentiAltriEntiDescrizione,
	D.PrecedentiImpiego,
	D.PrecedentiImpiegoDescrizione,
	D.IdTitoloStudioEsperienze,
	TE.Descrizione TitoloStudioEsperienze,
	D.FormazioneDisciplina,
	D.FormazioneAnno,
	D.FormazioneData,
	D.FormazioneItalia,
	D.FormazioneIstituto,
	D.FormazioneEnte,
	D.IscrizioneSuperioreAnno,
	D.IscrizioneSuperioreIstituto,
	D.IscrizioneLaureaAnno,
	D.IscrizioneLaureaCorso,
	D.IscrizioneLaureaIstituto,
	D.CorsiEffettuati,
	D.Specializzazioni,
	D.Competenze,
	D.Altro,
	P.Sito,
	DataRichiestaAnnullamento,
	MA.Descrizione MotivoAnnullamento,
	FormazioneAnagraficaDisciplina,
	FormazioneAnagraficaAnno,
	FormazioneAnagraficaItalia,
	FormazioneAnagraficaIstituto,
	FormazioneAnagraficaEnte,
	DichiarazioneResidenzaOK,
	DichiarazioneRequisitiGaranziaGiovani,
	DataPresaInCaricoGaranziaGiovani,
	LuogoPresaInCaricoGaranziaGiovani,
	DataDIDGaranziaGiovani,
	LuogoDIDGaranziaGiovani,
	AlternativaRequisitiGaranziaGiovani,
	DichiarazioneMinoriOpportunita

  FROM
		DomandaPartecipazione D LEFT JOIN
		SUSCN_DOL_PROGETTI_DISPONIBILI P ON
			D.CodiceProgettoSelezionato=P.CodiceProgetto AND
			D.CodiceSedeSelezionata=P.CodiceSede LEFT JOIN
		Motivazione M ON
			D.IdMotivazione=M.Id LEFT JOIN
		TitoloStudio TA ON
			D.IdTitoloStudio=TA.Id LEFT JOIN
		TitoloStudio TE ON
			D.IdTitoloStudioEsperienze=TE.Id LEFT JOIN
		MotivoAnnullamento MA ON
			MA.Id=D.IdMotivazioneAnnullamento
  WHERE
	D.DataPresentazione IS NOT NULL
GO
/****** Object:  View [dbo].[VW_VolontariNonPresentate]    Script Date: 12/08/2022 15:41:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VW_VolontariNonPresentate]
AS 

select U.Nome,U.Cognome,U.Email from
	DomandaPartecipazione D JOIN
	AspNetUsers U ON U.id=D.UserIdModifica

where DataModifica is not null and DataPresentazione is null
GO
