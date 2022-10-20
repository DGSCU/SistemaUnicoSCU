Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Imports System.Data.SqlClient
Imports System
Imports System.Data.SqlTypes
Imports System.Data
Imports System.Configuration.ConfigurationManager


Public Class ConsultaDomandeOnline
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Public dtsRisRicerca As DataSet
    Public dtsRisRicerca3 As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.ExpiresAbsolute = #1/1/1980#
        Response.AddHeader("Pragma", "no-cache")
        MaintainScrollPositionOnPostBack = True
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
       
        If Session("TipoUtente") = "E" Then
            'Disabilito la possibilità di selezionare la denominazione dell'ente
            lblDenominazioneEnte.Visible = False
            txtDenominazioneEnte.Visible = False
            lblCodiceEnte.Visible = False
            txtcodiceente.Visible = False
            divente.Visible = False
        Else
            lblDenominazioneEnte.Visible = True
            txtDenominazioneEnte.Visible = True
            lblCodiceEnte.Visible = True
            txtcodiceente.Visible = True
            divente.Visible = True
        End If
        If IsPostBack = False Then
            If VerificaAbilitazione(Session("Utente"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            If Request.QueryString("Tipo") = "SI" Then
                ddlIncorso.SelectedIndex = 0
            End If
            If Request.QueryString("Tipo") = "NO" Then
                ddlIncorso.SelectedIndex = 1
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            CaricaBando()
            'ChkEsportaTutti.Checked = False
        End If

    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        LblErrore.Text = ""
        If ddlIncorso.SelectedValue = "1" Then
            If ddlBandoOnLine.SelectedIndex = 0 Then
                LblErrore.Text = "ATTENZIONE! Si prega di specificare il Bando nella ricerca"
                Exit Sub
            End If
        End If
        Call CaricaGriglia()
    End Sub
    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        'utilizzo la session per memorizzare il dataset generato al momento della ricerca
        'Call CaricaGriglia()
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dtgRisultatoRicerca.DataBind()
        dtgRisultatoRicerca.SelectedIndex = -1
        'ControlloVolontariNew()  ora Antonello

    End Sub
    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        'carico la home
        Session("appDtsRisRicerca") = Nothing
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Sub CaricaGriglia()
        dtgRisultatoRicerca.CurrentPageIndex = 0
        Dim strSQL As String
        Dim strCondizione As String
        'preparo la query sulle risorse relative all'ente loggato Acquisito e Non
        strSQL = "select * from vw_dol_consultadomande "

        'strSQL = "SELECT DISTINCT " & _
        '         "Entità.IDEntità AS IDEntità, " & _
        '         "Attività.IDAttività AS IdAttività, " & _
        '         "Entità.Cognome + ' ' + Entità.Nome AS Nominativo, " & _
        '         "Attività.Titolo AS Titolo, " & _
        '         "Attività.CodiceEnte AS CodProg, " & _
        '         "enti.CodiceRegione, " & _
        '         "Enti.IdEnte as IdEnte, " & _
        '         "Enti.Denominazione as Ente ,entità.codicevolontario, " & _
        '         "Entità.codicefiscale as cf, entità.datanascita, cnascita.Denominazione as ComuneNascita, entità.telefono, entità.email, Bando.bandobreve "



        ''aggiungo il pulsante per vedere la maschera delle cronologie solo per i tipo utente U e R
        ''If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
        ''    strSQL = strSQL & ",'<img src='Images/Cronologia.gif' onClientclick='VisualizzaAtt(' + convert(varchar,Entità.IDEntità) + ')' STYLE='cursor:hand;' alt='Seleziona Nominativo' title='Seleziona Nominativo' /> ' as Crono "
        ''Else
        'strSQL = strSQL & ",'' as Pdf ,bando.Bando,entisediattuazioni.IDEnteSedeAttuazione,entisediattuazioni.Denominazione "
        ''End If

        'strSQL = strSQL & "FROM AttivitàEntiSediAttuazione " & _
        '         "INNER JOIN AttivitàEntità ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione = AttivitàEntità.IDAttivitàEnteSedeAttuazione " & _
        '         "RIGHT OUTER JOIN Entità ON Attivitàentità.IDEntità = Entità.IDEntità " & _
        '         "INNER JOIN StatiEntità ON Entità.IDStatoEntità = StatiEntità.IDStatoEntità " & _
        '         "INNER JOIN GRADUATORIEENTITà on graduatorieentità.identità = entità.identità " & _
        '         "INNER JOIN attivitàsediassegnazione on attivitàsediassegnazione.idattivitàsedeassegnazione = graduatorieentità.idattivitàsedeassegnazione " & _
        '         "INNER JOIN Entisedi on attivitàsediassegnazione.identesede = entisedi.identesede " & _
        '         "INNER JOIN entisediattuazioni ON entità.TMPIdSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
        '         "INNER JOIN Comuni ON Entisedi.IDComune = Comuni.IDComune " & _
        '         "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia " & _
        '         "INNER JOIN Regioni ON Provincie.IDRegione = Regioni.IDRegione " & _
        '         "inner join attività on attività.idattività = attivitàsediassegnazione.idattività " & _
        '         "INNER JOIN BandiAttività on BandiAttività.IdBandoAttività=attività.IDBandoAttività " & _
        '         "INNER JOIN Bando on Bando.IdBando=BandiAttività.IDBando " & _
        '         "inner join enti on attività.identepresentante = enti.idente " & _
        '         "INNER JOIN TIPIPROGETTO ON attività.idtipoprogetto=TIPIPROGETTO.idtipoprogetto " & _
        '         "INNER JOIN DOL_DomandePresentate ON Entità.DOL_id = DOL_DomandePresentate.id " & _
        '         " inner join comuni as Cnascita on entità.idcomunenascita = cnascita.idcomune " & _
        '         "LEFT JOIN StatiVerificaCFEntità ON Entità.IDStatiVerificaCFEntità = StatiVerificaCFEntità.IDStatiVerificaCFEntità "
      
        strCondizione = "WHERE "

        'se viene selezionato l'ente faccio il filtro su di lui
        'If Request.Params("Ente") = "OK" Then
        '    strSQL = strSQL & strCondizione & "Attività.IDEntePresentante = " & Session("IdEnte") & " "
        '    strCondizione = "AND "
        'End If
        'imposto le condizioni dinamicamente



        If txtCognomeVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Cognome LIKE '" & Replace(txtCognomeVolontario.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtNomeVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Nome LIKE '" & Replace(txtNomeVolontario.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        'If txtRegione.Text <> vbNullString Then
        '    strSQL = strSQL & strCondizione & "Regioni.Regione LIKE '" & Replace(txtRegione.Text, "'", "''") & "%' "
        '    strCondizione = "AND "
        'End If
        'If txtProvincia.Text <> vbNullString Then
        '    strSQL = strSQL & strCondizione & "Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%' "
        '    strCondizione = "AND "
        'End If
        'If txtComune.Text <> vbNullString Then
        '    strSQL = strSQL & strCondizione & "Comuni.Denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%' "
        '    strCondizione = "AND "
        'End If
        If Session("TipoUtente") = "E" Then
            strSQL = strSQL & strCondizione & "(CodiceEnte = '" & Session("txtCodEnte") & "' or CodiceEnteRiferimento like '" & Session("txtCodEnte") & "%') "
            'strSQL = strSQL & strCondizione & "CodiceEnte = '" & Session("txtCodEnte") & "' "
            strCondizione = "AND "

        Else
        If txtDenominazioneEnte.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Ente LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtcodiceente.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "CodiceEnte = '" & Replace(txtcodiceente.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        End If

        If txtTitoloProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Titolo LIKE '" & Replace(txtTitoloProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtCodiceProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "CodiceProgetto LIKE '" & Replace(txtCodiceProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If Trim(ddlBandoOnLine.SelectedItem.Text) <> "" Then
            strSQL = strSQL & strCondizione & " bandobreve = '" & Replace(ddlBandoOnLine.SelectedItem.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If


        ''prendera' in cosiderazione il nuovo campo di entità  ANTONELLO
        'If txtcodicesede.Text <> vbNullString Then
        '    strSQL = strSQL & strCondizione & "codicesede = '" & Replace(txtcodicesede.Text, "'", "''") & "' "
        '    strCondizione = "AND "
        'End If
        'If txtDenominazioneSede.Text <> vbNullString Then
        '    strSQL = strSQL & strCondizione & "nomesede LIKE '" & Replace(txtDenominazioneSede.Text, "'", "''") & "%' "
        '    strCondizione = "AND "
        'End If

        If txtcodicesede.Text <> vbNullString Then
            Dim valoretext As String
            valoretext = txtcodicesede.Text
            If (Not IsNumeric(valoretext)) Then
                'SetFocus(txtcodicesede.Text)
                'lblmessaggioRicerca.Text = "La ricerca non ha prodotto risultati."
                'Exit Sub
                Dim valore As String
                valore = "-1"
                strSQL = strSQL & strCondizione & "codicesede = '" & valore & "' "
                strCondizione = "AND "
            Else
                strSQL = strSQL & strCondizione & "codicesede = '" & Replace(txtcodicesede.Text, "'", "''") & "' "
                strCondizione = "AND "

            End If
        End If

        If ddlIncorso.SelectedValue = 0 Then 'in corso
            strSQL &= strCondizione & " getdate() between datainiziovolontari and datafinevolontari "
            strCondizione = "AND "
            dtgRisultatoRicerca.Columns(1).Visible = False 'CODICEVOLONTARIO
            dtgRisultatoRicerca.Columns(13).Visible = True 'PDF
        End If
        If ddlIncorso.SelectedValue = 1 Then 'non in corso
            strSQL &= strCondizione & " not getdate() between datainiziovolontari and datafinevolontari "
            strCondizione = "AND "
            dtgRisultatoRicerca.Columns(1).Visible = False 'CODICEVOLONTARIO
            dtgRisultatoRicerca.Columns(13).Visible = True 'PDF
        End If




        If txtCodiceVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & " codicevolontario= '" & Replace(txtCodiceVolontario.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If

        If txtCodiceFiscaleVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & " codicefiscale= '" & Replace(txtCodiceFiscaleVolontario.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        '

        'FiltroVisibilita 01/12/20104 da s.c.
        If Session("FiltroVisibilita") <> Nothing Then
            strSQL = strSQL & strCondizione & " MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            strCondizione = "AND "
        End If

        'per regioni
        If Session("TipoUtente") = "R" Then
            strSQL = strSQL & strCondizione & " IdRegioneCompetenza = '" & Session("IdRegioneCompetenzaUtente") & "' "
            strCondizione = "AND "
        End If

        'per utenze sedi
        If VerificaUtenzaSede(Session("Utente"), Session("conn")) Then
            strSQL = strSQL & strCondizione & " codicesede IN (Select spd.identesedeattuazionegestita from sedipassword sp inner join sedipassworddelega spd on sp.idsedepassword =spd.idsedepassword where sp.username='" & Session("Utente") & "') "
            strCondizione = "AND "
        End If


        'If Cbogmo.SelectedIndex > 0 Then
        '    If Cbogmo.SelectedValue = 1 Then
        '        strSQL &= strCondizione & "IsNull(Entità.GMO,'') <> '' "
        '        strCondizione = "AND "
        '    ElseIf Cbogmo.SelectedValue = 2 Then
        '        strSQL &= strCondizione & "IsNull(Entità.GMO,'') = '' "
        '        strCondizione = "AND "
        '    End If
        'End If

        'If Cbofami.SelectedIndex > 0 Then
        '    If Cbofami.SelectedValue = 1 Then
        '        strSQL &= strCondizione & "IsNull(Entità.FAMI,'') <> '' "
        '        strCondizione = "AND "
        '    ElseIf Cbofami.SelectedValue = 2 Then
        '        strSQL &= strCondizione & "IsNull(Entità.FAMI,'') = '' "
        '        strCondizione = "AND "
        '    End If
        'End If

        'If CboEsteroUe.SelectedIndex > 0 Then
        '    If CboEsteroUe.SelectedValue = 1 Then
        '        strSQL &= strCondizione & "Attività.EsteroUE = 'True'"
        '        strCondizione = "AND "
        '    ElseIf CboEsteroUe.SelectedValue = 2 Then
        '        strSQL &= strCondizione & "Attività.EsteroUE = 'False'"
        '        strCondizione = "AND "
        '    End If
        'End If

        'If CboTutoraggio.SelectedIndex > 0 Then
        '    If CboTutoraggio.SelectedValue = 1 Then
        '        strSQL &= strCondizione & "Attività.Tutoraggio = 'True'"
        '        strCondizione = "AND "
        '    ElseIf CboTutoraggio.SelectedValue = 2 Then
        '        strSQL &= strCondizione & "Attività.Tutoraggio = 'False'"
        '        strCondizione = "AND "
        '    End If
        'End If

        'If CboDurataProg.SelectedIndex > 0 Then

        '    strSQL &= strCondizione & "Attività.NMesi = " & CboDurataProg.SelectedValue
        '    strCondizione = "AND "

        'End If





        strSQL = strSQL & " order by codiceprogetto,codicesede, nominativo "


        ' ------

        ''eseguo la group by per il numero attività
        'strSQL = strSQL & " GROUP BY " & _
        '                  "Entità.IDEntità, " & _
        '                  "Attività.IDAttività, " & _
        '                  "Entità.Cognome + ' ' + Entità.Nome, " & _
        '                  "Attività.Titolo, " & _
        '                  "Attività.CodiceEnte, " & _
        '                  "Enti.CodiceRegione, " & _
        '                  "Enti.IdEnte, " & _
        '                  "Enti.Denominazione, " & _
        '                  "attività.CodiceEnte, " & _
        '                  "entità.codicevolontario,Entità.codicefiscale,bando.Bando,entisediattuazioni.IDEnteSedeAttuazione, entisediattuazioni.Denominazione order by Entità.Cognome + ' ' + Entità.Nome "
        'eseguo la query
        dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dtgRisultatoRicerca.DataSource = dtsRisRicerca

        dtgRisultatoRicerca.DataBind()
        If dtgRisultatoRicerca.Items.Count = 0 Then
            CmdEsportaCSV.Visible = False
            dtgRisultatoRicerca.Visible = False
            lblmessaggioRicerca.Text = "La ricerca non ha prodotto risultati."
        Else
            lblmessaggioRicerca.Text = ""
            dtgRisultatoRicerca.Visible = True
            CmdEsportaCSV.Visible = True
            'If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then

            'Else

            'End If
            CaricaDataTablePerStampa(dtsRisRicerca)
            Session("appDtsRisRicerca") = dtsRisRicerca
            CaricaDataTablePerStampa2(dtsRisRicerca)
            'Session("appDtsRisRicerca1") = dtsRisRicerca
            CmdEsportaCSV.Visible = True
            'ChkEsportaTutti.Visible = True
            ApriCSV1.Visible = False
            ApriCSV2.Visible = False
            ApriCSV3.Visible = False
        End If
    End Sub

    Private Sub dtgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand
        If e.CommandName = "seleziona" Then
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                Response.Redirect("WfrmVolontari.aspx?IdVol=" & e.Item.Cells(3).Text & "&IdAttivita=" & e.Item.Cells(2).Text)
            Else
                Response.Redirect("WfrmVolontari.aspx?IdVol=" & e.Item.Cells(3).Text & "&IdAttivita=" & e.Item.Cells(2).Text & "&Ente=OK")
            End If
        End If

        If e.CommandName = "selezionaPdf" Then

            Dim selectedItem As DataGridItem = e.Item
            Dim objHLink As HyperLink
            'Dim IdVolontario As String = selectedItem.Cells(3).Text
            'hlDownload.Visible = True
            'hlDownload.NavigateUrl = RecuperaDocumentoDomandaOnLine(e.Item.Cells(14).Text, Session("Utente"))
            'hlDownload.Text = "pdf"
            'hlDownload.Target = "_blank"


            objHLink = RecuperaDocumentoDomandaOnLine(e.Item.Cells(14).Text, Session("Utente"))

            hlDownload.Visible = True
            hlDownload.Text = objHLink.Text
            hlDownload.NavigateUrl = objHLink.NavigateUrl
            'hlDw.Visible = False
            'imgEsporta.Visible = True
            hlDownload.Target = "_blank"
           

            'ReadFileStream(e.Item.Cells(3).Text)
        End If

    End Sub

    Function DecodeToByte(ByVal enc As String) As Byte()
        Dim bt() As Byte
        bt = System.Convert.FromBase64String(enc)
        Return bt
    End Function
    Sub DecodeFile(ByVal srcFile As String, ByVal destFile As String)
        'Dim src As String
        'Dim sr As New IO.StreamReader(srcFile)
        'src = sr.ReadToEnd
        'sr.Close()
        Dim bt64 As Byte() = DecodeToByte(srcFile)
        If IO.File.Exists(destFile) Then
            IO.File.Delete(destFile)
        End If

        Dim sw As New IO.FileStream(destFile, IO.FileMode.CreateNew)
        sw.Write(bt64, 0, bt64.Length)
        sw.Close()
    End Sub

    Private Function RecuperaDocumentoDomandaOnLine(ByVal DOL_id As Integer, ByVal user As String) As HyperLink
        
        Dim oblLocalHLink As New HyperLink
        Dim WSInterno As New WSHeliosInterno.HeliosInterno
        Dim wsOut As String
        Dim strFile As String

        Dim strNomeFile As String
        Dim strCodiceProgettoSelezionato As String
        Dim strCodiceSedeSelezionata As String
        Dim strCodiceFiscale As String
        Try
            Dim dtr As Data.SqlClient.SqlDataReader
            'eseguo query per estrarre i dati da riportare nel nome file
            dtr = ClsServer.CreaDatareader("select codiceprogettoselezionato, codicesedeselezionata, codicefiscale FROM DOL_DomandePresentate WHERE (id = " & DOL_id & ")", Session("conn"))
            If dtr.HasRows = True Then
                Do While dtr.Read()
                    strCodiceProgettoSelezionato = dtr.GetValue(0) 'visualizzo stato
                    strCodiceSedeSelezionata = dtr.GetValue(1)
                    strCodiceFiscale = dtr.GetValue(2)
                Loop
            End If
            dtr.Close()
            dtr = Nothing

            strNomeFile = strCodiceProgettoSelezionato & "_" & strCodiceSedeSelezionata & "_" & strCodiceFiscale & ".pdf"
            'WSInterno.Url = ConfigurationSettings.AppSettings("URL_WSHeliosInterno")
            strFile = WSInterno.Recupera_DomandaDOL_Protocollo(DOL_id)
            If strFile = "errore" Then
                oblLocalHLink.Text = "Errore: PDF domanda non ancora disponibile"
                oblLocalHLink.NavigateUrl = ""
            End If

            DecodeFile(strFile, Server.MapPath("download/" & strNomeFile))
           
            oblLocalHLink.Text = strNomeFile
            oblLocalHLink.NavigateUrl = "download/" & strNomeFile
        Catch ex As Exception
            'MsgBox(ex.ToString)
            'MsgBox("Domanda non ancora disponibile")
        End Try
        Return oblLocalHLink



        'Dim CONDOL As SqlConnection = Session("Conn")
        ''Dim CONDOL As New SqlConnection

        ''CONDOL.ConnectionString = "user id=" & AppSettings("DOL_USERNAME") & ";password=" & AppSettings("DOL_PASSWORD") & ";data source=" & AppSettings("DOL_DATA_SOURCE") & ";persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=" & AppSettings("DOL_NAME") & ""
        ''CONDOL.Open()
        'Dim da As New SqlDataAdapter _
        '     ("SELECT distinct filedomanda, CodiceProgettoSelezionato + '_' + convert(varchar(20),CodiceSedeSelezionata) +'_' + codicefiscale +'.pdf' as FileName FROM sqldol.domandaonline.dbo.domandapartecipazione WHERE id = " & DOL_id, CONDOL)
        ''("SELECT distinct filedomanda, CodiceProgettoSelezionato + '_' + convert(varchar(20),CodiceSedeSelezionata) +'_' + codicefiscale +'.pdf' as FileName FROM domandapartecipazione WHERE id = " & DOL_id, CONDOL)
        'Dim cb As SqlCommandBuilder = New SqlCommandBuilder(da)
        'Dim ds As New DataSet
        'Dim nomefile As String

        ''Dim user As String
        'Dim paht As String
        'Try
        '    'Dim oblLocalHLink As New HyperLink

        '    da.Fill(ds, "_FileTest")
        '    Dim rw As DataRow
        '    rw = ds.Tables("_FileTest").Rows(0)

        '    ' Make sure you have some rows
        '    Dim i As Integer = ds.Tables("_FileTest").Rows.Count
        '    If i > 0 Then
        '        Dim bBLOBStorage() As Byte = _
        '        ds.Tables("_FileTest").Rows(0)("filedomanda")

        '        oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("Filename")
        '        nomefile = user & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now) & "_" & ds.Tables("_FileTest").Rows(0)("Filename")
        '        oblLocalHLink.NavigateUrl = FileByteToPathDomandeOnLine(bBLOBStorage, nomefile)

        '        'paht = FileByteToPathDomandeOnLine(bBLOBStorage, nomefile)
        '    End If


        '    Return oblLocalHLink
        'Catch ex As Exception
        '    MsgBox(ex.ToString)
        'End Try
    End Function
    Private Shared Function FileByteToPathDomandeOnLine(ByVal dataBuffer As Byte(), ByVal nomeFile As String) As String
        'dichiaro una variabile byte che bufferizza (carica in memoria) il file template richiesto
        'e trasformato in base64
        Dim fs As FileStream
        Dim myPath As New System.Web.UI.Page

        If File.Exists(myPath.Server.MapPath("download") & "\" & nomeFile) Then
            File.Delete(myPath.Server.MapPath("download") & "\" & nomeFile)
        End If
        'passo il template al filestream
        fs = New FileStream(myPath.Server.MapPath("download") & "\" & nomeFile, FileMode.Create, FileAccess.Write)
        'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
        If (dataBuffer.Length > 0) Then
            fs.Write(dataBuffer, 0, dataBuffer.Length)
        End If

        'chiudo lo streaming
        fs.Close()
        Return "download\" & nomeFile
    End Function
    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer



        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(14) As String
        Dim NomiCampiColonne(14) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        'NomeColonne(0) = "Codice Volontario"
        NomeColonne(0) = "Cognome"
        NomeColonne(1) = "Nome"
        NomeColonne(2) = "Data Nascita"
        NomeColonne(3) = "Comune / Nazione di nascita"
        NomeColonne(4) = "Codice Fiscale"
        NomeColonne(5) = "Telefono"
        NomeColonne(6) = "Email"
        NomeColonne(7) = "Codice Ente"
        NomeColonne(8) = "Denominazione Ente"
        NomeColonne(9) = "Codice Progetto"
        NomeColonne(10) = "Titolo Progetto"
        NomeColonne(11) = "Codice Sede"
        NomeColonne(12) = "Denominazione Sede"
        NomeColonne(13) = "GMO"
        NomeColonne(14) = "Codice Ente Riferimento"


        'NomiCampiColonne(0) = "CodiceVolontario"
        NomiCampiColonne(0) = "Cognome"
        NomiCampiColonne(1) = "Nome"
        NomiCampiColonne(2) = "datanascita"
        NomiCampiColonne(3) = "ComuneNascita"
        NomiCampiColonne(4) = "codicefiscale"
        NomiCampiColonne(5) = "Telefono"
        NomiCampiColonne(6) = "Email"
        NomiCampiColonne(7) = "codiceente"
        NomiCampiColonne(8) = "Ente"
        NomiCampiColonne(9) = "codiceprogetto"
        NomiCampiColonne(10) = "Titolo"
        NomiCampiColonne(11) = "codicesede"
        NomiCampiColonne(12) = "nomesede"
        NomiCampiColonne(13) = "GMO"
        NomiCampiColonne(14) = "CodiceEnteRiferimento"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 14
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 14
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If


        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub
    Sub CaricaDataTablePerStampa2(ByVal DataSetDaScorrere As DataSet)
        Dim dt1 As New DataTable
        Dim dr1 As DataRow
        Dim i As Integer
        Dim x As Integer



        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(10) As String
        Dim NomiCampiColonne(10) As String
        'nome della colonna 
        'e posizione nella griglia di lettura

        NomeColonne(0) = "Nome"
        NomeColonne(1) = "Cognome"
        NomeColonne(2) = "Codice Fiscale"
        NomeColonne(3) = "Codice Progetto"
        NomeColonne(4) = "Codice Sede"
        NomeColonne(5) = "Esito Selezione"
        NomeColonne(6) = "Punteggio"
        NomeColonne(7) = "Tipo Posto"
        NomeColonne(8) = "Codice Sede Primo Giorno"
        NomeColonne(9) = "Codice Sede Secondaria"
        'NomeColonne(10) = "Stato Civile"
        'NomeColonne(11) = "Codice Fiscale Coniuge"
        NomeColonne(10) = "Data Inizio Prevista"


        NomiCampiColonne(0) = "Nome"
        NomiCampiColonne(1) = "Cognome"
        NomiCampiColonne(2) = "codicefiscale"
        NomiCampiColonne(3) = "codiceprogetto"
        NomiCampiColonne(4) = "codicesede"
        NomiCampiColonne(5) = "esitoselezione"
        NomiCampiColonne(6) = "Punteggio"
        NomiCampiColonne(7) = "tipoposto"
        NomiCampiColonne(8) = "codicesedeprimogiorno"
        NomiCampiColonne(9) = "codicesedesecondaria"
        'NomiCampiColonne(10) = "statocivile"
        'NomiCampiColonne(11) = "codicefiscaleconiuge"
        NomiCampiColonne(10) = "datainizioprevista"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 10
            dt1.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr1 = dt1.NewRow()
                For x = 0 To 10
                    dr1(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt1.Rows.Add(dr1)
            Next
        End If


        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca1") = dt1

    End Sub

    Function StampaCSV(ByVal DTBRicerca As DataTable)

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            ApriCSV2.Visible = False
            ApriCSV3.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpElencoDomandeinCorso" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV", True, System.Text.Encoding.Default)
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


            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"


            Writer.Close()
            Writer = Nothing

        End If

    End Function
    Function StampaCSV1(ByVal DTBRicerca1 As DataTable)

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca1.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            ApriCSV2.Visible = False
            ApriCSV3.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpElencoDomande" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV", True, System.Text.Encoding.Default)
            'Creazione dell'inntestazione del CSV
            Dim intNumCol As Int64 = DTBRicerca1.Columns.Count
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca1.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            Writer.WriteLine(xLinea)
            xLinea = vbNullString

            'Scorro tutte le righe del datatable e riempio il CSV
            For i = 0 To DTBRicerca1.Rows.Count - 1

                For j = 0 To intNumCol - 1
                    If IsDBNull(DTBRicerca1.Rows(CInt(i)).Item(CInt(j))) = True Then
                        xLinea &= vbNullString & ";"
                    Else
                        xLinea &= ClsUtility.FormatExport(DTBRicerca1.Rows(CInt(i)).Item(CInt(j))) & ";"
                    End If
                Next

                Writer.WriteLine(xLinea)
                xLinea = vbNullString

            Next


            ApriCSV2.Visible = True
            ApriCSV2.NavigateUrl = "download\" & NomeUnivoco & ".CSV"


            Writer.Close()
            Writer = Nothing

        End If

    End Function
    Function StampaCSV3(ByVal DTBRicerca1 As DataTable)

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca1.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            ApriCSV2.Visible = False
            ApriCSV3.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpElencoDomandeTerminate" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV", True, System.Text.Encoding.Default)
            'Creazione dell'inntestazione del CSV
            Dim intNumCol As Int64 = DTBRicerca1.Columns.Count
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca1.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            Writer.WriteLine(xLinea)
            xLinea = vbNullString

            'Scorro tutte le righe del datatable e riempio il CSV
            For i = 0 To DTBRicerca1.Rows.Count - 1

                For j = 0 To intNumCol - 1
                    If IsDBNull(DTBRicerca1.Rows(CInt(i)).Item(CInt(j))) = True Then
                        xLinea &= vbNullString & ";"
                    Else
                        xLinea &= ClsUtility.FormatExport(DTBRicerca1.Rows(CInt(i)).Item(CInt(j))) & ";"
                    End If
                Next

                Writer.WriteLine(xLinea)
                xLinea = vbNullString

            Next


            ApriCSV3.Visible = True
            ApriCSV3.NavigateUrl = "download\" & NomeUnivoco & ".CSV"


            Writer.Close()
            Writer = Nothing

        End If

    End Function
    Sub CaricaDataTablePerStampa3(ByVal DataSetDaScorrere As DataSet)
        Dim dt2 As New DataTable
        Dim dr2 As DataRow
        Dim i As Integer
        Dim x As Integer



        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(66) As String
        Dim NomiCampiColonne(66) As String
        'nome della colonna 
        'e posizione nella griglia di lettura

        NomeColonne(0) = "CodiceProgettoSelezionato"
        NomeColonne(1) = "CodiceSedeSelezionata"
        NomeColonne(2) = "CodiceEnteRiferimento"
        NomeColonne(3) = "DataPresentazione"
        NomeColonne(4) = "CodiceFiscale"
        NomeColonne(5) = "Nome"
        NomeColonne(6) = "Cognome"
        NomeColonne(7) = "Genere"
        NomeColonne(8) = "DataNascita"
        NomeColonne(9) = "LuogoNascita"
        NomeColonne(10) = "NazioneNascita"
        NomeColonne(11) = "Cittadinanza"
        NomeColonne(12) = "Telefono"
        NomeColonne(13) = "Email"
        NomeColonne(14) = "ComuneResidenza"
        NomeColonne(15) = "ProvinciaResidenza"
        NomeColonne(16) = "ViaResidenza"
        NomeColonne(17) = "CivicoResidenza"
        NomeColonne(18) = "CapResidenza"
        NomeColonne(19) = "ComuneRecapito"
        NomeColonne(20) = "ProvinciaRecapito"
        NomeColonne(21) = "ViaRecapito"
        NomeColonne(22) = "CivicoRecapito"
        NomeColonne(23) = "CapRecapito"
        NomeColonne(24) = "GMO"
        NomeColonne(25) = "Motivazione"
        NomeColonne(26) = "CodiceDichiarazioneCittadinanza"
        NomeColonne(27) = "CondannePenali"
        NomeColonne(28) = "DisponibileSubentroStessoProgetto"
        NomeColonne(29) = "DisponibileSubentroAltroProgetto"
        NomeColonne(30) = "AltreDichiarazioniOk"
        NomeColonne(31) = "PrecedentiEnte"
        NomeColonne(32) = "PrecedentiEnteDescrizione"
        NomeColonne(33) = "PrecedentiAltriEnti"
        NomeColonne(34) = "PrecedentiAltriEntiDescrizione"
        NomeColonne(35) = "PrecedentiImpiego"
        NomeColonne(36) = "PrecedentiImpiegoDescrizione"

        NomeColonne(37) = "TitoloStudio"
        NomeColonne(38) = "FormazioneDisciplina"
        NomeColonne(39) = "FormazioneAnno"
        NomeColonne(40) = "FormazioneItalia"
        NomeColonne(41) = "FormazioneIstituto"
        NomeColonne(42) = "FormazioneEnte"

        NomeColonne(43) = "IscrizioneSuperioreAnno"
        NomeColonne(44) = "IscrizioneSuperioreIstituto"
        NomeColonne(45) = "IscrizioneLaureaAnno"
        NomeColonne(46) = "IscrizioneLaureaCorso"
        NomeColonne(47) = "IscrizioneLaureaIstituto"

        NomeColonne(48) = "TitoloStudioUlteriore"
        NomeColonne(49) = "FormazioneDisciplinaUlteriore"
        NomeColonne(50) = "FormazioneAnnoUlteriore"
        NomeColonne(51) = "FormazioneItaliaUlteriore"
        NomeColonne(52) = "FormazioneIstitutoUlteriore"
        NomeColonne(53) = "FormazioneEnteUlteriore"

        NomeColonne(54) = "CorsiEffettuati"
        NomeColonne(55) = "Specializzazioni"
        NomeColonne(56) = "Competenze"
        NomeColonne(57) = "Altro"
        NomeColonne(58) = "ProgettoGaranziaGiovani"
        NomeColonne(59) = "DichiarazioneResidenza"
        NomeColonne(60) = "DichiarazioneRequisitiGaranziaGiovani"
        NomeColonne(61) = "DataPresaInCaricoGaranziaGiovani"
        NomeColonne(62) = "LuogoPresaInCaricoGaranziaGiovani"
        NomeColonne(63) = "DataDIDGaranziaGiovani"
        NomeColonne(64) = "LuogoDIDGaranziaGiovani"
        NomeColonne(65) = "AlternativaRequisitiGaranziaGiovani"
        NomeColonne(66) = "NomeFileCV"


        NomiCampiColonne(0) = "CodiceProgettoSelezionato"
        NomiCampiColonne(1) = "CodiceSedeSelezionata"
        NomiCampiColonne(2) = "CodiceEnteRiferimento"
        NomiCampiColonne(3) = "DataPresentazione"
        NomiCampiColonne(4) = "CodiceFiscale"
        NomiCampiColonne(5) = "Nome"
        NomiCampiColonne(6) = "Cognome"
        NomiCampiColonne(7) = "Genere"
        NomiCampiColonne(8) = "DataNascita"
        NomiCampiColonne(9) = "LuogoNascita"
        NomiCampiColonne(10) = "NazioneNascita"
        NomiCampiColonne(11) = "Cittadinanza"
        NomiCampiColonne(12) = "Telefono"
        NomiCampiColonne(13) = "Email"
        NomiCampiColonne(14) = "ComuneResidenza"
        NomiCampiColonne(15) = "ProvinciaResidenza"
        NomiCampiColonne(16) = "ViaResidenza"
        NomiCampiColonne(17) = "CivicoResidenza"
        NomiCampiColonne(18) = "CapResidenza"
        NomiCampiColonne(19) = "ComuneRecapito"
        NomiCampiColonne(20) = "ProvinciaRecapito"
        NomiCampiColonne(21) = "ViaRecapito"
        NomiCampiColonne(22) = "CivicoRecapito"
        NomiCampiColonne(23) = "CapRecapito"
        NomiCampiColonne(24) = "GMO"
        NomiCampiColonne(25) = "Motivazione"
        NomiCampiColonne(26) = "CodiceDichiarazioneCittadinanza"
        NomiCampiColonne(27) = "CondannePenali"
        NomiCampiColonne(28) = "DisponibileSubentroStessoProgetto"
        NomiCampiColonne(29) = "DisponibileSubentroAltroProgetto"
        NomiCampiColonne(30) = "AltreDichiarazioniOk"
        NomiCampiColonne(31) = "PrecedentiEnte"
        NomiCampiColonne(32) = "PrecedentiEnteDescrizione"
        NomiCampiColonne(33) = "PrecedentiAltriEnti"
        NomiCampiColonne(34) = "PrecedentiAltriEntiDescrizione"
        NomiCampiColonne(35) = "PrecedentiImpiego"
        NomiCampiColonne(36) = "PrecedentiImpiegoDescrizione"

        NomiCampiColonne(37) = "TitoloStudio"
        NomiCampiColonne(38) = "FormazioneDisciplina"
        NomiCampiColonne(39) = "FormazioneAnno"
        NomiCampiColonne(40) = "FormazioneItalia"
        NomiCampiColonne(41) = "FormazioneIstituto"
        NomiCampiColonne(42) = "FormazioneEnte"

        NomiCampiColonne(43) = "IscrizioneSuperioreAnno"
        NomiCampiColonne(44) = "IscrizioneSuperioreIstituto"
        NomiCampiColonne(45) = "IscrizioneLaureaAnno"
        NomiCampiColonne(46) = "IscrizioneLaureaCorso"
        NomiCampiColonne(47) = "IscrizioneLaureaIstituto"

        NomiCampiColonne(48) = "TitoloStudioUlteriore"
        NomiCampiColonne(49) = "FormazioneDisciplinaUlteriore"
        NomiCampiColonne(50) = "FormazioneAnnoUlteriore"
        NomiCampiColonne(51) = "FormazioneItaliaUlteriore"
        NomiCampiColonne(52) = "FormazioneIstitutoUlteriore"
        NomiCampiColonne(53) = "FormazioneEnteUlteriore"

        NomiCampiColonne(54) = "CorsiEffettuati"
        NomiCampiColonne(55) = "Specializzazioni"
        NomiCampiColonne(56) = "Competenze"
        NomiCampiColonne(57) = "Altro"
        NomiCampiColonne(58) = "ProgettoGaranziaGiovani"
        NomiCampiColonne(59) = "DichiarazioneResidenza"
        NomiCampiColonne(60) = "DichiarazioneRequisitiGaranziaGiovani"
        NomiCampiColonne(61) = "DataPresaInCaricoGaranziaGiovani"
        NomiCampiColonne(62) = "LuogoPresaInCaricoGaranziaGiovani"
        NomiCampiColonne(63) = "DataDIDGaranziaGiovani"
        NomiCampiColonne(64) = "LuogoDIDGaranziaGiovani"
        NomiCampiColonne(65) = "AlternativaRequisitiGaranziaGiovani"
        NomiCampiColonne(66) = "NomeFileCV"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 66
            dt2.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr2 = dt2.NewRow()
                For x = 0 To 66
                    dr2(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt2.Rows.Add(dr2)
            Next
        End If


        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca3") = dt2

    End Sub
    Protected Sub CmdEsportaCSV_Click(sender As Object, e As EventArgs) Handles CmdEsportaCSV.Click
        CmdEsportaCSV.Visible = False
        Call CaricaDomandeTotali()
        If ddlIncorso.SelectedItem.Text = "NO" Then ' fine bando
            'Call CaricaDomandeTotali()
            StampaCSV1(Session("DtbRicerca1")) 'csv per importazione graduatorie
            StampaCSV3(Session("DtbRicerca3")) 'csv completo con tutti i dati

        Else 'SI' BANDO IN CORSO
            'StampaCSV(Session("DtbRicerca")) 'csv parziale con i soli dati di base delle domande presentate per i bandi in corso
            StampaCSV3(Session("DtbRicerca3")) 'csv completo con tutti i dati
        End If
    End Sub
    Private Sub CaricaBando()
        strsql = "SELECT Bando.idBando,bando.bandobreve,bando.annobreve  "
        strsql = strsql & " FROM bando"
        strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql = strsql & " WHERE bando.dol =1 and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strsql = strsql & " AND bando.datainiziovolontari is not null "
        'If Session("TipoUtente") = "E" Then
        '    strsql = strsql & " and bando.idbando in (select distinct idbando from bandiattività a inner join attività b on a.idbandoattività = b.idbandoattività where b.identepresentante = " & Session("IdEnte") & ")"
        'End If
        strsql = strsql & " UNION "
        strsql = strsql & " SELECT  '0','','9999'  from bando "
        strsql = strsql & " ORDER BY Bando.annobreve desc ,Bando.idbando"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        ddlBandoOnLine.DataSource = dtrgenerico
        ddlBandoOnLine.DataTextField = "bandobreve"
        ddlBandoOnLine.DataValueField = "IdBando"
        ddlBandoOnLine.DataBind()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Private Sub CaricaDomandeTotali()
        Dim strSQL As String
        Dim strCondizione As String
        'preparo la query sulle risorse relative all'ente loggato Acquisito e Non
        strSQL = "select a.CodiceEnteRiferimento,b.* from vw_dol_consultadomande a inner join VW_DOL_EXPORT_DOMANDE_COMPLETO_V2 b on a.identità = b.identità "
        strCondizione = "WHERE "
        If txtCognomeVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "a.Cognome LIKE '" & Replace(txtCognomeVolontario.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtNomeVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "a.Nome LIKE '" & Replace(txtNomeVolontario.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If

        If Session("TipoUtente") = "E" Then
            strSQL = strSQL & strCondizione & "(a.CodiceEnte = '" & Session("txtCodEnte") & "' or a.CodiceEnteRiferimento like '" & Session("txtCodEnte") & "%') "
            strCondizione = "AND "
        Else
            If txtDenominazioneEnte.Text <> vbNullString Then
                strSQL = strSQL & strCondizione & "a.Ente LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%' "
                strCondizione = "AND "
            End If
            If txtcodiceente.Text <> vbNullString Then
                strSQL = strSQL & strCondizione & "a.CodiceEnte = '" & Replace(txtcodiceente.Text, "'", "''") & "' "
                strCondizione = "AND "
            End If
        End If

        If txtTitoloProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "a.Titolo LIKE '" & Replace(txtTitoloProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtCodiceProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "a.CodiceProgetto LIKE '" & Replace(txtCodiceProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If Trim(ddlBandoOnLine.SelectedItem.Text) <> "" Then
            strSQL = strSQL & strCondizione & " a.bandobreve = '" & Replace(ddlBandoOnLine.SelectedItem.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If

        'prendera' in cosiderazione il nuovo campo di entità  ANTONELLO
        If txtcodicesede.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "a.codicesede = '" & Replace(txtcodicesede.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        If txtDenominazioneSede.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "a.nomesede LIKE '" & Replace(txtDenominazioneSede.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If


        If ddlIncorso.SelectedValue = 0 Then 'in corso
            strSQL &= strCondizione & " getdate() between a.datainiziovolontari and a.datafinevolontari "
            strCondizione = "AND "
        End If
        If ddlIncorso.SelectedValue = 1 Then 'non in corso
            strSQL &= strCondizione & " not getdate() between a.datainiziovolontari and a.datafinevolontari "
            strCondizione = "AND "
        End If


        If txtCodiceVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & " a.codicevolontario= '" & Replace(txtCodiceVolontario.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If

        If txtCodiceFiscaleVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & " a.codicefiscale= '" & Replace(txtCodiceFiscaleVolontario.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        '

        'FiltroVisibilita 
        If Session("FiltroVisibilita") <> Nothing Then
            strSQL = strSQL & strCondizione & " a.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            strCondizione = "AND "
        End If

        'per regioni
        If Session("TipoUtente") = "R" Then
            strSQL = strSQL & strCondizione & " IdRegioneCompetenza = '" & Session("IdRegioneCompetenzaUtente") & "' "
            strCondizione = "AND "
        End If

        'per utenze sedi
        If VerificaUtenzaSede(Session("Utente"), Session("conn")) Then
            strSQL = strSQL & strCondizione & " codicesede IN (Select spd.identesedeattuazionegestita from sedipassword sp inner join sedipassworddelega spd on sp.idsedepassword =spd.idsedepassword where sp.username='" & Session("Utente") & "') "
            strCondizione = "AND "
        End If

        strSQL = strSQL & " order by a.codiceprogetto,a.codicesede, a.nominativo "

        'eseguo la query
        dtsRisRicerca3 = ClsServer.DataSetGenerico(strSQL, Session("conn"))
        'assegno il dataset alla griglia del risultat

        CaricaDataTablePerStampa3(dtsRisRicerca3)


        ApriCSV3.Visible = True

    End Sub

    Public Sub ReadFileStream(ByVal idvolo As String)

        Dim strSql As String = "SELECT TOP(1) filename,binData FROM entitàdocumenti where identità= " & idvolo
        Dim comando As SqlClient.SqlCommand
        comando = New SqlClient.SqlCommand(strSql, Session("conn"))
        comando.CommandTimeout = 300
        'dtrLocal = comando.ExecuteReader()


        Using reader As SqlDataReader = comando.ExecuteReader()

            While reader.Read()
                Dim path As String = reader.GetString(0)
                Dim transactionContext As Byte() = reader.GetSqlBytes(1).Buffer

                Using fileStream As Stream = New SqlFileStream(path, transactionContext, FileAccess.Read, FileOptions.SequentialScan, allocationSize:=0)

                    For index As Long = 0 To fileStream.Length - 1
                        Console.WriteLine(fileStream.ReadByte())
                    Next
                End Using
            End While
        End Using
    End Sub

    Private Function VerificaAbilitazione(ByVal Utente As String, ByVal Conn As SqlClient.SqlConnection) As Boolean

        '** Verifico se l'utenza è abilitata alla visibilità del flag che consente il caricamento della programamzione con volontari e progetti terminati
        '** profilio menu creato appositamente per le richieste regionali
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Consulta Domande On Line'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Conn)

        VerificaAbilitazione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return VerificaAbilitazione
    End Function



    Private Function VerificaUtenzaSede(ByVal Utente As String, ByVal Conn As SqlClient.SqlConnection) As Boolean

        '** Verifico se l'utenza è di tipo SEDE
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Verifica username
        strSql = "SELECT USERNAME FROM SEDIPASSWORD WHERE USERNAME='" & Utente & "' "
        dtrgenerico = ClsServer.CreaDatareader(strSql, Conn)

        VerificaUtenzaSede = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return VerificaUtenzaSede

    End Function

    'Private Sub ChkEsportaTutti_CheckedChanged(sender As Object, e As System.EventArgs) Handles ChkEsportaTutti.CheckedChanged
    '    If ChkEsportaTutti.Checked = True Then
    '        CmdEsportaZip.Visible = True
    '    Else
    '        CmdEsportaZip.Visible = False
    '    End If
    'End Sub

    'Protected Sub CmdEsportaZip_Click(sender As Object, e As EventArgs) Handles CmdEsportaZip.Click
    '    CmdEsportaZip.Visible = False

    '    'lanciare procedura creazione cartella e zip


    'End Sub
End Class