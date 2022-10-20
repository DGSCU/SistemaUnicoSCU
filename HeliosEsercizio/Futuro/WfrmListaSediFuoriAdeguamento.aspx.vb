Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Security.Cryptography
'Imports System.Text.RegularExpressions.Regex
Imports System.IO
Imports System.Text.RegularExpressions
Imports Logger.Data
Public Class WfrmListaSediFuoriAdeguamento
    Inherits SmartPage
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim strsql As String
    Dim blnRicerca As Boolean
    Public blnForza As Boolean
    Public ShowPopUPControllo As String
    Const INDEX_DGRISULTATORICERCA_IDIMGSELSEDI As Byte = 0
    Const INDEX_DGRISULTATORICERCA_DATAVARIAZIONE As Byte = 1
    Const INDEX_DGRISULTATORICERCA_CODICEENTE As Byte = 2
    Const INDEX_DGRISULTATORICERCA_ENTE As Byte = 3
    Const INDEX_DGRISULTATORICERCA_SEDE As Byte = 4
    Const INDEX_DGRISULTATORICERCA_VARIAZIONI As Byte = 5
    Const INDEX_DGRISULTATORICERCA_VISTO As Byte = 6
    Const INDEX_DGRISULTATORICERCA_FLGVALIDAZIONE As Byte = 7
    Const INDEX_DGRISULTATORICERCA_IDENTESEDE As Byte = 8
    Const INDEX_DGRISULTATORICERCA_IDENTE As Byte = 9
    Const INDEX_DGRISULTATORICERCA_IDVARIAZIONE As Byte = 10
    Const INDEX_DGRISULTATORICERCA_STATOENTESEDE As Byte = 11



#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        Dim dtrGenericoLocal As SqlClient.SqlDataReader
        ShowPopUPControllo = ""
        Session("Seleziona") = 0
        Session("ValidaModifiche") = 0

        If IsPostBack = False Then
            Session("BloccaChK") = 0
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                divCertificazione.Visible = True
            Else
                divCertificazione.Visible = False
            End If
            Try
                'Ilaria Lombardi 10/11/09 gli passo anhce il codice regione quando vengo dalla maschera degli enti in accordo
                If Request.QueryString("codiceente") <> "" Then
                    TxtCodiceRegione.Text = Request.QueryString("codiceente")
                End If

                ddlTipologia.Items.Add("")

                ChiudiDataReader(dtrGenericoLocal)
                ChiudiDataReader(dtrGenerico)
                lblpage.Value = IIf(Not IsNothing(Context.Items("page")), Context.Items("page"), "")
                txtstrsql.Value = IIf(Not IsNothing(Context.Items("strsql")), Context.Items("strsql"), "")

                dtrGenericoLocal = ClsServer.CreaDatareader("Select tiposede,idtiposede from tipiSedi order by idtiposede desc ", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                Do While dtrGenericoLocal.Read()
                    ddlTipologia.Items.Add(dtrGenericoLocal("tiposede"))
                Loop

                ddlTipologia.SelectedIndex = 1
                ddlTipologia.Enabled = False
                ChiudiDataReader(dtrGenericoLocal)
                ddlstato.Items.Add("")
                dtrGenericoLocal = ClsServer.CreaDatareader("select replace(statoEnteSede, 'Accreditata', 'Iscritta') statoEnteSede from StatiEntiSedi", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                Do While dtrGenericoLocal.Read()
                    ddlstato.Items.Add(dtrGenericoLocal("statoEnteSede"))
                Loop
                ChiudiDataReader(dtrGenericoLocal)


                If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
                    txtDenominazioneEnte.Text = Request.QueryString("DenominazioneEnte")
                    lblpage.Value = String.Empty
                    lblErrore.Text = String.Empty
                    RicercaSedi()
                End If
                If Request.QueryString("CheckProvenienza") = "RicercaEnteInAccordo" Then
                    txtDenominazioneEnte.Enabled = False
                    TxtCodiceRegione.Enabled = False
                End If
            Catch ex As Exception
                ex.Message.ToString()
            End Try
        End If
        blnForza = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        If (blnForza) Then
            divForzaSanzione.Visible = True
        Else
            divForzaSanzione.Visible = False
        End If
    End Sub

#Region "Eventi"
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click

        If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
            Response.Redirect("assegnazionevincoliprogetti.aspx?idattivita=" & Request.QueryString("idattivita") & "&tipologia=" & Request.QueryString("tipologia") & "&Nazionale=" & Request.QueryString("Nazionale"))
        Else
            If Not Request.QueryString("VengoDa") Is Nothing Then
                'controllo se la variabile è valorizzata
                If Request.QueryString("VengoDa") <> String.Empty Then
                    'faccio la response.redirect verso l'albero
                    Response.Redirect(Request.QueryString("VengoDa").ToString)
                End If
            End If
            If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
                If Request.QueryString("CheckProvenienza") = "VisualizzazioneEntiInAccordo" Then
                    Response.Redirect("WfrmElencoEntiAccordo.aspx?Pagina=" & Request.QueryString("Pagina") & "&CheckProvenienza=" & Request.QueryString("CheckProvenienza") & "&Denominazione=" & Request.QueryString("Denominazione") & "&idente=" & Request.QueryString("idente") & "&DenominazioneEnte=" & Request.QueryString("DenomonazioneEnte"))
                Else
                    Response.Redirect("WfrmRicEnteinAccordo.aspx?Pagina=" & Request.QueryString("Pagina") & "&CheckProvenienza=" & Request.QueryString("CheckProvenienza") & "&Stato=" & Request.QueryString("Stato") & "&CF=" & Request.QueryString("CF") & "&ClasseAccreditamento=" & Request.QueryString("ClasseAccreditamento") & "&Tipologia=" & Request.QueryString("Tipologia") & "&CodiceRegione=" & Request.QueryString("CodicerRegione") & "&Denominazione=" & Request.QueryString("Denominazione") & "&VediEnte=1&DenominazioneEnte=" & Request.QueryString("DenomonazioneEnte"))
                End If
            Else
                Response.Redirect("WfrmMain.aspx")
            End If
        End If
    End Sub
    Public Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        lblpage.Value = String.Empty
        lblErrore.Text = String.Empty
        chkSelDesel.Checked = False
        RicercaSedi()

    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Dim identesede As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_IDENTESEDE).Text
                Dim idente As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_IDENTE).Text
                Dim acquisita As String = String.Empty
                Dim stato As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_STATOENTESEDE).Text

                Context.Items.Add("Ente", e.Item.Cells(INDEX_DGRISULTATORICERCA_ENTE).Text)
                Context.Items.Add("tipoazione", "Modifica")
                Context.Items.Add("stato", e.Item.Cells(INDEX_DGRISULTATORICERCA_STATOENTESEDE).Text)
                Context.Items.Add("page", dgRisultatoRicerca.CurrentPageIndex)
                Response.Redirect("WfrmAnagraficaSedi.aspx?identesede=" & identesede & "&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&idente=" & idente & "&acquisita=" & acquisita & "&stato=" & stato)
        End Select
    End Sub


    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn
        Dim ValoreRichkDt As Integer = 9 ' valore che ricorda il campo riccheck della query (si per la ricerca sedi di progetto che per le ricerca sedi)
        dtGriglia = Session("SessionDtSedi")
        'If Request.QueryString("PopSede") = 1 Then
        '    ValoreRichkDt =  ' id ricerca sede di progetto (riccheck)
        'Else
        '    ValoreRichkDt = 12 ' id ricerca sede campo (riccheck)
        'End If
        clGriglia = dtGriglia.Columns(ValoreRichkDt)
        clGriglia.ReadOnly = False

        'If chkSelDesel.Checked = True Then
        '    CeccaTutti()
        'End If
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            'If Mychk.Checked = True Then
            '    rwGriglia.Item(ValoreRichkDt) = "1"
            'Else
            '    rwGriglia.Item(ValoreRichkDt) = "0"
            'End If
            rwGriglia.Item(ValoreRichkDt) = Mychk.Checked
        Next i
        Session("SessionDtSedi") = dtGriglia
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("SessionDtSedi")
        dgRisultatoRicerca.DataBind()


        '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If rwGriglia.Item(ValoreRichkDt) = "True" Then
                Mychk.Checked = True
            Else
                Mychk.Checked = False
            End If
        Next i
        If chkSelDesel.Checked = True Then
            CeccaTutti()
            For i = 0 To dgRisultatoRicerca.Items.Count - 1
                Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
                rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                'If Mychk.Checked = True Then
                '    rwGriglia.Item(ValoreRichkDt) = "1"
                'Else
                '    rwGriglia.Item(ValoreRichkDt) = "0"
                'End If
                rwGriglia.Item(ValoreRichkDt) = Mychk.Checked
            Next i
            Session("SessionDtSedi") = dtGriglia
            dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
            dgRisultatoRicerca.DataSource = Session("SessionDtSedi")
            dgRisultatoRicerca.DataBind()

            '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
            For i = 0 To dgRisultatoRicerca.Items.Count - 1
                Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
                rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                If rwGriglia.Item(ValoreRichkDt) = "True" Then
                    Mychk.Checked = True
                Else
                    Mychk.Checked = False
                End If
            Next i
        Else
            If hdd_Check.Value = "SI" Then
                EliminaTuttiCheck()
                For i = 0 To dgRisultatoRicerca.Items.Count - 1
                    Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
                    rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                    'If Mychk.Checked = True Then
                    '    rwGriglia.Item(ValoreRichkDt) = "1"
                    'Else
                    '    rwGriglia.Item(ValoreRichkDt) = "0"
                    'End If
                    rwGriglia.Item(ValoreRichkDt) = Mychk.Checked
                Next i
                Session("SessionDtSedi") = dtGriglia
                dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
                dgRisultatoRicerca.DataSource = Session("SessionDtSedi")
                dgRisultatoRicerca.DataBind()

                '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
                For i = 0 To dgRisultatoRicerca.Items.Count - 1
                    Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
                    rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                    If rwGriglia.Item(ValoreRichkDt) = "True" Then
                        Mychk.Checked = True
                    Else
                        Mychk.Checked = False
                    End If
                Next i
            End If
        End If
        TogliCheck()
        FormattaVariazioni()
        ColoraCelle()
    End Sub
#End Region
#Region "Funzionalita"

    Private Function calcDiff(ByVal nomeCampo As String, ByVal etichetta As String) As String
        Return String.Format(" case when {0}New is null then '' else '{1}: <b>' + rtrim(ltrim({0}New)) + '</b> ('+ coalesce({0}Old, '<i>nessun valore</i>') + ') | ' End ", nomeCampo, etichetta)
    End Function
    Private Function calcDiffEnte(ByVal nomeCampo As String, ByVal etichetta As String) As String
        Return String.Format(" case when {0}New is null then '' else '{1}: <b>' + (select denominazione from enti where idEnte={0}New) + " &
            "'</b> ('+ coalesce((select CodiceRegione+' - ' +denominazione from enti where IdEnte={0}Old), '<i>nessun valore</i>') + ') | ' End ", nomeCampo, etichetta)
    End Function

    Private Function calcDiffN(ByVal nomeCampo As String, ByVal etichetta As String) As String
        Return String.Format(" case when {0}New is null then '' else '{1}: <b>' + cast({0}New as varchar(10)) + '</b> ('+ coalesce(cast({0}Old as varchar(10)), '<i>nessun valore</i>') + ') | ' End ", nomeCampo, etichetta)
    End Function

    Private Function calcDiffTitoloGiuridico(ByVal nomeCampo As String, ByVal etichetta As String) As String
        Return String.Format(" case when {0}New is null then '' else '{1}: <b>' + (select TitoloGiuridico from TitoliGiuridici where IdTitoloGiuridico={0}New) + " &
            "'</b> ('+ coalesce((select TitoloGiuridico from TitoliGiuridici where IdTitoloGiuridico={0}Old), '<i>nessun valore</i>') + ') | ' End ", nomeCampo, etichetta)
    End Function

    Private Sub RicercaSedi()
        Dim dtSedi As DataTable = New DataTable
        Dim IdEnteRicerca As String = String.Empty
        If TxtCodiceRegione.Text <> String.Empty Then
            Dim sqlGetIdEnte = "Select top 1 IDEnte FROM enti WHERE CodiceRegione = @CodiceRegione"
            Dim EnteCommand As New SqlCommand(sqlGetIdEnte, Session("conn"))
            EnteCommand.Parameters.AddWithValue("@CodiceRegione", TxtCodiceRegione.Text)
            Dim dtrEnte As SqlDataReader = EnteCommand.ExecuteReader()
            If dtrEnte.Read Then
                IdEnteRicerca = dtrEnte("IdEnte")

            End If
            dtrEnte.Close()
        End If

        strsql = "select distinct 0 ordina,enti.denominazione as ente,enti.codiceregione as CodiceEnte, cast(identesedeattuazione as varchar(10)) + ' - ' + entisedi.denominazione as sede," &
        " entisedi.idEntesede, entisedi.IdEnte, SediFuoriAdeguamento.IdVariazioneFuoriAdeguamento,SediFuoriAdeguamento.IdEnteSede," &
        calcDiffEnte("IdEnteProprietario", "Ente") & "+" & vbCrLf &
        calcDiff("Denominazione", "Denominazione") & "+" & vbCrLf &
        calcDiff("PrefissoTelefono", "Prefisso Telefono") & "+" & vbCrLf &
        calcDiff("Telefono", "Telefono") & "+" & vbCrLf &
        calcDiffTitoloGiuridico("IdTitoloGiuridico", "Titolo di disponibilità") & "+" & vbCrLf &
        calcDiffN("NMaxVolontari", "N. Volontari") & " as Variazioni, " &
        " SediFuoriAdeguamento.FlgValidazione,SediFuoriAdeguamento.DataVariazione,SediFuoriAdeguamento.FlgEmail, statientisedi.StatoEnteSede " &
        " from enti " &
        " inner join entisedi on(entisedi.idente=enti.idEnte) " &
        " left  join entisediattuazioni on entisediattuazioni.identesede=entisedi.identesede " &
        " left  join statientisedi statosedefisica on(entisediattuazioni.idstatoentesede=statosedefisica.idstatoEntesede) " &
        " inner  join statientisedi on(entisedi.idstatoentesede=statientisedi.idstatoEntesede) " &
        " inner join comuni on (entisedi.idcomune=Comuni.idcomune)  " &
        " inner join Provincie on (provincie.idprovincia=Comuni.idProvincia)  " &
        " INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione " &
        " inner join  Entiseditipi on (entisedi.identesede=Entiseditipi.idEntesede)  " &
        " inner join tipisedi on (tipisedi.idtiposede=Entiseditipi.idtiposede) " &
        " inner join TitoliGiuridici on TitoliGiuridici.IdTitoloGiuridico = entisedi.IdTitoloGiuridico" &
        " Inner Join SediFuoriAdeguamento on SediFuoriAdeguamento.IdenteSede= entisedi.IdEnteSede" &
        " where substring(entisedi.usernamestato,1,1) <> 'N' AND SediFuoriAdeguamento.FlgValido=1"

        If IdEnteRicerca <> String.Empty Then
            strsql = strsql & " and   entisedi.idente = " & IdEnteRicerca & ""
        End If

        'If Request.QueryString("CheckProvenienza") = "RicercaEnteInAccordo" Then

        '    strsql = strsql & " and enti.idente=" & Request.QueryString("IdEnteFiglio") & ""

        'Else


        If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
            strsql = strsql & " and enti.denominazione LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%'"
        End If
        If Trim(TxtCodiceRegione.Text) <> vbNullString Then
            strsql = strsql & " and enti.codiceregione = '" & Replace(TxtCodiceRegione.Text, " '", "''") & "'"
        End If
        If Trim(txtdenominazione.Text) <> "" Then
            strsql = strsql & " and entisedi.denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodice.Value) <> "" Then
            strsql = strsql & " and entisedi.codiceSedeEnte='" & Replace(txtCodice.Value, "'", "''") & "'"
        End If
        If Not IsNothing(ddlTipologia.SelectedItem.Text) Then
            If Trim(ddlTipologia.SelectedItem.Text) <> "" Then
                strsql = strsql & " and tipisedi.tiposede='" & Replace(ddlTipologia.SelectedItem.Text, "'", "''") & "'"
            End If
        End If
        If ddlstato.SelectedItem.Text <> "" Then
            'strsql = strsql & " and  statientisedi.statoentesede='" & ddlstato.SelectedItem.Text & "'"
            strsql = strsql & " and case isnull(statosedefisica.statoentesede,'vuoto') when 'vuoto' then statientisedi.statoentesede else  statosedefisica.statoentesede end =replace('" & ddlstato.SelectedItem.Text & "', 'Accreditata', 'Iscritta')"
        End If
        If Trim(txtregione.Text) <> vbNullString Then
            strsql = strsql & " and Regioni.Regione LIKE '" & Replace(txtregione.Text, "'", "''") & "%'"
        End If
        If Trim(txtProvincia.Text) <> vbNullString Then
            strsql = strsql & " and Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            strsql = strsql & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If

        If Trim(txtIndirizzo.Text) <> vbNullString Then
            strsql = strsql & " and (entisedi.indirizzo + ' ' + entisedi.Civico) LIKE '" & Replace(txtIndirizzo.Text, "'", "''") & "%'"
        End If

        If chkRiferimentoRimborsi.Checked = True Then
            strsql = strsql & " and EntiSedi.RiferimentoRimborsi = 1"
        End If

        Dim primo As Integer
        Dim secondo As Integer
        primo = Integer.TryParse(Trim(txtCodSedeAtt.Text), primo)
        If Trim(txtCodSedeAtt.Text) <> "" Then
            If primo = -1 Then
                strsql = strsql & " and EntiSediAttuazioni.identesedeattuazione=" & Trim(txtCodSedeAtt.Text) & ""
            Else
                lblErrore.Text = "Inserire Valore Numerico"
                primo = 1
            End If
        End If


        If ddlCertificazione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and isnull(entisediattuazioni.Certificazione,2) =" & ddlCertificazione.SelectedValue
        End If
        If ddlDuplicati.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and dbo.DoppioneSede(EntiSediAttuazioni.identesedeattuazione)=" & ddlDuplicati.SelectedValue
        End If
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If
        If ddlLocalizzazione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and comuni.ComuneNazionale = " & ddlLocalizzazione.SelectedValue
        End If
        If ddlRichistaVariazione.SelectedValue <> "" Then
            strsql = strsql & " and isnull(entisedi.RichiestaModifica,0)=" & ddlRichistaVariazione.SelectedValue & " "
        End If
        If ddlVisto.SelectedItem.Text <> "TUTTI" Then
            strsql = strsql & " and SediFuoriAdeguamento.FlgValidazione = " & ddlVisto.SelectedValue
        End If
        If IdEnteRicerca <> String.Empty Then
            strsql = strsql & " union " &
        "select distinct 1 ordina,enti.denominazione as ente,enti.codiceregione as CodiceEnte,cast(identesedeattuazione as varchar(10)) + ' - ' + entisedi.denominazione as sede," &
        " entisedi.idEntesede, entisedi.IdEnte, SediFuoriAdeguamento.IdVariazioneFuoriAdeguamento,SediFuoriAdeguamento.IdEnteSede," &
        calcDiffEnte("IdEnteProprietario", "Ente") & "+" & vbCrLf &
        calcDiff("Denominazione", "Denominazione") & "+" & vbCrLf &
        calcDiff("PrefissoTelefono", "Prefisso Telefono") & "+" & vbCrLf &
        calcDiff("Telefono", "Telefono") & "+" & vbCrLf &
        calcDiffTitoloGiuridico("IdTitoloGiuridico", "Titolo di disponibilità") & "+" & vbCrLf &
        calcDiffN("NMaxVolontari", "N. Volontari") & " as Variazioni, " &
        " SediFuoriAdeguamento.FlgValidazione,SediFuoriAdeguamento.DataVariazione,SediFuoriAdeguamento.FlgEmail, statientisedi.StatoEnteSede " &
        " from enti" &
        " inner join entisedi on(entisedi.idente=enti.idEnte) " &
        " left  join entisediattuazioni on entisediattuazioni.identesede=entisedi.identesede " &
        " left  join statientisedi statosedefisica on(entisediattuazioni.idstatoentesede=statosedefisica.idstatoEntesede) " &
        " inner  join statientisedi on(entisedi.idstatoentesede=statientisedi.idstatoEntesede) " &
        " inner join comuni on (entisedi.idcomune=Comuni.idcomune)  " &
        " inner join Provincie on (provincie.idprovincia=Comuni.idProvincia)  " &
        " INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione " &
        " inner join  Entiseditipi on (entisedi.identesede=Entiseditipi.idEntesede)" &
        " inner join tipisedi on (tipisedi.idtiposede=Entiseditipi.idtiposede)" &
        " inner join entirelazioni on (entirelazioni.identefiglio=enti.idente) " &
        " left join associaentirelazionisedi on (entisedi.identesede=associaentirelazionisedi.identesede " &
        " and entirelazioni.identerelazione=associaentirelazionisedi.identerelazione)" &
        " inner join TitoliGiuridici on TitoliGiuridici.IdTitoloGiuridico = entisedi.IdTitoloGiuridico" &
        " inner join SediFuoriAdeguamento on entisedi.IdEntesede=SediFuoriAdeguamento.IdEnteSede" &
        " where substring(entisedi.usernamestato,1,1) <> 'N' AND SediFuoriAdeguamento.FlgValido=1" 'and entirelazioni.datafinevalidità is  null
            If IdEnteRicerca <> String.Empty Then
                strsql = strsql & " and entirelazioni.identePadre = " & IdEnteRicerca & " "
            End If

            'If Request.QueryString("CheckProvenienza") = "RicercaEnteInAccordo" Then

            '    strsql = strsql & " and enti.idente=" & Request.QueryString("IdEnteFiglio") & ""

            'Else


            If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
                strsql = strsql & " and enti.denominazione LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%'"
            End If
            'If Trim(TxtCodiceRegione.Text) <> vbNullString Then
            '    strsql = strsql & " and enti.codiceregione = '" & Replace(TxtCodiceRegione.Text, " '", "''") & "'"
            'End If
            'End If

            If Trim(txtdenominazione.Text) <> "" Then
                strsql = strsql & " and entisedi.denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
            End If
            If Trim(txtCodice.Value) <> "" Then
                strsql = strsql & " and entisedi.codiceSedeEnte='" & Replace(txtCodice.Value, "'", "''") & "'"
            End If
            If Trim(ddlTipologia.SelectedItem.Text) <> "" Then
                strsql = strsql & " and tipisedi.tiposede='" & Replace(ddlTipologia.SelectedItem.Text, "'", "''") & "'"
            End If
            If ddlstato.SelectedItem.Text <> "" Then
                '            strsql = strsql & " and  statientisedi.statoentesede='" & ddlstato.SelectedItem.Text & "'"
                strsql = strsql & " and case isnull(statosedefisica.statoentesede,'vuoto') when 'vuoto' then statientisedi.statoentesede else  statosedefisica.statoentesede end ='" & ddlstato.SelectedItem.Text & "'"
            End If
            If Trim(txtregione.Text) <> vbNullString Then
                strsql = strsql & " and Regioni.Regione LIKE '" & Replace(txtregione.Text, "'", "''") & "%'"
            End If
            If Trim(txtProvincia.Text) <> vbNullString Then
                strsql = strsql & " and Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%'"
            End If
            If Trim(txtComune.Text) <> vbNullString Then
                strsql = strsql & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
            End If
            'If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
            '    If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
            '        strsql = strsql & " and enti.denominazione = '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "'"
            '    End If
            'Else
            '    If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
            '        strsql = strsql & " and enti.denominazione LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%'"
            '    End If
            'End If
            'If Trim(TxtCodiceRegione.Text) <> vbNullString Then
            '    strsql = strsql & " and enti.codiceregione = '" & Replace(TxtCodiceRegione.Text, " '", "''") & "'"
            'End If
            If Trim(txtIndirizzo.Text) <> vbNullString Then
                strsql = strsql & " and (entisedi.indirizzo + ' ' + entisedi.Civico) LIKE '" & Replace(txtIndirizzo.Text, "'", "''") & "%'"
            End If
            If chkRiferimentoRimborsi.Checked = True Then
                strsql = strsql & " and EntiSedi.RiferimentoRimborsi = 1 "
            End If


            secondo = Integer.TryParse(Trim(txtCodSedeAtt.Text), secondo)
            If Trim(txtCodSedeAtt.Text) <> "" Then
                If secondo = -1 Then
                    strsql = strsql & " and EntiSediAttuazioni.identesedeattuazione=" & Trim(txtCodSedeAtt.Text) & ""
                Else
                    lblErrore.Text = "Inserire Valore Numerico"
                    secondo = 1
                End If
            End If

            'agg. da sc il 05/12/2008
            If ddlCertificazione.SelectedItem.Text <> "Tutti" Then
                strsql = strsql & " and isnull(entisediattuazioni.Certificazione,0) =" & ddlCertificazione.SelectedValue
            End If
            If ddlLocalizzazione.SelectedItem.Text <> "Tutti" Then
                strsql = strsql & " and comuni.ComuneNazionale = " & ddlLocalizzazione.SelectedValue
            End If
            'agg. da sc il 05/07/2011 combo su segnalazione della sede
            If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
                strsql = strsql & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue & ""
            End If
            '*** Aggiunto da Simona Cordella il 15/09/2014
            'filtro per RichiestaVariazione
            If ddlRichistaVariazione.SelectedValue <> "" Then
                strsql = strsql & " and isnull(entisedi.RichiestaModifica,0)=" & ddlRichistaVariazione.SelectedValue & " "
            End If
            '*** 
            'agg. da sc il 09/06/2009 trovo se si sono sedi con lo stesso indirizzo,civico, comune,cap 


            If ddlDuplicati.SelectedItem.Text <> "Tutti" Then
                strsql = strsql & " and dbo.DoppioneSede(EntiSediAttuazioni.identesedeattuazione)=" & ddlDuplicati.SelectedValue
                '  strsql = strsql & " order by comune,indirizzo "
                'Else
                '    strsql = strsql & " order by ordina,statientisedi.ordine, ente,sede,acquisita"
            End If
            If ddlVisto.SelectedItem.Text <> "TUTTI" Then
                strsql = strsql & " and SediFuoriAdeguamento.FlgValidazione = " & ddlVisto.SelectedValue
            End If
        End If

        strsql = strsql & " order by ordina, ente, sede"


        If (primo = -1 And secondo = -1) Or (primo = 0 And secondo = 0) Then

            dtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If txtRicerca.Value <> "" Then
                dgRisultatoRicerca.CurrentPageIndex = 0
                txtRicerca.Value = ""
            End If
            If lblpage.Value <> "" Then
                dgRisultatoRicerca.CurrentPageIndex = CInt(lblpage.Value)
            Else
                dgRisultatoRicerca.CurrentPageIndex = 0
            End If

            'txtstrsql.Value = strsql
            CaricaDataGrid(dgRisultatoRicerca)
            If (dgRisultatoRicerca.Items.Count > 0) Then
                CmdEsporta.Visible = True
                ApriCSV1.Visible = False
                dgRisultatoRicerca.Caption = "Risultato Ricerca modifiche fuori adeguamento Sedi"
                chkSelDesel.Visible = True
                cmdSalva.Visible = True
            Else
                CmdEsporta.Visible = False
                ApriCSV1.Visible = False
                dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato"
                chkSelDesel.Visible = False
                cmdSalva.Visible = False
            End If
            dtSedi = ClsServer.CreaDataTable(strsql, False, Session("conn"))
            'assegno il dataset alla griglia del risultato
            dgRisultatoRicerca.DataSource = dtSedi
            dgRisultatoRicerca.DataBind()
            Session("SessionDtSedi") = dtSedi
            TogliCheck()
            controllaChek()
            ColoraCelle()
        End If
    End Sub

    Sub CaricaDataGrid(ByRef datagrid As DataGrid) 'valorizzo la datagrid passata
        'Verifico l'abilitazione alla visualizzazione di determinate colonne
        'Dim strMess As String
        'If Session("TipoUtente") = "E" Then
        '    datagrid.Columns(16).Visible = False
        '    ' agg da simona cordella il 11/12/2008
        '    ' se sono un utente di tipo E rendo la colonna invisibile certificazione invisibile
        '    datagrid.Columns(17).Visible = False

        'End If
        'datagrid.Columns(18).Visible = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        'datagrid.Columns(19).Visible = ClsUtility.ForzaSedeSottopostaVerifica(Session("Utente"), Session("conn"))

        datagrid.DataSource = dtsGenerico
        datagrid.DataBind()


        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(5) As String
        Dim NomiCampiColonne(5) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Ente"
        NomeColonne(1) = "Ente"
        NomeColonne(2) = "Sede"
        NomeColonne(3) = "Data Variazione"
        NomeColonne(4) = "Variazioni"
        NomeColonne(5) = "Visto"
        'NomeColonne(5) = "Indirizzo"
        'NomeColonne(6) = "Comune"
        'NomeColonne(7) = "Telefono"
        'NomeColonne(8) = "NMaxVolontari"
        'NomeColonne(9) = "Titolo di Possedimento"
        'NomeColonne(10) = "Normativa81"
        'NomeColonne(11) = "Conformita"
        'NomeColonne(12) = "Soggetto Estero"
        'NomeColonne(13) = "Dichiarazione di Soggetto Estero"


        'If Session("TipoUtente") <> "E" Then
        '    NomeColonne(14) = "Presenza Certificazione"
        'End If


        NomiCampiColonne(0) = "CodiceEnte"
        NomiCampiColonne(1) = "Ente"
        NomiCampiColonne(2) = "Sede"
        NomiCampiColonne(3) = "DataVariazione"
        NomiCampiColonne(4) = "Variazioni"
        NomiCampiColonne(5) = "FlgValidazione"
        'NomiCampiColonne(5) = "Indirizzo"
        'NomiCampiColonne(6) = "Comune"
        'NomiCampiColonne(7) = "Telefono"
        'NomiCampiColonne(8) = "NMaxVolontari"
        'NomiCampiColonne(9) = "TitoloPossedimento"
        'NomiCampiColonne(10) = "Normativa81"
        'NomiCampiColonne(11) = "Conformita"
        'NomiCampiColonne(12) = "Soggettoestero"
        'NomiCampiColonne(13) = "DichSoggettoEstero"
        'If Session("TipoUtente") <> "E" Then
        '    NomiCampiColonne(14) = "Certificazione"
        'End If

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        'If Session("TipoUtente") <> "E" Then
        '    CaricaDataTablePerStampa(dtsGenerico, 14, NomeColonne, NomiCampiColonne)
        'Else
        '    CaricaDataTablePerStampa(dtsGenerico, 13, NomeColonne, NomiCampiColonne)
        'End If
        CaricaDataTablePerStampa(dtsGenerico, 5, NomeColonne, NomiCampiColonne)
        'CaricaDataTablePerStampa(dtsGenerico, 8, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        If datagrid.Items.Count = 0 Then

            If Request.QueryString("esporta") = "si" Then
                CmdEsporta.Visible = False
            End If
        Else
            ColoraCelle()
            'ControllaSede()
            If Request.QueryString("esporta") = "si" Then
                CmdEsporta.Visible = True
                datagrid.Columns(0).Visible = False
            End If
        End If
        If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
            datagrid.Columns(0).Visible = False
        End If

    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub


    Private Sub controllaChek()
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkSedi"), CheckBox)
            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(7).Text.ToUpper
                Case "TRUE"
                    check.Checked = True
                Case Else
                    check.Checked = False
            End Select
            'check.Enabled = False
            dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text = Replace(dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text, " | ", "</br>")
        Next
    End Sub
    Private Sub ColoraCelle()

        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim intConta As Integer
        Dim img As ImageButton
        For Each item In dgRisultatoRicerca.Items

            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_STATOENTESEDE).Text
                'Case "1"
                    'For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                    '    dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
                    '    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.LightGreen
                    'Next
                    'aggiunto il 04/12/2008 da simona cordella
                    '    'se sono un utene U o R coloro il font della griglio in viola sono le sede è da valutare
                    '    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    '        'se la sede è da valutare coloro il FONT DI VIOLA
                    '        Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text
                    '            Case "Da Valutare"
                    '                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                    '                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.FromArgb(201, 96, 185)
                    '                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).Font.Bold = True
                    '                Next
                    '            Case "No"
                    '                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                    '                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                    '                Next
                    '        End Select
                    '    End If

                Case "Presentata"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Khaki
                    Next
                Case "Accreditata"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
                    Next

                Case "Sospesa"
                        For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Gainsboro
                    Next
                Case "Cancellata"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                    Next

            End Select

        Next
    End Sub
#End Region

    Private Sub Includi_tutte_sedi()
        'Creata da Alessandra Taballione il 29/07/2004
        'Per facilitare l'Utente UNSC l'Inserimento dei dati 
        Dim cmdGenerico As SqlClient.SqlCommand
        Dim MyRow As DataRow
        Dim datat As DataTable
        Dim datat2 As DataTable
        Dim MyRow2 As DataRow
        'Realizzato da Alessandra Taballione il 10/03/2004
        'inclusione della sede
        'Inserimento della relazione PadreFiglio
        strsql = "select 1 ordina, entisedi.identesede,entirelazioni.identefiglio,entirelazioni.identePadre " &
        " from enti " &
        " inner join entisedi on(entisedi.idente=enti.idEnte)  " &
        " inner join statientisedi on(entisedi.idstatoentesede=statientisedi.idstatoEntesede)  " &
        " inner join  Entiseditipi on (entisedi.identesede=Entiseditipi.idEntesede) " &
        " inner join tipisedi on (tipisedi.idtiposede=Entiseditipi.idtiposede) " &
        " inner join entirelazioni on (entirelazioni.identefiglio=enti.idente)  " &
        " left join associaentirelazionisedi on (entisedi.identesede=associaentirelazionisedi.identesede  " &
        " and entirelazioni.identerelazione=associaentirelazionisedi.identerelazione) " &
        " where(entirelazioni.identePadre = " & Session("idEnte") & " And entirelazioni.datafinevalidità Is null And associaentirelazionisedi.idassociaentirelazionisedi Is null)" &
        " and (statientisedi.attiva=1 or statientisedi.daaccreditare=1) and tipisedi.idtiposede = 4 order by entisedi.identesede"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        datat = ClsServer.CreaDataTable(strsql, False, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        For Each MyRow In datat.Rows
            strsql = "insert into associaentirelazionisedi(idEnteSede,identeRelazione,DataCreazionerecord)" &
            " select  " & MyRow.Item("identesede") & " ,identerelazione,getdate() from entirelazioni where identePadre= " & Session("IdEnte") & "  and identeFiglio=" & MyRow.Item("identeFiglio") & ""
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            'Aggiunto da Alessandra Taballione il 27.07.04
            'Insererimento della inclusione per tuute le sedi di attuazione della sede
            strsql = "Select entisediattuazioni.identeSedeAttuazione " &
            " from entisediattuazioni " &
            " inner join statientisedi on (entisediattuazioni.idstatoentesede=statientisedi.idstatoentesede)" &
            " Where identesede=" & MyRow.Item("identesede") & " And (statientisedi.attiva = 1 Or statientisedi.Defaultstato = 1)"
            'strsql = "Select * from entisediattuazioni where identesede=" & MyRow.Item("identesede") & ""
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            datat2 = ClsServer.CreaDataTable(strsql, False, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            For Each MyRow2 In datat2.Rows
                strsql = "insert into associaentirelazionisediAttuazioni(idEnteSedeattuazione,identeRelazione,DataCreazionerecord)" &
               " select  " & MyRow2.Item("identeSedeAttuazione") & " ,identerelazione,getdate() from entirelazioni where identePadre= " & Session("IdEnte") & "  and identeFiglio=" & MyRow.Item("identeFiglio") & ""
                cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            Next
        Next
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'If txtstrsql.Value <> "" Then
        '    RicercaSediStrsql(txtstrsql.Value)
        'Else
        RicercaSedi()
        'End If
    End Sub

    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
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

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea.Substring(0, xLinea.Length - 1))
        ' writer.WriteLine(xLinea)
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
            writer.WriteLine(xLinea.Substring(0, xLinea.Length - 1))
            ' writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function
    Private Sub ControllaSede()
        Dim indirizzo1, indirizzo2, denominazione1, denominazione2, sede, comune, cap, strApp, strSql As String
        Dim chkIndirizzo, chkSede, chkRipetizione As Boolean
        Dim idEnte, idsede As Integer
        Dim strArray
        Dim messaggio As String = ""
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim tRisultati = New DataTable
        Dim Img As ImageButton
        Dim rgx As Regex = New Regex("[0-9]")
        'strsql = "Select IdEnteSede, entisedi.indirizzo + ' ' + entisedi.Civico As IndirizzoCompleto"
        'strsql += " from entisedi inner join comuni on (entisedi.idcomune=Comuni.idcomune) inner join Provincie on (provincie.idprovincia=Comuni.idProvincia) INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione"
        'strsql += "where (entisedi.indirizzo + ' ' + entisedi.Civico) = '" & indirizzo & "' and EntiSedi.CAP='" & cap & "' and (comuni.denominazione +' ['+ provincie.provincia +']')= '" & comune & "'"
        'strsql += " And IDEnteSede<>" & idSede & " And IDEnte<>" & idEnte

        strSql = "Select IdEnteSede, entisedi.denominazione, entisedi.indirizzo + ' ' + entisedi.Civico As Indirizzo, EntiSedi.CAP, comuni.denominazione +' ['+ provincie.provincia +']' as comune"
        strSql += " from entisedi inner join comuni on (entisedi.idcomune=Comuni.idcomune) inner join Provincie on (provincie.idprovincia=Comuni.idProvincia) INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione"
        strSql += " where idEnte = " & Session("IDEnte")

        SqlCmd.CommandText = strSql
        SqlCmd.CommandType = CommandType.Text
        SqlCmd.Connection = Session("conn")
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(tRisultati)

        For Each item In dgRisultatoRicerca.Items
            chkSede = False
            chkIndirizzo = False
            chkRipetizione = False
            sede = dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_SEDE).Text
            Img = DirectCast(dgRisultatoRicerca.Items(item.ItemIndex).FindControl("IdImgAlertNome"), ImageButton)

            If tRisultati.Rows.Count > 0 Then

                denominazione1 = rgx.Replace(sede, "").Trim

                For Each r As DataRow In tRisultati.Rows
                    If r("IdEnteSede") <> dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_IDENTESEDE).Text Then
                        indirizzo2 = r("indirizzo").ToString.Trim & r("CAP").ToString.Trim & r("Comune").ToString.Trim
                        If indirizzo2 = indirizzo1 Then chkIndirizzo = True
                        denominazione2 = rgx.Replace(r("Denominazione"), "").Trim
                        If denominazione2 = denominazione1 Then chkSede = True
                    End If

                Next
            Else

            End If

            If sede.Length > 4 Then
                For c = 0 To sede.Length - 4
                    If sede.Substring(c, 1) = sede.Substring(c + 1, 1) And sede.Substring(c, 1) = sede.Substring(c + 2, 1) And sede.Substring(c, 1) = sede.Substring(c + 3, 1) Then
                        chkRipetizione = True
                    End If
                Next
                For d = 0 To sede.Length - 2
                    strArray = ""
                    If sede.Substring(d, 1) <> sede.Substring(d + 1, 1) Then
                        strArray = sede.Split(sede.Substring(d, 2))
                        If UBound(strArray) > 2 Then
                            chkRipetizione = True
                        End If
                    End If
                Next
            Else
                messaggio = "Lunghezza denominazione sede inferiore a 4 caratteri<br>"
            End If
            If chkSede Then messaggio = "Nome sede corrispondente ad altra sede<br>"
            If chkIndirizzo Then messaggio = "Indirizzo corrispondente ad altra sede"
            'If messaggio <> String.Empty Then
            '    Img.Visible = True
            '    Img.ToolTip = messaggio
            '    Img.AlternateText = messaggio
            'Else
            '    Img.Visible = False
            'End If
        Next
    End Sub

    Sub CeccaTutti()
        Dim item As DataGridItem
        Dim chkValore As CheckBox
        'ricordo il valore ceccato nella pagina corrente
        For Each item In dgRisultatoRicerca.Items
            chkValore = DirectCast(item.FindControl("chkSedi"), CheckBox)
            chkValore.Checked = True
        Next
    End Sub

    Sub EliminaTuttiCheck()
        Dim item As DataGridItem
        Dim chkValore As CheckBox
        'ricordo il valore ceccato nella pagina corrente
        For Each item In dgRisultatoRicerca.Items
            chkValore = DirectCast(item.FindControl("chkSedi"), CheckBox)
            chkValore.Checked = False
        Next
    End Sub

    Sub TogliCheck()
        Dim item As DataGridItem
        Dim chkValore As CheckBox
        'ricordo il valore ceccato nella pagina corrente
        For Each item In dgRisultatoRicerca.Items
            hdd_Check.Value = "NO"
            chkValore = DirectCast(item.FindControl("chkSedi"), CheckBox)
            chkValore.Attributes.Add("onclick", "TogliCheck()")
        Next
    End Sub


    Private Sub chkSelDesel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelDesel.CheckedChanged
        If hdPerSelezione.Value.ToUpper <> "BLOCCA" Then
            ShowPopUPControllo = 1
            If chkSelDesel.Checked Then
                lblMsgSeleziona.Text = "Vuoi Selezionare tutte le variazioni presenti nella lista di ricerca ?"
                cmdSeleziona.Text = "Seleziona Tutti"
            Else
                lblMsgSeleziona.Text = "Vuoi deselezionare tutte le variazioni presenti nella lista di ricerca ?"
                cmdSeleziona.Text = "Deseleziona Tutti"
            End If
        Else
            hdPerSelezione.Value = ""
        End If



    End Sub

    Private Sub cmdSeleziona_Click(sender As Object, e As EventArgs) Handles cmdSeleziona.Click


        Dim i, j, page As Integer
        Dim Mychk As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn
        Dim ValoreRichkDt As Integer = 9 ' valore che ricorda il campo riccheck della query (si per la ricerca sedi di progetto che per le ricerca sedi)
        page = dgRisultatoRicerca.CurrentPageIndex
        dtGriglia = Session("SessionDtSedi")
        clGriglia = dtGriglia.Columns(ValoreRichkDt)
        clGriglia.ReadOnly = False
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")

            If (Mychk.Enabled = True) Then
                Mychk.Checked = chkSelDesel.Checked
                hdd_Check.Value = "SI"
            End If
        Next i
        For j = 0 To dtGriglia.Rows.Count - 1
            rwGriglia = dtGriglia.Rows(j)
            If chkSelDesel.Checked Then
                rwGriglia.Item(ValoreRichkDt) = 1
            Else
                rwGriglia.Item(ValoreRichkDt) = 0
            End If

        Next

        Session("SessionDtSedi") = dtGriglia
        dgRisultatoRicerca.DataSource = Session("SessionDtSedi")
        dgRisultatoRicerca.DataBind()
        TogliCheck()
        FormattaVariazioni()
        ColoraCelle()

    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        ShowPopUPControllo = 2

    End Sub


    Sub ModificaSede(ByVal IDSedeVariazione As Integer, ByVal Validazione As Integer)
        Dim strSql As String
        Dim CmdSede As SqlClient.SqlCommand
        strSql = "UPDATE  Sedifuoriadeguamento SET "
        strSql = strSql & " flgValidazione = " & Validazione & "  WHERE IdVariazioneFuoriAdeguamento = " & IDSedeVariazione

        CmdSede = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub

    Private Sub cmdSalvaDati_Click(sender As Object, e As EventArgs) Handles cmdSalvaDati.Click
        Dim ContaCheck As Integer = 0
        Dim i As Integer
        Dim chkValore As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn


        'dgRisultatoRicerca.CurrentPageIndex = 0
        lblErrore.Text = ""


        dtGriglia = Session("SessionDtSedi")
        clGriglia = dtGriglia.Columns(9)
        clGriglia.ReadOnly = False

        'ricordo il valore ceccato nella pagina corrente
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            chkValore = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If chkValore.Checked = True Then
                rwGriglia.Item(9) = 1
            Else
                rwGriglia.Item(9) = 0
            End If
        Next i


        For i = 0 To dtGriglia.Rows.Count - 1
            ModificaSede(CInt(dtGriglia.Rows(i).Item(6)), CInt(dtGriglia.Rows(i).Item(9))) 'IDENTESEDETEATTUAZIONE
        Next i
        lblErrore.CssClass = "msgConferma"
        lblErrore.Text = "Salvataggio eseguito con successo."
        RicercaSedi()
    End Sub

    Sub FormattaVariazioni()
        For Each item In dgRisultatoRicerca.Items

            dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text = Replace(dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text, " | ", "</br>")
        Next
    End Sub


End Class