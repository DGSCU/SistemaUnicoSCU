Imports System.Drawing

Public Class WfrmGestioneServizi
    Inherits System.Web.UI.Page

    Dim strsql As String
    Dim dtsGenerico As DataSet
    'Protected WithEvents imgAlert As System.Web.UI.WebControls.Image
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Protected WithEvents txtidente As New System.Web.UI.WebControls.TextBox
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") And Request.QueryString("Servizi") = "Risorsa" Then
            'lblEnte.Visible = True
            'txtcodiceente.Visible = True
        Else
            'lblEnte.Visible = False
            'txtcodiceente.Visible = False
        End If
        If IsPostBack = False Then
            If Request.QueryString("Servizi") = "Risorsa" Then
                CaricaServizi()
            Else
                strsql = " Select enti.idclasseaccreditamento " & _
                " from classiaccreditamento " & _
                " inner join enti on enti.idclasseaccreditamentorichiesta=classiaccreditamento.idclasseaccreditamento " & _
                " inner join statienti on statienti.idstatoente=enti.idstatoente " & _
                " where entiinpartenariato = 1 "
                strsql = strsql & " And idente = " & Session("idEnte") & "" & _
                " and (statienti.presentazioneprogetti=1 or statienti.istruttoria=1)"
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If dtrGenerico.HasRows = True Then
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    CaricaServizi()
                Else
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    CaricaServiziVuota()
                    cmdConferma.Visible = False
                    'txtMessaggio.ForeColor = Color.Red
                    txtMessaggio.Text = "Attenzione l'Ente selezionato non può fornire il servizio desiderato. "
                    txtMessaggio.Text = ""
                    'imgAlert.Visible = True
                    'imgAlert.ImageUrl = "Images/alert3.gif"
                End If
            End If
        End If

        'FZ controllo per disabilitare la maschera nel caso sia un'"R" che sta 
        'controllando i progetti di un' ente che nn ha la stessa regiojneee di competenza
        If ClsUtility.ControlloRegioneCompetenza(Session("TipoUtente"), Session("idEnte"), Session("Utente"), Session("IdStatoEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = False Then
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            CaricaServiziVuota()
            cmdConferma.Visible = False
            'txtMessaggio.ForeColor = Color.Red
            txtMessaggio.Text = "Attenzione, l'ente non è di propria competenza. Impossibile effettuare modifiche."
            'imgAlert.Visible = True
            'imgAlert.ImageUrl = "Images/alert3.gif"
        End If
        'FZ fine controllo

    End Sub

    Private Sub CaricaServizi()
        'strsql = " Select 0 as ordina,  sistemi.idsistema,sistema, " & _
        '" isnull(entisistemi.identesistema,0)as identesistema, " & _
        '" entisistemi.Username,entisistemi.datacreazionerecord,  " & _
        '" isnull((select a.identeSistema from entisistemi a " & _
        '" INNER JOIN " & _
        '" EntiAcquisizioneServizi ON a.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema " & _
        '" where a.idente=" & Session("idEnte") & " and a.identesistema=entisistemi.identesistema ),0) as acquisito,idente as idente, " & _
        '" case sistemi.sistema when 'Sistema Formativo' then '<input type=""checkbox"" runat=""server"" value=""1"" checked onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',true)"" id=""chkScelta' + convert(varchar,sistemi.idsistema) + '"">' else '<input type=""checkbox"" runat=""server"" value=""1"" checked onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',false)"" id=""chkScelta' + convert(varchar,sistemi.idsistema) + '"">' end as chkScelta, " & _
        '" case sistemi.sistema when 'Sistema Formativo' then '<select disabled=""true"" name=""ddlScelta' + convert(varchar,sistemi.idsistema) + '"" id=""ddlScelta' + convert(varchar,sistemi.idsistema) + '"" onchange=""javascript: ControllaScelta(this.id,' + convert(varchar,sistemi.idsistema) + ')""><option selected value=""0"">Selezionare</option><option value=""1"">Ente</option><option value=""2"">Regione</option></select>' else '' end as ddlScelta, " & _
        '" '<input disabled=""true"" type=""text"" name=""txtCodiceEnteScelto' + convert(varchar,sistemi.idsistema) + '"" id=""txtCodiceEnteScelto' + convert(varchar,sistemi.idsistema) + '"">' as TxtCodEnte, " & _
        '" sistemi.idsistema as IdSistemaCheck " & _
        '" from sistemi " & _
        '" inner join entisistemi on entisistemi.idsistema=sistemi.idsistema  " & _
        '" where(idente = " & Session("idEnte") & ")" & _
        '" union " & _
        '" select 1 as ordina, " & _
        '" sistemi.idsistema,sistema,0 as identesistema, " & _
        '" '' as Username ,'' as datacreazionerecord,0 as acquisito,'0' as idente, " & _
        '" case sistemi.sistema when 'Sistema Formativo' then '<input type=""checkbox"" runat=""server"" value=""0"" onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',true)"" name=""chkScelta' + convert(varchar,sistemi.idsistema) + '"" id=""chkScelta' + convert(varchar,sistemi.idsistema) + '"">' else '<input type=""checkbox"" runat=""server"" value=""0"" onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',false)"" name""=chkScelta' + convert(varchar,sistemi.idsistema) + '"" id=""chkScelta' + convert(varchar,sistemi.idsistema) + '"">' end as chkScelta, " & _
        '" case sistemi.sistema when 'Sistema Formativo' then '<select disabled=""true"" name=""ddlScelta' + convert(varchar,sistemi.idsistema) + '"" id=""ddlScelta' + convert(varchar,sistemi.idsistema) + '"" onchange=""javascript: ControllaScelta(this.id,' + convert(varchar,sistemi.idsistema) + ')""><option selected value=""0"">Selezionare</option><option value=""1"">Ente</option><option value=""2"">Regione</option></select>' else '' end as ddlScelta, " & _
        '" '<input disabled=""true"" type=""text"" name=""txtCodiceEnteScelto' + convert(varchar,sistemi.idsistema) + '"" id=""txtCodiceEnteScelto' + convert(varchar,sistemi.idsistema) + '"">' as TxtCodEnte, " & _
        '" sistemi.idsistema as IdSistemaCheck " & _
        '" from sistemi " & _
        '" where sistemi.idsistema not in (select idsistema from entisistemi where idente=" & Session("idEnte") & ") " & _
        '" order by ordina,sistema "
        strsql = "Select 0 as ordina, "
        strsql = strsql & "sistemi.idsistema, "
        strsql = strsql & "sistemi.sistema, "
        strsql = strsql & "isnull(entisistemi.identesistema,-1) as identesistema, "
        strsql = strsql & "entisistemi.Username, "
        strsql = strsql & "entisistemi.datacreazionerecord, "
        strsql = strsql & "isnull((select a.identeSistema from entisistemi a INNER JOIN  EntiAcquisizioneServizi ON a.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema  where EntiAcquisizioneServizi.identesecondario='" & Session("idEnte") & "' and a.identesistema=entisistemi.identesistema and EntiAcquisizioneServizi.statorichiesta in(0,1)),0) as acquisito, "
        strsql = strsql & "idente as idente, "
        strsql = strsql & "case sistemi.sistema when 'Sistema Formativo' then '<input type=""checkbox"" runat=""server"" value=""1"" checked onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',true)"" id=""chkScelta' + convert(varchar,sistemi.idsistema) + '"">' else '<input type=""checkbox"" runat=""server"" value=""1"" checked onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',false)"" id=""chkScelta' + convert(varchar,sistemi.idsistema) + '"">' end as chkScelta, "
        strsql = strsql & "case sistemi.sistema when 'Sistema Formativo' then '<select disabled=""true"" name=""ddlScelta' + convert(varchar,sistemi.idsistema) + '"" id=""ddlScelta' + convert(varchar,sistemi.idsistema) + '"" onchange=""javascript: ControllaScelta(this.id,' + convert(varchar,sistemi.idsistema) + ')""><option selected value=""0"">Selezionare</option><option value=""1"">Ente</option><option value=""2"">Regione/Provincia Autonoma</option></select>' else '' end as ddlScelta, "
        strsql = strsql & "'<input disabled=""true"" type=""text"" name=""txtCodiceEnteScelto' + convert(varchar,sistemi.idsistema) + '"" id=""txtCodiceEnteScelto' + convert(varchar,sistemi.idsistema) + '"">' as TxtCodEnte, "
        strsql = strsql & "sistemi.idsistema as IdSistemaCheck "
        strsql = strsql & "from entiacquisizioneservizi u "
        strsql = strsql & "left join entisistemi on entisistemi.identesistema=u.identesistema "
        strsql = strsql & "left join sistemi  on case when u.identesistema is null then 2 else entisistemi.idsistema end = sistemi.idsistema "
        strsql = strsql & "where Sistemi.Nascosto=0 and (u.identesecondario ='" & Session("idEnte") & "' and u.statorichiesta in (0,1)) "
        strsql = strsql & "and isnull(sistemi.albo,'SCN') = 'SCN' " 'aggiunta il 20/07/2017
        strsql = strsql & "union "
        strsql = strsql & "select 1 as ordina, "
        strsql = strsql & "sistemi.idsistema, "
        strsql = strsql & "sistema, "
        strsql = strsql & "0 as identesistema, "
        strsql = strsql & "'' as Username, "
        strsql = strsql & "'' as datacreazionerecord, "
        strsql = strsql & "0 as acquisito, "
        strsql = strsql & "'0' as idente, "
        strsql = strsql & "case sistemi.sistema when 'Sistema Formativo' then '<input type=""checkbox"" runat=""server"" value=""0"" onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',true)"" name=""chkScelta' + convert(varchar,sistemi.idsistema) + '"" id=""chkScelta' + convert(varchar,sistemi.idsistema) + '"">' else '<input type=""checkbox"" runat=""server"" value=""0"" onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',false)"" name""=chkScelta' + convert(varchar,sistemi.idsistema) + '"" id=""chkScelta' + convert(varchar,sistemi.idsistema) + '"">' end as chkScelta, "
        strsql = strsql & "case sistemi.sistema when 'Sistema Formativo' then '<select disabled=""true"" name=""ddlScelta' + convert(varchar,sistemi.idsistema) + '"" id=""ddlScelta' + convert(varchar,sistemi.idsistema) + '"" onchange=""javascript: ControllaScelta(this.id,' + convert(varchar,sistemi.idsistema) + ')""><option selected value=""0"">Selezionare</option><option value=""1"">Ente</option><option value=""2"">Regione/Provincia Autonoma</option></select>' else '' end as ddlScelta, "
        strsql = strsql & "'<input disabled=""true"" type=""text"" name=""txtCodiceEnteScelto' + convert(varchar,sistemi.idsistema) + '"" id=""txtCodiceEnteScelto' + convert(varchar,sistemi.idsistema) + '"">' as TxtCodEnte, "
        strsql = strsql & "sistemi.idsistema as IdSistemaCheck "
        strsql = strsql & "from sistemi "
        strsql = strsql & "where Sistemi.Nascosto=0 "
        strsql = strsql & "and isnull(sistemi.albo,'SCN') = 'SCN' " 'aggiunta il 20/07/2017
        strsql = strsql & "and sistemi.idsistema not in "
        strsql = strsql & "(Select sistemi.idsistema "
        strsql = strsql & "from entiacquisizioneservizi u "
        strsql = strsql & "left join entisistemi on entisistemi.identesistema=u.identesistema "
        strsql = strsql & "left join sistemi  on case when u.identesistema is null then 2 else entisistemi.idsistema end = sistemi.idsistema "
        strsql = strsql & "where Sistemi.Nascosto=0 and (u.identesecondario ='" & Session("idEnte") & "') and u.statorichiesta in(0,1) ) "
        strsql = strsql & "order by ordina,sistema "
        dtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dgRisultatoRicerca.DataSource = dtsGenerico
        dgRisultatoRicerca.DataBind()
        controllaChek()
        AssegnaJavascript()
        'ControllaStatoCheck()
    End Sub

    Private Sub CaricaServiziVuota()
        strsql = "Select sistemi.idsistema,sistema,isnull(identesistema,0)as identesistema,idente,Username,datacreazionerecord,  " & _
                    " case sistemi.sistema when 'Sistema Formativo' then '<input type=""checkbox"" onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',true)"" id=chkScelta' + convert(varchar,sistemi.idsistema) + '>' else '<input type=""checkbox"" onclick=""javascript: SelezionaDeselezioneCampi(this.id,' + convert(varchar,sistemi.idsistema) + ',false)"" id=chkScelta' + convert(varchar,sistemi.idsistema) + '>' end as chkScelta, " & _
                    " case sistemi.sistema when 'Sistema Formativo' then '<select style=""visibility: hidden"" id=""ddlScelta' + convert(varchar,sistemi.idsistema) + '"" onchange=""javascript: ControllaScelta(this.id,' + convert(varchar,sistemi.idsistema) + ')""><option selected value=""0"">Selezionare</option><option value=""1"">Ente</option><option value=""2"">Regione/Provincia Autonoma</option></select>' else '' end as ddlScelta, " & _
                    " '<input style=""disabled: true; visibility: hidden"" type=""text"" id=""txtCodiceEnteScelto' + convert(varchar,sistemi.idsistema) + '"">' as TxtCodEnte " & _
                    " from sistemi " & _
                    " left join entisistemi on entisistemi.idsistema=sistemi.idsistema where idente=0 where Sistemi.Nascosto=0" & _
                    " and isnull(sistemi.albo,'SCN') = 'SCN' " 'aggiunta il 20/07/2017
        dtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dgRisultatoRicerca.DataSource = dtsGenerico
        dgRisultatoRicerca.DataBind()
    End Sub

    Private Sub controllaChek()
        'Generato da Alessandra Taballione il 01/04/04
        'Valorizzazione nella dataGrid del Check 

        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)

            If ((dgRisultatoRicerca.Items(item.ItemIndex).Cells(2).Text <> 0) And (dgRisultatoRicerca.Items(item.ItemIndex).Cells(2).Text <> -1)) Then
                check.Checked = True
                check.Enabled = False
                'devo andare a prendermi l'nz dell'ente di questo servizio
                'If dtrGenerico.HasRows = False Then
                'controllo se ci sono volontari in servizio 
                Dim dtrLeggiDati As SqlClient.SqlDataReader

                strsql = "select isnull(CodiceRegione,'') as CodiceRegione from enti " & _
                " inner join entisistemi on enti.idente=entisistemi.idente " & _
                " where entisistemi.identesistema='" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(2).Text & "'"

                dtrLeggiDati = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                dtrLeggiDati.Read()

                item.Cells(7).Text = "<input type=""text"" value=""" & dtrLeggiDati("CodiceRegione") & """ disabled=""true"" name=""txtCodiceEnteScelto" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & """ id=""txtCodiceEnteScelto" & item.Cells(0).Text & """>"

                If item.Cells(1).Text = "Sistema Formativo" Then
                    item.Cells(6).Text = "<select disabled=""true"" name=""ddlScelta" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & """ id=""ddlScelta" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & """ onchange=""javascript: ControllaScelta(this.id," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & ")""><option value=""0"">Selezionare</option><option selected value=""1"">Ente</option><option value=""2"">Regione/Provincia Autonoma</option></select>"
                End If

                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If
                '***

                'End If
            Else
                If (dgRisultatoRicerca.Items(item.ItemIndex).Cells(2).Text = -1) Then
                    check.Checked = True
                    check.Enabled = False

                    item.Cells(6).Text = "<select disabled=""true"" name=""ddlScelta" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & """ id=""ddlScelta" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & """ onchange=""javascript: ControllaScelta(this.id," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & ")""><option value=""0"">Selezionare</option><option value=""1"">Ente</option><option selected value=""2"">Regione/Provincia Autonoma</option></select>"
                Else
                    check.Checked = False
                End If
            End If
        Next
    End Sub

    Sub AssegnaJavascript()
        'Valorizzazione nella dataGrid del Check 
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items

            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)

            If (item.Cells(1).Text <> "Sistema Formativo") Then
                check.Attributes.Add("onclick", "SelezionaDeselezioneCampi(this.id," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text & ",false)")
            Else
                check.Attributes.Add("onclick", "SelezionaDeselezioneCampi(this.id," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text & ",true)")
            End If

        Next
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub cmdConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConferma.Click
        txtMessaggio.Text = ""
        ConfermaServizi()
    End Sub

    Sub ConfermaServizi()
        Dim null As String
        Dim item As DataGridItem
        Dim i As Integer
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
        null = "null"
        Try
            MyTransaction = CType(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), System.Data.SqlClient.SqlConnection).BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = MyTransaction

            For Each item In dgRisultatoRicerca.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                'controllo se è selezionato il check
                If check.Enabled = True Then
                    If check.Checked = True Then
                        'controllo se è presente la combo della scelta
                        If Not Request.Form("ddlScelta" & item.Cells(8).Text) Is Nothing Then
                            'se la combo è presente vado a controllare cos'è selezionato
                            Select Case Request.Form("ddlScelta" & item.Cells(8).Text)
                                Case ""

                                    'occorre selezionare la combo
                                    'selezionato 'Selezionare'
                                Case "0"
                                    txtMessaggio.Visible = True
                                    txtMessaggio.Text = "Scegliere la tipologia di acquisizione."
                                    'imgAlert.Visible = True
                                    'selezionato 'Ente'
                                Case "1" 'occorre inserire il codice regione
                                    'controllo il value della txt, se è vuota 
                                    'informo() 'utente della mancanza
                                    If Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text) = "" Then
                                        txtMessaggio.Visible = True
                                        txtMessaggio.Text = "Occorre inserire il codice dell'ente."
                                        'imgAlert.Visible = True
                                    Else 'codice regione presente procedo con l'inserimento

                                        '**************************************************************************************************************

                                        strsql = "Select idente, presentazioneprogetti from enti " & _
                                        " inner join statienti on statienti.idstatoente=enti.idstatoente " & _
                                        " where codiceregione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "'"
                                        myCommand.CommandText = strsql
                                        myCommand.ExecuteNonQuery()
                                        If Not dtrGenerico Is Nothing Then
                                            dtrGenerico.Close()
                                            dtrGenerico = Nothing
                                        End If
                                        dtrGenerico = myCommand.ExecuteReader()
                                        If dtrGenerico.HasRows = True Then
                                            dtrGenerico.Read()
                                            txtidente.Text = dtrGenerico("idente")
                                            'If dtrGenerico.HasRows = True Then
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            '++++++
                                            strsql = " Select enti.idclasseaccreditamento " & _
                                            " from classiaccreditamento " & _
                                            " inner join enti on enti.idclasseaccreditamentorichiesta=classiaccreditamento.idclasseaccreditamento " & _
                                            " inner join statienti on statienti.idstatoente=enti.idstatoente " & _
                                            " where entiinpartenariato = 1 " & _
                                            " and isnull(CLASSIACCREDITAMENTO.albo,'SCU') = 'SCN' " 'aggiunta il 20/07/2017
                                            strsql = strsql & " And codiceregione ='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "'" & _
                                            " and (statienti.presentazioneprogetti=1 or statienti.istruttoria=1)"
                                            myCommand.CommandText = strsql
                                            myCommand.ExecuteNonQuery()
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            dtrGenerico = myCommand.ExecuteReader()
                                            If dtrGenerico.HasRows = True Then
                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If

                                                '********************************************************************************

                                                strsql = "Select * from entisistemi " & _
                                                " inner join enti on enti.idente=entisistemi.idente " & _
                                                " where enti.CodiceRegione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "", "") & "' and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & " "
                                                myCommand.CommandText = strsql
                                                myCommand.ExecuteNonQuery()
                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If
                                                dtrGenerico = myCommand.ExecuteReader()
                                                If dtrGenerico.HasRows = False Then
                                                    'Inserimento della sede di Assegnazione
                                                    If Not dtrGenerico Is Nothing Then
                                                        dtrGenerico.Close()
                                                        dtrGenerico = Nothing
                                                    End If
                                                    strsql = "Insert into entisistemi (idente,idsistema,Username,datacreazioneRecord) Values " & _
                                                    "(" & Trim(txtidente.Text) & "," & item.Cells(0).Text & ",'" & Session("Utente") & "',getdate()) "
                                                    myCommand.CommandText = strsql
                                                    myCommand.ExecuteNonQuery()
                                                    '*****

                                                    If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then
                                                        strsql = "Insert into EntiAcquisizioneServizi " & _
                                                        "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                        "(" & Session("idEnte") & ",@@identity,'" & Session("Utente") & "',1,getdate(),getdate())"
                                                    Else
                                                        strsql = "Insert into EntiAcquisizioneServizi " & _
                                                        "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                        "(" & Session("idEnte") & ",@@identity,'" & Session("Utente") & "',0,getdate(),getdate())"
                                                    End If

                                                    myCommand.CommandText = strsql
                                                    myCommand.ExecuteNonQuery()
                                                Else
                                                    If Not dtrGenerico Is Nothing Then
                                                        dtrGenerico.Close()
                                                        dtrGenerico = Nothing
                                                    End If
                                                    'modificata da simona cordella il 07/03/2008 per nuovo accreditamento

                                                    strsql = "Select entisistemi.identesistema,EntiAcquisizioneServizi.* " & _
                                                    " from entisistemi " & _
                                                    " INNER Join " & _
                                                    " EntiAcquisizioneServizi ON entisistemi.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema " & _
                                                    " inner join enti on enti.idente=entisistemi.idente " & _
                                                    " where enti.CodiceRegione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "' and identeSecondario=" & Session("IdEnte") & " and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & " "

                                                    '********+
                                                    If Not dtrGenerico Is Nothing Then
                                                        dtrGenerico.Close()
                                                        dtrGenerico = Nothing
                                                    End If
                                                    myCommand.CommandText = strsql
                                                    myCommand.ExecuteNonQuery()
                                                    dtrGenerico = myCommand.ExecuteReader()
                                                    If dtrGenerico.HasRows = False Then
                                                        strsql = "select * from entisistemi " & _
                                                        " inner join enti on enti.idente=entisistemi.idente " & _
                                                        " where Codiceregione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "' and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & " "

                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        '***
                                                        myCommand.CommandText = strsql
                                                        myCommand.ExecuteNonQuery()
                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        dtrGenerico = myCommand.ExecuteReader()
                                                        If dtrGenerico.HasRows = True Then
                                                            'Inserimento della sede di Assegnazione
                                                            dtrGenerico.Read()
                                                            Dim idEntesistema As String
                                                            idEntesistema = dtrGenerico("Identesistema")
                                                            If Not dtrGenerico Is Nothing Then
                                                                dtrGenerico.Close()
                                                                dtrGenerico = Nothing
                                                            End If
                                                            If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then
                                                                strsql = "Insert into EntiAcquisizioneServizi " & _
                                                                "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                                "(" & Session("idEnte") & "," & idEntesistema & ",'" & Session("Utente") & "',1,getdate(),getdate())"
                                                            Else
                                                                strsql = "Insert into EntiAcquisizioneServizi " & _
                                                                "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                                "(" & Session("idEnte") & "," & idEntesistema & ",'" & Session("Utente") & "',0,getdate(),getdate())"
                                                            End If

                                                            If Not dtrGenerico Is Nothing Then
                                                                dtrGenerico.Close()
                                                                dtrGenerico = Nothing
                                                            End If
                                                            myCommand.CommandText = strsql
                                                            myCommand.ExecuteNonQuery()
                                                        End If
                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        '******
                                                    Else
                                                        'esiste già un accorso annullato per lo stesso ente e per lo stesso servizio
                                                        'modifica stato del campo statoservizio di EntiAcquisizioneServizi

                                                        strsql = "select * from entisistemi " & _
                                                                          " inner join enti on enti.idente=entisistemi.idente " & _
                                                                          " where Codiceregione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "' and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & " "

                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        '***
                                                        myCommand.CommandText = strsql
                                                        myCommand.ExecuteNonQuery()
                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        dtrGenerico = myCommand.ExecuteReader()
                                                        If dtrGenerico.HasRows = True Then
                                                            'Inserimento della sede di Assegnazione
                                                            dtrGenerico.Read()
                                                            Dim idEntesistema As String
                                                            idEntesistema = dtrGenerico("Identesistema")
                                                            If Not dtrGenerico Is Nothing Then
                                                                dtrGenerico.Close()
                                                                dtrGenerico = Nothing
                                                            End If
                                                            If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then
                                                                strsql = " Update EntiAcquisizioneServizi " & _
                                                                         " Set StatoRichiesta = 1, dataRichiesta = getdate() , Username='" & Session("Utente") & "' " & _
                                                                         " Where identeSecondario = " & Session("idEnte") & " " & _
                                                                         " and idEnteSistema= " & idEntesistema & " "

                                                            Else
                                                                strsql = " Update EntiAcquisizioneServizi " & _
                                                                         " Set StatoRichiesta = 0, dataRichiesta = getdate() , Username='" & Session("Utente") & "' " & _
                                                                         " Where identeSecondario =" & Session("idEnte") & " " & _
                                                                         " and idEnteSistema= " & idEntesistema & " "

                                                            End If

                                                            If Not dtrGenerico Is Nothing Then
                                                                dtrGenerico.Close()
                                                                dtrGenerico = Nothing
                                                            End If
                                                            myCommand.CommandText = strsql
                                                            myCommand.ExecuteNonQuery()
                                                        End If
                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        '******

                                                    End If
                                                End If

                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If

                                                'insertServiziEnteRisorsa()

                                                '********************************************************************************

                                            Else
                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If
                                                'txtMessaggio.ForeColor = Color.Red
                                                txtMessaggio.Text = "Attenzione l'Ente selezionato non può fornire il servizio desiderato. "
                                            End If
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            '++++++
                                            'Else
                                            '    If Not dtrGenerico Is Nothing Then
                                            '        dtrGenerico.Close()
                                            '        dtrGenerico = Nothing
                                            '    End If
                                            '    txtMessaggio.ForeColor = Color.Red
                                            '    txtMessaggio.Text = "Attenzione l'Ente selezionato non può accedere alla selezione dei Servizi. "
                                            'End If
                                        Else
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            'txtMessaggio.ForeColor = Color.Red
                                            txtMessaggio.Text = "Non è presente in archivio nessun Ente con il codice inserito. Verificare l'esatta immissione dei dati."
                                        End If

                                    End If

                                    '**************************************************************************************************************
                                    'selezionato 'Regione'
                                Case "2" ' non occorre inserire il codice regione
                                    'qui ci va il codice necessario all'inserimento con id entesistema a null

                                    '********************************************************************************

                                    'strsql = "Select idente, presentazioneprogetti from enti " & _
                                    '" inner join statienti on statienti.idstatoente=enti.idstatoente " & _
                                    '" where codiceregione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "'"
                                    'If Not dtrGenerico Is Nothing Then
                                    '    dtrGenerico.Close()
                                    '    dtrGenerico = Nothing
                                    'End If
                                    'dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    'If dtrGenerico.HasRows = True Then
                                    '    dtrGenerico.Read()
                                    '    txtidente.Text = dtrGenerico("idente")
                                    '    'If dtrGenerico.HasRows = True Then
                                    '    If Not dtrGenerico Is Nothing Then
                                    '        dtrGenerico.Close()
                                    '        dtrGenerico = Nothing
                                    '    End If
                                    '    '++++++
                                    '    strsql = " Select enti.idclasseaccreditamento " & _
                                    '    " from classiaccreditamento " & _
                                    '    " inner join enti on enti.idclasseaccreditamentorichiesta=classiaccreditamento.idclasseaccreditamento " & _
                                    '    " inner join statienti on statienti.idstatoente=enti.idstatoente " & _
                                    '    " where entiinpartenariato = 1 "
                                    '    strsql = strsql & " And codiceregione ='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "'" & _
                                    '    " and (statienti.presentazioneprogetti=1 or statienti.istruttoria=1)"
                                    '    If Not dtrGenerico Is Nothing Then
                                    '        dtrGenerico.Close()
                                    '        dtrGenerico = Nothing
                                    '    End If
                                    '    dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    '    If dtrGenerico.HasRows = True Then
                                    '        If Not dtrGenerico Is Nothing Then
                                    '            dtrGenerico.Close()
                                    '            dtrGenerico = Nothing
                                    '        End If

                                    '********************************************************************************


                                    strsql = "Select * from entisistemi " & _
                                        " inner join enti on enti.idente=entisistemi.idente " & _
                                        " where entisistemi.idente =" & Session("IdEnte") & "  and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & " "

                                    'enti.CodiceRegione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "", "") & "'

                                    'where(es.idente = 17326 And esas.identesistema Is null And es.idsistema = 2)
                                    myCommand.CommandText = strsql
                                    myCommand.ExecuteNonQuery()
                                    If Not dtrGenerico Is Nothing Then
                                        dtrGenerico.Close()
                                        dtrGenerico = Nothing
                                    End If
                                    dtrGenerico = myCommand.ExecuteReader()
                                    If dtrGenerico.HasRows = False Then
                                        'Inserimento della sede di Assegnazione
                                        If Not dtrGenerico Is Nothing Then
                                            dtrGenerico.Close()
                                            dtrGenerico = Nothing
                                        End If
                                        strsql = "Insert into entisistemi (idente,idsistema,Username,datacreazioneRecord) Values " & _
                                        "(" & Trim(Session("IdEnte")) & "," & item.Cells(0).Text & ",'" & Session("Utente") & "',getdate()) "
                                        myCommand.CommandText = strsql
                                        myCommand.ExecuteNonQuery()
                                        '*****

                                        '****
                                        If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then
                                            strsql = "Insert into EntiAcquisizioneServizi " & _
                                            "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                            "(" & Session("idEnte") & ",null,'" & Session("Utente") & "',1,getdate(),getdate())"
                                        Else
                                            strsql = "Insert into EntiAcquisizioneServizi " & _
                                            "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                            "(" & Session("idEnte") & ",null,'" & Session("Utente") & "',0,getdate(),getdate())"
                                        End If

                                        myCommand.CommandText = strsql
                                        myCommand.ExecuteNonQuery()
                                    Else
                                        If Not dtrGenerico Is Nothing Then
                                            dtrGenerico.Close()
                                            dtrGenerico = Nothing
                                        End If
                                        'strsql = "Select entisistemi.identesistema,EntiAcquisizioneServizi.* " & _
                                        '" from entisistemi " & _
                                        '" INNER Join " & _
                                        '" EntiAcquisizioneServizi ON entisistemi.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema " & _
                                        '" inner join enti on enti.idente=entisistemi.idente " & _
                                        '" where enti.CodiceRegione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "' and identeSecondario=" & Session("IdEnte") & " and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & ""


                                        strsql = "Select entisistemi.identesistema,EntiAcquisizioneServizi.* " & _
                                                " From entisistemi " & _
                                                " INNER Join EntiAcquisizioneServizi  " & _
                                                "         ON entisistemi.IDEnte = EntiAcquisizioneServizi.idEnteSecondario " & _
                                                " Where identeSecondario=" & Session("IdEnte") & " and " & _
                                                " idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & " " & _
                                                " and EntiAcquisizioneServizi.identesistema is null"


                                        '********+
                                        If Not dtrGenerico Is Nothing Then
                                            dtrGenerico.Close()
                                            dtrGenerico = Nothing
                                        End If
                                        myCommand.CommandText = strsql
                                        myCommand.ExecuteNonQuery()
                                        dtrGenerico = myCommand.ExecuteReader()
                                        If dtrGenerico.HasRows = False Then
                                            strsql = "select * from entisistemi " & _
                                            " inner join enti on enti.idente=entisistemi.idente " & _
                                            " where Codiceregione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "' and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & ""
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            '***
                                            myCommand.CommandText = strsql
                                            myCommand.ExecuteNonQuery()
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            dtrGenerico = myCommand.ExecuteReader()
                                            If dtrGenerico.HasRows = True Then
                                                'Inserimento della sede di Assegnazione
                                                dtrGenerico.Read()
                                                Dim idEntesistema As String
                                                idEntesistema = dtrGenerico("Identesistema")
                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If

                                                If Session("TipoUtente") = "R" Or Session("TipoUtente") = "U" Then
                                                    strsql = "Insert into EntiAcquisizioneServizi " & _
                                                    "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                    "(" & Session("idEnte") & ",null,'" & Session("Utente") & "',1,getdate(),getdate())"
                                                Else
                                                    strsql = "Insert into EntiAcquisizioneServizi " & _
                                                    "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                    "(" & Session("idEnte") & ",null,'" & Session("Utente") & "',0,getdate(),getdate())"
                                                End If

                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If
                                                myCommand.CommandText = strsql
                                                myCommand.ExecuteNonQuery()
                                            End If
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            '*******
                                        Else
                                            '*************** 
                                            strsql = "select * from entisistemi " & _
                                                  " inner join enti on enti.idente=entisistemi.idente " & _
                                                  " where entisistemi.idente =" & Session("idEnte") & " and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & ""
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            '***
                                            myCommand.CommandText = strsql
                                            myCommand.ExecuteNonQuery()
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            dtrGenerico = myCommand.ExecuteReader()
                                            If dtrGenerico.HasRows = True Then
                                                'Inserimento della sede di Assegnazione
                                                dtrGenerico.Read()
                                                Dim idEntesistema As String
                                                idEntesistema = dtrGenerico("Identesistema")
                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If

                                                If Session("TipoUtente") = "R" Or Session("TipoUtente") = "U" Then


                                                    strsql = " UPDATE  EntiAcquisizioneServizi " & _
                                                             " SET StatoRichiesta = 1 , Username = '" & Session("Utente") & "', " & _
                                                             " dataRichiesta = getdate()" & _
                                                             " WHERE identeSecondario = " & Session("idEnte") & " AND idEnteSistema IS NULL"

                                                Else
                                                    strsql = " UPDATE  EntiAcquisizioneServizi " & _
                                                             " SET StatoRichiesta = 0 , Username = '" & Session("Utente") & "', " & _
                                                             " dataRichiesta = getdate()" & _
                                                             " WHERE identeSecondario = " & Session("idEnte") & " AND IdEnteSistema IS NULL"


                                                End If

                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If
                                                myCommand.CommandText = strsql
                                                myCommand.ExecuteNonQuery()
                                            End If
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            '*******




                                        End If
                                    End If

                                    If Not dtrGenerico Is Nothing Then
                                        dtrGenerico.Close()
                                        dtrGenerico = Nothing
                                    End If

                                    'insertServiziEnteRisorsa()

                                    '********************************************************************************

                                    '    Else
                                    '        If Not dtrGenerico Is Nothing Then
                                    '            dtrGenerico.Close()
                                    '            dtrGenerico = Nothing
                                    '        End If
                                    '        txtMessaggio.ForeColor = Color.Red
                                    '        txtMessaggio.Text = "Attenzione l'Ente selezionato non può accedere alla selezione dei Servizi. "
                                    '    End If
                                    '    If Not dtrGenerico Is Nothing Then
                                    '        dtrGenerico.Close()
                                    '        dtrGenerico = Nothing
                                    '    End If
                                    '    '++++++
                                    '    'Else
                                    '    '    If Not dtrGenerico Is Nothing Then
                                    '    '        dtrGenerico.Close()
                                    '    '        dtrGenerico = Nothing
                                    '    '    End If
                                    '    '    txtMessaggio.ForeColor = Color.Red
                                    '    '    txtMessaggio.Text = "Attenzione l'Ente selezionato non può accedere alla selezione dei Servizi. "
                                    '    'End If
                                    'Else
                                    '    If Not dtrGenerico Is Nothing Then
                                    '        dtrGenerico.Close()
                                    '        dtrGenerico = Nothing
                                    '    End If
                                    '    txtMessaggio.ForeColor = Color.Red
                                    '    txtMessaggio.Text = "Non è presente in archivio nessun Ente con il codice " & txtcodiceente.Text & ". Verificare l'esatta immissione dei dati."
                                    'End If

                                    '********************************************************************************

                            End Select

                        Else 'la combo non è presente vado a controllare se è stato inserito il codice regione della txt
                            'controllo se è spuntato il check e se inserito il codice ente
                            If Not Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text) Is Nothing Then
                                If Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text) = "" Then
                                    txtMessaggio.Visible = True
                                    txtMessaggio.Text = "Occorre inserire il codice dell'ente."
                                    'imgAlert.Visible = True
                                Else 'codice regione presente procedo con l'inserimento


                                    '***********************************************************************************


                                    'If txtcodiceente.Visible = True Then
                                    'If txtcodiceente.Text = "" Then
                                    '    txtMessaggio.ForeColor = Color.Red
                                    '    txtMessaggio.Text = "E' necessario Inserire il Codice dell' Ente."
                                    '    Exit Sub
                                    'End If
                                    'ConfermaServizi()
                                    strsql = "Select idente, presentazioneprogetti from enti " & _
                                    " inner join statienti on statienti.idstatoente=enti.idstatoente " & _
                                    " where codiceregione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "'"
                                    myCommand.CommandText = strsql
                                    myCommand.ExecuteNonQuery()
                                    If Not dtrGenerico Is Nothing Then
                                        dtrGenerico.Close()
                                        dtrGenerico = Nothing
                                    End If
                                    dtrGenerico = myCommand.ExecuteReader()
                                    If dtrGenerico.HasRows = True Then
                                        dtrGenerico.Read()
                                        txtidente.Text = dtrGenerico("idente")
                                        'If dtrGenerico.HasRows = True Then
                                        If Not dtrGenerico Is Nothing Then
                                            dtrGenerico.Close()
                                            dtrGenerico = Nothing
                                        End If
                                        '++++++
                                        strsql = " Select enti.idclasseaccreditamento " & _
                                        " from classiaccreditamento " & _
                                        " inner join enti on enti.idclasseaccreditamentorichiesta=classiaccreditamento.idclasseaccreditamento " & _
                                        " inner join statienti on statienti.idstatoente=enti.idstatoente " & _
                                        " where entiinpartenariato = 1 " & _
                                        " and isnull(CLASSIACCREDITAMENTO.albo,'SCU') = 'SCN' " 'aggiunta il 20/07/2017
                                        strsql = strsql & " And codiceregione ='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "'" & _
                                        " and (statienti.presentazioneprogetti=1 or statienti.istruttoria=1)"
                                        myCommand.CommandText = strsql
                                        myCommand.ExecuteNonQuery()
                                        If Not dtrGenerico Is Nothing Then
                                            dtrGenerico.Close()
                                            dtrGenerico = Nothing
                                        End If
                                        dtrGenerico = myCommand.ExecuteReader()
                                        If dtrGenerico.HasRows = True Then
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If

                                            '********************************************************************************


                                            strsql = "Select * from entisistemi " & _
                                            " inner join enti on enti.idente=entisistemi.idente " & _
                                            " where enti.CodiceRegione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "", "") & "' and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & " "
                                            myCommand.CommandText = strsql
                                            myCommand.ExecuteNonQuery()
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            dtrGenerico = myCommand.ExecuteReader()
                                            If dtrGenerico.HasRows = False Then
                                                'Inserimento della sede di Assegnazione
                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If
                                                strsql = "Insert into entisistemi (idente,idsistema,Username,datacreazioneRecord) Values " & _
                                                "(" & Trim(txtidente.Text) & "," & item.Cells(0).Text & ",'" & Session("Utente") & "',getdate()) "
                                                myCommand.CommandText = strsql
                                                myCommand.ExecuteNonQuery()
                                                '*****


                                                If Session("TipoUtente") = "R" Or Session("TipoUtente") = "U" Then '****
                                                    strsql = "Insert into EntiAcquisizioneServizi " & _
                                                    "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                    "(" & Session("idEnte") & ",@@identity,'" & Session("Utente") & "',1,getdate(),getdate())"
                                                Else
                                                    strsql = "Insert into EntiAcquisizioneServizi " & _
                                                    "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                    "(" & Session("idEnte") & ",@@identity,'" & Session("Utente") & "',0,getdate(),getdate())"
                                                End If

                                                myCommand.CommandText = strsql
                                                myCommand.ExecuteNonQuery()
                                            Else
                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If
                                                strsql = "Select entisistemi.identesistema,EntiAcquisizioneServizi.* " & _
                                                " from entisistemi " & _
                                                " INNER Join " & _
                                                " EntiAcquisizioneServizi ON entisistemi.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema " & _
                                                " inner join enti on enti.idente=entisistemi.idente " & _
                                                " where enti.CodiceRegione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "' and identeSecondario=" & Session("IdEnte") & " and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & ""
                                                '********+
                                                If Not dtrGenerico Is Nothing Then
                                                    dtrGenerico.Close()
                                                    dtrGenerico = Nothing
                                                End If
                                                myCommand.CommandText = strsql
                                                myCommand.ExecuteNonQuery()
                                                dtrGenerico = myCommand.ExecuteReader()
                                                If dtrGenerico.HasRows = False Then
                                                    strsql = "select * from entisistemi " & _
                                                    " inner join enti on enti.idente=entisistemi.idente " & _
                                                    " where Codiceregione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "' and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & ""
                                                    If Not dtrGenerico Is Nothing Then
                                                        dtrGenerico.Close()
                                                        dtrGenerico = Nothing
                                                    End If
                                                    '***
                                                    myCommand.CommandText = strsql
                                                    myCommand.ExecuteNonQuery()
                                                    If Not dtrGenerico Is Nothing Then
                                                        dtrGenerico.Close()
                                                        dtrGenerico = Nothing
                                                    End If
                                                    dtrGenerico = myCommand.ExecuteReader()
                                                    If dtrGenerico.HasRows = True Then
                                                        'Inserimento della sede di Assegnazione
                                                        dtrGenerico.Read()
                                                        Dim idEntesistema As String
                                                        idEntesistema = dtrGenerico("Identesistema")
                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        If Session("TipoUtente") = "R" Or Session("TipoUtente") = "U" Then
                                                            strsql = "Insert into EntiAcquisizioneServizi " & _
                                                            "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                            "(" & Session("idEnte") & "," & idEntesistema & ",'" & Session("Utente") & "',1,getdate(),getdate())"
                                                        Else
                                                            strsql = "Insert into EntiAcquisizioneServizi " & _
                                                            "(identeSecondario,idEnteSistema,Username,StatoRichiesta,DataCreazioneRecord,dataRichiesta) values " & _
                                                            "(" & Session("idEnte") & "," & idEntesistema & ",'" & Session("Utente") & "',0,getdate(),getdate())"
                                                        End If

                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        myCommand.CommandText = strsql
                                                        myCommand.ExecuteNonQuery()
                                                    End If
                                                    If Not dtrGenerico Is Nothing Then
                                                        dtrGenerico.Close()
                                                        dtrGenerico = Nothing
                                                    End If
                                                    '*******
                                                Else
                                                    ' se esiste sesso servizio e stesso ente faccio update
                                                    strsql = "select * from entisistemi " & _
                                                    " inner join enti on enti.idente=entisistemi.idente " & _
                                                    " where Codiceregione='" & Replace(Request.Form("txtCodiceEnteScelto" & item.Cells(8).Text), "'", "''") & "' and idsistema=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & ""
                                                    If Not dtrGenerico Is Nothing Then
                                                        dtrGenerico.Close()
                                                        dtrGenerico = Nothing
                                                    End If
                                                    '***
                                                    myCommand.CommandText = strsql
                                                    myCommand.ExecuteNonQuery()
                                                    If Not dtrGenerico Is Nothing Then
                                                        dtrGenerico.Close()
                                                        dtrGenerico = Nothing
                                                    End If
                                                    dtrGenerico = myCommand.ExecuteReader()
                                                    If dtrGenerico.HasRows = True Then
                                                        'Inserimento della sede di Assegnazione
                                                        dtrGenerico.Read()
                                                        Dim idEntesistema As String
                                                        idEntesistema = dtrGenerico("Identesistema")
                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        If Session("TipoUtente") = "R" Or Session("TipoUtente") = "U" Then
                                                            strsql = " UPDATE  EntiAcquisizioneServizi " & _
                                                                     " SET StatoRichiesta = 1 , Username = '" & Session("Utente") & "', " & _
                                                                     " dataRichiesta = getdate()" & _
                                                                     " WHERE identeSecondario = " & Session("idEnte") & " AND idEnteSistema=" & idEntesistema & ""

                                                        Else
                                                            strsql = " UPDATE  EntiAcquisizioneServizi " & _
                                                                     " SET StatoRichiesta = 0 , Username = '" & Session("Utente") & "', " & _
                                                                     " dataRichiesta = getdate()" & _
                                                                     " WHERE identeSecondario = " & Session("idEnte") & " AND idEnteSistema=" & idEntesistema & ""
                                                        End If

                                                        If Not dtrGenerico Is Nothing Then
                                                            dtrGenerico.Close()
                                                            dtrGenerico = Nothing
                                                        End If
                                                        myCommand.CommandText = strsql
                                                        myCommand.ExecuteNonQuery()
                                                    End If
                                                    If Not dtrGenerico Is Nothing Then
                                                        dtrGenerico.Close()
                                                        dtrGenerico = Nothing
                                                    End If
                                                    '*******

                                                End If
                                            End If

                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If

                                            'insertServiziEnteRisorsa()

                                            '********************************************************************************

                                        Else
                                            If Not dtrGenerico Is Nothing Then
                                                dtrGenerico.Close()
                                                dtrGenerico = Nothing
                                            End If
                                            'txtMessaggio.ForeColor = Color.Red
                                            txtMessaggio.Text = "Attenzione l'Ente selezionato non può fornire il servizio desiderato. "
                                        End If
                                        If Not dtrGenerico Is Nothing Then
                                            dtrGenerico.Close()
                                            dtrGenerico = Nothing
                                        End If
                                        '++++++
                                        'Else
                                        '    If Not dtrGenerico Is Nothing Then
                                        '        dtrGenerico.Close()
                                        '        dtrGenerico = Nothing
                                        '    End If
                                        '    txtMessaggio.ForeColor = Color.Red
                                        '    txtMessaggio.Text = "Attenzione l'Ente selezionato non può accedere alla selezione dei Servizi. "
                                        'End If
                                    Else
                                        If Not dtrGenerico Is Nothing Then
                                            dtrGenerico.Close()
                                            dtrGenerico = Nothing
                                        End If
                                        'txtMessaggio.ForeColor = Color.Red
                                        txtMessaggio.Text = "Non è presente in archivio nessun Ente con il codice inserito. Verificare l'esatta immissione dei dati."
                                    End If
                                    'Else
                                    '    If Not dtrGenerico Is Nothing Then
                                    '        dtrGenerico.Close()
                                    '        dtrGenerico = Nothing
                                    '    End If

                                    '    '********************************************************************************

                                    'insertServiziEnte()

                                    '    '********************************************************************************
                                    'End If


                                    '***********************************************************************************


                                End If
                            End If

                        End If
                    End If
                End If
            Next
            MyTransaction.Commit()
        Catch e As Exception
            'imgAlert.Visible = True
            'imgAlert.ImageUrl = "images/alert3.gif"
            'txtMessaggio.ForeColor = Color.Red
            txtMessaggio.Text = e.Message.ToString
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
        Finally
            ' CompletaInserimento()
            If txtMessaggio.Text = "" Then
                'imgAlert.Visible = True
                'imgAlert.ImageUrl = "images/conf1.jpg"
                'txtMessaggio.ForeColor = Color.Navy
                txtMessaggio.Text = "L'Operazione è stata eseguita con successo."
                cmdConferma.Visible = False
                CaricaServizi()
                'bloccaCheck()
            End If
        End Try
        MyTransaction.Dispose()

    End Sub

End Class