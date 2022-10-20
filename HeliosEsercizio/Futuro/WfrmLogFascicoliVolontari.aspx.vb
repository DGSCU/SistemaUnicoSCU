Imports System.IO

Public Class WfrmLogFascicoliVolontari
    Inherits System.Web.UI.Page
    Dim SIGED As clsSiged

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        lblmess.Visible = False
        lblmess.Text = ""

        If Page.IsPostBack = False Then
            Session("DtbRicerca") = Nothing
            Session("DtbRicerca1") = Nothing
            CaricagrigliaEventi(0)
            'CmdRieseguiLog.Visible = True
            cmdRieseguiLog.Visible = True
            'imgAttendere.Width.Pixel(0)
            'imgAttendere.Visible = False
        Else
            'CmdRieseguiLog.Visible = False
            cmdRieseguiLog.Visible = True
            'imgAttendere.Width.Pixel(0)
            'imgAttendere.Visible = False
        End If
    End Sub

    Sub CaricagrigliaEventi(ByVal CK As Integer)
        Dim MyCommand As SqlClient.SqlDataAdapter
        Dim DsLog As DataSet = New DataSet
        MyCommand = New SqlClient.SqlDataAdapter("SP_LOGFASCICOLI_EVENTI", CType(Session("conn"), SqlClient.SqlConnection))
        MyCommand.SelectCommand.CommandType = CommandType.StoredProcedure
        MyCommand.SelectCommand.Parameters.Add("@TipoRicerca", SqlDbType.Int).Value = CK
        MyCommand.Fill(DsLog)
        dtgEventi.CurrentPageIndex = 0
        'controllo se ci sono dei record
        If DsLog.Tables(0).Rows.Count > 0 Then
            'al datasource sella combo passo il datareader
            dtgEventi.DataSource = DsLog
            Session("RisultatoGriglia") = DsLog
            CmdEsportaEventi.Visible = True
        Else
            CmdEsportaEventi.Visible = False
        End If
        dtgEventi.DataBind()
        Dim NomeColonne(DsLog.Tables(0).Columns.Count) As String
        Dim NomiCampiColonne(DsLog.Tables(0).Columns.Count) As String
        Dim intX As Integer
        For intX = 0 To DsLog.Tables(0).Columns.Count - 1
            NomeColonne(intX) = DsLog.Tables(0).Columns(intX).ColumnName
            NomiCampiColonne(intX) = DsLog.Tables(0).Columns(intX).ColumnName
        Next


        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(DsLog, DsLog.Tables(0).Columns.Count - 1, NomeColonne, NomiCampiColonne, 1)
    End Sub

    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String, ByVal GrigliaPassata As Integer)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della stored
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If
        If GrigliaPassata = 1 Then
            'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
            Session("DtbRicerca") = dt
        Else
            'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
            Session("DtbRicerca1") = dt
        End If

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            RadioButton2.Checked = False
            CaricagrigliaEventi(0)
            DtgDettagli.DataSource = Nothing
            DtgDettagli.DataBind()
            CmdEsportaDettagli.Visible = False
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked = True Then
            RadioButton1.Checked = False
            CaricagrigliaEventi(-1)
            DtgDettagli.DataSource = Nothing
            DtgDettagli.DataBind()
            CmdEsportaDettagli.Visible = False
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dtgEventi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgEventi.SelectedIndexChanged
        CaricagrigliaDettaglio(dtgEventi.SelectedItem.Cells(1).Text)
    End Sub

    Sub CaricagrigliaDettaglio(ByVal IdlogSel As Integer)

        Dim MyCommand As SqlClient.SqlDataAdapter
        Dim DsLog As DataSet = New DataSet
        MyCommand = New SqlClient.SqlDataAdapter("SP_LOGFASCICOLI_DETTAGLI", CType(Session("conn"), SqlClient.SqlConnection))
        MyCommand.SelectCommand.CommandType = CommandType.StoredProcedure
        MyCommand.SelectCommand.Parameters.Add("@IdLogFascicoliVolontari", SqlDbType.Int).Value = IdlogSel
        MyCommand.Fill(DsLog)
        DtgDettagli.CurrentPageIndex = 0
        'controllo se ci sono dei record
        If DsLog.Tables(0).Rows.Count > 0 Then
            'al datasource sella combo passo il datareader
            DtgDettagli.DataSource = DsLog
            CmdEsportaDettagli.Visible = True
            CmdEsportaEventi.Visible = True
        Else
            CmdEsportaDettagli.Visible = False
        End If
        DtgDettagli.DataBind()

        Dim NomeColonne(DsLog.Tables(0).Columns.Count) As String
        Dim NomiCampiColonne(DsLog.Tables(0).Columns.Count) As String
        Dim intX As Integer
        For intX = 0 To DsLog.Tables(0).Columns.Count - 1
            NomeColonne(intX) = DsLog.Tables(0).Columns(intX).ColumnName
            NomiCampiColonne(intX) = DsLog.Tables(0).Columns(intX).ColumnName
        Next


        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(DsLog, DsLog.Tables(0).Columns.Count - 1, NomeColonne, NomiCampiColonne, 2)

    End Sub

    Private Sub dtgEventi_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgEventi.PageIndexChanged
        DtgDettagli.DataSource = Nothing
        DtgDettagli.DataBind()
        CmdEsportaDettagli.Visible = False
        dtgEventi.SelectedIndex = -1
        dtgEventi.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dtgEventi.DataSource = Session("DtbRicerca")
        dtgEventi.DataBind()
    End Sub

    Private Sub cmdRieseguiLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRieseguiLog.Click
        If controlliSalvataggioServer() = True Then
            RieseguiLog()
        End If
    End Sub

    Private Sub RieseguiLog()
        Dim wsLocal As New WSHeliosInterno.HeliosInterno

        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim Mydataset As New DataSet
        Dim myCommand As System.Data.SqlClient.SqlCommand
        Dim x As Integer
        Dim strID As String
        cmdRieseguiLog.Visible = False
        'imgAttendere.Visible = True

        If TxtNumLog.Text <> "" Then
            strSql = "Select top " & TxtNumLog.Text & " IdLogFascicoliVolontari, Metodo, Username, IdEntità from LogFascicoliVolontari where eseguito=0"
        Else
            strSql = "Select  IdLogFascicoliVolontari, Metodo, Username, IdEntità from LogFascicoliVolontari where eseguito=0"
        End If

        'If Not dtrgenerico Is Nothing Then
        '    dtrgenerico.Close()
        '    dtrgenerico = Nothing
        'End If
        Mydataset = ClsServer.DataSetGenerico(strSql, Session("conn"))

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")

        Dim strEsito As String
        For x = 0 To Mydataset.Tables(0).Rows.Count - 1


            ''strEsito = wsLocal.GeneraFascicoloVolontari(Mydataset.Tables(0).Rows.Item(x).Item("Username"), Mydataset.Tables(0).Rows.Item(x).Item("IdEntità"), Mydataset.Tables(0).Rows.Item(x).Item("IdLogFascicoliVolontari"))

            strSql = "update LogFascicoliVolontari set Eseguito=2 where IdLogFascicoliVolontari = " & Mydataset.Tables(0).Rows.Item(x).Item("IdLogFascicoliVolontari")
            myCommand.CommandText = strSql
            myCommand.ExecuteNonQuery()

            'loggo su logfascicolivolontari
            strSql = "INSERT INTO LogFascicoliVolontari([Username],[Metodo],[IdEntità],[DataOraRichiesta],[DataOraEsecuzione],[Eseguito])"
            strSql = strSql & " VALUES('" & Mydataset.Tables(0).Rows.Item(x).Item("Username") & "','" & Mydataset.Tables(0).Rows.Item(x).Item("Metodo") & "','" & Mydataset.Tables(0).Rows.Item(x).Item("IdEntità") & "',getdate(),NULL,0)"
            myCommand.CommandText = strSql
            myCommand.ExecuteNonQuery()

            '---recupero l'id appena inserito

            strSql = "select @@identity as Id"
            dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrgenerico.Read()
            strID = dtrgenerico("Id")
            dtrgenerico.Close()
            dtrgenerico = Nothing

            'asincrono
            Dim resultAsync As IAsyncResult
            resultAsync = GeneraFascicoliVolontariAsincrono(Mydataset.Tables(0).Rows.Item(x).Item("Username"), Mydataset.Tables(0).Rows.Item(x).Item("IdEntità"), strID)

            ''ASSOCIO I FASCICOLI
            'AssociaFascicoliCreati(Mydataset.Tables(0).Rows.Item(x).Item("Username"), Mydataset.Tables(0).Rows.Item(x).Item("IdEntità"), strID)

            'If Mydataset.Tables(0).Rows.Item(x).Item("Metodo") = "GeneraFascicolo" Then
            '    If ClsUtility.GeneraFascicolo(Mydataset.Tables(0).Rows.Item(x).Item("Username"), Mydataset.Tables(0).Rows.Item(x).Item("IdEntità"), Session("conn"), strID) = "0" Then
            '        strSql = "update LogFascicoliVolontari set DataOraEsecuzione = getdate(), Eseguito=1 where IdLogFascicoliVolontari = " & strID
            '        myCommand.CommandText = strSql
            '        myCommand.ExecuteNonQuery()
            '    End If
            'Else
            '    If ClsUtility.GeneraFascicoloCumulati(Mydataset.Tables(0).Rows.Item(x).Item("Username"), Mydataset.Tables(0).Rows.Item(x).Item("IdEntità"), Session("conn"), strID) = "0" Then
            '        strSql = "update LogFascicoliVolontari set DataOraEsecuzione = getdate(), Eseguito=1 where IdLogFascicoliVolontari = " & strID
            '        myCommand.CommandText = strSql
            '        myCommand.ExecuteNonQuery()
            '    End If
            'End If

        Next x

        Response.Write("<script language=""javascript"">" & vbCrLf)
        Response.Write("alert(""Riesecuzione effettuata. Verificare nei Log!"");" & vbCrLf)
        Response.Write("</script>" & vbCrLf)
        CaricagrigliaEventi(-1)
        RadioButton2.Checked = True
        RadioButton1.Checked = False
        cmdRieseguiLog.Visible = True
        'imgAttendere.Visible = False
    End Sub


    Private Function GeneraFascicoliVolontariAsincrono(ByVal strUserName As String, ByVal strIdEntita As String, ByVal IntIdLog As Integer) As String
        'creata da simona cordella il 14/06/2017
        'routine che richiamoa in modo asincrono il metodo del WSINTERNO per la generazione dei Fascicolo dei volontari prnseti in graduatoria
        Dim localWS As New WSHeliosInterno.HeliosInterno
        'Dim ds As DataSet
        'Dim i As Integer
        'Dim strCodiceProgetto As String
        Dim ResultAsinc As IAsyncResult

        'richiamo WSDocumentazione

        localWS.Timeout = 1000000
        ResultAsinc = localWS.BeginGeneraFascicoloVolontari(strUserName, strIdEntita, IntIdLog, Nothing, "")

        'localWS.GenerazioneBOX16_BOX19Async(IdBandoAttivita, Session("Utente"))

        ' ResultAsinc = localWS.beg(IdBandoAttivita, Session("Utente"), Nothing, "")


    End Function

    Private Sub CmdEsportaEventi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsportaEventi.Click
        CmdEsportaEventi.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSVEventi(dtbRicerca)
    End Sub

    Private Sub StampaCSVEventi(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1Eventi.Visible = False
            CmdEsportaEventi.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1Eventi.Visible = True
            ApriCSV1Eventi.NavigateUrl = url
        End If

    End Sub

    Private Sub CmdEsportaDettagli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsportaDettagli.Click
        CmdEsportaDettagli.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca1")
        StampaCSVDettagli(dtbRicerca)
    End Sub

    Private Sub StampaCSVDettagli(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1Dettagli.Visible = False
            CmdEsportaDettagli.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1Dettagli.Visible = True
            ApriCSV1Dettagli.NavigateUrl = url
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

    Function controlliSalvataggioServer() As Boolean
        If TxtNumLog.Text.Trim = String.Empty Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire un numero."
            TxtNumLog.Focus()
            Return False
        End If

        Dim numeroLog As Integer
        Dim numeroLogInteger As Boolean
        numeroLogInteger = Integer.TryParse(TxtNumLog.Text.Trim, numeroLog)

        If numeroLogInteger = True Then
            If numeroLog = 0 Then
                lblmess.Visible = True
                lblmess.Text = "Il numero inserito deve essere maggiore di Zero."
                TxtNumLog.Focus()
                Return False
            End If
        Else
            lblmess.Visible = True
            lblmess.Text = "Il valore inserito deve essere un numero intero."
            TxtNumLog.Focus()
            Return False
        End If

        Return True

    End Function

   
    Private Function AssociaFascicoliCreati(ByVal strUserName As String, ByVal strIdEntita As String, ByVal IntIdLog As Integer) As Boolean
        'vado a leggere il log dei fascicoli
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim Mydataset As New DataSet

        Dim x As Integer

        Dim strCodiceVolontario As String

        'estraggo il codicevolontario da entità

        strSql = "Select CodiceVolontario from entità where identità in (" & strIdEntita.Replace("#", ",") & ")"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

        Do While dtrgenerico.Read()
            If strCodiceVolontario <> "" Then
                strCodiceVolontario = strCodiceVolontario & "#"
            End If
            strCodiceVolontario &= dtrgenerico("CodiceVolontario")
        Loop
        dtrgenerico.Close()
        dtrgenerico = Nothing

        Dim TmpVol As String()

        TmpVol = Split(strCodiceVolontario, "#")

        For i = 0 To UBound(TmpVol)
            strCodiceVolontario = TmpVol(i)

            RicercaFascicolo(strCodiceVolontario, strUserName)

        Next

    End Function

    Sub RicercaFascicolo(ByVal CodiceVolontario As String, ByVal strUserName As String)
        Dim myCommand As System.Data.SqlClient.SqlCommand

        Dim strSQL As String
        Dim dsUser As DataSet
        Dim dr As DataRow
        Dim riga As Integer

        Dim strNome As String
        Dim strCognome As String
        Dim titolario As String = " "
        Dim wsOUT As WS_SIGeD.MULTI_FASCICOLO
        Dim wsELENCO() As WS_SIGeD.FASCICOLO_DOCUMENTO_TROVATO
        Dim sEsito As String
        Dim IDSessioneSIGED As String

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")


        titolario = ForzaTitolarioVolontari(CodiceVolontario)
        'estraggo il nome e cognome dell'utente loggato
        strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & strUserName & "'"
        dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

        If dsUser.Tables(0).Rows.Count <> 0 Then
            strNome = dsUser.Tables(0).Rows(0).Item("Nome")
            strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
        End If
 
        SIGED = New clsSiged("", strNome, strCognome)
        If SIGED.Codice_Esito = 0 Then
            IDSessioneSIGED = SIGED.Esito
            CodiceVolontario = "%" & CodiceVolontario
            wsOUT = SIGED.SIGED_Ricerca_Fascicoli(CodiceVolontario, "", Trim(titolario), "", "", "")
        Else
            lblmess.Text = SIGED.Esito
        End If

        sEsito = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)
        If sEsito = 0 Then
            If SIGED.Esito_Occorrenze(wsOUT.ESITO) = "0" Then
                'SIGED_Esito_Occorrenze
                ' lblmess.Text = "Nessun fascicolo trovato"

            Else

                wsELENCO = wsOUT.ELENCO_DOCUMENTI

                SIGED.NormalizzaCodice(wsELENCO(riga).CODICEFASCICOLO)
                Dim NumFascicolo As String = Replace(wsELENCO(riga).NUMERO, " ", "")
                Dim DescrFascicolo As String = wsELENCO(riga).DESCRIZIONE.ToString
                ' Dim C As String = wsELENCO(riga).TITOLARIO
                Dim idFascicolo As String = SIGED.CodiceNormalizzato


                strSQL = "Update entità SET CodiceFascicolo ='" & NumFascicolo & "', " & _
                         " IDFascicolo = '" & idFascicolo & "', " & _
                         " DescrFascicolo = '" & Replace(DescrFascicolo, "'", "''") & "' "

                strSQL = strSQL & "WHERE CodiceVolontario='" & CodiceVolontario.Replace("%", "") & "'"

                myCommand.CommandText = strSQL
                myCommand.ExecuteNonQuery()

            End If
        Else

            If Left(wsOUT.ESITO, 5) = "02001" Then
                lblmess.Text = "Attenzione, la ricerca ha prodotto un numero elevato di dati. Si prega di ottimizzarla valorizzando ulteriori filtri."
            Else
                lblmess.Text = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))
            End If

        End If
        SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)

    End Sub

    Private Function NormalizzaTitolario(ByVal strTitolario) As String
        If strTitolario <> "" Then
            Dim strAppoTitolario As String
            strAppoTitolario = Mid(strTitolario, 1, InStr(strTitolario, "-") - 1)
            Return strAppoTitolario
        Else
            Return ""
        End If
    End Function

    Private Function ForzaTitolarioVolontari(ByVal CodiceVolontario As String) As String
        'creata da simona cordella il 30/09/2015 per la visualizazione del titolario dalla scheda del volontario
        Dim strSQL As String
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim TitolarioVolontario As String
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        strSQL = " Select bando.titolario "
        strSQL &= " FROM   entità "
        strSQL &= " INNER JOIN GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità "
        strSQL &= " INNER JOIN AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione "
        strSQL &= " INNER JOIN attività ON AttivitàSediAssegnazione.IDAttività = attività.IDAttività "
        strSQL &= " INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività "
        strSQL &= " INNER JOIN bando ON BandiAttività.IdBando = bando.IDBando "
        strSQL &= " WHERE entità.CodiceVolontario = '" & CodiceVolontario & "'"
        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            TitolarioVolontario = Replace(NormalizzaTitolario(dtrGenerico("titolario")), " ", "")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return TitolarioVolontario

    End Function
End Class