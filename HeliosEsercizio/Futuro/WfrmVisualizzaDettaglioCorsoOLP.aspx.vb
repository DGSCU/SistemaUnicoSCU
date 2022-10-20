Imports System.Data.SqlClient
Imports System.IO
Imports Ionic.Zip
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Public Class WfrmVisualizzaDettaglioCorsoOLP
    Inherits System.Web.UI.Page
    Dim dtsgenerico As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim vengoda As Integer
    Public strEsito As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
       

        If SICUREZZA_VERIFICA_AUTORIZZAZIONI() = True Then

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        Else
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Response.Redirect("wfrmAnomaliaDati.aspx")

        End If
        If Page.IsPostBack = False Then
            If Request.QueryString("VengoDa") = 2 Then
                vengoda = 2
                If Session("TipoUtente") = "E" Then
                    dgRisultatoRicerca.Columns(2).Visible = False
                End If
                CaricaMaschera()
                Exit Sub
            End If
            CaricaMaschera()
            CaricaGriglia()
            If Session("TipoUtente") = "E" Then
                dgRisultatoRicerca.Columns(2).Visible = False
            End If
            If Request.QueryString("pat") <> "" Then
                HyperLink1.Visible = True
                HyperLink1.Text = "Download File Zip Corso OLP"
                HyperLink1.NavigateUrl = Request.QueryString("pat")
            End If
        End If
    End Sub
    Private Sub CaricaMaschera()

        If vengoda = 2 Then
            strsql = "select DISTINCT CorsiFormazioneOLP.IdCorsoFormazioneOLP,case convert(varchar,StatoRichiesta) when '1' then 'Registrata' when '2' then 'Approvata' when '3' then 'Respinta' end as StatoRichiesta,UsernameRichiesta,DataRichiesta,UsernameValutazione,DataValutazione,b.IdEnte,b.CodiceRegione,b.Denominazione  from CorsiFormazioneOLP  "
            strsql = strsql & " inner join CorsiFormazioneOLPDettaglio a on CorsiFormazioneOLP.IdCorsoFormazioneOLP = a.IdCorsoFormazioneOLP "
            strsql = strsql & " inner join enti b on CorsiFormazioneOLP.idente = b.idente "
            If Session("TipoUtente") = "E" Then
                strsql = strsql & "  Where CorsiFormazioneOLP.IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso") & " Order by  1 desc "
            Else
                strsql = strsql & "  Where CorsiFormazioneOLP.IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso") & " Order by  1 desc "
            End If

        Else
            strsql = "select IdCorsoFormazioneOLP,case convert(varchar,StatoRichiesta) when '1' then 'Registrata' when '2' then 'Approvata' when '3' then 'Respinta' end as StatoRichiesta,UsernameRichiesta,DataRichiesta,UsernameValutazione,DataValutazione,b.IdEnte,b.CodiceRegione,b.Denominazione  from CorsiFormazioneOLP  "
            strsql = strsql & " inner join enti b on CorsiFormazioneOLP.idente = b.idente "
            If Session("TipoUtente") = "E" Then
                strsql = strsql & "  Where IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso") & " Order by  1 desc "
            Else
                strsql = strsql & "  Where IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso") & " Order by  1 desc "
            End If
        End If

        ' dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))




        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))


        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            lblStatoCorso.Text = dtrgenerico.Item("StatoRichiesta").ToString.ToUpper
            LblEnteDato.Text = dtrgenerico.Item("CodiceRegione").ToString
            LblTitoloDato.Text = dtrgenerico.Item("Denominazione").ToString
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        LblCorsoRiferimento.Text = Request.QueryString("IdCorso")
        If Session("TipoUtente") = "E" Then
            If lblStatoCorso.Text = "REGISTRATA" Then
                CmdStampa.Visible = False
                CmdAccetta.Visible = False
                CmdRespingi.Visible = False
            End If
            If lblStatoCorso.Text = "APPROVATA" Then
                CmdStampa.Visible = True
                CmdAccetta.Visible = False
                CmdRespingi.Visible = False
            End If
            If lblStatoCorso.Text = "RESPINTA" Then
                CmdStampa.Visible = False
                CmdAccetta.Visible = False
                CmdRespingi.Visible = False
            End If
        Else
            If lblStatoCorso.Text = "REGISTRATA" Then
                CmdStampa.Visible = False
                CmdAccetta.Visible = True
                CmdRespingi.Visible = True
            End If
            If lblStatoCorso.Text = "APPROVATA" Then
                CmdStampa.Visible = True
                CmdAccetta.Visible = False
                CmdRespingi.Visible = True
            End If
            If lblStatoCorso.Text = "RESPINTA" Then
                CmdStampa.Visible = False
                CmdAccetta.Visible = True
                CmdRespingi.Visible = False
            End If

        End If
        '2 VENGO DALLA PAGINA DI MODIFICA
        If vengoda = 2 Then
            CaricaGriglia()
        End If


    End Sub
    Private Sub CaricaGriglia()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'strsql = "select IdCorsoFormazioneOLPDettaglio,Cognome,Nome,EnteRiferimento,LuogoSvolgimento,DataSvolgimentoCorso,NumeroOre,case convert(varchar,StatoRichiesta) when '1' then 'Registrata' when '2' then 'Approvata' when '3' then 'Respinta' end as StatoRichiesta,UsernameRichiesta,DataRichiesta,UsernameValutazione,DataValutazione,b.IdEnte,b.CodiceRegione,a.IdCorsoFormazioneOLP,b.Denominazione  from CorsiFormazioneOLPDettaglio  "
        'strsql = strsql & " inner join CorsiFormazioneOLP a on a.IdCorsoFormazioneOLP = CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP "
        'strsql = strsql & " inner join enti b on a.idente = b.idente Where CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso") & " Order by  cognome desc"
        If vengoda = 2 Then
            strsql = "select CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLPDettaglio,CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP,Cognome,Nome,EnteRiferimento,LuogoSvolgimento,DataSvolgimentoCorso,NumeroOre,case convert(varchar,CorsiFormazioneOLP.StatoRichiesta) when '1' then 'Registrata' when '2' then 'Approvata' when '3' then 'Respinta' end as StatoRichiesta  from CorsiFormazioneOLPDettaglio "
            strsql = strsql & " inner join CorsiFormazioneOLP on CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP=CorsiFormazioneOLP.IdCorsoFormazioneOLP Where CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso") & " Order by  1 desc "
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
            dgRisultatoRicerca.DataSource = dtsgenerico
            dgRisultatoRicerca.DataBind()
            vengoda = 0

        Else
            strsql = "select CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLPDettaglio,CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP,Cognome,Nome,EnteRiferimento,LuogoSvolgimento,DataSvolgimentoCorso,NumeroOre,case convert(varchar,CorsiFormazioneOLP.StatoRichiesta) when '1' then 'Registrata' when '2' then 'Approvata' when '3' then 'Respinta' end as StatoRichiesta  from CorsiFormazioneOLPDettaglio "
            strsql = strsql & " inner join CorsiFormazioneOLP on CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP=CorsiFormazioneOLP.IdCorsoFormazioneOLP Where CorsiFormazioneOLPDettaglio.IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso") & " Order by  1 desc "
            'strsql = "select *  from CorsiFormazioneOLPDettaglio  Where IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso") & " Order by  1 desc "
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
            dgRisultatoRicerca.DataSource = dtsgenerico
            dgRisultatoRicerca.DataBind()
        End If


    End Sub

    Protected Sub CmdAccetta_Click(sender As Object, e As EventArgs) Handles CmdAccetta.Click
        strsql = "update CorsiFormazioneOLP set StatoRichiesta='" & 2 & "', UsernameValutazione='" & Session("Utente") & "',  DataValutazione=getdate()  where IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso")

        'sql command che mi esegue la insert
        myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()
        CaricaMaschera()
        CaricaGriglia()
        lblmessaggio.Text = "CORSO APPROVATO CON SUCCESSO"
    End Sub

    Protected Sub CmdRespingi_Click(sender As Object, e As EventArgs) Handles CmdRespingi.Click
        strsql = "update CorsiFormazioneOLP set StatoRichiesta='" & 3 & "', UsernameValutazione='" & Session("Utente") & "',  DataValutazione=getdate()  where IdCorsoFormazioneOLP=" & Request.QueryString("IdCorso")

        'sql command che mi esegue la insert
        myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()
        CaricaMaschera()
        CaricaGriglia()
        lblmessaggio.Text = "CORSO RESPINTO CON SUCCESSO"
    End Sub

    Protected Sub CmdStampa_Click(sender As Object, e As EventArgs) Handles CmdStampa.Click
       

        'Antonello
        Dim sDati As String
        Dim percorso As String
        Dim idcorso As String
        idcorso = Request.Params("IdCorso")
        sDati = "IdCorsoDett," & Request.Params("IdCorso") & ":"
        percorso = ClsServer.CreatePdfZip("crpOLPAttestatoDett.rpt", sDati, Me.Session, Session("conn"), idcorso)

      
        HyperLink1.Visible = True
        HyperLink1.NavigateUrl = percorso
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmVisualizzaCorsiOLP.aspx?id=" & LblCorsoRiferimento.Text & "&VengoDa=" & 1)
    End Sub
    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "DettCorso" Then
            Response.Redirect("~/WfrmModificaDettCorsoOLP.aspx?IdCorso=" & e.Item.Cells(1).Text & "&idDettaglio=" & e.Item.Cells(0).Text)
        End If
        If e.CommandName = "SelezionatoPdf" Then
            'CHIAMATA AL PDF
            'Dim sDati As String
            'Dim sEsito As String
            'sDati = "idCorsoDett," & e.Item.Cells(0).Text & ":"

            'strEsito = ClsServer.CreatePdf("crpOLPAttestatoDett.rpt", sDati, Me.Session)



            'If ClsServer.GetPdfError = "" Then
            '    Response.Redirect(strEsito)
            'Else
            '    strEsito = ClsServer.GetPdfError
            'End If


            Dim idcorso As String
            idcorso = e.Item.Cells(0).Text
           
            Response.Write("<script>")
            Response.Write("myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=71&IdCorso=" & idcorso & "','Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')")
            Response.Write("</script>")
        End If

    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaGriglia()
        dgRisultatoRicerca.SelectedIndex = -1
    End Sub

    Private Function SICUREZZA_VERIFICA_AUTORIZZAZIONI() As Boolean
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "select IdCorsoFormazioneOlp from CorsiFormazioneOLP where IdCorsoFormazioneOlp = " & Request.QueryString("IdCorso") & " and idente= " & Session("IdEnte")
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then

            SICUREZZA_VERIFICA_AUTORIZZAZIONI = True
        Else

            SICUREZZA_VERIFICA_AUTORIZZAZIONI = False
        End If



        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function
End Class