Imports System.Data.SqlClient

Public Class WfrmCalcoloRilevanzaCoerenza
    Inherits System.Web.UI.Page
#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub


    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Page.IsPostBack = False Then
            CaricaDati()
        End If
    End Sub

    Sub CaricaDati()
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim myCommand As SqlClient.SqlCommand = New SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        myCommand.CommandText = "Select isnull(Rilevanza,'0') as Rilevanza FROM CalcoloRilevanzaCoerenza WHERE (Nazionale = " & IIf(Request.QueryString("TipoProgetto") = 1, 1, 0) & ") AND (Contesto = " & Request.QueryString("Contesto") & ") AND (Obiettivi = " & Request.QueryString("Obiettivi") & ")"

        dtrLocal = myCommand.ExecuteReader

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            lblRilevanza.Text = dtrLocal("Rilevanza")
        Else
            lblRilevanza.Text = "Errore nel calcolo della Rilevanza."
        End If

        ChiudiDataReader(dtrLocal)

        myCommand.CommandText = "SELECT isnull(Coerenza,'0') as Coerenza FROM CalcoloRilevanzaCoerenza WHERE (Nazionale = " & IIf(Request.QueryString("TipoProgetto") = 1, 1, 0) & ") AND (Obiettivi = " & Request.QueryString("Obiettivi") & ") AND (Descrizione = " & Request.QueryString("Descrizione") & ")"

        dtrLocal = myCommand.ExecuteReader

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            lblCoerenza.Text = dtrLocal("Coerenza")
        Else
            lblCoerenza.Text = "Errore nel calcolo della Coerenza."
        End If
        ChiudiDataReader(dtrLocal)

        lblDescrizione.Text = Request.QueryString("Descrizione")
        lblContesto.Text = Request.QueryString("Contesto")
        lblObiettivi.Text = Request.QueryString("Obiettivi")

    End Sub

End Class