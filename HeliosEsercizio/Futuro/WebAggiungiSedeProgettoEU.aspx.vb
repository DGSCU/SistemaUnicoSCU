Imports System.Data.SqlClient
Public Class WebAggiungiSedeProgettoEU
    Inherits System.Web.UI.Page
    Dim dataset As DataSet
    Dim dataReader As SqlClient.SqlDataReader
    Dim sqlCommand As SqlClient.SqlCommand
    Dim query As String
    Dim dtsGenerico As DataSet
    Dim rstGenerico As SqlClient.SqlCommand
    Dim strNull As String 'variabile stringa che contiene valore NULL
    Public strIdEnteSede As String


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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        ChiudiDataReader(dataReader)
        If Request.QueryString("IdAttivita") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1


            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) <> 1 Then
                ChiudiDataReader(dataReader)
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If


        End If

        If IsPostBack = False Then

            CaricaComboNazioni()
            'CaricaDataGrid()

        End If
    End Sub
    Sub CaricaComboNazioni()
        'creato da simona cordella il 17/04/2012
        Dim dsNaz As DataSet
        
        query = " SELECT '0' as ID ,'' as Denominazione " & _
                 " UNION  " & _
                 " SELECT  d.IDComune as ID, d.Denominazione as Denominazione from nazioni a  " & _
                 " inner join regioni b on a.IDNazione = b.IDNazione  " & _
                 " inner join provincie c on b.IDRegione = c.IDRegione " & _
                 " inner join comuni d on c.IDProvincia = d.IDProvincia " & _
                 " where a.NazioneUE = 1 and nazione <> 'Italia' and not d.CodiceIstat is null and a.Nazione = d.Denominazione " & _
                 " Order by Denominazione "
        dsNaz = ClsServer.DataSetGenerico(query, Session("conn"))
        ddlProvincia.DataSource = dsNaz
        ddlProvincia.DataTextField = "Denominazione"
        ddlProvincia.DataValueField = "ID"

        ddlProvincia.DataBind()
    End Sub

    Private Function VerificaValiditaCampi() As Boolean
        Dim utility As ClsUtility = New ClsUtility()
        Dim campiValidi As Boolean = True
        Dim numero As Int32
        Dim dataTmp As Date
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"
        Dim numeroNonValido As String = "Il valore di '{0}' non è valido. Inserire solo numeri.<br/>"
        Dim VerificaCampiDoppioni As String = "Il campo {0} è già esistente."

        If (txtdenominazione.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Denominazione Sede")
            campiValidi = False
        End If

        If (ddlProvincia.SelectedItem.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Nazione")
            campiValidi = False
        End If

        If (txtCity.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Città Estera")
            campiValidi = False
        End If
        If (txtIndirizzo.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo Completo")
            campiValidi = False
        End If
        'controllo denominazione città indirizzo
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
        Dim strdenominazione As String
        Dim strcitta As String
        Dim strindirizzo As String
        Dim strNazione As Integer

        If campiValidi = True Then


            query = "SELECT entisediattuazioni.IdEnteCapofila,entisediattuazioni.Denominazione, entisedi.Indirizzo,entisedi.CittaEstera,entisedi.idcomune,entisediattuazioni.NoAccreditamento " & _
            " FROM entisedi " & _
            " INNER JOIN entisediattuazioni on entisediattuazioni.IDEnteSede=entisedi.IDEnteSede " & _
            " WHERE entisediattuazioni.NoAccreditamento = 1 and IdEnteCapofila=" & Session("idente") & " and entisediattuazioni.denominazione = '" & txtdenominazione.Text.Replace("'", "''") & "'"
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()



            If dataReader.HasRows = True Then
                msgErrore.Text = msgErrore.Text + String.Format(VerificaCampiDoppioni, "Denominazione Sede")
                campiValidi = False

            End If

            If Not dataReader Is Nothing Then
                dataReader.Close()
                dataReader = Nothing
            End If

            query = "SELECT entisediattuazioni.IdEnteCapofila,entisediattuazioni.Denominazione, entisedi.Indirizzo,entisedi.CittaEstera,entisedi.idcomune,entisediattuazioni.NoAccreditamento " & _
            " FROM entisedi " & _
            " INNER JOIN entisediattuazioni on entisediattuazioni.IDEnteSede=entisedi.IDEnteSede " & _
            " WHERE entisediattuazioni.NoAccreditamento = 1 and IdEnteCapofila=" & Session("idente") & " and entisedi.idcomune = " & ddlProvincia.SelectedValue & " and entisedi.cittaEstera = '" & txtCity.Text.Replace("'", "''") & "' and entisedi.indirizzo = '" & txtIndirizzo.Text.Replace("'", "''") & "'"
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()

            If dataReader.HasRows = True Then
                msgErrore.Text = msgErrore.Text + String.Format(VerificaCampiDoppioni, "Indirizzo Completo")
                campiValidi = False


            End If

            If Not dataReader Is Nothing Then
                dataReader.Close()
                dataReader = Nothing
            End If


        End If
        Return campiValidi
    End Function

    Private Function ValidaInteri(ByVal valore As String, ByVal nomeCampo As String) As Boolean
        Dim numero As Int32
        Dim campiValidi As Boolean = True
        Dim messaggioNumeroNonValido As String = "Il valore di '{0}' non è valido. Inserire solo numeri.<br/>"

        If (Int32.TryParse(valore, numero) = False) Then
            msgErrore.Text = msgErrore.Text + String.Format(messaggioNumeroNonValido, nomeCampo)
            campiValidi = False
        End If
        Return campiValidi

    End Function

    Private Sub CancellaMessaggi()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
        dgRisultatoRicerca.DataBind()
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        CancellaMessaggi()
        Dim strIdEnteCapoFila As String
        If VerificaValiditaCampi() = False Then
            Exit Sub
        End If

        'Creazione Stringa SQL per l'inserimento della sede dell'Ente
        strNull = "null"
        query = "Insert Into entiSedi(idente,denominazione,Indirizzo,DettaglioRecapito,FlagIndirizzoValido, "
        query = query & "civico,idcomune,cap,PrefissoTelefono,telefono,PrefissoFax,Fax, Palazzina, Scala, Piano, Interno,IdTitoloGiuridico,AltroTitoloGiuridico, http,"
        query = query & "dataControllohttp,httpvalido,email,datacontrolloEmail,EmailValido, "
        query = query & "idStatoenteSede,UsernameStato,datacreazionerecord,RiferimentoRimborsi,CittaEstera) values ( "
        'controllo se si tratta di ente figlioo ente padre

        query = query & Session("idente") & ""
        query = query & ",'" & Replace(txtdenominazione.Text, "'", "''") & "'"
        query = query & ",'" & Replace(txtIndirizzo.Text, "'", "''") & "',null,0,'-'"
        query = query & "," & ddlProvincia.SelectedValue & ",'-','-','-',null,null,null,null,0,null,6,null,null,null,0,null,null,0,3,'" & Replace(Session("Utente"), "'", "''") & "',getdate(),0,'" & Replace(txtCity.Text, "'", "''") & "'"
        query = query & ")"
        'trovo stato dell'ente Padre e lo inserisco

        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(query, Session("Conn"))
        'inserisco relazione EntisedeTipi - entisedi
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
        'If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = False Then
        '    dataReader = ClsServer.CreaDatareader("select identesede from entisedi where idente=" & lblidEnte.Value & " and denominazione='" & Trim(Replace(txtdenominazione.Text, "'", "''")) & "'", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'Else
        '    dataReader = ClsServer.CreaDatareader("select identesede from entisedi where idente=" & ddlEntiFigli.SelectedValue & " and denominazione='" & Trim(Replace(txtdenominazione.Text, "'", "''")) & "'", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'End If
        dataReader = ClsServer.CreaDatareader("select @@identity as identesede ", Session("Conn"))
        dataReader.Read()
        strIdEnteSede = dataReader("identesede")

        query = "insert into entiseditipi (identesede,idtiposede) values " & _
            " (" & strIdEnteSede & "," & 4 & ")"
        'se si tratta di un ente figlio assegno ad una variabile pubblica l'id della sede appena inserita
        'cos' che lo passo successivamente in fase di inclusione delle sedi
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(query, Session("Conn"))
        msgConferma.Text = "Inserimento SEDE eseguito con successo."
        msgConferma.Text = msgConferma.Text & "<br/>"
        'PulisciMaschera()
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If



        query = "SELECT identepadre FROM entirelazioni WHERE DataFinevalidità is null AND identefiglio='" & Session("IdEnte") & "'"

        dataReader = ClsServer.CreaDatareader(query, Session("Conn"))
        If dataReader.HasRows = True Then
            dataReader.Read()
            strIdEnteCapoFila = dataReader("identepadre")
        Else
            strIdEnteCapoFila = Session("IdEnte")
        End If

        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If


        query = "insert into entiSediattuazioni (Denominazione,identeSede,UsernameStato,idStatoEnteSede,UsernameInseritore,DataInserimento,IdEnteCapoFila,Certificazione,DataCertificazione ,UserCertificazione,NMaxVolontari,FlagMaxVolontari, UsernameMaxVolontari,NoAccreditamento,Note) " & _
                            "select '" & Replace(txtdenominazione.Text, "'", "''") & "'," & strIdEnteSede & ","
        query = query & "'" & Replace(Session("Utente"), "'", "''") & "', entisedi.idstatoEntesede, '" & Session("Utente") & "', getDate(), '" & strIdEnteCapoFila & "',"
        query = query & " 0, null , null, "
        query = query & " 999, "
        query = query & " 0, '" & Replace(Session("Utente"), "'", "''") & "',1 "
        query = query & " ,'" & txtNote.Text.Replace("'", "''") & "'"
        query = query & "from StatiEntiSedi " & _
        " inner join entisedi  on StatiEntiSedi.idstatoentesede=entisedi.idstatoentesede where identesede=" & strIdEnteSede & ""
        ChiudiDataReader(dataReader)
        rstGenerico = ClsServer.EseguiSqlClient(query, Session("Conn"))
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
        CaricaDataGrid(strIdEnteSede)

    End Sub
    Sub CaricaDataGrid(ByRef strIdEnteSede As Integer)
        Dim strsql As String
        strsql = "SELECT entisediattuazioni.IDEnteSedeAttuazione, entisediattuazioni.Denominazione, StatiEntiSedi.StatoEnteSede, entisediattuazioni.Note,entisediattuazioni.Certificazione " & _
        " FROM entisediattuazioni " & _
        " INNER Join entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede " & _
        " Left Join AssociaEntiRelazioniSediAttuazioni ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione " & _
        " INNER Join  enti ON entisedi.IDEnte = enti.IDEnte " & _
        " INNER Join StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede" & _
        " where IdEnteCapofila=" & Session("idente") & " and entisediattuazioni.noaccreditamento=1 and entisediattuazioni.IDEnteSede=" & strIdEnteSede & ""
        Session("dtsGenericoSedi") = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgRisultatoRicerca.DataSource = Session("dtsGenericoSedi")
        dgRisultatoRicerca.DataBind()


        lblidsedeattuazione.Value = dgRisultatoRicerca.Items(0).Cells(1).Text
        lblidentesede.Value = strIdEnteSede
    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'Response.Redirect("WebGestioneSediProgettoEU.aspx?IdAttivita=" & Request.QueryString("IdAttivita") & "&Nazionale=" & Request.QueryString("Nazionale") & "&VengoDa=" & Request.QueryString("VengoDa") & "&Modifica=" & Request.QueryString("Modifica") & "&EnteCapoFila=" & Request.QueryString("EnteCapoFila"))
        Response.Redirect("WebGestioneSediProgetto.aspx?EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&idTipoProg=" & Request.QueryString("idTipoProg") & "&idSede=" & lblidentesede.Value & "&IdSedeAttuazione=" & lblidsedeattuazione.Value & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&idattES=" & Request.QueryString("idattES") & "&blnVisualizzaVolontari=" & Request.QueryString("blnVisualizzaVolontari") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub

End Class