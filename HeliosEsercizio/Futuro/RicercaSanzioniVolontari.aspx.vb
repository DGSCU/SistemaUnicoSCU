﻿Imports System.IO

Public Class RicercaSanzioniVolontari
    Inherits System.Web.UI.Page
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            lblTitolo.Text = "Ricerca Sanzioni Volontari"
            If Request.QueryString("VengoDa") = "Inserimento" Then
                PersonalizzaMaschera()
            Else
                'se ho selezionato un volontario per confermare le sue assenze vado a fare l'updatre dello stato
                If Request.QueryString("Op") = "conferma" Then
                    strquery = "Update EntitàAssenze set stato=2, usernameConferma='" & Session("Utente") & "',dataConferma=getdate()  where Stato=1 and IDEntità=" & CInt(Request.QueryString("IdEntita")) & " "
                    'faccio l'aggiornamento dello stato delle assenze registrate del volontario selezionato
                    Dim cmdupdate As Data.SqlClient.SqlCommand
                    cmdupdate = New SqlClient.SqlCommand(strquery, Session("conn"))
                    ''informo l'utente dell'avvenuto inserimento
                    cmdupdate.ExecuteNonQuery()
                    cmdupdate.Dispose()
                End If
                ddlstato.Items.Add("Seleziona")
                ddlstato.Items.Add("Registrate")
                ddlstato.Items.Add("Confermate")
                ddlstato.Items.Add("Respinte")
                CaricaGriglia()
            End If
            'CaricaGriglia()
        End If
    End Sub

    Private Sub PersonalizzaMaschera()
        'Generato da Alessandra Taballione il 02/12/2004
        'Personalizzazione della Maschera in fase di Ricerca per Modifica Assenze.
        divStato.Visible = False
    End Sub

    Sub CaricaGriglia()
        Dim strSql As String
        Dim strWhere As String
        Dim MyDataSet As DataSet
        'DESCRIZIONE: routine che carica la griglia con tutti i progetti con stato attività=1
        'AUTORE: Michele d'Ascenzio    
        'DATA: 03/11/2004
        'Modificata da Alessandra taballione il 02/12/2004
        'Modificata da Bagnani jooooonnn il 07/12/2004
        'MOD. IL 28/11/2014 da s.c. FiltroVisibilita
        Try
            strSql = "SELECT distinct Entità.IDEntità as IdEntità, " & _
                     "Attività.IDAttività as IdAttività, " & _
                     "Enti.IdEnte as IdEnte, " & _
                     "Entità.Cognome + ' ' + Entità.Nome as Nominativo, " & _
                     "Entità.CodiceFiscale, " & _
                     "Entità.DataNascita, " & _
                     "Entità.CodiceVolontario, " & _
                     "Comuni.Denominazione as ComuneNascita, " & _
                     "Attività.Titolo as Progetto, " & _
                     "EntiSediAttuazioni.Denominazione as SedeAttuazione, " & _
                     "Enti.Denominazione as Ente "
            If Request.QueryString("VengoDa") = "Ricerca" Then
                strSql = strSql & ",isnull((select sum(giorni) from entitàassenze where stato=1 and identità=Entità.idEntità),0) as NProposte," & _
                        " isnull((select sum(giorni) from entitàassenze where stato=2 and identità=Entità.idEntità),0) as NConfermate, " & _
                        " isnull((select sum(giorni) from entitàassenze where stato=3 and identità=Entità.idEntità),0) as NRespinte, " & _
                        "'<img src=images/answer.gif onClick=''JavaScript:ConfermaAssenze(' + CONVERT(varchar, isnull((select count(*) from entitàassenze where stato=1 and identità=Entità.idEntità),0)) + ', ' + CONVERT(varchar,entitàassenze.identità) +')'' Style=cursor:hand Width=20 Height=20 title=Conferma border=0>' AS Conf, " & _
                        " isnull((select sum(giorni) from entitàassenze where identità=Entità.idEntità),0) as TotAssenze "
            Else
                strSql = strSql & ",'0'as NProposte,'' as Conf,'0'as NConfermate,'0'as NRespinte,'0'as TotAssenze "
            End If
            strSql = strSql & " FROM Entità "
            If Request.QueryString("VengoDa") = "Ricerca" Then
                strSql = strSql & " inner join entitàassenze on entitàassenze.identità=Entità.identità  "
            End If
            strSql = strSql & "INNER JOIN Comuni ON Entità.IdComuneNascita = Comuni.IdComune " & _
                              "INNER JOIN AttivitàEntità ON Entità.IdEntità = AttivitàEntità.IdEntità " & _
                              "INNER JOIN StatiAttivitàEntità ON StatiAttivitàEntità.IdStatoAttivitàEntità = AttivitàEntità.IdStatoAttivitàEntità AND StatiAttivitàEntità.DefaultStato = 1 " & _
                              "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntità.IDAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione " & _
                              "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IDEnteSedeAttuazione = EntiSediAttuazioni.IDEnteSedeAttuazione " & _
                              "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                              "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                              "INNER JOIN Enti ON attività.IdEntepresentante = Enti.IdEnte " & _
                              "INNER JOIN StatiEntità ON Entità.IdStatoEntità = Statientità.IdStatoEntità " & _
                              "INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
                              "WHERE (StatiEntità.inservizio = 1 or StatiEntità.Sospeso = 1) "
            If Session("TipoUtente") = "E" Then
                strWhere = strWhere & "AND Enti.IdEnte = " & Session("IdEnte") & " "
            End If
            If txtEnte.Text <> vbNullString Then
                strWhere = strWhere & "AND Enti.Denominazione like '" & Replace(txtEnte.Text, "'", "''") & "%' "
            End If
            If txtCodEnte.Text <> vbNullString Then
                strWhere = strWhere & "AND Enti.Codiceregione like '" & Replace(txtCodEnte.Text, "'", "''") & "%' "
            End If
            If txtCognome.Text <> vbNullString Then
                strWhere = strWhere & "AND Entità.Cognome like '" & Replace(txtCognome.Text, "'", "''") & "%' "
            End If
            If txtNome.Text <> vbNullString Then
                strWhere = strWhere & "AND Entità.Nome like '" & Replace(txtNome.Text, "'", "''") & "%' "
            End If
            If txtProgetto.Text <> vbNullString Then
                strWhere = strWhere & "AND Attività.Titolo like '" & Replace(txtProgetto.Text, "'", "''") & "%' "
            End If
            If txtProgetto.Text <> vbNullString Then
                strWhere = strWhere & "AND Attività.Titolo like '" & Replace(txtProgetto.Text, "'", "''") & "%' "
            End If
            If txtVolontario.Text <> vbNullString Then
                strWhere = strWhere & "AND Entità.CodiceVolontario like '" & Replace(txtVolontario.Text, "'", "''") & "%' "
            End If
            If txtCodProgetto.Text <> vbNullString Then
                strWhere = strWhere & "AND Attività.CodiceEnte like '" & txtCodProgetto.Text & "%'"
            End If
            ''Controllo se la data Riferimento è valorizzata
            'If Trim(txtdatadal.Text) <> "" And Trim(txtdataal.Text) = "" Then
            '    strWhere = strWhere & " and entitàassenze.dataRiferimento >= '" & txtdatadal.Text & "'"
            'End If
            'If Trim(txtdatadal.Text) <> "" And Trim(txtdataal.Text) <> "" Then
            '    strWhere = strWhere & " and entitàRimborsi.dataRiferimento BETWEEN '" & txtdatadal.Text & "' and '" & txtdataal.Text & "'"
            'End If
            'If Trim(txtdatadal.Text) = "" And Trim(txtdataal.Text) <> "" Then
            '    strWhere = strWhere & " and entitàRimborsi.dataRiferimento <= '" & txtdataal.Text & "'"
            'End If
            If ddlstato.Visible = True Then
                If ddlstato.SelectedItem.Text <> "Seleziona" Then
                    Select Case ddlstato.SelectedItem.Text
                        Case "Registrate"
                            strWhere = strWhere & "AND EntitàAssenze.Stato=1 "
                        Case "Confermate"
                            strWhere = strWhere & "AND EntitàAssenze.Stato=2 "
                        Case "Respinte"
                            strWhere = strWhere & "AND EntitàAssenze.Stato=3 "
                    End Select
                End If
            End If
            'FiltroVisibilita
            strWhere = strWhere & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            strSql = strSql & strWhere
            strSql = strSql & " order by nominativo "
            MyDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))
            'assegno il dataset alla griglia del risultato
            dgVolontari.DataSource = MyDataSet
            Session("appDtsRisRicerca") = MyDataSet
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                dgVolontari.Columns(10).Visible = True
            Else
                dgVolontari.Columns(10).Visible = False
            End If
            dgVolontari.DataBind()
            dgVolontari.Visible = True
        Catch ex As Exception
        End Try
        If Request.QueryString("VengoDa") = "Ricerca" Then
            Select Case ddlstato.SelectedItem.Text
                Case "Seleziona"
                    dgVolontari.Columns(11).Visible = True
                    dgVolontari.Columns(12).Visible = True
                    dgVolontari.Columns(13).Visible = True
                    dgVolontari.Columns(14).Visible = True
                    dgVolontari.Columns(15).Visible = True
                    'Aggiunto da Alessandra Taballione il 29.03.2005
                    '*********************************************************************************
                    'blocco per la creazione della datatable per la stampa della ricerca
                    'nome e posizione di lettura delle colopnne a base 0
                    Dim NomeColonne(9) As String
                    Dim NomiCampiColonne(9) As String
                    'nome della colonna 
                    'e posizione nella griglia di lettura
                    NomeColonne(0) = "Nominativo"
                    NomeColonne(1) = "Cod. Fiscale"
                    NomeColonne(2) = "Data Nascita"
                    NomeColonne(3) = "Comune Nascita"
                    NomeColonne(4) = "Progetto"
                    NomeColonne(5) = "Ente"
                    NomeColonne(6) = "Totale giorni"
                    NomeColonne(7) = "Giorni Confermati"
                    NomeColonne(8) = "Giorni Annullati"
                    NomeColonne(9) = "Giorni da Confermare"

                    NomiCampiColonne(0) = "Nominativo"
                    NomiCampiColonne(1) = "CodiceFiscale"
                    NomiCampiColonne(2) = "DataNascita"
                    NomiCampiColonne(3) = "ComuneNascita"
                    NomiCampiColonne(4) = "Progetto"
                    NomiCampiColonne(5) = "Ente"
                    NomiCampiColonne(6) = "TotAssenze"
                    NomiCampiColonne(7) = "NConfermate"
                    NomiCampiColonne(8) = "NRespinte"
                    NomiCampiColonne(9) = "NProposte"

                    'carico un datatable che userò poi nella pagina di stampa
                    'il numero delle colonne è a base 0
                    Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 9, NomeColonne, NomiCampiColonne)
                    '*********************************************************
                Case "Registrate" ' OK
                    dgVolontari.Columns(11).Visible = False
                    dgVolontari.Columns(12).Visible = False
                    dgVolontari.Columns(13).Visible = False
                    dgVolontari.Columns(14).Visible = True
                    'Aggiunto da Alessandra Taballione il 29.03.2005
                    '*********************************************************************************
                    'blocco per la creazione della datatable per la stampa della ricerca
                    'nome e posizione di lettura delle colopnne a base 0
                    Dim NomeColonne(6) As String
                    Dim NomiCampiColonne(6) As String
                    'nome della colonna 
                    'e posizione nella griglia di lettura
                    NomeColonne(0) = "Nominativo"
                    NomeColonne(1) = "Cod. Fiscale"
                    NomeColonne(2) = "Data Nascita"
                    NomeColonne(3) = "Comune Nascita"
                    NomeColonne(4) = "Progetto"
                    NomeColonne(5) = "Ente"
                    NomeColonne(6) = "Giorni da Confermare"


                    NomiCampiColonne(0) = "Nominativo"
                    NomiCampiColonne(1) = "CodiceFiscale"
                    NomiCampiColonne(2) = "DataNascita"
                    NomiCampiColonne(3) = "ComuneNascita"
                    NomiCampiColonne(4) = "Progetto"
                    NomiCampiColonne(5) = "Ente"
                    NomiCampiColonne(6) = "NProposte"

                    'carico un datatable che userò poi nella pagina di stampa
                    'il numero delle colonne è a base 0
                    Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 6, NomeColonne, NomiCampiColonne)
                    '*********************************************************
                Case "Confermate"
                    dgVolontari.Columns(11).Visible = False
                    dgVolontari.Columns(12).Visible = True
                    dgVolontari.Columns(13).Visible = False
                    dgVolontari.Columns(14).Visible = False
                    'Aggiunto da Alessandra Taballione il 29.03.2005
                    '*********************************************************************************
                    'blocco per la creazione della datatable per la stampa della ricerca
                    'nome e posizione di lettura delle colopnne a base 0
                    Dim NomeColonne(6) As String
                    Dim NomiCampiColonne(6) As String
                    'nome della colonna 
                    'e posizione nella griglia di lettura
                    NomeColonne(0) = "Nominativo"
                    NomeColonne(1) = "Cod. Fiscale"
                    NomeColonne(2) = "Data Nascita"
                    NomeColonne(3) = "Comune Nascita"
                    NomeColonne(4) = "Progetto"
                    NomeColonne(5) = "Ente"
                    NomeColonne(6) = "Giorni Confermati"


                    NomiCampiColonne(0) = "Nominativo"
                    NomiCampiColonne(1) = "CodiceFiscale"
                    NomiCampiColonne(2) = "DataNascita"
                    NomiCampiColonne(3) = "ComuneNascita"
                    NomiCampiColonne(4) = "Progetto"
                    NomiCampiColonne(5) = "Ente"
                    NomiCampiColonne(6) = "NConfermate"

                    'carico un datatable che userò poi nella pagina di stampa
                    'il numero delle colonne è a base 0
                    Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 6, NomeColonne, NomiCampiColonne)
                    '*********************************************************
                Case "Respinte"
                    dgVolontari.Columns(11).Visible = False
                    dgVolontari.Columns(12).Visible = False
                    dgVolontari.Columns(13).Visible = True
                    dgVolontari.Columns(14).Visible = False
                    'Aggiunto da Alessandra Taballione il 29.03.2005
                    '*********************************************************************************
                    'blocco per la creazione della datatable per la stampa della ricerca
                    'nome e posizione di lettura delle colopnne a base 0
                    Dim NomeColonne(6) As String
                    Dim NomiCampiColonne(6) As String
                    'nome della colonna 
                    'e posizione nella griglia di lettura
                    NomeColonne(0) = "Nominativo"
                    NomeColonne(1) = "Cod. Fiscale"
                    NomeColonne(2) = "Data Nascita"
                    NomeColonne(3) = "Comune Nascita"
                    NomeColonne(4) = "Progetto"
                    NomeColonne(5) = "Ente"
                    NomeColonne(6) = "Giorni Annullati"

                    NomiCampiColonne(0) = "Nominativo"
                    NomiCampiColonne(1) = "CodiceFiscale"
                    NomiCampiColonne(2) = "DataNascita"
                    NomiCampiColonne(3) = "ComuneNascita"
                    NomiCampiColonne(4) = "Progetto"
                    NomiCampiColonne(5) = "Ente"
                    NomiCampiColonne(6) = "NRespinte"

                    'carico un datatable che userò poi nella pagina di stampa
                    'il numero delle colonne è a base 0
                    Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 6, NomeColonne, NomiCampiColonne)
                    '*********************************************************
            End Select
        Else
            dgVolontari.Columns(11).Visible = False
            dgVolontari.Columns(12).Visible = False
            dgVolontari.Columns(13).Visible = False
            dgVolontari.Columns(14).Visible = False
            dgVolontari.Columns(15).Visible = False
            'Aggiunto da Alessandra Taballione il 29.03.2005
            '*********************************************************************************
            'blocco per la creazione della datatable per la stampa della ricerca
            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(5) As String
            Dim NomiCampiColonne(5) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Nominativo"
            NomeColonne(1) = "Cod. Fiscale"
            NomeColonne(2) = "Data Nascita"
            NomeColonne(3) = "Comune Nascita"
            NomeColonne(4) = "Progetto"
            NomeColonne(5) = "Ente"

            NomiCampiColonne(0) = "Nominativo"
            NomiCampiColonne(1) = "CodiceFiscale"
            NomiCampiColonne(2) = "DataNascita"
            NomiCampiColonne(3) = "ComuneNascita"
            NomiCampiColonne(4) = "Progetto"
            NomiCampiColonne(5) = "Ente"

            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 5, NomeColonne, NomiCampiColonne)
            '*********************************************************
        End If
        If dgVolontari.Items.Count = 0 Then
            lblmessaggio.Text = "Nessun Dato estratto."
            CmdEsporta.Visible = False
        Else
            lblmessaggio.Text = "Risultato Ricerca Sanzioni Volontari."
            CmdEsporta.Visible = True
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        CaricaGriglia()
    End Sub

    Private Sub dgVolontari_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgVolontari.PageIndexChanged
        'AUTORE: MIchele d'Ascenzio
        'DATA: 03/11/2004
        'Cambia pag della Griglia
        dgVolontari.CurrentPageIndex = e.NewPageIndex
        dgVolontari.DataSource = Session("appDtsRisRicerca")
        dgVolontari.DataBind()
        dgVolontari.SelectedIndex = -1
    End Sub

    Private Sub dgVolontari_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgVolontari.SelectedIndexChanged
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Session("IdEnte") = dgVolontari.SelectedItem.Cells(2).Text
            Session("Denominazione") = dgVolontari.SelectedItem.Cells(10).Text
        End If
        Response.Redirect("SanzioniVolontari.aspx?IdAttivita=" & dgVolontari.SelectedItem.Cells(1).Text & "&IdEntita=" & dgVolontari.SelectedItem.Cells(0).Text & "&VengoDa=" & Request.QueryString("VengoDa") & "&Op=" & Request.QueryString("Op"))
    End Sub

    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If

    End Sub

    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim Reader As StreamReader
        Dim url As String
        NomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        Writer = New StreamWriter(mapPath & "\" & NomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        Writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To DTBRicerca.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            Writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & NomeUnivoco & ".CSV"

        Writer.Close()
        Writer = Nothing
        Return url
    End Function

End Class