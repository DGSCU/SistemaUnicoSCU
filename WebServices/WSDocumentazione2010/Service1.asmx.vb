Imports System.Web.Services

<System.Web.Services.WebService(Namespace := "http://tempuri.org/WSDocumentazione/Service1")> _
Public Class Service1
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
    '<WebMethod()> _
    'Public Function HelloWorld() As String
    '   Return "Salve gente!"
    'End Function

End Class
