Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net.Http
Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports System.Text
Imports System.IO
Imports System.Net

Public Class Service
    'Inherits IService

    Private urlLoginVolontario As String '= "http://testingdol//Account/CheckCredenziali"
    Private urlInputCambioPassword As String '= "http://testingdol/Account/CambiaPasswordJson"
    Private urlInputRecuperoPassword As String '= "http://testingdol/Account/RecuperoPasswordJson"

    Public Function ClientRest(Of T)(ByVal url As String, ByVal parameters As Object) As T
        Dim client As HttpClient = New HttpClient()
        Dim httpParameters = New Dictionary(Of String, String)()

        For Each proprieta As PropertyInfo In parameters.[GetType]().GetProperties()

            If proprieta.GetValue(parameters, Nothing) IsNot Nothing Then httpParameters.Add(proprieta.Name, proprieta.GetValue(parameters, Nothing).ToString())

        Next

        Dim encodedContent = New FormUrlEncodedContent(httpParameters)
            Dim response As HttpResponseMessage = client.PostAsync(url, encodedContent).Result

            If response.IsSuccessStatusCode Then
                Dim text = response.Content.ReadAsStringAsync().Result

                Try
                    Return JsonConvert.DeserializeObject(Of T)(text)
                Catch ex As Exception
                    Throw New Exception("Errore nella deserizalizzazione della risposta", ex)
                End Try
            Else
                Throw New Exception("Errore nalla chiamata del servizio:(" & response.StatusCode & ") - " & response.ReasonPhrase)
            End If

    End Function

    Public Function LoginVolontario(ByVal userName As String, ByVal password As String) As ResponseData
        urlLoginVolontario = System.Configuration.ConfigurationManager.AppSettings("urlLoginVolontario")
        Return ClientRest(Of ResponseData)(urlLoginVolontario, New inputLoginVolontario With {
            .userName = userName,
            .password = password
        })
    End Function

    Public Function RecuperoPassword(ByVal userName As String, ByVal returnUrl As String) As ResponseData
        urlInputRecuperoPassword = System.Configuration.ConfigurationManager.AppSettings("urlInputRecuperoPassword")
        Return ClientRest(Of ResponseData)(urlInputRecuperoPassword, New inputRecuperoPassword With {
            .codiceFiscale = userName,
            .returnUrl = returnUrl
        })
    End Function

    Public Function CambioPassword(ByVal userName As String, ByVal passwordAttuale As String, ByVal password As String) As ResponseData
        urlInputCambioPassword = System.Configuration.ConfigurationManager.AppSettings("urlInputCambioPassword")
        Return ClientRest(Of ResponseData)(urlInputCambioPassword, New inputCambioPassword With {
            .userName = userName,
            .passwordAttuale = passwordAttuale,
            .password = password
        })
    End Function

    Public Function GetDataUsingDataContract(ByVal composite As CompositeType) As CompositeType
        If composite Is Nothing Then
            Throw New ArgumentNullException("composite")
        End If

        If composite.BoolValue Then
            composite.StringValue += "Suffix"
        End If

        Return composite
    End Function
End Class
