Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports
Imports System.Configuration.ConfigurationManager
Imports Ionic.Zip

Imports System.IO
Imports System.Web.UI.Page
Imports System.Security.Cryptography

Public Class ClsServer
	Public Shared GetPdfError As String
	Public Shared NumberFormat As System.Globalization.NumberFormatInfo = New System.Globalization.CultureInfo("en-US", False).NumberFormat

	Public Shared Sub GestioneFormatoNumerico()
		NumberFormat.CurrencyDecimalDigits = 2
		NumberFormat.CurrencyGroupSeparator = "."
		NumberFormat.CurrencyDecimalSeparator = ","
		NumberFormat.CurrencySymbol = ""
		NumberFormat.CurrencyNegativePattern = 1
	End Sub
	Public Shared Function trovaIstruttoriaEnte(ByVal idente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
		Dim dtrGenerico As SqlClient.SqlDataReader
		Dim strsql As String
		strsql = " select * from statienti " &
		" inner join enti on enti.idstatoente=statienti.idstatoente " &
		" where enti.idente=" & idente & " and statienti.istruttoria=1 "
		dtrGenerico = ClsServer.CreaDatareader(strsql, conn)
		trovaIstruttoriaEnte = dtrGenerico.HasRows
		If Not dtrGenerico Is Nothing Then
			dtrGenerico.Close()
			dtrGenerico = Nothing
		End If
	End Function
	Public Shared Function trovaEnteChiuso(ByVal idente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
		Dim dtrGenerico As SqlClient.SqlDataReader
		Dim strsql As String
		strsql = " select * from statienti " &
		" inner join enti on enti.idstatoente=statienti.idstatoente " &
		" where enti.idente=" & idente & " and (statienti.Chiuso=1 or statienti.Sospeso=1) "
		dtrGenerico = ClsServer.CreaDatareader(strsql, conn)
		trovaEnteChiuso = dtrGenerico.HasRows
		If Not dtrGenerico Is Nothing Then
			dtrGenerico.Close()
			dtrGenerico = Nothing
		End If
	End Function

	Public Shared Function EseguiStoreSostituzioneSede(ByVal IntIdProgetto As Integer, ByVal intIdCodiceSedeDaSostituire As Integer, ByVal intIdCodiceSedeInSostituzione As Integer, ByVal strDataRicollocamento As String, ByVal strUserName As String, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection) As String

		Try

			Dim MySqlCommand As SqlClient.SqlCommand
			MySqlCommand = New SqlClient.SqlCommand
			MySqlCommand.CommandType = CommandType.StoredProcedure
			MySqlCommand.CommandText = StrStoreProcedure
			MySqlCommand.Connection = conn

			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IdAttività"
			sparam.SqlDbType = SqlDbType.Int
			MySqlCommand.Parameters.Add(sparam)

			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@IdSedeDaSostituire"
			sparam2.SqlDbType = SqlDbType.Int
			MySqlCommand.Parameters.Add(sparam2)

			Dim sparam3 As SqlClient.SqlParameter
			sparam3 = New SqlClient.SqlParameter
			sparam3.ParameterName = "@IdSedeInSostituzione"
			sparam3.SqlDbType = SqlDbType.Int
			MySqlCommand.Parameters.Add(sparam3)

			Dim sparam4 As SqlClient.SqlParameter
			sparam4 = New SqlClient.SqlParameter
			sparam4.ParameterName = "@DataRicollocamento"
			sparam4.SqlDbType = SqlDbType.DateTime
			MySqlCommand.Parameters.Add(sparam4)

			Dim sparam5 As SqlClient.SqlParameter
			sparam5 = New SqlClient.SqlParameter
			sparam5.ParameterName = "@Username"
			sparam5.SqlDbType = SqlDbType.NVarChar
			MySqlCommand.Parameters.Add(sparam5)

			Dim sparamOutput As SqlClient.SqlParameter
			sparamOutput = New SqlClient.SqlParameter
			sparamOutput.ParameterName = "@Esito"
			sparamOutput.SqlDbType = SqlDbType.TinyInt
			sparamOutput.Direction = ParameterDirection.Output
			MySqlCommand.Parameters.Add(sparamOutput)

			Dim sparamOutput2 As SqlClient.SqlParameter
			sparamOutput2 = New SqlClient.SqlParameter
			sparamOutput2.ParameterName = "@Motivazione"
			sparamOutput2.SqlDbType = SqlDbType.NVarChar
			sparamOutput2.Size = 1000
			sparamOutput2.Direction = ParameterDirection.Output
			MySqlCommand.Parameters.Add(sparamOutput2)

			Dim Reader As SqlClient.SqlDataReader

			MySqlCommand.Parameters("@IdAttività").Value = IntIdProgetto
			MySqlCommand.Parameters("@IdSedeDaSostituire").Value = intIdCodiceSedeDaSostituire
			MySqlCommand.Parameters("@IdSedeInSostituzione").Value = intIdCodiceSedeInSostituzione
			MySqlCommand.Parameters("@DataRicollocamento").Value = strDataRicollocamento
			MySqlCommand.Parameters("@Username").Value = strUserName

			Reader = MySqlCommand.ExecuteReader

			If MySqlCommand.Parameters("@Esito").Value = 0 Then
				EseguiStoreSostituzioneSede = MySqlCommand.Parameters("@Motivazione").Value
			Else
				EseguiStoreSostituzioneSede = "OK"
			End If

			Reader.Close()
			Reader = Nothing

			Return EseguiStoreSostituzioneSede

		Catch ex As Exception
			EseguiStoreSostituzioneSede = "NEGATIVO - CONTATTARE L'ASSISTENZA"
			Return EseguiStoreSostituzioneSede
		End Try

	End Function

	Public Shared Function EseguiStoreSostituzioneRisorsaDiProgetto(ByVal IntIdRisorsaDaSostituire As Integer, ByVal intIdRisorsaInSostituzione As Integer, ByVal strUserName As String, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection) As String

		Try

			Dim MySqlCommand As SqlClient.SqlCommand
			MySqlCommand = New SqlClient.SqlCommand
			MySqlCommand.CommandType = CommandType.StoredProcedure
			MySqlCommand.CommandText = StrStoreProcedure
			MySqlCommand.Connection = conn

			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IdSostituito"
			sparam.SqlDbType = SqlDbType.Int
			MySqlCommand.Parameters.Add(sparam)

			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@IdSubentrante"
			sparam2.SqlDbType = SqlDbType.Int
			MySqlCommand.Parameters.Add(sparam2)

			Dim sparam3 As SqlClient.SqlParameter
			sparam3 = New SqlClient.SqlParameter
			sparam3.ParameterName = "@username"
			sparam3.SqlDbType = SqlDbType.NVarChar
			MySqlCommand.Parameters.Add(sparam3)

			Dim sparamOutput As SqlClient.SqlParameter
			sparamOutput = New SqlClient.SqlParameter
			sparamOutput.ParameterName = "@Esito"
			sparamOutput.SqlDbType = SqlDbType.TinyInt
			sparamOutput.Direction = ParameterDirection.Output
			MySqlCommand.Parameters.Add(sparamOutput)

			Dim sparamOutput2 As SqlClient.SqlParameter
			sparamOutput2 = New SqlClient.SqlParameter
			sparamOutput2.ParameterName = "@Messaggio"
			sparamOutput2.SqlDbType = SqlDbType.NVarChar
			sparamOutput2.Size = 1000
			sparamOutput2.Direction = ParameterDirection.Output
			MySqlCommand.Parameters.Add(sparamOutput2)

			Dim Reader As SqlClient.SqlDataReader

			MySqlCommand.Parameters("@IdSostituito").Value = IntIdRisorsaDaSostituire
			MySqlCommand.Parameters("@IdSubentrante").Value = intIdRisorsaInSostituzione
			MySqlCommand.Parameters("@username").Value = strUserName

			Reader = MySqlCommand.ExecuteReader

			If MySqlCommand.Parameters("@Esito").Value = 0 Then
				EseguiStoreSostituzioneRisorsaDiProgetto = MySqlCommand.Parameters("@Messaggio").Value
			Else
				EseguiStoreSostituzioneRisorsaDiProgetto = "OK"
			End If

			Reader.Close()
			Reader = Nothing

			Return EseguiStoreSostituzioneRisorsaDiProgetto

		Catch ex As Exception
			EseguiStoreSostituzioneRisorsaDiProgetto = "NEGATIVO - CONTATTARE L'ASSISTENZA"
			Return EseguiStoreSostituzioneRisorsaDiProgetto
		End Try

	End Function

	Public Shared Function EseguiStoreReaderInserimentoIstanza(ByVal strNomeStore As String, ByVal IntEnte As Integer, ByVal intIdRegioneCompetenza As Integer, ByVal intIdBando As Integer, ByVal conn As SqlClient.SqlConnection) As SqlClient.SqlDataReader

		Dim MyCommand As New SqlClient.SqlCommand
		MyCommand.CommandType = CommandType.StoredProcedure
		MyCommand.CommandText = strNomeStore
		MyCommand.Connection = conn

		'PRIMO PARAMEtrO IDentita
		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		MyCommand.Parameters.Add(sparam)

		'SECONDO PARAMEtrO UserName
		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@IDCOMPETENZA"
		sparam1.SqlDbType = SqlDbType.Int
		MyCommand.Parameters.Add(sparam1)

		'SECONDO PARAMEtrO UserName
		Dim sparam2 As SqlClient.SqlParameter
		sparam2 = New SqlClient.SqlParameter
		sparam2.ParameterName = "@IDBANDO"
		sparam2.SqlDbType = SqlDbType.Int
		MyCommand.Parameters.Add(sparam2)

		Dim Reader As SqlClient.SqlDataReader
		MyCommand.Parameters("@IdEnte").Value = IntEnte
		MyCommand.Parameters("@IDCOMPETENZA").Value = intIdRegioneCompetenza
		MyCommand.Parameters("@IDBANDO").Value = intIdBando
		Reader = MyCommand.ExecuteReader
		EseguiStoreReaderInserimentoIstanza = Reader
		If Not Reader Is Nothing Then
			'Reader.Close()
			'Reader = Nothing
		End If
	End Function

	Public Shared Function EseguiStoreGeneraCodiciProgetto(ByVal idbandoattività As Integer, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection)
		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand
		MySqlCommand.CommandType = CommandType.StoredProcedure
		MySqlCommand.CommandText = StrStoreProcedure
		MySqlCommand.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdBandoAttività"
		sparam.SqlDbType = SqlDbType.Int
		MySqlCommand.Parameters.Add(sparam)
		Dim Reader As SqlClient.SqlDataReader
		MySqlCommand.Parameters("@IdBandoAttività").Value = idbandoattività
		Reader = MySqlCommand.ExecuteReader
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
	End Function
	Public Shared Function EseguiStoreOrdina(ByVal idattivitàsedeassegnazione As Integer, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection)
		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand
		MySqlCommand.CommandType = CommandType.StoredProcedure
		MySqlCommand.CommandText = StrStoreProcedure
		MySqlCommand.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IDAttivitaSedeAssegnazione"
		sparam.SqlDbType = SqlDbType.Int
		MySqlCommand.Parameters.Add(sparam)

		Dim Reader As SqlClient.SqlDataReader
		MySqlCommand.Parameters("@IDAttivitaSedeAssegnazione").Value = idattivitàsedeassegnazione
		Reader = MySqlCommand.ExecuteReader
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
	End Function
	Public Shared Function EseguiStoreSP_ELIMINA_MIGRAZIONE_DATI(ByVal idEnte As Integer, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection)
		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand
		MySqlCommand.CommandType = CommandType.StoredProcedure
		MySqlCommand.CommandText = StrStoreProcedure
		MySqlCommand.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IDEnte"
		sparam.SqlDbType = SqlDbType.Int
		MySqlCommand.Parameters.Add(sparam)

		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Valore"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		MySqlCommand.Parameters.Add(sparam1)

		Dim Reader As SqlClient.SqlDataReader
		MySqlCommand.Parameters("@IDEnte").Value = idEnte
		Reader = MySqlCommand.ExecuteReader
		EseguiStoreSP_ELIMINA_MIGRAZIONE_DATI = MySqlCommand.Parameters("@Valore").Value
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
	End Function
	Public Shared Function EseguiStoreEliminaSediRipristinoEnte(ByVal idente As Integer, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection)
		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand
		MySqlCommand.CommandType = CommandType.StoredProcedure
		MySqlCommand.CommandText = StrStoreProcedure
		MySqlCommand.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		MySqlCommand.Parameters.Add(sparam)
		Dim Reader As SqlClient.SqlDataReader
		MySqlCommand.Parameters("@IdEnte").Value = idente
		Reader = MySqlCommand.ExecuteReader
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
	End Function
	Public Shared Function EseguiQueryColl(ByVal Query As System.Collections.ArrayList, ByVal transazione As String, ByVal SqlConn As SqlConnection) As Boolean
		Dim IntX As Int16
		Dim SqlComm As System.Data.SqlClient.SqlCommand
		Dim Mytransaction As System.Data.SqlClient.SqlTransaction
		SqlComm = New System.Data.SqlClient.SqlCommand
		SqlComm.Connection = SqlConn
        transazione = Right(transazione, 32)
		'Se è stata definita la transazione
		Mytransaction = SqlConn.BeginTransaction(transazione)
		SqlComm.Transaction = Mytransaction

		Try
			'Ciclo tutte le query che devo eseguire
			For IntX = 0 To Query.Count - 1
				SqlComm.CommandText = Query.Item(IntX)
				If SqlComm.CommandText <> "" Then
					SqlComm.ExecuteNonQuery()
				End If
			Next
			Mytransaction.Commit()
			EseguiQueryColl = True
		Catch
			Mytransaction.Rollback(transazione)
			EseguiQueryColl = False
		End Try
		Mytransaction.Dispose()
	End Function
	Public Shared Function RendiFormat(ByVal prova As String) As String
		If prova = "" Then
			prova = "0"
		End If
		prova = Replace(prova, ".", ",")
		'Formattazione dell'oggetto testo
		prova = Format(CDbl(prova), "#,###,###,##0.00")
		RendiFormat = prova
	End Function


	Public Shared Function RendiFormatPunto(ByVal Valore As String) As String
		If Valore = "" Then
			Valore = "0"
		End If
		Valore = Replace(Valore, ",", ".")

		RendiFormatPunto = Valore
	End Function
	' connessione attiva

	Public Shared Function EseguiSqlClient(ByVal QueryString As String, ByVal conX As SqlClient.SqlConnection) As SqlClient.SqlCommand
		Dim myCommand As New SqlClient.SqlCommand
		Try
			myCommand = New SqlClient.SqlCommand(QueryString, conX)
			myCommand.ExecuteNonQuery()
			myCommand.Dispose()
		Catch es As Exception
			Throw New Exception(QueryString & es.Message.ToString)
		End Try
		myCommand = Nothing

	End Function

	Public Shared Function Connessione(ByVal Tipo As String, ByVal IdStatoEnte As Integer, ByRef ConnessioneSnapshot As System.Data.SqlClient.SqlConnection, ByVal ConnessioneProduzione As System.Data.SqlClient.SqlConnection, ByVal FlagForzatura As Boolean) As Boolean
		'*** ROUTINE MODIFICATA IL 05/08/2015 
		'*** I DATI VENGONO REGISTRATI SU SNAPSHOT SOLO PER I NUOVI ENTI (QUELLI CHE SI TROVANO IN ISTRUTTORIA)
		If Tipo <> "E" Then
			'La connessione è quella del Db di Produzione
			Return True
		Else
			'Controllo se l'ente ha il flag impostato
			If FlagForzatura = False Then
				''Controllo il periodo temporale
				'Dim MyDtrControllo As System.Data.SqlClient.SqlDataReader
				'MyDtrControllo = ClsServer.CreaDatareader("select IdProcessoTemporale from ProcessiTemporali where (Accreditamento=1) and (GetDate() between DataInizio and DataFine)", ConnessioneProduzione)
				'If MyDtrControllo.HasRows = True Then
				'    MyDtrControllo.Close()
				'    MyDtrControllo = Nothing

				'Controllo lo statoIdStatoEnte = 9 Or 'In adeguamento
				If IdStatoEnte = 8 Then '  in istruttoria 'modificato il 05/08/2015
					'Controllo se la connessione di appoggio è statta impostata
					If ConnessioneSnapshot Is Nothing Then
						ConnessioneSnapshot = New SqlClient.SqlConnection
						'ConnessioneSnapshot.ConnectionString = "user id=sa;password=vbra250;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=UNSC_SNAPSHOT"
						ConnessioneSnapshot.ConnectionString = "user id=" & ConfigurationSettings.AppSettings("DB_USERNAME") & ";password=" & ConfigurationSettings.AppSettings("DB_PASSWORD") & ";data source=" & ConfigurationSettings.AppSettings("DB_DATA_SOURCE") & ";persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=" & ConfigurationSettings.AppSettings("DB_NAME_SNAPSHOT") & ""
						'"user id=unsc_lettura;password=DDL74UNSC;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=UNSC_SNAPSHOT"
						ConnessioneSnapshot.Open()
					End If
					'La connessione è quella del DB di Appoggio
					Return False
				Else
					'La connessione è quella del Db di Produzione
					Return True
				End If
				'Else
				'    MyDtrControllo.Close()
				'    MyDtrControllo = Nothing
				'    'L'ente è al di fuori dal processo temporale per l'accreditamento
				'    'Controllo se la connessione di appoggio è statta impostata
				'    If ConnessioneSnapshot Is Nothing Then
				'        ConnessioneSnapshot = New SqlClient.SqlConnection
				'        ConnessioneSnapshot.ConnectionString = "user id=sa;password=vbra250;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=UNSC_SNAPSHOT"
				'        ConnessioneSnapshot.Open()
				'    End If
				'    'La connessione è quella del DB di Appoggio
				'    Return False
				'End If
			Else
				'Flag Forzatura Accreditamento impostato
				'La connessione è quella del Db di Produzione
				Return True
			End If
		End If
		'If Tipo <> "E" Then
		'    'La connessione è quella del Db di Produzione
		'    Return True
		'Else
		'    'Controllo se l'ente ha il flag impostato
		'    If FlagForzatura = False Then
		'        ''Controllo il periodo temporale
		'        'Dim MyDtrControllo As System.Data.SqlClient.SqlDataReader
		'        'MyDtrControllo = ClsServer.CreaDatareader("select IdProcessoTemporale from ProcessiTemporali where (Accreditamento=1) and (Getdate() between DataInizio and DataFine)", ConnessioneProduzione)
		'        'If MyDtrControllo.HasRows = true Then
		'        '    MyDtrControllo.Close()
		'        '    MyDtrControllo = Nothing

		'        'Controllo lo stato
		'        If IdStatoEnte = 9 Or IdStatoEnte = 8 Then 'In adeguamento o in istruttoria
		'            'Controllo se la connessione di appoggio è statta impostata
		'            If ConnessioneSnapshot Is Nothing Then
		'                ConnessioneSnapshot = New SqlClient.SqlConnection
		'                'ConnessioneSnapshot.ConnectionString = "user id=sa;password=vbra250;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=UNSC_SNAPSHOT"
		'                ConnessioneSnapshot.ConnectionString = "user id=" & AppSettings("DB_USERNAME") & ";password=" & AppSettings("DB_PASSWORD") & ";data source=" & AppSettings("DB_DATA_SOURCE") & ";persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=" & AppSettings("DB_NAME_SNAPSHOT") & ""

		'                '"user id=unsc_lettura;password=DDL74UNSC;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=UNSC_SNAPSHOT"
		'                ConnessioneSnapshot.Open()
		'            End If
		'            'La connessione è quella del DB di Appoggio
		'            Return False
		'        Else
		'            'La connessione è quella del Db di Produzione
		'            Return True
		'        End If
		'        'Else
		'        '    MyDtrControllo.Close()
		'        '    MyDtrControllo = Nothing
		'        '    'L'ente è al di fuori dal processo temporale per l'accreditamento
		'        '    'Controllo se la connessione di appoggio è statta impostata
		'        '    If ConnessioneSnapshot Is Nothing Then
		'        '        ConnessioneSnapshot = New SqlClient.SqlConnection
		'        '        ConnessioneSnapshot.ConnectionString = "user id=sa;password=vbra250;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=UNSC_SNAPSHOT"
		'        '        ConnessioneSnapshot.Open()
		'        '    End If
		'        '    'La connessione è quella del DB di Appoggio
		'        '    Return False
		'        'End If
		'    Else
		'        'Flag Forzatura Accreditamento impostato
		'        'La connessione è quella del Db di Produzione
		'        Return True
		'    End If
		'End If
	End Function

	Public Shared Function NoApice(ByVal Stringa As String) As String
		NoApice = Replace(Stringa, "'", "''")
	End Function

	Public Shared Function EseguiStoreVincoli(ByVal Intprametro As Integer, ByVal IntClasse As Integer, ByVal StrStoreProcedure As String, ByVal IntVincolo As Integer, ByVal conn As SqlClient.SqlConnection, Optional ByVal IntVerifica As Integer = Nothing) As Boolean
		Dim DtrParametri As System.Data.SqlClient.SqlDataReader
		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand


		'Definizione dei parametri
		Dim Parametri As SqlClient.SqlParameter

		DtrParametri = CreaDatareader("Select * From VincoliParametri Where IdVincolo = " & IntVincolo & " Order By Ordine", conn)
		Do While DtrParametri.Read
			Parametri = New SqlClient.SqlParameter
			Parametri.ParameterName = DtrParametri.Item("Nome")
			If DtrParametri.Item("Tipo") = 1 Then
				Parametri.Direction = ParameterDirection.Input
				Parametri.SqlDbType = SqlDbType.Int
				If IsDBNull(DtrParametri.Item("Campo")) = True Then
					Parametri.Value = CInt(DtrParametri.Item("Valore"))
				Else
					'PUNTO CRUCIALE .
					If UCase(DtrParametri.Item("Campo")) = "IDENTE" Then
						Parametri.Value = Intprametro
					ElseIf UCase(DtrParametri.Item("Campo")) = "IDRUOLO" Then
						Parametri.Value = Intprametro
					ElseIf UCase(DtrParametri.Item("Campo")) = "IDCLASSEACCREDITAMENTO" Then
						Parametri.Value = IntClasse
					ElseIf UCase(DtrParametri.Item("Campo")) = UCase("idattività") Then
						Parametri.Value = Intprametro
					End If
				End If
			Else
				Parametri.Direction = ParameterDirection.Output
				Parametri.SqlDbType = SqlDbType.Bit
			End If
			MySqlCommand.Parameters.Add(Parametri)
		Loop
		DtrParametri.Close()

		MySqlCommand.CommandType = CommandType.StoredProcedure
		'Definizione della store procedure
		MySqlCommand.CommandText = StrStoreProcedure
		MySqlCommand.Connection = conn
		MySqlCommand.ExecuteScalar()
		'Avendo impostato l'ultimo parametro come parametro di output
		EseguiStoreVincoli = MySqlCommand.Parameters("@VALORE").Value
		MySqlCommand.Dispose()

	End Function
	Public Shared Function EseguiStoreStoricoProgetti(ByVal IntIdStorico As Integer, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection) As SqlClient.SqlDataReader

		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand
		MySqlCommand.CommandType = CommandType.StoredProcedure
		MySqlCommand.CommandText = StrStoreProcedure
		MySqlCommand.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@idStorico"
		sparam.SqlDbType = SqlDbType.Int
		MySqlCommand.Parameters.Add(sparam)

		'Dim sparam1 As SqlClient.SqlParameter
		'sparam1 = New SqlClient.SqlParameter
		'sparam1.ParameterName = "@punteggiofinale"
		'sparam1.SqlDbType = SqlDbType.Int
		'sparam1.Direction = ParameterDirection.Output
		'MySqlCommand.Parameters.Add(sparam1)

		'Dim sparam2 As SqlClient.SqlParameter
		'sparam2 = New SqlClient.SqlParameter
		'sparam2.ParameterName = "@punteggioCP"
		'sparam2.SqlDbType = SqlDbType.Decimal
		'sparam2.Direction = ParameterDirection.Output
		'MySqlCommand.Parameters.Add(sparam2)

		'Dim sparam3 As SqlClient.SqlParameter
		'sparam3 = New SqlClient.SqlParameter
		'sparam3.ParameterName = "@punteggioCKH"
		'sparam3.SqlDbType = SqlDbType.Decimal
		'sparam3.Direction = ParameterDirection.Output
		'MySqlCommand.Parameters.Add(sparam3)

		'Dim sparam4 As SqlClient.SqlParameter
		'sparam4 = New SqlClient.SqlParameter
		'sparam4.ParameterName = "@punteggioCO"
		'sparam4.SqlDbType = SqlDbType.Decimal
		'sparam4.Direction = ParameterDirection.Output
		'MySqlCommand.Parameters.Add(sparam4)


		Dim Reader As SqlClient.SqlDataReader
		MySqlCommand.Parameters("@IdStorico").Value = IntIdStorico
		Reader = MySqlCommand.ExecuteReader
		EseguiStoreStoricoProgetti = Reader
		'If Not Reader Is Nothing Then
		'    Reader.Close()
		'    Reader = Nothing
		'End If
	End Function
	Public Shared Function EseguiStoreStoricoProgrammi(ByVal IntIdStorico As Integer, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection) As SqlClient.SqlDataReader

		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand
		MySqlCommand.CommandType = CommandType.StoredProcedure
		MySqlCommand.CommandText = StrStoreProcedure
		MySqlCommand.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@idStorico"
		sparam.SqlDbType = SqlDbType.Int
		MySqlCommand.Parameters.Add(sparam)

		'Dim sparam1 As SqlClient.SqlParameter
		'sparam1 = New SqlClient.SqlParameter
		'sparam1.ParameterName = "@punteggiofinale"
		'sparam1.SqlDbType = SqlDbType.Int
		'sparam1.Direction = ParameterDirection.Output
		'MySqlCommand.Parameters.Add(sparam1)

		'Dim sparam2 As SqlClient.SqlParameter
		'sparam2 = New SqlClient.SqlParameter
		'sparam2.ParameterName = "@punteggioCP"
		'sparam2.SqlDbType = SqlDbType.Decimal
		'sparam2.Direction = ParameterDirection.Output
		'MySqlCommand.Parameters.Add(sparam2)

		'Dim sparam3 As SqlClient.SqlParameter
		'sparam3 = New SqlClient.SqlParameter
		'sparam3.ParameterName = "@punteggioCKH"
		'sparam3.SqlDbType = SqlDbType.Decimal
		'sparam3.Direction = ParameterDirection.Output
		'MySqlCommand.Parameters.Add(sparam3)

		'Dim sparam4 As SqlClient.SqlParameter
		'sparam4 = New SqlClient.SqlParameter
		'sparam4.ParameterName = "@punteggioCO"
		'sparam4.SqlDbType = SqlDbType.Decimal
		'sparam4.Direction = ParameterDirection.Output
		'MySqlCommand.Parameters.Add(sparam4)


		Dim Reader As SqlClient.SqlDataReader
		MySqlCommand.Parameters("@IdStorico").Value = IntIdStorico
		Reader = MySqlCommand.ExecuteReader
		EseguiStoreStoricoProgrammi = Reader
		'If Not Reader Is Nothing Then
		'    Reader.Close()
		'    Reader = Nothing
		'End If
	End Function
	Public Shared Function CreaDatareader(ByVal StrSqlGrid As String, ByVal conn As SqlConnection, Optional ByVal CheckOperazione As Integer = 0, Optional ByVal transazione As System.Data.SqlClient.SqlTransaction = Nothing) As SqlDataReader 'riempe la griglia passatagli
		Dim comando As SqlCommand
		Try
			If transazione Is Nothing Then
				comando = New SqlCommand(StrSqlGrid, conn)
			Else
				comando = New SqlCommand(StrSqlGrid, conn, transazione)
			End If
			comando.CommandTimeout = 300
			CreaDatareader = comando.ExecuteReader()
			'connessione.Close()
		Catch es As Exception
			Throw New Exception(es.Message.ToString)
		End Try
	End Function
	Public Shared Function CreaDataTable(ByVal Query As String, ByVal FirstrowBlank As Boolean, ByVal conn As SqlConnection) As System.Data.DataTable
		Dim MyTable As New System.Data.DataTable
		Dim MyRow As DataRow
		Dim Rows As DataRowCollection
		Dim DtrGenerico As System.Data.SqlClient.SqlDataReader

		DtrGenerico = CreaDatareader(Query, conn)
		Rows = DtrGenerico.GetSchemaTable.Rows

		For Each MyRow In Rows
			Dim MyCol As New DataColumn
			MyCol.ColumnName = MyRow("ColumnName").ToString
			MyCol.Unique = System.Convert.ToBoolean(MyRow("IsUnique"))
			MyCol.AllowDBNull = System.Convert.ToBoolean(MyRow("AllowDBNull"))
			MyCol.ReadOnly = System.Convert.ToBoolean(MyRow("IsReadOnly"))
			MyCol.DataType = Type.GetType(MyRow("DataType").ToString)
			MyTable.Columns.Add(MyCol)
		Next

		If FirstrowBlank = True Then
			Dim MyCol As DataColumn
			MyRow = MyTable.NewRow
			For Each MyCol In MyTable.Columns
				If MyCol.DataType.ToString = "System.Int32" Then
					MyRow(MyCol) = 0
				Else
					MyRow(MyCol) = ""
				End If
			Next
			MyTable.Rows.Add(MyRow)
		End If

		While DtrGenerico.Read
			Dim MyCol As DataColumn
			MyRow = MyTable.NewRow
			For Each MyCol In MyTable.Columns
				MyRow(MyCol) = DtrGenerico(MyCol.ColumnName)
			Next
			MyTable.Rows.Add(MyRow)
		End While
		DtrGenerico.Close()
		DtrGenerico = Nothing
		Return MyTable
	End Function

    Public Shared Function CreaDataTableInTransazione(ByVal Query As String, ByVal FirstrowBlank As Boolean, ByVal conn As SqlConnection, ByVal trans As SqlTransaction) As System.Data.DataTable
        Dim MyTable As New System.Data.DataTable
        Dim MyRow As DataRow
        Dim Rows As DataRowCollection
        Dim DtrGenerico As System.Data.SqlClient.SqlDataReader

        DtrGenerico = CreaDatareader(Query, conn, transazione:=trans)
        Rows = DtrGenerico.GetSchemaTable.Rows

        For Each MyRow In Rows
            Dim MyCol As New DataColumn
            MyCol.ColumnName = MyRow("ColumnName").ToString
            MyCol.Unique = System.Convert.ToBoolean(MyRow("IsUnique"))
            MyCol.AllowDBNull = System.Convert.ToBoolean(MyRow("AllowDBNull"))
            MyCol.ReadOnly = System.Convert.ToBoolean(MyRow("IsReadOnly"))
            MyCol.DataType = Type.GetType(MyRow("DataType").ToString)
            MyTable.Columns.Add(MyCol)
        Next

        If FirstrowBlank = True Then
            Dim MyCol As DataColumn
            MyRow = MyTable.NewRow
            For Each MyCol In MyTable.Columns
                If MyCol.DataType.ToString = "System.Int32" Then
                    MyRow(MyCol) = 0
                Else
                    MyRow(MyCol) = ""
                End If
            Next
            MyTable.Rows.Add(MyRow)
        End If

        While DtrGenerico.Read
            Dim MyCol As DataColumn
            MyRow = MyTable.NewRow
            For Each MyCol In MyTable.Columns
                MyRow(MyCol) = DtrGenerico(MyCol.ColumnName)
            Next
            MyTable.Rows.Add(MyRow)
        End While
        DtrGenerico.Close()
        DtrGenerico = Nothing
        Return MyTable
    End Function

	Public Shared Function DataSetGenerico(ByVal QuerySql As String, ByVal conn As SqlClient.SqlConnection) As DataSet
		Dim CMD As New SqlDataAdapter(QuerySql, conn)
		Dim DstPrimario As New DataSet
		CMD.Fill(DstPrimario)
		DataSetGenerico = DstPrimario
		DstPrimario = Nothing
	End Function

	Public Shared Function CreaMax(ByVal StrTabella As String, ByVal StrCampo As String, ByVal conn As SqlConnection) As Double
		'crata da Alessandra Taballione il 5/02/2004
		'funzione che restituisce il max dell' ID delle tabelle
		Dim dtrCreaMax As SqlClient.SqlDataReader
		dtrCreaMax = CreaDatareader("Select max(" & StrCampo & " )as Maxid from " & StrTabella & "", conn)
		dtrCreaMax.Read()
		If dtrCreaMax.HasRows = True Then
			CreaMax = dtrCreaMax("Maxid") + 1
		Else
			CreaMax = 1
		End If
		If Not dtrCreaMax Is Nothing Then
			dtrCreaMax.Close()
			dtrCreaMax = Nothing
		End If
	End Function
	Public Shared Function trovaId(ByVal StrTabella As String, ByVal StrCampo As String, ByVal strcondizione1 As String, ByVal strcondizione2 As String, ByVal conn As SqlConnection) As Double
		'crata da Alessandra Taballione il 5/02/2004
		'funzione che restituisce il max dell' ID delle tabelle
		Dim dtrCreaMax As SqlClient.SqlDataReader
		dtrCreaMax = CreaDatareader("Select " & StrCampo & " as id from " & StrTabella & " where " & strcondizione1 & "= '" & Replace(strcondizione2, "'", "''") & "'", conn)
		If dtrCreaMax.HasRows = True Then
			dtrCreaMax.Read()
			trovaId = dtrCreaMax("id")
		End If
		If Not dtrCreaMax Is Nothing Then
			dtrCreaMax.Close()
			dtrCreaMax = Nothing
		End If
	End Function
	Public Shared Function LeggiStore(ByVal IntSede As Integer, ByVal conn As SqlClient.SqlConnection) As Integer
		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_CONTA_SEDI"
		CustOrderHist.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(sparam)

		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Valore"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam1)

		Dim Reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IntSede
		Reader = CustOrderHist.ExecuteReader()
		' Insert code to read through the datareader.
		'If CustOrderHist.Parameters("@Valore").Value = 0 Then
		LeggiStore = CustOrderHist.Parameters("@Valore").Value
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
		'End If
	End Function

	Public Shared Function EseguiStoreReader(ByVal IntEnte As Integer, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection) As SqlClient.SqlDataReader
		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand
		MySqlCommand.CommandType = CommandType.StoredProcedure
		MySqlCommand.CommandText = StrStoreProcedure
		MySqlCommand.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		MySqlCommand.Parameters.Add(sparam)

		Dim Reader As SqlClient.SqlDataReader
		MySqlCommand.Parameters("@IdEnte").Value = IntEnte
		Reader = MySqlCommand.ExecuteReader
		EseguiStoreReader = Reader
		If Not Reader Is Nothing Then
			'Reader.Close()
			'Reader = Nothing
		End If
	End Function

	Public Shared Function LeggiStorejonathan(ByVal IdEntePersonale As Integer, ByVal conn As SqlClient.SqlConnection) As Integer

		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_VINCOLO_MINIMO_TUTOR_PER_PROVINCIA"
		CustOrderHist.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(sparam)

		'Dim sparam2 As SqlClient.SqlParameter
		'sparam2 = New SqlClient.SqlParameter
		'sparam2.ParameterName = "@IdEntePersonaleRuolo"
		'sparam2.SqlDbType = SqlDbType.Int
		'CustOrderHist.Parameters.Add(sparam2)

		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Valore"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam1)

		Dim Reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = 44
		'CustOrderHist.Parameters("@IdEntePersonaleRuolo").Value = Nothing
		Reader = CustOrderHist.ExecuteReader()
		' Insert code to read through the datareader.
		'If CustOrderHist.Parameters("@Valore").Value = 0 Then
		LeggiStorejonathan = CustOrderHist.Parameters("@Valore").Value
		'End If
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
	End Function
	Public Shared Function CreatePdf(ByVal NomeReport As String, ByVal StrDati As String, ByVal MySess As System.Web.SessionState.HttpSessionState, Optional ByVal SottoReport As String = "", Optional ByVal ReportStorico As Int16 = 1) As String
		'*************************************************************************************************
		'DESCRIZIONE: Genera il PDF nella directory Reports/Export del report selezionato
		'AUTORE: TESTA GUIDO    DATA: 04/10/2004
		'*************************************************************************************************
		Dim paramFieldDt As New ParameterField
		Dim discreteValDt As New ParameterDiscreteValue
		Dim myPath As New System.Web.UI.Page
		Dim crReportdocument As New ReportDocument
		Dim logOnInfo As New TableLogOnInfo
		Dim NameReportNew As String
		Dim i As Integer
		Dim sGruppo() As String         'matrice parametri/valori
		Dim sGruppo1() As String        'matrice sottoreport
		Dim sElemt() As String

		GetPdfError = ""

		NameReportNew = UCase(MySess.SessionID) & "-" & Format(Now, "dd-MM-yyyyhh-mm-ss")

		Try
			crReportdocument.Load(myPath.Server.MapPath("reports/" & NomeReport))

			sGruppo = Split(StrDati, ":")
			'1)parametri report*****************************
			For i = 0 To UBound(sGruppo) - 1
				sElemt = Split(sGruppo(i), ",")
				paramFieldDt.ParameterFieldName = "@" & sElemt(0)       'nome campo
				discreteValDt.Value = sElemt(1)                           'valore campo
				paramFieldDt.CurrentValues.Add(discreteValDt)

				Dim paramFieldDefDt As ParameterFieldDefinition = crReportdocument.DataDefinition.ParameterFields.Item(sElemt(0))

				Dim ParameterValuesDt As ParameterValues = paramFieldDt.CurrentValues
				paramFieldDefDt.ApplyCurrentValues(ParameterValuesDt)
			Next i
			'******************************************

			'2)parametri connessione*********************
			logOnInfo = crReportdocument.Database.Tables.Item(0).LogOnInfo

			If ReportStorico = 1 Then
				If ReportStorico = 1 Then
					With logOnInfo
						.ConnectionInfo.Password = AppSettings("DB_PASSWORD")
						.ConnectionInfo.ServerName = AppSettings("DB_DATA_SOURCE")
						.ConnectionInfo.DatabaseName = AppSettings("DB_NAME")
						.ConnectionInfo.UserID = AppSettings("DB_USERNAME")
					End With

				End If

			End If

			crReportdocument.Database.Tables(0).ApplyLogOnInfo(logOnInfo)
			'******************************************

			'3)gestione sotto report*********************
			sGruppo1 = Split(SottoReport, ":")
			i = 0
			For i = 0 To UBound(sGruppo1) - 1
				crReportdocument.OpenSubreport(sGruppo1(i)).Database.Tables(0).ApplyLogOnInfo(logOnInfo)
			Next i
			'******************************************


			'4)esportazione report in PDF***************
			Dim crDiskFileDestinationOptions As New CrystalDecisions.Shared.DiskFileDestinationOptions
			Dim crExportOptions As CrystalDecisions.Shared.ExportOptions


			crDiskFileDestinationOptions.DiskFileName = myPath.Server.MapPath("reports/export/" & NameReportNew & ".pdf")
			crExportOptions = crReportdocument.ExportOptions
			crExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
			crExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
			crExportOptions.DestinationOptions = crDiskFileDestinationOptions


			crReportdocument.Export()
			crReportdocument.Close()
			Return "reports/export/" & NameReportNew & ".pdf"

			'******************************************


		Catch ex As Exception
			GetPdfError = ex.Message
		End Try
	End Function

	Public Shared Function CreaChiave(ByVal pNomeTabella As String, ByVal pNomeCampoChiave As String, ByVal pConn As SqlClient.SqlConnection) As Long
		'crata da Michele d'Ascenzio il 22/10/2004
		'funzione che restituisce la prima chiave disponibile in una tabella senza contatore
		Dim dtrUltimoInserito As SqlClient.SqlDataReader

		dtrUltimoInserito = CreaDatareader("SELECT MAX(" & pNomeCampoChiave & ") as Id FROM " & pNomeTabella, pConn)
		dtrUltimoInserito.Read()
		If IsDBNull(dtrUltimoInserito("Id")) = True Then
			CreaChiave = 1
		Else
			CreaChiave = dtrUltimoInserito("Id") + 1
		End If

		If Not dtrUltimoInserito Is Nothing Then
			dtrUltimoInserito.Close()
			dtrUltimoInserito = Nothing
		End If

	End Function
	Public Shared Function CaricaDataTablePerStampa(ByVal DataSetdaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String) As DataTable
		Dim dt As New DataTable
		Dim dr As DataRow
		Dim i As Integer
		Dim x As Integer
		'carico i nomi delle colonne che andrò a stampare nella datagrid
		For x = 0 To NColonne
			dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
		Next
		'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
		If DataSetdaScorrere.Tables(0).Rows.Count > 0 Then
			For i = 1 To DataSetdaScorrere.Tables(0).Rows.Count
				dr = dt.NewRow()
				For x = 0 To NColonne
					dr(x) = DataSetdaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
				Next
				dt.Rows.Add(dr)
			Next
		End If
		'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
		CaricaDataTablePerStampa = dt
	End Function

	Public Shared Function EseguiStoreRicorsoProgetto(ByVal idAttivita As Integer, ByVal sUserID As String, ByVal conn As SqlClient.SqlConnection) As String
		'AUTORE: Testa Guido
		'DESCRIZIONE: richiamo la SP per la revisione dei progetti
		'DATA: 13/06/2006

		Dim Reader As SqlClient.SqlDataReader

		Try
			Dim sReturnValue As String
			Dim MyCommand As New SqlClient.SqlCommand
			MyCommand.CommandType = CommandType.StoredProcedure
			MyCommand.CommandText = "SP_RICORSO_PROGETTO"
			MyCommand.Connection = conn


			'PRIMO PARAMEtrO IDATTIVITA'
			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IDAttivita"
			sparam.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam)

			'SECONDO PARAMEtrO USERNAME
			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@UserName"
			sparam1.SqlDbType = SqlDbType.VarChar
			MyCommand.Parameters.Add(sparam1)

			'TERZO PARAMEtrO OUTPUT
			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@esito"
			sparam2.Size = 20
			sparam2.SqlDbType = SqlDbType.NVarChar
			sparam2.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam2)



			MyCommand.Parameters("@IDAttivita").Value = idAttivita
			MyCommand.Parameters("@UserName").Value = sUserID

			Reader = MyCommand.ExecuteReader()

			sReturnValue = MyCommand.Parameters("@esito").Value

			Reader.Close()
			Reader = Nothing

			Return sReturnValue

		Catch ex As Exception
			If Not Reader Is Nothing Then
				Reader.Close()
				Reader = Nothing
			End If
			Return "ERRORE"
		End Try
	End Function

	Public Shared Function EseguiStoreRevisioneProegtti(ByVal idAttivita As Integer, ByVal sUserID As String, ByVal conn As SqlClient.SqlConnection) As String
		'AUTORE: Testa Guido
		'DESCRIZIONE: richiamo la SP per la revisione dei progetti
		'DATA: 13/06/2006

		Dim Reader As SqlClient.SqlDataReader

		Try
			Dim sReturnValue As String
			Dim MyCommand As New SqlClient.SqlCommand
			MyCommand.CommandType = CommandType.StoredProcedure
			MyCommand.CommandText = "SP_REVISIONE_PROGETTO"
			MyCommand.Connection = conn


			'PRIMO PARAMEtrO IDATTIVITA'
			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IDAttivita"
			sparam.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam)

			'SECONDO PARAMEtrO USERNAME
			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@UserName"
			sparam1.SqlDbType = SqlDbType.VarChar
			MyCommand.Parameters.Add(sparam1)

			'TERZO PARAMEtrO OUTPUT
			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@esito"
			sparam2.Size = 20
			sparam2.SqlDbType = SqlDbType.NVarChar
			sparam2.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam2)



			MyCommand.Parameters("@IDAttivita").Value = idAttivita
			MyCommand.Parameters("@UserName").Value = sUserID

			Reader = MyCommand.ExecuteReader()

			sReturnValue = MyCommand.Parameters("@esito").Value

			Reader.Close()
			Reader = Nothing

			Return sReturnValue

		Catch ex As Exception
			If Not Reader Is Nothing Then
				Reader.Close()
				Reader = Nothing
			End If
			Return "ERRORE"
		End Try
	End Function

	Public Shared Function EseguiStoreCancellaGraduatoria(ByVal idAttivita As Integer, ByVal idAttivitaSede As Integer, ByVal conn As SqlClient.SqlConnection) As String
		'AUTORE: Testa Guido
		'DESCRIZIONE: richiamo la SP per la cancellazione della graduatoria volontari
		'DATA: 13/09/2006

		Dim Reader As SqlClient.SqlDataReader

		Try
			Dim sReturnValue As String
			Dim MyCommand As New SqlClient.SqlCommand
			MyCommand.CommandType = CommandType.StoredProcedure
			MyCommand.CommandText = "SP_ELIMINA_GRADUATORIA"
			MyCommand.Connection = conn


			'PRIMO PARAMEtrO IDATTIVITA'
			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IDAttivita"
			sparam.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam)

			'SECONDO PARAMEtrO ATTIVITÀSEDE
			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@IDAttivitaSede"
			sparam1.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam1)

			'TERZO PARAMEtrO OUTPUT
			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@esito"
			sparam2.Size = 20
			sparam2.SqlDbType = SqlDbType.NVarChar
			sparam2.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam2)



			MyCommand.Parameters("@IDAttivita").Value = idAttivita
			MyCommand.Parameters("@IDAttivitaSede").Value = idAttivitaSede

			Reader = MyCommand.ExecuteReader()

			sReturnValue = MyCommand.Parameters("@esito").Value

			Reader.Close()
			Reader = Nothing

			Return sReturnValue

		Catch ex As Exception
			If Not Reader Is Nothing Then
				Reader.Close()
				Reader = Nothing
			End If
			Return "ERRORE"
		End Try
	End Function
	Public Shared Function EseguiStoreCancellaEntita(ByVal IDentita As Integer, ByVal UserName As String, ByVal conn As SqlClient.SqlConnection) As String
		'AUTORE: Antonello Di Croce
		'DESCRIZIONE: richiamo la SP per la cancellazione dei volontari in stato registrato da una graduatoria
		'DATA: 05/01/2007
		Dim Reader As SqlClient.SqlDataReader

		Try

			Dim sReturnValue As Integer
			Dim MyCommand As New SqlClient.SqlCommand
			MyCommand.CommandType = CommandType.StoredProcedure
			MyCommand.CommandText = "SP_ELIMINA_ENTITA"
			MyCommand.Connection = conn

			'PRIMO PARAMEtrO IDentita
			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IDentita"
			sparam.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam)

			'SECONDO PARAMEtrO UserName
			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@USERNAME"
			sparam1.SqlDbType = SqlDbType.NVarChar
			MyCommand.Parameters.Add(sparam1)



			'PARAMEtrO OUTPUT
			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@Valore"
			sparam2.Size = 20
			sparam2.SqlDbType = SqlDbType.Int
			sparam2.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam2)



			MyCommand.Parameters("@IDentita").Value = IDentita
			MyCommand.Parameters("@USERNAME").Value = UserName

			Reader = MyCommand.ExecuteReader()

			sReturnValue = MyCommand.Parameters("@Valore").Value

			Reader.Close()
			Reader = Nothing

			Return sReturnValue

		Catch ex As Exception
			If Not Reader Is Nothing Then
				Reader.Close()
				Reader = Nothing
			End If
			Return 1
		End Try
	End Function

	Public Shared Function EseguiStoreAccorpamentoProgettiCoProgettati(ByVal strAttivitaCapoFila As String, ByVal strIdAttivitaDaAccorpare As String, ByVal strUtente As String, ByVal conn As SqlClient.SqlConnection) As String
		'richiamo alla store che accorpa 
		'fatta da Jean Connery
		'i progetti coprogettati
		'DATA: 10/01/2007

		Dim Reader As SqlClient.SqlDataReader

		Try
			Dim sReturnValue As String
			Dim MyCommand As New SqlClient.SqlCommand
			MyCommand.CommandType = CommandType.StoredProcedure
			MyCommand.CommandText = "SP_COPROGETTAZIONE"
			MyCommand.Connection = conn


			'PRIMO PARAMEtrO IDATTIVITACAPOFILA
			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IDATTIVITACAPO"
			sparam.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam)

			'SECONDO PARAMEtrO IDATTIVITÀDAACCORPARE
			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@IDATTIVITAFIGLIO"
			sparam1.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam1)

			'SECONDO PARAMEtrO USERNAME
			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@USERNAME"
			sparam2.SqlDbType = SqlDbType.VarChar
			MyCommand.Parameters.Add(sparam2)

			'TERZO PARAMEtrO OUTPUT
			Dim sparam3 As SqlClient.SqlParameter
			sparam3 = New SqlClient.SqlParameter
			sparam3.ParameterName = "@esito"
			sparam3.Size = 20
			sparam3.SqlDbType = SqlDbType.NVarChar
			sparam3.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam3)



			MyCommand.Parameters("@IDATTIVITACAPO").Value = strAttivitaCapoFila
			MyCommand.Parameters("@IDATTIVITAFIGLIO").Value = strIdAttivitaDaAccorpare
			MyCommand.Parameters("@USERNAME").Value = strUtente


			Reader = MyCommand.ExecuteReader()

			sReturnValue = MyCommand.Parameters("@esito").Value

			Reader.Close()
			Reader = Nothing

			Return sReturnValue

		Catch ex As Exception
			If Not Reader Is Nothing Then
				Reader.Close()
				Reader = Nothing
			End If
			Return "ERRORE"
		End Try
	End Function
	Public Shared Function EseguiScorporoSediStored(ByVal A As Integer, ByVal B As String, ByVal conn As SqlClient.SqlConnection) As String()
		'AUTORE: Antonello Di Croce  
		'DESCRIZIONE: richiamo la SP_SCORPOROSEDE per lo scorporo delle sedi di attuazione
		'DATA: 01/04/2008
		Dim Reader As SqlClient.SqlDataReader

		Try
			Dim x As String
			Dim y As String
			Dim ArreyOutPut(1) As String


			Dim MyCommand As New SqlClient.SqlCommand
			MyCommand.CommandType = CommandType.StoredProcedure
			MyCommand.CommandText = "SP_SCORPOROSEDE"
			MyCommand.Connection = conn

			'PRIMO PARAMEtrO A=IdEnteSedeAttuazione INPUT
			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IdEnteSedeAttuazione"
			sparam.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam)

			'SECONDO PARAMEtrO B=Username INPUT
			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@Username"
			sparam1.SqlDbType = SqlDbType.NVarChar
			MyCommand.Parameters.Add(sparam1)



			'PARAMEtrO 1=esito OUTPUT
			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@ESITO"
			sparam2.SqlDbType = SqlDbType.Int
			sparam2.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam2)


			'PARAMEtrO 2=motivazione OUTPUT
			Dim sparam3 As SqlClient.SqlParameter
			sparam3 = New SqlClient.SqlParameter
			sparam3.ParameterName = "@Motivazione"
			sparam3.Size = 1000
			sparam3.SqlDbType = SqlDbType.NVarChar
			sparam3.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam3)


			MyCommand.Parameters("@IdEnteSedeAttuazione").Value = A
			MyCommand.Parameters("@Username").Value = B

			Reader = MyCommand.ExecuteReader()

			x = CStr(MyCommand.Parameters("@Esito").Value)
			y = MyCommand.Parameters("@Motivazione").Value


			ArreyOutPut(0) = x
			ArreyOutPut(1) = y

			Reader.Close()
			Reader = Nothing

			Return ArreyOutPut

		Catch ex As Exception
			If Not Reader Is Nothing Then
				Reader.Close()
				Reader = Nothing
			End If
			Dim ArreyOutPut1(1) As String
			ArreyOutPut1(0) = "0"
			ArreyOutPut1(1) = "Contattare l'assistenza Helios/Futuro"
			Return ArreyOutPut1

		End Try
	End Function
	Public Shared Function LeggiStoreSediPartner(ByVal IntSede As Integer, ByVal conn As SqlClient.SqlConnection) As Integer
		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_CONTA_SEDI_PARTNER"
		CustOrderHist.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(sparam)

		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Valore"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam1)

		Dim Reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IntSede
		Reader = CustOrderHist.ExecuteReader()
		' Insert code to read through the datareader.
		'If CustOrderHist.Parameters("@Valore").Value = 0 Then
		LeggiStoreSediPartner = CustOrderHist.Parameters("@Valore").Value
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
		'End If
	End Function
	Public Shared Function LeggiStoreMaxSediPartner(ByVal IntSede As Integer, ByVal conn As SqlClient.SqlConnection) As Integer
		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_MAX_SEDI_PARTNER"
		CustOrderHist.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(sparam)

		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Valore"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam1)

		Dim Reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IntSede
		Reader = CustOrderHist.ExecuteReader()
		' Insert code to read through the datareader.
		'If CustOrderHist.Parameters("@Valore").Value = 0 Then
		LeggiStoreMaxSediPartner = CustOrderHist.Parameters("@Valore").Value
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
		'End If
	End Function
	Public Shared Function LeggiStoreSediPartner_Italia(ByVal IntSede As Integer, ByVal conn As SqlClient.SqlConnection) As Integer
		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_CONTA_SEDI_PARTNER_ITALIA"
		CustOrderHist.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(sparam)

		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Valore"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam1)

		Dim Reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IntSede
		Reader = CustOrderHist.ExecuteReader()
		' Insert code to read through the datareader.
		'If CustOrderHist.Parameters("@Valore").Value = 0 Then
		LeggiStoreSediPartner_Italia = CustOrderHist.Parameters("@Valore").Value
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
		'End If
	End Function
	Public Shared Function LeggiStore_Italia(ByVal IntSede As Integer, ByVal conn As SqlClient.SqlConnection) As Integer
		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_CONTA_SEDI_ITALIA"
		CustOrderHist.Connection = conn

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(sparam)

		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Valore"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam1)

		Dim Reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IntSede
		Reader = CustOrderHist.ExecuteReader()
		' Insert code to read through the datareader.
		'If CustOrderHist.Parameters("@Valore").Value = 0 Then
		LeggiStore_Italia = CustOrderHist.Parameters("@Valore").Value
		If Not Reader Is Nothing Then
			Reader.Close()
			Reader = Nothing
		End If
		'End If
	End Function
	Public Shared Function EseguiPresenzeINS(ByVal A As Integer, ByVal B As Integer, ByVal C As Date, ByVal D As String, ByVal E As Integer, ByVal connessione As SqlClient.SqlConnection) As String()
		'AUTORE: Antonello Di Croce  
		'DESCRIZIONE: richiamo la SP_PRESENZE_INSERT 
		'DATA: 03/03/2015
		Dim Reader As SqlClient.SqlDataReader
		Dim C1 As String
		C1 = Format(C, "dd/MM/yyyy")

		Try
			Dim x As String
			Dim y As String
			Dim ArreyOutPut(1) As String


			Dim MyCommand As New SqlClient.SqlCommand
			MyCommand.CommandType = CommandType.StoredProcedure
			MyCommand.CommandText = "SP_PRESENZE_INSERT"
			MyCommand.Connection = connessione

			'PRIMO PARAMEtrO INPUT A
			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IdEntita"
			sparam.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam)

			'PRIMO PARAMEtrO INPUT B
			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@IdCausalePresenza"
			sparam1.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam1)

			'SECONDO PARAMEtrO  INPUT C
			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@Giorno"
			sparam2.SqlDbType = SqlDbType.DateTime
			MyCommand.Parameters.Add(sparam2)


			'SECONDO PARAMEtrO INPUT D
			Dim sparam3 As SqlClient.SqlParameter
			sparam3 = New SqlClient.SqlParameter
			sparam3.ParameterName = "@Username"
			sparam3.SqlDbType = SqlDbType.NVarChar
			MyCommand.Parameters.Add(sparam3)



			'SECONDO PARAMEtrO  INPUT E
			Dim sparam4 As SqlClient.SqlParameter
			sparam4 = New SqlClient.SqlParameter
			sparam4.ParameterName = "@IdEnte"
			sparam4.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam4)


			'PARAMEtrO 1=esito OUTPUT
			Dim sparam5 As SqlClient.SqlParameter
			sparam5 = New SqlClient.SqlParameter
			sparam5.ParameterName = "@Esito"
			sparam5.Size = 10
			sparam5.SqlDbType = SqlDbType.VarChar
			sparam5.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam5)


			'PARAMEtrO 2=Messaggio OUTPUT
			Dim sparam6 As SqlClient.SqlParameter
			sparam6 = New SqlClient.SqlParameter
			sparam6.ParameterName = "@Messaggio"
			sparam6.Size = 255
			sparam6.SqlDbType = SqlDbType.NVarChar
			sparam6.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam6)


			MyCommand.Parameters("@IdEntita").Value = A
			MyCommand.Parameters("@IdCausalePresenza").Value = B
			MyCommand.Parameters("@Giorno").Value = C1
			MyCommand.Parameters("@Username").Value = D
			MyCommand.Parameters("@IdEnte").Value = E



			Reader = MyCommand.ExecuteReader()

			x = CStr(MyCommand.Parameters("@Esito").Value)
			y = MyCommand.Parameters("@Messaggio").Value


			ArreyOutPut(0) = x
			ArreyOutPut(1) = y

			Reader.Close()
			Reader = Nothing

			Return ArreyOutPut

		Catch ex As Exception
			If Not Reader Is Nothing Then
				Reader.Close()
				Reader = Nothing
			End If
			Dim ArreyOutPut1(1) As String
			ArreyOutPut1(0) = "0"
			ArreyOutPut1(1) = "Contattare l'assistenza Helios/Futuro"
			Return ArreyOutPut1

		End Try
	End Function
	Public Shared Function ConfermaDocumento(ByVal Utente As String, ByVal IdEntitàDocumento As Integer, ByVal Stato As Integer, ByVal Documento As String, ByVal IdEnte As Integer, ByVal connessione As SqlClient.SqlConnection, Optional ByRef msg As String = "", Optional ByRef Esito As String = "") As String
		'REALIZZATA DA: SIMONA CORDELLA 
		'DATA REALIZZAZIONE:  27/04/2015
		'FUNZIONALITA': RICHIAMO STORE PER LA VALIDITA' DEI DOCUMENTI
		Dim sqlCMD As New SqlClient.SqlCommand
		Dim strNomeStore As String = "[SP_VOLONTARI_VALIDA_DOCUMENTI]"


		Try
			sqlCMD = New SqlClient.SqlCommand(strNomeStore, connessione)
			sqlCMD.CommandType = CommandType.StoredProcedure
			sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Utente
			sqlCMD.Parameters.Add("@IdEntitàDocumento", SqlDbType.Int).Value = IdEntitàDocumento
			sqlCMD.Parameters.Add("@Stato", SqlDbType.Int).Value = Stato
			sqlCMD.Parameters.Add("@Documento", SqlDbType.VarChar).Value = Documento
			sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte

			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@Esito"
			sparam1.Size = 100
			sparam1.SqlDbType = SqlDbType.NVarChar
			sparam1.Direction = ParameterDirection.Output
			sqlCMD.Parameters.Add(sparam1)

			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@Messaggio"
			sparam2.Size = 100
			sparam2.SqlDbType = SqlDbType.NVarChar
			sparam2.Direction = ParameterDirection.Output
			sqlCMD.Parameters.Add(sparam2)


			sqlCMD.ExecuteScalar()
			Dim str As String
			msg = sqlCMD.Parameters("@Messaggio").Value
			Esito = sqlCMD.Parameters("@Esito").Value
			'Return str

		Catch ex As Exception

			Exit Function
		End Try
	End Function

	Public Shared Sub ChiudiFaseEnte(ByVal IdEnteFase As Integer, ByVal Utente As String, ByVal connessione As SqlClient.SqlConnection, Optional ByRef msg As String = "", Optional ByRef Esito As Integer = 0)

		Dim SqlCmd As New SqlClient.SqlCommand
		Try
			SqlCmd.CommandText = "SP_ACCREDITAMENTO_CHIUDI_FASE_ENTE"
			SqlCmd.CommandType = CommandType.StoredProcedure
			SqlCmd.Connection = connessione

			SqlCmd.Parameters.Add("@IdEnteFase", SqlDbType.Int).Value = IdEnteFase
			SqlCmd.Parameters.Add("@UserNameRichiesta ", SqlDbType.VarChar).Value = Utente
			SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
			SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

			SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
			SqlCmd.Parameters("@messaggio").Size = 1000
			SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

			SqlCmd.ExecuteNonQuery()

			msg = SqlCmd.Parameters("@messaggio").Value
			Esito = SqlCmd.Parameters("@Esito").Value
		Catch ex As Exception

		End Try
	End Sub
	Public Shared Function CreatePdfZip(ByVal NomeReport As String, ByVal StrDati As String, ByVal MySess As System.Web.SessionState.HttpSessionState, ByVal SqlConn As SqlConnection, ByVal idcorso As String, Optional ByVal SottoReport As String = "", Optional ByVal ReportStorico As Int16 = 1) As String
		'*************************************************************************************************
		'DESCRIZIONE: Genera il PDF nella directory Reports/Export del report selezionato
		'AUTORE: TESTA GUIDO    DATA: 04/10/2004
		'*************************************************************************************************
		Dim DtrParametri As System.Data.SqlClient.SqlDataReader
		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand
		Dim strsql As String
		Dim Nome As String
		Dim Cognome As String

		Dim paramFieldDt As New ParameterField
		Dim discreteValDt As New ParameterDiscreteValue
		Dim myPath As New System.Web.UI.Page
		Dim crReportdocument As New ReportDocument
		Dim logOnInfo As New TableLogOnInfo
		Dim NameReportNew As String
		Dim x As Integer
		Dim i As Integer
		Dim sGruppo() As String         'matrice parametri/valori
		Dim sGruppo1() As String        'matrice sottoreport
		Dim sElemt() As String
		Dim zip As ZipFile = New ZipFile()
		Dim IdCorsoFormazioneOLPDettaglio As Integer
		GetPdfError = ""

		Dim Parametri As SqlClient.SqlParameter
		strsql = "select CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLPDettaglio,CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP,Cognome,Nome,EnteRiferimento,LuogoSvolgimento,DataSvolgimentoCorso,NumeroOre,case convert(varchar,CorsiFormazioneOLP.StatoRichiesta) when '1' then 'Registrata' when '2' then 'Approvata' when '3' then 'Respinta' end as StatoRichiesta  from CorsiFormazioneOLPDettaglio "
		strsql = strsql & " inner join CorsiFormazioneOLP on CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP=CorsiFormazioneOLP.IdCorsoFormazioneOLP Where CorsiFormazioneOLPDettaglio.NumeroOre>=8 and CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP=" & idcorso & " Order by  1 desc "
		'strsql = "select *  from CorsiFormazioneOLPDettaglio  Where IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso") & " Order by  1 desc "

		DtrParametri = CreaDatareader(strsql, SqlConn)
		Do While DtrParametri.Read


			Nome = DtrParametri.Item("Nome")
			Cognome = DtrParametri.Item("Cognome")
			IdCorsoFormazioneOLPDettaglio = DtrParametri.Item("IdCorsoFormazioneOLPDettaglio")
			'StrDati = myTable.Rows(0).Item(0).ToString
			'crReportdocument.Load(myPath.Server.MapPath("reports/" & NomeReport))

			sGruppo = Split(StrDati, ":")
			'1)parametri report*****************************
			' For i = 0 To UBound(sGruppo) - 1

			crReportdocument.Load(myPath.Server.MapPath("reports/" & NomeReport))
			NameReportNew = Nome & " " & Cognome & "-" & Format(Now, "dd-MM-yyyyhh-mm-ss")
			sElemt = Split(sGruppo(i), ",")
			paramFieldDt.ParameterFieldName = "@" & sElemt(0)       'nome campo
			discreteValDt.Value = IdCorsoFormazioneOLPDettaglio 'sElemt(1)                           'valore campo
			paramFieldDt.CurrentValues.Add(discreteValDt)

			Dim paramFieldDefDt As ParameterFieldDefinition = crReportdocument.DataDefinition.ParameterFields.Item(sElemt(0))

			Dim ParameterValuesDt As ParameterValues = paramFieldDt.CurrentValues
			paramFieldDefDt.ApplyCurrentValues(ParameterValuesDt)

			'******************************************

			'2)parametri connessione*********************
			logOnInfo = crReportdocument.Database.Tables.Item(0).LogOnInfo

			If ReportStorico = 1 Then
				If ReportStorico = 1 Then
					With logOnInfo
						.ConnectionInfo.Password = AppSettings("DB_PASSWORD")
						.ConnectionInfo.ServerName = AppSettings("DB_DATA_SOURCE")
						.ConnectionInfo.DatabaseName = AppSettings("DB_NAME")
						.ConnectionInfo.UserID = AppSettings("DB_USERNAME")
					End With

				End If

			End If

			crReportdocument.Database.Tables(0).ApplyLogOnInfo(logOnInfo)
			'******************************************

			'3)gestione sotto report*********************
			sGruppo1 = Split(SottoReport, ":")
			x = 0
			For x = 0 To UBound(sGruppo1) - 1
				crReportdocument.OpenSubreport(sGruppo1(x)).Database.Tables(0).ApplyLogOnInfo(logOnInfo)
			Next x
			'******************************************

			'4)esportazione report in PDF***************
			Dim crDiskFileDestinationOptions As New CrystalDecisions.Shared.DiskFileDestinationOptions
			Dim crExportOptions As CrystalDecisions.Shared.ExportOptions


			crDiskFileDestinationOptions.DiskFileName = myPath.Server.MapPath("reports/export/" & NameReportNew & ".pdf")



			crExportOptions = crReportdocument.ExportOptions
			crExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
			crExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
			crExportOptions.DestinationOptions = crDiskFileDestinationOptions


			crReportdocument.Export()
			crReportdocument.Close()



			zip.AddFile(myPath.Server.MapPath("reports\export\" & NameReportNew & ".pdf"), "\")




			'******************************************

			' Next i




		Loop

		'Server.MapPath
		Dim NomeZip As String
		Dim percorso As String
		NomeZip = "ZIP_Corso_Riferimento_N_"

		'zip.Save(myPath.Server.MapPath("reports\export\" & NomeZip & " " & idcorso & ".rar"))
		zip.Save(myPath.Server.MapPath("reports\export\" & NomeZip & "_" & idcorso & ".rar"))
		'percorso = (myPath.Server.MapPath("reports\export\" & NomeZip & "_" & idcorso & ".rar"))
		percorso = "reports\export\" & NomeZip & "_" & idcorso & ".rar"
		If Not DtrParametri Is Nothing Then
			DtrParametri.Close()
			DtrParametri = Nothing
		End If
		Return percorso

	End Function

	Public Shared Sub RegistrazioneLogAccessi(ByVal Username As String, ByVal Descrizione As String, ByVal Esito As Integer, Conn As SqlClient.SqlConnection)

		'*** creato da simona cordella il 31/05/2018
		'*** registro gli accessi del sistema nel db

		Dim strsql As String
		Dim MySqlCommand As SqlClient.SqlCommand
		MySqlCommand = New SqlClient.SqlCommand

		strsql = " INSERT INTO LogAccessi "
		strsql &= " ( DataOraEvento,Username,Descrizione,Esito)"
		strsql &= " VALUES (getdate (), '" & Username & "', '" & Descrizione & "'," & Esito & ")"

		MySqlCommand.CommandText = strsql
		MySqlCommand.Connection = Conn
		MySqlCommand.ExecuteScalar()
		MySqlCommand.Dispose()
	End Sub

	Public Shared Function DataTableGenerico(ByVal QuerySql As String, ByVal conn As SqlClient.SqlConnection) As DataTable
		Dim CMD As New SqlDataAdapter(QuerySql, conn)
		Dim DtPrimario As New DataTable
		CMD.Fill(DtPrimario)
		DataTableGenerico = DtPrimario
		DtPrimario = Nothing
	End Function

	''' <summary>
	''' Restituisce il codice fiscale di un ente dato l'identificativo
	''' </summary>
	''' <param name="idEnte">identificativo dell'Ente</param>
	''' <param name="Conn">Connessione</param>
	''' <returns>Codice fiscale Ente. Null se non esiste</returns>
	Public Shared Function GetCodiceFiscaleEnte(ByVal idEnte As String, Conn As SqlConnection) As String
		'Calcolo Codice fiscale Ente
		Dim codiceFiscaleEnte As String
		Dim strSQL As String = "SELECT COALESCE(CodiceFiscale,CodiceFiscaleArchivio) from Enti WHERE IDEnte= '" & idEnte & "'"
		Dim dtrEnte = ClsServer.CreaDatareader(strSQL, Conn)
		If dtrEnte.Read() Then
			codiceFiscaleEnte = dtrEnte(0)
		End If
		dtrEnte.Close()
		Return codiceFiscaleEnte
    End Function

    ''' <summary>
    ''' Restituisce il codice fiscale di un ente dato l'identificativo
    ''' </summary>
    ''' <param name="idEnte">identificativo dell'Ente</param>
    ''' <param name="Conn">Connessione</param>
    ''' <returns>Codice fiscale Ente. Null se non esiste</returns>
    Public Shared Function GetAlboEnte(ByVal idEnte As String, Conn As SqlConnection) As String
        'Calcolo Codice fiscale Ente
        Dim AlboEnte As String
        Dim strSQL As String = "SELECT Albo from Enti WHERE IDEnte= '" & idEnte & "'"
        Dim dtrEnte = ClsServer.CreaDatareader(strSQL, Conn)
        If dtrEnte.Read() Then
            AlboEnte = dtrEnte(0)
        End If
        dtrEnte.Close()
        Return AlboEnte
    End Function

    Public Shared Function enteNuovo(ByVal idEnte As String, Conn As SqlConnection) As Boolean
        'Calcolo Codice fiscale Ente
        Dim DataAccreditamento As DateTime
        Dim strSQL As String = "SELECT DataAccreditamento from Enti WHERE IDEnte= '" & idEnte & "'"
        Dim dtrEnte = ClsServer.CreaDatareader(strSQL, Conn)
        If dtrEnte.Read() Then
            If IsDBNull(dtrEnte(0)) Then
                DataAccreditamento = Now()
            Else
                DataAccreditamento = dtrEnte(0)
            End If
        End If
        dtrEnte.Close()
        Return DataAccreditamento > "2021-06-01"
    End Function
	Public Shared Function GeneraHash(ByVal FileinByte() As Byte) As String
		Dim tmpHash() As Byte

		tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

		GeneraHash = ByteArrayToString(tmpHash)
		Return GeneraHash
	End Function

	Public Shared Function ByteArrayToString(ByVal arrInput() As Byte) As String
		Dim i As Integer
		Dim sOutput As New StringBuilder(arrInput.Length)
		For i = 0 To arrInput.Length - 1
			sOutput.Append(arrInput(i).ToString("X2"))
		Next
		Return sOutput.ToString()
	End Function

	Public Shared Function StreamToByte(input As Stream) As Byte()
		Dim blob(input.Length - 1) As Byte
		input.Read(blob, 0, input.Length)
		Return blob
    End Function

    Public Shared Sub ConfermaPreliminare(ByVal IdEntità As Integer, ByVal intAnno As Integer, ByVal intMese As Integer, ByVal strUsername As String, ByVal intAggiornaMalattieSpezzate As Integer, Conn As SqlClient.SqlConnection, Optional ByRef msg As String = "", Optional ByRef Esito As String = "")

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_PRESENZE_CONFERMA_PRELIMINARE_INDIVIDUALE]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Conn, SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEntità", SqlDbType.Int).Value = IdEntità
            sqlCMD.Parameters.Add("@Anno", SqlDbType.Int).Value = intAnno
            sqlCMD.Parameters.Add("@Mese", SqlDbType.Int).Value = intMese
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = strUsername
            sqlCMD.Parameters.Add("@AggiornaMalattieSpezzate", SqlDbType.Int).Value = intAggiornaMalattieSpezzate

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 50
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 255
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()

            msg = sqlCMD.Parameters("@Messaggio").Value
            Esito = sqlCMD.Parameters("@Esito").Value

        Catch ex As Exception
            Esito = "NEGATIVO"
            msg = "Errore non gestito."
        End Try
    End Sub

    Public Shared Function GeneraModuloPresenze(ByVal intIdEntità As Integer, ByVal intAnno As Integer, ByVal intMese As Integer, ByVal strPercorso As String, ByVal strUsername As String, Conn As SqlClient.SqlConnection, ByRef strMessaggio As String) As Byte()
        Dim documento As New AsposeWord
        Try
            documento.open(strPercorso)
        Catch ex As Exception
            'Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Template non valido", ex)
            strMessaggio = "Errore nella generazione del documento."
            Return Nothing
            Exit Function
        End Try

        Dim strsql As String
        Dim MyDataset As DataSet
        Dim strProgetto As String
        Dim strVolontario As String
        Dim strEnte As String
        Dim intIdEntitàPresenzeConfermaPreliminare As Integer

        strsql = "SELECT IdEntitàPresenzeConfermaPreliminare from EntitàPresenzeConfermaPreliminare a inner join comp_mensilità b on a.mensilità = b.mensilità where a.identità=" & intIdEntità & " and b.anno = " & intAnno & " and b.mese = " & intMese
        MyDataset = ClsServer.DataSetGenerico(strsql, Conn)

        If MyDataset.Tables(0).Rows.Count = 0 Then
            strMessaggio = "Per produrre il modulo precompilato è necessario prima effettuare la conferma preliminare del mese."
            Return Nothing
            Exit Function
        Else
            intIdEntitàPresenzeConfermaPreliminare = MyDataset.Tables(0).Rows(0).Item("IdEntitàPresenzeConfermaPreliminare")
        End If


        strsql = "SELECT e.denominazione + ' (' + e.codiceregione + ')' as Ente, d.titolo + ' (' + d.codiceente + ')' as Progetto, a.Cognome + ' ' + a.Nome + ' (' + a.codicevolontario + ')' as Volontario from Entità a inner join attivitàentità b on a.identità = b.identità and b.idstatoattivitàentità = 1 inner join attivitàentisediattuazione c on b.idattivitàentesedeattuazione = c.idattivitàentesedeattuazione inner join attività d on c.idattività = d.idattività inner join enti e on d.identepresentante = e.idente where a.identità=" & intIdEntità
        MyDataset = ClsServer.DataSetGenerico(strsql, Conn)

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            strProgetto = MyDataset.Tables(0).Rows(0).Item("Progetto")
            strVolontario = MyDataset.Tables(0).Rows(0).Item("Volontario")
            strEnte = MyDataset.Tables(0).Rows(0).Item("Ente")
        End If
        MyDataset.Dispose()


        'Dim managerAntimafia As New clsRuoloAntimafia()
        'Dim ruoli = managerAntimafia.GetRuoliAntimafia(Session("IdEnte"), Session("conn"))
        'Dim managerEnte = New clsEnte()
        'Dim ente = managerEnte.GetDatiEnte(Session("IdEnte"), Session("conn"))

        documento.addFieldValue("Ente", strEnte)
        documento.addFieldValue("Progetto", strProgetto)
        documento.addFieldValue("AnnoMese", intAnno & " - " & intMese)
        documento.addFieldValue("Volontario", strVolontario)
        documento.addFieldValue("Data", Date.Today.ToString("dd/MM/yyyy"))

        Dim html As New StringBuilder
        html.Append("<style>")
        html.Append("table {width:100%; border-collapse: collapse; font-size:10pt; margin-bottom:1em;}")
        html.Append("table, th, td {border: 1px solid lightgray;}")
        html.Append("table tr:nth-child(even) {background-color:#eee}")
        html.Append("td {padding:1pt; font-family:'courier new';}")
        html.Append(".space {width:50%;height:1em;}")
        html.Append("</style>")

        html.Append("<table><tbody>")
        html.Append("<tr style='fort-weight:bold'>")
        html.Append("<th>Lun</th>")
        html.Append("<th>Mar</th>")
        html.Append("<th>Mer</th>")
        html.Append("<th>Gio</th>")
        html.Append("<th>Ven</th>")
        html.Append("<th>Sab</th>")
        html.Append("<th>Dom</th>")
        html.Append("</tr>")

        Dim PrimaSettimana As Boolean = True
        Dim PrimoGiorno As Boolean = True
        Dim DOW As Integer = 0

        strsql = "select dbo.formatodata(a.Giorno) as Giorno, b.Codice , b.Descrizione, DATEPART(w,a.giorno) DOW from EntitàPresenze a inner join CausaliPresenze b on a.IDCausalePresenza = b.IDCausalePresenza where a.IDEntità = " & intIdEntità & " and year(a.Giorno) = " & intAnno & " and month(a.giorno) = " & intMese & " order by a.giorno "
        MyDataset = ClsServer.DataSetGenerico(strsql, Conn)

        Dim myrow As Data.DataRow
        If MyDataset.Tables(0).Rows.Count <> 0 Then
            For Each myrow In MyDataset.Tables(0).Rows
                DOW = DOW + 1
                If PrimaSettimana And PrimoGiorno And myrow.Item("DOW").ToString <> DOW Then
                    html.Append("<tr>")
                    For x = 1 To myrow.Item("DOW") - 1
                        html.Append("<td></td>")
                        DOW = DOW + 1
                    Next
                    html.Append("<td>" & myrow.Item("Giorno").ToString & "<br>" & myrow.Item("Codice").ToString & "</td>")
                    PrimoGiorno = False

                Else
                    If DOW = 1 Then
                        html.Append("<tr>")
                    End If
                    html.Append("<td>" & myrow.Item("Giorno").ToString & "<br>" & myrow.Item("Codice").ToString & "</td>")
                End If
                If DOW = 7 Then
                    html.Append("</tr>")
                    DOW = 0
                    PrimaSettimana = False
                End If
            Next
        End If
        MyDataset.Dispose()


        'Dim html As New StringBuilder
        'html.Append("<style>")
        'html.Append("table {width:100%; border-collapse: collapse; font-size:10pt; margin-bottom:1em;}")
        'html.Append("table, th, td {border: 1px solid lightgray;}")
        'html.Append("table tr:nth-child(even) {background-color:#eee}")
        'html.Append("td {padding:1pt; font-family:'courier new';}")
        'html.Append(".space {width:50%;height:1em;}")
        'html.Append("</style>")

        'html.Append("<table><tbody>")
        'html.Append("<tr style='fort-weight:bold'>")
        'html.Append("<th>Giorno</th>")
        'html.Append("<th>Codice Causale</th>")
        'html.Append("<th>Descrizione Causale</th>")
        'html.Append("</tr>")
        'Dim rigaPari As Boolean = True

        'strsql = "select dbo.formatodata(a.Giorno) as Giorno, b.Codice , b.Descrizione from EntitàPresenze a inner join CausaliPresenze b on a.IDCausalePresenza = b.IDCausalePresenza where a.IDEntità = " & intIdEntità & " and year(a.Giorno) = " & intAnno & " and month(a.giorno) = " & intMese & " order by a.giorno "
        'MyDataset = ClsServer.DataSetGenerico(strsql, Conn)

        'Dim myrow As Data.DataRow
        'If MyDataset.Tables(0).Rows.Count <> 0 Then
        '    For Each myrow In MyDataset.Tables(0).Rows
        '        html.Append("<tr>")
        '        html.Append("<td>" & myrow.Item("Giorno").ToString & "</td>")
        '        html.Append("<td>" & myrow.Item("Codice").ToString & "</td>")
        '        html.Append("<td>" & myrow.Item("Descrizione").ToString & "</td>")
        '        html.Append("</tr>")
        '    Next
        'End If
        'MyDataset.Dispose()

        html.Append("</tbody></table>")
        html.Append("</br>")

        documento.addFieldHtml("htmlCalendario", html.ToString)

        Try
            documento.merge()

            Dim MySqlCommand As SqlClient.SqlCommand
            MySqlCommand = New SqlClient.SqlCommand

            strsql = " UPDATE EntitàPresenzeConfermaPreliminare SET DataUltimaStampaModulo = GETDATE() WHERE IdEntitàPresenzeConfermaPreliminare = " & intIdEntitàPresenzeConfermaPreliminare

            MySqlCommand.CommandText = strsql
            MySqlCommand.Connection = Conn
            MySqlCommand.ExecuteScalar()
            MySqlCommand.Dispose()

            Return documento.pdfBytes


        Catch ex As Exception
            'Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Scrittura template", ex)
            strMessaggio = "Errore nella generazione del documento"
            Return Nothing
            Exit Function
        End Try
        'Log.Information(LogEvent.PRESENTAZIONE_RIEPILOGO)
        'Response.Clear()
        'Response.ContentType = "Application/pdf"
        'Response.AddHeader("Content-Disposition", "attachment; filename=" & lblCodiceVolontario.Text & "_" & Calendar1.VisibleDate.Year & "_" & Calendar1.VisibleDate.Month & ".pdf")
        'Response.BinaryWrite(documento.pdfBytes)
        'Response.End()
    End Function
End Class
