Public Class clsSiged
    Private _wsAuth As New WS_SIGeD_Auth.SIGED_AUTH

    Public Esito As String 'ritorna la descrizione dell'esito (IdSessione o Messaggio di errore)
    Public Codice_Esito As Integer
    Public Connessione As String
    Public CodiceFascicolo As String
    Public TipoNormalizzato As String 'RIPORTA SE IL CODICE è UN FASCICOLO, PROTOCOLLO O UN DOCUMENTO
    Public CodiceNormalizzato As String 'CODICE DEL FASCICOLO,PROTOCOLLO O DEL DOCUMENTO

    'Public CodiceDocumento As String

    Public Sub New(ByVal Sessione As String, ByVal strNome As String, ByVal strCognome As String)
        Dim str As String
        If Sessione = "" Then
            str = SIGED_Autenticazione(strNome, strCognome)
        Else
            Esito = Sessione
        End If
    End Sub

    Public Sub New()

    End Sub

    Public Function SIGED_Autenticazione(ByVal strNome As String, ByVal strCognome As String) As String
        'funzione per l'autentizacione
        Dim wsAuth As New WS_SIGeD_Auth.SIGED_AUTH
        Dim ws_siged_pwd As String = "webservice"
        Dim strAuth As String
        Try
            strAuth = wsAuth.SWS_NEWSESSION(strNome, strCognome, ws_siged_pwd)

            Codice_Esito = SIGED_Codice_Esito(strAuth)
            Esito = SIGED_Esito(strAuth)
            Connessione = Esito
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_Codice_Esito(ByVal strCodice As String) As Integer
        'funzione che ricava il codice esito dell'autenticazione
        Try
            Return Left(strCodice, 5)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_Esito(ByVal strCodice As String) As String
        'funzione ritorna idsessione se la sessione è andata a buon fine altrimenti ritorna messaggio di errore
        Try
            Return strCodice.Substring(8)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Sub SIGED_Chiudi_Autenticazione(ByVal strNome As String, ByVal strCognome As String)
        'funzione che chiude l'autenticazione
        Dim wsAuth As New WS_SIGeD_Auth.SIGED_AUTH

        Try
            wsAuth.Timeout = 300000
            wsAuth.SWS_CLOSESESSION(Connessione, strNome, strCognome)
            Esito = ""
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Sub

    Public Function SIGED_Ricerca_Fascicoli(ByVal Descrizione As String, ByVal Nominativo As String, ByVal Titolario As String, ByVal NumeroFascicolo As String, ByVal DataDA As String, ByVal DataA As String) As WS_SIGeD.MULTI_FASCICOLO
        'funzione per la ricerca i fascicoli esistenti
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Dim StrData As String = DataDA
        Try
            wsSiged.Timeout = 300000
            If DataA <> "" Then
                StrData += "@" & DataA
            End If
            Return wsSiged.RICERCAFASCICOLI(Connessione, "", Descrizione, Nominativo, Titolario, NumeroFascicolo, "", "", StrData, "")

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_Ricerca_Protocolli(ByVal Anno As String, ByVal NumeroProtocollo As String, ByVal DataProtocollazione As String, ByVal Corrispondente As String, ByVal Oggetto As String, ByVal TipoProtocollo As String, ByVal EstremiProtocollo As String, ByVal ProtocolloRifermento As String, ByVal Stato As String) As WS_SIGeD.MULTI_PROTOCOLLO
        'funzione per la ricerca i fascicoli esistenti
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.RICERCAPROTOCOLLI(Connessione, Anno, NumeroProtocollo, DataProtocollazione, Corrispondente, Oggetto, TipoProtocollo, EstremiProtocollo, ProtocolloRifermento, Stato)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_IndiceFascicoli(ByVal CodiceFascicolo As String) As WS_SIGeD.INDICE_FASCICOLO
        'funzione per la visualizzazione di tutti i fascicoli prenseti in griglia
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Dim codice As String
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.INDICEFASCICOLO(Connessione, CodiceFascicolo)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_CreaFascicoloExpress(ByVal CodiceProtocollo As String, ByVal Descrizione As String, ByVal Riferimento As String, ByVal CodiceTitolario As String, ByVal Stato As String, ByVal DataApertura As String, ByVal UnitaOrganizzativaResponsabile As String, ByVal Categoria As String, ByVal DescrizioneDocInt As String, ByVal NomeFile As String, ByVal Base64 As String, ByVal Path As String, ByVal CodiceDefault As String) As WS_SIGeD.FASCICOLOEX_CREATO
        'funzione che consente la creazione di un fasciolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CREAFASCICOLOEXPRESS(Connessione, CodiceProtocollo, Descrizione, Riferimento, CodiceTitolario, Stato, DataApertura, UnitaOrganizzativaResponsabile, Categoria, DescrizioneDocInt, NomeFile, Base64, Path, CodiceDefault)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_CreaCollegamentoFascicolo(ByVal CodiceFascicolo As String, ByVal CodiceDocumento As String) As String
        'funzione che consente di collegare al fascicolo dei documenti esiStenti (possono essere: fascicolo,protocollo o documenti)
        'vengono specificati dal TIPODOCUMENTO per quanto riguarda il documento non ho ha definizione corretta
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Dim strTipoDocumento As String
        Try
            wsSiged.Timeout = 300000

            NormalizzaCodice(CodiceDocumento)

            Select Case Me.TipoNormalizzato
                Case "FASC"
                    strTipoDocumento = "FASCICOLO"
                Case "PROT"
                    strTipoDocumento = "PROTOCOLLO"
                Case Else
                    'caso del documento interno
                    strTipoDocumento = ""
            End Select

            Return wsSiged.CREACOLLEGAMENTOFASCICOLO(Connessione, strTipoDocumento, CodiceFascicolo, CodiceDocumento, "", "")

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_IndiceProtocollo(ByVal CodiceProtocollo As String) As WS_SIGeD.INDICE_PROTOCOLLO
        'funzione che riporta l'elenco dei protocoli di un dato fascicolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Dim codice As String
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.INDICEPROTOCOLLO(Connessione, CodiceProtocollo)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_RestituisciAllegato(ByVal Anno As Integer, ByVal CodiceAllegato As String, ByVal FlagBase64 As String, ByVal Path As String) As WS_SIGeD.INDICE_ALLEGATO
        'funzione che riporta gli allegati di un dato protocollo
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Dim codice As String

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.RESTITUISCIALLEGATO(Connessione, Anno, CodiceAllegato, FlagBase64, Path)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_CreaProtocolloExpress(ByVal Anno As String, ByVal TipoProtocollo As String, ByVal CodAnagrafica As String, ByVal CNominativo As String, ByVal CIndirizzo As String, ByVal CCitta As String, ByVal CCap As String, ByVal CProv As String, ByVal CAzienda As String, ByVal CorrispondenteCodiceUnivoco As String, ByVal Oggetto As String, ByVal Responsabile As String, ByVal TipoDocumento As String, ByVal CodTitolario As String, ByVal Estremi As String, ByVal DataEstr As String, ByVal ProtRif As String, ByVal Allegato As String, ByVal NomeFile As String, ByVal AllB64 As String, ByVal AllegatoPath As String, ByVal CodDefault As String) As WS_SIGeD.PROTOCOLLOEX_CREATO
        Try
            'funzione che consente la creazione di un protocollo                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            Dim wsSiged As New WS_SIGeD.SIGED_WS
            'Dim a() As WS_SIGeD.MULTI_ANAG
            Dim wsAnag As New WS_SIGeD.ANAG_MULTI

            'ReDim a(0)
            'a(0) = New WS_SIGeD.MULTI_ANAG

            'a(0).AZIENDA = ""
            'a(0).CAP = ""
            'a(0).CITTA = ""
            'a(0).CODICEUNIVOCO = ""
            'a(0).INDIRIZZO = ""
            'a(0).NOMINATIVO = ""
            'a(0).PROVINCIA = ""

            'wsAnag.DATO = a
            wsSiged.Timeout = 300000
            Return wsSiged.CREAPROTOCOLLOEXPRESS(Connessione, Anno, TipoProtocollo, CodAnagrafica, CNominativo, CIndirizzo, CCitta, CCap, CProv, CAzienda, CorrispondenteCodiceUnivoco, wsAnag, Oggetto, Responsabile, TipoDocumento, CodTitolario, Estremi, DataEstr, ProtRif, Allegato, NomeFile, AllB64, AllegatoPath, CodDefault, "")
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_CreaAllegato(ByVal Anno As Integer, ByVal NumeroProtocollo As String, ByVal DescrDocPrincipale As String, ByVal FlagPricipale As String, ByVal NomeFile As String, ByVal Base64 As String, ByVal Path As String) As String
        'funzione che inserisce gli allegati(indentificato come documento principale o allegato --> FlagPricipale SI o NO ) al protocollo
        Dim wsSiged As New WS_SIGeD.SIGED_WS

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CREAALLEGATO(Connessione, Anno, NumeroProtocollo, DescrDocPrincipale, FlagPricipale, NomeFile, Base64, Path)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function NormalizzaCodice(ByVal Codice As String) As String
        'funzione che identifica il codice se fascicolo,protocollo o documento
        Dim strCodice
        Dim sCod() As String
        Dim str As String
        Dim i As Integer
        Dim intLen As Integer
        sCod = Split(Codice, "#")

        intLen = sCod.Length

        If intLen = 2 Then
            CodiceNormalizzato = sCod(1)
            TipoNormalizzato = "DOCU"
        Else
            For i = 0 To UBound(sCod)
                If i = UBound(sCod) Then
                    TipoNormalizzato = sCod(i - 1)
                    CodiceNormalizzato = sCod(i)
                End If
            Next
        End If

    End Function

    Public Function SIGED_CodiceProtocolloCompleto(ByVal AnnoProtocollo As String, ByVal NumeroProtocollo As String) As String
        'funzione che genera l'idprotocollo , concatenando l'anno #  PROT # numero protocollo
        Dim strCodice As String
        Try
            strCodice = AnnoProtocollo & "#" & "PROT" & "#" & NumeroProtocollo
            Return strCodice
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_IdFascicoloCompleto(ByVal IdFascicolo As String) As String
        'funzione che genera l'idprotocollo , concatenando l'anno #  PROT # numero protocollo
        Dim strCodice As String
        Try
            strCodice = Right(IdFascicolo, 4) & "#" & "FASC" & "#" & IdFascicolo
            Return strCodice
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_CodiceAllegatoCompleto(ByVal IdAllegato As String, ByVal TipoAllegato As String, ByVal NumProtocollo As String, ByVal AnnoProtocollo As String) As String
        ' CREATA IL 14/05/2012 DA SIMONA CORDELLA
        'funzione che genera l'IDALLEGATO , concatenando l'anno #  PROTO # numero protocollo # TipoAllegato # IDALLEGATO
        'modificato il 15/06/2012 da simona cordella
        'il campo TipoAllegato riporta PRINC (documento principale) ALLE (documento allegato)

        Dim strCodice As String
        Try
            strCodice = AnnoProtocollo & "#" & "PROTO" & "#" & NumProtocollo & "#" & TipoAllegato & "#" & IdAllegato
            Return strCodice
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function NormalizzaNumeroFascicolo(ByVal Codice As String) As String
        'funzione che converte il numero fascicolo in intero 
        Dim strCodice As String
        Dim strTitolatio As String
        Dim sCod() As String
        Dim i As Integer

        sCod = Split(Codice, "/")
        For i = 0 To UBound(sCod)
            If i = UBound(sCod) Then
                strCodice = sCod(i - 1)
                strTitolatio = sCod(i)
            End If
        Next
        Return Replace(strTitolatio, " ", "") & "/" & strCodice
    End Function

    Public Function Esito_Occorrenze(ByVal strEsito As String) As Integer
        'funzione che ricava il numero di occorrene che estrae nella ricerca del fascicolo
        Dim sOcc() As String
        Dim i As Integer
        Dim strEsitoOcc As String
        Try
            sOcc = Split(strEsito, "-")
            For i = 0 To UBound(sOcc)
                If i = UBound(sOcc) Then
                    strEsitoOcc = sOcc(i)
                End If
            Next

            Return strEsitoOcc
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_CreaFascicolo(ByVal Descrizione As String, ByVal Riferimento As String, ByVal CodiceTitolario As String, ByVal Stato As String, ByVal DataApertura As String, ByVal UnitaOrganizzativaResponsabile As String, ByVal Categoria As String, ByVal CodiceDefault As String) As WS_SIGeD.FASCICOLO_CREATO
        'funzione che consente la creazione di un fasciolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CREAFASCICOLO(Connessione, Descrizione, Riferimento, CodiceTitolario, Stato, DataApertura, UnitaOrganizzativaResponsabile, Categoria, CodiceDefault)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_ConvertiMultiValore(ByVal Valore As String) As WS_SIGeD.DATO_MULTI
        'funzione che consente la creazione di un fasciolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CONVERTIINMULTIVALORE(Valore)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_CreaFascicoloMultiplo(ByVal Descrizione As WS_SIGeD.DATO_MULTI, ByVal Riferimento As String, ByVal CodiceTitolario As String, ByVal Stato As String, ByVal DataApertura As String, ByVal UnitaOrganizzativaResponsabile As String, ByVal Categoria As String, ByVal CodiceDefault As String) As WS_SIGeD.MULTI_FASCICOLO_CREATO
        'funzione che consente la creazione di un fasciolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CREAFASCICOLOMULTIPLO(Connessione, Descrizione, Riferimento, CodiceTitolario, Stato, DataApertura, UnitaOrganizzativaResponsabile, Categoria, CodiceDefault)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function
    Public Function SIGED_CreaDocumentoInterno(ByVal CodiceFascicolo As String, ByVal Descrizione As String, ByVal NomeFile As String, ByVal Base64 As String, ByVal Path As String) As String
        'funzione che consente la creazione di un fasciolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CREADOCUMENTOINTERNO(Connessione, CodiceFascicolo, Descrizione, NomeFile, Base64, Path)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function
    Public Function SIGED_RestituisciDocumentoInterno(ByVal CodiceAllegato As String, ByVal Base64 As String, ByVal Path As String) As WS_SIGeD.INDICE_ALLEGATO
        'funzione che restituisce nl documento interno al fascicolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.RESTITUISCIDOCUMENTOINTERNO(Connessione, CodiceAllegato, Base64, Path)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function
    Public Function RICERCAUNITAORGANIZZATIVE() As WS_SIGeD.MULTI_UNITAORGANIZZATIVA

        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.RICERCAUNITAORGANIZZATIVE(Connessione, "", "")
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function
    Public Function LISTATIPODOCUMENTO() As WS_SIGeD.RISPOSTA_MULTI_VALORE
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.LISTATIPODOCUMENTO(Connessione)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function
    Public Function LISTACATEGORIEFASCICOLI() As WS_SIGeD.RISPOSTA_MULTI_VALORE
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.LISTACATEGORIEFASCICOLI(Connessione)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_NumeroAllegato(ByVal strCodice As String) As String
        'funzione ritorna il numero dell'allegato del documento interno al fascicolo
        Try

            Return Mid(strCodice, InStr(strCodice, "#") + 1, Len(strCodice))

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
End Class
