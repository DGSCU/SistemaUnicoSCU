Imports System.Data.SqlClient

Public Class WebAggiungiSedeProgetto
    Inherits System.Web.UI.Page
    Dim commandSQL As SqlClient.SqlCommand
    Dim dataReader As SqlClient.SqlDataReader
    Dim strSQL As String
    Dim dataSet As DataSet
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim myTransaction As System.Data.SqlClient.SqlTransaction
    Dim blnProgNazionale As Boolean



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
        Dim blnFAMI As Boolean
        Dim blnGMO As Boolean
        Dim blnEsteroUE As Boolean
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
        strSQL = "SELECT NazioneBase FROM TipiProgetto WHERE IdTipoProgetto = " & CInt(Request.QueryString("idTipoProg"))
        ChiudiDataReader(dataReader)
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dataReader.Read()
        blnProgNazionale = dataReader("NazioneBase")
        ChiudiDataReader(dataReader)

        If IsPostBack = True Then
            If (txtNoVittoNoAlloggio.Text = String.Empty) Then
                txtNoVittoNoAlloggio.Text = "0"
            End If
            If (txtVittoAlloggio.Text = String.Empty) Then
                txtVittoAlloggio.Text = "0"
            End If
            If (txtvitto.Text = String.Empty) Then
                txtvitto.Text = "0"
            End If
            If (TxtNumeroPostiFami.Text = String.Empty) Then
                TxtNumeroPostiFami.Text = "0"
            End If
            If (TxtNumeroPostiGMO.Text = String.Empty) Then
                TxtNumeroPostiGMO.Text = "0"
            End If
            txtvolontari.Text = CInt(txtNoVittoNoAlloggio.Text) + CInt(txtVittoAlloggio.Text) + CInt(txtvitto.Text)
        End If
        If IsPostBack = False Then

            lblidSede.Value = Request.QueryString("idSede")
            lblidsedeattuazione.Value = Request.QueryString("IdSedeAttuazione")
            lblidAttivita.Value = Request.QueryString("IdAttivita")
            lblidattEs.Value = Request.QueryString("idattES")
            'Verifica Dei Progetti attivi sulla Sede associata
            strSQL = "select count(*) as NProg" & _
            " from attività a  " & _
            " inner join attivitàEntiSediAttuazione on a.idattività=attivitàEntiSediAttuazione.idattività  " & _
            " inner join enti b on a.identepresentante=b.idente  " & _
            " left join bandiAttività h on a.idbandoattività=h.idbandoattività " & _
            " left join bando c on c.idbando=h.idbando   " & _
            " inner join statiattività e on a.idstatoattività=e.idstatoattività " & _
            " inner join ambitiattività f on f.idambitoattività=a.idambitoattività  " & _
            " inner join macroambitiattività g on f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività " & _
            " left join statibandiattività i on h.idstatobandoattività=i.idstatobandoattività  " & _
            " where(a.idattività Is Not null And b.idente = " & Session("idEnte") & " And attivitàEntiSediAttuazione.identesedeattuazione =" & lblidsedeattuazione.Value & ")and  " & _
            " (e.attiva=1 or e.daValutare=1 or e.DefaultStato=1 or e.daGraduare=1) "
            ChiudiDataReader(dataReader)
            dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
            dataReader.Read()
            If dataReader("Nprog") = 0 Then
                imgdettProg.Visible = False
            Else
                imgdettProg.Visible = True
            End If
            lblProgettiAttivi.Text = dataReader("Nprog")
            strSQL = "Select titolo, fami, EsteroUE,GiovaniMinoriOpportunità From attività where idattività=" & CInt(Request.QueryString("IdAttivita")) & ""
            ChiudiDataReader(dataReader)
            dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
            dataReader.Read()
            lblprogetto.Text = dataReader("Titolo")
            blnEsteroUE = dataReader("EsteroUE")
            blnFAMI = dataReader("fami")
            If blnFAMI = True Then
                divFAMI.Visible = True
            Else
                divFAMI.Visible = False
            End If
            blnGMO = dataReader("GiovaniMinoriOpportunità")

            Dim intComuneNazionale As Integer

            strSQL = "Select  entisedi.denominazione as sedeFisica,entisedi.prefissotelefono + '/' + entisedi.telefono as telefono, entisedi.indirizzo + '  n°'+ entisedi.civico  as Indirizzo, " & _
            " comuni.denominazione as comune,provincie.provincia,entisedi.cap," & _
            " entisediattuazioni.denominazione as sedeAttuazione, isnull(entisediattuazioni.note,'') as Note,comuni.ComuneNazionale, isnull(entisedi.CittaEstera,'') as CittaEstera " & _
            " from entisedi " & _
            " inner join entisediattuazioni on (entisedi.identesede=entisediattuazioni.identesede)  " & _
            " inner join comuni on  (comuni.idcomune=entisedi.idcomune)" & _
            " inner join provincie on (provincie.idprovincia=comuni.idprovincia)" & _
            " Where entisediattuazioni.identesede=" & lblidSede.Value & "  and entisediattuazioni.identesedeattuazione=" & lblidsedeattuazione.Value & " "
            ChiudiDataReader(dataReader)
            dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
            dataReader.Read()
            lblSedeFisica.Text = IIf(Not IsDBNull(dataReader("SedeFisica")), UCase(dataReader("SedeFisica")), "")
            lblSedeAttuazione.Text = IIf(Not IsDBNull(dataReader("SedeAttuazione")), UCase(dataReader("SedeAttuazione")), "")
            lblIndirizzo.Text = IIf(Not IsDBNull(dataReader("Indirizzo")), UCase(dataReader("Indirizzo")), "") & ", " & IIf(Not IsDBNull(dataReader("Comune")), dataReader("Comune"), "") & "  " & IIf(Not IsDBNull(dataReader("Cap")), dataReader("cap"), "") & "(" & IIf(Not IsDBNull(dataReader("Provincia")), dataReader("Provincia"), "") & ")"
            lbltelefono.Text = IIf(Not IsDBNull(dataReader("telefono")), dataReader("telefono"), "")
            txtNote.Text = dataReader("Note")
            intComuneNazionale = dataReader("ComuneNazionale")
            If dataReader("cittaestera") <> "" Then
                txtCitta.Text = dataReader("cittaestera")
                txtCitta.Enabled = False
            End If
            ChiudiDataReader(dataReader)

            If intComuneNazionale = 0 Then
                divSedeSecondaria.Visible = False
            Else
                divSedeSecondaria.Visible = True
            End If

            divGMO.Visible = False
            If CInt(Request.QueryString("Nazionale")) = 11 And intComuneNazionale = -1 And blnGMO Then
                divGMO.Visible = True
            End If
            If CInt(Request.QueryString("Nazionale")) = 12 And intComuneNazionale = 0 And blnGMO Then
                divGMO.Visible = True
            End If
            If CInt(Request.QueryString("Nazionale")) = 3 And blnGMO Then
                divGMO.Visible = True
            End If

            'verifico se sto in Modifica o in Inserimento
            strSQL = "Select * from attivitàentisediattuazione" & _
            " where idattività=" & lblidAttivita.Value & " and identesedeAttuazione=" & lblidsedeattuazione.Value & ""
            dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
            If dataReader.HasRows = True Then
                'cmdConferma.ImageUrl = "images/salva.jpg"
                isUpdate.Value = "1"
                dataReader.Read()
                txtNoVittoNoAlloggio.Text = dataReader("numeroPostiNoVittoNoAlloggio")
                txtVittoAlloggio.Text = dataReader("numeroPostiVittoAlloggio")
                txtvitto.Text = dataReader("numeroPostiVitto")
                txtvolontari.Text = CInt(txtNoVittoNoAlloggio.Text) + CInt(txtVittoAlloggio.Text) + CInt(txtvitto.Text)
                txtCognomeRif.Text = IIf(Not IsDBNull(dataReader("CognomeRiferimento")), dataReader("CognomeRiferimento"), "")
                txtNomeRif.Text = IIf(Not IsDBNull(dataReader("nomeRiferimento")), dataReader("nomeRiferimento"), "")
                txtCitta.Text = IIf(Not IsDBNull(dataReader("Città")), dataReader("Città"), "")
                txtDataNascita.Text = IIf(Not IsDBNull(dataReader("DataNascitaRiferimento")), dataReader("DataNascitaRiferimento"), "")
                txtCodSedeSecondaria.Text = IIf(Not IsDBNull(dataReader("IDEnteSedeAttuazioneSecondaria")), dataReader("IDEnteSedeAttuazioneSecondaria"), "")
                If divGMO.Visible Then
                    TxtNumeroPostiGMO.Text = IIf(Not IsDBNull(dataReader("NumeroPostiGMO")), dataReader("NumeroPostiGMO"), "0")
                End If
                If blnFAMI = True Then
                    TxtNumeroPostiFami.Text = dataReader("NumeroPostiFami")
                End If
            End If
            ChiudiDataReader(dataReader)

            If txtCodSedeSecondaria.Text <> "" Then
                strSQL = "Select enti.codiceregione, enti.denominazione, entisedi.indirizzo + '  n°'+ isnull(entisedi.civico,'')  as Indirizzo, " & _
                   " comuni.denominazione as comune,provincie.provincia," & _
                   " entisediattuazioni.denominazione as sedeAttuazione " & _
                   " from entisedi " & _
                   " inner join entisediattuazioni on (entisedi.identesede=entisediattuazioni.identesede)  " & _
                   " inner join comuni on  (comuni.idcomune=entisedi.idcomune)" & _
                   " inner join provincie on (provincie.idprovincia=comuni.idprovincia)" & _
                   " inner join enti on (entisedi.idente=enti.idente)" & _
                   " Where entisediattuazioni.identesedeattuazione=" & txtCodSedeSecondaria.Text & " "
                dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dataReader.Read()

                lblInfoSedeSecondaria.Text = "[" & dataReader("codiceregione") & "] " & _
                                                 dataReader("comune") & " - " & _
                                                  dataReader("indirizzo")
                ChiudiDataReader(dataReader)
            End If


            If Not blnProgNazionale Then
                divDatiPersonali.Visible = (intComuneNazionale = 0)
            ElseIf Request.QueryString("idTipoProg") = 9 Or Request.QueryString("idTipoProg") = 11 Or Request.QueryString("idTipoProg") = 4 Then
                divDatiPersonali.Visible = (intComuneNazionale = 0 And blnEsteroUE)
            End If
            If Request.QueryString("idTipoProg") = 11 Or Request.QueryString("idTipoProg") = 4 Then
                lblDataNascita.Visible = True
                txtDataNascita.Visible = True

            End If
            If Request.QueryString("idTipoProg") = 12 Then
                lblDataNascita.Visible = True
                txtDataNascita.Visible = True
            End If

        Else
            lblMessaggi.Text = String.Empty
        End If
    End Sub

    Private Sub cmdConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConferma.Click
        lblConferma.Text = String.Empty
        lblMessaggi.Text = String.Empty
        ChiudiDataReader(dataReader)

        Dim Motivazione As String
        Dim ComuneNazionale As Boolean
        Dim idclasseaccreditamento As Integer
        Dim NazioneBase As Boolean

        'Controllo congruenze sedi 07/08/2017
        If Store_PROGETTI_ConguenzaGeografica(CInt(Request.QueryString("IdAttivita")), CInt(Request.QueryString("IdSedeAttuazione")), Motivazione) = False Then
            lblMessaggi.Text = Motivazione
            Exit Sub
        End If


        Dim strmessaggio As String = String.Empty
        'Dim blnSedeSecondaria As Boolean
        'blnSedeSecondaria = Store_ConguenzaSedeSecondaria(CInt(Request.QueryString("IdAttivita")), CInt(Request.QueryString("IdSedeAttuazione")), CInt(Request.QueryString("IdSedeAttuazione")), strmessaggio)
        'If blnSedeSecondaria = false Then
        '    lblMessaggi.Text = strmessaggio
        '    Exit Sub
        'End If


        'controllo la congruenza della sede secondaria ADC 18/01/2021
        'Aggiungere controllo visibilità solo se progetto Italia
        If txtCodSedeSecondaria.Text <> "" Then
            If Store_ConguenzaSedeSecondaria(CInt(Request.QueryString("IdAttivita")), CInt(Request.QueryString("IdSedeAttuazione")), CInt(txtCodSedeSecondaria.Text), strmessaggio) = False Then
                lblMessaggi.Text = strmessaggio
                Exit Sub
            End If

        End If
        


        strSQL = "select Certificazione,isnull(noaccreditamento,0) as NoAccreditamento, idstatoentesede from " & _
        "entisediattuazioni   " & _
        "WHERE IDEnteSedeAttuazione = " & CInt(Request.QueryString("IdSedeAttuazione"))

        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dataReader.Read()

        If dataReader.Item("idstatoentesede") <> 1 Or dataReader.Item("Certificazione") <> 1 Then
            If dataReader.Item("NoAccreditamento") <> True Then
                lblMessaggi.Text = "Impossibile utilizzare la sede. Non risulta completa l'iscrizione della sede all'albo."
                ChiudiDataReader(dataReader)
                Exit Sub
            End If
        End If

        ChiudiDataReader(dataReader)


        strSQL = "select nazionebase from " & _
          "attività a inner join tipiprogetto b on a.idtipoprogetto = b.idtipoprogetto   " & _
          "WHERE a.IDAttività = " & CInt(Request.QueryString("IdAttivita"))

        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dataReader.Read()
        If dataReader.HasRows Then
            NazioneBase = dataReader.Item("nazionebase")
        End If
        ChiudiDataReader(dataReader)

        strSQL = "select c.comunenazionale, d.idclasseaccreditamento  " & _
          "from entisediattuazioni a inner join entisedi b on a.identesede = b.identesede   " & _
          "inner join comuni c on b.idcomune = c.idcomune   " & _
          "inner join enti d on b.idente = d.idente " & _
          "WHERE a.IDentesedeattuazione = " & CInt(Request.QueryString("IdSedeAttuazione"))

        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dataReader.Read()
        If dataReader.HasRows Then
            ComuneNazionale = dataReader.Item("comunenazionale")
            idclasseaccreditamento = dataReader.Item("idclasseaccreditamento")
        End If
        ChiudiDataReader(dataReader)

        If NazioneBase = False And ComuneNazionale = True And (idclasseaccreditamento = 8 Or idclasseaccreditamento = 9) Then
            'progetto estero sede appoggio italiana di ente titolare tutto ok

        Else
            strSQL = "select a.identesedeattuazione from " & _
                "entisediattuazioni a " & _
                "inner join entisedi b on a.identesede = b.identesede " & _
                "inner join enti c on b.idente = c.idente " & _
                "inner join entisettori d on c.idente = d.idente " & _
                "inner join macroambitiattività e on d.idmacroambitoattività = e.idmacroambitoattività " & _
                "inner join ambitiattività f on e.idmacroambitoattività = f.idmacroambitoattività " & _
                "inner join attività g on f.idambitoattività = g.idambitoattività " & _
                "WHERE g.IDAttività = " & CInt(Request.QueryString("IdAttivita")) & "  AND  a.IDEnteSedeAttuazione = " & CInt(Request.QueryString("IdSedeAttuazione"))
            strSQL = strSQL & " UNION SELECT IDENTESEDEATTUAZIONE FROM ENTISEDIATTUAZIONI WHERE ISNULL(NoAccreditamento,0) = 1 AND IDENTESEDEATTUAZIONE = " & CInt(Request.QueryString("IdSedeAttuazione"))

            dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
            dataReader.Read()
            If dataReader.HasRows = False Then
                lblMessaggi.Text = "Impossibile utilizzare la sede. L'ente a cui fa capo la sede non e' operante nel settore del progetto."
                ChiudiDataReader(dataReader)
                Exit Sub
            End If
            ChiudiDataReader(dataReader)
        End If



        If Session("TipoUtente") = "E" Then
            If CInt(txtNoVittoNoAlloggio.Text) = 0 And CInt(txtVittoAlloggio.Text) = 0 And CInt(txtvitto.Text) = 0 Then
                lblMessaggi.Text = "Indicare il numero di posti volontari."
                ChiudiDataReader(dataReader)
                Exit Sub
            End If
        End If

        If CInt(txtNoVittoNoAlloggio.Text) < 0 Or CInt(txtVittoAlloggio.Text) < 0 Or CInt(txtvitto.Text) < 0 Or CInt(TxtNumeroPostiGMO.Text) < 0 Then
            lblMessaggi.Text = "Impossibile utilizzare valori negativi."
            ChiudiDataReader(dataReader)
            Exit Sub
        End If

        If CInt(txtvolontari.Text) < TxtNumeroPostiFami.Text Then
            lblMessaggi.Text = "Il numero dei posti volontari FAMI è superiore al numero totale dei volontari indicato nella sede."
            ChiudiDataReader(dataReader)
            Exit Sub
        End If

        If CInt(txtvolontari.Text) < TxtNumeroPostiGMO.Text Then
            lblMessaggi.Text = "Il numero dei posti volontari GMO è superiore al numero totale dei volontari indicato nella sede."
            ChiudiDataReader(dataReader)
            Exit Sub
        End If

        If divDatiPersonali.Visible = True Then
            If Request.QueryString("idTipoProg") = 11 Or Request.QueryString("idTipoProg") = 12 Or Request.QueryString("idTipoProg") = 4 Then
                If txtDataNascita.Text = "" Then
                    lblMessaggi.Text = "Il Campo Data di Nascita e' obbligatorio."
                    ChiudiDataReader(dataReader)
                    Exit Sub
                End If

                Dim dataTmp As Date
                If (Date.TryParse(txtDataNascita.Text, dataTmp) = False) Then
                    lblMessaggi.Text = "Inserire la data nel formato gg/mm/aaaa."
                    ChiudiDataReader(dataReader)
                    Exit Sub
                End If
            End If
        End If
        'If CInt(txtvolontari.Text) / 2 < TxtNumeroPostiFami.Text Then
        '    lblMessaggi.Text = "Il numero dei posti volontari FAMI è superiore al 50% del numero totale dei volontari indicato nella sede."
        '    ChiudiDataReader(dataReader)
        '    Exit Sub
        'End If
        'eseguo controllo sulla capienza delle sedi solo se il progetto è NAZIONALE
        If blnProgNazionale Then
            strSQL = "SELECT BandiAttività.IdBando AS BANDO FROM attività INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività WHERE attività.IDAttività=" & lblidAttivita.Value
            dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
            dataReader.Read()
            If dataReader.HasRows = True Then 'ATTIVITA' LEGATA AL BANDO
                Dim idbando As Integer
                idbando = dataReader.Item("BANDO")
                'DEVO CONTROLLARE ,PER QUEL ENTE ,SU TUTTI I PROGETTI DELLO STESSO BANDO il numero dei volontari
                If Not dataReader Is Nothing Then
                    dataReader.Close()
                    dataReader = Nothing
                End If

                strSQL = "SELECT ISNULL(sum(ISNULL(attivitàentisediattuazione.NumeroPostiNoVittoNoAlloggio, 0) + ISNULL(attivitàentisediattuazione.NumeroPostiVittoAlloggio, 0) + ISNULL(attivitàentisediattuazione.NumeroPostiVitto, 0)),0) as TotVOL " & _
                "FROM BandiAttività " & _
                "INNER JOIN attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività " & _
                "Inner join TipiProgetto on Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
                "INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività " & _
                "WHERE TipiProgetto.NazioneBase = 1 AND attività.idstatoattività not in (6,7,10,11) and BandiAttività.IdBando = " & idbando & " and attività.idattività <> " & CInt(Request.QueryString("IdAttivita")) & " AND attivitàentisediattuazione.IDEnteSedeAttuazione = " & CInt(Request.QueryString("IdSedeAttuazione")) & " "
                dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dataReader.Read()
                Dim totvol As Integer

                totvol = dataReader.Item("TotVOL")
                ChiudiDataReader(dataReader)

                strSQL = "SELECT isnull(Nmaxvolontari,0) as Nmaxvolontari from entisediattuazioni where identesedeattuazione='" & Request.QueryString("IdSedeAttuazione") & "'"
                dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dataReader.Read()
                Dim NmaxVol As Integer
                NmaxVol = dataReader.Item("NMaxVolontari")


                If NmaxVol <= 0 Then
                    lblMessaggi.Text = "Impossibile utilizzare la sede. Il numero massimo di volontari allocabili non e' stato indicato in fase di accreditamento."
                    ChiudiDataReader(dataReader)
                    Exit Sub
                Else
                    ChiudiDataReader(dataReader)
                    If txtvolontari.Text = 0 And Session("TipoUtente") = "E" Then ' If txtvolontari.Text <> 0 Then
                        lblMessaggi.Text = "ATTENZIONE e' necessario indicare il numero dei Volontari."
                        Exit Sub

                    Else

                        If CInt(txtvolontari.Text) + totvol <= NmaxVol Then
                            If isUpdate.Value = "1" Then
                                If CInt(Request.QueryString("Nazionale")) >= 7 Or CInt(Request.QueryString("Nazionale")) = 3 Then
                                    Modifica()
                                Else
                                    Modifica_OLD()
                                End If

                            Else
                                If CInt(Request.QueryString("Nazionale")) >= 7 Or CInt(Request.QueryString("Nazionale")) = 3 Then
                                    Inserimento()
                                Else
                                    Inserimento_OLD()
                                End If

                            End If
                        Else

                            lblMessaggi.Text = "Impossibile utilizzare la sede. Il numero di posti volontario utilizzati supera la capienza massima definita in fase di accreditamento."
                            ChiudiDataReader(dataReader)
                            Exit Sub
                            '---------------------------------------------
                        End If
                    End If

                End If
                ChiudiDataReader(dataReader)

            Else 'ATTIVITA' NON LEGATO AL BANDO


                'bisogna controllare se attivita' e legata a un bando se no vale il codice sotto se si sopra ELSE
                ChiudiDataReader(dataReader)

                strSQL = "Select isnull(NMaxVolontari,0) as NMaxVolontari From entisediattuazioni where IdEnteSedeAttuazione=" & CInt(Request.QueryString("IdSedeAttuazione")) & ""
                dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dataReader.Read()


                If dataReader.Item("NMaxVolontari") = 0 Then
                    lblMessaggi.Text = "Impossibile utilizzare la sede. Il numero massimo di volontari allocabili non e' stato indicato in fase di accreditamento."
                    ChiudiDataReader(dataReader)
                    Exit Sub
                Else



                    If (txtvolontari.Text = String.Empty) Then
                        txtvolontari.Text = "0"
                    End If
                    If CInt(txtvolontari.Text) <= dataReader.Item("NMaxVolontari") Then
                        ChiudiDataReader(dataReader)
                        '--------------- STAVA DOPO L'END SANDOKAN-----------
                        If (isUpdate.Value = "1") Then
                            If CInt(Request.QueryString("Nazionale")) >= 7 Or CInt(Request.QueryString("Nazionale")) = 3 Then
                                Modifica()
                            Else
                                Modifica_OLD()
                            End If
                        Else
                            If CInt(Request.QueryString("Nazionale")) >= 7 Or CInt(Request.QueryString("Nazionale")) = 3 Then
                                Inserimento()
                            Else
                                Inserimento_OLD()
                            End If
                        End If
                        '---------------------------------------------
                    Else
                        lblMessaggi.Text = "Impossibile utilizzare la sede. Il numero di posti volontario utilizzati supera la capienza massima definita in fase di accreditamento."
                        ChiudiDataReader(dataReader)
                        Exit Sub
                    End If
                End If
                ChiudiDataReader(dataReader)
            End If
        Else
            If isUpdate.Value = "1" Then
                If CInt(Request.QueryString("Nazionale")) >= 7 Or CInt(Request.QueryString("Nazionale")) = 3 Then
                    Modifica()
                Else
                    Modifica_OLD()
                End If
            Else
                If CInt(Request.QueryString("Nazionale")) >= 7 Or CInt(Request.QueryString("Nazionale")) = 3 Then
                    Inserimento()
                Else
                    Inserimento_OLD()
                End If

            End If
        End If
    End Sub


    Function ControllaRegioneCompetenza(ByVal strIdAttivita As String, ByVal strIdEnteSedeAttuazione As String) As Boolean

        'variabile che prende l'id della regione di competenza qualora fosse presente in attività
        Dim strIdRegioneCompetenza As String

        'preparo la query per prendere l'id della regione di competenza su provincie
        strSQL = "SELECT isnull(IdRegioneCompetenza,0) as IdRegioneCompetenza "
        strSQL = strSQL & "FROM attività "
        strSQL = strSQL & "WHERE (IdAttività='" & strIdAttivita & "')"
        ChiudiDataReader(dataReader)
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dataReader.HasRows = True Then
            dataReader.Read()

            strIdRegioneCompetenza = dataReader("IdRegioneCompetenza")
            ChiudiDataReader(dataReader)

            'controllo se è null
            'se è null esco dalla funzione'
            'e procedo con l'inerimento
            If strIdRegioneCompetenza = 0 Then
                ChiudiDataReader(dataReader)
                ControllaRegioneCompetenza = True
                Return ControllaRegioneCompetenza
                Exit Function
            Else
                ChiudiDataReader(dataReader)

                'nel caso ci sia un idregionecompetenza
                'vado a controllare se si tratta di competenza nazionale
                strSQL = "select CodiceRegioneCompetenza from RegioniCompetenze where idregionecompetenza='" & strIdRegioneCompetenza & "'"

                dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

                If dataReader.HasRows = True Then
                    dataReader.Read()

                    'controllo se il tipo di competenza è NAZ (nazionale)
                    'se NAZ allora esco dalla routine e proseguo con l'inserimento
                    'oppure se è un progetto estero ed un ente con competenza regionale
                    If dataReader("CodiceRegioneCompetenza") = "NAZ" Then
                        ChiudiDataReader(dataReader)

                        ControllaRegioneCompetenza = True
                        Return ControllaRegioneCompetenza
                        Exit Function

                    Else
                        'se il codice non è nazionale vado a controllare se l'idregionecompetenza della sede
                        'che si sta tentando di inserire è uguale a quello presente in attività
                        'se non lo è blocco l'inserimento
                        'se lo è continuo con l'inserimento
                        ChiudiDataReader(dataReader)

                        'preparo la query per prendere l'id della regione di competenza su provincie
                        strSQL = "SELECT provincie.IdRegioneCompetenza AS IdRegioneCompetenza "
                        strSQL = strSQL & "FROM entisedi "
                        strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
                        strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
                        strSQL = strSQL & "INNER JOIN RegioniCompetenze ON provincie.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
                        strSQL = strSQL & "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede "
                        strSQL = strSQL & "WHERE (entisediattuazioni.IDEnteSedeAttuazione='" & strIdEnteSedeAttuazione & "')"

                        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

                        'controllo se l'id della sede che si sta cercando di inserire
                        'è lo stesso di quello presente ni attività
                        'se lo è do il via libera per l'inserimento
                        'altrimenti blocco l'inserimento
                        If dataReader.HasRows = True Then
                            dataReader.Read()
                            If dataReader("IdRegioneCompetenza") = strIdRegioneCompetenza Then
                                'chiudo il datareader
                                ChiudiDataReader(dataReader)
                                ControllaRegioneCompetenza = True
                                Return ControllaRegioneCompetenza
                                Exit Function
                            Else
                                ChiudiDataReader(dataReader)
                                ControllaRegioneCompetenza = False
                                Return ControllaRegioneCompetenza
                                Exit Function
                            End If
                        End If
                    End If
                End If
            End If
        End If
        ChiudiDataReader(dataReader)
    End Function
    Private Sub Inserimento()
        Dim strIdRegioneCompetenza As String
        Dim strUpdateAttivita As String = ""
        Dim Tipologia As String
        Dim transfrontaliero As Integer
        Dim esteroue As Boolean
        Dim intIdTipoProgetto As Integer

        '1=Nazionale, 2=Estero, 3=Bando Straordinario
        ChiudiDataReader(dataReader)

        'verifico il transfrontaliero
        strSQL = "Select  isnull(transfrontaliero,0) as transfrontaliero, isnull(esteroue,0) as esteroue, idtipoprogetto " & _
                " from attività " & _
                "Where Idattività=" & lblidAttivita.Value & " "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dataReader.HasRows = True Then
            dataReader.Read()
            transfrontaliero = dataReader("transfrontaliero")
            esteroue = dataReader("EsteroUE")
            intIdTipoProgetto = dataReader("idtipoprogetto")
        End If
        ChiudiDataReader(dataReader)

        'If (idTipoProgetto = 2 Or idTipoProgetto = 6 Or idTipoProgetto = 8) And Tipologia <> "Amministrazione Statale" Then
        If Not blnProgNazionale Then
            If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                lblMessaggi.Text = "Sui progetti esteri e' obbligatorio fornire il vitto e alloggio."
                Exit Sub
            End If
        End If

        If blnProgNazionale And esteroue And transfrontaliero = 0 Then
            If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                lblMessaggi.Text = "Sui progetti che prevedono un periodo all'estero nell'Unione Europea e' obbligatorio fornire il vitto e alloggio."
                Exit Sub
            End If
        End If

        If blnProgNazionale And esteroue And transfrontaliero = 1 Then
            If txtNoVittoNoAlloggio.Text <> "0" Then
                lblMessaggi.Text = "Per i progetti in territorio transfrontaliero e' obbligatorio fornire il vitto."
                Exit Sub
            End If
        End If
        'verifico se la sede è estera

        strSQL = "Select  comuni.ComuneNazionale " & _
                " from entisedi " & _
                "inner join entisediattuazioni on (entisedi.identesede=entisediattuazioni.identesede) " & _
                "inner join comuni on  (comuni.idcomune=entisedi.idcomune) " & _
                "Where entisediattuazioni.IdEnteSedeAttuazione='" & lblidsedeattuazione.Value & "' "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dataReader.HasRows = True Then
            dataReader.Read()
            ' Leucci 21/11/2018
            Dim Nazionale As Boolean = dataReader("ComuneNazionale")
            ChiudiDataReader(dataReader)

            'If dataReader("ComuneNazionale") = 0 And Not blnProgNazionale Then
            '  ChiudiDataReader(dataReader)
            If Not Nazionale And Not blnProgNazionale Then
                If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                    lblMessaggi.Text = "Sulle sedi all'estero e' obbligatorio fornire il vitto e alloggio."
                    Exit Sub
                End If

                If Trim(txtCognomeRif.Text) = "" Or Trim(txtNomeRif.Text) = "" Or Trim(txtCitta.Text) = "" Then
                    lblMessaggi.Text = "E' necessario compilare tutti i campi Cognome, Nome , Città."
                    Exit Sub
                End If
            End If

            'If dataReader("ComuneNazionale") = 0 And blnProgNazionale And esteroue Then
            '  ChiudiDataReader(dataReader)
            If Not Nazionale And blnProgNazionale And esteroue Then
                If txtDataNascita.Visible Then
                    If Trim(txtCognomeRif.Text) = "" Or Trim(txtNomeRif.Text) = "" Or Trim(txtCitta.Text) = "" Or Trim(txtDataNascita.Text) = "" Then
                        lblMessaggi.Text = "E' necessario compilare tutti i campi Cognome, Nome, Data Nascita e Città ."
                        Exit Sub
                    End If
                Else
                    If Trim(txtCognomeRif.Text) = "" Or Trim(txtNomeRif.Text) = "" Or Trim(txtCitta.Text) = "" Then
                        lblMessaggi.Text = "E' necessario compilare tutti i campi Cognome, Nome e Città ."
                        Exit Sub
                    End If
                End If
 
            End If
        End If
        ChiudiDataReader(dataReader)

        strSQL = "select IdAttivitàEnteSedeAttuazione from AttivitàEntiSediAttuazione "
        strSQL = strSQL & "where (IdAttività='" & lblidAttivita.Value & "' and IdEnteSedeAttuazione='" & lblidsedeattuazione.Value & "') "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dataReader.HasRows = True Then
            ChiudiDataReader(dataReader)
            Exit Sub
        End If
        ChiudiDataReader(dataReader)

        If ControllaRegioneCompetenza(lblidAttivita.Value, lblidsedeattuazione.Value) = False Then
            lblMessaggi.Text = "La sede selezionata non e' compatibile con la competenza del progetto."
            Exit Sub
        End If

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        ChiudiDataReader(dataReader)

        'query che controlla l'id della regione di competenza del progetto
        'nel caso in cui sia null vuol dire che devo fare l'update su attività
        'ed inserire l'id della regione di competenza in base all'id del comune
        'della sede in questione
        strSQL = "select isnull(IdRegioneCompetenza,0) as IdRegioneCompetenza from Attività "
        strSQL = strSQL & "where (IdAttività='" & lblidAttivita.Value & "') "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        Dim competenzaprogetto As Integer
        'controllo se ci sono righe
        If dataReader.HasRows = True Then
            dataReader.Read()
            competenzaprogetto = dataReader("IdRegioneCompetenza") 'competenza progetto
            'se è a null vado a prendermi l'idregionecompetenza relativo alla sede selezionata
            If competenzaprogetto = 0 Then
                ChiudiDataReader(dataReader)
                'preparo la query per prendere l'id della regione di competenza su provincie
                strSQL = "SELECT provincie.IdRegioneCompetenza AS IdRegioneCompetenza "
                strSQL = strSQL & "FROM entisedi "
                strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
                strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
                strSQL = strSQL & "INNER JOIN RegioniCompetenze ON provincie.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
                strSQL = strSQL & "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede "
                strSQL = strSQL & "WHERE (entisediattuazioni.IDEnteSedeAttuazione='" & lblidsedeattuazione.Value & "')"

                dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

                'controllo se ci sono righe
                If dataReader.HasRows = True Then
                    dataReader.Read()
                    'assegno ad una variabile locale l'idregione di competenza che poi userò 
                    'per l'update su attività
                    strIdRegioneCompetenza = dataReader("IdRegioneCompetenza") ' competenza sede

                    'Se si stà inserendo un progetto estero e l'ente è regionale non aggiorno IdRegioneCompetenza su attività

                    'If (CInt(idTipoProgetto) = 2 Or CInt(idTipoProgetto) = 5) And strIdRegioneCompetenza <> "NAZ" Then       'PROGETTO ESTERO / ENTE REGIONALE

                    'Else
                    '    'preparo la stringa per l'update in attività
                    '    strUpdateAttivita = "update attività set IdRegioneCompetenza='" & strIdRegioneCompetenza & "' where idattività='" & lblidAttivita.Value & "'"
                    'End If

                    '    'preparo la stringa per l'update in attività
                    If competenzaprogetto = 0 Then
                        strUpdateAttivita = "update attività set IdRegioneCompetenza='" & strIdRegioneCompetenza & "' where idattività='" & lblidAttivita.Value & "'"
                    End If
                    ChiudiDataReader(dataReader)
                End If
                ChiudiDataReader(dataReader)

            End If
        End If
        ChiudiDataReader(dataReader)
        '******************************************************************************

        Dim cittaesteraaccreditamento As String
        strSQL = "select isnull(cittaestera,'') as cittaestera from entisedi a inner join entisediattuazioni b on a.identesede = b.identesede where b.identesedeattuazione = " & lblidsedeattuazione.Value
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dataReader.Read()
        cittaesteraaccreditamento = dataReader("cittaestera")
        ChiudiDataReader(dataReader)

        Try
            myTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = myTransaction
            'modifico l'id della regione di competenza
            If strUpdateAttivita <> "" Then
                myCommand.CommandText = strUpdateAttivita
                myCommand.ExecuteNonQuery()
            End If
            'ChiudiDataReader(dataReader)

            If cittaesteraaccreditamento = "" And txtCitta.Text <> "" Then
                strSQL = "update entisedi set cittaestera = '" & txtCitta.Text.Replace("'", "''") & "' from entisedi a inner join entisediattuazioni b on a.identesede = b.identesede where b.identesedeattuazione = " & lblidsedeattuazione.Value
                myCommand.CommandText = strSQL
                myCommand.ExecuteNonQuery()
            End If


            strSQL = "insert into attivitàentisediattuazione " & _
            "(idattività,identesedeattuazione,IDEnteSedeAttuazioneSecondaria,NumeroPostiNoVittoNoAlloggio,NumeroPostiVittoAlloggio,NumeroPostiVitto,CognomeRiferimento, NomeRiferimento,Città,DataNascitaRiferimento,UsernameStato,DataUltimoStato,NumeroPostiFami,NumeroPostiGMO)" & _
            " values(" & lblidAttivita.Value & "," & lblidsedeattuazione.Value & "," & IIf(txtCodSedeSecondaria.Text = "", "Null", "'" & txtCodSedeSecondaria.Text & "'") & "," & txtNoVittoNoAlloggio.Text & "," & txtVittoAlloggio.Text & "," & txtvitto.Text & ",'" & txtCognomeRif.Text.Replace("'", "''") & "','" & txtNomeRif.Text.Replace("'", "''") & "','" & txtCitta.Text.Replace("'", "''") & "'," & IIf(txtDataNascita.Text = "", "Null", "'" & txtDataNascita.Text & "'") & " ,'" & Session("Utente") & "', getdate()" & "," & TxtNumeroPostiFami.Text & "," & IIf(divGMO.Visible, TxtNumeroPostiGMO.Text, "0") & " ) "
            myCommand.CommandText = strSQL
            myCommand.ExecuteNonQuery()
            'ChiudiDataReader(dataReader)

            Dim newID As Int32
            strSQL = "select scope_identity() as id"

            myCommand.CommandText = strSQL

            newID = Convert.ToInt32(myCommand.ExecuteScalar())
            'myCommand.ExecuteScalar()


            strSQL = "insert into Cronologiaattivitàentisediattuazione " & _
            "(idAttivitàentesedeattuazione,idattività,identesedeattuazione,NumeroPostiNoVittoNoAlloggio,NumeroPostiVittoAlloggio,NumeroPostiVitto,CognomeRiferimento, NomeRiferimento,Città,DataNascitaRiferimento,UsernameStato,DataUltimoStato,NumeroPostiFami,NumeroPostiGMO)" & _
            " values(" & newID & "," & lblidAttivita.Value & "," & lblidsedeattuazione.Value & "," & txtNoVittoNoAlloggio.Text & "," & txtVittoAlloggio.Text & "," & txtvitto.Text & ",'" & txtCognomeRif.Text.Replace("'", "''") & "','" & txtNomeRif.Text.Replace("'", "''") & "','" & txtCitta.Text.Replace("'", "''") & "'," & IIf(txtDataNascita.Text = "", "Null", "'" & txtDataNascita.Text & "'") & " ,'" & Session("Utente") & "', getdate()" & "," & TxtNumeroPostiFami.Text & "," & IIf(divGMO.Visible, TxtNumeroPostiGMO.Text, "0") & " ) "
            myCommand.CommandText = strSQL
            myCommand.ExecuteNonQuery()

            ChiudiDataReader(dataReader)

            Dim dtrlocal As SqlClient.SqlDataReader
            Dim intPostiVittoAlloggio As Integer
            Dim intPostiNovittoNoAlloggio As Integer
            Dim intPostiVitto As Integer
            Dim intPostiFami As Integer
            Dim intPostiGMO As Integer

            'se è nazionale 
            '1nazionale
            '2estero

            strSQL = "SELECT SUM(a.NumeroPostiNoVittoNoAlloggio) AS NoVittoNoAloggio, "
            strSQL = strSQL & "SUM(a.NumeroPostiVittoAlloggio) AS VittoAloggio, "
            strSQL = strSQL & "SUM(a.NumeroPostiVitto) AS Vitto, "
            strSQL = strSQL & "SUM(a.NumeroPostiFami) AS NumeroPostiFami ,"
            strSQL = strSQL & "SUM(a.NumeroPostiGMO) AS NumeroPostiGMO "
            strSQL = strSQL & "FROM attivitàentisediattuazione a "
            strSQL = strSQL & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
            strSQL = strSQL & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
            strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
            strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
            strSQL = strSQL & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
            strSQL = strSQL & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
            strSQL = strSQL & "WHERE (a.IDAttività='" & lblidAttivita.Value & "')"

            If blnProgNazionale = True Then
                'Area - Settore
                'strSQL = "select sum(a.NumeroPostiNovittoNoAlloggio) as NoVittoNoAloggio, "
                'strSQL = strSQL & "sum(a.NumeroPostivittoAlloggio) as VittoAloggio, "
                'strSQL = strSQL & "sum(a.NumeroPostivitto) as Vitto "
                'strSQL = strSQL & "from attivitàentisediattuazione as a "
                'strsql = strsql & "where a.idattività='" & lblidAttivita.Value & "'"

                strSQL = strSQL & "  AND (nazioni.NazioneBase = 1)"
            Else
                'strSQL = "SELECT SUM(a.NumeroPostiNoVittoNoAlloggio) AS NoVittoNoAloggio, "
                'strSQL = strSQL & "SUM(a.NumeroPostiVittoAlloggio) AS VittoAloggio, "
                'strSQL = strSQL & "SUM(a.NumeroPostiVitto) AS Vitto "
                'strSQL = strSQL & "FROM attivitàentisediattuazione a "
                'strSQL = strSQL & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
                'strSQL = strSQL & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
                'strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
                'strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
                'strSQL = strSQL & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
                'strSQL = strSQL & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
                'strsql = strsql & "WHERE (a.IDAttività='" & lblidAttivita.Value & "') AND (nazioni.NazioneBase = 0)"

                strSQL = strSQL & "  AND (nazioni.NazioneBase = 0)"
            End If

            myCommand.CommandText = strSQL
            dtrlocal = myCommand.ExecuteReader
            dtrlocal.Read()
            If dtrlocal.HasRows = True Then

                If IsDBNull(dtrlocal("NoVittoNoAloggio")) = False Then
                    intPostiNovittoNoAlloggio = CInt(dtrlocal("NoVittoNoAloggio"))
                Else
                    intPostiNovittoNoAlloggio = 0
                End If
                If IsDBNull(dtrlocal("VittoAloggio")) = False Then
                    intPostiVittoAlloggio = CInt(dtrlocal("VittoAloggio"))
                Else
                    intPostiVittoAlloggio = 0
                End If
                If IsDBNull(dtrlocal("Vitto")) = False Then
                    intPostiVitto = CInt(dtrlocal("Vitto"))
                Else
                    intPostiVitto = 0
                End If
                If IsDBNull(dtrlocal("NumeroPostiFami")) = False Then
                    intPostiFami = CInt(dtrlocal("NumeroPostiFami"))
                Else
                    intPostiFami = 0
                End If
                If IsDBNull(dtrlocal("NumeroPostiGMO")) = False Then
                    intPostiGMO = CInt(dtrlocal("NumeroPostiGMO"))
                Else
                    intPostiGMO = 0
                End If

            End If
            ChiudiDataReader(dtrlocal)

            strSQL = "Update attività set NumeroPostiNoVittoNoAlloggio=" & intPostiNovittoNoAlloggio & ", "
            strSQL = strSQL & "NumeroPostiVittoAlloggio=" & intPostiVittoAlloggio & ", "
            strSQL = strSQL & "NumeroPostiVitto=" & intPostiVitto & " ,"
            strSQL = strSQL & "NumeroPostiFami=" & intPostiFami & " "
            If intIdTipoProgetto >= 11 Or CInt(Request.QueryString("Nazionale")) = 3 Or CInt(Request.QueryString("Nazionale")) = 4 Then
                strSQL = strSQL & ",NumeroGiovaniMinoriOpportunità=" & intPostiGMO & " "
            End If
            strSQL = strSQL & "where idattività=" & CInt(lblidAttivita.Value)
            myCommand.CommandText = strSQL
            myCommand.ExecuteNonQuery()
            ChiudiDataReader(dataReader)
            'Aggiunto Da Alessandra Taballione il 27/04/2005
            'Controllo se è già stata inserita
            strSQL = "Select idAttivitàSedeassegnazione from AttivitàSediAssegnazione where identesede=" & lblidSede.Value & " and idattività=" & lblidAttivita.Value & ""
            myCommand.CommandText = strSQL
            myCommand.ExecuteNonQuery()
            ChiudiDataReader(dataReader)
            dataReader = myCommand.ExecuteReader()
            If dataReader.HasRows = False Then
                ChiudiDataReader(dataReader)
                strSQL = "insert into AttivitàSediAssegnazione (IdEnteSede, IdAttività, StatoGraduatoria, UsernameStato, DataUltimoStato) "
                strSQL = strSQL & "values"
                strSQL = strSQL & "(" & lblidSede.Value & "," & lblidAttivita.Value & ", 1,'" & Session("Utente") & "', getdate()" & ") "
                myCommand.CommandText = strSQL
                myCommand.ExecuteNonQuery()
            Else
                ChiudiDataReader(dataReader)
            End If



            myTransaction.Commit()

            ChiudiDataReader(dataReader)

            Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            myCommandSP.CommandText = "SP_PROGRAMMI_AGGIORNA_POSTI_da_progetto"
            myCommandSP.CommandType = CommandType.StoredProcedure

            myCommandSP.Connection = Session("conn")
            myCommandSP.Parameters.AddWithValue("@IdAttività", lblidAttivita.Value)
            myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            myCommandSP.Parameters("@messaggio").Size = 1000
            myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
            myCommandSP.ExecuteNonQuery()


            'Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            myCommandSP.CommandText = "SP_PROGETTI_COPROGETTAZIONE_ACCOGLIENZA"
            myCommandSP.CommandType = CommandType.StoredProcedure
            myCommandSP.Connection = Session("conn")
            myCommandSP.Parameters.Clear()
            myCommandSP.Parameters.AddWithValue("@IdAttività", lblidAttivita.Value)
            myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            myCommandSP.Parameters("@messaggio").Size = 1000
            myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
            myCommandSP.ExecuteNonQuery()

            ChiudiDataReader(dataReader)

            If txtCodSedeSecondaria.Text <> "" Then
                strSQL = "Select enti.codiceregione, enti.denominazione, entisedi.indirizzo + '  n°'+ isnull(entisedi.civico,'')  as Indirizzo, " & _
                      " comuni.denominazione as comune,provincie.provincia," & _
                      " entisediattuazioni.denominazione as sedeAttuazione " & _
                      " from entisedi " & _
                      " inner join entisediattuazioni on (entisedi.identesede=entisediattuazioni.identesede)  " & _
                      " inner join comuni on  (comuni.idcomune=entisedi.idcomune)" & _
                      " inner join provincie on (provincie.idprovincia=comuni.idprovincia)" & _
                      " inner join enti on (entisedi.idente=enti.idente)" & _
                      " Where entisediattuazioni.identesedeattuazione=" & txtCodSedeSecondaria.Text & " "
                dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dataReader.Read()

                lblInfoSedeSecondaria.Text = "[" & dataReader("codiceregione") & "] " & _
                                                 dataReader("comune") & " - " & _
                                                  dataReader("indirizzo")
                ChiudiDataReader(dataReader)
            End If

            lblConferma.Text = "Inserimento avvenuto con successo."
            isUpdate.Value = "1" ' imposto il flag per la modifica
        Catch ee As Exception
            lblMessaggi.Text = ee.Message.ToString
            ChiudiDataReader(dataReader)
            myTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))

        End Try
        myTransaction.Dispose()
    End Sub

    Private Sub Inserimento_OLD()
        Dim strIdRegioneCompetenza As String
        Dim strUpdateAttivita As String = ""
        Dim Tipologia As String


        '1=Nazionale, 2=Estero, 3=Bando Straordinario
        ChiudiDataReader(dataReader)

        'SANDOKAN 02/01/2012 Controllo per la validita dei volontari con vitto e aloggio su progetto estero
        strSQL = "select Tipologia from enti "
        strSQL = strSQL & "where IdEnte=" & Session("idente")
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
        If dataReader.HasRows = True Then
            dataReader.Read()
            Tipologia = dataReader("Tipologia")
        End If
        ChiudiDataReader(dataReader)

        'If (idTipoProgetto = 2 Or idTipoProgetto = 6 Or idTipoProgetto = 8) And Tipologia <> "Amministrazione Statale" Then
        If Not blnProgNazionale And Tipologia <> "Amministrazione Statale" Then
            If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                lblMessaggi.Text = "Sui progetti esteri e' obbligatorio fornire il vitto e alloggio."
                Exit Sub
            End If
        End If

        'verifico se la sede è estera

        strSQL = "Select  comuni.ComuneNazionale " & _
                " from entisedi " & _
                "inner join entisediattuazioni on (entisedi.identesede=entisediattuazioni.identesede) " & _
                "inner join comuni on  (comuni.idcomune=entisedi.idcomune) " & _
                "Where entisediattuazioni.IdEnteSedeAttuazione='" & lblidsedeattuazione.Value & "' "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dataReader.HasRows = True Then
            dataReader.Read()
            If dataReader("ComuneNazionale") = 0 And Not blnProgNazionale Then
                ChiudiDataReader(dataReader)
                If Tipologia <> "Amministrazione Statale" Then
                    If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                        lblMessaggi.Text = "Sulle sedi all'estero e' obbligatorio fornire il vitto e alloggio."
                        Exit Sub
                    End If
                End If
                If Trim(txtCognomeRif.Text) = "" Or Trim(txtNomeRif.Text) = "" Or Trim(txtCitta.Text) = "" Then
                    lblMessaggi.Text = "E' necessario compilare tutti i campi Cognome, Nome, Città."
                    Exit Sub
                End If
            End If
        End If
        ChiudiDataReader(dataReader)

        strSQL = "select IdAttivitàEnteSedeAttuazione from AttivitàEntiSediAttuazione "
        strSQL = strSQL & "where (IdAttività='" & lblidAttivita.Value & "' and IdEnteSedeAttuazione='" & lblidsedeattuazione.Value & "') "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dataReader.HasRows = True Then
            ChiudiDataReader(dataReader)
            Exit Sub
        End If
        ChiudiDataReader(dataReader)

        If ControllaRegioneCompetenza(lblidAttivita.Value, lblidsedeattuazione.Value) = False Then
            lblMessaggi.Text = "La sede selezionata non e' compatibile con la competenza del progetto."
            Exit Sub
        End If

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        ChiudiDataReader(dataReader)

        'query che controlla l'id della regione di competenza del progetto
        'nel caso in cui sia null vuol dire che devo fare l'update su attività
        'ed inserire l'id della regione di competenza in base all'id del comune
        'della sede in questione
        strSQL = "select isnull(IdRegioneCompetenza,0) as IdRegioneCompetenza from Attività "
        strSQL = strSQL & "where (IdAttività='" & lblidAttivita.Value & "') "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        Dim competenzaprogetto As Integer
        'controllo se ci sono righe
        If dataReader.HasRows = True Then
            dataReader.Read()
            competenzaprogetto = dataReader("IdRegioneCompetenza") 'competenza progetto
            'se è a null vado a prendermi l'idregionecompetenza relativo alla sede selezionata
            If competenzaprogetto = 0 Then
                ChiudiDataReader(dataReader)
                'preparo la query per prendere l'id della regione di competenza su provincie
                strSQL = "SELECT provincie.IdRegioneCompetenza AS IdRegioneCompetenza "
                strSQL = strSQL & "FROM entisedi "
                strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
                strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
                strSQL = strSQL & "INNER JOIN RegioniCompetenze ON provincie.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
                strSQL = strSQL & "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede "
                strSQL = strSQL & "WHERE (entisediattuazioni.IDEnteSedeAttuazione='" & lblidsedeattuazione.Value & "')"

                dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

                'controllo se ci sono righe
                If dataReader.HasRows = True Then
                    dataReader.Read()
                    'assegno ad una variabile locale l'idregione di competenza che poi userò 
                    'per l'update su attività
                    strIdRegioneCompetenza = dataReader("IdRegioneCompetenza") ' competenza sede

                    'Se si stà inserendo un progetto estero e l'ente è regionale non aggiorno IdRegioneCompetenza su attività

                    'If (CInt(idTipoProgetto) = 2 Or CInt(idTipoProgetto) = 5) And strIdRegioneCompetenza <> "NAZ" Then       'PROGETTO ESTERO / ENTE REGIONALE

                    'Else
                    '    'preparo la stringa per l'update in attività
                    '    strUpdateAttivita = "update attività set IdRegioneCompetenza='" & strIdRegioneCompetenza & "' where idattività='" & lblidAttivita.Value & "'"
                    'End If

                    '    'preparo la stringa per l'update in attività
                    If competenzaprogetto = 0 Then
                        strUpdateAttivita = "update attività set IdRegioneCompetenza='" & strIdRegioneCompetenza & "' where idattività='" & lblidAttivita.Value & "'"
                    End If
                    ChiudiDataReader(dataReader)
                End If
                ChiudiDataReader(dataReader)

            End If
        End If
        ChiudiDataReader(dataReader)
        '******************************************************************************


        Try
            myTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = myTransaction
            'modifico l'id della regione di competenza
            If strUpdateAttivita <> "" Then
                myCommand.CommandText = strUpdateAttivita
                myCommand.ExecuteNonQuery()
            End If
            'ChiudiDataReader(dataReader)

            strSQL = "insert into attivitàentisediattuazione " & _
            "(idattività,identesedeattuazione,IDEnteSedeAttuazioneSecondaria,NumeroPostiNoVittoNoAlloggio,NumeroPostiVittoAlloggio,NumeroPostiVitto,CognomeRiferimento, NomeRiferimento,Città,DataNascitaRiferimento, UsernameStato,DataUltimoStato,NumeroPostiFami)" & _
            " values(" & lblidAttivita.Value & "," & lblidsedeattuazione.Value & "," & IIf(txtCodSedeSecondaria.Text = "", "Null", "'" & txtCodSedeSecondaria.Text & "'") & "," & txtNoVittoNoAlloggio.Text & "," & txtVittoAlloggio.Text & "," & txtvitto.Text & ",'" & txtCognomeRif.Text.Replace("'", "''") & "','" & txtNomeRif.Text.Replace("'", "''") & "','" & txtCitta.Text.Replace("'", "''") & "','" & txtDataNascita.Text & "','" & Session("Utente") & "', getdate()" & "," & TxtNumeroPostiFami.Text & ") "
            myCommand.CommandText = strSQL
            myCommand.ExecuteNonQuery()
            'ChiudiDataReader(dataReader)

            Dim newID As Int32
            strSQL = "select scope_identity() as id"

            myCommand.CommandText = strSQL

            newID = Convert.ToInt32(myCommand.ExecuteScalar())
            'myCommand.ExecuteScalar()


            strSQL = "insert into Cronologiaattivitàentisediattuazione " & _
            "(idAttivitàentesedeattuazione,idattività,identesedeattuazione,NumeroPostiNoVittoNoAlloggio,NumeroPostiVittoAlloggio,NumeroPostiVitto,CognomeRiferimento, NomeRiferimento,Città,DataNascitaRiferimento,UsernameStato,DataUltimoStato,NumeroPostiFami)" & _
            " values(" & newID & "," & lblidAttivita.Value & "," & lblidsedeattuazione.Value & "," & txtNoVittoNoAlloggio.Text & "," & txtVittoAlloggio.Text & "," & txtvitto.Text & ",'" & txtCognomeRif.Text.Replace("'", "''") & "','" & txtNomeRif.Text.Replace("'", "''") & "','" & txtCitta.Text.Replace("'", "''") & "','" & txtDataNascita.Text & "','" & Session("Utente") & "', getdate()" & "," & TxtNumeroPostiFami.Text & ") "
            myCommand.CommandText = strSQL
            myCommand.ExecuteNonQuery()

            ChiudiDataReader(dataReader)

            Dim dtrlocal As SqlClient.SqlDataReader
            Dim intPostiVittoAlloggio As Integer
            Dim intPostiNovittoNoAlloggio As Integer
            Dim intPostiVitto As Integer
            Dim intPostiFami As Integer
            Dim intPostiGMO As Integer

            'se è nazionale 
            '1nazionale
            '2estero

            strSQL = "SELECT SUM(a.NumeroPostiNoVittoNoAlloggio) AS NoVittoNoAloggio, "
            strSQL = strSQL & "SUM(a.NumeroPostiVittoAlloggio) AS VittoAloggio, "
            strSQL = strSQL & "SUM(a.NumeroPostiVitto) AS Vitto, "
            strSQL = strSQL & "SUM(a.NumeroPostiFami) AS NumeroPostiFami ,"
            strSQL = strSQL & "SUM(a.NumeroPostiGMO) AS NumeroPostiGMO "
            strSQL = strSQL & "FROM attivitàentisediattuazione a "
            strSQL = strSQL & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
            strSQL = strSQL & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
            strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
            strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
            strSQL = strSQL & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
            strSQL = strSQL & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
            strSQL = strSQL & "WHERE (a.IDAttività='" & lblidAttivita.Value & "')"

            If blnProgNazionale = True Then
                'Area - Settore
                'strSQL = "select sum(a.NumeroPostiNovittoNoAlloggio) as NoVittoNoAloggio, "
                'strSQL = strSQL & "sum(a.NumeroPostivittoAlloggio) as VittoAloggio, "
                'strSQL = strSQL & "sum(a.NumeroPostivitto) as Vitto "
                'strSQL = strSQL & "from attivitàentisediattuazione as a "
                'strsql = strsql & "where a.idattività='" & lblidAttivita.Value & "'"

                strSQL = strSQL & "  AND (nazioni.NazioneBase = 1)"
            Else
                'strSQL = "SELECT SUM(a.NumeroPostiNoVittoNoAlloggio) AS NoVittoNoAloggio, "
                'strSQL = strSQL & "SUM(a.NumeroPostiVittoAlloggio) AS VittoAloggio, "
                'strSQL = strSQL & "SUM(a.NumeroPostiVitto) AS Vitto "
                'strSQL = strSQL & "FROM attivitàentisediattuazione a "
                'strSQL = strSQL & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
                'strSQL = strSQL & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
                'strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
                'strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
                'strSQL = strSQL & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
                'strSQL = strSQL & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
                'strsql = strsql & "WHERE (a.IDAttività='" & lblidAttivita.Value & "') AND (nazioni.NazioneBase = 0)"

                strSQL = strSQL & "  AND (nazioni.NazioneBase = 0)"
            End If

            myCommand.CommandText = strSQL
            dtrlocal = myCommand.ExecuteReader
            dtrlocal.Read()
            If dtrlocal.HasRows = True Then

                If IsDBNull(dtrlocal("NoVittoNoAloggio")) = False Then
                    intPostiNovittoNoAlloggio = CInt(dtrlocal("NoVittoNoAloggio"))
                Else
                    intPostiNovittoNoAlloggio = 0
                End If
                If IsDBNull(dtrlocal("VittoAloggio")) = False Then
                    intPostiVittoAlloggio = CInt(dtrlocal("VittoAloggio"))
                Else
                    intPostiVittoAlloggio = 0
                End If
                If IsDBNull(dtrlocal("Vitto")) = False Then
                    intPostiVitto = CInt(dtrlocal("Vitto"))
                Else
                    intPostiVitto = 0
                End If
                If IsDBNull(dtrlocal("NumeroPostiFami")) = False Then
                    intPostiFami = CInt(dtrlocal("NumeroPostiFami"))
                Else
                    intPostiFami = 0
                End If
                If IsDBNull(dtrlocal("NumeroPostiGMO")) = False Then
                    intPostiGMO = CInt(dtrlocal("NumeroPostiGMO"))
                Else
                    intPostiGMO = 0
                End If
            End If
            ChiudiDataReader(dtrlocal)

            strSQL = "Update attività set NumeroPostiNoVittoNoAlloggio=" & intPostiNovittoNoAlloggio & ", "
            strSQL = strSQL & "NumeroPostiVittoAlloggio=" & intPostiVittoAlloggio & ", "
            strSQL = strSQL & "NumeroPostiVitto=" & intPostiVitto & " ,"
            strSQL = strSQL & "NumeroPostiFami=" & intPostiFami & " ,"
            strSQL = strSQL & "NumeroGiovaniMinoriOpportunità=" & intPostiGMO & " "
            strSQL = strSQL & "where idattività=" & CInt(lblidAttivita.Value)
            myCommand.CommandText = strSQL
            myCommand.ExecuteNonQuery()
            ChiudiDataReader(dataReader)
            'Aggiunto Da Alessandra Taballione il 27/04/2005
            'Controllo se è già stata inserita
            strSQL = "Select idAttivitàSedeassegnazione from AttivitàSediAssegnazione where identesede=" & lblidSede.Value & " and idattività=" & lblidAttivita.Value & ""
            myCommand.CommandText = strSQL
            myCommand.ExecuteNonQuery()
            ChiudiDataReader(dataReader)
            dataReader = myCommand.ExecuteReader()
            If dataReader.HasRows = False Then
                ChiudiDataReader(dataReader)
                strSQL = "insert into AttivitàSediAssegnazione (IdEnteSede, IdAttività, StatoGraduatoria, UsernameStato, DataUltimoStato) "
                strSQL = strSQL & "values"
                strSQL = strSQL & "(" & lblidSede.Value & "," & lblidAttivita.Value & ", 1,'" & Session("Utente") & "', getdate()" & ") "
                myCommand.CommandText = strSQL
                myCommand.ExecuteNonQuery()
            Else
                ChiudiDataReader(dataReader)
            End If
            myTransaction.Commit()
            ChiudiDataReader(dataReader)
            lblConferma.Text = "Inserimento avvenuto con successo."
            isUpdate.Value = "1" ' imposto il flag per la modifica
        Catch ee As Exception
            lblMessaggi.Text = ee.Message.ToString
            ChiudiDataReader(dataReader)
            myTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))

        End Try
        myTransaction.Dispose()
    End Sub

    Private Sub Modifica()
        Dim dtrlocal As SqlClient.SqlDataReader
        'command che eseguirà le operazione sul DB
        Dim myCommand As New System.Data.SqlClient.SqlCommand
        Dim intPostiVittoAlloggio As Integer
        Dim intPostiNovittoNoAlloggio As Integer
        Dim intPostiVitto As Integer
        Dim Tipologia As String
        Dim intPostiFami As Integer
        Dim intPostiGMO As Integer
        Dim transfrontaliero As Integer
        Dim esteroue As Boolean
        Dim intIdTipoProgetto As Integer

        '1=Nazionale, 2=Estero, 3=Bando Straordinario
        ChiudiDataReader(dataReader)

        'verifico il transfrontaliero
        strSQL = "Select  isnull(transfrontaliero,0) as transfrontaliero, isnull(esteroue,0) as esteroue, idtipoprogetto " & _
                " from attività " & _
                "Where Idattività=" & lblidAttivita.Value & " "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dataReader.HasRows = True Then
            dataReader.Read()
            transfrontaliero = dataReader("transfrontaliero")
            esteroue = dataReader("EsteroUE")
            intIdTipoProgetto = dataReader("idtipoprogetto")
        End If
        ChiudiDataReader(dataReader)

        If Not blnProgNazionale Then
            If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                lblMessaggi.Text = "Sui progetti esteri &#232; obbligatorio fornire il vitto e alloggio."
                ChiudiDataReader(dataReader)
                Exit Sub
            End If
        End If

        If blnProgNazionale And esteroue And transfrontaliero = 0 Then
            If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                lblMessaggi.Text = "Sui progetti che prevedono un periodo all'estero nell'Unione Europea e' obbligatorio fornire il vitto e alloggio."
                Exit Sub
            End If
        End If

        If blnProgNazionale And esteroue And transfrontaliero = 1 Then
            If txtNoVittoNoAlloggio.Text <> "0" Then
                lblMessaggi.Text = "Per i progetti in territorio transfrontaliero e' obbligatorio fornire il vitto."
                Exit Sub
            End If
        End If
        'Fine SANDOKAN
        'il 09/06/2014 da simona cordella gestione campi CognomeRiferimento,NomeRiferimento e Città
        'verifico se la sede è estera

        strSQL = "Select  comuni.ComuneNazionale " & _
                " from entisedi " & _
                "inner join entisediattuazioni on (entisedi.identesede=entisediattuazioni.identesede) " & _
                "inner join comuni on  (comuni.idcomune=entisedi.idcomune) " & _
                "Where entisediattuazioni.IdEnteSedeAttuazione='" & lblidsedeattuazione.Value & "' "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dataReader.HasRows = True Then
            dataReader.Read()
            If dataReader("ComuneNazionale") = 0 And Not blnProgNazionale Then


                If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                    lblMessaggi.Text = "Sulle sedi all'estero e' obbligatorio fornire il vitto e alloggio."
                    ChiudiDataReader(dataReader)
                    Exit Sub
                End If

                If txtDataNascita.Visible Then
                    If Trim(txtCognomeRif.Text) = "" Or Trim(txtNomeRif.Text) = "" Or Trim(txtCitta.Text) = "" Or txtDataNascita.Text = "" Then
                        lblMessaggi.Text = "&#200; necessario compilare tutti i campi Cognome, Nome, Data Nascita e Città."
                        ChiudiDataReader(dataReader)
                        Exit Sub
                    End If
                Else
                    If Trim(txtCognomeRif.Text) = "" Or Trim(txtNomeRif.Text) = "" Or Trim(txtCitta.Text) = "" Then
                        lblMessaggi.Text = "&#200; necessario compilare tutti i campi Cognome, Nome e Città."
                        ChiudiDataReader(dataReader)
                        Exit Sub
                    End If
                End If

            End If
            If dataReader("ComuneNazionale") = 0 And blnProgNazionale And esteroue Then

                If txtDataNascita.Visible Then
                    If Trim(txtCognomeRif.Text) = "" Or Trim(txtNomeRif.Text) = "" Or Trim(txtCitta.Text) = "" Or txtDataNascita.Text = "" Then
                        lblMessaggi.Text = "E' necessario compilare tutti i campi Cognome, Nome, Data Nascita e Città."
                        ChiudiDataReader(dataReader)
                        Exit Sub
                    End If
                Else
                    If Trim(txtCognomeRif.Text) = "" Or Trim(txtNomeRif.Text) = "" Or Trim(txtCitta.Text) = "" Then
                        lblMessaggi.Text = "E' necessario compilare tutti i campi Cognome, Nome , Città."
                        ChiudiDataReader(dataReader)
                        Exit Sub
                    End If
                End If


            End If
        End If
        ChiudiDataReader(dataReader)

        strSQL = " Update attivitàentisediattuazione set NumeroPostiNoVittoNoAlloggio=" & txtNoVittoNoAlloggio.Text & ", "
        strSQL &= " NumeroPostiVittoAlloggio=" & txtVittoAlloggio.Text & ", "
        strSQL &= " NumeroPostiVitto=" & txtvitto.Text & " ,CognomeRiferimento='" & txtCognomeRif.Text.Replace("'", "''") & "', "
        strSQL &= " IDEnteSedeAttuazioneSecondaria=" & IIf(txtCodSedeSecondaria.Text = "", "null", "'" & txtCodSedeSecondaria.Text & "'") & ", "
        strSQL &= " NomeRiferimento='" & txtNomeRif.Text.Replace("'", "''") & "',Città='" & txtCitta.Text.Replace("'", "''") & "', "
        strSQL &= " DataNascitaRiferimento=" & IIf(txtDataNascita.Text = "", "null", "'" & txtDataNascita.Text & "'") & ", "
        strSQL &= " UsernameStato='" & Session("Utente") & "' , DataUltimoStato= getDate(), "
        strSQL &= " NumeroPostiFami=" & TxtNumeroPostiFami.Text & ", "
        strSQL &= " NumeroPostiGMO=" & IIf(divGMO.Visible, TxtNumeroPostiGMO.Text, "0") & " "
        strSQL &= " WHERE (idattività = " & lblidAttivita.Value & " And identesedeattuazione = " & lblidsedeattuazione.Value & ") "
        commandSQL = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
        ChiudiDataReader(dtrlocal)


        Dim newID As String = Request.QueryString("idattES")
        Dim idattivita As String = Request.QueryString("IdAttivita")
        Dim identesedeattuazione As String = Request.QueryString("IdSedeAttuazione")
        strSQL = "insert into Cronologiaattivitàentisediattuazione " & _
           "(idAttivitàentesedeattuazione,idattività,identesedeattuazione,NumeroPostiNoVittoNoAlloggio,NumeroPostiVittoAlloggio,NumeroPostiVitto,CognomeRiferimento, NomeRiferimento,Città, DataNascitaRiferimento, UsernameStato,DataUltimoStato,NumeroPostiFami,numeropostiGMO)" & _
           " values(" & newID & "," & idattivita & "," & identesedeattuazione & "," & txtNoVittoNoAlloggio.Text & "," & txtVittoAlloggio.Text & "," & txtvitto.Text & ",'" & txtCognomeRif.Text.Replace("'", "''") & "','" & txtNomeRif.Text.Replace("'", "''") & "','" & txtCitta.Text.Replace("'", "''") & "'," & IIf(txtDataNascita.Text = "", "null", "'" & txtDataNascita.Text & "'") & ",'" & Session("Utente") & "', getdate()" & "," & TxtNumeroPostiFami.Text & "," & IIf(divGMO.Visible, TxtNumeroPostiGMO.Text, "0") & ") "
        myCommand.Connection = Session("conn")
        myCommand.CommandText = strSQL
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()
        ChiudiDataReader(dtrlocal)


        'se è nazionale 
        '1nazionale
        '2estero
        strSQL = "SELECT SUM(a.NumeroPostiNoVittoNoAlloggio) AS NoVittoNoAloggio, "
        strSQL = strSQL & "SUM(a.NumeroPostiVittoAlloggio) AS VittoAloggio, "
        strSQL = strSQL & "SUM(a.NumeroPostiVitto) AS Vitto, "
        strSQL = strSQL & "SUM(a.NumeroPostiFami) AS NumeroPostiFami ,"
        strSQL = strSQL & "SUM(a.NumeroPostiGMO) AS NumeroPostiGMO "
        strSQL = strSQL & "FROM attivitàentisediattuazione a "
        strSQL = strSQL & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
        strSQL = strSQL & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
        strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
        strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
        strSQL = strSQL & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
        strSQL = strSQL & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
        strSQL = strSQL & "WHERE (a.IDAttività='" & lblidAttivita.Value & "')  "
        If blnProgNazionale = True Then
            'Area - Settore
            'strSQL = "select sum(a.NumeroPostiNovittoNoAlloggio) as NoVittoNoAloggio, "
            'strSQL = strSQL & "sum(a.NumeroPostivittoAlloggio) as VittoAloggio, "
            'strSQL = strSQL & "sum(a.NumeroPostivitto) as Vitto "
            'strSQL = strSQL & "from attivitàentisediattuazione as a "
            'strSQL = strSQL & "where a.idattività='" & lblidAttivita.Value & "'"
            strSQL = strSQL & " AND (nazioni.NazioneBase = 1)"
        Else
            'strSQL = "SELECT SUM(a.NumeroPostiNoVittoNoAlloggio) AS NoVittoNoAloggio, "
            'strSQL = strSQL & "SUM(a.NumeroPostiVittoAlloggio) AS VittoAloggio, "
            'strSQL = strSQL & "SUM(a.NumeroPostiVitto) AS Vitto "
            'strSQL = strSQL & "FROM attivitàentisediattuazione a "
            'strSQL = strSQL & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
            'strSQL = strSQL & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
            'strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
            'strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
            'strSQL = strSQL & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
            'strSQL = strSQL & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
            'strsql = strsql & "WHERE (a.IDAttività='" & lblidAttivita.Value & "') AND (nazioni.NazioneBase = 0)"
            strSQL = strSQL & " AND (nazioni.NazioneBase = 0)"
        End If
        myCommand.CommandText = strSQL
        dtrlocal = myCommand.ExecuteReader
        dtrlocal.Read()
        If dtrlocal.HasRows = True Then

            If IsDBNull(dtrlocal("NoVittoNoAloggio")) = False Then
                intPostiNovittoNoAlloggio = CInt(dtrlocal("NoVittoNoAloggio"))
            Else
                intPostiNovittoNoAlloggio = 0
            End If
            If IsDBNull(dtrlocal("VittoAloggio")) = False Then
                intPostiVittoAlloggio = CInt(dtrlocal("VittoAloggio"))
            Else
                intPostiVittoAlloggio = 0
            End If
            If IsDBNull(dtrlocal("Vitto")) = False Then
                intPostiVitto = CInt(dtrlocal("Vitto"))
            Else
                intPostiVitto = 0
            End If
            If IsDBNull(dtrlocal("NumeroPostiFami")) = False Then
                intPostiFami = CInt(dtrlocal("NumeroPostiFami"))
            Else
                intPostiFami = 0
            End If
            If IsDBNull(dtrlocal("NumeroPostiGMO")) = False Then
                intPostiGMO = CInt(dtrlocal("NumeroPostiGMO"))
            Else
                intPostiGMO = 0
            End If
        End If
        ChiudiDataReader(dtrlocal)
        'fine blocco relativo al caricamento dei posti

        strSQL = "Update attività set NumeroPostiNoVittoNoAlloggio=" & intPostiNovittoNoAlloggio & ", "
        strSQL = strSQL & "NumeroPostiVittoAlloggio=" & intPostiVittoAlloggio & ", "
        strSQL = strSQL & "NumeroPostiVitto=" & intPostiVitto & ", "
        strSQL = strSQL & "NumeroPostiFami=" & intPostiFami & " "
        If intIdTipoProgetto >= 11 Or CInt(Request.QueryString("Nazionale")) = 3 Or CInt(Request.QueryString("Nazionale")) = 4 Then
            strSQL = strSQL & ",NumeroGiovaniMinoriOpportunità=" & intPostiGMO & " "
        End If
        strSQL = strSQL & "where idattività=" & CInt(lblidAttivita.Value)
        commandSQL = ClsServer.EseguiSqlClient(strSQL, Session("conn"))

        ChiudiDataReader(dataReader)

        Dim myCommandSP As New System.Data.SqlClient.SqlCommand
        myCommandSP.CommandText = "SP_PROGRAMMI_AGGIORNA_POSTI_da_progetto"
        myCommandSP.CommandType = CommandType.StoredProcedure

        myCommandSP.Connection = Session("conn")
        myCommandSP.Parameters.AddWithValue("@IdAttività", lblidAttivita.Value)
        myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
        myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
        myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
        myCommandSP.Parameters("@messaggio").Size = 1000
        myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
        myCommandSP.ExecuteNonQuery()
        If txtCodSedeSecondaria.Text <> "" Then
            strSQL = "Select enti.codiceregione, enti.denominazione, entisedi.indirizzo + '  n°'+ isnull(entisedi.civico,'')  as Indirizzo, " & _
                  " comuni.denominazione as comune,provincie.provincia," & _
                  " entisediattuazioni.denominazione as sedeAttuazione " & _
                  " from entisedi " & _
                  " inner join entisediattuazioni on (entisedi.identesede=entisediattuazioni.identesede)  " & _
                  " inner join comuni on  (comuni.idcomune=entisedi.idcomune)" & _
                  " inner join provincie on (provincie.idprovincia=comuni.idprovincia)" & _
                  " inner join enti on (entisedi.idente=enti.idente)" & _
                  " Where entisediattuazioni.identesedeattuazione=" & txtCodSedeSecondaria.Text & " "
            dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
            dataReader.Read()

            lblInfoSedeSecondaria.Text = "[" & dataReader("codiceregione") & "] " & _
                                             dataReader("comune") & " - " & _
                                              dataReader("indirizzo")
            ChiudiDataReader(dataReader)
        End If

        lblConferma.Text = "Aggiornamento avvenuto con successo."
    End Sub

    Private Sub Modifica_OLD()
        Dim dtrlocal As SqlClient.SqlDataReader
        'command che eseguirà le operazione sul DB
        Dim myCommand As New System.Data.SqlClient.SqlCommand
        Dim intPostiVittoAlloggio As Integer
        Dim intPostiNovittoNoAlloggio As Integer
        Dim intPostiVitto As Integer
        Dim Tipologia As String
        Dim intPostiFami As Integer

        '1=Nazionale, 2=Estero, 3=Bando Straordinario
        ChiudiDataReader(dtrlocal)
        strSQL = "select Tipologia from enti "
        strSQL = strSQL & "where IdEnte=" & Session("idente")
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))
        If dataReader.HasRows = True Then
            dataReader.Read()
            Tipologia = dataReader("Tipologia")
        End If
        ChiudiDataReader(dataReader)

        If Not blnProgNazionale And Tipologia <> "Amministrazione Statale" Then
            If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                lblMessaggi.Text = "Sui progetti esteri &#232; obbligatorio fornire il vitto e alloggio."
                ChiudiDataReader(dataReader)
                Exit Sub
            End If
        End If
        'Fine SANDOKAN
        'il 09/06/2014 da simona cordella gestione campi CognomeRiferimento,NomeRiferimento e Città
        'verifico se la sede è estera

        strSQL = "Select  comuni.ComuneNazionale " & _
                " from entisedi " & _
                "inner join entisediattuazioni on (entisedi.identesede=entisediattuazioni.identesede) " & _
                "inner join comuni on  (comuni.idcomune=entisedi.idcomune) " & _
                "Where entisediattuazioni.IdEnteSedeAttuazione='" & lblidsedeattuazione.Value & "' "
        dataReader = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dataReader.HasRows = True Then
            dataReader.Read()
            If dataReader("ComuneNazionale") = 0 And Not blnProgNazionale Then
                ChiudiDataReader(dataReader)
                If Tipologia <> "Amministrazione Statale" Then
                    If txtNoVittoNoAlloggio.Text <> "0" Or txtvitto.Text <> "0" Then
                        lblMessaggi.Text = "Sulle sedi all'estero e' obbligatorio fornire il vitto e alloggio."
                        Exit Sub
                    End If
                End If

                If Trim(txtCognomeRif.Text) = "" Or Trim(txtNomeRif.Text) = "" Or Trim(txtCitta.Text) = "" Then
                    lblMessaggi.Text = "&#200; necessario compilare tutti i campi Cognome, Nome ,Città."
                    Exit Sub
                End If
            End If
        End If
        ChiudiDataReader(dataReader)

        strSQL = " Update attivitàentisediattuazione set NumeroPostiNoVittoNoAlloggio=" & txtNoVittoNoAlloggio.Text & ", "
        strSQL &= " NumeroPostiVittoAlloggio=" & txtVittoAlloggio.Text & ", "
        strSQL &= " NumeroPostiVitto=" & txtvitto.Text & " ,CognomeRiferimento='" & txtCognomeRif.Text.Replace("'", "''") & "', "
        strSQL &= " IDEnteSedeAttuazioneSecondaria=" & IIf(txtDataNascita.Text = "", "null", "'" & txtDataNascita.Text & "'") & ", "
        strSQL &= " NomeRiferimento='" & txtNomeRif.Text.Replace("'", "''") & "',Città='" & txtCitta.Text.Replace("'", "''") & "', "
        strSQL &= " DataNascitaRiferimento='" & txtDataNascita.Text & "', "
        strSQL &= " UsernameStato='" & Session("Utente") & "' , DataUltimoStato= getDate(), "
        strSQL &= " NumeroPostiFami=" & TxtNumeroPostiFami.Text & " "
        strSQL &= " WHERE (idattività = " & lblidAttivita.Value & " And identesedeattuazione = " & lblidsedeattuazione.Value & ") "
        commandSQL = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
        ChiudiDataReader(dtrlocal)


        Dim newID As String = Request.QueryString("idattES")
        Dim idattivita As String = Request.QueryString("IdAttivita")
        Dim identesedeattuazione As String = Request.QueryString("IdSedeAttuazione")
        strSQL = "insert into Cronologiaattivitàentisediattuazione " & _
           "(idAttivitàentesedeattuazione,idattività,identesedeattuazione,NumeroPostiNoVittoNoAlloggio,NumeroPostiVittoAlloggio,NumeroPostiVitto,CognomeRiferimento, NomeRiferimento,Città, DataNascitaRiferimento ,UsernameStato,DataUltimoStato,NumeroPostiFami)" & _
           " values(" & newID & "," & idattivita & "," & identesedeattuazione & "," & txtNoVittoNoAlloggio.Text & "," & txtVittoAlloggio.Text & "," & txtvitto.Text & ",'" & txtCognomeRif.Text.Replace("'", "''") & "','" & txtNomeRif.Text.Replace("'", "''") & "','" & txtCitta.Text.Replace("'", "''") & "','" & txtDataNascita.Text & "','" & Session("Utente") & "', getdate()" & "," & TxtNumeroPostiFami.Text & ") "
        myCommand.Connection = Session("conn")
        myCommand.CommandText = strSQL
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()
        ChiudiDataReader(dtrlocal)


        'se è nazionale 
        '1nazionale
        '2estero
        strSQL = "SELECT SUM(a.NumeroPostiNoVittoNoAlloggio) AS NoVittoNoAloggio, "
        strSQL = strSQL & "SUM(a.NumeroPostiVittoAlloggio) AS VittoAloggio, "
        strSQL = strSQL & "SUM(a.NumeroPostiVitto) AS Vitto, "
        strSQL = strSQL & "SUM(a.NumeroPostiFami) AS NumeroPostiFami "
        strSQL = strSQL & "FROM attivitàentisediattuazione a "
        strSQL = strSQL & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
        strSQL = strSQL & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
        strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
        strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
        strSQL = strSQL & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
        strSQL = strSQL & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
        strSQL = strSQL & "WHERE (a.IDAttività='" & lblidAttivita.Value & "')  "
        If blnProgNazionale = True Then
            'Area - Settore
            'strSQL = "select sum(a.NumeroPostiNovittoNoAlloggio) as NoVittoNoAloggio, "
            'strSQL = strSQL & "sum(a.NumeroPostivittoAlloggio) as VittoAloggio, "
            'strSQL = strSQL & "sum(a.NumeroPostivitto) as Vitto "
            'strSQL = strSQL & "from attivitàentisediattuazione as a "
            'strSQL = strSQL & "where a.idattività='" & lblidAttivita.Value & "'"
            strSQL = strSQL & " AND (nazioni.NazioneBase = 1)"
        Else
            'strSQL = "SELECT SUM(a.NumeroPostiNoVittoNoAlloggio) AS NoVittoNoAloggio, "
            'strSQL = strSQL & "SUM(a.NumeroPostiVittoAlloggio) AS VittoAloggio, "
            'strSQL = strSQL & "SUM(a.NumeroPostiVitto) AS Vitto "
            'strSQL = strSQL & "FROM attivitàentisediattuazione a "
            'strSQL = strSQL & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
            'strSQL = strSQL & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
            'strSQL = strSQL & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
            'strSQL = strSQL & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
            'strSQL = strSQL & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
            'strSQL = strSQL & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
            'strsql = strsql & "WHERE (a.IDAttività='" & lblidAttivita.Value & "') AND (nazioni.NazioneBase = 0)"
            strSQL = strSQL & " AND (nazioni.NazioneBase = 0)"
        End If
        myCommand.CommandText = strSQL
        dtrlocal = myCommand.ExecuteReader
        dtrlocal.Read()
        If dtrlocal.HasRows = True Then

            If IsDBNull(dtrlocal("NoVittoNoAloggio")) = False Then
                intPostiNovittoNoAlloggio = CInt(dtrlocal("NoVittoNoAloggio"))
            Else
                intPostiNovittoNoAlloggio = 0
            End If
            If IsDBNull(dtrlocal("VittoAloggio")) = False Then
                intPostiVittoAlloggio = CInt(dtrlocal("VittoAloggio"))
            Else
                intPostiVittoAlloggio = 0
            End If
            If IsDBNull(dtrlocal("Vitto")) = False Then
                intPostiVitto = CInt(dtrlocal("Vitto"))
            Else
                intPostiVitto = 0
            End If
            If IsDBNull(dtrlocal("NumeroPostiFami")) = False Then
                intPostiFami = CInt(dtrlocal("NumeroPostiFami"))
            Else
                intPostiFami = 0
            End If
        End If
        ChiudiDataReader(dtrlocal)
        'fine blocco relativo al caricamento dei posti

        strSQL = "Update attività set NumeroPostiNoVittoNoAlloggio=" & intPostiNovittoNoAlloggio & ", "
        strSQL = strSQL & "NumeroPostiVittoAlloggio=" & intPostiVittoAlloggio & ", "
        strSQL = strSQL & "NumeroPostiVitto=" & intPostiVitto & ", "
        strSQL = strSQL & "NumeroPostiFami=" & intPostiFami & " "
        strSQL = strSQL & "where idattività=" & CInt(lblidAttivita.Value)
        commandSQL = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
        lblConferma.Text = "Aggiornamento avvenuto con successo."
    End Sub



    Protected Sub imgdettProg_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgdettProg.Click
        Response.Redirect("WebElencoVisProgetti.aspx?Torna=SedeProgetto&EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&idTipoProg=" & Request.QueryString("idTipoProg") & "&idSede=" & Request.QueryString("idSede") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&idattES=" & Request.QueryString("idattES") & "&blnVisualizzaVolontari=" & Request.QueryString("blnVisualizzaVolontari") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub

    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WebGestioneSediProgetto.aspx?EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&idTipoProg=" & Request.QueryString("idTipoProg") & "&idSede=" & Request.QueryString("idSede") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&idattES=" & Request.QueryString("idattES") & "&blnVisualizzaVolontari=" & Request.QueryString("blnVisualizzaVolontari") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub

    Private Function Store_PROGETTI_ConguenzaGeografica(ByVal IdAttivita As Integer, ByVal IDEnteSedeattuazione As Integer, ByRef Motivazione As String) As Boolean
        '** Aggiunto da simona cordella
        '** Funzionalità: Annulla la richiesta di inizo asdeguamento .
        Dim Esito As Integer
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_PROGETTI_CONGRUENZA_GEOGRAFICA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = IdAttivita
            SqlCmd.Parameters.Add("@IDEnteSedeattuazione", SqlDbType.Int).Value = IDEnteSedeattuazione

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettu ato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@Motivazione", SqlDbType.VarChar)
            SqlCmd.Parameters("@Motivazione").Size = 1000
            SqlCmd.Parameters("@Motivazione").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            Motivazione = SqlCmd.Parameters("@Motivazione").Value()
            Esito = SqlCmd.Parameters("@Esito").Value()
            Return Esito
        Catch ex As Exception
            lblMessaggi.Text = ex.Message
            Return False
        End Try
    End Function

    Private Function Store_ConguenzaSedeSecondaria(ByVal IdAttivita As Integer, ByVal IDEnteSedeattuazione As Integer, ByVal IDEnteSedeattuazioneSecondaria As Integer, ByRef Messaggio As String) As Boolean
        '** Aggiunto da simona cordella
        '** Funzionalità: Annulla la richiesta di inizo asdeguamento .
        Dim Esito As Integer
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_PROGETTI_VERIFICA_SEDE_SECONDARIA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdAttività", SqlDbType.Int).Value = IdAttivita
            SqlCmd.Parameters.Add("@IdEnteSedeAttuazione", SqlDbType.Int).Value = IDEnteSedeattuazione
            SqlCmd.Parameters.Add("@IdEnteSedeAttuazioneSecondaria", SqlDbType.Int).Value = txtCodSedeSecondaria.Text



            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettu ato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@Messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@Messaggio").Size = 1000
            SqlCmd.Parameters("@Messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            Messaggio = SqlCmd.Parameters("@Messaggio").Value()
            Esito = SqlCmd.Parameters("@Esito").Value()
            Return Esito
        Catch ex As Exception
            lblMessaggi.Text = ex.Message
            Return False
        End Try
    End Function

  
End Class
