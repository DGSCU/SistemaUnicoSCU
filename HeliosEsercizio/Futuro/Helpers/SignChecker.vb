Imports System.IO
Imports System.Text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.parser
Imports iTextSharp.text.pdf.security
Imports Org.BouncyCastle.Cms
Imports Org.BouncyCastle.Pkcs
Imports Org.BouncyCastle.Asn1
Imports Org.BouncyCastle.Asn1.X509
Imports Org.BouncyCastle.Utilities.Encoders
Imports Org.BouncyCastle.Asn1.Cms
Imports System.Collections.Generic
Imports System.Security.Cryptography

''' <summary>
''' Classe per controllare in modo semplice le firme dei file pdf e p7m
''' </summary>
''' <remarks></remarks>
Public Class SignChecker
    Private _fileBytes As Byte()
    Private _firme As List(Of Firma)
    Private _tipoFile As String = "non definito"
    Private _lastError As String = ""
    Private _log As String = ""

    ''' <summary>
    '''  Costruttore che utilizza il documento sotto forma di array di byte
    ''' </summary>
    ''' <param name="bytes">il documento da controllare come array di byte</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal bytes As Byte())
        _log = "SignChecker(Byte())"
        _fileBytes = bytes
        loadCertificates()
    End Sub
    ''' <summary>
    ''' Costruttore che utilizza il documento come file memorizzato su disco
    ''' </summary>
    ''' <param name="filePath">il path completo del file pdf o p7m da controllare</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal filePath As String)
        _log = "SignChecker(" & filePath & ")"
        _fileBytes = File.ReadAllBytes(filePath)
        convertFromBase64()
        loadCertificates()
    End Sub
    Private Sub loadCertificates()
        _firme = New List(Of Firma)()
        'prova prima a vedere se e' un pdf, altrimenti se e' un p7m
        If Not loadCertificatesP7M() Then loadCertificatesPdf()
    End Sub
    Private Sub convertFromBase64()
        'se il file era in base64, lo traduce
        Try
            Dim s As String = System.Text.Encoding.Default.GetString(_fileBytes)
            If Not s Like "*BEGIN PKCS7*" Then Return
            'elimina header e footer
            s = System.Text.RegularExpressions.Regex.Replace(s, "-+BEGIN PKCS7-+[\r\n]", "")
            s = System.Text.RegularExpressions.Regex.Replace(s, "[\r\n]-+END PKCS7-+", "")
            _fileBytes = Convert.FromBase64String(s)
        Catch ex As Exception

        End Try
    End Sub

    Private Function loadCertificatesPdf() As Boolean
        Dim reader As PdfReader
        _log &= vbCrLf & "inizio loadCertificatesPdf()"
        Try
            reader = New PdfReader(_fileBytes)
        Catch ex As Exception
            _log &= vbCrLf & "exception " & ex.Message
            Return False
        End Try
        Dim af As AcroFields = reader.AcroFields
        Dim names As List(Of String) = af.GetSignatureNames()

        For i As Integer = 0 To names.Count - 1
            Dim name As String = CStr(names(i))
            Dim pk As PdfPKCS7 = af.VerifySignature(name)
            For Each cer In pk.Certificates
                Dim firma As Firma = New Firma() With {
                    .Certificato = cer,
                    .DataFirma = pk.SignDate,
                    .Certificatore = GetValue(cer.IssuerDN.ToString, "CN"),
                    .DataValiditaDa = cer.NotBefore,
                    .DataValiditaA = cer.NotAfter,
                    .CodiceFiscale = GetValue(cer.SubjectDN.ToString, "SERIALNUMBER"),
                    .Nominativo = GetValue(cer.SubjectDN.ToString, "CN"),
                    .isValidNow = cer.IsValidNow
                 }
                _log &= vbCrLf & "trovata firma [" & firma.CodiceFiscale & "]"
                _firme.Add(firma)
            Next
        Next
        _log &= vbCrLf & "fine loadCertificatesPdf(), trovate " & names.Count & " firme"
        _tipoFile = "PAdES"
        Return True
    End Function

    Private Function eliminaHeaderFooter(ByVal _fileBytes As Byte()) As Byte()
        'se il file era in base64, lo traduce
        Try
            Dim s As String = System.Text.Encoding.Default.GetString(_fileBytes)
            If Not s Like "*BEGIN PKCS7*" Then Return _fileBytes
            'elimina header e footer
            s = System.Text.RegularExpressions.Regex.Replace(s, "-+BEGIN PKCS7-+[\r\n]", "")
            s = System.Text.RegularExpressions.Regex.Replace(s, "[\r\n]-+END PKCS7-+", "")

            Return System.Text.Encoding.Default.GetBytes(s)

        Catch ex As Exception

        End Try
    End Function

    Private Function aggiungiFinali(ByVal _fileBytes As Byte()) As Byte()
        'se non esistono aggiunge gli "==" finali
        Try

            If _fileBytes(_fileBytes.Length - 2) <> 61 Then          'ultimo carattere non è "="
                ReDim Preserve _fileBytes(_fileBytes.Length + 1)
                _fileBytes(_fileBytes.Length - 2) = 61
                _fileBytes(_fileBytes.Length - 3) = 61
            ElseIf _fileBytes(_fileBytes.Length - 3) <> 61 Then     'penultimo carattere non è "="
                ReDim Preserve _fileBytes(_fileBytes.Length + 1)
                _fileBytes(_fileBytes.Length - 2) = 61
                _fileBytes(_fileBytes.Length - 3) = 61
            End If

            Return _fileBytes

        Catch ex As Exception

        End Try
    End Function


    Private Function myDecodeFrom64(ByVal bytes As Byte()) As Byte()
        'La funzione legge un array di bytes preso direttamente dal file Base 64

        bytes = eliminaHeaderFooter(bytes)  'elimina eventuali header/footer che ci sono in alcuni files

        Dim _lung As Integer = 0         'lunghezza del risultato
        Dim result(bytes.Length) As Byte 'dimensione del risultato, sovradimensionato all'inizio

        Using myTransform = New FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces)
            Dim myOutputBytes(myTransform.OutputBlockSize) As Byte

            Dim i As Integer = 0

            'Ciclo sul file a blocchi di 4 bytes (lunghezza del blocco di encode)
            While bytes.Length - i > 4
                Dim bytesWritten As Integer = myTransform.TransformBlock(bytes, i, 4, myOutputBytes, 0)
                For j As Integer = 0 To bytesWritten - 1
                    result(_lung + j) = myOutputBytes(j)
                Next
                _lung = _lung + bytesWritten
                i = i + 4
            End While

            'Decodifica della rimanenza
            myOutputBytes = myTransform.TransformFinalBlock(bytes, i, bytes.Length - i)
            For j As Integer = 0 To myOutputBytes.Length - 1
                _lung = _lung + 1
                result(_lung - 1) = myOutputBytes(j)
                result(result.Length - 1) = myOutputBytes(j)
                If myOutputBytes(j) = 0 Then Exit For
            Next
        End Using

        Array.Resize(result, _lung) 'ridimensionamento esatto del risultato
        Return result
    End Function

    Private Function loadCertificatesP7M() As Boolean
        Dim cms As CmsSignedData = Nothing
        _log &= vbCrLf & "inizio loadCertificatesP7M()"
        Try
            'cerco firme per file non encoded
            _log &= vbCrLf & "inizio ricerca firme non base64 encoded"
            cms = New CmsSignedData(_fileBytes)
        Catch e As CmsException
            _lastError = e.Message
            _log &= vbCrLf & "ricerca firme non base64 encoded fallita"
            Try
                'cerco firme per file base64 encoded
                _log &= vbCrLf & "inizio ricerca firme base64 encoded"
                cms = New CmsSignedData(myDecodeFrom64(_fileBytes))
            Catch enew As Exception
                Try
                    'cerco firme per file base64 encoded con aggiunta finali
                    _log &= vbCrLf & "inizio ricerca firme base64 encoded con aggiunta finali"
                    cms = New CmsSignedData(myDecodeFrom64(aggiungiFinali(_fileBytes)))
                Catch ex2 As Exception
                    _lastError = enew.Message
                    _log &= vbCrLf & "ricerca firme base64 encoded fallita, non sembra P7M"
                    Return False
                End Try
            End Try
        End Try

        Dim certStore = cms.GetCertificates("Collection")
        Dim certs = certStore.GetMatches(New Org.BouncyCastle.X509.Store.X509CertStoreSelector())
        For Each cer As Org.BouncyCastle.X509.X509Certificate In certs
            For Each signer As SignerInformation In cms.GetSignerInfos().GetSigners()
                If signer.Verify(cer.GetPublicKey) Then
                    Dim Asn1Time As Org.BouncyCastle.Asn1.Cms.Attribute = signer.SignedAttributes(CmsAttributes.SigningTime)
                    Dim signedTime As DateTime = Org.BouncyCastle.Asn1.Cms.Time.GetInstance(Asn1Time.AttrValues(0)).Date

                    Dim firma As Firma = New Firma() With {
                        .Certificato = cer,
                        .DataFirma = signedTime,
                        .Certificatore = GetValue(cer.IssuerDN.ToString, "CN"),
                        .DataValiditaDa = cer.NotBefore,
                        .DataValiditaA = cer.NotAfter,
                        .CodiceFiscale = GetValue(cer.SubjectDN.ToString, "SERIALNUMBER"),
                        .Nominativo = GetValue(cer.SubjectDN.ToString, "CN"),
                    .isValidNow = cer.IsValidNow
                     }
                    _firme.Add(firma)
                    _log &= "trovata firma [" & firma.CodiceFiscale & "]"
                End If
            Next
        Next
        _log &= vbCrLf & "fine loadCertificatesP7M(), trovate " & _firme.Count & " firme"
        Return True
    End Function
    Private Function GetValue(ByVal cert As String, ByVal field As String) As String
        cert = "," & cert & ","
        Dim RegExMatch As Text.RegularExpressions.MatchCollection = _
            System.Text.RegularExpressions.Regex.Matches(cert, "[{,]" & field & "=(.+?)[,}]")
        If RegExMatch.Count < 1 Then Return ""
        Return RegExMatch(0).Groups(1).ToString


    End Function
    ''' <summary>
    ''' Controlla se il documento e' effettivamente firmato dal codice fiscale passato come parametro.
    ''' Opzionalmente si possono passare anche altri parametri per ulteriori controlli
    ''' </summary>
    ''' <param name="codiceFiscale">il codice fiscale che ci si aspetta abbia firmato il documento</param>
    ''' <param name="minSignDate">data/ora minima in cui puo' essere stata apposta la firma (anche se la firma e' valida, il controllo non viene superato se il documento e' stato firmato prima di questa data/ora)</param>
    ''' <param name="validDate">data/ora in cui si richiede che siano validi i certificati</param>
    ''' <returns>True: la firma e' corretta</returns>
    ''' <remarks></remarks>
    Public Function IsSignedBy(ByVal codiceFiscale As String,
        Optional ByVal minSignDate? As DateTime = Nothing,
        Optional ByVal validDate? As DateTime = Nothing) As Boolean

        If IsNothing(minSignDate) Then minSignDate = "2000-01-01 00:00:00"
        If IsNothing(validDate) Then validDate = Now()
        For Each firma In _firme
            If firma.CodiceFiscale.ToLower().Replace("tinit-", "").Replace("it:", "") <> "" Then
                If firma.CodiceFiscale.ToLower().Replace("tinit-", "").Replace("it:", "") = codiceFiscale.ToLower().Replace("tinit-", "") Then
                    If firma.DataFirma >= minSignDate And validDate >= firma.DataValiditaDa And validDate <= firma.DataValiditaA Then Return True
                End If
            End If
        Next
        Return False
    End Function
    ''' <summary>
    ''' Funzione richiamabile senza dover instanziare la classe
    ''' </summary>
    ''' <param name="doc">il documento da controllare</param>
    ''' <param name="codiceFiscale1">il primo codice fiscale da controllare</param>
    ''' <param name="codiceFiscale2">un secondo eventuale codice fiscale da controllare</param>
    ''' <param name="minSignDate">data/ora minima in cui puo' essere stata apposta la firma (anche se la firma e' valida, il controllo non viene superato se il documento e' stato firmato prima di questa data/ora)</param>
    ''' <param name="validDate">data/ora in cui si richiede che siano validi i certificati</param>
    ''' <returns>True: la firma e' corretta</returns>
    ''' <remarks></remarks>
    Public Shared Function IsSignedBy(ByVal doc As Byte(), ByVal codiceFiscale1 As String,
         Optional ByVal codiceFiscale2 As String = "",
         Optional ByVal minSignDate? As DateTime = Nothing,
         Optional ByVal validDate? As DateTime = Nothing) As Boolean

        Dim sc As New SignChecker(doc)
        Dim ok As Boolean = sc.IsSignedBy(codiceFiscale1, minSignDate, validDate) OrElse sc.IsSignedBy(codiceFiscale2, minSignDate, validDate)
        Return ok
    End Function
    ''' <summary>
    ''' Funzione richiamabile senza dover instanziare la classe
    ''' </summary>
    ''' <param name="docPath">il path su disco del documento da controllare</param>
    ''' <param name="codiceFiscale1">il primo codice fiscale da controllare</param>
    ''' <param name="codiceFiscale2">un secondo eventuale codice fiscale da controllare</param>
    ''' <param name="minSignDate">data/ora minima in cui puo' essere stata apposta la firma (anche se la firma e' valida, il controllo non viene superato se il documento e' stato firmato prima di questa data/ora)</param>
    ''' <param name="validDate">data/ora in cui si richiede che siano validi i certificati</param>
    ''' <returns>True: la firma e' corretta</returns>
    ''' <remarks></remarks>
    Public Shared Function IsSignedBy(ByVal docPath As String, ByVal codiceFiscale1 As String,
        Optional ByVal codiceFiscale2 As String = "",
        Optional ByVal minSignDate? As DateTime = Nothing,
        Optional ByVal validDate? As DateTime = Nothing) As Boolean

        Dim sc As New SignChecker(docPath)
        Return sc.IsSignedBy(codiceFiscale1, minSignDate, validDate) OrElse sc.IsSignedBy(codiceFiscale2, minSignDate, validDate)
    End Function

    Public Function getLog() As String
        Return _log
    End Function

    Public Function checkSignature(ByVal codiceFiscale1 As String,
        Optional ByVal codiceFiscale2 As String = "",
        Optional ByVal minSignDate? As DateTime = Nothing,
        Optional ByVal validDate? As DateTime = Nothing) As Boolean
        _log &= vbCrLf & "checkSignature(""" & codiceFiscale1 & """, """ & codiceFiscale2 & """) inizio"
        Dim bOk As Boolean = IsSignedBy(codiceFiscale1, minSignDate, validDate) OrElse IsSignedBy(codiceFiscale2, minSignDate, validDate)
        _log &= vbCrLf & "checkSignature() termina con valore=" & bOk
        Return bOk
    End Function
    Function getBytesContentP7M(ByVal p7bytes As Byte()) As Byte()
        Try
            Dim cms As New CmsSignedData(p7bytes)
            Dim s As New System.IO.MemoryStream()
            cms.SignedContent().Write(s)
            Return s.ToArray()
        Catch e As CmsException
            _lastError = e.Message
            If Not IsNothing(e.InnerException) Then _lastError &= vbCrLf & e.InnerException.Message
            _log &= vbCrLf & "getBytesContentP7M exception [" & _lastError & "]"
            Return Nothing
        End Try
    End Function

    Public Function compareNotSigned(ByVal notSigned As Byte()) As Boolean
        'provo a prendere il contenuto da file non encoded
        Dim contentSigned As Byte() = getBytesContentP7M(_fileBytes)
        'se è vuoto provo a prendere il contenuto da file base64 encoded
        If contentSigned Is Nothing Then
            Try
                contentSigned = getBytesContentP7M(myDecodeFrom64(_fileBytes))
            Catch ex As Exception
                'ignoro l'errore che si ha quando si prova ad estrarre il contenuto in formato p7m da un file che non lo è
                Try
                    contentSigned = getBytesContentP7M(myDecodeFrom64(aggiungiFinali(_fileBytes)))
                Catch ex2 As Exception
                    'ignoro l'errore che si ha quando si prova ad estrarre il contenuto in formato p7m da un file che non lo è
                End Try
            End Try
        End If


        Dim testoNotSigned As String = ExtractTextFromPdf(notSigned).Replace(" ", "")
        Dim testoSigned As String
        If IsNothing(contentSigned) Then
            'non e' in formato p7m, prova con il pdf firmato
            testoSigned = ExtractTextFromPdf(_fileBytes).Replace(" ", "")
        Else
            'writeFile(contentSigned, "c:\temp\estratto.pdf")
            testoSigned = ExtractTextFromPdf(contentSigned).Replace(" ", "")
        End If

        'rimuovo caratteri speciali della funzione Like
        testoNotSigned = testoNotSigned.Replace("#", "").Replace("[", "").Replace("]", "")
        testoSigned = testoSigned.Replace("#", "").Replace("[", "").Replace("]", "")

        Return testoSigned Like "*" & testoNotSigned & "*"

    End Function
    Public Function ExtractTextFromPdf(ByVal pdfBytes As Byte()) As String
        Dim s As New StringBuilder()
        Using reader As New PdfReader(pdfBytes)
            Dim Strategy As New iTextSharp.text.pdf.parser.LocationTextExtractionStrategy()

            For i As Integer = 1 To reader.NumberOfPages
                s.Append(PdfTextExtractor.GetTextFromPage(reader, i, Strategy))
            Next
        End Using
        Return s.ToString()
    End Function

    Public Shared Function compareSignedNotSigned(ByVal signed As Byte(), ByVal notSigned As Byte()) As Boolean
        Dim sc As New SignChecker(signed)
        Return sc.compareNotSigned(notSigned)
    End Function
    Public Shared Sub writeFile(ByVal content As Byte(), ByVal path As String)
        Dim oFileStream As System.IO.FileStream
        oFileStream = New System.IO.FileStream(path, System.IO.FileMode.Create)
        oFileStream.Write(content, 0, content.Length)
        oFileStream.Close()
    End Sub
End Class

Public Class Firma
    Public Property CodiceFiscale As String
    Public Property Nominativo As String
    Public Property DataFirma As DateTime?
    Public Property DataValiditaDa As DateTime
    Public Property DataValiditaA As DateTime
    Public Property Certificatore As String
    Public Property Certificato As Org.BouncyCastle.X509.X509Certificate
    Public Property isValidNow As Boolean
End Class

