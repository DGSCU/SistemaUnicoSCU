Public Class ricercaentepersonaleacquisibile
    Inherits System.Web.UI.Page
    Dim strSql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CaricaDG()
    End Sub

    Private Sub CaricaDG()
        strSql = "SELECT AssociaSistemaRuolo.IdRuolo, entepersonale.Cognome + ' ' + entepersonale.Nome AS Nominativo, ruoli.Ruolo, entepersonale.Posizione, " &
                 "case entepersonaleruoli.Accreditato when 1 then 'Iscritto' when 0 then 'Da Valutare' when -1 then 'Chiuso' else 'Non Definito' end as Accreditato, " &
                 "case when EntePersonale.datafinevalidità is null then 'Attiva'	when not EntePersonale.datafinevalidità is null then 'Cancellata' end as Stato " &
                 "FROM ruoli INNER JOIN sistemi INNER JOIN AssociaSistemaRuolo ON sistemi.IDSistema = AssociaSistemaRuolo.IdSistema INNER JOIN " &
                 "entepersonaleruoli INNER JOIN entepersonale ON entepersonaleruoli.IDEntePersonale = entepersonale.IDEntePersonale ON " &
                 "AssociaSistemaRuolo.IdRuolo = entepersonaleruoli.IDRuolo ON ruoli.IDRuolo = entepersonaleruoli.IDRuolo INNER JOIN " &
                 "enti ON entepersonale.IDEnte = enti.IDEnte WHERE Sistemi.Nascosto=0 and entepersonaleruoli.datafinevalidità is null and enti.CodiceRegione ='" & Request.QueryString("CodEnte") & "' and sistemi.idsistema = '" & Request.QueryString("IdSist") & "' " &
                 "ORDER BY ruolo,nominativo"

        dtgPersAcquisibile.DataSource = ClsServer.DataSetGenerico(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtgPersAcquisibile.DataBind()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()" & vbCrLf)
        Response.Write("</script>")
    End Sub

    Private Sub dtgPersAcquisibile_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPersAcquisibile.PageIndexChanged
        dtgPersAcquisibile.CurrentPageIndex = e.NewPageIndex
        CaricaDG()
        dtgPersAcquisibile.SelectedIndex = -1
    End Sub

End Class