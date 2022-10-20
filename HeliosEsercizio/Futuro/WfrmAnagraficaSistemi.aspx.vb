Imports System.Data.SqlClient
Imports System.IO
Public Class WfrmAnagraficaSistemi
    Inherits System.Web.UI.Page
    Dim myDataSet As New DataSet
    Dim myDataTable As DataTable
    Dim myDataReader As SqlClient.SqlDataReader
    Dim querySql As String
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
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub

#End Region
#Region "Routine"
    Sub CaricaGrigliaSistemi(ByVal IdEnte As Integer)
        dtgSistemi.CurrentPageIndex = 0

        querySql = "  SELECT s.IDSistema,S.Sistema ,Accreditato,"
        querySql &= " CASE WHEN ISNULL(Accreditato,0)=0 THEN 'DA ACCREDITARE' WHEN ISNULL(Accreditato,0)='-1' THEN 'NON ACCREDITATO'  ELSE 'ISCRITTO' END  AS STATO, "
        querySql &= " CASE WHEN ISNULL(Accreditato,0)=0 then 'SI' WHEN ISNULL(Accreditato,0)='-1' then 'SI' WHEN ISNULL(Accreditato,0)=1 then 'SI' END AS PulsanteConfermaValida,"
        querySql &= " CASE WHEN ISNULL(Accreditato,0)=0 then 'SI' WHEN ISNULL(Accreditato,0)='-1' then 'SI' WHEN ISNULL(Accreditato,0)=1 then 'SI' END AS PulsanteConfermaNONValida  "
        querySql &= " FROM sistemi s "
        querySql &= " LEFT join  entisistemi es on s.IDSistema=es.IDSistema and IdEnte = " & IdEnte
        querySql &= " WHERE (ALBO ='SCU' OR Albo IS NULL)  "

        Session("myDataSet") = ClsServer.DataSetGenerico(querySql, Session("conn"))

        dtgSistemi.DataSource = Session("myDataSet")
        dtgSistemi.DataBind()

        If Session("TipoUtente") = "E" Then
            dtgSistemi.Columns(5).Visible = False 'valida
            dtgSistemi.Columns(6).Visible = False 'non valida 
        End If
        If UCase(StatoEnte(IdEnte)) = "REGISTRATO" Then
            cmdConferma.Visible = True
        Else
            cmdConferma.Visible = False
        End If

    End Sub
    Sub CaricaSistemiEnte(ByVal IdEnte As Integer)
        Dim dtrSistemi As SqlDataReader
        'variabile stringa locale per costruire la query per le aree
        Dim strSql As String
        Dim item As DataGridItem
        Dim i As Integer
        ChiudiDataReader(dtrSistemi)
        strSql = "select IDSistema from entisistemi WHERE IdEnte = " & IdEnte
        dtrSistemi = ClsServer.CreaDatareader(strSql, Session("conn"))
        While dtrSistemi.Read()
            For Each item In dtgSistemi.Items
                Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaSistema"), CheckBox)
                If dtrSistemi("IDSistema") = item.Cells(1).Text() Then
                    check.Checked = True
                End If
            Next
        End While
        ChiudiDataReader(dtrSistemi)
    End Sub
    Sub CaricaDocumenti(ByVal IdSistema As Integer, ByVal IdEnte As String)

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  24/07/2017
        'FUNZIONALITA': RICHIAMO STORE PER CARICAMENTO DELLA GRIGLIA DEI DOCUMENTI DEI SISTEMI

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_ELENCO_DOCUMENTI_SISTEMI]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdSistema", SqlDbType.VarChar).Value = IdSistema
            sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.VarChar).Value = IdEnte

            sqlDAP.Fill(dataSet)

            ' Session("appDtsRisRicerca") = dataSet
            dtgElencoDocumenti.DataSource = dataSet
            dtgElencoDocumenti.DataBind()

        Catch ex As Exception
            msgErrore.Visible = True
            msgErrore.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."

            Exit Sub
        End Try

        If Session("TipoUtente") = "E" Then
            dtgElencoDocumenti.Columns(7).Visible = False 'valida
            dtgElencoDocumenti.Columns(8).Visible = False 'non valida 
        End If

    End Sub
    Private Function ConfermaSistema(ByVal Utente As String, ByVal IdEnte As Integer, ByVal Stato As Integer, ByVal IdSistema As Integer, Optional ByRef msg As String = "", Optional ByRef Esito As String = "") As String
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  24/07/2017
        'FUNZIONALITA': RICHIAMO STORE PER LA VALIDITA' DEL SISITEMA
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_VALIDA_SISTEMI]"


        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, Session("conn"))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Utente
            sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            sqlCMD.Parameters.Add("@Stato", SqlDbType.Int).Value = Stato
            sqlCMD.Parameters.Add("@IdSistema", SqlDbType.Int).Value = IdSistema

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
    Private Function ConfermaDocumento(ByVal Utente As String, ByVal IdEnteDocumento As Integer, ByVal Stato As Integer, Optional ByRef msg As String = "", Optional ByRef Esito As String = "") As String
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  24/07/2017
        'FUNZIONALITA': RICHIAMO STORE PER LA VALIDITA' DEI DOCUMENTI DEI SISITEMI
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_VALIDA_DOCUMENTI_SISTEMI]"


        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, Session("conn"))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Utente
            sqlCMD.Parameters.Add("@IdEnteDocumento", SqlDbType.Int).Value = IdEnteDocumento
            sqlCMD.Parameters.Add("@Stato", SqlDbType.Int).Value = Stato


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
    Function StatoEnte(ByVal IdEnte As Integer) As String
        Dim strStatoEnte As String
        ChiudiDataReader(myDataReader)
        querySql = "  SELECT s.StatoEnte from enti e inner join statienti s on e.idstatoente=s.idstatoente where idente =" & IdEnte
        myDataReader = ClsServer.CreaDatareader(querySql, Session("conn"))
        If myDataReader.HasRows = True Then
            myDataReader.Read()
            strStatoEnte = myDataReader("StatoEnte")
        End If
        ChiudiDataReader(myDataReader)
        Return strStatoEnte
    End Function
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()

        If Page.IsPostBack = False Then
            hf_IdEnteFase.Value = Request.QueryString("IdEnteFase")
            CaricaGrigliaSistemi(Session("IDEnte"))
            CaricaSistemiEnte(Session("IDEnte"))
        End If
    End Sub

    Protected Sub cmdConferma_Click(sender As Object, e As EventArgs) Handles cmdConferma.Click
        Dim item As DataGridItem
        Dim i As Integer
        Dim myCommand As System.Data.SqlClient.SqlCommand
        Dim strSql As String

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            strSql = "Delete entisistemi where IdEnte =" & Session("IdEnte")
            myCommand.CommandText = strSql
            myCommand.ExecuteNonQuery()

            For Each item In dtgSistemi.Items
                Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaSistema"), CheckBox)
                'controllo se è selezionato il check
                If check.Enabled = True Then
                    strSql = "Insert into entisistemi (idente,idsistema,Username,datacreazioneRecord) Values " & _
                           "(" & Trim(Session("IdEnte")) & "," & item.Cells(1).Text & ",'" & Session("Utente") & "',getdate()) "
                    myCommand.CommandText = strSql
                    myCommand.ExecuteNonQuery()

                End If
            Next
            msgConferma.Text = "Aggiornamento effettuato con successo"
        Catch ex As Exception
            msgErrore.Text = "Errore imprevisto. Contattare l'assistenza."
        End Try

    End Sub

    Private Sub dtgSistemi_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgSistemi.ItemCommand

        Dim msg As String
        Dim Esito As String
        Select Case e.CommandName
            Case "Documento"
                DivDettaglioDocumenti.Visible = True
                CaricaDocumenti(e.Item.Cells(1).Text, Session("IdEnte"))

            Case "Valida"
                ConfermaSistema(Session("Utente"), Session("IdEnte"), 1, e.Item.Cells(1).Text, msg, Esito)
                If Esito = "NEGATIVO" Then
                    msgErrore.Visible = True
                    msgConferma.Visible = False
                    msgErrore.Text = msg
                Else
                    msgErrore.Visible = False
                    msgConferma.Visible = True
                    msgConferma.Text = msg
                End If
                CaricaGrigliaSistemi(Session("IdEnte"))
            Case "NonValida"
                ConfermaSistema(Session("Utente"), Session("IdEnte"), 2, e.Item.Cells(1).Text, msg, Esito)
                If Esito = "NEGATIVO" Then
                    msgErrore.Visible = True
                    msgConferma.Visible = False
                    msgErrore.Text = msg
                Else
                    msgErrore.Visible = False
                    msgConferma.Visible = True
                    msgConferma.Text = msg
                End If
                CaricaGrigliaSistemi(Session("IdEnte"))
        End Select
    End Sub

    Private Sub dtgSistemi_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgSistemi.PageIndexChanged
        dtgSistemi.CurrentPageIndex = e.NewPageIndex
        dtgSistemi.DataSource = Session("myDataSet")
        dtgSistemi.DataBind()
        dtgSistemi.SelectedIndex = -1
        dtgSistemi.EditItemIndex = -1
        CaricaSistemiEnte(Session("IDEnte"))
    End Sub

    Private Sub dtgElencoDocumenti_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgElencoDocumenti.ItemCommand
        Dim msg As String
        Dim Esito As String

        Select Case e.CommandName
            Case "Download"
                Response.Write("<SCRIPT>" & vbCrLf)
                Response.Write("window.open('WfrmDocFileDownload.aspx?Origine=Sistemi&IdEnteDocumento=" & e.Item.Cells(4).Text & "','report','height=600,width=600,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
                Response.Write("</SCRIPT>")
            Case "Valida"
                ConfermaDocumento(Session("Utente"), e.Item.Cells(4).Text, 1, msg, Esito)
                If Esito = "NEGATIVO" Then
                    msgErrore.Visible = True
                    msgConferma.Visible = False
                    msgErrore.Text = msg
                Else
                    msgErrore.Visible = False
                    msgConferma.Visible = True
                    msgConferma.Text = msg
                End If
                CaricaDocumenti(e.Item.Cells(5).Text, Session("IdEnte"))
                CaricaGrigliaSistemi(Session("IdEnte"))
            Case "NonValida"
                ConfermaDocumento(Session("Utente"), e.Item.Cells(4).Text, 2, msg, Esito)
                If Esito = "NEGATIVO" Then
                    msgErrore.Visible = True
                    msgConferma.Visible = False
                    msgErrore.Text = msg
                Else
                    msgErrore.Visible = False
                    msgConferma.Visible = True
                    msgConferma.Text = msg
                End If
                CaricaDocumenti(e.Item.Cells(5).Text, Session("IdEnte"))
                CaricaGrigliaSistemi(Session("IdEnte"))
        End Select
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click

        Dim tipologia As String = Request.QueryString("tipologia")
        Response.Redirect("WfrmAlbero.aspx?tipologia=" & tipologia)


    End Sub

    Protected Sub dtgSistemi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dtgSistemi.SelectedIndexChanged

    End Sub

    Protected Sub dtgElencoDocumenti_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dtgElencoDocumenti.SelectedIndexChanged

    End Sub
End Class