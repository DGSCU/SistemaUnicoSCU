Imports System.IO
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Security.Cryptography
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports
Imports System.Web.Mail
Imports System.Configuration
Imports System.Text
Imports System.Drawing
Imports Logger.Data
Public Class WfrmProgrammiIstanza
    Inherits SmartPage
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim MyDataSet As New DataSet
    Dim MyDataTable As DataTable
    Dim ROSSO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FF9966")
    Dim VERDE As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#99FF99")
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid(True)
        VerificaSessione()
        If Request.QueryString("idBP") <> "" Then
            IdBandoProgramma.Value = Request.QueryString("idBP")
        End If
        If IsPostBack = False Then


            '''''''''''''''''''''''''''''''ADC BANDO PROGRAMMA'
            If IdBandoProgramma.Value = "" Then 'Inserimento
                Dim strsqlstato As String
                Dim dtrStato As System.Data.SqlClient.SqlDataReader
                strsqlstato = "select idente from enti  where idente = " & Session("IdEnte") & " and idstatoente in (3,8,9) and albo = 'scu' and idclasseaccreditamentorichiesta in (8,9) "

                'controllo se utente o ente regionale
                'eseguo la query
                dtrStato = ClsServer.CreaDatareader(strsqlstato, Session("conn"))
                dtrStato.Read()
                If dtrStato.HasRows = False Then
                    lblErrore.Text = "L'ente non è abilitato alla presentazione di programmi"
                    ChiudiDataReader(dtrStato)
                    Exit Sub
                End If
                ChiudiDataReader(dtrStato)


            Else ' Modifica

                ImgControllaProvincie.Visible = True
                'imgCheckOLP.Visible = True

                Dim dtrstrsql As System.Data.SqlClient.SqlDataReader
                Dim strsql As String
                strsql = "select c.IdBandoAttività from BandiProgrammi a inner join bando b on a.IdBando = b.IDBando inner join BandiAttività c on a.IdEnte = c.IdEnte and b.IDBando = c.IDBando where a.IdBandoProgramma =" & IdBandoProgramma.Value
                dtrstrsql = ClsServer.CreaDatareader(strsql, Session("conn"))
                dtrstrsql.Read()
                If dtrstrsql.HasRows = True Then
                    IdBandoAttivita.Value = dtrstrsql("IdBandoAttività")
                End If
                ChiudiDataReader(dtrstrsql)

                If lblstato.Text = "Registrata" Then
                    ImgAnteprimaStampa.Visible = False
                End If
                

                Dim strATTIVITA As Integer = -1
                Dim strBANDOATTIVITA As Integer = -1
                Dim strENTEPERSONALE As Integer = -1
                Dim strENTITA As Integer = -1
                Dim strIDENTE As Integer = -1
                Dim strIDPROGRAMMA As Integer = -1

                If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE, strIDPROGRAMMA, IdBandoProgramma.Value) = 1 Then
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

            End If

            'If Request.QueryString("Stampa") = "SI" Then
            '    Dim JScript As String

            '    JScript = "<script>" & vbCrLf
            '    'Modifica il 14/03/2014 da s.c.
            '    'UNSC E le Regioni/Province Autonome utilizzano la procedura online x il caricamento dei progetti
            '    JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=72&IdBP=" & IdBandoProgramma.Value & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf

            '    JScript &= ("</script>")

            '    Response.Write(JScript)
            '    RegistraStampaAvvenuta()
            '    lblConferma.Text = "PRESENTAZIONE EFFETTUATA."
            'End If

            ' If Request.QueryString("Presenta") = "OK" Then
            If IdBandoProgramma.Value <> "" Then
                Dim dtrData As System.Data.SqlClient.SqlDataReader
                strsql = "select CONVERT(varchar(20), a.datapresentazione,103)+ ' ore ' + CONVERT(varchar(20), a.datapresentazione,114) as DataPresentazione from BandiProgrammi a inner join bando b on a.IdBando = b.IDBando inner join BandiAttività c on a.IdEnte = c.IdEnte and b.IDBando = c.IDBando where a.datapresentazione is not null and a.IdBandoProgramma =" & IdBandoProgramma.Value
                dtrData = ClsServer.CreaDatareader(strsql, Session("conn"))
                dtrData.Read()
                If dtrData.HasRows = True Then
                    strsql = dtrData("DataPresentazione")
                    lblConferma.Text = "PRESENTAZIONE EFFETTUATA SUL SISTEMA IL " & strsql
                    ChiudiDataReader(dtrData)
                    If linkpdf() = True Then
                        PDFistanzaPre.Visible = True
                    Else
                        PDFistanzaPre.Visible = False
                    End If
                End If
                ChiudiDataReader(dtrData)
            End If

            CaricaLoad()

        End If

        If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then CaricaStorico() 'se caricare ad ogni postback è troppo bisogna metterlo nel presenta/annulla e nel postback=false
    End Sub

    Private Sub CaricaStorico()
        If IdBandoProgramma.Value <> "" Then

            Dim sqlDAP As New SqlClient.SqlDataAdapter
            Dim dataSet As New DataSet
            Dim strNomeStore As String = "[SP_PROGRAMMI_PRESENTAZIONE_STORICO]"

            Try
                sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
                sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

                sqlDAP.SelectCommand.Parameters.Add("@IdBandoProgramma", SqlDbType.Int).Value = IdBandoProgramma.Value
                sqlDAP.Fill(dataSet)

                Session("dtsStoricoPresentazione") = dataSet
                dtgStoricoPresentazione.DataSource = dataSet
                dtgStoricoPresentazione.CurrentPageIndex = 0
                dtgStoricoPresentazione.DataBind()

                If dataSet.Tables(0).Rows.Count > 0 Then
                    divStoricoPresentazione.Visible = True
                    If PDFistanzaPre.Visible Then PDFistanzaPre.Visible = False 'il link sarebbe duplicato nella griglia
                Else
                    divStoricoPresentazione.Visible = False
                End If
            Catch ex As Exception
                Response.Write(ex.Message.ToString())
                Exit Sub
            End Try

        End If
    End Sub

    Private Sub CaricaLoad()
        If IdBandoProgramma.Value <> "" Then
            CaricaStatoIstanza()
        End If
        CaricaGriglia()
        CaricaDati()
        AbilitaDisabilita()
    End Sub
    Private Sub CaricaGriglia()
        If IdBandoProgramma.Value <> "" Then

            Dim sqlDAP As New SqlClient.SqlDataAdapter
            Dim dataSet As New DataSet
            Dim strNomeStore As String = "[SP_PROGRAMMI_ISTANZA_BANDO_SELEZIONATO]"

            Try
                sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
                sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

                sqlDAP.SelectCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
                sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.Int).Value = Session("IdEnte")
                sqlDAP.SelectCommand.Parameters.Add("@IdBandoProgramma", SqlDbType.Int).Value = IdBandoProgramma.Value
                sqlDAP.Fill(dataSet)

                Session("appDtsBandoProgrammaElenco") = dataSet
                DgdBandoProgramma.DataSource = dataSet
                DgdBandoProgramma.DataBind()
                IdBando.Value = DgdBandoProgramma.Items(0).Cells(1).Text
                BandoAperto.Value = DgdBandoProgramma.Items(0).Cells(5).Text
            Catch ex As Exception
                Response.Write(ex.Message.ToString())
                Exit Sub
            End Try
        Else
            Dim sqlDAP As New SqlClient.SqlDataAdapter
            Dim dataSet As New DataSet
            Dim strNomeStore As String = "[SP_PROGRAMMI_ISTANZA_BANDI_DISPONIBILI]"

            Try
                sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
                sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure


                sqlDAP.SelectCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
                sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.Int).Value = Session("IdEnte")


                sqlDAP.Fill(dataSet)

                Session("appDtsBandoProgrammaDisponibili") = dataSet
                DgdBandoProgramma.DataSource = dataSet
                DgdBandoProgramma.DataBind()

            Catch ex As Exception
                Response.Write(ex.Message.ToString())
                Exit Sub
            End Try

        End If
    End Sub
    Private Sub CaricaDati()
        Dim strsqlstato As String
        Dim dtrStato As System.Data.SqlClient.SqlDataReader
        strsqlstato = "SELECT  statienti.StatoEnte, enti.IDEnte FROM statienti INNER JOIN enti ON statienti.IDStatoEnte = enti.IDStatoEnte WHERE enti.IDEnte = " & Session("IdEnte")
        dtrStato = ClsServer.CreaDatareader(strsqlstato, Session("conn"))
        dtrStato.Read()
        If dtrStato.HasRows = True Then
            lblStatoEnte.Text = dtrStato("StatoEnte")
        End If
        ChiudiDataReader(dtrStato)

        If IdBandoProgramma.Value <> "" Then
            'Caricamento dati in modifica
            Dim sqlDAP As New SqlClient.SqlDataAdapter
            Dim dataSet As New DataSet
            Dim strNomeStore As String = "[SP_PROGRAMMI_ISTANZA_ELENCO_PROGRAMMI]"

            Try
                sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
                sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

                sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.Int).Value = Session("IdEnte")
                sqlDAP.SelectCommand.Parameters.Add("@IdBando", SqlDbType.Int).Value = IdBando.Value
                sqlDAP.SelectCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
                sqlDAP.SelectCommand.Parameters.Add("@EsitoControlliPresentazione", SqlDbType.Bit)
                sqlDAP.SelectCommand.Parameters("@EsitoControlliPresentazione").Direction = ParameterDirection.Output

                sqlDAP.Fill(dataSet)

                Session("appDtsProgrammi") = dataSet
                DgtProgrammi.DataSource = dataSet.Tables(0)
                DgtProgrammi.DataBind()
                Dgtattivita.DataSource = dataSet.Tables(1)
                Dgtattivita.DataBind()


                If DgtProgrammi.Items.Count = 0 Then
                    lblConferma.Text = ""
                    lblErrore.Text = ""
                    cmdInserisci.Visible = False
                    DgtProgrammi.Visible = False
                    lblprogramma.Visible = True
                    chkSelDesel.Visible = False
                    chkSelDesel2.Visible = False
                    export1.Visible = False
                    export2.Visible = False
                    export3.Visible = False
                    lblprogramma.Text = "Nessun programma disponibile."
                    'If Not DgdBandoProgramma.SelectedItem Is Nothing Then
                    '    lblprogramma.Visible = True
                    '    lblprogramma.Text = "Selezionare una circolare presentazione per visualizzare i programmi."
                    'End If
                Else
                    Dim item As DataGridItem
                    For Each item In DgtProgrammi.Items

                        Dim check As CheckBox = DirectCast(item.FindControl("chk"), CheckBox)
                        If DgtProgrammi.Items(item.ItemIndex).Cells(17).Text <> "0" Then
                            check.Checked = True
                        End If

                        Select Case DgtProgrammi.Items(item.ItemIndex).Cells(5).Text
                            Case False
                                DgtProgrammi.Items(item.ItemIndex).BackColor = ROSSO
                            Case True
                                DgtProgrammi.Items(item.ItemIndex).BackColor = VERDE
                        End Select
                    Next

                    For Each item In Dgtattivita.Items

                        Dim check As CheckBox = DirectCast(item.FindControl("chk"), CheckBox)
                        If Dgtattivita.Items(item.ItemIndex).Cells(6).Text <> "0" Then
                            check.Checked = True
                        End If

                        Select Case Dgtattivita.Items(item.ItemIndex).Cells(13).Text
                            Case False
                                Dgtattivita.Items(item.ItemIndex).BackColor = ROSSO
                            Case True
                                Dgtattivita.Items(item.ItemIndex).BackColor = VERDE
                        End Select
                    Next

                    chkSelDesel.Visible = True
                    chkSelDesel2.Visible = True
                    cmdInserisci.Visible = True
                End If

            Catch ex As Exception
                Response.Write(ex.Message.ToString())
                Exit Sub
            End Try

            If BandoAperto.Value = "0" Then 'CHIUSO
                lblErrore.Visible = True
                lblErrore.Text = "I TERMINI DI PRESENTAZIONE RISULTANO SCADUTI. IMPOSSIBILE EFFETTUARE MODIFICHE"
                CaricoTotali()
            Else 'APERTO
                '    'AbilitaDisabilita()
                '    CaricaStatoIstanza()

                CaricoTotali()

            End If
        Else
            lblstato.Text = "NESSUNA"
            TotProgrammi.Text = "0"
            TotProgetti.Text = "0"
            TotVolontari.Text = "0"
        End If
    End Sub
    Private Sub CaricaStatoIstanza()
        Dim strsql As String
        Dim dtrStatoIstanza As System.Data.SqlClient.SqlDataReader
        strsql = "select b.StatoBandoProgramma,b.defaultstato,b.davalutare,b.attivo,b.chiuso,b.inammissibile,b.cancellata,bando.annobreve  from BandiProgrammi a inner join statiBandiProgrammi b on a.IdStatoBandoProgramma=b.IdStatoBandoProgramma inner join bando on bando.idbando=a.idbando where a.IdBandoProgramma =" & IdBandoProgramma.Value
        dtrStatoIstanza = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrStatoIstanza.Read()
        If dtrStatoIstanza.HasRows = True Then
            lblstato.Text = dtrStatoIstanza("StatoBandoProgramma") 'STATO DELLA ISTANZA
        End If
        ChiudiDataReader(dtrStatoIstanza)

        strsql = "select b.idbandoattività from bandiprogrammi a inner join bandiattività b on a.idente = b.idente and a.idbando = b.idbando where a.IdBandoProgramma =" & IdBandoProgramma.Value
        dtrStatoIstanza = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrStatoIstanza.Read()
        If dtrStatoIstanza.HasRows = True Then
            IdBandoAttivita.Value = dtrStatoIstanza("idbandoattività") 'STATO DELLA ISTANZA
        End If
        ChiudiDataReader(dtrStatoIstanza)




    End Sub
    Private Sub CaricoTotali()
        Dim strsql As String
        Dim dtrtotali As System.Data.SqlClient.SqlDataReader
        strsql = "select count(distinct a.idprogramma) as NProgrammi,count(distinct b.idattività) as NProgetti,isnull(sum(b.numeroPostiNoVittoNoAlloggio+b.NumeroPostiVitto+b.NumeroPostiVittoAlloggio),0) as NVolontari from Programmi a left join attività b on a.IdProgramma = b.IdProgramma where a.IdBandoProgramma =" & IdBandoProgramma.Value
        dtrtotali = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrtotali.Read()
        If dtrtotali.HasRows = True Then
            TotProgrammi.Text = dtrtotali("NProgrammi")
            TotProgetti.Text = dtrtotali("NProgetti")
            TotVolontari.Text = dtrtotali("NVolontari")
        End If
        ChiudiDataReader(dtrtotali)
    End Sub
    Private Sub DgdBandoProgramma_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DgdBandoProgramma.ItemCommand

        If e.CommandName = "Select" Then
            Dim sqlDAP As New SqlClient.SqlDataAdapter
            Dim dataSet As New DataSet
            Dim strNomeStore As String = "[SP_PROGRAMMI_ISTANZA_ELENCO_PROGRAMMI]"
            IdBando.Value = CInt(e.Item.Cells(1).Text)
            Try
                sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
                sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

                sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.Int).Value = Session("IdEnte")
                sqlDAP.SelectCommand.Parameters.Add("@IdBando", SqlDbType.Int).Value = CInt(e.Item.Cells(1).Text)
                sqlDAP.SelectCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
                sqlDAP.SelectCommand.Parameters.Add("@EsitoControlliPresentazione", SqlDbType.Bit)
                sqlDAP.SelectCommand.Parameters("@EsitoControlliPresentazione").Direction = ParameterDirection.Output
                sqlDAP.Fill(dataSet)

                Session("appDtsProgrammi") = dataSet
                DgtProgrammi.DataSource = dataSet.Tables(0)
                DgtProgrammi.DataBind()
                Dgtattivita.DataSource = dataSet.Tables(1)
                Dgtattivita.DataBind()
                BandoAperto.Value = DgdBandoProgramma.Items(0).Cells(5).Text

                If DgtProgrammi.Items.Count = 0 Then
                    lblConferma.Text = ""
                    lblErrore.Text = ""
                    cmdInserisci.Visible = False
                    DgtProgrammi.Visible = False
                    lblprogramma.Visible = True
                    chkSelDesel.Visible = False
                    chkSelDesel2.Visible = False
                    export1.Visible = False
                    export2.Visible = False
                    export3.Visible = False
                    lblprogramma.Text = "Nessun programma disponibile."
                    'If Not DgdBandoProgramma.SelectedItem Is Nothing Then
                    '    lblprogramma.Visible = True
                    '    lblprogramma.Text = "Selezionare una circolare presentazione per visualizzare i programmi."
                    'End If
                Else
                    Dim item As DataGridItem
                    For Each item In DgtProgrammi.Items
                        Select Case DgtProgrammi.Items(item.ItemIndex).Cells(5).Text
                            Case False
                                DgtProgrammi.Items(item.ItemIndex).BackColor = ROSSO
                            Case True
                                DgtProgrammi.Items(item.ItemIndex).BackColor = VERDE
                        End Select
                    Next

                    For Each item In Dgtattivita.Items
                        Select Case Dgtattivita.Items(item.ItemIndex).Cells(13).Text
                            Case False
                                Dgtattivita.Items(item.ItemIndex).BackColor = ROSSO
                            Case True
                                Dgtattivita.Items(item.ItemIndex).BackColor = VERDE
                        End Select
                    Next

                    chkSelDesel.Visible = True
                    chkSelDesel2.Visible = True
                    cmdInserisci.Visible = True
                End If

            Catch ex As Exception
                Response.Write(ex.Message.ToString())
                Exit Sub
            End Try
        End If
    End Sub
    Private Sub AbilitaDisabilita()
        If IdBandoProgramma.Value <> "" Then
            If BandoAperto.Value = 1 Then 'Aperto
                Select Case lblstato.Text
                    Case "Registrata"
                        DgdBandoProgramma.Columns(0).Visible = False
                        cmdmodifica.Visible = True
                        cmdInserisci.Visible = False
                        cmdannulla.Visible = True
                        cmdPresentaIstanza.Visible = True
                        cmdChiudi.Visible = True
                        imgStampaAll.Visible = False
                        ImgAnteprimaStampa.Visible = False 'tolta visibilità 15/12/2021
                        lblMessaggioPresenta.Visible = True
                        cmdAnnullaPresentazione.Visible = False
                        DgdBandoProgramma.Columns(0).Visible = False 'selezione bando
                        Dgtattivita.Columns(11).Visible = True 'applica documenti
                    Case Else '"Presentata"
                        cmdAnnullaPresentazione.Visible = True
                        cmdChiudi.Visible = True
                        imgStampaAll.Visible = False 'tolta visibilità 15/12/2021
                        ImgAnteprimaStampa.Visible = False
                        lblMessaggioPresenta.Visible = False
                        cmdInserisci.Visible = False
                        DgdBandoProgramma.Columns(0).Visible = False 'selezione bando
                        Dgtattivita.Columns(11).Visible = False 'applica documenti
                        bloccaCheck()
                End Select

                export1.Visible = True
                export2.Visible = False
                export3.Visible = False

            Else 'chiuso


                DgdBandoProgramma.Columns(0).Visible = False
                cmdPresentaIstanza.Visible = False
                lblMessaggioPresenta.Visible = False
                cmdmodifica.Visible = False
                cmdannulla.Visible = False
                cmdRipristina.Visible = False
                cmdInserisci.Visible = False
                ImgAnteprimaStampa.Visible = False
                bloccaCheck()
                DgdBandoProgramma.Columns(0).Visible = False 'selezione bando
                Dgtattivita.Columns(11).Visible = False 'applica documenti
                'se valutatore abilito tasti
                cmdaccredita.Visible = False
                cmddissaccredita.Visible = False

                If lblstato.Text = "Presentata" Then
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        cmdaccredita.Visible = True
                        cmddissaccredita.Visible = True
                    End If
                End If

            End If
        Else
            cmdInserisci.Visible = True
            cmdChiudi.Visible = True
            export1.Visible = False
            export2.Visible = False
            export3.Visible = False

        End If


    End Sub

    Private Sub chkSelDesel_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelDesel.CheckedChanged
        MaintainScrollPositionOnPostBack = True
        If chkSelDesel.Checked = True Then
            chkSelDesel2.Checked = True
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = DgtProgrammi.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = DgtProgrammi.Items(x).Cells(3).FindControl("chk")
                If chkoggetto.Visible = True Then
                    chkoggetto.Checked = True
                Else
                    chkoggetto.Checked = False
                End If
            Next
        Else
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = DgtProgrammi.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = DgtProgrammi.Items(x).Cells(3).FindControl("chk")
                chkoggetto.Checked = False
            Next
            chkSelDesel2.Checked = False
        End If

    End Sub

    Private Sub chkSelDesel2_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelDesel2.CheckedChanged
        MaintainScrollPositionOnPostBack = True
        If chkSelDesel2.Checked = True Then
            chkSelDesel.Checked = True
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = DgtProgrammi.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = DgtProgrammi.Items(x).Cells(3).FindControl("chk")
                If chkoggetto.Visible = True Then
                    chkoggetto.Checked = True
                Else
                    chkoggetto.Checked = False
                End If

            Next

        Else
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = DgtProgrammi.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = DgtProgrammi.Items(x).Cells(3).FindControl("chk")
                chkoggetto.Checked = False
            Next
            chkSelDesel.Checked = False
        End If

    End Sub

    Private Sub bloccaCheck()

        Dim item As DataGridItem
        For Each item In DgtProgrammi.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chk"), CheckBox)
            check.Enabled = False
        Next
        chkSelDesel.Visible = False
        chkSelDesel2.Visible = False

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Dim Ente As String = Request.QueryString("Ente")
        Dim Bando As Integer = CInt(Request.QueryString("Bando"))
        Dim Anno As Integer = CInt(Request.QueryString("Anno"))
        Dim StatoAttivita As Integer = CInt(Request.QueryString("StatoAttivita"))
        Dim Competenza As Integer = CInt(Request.QueryString("Competenza"))


        'txtEnte.Value = Request.QueryString("Ente")
        'cboBando.value = CInt(Request.QueryString("Bando"))
        'cboAnno.Value = CInt(Request.QueryString("Anno"))
        'cboStatoAttivita.Value = CInt(Request.QueryString("StatoAttivita"))
        'cboCompetenza.Value = CInt(Request.QueryString("Competenza"))
        If Request.QueryString("Vengoda") = "Accettazione" Then
            Response.Redirect("WfrmRicAccettazioneIstanzaUNSCProgrammi.aspx?Ente=" & Ente & "&Bando=" & Bando & "&Anno=" & Anno & "&StatoAttivita=" & StatoAttivita & "&Competenza=" & Competenza & "&Vengoda=Accettazione" & "")
        Else
            Response.Redirect("WfrmMain.aspx")
        End If


    End Sub
    
    Protected Sub cmdInserisci_Click(sender As Object, e As EventArgs) Handles cmdInserisci.Click
        lblErrore.Text = ""
        lblConferma.Text = ""

       
        'inserimento istanza
        Dim Almeno1 As Boolean = False
        Dim strElencoProgrammi As String = ""
        Dim Indice As DataGridItem
        For Each Indice In DgtProgrammi.Items
            Dim check As CheckBox = DirectCast(Indice.FindControl("chk"), CheckBox)
            If check.Checked = True Then
                strElencoProgrammi = strElencoProgrammi & DgtProgrammi.Items(Indice.ItemIndex).Cells(0).Text & ","
                Almeno1 = True
            End If
        Next
        'verificare se e' ceccato almeno un programma
        If Almeno1 = False Then 'non e' chek nessuno
            lblErrore.Text = "Attenzione, selezionare almeno un programma prima di inserire l'istanza"
            Exit Sub
        End If


        Dim ArreyDiMessaggi() As String
        Dim ESITO As Integer = -1
        lblerrore.Text = ""
        lblConferma.Text = ""

        Dim SqlCmd As New SqlClient.SqlCommand

        Try
            SqlCmd.CommandText = "SP_PROGRAMMI_ISTANZA_INSERIMENTO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEnteProponente ", SqlDbType.Int).Value = CInt(Session("idente"))
            SqlCmd.Parameters.Add("@IdBando", SqlDbType.Int).Value = IdBando.Value
            SqlCmd.Parameters.Add("@ElencoProgrammi", SqlDbType.VarChar).Value = strElencoProgrammi
            SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Size = 10
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@IdBandoProgrammaInserito", SqlDbType.Int)
            SqlCmd.Parameters("@IdBandoProgrammaInserito").Direction = ParameterDirection.Output
            SqlCmd.ExecuteNonQuery()

            ESITO = SqlCmd.Parameters("@Esito").Value()
            If ESITO = 0 Then
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(SqlCmd.Parameters("@messaggio").Value(), ":")
                lblErrore.Text = ArreyDiMessaggi(0)
                Dim MessErrore() As String
                Dim NuovaStringa As String
                For i = 0 To UBound(ArreyDiMessaggi)
                    If i = 0 Then
                        ArreyDiMessaggi.Clear(ArreyDiMessaggi, 0, 1)
                        'funziona
                        NuovaStringa = ArreyDiMessaggi(1)
                        ReDim MessErrore(0)
                        MessErrore(0) = NuovaStringa
                        ReDim Preserve MessErrore(0)
                    End If
                Next
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(MessErrore(0), ".")
                lblErrore.Text = lblErrore.Text & ":" & vbCrLf & "<br/>"
                Dim rigadasplittare() As String

                rigadasplittare = ClsUtility.CreaArrayMessaggi(MessErrore(0), "|")
                For i = 0 To UBound(rigadasplittare) - 1
                    lblErrore.Text = lblErrore.Text & rigadasplittare(i) & vbCrLf & "<br/>"
                Next
            Else
                MaintainScrollPositionOnPostBack = True
                lblConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                lblConferma.Text = lblConferma.Text & "<br/>"
            End If
        Catch ex As Exception
            lblErrore.Text = ex.Message
        Finally

        End Try
        MaintainScrollPositionOnPostBack = False
        If SqlCmd.Parameters("@IdBandoProgrammaInserito").Value() <> -1 Then
            IdBandoProgramma.Value = SqlCmd.Parameters("@IdBandoProgrammaInserito").Value()
            'CaricaProgramma(IdProgramma.Value, 0)
            'cmdElimina.Visible = True
        End If

        CaricaLoad()
    End Sub

    Private Function ModificaIstanza() As Boolean
        'SP_PROGRAMMI_ISTANZA_MODIFICA
        lblErrore.Text = ""
        lblConferma.Text = ""


        'inserimento istanza
        Dim Almeno1 As Boolean = False
        Dim strElencoProgrammi As String = ""
        Dim Indice As DataGridItem
        For Each Indice In DgtProgrammi.Items
            Dim check As CheckBox = DirectCast(Indice.FindControl("chk"), CheckBox)
            If check.Checked = True Then
                strElencoProgrammi = strElencoProgrammi & DgtProgrammi.Items(Indice.ItemIndex).Cells(0).Text & ","
                Almeno1 = True
            End If
        Next
        'verificare se e' ceccato almeno un programma
        If Almeno1 = False Then 'non e' chek nessuno
            lblErrore.Text = "Attenzione, deve essere selezionato almeno un programma per l'istanza"
            ModificaIstanza = False
            Exit Function
        End If
        Dim ArreyDiMessaggi() As String
        Dim ESITO As Integer = -1
        lblErrore.Text = ""
        lblConferma.Text = ""

        Dim SqlCmd As New SqlClient.SqlCommand

        Try
            SqlCmd.CommandText = "SP_PROGRAMMI_ISTANZA_MODIFICA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            'SqlCmd.Parameters.Add("@IdEnteProponente ", SqlDbType.Int).Value = CInt(Session("idente"))
            SqlCmd.Parameters.Add("@IdBandoProgramma", SqlDbType.Int).Value = IdBandoProgramma.Value
            SqlCmd.Parameters.Add("@ElencoProgrammi", SqlDbType.VarChar).Value = strElencoProgrammi
            SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Size = 10
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            ESITO = SqlCmd.Parameters("@Esito").Value()
            If ESITO = 0 Then
                ModificaIstanza = False
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(SqlCmd.Parameters("@messaggio").Value(), ":")
                lblErrore.Text = ArreyDiMessaggi(0)
                Dim MessErrore() As String
                Dim NuovaStringa As String
                For i = 0 To UBound(ArreyDiMessaggi)
                    If i = 0 Then
                        ArreyDiMessaggi.Clear(ArreyDiMessaggi, 0, 1)
                        'funziona
                        NuovaStringa = ArreyDiMessaggi(1)
                        ReDim MessErrore(0)
                        MessErrore(0) = NuovaStringa
                        ReDim Preserve MessErrore(0)
                    End If
                Next
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(MessErrore(0), ".")
                lblErrore.Text = lblErrore.Text & ":" & vbCrLf & "<br/>"
                Dim rigadasplittare() As String

                rigadasplittare = ClsUtility.CreaArrayMessaggi(MessErrore(0), "|")
                For i = 0 To UBound(rigadasplittare) - 1
                    lblErrore.Text = lblErrore.Text & rigadasplittare(i) & vbCrLf & "<br/>"
                Next
            Else
                ModificaIstanza = True
                CaricaDati()
                MaintainScrollPositionOnPostBack = True
                lblConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                lblConferma.Text = lblConferma.Text & "<br/>"
            End If
        Catch ex As Exception
            ModificaIstanza = False
            lblErrore.Text = ex.Message
        Finally

        End Try

        CaricaLoad()

    End Function

    Private Function PresentaIstanza() As Boolean
        Dim ArreyDiMessaggi() As String
        Dim ESITO As Integer = -1
        lblErrore.Text = ""
        lblConferma.Text = ""

        Dim SqlCmd As New SqlClient.SqlCommand

        Try
            SqlCmd.CommandText = "SP_PROGRAMMI_ISTANZA_PRESENTA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            'SqlCmd.Parameters.Add("@IdEnteProponente ", SqlDbType.Int).Value = CInt(Session("idente"))
            SqlCmd.Parameters.Add("@IdBandoProgramma", SqlDbType.Int).Value = IdBandoProgramma.Value
            SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
            SqlCmd.Parameters.Add("@SoloVerifica", SqlDbType.Bit).Value = 1
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Size = 10
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            ESITO = SqlCmd.Parameters("@Esito").Value()
            If ESITO = 0 Then
                PresentaIstanza = False
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(SqlCmd.Parameters("@messaggio").Value(), ":")
                lblErrore.Text = ArreyDiMessaggi(0)
                Dim MessErrore() As String
                Dim NuovaStringa As String
                For i = 0 To UBound(ArreyDiMessaggi)
                    If i = 0 Then
                        ArreyDiMessaggi.Clear(ArreyDiMessaggi, 0, 1)
                        'funziona
                        NuovaStringa = ArreyDiMessaggi(1)
                        ReDim MessErrore(0)
                        MessErrore(0) = NuovaStringa
                        ReDim Preserve MessErrore(0)
                    End If
                Next
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(MessErrore(0), ".")
                lblErrore.Text = lblErrore.Text & ":" & vbCrLf & "<br/>"
                Dim rigadasplittare() As String

                rigadasplittare = ClsUtility.CreaArrayMessaggi(MessErrore(0), "|")
                For i = 0 To UBound(rigadasplittare) - 1
                    lblErrore.Text = lblErrore.Text & rigadasplittare(i) & vbCrLf & "<br/>"
                Next
            Else
                PresentaIstanza = True
                CaricaDati()
                MaintainScrollPositionOnPostBack = True
                lblConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                lblConferma.Text = lblConferma.Text & "<br/>"
            End If
        Catch ex As Exception
            PresentaIstanza = False
            lblErrore.Text = ex.Message
        Finally

        End Try

        CaricaLoad()

    End Function


    Protected Sub cmdmodifica_Click(sender As Object, e As EventArgs) Handles cmdmodifica.Click
        Dim esito As Boolean
        esito = ModificaIstanza()

    End Sub

    Protected Sub cmdannulla_Click(sender As Object, e As EventArgs) Handles cmdannulla.Click
        'SP_PROGRAMMI_ISTANZA_ELIMINA
        Dim ArreyDiMessaggi() As String
        Dim ESITO As Integer = -1
        lblErrore.Text = ""
        lblConferma.Text = ""
        Dim SqlCmd As New SqlClient.SqlCommand

        Try
            SqlCmd.CommandText = "SP_PROGRAMMI_ISTANZA_ELIMINA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")


            SqlCmd.Parameters.Add("@IdBandoProgramma", SqlDbType.Int).Value = IdBandoProgramma.Value
            SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Size = 10
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            ESITO = SqlCmd.Parameters("@Esito").Value()
            If ESITO = 0 Then
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(SqlCmd.Parameters("@messaggio").Value(), ":")
                lblErrore.Text = ArreyDiMessaggi(0)
                Dim MessErrore() As String
                Dim NuovaStringa As String
                For i = 0 To UBound(ArreyDiMessaggi)
                    If i = 0 Then
                        ArreyDiMessaggi.Clear(ArreyDiMessaggi, 0, 1)
                        'funziona
                        NuovaStringa = ArreyDiMessaggi(1)
                        ReDim MessErrore(0)
                        MessErrore(0) = NuovaStringa
                        ReDim Preserve MessErrore(0)
                    End If
                Next
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(MessErrore(0), ".")
                lblErrore.Text = lblErrore.Text & ":" & vbCrLf & "<br/>"
                Dim rigadasplittare() As String

                rigadasplittare = ClsUtility.CreaArrayMessaggi(MessErrore(0), "|")
                For i = 0 To UBound(rigadasplittare) - 1
                    lblErrore.Text = lblErrore.Text & rigadasplittare(i) & vbCrLf & "<br/>"
                Next
            Else
                MaintainScrollPositionOnPostBack = True
                lblConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                lblConferma.Text = lblConferma.Text & "<br/>"
            End If
        Catch ex As Exception
            lblErrore.Text = ex.Message
        Finally

        End Try
        BandoAperto.Value = 0
        IdBandoProgramma.Value = 0
        IdBando.Value = 0
        AbilitaDisabilita()
        'CaricaLoad()
    End Sub
    Private Function StatoDocumenti(ByVal idEnte As Integer) As Boolean
        'agg. da simoan cordella il 12/12/2012
        Dim strSql As String
        Dim dtrCount As SqlClient.SqlDataReader
        Dim blnReturn As Boolean
        strSql = " SELECT  NElaborazioniMancanti FROM LockDocumentiEnte l " & _
                 " WHERE IdEnte = " & idEnte
        dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))

        blnReturn = dtrCount.HasRows
        If Not dtrCount Is Nothing Then
            dtrCount.Close()
            dtrCount = Nothing
        End If

        Return blnReturn
    End Function
    Function ControllaStatoChiuso() As Boolean
        strsql = "select bando.idstatobando " & _
        " from bando " & _
        " inner join statibando  on bando.idstatobando=statibando.idstatobando" & _
        " where idbando=" & IdBando.Value & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            If dtrgenerico("idstatobando") = 3 Then
                ControllaStatoChiuso = True
            Else
                ControllaStatoChiuso = False
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Function
    Protected Sub cmdPresentaIstanza_Click(sender As Object, e As EventArgs) Handles cmdPresentaIstanza.Click

        Dim dtrstrsql As System.Data.SqlClient.SqlDataReader
        Dim strsql As String
        strsql = "select c.IdBandoAttività from BandiProgrammi a inner join bando b on a.IdBando = b.IDBando inner join BandiAttività c on a.IdEnte = c.IdEnte and b.IDBando = c.IDBando where a.IdBandoProgramma =" & IdBandoProgramma.Value
        dtrstrsql = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrstrsql.Read()
        If dtrstrsql.HasRows = True Then
            IdBandoAttivita.Value = dtrstrsql("IdBandoAttività")
        End If
        ChiudiDataReader(dtrstrsql)

        Dim esito As Boolean
        esito = ModificaIstanza()
        If esito = True Then
            esito = PresentaIstanza()
            If esito = True Then
                'GenerazioneBOX16_BOX19_Da_WSDocumentazione(IdBandoAttivita.Value)
                'Response.Redirect("WfrmInfoPresentazioneIstanza.aspx?IdBandoAttivita=" & IdBandoAttivita.Value & "&IdBP=" & IdBandoProgramma.Value)

                Response.Redirect("WfrmProgrammiIstanzaPresenta.aspx?IdBandoAttivita=" & IdBandoAttivita.Value & "&IdBP=" & IdBandoProgramma.Value)
            End If
        Else
            cmdPresentaIstanza.Visible = True
            msgPresentaIstanza.Visible = False
            cmdmodifica.Visible = True
            cmdannulla.Visible = True
            ImgAnteprimaStampa.Visible = False
        End If
    End Sub
    Private Sub GenerazioneBOX16_BOX19_Da_WSDocumentazione(ByVal IdBandoAttivita As Integer)

        Dim localWS As New WS_Editor.WSMetodiDocumentazione
        Dim ds As DataSet
        Dim i As Integer
        Dim strCodiceProgetto As String
        Dim ResultAsinc As IAsyncResult

        Dim cmdUp As SqlCommand
        cmdUp = New Data.SqlClient.SqlCommand(" UPDATE bandiattività" & _
                                              " SET InLavorazione= 1  " & _
                                              " WHERE idbandoattività=" & IdBandoAttivita & "", Session("conn"))
        cmdUp.ExecuteNonQuery()
        cmdUp.Dispose()

        'richiamo WSDocumentazione
        localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")
        localWS.Timeout = 1000000
        ResultAsinc = localWS.BeginGenerazioneBOX16_BOX19(IdBandoAttivita, Session("Utente"), Nothing, "")
    End Sub

    Private Sub PRESENTA_BUTTA()
        ''***Generata da Gianluigi Paesani in data:19/07/04
        ''***modulo che rendera i progetti associati al bando presentati e quindi non più
        ''***modificabili
        'Dim booverifica As Boolean = False 'setto a zero parametri utenza
        'Dim intAnnoBreve As Integer

        'cmdPresentaIstanza.Visible = False

        ''Aggiunto da Danilo Spagnulo il 12/12/2012
        'If StatoDocumenti(Session("idente")) = True Then
        '    ImgAnteprimaStampa.Visible = False

        '    cmdPresentaIstanza.Visible = False
        '    lblMessaggioPresenta.Visible = False
        '    cmdannulla.Visible = False
        '    cmdmodifica.Visible = False
        '    lblErrore.Text = "Attenzione, è in corso l'applicazione di documenti a progetti. Si prega di attendere il completamento dell'operazione e riprovare."

        '    bloccaCheck()
        '    'Response.Write("<script language=""javascript"">" & vbCrLf)
        '    'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
        '    'Response.Write("</script>" & vbCrLf)
        '    cmdPresentaIstanza.Visible = False
        '    Exit Sub
        'End If
        ''Aggiunto da Alessandra Taballione il 21/09/2005
        'If ControllaStatoChiuso() = True Then
        '    cmdPresentaIstanza.Visible = False
        '    lblMessaggioPresenta.Visible = False
        '    cmdannulla.Visible = False
        '    cmdmodifica.Visible = False
        '    lblErrore.Text = "Attenzione, non è possibile presentare l'istanza poichè i termini sono scaduti."

        '    bloccaCheck()
        '    cmdPresentaIstanza.Visible = False
        '    Exit Sub
        'End If

        ''EFFETTUO SALVATAGGIO DATI PRIMA DELLA PRESENTAZIONE
        'If ModificaIstanza() = True Then
        '    cmdPresentaIstanza.Visible = True
        '    Exit Sub 'se non ci sono flag interrompo comando
        'End If


        ''VERIFICARE CONTROLO OLP SU SEDI DIVERSE!!!!!!!

        ''If ControllaOlpSedi() = 1 Then
        ''    Response.Write("<script>" & vbCrLf)
        ''    Response.Write("window.open(""risorsesedidiverse.aspx?idbando=" & IdBando.Value & "&Messaggio=Impossibile Presentare l'Istanza. E' necessario verificare le seguenti anomalie sulle figure professionali."", """", ""width=700,height=620,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
        ''    Response.Write("</script>")
        ''    cmdPresentaIstanza.Visible = True
        ''    Exit Sub
        ''End If


        ''AGGIORNO BANDIPROGRAMMI: STATO E DATA PRESENTAZIONE
        ''AGGIORNO BANDIATTIVITà: STATO E DATA PRESENTAZIONE
        'Dim cmdinsert As Data.SqlClient.SqlCommand
        'Dim dtr As Data.SqlClient.SqlDataReader
        ''modifico stato istanza per non essere più modificabile
        'cmdinsert = New Data.SqlClient.SqlCommand("update bandiattività" & _
        '" set idstatobandoattività=(select idstatobandoattività" & _
        '" from statibandiattività where Davalutare=1),usernamepresentazione='" & Session("Utente") & "',datapresentazione=getdate()" & _
        '" where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        'cmdinsert.ExecuteNonQuery()
        'cmdinsert.Dispose()

        ''AGGIORNO ATTIVITA': CONTATORI POSTI RICHIESTI (NORMALI+FAMI+GMO) + DATAPRESENTAZIONE
        ''UpDate dei Posti Richiesti + FAMI
        'cmdinsert = New Data.SqlClient.SqlCommand("Update Attività Set NumeroPostiNoVittoNoAlloggioRic = NumeroPostiNoVittoNoAlloggio, NumeroPostiVittoAlloggioRic = NumeroPostiVittoAlloggio, NumeroPostiVittoRic = NumeroPostiVitto, NumeroPostiFamiRichiesti=NumeroPostiFami  Where IdBandoAttività = " & Request.QueryString("idBA") & "", Session("conn"))
        'cmdinsert.ExecuteNonQuery()
        'cmdinsert.Dispose()
        ''Modifico la data di presentazione dei progetti sulle singole attività
        'cmdinsert = New Data.SqlClient.SqlCommand("Update attività set DataPresentazioneProgetto=getdate() where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        'cmdinsert.ExecuteNonQuery()
        'cmdinsert.Dispose()

        ''AGGIORNO PROGRAMMI: CONTATORI POSTI RICHIESTI (NORMALI+FAMI+GMO) + STATO E DATAPRESENTAZIONE

        ''PERSONALIZZO MASCHERA (STATO E TASTI)
        ''eseguo query per visualizzazione stato attuale
        'dtr = ClsServer.CreaDatareader("select b.statobandoattività,b.defaultstato,b.davalutare,b.attivo,b.chiuso,b.inammissibile,b.cancellata,bando.annobreve  from bandiattività a" & _
        '" inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività" & _
        '" inner join bando on bando.idbando=a.idbando  " & _
        '" where a.idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        'If dtr.HasRows = True Then
        '    Do While dtr.Read()
        '        lblstato.Text = dtr.GetValue(0) 'visualizzo stato
        '        intAnnoBreve = dtr.GetValue(7)
        '        PerrsonalizzaTasti(dtr.GetValue(1), dtr.GetValue(2), dtr.GetValue(3), dtr.GetValue(4), dtr.GetValue(5), dtr.GetValue(6))
        '    Loop
        'End If
        'dtr.Close()
        'dtr = Nothing

        ''GENERO CODICI PROGRAMMA

        ''GENERO CODICI PROGETTO
        ''Eseguo StoreProcedure per codice progetto .
        'ClsServer.EseguiStoreGeneraCodiciProgetto(txtidbandoAttivita.Text, "SP_GENERA_CODICI_PROGETTO", Session("conn"))


        ''GENERO FASCICOLI
        ''modifica per generazione automatica fascicolo
        'Dim WSInterno As New WSHeliosInterno.HeliosInterno
        'Dim wsOut As String

        'Try
        '    WSInterno.Url = ConfigurationSettings.AppSettings("URL_WSHeliosInterno")
        '    wsOut = WSInterno.CreaFascicoloIstanza(Request.QueryString("id"), Session("idente"))
        'Catch ex As Exception

        'End Try


        ''GENERAZIONE BOX ASINCRONA
        'GenerazioneBOX16_BOX19_Da_WSDocumentazione(txtidbandoAttivita.Text)
        ''reindirazzamento ad una pagine informativa finche il flag InLavorazione di Bando Attività nn ritorta a 0
        ''SANDOKAN

        ''Server.Transfer("WfrmInfoPresentazioneIstanza.aspx?IdBandoAttivita=" & txtidbandoAttivita.Text & "")

        'Response.Redirect("WfrmInfoPresentazioneIstanza.aspx?IdBandoAttivita=" & txtidbandoAttivita.Text & "")
        'Exit Sub


        ''setto wform
        'lblMessaggio.Text = "L'Istanza è stata presentata con successo"
        'cmdInserisci.Visible = False
        'cmdannulla.Visible = False
        'cmdmodifica.Visible = False
        'cmdPresentaIstanza.Visible = False
        'lblMessaggioPresenta.Visible = False

        ''disabilito colonna info
        ''Dgtattivita.Columns(16).Visible = False
        'cmdAnnullaPresentazione.Visible = True
        'Dgtattivita.Columns(18).Visible = False ' pulsante applica
        'Dgtattivita.Columns(19).Visible = False ' colonna popup info
        'ImgAnteprimaStampa.Visible = False

        ''Aggiunto da Alessandra il 15/07/2005
        'If Session("tipoutente") = "E" Then
        '    cmddissaccredita.Visible = False
        '    cmdaccredita.Visible = False
        'End If

        'If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
        '    Call VerificaCompetenze()
        'End If

        'Dim JScript As String

        'JScript = "<script>" & vbCrLf
        'JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=16&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf

        'imgStampaAll.Visible = True


        'RegistraStampaAvvenuta()

        'CaricaLoad()
    End Sub

    Protected Sub CmdEsportaDoc_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles CmdEsportaDoc.Click
        export2.Visible = True
        export3.Visible = True
        EsportazioneProgetti()
        EsportazioneProgrammi()
    End Sub
    Sub EsportazioneProgetti()

        Dim i As Integer
        Dim arrParam(0) As SqlParameter
        Dim NomeFile As String

        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@IdBandoAttivita"
        arrParam(0).SqlDbType = SqlDbType.Int
        arrParam(0).Value = IdBandoAttivita.Value

        MyDataTable = New DataTable("DocumentiProgetto")
        MyDataTable = ExecuteDataTable("SP_EsportaDocumenti_Istanza", arrParam)
        MyDataSet.Tables.Add(MyDataTable)
        OutputXls(MyDataTable, "DocumentiProgetto", NomeFile)

        LinkDocProgetti.NavigateUrl = "download" & "\" + NomeFile
        LinkDocProgetti.Target = "_blank"

        LinkDocProgetti.Visible = True
        CmdEsportaDoc.Visible = False
        'lblperImgEsxport.Visible = False

    End Sub
    Sub EsportazioneProgrammi()

        Dim i As Integer
        Dim arrParam(0) As SqlParameter
        Dim NomeFile As String

        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@IdBandoProgramma"
        arrParam(0).SqlDbType = SqlDbType.Int
        arrParam(0).Value = IdBandoProgramma.Value

        MyDataTable = New DataTable("DocumentiProgramma")
        MyDataTable = ExecuteDataTable("SP_EsportaDocumenti_Istanza_Programmi", arrParam)
        MyDataSet.Tables.Add(MyDataTable)
        OutputXls(MyDataTable, "DocumentiProgramma", NomeFile)

        LinkDocProgramma.NavigateUrl = "download" & "\" + NomeFile
        LinkDocProgramma.Target = "_blank"

        LinkDocProgramma.Visible = True
        CmdEsportaDoc.Visible = False
        'lblperImgEsxport.Visible = False

    End Sub
    Protected Sub CmdEsportaDocRiepilogo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles CmdEsportaDocRiepilogo.Click
        export2.Visible = True
        export3.Visible = True
        EsportazioneRiepilogoDocumentiProgetti()
        EsportazioneRiepilogoDocumentiProgramma()
    End Sub
    Sub EsportazioneRiepilogoDocumentiProgetti()

        Dim i As Integer
        Dim arrParam(0) As SqlParameter
        Dim NomeFile As String

        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@IdBandoAttivita"
        arrParam(0).SqlDbType = SqlDbType.Int
        arrParam(0).Value = IdBandoAttivita.Value

        MyDataTable = New DataTable("DocumentiRiepilogoProgetti")
        MyDataTable = ExecuteDataTable("SP_EsportaRiepilogoProgetti_TipoDocumento", arrParam)
        MyDataSet.Tables.Add(MyDataTable)
        OutputXls(MyDataTable, "DocumentiRiepilogoProgetti", NomeFile)

        LinkRielilogoProgetti.NavigateUrl = "download" & "\" + NomeFile
        LinkRielilogoProgetti.Target = "_blank"

        LinkRielilogoProgetti.Visible = True
        CmdEsportaDocRiepilogo.Visible = False
        'lblperImgEsxportRiepig.Visible = False
    End Sub
    Sub EsportazioneRiepilogoDocumentiProgramma()
        Dim i As Integer
        Dim arrParam(0) As SqlParameter
        Dim NomeFile As String

        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@IdBandoProgramma"
        arrParam(0).SqlDbType = SqlDbType.Int
        arrParam(0).Value = IdBandoProgramma.Value

        MyDataTable = New DataTable("DocumentiRiepilogoProgramma")
        MyDataTable = ExecuteDataTable("SP_EsportaRiepilogoProgrammi_TipoDocumento", arrParam)
        MyDataSet.Tables.Add(MyDataTable)
        OutputXls(MyDataTable, "DocumentiRiepilogoProgramma", NomeFile)

        LinkRiepilogoProgrammi.NavigateUrl = "download" & "\" + NomeFile
        LinkRiepilogoProgrammi.Target = "_blank"

        LinkRiepilogoProgrammi.Visible = True
        CmdEsportaDocRiepilogo.Visible = False
        'lblperImgEsxportRiepig.Visible = False
    End Sub
    Public Function ExecuteDataTable(ByVal storedProcedureName As String, ByVal ParamArray arrParam() As SqlParameter) As DataTable
        Dim dt As DataTable

        ' Define the command 
        Dim cmd As New SqlCommand
        cmd.Connection = Session("Conn")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = storedProcedureName

        ' Handle the parameters 
        If Not arrParam.Length = 0 Then
            For Each param As SqlParameter In arrParam
                cmd.Parameters.Add(param)
            Next
        End If

        ' Define the data adapter and fill the dataset 
        Dim da As New SqlDataAdapter(cmd)
        dt = New DataTable
        da.Fill(dt)

        Return dt
    End Function
    Private Function OutputXls(ByVal Datasource As DataTable, ByVal Tipofile As String, ByRef NomeFile As String) As Boolean

        NomeFile = Session("Utente") & "_" & Tipofile & "_" & Format(DateTime.Now, "ddMMyyyyhhmmss") & "_" & ".csv"

        Dim stringWrite As System.IO.StringWriter = New System.IO.StringWriter

        If File.Exists(Server.MapPath("download") & "\" & NomeFile) Then
            File.Delete((Server.MapPath("download") & "\" & NomeFile))
        End If
        SaveTextToFile(MyDataTable, NomeFile)
        Return True
    End Function
    Function SaveTextToFile(ByVal DTBRicerca As DataTable, ByVal NomeFile As String)

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String

        'xPrefissoNome = Session("Utente")
        NomeUnivoco = NomeFile
        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco)
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        Writer.WriteLine(xLinea)
        xLinea = vbNullString
        'If DTBRicerca.Rows.Count = 0 Then
        '    'lblErr.Text = lblErr.Text & "La ricerca non ha prodotto nessun risultato."

        'Else


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
        ' End If
        Writer.Close()
        Writer = Nothing
    End Function
    Protected Sub ImgAnteprimaStampa_Click(sender As Object, e As EventArgs) Handles ImgAnteprimaStampa.Click
        Dim JScript As String
        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=73&IdBP=" & IdBandoProgramma.Value & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
        JScript &= ("</script>")
        Response.Write(JScript)
    End Sub
    Protected Sub imgStampaAll_Click(sender As Object, e As EventArgs) Handles imgStampaAll.Click
        Dim JScript As String

        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=72&IdBP=" & IdBandoProgramma.Value & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf


        JScript &= ("</script>")

        Response.Write(JScript)
        RegistraStampaAvvenuta()
    End Sub
    Private Sub RegistraStampaAvvenuta()
        Try
            Dim cmdinsert As Data.SqlClient.SqlCommand

            ' CInt(txtidbandoAttivita.Text)
            'Funzione che tiene traccia delle stampe avvenute 
            Dim strSqlCommand As String = " INSERT INTO BandiAttivitàStampe " & vbCrLf
            strSqlCommand &= " VALUES ("
            strSqlCommand &= IdBandoAttivita.Value & ","
            strSqlCommand &= "GETDATE(),"
            strSqlCommand &= "'" & ClsServer.NoApice(Session("utente")) & "',"
            strSqlCommand &= "1,"
            If Not IsNothing(Session("Sap")) Then
                If Session("Sap") = True Then
                    strSqlCommand &= "1,"
                Else
                    strSqlCommand &= "0,"
                End If
            Else
                strSqlCommand &= "0,"
            End If
            If Not IsNothing(Session("SapEst")) Then
                If Session("SapEst") = True Then
                    strSqlCommand &= "1"
                Else
                    strSqlCommand &= "0"
                End If
            Else
                strSqlCommand &= "0"
            End If
            strSqlCommand &= ")"


            cmdinsert = New Data.SqlClient.SqlCommand(strSqlCommand, Session("conn"))


            cmdinsert.ExecuteNonQuery()

            strSqlCommand = " INSERT INTO BandiProgrammiStampe " & vbCrLf
            strSqlCommand &= " VALUES ("
            strSqlCommand &= IdBandoProgramma.Value & ","
            strSqlCommand &= "GETDATE(),"
            strSqlCommand &= "'" & ClsServer.NoApice(Session("utente")) & "',"
            strSqlCommand &= "1,"
            If Not IsNothing(Session("Sap")) Then
                If Session("Sap") = True Then
                    strSqlCommand &= "1,"
                Else
                    strSqlCommand &= "0,"
                End If
            Else
                strSqlCommand &= "0,"
            End If
            If Not IsNothing(Session("SapEst")) Then
                If Session("SapEst") = True Then
                    strSqlCommand &= "1"
                Else
                    strSqlCommand &= "0"
                End If
            Else
                strSqlCommand &= "0"
            End If
            strSqlCommand &= ")"
            cmdinsert.CommandText = strSqlCommand
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub DgtProgrammi_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DgtProgrammi.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Dim info As String
                Dim idProgramma As Integer
                info = "InfoProgramma"
                idProgramma = e.Item.Cells(0).Text
                OpenWindow(info, idProgramma)
            Case "Documenti"

                Response.Redirect("WfrmProgrammiDocumenti.aspx?VengoDa=IstanzaProgramma&IdProgramma=" & e.Item.Cells(0).Text & "&idBP=" & Request.QueryString("idBP"))

                'Case "Applica"

                ' Response.Redirect("wfrmDocumentiProgetto_Applica.aspx?VengoDa=Istanza&IdAttivita=" & e.Item.Cells(0).Text & " &id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))

        End Select
    End Sub
    Protected Sub OpenWindow(quale, parametro)
        Select Case quale

            Case "InfoProgramma"
                MaintainScrollPositionOnPostBack = True
                Dim url As String = "WfrmProgrammiVerificaIstanzaAnomaliaProgramma.aspx?IdProgramma=" & parametro & "&IdBando=" & IdBando.Value.ToString
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

            Case "InfoProgetto"
                MaintainScrollPositionOnPostBack = True
                Dim url As String = "WfrmVerificaIstanzaAnomaliaProgetto.aspx?IdAttivita=" & parametro & "&VengoDa=IstanzaProgramma" & "&IdBando=" & IdBando.Value.ToString
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
                MaintainScrollPositionOnPostBack = True
            Case "Olp"
                Dim url As String = "risorsesedidiverse.aspx?idbando=" & parametro & "&VengoDa=" & 7
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=700,left=100,top=100,resizable=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
            Case "Provincia"
                Response.Redirect("WFrmControlloProvincie.aspx?idBando=" & parametro & "&VengoDa=" & 70 & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & IdBandoAttivita.Value & "&IdBP=" & Request.QueryString("IdBP"))

            Case "ImgSellProtollo"
                Dim url As String = "WfrmSIGEDElencoDocumenti.aspx?TxtProt=" & txtNumProt.Text & "&TxtData=" & txtDataProt.Text & "&NumeroFascicolo=" & TxtIdFascicolo.Text & "&VengoDa=" & 9
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
            Case "cmdSelProtocollo"
                Dim url As String = "WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" & TxtIdFascicolo.Text & "&VengoDa=" & 10
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
        End Select

    End Sub
    Private Sub imgCheckOLP_Command(sender As Object, e As System.Web.UI.WebControls.CommandEventArgs) Handles imgCheckOLP.Command
        If e.CommandName = "Olp" Then
            Dim info As String
            Dim idBando1 As Integer
            info = "Olp"
            idBando1 = IdBando.Value
            OpenWindow(info, idBando1)
        End If
    End Sub

    Private Sub ImgControllaProvincie_Command(sender As Object, e As System.Web.UI.WebControls.CommandEventArgs) Handles ImgControllaProvincie.Command
        If e.CommandName = "Provincia" Then
            Dim info As String
            Dim idBando1 As Integer
            info = "Provincia"
            idBando1 = IdBando.Value
            OpenWindow(info, idBando1)
        End If
    End Sub

    Private Sub Dgtattivita_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles Dgtattivita.ItemCommand
        Dim info As String
        Dim idattivita As Integer
        info = "InfoProgetto"
        idattivita = e.Item.Cells(2).Text
        Select Case e.CommandName
            Case "Select"
                OpenWindow(info, idattivita)
            Case "Documenti"
                Response.Redirect("wfrmDocumentiProgetto.aspx?VengoDa=IstanzaProgramma&IdAttivita=" & idattivita.ToString & "&Verso=" & Request.QueryString("Verso") & "&idBP=" & Request.QueryString("idBP"))
            Case "Applica"
                Response.Redirect("wfrmDocumentiProgetto_Applica.aspx?VengoDa=IstanzaProgramma&IdAttivita=" & idattivita.ToString & "&Verso=" & Request.QueryString("Verso") & "&idBP=" & Request.QueryString("idBP"))
        End Select
    End Sub

    Private Function AnnullaPresentazione() As Boolean
        'SP_PROGRAMMI_ISTANZA_ANNULLA_PRESENTAZIONE
        lblErrore.Text = ""
        lblConferma.Text = ""

        Dim ArreyDiMessaggi() As String
        Dim ESITO As Integer = -1
        lblErrore.Text = ""
        lblConferma.Text = ""

        Dim SqlCmd As New SqlClient.SqlCommand

        Try
            SqlCmd.CommandText = "SP_PROGRAMMI_ISTANZA_ANNULLA_PRESENTAZIONE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            'SqlCmd.Parameters.Add("@IdEnteProponente ", SqlDbType.Int).Value = CInt(Session("idente"))
            SqlCmd.Parameters.Add("@IdBandoProgramma", SqlDbType.Int).Value = IdBandoProgramma.Value
            SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Size = 10
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            ESITO = SqlCmd.Parameters("@Esito").Value()
            If ESITO = 0 Then
                AnnullaPresentazione = False
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(SqlCmd.Parameters("@messaggio").Value(), ":")
                lblErrore.Text = ArreyDiMessaggi(0)
                Dim MessErrore() As String
                Dim NuovaStringa As String
                For i = 0 To UBound(ArreyDiMessaggi)
                    If i = 0 Then
                        ArreyDiMessaggi.Clear(ArreyDiMessaggi, 0, 1)
                        'funziona
                        NuovaStringa = ArreyDiMessaggi(1)
                        ReDim MessErrore(0)
                        MessErrore(0) = NuovaStringa
                        ReDim Preserve MessErrore(0)
                    End If
                Next
                ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(MessErrore(0), ".")
                lblErrore.Text = lblErrore.Text & ":" & vbCrLf & "<br/>"
                Dim rigadasplittare() As String

                rigadasplittare = ClsUtility.CreaArrayMessaggi(MessErrore(0), "|")
                For i = 0 To UBound(rigadasplittare) - 1
                    lblErrore.Text = lblErrore.Text & rigadasplittare(i) & vbCrLf & "<br/>"
                Next
            Else
                AnnullaPresentazione = True
                CaricaDati()
                MaintainScrollPositionOnPostBack = True
                lblConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                lblConferma.Text = lblConferma.Text & "<br/>"
                PDFistanzaPre.Visible = False
            End If
        Catch ex As Exception
            AnnullaPresentazione = False
            lblErrore.Text = ex.Message
        Finally

        End Try

        CaricaLoad()

    End Function

  

    Private Sub cmdaccredita_Click(sender As Object, e As System.EventArgs) Handles cmdaccredita.Click
        '***Questa Routine accredita l'istanza presentata
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtr As Data.SqlClient.SqlDataReader
        Dim i As Integer

        If (Trim(lblStatoEnte.Text) = "Attivo" Or Trim(lblStatoEnte.Text) = "In Adeguamento") Then

            lblConferma.Text = ""

            'eseguo comando per cambio stato
            cmdinsert = New Data.SqlClient.SqlCommand("update bandiprogrammi" & _
            " set idstatobandoprogramma=3," & _
            " usernameAccreditatore='" & ClsServer.NoApice(Session("utente")) & "',DataAccettazione=getdate() where idbandoprogramma=" & Request.QueryString("idBP") & "", Session("conn"))
            cmdinsert.ExecuteNonQuery()
            cmdinsert.Dispose()

            'eseguo comando per cambio stato
            cmdinsert = New Data.SqlClient.SqlCommand("update bandiattività" & _
            " set idstatobandoattività=3," & _
            " usernameAccreditatore='" & ClsServer.NoApice(Session("utente")) & "',DataAccettazione=getdate() where idbandoattività=" & IdBandoAttivita.Value & "", Session("conn"))
            cmdinsert.ExecuteNonQuery()
            cmdinsert.Dispose()

            'eseguo query per visualizzazione stato attuale
            dtr = ClsServer.CreaDatareader("select b.statobandoprogramma from bandiprogrammi a" & _
            " inner join statibandiprogrammi b on a.idstatobandoprogramma=b.idstatobandoprogramma" & _
            " where a.idbandoprogramma=" & Request.QueryString("idBP") & "", Session("conn"))
            If dtr.HasRows = True Then
                Do While dtr.Read()
                    lblstato.Text = dtr.GetValue(0) 'visualizzo stato
                    cmddissaccredita.Visible = False
                    cmdaccredita.Visible = False
                Loop
            End If
            dtr.Close()
            dtr = Nothing

            'setto wform

            lblConferma.Text = "L'istanza e' stata accettata con successo"
            cmdInserisci.Visible = False

            'If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            '    Call VerificaCompetenze()
            'End If

        Else

            lblConferma.Text = "L'istanza non puo' essere accettata perche' lo stato dell'ente non e' Attivo."
            Exit Sub

        End If
    End Sub

    Private Sub cmddissaccredita_Click(sender As Object, e As System.EventArgs) Handles cmddissaccredita.Click
        '***Questa Routine annulla l'accreditamento 
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtable As DataTable
        Dim myRow As DataRow

        lblConferma.Text = ""

        imgStampaAll.Visible = False

        Session("Sap") = False
        Session("SapEst") = False


        'eseguo comando per cambio stato
        cmdinsert = New Data.SqlClient.SqlCommand("update bandiprogrammi" & _
        " set idstatobandoprogramma=5," & _
        " usernameAccreditatore='" & ClsServer.NoApice(Session("utente")) & "',DataAccettazione=getdate() where idbandoprogramma=" & Request.QueryString("idBP") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()

        'eseguo comando per cambio stato
        cmdinsert = New Data.SqlClient.SqlCommand("update bandiattività" & _
        " set idstatobandoattività=5," & _
        " usernameAccreditatore='" & ClsServer.NoApice(Session("utente")) & "',DataAccettazione=getdate() where idbandoattività=" & IdBandoAttivita.Value & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()


        'inserisco in cronologia e modifico lo stato del programma in Archiviato
        cmdinsert = New Data.SqlClient.SqlCommand(" Insert into cronologiaprogrammi " & _
                 " select idprogramma,idstatoprogramma,getdate(),0,'Archiviato per istanza irricevibile', " & _
                 " '" & ClsServer.NoApice(Session("utente")) & "',0 " & _
                 " FROM PROGRAMMI A " & _
                 " INNER JOIN BANDIPROGRAMMI B ON A.IDBANDOPROGRAMMA = B.IDBANDOPROGRAMMA " & _
                 " WHERE a.IDBANDOPROGRAMMA =" & Request.QueryString("idBP") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        cmdinsert = New Data.SqlClient.SqlCommand(" UPDATE PROGRAMMI " & _
                " SET IDSTATOPROGRAMMA = 8,DATAULTIMOSTATO=GETDATE(),USERNAMESTATO = '" & ClsServer.NoApice(Session("utente")) & "', " & _
                " NOTESTATO='Archiviato per istanza irricevibile'  " & _
                " FROM PROGRAMMI A " & _
                " INNER JOIN BANDIPROGRAMMI B ON A.IDBANDOPROGRAMMA = B.IDBANDOPROGRAMMA  " & _
                " WHERE a.IDBANDOPROGRAMMA = " & Request.QueryString("idBP") & "", Session("conn"))

        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        '******

        'inserisco in cronologia e modifico lo stato del progetto in Archiviato
        cmdinsert = New Data.SqlClient.SqlCommand(" Insert into cronologiaattività " & _
                 " select idattività,idstatoattività,getdate(),0,'Archiviato per istanza irricevibile', " & _
                 " '" & ClsServer.NoApice(Session("utente")) & "',0 " & _
                 " FROM ATTIVITà A " & _
                 " INNER JOIN BANDIATTIVITà B ON A.IDBANDOATTIVITà = B.IDBANDOATTIVITà " & _
                 " WHERE a.IDBANDOattività =" & IdBandoAttivita.Value & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        cmdinsert = New Data.SqlClient.SqlCommand(" UPDATE ATTIVITà " & _
                " SET IDSTATOATTIVITà = 11,DATAULTIMOSTATO=GETDATE(),USERNAMESTATO = '" & ClsServer.NoApice(Session("utente")) & "', " & _
                " NOTESTATO='Archiviato per istanza inammissibile'  " & _
                " FROM ATTIVITà A " & _
                " INNER JOIN BANDIATTIVITà B ON A.IDBANDOATTIVITà = B.IDBANDOATTIVITà  " & _
                " WHERE a.IDBANDOATTIVITà = " & IdBandoAttivita.Value & "", Session("conn"))

        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        '******

        'eseguo query per visualizzazione stato attuale
        'Modificato da Alessandra Taballione il 02/02/2005
        'Agguinto controllo sullo stato per blocco Attività
        dtable = ClsServer.CreaDataTable("select b.statobandoprogramma from bandiprogrammi a" & _
            " inner join statibandiprogrammi b on a.idstatobandoprogramma=b.idstatobandoprogramma" & _
            " where a.idbandoprogramma=" & Request.QueryString("idBP") & "", False, Session("conn"))

        For Each myRow In dtable.Rows
            lblstato.Text = myRow.Item("statobandoprogramma") 'dtr.GetValue(0) 'visualizzo stato
            cmdaccredita.Visible = False
            cmddissaccredita.Visible = False
        Next
        'visualizzo stato

        lblConferma.Text = "L'istanza e' stata respinta."
        cmdInserisci.Visible = False
        'Dgtattivita.Columns(16).Visible = True
        'Dgtattivita.Columns(17).Visible = True 'colonna popup documenti

        'Dgtattivita.Columns(18).Visible = True 'colonna pulcanse applica
        'Dgtattivita.Columns(19).Visible = True 'colonna popup info 

        'cmdannulla.Visible = False
        'cmdmodifica.Visible = False
        'cmddissaccredita.Visible = False
        'cmdaccredita.Visible=False
        'Aggiunto da Alessandra Taballione il 02.02.2005
        CaricaGriglia()
    End Sub


    Protected Sub btnConfermaAnnullaPresentazione_Click(sender As Object, e As EventArgs) Handles btnConfermaAnnullaPresentazione.Click
        Dim esito As Boolean
        esito = AnnullaPresentazione()
        If esito = True Then
            AbilitaDisabilita()
        End If
    End Sub
    Private Function linkpdf() As Boolean
        Dim dtrstrsql As System.Data.SqlClient.SqlDataReader
        Dim strsql As String
        Dim DocumentoPdf As String
        Dim _allegato As Byte()
        strsql = "select FileName FROM ProtocolloIstanzaProgramma a inner join bando on a.IdBando=bando.IDBando inner join BandiProgrammi on bando.IDBando= BandiProgrammi.IdBando  where a.IdEnte =" & Session("idente") & " And IdBandoProgramma =" & IdBandoProgramma.Value & " And dataannullamento is null"
        dtrstrsql = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrstrsql.Read()
        If dtrstrsql.HasRows = True Then
            ChiudiDataReader(dtrstrsql)
            Return True
        Else
            ChiudiDataReader(dtrstrsql)
            Return False
        End If
        ChiudiDataReader(dtrstrsql)

    End Function



    Protected Sub PDFistanzaPre_Click(sender As Object, e As EventArgs) Handles PDFistanzaPre.Click
        Dim dtrstrsql As System.Data.SqlClient.SqlDataReader
        Dim strsql As String
        Dim DocumentoPdf As String
        Dim _allegato As Byte()
        strsql = "select FileName,BinData FROM ProtocolloIstanzaProgramma a inner join bando on a.IdBando=bando.IDBando inner join BandiProgrammi on bando.IDBando= BandiProgrammi.IdBando  where a.IdEnte =" & Session("idente") & " And IdBandoProgramma =" & IdBandoProgramma.Value & " And dataannullamento is null"
        dtrstrsql = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrstrsql.Read()
        If dtrstrsql.HasRows = True Then
            DocumentoPdf = dtrstrsql("FileName")
            _allegato = DirectCast(dtrstrsql("BinData"), Byte())
            ChiudiDataReader(dtrstrsql)
            Response.Clear()
            Response.ContentType = "Application/pdf"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & DocumentoPdf)
            Response.BinaryWrite(_allegato)
            Response.End()
        End If
        ChiudiDataReader(dtrstrsql)


        

        'Dim Filename As String = MapPath(DocumentoPdf)

        ' ''This is an important header part that informs the client to download this file.
        'Response.AppendHeader("content-disposition", "attachment; filename=" + Path.GetFileName(Filename))
        'Response.ContentType = "Application/pdf"
        ' ''Write the file directly to the HTTP content output stream.
        'Response.WriteFile(Filename)

    End Sub

    Private Sub scaricaPdfIstanza(IdProtocolloIstanzaProgramma As Integer)
        Dim dtrstrsql As System.Data.SqlClient.SqlDataReader
        Dim strsql As String
        Dim DocumentoPdf As String
        Dim _allegato As Byte()
        strsql = "select FileName,BinData FROM ProtocolloIstanzaProgramma where id=" & IdProtocolloIstanzaProgramma
        dtrstrsql = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrstrsql.Read()
        If dtrstrsql.HasRows = True Then
            DocumentoPdf = dtrstrsql("FileName")
            _allegato = DirectCast(dtrstrsql("BinData"), Byte())
            ChiudiDataReader(dtrstrsql)
            Response.Clear()
            Response.ContentType = "Application/pdf"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & DocumentoPdf)
            Response.BinaryWrite(_allegato)
            Response.End()
        End If
        ChiudiDataReader(dtrstrsql)
    End Sub

    Protected Sub dtgStoricoPresentazione_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgStoricoPresentazione.ItemCommand

        If e.CommandName = "Scarica" Then
            Dim lnkPDFistanzaStorico As LinkButton = DirectCast(e.Item.FindControl("PDFistanzaSto"), LinkButton)
            If lnkPDFistanzaStorico.Visible Then
                scaricaPdfIstanza(Integer.Parse(e.Item.Cells(7).Text))
            End If
        End If

    End Sub

    Protected Sub dtgStoricoPresentazione_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgStoricoPresentazione.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim lnkPDFistanzaStorico As LinkButton = DirectCast(e.Item.FindControl("PDFistanzaSto"), LinkButton)
            If IsDBNull(e.Item.DataItem("Id")) Then lnkPDFistanzaStorico.Visible = False Else lnkPDFistanzaStorico.Visible = True
        End If
    End Sub

    Protected Sub dtgStoricoPresentazione_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgStoricoPresentazione.PageIndexChanged
        Dim dataSet As New DataSet
        dtgStoricoPresentazione.CurrentPageIndex = e.NewPageIndex
        dataSet = Session("dtsStoricoPresentazione")
        dtgStoricoPresentazione.DataSource = dataSet
        dtgStoricoPresentazione.DataBind()
    End Sub
End Class