Public Class ver_PersonaleEnte
    Inherits System.Web.UI.Page
 
    Dim strSql As String
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents imgStampaE As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Imagebutton1 As System.Web.UI.WebControls.ImageButton
    Dim dtsGenericoVol As DataSet
    Protected WithEvents LblEntePro As System.Web.UI.WebControls.Label
    Protected WithEvents LblDescEntePro As System.Web.UI.WebControls.Label
    Protected WithEvents LblProgetto As System.Web.UI.WebControls.Label
    Protected WithEvents LblDescProgetto As System.Web.UI.WebControls.Label
    Protected WithEvents LblEnteFiglio As System.Web.UI.WebControls.Label
    Protected WithEvents LblDescEnteFiglio As System.Web.UI.WebControls.Label

    Protected WithEvents dtgVolontariEnte As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgPersonaleEnte As System.Web.UI.WebControls.DataGrid

    Dim dtsGenericoPers As DataSet
    Protected WithEvents LblIndirizzo As System.Web.UI.WebControls.Label
    Protected WithEvents LblDescIndirizzo As System.Web.UI.WebControls.Label
    Protected WithEvents LblDescCap As System.Web.UI.WebControls.Label
    Protected WithEvents LblComune As System.Web.UI.WebControls.Label
    Protected WithEvents LblDescComune As System.Web.UI.WebControls.Label
    Protected WithEvents LblRegione As System.Web.UI.WebControls.Label
    Protected WithEvents LblDescRegione As System.Web.UI.WebControls.Label
    Protected WithEvents LblCapE As System.Web.UI.WebControls.Label
    Dim dtrGenerico As SqlClient.SqlDataReader
        'Dim DtbRicerca1 As DataTable

#Region " Codice generato da Progettazione Web Form "

        'Chiamata richiesta da Progettazione Web Form.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
        'Non spostarla o rimuoverla.
        Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
            'Non modificarla nell'editor del codice.
            InitializeComponent()
        End Sub

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Inserire qui il codice utente necessario per inizializzare la pagina
            CaricaIntestazione()
            CaricaVolontari()
            CaricaPersonale()
        End Sub
        Private Sub CaricaIntestazione()

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            strSql = "Select DISTINCT CodiceEnte + ' - ' + denominazione as EnteProponente,CodiceProgetto + ' - ' + Titolo  as Progetto ,EnteFiglio" & _
                     " from ver_vw_ricercasedi " & _
                    " where idente =" & Request.QueryString("Idente") & "  and idattività =" & Request.QueryString("Idattivita") & ""
            dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                LblDescEntePro.Text = dtrGenerico("EnteProponente")
                LblDescProgetto.Text = dtrGenerico("Progetto")
                LblDescEnteFiglio.Text = dtrGenerico("EnteFiglio")
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            'ricavo informazioni sedi
            strSql = " SELECT entisedi.Indirizzo + ' ' + entisedi.Civico AS indirizzo, entisedi.CAP, comuni.Denominazione  + ' ('+ provincie.DescrAbb + ')' as Comune, regioni.Regione " & _
                      " FROM provincie  " & _
                      " INNER JOIN comuni ON provincie.IDProvincia = comuni.IDProvincia  " & _
                      " INNER JOIN   regioni ON provincie.IDRegione = regioni.IDRegione  " & _
                      " INNER JOIN   entisedi ON comuni.IDComune = entisedi.IDComune  " & _
                      " INNER JOIN   entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                      " WHERE entisediattuazioni.IDEnteSedeAttuazione = " & Request.QueryString("IDEnteSedeAttuazione") & ""
            dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                LblDescIndirizzo.Text = dtrGenerico("indirizzo")
                LblDescCap.Text = dtrGenerico("CAP")
                LblDescComune.Text = dtrGenerico("Comune")
                LblDescRegione.Text = dtrGenerico("Regione")
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        End Sub
        Private Sub CaricaVolontari()
            dtgVolontariEnte.CurrentPageIndex = 0
            strSql = "SELECT distinct entità.CodiceVolontario, entità.Cognome + '  ' + entità.Nome AS Volontario, entità.CodiceFiscale,  " & _
                     " dbo.FormatoData(entità.DataInizioServizio) as DataInizioServizio, dbo.FormatoData(entità.DataFineServizio) as DataFineServizio, entità.OreFormazione " & _
                     "FROM entità " & _
                     "INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                     "INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                     "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                     "WHERE attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione =" & Request.QueryString("IDAESA") & " and idstatoentità =3"
            Session("dtsGenericoVol") = ClsServer.DataSetGenerico(strSql, Session("conn"))
            ' dtgVolontariEnte.DataSource = Session("dtsGenericoVol")
            'dtgVolontariEnte.DataBind()
            CaricaDataGridVolontario(dtgVolontariEnte)
        End Sub
        Private Sub CaricaPersonale()
            dtgPersonaleEnte.CurrentPageIndex = 0
            strSql = "SELECT entepersonale.Cognome + ' ' + entepersonale.Nome AS Nominativo, entepersonale.CodiceFiscale, ruoli.DescrAbb " & _
                     "FROM attività " & _
                     "INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività " & _
                     "INNER JOIN AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione ON  " & _
                     " attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione  " & _
                     "INNER JOIN entepersonaleruoli ON AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdEntePersonaleRuolo = entepersonaleruoli.IDEntePersonaleRuolo  " & _
                     "INNER JOIN entepersonale ON entepersonaleruoli.IDEntePersonale = entepersonale.IDEntePersonale " & _
                     "INNER JOIN ruoli ON entepersonaleruoli.IDRuolo = ruoli.IDRuolo " & _
                     "WHERE attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = " & Request.QueryString("IDAESA") & ""
            Session("dtsGenericoPers") = ClsServer.DataSetGenerico(strSql, Session("conn"))
            CaricaDataGridEnte(dtgPersonaleEnte)
            ' dtgPersonaleEnte.DataSource = Session("dtsGenericoPers")
            ' dtgPersonaleEnte.DataBind()
        End Sub

        Private Sub CaricaDataGridVolontario(ByRef GriddaCaricare As DataGrid)
            'assegno il dataset alla griglia del risultato
            GriddaCaricare.DataSource = Session("dtsGenericoVol")
            GriddaCaricare.DataBind()
            'blocco per la creazione della datatable per la stampa della ricerca

            If GriddaCaricare.Items.Count = 0 Then
            ' imgStampaV.Visible = False
            Else
            '  imgStampaV.Visible = True
            End If

            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(5) As String
            Dim NomiCampiColonne(5) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Cod. Volontario"
            NomeColonne(1) = "Volontario"
            NomeColonne(2) = "Codice Fiscale"
            NomeColonne(3) = "Data Inizio Servizio"
            NomeColonne(4) = "Data Fine Servizio"
            NomeColonne(5) = "Ore Formazione"

            NomiCampiColonne(0) = "CodiceVolontario"
            NomiCampiColonne(1) = "volontario"
            NomiCampiColonne(2) = "CodiceFiscale"
            NomiCampiColonne(3) = "DataInizioServizio"
            NomiCampiColonne(4) = "DataFineServizio"
            NomiCampiColonne(5) = "OreFormazione"

            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(Session("dtsGenericoVol"), 5, NomeColonne, NomiCampiColonne)

            '*********************************************************************************
            GriddaCaricare.Visible = True

        End Sub
        Private Sub CaricaDataGridEnte(ByRef GriddaCaricare As DataGrid)
            'assegno il dataset alla griglia del risultato
            GriddaCaricare.DataSource = Session("dtsGenericoPers")
            GriddaCaricare.DataBind()
            'blocco per la creazione della datatable per la stampa della ricerca

            If GriddaCaricare.Items.Count = 0 Then
            'imgStampaP.Visible = False
            Else
            'imgStampaP.Visible = True
            End If

            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(2) As String
            Dim NomiCampiColonne(2) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Nominativo"
            NomeColonne(1) = "Codice Fiscale"
            NomeColonne(2) = "Ruolo"

            NomiCampiColonne(0) = "Nominativo"
            NomiCampiColonne(1) = "CodiceFiscale"
            NomiCampiColonne(2) = "descrabb"

            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            Session("DtbRicerca1") = ClsServer.CaricaDataTablePerStampa(Session("dtsGenericoPers"), 2, NomeColonne, NomiCampiColonne)

            '*********************************************************************************
            GriddaCaricare.Visible = True
        End Sub

        Private Sub dtgVolontariEnte_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgVolontariEnte.PageIndexChanged
            dtgVolontariEnte.SelectedIndex = -1
            dtgVolontariEnte.EditItemIndex = -1
            dtgVolontariEnte.CurrentPageIndex = e.NewPageIndex
            CaricaDataGridVolontario(dtgVolontariEnte)
        End Sub

        Private Sub dtgPersonaleEnte_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgPersonaleEnte.PageIndexChanged
            dtgPersonaleEnte.SelectedIndex = -1
            dtgPersonaleEnte.EditItemIndex = -1
            dtgPersonaleEnte.CurrentPageIndex = e.NewPageIndex
            CaricaDataGridEnte(dtgPersonaleEnte)
        End Sub

End Class

