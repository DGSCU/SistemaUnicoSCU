Imports System
Imports System.Web.Services
Imports System.Security.Cryptography
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mail
Imports System.Configuration
Imports System.Text
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports
Imports System.Web.Services.Protocols
Imports System.ComponentModel
<System.Web.Services.WebService(Namespace := "http://tempuri.org/WSDocumentazione/WSMetodiDocumentazione")> _
Public Class WSMetodiDocumentazione
    Inherits System.Web.Services.WebService
	Public sqlConn As New SqlConnection

#Region " Codice generato da Progettazione servizi Web "

    Public Sub New()
        MyBase.New()

        'Chiamata richiesta da Progettazione servizi Web
        InitializeComponent()

        'Aggiungere il codice di inizializzazione dopo la chiamata a InitializeComponent()

    End Sub

    'Richiesto da Progettazione servizi Web
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione servizi Web.
    'Può essere modificata in Progettazione servizi Web.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        'CODEGEN: questa procedura è richiesta da Progettazione servizi Web.
        'Non modificarla nell'editor del codice.
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

    ' ESEMPIO DI SERVIZIO WEB
    ' Il servizio di esempio HelloWorld() restituisce la stringa "Salve gente!".
    ' Per generare il servizio, rimuovere i commenti dalle righe che seguono, quindi salvare e generare il progetto.
    ' Per eseguire il test del servizio, assicurarsi che la pagina iniziale sia costituita dal file .asmx 
    ' e premere F5.
    '
    <WebMethod()> _
    Public Function RecuperaTemplate(ByVal strPath As String, ByVal strNomeFile As String) As String
        'converto il file in base64
        Dim testoBase64 As String = ""

        Try
            'testoBase64 = FileToBase64(strPath & strNomeFile)

            Dim bFile() As Byte
            Dim fs As FileStream
            Dim _textB64 As String
            Try
                fs = New FileStream(strPath & strNomeFile, FileMode.Open)
                ReDim bFile(fs.Length - 1)
                fs.Read(bFile, 0, fs.Length)
                _textB64 = Convert.ToBase64String(bFile)
            Catch ex As Exception
                _textB64 = ex.Message
                'Finally
                '    fs.Close()
            End Try

            fs.Close()

            RecuperaTemplate = _textB64

            Return RecuperaTemplate

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    <WebMethod()> _
    Public Function ScriviTemplate(ByVal strPath As String, ByVal strFileBase64 As String, ByVal strNomeFile As String)
        'converto il file in base64
        Dim testoBase64 As String = ""

        Try
            Dim dataBuffer As Byte() = Convert.FromBase64String(strFileBase64)
            Dim fs As FileStream
            fs = New FileStream(strPath & strNomeFile, FileMode.Create, FileAccess.Write)
            If (dataBuffer.Length > 0) Then
                fs.Write(dataBuffer, 0, dataBuffer.Length)
            End If
            fs.Close()
        Catch ex As Exception

        End Try

    End Function
    '   <WebMethod()> _
    '   Public Function getBox(ByVal IdBandoAtttivita As Integer, ByVal username As String, ByVal nomereport As String) As String
    '       Dim localWS As New WS_PDF.getPDF

    '       localWS.Timeout = 1000000

    '       Return localWS.GeneraFile(IdBandoAtttivita, username, nomereport)

    'End Function

   <WebMethod()> _
    Public Function GenerazioneBOX16_BOX19(ByVal IdBandoAttivita As Integer, ByVal NomeUtente As String)

        'Dim localWS As New WS_Editor.WSMetodiDocumentazione
        Dim strsql As String
        Dim ds As New DataSet
        Dim SqlDA As SqlDataAdapter
        Dim i As Integer
        Dim strCodiceProgetto As String

        'RECUPERO DATI HELIOS
        sqlConn.ConnectionString = ConfigurationSettings.AppSettings("cnHelios")
        sqlConn.Open()

        strsql = "Select idAttività,CodiceEnte,idtipoProgetto,EsteroUE From Attività where IdBandoAttività = " & IdBandoAttivita

        SqlDA = New SqlDataAdapter(strsql, sqlConn)
        SqlDA.Fill(ds)

        'Dim cmdLog As New SqlCommand
        'cmdLog.Connection = sqlConn

        If ds.Tables(0).Rows.Count > 0 Then
            Dim NomeReport As String
            Dim strNomeFile As String
            Dim strFile As String
            Dim strCodice As String

            For Each Riga As DataRow In ds.Tables(0).Rows
                ' Concateno codice TipoProgetto e EsteroUE per verificarne le combinazioni
                strCodice = Riga.Item("idtipoProgetto").ToString & "-" & IIf(Riga.Item("EsteroUE"), "T", "")
                Select Case strCodice
                    Case "2-", "6-", "8-", "10-"
                        NomeReport = "crpElencoSAPEsteroProgetto.rpt"
                        strNomeFile = "Box19_"
                        strCodiceProgetto = Riga.Item("CodiceEnte")
                        strFile = GeneraFile(Riga.Item("idAttività"), NomeUtente, NomeReport) ',
                        DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                        clsGestioneDocumenti.CaricaDocumentoProgettoBOX(Riga.Item("idAttività"), NomeUtente, strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), sqlConn)

                        NomeReport = "crpElencoSAPEstero20Progetto.rpt"
                        strNomeFile = "Box20_"
                        strCodiceProgetto = Riga.Item("CodiceEnte")
                        strFile = GeneraFile(Riga.Item("idAttività"), NomeUtente, NomeReport) ',
                        DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                        clsGestioneDocumenti.CaricaDocumentoProgettoBOX(Riga.Item("idAttività"), NomeUtente, strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), sqlConn)

                    Case "5-T", "9-T"
                        NomeReport = "crpElencoSAPItaliaProgetto.rpt"
                        strNomeFile = "Box17_"
                        strCodiceProgetto = Riga.Item("CodiceEnte")
                        strFile = GeneraFile(Riga.Item("idAttività"), NomeUtente, NomeReport) ',
                        DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                        clsGestioneDocumenti.CaricaDocumentoProgettoBOX(Riga.Item("idAttività"), NomeUtente, strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), sqlConn)

                        NomeReport = "crpElencoSAPEsteroUEProgetto.rpt"
                        strNomeFile = "BoxUE_"
                        strCodiceProgetto = Riga.Item("CodiceEnte")
                        strFile = GeneraFile(Riga.Item("idAttività"), NomeUtente, NomeReport) ',
                        DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                        clsGestioneDocumenti.CaricaDocumentoProgettoBOX(Riga.Item("idAttività"), NomeUtente, strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), sqlConn)

                    Case "11-", "3-"
                        NomeReport = "crpBOXItalia2020.rpt"
                        strNomeFile = "BoxItalia_"
                        strCodiceProgetto = Riga.Item("CodiceEnte")
                        strFile = GeneraFile(Riga.Item("idAttività"), NomeUtente, NomeReport) ',
                        DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                        clsGestioneDocumenti.CaricaDocumentoProgettoBOX(Riga.Item("idAttività"), NomeUtente, strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), sqlConn)

                    Case "11-T", "12-"
                        NomeReport = "crpBOXItalia2020.rpt"
                        strNomeFile = "BoxItalia_"
                        strCodiceProgetto = Riga.Item("CodiceEnte")
                        strFile = GeneraFile(Riga.Item("idAttività"), NomeUtente, NomeReport) ',
                        DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                        clsGestioneDocumenti.CaricaDocumentoProgettoBOX(Riga.Item("idAttività"), NomeUtente, strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), sqlConn)

                        NomeReport = "crpBOXEstero2020.rpt"
                        strNomeFile = "BoxEstero_"
                        strCodiceProgetto = Riga.Item("CodiceEnte")
                        strFile = GeneraFile(Riga.Item("idAttività"), NomeUtente, NomeReport) ',
                        DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                        clsGestioneDocumenti.CaricaDocumentoProgettoBOX(Riga.Item("idAttività"), NomeUtente, strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), sqlConn)

                    Case Else

                        'ITALIA
                        NomeReport = "crpElencoSAPItaliaProgetto.rpt"
                        strNomeFile = "Box17_"
                        strCodiceProgetto = Riga.Item("CodiceEnte")
                        strFile = GeneraFile(Riga.Item("idAttività"), NomeUtente, NomeReport) ', 
                        DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                        clsGestioneDocumenti.CaricaDocumentoProgettoBOX(Riga.Item("idAttività"), NomeUtente, strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), sqlConn)

                End Select

            Next
        End If

        Dim cmdUp As SqlCommand
        'update InLavorazione a 1 
        'aggiunto flag LAVORAZIONE =1 da simona cordella il 16/10/2012
        cmdUp = New Data.SqlClient.SqlCommand(" UPDATE BandiAttività" & _
                                              " SET InLavorazione= 0  " & _
                                              " WHERE idbandoattività=" & IdBandoAttivita & "", sqlConn)
        cmdUp.ExecuteNonQuery()
        cmdUp.Dispose()
    End Function
    Public Function GeneraFile(ByVal idAttivita As Integer, ByVal username As String, ByVal NomeReport As String) As String
        Dim sDati As String
        Dim strEsito As String
        'Dim cmdLog As New SqlCommand
        'cmdLog.Connection = sqlConn

        'cmdLog.CommandText = "insert into _log values ('generafile 1')"
        'cmdLog.ExecuteNonQuery()

        sDati = "IdAttivita," & idAttivita & ":"

        Try
            'cmdLog.CommandText = "insert into _log values ('" & NomeReport & " - " & sDati & " - " & username & "')"
            'cmdLog.ExecuteNonQuery()

            strEsito = CreatePdf(NomeReport, sDati, username)

            If strEsito = "ERRORE" Then
                'cmdLog.CommandText = "insert into _log values ('ERRORE GENERAFILE')"
                'cmdLog.ExecuteNonQuery()
                Return strEsito
            Else
                Return FileToBase64(strEsito)
            End If

        Catch ex As Exception
            Return ex.Message '"ERRORE"
        End Try

    End Function
   
    Public Function FileToBase64(ByVal fileName As String) As String
        Dim bFile() As Byte
        Dim fs As FileStream
        Dim _textB64 As String
        Try
            fs = New FileStream(Server.MapPath(fileName), FileMode.Open)
            ReDim bFile(fs.Length - 1)
            fs.Read(bFile, 0, fs.Length)
            _textB64 = Convert.ToBase64String(bFile)
        Catch ex As Exception
            'gestione eccezione 
            Return "ERRORE"
        Finally
            fs.Close()
        End Try
        Return _textB64
    End Function
    Function CreatePdf(ByVal NomeReport As String, ByVal StrDati As String, ByVal strUserName As String, Optional ByVal SottoReport As String = "", Optional ByVal ReportStorico As Int16 = 1) As String
        '*************************************************************************************************
        'DESCRIZIONE: Genera il PDF nella directory Reports/Export del report selezionato
        'AUTORE: TESTA GUIDO    DATA: 04/10/2004
        '*************************************************************************************************
        'Dim cmdLog As New SqlCommand
        'cmdLog.Connection = sqlConn

        'cmdLog.CommandText = "insert into _log values ('createpdf 1')"
        'cmdLog.ExecuteNonQuery()

        Dim paramFieldDt As New ParameterField

        'cmdLog.CommandText = "insert into _log values ('createpdf 1.1')"
        'cmdLog.ExecuteNonQuery()

        Dim discreteValDt As New ParameterDiscreteValue

        'cmdLog.CommandText = "insert into _log values ('createpdf 1.2')"
        'cmdLog.ExecuteNonQuery()

        Dim myPath As New System.Web.UI.Page

        'cmdLog.CommandText = "insert into _log values ('createpdf 1.3')"
        'cmdLog.ExecuteNonQuery()
        Dim GetPdfError As String
       
            Dim crReportDocument As New ReportDocument

            'cmdLog.CommandText = "insert into _log values ('createpdf 1.4')"
            'cmdLog.ExecuteNonQuery()

            Dim logOnInfo As New TableLogOnInfo
            Dim NameReportNew As String



            Dim i As Integer
            Dim sGruppo() As String         'matrice parametri/valori
            Dim sGruppo1() As String        'matrice sottoreport
            Dim sElemt() As String
            'Dim dbg As Char = ConfigurationSettings.AppSettings("debugApp")


            'cmdLog.CommandText = "insert into _log values ('createpdf 2')"
            'cmdLog.ExecuteNonQuery()

            GetPdfError = ""

            NameReportNew = UCase(strUserName) & "-" & Format(Now, "dd-MM-yyyyhh-mm-ss")




 		Try

            crReportDocument.Load(myPath.Server.MapPath(NomeReport))

            'cmdLog.CommandText = "insert into _log values ('createpdf 3')"
            'cmdLog.ExecuteNonQuery()

            sGruppo = Split(StrDati, ":")
            '1)parametri report*****************************
            For i = 0 To UBound(sGruppo) - 1
                sElemt = Split(sGruppo(i), ",")
                paramFieldDt.ParameterFieldName = "@" & sElemt(0)       'nome campo
                discreteValDt.Value = sElemt(1)                           'valore campo
                paramFieldDt.CurrentValues.Add(discreteValDt)

                Dim paramFieldDefDt As ParameterFieldDefinition = crReportDocument.DataDefinition.ParameterFields.Item(sElemt(0))

                Dim ParameterValuesDt As ParameterValues = paramFieldDt.CurrentValues
                paramFieldDefDt.ApplyCurrentValues(ParameterValuesDt)
            Next i
            '******************************************
            If ReportStorico = 1 Then
                With logOnInfo
                    .ConnectionInfo.Password = ConfigurationSettings.AppSettings("connectionPassword")
                    .ConnectionInfo.ServerName = ConfigurationSettings.AppSettings("connectionServerName")
                    .ConnectionInfo.DatabaseName = ConfigurationSettings.AppSettings("PDFConnectionDatabaseNameStorico")
                    .ConnectionInfo.UserID = ConfigurationSettings.AppSettings("connectionUserid")
                End With
            Else
                With logOnInfo
                    .ConnectionInfo.Password = ConfigurationSettings.AppSettings("connectionPassword")
                    .ConnectionInfo.ServerName = ConfigurationSettings.AppSettings("connectionServerName")
                    .ConnectionInfo.DatabaseName = ConfigurationSettings.AppSettings("PDFConnectionDatabaseName")
                    .ConnectionInfo.UserID = ConfigurationSettings.AppSettings("connectionUserid")
                End With
            End If


            crReportDocument.Database.Tables(0).ApplyLogOnInfo(logOnInfo)
            '******************************************

            '3)gestione sotto report*********************
            sGruppo1 = Split(SottoReport, ":")
            i = 0
            For i = 0 To UBound(sGruppo1) - 1
                crReportDocument.OpenSubreport(sGruppo1(i)).Database.Tables(0).ApplyLogOnInfo(logOnInfo)
            Next i
            '******************************************

            '4)esportazione report in PDF***************
            Dim crDiskFileDestinationOptions As New CrystalDecisions.Shared.DiskFileDestinationOptions
            Dim crExportOptions As CrystalDecisions.Shared.ExportOptions

            ' Dim PathREPORT As String = ConfigurationSettings.AppSettings("PathREPORT")


            crDiskFileDestinationOptions.DiskFileName = myPath.Server.MapPath("reports/" & NameReportNew & ".pdf")
            'crDiskFileDestinationOptions.DiskFileName = ConfigurationSettings.AppSettings("PathREPORT") & NameReportNew & ".pdf"

            'cmdLog.CommandText = "insert into _log values ('" & Replace(crDiskFileDestinationOptions.DiskFileName, "'", "''") & "')"
            'cmdLog.ExecuteNonQuery()

            crExportOptions = crReportDocument.ExportOptions
            crExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
            crExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
            crExportOptions.DestinationOptions = crDiskFileDestinationOptions

            crReportDocument.Export()
            crReportDocument.Close()

            Return "reports/" & NameReportNew & ".pdf"

            '******************************************

        Catch ex As Exception



            'cmdLog.CommandText = "insert into _log values ('" & Replace(ex.Message, "'", "''") & "')"
            'cmdLog.ExecuteNonQuery()


            GetPdfError = ex.Message '"ERRORE" '
        End Try

    End Function
    Function DecodeToByte(ByVal enc As String) As Byte()
        Dim bt() As Byte
        bt = System.Convert.FromBase64String(enc)
        Return bt
    End Function
    Sub DecodeFile(ByVal srcFile As String, ByVal destFile As String)
        'Dim src As String
        'Dim sr As New IO.StreamReader(srcFile)
        'src = sr.ReadToEnd
        'sr.Close()
        Dim bt64 As Byte() = DecodeToByte(srcFile)
        If IO.File.Exists(destFile) Then
            IO.File.Delete(destFile)
        End If

        Dim sw As New IO.FileStream(destFile, IO.FileMode.CreateNew)
        sw.Write(bt64, 0, bt64.Length)
        sw.Close()
    End Sub

    <WebMethod()> _
    Public Function Async_InserimentoDocumentiProgetti(ByVal IdAttività As String, ByVal IdAttivitaDocumento As String, ByVal username As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_DocumentiProgetti_Applica]"
        Try
            'RECUPERO DATI HELIOS
            sqlConn.ConnectionString = ConfigurationSettings.AppSettings("cnHelios")
            sqlConn.Open()

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, sqlConn)
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.CommandTimeout = 1200
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
            Return ex.Message.ToString()
            Exit Function
        End Try
    End Function

    <WebMethod()> _
    Public Function Async_COMP_Elaborazione(ByVal IdElaborazione As String, ByVal UserNameRichiesta As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_COMP_ELABORAZIONE]"


        Try
            'RECUPERO DATI HELIOS
            sqlConn.ConnectionString = ConfigurationSettings.AppSettings("cnHelios")
            sqlConn.Open()

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, sqlConn)
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.CommandTimeout = 10800
            sqlCMD.Parameters.Add("@IdElaborazione", SqlDbType.Int).Value = IdElaborazione
            sqlCMD.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar).Value = UserNameRichiesta

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            'sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.TinyInt
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@messaggio"
            sparam2.Size = 1000
            sparam2.SqlDbType = SqlDbType.VarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)

            sqlCMD.ExecuteNonQuery()
            Dim str As String
            str = sqlCMD.Parameters("@messaggio").Value
            Return str

        Catch ex As Exception
            Return ex.Message.ToString()
            Exit Function
        End Try
    End Function

    <WebMethod()> _
    Public Function GenerazioneAllegato6_ElencoSedi(ByVal IdEnteFase As Integer, ByVal NomeUtente As String)

        'Dim localWS As New WS_Editor.WSMetodiDocumentazione
        Dim strsql As String
        Dim ds As New DataSet
        Dim SqlDA As SqlDataAdapter
        Dim i As Integer
        Dim strCodiceEnte As String

        Dim dtrLocal As SqlDataReader
        Dim myCommand As SqlCommand
        Dim tipofase As Integer

        'RECUPERO DATI HELIOS
        sqlConn.ConnectionString = ConfigurationSettings.AppSettings("cnHelios")
        sqlConn.Open()

        strsql = "select tipofase from entifasi where identefase = " & IdEnteFase
        myCommand = New SqlClient.SqlCommand
        myCommand.Connection = sqlConn

        myCommand.CommandText = strsql
        dtrLocal = myCommand.ExecuteReader
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            tipofase = dtrLocal("tipofase")
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        If tipofase = 3 Then 'art.2
            strsql = " SELECT DISTINCT E.IDEnte,E.CodiceRegione from EntiFasi_Sedi  EFS"
            strsql &= " INNER JOIN entifasi EF ON EFS.IdEnteFASE= EF.IdEnteFASE"
            strsql &= " INNER JOIN entifasi EFART2 ON EF.IdEnteFASE= EFART2.IdEnteFASERiferimento"
            strsql &= " INNER JOIN entisediattuazioni ESA ON EFS.IdEnteSedeAttuazione= ESA.IDEnteSedeAttuazione"
            strsql &= " INNER JOIN entisedi ES ON ESA.IDEnteSede=ES.IDEnteSede"
            strsql &= " INNER JOIN enti E ON ES.IDEnte=E.IDEnte"
            strsql &= " WHERE EFART2.identefase = " & IdEnteFase & " AND efs.Azione<>'Richiesta Cancellazione' and isnull(VariazioneArt2,0) = 1 "

        Else 'accreditamento / adeguamento
            strsql = " SELECT DISTINCT E.IDEnte,E.CodiceRegione from EntiFasi_Sedi  EFS"
            strsql &= " INNER JOIN entisediattuazioni ESA ON EFS.IdEnteSedeAttuazione= ESA.IDEnteSedeAttuazione"
            strsql &= " INNER JOIN entisedi ES ON ESA.IDEnteSede=ES.IDEnteSede"
            strsql &= " INNER JOIN enti E ON ES.IDEnte=E.IDEnte"
            strsql &= " WHERE EFS.identefase = " & IdEnteFase & " AND efs.Azione<>'Richiesta Cancellazione' "
        End If


        SqlDA = New SqlDataAdapter(strsql, sqlConn)
        SqlDA.Fill(ds)

        'Dim cmdLog As New SqlCommand
        'cmdLog.Connection = sqlConn

        If ds.Tables(0).Rows.Count > 0 Then
            Dim NomeReport As String
            Dim strNomeFile As String
            For i = 0 To ds.Tables(0).Rows.Count - 1

                'elenco sedi allegato 6
                NomeReport = "crpAllegato6_ElencoSedi.rpt"
                strNomeFile = "BoxSedi_"
                strCodiceEnte = ds.Tables(0).Rows.Item(i).Item("CodiceRegione")

                Dim strFile As String
                strFile = GeneraAllegato6_ElencoSedi(IdEnteFase, ds.Tables(0).Rows.Item(i).Item("IDEnte"), NomeUtente, NomeReport) ',

                DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceEnte & ".pdf"))

                clsGestioneDocumenti.CaricaAllegatoSediEnte(IdEnteFase, ds.Tables(0).Rows.Item(i).Item("IDEnte"), NomeUtente, strNomeFile & strCodiceEnte & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceEnte & ".pdf"), sqlConn)

            Next
        End If

        Dim cmdUp As SqlCommand
        'update InLavorazione a 0 
        'aggiunto flag LAVORAZIONE =1 da simona cordella il 16/10/2012
        cmdUp = New Data.SqlClient.SqlCommand(" UPDATE EntiFasi" & _
                                              " SET InLavorazione= 0  " & _
                                              " WHERE IdEnteFase=" & IdEnteFase & "", sqlConn)
        cmdUp.ExecuteNonQuery()
        cmdUp.Dispose()
    End Function

    Public Function GeneraAllegato6_ElencoSedi(ByVal IdEnteFase As Integer, ByVal IdEnte As Integer, ByVal username As String, ByVal NomeReport As String) As String
        Dim sDati As String
        Dim strEsito As String

        sDati = "@IdEnteFase," & IdEnteFase & ":@IdEnte," & IdEnte & ":"

        Try

            strEsito = CreatePdf(NomeReport, sDati, username)

            If strEsito = "ERRORE" Then
                Return strEsito
            Else
                Return FileToBase64(strEsito)
            End If

        Catch ex As Exception
            Return ex.Message '"ERRORE"
        End Try

    End Function
End Class
