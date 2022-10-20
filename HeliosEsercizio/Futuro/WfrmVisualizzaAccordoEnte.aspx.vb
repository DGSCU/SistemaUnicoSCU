Public Class WfrmVisualizzaAccordoEnte
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            'generato da Alessandra Taballione il 16.02.2005
            '
            strsql = "select Padre.denominazione as entepadre,isnull(Padre.CodiceRegione,'') as codpadre," & _
            " isnull(figlio.Codiceregione,'') as codFiglio,figlio.denominazione as entefiglio " & _
            " ,tipirelazioni.tiporelazione, isnull(entirelazioni.datainiziovalidità,'')as datainiziovalidità,datafinevalidità,datastipula,datascadenza " & _
            " from entirelazioni " & _
            " inner join enti Padre on Padre.idente=entirelazioni.identepadre  " & _
            " inner join enti figlio on figlio.idente=entirelazioni.identefiglio " & _
            " inner join TipiRelazioni on TipiRelazioni.idTipoRelazione=EntiRelazioni.idtiporelazione " & _
            " where identeFiglio=" & Session("IdEnte") & " and datafineValidità is null "
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                lblEntePadre.Text = dtrgenerico("EntePadre")
                lblCodEntePadre.Text = dtrgenerico("CodPadre")
                lblEnteFiglio.Text = dtrgenerico("EnteFiglio")
                lblCodEnteFiglio.Text = dtrgenerico("CodFiglio")
                lblTipoAccordo.Text = dtrgenerico("tipoRelazione")
                lbldataInizio.Text = IIf(Not IsDBNull(dtrgenerico("datainiziovalidità")), FormatDateTime(dtrgenerico("datainiziovalidità"), DateFormat.ShortDate), "")
                'datafine Validità
                If Not IsDBNull(dtrgenerico("datafinevalidità")) Then
                    lblfineAc.Text = FormatDateTime(dtrgenerico("datafinevalidità"), DateFormat.ShortDate)
                Else
                    lblfineAc.Text = ""
                End If
                'data STipula
                If Not IsDBNull(dtrgenerico("datastipula")) Then
                    lblStipula.Text = FormatDateTime(dtrgenerico("datastipula"), DateFormat.ShortDate)
                Else
                    lblStipula.Text = ""
                End If
                'data Scadenza
                If Not IsDBNull(dtrgenerico("datascadenza")) Then
                    lblScadenza.Text = FormatDateTime(dtrgenerico("datascadenza"), DateFormat.ShortDate)
                Else
                    lblScadenza.Text = ""
                End If
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>")
    End Sub

End Class