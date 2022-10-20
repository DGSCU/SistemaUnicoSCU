Imports System.Drawing
Public Class Presenze
    Inherits System.Web.UI.Page
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'query per quel volontario se cisono dati bindo la griglia

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        '--------------------INIZIO SICUREZZA-----------------------------------
            'IDENTE  Session("IdEnte")
            'CODICEENTE  NZ Session("codiceregione")  or Session("txtCodEnte")

            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, Request.QueryString("IdEntita"), strIDENTE) = 1 Then

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
            Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Response.Redirect("wfrmAnomaliaDati.aspx")

            End If

        '---------------------FINE SICUREZZA---------------------------
        Dim anno As String
        Dim mese As String

        'Dim VengoDa As String
        'VengoDa = Request.QueryString("VengoDa")

        'Select Case VengoDa
        '    Case 1


        'End Select

        If Request.QueryString("VengoDa") = "1" _
            Or Request.QueryString("VengoDa") = "2" _
                Or Request.QueryString("VengoDa") = "ValidaDocumento" _
                Or Request.QueryString("VengoDa") = "DocumentiVolontario" _
                Or Request.QueryString("VengoDa") = "ConfermaPreliminareMultipla" _
                Or Request.QueryString("VengoDa") = "GeneraModuloZip" Then ' 1 significa che vengo dalla CheckList
            anno = Request.QueryString("anno")
            mese = Request.QueryString("mese")
        Else
            anno = Calendar1.TodaysDate.Year.ToString
            mese = Calendar1.TodaysDate.Month.ToString
        End If




        If IsPostBack = False Then
            If Session("TipoUtente") = "U" Then
                imgStoricoNotifiche.Visible = True
            End If
            caricaCombo()
            ddlMotivo.Visible = True

            CaricaDati()
            Dim ultimogiornomesevisualizzato As String
            Dim Data_fi As Date = "01/" & mese & "/" & anno

            ' Calcola l'ultimo giorno del mese corrente
            ultimogiornomesevisualizzato = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, +1, Data_fi))


            CaricaGrigliaComulativa(ultimogiornomesevisualizzato)
            CaricaGriglia(anno, mese)


            If Request.QueryString("VengoDa") = "1" _
                Or Request.QueryString("VengoDa") = "2" _
                    Or Request.QueryString("VengoDa") = "ValidaDocumento" _
                    Or Request.QueryString("VengoDa") = "DocumentiVolontario" _
                Or Request.QueryString("VengoDa") = "ConfermaPreliminareMultipla" _
                Or Request.QueryString("VengoDa") = "GeneraModuloZip" Then
                Calendar1.VisibleDate = "01/" & mese & "/" & anno
            Else
                Calendar1.VisibleDate = Calendar1.TodaysDate
            End If

            'Calendar1.VisibleDate.Date.ToString = Date.Now.ToString("dd/MM/yyyy")
            'Calendar1.SelectedDate.Now.Today.ToString()

        End If


        CaricaGrigliaPresenze(anno, mese)
        ControlloEsistenzaDocumento(anno, mese)

        AbilitaPulsantiValidazione(anno, mese)
        ControlloStatoConfermaMese(Session("IdEnte"), anno, mese)

        MaintainScrollPositionOnPostBack = True
    End Sub

    Private Sub Calendar1_SelectionChanged(sender As Object, e As System.EventArgs) Handles Calendar1.SelectionChanged
        'Dim cmdCommand As SqlClient.SqlCommand
        'Dim strsql As String
        'Dim Motivo As String
        Dim data As String
        Dim Ritorno As String()
        'Dim IdEntitàPresenza As Integer
        Dim identita As String
        Dim CausalePresenza As Integer
        Dim RitornoDati As DataSet
        Dim user As String
        Dim ente As Integer
        divConferma.Visible = False

        data = Calendar1.SelectedDate.Date.ToString("dd/MM/yyyy")

        RitornoDati = GetDataColoreCausale(data)




        'data = e.Day.Date.ToString("dd/mm/yyyy")
        'If Not IsNothing(RitornoDati) Then
        identita = Request.QueryString("identita")
        CausalePresenza = ddlMotivo.SelectedValue

        user = Session("Utente")
        ente = Session("idEnte")
        'Motivo = RitornoDati.Tables(0).Rows(0).Item("Descrizione")
        'IdEntitàPresenza = RitornoDati.Tables(0).Rows(0).Item("IdEntitàPresenza")


        '    If Motivo <> ddlMotivo.SelectedItem.Text Then

        '        'update

        '        Response.Write("AGGIORNO")
        '        strsql = "Update EntitàPresenze set idCausalePresenza= " & ddlMotivo.SelectedValue & ", UsernameUltimaModifica='" & Session("Utente") & "', DataUltimaModifica=getdate() where identità=" & Request.QueryString("identita") & " and IdEntitàPresenza=" & IdEntitàPresenza & ""
        '        cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))


        '    End If

        'Else

        Ritorno = ClsServer.EseguiPresenzeINS(identita, CausalePresenza, data, user, ente, Session("conn"))

        If Ritorno(0) = "POSITIVO" Then
            lblMessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
            lblMessaggio.Text = Ritorno(1)
            lblMessaggio.Font.Bold = True
            lblMessaggio.Font.Size = FontSize.Large

        Else 'NEGATIVO
            lblMessaggio.Text = Ritorno(1)
            lblMessaggio.Font.Bold = True
            lblMessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E41B17")
            lblMessaggio.Font.Size = FontSize.Large

            MaintainScrollPositionOnPostBack = False

        End If
        '    'insert
        '    Response.Write("INSERISCO") 'STORED


        '    strsql = "Insert into EntitàPresenze (idEntità,idCausalePresenza,giorno,UsernameInserimento,DataInserimento) "
        '    strsql = strsql & "values "
        '    strsql = strsql & "(" & Request.QueryString("identita") & "," & ddlMotivo.SelectedValue & ",'" & data & "','" & Session("Utente") & "', getdate() )"
        '    cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

        'End If



        Dim anno As String
        Dim mese As String
        anno = Calendar1.SelectedDate.Year.ToString
        mese = Calendar1.SelectedDate.Month.ToString



        Dim ultimogiornomesevisualizzato As String
        Dim Data_fi As Date = "01/" & mese & "/" & anno

        ' Calcola l'ultimo giorno del mese corrente
        ultimogiornomesevisualizzato = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, +1, Data_fi))

        CaricaGrigliaComulativa(ultimogiornomesevisualizzato)
        CaricaGriglia(anno, mese)

        ControlloStatoConfermaMese(Session("IdEnte"), anno, mese)

    End Sub 'Selection_Change 
    Protected Sub Calendar1_DayRender(ByVal sender As Object, ByVal e As DayRenderEventArgs) Handles Calendar1.DayRender
        ' Display vacation dates in yellow boxes with purple borders.


        Dim RitornoDati As DataSet
        Dim Causale As String
        Dim DescrizioneAbb As String
        Dim colore As String
        Dim data As String
        Dim giorno As String
        Dim mese As String
        Dim anno As String

        'If e.Day.IsOtherMonth = True Then
        '    'e.Day.IsSelectable = False
        '    'e.Cell.Visible = False
        '    'e.Cell.Width = New Unit(0)
        'End If


        data = e.Day.Date.ToString("dd/MM/yyyy")


        RitornoDati = GetDataColoreCausale(data)



        If Not IsNothing(RitornoDati) Then


            Dim Presente As New Style()
            With Presente
                .BackColor = System.Drawing.ColorTranslator.FromHtml("#33FF00") '33FF00
                .BorderColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                .ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                .Font.Bold = True
                .BorderWidth = New Unit(2)
            End With

            ' Display weekend dates in green boxes.
            Dim NonLavorativo As New Style()

            With NonLavorativo
                .BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC")
                .BorderColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                .ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                .Font.Bold = True
                .BorderWidth = New Unit(2)
            End With

            Dim TuttoIlResto As New Style()

            With TuttoIlResto
                .BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF33")
                .BorderColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                .ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                .Font.Bold = True
                .BorderWidth = New Unit(2)
            End With
            DescrizioneAbb = RitornoDati.Tables(0).Rows(0).Item("Codice")
            Causale = RitornoDati.Tables(0).Rows(0).Item("Descrizione")
            colore = RitornoDati.Tables(0).Rows(0).Item("Tipo")
            data = RitornoDati.Tables(0).Rows(0).Item("Giorno")
            giorno = Mid(data, 1, 2)
            mese = Mid(data, 4, 2)
            anno = Mid(data, 7, 4)
            'se lo stato dal db e' presente
            If colore = 1 Then
                If e.Day.Date = New Date(anno, mese, giorno) Then
                    e.Cell.ApplyStyle(Presente)
                    Dim aLabel As Label = New Label()
                    aLabel.Text = "<br />" & DescrizioneAbb
                    aLabel.Font.Size = FontSize.Large
                    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    e.Cell.Controls.Add(aLabel)

                End If
            End If

            'se lo stato da db e' non lavorativo
            If colore = 2 Then
                If e.Day.Date = New Date(anno, mese, giorno) Then
                    e.Cell.ApplyStyle(NonLavorativo)
                    Dim aLabel As Label = New Label()
                    aLabel.Text = "<br />" & DescrizioneAbb
                    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    aLabel.Font.Size = FontSize.Large
                    e.Cell.Controls.Add(aLabel)
                End If
            End If
            'se lo stato da db e' tutto il resto CCCCCC()
            If colore = 3 Then
                If e.Day.Date = New Date(anno, mese, giorno) Then
                    e.Cell.ApplyStyle(TuttoIlResto)

                    Dim aLabel As Label = New Label()
                    aLabel.Text = "<br />" & DescrizioneAbb
                    aLabel.Font.Size = FontSize.Large
                    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    e.Cell.Controls.Add(aLabel)
                    'Select Case Causale

                    'Case Is = "Permesso Retribuito"
                    '    Dim aLabel As Label = New Label()
                    '    aLabel.Text = "<br />" & DescrizioneAbb
                    '    aLabel.Font.Size = FontSize.Large
                    '    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    '    e.Cell.Controls.Add(aLabel)
                    'Case Is = "Malattia"
                    '    Dim aLabel As Label = New Label()
                    '    aLabel.Text = "<br />" & DescrizioneAbb
                    '    aLabel.Font.Size = FontSize.Large
                    '    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    '    e.Cell.Controls.Add(aLabel)
                    'Case Is = "Infortunio in Servizio"
                    '    Dim aLabel As Label = New Label()
                    '    aLabel.Text = "<br />" & DescrizioneAbb
                    '    aLabel.Font.Size = FontSize.Large
                    '    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    '    e.Cell.Controls.Add(aLabel)
                    'Case Is = "Maternità"
                    '    Dim aLabel As Label = New Label()
                    '    aLabel.Text = "<br />" & DescrizioneAbb
                    '    aLabel.Font.Size = FontSize.Large
                    '    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    '    e.Cell.Controls.Add(aLabel)
                    'Case Is = "Donazione Sangue"
                    '    Dim aLabel As Label = New Label()
                    '    aLabel.Text = "<br />" & DescrizioneAbb
                    '    aLabel.Font.Size = FontSize.Large
                    '    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    '    e.Cell.Controls.Add(aLabel)
                    'Case Is = "Diritto di Voto"
                    '    Dim aLabel As Label = New Label()
                    '    aLabel.Text = "<br />" & DescrizioneAbb
                    '    aLabel.Font.Size = FontSize.Large
                    '    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    '    e.Cell.Controls.Add(aLabel)
                    'Case Is = "Testimone"
                    '    Dim aLabel As Label = New Label()
                    '    aLabel.Text = "<br />" & DescrizioneAbb
                    '    aLabel.Font.Size = FontSize.Large
                    '    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    '    e.Cell.Controls.Add(aLabel)
                    'Case Is = "Altre Retribuite"
                    '    Dim aLabel As Label = New Label()
                    '    aLabel.Text = "<br />" & DescrizioneAbb
                    '    aLabel.Font.Size = FontSize.Large
                    '    aLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000")
                    '    e.Cell.Controls.Add(aLabel)

                    'End Select

                End If
            End If

        End If


    End Sub

    Function GetDataColoreCausale(ByVal dataformattata As String) As DataSet
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT  a.IDEntità, a.Cognome, a.Nome, EntitàPresenze.IDEntitàPresenza, EntitàPresenze.Giorno, CausaliPresenze.Descrizione ,CausaliPresenze.Codice, CausaliPresenze.Tipo FROM  entità as a  INNER JOIN EntitàPresenze ON a.IDEntità = EntitàPresenze.IDEntità INNER JOIN CausaliPresenze ON EntitàPresenze.IDCausalePresenza = CausaliPresenze.IDCausalePresenza where a.identità=" & Request.QueryString("identita") & " and giorno='" & dataformattata & "' order by EntitàPresenze.Giorno"
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            lblNome.Text = MyDataset.Tables(0).Rows(0).Item("Nome")
            LblCognome.Text = MyDataset.Tables(0).Rows(0).Item("Cognome")
            Return MyDataset

        End If

    End Function
    Sub caricaCombo()
        '***Generata da Bagnani Jonathan in data:04/11/04
        Dim strLocal As String
        'datareader locale 

        Dim MyDataset As DataSet
        'controllo se ci sono sedi di attuazione assegnate all'ente selezionato
        'Identita
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strLocal = "select IDCausalePresenza, Descrizione , Ordine "

        strLocal = strLocal & "from CausaliPresenze order by ordine "

        MyDataset = ClsServer.DataSetGenerico(strLocal, Session("conn"))
        ddlMotivo.DataSource = MyDataset
        ddlMotivo.DataTextField = "Descrizione"
        ddlMotivo.DataValueField = "idCausalePresenza"
        ddlMotivo.DataBind()

        'eseguo la query e passo il risultato al datareader

        'controllo se ci sono sedi di attuazione assegnate al volontario selezionato

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


    End Sub
    Private Sub ddlMotivo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMotivo.SelectedIndexChanged
        Calendar1.SelectedDate = Nothing
    End Sub

    Sub CaricaDati()
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT  IDEntità, Cognome, Nome,DataInizioServizio, DataFineServizio, CodiceVolontario from entità where identità=" & Request.QueryString("identita") & " "
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))


        'Il Volontario potrebbe non avere il codicevolontario quindi cresh.....
        If MyDataset.Tables(0).Rows.Count <> 0 Then
            lblCodiceVolontario.Text = MyDataset.Tables(0).Rows(0).Item("CodiceVolontario")
            lblNome.Text = MyDataset.Tables(0).Rows(0).Item("Nome")
            LblCognome.Text = MyDataset.Tables(0).Rows(0).Item("Cognome")
            lblInizioServizio.Text = MyDataset.Tables(0).Rows(0).Item("DataInizioServizio")
            lblFineServizio.Text = MyDataset.Tables(0).Rows(0).Item("DataFineServizio")
        End If

        MyDataset.Dispose()



    End Sub
    Sub CaricaGriglia(ByVal anno As String, ByVal mese As String)
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT Count(EntitàPresenze.Giorno) as Totale, left(CausaliPresenze.Descrizione,30) as Descrizione,CausaliPresenze.Codice, CausaliPresenze.Ordine FROM  EntitàPresenze  INNER JOIN CausaliPresenze ON EntitàPresenze.IDCausalePresenza = CausaliPresenze.IDCausalePresenza where EntitàPresenze.identità=" & Request.QueryString("identita") & "  AND (YEAR(EntitàPresenze.Giorno) = '" & anno & "') AND (MONTH(EntitàPresenze.Giorno) = '" & mese & "') Group by  left(CausaliPresenze.Descrizione,30) ,CausaliPresenze.Codice, CausaliPresenze.Ordine order by CausaliPresenze.Ordine "
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            DtgRiepilogoPresenze.DataSource = MyDataset
            DtgRiepilogoPresenze.DataBind()
            fildGrigliaMese.Visible = True

        Else
            DtgRiepilogoPresenze.DataSource = Nothing
            DtgRiepilogoPresenze.DataBind()
            fildGrigliaMese.Visible = False
        End If
        lblTotale.Text = "0"
        For i = 0 To DtgRiepilogoPresenze.Items.Count - 1

            lblTotale.Text = lblTotale.Text + Convert.ToDouble(DtgRiepilogoPresenze.Items(i).Cells(2).Text)
        Next


        MyDataset.Dispose()


    End Sub
    Sub CaricaGrigliaComulativa(ByVal ultimogiornomesevisualizzato As String)
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT Count(EntitàPresenze.Giorno) as Totale,  left(CausaliPresenze.Descrizione,30) as Descrizione ,CausaliPresenze.Codice, CausaliPresenze.Ordine  FROM  EntitàPresenze  RIGHT JOIN CausaliPresenze ON EntitàPresenze.IDCausalePresenza = CausaliPresenze.IDCausalePresenza AND EntitàPresenze.identità=" & Request.QueryString("identita") & "  AND EntitàPresenze.Giorno <= '" & ultimogiornomesevisualizzato & "' where CausaliPresenze.idcausalepresenza in (3,4) Group by  left(CausaliPresenze.Descrizione,30) ,CausaliPresenze.Codice, CausaliPresenze.Ordine Order by CausaliPresenze.Ordine "
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        'If MyDataset.Tables(0).Rows.Count <> 0 Then
        DtgRiepilogoPresenzeinizioservizio.DataSource = MyDataset
        DtgRiepilogoPresenzeinizioservizio.DataBind()
        'fildGrigliaInizioServizio.Visible = True
        'Else
        'DtgRiepilogoPresenzeinizioservizio.DataSource = Nothing
        'DtgRiepilogoPresenzeinizioservizio.DataBind()
        'fildGrigliaInizioServizio.Visible = False
        'End If

        MyDataset.Dispose()


    End Sub
    Private Sub Calendar1_VisibleMonthChanged(sender As Object, e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles Calendar1.VisibleMonthChanged
        lblMessaggio.Text = ""
        'ControlloPresenzaPDF()
        Dim mese As String
        Dim anno As String
        anno = Calendar1.VisibleDate.Year.ToString
        mese = Calendar1.VisibleDate.Month.ToString

        CaricaGrigliaPresenze(anno, mese)

       
        '----------------------------------------------------------------------------------------
        RicaricaCalendario()
        hlDw.Visible = False
        ControlloEsistenzaDocumento(anno, mese)

        AbilitaPulsantiValidazione(anno, mese)
        ControlloStatoConfermaMese(Session("IdEnte"), anno, mese)
    End Sub
    Sub RicaricaCalendario()
        'Dim mese As String
        'Dim anno As String
        'Dim mesecorrente As String
        'mesecorrente = Month(Date.Today).ToString
        'If Calendar1.VisibleDate.Month.ToString <> mesecorrente Then
        '    anno = Calendar1.VisibleDate.Year.ToString
        '    mese = Calendar1.VisibleDate.Month.ToString
        '    CaricaGriglia(anno, mese)
        'Else
        '    
        '        anno = Calendar1.TodaysDate.Year.ToString
        '        mese = Calendar1.TodaysDate.Month.ToString
        '   
        '    CaricaGriglia(anno, mese)
        'End If

        CaricaGriglia(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)

        Dim ultimogiornomesevisualizzato As String
        Dim Data_fi As Date = "01/" & Calendar1.VisibleDate.Month.ToString & "/" & Calendar1.VisibleDate.Year.ToString

        ' Calcola l'ultimo giorno del mese corrente
        ultimogiornomesevisualizzato = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, +1, Data_fi))


        CaricaGrigliaComulativa(ultimogiornomesevisualizzato)
    End Sub


    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click

        Select Case Request.QueryString("VengoDa")
            Case "1"
                Response.Redirect("WfrmCheckListDettaglio.aspx?idLista=" & Request.QueryString("IdLista"))
            Case "2"
                Response.Redirect("WfrmCheckListDettaglioIndividuale.aspx?idLista=" & Request.QueryString("IdLista"))
            Case "ValidaDocumento" 'approvazione documenti volontario
                Response.Redirect("WfrmRiceraVolontariValidaDocumenti.aspx?Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Cognome=" & Request.QueryString("Cognome") & "&Nome=" & Request.QueryString("Nome") & "&CodVolontario=" & Request.QueryString("CodVolontario") & "&CodProgetto=" & Request.QueryString("CodProgetto") & "&CodSede=" & Request.QueryString("CodSede") & "&Doc=" & Request.QueryString("Doc") & "&StatoDoc=" & Request.QueryString("StatoDoc") & "&DataIS = " & Request.QueryString("DataIS") & "&VengoDa=" & Request.QueryString("VengoDa") & "&IdEntita=" & Request.QueryString("IdEntita") & "&IdRegione=" & Request.QueryString("IdRegione"))
            Case "DocumentiVolontario"
                Response.Redirect("WfrmVisualizzaElencoDocumentiVolontario.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdVol=" & Request.QueryString("IdEntita"))
            Case "ConfermaPreliminareMultipla"
                Response.Redirect("WfrmConfermaPreliminare.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdVol=" & Request.QueryString("IdEntita") & "&AnnoMese=" & Request.QueryString("AnnoMese"))
            Case "GeneraModuloZip"
                Response.Redirect("WfrmGeneraModuloZip.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdVol=" & Request.QueryString("IdEntita") & "&AnnoMese=" & Request.QueryString("AnnoMese"))

            Case Else
                Response.Redirect("ricercavolontaripresenze.aspx")
        End Select

    End Sub

    Protected Sub cmdUpload_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click
        Try
            Dim msg As String
            Dim PrefissoFile As String = ""
            lblMessaggio.Text = ""
            Dim DataPresenza As Date
            Dim DataDiOggi As Date
            DataDiOggi = Date.Now.ToString("dd/MM/yyyy")


            DataPresenza = "1" & "/" & Calendar1.VisibleDate.Month.ToString & "/" & Calendar1.VisibleDate.Year.ToString
            DataPresenza = DataPresenza.ToString("dd/MM/yyyy")
            'hlDw.Visible = False
            If clsGestioneDocumenti.VerificaEstensioneFile(txtSelFile) = False Then
                lblMessaggio.Text = "Il formato del file non è corretto.E' possibile associare documenti nel formato .PDF o .PDF.P7M"
                lblMessaggio.ForeColor = Color.Red

                MaintainScrollPositionOnPostBack = False

                ControlloStatoConfermaMese(Session("IdEnte"), Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
                AbilitaPulsantiValidazione(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
                Exit Sub
            End If
            If clsGestioneDocumenti.VerificaPrefissiDocumentiPresenze(txtSelFile, Session("conn"), PrefissoFile) = False Then
                lblMessaggio.Text = "Utilizzare uno dei prefissi consentiti per il nome del file."
                lblMessaggio.ForeColor = Color.Red

                MaintainScrollPositionOnPostBack = False

                ControlloStatoConfermaMese(Session("IdEnte"), Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
                AbilitaPulsantiValidazione(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
                Exit Sub
            End If

            Dim Ritorno As String()
            Ritorno = VERIFICA_CONGRUENZA_DATE_UPLOAD(Request.QueryString("IdEntita"), Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month)

            If Ritorno(0) = "NEGATIVO" Then
                lblMessaggio.Text = Ritorno(1)
                lblMessaggio.Font.Bold = True
                lblMessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E41B17")
                lblMessaggio.Font.Size = FontSize.Large

                MaintainScrollPositionOnPostBack = False

                ControlloStatoConfermaMese(Session("IdEnte"), Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
                AbilitaPulsantiValidazione(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)

                Exit Sub
            End If

            'If VerificaAbilitazioneMenuValidazioneDocumenti(Session("Utente")) = True Then
            '    'utente abilitato alla modifica di mesi confermati
            'Else
            '    If clsGestioneDocumenti.ControlloPresenzeConfermate(Session("idente"), DataPresenza, Session("Conn")) = False Then
            '        'no in 0-1 puo caricare il nuovo documento
            '        If ControlloAggiornamentoDocumento(Request.QueryString("IdEntita"), Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString) = False Then
            '            lblMessaggio.Text = "Non e' possibile caricare o modificare presenze già CONFERMATE"
            '            lblMessaggio.ForeColor = Color.Red
            '            Exit Sub
            '        End If

            '    End If

            '    If ControlloEsistenzaDocumento(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString) = True Then
            '        If ControlloAggiornamentoDocumento(Request.QueryString("IdEntita"), Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString) = False Then
            '            lblMessaggio.Text = "Per il mese selezionato è già presente il file. Per aggiornarlo rimuovere il file esistente."
            '            lblMessaggio.ForeColor = Color.Red
            '            Exit Sub
            '        End If

            '    End If
            'End If
            msg = clsGestioneDocumenti.CaricaDocumentoPresenzeEntità(Request.QueryString("IdEntita"), Session("Utente"), txtSelFile, DataDiOggi, DataPresenza, Session("conn"), PrefissoFile)
            If msg <> "" Then
                lblMessaggio.Text = msg
                lblMessaggio.ForeColor = Color.Red
            Else
                lblMessaggio.Text = "Presenze Caricate Con Successo"
                lblMessaggio.ForeColor = Color.Navy
                idupload.Visible = False 'aggiunto da danilo il 14/07/2015
                CaricaGrigliaPresenze(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
            End If

            MaintainScrollPositionOnPostBack = False

        Catch ex As Exception
            lblMessaggio.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            lblMessaggio.ForeColor = Color.Red

            MaintainScrollPositionOnPostBack = False
        Finally
            cmdUpload.Enabled = True
        End Try
        ControlloStatoConfermaMese(Session("IdEnte"), Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
        AbilitaPulsantiValidazione(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
        'ControlloEsistenzaDocumento(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
    End Sub

    Sub CaricaGrigliaPresenze(ByVal anno As String, ByVal mese As String)
        Dim strsql As String
        Dim MyDataset As DataSet
        'strsql = "select Mese, Anno from EntiConfermaPresenze where (Anno=" & anno & ") and IdEnte='" & Session("idente") & "' and (Mese=" & mese & ") "
        strsql = " SELECT idEntitàDocumento,FileName,dataInserimento,usernameInserimento,HashValue, " & _
                 " CASE EntitàDocumenti.Stato WHEN 0 then 'Da Validare' WHEN 1 then 'Validato' WHEN 2 then 'Non Valido' END as StatoDocumento " & _
                 " from EntitàDocumenti where identità=" & Request.QueryString("identita") & " and mese=" & Calendar1.VisibleDate.Month & " and anno= " & Calendar1.VisibleDate.Year & " "
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            gridPresenze.DataSource = MyDataset
            gridPresenze.DataBind()
            'gridPresenze.Rows.Item(0).Cells(1).Visible = False
            'gridPresenze.Columns(1).Visible = False
        Else
            gridPresenze.DataSource = Nothing
            gridPresenze.DataBind()


        End If

        MyDataset.Dispose()


    End Sub

    Private Function ControlloEsistenzaDocumento(ByVal anno As String, ByVal mese As String)
        'As Boolean
        Dim strsql As String
        Dim MyDataset As DataSet
        'strsql = "select Mese, Anno from EntiConfermaPresenze where (Anno=" & anno & ") and IdEnte='" & Session("idente") & "' and (Mese=" & mese & ") "
        strsql = "SELECT idEntitàDocumento,FileName,dataInserimento,usernameInserimento,HashValue,stato " & _
                 " from EntitàDocumenti where identità=" & Request.QueryString("identita") & "  " & _
                 " and mese=" & Calendar1.VisibleDate.Month & " and anno= " & Calendar1.VisibleDate.Year & " " & _
                 " and stato in (0,1) "




        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count > 0 Then
            idupload.Visible = False
            ''se statoDocumento = NON VALIDATO e TipoUtento =E (l'ente può ricaricare il file delle presenze)
            'If MyDataset.Tables(0).Rows(0).Item("stato") = 2 And Session("TipoUtente") = "E" Then
            '    idupload.Visible = True
            'Else
            '    idupload.Visible = False
            'End If
            'Return True
        Else
            idupload.Visible = True
            ' Return False
        End If

    End Function

    Private Sub gridPresenze_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridPresenze.RowCommand

        Dim DataPresenza As Date
        DataPresenza = "1" & "/" & Calendar1.VisibleDate.Month.ToString & "/" & Calendar1.VisibleDate.Year.ToString
        DataPresenza = DataPresenza.ToString("dd/MM/yyyy")
        If e.CommandName = "Elimina" Then
            If clsGestioneDocumenti.ControlloPresenzeConfermate(Session("idente"), DataPresenza, Session("Conn")) = False Then

                lblMessaggio.Text = "Non e' possibile caricare o modificare le presenze già CONFERMATE"
                lblMessaggio.ForeColor = Color.Red
                AbilitaPulsantiValidazione(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
                ControlloStatoConfermaMese(Session("idente"), Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)

                MaintainScrollPositionOnPostBack = False
                Exit Sub
            Else
                Dim iddaeliminare As Integer

                Dim index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = gridPresenze.Rows(index)

                iddaeliminare = row.Cells(1).Text

                ' iddaeliminare = gridPresenze.Rows.Item(0).Cells(1).Text

                clsGestioneDocumenti.RimuoviPresenze(iddaeliminare, Session("Conn"))
                CaricaGrigliaPresenze(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
                lblMessaggio.Text = "Cancellazione Effettuata"
                lblMessaggio.ForeColor = Color.Navy
                'delete
                hlDw.Visible = False
                idupload.Visible = True

                MaintainScrollPositionOnPostBack = False
            End If

        End If
        If e.CommandName = "Scarica" Then

            'Dim strsql As String
            'Dim MyDataset As DataSet
            Dim identitadocumento As Integer

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gridPresenze.Rows(index)

            identitadocumento = row.Cells(1).Text
            hlDw.Visible = True
            hlDw.NavigateUrl = clsGestioneDocumenti.RecuperaDocumentoVolontario(identitadocumento, Session("Utente"), Session("conn"))
            hlDw.Text = row.Cells(2).Text 'MyDataset.Tables(0).Rows(0).Item("FileName")
            hlDw.Target = "_blank"

            'strsql = "SELECT * from EntitàDocumenti where identità=" & Request.QueryString("identita") & " and mese=" & Calendar1.VisibleDate.Month & " and anno= " & Calendar1.VisibleDate.Year & " "
            'MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

            'If MyDataset.Tables(0).Rows.Count <> 0 Then

            '    hlDw.Visible = True
            '    identitadocumento = MyDataset.Tables(0).Rows(0).Item("IdEntitàDocumento")
            '    hlDw.NavigateUrl = clsGestioneDocumenti.RecuperaDocumentoVolontario(identitadocumento, Session("Utente"), Session("conn"))
            '    hlDw.Text = MyDataset.Tables(0).Rows(0).Item("FileName")
            '    hlDw.Target = "_blank"

            'End If

            'MyDataset.Dispose()
            'Correzione Bugs 23/06/2015 ADC


        End If
        AbilitaPulsantiValidazione(Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
        ControlloStatoConfermaMese(Session("idente"), Calendar1.VisibleDate.Year.ToString, Calendar1.VisibleDate.Month.ToString)
    End Sub

    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim strMg As String
        strMg = ValidaDocumento(1)

        lblMessaggio.Text = strMg
        MaintainScrollPositionOnPostBack = False

        CaricaGrigliaPresenze(Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month)
        VerificaStatoDocumento(Request.QueryString("identita"), Calendar1.VisibleDate.Month, Calendar1.VisibleDate.Year)
        ControlloStatoConfermaMese(Session("idente"), Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month)
    End Sub

    Protected Sub cmdRespingi_Click(sender As Object, e As EventArgs) Handles cmdRespingi.Click
        Dim strMg As String
        Dim esito As String
        strMg = ValidaDocumento(2)
        'lblMessaggio.Visible = True
        lblMessaggio.Text = strMg

        MaintainScrollPositionOnPostBack = False

        VerificaStatoDocumento(Request.QueryString("identita"), Calendar1.VisibleDate.Month, Calendar1.VisibleDate.Year)
        CaricaGrigliaPresenze(Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month)
        ControlloStatoConfermaMese(Session("idente"), Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month)
        'richiamo popup per inoltro email di comunicazione all'ente

        OpenWindow()

        ''richiamo store per invio mail di notifica all'ente
        'InvioMailEnte(Request.QueryString("identita"), Session("IDEnte"), Calendar1.VisibleDate.Month, Calendar1.VisibleDate.Year, Session("Conn"), strMg, Esito)
        'If esito = "NEGATIVO" Then
        '    lblMessaggio.ForeColor = Color.Red
        'Else
        '    lblMessaggio.ForeColor = Color.Navy
        'End If
    End Sub
    'update STATO in entitàdocumento
    Private Function ValidaDocumento(ByVal StatoDocumento As Integer) As String
        Dim strMg As String
        Dim esito As String
        Dim strsql As String
        Dim MyDataset As DataSet
        Dim identitadocumento As Integer
        strsql = "SELECT IdEntitàDocumento from EntitàDocumenti where identità=" & Request.QueryString("identita") & " and mese=" & Calendar1.VisibleDate.Month & " and anno= " & Calendar1.VisibleDate.Year & " and stato=0 "
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            identitadocumento = MyDataset.Tables(0).Rows(0).Item("IdEntitàDocumento")
        End If
        MyDataset.Dispose()

        ClsServer.ConfermaDocumento(Session("Utente"), identitadocumento, StatoDocumento, "PRESENZE", Session("IdEnte"), Session("Conn"), strMg, Esito)
        If esito = "NEGATIVO" Then
            lblMessaggio.ForeColor = Color.Red
        Else
            lblMessaggio.ForeColor = Color.Navy
        End If

        MaintainScrollPositionOnPostBack = False

        Return strMg

    End Function

    Private Sub VerificaStatoDocumento(ByVal IDEntità As Integer, ByVal mese As String, ByVal anno As String)
        Dim strMg As String
        Dim strsql As String
        Dim MyDataset As DataSet
        Dim stato As Integer
        strsql = "SELECT top 1 Stato from EntitàDocumenti where identità=" & IDEntità & " and mese=" & mese & " and anno= " & anno & " order by identitàdocumento desc"
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            stato = MyDataset.Tables(0).Rows(0).Item("Stato")
        End If
        MyDataset.Dispose()
        Select Case stato
            Case 0 'davalidare
                CmdConferma.Visible = True
                cmdRespingi.Visible = True
                CmdAnnullaValidazione.Visible = False
            Case 1 'validato positivo
                CmdConferma.Visible = False
                cmdRespingi.Visible = False
                CmdAnnullaValidazione.Visible = True
            Case 2 'validato negativo
                idupload.Visible = True
                CmdConferma.Visible = False
                cmdRespingi.Visible = False
                CmdAnnullaValidazione.Visible = True
        End Select
    End Sub

    Private Function VerificaAbilitazioneMenuValidazioneDocumenti(ByVal Utente As String) As Boolean
        'Agg da  Simona Cordella il 30/04/2015
        'Verifico se l'utene U è autorizzato alla visualizzazione del menu ValidazioneDocumenti
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
                 " WHERE VociMenu.descrizione = 'Validazione Documenti'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

        VerificaAbilitazioneMenuValidazioneDocumenti = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Sub AbilitaPulsantiValidazione(anno As String, mese As String)
        'agg. il 28/04/2015 
        If VerificaAbilitazioneMenuValidazioneDocumenti(Session("Utente")) = True Then
            If gridPresenze.Rows.Count <> 0 Then
                'verificastatodocumento per abilitazione pulsanti di validazione documento
                VerificaStatoDocumento(Request.QueryString("identita"), mese, anno)

            Else
                CmdConferma.Visible = False
                cmdRespingi.Visible = False
                CmdAnnullaValidazione.Visible = False
            End If
        Else
            CmdConferma.Visible = False
            cmdRespingi.Visible = False
            CmdAnnullaValidazione.Visible = False
        End If
    End Sub

    Private Sub ControlloStatoConfermaMese(ByVal IdEnte As Integer, ByVal Anno As String, ByVal mese As String)
        'Select stato from FN_PRESENZE_STATO_CONFERMA_MENSILE (456) WHERE ANNO='2015' AND MESE ='03' 
        Dim strSql As String
        Dim rstStato As SqlClient.SqlDataReader
        Dim strRitornaStato As String
        Dim DataInizio As String

        strSql = "SELECT IdEnteConfermaPresenze from EntiConfermaPresenze where idente =" & IdEnte & " and mese = '" & mese & "' and anno ='" & Anno & "'"
        rstStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        If rstStato.HasRows = True Then
            strRitornaStato = "CONFERMATO"
        Else
            strRitornaStato = "DA CONFERMARE"
        End If
        If Not rstStato Is Nothing Then
            rstStato.Close()
            rstStato = Nothing
        End If
        lblStatoConfermaMese.Text = strRitornaStato

        strSql = "select valore from Configurazioni where parametro = 'PRESENZE_V2'"
        rstStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        If rstStato.HasRows = True Then
            rstStato.Read()
            DataInizio = rstStato("valore")
            If Not rstStato Is Nothing Then
                rstStato.Close()
                rstStato = Nothing
            End If

            strSql = "Select Mensilità"
            strSql = strSql & " from COMP_Mensilità"
            strSql = strSql & " WHERE"
            strSql = strSql & " Mensilità >=  CONVERT(VARCHAR(10),YEAR('" & DataInizio & "')) + '-' + RIGHT('0' +CONVERT(VARCHAR(10),MONTH('" & DataInizio & "')),2)"
            strSql = strSql & " and anno = " & Anno & " and mese = " & mese

            rstStato = ClsServer.CreaDatareader(strSql, Session("conn"))
            If rstStato.HasRows = True Then
                If Not rstStato Is Nothing Then
                    rstStato.Close()
                    rstStato = Nothing
                End If
                strSql = "SELECT a.* FROM EntitàPresenzeConfermaPreliminare a inner join COMP_Mensilità b on a.mensilità = b.mensilità WHERE a.IdEntità= " & Request.QueryString("identita") & " AND b.anno = " & Anno & " and b.mese = " & mese
                rstStato = ClsServer.CreaDatareader(strSql, Session("conn"))
                If rstStato.HasRows = True Then
                    strRitornaStato = "CONFERMATO"
                Else
                    strRitornaStato = "DA CONFERMARE"
                End If
            Else
                strRitornaStato = "NON NECESSARIO"
            End If
        Else
            strRitornaStato = "NON NECESSARIO"
        End If
        If Not rstStato Is Nothing Then
            rstStato.Close()
            rstStato = Nothing
        End If

        lblStatoConfermaPreliminare.Text = strRitornaStato

    End Sub

    Private Function ControlloAggiornamentoDocumento(ByVal identità As Integer, ByVal anno As String, ByVal mese As String) As Boolean
        'verifico se esiste almeno un documento VALIDA o DA VALIDARE non posso caricare un nuovo file  su un mese confermato
        Dim strsql As String
        Dim MyDataset As DataSet

        strsql = "SELECT idEntitàDocumento,FileName,dataInserimento,usernameInserimento,HashValue,stato from EntitàDocumenti " & _
                 " where identità=" & identità & " and mese=" & mese & " and anno= " & anno & " and not Stato in (0,1)"
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function InvioMailEnte(ByVal IdEntità As Integer, ByVal IdEnte As Integer, ByVal mese As String, ByVal anno As String, ByVal connessione As SqlClient.SqlConnection, Optional ByRef msg As String = "", Optional ByRef Esito As String = "") As String
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  18/05/2015
        'FUNZIONALITA': RICHIAMO STORE PER LA VALIDITA' DEI DOCUMENTI
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_VOLONTARI_MAIL_PRESENZE_NONVALIDATE]"


        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, connessione)
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IdEntità", SqlDbType.Int).Value = IdEntità
            sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            sqlCMD.Parameters.Add("@Mese", SqlDbType.VarChar).Value = mese
            sqlCMD.Parameters.Add("@anno", SqlDbType.VarChar).Value = anno

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            Dim str As String
            msg = sqlCMD.Parameters("@Messaggio").Value
            Esito = sqlCMD.Parameters("@Esito").Value
            'Return str

        Catch ex As Exception

            Exit Function
        End Try
    End Function
    'apertura maschera popup per invio testo email
    Protected Sub OpenWindow()
        Dim url As String = "WfrmCheckListNotificaMailPresenze.aspx?IdVol=" & Request.QueryString("identita") & "&Mese=" & Calendar1.VisibleDate.Month & "&Anno=" & Calendar1.VisibleDate.Year & "&VengoDa=" & 5

        Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"

        ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

    End Sub

    Private Sub AnnullaValidazioneDocumenti(ByVal IdEntitàDocumento As Integer, Optional ByRef msg As String = "", Optional ByRef Esito As String = "")
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  26/08/2015
        'FUNZIONALITA': RICHIAMO STORE PER ANNULLARE LA VALIDAZIONE DELLE PRESENZE

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ANNULLA_VALIDAZIONE_DOCVOL]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEntitàDocumento", SqlDbType.Int).Value = IdEntitàDocumento


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 10
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 255
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()

            msg = sqlCMD.Parameters("@Messaggio").Value
            Esito = sqlCMD.Parameters("@Esito").Value

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Protected Sub CmdAnnullaValidazione_Click(sender As Object, e As EventArgs) Handles CmdAnnullaValidazione.Click
        Dim strMg As String
        Dim esito As String
        Dim strsql As String
        Dim MyDataset As DataSet
        Dim identitadocumento As Integer

        strsql = "SELECT top 1 IdEntitàDocumento from EntitàDocumenti where identità=" & Request.QueryString("identita") & " and mese=" & Calendar1.VisibleDate.Month & " and anno= " & Calendar1.VisibleDate.Year & " order by IdEntitàDocumento desc "
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            identitadocumento = MyDataset.Tables(0).Rows(0).Item("IdEntitàDocumento")
        End If
        MyDataset.Dispose()
        AnnullaValidazioneDocumenti(identitadocumento, strMg, esito)
        lblMessaggio.Text = strMg
        If esito = "NEGATIVO" Then
            lblMessaggio.ForeColor = Color.Red
        Else
            lblMessaggio.ForeColor = Color.Navy
        End If

        MaintainScrollPositionOnPostBack = False

        CaricaGrigliaPresenze(Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month)
        VerificaStatoDocumento(Request.QueryString("identita"), Calendar1.VisibleDate.Month, Calendar1.VisibleDate.Year)
        ControlloStatoConfermaMese(Session("idente"), Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month)
    End Sub

    Private Sub imgStoricoNotifiche_Click(sender As Object, e As System.EventArgs) Handles imgStoricoNotifiche.Click
        Dim JScript As String

        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmVisualizzaStoricoNotifiche.aspx?IdTipoNotifica=2&IdEntita=" & Request.QueryString("identita") & """, """", ""height=768,width=1024, ,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf
        JScript &= ("</script>")
        Response.Write(JScript)
    End Sub


    Private Function GeneraModuloPresenze(ByVal intIdEntità As Integer, ByVal intAnno As Integer, ByVal intMese As Integer, ByRef strMessaggio As String) As Byte()
        Dim documento As New AsposeWord
        Try
            documento.open(Server.MapPath("download\Master\Volontari\ModuloPresenze.docx"))
        Catch ex As Exception
            'Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Template non valido", ex)
            strMessaggio = "Errore nella generazione del documento"
            Exit Function
        End Try

        Dim strsql As String
        Dim MyDataset As DataSet
        Dim strProgetto As String
        Dim strVolontario As String
        Dim strEnte As String

        strsql = "SELECT e.denominazione + ' (' + e.codiceregione + ')' as Ente, d.titolo + ' (' + d.codiceente + ')' as Progetto, a.Cognome + ' ' + a.Nome + ' (' + a.codicevolontario + ')' as Volontario from Entità a inner join attivitàentità b on a.identità = b.identità and b.idstatoattivitàentità = 1 inner join attivitàentisediattuazione c on b.idattivitàentesedeattuazione = c.idattivitàentesedeattuazione inner join attività d on c.idattività = d.idattività inner join enti e on d.identepresentante = e.idente where a.identità=" & intIdEntità
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            strProgetto = MyDataset.Tables(0).Rows(0).Item("Progetto")
            strVolontario = MyDataset.Tables(0).Rows(0).Item("Volontario")
            strEnte = MyDataset.Tables(0).Rows(0).Item("Ente")
        End If
        MyDataset.Dispose()


        'Dim managerAntimafia As New clsRuoloAntimafia()
        'Dim ruoli = managerAntimafia.GetRuoliAntimafia(Session("IdEnte"), Session("conn"))
        'Dim managerEnte = New clsEnte()
        'Dim ente = managerEnte.GetDatiEnte(Session("IdEnte"), Session("conn"))

        documento.addFieldValue("Ente", strEnte)
        documento.addFieldValue("Progetto", strProgetto)
        documento.addFieldValue("AnnoMese", intAnno & " - " & intMese)
        documento.addFieldValue("Volontario", strVolontario)
        documento.addFieldValue("Data", Date.Today.ToString("dd/MM/yyyy"))


        Dim html As New StringBuilder
        html.Append("<style>")
        html.Append("table {width:100%; border-collapse: collapse; font-size:10pt; margin-bottom:1em;}")
        html.Append("table, th, td {border: 1px solid lightgray;}")
        html.Append("table tr:nth-child(even) {background-color:#eee}")
        html.Append("td {padding:1pt; font-family:'courier new';}")
        html.Append(".space {width:50%;height:1em;}")
        html.Append("</style>")

        html.Append("<table><tbody>")
        html.Append("<tr style='fort-weight:bold'>")
        html.Append("<th>Giorno</th>")
        html.Append("<th>Codice Causale</th>")
        html.Append("<th>Descrizione Causale</th>")
        html.Append("</tr>")
        Dim rigaPari As Boolean = True

        strsql = "select dbo.formatodata(a.Giorno) as Giorno, b.Codice , b.Descrizione from EntitàPresenze a inner join CausaliPresenze b on a.IDCausalePresenza = b.IDCausalePresenza where a.IDEntità = " & intIdEntità & " and year(a.Giorno) = " & intAnno & " and month(a.giorno) = " & intMese & " order by a.giorno "
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        Dim myrow As Data.DataRow
        If MyDataset.Tables(0).Rows.Count <> 0 Then
            For Each myrow In MyDataset.Tables(0).Rows
                html.Append("<tr>")
                html.Append("<td>" & myrow.Item("Giorno").ToString & "</td>")
                html.Append("<td>" & myrow.Item("Codice").ToString & "</td>")
                html.Append("<td>" & myrow.Item("Descrizione").ToString & "</td>")
                html.Append("</tr>")
            Next
        End If
        MyDataset.Dispose()

        'For Each ruolo In ruoli
        '    html.Append("<tr>")
        '    html.Append("<td>" & ruolo.CodiceFiscaleEnte & "</td>")
        '    html.Append("<td>" & ruolo.CodiceFiscale & "</td>")
        '    html.Append("<td>" & ruolo.Cognome & "</td>")
        '    html.Append("<td>" & ruolo.Nome & "</td>")
        '    html.Append("<td>" & ruolo.Ruolo & "</td>")
        '    html.Append("</tr>")
        'Next

        html.Append("</tbody></table>")
        html.Append("</br>")

        documento.addFieldHtml("htmlCalendario", html.ToString)

        Try
            documento.merge()

            Return documento.pdfBytes


        Catch ex As Exception
            'Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Scrittura template", ex)
            strMessaggio = "Errore nella generazione del documento"
            Exit Function
        End Try
        'Log.Information(LogEvent.PRESENTAZIONE_RIEPILOGO)
        'Response.Clear()
        'Response.ContentType = "Application/pdf"
        'Response.AddHeader("Content-Disposition", "attachment; filename=" & lblCodiceVolontario.Text & "_" & Calendar1.VisibleDate.Year & "_" & Calendar1.VisibleDate.Month & ".pdf")
        'Response.BinaryWrite(documento.pdfBytes)
        'Response.End()
    End Function

    Private Sub cmdModuloPrecompilato_Click(sender As Object, e As System.EventArgs) Handles cmdModuloPrecompilato.Click
        Dim myfile As Byte()
        Dim stresito As String

        myfile = ClsServer.GeneraModuloPresenze(Request.QueryString("identita"), Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month, Server.MapPath("download\Master\Volontari\ModuloPresenze.docx"), Session("Utente"), Session("conn"), stresito)
        ' myfile = GeneraModuloPresenze(Request.QueryString("identita"), Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month, stresito)

        If stresito = "" Then
            Response.Clear()
            Response.ContentType = "Application/pdf"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & lblCodiceVolontario.Text & "_" & Calendar1.VisibleDate.Year & "_" & Calendar1.VisibleDate.Month & ".pdf")
            Response.BinaryWrite(myfile)
            Response.End()
        Else
            lblMessaggio.ForeColor = Color.Red
            lblMessaggio.ForeColor = Color.Navy
            lblMessaggio.Text = stresito

            MaintainScrollPositionOnPostBack = False
        End If

    End Sub

    Private Sub cmdConfermaPreliminare_Click(sender As Object, e As System.EventArgs) Handles cmdConfermaPreliminare.Click
        Dim strMg As String
        Dim esito As String
        Dim strsql As String

        ClsServer.ConfermaPreliminare(Request.QueryString("identita"), Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month, Session("Utente"), -1, Session("conn"), strMg, esito)

        If esito = "NEGATIVO" Then 'PROBLEMA, CONFERMA NON EFFETTUATA
            lblMessaggio.Text = strMg
            lblMessaggio.ForeColor = Color.Red
        Else
            If esito = "DA CONFERMARE" Then 'ESISTONO MALATTIE SPEZZATE QUINDI CHIEDO CONFERMA
                'DA METTERE UN DIV CHE APPARE E CHIEDE LA CONFERMA ALL'UTENTE. NEL DIV DEVE ESSERE PRESENTE IL MESSAGGIO RICEVUTO E DUE PULSANTI "CERTIFICATO UNICO" E "CERTIFICATI DIVERSI"
                divConferma.Visible = True
                lblMessaggio.Text = ""
                'lblMessaggio.ForeColor = Color.Red
                lblConferma.Text = strMg

            Else 'POSITIVO
                lblMessaggio.Text = strMg
                lblMessaggio.ForeColor = Color.Navy
            End If
        End If

        MaintainScrollPositionOnPostBack = False

    End Sub

    Private Function VERIFICA_CONGRUENZA_DATE_UPLOAD(ByVal intIdEntità As Integer, ByVal intAnno As Integer, ByVal intMese As Integer) As String()
        Dim Reader As SqlClient.SqlDataReader

        Try
            Dim x As String
            Dim y As String
            Dim ArreyOutPut(1) As String


            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_PRESENZE_VERIFICA_CONGRUENZA_DATE_UPLOAD"
            MyCommand.Connection = Session("conn")

            'PRIMO PARAMEtrO INPUT A
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdEntita"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            'PRIMO PARAMEtrO INPUT B
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Anno"
            sparam1.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam1)

            'SECONDO PARAMEtrO  INPUT C
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Mese"
            sparam2.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam2)


            'PARAMEtrO 1=esito OUTPUT
            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@Esito"
            sparam5.Size = 10
            sparam5.SqlDbType = SqlDbType.VarChar
            sparam5.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam5)


            'PARAMEtrO 2=Messaggio OUTPUT
            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@Messaggio"
            sparam6.Size = 255
            sparam6.SqlDbType = SqlDbType.NVarChar
            sparam6.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam6)


            MyCommand.Parameters("@IdEntita").Value = intIdEntità
            MyCommand.Parameters("@Anno").Value = intAnno
            MyCommand.Parameters("@Mese").Value = intMese



            Reader = MyCommand.ExecuteReader()

            x = CStr(MyCommand.Parameters("@Esito").Value)
            y = MyCommand.Parameters("@Messaggio").Value


            ArreyOutPut(0) = x
            ArreyOutPut(1) = y

            Reader.Close()
            Reader = Nothing

            Return ArreyOutPut

        Catch ex As Exception
            If Not Reader Is Nothing Then
                Reader.Close()
                Reader = Nothing
            End If
            Dim ArreyOutPut1(1) As String
            ArreyOutPut1(0) = "0"
            ArreyOutPut1(1) = "Contattare l'assistenza Helios/Futuro"
            Return ArreyOutPut1

        End Try
    End Function

    Protected Sub cmdCertificatoUnico_Click(sender As Object, e As EventArgs) Handles cmdCertificatoUnico.Click
        Dim strMg As String
        Dim esito As String
        Dim strsql As String

        ClsServer.ConfermaPreliminare(Request.QueryString("identita"), Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month, Session("Utente"), 1, Session("conn"), strMg, esito)

        If esito = "NEGATIVO" Then 'PROBLEMA, CONFERMA NON EFFETTUATA
            lblMessaggio.Text = strMg
            lblMessaggio.ForeColor = Color.Red
        Else
            If esito = "DA CONFERMARE" Then 'ESISTONO MALATTIE SPEZZATE QUINDI CHIEDO CONFERMA
                'DA METTERE UN DIV CHE APPARE E CHIEDE LA CONFERMA ALL'UTENTE. NEL DIV DEVE ESSERE PRESENTE IL MESSAGGIO RICEVUTO E DUE PULSANTI "CERTIFICATO UNICO" E "CERTIFICATI DIVERSI"
                divConferma.Visible = True
                lblMessaggio.Text = ""
                'lblMessaggio.ForeColor = Color.Red
                lblConferma.Text = strMg

            Else 'POSITIVO
                lblMessaggio.Text = strMg
                lblMessaggio.ForeColor = Color.Navy
            End If
        End If
        divConferma.Visible = False
        MaintainScrollPositionOnPostBack = False


        'REFRESH
        Dim anno As String
        Dim mese As String
        anno = Calendar1.SelectedDate.Year.ToString
        mese = Calendar1.SelectedDate.Month.ToString

        Dim ultimogiornomesevisualizzato As String
        Dim Data_fi As Date = "01/" & mese & "/" & anno

        ' Calcola l'ultimo giorno del mese corrente
        ultimogiornomesevisualizzato = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, +1, Data_fi))

        CaricaGrigliaComulativa(ultimogiornomesevisualizzato)
        CaricaGriglia(anno, mese)

        ControlloStatoConfermaMese(Session("IdEnte"), anno, mese)

    End Sub

    Private Sub cmdCertificatiDiversi_Click(sender As Object, e As System.EventArgs) Handles cmdCertificatiDiversi.Click
        Dim strMg As String
        Dim esito As String
        Dim strsql As String

        ClsServer.ConfermaPreliminare(Request.QueryString("identita"), Calendar1.VisibleDate.Year, Calendar1.VisibleDate.Month, Session("Utente"), 0, Session("conn"), strMg, esito)

        If esito = "NEGATIVO" Then 'PROBLEMA, CONFERMA NON EFFETTUATA
            lblMessaggio.Text = strMg
            lblMessaggio.ForeColor = Color.Red
        Else
            If esito = "DA CONFERMARE" Then 'ESISTONO MALATTIE SPEZZATE QUINDI CHIEDO CONFERMA
                'DA METTERE UN DIV CHE APPARE E CHIEDE LA CONFERMA ALL'UTENTE. NEL DIV DEVE ESSERE PRESENTE IL MESSAGGIO RICEVUTO E DUE PULSANTI "CERTIFICATO UNICO" E "CERTIFICATI DIVERSI"
                divConferma.Visible = True
                lblMessaggio.Text = ""
                'lblMessaggio.ForeColor = Color.Red
                lblConferma.Text = strMg

            Else 'POSITIVO
                lblMessaggio.Text = strMg
                lblMessaggio.ForeColor = Color.Navy
            End If
        End If
        divConferma.Visible = False
        MaintainScrollPositionOnPostBack = False
    End Sub
End Class