Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary


Public Class WfrmSIGEDDocumento
    Inherits System.Web.UI.Page
    Dim SIGED As clsSiged

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTestoUP As System.Web.UI.WebControls.Label
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents lblNomeFile As System.Web.UI.WebControls.Label
    Protected WithEvents cmdScarica As System.Web.UI.WebControls.HyperLink

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    Public strPath As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strNome As String
        Dim strCognome As String
        Dim strSQL As String
        Dim dsUser As DataSet
        Try

            'Dim wsIN As New WS_Verifiche.VERIFICHEs
            'Dim wsOUT As New WS_Verifiche.RESTITUZIONE_DOCUMENTO_RISPOSTA
            Dim myPath As New System.Web.UI.Page
            Dim PathServerSiged As String

            Dim sEsito As String
            Dim pNumeroProtocollo As String = Request.QueryString("NumeroProtocollo")
            If pNumeroProtocollo <> "" Then pNumeroProtocollo = Request.QueryString("NumeroProtocollo")
            Dim pDataProtocollo As String = Request.QueryString("DataProtocollo")
            Dim pAnnoProtocollo As String
            Dim pCodiceFascicolo As String = Request.QueryString("CodiceFascicolo")
            Dim pIdentificativoDOC As String = Request.QueryString("IdentificativoDOC")
            Dim pNomeFile As String = Request.QueryString("NomeFile")


            ' pDataProtocollo = Request.QueryString("DataProtocollo")

            strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & Session("Utente") & "'"

            dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

            If dsUser.Tables(0).Rows.Count <> 0 Then
                strNome = dsUser.Tables(0).Rows(0).Item("Nome")
                strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
            End If

            pDataProtocollo = Year(CDate(pDataProtocollo))
            'End If

            '\\protest\wsTemp\Sum con LINQPAD.txt
            PathServerSiged = "\\" & ConfigurationSettings.AppSettings("SERVER_SIGED") & "\" & ConfigurationSettings.AppSettings("CARTELLA_SIGED")  '& "\" & pNomeFile

            strPath = myPath.Server.MapPath("download\") & pNomeFile
            lblNomeFile.Text = "Nome File: " & pNomeFile
            Documento(strCognome, strNome, pIdentificativoDOC, pDataProtocollo, pNomeFile, PathServerSiged, strPath)


        Catch ex As Exception
            lblNomeFile.Text = "Errore imprevisto: " & ex.Message
            lblTestoUP.Visible = False
            cmdScarica.Visible = False
        End Try
        SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
    End Sub
    Public Function FileToBase64(ByVal fileName As String) As String
        Dim bFile() As Byte
        Dim fs As FileStream
        Dim _textB64 As String
        Try
            fs = New FileStream(fileName, FileMode.Open)
            ReDim bFile(fs.Length - 1)
            fs.Read(bFile, 0, fs.Length)
            _textB64 = Convert.ToBase64String(bFile)
        Catch ex As Exception
            'gestione eccezione
        Finally
            fs.Close()
        End Try
        Return _textB64
    End Function

    Private Sub Base64ToFile(ByVal base64String As String, ByVal dstFilePath As String)

        Dim dataBuffer As Byte() = Convert.FromBase64String(base64String)
        Dim fs As FileStream
        fs = New FileStream(dstFilePath, FileMode.Create, FileAccess.Write)
        If (dataBuffer.Length > 0) Then
            fs.Write(dataBuffer, 0, dataBuffer.Length)
        End If

        fs.Close()

    End Sub

    Private Sub Documento(ByVal Cognome As String, ByVal Nome As String, ByVal CodiceAllegato As String, ByVal AnnoProtocollo As Integer, ByVal NomeFile As String, ByVal PathSiged As String, ByVal PathLocale As String)

        Dim wsOUT As New WS_SIGeD.INDICE_ALLEGATO
        'Dim WsDati As WS_SIGeD.ALLEGATO_DOCUMENTO
        Dim dr As DataRow
        Dim riga As Integer
        Try

            SIGED = New clsSiged("", Nome, Cognome)
            If SIGED.Codice_Esito <> 0 Then
                Exit Sub
            End If

            wsOUT = SIGED.SIGED_RestituisciAllegato(AnnoProtocollo, CodiceAllegato, "NO", PathSiged & "\" & NomeFile)
            If Left(wsOUT.ESITO, 5) = "00000" Then
                'WsDati = wsOUT.DATI
                If File.Exists(PathLocale) = True Then
                    File.Delete(PathLocale)
                    'PathSiged = Replace(PathSiged, ".serviziocivile.it", "")
                End If
                File.Copy(Trim(PathSiged) & "\" & Trim(NomeFile), Trim(PathLocale))
                lblTestoUP.Visible = True
                cmdScarica.Visible = True
                cmdScarica.NavigateUrl = "download\" & NomeFile
            Else
                lblNomeFile.Text = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))
                lblTestoUP.Visible = False
                cmdScarica.Visible = False

            End If
        Catch ex As Exception
            lblNomeFile.Text = "Errore imprevisto: " & ex.Message
            lblTestoUP.Visible = False
            cmdScarica.Visible = False
        End Try

    End Sub

End Class
