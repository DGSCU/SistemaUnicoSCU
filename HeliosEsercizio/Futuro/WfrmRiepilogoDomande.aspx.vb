Imports System.Data.SqlClient

Public Class WfrmRiepilogoDomande
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
        myQuerySql = " Select  "
        myQuerySql &= " case f.TipoFase "
        myQuerySql &= " when 1 then 'Iscrizione' when 2 then 'Adeguamento' END as TipoFase,"
        myQuerySql &= " f.IdEnteFase,	case f.stato  When 1 then case when  GETDATE() between f.DataInizioFase and f.DataFineFase then 'Aperta' ELSE 'Scaduta' end  when 2 then 'Annullata' when 3 then  'Presentata'	when 4  then 'Valutata' end as  Stato, "
        myQuerySql &= " f.DataInizioFase , f.DataFineFase, "
        myQuerySql &= " CASE f.Privacy WHEN 1 THEN 'Sì' WHEN 0 THEN 'No' END Privacy, "
        myQuerySql &= " CASE f.DichiarazioneImpegno WHEN 1 THEN 'Sì' WHEN 0 THEN 'No' END DichiarazioneImpegno, "
        myQuerySql &= " (SELECT COUNT(*) FROM EntiDocumenti WHERE IdTipoAllegato IN (13,14) And IdEnteFase = f.IdEnteFase ) NDomande, "
        myQuerySql &= " (SELECT COUNT(*) FROM EntiFasi EF JOIN EntiFasi_Enti EFE ON EF.IdEnteFase = EFE.IdEnteFase JOIN Enti E ON EFE.IdEnte = E.IdEnte WHERE EF.IDEnteFase = f.IdEnteFase ) NDocumenti "
        myQuerySql &= " From EntiFasi f"
        myQuerySql &= " join Enti e on e.IDEnte = f.IdEnte"
        myQuerySql &= " Where f.IdEnte = " & Session("IdEnte") & " and f.TipoFase IN (1,2)"

        myQuerySql &= "Order by f.DataFineFase Desc"
        Session("dtsCarica") = ClsServer.DataSetGenerico(myQuerySql, Session("conn"))
        dtgElencoDocumenti.DataSource = Session("dtsCarica")
        dtgElencoDocumenti.DataBind()
    End Sub

    Private Sub CaricaDichiarazioni(idFase As Integer)
        myQuerySql &= " SELECT"
        myQuerySql &= " 	E.Denominazione,  "
        myQuerySql &= " 	E.CodiceRegione,  "
        myQuerySql &= " 	D.IdEnteDocumento,  "
        myQuerySql &= " 	D.IdEnteFase"
        myQuerySql &= " FROM"
        myQuerySql &= " 	EntiFasi F JOIN"
        myQuerySql &= " 	EntiFasi_Enti EF ON EF.IdEnteFase = F.IdEnteFase JOIN"
        myQuerySql &= " 	Enti E ON EF.IdEnte = E.IdEnte JOIN"
        myQuerySql &= " 	EntiDocumenti D ON D.IdEnteDocumento = E.IdAllegatoImpegno"
        myQuerySql &= " WHERE"
        myQuerySql &= " 	F.IdEnteFase =" & idFase
        myQuerySql &= " Order by e.Denominazione"
        Session("dtsCaricaDichiarazioni") = ClsServer.DataSetGenerico(myQuerySql, Session("conn"))
        dtgDichiarazioni.DataSource = Session("dtsCaricaDichiarazioni")
        dtgDichiarazioni.DataBind()
        popUpDichiarazioni.Show()
    End Sub

    Private Sub dtgElencoDocumento_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgElencoDocumenti.ItemCommand
        Select Case e.CommandName
            Case "Domanda" 'Scarica la domanda
                ChiudiDataReader(myDataReader)
                myQuerySql = "SELECT FileName, BinData FROM EntiDocumenti WHERE IdTipoAllegato IN (13,14) and IdEnteFase = " + e.Item.Cells(1).Text
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
                CaricaDichiarazioni(e.Item.Cells(1).Text)

        End Select
    End Sub




    Private Sub dtgDichiarazioni_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles dtgDichiarazioni.ItemCommand
        Select Case e.CommandName
            Case "Download" 'Scarica la domanda
                ChiudiDataReader(myDataReader)
                myQuerySql = "SELECT FileName, BinData FROM EntiDocumenti WHERE IdEnteDocumento = " + e.Item.Cells(0).Text
                myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
                If myDataReader.Read() Then
                    Response.Clear()
                    Response.ContentType = "Application/pdf"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" & myDataReader("FileName"))
                    Response.BinaryWrite(myDataReader("BinData"))
                    ChiudiDataReader(myDataReader)
                    Response.End()
                Else 'aperta/annullata o scaduta
                    ChiudiDataReader(myDataReader)
                    msgErrore.Text = "Impossibile Scaricare la domanda"
                    Exit Sub
                End If


        End Select
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Dim tipologia As String = Request.QueryString("tipologia")
        Response.Redirect("WfrmAlbero.aspx?tipologia=" & tipologia)
    End Sub

    Private Sub dtgElencoDocumenti_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles dtgElencoDocumenti.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ImgDownload As ImageButton = DirectCast(e.Item.FindControl("ImgDownload"), ImageButton)
            Dim ImgDettaglio As ImageButton = DirectCast(e.Item.FindControl("ImgDettaglio"), ImageButton)

            Dim nDocumenti As Integer = IIf(IsDBNull(e.Item.DataItem("NDocumenti")), 0, CInt(e.Item.DataItem("NDocumenti")))
            Dim nDomande As Integer = IIf(IsDBNull(e.Item.DataItem("NDomande")), 0, CInt(e.Item.DataItem("NDomande")))

            If (nDomande > 0 And ImgDownload IsNot Nothing) Then
                ImgDownload.Visible = True
            Else
                ImgDownload.Visible = False
            End If

            If (nDocumenti > 0 And ImgDettaglio IsNot Nothing) Then
                ImgDettaglio.Visible = True
            Else
                ImgDettaglio.Visible = False
            End If


        End If

    End Sub
End Class