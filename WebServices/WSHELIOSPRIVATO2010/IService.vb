Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports System.Text

<ServiceContract>
Interface IService
    <OperationContract>
    Function LoginVolontario(ByVal codiceFiscale As String, ByVal password As String) As ResponseData
    <OperationContract>
    Function RecuperoPassword(ByVal userName As String, ByVal returnUrl As String) As ResponseData
    <OperationContract>
    Function CambioPassword(ByVal userName As String, ByVal passwordAttuale As String, ByVal password As String) As ResponseData
    <OperationContract>
    Function GetDataUsingDataContract(ByVal composite As CompositeType) As CompositeType
End Interface

<DataContract>
Public Class CompositeType


    <DataMember>
    Public Property boolValue As Boolean
        Get
            Return boolValue
        End Get
        Set(ByVal value As Boolean)
            boolValue = value
        End Set
    End Property

    <DataMember>
    Public Property StringValue As String

        Get
            Return StringValue
        End Get
        Set(ByVal value As String)
            StringValue = value
        End Set
    End Property
End Class

