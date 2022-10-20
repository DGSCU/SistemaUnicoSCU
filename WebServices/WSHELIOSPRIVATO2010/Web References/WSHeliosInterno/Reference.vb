﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Il codice è stato generato da uno strumento.
'     Versione runtime:4.0.30319.34209
'
'     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
'     il codice viene rigenerato.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'Il codice sorgente è stato generato automaticamente da Microsoft.VSDesigner, versione 4.0.30319.34209.
'
Namespace WSHeliosInterno
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="HeliosInternoSoap", [Namespace]:="http://tempuri.org/")>  _
    Partial Public Class HeliosInterno
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private CreaFascicoloIstanzaOperationCompleted As System.Threading.SendOrPostCallback
        
        Private InviaPECOperationCompleted As System.Threading.SendOrPostCallback
        
        Private UploadContrattoVolontarioOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.WsHeliosPrivato2010.My.MySettings.Default.WsHeliosPrivato2008_WSHeliosInterno_HeliosInterno
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event CreaFascicoloIstanzaCompleted As CreaFascicoloIstanzaCompletedEventHandler
        
        '''<remarks/>
        Public Event InviaPECCompleted As InviaPECCompletedEventHandler
        
        '''<remarks/>
        Public Event UploadContrattoVolontarioCompleted As UploadContrattoVolontarioCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CreaFascicoloIstanza", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function CreaFascicoloIstanza(ByVal IdBando As Integer, ByVal IdEnte As Integer) As String
            Dim results() As Object = Me.Invoke("CreaFascicoloIstanza", New Object() {IdBando, IdEnte})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub CreaFascicoloIstanzaAsync(ByVal IdBando As Integer, ByVal IdEnte As Integer)
            Me.CreaFascicoloIstanzaAsync(IdBando, IdEnte, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub CreaFascicoloIstanzaAsync(ByVal IdBando As Integer, ByVal IdEnte As Integer, ByVal userState As Object)
            If (Me.CreaFascicoloIstanzaOperationCompleted Is Nothing) Then
                Me.CreaFascicoloIstanzaOperationCompleted = AddressOf Me.OnCreaFascicoloIstanzaOperationCompleted
            End If
            Me.InvokeAsync("CreaFascicoloIstanza", New Object() {IdBando, IdEnte}, Me.CreaFascicoloIstanzaOperationCompleted, userState)
        End Sub
        
        Private Sub OnCreaFascicoloIstanzaOperationCompleted(ByVal arg As Object)
            If (Not (Me.CreaFascicoloIstanzaCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent CreaFascicoloIstanzaCompleted(Me, New CreaFascicoloIstanzaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/InviaPEC", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function InviaPEC(ByVal CodiceEnte As String, ByVal PecDestinatario As String) As String
            Dim results() As Object = Me.Invoke("InviaPEC", New Object() {CodiceEnte, PecDestinatario})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub InviaPECAsync(ByVal CodiceEnte As String, ByVal PecDestinatario As String)
            Me.InviaPECAsync(CodiceEnte, PecDestinatario, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub InviaPECAsync(ByVal CodiceEnte As String, ByVal PecDestinatario As String, ByVal userState As Object)
            If (Me.InviaPECOperationCompleted Is Nothing) Then
                Me.InviaPECOperationCompleted = AddressOf Me.OnInviaPECOperationCompleted
            End If
            Me.InvokeAsync("InviaPEC", New Object() {CodiceEnte, PecDestinatario}, Me.InviaPECOperationCompleted, userState)
        End Sub
        
        Private Sub OnInviaPECOperationCompleted(ByVal arg As Object)
            If (Not (Me.InviaPECCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent InviaPECCompleted(Me, New InviaPECCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UploadContrattoVolontario", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function UploadContrattoVolontario(ByVal IdVol As Integer, ByVal File As String, ByVal NomeFile As String) As String
            Dim results() As Object = Me.Invoke("UploadContrattoVolontario", New Object() {IdVol, File, NomeFile})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub UploadContrattoVolontarioAsync(ByVal IdVol As Integer, ByVal File As String, ByVal NomeFile As String)
            Me.UploadContrattoVolontarioAsync(IdVol, File, NomeFile, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub UploadContrattoVolontarioAsync(ByVal IdVol As Integer, ByVal File As String, ByVal NomeFile As String, ByVal userState As Object)
            If (Me.UploadContrattoVolontarioOperationCompleted Is Nothing) Then
                Me.UploadContrattoVolontarioOperationCompleted = AddressOf Me.OnUploadContrattoVolontarioOperationCompleted
            End If
            Me.InvokeAsync("UploadContrattoVolontario", New Object() {IdVol, File, NomeFile}, Me.UploadContrattoVolontarioOperationCompleted, userState)
        End Sub
        
        Private Sub OnUploadContrattoVolontarioOperationCompleted(ByVal arg As Object)
            If (Not (Me.UploadContrattoVolontarioCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent UploadContrattoVolontarioCompleted(Me, New UploadContrattoVolontarioCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")>  _
    Public Delegate Sub CreaFascicoloIstanzaCompletedEventHandler(ByVal sender As Object, ByVal e As CreaFascicoloIstanzaCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CreaFascicoloIstanzaCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")>  _
    Public Delegate Sub InviaPECCompletedEventHandler(ByVal sender As Object, ByVal e As InviaPECCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class InviaPECCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")>  _
    Public Delegate Sub UploadContrattoVolontarioCompletedEventHandler(ByVal sender As Object, ByVal e As UploadContrattoVolontarioCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class UploadContrattoVolontarioCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
End Namespace