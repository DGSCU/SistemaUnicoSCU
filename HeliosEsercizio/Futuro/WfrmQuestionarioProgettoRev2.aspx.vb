Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Linq

Public Class WfrmQuestionarioProgettoRev2
    Inherits System.Web.UI.Page

    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Public idBando As Integer
    Dim dtsgenerico As DataSet
    Dim counter As String
    Dim size As System.Web.UI.WebControls.FontUnit
    Dim comGenerico As SqlClient.SqlCommand
    Dim Table5 As Table
    Dim SEZIONE_ANAGRAFICA As String = "Anagrafica"
    Dim SEZIONE_FORMATORI As String = "Formatori"
    Dim SEZIONE_FORMAZIONE_GENERALE As String = "FormazioneGenerale"
    Dim SEZIONE_LINEA_GUIDA As String = "LineaGuida"
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

            strsql = "Select denominazione + '(' + codiceregione + ')' as Ente,idente, codiceregione, IdClasseAccreditamento, RegioniCompetenze.Descrizione as RegioneCompetenza from enti "
            strsql = strsql & "INNER JOIN RegioniCompetenze ON enti.IdRegioneCompetenza=RegioniCompetenze.IdRegioneCompetenza "
            strsql = strsql & " where enti.idente=" & Session("idente")
            ChiudiDataReader(dtrgenerico)
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrgenerico.Read()

            If dtrgenerico.HasRows = True Then
                lblEnte.Text = UCase(dtrgenerico("Ente"))
                lblCodice.Text = UCase(dtrgenerico("codiceregione"))
                lblClasseEnte.Text = UCase(dtrgenerico("IdClasseAccreditamento"))
                lblEnteNazionale.Text = UCase(dtrgenerico("RegioneCompetenza"))
            End If

            ChiudiDataReader(dtrgenerico)

            strsql = "Select idBando,Bando from Bando " & _
                                " where idbando=" & Session("idbando")

            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrgenerico.Read()

            If dtrgenerico.HasRows = True Then
                lblProgetto.Text = UCase(dtrgenerico("Bando"))
                idBando = dtrgenerico("iDBando")
            End If

            ChiudiDataReader(dtrgenerico)

            If idBando <> 0 Then
                Call VerificaGiorni(idBando)
            End If
            CreaTabellaParametri(SEZIONE_ANAGRAFICA, panelAnagrafica)

            CreaTabellaParametri(SEZIONE_FORMATORI, panelFormatori)
            CreaTabellaParametri(SEZIONE_FORMAZIONE_GENERALE, panelFormazioneGenerale)
            CreaTabellaParametri(SEZIONE_LINEA_GUIDA, panelLineaGuida)

            lblTitolo.Visible = True
            lblProgetto.Visible = True
            divBando.Visible = True
            PopolaMaschera(Session("idbando"))
            Session.Add("datiQuestionario", infoCheck)

        Else

            txtx.Value = 0
            CreaTabellaParametri(SEZIONE_ANAGRAFICA, panelAnagrafica)
            CreaTabellaParametri(SEZIONE_FORMATORI, panelFormatori)
            CreaTabellaParametri(SEZIONE_FORMAZIONE_GENERALE, panelFormazioneGenerale)
            CreaTabellaParametri(SEZIONE_LINEA_GUIDA, panelLineaGuida)

        End If

    End Sub
    Private Sub CreaTabellaParametri(ByVal sezione As String, ByRef contenitore As Panel)
        Dim dtGruppo As SqlClient.SqlDataReader
        Dim intMaxGruppo As Integer
        Dim intGrup As Integer
        Dim j As Integer
        Dim r As TableRow
        Dim c As TableCell
        Dim myRow As DataRow
        Dim Tab As DataTable
        intMaxGruppo = 0
        intGrup = 0


        strsql = "Select * from QuestionarioParametri where RevisioneFormazione=" & Request.QueryString("RevisioneFormazione") & " AND attivo=1  "

        If sezione = SEZIONE_ANAGRAFICA Then
            strsql = strsql & " and classePunteggio=0"
        End If
        If sezione = SEZIONE_FORMATORI Then
            strsql = strsql & " and classePunteggio=1"
        End If
        If sezione = SEZIONE_FORMAZIONE_GENERALE Then
            strsql = strsql & " and classePunteggio=2"
        End If
        If sezione = SEZIONE_LINEA_GUIDA Then
            strsql = strsql & " and classePunteggio=3"
        End If

        Tab = ClsServer.CreaDataTable(strsql, False, Session("conn"))

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
            panelInterno.ID = sezione + a.ToString
            Dim sceltaMultipla As Boolean
            Dim parametro = myRow.Item("parametro")
            Select Case myRow.Item("tipo")

                Case 0
                    txtx.Value = CInt(txtx.Value) + 1
                    lblPanelInterno.Text = parametro & " [Scelta Multipla] <br/>"
                    lblPanelInterno.Text = lblPanelInterno.Text & " <br/>"
                    sceltaMultipla = True
                Case 1
                    txtx.Value = CInt(txtx.Value) + 1
                    lblPanelInterno.Text = parametro & " [Scelta Singola] <br/>"
                    lblPanelInterno.Text = lblPanelInterno.Text & " <br/>"
                    sceltaMultipla = False
                Case 2
                    strsql = "SELECT isnull(MAX(Gruppo),0) AS MaxGruppo FROM QuestionarioVociParametri " & _
                             " WHERE (IDParametro = " & myRow.Item("idParametro") & ")"
                    dtGruppo = ClsServer.CreaDatareader(strsql, Session("conn"))
                    dtGruppo.Read()
                    intMaxGruppo = dtGruppo.Item("MaxGruppo")
                    ChiudiDataReader(dtGruppo)
                    txtx.Value = CInt(txtx.Value) + intMaxGruppo + 1
                    lblPanelInterno.Text = parametro & " [Scelta Singola per gruppi] <br/>"
                    lblPanelInterno.Text = lblPanelInterno.Text & " <br/>"
                    sceltaMultipla = True
            End Select

            panelInterno.Controls.Add(lblPanelInterno)
            contenitore.Controls.Add(panelInterno)
            Dim numeroGruppo As Byte = 0

            If myRow.Item("tipo") = 2 Then

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
    Private Sub CreaTabellaVociParametriGruppo(ByRef contenitore As Panel, ByVal idParametro As Integer, ByVal gruppo As Integer, ByVal sceltaMultipla As Boolean, ByVal descrizioneGruppo As String)
        Dim dataRow As DataRow
        Dim dataTable As DataTable
        ' Estrazione delle vociparametro Relative ad un parametro 
        strsql = "Select QuestionarioVociparametri.idvoce,QuestionarioVociparametri.voce,QuestionarioVociparametri.punteggio," & _
        " QuestionarioVociparametri.gruppo,Questionarioparametri.tipo,QuestionarioVociparametri.idparametro " & _
        " from QuestionarioVociparametri " & _
        " inner join Questionarioparametri on Questionarioparametri.idparametro=QuestionarioVociparametri.idparametro " & _
        " where Questionarioparametri.idparametro=" & idParametro & " and QuestionarioVociparametri.attivo=1 and QuestionarioVociparametri.gruppo=" & gruppo & " "

        ChiudiDataReader(dtrgenerico)
        dataTable = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        Dim tbl As String = "T" & counter
        Dim nomeGruppo As String = gruppo.ToString + idParametro.ToString
        For Each dataRow In dataTable.Rows
            Dim radioButton As RadioButton = New RadioButton
            radioButton.Checked = False
            counter = counter + 1
            radioButton.ID = counter & "cs"
            radioButton.ID = radioButton.ID & dataRow.Item("idparametro") & "X"
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
        AggiungiRigaVuota(contenitore)
    End Sub
    Private Sub CreaTabellaVociParametri(ByRef contenitore As Panel, ByVal idParametro As Integer, ByVal tipoParametro As Integer, ByVal sceltaMultipla As Boolean, ByVal descrizioneGruppo As String)
        Dim dataRow As DataRow
        Dim dataTable As DataTable
        ' Estrazione delle vociparametro Relative ad un parametro 
        strsql = "Select QuestionarioVociparametri.idvoce,QuestionarioVociparametri.voce,QuestionarioVociparametri.punteggio," & _
                " QuestionarioVociparametri.gruppo,Questionarioparametri.tipo,QuestionarioVociparametri.idparametro " & _
                " from QuestionarioVociparametri " & _
                " inner join Questionarioparametri on Questionarioparametri.idparametro=QuestionarioVociparametri.idparametro " & _
                " where Questionarioparametri.idparametro=" & idParametro & " and QuestionarioVociparametri.attivo=1 "

        ChiudiDataReader(dtrgenerico)
        dataTable = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        Dim nomeGruppo As String = tipoParametro & idParametro.ToString
        If (sceltaMultipla = False) Then
            For Each dataRow In dataTable.Rows
                Dim radioButton As RadioButton = New RadioButton
                counter = counter + 1
                If tipoParametro = 1 Then
                    radioButton.ID = counter & "cs"
                Else
                    radioButton.ID = counter & "cm"
                End If
                radioButton.ID = radioButton.ID & dataRow.Item("idparametro") & "X"
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
                If tipoParametro = 1 Then
                    checkBox.ID = counter & "cs"
                Else
                    checkBox.ID = counter & "cm"
                End If
                checkBox.ID = checkBox.ID & dataRow.Item("idparametro") & "X"
                checkBox.CssClass = "textbox"
                checkBox.Width = New Unit(400)
                checkBox.AutoPostBack = False
                checkBox.Text = " " & dataRow.Item("voce").ToString().ToUpper(System.Globalization.CultureInfo.CurrentCulture)
                checkBox.ValidationGroup = nomeGruppo
                If txtBloccaMaschera.Value = "Si" Then
                    checkBox.Enabled = False
                End If
                Dim labelIdVoce As Label = New Label
                labelIdVoce.ID = "l" & checkBox.ID
                labelIdVoce.Text = dataRow.Item("idvoce")
                labelIdVoce.Visible = False
                Dim labelCheckBoxId As Label = New Label
                labelCheckBoxId.ID = "lp" & checkBox.ID
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
                    datiVerificaQuestionario.TipoOggetto = TipoOggetto.CheckBox
                    datiVerificaQuestionario.Gruppo = nomeGruppo
                    datiVerificaQuestionario.Descrizione = descrizioneGruppo
                    infoCheck.Add(datiVerificaQuestionario)
                End If
            Next
        End If
        AggiungiRigaVuota(contenitore)
        ChiudiDataReader(dtrgenerico)
    End Sub
    Private Sub PopolaMaschera(ByVal idbando As Integer)
        Dim s As Integer
        Dim j As Integer
        Dim tblcel As TableCell
        Dim i As Integer
        Dim appo As String
        Dim nomech As String

        j = CInt(txtnome.Value)

        strsql = "select QuestionarioVociParametri.idvoce, QuestionarioVociParametri.idparametro, " 'QuestionariostoricoProgetti.notestorico, QuestionariostoricoProgetti.NumeroVolontari, QuestionariostoricoProgetti.NumeroFormatori " & _
        strsql = strsql & "QuestionariostoricoProgetti.IDStorico, "
        strsql = strsql & "QuestionariostoricoProgetti.IdBandoAttività, "
        strsql = strsql & "QuestionariostoricoProgetti.Username, "
        strsql = strsql & "dbo.FormatoData(QuestionariostoricoProgetti.DataInserimento) as DataInserimento, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroFormatori, "
        strsql = strsql & "QuestionariostoricoProgetti.EnteFormatore, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroFormatoriM, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroFormatoriF, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroFormatoriDiploma, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroFormatoriLaurea, "

        strsql = strsql & "QuestionariostoricoProgetti.NumeroFormatoriCorso, "
        strsql = strsql & "QuestionariostoricoProgetti.ProgettiConFormazioneEntro180GG, "
        strsql = strsql & "QuestionariostoricoProgetti.ProgettiConFormazioneEntro270GG, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroVolontariEnte, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroVolontariAltro, "
        strsql = strsql & "QuestionariostoricoProgetti.FrontalePerc, "
        strsql = strsql & "QuestionariostoricoProgetti.DinamichePerc, "
        strsql = strsql & "QuestionariostoricoProgetti.Distanza, "

        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiPattoFormativo, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiObiezioneCoscienza, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiDifesaPatria, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiCartaImpegnoEtico, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiFormazioneCivica, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiFormeCittadinanza, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiProtezioneCivile, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiVolNelServCiv, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiPresentazioneEnte, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiLavoroPerProgetti, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiOrganizzazioneSCN, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiRapportiEntiEVolontari, "
        strsql = strsql & "QuestionariostoricoProgetti.NumeroEspertiComunicazioneInterpersonale "
        strsql = strsql & " from QuestionarioVociParametri"
        strsql = strsql & " inner join Questionariostoricovoci on (Questionariostoricovoci.idvoce=QuestionarioVociParametri.idvoce) "
        strsql = strsql & " inner join QuestionariostoricoProgetti on (Questionariostoricovoci.idStorico=QuestionarioStoricoProgetti.idStorico) "
        strsql = strsql & " inner join BandiAttività ON QuestionarioStoricoProgetti.IdBandoAttività = BandiAttività.IdBandoAttività "
        strsql = strsql & " where QuestionariostoricoProgetti.TipoFormazioneGenerale=" & Request.QueryString("TipoFormazioneGenerale") & " and idbando=" & idbando & " And idente=" & Session("idente")

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        Do While dtrgenerico.Read
            For s = 1 To j
                nomech = s & "cs" & dtrgenerico("idparametro") & "X"
                ValorizzaCheck(nomech, dtrgenerico("idvoce"), dtrgenerico("idparametro"))
                nomech = s & "cm" & dtrgenerico("idparametro") & "X"
                ValorizzaCheck(nomech, dtrgenerico("idvoce"), dtrgenerico("idparametro"))

                txtFormazioneConclusaEntro180GG.Text = IIf(Not IsDBNull(dtrgenerico("ProgettiConFormazioneEntro180GG")), dtrgenerico("ProgettiConFormazioneEntro180GG"), "0")
                txtFormazioneConclusaEntro270GG.Text = IIf(Not IsDBNull(dtrgenerico("ProgettiConFormazioneEntro270GG")), dtrgenerico("ProgettiConFormazioneEntro270GG"), "0")
                txtNumFormatori.Text = IIf(Not IsDBNull(dtrgenerico("NumeroFormatori")), dtrgenerico("NumeroFormatori"), "0")
                txtDenominazioneEnte.Text = IIf(Not IsDBNull(dtrgenerico("EnteFormatore")), dtrgenerico("EnteFormatore"), "")
                txtNumeroFormatoriM.Text = IIf(Not IsDBNull(dtrgenerico("NumeroFormatoriM")), dtrgenerico("NumeroFormatoriM"), "0")
                txtNumeroFormatoriF.Text = IIf(Not IsDBNull(dtrgenerico("NumeroFormatoriF")), dtrgenerico("NumeroFormatoriF"), "0")
                txtNumeroFormatoriDiplomati.Text = IIf(Not IsDBNull(dtrgenerico("NumeroFormatoriDiploma")), dtrgenerico("NumeroFormatoriDiploma"), "0")
                txtNumeroFormatoriLaureati.Text = IIf(Not IsDBNull(dtrgenerico("NumeroFormatoriLaurea")), dtrgenerico("NumeroFormatoriLaurea"), "0")
                txtNumeroFormatoriCheHannoFrequentato.Text = IIf(Not IsDBNull(dtrgenerico("NumeroFormatoriCorso")), dtrgenerico("NumeroFormatoriCorso"), "0")
                txtNumeroVolontariDelVostroEnte.Text = IIf(Not IsDBNull(dtrgenerico("NumeroVolontariEnte")), dtrgenerico("NumeroVolontariEnte"), "0")
                txtNumeroVolontariAltriEnti.Text = IIf(Not IsDBNull(dtrgenerico("NumeroVolontariAltro")), dtrgenerico("NumeroVolontariAltro"), "0")
                txtLezioneFrontale.Text = IIf(Not IsDBNull(dtrgenerico("FrontalePerc")), dtrgenerico("FrontalePerc"), "0")
                txtDinamicheNonFormali.Text = IIf(Not IsDBNull(dtrgenerico("DinamichePerc")), dtrgenerico("DinamichePerc"), "0")
                txtFormazioneADistanza.Text = IIf(Not IsDBNull(dtrgenerico("Distanza")), dtrgenerico("Distanza"), "0")

                NumeroEspertiPattoFormativo.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiPattoFormativo")), dtrgenerico("NumeroEspertiPattoFormativo"), "0")
                NumeroEspertiObiezioneCoscienza.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiObiezioneCoscienza")), dtrgenerico("NumeroEspertiObiezioneCoscienza"), "0")
                NumeroEspertiDifesaPatria.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiDifesaPatria")), dtrgenerico("NumeroEspertiDifesaPatria"), "0")
                NumeroEspertiCartaImpegnoEtico.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiCartaImpegnoEtico")), dtrgenerico("NumeroEspertiCartaImpegnoEtico"), "0")
                NumeroEspertiFormazioneCivica.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiFormazioneCivica")), dtrgenerico("NumeroEspertiFormazioneCivica"), "0")
                NumeroEspertiFormeCittadinanza.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiFormeCittadinanza")), dtrgenerico("NumeroEspertiFormeCittadinanza"), "0")
                NumeroEspertiProtezioneCivile.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiProtezioneCivile")), dtrgenerico("NumeroEspertiProtezioneCivile"), "0")
                NumeroEspertiVolNelServCiv.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiVolNelServCiv")), dtrgenerico("NumeroEspertiVolNelServCiv"), "0")
                NumeroEspertiPresentazioneEnte.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiPresentazioneEnte")), dtrgenerico("NumeroEspertiPresentazioneEnte"), "0")
                NumeroEspertiLavoroPerProgetti.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiLavoroPerProgetti")), dtrgenerico("NumeroEspertiLavoroPerProgetti"), "0")
                NumeroEspertiOrganizzazioneSCN.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiOrganizzazioneSCN")), dtrgenerico("NumeroEspertiOrganizzazioneSCN"), "0")
                NumeroEspertiRapportiEntiEVolontari.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiRapportiEntiEVolontari")), dtrgenerico("NumeroEspertiRapportiEntiEVolontari"), "0")
                NumeroEspertiComunicazioneInterpersonale.Text = IIf(Not IsDBNull(dtrgenerico("NumeroEspertiComunicazioneInterpersonale")), dtrgenerico("NumeroEspertiComunicazioneInterpersonale"), "0")


            Next
        Loop
        dtrgenerico.Close()
        dtrgenerico = Nothing
    End Sub
    Private Sub ValorizzaCheck(ByVal NomeCheck As String, ByVal idvoce As String, ByVal parametro As String)
        Dim lbl As Label = DirectCast(panelAnagrafica.FindControl("l" & NomeCheck), Label)
        Dim lblp As Label = DirectCast(panelAnagrafica.FindControl("lp" & NomeCheck), Label)

        Dim check As CheckBox = TryCast(panelAnagrafica.FindControl(NomeCheck), CheckBox)
        Dim radiobutton As RadioButton = TryCast(panelAnagrafica.FindControl(NomeCheck), RadioButton)


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
        lblErrore.Text = String.Empty
        lblMessaggio.Text = String.Empty
        If (ValidaDati() = False) Then
            Exit Sub
        End If

        Dim s As Integer
        Dim j As Integer
        Dim tblcel As TableCell
        Dim i As Integer
        Dim appo As String
        Dim nomech As String
        Dim idStorico As String
        Dim idStoricoCanc As String
        Dim dtBandoAttivita As DataTable
        Dim iAtt As Integer
        Dim dtDomanda As DataTable

        Dim k As Integer = 0
        Dim ZZ As Integer
        j = CInt(txtnome.Value)
        'estraggo le istanze di presentazione di tutti i progetti dell'enete che hanno volontari con ore formazione caricate
        strsql = " SELECT DISTINCT BandiAttività.IdBandoAttività " _
               & " FROM entità INNER JOIN " _
               & " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " _
               & " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " _
               & " BandiAttività INNER JOIN " _
               & " attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " _
               & " inner join AttivitàFormazioneGenerale on AttivitàFormazioneGenerale.IdAttività  = attività.IDAttività " _
               & " WHERE AttivitàFormazioneGenerale.TipoFormazioneGenerale=" & Request.QueryString("TipoFormazioneGenerale") & " and (ISNULL(entità.OreFormazione, 0) > 0) AND (BandiAttività.IdBando = " & Session("idbando") & ") AND (BandiAttività.IdEnte = " & Session("idente") & ")"
        dtBandoAttivita = ClsServer.CreaDataTable(strsql, False, Session("conn"))

        For iAtt = 0 To dtBandoAttivita.Rows.Count - 1
            'cancello il questionario precedente
            'prendo l' idstorico del progetto
            strsql = "Select idstorico as maxid from QuestionarioStoricoProgetti Where TipoFormazioneGenerale=" & Request.QueryString("TipoFormazioneGenerale") & " and IdBandoAttività=" & dtBandoAttivita.Rows(iAtt).Item(0)
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrgenerico.Read()

            If dtrgenerico.HasRows = True Then
                idStoricoCanc = dtrgenerico("maxid")
            End If
            dtrgenerico.Close()
            dtrgenerico = Nothing

            If idStoricoCanc <> "" Then
                'cancello i record nella tabella QuestionarioStoricoVoci
                strsql = "Delete from QuestionarioStoricoVoci Where IdStorico=" & idStoricoCanc
                ClsServer.EseguiSqlClient(strsql, Session("conn"))

                'cancello i record nella tabella QuestionarioStoricoProgetti
                strsql = "Delete from QuestionarioStoricoNote Where IdStorico=" & idStoricoCanc
                ClsServer.EseguiSqlClient(strsql, Session("conn"))

                'cancello i record nella tabella QuestionarioStoricoProgetti
                strsql = "Delete from QuestionarioStoricoProgetti Where IdStorico=" & idStoricoCanc
                ClsServer.EseguiSqlClient(strsql, Session("conn"))

                idStoricoCanc = ""
            End If
           

            'inserisco il nuovo questionario
            strsql = "Insert into QuestionarioStoricoProgetti "
            strsql = strsql & "(IdBandoAttività, "
            strsql = strsql & "Username, "
            strsql = strsql & "DataInserimento, "
            strsql = strsql & "NumeroFormatori, "
            strsql = strsql & "EnteFormatore, "
            strsql = strsql & "NumeroFormatoriM, "
            strsql = strsql & "NumeroFormatoriF, "
            strsql = strsql & "NumeroFormatoriDiploma, "
            strsql = strsql & "NumeroFormatoriLaurea, "
            strsql = strsql & "NumeroFormatoriCorso, "
            strsql = strsql & "NumeroVolontari, "
            strsql = strsql & "NumeroVolontariEnte, "
            strsql = strsql & "NumeroVolontariAltro, "
            strsql = strsql & "FrontalePerc, "
            strsql = strsql & "DinamichePerc, "
            strsql = strsql & "Distanza, "
            strsql = strsql & "ProgettiConFormazioneEntro180GG, "
            strsql = strsql & "ProgettiConFormazioneEntro270GG,"
            strsql = strsql & "TipoFormazioneGenerale,"

            strsql = strsql & "NumeroEspertiPattoFormativo,"
            strsql = strsql & "NumeroEspertiObiezioneCoscienza,"
            strsql = strsql & "NumeroEspertiDifesaPatria,"
            strsql = strsql & "NumeroEspertiCartaImpegnoEtico,"
            strsql = strsql & "NumeroEspertiFormazioneCivica,"
            strsql = strsql & "NumeroEspertiFormeCittadinanza,"
            strsql = strsql & "NumeroEspertiProtezioneCivile,"
            strsql = strsql & "NumeroEspertiVolNelServCiv,"
            strsql = strsql & "NumeroEspertiPresentazioneEnte,"
            strsql = strsql & "NumeroEspertiLavoroPerProgetti,"
            strsql = strsql & "NumeroEspertiOrganizzazioneSCN,"
            strsql = strsql & "NumeroEspertiRapportiEntiEVolontari,"
            strsql = strsql & "NumeroEspertiComunicazioneInterpersonale) "
            strsql = strsql & "VALUES "
            strsql = strsql & "(" & dtBandoAttivita.Rows(iAtt).Item(0) & ", "
            strsql = strsql & "'" & Session("Utente") & "', "
            strsql = strsql & "GetDate(), "
            strsql = strsql & txtNumFormatori.Text & ", "
            strsql = strsql & "'" & Replace(txtDenominazioneEnte.Text, "'", "''") & "', "
            strsql = strsql & txtNumeroFormatoriM.Text & ", "
            strsql = strsql & txtNumeroFormatoriF.Text & ", "
            strsql = strsql & txtNumeroFormatoriDiplomati.Text & ", "
            strsql = strsql & txtNumeroFormatoriLaureati.Text & ", "
            strsql = strsql & txtNumeroFormatoriCheHannoFrequentato.Text & ", "
            strsql = strsql & CInt(txtNumeroVolontariDelVostroEnte.Text) + CInt(txtNumeroVolontariAltriEnti.Text) & ", "
            strsql = strsql & txtNumeroVolontariDelVostroEnte.Text & ", "
            strsql = strsql & txtNumeroVolontariAltriEnti.Text & ", "

            strsql = strsql & CInt(txtLezioneFrontale.Text) & ", "
            strsql = strsql & CInt(txtDinamicheNonFormali.Text) & ", "
            strsql = strsql & CInt(txtFormazioneADistanza.Text) & ", "
            strsql = strsql & txtFormazioneConclusaEntro180GG.Text & ", "
            strsql = strsql & txtFormazioneConclusaEntro270GG.Text & ","
            strsql = strsql & Request.QueryString("TipoFormazioneGenerale") & ","

            strsql = strsql & NumeroEspertiPattoFormativo.Text & ","
            strsql = strsql & NumeroEspertiObiezioneCoscienza.Text & ","
            strsql = strsql & NumeroEspertiDifesaPatria.Text & ","
            strsql = strsql & NumeroEspertiCartaImpegnoEtico.Text & ","
            strsql = strsql & NumeroEspertiFormazioneCivica.Text & ","
            strsql = strsql & NumeroEspertiFormeCittadinanza.Text & ","
            strsql = strsql & NumeroEspertiProtezioneCivile.Text & ","
            strsql = strsql & NumeroEspertiVolNelServCiv.Text & ","
            strsql = strsql & NumeroEspertiPresentazioneEnte.Text & ","
            strsql = strsql & NumeroEspertiLavoroPerProgetti.Text & ","
            strsql = strsql & NumeroEspertiOrganizzazioneSCN.Text & ","
            strsql = strsql & NumeroEspertiRapportiEntiEVolontari.Text & ","
            strsql = strsql & NumeroEspertiComunicazioneInterpersonale.Text & ") "

            ClsServer.EseguiSqlClient(strsql, Session("conn"))

            strsql = "Select @@identity as MaxId from QuestionarioStoricoProgetti"
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrgenerico.Read()

            If dtrgenerico.HasRows = True Then
                idStorico = dtrgenerico("MaxId")
            End If
            ChiudiDataReader(dtrgenerico)


            ' -------------------------------SIMO E ANTO--------------------------------------

            strsql = "Select Questionarioparametri.idparametro "
            'strsql = strsql & "count(QuestionarioVociparametri.idvoce) as TotRispo "
            strsql = strsql & "from QuestionarioVociparametri "
            strsql = strsql & "inner join Questionarioparametri on Questionarioparametri.idparametro=QuestionarioVociparametri.idparametro "
            strsql = strsql & "where QuestionarioVociparametri.attivo=1 and RevisioneFormazione=" & Request.QueryString("RevisioneFormazione") & ""
            strsql = strsql & "group by Questionarioparametri.idparametro"

            dtDomanda = ClsServer.CreaDataTable(strsql, False, Session("conn"))

            For k = 0 To dtDomanda.Rows.Count - 1
                s = 1
                For s = 1 To j
                    'dtDomanda.Rows(k).Item("TotRispo")
                    nomech = s & "cs" & dtDomanda.Rows(k).Item("idparametro") & "X" '& dtDomanda.Rows(k).Item("TotRispo")
                    Inserisci("s", panelAnagrafica, nomech, idStorico)
                    nomech = s & "cm" & dtDomanda.Rows(k).Item("idparametro") & "X" '& dtDomanda.Rows(k).Item("TotRispo")
                    Inserisci("s", panelAnagrafica, nomech, idStorico)

                Next

            Next

        Next

        lblMessaggio.Text = "Inserimento questionario progetti terminato con successo."
        cmdStampaVQ.Visible = True

    End Sub
    Private Sub Inserisci(ByVal tipo As String, ByRef panel As Panel, ByVal nomeCheck As String, ByVal idStorico As Double)
        Dim lbl As Label = DirectCast(panel.FindControl("l" & nomeCheck), Label)
        Dim lbl1 As Label = DirectCast(panel.FindControl("lpu" & nomeCheck), Label)
        Dim check As CheckBox = TryCast(panel.FindControl(nomeCheck), CheckBox)
        Dim radiobutton As RadioButton = TryCast(panel.FindControl(nomeCheck), RadioButton)
        Dim aggiornaStorico As Boolean = False

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
            strsql = "insert into QuestionarioStoricoVoci(idStorico,idvoce,punteggio) " & _
            " Select " & idStorico & "," & lbl.Text & ",0 from questionariovociparametri where idvoce=" & lbl.Text & ""
            ClsServer.EseguiSqlClient(strsql, Session("conn"))
            ChiudiDataReader(dtrgenerico)
        End If

    End Sub

    Private Sub VerificaGiorni(ByVal idBando As Integer)
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 18/07/2006
        'DESCRIZONE: controllo date progetto per effettuare il blocco del questionario per la modifica
        Dim strSQL As String
        Dim iGiorni As Integer

        iGiorni = 180                    'Gestione temporale modifica questionario ore <=180 gg


        strSQL = "SELECT datediff(dd,MAX(attività.DataInizioAttività),getdate()) as DiffGG " & _
                " FROM bando INNER JOIN " & _
                " BandiAttività ON bando.IDBando = BandiAttività.IdBando INNER JOIN " & _
                " attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività " & _
                " WHERE (bando.IDBando = " & idBando & ")"
        'and dbo.Formazione_TerminiQuestionario(BandiAttività.IdBandoAttività, getdate()) = 'SI'
        dtrgenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrgenerico.Read()

        If dtrgenerico.HasRows Then
            If dtrgenerico("DiffGG") > iGiorni Then
                txtBloccaMaschera.Value = "Si"
                cmdConferma.Visible = False
                ImpostaCampiInSolaLettura(True)
            Else
                txtBloccaMaschera.Value = "No"
                cmdConferma.Visible = True
                ImpostaCampiInSolaLettura(False)
            End If

        End If

        dtrgenerico.Close()
        dtrgenerico = Nothing

        'Antonello Di Croce Controllo inserito in fase di modifica nel caso in cui ci sia stata una proroga 
        'If Session("tipoazione") = "Modifica" Then
        strSQL = "select dbo.Formazione_TerminiQuestionario_2(BandiAttività.IdBandoAttività, getdate()," & Request.QueryString("TipoFormazioneGenerale") & ") as risposta from BandiAttività"
        strSQL = strSQL & " where IdBando= '" & Session("idbando") & "'  "
        strSQL = strSQL & " and idente=" & Session("idente")
        dtrgenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrgenerico.Read()
        Dim verifica As String
        verifica = dtrgenerico("Risposta")
        dtrgenerico.Close()
        dtrgenerico = Nothing
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
        lblMessaggio.Text = String.Empty
        lblErrore.Text = String.Empty
        Dim iMaxId As Integer = 0
        strsql = "SELECT COALESCE(Max(IdStorico),0) as MaxIdS FROM  QuestionarioStoricoProgetti INNER JOIN " & _
                " BandiAttività ON QuestionarioStoricoProgetti.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                "  WHERE QuestionarioStoricoProgetti.TipoFormazioneGenerale=" & Request.QueryString("TipoFormazioneGenerale") & _
                " and (BandiAttività.IdBando = " & Session("idbando") & ") and (BandiAttività.Idente = " & Session("idente") & ")"

        ChiudiDataReader(dtrgenerico)

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            iMaxId = dtrgenerico("MaxIdS")
        End If
        ChiudiDataReader(dtrgenerico)
        If iMaxId = 0 Then
            lblErrore.Text = "Effettuare prima il Salvataggio per richiedere la Stampa"
        Else
        'Mando in esecuzione la stampa della valutazione di qualità
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('WfrmReportistica.aspx?sTipoStampa=43&IdBando=" & Session("idbando") & "&IdStorico=" & iMaxId & "','report','height=800,width=800,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
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
        ChiudiDataReader(dtrgenerico)
    End Sub
    Private Sub ImpostaCampiInSolaLettura(ByVal isSolaLettura As Boolean)
        txtFormazioneConclusaEntro180GG.ReadOnly = isSolaLettura
        txtFormazioneConclusaEntro270GG.ReadOnly = isSolaLettura
        txtNumFormatori.ReadOnly = isSolaLettura
        txtDenominazioneEnte.ReadOnly = isSolaLettura
        txtNumeroFormatoriM.ReadOnly = isSolaLettura
        txtNumeroFormatoriF.ReadOnly = isSolaLettura
        txtNumeroFormatoriDiplomati.ReadOnly = isSolaLettura
        txtNumeroFormatoriLaureati.ReadOnly = isSolaLettura
        txtNumeroFormatoriCheHannoFrequentato.ReadOnly = isSolaLettura
        txtNumeroVolontariDelVostroEnte.ReadOnly = isSolaLettura
        txtNumeroVolontariAltriEnti.ReadOnly = isSolaLettura
        txtLezioneFrontale.ReadOnly = isSolaLettura
        txtDinamicheNonFormali.ReadOnly = isSolaLettura
        txtFormazioneADistanza.ReadOnly = isSolaLettura
        NumeroEspertiPattoFormativo.ReadOnly = isSolaLettura
        NumeroEspertiObiezioneCoscienza.ReadOnly = isSolaLettura
        NumeroEspertiDifesaPatria.ReadOnly = isSolaLettura
        NumeroEspertiCartaImpegnoEtico.ReadOnly = isSolaLettura
        NumeroEspertiFormazioneCivica.ReadOnly = isSolaLettura
        NumeroEspertiFormeCittadinanza.ReadOnly = isSolaLettura
        NumeroEspertiProtezioneCivile.ReadOnly = isSolaLettura
        NumeroEspertiVolNelServCiv.ReadOnly = isSolaLettura
        NumeroEspertiPresentazioneEnte.ReadOnly = isSolaLettura
        NumeroEspertiLavoroPerProgetti.ReadOnly = isSolaLettura
        NumeroEspertiOrganizzazioneSCN.ReadOnly = isSolaLettura
        NumeroEspertiRapportiEntiEVolontari.ReadOnly = isSolaLettura
        NumeroEspertiComunicazioneInterpersonale.ReadOnly = isSolaLettura
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
        Dim lezioneFrontale As Integer = 0
        Dim dinamicheNonFormali As Integer = 0
        Dim formazioneADistanza As Integer = 0


        'Validazione campi testo
        Dim numeroFormatori As Integer = 0
        Dim numeroVolontari As Integer = 0
        If (Integer.TryParse(txtNumFormatori.Text, numeroFormatori) = False Or numeroFormatori = 0) Then
            lblErrore.Text = lblErrore.Text + "Inserire il numero di formatori impiegati nella formazione generale dei volontari.</br>"
            validazioneOk = False
        Else
            Dim formatoriM As Integer = 0
            Dim formatoriF As Integer = 0
            Integer.TryParse(txtNumeroFormatoriM.Text, formatoriM)
            Integer.TryParse(txtNumeroFormatoriF.Text, formatoriF)
            If (formatoriM + formatoriF <> numeroFormatori) Then
                lblErrore.Text = lblErrore.Text + "Il numero totale dei formatori deve essere pari alla somma dei formatori e delle formatrici. </br>"
                validazioneOk = False
            End If
        End If

        Integer.TryParse(txtLezioneFrontale.Text, lezioneFrontale)
        Integer.TryParse(txtDinamicheNonFormali.Text, dinamicheNonFormali)
        Integer.TryParse(txtFormazioneADistanza.Text, formazioneADistanza)




        If ((lezioneFrontale + dinamicheNonFormali + formazioneADistanza > 0) And (lezioneFrontale + dinamicheNonFormali + formazioneADistanza < 100)) Then
            lblErrore.Text = lblErrore.Text + "La somma delle percentuali delle metodologie didattiche utilizzate deve essere pari a 0 o a 100. </br>"
            validazioneOk = False
        End If

        Dim formazioneConclusaEntro180GG As Integer = 0
        Dim formazioneConclusaEntro270GG As Integer = 0
        If Not (Integer.TryParse(txtFormazioneConclusaEntro180GG.Text, formazioneConclusaEntro180GG)) Then
            lblErrore.Text = lblErrore.Text + "Inserire un numero valido per 'Formazione conclusa entro 180 giorni dall’avvio del progetto. Totale progetti'.</br>"
            validazioneOk = False
        End If
        If Not (Integer.TryParse(txtFormazioneConclusaEntro270GG.Text, formazioneConclusaEntro270GG)) Then
            lblErrore.Text = lblErrore.Text + "Inserire un numero valido per 'Formazione conclusa entro 270 giorni dall’avvio del progetto. Totale progetti'.</br>"
            validazioneOk = False
        End If

        Dim espertiFormazione As Integer = 0
        'Dim msgErrore As String = "Inserire un numero valido per 'l’identità del gruppo in formazione e patto formativo. Numero Esperti'.</br>"
        Dim msgErrore As String = ""

        VerificaIntero(NumeroEspertiPattoFormativo.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'dall’obiezione di coscienza al servizio civile nazionale. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiObiezioneCoscienza.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'il dovere di difesa della Patria - difesa civile non armata e nonviolenta. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiDifesaPatria.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'la normativa vigente e la Carta di impegno etico. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiCartaImpegnoEtico.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'la formazione civica. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiFormazioneCivica.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'le forme di cittadinanza. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiFormeCittadinanza.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'la protezione civile. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiProtezioneCivile.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'la rappresentanza dei volontari nel servizio civile. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiVolNelServCiv.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'presentazione dell’Ente. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiPresentazioneEnte.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'il lavoro per progetti. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiLavoroPerProgetti.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'l’organizzazione del servizio civile e le sue figure. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiOrganizzazioneSCN.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'disciplina dei rapporti tra enti e volontari del servizio civile nazionale. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiRapportiEntiEVolontari.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido per 'comunicazione interpersonale e gestione dei conflitti. Numero Esperti'.</br>"

        VerificaIntero(NumeroEspertiComunicazioneInterpersonale.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero valido.</br>"

        'Antonello
        VerificaIntero(txtLezioneFrontale.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero Intero per le percentuali di Lezione frontale.</br>"

        VerificaIntero(txtDinamicheNonFormali.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero Intero per le percentuali di Dinamiche non formali.</br>"

        VerificaIntero(txtFormazioneADistanza.Text, msgErrore, validazioneOk)
        msgErrore = "Inserire un numero Intero per le percentuali di Formazione a distanza.</br>"
        '------------------------------------
        'Validazione checkBox e RadioButton
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
                    check = TryCast(panelAnagrafica.FindControl(itemGruppo.Id), CheckBox)
                    If check.Checked = True Then
                        checked = True
                        Exit For
                    Else
                        checked = False
                    End If
                Else
                    radiobutton = TryCast(panelAnagrafica.FindControl(itemGruppo.Id), RadioButton)
                    If radiobutton.Checked = True Then
                        checked = True
                        Exit For
                    Else
                        checked = False
                    End If
                End If
            Next
            If (checked = False) Then
                lblErrore.Text = lblErrore.Text + "Selezionare almeno un valore per '" + itemGruppo.Descrizione + "'.</br>"
                validazioneOk = False
                checked = True
            End If

        Next

        Return validazioneOk
    End Function
    Private Sub VerificaIntero(ByVal valore As String, ByVal testoErrore As String, ByRef validazioneOk As Boolean)
        Dim interoValido As Integer = 0

        If Not (Integer.TryParse(valore, interoValido)) Then
            lblErrore.Text = lblErrore.Text + testoErrore
            validazioneOk = False
        End If
    End Sub
    Private Function VerificaValiditaCampi() As Boolean

    End Function
End Class
