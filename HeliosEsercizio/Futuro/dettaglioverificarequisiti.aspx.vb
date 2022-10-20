Imports System.IO
Public Class dettaglioverificarequisiti
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
   

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strSql As String
    Dim ContaChk As String
    Dim strDataConfermaEsito As String
    Dim size As System.Web.UI.WebControls.FontUnit

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            txtx.Value = "0"
        End If
        Try
            txtx.Value = "0"
            Session("IdVerifica") = Request.QueryString("IdVerifica")
            Session("IDVerificheAssociate") = Request.QueryString("IDVerificheAssociate")
            Session("IDEnteSedeAttuazione") = Request.QueryString("IDEnteSedeAttuazione")
            Session("IdEntePro") = Request.QueryString("IDEnte")
            Session("StatoRequisiti") = Request.QueryString("StatoRequisiti")
            CaricaIntestazione()

            strDataConfermaEsito = ControllaRequisiti(Session("IDVerificheAssociate"))

            CreaTabellaparametri(Session("IdAttività"), panelAttuazionedelProgetto, 1, strDataConfermaEsito, "panelAttuazionedelProgetto") 'tblAttuazioneProgetto
            CreaTabellaparametri(Session("IdAttività"), panelFormazionedeiVolontari, 2, strDataConfermaEsito, "panelFormazionedeiVolontari") 'tblFormazioneVolontari
            CreaTabellaparametri(Session("IdAttività"), panelOperativitadeiVolontari, 3, strDataConfermaEsito, "panelOperativitadeiVolontari") 'tblOperativitaVolontari
            CreaTabellaparametri(Session("IdAttività"), panelRisorseFinanziarieErogate, 4, strDataConfermaEsito, "panelRisorseFinanziarieErogate") 'tblRisorseFinanziarieErogate
            CaricaRequisiti(Session("IDVerificheAssociate"), strDataConfermaEsito)

            StatoVerifica(Request.QueryString("StatoVerifica"))
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Sub CreaTabellaparametri(ByVal IdAttività As Integer, ByVal contenitore As Panel, ByVal Parametro As Integer, ByVal DataConfermaEsiti As String, ByVal sezione As String)
        'Generato da Alessandra Taballione il 09.07.2004
        'Creazione di una taballa 
        'modificata da simona cordella il 02/03/2012
        Dim i As Integer
        Dim j As Integer
        Dim r As TableRow
        Dim c As TableCell
        Dim myRow As DataRow
        Dim Tab As DataTable
        Dim coll As Collection
        Dim XX As Integer
        Dim intIdVersioneVerifiche As Integer
        'aggiunto da alessandra taballione il 21.12.2004
        Dim txt As TextBox = New TextBox

        Try
            txt.Text = "Pippo"
            ' txt.ForeColor = Color.Red


            strSql = " SELECT  isnull(bando.IDVersioneVerifiche,0) as IDVersioneVerifiche "
            strSql &= " FROM attività "
            strSql &= " INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività "
            strSql &= " INNER JOIN bando ON BandiAttività.IdBando = bando.IDBando"
            strSql &= " WHERE attività.IdAttività =  " & IdAttività

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                intIdVersioneVerifiche = dtrgenerico("IDVersioneVerifiche")
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            strSql = " SELECT isnull(TVerificheRequisito.rationale,'') as rationale ,TVerificheRequisito.Ordine,TVerificheRequisito.descrizione, TVerificheRequisito.idrequisito "
            strSql = strSql & " FROM TVerificheRequisito "
            strSql = strSql & " where idtiporequisito=" & Parametro & " AND idversioneverifiche = " & intIdVersioneVerifiche & " "

            If intIdVersioneVerifiche = 1 Then
                strSql = strSql & " and IdRegCompetenza = " & Session("IdRegCompetenza") & " "
            Else
                strSql = strSql & " and IdRegCompetenza =  22 "
            End If

            If DataConfermaEsiti <> "" Then
                strSql = strSql & " and (Abilitato = 0 or DataDisabilitazione >='" & DataConfermaEsiti & "')"
                strSql = strSql & " and (DataInserimento <= '" & DataConfermaEsiti & "') "
            Else
                strSql = strSql & " and abilitato = 0 "
            End If
            strSql = strSql & " order by ordine"




            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Tab = ClsServer.CreaDataTable(strSql, False, Session("conn"))
            ' Creo Righe e celle
            Dim a As Integer = 1
            For Each myRow In Tab.Rows
                a = a + 1
                Dim img As System.Web.UI.WebControls.Image = New System.Web.UI.WebControls.Image
                img.ImageUrl = "images\i5.gif"

                img.ToolTip = myRow.Item("rationale")
         
                Dim panelInterno As Panel = New Panel
                panelInterno.CssClass = "panel"
                Dim lblPanelInterno As Label = New Label
                lblPanelInterno.CssClass = "labelDati"

                panelInterno.ID = sezione + a.ToString + "1"
                panelInterno.Style.Add("width", "60%")
          
                Dim param = myRow.Item("descrizione")
                Dim ordine = myRow.Item("Ordine")
                txtx.Value = CInt(txtx.Value) + 1
                lblPanelInterno.Text = "(" & ordine & ")"
                lblPanelInterno.Text = lblPanelInterno.Text & param & " [Scelta Singola] <br/> "

                panelInterno.Controls.Add(lblPanelInterno)

                contenitore.Controls.Add(panelInterno)

                panelInterno = New Panel
                panelInterno.CssClass = "panel"
                panelInterno.Style.Add("width", "30%")
                panelInterno.ID = sezione + a.ToString + "2"

                contenitore.Controls.Add(panelInterno)
                Dim numeroGruppo As Byte = 0

                CreaTabellaVociParametri(panelInterno, myRow.Item("idrequisito")) ',
                AggiungiRigaVuota(contenitore)
            Next

            txtnome.Value = ContaChk.ToString()
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Sub AggiungiRigaVuota(ByRef contenitore As Panel)
        Dim panelRow As Panel = New Panel()
        panelRow.CssClass = "panel"
        Dim label As Label = New Label()

        label.Text = "&nbsp;&nbsp;&nbsp;&nbsp;    <br/>"
        panelRow.Controls.Add(label)
        contenitore.Controls.Add(panelRow)

    End Sub
    Private Function CreaDivRigaVuota() As Panel
        Dim panelRigaVuota As Panel = New Panel
        panelRigaVuota.CssClass = "RigaVuota"
        Return panelRigaVuota
    End Function
    Private Function CreaTabellaVociParametri(ByRef contenitore As Panel, ByVal Parametro As Integer) As Table
        Dim row As TableRow
        Dim cell As TableCell
        Dim newTbl As Table = New Table
        Dim myRow As DataRow
        Dim Tab As DataTable


        strSql = "SELECT  TVerificheEsitoTemplate.IDEsito, TVerificheEsitoTemplate.Esito "
        strSql = strSql & " FROM TVerificheEsitoTemplate  "
        strSql = strSql & " INNER JOIN TVerificheRequisitoEsiti ON TVerificheEsitoTemplate.IDEsito = TVerificheRequisitoEsiti.IDEsito"
        strSql = strSql & " WHERE TVerificheRequisitoEsiti.IDRequisito = " & Parametro & ""

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Tab = ClsServer.CreaDataTable(strSql, False, Session("conn"))
        'Formattazione Tabella
        'newTbl.Font.Bold = True
        'newTbl.Font.Name = "verdana"
        'newTbl.Font.Size = size.XXSmall
        'newTbl.GridLines = GridLines.Horizontal
        'newTbl.BorderStyle = BorderStyle.NotSet
        ''newTbl.BorderColor = Color.FromName("#c9dfff")
        ''newTbl.ForeColor = Color.Navy
        'newTbl.Width.Pixel(400)
        'Dim tbl As String
        'newTbl.ID = "T" & ContaChk
        'tbl = "T" & ContaChk

        For Each myRow In Tab.Rows
            'Creazione della righe

            row = New TableRow
            Dim Check As CheckBox = New CheckBox
            Check.Checked = False
            ContaChk = ContaChk + 1
            Check.ID = ContaChk & "cs" & "Z" & myRow.Item("idEsito") '& "Y" & Parametro & "X" & myRow.Item("Esito")

            Check.Font.Name = "Verdana"
            Check.Font.Size = size.XXSmall
            'Check.ForeColor = Color.Navy
            Check.Width = New Unit(400)
            Check.Font.Bold = True
            Check.TextAlign = TextAlign.Right
            Check.AutoPostBack = False
            Check.CssClass = "textbox"
            Check.Attributes.Add("OnClick", "Checkall('" & contenitore.ID & "','" & Check.ClientID & "'," & "this.id" & ")")
            Check.Text = myRow.Item("esito")

            contenitore.Controls.Add(Check)


            Dim lbl As Label = New Label
            lbl.ID = "l" & Check.ID
            'lbl.ForeColor = Color.FromName("#c9dfff")
            lbl.Text = Parametro
            lbl.Visible = False


            contenitore.Controls.Add(lbl)


            Dim lbl1 As Label = New Label
            lbl1.ID = "lp" & Check.ID
            'lbl1.ForeColor = Color.FromName("#c9dfff")
            lbl1.Text = myRow.Item("idEsito")
            lbl1.Visible = False

            contenitore.Controls.Add(lbl1)
        Next
        CreaTabellaVociParametri = newTbl
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Private Function CreaTabellaVociParametriOLD(ByVal parametro As Integer) As Table
        Dim row As TableRow
        Dim cell As TableCell
        Dim newTbl As Table = New Table
        Dim myRow As DataRow
        Dim Tab As DataTable

        Try
            strSql = "SELECT  TVerificheEsitoTemplate.IDEsito, TVerificheEsitoTemplate.Esito "
            strSql = strSql & " FROM TVerificheEsitoTemplate  "
            strSql = strSql & " INNER JOIN TVerificheRequisitoEsiti ON TVerificheEsitoTemplate.IDEsito = TVerificheRequisitoEsiti.IDEsito"
            strSql = strSql & " WHERE TVerificheRequisitoEsiti.IDRequisito = " & parametro & ""

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Tab = ClsServer.CreaDataTable(strSql, False, Session("conn"))
            'Formattazione Tabella
            newTbl.Font.Bold = True
            newTbl.Font.Name = "verdana"
            newTbl.Font.Size = size.XXSmall
            newTbl.GridLines = GridLines.Horizontal
            newTbl.BorderStyle = BorderStyle.NotSet
            '  newTbl.BorderColor = Color.FromName("#c9dfff")
            '   newTbl.ForeColor = Color.Navy
            newTbl.Width.Pixel(400)
            Dim tbl As String
            newTbl.ID = "T" & ContaChk
            tbl = "T" & ContaChk

            For Each myRow In Tab.Rows
                'Creazione della righe
                row = New TableRow

                Dim Check As CheckBox = New CheckBox

                Check.Checked = False
                ContaChk = ContaChk + 1

                Check.ID = ContaChk & "cs" '& "Z" & myRow.Item("IDEsito") & "Y" & parametro & "X" & myRow.Item("Esito")  '& "W" & myRow.Item("AssociaNote")

                Check.Font.Name = "Verdana"
                Check.Font.Size = size.XXSmall
                '   Check.ForeColor = Color.Navy
                Check.Width = New Unit(400)
                Check.Font.Bold = True
                Check.TextAlign = TextAlign.Right
                Check.AutoPostBack = False
                '*****
                Check.Attributes.Add("onclick", "Checkall('" & tbl & "','" & Check.ClientID & "')")
                '," & myRow.Item("punteggio") & "','" & myRow.Item("riferimentovoce") & "'," & "this.id" & ")")
                '****+
                Check.Text = myRow.Item("Esito")
                ' Check.ForeColor = Color.FromName("#c9dfff")
                '  Check.ForeColor = Color.Navy
                cell = New TableCell



            Next
            CreaTabellaVociParametriOLD = newTbl
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function

        End Try


    End Function

    Private Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        SalvaRequisiti()


        ' RedirectPage()

    End Sub
    Private Sub RedirectPage()
        Response.Redirect("dettaglioverificarequisiti.aspx?IdEnte=" & Request.QueryString("IdEnte") & "&IdEnteSedeAttuazione=" & Request.QueryString("IdEnteSedeAttuazione") & "&StatoVerifica=" & Request.QueryString("StatoVerifica") & "&IDVerificheAssociate=" & Request.QueryString("IDVerificheAssociate") & "&IdVerifica=" & Request.QueryString("IdVerifica"))
    End Sub
    Private Sub SalvaRequisiti()
        Dim s As Integer
        Dim j As Integer
        Dim tblcel As TableCell
        Dim i As Integer
        Dim appo As String
        Dim nomech As String
        Dim idVerifica As String
        Dim IdVerificaEsiti As String
        Dim dtVerifiche As DataTable
        Dim iAtt As Integer
        Dim dtRisposta As DataTable

        j = CInt(txtnome.Value)

        strSql = "Select IdVerificheEsiti from TVerificheEsiti where IDVerificheAssociate  =" & Session("IDVerificheAssociate") & " "
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrgenerico.Read()

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            'IdVerificaEsiti = dtrgenerico("IdVerificheEsiti")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            strSql = "Delete TVerificheEsiti where IDVerificheAssociate =" & Session("IDVerificheAssociate") & " "
            ClsServer.EseguiSqlClient(strSql, Session("conn"))
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        Dim w As Integer
        strSql = " select IdEsito, Esito from TVerificheEsitoTemplate"
        dtRisposta = ClsServer.CreaDataTable(strSql, False, Session("conn"))
        For w = 0 To dtRisposta.Rows.Count - 1
            s = 1
            For s = 1 To j

                nomech = s & "cs" & "Z" & dtRisposta.Rows(w).Item("IdEsito")

                Inserisci("s", panelAttuazionedelProgetto, nomech, Session("IDVerificheAssociate"))
            Next
        Next

        txtnome.Value = j.ToString()
        LblErrore.Visible = False
        lblmessaggio.Visible = True
        lblmessaggio.Text = "Salvataggio eseguito con successo."
    End Sub
    Private Sub Inserisci(ByVal tipo As String, ByVal tbl As Panel, ByVal nomeCheck As String, ByVal IDVerificheAssociate As Double)
        Dim IntCkeck As Integer
        Dim IdRequisito As Integer

        Dim check As CheckBox = DirectCast(tbl.FindControl(nomeCheck), CheckBox)
        Dim lbl As Label = DirectCast(tbl.FindControl("l" & nomeCheck), Label)


        If Not IsNothing(check) Then
            If check.Checked = True Then

                Dim lbl1 As Label = DirectCast(tbl.FindControl("lp" & nomeCheck), Label)

                IdRequisito = lbl.Text

                IntCkeck = lbl1.Text
                strSql = "INSERT INTO TVerificheEsiti(IDVerificheAssociate ,IdRequisito,Esito)"
                strSql = strSql & "values (" & IDVerificheAssociate & " ," & IdRequisito & " ," & IntCkeck & " )"

                ClsServer.EseguiSqlClient(strSql, Session("conn"))
            End If
        End If
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click

        Response.Redirect("verificarequisiti.aspx?IdEnte=" & Session("IdEntePro") & "&IDVerificheAssociate=" & Session("IDVerificheAssociate") & "&IdVerifica=" & Session("IdVerifica"))
    End Sub

    Private Function ControllaRequisiti(ByVal IDVerificheAssociate As Integer) As String

        strSql = " Select ISNULL(ConfermaRequisiti, 0) AS ConfermaRequisiti," & _
                 " dbo.FormatoData(DataConfermaRequisiti) as DataConfermaRequisiti " & _
                 " FROM TVerificheAssociate  where IDVerificheAssociate =" & IDVerificheAssociate
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            If dtrgenerico("ConfermaRequisiti") = True Then
                cmdConferma.Visible = False
                cmdSalva.Visible = False
                ControllaRequisiti = dtrgenerico("dataConfermaRequisiti")
            Else
                ControllaRequisiti = ""
            End If
        Else
            ControllaRequisiti = ""
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return ControllaRequisiti
    End Function

    Private Function CaricaRequisiti(ByVal IDVerificheAssociate As Integer, ByVal Flag As String) As Boolean
        Dim s As Integer
        Dim j As Integer
        Dim nomecec As String
        'If Flag <> "" Then
        strSql = " Select * " & _
                " FROM TVerificheEsiti " & _
                " where IDVerificheAssociate =" & IDVerificheAssociate & " "
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            j = CInt(txtnome.Value)
            Do While dtrgenerico.Read
                For s = 1 To j
                    nomecec = s & "cs" & "Z" & dtrgenerico("Esito")
                    ValorizzaCheck(nomecec, dtrgenerico("IdRequisito"), dtrgenerico("Esito"))
                Next
            Loop
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return CaricaRequisiti
        'End If

    End Function

    Private Sub ValorizzaCheck(ByVal NomeCheck As String, ByVal Idrequisito As String, ByVal Esito As Integer)
        Dim check As CheckBox = DirectCast(panelAttuazionedelProgetto.FindControl(NomeCheck), CheckBox)

        Dim appo As Integer
        Dim Requisito As Integer

        'Select Case Esito
        '    Case "1"
        Dim lbl As Label = DirectCast(panelAttuazionedelProgetto.FindControl("l" & NomeCheck), Label)
        If Not IsNothing(lbl) Then
            Requisito = lbl.Text
        End If

        If Requisito = Idrequisito Then
            If Not IsNothing(check) Then
                check.Checked = True
            End If
        End If
    End Sub

    Protected Sub cmdConferma_Click(sender As Object, e As EventArgs) Handles cmdConferma.Click



        If ControllaDomande() = False Then
            lblmessaggio.Visible = False
            LblErrore.Visible = True
            LblErrore.Text = "Attenzione! E' necessario effettuare la valutazione di tutti requisiti per procedere alla Conferma."

            Exit Sub
        End If
        Dim strsql As String
        Dim dtrCon As SqlClient.SqlDataReader
        Dim strData As String
        SalvaRequisiti()

        strsql = "Update TVerificheAssociate Set confermarequisiti =1, DataConfermaRequisiti = getdate() where IDVerificheAssociate  =" & Session("IDVerificheAssociate")
        ClsServer.EseguiSqlClient(strsql, Session("conn"))
        cmdConferma.Visible = False
        cmdSalva.Visible = False
        cmdRipristina.Visible = True


        strsql = " Select isnull(TVerificheAssociate.confermarequisiti,0)" & _
                 " from TVerificheAssociate " & _
                 " inner join tVerifiche on TVerificheAssociate.idverifica = tVerifiche.idverifica " & _
                 " where tVerifiche.idverifica = " & Session("idverifica") & " " & _
                 " and isnull(TVerificheAssociate.confermarequisiti,0)=0   "
        '" isnull(dataFineVerifica,0)  as dataInizioVerifica isnull(dataInizioVerifica,0) as dataInizioVerifica," & _
        dtrCon = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrCon.HasRows = False Then
            dtrCon.Read()
            ' strData = dtrCon("dataInizioVerifica")
            If Not dtrCon Is Nothing Then
                dtrCon.Close()
                dtrCon = Nothing
            End If

            strsql = " Select tVerifiche.dataInizioVerifica,tVerifiche.datafineVerifica " & _
                     " from TVerificheAssociate " & _
                     " inner join tVerifiche on TVerificheAssociate.idverifica = tVerifiche.idverifica " & _
                     " where tVerifiche.idverifica = " & Session("idverifica") & " "
            ' " and  isnull(tVerifiche.dataInizioVerifica,0)<>0 and isnull(tVerifiche.datafineVerifica,0)<>0  "
            '" isnull(dataFineVerifica,0)  as dataInizioVerifica isnull(dataInizioVerifica,0) as dataInizioVerifica," & _
            dtrCon = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrCon.HasRows = True Then
                dtrCon.Read()
                strData = "" & dtrCon("dataInizioVerifica")
                If Not dtrCon Is Nothing Then
                    dtrCon.Close()
                    dtrCon = Nothing
                End If

                strsql = "Update TVerifiche Set "
                If strData <> "" Then
                    strsql = strsql & " IdStatoVerifica =7,"
                End If

                strsql = strsql & "confermarequisiti =1, DataConfermaRequisiti = getdate() where IDVerifica  =" & Session("IDVerifica")
                ClsServer.EseguiSqlClient(strsql, Session("conn"))
            End If
        End If
        If Not dtrCon Is Nothing Then
            dtrCon.Close()
            dtrCon = Nothing
        End If
        BloccaSbloccaChek(False) 'blocco i  check


    End Sub

    Private Sub BloccaSbloccaChek(ByVal valore As Boolean)
        Dim j As Integer
        Dim s As Integer
        Dim nomech As String
        Dim w As Integer
        Dim dtRisposta As DataTable


        strSql = "select IdEsito from TVerificheEsitoTemplate"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            j = CInt(txtnome.Value)
            Dim check As CheckBox
            Do While dtrgenerico.Read
                For s = 1 To j
                    nomech = s & "cs" & "Z" & dtrgenerico("IdEsito")
                    check = DirectCast(panelAttuazionedelProgetto.FindControl(nomech), CheckBox)
                    If Not IsNothing(check) Then
                        check.Enabled = valore
                    Else
                        check = DirectCast(panelFormazionedeiVolontari.FindControl(nomech), CheckBox)
                        If Not IsNothing(check) Then
                            check.Enabled = valore
                        Else
                            check = DirectCast(panelOperativitadeiVolontari.FindControl(nomech), CheckBox)
                            If Not IsNothing(check) Then
                                check.Enabled = valore
                            Else
                                check = DirectCast(panelRisorseFinanziarieErogate.FindControl(nomech), CheckBox)
                                If Not IsNothing(check) Then
                                    check.Enabled = valore
                                 
                                End If
                            End If
                        End If

                    End If
                Next
            Loop
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If



    End Sub

    Protected Sub cmdApplica_Click(sender As Object, e As EventArgs) Handles cmdApplica.Click

        Dim strsql As String
        Dim dtrApplica As SqlClient.SqlDataReader
        Dim IDVerificheAssociate As Integer

        SalvaRequisiti()

        strsql = " Select IDVerificheAssociate from TVerificheAssociate " & _
                " Where IDVerificheAssociate <> " & Session("IDVerificheAssociate") & " " & _
                " and IdVerifica = " & Session("IdVerifica") & " and isnull(ConfermaRequisiti,0) =0"
        dtrApplica = ClsServer.CreaDatareader(strsql, Session("conn"))

        Dim vDelete() As String

        ReDim vDelete(0)

        vDelete(0) = ""

        If dtrApplica.HasRows = True Then
            Do While dtrApplica.Read
                IDVerificheAssociate = dtrApplica("IDVerificheAssociate")

                If vDelete(0) = "" Then
                    vDelete(0) = "Delete from TVerificheEsiti WHERE IDVerificheAssociate = " & IDVerificheAssociate
                Else
                    ReDim Preserve vDelete(UBound(vDelete) + 1)
                    vDelete(UBound(vDelete)) = "Delete from TVerificheEsiti WHERE IDVerificheAssociate = " & IDVerificheAssociate
                End If

                If vDelete(0) <> "" Then
                    ReDim Preserve vDelete(UBound(vDelete) + 1)
                    strsql = "INSERT INTO TVerificheEsiti (IDVerificheAssociate, IdRequisito, Esito)"
                    strsql = strsql & " SELECT " & IDVerificheAssociate & " , IdRequisito, Esito"
                    strsql = strsql & " FROM TVerificheEsiti"
                    strsql = strsql & " WHERE IDVerificheAssociate = " & Session("IDVerificheAssociate")
                    vDelete(UBound(vDelete)) = strsql
                End If

            Loop


            If Not dtrApplica Is Nothing Then
                dtrApplica.Close()
                dtrApplica = Nothing
            End If
            Dim intX As Integer
            Dim myCommand As SqlClient.SqlCommand
            myCommand = New SqlClient.SqlCommand

            myCommand.Connection = Session("conn")

            For intX = 0 To UBound(vDelete)
                myCommand.CommandText = vDelete(intX)
                myCommand.ExecuteNonQuery()
            Next

            myCommand.Dispose()

        End If
        If Not dtrApplica Is Nothing Then
            dtrApplica.Close()
            dtrApplica = Nothing
        End If
        RedirectPage()
    End Sub




    Private Sub CaricaIntestazione()
        Dim dtrSede As SqlClient.SqlDataReader

        If Not dtrSede Is Nothing Then
            dtrSede.Close()
            dtrSede = Nothing
        End If


        strSql = " Select e.denominazione + ' (' + e.codiceregione + ')' as enteproponente, "
        strSql = strSql & " esa.denominazione + ' (' + convert(varchar,esa.IDEnteSedeAttuazione)  + ')'  As EnteSede,"
        strSql = strSql & " es.indirizzo + ' ' + es.civico + ' ' + es.cap as indirizzo,"
        strSql = strSql & " c.denominazione + ' (' + p.descrabb +')' as comune,"
        strSql = strSql & " r.regione, "
        strSql = strSql & " es.PrefissoTelefono + '' + es.Telefono AS Telefono, es.PrefissoFax + '' + es.Fax AS Fax"
        strSql = strSql & " from entisediattuazioni esa"
        strSql = strSql & " inner join entisedi es on esa.identesede =es.identesede"
        strSql = strSql & " inner join enti e on es.idente =e.idente"
        strSql = strSql & " inner join comuni c on es.idcomune = c.idcomune"
        strSql = strSql & " inner join provincie p on p.idprovincia = c.idprovincia"
        strSql = strSql & " inner join regioni r on r.idregione = p.idregione"
        strSql = strSql & " where IDEnteSedeAttuazione = " & Session("IDEnteSedeAttuazione") & ""

        dtrSede = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrSede.HasRows = True Then
            dtrSede.Read()
            lblEnte.Text = "" & dtrSede("enteproponente")
            lblsede.Text = "" & dtrSede("EnteSede")
            LblIndirizzoSede.Text = "" & dtrSede("indirizzo")
            LblComuneSede.Text = "" & dtrSede("comune")
            LblRegioneSede.Text = "" & dtrSede("regione")
            LblTelefonoSede.Text = "" & dtrSede("Telefono")
            LblFaxSede.Text = "" & dtrSede("Fax")
        End If
        If Not dtrSede Is Nothing Then
            dtrSede.Close()
            dtrSede = Nothing
        End If
    End Sub
    Private Sub StatoVerifica(ByVal StatoVerifica As String)
        Select Case StatoVerifica
            Case "Aperta"
                TastiNonVisibili()
            Case "In Esecuzione"
                TastiVisibili()
                If Session("StatoRequisiti") = "Si" Then
                    cmdRipristina.Visible = True
                    cmdSalva.Visible = False
                    cmdConferma.Visible = False
                    BloccaSbloccaChek(False) 'blocco i check
                End If
            Case "Eseguita"
            Case "Chiusa Positivamente"
                TastiNonVisibili()
                BloccaSbloccaChek(False) 'blocco i check
            Case "Chiusa con Richiamo"
                TastiNonVisibili()
                BloccaSbloccaChek(False) 'blocco i check
            Case "Contestata"
                TastiNonVisibili()
                BloccaSbloccaChek(False) 'blocco i check
            Case "Sanzionata"
                TastiNonVisibili()
                BloccaSbloccaChek(False) 'blocco i check
            Case "Sospesa"
                TastiNonVisibili()
                BloccaSbloccaChek(False) 'blocco i check
        End Select

    End Sub
    Private Sub TastiVisibili()
        cmdConferma.Visible = True
        cmdApplica.Visible = True
        cmdSalva.Visible = True
    End Sub
    Private Sub TastiNonVisibili()
        cmdConferma.Visible = False
        cmdApplica.Visible = False
        cmdSalva.Visible = False
        cmdRipristina.Visible = False
    End Sub

    Private Sub cmdRipristina_Click(sender As Object, e As EventArgs) Handles cmdRipristina.Click
        Dim strNull As String = "Null"

        strSql = "Update TVerificheAssociate set confermarequisiti = 0, DataConfermaRequisiti = " & strNull & " where IDVerificheAssociate  =" & Session("IDVerificheAssociate")
        ClsServer.EseguiSqlClient(strSql, Session("conn"))

        strSql = "Update TVerifiche Set "
        strSql = strSql & "confermarequisiti = 0, DataConfermaRequisiti =" & strNull & " where IDVerifica  =" & Session("IDVerifica")
        ClsServer.EseguiSqlClient(strSql, Session("conn"))


        cmdRipristina.Visible = False
        'SbloccaChek()
        BloccaSbloccaChek(True) 'sblocco i check
        RedirectPage()
    End Sub


    Private Function ControllaDomande() As Boolean

        Dim x As Integer
        x = 0
        Dim allTextBoxValues As String = ""
        Dim c As Control
        Dim childc As Control
        ControllaDomande = False
        For Each c In panelAttuazionedelProgetto.Controls
            For Each childc In c.Controls
                If TypeOf childc Is CheckBox Then
                    Dim cc As CheckBox
                    cc = CType(childc, CheckBox)

                    If cc.Checked = True Then
                        x = x + 1
                    End If
                End If
            Next
        Next

        For Each c In panelFormazionedeiVolontari.Controls
            For Each childc In c.Controls
                If TypeOf childc Is CheckBox Then
                    Dim cc As CheckBox
                    cc = CType(childc, CheckBox)

                    If cc.Checked = True Then
                        x = x + 1
                    End If
                End If
            Next
        Next


        For Each c In panelOperativitadeiVolontari.Controls
            For Each childc In c.Controls
                If TypeOf childc Is CheckBox Then
                    Dim cc As CheckBox
                    cc = CType(childc, CheckBox)

                    If cc.Checked = True Then
                        x = x + 1
                    End If
                End If
            Next
        Next

        For Each c In panelRisorseFinanziarieErogate.Controls
            For Each childc In c.Controls
                If TypeOf childc Is CheckBox Then
                    Dim cc As CheckBox
                    cc = CType(childc, CheckBox)

                    If cc.Checked = True Then
                        x = x + 1
                    End If
                End If
            Next
        Next

        If txtx.Value = x Then
            ControllaDomande = True
        End If
    End Function



End Class

