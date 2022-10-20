Imports System.IO
Imports System.Web.UI
Imports System.Data
Imports System.Data.SqlClient
Public Class WfrmEsportazioneTransitoSCU
    Inherits System.Web.UI.Page

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
        VerificaSessione()
        If Page.IsPostBack = False Then
            If ControlloStatoEnte(Session("IdEnte")) = True Then
                CmdElabora.Visible = True
                lblMessaggioErrore.Visible = False
                lblMessaggioErrore.Text = ""
            Else
                CmdElabora.Visible = False
                lblMessaggioErrore.Visible = True
                lblMessaggioErrore.Text = "Funzione abilitata solamente agli Enti accreditati.<br> Nel caso di ente in fase di transito dall'albo di SCN all'albo di SCU accedere al sistema utilizzando le credenziali di accesso di SCN."
            End If

        End If
    End Sub

    Private Sub CmdElabora_Click(sender As Object, e As System.EventArgs) Handles CmdElabora.Click
        Dim objHLinkEnti As HyperLink
        Dim objHLinkSedi As HyperLink
        Dim objHLinkRisorse As HyperLink
        Dim Esito As String
        Dim strMessaggio As String = ""

        lblMessaggio.Visible = False
        lblMessaggioErrore.Visible = False
        dwFile.Visible = False

        ' Modifica del 02 05 2019 - Leucci Luigi
        ' Eliminazione della creazione dei file csv
        'Esito = EseguiStoreEXPORT(Session("IdEnte"), Session("Utente"), strMessaggio, Session("conn"))
        'If Esito = "OK" Then
        lblMessaggio.Visible = True
        lblMessaggio.Text = strMessaggio

        dwFile.Visible = True

        objHLinkEnti = RecuperaFileExportEnti(Session("IdEnte"), Session("Conn"))

        'hlScaricaEnti.Visible = True
        hlScaricaEnti.Text = objHLinkEnti.Text
        hlScaricaEnti.NavigateUrl = objHLinkEnti.NavigateUrl

        objHLinkSedi = RecuperaFileExportSedi(Session("IdEnte"), Session("Conn"))

        'hlScaricaSedi.Visible = True
        hlScaricaSedi.Text = objHLinkSedi.Text
        hlScaricaSedi.NavigateUrl = objHLinkSedi.NavigateUrl

        objHLinkRisorse = RecuperaFileExportRisorse(Session("IdEnte"), Session("Conn"))

        'hlScaricaRisorse.Visible = True
        hlScaricaRisorse.Text = objHLinkRisorse.Text
        hlScaricaRisorse.NavigateUrl = objHLinkRisorse.NavigateUrl


        'Else
        'lblMessaggioErrore.Visible = True
        'lblMessaggioErrore.Text = strMessaggio
        'End If

    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Function ControlloStatoEnte(ByVal IdEnte As Integer) As Boolean
        Dim dtr As SqlClient.SqlDataReader
        Dim strsql As String
        Dim bln As Boolean

        strsql = "Select idente from enti where idstatoente in (3,9) and idente =" & IdEnte
        dtr = ClsServer.CreaDatareader(strsql, Session("conn"))
        bln = dtr.HasRows
        ChiudiDataReader(dtr)

        If Not bln Then
            strsql = "Select idente from ChiusureEntiDismissioneAlboSCN where idVecchioStato in (3,9) and idente =" & IdEnte
            dtr = ClsServer.CreaDatareader(strsql, Session("conn"))
            bln = dtr.HasRows
            ChiudiDataReader(dtr)
        End If

        Return bln
    End Function

    Private Function EseguiStoreEXPORT(ByVal IdEnte As Integer, ByVal USERNAME As String, ByRef Messaggio As String, ByVal conn As SqlClient.SqlConnection) As String
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE: 09/10/2017
        'FUNZIONALITA': RICHIAMO STORE PER LA GESTIONE DELLA VISIBILITà DEI FAMI

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_EXPORT]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, conn)
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            sqlCMD.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = USERNAME

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 2
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 1000
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)
            sqlCMD.ExecuteScalar()

            Dim strEsito As String


            strEsito = sqlCMD.Parameters("@Esito").Value
            Messaggio = sqlCMD.Parameters("@Messaggio").Value
            Return strEsito

        Catch ex As Exception

            Exit Function
        End Try
    End Function

    Private Function RecuperaFileExportEnti(ByVal IdEnte As Integer, ByRef cnLocal As SqlConnection) As HyperLink
        Dim da As New SqlDataAdapter _
                        ("SELECT BinData_Accoglienza, FileName_Accoglienza FROM Accreditamento_Export_CSV   WHERE IdEnte = " & IdEnte, cnLocal)
        Dim cb As SqlCommandBuilder = New SqlCommandBuilder(da)
        Dim ds As New DataSet

        Try
            Dim oblLocalHLink As New HyperLink

            da.Fill(ds, "_FileTest")
            Dim rw As DataRow
            rw = ds.Tables("_FileTest").Rows(0)

            ' Make sure you have some rows
            Dim i As Integer = ds.Tables("_FileTest").Rows.Count
            If i > 0 Then
                Dim bBLOBStorage() As Byte = _
                ds.Tables("_FileTest").Rows(0)("BinData_Accoglienza")
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("FileName_Accoglienza")
                oblLocalHLink.NavigateUrl = FileByteToPath(bBLOBStorage, ds.Tables("_FileTest").Rows(0)("FileName_Accoglienza"))
            End If

            Return oblLocalHLink
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
      
    End Function

    Private Function RecuperaFileExportSedi(ByVal IdEnte As Integer, ByRef cnLocal As SqlConnection) As HyperLink
        Dim da As New SqlDataAdapter _
            ("SELECT BinData_Sedi, FileName_Sedi FROM Accreditamento_Export_CSV   WHERE IdEnte = " & IdEnte, cnLocal)
        Dim cb As SqlCommandBuilder = New SqlCommandBuilder(da)
        Dim ds As New DataSet

        Try
            Dim oblLocalHLink As New HyperLink

            da.Fill(ds, "_FileTest")
            Dim rw As DataRow
            rw = ds.Tables("_FileTest").Rows(0)

            ' Make sure you have some rows
            Dim i As Integer = ds.Tables("_FileTest").Rows.Count
            If i > 0 Then
                Dim bBLOBStorage() As Byte = _
                ds.Tables("_FileTest").Rows(0)("BinData_Sedi")
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("FileName_Sedi")
                oblLocalHLink.NavigateUrl = FileByteToPath(bBLOBStorage, ds.Tables("_FileTest").Rows(0)("FileName_Sedi"))
            End If

            Return oblLocalHLink
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function
    Private Function RecuperaFileExportRisorse(ByVal IdEnte As Integer, ByRef cnLocal As SqlConnection) As HyperLink
        Dim da As New SqlDataAdapter _
            ("SELECT BinData_Risorse, FileName_Risorse FROM Accreditamento_Export_CSV WHERE IdEnte = " & IdEnte, cnLocal)
        Dim cb As SqlCommandBuilder = New SqlCommandBuilder(da)
        Dim ds As New DataSet

        Try
            Dim oblLocalHLink As New HyperLink

            da.Fill(ds, "_FileTest")
            Dim rw As DataRow
            rw = ds.Tables("_FileTest").Rows(0)

            ' Make sure you have some rows
            Dim i As Integer = ds.Tables("_FileTest").Rows.Count
            If i > 0 Then
                Dim bBLOBStorage() As Byte = _
                ds.Tables("_FileTest").Rows(0)("BinData_Risorse")
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("FileName_Risorse")
                oblLocalHLink.NavigateUrl = FileByteToPath(bBLOBStorage, ds.Tables("_FileTest").Rows(0)("FileName_Risorse"))
            End If

            Return oblLocalHLink
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function

    Private Function FileByteToPath(ByVal dataBuffer As Byte(), ByVal nomeFile As String) As String
        'dichiaro una variabile byte che bufferizza (carica in memoria) il file template richiesto
        'e trasformato in base64
        Dim fs As FileStream
        Dim myPath As New System.Web.UI.Page

        If File.Exists(myPath.Server.MapPath("download") & "\" & nomeFile) Then
            File.Delete(myPath.Server.MapPath("download") & "\" & nomeFile)
        End If
        'passo il template al filestream
        fs = New FileStream(myPath.Server.MapPath("download") & "\" & nomeFile, FileMode.Create, FileAccess.Write)
        'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
        If (dataBuffer.Length > 0) Then
            fs.Write(dataBuffer, 0, dataBuffer.Length)
        End If

        'chiudo lo streaming
        fs.Close()
        Return "download\" & nomeFile
    End Function
End Class