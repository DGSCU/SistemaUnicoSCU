Public Class informazioniente
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        PopolaMaschera()
        CaricaEntiSettori()
    End Sub
    Private Sub PopolaMaschera()
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strsql As String
        Dim MyCommand As SqlClient.SqlDataAdapter
        Dim DsSedi As DataSet = New DataSet

        'Generata da New JonaTolls

        'strsql = "Select * from Enti Where idEnte =" & Session("IdEnte") & ""
        strsql = "select day(datacontrolloemail)as ggDCEmail,month(datacontrolloemail)as monthDCEmail,year(datacontrolloemail)as yearDCEmail," & _
        " day(datacontrollohttp)as ggDChttp,month(datacontrollohttp)as monthDChttp,year(datacontrollohttp)as yearDChttp, statienti.statoente," & _
        " classiaccreditamento.classeaccreditamento,classiaccreditamento.EntiInPartenariato, " & _
        " enti.Tipologia,enti.CodiceFiscale,isnull(enti.CodiceRegione,'Assente')as codiceregione," & _
        " enti.Datacontrolloemail,enti.Datacontrollohttp," & _
        " enti.dataCostituzione,enti.DataRicezioneCartacea,enti.dataCreazioneRecord,enti.Denominazione,enti.EstremiDeliberaStrutturaGestione," & _
        " enti.http,enti.Datacontrollohttp,enti.httpvalido," & _
        " enti.Email,enti.EmailCertificata,enti.Datacontrolloemail,enti.emailvalido,enti.PartitaIva,enti.NoteRichiestaRegistrazione," & _
        " enti.TelefonoRichiestaRegistrazione, CASE isnull(Enti.Firma,0) WHEN 0 THEN 'NO' ELSE 'SI' END AS FIRMA,  " & _
        " enti.PrefissoTelefonoRichiestaRegistrazione,enti.PrefissoFax,enti.Fax,entipassword.Username,enti.dataultimaClasseaccreditamento," & _
        " enti.idclasseaccreditamentorichiesta,cr.classeaccreditamento as classeaccreditamentorichiesta,enti.idclasseaccreditamento,isnull(enti.CodiceFiscaleArchivio,'') as CodiceFiscaleArchivio, isnull(enti.PartitaIVAArchivio,'') as PartitaIVAArchivio " & _
        " from enti " & _
        " inner join classiaccreditamento on (classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamento)" & _
        " inner join classiaccreditamento cr on (cr.idclasseaccreditamento=enti.idclasseaccreditamentorichiesta)" & _
        " inner join statienti on (statienti.idstatoente=enti.idstatoente) " & _
        " left join entipassword on (enti.idente=entipassword.idente)" & _
        " where enti.idente=" & Session("IdEnte") & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()
        If Not IsDBNull(dtrgenerico("StatoEnte")) Then
            lblStato.Text = dtrgenerico("StatoEnte")
        End If
        'If Not IsDBNull(dtrgenerico("Tipologia")) Then
        '    If UCase(dtrgenerico("Tipologia")) = "PRIVATO" Then
        '        ddlTipologia.SelectedIndex = trovaindex(dtrgenerico("Tipologia"), ddlTipologia)
        '    Else
        '        ddlTipologia.SelectedIndex = 1
        '        ddlGiuridica.SelectedIndex = trovaindex(dtrgenerico("Tipologia"), ddlGiuridica)
        '    End If

        'End If
        lblCodiceFiscale.Text = IIf(Not IsDBNull(dtrGenerico("CodiceFiscale")), dtrGenerico("CodiceFiscale"), "")
        LblEstremiDelibera.Text = IIf(Not IsDBNull(dtrGenerico("EstremiDeliberaStrutturaGestione")), dtrGenerico("EstremiDeliberaStrutturaGestione"), "")
        LblRichiedenteAccount.Text = IIf(Not IsDBNull(dtrGenerico("NoteRichiestaRegistrazione")), dtrGenerico("NoteRichiestaRegistrazione"), "")
        LblDataCostituzione.Text = IIf(Not IsDBNull(dtrGenerico("dataCostituzione")), dtrGenerico("dataCostituzione"), "")
        lblTipo.Text = IIf(Not IsDBNull(dtrGenerico("Tipologia")), dtrGenerico("Tipologia"), "")
        lblCompetenzaEnte.Text = Session("Competenza") 'IIf(Not IsDBNull(dtrGenerico("Tipologia")), dtrGenerico("Tipologia"), "")

        'txtCodRegione.Text = IIf(Not IsDBNull(dtrgenerico("CodiceRegione")), dtrgenerico("CodiceRegione"), "")
        'If Not IsDBNull(dtrgenerico("Datacontrolloemail")) Then
        '    txtdataControlloEmail.Text = dtrgenerico("ggDCemail") & "/" & IIf(CInt(dtrgenerico("monthDCemail")) < 10, "0" & dtrgenerico("monthDCemail"), dtrgenerico("monthDCemail")) & "/" & dtrgenerico("YearDCemail")
        'End If
        'If Not IsDBNull(dtrgenerico("Datacontrollohttp")) Then
        '    txtdataControllohttp.Text = dtrgenerico("ggDChttp") & "/" & IIf(CInt(dtrgenerico("monthDChttp")) < 10, "0" & dtrgenerico("monthDChttp"), dtrgenerico("monthDChttp")) & "/" & dtrgenerico("yearDChttp")
        'End If
        'Aggiunto da Alessandra Taballione il 28/02/2005
        'Implementazione del campo data di costituzione Ente
        'txtDataCostituzione.Text = IIf(Not IsDBNull(dtrgenerico("dataCostituzione")), dtrgenerico("DataCostituzione"), "")
        ''Aggiunto da Alessandra Taballione il 16/05/2005
        ''Implementazione del campo data Richiesta Ente
        'txtDataRicezioneCartacea.Text = IIf(Not IsDBNull(dtrgenerico("DataRicezioneCartacea")), dtrgenerico("DataRicezioneCartacea"), "")
        'If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
        '    txtDataRicezioneCartacea.ReadOnly = True
        '    txtDataRicezioneCartacea.BackColor = Color.Gainsboro
        'End If
        lblEnte.Text = IIf(Not IsDBNull(dtrGenerico("Denominazione")), dtrGenerico("Denominazione"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("CodiceRegione")), dtrGenerico("CodiceRegione"), "")
        'txtEstremiDSG.Text = IIf(Not IsDBNull(dtrgenerico("EstremiDeliberaStrutturaGestione")), dtrgenerico("EstremiDeliberaStrutturaGestione"), "")
        If Not IsDBNull(dtrgenerico("http")) Then
            lblHTTP.Text = dtrGenerico("http")
            'If Not IsDBNull(dtrgenerico("Datacontrollohttp")) Then
            '    If dtrgenerico("httpvalido") = True Then
            '        chkVerohttp.Checked = True
            '        chkFalsohttp.Checked = False
            '    Else
            '        chkVerohttp.Checked = False
            '        chkFalsohttp.Checked = True
            '    End If
            'Else
            '    chkVerohttp.Checked = False
            '    chkFalsohttp.Checked = False
            'End If
        End If
        If Not IsDBNull(dtrgenerico("Email")) Then
            lblEmail.Text = dtrGenerico("Email")
            'If Not IsDBNull(dtrgenerico("Datacontrolloemail")) Then
            '    If dtrgenerico("emailvalido") = True Then
            '        chkVeroEmail.Checked = True
            '        chkFalsoEmail.Checked = False
            '    Else
            '        chkVeroEmail.Checked = False
            '        chkFalsoEmail.Checked = True
            '    End If
            'Else
            '    chkVeroEmail.Checked = False
            '    chkFalsoEmail.Checked = False
            'End If
        End If
        lblEmailCertificata.Text = IIf(Not IsDBNull(dtrGenerico("EmailCertificata")), dtrGenerico("EmailCertificata"), "")
        LblFirma.Text = dtrGenerico("Firma")
        'Modificato da Alessandra Taballione il 09/06/2004
        'non verrà piu inserito il numero delle sedi ma ci sarà una tabella
        'che conterrà le informazioni relative alla classe Richiesta
        'CodiceFiscaleArchivio, isnull(enti.PartitaIVAArchivio) as PartitaIVAArchivio
        'txtCodFisArchiviato.Text = dtrgenerico("CodiceFiscaleArchivio")
        'txtPartitaIvaArchiviata.Text = dtrgenerico("PartitaIVAArchivio")
        lblPIVA.Text = IIf(Not IsDBNull(dtrGenerico("PartitaIva")), dtrGenerico("PartitaIva"), "")
        'txtRichiedente.Text = IIf(Not IsDBNull(dtrgenerico("NoteRichiestaRegistrazione")), dtrgenerico("NoteRichiestaRegistrazione"), "")
        lblTelefono.Text = IIf(Not IsDBNull(dtrGenerico("PrefissoTelefonoRichiestaRegistrazione")), dtrGenerico("PrefissoTelefonoRichiestaRegistrazione"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("TelefonoRichiestaRegistrazione")), dtrGenerico("TelefonoRichiestaRegistrazione"), "")
        'txtprefisso.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoTelefonoRichiestaRegistrazione")), dtrgenerico("PrefissoTelefonoRichiestaRegistrazione"), "")
        '**********************BLOCCO FAX**********************************************
        'txtprefissofax.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoFax")), dtrgenerico("PrefissoFax"), "")
        lblFax.Text = IIf(Not IsDBNull(dtrGenerico("PrefissoFax")), dtrGenerico("PrefissoFax"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("Fax")), dtrGenerico("Fax"), "")
        '**********************FINE BLOCCO FAX**********************************************
        'txtUtenza.Text = IIf(Not IsDBNull(dtrgenerico("Username")), dtrgenerico("Username"), "")
        lblClasseAttribuita.Text = dtrGenerico("classeaccreditamento")
        'lblClasseAttribuita.Text = dtrGenerico("idclasseaccreditamento")
        'lblDataClasse.Text = Mid(IIf(Not IsDBNull(dtrgenerico("dataultimaClasseaccreditamento")), dtrgenerico("dataultimaClasseaccreditamento"), ""), 1, 10) 'dtrgenerico("dataultimaClasseaccreditamento")
        'lblInPartenariato.Text = dtrgenerico("EntiInPartenariato")
        'txtidClasseRichiesta.Text = dtrgenerico("idclasseaccreditamentorichiesta")
        If Not IsDBNull(dtrGenerico("idclasseaccreditamentorichiesta")) And dtrGenerico("idclasseaccreditamentorichiesta") <> 6 Then
            lblClasseRichiesta.Text = dtrGenerico("classeaccreditamentorichiesta")

            'metto in sessione l'id della classe dell'ente selezionato
            'per controllare il change della combo delle classi
            'controllo aggiunto da BagaJon il 7/3/2006
            'Session("JonIdClasseAccreditamento") = dtrgenerico("idclasseaccreditamentorichiesta")
            'txtidClasseRichiesta.Text = dtrgenerico("idclasseaccreditamentorichiesta")
            'Modificato da Alessandra Taballione il 09/06/2004
            'implementazione delle informazioni della classe richiesta
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            'dtrgenerico = ClsServer.CreaDatareader("Select * from classiaccreditamento where idclasseaccreditamento=" & ddlClassi.SelectedValue & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            'dtrgenerico.Read()
            'If dtrgenerico.HasRows = True Then
            '    lblmaxsedi.Text = dtrgenerico("maxsedi")
            '    lblminSedi.Text = dtrgenerico("minSedi")
            '    lblmaxVol.Text = dtrgenerico("MaxEntitàperanno")
            '    lblminVol.Text = dtrgenerico("MinEntitàperanno")
            'End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        '-----------------------------------------Antonello ------------------------------------------------    
        'Dim ritorno As Integer
        ''dtrGenerico = ClsServer.LeggiStore(Session("IdEnte"), Session("Conn"))
        ''A
        'ritorno = ClsServer.LeggiStore_Italia(Session("idEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'lbltotsedi.Text = ritorno
        'ritorno = 0
        ''B
        'ritorno = ClsServer.LeggiStoreSediPartner_Italia(Session("idEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'lblsedipartner.Text = ritorno
        'ritorno = 0

        'ritorno = ClsServer.LeggiStoreMaxSediPartner(Session("idEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'lblmaxsedipartner.Text = ritorno
        'ritorno = 0

        'If lblsedipartner.Text <> "0" Then
        '    If lbltotsedi.Text <> "0" Then
        '        lblpercentosedipartner.Text = 100 * CInt(lblsedipartner.Text) / CInt(lbltotsedi.Text)
        '        lblpercentosedipartner.Text = Format(CDec(lblpercentosedipartner.Text), "##.##") & "%"
        '    Else
        '        lblpercentosedipartner.Text = "100%"
        '    End If
        'Else
        '    lblpercentosedipartner.Text = "0%"
        'End If


        '----------------------------------------Fine----------------------------------------------

        'Aggiunto da Alesssandra Taballione il 18/05/2005
        'Implementazione del tasto Ripristina 
        'Visibile solo unsc e solo se ente sospeso 
        'Cambio stato da sospeso a registrato
        'con clonologia
        'If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
        '    strsql = "select * from statienti " & _
        '    " inner join enti on enti.idstatoente=statienti.idstatoente " & _
        '    " where idente = " & Session("idEnte") & " And sospeso = 1 "
        '    If Not dtrgenerico Is Nothing Then
        '        dtrgenerico.Close()
        '        dtrgenerico = Nothing
        '    End If
        '    dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        '    If dtrgenerico.HasRows = True Then
        '        cmdRipristina.Visible = True
        '    Else
        '        cmdRipristina.Visible = False
        '    End If
        '    If Not dtrgenerico Is Nothing Then
        '        dtrgenerico.Close()
        '        dtrgenerico = Nothing
        '    End If
        'End If
        'Aggiunto da Alessandra Taballione il 11/11/2005
        'Ricerca e popolamento dei dati relativi alla sede principale dell'Ente
        strsql = "SELECT  entisedi.identesede,entisedi.idcomune,entisedi.indirizzo, entisedi.DettaglioRecapito, entisedi.civico,entisedi.cap,comuni.denominazione as comune," & _
        " provincie.provincia " & _
        " FROM entisedi " & _
        " INNER JOIN statientisedi on statientisedi.idstatoentesede=entisedi.idstatoentesede " & _
        " INNER JOIN entiseditipi ON entisedi.IDEnteSede = entiseditipi.IDEnteSede " & _
        " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " & _
        " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " & _
        " WHERE entisedi.IDEnte = " & Session("IdEnte") & " And entiseditipi.idtiposede = 1 and (statientisedi.attiva=1 or statientisedi.DefaultStato=1)"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            lblIndirizzo.Text = dtrGenerico("Indirizzo")
            lblNumero.Text = dtrGenerico("civico")
            lblCAP.Text = dtrGenerico("cap")
            lblComune.Text = dtrGenerico("Comune")
            LblDettaglioRecapito.Text = IIf(Not IsDBNull(dtrGenerico("DettaglioRecapito")), dtrGenerico("DettaglioRecapito"), "")
            'txtprovincia.Text = dtrgenerico("provincia")
            'txtIdSede.Text = dtrgenerico("identeSede")
            'txtIDComunes.Text = dtrgenerico("idcomune")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Sub CaricaEntiSettori()
        'aggiunto il 19/08/2014 da Simona Cordella
        Dim dtsSettori As DataSet
        'variabile stringa locale per costruire la query per le aree
        Dim strSql As String
        Dim item As DataGridItem
        strSql = "SELECT macroambitiattività.IDMacroAmbitoAttività, macroambitiattività.codifica + ' - ' + macroambitiattività.MacroAmbitoAttività as MacroAmbitoAttività FROM macroambitiattività INNER JOIN EntiSettori ON macroambitiattività.IDMacroAmbitoAttività = EntiSettori.IdMacroAmbitoAttività WHERE EntiSettori.IdEnte = " & Session("IdEnte") & " order by macroambitiattività.codifica "
        dtsSettori = ClsServer.DataSetGenerico(strSql, Session("conn"))

        'controllo se ci sono dei record
        If dtsSettori.Tables(0).Rows.Count > 0 Then
            dtgSettori.DataSource = dtsSettori
            dtgSettori.DataBind()
        End If

    End Sub
End Class