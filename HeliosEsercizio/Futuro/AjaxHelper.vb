Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Class AjaxHelper
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetIndirizzi(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager _
             .ConnectionStrings("unscproduzionenewConnectionString").ConnectionString


        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = " Select Top 30 CAP_INDIRIZZI.Indirizzo as CityName FROM  CAP_INDIRIZZI WHERE (CAP_INDIRIZZI.Indirizzo LIKE '%" + prefixText.Replace("'", "''") + "%') and idcomune='" & contextKey & "'  ORDER BY CAP_INDIRIZZI.Indirizzo"
        cmd.Connection = conn
        conn.Open()

        Dim oReader As SqlDataReader = cmd.ExecuteReader
        Dim indirizzi As List(Of String) = New List(Of String)

        While oReader.Read
            indirizzi.Add(oReader.GetString(0))
        End While


        If Not oReader Is Nothing Then
            oReader.Close()
            oReader = Nothing
            conn.Close()
        End If
        Return indirizzi


    End Function
End Class
