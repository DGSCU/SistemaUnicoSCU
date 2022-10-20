Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Configuration.ConfigurationManager
Imports System.Web.Services.WebService

Public Class wfrmSuggerimenti
    Inherits System.Web.UI.Page
  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            Dim cercaSuggerimenti As New clsSuggerimenti(Request.QueryString("strIndirizzo"), Request.QueryString("idcomune"), Session("conn"))
            Dim dtsLocal As New DataSet
            Dim intX As Integer
            Dim intY As Integer
            Dim arrayIndirizzo As Array = Split(Replace(Request.QueryString("strIndirizzo"), "|", " "), " ")
            Dim idComune As String
            idComune = Request.QueryString("idcomune")
            AutoCompleteExtender1.ContextKey = idComune
            'AutoCompleteExtender1.ContextKey = "34022"
            'variabile necessaria al completamento AJAX
            Session("IdComune") = Request.QueryString("idcomune")

            lblIndirizzo.Text = """" & Replace(Request.QueryString("strIndirizzo"), "|", "'") & """"

            For intX = 0 To UBound(arrayIndirizzo)
                Dim checkParola As New clsControllaParoleIndirizzi(arrayIndirizzo.GetValue(intX))
                If checkParola.CheckIndirizzo <> "" Then
                    Dim cercaSuggerimentiPerParola As New clsSuggerimenti(checkParola.CheckIndirizzo, Request.QueryString("idcomune"), Session("conn"))

                    dtsLocal = cercaSuggerimentiPerParola.IndirizzoSimilePerParola

                    For intY = 0 To dtsLocal.Tables(0).Rows.Count - 1
                        lblSuggerimenti.Text = lblSuggerimenti.Text & dtsLocal.Tables(0).Rows.Item(intY).Item(0) & "<br>"
                    Next
                End If
            Next

            dtsLocal = cercaSuggerimenti.IndirizziSimili

            For intX = 0 To dtsLocal.Tables(0).Rows.Count - 1
                lblSuggerimenti.Text = lblSuggerimenti.Text & dtsLocal.Tables(0).Rows.Item(intX).Item(0) & "<br>"
            Next


        End If


    End Sub
   
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim localConn As New SqlClient.SqlConnection
        localConn.ConnectionString = "user id=sa;password=vbra250;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=unscproduzionenew"
        localConn.Open()


        Dim aOut As ArrayList = New ArrayList

        Dim x As String()
        Dim i As Integer = 0
        Dim strsql As String


        strsql = " Select Top 30 CAP_INDIRIZZI.Indirizzo as CityName FROM  CAP_INDIRIZZI WHERE (CAP_INDIRIZZI.Indirizzo LIKE '%" + prefixText.Replace("'", "''") + "%') and idcomune='" & contextKey & "'  ORDER BY CAP_INDIRIZZI.Indirizzo"



        Dim oReader As SqlClient.SqlDataReader


        oReader = ClsServer.CreaDatareader(strsql, localConn)

        While oReader.Read()
            ReDim Preserve x(i)
            x(i) = oReader.GetString(0)
            i = i + 1
        End While

        If Not oReader Is Nothing Then
            oReader.Close()
            oReader = Nothing
        End If

        

        Return x


    End Function

    Protected Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
     
        CercaVie(Request.QueryString("idcomune"), txtIndirizzo.Text)
    End Sub
    Function CercaVie(ByVal idcomune As String, ByVal TestoDaCercare As String) As DataSet
        Dim strsql As String
        Dim dtsGenerico As DataSet

        strsql = " Select Top 30 CAP_INDIRIZZI.Indirizzo as Indirizzo FROM  CAP_INDIRIZZI WHERE (CAP_INDIRIZZI.Indirizzo LIKE '%" + TestoDaCercare.Replace("'", "''") + "%') and idcomune='" & idcomune & "'  ORDER BY CAP_INDIRIZZI.Indirizzo"
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
       
        CercaVie = dtsGenerico

        listaIndirizzi.DataSource = dtsGenerico
        listaIndirizzi.DataBind()
        
        dtsGenerico = Nothing

        Return CercaVie

    End Function
End Class