Imports System.Data.SqlClient
Imports System.Drawing

Public Class WfrmDettaglioAssegnazioneImportiVirtuali
    Inherits System.Web.UI.Page
    Dim strSql As String
#Region "Utility"

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Page.IsPostBack = False Then
            CaricaDataGrid(dtgAssegna)
        End If
    End Sub

    Private Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid)
        strSql = "SELECT DISTINCT attività.titolo, TipiProgetto.Descrizione, Errori_volontari_attivita.VolontariAttività, Errori_volontari_attivita.VolontariSediAtt,Errori_volontari_attivita.idbando, Errori_volontari_attivita.idattività " & _
                " FROM Errori_volontari_attivita " & _
                " INNER Join attività ON Errori_volontari_attivita.idAttività = attività.IDAttività " & _
                " INNER Join TipiProgetto ON attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
                " INNER JOIN nazioni ON Errori_volontari_attivita.NazioneBase = nazioni.NazioneBase " & _
                " WHERE Errori_volontari_attivita.idbando ='" & Session("IdBando") & "' " & _
                " Order by attività.titolo"
        GridDaCaricare.DataSource = ClsServer.DataSetGenerico(strSql, Session("conn"))
        GridDaCaricare.DataBind()
        GridDaCaricare.Visible = True
        If GridDaCaricare.Items.Count <> 0 Then
            GridDaCaricare.Caption = "Elenco eccezioni durante l'assegnazione degli importi virtuali al volontario"
        Else
            GridDaCaricare.Visible = False
            MessaggiConvalida("Nessuna eccezione nell'assegnazione degli importi virtuali al volontario.")
        End If
    End Sub
    Private Sub MessaggiConvalida(ByVal strMessaggio)
        lblInfo.Text = strMessaggio
    End Sub

End Class