Imports System.Data.SqlClient

Public Class WfrmRisFinanzAgg
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

    Private Sub CancellaMessaggi()
        msgErrore.Text = String.Empty
    End Sub
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Page.IsPostBack = False Then
            txtnumvol.Text = Session("TotVol")
        End If

    End Sub


    Private Sub cmdCalcolaPerc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCalcolaPerc.Click
        CancellaMessaggi()
        Dim divisore As Double
        Dim dividendo As Double
        Dim percentuale As Integer
        If txtnumvol.Text = String.Empty Or txtnumvol.Text = 0 Then
            txtnumvol.Text = 0
            msgErrore.Text = "Inserire un valore per il campo N° Volontari."
            Exit Sub
        End If
        If txtimporto.Text = String.Empty Or txtimporto.Text = 0 Then
            txtimporto.Text = 0
            Exit Sub
        End If
        dividendo = CDbl(txtimporto.Text)
        divisore = CInt(txtnumvol.Text) * 12 * 433.06
        percentuale = dividendo / divisore * 100
        txtperc.Text = percentuale
    End Sub
End Class