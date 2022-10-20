Imports System.Data.SqlClient

Public Class WfrmElencoProfili
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim CmdGenerico As SqlClient.SqlCommand
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
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If IsPostBack = False Then
            ElencoProfili()
            LblDescrTipoProgetto.Text = (Request.QueryString("strTipoProgetto"))
            SelezionaProfiliTipiProgetto(Request.QueryString("strIdTipoProgetto"))
            If Request.QueryString("Stato") = "Aperto" Or Request.QueryString("Stato") = "Annullato" Then
                chkSelDesel.Visible = False
                cmdSalva.Visible = False
                bloccaCheck()
            End If
        End If
    End Sub

    Private Sub ElencoProfili()
        Dim dtsgenerico As DataSet
        Dim item As DataGridItem

        strsql = "SELECT DESCRIZIONE,TIPO,IDPROFILO  FROM PROFILI WHERE ABILITATO=1 ORDER BY TIPO,DESCRIZIONE"
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgElencoProfili.DataSource = dtsgenerico
        Session("RisultatoElencoProfili") = dtsgenerico
        dgElencoProfili.DataBind()
    End Sub

    Private Sub SelezionaProfiliTipiProgetto(ByVal strIdTipoProgetto As String)
        Dim item As DataGridItem

        strsql = " SELECT A.idprofilo  " & _
                 " FROM AssociaProfiliTipiProgetto A" & _
                 " INNER JOIN tipiprogetto t ON T.IDTIPOPROGETTO= A.IDTIPOPROGETTO " & _
                 " WHERE A.IDTIPOPROGETTO = '" & strIdTipoProgetto & "'"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        While dtrgenerico.Read()
            For Each item In dgElencoProfili.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                If dtrgenerico("idprofilo") = dgElencoProfili.Items(item.ItemIndex).Cells(3).Text Then
                    check.Checked = True
                End If
            Next
        End While
        ChiudiDataReader(dtrgenerico)
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        ClosePage()
    End Sub

    Private Sub dgElencoProfili_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dgElencoProfili.CurrentPageIndex = e.NewPageIndex
        dgElencoProfili.DataSource = Session("RisultatoElencoProfili")
        dgElencoProfili.DataBind()
        SelezionaProfiliTipiProgetto(Request.QueryString("strIdTipoProgetto"))
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        CancellaProfili(Request.QueryString("strIdTipoProgetto"))
        InserimentoProfili(Request.QueryString("strIdTipoProgetto"))
        ClosePage()
    End Sub

    Private Sub CancellaProfili(ByVal IdProgetto As Integer)
        ' Generato da Simona Cordella il 07/04/2006
        'Elimino tutti i profili legati ad un certo tipo progetto
        CmdGenerico = New System.Data.SqlClient.SqlCommand
        CmdGenerico.Connection = Session("conn")

        strsql = "Delete from AssociaProfiliTipiProgetto where idtipoProgetto=" & IdProgetto & ""
        CmdGenerico.CommandText = strsql
        CmdGenerico.ExecuteNonQuery()
    End Sub

    Private Sub InserimentoProfili(ByVal IdProgetto As Integer)
        Dim item2 As DataGridItem
        ' Generato da Simona Cordella il 07/04/2006
        'Inserimento profili legati ad un certo tipo progetto
        CmdGenerico = New System.Data.SqlClient.SqlCommand
        CmdGenerico.Connection = Session("conn")
        For Each item2 In dgElencoProfili.Items
            Dim check As CheckBox = DirectCast(item2.FindControl("check1"), CheckBox)
            If check.Checked = True Then
                strsql = "Insert into AssociaProfiliTipiProgetto (IdTipoProgetto,IdProfilo) " & _
                        " Values ( " & IdProgetto & "," & dgElencoProfili.Items(item2.ItemIndex).Cells(3).Text & ")"
                CmdGenerico.CommandText = strsql
                CmdGenerico.ExecuteNonQuery()
            End If
        Next
    End Sub

    Private Sub ControllaCheck()
        'Generato da Simona CordelLa il 30/03/2006
        Dim item As DataGridItem
        If Hdd_chkFlag.Value = "" Then
            For Each item In dgElencoProfili.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                check.Checked = True
                Hdd_chkFlag.Value = True
            Next
        Else
            For Each item In dgElencoProfili.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                check.Checked = False
                Hdd_chkFlag.Value = ""
            Next
        End If
    End Sub



    Private Sub bloccaCheck()
        Dim item As DataGridItem
        For Each item In dgElencoProfili.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            check.Enabled = False
        Next
    End Sub
    Sub ClosePage()
        'modifcato il 31/05/2006 da simona cordella
        'gestione del nuovo campo descrizione abbreviata bando
        If Request.QueryString("TipoAzione") = "Inserimento" Then
            Response.Redirect("WfrmGestioneBandi.aspx?Hdd_rif=" & (Request.QueryString("Hdd_rif")) & " &Hdd_DataIVol=" & (Request.QueryString("Hdd_DataIVol")) & "&Hdd_DataFVol=" & (Request.QueryString("Hdd_DataFVol")) & "&Hdd_Check=" & (Request.QueryString("Hdd_Check")) & "&TipoAzione=" & (Request.QueryString("TipoAzione")) & "&Vengoda=" & (Request.QueryString("Vengoda")) & "&strGruppo=" & (Request.QueryString("strGruppo")) & "&strCheck=" & (Request.QueryString("strCheck")) & "&strDataFineVol= " & Request.QueryString("strDataFineVol") & "&strDataInizioVol=" & Request.QueryString("strDataInizioVol") & "&strDataFine= " & Request.QueryString("strDataFine") & "&strDataInizio=" & Request.QueryString("strDataInizio") & "&strImporto=" & Request.QueryString("strImporto") & "&strAnnoRif=" & Request.QueryString("strAnnoRif") & "&strRiferimento=" & Request.QueryString("strRiferimento") & "&strBandoBreve=" & Request.QueryString("strBandoBreve") & "&strBando=" & Request.QueryString("strBando") & "&strDataScadGrad=" & Request.QueryString("strDataScadGrad") & "&strNMaxVolontariProgettoItalia=" & Request.QueryString("strNMaxVolontariProgettoItalia") & "&strNMinVolontariProgettoItalia=" & Request.QueryString("strNMinVolontariProgettoItalia") & "&strNMaxVolontariProgettoEstero=" & Request.QueryString("strNMaxVolontariProgettoEstero") & "&strNMinVolontariProgettoEstero=" & Request.QueryString("strNMinVolontariProgettoEstero") & "&strNMinVolontariSedeItalia=" & Request.QueryString("strNMinVolontariSedeItalia") & "&strNMinVolontariSedeEstero=" & Request.QueryString("strNMinVolontariSedeEstero") & " ")
        Else
            Response.Redirect("WfrmGestioneBandi.aspx?Stato=" & (Request.QueryString("Stato")) & "&Pagina=" & (Request.QueryString("Pagina")) & " &strIdBando=" & (Request.QueryString("strIdBando")) & "&Hdd_rif=" & (Request.QueryString("Hdd_rif")) & " &Hdd_DataIVol=" & (Request.QueryString("Hdd_DataIVol")) & "&Hdd_DataFVol=" & (Request.QueryString("Hdd_DataFVol")) & "&Hdd_Check=" & (Request.QueryString("Hdd_Check")) & "&TipoAzione=" & (Request.QueryString("TipoAzione")) & "&Vengoda=" & (Request.QueryString("Vengoda")) & "&strGruppo=" & (Request.QueryString("strGruppo")) & "&strCheck=" & (Request.QueryString("strCheck")) & "&strDataFineVol= " & Request.QueryString("strDataFineVol") & "&strDataInizioVol=" & Request.QueryString("strDataInizioVol") & "&strDataFine= " & Request.QueryString("strDataFine") & "&strDataInizio=" & Request.QueryString("strDataInizio") & "&strImporto=" & Request.QueryString("strImporto") & "&strAnnoRif=" & Request.QueryString("strAnnoRif") & "&strRiferimento=" & Request.QueryString("strRiferimento") & "&strBandoBreve=" & Request.QueryString("strBandoBreve") & "&strBando=" & Request.QueryString("strBando") & "&strDataScadGrad=" & Request.QueryString("strDataScadGrad") & "&strNMaxVolontariProgettoItalia=" & Request.QueryString("strNMaxVolontariProgettoItalia") & "&strNMinVolontariProgettoItalia=" & Request.QueryString("strNMinVolontariProgettoItalia") & "&strNMaxVolontariProgettoEstero=" & Request.QueryString("strNMaxVolontariProgettoEstero") & "&strNMinVolontariProgettoEstero=" & Request.QueryString("strNMinVolontariProgettoEstero") & "&strNMinVolontariSedeItalia=" & Request.QueryString("strNMinVolontariSedeItalia") & "&strNMinVolontariSedeEstero=" & Request.QueryString("strNMinVolontariSedeEstero") & " ")
        End If
    End Sub

    Private Sub cmdSeleziona_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles chkSelDesel.CheckedChanged
        Dim chkSelVol As CheckBox
        If chkSelDesel.Checked = True Then
            For i = 0 To dgElencoProfili.Items.Count - 1
                chkSelVol = dgElencoProfili.Items(i).FindControl("check1")
                chkSelVol.Checked = True
            Next
            chkSelDesel.Text = "Deseleziona tutto"
        Else
            For i = 0 To dgElencoProfili.Items.Count - 1
                chkSelVol = dgElencoProfili.Items(i).FindControl("check1")
                chkSelVol.Checked = False
            Next
            chkSelDesel.Text = "Seleziona tutto"
        End If
    End Sub

End Class