Imports System.Web.Services
Imports System.DirectoryServices

<System.Web.Services.WebService(Namespace:="http://tempuri.org/WSAUTENTICAZIONE/wsAutenticazione")> _
Public Class wsAutenticazione
    Inherits System.Web.Services.WebService

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
        components = New System.ComponentModel.Container
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
Public Function EseguiAutenticazione(ByVal username As String, ByVal pwd As String) As String
        Dim decriptadati As New criptString
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim chkEsistenza As String = "0"

        'username = decriptadati.EncryptData(username)
        'pwd = decriptadati.EncryptData(pwd)

        Dim AD_DE As DirectoryEntry = New DirectoryEntry("LDAP://" & "obiettori", decriptadati.DecryptData(username), decriptadati.DecryptData(pwd))

        Try
            'Bind to the native AdsObject to force authentication.			
            Dim obj As Object = AD_DE.NativeObject
            Dim search As DirectorySearcher = New DirectorySearcher(AD_DE)

            'search.Filter = "(SAMAccountName=" & "pippo" & ")"
            'search.PropertiesToLoad.Add("cn")
            Dim result As SearchResult = search.FindOne()

            If (result Is Nothing) Then
                chkEsistenza = "0"
            End If

            chkEsistenza = "1"

        Catch ex As System.Runtime.InteropServices.COMException
            If ex.ErrorCode = -2147023570 Then
                chkEsistenza = "0"
            Else
                chkEsistenza = "-1"
            End If
        End Try

        dtT.Columns.Add(New DataColumn("ESITO", GetType(String)))

        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()

        'carico la prima riga della datatable
        Select Case chkEsistenza
            Case "0"
                dtR(0) = "NEGATIVO"
            Case "1"
                dtR(0) = "POSITIVO"
            Case "-1"
                dtR(0) = "ERRORE"
        End Select

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "EsitoAutenicazione"

        dtsLocal.Tables(0).TableName = "DettaglioEsito"

        EseguiAutenticazione = dtsLocal.GetXml.ToString

        Return EseguiAutenticazione

    End Function

End Class
