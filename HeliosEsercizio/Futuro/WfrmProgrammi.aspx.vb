Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Color
Imports Logger.Data

Public Class WfrmProgrammi
    Inherits SmartPage
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
#Region "dichiarazionivariabili"
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim errore As String
    Dim classeHelper As Helper = New Helper()
    'Dim strProgramma As Integer = -1
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid(True)
        VerificaSessione()

        If Request.QueryString("IdProgramma") <> "" Then
            IdProgramma.Value = Request.QueryString("IdProgramma")
        End If

        Dim strATTIVITA As Integer = -1
        Dim strBANDOATTIVITA As Integer = -1
        Dim strENTEPERSONALE As Integer = -1
        Dim strENTITA As Integer = -1
        Dim strIDENTE As Integer = -1


        If IdProgramma.Value <> "" Then
            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE, IdProgramma.Value) = 1 Then
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

            If Session("TipoUtente") = "E" Then
                Dim abilitato As Integer

                abilitato = ClsUtility.LoadProgrammiAbilitaModificaEnte(IdProgramma.Value, Session("Conn"))
                If abilitato = 0 Then
                    BloccaMaschera("L'ente non è abilitato modifica")
                Else
                    'controllo se coprogrammante. allora personalizzo funzioni
                    If ClsUtility.ProgrammiLimitaFunzioniCoprogrammante(IdProgramma.Value, Session("IdEnte"), Session("Conn")) Then
                        BloccaMaschera("L'ente coprogrammante non può effettuare modifiche al programma")
                        imgElencoDocumentiProg.Visible = False
                    End If
                End If

            End If
            If Session("TipoUtente") = "R" Then
                BloccaMaschera("Utente non abilitato alla modifica")
            End If
        Else
            'inserimento quindi ente almeno in istruttoria

            Dim strsqlstato As String
            Dim dtrStato As System.Data.SqlClient.SqlDataReader
            strsqlstato = "select idente from enti  where idente = " & Session("IdEnte") & " and idstatoente in (3,8,9) and albo = 'scu' and idclasseaccreditamentorichiesta in (8,9) "

            'controllo se utente o ente regionale
            'eseguo la query
            dtrStato = ClsServer.CreaDatareader(strsqlstato, Session("conn"))
            dtrStato.Read()
            If dtrStato.HasRows = False Then
                BloccaMaschera("L'ente non è abilitato all'inserimento di programmi")
            End If
            ChiudiDataReader(dtrStato)
            cmdElimina.Visible = False

            If Session("TipoUtente") = "R" Then
                BloccaMaschera("Utente non abilitato all'inserimento")
            End If
        End If

        If Page.IsPostBack = False Then
            If IdProgramma.Value = "" Then 'Inserimento load
                lblprogetti.Visible = True
                lblSettori.Visible = True
                lblcodiceentePropo.Text = Session("txtCodEnte")
                lblDenominazioneEnte.Text = Session("Denominazione")
                idlinkalti.Visible = False
                divCoProgrammazione.Visible = False
                NvolEDurata.Visible = False
                ContatoriGMOUETUTORAGGIO1.Visible = False
                'ContatoriGMOUETUTORAGGIO2.Visible = False
                CaricaGrigliaObiettivi()
                CaricaGrigliaAmbitoAzione()
                CaricaComboTerritorio()
                CaricaComboTipiProgrammi()
                CaricaComboRegioniGG()
                CaricaComboMisureGG("nessuna")
                DivProgrammiGG.Visible = False
            Else 'Modifica 

                CaricaProgramma(IdProgramma.Value, 0)

            End If
        End If



    End Sub
    Sub CaricaProgramma(ByVal IdProgramma As Integer, ByVal Visualizzazione As Integer)
        lblprogetti.Visible = False
        lblSettori.Visible = False
        idlinkalti.Visible = True
        divCoProgrammazione.Visible = True
        NvolEDurata.Visible = True
        ContatoriGMOUETUTORAGGIO1.Visible = True
        'ContatoriGMOUETUTORAGGIO2.Visible = True
        CaricaGrigliaObiettivi()
        CaricaGrigliaObiettiviMod()
        CaricaGrigliaAmbitoAzione()
        CaricaGrigliaAmbitoAzioneMod()
        CaricaComboTerritorio()
        CaricaComboTipiProgrammi()
        CaricaComboRegioniGG()
        CaricaDatiMaschera(IdProgramma)
        CaricaGrigliaElencoProgettiProgramma()
        CaricaGrigliaSettori()
        CaricaGrigliadgRicercaEnteCoprogrammante()
    End Sub
    Private Sub CaricaComboTerritorio()
        Dim dtrTerritorio As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT 0 as IdTerritorio,'' as Descrizione UNION " & _
                 "SELECT IdTerritorio AS IdTerritorio, Descrizione From Territori order by IdTerritorio"
        dtrTerritorio = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrTerritorio.HasRows = True Then
            ddlTerritorio.DataSource = dtrTerritorio
            ddlTerritorio.DataTextField = "Descrizione"
            ddlTerritorio.DataValueField = "IdTerritorio"
            ddlTerritorio.DataBind()
        End If
        ChiudiDataReader(dtrTerritorio)


    End Sub
    Private Sub CaricaComboTipiProgrammi()
        Dim dtrTipiProgramma As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT 0 as IdTipoProgramma,'' as Descrizione UNION " & _
                 "SELECT IdTipoProgramma , Descrizione From TipiProgramma order by IdTipoProgramma"
        dtrTipiProgramma = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrTipiProgramma.HasRows = True Then
            ddlTipoProgramma.DataSource = dtrTipiProgramma
            ddlTipoProgramma.DataTextField = "Descrizione"
            ddlTipoProgramma.DataValueField = "IdTipoProgramma"
            ddlTipoProgramma.DataBind()
        End If
        ChiudiDataReader(dtrTipiProgramma)
    End Sub

    Private Sub CaricaComboRegioniGG()
        Dim dtrRegioniGG As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT '' as Regione UNION " & _
                 "SELECT distinct Regione From TipiGG where attivo = 1 order by Regione"
        dtrRegioniGG = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrRegioniGG.HasRows = True Then
            ddlRegioneGG.DataSource = dtrRegioniGG
            ddlRegioneGG.DataTextField = "Regione"
            ddlRegioneGG.DataValueField = "Regione"
            ddlRegioneGG.DataBind()
        End If
        ChiudiDataReader(dtrRegioniGG)
    End Sub

    Private Sub CaricaComboMisureGG(ByVal regione As String)
        Dim dtrMisureGG As SqlClient.SqlDataReader
        Dim strSql As String
        Dim STRIDPROGRAMMA As String
        If IdProgramma.Value <> "" Then
            STRIDPROGRAMMA = IdProgramma.Value
        Else
            STRIDPROGRAMMA = -1
        End If

        strSql = "SELECT 0 as IdTipoGG,'' as Descrizione, '' as Regione UNION " & _
                 "SELECT idtipogg, descrizione, Regione From TipiGG where attivo = 1 and regione = '" & regione.Replace("'", "''") & "' " & _
                 "UNION SELECT A.IDTIPOGG,A.DESCRIZIONE,A.REGIONE FROM TIPIGG A INNER JOIN PROGRAMMI B ON A.IdTipoGG = B.IdTipoGG WHERE IdProgramma = " & STRIDPROGRAMMA & " " & _
                 "order by idtipogg"
        dtrMisureGG = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrMisureGG.HasRows = True Then
            ddlMisuraGG.DataSource = dtrMisureGG
            ddlMisuraGG.DataTextField = "descrizione"
            ddlMisuraGG.DataValueField = "idtipogg"
            ddlMisuraGG.DataBind()
        End If
        ChiudiDataReader(dtrMisureGG)
    End Sub


    Private Sub CaricaGrigliaObiettivi()
        Dim strSql As String
        strSql = "SELECT * from obiettivi order by 1  "
        Session("sqlDataSetObiettivi") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dtgObiettivi.DataSource = Session("sqlDataSetObiettivi")
        dtgObiettivi.DataBind()

        ChiudiDataReader(dtrgenerico)

    End Sub
    Private Sub CaricaGrigliaObiettiviMod()
        Dim indiceOb As String = ""
        If IdProgramma.Value <> "" Then
            Dim strSql1 As String
            strSql1 = "SELECT a.idObiettivo from Obiettivi a inner join programmiObiettivi b on a.idObiettivo = b.idObiettivo where idprogramma = " & IdProgramma.Value
            dtrgenerico = ClsServer.CreaDatareader(strSql1, Session("conn"))

            Dim x As DataGridItem

            Do While dtrgenerico.Read()
                indiceOb = dtrgenerico("idObiettivo")

                For Each x In dtgObiettivi.Items
                    Dim check As CheckBox = DirectCast(x.FindControl("chkSeleziona"), CheckBox)
                    If dtgObiettivi.Items(x.ItemIndex).Cells(0).Text = indiceOb Then
                        check.Checked = True
                        dtgObiettivi.Items(x.ItemIndex).ForeColor = Black
                        dtgObiettivi.Items(x.ItemIndex).Font.Bold = True
                        dtgObiettivi.Items(x.ItemIndex).BackColor = LightGray
                        Exit For

                    End If
                Next
            Loop

        End If
        ChiudiDataReader(dtrgenerico)


    End Sub
    Private Sub CaricaGrigliaAmbitoAzione()
        Dim strSql As String
        strSql = "SELECT * from AmbitiAzione order by 1 "
        Session("sqlDataSetAmbitoAzione") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dtgAmbitodiAzione.DataSource = Session("sqlDataSetAmbitoAzione")
        dtgAmbitodiAzione.DataBind()
        ChiudiDataReader(dtrgenerico)
    End Sub
    Private Sub CaricaGrigliaAmbitoAzioneMod()
        Dim indice As String = ""
        Dim strSql1 As String
        strSql1 = "SELECT a.idAmbitoAzione from AmbitiAzione a inner join programmi b on a.idambitoazione = b.idambitoazione where idprogramma = " & IdProgramma.Value
        dtrgenerico = ClsServer.CreaDatareader(strSql1, Session("conn"))
        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            indice = dtrgenerico("idAmbitoAzione")
        End If



        For Each indiceADC In dtgAmbitodiAzione.Items
            Dim check As CheckBox = DirectCast(indiceADC.FindControl("chkSeleziona"), CheckBox)
            If dtgAmbitodiAzione.Items(indiceADC.ItemIndex).Cells(0).Text = indice Then
                check.Checked = True
                dtgAmbitodiAzione.Items(indiceADC.ItemIndex).ForeColor = Black
                dtgAmbitodiAzione.Items(indiceADC.ItemIndex).Font.Bold = True
                dtgAmbitodiAzione.Items(indiceADC.ItemIndex).BackColor = LightGray
            Else
                ChiudiDataReader(dtrgenerico)

            End If
        Next


        ChiudiDataReader(dtrgenerico)

    End Sub
    Private Sub CaricaGrigliaElencoProgettiProgramma()
        Dim strSql As String
        strSql = "SELECT IDAttività,IdProgramma, IdEnteReferenteProgramma, Titolo,IdTipoProgetto, NumeroPostiNoVittoNoAlloggio + NumeroPostiVittoAlloggio + NumeroPostiVitto as NPostiVol,DataAssociazioneProgramma,MesiDurata FROM attività where idprogramma = " & IdProgramma.Value & " order by DataAssociazioneProgramma asc"
        Session("ElencoProgettiProgramma") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dgElencoProgettiProgramma.DataSource = Session("ElencoProgettiProgramma")
        dgElencoProgettiProgramma.DataBind()
        dgElencoProgettiProgramma.Visible = True

        ChiudiDataReader(dtrgenerico)
    End Sub
    Private Sub CaricaGrigliaSettori()
        Dim strSql As String
        strSql = "SELECT ProgrammiSettori.IdProgrammaSettore, ProgrammiSettori.IdProgramma, macroambitiattività.Codifica, macroambitiattività.MacroAmbitoAttività " & _
                  "FROM ProgrammiSettori INNER JOIN macroambitiattività ON ProgrammiSettori.IdMacroAmbitoAttività = macroambitiattività.IDMacroAmbitoAttività where idprogramma = " & IdProgramma.Value
        Session("ElencoMacroAmbiti") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dtgMacroAmbito.DataSource = Session("ElencoMacroAmbiti")
        dtgMacroAmbito.DataBind()
        dtgMacroAmbito.Visible = True
        ChiudiDataReader(dtrgenerico)
    End Sub
    Private Sub CaricaGrigliadgRicercaEnteCoprogrammante()
        Dim sqlDAPstrNomeStoreEnteCoprogrammante As New SqlClient.SqlDataAdapter
        Dim dataSetstrNomeStoreEnteCoprogrammante As New DataSet
        Dim strNomeStoreEnteCoprogrammante As String = "[SP_PROGRAMMI_EntiCoprogrammanti]"

        Try
            sqlDAPstrNomeStoreEnteCoprogrammante = New SqlClient.SqlDataAdapter(strNomeStoreEnteCoprogrammante, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAPstrNomeStoreEnteCoprogrammante.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAPstrNomeStoreEnteCoprogrammante.SelectCommand.Parameters.Add("@idProgramma", SqlDbType.VarChar).Value = CInt(IdProgramma.Value)
            sqlDAPstrNomeStoreEnteCoprogrammante.Fill(dataSetstrNomeStoreEnteCoprogrammante)
            dgRicercaEnteCoprogrammante.DataSource = dataSetstrNomeStoreEnteCoprogrammante
            dgRicercaEnteCoprogrammante.DataBind()
            dgRicercaEnteCoprogrammante.Visible = True
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try


        ChiudiDataReader(dtrgenerico)
    End Sub
    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim strElencoObiettivi As String = ""
        Dim Indice As DataGridItem
        For Each Indice In dtgObiettivi.Items
            Dim check As CheckBox = DirectCast(Indice.FindControl("chkSeleziona"), CheckBox)
            If check.Checked = True Then
                strElencoObiettivi = strElencoObiettivi & dtgObiettivi.Items(Indice.ItemIndex).Cells(0).Text & ","
            End If
        Next

        Dim ArreyDiMessaggi() As String
        Dim ESITO As Integer = -1
        lblerrore.Text = ""
        lblMessaggioConferma.Text = ""
        If IdProgramma.Value = "" Then 'inserimento
            Dim SqlCmd As New SqlClient.SqlCommand

            Try
                SqlCmd.CommandText = "SP_PROGRAMMI_INSERIMENTO"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Connection = Session("Conn")

                SqlCmd.Parameters.Add("@IdEnteProponente ", SqlDbType.Int).Value = CInt(Session("idente"))
                SqlCmd.Parameters.Add("@Titolo ", SqlDbType.VarChar).Value = txtTitoloProgramma.Text
                SqlCmd.Parameters.Add("@ElencoObiettivi", SqlDbType.VarChar).Value = strElencoObiettivi
                SqlCmd.Parameters.Add("@IdAmbitoAzione", SqlDbType.Int).Value = IIf(IdAmbitoSelezionato.Value = "", 0, IdAmbitoSelezionato.Value)
                SqlCmd.Parameters.Add("@IdTerritorio", SqlDbType.Int).Value = ddlTerritorio.SelectedItem.Value
                SqlCmd.Parameters.Add("@IdTipoProgramma", SqlDbType.Int).Value = ddlTipoProgramma.SelectedItem.Value
                If DivProgrammiGG.Visible Then
                    SqlCmd.Parameters.Add("@IdTipoGG", SqlDbType.Int).Value = ddlMisuraGG.SelectedItem.Value
                End If
                SqlCmd.Parameters.Add("@Reti", SqlDbType.VarChar).Value = ddlReti.SelectedItem.Text
                SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
                SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
                SqlCmd.Parameters("@Esito").Size = 10
                SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
                SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
                SqlCmd.Parameters("@messaggio").Size = 1000
                SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output
                SqlCmd.Parameters.Add("@IdProgrammaInserito", SqlDbType.Int)
                SqlCmd.Parameters("@IdProgrammaInserito").Direction = ParameterDirection.Output
                SqlCmd.ExecuteNonQuery()

                ESITO = SqlCmd.Parameters("@Esito").Value()
                If ESITO = 0 Then
                    ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(SqlCmd.Parameters("@messaggio").Value(), ":")
                    lblerrore.Text = ArreyDiMessaggi(0)
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
                    lblerrore.Text = lblerrore.Text & ":" & vbCrLf & "<br/>"
                    Dim rigadasplittare() As String

                    rigadasplittare = ClsUtility.CreaArrayMessaggi(MessErrore(0), "|")
                    For i = 0 To UBound(rigadasplittare) - 1
                        lblerrore.Text = lblerrore.Text & rigadasplittare(i) & vbCrLf & "<br/>"
                    Next
                Else
                    MaintainScrollPositionOnPostBack = True
                    lblMessaggioConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                    lblMessaggioConferma.Text = lblMessaggioConferma.Text & "<br/>"
                End If
            Catch ex As Exception
                lblerrore.Text = ex.Message
            Finally

            End Try
            MaintainScrollPositionOnPostBack = False
            If SqlCmd.Parameters("@IdProgrammaInserito").Value() <> -1 Then
                IdProgramma.Value = SqlCmd.Parameters("@IdProgrammaInserito").Value()
                CaricaProgramma(IdProgramma.Value, 0)
                cmdElimina.Visible = True
            End If

        Else 'modifica
            Dim SqlCmd As New SqlClient.SqlCommand

            Try
                SqlCmd.CommandText = "SP_PROGRAMMI_MODIFICA"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Connection = Session("Conn")

                SqlCmd.Parameters.Add("@IdProgramma ", SqlDbType.Int).Value = IdProgramma.Value
                SqlCmd.Parameters.Add("@Titolo ", SqlDbType.VarChar).Value = txtTitoloProgramma.Text
                SqlCmd.Parameters.Add("@ElencoObiettivi", SqlDbType.VarChar).Value = strElencoObiettivi
                SqlCmd.Parameters.Add("@IdAmbitoAzione", SqlDbType.Int).Value = IdAmbitoSelezionato.Value
                SqlCmd.Parameters.Add("@IdTerritorio", SqlDbType.Int).Value = ddlTerritorio.SelectedItem.Value
                SqlCmd.Parameters.Add("@IdTipoProgramma", SqlDbType.Int).Value = ddlTipoProgramma.SelectedItem.Value
                If DivProgrammiGG.Visible Then
                    SqlCmd.Parameters.Add("@IdTipoGG", SqlDbType.Int).Value = ddlMisuraGG.SelectedItem.Value
                End If
                SqlCmd.Parameters.Add("@Reti", SqlDbType.VarChar).Value = ddlReti.SelectedItem.Text
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

                    lblerrore.Text = ArreyDiMessaggi(0)
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
                    lblerrore.Text = lblerrore.Text & ":" & vbCrLf & "<br/>"
                    Dim rigadasplittare() As String

                    rigadasplittare = ClsUtility.CreaArrayMessaggi(MessErrore(0), "|")
                    For i = 0 To UBound(rigadasplittare) - 1
                        lblerrore.Text = lblerrore.Text & rigadasplittare(i) & vbCrLf & "<br/>"
                    Next
                Else
                    lblMessaggioConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                    lblMessaggioConferma.Text = lblMessaggioConferma.Text & "<br/>"
                End If
                MaintainScrollPositionOnPostBack = False
            Catch ex As Exception
                lblerrore.Text = ex.Message
            Finally

            End Try

        End If
    End Sub
    Protected Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        Select Case Request.QueryString("VengoDa")
            Case "AccettazioneProgrammi"

                Response.Redirect("assegnazionevincoliprogrammi.aspx?idprogramma=" & Request.QueryString("idProgramma") & "&VengoDa=" & Request.QueryString("VengoDa") & "")
            Case "ValQualita"

                Response.Redirect("WfrmValutazioneQualProgrammi.aspx?idprogramma=" & Request.QueryString("idProgramma") & "&VengoDa=" & Request.QueryString("VengoDa") & "")
            Case Else

                Response.Redirect("WfrmMain.aspx")
        End Select
        'If Request.QueryString("VengoDa") = "AccettazioneProgrammi" Then
        '    Response.Redirect("assegnazionevincoliprogrammi.aspx?idprogramma=" & Request.QueryString("idProgramma") & "&VengoDa=" & Request.QueryString("VengoDa") & "")
        'End If
        'If Request.QueryString("VengoDa") = "ValQualita" Then
        '    Response.Redirect("assegnazionevincoliprogrammi.aspx?idprogramma=" & Request.QueryString("idProgramma") & "&VengoDa=" & Request.QueryString("VengoDa") & "")
        'End If


    End Sub
    Sub CaricaDatiMaschera(ByVal IdProgramma As Integer)
        Dim strSql As String
        Dim strRegione As String = ""
        Dim intIdTipoGG As Integer = 0

        strSql = "SELECT a.IdenteProponente,a.titolo,a.NumeroPostiNoVittoNoAlloggio + a.NumeroPostiVittoAlloggio + a.NumeroPostiVitto as NPostiVol,a.idTerritorio,a.idtipoprogramma, d.macrotipoprogramma, isnull(a.idtipogg,0) as idtipogg, isnull(c.regione,'') as RegioneGG , a.reti,a.NMesi,b.codiceregione,b.denominazione,a.idambitoazione, a.coprogrammazione  from Programmi a inner join Enti b on a.identeproponente = b.idente inner join tipiprogramma d on a.idtipoprogramma = d.idtipoprogramma left join tipigg c on a.idtipogg = c.idtipogg where idprogramma =" & IdProgramma
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            lblTitoloProgramma.Text = "Programma :" & " " & dtrgenerico("Titolo")
            txtTitoloProgramma.Text = dtrgenerico("Titolo")
            lblcodiceentePropo.Text = dtrgenerico("Codiceregione")
            lblDenominazioneEnte.Text = dtrgenerico("Denominazione")
            IdAmbitoSelezionato.Value = dtrgenerico("idambitoazione")
            ddlTerritorio.SelectedIndex = dtrgenerico("idTerritorio")
            ddlTipoProgramma.SelectedIndex = dtrgenerico("idtipoprogramma")
            If dtrgenerico("macrotipoprogramma") = "GG" Then
                DivProgrammiGG.Visible = True
            Else
                DivProgrammiGG.Visible = False
            End If
            ddlRegioneGG.SelectedValue = dtrgenerico("regionegg")
            intIdTipoGG = dtrgenerico("idtipogg")
            strRegione = dtrgenerico("regionegg")


            If dtrgenerico("Coprogrammazione") = "True" Then
                chkCoProgrammato.Checked = True
            Else
                chkCoProgrammato.Checked = False
            End If
            If IsDBNull(dtrgenerico("NPostiVol")) = False Then
                txttotvol1.Text = dtrgenerico("NPostiVol")
            Else
                txttotvol1.Text = "0"
            End If
            If IsDBNull(dtrgenerico("NMesi")) = False Then
                txtdurataProgramma.Text = dtrgenerico("NMesi")
            Else
                txtdurataProgramma.Text = "0"
            End If

            If dtrgenerico("Reti") = True Then
                ddlReti.SelectedIndex = 2
            Else
                ddlReti.SelectedIndex = 1
            End If
        End If
        ChiudiDataReader(dtrgenerico)
        CaricaComboMisureGG(strRegione)
        ddlMisuraGG.SelectedValue = intIdTipoGG

        Dim SqlCmd As New SqlClient.SqlCommand

        Try
            SqlCmd.CommandText = "SP_PROGRAMMI_CONTATORI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdProgramma ", SqlDbType.Int).Value = IdProgramma

            SqlCmd.Parameters.Add("@TOTPROG", SqlDbType.TinyInt)

            SqlCmd.Parameters("@TOTPROG").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@GMO", SqlDbType.TinyInt)

            SqlCmd.Parameters("@GMO").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@UE", SqlDbType.TinyInt)

            SqlCmd.Parameters("@UE").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@TUTO", SqlDbType.TinyInt)

            SqlCmd.Parameters("@TUTO").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@GMOUE", SqlDbType.TinyInt)

            SqlCmd.Parameters("@GMOUE").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@GMOTUTO", SqlDbType.TinyInt)

            SqlCmd.Parameters("@GMOTUTO").Direction = ParameterDirection.Output
            SqlCmd.ExecuteNonQuery()


            txtPogetGMO1.Text = SqlCmd.Parameters("@GMO").Value()
            txtPogetUE1.Text = SqlCmd.Parameters("@UE").Value()
            txtProgetTUTO1.Text = SqlCmd.Parameters("@TUTO").Value()
            txtProgetGMOUE1.Text = SqlCmd.Parameters("@GMOUE").Value()
            txtProgetGMOTUTO1.Text = SqlCmd.Parameters("@GMOTUTO").Value()
            txttotprogetti.Text = SqlCmd.Parameters("@TOTPROG").Value()
            'ESITO = SqlCmd.Parameters("@Esito").Value()
            'If ESITO = 0 Then

            'End If
        Catch ex As Exception
            lblerrore.Text = ex.Message
        Finally

        End Try

    End Sub
    Protected Sub OnCheckChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim selezionato As Integer
        Dim strappo As String

        strappo = sender.clientid
        strappo = StrReverse(strappo)
        strappo = Left(strappo, InStr(strappo, "_") - 1)
        strappo = StrReverse(strappo)
        selezionato = strappo
        selezionato = selezionato
        For Each Indice In dtgAmbitodiAzione.Items
            Dim check As CheckBox = DirectCast(Indice.FindControl("chkSeleziona"), CheckBox)
            If Indice.ItemIndex = selezionato Then
                check.Checked = True
                IdAmbitoSelezionato.Value = dtgAmbitodiAzione.Items(Indice.ItemIndex).Cells(0).Text
                'Coloro selezione
                dtgAmbitodiAzione.Items(Indice.ItemIndex).ForeColor = Black
                dtgAmbitodiAzione.Items(Indice.ItemIndex).Font.Bold = True
                dtgAmbitodiAzione.Items(Indice.ItemIndex).BackColor = LightGray
            Else
                'decoloro selezione
                dtgAmbitodiAzione.Items(Indice.ItemIndex).ForeColor = Black
                dtgAmbitodiAzione.Items(Indice.ItemIndex).Font.Bold = False
                dtgAmbitodiAzione.Items(Indice.ItemIndex).BackColor = White
                check.Checked = False
            End If

        Next


    End Sub
    Protected Sub imgProgettiProgramma_Click(sender As Object, e As EventArgs) Handles imgProgettiProgramma.Click
        Response.Redirect("WfrmProgrammi_AssociaRimuovi.aspx?idProgramma=" & CInt(IdProgramma.Value))
    End Sub
    Private Sub imgElencoDocumentiProg_Click(sender As Object, e As System.EventArgs) Handles imgElencoDocumentiProg.Click
        Response.Redirect("wfrmProgrammiDocumenti.aspx?idProgramma=" & CInt(IdProgramma.Value))
    End Sub
    Private Sub imgCoProgrammazione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCoProgrammazione.Click
        Response.Redirect("WfrmProgrammiCoprogrammazione.aspx?idProgramma=" & CInt(IdProgramma.Value))
    End Sub
    Private Sub BloccaMaschera(ByVal strmessaggio As String)
        txtTitoloProgramma.Enabled = False
        ddlTerritorio.Enabled = False
        ddlTipoProgramma.Enabled = False
        ddlRegioneGG.Enabled = False
        ddlMisuraGG.Enabled = False
        ddlReti.Enabled = False
        cmdSalva.Visible = False
        dtgObiettivi.Columns(1).Visible = False
        dtgAmbitodiAzione.Columns(1).Visible = False
        lblerrore.Text = strmessaggio
        cmdElimina.Visible = False
    End Sub
    Private Sub dgElencoProgettiProgramma_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgElencoProgettiProgramma.ItemCommand
        Select Case e.CommandName
            Case "Select"
                If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then
                    Response.Redirect("TabProgetti2020.aspx?IdProgramma=" & CInt(e.Item.Cells(1).Text) & "&Nazionale=" & CInt(e.Item.Cells(7).Text) & "&IdAttivita=" & CInt(e.Item.Cells(0).Text) & "&Modifica=1")
                Else
                    Response.Redirect("TabProgetti2020.aspx?IdProgramma=" & CInt(e.Item.Cells(1).Text) & "&Nazionale=" & CInt(e.Item.Cells(7).Text) & "&IdAttivita=" & CInt(e.Item.Cells(0).Text) & "")
                End If

        End Select
    End Sub
    Protected Sub OnCheckChanged1(ByVal sender As Object, ByVal e As EventArgs)
        MaintainScrollPositionOnPostBack = True

        Dim selezionato As Integer
        Dim strappo As String

        strappo = sender.clientid
        strappo = StrReverse(strappo)
        strappo = Left(strappo, InStr(strappo, "_") - 1)
        strappo = StrReverse(strappo)
        selezionato = strappo
        selezionato = selezionato
        For Each Indice In dtgObiettivi.Items
            Dim check As CheckBox = DirectCast(Indice.FindControl("chkSeleziona"), CheckBox)
            If Indice.ItemIndex = selezionato Then
                If check.Checked = True Then
                    dtgObiettivi.Items(Indice.ItemIndex).ForeColor = Black
                    dtgObiettivi.Items(Indice.ItemIndex).Font.Bold = True
                    dtgObiettivi.Items(Indice.ItemIndex).BackColor = LightGray
                Else
                    dtgObiettivi.Items(Indice.ItemIndex).ForeColor = Black
                    dtgObiettivi.Items(Indice.ItemIndex).Font.Bold = False
                    dtgObiettivi.Items(Indice.ItemIndex).BackColor = White
                    check.Checked = False
                End If
            End If
        Next
    End Sub
    Protected Sub cmdElimina_Click(sender As Object, e As EventArgs) Handles cmdElimina.Click
        Dim MySqlCommand As New SqlClient.SqlCommand
        Dim ESITO As Integer
        If IdProgramma.Value <> "" Then
            Try
                MySqlCommand.CommandText = "SP_PROGRAMMI_Elimina"
                MySqlCommand.CommandType = CommandType.StoredProcedure
                MySqlCommand.Connection = Session("Conn")
                MySqlCommand.Parameters.Add("@IdProgramma ", SqlDbType.Int).Value = IdProgramma.Value
                MySqlCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
                MySqlCommand.Parameters.Add("@Esito", SqlDbType.TinyInt)
                MySqlCommand.Parameters("@Esito").Size = 10
                MySqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output
                MySqlCommand.Parameters.Add("@messaggio", SqlDbType.VarChar)
                MySqlCommand.Parameters("@messaggio").Size = 1000
                MySqlCommand.Parameters("@messaggio").Direction = ParameterDirection.Output
                MySqlCommand.ExecuteNonQuery()
                ESITO = MySqlCommand.Parameters("@Esito").Value()
                If ESITO = 0 Then
                    lblerrore.Text = MySqlCommand.Parameters("@Messaggio").Value
                Else
                    BloccaMaschera("")
                    IdProgramma.Value = ""
                    idlinkalti.Visible = False
                    divCoProgrammazione.Visible = False
                    dgElencoProgettiProgramma.Columns(6).Visible = False
                    lblMessaggioConferma.Text = MySqlCommand.Parameters("@Messaggio").Value
                End If
                If Not MySqlCommand Is Nothing Then
                    MySqlCommand = Nothing
                End If
            Catch ex As Exception
                lblerrore.Text = ex.Message
            Finally

            End Try
        End If
        MaintainScrollPositionOnPostBack = False
    End Sub



    Private Sub ddlRegioneGG_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlRegioneGG.SelectedIndexChanged
        Dim strSql1 As String
        If ddlRegioneGG.SelectedValue = "" Then
            CaricaComboMisureGG("vuota")
        Else
            CaricaComboMisureGG(ddlRegioneGG.SelectedValue)
        End If
    End Sub

    Protected Sub ddlTipoProgramma_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoProgramma.SelectedIndexChanged
        Dim strSql1 As String
        If ddlTipoProgramma.SelectedValue = 0 Then
            DivProgrammiGG.Visible = False
        Else
            strSql1 = "SELECT macrotipoprogramma from tipiprogramma where idtipoprogramma = " & ddlTipoProgramma.SelectedValue
            dtrgenerico = ClsServer.CreaDatareader(strSql1, Session("conn"))
            dtrgenerico.Read()
            If dtrgenerico.HasRows = True Then
                If dtrgenerico("macrotipoprogramma") = "GG" Then
                    DivProgrammiGG.Visible = True
                Else
                    DivProgrammiGG.Visible = False
                End If
            End If

            ChiudiDataReader(dtrgenerico)
        End If
    End Sub
End Class