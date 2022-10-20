Imports System.Data.SqlClient

Public Class WFrmAssociaEntiSettori
    Inherits System.Web.UI.Page
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String
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
    Private Sub CancellaMessaggiInfo()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Request.QueryString("Id") <> "" Then

            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1


            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, strENTITA, Request.QueryString("Id")) = 1 Then
                ChiudiDataReader(dtrgenerico)
            Else
              ChiudiDataReader(dtrgenerico)
                Response.Redirect("wfrmAnomaliaDati.aspx")

            End If

            If Page.IsPostBack = False Then
                If Request.QueryString("Blocco") = "FALSE" Then
                    CaricaSettori()
                    CaricaEntiSettori()
                Else
                    CaricaSettori()
                    CaricaEntiSettori()
                    msgInfo.Text = "Maschera in sola visualizazione. Le modifiche apportate non verranno salvate"
                    msgConferma.Visible = False
                End If
            Else

            End If
        Else
            If Request.QueryString("Id2") <> "" And Request.QueryString("Id2") <> "-1" Then

                Dim strATTIVITA As Integer = -1
                Dim strBANDOATTIVITA As Integer = -1
                Dim strENTEPERSONALE As Integer = -1
                Dim strENTITA As Integer = -1
                Dim strIDENTE As Integer = -1


                If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, strENTITA, Request.QueryString("Id2")) = 1 Then
                    ChiudiDataReader(dtrgenerico)
                Else
                    ChiudiDataReader(dtrgenerico)
                    Response.Redirect("wfrmAnomaliaDati.aspx")
                End If

                If Page.IsPostBack = False Then
                    If Request.QueryString("Blocco") = "FALSE" Then
                        CaricaSettori()
                        CaricaEntiSettori()
                    Else
                        CaricaSettori()
                        CaricaEntiSettori()
                        msgInfo.Text = "Maschera in sola visualizazione. Le modifiche apportate non verranno salvate"
                        cmdConferma.Visible = False
                    End If
                Else

                End If


            Else
                msgErrore.Text = "&#200; Necessario Salvare prima L'Ente in Accordo per poter inserire i Settori di Intervento"
                msgConferma.Visible = False
            End If
        End If
    End Sub
    Function CancellaRecord() As ICollection
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add()
        dt.Columns.Add(New DataColumn("IDMacroAmbitoAttività", GetType(String)))
        dt.Columns.Add(New DataColumn("MacroAmbitoAttività", GetType(String)))

        Dim x As Integer
        If dtgAmbitiSelezionati.Items.Count = 0 Or dtgAmbitiSelezionati.Items.Count = 1 Then
            dt = Nothing
        Else
            For x = 1 To dtgAmbitiSelezionati.Items.Count
                If dtgAmbitiSelezionati.SelectedIndex <> x - 1 Then
                    dr = dt.NewRow()
                    dr(1) = dtgAmbitiSelezionati.Items(x - 1).Cells(1).Text
                    dr(2) = dtgAmbitiSelezionati.Items(x - 1).Cells(2).Text
                    dt.Rows.Add(dr)
                Else
                End If
            Next
        End If
        Dim dv As New DataView(dt)
        Return dv
    End Function
    Private Sub dtgAmbiti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgAmbiti.SelectedIndexChanged
        dtgAmbitiSelezionati.DataSource = CreateDataSource()
        dtgAmbitiSelezionati.DataBind()
    End Sub
    Private Sub dtgAmbitiSelezionati_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgAmbitiSelezionati.SelectedIndexChanged
        If dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "Nessun Settore." And dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "&nbsp;" Then
            dtgAmbitiSelezionati.DataSource = CancellaRecord()
            dtgAmbitiSelezionati.DataBind()
            dtgAmbitiSelezionati.SelectedIndex = -1
        End If
    End Sub

    Private Sub imgConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConferma.Click
        Dim x As Integer
        Dim Strsql As String
        Dim myCommand As SqlClient.SqlCommand

        Dim strIDSettori As String = ""
        If Session("TipoUtente") = "E" Then
            If dtgAmbitiSelezionati.Items.Count > 0 Then
                For x = 1 To dtgAmbitiSelezionati.Items.Count
                    strIDSettori = strIDSettori & dtgAmbitiSelezionati.Items(x - 1).Cells(1).Text & ";"
                Next
            End If
            'Richiamo store per storicizzazione Settori
            LoadStoreEntiVariazioniSettori(Request.QueryString("Id"), strIDSettori)
        End If

        If Request.QueryString("Id") <> "" Then
            Strsql = "Delete from EntiSettori Where IdEnte=" & Request.QueryString("Id")

        Else
            Strsql = "Delete from EntiSettori Where IdEnte=" & Request.QueryString("Id2")
        End If

        myCommand = New SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        myCommand.CommandText = Strsql
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()

        If dtgAmbitiSelezionati.Items.Count > 0 Then
            Try
                For x = 1 To dtgAmbitiSelezionati.Items.Count
                    Dim myCommand1 As SqlClient.SqlCommand
                    myCommand1 = New SqlClient.SqlCommand
                    myCommand1.Connection = Session("conn")
                    If Request.QueryString("Id") <> "" Then
                        Strsql = "Insert into EntiSettori (idente,idMacroAmbitoAttività,UsernameInserimento,DataInserimento) Values " & _
                                                     "(" & Request.QueryString("Id") & "," & dtgAmbitiSelezionati.Items(x - 1).Cells(1).Text & ",'" & Session("Utente") & "',getdate()) "
                    Else
                        Strsql = "Insert into EntiSettori (idente,idMacroAmbitoAttività,UsernameInserimento,DataInserimento) Values " & _
                                 "(" & Request.QueryString("Id2") & "," & dtgAmbitiSelezionati.Items(x - 1).Cells(1).Text & ",'" & Session("Utente") & "',getdate()) "

                    End If

                    myCommand1.CommandText = Strsql
                    myCommand1.ExecuteNonQuery()
                    myCommand1.Dispose()


                Next

                msgErrore.Text = "AGGIORNAMENTO EFFETTUATO."
                msgErrore.ForeColor = Drawing.Color.Black
                msgInfo.Font.Bold = True
                cmdConferma.Visible = False
            Catch ex As Exception
                msgErrore.Text = "Si è verificato un errore durante l'operazione di conferma. Contattare l'assistenza."
                cmdConferma.Visible = False
            End Try
           
            'Server.Transfer("WfrmAlbero.aspx")
        End If
    End Sub
    Function CreateDataSource() As ICollection
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim blnCheck As Boolean = False

        dt.Columns.Add()
        dt.Columns.Add(New DataColumn("IDMacroAmbitoAttività", GetType(String)))
        dt.Columns.Add(New DataColumn("MacroAmbitoAttività", GetType(String)))


        Dim i As Integer
        Dim x As Integer

        If Not dtgAmbiti.SelectedItem Is Nothing Then
            If dtgAmbitiSelezionati.Items.Count > 0 Then
                If dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "Nessun Settore." And dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "&nbsp;" Then
                    For x = 1 To dtgAmbitiSelezionati.Items.Count
                        If (dtgAmbitiSelezionati.Items(x - 1).Cells(2).Text = dtgAmbiti.SelectedItem.Cells(2).Text) Then
                            blnCheck = True
                            Exit For
                        End If
                    Next
                    If blnCheck = False Then
                        For i = 1 To dtgAmbitiSelezionati.Items.Count
                            dr = dt.NewRow()
                            dr(1) = dtgAmbitiSelezionati.Items(i - 1).Cells(1).Text
                            dr(2) = dtgAmbitiSelezionati.Items(i - 1).Cells(2).Text

                            dt.Rows.Add(dr)
                        Next

                        dr = dt.NewRow()
                        dr(1) = dtgAmbiti.SelectedItem.Cells(1).Text
                        dr(2) = dtgAmbiti.SelectedItem.Cells(2).Text

                        dt.Rows.Add(dr)



                        Dim dvII As New DataView(dt)

                        Return dvII
                    Else
                        For i = 1 To dtgAmbitiSelezionati.Items.Count
                            dr = dt.NewRow()
                            dr(1) = dtgAmbitiSelezionati.Items(i - 1).Cells(1).Text
                            dr(2) = dtgAmbitiSelezionati.Items(i - 1).Cells(2).Text

                            dt.Rows.Add(dr)
                        Next
                        Dim dvII As New DataView(dt)

                        Return dvII
                    End If
                Else
                    dr = dt.NewRow()
                    dr(1) = dtgAmbiti.SelectedItem.Cells(1).Text
                    dr(2) = dtgAmbiti.SelectedItem.Cells(2).Text

                    dt.Rows.Add(dr)

                    Dim dvII As New DataView(dt)

                    Return dvII
                End If

                For i = 1 To dtgAmbitiSelezionati.Items.Count
                    dr = dt.NewRow()
                    dr(1) = dtgAmbitiSelezionati.Items(i - 1).Cells(1).Text
                    dr(2) = dtgAmbitiSelezionati.Items(i - 1).Cells(2).Text

                    dt.Rows.Add(dr)
                Next

                dr = dt.NewRow()
                dr(1) = dtgAmbiti.SelectedItem.Cells(1).Text
                dr(2) = dtgAmbiti.SelectedItem.Cells(2).Text

                dt.Rows.Add(dr)



            Else
                dr = dt.NewRow()
                dr(1) = dtgAmbiti.SelectedItem.Cells(1).Text
                dr(2) = dtgAmbiti.SelectedItem.Cells(2).Text

                dt.Rows.Add(dr)

            End If
        Else

        End If

        Dim dv As New DataView(dt)

        Return dv
    End Function

    Sub CaricaSettori()
        Dim dtsSettori As DataSet
        'variabile stringa locale per costruire la query per le aree
        Dim AlboEnte As String
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

        Dim strSql As String
        'preparo la query per i settori
        strSql = "select IDMacroAmbitoAttività, Codifica, MacroAmbitoAttività, IDIperAmbitoAttività FROM macroambitiattività"
        If AlboEnte = "" Then
            strSql &= " WHERE (ALBO='SCU' OR ALBO IS NULL) "
        Else
            strSql &= " WHERE (ALBO='" & AlboEnte & "' OR ALBO IS NULL) "
        End If

        'eseguo la query e passo il risultato al datareader
        dtsSettori = ClsServer.DataSetGenerico(strSql, Session("conn"))
        'controllo se ci sono dei record
        If dtsSettori.Tables(0).Rows.Count > 0 Then
            'al datasource sella combo passo il datareader
            dtgAmbiti.DataSource = dtsSettori
            If Request.QueryString("blocco") = "TRUE" Then
                dtgAmbiti.Columns(0).Visible = False
            End If
            dtgAmbiti.DataBind()
        End If
    End Sub
    Sub CaricaEntiSettori()
        Dim dtsEntiSettori As DataSet
        'variabile stringa locale per costruire la query per le aree
        Dim strSql As String
        'preparo la query per i settori
        If Request.QueryString("Id") <> "" Then
            strSql = "SELECT macroambitiattività.IDMacroAmbitoAttività, macroambitiattività.MacroAmbitoAttività FROM macroambitiattività INNER JOIN EntiSettori ON macroambitiattività.IDMacroAmbitoAttività = EntiSettori.IdMacroAmbitoAttività WHERE EntiSettori.IdEnte = " & Request.QueryString("Id")

        Else
            strSql = "SELECT macroambitiattività.IDMacroAmbitoAttività, macroambitiattività.MacroAmbitoAttività FROM macroambitiattività INNER JOIN EntiSettori ON macroambitiattività.IDMacroAmbitoAttività = EntiSettori.IdMacroAmbitoAttività WHERE EntiSettori.IdEnte = " & Request.QueryString("Id2")

        End If
        'eseguo la query e passo il risultato al datareader
        dtsEntiSettori = ClsServer.DataSetGenerico(strSql, Session("conn"))
        'controllo se ci sono dei record
        If dtsEntiSettori.Tables(0).Rows.Count > 0 Then
            'al datasource sella combo passo il datareader
            dtgAmbitiSelezionati.DataSource = dtsEntiSettori
            If Request.QueryString("blocco") = "TRUE" Then
                dtgAmbitiSelezionati.Columns(0).Visible = False
            End If
            dtgAmbitiSelezionati.DataBind()
        End If
    End Sub
    Private Function LoadStoreEntiVariazioniSettori(ByVal IdEnte As Integer, ByVal NuoviSettori As String) As DataSet
        'aggiunto da simona cordella il 18/10/2013
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_ENTIVARIAZIONISETTORI]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            sqlDAP.SelectCommand.Parameters.Add("@NuoviSettori", SqlDbType.NVarChar).Value = NuoviSettori

            sqlDAP.Fill(dataSet)
            Return dataSet
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try
        Return dataSet
    End Function

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Dim tipologia As String = Request.QueryString("tipologia")
        Dim arrivo As String = Request.QueryString("Arrivo")
        Dim vediEnte As String = Request.QueryString("VediEnte")
        Dim identePadre As String = Request.QueryString("identePadre")
        Response.Redirect("WfrmAlbero.aspx?tipologia=" & tipologia & "&Arrivo=" & arrivo & "&VediEnte=" & vediEnte & "&identePadre=" & identePadre)
    End Sub
End Class