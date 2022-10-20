Imports System.Data.SqlClient
Imports System.IO
Imports System.Collections.Generic
Imports Futuro.RiepilogoAccreditamento

''' <summary>
''' Questa classe contiene le funzionalità per l'accesso ai dati delle informazioni sull'ente
''' </summary>
Public Class clsEnte
	Public Function GetDatiEnte(
		idEnte As Integer,
		conn As SqlConnection,
		Optional tran As SqlTransaction = Nothing
	) As Ente
		'Caricamento informazioni Ente Titolare
		Dim sqlCaricaRiepilogo As String = ""
		sqlCaricaRiepilogo += "SELECT "
		sqlCaricaRiepilogo += "  E.IdEnte,"
		sqlCaricaRiepilogo += "  S.IdClasseAccreditamento,"
		sqlCaricaRiepilogo += "  IntestazioneDocumenti Sezione,"
		sqlCaricaRiepilogo += "  T.Privato,"
		sqlCaricaRiepilogo += "  CodiceRegione CodiceEnte,"
		sqlCaricaRiepilogo += "  Denominazione,"
		sqlCaricaRiepilogo += "  Tipologia,"
		sqlCaricaRiepilogo += "  DataRichiestaAccreditamento,"
		sqlCaricaRiepilogo += "  CodiceFiscale,DataNominaRL,"
		sqlCaricaRiepilogo += "  PrefissoTelefonoRichiestaRegistrazione + ' ' + TelefonoRichiestaRegistrazione Telefono,"
		sqlCaricaRiepilogo += "  DataCostituzione,"
		sqlCaricaRiepilogo += "  S.Sezione Classe,"
		sqlCaricaRiepilogo += "  PrefissoFax + ' ' + E.Fax Fax,"
		sqlCaricaRiepilogo += "  E.Http Sito,"
		sqlCaricaRiepilogo += "  Email,EmailCertificata PEC,"
		sqlCaricaRiepilogo += "  CASE WHEN Indirizzo IS NULL THEN NULL ELSE Indirizzo + ', - ' + Cap + ' ' + Comune + ' (' + Provincia +')' END SedeLegale,"
		sqlCaricaRiepilogo += "  DataRichiestaEnte "
		sqlCaricaRiepilogo += "FROM Enti E JOIN "
		sqlCaricaRiepilogo += "  SezioniAlboSCU S ON E.IdSezione=S.IdSezione JOIN "
		sqlCaricaRiepilogo += "  TipologieEnti T ON T.Descrizione = E.Tipologia LEFT JOIN "
		sqlCaricaRiepilogo += "  SezioniAlboSCU SA ON E.IdSezione = SA.IdSezione LEFT JOIN "
		sqlCaricaRiepilogo += "  RegioniCompetenze R ON R.IdRegioneCompetenza=SA.IdRegioneCompetenza "
		sqlCaricaRiepilogo += "WHERE IDEnte =@IdEnte"
		Dim cmdCaricaRiepilogo As New SqlCommand(sqlCaricaRiepilogo, conn)
		cmdCaricaRiepilogo.Parameters.AddWithValue("@IdEnte", idEnte)
		If tran IsNot Nothing Then
			cmdCaricaRiepilogo.Transaction = tran
		End If
		Dim rdrCaricaRiepilogo As SqlDataReader = Nothing
		Dim ente As Ente
		Try
			rdrCaricaRiepilogo = cmdCaricaRiepilogo.ExecuteReader()
			If rdrCaricaRiepilogo.Read() Then
				ente = New Ente() With {
				.IdEnte = GetData(rdrCaricaRiepilogo("IdEnte")),
				.Sezione = GetData(rdrCaricaRiepilogo("Sezione")),
				.IdClasseAccreditamento = GetData(rdrCaricaRiepilogo("IdClasseAccreditamento")),
				.Privato = GetData(rdrCaricaRiepilogo("Privato")),
				.CodiceEnte = GetData(rdrCaricaRiepilogo("CodiceEnte")),
				.Denominazione = GetData(rdrCaricaRiepilogo("Denominazione")),
				.Tipo = GetData(rdrCaricaRiepilogo("Tipologia")),
				.DataRichiesta = GetData(rdrCaricaRiepilogo("DataRichiestaAccreditamento")),
				.CodiceFiscale = GetData(rdrCaricaRiepilogo("CodiceFiscale")),
				.DataNominaRappresentanteLegale = GetData(rdrCaricaRiepilogo("DataNominaRL")),
				.Telefono = GetData(rdrCaricaRiepilogo("Telefono")),
				.DataCostituzione = GetData(rdrCaricaRiepilogo("DataCostituzione")),
				.Classe = GetData(rdrCaricaRiepilogo("Classe")),
				.Fax = GetData(rdrCaricaRiepilogo("Fax")),
				.Sito = GetData(rdrCaricaRiepilogo("Sito")),
				.Email = GetData(rdrCaricaRiepilogo("Email")),
				.PEC = GetData(rdrCaricaRiepilogo("PEC"))
				}
			Else
				Throw New Exception("Dati Ente / Classe non trovati")
			End If
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati Ente Titolare", ex)
		Finally
			If rdrCaricaRiepilogo IsNot Nothing Then
				rdrCaricaRiepilogo.Close()
			End If
		End Try

		Return ente
	End Function

	Function GetSedeLegale(
		idEnte As Integer,
		conn As SqlConnection,
		Optional tran As SqlTransaction = Nothing
	) As Sede
		Dim sqlSedi As String = ""
		sqlSedi += "SELECT "
		sqlSedi += "  IdEnte,"
		sqlSedi += "  S.Denominazione Nome,"
		sqlSedi += "  NMaxVolontari NumeroVolontari,"
		sqlSedi += "  CASE WHEN C.IDComune IS NULL THEN NULL WHEN P.IDProvincia IS NULL THEN C.Denominazione ELSE S.Indirizzo + ', ' + Civico + ' - ' + S.Cap + ' ' + C.Denominazione + ' (' + P.DescrAbb +')' END Indirizzo, "
		sqlSedi += "  T.TitoloGiuridico,"
		sqlSedi += "  Http Sito, "
		sqlSedi += "  Email,"
		sqlSedi += "  EST.IDTipoSede,"
		sqlSedi += "  C.ComuneNazionale "
		sqlSedi += "FROM "
		sqlSedi += "  entisedi S LEFT JOIN "
		sqlSedi += "  entisediattuazioni SA on S.IDEnteSede=SA.IDEnteSede LEFT JOIN"
		sqlSedi += "  comuni C on C.IDComune = S.IDComune LEFT JOIN"
		sqlSedi += "  provincie P on P.IDProvincia = C.IDProvincia LEFT JOIN"
		sqlSedi += "  TitoliGiuridici T ON T.IdTitoloGiuridico =S.IdTitoloGiuridico LEFT JOIN"
		sqlSedi += "  entiseditipi EST ON EST.IDEnteSede = S.IDEnteSede "
		sqlSedi += "WHERE "
		sqlSedi += "  S.IDStatoEnteSede IN (1,4)"
		sqlSedi += "  AND IDTipoSede = 1"
		sqlSedi += "  AND IDEnte =@IdEnte"
		Dim cmdSedi As New SqlCommand(sqlSedi, conn)
		If tran IsNot Nothing Then
			cmdSedi.Transaction = tran
		End If
		cmdSedi.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrSedi As SqlDataReader = Nothing
		Try
			rdrSedi = cmdSedi.ExecuteReader()
			If rdrSedi.Read() Then
				Dim sede As New Sede
				sede.IdEnte = GetData(rdrSedi("IdEnte"))
				sede.IdTipoSede = GetData(rdrSedi("IDTipoSede"))
				sede.Nome = GetData(rdrSedi("Nome"))
				sede.NumeroVolontari = GetData(rdrSedi("NumeroVolontari"))
				sede.Indirizzo = GetData(rdrSedi("Indirizzo"))
				sede.Sito = GetData(rdrSedi("Sito"))
				sede.Email = GetData(rdrSedi("Email"))
				sede.Nazionale = GetData(rdrSedi("ComuneNazionale"))
				Return sede
			End If
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati Sedi", ex)
		Finally
			If rdrSedi IsNot Nothing Then
				rdrSedi.Close()
			End If
		End Try
		Return Nothing
	End Function

	Function GetRappresentanteLegale(
		idEnte As Integer,
		conn As SqlConnection,
		Optional tran As SqlTransaction = Nothing
	) As Risorsa
		Dim sqlRisorsa As String = ""
		sqlRisorsa += "SELECT"
		sqlRisorsa += "  Nome + ' ' + Cognome Nome,"
		sqlRisorsa += "  CodiceFiscale,"
		sqlRisorsa += "  DataNascita,"
		sqlRisorsa += "  CASE WHEN PN.IDProvincia IS NULL THEN CN.Denominazione ELSE CN.Denominazione + ' (' + PN.DescrAbb + ')' END LuogoNascita, CASE WHEN CR.IDComune IS NULL THEN NULL WHEN PR.IDProvincia IS NULL THEN CR.Denominazione ELSE E.Indirizzo + ', ' + Civico + ' - ' + E.Cap + ' ' + CR.Denominazione + ' (' + PR.DescrAbb +')' END IndirizzoResidenza "
		sqlRisorsa += "FROM "
		sqlRisorsa += "  entepersonale E JOIN"
		sqlRisorsa += "  entepersonaleruoli R ON R.IDEntePersonale = E.IDEntePersonale LEFT JOIN"
		sqlRisorsa += "  comuni CR on CR.IDComune = E.IDComuneResidenza  LEFT JOIN"
		sqlRisorsa += "  provincie PR on PR.IDProvincia = CR.IDProvincia LEFT JOIN"
		sqlRisorsa += "  comuni CN on CN.IDComune = E.IDComuneNascita LEFT JOIN"
		sqlRisorsa += "  provincie PN on PN.IDProvincia = CN.IDProvincia "
		sqlRisorsa += "WHERE "
		sqlRisorsa += "  R.IDRuolo = 4"
		sqlRisorsa += "  AND E.DataFineValidità is null"
        sqlRisorsa += "  AND R.DataFineValidità is null"
        sqlRisorsa += "  AND R.Accreditato in (0,1)"
        sqlRisorsa += "  AND IDEnte = @IdEnte"
        sqlRisorsa += "  ORDER BY R.DataInizioValidità desc"
		Dim cmdRisorsa As New SqlCommand(sqlRisorsa, conn)
		If tran IsNot Nothing Then
			cmdRisorsa.Transaction = tran
		End If
		cmdRisorsa.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrRisorse As SqlDataReader = Nothing
		Try
			rdrRisorse = cmdRisorsa.ExecuteReader()
			If rdrRisorse.Read() Then
				Dim rapresentanteLegale As New Risorsa
				rapresentanteLegale.Nome = GetData(rdrRisorse("Nome"))
				rapresentanteLegale.CodiceFiscale = GetData(rdrRisorse("CodiceFiscale"))
				rapresentanteLegale.DataNascita = GetData(rdrRisorse("DataNascita"))
				rapresentanteLegale.LuogoNascita = GetData(rdrRisorse("LuogoNascita"))
				rapresentanteLegale.IndirizzoResidenza = GetData(rdrRisorse("IndirizzoResidenza"))
				Return rapresentanteLegale
			End If
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati Rappresentante legale", ex)
		Finally
			If rdrRisorse IsNot Nothing Then
				rdrRisorse.Close()
			End If
		End Try
		Return Nothing
	End Function

	Function GetCoordinatoreResponsabile(
		idEnte As Integer,
		conn As SqlConnection,
		Optional tran As SqlTransaction = Nothing
	) As Risorsa
		Dim sqlRisorsa As String = ""
		sqlRisorsa += "SELECT"
		sqlRisorsa += "  Nome + ' ' + Cognome Nome,"
		sqlRisorsa += "  CodiceFiscale,"
		sqlRisorsa += "  DataNascita,"
		sqlRisorsa += "  CASE WHEN PN.IDProvincia IS NULL THEN CN.Denominazione ELSE CN.Denominazione + ' (' + PN.DescrAbb + ')' END LuogoNascita, CASE WHEN CR.IDComune IS NULL THEN NULL WHEN PR.IDProvincia IS NULL THEN CR.Denominazione ELSE E.Indirizzo + ', ' + Civico + ' - ' + E.Cap + ' ' + CR.Denominazione + ' (' + PR.DescrAbb +')' END IndirizzoResidenza "
		sqlRisorsa += "FROM "
		sqlRisorsa += "  entepersonale E JOIN"
		sqlRisorsa += "  entepersonaleruoli R ON R.IDEntePersonale = E.IDEntePersonale LEFT JOIN"
		sqlRisorsa += "  comuni CR on CR.IDComune = E.IDComuneResidenza  LEFT JOIN"
		sqlRisorsa += "  provincie PR on PR.IDProvincia = CR.IDProvincia LEFT JOIN"
		sqlRisorsa += "  comuni CN on CN.IDComune = E.IDComuneNascita LEFT JOIN"
		sqlRisorsa += "  provincie PN on PN.IDProvincia = CN.IDProvincia "
		sqlRisorsa += "WHERE "
		sqlRisorsa += "  R.IDRuolo = 20"
		sqlRisorsa += "  AND E.DataFineValidità is null"
        sqlRisorsa += "  AND R.DataFineValidità is null"
        sqlRisorsa += "  AND R.Accreditato in (0,1)"
		sqlRisorsa += "  AND IDEnte = @IdEnte"
		Dim cmdRisorsa As New SqlCommand(sqlRisorsa, conn)
		If tran IsNot Nothing Then
			cmdRisorsa.Transaction = tran
		End If
		cmdRisorsa.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrRisorse As SqlDataReader = Nothing
		Try
			rdrRisorse = cmdRisorsa.ExecuteReader()
			If rdrRisorse.Read() Then
				Dim rapresentanteLegale As New Risorsa
				rapresentanteLegale.Nome = GetData(rdrRisorse("Nome"))
				rapresentanteLegale.CodiceFiscale = GetData(rdrRisorse("CodiceFiscale"))
				rapresentanteLegale.DataNascita = GetData(rdrRisorse("DataNascita"))
				rapresentanteLegale.LuogoNascita = GetData(rdrRisorse("LuogoNascita"))
				rapresentanteLegale.IndirizzoResidenza = GetData(rdrRisorse("IndirizzoResidenza"))
				Return rapresentanteLegale
			End If
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati Rappresentante legale", ex)
		Finally
			If rdrRisorse IsNot Nothing Then
				rdrRisorse.Close()
			End If
		End Try
		Return Nothing
	End Function

	Public Function RicavaIdEnteFase(
		IdEnte As Integer,
		conn As SqlConnection
	) As Integer
        Dim IdEF As Integer = 0
		Dim myQuerySql As String
		Dim myDataReader As SqlDataReader

		myQuerySql = " Select IdEnteFase from EntiFasi where idente=" & IdEnte & "  AND STATO =1 and TipoFase IN (1,2) and GETDATE() between DataInizioFase and DataFineFase order by IdEnteFase Desc "
		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, conn)
		myDataReader.Read()
        If myDataReader.HasRows = True Then
            If Not IsDBNull(myDataReader("IdEnteFase")) Then
                IdEF = myDataReader("IdEnteFase")
            End If
        End If
        If Not myDataReader Is Nothing Then
            myDataReader.Close()
            myDataReader = Nothing
        End If

        Return IdEF
    End Function


	''' <summary>
	''' Verifica Se nella fase di adeguamento corrente sono stati aggiunti uno o più enti di accoglienza
	''' </summary>
	''' <param name="IdEnte">Identificativo dell'ente Titolare</param>
	''' <param name="conn">Connessione al DB sul quale operare i dati</param>
	''' <returns></returns>
	Public Function VerificaInserimentouovoEnte(
		IdEnte As Integer,
		conn As SqlConnection,
		Optional tran As SqlTransaction = Nothing
	) As Boolean
		Dim query As String = ""
		Dim myDataReader As SqlDataReader
		query += " select a.codiceregione"
		query += " from enti a  "
		query += " inner join entirelazioni b on a.idente = b.identefiglio  "
		query += " inner join statienti c on a.idstatoente = c.idstatoente "
		query += " left join (select EFE.IdEnte "
		query += "      from entifasi_enti EFE "
		query += "      INNER JOIN EntiFasi EF on EF.IdEnteFase=EFE.IdEnteFase  "
		query += "     WHERE  EF.Stato=3) PRESENTATE "
		query += "    on a.IDEnte = PRESENTATE.IdEnte  "
		query += " where b.IDEntePadre = " & IdEnte & " and a.idstatoente = 6 AND PRESENTATE.IdEnte IS NULL "
		myDataReader = ClsServer.CreaDatareader(query, conn, transazione:=tran)
		Dim inseritoEnte As Boolean = myDataReader.HasRows
		ChiudiDataReader(myDataReader)
		Return inseritoEnte
	End Function

	''' <summary>
	''' Verifica Se è stata selezionata la dichiarazione di impegno
	''' </summary>
	''' <param name="IdEnte">Identificativo dell'ente Titolare</param>
	''' <param name="conn">Connessione al DB sul quale operare i dati</param>
	''' <returns></returns>
	Public Function VerificaDichiarazioneImpegno(
		IdEnte As Integer,
		conn As SqlConnection,
		Optional tran As SqlTransaction = Nothing
	) As Boolean
		Dim query As String = ""
		Dim myDataReader As SqlDataReader
		query += " select DichiarazioneImpegno"
		query += " from enti "
		query += " where IDEnte = " & IdEnte
		myDataReader = ClsServer.CreaDatareader(query, conn, transazione:=tran)
		Dim inseritoEnte As Boolean = False
		If myDataReader.Read Then
			inseritoEnte = GetData(myDataReader("DichiarazioneImpegno")) = True
		End If
		ChiudiDataReader(myDataReader)
		Return inseritoEnte
	End Function

	Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
		If Not dataReader Is Nothing Then
			dataReader.Close()
			dataReader = Nothing
		End If
	End Sub

	Private Function GetData(data As Object) As Object
		If IsDBNull(data) Then
			Return Nothing
		End If
		Return data
	End Function
End Class
