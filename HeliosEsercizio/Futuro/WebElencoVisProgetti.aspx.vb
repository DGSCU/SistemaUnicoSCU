Imports System.Data.SqlClient

Public Class WebElencoVisProgetti
    Inherits System.Web.UI.Page

    Dim strsql As String
    Dim dtsgenerico As DataSet
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
            If Not IsNothing(Request.QueryString("Vengoda")) Then
                lblErrore.Text = "Impossibile eliminare la Sede di Attuazione perchè sono presenti progetti Attivi o in Valutazione."
            Else
                lblErrore.Text = String.Empty
            End If
            If Request.QueryString("IdEnte") <> "" Then
                'vengo dall'albero dei vincoli
                esegui_RicercaProgettiEnte()
                AbilitaPulsanteForzaChiusuraEnte()
            Else
                'vengo dalla maschera WebAggiungiSedeProgetto.aspx
                esegui_RicercaProgetti()
            End If
        End If
    End Sub

    Private Sub esegui_RicercaProgetti()
        strsql = "select  b.denominazione, a.titolo, c.bando, g.macroambitoattività + ' / ' + " & _
        " f.ambitoattività as Ambito, convert(varchar,isnull(a.dataultimostato,''),3) as Data," & _
        " a.idattività, b.idente, Convert(tinyint, isnull(e.defaultstato, 0)) + Convert(tinyint, isnull(i.defaultstato, 0))" & _
        " modificabile,a.IdTipoProgetto, e.statoattività " & _
        " from attività a " & _
        " inner join attivitàEntiSediAttuazione on a.idattività=attivitàEntiSediAttuazione.idattività " & _
        " inner join enti b on a.identepresentante=b.idente " & _
        " left join bandiAttività h on a.idbandoattività=h.idbandoattività " & _
        " left join bando c on c.idbando=h.idbando  " & _
        " inner join statiattività e on a.idstatoattività=e.idstatoattività " & _
        " inner join ambitiattività f on f.idambitoattività=a.idambitoattività " & _
        " inner join macroambitiattività g on f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività " & _
        " left join statibandiattività i on h.idstatobandoattività=i.idstatobandoattività " & _
        " where(a.idattività Is Not null And b.idente = " & Session("idEnte") & " And attivitàEntiSediAttuazione.identesedeattuazione =" & Request.QueryString("IdSedeAttuazione") & ")" & _
        "and  (e.attiva=1 or e.daValutare=1 or e.DefaultStato=1 or e.daGraduare=1)" & _
        " order by year(a.dataultimostato)desc,month(a.dataultimostato)desc,day(a.dataultimostato)desc,1,2"
        dtsgenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dgRisultatoRicerca.DataSource = dtsgenerico
        dgRisultatoRicerca.DataBind()
    End Sub
    Private Sub esegui_RicercaProgettiEnte()
        strsql = " SELECT DISTINCT b.denominazione, a.titolo, c.bando, g.macroambitoattività + ' / ' + "
        strsql &= " f.ambitoattività as Ambito, convert(varchar,isnull(a.dataultimostato,''),3) as Data,"
        strsql &= " a.idattività, b.idente, Convert(tinyint, isnull(e.defaultstato, 0)) + Convert(tinyint, isnull(i.defaultstato, 0))"
        strsql &= " modificabile,a.IdTipoProgetto, e.statoattività, "
        strsql &= " year(a.dataultimostato) ,month(a.dataultimostato) ,day(a.dataultimostato) "
        strsql &= " FROM attività a "
        strsql &= " INNER JOIN attivitàEntiSediAttuazione on a.idattività=attivitàEntiSediAttuazione.idattività "
        strsql &= " INNER JOIN enti b on a.identepresentante=b.idente "
        strsql &= " LEFT JOIN bandiAttività h on a.idbandoattività=h.idbandoattività "
        strsql &= " LEFT JOIN bando c on c.idbando=h.idbando  "
        strsql &= " INNER JOIN statiattività e on a.idstatoattività=e.idstatoattività "
        strsql &= " INNER JOIN ambitiattività f on f.idambitoattività=a.idambitoattività "
        strsql &= " INNER JOIN macroambitiattività g on f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività "
        strsql &= " LEFT JOIN statibandiattività i on h.idstatobandoattività=i.idstatobandoattività "
        strsql &= " WHERE (a.idattività Is Not null AND b.idente = " & Session("idEnte") & " ) "
        strsql &= " AND  (e.attiva=1 or e.daValutare=1 or e.DefaultStato=1 or e.daGraduare=1) "
        strsql &= " UNION "
        strsql &= " SELECT DISTINCT "
        strsql &= " b.Denominazione, a.Titolo, c.Bando, g.MacroAmbitoAttività + ' / ' + f.AmbitoAttività AS Ambito, CONVERT(varchar, ISNULL(a.DataUltimoStato, ''), 3) "
        strsql &= " AS Data, a.IDAttività, b.IDEnte, CONVERT(tinyint, ISNULL(e.DefaultStato, 0)) + CONVERT(tinyint, ISNULL(i.DefaultStato, 0)) AS modificabile, "
        strsql &= " a.IdTipoProgetto, e.StatoAttività, YEAR(a.DataUltimoStato) AS Expr1, MONTH(a.DataUltimoStato) AS Expr2, DAY(a.DataUltimoStato) AS Expr3"
        strsql &= " FROM  attività AS a "
        strsql &= " INNER JOIN statiattività AS e ON a.IDStatoAttività = e.IDStatoAttività "
        strsql &= " INNER JOIN ambitiattività AS f ON f.IDAmbitoAttività = a.IDAmbitoAttività "
        strsql &= " INNER JOIN macroambitiattività AS g ON f.IDMacroAmbitoAttività = g.IDMacroAmbitoAttività "
        strsql &= " INNER JOIN AttivitàEntiCoprogettazione ON a.IDAttività = AttivitàEntiCoprogettazione.IdAttività "
        strsql &= " INNER JOIN enti AS b ON AttivitàEntiCoprogettazione.IdEnte = b.IDEnte "
        strsql &= " LEFT OUTER JOIN BandiAttività AS h ON a.IDBandoAttività = h.IdBandoAttività "
        strsql &= " LEFT OUTER JOIN bando AS c ON c.IDBando = h.IdBando "
        strsql &= " LEFT OUTER JOIN statiBandiAttività AS i ON h.IdStatoBandoAttività = i.IdStatoBandoAttività"
        strsql &= " WHERE (a.idattività Is Not null AND b.idente = " & Session("idEnte") & " ) "
        strsql &= " AND  (e.attiva=1 or e.daValutare=1 or e.DefaultStato=1 or e.daGraduare=1) "
        strsql &= " ORDER BY year(a.dataultimostato)desc,month(a.dataultimostato)desc,day(a.dataultimostato)desc,1,2"

        dtsgenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dgRisultatoRicerca.DataSource = dtsgenerico
        dgRisultatoRicerca.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        If Request.QueryString("IdEnte") <> "" Then
            esegui_RicercaProgettiEnte()
        Else
            esegui_RicercaProgetti()
        End If
    End Sub
    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dtsgenerico
        GridDaCaricare.DataBind()
    End Sub

    Private Sub imgForzaChiusuraEnte_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgForzaChiusuraEnte.Click
        If LeggiStoreChiusuraEnte(Session("IdEnte"), Session("Utente")) = 1 Then
            lblErrore.Text = "Errore durante la chiusura dell'Ente.Contattare l'assistenza."
        Else
            lblConferma.Text = "Chiusura effettuata con successo."
        End If
    End Sub

    Private Function LeggiStoreChiusuraEnte(ByVal IDEnte As Integer, ByVal Utente As String) As Integer
        Dim intValore As Integer

        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_ACCREDITAMENTO_CHIUSURA_ENTE"
        CustOrderHist.Connection = Session("conn")

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdEnte"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)


        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@Utente"
        sparam1.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@Esito"
        sparam2.SqlDbType = SqlDbType.Int
        sparam2.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam2)

        CustOrderHist.Parameters("@IdEnte").Value = IDEnte
        CustOrderHist.Parameters("@Utente").Value = Utente
        CustOrderHist.ExecuteScalar()
        intValore = CustOrderHist.Parameters("@Esito").Value

        Return intValore
    End Function

    Private Sub AbilitaPulsanteForzaChiusuraEnte()
        'modificato da simona cordella il 14/05/2010 
        'Se ho l'autorizzazione rendo visibile il pulsante per la Chiusura dell'ente.
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu" & _
                 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza InChiusura Ente'" & _
                 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            imgForzaChiusuraEnte.Visible = True
        Else
            imgForzaChiusuraEnte.Visible = False
        End If
        ChiudiDataReader(dtrgenerico)
    End Sub

    Protected Sub imgchiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgchiudi.Click
        If Request.QueryString("Torna") = "SedeProgetto" Then
            Response.Redirect("WebAggiungiSedeProgetto.aspx?EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&idTipoProg=" & Request.QueryString("idTipoProg") & "&idSede=" & Request.QueryString("idSede") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&idattES=" & Request.QueryString("idattES") & "&blnVisualizzaVolontari=" & Request.QueryString("blnVisualizzaVolontari") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale"))
        End If
        If Request.QueryString("Torna") = "Albero" Then
            Response.Redirect("WfrmAlbero.aspx?tipologia=Enti&Arrivo=WfrmMain.aspx&VediEnte=1")
        End If
    End Sub
End Class