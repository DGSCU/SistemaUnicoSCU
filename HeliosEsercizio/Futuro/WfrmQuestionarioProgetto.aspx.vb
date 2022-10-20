Imports System.Data.SqlClient
Imports System.Drawing

Public Class WfrmQuestionarioProgetto
    Inherits System.Web.UI.Page

    Dim query As String
    Dim dataReader As SqlClient.SqlDataReader
    Dim dtsgenerico As DataSet
    Dim comGenerico As SqlClient.SqlCommand
    Dim idBando As Integer
    Dim counter As Integer
    Dim SEZIONE_CARATTERISTICHE_ORGANIZZATIVE As String = "CaratteristicheOrganizzative"
    Dim SEZIONE_CARATTERISTICHE_PROGETTO As String = "CaratteristicheProgetto"
    Dim SEZIONE_CARATTERISTICHE_CONOSCENZE_ACQUISITE As String = "CaratteristicheConoscenzeAcquisite"
    Dim infoCheck As List(Of DatiQuestionario)
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
        idBando = 0

        If IsPostBack = False Then
            Session.Remove("datiQuestionario")
            infoCheck = New List(Of DatiQuestionario)
            Session("tipoazione") = Context.Items("tipoazione")

            If Session("tipoazione") = "Inserimento" Then
                cmdStampaVQ.Visible = False
            End If
            ChiudiDataReader(dataReader)
            query = "Select denominazione + '(' + codiceregione + ')' as Ente,idente from enti " & _
                     " where enti.idente=" & Session("idente")

            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()

            If dataReader.HasRows = True Then
                lblEnte.Text = UCase(dataReader("Ente"))
            End If
            ChiudiDataReader(dataReader)

            query = "Select idBando,Bando from Bando " & _
                                " where idbando=" & Session("idbando")

            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()

            If dataReader.HasRows = True Then
                lblProgetto.Text = UCase(dataReader("Bando"))
                idBando = dataReader("iDBando")
            End If
            ChiudiDataReader(dataReader)

            If idBando <> 0 Then
                Call VerificaGiorni(idBando)
            End If

            CreaTabellaParametri(SEZIONE_CARATTERISTICHE_PROGETTO, panelCaratteristicheProgetto)
            CreaTabellaParametri(SEZIONE_CARATTERISTICHE_ORGANIZZATIVE, panelCaratteristicheOrganizzative)
            CreaTabellaParametri(SEZIONE_CARATTERISTICHE_CONOSCENZE_ACQUISITE, panelConoscenzeAcquisite)

            lblTitolo.Visible = True
            lblProgetto.Visible = True
            divBando.Visible = True
            PopolaMaschera(Session("idbando"))
            Session.Add("datiQuestionario", infoCheck)

        Else
            txtx.Value = 0
            CreaTabellaParametri(SEZIONE_CARATTERISTICHE_PROGETTO, panelCaratteristicheProgetto)
            CreaTabellaParametri(SEZIONE_CARATTERISTICHE_ORGANIZZATIVE, panelCaratteristicheOrganizzative)
            CreaTabellaParametri(SEZIONE_CARATTERISTICHE_CONOSCENZE_ACQUISITE, panelConoscenzeAcquisite)

        End If

    End Sub

    Private Sub CreaTabellaParametri(ByVal sezione As String, ByRef contenitore As Panel)
        Dim dtGruppo As SqlClient.SqlDataReader
        Dim intMaxGruppo As Integer
        Dim intGrup As Integer
        Dim i As Integer
        Dim j As Integer
        Dim r As TableRow
        Dim c As TableCell
        Dim myRow As DataRow
        Dim Tab As DataTable
        Dim coll As Collection
        Dim XX As Integer

        Dim td As Table
        intMaxGruppo = 0
        intGrup = 0


        query = "Select * from QuestionarioParametri where RevisioneFormazione=" & Request.QueryString("RevisioneFormazione") & " AND attivo=1  "

        If sezione = SEZIONE_CARATTERISTICHE_PROGETTO Then
            query = query & " and classePunteggio=0"
        End If
        If sezione = SEZIONE_CARATTERISTICHE_ORGANIZZATIVE Then
            query = query & " and classePunteggio=1"
        End If
        If sezione = SEZIONE_CARATTERISTICHE_CONOSCENZE_ACQUISITE Then
            query = query & " and classePunteggio=2"
        End If


        Tab = ClsServer.CreaDataTable(query, False, Session("conn"))

        ' Creo Righe e celle
        Dim a As Integer = 1

        For Each myRow In Tab.Rows
            Dim img As System.Web.UI.WebControls.Image = New System.Web.UI.WebControls.Image
            img.ImageUrl = "images\info_small.png"
            img.ID = "img" & sezione & a
            a = a + 1
            img.ToolTip = myRow.Item("rationale")

            r = New TableRow
            c = New TableCell
            c.Controls.Add(img)
            Dim panelInterno As Panel = New Panel
            panelInterno.CssClass = "panel"
            Dim lblPanelInterno As Label = New Label
            lblPanelInterno.CssClass = "labelDati"
            Dim sceltaMultipla As Boolean
            Dim parametro = myRow.Item("parametro")
            Select Case myRow.Item("tipo")

                Case 0
                    txtx.Value = CInt(txtx.Value) + 1
                    lblPanelInterno.Text = "(" & myRow.Item("Ordine") & ")" & parametro & " [Scelta Multipla] <br/>"
                    lblPanelInterno.Text = lblPanelInterno.Text & " <br/>"
                    sceltaMultipla = True
                Case 1
                    txtx.Value = CInt(txtx.Value) + 1
                    lblPanelInterno.Text = "(" & myRow.Item("Ordine") & ")" & parametro & " [Scelta Singola] <br/>"
                    lblPanelInterno.Text = lblPanelInterno.Text & " <br/>"
                    sceltaMultipla = False
                Case 2
                    query = "SELECT isnull(MAX(Gruppo),0) AS MaxGruppo FROM QuestionarioVociParametri " & _
                             " WHERE (IDParametro = " & myRow.Item("idParametro") & ")"
                    dtGruppo = ClsServer.CreaDatareader(query, Session("conn"))
                    dtGruppo.Read()
                    intMaxGruppo = dtGruppo.Item("MaxGruppo")

                    ChiudiDataReader(dtGruppo)
                    txtx.Value = CInt(txtx.Value) + intMaxGruppo + 1
                    lblPanelInterno.Text = "(" & myRow.Item("Ordine") & ")" & parametro & " [Scelta Singola per gruppi] <br/>"
                    lblPanelInterno.Text = lblPanelInterno.Text & " <br/>"
                    sceltaMultipla = False
            End Select

            panelInterno.Controls.Add(lblPanelInterno)
            contenitore.Controls.Add(panelInterno)
            

            If myRow.Item("tipo") = 2 Then
                Dim numeroGruppo As Byte = 0
                For intGrup = 0 To intMaxGruppo
                    Dim label As Label = New Label
                    numeroGruppo = intGrup + 1
                    label.CssClass = "labelDati"
                    label.Text = "Gruppo " & numeroGruppo & "<br/>"
                    panelInterno.Controls.Add(label)
                    panelInterno.Controls.Add(CreaDivRigaVuota())
                    Dim descrizione As String = parametro & " - " & "Gruppo " & numeroGruppo
                    CreaTabellaVociParametriGruppo(panelInterno, myRow.Item("idParametro"), intGrup, sceltaMultipla, descrizione)
                    panelInterno.Controls.Add(CreaDivRigaVuota())
                Next
                panelInterno.Controls.Add(CreaDivRigaVuota())
            Else
                CreaTabellaVociParametri(panelInterno, myRow.Item("idParametro"), myRow.Item("tipo"), sceltaMultipla, parametro)
            End If
        Next
        txtnome.Value = counter
    End Sub
    Private Function CreaDivRigaVuota() As Panel
        Dim panelRigaVuota As Panel = New Panel
        panelRigaVuota.CssClass = "RigaVuota"
        Return panelRigaVuota
    End Function


    Private Sub CreaTabellaVociParametri(ByRef contenitore As Panel, ByVal idParametro As Integer, ByVal tipoParametro As Integer, ByVal sceltaMultipla As Boolean, ByVal descrizioneGruppo As String)
        Dim dataRow As DataRow
        Dim dataTable As DataTable
        ' Estrazione delle vociparametro Relative ad un parametro 
        query = "Select QuestionarioVociparametri.idvoce,QuestionarioVociparametri.voce,QuestionarioVociparametri.punteggio," & _
                " QuestionarioVociparametri.gruppo,Questionarioparametri.tipo " & _
                " from QuestionarioVociparametri " & _
                " inner join Questionarioparametri on Questionarioparametri.idparametro=QuestionarioVociparametri.idparametro " & _
                " where Questionarioparametri.idparametro=" & idParametro & " and QuestionarioVociparametri.attivo=1 "

        ChiudiDataReader(dataReader)
        dataTable = ClsServer.CreaDataTable(query, False, Session("conn"))
        Dim nomeGruppo As String = tipoParametro & idParametro.ToString
        If (sceltaMultipla = False) Then
            For Each dataRow In dataTable.Rows
                Dim radioButton As RadioButton = New RadioButton
                radioButton.Checked = False
                counter = counter + 1
                radioButton.ID = counter & "cs"
                radioButton.CssClass = "textbox"
                radioButton.Width = New Unit(400)
                radioButton.AutoPostBack = False
                radioButton.Text = dataRow.Item("voce").ToString().ToUpper(System.Globalization.CultureInfo.CurrentCulture)
                If txtBloccaMaschera.Value = "Si" Then
                    radioButton.Enabled = False
                End If
               radioButton.GroupName = nomeGruppo
                Dim labelIdVoce As Label = New Label
                labelIdVoce.ID = "l" & radioButton.ID
                labelIdVoce.ForeColor = Color.FromName("#c9dfff")
                labelIdVoce.Text = dataRow.Item("idvoce")
                labelIdVoce.Visible = False
                Dim labelCheckBoxId As Label = New Label
                labelCheckBoxId.ID = "lp" & radioButton.ID
                labelCheckBoxId.ForeColor = Color.FromName("#c9dfff")
                labelCheckBoxId.Text = idParametro
                labelCheckBoxId.Visible = False
                Dim labelPunteggio As Label = New Label
                labelPunteggio.ID = "lpu" & radioButton.ID
                labelPunteggio.ForeColor = Color.Red
                labelPunteggio.Text = dataRow.Item("Punteggio")
                labelPunteggio.Visible = False
                contenitore.Controls.Add(radioButton)
                contenitore.Controls.Add(labelCheckBoxId)
                contenitore.Controls.Add(labelPunteggio)
                contenitore.Controls.Add(labelIdVoce)
                If IsPostBack = False Then
                    Dim datiVerificaQuestionario As DatiQuestionario = New DatiQuestionario()
                    datiVerificaQuestionario.Id = radioButton.ID
                    datiVerificaQuestionario.TipoOggetto = TipoOggetto.RadioButton
                    datiVerificaQuestionario.Gruppo = nomeGruppo
                    datiVerificaQuestionario.Descrizione = descrizioneGruppo
                    infoCheck.Add(datiVerificaQuestionario)
                End If

            Next
        Else
            For Each dataRow In dataTable.Rows
                Dim checkBox As CheckBox = New CheckBox
                checkBox.Checked = False
                counter = counter + 1
                checkBox.ID = counter & "cs"
                checkBox.CssClass = "textbox"
                checkBox.Width = New Unit(400)
                checkBox.AutoPostBack = False
                checkBox.Text = dataRow.Item("voce").ToString().ToUpper(System.Globalization.CultureInfo.CurrentCulture)
                If txtBloccaMaschera.Value = "Si" Then
                    checkBox.Enabled = False
                End If
                 checkBox.ValidationGroup = nomeGruppo
                Dim labelIdVoce As Label = New Label
                labelIdVoce.ID = "l" & checkBox.ID
                labelIdVoce.ForeColor = Color.FromName("#c9dfff")
                labelIdVoce.Text = dataRow.Item("idvoce")
                labelIdVoce.Visible = False
                Dim labelCheckBoxId As Label = New Label
                labelCheckBoxId.ID = "lp" & checkBox.ID
                labelCheckBoxId.ForeColor = Color.FromName("#c9dfff")
                labelCheckBoxId.Text = idParametro
                labelCheckBoxId.Visible = False
                Dim labelPunteggio As Label = New Label
                labelPunteggio.ID = "lpu" & checkBox.ID
                labelPunteggio.ForeColor = Color.Red
                labelPunteggio.Text = dataRow.Item("Punteggio")
                labelPunteggio.Visible = False
                contenitore.Controls.Add(checkBox)
                contenitore.Controls.Add(labelCheckBoxId)
                contenitore.Controls.Add(labelPunteggio)
                contenitore.Controls.Add(labelIdVoce)
                If IsPostBack = False Then
                    Dim datiVerificaQuestionario As DatiQuestionario = New DatiQuestionario()
                    datiVerificaQuestionario.Id = checkBox.ID
                    datiVerificaQuestionario.TipoOggetto = TipoOggetto.RadioButton
                    datiVerificaQuestionario.Gruppo = nomeGruppo
                    datiVerificaQuestionario.Descrizione = descrizioneGruppo
                    infoCheck.Add(datiVerificaQuestionario)
                End If

            Next
        End If
        AggiungiRigaVuota(contenitore)
        ChiudiDataReader(dataReader)



    End Sub
    Private Function CreaTabellaVociParametriGruppo(ByRef contenitore As Panel, ByVal idParametro As Integer, ByVal gruppo As Integer, ByVal sceltaMultipla As Boolean, ByVal descrizioneGruppo As String) As Table
        Dim dataRow As DataRow
        Dim dataTable As DataTable
        ' Estrazione delle vociparametro Relative ad un parametro 
        query = "Select QuestionarioVociparametri.idvoce,QuestionarioVociparametri.voce,QuestionarioVociparametri.punteggio," & _
        " QuestionarioVociparametri.gruppo,Questionarioparametri.tipo " & _
        " from QuestionarioVociparametri " & _
        " inner join Questionarioparametri on Questionarioparametri.idparametro=QuestionarioVociparametri.idparametro " & _
        " where Questionarioparametri.idparametro=" & idParametro & " and QuestionarioVociparametri.attivo=1 and QuestionarioVociparametri.gruppo=" & gruppo & " "

        ChiudiDataReader(dataReader)
        dataTable = ClsServer.CreaDataTable(query, False, Session("conn"))
        Dim nomeGruppo As String = gruppo.ToString + idParametro.ToString
        For Each dataRow In dataTable.Rows
            Dim radioButton As RadioButton = New RadioButton
            radioButton.Checked = False
            counter = counter + 1
            radioButton.ID = counter & "cs"
            radioButton.SkinID = gruppo.ToString
            radioButton.CssClass = "textbox"
            radioButton.Width = New Unit(400)
            radioButton.AutoPostBack = False
            radioButton.GroupName = nomeGruppo
            radioButton.Text = dataRow.Item("voce").ToString().ToUpper(System.Globalization.CultureInfo.CurrentCulture)
            If txtBloccaMaschera.Value = "Si" Then
                radioButton.Enabled = False
            End If
            Dim labelIdVoce As Label = New Label
            labelIdVoce.ID = "l" & radioButton.ID
            labelIdVoce.ForeColor = Color.FromName("#c9dfff")
            labelIdVoce.Text = dataRow.Item("idvoce")
            labelIdVoce.Visible = False
            Dim labelRadioButtonId As Label = New Label
            labelRadioButtonId.ID = "lp" & radioButton.ID
            labelRadioButtonId.ForeColor = Color.FromName("#c9dfff")
            labelRadioButtonId.Text = idParametro
            labelRadioButtonId.Visible = False
            Dim labelPunteggio As Label = New Label
            labelPunteggio.ID = "lpu" & radioButton.ID
            labelPunteggio.ForeColor = Color.Red
            labelPunteggio.Text = dataRow.Item("Punteggio")
            labelPunteggio.Visible = False
            contenitore.Controls.Add(radioButton)
            contenitore.Controls.Add(labelRadioButtonId)
            contenitore.Controls.Add(labelPunteggio)
            contenitore.Controls.Add(labelIdVoce)
            If IsPostBack = False Then
                Dim datiVerificaQuestionario As DatiQuestionario = New DatiQuestionario()
                datiVerificaQuestionario.Id = radioButton.ID
                datiVerificaQuestionario.TipoOggetto = TipoOggetto.RadioButton
                datiVerificaQuestionario.Gruppo = nomeGruppo
                datiVerificaQuestionario.Descrizione = descrizioneGruppo
                infoCheck.Add(datiVerificaQuestionario)
            End If
        Next
        AggiungiRigaVuota(contenitore)
        ChiudiDataReader(dataReader)
    End Function

    Private Sub PopolaMaschera(ByVal idbando As Integer)
        Dim s As Integer
        Dim j As Integer
        Dim nomech As String

        j = CInt(txtnome.Value)

        query = "select QuestionarioVociParametri.idvoce, QuestionarioVociParametri.idparametro,QuestionariostoricoProgetti.notestorico, QuestionariostoricoProgetti.NumeroVolontari, QuestionariostoricoProgetti.NumeroFormatori " & _
        " from QuestionarioVociParametri" & _
        " inner join Questionariostoricovoci on (Questionariostoricovoci.idvoce=QuestionarioVociParametri.idvoce)" & _
        " inner join QuestionariostoricoProgetti on (Questionariostoricovoci.idStorico=QuestionarioStoricoProgetti.idStorico)" & _
        " inner join BandiAttività ON QuestionarioStoricoProgetti.IdBandoAttività = BandiAttività.IdBandoAttività " & _
        " where idbando=" & idbando & " And idente=" & Session("idente")

        dataReader = ClsServer.CreaDatareader(query, Session("conn"))

        Do While dataReader.Read
            For s = 1 To j
                nomech = s & "cs"
                ValorizzaCheck(nomech, dataReader("idvoce"), dataReader("idparametro"))
                nomech = s & "cm"
                ValorizzaCheck(nomech, dataReader("idvoce"), dataReader("idparametro"))
                txtnote.Text = IIf(Not IsDBNull(dataReader("noteStorico")), dataReader("notestorico"), "")
                txtNumFormatori.Text = IIf(Not IsDBNull(dataReader("NumeroFormatori")), dataReader("NumeroFormatori"), "")
                txtNumVolontari.Text = IIf(Not IsDBNull(dataReader("NumeroVolontari")), dataReader("NumeroVolontari"), "")
            Next
        Loop
        ChiudiDataReader(dataReader)
    End Sub
    Private Sub ValorizzaCheck(ByVal NomeCheck As String, ByVal idvoce As String, ByVal parametro As String)
        Dim lbl As Label = DirectCast(panelCaratteristicheProgetto.FindControl("l" & NomeCheck), Label)
        Dim lblp As Label = DirectCast(panelCaratteristicheProgetto.FindControl("lp" & NomeCheck), Label)
        Dim check As CheckBox = TryCast(panelCaratteristicheProgetto.FindControl(NomeCheck), CheckBox)
        Dim radiobutton As RadioButton = TryCast(panelCaratteristicheProgetto.FindControl(NomeCheck), RadioButton)



        If Not IsNothing(lbl) Then
            If lbl.Text = idvoce And lblp.Text = parametro Then
                If Not IsNothing(check) Then
                    check.Checked = True
                Else
                    If Not IsNothing(radiobutton) Then
                        radiobutton.Checked = True
                    End If
                End If
            End If
        End If
    End Sub


    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Dim sVengoDa As String
        Session("idbando") = Nothing
        sVengoDa = Session("tipoazione")
        Response.Redirect("WfrmRicercaBandiQuestionario.aspx?VengoDa=" & sVengoDa)

    End Sub
    Private Sub cmdConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConferma.Click
        lblerrore.Text = String.Empty
        lblmessaggio.Text = String.Empty
        If (ValidaDati() = False) Then
            Exit Sub
        End If
        Dim s As Integer
        Dim j As Integer
        Dim nomech As String
        Dim idStorico As String
        Dim idStoricoCanc As String
        Dim dtBandoAttivita As DataTable
        Dim iAtt As Integer

        j = CInt(txtnome.Value)

        'estraggo le istanze di presentazione di tutti i progetti dell'enete che hanno volontari con ore formazione caricate
        query = "SELECT DISTINCT BandiAttività.IdBandoAttività " _
               & " FROM entità INNER JOIN " _
               & " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " _
               & " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " _
               & " BandiAttività INNER JOIN " _
               & " attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " _
               & " inner join AttivitàFormazioneGenerale on AttivitàFormazioneGenerale.IdAttività  = attività.IDAttività " _
               & " WHERE AttivitàFormazioneGenerale.TipoFormazioneGenerale=" & Request.QueryString("TipoFormazioneGenerale") & " AND (ISNULL(entità.OreFormazione, 0) > 0) AND (BandiAttività.IdBando = " & Session("idbando") & ") AND (BandiAttività.IdEnte = " & Session("idente") & ")"
        dtBandoAttivita = ClsServer.CreaDataTable(query, False, Session("conn"))

        For iAtt = 0 To dtBandoAttivita.Rows.Count - 1
            'cancello il questionario precedente
            'prendo l' idstorico del progetto
            query = "Select idstorico as maxid from QuestionarioStoricoProgetti Where TipoFormazioneGenerale=" & Request.QueryString("TipoFormazioneGenerale") & " and IdBandoAttività=" & dtBandoAttivita.Rows(iAtt).Item(0)
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()

            If dataReader.HasRows = True Then
                idStoricoCanc = dataReader("maxid")
            End If
            ChiudiDataReader(dataReader)

            If idStoricoCanc <> "" Then
                'cancello i record nella tabella QuestionarioStoricoVoci
                query = "Delete from QuestionarioStoricoVoci Where IdStorico=" & idStoricoCanc
                ClsServer.EseguiSqlClient(query, Session("conn"))

                'cancello i record nella tabella QuestionarioStoricoProgetti
                query = "Delete from QuestionarioStoricoProgetti Where IdStorico=" & idStoricoCanc
                ClsServer.EseguiSqlClient(query, Session("conn"))

                idStoricoCanc = ""
            End If

            'inserisco il nuovo questionario
            query = "Insert into QuestionarioStoricoProgetti (idbandoattività, dataInserimento," & _
            "  Username,notestorico,NumeroFormatori,NumeroVolontari,TipoFromazioneGenerale) values " & _
            "(" & dtBandoAttivita.Rows(iAtt).Item(0) & ",getdate(), " & _
            "'" & Session("Utente") & "','" & Replace(txtnote.Text, "'", "''") & "'," & txtNumFormatori.Text & "," & txtNumVolontari.Text & ", " & Request.QueryString("TipoFormazioneGenerale") & ")"

            ClsServer.EseguiSqlClient(query, Session("conn"))

            query = "Select @@identity as maxid from QuestionarioStoricoProgetti"
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()

            If dataReader.HasRows = True Then
                idStorico = dataReader("maxid")
            End If
            ChiudiDataReader(dataReader)

            For s = 1 To j
                nomech = s & "cs"
                Inserisci("s", panelCaratteristicheProgetto, nomech, idStorico)
                nomech = s & "cm"
                Inserisci("s", panelCaratteristicheProgetto, nomech, idStorico)
            Next
        Next

        lblmessaggio.Text = "Inserimento questionario progetti terminato con successo."
        cmdStampaVQ.Visible = True

    End Sub
    Private Sub Inserisci(ByVal tipo As String, ByRef panel As Panel, ByVal nomeCheck As String, ByVal idStorico As Double)

        Dim lbl As Label = DirectCast(panel.FindControl("l" & nomeCheck), Label)
        Dim lbl1 As Label = DirectCast(panel.FindControl("lpu" & nomeCheck), Label)
        Dim check As CheckBox = TryCast(panel.FindControl(nomeCheck), CheckBox)
        Dim radiobutton As RadioButton = TryCast(panel.FindControl(nomeCheck), RadioButton)
        Dim aggiornaStorico As Boolean = False
        ChiudiDataReader(dataReader)
        If Not IsNothing(check) Then
            If check.Checked = True Then
                aggiornaStorico = True
            End If
        ElseIf Not IsNothing(radiobutton) Then
            If radiobutton.Checked = True Then
                aggiornaStorico = True
            End If
        End If
        If (aggiornaStorico = True) Then
            'Inserimento StoricoVoci
            query = "insert into QuestionarioStoricoVoci(idStorico,idvoce,punteggio) " & _
            " Select " & idStorico & "," & lbl.Text & ",0 from questionariovociparametri where idvoce=" & lbl.Text & ""
            ClsServer.EseguiSqlClient(query, Session("conn"))
            ChiudiDataReader(dataReader)
        End If

    End Sub

    Private Sub VerificaGiorni(ByVal idBando As Integer)
        Dim queryVerificaGiorni As String
        Dim numeroMassimoGiorniPerModifica As Integer = 180 'Gestione temporale modifica questionario ore <=180 gg

        queryVerificaGiorni = "SELECT datediff(dd,MAX(attività.DataInizioAttività),getdate()) as DiffGG " & _
                " FROM bando INNER JOIN " & _
                " BandiAttività ON bando.IDBando = BandiAttività.IdBando INNER JOIN " & _
                " attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività " & _
                " WHERE (bando.IDBando = " & idBando & ")"

        dataReader = ClsServer.CreaDatareader(queryVerificaGiorni, Session("conn"))
        dataReader.Read()

        If dataReader.HasRows Then
            If dataReader("DiffGG") > numeroMassimoGiorniPerModifica Then
                txtBloccaMaschera.Value = "Si"
                cmdConferma.Visible = False
                ImpostaCampiInSolaLettura(True)
            Else
                txtBloccaMaschera.Value = "No"
                cmdConferma.Visible = True
                ImpostaCampiInSolaLettura(False)
            End If
        End If
        ChiudiDataReader(dataReader)


        queryVerificaGiorni = "select dbo.Formazione_TerminiQuestionario_2(BandiAttività.IdBandoAttività, getdate()," & Request.QueryString("TipoFormazioneGenerale") & ") as risposta from BandiAttività"
        queryVerificaGiorni = queryVerificaGiorni & " where IdBando= '" & Session("idbando") & "'  "
        queryVerificaGiorni = queryVerificaGiorni & " and idente=" & Session("idente")
        dataReader = ClsServer.CreaDatareader(queryVerificaGiorni, Session("conn"))
        dataReader.Read()
        Dim verifica As String
        verifica = dataReader("Risposta")
        ChiudiDataReader(dataReader)
        If verifica = "SI" Then
            cmdConferma.Visible = True
            txtBloccaMaschera.Value = "No"
            ImpostaCampiInSolaLettura(False)
        Else
            cmdConferma.Visible = False
            txtBloccaMaschera.Value = "Si"
            ImpostaCampiInSolaLettura(True)
        End If
    End Sub

    Private Sub cmdStampaVQ_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdStampaVQ.Click
        'Calcolo l'IdStorico relativamente all'IdProgetto in valutazione
        Dim maxId As Integer
        query = "SELECT isnull(Max(IdStorico),0) as MaxIdS FROM  QuestionarioStoricoProgetti INNER JOIN " & _
                " BandiAttività ON QuestionarioStoricoProgetti.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                "  WHERE ISNULL(QuestionarioStoricoProgetti.TipoFormazioneGenerale,1)=" & Request.QueryString("TipoFormazioneGenerale") & " and (BandiAttività.IdBando = " & Session("idbando") & ") and (BandiAttività.Idente = " & Session("idente") & ")"

        'ChiudiDataReader(dataReader)

        'dataReader = ClsServer.CreaDatareader(query, Session("conn"))
        'dataReader.Read()
        'maxId = dataReader("MaxIdS")
        'ChiudiDataReader(dataReader)

        ChiudiDataReader(dataReader)

        dataReader = ClsServer.CreaDatareader(query, Session("conn"))
        If dataReader.HasRows = True Then
            dataReader.Read()
            maxId = dataReader("MaxIdS")
        End If

        ChiudiDataReader(dataReader)
        If maxId = 0 Then
            lblerrore.Text = "Effettuare prima il Salvataggio per richiedere la Stampa"
        Else

            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("window.open('WfrmReportistica.aspx?sTipoStampa=22&IdBando=" & Session("idbando") & "&IdStorico=" & maxId & "','report','height=800,width=800,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
            Response.Write("</SCRIPT>")
        End If
    End Sub
    Private Sub AggiungiRigaVuota(ByRef contenitore As Panel)
        Dim panelRow As Panel = New Panel()
        panelRow.CssClass = "rigaVuota"
        Dim label As Label = New Label()
        label.Text = "&nbsp;"
        panelRow.Controls.Add(label)
        contenitore.Controls.Add(panelRow)
        ChiudiDataReader(dataReader)
    End Sub
    Private Sub ImpostaCampiInSolaLettura(ByVal solaLettura As Boolean)
        txtnote.ReadOnly = solaLettura
        txtNumFormatori.ReadOnly = solaLettura
        txtNumVolontari.ReadOnly = solaLettura

    End Sub

    Private Function ValidaDati() As Boolean
        Dim datiQuestionario As List(Of DatiQuestionario) = CType(Session("datiQuestionario"), List(Of DatiQuestionario))
        Dim item As DatiQuestionario
        Dim itemGruppo As DatiQuestionario
        Dim check As CheckBox
        Dim radiobutton As RadioButton
        Dim gruppo As String
        Dim tipoOggetto As TipoOggetto
        Dim gruppiValutati As List(Of String) = New List(Of String)
        Dim validazioneOk As Boolean = True

        Dim numeroFormatori As Integer = 0
        Dim numeroVolontari As Integer = 0
        If (Integer.TryParse(txtNumFormatori.Text, numeroFormatori) = False Or numeroFormatori = 0) Then
            lblerrore.Text = lblerrore.Text + "Inserire il numero di formatori impiegati nella formazione generale dei volontari.</br>"
            validazioneOk = False
        End If
        If (Integer.TryParse(txtNumVolontari.Text, numeroVolontari) = False Or numeroVolontari = 0) Then
            lblerrore.Text = lblerrore.Text + "Inserire il numero di volontari che hanno partecipato al/ai corso/i di formazione generale.</br>"
            validazioneOk = False
        End If

        For Each item In datiQuestionario
            Dim checked As Boolean = True
            gruppo = item.Gruppo
            tipoOggetto = item.TipoOggetto
            If (gruppiValutati.Contains(gruppo)) Then
                Continue For
            End If
            gruppiValutati.Add(gruppo)

            Dim oggettiDelGruppo As List(Of DatiQuestionario) = (From p As DatiQuestionario In datiQuestionario Where p.Gruppo = gruppo Select p).ToList()
            For Each itemGruppo In oggettiDelGruppo
                If (tipoOggetto = tipoOggetto.CheckBox) Then
                    check = TryCast(panelCaratteristicheProgetto.FindControl(itemGruppo.Id), CheckBox)
                    If check.Checked = True Then
                        checked = True
                        Exit For
                    Else
                        checked = False
                    End If
                Else
                    radiobutton = TryCast(panelCaratteristicheProgetto.FindControl(itemGruppo.Id), RadioButton)
                    If radiobutton.Checked = True Then
                        checked = True
                        Exit For
                    Else
                        checked = False
                    End If
                End If
            Next
            If (checked = False) Then
                lblerrore.Text = lblerrore.Text + "Selezionare almeno un valore per '" + itemGruppo.Descrizione + "'.</br>"
                validazioneOk = False
                checked = True
            End If

        Next

        Return validazioneOk
    End Function
End Class