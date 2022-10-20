Imports System
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.HttpServerUtility
Imports System.Web.UI.HtmlControls.HtmlInputFile
Imports System.Drawing

Public Class WfrmGestioneManualiUtente
    Inherits System.Web.UI.Page
    Shared IntIDManuale As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaGriglia()
        End If
    End Sub

    Sub CaricaGriglia()
        Dim strsql As String
        Dim MyDataset As DataSet

        dgManuali.CurrentPageIndex = 0
        strsql = " SELECT  IdManuale, NomeApplicazione, Manuale, Versione, isnull(FileName,'') as FileName,"
        strsql &= " isnull(dbo.formatodata(DataInserimento),'') as DataInserimento,"
        strsql &= " isnull(UsernameInserimento,'') as UsernameInserimento,isnull(HashValue,'') as HashValue"
        strsql &= " FROM Manuali"
        strsql &= " WHERE NomeApplicazione='" & Session("Sistema") & "' "
        strsql &= " ORDER BY ordine"

        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgManuali.DataSource = MyDataset
        dgManuali.DataBind()

        Session("DtsManuali") = MyDataset

        MyDataset.Dispose()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub cmdUpload_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click
        Try
            If IntIDManuale = 0 Then
                LblMsgFile.Text = "E' necessario selezionare il Manuale."
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If
            Dim msg As String
            Dim PrefissoFile As String = ""
            LblMsgFile.Text = ""

            If txtSelFile.Value = "" Then
                LblMsgFile.Text = "E' necessario selezionare il file da associare al Manuale."
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If
            If VerificaEstensioneManuale(txtSelFile) = False Then
                LblMsgFile.Text = "Il formato del file non è corretto.E' possibile associare documenti nel formato .PDF o .PDF.P7M"
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If
            msg = CaricaManuale(IntIDManuale, Session("Utente"), txtSelFile, Session("conn"), PrefissoFile)
            If msg = "ok" Then
                LblMsgFile.Text = "Effettuato aggionamento Manuale."
                LblMsgFile.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
            Else
                LblMsgFile.Text = msg
                LblMsgFile.ForeColor = Color.Red
            End If
            IntIDManuale = 0
            CaricaGriglia()

        Catch ex As Exception
            LblMsgFile.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            LblMsgFile.ForeColor = Color.Red
        Finally
            cmdUpload.Enabled = True
        End Try
    End Sub
    Private Function VerificaEstensioneManuale(ByVal objPercorsoFile As HtmlInputFile) As Boolean
        'sono accettati solo documento con estensione .pdf e .pdf.p7m
        Dim NomeFile As String = ""
        Dim i As Integer

        VerificaEstensioneManuale = False

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeFile = Mid(objPercorsoFile.Value, i, 1) & NomeFile
        Next

        If UCase(Right(NomeFile, 4)) = ".PDF" Or UCase(Right(NomeFile, 8)) = ".PDF.P7M" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function CaricaManuale(ByVal IdManuale As Integer, ByVal strUtente As String, ByVal objPercorsoFile As HtmlInputFile, ByRef cnLocal As SqlConnection, ByVal PrefissoFile As String) As String
        Dim NomeUnivoco As String = ""
        Dim strPercorsoServer As String
        Dim i As Integer
        Dim myPath As New System.Web.UI.Page
        Dim msg As String = ""
        Dim STRSQL As String

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeUnivoco = Mid(objPercorsoFile.Value, i, 1) & NomeUnivoco
        Next

        Dim strNomeFile As String = Now.Year.ToString & Now.Month.ToString & Now.Day.ToString & Now.Hour.ToString & Now.Minute.ToString & Now.Second.ToString & "_" & strUtente

        strPercorsoServer = myPath.Server.MapPath("upload") & "\" & strNomeFile & "_" & NomeUnivoco

        If File.Exists(strPercorsoServer) Then
            File.Delete(strPercorsoServer)
        End If

        objPercorsoFile.PostedFile.SaveAs(strPercorsoServer)

        Dim fs As New FileStream _
                (strPercorsoServer, FileMode.Open, FileAccess.Read)
        Dim iLen As Integer = CInt(fs.Length - 1)
        'Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen < 0 Then
            msg = "Attenzione.Impossibile caricare un file vuoto."
        Else
            If iLen > 20971520 Then
                msg = "Attenzione.La dimensione massima è di 20 MB."
            Else
                Dim numBytesToRead As Integer = CType(fs.Length, Integer)
                Dim numBytesRead As Integer = 0

                While (numBytesToRead > 0)
                    ' Read may return anything from 0 to numBytesToRead.
                    Dim n As Integer = fs.Read(bBLOBStorage, numBytesRead, _
                        numBytesToRead)
                    ' Break when the end of the file is reached.
                    If (n = 0) Then
                        Exit While
                    End If
                    numBytesRead = (numBytesRead + n)
                    numBytesToRead = (numBytesToRead - n)

                End While
                numBytesToRead = bBLOBStorage.Length

                fs.Close()

                Dim strHashValue As String
                strHashValue = GeneraHash(bBLOBStorage)

                InserimentoCronologiaManuali(IdManuale, cnLocal)
                UpdateManuale(IdManuale, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, cnLocal)
                msg = "ok"
            End If
        End If

        If File.Exists(strPercorsoServer) Then
            File.Delete(strPercorsoServer)
        End If
        Return msg
    End Function

    Private Function GeneraHash(ByVal FileinByte() As Byte) As String
        Dim tmpHash() As Byte

        tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

        GeneraHash = ByteArrayToString(tmpHash)
        Return GeneraHash
    End Function

    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim i As Integer
        Dim sOutput As New StringBuilder(arrInput.Length)
        For i = 0 To arrInput.Length - 1
            sOutput.Append(arrInput(i).ToString("X2"))
        Next
        Return sOutput.ToString()
    End Function
    Private Shared Sub InserimentoCronologiaManuali(ByVal IDManuale As Integer, ByVal cnLocal As SqlClient.SqlConnection)

        'sub che consente l'inserimento dei documenti da associare al progetto
        Dim cmd As SqlClient.SqlCommand = New SqlCommand _
            (" INSERT INTO ManualiCronologia(IdManuale, NomeApplicazione, Manuale, Versione, BinData, Note, " & _
             " FileName, DataInserimento, UsernameInserimento, HashValue, " & _
             " Visibile, Ente, DGSCN, RPA, Ordine) " & _
             "   SELECT    IdManuale, NomeApplicazione, Manuale, Versione, BinData, Note,  " & _
             " FileName, DataInserimento, UsernameInserimento, HashValue, " & _
             " Visibile, Ente, DGSCN, RPA, Ordine FROM  Manuali WHERE IdManuale = " & IDManuale & " ", cnLocal)

        cmd.ExecuteNonQuery()
    End Sub

    Sub UpdateManuale(IdManuale, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, cnLocal)
        'sub che consente l'update dei manuali 
        Dim cmd As SqlClient.SqlCommand = New SqlCommand _
        ("UPDATE Manuali SET BinData = @blob_data, " & _
                            " FileName = @blob_filename, " & _
                            " DataInserimento =getdate(), " & _
                            " UsernameInserimento = @utente, " & _
                            " HashValue= @hash_value, " & _
                            " NomeApplicazione='" & Session("Sistema") & "' " & _
                            " WHERE IdManuale = @IdManuale ", cnLocal)

        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("@IdManuale", SqlDbType.Int)
        cmd.Parameters("@IdManuale").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_filename", SqlDbType.VarChar)
        cmd.Parameters("@blob_filename").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
        cmd.Parameters("@blob_data").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@utente", SqlDbType.VarChar)
        cmd.Parameters("@utente").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@hash_value", SqlDbType.VarChar)
        cmd.Parameters("@hash_value").Direction = ParameterDirection.Input


        cmd.Parameters("@IdManuale").Value = IdManuale
        cmd.Parameters("@blob_filename").Value = NomeUnivoco
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue

        cmd.ExecuteNonQuery()
    End Sub

    Private Sub dgManuali_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgManuali.ItemCommand
        If e.CommandName = "Seleziona" Then
            ColoreGriglia()
            IntIDManuale = e.Item.Cells(0).Text
            e.Item.BackColor = Color.LightYellow
        End If
    End Sub

    Private Sub dgManuali_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgManuali.PageIndexChanged
        dgManuali.CurrentPageIndex = e.NewPageIndex
        dgManuali.DataSource = Session("DtsManuali")
        dgManuali.DataBind()
        dgManuali.SelectedIndex = -1
    End Sub

    Sub ColoreGriglia()
        Dim dtgItem As DataGridItem
        For Each dtgItem In dgManuali.Items
            dtgItem.BackColor = Color.White
        Next

    End Sub
End Class