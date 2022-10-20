Imports System.Data.SqlClient

Public Class WebElencoEliminaSedeProgetto
    Inherits System.Web.UI.Page
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strsql As String
    Dim dtsGenerico As DataSet
    Dim cmdGenerico As SqlClient.SqlCommand
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

        lblmessaggioInfo.Text = "Per eliminare la sede è necessario effettuare la cancellazione del Personale associato."
       VerificaSessione()
        If IsPostBack = False Then
            'mod. il 02/0/2006 da simona cordella
            'ricordo l'IdAttivitàEntiSedeAttuazione per le query
            lblidsedeattuazione.Value = Request.QueryString("idattES")
            RicercaOLP()
        End If
    End Sub
    Private Sub RicercaOLP()
        'Generata da Alessandra Taballione il 29.09.2004
        'Effettua la ricerca di tutto il personale Associato alla sede di attuazione
        'Modificato il 02/10/2006 da Simona Cordella
        'In maschera vengono visualizzati gli OLP , RLEA e i TUTOR
        strsql = "Select '<IMG width=25 height=20 src=""images/xp1.gif"">' as img,entepersonaleruoli.Accreditato ,entepersonaleruoli.identePersonaleRuolo," & _
             " case isnull(attivitàentisediattuazione.identesedeattuazione,-1)when -1 then '0'else attivitàentisediattuazione.identesedeattuazione end as identesedeattuazione," & _
             " case isnull(a.idassociaEntepersonaleRuoliattivitàentisediattuazione,-1)when -1 then 0 else a.idassociaEntepersonaleRuoliattivitàentisediattuazione end as idass," & _
             " tipiruoli.tiporuolo, entepersonale.identepersonale,entepersonale.nome + ' ' + entepersonale.cognome as nominativo," & _
             " entepersonale.email,entepersonale.telefono,entepersonale.datanascita," & _
             " Comuni.denominazione, entepersonale.idente,entepersonale.cognome,entepersonale.nome,ruoli.descrabb" & _
             " from entepersonale " & _
             " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)" & _
             " inner join associaEntepersonaleRuoliattivitàentisediattuazione a " & _
             " on  (a.idEntepersonaleRuolo=entepersonaleruoli.identepersonaleRuolo)" & _
             " inner join attivitàentisediattuazione on " & _
             " (attivitàentisediattuazione.idAttivitàEntesedeAttuazione=a.idAttivitàEntesedeAttuazione)  " & _
             " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)" & _
             " inner join tipiruoli on (tipiruoli.idTipoRuolo=ruoli.idtiporuolo)" & _
             " inner join Comuni on (Comuni.idComune=entepersonale.idComuneNascita) " & _
             " where tipiruoli.tiporuolo IN ('Attività','Ente','Sede')" & _
             " and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0)" & _
             " and attivitàentisediattuazione.idattivitàentesedeattuazione=" & lblidsedeattuazione.Value & " "
        '" and attivitàentisediattuazione.identesedeattuazione=" & lblidsedeattuazione.Text & " "
        strsql = strsql & " order by entepersonale.Cognome,entepersonale.nome"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        Session("SessionDtOlp") = dtsGenerico
        CaricaDataGrid(dgRisultatoRicerca)
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()
    End Sub
    Private Sub cmdConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConferma.Click
        'mod. il 02/10/2006 da simona cordella
        lblConferma.Text = String.Empty
        lblmessaggioInfo.Text = String.Empty
        lblmessaggio.Text = String.Empty

        Try
            Dim _errore As String
            EliminaOLPDaSede(Integer.Parse(lblidsedeattuazione.Value))

            If Not String.IsNullOrEmpty(_errore) Then
                lblConferma.Text = _errore
            Else
                ChiudiERitornaSuChiamante()
            End If

        Catch ex As Exception
            lblConferma.Text = "Errore nell'eliminazione"
        End Try

        'strsql = "delete from associaentePersonaleruoliattivitàEntisediAttuazione where idassociaEntePersonaleRuoliAttivitàEntiSediAttuazione in (" & _
        '    "Select a.idassociaEntepersonaleRuoliattivitàentisediattuazione" & _
        '     " from entepersonale " & _
        '     " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)" & _
        '     " inner join associaEntepersonaleRuoliattivitàentisediattuazione a " & _
        '     " on  (a.idEntepersonaleRuolo=entepersonaleruoli.identepersonaleRuolo)" & _
        '     " inner join attivitàentisediattuazione on " & _
        '     " (attivitàentisediattuazione.idAttivitàEntesedeAttuazione=a.idAttivitàEntesedeAttuazione)  " & _
        '     " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)" & _
        '     " inner join tipiruoli on (tipiruoli.idTipoRuolo=ruoli.idtiporuolo)" & _
        '     " inner join Comuni on (Comuni.idComune=entepersonale.idComuneNascita) " & _
        '     " where tipiruoli.tiporuolo IN ('Attività','Ente','Sede')" & _
        '     " and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0)" & _
        '     " and attivitàentisediattuazione.idattivitàentesedeattuazione=" & lblidsedeattuazione.Value & ") "

        'cmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        'lblConferma.Text = "Operazione avvenuta con successo."
        'ChiudiERitornaSuChiamante()
    End Sub


    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        ChiudiERitornaSuChiamante()
    End Sub

    Protected Sub ChiudiERitornaSuChiamante()
        Response.Redirect("WebGestioneSediProgetto.aspx?EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&idTipoProg=" & Request.QueryString("idTipoProg") & "&idSede=" & Request.QueryString("idSede") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&idattES=" & Request.QueryString("idattES") & "&blnVisualizzaVolontari=" & Request.QueryString("blnVisualizzaVolontari") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale"))
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("SessionDtOlp")
        dgRisultatoRicerca.DataBind()
    End Sub

    Function EliminaOLPDaSede(IdAttivitàEnteSedeAttuazione As Integer)
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_OLP_ELIMINA_DA_SEDE]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = IdAttivitàEnteSedeAttuazione
        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, -1)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()

        Return sqlCMD.Parameters("@Errore").Value
    End Function

End Class