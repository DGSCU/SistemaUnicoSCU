Public Class cronologiadocumenti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        Response.ExpiresAbsolute = #1/1/1980#
        Response.AddHeader("Pragma", "no-cache")
        If Page.IsPostBack = False Then
            Session("LocalDataSet") = Nothing
            If Request.QueryString("VengoDa") = "progetti" Then
                caricagrigliaProgetti(CInt(Session("IdEnte")))
            Else
                caricagriglia(CInt(Session("IdEnte")))
            End If

        End If
    End Sub

    Sub caricagrigliaProgetti(ByVal intIdente As Integer)
        Dim strSql As String
        Dim Mydataset As New DataSet
        strSql = "select UserName, "
        strSql = strSql & "DataDocumento, "
        strSql = strSql & "case Documento "
        strSql = strSql & "when 'LetteraNegativaPositiva' then 'Lettera Negativa Positiva' "
        strSql = strSql & "when 'LetteraNegativa' then 'Lettera Negativa' "
        strSql = strSql & "when 'DeterminaNegativaSingola' then 'Determina Negativa Singola' "
        strSql = strSql & "when 'DeterminaNegativaMultipla' then 'Determina Negativa Multipla' "
        strSql = strSql & "when 'elencoProgettiNeg' then 'Elenco Progetti Negativi' "
        strSql = strSql & "when 'LetteraPositiva' then 'Lettera Positiva' "
        strSql = strSql & "when 'letteraditrasmissione1' then 'Lettera Trasmissione' "
        strSql = strSql & "when 'comunicazionepositiva1' then 'Comunicazione Positiva'"
        strSql = strSql & "when 'limitataplurima1' then 'Limitata Plurima'"
        strSql = strSql & "when 'limitatasingola1' then 'Limitata Singola'"
        strSql = strSql & "when 'negativaplurima' then 'Negativa Plurima'"
        strSql = strSql & "when 'negativasingola1' then 'Negativa Singola'"
        strSql = strSql & "when 'allegatonegativi1' then 'Allegato Negativi'"
        strSql = strSql & "when 'allegatopositivi1' then 'Allegato Positivi'"
        strSql = strSql & "when 'allegatopositivilimitati1' then 'Allegato Positivi Limitati'"
        strSql = strSql & "when 'DeterminaPositivaSingola' then 'Determina Positiva Singola' "
        strSql = strSql & "when 'DeterminaPositivaMultipla' then 'Determina Positiva Multipla' "
        strSql = strSql & "when 'DeterminaNegEnteNonAccr' then 'Determina Negativa Ente Non Accreditato' "
        strSql = strSql & "when 'elencoprogettiPos' then 'Elenco Progetti Positivi' "
        strSql = strSql & "end as Documento "
        strSql = strSql & "from CronologiaEntiDocumenti "
        strSql = strSql & "where idente = " & Session("IdEnte") & " and TipoDocumento = 1 order by DataDocumento asc"

        Mydataset = ClsServer.DataSetGenerico(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dtgRisultatoRicerca.DataSource = Mydataset
        dtgRisultatoRicerca.DataBind()

        Session("LocalDataSet") = Mydataset
    End Sub

    Sub caricagriglia(ByVal intIdente As Integer)
        Dim strSql As String
        Dim Mydataset As New DataSet

        'Generato da Bagnani Jonathan il 17/05/05
        strSql = "select UserName, "
        strSql = strSql & "DataDocumento, "
        strSql = strSql & "case Documento "
        strSql = strSql & "when 'art10accreditamento' then 'art.10 Iscrizione' "
        strSql = strSql & "when 'attribuzionecodice' then 'Attribuzione Codice' "
        strSql = strSql & "when 'detnegaccrtreanni' then 'Determina negativa tre anni' "
        strSql = strSql & "when 'detnegaccraseguitorispostaente' then 'Determina negativa a seguito risposta Ente' "
        strSql = strSql & "when 'detnegaccrsenzarispostaente' then 'Determina negativa senza risposta Ente' "
        strSql = strSql & "when 'letteraavvioprocedimento' then 'Comunicazione Avvio Procedimento Iscrizione' "
        strSql = strSql & "when 'LetteraCompleDocu' then 'Lettera Completamento Documentazione Iscrizione' "
        strSql = strSql & "when 'letteraavvioprocedimentoAdeg' then 'Comunicazione Avvio Procedimento Adeguamento' "
        strSql = strSql & "when 'LetteraCompleDocuAdeg' then 'Lettera Completamento Documentazione Adeguamento' "
        strSql = strSql & "when 'letteratrasmissionedeterminanegativa' then 'Lettera trasmissione determina negativa' "
        strSql = strSql & "when 'letteraadegpositivoenegativo' then 'Lettera Adeg. Positivo/Negativo' "
        strSql = strSql & "when 'DeterminaAdeguamentoPositivo' then 'Determina Adeg. Positivo' "
        strSql = strSql & "when 'DeterminaAdeguamentoPositivoconLimiti' then 'Determina Adeg.Positivo limitazioni' "
        strSql = strSql & "when 'DeterminaAdeguamentoNegativo' then 'Determina Adeguamento Negativo' "
        strSql = strSql & "when 'allegatobdeterminazione' then 'Allegato B' "
        strSql = strSql & "when 'allegatoadeterminazione' then 'Allegato A' "
        strSql = strSql & "when 'letteraaccreditamentopositivo' then 'Lettera Iscrizione Positivo'"
        strSql = strSql & "when 'letteraaccreditamentonegativo' then 'Lettera Iscrizione Negativo'"
        strSql = strSql & "when 'DeterminaAccreditamentoPositivo' then 'Determina Iscrizione Positivo'"
        strSql = strSql & "when 'DeterminaAccreditamentoPositivoconLimiti' then 'Determina Iscrizione Positivo con Limiti'"
        strSql = strSql & "when 'DeterminaAccreditamentoNegativo' then 'Determina Iscrizione Negativo'"
        strSql = strSql & "when 'letteraRevisionepositivaenegativa' then 'Lettera Revisione Positiva e Negativa'"
        strSql = strSql & "when 'art10Revisione' then 'art.10 Revisione'"
        strSql = strSql & "when 'art10Adeguamento' then 'art.10 Adeguamento'"
        strSql = strSql & "when 'DeterminaRevisionePositivaconLimiti' then 'Determina Revisione Positiva con Limiti'"
        strSql = strSql & "when 'DeterminaRiesameNegativa' then 'Determina Riesame Negativa'"
        strSql = strSql & "when 'DeterminaRiesamePositiva' then 'Determina Riesame Positiva'"
        strSql = strSql & "when 'DeterminaNegEnteNonAccr' then 'Determina Negativa Ente Non Accreditato' "
        strSql = strSql & "when 'allegatob' then 'Allegato B' "
        strSql = strSql & "when 'allegatoa1' then 'Allegato A1' "
        strSql = strSql & "when 'allegatoa2' then 'Allegato A2' "
        strSql = strSql & "when 'allegatobAdeguamento' then 'Allegato B Adeguamento' "
        strSql = strSql & "when 'allegatoA1Adeguamento' then 'Allegato A1 Adeguamento' "
        strSql = strSql & "when 'allegatoA2Adeguamento' then 'Allegato A2 Adeguamento' "
        strSql = strSql & "when 'allegatobRevisione' then 'Allegato B Revisione'  "
        strSql = strSql & "when 'allegatoA1Revisione' then 'Allegato A1 Revisione'  "
        strSql = strSql & "when 'allegatoA2Revisione' then 'Allegato A2 Revisione'  "
        'DeterminaAccreditamentoPositivoArt10
        strSql = strSql & "when 'DeterminaAccreditamentoPositivoArt10' then 'Determina Iscrizione Positivo Art10'  "
        'determinaaccreditamentopositivoconlimiti
        strSql = strSql & "when 'determinaaccreditamentopositivoconlimiti' then 'Determina Iscrizione Positivo con Limiti'  "
        'determinaaccreditamentonegativo
        strSql = strSql & "when 'determinaaccreditamentonegativo' then 'Determina Iscrizione Negativo'  "
        'DeterminaAdeguamentoPositivoArt10
        strSql = strSql & "when 'DeterminaAdeguamentoPositivoArt10' then 'Determina Adeguamento Positivo Art10'  "
        'DeterminaAdeguamentoPositivoconLimiti
        strSql = strSql & "when 'DeterminaAdeguamentoPositivoconLimiti' then 'Determina Adeguamento Positivo con Limiti'  "
        'DeterminaAdeguamentoNegativo
        strSql = strSql & "when 'DeterminaAdeguamentoNegativo' then 'Determina Adeguamento Negativo'  "
        'DeterminaRiesamePositivaart10
        strSql = strSql & "when 'DeterminaRiesamePositivaart10' then 'Determina Riesame Positiva art10'  "
        'determinarevisionePositivaconlimiti
        strSql = strSql & "when 'determinarevisionePositivaconlimiti' then 'Determina Revisione Positiva con Limiti'  "
        'determinariesamenegativa
        strSql = strSql & "when 'determinariesamenegativa' then 'Determina Riesame Negativa'  "
        'Tutti gli altri casi
        strSql = strSql & " Else Documento "
        strSql = strSql & "end as Documento "
        strSql = strSql & "from CronologiaEntiDocumenti "
        strSql = strSql & "where idente = " & Session("IdEnte") & " and TipoDocumento = 0 order by DataDocumento asc"

        Mydataset = ClsServer.DataSetGenerico(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dtgRisultatoRicerca.DataSource = Mydataset
        dtgRisultatoRicerca.DataBind()

        Session("LocalDataSet") = Mydataset
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        'Generato da Bagnani Jonathan il 26/10/04
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dtgRisultatoRicerca.DataSource = Session("LocalDataSet")
        dtgRisultatoRicerca.DataBind()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()" & vbCrLf)
        Response.Write("</script>")
    End Sub

End Class