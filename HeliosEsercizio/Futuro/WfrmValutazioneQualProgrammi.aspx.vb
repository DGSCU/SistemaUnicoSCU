Imports System.Data.SqlClient
Imports System.Drawing
Public Class WfrmValutazioneQualProgrammi
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsgenerico As DataSet
    Dim counter As String
    Public tipoProgetto As Integer
    Const SEZIONE_CARATTERITICHE_PROGETTO As String = "CaratteristicheProgetto"
    Const SEZIONE_CARATTERITICHE_ORGANIZZATIVE As String = "CaratteristicheOrganizzative"
    Const SEZIONE_CARATTERITICHE_CONOSCENZE_ACQUISITE As String = "CaratteristicheConAcquisite"
    Const SEZIONE_COERENZA_GENERALE As String = "CoerenzaGenerale"
    Const SEZIONE_MINORI_OPPORTUNITA As String = "MinoriOpportunita"
    Const SEZIONE_PAESE_UE As String = "PaeseUE"
    Const SEZIONE_TUTORAGGIO As String = "Tutoraggio"
    Const SEZIONE_NOTE_RISERVATE As String = "NoteRiservate"
    Const SENDER_PULSANTE_CONFERMA As String = "cmdConferma"
    Const SENDER_PULSANTE_SALVA As String = "cmdSalva"
    Dim valoreMassimoDeflettoreSanzione As Int32 = 10
    Dim valoreMassimoDeflettoreInfortuni As Int32 = 5
    Dim valoreMassimoDeflettoreInflazione As Int32 = 5
    Dim valoreMassimoDeflettoreCopertura As Int32 = 5
    Dim valoreMassimoPunteggioRegionale As Int32 = 20
    Dim classeHelper As Helper = New Helper()
    Dim infoCheck As List(Of DatiValutazioneQualita)


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
    Private Sub VerificaVisualizzazione()
        strsql = "select VisualizzaValQual from UtentiUNSC where username='" & Session("Utente") & "'"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        Session("VisQualPosizione") = dtrgenerico("VisualizzaValQual")
        ChiudiDataReader(dtrgenerico)
    End Sub
    Private Sub CancellaMessaggi()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty

    End Sub
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub


#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        VerificaSessione()
        ChiudiDataReader(dtrgenerico)
        VerificaVisualizzazione()



        If IsPostBack = False Then

            Session.Remove("DatiValutazioneQualita")
            infoCheck = New List(Of DatiValutazioneQualita)

            If Session("TipoUtente") = "U" Then
                'txtDataSeduta.Text = Session("dataserver")
            Else
                lblDataSeduta.Visible = False
                txtDataSeduta.Visible = False
            End If

            If hf_IdProgramma.Value = "" Then
                hf_IdProgramma.Value = Request.QueryString("IdProgramma")
                'Context.Items("idprogetto")

                Session("IdProgramma") = hf_IdProgramma.Value
                hf_Pagina.Value = IIf(Not IsNothing(Request.QueryString("Pagina")), Request.QueryString("Pagina"), "")
                txtDataSeduta.Text = Session("dataserver")
            End If
            strsql = "select programmi.idprogramma, statibando.invalutazione,statiprogrammi.idstatoprogramma,"
            strsql = strsql & "dbo.formatodata(programmi.dataseduta) as DataSeduta,  " &
                            " isnull(programmi.confermavalutazione,0) as confermavalutazione, statiprogrammi.statoprogramma " &
                            " from programmi " &
                            " inner join bandiprogrammi  on programmi.idbandoprogramma=bandiprogrammi.idbandoprogramma " &
                            " inner join bando  on bando.idbando=bandiprogrammi.idbando " &
                            " inner join statibando  on bando.idstatobando=statibando.idstatobando " &
                            " inner join statiprogrammi  on programmi.idstatoprogramma=statiprogrammi.idstatoprogramma " &
                            " where programmi.idprogramma=" & hf_IdProgramma.Value & ""
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrgenerico.Read()

            If IsDBNull(dtrgenerico("DataSeduta")) = True Then
                'txtDataSeduta.Text = CDate(Session("dataserver"))
                txtDataSeduta.Text = ""
            Else
                txtDataSeduta.Text = dtrgenerico("DataSeduta")
            End If

            'If dtrgenerico("NoteRiservate") = True Then
            '    lblNoteRiservate.Text = " Si"
            'Else
            '    lblNoteRiservate.Text = " No"
            'End If
            'If dtrgenerico("IdRegioneCompetenza") <> 22 Then
            '    panelNoteRiservate.Visible = False
            '    panelNoteRiservateDisabilitate.Visible = True
            'Else
            '    panelNoteRiservate.Visible = True
            '    panelNoteRiservateDisabilitate.Visible = False
            'End If


            If dtrgenerico("idstatoprogramma") = 4 Or dtrgenerico("idstatoprogramma") = 5 Or dtrgenerico("idstatoprogramma") = 6 Or dtrgenerico("idstatoprogramma") = 7 Then
                hf_BloccaMaschera.Value = "Si"
                PersonalizzaMaschera()
            Else
                hf_BloccaMaschera.Value = "No"
            End If
            If dtrgenerico("confermavalutazione") = "1" Then
                cmdSalva.Visible = False
            End If
            'ricordo il progetto è confermato o è da confermare
            Hdd_ConfermaValutazione.Value = dtrgenerico("confermavalutazione")
            '***********************************************************************************
            'lblinternet.Text = dtrgenerico("ControlloInternet")
            '***********************************************************************************
            ChiudiDataReader(dtrgenerico)

            
            Session("IdRegioneCompetenzaProgetto") = 22
            'ChiudiDataReader(dtrgenerico)
            imgElencoDocumentiProg.Visible = True
            'If ClsUtility.ForzaFascicoloInformaticoProgetti(Session("Utente"), Session("conn")) = True And Session("IdRegioneCompetenzaProgetto") = "22" Then
            '    imgAssociaDocumentiProg.Visible = True
            'End If
            '**********************************************************
            If hf_IdProgramma.Value <> "" Then
                strsql = "Select NumeroPostiNoVittoNoAlloggio, NumeroPostiVittoAlloggio, NumeroPostiVitto,punteggioTotale,statiprogrammi.idstatoprogramma,statiprogrammi.statoprogramma, " &
                " titolo + '(' + isnull(codiceprogramma,'') + ')' as Titolo,denominazione + '(' + codiceregione + ')' as Ente,idente from programmi " &
                " inner join enti on programmi.IdEnteProponente=enti.idente " &
                " inner join statiprogrammi on (programmi.idstatoprogramma=statiprogrammi.idstatoprogramma)" &
                " where idprogramma=" & hf_IdProgramma.Value & ""
                ChiudiDataReader(dtrgenerico)
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                dtrgenerico.Read()
                Dim NumeroPostiNoVittoNoAlloggio As String
                Dim NumeroPostiVittoAlloggio As String
                Dim NumeroPostiVitto As String
                If dtrgenerico.HasRows = True Then
                    NumeroPostiNoVittoNoAlloggio = IIf(Not IsDBNull(dtrgenerico("NumeroPostiNoVittoNoAlloggio")), dtrgenerico("NumeroPostiNoVittoNoAlloggio"), 0)
                    NumeroPostiVittoAlloggio = IIf(Not IsDBNull(dtrgenerico("NumeroPostiVittoAlloggio")), dtrgenerico("NumeroPostiVittoAlloggio"), 0) 'dtrgenerico("NumeroPostiVittoAlloggio")
                    NumeroPostiVitto = IIf(Not IsDBNull(dtrgenerico("NumeroPostiVitto")), dtrgenerico("NumeroPostiVitto"), 0) 'dtrgenerico("NumeroPostiVitto")
                    Session("TotVol") = CInt(NumeroPostiNoVittoNoAlloggio) + CInt(NumeroPostiVittoAlloggio) + CInt(NumeroPostiVitto)
                    hf_StatoProgramma.Value = dtrgenerico("idstatoprogramma")
                    lblProgramma.Text = UCase(dtrgenerico("Titolo"))
                    lblEnte.Text = UCase(dtrgenerico("Ente"))
                    Session("IDEnte") = dtrgenerico("idEnte")
                    LblPuntegioTotaleRis.Text = IIf(Not IsDBNull(dtrgenerico("punteggioTotale")), dtrgenerico("punteggioTotale"), 0)
                End If
                ChiudiDataReader(dtrgenerico)
            End If
            hf_IdStorico.Value = IIf(Not IsNothing(Context.Items("idStorico")), Context.Items("idStorico"), "")
            If IsNothing(Context.Items("personalizza")) Then
                hf_Personalizza.Value = "Inserimento"
            Else
                hf_Personalizza.Value = "Modifica"
            End If
            ChiudiDataReader(dtrgenerico)
            If hf_IdProgramma.Value <> "" Then
                txtx.Value = 0

                CreaTabellaParametri(SEZIONE_CARATTERITICHE_PROGETTO, panelCaratteristicheProgramma)
                'CreaTabellaParametri(SEZIONE_CARATTERITICHE_ORGANIZZATIVE, panelCaratteristicheOrganizzative)
                'CreaTabellaParametri(SEZIONE_CARATTERITICHE_CONOSCENZE_ACQUISITE, panelCaratteristicheConAcquisite)
                CreaTabellaParametri(SEZIONE_COERENZA_GENERALE, panelCoerenzaGenerale)
                'CreaTabellaParametri(SEZIONE_MINORI_OPPORTUNITA, panelMinoriOpportunita)
                'CreaTabellaParametri(SEZIONE_PAESE_UE, panelPaeseUE)
                ' CreaTabellaParametri(SEZIONE_TUTORAGGIO, panelTutoraggio)

                'carico i dati dei punteggi
                PopolaMaschera()
                'tipoProgetto = TipologiaProgetti(hf_IdProgramma.Value)
                'If (tipoProgetto = 0) Then
                '    divTipoProgramma_0.Visible = True
                'ElseIf (tipoProgetto = 1 Or tipoProgetto = 3) Then
                '    divTipoProgramma_1_3.Visible = True
                'End If
            End If
            Session.Add("DatiValutazioneQualita", infoCheck)

        Else


            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            If hf_IdProgramma.Value <> "" Then
                txtx.Value = 0
                CreaTabellaParametri(SEZIONE_CARATTERITICHE_PROGETTO, panelCaratteristicheProgramma)
                'CreaTabellaParametri(SEZIONE_CARATTERITICHE_ORGANIZZATIVE, panelCaratteristicheOrganizzative)
                'CreaTabellaParametri(SEZIONE_CARATTERITICHE_CONOSCENZE_ACQUISITE, panelCaratteristicheConAcquisite)
                CreaTabellaParametri(SEZIONE_COERENZA_GENERALE, panelCoerenzaGenerale)
                'CreaTabellaParametri(SEZIONE_MINORI_OPPORTUNITA, panelMinoriOpportunita)
                'CreaTabellaParametri(SEZIONE_PAESE_UE, panelPaeseUE)
                'CreaTabellaParametri(SEZIONE_TUTORAGGIO, panelTutoraggio)
                'tipoProgetto = TipologiaProgetti(hf_IdProgramma.Value)
                'If (tipoProgetto = 0) Then
                '    divTipoProgramma_0.Visible = True
                'ElseIf (tipoProgetto = 1 Or tipoProgetto = 3) Then
                '    divTipoProgramma_1_3.Visible = True
                'End If
            End If

        End If
        ChiudiDataReader(dtrgenerico)


        If Request.QueryString("VengoDa") = "AssociaVolontari" Then
            MascheraINLettura()
        End If



        'If posizione = 1 Then
        '    txtPuntRegio.Focus()
        'Else
        '    MaintainScrollPositionOnPostBack = True
        'End If
        MaintainScrollPositionOnPostBack = True



    End Sub
    Private Sub PersonalizzaMaschera()
        
        cmdConferma.Visible = False
        cmdSalva.Visible = False
       
        msgInfo.Text = "La Valutazione non può essere modificata perchè la graduatoria è gia stata prodotta e approvata"
    End Sub
    Private Sub CreaTabellaParametri(ByVal sezione As String, ByRef contenitore As Panel)

        Dim i As Integer
        Dim j As Integer
        Dim r As TableRow
        Dim c As TableCell
        Dim myRow As DataRow
        Dim Tab As DataTable
        Dim coll As Collection
        Dim XX As Integer


        'prelevo i Parametri a seconda della Nazionalità del gruppo e se sono atttivi o no 
        'FZ vecchia query che nn prendeva in considerazione la tabella ASSOCIABANDOVALUTAZIONE
        'strsql = "Select  parametri.* from parametri where attivo=1  "
        strsql = "SELECT parametriprogrammi.*,(select count(distinct gruppo) from vociparametriProgrammi x where x.idparametro = parametriprogrammi.idparametro) as NGruppi FROM "
        strsql = strsql & "parametriprogrammi INNER JOIN AssociaBandoValutazioneprogrammi "
        strsql = strsql & "ON parametriprogrammi.IDParametro = AssociaBandoValutazioneprogrammi.IdParametro INNER JOIN "
        strsql = strsql & "bando ON AssociaBandoValutazioneprogrammi.IdBando = bando.IDBando INNER JOIN "
        strsql = strsql & "Bandiprogrammi ON bando.IDBando = Bandiprogrammi.IdBando INNER JOIN "
        strsql = strsql & "programmi ON Bandiprogrammi.IdBandoprogramma = programmi.IDBandoprogramma "
        strsql = strsql & "WHERE (parametriprogrammi.Attivo = 1) "

        strsql = strsql & " and parametriprogrammi.Nazionale=1 "
        'If lblNazione.Text = "Italiana" Then
        '    strsql = strsql & " and parametri.Nazionale=1 "
        'Else
        '    strsql = strsql & " and parametri.Nazionale=0 "
        'End If

        If sezione = SEZIONE_CARATTERITICHE_PROGETTO Then
            strsql = strsql & " and parametriprogrammi.classePunteggio=0"
        End If
        'If sezione = SEZIONE_CARATTERITICHE_ORGANIZZATIVE Then
        '    strsql = strsql & " and parametri.classePunteggio=1"
        'End If
        'If sezione = SEZIONE_CARATTERITICHE_CONOSCENZE_ACQUISITE Then
        '    strsql = strsql & " and parametri.classePunteggio=2"
        'End If
        If sezione = SEZIONE_COERENZA_GENERALE Then
            strsql = strsql & " and parametriprogrammi.classePunteggio=1"
        End If
        'If sezione = SEZIONE_MINORI_OPPORTUNITA Then
        '    strsql = strsql & " and parametri.classePunteggio=4"
        'End If
        'If sezione = SEZIONE_PAESE_UE Then
        '    strsql = strsql & " and parametri.classePunteggio=5"
        'End If
        'If sezione = SEZIONE_TUTORAGGIO Then
        '    strsql = strsql & " and parametri.classePunteggio=6"
        'End If
        'FZ condizione where per prendere in considerazione l'ID del progetto
        strsql = strsql & "AND (programmi.IDprogramma = " & hf_IdProgramma.Value & ")"

        strsql = strsql & " Order by parametriprogrammi.Ordine "

        ChiudiDataReader(dtrgenerico)
        Tab = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        ' Creo Righe e celle
        Dim a As Integer = 1
        For Each myRow In Tab.Rows
            a = a + 1
            Dim img As System.Web.UI.WebControls.Image = New System.Web.UI.WebControls.Image
            img.ImageUrl = "images\i5.gif"
            'img.ID = "img" & tbl.ID
            img.ToolTip = myRow.Item("rationale")
            r = New TableRow
            c = New TableCell
            c.Controls.Add(img)
            Dim panelInterno As Panel = New Panel
            panelInterno.CssClass = "panel"
            Dim lblPanelInterno As Label = New Label
            lblPanelInterno.CssClass = "labelDati"
            panelInterno.ID = sezione + a.ToString
            Dim parametro = myRow.Item("parametro")
            Dim ordine = myRow.Item("Ordine")
            Dim sceltaMultipla As Boolean
            Select Case myRow.Item("tipo")
                Case 0
                    txtx.Value = CInt(txtx.Value) + 1
                    lblPanelInterno.Text = "(" & ordine & ")"
                    lblPanelInterno.Text = lblPanelInterno.Text & parametro & " [Scelta Multipla] <br/>"
                    lblPanelInterno.Text = lblPanelInterno.Text & " <br/>"
                    sceltaMultipla = True
                Case 1
                    txtx.Value = CInt(txtx.Value) + 1
                    lblPanelInterno.Text = "(" & ordine & ")"
                    lblPanelInterno.Text = lblPanelInterno.Text & parametro & " [Scelta Singola] <br/>"
                    lblPanelInterno.Text = lblPanelInterno.Text & " <br/>"
                    sceltaMultipla = False
                Case 2
                    txtx.Value = CInt(txtx.Value) + 4
                    lblPanelInterno.Text = "(" & ordine & ")"
                    lblPanelInterno.Text = lblPanelInterno.Text & parametro & " [Scelta Singola per gruppi] <br/>"
                    lblPanelInterno.Text = lblPanelInterno.Text & " <br/>"
                    sceltaMultipla = True
            End Select
            panelInterno.Controls.Add(lblPanelInterno)
            contenitore.Controls.Add(panelInterno)
            Dim numeroGruppo As Byte = 0

            If myRow.Item("tipo") = 2 Then

                For intGrup = 0 To myRow.Item("NGruppi") - 1
                    Dim label As Label = New Label
                    numeroGruppo = intGrup + 1
                    label.CssClass = "labelDati"
                    label.Text = "Gruppo " & numeroGruppo & "<br/>"
                    panelInterno.Controls.Add(label)
                    panelInterno.Controls.Add(classeHelper.CreaDivRigaVuota())
                    Dim descrizione As String = parametro & " - " & "Gruppo " & numeroGruppo
                    CreaTabellaVociParametriGruppo(panelInterno, myRow.Item("idParametro"), intGrup, sceltaMultipla, descrizione)
                    panelInterno.Controls.Add(classeHelper.CreaDivRigaVuota())
                Next
            Else
                CreaTabellaVociParametri(panelInterno, myRow.Item("idParametro"), myRow.Item("tipo"), sceltaMultipla, parametro)
            End If
        Next
        txtnome.Value = counter
        ChiudiDataReader(dtrgenerico)

    End Sub
    Private Sub CreaTabellaVociParametriGruppo(ByRef contenitore As Panel, ByVal idParametro As Integer, ByVal gruppo As Integer, ByVal sceltaMultipla As Boolean, ByVal descrizioneGruppo As String)
        Dim dataRow As DataRow
        Dim dataTable As DataTable
        ' Estrazione delle vociparametro Relative ad un parametro 
        Dim strIdRegioniAbilitate As String = ";22;"
        Dim ContaCheckParziale As Integer = 0
        ' Estrazione delle vociparametro Relative ad un parametro 
        strsql = "Select parametriprogrammi.idparametro,Vociparametriprogrammi.idvoce,Vociparametriprogrammi.CampoNote,Vociparametriprogrammi.voce,Vociparametriprogrammi.punteggio,Vociparametriprogrammi.gruppo,isnull(Vociparametriprogrammi.RiferimentoVoce,'') AS RiferimentoVoce,parametriprogrammi.tipo,isnull(Vociparametriprogrammi.AssociaNote,0) as AssociaNote " & _
        " from Vociparametriprogrammi " & _
        " inner join parametriprogrammi on parametriprogrammi.idparametro=Vociparametriprogrammi.idparametro " & _
        " where parametriprogrammi.idparametro=" & idParametro & " and Vociparametriprogrammi.attivo=1 and Vociparametriprogrammi.gruppo=" & gruppo & " "
        strsql = strsql & "AND Vociparametriprogrammi.UNSC=0 "
        'If InStr(strIdRegioniAbilitate, ";" & Session("IdRegioneCompetenzaProgetto") & ";") = 0 Then
        '    strsql = strsql & "AND Vociparametriprogrammi.UNSC=0 "
        'End If
        ChiudiDataReader(dtrgenerico)
        dataTable = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        Dim tbl As String = "T" & counter
        Dim nomeGruppo As String = gruppo.ToString + idParametro.ToString
        For Each dataRow In dataTable.Rows
            Dim checkBox As CheckBox = New CheckBox
            checkBox.Checked = False
            counter = counter + 1
            checkBox.ID = counter & "cs" & "Z" & dataRow.Item("idvoce") & "Y" & idParametro & "X" & dataRow.Item("CampoNote") & "W" & dataRow.Item("AssociaNote")
            checkBox.CssClass = "textbox"
            'radioButton.Width = New Unit(400)
            checkBox.Width = New Unit(48, UnitType.Percentage)
            checkBox.AutoPostBack = False
            checkBox.ValidationGroup = nomeGruppo
            checkBox.AutoPostBack = True
            AddHandler checkBox.CheckedChanged, AddressOf CheckBox_CheckedChanged
            checkBox.Text = dataRow.Item("voce").ToString().ToUpper(System.Globalization.CultureInfo.CurrentCulture)
            If hf_BloccaMaschera.Value = "Si" Then
                checkBox.Enabled = False
            End If
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
            labelCheckBoxId.AssociatedControlID = checkBox.ID
            Dim labelPunteggio As Label = New Label
            labelPunteggio.ID = "lpu" & checkBox.ID
            labelPunteggio.CssClass = "labelDati"
            labelPunteggio.Text = dataRow.Item("Punteggio")
            'labelPunteggio.Text = labelPunteggio.Text + "&nbsp;"
            labelPunteggio.Width = New Unit(2, UnitType.Percentage)
            labelPunteggio.Visible = True






            If Session("VisQualPosizione") = "DX" Then
                contenitore.Controls.Add(checkBox)
                contenitore.Controls.Add(labelCheckBoxId)
                contenitore.Controls.Add(labelIdVoce)
                contenitore.Controls.Add(labelPunteggio)
                LinkButtonSx.Visible = True
            End If
            If Session("VisQualPosizione") = "SX" Then
                contenitore.Controls.Add(labelPunteggio)
                contenitore.Controls.Add(checkBox)
                contenitore.Controls.Add(labelCheckBoxId)
                contenitore.Controls.Add(labelIdVoce)
                LinkButtonDx.Visible = True

            End If


            'If Not Request.Cookies("SUSCNAllineamento") Is Nothing Then

            '    If Request.Cookies("SUSCNAllineamento")("Posizione") = "DX" Then
            '        contenitore.Controls.Add(checkBox)
            '        contenitore.Controls.Add(labelCheckBoxId)
            '        contenitore.Controls.Add(labelIdVoce)
            '        contenitore.Controls.Add(labelPunteggio)
            '        LinkButtonSx.Visible = True
            '    End If
            '    If Request.Cookies("SUSCNAllineamento")("Posizione") = "SX" Then
            '        contenitore.Controls.Add(labelPunteggio)
            '        contenitore.Controls.Add(checkBox)
            '        contenitore.Controls.Add(labelCheckBoxId)
            '        contenitore.Controls.Add(labelIdVoce)
            '        LinkButtonDx.Visible = True

            '    End If

            '    If Request.Cookies("SUSCNAllineamento")("Posizione") = "" Then
            '        contenitore.Controls.Add(checkBox)
            '        contenitore.Controls.Add(labelCheckBoxId)
            '        contenitore.Controls.Add(labelIdVoce)
            '        contenitore.Controls.Add(labelPunteggio)
            '        LinkButtonSx.Visible = True
            '    End If
            'Else
            '    contenitore.Controls.Add(checkBox)
            '    contenitore.Controls.Add(labelCheckBoxId)
            '    contenitore.Controls.Add(labelIdVoce)
            '    contenitore.Controls.Add(labelPunteggio)
            '    LinkButtonSx.Visible = True

            'End If



            If IsPostBack = False Then
                Dim datiVerificaValutazioneQualita As DatiValutazioneQualita = New DatiValutazioneQualita()
                datiVerificaValutazioneQualita.Id = checkBox.ID
                datiVerificaValutazioneQualita.TipoOggetto = TipoOggetto.RadioButton
                datiVerificaValutazioneQualita.Gruppo = nomeGruppo
                datiVerificaValutazioneQualita.Descrizione = descrizioneGruppo
                infoCheck.Add(datiVerificaValutazioneQualita)
            End If
        Next
        classeHelper.AggiungiRigaVuota(contenitore)
    End Sub
    Private Sub CreaTabellaVociParametri(ByRef contenitore As Panel, ByVal idParametro As Integer, ByVal tipoParametro As Integer, ByVal sceltaMultipla As Boolean, ByVal descrizioneGruppo As String)
        Dim dataRow As DataRow
        Dim dataTable As DataTable
        'Dim strIdRegioniAbilitate As String = ";12;16;7;5;6;10;9;20;21;22;13;19;"
        Dim strIdRegioniAbilitate As String = ";22;"
        ' Estrazione delle vociparametro Relative ad un parametro 
        strsql = "Select parametriprogrammi.idparametro, Vociparametriprogrammi.idvoce,Vociparametriprogrammi.CampoNote,Vociparametriprogrammi.voce,Vociparametriprogrammi.punteggio,Vociparametriprogrammi.gruppo,isnull(Vociparametriprogrammi.RiferimentoVoce,'') AS RiferimentoVoce,parametriprogrammi.tipo,isnull(Vociparametriprogrammi.AssociaNote,0) as AssociaNote " & _
        " from Vociparametriprogrammi " & _
        " inner join parametriprogrammi on parametriprogrammi.idparametro=Vociparametriprogrammi.idparametro " & _
        " where parametriprogrammi.idparametro=" & idParametro & " and Vociparametriprogrammi.attivo=1 "
        strsql = strsql & "AND Vociparametriprogrammi.UNSC=0 "
        'If InStr(strIdRegioniAbilitate, ";" & Session("IdRegioneCompetenzaProgetto") & ";") = 0 Then
        '    strsql = strsql & "AND Vociparametri.UNSC=0 "
        'End If
        ChiudiDataReader(dtrgenerico)
        dataTable = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        Dim nomeGruppo As String = tipoParametro & idParametro.ToString
        If (sceltaMultipla = False) Then
            For Each dataRow In dataTable.Rows
                Dim checkBox As CheckBox = New CheckBox
                counter = counter + 1
                If tipoParametro = 1 Then
                    checkBox.ID = counter & "cs" & "Z" & dataRow.Item("idvoce") & "Y" & idParametro & "X" & dataRow.Item("CampoNote") & "W" & dataRow.Item("AssociaNote")
                Else
                    checkBox.ID = counter & "cm" & "Z" & dataRow.Item("idvoce") & "Y" & idParametro & "X" & dataRow.Item("CampoNote") & "W" & dataRow.Item("AssociaNote")
                End If

                checkBox.CssClass = "textbox"
                'radioButton.Width = New Unit(400)
                checkBox.Width = New Unit(48, UnitType.Percentage)
                checkBox.AutoPostBack = False
                checkBox.ValidationGroup = nomeGruppo
                'radioButton.
                checkBox.AutoPostBack = True
                AddHandler checkBox.CheckedChanged, AddressOf CheckBox_CheckedChanged
                checkBox.Text = dataRow.Item("voce").ToString().ToUpper(System.Globalization.CultureInfo.CurrentCulture)
                If hf_BloccaMaschera.Value = "Si" Then
                    checkBox.Enabled = False
                End If

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
                labelCheckBoxId.AssociatedControlID = checkBox.ID
                Dim labelPunteggio As Label = New Label
                labelPunteggio.ID = "lpu" & checkBox.ID
                labelPunteggio.CssClass = "labelDati"
                labelPunteggio.Text = dataRow.Item("Punteggio")
                labelPunteggio.Width = New Unit(2, UnitType.Percentage)
                'labelPunteggio.Text = labelPunteggio.Text + "&nbsp;"
                labelPunteggio.Visible = True
                'Response.Write("<hr />")
                'contenitore.Controls.Add("<hr />")




                If Session("VisQualPosizione") = "DX" Then
                    contenitore.Controls.Add(checkBox)
                    contenitore.Controls.Add(labelCheckBoxId)
                    contenitore.Controls.Add(labelIdVoce)
                    contenitore.Controls.Add(labelPunteggio)
                    LinkButtonSx.Visible = True
                End If
                If Session("VisQualPosizione") = "SX" Then
                    contenitore.Controls.Add(labelPunteggio)
                    contenitore.Controls.Add(checkBox)
                    contenitore.Controls.Add(labelCheckBoxId)
                    contenitore.Controls.Add(labelIdVoce)
                    LinkButtonDx.Visible = True

                End If




                'If Not Request.Cookies("SUSCNAllineamento") Is Nothing Then

                '    If Request.Cookies("SUSCNAllineamento")("Posizione") = "DX" Then
                '        contenitore.Controls.Add(checkBox)
                '        contenitore.Controls.Add(labelCheckBoxId)
                '        contenitore.Controls.Add(labelIdVoce)
                '        contenitore.Controls.Add(labelPunteggio)
                '        LinkButtonSx.Visible = True
                '    End If
                '    If Request.Cookies("SUSCNAllineamento")("Posizione") = "SX" Then
                '        contenitore.Controls.Add(labelPunteggio)
                '        contenitore.Controls.Add(checkBox)
                '        contenitore.Controls.Add(labelCheckBoxId)
                '        contenitore.Controls.Add(labelIdVoce)
                '        LinkButtonDx.Visible = True

                '    End If

                '    If Request.Cookies("SUSCNAllineamento")("Posizione") = "" Then
                '        contenitore.Controls.Add(checkBox)
                '        contenitore.Controls.Add(labelCheckBoxId)
                '        contenitore.Controls.Add(labelIdVoce)
                '        contenitore.Controls.Add(labelPunteggio)
                '        LinkButtonSx.Visible = True
                '    End If
                'Else
                '    contenitore.Controls.Add(checkBox)
                '    contenitore.Controls.Add(labelCheckBoxId)
                '    contenitore.Controls.Add(labelIdVoce)
                '    contenitore.Controls.Add(labelPunteggio)
                '    LinkButtonSx.Visible = True

                'End If


                'contenitore.Controls.Add(labelPunteggio)
                'contenitore.Controls.Add(checkBox)
                'contenitore.Controls.Add(labelCheckBoxId)
                'contenitore.Controls.Add(labelIdVoce)

                If IsPostBack = False Then
                    Dim datiVerificaValQualita As DatiValutazioneQualita = New DatiValutazioneQualita()
                    datiVerificaValQualita.Id = checkBox.ID
                    datiVerificaValQualita.TipoOggetto = TipoOggetto.RadioButton
                    datiVerificaValQualita.Gruppo = nomeGruppo
                    datiVerificaValQualita.Descrizione = descrizioneGruppo
                    infoCheck.Add(datiVerificaValQualita)
                End If
            Next

        Else
            For Each dataRow In dataTable.Rows
                Dim checkBox As CheckBox = New CheckBox
                checkBox.Checked = False
                counter = counter + 1
                If tipoParametro = 1 Then
                    checkBox.ID = counter & "cs" & "Z" & dataRow.Item("idvoce") & "Y" & idParametro & "X" & dataRow.Item("CampoNote") & "W" & dataRow.Item("AssociaNote")
                Else
                    checkBox.ID = counter & "cm" & "Z" & dataRow.Item("idvoce") & "Y" & idParametro & "X" & dataRow.Item("CampoNote") & "W" & dataRow.Item("AssociaNote")
                End If

                checkBox.CssClass = "textbox"
                checkBox.Width = New Unit(48, UnitType.Percentage)
                checkBox.AutoPostBack = False
                checkBox.Text = " " & dataRow.Item("voce").ToString().ToUpper(System.Globalization.CultureInfo.CurrentCulture)
                checkBox.ValidationGroup = nomeGruppo
                If hf_BloccaMaschera.Value = "Si" Then
                    checkBox.Enabled = False
                End If

                'adc
                AddHandler checkBox.CheckedChanged, AddressOf CheckBox_CheckedChanged

                Dim labelIdVoce As Label = New Label
                labelIdVoce.ID = "l" & checkBox.ID
                labelIdVoce.Text = dataRow.Item("idvoce")
                labelIdVoce.Visible = False
                Dim labelCheckBoxId As Label = New Label
                labelCheckBoxId.ID = "lp" & checkBox.ID
                labelCheckBoxId.Text = idParametro
                labelCheckBoxId.Visible = False
                labelCheckBoxId.AssociatedControlID = checkBox.ID
                Dim labelPunteggio As Label = New Label
                labelPunteggio.ID = "lpu" & checkBox.ID
                labelPunteggio.CssClass = "labelDati"
                labelPunteggio.Text = dataRow.Item("Punteggio")
                labelPunteggio.Width = New Unit(2, UnitType.Percentage)
                'labelPunteggio.Text = labelPunteggio.Text + "&nbsp;"
                labelPunteggio.Visible = True


                If Session("VisQualPosizione") = "DX" Then
                    contenitore.Controls.Add(checkBox)
                    contenitore.Controls.Add(labelCheckBoxId)
                    contenitore.Controls.Add(labelIdVoce)
                    contenitore.Controls.Add(labelPunteggio)
                    LinkButtonSx.Visible = True
                End If
                If Session("VisQualPosizione") = "SX" Then
                    contenitore.Controls.Add(labelPunteggio)
                    contenitore.Controls.Add(checkBox)
                    contenitore.Controls.Add(labelCheckBoxId)
                    contenitore.Controls.Add(labelIdVoce)
                    LinkButtonDx.Visible = True

                End If






                'If Not Request.Cookies("SUSCNAllineamento") Is Nothing Then

                '    If Request.Cookies("SUSCNAllineamento")("Posizione") = "DX" Then
                '        contenitore.Controls.Add(checkBox)
                '        contenitore.Controls.Add(labelCheckBoxId)
                '        contenitore.Controls.Add(labelIdVoce)
                '        contenitore.Controls.Add(labelPunteggio)
                '        LinkButtonSx.Visible = True
                '    End If
                '    If Request.Cookies("SUSCNAllineamento")("Posizione") = "SX" Then
                '        contenitore.Controls.Add(labelPunteggio)
                '        contenitore.Controls.Add(checkBox)
                '        contenitore.Controls.Add(labelCheckBoxId)
                '        contenitore.Controls.Add(labelIdVoce)
                '        LinkButtonDx.Visible = True

                '    End If

                '    If Request.Cookies("SUSCNAllineamento")("Posizione") = "" Then
                '        contenitore.Controls.Add(checkBox)
                '        contenitore.Controls.Add(labelCheckBoxId)
                '        contenitore.Controls.Add(labelIdVoce)
                '        contenitore.Controls.Add(labelPunteggio)
                '        LinkButtonSx.Visible = True
                '    End If
                'Else
                '    contenitore.Controls.Add(checkBox)
                '    contenitore.Controls.Add(labelCheckBoxId)
                '    contenitore.Controls.Add(labelIdVoce)
                '    contenitore.Controls.Add(labelPunteggio)
                '    LinkButtonSx.Visible = True

                'End If

                'contenitore.Controls.Add(labelPunteggio)
                'contenitore.Controls.Add(checkBox)
                'contenitore.Controls.Add(labelCheckBoxId)
                'contenitore.Controls.Add(labelIdVoce)

                If IsPostBack = False Then
                    Dim datiVerificaValutazioneQualita As DatiValutazioneQualita = New DatiValutazioneQualita()
                    datiVerificaValutazioneQualita.Id = checkBox.ID
                    datiVerificaValutazioneQualita.TipoOggetto = TipoOggetto.CheckBox
                    datiVerificaValutazioneQualita.Gruppo = nomeGruppo
                    datiVerificaValutazioneQualita.Descrizione = descrizioneGruppo
                    infoCheck.Add(datiVerificaValutazioneQualita)
                End If
            Next

        End If
        classeHelper.AggiungiRigaVuota(contenitore)

        ChiudiDataReader(dtrgenerico)
    End Sub


    Private Sub PopolaMaschera()
        Dim s As Integer
        Dim j As Integer
        Dim tblcel As TableCell
        Dim i As Integer
        Dim appo As String
        Dim nomech As String

        If txtnome.Value = "" Then
            j = 0
            msgInfo.Text = "Non e' prevista valutazione per questo bando."
        Else
            j = CInt(txtnome.Value)
        End If

        tipoProgetto = TipologiaProgrammi(hf_IdProgramma.Value)


        'Cricamento dati vecchi progetti (fino al 2006)
        strsql = "select *,VociParametriprogrammi.AssociaNote,Vociparametriprogrammi.CampoNote, " & _
        " storicoprogrammi.punteggiofinale,storicoprogrammi.punteggiocp,storicoprogrammi.punteggioCOE,storicoprogrammi.noteStorico" & _
        " from VociParametriprogrammi " & _
        " inner join storicovociprogrammi on (storicovociprogrammi.idvoce=VociParametriprogrammi.idvoce)" & _
        " inner join storicoprogrammi on (storicovociprogrammi.idStorico=Storicoprogrammi.idStorico)" & _
        " where idprogramma=" & hf_IdProgramma.Value & " and storicovociprogrammi.idStorico= "
        If hf_Personalizza.Value = "Modifica" Then
            strsql = strsql & "" & hf_IdStorico.Value & ""
        Else
            strsql = strsql & " (select max(idstorico)as maxid from storicoprogrammi where idprogramma=" & hf_IdProgramma.Value & ")"
        End If
        ChiudiDataReader(dtrgenerico)

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        Do While dtrgenerico.Read

            lbldataUltima.Text = dtrgenerico("dataInserimento")




            For s = 1 To j
                'nomech = s & "cs" & "Y" & dtrgenerico("idparametro") & "X" & IIf(Not IsDBNull(dtrgenerico("AssociaNote")), dtrgenerico("AssociaNote"), 0)
                nomech = s & "cs" & "Z" & dtrgenerico("idvoce") & "Y" & dtrgenerico("idparametro") & "X" & dtrgenerico("CampoNote") & "W" & dtrgenerico("AssociaNote")

                'Inserisci("s", Table5, nomech)
                ValorizzaCheck(nomech, dtrgenerico("idvoce"), dtrgenerico("idparametro"))
                'nomech = s & "cm" & "Y" & dtrgenerico("idparametro") & "X" & IIf(Not IsDBNull(dtrgenerico("AssociaNote")), dtrgenerico("AssociaNote"), 0)
                nomech = s & "cm" & "Z" & dtrgenerico("idvoce") & "Y" & dtrgenerico("idparametro") & "X" & dtrgenerico("CampoNote") & "W" & dtrgenerico("AssociaNote")

                ValorizzaCheck(nomech, dtrgenerico("idvoce"), dtrgenerico("idparametro"))
                'Inserisci("s", Table5, nomech)
                'If tipoProgetto = 1 Or tipoProgetto = 3 Then
                '    txtPuntRegio.Text = Replace(IIf(Not IsDBNull(dtrgenerico("puntRegionale")), dtrgenerico("puntRegionale"), 0), ",", ".")
                'End If
                'txtPuntoCoerenza.Text = IIf(Not IsDBNull(dtrgenerico("puntCoerenza")), dtrgenerico("puntCoerenza"), 0)
                'txtPuntoRilevanza.Text = IIf(Not IsDBNull(dtrgenerico("puntRilevanza")), dtrgenerico("puntRilevanza"), 0)
                'txtDefCopertura.Text = IIf(Not IsDBNull(dtrgenerico("defCopertura")), dtrgenerico("defCopertura"), 0)
                'txtdefInflazione.Text = IIf(Not IsDBNull(dtrgenerico("definfrazione")), dtrgenerico("definfrazione"), 0)
                'ddlinternet.SelectedValue = IIf(Not IsDBNull(dtrgenerico("puntInternet")), dtrgenerico("puntInternet"), "")
                '** agg. il 10/12/2009 da simoma cordella
                lblpunteggioCOE.Text = IIf(Not IsDBNull(dtrgenerico("PunteggioCOE")), dtrgenerico("PunteggioCOE"), 0)
                'txtDefSanzione.Text = IIf(Not IsDBNull(dtrgenerico("defSanzioni")), dtrgenerico("defSanzioni"), 0)
                'txtDefInfortuni.Text = IIf(Not IsDBNull(dtrgenerico("defInfortuni")), dtrgenerico("defInfortuni"), 0)
                '**
                'lblPunteggioCKH.Text = IIf(Not IsDBNull(dtrgenerico("PunteggioCKH")), dtrgenerico("PunteggioCKH"), 0)
                'lblpunteggioCO.Text = IIf(Not IsDBNull(dtrgenerico("punteggioCO")), dtrgenerico("punteggioCO"), 0)
                lblPunteggioCP.Text = IIf(Not IsDBNull(dtrgenerico("punteggioCp")), dtrgenerico("punteggioCp"), 0)
                LblPuntegioFinale.Text = IIf(Not IsDBNull(dtrgenerico("punteggioFinale")), dtrgenerico("punteggioFinale"), 0)
                ''ADC LblPuntegioTotaleRis.Text = IIf(Not IsDBNull(dtrgenerico("punteggioTotale")), dtrgenerico("punteggioTotale"), 0)
                txtnote.Text = IIf(Not IsDBNull(dtrgenerico("noteStorico")), dtrgenerico("notestorico"), "")
                'Txt24Assenti.Text = IIf(Not IsDBNull(dtrgenerico("Note24A")), dtrgenerico("Note24A"), "")
                'Txt24Presenti.Text = IIf(Not IsDBNull(dtrgenerico("Note24B")), dtrgenerico("Note24B"), "")
                'Txt24EsteroAssenti.Text = IIf(Not IsDBNull(dtrgenerico("Note24C")), dtrgenerico("Note24C"), "")
                'Txt25Assenti.Text = IIf(Not IsDBNull(dtrgenerico("Note25")), dtrgenerico("Note25"), "")
                'Txt28Assenti.Text = IIf(Not IsDBNull(dtrgenerico("Note28")), dtrgenerico("Note28"), "")
                'Txt29Assenti.Text = IIf(Not IsDBNull(dtrgenerico("Note29")), dtrgenerico("Note29"), "")


            Next

        Loop


        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


        'ControllaNoteAbilitate()

        'Cricamento dati vecchi progetti (fino al 2006)
        strsql = "select isnull(sum(vociparametriprogrammi.punteggio),0) as Descrizione " & _
        " from VociParametriprogrammi" & _
        " inner join storicovociprogrammi on (storicovociprogrammi.idvoce=VociParametriprogrammi.idvoce)" & _
        " inner join storicoprogrammi on (storicovociprogrammi.idStorico=Storicoprogrammi.idStorico)" & _
        " where vociparametriprogrammi.riferimentovoce  = 'D' and idprogramma=" & hf_IdProgramma.Value & " and storicovociprogrammi.idStorico= "
        If hf_Personalizza.Value = "Modifica" Then
            strsql = strsql & "" & hf_IdStorico.Value & ""
        Else
            strsql = strsql & " (select max(idstorico)as maxid from storicoprogrammi where idprogramma=" & hf_IdProgramma.Value & ")"
        End If
        ChiudiDataReader(dtrgenerico)
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            txtTOTALEPUNTEGGIODESCRIZIONE.Value = dtrgenerico("Descrizione")
        End If
        ChiudiDataReader(dtrgenerico)

        'Cricamento dati vecchi progetti (fino al 2006)
        strsql = "select isnull(sum(vociparametriprogrammi.punteggio),0) as Obiettivi " & _
        " from VociParametriprogrammi" & _
        " inner join storicovociprogrammi on (storicovociprogrammi.idvoce=VociParametriprogrammi.idvoce)" & _
        " inner join storicoprogrammi on (storicovociprogrammi.idStorico=Storicoprogrammi.idStorico)" & _
        " where vociparametriprogrammi.riferimentovoce  = 'O' and idprogramma=" & hf_IdProgramma.Value & " and storicovociprogrammi.idStorico= "
        If hf_Personalizza.Value = "Modifica" Then
            strsql = strsql & "" & hf_IdStorico.Value & ""
        Else
            strsql = strsql & " (select max(idstorico)as maxid from storicoprogrammi where idprogramma=" & hf_IdProgramma.Value & ")"
        End If
        ChiudiDataReader(dtrgenerico)

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            txtTOTALEPUNTEGGIOOBIETTIVI.Value = dtrgenerico("Obiettivi")
        End If
        ChiudiDataReader(dtrgenerico)

        'Cricamento dati vecchi progetti (fino al 2006)
        strsql = "select isnull(sum(vociparametriprogrammi.punteggio),0) as Contesto " & _
        " from VociParametriprogrammi" & _
        " inner join storicovociprogrammi on (storicovociprogrammi.idvoce=VociParametriprogrammi.idvoce)" & _
        " inner join storicoprogrammi on (storicovociprogrammi.idStorico=Storicoprogrammi.idStorico)" & _
        " where vociparametriprogrammi.riferimentovoce  = 'C' and idprogramma=" & hf_IdProgramma.Value & " and storicovociprogrammi.idStorico= "
        If hf_Personalizza.Value = "Modifica" Then
            strsql = strsql & "" & hf_IdStorico.Value & ""
        Else
            strsql = strsql & " (select max(idstorico)as maxid from storicoprogrammi where idprogramma=" & hf_IdProgramma.Value & ")"
        End If
        ChiudiDataReader(dtrgenerico)

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            txtTOTALEPUNTEGGIOCONTESTO.Value = dtrgenerico("Contesto")
        End If
        ChiudiDataReader(dtrgenerico)
        'TabStrip1.SelectedIndex = 0
        'Cricamento dati nuovi progetti (dopo il 2006)
    End Sub


    Private Sub ValorizzaCheck(ByVal NomeCheck As String, ByVal idvoce As String, ByVal parametro As String)
        Dim lbl As Label = DirectCast(panelCaratteristicheProgramma.FindControl("l" & NomeCheck), Label)
        Dim lblp As Label = DirectCast(panelCaratteristicheProgramma.FindControl("lp" & NomeCheck), Label)

        Dim check As CheckBox = TryCast(panelCaratteristicheProgramma.FindControl(NomeCheck), CheckBox)
        Dim radiobutton As RadioButton = TryCast(panelCaratteristicheProgramma.FindControl(NomeCheck), RadioButton)


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
        'mod. da simona cordella il 14/05/2009 
        If hf_Pagina.Value = "" Then
            If Request.QueryString("VengoDa") = "AssociaVolontari" Then
                'vengo da maschera WfrmAssociaVolontari.aspx?(ggraduatorie)
                Response.Redirect("WfrmAssociaVolontari.aspx?VengoDa=AssociaVolontari&CheckIndietro=" & Request.QueryString("CheckIndietro") & "&Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Progetto=" & Request.QueryString("Progetto") & "&Bando=" & Request.QueryString("Bando") & "&Settore=" & Request.QueryString("Settore") & "&Area=" & Request.QueryString("Area") & "&InAttesa=" & Request.QueryString("InAttesa") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&IDAttivitasedeAssegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "&presenta=" & Request.QueryString("presenta") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&CodiceFascicolo=" & Request.QueryString("CodiceFascicolo") & "&DescFascicolo=" & Request.QueryString("DescFascicolo") & "&NumeroFascicolo=" & Request.QueryString("NumeroFascicolo"))
            Else
                Response.Redirect("WfrmProgrammidaValQualita.aspx?VengoDa=Valutare")
            End If
        Else
            Response.Redirect("ricercaprogrammi.aspx")
        End If
    End Sub
    Private Sub cmdConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConferma.Click
        Salva(SENDER_PULSANTE_CONFERMA)
        If msgErrore.Text = "" Then
            msgConferma.Visible = True
            msgConferma.Text = "Conferma Avvenuta con Successo."

            'txtPuntRegio.Focus()
            'MaintainScrollPositionOnPostBack = False
            'CancellaMessaggi()

        End If

    End Sub

    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        Salva(SENDER_PULSANTE_SALVA)
        If msgErrore.Text = "" Then
            msgConferma.Visible = True
            msgConferma.Text = "Salvataggio Avvenuto con Successo."

            'txtPuntRegio.Focus()
            'MaintainScrollPositionOnPostBack = False
            'CancellaMessaggi()


        End If
    End Sub
    Private Sub Salva(Optional ByVal clientIdSender As String = SENDER_PULSANTE_CONFERMA)
        CancellaMessaggi()
        If Not (ValidaDati()) Then
            Exit Sub
        End If
        Dim s As Integer
        Dim j As Integer
        Dim tblcel As TableCell
        Dim i As Integer
        Dim appo As String
        Dim nomech As String
        Dim idStorico As String
        Dim bytNoteRiservate As Byte = 0
        Dim dtDomanda As DataTable
        Dim k As Integer = 0
        Dim dtRisposta As DataTable
        Dim w As Integer = 0
        Dim RitornoIdStoricoTMP As Integer
        Dim strRitornoMesaggio As String


        If ControlloValutazioneProgetti(hf_IdProgramma.Value) = False Then
            msgErrore.Text = "Esistono progetti associati al programma non ancora valutati formalmente."
            'MaintainScrollPositionOnPostBack = False
            msgErrore.Focus()
        Else
            If ControlloNumeroProgettiAmmissibili(hf_IdProgramma.Value) = False Then
                msgErrore.Text = "Il numero di progetti ammissibili associati al programma è inferiore a 2."
                'MaintainScrollPositionOnPostBack = False
                msgErrore.Focus()
            End If
        End If

        'If Txt28Assenti.Text <> "" Or Txt24Assenti.Text <> "" Or Txt24Presenti.Text <> "" Or Txt29Assenti.Text <> "" Or Txt25Assenti.Text <> "" Or Txt24EsteroAssenti.Text <> "" Then
        '    bytNoteRiservate = 1
        'End If
        j = CInt(txtnome.Value)

        RitornoIdStoricoTMP = VerificaCongruenzaValutazione(strRitornoMesaggio, j)

        Select Case RitornoIdStoricoTMP
            Case 0 'non valida (risposte incompatibili)

                msgErrore.Text = strRitornoMesaggio
                'MaintainScrollPositionOnPostBack = False
                msgErrore.Focus()




                Exit Sub
            Case 1 'valida ma incompleta
                If clientIdSender = "cmdConferma" Then
                    msgErrore.Text = strRitornoMesaggio
                    'MaintainScrollPositionOnPostBack = False
                    msgErrore.Focus()



                    Exit Sub
                End If
            Case 2 'valida e completa
                'esegue tutto OK
        End Select
        ChiudiDataReader(dtrgenerico)

        'Inserimento(StoricoProgetti)
        strsql = "Insert into storicoprogrammi (idprogramma,dataInserimento," & _
                " statoprogramma,Username,notestorico) " & _
                "values (" & hf_IdProgramma.Value & ",getdate()," & hf_StatoProgramma.Value & ", " & _
                "'" & Session("Utente") & "', '" & Replace(txtnote.Text, "'", "''") & "') "


        ChiudiDataReader(dtrgenerico)
        ClsServer.EseguiSqlClient(strsql, Session("conn"))
        strsql = "Select @@identity as maxid from storicoprogrammi"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            idStorico = dtrgenerico("maxid")
        End If
        ChiudiDataReader(dtrgenerico)
        ' query per trovare gli id delle domande e il campo associanote

        strsql = "SELECT distinct parametriprogrammi.idparametro, isnull(Vociparametriprogrammi.AssociaNote,0) as AssociaNote "
        strsql = strsql & " FROM parametriprogrammi "
        strsql = strsql & " INNER JOIN Vociparametriprogrammi  on parametriprogrammi.idparametro=Vociparametriprogrammi.idparametro"
        strsql = strsql & " INNER JOIN AssociaBandoValutazioneprogrammi ON parametriprogrammi.IDParametro = AssociaBandoValutazioneprogrammi.IdParametro "
        strsql = strsql & " INNER JOIN bando ON AssociaBandoValutazioneprogrammi.IdBando = bando.IDBando "
        strsql = strsql & " INNER JOIN Bandiprogrammi ON bando.IDBando = Bandiprogrammi.IdBando "
        strsql = strsql & " INNER JOIN programmi ON Bandiprogrammi.IdBandoprogramma = programmi.IDBandoprogramma "
        strsql = strsql & " WHERE  (parametriprogrammi.Attivo = 1) "
        strsql = strsql & " and parametriprogrammi.Nazionale=1 "

        strsql = strsql & " and parametriprogrammi.classePunteggio IN(0,1,2,3,4,5,6) "
        strsql = strsql & " AND (programmi.idprogramma = " & hf_IdProgramma.Value & ")"


        dtDomanda = ClsServer.CreaDataTable(strsql, False, Session("conn"))

        For k = 0 To dtDomanda.Rows.Count - 1
            strsql = "select idvoce,CampoNote from Vociparametriprogrammi where idparametro =" & dtDomanda.Rows(k).Item("idparametro")
            dtRisposta = ClsServer.CreaDataTable(strsql, False, Session("conn"))


            For w = 0 To dtRisposta.Rows.Count - 1

                s = 1
                For s = 1 To j
                    'nomech = s & "cs" & "Y" & dtDomanda.Rows(k).Item("idparametro") & "X" & dtDomanda.Rows(k).Item("AssociaNote")
                    nomech = s & "cs" & "Z" & dtRisposta.Rows(w).Item("idvoce") & "Y" & dtDomanda.Rows(k).Item("idparametro") & "X" & dtRisposta.Rows(w).Item("CampoNote") & "W" & dtDomanda.Rows(k).Item("AssociaNote")

                    Inserisci("s", panelCaratteristicheProgramma, nomech, idStorico)

                    'nomech = s & "cm" & "Y" & dtDomanda.Rows(k).Item("idparametro") & "X" & dtDomanda.Rows(k).Item("AssociaNote")
                    nomech = s & "cm" & "Z" & dtRisposta.Rows(w).Item("idvoce") & "Y" & dtDomanda.Rows(k).Item("idparametro") & "X" & dtRisposta.Rows(w).Item("CampoNote") & "W" & dtDomanda.Rows(k).Item("AssociaNote")

                    Inserisci("s", panelCaratteristicheProgramma, nomech, idStorico)
                Next
            Next
        Next
        'eseguo storeProcedure ComputeScore
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'eseguo storeProcedure ComputeScore
        dtrgenerico = ClsServer.EseguiStoreStoricoProgetti(idStorico, "computeScoreProgrammi", Session("conn"))
        Do While dtrgenerico.Read
            LblPuntegioFinale.Text = IIf(Not IsDBNull(dtrgenerico("punteggiofinale")), dtrgenerico("punteggiofinale"), 0)
            LblPuntegioFinale.Text = Math.Round(CDbl(LblPuntegioFinale.Text), 2)
            lblPunteggioCP.Text = IIf(Not IsDBNull(dtrgenerico("punteggioCP")), dtrgenerico("punteggioCP"), 0)
            lblPunteggioCP.Text = Math.Round(CDec(lblPunteggioCP.Text))
            'lblPunteggioCKH.Text = IIf(Not IsDBNull(dtrgenerico("punteggioCkh")), dtrgenerico("punteggioCkh"), 0)
            'lblPunteggioCKH.Text = Math.Round(CDec(lblPunteggioCKH.Text))
            'lblpunteggioCO.Text = IIf(Not IsDBNull(dtrgenerico("punteggioCo")), dtrgenerico("punteggioCo"), 0)
            'lblpunteggioCO.Text = Math.Round(CDec(lblpunteggioCO.Text))
            'agg. il 10/12/2009 da simona cordella
            lblpunteggioCOE.Text = IIf(Not IsDBNull(dtrgenerico("punteggioCoe")), dtrgenerico("punteggioCoe"), 0)
            lblpunteggioCOE.Text = Math.Round(CDec(lblpunteggioCOE.Text))
        Loop
        ChiudiDataReader(dtrgenerico)

        strsql = "Select punteggioTotale from programmi where idprogramma=" & hf_IdProgramma.Value & ""
        ChiudiDataReader(dtrgenerico)
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
       
        If dtrgenerico.HasRows = True Then
            LblPuntegioTotaleRis.Text = IIf(Not IsDBNull(dtrgenerico("punteggioTotale")), dtrgenerico("punteggioTotale"), 0)
        End If
        ChiudiDataReader(dtrgenerico)
       

        If clientIdSender = "cmdConferma" Then
            If txtDataSeduta.Text <> "" Then
                strsql = "Update programmi set ConfermaValutazione=1, DataSeduta='" & txtDataSeduta.Text & "' where idprogramma=" & hf_IdProgramma.Value & ""
            Else
                strsql = "Update programmi set ConfermaValutazione=1, DataSeduta=null where idprogramma=" & hf_IdProgramma.Value & ""
            End If

            cmdSalva.Visible = False
            Hdd_ConfermaValutazione.Value = "True"
        Else
            If txtDataSeduta.Text <> "" Then
                strsql = "Update programmi set ConfermaValutazione=0, DataSeduta='" & txtDataSeduta.Text & "' where idprogramma=" & hf_IdProgramma.Value & ""
            Else
                strsql = "Update programmi set ConfermaValutazione=0, DataSeduta=null where idprogramma=" & hf_IdProgramma.Value & ""
            End If

            Hdd_ConfermaValutazione.Value = "False"
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        ClsServer.EseguiSqlClient(strsql, Session("conn"))
        'Aggiunto da Alessandra Taballione il 24/10/2005
        lbldataUltima.Text = Session("dataserver")

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
            strsql = "insert into storicovociprogrammi(idStorico,idvoce,Punteggio) " & _
                  " Select " & idStorico & "," & lbl.Text & "," & lbl1.Text & " from vociparametriprogrammi where idvoce=" & lbl.Text & ""
            ClsServer.EseguiSqlClient(strsql, Session("conn"))
            ChiudiDataReader(dtrgenerico)
        End If

    End Sub
    Private Sub InserisciStoricoVociTMP(ByVal tipo As String, ByRef panel As Panel, ByVal nomeCheck As String, ByVal idStoricoTMP As Double)

        Dim check As CheckBox = DirectCast(panel.FindControl(nomeCheck), CheckBox)
        Dim lbl As Label = DirectCast(panel.FindControl("l" & nomeCheck), Label)
        Dim lbl1 As Label = DirectCast(panel.FindControl("lpu" & nomeCheck), Label)
        If Not IsNothing(check) Then
            If check.Checked = True Then
                'Inserimento StoricoVoci
                strsql = "insert into storicovociprogrammiTMP(idStoricoTMP,idvoceTMP,Punteggio) " & _
                " Select " & idStoricoTMP & "," & lbl.Text & "," & lbl1.Text & " from vociparametriprogrammi where idvoce=" & lbl.Text & ""
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                ClsServer.EseguiSqlClient(strsql, Session("conn"))
                'Inserimento StoricoProgetti
            End If
        End If
    End Sub


    Private Sub cmdStampaVQ_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdStampaVQ.Click
        'Calcolo l'IdStorico relativamente all'IdProgetto in valutazione
        If Hdd_ConfermaValutazione.Value = "True" Then
            Dim intTipoStampa As Integer
            strsql = "Select Max(IdStorico) As MaxIdS From storicoprogrammi Where Idprogramma=" & hf_IdProgramma.Value
            ChiudiDataReader(dtrgenerico)
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrgenerico.Read()

            If tipoProgetto = 0 Then
                intTipoStampa = 17
            ElseIf tipoProgetto = 1 Then
                intTipoStampa = 23
            ElseIf tipoProgetto = 2 Then
                intTipoStampa = 24
            ElseIf tipoProgetto = 3 Then
                intTipoStampa = 33
            ElseIf tipoProgetto = 4 Then
                intTipoStampa = 34
            End If

            'Mando in esecuzione la stampa della valutazione di qualità
            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("window.open('WfrmReportistica.aspx?sTipoStampa=" & intTipoStampa & "&IdAttivita=" & hf_IdProgramma.Value & "&IdStorico=" & dtrgenerico("MaxIdS") & "','report','height=800,width=800,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
            Response.Write("</SCRIPT>")
            ChiudiDataReader(dtrgenerico)
        Else
            msgErrore.Text = "&#201; necessario completare la Valutazione del Progetto prima di procedera alla stampa del documento."
            'MaintainScrollPositionOnPostBack = False

        End If

    End Sub



    'Sub per la stampa totale della valutazione di qualità
    Private Sub imgStampaTot_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgStampaTot.Click
        ChiudiDataReader(dtrgenerico)
        Dim intIdBando As Integer
        'Calcolo l'IdStorico relativamente agli IdProgetto in valutazione per l'ente
        strsql = "Select b.idbando From programmi a Inner Join Bandiprogrammi b " & _
                 " On a.idbandoprogramma = b.idbandoprogramma  Where a.idprogramma=" & hf_IdProgramma.Value

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        intIdBando = dtrgenerico.Item("idbando")
        ChiudiDataReader(dtrgenerico)

        'Mando in esecuzione la stampa della valutazione di qualità
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('WfrmReportistica.aspx?IdBando=" & intIdBando & "&sTipoStampa=18&IdEnte=" & Session("IdEnte") & "','report','height=800,width=800,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
        Response.Write("</SCRIPT>")
    End Sub

    Private Function TipologiaProgrammi(ByVal IdProgramma As Integer)
        Dim intTipoPrg As Integer = 2020


        Return intTipoPrg


    End Function


    Private Function VerificaCongruenzaValutazione(ByRef strMessaggio As String, ByVal ContaCeck As Integer) As Integer
        Dim intIDStoricoTMP As Integer
        Dim IntEsito As Integer
        'Creata da simona Cordella il 09/12/2009
        'la funzione verifica che siano state Valutate correttamente tutte le domande
        'richiamo funzione che cancello tabella temporanea
        DeleteTMP(Session("Utente"))
        'richiamo funzione che inserisce in tabelle temporanee
        intIDStoricoTMP = InsertTMP(Session("Utente"), ContaCeck)
        'richiamo store
        IntEsito = StoreVerificaValutazioneQual(intIDStoricoTMP, strMessaggio)
        Return IntEsito
    End Function

    Private Sub DeleteTMP(ByVal UserName As String)
        'Creata da simona Cordella il 09/12/2009
        Dim strSql As String
        Dim rstDelete As SqlClient.SqlDataReader
        Dim idStoricoTMP As Integer

        strSql = "Delete storicovociprogrammiTMP where IdStoricoTMP in (Select IdStoricoTMP from storicoprogrammiTMP where username = '" & UserName & "')"
        rstDelete = ClsServer.CreaDatareader(strSql, Session("conn"))
        ChiudiDataReader(rstDelete)
        strSql = "Delete storicoprogrammiTMP where username = '" & UserName & "'"
        rstDelete = ClsServer.CreaDatareader(strSql, Session("conn"))
        ChiudiDataReader(rstDelete)
    End Sub

    Private Function InsertTMP(ByVal UserName As String, ByVal ContaCeck As Integer) As Integer
        'Creata da simona Cordella il 09/12/2009

        Dim s As Integer
        Dim j As Integer
        Dim tblcel As TableCell
        Dim i As Integer
        Dim appo As String
        Dim nomech As String
        Dim idStoricoTMP As String
        Dim bytNoteRiservate As Byte = 0
        Dim dtDomanda As DataTable
        Dim k As Integer = 0
        Dim dtRisposta As DataTable
        Dim w As Integer = 0
        j = ContaCeck
        'Inserimento(StoricoProgetti)
        strsql = "Insert into storicoProgrammiTMP (idprogramma,dataInserimento," & _
                " statoprogramma,Username,notestorico )values " & _
                "(" & hf_IdProgramma.Value & ",getdate()," & hf_StatoProgramma.Value & "," & _
                "'" & UserName & "', '" & Replace(txtnote.Text, "'", "''") & "')"
        ChiudiDataReader(dtrgenerico)
        ClsServer.EseguiSqlClient(strsql, Session("conn"))
        strsql = "Select @@identity as maxid from storicoprogrammiTMP"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            idStoricoTMP = dtrgenerico("maxid")
        End If
        ChiudiDataReader(dtrgenerico)

        ' query per trovare gli id delle domande e il campo associanote
        strsql = "SELECT distinct parametriprogrammi.idparametro, isnull(Vociparametriprogrammi.AssociaNote,0) as AssociaNote "
        strsql = strsql & " FROM parametriprogrammi "
        strsql = strsql & " INNER JOIN Vociparametriprogrammi  on parametriprogrammi.idparametro=Vociparametriprogrammi.idparametro"
        strsql = strsql & " INNER JOIN AssociaBandoValutazioneprogrammi ON parametriprogrammi.IDParametro = AssociaBandoValutazioneprogrammi.IdParametro "
        strsql = strsql & " INNER JOIN bando ON AssociaBandoValutazioneprogrammi.IdBando = bando.IDBando "
        strsql = strsql & " INNER JOIN Bandiprogrammi ON bando.IDBando = Bandiprogrammi.IdBando "
        strsql = strsql & " INNER JOIN programmi ON Bandiprogrammi.IdBandoprogramma = programmi.IDBandoprogramma "
        strsql = strsql & " WHERE  (parametriprogrammi.Attivo = 1) "
        strsql = strsql & " and parametriprogrammi.Nazionale=1 "
        
        strsql = strsql & " and parametriprogrammi.classePunteggio IN(0,1,2,3,4,5,6) "
        strsql = strsql & " AND (programmi.idprogramma = " & hf_IdProgramma.Value & ")"
        dtDomanda = ClsServer.CreaDataTable(strsql, False, Session("conn"))

        For k = 0 To dtDomanda.Rows.Count - 1
            strsql = "select idvoce,CampoNote from Vociparametriprogrammi where idparametro =" & dtDomanda.Rows(k).Item("idparametro")
            dtRisposta = ClsServer.CreaDataTable(strsql, False, Session("conn"))
            For w = 0 To dtRisposta.Rows.Count - 1

                s = 1
                For s = 1 To j
                    'nomech = s & "cs" & "Y" & dtDomanda.Rows(k).Item("idparametro") & "X" & dtDomanda.Rows(k).Item("AssociaNote")
                    nomech = s & "cs" & "Z" & dtRisposta.Rows(w).Item("idvoce") & "Y" & dtDomanda.Rows(k).Item("idparametro") & "X" & dtRisposta.Rows(w).Item("CampoNote") & "W" & dtDomanda.Rows(k).Item("AssociaNote")

                    InserisciStoricoVociTMP("s", panelCaratteristicheProgramma, nomech, idStoricoTMP)

                    'nomech = s & "cm" & "Y" & dtDomanda.Rows(k).Item("idparametro") & "X" & dtDomanda.Rows(k).Item("AssociaNote")
                    nomech = s & "cm" & "Z" & dtRisposta.Rows(w).Item("idvoce") & "Y" & dtDomanda.Rows(k).Item("idparametro") & "X" & dtRisposta.Rows(w).Item("CampoNote") & "W" & dtDomanda.Rows(k).Item("AssociaNote")

                    InserisciStoricoVociTMP("s", panelCaratteristicheProgramma, nomech, idStoricoTMP)
                Next
            Next
        Next
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        Return idStoricoTMP
    End Function
    Private Function StoreVerificaValutazioneQual(ByVal IdStoricoTMP As Integer, ByRef StrMessaggio As String) As Integer
        'Agg. da Simona Cordella il 17/11/2009
        'richiamo store che verifca  se ci sono anomalie sul progetto durante l'istnza di presentazione
        Dim intValore As Integer
        Dim CustOrderHist As SqlClient.SqlCommand

        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_VERIFICA_VALUTAZIONE_QUAL_PROGRAMMI"
        CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdStoricoTMP"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)


        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@Esito"
        sparam1.SqlDbType = SqlDbType.Int
        sparam1.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@Motivazione"
        sparam2.SqlDbType = SqlDbType.VarChar
        sparam2.Size = 1000
        sparam2.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam2)

        Dim dataReader As SqlClient.SqlDataReader
        CustOrderHist.Parameters("@IdStoricoTMP").Value = IdStoricoTMP
        dataReader = CustOrderHist.ExecuteReader()
        'CustOrderHist.ExecuteNonQuery()
        ' Insert code to read through the datareader.
        intValore = CustOrderHist.Parameters("@Esito").Value
        StrMessaggio = CustOrderHist.Parameters("@Motivazione").Value
        StoreVerificaValutazioneQual = intValore
        ChiudiDataReader(dataReader)
    End Function

    Private Sub imgElencoDocumentiProg_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgElencoDocumentiProg.Click
        CancellaMessaggi()
        If (cmdSalva.Visible = True) Then
            Salva(SENDER_PULSANTE_SALVA)
        End If
        If (msgErrore.Text = String.Empty) Then
            Response.Redirect("wfrmProgrammiDocumenti.aspx?VengoDa=" & Costanti.VENGO_DA_VALUTAZIONE_QUALITA & "&IdProgramma=" & hf_IdProgramma.Value)
        End If

    End Sub

    Protected Sub Prog_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Prog.Click
        CancellaMessaggi()
        If (cmdSalva.Visible = True) Then
            Salva(SENDER_PULSANTE_SALVA)
        End If
        If (msgErrore.Text = String.Empty) Then

            Response.Redirect("WfrmProgrammi.aspx?VengoDa=" & Costanti.VENGO_DA_VALUTAZIONE_QUALITA & "&popup=1&Modifica=1&IdProgramma=" & hf_IdProgramma.Value)
            'Response.Redirect("TabProgetti.aspx?VengoDa=" & Costanti.VENGO_DA_VALUTAZIONE_QUALITA & "&popup=1&Modifica=1&idattivita=" & hf_IdProgramma.Value & "&Nazionale=" & TxtTipoprog1.Value)
        End If

    End Sub

    Private Sub imgElStorici_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgElStorici.Click
        CancellaMessaggi()
        If (cmdSalva.Visible = True) Then
            Salva(SENDER_PULSANTE_SALVA)
        End If
        If (msgErrore.Text = String.Empty) Then
            Response.Redirect("WebFrmStoricoProgrammi.aspx?VengoDa=" & Costanti.VENGO_DA_VALUTAZIONE_QUALITA & "&idprogramma=" & hf_IdProgramma.Value & "&Pagina=" & hf_Pagina.Value)
        End If

    End Sub

    Private Function ValidaDati() As Boolean
        Dim gruppiValutati As List(Of String) = New List(Of String)
        Dim errore As StringBuilder = New StringBuilder()
        Dim campiValidi As Boolean = True
        Dim numeroIntero As Integer
        Dim valoriConsentiti As List(Of Int32) = New List(Of Int32)(New Int32() {0, 1, 3, 5})
        Dim dataTmp As Date
        Dim dataValida As Boolean = True
        Dim messaggioDataValida As String = "Il valore di '{0}' non è valido. Inserire la data nel formato gg/mm/aaaa.<br/>"

        If Not (txtDataSeduta.Text = String.Empty) Then
            If (Date.TryParse(txtDataSeduta.Text, dataTmp) = False) Then
                errore.AppendLine(String.Format(messaggioDataValida, "DataSeduta"))
                dataValida = False
            End If
        End If


        If (errore.ToString() <> String.Empty) Then
            msgErrore.Text = errore.ToString()
            campiValidi = False
            'MaintainScrollPositionOnPostBack = False

            msgErrore.Focus()


        End If
        'ControlloNoteRiservate()
        Return campiValidi

    End Function

    'Private Sub ValidaNoteRiservate()


    '    If (Session("IdRegioneCompetenzaProgetto") = 22 And panelNoteRiservate.Visible = True) Then
    '        Dim warningAttivo As Boolean = False
    '        Dim warningNote As StringBuilder = New StringBuilder("Non sono state indicate Note Riservate per le seguenti domande:<br\>")
    '        If (lblNazione.Text = "Italiana") Then
    '            If (Txt24Assenti.Text = String.Empty And lbl31A.Visible = True) Then
    '                warningNote.AppendLine("Domanda 24 Punteggio 0.")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If
    '            If (Txt24Presenti.Text = String.Empty And lbl31P.Visible = True) Then
    '                warningNote.AppendLine("Domanda 24 Punteggio 1.")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If
    '            If (Txt25Assenti.Text = String.Empty And lbl32.Visible = True) Then
    '                warningNote.AppendLine("Domanda 25 Punteggio 0.")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If
    '            If (Txt28Assenti.Text = String.Empty And lbl28.Visible = True) Then
    '                warningNote.AppendLine("Domanda 28 Punteggio 0.")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If
    '            If (Txt29Assenti.Text = String.Empty And lbl28.Visible = True) Then
    '                warningNote.AppendLine("Domanda 29 Punteggio 0.")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If

    '        Else
    '            If (Txt24EsteroAssenti.Text = String.Empty And lbl24EsteroAssenti.Visible = True) Then
    '                warningNote.AppendLine("Domanda 24 Punteggio 0.")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If
    '            If ((Txt24Assenti.Text = String.Empty And lbl31A.Visible = True) Or (Txt24EsteroAssenti.Text = String.Empty And lbl24EsteroAssenti.Visible = True)) Then
    '                warningNote.Append("Domanda 31 Punteggio 0.")
    '                warningNote.Append("<br\>")
    '                warningAttivo = True
    '            End If
    '            If (Txt24Presenti.Text = String.Empty And lbl31P.Visible = True) Then
    '                warningNote.AppendLine("Domanda 31 Punteggio 1.")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If
    '            If (Txt25Assenti.Text = String.Empty And lbl32.Visible = True) Then
    '                warningNote.AppendLine("Domanda 32 Punteggio 0. ")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If
    '            If (Txt28Assenti.Text = String.Empty And lbl35.Visible = True) Then
    '                warningNote.AppendLine("Domanda 35 Punteggio 0.")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If
    '            If (Txt29Assenti.Text = String.Empty And lbl36.Visible = True) Then
    '                warningNote.AppendLine("Domanda 36 Punteggio 0.")
    '                warningNote.AppendLine("<br\>")
    '                warningAttivo = True
    '            End If
    '        End If
    '        If (warningAttivo = True And cmdConferma.Enabled = True And msgErrore.Text = String.Empty) Then
    '            msgInfo.Text = warningNote.ToString()
    '            'MaintainScrollPositionOnPostBack = False
    '            msgInfo.Focus()

    '        End If

    '    End If

    'End Sub

    Private Sub CheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        CancellaMessaggi()


        MaintainScrollPositionOnPostBack = True
        Dim check As New CheckBox
        check = DirectCast(sender, CheckBox)
        Dim nome = check.ValidationGroup
        Dim SelectedRadioButton = classeHelper.GetCheckBoxByValidationGroup(Me, check.ValidationGroup).FirstOrDefault(Function(x) x.Checked And x.ID <> check.ID)
        If SelectedRadioButton IsNot Nothing Then
            SelectedRadioButton.Checked = False
        End If
    End Sub
    Protected Sub ConfiguraPostBackSezioni()

        Dim isPostBack As Boolean = Me.IsPostBack
        Me.hdnIsPostbackCP.Value = isPostBack
        Me.hdnIsPostbackCO.Value = isPostBack
        Me.hdnIsPostbackCCA.Value = isPostBack
        Me.hdnIsPostbackCG.Value = isPostBack
        Me.hdnIsPostbackNOTE.Value = isPostBack
        Me.hdnIsPostbackMinoriOpportunita.Value = isPostBack
        Me.hdnIsPostbackPaeseUE.Value = isPostBack
        Me.hdnIsPostbackTutoraggio.Value = isPostBack
    End Sub

    Private Sub MascheraINLettura()
        Prog.Visible = False
        imgElStorici.Visible = False

        imgElencoDocumentiProg.Visible = False

        cmdStampaVQ.Visible = False
        imgStampaTot.Visible = False
        'FidsetSezioni.Visible = False
        ConfiguraPostBackSezioni()

    End Sub

    Protected Sub LinkButtonSx_Click(sender As Object, e As EventArgs) Handles LinkButtonSx.Click

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "Update UtentiUNSC set VisualizzaValQual='SX' where UserName='" & Session("Utente") & "'"

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        ClsServer.EseguiSqlClient(strsql, Session("conn"))


        Response.Redirect("WfrmMain.aspx")
        'ViewState.Clear()
    End Sub

    Protected Sub LinkButtonDx_Click(sender As Object, e As EventArgs) Handles LinkButtonDx.Click

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "Update UtentiUNSC set VisualizzaValQual='DX' where UserName='" & Session("Utente") & "'"

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        ClsServer.EseguiSqlClient(strsql, Session("conn"))
        Response.Redirect("WfrmMain.aspx")
        'ViewState.Clear()
    End Sub

    Function ControlloValutazioneProgetti(ByVal intidprogramma As Integer) As Boolean
        'Controllo se esistono progetti legati al programma non valutati formalmente
        Dim dtrProgramma As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = " SELECT Programmi.IdProgramma"
        strSql = strSql & " FROM Programmi "
        strSql = strSql & " left JOIN attività on Programmi.idprogramma=attività.idprogramma "
        strSql = strSql & " WHERE Programmi.idprogramma=" & intidprogramma & " "
        strSql = strSql & " AND attività.IDStatoattività=4 " ' -- STATO PROGETTO PROPOSTO

        dtrProgramma = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrProgramma.HasRows = True Then
            ControlloValutazioneProgetti = False
        Else
            ControlloValutazioneProgetti = True
        End If

        dtrProgramma.Close()
        dtrProgramma = Nothing
        Return ControlloValutazioneProgetti
    End Function

    Function ControlloNumeroProgettiAmmissibili(ByVal intidprogramma As Integer) As Boolean
        'Controllo se il numero di progetti ammissibili legati al programma sia almeno 2
        Dim dtrProgramma As SqlClient.SqlDataReader
        Dim strSql As String
        Dim nprog As Integer

        nprog = 0

        strSql = " SELECT count(*) AS NPROGETTI"
        strSql = strSql & " FROM Programmi "
        strSql = strSql & " left JOIN attività on Programmi.idprogramma=attività.idprogramma "
        strSql = strSql & " WHERE Programmi.idprogramma=" & intidprogramma & " "
        strSql = strSql & " AND attività.IDStatoattività=9 " ' -- STATO PROGETTO IN ATTESA GRADUATORIA

        dtrProgramma = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrProgramma.HasRows = True Then
            dtrProgramma.Read()
            nprog = dtrProgramma("NPROGETTI")
        End If

        If nprog >= 2 Then
            ControlloNumeroProgettiAmmissibili = True
        Else
            ControlloNumeroProgettiAmmissibili = False
        End If


        dtrProgramma.Close()
        dtrProgramma = Nothing

        Return ControlloNumeroProgettiAmmissibili
    End Function
End Class