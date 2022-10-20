Imports System.IO
Public Class WfrmSIGEDProtocollazione
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
  
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents imgUpLoad As System.Web.UI.WebControls.Image
    Protected WithEvents lblTotOk As System.Web.UI.WebControls.Label
    Protected WithEvents lblTot As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotKo As System.Web.UI.WebControls.Label
    Protected WithEvents txtCodFasc As System.Web.UI.WebControls.HiddenField

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    Shared strNome As String
    Shared strCognome As String
    Shared strServizio As String
    Dim NumProt As String
    Dim DataProt As String
    Dim SIGED As clsSiged
    Dim IdFascicolo As String
    Dim numFascicolo As String
    Dim DescrFascicolo As String
    '** aggiunto da simona cordella il 26/01/2011
    Shared Fascicolo As String '= ""
    Dim TitolarioFascicolo As String = ""

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Page.IsPostBack = False Then

            '**
            If Request.QueryString("NumeroFascicolo") <> "" Then
                txtCodFasc.Value = Request.QueryString("NumeroFascicolo") 'riporta idfascicolo
                txtDescFascicolo.Text = Request.QueryString("DescFascicolo")
                txtDescFascicolo.ReadOnly = True
                '** aggiunto da simona cordella il 26/01/2011
                Fascicolo = Request.QueryString("Fascicolo") 'riporta il numero cascicolo
                TitolarioFascicolo = Mid(Fascicolo, 1, InStr(Fascicolo, "/") - 1)
                '** 
            End If
            cboTipoDocumento.Items.Add("Lettera")
            cboTipoDocumento.Items.Add("FAX")

            'modificato da simona cordella il 20/12/2010
            'sostituito dal caricemnto del titolario dal db
            CaricaTitolario(Request.QueryString("Processo"))
            '*** Aggiunto il 26/01/2011 da simona cordella
            If TitolarioFascicolo <> "" Then
                ' Dim I As Integer = cboTitolario.ID.IndexOf(Fascicolo)
                Try
                    cboTitolario.SelectedValue = Replace(TitolarioFascicolo, " ", "")
                Catch ex As Exception
                    lblMessaggio.Text = "Titolario non trovato"
                    cboTitolario.SelectedValue = 0
                End Try
            End If
            '****

        End If
    End Sub

    Private Sub imgConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgConferma.Click

        Dim wsOUT As New WS_SIGeD.PROTOCOLLOEX_CREATO
        Dim wsOUTFasc As New WS_SIGeD.FASCICOLOEX_CREATO
        Dim wsCollFascicolo As String
        Dim wsOUTAllegato As String

        Dim myPath As New System.Web.UI.Page
        Dim NomeUnivoco As String
        Dim sEstensioneFile As String
        Dim sNomeFile As String
        Dim i As Integer
        Dim strFlagProtocollo As String = "NO"
        Dim strFlagFascicolo As String = "NO"
        Dim strFlagPrincipale As String = "NO"
        Dim strCodiceFascicolo As String = "*****"
        Dim dtrServizi As SqlClient.SqlDataReader
        'dati richiedente
        Dim strSQL As String
        Dim DataSetRicerca As DataSet
        Dim strNominativo As String = ""
        Dim strCAP As String = ""
        Dim strComune As String = ""
        Dim strIndirizzo As String = ""
        Dim strProvincia As String = ""
        Dim dsUser As DataSet
        Dim strPathSiged As String
        Dim strPathLocale As String
        Dim strCorrispondeteCodiceUnivoco As String
        If (ValidaCampi() = True) Then


            Try
                If Request.QueryString("CorrCodUnivoco") = Nothing Then
                    strCorrispondeteCodiceUnivoco = ""
                Else
                    strCorrispondeteCodiceUnivoco = Request.QueryString("CorrCodUnivoco")
                End If

                'ricavo nome e cognome dell'utente loggato

                strSQL = " Select u.Nome, u.Cognome, s.descrizione " & _
                         " From UtentiUNSC u INNER JOIN TServiziSiged  s on  u.idservizio = s.idservizio " & _
                         " Where u.UserName='" & Session("Utente") & "'"
                dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

                If dsUser.Tables(0).Rows.Count <> 0 Then
                    strNome = dsUser.Tables(0).Rows(0).Item("Nome")
                    strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
                    strServizio = dsUser.Tables(0).Rows(0).Item("descrizione")
                End If

                SIGED = New clsSiged("", strNome, strCognome)
                If SIGED.Codice_Esito <> 0 Then
                    lblmessaggio.Text = SIGED.Esito
                    Exit Sub
                End If


                'estraggo l'estensione del file
                'sEstensioneFile = Right(txtSelFile.Value, 3)
                sEstensioneFile = System.IO.Path.GetExtension(txtSelFile.PostedFile.FileName)

                'estraggo il nome del file
                For i = Len(txtSelFile.PostedFile.FileName) To 1 Step -1
                    If InStr(Mid(txtSelFile.PostedFile.FileName, i, 1), "\") Then
                        sNomeFile = Mid(sNomeFile, 1, InStr(sNomeFile, ".") - 1)
                        Exit For
                    Else
                        sNomeFile = Mid(txtSelFile.PostedFile.FileName, i, 1) & sNomeFile
                    End If
                Next

                'ADC 23/05/2019
                sNomeFile = Replace(Replace(Replace(Replace(sNomeFile, "&", ""), "#", ""), "’", "'"), "+", " ")
                'costruisco un nome file univoco
                'NomeUnivoco = "FileSIGED-" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now) & "." & sEstensioneFile

                'salvo il file sul server

                File.Delete(Server.MapPath("upload") & "\" & sNomeFile & sEstensioneFile)

                txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & sNomeFile & sEstensioneFile)


                'strPathSiged = FileCopy(txtSelFile.Value, sNomeFile & "." & sEstensioneFile)

                ' 'converto il file in base64
                'Dim testoBase64 As String
                'testoBase64 = FileToBase64(myPath.Server.MapPath("upload/" & NomeUnivoco))

                'PathServerSiged = "\\" & ConfigurationSettings.AppSettings("SERVER_SIGED") & "\" & ConfigurationSettings.AppSettings("CARTELLA_SIGED") & "\" & sNomeFile & "." & sEstensioneFile
                strPathSiged = FileCopy(Server.MapPath("upload") & "\" & sNomeFile & sEstensioneFile, sNomeFile & sEstensioneFile)

                If chkProtocollo.Checked = True Then strFlagProtocollo = "SI"
                If txtCodFasc.Value = "" Then
                    strFlagFascicolo = "SI"
                Else
                    strCodiceFascicolo = Request.QueryString("NumeroFascicolo")
                End If

                If Not Request.QueryString("IdIGF") Is Nothing Then
                    '**** MODIFICATO IL 25/01/2008 DA SIMONA CORDELLA
                    '**** NEL CAMPO INDIRIZZO, NEL CASO DI VERIFICATORE INTERNO, 
                    '**** NON SCARICO PIU' L'INDIRIZZO DELLA SEDE MA LA DICITURA "SEDE" 
                    If CInt(Request.QueryString("IdIGF")) <> 0 Then
                        '***Destinatario per tutte le richieste di protocollazione = dall'incarico
                        strSQL = "SELECT Cognome + ' ' + Nome As Nominativo, isnull(Riferimento, '__') as Indirizzo,tipologia " & _
                                "  FROM TVerificatori WHERE IDVerificatore=" & Request.QueryString("IdIGF")
                        DataSetRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))

                        If DataSetRicerca.Tables(0).Rows.Count <> 0 Then
                            strNominativo = "" & DataSetRicerca.Tables(0).Rows(0).Item("Nominativo")
                            strCAP = "" & "00000"
                            strComune = "" & "__"
                            If DataSetRicerca.Tables(0).Rows(0).Item("Tipologia") = True Then 'igf
                                strIndirizzo = "" & Replace(DataSetRicerca.Tables(0).Rows(0).Item("Indirizzo"), "\par", " ")
                            Else 'interno
                                'strIndirizzo = "" & "SEDE"
                                strIndirizzo = "" & DataSetRicerca.Tables(0).Rows(0).Item("Indirizzo")
                            End If
                            strProvincia = "" & "__"
                        End If
                        DataSetRicerca.Dispose()
                    Else
                        'Aggiunto da s.c. il 04/03/2011 
                        'Gestione della protocollazione "TRASMISSIONE SANZIONE SERVIZI"
                        'Se richiamo la protocollazione Sanzione Servizi (("SanzServ")) = 1) imposto 
                        'come destinatari i Responsabili dei servizi che ho indicato sulal verifica
                        If CInt(Request.QueryString("SanzServ")) = 1 Then '
                            Dim idVerifica As Integer = Request.QueryString("IdVerifica")
                            '***Destinataro/i diregente/i del/i servizi/o che sono stati indicati nella verifica
                            strSQL = " SELECT TServiziSiged.Descrizione as Servizio " & _
                                     " FROM  TVerificheServizi  INNER JOIN TServiziSiged ON TVerificheServizi.IDServizio = TServiziSiged.IDServizio " & _
                                     " WHERE IdModello =94 and IdVerifica=" & idVerifica & ""
                            dtrServizi = ClsServer.CreaDatareader(strSQL, Session("conn"))
                            Do While dtrServizi.Read
                                If strNominativo = "" Then
                                    strNominativo = "" & dtrServizi("Servizio")
                                Else
                                    strNominativo = strNominativo & "#" & dtrServizi("Servizio")
                                End If
                            Loop
                            strCAP = "" & "00000"
                            strComune = "" & "__"
                            strIndirizzo = "" & "SEDE"
                            strProvincia = "" & "__"

                            dtrServizi.Close()
                            dtrServizi = Nothing
                        Else
                            'aggiunto il 08/0/2008 da simona cordella
                            'Se protocollo una lettera al direttore generale devo indicare come indirizzo la dicitura "SEDE"
                            If CInt(Request.QueryString("DR")) = 0 Then
                                '***Destinatario per tutte le richieste di protocollazione <> dall'incarico (CREDENZIALI)
                                strSQL = "SELECT  Denominazione, Indirizzo, Cap, Comune, ProvinciaBreve as Provincia " & _
                                    "  FROM vw_bo_enti WHERE (CodiceRegione ='" & Request.QueryString("CodEnte") & "')"
                                DataSetRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))

                                If DataSetRicerca.Tables(0).Rows.Count <> 0 Then
                                    strNominativo = "" & DataSetRicerca.Tables(0).Rows(0).Item("Denominazione")
                                    strCAP = "" & DataSetRicerca.Tables(0).Rows(0).Item("Cap")
                                    strComune = "" & DataSetRicerca.Tables(0).Rows(0).Item("Comune")
                                    strIndirizzo = "" & DataSetRicerca.Tables(0).Rows(0).Item("Indirizzo")
                                    strProvincia = "" & DataSetRicerca.Tables(0).Rows(0).Item("Provincia")
                                End If
                                DataSetRicerca.Dispose()
                            Else 'protocollo lettara al DIRETTORE GENERALE
                                strNominativo = "" & "DIRETTORE GENERALE"
                                strCAP = "" & "00000"
                                strComune = "" & "__"
                                strIndirizzo = "" & "SEDE"
                                strProvincia = "" & "__"
                            End If
                        End If
                    End If

                    ''*** Aggiunto il 07/01/2008 da Simona Cordella
                    ''*** multi destinatario
                    ''*** strNominativo = strNominativo +#DenomEnteFiglio

                    'Dim dtrMultiDest As SqlClient.SqlDataReader
                    'Dim strDenFiglio As String
                    'strSQL = "SELECT EnteFiglio FROM VER_VW_RICERCA_VERIFICHE WHERE IDVerifica='" & Request.QueryString("IDVerifica") & "'"
                    'dtrMultiDest = ClsServer.CreaDatareader(strSQL, Session("conn"))
                    'If dtrMultiDest.HasRows = True Then
                    '    Do While dtrMultiDest.Read
                    '        strDenFiglio = "#" & dtrMultiDest("EnteFiglio")
                    '        strNominativo = strNominativo & strDenFiglio
                    '    Loop
                    'End If
                    'If Not dtrMultiDest Is Nothing Then
                    '    dtrMultiDest.Close()
                    '    dtrMultiDest = Nothing
                    'End If
                Else ' NEL CASO DELL'ASSEGNA VERIFICATORE
                    strNominativo = "" & "__"
                    strCAP = "" & "00000"
                    strComune = "" & "__"
                    strIndirizzo = "" & "__"
                    strProvincia = "" & "__"
                End If
                'avvio la PROTOCOLLAZIONE
                wsOUT = SIGED.SIGED_CreaProtocolloExpress(Year(Now), "USCITA", "", strNominativo, strIndirizzo, strComune, strCAP, strProvincia, "", strCorrispondeteCodiceUnivoco, Replace(txtOggetto.Text, "'", "''"), strServizio, cboTipoDocumento.SelectedItem.Text, cboTitolario.SelectedValue, "", "", "", "", "", "", "", "")   '"FASC0001"

                'esito fascicolazione
                'If Left(wsOUT.ESITO, 5) <> "00000" Then          'errore
                If SIGED.SIGED_Codice_Esito(wsOUT.ESITO) <> "0" Then
                    'lblMessaggio.Text = Mid(wsOUT.ESITO, 8, Len(wsOUT.ESITO))
                    lblmessaggio.Text = SIGED.SIGED_Esito(wsOUT.ESITO)
                Else
                    'crea allegato
                    wsOUTAllegato = SIGED.SIGED_CreaAllegato(Year(Now), wsOUT.NUMEROPROTOCOLLO, "", strFlagPrincipale, sNomeFile & "." & sEstensioneFile, "", strPathSiged)
                    'If Left(wsOUTAllegato, 5) <> "00000" Then          'errore
                    If SIGED.SIGED_Codice_Esito(wsOUTAllegato) <> "0" Then
                        'lblMessaggio.Text = Mid(wsOUTAllegato, 8, Len(wsOUTAllegato))
                        lblmessaggio.Text = SIGED.SIGED_Esito(wsOUTAllegato)
                    Else
                        'Verifico esisteNza fascicolo e collego il nuovo protocollo al fasciciolo esistente
                        If txtCodFasc.Value <> "" Then
                            IdFascicolo = SIGED.SIGED_IdFascicoloCompleto(txtCodFasc.Value)

                            wsCollFascicolo = SIGED.SIGED_CreaCollegamentoFascicolo(IdFascicolo, wsOUT.CODICEPROTOCOLLO)

                            SIGED.NormalizzaCodice(IdFascicolo)
                            IdFascicolo = SIGED.CodiceNormalizzato
                            numFascicolo = Fascicolo
                            DescrFascicolo = txtDescFascicolo.Text
                        Else 'se non ho nessun fascicolo ne creo uno nuovo  e salvo id, nuemro e descrizioni nelel variabili di appoggio
                            wsOUTFasc = SIGED.SIGED_CreaFascicoloExpress(wsOUT.CODICEPROTOCOLLO, Replace(txtDescFascicolo.Text, "'", "''"), "", cboTitolario.SelectedValue, "", "", strServizio, "", "", "", "", "", "")
                            SIGED.NormalizzaCodice(wsOUTFasc.CODICEFASCICOLO)
                            IdFascicolo = SIGED.CodiceNormalizzato
                            numFascicolo = cboTitolario.SelectedValue & "/" & CInt(wsOUTFasc.NUMEROFASCICOLO)  '4.29.4.2/01624
                            DescrFascicolo = txtDescFascicolo.Text
                        End If

                        If (Request.QueryString("VArUpdate") <> 0 Or Request.QueryString("VArUpdate") = Nothing) Then 'aggiunta questa IF da Antonello per la provenienza dalla maschera del volontario

                            NumProt = CInt(wsOUT.NUMEROPROTOCOLLO)
                            DataProt = FormatDateTime(wsOUT.DATAPROTOCOLLO, DateFormat.ShortDate)

                            'torno i valori alla form chiamante
                            Response.Write("<script>" & vbCrLf)
                            Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("TxtFasc") & ".value='" & IdFascicolo & "';")
                            Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("TxtCodFasc") & ".value='" & Replace(numFascicolo, " ", "") & "';")
                            '**
                            If InStr(txtDescFascicolo.Text, """") + InStr(txtDescFascicolo.Text, "'") > 0 Then
                                If InStr(txtDescFascicolo.Text, """") > 0 Then
                                    Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("TxtDescFascicolo") & ".value='" & Trim(DescrFascicolo.Replace(vbCrLf, " ")) & "';")
                                End If
                                If InStr(txtDescFascicolo.Text, "'") > 0 Then
                                    Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("TxtDescFascicolo") & ".value=""" & Trim(DescrFascicolo.Replace(vbCrLf, " ")) & """;")
                                End If
                            Else
                                Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("TxtDescFascicolo") & ".value=""" & Trim(DescrFascicolo.Replace(vbCrLf, " ")) & """;")
                            End If
                        End If 'fine Antonello
                        If Request.QueryString("VArUpdate") = 1 Then
                            If strFlagFascicolo = "SI" Then
                                'SALVO IL FASCICOLO 'CODICE FASCICOLO
                                TrovaFascicolo(Session("Utente"), IdFascicolo, numFascicolo, DescrFascicolo, Request.QueryString("VarVolontario"))
                            End If

                            If Request.QueryString("TipoStampa") = "LettSub" Or Request.QueryString("TipoStampa") = "LettSubB" Then
                                'stampo la lettera soubentro solo nella maschera WfrmElencoDocumentazioneVolontario
                                UpdateElencoDocumentiPerLetteraSubentro()
                            Else
                                Call UpdateElencoDocumenti(Request.QueryString("VarVolontario"))
                            End If
                            If Trim(Request.QueryString("VengoDa")) = Nothing Then
                                '**COMMENTATO I 19/09/2011 VA RIPRISTIBATO CON IL WEB SERVICE EPR IL FASCICOLO
                                Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("txtcodicefascicolonascosto") & ".value='" & IdFascicolo & "';")
                                Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("txtnumerofascicoloinvisione") & ".value='" & Replace(numFascicolo, " ", "") & "';")
                                '**
                                Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("txtdescrizionefascicolo") & ".value=""" & Trim(DescrFascicolo.Replace(vbCrLf, " ")) & """;")
                            End If
                        ElseIf Request.QueryString("VArUpdate") = 2 Then

                            If strFlagFascicolo = "SI" Then
                                'SALVO IL FASCICOLO
                                TrovaFascicolo(Session("Utente"), IdFascicolo, numFascicolo, DescrFascicolo, Request.QueryString("VarVolontario"))
                            End If

                            Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("txtnumerofascicoloinvisione") & ".value='" & Replace(numFascicolo, " ", "") & "';")
                            Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("txtdescrizionefascicolo") & ".value=""" & Trim(DescrFascicolo.Replace(vbCrLf, " ")) & """;")
                        Else '=3 
                            If strFlagFascicolo = "SI" Then
                                'SALVO IL FASCICOLO
                                TrovaFascicolo(Session("Utente"), IdFascicolo, numFascicolo, DescrFascicolo, Request.QueryString("IdAttivita"))
                                '**
                            End If
                            UpdateElencoEntiDocumenti()
                        End If
                        If (Request.QueryString("VArUpdate") <> 0 Or Request.QueryString("VArUpdate") = Nothing) Then 'IF aggiunta da Antonello
                            Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("TxtProt") & ".value='" & NumProt & "';")

                            If wsOUT.DATAPROTOCOLLO <> "" Then
                                Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("TxtData") & ".value='" & DataProt & "';")
                            End If
                            If Request.QueryString("VengoDa") = "Stampa" Then
                                Response.Write("window.opener.document." & Request.QueryString("objForm") & "." & Request.QueryString("objHddModifica") & ".value=1;")
                            End If
                            Response.Write("window.close()")
                            Response.Write("</script>")
                        End If ' fine IF Antonello
                        If Request.QueryString("VArUpdate") = 0 Then
                            Response.Write("<script>" & vbCrLf)
                            Response.Write("window.close()")
                            Response.Write("</script>")
                        End If
                    End If
                End If
                SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
            Catch ex As Exception
                lblmessaggio.Text = "Errore imprevisto: " & ex.Message
            End Try
            imgConferma.Enabled = True

        End If

    End Sub

    Private Function TrovaFascicolo(ByVal strUserName As String, ByVal IdFascicolo As String, ByVal NumFascicolo As String, ByVal DescrFascicolo As String, ByVal Valore As String) As String

        'Dim wsOUT As WS_SIGeD.MULTI_FASCICOLO
        'Dim wsELENCO() As WS_SIGeD.FASCICOLO_DOCUMENTO_TROVATO
        Dim sEsito As String

        Dim strSQL As String
        Dim DataSetRicerca As DataSet
        Dim strDescrizioneFascicolo As String
        Dim strNomeUtente As String
        Dim strCognomeUtente As String
        Dim riga As Integer
        Dim strNumeroFascicolo As String
        Dim DescrizioneFascicolo As String

        Dim CmdGenerico As SqlClient.SqlCommand
        'Dim CodFascicolo As String
        Try
            ''***Destinatario per tutte le richieste di protocollazione = dall'incarico
            'strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & strUserName & "'"

            'DataSetRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))

            'strNomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Nome")
            'strCognomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Cognome")

            'DataSetRicerca.Clear()

            'SIGED = New clsSiged("", strNome, strCognome)
            'If SIGED.Codice_Esito = 0 Then

            '    wsOUT = SIGED.SIGED_Ricerca_Fascicoli("", "", "", CodFascicolo, "", "")
            'Else
            '    lblMessaggio.Text = SIGED.Esito
            'End If
            'sEsito = Left(wsOUT.ESITO, 5)
            'If sEsito = "00000" Then

            '    If Right(wsOUT.ESITO, 1) = "0" Then

            '    Else
            '        wsELENCO = wsOUT.ELENCO_DOCUMENTI

            '        For riga = LBound(wsELENCO) To UBound(wsELENCO)
            '            If Not wsELENCO(riga).NUMERO Is Nothing Then
            '                'L'utente loggato ha accesso al fascicolo
            '                'If wsELENCO(riga).DATA_CREAZIONE <> "" Then

            strNumeroFascicolo = NumFascicolo
            DescrizioneFascicolo = DescrFascicolo

            If Request.QueryString("VArUpdate") = 1 Then
                Call UpdateFascicoloEntità(strNumeroFascicolo, IdFascicolo, DescrizioneFascicolo, Valore)
            ElseIf Request.QueryString("VArUpdate") = 2 Then
                Call SalvaProtocolloAssocia(strNumeroFascicolo, IdFascicolo, DescrizioneFascicolo, Valore)
            ElseIf Request.QueryString("VArUpdate") = 3 Then 'agg. da sc per salvataggio in cronologiaentidocumenti
                Call UpdateFascicoloProgetto(strNumeroFascicolo, IdFascicolo, DescrizioneFascicolo, Valore)
            End If
            '                'End If
            '            End If
            '        Next
            '    End If
            'Else
            '    TrovaFascicolo = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))

            'End If


            'wsOUT = wsIN.RICERCA_FASCICOLI(strCognomeUtente, strNomeUtente, "500", IdFascicolo, "", "", "", "", "", "")

            'sEsito = Left(wsOUT.ESITO, 4)

            'If sEsito = "0000" Then

            '    If Right(wsOUT.ESITO, 1) = "0" Then

            '    Else
            '        wsELENCO = wsOUT.ELENCO_FASCICOLI
            '        For riga = LBound(wsELENCO) To UBound(wsELENCO)
            '            If Not wsELENCO(riga).NUMERO_FASCICOLO Is Nothing Then
            '                'L'utente loggato ha accesso al fascicolo
            '                If wsELENCO(riga).DATA_CREAZIONE <> "" Then
            '                    strNumeroFascicolo = wsELENCO(riga).NUMERO_FASCICOLO
            '                    DescrizioneFascicolo = wsELENCO(riga).DESCRIZIONE

            '                    If Request.QueryString("VArUpdate") = 1 Then
            '                        Call UpdateFascicoloEntità(strNumeroFascicolo, IdFascicolo, DescrizioneFascicolo, Valore)
            '                    ElseIf Request.QueryString("VArUpdate") = 2 Then
            '                        Call SalvaProtocolloAssocia(strNumeroFascicolo, IdFascicolo, DescrizioneFascicolo, Valore)
            '                    ElseIf Request.QueryString("VArUpdate") = 3 Then 'agg. da sc per salvataggio in cronologiaentidocumenti
            '                        Call UpdateFascicoloProgetto(strNumeroFascicolo, IdFascicolo, DescrizioneFascicolo, Valore)
            '                    End If
            '                End If
            '            End If
            '        Next
            '    End If
            'Else
            '    TrovaFascicolo = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))

            'End If
            SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
        Catch ex As Exception
            TrovaFascicolo = ex.Message
            DataSetRicerca.Dispose()
        End Try
    End Function
    Private Sub SalvaProtocolloAssocia(ByVal strNumeroFascicolo As String, ByVal IdFascicolo As String, ByVal DescrizioneFascicolo As String, ByVal strIdEntita As String)

        Dim strLocal As String
        Dim dtrCancellazione As SqlClient.SqlDataReader
        Dim mycommand As New SqlClient.SqlCommand
        Dim mydatatable As New DataTable

        mycommand.Connection = Session("conn")
        'cancella
        strLocal = "select IDAttivitàSedeAssegnazione from attività inner join attivitàsediassegnazione on " & _
        "attività.idattività = attivitàsediassegnazione.idattività where(attività.idattività = " & Request.QueryString("IdAttivita") & ")"
        Try
            mydatatable = ClsServer.CreaDataTable(strLocal, False, Session("conn"))

            Dim k As Int16

            For k = 0 To mydatatable.Rows.Count - 1
                strLocal = "update cronologiaentidocumenti set dataprot =null,  nprot = null  " & _
                    " where tipodocumento = 2 and IDAttivitàSedeAssegnazione = " & mydatatable.Rows(k).Item("IDAttivitàSedeAssegnazione")

                mycommand.CommandText = strLocal
                mycommand.ExecuteNonQuery()
            Next
            '*******************************************************************************
        Catch ex As Exception
            'Response.Write(ex.Message.ToString())
        End Try


        strLocal = "update attività  set codicefascicoloai ='" & strNumeroFascicolo & "', idfascicoloai='" & IdFascicolo & _
        "', descrfascicoloai='" & DescrizioneFascicolo & "' where IdAttività = " & Request.QueryString("IdAttivita") & ""
        mycommand.CommandText = strLocal
        mycommand.ExecuteNonQuery()

        'Response.Redirect("WfrmRicercaAttivitaGraduatoria.aspx?CheckIndietro=" & Request.QueryString("CheckIndietro") & "&Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Progetto=" & Request.QueryString("Progetto") & "&Bando=" & Request.QueryString("Bando") & "&Settore=" & Request.QueryString("Settore") & "&Area=" & Request.QueryString("Area") & "&InAttesa=" & Request.QueryString("InAttesa") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&Vengoda=" & Request.QueryString("Azione") & "")

    End Sub

    Sub UpdateFascicoloEntità(ByVal strNumeroFascicolo As String, ByVal IdFascicolo As String, ByVal DescrizioneFascicolo As String, ByVal strIdEntita As String)
        Dim strSQL As String
        Dim CmdGenerico As SqlClient.SqlCommand
        strSQL = " Update entità set CodiceFascicolo= '" & strNumeroFascicolo & "', " & _
                 " IDFascicolo ='" & IdFascicolo & "', " & _
                 " DescrFascicolo = '" & Replace(DescrizioneFascicolo, "'", "''") & "' " & _
                 " where identità=" & strIdEntita & ""
        CmdGenerico = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
    End Sub
    Sub UpdateFascicoloProgetto(ByVal strNumeroFascicolo As String, ByVal IdFascicolo As String, ByVal DescrizioneFascicolo As String, ByVal IdAttività As Integer)
        Dim strSQL As String
        Dim CmdGenerico As SqlClient.SqlCommand

        strSQL = " Update attività set CodiceFascicoloAI= '" & strNumeroFascicolo & "', " & _
                 " IDFascicoloAI ='" & IdFascicolo & "', " & _
                 " DescrFascicoloAI = '" & Replace(DescrizioneFascicolo, "'", "''") & "' " & _
                 " where idattività=" & IdAttività & ""
        CmdGenerico = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
    End Sub

    Private Sub UpdateElencoDocumenti(ByVal IdEntità As Integer)

        Dim strSql As String
        Dim Documento As String

        If Request.QueryString("TipoStampa") <> Nothing Then
            Documento = ReturnNomeDocumento(IdEntità)
        Else
            Documento = Request.QueryString("TipoDocumento")
        End If

        strSql = "Update CronologiaEntitàDocumenti set DataProt = '" & DataProt & "', " & _
                " NProt = '" & NumProt & "' " & _
                " where identità = " & IdEntità & " " & _
                " and documento = '" & Documento & "'"


        Dim cmdUpCron As New SqlClient.SqlCommand(strSql, Session("conn"))
        cmdUpCron.ExecuteNonQuery()
    End Sub
    Private Sub UpdateElencoEntiDocumenti()
        Dim strSql As String
        Dim IdAttivitàSedeAssegnazione As Integer
        IdAttivitàSedeAssegnazione = TrovaIdAttivitàSedeAssegnazione(Request.QueryString("VarVolontario"))

        strSql = "Update CronologiaEntiDocumenti set DataProt = '" & DataProt & "', " & _
                " NProt = '" & NumProt & "' " & _
                " where IdEnte = " & Session("IdEnte") & _
                " and documento = '" & Request.QueryString("TipoDocumento") & "'" & _
                " and IdAttivitàSedeAssegnazione = " & IdAttivitàSedeAssegnazione

        Dim cmdUpCron As New SqlClient.SqlCommand(strSql, Session("conn"))
        cmdUpCron.ExecuteNonQuery()
        If Request.QueryString("TipoDocumento") = "elencovolontariammessi" Then
            UpdateVolontariGraduatoria(IdAttivitàSedeAssegnazione)
        End If
    End Sub
    Private Sub UpdateElencoDocumentiPerLetteraSubentro()

        Dim strSql As String
        Dim Documento As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim IdAttivitàSedeAssegnazione As Integer

        If Session("StatoVolontario") = "In Servizio" Then

            IdAttivitàSedeAssegnazione = TrovaIdAttivitàSedeAssegnazione(Request.QueryString("VarVolontario"))

            Documento = ReturnNomeDocumentoLetteraSubentro()
            strSql = "Update CronologiaEntiDocumenti set DataProt = '" & DataProt & "', " & _
                     " NProt = '" & NumProt & "' " & _
                     " where IdEnte = " & Session("IdEnte") & _
                     " and documento = '" & Documento & "'" & _
                     " and IdAttivitàSedeAssegnazione = " & IdAttivitàSedeAssegnazione
            '                     " and username ='" & Session("Utente") & "' " & _
        Else
            Documento = ReturnNomeDocumentoLetteraSubentro()
            strSql = "Update CronologiaEntitàDocumenti set DataProt = '" & DataProt & "', " & _
                     " NProt = '" & NumProt & "' " & _
                     " where identità = " & Request.QueryString("VarVolontario") & " " & _
                     " and documento = '" & Documento & "'"
            '                     " and username ='" & Session("Utente") & "'"
        End If

        Dim cmdUpCron As New SqlClient.SqlCommand(strSql, Session("conn"))
        cmdUpCron.ExecuteNonQuery()
    End Sub

    Sub UpdateVolontariGraduatoria(ByVal IdSedeAssegnazione As Integer)
        'creato da sc il 28/01/2008
        Dim strsql As String

        strsql = " UPDATE CronologiaEntitàDocumenti SET "
        strsql = strsql & "	DataProt = '" & DataProt & "', "
        strsql = strsql & " NProt = '" & NumProt & "' "
        strsql = strsql & "from entità "
        strsql = strsql & "INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità  "
        strsql = strsql & "INNER JOIN CronologiaEntitàDocumenti ON entità.IDEntità = CronologiaEntitàDocumenti.IDEntità   "
        strsql = strsql & "INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione  "
        strsql = strsql & "inner join statientità on statientità.idstatoentità=entità.idstatoentità "
        strsql = strsql & "left join impVolontariLotus on entità.codicefiscale=impVolontariLotus.cf "
        strsql = strsql & "inner join graduatorieEntità on graduatorieEntità.identità=Entità.identità "
        strsql = strsql & "left join TipologiePosto on graduatorieEntità.idtipologiaposto=TipologiePosto.idtipologiaposto "
        strsql = strsql & "inner join comuni on comuni.idcomune=entità.idcomunenascita "
        strsql = strsql & "inner join provincie on  provincie.idprovincia=comuni.idprovincia  "
        strsql = strsql & "where graduatorieEntità.idattivitàsedeassegnazione='" & IdSedeAssegnazione & "' "
        strsql = strsql & "and graduatorieEntità.ammesso = 1 and statientità.inservizio=1 and attivitàentità.idstatoattivitàentità=1 "
        strsql = strsql & "AND CronologiaEntitàDocumenti.Documento='Graduatoria Volontari'"

        Dim cmdUpCron As New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdUpCron.ExecuteNonQuery()
    End Sub

    Public Function FileToBase64(ByVal fileName As String) As String
        Dim bFile() As Byte
        Dim fs As FileStream
        Dim _textB64 As String
        Try
            fs = New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            ReDim bFile(fs.Length - 1)
            fs.Read(bFile, 0, fs.Length)
            _textB64 = Convert.ToBase64String(bFile)
        Catch ex As Exception
            'gestione eccezione 
        Finally
            fs.Close()
        End Try
        Return _textB64
    End Function

    Private Function NormalizzaTitolario(ByVal strTitolario) As String
        Dim strAppoTitolario As String
        strAppoTitolario = Mid(strTitolario, 1, InStr(strTitolario, "-") - 1)
        strAppoTitolario = Replace(strAppoTitolario, " ", "")
        Return strAppoTitolario
    End Function

    Private Function ReturnNomeDocumento(ByVal IdEntita As Integer) As String
        'creata da s.c
        'FUNZIONE CHE RITORNA IL NOME DEL DOCUMENTO DI STAMPA 
        'PER LE LETTERE DI ASSEGNAZIONE DEL VOLONTARIO 
        Dim strsql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim NomeDocumentoStampa As String
        Dim NomeDocumentoStampaB As String

        strsql = "select a.idtipoprogetto as naz, " & _
                 " isnull(identitàSubentrante,0) as  subentro, " & _
                 " e.datainizioservizio,a.datainizioattività " & _
                 " from Attività a  " & _
                 " inner join attivitàsediassegnazione asa on asa.idattività=a.idattività  " & _
                 " inner join graduatorieentità ge on ge.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione  " & _
                 " inner join entità e on e.idEntità=ge.idEntità  " & _
                 " left join cronologiasostituzioni cs on cs.identitàsubentrante=ge.idEntità " & _
                 " where ge.idEntità=" & IdEntita & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        'Verifica se Volontario Estero o nazionale
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            If dtrgenerico("naz") = 1 Or dtrgenerico("naz") = 3 Then
                If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'verifico se vengo dalla maschera stampa documentazione della gestione sostituzione o da quella del volontario
                    NomeDocumentoStampa = "Assegnazione Volontario - Nazionale"
                    NomeDocumentoStampaB = "AssegnazioneVolontariNazionaliB"
                Else
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'verifico se vengo dalla maschera stampa documentazione della gestione sostituzione o da quella del volontario
                    NomeDocumentoStampa = "Sostituzione Volontario - Nazionale"
                    NomeDocumentoStampaB = "SostituzioneVolontariNazionaliB"
                End If
            Else
                If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'verifico se vengo dalla maschera stampa documentazione della gestione sostituzione o da quella del volontario
                    NomeDocumentoStampa = "Assegnazione Volontario - Estero"
                    NomeDocumentoStampaB = "AssegnazioneVolontariEsteroB"

                Else
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'verifico se vengo dalla maschera stampa documentazione della gestione sostituzione o da quella del volontario
                    NomeDocumentoStampa = "Sostituzione Volontario - Estero"
                    NomeDocumentoStampaB = "SostituzioneVolontariEsteriB"
                End If
            End If
        End If
        If Request.QueryString("TipoStampa") = "AssVol" Then
            ReturnNomeDocumento = NomeDocumentoStampa
        Else
            ReturnNomeDocumento = NomeDocumentoStampaB
        End If

    End Function
    Private Function ReturnNomeDocumentoLetteraSubentro() As String
        'creata da s.c 22/01/2009
        'FUNZIONE CHE RITORNA IL NOME DEL DOCUMENTO DI STAMPA PER LE LETTERE DI SUBENTRO GEENRATE NELLA MASCHERA 
        'SE IL VOLONTARIO E' IN SERVIZIO LETTERE RINUNCIA MULTIPLA
        'ALTRIMENTI  LETTERE RINUNCIA 

        'Dim strsql As String
        'Dim dtrgenerico As SqlClient.SqlDataReader
        Dim NomeDocumentoStampa As String
        Dim NomeDocumentoStampaB As String

        If Session("StatoVolontario") = "In Servizio" Then
            NomeDocumentoStampa = "rinunciaserviziovolontariomultipla"
            NomeDocumentoStampaB = "rinunciaserviziovolontariomultiplaCopiaReg"
        Else
            NomeDocumentoStampa = "Rinuncia Servizio Volontario"
            NomeDocumentoStampaB = "rinunciaserviziovolontarioCopiaReg"
        End If

        If Request.QueryString("TipoStampa") = "LettSub" Then
            ReturnNomeDocumentoLetteraSubentro = NomeDocumentoStampa
        Else
            ReturnNomeDocumentoLetteraSubentro = NomeDocumentoStampaB
        End If
    End Function

    Function TrovaIdAttivitàSedeAssegnazione(ByVal IdEntità As Integer)
        'trovo IdAttivitàSedeAssegnazione del volontario
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        strSql = "SELECT GraduatorieEntità.IdAttivitàSedeAssegnazione " & _
        " FROM GraduatorieEntità " & _
        " WHERE identità=" & IdEntità & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            TrovaIdAttivitàSedeAssegnazione = dtrgenerico("IdAttivitàSedeAssegnazione")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return TrovaIdAttivitàSedeAssegnazione
    End Function

    Sub CaricaTitolario(ByVal strProcesso As String)
        'creata da simona cordella il 13/12/2010
        'i titolari vengono caricati della vista VW_Titolario presente in SQL
        Dim strSQL As String
        Dim dtrGenerico As SqlClient.SqlDataReader

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strSQL = " SELECT Codice,DESCRIZIONE,LIV1,LIV2,LIV3,LIV4 FROM VW_TITOLARIO " & _
                 " Where processo ='" & strProcesso & "' and Anno = (Select max(anno) from VW_TITOLARIO) " & _
                 " UNION " & _
                 " Select '0' ,'','','','','' From VW_TITOLARIO  " & _
                 " Where processo ='PROGETTI' and Anno = (Select max(anno) from VW_TITOLARIO)" & _
                 " order by LIV1,LIV2,LIV3,LIV4 "
        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("Conn"))

        cboTitolario.DataTextField = "Descrizione"
        cboTitolario.DataValueField = "Codice"
        cboTitolario.DataSource = dtrGenerico
        cboTitolario.DataBind()

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

    End Sub

    Private Sub cboAnno_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CaricaTitolario(Request.QueryString("Processo"))
    End Sub

    Function FileCopy(ByVal PathLocale As String, ByVal NomeFile As String) As String
        'CREATA DA SIMONA CORDELLA 
        'copia il file dalla cartella di origine alla cartella del server
        'il nome del server e la cartella vengono letti dal web.config
        Dim PathServerSiged As String

        PathServerSiged = "\\" & ConfigurationSettings.AppSettings("SERVER_SIGED") & "\" & ConfigurationSettings.AppSettings("CARTELLA_SIGED") & "\" & NomeFile
        If File.Exists(PathServerSiged) = True Then
            File.Delete(PathServerSiged)
        End If
        File.Copy(PathLocale, PathServerSiged)
        Return PathServerSiged
    End Function

    Function ValidaCampi() As Boolean
        Dim campiValidi As Boolean = True
        Dim errore As StringBuilder = New StringBuilder()

        If (txtSelFile.PostedFile.FileName = "") Then
            errore.Append("Selezionare il file da inviare.<br/>")
            campiValidi = False
        End If
        If (txtCodFasc.Value = "" And txtDescFascicolo.Text = "") Then
            errore.Append("Indicare la descrizione del fascicolo.<br/>")
            campiValidi = False
        End If
        If (txtOggetto.Text = "") Then
            errore.Append("Indicare la descrizione dell'oggetto.<br/>")
            campiValidi = False
        End If
        If (campiValidi = False) Then
            lblmessaggio.Text = errore.ToString()

        End If


        Return campiValidi
    End Function


End Class
