Imports System.Data.SqlClient

Public Class WfrmRiepilogoAntimafia
    Inherits System.Web.UI.Page
    Dim myQuerySql As String
    Dim myDataReader As SqlClient.SqlDataReader

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

    Private Sub VerificaAtuorizzazioni()
        If Session("TipoUtente") <> "U" Then
            Dim tipologia As String = Request.QueryString("tipologia")
            Response.Redirect("WfrmAlbero.aspx?tipologia=" & tipologia)
        End If
    End Sub
    Private Sub CancellaMessaggiInfo()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        VerificaAtuorizzazioni()
        If IsPostBack = False Then
            CaricaGriglia()
        End If
    End Sub
    Private Sub CaricaGriglia()
        myQuerySql &= " SELECT"
        myQuerySql &= " 	IdEnteFaseAntimafia,"
        myQuerySql &= " 	CASE Stato WHEN 1 THEN 'Aperta' WHEN 3 THEN 'Chiusa' END Stato,"
        myQuerySql &= " 	DataInizioFase,"
        myQuerySql &= " 	DataChiusuraFase,"
        myQuerySql &= " 	IdAllegatoComunicazioneAntimafia"
        myQuerySql &= " FROM "
        myQuerySql &= " 	EntiFasiAntimafia"
        myQuerySql &= " WHERE IdEnte = " & Session("IdEnte")

        myQuerySql &= "Order by DataFineFase Desc"
        Session("dtsCarica") = ClsServer.DataSetGenerico(myQuerySql, Session("conn"))
        dtgElencoDocumenti.DataSource = Session("dtsCarica")
        dtgElencoDocumenti.DataBind()
    End Sub

    Private Sub CaricaRuoli(idFase As Integer)
        myQuerySql &= " SELECT "
        myQuerySql &= " 	E.Denominazione,"
        myQuerySql &= " 	T.RuoloAntiMafia,"
        myQuerySql &= " 	R.CodiceFiscale,"
        myQuerySql &= " 	R.Nome + ' ' + R.Cognome Nome"
        myQuerySql &= " FROM "
        myQuerySql &= " 	RuoliAntimafia R JOIN"
        myQuerySql &= " 	ElencoRuoliAntimafia T ON T.Id=R.IdElencoRuoliAntimafia JOIN"
        myQuerySql &= " 	Enti E on E.IdEnte=R.IdEnte"
        myQuerySql &= " WHERE"
        myQuerySql &= " 	R.IdEnteFaseAntimafia =" & idFase
        myQuerySql &= " Order by E.Denominazione,R.Cognome, R.Nome"
        Session("dtsCaricaRuoli") = ClsServer.DataSetGenerico(myQuerySql, Session("conn"))
        dtgDichiarazioni.DataSource = Session("dtsCaricaRuoli")
        dtgDichiarazioni.DataBind()
        popUpDichiarazioni.Show()
    End Sub

    Private Sub dtgElencoDocumenti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgElencoDocumenti.ItemCommand
        Select Case e.CommandName
            Case "Download" 'Scarica la domanda
                ChiudiDataReader(myDataReader)
                myQuerySql = "SELECT FileName, BinData FROM Allegato WHERE IdAllegato = " + e.Item.Cells(4).Text
                myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
                If myDataReader.Read() Then
                    Response.Clear()
                    Response.ContentType = "Application/pdf"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" & myDataReader("FileName"))
                    Response.BinaryWrite(myDataReader("BinData"))
                    ChiudiDataReader(myDataReader)
                    Response.End()
                Else 'aperta/annullata o scaduta
                    msgErrore.Text = "Impossibile Scaricare la domanda"
                    Exit Sub
                End If

            Case "Dichiarazioni"
                CaricaRuoli(e.Item.Cells(0).Text)

        End Select
    End Sub





    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Dim tipologia As String = Request.QueryString("tipologia")
        Response.Redirect("WfrmAlbero.aspx?tipologia=" & tipologia)
    End Sub

    Private Sub dtgElencoDocumenti_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles dtgElencoDocumenti.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ImgDownload As ImageButton = DirectCast(e.Item.FindControl("ImgDownload"), ImageButton)

            Dim idAllegato As Integer = 0
            If Not IsDBNull(e.Item.DataItem("IdAllegatoComunicazioneAntimafia")) Then
                idAllegato = CInt(e.Item.DataItem("IdAllegatoComunicazioneAntimafia"))
            End If

            If (idAllegato > 0) Then
                    ImgDownload.Visible = True
                Else
                    ImgDownload.Visible = False
                End If


            End If

    End Sub
End Class