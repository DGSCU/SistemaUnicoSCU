Imports System.Data.SqlClient
Imports System.Threading

Public Class wfrmDocumentiProgetto_Applica
    Inherits System.Web.UI.Page
    Dim dtrgenerico As SqlClient.SqlDataReader

    Protected WithEvents lbltipo As System.Web.UI.WebControls.Label
    Protected WithEvents chkSelDesel As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblIdente As System.Web.UI.WebControls.Label

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

        If Request.QueryString("IdAttivita") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then
                ChiudiDataReader(dtrgenerico)
            Else
                ChiudiDataReader(dtrgenerico)
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
        End If
        If Page.IsPostBack = False Then
            If StatoDocumenti(Request.QueryString("IdAttivita")) Then

                Response.Redirect("WfrmStatoApplicazioneDocumentiProgetti.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))
            Else
                LoadIntestazione(Request.QueryString("IdAttivita"))
                'controllo se coprogrammante. allora personalizzo funzioni
                If ClsUtility.ProgettiLimitaFunzioniEnteNonProponente(Request.QueryString("IdAttivita"), Session("IdEnte"), Session("Conn")) Then
                    lblmsg.Text = "L'ente non è abilitato alla gestione dei documenti del progetto"
                    lblmsg.Visible = True
                    divGestione.Visible = False
                    cmdSalva.Visible = False
                Else

                    LoadElencoDocumentiProgetto()
                    LoadProvince()
                    LoadProgettiRegistrati(Request.QueryString("IdAttivita"), 0)
                End If

            End If
        End If
    End Sub
    Sub LoadIntestazione(ByVal IdAttivita As Integer)
        Dim strSql As String
        Dim rtsProg As SqlClient.SqlDataReader

        strSql = " SELECT attività.titolo, attività.codiceente as codice,statoattività,isnull(StatoBandoAttività,'Non presente') as StatoBandoAttività " & _
                 " FROM attività  inner join statiattività on attività.idstatoattività = statiattività.idstatoattività " & _
                 " left join bandiattività on attività.idbandoattività = bandiattività.idbandoattività " & _
                 " left join statiBandiAttività on bandiattività.IdStatoBandoAttività=statiBandiAttività.IdStatoBandoAttività " & _
                 " WHERE attività.IDAttività = " & IdAttivita
        rtsProg = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If rtsProg.HasRows = True Then
            rtsProg.Read()
            lblTitoloProgetto.Text = "" & rtsProg("Titolo")
            LblCodice.Text = "" & rtsProg("Codice")
            LblStatoProgetto.Text = "" & rtsProg("statoattività")
            LblStatoIstanza.Text = "" & rtsProg("StatoBandoAttività")
        End If
        rtsProg.Close()
        rtsProg = Nothing
    End Sub

    Sub LoadElencoDocumentiProgetto()
        Dim strSql As String
        Dim sqlDataSet As DataSet
        strSql = "SELECT IdAttivitàDocumento, FileName, DataInserimento,hashvalue FROM AttivitàDocumenti a " & _
                " left join PrefissiAttivitàDocumenti b on left(a.filename, charindex('_',a.filename)) = b.prefisso  " & _
                " where left(a.filename, charindex('_',a.filename)) <> 'PROG_' and left(a.filename, charindex('_',a.filename)) <> 'PROGGG_' and left(a.filename, charindex('_',a.filename)) <> 'OLP_' and idattività =" & Request.QueryString("IdAttivita") & " " & _
                " order by isnull(b.ordine,99) "
        Session("sqlDataSet") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dtgElencoDocumentiProgetto.DataSource = Session("sqlDataSet")
        dtgElencoDocumentiProgetto.DataBind()

    End Sub

    Private Sub LoadProgettiRegistrati(ByVal IdAttivita As Integer, ByVal IdProvincia As Integer)
        dtgProgetti.DataSource = LoadStoreRitornaElencoProgetti(IdAttivita, IdProvincia)
        dtgProgetti.DataBind()
    End Sub

    Private Sub LoadProvince()

        Dim strSql As String
        Dim dtrPro As SqlClient.SqlDataReader
        strSql = " SELECT 'TUTTE' AS Provincia ,0  AS IdProvincia , -1 as ordine     " & _
                 " UNION " & _
                 " SELECT Provincia , IdProvincia , 1 as ordine FROM PROVINCIE" & _
                 " WHERE ProvinceNazionali = 1 and isnull(CodiceIstat,0)<>0 " & _
                 " Order by ordine,Provincia "
        dtrPro = ClsServer.CreaDatareader(strSql, Session("Conn"))
        ddlProvincia.DataSource = dtrPro
        ddlProvincia.DataTextField = "Provincia"
        ddlProvincia.DataValueField = "IdProvincia"
        ddlProvincia.DataBind()
        dtrPro.Close()
        dtrPro = Nothing
    End Sub

    Private Function LoadStoreRitornaElencoProgetti(ByVal IdAttivita As Integer, ByVal IdProvincia As Integer) As DataSet
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_ElencoProgetti_ApplicaDocumenti]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = IdAttivita
            sqlDAP.SelectCommand.Parameters.Add("@IdProvincia", SqlDbType.Int).Value = IdProvincia
            sqlDAP.Fill(dataSet)
            Return dataSet
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click

       If (Request.QueryString("VengoDa") = Costanti.VENGO_DA_ISTANZA_PROGRAMMI) Then
            'DA MANDARE SULLA MASCHERA CHIAMANTE SIA IN INSERIMENTO CHE IN MODIFICA
            If Request.QueryString("idBP") Is Nothing Then
                Response.Redirect("WfrmProgrammiIstanza.aspx?Verso=Ins&VediEnte=1")
            Else
                Response.Redirect("WfrmProgrammiIstanza.aspx?idBP=" & Request.QueryString("idBP"))
            End If
        End If

        If Request.QueryString("Verso") = "Mod" Then
            Response.Redirect("wfrmIstanzaPresentazione.aspx?id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))
            Exit Sub
        End If


        If Request.QueryString("VengoDa") = "Istanza" Then

            Response.Redirect("wfrmIstanzaPresentazione.aspx?VengoDa=Istanza&IdAttivita=" & Request.QueryString("IdAttivita") & "&Verso=Ins" & "&VediEnte=1")
        Else
            Response.Redirect("wfrmDocumentiProgetto.aspx?StatoApplica=SI&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")))

        End If
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        Dim i As Integer = 0
        Dim chkDoc As CheckBox
        Dim strDoc As String = String.Empty 'riporto tutti gli id selezionati intervallati dal ;
        Dim chkProg As CheckBox
        Dim strIdProg As String = String.Empty  'riporto tutti gli id selezionati intervallati dal ;
        Dim strsql As String
        Dim cmdIns As SqlClient.SqlCommand
        lblmsg.Text = ""


        Try



            If ControllaCheck() = False Then Exit Sub
            If ControllaCheckProgetti() = False Then Exit Sub
            For i = 0 To dtgElencoDocumentiProgetto.Items.Count - 1
                chkDoc = dtgElencoDocumentiProgetto.Items(i).FindControl("chkSel")
                If chkDoc.Checked = True Then
                    strDoc = strDoc & dtgElencoDocumentiProgetto.Items(i).Cells(0).Text & ";"
                End If
            Next
            For i = 0 To dtgProgetti.Items.Count - 1
                chkProg = dtgProgetti.Items(i).FindControl("chkSelProg")
                If chkProg.Checked = True Then
                    strIdProg = strIdProg & dtgProgetti.Items(i).Cells(0).Text & ";"
                End If
            Next

            strsql = " INSERT INTO LockDocumentiEnte(IdEnte,NDoc,NProg,NElaborazioniMancanti) " & _
                     " SELECT IDENTEPRESENTANTE,0,0,0 FROM Attività WHERE Idattività = " & Request.QueryString("IdAttivita")
            cmdIns = New Data.SqlClient.SqlCommand(strsql, Session("conn"))
            cmdIns.ExecuteNonQuery()
            cmdIns.Dispose()

            'mod. il 11/12/12 per l'inserimento dei documenti viene richiamato un metodo di ws documentazionesperimentale che lavora in modo asincrono
            Async_InserimentoDocumentiProgetti(strIdProg, strDoc, Session("Utente"))




            Dim modifica As String = Request.QueryString("Modifica")
            Dim Nazionale As String = Request.QueryString("Nazionale")
            Dim idBA As String = Request.QueryString("idBA")
            Dim id As String = Request.QueryString("id")




            If StatoDocumenti(Request.QueryString("IdAttivita")) Then
                'Server.Transfer("WfrmStatoApplicazioneDocumentiProgetti.aspx?id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&idBA=" & Request.QueryString("idBA") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&VengoDa= " & Request.QueryString("VengoDa") & " &idattivita=" & Request.QueryString("IdAttivita") & "")
                Response.Redirect("WfrmStatoApplicazioneDocumentiProgetti.aspx?id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&idBA=" & Request.QueryString("idBA") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&VengoDa= " & Request.QueryString("VengoDa") & " &idattivita=" & Request.QueryString("IdAttivita") & "")
            End If

            'If Request.QueryString("VengoDa") = "Istanza" Then
            '    Response.Write("<script>" & vbCrLf)
            '    Response.Write("opener.location.href = opener.location;" & vbCrLf)
            '    Response.Write("</script>")
            'End If



        Catch ex As Exception

            lblmsg.Text = "Errore generico.Contattare l'assistenza."
            lblmsg.Text = ex.Message


        End Try
    End Sub

    Private Sub Async_InserimentoDocumentiProgetti(ByVal IdAttività As String, ByVal IdAttivitaDocumento As String, ByVal username As String)
        'funzionche richiama un metodo asincrono per l'inserimento multiplo di n documenti su n progetti
        Dim localWS As New WS_Editor.WSMetodiDocumentazione
        Dim ResultAsinc As IAsyncResult
        localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")
        localWS.Timeout = 1000000
        ResultAsinc = localWS.BeginAsync_InserimentoDocumentiProgetti(IdAttività, IdAttivitaDocumento, username, Nothing, "")
    End Sub

    Private Function InserimentoDocumentiProgetti(ByVal IdAttività As String, ByVal IdAttivitaDocumento As String, ByVal username As String) As String
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_DocumentiProgetti_Applica]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IdAttivita", SqlDbType.VarChar).Value = IdAttività
            sqlCMD.Parameters.Add("@IdAttivitaDocumento", SqlDbType.VarChar).Value = IdAttivitaDocumento
            sqlCMD.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Esito").Value
            Return str

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try

    End Function

    Private Function ControllaCheck() As Boolean
        Dim item As DataGridItem
        ControllaCheck = False
        For Each item In dtgElencoDocumentiProgetto.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkSel"), CheckBox)
            If check.Checked = True Then
                ControllaCheck = True
                Exit For
            End If
        Next
        If ControllaCheck = False Then
            lblmsg.Visible = True
            lblmsg.Text = "Attenzione, non è stato selezionato nessun documento."
        End If
    End Function

    Private Function ControllaCheckProgetti() As Boolean
        Dim item As DataGridItem
        ControllaCheckProgetti = False
        For Each item In dtgProgetti.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkSelProg"), CheckBox)
            If check.Checked = True Then
                ControllaCheckProgetti = True
                Exit For
            End If
        Next
        If ControllaCheckProgetti = False Then
            lblmsg.Visible = True
            lblmsg.Text = "Attenzione, non è stato selezionato nessun progetto."
        End If
    End Function

    Private Sub ddlProvincia_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlProvincia.SelectedIndexChanged
        dtgProgetti.CurrentPageIndex = 0
        LoadProgettiRegistrati(Request.QueryString("IdAttivita"), ddlProvincia.SelectedValue)
    End Sub
    Private Function StatoDocumenti(ByVal idattivita As Integer) As Boolean
        Dim strSql As String
        Dim dtrCount As SqlClient.SqlDataReader
        Dim blnReturn As Boolean
        strSql = " SELECT  NElaborazioniMancanti FROM LockDocumentiEnte l " & _
                 " INNER JOIN Attività a on a.identepresentante = l.idente WHERE a.idattività = " & idattivita
        dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))
        blnReturn = dtrCount.HasRows
        ChiudiDataReader(dtrCount)
        Return blnReturn
    End Function
End Class
