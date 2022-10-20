Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Drawing
Imports System.Collections.Generic
Imports Futuro.RiepilogoAccreditamento

Public Class WfrmAlbero
	Inherits SmartPage
	'

	Dim dtrGen As SqlClient.SqlDataReader
	Dim myCmd As SqlClient.SqlCommand
	Dim strVerificaAccredita As String 'salvo classe per controllo accreditamento
	Dim myDataReader As SqlClient.SqlDataReader
	Dim idRuolo As Int32
	Dim myQuerySql As String
	Dim bytePassaBordo As Byte 'passo variabile per valore di bytbordo per query inizio relazione vincoli
	Dim idEntePersonale As Int32
	Dim tipologia As String
	Dim idEnte As String

	Const NODE_VALUE_FOREST_GREEN As String = "TreeviewBorderGreen"
	Const NODE_VALUE_NO_COLOR As String = "TreeviewNoBorder"
	Const NODE_VALUE_GREEN As String = "TreeviewColorGreen"
	Const NODE_VALUE_RED As String = "TreeviewColorRed"
	Const VALORE_TRUE As String = "True"
	Const URL_IMMAGINE_KO_PRESENTAZIONE As String = "~\images\vistacattiva_small.png"
	Const URL_IMMAGINE_OK_PRESENTAZIONE As String = "~\images\vistabuona_small.png"
	Const URL_IMMAGINE_VINCOLO_GIALLO As String = "~\images\VincoloGiallo.jpg"
	Const URL_IMMAGINE_VINCOLO_VERDE As String = "~\images\VincoloVerde.jpg"
	Const URL_IMMAGINE_VINCOLO_ROSSO As String = "~\images\VincoloRosso.jpg"
	Const URL_IMMAGINE_CLASSE As String = "~\images\cartella_small.png"
	Const ENTE_ABILITATO As String = "Ente abilitato all'inserimento dati."
	Const ENTE_DISABILITATO As String = "Ente disabilitato all'inserimento dati."

	Const TIPOLOGIA_ENTE As String = "Enti"
	Const TIPOLOGIA_RUOLO As String = "Ruoli"
	Const TIPOLOGIA_PROGETTO As String = "Progetti"



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
	Private Sub CancellaMessaggiInfo()
		msgErrore.Text = String.Empty
		msgInfo.Text = String.Empty
		msgConferma.Text = String.Empty
		lblmess.Text = String.Empty
	End Sub

	Private Sub NascondiMenuLaterale()
		Session("TP") = True
	End Sub

#End Region
#Region "Style TreeView"
	Private Sub ImpostaStyleTreeView()
		TrwVincoli.ShowLines = True
	End Sub

	Private Sub ImpostaStyleVincoloRosso(ByRef nodo As TreeNode)
		Dim style As TreeNodeStyle = New TreeNodeStyle()
		nodo.ImageUrl = URL_IMMAGINE_VINCOLO_ROSSO
		nodo.ImageToolTip = "Non Valido"
	End Sub
	Private Sub ImpostaStyleVincoloValido(ByRef nodo As TreeNode)
		Dim style As TreeNodeStyle = New TreeNodeStyle()
		nodo.ImageUrl = URL_IMMAGINE_VINCOLO_VERDE
		nodo.ImageToolTip = "Ok"
	End Sub
	Private Sub ImpostaStyleVincoloDaValidare(ByRef nodo As TreeNode)
		Dim style As TreeNodeStyle = New TreeNodeStyle()
		nodo.ImageUrl = URL_IMMAGINE_VINCOLO_GIALLO
		nodo.ImageToolTip = "Vincolo da validare"
	End Sub
	Private Sub ImpostaStyleOkPresentazione(ByRef nodo As TreeNode)
		Dim style As TreeNodeStyle = New TreeNodeStyle()
		nodo.ImageUrl = URL_IMMAGINE_OK_PRESENTAZIONE
		nodo.ImageToolTip = "Vincolo Valido"
	End Sub
	Private Sub ImpostaStyleKOPresentazione(ByRef nodo As TreeNode)
		Dim style As TreeNodeStyle = New TreeNodeStyle()
		nodo.ImageUrl = URL_IMMAGINE_KO_PRESENTAZIONE
		nodo.ImageToolTip = "Vincolo Non Valido"
	End Sub
	Private Sub ImpostaStyleClasse(ByRef nodo As TreeNode)
		Dim style As TreeNodeStyle = New TreeNodeStyle()
		nodo.ImageUrl = URL_IMMAGINE_CLASSE
        nodo.ImageToolTip = "Sezione"
	End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
        Dim bytbordo As Byte 'variabile passaggio parametro se accreditato (ente o ruolo)
        Dim BytPresentazione As Byte
        tipologia = Request.QueryString("tipologia")
        idEnte = Session("IdEnte")
        VerificaSessione()
        If Session("IdStatoEnte") = 8 Then
            CancellaMessaggiInfo()
            msgConferma.Visible = True
            msgConferma.Text = "La domanda di iscrizione &egrave; stata presentata ed &egrave; ora in fase di istruttoria."
            ImgPresentazioneDomanda.Visible = False
        End If
        'Seleziono la classe cui voglio eseguire
        Dim VengoDaAlbero As Boolean = True 'serve solo per abilitare le regione al READ dei documenti

        If Page.IsPostBack = False Then

            ImpostaStyleTreeView()
            If ClsUtility.ControlloRegioneCompetenza(Session("TipoUtente"), Session("idEnte"), Session("Utente"), Session("IdStatoEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), VengoDaAlbero) = False Then
                BloccaMaschera("L'ente non è di propria competenza. Impossibile effettuare modifiche.")
                Exit Sub
            End If

            If Session("TipoUtente") <> "U" Then
                CmdInforScu2.Visible = False
            End If
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                imbComuni.Visible = True
                'imgsediduplicate.Visible = True
                imgSediSovradimensionate.Visible = True
                imgsettori.Visible = True
                imgConsultaDoc.Text = "Consulta e Valida Documenti Ente" 'aggiunto da s.c. per accreditaento/adeguamento SCU
                'Nascosto da Danilo il 02/11/2015 per manifesta inutilità
                'imgVisErrori.Visible = True
                If VerificaModIns() = True And Request.QueryString("tipologia") = "Enti" Then
                    DivAccreditaRiserva.Visible = True
                    BtnAssegna.Visible = True
                    CaricaCausaliRiserva()
                    CaricaRiservaSel()
                End If
            End If
            If Not Request.QueryString("identepadre") Is Nothing Then
                hf_idEntePadre.Value = Request.QueryString("identepadre")
            End If

            'Aggiunto Da Alessandra Taballione il 20/06/2005
            myQuerySql = "select statienti.istruttoria,classiaccreditamento.defaultclasse,enti.tipologia,TipologieEnti.idtipologieenti from enti " &
               "inner join TipologieEnti on TipologieEnti.Descrizione=enti.tipologia " &
               "inner join statienti on statienti.idstatoente=enti.idstatoente " &
               "inner join classiaccreditamento on classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamento " &
               "where idente= " & Session("IdEnte") & ""

            ChiudiDataReader(dtrGen)
            dtrGen = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrGen.HasRows = True Then
                dtrGen.Read()
                txtstato.Value = dtrGen("istruttoria") '1 = Ente in Adeguamento o Istruttria  0 = Altri stati
                txtclasse.Value = dtrGen("defaultclasse") '1 = Nessuna Classe Attribuita  0 = Una Classe Attribuita
                '' aggiungo campi TIPOLOGIA
                hddAmbito.Value = dtrGen("idtipologieenti")
                hddTipologia.Value = dtrGen("tipologia")
            End If

            ChiudiDataReader(dtrGen)



            'Controllo relativo ai tasti DISACCREDITA e GENERA CODICE
            If Request.QueryString("tipologia") = "Enti" Then 'controllo se ente per gestione
                lblNomeFrameset.Text = "Presentazione e invio domanda" '"Gestione Fasi e Documenti Ente"
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then

                    Dim SqlLocal As String
                    'Controllo chè l'ente non sia nello stato (Chiuso, Richiesta Registrazione, Registrazione Respinta)
                    'e che l'ente non sia un ente figlio
                    dtrGen = ClsServer.CreaDatareader("Select StatiEnti.DefaultStato, StatiEnti.Sospeso, StatiEnti.Chiuso From Enti INNER JOIN StatiEnti ON Enti.IdStatoEnte = StatiEnti.IdStatoEnte Where IdEnte = " & Session("IdEnte") & " And IdClasseAccreditamentoRichiesta IN (1,2,3,4,6)", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    If dtrGen.HasRows = True Then
                        dtrGen.Read()
                        If dtrGen.Item(0) = False And dtrGen.Item(1) = False And dtrGen.Item(2) = False Then
                            ChiudiDataReader(dtrGen)

                            'Controllo lo stato ABILITATO / DISABILITATO
                            LblStato.Visible = True
                            dtrGen = ClsServer.CreaDatareader("Select * From Enti Where IdEnte = " & Session("IdEnte") & " And FlagForzaturaAccreditamento = 1", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                            If dtrGen.HasRows = True Then
                                'L'Utente può inserire dati
                                LblStato.Text = ENTE_ABILITATO
                                LblStato.CssClass = "msgConferma"
                            Else
                                ChiudiDataReader(dtrGen)
                                dtrGen = ClsServer.CreaDatareader("Select * From ProcessiTemporali Where GetDate() BETWEEN DataInizio And DataFine", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                If dtrGen.HasRows = True Then
                                    'Controllo che lo stato dell'ente sia registrato o attivo
                                    ChiudiDataReader(dtrGen)
                                    dtrGen = ClsServer.CreaDatareader("Select * From Enti INNER JOIN StatiEnti ON Enti.IdStatoEnte = StatiEnti.IdStatoEnte Where IdEnte = " & Session("IdEnte") & " And (StatiEnti.PresentazioneProgetti = 1 Or (PresentazioneProgetti = 0 And DefaultStato = 0 And Chiuso = 0 And Sospeso = 0 And Istruttoria = 0))", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    If dtrGen.HasRows = True Then
                                        'L'utente può inserire dati
                                        LblStato.Text = ENTE_ABILITATO
                                        LblStato.CssClass = "msgConferma"
                                    Else
                                        'L'utente non può iserire i dati
                                        LblStato.Text = ENTE_DISABILITATO
                                        LblStato.CssClass = "msgErrore"

                                    End If
                                Else
                                    'L'utente non può iserire i dati
                                    LblStato.Text = ENTE_DISABILITATO
                                    LblStato.CssClass = "msgErrore"
                                End If
                            End If
                            ChiudiDataReader(dtrGen)
                        Else
                            ChiudiDataReader(dtrGen)
                        End If
                    Else
                        ChiudiDataReader(dtrGen)
                    End If

                    'Se L' UTENTE è UNSC ho la possibilità di Generare il codice Nazionale o Disaccreditare l'ente
                    SqlLocal = "Select CodiceRegione From Enti Where IdEnte = " & Session("IdEnte") & " And idclasseaccreditamentoRichiesta Not In (" &
                         "Select IdClasseAccreditamento From ClassiAccreditamento Where DefaultClasse = 1)"
                    ChiudiDataReader(dtrGen)
                    dtrGen = ClsServer.CreaDatareader(SqlLocal, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    If dtrGen.HasRows = True Then
                        dtrGen.Read()
                        If Not IsDBNull(dtrGen("CodiceRegione")) Then
                            If dtrGen("CodiceRegione") <> "" Then
                                txtCodiceNazionale.Value = dtrGen("Codiceregione")
                            End If
                        End If
                    Else
                        ChiudiDataReader(dtrGen)
                        If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
                            cmdChiudiEnte.Visible = True
                        End If

                    End If
                    ChiudiDataReader(dtrGen)
                End If

                Dim intappo As Integer = 0
                Dim strsql As String
                Dim dtrgenerico As Data.SqlClient.SqlDataReader

                '************************MODIFICATA QUERY PER PRESENTAZIONE DOMANDA
                strsql = "Select Distinct a.idclasseaccreditamentorichiesta,b.classeaccreditamento," &
                   "a.denominazione,a.idclasseaccreditamento,c.presentazioneprogetti," &
                   "ClasseAccreditamentoSubIudice, d.idente, b.maxsedi " &
                   "From Enti a " &
                   "INNER JOIN classiaccreditamento b on a.idclasseaccreditamentorichiesta=b.idclasseaccreditamento " &
                   "INNER JOIN statienti c on a.idstatoente=c.idstatoente " &
                   "LEFT JOIN SospensioneAlbero d on a.idente=d.idente " &
                   "WHERE a.idente=" & Session("IdEnte") & ""
                dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'verifico accreditamento

                Do While dtrgenerico.Read
                    'valorizzo componente per salvare dato campo ClasseAccreditamentoSubIudice su T. enti 
                    If dtrgenerico.GetValue(5) = True Then
                        lblMessaggio.CssClass = "1"
                    End If
                    'verifico se accreditata per segnalarlo all'utente al caricamento dell'albero (per ente)
                    If dtrgenerico.GetValue(4) = True Then
                        bytbordo = 1
                        bytePassaBordo = 1 'valorizzo variabile per query partenza relazione vincoli
                    Else
                        bytbordo = 0
                    End If
                    If IsDBNull(dtrgenerico.GetValue(6)) = False Then
                        BytPresentazione = 1
                    Else
                        BytPresentazione = 0
                    End If

                    'controllo visibilità tasto presenta per enti con maxsedi > 0
                    If IsDBNull(dtrgenerico.GetValue(7)) = True Or Session("IdStatoEnte") = 8 Then
                        ImgPresentazioneDomanda.Visible = False
                    Else
                        If dtrgenerico.GetValue(7) = 0 Then
                            ImgPresentazioneDomanda.Visible = False
                        End If
                    End If

                    intappo = dtrgenerico.GetValue(0) 'valorizzo con l'idclasseaccreditamentorichiesta per parametro di entrata 
                    lblMessaggio.Text = "Vincoli di Sezione"
                Loop

                If intappo <> 0 Then
                    ChiudiDataReader(dtrgenerico)
                    CaricamentoAlbero(intappo, , , , bytbordo)

                    'Controllo se Flag Forzatura Accreditamento = 0
                    strsql = "Select IsNull(FlagForzaturaAccreditamento,0) From Enti Where IdEnte = " & Session("IdEnte")
                    dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    dtrgenerico.Read()
                    If dtrgenerico.Item(0) = False Then
                        ChiudiDataReader(dtrgenerico)
                        'Controllo Processo Temporale
                        strsql = "Select * From ProcessiTemporali Where Accreditamento = 1 And GetDate() BETWEEN DataInizio And DataFine"
                        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        If dtrgenerico.HasRows = False Then
                            ImgPresentazioneDomanda.Visible = False
                        Else
                            'Controllo se lo stato dell'ente risulta In Adeguamento o in Istruttoria (è gia stata effettuata una presentazione)
                            ChiudiDataReader(dtrgenerico)
                            strsql = "Select StatiEnti.Istruttoria From Enti INNER JOIN StatiEnti ON Enti.IdStatoEnte = StatiEnti.IdStatoEnte Where Enti.IdEnte = " & Session("IdEnte")
                            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'verifico accreditamento
                            dtrgenerico.Read()
                            If dtrgenerico.Item("Istruttoria") = True Or Session("IdStatoEnte") = 8 Then
                                ImgPresentazioneDomanda.Visible = False
                            End If
                        End If
                    End If
                    ChiudiDataReader(dtrgenerico)



                    'Visualizzazione della maschera di presentazione della domanda
                    If Not Request.QueryString("Presenta") Is Nothing Then
                        VisualizzaCodiciAssegnati()
                        'StampaCopertinaAccreditamentoAdeguamento()
                        StampaCopertina(Request.QueryString("IdEnteFase"))
                        imgStampaCopertina.Visible = True
                        'Dim intIDEnteFase As Integer
                        'intIDEnteFase = RicavaIdEnteFase(Session("IdEnte"))
                        'ClsServer.ChiudiFaseEnte(intIDEnteFase, Session("Utente"), Session("conn"))
                    End If

                Else
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                AbilitaPulsantiFunzione(Session("IdEnte"))

            ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'gestisco se ruoli

                CmdInforScu2.Visible = False
                lblNomeFrameset.Text = "Valutazione Ruolo Risorsa"
                fsOperativita.Visible = False
                fsFasi.Style("Width") = "100%"
                idEntePersonale = Request.QueryString("IDEntePersonale")
                idRuolo = Request.QueryString("IDRuolo")
                ImgPresentazioneDomanda.Visible = False
                Dim dtrgenerico As Data.SqlClient.SqlDataReader
                myQuerySql = "select b.accreditato, a.cognome + ' ' + a.nome as Titolo from entepersonale a inner join" &
                " entepersonaleruoli b on a.identepersonale=b.identepersonale" &
                " where b.identepersonale = " & Request.QueryString("IDEntePersonale") & " and" &
                " b.idruolo = " & Request.QueryString("IDRuolo") & ""
                dtrgenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                Do While dtrgenerico.Read '
                    lblMessaggio.Text = "Vincoli di Ruolo: " & dtrgenerico.GetValue(1) 'nome e cognome person.
                    If dtrgenerico.GetValue(0) = 1 Then 'verifico se accreditata per segnalarlo all'utente al caricamento dell'albero (per ente)
                        bytbordo = 1
                    End If
                Loop
                dtrgenerico.Close()
                dtrgenerico = Nothing

                CaricamentoAlbero(, , , , bytbordo)
                cmdChiudiEnte.Text = "Accreditamento Negativo"
            ElseIf Request.QueryString("tipologia") = "Progetti" Then 'gestico progetti
                CmdInforScu2.Visible = False
                ImgPresentazioneDomanda.Visible = False
                Dim dtrgenerico As Data.SqlClient.SqlDataReader
                myQuerySql = "select DaGraduare, titolo from attività a" &
                " left join statiattività b on a.idstatoattività=b.idstatoattività" &
                " where idattività=" & Request.QueryString("idattivita") & ""

                dtrgenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                Do While dtrgenerico.Read
                    lblMessaggio.Text = "Vincoli di Progetto: " & dtrgenerico.GetValue(1) 'nome e cognome person.
                    If dtrgenerico.GetValue(0) = True Then 'verifico se accreditata per segnalarlo all'utente al caricamento dell'albero (per ente)
                        bytbordo = 1
                    Else
                        bytbordo = 0
                    End If
                Loop
                dtrgenerico.Close()
                dtrgenerico = Nothing

                CaricamentoAlbero(, , , , bytbordo)
            End If
        End If

    End Sub
	Private Sub VisualizzaCodiciAssegnati()
		Dim query As String
		Dim myReader As System.Data.SqlClient.SqlDataReader
		query = "Select tipofase from entifasi Where IdEnteFase = " & Request.QueryString("IDEnteFase")
		myReader = ClsServer.CreaDatareader(query, Session("Conn"))
		myReader.Read()
		Dim tipofase As Integer
		tipofase = myReader.Item("tipofase")
		If tipofase = 3 Then
			Div1.Visible = True
			codiciAssegnati.Visible = False
		Else
			Div1.Visible = False
			codiciAssegnati.Visible = True
		End If


		ChiudiDataReader(myReader)
		query = "Select CodiceRegione,RegioniCompetenze.CodiceRegioneCompetenza,RegioniCompetenze.Descrizione From Enti INNER JOIN RegioniCompetenze ON Enti.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza Where Enti.IdEnte = " & Session("IdEnte")
		myReader = ClsServer.CreaDatareader(query, Session("Conn"))
		myReader.Read()
		LblCodice.Text = myReader.Item("CodiceRegioneCompetenza") & " - " & myReader.Item("CodiceRegione")
		LblRegione.Text = myReader.Item("Descrizione")
		ChiudiDataReader(myReader)
	End Sub
	Private Sub BloccaMaschera(ByVal strmessaggio As String)
		ImgAbilitaAdeguamento.Visible = False
		Imgaccredita.Visible = False
		ImgAnnullaInizioAdeguamento.Visible = False
		imgConsultaDoc.Visible = False
		cmdChiudiEnte.Visible = False
		imgElencoDocumentiEnti.Visible = False
		ImgInizioArt10.Visible = False
		ImgInizioArt2.Visible = False
		ImgPresentaDocArt10.Visible = False
		ImgPresentaDocArt2.Visible = False
		ImgPresentazioneDomanda.Visible = False
        'imgsediduplicate.Visible = False
		imgSediSovradimensionate.Visible = False
		imgsettori.Visible = False
		imgStampaCopertina.Visible = False
		ImgVariazioneEnte.Visible = False
		imgVisErrori.Visible = False

		imgAnagraficaSistemi.Visible = False

		msgErrore.Text = strmessaggio

		'adc
		LinkAggiornaSediArt2.Visible = False
	End Sub
	Private Function AlertDoppioniSedi() As Boolean
		Dim MyCommand As SqlClient.SqlDataAdapter
		Dim DsSediCondivise As New DataSet
		Dim myAlert As Boolean

		MyCommand = New SqlClient.SqlDataAdapter("SP_RITORNA_ANOMALIA_SEDI_CONDIVISE", CType(Session("conn"), SqlClient.SqlConnection))
		MyCommand.SelectCommand.CommandType = CommandType.StoredProcedure
		MyCommand.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")
		MyCommand.Fill(DsSediCondivise)
		If DsSediCondivise.Tables(0).Rows.Count > 0 Then
			myAlert = True
		Else
			myAlert = False
		End If

		Return myAlert
	End Function
	Private Function AlertSediSovradimensionate() As Boolean
		Dim myCommand As SqlClient.SqlDataAdapter
		Dim dsSediCondivise As New DataSet
		Dim myAlert As Boolean

		myCommand = New SqlClient.SqlDataAdapter("SP_RITORNA_SEGNALAZIONE_SEDI_SOVRADIMENSIONATE", CType(Session("conn"), SqlClient.SqlConnection))
		myCommand.SelectCommand.CommandType = CommandType.StoredProcedure
		myCommand.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")
		myCommand.Fill(dsSediCondivise)
		If dsSediCondivise.Tables(0).Rows.Count > 0 Then
			myAlert = True
		Else
			myAlert = False
		End If

		Return myAlert
	End Function

	Private Function ControllaVincoliDocumentali(ByVal idEnte As Integer, ByVal vincolo As Integer) As Int16
		Dim dataReader As System.Data.SqlClient.SqlDataReader

		Dim valoreVincolo As Int16
		'***Questa Routine controlle se i vincoli (per ente,personale e progetto) 
		'***siano gia stati documentati restituendo valori che identificano la
		'***la situazione
		If Request.QueryString("tipologia") = "Enti" Then 'modifica per la doppia gestione dell'albero

			myQuerySql = "Select Valore From FlagEnti Where IdEnte = " & idEnte & " AND IdVincolo = " & vincolo
			dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			If dataReader.HasRows = False Then
				dataReader.Close()
				Return 1 'non documentato Enti
			Else
				dataReader.Read()
				If dataReader.Item("Valore") = 0 Then
					dataReader.Close()
					Return 2 'documentato NO Enti
				Else
					dataReader.Close()
					Return 3 'documentato SI Enti
				End If
			End If

		ElseIf Request.QueryString("tipologia") = "Ruoli" Then
			myQuerySql = "Select Valore From flagentepersonale Where IdEntePersonale = " & idEnte & " AND IdVincolo = " & vincolo
			dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			If dataReader.HasRows = False Then
				valoreVincolo = 1 'non documentato Personale
			Else
				dataReader.Read()
				If dataReader.Item("Valore") = 0 Then
					valoreVincolo = 2 'documentato NO Personale
				Else
					valoreVincolo = 3 'documentato SI Personale
				End If
			End If

		ElseIf Request.QueryString("tipologia") = "Progetti" Then
			myQuerySql = "Select Valore From FlagAttività Where Idattività = " & idEnte & " AND IdVincolo = " & vincolo
			dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			If dataReader.HasRows = False Then
				valoreVincolo = 1 'non documentato Personale
			Else
				dataReader.Read()
				If dataReader.Item("Valore") = 0 Then
					valoreVincolo = 2 'documentato NO Personale
				Else
					valoreVincolo = 3 'documentato SI Personale
				End If
			End If

		End If
		ChiudiDataReader(dataReader)
		Return valoreVincolo
	End Function

	Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
		If Not Request.QueryString("Arrivo") Is Nothing Then
			Select Case Request.QueryString("Arrivo")
				Case Is = "WfrmMain.aspx"
					Response.Redirect("WfrmMain.aspx")
				Case Is = "WfrmEntidaAccreditare.aspx"
					Response.Redirect("WfrmEntidaAccreditare.aspx")
				Case Is = "WfrmRisorsedaAccreditare.aspx"
					Response.Redirect("WfrmRisorsedaAccreditare.aspx")
				Case Is = "WfrmProgettidaValutare.aspx"
					Response.Redirect("WfrmProgettidaValutare.aspx?VengoDa=Valutare")
				Case Is = "WfrmProgettidaValutare.aspxModifica"
					Response.Redirect("WfrmProgettidaValutare.aspx?VengoDa=Modifica")
				Case Is = "WfrmElencoEntiAccordo.aspx"
					Session("Idente") = Request.QueryString("identepadre")
					Session("txtCodEnte") = Left(Session("txtCodEnte"), 7) 'aggiunta da DS il 29/05/2017
					Response.Redirect("WfrmElencoEntiAccordo.aspx?idente=" + CStr(Session("IdEnte")))
				Case Else
					Response.Redirect("WfrmMain.aspx")
			End Select
		Else
			If hf_idEntePadre.Value <> "" Then
				Session("Idente") = hf_idEntePadre.Value

				Response.Redirect("WfrmElencoEntiAccordo.aspx?idente=" + CStr(Session("IdEnte")))
			Else
				Response.Redirect("WfrmMain.aspx")
			End If
		End If
	End Sub


	Private Function ControllaAccreditamentoFigli() As Boolean
		'verifica che tutti gli enti figlio siano gia stati valutati
		myQuerySql = "SELECT  enti.idente " &
		" FROM entirelazioni " &
		" INNER JOIN enti ON entirelazioni.IDEnteFiglio = enti.IDEnte " &
		" INNER JOIN statienti ON enti.IDStatoEnte = statienti.IDStatoEnte " &
		" WHERE(entirelazioni.IDEntePadre = " & Session("IdEnte") & ")" &
		" AND (presentazioneprogetti= 0 and statienti.DefaultStato = 0 and chiuso=0 and sospeso=0 )" &
		" AND entirelazioni.DataFineValidità IS NULL"

		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If myDataReader.HasRows = True Then
			ControllaAccreditamentoFigli = False
		Else
			ControllaAccreditamentoFigli = True
		End If
		ChiudiDataReader(myDataReader)
	End Function

	Private Function ControllaAccreditamentoPadre() As Boolean
		'verifica che l'ente padre e' in valutazione
		myQuerySql = "SELECT  enti.idente " &
		" FROM entirelazioni " &
		" INNER JOIN enti ON entirelazioni.IDEntePadre = enti.IDEnte " &
		" INNER JOIN statienti ON enti.IDStatoEnte = statienti.IDStatoEnte " &
		" WHERE(entirelazioni.IDEnteFiglio = " & Session("IdEnte") & ")" &
		" AND (Istruttoria = 0 )" &
		" AND entirelazioni.DataFineValidità IS NULL"
		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If myDataReader.HasRows = True Then
			ControllaAccreditamentoPadre = False
		Else
			ControllaAccreditamentoPadre = True
		End If
		ChiudiDataReader(myDataReader)
	End Function

	Private Function ControllaCertificazioneFigli() As Boolean
		ChiudiDataReader(myDataReader)
		Dim strsql2 As String = "Select certificazione from entisediattuazioni  inner join entisedi on entisediattuazioni.identesede=entisedi.identesede where idente=" & Session("IdEnte") & " and certificazione=2 "
		Dim dtrcertificazione As SqlClient.SqlDataReader
		dtrcertificazione = ClsServer.CreaDatareader(strsql2, Session("conn"))
		If dtrcertificazione.HasRows = True Then
			ControllaCertificazioneFigli = False
		Else
			ControllaCertificazioneFigli = True
		End If

		ChiudiDataReader(dtrcertificazione)
	End Function
	Private Function ControllaCertificazionePadre() As Boolean
		ChiudiDataReader(myDataReader)
		Dim strsql1 As String = "Select certificazione from entisediattuazioni where identecapofila=" & Session("IdEnte") & " and certificazione=2 "
		Dim dtrcertificazione As SqlClient.SqlDataReader
		dtrcertificazione = ClsServer.CreaDatareader(strsql1, Session("conn"))
		If dtrcertificazione.HasRows = True Then
			ControllaCertificazionePadre = False
		Else
			ControllaCertificazionePadre = True

		End If
		ChiudiDataReader(dtrcertificazione)
	End Function
	Private Function ControllaAccreditamentoRisorse() As Boolean
		'verifica che tutte le risorse siano gia state valutate
		myQuerySql = "SELECT entepersonaleruoli.IDEntePersonale " &
		" FROM entepersonale INNER JOIN " &
		" entepersonaleruoli ON entepersonale.IDEntePersonale = entepersonaleruoli.IDEntePersonale " &
		" INNER JOIN ruoli ON entepersonaleruoli.idruolo = ruoli.idruolo " &
		" WHERE (entepersonale.IDEnte = " & Session("IdEnte") & ") AND (entepersonaleruoli.Accreditato = 0) AND (entepersonale.DataFineValidità IS NULL) AND (entepersonaleruoli.datafinevalidità is null) and (RUOLI.RUOLOACCREDITAMENTO = 1) and  Ruoli.Nascosto = 0 "
		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If myDataReader.HasRows = True Then
			ControllaAccreditamentoRisorse = False
		Else
			ControllaAccreditamentoRisorse = True
		End If
		ChiudiDataReader(myDataReader)
	End Function
	'/* Controllo se nell'Ente da Accreditare ci sono risorse e enti non accreditati */
	'Richiamo Store che verifica se l'UNSC/RPA ha completato tutte le operazioni necessarie per procedere all'acreditamento/adeguamento*/
	Private Sub AccreditamentoEnte()
		CancellaMessaggiInfo()
		Dim idStatoEnte As Int64
		Dim BlnEnteFiglio As Boolean
		Dim strRitornoStore As String
		strRitornoStore = LeggiStoreVerificaValutazioneAccreditamento(Session("IdEnte"))

		If strRitornoStore <> "" Then

			msgErrore.Text = strRitornoStore
			Exit Sub
		End If


		If ControllaAccreditamentoRisorse() = False Then
			msgErrore.Text = "L'Ente non può essere Accreditato perchè alcune Risorse non risultano ancora valutate."
			Exit Sub
		End If
		If ControllaAccreditamentoPadre() = False Then
			msgErrore.Text = "L'Ente non può essere Accreditato perchè l'Ente Principale non risulta in Valutazione."
			Exit Sub
		End If
		'Controllo se esistono ancora servizi acquisiti da valutare
		If ControlloServizi() = True Then
			msgErrore.Text = "L'Ente non può essere Accreditato perchè esistono ancora servizi acquisiti in Valutazione."
			Exit Sub
		End If


		'Aggiunto da Alessandra Taballione
		Dim dtrgenerico As Data.SqlClient.SqlDataReader
		'Eseguo la Store se l'ente accreditato è di tipo padre (Classe Richiesta <= 4)
		dtrgenerico = ClsServer.CreaDatareader("Select IdEnte From Enti Where IdEnte = " & Session("IdEnte") & " And IdClasseAccreditamentoRichiesta <= 4", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If dtrgenerico.HasRows = True Then
			BlnEnteFiglio = False
			ChiudiDataReader(dtrgenerico)
			'Eseguo la Store SP_ELIMINA_MIGRAZIONE_DATI
			If ClsServer.EseguiStoreSP_ELIMINA_MIGRAZIONE_DATI(Session("IdEnte"), "SP_ELIMINA_MIGRAZIONE_DATI", Session("conn")) = 1 Then
				msgErrore.Text = "Si è verificato un errore durante l'aggiornamento dei dati. Contattare l'assistenza."
				Exit Sub
			End If
		Else
			BlnEnteFiglio = True
			ChiudiDataReader(dtrgenerico)
		End If

		Dim bytForzaturaEnte, bytFEStorico As Byte
		'***Eliminazione Record su tabella per ripristino situazione precedente a presentazione
		Dim sqlcommand As Data.SqlClient.SqlCommand
		sqlcommand = New SqlClient.SqlCommand("delete from SospensioneAlbero where idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		sqlcommand.ExecuteNonQuery()
		sqlcommand.Dispose()
		'salvo se accreditato su variabile
		Dim strSubIu As String = InStr(TrwVincoli.Nodes.Item(0).Value, NODE_VALUE_FOREST_GREEN)
		'controllo subiudice
		If lblMessaggio.CssClass = "1" And strSubIu <> "0" Then
			Dim ComUpdEnti As New Data.SqlClient.SqlCommand("update enti set" &
			" ClasseAccreditamentoSubIudice=0 where idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			ComUpdEnti.ExecuteNonQuery()
			ComUpdEnti.Dispose()
			msgConferma.Text = "Accreditamento avvenuto con successo"
			Exit Sub
		End If

		dtrgenerico = ClsServer.CreaDatareader("select idclasseaccreditamento," &
							" idclasseaccreditamentorichiesta,idstatoente,forzatura from enti" &
							" where idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		'verifico se è possibile accreditare verificando diversita idC.A. con idC.A.R.
		dtrgenerico.Read()
		idStatoEnte = dtrgenerico.GetValue(2)
		'CONTROLLO LO STATO DEL'ENTE PADRE (ISTRUTTORIA) O ENTE FIGLIO (REGISTRATO) per capire se è un nuovo accreditamento
		If dtrgenerico("idstatoente") = 8 Or dtrgenerico("idstatoente") = 6 Then
			dtrgenerico.Close()
			dtrgenerico = Nothing

			Dim ComUpdEnti As New Data.SqlClient.SqlCommand("Update enti set" &
			" Idclasseaccreditamento=idclasseaccreditamentorichiesta,idstatoente=(select idstatoente from statienti where presentazioneprogetti=1)," &
			" dataultimaclasseaccreditamento = getdate(),DataAccreditamento=GetDate(),forzatura=" & bytForzaturaEnte & "" &
			" where idente=" & Session("IdEnte") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			ComUpdEnti.ExecuteNonQuery()
			ComUpdEnti.Dispose()


			'proseguo andando a popolare la tabella della cronologia (storico)
			Dim strquery As String = "insert into" &
			 " cronologiaentistati(idente,idstatoente,datacronologia,usernameaccreditatore," &
			 " idclasseaccreditamento,idtipocronologia,forzatura) select" &
			 " " & Session("IdEnte") & "," & idStatoEnte & ",getdate(),'" & ClsServer.NoApice(Session("Utente")) & "'," &
			 " (select idclasseaccreditamento from enti where idente=" & Session("IdEnte") & "),0," & bytFEStorico & "" &
			 " from statienti where presentazioneprogetti=1"
			Dim CmdInsCronologiaEnti As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdInsCronologiaEnti.ExecuteNonQuery()
			CmdInsCronologiaEnti.Dispose()

            msgConferma.Text = "Iscrizione avvenuta con successo"

			Dim strquery1 As String
			strquery1 = "Update entisediattuazioni  set usernamestato = '" & Session("Utente") & "' , dataultimostato = getdate(), idstatoentesede = 1 from entisediattuazioni A INNER JOIN ENTISEDI B ON A.IDENTESEDE = B.IDENTESEDE where A.IDSTATOENTESEDE  = 4 and substring(A.usernamestato,1,1) <> 'N' AND A.CERTIFICAZIONE = 1 AND B.IDENTE = " & Session("idEnte")
			Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, Session("Conn"))
			CmdUpDateSediAttuazione.ExecuteNonQuery()
			CmdUpDateSediAttuazione.Dispose()

			Dim strquery2 As String
			strquery2 = "Update entisediattuazioni  set usernamestato = '" & Session("Utente") & "' , dataultimostato = getdate(), idstatoentesede = 2 from entisediattuazioni A INNER JOIN ENTISEDI B ON A.IDENTESEDE = B.IDENTESEDE where A.IDSTATOENTESEDE = 4 and substring(A.usernamestato,1,1) <> 'N' AND A.CERTIFICAZIONE = 0 AND B.IDENTE = " & Session("idEnte")
			Dim CmdUpDateSediAttuazioneBis As New Data.SqlClient.SqlCommand(strquery2, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdUpDateSediAttuazioneBis.ExecuteNonQuery()
			CmdUpDateSediAttuazioneBis.Dispose()

			Dim strquery3 As String
			strquery3 = "UPDATE ENTISEDI  SET usernamestato = '" & Session("Utente") & "' , dataultimostato = getdate(), idstatoentesede = 1 FROM ENTISEDI B INNER JOIN ENTISEDIATTUAZIONI A ON A.IDENTESEDE = B.IDENTESEDE WHERE B.IDENTE = " & Session("idEnte") & " And B.IDSTATOENTESEDE = 4 And A.IDSTATOENTESEDE = 1 "
			Dim CmdUpDateSedi As New Data.SqlClient.SqlCommand(strquery3, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdUpDateSedi.ExecuteNonQuery()
			CmdUpDateSedi.Dispose()

			Dim strquery4 As String
			strquery4 = "UPDATE ENTISEDI SET usernamestato = '" & Session("Utente") & "' , dataultimostato = getdate(), idstatoentesede = 2 FROM ENTISEDI B  INNER JOIN ENTISEDIATTUAZIONI A ON A.IDENTESEDE = B.IDENTESEDE WHERE B.IDENTE = " & Session("idEnte") & " And B.IDSTATOENTESEDE  = 4 "
			Dim CmdUpDateSediBis As New Data.SqlClient.SqlCommand(strquery4, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdUpDateSediBis.ExecuteNonQuery()
			CmdUpDateSediBis.Dispose()
		End If


		'Competenza Regionale
		Dim CmdCompetenza As New Data.SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If BlnEnteFiglio = False Then
			'Se non sono un ente figlio aggiorno la competenza (Dell'ente padre e dei figli)
			myQuerySql = "Update Enti Set IdRegioneCompetenza = dbo.RegioneCompetenza(" & Session("IdEnte") & "),IdRegioneAppartenenza = dbo.RegioneAppartenenza(" & Session("IdEnte") & ") Where IdEnte = " & Session("IdEnte")
			CmdCompetenza.ExecuteNonQuery()
			CmdCompetenza.Dispose()

			myQuerySql = "Update Enti Set Enti.IdRegioneCompetenza = Enti1.IdRegioneCompetenza,Enti.IdRegioneAppartenenza = Enti1.IdRegioneAppartenenza " &
					 "From Enti " &
					 "INNER JOIN EntiRelazioni ON Enti.IdEnte = EntiRelazioni.IdEnteFiglio And EntiRelazioni.DataFineValidità IS NULL " &
					 "INNER JOIN Enti AS Enti1 ON EntiRelazioni.IdEntePadre = Enti1.IdEnte " &
					 "Where Enti1.IdEnte = " & Session("IdEnte")
			CmdCompetenza.ExecuteNonQuery()
			CmdCompetenza.Dispose()
		Else
			'Se sono un ente figlio imposto la competenza del padre
			myQuerySql = "Update Enti Set Enti.IdRegioneCompetenza = Enti1.IdRegioneCompetenza,Enti.IdRegioneAppartenenza = Enti1.IdRegioneAppartenenza " &
					 "From Enti " &
					 "INNER JOIN EntiRelazioni ON Enti.IdEnte = EntiRelazioni.IdEnteFiglio " &
					 "INNER JOIN Enti AS Enti1 ON EntiRelazioni.IdEntePadre = Enti1.IdEnte " &
					 "Where Enti.IdEnte = " & Session("IdEnte")
			CmdCompetenza.ExecuteNonQuery()
			CmdCompetenza.Dispose()
		End If

		'Controllo Flag Forzatura Accreditamento
		'Controllo se è stato impostato il flag
		dtrgenerico = ClsServer.CreaDatareader("Select IsNull(FlagForzaturaAccreditamento,0) From Enti Where IdEnte = " & Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		dtrgenerico.Read()
		If dtrgenerico.Item(0) = True Then
			ChiudiDataReader(dtrgenerico)
			'Imposto a zero il bit di Flag Forzatura Accreditamento
			myQuerySql = "UpDate Enti Set FlagForzaturaAccreditamento = 0 Where IdEnte = " & Session("IdEnte")
			CmdCompetenza = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdCompetenza.ExecuteNonQuery()

			'Inserimento della cronologia di chiusura dell'apertura accreditamento
			myQuerySql = "Insert Into CronologiaApertureAccreditamento (IdEnte,Data,Utente,TipoOperazione) Values (" & Session("IdEnte") & ",GetDate(),'" & Session("Utente") & "',2)"
			CmdCompetenza = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdCompetenza.ExecuteNonQuery()
			CmdCompetenza.Dispose()
		Else
			ChiudiDataReader(dtrgenerico)
		End If

		Dim ComUpd As New Data.SqlClient.SqlCommand

		' CHIUSURA VALUTAZIONE FASE
		'AGGIORNO ESITI SINGOLI ELEMENTI RELATIVI ALLA FASE
		myQuerySql = " update EntiFasi_Enti set Stato=1 where IdEnteFase = (Select IdEnteFase from EntiFAsi where idente= " & Session("IdEnte") & " and TipoFase=1) "
		ComUpd = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		ComUpd.ExecuteNonQuery()

		myQuerySql = " update EntiFasi_Personale set Stato=1 where IdEnteFase =(Select IdEnteFase from EntiFAsi where idente= " & Session("IdEnte") & " and TipoFase=1) "
		ComUpd = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		ComUpd.ExecuteNonQuery()

		myQuerySql = " update EntiFasi_Risorse set Stato=1 where IdEnteFase = (Select IdEnteFase from EntiFAsi where idente= " & Session("IdEnte") & " and TipoFase=1) "
		ComUpd = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		ComUpd.ExecuteNonQuery()

		myQuerySql = " update EntiFasi_Sedi set Stato=1 where IdEnteFase =(Select IdEnteFase from EntiFAsi where idente= " & Session("IdEnte") & " and TipoFase=1) "
		ComUpd = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		ComUpd.ExecuteNonQuery()

		myQuerySql = " update EntiFasi_ServiziAcquisiti set Stato=1 where IdEnteFase = (Select IdEnteFase from EntiFAsi where idente= " & Session("IdEnte") & " and TipoFase=1) "
		ComUpd = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		ComUpd.ExecuteNonQuery()

		''--AGGIORNO STATO FASE IN VALUTATA = 4
		myQuerySql = " UPDATE EntiFasi set Stato = 4,usernamevalutazione = '" & Session("Utente") & "',Datavalutazione = GETDATE() where IdEnteFase = (Select IdEnteFase from EntiFAsi where idente= " & Session("IdEnte") & " and TipoFase=1) "
		ComUpd = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		ComUpd.ExecuteNonQuery()

		AbilitaPulsantiFunzione(Session("IdEnte"))

	End Sub

	Private Sub Imgaccredita_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles Imgaccredita.Click
		'***Questa routine gestisce l'accreditamento per ente,personale e progetto
		'***Ripristino situazione precedente alla "Presentazione Domanda" attraverso delete su SospensioneAlbero 

		Dim intstato As Int64

		If Request.QueryString("tipologia") = "Enti" Then 'tiro fuori classi accreditamento con idstatoente
			'distinguere se stiamo in accreditamento o adeguamento

			Dim bytForzaturaEnte, bytFEStorico As Byte

			myDataReader = ClsServer.CreaDatareader("select idclasseaccreditamento," &
					" idclasseaccreditamentorichiesta,idstatoente,forzatura from enti" &
					" where idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'verifico se è possibile accreditare verificando diversita idC.A. con idC.A.R.
			myDataReader.Read()
			intstato = myDataReader.GetValue(2)
			bytFEStorico = myDataReader.GetValue(3)
			Select Case TrwVincoli.Nodes.Item(0).Value
				Case Is = NODE_VALUE_FOREST_GREEN
					bytForzaturaEnte = 0
				Case Is = NODE_VALUE_GREEN
					bytForzaturaEnte = 0
				Case Else
					bytForzaturaEnte = 1
			End Select

			'agg da danilo e simona il 09/07/2009
			'CONTROLLO LO STATO DEL'ENTE PADRE (ISTRUTTORIA) O ENTE FIGLIO (REGISTRATO) per capire se è un nuovo accreditamento
			If myDataReader("idstatoente") = 8 Or myDataReader("idstatoente") = 6 Then
				ChiudiDataReader(myDataReader)
				AccreditamentoEnte()
			Else
				ChiudiDataReader(myDataReader)
				Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Costanti.VENGO_DA_ADEGUAMENTO_ADEGUAMENTO_OK & "&tipologia=" & tipologia)
			End If

		ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'gestione accreditamento ruolo
			Dim CmdInsCronologia As Data.SqlClient.SqlCommand
			Dim intidentepersonaleruolo As Int32
			Dim intValoreAccredito As Int32
			Dim bytForzaturaRuolo, bytFRStorico As Byte
			Dim dtrgenerico As Data.SqlClient.SqlDataReader
			'verifico se è stato già accreditato
			dtrgenerico = ClsServer.CreaDatareader("select identepersonaleruolo," &
			" accreditato,forzatura from entepersonaleruoli where" &
			" idruolo=" & Request.QueryString("IDRuolo") & " and" &
			" identepersonale=" & Request.QueryString("IDEntePersonale") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			dtrgenerico.Read()
			If IsDBNull(dtrgenerico.GetValue(0)) = False Then
				If dtrgenerico.GetValue(1) <> 1 Then
					intidentepersonaleruolo = dtrgenerico.GetValue(0)
					intValoreAccredito = dtrgenerico.GetValue(1)
					bytFRStorico = dtrgenerico.GetValue(2)
					Select Case TrwVincoli.Nodes.Item(0).Value
						Case Is = NODE_VALUE_FOREST_GREEN
							bytForzaturaRuolo = 0
						Case Is = NODE_VALUE_GREEN
							bytForzaturaRuolo = 0
						Case Else
							bytForzaturaRuolo = 1
					End Select
					dtrgenerico.Close()
					dtrgenerico = Nothing 'popolo cronologia
					CmdInsCronologia = New SqlClient.SqlCommand("insert into" &
					" cronologiaentepersonaleruoli" &
					" (identepersonaleruolo,accreditato,datacronologia," &
					" idtipocronologia,usernameaccreditatore,forzatura)" &
					" values (" & intidentepersonaleruolo & "," & intValoreAccredito & ",getdate(),0," &
					" '" & ClsServer.NoApice(Session("Utente")) & "'," & bytFRStorico & ")", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
					CmdInsCronologia.ExecuteNonQuery()
					CmdInsCronologia.Dispose()
					Dim ComUpdRuoli As Data.SqlClient.SqlCommand 'eseguo comando di modifica su entepersonaleruoli
					ComUpdRuoli = New SqlClient.SqlCommand("update entepersonaleruoli" &
					" set accreditato=1,dataaccreditamento=getdate()," &
					" usernameaccreditatore='" & ClsServer.NoApice(Session("Utente")) & "',forzatura=" & bytForzaturaRuolo & "" &
					" where idruolo=" & Request.QueryString("IDRuolo") & " and identepersonale=" & Request.QueryString("IDEntePersonale") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
					ComUpdRuoli.ExecuteNonQuery()
					ComUpdRuoli.Dispose()
                    msgConferma.Text = "Iscrizione avvenuta con successo"
				Else
					ChiudiDataReader(dtrgenerico)
				End If
			Else 'se non è possibile accreditare chiudo solamente dataREader
				ChiudiDataReader(dtrgenerico)
			End If


		ElseIf Request.QueryString("tipologia") = "Progetti" Then 'accredito progetti
			Dim dtrgenerico As Data.SqlClient.SqlDataReader
			Dim intstatoattività As Integer
			'*********controllo se l'attività è in graduatoria ed è stata confermata
			dtrgenerico = ClsServer.CreaDatareader("select idgraduatoriaprogetto" &
			" from graduatorieprogetti where statograduatoria=1" &
			" and idattività=" & CInt(Request.QueryString("idattivita")) & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			If dtrgenerico.HasRows = True Then
				msgErrore.Text = "Il progetto è già in Graduatoria. Impossibile accreditare"
				ChiudiDataReader(dtrgenerico)
				Exit Sub
			End If
			dtrgenerico.Close()
			dtrgenerico = Nothing

			dtrgenerico = ClsServer.CreaDatareader("select a.idattività,a.titolo," &
					" d.idstatoattività,d.statoattività,b.idbandoattività," &
					" b.idbando,c.idstatobandoattività,c.statobandoattività from attività a" &
					" inner join bandiattività b on a.idbandoattività=b.idbandoattività" &
					" inner join statibandiattività c on b.idstatobandoattività=c.idstatobandoattività" &
					" inner join statiattività d on a.idstatoattività=d.idstatoattività" &
					" where c.attivo = 1 And (d.DaValutare = 1 or d.chiusa=1) And a.idattività=" & CInt(Request.QueryString("idattivita")) & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'Verifico stato attuale dell'attività
			If dtrgenerico.HasRows = True Then 'entro in modifica per accreditare
				dtrgenerico.Read()
				intstatoattività = dtrgenerico.GetValue(2)
				ChiudiDataReader(dtrgenerico)
				Dim CmdModifica As Data.SqlClient.SqlCommand
				'modifico attività
				CmdModifica = New SqlClient.SqlCommand("update attività set" &
				" idstatoattività=(select idstatoattività from statiattività" &
				" where DaGraduare=1),dataultimostato=getdate() where idattività=" & Request.QueryString("idattivita") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				CmdModifica.ExecuteNonQuery()
				CmdModifica.Dispose()
				'inserisco nella cronologia
				CmdModifica = New SqlClient.SqlCommand("insert into CronologiaAttività" &
				" (idattività,idstatoattività,datacronologia,UsernameAccreditatore,idTipoCronologia)" &
				" values(" & Request.QueryString("idattivita") & "," &
				" " & intstatoattività & ", getdate(),'" & Session("Utente") & "',0)", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				CmdModifica.ExecuteNonQuery()
				CmdModifica.Dispose()
                msgConferma.Text = "Iscrizione avvenuta con successo"
			Else
				ChiudiDataReader(dtrgenerico)
				msgErrore.Text = "Impossibile valutare il progetto prima dell'accettazione dell'Istanza di Presentazione!"
			End If
		End If
	End Sub

	Private Sub cmdChiudiEnte_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudiEnte.Click
		'***Generata da Gianluigi Paesani in data:15/06/04
		'***Modificata da Gianluigi Paesani in data:15/03/05
		'***Questa routine annulla l'accredito per ente, personale e progetti
		'***Ripristino situazione precedente alla "Presentazione Domanda" attraverso delete su SospensioneAlbero 

		'Dim intstato As Int64
		CancellaMessaggiInfo()

		If Request.QueryString("tipologia") = "Enti" Then 'controllo formale se ente già disaccreditato
			'Aggiunto da Alessandra Taballione il 29.11.2005
			'Controllo sulle sedi dell'Ente utilizzate su progetti attivi
			If ControllaAttivitàSede() = False Or ControllaCoProgettazione() = False Then
                msgErrore.Text = "L'Ente non può avere una Iscrizione Negativa perchè alcune sedi risultano legate a Progetti non ancora Chiusi o risulta Ente coprogettante di Progetti non ancora Chiusi."
				Response.Redirect("WebElencoVisProgetti.aspx?Torna=Albero&IdEnte=" & Session("IdEnte") & "VengoDa=" & Costanti.VENGO_DA_ADEGUAMENTO & "&tipologia=" & tipologia)
				Exit Sub
			Else
				Dim Ente As String = Session("CodiceRegioneEnte") & " " & Session("Denominazione")
				lblChiudiEnte.Text = "Attenzione con questa operazione si procede alla chiusura dell'Ente " & Ente & " e di tutte le Sedi. Si desidera continuare? "
				divChiusuraEnte.Visible = True
			End If



			''Controllo se il padre e' in Valutazione
			'If ControllaAccreditamentoPadre() = False Then
			'    msgErrore.Text = "L'Ente non può essere Valutato perchè l'Ente Principale non risulta in Valutazione."
			'    Exit Sub
			'End If

			''Aggiunto da Alessandra Taballione
			'Dim dtrgenerico As Data.SqlClient.SqlDataReader
			''Eseguo la Store se l'ente accreditato è di tipo padre (Classe Richiesta <= 4)
			'dtrgenerico = ClsServer.CreaDatareader("Select IdEnte From Enti Where IdEnte = " & Session("IdEnte") & " And IdClasseAccreditamentoRichiesta <= 4", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'If dtrgenerico.HasRows = True Then
			'    ChiudiDataReader(dtrgenerico)
			'    'Eseguo la Store SP_ELIMINA_MIGRAZIONE_DATI
			'    If ClsServer.EseguiStoreSP_ELIMINA_MIGRAZIONE_DATI(Session("IdEnte"), "SP_ELIMINA_MIGRAZIONE_DATI", Session("conn")) = 1 Then
			'        msgErrore.Text = "Si è verificato un errore durante l'aggiornamento dei dati. Contattare l'assistenza."
			'        Exit Sub
			'    End If
			'Else
			'    ChiudiDataReader(dtrgenerico)
			'End If

			''************************************************************
			''Modifica del 6/09/2005 di Amilcare Paolella
			''Cancello i campi CF e PI e li salvo in CFArchivio e PIArchivio
			'Dim strSqlCFPI As String = "select isnull(CodiceFiscale,'') as CodiceFiscale, isnull(PartitaIva,'') as PartitaIva from enti where idente=" & Session("idente") & ""
			'Dim dtrGen As Data.SqlClient.SqlDataReader
			'Dim strCF As String
			'Dim strPI As String
			'dtrGen = ClsServer.CreaDatareader(strSqlCFPI, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'dtrGen.Read()
			'strCF = dtrGen.GetValue(0)
			'strPI = dtrGen.GetValue(1)
			'ChiudiDataReader(dtrGen)
			'strSqlCFPI = "Update enti set CodiceFiscale= null, PartitaIva= null, " & _
			'             " CodiceFiscaleArchivio= '" & strCF.ToString & "'," & _
			'             " PartitaIvaArchivio= '" & strPI.ToString & "'" & _
			'             " where idente= " & Session("idente")
			'Dim CmdCFPI As New Data.SqlClient.SqlCommand(strSqlCFPI, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'CmdCFPI.ExecuteNonQuery()
			'CmdCFPI.Dispose()
			''************************************************************

			''Controllo Flag Forzatura Accreditamento
			''Controllo se è stato impostato il flag
			'dtrgenerico = ClsServer.CreaDatareader("Select IsNull(FlagForzaturaAccreditamento,0) From Enti Where IdEnte = " & Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'dtrgenerico.Read()
			'If dtrgenerico.Item(0) = True Then
			'    ChiudiDataReader(dtrgenerico)
			'    'Imposto a zero il bit di Flag Forzatura Accreditamento
			'    myQuerySql = "UpDate Enti Set FlagForzaturaAccreditamento = 0 Where IdEnte = " & Session("IdEnte")
			'    CmdCFPI = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'    CmdCFPI.ExecuteNonQuery()

			'    'Inserimento della cronologia di chiusura dell'apertura accreditamento
			'    myQuerySql = "Insert Into CronologiaApertureAccreditamento (IdEnte,Data,Utente,TipoOperazione) Values (" & Session("IdEnte") & ",GetDate(),'" & Session("Utente") & "',2)"
			'    CmdCFPI = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'    CmdCFPI.ExecuteNonQuery()
			'    CmdCFPI.Dispose()
			'Else
			'    ChiudiDataReader(dtrgenerico)
			'End If

			''************************************************************
			'Dim bytForzaturaEnte As Byte

			''***Eliminazione Record su tabella per ripristino situazione precedente a presentazione
			'Dim sqlcommand As Data.SqlClient.SqlCommand
			'sqlcommand = New SqlClient.SqlCommand("delete from SospensioneAlbero where idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'sqlcommand.ExecuteNonQuery()
			'sqlcommand.Dispose()

			'dtrgenerico = ClsServer.CreaDatareader("select a.idente," & _
			'" a.idstatoente, b.presentazioneprogetti,a.forzatura, b.sospeso from enti a" & _
			'" inner join statienti b" & _
			'" on a.idstatoente=b.idstatoente" & _
			'" where b.Sospeso<>1 and a.idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'Do While dtrgenerico.Read()
			'    If dtrgenerico.GetValue(2) = True Or dtrgenerico.GetValue(4) = False Then 'se non è già disaccreditato 
			'        intstato = dtrgenerico.GetValue(1)
			'        bytForzaturaEnte = dtrgenerico.GetValue(3)
			'        ChiudiDataReader(dtrgenerico)
			'        'proseguo andando a modificare tabella enti
			'        Dim strappo As String = "update enti set dataDeterminazioneNegativa=getdate()," & _
			'        " idstatoente=(select idstatoente from statienti where sospeso=1),ClasseAccreditamentoSubIudice=0,IdRegioneAppartenenza=dbo.RegioneAppartenenza(" & Session("idente") & "),IdRegioneCompetenza=dbo.RegioneCompetenza(" & Session("idente") & ") " & _
			'        " where idente=" & Session("idente") & ""
			'        Dim cmdente As New Data.SqlClient.SqlCommand(strappo, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'        cmdente.ExecuteNonQuery()
			'        cmdente.Dispose()
			'        'proseguo andando a popolare la tabella della cronologia (storic
			'        Dim strquery As String = "insert into" & _
			'         " cronologiaentistati(idente,idstatoente,datacronologia,usernameaccreditatore," & _
			'         " idclasseaccreditamento,idtipocronologia,forzatura) select" & _
			'         " " & Session("IdEnte") & "," & intstato & ",getdate(),'" & ClsServer.NoApice(Session("Utente")) & "'," & _
			'         " (select idclasseaccreditamento from enti where idente=" & Session("IdEnte") & "),0," & bytForzaturaEnte & "" & _
			'         " from statienti where Sospeso=1"
			'        Dim CmdInsCronologiaEnti As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'        CmdInsCronologiaEnti.ExecuteNonQuery()
			'        CmdInsCronologiaEnti.Dispose()


			'        'Aggiunto da Alessandra Taballione il 14/11/2005
			'        'Anche Se sono un Ente Padre Cancello sedi e sedi di attuazione
			'        'sedi 
			'        strquery = " Update entisedi set idstatoentesede=2 " & _
			'        " from entisedi " & _
			'        " where idente = " & Session("idEnte") & ""
			'        Dim CmdEliminaSedi As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'        CmdEliminaSedi.ExecuteNonQuery()
			'        CmdEliminaSedi.Dispose()

			'        'Sedi Attuazione
			'        strquery = " Update entisediattuazioni set idstatoentesede=2 " & _
			'        " from entisediattuazioni " & _
			'        " where identesede in " & _
			'        " (select identesede from entisedi " & _
			'        " where idente =" & Session("IdEnte") & ")"
			'        Dim CmdEliminaSediAttuazione As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'        CmdEliminaSediAttuazione.ExecuteNonQuery()
			'        CmdEliminaSediAttuazione.Dispose()


			'        'Aggiunto da Alessandra Taballione il 11/07/2005
			'        'Se sono un Ente figlio Cancello sedi e sedi di attuazione
			'        'sedi 
			'        strquery = " Update entisedi set idstatoentesede=2 " & _
			'        " from entisedi " & _
			'        " where idente in (select identefiglio from entirelazioni where identeFiglio=" & Session("idEnte") & ")"
			'        Dim CmdEliminaSediProprie As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'        CmdEliminaSediProprie.ExecuteNonQuery()
			'        CmdEliminaSediProprie.Dispose()

			'        'Sedi Attuazione
			'        strquery = " Update entisediattuazioni set idstatoentesede=2 " & _
			'        " from entisediattuazioni " & _
			'        " where identesede in " & _
			'        " (select identesede from entisedi " & _
			'        " where idente in (select identefiglio from entirelazioni where identefiglio=" & Session("IdEnte") & "))"
			'        Dim CmdEliminaSediAttuazioneProprie As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'        CmdEliminaSediAttuazioneProprie.ExecuteNonQuery()
			'        CmdEliminaSediAttuazioneProprie.Dispose()
			'        msgConferma.Text = "L'ente è stato chiuso."
			'        Exit Sub
			'    End If
			'Loop
			'ChiudiDataReader(dtrgenerico)
		ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'gestione accreditamento ruolo    'disaccredito ruolo
			Dim intidentepersonaleruolo As Int64
			Dim bytForzaturaRuolo As Byte
			Dim dtrgenerico As Data.SqlClient.SqlDataReader
			'verifico se è possibile disaccreditare verificando solamente se sia già disaccreditato
			dtrgenerico = ClsServer.CreaDatareader("select identepersonaleruolo," &
			" accreditato,forzatura from entepersonaleruoli where" &
			" idruolo=" & Request.QueryString("IDRuolo") & " and" &
			" identepersonale=" & Request.QueryString("IDEntePersonale") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			If dtrgenerico.HasRows = True Then
				dtrgenerico.Read()
				If dtrgenerico.GetValue(1) = -1 Then
					ChiudiDataReader(dtrgenerico)
					Exit Sub
				Else
					intidentepersonaleruolo = dtrgenerico.GetValue(0)
					bytForzaturaRuolo = dtrgenerico.GetValue(2)
					ChiudiDataReader(dtrgenerico)
				End If
				Dim cmdInsCronologia As Data.SqlClient.SqlCommand
				cmdInsCronologia = New SqlClient.SqlCommand("insert into cronologiaentepersonaleruoli" &
				" (identepersonaleruolo,accreditato,datacronologia,idtipocronologia," &
				" usernameaccreditatore,forzatura)" &
				" select identepersonaleruolo,accreditato,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'," & bytForzaturaRuolo & "" &
				" from entepersonaleruoli where identepersonaleruolo=" & intidentepersonaleruolo & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				cmdInsCronologia.ExecuteNonQuery()
				cmdInsCronologia.Dispose()
				Dim ComUpdRuoli As Data.SqlClient.SqlCommand 'eseguo comando di modifica
				ComUpdRuoli = New SqlClient.SqlCommand("update entepersonaleruoli set accreditato=-1,dataaccreditamento=getdate(),usernameaccreditatore='" & ClsServer.NoApice(Session("Utente")) & "'" &
				" where idruolo=" & Request.QueryString("IDRuolo") & " and identepersonale=" & Request.QueryString("IDEntePersonale") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				ComUpdRuoli.ExecuteNonQuery()
				ComUpdRuoli.Dispose()
                msgConferma.Text = "ISCRIZIONE ANNULLATA."

			Else 'se non è possibile accreditare chiudo solamente dataREader
				dtrgenerico.Close()
				dtrgenerico = Nothing
			End If
		ElseIf Request.QueryString("tipologia") = "Progetti" Then 'accredito progetti
			Dim dtrgenerico As Data.SqlClient.SqlDataReader
			'*********controllo se l'attività è in graduatoria ed è stata confermata
			dtrgenerico = ClsServer.CreaDatareader("select idgraduatoriaprogetto" &
			" from graduatorieprogetti where statograduatoria=1" &
			" and idattività=" & CInt(Request.QueryString("idattivita")) & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			If dtrgenerico.HasRows = True Then
                msgErrore.Text = "Il progetto è già in graduatoria. Impossibile annullare l'iscrizione"
				ChiudiDataReader(dtrgenerico)
				Exit Sub
			End If
			ChiudiDataReader(dtrgenerico)
			'prima di lanciare form di gestione verifico accreditamento 
			dtrgenerico = ClsServer.CreaDatareader("select idattività from attività a" &
			" inner join statiattività b on a.idstatoattività=b.idstatoattività" &
			" where a.idattività=" & Request.QueryString("idattivita") & "" &
			" and (b.Dagraduare=1 or b.davalutare=1)", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			'solo se il progetto risulta accreditato lancio form
			If dtrgenerico.HasRows = True Then
				ChiudiDataReader(dtrgenerico)
				Response.Redirect("WfrmDisAccrProgetto.aspx?Tipoatt=" & Request.QueryString("idattivita") & "")
			Else
				'ImgAlert.Visible = True
				msgErrore.Text = "Impossibile valutare il progetto prima dell'accettazione dell'Istanza di Presentazione!"
				ChiudiDataReader(dtrgenerico)
			End If
		End If
	End Sub

	Private Sub ImgPresentazioneDomanda_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgPresentazioneDomanda.Click
		Dim prova As Byte()
		CancellaMessaggiInfo()
		Dim strRitornoStore As String
		strRitornoStore = LeggiStoreVerificaCompletamentoAccreditamento(Session("IdEnte"))
		If strRitornoStore <> "" Then
			msgErrore.Text = strRitornoStore
			Exit Sub
		End If
		Session("LoadedRiepilogo") = Nothing
		Session("LoadedDesignazione") = Nothing
		If Session("IdStatoEnte") = 6 Then
			Response.Redirect("WfrmIscrizione.aspx")
		Else
			Response.Redirect("WfrmAdeguamento.aspx")
		End If


	End Sub

	Private Sub btnPresentaDomanda_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnPresentaDomanda.Click
		''***** Aggiunto da Simona Cordella il 16/06/2009
		''Richiamo Store che verifica se l'ente ha completato tutte le operazioni necessarie per procedere all'acreditamento/adeguamento
		ImgPresentazioneDomanda.Visible = False
		CancellaMessaggiInfo()

		Dim strRitornoStore As String
		strRitornoStore = LeggiStoreVerificaCompletamentoAccreditamento(Session("IdEnte"))
		If strRitornoStore <> "" Then
			msgErrore.Text = strRitornoStore
			Exit Sub
		End If

		'Aggiunto da Alessandra Taballione il 22/04/2005
		'eseguo la query per leggere di che tipologia di classe si tratta
		'CodiceRegione ***********************************
		Dim VstrWhereCondition() As String
		Dim strCodiceGenerato As String
		Dim MyTransaction As System.Data.SqlClient.SqlTransaction 'Transazione che utilizzo per accaertarmi che tutte le operazioni vadano a buon fine
		Dim CmdAggiornaDB As Data.SqlClient.SqlCommand 'Commadn locale che utlizzo per aggiornare i dati nella database
		Dim BlnCodicePresente As Boolean = False
		Try
			'Inizializzo la tranzazione assegnandole un nome univoco
			MyTransaction = CType(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), System.Data.SqlClient.SqlConnection).BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))

			'ridimensiono il vettore delle condizioni a zero
			ReDim VstrWhereCondition(0)

			VstrWhereCondition(0) = ""

			'Eseguo la query per leggere di che tipologia di classe si tratta
			myQuerySql = "Select IsNull(CodiceRegione,'') as CodiceRegione,IdClasseAccreditamentoRichiesta From Enti Where IdEnte = " & Session("IdEnte")
			ChiudiDataReader(myDataReader)
			myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
			myDataReader.Read()

			'Generazione del codice NZ nel caso in cui l'ente non abbia il codice NZ
			If myDataReader("CodiceRegione") = "" Then
				'Associazione della classe e del nuovo codice NZ
				Dim StrCodiceRegione As String
				StrCodiceRegione = CodiceGenerato(CInt(myDataReader("idclasseaccreditamentoRichiesta")), MyTransaction)
				ChiudiDataReader(myDataReader)

				'If StrCodiceRegione = "NZ" Then
				'    txtCodiceRegione.Value = "NZ00001"
				'Else
				'    txtCodiceRegione.Value = StrCodiceRegione
				'End If
				If StrCodiceRegione = "SU" Then
					txtCodiceRegione.Value = "SU00001"
				Else
					txtCodiceRegione.Value = StrCodiceRegione
				End If
			Else
				BlnCodicePresente = True
				txtCodiceRegione.Value = myDataReader("CodiceRegione")
				myDataReader.Close()
				myDataReader = Nothing
			End If

			'Controllo se lo stato dell'ente risultava chiuso
			myQuerySql = "Select StatiEnti.PresentazioneProgetti,StatiEnti.DefaultStato,StatiEnti.Chiuso,StatiEnti.Sospeso,StatiEnti.Istruttoria From Enti " &
					 "INNER JOIN StatiEnti ON StatiEnti.IdStatoEnte = Enti.IdStatoEnte " &
					 "Where IdEnte = " & Session("IdEnte")
			ChiudiDataReader(myDataReader)
			myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
			myDataReader.Read()

			'Modifica dello stato dell'ente
			Dim StrCronologia As String = ""
			myQuerySql = "UpDate Enti Set IdRegioneAppartenenza = dbo.RegioneAppartenenza(" & Session("IdEnte") & "),IdRegioneCompetenza = dbo.RegioneCompetenza(" & Session("IdEnte") & "),CodiceRegione = '" & ClsServer.NoApice(txtCodiceRegione.Value) & "'"
			If myDataReader.Item("PresentazioneProgetti") = True And myDataReader.Item("DefaultStato") = False And
			myDataReader.Item("Chiuso") = False And myDataReader.Item("Sospeso") = False And myDataReader.Item("Istruttoria") = False Then 'Imposto lo stato "In Adeguamento" se risulta "Attivo"
				'Mette lo stato di ADEGUAMENTO
				myQuerySql = myQuerySql & ",DataUltimaRichiestaAdeguamento=GetDate(),IdStatoEnte = (Select IdStatoEnte From StatiEnti Where Istruttoria=1 And statoente <> 'Istruttoria') "
				'Inserimento nella cronologia dell'ente della modifica dello stato
				StrCronologia = "Insert Into CronologiaEntiStati(IdEnte,IdStatoEnte,DataCronologia,IdTipoCronologia,UserNameAccreditatore,Forzatura)  select idente,idstatoente,GetDate(),0,'" & ClsServer.NoApice(Session("Utente")) & "',0 from enti where idente=" & Session("idEnte")
			ElseIf myDataReader.Item("PresentazioneProgetti") = False And myDataReader.Item("DefaultStato") = False And
			myDataReader.Item("Chiuso") = False And myDataReader.Item("Istruttoria") = False Then                         'Imposto lo stato "Istruttoria" se risulta "Registrato" o "Chiuso"
				'Mette lo stato di ISTRUTTORIA
				myQuerySql = myQuerySql & ",DataRichiestaAccreditamento=GetDate(),IdStatoEnte = (Select IdStatoEnte From StatiEnti Where Istruttoria = 1 And StatoEnte = 'Istruttoria') "
				'Inserimento nella cronologia dell'ente della modifica dello stato
				StrCronologia = "Insert Into CronologiaEntiStati(IdEnte,IdStatoEnte,DataCronologia,IdTipoCronologia,UserNameAccreditatore,Forzatura)  select idente,idstatoente,GetDate(),0,'" & ClsServer.NoApice(Session("Utente")) & "',0 from enti where idente=" & Session("idEnte")
			End If
			myQuerySql = myQuerySql & " Where IdEnte = " & Session("IdEnte")
			myDataReader.Close()

			'Inserimento dei dati nella base dati
			If StrCronologia.Length <> 0 Then
				CmdAggiornaDB = New SqlClient.SqlCommand(StrCronologia, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
				CmdAggiornaDB.ExecuteNonQuery()
			End If
			CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
			CmdAggiornaDB.ExecuteNonQuery()

			'vado a ciclarmi gli enti figlio e fado a generare il loro codice regione, lo metto nell'array per poi scaricare le operazioni nel db
			myQuerySql = "Select EntiRelazioni.IdEnteFiglio From EntiRelazioni INNER JOIN Enti ON EntiRelazioni.IdEnteFiglio = Enti.IdEnte Where IdEntePadre = " & Session("IdEnte") & " And IsNull(Enti.CodiceRegione,'') = ''"
			myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
			While myDataReader.Read
				If VstrWhereCondition(0) = "" Then
					VstrWhereCondition(0) = "where idente=" & CInt(myDataReader("IdEnteFiglio"))
				Else
					ReDim Preserve VstrWhereCondition(UBound(VstrWhereCondition) + 1)
					VstrWhereCondition(UBound(VstrWhereCondition)) = "where idente=" & CInt(myDataReader("identefiglio"))
				End If
			End While
			ChiudiDataReader(myDataReader)
			'controllo se ci sono codici regione da scaricare sugli enti figlio
			If VstrWhereCondition(0) <> "" Then
				'ciclo tutti gli enti figlio
				Dim IntX As Integer
				For IntX = 0 To UBound(VstrWhereCondition)
					'Genero il codice che devo associare all'ente figlio
					myQuerySql = "SELECT MAX(Enti.CodiceRegione) + CHAR(LEFT(MAX(Convert(Varchar(5),ASCII(SUBSTRING " &
							 "(enti_1.CodiceRegione,8,1))) + Convert(varchar(5),right(enti_1.CodiceRegione,2))) + 1, " &
							 "LEN(MAX(Convert(varchar(5),ascii(SUBSTRING ( enti_1.CodiceRegione , 8 , 1 ))) + " &
							 "Convert(Varchar(5),right(enti_1.CodiceRegione,2))) + 1) - 2)) + right(max(convert(varchar(5), " &
							 "ASCII(SUBSTRING(enti_1.CodiceRegione,8,1)))+convert(varchar(5),right(enti_1.CodiceRegione,2))) + 1,2) as CodiceGenerato " &
							 "FROM Enti " &
							 "INNER JOIN entirelazioni ON enti.IDEnte = entirelazioni.IDEntePadre " &
							 "INNER JOIN enti enti_1 ON entirelazioni.IDEnteFiglio = enti_1.IDEnte " &
							 "WHERE (enti.idente=" & CInt(Session("IdEnte")) & ")"

					myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
					myDataReader.Read()
					If Not IsDBNull(myDataReader("CodiceGenerato")) Then
						'codice generato
						strCodiceGenerato = myDataReader("CodiceGenerato")
					Else
						'se restituisce null è il primo quindi forzo il primo codice generato in A00 + il codice del padre
						strCodiceGenerato = txtCodiceRegione.Value & "A00"
					End If
					ChiudiDataReader(myDataReader)
					'Eseguo la query di aggiornamento dell'ente figlio
					myQuerySql = "UpDate Enti Set CodiceRegione = '" & strCodiceGenerato & "' " & VstrWhereCondition(IntX)
					CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
					CmdAggiornaDB.ExecuteNonQuery()
				Next
			End If

			'Eseguo la query di aggiornamento competenze degli enti figlio
			myQuerySql = "UpDate Enti Set idregioneappartenenza = c.idregioneappartenenza, idregionecompetenza = c.idregionecompetenza from enti inner join entirelazioni b on idente = b.identefiglio inner join enti c on c.idente = b.identepadre where b.identepadre = " & CInt(Session("IdEnte")) & ""
			CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
			CmdAggiornaDB.ExecuteNonQuery()

			'Controllo se è stato impostato il flag
			myDataReader = ClsServer.CreaDatareader("Select IsNull(FlagForzaturaAccreditamento,0) From Enti Where IdEnte = " & Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
			myDataReader.Read()
			If myDataReader.Item(0) = True Then
				myDataReader.Close()

				'Imposto a zero il bit di Flag Forzatura Accreditamento
				myQuerySql = "UpDate Enti Set FlagForzaturaAccreditamento = 0 Where IdEnte = " & Session("IdEnte")
				CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
				CmdAggiornaDB.ExecuteNonQuery()

				'Inserimento della cronologia di chiusura dell'apertura accreditamento
				myQuerySql = "Insert Into CronologiaApertureAccreditamento (IdEnte,Data,Utente,TipoOperazione) Values (" & Session("IdEnte") & ",GetDate(),'" & Session("Utente") & "',2)"
				CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
				CmdAggiornaDB.ExecuteNonQuery()
			Else
				myDataReader.Close()
			End If
			If (Session("IdStatoEnte") = 6 Or Session("IdStatoEnte") = 8) Then
				'RISULTATO STORE PROCEDURE DI MIGRAZIONE DATI: False=OK  True=ERR
				If StoreAggiornDB(Session("IdEnte"), MyTransaction) = True Then
					MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
					Exit Sub
				End If
			End If
			MyTransaction.Commit()

			'Aggiorno la Session Contenente lo stato dell'ente
			myQuerySql = "Select IdStatoEnte From Enti Where IdEnte = " & Session("IdEnte")
			myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			myDataReader.Read()
			Session("IdStatoEnte") = myDataReader.Item("IdStatoEnte")
			myDataReader.Close()
			myDataReader = Nothing

			'aggiorno lo stato della session della forzatura in sessione (parametro per la connessione al db snapshot)
			Session("FlagForzatura") = False
		Catch ex As Exception
			'ROLLBACK
			MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
			Exit Sub
		End Try
		Dim intIDEnteFase As Integer
		intIDEnteFase = RicavaIdEnteFase(Session("IdEnte"))
		If Request.QueryString("tipologia") = "Enti" Then
			'ChiudiFaseEnte(intIDEnteFase)
			Dim strMg As String
			Dim Esito As Integer
			'CHIAMO LO STORED
			ClsServer.ChiudiFaseEnte(intIDEnteFase, Session("Utente"), Session("conn"), strMg, Esito)

			Select Case Esito
				Case 0 'ERRORE
					msgErrore.Text = strMg
					Exit Sub
				Case 1 'GENERAZIONE COPERTINA PRESENTAZIONE SENZA BOXSEDI
					StampaCopertina(intIDEnteFase)
					Response.Redirect("WfrmAlbero.aspx?tipologia=Enti&Presenta=1&IDEnteFase=" & intIDEnteFase & "&CodiceRegione='" & txtCodiceRegione.Value & "'")
				Case 2 'GENERAZIONE DEL BOXSEDI (RICHIAMO WSDOCUMENTAZIONE)
					'modificato il 02/01/2018 da s.c.
					'chiamo WfrmInfoPresentazioneAccreditamento 
					GenerazioneAllegato6_ElencoSedi(intIDEnteFase)
					Response.Redirect("WfrmInfoPresentazioneAccreditamento.aspx?IDEnteFase=" & intIDEnteFase & "")
			End Select




			'If Esito = 0 Then
			'    msgErrore.Text = strMg
			'    Exit Sub
			'Else
			'    'modificato il 02/01/2018 da s.c.
			'    'chiamo WfrmInfoPresentazioneAccreditamento 
			'    GenerazioneAllegato6_ElencoSedi(intIDEnteFase)
			'    Response.Redirect("WfrmInfoPresentazioneAccreditamento.aspx?IDEnteFase=" & intIDEnteFase & "")

			'    'StampaCopertina(intIDEnteFase)
			'    'Response.Redirect("WfrmAlbero.aspx?tipologia=Enti&Presenta=1&CodiceRegione='" & txtCodiceRegione.Value & "'")
			'End If
		End If
	End Sub
	Private Sub GenerazioneAllegato6_ElencoSedi(ByVal IdEnteFase As Integer)

		Dim localWS As New WS_Editor.WSMetodiDocumentazione
		Dim ds As DataSet
		Dim i As Integer
		Dim strCodiceProgetto As String
		Dim ResultAsinc As IAsyncResult

		Dim cmdUp As SqlCommand

		'update InLavorazione a 1 
		'aggiunto flag LAVORAZIONE =1 da simona cordella il 02/01/2018
		cmdUp = New Data.SqlClient.SqlCommand(" UPDATE EntiFasi" &
											  " SET InLavorazione= 1  " &
											  " WHERE IdEnteFase=" & IdEnteFase & "", Session("conn"))
		cmdUp.ExecuteNonQuery()
		cmdUp.Dispose()



		'richiamo WSDocumentazione
		localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")
		localWS.Timeout = 1000000




		ResultAsinc = localWS.BeginGenerazioneAllegato6_ElencoSedi(IdEnteFase, Session("Utente"), Nothing, "")



	End Sub
	'store che migra i dati nel DB SnapShot da unsc_ lettura
	Private Function StoreAggiornDB(ByVal IdEnte As Long, ByVal Transazione As System.Data.SqlClient.SqlTransaction) As Boolean
		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_MIGRAZIONE_DATI"
		CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
		CustOrderHist.Transaction = Transazione

		Dim paramEnte As SqlClient.SqlParameter
		paramEnte = New SqlClient.SqlParameter
		paramEnte.ParameterName = "@IdEnte"
		paramEnte.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(paramEnte)

		Dim paramValore As SqlClient.SqlParameter
		paramValore = New SqlClient.SqlParameter
		paramValore.ParameterName = "@Valore"
		paramValore.SqlDbType = SqlDbType.Bit
		paramValore.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(paramValore)

		Dim Reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IdEnte
		Reader = CustOrderHist.ExecuteReader()
		StoreAggiornDB = CustOrderHist.Parameters("@Valore").Value
		ChiudiDataReader(Reader)
	End Function

	'creata da jbagnani il 05.01.2005
	Function CodiceGenerato(ByVal idclasseselezionata As Integer, Optional ByVal Transazione As System.Data.SqlClient.SqlTransaction = Nothing) As String
		Dim strLocal As String
		Dim strDescrizioneClasse As String
		Dim intIdClasse As Integer
		Dim strNuovoCodice As String

		strLocal = "select IDClasseAccreditamento, classeaccreditamento from classiaccreditamento where idclasseaccreditamento=" & idclasseselezionata
		ChiudiDataReader(myDataReader)
		'eseguo la query per leggere di che tipologia di classe si tratta
		myDataReader = ClsServer.CreaDatareader(strLocal, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , Transazione)

		'controllo quale classe ho appena selezionato
		If myDataReader.HasRows Then
			myDataReader.Read()
			strDescrizioneClasse = IIf(IsDBNull(myDataReader("classeaccreditamento")) = True, "", myDataReader("classeaccreditamento"))
			intIdClasse = myDataReader("IDClasseAccreditamento")
		End If

		ChiudiDataReader(myDataReader)
		'*******************  VECCHIA CREAZIONE CODICE REGION NZ per gli encti SCN**************************
		''preparo la stringa con il codice max + 1
		'strLocal = "select replicate('0', len(max(CodiceRegione)) - 2 - len((max(right(CodiceRegione,len(CodiceRegione) - 2)) + 1))) + convert(varchar,max(right(CodiceRegione,len(CodiceRegione) - 2)) + 1) as MAXX from enti where len(codiceregione)=7"

		''eseguo la query per creare il max codice regione
		'myDataReader = ClsServer.CreaDatareader(strLocal, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , Transazione)

		''controllo quale classe ho appena selezionato
		'If myDataReader.HasRows Then
		'    myDataReader.Read()
		'    strNuovoCodice = "NZ" & myDataReader("MAXX")
		'End If
		''e di conseguenza creo la stringa per inserire il codiceregione(codente)
		'Select Case strDescrizioneClasse
		'    Case "Classe 1"
		'        CodiceGenerato = strNuovoCodice
		'    Case "Classe 2"
		'        CodiceGenerato = strNuovoCodice
		'    Case "Classe 3"
		'        CodiceGenerato = strNuovoCodice
		'    Case "Classe 4"
		'        CodiceGenerato = strNuovoCodice
		'    Case Else
		'        CodiceGenerato = "null"
		'End Select
		'******************* fine  VECCHIA CREAZIONE CODICE REGION NZ per gli encti SCN**************************


		'*******************  NUOVA CREAZIONE CODICE REGIONE SU per gli encti SCU **************************
		'creato da simona cordella il 25/07/2017 GENERO IL NUVO CODICE PER GLI ENTI SCU 

		strLocal = "SELECT ISNULL(replicate('0', len(max(CodiceRegione)) - 2 - len((max(right(CodiceRegione,len(CodiceRegione) - 2)) + 1))) + convert(varchar,max(right(CodiceRegione,len(CodiceRegione) - 2)) + 1),'00001') as MAXX from enti where len(codiceregione)=7 AND LEFT(CodiceRegione,2)='SU'"

		'eseguo la query per creare il max codice regione
		myDataReader = ClsServer.CreaDatareader(strLocal, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , Transazione)

		If myDataReader.HasRows Then
			myDataReader.Read()
			strNuovoCodice = "SU" & myDataReader("MAXX")
		End If

		' creo la stringa per inserire il codiceregione(codente)
		Select Case intIdClasse
			Case 1 To 4 'CLASSI DA 1 A 4 
				CodiceGenerato = strNuovoCodice
			Case 8, 9 'SCU - Albo Generale E SCU - Sezione RPA
				CodiceGenerato = strNuovoCodice
			Case Else 'Enti Vincoli Associativi, Consortili, Federativi, Canonico-Pastorali -   Nessuna(Classe) - Enti In Partenariato
				CodiceGenerato = "null"
		End Select
		'******************* FINE  NUOVA CREAZIONE CODICE REGIONE SU per gli encti SCU **************************
		ChiudiDataReader(myDataReader)
	End Function

	Private Function ControllaAttivitàSede() As Boolean
		myQuerySql = "select distinct attivitàEntiSediAttuazione.identesedeattuazione from attivitàEntiSediAttuazione " &
		" inner join entisediattuazioni on entisediattuazioni.identesedeattuazione=attivitàEntiSediAttuazione.identesedeattuazione " &
		" inner join attività on (attivitàEntiSediAttuazione.idattività=attività.idattività)" &
		" inner join statiattività on (attività.idstatoattività=statiattività.idstatoattività)" &
		" where attività.identepresentante=" & Session("IdEnte") & "  and (statiattività.chiusa=0 and  statiattività.cancellata=0 and  statiattività.defaultstato=0)"
		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If myDataReader.HasRows = True Then
			ControllaAttivitàSede = False
		Else
			ControllaAttivitàSede = True
		End If
		ChiudiDataReader(myDataReader)
	End Function

	Private Function ControllaCoProgettazione() As Boolean
		'Verifica se l'ente è in CoProgettazione con altri enti
		myQuerySql = "SELECT distinct attività.* " &
		" FROM  AttivitàEntiCoprogettazione  " &
		" INNER JOIN attività ON AttivitàEntiCoprogettazione.IdAttività = attività.IDAttività " &
		" INNER JOIN statiattività ON attività.IDStatoAttività = statiattività.IDStatoAttività" &
		" WHERE AttivitàEntiCoprogettazione.IdEnte = " & Session("IdEnte") & "  " &
		" and statiattività.chiusa=0 and  statiattività.cancellata=0 and  statiattività.defaultstato=0 "

		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If myDataReader.HasRows = True Then
			ControllaCoProgettazione = False
		Else
			ControllaCoProgettazione = True
		End If
		ChiudiDataReader(myDataReader)
	End Function

	'Funzione per il controllo dell'esistenza di servizi acquisiti da valutare (Ritorna TRUE se esistono ancora servizi da valutare)
	Private Function ControlloServizi() As Boolean
		myQuerySql = "SELECT * FROM EntiAcquisizioneServizi WHERE idEnteSecondario = " & Session("IdEnte") & " " &
				 "AND (StatoRichiesta = 0)"
		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		ControlloServizi = myDataReader.HasRows
		ChiudiDataReader(myDataReader)
	End Function

	Private Function LeggiStoreVerificaCompletamentoAccreditamento(ByVal IDEnte As Integer) As String
		'Agg. da Simona Cordella il 16/06/2009
		'richiamo store che verifca se l'ente ha completato tutti gli inserimeni e gli aggiornamenti necessari per effettuare la presentazione della domanda di accrditamento /adeguamento
		Dim intValore As Integer

		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_VERIFICA_COMPLETAMENTO_ACCREDITAMENTO"
		CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(sparam)


		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Esito"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam1)

		Dim sparam2 As SqlClient.SqlParameter
		sparam2 = New SqlClient.SqlParameter
		sparam2.ParameterName = "@Motivazione"
		sparam2.SqlDbType = SqlDbType.VarChar
        sparam2.Size = -1
		sparam2.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam2)

		Dim reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IDEnte
		CustOrderHist.ExecuteScalar()
		intValore = CustOrderHist.Parameters("@Esito").Value
		LeggiStoreVerificaCompletamentoAccreditamento = CustOrderHist.Parameters("@Motivazione").Value
		ChiudiDataReader(reader)
	End Function
	Private Function LeggiStoreVerificaValutazioneAccreditamento(ByVal IDEnte As Integer) As String
		'Agg. da Simona Cordella il 16/06/2009
		'richiamo store che verifca se l'ente ha completato tutti gli inserimeni e gli aggiornamenti necessari per effettuare la presentazione della domanda di accrditamento /adeguamento
		Dim intValore As Integer

		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_VERIFICA_VALUTAZIONE_ACCREDITAMENTO"
		CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(sparam)


		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Esito"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam1)

		Dim sparam2 As SqlClient.SqlParameter
		sparam2 = New SqlClient.SqlParameter
		sparam2.ParameterName = "@Motivazione"
		sparam2.SqlDbType = SqlDbType.VarChar
		sparam2.Size = 1000
		sparam2.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam2)

		Dim reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IDEnte
		CustOrderHist.ExecuteScalar()
		'CustOrderHist.ExecuteNonQuery()
		intValore = CustOrderHist.Parameters("@Esito").Value
		LeggiStoreVerificaValutazioneAccreditamento = CustOrderHist.Parameters("@Motivazione").Value

		ChiudiDataReader(reader)
	End Function
	Private Function ControlloStatoEnteChiuso(ByVal IdEnte As Integer) As Boolean
		Dim dtrStato As SqlClient.SqlDataReader
		Dim strSql As String
		Dim enteChiuso As Boolean = False
		strSql = "Select Idente From Enti where idente =" & IdEnte & " and IdStatoEnte =7"
		dtrStato = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If dtrStato.HasRows = True Then
			enteChiuso = True
		End If
		ChiudiDataReader(dtrStato)
		Return enteChiuso
	End Function

	Private Sub ImgAbilitaAdeguamento_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgAbilitaAdeguamento.Click
		'ANTONELLO Parametrizzare il tipo di fase da passare ?????? come faccio a sapere se sto facendo articolo 2 articolo 10 adeguamento o accreditamento?
		Dim CmdAggiornaDB As Data.SqlClient.SqlCommand
		Dim Valore As Integer
		Dim dtrFase As SqlClient.SqlDataReader
		Dim strSql As String
		CancellaMessaggiInfo()
		'lblMessaggio.Text = ""
		ChiudiDataReader(myDataReader)

		strSql = "SELECT * FROM ENTIFASI WHERE IDENTE = " & Session("IdEnte") & " and getdate() between datainiziofase and datafinefase and TipoFase = 2"
		dtrFase = ClsServer.CreaDatareader(strSql, Session("conn"))
		If dtrFase.HasRows = False Then
			ChiudiDataReader(dtrFase)

			myQuerySql = "select valore from configurazioni where parametro = 'DURATA_ADEG'"
			myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
			myDataReader.Read()
			Valore = myDataReader("valore")
			ChiudiDataReader(myDataReader)
			myQuerySql = "Update Enti Set FlagForzaturaAccreditamento= 1 where IdEnte= " & Session("IdEnte") & ""
			CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdAggiornaDB.ExecuteNonQuery()

			myQuerySql = "Insert Into EntiFasi (IdEnte,TipoFase,DataInizioFase,DataFineFase,Stato,UserNameInizioFase) Values (" & Session("IdEnte") & ",2,GetDate(), DATEADD(S,-1,DATEADD(D,convert(int," & Valore & ")+1,DBO.FORMATODATADT(GetDate()))),1,'" & Session("Utente") & "')"
			CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdAggiornaDB.ExecuteNonQuery()
            imgElencoDocumentiEnti.Visible = False
			ImgAbilitaAdeguamento.Visible = False
			AbilitaPulsantiFunzione(Session("IdEnte"))
			LblStato.Text = ENTE_ABILITATO
			LblStato.CssClass = "msgConferma"
		Else
			ChiudiDataReader(dtrFase)
			LblStato.Text = "Una Fase di Adeguamento è già aperta. Impossibile continuare."
			LblStato.CssClass = "msgConferma"
		End If

	End Sub


	Private Sub imgStampaCopertina_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgStampaCopertina.Click
		StampaCopertinaAccreditamentoAdeguamento()
	End Sub
	Sub StampaCopertinaAccreditamentoAdeguamento()
		Dim NumDocumenti As Integer
		ChiudiDataReader(myDataReader)
		'myQuerySql = " Select IdEnteFase from EntiFasi where idente=" & Session("IdEnte") & " order by IdEnteFase Desc "
		myQuerySql = " Select top 1 IdEnteFase from EntiFasi where idente=" & Session("IdEnte") & " AND STATO =3 and TipoFase IN (1,2) order by DataFineFase Desc  "
		myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
		myDataReader.Read()
		hf_IdEnteFase.Value = myDataReader("IdEnteFase")
		ChiudiDataReader(myDataReader)
		myQuerySql = " Select count(isnull(identedocumento,0)) as NumDocumenti from EntiDocumenti where IdEnteFase=" & hf_IdEnteFase.Value & ""

		myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
		myDataReader.Read()
		NumDocumenti = myDataReader("NumDocumenti")

		ChiudiDataReader(myDataReader)
		If NumDocumenti > 0 Then 'copertina con elenco documenti
			hf_StampaCoperinaConDocumenti.Value = "1"
		Else 'copertina senza documenti
			hf_StampaCoperinaSenzaDocumenti.Value = "1"
		End If

	End Sub

	'Sub ChiudiFaseEnte(ByVal IdEnteFase As Integer)

	'    Dim SqlCmd As New SqlClient.SqlCommand
	'    Try
	'        SqlCmd.CommandText = "SP_ACCREDITAMENTO_CHIUDI_FASE_ENTE"
	'        SqlCmd.CommandType = CommandType.StoredProcedure
	'        SqlCmd.Connection = Session("Conn")

	'        SqlCmd.Parameters.Add("@IdEnteFase", SqlDbType.Int).Value = IdEnteFase
	'        SqlCmd.Parameters.Add("@UserNameRichiesta ", SqlDbType.VarChar).Value = Session("Utente")
	'        SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
	'        SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

	'        SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
	'        SqlCmd.Parameters("@messaggio").Size = 1000
	'        SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

	'        SqlCmd.ExecuteNonQuery()
	'    Catch ex As Exception

	'    End Try
	' End Sub

	Private Function RitornaStatoEnte(ByVal IdEnte As Integer) As Integer
		Dim strSql As String
		Dim dtrgenerico As SqlClient.SqlDataReader
		If Not dtrgenerico Is Nothing Then
			dtrgenerico.Close()
			dtrgenerico = Nothing
		End If

		'Verifica menu sicurezza su funzione accredita
		strSql = "SELECT idstatoente from enti where idente = " & IdEnte
		dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

		dtrgenerico.Read()

		RitornaStatoEnte = dtrgenerico("idstatoente")

		If Not dtrgenerico Is Nothing Then
			dtrgenerico.Close()
			dtrgenerico = Nothing
		End If
		Return RitornaStatoEnte
	End Function



	Sub AbilitaPulsantiFunzione(ByVal IdEnte As Integer)
		Dim SqlCmd As New SqlClient.SqlCommand
		Try
			SqlCmd.CommandText = "SP_ACCREDITAMENTO_ACCESSOMASCHERA_ALBERO"
			SqlCmd.CommandType = CommandType.StoredProcedure
			SqlCmd.Connection = Session("Conn")

			SqlCmd.Parameters.Add("@TipoUtente ", SqlDbType.VarChar).Value = Session("TipoUtente")
			SqlCmd.Parameters.Add("@IdEnte ", SqlDbType.Int).Value = IdEnte

			SqlCmd.Parameters.Add("@paccredita", SqlDbType.TinyInt)
			SqlCmd.Parameters("@paccredita").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@paccreditnegativo", SqlDbType.TinyInt)
			SqlCmd.Parameters("@paccreditnegativo").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@ppresenta", SqlDbType.TinyInt)
			SqlCmd.Parameters("@ppresenta").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@pabilitainserimento", SqlDbType.TinyInt)
			SqlCmd.Parameters("@pabilitainserimento").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@pinizioadeguamento", SqlDbType.TinyInt)
			SqlCmd.Parameters("@pinizioadeguamento").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@ppresentadocart2", SqlDbType.TinyInt)
			SqlCmd.Parameters("@ppresentadocart2").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@ppresentadocart10", SqlDbType.TinyInt)
			SqlCmd.Parameters("@ppresentadocart10").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@pinizioart2", SqlDbType.TinyInt)
			SqlCmd.Parameters("@pinizioart2").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@pinizioart10", SqlDbType.TinyInt)
			SqlCmd.Parameters("@pinizioart10").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@pelencodocumentienti", SqlDbType.TinyInt)
			SqlCmd.Parameters("@pelencodocumentienti").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@pstampacopertina", SqlDbType.TinyInt)
			SqlCmd.Parameters("@pstampacopertina").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@pannullainizioadeguamento", SqlDbType.TinyInt)
			SqlCmd.Parameters("@pannullainizioadeguamento").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@psistemi", SqlDbType.TinyInt)
			SqlCmd.Parameters("@psistemi").Direction = ParameterDirection.Output
			SqlCmd.Parameters.Add("@pRiserva", SqlDbType.TinyInt)
			SqlCmd.Parameters("@pRiserva").Direction = ParameterDirection.Output
			'adc
			SqlCmd.Parameters.Add("@paggiornasediart2", SqlDbType.TinyInt)
			SqlCmd.Parameters("@paggiornasediart2").Direction = ParameterDirection.Output

			'SqlCmd.Parameters.Add("@pTogliRiserva", SqlDbType.TinyInt)
			'SqlCmd.Parameters("@pTogliRiserva").Direction = ParameterDirection.Output
			'adc

			SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
			SqlCmd.Parameters("@messaggio").Size = 1000
			SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

			SqlCmd.ExecuteNonQuery()

			'If SqlCmd.Parameters("@paccredita").Value = 1 Then
			'    Imgaccredita.Visible = True
			'Else
			'    Imgaccredita.Visible = False
			'End If
			If SqlCmd.Parameters("@paccredita").Value = 1 And Request.QueryString("tipologia") = "Enti" Then

				If RitornaStatoEnte(IdEnte) <> 8 Then 'accreditamento accoglienza
					Imgaccredita.Visible = True
					ImgCompleta.Visible = False
				Else 'accreditamento titolare
					Imgaccredita.Visible = False

					myDataReader = ClsServer.CreaDatareader("select idclasseaccreditamento from enti where FlagAccreditamentoCompleto = 1 AND idente=" & Session("idente") & "", Session("Conn"))

					'verifico se conferma valutazione già effettuata
					If myDataReader.HasRows Then
						msgConferma.Text = "Conferma della valutazione dell'ente effettuata. In attesa di Decreto Iscrizione."
						ChiudiDataReader(myDataReader)
						ImgCompleta.Visible = False
					Else
						ChiudiDataReader(myDataReader)
						ImgCompleta.Visible = True
					End If

				End If

			End If
			If SqlCmd.Parameters("@paccredita").Value = 0 Then
				Imgaccredita.Visible = False
				ImgCompleta.Visible = False
			End If

			If SqlCmd.Parameters("@paccreditnegativo").Value = 1 Then
				cmdChiudiEnte.Visible = True
			Else
				cmdChiudiEnte.Visible = False
			End If

            If SqlCmd.Parameters("@ppresenta").Value = 1 And Session("IdStatoEnte") <> 8 Then
                ImgPresentazioneDomanda.Visible = True
            Else
                ImgPresentazioneDomanda.Visible = False
            End If

            If SqlCmd.Parameters("@pinizioadeguamento").Value = 1 And Session("IdStatoEnte") <> 8 Then
                ImgAbilitaAdeguamento.Visible = True
            Else
                ImgAbilitaAdeguamento.Visible = False
            End If

			If SqlCmd.Parameters("@ppresentadocart2").Value = 1 Then
				ImgPresentaDocArt2.Visible = True
			Else
				ImgPresentaDocArt2.Visible = False
			End If
			'adc
			If SqlCmd.Parameters("@paggiornasediart2").Value = 1 Then
				LinkAggiornaSediArt2.Visible = True
			Else
				LinkAggiornaSediArt2.Visible = False
			End If
			If SqlCmd.Parameters("@ppresentadocart10").Value = 1 Then
				ImgPresentaDocArt10.Visible = True
			Else
				ImgPresentaDocArt10.Visible = False
			End If

			If SqlCmd.Parameters("@pinizioart2").Value = 1 Then
				ImgInizioArt2.Visible = True
			Else
				ImgInizioArt2.Visible = False
			End If

			If SqlCmd.Parameters("@pinizioart10").Value = 1 Then
				ImgInizioArt10.Visible = True
			Else
				ImgInizioArt10.Visible = False
			End If

            If SqlCmd.Parameters("@pelencodocumentienti").Value = 1 And Session("IdStatoEnte") <> 6 And Session("IdStatoEnte") <> 9 Then
                imgElencoDocumentiEnti.Visible = True
            Else
                imgElencoDocumentiEnti.Visible = False
            End If

			If SqlCmd.Parameters("@pstampacopertina").Value = 1 Then
				imgStampaCopertina.Visible = True
			Else
				imgStampaCopertina.Visible = False
			End If

			If SqlCmd.Parameters("@pannullainizioadeguamento").Value = 1 Then
				ImgAnnullaInizioAdeguamento.Visible = True
			Else
				ImgAnnullaInizioAdeguamento.Visible = False
			End If


            If SqlCmd.Parameters("@psistemi").Value = 1 And Session("IdStatoEnte") <> 6 Then
                imgAnagraficaSistemi.Visible = True
            Else
                imgAnagraficaSistemi.Visible = False
            End If
			'aggiunto il 19/12/2017 nuovo pulsante per l'indicazione della riserva (per l'accreditamento)
			If SqlCmd.Parameters("@pRiserva").Value = 1 And Request.QueryString("tipologia") = "Enti" Then
				btnRiserva.Visible = True
			Else
				btnRiserva.Visible = False
			End If
			'If SqlCmd.Parameters("@pTogliRiserva").Value = 1 Then
			'    imgTogliRiserva.Visible = True
			'Else
			'    imgTogliRiserva.Visible = False
			'End If
		Catch ex As Exception
			msgErrore.Visible = True
			msgErrore.Text = "Errore imprevisto. Contattare l'assistenza."
		End Try
	End Sub



	Private Sub Store_ACCREDITAMENTO_CONFERMA_ADEGUAMENTO_ENTE()
		Dim SqlCmd As New SqlClient.SqlCommand
		Try
			SqlCmd.CommandText = "SP_ACCREDITAMENTO_CONFERMA_ADEGUAMENTO_ENTE"
			SqlCmd.CommandType = CommandType.StoredProcedure
			SqlCmd.Connection = Session("Conn")

			SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")
			SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

			'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
			SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
			SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

			SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
			SqlCmd.Parameters("@messaggio").Size = 1000
			SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

			SqlCmd.ExecuteNonQuery()
			lblMessaggio.Text = SqlCmd.Parameters("@messaggio").Value()
		Catch ex As Exception
			lblMessaggio.Text = ex.Message

		End Try
	End Sub
	Private Sub Store_ACCREDITAMENTO_Annulla_Inzio_Adeguamento(ByVal IdEnte As Integer)
		'** Aggiunto da simona cordella
		'** Funzionalità: Annulla la richiesta di inizo asdeguamento .

		Dim SqlCmd As New SqlClient.SqlCommand
		Try
			SqlCmd.CommandText = "SP_ACCREDITAMENTO_ANNULLAINIZIOADEGUAMENTO"
			SqlCmd.CommandType = CommandType.StoredProcedure
			SqlCmd.Connection = Session("Conn")

			SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
			SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

			'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
			SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
			SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

			SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
			SqlCmd.Parameters("@messaggio").Size = 1000
			SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

			SqlCmd.ExecuteNonQuery()
			lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
		Catch ex As Exception
			lblMessaggio.Text = ex.Message
		End Try
	End Sub
	Private Sub ImgAnnullaInizioAdeguamento_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgAnnullaInizioAdeguamento.Click
		'richiamo Store  per annullare la procedura di inizio Aadeguamento
		CancellaMessaggiInfo()
		Store_ACCREDITAMENTO_Annulla_Inzio_Adeguamento(Session("IdEnte"))
		AbilitaPulsantiFunzione(Session("IdEnte"))
		LblStato.Text = ENTE_DISABILITATO
		LblStato.CssClass = "msgConferma"
	End Sub

	Private Function RicavaIdEnteFase(ByVal IdEnte As Integer) As Integer
		Dim IdEF As Integer

		myQuerySql = " Select IdEnteFase from EntiFasi where idente=" & Session("IdEnte") & "  AND STATO =1 and TipoFase IN (1,2) and GETDATE() between DataInizioFase and DataFineFase order by IdEnteFase Desc "
		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
		myDataReader.Read()
		IdEF = myDataReader("IdEnteFase")
		If Not myDataReader Is Nothing Then
			myDataReader.Close()
			myDataReader = Nothing
		End If

		Return IdEF
	End Function

	Private Sub CaricamentoAlbero(Optional ByVal IntClasse As Integer = Nothing, Optional ByVal IntPadre As Integer = 0, Optional ByVal OpLogico As String = "", Optional ByVal blnPrimo As Boolean = True, Optional ByVal bytbordo As Byte = 0, Optional ByVal BytPresentata As Byte = 0)
		'***Questa routine carica l'albero attraverso una azione ricorsiva
		'***verificando le relazioni logiche dei vincolo (dai nodi centrali e nodo padre) con le classi 
		'***di accreditamento (se ente) o per i ruoli (se personale),"ANCHE" PROGETTI.
		'***Gestione Presentazione Ente
		Dim StrOpLogica As String = "" 'variabile appoggio operatore logico
		Dim BlnSoddisfatto As Boolean 'variabile controllo operatore logico
		Dim IntInizio As Integer 'In base alla classe selezionata trovo il suo punto di appoggio


		If blnPrimo = False Then 'verifico se è il primo passaggio per ricavare nodopadre
			IntInizio = IntPadre
		Else

			If Request.QueryString("tipologia") = "Enti" Then 'controllo se enti
				Dim dataReader As System.Data.SqlClient.SqlDataReader
				'Sono entrato da una classe (Quindi inizio l'albero con la descrizione della classe)
				'Calcolo prima la descrizione della classe
				myQuerySql = "Select ClasseAccreditamento From ClassiAccreditamento WHERE IdClasseAccreditamento = " & IntClasse
				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				dataReader.Read()
				Dim nodoPadre As New TreeNode

				If bytbordo = 1 Then 'formatto il nodo padre aggiungendo una cornice se è già accreditato
					nodoPadre.Value = NODE_VALUE_FOREST_GREEN
				Else
					nodoPadre.Value = NODE_VALUE_NO_COLOR
				End If
				'classe accreditamento padre
				nodoPadre.Text = dataReader.Item("ClasseAccreditamento")
				strVerificaAccredita = dataReader.Item("ClasseAccreditamento") 'valorizzo per controllo tasto accredita
				'formatto
				ImpostaStyleClasse(nodoPadre)
				nodoPadre.Expanded = True
				'NodoPadre.ID = "pippo"
				TrwVincoli.Nodes.Add(nodoPadre)

				dataReader.Close()
				dataReader = Nothing
				'verifico se esistono relazioni
				myQuerySql = "Select IdRelazioneVincoli From RelazioneVincoli Where IdClasseAccreditamento = " & IntClasse
				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If dataReader.HasRows = False Then 'se non esistono esco
					dataReader.Close()
					dataReader = Nothing
					Exit Sub
				End If
				dataReader.Read()
				IntInizio = dataReader.Item("IdRelazioneVincoli") 'salvo inizio per ricorsiva
				dataReader.Close()
				dataReader = Nothing

				myQuerySql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," &
						 " VociMenu.IdVoceMenuPadre" &
						 " FROM VociMenu" &
						 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" &
						 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"

				'============================================================================================================================
				'====================================================30/09/2008==============================================================
				'=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
				'=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
				'============================================================================================================================
				If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
				Else
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
				End If

				myQuerySql = myQuerySql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" &
						 " WHERE VociMenu.descrizione = 'Forza Accreditamento Enti'" &
						 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"


				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If dataReader.HasRows = True Then

					If Not dataReader Is Nothing Then
						dataReader.Close()
						dataReader = Nothing
					End If
					If txtstato.Value = VALORE_TRUE And txtclasse.Value = "False" Then
						hf_AdeguamentoOk.Value = VALORE_TRUE
						Imgaccredita.Text = "Adeguamento OK"
						'12/09/2014 abilito la stampa della copertina dell'adeguamento accreditamento 
						imgStampaCopertina.Visible = True
						'rendo invisibile maschera caricamento documenti
						imgElencoDocumentiEnti.Visible = False
						If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
							'Imgaccredita.Visible = True
							cmdChiudiEnte.Visible = False
						End If
					Else
						If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
							'Imgaccredita.Visible = True
							cmdChiudiEnte.Visible = True
						End If
					End If
				End If
				If Not dataReader Is Nothing Then
					dataReader.Close()
					dataReader = Nothing
				End If

				'DtrGenerico.Close()
				'DtrGenerico = Nothing
			ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'inizio parte ruoli
				Dim dataReader As System.Data.SqlClient.SqlDataReader
				myQuerySql = "Select ruolo From ruoli WHERE Idruolo = " & idRuolo
				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'mi ricavo il ruolo
				dataReader.Read()
				Dim nodoPadre As New TreeNode

				If bytbordo = 1 Then 'formatto il nodo padre aggiungendo una cornice se è già accreditato
					nodoPadre.Value = NODE_VALUE_FOREST_GREEN
				Else
					nodoPadre.Value = NODE_VALUE_NO_COLOR
				End If
				'ruolo nodo padre
				nodoPadre.Text = dataReader.Item("ruolo")
				strVerificaAccredita = dataReader.Item("ruolo") 'valorizzo per controllo tasto accredita
				'formatto
				ImpostaStyleClasse(nodoPadre)
				nodoPadre.Expanded = True
				TrwVincoli.Nodes.Add(nodoPadre)

				dataReader.Close()
				dataReader = Nothing

				'verifico se esistono relazioni
				myQuerySql = "Select idrelazionevincoli From RelazioneVincoli Where idruolo = " & idRuolo
				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If dataReader.HasRows = False Then 'se non esistono esco
					dataReader.Close()
					dataReader = Nothing
					Exit Sub
				End If
				dataReader.Read() 'salvo inizio per ricorsiva
				IntInizio = dataReader.Item("IdRelazioneVincoli")
				dataReader.Close()
				dataReader = Nothing
				myQuerySql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," &
						 " VociMenu.IdVoceMenuPadre" &
						 " FROM VociMenu" &
						 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" &
						 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
				'============================================================================================================================
				'====================================================30/09/2008==============================================================
				'=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
				'=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
				'============================================================================================================================
				If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
				Else
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
				End If
				myQuerySql = myQuerySql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" &
						 " WHERE (VociMenu.descrizione = 'Forza Accreditamento Risorse')" &
						 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"

				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If dataReader.HasRows = True Then
					Imgaccredita.Visible = True
					cmdChiudiEnte.Visible = True
				End If
				dataReader.Close()
				dataReader = Nothing

			ElseIf Request.QueryString("tipologia") = "Progetti" Then 'gestico progetti
				Dim DtrGenerico As System.Data.SqlClient.SqlDataReader
				Dim intappo As Integer
				Dim strappo As String
				myQuerySql = "select a.idtipoprogetto,b.descrizione from attività a" &
				" inner join tipiprogetto b on a.idtipoprogetto=b.idtipoprogetto" &
				" where a.idattività=" & Request.QueryString("idattivita") & ""
				DtrGenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If DtrGenerico.HasRows = False Then
					DtrGenerico.Close()
					DtrGenerico = Nothing
					Exit Sub
				Else
					Do While DtrGenerico.Read
						If IsDBNull(DtrGenerico.GetValue(0)) = False Then
							intappo = DtrGenerico.GetValue(0)
							strappo = DtrGenerico.GetValue(1)
						Else
							DtrGenerico.Close()
							DtrGenerico = Nothing
							Exit Sub
						End If
					Loop
					Dim NodoPadre As New TreeNode

					If bytbordo = 1 Then 'formatto il nodo padre aggiungendo una cornice se è già accreditato
						NodoPadre.Value = NODE_VALUE_FOREST_GREEN
					Else
						NodoPadre.Value = NODE_VALUE_NO_COLOR
					End If
					'classe accreditamento padre
					NodoPadre.Text = strappo
					strVerificaAccredita = strappo
					'formatto
					NodoPadre.ImageUrl = URL_IMMAGINE_CLASSE
					NodoPadre.Expanded = True
					TrwVincoli.Nodes.Add(NodoPadre)

					DtrGenerico.Close()
					DtrGenerico = Nothing


					myQuerySql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," &
							 " VociMenu.IdVoceMenuPadre" &
							 " FROM VociMenu" &
							 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" &
							 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
					'============================================================================================================================
					'====================================================30/09/2008==============================================================
					'=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
					'=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
					'============================================================================================================================
					If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
						myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
					Else
						myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
					End If
					myQuerySql = myQuerySql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" &
							 " WHERE (VociMenu.descrizione = 'Forza Validazione Progetti')" &
							 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"

					DtrGenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
					If DtrGenerico.HasRows = True Then
						Imgaccredita.Visible = True
						cmdChiudiEnte.Visible = True
					End If
					DtrGenerico.Close()
					DtrGenerico = Nothing

				End If
				'verifico se esistono relazioni
				myQuerySql = "Select idrelazionevincoli From RelazioneVincoli Where idtipoprogetto = " & intappo
				DtrGenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If DtrGenerico.HasRows = False Then 'se non esistono esco
					DtrGenerico.Close()
					DtrGenerico = Nothing
					Exit Sub
				End If
				DtrGenerico.Read() 'salvo inizio per ricorsiva
				IntInizio = DtrGenerico.GetValue(0)
				DtrGenerico.Close()
				DtrGenerico = Nothing
				myQuerySql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," &
				  " VociMenu.IdVoceMenuPadre" &
				  " FROM VociMenu" &
				  " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" &
				  " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
				'============================================================================================================================
				'====================================================30/09/2008==============================================================
				'=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
				'=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
				'============================================================================================================================
				If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
				Else
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
				End If
				myQuerySql = myQuerySql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" &
				" WHERE (VociMenu.descrizione = 'Forza Validazione Progetti')" &
				" AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"


				DtrGenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If DtrGenerico.HasRows = True Then
					Imgaccredita.Visible = True
					cmdChiudiEnte.Visible = True
				End If
				DtrGenerico.Close()
				DtrGenerico = Nothing
			End If
		End If

		'Query per la definizione dei figli (In base al punto di ingresso indicato)
		If BytPresentata = 0 Then
			myQuerySql = "SELECT C.IdFiglio,TipoNodo,B.IdVincolo,B.Vincolo,C.IdPadre,A.Descrizione,IsNull(B.Valore,'') AS Valore ,isnull(a.link,'') as link , subiudice " &
					 "FROM RelazioneVincoli A " &
					 "INNER JOIN Associarelazionevincoli C ON A.IdRelazioneVincoli = C.IdFiglio " &
					 "LEFT JOIN Vincoli B ON A.IdVincolo = B.IdVincolo WHERE C.IdPadre = " & IntInizio
		Else ' eseguo altra query per visione albero sospeso
			myQuerySql = " SELECT C.IdFiglio,a.TipoNodo,B.IdVincolo,B.Vincolo,C.IdPadre,A.Descrizione,IsNull(B.Valore,'') AS Valore ,'' as link ," &
					 " subiudice, idsospensionealbero, d.idfiglio, d.idpadre, IsNull(d.imageurl,'') as imageurl," &
					 " IsNull(d.style,'') as style, '' as linkpresentata, IsNull(nodedata,'') as nodedata" &
					 " FROM RelazioneVincoli A" &
					 " INNER JOIN Associarelazionevincoli C ON A.IdRelazioneVincoli = C.IdFiglio" &
					 " LEFT JOIN Vincoli B ON A.IdVincolo = B.IdVincolo" &
					 " left join sospensionealbero D on c.idfiglio=d.idfiglio and c.idpadre=d.idpadre" &
					 " WHERE C.Idpadre = " & IntInizio & " And d.idente = " & Session("IdEnte") & ""
		End If
		'se gestisco enti e se è accreditato includo condizione subiudice<>1
		If Request.QueryString("tipologia") = "Enti" And bytePassaBordo = 1 Then
			myQuerySql = myQuerySql & " and A.subiudice<>1"
		End If

		'DtrGenerico = ClsServer.CreaDatareader(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

		Dim MyDataTable As System.Data.DataTable
		MyDataTable = ClsServer.CreaDataTable(myQuerySql, False, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

		Dim MyRow As System.Data.DataRow

		' ''Imposto la Variabile Soddisfatto (Riferita alla Variabile BlnSoddisfatto del ciclo ricorsivo precedente)
		' ''In modo che ritornando poi al ciclo ricorsivo precedente nella variabile BlnSoddisfatto trovo il risultato
		' ''impostato in Soddisfatto

		If OpLogico = "AND" Then
			BlnSoddisfatto = True
		ElseIf OpLogico = "OR" Then
			BlnSoddisfatto = False
		ElseIf OpLogico = "" Then 'Qualora io sia un vincolo e mio padre sia una Radice (Lo tratto come un OR visto che un solo vicolo può stare sotto una Radice se è senza operatore)
			BlnSoddisfatto = True
		End If


		Dim nodo As TreeNode 'Nodo Generico

		For Each MyRow In MyDataTable.Rows
			'Definizione del tipo di nodo (Inserimento della relativa immagine e del relativo testo)
			If MyRow.Item("TipoNodo") = "R" Then 'RADICE

				nodo = New TreeNode
				nodo.Text = MyRow.Item("Descrizione")
				If UCase(nodo.Text) = "MODELLO 1" Then
					nodo.Expanded = True
				Else
					nodo.Expanded = False
				End If
				'verifico se campo subiudice sia valorizzato per salvataggio valore
				If MyRow.Item("subiudice") = True Then
					nodo.Target = "PD"
				End If
				nodo.NavigateUrl = MyRow.Item("link") 'scommentato da vedere
				nodo.Value = NODE_VALUE_NO_COLOR
				StrOpLogica = ""

			ElseIf MyRow.Item("TipoNodo") = "A" Then 'AND

				nodo = New TreeNode
				nodo.Text = "<strong>E</strong>"
				nodo.Expanded = True
				nodo.Value = NODE_VALUE_NO_COLOR
				StrOpLogica = "AND"

			ElseIf MyRow.Item("TipoNodo") = "O" Then 'OR

				nodo = New TreeNode
				nodo.Text = "<strong>O</strong>"
				nodo.Expanded = True
				nodo.Value = NODE_VALUE_NO_COLOR
				StrOpLogica = "OR"



			ElseIf MyRow.Item("TipoNodo") = "V" Then 'VINCOLO padre
				Dim vincolo As String = MyRow.Item("Vincolo")
				nodo = New TreeNode
				nodo.Text = vincolo
				'nodo.Target = MyRow.Item("IdVincolo")
				'Nel caso dei vincolo controllo se sono 'Documentali o Calcolati
				'Per i Calcolati eseguo la StoreProcedure identificata nel Campo Valore
				'Per i documetali eseguo il controllo di valorizzazione
				If MyRow.Item("Valore") = "" Then
					nodo.Value = NODE_VALUE_NO_COLOR
					If Request.QueryString("tipologia") = "Enti" Then
						Select Case ControllaVincoliDocumentali(Session("IdEnte"), MyRow.Item("IdVincolo"))         'modifica per la doppia gestione dell'albero
							Case 1 'Occorrenza non trovata
								ImpostaStyleVincoloDaValidare(nodo)
								'controllo nodi e radici secondarie sempre verdi solo per enti e e utenti ente
								If Request.QueryString("tipologia") = "Enti" And Session("TipoUtente") = "E" Then
									hf_NodeData.Value = "SI"
								Else
									hf_NodeData.Value = "NO"
								End If
								' hf_NodeData.Value  = "NO"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR
								End If
								If BytPresentata = 1 Then
									nodo.ImageUrl = Trim(MyRow.Item("imageurl"))
									nodo.Value = NODE_VALUE_NO_COLOR
									hf_NodeData.Value = Trim(MyRow.Item("nodedata"))
								End If
							Case 2 'Occorrenza Vuota
								ImpostaStyleVincoloRosso(nodo)
								hf_NodeData.Value = "NO"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR
								End If
								If BytPresentata = 1 Then
									nodo.ImageUrl = Trim(MyRow.Item("imageurl"))
									nodo.Value = NODE_VALUE_NO_COLOR
									hf_NodeData.Value = Trim(MyRow.Item("nodedata"))
								End If
							Case 3 'Occorrenza Piena
								ImpostaStyleVincoloValido(nodo)
								hf_NodeData.Value = "SI"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR
								End If
								'Controllo per dato da Presentazione Domanda
								If BytPresentata = 1 Then
									nodo.ImageUrl = Trim(MyRow.Item("imageurl"))
									nodo.Value = NODE_VALUE_NO_COLOR
									hf_NodeData.Value = Trim(MyRow.Item("nodedata"))
								End If
						End Select

					ElseIf Request.QueryString("tipologia") = "Ruoli" Then
						Select Case ControllaVincoliDocumentali(idEntePersonale, MyRow.Item("IdVincolo"))         'modifica per la doppia gestione dell'albero
							Case 1 'Occorrenza non trovata
								ImpostaStyleVincoloDaValidare(nodo)
								hf_NodeData.Value = "NO"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR
								End If
							Case 2 'Occorrenza Vuota
								ImpostaStyleVincoloRosso(nodo)
								hf_NodeData.Value = "NO"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR
								End If
							Case 3 'Occorrenza Piena
								ImpostaStyleVincoloValido(nodo)
								hf_NodeData.Value = "SI"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR
								End If
						End Select
					ElseIf Request.QueryString("tipologia") = "Progetti" Then
						nodo.Text = vincolo
						'attenzione ho commentato i controlli tipo di utente in attesa di informazioni su gestione

						Select Case ControllaVincoliDocumentali(Request.QueryString("idattivita"), MyRow.Item("IdVincolo"))         'modifica per la doppia gestione dell'albero
							Case 1 'Occorrenza non trovata
								ImpostaStyleVincoloDaValidare(nodo)
								hf_NodeData.Value = "NO"

							Case 2 'Occorrenza Vuota
								ImpostaStyleVincoloRosso(nodo)
								hf_NodeData.Value = "NO"
							Case 3 'Occorrenza Piena
								ImpostaStyleVincoloValido(nodo)
								hf_NodeData.Value = "SI"
						End Select
					End If
				Else
					nodo.Value = NODE_VALUE_NO_COLOR
					nodo.Text = vincolo
					'Eseguo la Store Procedure per vedere se il vincolo è Soddisfatto
					If Request.QueryString("tipologia") = "Enti" Then 'per enti *********
						If ClsServer.EseguiStoreVincoli(Session("IdEnte"), IntClasse, MyRow.Item("Valore"), MyRow.Item("IdVincolo"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
							ImpostaStyleVincoloValido(nodo)
							hf_NodeData.Value = "SI"
							nodo.Value = NODE_VALUE_NO_COLOR
						Else
							ImpostaStyleVincoloRosso(nodo)
							hf_NodeData.Value = "NO"
							nodo.Value = NODE_VALUE_NO_COLOR
						End If
						'Controllo per dato da Presentazione Domanda
						If BytPresentata = 1 Then
							nodo.ImageUrl = Trim(MyRow.Item("imageurl"))
							nodo.Value = NODE_VALUE_NO_COLOR

							hf_NodeData.Value = Trim(MyRow.Item("nodedata"))
						End If
					ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'per ruoli *********

						If ClsServer.EseguiStoreVincoli(idEntePersonale, IntClasse, MyRow.Item("Valore"), MyRow.Item("IdVincolo"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
							ImpostaStyleVincoloValido(nodo)
							hf_NodeData.Value = "SI"
							nodo.Value = NODE_VALUE_NO_COLOR

						Else
							ImpostaStyleVincoloRosso(nodo)
							hf_NodeData.Value = "NO"
							nodo.Value = NODE_VALUE_NO_COLOR

						End If

					ElseIf Request.QueryString("tipologia") = "Progetti" Then 'per progetti *********

						If ClsServer.EseguiStoreVincoli(Request.QueryString("idattivita"), IntClasse, MyRow.Item("Valore"), MyRow.Item("IdVincolo"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
							ImpostaStyleVincoloValido(nodo)
							hf_NodeData.Value = "SI"
							nodo.Value = NODE_VALUE_NO_COLOR

						Else
							ImpostaStyleVincoloRosso(nodo)
							hf_NodeData.Value = "NO"


						End If
					End If
				End If
				StrOpLogica = ""
			End If

			'Se il nodo padre è = Nothing, Aggiungo il nodo creato all'albero
			TrwVincoli.Nodes.Add(nodo)

			'Richiamo la routine passando come nodo padre il nodo appena creato
			'Viene eseguita solamente nel caso che il tipo non sia un vincolo
			If MyRow.Item("TipoNodo") <> "V" Then
				CaricamentoFiglio(nodo, IntClasse, MyRow.Item("IdFiglio"), StrOpLogica, False, , BytPresentata)
			End If


			'Imposto la Variabile Soddisfatto (Riferita alla Variabile BlnSoddisfatto del ciclo ricorsivo precedente)
			'In modo che ritornando poi al ciclo ricorsivo precedente nella variabile BlnSoddisfatto trovo il risultato
			'impostato in Soddisfatto
			If OpLogico = "AND" Then
				If hf_NodeData.Value = "NO" Then
					BlnSoddisfatto = False
				End If
			ElseIf OpLogico = "OR" Then
				If hf_NodeData.Value = "SI" Then
					BlnSoddisfatto = True
				End If
			ElseIf OpLogico = "" Then 'Qualora io sia un vincolo e mio padre sia una Radice (Lo tratto come un OR visto che un solo vicolo può stare sotto una Radice se è senza operatore)
				If hf_NodeData.Value = "NO" Then
					BlnSoddisfatto = False
				Else
					BlnSoddisfatto = True
				End If
			End If
		Next


		If TrwVincoli.Nodes.Count > 0 Then 'If MyNode.Nodes.Count > 0 Then
			If BlnSoddisfatto = False Then

				nodo.Value = NODE_VALUE_RED
				nodo.Target = "NO"

				'Valorizzo radice Padre se Rossa per presentazione domanda
				If nodo.Depth = "0" Then
					nodo.Value = "0"
				End If
				'Se il MyNode è di Tipo Modulo, Aggiungo l'icona
				If Not nodo Is Nothing Then
					If nodo.ImageUrl = "" And nodo.Text <> "E" And nodo.Text <> "O" Then
						ImpostaStyleKOPresentazione(nodo)
					End If
				End If

				'Anche se l'Ente non può essere accreditato lo si può disaccreditare solo se Utente Unsc
				If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
					'verifico se putruò accreditare, se il tipo di utente è UNSC
					If hf_AdeguamentoOk.Value = VALORE_TRUE Then
						cmdChiudiEnte.Visible = False
					Else
						If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
							cmdChiudiEnte.Visible = True
						End If
					End If
				End If

			Else

				nodo.Value = NODE_VALUE_GREEN
				nodo.Target = "SI"
				'verifico radice e colore per tasto imgaccredita
				'If Trim(nodo.Text) = Trim(strVerificaAccredita) Then 'verifico se può accreditare
				'    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then 'verifico se può accreditare, se il tipo di utente è UNSC
				'        If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
				'            Imgaccredita.Visible = True
				'            cmdChiudiEnte.Visible = True
				'        End If

				'    End If
				'Else ' Aggiunto da Alessandra Taballione il 09/06/2005
				'    'Anche se l'Ente non può essere accreditato lo si può disaccreditare solo se Utente Unsc
				'    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then 'verifico se può accreditare, se il tipo di utente è UNSC
				'        If hf_AdeguamentoOk.Value = VALORE_TRUE Then
				'            cmdChiudiEnte.Visible = False
				'        Else
				'            If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
				'                cmdChiudiEnte.Visible = True
				'            End If
				'        End If
				'    End If
				'End If
				If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then 'verifico se può accreditare, se il tipo di utente è UNSC
					If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
						Imgaccredita.Visible = True
						cmdChiudiEnte.Visible = True
					End If

				End If
				'Valorizzo radice Padre se Verde per presentazione domanda
				If nodo.Depth = "0" Then
					nodo.Target = "1"
				End If
				'Se il MyNode è di Tipo Modulo, Aggiungo l'icona
				If Not nodo Is Nothing Then
					If nodo.ImageUrl = "" And nodo.Text <> "E" And nodo.Text <> "O" Then
						'valorizzo proprietà della label solo per appoggiare il parametro su di un oggetto qualsiasi
						If nodo.Value = "PD" Then
							lblmess.CssClass = "1"
						End If
						ImpostaStyleOkPresentazione(nodo)
					End If
				End If
			End If
		End If
	End Sub

	Private Sub CaricamentoFiglio(ByRef MyNode As TreeNode, Optional ByVal IntClasse As Integer = Nothing, Optional ByVal IntPadre As Integer = 0, Optional ByVal OpLogico As String = "", Optional ByVal blnPrimo As Boolean = True, Optional ByVal bytbordo As Byte = 0, Optional ByVal BytPresentata As Byte = 0)
		'***Questa routine carica l'albero attraverso una azione ricorsiva
		'***verificando le relazioni logiche dei vincolo (dai nodi centrali e nodo padre) con le classi 
		'***di accreditamento (se ente) o per i ruoli (se personale),"ANCHE" PROGETTI.
		'***Gestione Presentazione Ente
		Dim operazioneLogica As String = "" 'variabile appoggio operatore logico
		Dim BlnSoddisfatto As Boolean 'variabile controllo operatore logico
		Dim inizio As Integer 'In base alla classe selezionata trovo il suo punto di appoggio


		If blnPrimo = False Then 'verifico se è il primo passaggio per ricavare nodopadre
			inizio = IntPadre
		Else

			If Request.QueryString("tipologia") = "Enti" Then 'controllo se enti
				Dim dataReader As System.Data.SqlClient.SqlDataReader
				'Sono entrato da una classe (Quindi inizio l'albero con la descrizione della classe)
				'Calcolo prima la descrizione della classe
				myQuerySql = "Select ClasseAccreditamento From ClassiAccreditamento WHERE IdClasseAccreditamento = " & IntClasse
				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				dataReader.Read()
				Dim nodoPadre As New TreeNode
				If bytbordo = 1 Then 'formatto il nodo padre aggiungendo una cornice se è già accreditato
					nodoPadre.Value = NODE_VALUE_FOREST_GREEN
				Else
					nodoPadre.Value = NODE_VALUE_NO_COLOR
				End If
				'classe accreditamento padre
				nodoPadre.Text = dataReader.Item("ClasseAccreditamento")
				strVerificaAccredita = dataReader.Item("ClasseAccreditamento") 'valorizzo per controllo tasto accredita
				'formatto
				ImpostaStyleClasse(nodoPadre)
				nodoPadre.Expanded = True
				TrwVincoli.Nodes.Add(nodoPadre)
				MyNode = nodoPadre
				ChiudiDataReader(dataReader)
				'verifico se esistono relazioni
				myQuerySql = "Select IdRelazioneVincoli From RelazioneVincoli Where IdClasseAccreditamento = " & IntClasse
				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If dataReader.HasRows = False Then 'se non esistono esco
					ChiudiDataReader(dataReader)
					Exit Sub
				End If
				dataReader.Read()
				inizio = dataReader.Item("IdRelazioneVincoli") 'salvo inizio per ricorsiva
				ChiudiDataReader(dataReader)

				myQuerySql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," &
						 " VociMenu.IdVoceMenuPadre" &
						 " FROM VociMenu" &
						 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" &
						 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"

				If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
				Else
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
				End If

				myQuerySql = myQuerySql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" &
						 " WHERE VociMenu.descrizione = 'Forza Accreditamento Enti'" &
						 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"


				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If dataReader.HasRows = True Then
					ChiudiDataReader(dataReader)
					If txtstato.Value = VALORE_TRUE And txtclasse.Value = "False" Then
						hf_AdeguamentoOk.Value = VALORE_TRUE
						Imgaccredita.ToolTip = "Adeguamento OK"
						'12/09/2014 abilito la stampa della copertina dell'adeguamento accreditamento 
						imgStampaCopertina.Visible = True
						'rendo invisibile maschera caricamento documenti
						imgElencoDocumentiEnti.Visible = False
						If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
							Imgaccredita.Visible = True
							cmdChiudiEnte.Visible = False
						End If
					Else
						If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
							Imgaccredita.Visible = True
							cmdChiudiEnte.Visible = True
						End If
					End If
				End If
				ChiudiDataReader(dataReader)


			ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'inizio parte ruoli
				Dim dataReader As System.Data.SqlClient.SqlDataReader
				myQuerySql = "Select ruolo From ruoli WHERE Idruolo = " & idRuolo
				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'mi ricavo il ruolo
				dataReader.Read()
				Dim nodoPadre As New TreeNode
				If bytbordo = 1 Then 'formatto il nodo padre aggiungendo una cornice se è già accreditato
					nodoPadre.Value = NODE_VALUE_FOREST_GREEN 'MyStyle.FromString("font-name:Verdana;font-size:12pt;font-weight:bold;color:Black;border:double 5px;border-color:forestgreen")
				Else
					nodoPadre.Value = NODE_VALUE_NO_COLOR
				End If
				'ruolo nodo padre
				nodoPadre.Text = dataReader.Item("ruolo")
				strVerificaAccredita = dataReader.Item("ruolo") 'valorizzo per controllo tasto accredita
				'formatto
				ImpostaStyleClasse(nodoPadre)
				nodoPadre.Expanded = True
				TrwVincoli.Nodes.Add(nodoPadre)
				MyNode = nodoPadre
				ChiudiDataReader(dataReader)

				'verifico se esistono relazioni
				myQuerySql = "Select idrelazionevincoli From RelazioneVincoli Where idruolo = " & idRuolo
				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If dataReader.HasRows = False Then 'se non esistono esco
					ChiudiDataReader(dataReader)
					Exit Sub
				End If
				dataReader.Read() 'salvo inizio per ricorsiva
				inizio = dataReader.Item("IdRelazioneVincoli")
				ChiudiDataReader(dataReader)
				myQuerySql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," &
						  " VociMenu.IdVoceMenuPadre" &
						  " FROM VociMenu" &
						  " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" &
						  " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
				If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
				Else
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
				End If
				myQuerySql = myQuerySql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" &
						 " WHERE (VociMenu.descrizione = 'Forza Accreditamento Risorse')" &
						 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"

				dataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If dataReader.HasRows = True Then
					Imgaccredita.Visible = True
					cmdChiudiEnte.Visible = True
				End If
				ChiudiDataReader(dataReader)

			ElseIf Request.QueryString("tipologia") = "Progetti" Then 'gestico progetti
				Dim DtrGenerico As System.Data.SqlClient.SqlDataReader
				Dim intappo As Integer
				Dim strappo As String
				myQuerySql = "select a.idtipoprogetto,b.descrizione from attività a" &
				" inner join tipiprogetto b on a.idtipoprogetto=b.idtipoprogetto" &
				" where a.idattività=" & Request.QueryString("idattivita") & ""
				DtrGenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If DtrGenerico.HasRows = False Then
					DtrGenerico.Close()
					DtrGenerico = Nothing
					Exit Sub
				Else
					Do While DtrGenerico.Read
						If IsDBNull(DtrGenerico.GetValue(0)) = False Then
							intappo = DtrGenerico.GetValue(0)
							strappo = DtrGenerico.GetValue(1)
						Else
							ChiudiDataReader(DtrGenerico)
							Exit Sub
						End If
					Loop
					Dim NodoPadre As New TreeNode
					If bytbordo = 1 Then 'formatto il nodo padre aggiungendo una cornice se è già accreditato
						NodoPadre.Value = NODE_VALUE_FOREST_GREEN
					Else
						NodoPadre.Value = NODE_VALUE_NO_COLOR
					End If
					'classe accreditamento padre
					NodoPadre.Text = strappo
					strVerificaAccredita = strappo
					'formatto
					ImpostaStyleClasse(NodoPadre)
					NodoPadre.Expanded = True
					TrwVincoli.Nodes.Add(NodoPadre)
					MyNode = NodoPadre
					ChiudiDataReader(DtrGenerico)


					myQuerySql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," &
							 " VociMenu.IdVoceMenuPadre" &
							 " FROM VociMenu" &
							 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" &
							 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
					'============================================================================================================================
					'====================================================30/09/2008==============================================================
					'=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
					'=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
					'============================================================================================================================
					If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
						myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
					Else
						myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
					End If
					myQuerySql = myQuerySql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" &
							 " WHERE (VociMenu.descrizione = 'Forza Validazione Progetti')" &
							 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"

					DtrGenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
					If DtrGenerico.HasRows = True Then
						Imgaccredita.Visible = True
						cmdChiudiEnte.Visible = True
					End If
					DtrGenerico.Close()
					DtrGenerico = Nothing

				End If
				'verifico se esistono relazioni
				myQuerySql = "Select idrelazionevincoli From RelazioneVincoli Where idtipoprogetto = " & intappo
				DtrGenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If DtrGenerico.HasRows = False Then 'se non esistono esco
					DtrGenerico.Close()
					DtrGenerico = Nothing
					Exit Sub
				End If
				DtrGenerico.Read() 'salvo inizio per ricorsiva
				inizio = DtrGenerico.GetValue(0)
				DtrGenerico.Close()
				DtrGenerico = Nothing

				myQuerySql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," &
				" VociMenu.IdVoceMenuPadre" &
				" FROM VociMenu" &
				" INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" &
				" INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
				'============================================================================================================================
				'====================================================30/09/2008==============================================================
				'=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
				'=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
				'============================================================================================================================
				If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
				Else
					myQuerySql = myQuerySql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
				End If
				myQuerySql = myQuerySql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" &
				" WHERE (VociMenu.descrizione = 'Forza Validazione Progetti')" &
				" AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"


				DtrGenerico = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				If DtrGenerico.HasRows = True Then
					Imgaccredita.Visible = True
					cmdChiudiEnte.Visible = True
				End If
				DtrGenerico.Close()
				DtrGenerico = Nothing
			End If
		End If

		'Query per la definizione dei figli (In base al punto di ingresso indicato)
		If BytPresentata = 0 Then
			myQuerySql = "SELECT C.IdFiglio,TipoNodo,B.IdVincolo,B.Vincolo,C.IdPadre,A.Descrizione,IsNull(B.Valore,'') AS Valore ,isnull(a.link,'') as link , subiudice " &
					 "FROM RelazioneVincoli A " &
					 "INNER JOIN Associarelazionevincoli C ON A.IdRelazioneVincoli = C.IdFiglio " &
					 "LEFT JOIN Vincoli B ON A.IdVincolo = B.IdVincolo WHERE C.IdPadre = " & inizio
		Else ' eseguo altra query per visione albero sospeso
			myQuerySql = " SELECT C.IdFiglio,a.TipoNodo,B.IdVincolo,B.Vincolo,C.IdPadre,A.Descrizione,IsNull(B.Valore,'') AS Valore ,'' as link ," &
					 " subiudice, idsospensionealbero, d.idfiglio, d.idpadre, IsNull(d.imageurl,'') as imageurl," &
					 " IsNull(d.style,'') as style, '' as linkpresentata, IsNull(nodedata,'') as nodedata" &
					 " FROM RelazioneVincoli A" &
					 " INNER JOIN Associarelazionevincoli C ON A.IdRelazioneVincoli = C.IdFiglio" &
					 " LEFT JOIN Vincoli B ON A.IdVincolo = B.IdVincolo" &
					 " left join sospensionealbero D on c.idfiglio=d.idfiglio and c.idpadre=d.idpadre" &
					 " WHERE C.Idpadre = " & inizio & " And d.idente = " & Session("IdEnte") & ""
		End If
		'se gestisco enti e se è accreditato includo condizione subiudice<>1
		If Request.QueryString("tipologia") = "Enti" And bytePassaBordo = 1 Then
			myQuerySql = myQuerySql & " and A.subiudice<>1"
		End If

		Dim MyDataTable As System.Data.DataTable
		MyDataTable = ClsServer.CreaDataTable(myQuerySql, False, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

		Dim MyRow As System.Data.DataRow

		'Imposto la Variabile Soddisfatto (Riferita alla Variabile BlnSoddisfatto del ciclo ricorsivo precedente)
		'In modo che ritornando poi al ciclo ricorsivo precedente nella variabile BlnSoddisfatto trovo il risultato
		'impostato in Soddisfatto

		If OpLogico = "AND" Then
			BlnSoddisfatto = True
		ElseIf OpLogico = "OR" Then
			BlnSoddisfatto = False
		ElseIf OpLogico = "" Then 'Qualora io sia un vincolo e mio padre sia una Radice (Lo tratto come un OR visto che un solo vicolo può stare sotto una Radice se è senza operatore)
			BlnSoddisfatto = True
		End If


		Dim nodo As TreeNode 'Nodo Generico

		For Each MyRow In MyDataTable.Rows
			'Definizione del tipo di nodo (Inserimento della relativa immagine e del relativo testo)
			If MyRow.Item("TipoNodo") = "R" Then 'RADICE

				nodo = New TreeNode
				nodo.Text = MyRow.Item("Descrizione")
				If UCase(nodo.Text) = "MODELLO 1" Then
					nodo.Expanded = True
				Else
					nodo.Expanded = False
				End If
				'verifico se campo subiudice sia valorizzato per salvataggio valore
				If MyRow.Item("subiudice") = True Then
					nodo.Target = "PD"
				End If
				nodo.NavigateUrl = MyRow.Item("link") 'scommentato da vedere
				nodo.Value = NODE_VALUE_NO_COLOR
				operazioneLogica = ""

			ElseIf MyRow.Item("TipoNodo") = "A" Then 'AND

				nodo = New TreeNode
				nodo.Text = "<strong>E</strong>"
				nodo.Expanded = True
				nodo.Value = NODE_VALUE_NO_COLOR
				operazioneLogica = "AND"

			ElseIf MyRow.Item("TipoNodo") = "O" Then 'OR
				nodo = New TreeNode
				nodo.Text = "<strong>O</strong>"
				nodo.Expanded = True
				nodo.Value = NODE_VALUE_NO_COLOR

				operazioneLogica = "OR"



			ElseIf MyRow.Item("TipoNodo") = "V" Then 'VINCOLO

				nodo = New TreeNode
				Dim vincolo As String = MyRow.Item("Vincolo")
				Dim vincoloItalic As String = "<em >" & vincolo & "</em>"
				nodo.Text = vincoloItalic

				'nodo.Target = MyRow.Item("IdVincolo")
				nodo.NavigateUrl = CostruisciUrl(MyRow.Item("IdVincolo"))
				'Nel caso dei vincolo controllo se sono 'Documentali o Calcolati
				'Per i Calcolati eseguo la StoreProcedure identificata nel Campo Valore
				'Per i documetali eseguo il controllo di valorizzazione
				If MyRow.Item("Valore") = "" Then
					nodo.Value = NODE_VALUE_NO_COLOR
					If Request.QueryString("tipologia") = "Enti" Then
						Select Case ControllaVincoliDocumentali(Session("IdEnte"), MyRow.Item("IdVincolo"))         'modifica per la doppia gestione dell'albero
							Case 1 'Occorrenza non trovata
								ImpostaStyleVincoloDaValidare(nodo)
								'controllo nodi e radici secondarie sempre verdi solo per enti e e utenti ente
								If Request.QueryString("tipologia") = "Enti" And Session("TipoUtente") = "E" Then
									hf_NodeData.Value = "SI"
								Else
									hf_NodeData.Value = "NO"
								End If
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR
								End If
								If BytPresentata = 1 Then
									nodo.ImageUrl = Trim(MyRow.Item("imageurl"))
									nodo.Value = NODE_VALUE_NO_COLOR
									hf_NodeData.Value = Trim(MyRow.Item("nodedata"))
								End If
							Case 2 'Occorrenza Vuota
								ImpostaStyleVincoloRosso(nodo)
								hf_NodeData.Value = "NO"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR

								End If
								If BytPresentata = 1 Then
									nodo.ImageUrl = Trim(MyRow.Item("imageurl"))
									nodo.Value = NODE_VALUE_NO_COLOR

									hf_NodeData.Value = Trim(MyRow.Item("nodedata"))
								End If
							Case 3 'Occorrenza Piena
								ImpostaStyleVincoloValido(nodo)
								hf_NodeData.Value = "SI"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR

								End If
								'Controllo per dato da Presentazione Domanda
								If BytPresentata = 1 Then
									nodo.ImageUrl = Trim(MyRow.Item("imageurl"))
									nodo.Value = NODE_VALUE_NO_COLOR

									hf_NodeData.Value = Trim(MyRow.Item("nodedata"))
								End If
						End Select

					ElseIf Request.QueryString("tipologia") = "Ruoli" Then
						Select Case ControllaVincoliDocumentali(idEntePersonale, MyRow.Item("IdVincolo"))         'modifica per la doppia gestione dell'albero
							Case 1 'Occorrenza non trovata
								ImpostaStyleVincoloDaValidare(nodo)
								hf_NodeData.Value = "NO"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR

								End If
							Case 2 'Occorrenza Vuota
								ImpostaStyleVincoloRosso(nodo)
								hf_NodeData.Value = "NO"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR

								End If
							Case 3 'Occorrenza Piena
								ImpostaStyleVincoloValido(nodo)
								hf_NodeData.Value = "SI"
								If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
									nodo.Value = NODE_VALUE_NO_COLOR

								End If
						End Select
					ElseIf Request.QueryString("tipologia") = "Progetti" Then

						'attenzione ho commentato i controlli tipo di utente in attesa di informazioni su gestione

						Select Case ControllaVincoliDocumentali(Request.QueryString("idattivita"), MyRow.Item("IdVincolo"))         'modifica per la doppia gestione dell'albero
							Case 1 'Occorrenza non trovata
								ImpostaStyleVincoloDaValidare(nodo)
								nodo.Text = vincoloItalic
								nodo.NavigateUrl = CostruisciUrl(MyRow.Item("IdVincolo"))
								hf_NodeData.Value = "NO"
							Case 2 'Occorrenza Vuota
								ImpostaStyleVincoloRosso(nodo)
								hf_NodeData.Value = "NO"

							Case 3 'Occorrenza Piena
								ImpostaStyleVincoloValido(nodo)
								hf_NodeData.Value = "SI"

						End Select
					End If
				Else
					nodo.Text = vincolo
					nodo.Value = NODE_VALUE_NO_COLOR
					'Eseguo la Store Procedure per vedere se il vincolo è Soddisfatto
					If Request.QueryString("tipologia") = "Enti" Then 'per enti *********
						If ClsServer.EseguiStoreVincoli(Session("IdEnte"), IntClasse, MyRow.Item("Valore"), MyRow.Item("IdVincolo"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
							ImpostaStyleVincoloValido(nodo)
							hf_NodeData.Value = "SI"
							nodo.Value = NODE_VALUE_NO_COLOR
						Else
							ImpostaStyleVincoloRosso(nodo)
							hf_NodeData.Value = "NO"
							nodo.Value = NODE_VALUE_NO_COLOR
						End If
						'Controllo per dato da Presentazione Domanda
						If BytPresentata = 1 Then
							nodo.ImageUrl = Trim(MyRow.Item("imageurl"))
							nodo.Value = NODE_VALUE_NO_COLOR
							hf_NodeData.Value = Trim(MyRow.Item("nodedata"))
						End If
					ElseIf Request.QueryString("tipologia") = "Ruoli" Then 'per ruoli *********

						If ClsServer.EseguiStoreVincoli(idEntePersonale, IntClasse, MyRow.Item("Valore"), MyRow.Item("IdVincolo"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
							ImpostaStyleVincoloValido(nodo)
							hf_NodeData.Value = "SI"
							nodo.Value = NODE_VALUE_NO_COLOR
						Else
							ImpostaStyleVincoloRosso(nodo)
							hf_NodeData.Value = "NO"
							nodo.Value = NODE_VALUE_NO_COLOR
						End If

					ElseIf Request.QueryString("tipologia") = "Progetti" Then 'per progetti *********

						If ClsServer.EseguiStoreVincoli(Request.QueryString("idattivita"), IntClasse, MyRow.Item("Valore"), MyRow.Item("IdVincolo"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
							ImpostaStyleVincoloValido(nodo)
							hf_NodeData.Value = "SI"
							nodo.Value = NODE_VALUE_NO_COLOR
						Else
							ImpostaStyleVincoloRosso(nodo)
							hf_NodeData.Value = "NO"
							nodo.Value = NODE_VALUE_NO_COLOR
						End If
					End If
				End If
				operazioneLogica = ""
			End If

			'Se il nodo padre è = Nothing, Aggiungo il nodo creato all'albero
			'IIf(MyNode Is Nothing, TrwVincoli, MyNode).Nodes.Add(NodoGenerico)
			MyNode.ChildNodes.Add(nodo)
			'Richiamo la routine passando come nodo padre il nodo appena creato
			'Viene eseguita solamente nel caso che il tipo non sia un vincolo
			If MyRow.Item("TipoNodo") <> "V" Then
				CaricamentoFiglio(nodo, IntClasse, MyRow.Item("IdFiglio"), operazioneLogica, False, , BytPresentata)
			End If


			'Imposto la Variabile Soddisfatto (Riferita alla Variabile BlnSoddisfatto del ciclo ricorsivo precedente)
			'In modo che ritornando poi al ciclo ricorsivo precedente nella variabile BlnSoddisfatto trovo il risultato
			'impostato in Soddisfatto
			If OpLogico = "AND" Then
				If hf_NodeData.Value = "NO" Then
					BlnSoddisfatto = False
				End If
			ElseIf OpLogico = "OR" Then
				If hf_NodeData.Value = "SI" Then
					BlnSoddisfatto = True
				End If
			ElseIf OpLogico = "" Then 'Qualora io sia un vincolo e mio padre sia una Radice (Lo tratto come un OR visto che un solo vicolo può stare sotto una Radice se è senza operatore)
				If hf_NodeData.Value = "NO" Then
					BlnSoddisfatto = False
				Else
					BlnSoddisfatto = True
				End If
			End If
		Next


		If MyNode.ChildNodes.Count > 0 Then 'If MyNode.Nodes.Count > 0 Then
			If BlnSoddisfatto = False Then

				MyNode.Value = NODE_VALUE_RED
				hf_NodeData.Value = "NO"

				'Valorizzo radice Padre se Rossa per presentazione domanda
				If MyNode.Depth = "0" Then
					MyNode.Value = "0"
				End If
				'Se il MyNode è di Tipo Modulo, Aggiungo l'icona
				If Not MyNode Is Nothing Then
					If MyNode.ImageUrl = "" And MyNode.Text <> "E" And MyNode.Text <> "O" Then
						ImpostaStyleKOPresentazione(MyNode)
					End If
				End If


				' Aggiunto da Alessandra Taballione il 09/06/2005
				'Anche se l'Ente non può essere accreditato lo si può disaccreditare solo se Utente Unsc
				If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
					'verifico se può accreditare, se il tipo di utente è UNSC
					If hf_AdeguamentoOk.Value = VALORE_TRUE Then
						cmdChiudiEnte.Visible = False
					Else
						If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
							cmdChiudiEnte.Visible = True
						End If
					End If
				End If

			Else
				MyNode.Value = NODE_VALUE_FOREST_GREEN
				hf_NodeData.Value = "SI"
				If Trim(MyNode.Text) = Trim(strVerificaAccredita) Then 'verifico se può accreditare
					If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then 'verifico se può accreditare, se il tipo di utente è UNSC
						If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
							'Imgaccredita.Visible = True 'MOD DANANTO
							cmdChiudiEnte.Visible = True
						End If

					End If
				Else ' Aggiunto da Alessandra Taballione il 09/06/2005
					'Anche se l'Ente non può essere accreditato lo si può disaccreditare solo se Utente Unsc
					If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then 'verifico se può accreditare, se il tipo di utente è UNSC
						If hf_AdeguamentoOk.Value = VALORE_TRUE Then
							cmdChiudiEnte.Visible = False
						Else
							If ControlloStatoEnteChiuso(Session("IdEnte")) = False Then
								cmdChiudiEnte.Visible = True
							End If
						End If
					End If
				End If
				'Valorizzo radice Padre se Verde per presentazione domanda
				If MyNode.Depth = "0" Then
					MyNode.Value = "1"
				End If
				'Se il MyNode è di Tipo Modulo, Aggiungo l'icona
				If Not MyNode Is Nothing Then
					If MyNode.ImageUrl = "" And MyNode.Text <> "E" And MyNode.Text <> "O" Then
						'valorizzo proprietà della label solo per appoggiare il parametro su di un oggetto qualsiasi
						If MyNode.Value = "PD" Then
							lblmess.CssClass = "1"
						End If
						ImpostaStyleOkPresentazione(MyNode)
					End If
				End If
			End If
		End If
	End Sub
	Protected Sub imgsettori_Click(sender As Object, e As EventArgs) Handles imgsettori.Click
		Dim arrivo As String = Request.QueryString("Arrivo")
		Dim vediEnte As String = Request.QueryString("VediEnte")
		Dim identePadre As String = Request.QueryString("identePadre")
		Response.Redirect("WFrmAssociaEntiSettori.aspx?Id=" & idEnte & "&Blocco=FALSE" & "&tipologia=" & tipologia & "&Arrivo=" & arrivo & "&VediEnte=" & vediEnte & "&identePadre=" & identePadre)
	End Sub

	Protected Sub imgConsultaDoc_Click(sender As Object, e As EventArgs) Handles imgConsultaDoc.Click
		Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Costanti.VENGO_DA_ADEGUAMENTO_CONSULTA_DOCUMENTI & "&tipologia=" & tipologia)
	End Sub

    Protected Sub imgElencoDocumentiEnti_Click(sender As Object, e As EventArgs) Handles imgElencoDocumentiEnti.Click
        Response.Redirect("DettaglioFunzioni.aspx?IdVoceMenu=115")
        'Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Costanti.VENGO_DA_ADEGUAMENTO_INSERISCI_DOCUMENTI & "&tipologia=" & tipologia)
    End Sub

	Private Sub ImgVariazioneEnte_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgVariazioneEnte.Click
		Dim tipologia As String = Request.QueryString("tipologia")
		Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Costanti.VENGO_DA_ADEGUAMENTO_VARIAZIONE_ENTE & "&tipologia=" & tipologia)
	End Sub
	Private Sub ImgInizioArt2_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgInizioArt2.Click
		Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Costanti.VENGO_DA_ADEGUAMENTO_ARTICOLO_2 & "&tipologia=" & tipologia)
	End Sub

	Private Sub ImgInizioArt10_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgInizioArt10.Click
		Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Costanti.VENGO_DA_ADEGUAMENTO_ARTICOLO_10 & "&tipologia=" & tipologia)
	End Sub

	Private Sub ImgPresentaDocArt2_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgPresentaDocArt2.Click
		Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Costanti.VENGO_DA_ADEGUAMENTO_PRESENTA_ARTICOLO_2 & "&txtCodiceRegione=" & txtCodiceRegione.Value & "&tipologia=" & tipologia)
	End Sub

	Private Sub ImgPresentaDocArt10_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgPresentaDocArt10.Click
		Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Costanti.VENGO_DA_ADEGUAMENTO_PRESENTA_ARTICOLO_10 & "&txtCodiceRegione=" & txtCodiceRegione.Value & "&tipologia=" & tipologia)

	End Sub

	Private Sub LinkAggiornaSediArt2_Click(sender As Object, e As EventArgs) Handles LinkAggiornaSediArt2.Click
		Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Costanti.VENGO_DA_AGGIORNA_SEDI_ARTICOLO_2 & "&txtCodiceRegione=" & txtCodiceRegione.Value & "&tipologia=" & tipologia)
	End Sub
	Private Function CostruisciUrl(idVincolo As String) As String
		Dim url As String = String.Empty
		Dim idAttivita As String = Request.QueryString("idattivita")
		Dim idEntePersonale As String = Request.QueryString("IDEntePersonale")
		Dim IDRuolo As String = Request.QueryString("IDRuolo")
		Dim vengoDa As String = Costanti.VENGO_DA_ADEGUAMENTO
		Dim tipologiaUtente As String = Session("TipoUtente")
		Dim myReader As SqlClient.SqlDataReader
		Dim query As String = "select idtipovincolo from vincoli where idvincolo='" & ClsServer.NoApice(idVincolo) & "'"
		Try
			myReader = ClsServer.CreaDatareader(query, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) 'verifico se il vincolo può essere gestito

			If myReader.HasRows = True Then
				myReader.Read()
				If myReader.GetValue(0) = "1" Then
					If (tipologia = TIPOLOGIA_ENTE) Then
						If (tipologiaUtente = Costanti.TIPOLOGIA_UTENTE_UNSC Or tipologiaUtente = Costanti.TIPOLOGIA_UTENTE_REGIONE Or (tipologiaUtente = Costanti.TIPOLOGIA_UTENTE_ENTE And ImgPresentazioneDomanda.Visible = True)) Then
							url = "WfrmGestFlagEnti.aspx?Vincolo=" & idVincolo & "&tipologia=" & tipologia
						End If
					ElseIf (tipologia = TIPOLOGIA_RUOLO) Then
						url = "WfrmGestFlagEnti.aspx?Vincolo=" & idVincolo & "&tipologia=" & tipologia & "&IDEntePersonale=" & idEntePersonale & "&IDRuolo=" & IDRuolo
					ElseIf (tipologia = TIPOLOGIA_PROGETTO) Then
						url = "WfrmGestFlagEnti.aspx?Vincolo=" & idVincolo & "&tipologia=" & tipologia & "&IDattivita=" & idAttivita
					End If
				End If
			End If
		Catch e As Exception
			Throw e
		Finally
			ChiudiDataReader(myReader)
		End Try

		Return url
	End Function

	Sub StampaCopertina(ByVal idEnteFase As Integer)
		Dim numDocumenti As Integer
		Dim script As String

		myQuerySql = " Select count(isnull(identedocumento,0)) as NumDocumenti from EntiDocumenti where IdEnteFase=" & idEnteFase & ""
		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
		myDataReader.Read()
		numDocumenti = myDataReader("NumDocumenti")

		ChiudiDataReader(myDataReader)
		script = "<script>" & vbCrLf
		If numDocumenti > 0 Then 'copertina con elenco documenti
			script &= "window.open(""WfrmReportistica.aspx?sTipoStampa=39&IDEnteFase=" & idEnteFase & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
		Else
			script &= "window.open(""WfrmReportistica.aspx?sTipoStampa=40&IDEnteFase=" & idEnteFase & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
		End If
		script &= ("</script>")
		Response.Write(script)
	End Sub

	Protected Sub cmdConfermaChiusura_Click(sender As Object, e As EventArgs) Handles cmdConfermaChiusura.Click
		ChiudiEnte()
		msgConferma.Visible = True
		cmdChiudiEnte.Visible = False
		divChiusuraEnte.Visible = False
	End Sub

	Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
		divChiusuraEnte.Visible = False
	End Sub

	Sub ChiudiEnte()
		Dim intstato As Int64
		'Controllo se il padre e' in Valutazione
		If ControllaAccreditamentoPadre() = False Then
			msgErrore.Text = "L'Ente non può essere Valutato perchè l'Ente Principale non risulta in Valutazione."
			Exit Sub
		End If

		'Aggiunto da Alessandra Taballione
		Dim dtrgenerico As Data.SqlClient.SqlDataReader
		'Eseguo la Store se l'ente accreditato è di tipo padre (Classe Richiesta <= 4)
		dtrgenerico = ClsServer.CreaDatareader("Select IdEnte From Enti Where IdEnte = " & Session("IdEnte") & " And IdClasseAccreditamentoRichiesta <= 4", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If dtrgenerico.HasRows = True Then
			ChiudiDataReader(dtrgenerico)
			'Eseguo la Store SP_ELIMINA_MIGRAZIONE_DATI
			If ClsServer.EseguiStoreSP_ELIMINA_MIGRAZIONE_DATI(Session("IdEnte"), "SP_ELIMINA_MIGRAZIONE_DATI", Session("conn")) = 1 Then
				msgErrore.Text = "Si è verificato un errore durante l'aggiornamento dei dati. Contattare l'assistenza."
				Exit Sub
			End If
		Else
			ChiudiDataReader(dtrgenerico)
		End If

		'************************************************************
		'Modifica del 6/09/2005 di Amilcare Paolella
		'Cancello i campi CF e PI e li salvo in CFArchivio e PIArchivio
		Dim strSqlCFPI As String = "select isnull(CodiceFiscale,'') as CodiceFiscale, isnull(PartitaIva,'') as PartitaIva from enti where idente=" & Session("idente") & ""
		Dim dtrGen As Data.SqlClient.SqlDataReader
		Dim strCF As String
		Dim strPI As String
		dtrGen = ClsServer.CreaDatareader(strSqlCFPI, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		dtrGen.Read()
		strCF = dtrGen.GetValue(0)
		strPI = dtrGen.GetValue(1)
		ChiudiDataReader(dtrGen)
		strSqlCFPI = "Update enti set CodiceFiscale= null, PartitaIva= null, " &
					 " CodiceFiscaleArchivio= '" & strCF.ToString & "'," &
					 " PartitaIvaArchivio= '" & strPI.ToString & "'" &
					 " where idente= " & Session("idente")
		Dim CmdCFPI As New Data.SqlClient.SqlCommand(strSqlCFPI, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		CmdCFPI.ExecuteNonQuery()
		CmdCFPI.Dispose()
		'************************************************************

		'Controllo Flag Forzatura Accreditamento
		'Controllo se è stato impostato il flag
		dtrgenerico = ClsServer.CreaDatareader("Select IsNull(FlagForzaturaAccreditamento,0) From Enti Where IdEnte = " & Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		dtrgenerico.Read()
		If dtrgenerico.Item(0) = True Then
			ChiudiDataReader(dtrgenerico)
			'Imposto a zero il bit di Flag Forzatura Accreditamento
			myQuerySql = "UpDate Enti Set FlagForzaturaAccreditamento = 0 Where IdEnte = " & Session("IdEnte")
			CmdCFPI = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdCFPI.ExecuteNonQuery()

			'Inserimento della cronologia di chiusura dell'apertura accreditamento
			myQuerySql = "Insert Into CronologiaApertureAccreditamento (IdEnte,Data,Utente,TipoOperazione) Values (" & Session("IdEnte") & ",GetDate(),'" & Session("Utente") & "',2)"
			CmdCFPI = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			CmdCFPI.ExecuteNonQuery()
			CmdCFPI.Dispose()
		Else
			ChiudiDataReader(dtrgenerico)
		End If

		'************************************************************
		Dim bytForzaturaEnte As Byte

		'***Eliminazione Record su tabella per ripristino situazione precedente a presentazione
		Dim sqlcommand As Data.SqlClient.SqlCommand
		sqlcommand = New SqlClient.SqlCommand("delete from SospensioneAlbero where idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		sqlcommand.ExecuteNonQuery()
		sqlcommand.Dispose()

		dtrgenerico = ClsServer.CreaDatareader("select a.idente," &
		" a.idstatoente, b.presentazioneprogetti,a.forzatura, b.sospeso from enti a" &
		" inner join statienti b" &
		" on a.idstatoente=b.idstatoente" &
		" where b.Sospeso<>1 and a.idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		Do While dtrgenerico.Read()
			If dtrgenerico.GetValue(2) = True Or dtrgenerico.GetValue(4) = False Then 'se non è già disaccreditato 
				intstato = dtrgenerico.GetValue(1)
				bytForzaturaEnte = dtrgenerico.GetValue(3)
				ChiudiDataReader(dtrgenerico)
				'proseguo andando a modificare tabella enti
				Dim strappo As String = "update enti set dataDeterminazioneNegativa=getdate()," &
				" idstatoente=(select idstatoente from statienti where sospeso=1),ClasseAccreditamentoSubIudice=0,IdRegioneAppartenenza=dbo.RegioneAppartenenza(" & Session("idente") & "),IdRegioneCompetenza=dbo.RegioneCompetenza(" & Session("idente") & ") " &
				" where idente=" & Session("idente") & ""
				Dim cmdente As New Data.SqlClient.SqlCommand(strappo, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				cmdente.ExecuteNonQuery()
				cmdente.Dispose()
				'proseguo andando a popolare la tabella della cronologia (storic
				Dim strquery As String = "insert into" &
				 " cronologiaentistati(idente,idstatoente,datacronologia,usernameaccreditatore," &
				 " idclasseaccreditamento,idtipocronologia,forzatura) select" &
				 " " & Session("IdEnte") & "," & intstato & ",getdate(),'" & ClsServer.NoApice(Session("Utente")) & "'," &
				 " (select idclasseaccreditamento from enti where idente=" & Session("IdEnte") & "),0," & bytForzaturaEnte & "" &
				 " from statienti where Sospeso=1"
				Dim CmdInsCronologiaEnti As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				CmdInsCronologiaEnti.ExecuteNonQuery()
				CmdInsCronologiaEnti.Dispose()


				'Aggiunto da Alessandra Taballione il 14/11/2005
				'Anche Se sono un Ente Padre Cancello sedi e sedi di attuazione
				'sedi 
				strquery = " Update entisedi set idstatoentesede=2 " &
				" from entisedi " &
				" where idente = " & Session("idEnte") & ""
				Dim CmdEliminaSedi As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				CmdEliminaSedi.ExecuteNonQuery()
				CmdEliminaSedi.Dispose()

				'Sedi Attuazione
				strquery = " Update entisediattuazioni set idstatoentesede=2 " &
				" from entisediattuazioni " &
				" where identesede in " &
				" (select identesede from entisedi " &
				" where idente =" & Session("IdEnte") & ")"
				Dim CmdEliminaSediAttuazione As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				CmdEliminaSediAttuazione.ExecuteNonQuery()
				CmdEliminaSediAttuazione.Dispose()


				'Aggiunto da Alessandra Taballione il 11/07/2005
				'Se sono un Ente figlio Cancello sedi e sedi di attuazione
				'sedi 
				strquery = " Update entisedi set idstatoentesede=2 " &
				" from entisedi " &
				" where idente in (select identefiglio from entirelazioni where identeFiglio=" & Session("idEnte") & ")"
				Dim CmdEliminaSediProprie As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				CmdEliminaSediProprie.ExecuteNonQuery()
				CmdEliminaSediProprie.Dispose()

				'Sedi Attuazione
				strquery = " Update entisediattuazioni set idstatoentesede=2 " &
				" from entisediattuazioni " &
				" where identesede in " &
				" (select identesede from entisedi " &
				" where idente in (select identefiglio from entirelazioni where identefiglio=" & Session("IdEnte") & "))"
				Dim CmdEliminaSediAttuazioneProprie As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				CmdEliminaSediAttuazioneProprie.ExecuteNonQuery()
				CmdEliminaSediAttuazioneProprie.Dispose()
				msgConferma.Text = "L'ente è stato chiuso."

				Exit Sub
			End If
		Loop
	End Sub

	Protected Sub imgAnagraficaSistemi_Click(sender As Object, e As EventArgs) Handles imgAnagraficaSistemi.Click
		Dim arrivo As String = Request.QueryString("Arrivo")
		Dim vediEnte As String = Request.QueryString("VediEnte")
		Dim identePadre As String = Request.QueryString("identePadre")
		Dim url As String = "WfrmAnagraficaSistemi.aspx?Id=" & idEnte & "&Blocco=FALSE" & "&tipologia=" & tipologia & "&Arrivo=" & arrivo & "&VediEnte=" & vediEnte & "&identePadre=" & identePadre
		If True Or Session("TipoUTente") <> "U" Then
			url = "WfrmStrutturaOrganizzativaSistemi.aspx"
		End If
		Response.Redirect(url)
	End Sub


	Sub CaricaCausaliRiserva()

		Dim dtsCausaliRiserva As DataSet
		Dim strSql As String

		strSql = "select Causali.IDCausale, Descrizione, IdEnteCausaleRiserva FROM Causali left join EntiCausaliRiserva a on Causali.IDCausale = a.IDCausale  and IdEnte = " & Session("IdEnte") & " where tipo=11 order by Causali.IDCausale"

		dtsCausaliRiserva = ClsServer.DataSetGenerico(strSql, Session("conn"))
		'controllo se ci sono dei record
		If dtsCausaliRiserva.Tables(0).Rows.Count > 0 Then
			dtgCausaliRiserva.DataSource = dtsCausaliRiserva
			dtgCausaliRiserva.DataBind()
		End If
	End Sub

	Sub CaricaRiservaSel()

		Dim dtrRiserva As SqlClient.SqlDataReader
		Dim strSql As String
		Dim item As DataGridItem
		strSql = "select Causali.IDCausale, Descrizione, ISNULL(IdEnteCausaleRiserva,0) AS IdEnteCausaleRiserva FROM Causali left join EntiCausaliRiserva a on Causali.IDCausale = a.IDCausale   and IdEnte = " & Session("IdEnte") & " where tipo=11 order by Causali.IDCausale "
		dtrRiserva = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		While dtrRiserva.Read()
			For Each item In dtgCausaliRiserva.Items
				Dim check As CheckBox = DirectCast(item.FindControl("chkSeleziona"), CheckBox)

				If item.Cells(1).Text() <> "&nbsp;" Then
					If dtrRiserva("IdEnteCausaleRiserva") = item.Cells(1).Text() Then
						check.Checked = True
					End If
				Else
					check.Checked = False
				End If

			Next
		End While
		If Not dtrRiserva Is Nothing Then
			dtrRiserva.Close()
			dtrRiserva = Nothing
		End If
	End Sub

	Private Function VerificaCheck() As Boolean
		'controllo è  stata checcato almeno una causale per il salvataggio
		VerificaCheck = False
		Dim item As DataGridItem
		For Each item In dtgCausaliRiserva.Items
			Dim check As CheckBox = DirectCast(item.FindControl("chkSeleziona"), CheckBox)
			If check.Checked = True Then
				VerificaCheck = True
			End If
		Next
		Return VerificaCheck
	End Function

	Protected Sub btnRiserva_Click(sender As Object, e As EventArgs) Handles btnRiserva.Click
		DivAccreditaRiserva.Visible = True
		BtnAssegna.Visible = True
		CaricaCausaliRiserva()
		CaricaRiservaSel()
	End Sub

	Protected Sub BtnAssegna_Click(sender As Object, e As EventArgs) Handles BtnAssegna.Click
		Dim modins As Integer
		If VerificaModIns() = True Then
			modins = 1 'modifica
		Else
			modins = 0 'inserimento
		End If
		If modins = 0 Then 'INSERIMENTO
			Dim mySql As String
			Dim CmdCausaleRiserva As SqlClient.SqlCommand
			If VerificaCheck() = False Then
				msgErrore.ForeColor = Color.Red
				msgErrore.Text = "E' necessario selezionare almeno una causale riserva."
				Exit Sub
			End If


			Dim check As CheckBox
			Dim IDCausale As String
			Dim IdEnteCausaleRiserva As String
			For i As Integer = 0 To dtgCausaliRiserva.Items.Count - 1
				check = dtgCausaliRiserva.Items.Item(i).FindControl("chkSeleziona")
				IDCausale = dtgCausaliRiserva.Items.Item(i).Cells(0).Text
				IdEnteCausaleRiserva = dtgCausaliRiserva.Items.Item(i).Cells(1).Text
				If check.Checked = True Then


					Try
						mySql = "Insert into  EntiCausaliRiserva (IdEnte ,IdCausale, UsernameInserimento, DataInserimento)  Values (" & Session("IdEnte") & ", '" & IDCausale & "', '" & Session("Utente") & "',  getdate() )"
						CmdCausaleRiserva = New SqlClient.SqlCommand(mySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
						CmdCausaleRiserva.ExecuteNonQuery()

					Catch ex As Exception
						msgErrore.Visible = True
						msgErrore.Text = "Contattare l'assistenza."
					End Try
				End If
			Next

			Dim myQuerySql As String
			Dim CmdRiserva As SqlClient.SqlCommand

			Try
				myQuerySql = "Update Enti Set Riserva = 1, UsernameRiserva='" & Session("Utente") & "', DataRiserva= getdate() where IdEnte= " & Session("IdEnte") & ""
				CmdRiserva = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				CmdRiserva.ExecuteNonQuery()
				msgConferma.Visible = True
				msgConferma.Text = "Modifica effettuata con successo."

			Catch ex As Exception
				msgErrore.Visible = True
				msgErrore.Text = "Contattare l'assistenza."
			End Try




		End If
		If modins = 1 Then 'MODIFICA


			Dim check As CheckBox
			Dim IDCausale As String
			Dim IdEnteCausaleRiserva As String
			Dim mySql As String
			Dim CmdCausaleRiserva As SqlClient.SqlCommand
			Dim zerocheck As Boolean
			For i As Integer = 0 To dtgCausaliRiserva.Items.Count - 1
				check = dtgCausaliRiserva.Items.Item(i).FindControl("chkSeleziona")
				IDCausale = dtgCausaliRiserva.Items.Item(i).Cells(0).Text
				IdEnteCausaleRiserva = dtgCausaliRiserva.Items.Item(i).Cells(1).Text

				If check.Checked = True And IdEnteCausaleRiserva = "&nbsp;" Then 'ho spuntato sulla griglia
					Try
						mySql = "Insert into  EntiCausaliRiserva (IdEnte ,IdCausale, UsernameInserimento, DataInserimento)  Values (" & Session("IdEnte") & ", '" & IDCausale & "', '" & Session("Utente") & "',  getdate() )"
						CmdCausaleRiserva = New SqlClient.SqlCommand(mySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
						CmdCausaleRiserva.ExecuteNonQuery()

					Catch ex As Exception
						msgErrore.Visible = True
						msgErrore.Text = "Contattare l'assistenza."
					End Try

				End If



				If check.Checked = False And IdEnteCausaleRiserva <> "&nbsp;" Then  'sto  togliendo la spunta sulla griglia
					Try
						mySql = "Insert into CronologiaEntiCausaliRiserva (IdEnteCausaleRiserva,IdEnte,IdCausale,UsernameInserimento, DataInserimento, UsernameCancellazione, DataCancellazione) select  IdEnteCausaleRiserva,IdEnte, IdCausale, UsernameInserimento, DataInserimento, '" & Session("Utente") & "' , getdate() FROM  EntiCausaliRiserva where  IdEnteCausaleRiserva = " & IdEnteCausaleRiserva & ""
						CmdCausaleRiserva = New SqlClient.SqlCommand(mySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
						CmdCausaleRiserva.ExecuteNonQuery()


						mySql = "Delete from EntiCausaliRiserva where IdEnte =" & Session("IdEnte") & " and IdEnteCausaleRiserva = " & IdEnteCausaleRiserva & ""
						CmdCausaleRiserva = New SqlClient.SqlCommand(mySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
						CmdCausaleRiserva.ExecuteNonQuery()

					Catch ex As Exception
						msgErrore.Visible = True
						msgErrore.Text = "Contattare l'assistenza."
					End Try


				End If

				If check.Checked = True Then ' se esiste una spunta
					zerocheck = True
				End If

			Next





			Dim myQuerySql As String
			Dim CmdRiserva As SqlClient.SqlCommand

			If zerocheck = False Then 'se ho tolto tutte le spunte
				Try
					myQuerySql = "Update Enti Set Riserva = 0, UsernameNoRiserva='" & Session("Utente") & "', DataNoRiserva= getdate() where IdEnte= " & Session("IdEnte") & ""
					CmdRiserva = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
					CmdRiserva.ExecuteNonQuery()
					msgConferma.Visible = True
					msgConferma.Text = "Modifica effettuata con successo."

				Catch ex As Exception
					msgErrore.Visible = True
					msgErrore.Text = "Contattare l'assistenza."
				End Try



			Else


				Dim dtrTrueFalse As SqlClient.SqlDataReader
				Dim strSql As String
				Dim item As DataGridItem
				strSql = "select Riserva FROM enti where IdEnte = " & Session("IdEnte") & " "
				dtrTrueFalse = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
				dtrTrueFalse.Read()

				If dtrTrueFalse("Riserva") = True Then
					If Not dtrTrueFalse Is Nothing Then
						dtrTrueFalse.Close()
						dtrTrueFalse = Nothing
					End If
					CaricaCausaliRiserva()
					CaricaRiservaSel()

					BtnAssegna.Visible = False
					msgErrore.Text = ""
					Exit Sub

				End If


				If Not dtrTrueFalse Is Nothing Then
					dtrTrueFalse.Close()
					dtrTrueFalse = Nothing
				End If



				Try
					myQuerySql = "Update Enti Set Riserva = 1, UsernameRiserva='" & Session("Utente") & "', DataRiserva= getdate() where IdEnte= " & Session("IdEnte") & ""
					CmdRiserva = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
					CmdRiserva.ExecuteNonQuery()
					msgConferma.Visible = True
					msgConferma.Text = "Modifica effettuata con successo."
				Catch ex As Exception
					msgErrore.Visible = True
					msgErrore.Text = "Contattare l'assistenza."
				End Try

			End If



		End If
		CaricaCausaliRiserva()
		CaricaRiservaSel()

		BtnAssegna.Visible = False
		msgErrore.Text = ""
	End Sub

	Function VerificaModIns()

		Dim dtrRiserva As SqlClient.SqlDataReader
		Dim strSql As String
		Dim item As DataGridItem
		strSql = "select Causali.IDCausale, Descrizione, IdEnteCausaleRiserva FROM Causali left join EntiCausaliRiserva a on Causali.IDCausale = a.IDCausale where tipo=11 and IdEnte = " & Session("IdEnte") & " order by Causali.IDCausale "
		dtrRiserva = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		While dtrRiserva.Read()

			If dtrRiserva("IdEnteCausaleRiserva") > 0 Then
				If Not dtrRiserva Is Nothing Then
					dtrRiserva.Close()
					dtrRiserva = Nothing
				End If
				Return True
				Exit Function

			End If
		End While

		If Not dtrRiserva Is Nothing Then
			dtrRiserva.Close()
			dtrRiserva = Nothing
		End If

		Return False
	End Function

	Protected Sub CmdAnnullaRiservaSiNo_Click(sender As Object, e As EventArgs) Handles CmdAnnullaRiservaSiNo.Click
		DivAccreditaRiserva.Visible = False
	End Sub
	Protected Sub OpenWindow()
		Dim dtrCodFisc As SqlClient.SqlDataReader
		Dim strSql As String
		Dim codfis As String
		ChiudiDataReader(dtrCodFisc)

		strSql = "Select codicefiscale from enti where idente = " & Session("IdEnte")
		dtrCodFisc = ClsServer.CreaDatareader(strSql, Session("Conn"))
		If dtrCodFisc.HasRows = True Then
			dtrCodFisc.Read()
			codfis = dtrCodFisc("codicefiscale")
		End If
		ChiudiDataReader(dtrCodFisc)


		Dim url As String = "WfrmNavigaEnte.aspx?CodiceFiscale=" & codfis
		Dim s As String = "window.open('" & url + "', 'popup_window', 'width=1024,height=500,left=100,top=100,scrollbars=yes,resizable=yes');"
		Page.ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

	End Sub
	Protected Sub CmdInforScu2_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles CmdInforScu2.Click
		OpenWindow()
	End Sub


	Private Sub ImgCompleta_Click(sender As Object, e As System.EventArgs) Handles ImgCompleta.Click


		myDataReader = ClsServer.CreaDatareader("select idclasseaccreditamento from enti where FlagAccreditamentoCompleto = 1 AND idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

		'verifico se è possibile accreditare verificando diversita idC.A. con idC.A.R.
		If myDataReader.HasRows Then
			msgConferma.Text = "La conferma della valutazione dell'ente è già stata effettuata."
			ChiudiDataReader(myDataReader)
			Exit Sub
		End If

		ChiudiDataReader(myDataReader)

		Dim bytForzaturaEnte, bytFEStorico As Byte
		Dim intstato As Int64

		myDataReader = ClsServer.CreaDatareader("select idclasseaccreditamento," &
				" idclasseaccreditamentorichiesta,idstatoente,forzatura from enti" &
				" where idente=" & Session("idente") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		'verifico se è possibile accreditare verificando diversita idC.A. con idC.A.R.
		myDataReader.Read()
		intstato = myDataReader.GetValue(2)
		bytFEStorico = myDataReader.GetValue(3)
		Select Case TrwVincoli.Nodes.Item(0).Value
			Case Is = NODE_VALUE_FOREST_GREEN
				bytForzaturaEnte = 0
			Case Is = NODE_VALUE_GREEN
				bytForzaturaEnte = 0
			Case Else
				bytForzaturaEnte = 1
		End Select
		ChiudiDataReader(myDataReader)

		CancellaMessaggiInfo()
		Dim idStatoEnte As Int64
		Dim BlnEnteFiglio As Boolean

		Dim strRitornoStore As String
		strRitornoStore = LeggiStoreVerificaValutazioneAccreditamento(Session("IdEnte"))
		If strRitornoStore <> "" Then
			msgErrore.Text = strRitornoStore
			Exit Sub
		End If

		If ControllaAccreditamentoRisorse() = False Then
			msgErrore.Text = "L'Ente non può essere Accreditato perchè alcune Risorse non risultano ancora valutate."
			Exit Sub
		End If
		If ControllaAccreditamentoPadre() = False Then
			msgErrore.Text = "L'Ente non può essere Accreditato perchè l'Ente Principale non risulta in Valutazione."
			Exit Sub
		End If
		'Controllo se esistono ancora servizi acquisiti da valutare
		If ControlloServizi() = True Then
			msgErrore.Text = "L'Ente non può essere Accreditato perchè esistono ancora servizi acquisiti in Valutazione."
			Exit Sub
		End If

		Dim strquery1 As String
		strquery1 = "Update entisediattuazioni  set usernamestato = '" & Session("Utente") & "' , dataultimostato = getdate(), idstatoentesede = 1 from entisediattuazioni A INNER JOIN ENTISEDI B ON A.IDENTESEDE = B.IDENTESEDE where A.IDSTATOENTESEDE  = 4 and substring(A.usernamestato,1,1) <> 'N' AND A.CERTIFICAZIONE = 1 AND B.IDENTE = " & Session("idEnte")
		Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		'Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, Session("Conn"))
		CmdUpDateSediAttuazione.ExecuteNonQuery()
		CmdUpDateSediAttuazione.Dispose()

		Dim strquery2 As String
		strquery2 = "Update entisediattuazioni  set usernamestato = '" & Session("Utente") & "' , dataultimostato = getdate(), idstatoentesede = 2 from entisediattuazioni A INNER JOIN ENTISEDI B ON A.IDENTESEDE = B.IDENTESEDE where A.IDSTATOENTESEDE = 4 and substring(A.usernamestato,1,1) <> 'N' AND A.CERTIFICAZIONE = 0 AND B.IDENTE = " & Session("idEnte")
		Dim CmdUpDateSediAttuazioneBis As New Data.SqlClient.SqlCommand(strquery2, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		CmdUpDateSediAttuazioneBis.ExecuteNonQuery()
		CmdUpDateSediAttuazioneBis.Dispose()

		Dim strquery3 As String
		strquery3 = "UPDATE ENTISEDI  SET usernamestato = '" & Session("Utente") & "' , dataultimostato = getdate(), idstatoentesede = 1 FROM ENTISEDI B INNER JOIN ENTISEDIATTUAZIONI A ON A.IDENTESEDE = B.IDENTESEDE WHERE B.IDENTE = " & Session("idEnte") & " And B.IDSTATOENTESEDE = 4 And A.IDSTATOENTESEDE = 1 "
		Dim CmdUpDateSedi As New Data.SqlClient.SqlCommand(strquery3, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		CmdUpDateSedi.ExecuteNonQuery()
		CmdUpDateSedi.Dispose()

		Dim strquery4 As String
		strquery4 = "UPDATE ENTISEDI SET usernamestato = '" & Session("Utente") & "' , dataultimostato = getdate(), idstatoentesede = 2 FROM ENTISEDI B  INNER JOIN ENTISEDIATTUAZIONI A ON A.IDENTESEDE = B.IDENTESEDE WHERE B.IDENTE = " & Session("idEnte") & " And B.IDSTATOENTESEDE  = 4 "
		Dim CmdUpDateSediBis As New Data.SqlClient.SqlCommand(strquery4, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		CmdUpDateSediBis.ExecuteNonQuery()
		CmdUpDateSediBis.Dispose()



		'marco valutazione accreditamento come completa 
		myQuerySql = "Update Enti Set FlagAccreditamentoCompleto = 1,UsernameAccreditamentoComlpeto='" & Session("Utente") & "' ,DataAccreditamentoCompleto= getdate() Where IdEnte = " & Session("IdEnte")
		Dim CmdUpdate As New Data.SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		CmdUpdate.ExecuteNonQuery()
		CmdUpdate.Dispose()

		'aggiornamenti ulteriori (documenti tutti validi, indirizzi tutti validi, codice provvisorio diventa codice definitivo
		myQuerySql = "exec dbo.SP_AGGIORNADATIISCRIZIONE " & Session("IdEnte")
		'Calcolo Codice fiscale Ente
		Dim codice As String = Session("CodiceRegioneEnte")
		Dim dtrEnte = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If dtrEnte.Read() Then
			codice = dtrEnte(0)
			Session("CodiceRegioneEnte") = codice
		End If
		dtrEnte.Close()

		AbilitaPulsantiFunzione(Session("IdEnte"))

		msgConferma.Text = "Valutazione completata con successo: &egrave; stato assegnato il nuovo codice <b>" & codice & "</b>. E' ora possibile produrre i Decreti di iscrizione."

	End Sub

	Private Function GetData(data As Object) As Object
		If IsDBNull(data) Then
			Return Nothing
		End If
		Return data
	End Function
End Class

