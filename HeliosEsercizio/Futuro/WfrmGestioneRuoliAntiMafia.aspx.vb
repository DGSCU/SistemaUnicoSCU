Imports System.Security.Cryptography
Imports Futuro.RiepilogoAccreditamento
Imports Logger.Data
Public Class WfrmGestioneRuoliAntiMafia
	Inherits SmartPage

	Const INDEX_DGRISULTATORICERCA_IDRUOLOANTIMAFIA As Byte = 6
	Private Const PATH_DOC_RIEPILOGO As String = "download\Master\Iscrizione\RiepilogoAntimafia.docx"
	Private Const PATH_DOC_RIEPILOGO_COORDINATORE As String = "download\Master\Iscrizione\RiepilogoAntimafiacOORDINATORE.docx"
	Private Const TIPO_RAPPRESENTANTE_LEGALE As String = "Rappresentante Legale"
	Private Const TIPO_COORDINATORE_RESPONSABILE As String = "Coordinatore Responsabile del Servizio Civile Universale"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		checkSpid()
		'controllo se è stato effettuato il login
		If Not Session("LogIn") Is Nothing Then
			'se non è stato effettuato login
			If Session("LogIn") = False Then
				'carico la pagina LogOut dove svuoto eventuali session aperte
				Response.Redirect("LogOn.aspx")
			End If
		Else
			'carico la pagina LogOut dove svuoto eventuali session aperte
			Response.Redirect("LogOn.aspx")
		End If

		'--- IMPORTANTE!!! INSERIRE CONTROLLO PERMESSI PER ACCESSO MASCHERA E PER EVENTUALI DATI IN QUERY STRING

		If Session("Denominazione") = "" Then
			lgContornoPagina.InnerText = "Gestione Ruoli Antimafia"
		Else
			lgContornoPagina.InnerText = "Gestione Ruoli Antimafia Ente - " & Session("Denominazione")
		End If

		If Not Page.IsPostBack Then

			Session("DocumentoDaFirmare") = Nothing

			'Controlli accesso/abilitazioni
			Dim _info As New clsRuoloAntimafia.InfoAdeguamentoAntimafia(Session("IdEnte"), Session("conn"), False)

			If Not _info.Trovato Then
				'errore nei dati, visualizzo solo un messaggio di errore
				lblMessaggio.Text = "ERRORE NEI DATI, ENTE NON TROVATO"
				divPrincipale.Visible = False
				Exit Sub
			ElseIf Not _info.isEntePrivato And Not _info.isEnteTitolare Then
				'la funzionalità non è abilitata per enti pubblici che non sono titolari
				lblMessaggio.Text = "FUNZIONALITA' NON DISPONIBILE PER ENTI PUBBLICI NON TITOLARI"
				divPrincipale.Visible = False
				pnlApriFaseAntimafia.Visible = False
				pnlAnnullaFaseAntimafia.Visible = False
				pnlChiudiFaseAntimafia.Visible = False
				Exit Sub
			Else
				cmdInserisci.Visible = _info.isAperto
				btnAutocertificazioni.Visible = _info.isAperto
				If Not _info.isEnteTitolare Then
					'devo rendere invisibili i tasti inizio adeguamento/fine adeguamento
					btnAvviaAdeguamentoAntimafia.Visible = False
					btnTerminaAdeguamentoAntimafia.Visible = False
				ElseIf _info.isAperto Then
					If _info.isAccreditamento Then
						btnAvviaAdeguamentoAntimafia.Visible = False
						btnTerminaAdeguamentoAntimafia.Visible = False
					Else
						btnAvviaAdeguamentoAntimafia.Visible = False
						btnTerminaAdeguamentoAntimafia.Visible = True
					End If
				Else
					btnAvviaAdeguamentoAntimafia.Visible = True
					btnTerminaAdeguamentoAntimafia.Visible = False
				End If
				btnAnnullaAdeguamentoAntimafia.Visible = btnTerminaAdeguamentoAntimafia.Visible
				pnlChiudiFaseAntimafia.Visible = btnTerminaAdeguamentoAntimafia.Visible
				pnlAnnullaFaseAntimafia.Visible = btnTerminaAdeguamentoAntimafia.Visible
				pnlApriFaseAntimafia.Visible = btnAvviaAdeguamentoAntimafia.Visible
			End If

			hIdEnteFaseAntimafia.Value = _info.IdEnteFaseAntimafia.ToString
			hdsElencoRuoliAntiMafia.Value = Guid.NewGuid().ToString

			Dim SelRuoloAntimafia As New clsRuoloAntimafia
			SelRuoloAntimafia.CaricaDdlEntiByIdEntePadre(ddlEnti, Session("IdEnte"), Session("Conn"), True)

			If Request.QueryString("FiltroEnte") <> Nothing Then
				ddlEnti.SelectedValue = Request.QueryString("FiltroEnte")
			End If

			'Solo ruoli per enti privati
			'Dim _privato = IsEntePrivato(ddlEnti.SelectedValue)
			Dim _privato = 1

			SelRuoloAntimafia = New clsRuoloAntimafia
			SelRuoloAntimafia.CaricaDdlRuoliAntimafia(ddlRuoliAntiMafia, _privato, Session("Conn"))

			RicercaRuoliAntimafia()

			If Request.QueryString("Messaggio") <> Nothing Then
				lblMessaggio.CssClass = "msgInfo"
				lblMessaggio.Text = Request.QueryString("Messaggio")
			Else
				lblMessaggio.Text = ""  'la prima ricerca senza messaggio
			End If

		End If
	End Sub

	Protected Sub cmdInserisci_Click(sender As Object, e As EventArgs) Handles cmdInserisci.Click
		Response.Redirect("WfrmRuoloAntimafia.aspx?IdRuoloAntimafia=0&FiltroEnte=" & ddlEnti.SelectedValue)
	End Sub

	Sub RicercaRuoliAntimafia()
		Dim _clsR As New clsRuoloAntimafia
		'se gli si passa un idente=0 allora prende l'ente in sessione e tutti gli enti figli, altrimenti solo l'ente selezionato
		Session(hdsElencoRuoliAntiMafia.Value) = _clsR.GetRuoliAntimafia(
			IIf(ddlEnti.SelectedValue = 0, Session("IdEnte"), ddlEnti.SelectedValue), hIdEnteFaseAntimafia.Value,
			Session("Conn"), txtcognome.Text, txtnome.Text, txtcodicefiscale.Text, IIf(ddlRuoliAntiMafia.Items.Count >= 0, ddlRuoliAntiMafia.SelectedItem.Value, 0),
		ddlEnti.SelectedValue = "0", txtCodiceEnte.Text
			)

		dgRisultatoRicerca.DataSource = Session(hdsElencoRuoliAntiMafia.Value)
		dgRisultatoRicerca.DataBind()

		If (dgRisultatoRicerca.Items.Count > 0) Then
			dgRisultatoRicerca.Caption = "Risultato Ricerca Ruoli Antimafia"
		Else
			'dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato"
			lblMessaggio.Text = "La ricerca non ha prodotto alcun risultato"
			dgRisultatoRicerca.Caption = "INSERISCI RUOLI ANTIMAFIA"
		End If
	End Sub

	Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
		Select Case e.CommandName
			Case "Select"
				Dim idRuoloAntimafia As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_IDRUOLOANTIMAFIA).Text

				Response.Redirect("WfrmRuoloAntimafia.aspx?IdRuoloAntimafia=" & idRuoloAntimafia)
		End Select
	End Sub

	Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
		dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
		dgRisultatoRicerca.DataSource = Session(hdsElencoRuoliAntiMafia.Value)
		dgRisultatoRicerca.DataBind()
		lblMessaggio.Text = ""
	End Sub

	Private Function IsEntePrivato(ByVal idEnte As Integer) As Integer
		'La funzione restituisce 1 se ente privato, 0 se ente pubblico, -1 negli altri casi o errore
		Dim dtrTipologia As SqlClient.SqlDataReader
		Dim strSql As String
		Dim _ret As Integer = -1

		Try
			strSql = "Select t.privato from enti e inner join TipologieEnti t on e.tipologia =t.descrizione  where idente =" & idEnte
			dtrTipologia = ClsServer.CreaDatareader(strSql, Session("Conn"))
			If dtrTipologia.HasRows = True Then
				dtrTipologia.Read()
				'If dtrTipologia("Tipologia") = "Privato" Then
				If dtrTipologia("privato") = True Then
					_ret = 1
				ElseIf dtrTipologia("privato") = False Then
					_ret = 0
				End If
			End If
			dtrTipologia.Close()
			dtrTipologia = Nothing

		Catch ex As Exception
			'viene restituito -1 in automatico
		End Try

		Return _ret

	End Function

	Protected Sub btnPulisci_Click(sender As Object, e As EventArgs) Handles btnPulisci.Click
		If ddlEnti.Items.Count > 0 Then ddlEnti.SelectedIndex = 0
		txtcodicefiscale.Text = ""
		txtcognome.Text = ""
		txtnome.Text = ""
		If ddlRuoliAntiMafia.Items.Count >= 0 Then
			ddlRuoliAntiMafia.SelectedIndex = 0
		End If
		RicercaRuoliAntimafia()
		lblMessaggio.Text = ""
	End Sub

	Protected Sub btnRicerca_Click(sender As Object, e As EventArgs) Handles btnRicerca.Click
		lblMessaggio.Text = ""
		RicercaRuoliAntimafia()
	End Sub

	Private Sub ddlEnti_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlEnti.SelectedIndexChanged
		Dim _privato = IsEntePrivato(ddlEnti.SelectedValue)

		Dim SelRuoloAntimafia = New clsRuoloAntimafia
		SelRuoloAntimafia.CaricaDdlRuoliAntimafia(ddlRuoliAntiMafia, _privato, Session("Conn"))
	End Sub

	Protected Sub btnApriFaseAntimafia_Click(sender As Object, e As EventArgs) Handles btnApriFaseAntimafia.Click
		Try
			Dim _ra = New clsRuoloAntimafia
			Dim _ret As String = _ra.AvviaAdeguamentoAntimafia(Session("IdEnte"), Session("Conn"))

			If _ret <> "" Then
				lblMessaggio.CssClass = "msgErrore"
				lblMessaggio.Text = _ret
			Else
				Response.Redirect("WfrmGestioneRuoliAntimafia.aspx?Messaggio=" & "Aggiornamento Antimafia avviato con successo")
			End If
		Catch ex As Exception
			Log.Error(LogEvent.ANTIMAFIA_ERRORE, "Errore nell'avvio dell'aggiornamento Antimafia.", exception:=ex)
			lblMessaggio.CssClass = "msgErrore"
			lblMessaggio.Text = "Errore nell'avvio dell'aggiornamento Antimafia."
		End Try
	End Sub

	Protected Sub btnConfermaChiusuraAntimafia_Click(sender As Object, e As EventArgs) Handles btnConfermaChiusuraAntimafia.Click
		'Verifica se è stato inserito il file
		If fileAntimafia.PostedFile Is Nothing Or String.IsNullOrEmpty(fileAntimafia.PostedFile.FileName) Then
			lblErroreChiusuraAntimafia.Text = ("Non è stato scelto nessun file")
			popUpChiudiFaseAntimafia.Show()
			Exit Sub
		End If
		'Controllo Tipo File
		If Not clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileAntimafia) Then
			lblErroreChiusuraAntimafia.Text = ("Il formato file non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M")
			popUpChiudiFaseAntimafia.Show()
			Exit Sub
		End If
		'Controlli dimensioni del file
		Dim fs = fileAntimafia.PostedFile.InputStream
		Dim iLen As Integer = CInt(fs.Length)
		Dim bBLOBStorage(iLen) As Byte

		If iLen <= 0 Then
			lblErroreChiusuraAntimafia.Text = ("Attenzione. Impossibile caricare documento vuoto.")
			popUpChiudiFaseAntimafia.Show()
			Exit Sub
		End If
		If iLen > 20971520 Then
			lblErroreChiusuraAntimafia.Text = ("Attenzione. La dimensione massima del file è di 20 MB.")
			popUpChiudiFaseAntimafia.Show()
			Exit Sub
		End If


		Dim numBytesToRead As Integer = CType(fs.Length, Integer)
		Dim numBytesRead As Integer = 0

		While (numBytesToRead > 0)
			' Read may return anything from 0 to numBytesToRead.
			Dim n As Integer = fs.Read(bBLOBStorage, numBytesRead, numBytesToRead)
			' Break when the end of the file is reached.
			If (n = 0) Then
				Exit While
			End If
			numBytesRead = (numBytesRead + n)
			numBytesToRead = (numBytesToRead - n)

		End While

		fs.Close()

		Dim managerEnte = New clsEnte()

		Dim _documentoDaFirmare As Byte() = Session("DocumentoDaFirmare")

		If IsNothing(_documentoDaFirmare) Then
			Log.Warning(LogEvent.FIRMA_NON_VALIDA, message:="Non è stata scaricata la ComunicazioneAntimafia da firmare ed allegare")
			lblErroreChiusuraAntimafia.Text = ("Non è stata scaricata la ComunicazioneAntimafia da firmare ed allegare")
			popUpChiudiFaseAntimafia.Show()
			Exit Sub
		End If

		If Not ConfigurationSettings.AppSettings("DisabilitaFirma") = "true" Then

			Dim isRappresentanteLegale As Boolean = ddlTipoRappresentante.SelectedValue = TIPO_RAPPRESENTANTE_LEGALE

			Dim responsabile As String
			If isRappresentanteLegale Then
				Dim rappresentanteLegale = managerEnte.GetRappresentanteLegale(Session("IdEnte"), Session("conn"))
				If rappresentanteLegale Is Nothing Then
					lblErroreChiusuraAntimafia.Text = ("Non è presente il Rappresentante Legale dell'Ente")
					Exit Sub
				End If
				responsabile = rappresentanteLegale.CodiceFiscale

			Else
				Dim coordinatore = managerEnte.GetCoordinatoreResponsabile(Session("IdEnte"), Session("conn"))
				If coordinatore Is Nothing Then
					lblErroreChiusuraAntimafia.Text = ("Non è presente il " + TIPO_COORDINATORE_RESPONSABILE)
					Exit Sub
				End If
				responsabile = coordinatore.CodiceFiscale

			End If


			Dim sc As New SignChecker(bBLOBStorage)
			If Not sc.checkSignature(responsabile) Then
				Log.Warning(LogEvent.FIRMA_NON_VALIDA, sc.getLog())
				lblErroreChiusuraAntimafia.Text = ("Il documento non è firmato digitalmente o non è firmato dal " & ddlTipoRappresentante.SelectedValue)
				popUpChiudiFaseAntimafia.Show()
				Exit Sub
			End If

			If Not sc.compareSignedNotSigned(bBLOBStorage, _documentoDaFirmare) Then
				Log.Warning(LogEvent.FIRMA_NON_VALIDA, message:="Il file firmato non corrisponde alla ComunicazioneAntimafia da firmare ed allegare")
				lblErroreChiusuraAntimafia.Text = ("Il file firmato non corrisponde alla ComunicazioneAntimafia da firmare ed allegare")
				popUpChiudiFaseAntimafia.Show()
				Exit Sub
			End If

		End If


		Dim ente = managerEnte.GetDatiEnte(Session("IdEnte"), Session("conn"))

		Dim hashValue As String
		hashValue = GeneraHash(bBLOBStorage)

		Dim estensione As String = System.IO.Path.GetExtension(fileAntimafia.PostedFile.FileName)
		If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

		Dim allegato As New Allegato With {
		 .Blob = bBLOBStorage,
		 .Filename = "DOMANDAANTIMAFIA_" & ente.CodiceFiscale & estensione,
		 .Hash = hashValue,
		 .DataInserimento = Date.Now,
		 .Filesize = iLen
		}
		Try
			Dim _ra = New clsRuoloAntimafia
			Dim _ret As String = _ra.TerminaAdeguamentoAntimafia(Session("IdEnte"), Session("Conn"), allegato, If(Session("CodiceFiscaleUtente"), Session("Account")))

			If _ret <> "" Then
				lblMessaggio.CssClass = "msgErrore"
				lblMessaggio.Text = _ret
			Else
				Response.Redirect("WfrmGestioneRuoliAntimafia.aspx?Messaggio=" & "Aggiornamento Antimafia terminato con successo")
			End If
		Catch ex As Exception
			Log.Error(LogEvent.ANTIMAFIA_ERRORE, "Errore nel terminare l'Aggiornamento Antimafia.", exception:=ex)
			lblMessaggio.CssClass = "msgErrore"
			lblMessaggio.Text = "Errore nel terminare l'Aggiornamento Antimafia."
		End Try
	End Sub

	Sub CaricaBoxInfoAdeguamentoAntimafia()
		Dim _ra As New clsRuoloAntimafia()
		Response.Write(_ra.GetBoxInfoAntimafia(Session("IdEnte"), Session("conn"), False))
	End Sub

	Protected Sub btnScaricaComunicazioneAntimafia_Click(sender As Object, e As EventArgs)

		Dim documento As New AsposeWord
		Dim responsabile As Risorsa
		Dim managerEnte = New clsEnte()
		Try
			Dim isRappresentanteLegale As Boolean = ddlTipoRappresentante.SelectedValue = TIPO_RAPPRESENTANTE_LEGALE
			If isRappresentanteLegale Then
				documento.open(Server.MapPath(PATH_DOC_RIEPILOGO))
				responsabile = managerEnte.GetRappresentanteLegale(Session("IdEnte"), Session("conn"))
			Else
				documento.open(Server.MapPath(PATH_DOC_RIEPILOGO_COORDINATORE))
				responsabile = managerEnte.GetCoordinatoreResponsabile(Session("IdEnte"), Session("conn"))
			End If
		Catch ex As Exception
			Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Template non valido", ex)
			lblMessaggio.Text = "Errore nella generazione della Comunicazione antimafia"
			Exit Sub
		End Try


		Dim managerAntimafia As New clsRuoloAntimafia()
		Dim ruoli = managerAntimafia.GetRuoliAntimafia(Session("IdEnte"), Session("conn"))
		Dim ente = managerEnte.GetDatiEnte(Session("IdEnte"), Session("conn"))
		documento.addFieldValue("NomeEnte", ente.Denominazione)
		documento.addFieldValue("CodiceFiscaleEnte", ente.CodiceFiscale)
		documento.addFieldValue("TipoRappresentante", ddlTipoRappresentante.SelectedValue)

		If responsabile IsNot Nothing Then
			documento.addFieldValue("RappresentanteLegaleNome", responsabile.Nome.ToUpper())
			documento.addFieldValue("RappresentanteLegaleLuogoNascita", responsabile.LuogoNascita)
			Dim indirizzo As String = ""
			If Not String.IsNullOrEmpty(responsabile.IndirizzoResidenza) Then
				indirizzo = " e residente in " + responsabile.IndirizzoResidenza
			End If
			documento.addFieldValue("RappresentanteLegaleIndirizzo", indirizzo)
			If responsabile.DataNascita.HasValue Then
				documento.addFieldValue("RappresentanteLegaleDataNascita", responsabile.DataNascita.Value.ToString("dd/MM/yyyy"))
			End If
		End If
		If ente.SedeLegale IsNot Nothing Then
			documento.addFieldValue("IndirizzoEnte", ente.SedeLegale.Indirizzo)
		End If
		ente.SedeLegale = managerEnte.GetSedeLegale(Session("IdEnte"), Session("conn"))

		If ente.SedeLegale IsNot Nothing Then
			documento.addFieldValue("IndirizzoEnte", ente.SedeLegale.Indirizzo)
		End If

		documento.addFieldValue("Data", Date.Today.ToString("dd/MM/yyyy"))
		documento.addFieldValue("CodiceEnte", ente.CodiceEnte)

		Dim html As New StringBuilder
		html.Append("<style>")
		html.Append("table {width:100%; border-collapse: collapse; font-size:10pt; margin-bottom:1em;}")
		html.Append("table, th, td {border: 1px solid lightgray;}")
		html.Append("table tr:nth-child(even) {background-color:#eee}")
		html.Append("td {padding:1pt; font-family:'courier new';}")
		html.Append(".space {width:50%;height:1em;}")
		html.Append("</style>")

		html.Append("<table><tbody>")
		html.Append("<tr style='fort-weight:bold'>")
		html.Append("<th>Codice Fiscale Ente</th>")
		html.Append("<th>Codice Fiscale</th>")
		html.Append("<th>Cognome</th>")
		html.Append("<th>Nome</th>")
		html.Append("<th>Ruolo</th>")
		html.Append("</tr>")
		Dim rigaPari As Boolean = True
		For Each ruolo In ruoli
			html.Append("<tr>")
			html.Append("<td>" & ruolo.CodiceFiscaleEnte & "</td>")
			html.Append("<td>" & ruolo.CodiceFiscale & "</td>")
			html.Append("<td>" & ruolo.Cognome & "</td>")
			html.Append("<td>" & ruolo.Nome & "</td>")
			html.Append("<td>" & ruolo.Ruolo & "</td>")
			html.Append("</tr>")
		Next

		html.Append("</tbody></table>")
		html.Append("</br>")

		documento.addFieldHtml("htmlAntimafia", html.ToString)

		Try
			documento.merge()

			Session("DocumentoDaFirmare") = documento.pdfBytes

		Catch ex As Exception
			Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Scrittura template", ex)
			lblMessaggio.Text = "Errore nella generazione della Domanda di Iscrizione"
			Exit Sub
		End Try
		Log.Information(LogEvent.PRESENTAZIONE_RIEPILOGO)
		Response.Clear()
		Response.ContentType = "Application/pdf"
		Response.AddHeader("Content-Disposition", "attachment; filename=" & "ComunicazioneAntimafia.pdf")
		Response.BinaryWrite(documento.pdfBytes)
		Response.End()

	End Sub

	Private Function GeneraHash(ByVal FileinByte() As Byte) As String
		Dim tmpHash() As Byte

		tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

		GeneraHash = ByteArrayToString(tmpHash)
		Return GeneraHash
	End Function
	Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
		Dim i As Integer
		Dim sOutput As New StringBuilder(arrInput.Length)
		For i = 0 To arrInput.Length - 1
			sOutput.Append(arrInput(i).ToString("X2"))
		Next
		Return sOutput.ToString()
	End Function

	Protected Sub btnAnnullaFaseAntimafia_Click(sender As Object, e As EventArgs)
		Dim _ret As String
		Try
			Dim _ra = New clsRuoloAntimafia
			_ret = _ra.AnnullaFaseAntimafia(Session("IdEnte"), Session("Conn"))
		Catch ex As Exception
			Log.Error(LogEvent.ANTIMAFIA_ERRORE, "Errore nell'annullamento dell'aggiornamento Antimafia.", exception:=ex)
			_ret = "Errore nell'annullamento dell'Aggiornamento Antimafia."
		End Try

		If _ret <> "" Then
			lblMessaggio.CssClass = "msgErrore"
			lblMessaggio.Text = _ret
		Else
			Response.Redirect("WfrmGestioneRuoliAntimafia.aspx?Messaggio=" & "Aggiornamento Antimafia annullato con successo")
		End If
	End Sub

	Protected Sub btnAutocertificazioni_Click(sender As Object, e As EventArgs)
		Response.Redirect("Autocertificazioni.aspx")
	End Sub

	Protected Sub btnTerminaAdeguamentoAntimafia_Click(sender As Object, e As EventArgs)
		Dim manager = New clsRuoloAntimafia()
		Dim errori = ""

		If Not manager.ControllaEsistenzaRuoliAntimafia(Session("IdEnte"), Session("conn")) Then
			errori = "Non sono presenti Ruoli Antimafia."
		Else
			If Not manager.ControllaRuoliAntimafia(Session("IdEnte"), Session("conn")) Then
				errori += "Non sono presenti Ruoli Antimafia per tutti gli enti privati.<br/>"
			End If
			If Not manager.ControllaAutocertificazioni(Session("IdEnte"), Session("conn")) Then
				errori += "Non sono state inserite le autocertificazioni per tutti gli enti privati.<br/>"
			End If
		End If
		If errori = "" Then
			popUpChiudiFaseAntimafia.Show()
		Else
			lblMessaggio.CssClass = "msgErrore"
			lblMessaggio.Text = errori
		End If
	End Sub

	Protected Sub ddlTipoRappresentante_SelectedIndexChanged(sender As Object, e As EventArgs)
		If Session("DocumentoDaFirmare") IsNot Nothing Then
			Session("DocumentoDaFirmare") = Nothing
            lblErroreChiusuraAntimafia.Text = ("Attenzione! È necessario scaricare nuovamente il documento di riepilogo a seguito della variazione del firmatario")
		End If
		popUpChiudiFaseAntimafia.Show()
	End Sub
End Class