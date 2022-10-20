Imports System.Data.SqlClient
Imports System.IO
Public Class WfrmVisualizzaCorsiOLP
    Inherits System.Web.UI.Page
    Dim dtsgenerico As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim vengoda As Integer
    Dim strsql As String
    Public strEsito As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Page.IsPostBack = False Then
            If Session("txtCodEnte") <> Nothing Then
                txtCodiceEnte.Text = Session("txtCodEnte")
            End If
            If Session("TipoUtente") = "E" Then
                riganonente.Visible = False
                dgRisultatoRicerca.Columns(8).Visible = False
                dgRisultatoRicerca.Columns(9).Visible = False
                divDataValutazione.Visible = False
            Else
                riganonente.Visible = True
            End If
            '1 VENGO DALLA PAGINA DI DETTAGLIO
            If Request.QueryString("VengoDa") = 1 Then
                vengoda = 1
                RicercaCorsoOLP()
            End If
        End If
       
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        'lblRicerca.Value = ""
        dgRisultatoRicerca.CurrentPageIndex = 0
        'lblpage.Value = ""
        RicercaCorsoOLP()
    End Sub
    Private Sub RicercaCorsoOLP()
       
        If vengoda = 1 Then
            strsql = "select CorsiFormazioneOLP.IdCorsoFormazioneOLP,case convert(varchar,StatoRichiesta) when '1' then 'Registrata' when '2' then 'Approvata' when '3' then 'Respinta' end as StatoRichiesta,UsernameRichiesta,DataRichiesta,UsernameValutazione,DataValutazione,b.IdEnte,b.Denominazione + ' (' + isnull(b.CodiceRegione,'') + ') ' as Ente, b.Denominazione, b.codiceregione, c.descrizione as Competenza  from CorsiFormazioneOLP  "
            strsql = strsql & " inner join enti b on CorsiFormazioneOLP.idente = b.idente "
            strsql = strsql & " inner join RegioniCompetenze c on c.IdRegioneCompetenza = b.IdRegioneCompetenza "
            If Session("TipoUtente") = "E" Then
                strsql = strsql & " Where b.idente=" & Session("IdEnte") & " and IdCorsoFormazioneOLP=" & Request.QueryString("Id")
            Else
                strsql = strsql & " Where 1=1 and IdCorsoFormazioneOLP=" & Request.QueryString("Id")
            End If
            vengoda = 0
        Else

            strsql = "select CorsiFormazioneOLP.IdCorsoFormazioneOLP,case convert(varchar,StatoRichiesta) when '1' then 'Registrata' when '2' then 'Approvata' when '3' then 'Respinta' end as StatoRichiesta,UsernameRichiesta,DataRichiesta,UsernameValutazione,DataValutazione,b.IdEnte,b.Denominazione + ' (' + isnull(b.CodiceRegione,'') + ') ' as Ente,b.Denominazione, b.codiceregione, c.descrizione as Competenza  from CorsiFormazioneOLP  "
            strsql = strsql & " inner join enti b on CorsiFormazioneOLP.idente = b.idente "
            strsql = strsql & " inner join RegioniCompetenze c on c.IdRegioneCompetenza = b.IdRegioneCompetenza "
            If Session("TipoUtente") = "E" Then
                strsql = strsql & " Where b.idente=" & Session("IdEnte")
            Else
                strsql = strsql & " Where 1=1 "
            End If
        End If




        If Trim(txtCodiceEnte.Text) <> "" Then
            strsql = strsql & " and b.CodiceRegione = '" & txtCodiceEnte.Text & "'"
        End If
        If Trim(txtCodiceEnte.Text) <> "" Then
            strsql = strsql & " and b.Denominazione like '%" & Replace(txtNomeEnte.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodCORSO.Text) <> "" Then
            strsql = strsql & " and IdCorsoFormazioneOLP = '" & txtCodCORSO.Text & "'"
        End If
        If Trim(ddlStatoRichiesta.SelectedValue.ToString) <> 0 Then
            strsql = strsql & " and StatoRichiesta = '" & ddlStatoRichiesta.SelectedValue.ToString & "'"
        End If

        If Trim(txtdatadal1.Text) <> "" And txtdataal1.Text = "" Then
            strsql = strsql & " and dataRichiesta >= '" & txtdatadal1.Text & "'"
        End If
        If Trim(txtdatadal1.Text) <> "" And Trim(txtdataal1.Text) <> "" Then
            strsql = strsql & " and DataRichiesta BETWEEN '" & txtdatadal1.Text & "' and  '" & txtdataal1.Text & " 23:59:59'"
        End If
        If Trim(txtdatadal1.Text) = "" And txtdataal1.Text <> "" Then
            strsql = strsql & " and dataRichiesta <= '" & txtdataal1.Text & " 23:59:59'"
        End If

        If Trim(txtdatadal.Text) <> "" And txtdataal.Text = "" Then
            strsql = strsql & " and DataValutazione >= '" & txtdatadal.Text & "'"
        End If
        If Trim(txtdatadal.Text) <> "" And Trim(txtdataal.Text) <> "" Then
            strsql = strsql & " and DataValutazione BETWEEN '" & txtdatadal.Text & "' and '" & txtdataal.Text & " 23:59:59'"
        End If
        If Trim(txtdatadal.Text) = "" And txtdataal.Text <> "" Then
            strsql = strsql & " and DataValutazione <= '" & txtdataal.Text & " 23:59:59'"
        End If


        strsql = strsql & "Order by  1 desc "

        'Eval("pippo").ToString().Equals()=
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))

        CaricaDataGrid(dgRisultatoRicerca)
    End Sub
    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dtsgenerico
        GridDaCaricare.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "Selezionato" Then

            Session("IdEnte") = e.Item.Cells(1).Text
            Session("Denominazione") = e.Item.Cells(11).Text
            Session("Competenza") = e.Item.Cells(12).Text
            Session("CodiceRegioneEnte") = "[" & e.Item.Cells(13).Text & "]"
            Session("txtCodEnte") = e.Item.Cells(13).Text
            Response.Redirect("~/WfrmVisualizzaDettaglioCorsoOLP.aspx?IdCorso=" & e.Item.Cells(0).Text)
        End If

        If e.CommandName = "SelezionatoPdf" Then
            ''CHIAMATA AL PDF
            'Dim sDati As String
            'Dim sEsito As String
            'sDati = "idCorso," & e.Item.Cells(0).Text & ":"

            'strEsito = ClsServer.CreatePdf("crpOLPAttestato.rpt", sDati, Me.Session)
            'If ClsServer.GetPdfError = "" Then
            '    Response.Redirect(strEsito)
            'Else
            '    strEsito = ClsServer.GetPdfError
            'End If
            Dim idcorso As String
            idcorso = e.Item.Cells(0).Text
            

            Response.Write("<script>")
            Response.Write("myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=70&IdCorso=" & idcorso & "','Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')")
            Response.Write("</script>")
        End If

    End Sub
    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        RicercaCorsoOLP()
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class