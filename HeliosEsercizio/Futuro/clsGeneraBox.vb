Imports System.Web.Services
Imports System.Security.Cryptography
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports
Imports System.IO
Imports System.Web.Mail
Imports System.Configuration
Imports System.Text

Public Class clsGeneraBox
    Inherits System.Web.UI.Page

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

    Public Function GeneraFile(ByVal idAttivita As Integer, ByVal username As String, ByVal NomeReport As String) As String
        Dim sDati As String
        Dim strEsito As String

        sDati = "IdAttivita," & idAttivita & ":"

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

    Function CreatePdf(ByVal NomeReport As String, ByVal StrDati As String, ByVal strUserName As String, Optional ByVal SottoReport As String = "", Optional ByVal ReportStorico As Int16 = 1) As String
        '*************************************************************************************************
        'DESCRIZIONE: Genera il PDF nella directory Reports/Export del report selezionato
        'AUTORE: TESTA GUIDO    DATA: 04/10/2004
        '*************************************************************************************************
        Dim paramFieldDt As New ParameterField
        Dim discreteValDt As New ParameterDiscreteValue
        Dim myPath As New System.Web.UI.Page
        Dim crReportDocument As New ReportDocument
        Dim logOnInfo As New TableLogOnInfo
        Dim NameReportNew As String
        Dim i As Integer
        Dim sGruppo() As String         'matrice parametri/valori
        Dim sGruppo1() As String        'matrice sottoreport
        Dim sElemt() As String
        Dim GetPdfError As String
        Dim dbg As Char = ConfigurationSettings.AppSettings("debugApp")

        GetPdfError = ""

        NameReportNew = UCase(strUserName) & "-" & Format(Now, "dd-MM-yyyyhh-mm-ss")

        Try

            crReportDocument.Load(myPath.Server.MapPath(NomeReport))

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


            crDiskFileDestinationOptions.DiskFileName = myPath.Server.MapPath("reports/" & NameReportNew & ".pdf")
            crExportOptions = crReportDocument.ExportOptions
            crExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
            crExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
            crExportOptions.DestinationOptions = crDiskFileDestinationOptions

            crReportDocument.Export()
            crReportDocument.Close()

            Return "reports/" & NameReportNew & ".pdf"

            '******************************************

        Catch ex As Exception
            GetPdfError = ex.Message '"ERRORE" '
        End Try

    End Function
End Class
