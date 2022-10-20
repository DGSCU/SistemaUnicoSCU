Imports System.IO
Imports System.Drawing
Imports System.Data.SqlClient
Public Class WfrmAssociaVolontari
    Inherits System.Web.UI.Page
    Dim cmdCommand As SqlClient.SqlCommand
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim strsql As String
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim strSql1 As String
    Dim cmdGenerico As SqlClient.SqlCommand
    Dim mesg As String
    Dim PROGETTO_GARANZIA_GIOVANI As String = "4"
    ' Public Shared messaggio As String
    Private Sub WfrmAssociaVolontari_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If


        If Request.QueryString("idattivita") <> "" Then

           
            If abilitalink(Request.QueryString("idattivitaSedeAssegnazione")) = "SI" Then
                imgCronoRiattivazione.Visible = True
                riattivazione.Visible = True
            Else
                imgCronoRiattivazione.Visible = False
                riattivazione.Visible = False
            End If

            If abilitalinkRimodulazione(Request.QueryString("idattivitaSedeAssegnazione")) = "SI" Then
                imgCronoRimodulazione.Visible = True
                riattivazione.Visible = True
            Else
                imgCronoRimodulazione.Visible = False
                riattivazione.Visible = False
            End If

            Dim esitogmo As String = String.Empty
            esitogmo = SegnalazioneCoperturaGMO(Request.QueryString("idattivitaSedeAssegnazione"), Session("Utente"))
            If esitogmo = "" Then
                alertgmo.Visible = False
            Else
                alertgmo.Visible = True
                alertgmo.Text = esitogmo
            End If


            If abilitaFCGG(Request.QueryString("idattivitaSedeAssegnazione"), Session("Utente")) = 0 Then
                CmdFCGG.Visible = False
            Else
                CmdFCGG.Visible = True
            End If

            'IDENTE  Session("IdEnte")
            'CODICEENTE  NZ Session("codiceregione")  or Session("txtCodEnte")

            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("idattivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then

                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
            Else
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                Response.Redirect("wfrmAnomaliaDati.aspx")

            End If

        End If


        If Not IsNothing(Request.QueryString("presenta")) Then
            lblpresentata.Text = Request.QueryString("presenta")
        End If
        If Not IsNothing(Request.QueryString("identesede")) Then
            lblidSede.Text = Request.QueryString("identesede")
        End If
        lblMessaggioAlert.Text = ""
        'imgAlert.Visible = False
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            lblGenera.Visible = True
            imgGeneraDoc.Visible = True
            rigaLegenda.Visible = True
            '------------------------------ANTONELLOtblLegenda.Visible = True
        Else
            '------------------------------ANTONELLOtblLegenda.Visible = False
            'nascondo le note

            'RigaNote.Visible = False  'adc
            lblNote.Visible = False
            cmdInsNote.Visible = False
            txtNote.Visible = False

            rigaLegenda.Visible = False
            lblNote.Visible = False
            txtNote.Visible = False
            cmdInsNote.Visible = False
        End If

        dgRisultatoRicerca.Columns(29).Visible = ClsUtility.ForzaFascicoloInformaticoVolontari(Session("Utente"), Session("conn"))

        If IsPostBack = False Then
            If Request.QueryString("idattivita") <> "" Then

                If Session("TipoUtente") = "U" Then
                    ConfiguraPostBackSezioni()
                    Dim lblprogramma1 As New DataTable
                    lblprogramma1 = lblprogramma(Request.QueryString("idattivita"))

                    If lblprogramma1.Rows.Count > 0 Then
                        Label9.Text = lblprogramma1.Columns(0).ToString
                        Label10.Text = lblprogramma1.Columns(0).Table(0).Item(0).ToString
                        Label13.Text = lblprogramma1.Columns(1).ToString
                        Label14.Text = lblprogramma1.Columns(1).Table(0).Item(1).ToString
                        Label15.Text = lblprogramma1.Columns(2).ToString
                        Label16.Text = lblprogramma1.Columns(2).Table(0).Item(2).ToString
                        Label17.Text = lblprogramma1.Columns(3).ToString
                        Label18.Text = lblprogramma1.Columns(3).Table(0).Item(3).ToString


                        Call ElencoProgettiProgramma()
                    Else
                        flsProgramma.Visible = False

                    End If

                Else
                    flsProgramma.Visible = False
                End If
            End If

            'abilito i campi del fascicolo solo per l'UNSC
            'agg da sc il 10/02/2009
            piker.Value = 1
            If Session("TipoUtente") = "U" Then
                LblNumFascicolo.Visible = True
                txtNumeroFascicoloinVisione.Visible = True
                LblDescFascicolo.Visible = True
                txtdescrizionefascicolo.Visible = True
                LblNumProtocollo.Visible = True
                TxtNumProtocollo.Visible = True
                'cmdScSelProtocolloLV.Visible = True
                'cmdScAllegatiLV.Visible = True
                'cmdNuovoFasciocloLV.Visible = True
                LblDataProtocollo.Visible = True
                TxtDataProtocollo.Visible = True
                ImgSalvaProt.Visible = True
                imgValutazioneProg.Visible = True

                'mauro lanna
                strsql = "select * from CronologiaEntiDocumenti where idente = " & CInt(Session("IdEnte")) & _
                " and tipodocumento = 2 and IDAttivitàSedeAssegnazione = " & Request.QueryString("IDAttivitasedeAssegnazione")
                Try
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If

                    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    If dtrGenerico.HasRows = True Then
                        dtrGenerico.Read()

                        If Not IsDBNull(dtrGenerico("dataprot")) Then
                            TxtDataProtocollo.Text = dtrGenerico("dataprot")
                            TxtNumProtocollo.Text = dtrGenerico("nprot")
                            cmdScSelProtocolloLV.Visible = True
                            cmdScAllegatiLV.Visible = True
                            cmdNuovoFasciocloLV.Visible = True
                            ImgSalvaProt.Visible = True
                        End If

                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If

                    End If
                    '*******************************************************************************
                Catch

                End Try
                '**

                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If


                'If Request.QueryString("CodiceFascicolo") <> "" Then
                'mauro lanna
                TxtNumFascicolo.Value = Request.QueryString("NumeroFascicolo")
                txtNumeroFascicoloinVisione.Text = Request.QueryString("CodiceFascicolo")
                txtdescrizionefascicolo.Text = Request.QueryString("DescFascicolo")
                'End If
            End If
            PrimioCaricamento()
            If Session("TipoUtente") = "U" Then
                imgAvvisoSede.Visible = False
                If ClsUtility.ControlloRichiestaModificaSede(Request.QueryString("identesede"), Session("conn")) = True Then
                    imgAvvisoSede.Visible = True
                    divSede.Attributes("class") = "divErrore"
                End If
            Else
                dgRisultatoRicerca.Columns(33).Visible = False

            End If
        Else
            'controllo se sto cercando di cancellare un volontario
            If Request.Form("txtIdVolCanc") <> Nothing Then
                If Request.Form("txtStatoVol") <> Nothing Then
                    Call CancellaVolontarioGrad(Request.Form("txtIdVolCanc"), Request.Form("txtStatoVol"))
                Else
                    Call CancellaVolontarioGrad(Request.Form("txtIdVolCanc"), txtStatoVol.Value)
                End If

            End If
        End If
    End Sub

    Sub PrimioCaricamento()
        If Session("TipoUtente") <> "E" Then

            'modifica realizzata da Jon J.Crokket ("non sono venuto per salvare lui da voi, ma voi da lui") Colonnello Trautman
            'venerdì 05 ottobre 2007
            'vado a vedere il campo SegnalaGraduatorieVolontari
            'False = 
            'True = Coloro la riga

            strsql = "SELECT SegnalaGraduatorieVolontari FROM attività WHERE idattività=" & Request.QueryString("idattivita")
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                If dtrGenerico("SegnalaGraduatorieVolontari") = False Then
                    imgSegnalaOk.ImageUrl = "images/vistabuona_small.png"
                    lblSegnalazione.Text = "Segnala Progetto"
                Else
                    imgSegnalaOk.ImageUrl = "images/vistacattiva_small.png"
                    lblSegnalazione.Text = "Ripristina Progetto"
                End If
            End If

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If



            'Modifica del 17/01/2006 di Amilcare Paolella ***************************
            'Ricavo le informazioni dell'utente per valorizzare la path dei documenti
            strsql = "SELECT RegioniCompetenze.CodiceRegioneCompetenza AS Path FROM UtentiUNSC INNER JOIN " & _
                     "RegioniCompetenze ON UtentiUNSC.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                     "WHERE UtentiUNSC.UserName ='" & Session("Utente") & "'"
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrGenerico.Read = True Then
                Session("Path") = dtrGenerico("Path")
                Session("Path") &= "/"
                dtrGenerico.Close()
                dtrGenerico = Nothing
            Else
                'Non c'è corrispondenza è successo qualcosa di inusuale;esco e torno alla logon
                dtrGenerico.Close()
                dtrGenerico = Nothing
                Response.Redirect("LogOn.aspx")
            End If
        Else
          
            imgSegnalaOk.Visible = False
            lblSegnalazione.Visible = False
            imgRecuperoOk.Visible = False
            lblRecupero.Visible = False
            '------------------------------ANTONELLO DIV Segnalazione.Visible = False
            riga1.Visible = False
            riga2.Visible = False
            'riga3.Visible = False
            dtgStoricoNote.Visible = False
            RigaNoteIns.Visible = False
            RigaGrigliaNote.Visible = False
            '--------------------------------------------------------------------------
        End If

        'visualizza date diff partenza
        '[SP_GRADUATORIA_VISUALIZZA_PARTENZA_DIFFERITA]

        ControlloVisualizzaDateDiff(Request.QueryString("IDAttivitasedeAssegnazione"))

        If strAbilitaPulsante.Value = "SI" Then
            ImgPartenzaDiff.Visible = True
            lblPartenzaDifferita.Visible = True
        Else
            ImgPartenzaDiff.Visible = False
            lblPartenzaDifferita.Visible = False
        End If

        If strAbilitaDiv.Value = "SI" Then
            PartDiff.Visible = True
        Else
            PartDiff.Visible = False
        End If

        'Realizzata da Alessandra Taballione il 20/10/2004
        'popolamento della maschera e della dataGrid con i volontari Associati 
        'alla sede di attuazione 
        'Aggiunto da Jonathan il 05.10.2005 aggiunta voce dello username che ha confermato la graduatoria
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Call CaricaUserName(Request.QueryString("IDAttivitasedeAssegnazione"))
        Else
            lblVisConfermataDa.Visible = False
            lblConfermataDa.Visible = False
        End If
        Popolamaschera()
        CaricaDataGrid(dgRisultatoRicerca)
        If Not BandoDol() Then
            dgRisultatoRicerca.Columns(33).Visible = False
            Call AbilitaPulsanteCancella2()
        End If

        'AUTORE: TESTA GUIDO    DATA: 13/09/2006
        'Abilito il pulsante per cancellare la graduatoria solo se l'utente ha le abilitazioni
        If Not BandoDol() Then
            Call AbilitaPulsanteCancella()
        End If


        '****************************************************************************************
        Call AbilitaRecuperoPosti()

        If dgRisultatoRicerca.Items.Count <> 0 Then
            hdd_CountGriglia.Value = dgRisultatoRicerca.Items.Count
        Else
            hdd_CountGriglia.Value = 0
        End If
        Select Case lblpresentata.Text
            Case "1" 'Presentazione 
                lblTitolo.Text = "Presentazione Graduatoria"
                ChekBloccaIdonei()
                BloccaDataInizio()
                If lblStatoGraduatoria.Text = "Registrata" Then
                    cmdSalva.Visible = False

                    cmdAggiungi.Visible = False
                    cmdRespingi.Visible = False
                    cmdConferma.Visible = True
                End If
                If lblStatoGraduatoria.Text = "Presentata" Or lblStatoGraduatoria.Text = "Respinta" Or lblStatoGraduatoria.Text = "Confermata" Then
                    cmdSalva.Visible = False

                    cmdAggiungi.Visible = False
                    cmdConferma.Visible = False
                    cmdRespingi.Visible = False
                    ChekBloccaIdonei()
                    dgRisultatoRicerca.Columns(18).Visible = False
                End If
            Case "2" 'Associazione Volontari da Presentazione Istanza
                lblTitolo.Text = "Associazione Volontari"
                If lblStatoGraduatoria.Text = "Registrata" Then
                    cmdSalva.Visible = True
                    If Not BandoDol() Then
                        cmdAggiungi.Visible = True
                    End If

                    cmdConferma.Visible = False
                    cmdRespingi.Visible = False
                End If
                If lblStatoGraduatoria.Text = "Presentata" Or lblStatoGraduatoria.Text = "Respinta" Or lblStatoGraduatoria.Text = "Confermata" Or lblStatoGraduatoria.Text = "Mancante" Then
                    cmdSalva.Visible = False

                    cmdAggiungi.Visible = False
                    cmdConferma.Visible = False
                    cmdRespingi.Visible = False
                    ChekBloccaIdonei()
                    dgRisultatoRicerca.Columns(18).Visible = False
                End If
            Case "0" 'Associazione Volontari
                lblTitolo.Text = "Associazione Volontari"
                If lblStatoGraduatoria.Text = "Registrata" Then
                    If Not BandoDol() Then
                        cmdSalva.Visible = True 'conferma
                        cmdAggiungi.Visible = True
                    End If

                    cmdConferma.Visible = False
                    cmdRespingi.Visible = False
                End If
                If lblStatoGraduatoria.Text = "Confermata" Or lblStatoGraduatoria.Text = "Presentata" Or lblStatoGraduatoria.Text = "Respinta" Or lblStatoGraduatoria.Text = "Mancante" Then
                    cmdSalva.Visible = False

                    cmdAggiungi.Visible = False
                    cmdConferma.Visible = False
                    cmdRespingi.Visible = False
                    ChekBloccaIdonei()
                    dgRisultatoRicerca.Columns(18).Visible = False
                End If
            Case "3" 'Conferma Graduatoria Volontari
                lblTitolo.Text = "Conferma/Respingi Volontari"
                'Aggiunto da Alessandra Taballione il 25/05/2005
                'Aggiunto da Alessandra Taballione il 09/06/2005
                dgRisultatoRicerca.Columns(23).Visible = True
                'ControlloVolontari()
                If lblStatoGraduatoria.Text = "Respinta" Then

                    cmdAggiungi.Visible = False
                    cmdConferma.Visible = False
                    cmdSalva.Visible = False
                    'cmdSalva.Text = "Conferma"
                    'cmdSalva.ImageUrl = "images/confermared.jpg"
                    cmdSalva.ToolTip = "Conferma Graduatoria"
                    cmdRespingi.Visible = False
                    ChekBloccaIdonei()
                    dgRisultatoRicerca.Columns(18).Visible = False
                    'Aggiunto Da Alessandra Taballione il 19/04/2005
                    BloccaDataInizio()
                    cmdChiudi.Visible = True
                End If
                If lblStatoGraduatoria.Text = "Confermata" Then
                    If Not BandoDol() Then
                        cmdAggiungi.Visible = True
                    End If

                    cmdConferma.Visible = False
                    cmdSalva.Visible = False
                    'cmdSalva.Text = "Conferma"
                    'cmdSalva.ImageUrl = "images/confermared.jpg"
                    cmdSalva.ToolTip = "Conferma Graduatoria"
                    cmdRespingi.Visible = False

                    ChekBloccaIdonei()
                    dgRisultatoRicerca.Columns(18).Visible = False
                    'Aggiunto Da Alessandra Taballione il 19/04/2005
                    BloccaDataInizio()
                    cmdChiudi.Visible = True
                    'aggiunto per gestione recupero posti
                    If txtInizioRecupero.Visible And (Session("TipoUtente") = "U") Then
                        cmdModificaUNSC.Visible = True
                        dgRisultatoRicerca.Columns(18).Visible = False
                    End If
                End If
                If lblStatoGraduatoria.Text = "Presentata" Or lblStatoGraduatoria.Text = "Registrata" Then
                    cmdConferma.Visible = False
                    'If BandoDol() And lblStatoGraduatoria.Text = "Registrata" Then
                    '    cmdSalva.Visible = False
                    'Else
                    '    cmdSalva.Visible = True
                    'End If
                    cmdSalva.Visible = True
                    'cmdSalva.Text = "Conferma"
                    'cmdSalva.ImageUrl = "images/confermared.jpg"
                    cmdSalva.ToolTip = "Conferma Graduatoria"
                    'Antonellone
                    cmdRespingi.Visible = True
                    cmdRespingi.Visible = False
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        If BandoDol() And lblStatoGraduatoria.Text = "Registrata" Then
                            cmdModificaUNSC.Visible = False
                        Else
                            cmdModificaUNSC.Visible = True
                        End If

                        dgRisultatoRicerca.Columns(18).Visible = False
                    End If
                    If (lblStatoGraduatoria.Text = "Registrata" And hdd_CountGriglia.Value = 0) Then
                        txtinizioEff.ReadOnly = True
                        'Image3.Visible = False

                    End If
                    If txtinizioEff.Text <> "" Then
                        piker.Value = 0
                        txtinizioEff.ReadOnly = True

                    End If
                End If
                'Aggiunta Da Alessandra Taballione il 27/05/2005
            Case "4" 'Visualizzazione 
                lblTitolo.Text = "Visualizzazione Graduatoria"
                ChekBloccaIdonei()
                BloccaDataInizio()
                cmdSalva.Visible = False

                cmdAggiungi.Visible = False
                cmdConferma.Visible = False
                cmdRespingi.Visible = False
                ChekBloccaIdonei()
                dgRisultatoRicerca.Columns(18).Visible = False
                dgRisultatoRicerca.Columns(0).Visible = False
        End Select
        If Session("TipoUtente") = "E" Then
            dgRisultatoRicerca.Columns(19).Visible = False
            BloccaDataInizio()
        End If
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            If lblpresentata.Text <> 3 And (lblStatoGraduatoria.Text = "Presentata" Or lblStatoGraduatoria.Text = "Respinta" Or lblStatoGraduatoria.Text = "Confermata") Then
                BloccaDataInizio()
            End If
        End If
        If lblStatoGraduatoria.Text <> "Confermata" And Session("TipoUtente") = "E" Then
            txtinizioEff.Visible = False
            txtFineEff.Visible = False
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        '*******************************************************************************
        'aggiunto da jonastrans il 01/06/2006
        'controllo se l'utente può annullare la conferma

        If lblStatoGraduatoria.Text = "Confermata" Then

            'imgAnnullaConferma.Visible = True

            If txtinizioEff.Text <> "" Then
                If PartDiff.Visible = False Then 'partenza ordinaria
                    If CDate(Session("dataserver")) < CDate(txtinizioEff.Text) Then
                        imgAnnullaConferma.Visible = True
                    Else
                        imgAnnullaConferma.Visible = False
                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        Exit Sub
                    End If
                Else 'partenza differita
                    If CDate(Session("dataserver")) < CDate(txtInizioServizioSede.Text) Then
                        imgAnnullaConferma.Visible = True
                    Else
                        imgAnnullaConferma.Visible = False
                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        Exit Sub
                    End If
                End If

            End If

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
            " VociMenu.IdVoceMenuPadre" & _
            " FROM VociMenu" & _
            " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
            " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If Session("Read") <> "1" Then
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
            " WHERE (VociMenu.descrizione = 'Forza Annulla Conferma Graduatoria')" & _
            " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
            If dtrGenerico.HasRows = True Then
                imgAnnullaConferma.Visible = True
            Else
                imgAnnullaConferma.Visible = False
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                Exit Sub
            End If

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        Else
            imgAnnullaConferma.Visible = False
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            Exit Sub
        End If


    End Sub

    Sub CancellaVolontarioGrad(ByVal idvol As String, ByVal StatoVol As Integer)
        Dim sEsito As String
        If StatoVol = 1 Then
            'cancellazione con store
            sEsito = ClsServer.EseguiStoreCancellaEntita(Request.Form("txtIdVolCanc"), Session("Utente"), Session("conn"))

            If sEsito <> 0 Then
                Response.Write("<script language=""javascript"">" & vbCrLf)
                ' Response.Write("<!--" & vbCrLf)
                Response.Write("alert(""Errore imprevisto durante l'operazione di 'Eliminazione del Volontario'. Contattare l'assistenza Helios/Futuro."");" & vbCrLf)
                'Response.Write("//-->" & vbCrLf)
                Response.Write("</script>" & vbCrLf)
            Else
                Response.Write("<script language=""javascript"">" & vbCrLf)
                ' Response.Write("<!--" & vbCrLf)
                Response.Write("alert(""Cancellazione Effettuata."");" & vbCrLf)
                'Response.Write("//-->" & vbCrLf)
                Response.Write("</script>" & vbCrLf)
                PrimioCaricamento()
            End If
        Else
            Response.Write("<script language=""javascript"">" & vbCrLf)
            ' Response.Write("<!--" & vbCrLf)
            Response.Write("alert(""Impossibile cancellare il volontario in quanto non e' nello stato Registrato.Operazione Interrotta."");" & vbCrLf)
            'Response.Write("//-->" & vbCrLf)
            Response.Write("</script>" & vbCrLf)
        End If

    End Sub

    Sub CaricaUserName(ByVal IdAttSedeAss As String)
        Dim dtrLocal As System.Data.SqlClient.SqlDataReader
        'variabile che prende il valore dell'id della nazione base(quella che apparirà prima nella combo)
        Dim IdNazioneBase As Integer
        Dim strSql As String
        Dim i As Integer
        'preparo la query per le nazioni
        strSql = "select isnull(UserNameStato,'') as UserNameStato from AttivitàSediAssegnazione where idattivitàsedeassegnazione='" & IdAttSedeAss & "' and StatoGraduatoria=3"
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))
        'se ci sono dei record li carico nelle combo
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            'passo il valore dello username alla label
            lblConfermataDa.Text = dtrLocal("UserNameStato")
        Else
            lblConfermataDa.Visible = False
            lblVisConfermataDa.Visible = False
        End If
        'controllo e chiudo il datareader se aperto
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

    End Sub

    Private Sub BloccaDataInizio()
        'Aggiunto da Alessandra Taballione il 23/04/2005
        'blocco della data inizio Attività
        piker.Value = 0
        txtinizioEff.ReadOnly = True
        txtInizioServizioSede.ReadOnly = True

        Dim dataInizioEffettivo As Date
        If Session("TipoUtente") = "E" Then
            If (Date.TryParse(txtinizioEff.Text, dataInizioEffettivo)) Then
                If (dataInizioEffettivo > Date.Now) Then
                    txtinizioEff.Text = ""
                    txtFineEff.Text = ""
                End If
            End If
        End If


        'Image3.Visible = False
    End Sub

    Private Sub ControlloVolontari()
        'Generata da Alessandra Taballione il 25/05/2005
        'Routine che verifica Anomalie volontario
        Dim item As DataGridItem
        Dim Eta As Integer
        Dim i As Integer
        'Dim color1 As New System.Drawing.Color
        'Dim color As New System.Drawing.Color
        For Each item In dgRisultatoRicerca.Items


            'Aggiunto da Alessandra Taballione il 09/06/2005
            'Verifica su tabella con codici Fiscali lotus 
            strsql = "select * from impVolontariLotus where CF='" & Replace(dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text, "'", "''") & "' "
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrGenerico.HasRows = True Then
                For i = 0 To 26
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.FromArgb(155, 255, 155)
                Next
            Else
                'dgRisultatoRicerca.Items(item.ItemIndex).Cells(23).Text = ""
            End If

            'Verifico se l'età del volontario risulta maggiore dei 28 anni
            strsql = "select e.datanascita, "
            strsql = strsql & "(select "
            strsql = strsql & "case when dateadd (yy,bando.maxeta,e.datanascita) between CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataInizioVolontari ELSE BANDORICORSI.DataInizioVolontari END and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END then 'Giallo' "
            strsql = strsql & "when dateadd (yy,bando.maxeta,e.datanascita) < CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataInizioVolontari ELSE BANDORICORSI.DataInizioVolontari END then 'Arancio' "
            strsql = strsql & "else 'Ammesso' end  "
            strsql = strsql & "from bando "
            strsql = strsql & "inner join bandiattività on bandiattività.idbando=bando.idbando "
            strsql = strsql & "inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività "
            strsql = strsql & "inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività "
            strsql = strsql & "where attivitàsediassegnazione.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione )as Ammesso "
            strsql = strsql & "from entità e "
            strsql = strsql & "inner join graduatorieentità ge on e.identità=ge.identità "
            strsql = strsql & "inner join attivitàsediassegnazione asa on asa.idattivitàsedeassegnazione=ge.idattivitàsedeassegnazione "
            strsql = strsql & "inner join attività a on a.idattività=asa.idattività "
            strsql = strsql & "LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO "
            strsql = strsql & "where e.identità='" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & "'"

            '" where e.identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & " "

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrGenerico.Read()
            'Eta = dgRisultatoRicerca.Items(item.ItemIndex).Cells(21).Text
            If dtrGenerico("Ammesso") <> "Ammesso" Then
                Select Case dtrGenerico("Ammesso")
                    Case "Giallo"
                        For i = 0 To 26
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.Khaki
                        Next
                    Case "Arancio"
                        For i = 0 To 26
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.FromArgb(240, 186, 64)
                        Next
                End Select
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            'Verifico se l'età del volontario risulta minore dei 18 anni
            'Eta = dgRisultatoRicerca.Items(item.ItemIndex).Cells(21).Text

            strsql = "select e.datanascita, "
            strsql = strsql & "(select "
            strsql = strsql & "case when DATEADD (YY,bando.mineta,e.datanascita ) <= CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataInizioVolontari ELSE BANDORICORSI.DataInizioVolontari END then 'Ammesso' "
            strsql = strsql & "when dateadd (yy,bando.mineta,e.datanascita) between CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataInizioVolontari ELSE BANDORICORSI.DataInizioVolontari END and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END then 'Giallo' "
            strsql = strsql & "else 'Arancio' end "
            strsql = strsql & "from bando "
            strsql = strsql & "inner join bandiattività on bandiattività.idbando=bando.idbando  "
            strsql = strsql & "inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività  "
            strsql = strsql & "inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività  "
            strsql = strsql & "where attivitàsediassegnazione.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione )as Ammesso "
            strsql = strsql & "from entità e "
            strsql = strsql & "inner join graduatorieentità ge on e.identità=ge.identità "
            strsql = strsql & "inner join attivitàsediassegnazione asa on asa.idattivitàsedeassegnazione=ge.idattivitàsedeassegnazione "
            strsql = strsql & "inner join attività a on a.idattività=asa.idattività "
            strsql = strsql & "LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO "
            strsql = strsql & "where e.identità='" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & "'"

            'strsql = "select e.datanascita," & _
            '" (select case when DATEADD ( YY , 18, e.datanascita ) <= datafinevolontari then 'Ammesso' else 'NonAmmesso' end" & _
            '" from bando " & _
            '" inner join bandiattività on bandiattività.idbando=bando.idbando  " & _
            '" inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività  " & _
            '" inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività  " & _
            '" where attivitàsediassegnazione.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione )as Ammesso " & _
            '" from entità e " & _
            '" inner join graduatorieentità ge on e.identità=ge.identità " & _
            '" inner join attivitàsediassegnazione asa on asa.idattivitàsedeassegnazione=ge.idattivitàsedeassegnazione " & _
            '" inner join attività a on a.idattività=asa.idattività " & _
            '" where e.identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & " "
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrGenerico.Read()
            If dtrGenerico("Ammesso") <> "Ammesso" Then
                Select Case dtrGenerico("Ammesso")
                    Case "Giallo"
                        For i = 0 To 26
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.Khaki
                        Next
                    Case "Arancio"
                        For i = 0 To 26
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.FromArgb(240, 186, 64)
                        Next
                End Select
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            'Esiste Record in AttivitàEntità con stato <> rinuncia?
            'strsql = "Select distinct Entità.identità,Entità.cognome,Entità.nome," & _
            '" statiEntità.Rinuncia from Entità " & _
            '" inner join AttivitàEntità on (Entità.identità=AttivitàEntità.identità) " & _
            ' " inner join AttivitàEntisediAttuazione on (AttivitàEntisediAttuazione.idattivitàEntesedeattuazione=AttivitàEntità.idattivitàEntesedeattuazione) " & _
            '" inner join statiEntità on (statiEntità.idstatoEntità=Entità.idstatoentità) " & _
            '" where entità.identità<>" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & " and statiEntità.rinuncia <> 1 "

            strsql = "Select count(*) as n " & _
                     " from Entità " & _
                     " inner join AttivitàEntità on (Entità.identità=AttivitàEntità.identità) " & _
                     " inner join AttivitàEntisediAttuazione on (AttivitàEntisediAttuazione.idattivitàEntesedeattuazione=AttivitàEntità.idattivitàEntesedeattuazione) " & _
                     " inner join statiEntità on (statiEntità.idstatoEntità=Entità.idstatoentità) " & _
                     " left join causali on entità.idcausalechiusura = causali.idcausale " & _
                     " where ENTITà.IDENTITà<>'" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & " ' AND entità.codicefiscale='" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text & "' and statiEntità.rinuncia <> 1 and isnull(causali.bloccanuovoservizio,1) <> 0" ' and AttivitàEntisediAttuazione.idattività <> " & lblidattivita.Text & ""

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            'If dtrGenerico.HasRows = True Then
            '    For i = 0 To 25
            '        dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.LightSalmon
            '    Next
            'End If
            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                If dtrGenerico("n") > 0 Then
                    For i = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.Plum
                    Next
                End If
            End If

            'Esiste Record in GraduatorieEntità per stesso Volontario e Stesso Bando?
            strsql = "SELECT count(graduatorieentità.identità) as idEntità FROM graduatorieentità " & _
            " inner join entità on entità.identità = graduatorieentità.identità " & _
            " inner join attivitàsediassegnazione on " & _
            " (attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieentità.idattivitàsedeassegnazione)" & _
            " inner join attività on (attività.idattività=attivitàsediassegnazione.idattività)" & _
            " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
            " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
            " where entità.codicefiscale='" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text & "' and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.Gruppo ELSE BANDORICORSI.Gruppo END =(Select CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.Gruppo ELSE BANDORICORSI.Gruppo END from Attività " & _
            " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
            " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
            " where Attività.idattività=" & lblidattivita.Text & ")"
            '" where graduatorieentità.identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(19).Text & " and bando.idbando =(Select bando.idBando from Attività " & _
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                If dtrGenerico("identità") > 1 Then
                    For i = 0 To 26
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.LightSalmon
                    Next
                End If
            End If
            'End If 'qui
        Next
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Sub ControlloVolontariNew()
        Dim item As DataGridItem
        Dim strRisultato As String
        Dim strVisualizzaEsito As String
        Dim i As Integer
        Dim blnColoraCellaVerifica As Boolean = False

        For Each item In dgRisultatoRicerca.Items
            strRisultato = UCase(LeggiStoreVolontariControlli(dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text, strVisualizzaEsito))
            'If strRisultato = "LOTUS" Then
            '    For i = 0 To 26
            '        dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.FromArgb(155, 255, 155)
            '    Next
            'End If

            'If strRisultato = "DA VERIFICARE" Then
            '    For i = 0 To 26
            '        dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.Khaki
            '    Next
            'End If

            'If strRisultato = "ETA" Then
            '    For i = 0 To 26
            '        dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.FromArgb(240, 186, 64)
            '    Next
            'End If

            'If strRisultato = "SERVIZIO EFFETTUATO" Then
            '    For i = 0 To 26
            '        dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.Plum
            '    Next
            'End If

            'If strRisultato = "DOPPIA DOMANDA" Then
            '    For i = 0 To 26
            '        dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.LightSalmon
            '    Next
            'End If

            If strRisultato = "ANOMALIA" Then
                For i = 0 To dgRisultatoRicerca.Columns.Count - 1
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.LightSalmon
                    ''dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).Font.Size = 9
                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(1).Font.Bold = True
                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(23).Font.Bold = True
                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).ForeColor = Color.Black
                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(23).ForeColor = Color.Red
                Next
            End If
            If strRisultato = "DA VERIFICARE" Then
                For i = 0 To dgRisultatoRicerca.Columns.Count - 1
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.Khaki
                    ''dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).Font.Size = 9
                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(1).Font.Bold = True
                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(23).Font.Bold = True
                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(i).ForeColor = Color.Black
                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(23).ForeColor = Color.Red
                Next
            End If
            dgRisultatoRicerca.Items(item.ItemIndex).Cells(23).Text = dgRisultatoRicerca.Items(item.ItemIndex).Cells(23).Text & "<br />" & strVisualizzaEsito

            If Session("Sistema") = "Futuro" Then

                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                strsql = "Select isnull(requisiti,'') as requisiti from entità where identità =" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text
                dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
                If dtrGenerico.HasRows = True Then
                    dtrGenerico.Read()
                    If UCase(dtrGenerico("requisiti")) = "IN DEFINIZIONE" Then
                        blnColoraCellaVerifica = True
                    Else
                        blnColoraCellaVerifica = False
                    End If
                End If
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                If blnColoraCellaVerifica = True Then 'coloro la cella VERIFICA solo se il requisito è IN DEFINIZIONE
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(23).BackColor = Color.FromArgb(240, 205, 174)
                End If
            End If

        Next

    End Sub

    Private Function LeggiStoreVolontariControlli(ByVal IDEntità As Integer, ByRef VisualizzaEsito As String) As String
        'Agg. da Simona Cordella il 16/06/2009
        'richiamo store che verifca se l'ente ha completato tutti gli inserimeni e gli aggiornamenti necessari per effettuare la presentazione della domanda di accrditamento /adeguamento
        Dim intValore As Integer

        Dim myCommand As SqlClient.SqlCommand
        myCommand = New SqlClient.SqlCommand
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = "SP_VOLONTARI_CONTROLLI"
        myCommand.Connection = Session("Conn")

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdEntita"
        sparam.SqlDbType = SqlDbType.Int
        myCommand.Parameters.Add(sparam)


        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@Esito"
        sparam1.SqlDbType = SqlDbType.VarChar
        sparam1.Size = 50
        sparam1.Direction = ParameterDirection.Output
        myCommand.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@VisualizzaEsito"
        sparam2.SqlDbType = SqlDbType.VarChar
        sparam2.Size = 100
        sparam2.Direction = ParameterDirection.Output
        myCommand.Parameters.Add(sparam2)


        myCommand.Parameters("@IdEntita").Value = IDEntità
        'Reader = CustOrderHist.ExecuteReader()
        myCommand.ExecuteScalar()

        VisualizzaEsito = myCommand.Parameters("@VisualizzaEsito").Value
        LeggiStoreVolontariControlli = myCommand.Parameters("@Esito").Value

    End Function

    Private Sub Popolamaschera()
        strsql = "Select entisedi.idente,getdate() as dataOdierna, entisedi.denominazione as sedefisica,attività.titolo,attività.codiceente as CodProg, attivitàsediassegnazione.StatoGraduatoria," & _
        " attivitàsediassegnazione.idattività as idattivita ,attivitàsediassegnazione.idattivitàsedeassegnazione,isnull(attivitàsediassegnazione.NoteGraduatoria,'') as NoteGraduatoria, " & _
           "  entisedi.identesede,comuni.denominazione + '('+ provincie.provincia + ')'as comune," & _
           "  entisedi.indirizzo + ' Nr.' + entisedi.civico as indirizzo ,entisedi.prefissotelefono + '/' + entisedi.telefono as telefono, " & _
           " attività.datainizioAttività,attività.datafineAttività,attività.datainizioprevista,attività.datafinePrevista, attivitàsediassegnazione.datainiziodifferita, attivitàsediassegnazione.datafinedifferita " & _
           " from entisedi " & _
           " inner join attivitàsediassegnazione on (entisedi.identesede=attivitàsediassegnazione.identesede)" & _
           " inner join attività on (attività.idattività=attivitàsediassegnazione.idattività)" & _
           " inner join comuni on (entisedi.idcomune=comuni.idcomune) " & _
           " inner join provincie on (provincie.idprovincia=comuni.idprovincia) " & _
           " where entisedi.identesede=" & Request.QueryString("IdEnteSede") & " and attivitàsediassegnazione.idattivitàSedeAssegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            lblSedeFisica.Text = dtrGenerico("sedefisica")
            lblIndirizzo.Text = dtrGenerico("indirizzo")
            lblComune.Text = dtrGenerico("Comune")
            lbltelefono.Text = IIf(IsDBNull(dtrGenerico("telefono")) = True, "", dtrGenerico("telefono"))
            lblidattivita.Text = dtrGenerico("idattivita")
            lblProgetto.Text = dtrGenerico("Titolo")
            lblcodprogetto.Text = dtrGenerico("CodProg")
            lblidattivitasedeassegnazione.Text = dtrGenerico("idattivitàsedeassegnazione")
            Session("IdASA") = dtrGenerico("idattivitàsedeassegnazione")
            txtidente.Text = dtrGenerico("idente")
            txtDataOdierna.Text = dtrGenerico("dataOdierna")

            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                'RigaNote.Visible = True  adc
                lblNote.Visible = True
                cmdInsNote.Visible = True
                txtNote.Visible = True

                
                cmdInsNote.Visible = True
                txtNote.Text = dtrGenerico("NoteGraduatoria")
            End If
            'Aggiunto da Alessandra Taballione il 20.04.2005
            'Data Inizio Attività
            If Not IsDBNull(dtrGenerico("datainizioAttività")) Then
                txtinizioEff.Text = dtrGenerico("datainizioAttività")
                txtdatainizioDB.Text = dtrGenerico("datainizioAttività")
            End If
            'Data Fine Attività
            If Not IsDBNull(dtrGenerico("datafineAttività")) Then
                txtFineEff.Text = dtrGenerico("datafineAttività")
            End If
            'Data Inizio Differita
            If Not IsDBNull(dtrGenerico("datainiziodifferita")) Then
                txtInizioServizioSede.Text = dtrGenerico("datainiziodifferita")
                cmdAnnullaDifferita.Visible = False
            End If
            'Data Fine Differita
            If Not IsDBNull(dtrGenerico("datafinedifferita")) Then
                txtFineServizioSede.Text = dtrGenerico("datafinedifferita")
            End If
            If Not IsDBNull(dtrGenerico("datainizioprevista")) Then
                lblinPrev.Text = dtrGenerico("datainizioprevista")
                'Se non è stata confermata o repinta viene suggerita la data inizio prevista
                'If txtinizioEff.Text = "" And dtrGenerico("StatoGraduatoria") < 3 Then
                '    txtinizioEff.Text = dtrGenerico("datainizioprevista")
                'End If
            End If
            If Not IsDBNull(dtrGenerico("datafineprevista")) Then
                lblFinePre.Text = dtrGenerico("datafineprevista")
            End If
            'If txtFineEff.Text = "" And dtrGenerico("StatoGraduatoria") < 3 Then
            '    Dim data As Date
            '    data = CDate(txtinizioEff.Text)
            '    txtFineEff.Text = data.AddDays(365)
            'End If
            '1:Registrata 2:Presentata 3:Confermata
            Select Case dtrGenerico("StatoGraduatoria")
                Case 1
                    lblStatoGraduatoria.Text = "Registrata"
                Case 2
                    lblStatoGraduatoria.Text = "Presentata"
                Case 3
                    lblStatoGraduatoria.Text = "Confermata"
                Case 4
                    lblStatoGraduatoria.Text = "Respinta"
                Case 0
                    lblStatoGraduatoria.Text = "Mancante"
            End Select
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'Aggiunto da Alessandra Taballione il 10/05/2005
        'Controllo Volontari Inseriti e Selezionati <= Totale Volontari Progetto
        'strsql = "SELECT  ISNULL(SUM(s2.NumeroPostiNoVittoNoAlloggio + s2.NumeroPostiVittoAlloggio + " & _
        '" s2.NumeroPostiVitto), 0) as VolRic " & _
        '" FROM  attivitàentisediattuazione S2 " & _
        '" WHERE S2.idattività=" & lblidattivita.Text & ""

        'Modificata temporaneamente il 24/05/2005 
        'cambio tabella da attivitàentisediattuazione ad attività
        strsql = "SELECT  ISNULL(SUM(s2.NumeroPostiNoVittoNoAlloggio + s2.NumeroPostiVittoAlloggio + " & _
        " s2.NumeroPostiVitto), 0) as VolRic " & _
        " FROM  attività S2 " & _
        " WHERE S2.idattività=" & lblidattivita.Text & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            lblNVolRichiesti.Text = dtrGenerico("VolRic")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        strsql = " SELECT COUNT(*) as VolAmm " & _
        " FROM graduatorieentità s1 " & _
        " inner join entità on entità.identità=s1.identità" & _
        " inner join statientità on entità.idstatoentità=statientità.idstatoentità" & _
        " WHERE s1.idattivitàsedeassegnazione in (select attivitàsediassegnazione.idattivitàsedeassegnazione  " & _
        " from attivitàsediassegnazione where attivitàsediassegnazione.idattività=" & lblidattivita.Text & " )" & _
        " and ammesso=1 and (statientità.defaultstato=1 or statientità.inservizio=1) "
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()
        lblVolSel.Text = dtrGenerico("VolAmm")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        ''FAMI
        'Dim INTFAMI As Integer = 0
        'Dim INTGMO As Integer = 0
        'strsql = "SELECT  ISNULL(a.NumeroPostiFami,0) as FAMI " & _
        '   " FROM  attivitàentisediattuazione a   " & _
        '   " INNER JOIN entisediattuazioni e ON a.IDEnteSedeAttuazione = e.IDEnteSedeAttuazione " & _
        '   " WHERE a.IDAttività = " & lblidattivita.Text & " And e.IDEnteSede = " & lblidSede.Text & ""

        ''IDEnteSedeAttuazione
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If
        'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        'dtrGenerico.Read()
        'IntFAMI = dtrGenerico("FAMI")
        'If INTFAMI <> 0 Then
        '    lblNVolRichiesti.Text = lblNVolRichiesti.Text & " (FAMI:  " & INTFAMI & ")"
        'End If

        ''GMO
        'strsql = "SELECT  ISNULL(NumeroGiovaniMinoriOpportunità,0) as GMO " & _
        '        " FROM  attività S2 " & _
        '        " WHERE S2.idattività=" & lblidattivita.Text & ""

        ''IDEnteSedeAttuazione
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If
        'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        'dtrGenerico.Read()
        'INTGMO = dtrGenerico("GMO")
        'If INTGMO <> 0 Then
        '    lblNVolRichiesti.Text = lblNVolRichiesti.Text & " (GMO:  " & INTGMO & ")"
        'End If
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If
    End Sub

    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        'Aggiunto da Alessandra Taballione il 26/05/2005
        'Se stato graduatoria = Presentata e utenza di tipo E e esiste cronologia con username E
        'Visualizzo Cronologia graduatorie entità
        If lblStatoGraduatoria.Text = "Presentata" And Session("TipoUtente") = "E" Then
            'e esiste cronologia con username E
            strsql = "select * from cronologiagraduatorieEntità " & _
            " where(cronologiagraduatorieEntità.idattivitàsedeassegnazione =" & Request.QueryString("IDAttivitasedeAssegnazione") & ") " & _
            " and substring(cronologiagraduatorieEntità.username,1,1)='E'"
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrGenerico.HasRows = True Then

                strsql = "Select cronologiagraduatorieEntità.Ammesso as amm, " & _
                    " cronologiagraduatorieEntità.Stato as ido,case isnull(TipologiePosto.idtipologiaposto,-1) " & _
                    " when -1 then 0 else TipologiePosto.idtipologiaposto end as tipoposto, " & _
                    " convert(varchar,replace(cronologiagraduatorieEntità.Punteggio,'.',',')) as punteggio, " & _
                    " '<img src=''images/Icona_Volontario_small.png'' alt=''Volontario'' style=''CURSOR: pointer;''  " & _
                    " onclick=''JavaScript:ModificaVolontario(' + convert(varchar,entità.IDEntità) +'," & lblidattivitasedeassegnazione.Text & "," & Request.QueryString("IdEnteSede") & "," & lblpresentata.Text & "," & Request.QueryString("IdAttivita") & ")'' />' as img, " & _
                    " entità.identità,cronologiagraduatorieEntità.idgraduatoriaentità, " & _
                    " entità.cognome +' '+ entità.nome + ' ('+ statientità.statoentità +  " & _
                    " case when (SELECT count(*) FROM attivitàentità att WHERE att.IDEntità =entità.IDEntità) >1 then ' -Ricollocato-' else '' End +')' as nominativo, " & _
                    " statientità.idstatoentità as statoentità," & _
                    " entità.Codicefiscale,entità.datanascita," & _
                    " CASE CONVERT(varchar,isnull(entità.TMPIdSedeAttuazioneSecondaria,0)) " & _
                    " when 0 then CONVERT(varchar,entità.tMPIdSedeAttuazione) else " & _
                    " CONVERT(varchar,entità.tMPIdSedeAttuazione) + ' (' + CONVERT(varchar,isnull(entità.TMPIdSedeAttuazioneSecondaria,0)) + ')'  end as codiceSede ," & _
                    " comuni.denominazione + ' ('+ provincie.provincia + ')'as comune," & _
                    " telefono, Email,ordine, '<img src=''images/vincoli_small.png'' style=''cursor:pointer;'' alt=''Vincoli Volontario''  onclick=''JavaScript:ApriVincoli(' + convert(varchar,entità.IDEntità) + ')'' />' as Valutazione," & _
                    " cronologiagraduatorieEntità.identità, " & _
                    " isnull((select datediff (yy,entità.datanascita,CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END) " & _
                    " from bando " & _
                    " inner join bandiattività on bandiattività.idbando=bando.idbando " & _
                    " inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività " & _
                    " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
                    " inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività " & _
                    " where attivitàsediassegnazione.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "),0) as eta," & _
                    " isnull(entità.tmpidsedeattuazione,0)as tmpidsedeattuazione ,  " & _
                    " '<img src=''images/canc_small.png'' alt=''Cancella Volontario''  style=''CURSOR: pointer;''  " & _
                    " onclick=''JavaScript:ConfermaCanc(' + convert(varchar,entità.IDEntità) + ',' + convert(varchar,statientità.idstatoentità) + ')'' />' as CancellaV, " & _
                    " '<img src=''images/xp2.gif'' style=''cursor:pointer;'' width=''20px'' alt=''Dettaglio Volontario''  height=''20px''   " & _
                    " onclick=''JavaScript:Verificalotus(""' + convert(varchar,isnull((select top 1 codicevolontario from impVolontariLotus where entità.codicefiscale=impVolontariLotus.cf),0)) + '"" ," & _
                    " ""'+ entità.Codicefiscale + '""," & _
                    " ' + convert(varchar,isnull((select datediff (yy,entità.datanascita,CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END)   " & _
                    " from bando " & _
                    " inner join bandiattività on bandiattività.idbando=bando.idbando  " & _
                    " inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività  " & _
                    " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
                    " inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività  " & _
                    " where attivitàsediassegnazione.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "),0)) + '," & _
                    " '+ convert(varchar,entità.identità) +')''/>' as codFis," & _
                    " entità.identità, " & _
                    "CASE isnull(Causali.Descrizione, '-1') WHEN '-1' THEN '' ELSE '<img src=''images/i5.gif'' style=width=''20px'' height=''20px''  alt=''Causale chiusura: ' END + isnull(Causali.Descrizione +'''/>', '') as Info " & _
                    " ,isnull(entità.ammessorecupero,0) as AmmessoRecupero " & _
                    " , CASE isnull(entità.GMO,'') WHEN '' THEN 0 ELSE 1 END AS GMO " & _
                    " , CASE isnull(entità.FAMI,'') WHEN '' THEN 0 ELSE 1 END AS FAMI " & _
                    " , CASE isnull(entità.FAMI,'') WHEN '' THEN 0 ELSE 1 END AS FAMI " & _
                    " , isnull(entità.dol_id,0) as Dol_Id " & _
                    " from entità " & _
                    " inner join statientità on statientità.idstatoentità=entità.idstatoentità " & _
                    " inner join cronologiagraduatorieEntità on cronologiagraduatorieEntità.identità=Entità.identità " & _
                    " left join TipologiePosto on cronologiagraduatorieEntità.idtipologiaposto=TipologiePosto.idtipologiaposto " & _
                    " inner join comuni on comuni.idcomune=entità.idcomunenascita  " & _
                    " inner join provincie on (provincie.idprovincia=comuni.idprovincia)  LEFT OUTER JOIN " & _
                    "Causali ON entità.IDCausaleChiusura = Causali.IDCausale" & _
                    " where  cronologiagraduatorieEntità.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "  and substring(cronologiagraduatorieEntità.username,1,1)='E' order by case cronologiagraduatorieEntità.stato when 0 then 2 else cronologiagraduatorieEntità.stato  end, cronologiagraduatorieEntità.ordine"
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
                GridDaCaricare.DataSource = dtsGenerico
                GridDaCaricare.DataBind()
                ChekIdonei()
            Else

                strsql = "Select graduatorieEntità.Ammesso as amm,graduatorieEntità.Stato as ido,case isnull(TipologiePosto.idtipologiaposto,-1)when -1 then 0 else TipologiePosto.idtipologiaposto end as tipoposto, convert(varchar,replace(graduatorieEntità.Punteggio,'.',',')) as punteggio, " & _
                    " '<img src=''images/Icona_Volontario_small.png'' style=''cursor:pointer;'' alt=''Volontario''  onclick=''JavaScript:ModificaVolontario(' + convert(varchar,entità.IDEntità) +'," & lblidattivitasedeassegnazione.Text & "," & Request.QueryString("IdEnteSede") & "," & lblpresentata.Text & "," & Request.QueryString("IdAttivita") & ")'' />' as img,entità.identità,graduatorieEntità.idgraduatoriaentità," & _
                    " entità.cognome +' '+ entità.nome + ' ('+ statientità.statoentità + " & _
                    " case when (SELECT count(*) FROM attivitàentità att WHERE att.IDEntità =entità.IDEntità) >1 then ' -Ricollocato-' else '' End +')' as nominativo, " & _
                    " statientità.idstatoentità as statoentità," & _
                    " entità.Codicefiscale,entità.datanascita," & _
                    " comuni.denominazione + ' ('+ provincie.provincia + ')'as comune," & _
                    " telefono, Email,ordine, '<img src=''images/vincoli_small.png'' style=''cursor:pointer''; alt=''Vincoli Volontario''  onclick=''JavaScript:ApriVincoli(' + convert(varchar,entità.IDEntità) + ')'' />' as Valutazione," & _
                    " graduatorieEntità.identità, " & _
                    " CASE CONVERT(varchar,isnull(entità.TMPIdSedeAttuazioneSecondaria,0)) " & _
                    " when 0 then CONVERT(varchar,entità.tMPIdSedeAttuazione) else " & _
                    " CONVERT(varchar,entità.tMPIdSedeAttuazione) + ' (' + CONVERT(varchar,isnull(entità.TMPIdSedeAttuazioneSecondaria,0)) + ')'  end as codiceSede ," & _
                    " isnull((select datediff (yy,entità.datanascita,CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END) " & _
                    " from bando " & _
                    " inner join bandiattività on bandiattività.idbando=bando.idbando " & _
                    " inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività " & _
                    " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
                    " inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività " & _
                    " where attivitàsediassegnazione.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "),0) as eta," & _
                    " isnull(entità.tmpidsedeattuazione,0)as tmpidsedeattuazione , " & _
                    " '<img src=''images/canc_small.png'' alt=''Cancella Volontario'' style=''CURSOR: pointer;''  " & _
                    " onclick=''JavaScript:ConfermaCanc(' + convert(varchar,entità.IDEntità) + ',' + convert(varchar,statientità.idstatoentità) + ')'' />' as CancellaV, " & _
                    " '<img src=''images/xp2.gif'' style=''cursor:pointer;'' alt=''Dettaglio Volontario'' width=''20px'' height=''20px'' style=CURSOR: pointer;  " & _
                    " onclick=''JavaScript:Verificalotus(""' + convert(varchar,isnull((select top 1 codicevolontario from impVolontariLotus where entità.codicefiscale=impVolontariLotus.cf),0)) + '""," & _
                    " ""'+ entità.Codicefiscale + '""," & _
                    " ' + convert(varchar,isnull((select datediff (yy,entità.datanascita,CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END)  " & _
                    " from bando " & _
                    " inner join bandiattività on bandiattività.idbando=bando.idbando  " & _
                    " inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività  " & _
                    " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
                    " inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività  " & _
                    " where attivitàsediassegnazione.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "),0)) + '," & _
                    " ' + convert(varchar,entità.identità) + ')''/>' as codFis," & _
                    " entità.identità, " & _
                    "CASE isnull(Causali.Descrizione, '-1') WHEN '-1' THEN '' ELSE '<img src=''images/i5.gif'' style=width=''20px'' height=''20px''  alt=''Causale chiusura: ' END + isnull(Causali.Descrizione +'''/>', '') as Info " & _
                    " ,isnull(entità.ammessorecupero,0) as AmmessoRecupero " & _
                    " , CASE isnull(entità.GMO,'') WHEN '' THEN 0 ELSE 1 END AS GMO " & _
                    " , CASE isnull(entità.FAMI,'') WHEN '' THEN 0 ELSE 1 END AS FAMI " & _
                    " , isnull(entità.dol_id,0) as Dol_Id " & _
                    " from entità " & _
                    " inner join statientità on statientità.idstatoentità=entità.idstatoentità " & _
                    " inner join graduatorieEntità on graduatorieEntità.identità=Entità.identità " & _
                    " left join TipologiePosto on graduatorieEntità.idtipologiaposto=TipologiePosto.idtipologiaposto " & _
                    " inner join comuni on comuni.idcomune=entità.idcomunenascita  " & _
                    " inner join provincie on (provincie.idprovincia=comuni.idprovincia)  LEFT OUTER JOIN " & _
                    "Causali ON entità.IDCausaleChiusura = Causali.IDCausale" & _
                    " where  graduatorieEntità.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & " order by case graduatorieEntità.stato when 0 then 2 else graduatorieEntità.stato  end, graduatorieEntità.ordine"
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
                GridDaCaricare.DataSource = dtsGenerico
                GridDaCaricare.DataBind()
                ChekIdonei()
            End If
        Else

            strsql = "Select  graduatorieEntità.Ammesso as amm,graduatorieEntità.Stato as ido,case isnull(TipologiePosto.idtipologiaposto,-1)when -1 then 0 else TipologiePosto.idtipologiaposto end as tipoposto,  convert(varchar,replace(graduatorieEntità.Punteggio,'.',',')) as punteggio, " & _
                " '<img src=''images/Icona_Volontario_small.png'' style=''CURSOR: pointer;'' alt=''Volontario''  onclick=''JavaScript:ModificaVolontario(' + convert(varchar,entità.IDEntità) +'," & lblidattivitasedeassegnazione.Text & "," & Request.QueryString("IdEnteSede") & "," & lblpresentata.Text & "," & Request.QueryString("IdAttivita") & ")'' />' as img,entità.identità,graduatorieEntità.idgraduatoriaentità," & _
                " entità.cognome +' '+ entità.nome + " & _
                " ' ('+ statientità.statoentità +  " & _
                " case " & _
                " when entità.datainizioservizio <> '" & txtinizioEff.Text & "' " & _
                " then  ' ' + convert(varchar,entità.datainizioservizio,3) else '' end +" & _
                " case when (SELECT count(*) FROM attivitàentità att WHERE att.IDEntità =entità.IDEntità) >1 then ' -Ricollocato-' else '' End +')' as nominativo, " & _
                " statientità.idstatoentità as statoentità," & _
                " case isnull(categorieentità.idcategoriaentità,1) when 1 then entità.Codicefiscale else case entità.anomaliaCF when 1 then entità.codicefiscale + '<BR>' + categorieentità.categoriaabbreviata + '<BR>(ANOMALIA CF)' else entità.codicefiscale + '<BR>' + categorieentità.categoriaabbreviata end end as CodiceFiscale,entità.datanascita," & _
                " comuni.denominazione + ' ('+ provincie.provincia + ')'as comune," & _
                " telefono, Email,ordine, '<img src=''images/vincoli_small.png'' style=''cursor:pointer;'' alt=''Vincoli Volontario'' onclick=''JavaScript:ApriVincoli(' + convert(varchar,entità.IDEntità) + ')''/>' as Valutazione," & _
                " graduatorieEntità.identità," & _
                " CASE CONVERT(varchar,isnull(entità.TMPIdSedeAttuazioneSecondaria,0)) " & _
                " when 0 then CONVERT(varchar,entità.tMPIdSedeAttuazione) else " & _
                " CONVERT(varchar,entità.tMPIdSedeAttuazione) + ' (' + CONVERT(varchar,isnull(entità.TMPIdSedeAttuazioneSecondaria,0)) + ')'  end as codiceSede ," & _
                " isnull((select datediff (yy,entità.datanascita,CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END) " & _
                " from bando " & _
                " inner join bandiattività on bandiattività.idbando=bando.idbando " & _
                " inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività " & _
                " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
                " inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività " & _
                " where attivitàsediassegnazione.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "),0) as eta," & _
                " isnull(entità.tmpidsedeattuazione,0)as tmpidsedeattuazione , " & _
                " '<img src=''images/canc_small.png'' alt=''Cancella Volontario'' style=''CURSOR: pointer;''  " & _
                " onclick=''JavaScript:ConfermaCanc(' + convert(varchar,entità.IDEntità) + ',' + convert(varchar,statientità.idstatoentità) + ')'' />' as CancellaV, " & _
                " '<img src=''images/xp2.gif'' style=''cursor:pointer;'' alt=''Dettaglio Volontario'' width=''20px'' height=''20px''  " & _
                " onclick=''JavaScript:Verificalotus(""' + convert(varchar,isnull((select top 1 codicevolontario from impVolontariLotus where entità.codicefiscale=impVolontariLotus.cf),0)) + '""," & _
                " ""'+ entità.Codicefiscale + '""," & _
                " ' + convert(varchar,isnull((select datediff (yy,entità.datanascita,CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataFineVolontari ELSE BANDORICORSI.DataFineVolontari END)  " & _
                " from bando " & _
                " inner join bandiattività on bandiattività.idbando=bando.idbando  " & _
                " inner join attività on attività.idbandoAttività=bandiattività.idbandoAttività  " & _
                " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
                " inner join attivitàsediassegnazione on attivitàsediassegnazione.idattività=attività.idattività  " & _
                " where attivitàsediassegnazione.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "),0)) + '," & _
                " ' + convert(varchar,entità.identità) + ')''/>' as codFis," & _
                " entità.identità, " & _
                "CASE isnull(Causali.Descrizione, '-1') WHEN '-1' THEN '' ELSE '<img src=images/i5.gif style=width=''20px'' height=''20px''  alt=''Causale chiusura: ' END + isnull(Causali.Descrizione +'''/>', '') as Info " & _
                " ,isnull(entità.ammessorecupero,0) as AmmessoRecupero " & _
                " , CASE isnull(entità.GMO,'') WHEN '' THEN 0 ELSE 1 END AS GMO " & _
                " , CASE isnull(entità.FAMI,'') WHEN '' THEN 0 ELSE 1 END AS FAMI " & _
                " , isnull(entità.dol_id,0) as Dol_Id " & _
                "FROM entità INNER JOIN " & _
                "StatiEntità ON StatiEntità.IDStatoEntità = entità.IDStatoEntità LEFT OUTER JOIN " & _
                "IMPVOLONTARILOTUS ON entità.CodiceFiscale = IMPVOLONTARILOTUS.CF INNER JOIN " & _
                "GraduatorieEntità ON GraduatorieEntità.IdEntità = entità.IDEntità LEFT OUTER JOIN " & _
                "TipologiePosto ON GraduatorieEntità.IDTipologiaPosto = TipologiePosto.IDTipologiaPosto INNER JOIN " & _
                "comuni ON comuni.IDComune = entità.IDComuneNascita INNER JOIN " & _
                "provincie ON provincie.IDProvincia = comuni.IDProvincia LEFT OUTER JOIN " & _
                "Causali ON entità.IDCausaleChiusura = Causali.IDCausale LEFT OUTER JOIN  Categorieentità on entità.idCategoriaentità = Categorieentità.idCategoriaentità " & _
                " where  graduatorieEntità.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & " order by case graduatorieEntità.stato when 0 then 2 else graduatorieEntità.stato  end , graduatorieEntità.ordine"
            dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
            GridDaCaricare.DataSource = dtsGenerico
            GridDaCaricare.DataBind()
            ChekIdonei()
            If Session("Sistema") = "Futuro" And lblStatoGraduatoria.Text = "Confermata" And Session("TipoUtente") = "E" Then
                dgRisultatoRicerca.Columns(23).Visible = True
                ControlloVolontariNew()
            End If
        End If

        If lblpresentata.Text = "3" Then
            'ControlloVolontari()
            ControlloVolontariNew()

        End If
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Cancella"
                'CONTROLLI
                'Elimino Cronologia Graduatorie Volontario
                strsql = "delete cronologiagraduatorieEntità where idgraduatoriaentità=" & dgRisultatoRicerca.Items(e.Item.ItemIndex).Cells(3).Text & ""
                cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                'Elimino Associazione Volontario
                strsql = "delete graduatorieEntità where idgraduatoriaentità=" & dgRisultatoRicerca.Items(e.Item.ItemIndex).Cells(3).Text & ""
                cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                CaricaDataGrid(dgRisultatoRicerca)
            Case "Documenti"

                Response.Write("<script>")
                Response.Write("window.open('WfrmAssociaDocumentiVolontari.aspx?VengoDa=Graduatoria&IdEntita=" & e.Item.Cells(24).Text & "&IdAttivita=" & Request.QueryString("idattivita") & "', 'Visualizza', 'width=800,height=600,dependent=no,scrollbars=yes,status=si')")
                Response.Write("</script>")
            Case "selezionaPdf"
                Dim selectedItem As DataGridItem = e.Item
                Dim objHLink As HyperLink
                'Dim IdVolontario As String = selectedItem.Cells(3).Text
                'hlDownload.Visible = True
                'hlDownload.NavigateUrl = RecuperaDocumentoDomandaOnLine(e.Item.Cells(14).Text, Session("Utente"))
                'hlDownload.Text = "pdf"
                'hlDownload.Target = "_blank"


                objHLink = RecuperaDocumentoDomandaOnLine(e.Item.Cells(34).Text, Session("Utente"))

                hlDownload.Visible = True
                hlDownload.Text = objHLink.Text
                hlDownload.NavigateUrl = objHLink.NavigateUrl
                'hlDw.Visible = False
                'imgEsporta.Visible = True
                hlDownload.Target = "_blank"
        End Select



    End Sub
    Private Function RecuperaDocumentoDomandaOnLine(ByVal DOL_id As Integer, ByVal user As String) As HyperLink
        Dim CONDOL As SqlConnection = Session("Conn")
        'Dim CONDOL As New SqlConnection

        'CONDOL.ConnectionString = "user id=" & AppSettings("DOL_USERNAME") & ";password=" & AppSettings("DOL_PASSWORD") & ";data source=" & AppSettings("DOL_DATA_SOURCE") & ";persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=" & AppSettings("DOL_NAME") & ""
        'CONDOL.Open()

        Dim da As New SqlDataAdapter _
             ("SELECT distinct filedomanda, CodiceProgettoSelezionato + '_' + convert(varchar(20),CodiceSedeSelezionata) +'_' + codicefiscale +'.pdf' as FileName FROM sqldol.domandaonline.dbo.domandapartecipazione WHERE id = " & DOL_id, CONDOL)
        '("SELECT distinct filedomanda, CodiceProgettoSelezionato + '_' + convert(varchar(20),CodiceSedeSelezionata) +'_' + codicefiscale +'.pdf' as FileName FROM domandapartecipazione WHERE id = " & DOL_id, CONDOL)
        Dim cb As SqlCommandBuilder = New SqlCommandBuilder(da)
        Dim ds As New DataSet
        Dim nomefile As String

        'Dim user As String
        Dim paht As String
        Try
            Dim oblLocalHLink As New HyperLink

            da.Fill(ds, "_FileTest")
            Dim rw As DataRow
            rw = ds.Tables("_FileTest").Rows(0)

            ' Make sure you have some rows
            Dim i As Integer = ds.Tables("_FileTest").Rows.Count
            If i > 0 Then
                Dim bBLOBStorage() As Byte = _
                ds.Tables("_FileTest").Rows(0)("filedomanda")

                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("Filename")
                nomefile = user & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now) & "_" & ds.Tables("_FileTest").Rows(0)("Filename")
                oblLocalHLink.NavigateUrl = FileByteToPathDomandeOnLine(bBLOBStorage, nomefile)

                paht = FileByteToPathDomandeOnLine(bBLOBStorage, nomefile)
            End If


            Return oblLocalHLink
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function
    Private Shared Function FileByteToPathDomandeOnLine(ByVal dataBuffer As Byte(), ByVal nomeFile As String) As String
        'dichiaro una variabile byte che bufferizza (carica in memoria) il file template richiesto
        'e trasformato in base64
        Dim fs As FileStream
        Dim myPath As New System.Web.UI.Page

        If File.Exists(myPath.Server.MapPath("download") & "\" & nomeFile) Then
            File.Delete(myPath.Server.MapPath("download") & "\" & nomeFile)
        End If
        'passo il template al filestream
        fs = New FileStream(myPath.Server.MapPath("download") & "\" & nomeFile, FileMode.Create, FileAccess.Write)
        'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
        If (dataBuffer.Length > 0) Then
            fs.Write(dataBuffer, 0, dataBuffer.Length)
        End If

        'chiudo lo streaming
        fs.Close()
        Return "download\" & nomeFile
    End Function
    Private Sub ModificaGraduatoria()
        'Eseguo Modifiche du graduatoriaEntità Ammessso Idoneo
        Dim item As DataGridItem
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = MyTransaction
            For Each item In dgRisultatoRicerca.Items
                'Idonei
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                Dim text As TextBox = DirectCast(item.FindControl("Text1"), TextBox)
                Dim ddl As DropDownList = DirectCast(item.FindControl("DDL1"), DropDownList)
                'se in Conferma Graduatoria 
                'If txtmodificato.Text = "Cambiato" Then
                If lblpresentata.Text = "3" Then
                    strsql = "insert into cronologiaGraduatorieEntità (idgraduatoriaEntità,identità," & _
                    " idAttivitàsedeassegnazione,punteggio,ordine,Ammesso,stato,idtipologiaposto,Username,dataModifica)" & _
                    " select idgraduatoriaEntità,identità, " & _
                    " idAttivitàsedeassegnazione,punteggio,ordine,Ammesso,stato,idtipologiaposto,Username,dataModifica from " & _
                    " graduatorieentità where idgraduatoriaentità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(3).Text & ""
                    'cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                    myCommand.CommandText = strsql
                    myCommand.ExecuteNonQuery()
                End If
                'End If
                strsql = " update GraduatorieEntità set "
                'Idoneo
                If check.Checked = True Then
                    strsql = strsql & "stato=1" ',punteggio=" & text.Text & " where idgraduatoriaentità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(2).Text & ""
                    If check2.Checked = True Then
                        strsql = strsql & ",ammesso=1"
                        strsql = strsql & ",idtipologiaposto=" & ddl.SelectedValue & ""
                    Else
                        strsql = strsql & ",ammesso=0"
                    End If
                Else
                    strsql = strsql & "stato=2,ammesso=0"
                End If
                strsql = strsql & ",punteggio=" & Replace(text.Text, ",", ".") & ",Username='" & Session("Utente") & "',datamodifica=getdate() "
                strsql = strsql & "where idgraduatoriaentità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(3).Text & ""
                'cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                myCommand.CommandText = strsql
                myCommand.ExecuteNonQuery()
            Next
            'aggiunto da Alessandra taballione il 18/04/2005
            'modifico la data di inizio attività e fine Attività
            Dim null As String
            null = "null"
            strsql = "Update attività set "
            If txtinizioEff.Text <> "" Then
                strsql = strsql & "datainizioAttività='" & txtinizioEff.Text & "',"
            Else
                strsql = strsql & "datainizioAttività=" & null & ","
            End If
            If txtFineEff.Text <> "" Then
                strsql = strsql & "datafineattività='" & txtFineEff.Text & "'"
            Else
                strsql = strsql & "datafineattività=" & null & ""
            End If
            strsql = strsql & " where idattività=" & lblidattivita.Text & " "

            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            MyTransaction.Commit()
        Catch ee As Exception
            imgmess.Visible = True
            imgmess.ImageUrl = "images/alert3.gif"
            lblMessaggi.Visible = True
            lblMessaggi.ForeColor = Color.Red
            lblMessaggi.Text = ee.Message.ToString
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            Exit Sub
        Finally
        End Try
        MyTransaction.Dispose()
        txtdatainizioDB.Text = txtinizioEff.Text
        'Eseguo StoreProcedure per ordinare la graduatoria per il punteggio Inserito.
        ClsServer.EseguiStoreOrdina(lblidattivitasedeassegnazione.Text, "SP_ORDINA_GRADUATORIA", Session("conn"))
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub

    Private Sub ChekIdonei()
        'Generato da Alessandra Taballione il 01/11/04
        'Valorizzazione nella dataGrid dei Check Idoneo e aAmmesso 
        'Per evidenziare se la Sede in Elenco è Propria, Inclusa e non Inclusa
        Dim item As DataGridItem
        Dim conta As Integer
        strsql = "Select idTipologiaPosto,Descrizione from Tipologieposto order by idTipologiaPosto desc "
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        conta = 0
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        For Each item In dgRisultatoRicerca.Items
            'Idoneo=1
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
            Dim text As TextBox = DirectCast(item.FindControl("Text1"), TextBox)
            Dim ddl As DropDownList = DirectCast(item.FindControl("DDL1"), DropDownList)
            conta = conta + 1
            'IDonei
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(15).Text = "1" Then
                check.Checked = True
            Else
                check.Checked = False
            End If
            'Ammessi
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(14).Text = "1" Then
                check2.Checked = True
            Else
                check2.Checked = False
            End If
            If text.Text = "" Then
                text.Text = 0
            End If
            'valore punteggio
            text.Text = dgRisultatoRicerca.Items(item.ItemIndex).Cells(16).Text
            'tipo posto
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            ddl.DataSource = dtrGenerico
            ddl.DataValueField = "idTipologiaPosto"
            ddl.DataTextField = "Descrizione"
            ddl.DataBind()
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text <> 0 Then
                ddl.SelectedValue = dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text
            End If
            dgRisultatoRicerca.Items(item.ItemIndex).Cells(1).Text = conta
        Next
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Sub ChekBloccaIdonei()
        'Generato da Alessandra Taballione il 01/04/04
        'Valorizzazione nella dataGrid del Check Inclusa 
        'Per evidenziare se la Sede in Elenco è Propria, Inclusa e non Inclusa
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            'Idoneo=1
            If item.Cells(30).Text <> "1" Or Session("TipoUtente") = "E" Then
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                Dim text As TextBox = DirectCast(item.FindControl("Text1"), TextBox)
                Dim ddl As DropDownList = DirectCast(item.FindControl("DDL1"), DropDownList)
                check2.Enabled = False
                check.Enabled = False
                'text.ReadOnly = True
                text.Enabled = False
                ddl.Enabled = False
            End If
            If item.Cells(30).Text = "1" And Session("TipoUtente") = "U" And imgConfermaRecupero.Visible = False Then
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                Dim text As TextBox = DirectCast(item.FindControl("Text1"), TextBox)
                Dim ddl As DropDownList = DirectCast(item.FindControl("DDL1"), DropDownList)
                check2.Enabled = False
                check.Enabled = False
                'text.ReadOnly = True
                text.Enabled = False
                ddl.Enabled = False
            End If
        Next
    End Sub

    Private Sub ChekControllaSalvataggio()
        'Generato da Alessandra Taballione il 01/04/04
        'Valorizzazione nella dataGrid del Check Inclusa 
        'Per evidenziare se la Sede in Elenco è Propria, Inclusa e non Inclusa
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            'Idoneo=1
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            If check.Checked = True And dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text = "2" Then
                lblMessaggioAlert.Visible = True
                'imgAlert.Visible = True
                lblMessaggioAlert.Text = "Per effettuare il salvataggio è necessario premere il tasto Salva."
                Exit Sub
            End If
        Next
        'lblMessaggioAlert.Text = "fatto"
    End Sub

    Sub Chiudi()
        If lblMessaggioAlert.Text = "" Or lblMessaggioAlert.Text = "fatto" Then
            'If lblpresentata.Text > 0 Then
            '    Response.Redirect("WfrmElencoSediAttuazione.aspx?IdAttivita=" & lblidattività.Text & "&Azione=Presentazione")
            'Else
            '    Response.Redirect("WfrmElencoSediAttuazione.aspx?IdAttivita=" & lblidattività.Text & "&Azione=Inserimento")
            'End If
            'PaginaGrid=" & request.querystring("PaginaGrid") & "&CodiceProgetto=" & request.querystring("CodiceProgetto") & "&InAttesa=" & request.querystring("InAttesa") & "&Area=" & request.querystring("Area") & "&Settore=" & request.querystring("Settore") & "&Bando=" & request.querystring("Bando") & "&Progetto=" & request.querystring("Progetto") & "&CodEnte=" & request.querystring("CodEnte") & "&Ente=" & request.querystring("Ente") & "&CheckIndietro=" & request.querystring("CheckIndietro") & "&

            Dim blnSegnalata As Boolean

            'controllo se la graduatoria è stata segnalata
            If imgSegnalaOk.ImageUrl = "images/vistabuona_small.png" Then
                blnSegnalata = False
            Else
                blnSegnalata = True
            End If
            Select Case lblpresentata.Text
                Case 0
                    Response.Redirect("WfrmElencoSediAttuazione.aspx?Segnalato=" & blnSegnalata & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&InAttesa=" & Request.QueryString("InAttesa") & "&Area=" & Request.QueryString("Area") & "&Settore=" & Request.QueryString("Settore") & "&Bando=" & Request.QueryString("Bando") & "&Progetto=" & Request.QueryString("Progetto") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Ente=" & Request.QueryString("Ente") & "&CheckIndietro=" & Request.QueryString("CheckIndietro") & "&IdAttivita=" & lblidattivita.Text & "&idente=" & txtidente.Text & "&Azione=Inserimento")
                Case 1
                    Response.Redirect("WfrmElencoSediAttuazione.aspx?Segnalato=" & blnSegnalata & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&InAttesa=" & Request.QueryString("InAttesa") & "&Area=" & Request.QueryString("Area") & "&Settore=" & Request.QueryString("Settore") & "&Bando=" & Request.QueryString("Bando") & "&Progetto=" & Request.QueryString("Progetto") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Ente=" & Request.QueryString("Ente") & "&CheckIndietro=" & Request.QueryString("CheckIndietro") & "&IdAttivita=" & lblidattivita.Text & "&idente=" & txtidente.Text & "&Azione=Presentazione")
                Case 2
                    Response.Redirect("WfrmElencoSediAttuazione.aspx?Segnalato=" & blnSegnalata & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&InAttesa=" & Request.QueryString("InAttesa") & "&Area=" & Request.QueryString("Area") & "&Settore=" & Request.QueryString("Settore") & "&Bando=" & Request.QueryString("Bando") & "&Progetto=" & Request.QueryString("Progetto") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Ente=" & Request.QueryString("Ente") & "&CheckIndietro=" & Request.QueryString("CheckIndietro") & "&IdAttivita=" & lblidattivita.Text & "&idente=" & txtidente.Text & "&Azione=Presentazione")
                Case 3
                    Response.Redirect("WfrmElencoSediAttuazione.aspx?Segnalato=" & blnSegnalata & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&InAttesa=" & Request.QueryString("InAttesa") & "&Area=" & Request.QueryString("Area") & "&Settore=" & Request.QueryString("Settore") & "&Bando=" & Request.QueryString("Bando") & "&Progetto=" & Request.QueryString("Progetto") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Ente=" & Request.QueryString("Ente") & "&CheckIndietro=" & Request.QueryString("CheckIndietro") & "&IdAttivita=" & lblidattivita.Text & "&idente=" & txtidente.Text & "&Azione=Conferma")
                Case 4
                    Response.Redirect("WfrmElencoSediAttuazione.aspx?Segnalato=" & blnSegnalata & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&InAttesa=" & Request.QueryString("InAttesa") & "&Area=" & Request.QueryString("Area") & "&Settore=" & Request.QueryString("Settore") & "&Bando=" & Request.QueryString("Bando") & "&Progetto=" & Request.QueryString("Progetto") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Ente=" & Request.QueryString("Ente") & "&CheckIndietro=" & Request.QueryString("CheckIndietro") & "&IdAttivita=" & lblidattivita.Text & "&idente=" & txtidente.Text & "&Azione=Visualizzazione")
            End Select
        Else
            ChekControllaSalvataggio()
        End If
    End Sub

    Private Sub ConfermaGraduatoria()
        'Realizzato da Alessandra Taballione aprile 2004
        'routine che effettua la modifica dell'ente
        Dim Null As String = "Null"
        cmdSalva.Visible = False
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = MyTransaction
            'inserisco cronologia stato graduatoria vecchio stato
            strsql = "Insert into CronologiaAttivitàSediAssegnazione (idAttivitàSedeAssegnazione,StatoGraduatoria," & _
            " usernamestato,datacronologia )" & _
            " select " & lblidattivitasedeassegnazione.Text & ",StatoGraduatoria,'" & Session("Utente") & "',getdate()" & _
            " from attivitàsediassegnazione where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'Modifico stato graduatoria inserendo chi ha effettuatio la modifica e la data
            strsql = "Update attivitàsediassegnazione set statograduatoria=3," & _
            " usernamestato='" & Session("Utente") & "',dataultimostato=getdate(), NoteGraduatoria='" & ClsServer.NoApice(txtNote.Text) & "'" & _
            " where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            '*********************************************
            'eseguito da jonadani spagnani il 26/11/2004
            'aggiorno cronologia volontari IDONEI e SELEZIONATI
            strsql = "insert into CronologiaEntità (IdEntità, IdStatoEntità, UsernameStato, DataChiusura, DataCronologia, NoteStato, IdCausaleChiusura) "
            strsql = strsql & "select entità.IdEntità, entità.IdStatoEntità, entità.UsernameStato, entità.DataChiusura, entità.DataUltimoStato, entità.NoteStato, entità.IdCausaleChiusura "
            strsql = strsql & "from entità "
            strsql = strsql & "inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità "
            strsql = strsql & "where Ammesso=1 and IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'aggiorno stato volontari IDONEI e SELEZIONATI
            'strsql = "update entità set IdStatoEntità=(select IdStatoEntità from StatiEntità where DaAllocare=1), "
            'Modificata da Alessandra Taballione il 30/05/2005
            'a seguito di interventi implementativi richiesti dalla dott.ssa Cagiati

            'Agg.il 17/04/2008 da simona cordella campo POSTOOCCUPATO = 1
            strsql = "update entità set IdStatoEntità=(select IdStatoEntità from StatiEntità where inservizio=1), "
            strsql = strsql & "UserNameStato='" & Session("Utente") & "',"
            strsql = strsql & "DataUltimoStato=GetDate(),DataInizioServizio='" & txtinizioEff.Text & "',DataFineServizio='" & txtFineEff.Text & "',PostoOccupato=1 "
            strsql = strsql & "from entità "
            strsql = strsql & "inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità "
            strsql = strsql & "where Ammesso=1 and IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            '***********
            'Implementata da Alessandra Taballione il 30/05/2005
            'a seguito di interventi implementativi richiesti dalla dott.ssa Cagiati**********************************
            strsql = "Update attivitàEntisediAttuazione set StatoAssegnazione=2 from attivitàEntisediAttuazione " & _
            " inner join Entità on attivitàEntisediAttuazione.IDEnteSedeAttuazione=Entità.Tmpidsedeattuazione " & _
            " Inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità " & _
            " where GraduatorieEntità.Ammesso=1 and GraduatorieEntità.IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'Insert solo idonei 
            Dim item As DataGridItem
            Dim idAttiivitàEntisediattuazione As String
            '******************
            For Each item In dgRisultatoRicerca.Items
                'Idonei
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                Dim text As TextBox = DirectCast(item.FindControl("Text1"), TextBox)
                Dim ddl As DropDownList = DirectCast(item.FindControl("DDL1"), DropDownList)
                ' se Idoneo o Selezionato
                If check.Checked = True And check2.Checked = True Then
                    'strsql = " Select idattivitàentesedeattuazione from attivitàentisediattuazione where idattività=" & lblidattivita.Text & " and IDEnteSedeAttuazione=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(22).Text & ""
                    'If Not dtrGenerico Is Nothing Then
                    '    dtrGenerico.Close()
                    '    dtrGenerico = Nothing
                    'End If
                    ''
                    'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    'If dtrGenerico.HasRows = True Then
                    '    dtrGenerico.Read()
                    '    idAttiivitàEntisediattuazione = dtrGenerico("idattivitàentesedeattuazione")
                    '    If Not dtrGenerico Is Nothing Then
                    '        dtrGenerico.Close()
                    '        dtrGenerico = Nothing
                    '    End If
                    'End If
                    'strsql = " Insert into AttivitàEntità (IDAttivitàEnteSedeAttuazione,IDEntità,DataInizioAttivitàEntità," & _
                    '" DataFineAttivitàEntità,IdStatoAttivitàEntità,PercentualeUtilizzo,IDTipologiaPosto)Values " & _
                    '" (" & idAttiivitàEntisediattuazione & "," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & ", " & _
                    '"'" & txtinizioEff.Text & "','" & txtFineEff.Text & "',1,100," & ddl.SelectedValue & ") "
                    'cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                    strsql = " Insert into AttivitàEntità (IDAttivitàEnteSedeAttuazione,IDEntità,DataInizioAttivitàEntità," & _
                                     " DataFineAttivitàEntità,IdStatoAttivitàEntità,PercentualeUtilizzo,IDTipologiaPosto) " & _
                                     " select idattivitàentesedeattuazione ," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & ", " & _
                                     "'" & txtinizioEff.Text & "','" & txtFineEff.Text & "',1,100," & ddl.SelectedValue & " " & _
                                     " from attivitàentisediattuazione where idattività=" & lblidattivita.Text & " and IDEnteSedeAttuazione=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(22).Text & "  "
                    myCommand.CommandText = strsql
                    myCommand.ExecuteNonQuery()

                    'Controllo se il codice del volntario è già presente
                    If VerificaCodiceVolontario(dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text) = False Then
                        'Aggiunto da Alessandra Taballione il 09/06/2005


                        strsql = " UPDATE ENTITÀ SET UserName= dbo.FN_CalcoloUsernameVolontario(" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "),  " & _
                                 " password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                                 " (SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                                 " CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                                 " CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                                  " FROM ENTITÀ WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                                 " WHERE identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "


                        'If CInt(dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text) <= 999999 Then
                        '    strsql = "update entità set username=(SELECT 'V' + left(codicefiscale,3) + replicate('0',6-len(convert(varchar,identità))) + convert(varchar, identità) from entità where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "), password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                        '         "(SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                        '         "CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                        '         "CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                        '         "From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                        '         "where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "
                        'Else
                        '    strsql = "update entità set username=(SELECT 'V' + left(codicefiscale,3) + replicate('0',7-len(convert(varchar,identità))) + 'A'+ RIGHT(convert(varchar, identità),5) from entità where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "), password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                        '         "(SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                        '         "CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                        '         "CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                        '         "From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                        '         "where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "
                        'End If



                        'gennaio 2006, codice volontario sostituito
                        '"(select 'V'+ convert(nvarchar(4),year(getdate()))+ convert(varchar(6)," & _
                        '" replicate('0', 6-len( convert(varchar(6),max(convert(int,right(isnull(codicevolontario,0),6))+1))))) + " & _
                        '" Convert(varchar(6), max(Convert(Int, Right(isnull(codicevolontario, 0), 6)) + 1))" & _
                        '" from entità)" & _

                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        myCommand.CommandText = strsql
                        myCommand.ExecuteNonQuery()

                    End If
                End If
            Next
            MyTransaction.Commit()
        Catch ex As Exception
            'Response.Write(strsql)
            'Response.Write("<br>")
            Response.Write(ex.Message.ToString)
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            MyTransaction.Dispose()
            MyTransaction = Nothing
            cmdSalva.Visible = True
            Exit Sub
        End Try
        MyTransaction.Dispose()
        MyTransaction = Nothing
        '********************
        lblStatoGraduatoria.Text = "Confermata"
        cmdConferma.Visible = False
        imgmess.Visible = True
        lblMessaggi.Visible = True

        cmdAggiungi.Visible = False
        cmdSalva.Visible = False
        lblMessaggi.Text = "La Graduatoria è stata Confermata con successo."
    End Sub

    Private Sub ConfermaGraduatoriaDifferita()
        'Realizzato da Alessandra Taballione aprile 2004
        'routine che effettua la modifica dell'ente
        Dim Null As String = "Null"
        cmdSalva.Visible = False
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = MyTransaction
            'inserisco cronologia stato graduatoria vecchio stato
            strsql = "Insert into CronologiaAttivitàSediAssegnazione (idAttivitàSedeAssegnazione,StatoGraduatoria," & _
            " usernamestato,datacronologia )" & _
            " select " & lblidattivitasedeassegnazione.Text & ",StatoGraduatoria,'" & Session("Utente") & "',getdate()" & _
            " from attivitàsediassegnazione where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'Modifico stato graduatoria inserendo chi ha effettuatio la modifica e la data
            strsql = "Update attivitàsediassegnazione set statograduatoria=3," & _
            " usernamestato='" & Session("Utente") & "',dataultimostato=getdate(), NoteGraduatoria='" & ClsServer.NoApice(txtNote.Text) & "'" & _
            " ,datainiziodifferita='" & txtInizioServizioSede.Text & "', datafinedifferita = '" & txtFineServizioSede.Text & "'" & _
            " where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            '*********************************************
            'eseguito da jonadani spagnani il 26/11/2004
            'aggiorno cronologia volontari IDONEI e SELEZIONATI
            strsql = "insert into CronologiaEntità (IdEntità, IdStatoEntità, UsernameStato, DataChiusura, DataCronologia, NoteStato, IdCausaleChiusura) "
            strsql = strsql & "select entità.IdEntità, entità.IdStatoEntità, entità.UsernameStato, entità.DataChiusura, entità.DataUltimoStato, entità.NoteStato, entità.IdCausaleChiusura "
            strsql = strsql & "from entità "
            strsql = strsql & "inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità "
            strsql = strsql & "where Ammesso=1 and IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'aggiorno stato volontari IDONEI e SELEZIONATI
            'strsql = "update entità set IdStatoEntità=(select IdStatoEntità from StatiEntità where DaAllocare=1), "
            'Modificata da Alessandra Taballione il 30/05/2005
            'a seguito di interventi implementativi richiesti dalla dott.ssa Cagiati

            'Agg.il 17/04/2008 da simona cordella campo POSTOOCCUPATO = 1
            strsql = "update entità set IdStatoEntità=(select IdStatoEntità from StatiEntità where inservizio=1), "
            strsql = strsql & "UserNameStato='" & Session("Utente") & "',"
            strsql = strsql & "DataUltimoStato=GetDate(),DataInizioServizio='" & txtInizioServizioSede.Text & "',DataFineServizio='" & txtFineServizioSede.Text & "',PostoOccupato=1 "
            strsql = strsql & "from entità "
            strsql = strsql & "inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità "
            strsql = strsql & "where Ammesso=1 and IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            '***********
            'Implementata da Alessandra Taballione il 30/05/2005
            'a seguito di interventi implementativi richiesti dalla dott.ssa Cagiati**********************************
            strsql = "Update attivitàEntisediAttuazione set StatoAssegnazione=2 from attivitàEntisediAttuazione " & _
            " inner join Entità on attivitàEntisediAttuazione.IDEnteSedeAttuazione=Entità.Tmpidsedeattuazione " & _
            " Inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità " & _
            " where GraduatorieEntità.Ammesso=1 and GraduatorieEntità.IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'Insert solo idonei 
            Dim item As DataGridItem
            Dim idAttiivitàEntisediattuazione As String
            '******************
            For Each item In dgRisultatoRicerca.Items
                'Idonei
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                Dim text As TextBox = DirectCast(item.FindControl("Text1"), TextBox)
                Dim ddl As DropDownList = DirectCast(item.FindControl("DDL1"), DropDownList)
                ' se Idoneo o Selezionato
                If check.Checked = True And check2.Checked = True Then
                    'strsql = " Select idattivitàentesedeattuazione from attivitàentisediattuazione where idattività=" & lblidattivita.Text & " and IDEnteSedeAttuazione=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(22).Text & ""
                    'If Not dtrGenerico Is Nothing Then
                    '    dtrGenerico.Close()
                    '    dtrGenerico = Nothing
                    'End If
                    ''
                    'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    'If dtrGenerico.HasRows = True Then
                    '    dtrGenerico.Read()
                    '    idAttiivitàEntisediattuazione = dtrGenerico("idattivitàentesedeattuazione")
                    '    If Not dtrGenerico Is Nothing Then
                    '        dtrGenerico.Close()
                    '        dtrGenerico = Nothing
                    '    End If
                    'End If
                    'strsql = " Insert into AttivitàEntità (IDAttivitàEnteSedeAttuazione,IDEntità,DataInizioAttivitàEntità," & _
                    '" DataFineAttivitàEntità,IdStatoAttivitàEntità,PercentualeUtilizzo,IDTipologiaPosto)Values " & _
                    '" (" & idAttiivitàEntisediattuazione & "," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & ", " & _
                    '"'" & txtinizioEff.Text & "','" & txtFineEff.Text & "',1,100," & ddl.SelectedValue & ") "
                    'cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                    strsql = " Insert into AttivitàEntità (IDAttivitàEnteSedeAttuazione,IDEntità,DataInizioAttivitàEntità," & _
                                     " DataFineAttivitàEntità,IdStatoAttivitàEntità,PercentualeUtilizzo,IDTipologiaPosto) " & _
                                     " select idattivitàentesedeattuazione ," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & ", " & _
                                     "'" & txtInizioServizioSede.Text & "','" & txtFineServizioSede.Text & "',1,100," & ddl.SelectedValue & " " & _
                                     " from attivitàentisediattuazione where idattività=" & lblidattivita.Text & " and IDEnteSedeAttuazione=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(22).Text & "  "
                    myCommand.CommandText = strsql
                    myCommand.ExecuteNonQuery()

                    'Controllo se il codice del volntario è già presente
                    If VerificaCodiceVolontario(dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text) = False Then
                        'Aggiunto da Alessandra Taballione il 09/06/2005


                        strsql = " UPDATE ENTITÀ SET UserName= dbo.FN_CalcoloUsernameVolontario(" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "),  " & _
                                 " password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                                 " (SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                                 " CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                                 " CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                                  " FROM ENTITÀ WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                                 " WHERE identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "


                        'If CInt(dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text) <= 999999 Then
                        '    strsql = "update entità set username=(SELECT 'V' + left(codicefiscale,3) + replicate('0',6-len(convert(varchar,identità))) + convert(varchar, identità) from entità where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "), password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                        '         "(SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                        '         "CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                        '         "CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                        '         "From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                        '         "where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "
                        'Else
                        '    strsql = "update entità set username=(SELECT 'V' + left(codicefiscale,3) + replicate('0',7-len(convert(varchar,identità))) + 'A'+ RIGHT(convert(varchar, identità),5) from entità where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "), password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                        '         "(SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                        '         "CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                        '         "CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                        '         "From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                        '         "where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "
                        'End If



                        'gennaio 2006, codice volontario sostituito
                        '"(select 'V'+ convert(nvarchar(4),year(getdate()))+ convert(varchar(6)," & _
                        '" replicate('0', 6-len( convert(varchar(6),max(convert(int,right(isnull(codicevolontario,0),6))+1))))) + " & _
                        '" Convert(varchar(6), max(Convert(Int, Right(isnull(codicevolontario, 0), 6)) + 1))" & _
                        '" from entità)" & _

                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        myCommand.CommandText = strsql
                        myCommand.ExecuteNonQuery()

                    End If
                End If
            Next
            MyTransaction.Commit()
        Catch ex As Exception
            'Response.Write(strsql)
            'Response.Write("<br>")
            Response.Write(ex.Message.ToString)
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            MyTransaction.Dispose()
            MyTransaction = Nothing
            cmdSalva.Visible = True
            Exit Sub
        End Try
        MyTransaction.Dispose()
        MyTransaction = Nothing
        '********************
        lblStatoGraduatoria.Text = "Confermata"
        cmdConferma.Visible = False
        imgmess.Visible = True
        lblMessaggi.Visible = True

        cmdAggiungi.Visible = False
        cmdSalva.Visible = False
        lblMessaggi.Text = "La Graduatoria è stata Confermata con successo."
    End Sub
    Private Function VerificaCodiceVolontario(ByVal IDEntità As Integer) As Boolean
        'creata il 10/10/2006 da Simona Cordella
        'Controlla se il volontario è già stato assegnato un codice
        Dim strCodice As String
        Dim dtrCodice As SqlClient.SqlDataReader

        strCodice = "select Identità,CodiceVolontario from entità where identità ='" & IDEntità & "' "
        myCommand.CommandText = strCodice
        dtrCodice = myCommand.ExecuteReader
        'leggo il datareader
        dtrCodice.Read()
        If IsDBNull(dtrCodice("CodiceVolontario")) = False Then 'se esiste =TRUE
            VerificaCodiceVolontario = True
        Else 'se non esiste =FALSE inserisco il nuovo codice
            VerificaCodiceVolontario = False
        End If
        If Not dtrCodice Is Nothing Then
            dtrCodice.Close()
            dtrCodice = Nothing
        End If
    End Function

    Private Sub ConfermaGraduatoriaOld()
        'inserisco cronologia stato graduatoria vecchio stato
        strsql = "Insert into CronologiaAttivitàSediAssegnazione (idAttivitàSedeAssegnazione,StatoGraduatoria," & _
        " usernamestato,datacronologia )" & _
        " select " & lblidattivitasedeassegnazione.Text & ",StatoGraduatoria,'" & Session("Utente") & "',getdate()" & _
        " from attivitàsediassegnazione where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
        myCommand.CommandText = strsql
        myCommand.ExecuteNonQuery()
        'Modifico stato graduatoria inserendo chi ha effettuatio la modifica e la data
        strsql = "Update attivitàsediassegnazione set statograduatoria=3," & _
        " usernamestato='" & Session("Utente") & "',dataultimostato=getdate(), NoteGraduatoria='" & ClsServer.NoApice(txtNote.Text) & "'" & _
        " where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
        myCommand.CommandText = strsql
        myCommand.ExecuteNonQuery()
        '*********************************************
        'eseguito da jonadani spagnani il 26/11/2004
        'aggiorno cronologia volontari IDONEI e SELEZIONATI
        strsql = "insert into CronologiaEntità (IdEntità, IdStatoEntità, UsernameStato, DataChiusura, DataCronologia, NoteStato, IdCausaleChiusura) "
        strsql = strsql & "select entità.IdEntità, entità.IdStatoEntità, entità.UsernameStato, entità.DataChiusura, entità.DataUltimoStato, entità.NoteStato, entità.IdCausaleChiusura "
        strsql = strsql & "from entità "
        strsql = strsql & "inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità "
        strsql = strsql & "where Ammesso=1 and IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
        myCommand.CommandText = strsql
        myCommand.ExecuteNonQuery()
        'aggiorno stato volontari IDONEI e SELEZIONATI
        'strsql = "update entità set IdStatoEntità=(select IdStatoEntità from StatiEntità where DaAllocare=1), "
        'Modificata da Alessandra Taballione il 30/05/2005
        'a seguito di interventi implementativi richiesti dalla dott.ssa Cagiati
        strsql = "update entità set IdStatoEntità=(select IdStatoEntità from StatiEntità where inservizio=1), "
        strsql = strsql & "UserNameStato='" & Session("Utente") & "',"
        strsql = strsql & "DataUltimoStato=GetDate(),DataInizioServizio='" & txtinizioEff.Text & "',DataFineServizio='" & txtFineEff.Text & "' "
        strsql = strsql & "from entità "
        strsql = strsql & "inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità "
        strsql = strsql & "where Ammesso=1 and IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
        myCommand.CommandText = strsql
        myCommand.ExecuteNonQuery()
        '***********
        'Implementata da Alessandra Taballione il 30/05/2005
        'a seguito di interventi implementativi richiesti dalla dott.ssa Cagiati**********************************
        strsql = "Update attivitàEntisediAttuazione set StatoAssegnazione=2 from attivitàEntisediAttuazione " & _
        " inner join Entità on attivitàEntisediAttuazione.IDEnteSedeAttuazione=Entità.Tmpidsedeattuazione " & _
        " Inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità " & _
        " where GraduatorieEntità.Ammesso=1 and GraduatorieEntità.IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
        myCommand.CommandText = strsql
        myCommand.ExecuteNonQuery()
        'Insert solo idonei 
        Dim item As DataGridItem
        Dim idAttiivitàEntisediattuazione As String
        '******************
        For Each item In dgRisultatoRicerca.Items
            'Idonei
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
            Dim text As TextBox = DirectCast(item.FindControl("Text1"), TextBox)
            Dim ddl As DropDownList = DirectCast(item.FindControl("DDL1"), DropDownList)
            ' se Idoneo o Selezionato
            If check.Checked = True And check2.Checked = True Then
                'strsql = " Select idattivitàentesedeattuazione from attivitàentisediattuazione where idattività=" & lblidattivita.Text & " and IDEnteSedeAttuazione=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(22).Text & ""
                'If Not dtrGenerico Is Nothing Then
                '    dtrGenerico.Close()
                '    dtrGenerico = Nothing
                'End If
                ''
                'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                'If dtrGenerico.HasRows = True Then
                '    dtrGenerico.Read()
                '    idAttiivitàEntisediattuazione = dtrGenerico("idattivitàentesedeattuazione")
                '    If Not dtrGenerico Is Nothing Then
                '        dtrGenerico.Close()
                '        dtrGenerico = Nothing
                '    End If
                'End If
                strsql = " Insert into AttivitàEntità (IDAttivitàEnteSedeAttuazione,IDEntità,DataInizioAttivitàEntità," & _
                " DataFineAttivitàEntità,IdStatoAttivitàEntità,PercentualeUtilizzo,IDTipologiaPosto) " & _
                " select idattivitàentesedeattuazione ," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & ", " & _
                "'" & txtinizioEff.Text & "','" & txtFineEff.Text & "',1,100," & ddl.SelectedValue & " " & _
                " from attivitàentisediattuazione where idattività=" & lblidattivita.Text & " and IDEnteSedeAttuazione=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(22).Text & "  "
                myCommand.CommandText = strsql
                myCommand.ExecuteNonQuery()



                If dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text < 999999 Then

                    'Aggiunto da Alessandra Taballione il 09/06/2005 mod il 17/06/2016
                    strsql = "update entità set username=(SELECT 'V' + left(codicefiscale,3) + replicate('0',6-len(convert(varchar,identità))) + convert(varchar, identità) from entità where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "), password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                    "(SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                    "CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                    "CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                    "From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                    "where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "
                Else
                    strsql = "update entità set username=(SELECT 'V' + left(codicefiscale,3) + replicate('0',7-len(convert(varchar,identità))) + 'A'+ RIGHT(convert(varchar, identità),5) from entità where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "), password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                    "(SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                    "CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                    "CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                    "From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                    "where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "

                End If
                'gennaio 2006, codice volontario vecchio
                '"(select 'V'+ convert(nvarchar(4),year(getdate()))+ convert(varchar(6)," & _
                '" replicate('0', 6-len( convert(varchar(6),max(convert(int,right(isnull(codicevolontario,0),6))+1))))) + " & _
                '" Convert(varchar(6), max(Convert(Int, Right(isnull(codicevolontario, 0), 6)) + 1))" & _
                '" from entità)" & _
                myCommand.CommandText = strsql
                myCommand.ExecuteNonQuery()
            End If
        Next
        '********************
        lblStatoGraduatoria.Text = "Confermata"
        cmdConferma.Visible = False
        imgmess.Visible = True
        lblMessaggi.Visible = True

        cmdAggiungi.Visible = False
        cmdSalva.Visible = False
        lblMessaggi.Text = "La Graduatoria è stata Confermata con successo."
    End Sub

    Private Function GeneraCodiceVolontario()
        Dim appo As String
        Dim i As Integer
        Dim x As Integer
        strsql = "select isnull(max(codiceVolontario),0)as codiceVol,year(getdate()) as data from entità"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            appo = dtrGenerico("data")
            appo = appo & "V"
            If dtrGenerico("codiceVol") = "0" Then
                appo = appo & "000001"
            Else
                '6
                i = 6 - Len(CStr(dtrGenerico("codicevol")))
                For x = i To 6
                    appo = appo & "0"
                Next
            End If
        End If
        GeneraCodiceVolontario = appo
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Function

    Private Sub RespingiGraduatoria()

        'inserisco cronologia stato graduatoria vecchio stato
        strsql = "Insert into CronologiaAttivitàSediAssegnazione (idAttivitàSedeAssegnazione,StatoGraduatoria," & _
        " usernamestato,datacronologia )" & _
        " select " & lblidattivitasedeassegnazione.Text & ",StatoGraduatoria,'" & Session("Utente") & "',getdate()" & _
        " from attivitàsediassegnazione where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
        cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        'Modifico stato graduatoria inserendo chi ha effettuatio la modifica e la data
        strsql = "Update attivitàsediassegnazione set statograduatoria=4," & _
        " usernamestato='" & Session("Utente") & "',dataultimostato=getdate(), NoteGraduatoria='" & ClsServer.NoApice(txtNote.Text) & "'" & _
        " where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
        cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        lblStatoGraduatoria.Text = "Respinta"
        cmdConferma.Visible = False
        cmdRespingi.Visible = False
        imgmess.Visible = True
        lblMessaggi.Visible = True
        lblMessaggi.Text = "La Graduatoria è stata Respinta."
    End Sub

    Private Function ContaVol() As Boolean
        'Aggiunto da Alessandra Taballione il 09/05/2005
        'Controllo Volontari Inseriti e Selezionati <= Totale Volontari Progetto
        Dim IntVolAmm As Integer
        Dim IntVolRic As Integer
        strsql = "SELECT  ISNULL(SUM(s2.NumeroPostiNoVittoNoAlloggio + s2.NumeroPostiVittoAlloggio + " & _
        " s2.NumeroPostiVitto), 0) as VolRic " & _
        " FROM  attività S2 " & _
        " WHERE S2.idattività=" & lblidattivita.Text & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()
        IntVolRic = dtrGenerico("VolRic")
        strsql = " SELECT COUNT(*) as VolAmm " & _
        " FROM graduatorieentità s1 " & _
        " inner join entità on entità.identità=s1.identità" & _
        " inner join statientità on entità.idstatoentità=statientità.idstatoentità" & _
        " WHERE s1.idattivitàsedeassegnazione in (select attivitàsediassegnazione.idattivitàsedeassegnazione  " & _
        " from attivitàsediassegnazione where attivitàsediassegnazione.idattività=" & lblidattivita.Text & " and identesede <> " & lblidSede.Text & " )" & _
        " and ammesso=1 and (statientità.defaultstato=1 or statientità.inservizio=1) "
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()
        IntVolAmm = dtrGenerico("VolAmm")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
            If check2.Checked = True And item.Cells(28).Text <> 4 And item.Cells(28).Text <> 5 Then
                IntVolAmm = IntVolAmm + 1
            End If
        Next
        If IntVolAmm > IntVolRic Then
            ContaVol = False
        Else
            ContaVol = True
        End If
    End Function
    Private Function ContaGMO() As String
        'aggiunto da simona cordella il 18/07/2018
        'conto quanti volontari GMO sono previsti sul progetto e vefico quanti ne sono in graduatoria
        Dim IntVolAmmGMO As Integer
        Dim IntVolRicGMO As Integer
        Dim IntVolAmmNOGMO As Integer
        Dim IntVolRicNOGMO As Integer
        Dim msg As String = ""


        strsql = "select d.NumeroPostiNoVittoNoAlloggio + d.NumeroPostiVittoAlloggio + d.NumeroPostiVitto as NPosti, d.numeropostiGMO as NGMO from AttivitàSediAssegnazione a " & _
                 "inner join entisedi b on a.IDEnteSede = b.IDEnteSede " & _
                 "inner join entisediattuazioni c on b.identesede = c.IDEnteSede " & _
                 "inner join attivitàentisediattuazione d on c.IDEnteSedeAttuazione = d.IDEnteSedeAttuazione and a.IDAttività = d.IDAttività " & _
                 "where a.IDAttivitàSedeAssegnazione = " & lblidattivitasedeassegnazione.Text
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()
        IntVolRicGMO = dtrGenerico("NGMO")
        IntVolRicNOGMO = dtrGenerico("NPosti") - IntVolRicGMO
        IntVolAmmGMO = 0
        IntVolAmmNOGMO = 0
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
            If check2.Checked = True And item.Cells(28).Text <> 4 And item.Cells(28).Text <> 5 _
                And item.Cells(31).Text = 1 Then
                IntVolAmmGMO = IntVolAmmGMO + 1
            End If
            If check2.Checked = True And item.Cells(28).Text <> 4 And item.Cells(28).Text <> 5 _
                And item.Cells(31).Text = 0 Then
                IntVolAmmNOGMO = IntVolAmmNOGMO + 1
            End If
        Next

        'rimosso controllo su posti non GMO richiesto dal Dipartimento
        'If IntVolAmmNOGMO > IntVolRicNOGMO Then
        '    msg = "Attenzione. Superato il numero di volontari NON GMO previsti dalla sede di progetto."
        'End If
        'fine rimozione
        Return msg


        'strsql = "SELECT  ISNULL(NumeroGiovaniMinoriOpportunità,0) as GMO " & _
        '" FROM  attività S2 " & _
        '" WHERE S2.idattività=" & lblidattivita.Text & ""
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If
        'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        'dtrGenerico.Read()
        'IntVolRicGMO = dtrGenerico("GMO")

        'strsql = "SELECT  (ISNULL(SUM(s2.NumeroPostiNoVittoNoAlloggio + s2.NumeroPostiVittoAlloggio + " & _
        '         " s2.NumeroPostiVitto), 0) )as NOGMO " & _
        '         " FROM  attività S2 " & _
        '         " WHERE S2.idattività=" & lblidattivita.Text & ""
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If
        'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        'dtrGenerico.Read()
        'IntVolRicNOGMO = dtrGenerico("NOGMO") - IntVolRicGMO


        'strsql = " SELECT COUNT(*) as VolAmmGMO " & _
        '" FROM graduatorieentità s1 " & _
        '" inner join entità on entità.identità=s1.identità" & _
        '" inner join statientità on entità.idstatoentità=statientità.idstatoentità" & _
        '" WHERE s1.idattivitàsedeassegnazione in (select attivitàsediassegnazione.idattivitàsedeassegnazione  " & _
        '" from attivitàsediassegnazione where attivitàsediassegnazione.idattività=" & lblidattivita.Text & " and identesede <> " & lblidSede.Text & " )" & _
        '" and ammesso=1 and (statientità.defaultstato=1 or statientità.inservizio=1) and isnull(entità.gmo,'') <> '' "
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If
        'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        'dtrGenerico.Read()
        'IntVolAmmGMO = dtrGenerico("VolAmmGMO")
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If

        'strsql = " SELECT COUNT(*) as VolAmmNOGMO " & _
        '        " FROM graduatorieentità s1 " & _
        '        " inner join entità on entità.identità=s1.identità" & _
        '        " inner join statientità on entità.idstatoentità=statientità.idstatoentità" & _
        '        " WHERE s1.idattivitàsedeassegnazione in (select attivitàsediassegnazione.idattivitàsedeassegnazione  " & _
        '        " from attivitàsediassegnazione where attivitàsediassegnazione.idattività=" & lblidattivita.Text & " and identesede <> " & lblidSede.Text & " )" & _
        '        " and ammesso=1 and (statientità.defaultstato=1 or statientità.inservizio=1) and isnull(entità.gmo,'') = '' "
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If
        'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        'dtrGenerico.Read()
        'IntVolAmmNOGMO = dtrGenerico("VolAmmNOGMO")
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If


        'Dim item As DataGridItem
        'For Each item In dgRisultatoRicerca.Items
        '    Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
        '    If check2.Checked = True And item.Cells(28).Text <> 4 And item.Cells(28).Text <> 5 _
        '        And item.Cells(31).Text = 1 Then
        '        IntVolAmmGMO = IntVolAmmGMO + 1
        '    End If
        '    If check2.Checked = True And item.Cells(28).Text <> 4 And item.Cells(28).Text <> 5 _
        '        And item.Cells(31).Text = 0 Then
        '        IntVolAmmNOGMO = IntVolAmmNOGMO + 1
        '    End If
        'Next
        'If IntVolAmmGMO > IntVolRicGMO Then
        '    msg = "Attenzione. Superato il numero di volontari GMO previsti dal progetto."
        'End If
        'If IntVolAmmNOGMO > IntVolRicNOGMO Then
        '    msg = "Attenzione. Superato il numero di volontari non GMO previsti dal progetto."
        'End If
        'Return msg
    End Function


    Private Function ContaFAMI() As String
        'aggiunto da simona cordella il 20/07/2018
        'conto quanti volontari FAMI sono previsti sulla sede di progetto e vefico quanti ne sono in graduatoria
        Dim IntVolAmmFAMI As Integer = 0
        Dim IntVolRicFAMI As Integer = 0
        Dim IntVolAmmNOFAMI As Integer = 0
        Dim IntVolRicNOFAMI As Integer = 0
        Dim msg As String = ""

        strsql = "SELECT  ISNULL(a.NumeroPostiFami,0) as FAMI " & _
                 " FROM  attivitàentisediattuazione a   " & _
                 " INNER JOIN entisediattuazioni e ON a.IDEnteSedeAttuazione = e.IDEnteSedeAttuazione " & _
                 " WHERE a.IDAttività = " & lblidattivita.Text & " And e.IDEnteSede = " & lblidSede.Text & ""



        'IDEnteSedeAttuazione
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()
        IntVolRicFAMI = dtrGenerico("FAMI")

        strsql = "SELECT  (ISNULL(SUM(a.NumeroPostiNoVittoNoAlloggio + a.NumeroPostiVittoAlloggio + a.NumeroPostiVitto), 0)) as NOFAMI " & _
                 " FROM  attivitàentisediattuazione a   " & _
                 " INNER JOIN entisediattuazioni e ON a.IDEnteSedeAttuazione = e.IDEnteSedeAttuazione " & _
                 " WHERE a.IDAttività = " & lblidattivita.Text & " And e.IDEnteSede = " & lblidSede.Text & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()
        IntVolRicNOFAMI = dtrGenerico("NOFAMI") - IntVolRicFAMI



        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If


        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
            If check2.Checked = True And item.Cells(28).Text <> 4 And item.Cells(28).Text <> 5 _
                And item.Cells(32).Text = 1 Then
                IntVolAmmFAMI = IntVolAmmFAMI + 1
            End If
            If check2.Checked = True And item.Cells(28).Text <> 4 And item.Cells(28).Text <> 5 _
                And item.Cells(32).Text = 0 Then
                IntVolAmmNOFAMI = IntVolAmmNOFAMI + 1
            End If
        Next
        If IntVolAmmFAMI > IntVolRicFAMI Then
            msg = "Attenzione. Superato il numero di volontari FAMI previsti dalla sede."
        End If
        If IntVolAmmNOFAMI > IntVolRicNOFAMI Then
            msg = "Attenzione. Superato il numero di volontari non FAMI previsti dalla sede."
        End If
        Return msg
    End Function



    Function Elencovolontariammessi(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim arrayIdVol As New ArrayList

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'creo il nome del file
            strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
            'creo il percorso del file da salvare
            strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

            'apro il file che fa da template
            Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))

            Writer = New StreamWriter(strPercorsoFile)

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If


            strsql = "SELECT DISTINCT "
            strsql = strsql & " isnull(upper(replace(replace(replace(replace(replace(replace(replace(attività.Titolo,'°',''),'ì','i'''),'é','e'''),"
            strsql = strsql & "'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '')as Titolo,"
            strsql = strsql & " isnull(attività.CodiceEnte, 'Nessun Codice') as codiceprogetto, "
            strsql = strsql & "isnull(case len(day(attività.datainizioattività)) when 1 then '0' + convert(varchar(20),day(attività.datainizioattività)) "
            strsql = strsql & "else convert(varchar(20),day(attività.datainizioattività))  end + '/' + "
            strsql = strsql & "(case len(month(attività.datainizioattività)) when 1 then '0' + convert(varchar(20),month(attività.datainizioattività)) "
            strsql = strsql & "else convert(varchar(20),month(attività.datainizioattività))  end + '/' + "
            strsql = strsql & "Convert(varchar(20), Year(attività.datainizioattività))),'XX/XX/XXXX') as DataInizio, "
            'strsql = strsql & "comuni.Denominazione as Comune "
            strsql = strsql & " isnull(upper(replace(replace(replace(replace(replace(replace(replace(comuni.Denominazione,'°',''),'ì','i'''),'é','e'''),"
            strsql = strsql & "'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '')as Comune "
            strsql = strsql & "FROM attività INNER JOIN "
            strsql = strsql & "AttivitàSediAssegnazione ON attività.IDAttività = AttivitàSediAssegnazione.IDAttività INNER JOIN "
            strsql = strsql & "entisedi ON AttivitàSediAssegnazione.IDEnteSede = entisedi.IDEnteSede INNER JOIN "
            strsql = strsql & "comuni ON entisedi.IDComune = comuni.IDComune INNER JOIN "
            strsql = strsql & "GraduatorieEntità ON AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione = GraduatorieEntità.IdAttivitàSedeAssegnazione INNER JOIN "
            strsql = strsql & "entità ON GraduatorieEntità.IdEntità = entità.IDEntità "
            strsql = strsql & "WHERE AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione='" & Request.QueryString("IDAttivitasedeAssegnazione") & "'"

            'strsql = "select titolo as titolo, "
            'strsql = strsql & "isnull(codiceente,'Nessun Codice') as codiceprogetto, "
            'strsql = strsql & "isnull(case len(day(datainizioattività)) when 1 then '0' + convert(varchar(20),day(datainizioattività)) "
            'strsql = strsql & "else convert(varchar(20),day(datainizioattività))  end + '/' + "
            'strsql = strsql & "(case len(month(datainizioattività)) when 1 then '0' + convert(varchar(20),month(datainizioattività)) "
            'strsql = strsql & "else convert(varchar(20),month(datainizioattività))  end + '/' + "
            'strsql = strsql & "Convert(varchar(20), Year(datainizioattività))),'XX/XX/XXXX') as DataInizio "
            'strsql = strsql & "from attività "
            'strsql = strsql & "where idattività='" & lblidattivita.Text & "'"

            'eseguo la query e passo il risultato al datareader
            dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()

                'Writer.WriteLine("{\rtf1")

                'apro il template
                xLinea = Reader.ReadLine()

                Dim strTitolo As String = dtrLeggiDati("titolo")
                Dim strCodiceProgetto As String = dtrLeggiDati("codiceprogetto")
                Dim strDataAvvio As String = dtrLeggiDati("DataInizio")
                Dim strSede As String = dtrLeggiDati("Comune")

                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If

                strsql = "Select DISTINCT entità.identità,isnull(entità.CodiceVolontario, 'Nessun Codice') as CodiceVolontario, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entità.cognome,'°',''),'ì','i'''),'é','e'''),"
                strsql = strsql & " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') + ' ' +"
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(entità.nome,'°',''),'ì','i'''),'é','e'''),"
                strsql = strsql & "'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '')as nominativo, "
                strsql = strsql & "entità.Codicefiscale, "
                strsql = strsql & "isnull(case len(day(entità.datanascita)) when 1 then '0' + convert(varchar(20),day(entità.datanascita)) "
                strsql = strsql & "else convert(varchar(20),day(entità.datanascita))  end + '/' + "
                strsql = strsql & "(case len(month(entità.datanascita)) when 1 then '0' + convert(varchar(20),month(entità.datanascita)) "
                strsql = strsql & "else convert(varchar(20),month(entità.datanascita))  end + '/' + "
                strsql = strsql & "Convert(varchar(20), Year(entità.datanascita))),'') as datanascita, "
                strsql = strsql & "isnull(case len(day(entità.DataInizioServizio)) when 1 then '0' + convert(varchar(20),day(entità.DataInizioServizio)) "
                strsql = strsql & "else convert(varchar(20),day(entità.DataInizioServizio))  end + '/' + "
                strsql = strsql & "(case len(month(entità.DataInizioServizio)) when 1 then '0' + convert(varchar(20),month(entità.DataInizioServizio)) "
                strsql = strsql & "else convert(varchar(20),month(entità.DataInizioServizio))  end + '/' + "
                strsql = strsql & "Convert(varchar(20), Year(entità.DataInizioServizio))),'') as DataInizioServizio, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni.denominazione,'°',''),'ì','i'''),'é','e'''),"
                strsql = strsql & "'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') + '(' + isnull(replace(replace(replace(replace(replace(replace(replace(provincie.provincia ,'°',''),'ì','i'''),'é','e'''),"
                strsql = strsql & "'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') + ')'as comune, "
                strsql = strsql & " isnull(convert(varchar,attivitàentisediattuazione.idEntesedeattuazione),'Nessun Codice') as CodiceSede "
                strsql = strsql & " from entità "
                strsql = strsql & " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
                strsql = strsql & " INNER JOIN attivitàentisediattuazione"
                strsql = strsql & " ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
                strsql = strsql & " inner join statientità on statientità.idstatoentità=entità.idstatoentità "
                strsql = strsql & "left join impVolontariLotus on entità.codicefiscale=impVolontariLotus.cf "
                strsql = strsql & "inner join graduatorieEntità on graduatorieEntità.identità=Entità.identità "
                strsql = strsql & "left join TipologiePosto on graduatorieEntità.idtipologiaposto=TipologiePosto.idtipologiaposto "
                strsql = strsql & "inner join comuni on comuni.idcomune=entità.idcomunenascita "
                strsql = strsql & "inner join provincie on (provincie.idprovincia=comuni.idprovincia) "
                strsql = strsql & "where(graduatorieEntità.idattivitàsedeassegnazione='" & Request.QueryString("IDAttivitasedeAssegnazione") & "') and graduatorieEntità.ammesso = 1 and statientità.inservizio=1 and attivitàentità.idstatoattivitàentità=1 "
                strsql = strsql & "order by nominativo"

                'eseguo la query e passo il risultato al datareader
                dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

                Dim strVolontari As String = ""

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<TitoloProgetto>", strTitolo)
                    xLinea = Replace(xLinea, "<CodiceProgetto>", strCodiceProgetto)
                    xLinea = Replace(xLinea, "<DataAvvio>", strDataAvvio)
                    xLinea = Replace(xLinea, "<Sede>", strSede)

                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        If dtrLeggiDati.HasRows = True Then
                            While dtrLeggiDati.Read
                                strVolontari = strVolontari & (dtrLeggiDati("CodiceVolontario") & " " & dtrLeggiDati("Nominativo") & " nato/a il " & dtrLeggiDati("datanascita") & " a " & dtrLeggiDati("comune") & " avvio il " & dtrLeggiDati("DataInizioServizio") & " Codice Sede " & dtrLeggiDati("CodiceSede") & "\par") & vbCrLf

                                'Array degli id dei volontari per la gestione della cronologia dei documenti
                                arrayIdVol.Add(dtrLeggiDati("identità"))
                            End While
                            xLinea = Replace(xLinea, "<BreakPoint>", strVolontari)
                        End If
                    End If

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Elencovolontariammessi = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento,IDAttivitàSedeAssegnazione) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',2," & Request.QueryString("IDAttivitasedeAssegnazione") & ")"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

            'Cronologia documenti volontari in graduatoria
            Dim intX As Integer
            For intX = 0 To arrayIdVol.Count - 1
                ClsUtility.CronologiaDocEntità(arrayIdVol(intX), Session("Utente"), "Graduatoria Volontari", Session("conn"))
            Next

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function

    Sub NuovaCronologia(ByVal strDocumento As String, Optional ByRef DataProt As String = "", Optional ByRef NProt As String = "")
        'vado a fare la insert
        Dim cmdinsert As Data.SqlClient.SqlCommand
        strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento,IDAttivitàSedeAssegnazione,DataProt,NProt) "
        strsql = strsql & "values "
        strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & strDocumento & "',2," & Request.QueryString("IDAttivitasedeAssegnazione") & ","

        If DataProt = "" Then
            strsql = strsql & " null,"
        Else
            strsql = strsql & " '" & DataProt & "',"
        End If
        If NProt = "" Then
            strsql = strsql & " null"
        Else
            strsql = strsql & "'" & NProt & "'"
        End If
        strsql = strsql & ")"

        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()

        cmdinsert.Dispose()

    End Sub

    Sub CronologiaVolontariGraduatoria(ByVal IdSedeAssegnazione As Integer, ByVal DataProt As String, ByVal NProt As String)

        strsql = "Select DISTINCT entità.identità"
        strsql = strsql & " from entità "
        strsql = strsql & " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
        strsql = strsql & " INNER JOIN attivitàentisediattuazione"
        strsql = strsql & " ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
        strsql = strsql & " inner join statientità on statientità.idstatoentità=entità.idstatoentità "
        strsql = strsql & "left join impVolontariLotus on entità.codicefiscale=impVolontariLotus.cf "
        strsql = strsql & "inner join graduatorieEntità on graduatorieEntità.identità=Entità.identità "
        strsql = strsql & "left join TipologiePosto on graduatorieEntità.idtipologiaposto=TipologiePosto.idtipologiaposto "
        strsql = strsql & "inner join comuni on comuni.idcomune=entità.idcomunenascita "
        strsql = strsql & "inner join provincie on (provincie.idprovincia=comuni.idprovincia) "
        strsql = strsql & "where(graduatorieEntità.idattivitàsedeassegnazione='" & IdSedeAssegnazione & "') and graduatorieEntità.ammesso = 1 and statientità.inservizio=1 and attivitàentità.idstatoattivitàentità=1 "

        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))

        'Cronologia documenti volontari in graduatoria
        Dim intX As Integer

        For intX = 0 To dtsGenerico.Tables(0).Rows.Count - 1
            ClsUtility.CronologiaDocEntità(dtsGenerico.Tables(0).Rows(intX).Item("identità"), Session("Utente"), "Graduatoria Volontari", Session("conn"), DataProt, NProt)
        Next

    End Sub

    Private Sub imgGeneraDoc_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGeneraDoc.Click

        lblGenera.Text = "Scarica File  "
        hplDownload.Visible = True
        If Session("TipoUtente") = "U" Then
            cmdScSelProtocolloLV.Visible = True
            cmdScAllegatiLV.Visible = True
            cmdNuovoFasciocloLV.Visible = True
            ImgSalvaProt.Visible = True
        End If

        Dim Documento As New GeneratoreModelli
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Dim utility As New ClsUtility()
        Dim IdTipologiaProgetto As String = utility.TipologiaProgettoDaIdAttivita(Request.QueryString("IdAttivita"), Session("conn"))

        If (IdTipologiaProgetto = PROGETTO_GARANZIA_GIOVANI) Then
            hplDownload.NavigateUrl = Documento.VOL_ElencoVolontariAmmessiGaranziaGiovani(Request.QueryString("IDAttivitasedeAssegnazione"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
            NuovaCronologia("elencovolontariammessiGaranziaGiovani", TxtDataProtocollo.Text, TxtNumProtocollo.Text)
            CronologiaVolontariGraduatoria(Request.QueryString("IDAttivitasedeAssegnazione"), TxtDataProtocollo.Text, TxtNumProtocollo.Text)
        Else
            hplDownload.NavigateUrl = Documento.VOL_elencovolontariammessi(Request.QueryString("IDAttivitasedeAssegnazione"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
            NuovaCronologia("elencovolontariammessi", TxtDataProtocollo.Text, TxtNumProtocollo.Text)
            CronologiaVolontariGraduatoria(Request.QueryString("IDAttivitasedeAssegnazione"), TxtDataProtocollo.Text, TxtNumProtocollo.Text)
        End If



        'ANTONELLO -------------------------------------Documento.dispose()



        imgGeneraDoc.Visible = False
        hplDownload.Target = "_blank"

    End Sub



    Private Sub AbilitaPulsanteCancella()

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                " VociMenu.IdVoceMenuPadre" & _
                " FROM VociMenu" & _
                " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If Session("Read") <> "1" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If
        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                " WHERE (VociMenu.descrizione = 'Forza Cancella Graduatoria')" & _
                " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))

        If dtrGenerico.HasRows = True Then
            imgCancellaGrad.Visible = True
        Else
            imgCancellaGrad.Visible = False
        End If

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Sub AbilitaPulsanteCancella2()
        If Session("TipoUtente") = "U" Then
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                    " VociMenu.IdVoceMenuPadre" & _
                    " FROM VociMenu" & _
                    " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                    " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If Session("Read") <> "1" Then
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                    " WHERE (VociMenu.descrizione = 'Forza Cancella Volontario')" & _
                    " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))

            If dtrGenerico.HasRows = True Then
                dgRisultatoRicerca.Columns.Item(27).Visible = True
            Else
                dgRisultatoRicerca.Columns.Item(27).Visible = False
            End If

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        End If
    End Sub
    Private Function VerificaStatoGraduatoria() As Boolean
        'se lo stato è confermato non permetto il salvataggio
        Dim dtrStato As SqlClient.SqlDataReader
        Dim bolStato As Boolean
        bolStato = False

        strsql = "SELECT StatoGraduatoria " & _
                 " FROM AttivitàSediAssegnazione " & _
                 " WHERE IDAttivitàSedeAssegnazione = " & Request.QueryString("IDAttivitasedeAssegnazione")
        dtrStato = ClsServer.CreaDatareader(strsql, Session("Conn"))

        If dtrStato.HasRows = True Then
            dtrStato.Read()
            If dtrStato.Item("StatoGraduatoria") = 3 Then       'Confermato
                bolStato = True
            End If
        End If

        dtrStato.Close()
        dtrStato = Nothing

        Return bolStato

    End Function
    Private Sub imgSegnalaOk_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSegnalaOk.Click
        Dim strsql As String

        Try
            If imgSegnalaOk.ImageUrl = "images/vistabuona_small.png" Then
                strsql = "UPDATE attività SET SegnalaGraduatorieVolontari=1 where idattività=" & Request.QueryString("idattivita")
                imgSegnalaOk.ImageUrl = "images/vistacattiva_small.png"
                lblSegnalazione.Text = "Ripristina Progetto"
            Else
                strsql = "UPDATE attività SET SegnalaGraduatorieVolontari=0 where idattività=" & Request.QueryString("idattivita")
                imgSegnalaOk.ImageUrl = "images/vistabuona_small.png"
                lblSegnalazione.Text = "Segnala Progetto"
            End If

            myCommand = New System.Data.SqlClient.SqlCommand
            myCommand.Connection = Session("conn")

            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            lblMessaggioAlert.Visible = True
            lblMessaggioAlert.Text = ex.Message
            'imgAlert.Visible = True
        End Try

    End Sub
    Private Sub SalvaProtocolli()
        Dim strsql As String
        If TxtNumProtocollo.Text = "" Then
            Exit Sub
        End If

        strsql = "update cronologiaentidocumenti set dataprot ='" & TxtDataProtocollo.Text & _
        "',  nprot = '" & TxtNumProtocollo.Text & "' where idente = " & CInt(Session("IdEnte")) & _
        " and tipodocumento = 2  and documento = 'elencovolontariammessi' and IDAttivitàSedeAssegnazione = " & Request.QueryString("IDAttivitasedeAssegnazione")

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        myCommand.CommandText = strsql
        myCommand.ExecuteNonQuery()

    End Sub

    Sub UpdateVolontariGraduatoria(ByVal IdSedeAssegnazione As Integer)
        'creato da sc il 28/01/2008
        Dim strsql As String

        strsql = " UPDATE CronologiaEntitàDocumenti SET "
        strsql = strsql & "	DataProt = '" & TxtDataProtocollo.Text & "', "
        strsql = strsql & " NProt = '" & TxtNumProtocollo.Text & "' "
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

    Private Sub imgValutazioneProg_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgValutazioneProg.Click
        'Agg. da simona cordella il 14/05/2009
        'gestione pulsante che richiama la maschere WfrmValutazioneQual (graduatorie)
        'Context.Items.Add("idprogetto", Request.QueryString("idattivita"))
        'Server.Transfer("WfrmValutazioneQual.aspx?VengoDa=AssociaVolontari&CheckIndietro=" & Request.QueryString("CheckIndietro") & "&Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Progetto=" & Request.QueryString("Progetto") & "&Bando=" & Request.QueryString("Bando") & "&Settore=" & Request.QueryString("Settore") & "&Area=" & Request.QueryString("Area") & "&InAttesa=" & Request.QueryString("InAttesa") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&IDAttivitasedeAssegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "&presenta=" & Request.QueryString("presenta") & "&IdAttivita=" & Request.QueryString("idattivita") & "&CodiceFascicolo=" & TxtCodFascicolo.Value & "&DescFascicolo=" & txtdescrizionefascicolo.Text & "&NumeroFascicolo=" & txtNumeroFascicoloinVisione.Text, True)

        Response.Redirect(ClsUtility.RitornaMascheraValutazioneProgetto(Request.QueryString("idattivita"), Session("conn")) & "?VengoDa=AssociaVolontari&CheckIndietro=" & Request.QueryString("CheckIndietro") & "&Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Progetto=" & Request.QueryString("Progetto") & "&Bando=" & Request.QueryString("Bando") & "&Settore=" & Request.QueryString("Settore") & "&Area=" & Request.QueryString("Area") & "&InAttesa=" & Request.QueryString("InAttesa") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&IDAttivitasedeAssegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "&presenta=" & Request.QueryString("presenta") & "&IdAttivita=" & Request.QueryString("idattivita") & "&idprogetto=" & Request.QueryString("idattivita") & "&CodiceFascicolo=" & TxtCodFascicolo.Value & "&DescFascicolo=" & txtdescrizionefascicolo.Text & "&NumeroFascicolo=" & txtNumeroFascicoloinVisione.Text, True)
    End Sub

    Private Sub imgRecuperoOk_Click1(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRecuperoOk.Click
        Dim strsql As String

        Try
            If imgRecuperoOk.ImageUrl = "images/vistabuona_small.png" Then
                strsql = "UPDATE attivitàsediassegnazione SET AmmessoRecupero=1 , UsernameAmmessoRecupero = '" & Session("Utente") & "', DataAmmessoRecupero = getdate() where idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione")
                imgRecuperoOk.ImageUrl = "images/vistacattiva_small.png"
                lblRecupero.Text = "Annulla Recupero Posti"
            Else
                strsql = "UPDATE attivitàsediassegnazione SET AmmessoRecupero=0 , UsernameAmmessoRecupero = null, DataAmmessoRecupero = null where idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione")
                imgRecuperoOk.ImageUrl = "images/vistabuona_small.png"
                lblRecupero.Text = "Recupero Posti"
            End If

            myCommand = New System.Data.SqlClient.SqlCommand
            myCommand.Connection = Session("conn")

            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            lblMessaggioAlert.Visible = True
            lblMessaggioAlert.Text = ex.Message
            'imgAlert.Visible = True
        End Try

    End Sub
    Private Sub AbilitaRecuperoPosti()

        If lblStatoGraduatoria.Text = "Confermata" Then
            'CONTROLLI VISIBILITA' POSTI AMMESSI A RECUPERO
            strsql = "SELECT ISNULL(AmmessoRecupero,0) AS AmmessoRecupero, dbo.formatodata(DataAvvioRecuperoPosti) as DataAvvio FROM attivitàsediassegnazione WHERE idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione")
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                Select Case dtrGenerico("AmmessoRecupero")
                    Case "0"
                        imgRecuperoOk.ImageUrl = "images/vistabuona_small.png"
                        lblRecupero.Text = "Recupero Posti"
                    Case "1"
                        imgRecuperoOk.ImageUrl = "images/vistacattiva_small.png"
                        lblRecupero.Text = "Annulla Recupero Posti"
                    Case "2"
                        imgRecuperoOk.Visible = False
                        lblRecupero.Text = "Conferma Recupero Posti"
                        lblRecupero.Visible = False
                        imgConfermaRecupero.ImageUrl = "images/save.gif"
                        imgConfermaRecupero.Visible = True
                        txtInizioRecupero.Visible = True
                        lblConfermaRecupero.Visible = True
                    Case "3"
                        imgRecuperoOk.Visible = False
                        lblRecupero.Text = "Recupero posti confermati"
                        lblRecupero.Visible = False
                        imgConfermaRecupero.Visible = False
                        txtInizioRecupero.Visible = True
                        txtInizioRecupero.Text = dtrGenerico("DataAvvio")
                        txtInizioRecupero.Enabled = False
                        lblConfermaRecupero.Visible = True
                End Select

            End If

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        Else
            imgRecuperoOk.Visible = False
            lblRecupero.Visible = False
        End If
    End Sub

    Private Sub imgConfermaRecupero_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgConfermaRecupero.Click
        Dim strMessaggioErrore As String

        If txtInizioRecupero.Text = "" Then
            lblMessaggi.Visible = True

            lblMessaggi.Text = "il campo Inizio Recupero è obbligatorio"
            txtInizioRecupero.Focus()
            Exit Sub
        End If
        'Eseguo Modifiche du graduatoriaEntità Ammessso Idoneo
        If ContaVol() = True Then
            ModificaGraduatoria()
            strsql = " SELECT COUNT(*) as VolAmm " & _
            " FROM graduatorieentità s1 " & _
            " WHERE s1.idattivitàsedeassegnazione in (select attivitàsediassegnazione.idattivitàsedeassegnazione  " & _
            " from attivitàsediassegnazione where attivitàsediassegnazione.idattività=" & lblidattivita.Text & ")" & _
            " and ammesso=1"
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrGenerico.Read()
            lblVolSel.Text = dtrGenerico("VolAmm")
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            If lblpresentata.Text = "3" Then
                'CONTINUARE CORREZIONI DA QUI!!!! DANILO 22/05/2012
                ConfermaGraduatoriaRecupero()

                Dim item As DataGridItem

                ReDim Session("arrayErrori")(0)
                Session("arrayErrori")(0) = ""

                Dim strIdEntita As String

                '******************
                For Each item In dgRisultatoRicerca.Items
                    Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                    Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                    ' se Idoneo o Selezionato
                    If check.Checked = True And check2.Checked = True And dgRisultatoRicerca.Items(item.ItemIndex).Cells(30).Text = "1" Then
                        'PRIMA DI MODIFICA DI PASSAGGIO DI PIU' VALORI SEPARATI DA #
                        'esito fascicolazione
                        'If Left(ClsUtility.GeneraFascicolo(Session("Utente"), dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text, Session("conn")), 4) <> "0000" Then       'errore
                        '    If Session("arrayErrori")(0) = "" Then
                        '        Session("arrayErrori")(0) = "Per i seguenti volontari non e' stato possibile creare il fascicolo SIGED: " & "<br /><br />" & "- " & item.Cells(4).Text & "<br />"
                        '    Else
                        '        ReDim Preserve Session("arrayErrori")(UBound(Session("arrayErrori")) + 1)
                        '        Session("arrayErrori")(UBound(Session("arrayErrori"))) = "- " & item.Cells(4).Text & vbCrLf
                        '    End If
                        'End If
                        '*** 09/12/2010 verifico esistenza del codice fascicolo
                        strsql = "Select isnull(CodiceFascicolo,'') as CodiceFascicolo from Entità where identità = " & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text
                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        dtrGenerico.Read()
                        If dtrGenerico("CodiceFascicolo") = "" Then
                            strIdEntita = strIdEntita & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "#"
                        End If
                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        '***
                    End If
                Next
                If strIdEntita <> "" Then

                    'loggo su logfascicolivolontari
                    strsql = "INSERT INTO LogFascicoliVolontari([Username],[Metodo],[IdEntità],[DataOraRichiesta],[DataOraEsecuzione],[Eseguito])"
                    strsql = strsql & " VALUES('" & Session("Utente") & "','GeneraFascicoloCumulati','" & Mid(strIdEntita, 1, Len(strIdEntita) - 1) & "',getdate(),NULL,0)"
                    myCommand.CommandText = strsql
                    myCommand.ExecuteNonQuery()

                    '---recupero l'id appena inserito
                    Dim strID As String
                    strsql = "select @@identity as Id"
                    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    dtrGenerico.Read()
                    strID = dtrGenerico("Id")
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                    '09/06/2017 DA SIMONA CORDELLA
                    'richiamo la creazione del fascicolo con il WSINTERNO in MODO asincrono 
                    Dim resultAsync As IAsyncResult
                    resultAsync = GeneraFascicoliVolontari(Session("Utente"), Mid(strIdEntita, 1, Len(strIdEntita) - 1), strID)

                    'If ClsUtility.GeneraFascicoloCumulati(Session("Utente"), Mid(strIdEntita, 1, Len(strIdEntita) - 1), Session("conn"), strID) <> "0" Then       'errore

                    '    Dim strErr As String
                    '    strErr = "<table align=""center"" width=""750"" style=""width: 750px"" cellSpacing=""1"" cellPadding=""1""><tr><td align=""750"">"
                    '    strErr = strErr & "<table align=""center"" cellSpacing=""1"" cellPadding=""1""><tr><td><img src=""http://helios.serviziocivile.it/images/logoIII.jpg"" border=""0""></td></tr></table>"
                    '    strErr = strErr & "<table align=""center"" bgcolor=""orange"" width=""750""><tr><td><b>Errore generazione fascicolo:Maschera WfrmAssociaVolontari.aspx</b></td></tr>"
                    '    strErr = strErr & "<tr><td><b>Utente:</b> " & Session("Utente") & "</td></tr>"
                    '    strErr = strErr & "<tr><td><b>IP:</b> " & HttpContext.Current.Request.UserHostAddress & "</td></tr>"
                    '    strErr = strErr & "<tr><td><b>Ente:</b> " & Session("Denominazione") & "</td></tr>"
                    '    strErr = strErr & "<tr><td><b>Codice Ente:</b> " & Session("CodiceRegioneEnte") & "</td></tr>"
                    '    strErr = strErr & "<tr><td><b>Codice Progetto:</b>" + lblcodprogetto.Text & "</td></tr>"
                    '    strErr = strErr & "<tr><td><b>Codice Sede Graduatoria: </b>" & Request.QueryString("IDAttivitasedeAssegnazione") & "</td></tr>"
                    '    strErr = strErr & "</table>"
                    '    strErr = strErr & "</td></tr></table>"

                    '    Session("arrayErrori")(0) = "Si è verificato un errore durante la creazione dei fascicoli. Contattare l'assistenza SIGED."
                    '    'If Session("arrayErrori")(0) = "" Then
                    '    '    Session("arrayErrori")(0) = "Per i seguenti volontari non e' stato possibile creare il fascicolo SIGED: " & "<br /><br />" & "- " & item.Cells(4).Text & "<br />"
                    '    'Else
                    '    '    ReDim Preserve Session("arrayErrori")(UBound(Session("arrayErrori")) + 1)
                    '    '    Session("arrayErrori")(UBound(Session("arrayErrori"))) = "- " & item.Cells(4).Text & vbCrLf
                    '    'End If
                    '    ClsUtility.invioEmail("heliosweb@serviziocivile.it", "heliosweb@serviziocivile.it", "d.spagnulo@grupposerap.it;a.dicroce@grupposerap.it;r.macioce@grupposerap.it;s.cordella@grupposerap.it", "ERRORE GENERAZIONE FASCICOLO", strErr)

                    'Else
                    '    strsql = "update LogFascicoliVolontari set DataOraEsecuzione = getdate(), Eseguito=1 where IdLogFascicoliVolontari = " & strID
                    '    myCommand.CommandText = strsql
                    '    myCommand.ExecuteNonQuery()
                    'End If

                    'If Session("arrayErrori")(0) <> "" Then
                    '    Response.Write("<SCRIPT>" & vbCrLf)
                    '    Response.Write("window.open('WfrmAnomalieCreazioneFascicoli.aspx','anomalie','height=450,width=450,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
                    '    Response.Write("</SCRIPT>")
                    'End If

                End If


                cmdRespingi.Visible = False
                cmdModificaUNSC.Visible = False
                BloccaDataInizio()
                ChekBloccaIdonei()
            End If
        Else
            lblMessaggioAlert.Visible = True
            'imgAlert.Visible = True
            lblMessaggioAlert.Text = "Attenzione il numero dei Volontari Selezionati ha raggiunto la capienza del Progetto."
            CaricaDataGrid(dgRisultatoRicerca)
            ChekBloccaIdonei()
        End If
    End Sub
    Private Sub ConfermaGraduatoriaRecupero()
        'Realizzato da Danilo Spagnulo maggio 2012
        'routine che effettua la conferma per i soli volontari ammessi a recupero
        Dim Null As String = "Null"
        imgConfermaRecupero.Visible = False
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = MyTransaction
            'Modifico stato graduatoria inserendo chi ha effettuatio la conferma e la data
            strsql = "Update attivitàsediassegnazione set ammessorecupero=3," & _
            " UsernameConfermaRecupero='" & Session("Utente") & "',DataConfermaRecupero=getdate(), DataAvvioRecuperoPosti='" & txtInizioRecupero.Text & "' " & _
            " where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""

            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            '*********************************************
            'eseguito da jonadani spagnani il 26/11/2004
            'aggiorno cronologia volontari IDONEI e SELEZIONATI
            strsql = "insert into CronologiaEntità (IdEntità, IdStatoEntità, UsernameStato, DataChiusura, DataCronologia, NoteStato, IdCausaleChiusura) "
            strsql = strsql & "select entità.IdEntità, entità.IdStatoEntità, entità.UsernameStato, entità.DataChiusura, entità.DataUltimoStato, entità.NoteStato, entità.IdCausaleChiusura "
            strsql = strsql & "from entità "
            strsql = strsql & "inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità "
            strsql = strsql & "where isnull(entità.ammessorecupero,0) = 1 and Ammesso=1 and IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'aggiorno stato volontari IDONEI e SELEZIONATI

            'Agg.il 17/04/2008 da simona cordella campo POSTOOCCUPATO = 1
            strsql = "update entità set IdStatoEntità=(select IdStatoEntità from StatiEntità where inservizio=1), "
            strsql = strsql & "UserNameStato='" & Session("Utente") & "',"
            strsql = strsql & "DataUltimoStato=GetDate(),DataInizioServizio='" & txtInizioRecupero.Text & "',DataFineServizio=dateadd(d,-1,dateadd(yy,1,'" & txtInizioRecupero.Text & " ')),PostoOccupato=1 "
            strsql = strsql & "from entità "
            strsql = strsql & "inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità "
            strsql = strsql & "where isnull(entità.ammessorecupero,0) = 1 and Ammesso=1 and IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            '***********
            'Implementata da Alessandra Taballione il 30/05/2005
            'a seguito di interventi implementativi richiesti dalla dott.ssa Cagiati**********************************
            strsql = "Update attivitàEntisediAttuazione set StatoAssegnazione=2 from attivitàEntisediAttuazione " & _
            " inner join Entità on attivitàEntisediAttuazione.IDEnteSedeAttuazione=Entità.Tmpidsedeattuazione " & _
            " Inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità " & _
            " where isnull(entità.ammessorecupero,0) = 1 and GraduatorieEntità.Ammesso=1 and GraduatorieEntità.IdAttivitàSedeAssegnazione=" & CInt(lblidattivitasedeassegnazione.Text) & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            'Insert solo idonei 
            Dim item As DataGridItem
            Dim idAttiivitàEntisediattuazione As String
            '******************
            For Each item In dgRisultatoRicerca.Items
                'Idonei
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                Dim text As TextBox = DirectCast(item.FindControl("Text1"), TextBox)
                Dim ddl As DropDownList = DirectCast(item.FindControl("DDL1"), DropDownList)
                ' se Idoneo o Selezionato
                If check.Checked = True And check2.Checked = True And dgRisultatoRicerca.Items(item.ItemIndex).Cells(30).Text = "1" Then
                    strsql = " Insert into AttivitàEntità (IDAttivitàEnteSedeAttuazione,IDEntità,DataInizioAttivitàEntità," & _
                                     " DataFineAttivitàEntità,IdStatoAttivitàEntità,PercentualeUtilizzo,IDTipologiaPosto) " & _
                                     " select idattivitàentesedeattuazione ," & dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text & ", " & _
                                     "'" & txtInizioRecupero.Text & "',dateadd(d,-1,dateadd(yy,1,'" & txtInizioRecupero.Text & " ')),1,100," & ddl.SelectedValue & " " & _
                                     " from attivitàentisediattuazione where idattività=" & lblidattivita.Text & " and IDEnteSedeAttuazione=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(22).Text & "  "
                    myCommand.CommandText = strsql
                    myCommand.ExecuteNonQuery()

                    'Controllo se il codice del volntario è già presente
                    If VerificaCodiceVolontario(dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text) = False Then
                        'mod. il 20/06/2016 da s.c.
                        strsql = "update entità set UserName= dbo.FN_CalcoloUsernameVolontario(" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "), password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                        "(SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                        "CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                        "CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                        "From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                        "where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "

                        'Aggiunto da Alessandra Taballione il 09/06/2005
                        'strsql = "update entità set username=(SELECT 'V' + left(codicefiscale,3) + replicate('0',6-len(convert(varchar,identità))) + convert(varchar, identità) from entità where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "), password='" & ClsUtility.CriptaNuovaPass & "', codiceVolontario= " & _
                        '"(SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) + " & _
                        '"CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) + " & _
                        '"CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) " & _
                        '"From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) " & _
                        '"where identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & " "

                        'gennaio 2006, codice volontario sostituito
                        '"(select 'V'+ convert(nvarchar(4),year(getdate()))+ convert(varchar(6)," & _
                        '" replicate('0', 6-len( convert(varchar(6),max(convert(int,right(isnull(codicevolontario,0),6))+1))))) + " & _
                        '" Convert(varchar(6), max(Convert(Int, Right(isnull(codicevolontario, 0), 6)) + 1))" & _
                        '" from entità)" & _

                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        myCommand.CommandText = strsql
                        myCommand.ExecuteNonQuery()

                    End If
                End If
            Next
            MyTransaction.Commit()
        Catch ex As Exception
            Response.Write(strsql)
            Response.Write("<br>")
            Response.Write(ex.Message.ToString)
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            MyTransaction.Dispose()
            MyTransaction = Nothing
            cmdSalva.Visible = True
            Exit Sub
        End Try
        MyTransaction.Dispose()
        MyTransaction = Nothing
        '********************
        lblStatoGraduatoria.Text = "Confermata"
        cmdConferma.Visible = False
        imgmess.Visible = True
        lblMessaggi.Visible = True

        cmdAggiungi.Visible = False
        cmdSalva.Visible = False
        lblMessaggi.Text = "La Graduatoria è stata Confermata con successo."
    End Sub
    Private Sub imgAssociaProtocollo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAssociaProtocollo.Click

        'FUNZIONALITA': RICHIAMA LA MASCHERA WfrmAssociazioneMultiplaProtocolloVolontari 
        ' ''Response.Write("<script>")
        ' ''Response.Write("window.open('WfrmAssociazioneMultiplaProtocolloVolontari.aspx?IdAttivitaSedeAssegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "', 'Visualizza', 'width=800,height=600,scrollbars=yes')")
        ' ''Response.Write("</script>")
        Response.Redirect("WfrmAssociazioneMultiplaProtocolloVolontari.aspx?CheckIndietro=" & Request.QueryString("CheckIndietro") & "&Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Progetto=" & Request.QueryString("Progetto") & "&Bando=" & Request.QueryString("Bando") & "&Settore=" & Request.QueryString("Settore") & "&Area=" & Request.QueryString("Area") & "&InAttesa=" & Request.QueryString("InAttesa") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&Vengoda=" & Request.QueryString("Vengoda") & "&IdAttivitaSedeAssegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&presenta=" & lblpresentata.Text & "&CodiceFascicolo=" & Request.QueryString("CodiceFascicolo") & "&DescFascicolo=" & Request.QueryString("DescFascicolo") & "&NumeroFascicolo=" & Request.QueryString("NumeroFascicolo"))



    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        If Session("TipoUtente") = "U" Then
            Call SalvaProtocolli()
        End If

        Call Chiudi()
    End Sub
    Protected Sub imgCancellaGrad_Click(sender As Object, e As EventArgs) Handles imgCancellaGrad.Click

        Dim sEsito As String
        Dim strMessaggio As String

        sEsito = ClsServer.EseguiStoreCancellaGraduatoria(CInt(lblidattivita.Text), CInt(Request.QueryString("IDAttivitasedeAssegnazione")), Session("conn"))

        If sEsito = "ERRORE" Then
            strMessaggio = "Errore imprevisto durante l'operazione di 'Eliminazione Graduatoria'. Contattare l'assistenza Helios/Futuro."
        ElseIf sEsito = "KO" Then
            strMessaggio = "Non si può eliminare una graduatoria con stato Confermato o Respinta"
        Else
            strMessaggio = "Eliminazione Graduatoria terminata con successo."
            'Response.Redirect("WfrmElencoSediAttuazione.aspx?PaginaGrid=" & Request.QueryString("PaginaGrid") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&InAttesa=" & Request.QueryString("InAttesa") & "&Area=" & Request.QueryString("Area") & "&Settore=" & Request.QueryString("Settore") & "&Bando=" & Request.QueryString("Bando") & "&Progetto=" & Request.QueryString("Progetto") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Ente=" & Request.QueryString("Ente") & "&CheckIndietro=" & Request.QueryString("CheckIndietro") & "&IdAttivita=" & lblidattivita.Text & "&idente=" & txtidente.Text & "&Azione=Visualizzazione")
            Call CaricaDataGrid(dgRisultatoRicerca)
        End If


        lblMessaggioAlert.Visible = True
        'imgAlert.Visible = True
        lblMessaggioAlert.Text = strMessaggio
    End Sub
    Protected Sub cmdRespingi_Click(sender As Object, e As EventArgs) Handles cmdRespingi.Click
        RespingiGraduatoria()
        ChekBloccaIdonei()
        cmdSalva.Visible = False

        cmdAggiungi.Visible = False
        cmdModificaUNSC.Visible = False
        BloccaDataInizio()
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim strMessaggioErrore As String
        Dim strMiaData As String
        Dim strMioEsito As String

        If PartDiff.Visible = False Then 'conferma ordinaria
            If txtinizioEff.Text = String.Empty Then
                lblMessaggi.Visible = True
                lblMessaggi.ForeColor = Color.Red
                lblMessaggi.Text = "Inserire la data inizio servizio"
                Exit Sub
            End If

            If ValidaData() <> "" Then
                lblMessaggi.Visible = True
                lblMessaggi.ForeColor = Color.Red
                lblMessaggi.Text = mesg
                Exit Sub
            Else
                If CheckIntervalloDate() <> "" Then
                    lblMessaggi.Visible = True
                    lblMessaggi.ForeColor = Color.Red
                    lblMessaggi.Text = mesg
                    Exit Sub
                End If
            End If
        Else
            'conferma partenza differita
            If txtInizioServizioSede.Text = String.Empty Then
                lblMessaggi.Visible = True
                lblMessaggi.ForeColor = Color.Red
                lblMessaggi.Text = "Inserire la data inizio servizio sede"
                Exit Sub
            End If
            If ValidaDataDifferita() <> "" Then
                lblMessaggi.Visible = True
                lblMessaggi.ForeColor = Color.Red
                lblMessaggi.Text = mesg
                Exit Sub
            End If

        End If

        If lblStatoGraduatoria.Text = "Registrata" And dgRisultatoRicerca.Items.Count = 0 Then
        Else
            If leggidata(txtinizioEff.Text, txtDataOdierna.Text) <> "" And PartDiff.Visible = False Then
                lblMessaggi.Visible = True
                lblMessaggi.ForeColor = Color.Red
                lblMessaggi.Text = mesg
                Exit Sub
            Else
                If PartDiff.Visible = False Then
                    txtFineEff.Text = DateAdd("d", -1, DateAdd("m", NumeroMesiProgetto(Request.QueryString("IdAttivita")), txtinizioEff.Text))
                Else
                    txtFineServizioSede.Text = DateAdd("d", -1, DateAdd("m", NumeroMesiProgetto(Request.QueryString("IdAttivita")), txtInizioServizioSede.Text))
                End If
            End If
        End If


        If PartDiff.Visible = False Then
            strMiaData = txtinizioEff.Text
        Else
            strMiaData = txtInizioServizioSede.Text
        End If

        strMioEsito = ControlloDataNeet(lblidattivitasedeassegnazione.Text, strMiaData)
        If strMioEsito <> "" Then
            lblMessaggi.Visible = True
            lblMessaggi.ForeColor = Color.Red
            lblMessaggi.Text = strMioEsito
            Exit Sub
        End If

        'richiamo la store per il controllo della graduatora
        If ControlloGraduatoria(lblidattivita.Text, txtinizioEff.Text) = "NEGATIVO" Then
            Response.Write("<script>" & vbCrLf)
            Response.Write("window.open(""WfrmControlliGraduatoria.aspx?IdAttivita=" & Request.QueryString("IdAttivita") & "&DataAvvio=" & txtinizioEff.Text & """, """", ""width=950,height=600,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf)
            Response.Write("</script>")
            Exit Sub
        End If




            'If leggidata(txtinizioEff.Text, txtDataOdierna.Text) <> "" Then
            '    lblMessaggi.Visible = True
            '    lblMessaggi.ForeColor = Color.Red
            '    lblMessaggi.Text = mesg
            '    Exit Sub
            'Else
            '    txtFineEff.Text = DateAdd("d", -1, DateAdd("yyyy", 1, txtinizioEff.Text))
            'End If
            'aggiungo il controllo sul titolario
            'se c'è vado avanti con la cvonferma della graduatoria
            'altrimenti informo l'utente che non è possibile procedere finchè 
            'non aggiunge il titolario per il bando
        If ClsUtility.TrovaTitolario(lblidattivita.Text, Session("conn")) = True Then
            If VerificaStatoGraduatoria() = False Then          'se la graduatoria è confermata non eseguo il salvataggio (controllo aggiunto per bloccare l'azione successiva alla pressione del tasto F5)
                'Eseguo Modifiche du graduatoriaEntità Ammessso Idoneo
                'PROVO LA FUNZIONE CHE CONTROLLA I GMO SUL PROGETTO E SULLA GRADUATORIA
                strMessaggioErrore = ContaGMO()
                If strMessaggioErrore <> "" Then
                    lblMessaggioAlert.Visible = True

                    lblMessaggioAlert.Text = strMessaggioErrore
                    CaricaDataGrid(dgRisultatoRicerca)
                    Exit Sub
                End If
                strMessaggioErrore = ContaFAMI()
                If strMessaggioErrore <> "" Then
                    lblMessaggioAlert.Visible = True

                    lblMessaggioAlert.Text = strMessaggioErrore
                    CaricaDataGrid(dgRisultatoRicerca)
                    Exit Sub
                End If


                If ContaVol() = True Then
                    ModificaGraduatoria()
                    strsql = " SELECT COUNT(*) as VolAmm " & _
                    " FROM graduatorieentità s1 " & _
                    " WHERE s1.idattivitàsedeassegnazione in (select attivitàsediassegnazione.idattivitàsedeassegnazione  " & _
                    " from attivitàsediassegnazione where attivitàsediassegnazione.idattività=" & lblidattivita.Text & ")" & _
                    " and ammesso=1"
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    dtrGenerico.Read()
                    lblVolSel.Text = dtrGenerico("VolAmm")
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    If lblpresentata.Text = "3" Then

                        If PartDiff.Visible = False Then
                            ConfermaGraduatoria()
                        Else
                            ConfermaGraduatoriaDifferita()
                        End If

                        Dim item As DataGridItem

                        ReDim Session("arrayErrori")(0)
                        Session("arrayErrori")(0) = ""

                        Dim strIdEntita As String

                        '******************
                        For Each item In dgRisultatoRicerca.Items
                            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                            Dim check2 As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                            ' se Idoneo o Selezionato
                            If check.Checked = True And check2.Checked = True Then
                                'PRIMA DI MODIFICA DI PASSAGGIO DI PIU' VALORI SEPARATI DA #
                                'esito fascicolazione
                                'If Left(ClsUtility.GeneraFascicolo(Session("Utente"), dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text, Session("conn")), 4) <> "0000" Then       'errore
                                '    If Session("arrayErrori")(0) = "" Then
                                '        Session("arrayErrori")(0) = "Per i seguenti volontari non e' stato possibile creare il fascicolo SIGED: " & "<br /><br />" & "- " & item.Cells(4).Text & "<br />"
                                '    Else
                                '        ReDim Preserve Session("arrayErrori")(UBound(Session("arrayErrori")) + 1)
                                '        Session("arrayErrori")(UBound(Session("arrayErrori"))) = "- " & item.Cells(4).Text & vbCrLf
                                '    End If
                                'End If
                                '*** 09/12/2010 verifico esistenza del codice fascicolo
                                strsql = "Select isnull(CodiceFascicolo,'') as CodiceFascicolo from Entità where identità = " & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text
                                If Not dtrGenerico Is Nothing Then
                                    dtrGenerico.Close()
                                    dtrGenerico = Nothing
                                End If
                                dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                                dtrGenerico.Read()
                                If dtrGenerico("CodiceFascicolo") = "" Then
                                    strIdEntita = strIdEntita & dgRisultatoRicerca.Items(item.ItemIndex).Cells(24).Text & "#"
                                End If
                                If Not dtrGenerico Is Nothing Then
                                    dtrGenerico.Close()
                                    dtrGenerico = Nothing
                                End If
                                '***
                            End If
                        Next
                        If strIdEntita <> "" Then

                            'loggo su logfascicolivolontari
                            strsql = "INSERT INTO LogFascicoliVolontari([Username],[Metodo],[IdEntità],[DataOraRichiesta],[DataOraEsecuzione],[Eseguito])"
                            strsql = strsql & " VALUES('" & Session("Utente") & "','GeneraFascicoloCumulati','" & Mid(strIdEntita, 1, Len(strIdEntita) - 1) & "',getdate(),NULL,0)"
                            myCommand.CommandText = strsql
                            myCommand.ExecuteNonQuery()

                            '---recupero l'id appena inserito
                            Dim strID As String
                            strsql = "select @@identity as Id"
                            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                            dtrGenerico.Read()
                            strID = dtrGenerico("Id")
                            dtrGenerico.Close()
                            dtrGenerico = Nothing

                            '09/06/2017 DA SIMONA CORDELLA
                            'richiamo la creazione del fascicolo con il WSINTERNO in MODO asincrono 
                            Dim resultAsync As IAsyncResult
                            resultAsync = GeneraFascicoliVolontari(Session("Utente"), Mid(strIdEntita, 1, Len(strIdEntita) - 1), strID)

                            'If ClsUtility.GeneraFascicoloCumulati(Session("Utente"), Mid(strIdEntita, 1, Len(strIdEntita) - 1), Session("conn"), strID) <> "0" Then       'errore

                            '    Dim strErr As String
                            '    strErr = "<table align=""center"" width=""750"" style=""width: 750px"" cellSpacing=""1"" cellPadding=""1""><tr><td align=""750"">"
                            '    strErr = strErr & "<table align=""center"" cellSpacing=""1"" cellPadding=""1""><tr><td><img src=""http://helios.serviziocivile.it/images/logoIII.jpg"" border=""0""></td></tr></table>"
                            '    strErr = strErr & "<table align=""center"" bgcolor=""orange"" width=""750""><tr><td><b>Errore generazione fascicolo:Maschera WfrmAssociaVolontari.aspx</b></td></tr>"
                            '    strErr = strErr & "<tr><td><b>Utente:</b> " & Session("Utente") & "</td></tr>"
                            '    strErr = strErr & "<tr><td><b>IP:</b> " & HttpContext.Current.Request.UserHostAddress & "</td></tr>"
                            '    strErr = strErr & "<tr><td><b>Ente:</b> " & Session("Denominazione") & "</td></tr>"
                            '    strErr = strErr & "<tr><td><b>Codice Ente:</b> " & Session("CodiceRegioneEnte") & "</td></tr>"
                            '    strErr = strErr & "<tr><td><b>Codice Progetto:</b>" + lblcodprogetto.Text & "</td></tr>"
                            '    strErr = strErr & "<tr><td><b>Codice Sede Graduatoria: </b>" & Request.QueryString("IDAttivitasedeAssegnazione") & "</td></tr>"
                            '    strErr = strErr & "</table>"
                            '    strErr = strErr & "</td></tr></table>"

                            '    Session("arrayErrori")(0) = "Si è verificato un errore durante la creazione dei fascicoli. Contattare l'assistenza SIGED."
                            '    'If Session("arrayErrori")(0) = "" Then
                            '    '    Session("arrayErrori")(0) = "Per i seguenti volontari non e' stato possibile creare il fascicolo SIGED: " & "<br /><br />" & "- " & item.Cells(4).Text & "<br />"
                            '    'Else
                            '    '    ReDim Preserve Session("arrayErrori")(UBound(Session("arrayErrori")) + 1)
                            '    '    Session("arrayErrori")(UBound(Session("arrayErrori"))) = "- " & item.Cells(4).Text & vbCrLf
                            '    'End If
                            '    ClsUtility.invioEmail("heliosweb@serviziocivile.it", "heliosweb@serviziocivile.it", "d.spagnulo@grupposerap.it;a.dicroce@grupposerap.it;r.macioce@grupposerap.it;s.cordella@grupposerap.it", "ERRORE GENERAZIONE FASCICOLO", strErr)

                            'Else
                            '    strsql = "update LogFascicoliVolontari set DataOraEsecuzione = getdate(), Eseguito=1 where IdLogFascicoliVolontari = " & strID
                            '    myCommand.CommandText = strsql
                            '    myCommand.ExecuteNonQuery()
                            'End If

                            'If Session("arrayErrori")(0) <> "" Then
                            '    Response.Write("<SCRIPT>" & vbCrLf)
                            '    Response.Write("window.open('WfrmAnomalieCreazioneFascicoli.aspx','anomalie','height=450,width=450,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
                            '    Response.Write("</SCRIPT>")
                            'End If

                        End If


                        cmdRespingi.Visible = False
                        cmdModificaUNSC.Visible = False
                        cmdAnnullaDifferita.Visible = False
                        BloccaDataInizio()
                        ChekBloccaIdonei()
                    End If
                Else
                    lblMessaggioAlert.Visible = True
                    'imgAlert.Visible = True
                    lblMessaggioAlert.Text = "Attenzione il numero dei Volontari Selezionati ha raggiunto la capienza del Progetto."
                    CaricaDataGrid(dgRisultatoRicerca)

                End If
            Else
                cmdSalva.Visible = False
                cmdRespingi.Visible = False
                cmdModificaUNSC.Visible = False
                BloccaDataInizio()
                ChekBloccaIdonei()
            End If
        Else
            '=========================================
            'SE NON C'E' IL TITOLARIO INFORMO L'UTENTE
            '=========================================
            lblMessaggioAlert.Visible = True
            'imgAlert.Visible = True
            lblMessaggioAlert.Text = "Attenzione per il bando di riferimento del progetto non è associato un titolario. Provvedere all'associazione e ripetere l'operazione."
            CaricaDataGrid(dgRisultatoRicerca)
        End If
    End Sub
    Protected Sub cmdConferma_Click(sender As Object, e As EventArgs) Handles cmdConferma.Click
        If ValidaData() <> "" Then
            lblMessaggi.Visible = True
            lblMessaggi.ForeColor = Color.Red
            lblMessaggi.Text = mesg
            Exit Sub
        Else
            If CheckIntervalloDate() <> "" Then
                lblMessaggi.Visible = True
                lblMessaggi.ForeColor = Color.Red
                lblMessaggi.Text = mesg
                Exit Sub
            End If
        End If

        If lblStatoGraduatoria.Text = "Registrata" And dgRisultatoRicerca.Items.Count = 0 Then
        Else
            If leggidata(txtinizioEff.Text, txtDataOdierna.Text) <> "" Then
                lblMessaggi.Visible = True
                lblMessaggi.ForeColor = Color.Red
                lblMessaggi.Text = mesg
                Exit Sub
            Else
                txtFineEff.Text = DateAdd("d", -1, DateAdd("mm", NumeroMesiProgetto(Request.QueryString("IdAttivita")), txtinizioEff.Text))
            End If
        End If


        'inserisco cronologia stato graduatoria vecchio stato
        strsql = "Insert into CronologiaAttivitàSediAssegnazione (idAttivitàSedeAssegnazione,StatoGraduatoria," & _
        " usernamestato,datacronologia )" & _
        " select " & lblidattivitasedeassegnazione.Text & ",StatoGraduatoria,'" & Session("Utente") & "',getdate()" & _
        " from attivitàsediassegnazione where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
        cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        'Modifico stato graduatoria inserendo chi ha effettuatio la modifica e la data
        strsql = "Update attivitàsediassegnazione set statograduatoria=2," & _
        " usernamestato='" & Session("Utente") & "',dataultimostato=getdate(), NoteGraduatoria='" & ClsServer.NoApice(txtNote.Text) & "'" & _
        " where idattivitàsedeassegnazione=" & lblidattivitasedeassegnazione.Text & ""
        cmdCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        lblStatoGraduatoria.Text = "Presentata"
        cmdConferma.Visible = False
        imgmess.Visible = True
        lblMessaggi.Visible = True
        cmdModificaUNSC.Visible = False
        BloccaDataInizio()

        lblMessaggi.Text = "La Graduatoria è stata Presentata con successo."
    End Sub
    Protected Sub cmdModificaUNSC_Click(sender As Object, e As EventArgs) Handles cmdModificaUNSC.Click
       

        'alertgmo.Visible = False
        'alertgmo.Text = ""
        If ContaVol() = True Then
            ModificaGraduatoria()
            strsql = " SELECT COUNT(*) as VolAmm " & _
           " FROM graduatorieentità s1 " & _
           " WHERE s1.idattivitàsedeassegnazione in (select attivitàsediassegnazione.idattivitàsedeassegnazione  " & _
           " from attivitàsediassegnazione where attivitàsediassegnazione.idattività=" & lblidattivita.Text & ")" & _
           " and ammesso=1"
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrGenerico.Read()
            lblVolSel.Text = dtrGenerico("VolAmm")
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            If lblStatoGraduatoria.Text = "Confermata" Then
                ChekBloccaIdonei()
            End If

        Else
            lblMessaggioAlert.Visible = True
            'imgAlert.Visible = True
            lblMessaggioAlert.Text = "Attenzione il numero dei Volontari Selezionati ha raggiunto la capienza del Progetto."
            CaricaDataGrid(dgRisultatoRicerca)
            If lblStatoGraduatoria.Text = "Confermata" Then
                ChekBloccaIdonei()
            End If
        End If
        Dim esitogmo As String = String.Empty
        esitogmo = SegnalazioneCoperturaGMO(Request.QueryString("idattivitaSedeAssegnazione"), Session("Utente"))
        If esitogmo = "" Then
            alertgmo.Visible = False
        Else
            alertgmo.Visible = True
            alertgmo.Text = esitogmo
        End If
    End Sub

    Protected Sub cmdAggiungi_Click(sender As Object, e As EventArgs) Handles cmdAggiungi.Click
        Session("idsedeattuazione") = Nothing
        'imgAlert.Visible = False
        lblMessaggioAlert.Visible = False
        lblMessaggioAlert.Text = ""
        alertgmo.Visible = False
        alertgmo.Text = ""
        Response.Redirect("WfrmVolontari.aspx?idattivitaSedeAssegnazione=" & lblidattivitasedeassegnazione.Text & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&presenta=" & lblpresentata.Text & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&Disabilita=OK")
    End Sub

    Protected Sub ImgSalvaProt_Click(sender As Object, e As EventArgs) Handles ImgSalvaProt.Click
        Call SalvaProtocolli()
        UpdateVolontariGraduatoria(Request.QueryString("IDAttivitasedeAssegnazione"))
    End Sub

    Protected Sub imgAnnullaConferma_Click(sender As Object, e As EventArgs) Handles imgAnnullaConferma.Click
        'variabile stringa per comporre le query
        Dim strSql As String
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction

        Try

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            'myCommand = New SqlClient.SqlCommand
            'myCommand.Connection = Session("conn")
            'myCommand.Transaction = MyTransaction

            'inserisco cronologia stato graduatoria vecchio stato
            strSql = "Insert into CronologiaAttivitàSediAssegnazione (idAttivitàSedeAssegnazione,StatoGraduatoria, "
            strSql = strSql & "usernamestato,datacronologia ) "
            strSql = strSql & "select " & Request.QueryString("IDAttivitasedeAssegnazione") & ",StatoGraduatoria,'" & Session("Utente") & "',getdate() "
            strSql = strSql & "from attivitàsediassegnazione where idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & ""

            myCommand = New SqlClient.SqlCommand(strSql, Session("conn"))
            myCommand.Transaction = MyTransaction
            myCommand.ExecuteNonQuery()

            'myCommand = New SqlClient.SqlCommand(strSql, Session("conn"))
            'myCommand.ExecuteNonQuery()

            'cronologia entità
            strSql = "insert into CronologiaEntità (IdEntità, IdStatoEntità, UsernameStato, DataChiusura, DataCronologia, NoteStato, IdCausaleChiusura) "
            strSql = strSql & "select entità.IdEntità, entità.IdStatoEntità, entità.UsernameStato, entità.DataChiusura, entità.DataUltimoStato, entità.NoteStato, entità.IdCausaleChiusura "
            strSql = strSql & "from entità "
            strSql = strSql & "inner join GraduatorieEntità on entità.IdEntità=GraduatorieEntità.IdEntità "
            strSql = strSql & "where Ammesso=1 and IdAttivitàSedeAssegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & ""

            'myCommand = New SqlClient.SqlCommand(strSql, Session("conn"))
            'myCommand.ExecuteNonQuery()
            myCommand.Connection = Session("conn")
            myCommand.CommandText = strSql
            myCommand.ExecuteNonQuery()

            'elenco identità della sede selezionata da ripristinare(Datainizioservizio=null, datafineservizio=null, idstatoentità=idstatoentità su cronologia)
            strSql = "update entità set DataInizioServizio=null, DataFineServizio=null, IdStatoEntità=1 where identità in (SELECT entità.identità "
            strSql = strSql & "FROM AttivitàSediAssegnazione INNER JOIN "
            strSql = strSql & "GraduatorieEntità ON AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione = GraduatorieEntità.IdAttivitàSedeAssegnazione INNER JOIN "
            strSql = strSql & "entità ON GraduatorieEntità.IdEntità = entità.IDEntità INNER JOIN "
            strSql = strSql & "attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
            strSql = strSql & "WHERE (AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione='" & Request.QueryString("IDAttivitasedeAssegnazione") & "'))"

            myCommand.Connection = Session("conn")
            myCommand.CommandText = strSql
            myCommand.ExecuteNonQuery()

            strSql = "delete from attivitàentità where identità in (SELECT entità.identità "
            strSql = strSql & "FROM AttivitàSediAssegnazione INNER JOIN "
            strSql = strSql & "GraduatorieEntità ON AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione = GraduatorieEntità.IdAttivitàSedeAssegnazione INNER JOIN "
            strSql = strSql & "entità ON GraduatorieEntità.IdEntità = entità.IDEntità INNER JOIN "
            strSql = strSql & "attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
            strSql = strSql & "WHERE (AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione='" & Request.QueryString("IDAttivitasedeAssegnazione") & "'))"

            myCommand.Connection = Session("conn")
            myCommand.CommandText = strSql
            myCommand.ExecuteNonQuery()

            strSql = "update AttivitàSediAssegnazione set StatoGraduatoria=2, datainiziodifferita = null, datafinedifferita = null where IDAttivitàSedeAssegnazione='" & Request.QueryString("IDAttivitasedeAssegnazione") & "'"

            myCommand.Connection = Session("conn")
            myCommand.CommandText = strSql
            myCommand.ExecuteNonQuery()


            'Controllo che nn ci sia nessuna graduatoria presentata, se non ci sono 
            'aggiorno la datainizioattività e datafineattività a NULL
            Dim dtrLeggiDati As SqlClient.SqlDataReader
            'eseguo la query e passo il risultato al datareader
            strSql = "SELECT IDAttivitàSedeAssegnazione FROM AttivitàSediAssegnazione WHERE "
            strSql = strSql & "STATOGRADUATORIA = 3 AND "
            strSql = strSql & "IDAttività=" & Request.QueryString("IdAttivita")

            myCommand.Connection = Session("conn")
            myCommand.CommandText = strSql
            dtrLeggiDati = myCommand.ExecuteReader

            If dtrLeggiDati.HasRows = False Then
                strSql = "UPDATE Attività set datainizioattività=NULL, datafineattività=NULL where IDAttività=" & Request.QueryString("IdAttivita")
                dtrLeggiDati.Close()
                myCommand.Connection = Session("conn")
                myCommand.CommandText = strSql
                myCommand.ExecuteNonQuery()

            End If
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
            'myCommand = Nothing

            MyTransaction.Commit()



        Catch ex As Exception
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
        End Try
        myCommand.Dispose()
        MyTransaction.Dispose()
        myCommand = Nothing
        Chiudi()
    End Sub
    'Protected Sub Selection_ChangeDataDa(sender As Object, e As EventArgs) Handles CalendarDa.SelectionChanged
    '    txtinizioEff.Text = CalendarDa.SelectedDate.ToShortDateString()
    '    CalendarDa.Dispose()
    'End Sub
    Protected Sub cmdStoricoNote_Click(sender As Object, e As EventArgs) Handles cmdStoricoNote.Click
        If cmdStoricoNote.Text = "Visualizza Note" Then
            RigaGrigliaNote.Visible = True
            CaricaDataGridNote(dtgStoricoNote)
            dtgStoricoNote.Visible = True
            cmdStoricoNote.Text = "Nascondi Note"
        Else
            dtgStoricoNote.Visible = False
            cmdStoricoNote.Text = "Visualizza Note"
            RigaGrigliaNote.Visible = False
        End If
    End Sub

    Protected Sub cmdInsNote_Click(sender As Object, e As EventArgs) Handles cmdInsNote.Click
        If cmdInsNote.Text = "Inserisci Nota" Then
            RigaNoteIns.Visible = True
            'RigaGrigliaNote.Visible = True
            'RigaNote.Visible = False   'ADC
            'lblNote.Visible = False
            'cmdInsNote.Visible = False
            'txtNote.Visible = False
            'cmdInsNote.Visible = False
            cmdInsNote.Text = "Chiudi Nota"
        Else
            cmdInsNote.Text = "Inserisci Nota"
            RigaNoteIns.Visible = False
        End If

       
    End Sub
    Private Sub dtgStoricoNote_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgStoricoNote.PageIndexChanged
        dtgStoricoNote.SelectedIndex = -1
        dtgStoricoNote.EditItemIndex = -1
        dtgStoricoNote.CurrentPageIndex = e.NewPageIndex
        CaricaDataGridNote(dtgStoricoNote)
    End Sub
    Private Sub CaricaDataGridNote(ByRef GridDaCaricareNote As DataGrid)
        strSql1 = "Select * from CronologiaNoteGraduatoria where IDAttivitàSedeAssegnazione ='" & Session("IdASA") & "' Order by IDCronologiaNoteGraduatoria desc"
        GridDaCaricareNote.DataSource = ClsServer.DataSetGenerico(strSql1, Session("conn"))
        GridDaCaricareNote.DataBind()
        GridDaCaricareNote.Visible = True
    End Sub
    Protected Sub CmdSalvaNota_Click(sender As Object, e As EventArgs) Handles CmdSalvaNota.Click
        If txtNuovaNota.Text.Trim <> "" Then
            strSql1 = "Insert Into CronologiaNoteGraduatoria (IDAttivitàSedeAssegnazione,NoteGraduatoria,DataNota,UserNameNota) Values ('" & Session("IdASA") & "','" & ClsServer.NoApice(txtNuovaNota.Text) & "', getdate(),'" & Session("Utente") & "')"
            Dim cmdNote As New SqlClient.SqlCommand
            cmdNote.CommandText = strSql1
            cmdNote.Connection = Session("Conn")
            cmdNote.ExecuteNonQuery()

            strSql1 = "Update AttivitàSediAssegnazione Set NoteGraduatoria ='" & ClsServer.NoApice(txtNuovaNota.Text) & "', DataNota=getdate(), UserNameNota='" & Session("Utente") & "' Where IDAttivitàSedeAssegnazione='" & Session("IdASA") & "'"
            cmdNote.CommandText = strSql1
            cmdNote.Connection = Session("Conn")
            cmdNote.ExecuteNonQuery()

            CaricaDataGridNote(dtgStoricoNote)

            txtNuovaNota.Text = ""
            If Not cmdNote Is Nothing Then
                cmdNote.Dispose()
                cmdNote = Nothing
            End If
            'ADC
            'dtgStoricoNote.Visible = False
            RigaNoteIns.Visible = False
            'RigaGrigliaNote.Visible = False
            'RigaNote.Visible = True  'adc
            lblNote.Visible = True
            cmdInsNote.Visible = True
            txtNote.Visible = True
            cmdInsNote.Text = "Inserisci Nota"

            
            PrimioCaricamento()
        End If
    End Sub
    Private Sub dtgStoricoNote_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgStoricoNote.ItemCommand
        If e.CommandName = "Rimuovi" Then
            strSql1 = "delete CronologiaNoteGraduatoria where idCronologiaNoteGraduatoria='" & e.Item.Cells(0).Text & "'"
            cmdGenerico = ClsServer.EseguiSqlClient(strSql1, Session("conn"))
            'FZ 08/01/07
            'In fase di cancellazione delle note, aggiorno il campo noteGRADUATORIA in ATTIVITàSEDIASSEGNAZIONE
            'Con l'ultima nota rimasta salvata. Se non ci sono piu' note, allora imposto il campo a NULL
            'Leggo in CRONOLOGIANOTEGRADUATORIA tutti i record legati alla graduatoria presa in esame
            Dim dtrLeggiDati As SqlClient.SqlDataReader
            strSql1 = "SELECT TOP 1 IDCronologiaNoteGraduatoria, IDAttivitàSedeAssegnazione, NoteGraduatoria " & _
                     "FROM CronologiaNoteGraduatoria WHERE (IDAttivitàSedeAssegnazione = " & Session("IdASA") & ") " & _
                     "ORDER BY IDCronologiaNoteGraduatoria DESC"

            dtrLeggiDati = ClsServer.CreaDatareader(strSql1, Session("conn"))
            If dtrLeggiDati.Read = True Then
                'Se c'è una nota rimasta allora faccio l'update su ATTIVITàSEDIASSEGNAZIONE
                Dim AppoNOte As String = dtrLeggiDati("NoteGraduatoria")

                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
                strSql1 = "UPDATE AttivitàSediAssegnazione SET NOTEGRADUATORIA = '" & AppoNOte & "' " & _
                "WHERE IDAttivitàSedeAssegnazione = " & Session("IdASA")




                Dim Command As SqlClient.SqlCommand
                Command = New SqlClient.SqlCommand(strSql1, Session("conn"))
                Command.ExecuteNonQuery()

            Else
                'non ci sono più note salvate, quindi imposto il campo su ATTIVITàSEDIASSEGNAZIONE a NULL

                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
                strSql1 = "UPDATE AttivitàSediAssegnazione SET NOTEGRADUATORIA = NULL " & _
                "WHERE IDAttivitàSedeAssegnazione = " & Session("IdASA")
                'Eseguo lo script di aggiornamento della pagina sottostante

                Dim Command As SqlClient.SqlCommand
                Command = New SqlClient.SqlCommand(strSql1, Session("conn"))
                Command.ExecuteNonQuery()


            End If
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If
            'FZ fine


        End If
        CaricaDataGridNote(dtgStoricoNote)
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    'Protected Sub CmdChiudiNota_Click(sender As Object, e As EventArgs) Handles CmdChiudiNota.Click
    '    'dtgStoricoNote.Visible = False
    '    RigaNoteIns.Visible = False
    '    'RigaGrigliaNote.Visible = False
    '    'RigaNote.Visible = True  'ADC
    '    lblNote.Visible = True
    '    cmdInsNote.Visible = True
    '    txtNote.Visible = True
    'End Sub

    Function CheckIntervalloDate() As String
        mesg = ""
        Dim errore As StringBuilder = New StringBuilder()
        Dim dataFine As Date
        Dim dataInizio As Date
        Dim dataDiffValida As Boolean = True

        dataInizio = txtinizioEff.Text
        dataFine = DateAdd("YYYY", 1, DateAdd("d", -1, dataInizio))
        'If (Date.TryParse(txtFineEff.Text, dataFine) = True) Then
        'If DateDiff("YYYY", dataInizio, DateAdd("d", 1, dataFine)) Then
        '    errore.Append("DATA ERRATA. <br/>")
        '    lblMessaggi.Text = lblMessaggi.Text + errore.ToString()
        '    mesg = lblMessaggi.Text
        'Else
        '    mesg = ""
        'End If


        Return mesg
        'End If

    End Function

    Function ValidaDataDifferita() As String
        mesg = ""
        Dim data As Date
        Dim dataprogetto As Date
        Dim dataValida As Boolean = True

        Dim errore As StringBuilder = New StringBuilder()
        If (txtInizioServizioSede.Text <> String.Empty) Then
            If Date.TryParse(txtInizioServizioSede.Text, data) = False Then
                errore.AppendLine("Il campo 'Inizio Servizio Sede' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>")
                dataValida = False
            Else
                data = txtInizioServizioSede.Text
                dataprogetto = txtinizioEff.Text
                If data <= dataprogetto Then
                    errore.AppendLine("La data inizio servizio sede deve essere successiva alla data inizio del progetto. <br/>")
                    dataValida = False
                End If
            End If
        End If

        If (txtFineServizioSede.Text <> String.Empty) Then
            If Date.TryParse(txtFineServizioSede.Text, data) = False Then
                errore.AppendLine("Il campo  'Fine Servizio Sede' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>")
                dataValida = False
            End If
        End If

        If (dataValida = False) Then
            mesg = errore.ToString()
            lblMessaggi.Text = errore.ToString()
            lblMessaggi.Visible = True
        End If
        Return mesg
    End Function

    Function ValidaData() As String
        mesg = ""
        Dim data As Date
        Dim dataValida As Boolean = True

        Dim errore As StringBuilder = New StringBuilder()
        If (txtinizioEff.Text <> String.Empty) Then
            If Date.TryParse(txtinizioEff.Text, data) = False Then
                errore.AppendLine("Il campo 'Data Inizio' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>")
                dataValida = False
            End If
        End If

        If (txtFineEff.Text <> String.Empty) Then
            If Date.TryParse(txtFineEff.Text, data) = False Then
                errore.AppendLine("Il campo  data Fine del' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>")
                dataValida = False
            End If
        End If
        If (txtInizioRecupero.Text <> String.Empty) Then
            If Date.TryParse(txtInizioRecupero.Text, data) = False Then
                errore.AppendLine("Il campo 'Data Inizio Recupero Servizio' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>")
                dataValida = False
            End If
        End If
        If (dataValida = False) Then
            mesg = errore.ToString()
            lblMessaggi.Text = errore.ToString()
            lblMessaggi.Visible = True
        End If
        Return mesg
    End Function
    Function leggidata(ByVal inizio As Date, ByVal Odierna As Date) As String

        If DateDiff("d", Odierna.ToShortDateString, inizio) <= 0 Then
            mesg = "La data di Inizio Attivita' non puo' essere antecedente alla data Odierna."
        End If



        Return mesg
    End Function
    Function ControllaCampi()
        Dim dtgItem As DataGridItem
        For Each dtgItem In dgRisultatoRicerca.Items
            Dim txtNumPunteggio As TextBox = DirectCast(dtgItem.FindControl("Text1"), TextBox)

            Dim valoretext As String
            valoretext = txtNumPunteggio.Text

            If (Not IsNumeric(valoretext)) And (Asc(valoretext) <> 8) Then
                SetFocus(dtgItem.FindControl("Text1").ClientID)
                Return False
                Exit Function
            End If
        Next
        Return True
    End Function

    Private Function ControlloDataNeet(ByVal IdAttivitaSedeAssegnazione As Integer, ByVal DataAvvio As String) As String

        Dim sqlDAP As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strEsito As String
        Dim strNomeStore As String = "[SP_GG_VERIFICA_DATE_CONTROLLI_NEET]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdAttivitàSedeAssegnazione", SqlDbType.Int).Value = IdAttivitaSedeAssegnazione
            sqlDAP.SelectCommand.Parameters.Add("@DataAvvio", SqlDbType.Date).Value = CDate(DataAvvio)

            Dim sparam0 As SqlClient.SqlParameter
            sparam0 = New SqlClient.SqlParameter
            sparam0.ParameterName = "@Esito"
            sparam0.Size = 100
            sparam0.SqlDbType = SqlDbType.Int
            sparam0.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam0)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Motivazione"
            sparam1.Size = 1000
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam1)

            sqlDAP.Fill(dataSet)
            strEsito = sqlDAP.SelectCommand.Parameters("@Motivazione").Value
            Return strEsito
        Catch ex As Exception
            lblMessaggioAlert.Visible = True
            lblMessaggioAlert.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            Exit Function
        End Try


    End Function

    Private Function ControlloGraduatoria(ByVal IdAttivita As Integer, ByVal DataAvvio As String) As String

        Dim sqlDAP As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strEsito As String
        Dim strNomeStore As String = "[SP_GRADUATORIA_CONTROLLI_V3]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = IdAttivita
            sqlDAP.SelectCommand.Parameters.Add("@DataAvvio", SqlDbType.Date).Value = CDate(DataAvvio)
            sqlDAP.SelectCommand.Parameters.Add("@IDAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = -1

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam1)

            sqlDAP.Fill(dataSet)
            strEsito = sqlDAP.SelectCommand.Parameters("@Esito").Value
            Return strEsito
        Catch ex As Exception
            lblMessaggioAlert.Visible = True
            lblMessaggioAlert.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            Exit Function
        End Try


    End Function

    Private Function GeneraFascicoliVolontari(ByVal strUserName As String, ByVal strIdEntita As String, ByVal IntIdLog As Integer) As String
        'creata da simona cordella il 14/06/2017
        'routine che richiamoa in modo asincrono il metodo del WSINTERNO per la generazione dei Fascicolo dei volontari prnseti in graduatoria
        Dim localWS As New WSHeliosInterno.HeliosInterno
        'Dim ds As DataSet
        'Dim i As Integer
        'Dim strCodiceProgetto As String
        Dim ResultAsinc As IAsyncResult

        'richiamo WSDocumentazione

        localWS.Timeout = 1000000
        ResultAsinc = localWS.BeginGeneraFascicoloVolontari(strUserName, strIdEntita, IntIdLog, Nothing, "")

        'localWS.GenerazioneBOX16_BOX19Async(IdBandoAttivita, Session("Utente"))

        ' ResultAsinc = localWS.beg(IdBandoAttivita, Session("Utente"), Nothing, "")


    End Function

    Private Function NumeroMesiProgetto(ByVal IDAttività As Integer) As Integer
        Dim NMesi As Integer
        strsql = " SELECT nmesi from attività where idattività=" & IDAttività

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()

        NMesi = dtrGenerico("nmesi")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        Return NMesi
    End Function

    Private Function BandoDol() As Boolean

        Dim FlagDOL As Integer

        strsql = " SELECT convert(int,d.DOL) as DOL from attivitàsediassegnazione a inner join attività b on a.idattività = b.idattività inner join bandiattività c on b.idbandoattività = c.idbandoattività inner join bando d on c.idbando = d.idbando where a.idattivitàsedeassegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione")

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()

        FlagDOL = dtrGenerico("DOL")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        If FlagDOL = 0 Then
            Return False
        Else
            Return True
        End If

    End Function
    Private Sub ControlloVisualizzaDateDiff(ByVal strIdAttivitasedeassegnazione As String)

        Dim strUtente As String
        Dim SqlCmd As New SqlClient.SqlCommand

        strUtente = Session("Utente")

        Try
            SqlCmd.CommandText = "SP_GRADUATORIA_VISUALIZZA_PARTENZA_DIFFERITA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@Utente", SqlDbType.VarChar, 100).Value = strUtente
            SqlCmd.Parameters.Add("@IdAttivitàSedeAssegnazione", SqlDbType.Int).Value = strIdAttivitasedeassegnazione


            SqlCmd.Parameters.Add("@AbilitaPulsante", SqlDbType.VarChar)
            SqlCmd.Parameters("@AbilitaPulsante").Size = 10
            SqlCmd.Parameters("@AbilitaPulsante").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@AbilitaDiv", SqlDbType.VarChar)
            SqlCmd.Parameters("@AbilitaDiv").Size = 10
            SqlCmd.Parameters("@AbilitaDiv").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            strAbilitaPulsante.Value = SqlCmd.Parameters("@AbilitaPulsante").Value()
            strAbilitaDiv.Value = SqlCmd.Parameters("@AbilitaDiv").Value()


        Catch ex As Exception

        Finally

        End Try
    End Sub


    Protected Sub ImgPartenzaDiff_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgPartenzaDiff.Click
        PartDiff.Visible = True
    End Sub

    Private Sub cmdAnnullaDifferita_Click(sender As Object, e As System.EventArgs) Handles cmdAnnullaDifferita.Click
        PartDiff.Visible = False
    End Sub

    Protected Sub imgCronoRiattivazione_Click(sender As Object, e As EventArgs) Handles imgCronoRiattivazione.Click

        Dim resp As StringBuilder = New StringBuilder()
        Dim windowOption As String = "dependent=no,scrollbars=yes,status=no,resizable=yes,width=1000px ,height=400px"
        resp.Append("<script  type=""text/javascript"">" & vbCrLf)
        resp.Append("myWin = window.open('WfrmCronologiaRiattivazione.aspx?IdAttivitaSedeAssegnazione=" + Request.QueryString("idattivitaSedeAssegnazione") + "', 'win'" + ",'" + windowOption + "')")
        resp.Append("</script>")
        Response.Write(resp.ToString())

    End Sub

    Public Function abilitalinkRimodulazione(ByVal idattivitaSedeAssegnazione As Integer)
        'SP_RIATTIVAZIONE_VERIFICA_CRONO
        Dim strUtente As String
        Dim SqlCmd As New SqlClient.SqlCommand


        Try
            SqlCmd.CommandText = "SP_RIMODULAZIONEPROGETTI_VERIFICA_CRONO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IDEntità", SqlDbType.Int).Value = 0
            SqlCmd.Parameters.Add("@IDAttivitàSedeAssegnazione", SqlDbType.Int).Value = idattivitaSedeAssegnazione

            SqlCmd.Parameters.Add("@Esito", SqlDbType.VarChar, 5)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output



            SqlCmd.ExecuteNonQuery()

            Return SqlCmd.Parameters("@Esito").Value()


        Catch ex As Exception
            'Esito = "ERR"

        Finally

        End Try
    End Function

    Public Function abilitalink(ByVal idattivitaSedeAssegnazione As Integer)
        'SP_RIATTIVAZIONE_VERIFICA_CRONO
        Dim strUtente As String
        Dim SqlCmd As New SqlClient.SqlCommand


        Try
            SqlCmd.CommandText = "SP_RIATTIVAZIONE_VERIFICA_CRONO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IDEntità", SqlDbType.Int).Value = 0
            SqlCmd.Parameters.Add("@IDAttivitàSedeAssegnazione", SqlDbType.Int).Value = idattivitaSedeAssegnazione

            SqlCmd.Parameters.Add("@Esito", SqlDbType.VarChar, 5)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output



            SqlCmd.ExecuteNonQuery()

            Return SqlCmd.Parameters("@Esito").Value()


        Catch ex As Exception
            'Esito = "ERR"

        Finally

        End Try
    End Function
    Public Function abilitaFCGG(ByVal idattivitaSedeAssegnazione As Integer, ByVal utente As String)

        Dim strUtente As String
        Dim SqlCmd As New SqlClient.SqlCommand


        Try
            SqlCmd.CommandText = "SP_GG_VisualizzaPulsateForzaControlloGraduatoria"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")


            SqlCmd.Parameters.Add("@IDAttivitàSedeAssegnazione", SqlDbType.Int).Value = idattivitaSedeAssegnazione
            SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar, 10).Value = utente
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            Return SqlCmd.Parameters("@Esito").Value


        Catch ex As Exception
            'Esito = "ERR"

        Finally

        End Try
    End Function

    Protected Sub CmdFCGG_Click(sender As Object, e As EventArgs) Handles CmdFCGG.Click
        CmdFCGG.Visible = False
        Dim messaggio As String = String.Empty
        If FCmdFCGG(messaggio) = 0 Then
            lblMessaggi.Visible = False
            lblMessaggioAlert.Visible = True
            lblMessaggioAlert.Text = messaggio
        Else
            lblMessaggioAlert.Visible = False
            lblMessaggi.Visible = True
            lblMessaggi.Text = messaggio
        End If
    End Sub
    Private Function FCmdFCGG(ByRef messaggio As String) As Boolean

        Dim strUtente As String
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim esito As Integer

        Try
            SqlCmd.CommandText = "SP_GG_ForzaControlloGraduatoria"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")


            SqlCmd.Parameters.Add("@IDAttivitàSedeAssegnazione", SqlDbType.Int).Value = Request.QueryString("idattivitaSedeAssegnazione")
            SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar, 10).Value = Session("Utente")
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output
            SqlCmd.ExecuteNonQuery()

            ' Return SqlCmd.Parameters("@Esito").Value
            esito = SqlCmd.Parameters("@Esito").Value
            messaggio = SqlCmd.Parameters("@MESSAGGIO").Value

            Return esito

        Catch ex As Exception
            'Esito = "ERR"

        Finally

        End Try
    End Function
    Public Function SegnalazioneCoperturaGMO(ByVal idattivitaSedeAssegnazione As Integer, ByVal utente As String)

        Dim strUtente As String
        Dim SqlCmd As New SqlClient.SqlCommand


        Try
            SqlCmd.CommandText = "SP_GMO_VisualizzaSegnalazioneCopertura"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")


            SqlCmd.Parameters.Add("@IDAttivitàSedeAssegnazione", SqlDbType.Int).Value = idattivitaSedeAssegnazione
            SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar, 10).Value = utente
            SqlCmd.Parameters.Add("@Esito", SqlDbType.VarChar, 1000)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            Return SqlCmd.Parameters("@Esito").Value


        Catch ex As Exception
            'Esito = "ERR"

        Finally

        End Try
    End Function
    Public Function lblprogramma(ByVal idattivita As Integer) As DataTable


        Dim SqlCmd As New SqlClient.SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        lblprogramma = New DataTable

        Try
            SqlCmd.CommandText = "SP_GRADUATORIE_DATI_PROGRAMMA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")


            SqlCmd.Parameters.Add("@IDAttività", SqlDbType.Int).Value = idattivita

            ' SqlCmd.ExecuteNonQuery()
            dataAdapter.SelectCommand = SqlCmd
            dataAdapter.Fill(lblprogramma)
            Return lblprogramma


        Catch ex As Exception
            'Esito = "ERR"

        Finally

        End Try
    End Function

    Private Sub ElencoProgettiProgramma()

        Dim sqlDAP As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_GRADUATORIE_ELENCO_PROGETTI]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdAttività", SqlDbType.Int).Value = Request.QueryString("idattivita")


            sqlDAP.Fill(dataSet)

            If dataSet.Tables(0).Rows.Count > 0 Then
                'DivControlloVolontario.Visible = True
                Session("RisultatoProgetti") = dataSet.Tables(0)

                dtgProgrammi1.DataSource = dataSet.Tables(0)
                dtgProgrammi1.DataBind()
            End If


        Catch ex As Exception

            Exit Sub
        End Try
    End Sub
    Protected Sub ConfiguraPostBackSezioni()
        Dim isPostBack As Boolean = Me.IsPostBack
        Me.hdnIsPostbackProgetto1.Value = isPostBack
    End Sub




    Private Sub dtgProgrammi1_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dtgProgrammi1.PageIndexChanging
        dtgProgrammi1.PageIndex = e.NewPageIndex
        dtgProgrammi1.DataSource = Session("RisultatoProgetti")
        dtgProgrammi1.DataBind()
    End Sub

    Private Sub imgCronoRimodulazione_Click(sender As Object, e As System.EventArgs) Handles imgCronoRimodulazione.Click

        Dim resp As StringBuilder = New StringBuilder()
        Dim windowOption As String = "dependent=no,scrollbars=yes,status=no,resizable=yes,width=1000px ,height=400px"
        resp.Append("<script  type=""text/javascript"">" & vbCrLf)
        resp.Append("myWin = window.open('WfrmCronologiaRimodulazioneProgetti.aspx?IdAttivitaSedeAssegnazione=" + Request.QueryString("idattivitaSedeAssegnazione") + "', 'win'" + ",'" + windowOption + "')")
        resp.Append("</script>")
        Response.Write(resp.ToString())

    End Sub


End Class
