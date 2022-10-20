
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

Public Class clsGestioneDocumenti
    Inherits System.Web.UI.Page

    'REALIZZATA DA: SIMONA CORDELLA 
    'DATA REALIZZAZIONE: 28/06/2012
    'FUNZIONALITA': GESTIONE DEI DOCUMENTI ALL'INTERNO DI HELIOS

    Public Shared Sub CaricaDocumentoProgettoBOX(ByVal IdProgetto As Integer, ByVal strUtente As String, ByVal NomeFileBOX As String, ByVal PercorsoFileBOX As String, ByRef cnLocal As SqlConnection)
        'Dim NomeUnivoco As String = ""
        Dim strPercorsoServer As String
        Dim i As Integer
        Dim myPath As New System.Web.UI.Page

        Dim fs As New FileStream _
                (PercorsoFileBOX, FileMode.OpenOrCreate, FileAccess.Read)
        Dim iLen As Integer = CInt(fs.Length - 1)
        Dim bBLOBStorage(iLen) As Byte
        fs.Read(bBLOBStorage, 0, iLen)
        fs.Close()

        Dim strHashValue As String
        strHashValue = GeneraHash(bBLOBStorage)

        'StoreAttivitaDocumenti(IdProgetto, NomeFileBOX, bBLOBStorage(), strUtente, strHashValue, cnLocal)

        Dim cmd As SqlCommand = New SqlCommand _
         ("INSERT INTO AttivitàDocumenti (IdAttività,BinData, FileName,DataInserimento,UsernameInserimento,HashValue) " _
            & "VALUES(@idattivita,@blob_data,@blob_filename ,getdate(),@utente,@hash_value   )", cnLocal)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("@idattivita", SqlDbType.Int)
        cmd.Parameters("@idattivita").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_filename", SqlDbType.VarChar)
        cmd.Parameters("@blob_filename").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
        cmd.Parameters("@blob_data").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@utente", SqlDbType.VarChar)
        cmd.Parameters("@utente").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@hash_value", SqlDbType.VarChar)
        cmd.Parameters("@hash_value").Direction = ParameterDirection.Input


        cmd.Parameters("@idattivita").Value = IdProgetto
        cmd.Parameters("@blob_filename").Value = NomeFileBOX
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue

        cmd.ExecuteNonQuery()

    End Sub

    Private Shared Function FileByteToPath(ByVal dataBuffer As Byte(), ByVal nomeFile As String) As String
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

    Private Shared Function GeneraHash(ByVal FileinByte() As Byte) As String
        Dim tmpHash() As Byte

        tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

        GeneraHash = ByteArrayToString(tmpHash)
        Return GeneraHash
    End Function

    Private Shared Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim i As Integer
        Dim sOutput As New StringBuilder(arrInput.Length)
        For i = 0 To arrInput.Length - 1
            sOutput.Append(arrInput(i).ToString("X2"))
        Next
        Return sOutput.ToString()
    End Function


    Public Shared Sub CaricaAllegatoSediEnte(ByVal IdEnteFase As Integer, ByVal IdEnte As Integer, ByVal strUtente As String, ByVal NomeFileBOX As String, ByVal PercorsoFileBOX As String, ByRef cnLocal As SqlConnection)
        'Dim NomeUnivoco As String = ""
        Dim strPercorsoServer As String
        Dim i As Integer
        Dim myPath As New System.Web.UI.Page

        Dim fs As New FileStream _
                (PercorsoFileBOX, FileMode.OpenOrCreate, FileAccess.Read)
        Dim iLen As Integer = CInt(fs.Length - 1)
        Dim bBLOBStorage(iLen) As Byte
        fs.Read(bBLOBStorage, 0, iLen)
        fs.Close()

        Dim strHashValue As String
        strHashValue = GeneraHash(bBLOBStorage)



        Dim cmd As SqlCommand = New SqlCommand _
         ("INSERT INTO EntiDocumenti (IdEnteFase,BinData, FileName,DataInserimento,UsernameInserimento,HashValue) " _
            & "VALUES(@IdEnteFase,@blob_data,@blob_filename ,getdate(),@utente,@hash_value )", cnLocal)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("@IdEnteFase", SqlDbType.Int)
        cmd.Parameters("@IdEnteFase").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
        cmd.Parameters("@blob_data").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_filename", SqlDbType.VarChar)
        cmd.Parameters("@blob_filename").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@utente", SqlDbType.VarChar)
        cmd.Parameters("@utente").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@hash_value", SqlDbType.VarChar)
        cmd.Parameters("@hash_value").Direction = ParameterDirection.Input


        cmd.Parameters("@IdEnteFase").Value = IdEnteFase
        cmd.Parameters("@blob_filename").Value = NomeFileBOX
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue

        cmd.ExecuteNonQuery()

    End Sub


End Class
