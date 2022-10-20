Imports System
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text
Imports System.Drawing.Imaging
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.HttpServerUtility
Imports System.Web.UI.HtmlControls.HtmlInputFile
Public Class clsGestioneDocumentiAccreditamento
    Private Shared Function FileByteToPath(ByVal dataBuffer As Byte(), ByVal nomeFile As String, ByVal HashValue As String) As String
        'dichiaro una variabile byte che bufferizza (carica in memoria) il file template richiesto
        'e trasformato in base64
        Dim fs As FileStream
        Dim myPath As New System.Web.UI.Page

        If File.Exists(myPath.Server.MapPath("download") & "\" & HashValue & "_" & nomeFile) Then
            File.Delete(myPath.Server.MapPath("download") & "\" & HashValue & "_" & nomeFile)
        End If
        ' nomeFile = nomeFile.Replace("#", "").Replace("&", "")
        'passo il template al filestream
        fs = New FileStream(myPath.Server.MapPath("download") & "\" & HashValue & "_" & nomeFile, FileMode.Create, FileAccess.Write)
        'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
        If (dataBuffer.Length > 0) Then
            fs.Write(dataBuffer, 0, dataBuffer.Length)
        End If

        'chiudo lo streaming
        fs.Close()
        Return "download\" & HashValue & "_" & nomeFile
    End Function
    Private Shared Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim i As Integer
        Dim sOutput As New StringBuilder(arrInput.Length)
        For i = 0 To arrInput.Length - 1
            sOutput.Append(arrInput(i).ToString("X2"))
        Next
        Return sOutput.ToString()
    End Function
    Private Shared Function GeneraHash(ByVal FileinByte() As Byte) As String
        Dim tmpHash() As Byte

        tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

        GeneraHash = ByteArrayToString(tmpHash)
        Return GeneraHash
    End Function
    Public Shared Function VerificaPrefissiDocumentiAccreditamento(ByVal objPercorsoFile As HtmlInputFile, ByVal conn As SqlClient.SqlConnection, ByRef PrefissoFile As String, ByVal Albo As String, ByRef blnBloccoAccreditamento As Boolean, ByVal TipoUtente As String) As Boolean
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        Dim StrSql As String
        Dim DsPrefisso As New DataSet

        'verifico se esiste blocco accreditamento in corso
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim BloccoAccreditamento As String
        StrSql = "Select valore From Configurazioni where Parametro='BLOCCO_ACCREDITAMENTO'"
        dtrgenerico = ClsServer.CreaDatareader(StrSql, conn)
        dtrgenerico.Read()
        BloccoAccreditamento = dtrgenerico("valore")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        VerificaPrefissiDocumentiAccreditamento = False
        'carico in un dt i prefissi dei documenti
        StrSql = "Select Prefisso,ConsentiDuranteBlocco From PrefissiEntiDocumenti where MOdalit‡Invio='Helios' "
        StrSql = StrSql & " AND (Albo is null or Albo = '" & Albo & "')"
        StrSql &= "and prefisso not in ('ATTOCOSTITUTIVO_', 'STATUTO_', 'DELIBERA_', 'CARTAIMPEGNOETICO_') "
        StrSql &= "and not prefisso like 'CV%' "
        DsPrefisso = ClsServer.DataSetGenerico(StrSql, conn)

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeFile = Mid(objPercorsoFile.Value, i, 1) & NomeFile
        Next
        Prefisso = Split(NomeFile, "_")
        'PrefissoFile = Prefisso & "_"
        If Prefisso.Length = 1 Then
            'errore
        Else
            For Each r As DataRow In DsPrefisso.Tables(0).Rows
                If UCase(r.Item("Prefisso")) = UCase(Prefisso(0).ToString) & "_" Then
                    If TipoUtente = "E" Then
                        If BloccoAccreditamento = "SI" And r.Item("ConsentiDuranteBlocco") = False Then
                            VerificaPrefissiDocumentiAccreditamento = False
                            blnBloccoAccreditamento = True
                        Else
                            PrefissoFile = UCase(Prefisso(0).ToString) & "_"
                            VerificaPrefissiDocumentiAccreditamento = True
                        End If
                    Else
                        PrefissoFile = UCase(Prefisso(0).ToString) & "_"
                        VerificaPrefissiDocumentiAccreditamento = True
                    End If

                    Exit For
                End If
            Next
        End If
        Return VerificaPrefissiDocumentiAccreditamento
    End Function
    Public Shared Function VerificaEstensioneFileAccreditamento(ByVal objPercorsoFile As HtmlInputFile) As Boolean
        'sono accettati solo documento con estensione .pdf e .pdf.p7m
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        'Dim strEstensione As String
        'Dim strEstensione1 As String

        

        VerificaEstensioneFileAccreditamento = False

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeFile = Mid(objPercorsoFile.Value, i, 1) & NomeFile
        Next
        'strEstensione = Mid(NomeFile, InStr(NomeFile, "."), Len(NomeFile))

        'strEstensione = Right(NomeFile, 4)
        'strEstensione1 = Right(NomeFile, 8)
        If UCase(Right(NomeFile, 4)) = ".PDF" Or UCase(Right(NomeFile, 8)) = ".PDF.P7M" Then

            Return True
        Else
            Return False
        End If





    End Function
    'Public Shared Function VerificaFirmaDocumentiEnte(ByVal objPercorsoFile As HtmlInputFile, ByVal PrefissoFile As String, ByRef cnLocal As SqlConnection) As Boolean
    '    Dim StrSql As String
    '    Dim StrSql1 As String
    '    Dim DsPrefisso As New DataSet
    '    Dim dtrgenerico As SqlClient.SqlDataReader
    '    Dim i As Integer
    '    Dim NomeFile As String = ""
    '    Dim Prefisso() As String
    '    Dim conn As SqlClient.SqlConnection = cnLocal
    '    Dim firma As Boolean
    '    Dim Reader As StreamReader
    '    Dim stringaDaRicercare As String = "<</Type/Sig/Filter/Adobe.PPKLite/SubFilter/adbe.pkcs7.sha1"
    '    Dim percorso As String
    '    StrSql1 = "Select Prefisso From PrefissiEntiDocumenti where MOdalit‡Invio='Helios' "
    '    DsPrefisso = ClsServer.DataSetGenerico(StrSql1, conn)
    '    Dim myPath As New System.Web.UI.Page

    '    For i = Len(objPercorsoFile.Value) To 1 Step -1
    '        If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
    '            Exit For
    '        End If
    '        NomeFile = Mid(objPercorsoFile.Value, i, 1) & NomeFile
    '    Next

    '    Prefisso = Split(NomeFile, "_")
    '    'PrefissoFile = Prefisso & "_"
    '    If Prefisso.Length = 1 Then
    '        'errore
    '    Else
    '        For Each riga As DataRow In DsPrefisso.Tables(0).Rows
    '            If riga.Item("Prefisso") = UCase(Prefisso(0).ToString) & "_" Then
    '                PrefissoFile = UCase(Prefisso(0).ToString) & "_"
    '                Exit For
    '            End If
    '        Next
    '    End If
    '    If UCase(Right(NomeFile, 4)) = ".PDF" Then
    '        StrSql = "Select FirmaNecessaria From PrefissiEntiDocumenti where Prefisso='" & PrefissoFile & "'"
    '        dtrgenerico = ClsServer.CreaDatareader(StrSql, conn)

    '        If dtrgenerico.HasRows = True Then
    '            dtrgenerico.Read()
    '            firma = dtrgenerico("FirmaNecessaria")
    '            If firma = True Then
    '                'devo controllare se esiste la firma all'interno del file
    '                Dim objreader As IO.StreamReader
    '                Dim testo As String

    '                percorso = myPath.Server.MapPath("upload") & "\" & NomeFile
    '                objPercorsoFile.PostedFile.SaveAs(percorso)
    '                'Dim fs As New FileStream(percorso, FileMode.Open, FileAccess.Read)
    '                objreader = New IO.StreamReader(percorso)
    '                testo = objreader.ReadToEnd

    '                If InStr(testo, stringaDaRicercare) <> 0 Then
    '                    objreader.Close()
    '                    If Not dtrgenerico Is Nothing Then
    '                        dtrgenerico.Close()
    '                        dtrgenerico = Nothing
    '                    End If
    '                    File.Delete((myPath.Server.MapPath("upload") & "\" & NomeFile))
    '                    Return True
    '                Else
    '                    objreader.Close()
    '                    File.Delete((myPath.Server.MapPath("upload") & "\" & NomeFile))
    '                    If Not dtrgenerico Is Nothing Then
    '                        dtrgenerico.Close()
    '                        dtrgenerico = Nothing
    '                    End If
    '                    Return False
    '                End If
    '            Else
    '                'firma non necessaria
    '                If Not dtrgenerico Is Nothing Then
    '                    dtrgenerico.Close()
    '                    dtrgenerico = Nothing
    '                End If
    '                'File.Delete((myPath.Server.MapPath("upload") & "\" & NomeFile))
    '                'Return False
    '            End If
    '        End If
    '    Else
    '        If Not dtrgenerico Is Nothing Then
    '            dtrgenerico.Close()
    '            dtrgenerico = Nothing
    '        End If
    '        Return False
    '    End If
    '    If Not dtrgenerico Is Nothing Then
    '        dtrgenerico.Close()
    '        dtrgenerico = Nothing
    '    End If
    'End Function

    Public Shared Function CaricaDocumentoAccreditamento(ByVal IdEnteFase As Integer, ByVal strUtente As String, ByVal objPercorsoFile As HtmlInputFile, ByVal IdEnte As Integer, ByRef cnLocal As SqlConnection, ByVal PrefissoFile As String) As String
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

        




        'BLOCCO CONTROLLO FIRMA ADC


        Dim StrSql2 As String
        Dim StrSql1 As String
        Dim DsPrefisso As New DataSet
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim Prefisso() As String
        Dim firma As Boolean
        'Dim stringaDaRicercare As String = "<</Type/Sig/Filter/Adobe.PPKLite/SubFilter/adbe.pkcs7.sha1"
        Dim stringaDaRicercare As String = "adbe.pkcs7"
        Dim stringaDaRicercare2 As String = "Adobe.PPKLite"
        'StrSql1 = "Select Prefisso From PrefissiEntiDocumenti where MOdalit‡Invio='Helios' "
        'DsPrefisso = ClsServer.DataSetGenerico(StrSql1, cnLocal)

       
        If UCase(Right(NomeUnivoco, 4)) = ".PDF" Then
            StrSql2 = "Select FirmaNecessaria From PrefissiEntiDocumenti where Prefisso='" & PrefissoFile & "'"
            dtrgenerico = ClsServer.CreaDatareader(StrSql2, cnLocal)

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                firma = dtrgenerico("FirmaNecessaria")
                If firma = True Then
                    'devo controllare se esiste la firma all'interno del file
                    Dim objreader As IO.StreamReader
                    Dim testo As String

                    'Dim fs As New FileStream(percorso, FileMode.Open, FileAccess.Read)
                    objreader = New IO.StreamReader(strPercorsoServer)
                    testo = objreader.ReadToEnd

                    If InStr(testo, stringaDaRicercare) <> 0 Or InStr(testo, stringaDaRicercare2) Then
                        objreader.Close()
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                    Else
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        msg = "Attenzione.Per questo documento Ë richiesta la firma digitale."
                        objreader.Close()
                        File.Delete(strPercorsoServer)

                        Return msg
                        'Exit Function
                        'messaggio di ritorno
                    End If
                Else
                'firma non necessaria
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

            End If
        End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'FINE BLOCCO CONTROLLO FIRMA

        Dim fs As New FileStream _
                        (strPercorsoServer, FileMode.Open, FileAccess.Read)
        Dim iLen As Integer = CInt(fs.Length - 1)
        'Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen < 0 Then
            msg = "Attenzione.Impossibile caricare un file vuoto."
        Else
            If iLen > 20971520 Then
                msg = "Attenzione.La dimensione massima Ë di 20 MB."
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
                Dim dtrHash As SqlDataReader

                dtrHash = ClsServer.CreaDatareader("Select HashValue from EntiDocumenti where IdEnteFase = " & IdEnteFase & " and  HashValue = '" & strHashValue & "'", cnLocal)
                If dtrHash.HasRows = True Then
                    dtrHash.Close()
                    dtrHash = Nothing
                    msg = "Attentione.Questo file Ë gi‡ presente."
                Else
                    dtrHash.Close()
                    dtrHash = Nothing
                    'controllo se l'hash value dei documenti di Accreditamento  Ë gi‡ presente  per lo stesso ente

                    STRSQL = " SELECT EntiDocumenti.IDEnteDocumento  " & _
                            " FROM  enti " & _
                            " INNER JOIN	EntiFasi ON enti.IDEnte = EntiFasi.IdEnte " & _
                            " INNER JOIN EntiDocumenti ON EntiFasi.IdEnteFase = EntiDocumenti.IdEnteFase " & _
                            " WHERE Enti.IDENTE = " & IdEnte & " AND HashValue='" & strHashValue & "' "

                    dtrHash = ClsServer.CreaDatareader(STRSQL, cnLocal)
                    If dtrHash.HasRows = True Then
                        dtrHash.Close()
                        dtrHash = Nothing
                        msg = "Questo file Ë stato gi‡ associato all' Ente."
                    Else

                        ''''prova ADC---------------------------------------
                        If Not dtrHash Is Nothing Then
                            dtrHash.Close()
                            dtrHash = Nothing
                        End If
                        InserimentoDocumento(IdEnteFase, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, cnLocal)
                        If Not dtrHash Is Nothing Then
                            dtrHash.Close()
                            dtrHash = Nothing
                        End If

                    End If
                End If
            End If
        End If

        If File.Exists(strPercorsoServer) Then
            File.Delete(strPercorsoServer)
        End If
        Return msg
    End Function
    Private Shared Sub InserimentoDocumento(ByVal IdEnteFase As Integer, ByVal NomeUnivoco As String, ByVal bBLOBStorage() As Byte, ByVal strUtente As String, ByVal strHashValue As String, ByVal cnLocal As SqlClient.SqlConnection)

        'sub che consente l'inserimento dei documenti da associare al progetto
        Dim cmd As SqlCommand = New SqlCommand _
        ("INSERT INTO EntiDocumenti (IdEnteFase,BinData, FileName,DataInserimento,UsernameInserimento,HashValue) " _
            & "VALUES(@IdEnteFase,@blob_data,@blob_filename ,getdate(),@utente,@hash_value   )", cnLocal)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("@IdEnteFase", SqlDbType.Int)
        cmd.Parameters("@IdEnteFase").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_filename", SqlDbType.VarChar)
        cmd.Parameters("@blob_filename").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
        cmd.Parameters("@blob_data").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@utente", SqlDbType.VarChar)
        cmd.Parameters("@utente").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@hash_value", SqlDbType.VarChar)
        cmd.Parameters("@hash_value").Direction = ParameterDirection.Input


        cmd.Parameters("@IdEnteFase").Value = IdEnteFase
        cmd.Parameters("@blob_filename").Value = NomeUnivoco
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue

        cmd.ExecuteNonQuery()
    End Sub
    Public Shared Function RecuperaDocumentoEnte(ByVal idEnteDocumento As Integer, ByRef cnLocal As SqlConnection) As HyperLink
        Dim da As New SqlDataAdapter _
            ("SELECT BinData,replace(Replace(Replace(Replace(FileName,'&',''),'#',''),'í',''),'+',' ') as FileName, HashValue FROM EntiDocumenti WHERE idEnteDocumento = " & idEnteDocumento, cnLocal)
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
                ds.Tables("_FileTest").Rows(0)("BinData")
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("Filename")
                oblLocalHLink.NavigateUrl = FileByteToPath(bBLOBStorage, ds.Tables("_FileTest").Rows(0)("Filename"), ds.Tables("_FileTest").Rows(0)("HashValue"))
            End If

            Return oblLocalHLink
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function
    Public Shared Function RecuperaDocumentoSigmaTXT(ByVal IdGenerazioneAllegato As Integer, ByRef cnLocal As SqlConnection) As HyperLink
        Dim da As New SqlDataAdapter _
            ("SELECT BinDataTXT,FileNameTXT FROM SIGMA_Generazione_File_Allegati WHERE IdGenerazioneAllegato = " & IdGenerazioneAllegato, cnLocal)
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
                ds.Tables("_FileTest").Rows(0)("BinDataTXT")
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("FileNameTXT")
                oblLocalHLink.NavigateUrl = FileByteToPath(bBLOBStorage, ds.Tables("_FileTest").Rows(0)("FileNameTXT"), "")
            End If

            Return oblLocalHLink
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function
    Public Shared Function RecuperaDocumentoSigmaCSV(ByVal IdGenerazioneAllegato As Integer, ByRef cnLocal As SqlConnection) As HyperLink
        Dim da As New SqlDataAdapter _
            ("SELECT BinDataCSV,FileNameCSV FROM SIGMA_Generazione_File_Allegati WHERE IdGenerazioneAllegato = " & IdGenerazioneAllegato, cnLocal)
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
                ds.Tables("_FileTest").Rows(0)("BinDataCSV")
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("FileNameCSV")
                oblLocalHLink.NavigateUrl = FileByteToPath(bBLOBStorage, ds.Tables("_FileTest").Rows(0)("FileNameCSV"), "")
            End If

            Return oblLocalHLink
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function
    Shared Sub RimuoviDocumentoEnte(ByVal IdEntedocumento As Integer, ByVal cnLocal As SqlConnection)

        Dim cb As New SqlCommand
        cb.CommandText = "Delete from EntiDocumenti WHERE IdEntedocumento = " & IdEntedocumento
        cb.Connection = cnLocal
        cb.ExecuteNonQuery()

    End Sub

    Private Sub cerca(ByVal testodacercare As String)
        Dim objreader As IO.StreamReader
        Dim testo As String

        objreader = New IO.StreamReader("c:\log.html")

        testo = objreader.ReadToEnd

        If InStr(testo, testodacercare) <> 0 Then
            MsgBox("Trovato!")
        End If

        objreader.Close()
    End Sub


    Public Shared Function RecuperaDocumentoSistemi(ByVal idEntitaDocumento As Integer, ByVal user As String, ByRef cnLocal As SqlConnection) As String
        Dim da As New SqlDataAdapter _
            ("SELECT BinData,replace(Replace(Replace(Replace(FileName,'&',''),'#',''),'í',''),'+',' ') as FileName, HashValue FROM EntiDocumenti WHERE idEnteDocumento = " & idEntitaDocumento, cnLocal)
        Dim cb As SqlCommandBuilder = New SqlCommandBuilder(da)
        Dim ds As New DataSet
        Dim mese As String
        Dim anno As String
        Dim data As String
        Dim nomefile As String

        'Dim user As String
        Dim paht As String
        Try
            Dim oblLocalHLink As New HyperLink

            da.Fill(ds, "_FileTest")
            Dim rw As DataRow
            rw = ds.Tables("_FileTest").Rows(0)

            ' Make sure you have some rows
            Dim i As Integer = ds.Tables("_FileTest").Rows.Count
            If i > 0 Then
                Dim bBLOBStorage() As Byte = _
                ds.Tables("_FileTest").Rows(0)("BinData")
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("Filename")
                nomefile = user & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now) & "_" & ds.Tables("_FileTest").Rows(0)("Filename")
                oblLocalHLink.NavigateUrl = FileByteToPath(bBLOBStorage, nomefile, ds.Tables("_FileTest").Rows(0)("HashValue"))

                paht = FileByteToPath(bBLOBStorage, nomefile, ds.Tables("_FileTest").Rows(0)("HashValue"))
            End If


            Return paht
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function
    Public Shared Function ControlloNomeFile(ByVal objNomeFile As HtmlInputFile, ByVal IdEnteFase As Integer, ByRef cnLocal As SqlConnection) As Boolean
      
        Dim dtrNomeFile As SqlDataReader
        Dim NomeFile As String
        Dim PercorsoFile As String = objNomeFile.PostedFile.FileName
        Dim ArrayPercorso As String()
        Dim i As Integer

        If Not dtrNomeFile Is Nothing Then
            dtrNomeFile.Close()
            dtrNomeFile = Nothing
        End If

        ArrayPercorso = Split(PercorsoFile, "\")
        i = UBound(ArrayPercorso)
        NomeFile = ArrayPercorso(i)

        dtrNomeFile = ClsServer.CreaDatareader("Select FileName from EntiDocumenti where IdEnteFase = " & IdEnteFase & " and  FileName = '" & NomeFile & "'", cnLocal)

        If dtrNomeFile.HasRows = True Then
            dtrNomeFile.Close()
            dtrNomeFile = Nothing
            'msg = "Attentione.Il Nome di questo file Ë gi‡ presente."
            Return True
        End If
        If Not dtrNomeFile Is Nothing Then
            dtrNomeFile.Close()
            dtrNomeFile = Nothing
        End If
    End Function

End Class
