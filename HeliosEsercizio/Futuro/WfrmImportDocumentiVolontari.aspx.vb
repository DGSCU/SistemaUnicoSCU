Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Ionic.Zip
Imports Logger.Data
Imports System.Security.Cryptography

Public Class WfrmImportDocumentiVolontari
    Inherits SmartPage
    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private xIdAttivita As Integer
    Private strSql As String
    Private dtrGenreico As SqlClient.SqlDataReader

    Sub ImpostaMaschera(ByVal AlboEnte As String)
        Dim StrTitoloLegend As String
        Dim StrTitolo As String
        Dim strTestoRicerca As String
     
        StrTitoloLegend = "Importazione Documenti Volontari"
        'StrTitolo = "Importazione Documenti Volontari"
        DivImportCV.Visible = True

        lblTitoloLegend.Text = StrTitoloLegend
        'lblTitolo.Text = StrTitolo

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
        Dim TipoInserimento As Integer

        If Page.IsPostBack = False Then

            Dim AlboEnte As String
            AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))
            ImpostaMaschera(Session(AlboEnte))

            If Request.QueryString("VengoDA") Is Nothing Then
                TipoInserimento = 1
            Else
                TipoInserimento = 0
            End If
            CaricaGriglia()

        End If

    End Sub

    Sub CaricaGriglia()
        Dim strSql As String
        Dim sqlDataSet As DataSet

        strSql = " SELECT IDPrefisso, Prefisso, TipologiaDocumento, ModalitàInvio FROM PrefissiEntitàDocumenti Where TipoInserimento = 0 and Sistema='" & Session("Sistema") & "' ORDER BY ORDINE"
        sqlDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgElencoPrefissi.DataSource = sqlDataSet
        dgElencoPrefissi.DataBind()
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub btnCaricaCV_Click(sender As Object, e As EventArgs) Handles btnCaricaCV.Click

        txtErroreCV.Text = ""
        Dim UniqueName = Guid.NewGuid.ToString()
        Dim FilezipPath = Server.MapPath("reports\export\") & UniqueName & ".zip"
        FileCV.PostedFile.SaveAs(FilezipPath)
        Dim zip As ZipFile
        Try
            zip = New ZipFile(FilezipPath)
        Catch ex As Exception
            Log.Error(LogEvent.RISORSE_ENTE_ERRORE_IMPORTAZIONE_CV)
            txtErroreCV.Text = "Formato file Non valido. Zip non valido"
            File.Delete(FilezipPath)
            Exit Sub
        End Try

        Dim filesDaElaborare As New List(Of Allegato)
        Dim risorseEnte As New Dictionary(Of String, Integer)
        Dim filesConErrore As New Dictionary(Of String, String)
        Dim strSql As String

        strSql = "SELECT DISTINCT e.CodiceVolontario, e.IDEntità FROM entità e inner join attivitàentità ae on ae.IDEntità=e.IDEntità inner join attivitàentisediattuazione aesa on ae.IDAttivitàEnteSedeAttuazione=aesa.IDAttivitàEnteSedeAttuazione inner join attività b on b.IDAttività=aesa.IDAttività inner join enti ee on ee.IDEnte=b.IDEntePresentante	inner join TipiProgetto tp on b.IdTipoProgetto=tp.IdTipoProgetto " & _
                 "WHERE e.IDStatoEntità in (3,5,6) and e.datainizioservizio<getdate() and ae.IdStatoAttivitàEntità = 1 and ee.IDEnte=" & Session("IdEnte")

        If Session("Sistema") = "Helios" Then
            strSql = strSql & " and tp.MacroTipoProgetto <> 'GG'"
        Else
            strSql = strSql & " and tp.MacroTipoProgetto = 'GG'"
        End If

        dtrGenreico = ClsServer.CreaDatareader(strSql, Session("conn"))
        Try
            While dtrGenreico.Read
                risorseEnte.Add(dtrGenreico("CodiceVolontario"), dtrGenreico("IDEntità"))
            End While
        Catch ex As Exception
            Log.Error(LogEvent.RISORSE_ENTE_ERRORE_IMPORTAZIONE_CV, "Caricamento dati", ex)
            txtErroreCV.Text = "Errore nel recupero delle informazioni."
            File.Delete(FilezipPath)
            Exit Sub
        Finally
            ChiudiDataReader(dtrGenreico)
        End Try

        For Each file As ZipEntry In zip

            Dim NomeFile As String
            Try
                NomeFile = file.FileName
                If UCase(Right(NomeFile, 4)) <> ".PDF" And UCase(Right(NomeFile, 8)) <> ".PDF.P7M" Then
                    filesConErrore.Add(NomeFile, "Formato File non corretto.")
                    Continue For
                End If
            Catch ex As Exception
                filesConErrore.Add(NomeFile, "Formato File non corretto.")
                Continue For
            End Try

            Dim NomeDocumento As String
            Dim Prefisso As String
            Try
                NomeDocumento = NomeFile.Split(".")(0)
                Prefisso = NomeDocumento.Split("_")(0)
                If cercaPrefisso(Prefisso) = False Then
                    filesConErrore.Add(NomeFile, "Prefisso File non corretto.")
                    Continue For
                End If
            Catch ex As Exception
                filesConErrore.Add(NomeFile, "Prefisso File non corretto.")
                Continue For
            End Try

            Dim CodiceVolontario As String
            Try
                CodiceVolontario = NomeDocumento.Split("_")(1)
                If Not risorseEnte.ContainsKey(CodiceVolontario) Then
                    filesConErrore.Add(NomeFile, "Codice Volontario non presente nell'ente.")
                    Continue For
                End If
            Catch ex As Exception
                filesConErrore.Add(NomeFile, "Codice Volontario non presente nell'ente.")
                Continue For
            End Try

            Dim Identita As Integer
            If risorseEnte.ContainsKey(CodiceVolontario) Then
                Identita = risorseEnte.Item(CodiceVolontario)
            End If

            Dim blob As Byte()
            Dim ms As New MemoryStream
            file.Extract(ms)
            blob = ms.ToArray
            Dim hash As String = GeneraHash(blob)

            Dim nuovoAllegato = New Allegato With {
             .Blob = blob,
             .Filename = NomeFile,
             .Hash = hash,
             .Id = Identita
                     }

            If cercaHash(Identita, nuovoAllegato.Hash) = True Then
                filesConErrore.Add(NomeFile, "File già presente.")
                Continue For
            End If

            For Each allegato As Allegato In filesDaElaborare
                If nuovoAllegato.Hash = allegato.Hash Then
                    filesConErrore.Add(NomeFile, "File già presente nel file Zip: " + allegato.Filename)
                    Exit For
                End If
            Next

            filesDaElaborare.Add(nuovoAllegato)

        Next
        zip.Dispose()
        File.Delete(FilezipPath)

        If filesConErrore.Count = 0 Then
            Dim tran As SqlTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            Try
                For Each allegato As Allegato In filesDaElaborare
                    Dim idAllegato As Integer = SalvaAllegatoEntita(allegato, tran)
                Next
                tran.Commit()
                lblElaborazioneCV.Text = "Elaborazione effettuata correttamente. Sono stati importati " & filesDaElaborare.Count & " Documenti"
                lblElaborazioneCV.CssClass = String.Empty
                lstErroriCV.Text = String.Empty
                modalCVResult.Show()
                Log.Information(LogEvent.RISORSE_ENTE_IMPORTAZIONE_CV)
            Catch Ex As Exception
                tran.Rollback()
                Log.Error(LogEvent.RISORSE_ENTE_ERRORE_IMPORTAZIONE_CV, "Scrittura Database", Ex)
                txtErroreCV.Text = "Errore nel caricamento del file ZIP."
                File.Delete(FilezipPath)
                Exit Sub
            End Try
        Else
            Dim htmlListErrore As New StringBuilder("<table class=""table"" cellspacing=""0"" rules=""all"" border=""1"" style=""width: 100%;""><tr><th>Nome File</th><th>Problema</th></tr>")
            lblElaborazioneCV.Text = "Si sono verificati i seguenti errori nell'importazione dei Documenti"
            lblElaborazioneCV.CssClass = "msgErrore"
            For Each errore In filesConErrore
                htmlListErrore.Append("<tr class=""tr"" align=""center""><td>" & errore.Key & "</td><td>" & errore.Value & "</td></tr>")
            Next
            htmlListErrore.Append("</table>")
            lstErroriCV.Text = htmlListErrore.ToString()
            Log.Warning(LogEvent.RISORSE_ENTE_ERRORE_IMPORTAZIONE_CV, parameters:=filesConErrore)
            modalCVResult.Show()
        End If

    End Sub

    Private Function GeneraHash(FileinByte() As Byte) As String
        Dim tmpHash() As Byte

        tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

        GeneraHash = ByteArrayToString(tmpHash)
        Return GeneraHash
    End Function
    Private Function ByteArrayToString(arrInput() As Byte) As String
        Dim i As Integer
        Dim sOutput As New StringBuilder(arrInput.Length)
        For i = 0 To arrInput.Length - 1
            sOutput.Append(arrInput(i).ToString("X2"))
        Next
        Return sOutput.ToString()
    End Function

    Function SalvaAllegatoEntita(A As Allegato, tran As SqlTransaction) As Integer
        Dim MyCommand As New SqlCommand("", Session("conn"))
        MyCommand.Transaction = tran
        Dim _ret As Integer = SalvaAllegatoEntitaDocumento(A, MyCommand)
        MyCommand.Dispose()
        Return _ret
    End Function

    Function SalvaAllegatoEntitaDocumento(A As Allegato, MyCommand As SqlClient.SqlCommand) As Integer
        Dim strsql As String
        Dim _ret As Integer
        MyCommand.Parameters.Clear()
        MyCommand.Parameters.AddWithValue("@BinData", A.Blob)
        MyCommand.Parameters.AddWithValue("@FileName", A.Filename)
        MyCommand.Parameters.AddWithValue("@HashValue", A.Hash)
        MyCommand.Parameters.AddWithValue("@IdEntità", A.Id)
        MyCommand.Parameters.AddWithValue("@UsernameInserimento", If(Session("CodiceFiscaleUtente"), Session("Account")))
        strsql = " INSERT INTO EntitàDocumenti ( IdEntità, BinData, FileName, HashValue, DataInserimento, UsernameInserimento,Stato,Trasmesso,Note,Anno,Mese,DataTrasmissione,UsernameStato,DataStato) " _
              & " VALUES (@IdEntità,@BinData,@FileName,@HashValue,GETDATE(),@UsernameInserimento,0,0,null,null,null,null,null,null); " _
              & "SELECT SCOPE_IDENTITY()"
        MyCommand.CommandText = strsql
        _ret = CInt(MyCommand.ExecuteScalar)
        A.Id = _ret
        Return _ret
    End Function

    Function cercaPrefisso(Prefisso As String) As Boolean
        Dim strSql As String
        ChiudiDataReader(dtrGenreico)
        strSql = "select IDPrefisso from prefissientitàdocumenti where tipoinserimento=0 and Sistema='" & Session("Sistema") & "' AND Prefisso='" & Prefisso & "_'"
        dtrGenreico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenreico.HasRows = True Then
            ChiudiDataReader(dtrGenreico)
            Return True
        Else
            ChiudiDataReader(dtrGenreico)
            Return False
        End If
    End Function

    Function cercaHash(Identita As Integer, p2 As String) As Boolean
        Dim strSql As String
        strSql = "SELECT IdEntitàDocumento FROM ENTITàDOCUMENTI WHERE IdEntità=" & Identita & " and HashValue='" & p2 & "'"
        dtrGenreico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenreico.HasRows = True Then
            ChiudiDataReader(dtrGenreico)
            Return True
        Else
            ChiudiDataReader(dtrGenreico)
            Return False
        End If
    End Function

    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

End Class