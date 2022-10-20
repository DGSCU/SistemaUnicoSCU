
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



    'FUNZIONALITA': GESTIONE DEI DOCUMENTI ALL'INTERNO DI HELIOS

    Public Shared Function CaricaDocumentoProgetto(ByVal IdProgetto As Integer, ByVal strUtente As String, ByVal objPercorsoFile As HtmlInputFile, ByVal IdEntePresentante As Integer, ByRef cnLocal As SqlConnection, ByVal PrefissoFile As String) As String
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
        Dim stringaDaRicercare3 As String = "Adobe.PPKMS"
        'StrSql1 = "Select Prefisso From PrefissiEntiDocumenti where MOdalitàInvio='Helios' "
        'DsPrefisso = ClsServer.DataSetGenerico(StrSql1, cnLocal)

        StrSql2 = "Select FirmaNecessaria From PrefissiAttivitàDocumenti where Prefisso='" & PrefissoFile & "'"
        dtrgenerico = ClsServer.CreaDatareader(StrSql2, cnLocal)
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            firma = dtrgenerico("FirmaNecessaria")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        If UCase(Right(NomeUnivoco, 4)) = ".PDF" Then
            If firma = True Then
                'devo controllare se esiste la firma all'interno del file
                Dim objreader As IO.StreamReader
                Dim testo As String

                'Dim fs As New FileStream(percorso, FileMode.Open, FileAccess.Read)
                objreader = New IO.StreamReader(strPercorsoServer)
                testo = objreader.ReadToEnd

                If InStr(testo, stringaDaRicercare) <> 0 Or InStr(testo, stringaDaRicercare2) Or InStr(testo, stringaDaRicercare3) Then
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
                    msg = "Attenzione.Per questo documento è richiesta la firma digitale."
                    objreader.Close()
                    'File.Delete(strPercorsoServer)
                    If File.Exists(strPercorsoServer) Then
                        File.Delete(strPercorsoServer)
                    End If
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
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        If String.IsNullOrEmpty(msg) AndAlso firma Then
            '----- MV: 19/04/2022 CONTROLLO FIRME Rappresentante Legale o Coordinatore Responsabile
            Dim managerEnte = New clsEnte()

            'il rappresentante legale è obbligatorio
            Dim rappresentanteLegale = managerEnte.GetRappresentanteLegale(IdEntePresentante, cnLocal)
            If rappresentanteLegale Is Nothing Then
                msg = "Attenzione. Non è presente il Rappresentante Legale dell'Ente."
                If File.Exists(strPercorsoServer) Then
                    File.Delete(strPercorsoServer)
                End If
                Return msg
            End If
            Dim rapLegale As String = rappresentanteLegale.CodiceFiscale

            'il coordinatore responsabile del servizio civile universale NON è obbligatorio
            Dim coordinatore = managerEnte.GetCoordinatoreResponsabile(IdEntePresentante, cnLocal)
            Dim coord As String = String.Empty
            If Not coordinatore Is Nothing Then
                coord = coordinatore.CodiceFiscale
            End If

            'controllo che il documento sia firmato dal Rapprentante Legale o dal Coordinatore Responsabile del Servizio Civile Universale
            Dim _doc As Byte() = File.ReadAllBytes(strPercorsoServer)

            Dim sc As New SignChecker(_doc)

            If Not sc.checkSignature(rapLegale) AndAlso Not sc.checkSignature(coord) Then
                msg = "Attenzione. Il documento non è firmato dal Rappresentante Legale o dal Coordinatore Responsabile del Servizio Civile Universale dell'Ente."
                Return msg
            End If
            '----- FINE CONTROLLO FIRME Rappresentante Legale o Coordinatore Responsabile
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
                Dim dtrHash As SqlDataReader

                dtrHash = ClsServer.CreaDatareader("Select HashValue from AttivitàDocumenti where IdAttività = " & IdProgetto & " and  HashValue = '" & strHashValue & "'", cnLocal)
                If dtrHash.HasRows = True Then
                    dtrHash.Close()
                    dtrHash = Nothing
                    msg = "Attenzione.Questo file è già presente nel progetto."
                Else
                    dtrHash.Close()
                    dtrHash = Nothing
                    'controllo se l'hash value dei documenti PROG/PROGGG è già presente nei progetti per lo stesso ente

                    STRSQL = " SELECT AttivitàDocumenti.IDAttivitàDocumento  " & _
                             " FROM  enti " & _
                             " INNER JOIN	attività ON enti.IDEnte = attività.IDEntePresentante " & _
                             " INNER JOIN AttivitàDocumenti ON attività.IDAttività = AttivitàDocumenti.IdAttività " & _
                             " WHERE IDENTE = " & IdEntePresentante & "  AND (LEFT(FileName, 5) = 'PROG_' OR LEFT(FileName, 7) = 'PROGGG_')  " & _
                             " AND HashValue='" & strHashValue & "' "

                    dtrHash = ClsServer.CreaDatareader(STRSQL, cnLocal)
                    If dtrHash.HasRows = True Then
                        dtrHash.Close()
                        dtrHash = Nothing
                        msg = "Questo file è stato già associato ad un'altro progetto dell'Ente."
                    Else
                        dtrHash.Close()
                        dtrHash = Nothing
                        'controllo se nel progetto è stato già caricato un file tipo PROG_ 
                        STRSQL = " SELECT  LEFT(FileName,4) as PrefissoFile,  IdAttivitàDocumento " & _
                                 " FROM  AttivitàDocumenti " & _
                                 " WHERE (LEFT(FileName, 5) = 'PROG_' OR (LEFT(FileName, 7) = 'PROGGG_')) and IdAttività = " & IdProgetto & " "
                        '' AND HashValue<> '" & strHashValue & "' "

                        dtrHash = ClsServer.CreaDatareader(STRSQL, cnLocal)
                        If dtrHash.HasRows = True Then
                            dtrHash.Read()
                            If dtrHash("PrefissoFile") = Left(PrefissoFile, 4) Then
                                dtrHash.Close()
                                dtrHash = Nothing
                                msg = "E' stato già associato un file di tipo SCHEDA PROGETTO al progetto."

                            Else

                                dtrHash.Close()
                                dtrHash = Nothing
                                InserimentoDocumento(IdProgetto, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, cnLocal)
                            End If
                        Else
                            If Not dtrHash Is Nothing Then
                                dtrHash.Close()
                                dtrHash = Nothing
                            End If

                            InserimentoDocumento(IdProgetto, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, cnLocal)

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

    Public Shared Function CaricaDocumentoProgramma(ByVal IdProgramma As Integer, ByVal strUtente As String, ByVal objPercorsoFile As HtmlInputFile, ByVal IdEntePresentante As Integer, ByRef cnLocal As SqlConnection, ByVal PrefissoFile As String) As String
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
        Dim stringaDaRicercare3 As String = "Adobe.PPKMS"

        'StrSql1 = "Select Prefisso From PrefissiEntiDocumenti where MOdalitàInvio='Helios' "
        'DsPrefisso = ClsServer.DataSetGenerico(StrSql1, cnLocal)

        StrSql2 = "Select FirmaNecessaria From PrefissiProgrammiDocumenti where Prefisso='" & PrefissoFile & "'"
        dtrgenerico = ClsServer.CreaDatareader(StrSql2, cnLocal)
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            firma = dtrgenerico("FirmaNecessaria")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        If UCase(Right(NomeUnivoco, 4)) = ".PDF" Then
            If firma = True Then
                'devo controllare se esiste la firma all'interno del file
                Dim objreader As IO.StreamReader
                Dim testo As String

                'Dim fs As New FileStream(percorso, FileMode.Open, FileAccess.Read)
                objreader = New IO.StreamReader(strPercorsoServer)
                testo = objreader.ReadToEnd

                If InStr(testo, stringaDaRicercare) <> 0 Or InStr(testo, stringaDaRicercare2) Or InStr(testo, stringaDaRicercare3) Then
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
                    msg = "Attenzione.Per questo documento è richiesta la firma digitale."
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
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        If String.IsNullOrEmpty(msg) AndAlso firma Then
            '----- MV: 19/04/2022 CONTROLLO FIRME Rappresentante Legale o Coordinatore Responsabile
            Dim managerEnte = New clsEnte()

            'il rappresentante legale è obbligatorio
            Dim rappresentanteLegale = managerEnte.GetRappresentanteLegale(IdEntePresentante, cnLocal)
            If rappresentanteLegale Is Nothing Then
                msg = "Attenzione. Non è presente il Rappresentante Legale dell'Ente."
                Return msg
            End If
            Dim rapLegale As String = rappresentanteLegale.CodiceFiscale

            'il coordinatore responsabile del servizio civile universale NON è obbligatorio
            Dim coordinatore = managerEnte.GetCoordinatoreResponsabile(IdEntePresentante, cnLocal)
            Dim coord As String = String.Empty
            If Not coordinatore Is Nothing Then
                coord = coordinatore.CodiceFiscale
            End If

            'controllo che il documento sia firmato dal Rapprentante Legale o dal Coordinatore Responsabile del Servizio Civile Universale
            Dim _doc As Byte() = File.ReadAllBytes(strPercorsoServer)
            Dim sc As New SignChecker(_doc)
            If Not sc.checkSignature(rapLegale) AndAlso Not sc.checkSignature(coord) Then
                msg = "Attenzione. Il documento non è firmato dal Rappresentante Legale o dal Coordinatore Responsabile del Servizio Civile Universale dell'Ente."
                Return msg
            End If
            '----- FINE CONTROLLO FIRME Rappresentante Legale o Coordinatore Responsabile
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
                Dim dtrHash As SqlDataReader

                dtrHash = ClsServer.CreaDatareader("Select HashValue from ProgrammiDocumenti where IdProgramma = " & IdProgramma & " and  HashValue = '" & strHashValue & "'", cnLocal)
                If dtrHash.HasRows = True Then
                    dtrHash.Close()
                    dtrHash = Nothing
                    msg = "Attenzione.Questo file è già presente nel progetto."
                Else
                    dtrHash.Close()
                    dtrHash = Nothing
                    'controllo se l'hash value dei documenti PROG/PROGGG è già presente nei progetti per lo stesso ente

                    STRSQL = " SELECT ProgrammiDocumenti.IdProgrammaDocumento  " & _
                             " FROM  enti " & _
                             " INNER JOIN	programmi ON enti.IDEnte = programmi.IDEnteProponente " & _
                             " INNER JOIN ProgrammiDocumenti ON programmi.idprogramma = ProgrammiDocumenti.IdProgramma " & _
                             " WHERE IDENTE = " & IdEntePresentante & "  AND (LEFT(FileName, 10) = 'PROGRAMMA_' )  " & _
                             " AND HashValue='" & strHashValue & "' "

                    dtrHash = ClsServer.CreaDatareader(STRSQL, cnLocal)
                    If dtrHash.HasRows = True Then
                        dtrHash.Close()
                        dtrHash = Nothing
                        msg = "Questo file è stato già associato ad un'altro programma dell'Ente."
                    Else
                        dtrHash.Close()
                        dtrHash = Nothing
                        'controllo se nel progetto è stato già caricato un file tipo PROG_ 
                        STRSQL = " SELECT  LEFT(FileName,9) as PrefissoFile,  IdProgrammaDocumento " & _
                                 " FROM  ProgrammiDocumenti " & _
                                 " WHERE (LEFT(FileName, 10) = 'PROGRAMMA_' ) and IdProgramma = " & IdProgramma & " "
                        '' AND HashValue<> '" & strHashValue & "' "

                        dtrHash = ClsServer.CreaDatareader(STRSQL, cnLocal)
                        If dtrHash.HasRows = True Then
                            dtrHash.Read()
                            If dtrHash("PrefissoFile") = Left(PrefissoFile, 9) Then
                                dtrHash.Close()
                                dtrHash = Nothing
                                msg = "E' stato già associato un file di tipo SCHEDA PROGRAMMA al progetto."

                            Else

                                dtrHash.Close()
                                dtrHash = Nothing
                                InserimentoDocumentoProgramma(IdProgramma, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, cnLocal)
                            End If
                        Else
                            If Not dtrHash Is Nothing Then
                                dtrHash.Close()
                                dtrHash = Nothing
                            End If

                            InserimentoDocumentoProgramma(IdProgramma, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, cnLocal)

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

    Public Shared Sub CaricaDocumentoProgettoBOX(ByVal IdProgetto As Integer, ByVal strUtente As String, ByVal NomeFileBOX As String, ByVal PercorsoFileBOX As String, ByRef cnLocal As SqlConnection)
        'Dim NomeUnivoco As String = ""
        Dim strPercorsoServer As String
        Dim i As Integer
        Dim myPath As New System.Web.UI.Page

        'strPercorsoServer = myPath.Server.MapPath("upload") & "\" & NomeFileBOX

        'If File.Exists(strPercorsoServer) Then
        '    File.Delete(strPercorsoServer)
        'End If
        'File.Copy(PercorsoFileBOX, strPercorsoServer)
        '' objPercorsoFile.PostedFile.SaveAs(strPercorsoServer)

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

    Public Shared Sub BUTTACaricaDocumentoProgetto(ByVal IdProgetto As Integer, ByVal strUtente As String, ByVal objPercorsoFile As HtmlInputFile, ByRef cnLocal As SqlConnection)
        Dim NomeUnivoco As String = ""
        Dim strPercorsoServer As String
        Dim i As Integer
        Dim myPath As New System.Web.UI.Page

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeUnivoco = Mid(objPercorsoFile.Value, i, 1) & NomeUnivoco
        Next
        strPercorsoServer = myPath.Server.MapPath("upload") & "\" & NomeUnivoco
        If File.Exists(strPercorsoServer) Then
            File.Delete(strPercorsoServer)
        End If
        objPercorsoFile.PostedFile.SaveAs(strPercorsoServer)

        Dim fs As New FileStream _
                (strPercorsoServer, FileMode.OpenOrCreate, FileAccess.Read)
        Dim iLen As Integer = CInt(fs.Length - 1)
        Dim bBLOBStorage(iLen) As Byte
        fs.Read(bBLOBStorage, 0, iLen)
        fs.Close()

        Dim strHashValue As String
        strHashValue = GeneraHash(bBLOBStorage)

        Dim cmd As SqlCommand
        Dim STRSQL As String
        cmd = New SqlCommand
        cmd.Connection = cnLocal
        cmd.CommandText = "INSERT INTO AttivitàDocumenti (IdAttività,BinData, FileName,DataInserimento,UsernameInserimento,HashValue) " _
                        & "VALUES(@idattivita,@blob_data,@blob_filename ,getdate(),@utente,@hash_value)  "
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

        For i = 1 To 4000
            cmd.Parameters("@idattivita").Value = IdProgetto
            cmd.Parameters("@blob_filename").Value = i & "_" & NomeUnivoco
            cmd.Parameters("@blob_data").Value = bBLOBStorage
            cmd.Parameters("@utente").Value = strUtente
            cmd.Parameters("@hash_value").Value = strHashValue

            cmd.ExecuteNonQuery()
        Next

    End Sub


    Public Shared Function RecuperaDocumentoProgetto(ByVal idAttivitaDocumento As Integer, ByRef cnLocal As SqlConnection) As HyperLink
        Dim da As New SqlDataAdapter _
            ("SELECT BinData,replace(Replace(Replace(Replace(FileName,'&',''),'#',''),'’',''),'+',' ') as FileName, HashValue FROM AttivitàDocumenti WHERE idAttivitàDocumento = " & idAttivitaDocumento, cnLocal)
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

    Public Shared Function RecuperaDocumentoProgramma(ByVal idProgrammaDocumento As Integer, ByRef cnLocal As SqlConnection) As HyperLink
        Dim da As New SqlDataAdapter _
            ("SELECT BinData,replace(Replace(Replace(Replace(FileName,'&',''),'#',''),'’',''),'+',' ') as FileName, HashValue FROM ProgrammiDocumenti WHERE idProgrammaDocumento = " & idProgrammaDocumento, cnLocal)
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

    Private Shared Function FileByteToPath(ByVal dataBuffer As Byte(), ByVal nomeFile As String, ByVal HashValue As String) As String
        'dichiaro una variabile byte che bufferizza (carica in memoria) il file template richiesto
        'e trasformato in base64
        Dim fs As FileStream
        Dim myPath As New System.Web.UI.Page

        If File.Exists(myPath.Server.MapPath("download") & "\" & HashValue & "_" & nomeFile) Then
            File.Delete(myPath.Server.MapPath("download") & "\" & HashValue & "_" & nomeFile)
        End If
        'nomeFile = nomeFile.Replace("#", "").Replace("&", "")
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

    Shared Sub RimuoviDocumentoProgetto(ByVal IdAttivitadocumento As Integer, ByVal cnLocal As SqlConnection)

        Dim cb As New SqlCommand
        cb.CommandText = "Delete from AttivitàDocumenti WHERE IdAttivitàdocumento = " & IdAttivitadocumento
        cb.Connection = cnLocal
        cb.ExecuteNonQuery()

    End Sub

    Shared Sub RimuoviDocumentoProgramma(ByVal IdProgrammadocumento As Integer, ByVal cnLocal As SqlConnection)

        Dim cb As New SqlCommand
        cb.CommandText = "Delete from ProgrammiDocumenti WHERE IdProgrammadocumento = " & IdProgrammadocumento
        cb.Connection = cnLocal
        cb.ExecuteNonQuery()

    End Sub

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

    Public Shared Function VerificaPrefissiDocumenti(ByVal objPercorsoFile As HtmlInputFile, ByVal conn As SqlClient.SqlConnection, ByRef PrefissoFile As String, ByVal Sistema As String, ByVal IdProgetto As Integer) As Boolean
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        Dim StrSql As String
        Dim DsPrefisso As New DataSet
        VerificaPrefissiDocumenti = False
        'carico in un dt i prefissi dei documenti
        If Sistema = "Helios" Then
            StrSql = "Select Prefisso From PrefissiAttivitàDocumenti where MOdalitàInvio='Helios' and prefisso <> 'PROGGG_' "
        Else
            StrSql = "Select Prefisso From PrefissiAttivitàDocumenti where MOdalitàInvio='Helios' and prefisso <> 'PROG_' "
        End If

        StrSql = StrSql & " and idprefisso in (select idprefisso from AssociaPrefissiTipiProgetto a inner join attività b on a.idtipoprogetto = b.idtipoprogetto where b.idattività = " & IdProgetto & ")"

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
                If r.Item("Prefisso") = UCase(Prefisso(0).ToString) & "_" Then
                    PrefissoFile = UCase(Prefisso(0).ToString) & "_"
                    VerificaPrefissiDocumenti = True
                    Exit For
                End If
            Next
        End If
        Return VerificaPrefissiDocumenti
    End Function

    Public Shared Function VerificaPrefissiDocumentiProgramma(ByVal objPercorsoFile As HtmlInputFile, ByVal conn As SqlClient.SqlConnection, ByRef PrefissoFile As String, ByVal Sistema As String) As Boolean
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        Dim StrSql As String
        Dim DsPrefisso As New DataSet
        VerificaPrefissiDocumentiProgramma = False
        'carico in un dt i prefissi dei documenti
        If Sistema = "Helios" Then
            StrSql = "Select Prefisso From PrefissiProgrammiDocumenti where MOdalitàInvio='Helios' and prefisso <> 'PROGGG_' "
        Else
            StrSql = "Select Prefisso From PrefissiProgrammiDocumenti where MOdalitàInvio='Helios' and prefisso <> 'PROG_' "
        End If

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
                If r.Item("Prefisso") = UCase(Prefisso(0).ToString) & "_" Then
                    PrefissoFile = UCase(Prefisso(0).ToString) & "_"
                    VerificaPrefissiDocumentiProgramma = True
                    Exit For
                End If
            Next
        End If
        Return VerificaPrefissiDocumentiProgramma
    End Function

    Public Shared Function VerificaEstensioneFile(ByVal objPercorsoFile As HtmlInputFile) As Boolean
        'sono accettati solo documento con estensione .pdf e .pdf.p7m
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        'Dim strEstensione As String
        'Dim strEstensione1 As String
        VerificaEstensioneFile = False

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

    Private Shared Sub InserimentoDocumento(ByVal IdProgetto As Integer, ByVal NomeUnivoco As String, ByVal bBLOBStorage() As Byte, ByVal strUtente As String, ByVal strHashValue As String, ByVal cnLocal As SqlClient.SqlConnection)

        'sub che consente l'inserimento dei documenti da associare al progetto
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
        cmd.Parameters("@blob_filename").Value = NomeUnivoco
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue

        cmd.ExecuteNonQuery()
    End Sub

    Private Shared Sub InserimentoDocumentoProgramma(ByVal IdProgramma As Integer, ByVal NomeUnivoco As String, ByVal bBLOBStorage() As Byte, ByVal strUtente As String, ByVal strHashValue As String, ByVal cnLocal As SqlClient.SqlConnection)

        'sub che consente l'inserimento dei documenti da associare al progetto
        Dim cmd As SqlCommand = New SqlCommand _
        ("INSERT INTO ProgrammiDocumenti (IdProgramma,BinData, FileName,DataInserimento,UsernameInserimento,HashValue) " _
            & "VALUES(@idprogramma,@blob_data,@blob_filename ,getdate(),@utente,@hash_value   )", cnLocal)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("@idprogramma", SqlDbType.Int)
        cmd.Parameters("@idprogramma").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_filename", SqlDbType.VarChar)
        cmd.Parameters("@blob_filename").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
        cmd.Parameters("@blob_data").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@utente", SqlDbType.VarChar)
        cmd.Parameters("@utente").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@hash_value", SqlDbType.VarChar)
        cmd.Parameters("@hash_value").Direction = ParameterDirection.Input


        cmd.Parameters("@idprogramma").Value = IdProgramma
        cmd.Parameters("@blob_filename").Value = NomeUnivoco
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue

        cmd.ExecuteNonQuery()
    End Sub

    Public Shared Sub Verifica_CancellaBOX(ByVal IdBandoAttivita As Integer, ByVal cnLocal As SqlClient.SqlConnection)
        'verifico se è stato generato un box 16o19 
        Dim strSql As String
        Dim dtsVer As DataSet
        Dim strIdAttiDocumento As String
        Dim cmd As SqlCommand = New SqlCommand
        strSql = " SELECT AttivitàDocumenti.IDAttivitàDocumento, LEFT(AttivitàDocumenti.FileName, 6) as TipoDocumento"
        strSql &= " FROM enti"
        strSql &= " INNER JOIN attività ON enti.IDEnte = attività.IDEntePresentante "
        strSql &= " INNER JOIN AttivitàDocumenti ON attività.IDAttività = AttivitàDocumenti.IdAttività "
        strSql &= " INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività"
        strSql &= " WHERE LEFT(AttivitàDocumenti.FileName, 3) = 'BOX'  AND"
        strSql &= " BandiAttività.IdBandoAttività = " & IdBandoAttivita
        dtsVer = ClsServer.DataSetGenerico(strSql, cnLocal)
        cmd.Connection = cnLocal
        For Each r As DataRow In dtsVer.Tables(0).Rows
            RimuoviDocumentoProgetto(r.Item("IDAttivitàDocumento"), cnLocal)
        Next
    End Sub

    Public Shared Function VerificaPrefissiDocumentiPresenze(ByVal objPercorsoFile As HtmlInputFile, ByVal conn As SqlClient.SqlConnection, ByRef PrefissoFile As String) As Boolean
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        Dim StrSql As String
        Dim DsPrefisso As New DataSet
        VerificaPrefissiDocumentiPresenze = False
        'carico in un dt i prefissi dei documenti
        StrSql = "Select Prefisso From PrefissiEntitàDocumenti where ModalitàInvio='FUTURO' and TipoInserimento=1 "
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
                If r.Item("Prefisso") = UCase(Prefisso(0).ToString) & "_" Then
                    PrefissoFile = UCase(Prefisso(0).ToString) & "_"
                    VerificaPrefissiDocumentiPresenze = True
                    Exit For
                End If
            Next
        End If
        Return VerificaPrefissiDocumentiPresenze
    End Function
    Public Shared Function CaricaDocumentoPresenzeEntità(ByVal IdEntità As Integer, ByVal strUtente As String, ByVal objPercorsoFile As HtmlInputFile, ByVal dataOdierna As Date, ByVal dataPresenza As Date, ByRef cnLocal As SqlConnection, ByVal PrefissoFile As String) As String
        Dim NomeUnivoco As String = ""
        Dim strPercorsoServer As String
        Dim i As Integer
        Dim myPath As New System.Web.UI.Page
        Dim msg As String = ""
        Dim anno As String
        Dim mese As String
        mese = Mid(dataOdierna, 4, 2)
        anno = Mid(dataOdierna, 7, 4)
        Dim annoPresenza As String
        Dim mesePresenza As String
        mesePresenza = Mid(dataPresenza, 4, 2)
        annoPresenza = Mid(dataPresenza, 7, 4)

        Dim dataattuale As Date
        Dim datapresenzaConfronto As Date

        dataattuale = "01" & "/" & mese & "/" & anno
        dataattuale = dataattuale.ToString("dd/MM/yyyy")

        datapresenzaConfronto = "01" & "/" & mesePresenza & " / " & annoPresenza
        datapresenzaConfronto = datapresenzaConfronto.ToString("dd/MM/yyyy")
        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeUnivoco = Mid(objPercorsoFile.Value, i, 1) & NomeUnivoco
        Next

        Dim strNomeFile As String = annoPresenza & mesePresenza & "_" & strUtente

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
            File.Delete(strPercorsoServer)
            msg = "Attenzione.Impossibile caricare un file vuoto."
        Else
            If iLen > 20971520 Then
                File.Delete(strPercorsoServer)
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
                Dim dtrHash As SqlDataReader

                dtrHash = ClsServer.CreaDatareader("Select HashValue from EntitàDocumenti where IdEntità = " & IdEntità & " and  HashValue = '" & strHashValue & "'", cnLocal)
                If dtrHash.HasRows = True Then
                    dtrHash.Close()

                    dtrHash = Nothing
                    File.Delete(strPercorsoServer)
                    msg = "Attentione.Questo file è già presente."

                Else
                    dtrHash.Close()
                    dtrHash = Nothing

                    If datapresenzaConfronto < dataattuale Then 'bbgigyfgyfy7ft7ft7ft7iftf7tftfitfitfcitfctif
                        InserimentoDocumentoPresenza(IdEntità, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, annoPresenza, mesePresenza, cnLocal)
                    Else
                        File.Delete(strPercorsoServer)
                        msg = "Impossibile Inserire le Presenze PER IL MESE ATTUALE O SUCCESSIVO." 'chiaro come messaggio?  :-)))))
                    End If


                End If
            End If


        End If

        If File.Exists(strPercorsoServer) Then
            File.Delete(strPercorsoServer)
        End If

        Return msg
    End Function
    Private Shared Sub InserimentoDocumentoPresenza(ByVal IdEntità As Integer, ByVal NomeUnivoco As String, ByVal bBLOBStorage() As Byte, ByVal strUtente As String, ByVal strHashValue As String, ByVal anno As String, ByVal mese As String, ByVal cnLocal As SqlClient.SqlConnection)

        'sub che consente l'inserimento dei documenti da associare al progetto
        Dim cmd As SqlCommand = New SqlCommand _
        ("INSERT INTO EntitàDocumenti (IdEntità,BinData, FileName,DataInserimento,UsernameInserimento,HashValue,anno , mese) " _
            & "VALUES(@idEntità,@blob_data,@blob_filename ,getdate(),@utente,@hash_value,@anno,@mese)", cnLocal)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("@idEntità", SqlDbType.Int)
        cmd.Parameters("@idEntità").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_filename", SqlDbType.VarChar)
        cmd.Parameters("@blob_filename").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
        cmd.Parameters("@blob_data").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@utente", SqlDbType.VarChar)
        cmd.Parameters("@utente").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@hash_value", SqlDbType.VarChar)
        cmd.Parameters("@hash_value").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@anno", SqlDbType.SmallInt)
        cmd.Parameters("@anno").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@mese", SqlDbType.SmallInt)
        cmd.Parameters("@mese").Direction = ParameterDirection.Input


        cmd.Parameters("@idEntità").Value = IdEntità
        cmd.Parameters("@blob_filename").Value = NomeUnivoco
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue
        cmd.Parameters("@anno").Value = anno
        cmd.Parameters("@mese").Value = mese

        cmd.ExecuteNonQuery()
        cmd.Dispose()
        cmd = Nothing
    End Sub

    Public Shared Function RecuperaDocumentoVolontario(ByVal idEntitaDocumento As Integer, ByVal user As String, ByRef cnLocal As SqlConnection) As String
        Dim da As New SqlDataAdapter _
            ("SELECT BinData,replace(Replace(Replace(Replace(FileName,'&',''),'#',''),'’',''),'+',' ') as FileName, HashValue, mese, anno, usernameinserimento FROM EntitàDocumenti WHERE idEntitàDocumento = " & idEntitaDocumento, cnLocal)
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
                'anno = ds.Tables("_FileTest").Rows(0)("anno")
                ' If Len(ds.Tables("_FileTest").Rows(0)("mese")).ToString = 1 Then
                'mese = "0" & ds.Tables("_FileTest").Rows(0)("mese")
                'Else
                'mese = ds.Tables("_FileTest").Rows(0)("mese").ToString
                'End If
                'If mese.Length = 1 Then
                '    mese = "0" & mese

                'End If
                ' user = ds.Tables("_FileTest").Rows(0)("usernameinserimento")
                'data = Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("Filename")
                nomefile = user & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now) & "_" & ds.Tables("_FileTest").Rows(0)("Filename")
                oblLocalHLink.NavigateUrl = FileByteToPathPresenze(bBLOBStorage, nomefile, ds.Tables("_FileTest").Rows(0)("HashValue"))

                paht = FileByteToPathPresenze(bBLOBStorage, nomefile, ds.Tables("_FileTest").Rows(0)("HashValue"))
            End If


            Return paht
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function
    Private Shared Function FileByteToPathPresenze(ByVal dataBuffer As Byte(), ByVal nomeFile As String, ByVal HashValue As String) As String
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
    Public Shared Function ControlloPresenzeConfermate(ByVal idente As Integer, ByVal datarichiesta As Date, ByRef cnLocal As SqlConnection) As Boolean
        Dim dtrreader As SqlDataReader
        Dim STRSQL As String
        Dim anno As String
        Dim mese As String
        mese = Mid(datarichiesta, 4, 2)
        anno = Mid(datarichiesta, 7, 4)
        STRSQL = "select Mese, Anno from EntiConfermaPresenze where (Anno=" & anno & ") and IdEnte='" & idente & "' and (Mese=" & mese & ") "
        dtrreader = ClsServer.CreaDatareader(STRSQL, cnLocal)

        If dtrreader.HasRows = True Then
            dtrreader.Close()
            dtrreader = Nothing
            Return False
        Else
            dtrreader.Close()
            dtrreader = Nothing
            Return True
        End If
        dtrreader.Close()
        dtrreader = Nothing
    End Function

    Shared Sub RimuoviPresenze(ByVal IdEntitadocumento As Integer, ByVal cnLocal As SqlConnection)

        Dim cb As New SqlCommand
        cb.CommandText = "Delete from EntitàDocumenti WHERE IdEntitàdocumento = " & IdEntitadocumento
        cb.Connection = cnLocal
        cb.ExecuteNonQuery()
        cb.Dispose()
        cb = Nothing
    End Sub

    Public Shared Function VerificaPrefissiDocumentoVolontario(ByVal objPercorsoFile As HtmlInputFile, ByVal conn As SqlClient.SqlConnection, ByRef PrefissoFile As String, ByRef Sistema As String) As Boolean
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        Dim StrSql As String
        Dim DsPrefisso As New DataSet
        VerificaPrefissiDocumentoVolontario = False
        'carico in un dt i prefissi dei documenti
        StrSql = "select prefisso from prefissientitàdocumenti where tipoinserimento=0 and Sistema='" & Sistema & "'"
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
                If r.Item("Prefisso") = UCase(Prefisso(0).ToString) & "_" Then
                    PrefissoFile = UCase(Prefisso(0).ToString) & "_"
                    VerificaPrefissiDocumentoVolontario = True
                    Exit For
                End If
            Next
        End If
        Return VerificaPrefissiDocumentoVolontario
    End Function

    Public Shared Function VerificaEstensioneFileVolontario(ByVal objPercorsoFile As HtmlInputFile) As Boolean
        'sono accettati solo documento con estensione .pdf e .pdf.p7m
        Dim NomeFile As String = ""
        'Dim Prefisso() As String
        Dim i As Integer
        'Dim strEstensione As String
        'Dim strEstensione1 As String
        VerificaEstensioneFileVolontario = False

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

    Public Shared Function CaricaDocumentoEntità(ByVal IdEntità As Integer, ByVal strUtente As String, ByVal objPercorsoFile As HtmlInputFile, ByRef cnLocal As SqlConnection, ByVal PrefissoFile As String) As String
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

                'richamo store che fa i controlli su documento

                msg = ControlloDocumentiVolontario(IdEntità, PrefissoFile, strHashValue, cnLocal)
                If msg = "ok" Then
                    InserimentoDocumentoVolontario(IdEntità, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, cnLocal)
                    msg = "ok"
                End If

            End If
        End If

        If File.Exists(strPercorsoServer) Then
            File.Delete(strPercorsoServer)
        End If
        Return msg
    End Function
    Public Shared Function ControlloDocumentiVolontario(ByVal IdEntità As Integer, TipoDocumento As String, HashValue As String, ByRef cnLocal As SqlConnection) As String

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  24/03/2015
        'FUNZIONALITA': RICHIAMO STORE PER IL CONTROLLO DEI DOCUMENTI DEL VOLONTARIO

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_VOLONTARI_CONTROLLO_DOCUMENTI]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, cnLocal)
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEntità", SqlDbType.VarChar).Value = IdEntità
            sqlCMD.Parameters.Add("@TipoDocumento", SqlDbType.VarChar).Value = TipoDocumento
            sqlCMD.Parameters.Add("@HashValue", SqlDbType.VarChar).Value = HashValue

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


            str = sqlCMD.Parameters("@Messaggio").Value

            Return str



        Catch ex As Exception

            Exit Function
        End Try

    End Function

    Public Shared Sub VolontarioVerificaPrefisso(ByVal IdEntità As Integer, PrefissoDocumento As String, ByRef cnLocal As SqlConnection, ByRef Esito As String, ByRef Messaggio As String)

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  04/05/2016
        'FUNZIONALITA': RICHIAMO STORE PER LA VERIFICA DEI PREFISSI

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_VOLONTARI_VERIFICA_PREFISSI]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, cnLocal)
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.VarChar).Value = IdEntità
            sqlCMD.Parameters.Add("@PrefissoDocumento", SqlDbType.VarChar).Value = PrefissoDocumento

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
            sparam2.Size = 200
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()


            Esito = sqlCMD.Parameters("@Esito").Value
            Messaggio = sqlCMD.Parameters("@Messaggio").Value

        Catch ex As Exception

            Exit Sub
        End Try

    End Sub

    Private Shared Sub InserimentoDocumentoVolontario(ByVal IdEntità As Integer, ByVal NomeUnivoco As String, ByVal bBLOBStorage() As Byte, ByVal strUtente As String, ByVal strHashValue As String, ByVal cnLocal As SqlClient.SqlConnection)

        'sub che consente l'inserimento dei documenti da associare al progetto
        Dim cmd As SqlClient.SqlCommand = New SqlCommand _
        ("INSERT INTO EntitàDocumenti (IdEntità,BinData, FileName,DataInserimento,UsernameInserimento,HashValue) " _
            & "VALUES(@IdEntità,@blob_data,@blob_filename ,getdate(),@utente,@hash_value   )", cnLocal)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("@IdEntità", SqlDbType.Int)
        cmd.Parameters("@IdEntità").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_filename", SqlDbType.VarChar)
        cmd.Parameters("@blob_filename").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
        cmd.Parameters("@blob_data").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@utente", SqlDbType.VarChar)
        cmd.Parameters("@utente").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@hash_value", SqlDbType.VarChar)
        cmd.Parameters("@hash_value").Direction = ParameterDirection.Input


        cmd.Parameters("@IdEntità").Value = IdEntità
        cmd.Parameters("@blob_filename").Value = NomeUnivoco
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue

        cmd.ExecuteNonQuery()
    End Sub
    Public Shared Function RecuperaDocumentoPresenzeFormatori(ByVal idDoc As Integer, ByRef cnLocal As SqlConnection) As String
        Dim da As New SqlDataAdapter _
            ("SELECT BinData,FileName, HashValue, usernameinserimento FROM AttivitàDocumentiFormazione WHERE IdAttivitàDocumentoFormazione = " & idDoc, cnLocal)
        Dim cb As SqlCommandBuilder = New SqlCommandBuilder(da)
        Dim ds As New DataSet

        Dim user As String
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
                'anno = ds.Tables("_FileTest").Rows(0)("anno")
                ' If Len(ds.Tables("_FileTest").Rows(0)("mese")).ToString = 1 Then
                'mese = "0" & ds.Tables("_FileTest").Rows(0)("mese")
                'Else
                'mese = ds.Tables("_FileTest").Rows(0)("mese").ToString
                'End If
                'If mese.Length = 1 Then
                '    mese = "0" & mese

                'End If
                user = ds.Tables("_FileTest").Rows(0)("usernameinserimento")
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("Filename")
                oblLocalHLink.NavigateUrl = FileByteToPathPresenze(bBLOBStorage, user & "_" & ds.Tables("_FileTest").Rows(0)("Filename"), ds.Tables("_FileTest").Rows(0)("HashValue"))

                paht = FileByteToPathPresenze(bBLOBStorage, user & "_" & ds.Tables("_FileTest").Rows(0)("Filename"), ds.Tables("_FileTest").Rows(0)("HashValue"))
            End If


            Return paht
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function
    Shared Sub RimuoviDocumentoPresenzeFormatori(ByVal IdDoc As Integer, ByVal cnLocal As SqlConnection)

        Dim cb As New SqlCommand
        cb.CommandText = "Delete from AttivitàDocumentiFormazione WHERE IdAttivitàDocumentoFormazione = " & IdDoc
        cb.Connection = cnLocal
        cb.ExecuteNonQuery()
        cb.Dispose()
        cb = Nothing
    End Sub
    Public Shared Function VerificaPrefissiDocumentoFormatori(ByVal objPercorsoFile As HtmlInputFile, ByVal conn As SqlClient.SqlConnection, ByRef PrefissoFile As String) As Boolean
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        Dim StrSql As String
        Dim DsPrefisso As New DataSet
        VerificaPrefissiDocumentoFormatori = False
        'carico in un dt i prefissi dei documenti
        StrSql = "select prefisso from prefissientitàdocumenti where tipoinserimento=3 "
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
                If r.Item("Prefisso") = UCase(Prefisso(0).ToString) & "_" Then
                    PrefissoFile = UCase(Prefisso(0).ToString) & "_"
                    VerificaPrefissiDocumentoFormatori = True
                    Exit For
                End If
            Next
        End If
        Return VerificaPrefissiDocumentoFormatori
    End Function
    Public Shared Function CaricaDocumentoPresenzeFormatori(ByVal IdAttività As Integer, ByVal strUtente As String, ByVal objPercorsoFile As HtmlInputFile, ByRef cnLocal As SqlConnection, ByVal PrefissoFile As String) As String
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

                'richamo store che fa i controlli su documento

                msg = ControlloDocumentiPresenzeFormatori(IdAttività, PrefissoFile, strHashValue, cnLocal)
                If msg = "ok" Then
                    InserimentoDocumentoPresenzeFormatori(IdAttività, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, cnLocal)
                    msg = "ok"
                End If

            End If
        End If

        If File.Exists(strPercorsoServer) Then
            File.Delete(strPercorsoServer)
        End If
        Return msg
    End Function
    Public Shared Function ControlloDocumentiPresenzeFormatori(ByVal IdAttività As Integer, TipoDocumento As String, HashValue As String, ByRef cnLocal As SqlConnection) As String


        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_FORMATORI_CONTROLLO_DOCUMENTI]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, cnLocal)
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdAttività", SqlDbType.VarChar).Value = IdAttività
            sqlCMD.Parameters.Add("@TipoDocumento", SqlDbType.VarChar).Value = TipoDocumento
            sqlCMD.Parameters.Add("@HashValue", SqlDbType.VarChar).Value = HashValue

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


            str = sqlCMD.Parameters("@Messaggio").Value

            Return str



        Catch ex As Exception

            Exit Function
        End Try

    End Function
    Private Shared Sub InserimentoDocumentoPresenzeFormatori(ByVal IdAttività As Integer, ByVal NomeUnivoco As String, ByVal bBLOBStorage() As Byte, ByVal strUtente As String, ByVal strHashValue As String, ByVal cnLocal As SqlClient.SqlConnection)

        'sub che consente l'inserimento dei documenti da associare al progetto
        Dim cmd As SqlClient.SqlCommand = New SqlCommand _
        ("INSERT INTO AttivitàDocumentiFormazione (IdAttività,BinData, FileName,DataInserimento,UsernameInserimento,HashValue) " _
            & "VALUES(@IdAttività,@blob_data,@blob_filename ,getdate(),@utente,@hash_value   )", cnLocal)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("@IdAttività", SqlDbType.Int)
        cmd.Parameters("@IdAttività").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_filename", SqlDbType.VarChar)
        cmd.Parameters("@blob_filename").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
        cmd.Parameters("@blob_data").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@utente", SqlDbType.VarChar)
        cmd.Parameters("@utente").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@hash_value", SqlDbType.VarChar)
        cmd.Parameters("@hash_value").Direction = ParameterDirection.Input


        cmd.Parameters("@IdAttività").Value = IdAttività
        cmd.Parameters("@blob_filename").Value = NomeUnivoco
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue

        cmd.ExecuteNonQuery()
    End Sub
    Public Shared Sub AggiornaStatoAttivitaDocumentiFormazione(ByVal idAttivitaDocumentoFormazione As Integer, ByVal stato As Byte, ByVal strUtente As String, ByVal connessione As SqlConnection)
        Dim sqlCommand As New SqlCommand
        sqlCommand.CommandText = "Update AttivitàDocumentiFormazione set Stato= " & stato & " , UsernameStato='" & strUtente & "', DataStato= getdate ()  WHERE IdAttivitàDocumentoFormazione = " & idAttivitaDocumentoFormazione
        sqlCommand.Connection = connessione
        sqlCommand.ExecuteNonQuery()
    End Sub
End Class
