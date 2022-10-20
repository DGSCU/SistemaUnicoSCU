Imports System.Data.SqlClient
Imports System.IO

Public Class WfrmRicercaVolontariStampa
    Inherits System.Web.UI.Page
    Public dtsRisRicerca As DataSet
    Dim dtsGenerico As DataSet
    Private Const INDEX_DATAGRID_DTGRISULTATORICERCA_CHECKBOX_SELEZIONA As Integer = 1
    Private Const INDEX_DATAGRID_DTGRISULTATORICERCA_IDVOLONTARIO As Integer = 4

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If

 
    End Sub
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
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
    Private Sub CancellaMessaggi()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()

        If (Session("STRINGACHECKSEL") Is Nothing Or Session("STRINGACHECKSEL") = String.Empty) Then
            fldSetGestioneVolontariSelezionati.Visible = False
        Else
            fldSetGestioneVolontariSelezionati.Visible = True
        End If

        If Page.IsPostBack = False Then

            If Session("STRINGACHECKSEL") Is Nothing Then
                'imposto a vuoto la sessione che mi tiene in memoria gli id selezionati
                Session("STRINGACHECKSEL") = String.Empty
                'dimensiono a vuoto il vettore con gli id dei volontari
                ReDim Session("JonListaIdVolontari")(0)
                Session("JonListaIdVolontari")(0) = ""
            End If
            Call CaricaDatiIniziali()

        End If

        If Session("TipoUtente") = "U" Then
            NascondiMenuLaterale()
        End If
    End Sub

    Sub CaricaGriglia()
        dtgRisultatoRicerca.CurrentPageIndex = 0
        dtgRisultatoRicerca.Visible = True
        Dim strSQL As String
        Dim strCondizione As String



        'preparo la query sulle risorse relative all'ente loggato Acquisito e Non
        strSQL = "SELECT DISTINCT " & _
                 "Entità.IDEntità AS IDEntità, " & _
                 "isnull(Entità.AltreInformazioni,'') + case isnull(entità.notestato,'-1')when '-1' then '' " & _
                 " else 'note chiusura: ' end +  isnull(entità.notestato,'') AS AltreInfo,  " & _
                 "isnull(rtrim(ltrim(StatiAttestato.StatoAttestato)),'Indefinito') AS CLP, " & _
                 "Attività.IDAttività AS IdAttività, " & _
                 "Entità.Cognome + ' ' + Entità.Nome AS Nominativo, " & _
                 "Entità.Nome as Nome, Entità.Cognome AS Cognome, " & _
                 "Comuni.Denominazione + ' (' + isnull(Provincie.DescrAbb,Provincie.Provincia) + ') ' AS ComProv, " & _
                 "Comuni.Denominazione AS Comune, " & _
                 "Provincie.DescrAbb AS Provincia, " & _
                 "CASE Entità.Sesso WHEN 0 THEN 'UOMO' WHEN 1 THEN 'DONNA' END AS Sesso, " & _
                 "CASE Entità.Abilitato	WHEN 0 THEN 'NO' WHEN 1 THEN 'SI'  END AS Abilitato, " & _
                 "COUNT(Attività.Titolo) as NumeroAttivita, " & _
                 "CONVERT(varchar, Entità.DataInizioServizio, 103)as DataInizio, " & _
                 "CONVERT(varchar, Entità.DataFineServizio, 103) as DataFine, " & _
                 "StatiEntità.StatoEntità as Stato, " & _
                 "enti.CodiceRegione, " & _
                 "Enti.IdEnte as IdEnte, " & _
                 "Enti.Denominazione + ' (' + isnull(attività.CodiceEnte,'') + ') ' as Ente ,entità.codicevolontario, " & _
                 "Enti.Denominazione as Ente1,Entità.codicefiscale as cf, Entità.datanascita, Attività.titolo as progetto, " & _
                 "Entità.altreinformazioni as note, (Entità.indirizzo + ' ' + Entità.numerocivico) as indirizzo, " & _
                 "(Entità.cap + ' ' + Comuni.Denominazione + ' ('+  Provincie.DescrAbb + ')' ) as localita "

        strSQL = strSQL & ", '<input type=""checkbox"" class=""chkSelVol""  name=""chkSelVol""  title=""Sel./Des."" id=""chkSelVol' + convert(varchar,Entità.IDEntità) + '"" onclick=""javascript:OpenContaVolontari(' + convert(varchar,Entità.IDEntità) + ',this.checked)""/>' as chkSelVol "

        strSQL = strSQL & "FROM AttivitàEntiSediAttuazione " & _
                 "INNER JOIN AttivitàEntità ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione = AttivitàEntità.IDAttivitàEnteSedeAttuazione " & _
                 "RIGHT OUTER JOIN Entità ON Attivitàentità.IDEntità = Entità.IDEntità " & _
                 "INNER JOIN StatiEntità ON Entità.IDStatoEntità = StatiEntità.IDStatoEntità " & _
                 "INNER JOIN GRADUATORIEENTITà on graduatorieentità.identità = entità.identità " & _
                 "INNER JOIN attivitàsediassegnazione on attivitàsediassegnazione.idattivitàsedeassegnazione = graduatorieentità.idattivitàsedeassegnazione " & _
                 "INNER JOIN Entisedi on attivitàsediassegnazione.identesede = entisedi.identesede " & _
                 "INNER JOIN Comuni ON Entisedi.IDComune = Comuni.IDComune " & _
                 "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia " & _
                 "INNER JOIN Regioni ON Provincie.IDRegione = Regioni.IDRegione " & _
                 "inner join attività on attività.idattività = attivitàsediassegnazione.idattività " & _
                 "inner join enti on attività.identepresentante = enti.idente " & _
                 "left outer join StatiAttestato on entità.IdStatoAttestato = StatiAttestato.IdStatoAttestato " & _
                 "INNER JOIN TIPIPROGETTO ON attività.idtipoprogetto=TIPIPROGETTO.idtipoprogetto "
        '"INNER JOIN Attività ON AttivitàEntiSediAttuazione.IDAttività = Attività.IDAttività " & _
        '"INNER JOIN Enti ON Attività.IDEntePresentante = Enti.IDEnte " & _
        strCondizione = "WHERE "
        'se viene selezionato l'ente faccio il filtro su di lui
        If Request.Params("Ente") = "OK" Then
            strSQL = strSQL & strCondizione & "Attività.IDEntePresentante = " & Session("IdEnte") & " "
            strCondizione = "AND "
        End If
        'imposto le condizioni dinamicamente
        If cboSesso.SelectedIndex >= 1 Then
            strSQL = strSQL & strCondizione & "Sesso = '" & cboSesso.Items(cboSesso.SelectedIndex).Value.ToString & "' "
            strCondizione = "AND "
        End If
        If cboStato.SelectedIndex >= 1 Then
            strSQL = strSQL & strCondizione & "Entità.IDStatoEntità = '" & cboStato.Items(cboStato.SelectedIndex).Value.ToString & "' "
            strCondizione = "AND "
        End If


        strSQL = strSQL & strCondizione & " isnull(rtrim(ltrim(StatiAttestato.IdStatoAttestato)),1) = '" & cboStatiAttestato.Items(cboStatiAttestato.SelectedIndex).Value.ToString & "' "
        strCondizione = "AND "


        If txtCognome.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Cognome LIKE '" & Replace(txtCognome.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtNome.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Nome LIKE '" & Replace(txtNome.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtRegione.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Regioni.Regione LIKE '" & Replace(txtRegione.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtProvincia.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtComune.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Comuni.Denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtDescEnte.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Enti.Denominazione LIKE '" & Replace(txtDescEnte.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtCodEnte.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Enti.CodiceRegione = '" & Replace(txtCodEnte.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        If txtProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Attività.Titolo LIKE '" & Replace(txtProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtCodProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Attività.CodiceEnte LIKE '" & Replace(txtCodProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If

        'Controllo Data Inizio Servizio
        If txtDataInizServ.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Entità.DataInizioServizio = '" & txtDataInizServ.Text & "'"
            strCondizione = "AND "
        End If

        If txtalladata.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "'" & txtalladata.Text & "' between Entità.datainizioservizio and Entità.datafineservizio and Entità.datainizioservizio <> Entità.datafineservizio "
            strCondizione = "AND "

        End If

        If txtCodVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & " entità.codicevolontario= '" & Replace(txtCodVolontario.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        'FiltroVisibilita 01/12/20104 da s.c.
        If Session("FiltroVisibilita") <> Nothing Then
            strSQL = strSQL & strCondizione & " TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            strCondizione = "AND "
        End If
        'eseguo la group by per il numero attività
        strSQL = strSQL & " GROUP BY " & _
                          "Entità.IDEntità, " & _
                          "Attività.IDAttività, " & _
                          "isnull(Entità.AltreInformazioni,'') + case isnull(entità.notestato,'-1')when '-1' then '' " & _
                          " else 'note chiusura: ' end +  isnull(entità.notestato,''), " & _
                          "Entità.Cognome + ' ' + Entità.Nome, " & _
                          "Entità.Nome, Entità.Cognome, " & _
                          "Comuni.Denominazione, " & _
                          "Provincie.Provincia,  " & _
                          "CASE Entità.Sesso WHEN 0 THEN 'UOMO' WHEN 1 THEN 'DONNA' END, " & _
                          "CASE Entità.Abilitato	WHEN 0 THEN 'NO' WHEN 1 THEN 'SI'  END, " & _
                          "CONVERT(varchar, Entità.DataInizioServizio, 103), " & _
                          "CONVERT(varchar, Entità.DataFineServizio, 103), " & _
                          "Attività.Titolo, " & _
                          "Enti.CodiceRegione, " & _
                          "StatiEntità.StatoEntità, " & _
                          "Enti.CodiceRegione, " & _
                          "Enti.IdEnte, " & _
                          "Enti.Denominazione, " & _
                          "attività.CodiceEnte, " & _
                          "StatiAttestato.StatoAttestato, " & _
                          "Provincie.DescrAbb, " & _
                          "entità.codicevolontario,Entità.codicefiscale, " & _
                          "Entità.datanascita, Attività.titolo , Entità.altreinformazioni,(Entità.indirizzo + ' ' + Entità.numerocivico),Entità.cap + ' ' + Comuni.Denominazione + ' ('+  Provincie.DescrAbb + ')'  " & _
                          " order by Entità.Cognome + ' ' + Entità.Nome "
        'eseguo la query
        dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dtgRisultatoRicerca.DataSource = dtsRisRicerca

        If dtsRisRicerca.Tables(0).Rows.Count > 0 Then
            dtgRisultatoRicerca.Caption = "Risultato Ricerca Volontari"
            DivSelezionaTutto.Visible = True
            fldSetGestioneVolontariSelezionati.Visible = True
        Else
            dtgRisultatoRicerca.Caption = "La ricerca non ha prodotto risultati."
            DivSelezionaTutto.Visible = False
            If (Session("STRINGACHECKSEL") Is Nothing Or Session("STRINGACHECKSEL") = String.Empty) Then
                fldSetGestioneVolontariSelezionati.Visible = False
            Else
                fldSetGestioneVolontariSelezionati.Visible = True
            End If
        End If

        Session("appDtsRisRicerca") = dtsRisRicerca
        dtgRisultatoRicerca.DataBind()
    End Sub

    Sub CaricaDatiIniziali()
        Dim strSql As String
        Try
            'SESSO
            cboSesso.Items.Add("")
            cboSesso.Items(0).Value = ""
            cboSesso.Items.Add("Uomo")
            cboSesso.Items(1).Value = 0
            cboSesso.Items.Add("Donna")
            cboSesso.Items(2).Value = 1

            Dim MyDataset As DataSet

            cboStato.Items.Clear()
            strSql = "SELECT '0' as IdStatoEntità, '' as Statoentità FROM StatiEntità " & _
                     "UNION SELECT IdStatoEntità, StatoEntità  FROM StatiEntità"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            cboStato.DataSource = MyDataset
            cboStato.DataValueField = "IdStatoEntità"
            cboStato.DataTextField = "StatoEntità"
            cboStato.DataBind()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        Call CaricaGriglia()
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged

        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dtgRisultatoRicerca.DataBind()
        dtgRisultatoRicerca.SelectedIndex = -1
        checkSelDesel.Checked = False
        checkSelDesel.Text = "Seleziona Tutto"
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Session("STRINGACHECKSEL") = Nothing
        'dimensiono a vuoto il vettore con gli id dei volontari
        Session("JonListaIdVolontari") = Nothing
        'carico la home
        Session("appDtsRisRicerca") = Nothing
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dtgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand
        Dim idVolontario As String = String.Empty
        If e.CommandName = "SelezionaVolontario" Then
            idVolontario = e.Item.Cells(4).Text
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                If e.Item.Cells(12).Text = "&nbsp;" Or e.Item.Cells(12).Text = "" Or e.Item.Cells(12).Text = "&nbsp" Then
                    Session("IdEnte") = -1
                Else
                    Session("IdEnte") = e.Item.Cells(12).Text
                End If
                Session("Denominazione") = e.Item.Cells(13).Text
            End If
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                Response.Redirect("WfrmVolontari.aspx?Vengoda=stampe&IdVol=" & idVolontario & "&IdAttivita=" & e.Item.Cells(3).Text)
            Else
                Response.Redirect("WfrmVolontari.aspx?Vengoda=stampe&IdVol=" & idVolontario & "&IdAttivita=" & e.Item.Cells(3).Text & "&Ente=OK")
            End If

        End If
        If e.CommandName = "StoricoVolontario" Then
            idVolontario = e.Item.Cells(4).Text
            Response.Write("<script>" & vbCrLf)
            Response.Write("window.open(""WfrmAttivitaVolontari.aspx?IdVolontario=" & idVolontario & """, ""Visualizza"", ""width=600,height=300,dependent=no,resizable=yes,scrollbars=yes,status=no"")" & vbCrLf)
            Response.Write("</script>")
        End If

    End Sub

    Private Function CreateDataSource() As DataView

        Dim dt As New DataTable("Addresses")
        dt.Columns.Add(New DataColumn("Nome", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Cognome", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Comune", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Provincia", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Nascita", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Progetto", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Ente", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("DIS", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("DFS", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Note", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Codice", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Data", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Indirizzo", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Localita", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("IdEntità", Type.GetType("System.String")))



        Dim values(14) As Object
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim strSQL As String
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim myCommand As SqlClient.SqlCommand

        For i = 0 To UBound(Session("JonListaIdVolontari"))
            ChiudiDataReader(dtrLocal)

            strSQL = "SELECT  DISTINCT "
            strSQL = strSQL & "Entità.Nome as Nome, "
            strSQL = strSQL & "Entità.Cognome AS Cognome, "
            strSQL = strSQL & "cn.Denominazione AS Comune, "
            strSQL = strSQL & "pn.DescrAbb AS Provincia, "
            strSQL = strSQL & "Entità.datanascita, "
            strSQL = strSQL & "Attività.titolo as progetto, "
            strSQL = strSQL & "Enti.Denominazione as Ente1, "
            strSQL = strSQL & "CONVERT(varchar, Entità.DataInizioServizio, 103)as DataInizio, "
            strSQL = strSQL & "CONVERT(varchar, Entità.DataFineServizio, 103) as DataFine, "
            strSQL = strSQL & "Entità.altreinformazioni as note, "
            strSQL = strSQL & "entità.codicevolontario, "
            strSQL = strSQL & "getdate() data, "
            strSQL = strSQL & "(Entità.indirizzo + ' ' + Entità.numerocivico + case Entità.DettaglioRecapitoResidenza when '' then '' else isnull(' - ' + Entità.DettaglioRecapitoResidenza,'') end ) as indirizzo, "
            strSQL = strSQL & "Entità.cap + ' ' + Comuni.Denominazione + case  isnull(provincie.descrabb,'') when '' then '' else ' ('+  Provincie.DescrAbb + ')'  end as localita, Entità.IdEntità "
            strSQL = strSQL & "FROM AttivitàEntiSediAttuazione "
            strSQL = strSQL & "INNER JOIN AttivitàEntità ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione =  AttivitàEntità.IDAttivitàEnteSedeAttuazione "
            strSQL = strSQL & "RIGHT OUTER JOIN Entità ON Attivitàentità.IDEntità = Entità.IDEntità "
            strSQL = strSQL & "INNER JOIN StatiEntità ON Entità.IDStatoEntità = StatiEntità.IDStatoEntità "
            strSQL = strSQL & "INNER JOIN GRADUATORIEENTITà on graduatorieentità.identità = entità.identità "
            strSQL = strSQL & "INNER JOIN attivitàsediassegnazione on attivitàsediassegnazione.idattivitàsedeassegnazione =  graduatorieentità.idattivitàsedeassegnazione "
            strSQL = strSQL & "INNER JOIN Entisedi on attivitàsediassegnazione.identesede = entisedi.identesede "
            strSQL = strSQL & "INNER JOIN Comuni cn ON entità.IDComuneNascita = cn.IDComune  "
            strSQL = strSQL & "INNER JOIN Provincie pn ON cn.IDProvincia = pn.IDProvincia  "
            strSQL = strSQL & "INNER JOIN Comuni ON entità.IDComuneResidenza = Comuni.IDComune "
            strSQL = strSQL & "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia "
            strSQL = strSQL & "INNER JOIN Regioni ON Provincie.IDRegione = Regioni.IDRegione "
            strSQL = strSQL & "inner join attività on attività.idattività = attivitàsediassegnazione.idattività "
            strSQL = strSQL & "inner join enti on attività.identepresentante = enti.idente "
            strSQL = strSQL & "left outer join StatiAttestato on entità.IdStatoAttestato = StatiAttestato.IdStatoAttestato "
            strSQL = strSQL & "WHERE  entità.identità = '" & Session("JonListaIdVolontari")(i) & "' "

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = Session("conn")
            myCommand.CommandText = strSQL

            dtrLocal = myCommand.ExecuteReader

            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                values(0) = dtrLocal("Nome")       'nome
                values(1) = dtrLocal("Cognome")       'cognome
                values(2) = dtrLocal("Comune")     'comune
                values(3) = dtrLocal("Provincia")      'provincia
                values(4) = FormatDateTime(dtrLocal("datanascita"), DateFormat.ShortDate)      'data di nascita
                values(5) = dtrLocal("progetto")       'progetto    
                values(6) = dtrLocal("Ente1")      'ente
                values(7) = dtrLocal("DataInizio")       'dis
                values(8) = dtrLocal("DataFine")       'dfs
                values(9) = dtrLocal("note")       'note
                values(10) = dtrLocal("codicevolontario")       'codice volonatrio
                values(11) = FormatDateTime(Now, DateFormat.ShortDate)             'data
                values(12) = dtrLocal("indirizzo")     'indirizzo
                values(13) = dtrLocal("localita")
                values(14) = dtrLocal("IdEntità")
            End If
            dt.Rows.Add(values)
            ChiudiDataReader(dtrLocal)
        Next
        ChiudiDataReader(dtrLocal)
        Dim dw As New DataView(dt)
        dw.Sort = "IdEntità"
        Return dw

    End Function

    Private Sub imgStampa_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgStampa.Click
        Dim i As Integer
        Dim intCronoStampa As Integer = MaxIdCronologia()

        If Not Session("JonListaIdVolontari") Is Nothing Then
            'inserisco la cronologia stampa
            If CStr(Session("JonListaIdVolontari")(0)) <> "" Then
                For i = 0 To UBound(Session("JonListaIdVolontari"))
                    Call InsertCronologia(intCronoStampa, Session("JonListaIdVolontari")(i))
                    'aggioro lo stato
                    Call AggiornaStatoStampa(Session("JonListaIdVolontari")(i))
                Next
            End If

            'Stampa attestato
            If optAttestato.Checked = True Then
                Dim intIDRptAttestato As Integer
                Dim intIDRptAttestatoDuplicato As Integer
                If Session("Sistema") = "Futuro" Then
                    intIDRptAttestato = 47
                    intIDRptAttestatoDuplicato = 48
                Else
                    intIDRptAttestato = 25
                    intIDRptAttestatoDuplicato = 27
                End If

                'strEsito = ClsServer.CreatePdf("crpVolontariAttestato.rpt", sDati, Me.Session)
                Response.Write("<script>")
                'controllo se è selezionata la voce Originale o Duplicato
                'per aprire il diverso template
                If ddlStampa.SelectedValue = 0 Then
                    Response.Write("myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=" & intIDRptAttestato & "&IdVolontario=" & intCronoStampa & "','Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')")
                Else
                    Response.Write("myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=" & intIDRptAttestatoDuplicato & "&IdVolontario=" & intCronoStampa & "','Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')")
                End If
                Response.Write("</script>")
            End If

            'Stampa lettera attestato
            If optLettera.Checked = True Then
                Response.Write("<script>")
                If ddlStampa.SelectedValue = 0 Then
                    Response.Write("myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=26&IdVolontario=" & intCronoStampa & "','Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')")
                Else
                    Response.Write("myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=28&IdVolontario=" & intCronoStampa & "','Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')")
                End If
                Response.Write("</script>")
            End If
        End If
    End Sub

    Private Sub AggiornaStatoStampa(ByVal idVolontario As Long)
        Dim strSQL As String

        'aggiorno la tabella entià (In Stampa)
        strSQL = "Update Entità Set IdStatoAttestato=3, DataAttestato=GetDate(),UsernameAttestato='" & Session("utente") & "' Where identità=" & idVolontario
        ClsServer.EseguiSqlClient(strSQL, Session("conn"))
        'inserisco la cronologia
        strSQL = "Insert Into CronologiaStatiAttestato (IDEntità,IDStato,DataStato,UserNameStato) " & _
                " VALUES (" & idVolontario & ", 3, GetDate(),'" & Session("utente") & "')"
        ClsServer.EseguiSqlClient(strSQL, Session("conn"))

    End Sub

    Private Sub InsertCronologia(ByVal idStampa As Integer, ByVal idVolontario As Integer)
        Dim strSQL As String
        strSQL = "Insert into CronologiaStampeAttestati (IdStampa,IdVolontario,DataStampa,UsernameStampa) VALUES " _
                & "(" & idStampa & "," & idVolontario & ",GetDate(),'" & Session("Utente") & "')"

        Dim myCommand As New SqlClient.SqlCommand
        myCommand = New SqlClient.SqlCommand(strSQL, Session("conn"))
        myCommand.ExecuteNonQuery()

        myCommand.Dispose()

    End Sub

    Private Function MaxIdCronologia() As Integer
        Dim strSQL As String
        Dim myDS As New DataSet
        Dim idCrono As Integer

        strSQL = "Select isnull(max(IdStampa),0) as MaxID From CronologiaStampeAttestati "
        myDS = ClsServer.DataSetGenerico(strSQL, Session("conn"))

        If myDS.Tables(0).Rows(0).Item("MaxID") = 0 Then
            idCrono = 1
        Else
            idCrono = myDS.Tables(0).Rows(0).Item("MaxID") + 1
        End If

        myDS.Dispose()

        Return idCrono
    End Function

    Private Sub imgAnnullaSelezione_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgAnnullaSelezione.Click
        checkSelDesel.Checked = False
        checkSelDesel.Text = "Seleziona Tutto"
        Session("STRINGACHECKSEL") = ""
        Session("JonListaIdVolontari") = Nothing
        If (dtgRisultatoRicerca.Items.Count = 0) Then
            fldSetGestioneVolontariSelezionati.Visible = False
        End If

    End Sub

    Private Sub checkSelDesel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkSelDesel.CheckedChanged
        Dim i As Integer
        Dim Mychk As CheckBox

        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dtgRisultatoRicerca.Items.Count - 1
            Mychk = dtgRisultatoRicerca.Items.Item(i).FindControl("chkSelVol")
            Dim dichiarazioneCheck As String = dtgRisultatoRicerca.Items.Item(i).Cells(INDEX_DATAGRID_DTGRISULTATORICERCA_CHECKBOX_SELEZIONA).Text
            Dim checked As String = " checked=""checked"" "
            If (checkSelDesel.Checked) Then
                dichiarazioneCheck = dichiarazioneCheck.Insert(7, checked)
            Else
                dichiarazioneCheck = dichiarazioneCheck.Replace(checked, String.Empty)
            End If
            dtgRisultatoRicerca.Items.Item(i).Cells(INDEX_DATAGRID_DTGRISULTATORICERCA_CHECKBOX_SELEZIONA).Text = dichiarazioneCheck
            ContaVol(checkSelDesel.Checked, dtgRisultatoRicerca.Items.Item(i).Cells(INDEX_DATAGRID_DTGRISULTATORICERCA_IDVOLONTARIO).Text)
        Next i
        If (checkSelDesel.Checked) Then
            checkSelDesel.Text = "Deseleziona Tutto"

        Else
            checkSelDesel.Text = "Seleziona Tutto"
        End If
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""contavolontari.aspx?blocco=" & "0" & """, ""Visualizza"", ""width=250,height=250,dependent=no,resizable=no,scrollbars=no,status=no"")" & vbCrLf)
        Response.Write("</script>")

    End Sub



    Private Sub CmdEsporta_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles CmdEsporta.Click
        'Dim dt As New DataTable
        Dim dt As New DataView

        'se non ho selezionato volontari non faccio nulla
        If Session("STRINGACHECKSEL") <> "" Then
            dt = CreateDataSource()

            Session("DataTableElencoVolontariStampa") = dt
            EsportaHtml()
        End If

    End Sub
    Private Sub EsportaHtml()
        Dim grid As New DataGrid
        grid.ShowHeader = True
        grid.HeaderStyle.Font.Bold = True

        grid.DataSource = Session("DataTableElencoVolontariStampa")
        grid.AutoGenerateColumns = False

        Dim bc1 As New BoundColumn
        bc1.DataField = "Nome"
        bc1.HeaderText = "Nome"
        grid.Columns.Add(bc1)

        Dim bc2 As New BoundColumn
        bc2.DataField = "Cognome"
        bc2.HeaderText = "Cognome"
        grid.Columns.Add(bc2)

        Dim bc3 As New BoundColumn
        bc3.DataField = "Comune"
        bc3.HeaderText = "Comune"
        grid.Columns.Add(bc3)

        Dim bc4 As New BoundColumn
        bc4.DataField = "Provincia"
        bc4.HeaderText = "Provincia"
        grid.Columns.Add(bc4)

        Dim bc5 As New BoundColumn
        bc5.DataField = "Nascita"
        bc5.HeaderText = "Nascita"
        grid.Columns.Add(bc5)

        Dim bc6 As New BoundColumn
        bc6.DataField = "Progetto"
        bc6.HeaderText = "Progetto"
        grid.Columns.Add(bc6)

        Dim bc7 As New BoundColumn
        bc7.DataField = "Ente"
        bc7.HeaderText = "Ente"
        grid.Columns.Add(bc7)

        Dim bc8 As New BoundColumn
        bc8.DataField = "DIS"
        bc8.HeaderText = "DIS"
        grid.Columns.Add(bc8)

        Dim bc10 As New BoundColumn
        bc10.DataField = "DFS"
        bc10.HeaderText = "DFS"
        grid.Columns.Add(bc10)

        Dim bc11 As New BoundColumn
        bc11.DataField = "Note"
        bc11.HeaderText = "Note"
        grid.Columns.Add(bc11)

        Dim bc12 As New BoundColumn
        bc12.DataField = "Codice"
        bc12.HeaderText = "Codice"
        grid.Columns.Add(bc12)

        Dim bc13 As New BoundColumn
        bc13.DataField = "Data"
        bc13.HeaderText = "Data"
        grid.Columns.Add(bc13)

        Dim bc14 As New BoundColumn
        bc14.DataField = "Indirizzo"
        bc14.HeaderText = "Indirizzo"
        grid.Columns.Add(bc14)

        Dim bc15 As New BoundColumn
        bc15.DataField = "Localita"
        bc15.HeaderText = "Localita"
        grid.Columns.Add(bc15)

        grid.DataBind()

        ' 2) imposto il tipo di ContentType e la codifica per il Response
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.ContentEncoding = System.Text.Encoding.Default
        Me.EnableViewState = False

        ' 3) creo l'HtmlTextWriter su cui renderizzo la griglia
        Dim tw As New System.IO.StringWriter
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        grid.RenderControl(hw)

        ' 4) ottiene la stringa HTML dallo stream, e lo manda al browser, per essere intrpretato da Excel
        Dim str_tw As String = tw.ToString()
        Response.Write(str_tw)
        Response.Flush()
        Response.Close()
    End Sub
    Private Sub dtgRisultatoRicerca_ItemDataBound(ByVal source As Object, ByVal e As DataGridItemEventArgs) Handles dtgRisultatoRicerca.ItemDataBound
        Dim idVolontario As String = e.Item.Cells(INDEX_DATAGRID_DTGRISULTATORICERCA_IDVOLONTARIO).Text
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            If Not Session("JonListaIdVolontari") Is Nothing Then
                For i = 0 To UBound(Session("JonListaIdVolontari"))
                    If (Session("JonListaIdVolontari")(i) = idVolontario) Then
                        '(Divina) si, mi vergogno tantissimo per questa porcheria
                        Dim dichiarazioneCheck As String = e.Item.Cells(INDEX_DATAGRID_DTGRISULTATORICERCA_CHECKBOX_SELEZIONA).Text
                        Dim checked As String = " checked=""checked"" "
                        e.Item.Cells(1).Text = dichiarazioneCheck.Insert(7, checked)
                        Exit For
                    End If
                Next
            End If
        End If

    End Sub

    Function ContaVol(ByVal blnStato As Boolean, ByVal IdVol As Integer) As Integer
        Dim intX As Integer
        Dim strUltimoValore As String
        'controllo stato check
  
        'inserimento
        If blnStato = True Then
            If Session("JonListaIdVolontari") Is Nothing Then
                ReDim Session("JonListaIdVolontari")(0)
                Session("JonListaIdVolontari")(0) = String.Empty
            End If
            'primo
            If Session("JonListaIdVolontari")(0).ToString = String.Empty Then
                Session("JonListaIdVolontari")(0) = IdVol
            Else 'ridiminesiono il vettore
                ReDim Preserve Session("JonListaIdVolontari")(UBound(Session("JonListaIdVolontari")) + 1)
                Session("JonListaIdVolontari")(UBound(Session("JonListaIdVolontari"))) = IdVol
            End If
        Else 'cancellazione
            strUltimoValore = Session("JonListaIdVolontari")(UBound(Session("JonListaIdVolontari")))
            For intX = 0 To UBound(Session("JonListaIdVolontari"))
                If IdVol.ToString = Session("JonListaIdVolontari")(intX) Then
                    Session("JonListaIdVolontari")(intX) = strUltimoValore
                    ReDim Preserve Session("JonListaIdVolontari")(UBound(Session("JonListaIdVolontari")) - 1)
                    If UBound(Session("JonListaIdVolontari")) = -1 Then
                        Session("JonListaIdVolontari") = Nothing
                    End If
                    Exit For
                End If
            Next
        End If
        If Session("JonListaIdVolontari") Is Nothing Then
            ContaVol = 0
            Session("STRINGACHECKSEL") = ""
        Else
            Session("STRINGACHECKSEL") = ""
            For intX = 0 To UBound(Session("JonListaIdVolontari"))
                Session("STRINGACHECKSEL") = Session("STRINGACHECKSEL") & Session("JonListaIdVolontari")(intX) & "&"
            Next
            ContaVol = UBound(Session("JonListaIdVolontari")) + 1
        End If

        Return ContaVol

    End Function
#Region "Versione esportazione classica"
    'Private Function CreateDataSource() As DataTable
    '    Dim values(14) As Object
    '    Dim i As Integer
    '    Dim strSQL As String
    '    Dim dtrLocal As SqlClient.SqlDataReader
    '    Dim myCommand As SqlClient.SqlCommand
    '    Dim dt As New DataTable("Addresses")

    '    dt.Columns.Add(New DataColumn("Nome", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Cognome", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Comune", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Provincia", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Nascita", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Progetto", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Ente", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("DIS", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("DFS", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Note", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Codice", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Data", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Indirizzo", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Localita", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("IdEntità", Type.GetType("System.String")))


    '    For i = 0 To UBound(Session("JonListaIdVolontari"))
    '        'chiudo il datareader
    '        If Not dtrLocal Is Nothing Then
    '            dtrLocal.Close()
    '            dtrLocal = Nothing
    '        End If

    '        strSQL = "SELECT  DISTINCT "
    '        strSQL = strSQL & "Entità.Nome as Nome, "
    '        strSQL = strSQL & "Entità.Cognome AS Cognome, "
    '        strSQL = strSQL & "cn.Denominazione AS Comune, "
    '        strSQL = strSQL & "pn.DescrAbb AS Provincia, "
    '        strSQL = strSQL & "Entità.datanascita, "
    '        strSQL = strSQL & "Attività.titolo as progetto, "
    '        strSQL = strSQL & "Enti.Denominazione as Ente1, "
    '        strSQL = strSQL & "CONVERT(varchar, Entità.DataInizioServizio, 103)as DataInizio, "
    '        strSQL = strSQL & "CONVERT(varchar, Entità.DataFineServizio, 103) as DataFine, "
    '        strSQL = strSQL & "Entità.altreinformazioni as note, "
    '        strSQL = strSQL & "entità.codicevolontario, "
    '        strSQL = strSQL & "getdate() data, "
    '        strSQL = strSQL & "(Entità.indirizzo + ' ' + Entità.numerocivico + case Entità.DettaglioRecapitoResidenza when '' then '' else isnull(' - ' + Entità.DettaglioRecapitoResidenza,'') end ) as indirizzo, "
    '        strSQL = strSQL & "Entità.cap + ' ' + Comuni.Denominazione + case  isnull(provincie.descrabb,'') when '' then '' else ' ('+  Provincie.DescrAbb + ')'  end as localita, Entità.IdEntità "
    '        strSQL = strSQL & "FROM AttivitàEntiSediAttuazione "
    '        strSQL = strSQL & "INNER JOIN AttivitàEntità ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione =  AttivitàEntità.IDAttivitàEnteSedeAttuazione "
    '        strSQL = strSQL & "RIGHT OUTER JOIN Entità ON Attivitàentità.IDEntità = Entità.IDEntità "
    '        strSQL = strSQL & "INNER JOIN StatiEntità ON Entità.IDStatoEntità = StatiEntità.IDStatoEntità "
    '        strSQL = strSQL & "INNER JOIN GRADUATORIEENTITà on graduatorieentità.identità = entità.identità "
    '        strSQL = strSQL & "INNER JOIN attivitàsediassegnazione on attivitàsediassegnazione.idattivitàsedeassegnazione =  graduatorieentità.idattivitàsedeassegnazione "
    '        strSQL = strSQL & "INNER JOIN Entisedi on attivitàsediassegnazione.identesede = entisedi.identesede "
    '        strSQL = strSQL & "INNER JOIN Comuni cn ON entità.IDComuneNascita = cn.IDComune  "
    '        strSQL = strSQL & "INNER JOIN Provincie pn ON cn.IDProvincia = pn.IDProvincia  "
    '        strSQL = strSQL & "INNER JOIN Comuni ON entità.IDComuneResidenza = Comuni.IDComune "
    '        strSQL = strSQL & "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia "
    '        strSQL = strSQL & "INNER JOIN Regioni ON Provincie.IDRegione = Regioni.IDRegione "
    '        strSQL = strSQL & "inner join attività on attività.idattività = attivitàsediassegnazione.idattività "
    '        strSQL = strSQL & "inner join enti on attività.identepresentante = enti.idente "
    '        strSQL = strSQL & "left outer join StatiAttestato on entità.IdStatoAttestato = StatiAttestato.IdStatoAttestato "
    '        strSQL = strSQL & "WHERE  entità.identità = '" & Session("JonListaIdVolontari")(i) & "' "

    '        myCommand = New SqlClient.SqlCommand
    '        myCommand.Connection = Session("conn")
    '        myCommand.CommandText = strSQL

    '        dtrLocal = myCommand.ExecuteReader

    '        If dtrLocal.HasRows = True Then
    '            dtrLocal.Read()
    '            values(0) = dtrLocal("Nome")       'nome
    '            values(1) = dtrLocal("Cognome")       'cognome
    '            values(2) = dtrLocal("Comune")     'comune
    '            values(3) = dtrLocal("Provincia")      'provincia
    '            values(4) = FormatDateTime(dtrLocal("datanascita"), DateFormat.ShortDate)      'data di nascita
    '            values(5) = dtrLocal("progetto")       'progetto    
    '            values(6) = dtrLocal("Ente1")      'ente
    '            values(7) = dtrLocal("DataInizio")       'dis
    '            values(8) = dtrLocal("DataFine")       'dfs
    '            values(9) = dtrLocal("note")       'note
    '            values(10) = dtrLocal("codicevolontario")       'codice volonatrio
    '            values(11) = FormatDateTime(Now, DateFormat.ShortDate)             'data
    '            values(12) = dtrLocal("indirizzo")     'indirizzo
    '            values(13) = dtrLocal("localita")
    '            values(14) = dtrLocal("IdEntità")
    '        End If

    '        ChiudiDataReader(dtrLocal)

    '        dt.Rows.Add(values)

    '    Next
    '    ChiudiDataReader(dtrLocal)

    '    Dim dw As New DataView(dt)

    '    dw.Sort = "IdEntità"



    '    Return dt

    'End Function
    'Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
    '    If Session("STRINGACHECKSEL") <> "" Then
    '        CmdEsporta.Visible = False
    '        Dim dtbRicerca As DataTable = CreateDataSource()
    '        Session("dtbRicerca") = dtbRicerca
    '        CaricaDataGrid()
    '        StampaCSV(dtbRicerca)
    '    End If
    'End Sub
    'Private Function CreateDataSource() As DataTable
    '    Dim values(14) As Object
    '    Dim i As Integer
    '    Dim strSQL As String
    '    Dim dtrLocal As SqlClient.SqlDataReader
    '    Dim myCommand As SqlClient.SqlCommand
    '    Dim dt As New DataTable("Addresses")

    '    dt.Columns.Add(New DataColumn("Nome", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Cognome", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Comune", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Provincia", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Nascita", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Progetto", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Ente", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("DIS", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("DFS", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Note", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Codice", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Data", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Indirizzo", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("Localita", Type.GetType("System.String")))
    '    dt.Columns.Add(New DataColumn("IdEntità", Type.GetType("System.String")))


    '    For i = 0 To UBound(Session("JonListaIdVolontari"))
    '        'chiudo il datareader
    '        If Not dtrLocal Is Nothing Then
    '            dtrLocal.Close()
    '            dtrLocal = Nothing
    '        End If

    '        strSQL = "SELECT  DISTINCT "
    '        strSQL = strSQL & "Entità.Nome as Nome, "
    '        strSQL = strSQL & "Entità.Cognome AS Cognome, "
    '        strSQL = strSQL & "cn.Denominazione AS Comune, "
    '        strSQL = strSQL & "pn.DescrAbb AS Provincia, "
    '        strSQL = strSQL & "Entità.datanascita, "
    '        strSQL = strSQL & "Attività.titolo as progetto, "
    '        strSQL = strSQL & "Enti.Denominazione as Ente1, "
    '        strSQL = strSQL & "CONVERT(varchar, Entità.DataInizioServizio, 103)as DataInizio, "
    '        strSQL = strSQL & "CONVERT(varchar, Entità.DataFineServizio, 103) as DataFine, "
    '        strSQL = strSQL & "Entità.altreinformazioni as note, "
    '        strSQL = strSQL & "entità.codicevolontario, "
    '        strSQL = strSQL & "getdate() data, "
    '        strSQL = strSQL & "(Entità.indirizzo + ' ' + Entità.numerocivico + case Entità.DettaglioRecapitoResidenza when '' then '' else isnull(' - ' + Entità.DettaglioRecapitoResidenza,'') end ) as indirizzo, "
    '        strSQL = strSQL & "Entità.cap + ' ' + Comuni.Denominazione + case  isnull(provincie.descrabb,'') when '' then '' else ' ('+  Provincie.DescrAbb + ')'  end as localita, Entità.IdEntità "
    '        strSQL = strSQL & "FROM AttivitàEntiSediAttuazione "
    '        strSQL = strSQL & "INNER JOIN AttivitàEntità ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione =  AttivitàEntità.IDAttivitàEnteSedeAttuazione "
    '        strSQL = strSQL & "RIGHT OUTER JOIN Entità ON Attivitàentità.IDEntità = Entità.IDEntità "
    '        strSQL = strSQL & "INNER JOIN StatiEntità ON Entità.IDStatoEntità = StatiEntità.IDStatoEntità "
    '        strSQL = strSQL & "INNER JOIN GRADUATORIEENTITà on graduatorieentità.identità = entità.identità "
    '        strSQL = strSQL & "INNER JOIN attivitàsediassegnazione on attivitàsediassegnazione.idattivitàsedeassegnazione =  graduatorieentità.idattivitàsedeassegnazione "
    '        strSQL = strSQL & "INNER JOIN Entisedi on attivitàsediassegnazione.identesede = entisedi.identesede "
    '        strSQL = strSQL & "INNER JOIN Comuni cn ON entità.IDComuneNascita = cn.IDComune  "
    '        strSQL = strSQL & "INNER JOIN Provincie pn ON cn.IDProvincia = pn.IDProvincia  "
    '        strSQL = strSQL & "INNER JOIN Comuni ON entità.IDComuneResidenza = Comuni.IDComune "
    '        strSQL = strSQL & "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia "
    '        strSQL = strSQL & "INNER JOIN Regioni ON Provincie.IDRegione = Regioni.IDRegione "
    '        strSQL = strSQL & "inner join attività on attività.idattività = attivitàsediassegnazione.idattività "
    '        strSQL = strSQL & "inner join enti on attività.identepresentante = enti.idente "
    '        strSQL = strSQL & "left outer join StatiAttestato on entità.IdStatoAttestato = StatiAttestato.IdStatoAttestato "
    '        strSQL = strSQL & "WHERE  entità.identità = '" & Session("JonListaIdVolontari")(i) & "' "

    '        myCommand = New SqlClient.SqlCommand
    '        myCommand.Connection = Session("conn")
    '        myCommand.CommandText = strSQL

    '        dtrLocal = myCommand.ExecuteReader

    '        If dtrLocal.HasRows = True Then
    '            dtrLocal.Read()
    '            values(0) = dtrLocal("Nome")       'nome
    '            values(1) = dtrLocal("Cognome")       'cognome
    '            values(2) = dtrLocal("Comune")     'comune
    '            values(3) = dtrLocal("Provincia")      'provincia
    '            values(4) = FormatDateTime(dtrLocal("datanascita"), DateFormat.ShortDate)      'data di nascita
    '            values(5) = dtrLocal("progetto")       'progetto    
    '            values(6) = dtrLocal("Ente1")      'ente
    '            values(7) = dtrLocal("DataInizio")       'dis
    '            values(8) = dtrLocal("DataFine")       'dfs
    '            values(9) = dtrLocal("note")       'note
    '            values(10) = dtrLocal("codicevolontario")       'codice volonatrio
    '            values(11) = FormatDateTime(Now, DateFormat.ShortDate)             'data
    '            values(12) = dtrLocal("indirizzo")     'indirizzo
    '            values(13) = dtrLocal("localita")
    '            values(14) = dtrLocal("IdEntità")
    '        End If

    '        ChiudiDataReader(dtrLocal)

    '        dt.Rows.Add(values)

    '    Next
    '    ChiudiDataReader(dtrLocal)

    '    Dim dw As New DataView(dt)

    '    dw.Sort = "IdEntità"



    '    Return dt

    'End Function
    'Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
    '    Dim path As String
    '    Dim xPrefissoNome As String
    '    Dim url As String
    '    Dim utility As ClsUtility = New ClsUtility()

    '    If dtbRicerca.Rows.Count = 0 Then
    '        ApriCSV1.Visible = False
    '        CmdEsporta.Visible = False
    '    Else
    '        xPrefissoNome = Session("Utente")
    '        path = Server.MapPath("download")
    '        url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
    '        ApriCSV1.Visible = True
    '        ApriCSV1.NavigateUrl = url
    '    End If
    'End Sub
    'Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

    '    Dim writer As StreamWriter
    '    Dim xLinea As String = String.Empty
    '    Dim i As Int64
    '    Dim j As Int64
    '    Dim nomeUnivoco As String
    '    Dim url As String
    '    nomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
    '    writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
    '    'Creazione dell'inntestazione del CSV
    '    Dim intNumCol As Int64 = DTBRicerca.Columns.Count
    '    For i = 0 To intNumCol - 1
    '        xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
    '    Next
    '    writer.WriteLine(xLinea)
    '    xLinea = vbNullString

    '    'Scorro tutte le righe del datatable e riempio il CSV
    '    For i = 0 To DTBRicerca.Rows.Count - 1

    '        For j = 0 To intNumCol - 1
    '            If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
    '                xLinea &= vbNullString & ";"
    '            Else
    '                xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
    '            End If
    '        Next

    '        writer.WriteLine(xLinea)
    '        xLinea = vbNullString

    '    Next
    '    url = "download\" & nomeUnivoco & ".CSV"

    '    writer.Close()
    '    writer = Nothing
    '    Return url
    'End Function
    'Private Sub CaricaDataGrid()
    '    ' Creo manualmente l'istanza del controllo DataGrid
    '    Dim grid As New DataGrid
    '    grid.ShowHeader = True

    '    grid.HeaderStyle.Font.Bold = True

    '    '' 1) imposto la sorgente dei dati per la griglia
    '    ''grid.DataSource = Session("appDtsRisRicerca").Tables(0)
    '    'grid.DataSource = dtsGenerico
    '    'grid.DataBind()
    '    Dim dataTable As DataTable = Session("dtbRicerca")

    '    Dim NomeColonne(14) As String
    '    Dim NomiCampiColonne(14) As String

    '    NomeColonne(0) = "Nome"
    '    NomeColonne(1) = "Cognome"
    '    NomeColonne(2) = "Comune"
    '    NomeColonne(3) = "Provincia"
    '    NomeColonne(4) = "Nascita"
    '    NomeColonne(5) = "Progetto"
    '    NomeColonne(6) = "Ente"
    '    NomeColonne(7) = "DIS"
    '    NomeColonne(8) = "DFS"
    '    NomeColonne(9) = "Note"
    '    NomeColonne(10) = "Codice"
    '    NomeColonne(11) = "Data"
    '    NomeColonne(12) = "Indirizzo"
    '    NomeColonne(13) = "Localita"


    '    NomiCampiColonne(0) = "Nome"
    '    NomiCampiColonne(1) = "Cognome"
    '    NomiCampiColonne(2) = "Comune"
    '    NomiCampiColonne(3) = "Provincia"
    '    NomiCampiColonne(4) = "Nascita"
    '    NomiCampiColonne(5) = "Progetto"
    '    NomiCampiColonne(6) = "Ente"
    '    NomiCampiColonne(7) = "DIS"
    '    NomiCampiColonne(8) = "DFS"
    '    NomiCampiColonne(9) = "Note"
    '    NomiCampiColonne(10) = "Codice"
    '    NomiCampiColonne(11) = "Data"
    '    NomiCampiColonne(12) = "Indirizzo"
    '    NomiCampiColonne(13) = "Localita"

    '    CaricaDataTablePerStampa(dataTable, 13, NomeColonne, NomiCampiColonne)

    'End Sub
    'Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataTable, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
    'Dim dt As New DataTable
    'Dim dr As DataRow
    'Dim i As Integer
    'Dim x As Integer

    ''carico i nomi delle colonne che andrò a stampare nella datagrid
    '    For x = 0 To NColonne
    '        dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
    '    Next

    ''carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
    '    If DataSetDaScorrere.Rows.Count > 0 Then
    '        For i = 1 To DataSetDaScorrere.Rows.Count
    '            dr = dt.NewRow()
    '            For x = 0 To NColonne
    '                dr(x) = DataSetDaScorrere.Rows.Item(i - 1).Item(NomiCampiColonne(x))
    '            Next
    '            dt.Rows.Add(dr)
    '        Next
    '    End If

    ''passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
    '    Session("DtbRicerca") = dt

    'End Sub
#End Region
End Class