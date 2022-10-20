Imports System.Data.SqlClient
Imports System.IO
Imports System.Collections.Generic
Imports Futuro.RiepilogoAccreditamento

Public Class clsRuoloAntimafia
	'Questa classe contiene sia le funzionalità dell'import massivo sia le funzionalità per gestire le maschere di visualizzazione, inserimento etc. etc.

	Const NumeroCampiCSV = 16
	Const IndiceCSVCodiceFiscaleEnte = 0
	Const IndiceCSVCodiceFiscale = 1
	Const IndiceCSVCognome = 2
	Const IndiceCSVNome = 3
	Const IndiceCSVRuoloAntimafia = 4
	Const IndiceCSVDataNascita = 5
	Const IndiceCSVProvinciaNascita = 6
	Const IndiceCSVCittaNascita = 7
	Const IndiceCSVProvinciaResidenza = 8
	Const IndiceCSVCittaResidenza = 9
	Const IndiceCSVVia = 10
	Const IndiceCSVCivico = 11
	Const IndiceCSVCAP = 12
	Const IndiceCSVTelefono = 13
	Const IndiceCSVPEC = 14
	Const IndiceCSVEmail = 15

	Dim _listaProvincie As LProvincie
	Dim _listaComuni As LComuni
	Dim _listaRuoliAntimafia As LElencoRuoliAntiMafia
	Dim _listaCodiciFiscaliEnte As LCodiciFiscaliEnte
    Dim doppioComune As String = ""
	' INIZIO funzionalità import massivo
	Public Class Provincia
		Public Id As Integer
		Public Denominazione As String
	End Class

	Class Comune
		Public Id As Integer
		Public IdProvincia As Integer
		Public CodiceCatastale As String
		Public Denominazione As String
		Public CAP As String
		Public Multicap As Boolean
		Public Dismesso As Boolean
	End Class

	Public Class ElencoRuoliAntimafia
		Public Id As Integer
		Public Denominazione As String
		Public Privato As Boolean
	End Class

	Class CfEnte
		Public IdEnte As Integer
		Public CodiceFiscaleEnte As String
		Public Privato As Boolean
	End Class


	Public Class RigaRuoloDaInserire
		Public Riga As Integer
		Public IdEnte As Integer
		Public EnteCSV As String
		Public CodiceFiscale As String
		Public Cognome As String
		Public Nome As String
		Public IdElencoRuoliAntimafia As Integer
		Public RuoloAntimafiaCSV As String
		Public DataNascita As Date
		Public IdComuneNascita As Integer
		Public IdComuneResidenza As Integer
		Public IndirizzoResidenza As String
		Public NumeroCivicoResidenza As String
		Public CAPResidenza As String
		Public Telefono As String
		Public PEC As String
        Public Email As String
        Public UltimaEsportazioneDati As Date



	End Class

	Public Class CampoRuoloErrore
		Public NomeCampo As String
		Public Valore As String
		Public Errore As String
	End Class

	Public Class RigaRuoloErrore
		Public Riga As Integer
		Public Errori As List(Of CampoRuoloErrore)
	End Class

	Public Class ControlloCSV
		Public RigheCSV As List(Of RigaRuoloDaInserire)
		Public ErroriCSV As List(Of RigaRuoloErrore)
		Public Nomefile As String
		Public File As Byte() 'Se il file è in formato corretto contiene il file
		Public Sub New()
			RigheCSV = New List(Of RigaRuoloDaInserire)
			ErroriCSV = New List(Of RigaRuoloErrore)
		End Sub
	End Class

	Class LCodiciFiscaliEnte
		'Contiene l'elenco dei codici fiscali dell'ente principale e degli enti in accordo
		Public ListaCodiciFiscaliEnte As New List(Of CfEnte)
		Public Sub New(ByVal idEnte As Integer, ByVal conn As SqlClient.SqlConnection)
			Dim strSQL As String = "select IDEnte,CodiceFiscale as CodiceFiscaleEnte, Privato from enti e inner join TipologieEnti t on e.tipologia =t.descrizione where IDEnte=" & idEnte & " union"
			strSQL += " select e.IDEnte,e.CodiceFiscale as CodiceFiscaleEnte, Privato from enti e inner join entirelazioni er on e.IDEnte=er.IDEnteFiglio inner join TipologieEnti t on e.tipologia =t.descrizione"
            strSQL += " where er.IDEntePadre = " & idEnte & " And er.DataFineValidità Is null And e.idStatoEnte in(3,6)"
			Using dtrCfEnti As SqlDataReader = ClsServer.CreaDatareader(strSQL, conn)
				With dtrCfEnti
					While dtrCfEnti.Read()
						ListaCodiciFiscaliEnte.Add(New CfEnte With {.IdEnte = dtrCfEnti("IdEnte"), .CodiceFiscaleEnte = dtrCfEnti("CodiceFiscaleEnte").ToString.ToUpper, .Privato = dtrCfEnti("Privato")})
					End While
					.Close()
				End With
			End Using
		End Sub
	End Class

	Public Class LElencoRuoliAntiMafia
		Public ListaElencoRuoloAntiMafia As New List(Of ElencoRuoliAntimafia)
		Public Sub New(ByVal conn As SqlClient.SqlConnection)
			Dim strSQL As String = "SELECT Id, RuoloAntimafia, Privato FROM ElencoRuoliAntimafia where privato=1"
			Using dtrRuoli As SqlDataReader = ClsServer.CreaDatareader(strSQL, conn)
				With dtrRuoli
					While dtrRuoli.Read()
						ListaElencoRuoloAntiMafia.Add(New ElencoRuoliAntimafia With {.Id = dtrRuoli("Id"), .Denominazione = dtrRuoli("RuoloAntimafia").ToString.ToUpper, .Privato = dtrRuoli("Privato")})
					End While
					.Close()
				End With
			End Using
		End Sub
	End Class

	Class LProvincie
		Public ListaProvincie As New List(Of Provincia)
		Public Sub New(ByVal conn As SqlClient.SqlConnection)
			Dim strSQL As String = "SELECT IdProvincia, Provincia FROM Provincie"
			Using dtrProvincie As SqlDataReader = ClsServer.CreaDatareader(strSQL, conn)
				With dtrProvincie
					While dtrProvincie.Read()
						ListaProvincie.Add(New Provincia With {.Id = dtrProvincie("IdProvincia"), .Denominazione = dtrProvincie("Provincia").ToString.ToUpper})
					End While
					.Close()
				End With
			End Using
		End Sub
	End Class

	Class LComuni
		Public ListaComuni As New List(Of Comune)
		Public Sub New(ByVal conn As SqlClient.SqlConnection)
			Dim strSQL As String = "select IdComune, IdProvincia, isnull(CF,'') CF, Denominazione,isnull(CAP,'') CAP,Multicap, case when isnull(CodiceIstatDismesso,'') <>'' "
			strSQL += "then 1 else 0 end as Dismesso from comuni where isnull(CodiceISTAT,'') <>'' or isnull(CodiceIstatDismesso,'') <>''"
			Using dtrComuni As SqlDataReader = ClsServer.CreaDatareader(strSQL, conn)
				With dtrComuni
					While dtrComuni.Read()
						ListaComuni.Add(New Comune With {
										.Id = dtrComuni("IdComune"),
										.IdProvincia = dtrComuni("IdProvincia"),
										.CodiceCatastale = dtrComuni("CF").ToString.Trim.ToUpper,
										.Denominazione = dtrComuni("Denominazione").ToString.ToUpper,
										.CAP = dtrComuni("CAP"),
										.Multicap = dtrComuni("Multicap"),
										.Dismesso = dtrComuni("Dismesso")
										}
									   )
					End While
					.Close()
				End With
			End Using
		End Sub
	End Class

	Public Function ImportMassivoRuoliAntiMafia(ByVal streamCSV As Stream, ByVal idEnte As Integer, ByRef _risultato As ControlloCSV, ByVal conn As SqlClient.SqlConnection) As String
		'questa funzione importa lo stream nel db
		Dim _ret As String = ""
		Dim _strImport As String()  'Contiene l'array di righe del file importato
		Dim br As New BinaryReader(streamCSV)

		Dim bytes As Byte() = br.ReadBytes(Convert.ToInt32(streamCSV.Length))
        _strImport = Text.Encoding.Default.GetString(bytes).Split(New String() {Environment.NewLine}, StringSplitOptions.None)

		If _strImport.Length = 0 Or Not VerificaHeaderCSV(_strImport(0)) Then
			Return "Formato file non corretto"
		End If

		_risultato.File = bytes
		ControllaCSV(_strImport, _risultato, idEnte, conn)

		If _risultato.ErroriCSV.Count = 0 Then
			'si può procedere con l'importazione
		Else
			'ci sono uno o più errori
		End If

		Return _ret
	End Function

	Sub ControllaCSV(ByVal csvArray As String(), ByRef risultato As ControlloCSV, ByVal idEnte As Integer, ByVal conn As SqlClient.SqlConnection)
		'Carico le liste che mi serviranno per limitare il numero di chiamate a db
		_listaProvincie = New LProvincie(conn)
		_listaComuni = New LComuni(conn)
		_listaRuoliAntimafia = New LElencoRuoliAntiMafia(conn)
		_listaCodiciFiscaliEnte = New LCodiciFiscaliEnte(idEnte, conn)
		'A rigor di logica (per limitare il numero di chiamate a db) ci andrebbe anche la lista indirizzi per il controllo multicap ma, per non replicare i controlli 
		'della SP_CAP_CONTROLLOINDIRIZZI, mio malgrado farò una chiamata per ogni riga del csv (solo se serve) usando la funzione esistente e già usata da altre parti

		Dim _rigaDaInserire As RigaRuoloDaInserire

		Dim i As Integer
		For i = 1 To UBound(csvArray)   'parto da 1 poichè la prima riga è l'header

			Dim _datiPerCfOK As Boolean = True
			Dim _datiPerCapOK As Boolean = True

			'inserisco l'eventuale riga calcolata all'iterazione precedente
			If Not _rigaDaInserire Is Nothing Then risultato.RigheCSV.Add(_rigaDaInserire)
			_rigaDaInserire = Nothing

			If csvArray(i) = "" Then Continue For 'ignoro le righe vuote
			_rigaDaInserire = New RigaRuoloDaInserire

			_rigaDaInserire.Riga = i

			Dim rigaCSV As String() = csvArray(i).ToUpper.Split(";"c).Select((Function(s) s.Trim)).ToArray()    'trimmo e uppo i campi del CSV
			If rigaCSV.Length <> NumeroCampiCSV Then
				AggiungiErrore(risultato.ErroriCSV, i, "Errore generale", "", "Numero campi non valido")
				Continue For
			End If

			Dim _cfente As CfEnte
            _rigaDaInserire.EnteCSV = rigaCSV(IndiceCSVCodiceFiscaleEnte)
			If rigaCSV(IndiceCSVCodiceFiscaleEnte) = "" Then
				AggiungiErrore(risultato.ErroriCSV, i, "CodiceFiscaleEnte", "", "obbligatorio")
            Else
                rigaCSV(IndiceCSVCodiceFiscaleEnte) = Right("00000000000" & rigaCSV(IndiceCSVCodiceFiscaleEnte), 11)
                _rigaDaInserire.EnteCSV = rigaCSV(IndiceCSVCodiceFiscaleEnte)
                _cfente = _listaCodiciFiscaliEnte.ListaCodiciFiscaliEnte.Find(Function(cf) cf.CodiceFiscaleEnte = rigaCSV(IndiceCSVCodiceFiscaleEnte))
                If _cfente Is Nothing Then
                    AggiungiErrore(risultato.ErroriCSV, i, "CodiceFiscaleEnte", rigaCSV(IndiceCSVCodiceFiscaleEnte), "non valido")
                ElseIf Not _cfente.Privato Then
                    AggiungiErrore(risultato.ErroriCSV, i, "CodiceFiscaleEnte", rigaCSV(IndiceCSVCodiceFiscaleEnte), "riferito a un Ente pubblico")
                Else
                    _rigaDaInserire.IdEnte = _cfente.IdEnte
                End If
			End If

			_rigaDaInserire.CodiceFiscale = rigaCSV(IndiceCSVCodiceFiscale)
			If rigaCSV(IndiceCSVCodiceFiscale) = "" Then
				AggiungiErrore(risultato.ErroriCSV, i, "CodiceFiscale", "", "obbligatorio")
				_datiPerCfOK = False
			End If

			_rigaDaInserire.RuoloAntimafiaCSV = rigaCSV(IndiceCSVRuoloAntimafia)
			If rigaCSV(IndiceCSVRuoloAntimafia) = "" Then
				AggiungiErrore(risultato.ErroriCSV, i, "RuoloAntiMafia", "", "obbligatorio")
			Else
				Dim _ruolo As ElencoRuoliAntimafia = _listaRuoliAntimafia.ListaElencoRuoloAntiMafia.Find(Function(r) r.Denominazione = rigaCSV(IndiceCSVRuoloAntimafia) And r.Privato = If(_cfente Is Nothing, -1, _cfente.Privato))
				If _ruolo Is Nothing Then
					AggiungiErrore(risultato.ErroriCSV, i, "RuoloAntiMafia", rigaCSV(IndiceCSVRuoloAntimafia), "non valido")
				Else
					_rigaDaInserire.IdElencoRuoliAntimafia = _ruolo.Id
				End If
			End If

			If rigaCSV(IndiceCSVCognome) = "" Then
				AggiungiErrore(risultato.ErroriCSV, i, "Cognome", "", "obbligatorio")
			Else
				_rigaDaInserire.Cognome = rigaCSV(IndiceCSVCognome)
			End If

			If rigaCSV(IndiceCSVNome) = "" Then
				AggiungiErrore(risultato.ErroriCSV, i, "Nome", "", "obbligatorio")
			Else
				_rigaDaInserire.Nome = rigaCSV(IndiceCSVNome)
			End If

			If rigaCSV(IndiceCSVDataNascita) = "" Then
				AggiungiErrore(risultato.ErroriCSV, i, "DataNascita", "", "obbligatorio")
				_datiPerCfOK = False
			Else
				Dim dataNascita As Date
				If Len(rigaCSV(IndiceCSVDataNascita)) <> 10 Then
					AggiungiErrore(risultato.ErroriCSV, i, "DataNascita", rigaCSV(IndiceCSVDataNascita), "non valido. Inserire la data nel formato GG/MM/AAAA")
					_datiPerCfOK = False
				ElseIf (Date.TryParse(rigaCSV(IndiceCSVDataNascita), dataNascita) = False) Then
					AggiungiErrore(risultato.ErroriCSV, i, "DataNascita", rigaCSV(IndiceCSVDataNascita), "non valido. Inserire la data nel formato GG/MM/AAAA")
					_datiPerCfOK = False
				Else
					_rigaDaInserire.DataNascita = dataNascita
				End If
			End If

			Dim _provinciaNascita As Provincia
			If rigaCSV(IndiceCSVProvinciaNascita) = "" Then
				AggiungiErrore(risultato.ErroriCSV, i, "ProvinciaNascita", "", "obbligatorio")
				_datiPerCfOK = False
			Else
				_provinciaNascita = _listaProvincie.ListaProvincie.Find(Function(p) p.Denominazione = rigaCSV(IndiceCSVProvinciaNascita))
				If _provinciaNascita Is Nothing Then
					AggiungiErrore(risultato.ErroriCSV, i, "ProvinciaNascita", rigaCSV(IndiceCSVProvinciaNascita), "non valido")
					_datiPerCfOK = False
				End If
			End If

			Dim _comuneNascita As Comune
			If rigaCSV(IndiceCSVCittaNascita) = "" Then
				AggiungiErrore(risultato.ErroriCSV, i, "CittaNascita", "", "obbligatorio")
				_datiPerCfOK = False
			ElseIf _provinciaNascita Is Nothing Then    'se la provincia non esiste allora anche il comune non ha senso
				AggiungiErrore(risultato.ErroriCSV, i, "CittaNascita", rigaCSV(IndiceCSVCittaNascita), "non valido")
				_datiPerCfOK = False
			Else
				_comuneNascita = _listaComuni.ListaComuni.Find(Function(c) c.Denominazione = rigaCSV(IndiceCSVCittaNascita))
				If _comuneNascita Is Nothing Then
					AggiungiErrore(risultato.ErroriCSV, i, "CittaNascita", rigaCSV(IndiceCSVCittaNascita), "non valido")
					_datiPerCfOK = False
                Else
                    Dim strSQL As String = "SELECT  provincie.IDProvincia, provincie.Provincia, comuni.IDComune, comuni.Denominazione, comuni.CAP, comuni.CF FROM provincie INNER JOIN comuni ON provincie.IDProvincia = comuni.IDProvincia WHERE (provincie.Provincia ='" & rigaCSV(IndiceCSVProvinciaNascita).Replace("'", "''") & "') and Denominazione= '" & rigaCSV(IndiceCSVCittaNascita).Replace("'", "''") & "'"
                    Using dtrprovncia As SqlDataReader = ClsServer.CreaDatareader(strSQL, conn)
                        If dtrprovncia.HasRows = True Then
                            dtrprovncia.Read()
                            If dtrprovncia("IDComune") <> _comuneNascita.Id Then
                                _comuneNascita.Id = Trim(dtrprovncia("IDComune"))
                                _rigaDaInserire.IdComuneNascita = _comuneNascita.Id
                                doppioComune = dtrprovncia("CF")
                            Else
                                _rigaDaInserire.IdComuneNascita = _comuneNascita.Id
                                doppioComune = ""
                            End If
                        Else
                            _rigaDaInserire.IdComuneNascita = _comuneNascita.Id
                            doppioComune = ""

                        End If

                    End Using
                    ' _rigaDaInserire.IdComuneNascita = _comuneNascita.Id
				End If
			End If

            Dim regex As Regex

			'verifica correttezza codice fiscale
			If rigaCSV(IndiceCSVCodiceFiscale).Length <> 16 Or Not _datiPerCfOK Then
				AggiungiErrore(risultato.ErroriCSV, i, "CodiceFiscale", rigaCSV(IndiceCSVCodiceFiscale), "non valido")
            Else
                regex = New Regex("^[a-zA-Z0-9]*$")

                If regex.Match(rigaCSV(IndiceCSVCodiceFiscale)).Success = False Then
                    AggiungiErrore(risultato.ErroriCSV, i, "CodiceFiscale", rigaCSV(IndiceCSVCodiceFiscale), "contiene caratteri non validi")
                    _datiPerCfOK = False
                Else

                    Dim strCodCatasto As String
                    Dim strCF_M As String
                    Dim strCF_F As String


                    If doppioComune = "" Then
                        strCF_M = ClsUtility.CreaCF(Trim(Replace(_rigaDaInserire.Cognome, "'", "''")), Trim(Replace(_rigaDaInserire.Nome, "'", "''")), Trim(_rigaDaInserire.DataNascita.ToString("dd/MM/yyyy")), _comuneNascita.CodiceCatastale, "M")
                        strCF_F = ClsUtility.CreaCF(Trim(Replace(_rigaDaInserire.Cognome, "'", "''")), Trim(Replace(_rigaDaInserire.Nome, "'", "''")), Trim(_rigaDaInserire.DataNascita.ToString("dd/MM/yyyy")), _comuneNascita.CodiceCatastale, "F")
                    Else
                        strCF_M = ClsUtility.CreaCF(Trim(Replace(_rigaDaInserire.Cognome, "'", "''")), Trim(Replace(_rigaDaInserire.Nome, "'", "''")), Trim(_rigaDaInserire.DataNascita.ToString("dd/MM/yyyy")), doppioComune, "M")
                        strCF_F = ClsUtility.CreaCF(Trim(Replace(_rigaDaInserire.Cognome, "'", "''")), Trim(Replace(_rigaDaInserire.Nome, "'", "''")), Trim(_rigaDaInserire.DataNascita.ToString("dd/MM/yyyy")), doppioComune, "F")
                    End If

                    'strCF_M = ClsUtility.CreaCF(Trim(Replace(_rigaDaInserire.Cognome, "'", "''")), Trim(Replace(_rigaDaInserire.Nome, "'", "''")), Trim(_rigaDaInserire.DataNascita.ToString("dd/MM/yyyy")), _comuneNascita.CodiceCatastale, "M")
                    'strCF_F = ClsUtility.CreaCF(Trim(Replace(_rigaDaInserire.Cognome, "'", "''")), Trim(Replace(_rigaDaInserire.Nome, "'", "''")), Trim(_rigaDaInserire.DataNascita.ToString("dd/MM/yyyy")), _comuneNascita.CodiceCatastale, "F")

                    If rigaCSV(IndiceCSVCodiceFiscale) <> strCF_M And rigaCSV(IndiceCSVCodiceFiscale) <> strCF_F Then
                        'Verifico Omocodia
                        If ClsUtility.VerificaOmocodia(UCase(strCF_M), rigaCSV(IndiceCSVCodiceFiscale)) Or ClsUtility.VerificaOmocodia(UCase(strCF_F), rigaCSV(IndiceCSVCodiceFiscale)) Then
                            _datiPerCfOK = True
                            _rigaDaInserire.CodiceFiscale = rigaCSV(IndiceCSVCodiceFiscale)
                        Else
                            AggiungiErrore(risultato.ErroriCSV, i, "CodiceFiscale", rigaCSV(IndiceCSVCodiceFiscale), "non valido")
                            _datiPerCfOK = False
                        End If
                    Else
                        _rigaDaInserire.CodiceFiscale = rigaCSV(IndiceCSVCodiceFiscale)
                    End If
                End If
            End If
            'fine verifica correttezza codice fiscale

            Dim _provinciaResidenza As Provincia
            If rigaCSV(IndiceCSVProvinciaResidenza) = "" Then
                _datiPerCapOK = False
                AggiungiErrore(risultato.ErroriCSV, i, "ProvinciaResidenza", "", "obbligatorio")
            Else
                _provinciaResidenza = _listaProvincie.ListaProvincie.Find(Function(p) p.Denominazione = rigaCSV(IndiceCSVProvinciaResidenza))
                If _provinciaNascita Is Nothing Then
                    _datiPerCapOK = False
                    AggiungiErrore(risultato.ErroriCSV, i, "ProvinciaResidenza", rigaCSV(IndiceCSVProvinciaResidenza), "non valido")
                End If
            End If

            Dim _comuneResidenza As Comune
            If rigaCSV(IndiceCSVCittaResidenza) = "" Then
                _datiPerCapOK = False
                AggiungiErrore(risultato.ErroriCSV, i, "CittaResidenza", "", "obbligatorio")
            ElseIf _provinciaResidenza Is Nothing Then    'se la provincia non esiste allora anche il comune non ha senso
                _datiPerCapOK = False
                AggiungiErrore(risultato.ErroriCSV, i, "CittaResidenza", rigaCSV(IndiceCSVCittaResidenza), "non valido")
            Else
                _comuneResidenza = _listaComuni.ListaComuni.Find(Function(c) c.Denominazione = rigaCSV(IndiceCSVCittaResidenza And c.Dismesso = 0)) 'solo comuni non dismessi
                If _comuneResidenza Is Nothing Then
                    _datiPerCapOK = False
                    AggiungiErrore(risultato.ErroriCSV, i, "CittaResidenza", rigaCSV(IndiceCSVCittaResidenza), "non valido")
                Else
                    _rigaDaInserire.IdComuneResidenza = _comuneResidenza.Id
                End If
            End If

            If rigaCSV(IndiceCSVVia) = "" Then
                _datiPerCapOK = False
                AggiungiErrore(risultato.ErroriCSV, i, "Via", "", "obbligatorio")
            Else
                _rigaDaInserire.IndirizzoResidenza = rigaCSV(IndiceCSVVia)
            End If

            If rigaCSV(IndiceCSVCivico) = "" Then
                _datiPerCapOK = False
                AggiungiErrore(risultato.ErroriCSV, i, "Numero", "", "obbligatorio")
            Else
                _rigaDaInserire.NumeroCivicoResidenza = rigaCSV(IndiceCSVCivico)
            End If

            If rigaCSV(IndiceCSVCAP) = "" Then
                AggiungiErrore(risultato.ErroriCSV, i, "CAP", "", "obbligatorio")
            ElseIf _datiPerCapOK = False Then
                AggiungiErrore(risultato.ErroriCSV, i, "CAP", rigaCSV(IndiceCSVCAP), "non valido per la residenza inserita.")
            Else
                Dim cap As Int64
                Dim capInteger As Boolean
                capInteger = Int64.TryParse(rigaCSV(IndiceCSVCAP), cap)
                If capInteger = False Then
                    AggiungiErrore(risultato.ErroriCSV, i, "CAP", rigaCSV(IndiceCSVCAP), "non valido. Inserire solo formati: 21 oppure 21/A oppure 21/A5 oppure 21 BIS oppure KM 21,500 OPPURE IL VALORE SNC")
                Else
                    Dim _res As String
                    Dim _flag As Boolean
                    If ClsUtility.CAP_VERIFICA(conn, _res, _flag, rigaCSV(IndiceCSVCAP), _rigaDaInserire.IdComuneResidenza, "", "", _rigaDaInserire.IndirizzoResidenza, _rigaDaInserire.NumeroCivicoResidenza) = False Then
                        AggiungiErrore(risultato.ErroriCSV, i, "CAP", rigaCSV(IndiceCSVCAP), "non valido: " & _res)
                    Else
                        _rigaDaInserire.CAPResidenza = rigaCSV(IndiceCSVCAP)
                    End If
                End If
            End If

            If rigaCSV(IndiceCSVTelefono) = "" Then
                ' AggiungiErrore(risultato.ErroriCSV, i, "Telefono", "", "obbligatorio") 'rimossa obbligatorietà su richiesta dipartimento del 22/10/2021
                _rigaDaInserire.Telefono = rigaCSV(IndiceCSVTelefono)
            ElseIf rigaCSV(IndiceCSVTelefono).Length > 11 Then
                AggiungiErrore(risultato.ErroriCSV, i, "Telefono", rigaCSV(IndiceCSVTelefono), "può contenere massimo 11 caratteri numerici")
            Else
                Dim telefono As Int64
                Dim telefonoInteger As Boolean
                telefonoInteger = Int64.TryParse(rigaCSV(IndiceCSVTelefono), telefono)
                If telefonoInteger = False Then
                    AggiungiErrore(risultato.ErroriCSV, i, "Telefono", rigaCSV(IndiceCSVTelefono), "può contenere solo numeri")
                Else
                    _rigaDaInserire.Telefono = rigaCSV(IndiceCSVTelefono)
                End If
            End If

            regex = New Regex("^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")

            If rigaCSV(IndiceCSVPEC) <> "" Then
                Dim match As Match = regex.Match(rigaCSV(IndiceCSVPEC))
                If match.Success = False Then
                    AggiungiErrore(risultato.ErroriCSV, i, "PEC", rigaCSV(IndiceCSVPEC), "non valido")
                Else
                    _rigaDaInserire.PEC = rigaCSV(IndiceCSVPEC)
                End If
            Else
                _rigaDaInserire.PEC = ""
            End If

            If rigaCSV(IndiceCSVEmail) = "" Then
                'AggiungiErrore(risultato.ErroriCSV, i, "Email", "", "obbligatorio") 'rimossa obbligatorietà su richiesta dipartimento del 22/10/2021
                _rigaDaInserire.Email = rigaCSV(IndiceCSVEmail)
            Else
                Dim match As Match = regex.Match(rigaCSV(IndiceCSVEmail))
                If match.Success = False Then
                    AggiungiErrore(risultato.ErroriCSV, i, "Email", rigaCSV(IndiceCSVEmail), "non valido")
                Else
                    _rigaDaInserire.Email = rigaCSV(IndiceCSVEmail)
                End If
            End If


            Dim strUltimaEsport As String
            Dim Esport As String
            Dim EsportDate As Date
            Dim DateZero As New System.DateTime(1900, 1, 1, 0, 0, 0)

            strUltimaEsport = "select isnull(max(UltimaEsportazioneDati),0) as UltimaEsportazione from RuoliAntimafia where idente=" & _rigaDaInserire.IdEnte & " and CodiceFiscale='" & _rigaDaInserire.CodiceFiscale & "' and IdElencoRuoliAntimafia=" & _rigaDaInserire.IdElencoRuoliAntimafia

            Using UltimaEsport As SqlDataReader = ClsServer.CreaDatareader(strUltimaEsport, conn)
                If UltimaEsport.HasRows = True Then
                    UltimaEsport.Read()
                    Esport = UltimaEsport("UltimaEsportazione")
                    If (Date.TryParse(Esport, EsportDate) = True) Then
                        If EsportDate = DateZero Then
                            _rigaDaInserire.UltimaEsportazioneDati = DateZero
                        Else
                            _rigaDaInserire.UltimaEsportazioneDati = EsportDate
                        End If
                    Else
                        _rigaDaInserire.UltimaEsportazioneDati = DateZero
                    End If
                Else
                    _rigaDaInserire.UltimaEsportazioneDati = DateZero
                End If
            End Using


        Next

		'inserisco l'eventuale ultima riga
		If Not _rigaDaInserire Is Nothing Then risultato.RigheCSV.Add(_rigaDaInserire)

		ControlloPostInterpretazioneCSV(risultato)
	End Sub

	Sub ControlloPostInterpretazioneCSV(ByRef risultato As ControlloCSV)
		' per tutte le righe valide devo effettuare i controlli di coerenza sui valori, in pratica replicando quelli della USP_RuoloAntimafia
		For Each r As RigaRuoloDaInserire In risultato.RigheCSV
			'parto dalla seconda riga e confronto con le precedenti
			If r.Riga > 1 Then
				'controllo che sia valida cioè che non ci sia una riga corrispondente alla mia con errori
				Dim _eFind As RigaRuoloErrore = risultato.ErroriCSV.Find(Function(ef) ef.Riga = r.Riga)
				If _eFind Is Nothing Then   'se è valida
					'cerco un idente, un codice fiscale ed un ruolo uguale al mio nelle righe precedenti
					Dim _rFind As RigaRuoloDaInserire = risultato.RigheCSV.Find(Function(rf) rf.IdEnte = r.IdEnte And rf.CodiceFiscale = r.CodiceFiscale And rf.IdElencoRuoliAntimafia = r.IdElencoRuoliAntimafia And rf.Riga < r.Riga)
					If Not _rFind Is Nothing Then
						'La riga (che nella struttura dati è ad indice iniziale 0) è incrementata di 1 per riflettere il numero di riga del file .csv
						AggiungiErrore(risultato.ErroriCSV, r.Riga, "Errore generale", "", "Il Codice Fiscale è già presente con lo stesso Ruolo Antimafia (riga " & _rFind.Riga + 1 & ")")
					End If
				End If
			End If
		Next

	End Sub

	Sub AggiungiErrore(ByRef _listaErrori As List(Of RigaRuoloErrore), ByVal _indice As Integer, ByVal _campo As String, ByVal _valore As String, ByVal _errore As String)
		Dim _errCampo = New CampoRuoloErrore With {.Errore = _errore, .NomeCampo = _campo, .Valore = _valore}

		'cerco la riga errori altrimenti la creo
		Dim _new As Boolean = False
		Dim _errRiga = _listaErrori.Find(Function(r) r.Riga = _indice)
		If _errRiga Is Nothing Then
			_errRiga = New RigaRuoloErrore
			_errRiga.Riga = _indice
			_new = True
		End If
		If _errRiga.Errori Is Nothing Then _errRiga.Errori = New List(Of CampoRuoloErrore)
		_errRiga.Errori.Add(_errCampo)
		If _new Then _listaErrori.Add(_errRiga)
	End Sub


	Function VerificaHeaderCSV(ByVal header As String) As Boolean
		'questa funzione verifica il formato del CSV controllando che la prima riga (header) corrisponda a quella che ci aspettiamo
		Const _header = "CodiceFiscaleEnte;CodiceFiscale;Cognome;Nome;RuoloAntiMafia;DataNascita;ProvinciaNascita;CittaNascita;ProvinciaResidenza;CittaResidenza;Via;Numero;CAP;Telefono;PEC;email"
		Return header.ToUpper = _header.ToUpper
	End Function
	' FINE funzionalità import massivo

	Public Class InfoAdeguamentoAntimafia
		Public IdEnteFaseAntimafia As Integer   'ultimo adeguamento antimafia
		Public isAperto As Boolean              'per vedere se l'adeguamento è aperto
		Public isEntePrivato As Boolean         'per vedere se si è abilitati alle funzionalità (inserimento/modifica/eliminazione)
		Public isEnteTitolare As Boolean        'per vedere se si può aprire/chiudere un adeguamento antimafia
		Public isAccreditamento As Boolean      'per vedere se si può aprire/chiudere un adeguamento antimafia
		Public Trovato As Boolean               'se false c'è un errore nei dati (ente non trovato)
		Public UltimaEsportazioneDati As DateTime
        Public DataChiusuraFase As DateTime
        Public EsisteEntePrivato As Boolean
		Public Sub New(ByVal IdEnte As Integer, ByVal conn As SqlClient.SqlConnection, ByVal SoloChiusi As Boolean)
			'Dim strSQL As String = "select isnull(t.IdEnteFaseAntimafia,0) IdEnteFaseAntimafia,	isnull(t.isAperto,0) isAperto,"
			'strSQL += " te.Privato isEntePrivato,case when er.IDEnteRelazione is null then 1 else 0 end isEnteTitolare,"
			'strSQL += " isnull((select case when ef.IdEnteFaseRiferimento is null then 0 else 1 end from EntiFasiAntimafia ef where IdEnteFaseAntimafia=t.IdEnteFaseAntimafia),0) isAccreditamento"
			'strSQL += " from Enti e inner join TipologieEnti te on te.Descrizione=e.Tipologia"
			'strSQL += " left join entirelazioni er on er.IDEnteFiglio=e.IDEnte and er.DataFineValidità is null"
			'strSQL += " left join (select max(efa.IdEnteFaseAntimafia) IdEnteFaseAntimafia,case efa.stato when 1 then 1 else 0 end isAperto,"
			'strSQL += IdEnte & " IdEnte from EntiFasiAntimafia efa where IdEnte=" & IdEnte & " or idEnte=(select IDEntePadre from entirelazioni er where IDEnteFiglio=" & IdEnte & " and er.DataFineValidità is null) group by efa.IdEnte,efa.Stato) t on t.IdEnte=e.IDEnte"
			'strSQL += " where e.IDEnte=" & idEnte

			Dim strSQL As String = "select isnull(t.IdEnteFaseAntimafia,0) IdEnteFaseAntimafia,	"
			strSQL += " isnull((select case	when ef.Stato is null then 0"
			strSQL += " when ef.Stato != 1 then 0 else 1 end from EntiFasiAntimafia ef "
			strSQL += " where IdEnteFaseAntimafia=t.IdEnteFaseAntimafia),0) isAperto, "
			strSQL += " te.Privato isEntePrivato,"
			strSQL += " case when er.IDEnteRelazione is null then 1 else 0 end isEnteTitolare, "
			strSQL += " isnull((select case when ef.IdEnteFaseRiferimento is null then 0 else 1 end "
			strSQL += " from EntiFasiAntimafia ef where IdEnteFaseAntimafia=t.IdEnteFaseAntimafia),0) isAccreditamento, "
			strSQL += " (select UltimaEsportazioneDati from EntiFasiAntimafia where IdEnteFaseAntimafia=t.IdEnteFaseAntimafia) UltimaEsportazioneDati,"
            strSQL += " (select DataChiusuraFase from EntiFasiAntimafia where IdEnteFaseAntimafia=t.IdEnteFaseAntimafia) DataChiusuraFase, "

            ' aggiunto campo per vedere se c'è almeno un ente privato tra i propri enti (compreso se stesso)
            strSQL += " isnull((select top 1 1 from enti et"
            strSQL += " inner join TipologieEnti tet on et.Tipologia=tet.Descrizione"
            strSQL += " left join entirelazioni er on er.IDEntePadre=et.IDEnte and er.DataFineValidità is null"
            strSQL += " left join enti ep on er.IDEnteFiglio=ep.IDEnte"
            strSQL += " left join TipologieEnti tep on ep.Tipologia=tep.Descrizione"
            strSQL += " where (tet.Privato=1 or tep.Privato=1) and et.idente=" & IdEnte & "),0) esisteenteprivato"

			strSQL += " from Enti e inner join TipologieEnti te on te.Descrizione=e.Tipologia "
			strSQL += " left join entirelazioni er on er.IDEnteFiglio=e.IDEnte and er.DataFineValidità is null "
			strSQL += " left join (	select max(efa.IdEnteFaseAntimafia) IdEnteFaseAntimafia," & IdEnte & " IdEnte "
			strSQL += " from EntiFasiAntimafia efa where (IdEnte=" & IdEnte & " or idEnte="
			strSQL += " (select IDEntePadre from entirelazioni er where IDEnteFiglio=" & IdEnte & " and er.DataFineValidità is null)) "
			If SoloChiusi Then strSQL += " and efa.Stato=3"
			strSQL += " group by efa.IdEnte) t on t.IdEnte=e.IDEnte where e.IDEnte=" & IdEnte

			Dim myDatatable As DataTable = ClsServer.CreaDataTable(strSQL, False, conn)

            EsisteEntePrivato = False
			If myDatatable.Rows.Count <> 1 Then
				Trovato = False
			Else
				Trovato = True
				IdEnteFaseAntimafia = myDatatable.Rows(0).Item("IdEnteFaseAntimafia")
				isAperto = If(myDatatable.Rows(0).Item("isAperto") = 1, True, False)
				isEntePrivato = myDatatable.Rows(0).Item("isEntePrivato")
				isEnteTitolare = If(myDatatable.Rows(0).Item("isEnteTitolare") = 1, True, False)
				isAccreditamento = If(myDatatable.Rows(0).Item("isAccreditamento") = 1, True, False)
				UltimaEsportazioneDati = If(IsDBNull(myDatatable.Rows(0).Item("UltimaEsportazioneDati")), Nothing, DateTime.Parse(myDatatable.Rows(0).Item("UltimaEsportazioneDati")))
                DataChiusuraFase = If(IsDBNull(myDatatable.Rows(0).Item("DataChiusuraFase")), Nothing, DateTime.Parse(myDatatable.Rows(0).Item("DataChiusuraFase")))
                EsisteEntePrivato = If(myDatatable.Rows(0).Item("esisteenteprivato") = 1, True, False)
			End If
		End Sub
	End Class

	Public Function GetBoxInfoAntimafia(ByVal IdEnte As Integer, ByVal conn As SqlClient.SqlConnection, ByVal inMascheraAntimafia As Boolean) As String
		'restituisce l'HTML del box delle Informazioni Antimafia (che va messo nella main e nella GestioneRuoliAntimafia)
		Dim _ret As String
		Try
			Dim _info As New InfoAdeguamentoAntimafia(IdEnte, conn, True)
			Dim _tmpRet As String

            If _info.EsisteEntePrivato = False Then Return _ret

			_tmpRet = "<fieldset><ul>"

			Dim _ultimaDataEsportazioneDati As String
			Dim _dataScadenzaDatiAntimafia As String
			If _info.UltimaEsportazioneDati.Year = 1 And _info.DataChiusuraFase.Year = 1 Then
                _ultimaDataEsportazioneDati = "<span style='color:red'><b><i>NON PRESENTE A SISTEMA</i></b></span>"
                _dataScadenzaDatiAntimafia = "<span style='color:red'><b><i>DATI SCADUTI</i><b></span>"
			ElseIf _info.UltimaEsportazioneDati.Year = 1 Then
				If _info.DataChiusuraFase.AddMonths(6) < Now Then
                    _dataScadenzaDatiAntimafia = "<span style='color:red'><b><i>DATI SCADUTI</i><b></span>"
                    _ultimaDataEsportazioneDati = "<span style='color:red'><b><i>" + _info.DataChiusuraFase.ToString("dd/MM/yyyy") + "</i></b></span>"
				Else
					_dataScadenzaDatiAntimafia = "<b>" + _info.DataChiusuraFase.AddMonths(6).ToString("dd/MM/yyyy") + "</b>"
					_ultimaDataEsportazioneDati = "<b>" + _info.DataChiusuraFase.ToString("dd/MM/yyyy") + "</b>"
				End If
			Else
				If _info.UltimaEsportazioneDati.AddMonths(6) < Now Then
                    _dataScadenzaDatiAntimafia = "<span style='color:red'><b><i>DATI SCADUTI</i><b></span>"
                    _ultimaDataEsportazioneDati = "<span style='color:red'><b><i>" + _info.UltimaEsportazioneDati.ToString("dd/MM/yyyy") + "</i></b></span>"
				Else
					_dataScadenzaDatiAntimafia = "<b>" + _info.UltimaEsportazioneDati.AddMonths(6).ToString("dd/MM/yyyy") + "</b>"
					_ultimaDataEsportazioneDati = "<b>" + _info.UltimaEsportazioneDati.ToString("dd/MM/yyyy") + "</b>"
				End If
			End If

			_tmpRet += "<li><b>Ultimo aggiornamento dati Antimafia</b> : " & _ultimaDataEsportazioneDati & "</li>"
			_tmpRet += "<li><b>Scadenza aggiornamento dati Antimafia</b> : " & _dataScadenzaDatiAntimafia & "</li>"

			If Not _info.isAperto Then
                _tmpRet += "<li><b>Per modificare i dati utilizzare la funzione Avvia Aggiornamento Antimafia, inserire i dati necessari e concludere utilizzando la funzione Termina Aggiornamento Antimafia" + If(inMascheraAntimafia, " nella maschera Gestione Ruoli Antimafia", "") + "</b></li>"
                _tmpRet += "<li><b>Per confermare i dati, se invariati alla scadenza dei sei mesi, utilizzare la funzione Avvia Aggiornamento Antimafia e di seguito la funzione Termina Aggiornamento Antimafia</b></li>"
			End If

			_tmpRet += "</ul></fieldset>"

			_ret = _tmpRet
		Catch ex As Exception
			'per evitare di far scoppiare tutto, è solo per sicurezza non dovrebbe accadere mai
		End Try

		Return _ret
	End Function

	Public Function AvviaAdeguamentoAntimafia(ByVal IdEnte As Integer, ByVal conn As SqlClient.SqlConnection) As String
		Dim _info As New InfoAdeguamentoAntimafia(IdEnte, conn, False)

		'I seguenti controlli sono solo per sicurezza, non dovrebbero verificarsi tali errori se le chiamate vengono fatte nel modo corretto
		'If Not _info.isEntePrivato Then Return "Gli Enti pubblici non possono avviare Aggiornamenti Antimafia."
		If Not _info.isEnteTitolare Then Return "Gli Enti in accordo non possono avviare Aggiornamenti Antimafia."
		If _info.isAperto Then
			If _info.isAccreditamento Then
                Return "Non si possono avviare Aggiornamenti Antimafia durante l'Iscrizione."
			Else
                Return "Esiste già un Aggiornamento Antimafia avviato."
			End If
		End If

		Dim _adeguamentoPrecedente As Integer = _info.IdEnteFaseAntimafia

		Dim sqlCommand As New SqlClient.SqlCommand
		Dim _errore As String = ""
		sqlCommand.CommandText = "SP_AVVIA_ADEGUAMENTO_ANTIMAFIA"
		sqlCommand.CommandType = CommandType.StoredProcedure
		sqlCommand.Connection = conn


		sqlCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
		sqlCommand.Parameters.AddWithValue("@IdEnteFaseAntimafiaOLD", _adeguamentoPrecedente)

        sqlCommand.ExecuteNonQuery()

		Return ""

	End Function




	Public Function TerminaAdeguamentoAntimafia(ByVal IdEnte As Integer, ByVal conn As SqlClient.SqlConnection, allegato As Allegato, utente As String) As String
		Dim _info As New InfoAdeguamentoAntimafia(IdEnte, conn, False)

		'I seguenti controlli sono solo per sicurezza, non dovrebbero verificarsi tali errori se le chiamate vengono fatte nel modo corretto
        If Not _info.isEnteTitolare Then Return "Gli Enti in accordo non possono terminare Aggiornamenti Antimafia."
		If _info.isAperto Then
			If _info.isAccreditamento Then
                Return "Non si possono terminare Aggiornamenti Antimafia durante l'Iscrizione."
			End If
		Else
            Return "Non esiste un Aggiornamento Antimafia avviato."
		End If
		If Not ControllaAutocertificazioni(IdEnte, conn) Then
			Return "Non sono state effettuate le autocertificazioni."
		End If
		'Controllo vincoli antimafia
		Dim sqlCommand As New SqlClient.SqlCommand
		sqlCommand.CommandText = "SP_VINCOLO_RUOLI_ANTIMAFIA"
		sqlCommand.CommandType = CommandType.StoredProcedure
		sqlCommand.Connection = conn

		sqlCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
        sqlCommand.Parameters.AddWithValue("@SceltaFase", 2)

		sqlCommand.Parameters.Add("@Esito", SqlDbType.Int)
		sqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output

		sqlCommand.ExecuteNonQuery()

		Dim _esito As Integer = sqlCommand.Parameters("@Esito").Value()

		If _esito = 0 Then
			Return "Non sono presenti Ruoli Antimafia per tutti gli enti privati."
		End If

		If _esito = 2 Then
            Return "Fase di Aggiornamento Antimafia non trovata."
		End If

        'Controllo certificazioni antimafia
        sqlCommand = New SqlClient.SqlCommand
        sqlCommand.CommandText = "SP_VERIFICA_EntiAutocertificazioni"
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Connection = conn

        sqlCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
        sqlCommand.Parameters.AddWithValue("@SceltaFase", 2)
        sqlCommand.Parameters.Add("@Esito", SqlDbType.Int)
        sqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output

        sqlCommand.ExecuteNonQuery()

        _esito = sqlCommand.Parameters("@Esito").Value()

        If _esito = 0 Then
            Return "Non sono presenti Certificazioni/Consensi Antimafia per tutti gli enti privati."
        End If

        sqlCommand.CommandText = "SP_CHIUDI_FASE_ANTIMAFIA"
		sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Connection = conn
        sqlCommand.Parameters.Clear()
        sqlCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
		sqlCommand.Parameters.AddWithValue("@IdTipoAllegato", TipoFile.COMUNICAZIONE_ANTIMAFIA)
		sqlCommand.Parameters.AddWithValue("@IdEnteFaseAntimafia", _info.IdEnteFaseAntimafia)
		sqlCommand.Parameters.AddWithValue("@BinData", allegato.Blob)
		sqlCommand.Parameters.AddWithValue("@FileName", allegato.Filename)
		sqlCommand.Parameters.AddWithValue("@HashValue", allegato.Hash)
		sqlCommand.Parameters.AddWithValue("@FileLength", allegato.Filesize)
		sqlCommand.Parameters.AddWithValue("@UsernameInserimento", utente)
        sqlCommand.Parameters.Add("@Esito", SqlDbType.Int)
        sqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output

		sqlCommand.ExecuteNonQuery()

		Return ""

	End Function

    Public Function CaricaDdlEntiByIdEntePadre(ByVal ObjCombo As DropDownList, ByVal IdEntePadre As Integer, ByVal conn As SqlClient.SqlConnection, _
        ByVal conRigaVuota As Boolean, Optional ByVal visualizzaAsterischiCertificazioni As Boolean = False) As DropDownList
        'Carica la combo passata come parametro con l'ente principale passato come parametro e tutti gli enti in accordo
        'Se i risultati sono più di 1 aggiunge automaticamente in testa una riga vuota
        Dim strsql As String
        Dim Privato As String
        Dim myDateset As New DataSet

        ObjCombo.DataSource = Nothing

        'strsql = "select idente, denominazione from"
        'strsql &= " ("
        'strsql &= " 	select 0 as idente,'' as denominazione,0 as ordine from entirelazioni er where er.IDEntePadre=" & IdEntePadre & " and er.datafinevalidità is null having count(*)>0"
        'strsql &= "         union"
        'strsql &= " 	select idente,denominazione,1 as ordine from enti where IDEnte=" & IdEntePadre
        'strsql &= "         union"
        'strsql &= " 	select idente,denominazione,2 as ordine from enti e inner join entirelazioni er on er.IDEnteFiglio=e.idente"
        'strsql &= "         where(er.IDEntePadre = " & IdEntePadre & " And er.datafinevalidità Is null)"
        'strsql &= " ) t"
        'strsql &= " order by t.ordine, t.Denominazione" 
        If visualizzaAsterischiCertificazioni Then 'asterischi
            strsql = "select t.idente, case when ea.idEnte is null then '( ) ' else '(*) ' end + rtrim(ltrim(denominazione)) as denominazione from "
        Else
            strsql = "select t.idente, rtrim(ltrim(denominazione)) as denominazione from "
        End If


        strsql &= " ( 	"
        strsql &= " select "
        strsql &= " idente,denominazione,Tipologia, 1 as ordine from enti where IDEnte=" & IdEntePadre
        strsql &= " union 	"
        strsql &= " select idente,denominazione,Tipologia, 2 as ordine"
        strsql &= " from enti e inner join entirelazioni er on er.IDEnteFiglio=e.idente         "
        strsql &= " where(er.IDEntePadre = " & IdEntePadre & " And er.datafinevalidità Is null And e.idStatoEnte in(3,6)) "
        strsql &= " ) t "
        strsql &= " inner join TipologieEnti te on te.Descrizione=t.Tipologia"
        strsql &= " left join entiautocertificazioni ea on ea.idEnte = t.idEnte"
        strsql &= " where te.Privato=1"
        strsql &= " order by ordine, rtrim(ltrim(denominazione))"

        Dim myDatatable As DataTable = ClsServer.CreaDataTable(strsql, False, conn)

        'myDateset = ClsServer.DataSetGenerico(strsql, conn)
        If myDatatable.Rows.Count <> 1 And conRigaVuota Then
            Dim _r As DataRow = myDatatable.NewRow()
            _r("idente") = 0
            _r("denominazione") = ""
            myDatatable.Rows.InsertAt(_r, 0)
        End If

        ObjCombo.DataSource = myDatatable
        ObjCombo.DataTextField = "denominazione"
        ObjCombo.DataValueField = "idente"
        ObjCombo.DataBind()

        Return ObjCombo

    End Function

	Public Function CaricaDdlRuoliAntimafia(ByVal ObjCombo As DropDownList, ByVal EntePrivato As Integer, ByVal conn As SqlClient.SqlConnection) As DropDownList
		'Carica la combo passata come parametro con i ruoli antimafia
		'Se i risultati sono più di 1 aggiunge automaticamente in testa una riga vuota
		'Se si passa EntePrivato=-1 carica TUTTI i ruoli antimafia
		Dim strsql As String
		Dim Privato As String
		Dim myDateset As New DataSet

		ObjCombo.DataSource = Nothing
		If EntePrivato = 1 Then
			Privato = "1" 'Ente Privato
		ElseIf EntePrivato = 0 Then
			Privato = "0" 'Ente Pubblico
		End If

		If EntePrivato = -1 Then
			strsql = "select 0 as IdRuoloAntiMafia,'' as RuoloAntimafia"
			strsql &= " union"
			strsql &= " select Id as IdRuoloAntimafia, RuoloAntiMafia from ElencoRuoliAntimafia order by 2"
		Else
			strsql = "select 0 as IdRuoloAntiMafia,'' as RuoloAntimafia from ElencoRuoliAntimafia where Privato=" & Privato & " having count(*)>1"
			strsql &= " union"
			strsql &= " select Id as IdRuoloAntimafia, RuoloAntiMafia from ElencoRuoliAntimafia where Privato=" & Privato & "  order by 2"
		End If

		myDateset = ClsServer.DataSetGenerico(strsql, conn)
		ObjCombo.DataSource = myDateset
		ObjCombo.DataTextField = "RuoloAntimafia"
		ObjCombo.DataValueField = "IdRuoloAntimafia"
		ObjCombo.DataBind()

		Return ObjCombo
	End Function

	Public Function SalvaRuoloAntimafia(
											ByRef Id As Integer,
											ByVal IdEnte As Integer,
											ByVal CodiceFiscale As String,
											ByVal Cognome As String,
											ByVal Nome As String,
											ByVal IdElencoRuoliAntimafia As Integer,
											ByVal DataNascita As Date,
											ByVal IdComuneNascita As Integer,
											ByVal IdComuneResidenza As Integer,
											ByVal IndirizzoResidenza As String,
											ByVal NumeroCivicoResidenza As String,
											ByVal CAPResidenza As String,
											ByVal Telefono As String,
											ByVal PEC As String,
											ByVal Email As String,
											ByVal IdEnteFaseAntimafia As Integer,
											ByVal conn As SqlClient.SqlConnection
										) As String

		Dim sqlCommand As New SqlClient.SqlCommand
		Dim _errore As String = ""
		sqlCommand.CommandText = "USP_RuoloAntimafia"
		sqlCommand.CommandType = CommandType.StoredProcedure
		sqlCommand.Connection = conn

		sqlCommand.Parameters.AddWithValue("@Id", Id)
		sqlCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
		sqlCommand.Parameters.AddWithValue("@CodiceFiscale", CodiceFiscale)
		sqlCommand.Parameters.AddWithValue("@Cognome", Cognome)
		sqlCommand.Parameters.AddWithValue("@Nome", Nome)
		sqlCommand.Parameters.AddWithValue("@IdElencoRuoliAntimafia", IdElencoRuoliAntimafia)
		sqlCommand.Parameters.AddWithValue("@DataNascita", DataNascita)
		sqlCommand.Parameters.AddWithValue("@IdComuneNascita", IdComuneNascita)
		sqlCommand.Parameters.AddWithValue("@IdComuneResidenza", IdComuneResidenza)
		sqlCommand.Parameters.AddWithValue("@IndirizzoResidenza", IndirizzoResidenza)
		sqlCommand.Parameters.AddWithValue("@NumeroCivicoResidenza", NumeroCivicoResidenza)
		sqlCommand.Parameters.AddWithValue("@CAPResidenza", CAPResidenza)
		sqlCommand.Parameters.AddWithValue("@Telefono", Telefono)
		sqlCommand.Parameters.AddWithValue("@PEC", PEC)
		sqlCommand.Parameters.AddWithValue("@Email", Email)
		sqlCommand.Parameters.AddWithValue("@IdEnteFaseAntimafia", IdEnteFaseAntimafia)

		sqlCommand.Parameters.Add("@IdRuoloAntimafia", SqlDbType.Int)
		sqlCommand.Parameters("@IdRuoloAntimafia").Direction = ParameterDirection.Output

		sqlCommand.Parameters.Add("@Errore", SqlDbType.VarChar, 1000)
		sqlCommand.Parameters("@Errore").Direction = ParameterDirection.Output

		sqlCommand.ExecuteNonQuery()
		Id = sqlCommand.Parameters("@IdRuoloAntimafia").Value()
		_errore = sqlCommand.Parameters("@Errore").Value()

		Return _errore
	End Function

	Public Function EliminaRuoloAntimafia(ByVal idRuoloAntimafia As Integer, ByVal idEnteFaseAntimafia As Integer, ByVal conn As SqlClient.SqlConnection)
		Dim sqlCommand As New SqlClient.SqlCommand
		Dim _errore As String = ""
		sqlCommand.CommandText = "DELETE from RuoliAntimafia where Id=" & idRuoloAntimafia.ToString & " and IdEnteFaseAntimafia=" & idEnteFaseAntimafia
		sqlCommand.CommandType = CommandType.Text
		sqlCommand.Connection = conn
		sqlCommand.ExecuteNonQuery()
		Return _errore
	End Function


	Public Function GetRuoliAntimafia(
										ByVal idEnte As Integer,
										ByVal idEnteFaseAntimafia As Integer,
										ByVal conn As SqlClient.SqlConnection,
										ByVal filtroCognome As String,
										ByVal filtroNome As String,
										ByVal filtroCodiceFiscale As String,
										ByVal filtroIdElencoRuoloAntiMafia As Integer,
										ByVal AncheFigli As Boolean,
										ByVal filtroCodiceEnte As String
									 ) As DataSet

		Dim _elencoRuoli As New DataSet
		Dim sqlCommand As New SqlClient.SqlCommand
		Dim _errore As String = ""
		Dim strSql = ""

		strSql = "select Enti.Denominazione, r.Cognome, r.Nome, r.CodiceFiscale, e.RuoloAntiMafia, r.IdEnte, r.Id as IdRuoloAntiMafia "
		strSql += " from"
		strSql += " RuoliAntimafia r"
		strSql += " inner join ElencoRuoliAntimafia e on r.IdElencoRuoliAntimafia=e.Id "
		strSql += " inner join enti on enti.idente= r.IdEnte "
		strSql += " where r.IdEnte = @idEnte and r.IdEnteFaseAntimafia= @idEnteFaseAntimafia"

		If filtroNome.Trim <> "" Then
			strSql += " and r.Nome like @filtroNome"
		End If

		If filtroCognome.Trim <> "" Then
			strSql += " and r.Cognome like @filtroCognome"
		End If

		If filtroCodiceFiscale.Trim <> "" Then
			strSql += " and r.CodiceFiscale like @filtroCodiceFiscale"
		End If

		If filtroIdElencoRuoloAntiMafia <> 0 Then   'qui non c'è pericolo di sql injection
			strSql += " and r.idElencoRuoliAntimafia=" + filtroIdElencoRuoloAntiMafia.ToString
		End If

		If filtroCodiceEnte <> "" Then
			strSql += " and CodiceRegione  like '" & ClsServer.NoApice(filtroCodiceEnte) & "'"
		End If



		If AncheFigli Then
			strSql += " union"
			strSql += " select Enti.Denominazione, r.Cognome, r.Nome, r.CodiceFiscale, e.RuoloAntiMafia, r.IdEnte, r.Id as IdRuoloAntiMafia "
			strSql += " from"
			strSql += " RuoliAntimafia r"
			strSql += " inner join ElencoRuoliAntimafia e on r.IdElencoRuoliAntimafia=e.Id"
			strSql += " inner join entirelazioni er on er.IDEnteFiglio=r.IdEnte"
			strSql += " inner join enti on enti.idente= er.IDEnteFiglio "
			strSql += " where"
			strSql += " er.DataFineValidità is null and er.IDEntePadre=@idEnte and r.IdEnteFaseAntimafia= @idEnteFaseAntimafia"

			If filtroNome.Trim <> "" Then
				strSql += " and r.Nome like @filtroNome"
			End If

			If filtroCognome.Trim <> "" Then
				strSql += " and r.Cognome like @filtroCognome"
			End If

			If filtroCodiceFiscale.Trim <> "" Then
				strSql += " and r.CodiceFiscale like @filtroCodiceFiscale"
			End If

			If filtroIdElencoRuoloAntiMafia <> 0 Then   'qui non c'è pericolo di sql injection
				strSql += " and r.idElencoRuoliAntimafia=" + filtroIdElencoRuoloAntiMafia.ToString
			End If

			If filtroCodiceEnte <> "" Then
				strSql += " and CodiceRegione  like '" & ClsServer.NoApice(filtroCodiceEnte) & "'"
			End If
		End If

		sqlCommand.Parameters.AddWithValue("@idEnte", idEnte)
		sqlCommand.Parameters.AddWithValue("@idEnteFaseAntimafia", idEnteFaseAntimafia)
		sqlCommand.Parameters.AddWithValue("@filtroNome", filtroNome.Trim + "%")
		sqlCommand.Parameters.AddWithValue("@filtroCognome", filtroCognome.Trim + "%")
		sqlCommand.Parameters.AddWithValue("@filtroCodiceFiscale", filtroCodiceFiscale.Trim + "%")

		strSql += " ORDER BY COGNOME"
		sqlCommand.CommandText = strSql
		sqlCommand.CommandType = CommandType.Text
		sqlCommand.Connection = conn

		Dim CMD As New SqlDataAdapter(sqlCommand)
		CMD.Fill(_elencoRuoli)

		Return _elencoRuoli

	End Function

	Public Function GetRuoloAntimafia(ByVal idRuoloAntiMafia As Integer, ByVal conn As SqlClient.SqlConnection) As DataSet
		Dim _ruoloAntiMafia As New DataSet
		Dim sqlCommand As New SqlClient.SqlCommand
		Dim _errore As String = ""
		Try
			Dim strSql = "SELECT r.IdEnte, r.CodiceFiscale, r.Cognome, r.Nome, r.IdElencoRuoliAntimafia, r.DataNascita, r.IdComuneNascita, r.IdComuneResidenza, r.IndirizzoResidenza,"
			strSql += " r.NumeroCivicoResidenza, r.CAPResidenza, r.Telefono, r.PEC, r.Email, pn.IDProvincia AS IdProvinciaNascita, pr.IDProvincia AS IdProvinciaResidenza, cn.ComuneNazionale as ComuneNazionaleNascita, cr.ComuneNazionale as ComuneNazionaleResidenza"
			strSql += " FROM RuoliAntimafia AS r INNER JOIN comuni AS cn ON r.IdComuneNascita = cn.IDComune INNER JOIN provincie AS pn ON cn.IDProvincia = pn.IDProvincia INNER JOIN"
			strSql += " comuni AS cr ON r.IdComuneResidenza = cr.IDComune INNER JOIN provincie AS pr ON cr.IDProvincia = pr.IDProvincia where r.Id=@idRuoloAntiMafia ORDER BY COGNOME"

			'strSql = "select IdEnte, CodiceFiscale, Cognome, Nome, IdElencoRuoliAntimafia, DataNascita, IdComuneNascita, IdComuneResidenza, IndirizzoResidenza, NumeroCivicoResidenza, CAPResidenza, Telefono, PEC, Email from RuoliAntimafia r where r.Id=@idRuoloAntiMafia"
			sqlCommand.Parameters.AddWithValue("@idRuoloAntiMafia", idRuoloAntiMafia)

			sqlCommand.CommandText = strSql
			sqlCommand.CommandType = CommandType.Text
			sqlCommand.Connection = conn

			Dim CMD As New SqlDataAdapter(sqlCommand)
			CMD.Fill(_ruoloAntiMafia)

		Catch ex As Exception
			_errore = "Errore imprevisto, contattare l'Assistenza"
			'INSERIRE LOG DEGLI ERRORI
		End Try

		Return _ruoloAntiMafia
	End Function
	Public Function IsRuoloAntimafiaInEnte(ByVal idRuoloAntimafia As Integer, ByVal idEnte As Integer, ByVal conn As SqlClient.SqlConnection) As Integer
		'Controlla che sia stato passato un ruolo antimafia afferente al mio ente (mio ente o ente figlio)
		Dim _ret As Boolean = False
		Dim sqlCommand As New SqlClient.SqlCommand
		Dim _errore As String = ""
		Dim _esiste As New DataSet
		Try
			Dim strSql = "select id from RuoliAntimafia "
			strSql += " where id=@idRuoloAntiMafia and idente=@idEnte"
			strSql += " union"
			strSql += " select id from RuoliAntimafia r inner join entirelazioni er on er.IDEnteFiglio=r.IdEnte"
			strSql += " where(id = @idRuoloAntiMafia And er.IDEntePadre = @idEnte And er.DataFineValidità Is null)"

			sqlCommand.Parameters.AddWithValue("@idRuoloAntiMafia", idRuoloAntimafia)
			sqlCommand.Parameters.AddWithValue("@idEnte", idEnte)
			sqlCommand.CommandText = strSql
			sqlCommand.CommandType = CommandType.Text
			sqlCommand.Connection = conn
			Dim CMD As New SqlDataAdapter(sqlCommand)
			CMD.Fill(_esiste)
			If _esiste.Tables.Count = 1 AndAlso _esiste.Tables(0).Rows.Count = 1 Then _ret = True
		Catch ex As Exception
			_errore = "Errore imprevisto, contattare l'Assistenza"
			'INSERIRE LOG DEGLI ERRORI
		End Try
		Return _ret
	End Function


	Public Function GetRuoliAntimafiaEsportazioneTitolare(
										ByVal idEnte As Integer,
										ByVal idEnteFaseAntimafia As Integer,
										ByVal conn As SqlClient.SqlConnection
									 ) As DataSet

		Dim _elencoRuoli As New DataSet
		Dim sqlCommand As New SqlClient.SqlCommand

		Dim strSql = "select e.CodiceFiscale CodiceFiscaleEnte, r.CodiceFiscale,r.Cognome,r.Nome,era.RuoloAntiMafia,convert(varchar(10),r.DataNascita,103) DataNascita,pn.Provincia as ProvinciaNascita,cn.Denominazione as CittaNascita,"
		strSql += "pr.Provincia as ProvinciaResidenza,cr.Denominazione as CittaResidenza, r.IndirizzoResidenza as Via, r.NumeroCivicoResidenza as Numero,"
		strSql += "r.CAPResidenza as CAP,r.Telefono,r.PEC,r.Email"
		strSql += " from enti e"
		strSql += " inner join RuoliAntimafia r on r.IdEnte=e.IDEnte and r.IdEnteFaseAntimafia=@IdEnteFaseAntimafia"
		strSql += " inner join ElencoRuoliAntimafia era on r.IdElencoRuoliAntimafia=era.Id"
		strSql += " INNER JOIN comuni AS cn ON r.IdComuneNascita = cn.IDComune INNER JOIN provincie AS pn ON cn.IDProvincia = pn.IDProvincia "
		strSql += " INNER JOIN comuni AS cr ON r.IdComuneResidenza = cr.IDComune INNER JOIN provincie AS pr ON cr.IDProvincia = pr.IDProvincia"
		strSql += " where r.IdEnte = @idEnte"
		strSql += " union"
		strSql += " select e.CodiceFiscale, r.CodiceFiscale,r.Cognome,r.Nome,era.RuoloAntiMafia,convert(varchar(10),r.DataNascita,103) DataNascita,pn.Provincia as ProvinciaNascita,cn.Denominazione as CittaNascita,"
		strSql += "pr.Provincia as ProvinciaResidenza,cr.Denominazione as CittaResidenza, r.IndirizzoResidenza as Via, r.NumeroCivicoResidenza as Numero,"
		strSql += "r.CAPResidenza as CAP,r.Telefono,r.PEC,r.email"
		strSql += " from entirelazioni er"
		strSql += " inner join enti e on er.IDEnteFiglio=e.IDEnte"
		strSql += " inner join RuoliAntimafia r on r.IdEnte=e.IDEnte and r.IdEnteFaseAntimafia=@IdEnteFaseAntimafia"
		strSql += " inner join ElencoRuoliAntimafia era on r.IdElencoRuoliAntimafia=era.Id"
		strSql += " INNER JOIN comuni AS cn ON r.IdComuneNascita = cn.IDComune INNER JOIN provincie AS pn ON cn.IDProvincia = pn.IDProvincia "
		strSql += " INNER JOIN comuni AS cr ON r.IdComuneResidenza = cr.IDComune INNER JOIN provincie AS pr ON cr.IDProvincia = pr.IDProvincia"
		strSql += " where er.IDEntePadre=@idEnte and er.datafinevalidità is null"
		strSql += " ORDER BY e.CodiceFiscale,COGNOME"
		sqlCommand.Parameters.AddWithValue("@idEnte", idEnte)
		sqlCommand.Parameters.AddWithValue("@IdEnteFaseAntimafia", idEnteFaseAntimafia)
		sqlCommand.CommandText = strSql
		sqlCommand.CommandType = CommandType.Text
		sqlCommand.Connection = conn

		Dim CMD As New SqlDataAdapter(sqlCommand)
		CMD.Fill(_elencoRuoli)

		Return _elencoRuoli

	End Function

	Public Sub ExportCSV(ByVal Dati As DataSet, ByVal Nomefile As String, ByVal Pagina As Web.UI.Page)
		'Costruisce e fa il download di un CSV (separato da ";") a partire da un dataset
		'Fatta per essere chiamata da una pagina (che gli viene passata come ultimo parametro)
		Dim csv As String = String.Empty

		For Each column As DataColumn In Dati.Tables(0).Columns
			'Add the Header row for CSV file.
			csv += column.ColumnName + ";"c
		Next

		'toglie l'eventuale ultimo ";"
		If csv(csv.Length - 1) = ";" Then csv = Left(csv, csv.Length - 1)

		'Add new line.
		csv += vbCr & vbLf

		For Each row As DataRow In Dati.Tables(0).Rows
			For Each column As DataColumn In Dati.Tables(0).Columns
				'Add the Data rows.
				csv += row(column.ColumnName).ToString().Replace(";", ":") + ";"c
			Next

			'toglie l'eventuale ultimo ";"
			If csv(csv.Length - 1) = ";" Then csv = Left(csv, csv.Length - 1)
			'Add new line.
			csv += vbCr & vbLf
		Next

		'Download the CSV file.
		Pagina.Response.Clear()
		Pagina.Response.Buffer = True
        Pagina.Response.AddHeader("content-disposition", "attachment;filename=" & Nomefile)
        'Pagina.Response.AddHeader("Content-Type", "text/html;charset=UTF-8")
        Pagina.Response.Charset = "ISO-8859-1"
        Pagina.CodePage = 28591
        'Pagina.Response.Charset = "UTF-8"
        'Pagina.Response.ContentType = "application/text"
		Pagina.Response.Output.Write(csv)
		Pagina.Response.Flush()
		Pagina.Response.End()
	End Sub


	Public Function GetRuoliAntimafia(
		idEnte As Integer,
		conn As SqlConnection
	) As List(Of RuoloAntimafia)

		Dim RuoliAntimafia As New List(Of RuoloAntimafia)
		Dim sqlRuoliAntimafia As String = ""
		sqlRuoliAntimafia += "Select enti.codicefiscale CodiceFiscaleEnte,r.CodiceFiscale,r.Nome,	r.Cognome,	e.RuoloAntiMafia from "
		sqlRuoliAntimafia += "RuoliAntimafia r "
		sqlRuoliAntimafia += "INNER JOIN ElencoRuoliAntimafia e on r.IdElencoRuoliAntimafia=e.Id "
		sqlRuoliAntimafia += "INNER JOIN enti on enti.idente= r.IdEnte "
		sqlRuoliAntimafia += "INNER JOIN EntiFasiAntimafia efa on efa.IdEnteFaseAntimafia = r.IdEnteFaseAntimafia "
		sqlRuoliAntimafia += "WHERE r.IdEnte "
		sqlRuoliAntimafia += "IN (SELECT IdEnteFiglio From entirelazioni where DataFineValidità IS NULL AND IDEntePadre=@Idente UNION SELECT @idEnte)"
		sqlRuoliAntimafia += "AND efa.DataChiusuraFase IS NULL "
		sqlRuoliAntimafia += "ORDER BY CASE WHEN enti.Idente=@IdEnte THEN '0' else enti.codicefiscale END, r.cognome, r.nome"
		Dim cmdRuoliAntimafia As New SqlCommand(sqlRuoliAntimafia, conn)
		cmdRuoliAntimafia.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrRuoliAntimafia As SqlDataReader = Nothing
		Try
			rdrRuoliAntimafia = cmdRuoliAntimafia.ExecuteReader()
			While rdrRuoliAntimafia.Read()
				Dim RuoloAntimafia As New RuoloAntimafia()
				RuoloAntimafia.CodiceFiscale = GetData(rdrRuoliAntimafia("CodiceFiscale"))
				RuoloAntimafia.CodiceFiscaleEnte = GetData(rdrRuoliAntimafia("CodiceFiscaleEnte"))
				RuoloAntimafia.Nome = GetData(rdrRuoliAntimafia("Nome"))
				RuoloAntimafia.Cognome = GetData(rdrRuoliAntimafia("Cognome"))
				RuoloAntimafia.Ruolo = GetData(rdrRuoliAntimafia("RuoloAntiMafia"))
				RuoliAntimafia.Add(RuoloAntimafia)
			End While
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati RuoliAntimafia", ex)
		Finally
			If rdrRuoliAntimafia IsNot Nothing Then
				rdrRuoliAntimafia.Close()
			End If
		End Try


		Return RuoliAntimafia
	End Function

	Public Function ControllaAutocertificazioni(IdEnte As Integer, conn As SqlClient.SqlConnection) As Boolean
		Dim sqlCommand As New SqlClient.SqlCommand
		sqlCommand.CommandText = "SP_VERIFICA_EntiAutocertificazioni"
		sqlCommand.CommandType = CommandType.StoredProcedure
		sqlCommand.Connection = conn
        sqlCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
        sqlCommand.Parameters.AddWithValue("@SceltaFase", 2)
		sqlCommand.Parameters.AddWithValue("@Esito", IdEnte)
		sqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output
		sqlCommand.ExecuteNonQuery()

		Return sqlCommand.Parameters("@Esito").Value = 1
	End Function

	Public Function ControllaRuoliAntimafia(IdEnte As Integer, conn As SqlClient.SqlConnection) As Boolean
		Dim sqlCommand As New SqlClient.SqlCommand
		sqlCommand.CommandText = "SP_VINCOLO_RUOLI_ANTIMAFIA"
		sqlCommand.CommandType = CommandType.StoredProcedure
		sqlCommand.Connection = conn
        sqlCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
        sqlCommand.Parameters.AddWithValue("@SceltaFase", 2)
		sqlCommand.Parameters.AddWithValue("@Esito", IdEnte)
		sqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output
		sqlCommand.ExecuteNonQuery()

		Return sqlCommand.Parameters("@Esito").Value = 1
	End Function

    Public Function ControllaEsistenzaRuoliAntimafia(IdEnte As Integer, conn As SqlClient.SqlConnection) As Boolean
        Dim sqlCommand As New SqlClient.SqlCommand
        sqlCommand.CommandText = "SP_ESISTONO_RUOLI_ANTIMAFIA"
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Connection = conn
        sqlCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
        sqlCommand.Parameters.AddWithValue("@Esito", IdEnte)
        sqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output
        sqlCommand.ExecuteNonQuery()

        Return sqlCommand.Parameters("@Esito").Value = 1
    End Function

	Public Function AnnullaFaseAntimafia(IdEnte As Integer, conn As SqlClient.SqlConnection) As String
		Dim _info As New InfoAdeguamentoAntimafia(IdEnte, conn, False)

        If Not _info.isEntePrivato Then Return "Gli Enti pubblici non possono terminare Aggiornamenti Antimafia."
        If Not _info.isEnteTitolare Then Return "Gli Enti in accordo non possono terminare Aggiornamenti Antimafia."
		If _info.isAperto Then
			If _info.isAccreditamento Then
                Return "Non si possono terminare Aggiornamenti Antimafia durante l'Iscrizione."
			End If
		Else
            Return "Non esiste un Aggiornamento Antimafia avviato."
		End If


		Dim sqlCommand As New SqlClient.SqlCommand
		sqlCommand.CommandText = "SP_ANNULLA_FASE_ANTIMAFIA"
		sqlCommand.CommandType = CommandType.StoredProcedure
		sqlCommand.Connection = conn
		sqlCommand.Parameters.AddWithValue("@IdEnteFaseAntimafia", _info.IdEnteFaseAntimafia)
		sqlCommand.ExecuteNonQuery()
		Return ""
	End Function


	Private Function GetData(data As Object) As Object
		If IsDBNull(data) Then
			Return Nothing
		End If
		Return data
	End Function
End Class
