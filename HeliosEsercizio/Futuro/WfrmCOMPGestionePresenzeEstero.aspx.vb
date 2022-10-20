Imports System.Data.SqlClient
Imports System.IO
Public Class WfrmCOMPGestionePresenzeEstero
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtsGenerico As DataSet
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim SqlCmd As New SqlClient.SqlCommand
    Dim mioAdapter As SqlClient.SqlDataAdapter
    Dim mytable As DataTable
    Dim Reader As SqlClient.SqlDataReader
    Public Shared applicagg As String
    Public Shared IdEnteCliccato As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If



        If IsPostBack = False Then
            If ClsUtility.ForzaCaricamentoPaghe(Session("Utente"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            CaricaComboEstero()
            If Request.QueryString("VadoA") = "ESTERO" Then


                'posizionare combo mensilità
                cboAnnoMeseEstero.SelectedValue = Request.QueryString("Mensilita")
                'fare il caricaGriglia per enti
                CaricaGriglia()
                'fare il caricagriglia per mensilita e ente 

                Dim sqlDAP As New SqlClient.SqlDataAdapter
                Dim dataSet As New DataSet
                Dim strNomeStore As String = "[SP_COMP_VOLONTARI_ESTERO]"

                'dgvolontariestero.CurrentPageIndex = 0
                Try
                    sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
                    sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
                    sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Request.QueryString("IdEnte")
                    sqlDAP.SelectCommand.Parameters.Add("@Mensilità", SqlDbType.Char).Value = cboAnnoMeseEstero.SelectedValue

                    sqlDAP.Fill(dataSet)

                Catch ex As Exception
                    Response.Write(ex.Message.ToString())
                    Exit Sub
                End Try

                dgvolontariestero.DataSource = dataSet

                dgvolontariestero.DataBind()
                CmdSalva.Visible = True
                CmdSalva1.Visible = True
                CmdCompleta.Visible = True
                CmdCompleta1.Visible = True
                lblApplica.Visible = True
                txtApplica.Visible = True
                CmdApplica.Visible = True
                If dgvolontariestero.Items.Count = 0 Then

                    lblMessaggio1.Text = "Nessun Dato estratto."
                   
                Else
                    lblMessaggio1.Text = "Risultato Ricerca Volontari Estero."

                End If


            End If

        Else
           

            lblmess.Visible = False
            lblmess.Text = ""

        End If
    End Sub
    Private Sub CaricaComboEstero()


        Try

            SqlCmd = New SqlClient.SqlCommand("SP_COMP_MESI_ESTERO", Session("Conn"))
            SqlCmd.CommandType = CommandType.StoredProcedure
            mioAdapter = New SqlClient.SqlDataAdapter(SqlCmd)
            mytable = New DataTable()
            mioAdapter.Fill(mytable)

            Reader = SqlCmd.ExecuteReader()

            cboAnnoMeseEstero.DataTextField = "Mensilità"
            cboAnnoMeseEstero.DataValueField = "Mensilità"
            cboAnnoMeseEstero.DataSource = mytable
            cboAnnoMeseEstero.DataBind()
            ChiudiDataReader(Reader)
            
            SqlCmd.Dispose()


          

        Catch ex As Exception
            lblmess.Visible = True
            lblmess.Text = ex.Message
        End Try
    End Sub
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Protected Sub CmdDettaglio_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles CmdDettaglio.Click
        filtriVolontari.Visible = False

        CaricaGriglia()
    End Sub
    'Private Function DTDisconnesso(ByVal IdEnte As Integer, ByVal Mensilità As String) As DataTable
    '    Dim sqlDAP As New SqlClient.SqlDataAdapter
    '    Dim strNomeStore As String = "[SP_COMP_VOLONTARI_ESTERO]"
    '    Dim dataTB As New DataTable

    '    Try
    '        sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
    '        sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
    '        sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
    '        sqlDAP.SelectCommand.Parameters.Add("@Mensilità", SqlDbType.Char).Value = Mensilità
    '        sqlDAP.Fill(dataTB)
    '        Return dataTB
    '        Session("mioDataTableVolEst") = dataTB
    '    Catch ex As Exception
    '        Response.Write(ex.Message.ToString())
    '        Exit Function
    '    End Try
    'End Function
    Private Sub dgElencoEnti_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgElencoEnti.ItemCommand
        If e.CommandName = "Seleziona" Then
            IdEnteCliccato = CInt(e.Item.Cells(1).Text)
            Dim sqlDAP As New SqlClient.SqlDataAdapter
            Dim dataSet As New DataSet
            Dim strNomeStore As String = "[SP_COMP_VOLONTARI_ESTERO]"
           
            'dgvolontariestero.CurrentPageIndex = 0
            Try
                sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
                sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
                sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = CInt(e.Item.Cells(1).Text)
                sqlDAP.SelectCommand.Parameters.Add("@Mensilità", SqlDbType.Char).Value = cboAnnoMeseEstero.SelectedValue

                sqlDAP.Fill(dataSet)
                'Return dataSet
            Catch ex As Exception
                Response.Write(ex.Message.ToString())
                Exit Sub
            End Try
            Session("mioDataSet") = dataSet
            dgvolontariestero.DataSource = dataSet
            '
            dgvolontariestero.DataBind()
            CmdSalva.Visible = True
            CmdSalva1.Visible = True
            CmdCompleta.Visible = True
            CmdCompleta1.Visible = True
            lblApplica.Visible = True
            txtApplica.Visible = True
            CmdApplica.Visible = True
            filtriVolontari.Visible = True
            If dgvolontariestero.Items.Count = 0 Then

                lblMessaggio1.Text = "Nessun Dato estratto."

            Else
                lblMessaggio1.Text = "Risultato Ricerca Volontari Estero."

            End If
        End If
        'DTDisconnesso(CInt(e.Item.Cells(1).Text), cboAnnoMeseEstero.SelectedValue)

    End Sub

    Private Sub dgElencoEnti_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgElencoEnti.PageIndexChanged
        dgElencoEnti.CurrentPageIndex = e.NewPageIndex
        dgElencoEnti.DataSource = Session("appDtsElencoEnti")
        dgElencoEnti.DataBind()
        dgElencoEnti.SelectedIndex = -1

    End Sub
    Private Sub CaricaGriglia()
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_COMP_ENTI_ESTERO]"
       
        dgElencoEnti.CurrentPageIndex = 0
        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@Mensilità", SqlDbType.Char).Value = cboAnnoMeseEstero.SelectedValue
            If CboConfermaAssenze.SelectedValue <> "Selezionare" Then
                sqlDAP.SelectCommand.Parameters.Add("@ConfermaAssenzeSINO", SqlDbType.VarChar).Value = CboConfermaAssenze.SelectedValue
            End If
            If CboCompletato.SelectedValue <> "Selezionare" Then
                sqlDAP.SelectCommand.Parameters.Add("@CompletatoSINO", SqlDbType.VarChar).Value = CboCompletato.SelectedValue
            End If

            sqlDAP.Fill(dataSet)
            'Return dataSet
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try

        dgElencoEnti.DataSource = dataSet
        Session("appDtsElencoEnti") = dataSet
        dgElencoEnti.DataBind()
        If dgElencoEnti.Items.Count = 0 Then

            lblMessaggio.Text = "Nessun Dato estratto."

        Else
            lblMessaggio.Text = "Risultato Ricerca Enti."

            Dim dd As Integer

            For dd = 0 To dataSet.Tables(0).Rows.Count

            Next

            '*********************************************************************************
            'blocco per la creazione della datatable per la stampa della ricerca

            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(5) As String
            Dim NomiCampiColonne(5) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "COD.ENTE"
            NomeColonne(1) = "DENOMINAZIONE"
            NomeColonne(2) = "TOT.VOL"
            NomeColonne(3) = "TOT.VOL LAVORATI"
            NomeColonne(4) = "CONFERMA ASSENZE SI/NO"
            NomeColonne(5) = "COMPLETATO SI/NO"



            NomiCampiColonne(0) = "CodiceEnte"
            NomiCampiColonne(1) = "Denominazione"
            NomiCampiColonne(2) = "TotaleVolontari"
            NomiCampiColonne(3) = "TotaleVolontariLavorati"
            NomiCampiColonne(4) = "ConfermaAssenze"
            NomiCampiColonne(5) = "Completato"


            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            CaricaDataTablePerStampa(dataSet, 5, NomeColonne, NomiCampiColonne)

        End If
        CmdEsporta.Visible = True
        'imgStampa.Visible = True
        ' CaricaDataGrid(dgElencoEnti)
    End Sub
    Private Sub dgvolontariestero_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgvolontariestero.ItemCommand
        If e.CommandName = "Vai" Then
            Response.Redirect("WfrmCOMPRimborsiDecurtazioni.aspx?IdVol=" + e.Item.Cells(0).Text + "&IdAttivita=" & e.Item.Cells(2).Text + "&IdEnte=" & e.Item.Cells(1).Text + "&Mensilita=" & cboAnnoMeseEstero.SelectedValue + "&VengoDa=ESTERO")
        End If
        'If e.CommandName = "Applica" Then
        '    Dim selezionato As Integer
        '    selezionato = e.Item.ItemIndex
        '    Dim ValoreDaApplicare As String

        '    If txtApplica1.Value <> "" Then
        '        ValoreDaApplicare = txtApplica1.Value
        '    Else
        '        ValoreDaApplicare = e.Item.Cells(12).Text
        '    End If

        '    'update tutti i campi a null 

        '    Dim item As DataGridItem
        '    For Each item In dgvolontariestero.Items
        '        Dim immagine As ImageButton = DirectCast(item.FindControl("ImgBtnApplica"), ImageButton)
        '        Dim text As TextBox = DirectCast(item.FindControl("txtgiorniestero"), TextBox)
        '        'If item.ItemIndex = selezionato Then
        '        'immagine.Visible = False
        '        'End If
        '        If text.Text = "" Then

        '            'dgvolontariestero.Items(item.ItemIndex).Cells(12).Text = ValoreDaApplicare
        '            text.Text = ValoreDaApplicare
        '            'immagine.Visible = False
        '        End If
        '    Next

        'End If

    End Sub
    Protected Sub CmdSalva_Click(sender As Object, e As EventArgs) Handles CmdSalva.Click
        EseguiStoreSalva()
        txtApplica.Text = ""
        CaricaGriglia()
    End Sub
    Protected Sub CmdSalva1_Click(sender As Object, e As EventArgs) Handles CmdSalva1.Click
        EseguiStoreSalva()
        txtApplica.Text = ""
        CaricaGriglia()
    End Sub
    Protected Sub CmdCompleta_Click(sender As Object, e As EventArgs) Handles CmdCompleta.Click
        EseguiStoreSalva()
        lblmess.Visible = True
        lblmess.Text = EseguiStoreCOMPLETA()
        txtApplica.Text = ""
        CaricaGriglia()
    End Sub
    Protected Sub CmdCompleta1_Click(sender As Object, e As EventArgs) Handles CmdCompleta1.Click
        EseguiStoreSalva()
        lblmess.Visible = True
        lblmess.Text = EseguiStoreCOMPLETA()
        txtApplica.Text = ""
        CaricaGriglia()
    End Sub
    Private Sub cboAnnoMeseEstero_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboAnnoMeseEstero.SelectedIndexChanged
        dgvolontariestero.DataSource = Nothing
        dgvolontariestero.DataBind()
        CmdSalva.Visible = False
        CmdSalva1.Visible = False
        CmdCompleta.Visible = False
        If dgvolontariestero.Items.Count = 0 Then

            lblmessaggio1.Text = "Nessun Dato estratto."

        Else
            lblmessaggio1.Text = "Risultato Ricerca Volontari Estero."

        End If
        CmdCompleta1.Visible = False
        lblApplica.Visible = False
        txtApplica.Visible = False
        CmdApplica.Visible = False
    End Sub
    Protected Sub CmdApplica_Click(sender As Object, e As EventArgs) Handles CmdApplica.Click
        Dim ValoreDaApplicare As String = txtApplica.Text
        'update tutti i campi a null solo client

        For Each item In dgvolontariestero.Items
            Dim text As TextBox = DirectCast(item.FindControl("txtgiorniestero"), TextBox)
            If text.Text = "" Then
                text.Text = ValoreDaApplicare
            End If
        Next
    End Sub
    Private Function EseguiStoreSalva() As String
        'AUTORE: Antonello

        Try
            Dim sReturnValue As String
            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandTimeout = 300
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_COMP_SALVA_PERIODOESTERO"
            MyCommand.Connection = Session("conn")



            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdEntità"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Mensilità"
            sparam1.Size = 10
            sparam1.SqlDbType = SqlDbType.VarChar
            MyCommand.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Giorni"
            sparam2.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam2)


            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@UsernameRichiesta"
            sparam3.Size = 10
            sparam3.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam3)

            'OUTPUT
            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@Esito"
            sparam4.Size = 255
            sparam4.SqlDbType = SqlDbType.TinyInt
            sparam4.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam4)

            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@messaggio"
            sparam5.Size = 1000
            sparam5.SqlDbType = SqlDbType.NVarChar
            sparam5.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam5)
            Dim conta As Integer = 0
            For Each item In dgvolontariestero.Items

                Dim text As TextBox = DirectCast(item.FindControl("txtgiorniestero"), TextBox)

                Dim lblesito As Label = DirectCast(item.FindControl("lblEsito"), Label)

                MyCommand.Parameters("@IdEntità").Value = CInt(dgvolontariestero.Items(item.ItemIndex).Cells(0).Text)
                MyCommand.Parameters("@Mensilità").Value = dgvolontariestero.Items(item.ItemIndex).Cells(14).Text
                If text.Text <> "" Then
                    MyCommand.Parameters("@Giorni").Value = CInt(text.Text)
                    'Else
                    '    MyCommand.Parameters("@Giorni").Value = CInt(dgvolontariestero.Items(item.ItemIndex).Cells(12).Text)

                    MyCommand.Parameters("@UsernameRichiesta").Value = Session("Utente")
                    MyCommand.ExecuteNonQuery()
                    sReturnValue = MyCommand.Parameters("@messaggio").Value
                    If MyCommand.Parameters("@Esito").Value = 0 Then
                        lblesito.ForeColor = Drawing.Color.Red
                    Else
                        lblesito.ForeColor = Drawing.Color.Navy
                    End If
                    lblesito.Text = sReturnValue
                End If


                conta = conta + 1

            Next


            'sReturnValue = MyCommand.Parameters("@messaggio").Value



            Return sReturnValue

        Catch ex As Exception

            Return "ERRORE" & ex.Message
        End Try
    End Function
    Private Function EseguiStoreCOMPLETA() As String
        'AUTORE: Antonello

        Try
            Dim sReturnValue As String
            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandTimeout = 300
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_COMP_COMPLETA_PERIODOESTERO"
            MyCommand.Connection = Session("conn")



            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdEnte"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Mensilità"
            sparam1.Size = 10
            sparam1.SqlDbType = SqlDbType.VarChar
            MyCommand.Parameters.Add(sparam1)

           
            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@UsernameRichiesta"
            sparam3.Size = 10
            sparam3.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam3)

            'OUTPUT
            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@Esito"
            sparam4.Size = 255
            sparam4.SqlDbType = SqlDbType.TinyInt
            sparam4.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam4)

            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@messaggio"
            sparam5.Size = 1000
            sparam5.SqlDbType = SqlDbType.NVarChar
            sparam5.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam5)


            MyCommand.Parameters("@IdEnte").Value = CInt(dgvolontariestero.Items(0).Cells(1).Text)
            MyCommand.Parameters("@Mensilità").Value = dgvolontariestero.Items(0).Cells(14).Text
            MyCommand.Parameters("@UsernameRichiesta").Value = Session("Utente")
            MyCommand.ExecuteNonQuery()




            sReturnValue = MyCommand.Parameters("@messaggio").Value



            Return sReturnValue

        Catch ex As Exception

            Return "ERRORE" & ex.Message
        End Try
    End Function

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        CaricaGriglia()
    End Sub

    Protected Sub CmdRicercaVol_Click(sender As Object, e As EventArgs) Handles CmdRicercaVol.Click
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_COMP_VOLONTARI_ESTERO]"

        'dgvolontariestero.CurrentPageIndex = 0
        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = CInt(IdEnteCliccato)
            sqlDAP.SelectCommand.Parameters.Add("@Mensilità", SqlDbType.Char).Value = cboAnnoMeseEstero.SelectedValue
            If txtCodiceVolontario.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = txtCodiceVolontario.Text
            End If
            If txtNominativo.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Nominativo", SqlDbType.VarChar).Value = txtNominativo.Text
            End If
            If txtCodiceProgetto.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Progetto", SqlDbType.VarChar).Value = txtCodiceProgetto.Text
            End If
            If txtFascia.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Fascia", SqlDbType.VarChar).Value = txtFascia.Text
            End If
            If CboVuoto.SelectedValue <> "Selezionare" Then
                sqlDAP.SelectCommand.Parameters.Add("@GiorniEsteroSINO", SqlDbType.VarChar).Value = CboVuoto.SelectedValue
            End If
            sqlDAP.Fill(dataSet)
            'Return dataSet
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
        Session("mioDataSet") = dataSet
        dgvolontariestero.DataSource = dataSet
        '
        dgvolontariestero.DataBind()
        CmdSalva.Visible = True
        CmdSalva1.Visible = True
        CmdCompleta.Visible = True
        CmdCompleta1.Visible = True
        lblApplica.Visible = True
        txtApplica.Visible = True
        CmdApplica.Visible = True
        filtriVolontari.Visible = True
        If dgvolontariestero.Items.Count = 0 Then

            lblmessaggio1.Text = "Nessun Dato estratto."

        Else
            lblmessaggio1.Text = "Risultato Ricerca Volontari Estero."

        End If
    End Sub

    'Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
    '    GridDaCaricare.DataSource = Session("appDtsElencoEnti")
    '    GridDaCaricare.DataBind()

    '    Dim dd As Integer

    '    For dd = 0 To dtsGenerico.Tables(0).Rows.Count

    '    Next

    '    '*********************************************************************************
    '    'blocco per la creazione della datatable per la stampa della ricerca

    '    'nome e posizione di lettura delle colopnne a base 0
    '    Dim NomeColonne(14) As String
    '    Dim NomiCampiColonne(5) As String
    '    'nome della colonna 
    '    'e posizione nella griglia di lettura
    '    NomeColonne(0) = "CodiceEnte"
    '    NomeColonne(1) = "Denominazione"
    '    NomeColonne(2) = "TotaleVolontari"
    '    NomeColonne(3) = "TotaleVolontariLavorati"
    '    NomeColonne(4) = "ConfermaAssenze"
    '    NomeColonne(5) = "Completato"



    '    NomiCampiColonne(0) = "Codice Ente"
    '    NomiCampiColonne(1) = "Denominazione"
    '    NomiCampiColonne(2) = "Totale Volontari"
    '    NomiCampiColonne(3) = "Totale Volontari Lavorati"
    '    NomiCampiColonne(4) = "Conferma Assenze Si/No"
    '    NomiCampiColonne(5) = "Completato Si/No"


    '    'carico un datatable che userò poi nella pagina di stampa
    '    'il numero delle colonne è a base 0
    '    CaricaDataTablePerStampa(dtsGenerico, 5, NomeColonne, NomiCampiColonne)

    '    '*********************************************************************************

    '    GridDaCaricare.Visible = True
    '    If GridDaCaricare.Items.Count = 0 Then
    '        'GridDaCaricare.Visible = False
    '        lblMessaggio.Text = "Nessun Dato estratto."
    '        'ApriCSV1.Visible = False
    '        CmdEsporta.Visible = False
    '    Else
    '        lblMessaggio.Text = "Risultato Ricerca Elenco Enti."
    '        'ApriCSV1.Visible = True
    '        CmdEsporta.Visible = True
    '    End If
    '    'ControllahttpEmailVerificato()
    'End Sub
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
        Dim stampa As String
        stampa = Request.Params("Stampa")

        'dgRisultatoRicerca.DataSource = Session("DtbRicerca")
        'lblNumOcc.Text = CType(Session("DtbRicerca"), DataTable).Rows.Count.ToString("###,##0")
        'dgRisultatoRicerca.DataBind()


    End Sub
    Function StampaCSV(ByVal DTBRicerca As DataTable)

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            'lblmessaggio.Text = lblmessaggio.Text & "La ricerca non ha prodotto nessun risultato."
            'lblStampa.Visible = False
            'hlCSVRicerca.Visible = False
            ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
            'Creazione dell'inntestazione del CSV
            Dim intNumCol As Int64 = DTBRicerca.Columns.Count
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            Writer.WriteLine(xLinea)
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

                Writer.WriteLine(xLinea)
                xLinea = vbNullString

            Next


            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function
    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        StampaCSV(Session("DtbRicerca"))
    End Sub
End Class