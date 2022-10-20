Public Class WfrmSIGEDElencoDocumenti
    Inherits System.Web.UI.Page

    Shared strNome As String
    Shared strCognome As String
    Dim dt As New DataTable
    Dim pNumeroProtocollo As String
    Dim pDataProtocollo As String
    Dim pNomeFile As String
    Dim SIGED As clsSiged

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblmessaggio.Text = ""
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        Try
            If Page.IsPostBack = False Then
                CaricaGliglia()
            End If
        Catch ex As Exception
            lblmessaggio.Text = "Errore imprevisto: " & ex.Message
            PulsantiProtocollo(False)
        End Try
    End Sub

    Private Sub cmdIndietro_Click(sender As Object, e As EventArgs) Handles cmdIndietro.Click
        Response.Write("<script>" & vbCrLf)
        If Request.QueryString("VengoDa") = "Stampa" Then
            Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objHddModifica") & """).value=1;")
        End If
        Response.Write("self.close()")
        Response.Write("</script>")

    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Dim pIdentificativoDOC As String = String.Empty
        If e.Item.ItemIndex <> -1 Then
            pIdentificativoDOC = "" & e.Item.Cells(1).Text.Replace("&nbsp;", "")
            'controllo aggiunto il 05/10/2011 se selezione un fasciolo consento la navigazione al suo interno
            If e.Item.Cells(5).Text.Replace("&nbsp;", "") = "Fascicolo" Then
                'SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
                Navigazione(pIdentificativoDOC)
            Else
                pNumeroProtocollo = "" & e.Item.Cells(4).Text.Replace("&nbsp;", "")
                pDataProtocollo = "" & e.Item.Cells(2).Text.Replace("&nbsp;", "")
                'modificato il formato della data 11/01/2013
                If pDataProtocollo <> "" Then
                    pDataProtocollo = FormatDateTime(pDataProtocollo, DateFormat.ShortDate)
                End If
                If Request.QueryString("objForm") Is Nothing Then

                    If pDataProtocollo = "" Then  'pNumeroProtocollo      'APRO DIRETTAMENTE IL FILE
                        pIdentificativoDOC = Replace(e.Item.Cells(1).Text, "#", "%23")
                        pNomeFile = e.Item.Cells(3).Text
                        'adc
                        pNomeFile = Replace(Replace(Replace(Replace(e.Item.Cells(3).Text, "&", ""), "#", ""), "’", "'"), "+", " ")
                        Response.Redirect("WfrmSIGEDDocumento.aspx?TipoDocumento=" & "" & e.Item.Cells(5).Text.Replace("&nbsp;", "") & "&NomeFile=" & pNomeFile & "&NumeroProtocollo=" & pNumeroProtocollo & "&DataProtocollo=" & pDataProtocollo & "&CodiceFascicolo=" & Request.QueryString("NumeroFascicolo") & "&IdentificativoDOC=" & pIdentificativoDOC)
                        'Response.Redirect("WfrmSIGEDDocumento.aspx?NumeroProtocollo=" & pNumeroProtocollo & "&DataProtocollo=" & pDataProtocollo & "&CodiceFascicolo=" & Request.QueryString("NumeroFascicolo") & "&IdentificativoDOC=" & pIdentificativoDOC)
                    Else                                    'PRPRONGO ELENCO FILE ALLEGATI
                        Response.Redirect("WfrmSIGEDElencoAllegati.aspx?NumeroProtocollo=" & pNumeroProtocollo & "&DataProtocollo=" & pDataProtocollo & "&CodiceFascicolo=" & Request.QueryString("NumeroFascicolo"))
                    End If

                Else 'SELEZIONE PROTOCOLLO
                    If Request.QueryString("objDoc") Is Nothing Then
                        If pNumeroProtocollo = "" Then
                            pDataProtocollo = ""
                        End If
                        Response.Write("<script type=""text/javascript"">" & vbCrLf)
                        If e.Item.Cells(5).Text.Replace("&nbsp;", "") = "Documento Interno" Then
                            pNomeFile = e.Item.Cells(3).Text
                            'adc
                            pNomeFile = Replace(Replace(Replace(Replace(e.Item.Cells(3).Text, "&", ""), "#", ""), "’", "'"), "+", " ")
                            pIdentificativoDOC = Replace(e.Item.Cells(1).Text, "#", "%23")
                            Response.Write("window.opener.document.getElementById(""" & Request.QueryString("TxtNumDoc") & """).value='" & pNumeroProtocollo & "';")
                            Response.Write("window.opener.document.getElementById(""" & Request.QueryString("txtNomeFile") & """).value='" & pNomeFile & "';")
                            Response.Write("window.opener.document.getElementById(""" & Request.QueryString("txtIDDocInterno") & """).value='" & pIdentificativoDOC & "';")
                        Else
                            Response.Write("window.opener.document.getElementById(""" & Request.QueryString("TxtProt") & """).value='" & pNumeroProtocollo & "';")
                            Response.Write("window.opener.document.getElementById(""" & Request.QueryString("TxtData") & """).value='" & pDataProtocollo & "';")
                        End If

                        'controllo aggiunto il 22/06/2011 da sc verifica se riporto i dati relativi alla sanzione
                        If (Request.QueryString("TxtData") = "txtDataProtocolloEsecuzioneSanzione" Or Request.QueryString("TxtProt") = "TxtNumeroProtocolloEsecuzioneSanzione") Then
                            'Response.Write("window.opener.document.getElementById('cmdChiusaContestata').style.visibility='hidden';") Then
                            'rendo invisibile pulsante  CHIUDI CONTESTAZIONE 
                            If Request.QueryString("ChiudiContestata") = 1 Then
                                Response.Write("window.opener.document.getElementById('cmdChiusaContestata').style.visibility='hidden';")
                            End If

                            'abilito pulsante CMDSANZIONE
                            Response.Write("window.opener.document.getElementById('cmdSanzione').disabled = false;")
                        End If 'document.getElementById(NumProt).disabled=true

                        If Request.QueryString("VArUpdate") = 1 Then
                            TrovaFascicolo(Session("Utente"), Request.QueryString("NumeroFascicolo"), Request.QueryString("VarVolontario"))
                            If Request.QueryString("TipoStampa") = "LettSub" Or Request.QueryString("TipoStampa") = "LettSubB" Then
                                'stampo la lettera soubentro solo nella maschera WfrmElencoDocumentazioneVolontario
                                UpdateElencoDocumentiPerLetteraSubentro()
                            Else
                                Call UpdateElencoDocumenti(Request.QueryString("VarVolontario"))
                            End If

                        ElseIf Request.QueryString("VArUpdate") = 3 Then 'agg. da sc per salvataggio in cronologiaentidocumenti
                            Call UpdateElencoEntiDocumenti()
                        End If
                    Else
                        If pIdentificativoDOC = "" Then
                            pDataProtocollo = ""
                        End If
                        Response.Write("<script type=""text/javascript"">" & vbCrLf)
                        Response.Write("window.opener.document.getElementById(""" & Request.QueryString("TxtDoc") & """).value='" & pIdentificativoDOC & "';")
                        Response.Write("window.opener.document.getElementById(""" & Request.QueryString("TxtData") & """).value='" & pDataProtocollo & "';")
                        If Request.QueryString("VArUpdate") = 1 Then
                            If Request.QueryString("TipoStampa") = "LettSub" Or Request.QueryString("TipoStampa") = "LettSubB" Then
                                'stampo la lettera soubentro solo nella maschera WfrmElencoDocumentazioneVolontario
                                UpdateElencoDocumentiPerLetteraSubentro()
                            Else
                                Call UpdateElencoDocumenti(Request.QueryString("VarVolontario"))
                            End If
                        ElseIf Request.QueryString("VArUpdate") = 3 Then 'agg. da sc per salvataggio in cronologiaentidocumenti
                            Call UpdateElencoEntiDocumenti()
                        End If
                    End If
                    If Request.QueryString("VengoDa") = "Stampa" Then
                        Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objHddModifica") & """).value=1;")
                    End If


                    Response.Write("self.close()")
                    Response.Write("</script>")
                End If
            End If
        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("dtRisulatato")
        dgRisultatoRicerca.DataBind()
    End Sub
    Private Sub UpdateElencoDocumenti(ByVal IdEntità As Integer)

        Dim strSql As String
        Dim Documento As String
        If pNumeroProtocollo <> "" Then
            If Request.QueryString("TipoStampa") <> Nothing Then
                Documento = ReturnNomeDocumento(IdEntità)
            Else
                Documento = Request.QueryString("TipoDocumento")
            End If

            strSql = "Update CronologiaEntitàDocumenti set DataProt = '" & pDataProtocollo & "', " & _
                    " NProt = '" & pNumeroProtocollo & "' " & _
                    " where identità = " & IdEntità & _
                    " and documento = '" & Documento & "'"
            '" and username ='" & Session("Utente") & "'"

            Dim cmdUpCron As New SqlClient.SqlCommand(strSql, Session("conn"))
            cmdUpCron.ExecuteNonQuery()
        End If
    End Sub
    Private Sub UpdateElencoEntiDocumenti()
        Dim strSql As String
        Dim IdAttivitàSedeAssegnazione As Integer
        If pNumeroProtocollo <> "" Then
            IdAttivitàSedeAssegnazione = TrovaIdAttivitàSedeAssegnazione(Request.QueryString("VarVolontario"))

            strSql = "Update CronologiaEntiDocumenti set DataProt = '" & pDataProtocollo & "', " & _
                    " NProt = '" & pNumeroProtocollo & "' " & _
                    " where IdEnte = " & Session("IdEnte") & _
                    " and documento = '" & Request.QueryString("TipoDocumento") & "'" & _
                    " and IdAttivitàSedeAssegnazione = " & IdAttivitàSedeAssegnazione
            '" and username ='" & Session("Utente") & "' " & _
            Dim cmdUpCron As New SqlClient.SqlCommand(strSql, Session("conn"))
            cmdUpCron.ExecuteNonQuery()
            If Request.QueryString("TipoDocumento") = "elencovolontariammessi" Then
                UpdateVolontariGraduatoria(IdAttivitàSedeAssegnazione)
            End If
        End If
    End Sub
    Sub UpdateVolontariGraduatoria(ByVal IdSedeAssegnazione As Integer)
        'creato da sc il 28/01/2009
        Dim strsql As String

        strsql = " UPDATE CronologiaEntitàDocumenti SET "
        strsql = strsql & "	DataProt = '" & pDataProtocollo & "', "
        strsql = strsql & " NProt = '" & pNumeroProtocollo & "' "
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

    Private Function TrovaFascicolo(ByVal strUserName As String, ByVal IdFascicolo As String, ByVal Valore As String) As String
        'Dim wsIN As New WS_Verifiche.VERIFICHEs
        'Dim wsOUT As New WS_Verifiche.RICERCA_FASCICOLO_RISPOSTA
        'Dim wsELENCO() As WS_Verifiche.FASCICOLO_TROVATO
        Dim strSQL As String
        Dim dsUser As DataSet

        Dim wsOUT As New WS_SIGeD.INDICE_FASCICOLO
        Dim wsELENCO As WS_SIGeD.FASCICOLO_DOCUMENTO

        Dim sEsito As String

        Dim DataSetRicerca As DataSet
        Dim strDescrizioneFascicolo As String
        Dim strNomeUtente As String
        Dim strCognomeUtente As String
        Dim riga As Integer
        Dim strNumeroFascicolo As String
        Dim DescrizioneFascicolo As String
        Dim CmdGenerico As SqlClient.SqlCommand
        Dim CodFascicolo As String
        Try

            'modificato il 30/08/2011
            SIGED = New clsSiged("", strNome, strCognome)

            CodFascicolo = SIGED.SIGED_IdFascicoloCompleto(IdFascicolo)

            wsOUT = SIGED.SIGED_IndiceFascicoli(CodFascicolo)
            sEsito = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)

            If sEsito = 0 Then
                If Right(wsOUT.ESITO, 1) = "0" Then

                Else
                    strNumeroFascicolo = wsELENCO.NUMERO
                    DescrizioneFascicolo = wsELENCO.DESCRIZIONE
                    If Request.QueryString("VArUpdate") = 1 Then
                        Call UpdateFascicoloEntità(strNumeroFascicolo, IdFascicolo, DescrizioneFascicolo, Valore)
                    ElseIf Request.QueryString("VArUpdate") = 3 Then 'agg. da sc per salvataggio in cronologiaentidocumenti
                        Call UpdateFascicoloProgetto(strNumeroFascicolo, IdFascicolo, DescrizioneFascicolo, Valore)
                    End If

                End If

            End If





            ''***Destinatario per tutte le richieste di protocollazione = dall'incarico
            'strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & strUserName & "'"

            'DataSetRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))

            'strNomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Nome")
            'strCognomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Cognome")

            'DataSetRicerca.Clear()

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

        Catch ex As Exception
            ' TrovaFascicolo = ex.Message
            'DataSetRicerca.Dispose()
        End Try
    End Function
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
    Private Function ReturnNomeDocumento(ByVal IdEntita As Integer) As String
        'creata da s.c
        'FUNZIONE CHE RITORNA IL NOME DEL DOCUMENTO DI STAMPA PER LE LETTERE DI ASSEGNAZIONE DEL VOLONTARIO 
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
                    'If Request.QueryString("VengoDa") = "SostVolontario" Then 'vengo dall maschera WfrmElencoDocumentazioneSostituzioneVolontario
                    NomeDocumentoStampa = "Assegnazione Volontario - Nazionale"
                    NomeDocumentoStampaB = "AssegnazioneVolontariNazionaliB"
                    'Else 'vengo dall maschera WfrmElencoDocumentazioneVolontario
                    '    NomeDocumentoStampa = "Lettera Assegnazione Volontario - Nazionale"
                    '    NomeDocumentoStampaB = "Lettera Assegnazione Volontario - NazionaleB"
                    'End If
                Else
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'verifico se vengo dalla maschera stampa documentazione della gestione sostituzione o da quella del volontario
                    'If Request.QueryString("VengoDa") = "SostVolontario" Then 'vengo dall maschera WfrmElencoDocumentazioneSostituzioneVolontario
                    NomeDocumentoStampa = "Sostituzione Volontario - Nazionale"
                    NomeDocumentoStampaB = "SostituzioneVolontariNazionaliB"
                    'Else 'vengo dall maschera WfrmElencoDocumentazioneVolontario
                    '     NomeDocumentoStampa = "Lettera Sostituzione Volontario - Nazionale"
                    '    NomeDocumentoStampaB = "Lettera Sostituzione Volontario - NazionaleB"
                    'End If
                End If
            Else
                If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'verifico se vengo dalla maschera stampa documentazione della gestione sostituzione o da quella del volontario
                    'If Request.QueryString("VengoDa") = "SostVolontario" Then 'vengo dall maschera WfrmElencoDocumentazioneSostituzioneVolontario
                    NomeDocumentoStampa = "Assegnazione Volontario - Estero"
                    NomeDocumentoStampaB = "AssegnazioneVolontariEsteroB"
                    'Else 'vengo dall maschera WfrmElencoDocumentazioneVolontario
                    '    NomeDocumentoStampa = "Lettera Assegnazione Volontario - Estero"
                    '    NomeDocumentoStampaB = "Lettera Assegnazione Volontario - EsteroB"
                    'End If
                Else
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'verifico se vengo dalla maschera stampa documentazione della gestione sostituzione o da quella del volontario
                    'If Request.QueryString("VengoDa") = "SostVolontario" Then 'vengo dall maschera WfrmElencoDocumentazioneSostituzioneVolontario
                    NomeDocumentoStampa = "Sostituzione Volontario - Estero"
                    NomeDocumentoStampaB = "SostituzioneVolontariEsteriB"
                    'Else 'vengo dall maschera WfrmElencoDocumentazioneVolontario
                    '    NomeDocumentoStampa = "Lettera Sostituzione Volontario - Estero"
                    '    NomeDocumentoStampaB = "Lettera Sostituzione Volontario - EsteroB"
                    'End If
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

    Private Sub UpdateElencoDocumentiPerLetteraSubentro()

        Dim strSql As String
        Dim Documento As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim IdAttivitàSedeAssegnazione As Integer
        If Session("StatoVolontario") = "In Servizio" Then

            IdAttivitàSedeAssegnazione = TrovaIdAttivitàSedeAssegnazione(Request.QueryString("VarVolontario"))

            Documento = ReturnNomeDocumentoLetteraSubentro()
            strSql = "Update CronologiaEntiDocumenti set DataProt = '" & pDataProtocollo & "', " & _
                    " NProt = '" & pNumeroProtocollo & "' " & _
                    " where IdEnte = " & Session("IdEnte") & _
                    " and documento = '" & Documento & "'" & _
                    " and IdAttivitàSedeAssegnazione = " & IdAttivitàSedeAssegnazione
            '                    " and username ='" & Session("Utente") & "' " & _
        Else
            Documento = ReturnNomeDocumentoLetteraSubentro()
            strSql = "Update CronologiaEntitàDocumenti set DataProt = '" & pDataProtocollo & "', " & _
                    " NProt = '" & pNumeroProtocollo & "' " & _
                    " where identità = " & Request.QueryString("VarVolontario") & _
                    " and documento = '" & Documento & "'"
            '" and username ='" & Session("Utente") & "'"

        End If

        Dim cmdUpCron As New SqlClient.SqlCommand(strSql, Session("conn"))
        cmdUpCron.ExecuteNonQuery()
    End Sub

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

    Private Sub cmdAggiungi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAggiungi.Click
        AggiungiProtocolloEsistente(TxtNumProtocollo.Text, TxtAnno.Text)
    End Sub

    Private Sub CaricaGliglia()

        Dim wsOUT As New WS_SIGeD.INDICE_FASCICOLO
        Dim wsELENCO() As WS_SIGeD.COLLEGAMENTO_DOCUMENTO_TROVATO

        Dim sEsito As String
        Dim sNumeroDoc As String
        Dim dr As DataRow
        Dim riga As Integer

        Dim strNome As String
        Dim strCognome As String
        Dim strSQL As String
        Dim dsUser As DataSet

        Dim TxtNumFasc As String

        strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & Session("Utente") & "'"

        dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

        If dsUser.Tables(0).Rows.Count <> 0 Then
            strNome = dsUser.Tables(0).Rows(0).Item("Nome")
            strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
        End If
        SIGED = New clsSiged("", strNome, strCognome)
        If SIGED.Codice_Esito <> 0 Then
            lblmessaggio.Text = SIGED.Esito
            Exit Sub
        End If

        TxtNumFasc = Request.QueryString("NumeroFascicolo") 'idfascicolo
        'TxtNumFasc = SIGED.SIGED_IdFascicoloCompleto(TxtNumFasc)

        wsOUT = SIGED.SIGED_IndiceFascicoli(TxtNumFasc)

        sEsito = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)

        If sEsito = 0 Then
            'sNumeroDoc = wsOUT.NUMERO_DOCUMENTI
            wsELENCO = wsOUT.ELENCO_DOCUMENTI
            'lblFascicolo.Text = "Fascicolo:" & " "
            '& wsOUT.DESCRIZIONE_FASCICOLO()

            'wsELENCO = wsOUT.ELENCO_DOCUMENTI
            If Not wsOUT.ELENCO_DOCUMENTI Is Nothing Then
                dt.Columns.Add(New DataColumn("Codice Documento", GetType(String)))
                dt.Columns.Add(New DataColumn("Data Protocollo", GetType(String)))
                dt.Columns.Add(New DataColumn("Descrizione", GetType(String)))
                dt.Columns.Add(New DataColumn("Numero", GetType(String)))
                dt.Columns.Add(New DataColumn("Tipo Documento", GetType(String)))
                'filtrare solo per i fascicolo "2011#PROT#1018/2011" leggere se è   FASC

                For riga = LBound(wsELENCO) To UBound(wsELENCO)

                    SIGED.NormalizzaCodice(wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO)
                    Select Case UCase(Request.QueryString("Classificazione"))
                        Case "TUTTI" 'visualizzo tutti i documenti prenseti nel fascicolo
                            dr = dt.NewRow
                            Select Case UCase(wsELENCO(riga).TIPOCOLLEGAMENTO)
                                Case "FASCICOLO"
                                    dr(0) = wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO
                                    dr(1) = ""
                                    dr(2) = wsELENCO(riga).DESCRIZIONECOLLEGAMENTO
                                    dr(3) = Replace(wsELENCO(riga).DETTAGLICOLLEGAMENTO, " ", "") 'numero fascicolo
                                    dr(4) = wsELENCO(riga).TIPOCOLLEGAMENTO
                                Case "PROTOCOLLO USCITA", "PROTOCOLLO ENTRATA", "PROTOCOLLO INTERNO"
                                    dr(0) = wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO
                                    dr(1) = wsELENCO(riga).DETTAGLICOLLEGAMENTO 'data del protocollo
                                    dr(2) = wsELENCO(riga).DESCRIZIONECOLLEGAMENTO
                                    dr(3) = SIGED.CodiceNormalizzato
                                    dr(4) = wsELENCO(riga).TIPOCOLLEGAMENTO 'tipolocia di documento (fascicolo,protocollo e documento interno)
                                Case Else
                                    dr = dt.NewRow
                                    dr(0) = wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO
                                    dr(1) = ""
                                    dr(2) = wsELENCO(riga).DESCRIZIONECOLLEGAMENTO
                                    dr(3) = SIGED.SIGED_NumeroAllegato(wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO)
                                    dr(4) = wsELENCO(riga).TIPOCOLLEGAMENTO 'tipolocia di documento (fascicolo,protocollo e documento interno)
                            End Select
                            dt.Rows.Add(dr)
                        Case "DOCUMENTI" 'visualizzo solo i documenti interni
                            Select Case UCase(wsELENCO(riga).TIPOCOLLEGAMENTO)
                                Case "DOCUMENTO INTERNO"
                                    dr = dt.NewRow
                                    dr(0) = wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO
                                    dr(1) = ""
                                    dr(2) = wsELENCO(riga).DESCRIZIONECOLLEGAMENTO
                                    dr(3) = SIGED.SIGED_NumeroAllegato(wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO)
                                    dr(4) = wsELENCO(riga).TIPOCOLLEGAMENTO 'tipolocia di documento (fascicolo,protocollo e documento interno)
                                    dt.Rows.Add(dr)
                            End Select
                        Case Else 'visualizzo tutti i protocolli
                            Select Case UCase(wsELENCO(riga).TIPOCOLLEGAMENTO)
                                Case "PROTOCOLLO USCITA", "PROTOCOLLO ENTRATA", "PROTOCOLLO INTERNO"
                                    dr = dt.NewRow
                                    dr(0) = wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO
                                    dr(1) = wsELENCO(riga).DETTAGLICOLLEGAMENTO 'data del protocollo
                                    dr(2) = wsELENCO(riga).DESCRIZIONECOLLEGAMENTO
                                    dr(3) = SIGED.CodiceNormalizzato
                                    dr(4) = wsELENCO(riga).TIPOCOLLEGAMENTO 'tipolocia di documento (fascicolo,protocollo e documento interno)
                                    dt.Rows.Add(dr)
                            End Select
                    End Select
                Next

                Dim dv As New DataView
                dv = dt.DefaultView
                dv.Sort = "Data Protocollo DESC , Numero DESC"

                dgRisultatoRicerca.DataSource = dv
                dgRisultatoRicerca.DataBind()
                Session("dtRisulatato") = dt
            End If
            '**** agg da simona cordella il 10/02/2011 
            'abilito visibilità text protocolli
            If UCase(Request.QueryString("Classificazione")) = "DOCUMENTI" Then
                PulsantiProtocollo(False)
            Else
                PulsantiProtocollo(True)
            End If

            ' *****
        Else
            lblmessaggio.Text = SIGED.SIGED_Esito(wsOUT.ESITO)
            '**** agg da simona cordella il 10/02/2011 
            'disabilito visibilità text protocolli
            If sEsito = 1016 Then 'intercetto errore "Fascicolo vuoto" visualizzo pulsanti associazione protocollo
                PulsantiProtocollo(True)
            Else
                PulsantiProtocollo(False)
            End If
            ' ****
        End If
        SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
    End Sub

    Private Sub AggiungiProtocolloEsistente(ByVal IdProtocollo As String, ByVal Anno As String)
        '*** creata da simona cordella il 10/02/2011
        '*** consento di inserire un protocollo esistente al fascicolo
        'Dim wsIN As New WS_Verifiche.VERIFICHEs
        'Dim wsOUT As New WS_Verifiche.FASCICOLAZIONE_PROTOCOLLO_RISPOSTA

        'Dim wsIN As New WS_Verifiche.VERIFICHEs
        'Dim wsOUT As New WS_SIGeD.R
        Dim wsOUT As String

        Dim strIdFasc As String 'idfascicolo
        Dim IdFascSIGED As String 'idfascicolo
        Dim IdProtocolloSIGED As String
        Dim sEsito As String
        Dim sNumeroDoc As String
        Dim dr As DataRow
        Dim riga As Integer

        Dim strNome As String
        Dim strCognome As String
        Dim strSQL As String
        Dim dsUser As DataSet
        lblmessaggio.Text = ""

        strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & Session("Utente") & "'"
        dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

        If dsUser.Tables(0).Rows.Count <> 0 Then
            strNome = dsUser.Tables(0).Rows(0).Item("Nome")
            strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
        End If

        SIGED = New clsSiged("", strNome, strCognome)
        If SIGED.Codice_Esito <> 0 Then
            lblmessaggio.Text = SIGED.Esito
            Exit Sub
        End If
        IdProtocolloSIGED = SIGED.SIGED_CodiceProtocolloCompleto(Anno, IdProtocollo)

        strIdFasc = Request.QueryString("NumeroFascicolo") 'idfascicolo
        IdFascSIGED = SIGED.SIGED_IdFascicoloCompleto(strIdFasc)

        wsOUT = SIGED.SIGED_CreaCollegamentoFascicolo(IdFascSIGED, IdProtocolloSIGED)


        If SIGED.SIGED_Codice_Esito(wsOUT) = 0 Then
            LblOperazioneOk.Text = "Aggiornamento eseguito."
            CaricaGliglia()
        Else
            'modificato il 11/01/2013 da simona cordella
            'verifico se il protocollo è già presente nel fascicolo
            If InStr(SIGED.SIGED_Esito(wsOUT), "PRIMARY KEY") <> 0 Then
                lblmessaggio.Text = "Protocollo già esistente."
            Else
                lblmessaggio.Text = Mid(wsOUT, 8, Len(wsOUT))
            End If
        End If
        SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
    End Sub

    Sub PulsantiProtocollo(ByVal Valore As Boolean)
        LblNumProtocollo.Visible = Valore
        TxtNumProtocollo.Visible = Valore
        LblAnno.Visible = Valore
        TxtAnno.Visible = Valore
        cmdAggiungi.Visible = Valore
        If Valore = True Then
            TxtAnno.Text = Year(Now())
        End If
    End Sub

    Sub Navigazione(ByVal CodFascicolo As String)
        'AGGIUNTA IL 05/10/2011
        'la funzione viene richiamata dal pulsante Navigazione della griglia che consente di visalizzare i fascicoli e i protocolli associati ad un fascicolo padre
        Try
            lblmessaggio.Text = ""
            dgRisultatoRicerca.CurrentPageIndex = 0

            Dim wsOUT As New WS_SIGeD.INDICE_FASCICOLO
            Dim wsELENCO() As WS_SIGeD.COLLEGAMENTO_DOCUMENTO_TROVATO

            Dim sEsito As String
            Dim dr As DataRow
            Dim riga As Integer

            SIGED = New clsSiged("", strNome, strCognome)
            If SIGED.Codice_Esito <> 0 Then
                lblmessaggio.Text = SIGED.Esito
                Exit Sub
            End If

            wsOUT = SIGED.SIGED_IndiceFascicoli(CodFascicolo)

            sEsito = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)

            If sEsito = 0 Then
                If SIGED.SIGED_Esito(wsOUT.ESITO) = "0" Then
                    lblmessaggio.Text = "Nessun fascicolo trovato"
                    dgRisultatoRicerca.Visible = False
                    lblRisultatoRicerca.Visible = False
                Else
                    dgRisultatoRicerca.Visible = True
                    lblRisultatoRicerca.Visible = True
                    wsELENCO = wsOUT.ELENCO_DOCUMENTI

                    dt.Columns.Add(New DataColumn("Codice Documento", GetType(String)))
                    dt.Columns.Add(New DataColumn("Data Protocollo", GetType(String)))
                    dt.Columns.Add(New DataColumn("Descrizione", GetType(String)))
                    dt.Columns.Add(New DataColumn("Numero", GetType(String)))
                    dt.Columns.Add(New DataColumn("Tipo Documento", GetType(String)))
                    'filtrare solo per i fascicolo "2011#PROT#1018/2011" leggere se è   FASC

                    For riga = LBound(wsELENCO) To UBound(wsELENCO)

                        SIGED.NormalizzaCodice(wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO)

                        If wsELENCO(riga).TIPOCOLLEGAMENTO <> "Documento Interno" Then
                            dr = dt.NewRow
                            Select Case wsELENCO(riga).TIPOCOLLEGAMENTO
                                Case "Fascicolo"
                                    dr(0) = wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO
                                    dr(1) = ""
                                    dr(2) = wsELENCO(riga).DESCRIZIONECOLLEGAMENTO
                                    dr(3) = Replace(wsELENCO(riga).DETTAGLICOLLEGAMENTO, " ", "") 'numero fascicolo
                                    dr(4) = wsELENCO(riga).TIPOCOLLEGAMENTO
                                Case "Protocollo Uscita"
                                    dr(0) = wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO
                                    dr(1) = wsELENCO(riga).DETTAGLICOLLEGAMENTO 'data del protocollo
                                    dr(2) = wsELENCO(riga).DESCRIZIONECOLLEGAMENTO
                                    dr(3) = SIGED.CodiceNormalizzato
                                    dr(4) = wsELENCO(riga).TIPOCOLLEGAMENTO 'tipolocia di documento (fascicolo,protocollo e documento interno)
                            End Select
                            dt.Rows.Add(dr)
                        End If
                    Next


                    dgRisultatoRicerca.DataSource = dt
                    dgRisultatoRicerca.DataBind()
                    lblRisultatoRicerca.Visible = True
                    Session("dtRisulatato") = dt

                End If
            Else
                lblmessaggio.Text = SIGED.SIGED_Esito(wsOUT.ESITO) 'Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))
                dgRisultatoRicerca.Visible = False
                lblRisultatoRicerca.Visible = False
            End If

        Catch ex As Exception
            lblmessaggio.Text = "Errore imprevisto: " & ex.Message
        Finally
            SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
        End Try
    End Sub



    Protected Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged

    End Sub
End Class