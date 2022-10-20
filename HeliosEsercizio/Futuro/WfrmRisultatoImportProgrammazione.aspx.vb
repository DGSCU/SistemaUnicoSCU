Imports System.Collections
Imports System.IO
Public Class WfrmRisultatoImportProgrammazione
    Inherits System.Web.UI.Page

    Dim IdProgrammazione As Integer
    Dim MyCommand As SqlClient.SqlCommand
    Dim DefArr As String()
#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlClient.SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If IsPostBack = False Then
            'Inserire qui il codice utente necessario per inizializzare la pagina
            CaricaCompetenze()
            If Session("Sistema") = "Helios" Then
                If Trim(Request.QueryString("IdRegCompetenza")) <> 0 Then
                    Select Case Trim(Request.QueryString("IdRegCompetenza"))
                        Case 22
                            ddlCompetenza.SelectedValue = -1
                        Case Else
                            ddlCompetenza.SelectedValue = Trim(Request.QueryString("IdRegCompetenza"))
                    End Select
                End If
            Else
                If Session("TipoUtente") = "U" Then
                    ddlCompetenza.SelectedValue = -1
                Else
                    If Trim(Request.QueryString("IdRegCompetenza")) <> 0 Then
                        Select Case Trim(Request.QueryString("IdRegCompetenza"))
                            Case 22
                                ddlCompetenza.SelectedValue = -1
                            Case Else
                                ddlCompetenza.SelectedValue = Trim(Request.QueryString("IdRegCompetenza"))
                        End Select
                    End If
                End If
            End If

            ddlCompetenza.Enabled = False
            If Session("Sistema") = "Helios" Then
                CaricaBando()
            Else
                CaricaBandoGG()
            End If



            ddlBando.Enabled = False

            CaricaGriglia()

            lblTotali.Text = "Sono state inviate " & Request.QueryString("Tot") & " righe. " & _
                             Request.QueryString("TotOk") & " con esito positivo. " & Request.QueryString("TotKo") & " con esito negativo."

            hlDownLoad.NavigateUrl = "download\" & Request.QueryString("NomeFile") & ".CSV"

           

            If CInt(Request.QueryString("TotKo")) > 0 Or Request.QueryString("Tot") = 0 Then
                CmdConferma.Visible = False
            Else
                AvvisoConferma.Visible = True
                avviso.Visible = True
                testoavviso.InnerHtml = "LA VERIFICA DEI DATI IMMESSI NEL FILE CSV RISULTA CORRETTA. PER SALVARE DEFINITIVAMENTE I DATI PREMERE IL TASTO CONFERMA."
            End If
        End If

    End Sub

    'OK 1
    Private Sub CaricaGriglia()
        Dim dtCSV As DataTable = New DataTable
        Dim rwCSV As DataRow
        Dim clCSV As DataColumn

        Dim strSql As String
        Dim i As Integer

        Dim TmpArr() As String

        Dim Reader As StreamReader
        Dim xLinea As String


        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Note"
        clCSV.Caption = "Note"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "CodiceProgetto"
        clCSV.Caption = "CodiceProgetto"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "CodiceSede"
        clCSV.Caption = "CodiceSede"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Verificatore"
        clCSV.Caption = "Verificatore"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        Reader = New StreamReader(Server.MapPath("download") & "\" & Request.QueryString("NomeFile") & ".CSV")

        xLinea = Reader.ReadLine()
        xLinea = Reader.ReadLine()


        While (xLinea <> "")

            DefArr = CreaArray(xLinea)
            rwCSV = dtCSV.NewRow

            rwCSV(0) = DefArr(0)

            If UBound(DefArr) = 0 Then
                rwCSV(1) = vbNullString
                rwCSV(2) = vbNullString
                rwCSV(3) = vbNullString
            Else

                rwCSV(1) = DefArr(1)
                If UBound(DefArr) = 1 Then
                    rwCSV(2) = vbNullString
                Else
                    rwCSV(2) = DefArr(2)
                    If UBound(DefArr) = 2 Then
                        rwCSV(3) = vbNullString
                    Else
                        If DefArr(3) = vbNullString Then
                            rwCSV(3) = vbNullString
                        Else
                            If IsNumeric(Trim(DefArr(3))) = True Then
                                rwCSV(3) = RicavaVerificatore(Trim(DefArr(3)))
                            End If
                        End If
                    End If
                End If
            End If

            dtCSV.Rows.Add(rwCSV)
            xLinea = Reader.ReadLine()
        End While

        dtgCSV.DataSource = dtCSV
        dtgCSV.DataBind()


    End Sub

    'OK 1
    Private Function CreaArray(ByVal pLinea As String) As String()
        Dim TmpArr As String()

        Dim i As Integer
        Dim x As Integer

        TmpArr = Split(pLinea, ";")

        For i = 0 To UBound(TmpArr)
            If i = 0 Then
                ReDim DefArr(0)
            Else
                ReDim Preserve DefArr(UBound(DefArr) + 1)
            End If
            If Left(TmpArr(i), 1) = Chr(34) Then
                x = i
                Do While Right(TmpArr(x), 1) <> Chr(34)
                    If x = i Then
                        DefArr(UBound(DefArr)) = Mid(TmpArr(x), 2) & "; "
                    Else
                        DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & TmpArr(x) & "; "
                    End If
                    x = x + 1
                Loop
                DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
                i = x
            Else
                DefArr(UBound(DefArr)) = TmpArr(i)
            End If
        Next

        CreaArray = DefArr

    End Function


    'OK 1
    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand

        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_PROGRAMMAZIONE]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub

    Private Sub InserimentoScenario(ByVal IdProgrammazione As Integer)
        Dim strsql As String
        strsql = "insert into tverifichescenario (idprogrammazione,descrizione,idstatoscenario,datainserimento,userinseritore,note) " & _
                "values (" & IdProgrammazione & ",'SCENARIO IMPORTAZIONE',1,getdate(),'" & Session("Utente") & "','SCENARIO IMPORTAZIONE')"
        'MyCommand.CommandText = strsql
        'MyCommand.ExecuteNonQuery()
        Dim InserCom As New SqlClient.SqlCommand(strsql, Session("conn"))
        InserCom.ExecuteNonQuery()
        '(strsql, Session("conn"))
        '       InserCom.ExecuteNonQuery()

    End Sub

    Private Sub InserimentoVerifica(ByVal IdProgrammazione As Integer)
        Dim strSql As String
        Dim rstScen As SqlClient.SqlDataReader
        Dim intIdScenario As Integer
        Dim sqlCommInsVer As New SqlClient.SqlCommand
        Dim item As DataGridItem
        Dim intCompetenza As Integer
        '*** MODIFICATO DA SIMONA CORDELLA IL 26/09/2016
        '** E' STATO RICHESITO DI APPROVVARE LA PROGRAMMAZIONI IN FASE DI IMPORT:
        '** lo stato della programmazione (IDSTATOPROGRAMMAZIONE -->TVERIFICHEPROGRAMMAZIONE) passa da 2(proposta)a 4(Approvata)
        '** lo stato della veridica (IDSTATOVERIFICA -->TVERIFICHE) passa da 4(assegnata)a 5(Aperta)
        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case -1
                    intCompetenza = 22
                Case Else
                    intCompetenza = ddlCompetenza.SelectedValue()
            End Select
        End If

        'trovo l'id dell'ultima programmazione inserita
        strSql = "Select @@identity as IdScenario from tverifichescenario "
        rstScen = ClsServer.CreaDatareader(strSql, Session("conn"))
        If rstScen.HasRows = True Then
            rstScen.Read()
            intIdScenario = rstScen("IdScenario")
        End If
        If Not rstScen Is Nothing Then
            rstScen.Close()
            rstScen = Nothing
        End If

        For Each item In dtgCSV.Items
            Dim IdAttivitaEnteSedeAttuazione As Integer = AttivitaEnteSede(item.Cells(2).Text, item.Cells(1).Text)

            sqlCommInsVer.CommandType = CommandType.StoredProcedure
            sqlCommInsVer.CommandText = "[SP_VER_INSERIMENTO_IMPORTAZIONE_VERIFICHE]"
            sqlCommInsVer.Connection = Session("conn")
            sqlCommInsVer.Parameters.Add("@IDSCENARIO", intIdScenario)
            sqlCommInsVer.Parameters.Add("@IDATTIVITAENTESEDEATTUAZIONE", IdAttivitaEnteSedeAttuazione)
            sqlCommInsVer.Parameters.Add("@IDSTATOVERIFICA", 4) 'assegnata
            sqlCommInsVer.Parameters.Add("@UTENTE", Session("Utente"))
            sqlCommInsVer.Parameters.Add("@IDPROGRAMMAZIONE", IdProgrammazione)
            sqlCommInsVer.Parameters.Add("@IDREGCOMPETENZA", intCompetenza)
            sqlCommInsVer.Parameters.Add("@VERIFICATORE", item.Cells(3).Text)
            sqlCommInsVer.Parameters.Add("@INIZIOVERIFICA", TxtDataInizio.Text)
            sqlCommInsVer.Parameters.Add("@FINEVERIFICA", TxtDataFine.Text)
            sqlCommInsVer.ExecuteNonQuery()
            sqlCommInsVer.Parameters.Clear()
        Next

        Dim sqlComUpdVerifica As New SqlClient.SqlCommand

        sqlComUpdVerifica.Connection = Session("conn")
        sqlComUpdVerifica.CommandType = CommandType.Text

        '** mod. il 26/09/2016 da s.c.  lo stato della programmazione (IDSTATOPROGRAMMAZIONE -->TVERIFICHEPROGRAMMAZIONE) passa da 2(proposta)a 4(Approvata)
        sqlComUpdVerifica.CommandText = " Update TVerificheProgrammazione SET  " & _
                                        " IdStatoProgrammazione = 4, " & _
                                        " DataPresentazione = getdate() " & _
                                        " Where IdProgrammazione = " & IdProgrammazione
        sqlComUpdVerifica.ExecuteNonQuery()

        'aggiunto da sc il 04/03/2011 modifico lo stato dello scenario a ASSOCIATO
        sqlComUpdVerifica.CommandText = "UPDATE TVerificheScenario SET idstatoscenario = 3 WHERE idscenario = " & intIdScenario & ""
        sqlComUpdVerifica.ExecuteNonQuery()



        '** mod. il 26/09/2016 da s.c. lo stato della veridica (IDSTATOVERIFICA -->TVERIFICHE) passa da 4(assegnata)a 5(Aperta)
        sqlComUpdVerifica.CommandText = "UPDATE TVerifiche SET IDStatoVerifica = 5 , DataApprovazione = getdate()  where IDProgrammazione = " & IdProgrammazione & " and IDStatoVerifica = 4 "
        sqlComUpdVerifica.ExecuteNonQuery()

    End Sub

    Private Function AttivitaEnteSede(ByVal pCodiceSede As String, ByVal pCodiceAttivita As String) As Integer
        Dim dtrAttivitaEnteSede As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT IdAttivitàEnteSedeAttuazione FROM AttivitàEntiSediAttuazione " & _
                 "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 "WHERE Attività.CodiceEnte = '" & pCodiceAttivita & "' " & _
                 "AND AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = " & pCodiceSede

        dtrAttivitaEnteSede = ClsServer.CreaDatareader(strSql, Session("conn"))

        'Se esiste ritorna True 
        dtrAttivitaEnteSede.Read()
        If dtrAttivitaEnteSede.HasRows = True Then
            AttivitaEnteSede = dtrAttivitaEnteSede("IdAttivitàEnteSedeAttuazione")
        End If
        dtrAttivitaEnteSede.Close()
        dtrAttivitaEnteSede = Nothing
    End Function

    Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                ''trSQL = strSQL & " select '0',' TUTTI ','','A' "
                ''strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlCompetenza.DataSource = dtrCompetenze
                ddlCompetenza.Items.Add("")
                ddlCompetenza.DataTextField = "Descrizione"
                ddlCompetenza.DataValueField = "IDRegioneCompetenza"
                ddlCompetenza.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If

            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenza.Enabled = True
                ddlCompetenza.SelectedIndex = 0

            Else

                'CboCompetenza.SelectedIndex = 1
                'CboCompetenza.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenza.Enabled = True
                    End If
                End If

                If Session("TipoUtente") = "R" Then
                    ddlCompetenza.Enabled = False
                End If

            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub

    Private Function Programmazione() As Integer
        Dim strSql As String
        Dim rstProg As SqlClient.SqlDataReader
        Dim intIdProgrammazione As Integer

        SalvaProgrammazione()

        If Not rstProg Is Nothing Then
            rstProg.Close()
            rstProg = Nothing
        End If
        'trovo l'id dell'ultima programmazione inserita
        strSql = "Select @@identity as IdProgrammazione from TVerificheProgrammazione "
        rstProg = ClsServer.CreaDatareader(strSql, Session("conn"))
        If rstProg.HasRows = True Then
            rstProg.Read()
            intIdProgrammazione = rstProg("IdProgrammazione")
        End If
        If Not rstProg Is Nothing Then
            rstProg.Close()
            rstProg = Nothing
        End If
        Programmazione = intIdProgrammazione
    End Function

    Private Sub SalvaProgrammazione()
        Dim strSql As String
        Dim strNull As String = "null"
        Dim intCompetenza As Integer
        Dim CmdGenerico As SqlClient.SqlCommand

        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case -1
                    intCompetenza = 22
                Case Else
                    intCompetenza = ddlCompetenza.SelectedValue()
            End Select
        End If
        'Session("IdRegCompetenza") = intCompetenza

        strSql = "INSERT INTO TVerificheProgrammazione"
        strSql = strSql & " (IDStatoProgrammazione, Descrizione, DataInizioValidità,DataFineValidità, "
        strSql = strSql & "  DataInserimento, UserInseritore, Note,IdRegCompetenza,IdBando)"
        strSql = strSql & "VALUES  ( 1,'" & Replace(TxtDescrizione.Text, "'", "''") & "', '" & TxtDataInizio.Text & "','" & TxtDataFine.Text & "', "
        strSql = strSql & " getdate(),'" & Session("Utente") & "', "
        If TxtNote.Text = "" Then
            strSql = strSql & " " & strNull & " ,"
        Else
            strSql = strSql & " '" & Replace(TxtNote.Text, "'", "''") & "',"
        End If
        strSql = strSql & " " & intCompetenza & ", " & ddlBando.SelectedValue() & " )"

        CmdGenerico = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub
 
    Private Sub CaricaBando()
        Dim strSql As String
        Dim rstBando As SqlClient.SqlDataReader
        Dim intIdReg As Integer
        'Gestione Bandi
        'carico i bandi secondo la regione di competenza della programmazione

        If Not rstBando Is Nothing Then
            rstBando.Close()
            rstBando = Nothing
        End If
        If ddlCompetenza.SelectedValue = "-1" Then
            intIdReg = 22
        Else
            intIdReg = ddlCompetenza.SelectedValue
        End If


        strSql = " SELECT bando.IDBando,bando.BandoBreve " & _
                " FROM bando " & _
                " INNER JOIN AssociaBandoRegioniCompetenze ON bando.IDBando = AssociaBandoRegioniCompetenze.IdBando " & _
                " WHERE AssociaBandoRegioniCompetenze.IdRegioneCompetenza = " & intIdReg & " and   bando.IDBando=" & Trim(Request.QueryString("IdBando")) & " " & _
                " order by BandoBreve desc "
        rstBando = ClsServer.CreaDatareader(strSql, Session("conn"))
        If rstBando.HasRows = True Then
            ddlBando.DataSource = rstBando
            ddlBando.DataTextField = "BandoBreve"
            ddlBando.DataValueField = "IDBando"
            ddlBando.DataBind()
            ddlBando.SelectedValue = Trim(Request.QueryString("IdBando"))
        End If

        If Not rstBando Is Nothing Then
            rstBando.Close()
            rstBando = Nothing
        End If

    End Sub

    Private Sub CaricaBandoGG()
        Dim strSql As String
        Dim rstBando As SqlClient.SqlDataReader
        Dim intIdReg As Integer
        'Gestione Bandi
        'carico i bandi secondo la regione di competenza della programmazione

        If Not rstBando Is Nothing Then
            rstBando.Close()
            rstBando = Nothing
        End If
        
        strSql = " SELECT bando.IDBando,bando.BandoBreve " & _
                " FROM bando " & _
                " INNER JOIN AssociaBandoRegioniCompetenze ON bando.IDBando = AssociaBandoRegioniCompetenze.IdBando " & _
                " WHERE bando.IDBando=" & Trim(Request.QueryString("IdBando")) & " " & _
                " order by BandoBreve desc "
        rstBando = ClsServer.CreaDatareader(strSql, Session("conn"))
        If rstBando.HasRows = True Then
            ddlBando.DataSource = rstBando
            ddlBando.DataTextField = "BandoBreve"
            ddlBando.DataValueField = "IDBando"
            ddlBando.DataBind()
            ddlBando.SelectedValue = Trim(Request.QueryString("IdBando"))
        End If

        If Not rstBando Is Nothing Then
            rstBando.Close()
            rstBando = Nothing
        End If

    End Sub


    Private Function RicavaVerificatore(ByVal IDVerificatore As Integer) As String
        Dim idCompetenzaUtente As Integer
        Dim StrSql As String
        Dim dtrVer As SqlClient.SqlDataReader
        Dim intCompetenza As Integer

        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case -1
                    intCompetenza = 22
                Case Else
                    intCompetenza = ddlCompetenza.SelectedValue()
            End Select
        End If

        'ricerco tutti i verificatori interni a secondo della competenza dell'Utente in sessione
        StrSql = " SELECT  UPPER(Cognome + ' ' + Nome) as Verificatore FROM TVerificatori WHERE IDVerificatore = " & IDVerificatore & " AND IdRegCompetenza = " & intCompetenza & ""
        dtrVer = ClsServer.CreaDatareader(StrSql, Session("conn"))

        'Se esiste ritorna True 
        If dtrVer.HasRows = True Then
            dtrVer.Read()
            RicavaVerificatore = dtrVer("Verificatore")
        Else
            RicavaVerificatore = ""
        End If

        dtrVer.Close()
        dtrVer = Nothing
    End Function

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("WfrmImportProgrammazione.aspx")


    End Sub
    Private Function Controlliformali() As Boolean
        Dim bln As Boolean = True

        LblErrore.Text = ""
        If TxtDescrizione.Text = String.Empty Then
            LblErrore.Text = LblErrore.Text + "Inserire la descrizione della Programmazione.<br/>"
            bln = False
        End If
        If TxtDataInizio.Text = String.Empty Then
            LblErrore.Text = LblErrore.Text + "Inserire la Data Inizio Programmazione.<br/>"
            bln = False
        End If
        If TxtDataFine.Text = String.Empty Then
            LblErrore.Text = LblErrore.Text + "Inserire la Data Fine Programmazione.<br/>"
            bln = False
        End If
        Dim DataInizio As Date
        If (TxtDataInizio.Text.Trim <> String.Empty AndAlso Date.TryParse(TxtDataInizio.Text, DataInizio) = False) Then
            LblErrore.Text = LblErrore.Text + "La Data Inizio Programamzione non è valida. Inserire la data nel formato GG/MM/AAAA.</br>"
            bln = False
        End If
        Dim DataFine As Date
        If (TxtDataFine.Text.Trim <> String.Empty AndAlso Date.TryParse(TxtDataFine.Text, DataFine) = False) Then
            LblErrore.Text = LblErrore.Text + "La Data Fime Programamzione non è valida. Inserire la data nel formato GG/MM/AAAA.</br>"
            bln = False
        End If
        If D1LTD2(TxtDataFine.Text, TxtDataInizio.Text) Then
            LblErrore.Text = LblErrore.Text + "La Data Fine Programamzione deve essere maggiore o uguale alla data Inzio Programmazione.<br/>"
            bln = False
        End If


        Return bln
    End Function
    Private Function D1LTD2(ByVal d1 As String, d2 As String) As Boolean

        ' ritorna true se la data d1< d2 - > messaggio errore 
        Dim data1 As Date
        Dim data2 As Date

        D1LTD2 = False

        Try
            If (d1 <> "" And d2 <> "") Then
                data1 = Convert.ToDateTime(d1)
                data2 = Convert.ToDateTime(d2)

                If data1 < data2 Then
                    D1LTD2 = True
                End If
            End If
        Catch ex As Exception
            D1LTD2 = True
        End Try

    End Function
    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean


        If Controlliformali() = False Then
            LblErrore.Visible = True
            Exit Sub
        End If
        MyCommand = New SqlClient.SqlCommand
        'MyCommand.Connection = Session("conn")
        CmdConferma.Visible = False

        Try

            'MyTransaction = Session("conn").BeginTransaction(Session("Utente"))
            'MyCommand.Transaction = MyTransaction
            IdProgrammazione = Programmazione()
            InserimentoScenario(IdProgrammazione)
            InserimentoVerifica(IdProgrammazione)

    
            'MyTransaction.Commit()

        Catch exc As Exception

            'MyTransaction.Rollback(Session("Utente"))
            swErr = True

        End Try

        'MyCommand.Dispose()


        If swErr = False Then
            'Esito positivo
            lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."
            lblEsito.Visible = True
            If Session("TipoUtente") = "U" Then
                CreazioneFascioliVerifiche(IdProgrammazione)
            End If

            ' imgAnnulla.ImageUrl = "images/chiudi.jpg"
            'If EseguiOrdinaGraduatoria() = False Then
            '    'Errore EseguiOrdinaGraduatoria
            '    lblEsito.Text = "Errore durante l'operazione di ordinamento della graduatoria."
            '    lblEsito.Visible = True
            '    imgAnnulla.ImageUrl = "images/annulla.jpg"
            'Else
            '    'Esito positivo
            '    lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."
            '    lblEsito.Visible = True
            '    imgAnnulla.ImageUrl = "images/chiudi.jpg"
            'End If
        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
            ' imgAnnulla.ImageUrl = "images/annulla.jpg"
        End If

        CancellaTabellaTemp()
    End Sub

    Private Sub CreazioneFascioliVerifiche(ByVal IdProgrammazione As Integer)
        Dim strSql As String
        Dim dtrVerifica As SqlClient.SqlDataReader
        Dim strIdVerifica As String = ""

        ChiudiDataReader(dtrVerifica)
        strSql = "SELECT IDVerifica, isnull(CodiceFascicolo,'') as CodiceFascicolo FROM TVerifiche WHERE IDProgrammazione=" & IdProgrammazione
        dtrVerifica = ClsServer.CreaDatareader(strSql, Session("Conn"))

        If dtrVerifica.HasRows = True Then
            Do While dtrVerifica.Read
                If dtrVerifica("CodiceFascicolo") = "" Then
                    strIdVerifica = strIdVerifica & dtrVerifica("IDVerifica") & "#"
                End If
            Loop
        End If
        ChiudiDataReader(dtrVerifica)

        If strIdVerifica <> "" Then
            ClsUtility.GeneraFascicoloCumulatiVerifiche(Session("Utente"), Mid(strIdVerifica, 1, Len(strIdVerifica) - 1), Session("Conn"))
        End If
    End Sub
End Class