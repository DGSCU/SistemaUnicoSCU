Imports System.IO
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Security.Cryptography
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports
Imports System.Web.Mail
Imports System.Configuration
Imports System.Text
Imports System.Drawing
Imports Logger.Data
Public Class WfrmIstanzaPresentazione
    Inherits SmartPage
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim MyDataSet As New DataSet
    Dim MyDataTable As DataTable
    Public bCompetenza As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '***Generata da Gianluigi Paesani in data:13/07/04
        '***Inizializzo  verificando da dove arriva la chiamata se da modifica o da inserimento
        'Inserire qui il codice utente necessario per inizializzare la pagina
        checkSpid(True)
        Dim strappocompetenza As String
        If Not Session("LogIn") Is Nothing Then 'verifico validità log-in
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        '''''''''''''''''''''''''''''''ADC BANDO ATTIVITA'
        If Request.QueryString("idBA") <> "" Then
            If lblstato.Text <> "ELIMINATA" And UCase(lblstato.Text) <> "NESSUNO" Then
                Dim strATTIVITA As Integer = -1
                Dim strBANDOATTIVITA As Integer = -1
                Dim strENTEPERSONALE As Integer = -1
                Dim strENTITA As Integer = -1
                Dim strIDENTE As Integer = -1

                If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, Request.QueryString("idBA"), strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                Else
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Response.Redirect("wfrmAnomaliaDati.aspx")
                End If
            End If
        End If
        '''''''''''''''''''''''''''''''ADC BANDO ATTIVITA'
        If IsPostBack = False Then
            'routine carica griglia
            If Not IsNothing(Request.QueryString("id")) Then
                txtidbando.Text = Request.QueryString("id")
                txtidbandoAttivita.Text = Request.QueryString("idBA")
            End If
            CaricaGriglia()
            Cotacheck()
            If Request.QueryString("Verso") <> "Ins" Then
                'ReturnRegioneCompetenzaBando(Request.QueryString("idBA")) = 22 And 
                If lblstato.Text = "Registrata" Then
                    ImgAnteprimaStampa.Visible = True

                End If
                'If ReturnRegioneCompetenzaBando(Request.QueryString("idBA")) <> 22 Then
                '    imgEsporta.Visible = False
                '    imgEsportaRiepilogo.Visible = False
                '    
                '    
                '    Dgtattivita.Columns(18).Visible = False
                '    Dgtattivita.Columns(17).Visible = False
                '    Dgtattivita.Columns(16).Visible = False
                'End If
                'Aggiunto da Alessandra Taballione il 18/07/2005
                'L'immagine d stampa visibile solo se è presentata
                strsql = "select abr.Idregionecompetenza, sba.idstatobandoattività,b.annobreve, isnull(ba.VisibilitaBox,0) as VisibilitaBox, dbo.formatodata(b.DataFineValidità) AS DataFineBando from statibandiattività sba " & _
                " inner join bandiattività ba on ba.idstatobandoattività=sba.idstatobandoattività " & _
                " inner join bando b on b.idbando=ba.idbando " & _
                " inner join AssociaBandoRegioniCompetenze abr on abr.Idbando = b.idbando " & _
                " where idbandoattività=" & txtidbandoAttivita.Text & " and sba.chiuso=0 and sba.defaultstato=0"
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    strappocompetenza = dtrgenerico("idregionecompetenza")
                    'Dim strAnnoBreve As String
                    'strAnnoBreve = CStr(dtrgenerico("AB"))
                    'visualizzo la stampa box 16 solo per i bandi dal 2010 in poi
                    If CStr(dtrgenerico("annobreve")) >= 10 Then
                        If CDate(Session("dataserver")) > CDate(dtrgenerico("DataFineBando")) Then
                            If Session("TipoUtente") <> "E" Then
                                If dtrgenerico("VisibilitaBox") = True Then
                                    If Not dtrgenerico Is Nothing Then
                                        dtrgenerico.Close()
                                        dtrgenerico = Nothing
                                    End If
                                    imgStampaAll.Visible = True

                                    VisualizzaStampePerNazioneBase(CInt(txtidbandoAttivita.Text))
                                End If
                                ImgAnteprimaStampa.Visible = False

                            End If
                        Else
                            imgStampaAll.Visible = True

                            ImgAnteprimaStampa.Visible = False

                            VisualizzaStampePerNazioneBase(CInt(txtidbandoAttivita.Text))

                            If controlladata() = True Then
                                cmdAnnullaPresentazione.Visible = True
                                lblMessaggio.Visible = False

                            Else
                                cmdAnnullaPresentazione.Visible = False
                            End If
                        End If

                        '****** inizio rem Antonello** 01/09/2010****da verificare con jonathan*
                        'If Session("TipoUtente") <> "E" Then
                        '    If dtrgenerico("VisibilitaBox") = True then ---Or dtrgenerico("VisibilitaBox") = Nothing Then
                        '        If Not dtrgenerico Is Nothing Then
                        '            dtrgenerico.Close()
                        '            dtrgenerico = Nothing
                        '        End If
                        '        VisualizzaStampePerNazioneBase(CInt(txtidbandoAttivita.Text))
                        '    End If
                        'End If
                        '****** fine rem Antonello*******

                        'imgStampaSAP.Visible = True
                        'LblStampaSAP.Visible = True
                        'imgStampaSAPEstero.Visible = True
                        'LblStampaSapEstero.Visible = True
                    Else
                        Session("Sap") = False
                        Session("SapEst") = False

                    End If
                    'Modifica il 14/03/2014 da s.c.
                    'UNSC E le Regioni/Province Autonome utilizzano la procedura online x il caricamento dei progetti
                    'If strappocompetenza <> 22 Then
                    '                  ImgAnteprimaStampa.Visible = False
                    '                  LblAnteprimaStampa.Visible = False
                    '                  imgEsporta.Visible = False
                    '                  imgEsportaRiepilogo.Visible = False
                    '                  
                    '                  
                    '                  Dgtattivita.Columns(18).Visible = False
                    '                  Dgtattivita.Columns(17).Visible = False
                    '                  Dgtattivita.Columns(16).Visible = False
                    '              End If
                End If
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                'Aggiunto da Alessandra Taballione il 21/09/2005
                If ControllaStatoChiuso() = True Then
                    cmdPresentaIstanza.Visible = False
                    lblMessaggioPresenta.Visible = False
                    cmdannulla.Visible = False
                    cmdmodifica.Visible = False
                    lblMessaggio.Text = "Attenzione, non è possibile Modificare l'Istanza poichè la Circolare presentazione progetti risulta CHIUSA."
                    bloccaCheck()
                End If
                'controllo se ci sono documenti da registrare 
                If StatoDocumenti(CInt(Session("IdEnte"))) Then
                    cmdPresentaIstanza.Visible = False
                    cmdannulla.Visible = False
                    cmdmodifica.Visible = False
                    ImgAnteprimaStampa.Visible = False

                End If
            End If

            'AGGIUNTA MESSA DA JONATHAN IL 10 marzo 2010
            'BLOCCATE LE ICONE DI STAMPA DEI BOX SIA PER GLI ENTI CHE PER GLI UTENTI UNSC
            'imgStampaSAP.Visible = False
            'imgStampaSAPEstero.Visible = False
            'LblStampaSAP.Visible = False
            'LblStampaSapEstero.Visible = False

            'controllo se si tratta di un utente unsc, così mostro le informazioni riguardo l'ente
            If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
                lblVoceClasseAttribuita.Visible = False
                lblVoceMassimoVolontariPrevisti.Visible = False
                'lblVoceMinimoVolontariPrevisti.Visible = False
                lblVOCEStatoEnte.Visible = False
                lblVoceTotaleVolontariRichiesti.Visible = False
                lblClasseAttribuita.Visible = False
                lblMassimoVolontari.Visible = False
                'lblMinimoVolontari.Visible = False
                lblStatoEnte.Visible = False
                lblTotVolRic.Visible = False
            Else
                'vado a caricare i dati per l'utente UNSC 
                CaricaInfoEnte()
                'Modifica il 14/03/2014 da s.c.
                'UNSC E le Regioni/Province Autonome utilizzano la procedura online x il caricamento dei progetti
                If Request.QueryString("Verso") <> "Ins" Then
                    lblperImgEsxport.Visible = True
                    imgEsporta.Visible = True
                    lblperImgEsxportRiepig.Visible = True
                    imgEsportaRiepilogo.Visible = True
                    
                End If
                'MODIFICATO IL 17/02/2011 da Simona Cordella
                'solo per gli utenti unsc
                GestioneFascicolo(Session("TipoUtente"), txtidbando.Text)
                If ClsUtility.ForzaFascicoloInformaticoProgetti(Session("Utente"), Session("conn")) = True And strappocompetenza = 22 Then
                    'agg da simona cordella il 15/05/2012 - abilito visualizzazione e inserimento protocolli 
                    'mod. il 30/05/2013 - aggiunto controllo regione competenza


                    dtgElencoProt.Visible = True
                    LblNumProt.Visible = True
                    txtNumProt.Visible = True
                    ImgSellProtollo.Visible = True
                    LblDataProt.Visible = True
                    txtDataProt.Visible = True
                    imgSalvaProt.Visible = True
                    CaricaProtocolli(txtidbandoAttivita.Text)
                End If
            End If


            If Request.QueryString("Stampa") = "SI" Then
                Dim JScript As String

                JScript = "<script>" & vbCrLf
                'Modifica il 14/03/2014 da s.c.
                'UNSC E le Regioni/Province Autonome utilizzano la procedura online x il caricamento dei progetti
                JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=16&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf

                'If ReturnRegioneCompetenzaBando(txtidbandoAttivita.Text) = 22 Then
                '    JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=16&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
                'Else 'stampa copertina progetti regionali
                '    JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=37&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
                '    If Not Session("Sap") Is Nothing Then
                '        If Session("Sap") = True Then
                '            JScript &= "myWin = window.open (""WfrmReportistica.aspx?sTipoStampa=32&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
                '        End If
                '    End If
                '    If Not Session("SapEst") Is Nothing Then
                '        If Session("SapEst") = True Then
                '            JScript &= "myWin = window.open (""WfrmReportistica.aspx?sTipoStampa=35&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")"
                '        End If
                '    End If
                'End If
                JScript &= ("</script>")

                Response.Write(JScript)
                RegistraStampaAvvenuta()
            End If
            If Session("TipoUtente") <> "E" Then
                'ripristino solo se non ho la data inizio bando volontari e istanza inammissibile

                If VerificaIstanzaInammissibile(Request.QueryString("idBA")) = True Then
                    If ControlloDataInizioVolontari(Request.QueryString("idBA")) = True Then
                        cmdRipristina.Visible = True
                    Else
                        cmdRipristina.Visible = False
                    End If
                End If
            End If


        End If
    End Sub


    Sub CaricaInfoEnte()
        'Aggiunto da Alessandra Taballione il 18/07/2005
        'L'immagine d stampa visibile solo se è presentata
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'strsql = "select IdClasseAccreditamento, classiaccreditamento.MaxEntitàperanno, classiaccreditamento.MinEntitàperanno, statiente.statoente, "
        'strsql = strsql & "SUM(attività.NumeroPostiVitto) + SUM(attività.NumeroPostiVittoAlloggio) + SUM(attività.NumeroPostiNoVittoNoAlloggio) AS Tot from enti "
        'strsql = strsql & "inner join classiaccreditamento on enti.idclasseaccreditamento=classiaccreditamento.idclasseaccreditamento "
        'strsql = strsql & "inner join statiente on statiente.idstatoente=enti.idstatoente "
        'strsql = strsql & "inner join attività on enti.idente=attività.identepresentante "
        'strsql = strsql & "inner join BandiAttività on BandiAttività.IdBandoAttività=attività.IDBandoAttività "
        'strsql = strsql & "where enti.idente='" & Session("IdEnte") & "' and BandiAttività.idbando='" & txtidbando.Text & "'"

        strsql = "select enti.IdClasseAccreditamento, classiaccreditamento.MaxEntitàperanno, classiaccreditamento.MinEntitàperanno, statienti.statoente, "
        strsql = strsql & "IsNull(SUM(attività.NumeroPostiVitto) + SUM(attività.NumeroPostiVittoAlloggio) + SUM(attività.NumeroPostiNoVittoNoAlloggio),0) AS Tot from enti "
        strsql = strsql & "inner join classiaccreditamento on enti.idclasseaccreditamento=classiaccreditamento.idclasseaccreditamento "
        strsql = strsql & "inner join statienti on statienti.idstatoente=enti.idstatoente "
        strsql = strsql & "inner join attività on enti.idente=attività.identepresentante "
        strsql = strsql & "inner join BandiAttività on BandiAttività.IdBandoAttività=attività.IDBandoAttività "
        strsql = strsql & "inner join statiattività on attività.idstatoattività=statiattività.idstatoattività "
        strsql = strsql & "where enti.idente='" & Session("IdEnte") & "' and BandiAttività.idbando='" & txtidbando.Text & "' "
        strsql = strsql & "group by enti.IDClasseAccreditamento, classiaccreditamento.MaxEntitàPerAnno, "
        strsql = strsql & "classiaccreditamento.MinEntitàPerAnno, statienti.StatoEnte "

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            lblClasseAttribuita.Text = dtrgenerico("IdClasseAccreditamento")
            lblMassimoVolontari.Text = dtrgenerico("MaxEntitàperanno")
            'lblMinimoVolontari.Text = dtrgenerico("MinEntitàperanno")
            lblTotVolRic.Visible = True
            lblTotVolRic.Text = dtrgenerico("Tot")
            lblStatoEnte.Text = dtrgenerico("statoente")

        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Function ControllaStatoChiuso() As Boolean
        strsql = "select bando.idstatobando " & _
        " from bando " & _
        " inner join statibando  on bando.idstatobando=statibando.idstatobando" & _
        " where idbando=" & txtidbando.Text & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            If dtrgenerico("idstatobando") = 3 Then
                ControllaStatoChiuso = True
            Else
                ControllaStatoChiuso = False
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Function

    Private Sub PreparaInserimento()
        Dim mydataset As DataSet
        Dim strsql As String
        cmdannulla.Visible = False
        cmdmodifica.Visible = False
        cmdPresentaIstanza.Visible = False
        lblMessaggioPresenta.Visible = False
        lblstato.Text = "NESSUNO"
        'seleziono bandi non collegati con attività per ente loggato
        mydataset = ClsServer.DataSetGenerico("select a.idbando,a.bando,RegioniCompetenze.idregionecompetenza from bando a INNER JOIN AssociaBandoRegioniCompetenze ON a.IDBando = AssociaBandoRegioniCompetenze.IdBando INNER JOIN " & _
        " RegioniCompetenze ON AssociaBandoRegioniCompetenze.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
        " INNER JOIN STATIBANDO b on a.idstatobando=b.idstatobando" & _
        " where B.INVALUTAZIONE = 1" & _
        " AND a.idbando <> all (SELECT IDBANDO FROM BANDIATTIVITà a inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività WHERE IDENTE =" & Session("idente") & " and b.cancellata<>1)" & _
        " order by 1", Session("conn"))
        DgdBando.DataSource = mydataset
        DgdBando.DataBind() 'valorizzo griglia bando
        'se non ho record non viusalizzo griglia e nascondo pulsante inserimento
        If DgdBando.Items.Count = 0 Then
            lblMessaggio.Text = ""
            cmdInserisci.Visible = False
            DgdBando.Visible = False
            lblbando.Visible = True
        End If
        'in inserimento
        'Modificato da Alessandra Taballione il 31.01.2004
        'Conrollo sull'Inserimento di una nuova Istanza.
        'Nel Progetto non possono essere presenti sedi Assegnazione o sedi Attuazione o olp
        'diversi da quelli Inseriti a quelli richiesti
        ' (numeropostinovittonoalloggio + numeropostivittoalloggio + numeropostivitto) as numerovolontari,  
        strsql = "select att.idattività, titolo," & _
        " case isnull((select sum(NumeroPostiNoVittoNoAlloggio)+ sum(NumeroPostiVittoAlloggio)+  sum(NumeroPostiVitto) " & _
        " from attivitàentisediattuazione  where idattività =att.idattività),-1) when -1 then 0 else " & _
        " (select sum(NumeroPostiNoVittoNoAlloggio)+ sum(NumeroPostiVittoAlloggio)+  sum(NumeroPostiVitto) " & _
        " from attivitàentisediattuazione  where idattività =att.idattività) end as numerovolontari,att.idbandoattività as bandiattività ," & _
        " (select count(idattivitàentesedeattuazione) from attivitàentisediattuazione a inner join entisediattuazioni e " & _
        " on e.identesedeattuazione=a.identesedeattuazione inner join statientisedi s " & _
        " on e.idstatoentesede=s.idstatoentesede where  idattività=att.idattività and " & _
        " (s.attiva=1 or s.daaccreditare=1 or s.defaultstato=1))as idattsedeatt," & _
        " (select count(idattivitàsedeassegnazione) from attivitàsediassegnazione a " & _
        " inner join entisedi e on e.identesede=a.identesede inner join statientisedi s " & _
        " on e.idstatoentesede=s.idstatoentesede where idattività=att.idattività and " & _
        " (s.attiva=1 or s.daaccreditare=1 or s.defaultstato=1))as idattsedeass,"
        strsql = strsql & "(select  " & _
        " sum(ceiling(convert(decimal(10,2),(isnull(attivitàentisediattuazione.NumeroPostiNoVittoNoAlloggio,0) + " & _
        " isnull(attivitàentisediattuazione.NumeroPostiVittoAlloggio, 0)" & _
        " + isnull(attivitàentisediattuazione.NumeroPostiVitto,0)))/ " & _
        " isnull(iperambitiattività.MaxVolontariPerOLP,0)))" & _
        " from attività " & _
        " left join attivitàentisediattuazione on attivitàentisediattuazione.idattività = attività.idattività " & _
        " left join ambitiattività  on  (ambitiattività.idambitoattività=attività.idambitoattività) " & _
        " left join  macroambitiattività  on  (macroambitiattività.idmacroambitoattività=ambitiattività.idmacroambitoattività) " & _
        " left join iperambitiattività on (macroambitiattività.idiperambitoattività=iperambitiattività.idiperambitiattività)" & _
        " where attività.idattività =att.idAttività) as NOlpRic,"
        strsql = strsql & " (select count(*)  " & _
        " from associaEntePersonaleRuoliAttivitàEntiSediAttuazione ass  " & _
        " inner join attivitàentisediattuazione b on (ass.idattivitàentesedeattuazione=b.idattivitàentesedeattuazione) " & _
        " inner join entepersonaleRuoli epr on epr.identepersonaleruolo= ass.identepersonaleruolo" & _
        " where epr.datafinevalidità is null and  b.idattività=att.idattività and epr.idruolo=1) as nolpins, " & _
        " (Select  count(entepersonale.identepersonale)  " & _
        " from entepersonale " & _
        " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)  " & _
        " inner join associaEntepersonaleRuoliattivitàentisediattuazione a   " & _
        " on  (a.idEntepersonaleRuolo=entepersonaleruoli.identepersonaleRuolo)  " & _
        " inner join attivitàentisediattuazione  " & _
        " on  (attivitàentisediattuazione.idAttivitàEntesedeAttuazione=a.idAttivitàEntesedeAttuazione)    " & _
        " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)  " & _
        " where entepersonaleruoli.datafinevalidità is null and (entepersonale.idente = " & Session("idente") & " And entepersonale.datafinevalidità Is null) " & _
        " and entepersonaleruoli.idruolo=6 and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0)  " & _
        " and attivitàentisediattuazione.idattività=att.idattività ) as nRleaIns , " & _
        " (Select  count(entepersonale.identepersonale)  " & _
        " from entepersonale " & _
        " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)  " & _
        " inner join associaEntepersonaleRuoliattivitàentisediattuazione a   " & _
        " on  (a.idEntepersonaleRuolo=entepersonaleruoli.identepersonaleRuolo)  " & _
        " inner join attivitàentisediattuazione  " & _
        " on  (attivitàentisediattuazione.idAttivitàEntesedeAttuazione=a.idAttivitàEntesedeAttuazione)    " & _
        " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)  " & _
        " where entepersonaleruoli.datafinevalidità is null and (entepersonale.idente = " & Session("idente") & " And entepersonale.datafinevalidità Is null) " & _
        " and entepersonaleruoli.idruolo=5 and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0)  " & _
        " and attivitàentisediattuazione.idattività=att.idattività ) as nTutorIns,RegioniCompetenze.descrizione as RegComp   " & _
        " from attività att" & _
        " inner join enti b on att.identepresentante=b.idente" & _
        " inner join statiattività c on att.idstatoattività=c.idstatoattività" & _
        " INNER JOIN RegioniCompetenze ON att.IDRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza" & _
        " where b.idente=" & Session("idente") & " and c.Defaultstato=1"
        mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        'mydataset = ClsServer.DataSetGenerico("select a.idattività, titolo," & _
        '" (numeropostinovittonoalloggio + numeropostivittoalloggio + " & _
        '" numeropostivitto) as numerovolontari,'0' as bandiattività" & _
        '" from attività a" & _
        '" inner join enti b on a.identepresentante=b.idente" & _
        '" inner join statiattività c on a.idstatoattività=c.idstatoattività" & _
        '" where b.idente=" & Session("idente") & " and c.Defaultstato=1", Session("conn"))
        Dgtattivita.DataSource = mydataset
        Dgtattivita.DataBind() 'valorizzo griglia attività

        'se non ho record non viusalizzo griglia e nascondo pulsante inserimento
        If Dgtattivita.Items.Count = 0 Then
            lblMessaggio.Text = ""
            cmdInserisci.Visible = False
            Dgtattivita.Visible = False
            lblprogetto.Visible = True
        End If
    End Sub

    Private Sub CaricaGriglia()
        '***Generata da Gianluigi Paesani in data:13/07/04
        '***Modificata da Gianluigi Paesani in data:20/09/04
        '***Modificata da Guido testa in data:12/07/2006 (Filtro per competenza regionale)
        '***Inizializzo  la web form a seconda dell'inserimento o della
        '***della modifica(presentazione,modifica,accreditamento,annullamento accreditamento)
        '***Modificata da Simona Cordella il 25/10/2007
        '***Tolgo subquery nella condizione di IdTipoProgetto
        '**** modificata da Simona Cordella per presentazione nuovi progetti 2010
        Dim i As Integer
        Dim mydataset As DataSet
        Dim strsql As String
        Dim bolCompetenza As Boolean
        bolCompetenza = False
        Dim IdbandoAtt As String
        Dim dtsBando As SqlClient.SqlDataReader
        Dim bolCodAtt As Boolean
        Dim IDTipoProgetto As String
        bolCodAtt = False
        strsql = ""
        IdbandoAtt = "" & Request.Params("idBA")


        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Call VerificaCompetenze()
        End If

        If IdbandoAtt <> "" Then
            strsql = "Select * From bandiattività Where idbandoattività=" & IdbandoAtt
            dtsBando = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtsBando.HasRows Then
                bolCodAtt = True
            End If
            dtsBando.Close()
            dtsBando = Nothing
        End If

        If Request.QueryString("Verso") = "Ins" Or bolCodAtt = False Then
            Dim strappocompetenza As String
            If DgdBando.Items.Count <> 0 Then
                strappocompetenza = DgdBando.SelectedItem.Cells(3).Text
            End If

            imgEsporta.Visible = False
            imgEsportaRiepilogo.Visible = False
            lblperImgEsxport.Visible = False
            lblperImgEsxportRiepig.Visible = False


            cmdannulla.Visible = False
            cmdmodifica.Visible = False
            cmdPresentaIstanza.Visible = False
            lblMessaggioPresenta.Visible = False
            lblstato.Text = "NESSUNO"
            'carico sempre il bando di competenza nazionale
            'AND (bando.Riferimento = '1')" & _
            strsql = "SELECT distinct bando.IDBando, bando.Bando, AssociaBandoRegioniCompetenze.IdRegioneCompetenza " & _
                     "FROM bando INNER JOIN AssociaBandoRegioniCompetenze ON bando.IDBando = AssociaBandoRegioniCompetenze.IdBando INNER JOIN " & _
                     " RegioniCompetenze ON AssociaBandoRegioniCompetenze.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                     " INNER JOIN AssociaBandoTipiProgetto ON AssociaBandoTipiProgetto.IdBando =bando.IDBando " & _
                     " WHERE bando.programmi = 0 and (AssociaBandoRegioniCompetenze.IdRegioneCompetenza = 22) " & _
                     " AND bando.idbando <> all (SELECT IDBANDO FROM BANDIATTIVITà a inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività WHERE IDENTE =" & Session("idente") & " and b.cancellata<>1) " & _
                     " AND bando.DataInizioValidità <= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)" & _
                     " AND bando.DataFineValidità >= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)"

            'Modiifcata da Luigi Leucci il 12/10/2018
            '   strsql &= " AND (AssociaBandoTipiProgetto.IdTipoProgetto=3 OR AssociaBandoTipiProgetto.IdTipoProgetto=2 OR AssociaBandoTipiProgetto.IdTipoProgetto=5) "
            strsql &= " AND (AssociaBandoTipiProgetto.IdTipoProgetto IN (2, 3, 5, 8, 9, 10)) "

            'seleziono bandi non collegati con attività per ente loggato
            strsql = strsql & " UNION select distinct a.idbando,a.bando,RegioniCompetenze.IdRegioneCompetenza  from bando a" & _
            " INNER JOIN AssociabandoTipiProgetto ab on a.idbando=ab.idbando " & _
            " INNER JOIN TipiProgetto ON ab.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
            " INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
            " INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "

            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If

            strsql = strsql & " INNER JOIN STATIBANDO b on a.idstatobando=b.idstatobando"
            'AGGIUNTA JOIN PER RegioniCompetenze E AssociaBandoRegioniCompetenze
            strsql = strsql & " INNER JOIN AssociaBandoRegioniCompetenze ON a.IDBando = AssociaBandoRegioniCompetenze.IdBando" & _
            " INNER JOIN RegioniCompetenze ON AssociaBandoRegioniCompetenze.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
            '---------------------------------------------------------------------------------------------------------------------------
            strsql = strsql & " WHERE B.INVALUTAZIONE = 1" & _
            " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'" & _
            " AND a.DataInizioValidità <= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)" & _
            " AND a.DataFineValidità >= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)" & _
            " AND a.idbando <> all (SELECT IDBANDO FROM BANDIATTIVITà a inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività WHERE IDENTE =" & Session("idente") & " and b.cancellata<>1)" & _
            " and AssociaUtenteGruppo.Username='" & Session("Utente") & "'  "
            'AGGIUNTO FILTRO PER IdRegioneCompetenza SU VISTA CHE RACCOGLIE TUTTI GLI ENTI OPERATIVI E ACCREDITATI
            strsql = strsql & " AND  (RegioniCompetenze.IdRegioneCompetenza IN " & _
            " (SELECT idregionecompetenza FROM VW_ELENCO_ENTI_ACCREDITATI WHERE IDENTE = " & Session("idente") & "))"
            '---------------------------------------------------------------------------------------------------------------------------
            If Session("TipoUtente") = "E" Then
                strsql = strsql & " and isnull(a.enteabilitato,1) = 1 and a.programmi = 0 "
            End If
            strsql = strsql & " order by 2"
            mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
            DgdBando.DataSource = mydataset
            DgdBando.DataBind() 'valorizzo griglia bando

            'se non ho record non viusalizzo griglia e nascondo pulsante inserimento
            If DgdBando.Items.Count = 0 Then
                lblMessaggio.Text = ""
                cmdInserisci.Visible = False
                DgdBando.Visible = False
                lblbando.Visible = True
            End If

            If Request.QueryString("Verso") <> "Ins" And lblstato.Text <> "NESSUNO" Then
                imgEsporta.Visible = True
                imgEsportaRiepilogo.Visible = True
                lblperImgEsxport.Visible = True
                lblperImgEsxportRiepig.Visible = True

            End If
            Dgtattivita.Columns(18).Visible = True
            Dgtattivita.Columns(17).Visible = True
            Dgtattivita.Columns(16).Visible = True

            'If strappocompetenza = 22 Then
            '             If Request.QueryString("Verso") <> "Ins" Then
            '                 imgEsporta.Visible = True
            '                 imgEsportaRiepilogo.Visible = True
            '                 
            '                 
            '             End If
            '             Dgtattivita.Columns(18).Visible = True
            '             Dgtattivita.Columns(17).Visible = True
            '             Dgtattivita.Columns(16).Visible = True
            '         Else
            '             imgEsporta.Visible = False
            '             imgEsportaRiepilogo.Visible = False
            '             
            '             
            '             Dgtattivita.Columns(18).Visible = False
            '             Dgtattivita.Columns(17).Visible = False
            '             Dgtattivita.Columns(16).Visible = False
            '             ImgAnteprimaStampa.Visible = False
            '             LblAnteprimaStampa.Visible = False
            '         End If

            'in inserimento
            'Modificato da Alessandra Taballione il 31.01.2004
            'Conrollo sull'Inserimento di una nuova Istanza.
            'Nel Progetto non possono essere presenti sedi Assegnazione o sedi Attuazione o olp
            'diversi da quelli Inseriti a quelli richiesti
            ' (numeropostinovittonoalloggio + numeropostivittoalloggio + numeropostivitto) as numerovolontari,  
            'Modificato da Guido Testa il 13/07/2004
            'Aggiunta Join e relativo filtro con la RegioneCompetenza


            '*******************
            'Modificato da Simona Cordella il 25/10/2007
            'Aggiungo query per ricavare l'IdTipoProgetto del bando
            If Not DgdBando.SelectedItem Is Nothing Then
                Dim dtr As Data.SqlClient.SqlDataReader

                dtr = ClsServer.CreaDatareader("select abp.idtipoprogetto " & _
                            " from bando " & _
                            " inner join AssociaBandotipiProgetto abp on abp.idbando=bando.idbando " & _
                            " where bando.idbando=" & DgdBando.SelectedItem.Cells(1).Text & "", Session("conn"))
                If dtr.HasRows = True Then
                    IDTipoProgetto = ""
                    While dtr.Read
                        If IDTipoProgetto = "" Then
                            IDTipoProgetto = dtr("idtipoprogetto")
                        Else
                            IDTipoProgetto = IDTipoProgetto & "," & dtr("idtipoprogetto")
                        End If
                    End While
                End If
                dtr.Close()
                dtr = Nothing
                'Agg. da s.c. il 16/11/2009
                'Controllo se il bando selezionato prevede i TUTOR
                Dim blnTutor As Boolean = VerificaTutor(DgdBando.SelectedItem.Cells(1).Text)
                If blnTutor = False Then 'rendo invisibile la colonna dei tutor
                    Dgtattivita.Columns(10).Visible = False
                Else 'rendo visibile colonna
                    Dgtattivita.Columns(10).Visible = True
                End If
                'rimossa visibilità RLEA
                Dgtattivita.Columns(9).Visible = False
            End If

            '***************
            If Not DgdBando.SelectedItem Is Nothing Then

                Dim dtrLocal As SqlClient.SqlDataReader

                '***************************************************************************
                'MODIFICA QUERY TIME OUT 29/10/2007
                dtrLocal = ClsServer.EseguiStoreReaderInserimentoIstanza("SP_ISTANZA_ELENCO_DISPONIBILI", Session("idente"), DgdBando.SelectedItem.Cells(3).Text, DgdBando.SelectedItem.Cells(1).Text, Session("conn"))
                '***************************************************************************
                Dgtattivita.Visible = True
                lblprogetto.Visible = False
                lblprogetto.Text = ""
                'mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
                Dgtattivita.DataSource = dtrLocal
                Dgtattivita.DataBind() 'valorizzo griglia attività
                dtrLocal.Close()
                dtrLocal = Nothing
            Else
                lblprogetto.Visible = True
                lblprogetto.Text = "Selezionare una circolare presentazione per visualizzare i progetti."
                Dgtattivita.Visible = False
            End If
            'se non ho record non viusalizzo griglia e nascondo pulsante inserimento
            If Dgtattivita.Items.Count = 0 Then
                lblMessaggio.Text = ""
                cmdInserisci.Visible = False
                Dgtattivita.Visible = False
                lblprogetto.Visible = True
                chkSelDesel.Visible = False
                chkSelDesel2.Visible = False
                imgEsporta.Visible = False
                imgEsportaRiepilogo.Visible = False
                lblperImgEsxport.Visible = False
                lblperImgEsxportRiepig.Visible = False


                'lblseltutto.Visible = False
                'ImgSelezionaTutto.Visible = False
                'lblseltutto2.Visible = False
                'ImgSelezionaTutto2.Visible = False
                'lblseltutto.Visible = False
                'ImgSelezionaTutto.Visible = False
                'lblseltutto2.Visible = False
                'ImgSelezionaTutto2.Visible = False
                If Not DgdBando.SelectedItem Is Nothing Then
                    lblprogetto.Visible = True
                    lblprogetto.Text = "Nessun progetto disponibile."
                Else
                    lblprogetto.Visible = True
                    lblprogetto.Text = "Selezionare una circolare presentazione per visualizzare i progetti."
                End If
            Else
                chkSelDesel.Visible = True
                chkSelDesel2.Visible = True
                'lblseltutto.Visible = True
                'ImgSelezionaTutto.Visible = True
                'lblseltutto2.Visible = True
                'ImgSelezionaTutto2.Visible = True
                'lblseltutto.Visible = True
                cmdInserisci.Visible = True
            End If
            '*********************entro qui, per tutto ciò che riguarda la modifica***************
        Else 'Request.QueryString("Verso") = "Mod"
            Dim dtr As Data.SqlClient.SqlDataReader
            ImgControllaProvincie.Visible = True
            imgCheckOLP.Visible = False
            lblstato.Text = Request.QueryString("Stato")
            cmdInserisci.Visible = False
            'seleziono bando preso dalla maschera di ricerca
            'mydataset = ClsServer.DataSetGenerico("select idbando,bando from bando" & _
            '" where idbando=" & Request.QueryString("id") & "", Session("conn"))

            mydataset = ClsServer.DataSetGenerico("SELECT bando.IDBando, bando.Bando, AssociaBandoRegioniCompetenze.IdRegioneCompetenza " & _
                        " FROM bando INNER JOIN AssociaBandoRegioniCompetenze ON bando.IDBando = AssociaBandoRegioniCompetenze.IdBando " & _
                        " WHERE bando.idbando=" & Request.QueryString("id") & "", Session("conn"))

            DgdBando.DataSource = mydataset
            DgdBando.DataBind() 'valorizzo griglia bando
            'se non ho record non viusalizzo griglia e nascondo pulsante inserimento
            If DgdBando.Items.Count = 0 Then
                'lblMessaggio.Text = ""
                cmdInserisci.Visible = False
                cmdannulla.Visible = False
                cmdmodifica.Visible = False
                DgdBando.Visible = False
                lblbando.Visible = True
                cmdAnnullaPresentazione.Visible = False
            End If
            'controllo presenza di *****progetti presentati***** quindi istanza in stato di presentato
            dtr = ClsServer.CreaDatareader("select idbandoattività" & _
            " from bandiattività a" & _
            " inner join statibandiattività b" & _
            " on a.idstatobandoattività=b.idstatobandoattività" & _
            " where idbandoattività=" & Request.QueryString("idBA") & "" & _
            " and b.DaValutare=1", Session("conn"))

            If dtr.HasRows = False Then 'se non ci sono record presentati vado in modifica
                'se non vi sono progetti presentati vado con la normale gestione di modifica
                dtr.Close()
                dtr = Nothing
                '*************verifico sel'istanza è già accreditata
                dtr = ClsServer.CreaDatareader("select idbandoattività" & _
                    " from bandiattività a" & _
                    " inner join statibandiattività b" & _
                    " on a.idstatobandoattività=b.idstatobandoattività" & _
                    " where idbandoattività=" & Request.QueryString("idBA") & "" & _
                    " and (b.attivo=1 or b.inammissibile=1) ", Session("conn"))
                If dtr.HasRows = True Then
                    dtr.Close()
                    dtr = Nothing
                    'Modificato da Alessandra Taballione il 31.01.2004
                    'Conrollo sulla Modifica di una nuova Istanza.
                    'Nel Progetto non possono essere presenti sedi Assegnazione o sedi Attuazione o olp
                    'diversi da quelli Inseriti a quelli richiesti

                    Dim dtrLocalValutazione As SqlClient.SqlDataReader

                    If Not dtrLocalValutazione Is Nothing Then
                        dtrLocalValutazione.Close()
                        dtrLocalValutazione = Nothing
                    End If

                    dtrLocalValutazione = ClsServer.EseguiStoreReaderInserimentoIstanza("SP_ISTANZA_ELENCO_DISPONIBILI_VALUTAZIONE", Session("idente"), DgdBando.Items(0).Cells(3).Text, Request.QueryString("id"), Session("conn"))

                    'mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
                    Dgtattivita.DataSource = dtrLocalValutazione
                    Dgtattivita.DataBind()
                    'Dim intCountDataReader
                    If Not dtrLocalValutazione Is Nothing Then
                        dtrLocalValutazione.Close()
                        dtrLocalValutazione = Nothing
                    End If
                    'Controllo se il bando selezionato prevede i TUTOR
                    Dim blnVerTutor As Boolean = VerificaTutor(Request.QueryString("id"))
                    If blnVerTutor = False Then 'rendo invisibile la colonna dei tutor
                        Dgtattivita.Columns(10).Visible = False
                    Else 'rendo visibile colonna
                        Dgtattivita.Columns(10).Visible = True
                    End If
                    'rimossa visibilità RLEA
                    Dgtattivita.Columns(9).Visible = False
                    'ciclo oggetti checkbox della griglia per valorizzarli scorrendo il dataset
                    'il processo è in fase di accreditamento o annullamento accreditamento
                    If Dgtattivita.Items.Count <> 0 Then
                        'For intj As Int32 = 0 To mydataset.Tables(0).Rows.Count - 1
                        Dim item As DataGridItem
                        For Each item In Dgtattivita.Items
                            If Dgtattivita.Items(item.ItemIndex).Cells(4).Text <> 0 Then
                                'If mydataset.Tables(0).Rows(intj).Item(3) <> 0 Then
                                'Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(intj).FindControl("chk")
                                Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(item.ItemIndex).FindControl("chk")
                                chkoggetto.Checked = True
                                chkoggetto.Enabled = False
                            Else
                                'Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(intj).FindControl("chk")
                                Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(item.ItemIndex).FindControl("chk")
                                chkoggetto.Checked = False
                                chkoggetto.Enabled = False
                            End If
                        Next
                        'setto input maschera
                        'lblMessaggio.Text = "L'istanza risulta essere proposta è non può essere modificata"
                        cmdInserisci.Visible = False
                        cmdannulla.Visible = False
                        cmdmodifica.Visible = False
                        cmdPresentaIstanza.Visible = False
                        lblMessaggioPresenta.Visible = False
                        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                            cmdaccredita.Visible = True
                            cmddissaccredita.Visible = True
                            Call VerificaCompetenze()
                        Else
                            lblMessaggio.Text = "L'istanza risulta accreditata"
                        End If
                        dtr = ClsServer.CreaDatareader("select idgraduatoriaprogetto" & _
                           " from graduatorieprogetti" & _
                           " where idbando=" & Request.QueryString("id") & " and statograduatoria=1", Session("conn"))
                        If dtr.HasRows = True Then
                            cmdaccredita.Visible = False
                            cmddissaccredita.Visible = False
                            lblMessaggio.Text = "Attenzione, l'Istanza risulta associata ad una Graduatoria Pubblicata, pertanto non può essere modificata"
                        End If
                        dtr.Close()
                        dtr = Nothing

                        Exit Sub
                    End If
                Else
                    dtr.Close()
                    dtr = Nothing
                    cmdInserisci.Visible = False
                    cmdannulla.Visible = True
                    cmdmodifica.Visible = True
                    Dgtattivita.Visible = False
                    lblprogetto.Visible = True
                    cmddissaccredita.Visible = False
                    cmdaccredita.Visible = False
                    cmdAnnullaPresentazione.Visible = False
                    imgEsporta.Visible = True
                    lblperImgEsxport.Visible = True
                    hlDw.Visible = False
                    imgEsportaRiepilogo.Visible = True
                    lblperImgEsxportRiepig.Visible = True
                    hlDwRip.Visible = False


                End If
                '*************fine verifica

                'Modificato da Simona Cordella il 25/10/2007
                'Aggiungo query per ricavare l'IdTipoProgetto del bando
                dtr = ClsServer.CreaDatareader("select abp.idtipoprogetto " & _
                              " from bando " & _
                              " inner join AssociaBandotipiProgetto abp on abp.idbando=bando.idbando " & _
                              " where bando.idbando=" & Request.QueryString("id") & "", Session("conn"))
                If dtr.HasRows = True Then
                    IDTipoProgetto = ""
                    While dtr.Read
                        If IDTipoProgetto = "" Then
                            IDTipoProgetto = dtr("idtipoprogetto")
                        Else
                            IDTipoProgetto = IDTipoProgetto & "," & dtr("idtipoprogetto")
                        End If
                    End While
                    dtr.Close()
                    dtr = Nothing
                End If


                'con questa query seleziono i progetti istanziati per primi e nella 2 parte della
                'union  i progetti legati al bando ma non ancora istanziati
                ''' and att.idtipoprogetto in ( select abp.idtipoprogetto  from bando  inner join AssociaBandotipiProgetto abp on abp.idbando=bando.idbando  where bando.idbando=" & Request.QueryString("id") & ") " & _


                Dim dtrLocal2 As SqlClient.SqlDataReader

                If Not dtrLocal2 Is Nothing Then
                    dtrLocal2.Close()
                    dtrLocal2 = Nothing
                End If

                dtrLocal2 = ClsServer.EseguiStoreReaderInserimentoIstanza("SP_ISTANZA_ELENCO_DISPONIBILI_MODIFICA", Session("idente"), DgdBando.Items(0).Cells(3).Text, Request.QueryString("id"), Session("conn"))

                'mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
                Dgtattivita.DataSource = dtrLocal2
                Dgtattivita.DataBind()

                Dim intCountDataReader

                If Not dtrLocal2 Is Nothing Then
                    dtrLocal2.Close()
                    dtrLocal2 = Nothing
                End If
                If Dgtattivita.Items.Count > 0 Then
                    Dgtattivita.Visible = True
                    lblprogetto.Text = ""
                End If
                'Controllo se il bando selezionato prevede i TUTOR
                Dim blnTutor As Boolean = VerificaTutor(Request.QueryString("id"))
                If blnTutor = False Then 'rendo invisibile la colonna dei tutor
                    Dgtattivita.Columns(10).Visible = False
                Else 'rendo visibile colonna
                    Dgtattivita.Columns(10).Visible = True
                End If
                'rimossa visibilità RLEA
                Dgtattivita.Columns(9).Visible = False
                ' If controlladata() = True Or (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then  'TOLTO IL 24/10/2016
                If controlladata() = True Then
                    cmdPresentaIstanza.Visible = True
                    lblMessaggioPresenta.Visible = True
                Else
                    cmdPresentaIstanza.Visible = False
                    lblMessaggioPresenta.Visible = False
                    cmdannulla.Visible = False
                    cmdmodifica.Visible = False
                    lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè la data attuale non è compresa tra quella di inizio e fine Circolare presentazione progetti."
                End If
                'opero query per visualizzazione tasto presentazione istanza 
                'Modificato da Alessandra Taballione il 26/07/2005
                'solo gli enti chiusi o sospesi non possono effettuare la presentazione.
                '*********************************************
                'modifica di danatan spagnani il 29.09.2006
                'solo enti attivi o in adeguamento possono presentare progetti
                dtr = ClsServer.CreaDatareader("select a.idstatoente,isnull(dataaccreditamento,'01/01/2004') as DataAccreditamento, isnull(dataultimoadeguamento,'01/01/2004') as DataAdeguamento from enti a" & _
                " inner join statienti b on a.idstatoente=b.idstatoente" & _
                " where a.idente=" & Session("idente") & " and  b.idstatoente in (3,9)", Session("conn"))
                If dtr.HasRows = False Then
                    cmdPresentaIstanza.Visible = False
                    lblMessaggioPresenta.Visible = False
                    lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè l'Ente associato non risulta accreditato"
                Else
                    'SANDOKAN
                    dtr.Read()
                    If dtr("DataAccreditamento") < "22/06/2009" And dtr("DataAdeguamento") < "22/06/2009" Then
                        cmdPresentaIstanza.Visible = False
                        lblMessaggioPresenta.Visible = False
                        lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè l'Ente non risulta adeguato."
                    End If
                End If

                dtr.Close()
                dtr = Nothing
                'ciclo oggetti checkbox della griglia per valorizzarli scorrendo il dataset
                'il processo è ancora in *********MODIFICA*********
                If Dgtattivita.Items.Count <> 0 Then
                    '                    For intj As Int32 = 0 To mydataset.Tables(0).Rows.Count - 1
                    Dim item As DataGridItem
                    For Each item In Dgtattivita.Items
                        'If mydataset.Tables(0).Rows(intj).Item(3) <> 0 Then
                        If Dgtattivita.Items(item.ItemIndex).Cells(4).Text <> 0 Then
                            Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(item.ItemIndex).FindControl("chk")
                            'Aggiunto da Alessandra Taballione il 01.02.2005
                            'Controllo se sono state effettuate delle modifiche al progetto 
                            'che compromettono l'associazione al bando

                            'controllo la competenza del progetto
                            'se nazionale il numero minimo dev'essere di 4
                            'se regionale il numero minimo dev'essere di 2
                            'aggiunto il 29/09/2006
                            'controllo aggiunto da Jona Strauss Jeans

                            If Not dtrgenerico Is Nothing Then
                                dtrgenerico.Close()
                                dtrgenerico = Nothing
                            End If

                            Dim strNVOL As String = Dgtattivita.Items(item.ItemIndex).Cells(2).Text
                            'If CInt(IIf(strNVOL <> "&nbsp;", strNVOL, "0")) >= 4 Then
                            'CInt(IIf(strNVOL <> "&nbsp;", strNVOL, "0")) >= IIf(CInt(strIdTipoProgetto) = 3, 1, 4) And 

                            If CInt(Dgtattivita.Items(item.ItemIndex).Cells(12).Text) = 1 _
                                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(13).Text) = 1 _
                                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(14).Text) = 1 _
                                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(15).Text) = 1 _
                                And Dgtattivita.Items(item.ItemIndex).Cells(6).Text <> 0 _
                                And Dgtattivita.Items(item.ItemIndex).Cells(5).Text <> 0 _
                                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(20).Text) = 1 _
                                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(21).Text) = 1 _
                                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(22).Text) = 1 _
                                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(23).Text) = 1 _
                                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(24).Text) = 1 _
                                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(25).Text) = 1 _
                                And (CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) Then
                                chkoggetto.Checked = True
                            Else
                                'Tolgo l'associazione dall'attività
                                ' e storicizzo
                                Dim cmdinsert As Data.SqlClient.SqlCommand
                                'modifico prima la cronologia dell'attività
                                cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaAttività" & _
                                " (idattività,idstatoattività,datacronologia,idTipoCronologia," & _
                                " usernameaccreditatore)" & _
                                " select " & CInt(Dgtattivita.Items(item.ItemIndex).Cells(0).Text) & "," & _
                                " idstatoattività,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'" & _
                                " from attività where idattività=" & CInt(Dgtattivita.Items(item.ItemIndex).Cells(0).Text) & "", Session("conn"))
                                cmdinsert.ExecuteNonQuery()
                                cmdinsert.Dispose()
                                'modifico tabella attività progetti
                                cmdinsert = New Data.SqlClient.SqlCommand("update attività" & _
                                "  set idbandoattività=null," & _
                                " idstatoattività=(select idstatoattività from" & _
                                "  statiattività where defaultstato=1)" & _
                                " where idattività=" & CInt(Dgtattivita.Items(item.ItemIndex).Cells(0).Text) & "", Session("conn"))
                                cmdinsert.ExecuteNonQuery()
                                cmdinsert.Dispose() 'modifico tabella dell'attività
                                lblMessaggio.Visible = True

                                lblMessaggio.Text = "Alcuni progetti precedentemente selezionati non possono essere associati alla Circolare presentazione progetti perchè attualmente risultano incompleti."
                                'Aggiunto da Alessandra Taballione il 25/02/2005
                                ' se è l'ultimo progetto elimino il bando 
                                strsql = "select * from attività where idbandoattività=" & Request.QueryString("idBA") & " and identepresentante=" & Session("Idente") & ""
                                dtr = ClsServer.CreaDatareader(strsql, Session("conn"))
                                If dtr.HasRows = False Then
                                    'elimino record in bandiattività
                                    dtr.Close()
                                    dtr = Nothing
                                    cmdinsert = New Data.SqlClient.SqlCommand("delete from bandiattività" & _
                                    " where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
                                    cmdinsert.ExecuteNonQuery()
                                    cmdinsert.Dispose()
                                    'cmdPresentaIstanza.Visible = False
                                    'lblMessaggio.Text = "Tutti i progetti precedentemente selezionati non possono essere associati al bando perchè attualmente risultano inclompleti. Provvedere a ricreare una nuova Istanza."
                                    Response.Write("<script language=""javascript"">" & vbCrLf)
                                    Response.Write("<!--" & vbCrLf)
                                    Response.Write("alert('Tutti i progetti precedentemente selezionati non possono essere associati alla Circolare presentazione progetti perchè attualmente risultano incompleti. ")
                                    Response.Write("Provvedere a ricreare una nuova Istanza." & "');" & vbCrLf)
                                    Response.Write("//-->" & vbCrLf)
                                    Response.Write("</script>" & vbCrLf)
                                    'Response.Redirect("WfrmIstanzaPresentazione.aspx?Verso=Ins&VediEnte=1")
                                    ''Response.Redirect("WfrmMain.aspx")
                                    ''PreparaInserimento()
                                    PreparaInserimento2()
                                    cmdInserisci.Visible = True
                                    lblMessaggio.Text = ""
                                    imgEsporta.Visible = False
                                    imgEsportaRiepilogo.Visible = False
                                    lblperImgEsxport.Visible = False
                                    lblperImgEsxportRiepig.Visible = False

                                    Controllacheck()
                                    Exit Sub
                                End If
                                dtr.Close()
                                dtr = Nothing
                            End If
                        Else
                            Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(item.ItemIndex).FindControl("chk")
                            chkoggetto.Checked = False
                        End If
                    Next
                Else
                    'se la griglia appare vuota
                    'lblMessaggio.Text = ""
                    cmdInserisci.Visible = False
                    cmdannulla.Visible = False
                    cmdmodifica.Visible = False
                    Dgtattivita.Visible = False
                    lblprogetto.Visible = True
                    cmddissaccredita.Visible = False
                    cmdaccredita.Visible = False
                End If
            Else 'se invece ci sono progetti legati al bando che sono in stato di proposta ovvero presentati
                dtr.Close()
                dtr = Nothing
                'inizio verifica subiudice
                dtr = ClsServer.CreaDatareader("select" & _
                " ClasseAccreditamentoSubIudice from enti" & _
                " where idente=" & Session("idente") & " and ClasseAccreditamentoSubIudice=1", Session("conn"))
                If dtr.HasRows = True Then 'se subiudice blocco wfrm e visualizzo tasto che indirizza all'albero

                    cmdaccredita.Visible = False
                    cmddissaccredita.Visible = False
                    cmdInserisci.Visible = False
                    cmdmodifica.Visible = False
                    cmdannulla.Visible = False
                    cmdPresentaIstanza.Visible = False
                    lblMessaggioPresenta.Visible = False
                    dtr.Close()
                    dtr = Nothing
                    If controlladata() = True Then
                        cmdPresentaIstanza.Visible = True
                        lblMessaggioPresenta.Visible = True
                    Else
                        cmdPresentaIstanza.Visible = False
                        lblMessaggioPresenta.Visible = False
                        lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè la data attuale non è compresa tra quella di inizio e fine Circolare presentazione progetti."
                    End If
                    'eseguo query per valorizzare gattività
                    strsql = "select att.idattività, titolo," & _
                    " case isnull((select sum(NumeroPostiNoVittoNoAlloggio)+ sum(NumeroPostiVittoAlloggio)+  sum(NumeroPostiVitto) " & _
                    " from attivitàentisediattuazione  where idattività =att.idattività),-1) when -1 then 0 else " & _
                    " (select sum(attivitàentisediattuazione.NumeroPostiNoVittoNoAlloggio)+ sum(attivitàentisediattuazione.NumeroPostiVittoAlloggio)+  sum(attivitàentisediattuazione.NumeroPostiVitto) " & _
                    " from attivitàentisediattuazione " & _
                    " inner join attività on attivitàentisediattuazione.idattività = attività.idattività " & _
                    " INNER JOIN TipiProgetto ON attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
                    " inner join entisediattuazioni on entisediattuazioni.identesedeattuazione = attivitàentisediattuazione.identesedeattuazione " & _
                    " inner join entisedi on entisediattuazioni.identesede = entisedi.identesede inner join comuni on comuni.idcomune = entisedi.idcomune " & _
                    " inner join provincie on provincie.idprovincia = comuni.idprovincia inner join regioni on regioni.idregione = provincie.idregione " & _
                    " inner join nazioni on regioni.idnazione = nazioni.idnazione where attivitàentisediattuazione.idattività =att.idattività and (nazioni.nazionebase=0 or TipiProgetto.nazionebase = 1) end as numerovolontari, att.idbandoattività as bandiattività," & _
                    " (select count(idattivitàentesedeattuazione) from attivitàentisediattuazione a inner join entisediattuazioni e " & _
                    " on e.identesedeattuazione=a.identesedeattuazione inner join statientisedi s " & _
                    " on e.idstatoentesede=s.idstatoentesede where  idattività=att.idattività and " & _
                    " (s.attiva=1 or s.daaccreditare=1 or s.defaultstato=1))as idattsedeatt," & _
                    " (select count(idattivitàsedeassegnazione) from attivitàsediassegnazione a " & _
                    " inner join entisedi e on e.identesede=a.identesede inner join statientisedi s " & _
                    " on e.idstatoentesede=s.idstatoentesede where idattività=att.idattività and " & _
                    " (s.attiva=1 or s.daaccreditare=1 or s.defaultstato=1))as idattsedeass,"
                    strsql = strsql & "(select  " & _
                    " isnull(sum(ceiling(convert(decimal(10,2),(isnull(attivitàentisediattuazione.NumeroPostiNoVittoNoAlloggio,0) + " & _
                    " isnull(attivitàentisediattuazione.NumeroPostiVittoAlloggio, 0)" & _
                    " + isnull(attivitàentisediattuazione.NumeroPostiVitto,0)))/ " & _
                    " isnull(iperambitiattività.MaxVolontariPerOLP,0))),0)" & _
                    " from attività " & _
                    " left join attivitàentisediattuazione on attivitàentisediattuazione.idattività = attività.idattività left join entisediattuazioni on entisediattuazioni.identesedeattuazione = attivitàentisediattuazione.identesedeattuazione " & _
                    " left join entisedi on entisediattuazioni.identesede = entisedi.identesede left join comuni on comuni.idcomune = entisedi.idcomune " & _
                    " left join provincie on provincie.idprovincia = comuni.idprovincia left join regioni on regioni.idregione = provincie.idregione " & _
                    " left join nazioni on regioni.idnazione = nazioni.idnazione" & _
                    " left join ambitiattività  on  (ambitiattività.idambitoattività=attività.idambitoattività) " & _
                    " left join  macroambitiattività  on  (macroambitiattività.idmacroambitoattività=ambitiattività.idmacroambitoattività) " & _
                    " left join iperambitiattività on (macroambitiattività.idiperambitoattività=iperambitiattività.idiperambitiattività)" & _
                    " where attività.idattività =att.idAttività and nazioni.nazionebase = 1 ) as NOlpRic,"

                    strsql = strsql & " (select count(*)  " & _
                    " from associaEntePersonaleRuoliAttivitàEntiSediAttuazione ass  " & _
                    " inner join attivitàentisediattuazione c on (ass.idattivitàentesedeattuazione=c.idattivitàentesedeattuazione) " & _
                    " inner join entepersonaleRuoli epr on epr.identepersonaleruolo= ass.identepersonaleruolo" & _
                    " where epr.datafinevalidità is null and c.idattività=att.idattività and epr.idruolo=1) as nolpins, " & _
                    " (Select  count(entepersonale.identepersonale)  " & _
                    " from entepersonale " & _
                    " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)  " & _
                    " inner join associaEntepersonaleRuoliattivitàentisediattuazione a   " & _
                    " on  (a.idEntepersonaleRuolo=entepersonaleruoli.identepersonaleRuolo)  " & _
                    " inner join attivitàentisediattuazione  " & _
                    " on  (attivitàentisediattuazione.idAttivitàEntesedeAttuazione=a.idAttivitàEntesedeAttuazione)    " & _
                    " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)  " & _
                    " where entepersonaleruoli.datafinevalidità is null and ( entepersonale.datafinevalidità Is null) " & _
                    " and entepersonaleruoli.idruolo=6 and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0)  " & _
                    " and attivitàentisediattuazione.idattività=att.idattività ) as nRleaIns , " & _
                    " (Select  count(entepersonale.identepersonale)  " & _
                    " from entepersonale " & _
                    " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)  " & _
                    " inner join associaEntepersonaleRuoliattivitàentisediattuazione a   " & _
                    " on  (a.idEntepersonaleRuolo=entepersonaleruoli.identepersonaleRuolo)  " & _
                    " inner join attivitàentisediattuazione  " & _
                    " on  (attivitàentisediattuazione.idAttivitàEntesedeAttuazione=a.idAttivitàEntesedeAttuazione)    " & _
                    " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)  " & _
                    " where entepersonaleruoli.datafinevalidità is null and (entepersonale.datafinevalidità Is null) " & _
                    " and entepersonaleruoli.idruolo=5 and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0)  " & _
                    " and attivitàentisediattuazione.idattività=att.idattività ) as nTutorIns, RegioniCompetenze.Descrizione as RegComp  " & _
                    " from bandiattività a" & _
                    " inner join attività att on att.idbandoattività=a.idbandoattività" & _
                    " inner join bando c on a.idbando=c.idbando" & _
                    " inner join statibandiattività d on a.idstatobandoattività=d.idstatobandoattività" & _
                    " INNER JOIN RegioniCompetenze ON att.IDRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza" & _
                    " where a.idente=" & Session("idente") & " And a.idbando=" & Request.QueryString("id") & " and d.DaValutare=1 "

                    'mydataset = ClsServer.DataSetGenerico("select b.idattività, b.titolo," & _
                    '" (numeropostinovittonoalloggio + numeropostivittoalloggio +" & _
                    '" numeropostivitto) as numerovolontari, a.idbandoattività as bandiattività" & _
                    '" from bandiattività a" & _
                    '" inner join attività b on a.idbandoattività=b.idbandoattività" & _
                    '" inner join bando c on a.idbando=c.idbando" & _
                    '" inner join statibandiattività d on a.idstatobandoattività=d.idstatobandoattività" & _
                    '" where a.idente=" & Session("idente") & " And a.idbando=" & Request.QueryString("id") & " and d.DaValutare=1", Session("conn"))
                    mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
                    Dgtattivita.DataSource = mydataset
                    Dgtattivita.DataBind()
                    If Dgtattivita.Items.Count <> 0 Then
                        For intj As Int32 = 0 To mydataset.Tables(0).Rows.Count - 1
                            If mydataset.Tables(0).Rows(intj).Item(3) <> 0 Then
                                Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(intj).FindControl("chk")
                                chkoggetto.Checked = True
                                chkoggetto.Enabled = False
                            Else
                                Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(intj).FindControl("chk")
                                chkoggetto.Checked = False
                                chkoggetto.Enabled = False
                            End If
                        Next
                        Exit Sub 'esco forzatamente per subiudice
                    End If
                    Exit Sub
                End If
                dtr.Close()
                dtr = Nothing

                Dim dtrLocalValutazione As SqlClient.SqlDataReader

                If Not dtrLocalValutazione Is Nothing Then
                    dtrLocalValutazione.Close()
                    dtrLocalValutazione = Nothing
                End If

                dtrLocalValutazione = ClsServer.EseguiStoreReaderInserimentoIstanza("SP_ISTANZA_ELENCO_DISPONIBILI_VALUTAZIONE", Session("idente"), DgdBando.Items(0).Cells(3).Text, Request.QueryString("id"), Session("conn"))

                'mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
                'Dgtattivita.Visible = True '(messo da Antonello il 24/06/2015)'
                Dgtattivita.DataSource = dtrLocalValutazione
                Dgtattivita.DataBind()

                Dim intCountDataReader

                If Not dtrLocalValutazione Is Nothing Then
                    dtrLocalValutazione.Close()
                    dtrLocalValutazione = Nothing
                End If
                'Controllo se il bando selezionato prevede i TUTOR
                Dim blnTutor As Boolean = VerificaTutor(Request.QueryString("id"))
                If blnTutor = False Then 'rendo invisibile la colonna dei tutor
                    Dgtattivita.Columns(10).Visible = False
                Else 'rendo visibile colonna
                    Dgtattivita.Columns(10).Visible = True
                End If
                'rimossa visibilità RLEA
                Dgtattivita.Columns(9).Visible = False
                'ciclo oggetti checkbox della griglia per valorizzarli scorrendo il dataset
                'il processo è dopo la *********presentazione dell'istanza**********
                If Dgtattivita.Items.Count <> 0 Then
                    'For intj As Int32 = 0 To mydataset.Tables(0).Rows.Count - 1
                    Dim item As DataGridItem
                    For Each item In Dgtattivita.Items
                        'If mydataset.Tables(0).Rows(intj).Item(3) <> 0 Then
                        If Dgtattivita.Items(item.ItemIndex).Cells(4).Text <> 0 Then
                            'Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(intj).FindControl("chk")
                            Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(item.ItemIndex).FindControl("chk")
                            chkoggetto.Checked = True
                            chkoggetto.Enabled = False
                        Else
                            'Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(intj).FindControl("chk")
                            Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(item.ItemIndex).FindControl("chk")
                            chkoggetto.Checked = False
                            chkoggetto.Enabled = False
                        End If
                    Next
                    'visualizzo pulsante accreditamento disaccreditamento per unsc
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        cmdaccredita.Visible = True
                        cmddissaccredita.Visible = True
                        Call VerificaCompetenze()
                    Else 'se ente
                        cmdaccredita.Visible = False
                        cmddissaccredita.Visible = False
                    End If
                    'setto input maschera
                    lblMessaggio.Text = "L'istanza risulta essere proposta e pertanto non può essere modificata"
                    cmdInserisci.Visible = False
                    cmdannulla.Visible = False
                    cmdmodifica.Visible = False
                    cmdPresentaIstanza.Visible = False
                    lblMessaggioPresenta.Visible = False
                    'Dgtattivita.Columns(16).Visible = False
                    Dgtattivita.Columns(18).Visible = False
                    imgEsporta.Visible = True
                    lblperImgEsxport.Visible = True
                    hlDw.Visible = False
                    imgEsportaRiepilogo.Visible = True
                    lblperImgEsxportRiepig.Visible = True
                    hlDwRip.Visible = False


                    'If controlladata() = True Then
                    '    cmdPresentaIstanza.Visible = True
                    'Else
                    '    cmdPresentaIstanza.Visible = False
                    '    lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè la data attuale non è compresa tra quella di inizio e fine Bando."
                    '    
                    'End If
                    'cmdaccredita.Visible = True
                    'cmddissaccredita.Visible = True
                Else
                    'se la griglia appare vuota
                    lblMessaggio.Text = ""
                    cmdInserisci.Visible = False
                    cmdannulla.Visible = False
                    cmdmodifica.Visible = False
                    cmdaccredita.Visible = False
                    cmddissaccredita.Visible = False
                    cmdPresentaIstanza.Visible = False
                    lblMessaggioPresenta.Visible = False
                    If controlladata() = True Then
                        cmdPresentaIstanza.Visible = True
                        lblMessaggioPresenta.Visible = True
                    Else
                        cmdPresentaIstanza.Visible = False
                        lblMessaggioPresenta.Visible = False
                        lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè la data attuale non è compresa tra quella di inizio e fine Circolare presentazione progetti."
                    End If
                    lblprogetto.Visible = True
                End If
            End If

        End If

        Controllacheck()
    End Sub

    Private Sub Controllacheck()
        'Generato da Alessandra Taballione il 31/01/2004
        'Controllo se posso inserire una istanza
        Dim item As DataGridItem
        Dim item2 As DataGridItem
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim intCol As Integer

        For Each item In Dgtattivita.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chk"), CheckBox)
            Dim blnControlloCoprogettazione As Boolean

            blnControlloCoprogettazione = False

            'controllo la competenza del progetto
            'se nazionale il numero minimo dev'essere di 4
            'se regionale il numero minimo dev'essere di 2
            'aggiunto il 29/09/2006
            'controllo aggiunto da Jona Strauss Jeans


            'controllo se il progetto è co-progettato
            'se è coprogettato evito di fare il controllo sul numero dei 
            'volontari minimi per il progetto
            'aggiunto il 19/10/2006
            'modificato il 23/10/2006
            'controllo aggiunto da Jon J. Rambo
            'blnControlloCoprogettazione = True  ====> non faccio il controllo sul minimo dei volontari
            'blnControlloCoprogettazione = False ====> faccio il controllo sul minimo dei volontari
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            strsql = "SELECT attività.CoProgettazione as CoProgettazione "
            strsql = strsql & "FROM attività "
            strsql = strsql & "WHERE attività.IdAttività='" & Dgtattivita.Items(item.ItemIndex).Cells(0).Text & "'"

            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                blnControlloCoprogettazione = dtrgenerico("CoProgettazione")
            Else
                blnControlloCoprogettazione = False
            End If

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'modificato da s.c. 
            'If CInt(Dgtattivita.Items(item.ItemIndex).Cells(12).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(13).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(14).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(15).Text) = 1 And _
            '    CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 _
            '    And CInt(Dgtattivita.Items(item.ItemIndex).Cells(17).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(18).Text) = 1 Then
            If CInt(Dgtattivita.Items(item.ItemIndex).Cells(12).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(13).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(14).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(15).Text) = 1 And _
                CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(20).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(21).Text) = 1 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(22).Text) = 1 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(23).Text) = 1 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(24).Text) = 1 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(25).Text) = 1 Then
                '(CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) 
                check.Visible = True
            Else
                check.Visible = False
                Dim intConta As Integer
                For Each item2 In Dgtattivita.Items
                    For intConta = 0 To 16
                        Dgtattivita.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                    Next
                Next
            End If
            ' End If
            'End If
            'controllo i campi CONTROLLOSETTORE,CONTROLLOMAXVOL, CONTROLLOVOLPROG, CONTROLLOVOLSEDI 
            'se sono tutti = 1 posso procedere con l'istanza di prensetazione
            'If CInt(Dgtattivita.Items(item.ItemIndex).Cells(12).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(13).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(14).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(15).Text) = 1 Then
            '    check.Visible = True
            'Else '= 0 errore... inserisco riga rossa
            '    check.Visible = False
            '    Dim intConta As Integer
            '    For Each item2 In Dgtattivita.Items
            '        For intConta = 0 To 11
            '            Dgtattivita.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
            '        Next
            '    Next
            'End If




            'strsql = "SELECT attività.IdTipoProgetto, RegioniCompetenze.CodiceRegioneCompetenza as CodiceRegioneCompetenza "
            'strsql = strsql & "FROM attività "
            'strsql = strsql & "INNER JOIN RegioniCompetenze ON attività.IDRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
            'strsql = strsql & "WHERE attività.IdAttività='" & Dgtattivita.Items(item.ItemIndex).Cells(0).Text & "'"

            'If Not dtrgenerico Is Nothing Then
            '    dtrgenerico.Close()
            '    dtrgenerico = Nothing
            'End If

            'dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            'If dtrgenerico.HasRows = True Then
            '    dtrgenerico.Read()

            ''se nazionale controllo che siano almeno 4 i volontari
            'If dtrgenerico("CodiceRegioneCompetenza") = "NAZ" Then
            '    'AGG. IL 26/10/2006 DA SIMONA CORDELLA
            '    ' CONTROLLO SE L'ISTANZA E' STATA PRESENTATA DALL'ARCI SALTO IL CONTROLLO DEL NUMERO DEI VOLONTARI
            '    'If Session("idente") = 347 Then
            '    '    If CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 And (CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) Then
            '    '        check.Visible = True
            '    '    Else
            '    '        check.Visible = False
            '    '        Dim intConta As Integer
            '    '        For Each item2 In Dgtattivita.Items
            '    '            For intConta = 0 To 11
            '    '                Dgtattivita.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
            '    '            Next
            '    '        Next
            '    '    End If
            '    'Else

            '    'If IIf(Dgtattivita.Items(item.ItemIndex).Cells(2).Text <> "&nbsp;", Dgtattivita.Items(item.ItemIndex).Cells(2).Text, 0) < IIf(dtrgenerico("IdTipoProgetto") = 3, 1, 4) Then
            '    '    check.Visible = False
            '    '    For intCol = 0 To Dgtattivita.Columns.Count - 1
            '    '        Dgtattivita.Items(item.ItemIndex).Cells(intCol).BackColor = Color.LightSalmon
            '    '    Next
            '    'Else

            'Else
            '    If 1 = 1 Then
            '        ''AGG. IL 26/10/2006 DA SIMONA CORDELLA
            '        '' CONTROLLO SE L'ISTANZA E' STATA PRESENTATA DALL'ARCI SALTO IL CONTROLLO DEL NUMERO DEI VOLONTARI
            '        'If Session("idente") = 347 Then
            '        '    If CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 And (CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) Then
            '        '        check.Visible = True
            '        '    Else
            '        '        check.Visible = False
            '        '        Dim intConta As Integer
            '        '        For Each item2 In Dgtattivita.Items
            '        '            For intConta = 0 To 11
            '        '                Dgtattivita.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
            '        '            Next
            '        '        Next
            '        '    End If
            '        'Else
            '        'altrimeni controllo che devono essere almeno 2 volontari
            '        'If IIf(Dgtattivita.Items(item.ItemIndex).Cells(2).Text <> "&nbsp;", Dgtattivita.Items(item.ItemIndex).Cells(2).Text, 0) < IIf(dtrgenerico("IdTipoProgetto") = 3, 1, 2) Then

            '        '    check.Visible = False

            '        '    For intCol = 0 To Dgtattivita.Columns.Count - 1
            '        '        Dgtattivita.Items(item.ItemIndex).Cells(intCol).BackColor = Color.LightSalmon
            '        '    Next
            '        'Else
            '        'controllo i campi CONTROLLOSETTORE,CONTROLLOMAXVOL, CONTROLLOVOLPROG, CONTROLLOVOLSEDI 
            '        'se sono tutti = 1 posso procedere con l'istanza di prensetazione
            '        If CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 And (CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) Then
            '            check.Visible = True
            '        Else '= 0 errore... inserisco riga rossa
            '            check.Visible = False
            '            Dim intConta As Integer
            '            For Each item2 In Dgtattivita.Items
            '                For intConta = 0 To 11
            '                    Dgtattivita.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
            '                Next
            '            Next
            '            'End If
            '        End If
            '        ' End If
            '        'Else
            '        '    If CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 And (CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) Then
            '        '        check.Visible = True
            '        '    Else
            '        '        check.Visible = False
            '        '        Dim intConta As Integer
            '        '        For Each item2 In Dgtattivita.Items
            '        '            For intConta = 0 To 11
            '        '                Dgtattivita.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
            '        '            Next
            '        '        Next
            '        ''    End If
            '        'controllo i campi CONTROLLOSETTORE,CONTROLLOMAXVOL, CONTROLLOVOLPROG, CONTROLLOVOLSEDI 
            '        'se sono tutti = 1 posso procedere con l'istanza di prensetazione
            '        If CInt(Dgtattivita.Items(item.ItemIndex).Cells(12).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(13).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(14).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(15).Text) = 1 Then
            '            check.Visible = True
            '        Else '= 0 errore... inserisco riga rossa
            '            check.Visible = False
            '            Dim intConta As Integer
            '            For Each item2 In Dgtattivita.Items
            '                For intConta = 0 To 11
            '                    Dgtattivita.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
            '                Next
            '            Next
            '        End If
            '    End If
            'End If
            'End If

            'If Not dtrgenerico Is Nothing Then
            '    dtrgenerico.Close()
            '    dtrgenerico = Nothing
            'End If
        Next
    End Sub

    Private Sub Cotacheck()
        'Generato da Alessandra Taballione il 18/07/2005
        Dim item As DataGridItem
        Dim conta As Integer
        Dim bloccato As Boolean
        Dim blnControlloCoprogettazione As Boolean

        blnControlloCoprogettazione = False

        conta = 0
        For Each item In Dgtattivita.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chk"), CheckBox)


            'controllo la competenza del progetto
            'se nazionale il numero minimo dev'essere di 4
            'se regionale il numero minimo dev'essere di 2
            'aggiunto il 29/09/2006
            'controllo aggiunto da Jona Strauss Jeans

            'controllo se il progetto è co-progettato
            'se è coprogettato evito di fare il controllo sul numero dei 
            'volontari minimi per il progetto
            'aggiunto il 19/10/2006
            'modificato il 23/10/2006
            'controllo aggiunto da Jon J. Rambo
            'blnControlloCoprogettazione = True  ====> non faccio il controllo sul minimo dei volontari
            'blnControlloCoprogettazione = False ====> faccio il controllo sul minimo dei volontari
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            strsql = "SELECT attività.CoProgettazione as CoProgettazione "
            strsql = strsql & "FROM attività "
            strsql = strsql & "WHERE attività.IdAttività='" & Dgtattivita.Items(item.ItemIndex).Cells(0).Text & "'"

            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                blnControlloCoprogettazione = dtrgenerico("CoProgettazione")
            Else
                blnControlloCoprogettazione = False
            End If

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'modificato da s.c. 
            'If IIf(Dgtattivita.Items(item.ItemIndex).Cells(2).Text <> "&nbsp;", Dgtattivita.Items(item.ItemIndex).Cells(2).Text, 0) >= IIf(dtrgenerico("IdTipoProgetto") = 3, 1, 2) Then
            'If CInt(Dgtattivita.Items(item.ItemIndex).Cells(12).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(13).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(14).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(15).Text) = 1 And _
            '    CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 _
            '    And CInt(Dgtattivita.Items(item.ItemIndex).Cells(17).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(18).Text) = 1 Then
            If CInt(Dgtattivita.Items(item.ItemIndex).Cells(12).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(13).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(14).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(15).Text) = 1 And _
                CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(20).Text) = 1 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(21).Text) = 1 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(22).Text) = 1 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(23).Text) = 1 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(24).Text) = 1 _
                And CInt(Dgtattivita.Items(item.ItemIndex).Cells(25).Text) = 1 Then
                check.Visible = True
                conta = conta + 1
                If check.Enabled = False Then
                    bloccato = True
                End If
            End If
            'End If





            'strsql = "SELECT attività.IdTipoProgetto, RegioniCompetenze.CodiceRegioneCompetenza as CodiceRegioneCompetenza "
            'strsql = strsql & "FROM attività "
            'strsql = strsql & "INNER JOIN RegioniCompetenze ON attività.IDRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
            'strsql = strsql & "WHERE attività.IdAttività='" & Dgtattivita.Items(item.ItemIndex).Cells(0).Text & "'"

            'If Not dtrgenerico Is Nothing Then
            '    dtrgenerico.Close()
            '    dtrgenerico = Nothing
            'End If

            'dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            'If dtrgenerico.HasRows = True Then
            '    dtrgenerico.Read()
            '    If dtrgenerico("CodiceRegioneCompetenza") = "NAZ" Then

            '        'AGG. IL 26/10/2006 DA SIMONA CORDELLA
            '        '  SE L'ISTANZA E' STATA PRESENTATA DALL'ARCI SALTO IL CONTROLLO DEL NUMERO DEI VOLONTARI
            '        'If Session("idente") = 347 Then
            '        '    If CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 And (CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) Then
            '        '        check.Visible = True
            '        '        conta = conta + 1
            '        '        If check.Enabled = False Then
            '        '            bloccato = True
            '        '        End If
            '        '    End If
            '        'Else
            '        If IIf(Dgtattivita.Items(item.ItemIndex).Cells(2).Text <> "&nbsp;", Dgtattivita.Items(item.ItemIndex).Cells(2).Text, 0) >= IIf(dtrgenerico("IdTipoProgetto") = 3, 1, 4) Then
            '            If CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 And (CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) Then
            '                check.Visible = True
            '                conta = conta + 1
            '                If check.Enabled = False Then
            '                    bloccato = True
            '                End If
            '            End If
            '        End If
            '        'End If
            '    Else
            '        If 1 = 1 Then
            '            If IIf(Dgtattivita.Items(item.ItemIndex).Cells(2).Text <> "&nbsp;", Dgtattivita.Items(item.ItemIndex).Cells(2).Text, 0) >= IIf(dtrgenerico("IdTipoProgetto") = 3, 1, 2) Then
            '                If CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 And (CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) Then
            '                    check.Visible = True
            '                    conta = conta + 1
            '                    If check.Enabled = False Then
            '                        bloccato = True
            '                    End If
            '                End If
            '            End If
            '            'Else
            '            '    If CInt(Dgtattivita.Items(item.ItemIndex).Cells(5).Text) <> 0 And CInt(Dgtattivita.Items(item.ItemIndex).Cells(6).Text) <> 0 And (CInt(Dgtattivita.Items(item.ItemIndex).Cells(8).Text) >= CInt(Dgtattivita.Items(item.ItemIndex).Cells(7).Text)) Then
            '            '        check.Visible = True
            '            '        conta = conta + 1
            '            '        If check.Enabled = False Then
            '            '            bloccato = True
            '            '        End If
            '            '''    End If
            '        End If
            '    End If
            'End If
        Next

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        If conta > 1 And bloccato = False Then
            chkSelDesel.Visible = True
            chkSelDesel2.Visible = True
            'lblseltutto.Visible = True
            'ImgSelezionaTutto.Visible = True
            'lblseltutto2.Visible = True
            'ImgSelezionaTutto2.Visible = True
        End If
        If Dgtattivita.Visible = False Then
            chkSelDesel.Visible = False
            chkSelDesel2.Visible = False
            'lblseltutto.Visible = False
            'ImgSelezionaTutto.Visible = False
            'lblseltutto2.Visible = False
            'ImgSelezionaTutto2.Visible = False
        End If
    End Sub

    Private Function ControllaRISORSE() As Byte
        Dim myDataSet As DataSet
        Dim MyQuery As New System.Collections.ArrayList
        Dim blnControlloClasse As Boolean 'verfico se è possibile controllo sulle risorse
        Dim blnTutor As Boolean
        blnControlloClasse = False
        'AGG. IL 26/10/2006 DA SIMONA CORDELLA
        'CONTROLLO SE L'ENTE è DI CLASSE <>4 SALTO IL CONTROLLO
        strsql = "Select IdClasseAccreditamento from Enti where idente =" & Session("Idente") & ""

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        If dtrgenerico("IdClasseAccreditamento") <> 4 Then
            blnControlloClasse = True
        Else
            blnControlloClasse = False
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'agg. il 16/11/2009
        blnTutor = VerificaTutor(txtidbando.Text)

        If blnControlloClasse = True Then
            'Creazione della Tabella di Appoggio
            MyQuery.Add("CREATE TABLE #CONTROLLOPROVINCIE (" & _
                        "IdProvincia INT, Provincia VARCHAR(100), NVol INT, NRleaRic INT, NTutorRic INT, NRleaIns INT, NTutorIns INT)")

            'Inserimento Dei Dati Nella Tabella
            strsql = "Insert Into #CONTROLLOPROVINCIE(IdProvincia,Provincia,NVol,NRleaRic,NTutorRic) " & _
                     "SELECT  P.IdProvincia,P.Provincia,Sum(IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0)+ IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)) NVol," & _
                     "Case When Sum(IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0)+ IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)) >= 30 Then 1 Else 0 End RLEARic," & _
                     "Sum(IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0)+ IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0))/30 TutorRic " & _
                     "FROM bando " & _
                     "INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando " & _
                     "INNER JOIN Attività ON BandiAttività.IdBandoAttività = Attività.IDBandoAttività " & _
                     "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IDAttività = AttivitàEntiSediAttuazione.IDAttività " & _
                     "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi ON EntiSediAttuazioni.IDEnteSede = EntiSedi.IDEnteSede " & _
                     "INNER JOIN Comuni ON EntiSedi.IDComune = Comuni.IDComune " & _
                     "INNER JOIN Provincie P ON Comuni.IDProvincia = P.IDProvincia " & _
                     "INNER JOIN REGIONI R on P.IdRegione = R.IdRegione " & _
                     "INNER JOIN NAZIONI N ON R.IDNAZIONE = N.IDNAZIONE " & _
                     "WHERE NOT Attività.IdTipoProgetto IN (2,6,8,10) and Attività.IdEntePresentante = " & Session("Idente") & " AND (BANDO.GRUPPO = (select GRUPPO from bando where idbando = '" & txtidbando.Text & "') ) AND N.NAZIONEBASE = 1 and entisediattuazioni.identecapofila = bandiattività.idente " & _
                     "Group by P.IdProvincia,P.Provincia "
            MyQuery.Add(strsql)

            'Metto gli elementi RLEA inseriti nelle varie provincie
            strsql = "Select COUNT(Distinct Entepersonale.IDEntePersonale) As NRLEA ,Comuni.IdProvincia " & _
                     "Into #TmpRleaIns " & _
                     "FROM EntePersonale " & _
                     "INNER JOIN EntePersonaleRuoli ON EntePersonaleRuoli.IDEntePersonale = Entepersonale.IDEntePersonale " & _
                     "INNER JOIN AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione a ON a.IdEntePersonaleRuolo = EntePersonaleRuoli.IDEntePersonaleRuolo " & _
                     "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione = a.IdAttivitàEnteSedeAttuazione " & _
                     "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "inner join bando ON BANDIATTIVITà.IDBANDO = BANDO.IDBANDO " & _
                     "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IDEnteSedeAttuazione = EntiSediAttuazioni.IDEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi ON EntiSediAttuazioni.IDEnteSede = EntiSedi.IDEnteSede " & _
                     "INNER JOIN Comuni ON Entisedi.IDComune = Comuni.IDComune " & _
                     "WHERE entepersonaleruoli.datafinevalidità is null and BANDO.GRUPPO = (select GRUPPO from bando where idbando = '" & txtidbando.Text & "') AND BandiAttività.IDEnte = " & Session("Idente") & " " & _
                     "AND EntePersonaleRuoli.IDRuolo = 6 AND (EntePersonaleRuoli.Accreditato = 1 OR EntePersonaleRuoli.Accreditato = 0) " & _
                     "Group By Comuni.IdProvincia"
            MyQuery.Add(strsql)

            '******* MODIFICA PER PRESENTAZIONE DEI PROGETTI 2010 *******
            'Mod. il 13/11/2009 da Simona Cordella Il Tutor non è più una figura necessaria

            ''Metto gli elementi TUTOR inseriti nelle varie provincie
            If blnTutor = True Then
                strsql = "Select COUNT(Distinct Entepersonale.IDEntePersonale) As NTUTOR ,Comuni.IdProvincia " & _
                         "Into #TmpTutorIns " & _
                         "FROM EntePersonale " & _
                         "INNER JOIN EntePersonaleRuoli ON EntePersonaleRuoli.IDEntePersonale = Entepersonale.IDEntePersonale " & _
                         "INNER JOIN AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione a ON a.IdEntePersonaleRuolo = EntePersonaleRuoli.IDEntePersonaleRuolo " & _
                         "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione = a.IdAttivitàEnteSedeAttuazione " & _
                         "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                         "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                         "inner join bando ON BANDIATTIVITà.IDBANDO = BANDO.IDBANDO " & _
                         "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IDEnteSedeAttuazione = EntiSediAttuazioni.IDEnteSedeAttuazione " & _
                         "INNER JOIN EntiSedi ON EntiSediAttuazioni.IDEnteSede = EntiSedi.IDEnteSede " & _
                         "INNER JOIN Comuni ON Entisedi.IDComune = Comuni.IDComune " & _
                         "WHERE entepersonaleruoli.datafinevalidità is null and BANDO.GRUPPO = (select GRUPPO from bando where idbando = '" & txtidbando.Text & "') AND BandiAttività.IDEnte = " & Session("Idente") & "  " & _
                         "AND EntePersonaleRuoli.IDRuolo = 5 AND (EntePersonaleRuoli.Accreditato = 1 OR EntePersonaleRuoli.Accreditato = 0) " & _
                         "Group By Comuni.IdProvincia"
                MyQuery.Add(strsql)
            End If

            '****************** FINE ********************
            'Aggiornamento del numero degli RLEA
            strsql = "UpDate #CONTROLLOPROVINCIE Set #CONTROLLOPROVINCIE.NRleaIns = #TmpRleaIns.NRLEA " & _
                     "From #CONTROLLOPROVINCIE " & _
                     "INNER JOIN #TmpRleaIns ON #CONTROLLOPROVINCIE.IdProvincia = #TmpRleaIns.IdProvincia"
            MyQuery.Add(strsql)
            '******* MODIFICA PER PRESENTAZIONE DEI PROGETTI 2010 *******
            'Mod. il 13/11/2009 da Simona Cordella Il Tutor non è più una figura necessaria
            'Aggiornamento del numero di TUTOR
            If blnTutor = True Then
                strsql = "UpDate #CONTROLLOPROVINCIE Set #CONTROLLOPROVINCIE.NTutorIns = #TmpTutorIns.NTUTOR " & _
                                     "From #CONTROLLOPROVINCIE " & _
                                     "INNER JOIN #TmpTutorIns ON #CONTROLLOPROVINCIE.IdProvincia = #TmpTutorIns.IdProvincia"
                MyQuery.Add(strsql)
            End If
            '****************** FINE ********************
            'Imposto a zero tutti gli elementi con null
            MyQuery.Add("Update #CONTROLLOPROVINCIE Set NRleaIns = 0 Where NRleaIns Is Null")
            MyQuery.Add("Update #CONTROLLOPROVINCIE Set NTutorIns = 0 Where NTutorIns Is Null")

            If ClsServer.EseguiQueryColl(MyQuery, Session.SessionID, Session("Conn")) = True Then
                strsql = "Select * From #CONTROLLOPROVINCIE"
                strsql = strsql & " Where NRleaRic > NRleaIns "
                'Or NTutorRic > NTutorIns"
                strsql = strsql & " Order By Provincia"
                'Carico il DataSet
                myDataSet = ClsServer.DataSetGenerico(strsql, Session("conn"))

                If myDataSet.Tables(0).Rows.Count <> 0 Then
                    ControllaRISORSE = 1
                Else
                    ControllaRISORSE = 0
                End If

                'Elimino le Tabelle di Appoggio
                ClsServer.EseguiSqlClient("DROP TABLE #CONTROLLOPROVINCIE", Session("Conn"))
                ClsServer.EseguiSqlClient("DROP TABLE #TmpRleaIns", Session("Conn"))
                '******* MODIFICA PER PRESENTAZIONE DEI PROGETTI 2010 *******
                'Mod. il 13/11/2009 da Simona Cordella Il Tutor non è più una figura necessaria
                'ClsServer.EseguiSqlClient("DROP TABLE #TmpTutorIns", Session("Conn"))
                '****************** FINE ********************
            Else
                Response.Write("<SCRIPT>" & vbCrLf)
                Response.Write("alert('Si sono verificati problemi durante l\'accesso ai dati')" & vbCrLf)
                Response.Write("self.close()" & vbCrLf)
                Response.Write("</SCRIPT>")
                ControllaRISORSE = 1
            End If
        End If
    End Function

    Private Function ControllaOlpSedi() As Byte
        Dim strsql As String
        Dim dt As DataTable
        Dim myRow As DataRow
        Dim dtrgenerico As SqlClient.SqlDataReader
        strsql = "SELECT epr.identepersonaleRUOLO " & _
        " FROM bando " & _
        " INNER JOIN   BandiAttività ON bando.IDBando = BandiAttività.IdBando" & _
        " INNER JOIN   attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività " & _
        " INNER JOIN   attivitàentisediattuazione  ON attività.IDAttività = attivitàentisediattuazione.IDAttività " & _
        " inner join   associaEntepersonaleRuoliAttivitàentisediattuazione apra  " & _
        " on apra.idattivitàentesedeattuazione=attivitàentisediattuazione.idattivitàentesedeattuazione " & _
        " inner join   entepersonaleRuoli epr on epr.identepersonaleruolo=apra.identepersonaleruolo " & _
        " WHERE (attività.identepresentante = " & Session("Idente") & " AND (BANDO.GRUPPO = (select GRUPPO from bando where idbando = '" & txtidbando.Text & "') ) And epr.idruolo = 1)" & _
        " group by epr.identepersonaleRUOLO " & _
        " having (COUNT(DISTINCT attivitàentisediattuazione.IDEnteSedeAttuazione))  >1 "
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dt = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        For Each myRow In dt.Rows
            strsql = "SELECT count(DISTINCT attivitàentisediattuazione.IDEnteSedeAttuazione) as sediatt " & _
                    " FROM  AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione INNER JOIN " & _
                    " attivitàentisediattuazione ON " & _
                    " AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
                    " entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
                    " INNER JOIN   attività ON attivitàentisediattuazione.Idattività = attività.IDAttività " & _
                    " INNER JOIN   BandiAttività ON attività.IDBandoattività = BandiAttività.IdBandoattività " & _
                    " inner join bando ON BANDIATTIVITà.IDBANDO = BANDO.IDBANDO " & _
                    " WHERE  AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdEntePersonaleRuolo = " & myRow.Item("identepersonaleRUOLO") & " and (attività.identepresentante = " & Session("Idente") & ") AND (BANDO.GRUPPO = (select GRUPPO from bando where idbando = '" & txtidbando.Text & "') ) "
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrgenerico.Read()
            If dtrgenerico("sediatt") > 1 Then
                ControllaOlpSedi = 1
                Exit For
            Else
                ControllaOlpSedi = 0
            End If
        Next

        'If dtrgenerico.HasRows = True Then
        '    ControllaOlpSedi = 1
        'Else
        '    ControllaOlpSedi = 0
        'End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Private Function ControllaRuoliRisorse() As Byte
        'Agg. il 03/10/2006 da Simona Cordella
        'Controllo se esistono risorse con ruoli incompatibili
        Dim strsql As String
        Dim dt As DataTable
        Dim myRow As DataRow
        Dim dtrgenerico As SqlClient.SqlDataReader

        strsql = "SELECT  entepersonale.Cognome, entepersonale.Nome, entepersonale.CodiceFiscale, COUNT(DISTINCT entepersonaleruoli.IDRuolo) AS NRuoli,entepersonale.IdentePersonale"
        strsql = strsql & " FROM bando INNER JOIN"
        strsql = strsql & " BandiAttività ON bando.IDBando = BandiAttività.IdBando INNER JOIN"
        strsql = strsql & " attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività INNER JOIN"
        strsql = strsql & " attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività INNER JOIN"
        strsql = strsql & " AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione ON "
        strsql = strsql & " attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione INNER JOIN"
        strsql = strsql & " entepersonaleruoli ON "
        strsql = strsql & " AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdEntePersonaleRuolo = entepersonaleruoli.IDEntePersonaleRuolo INNER JOIN"
        strsql = strsql & " entepersonale ON entepersonaleruoli.IDEntePersonale = entepersonale.IDEntePersonale"
        strsql = strsql & " WHERE (attività.IDEntePresentante ='" & Session("Idente") & "') "
        strsql = strsql & " AND (BANDO.GRUPPO = (select GRUPPO from bando where idbando = '" & txtidbando.Text & "') ) "
        strsql = strsql & " GROUP BY entepersonale.Cognome, entepersonale.Nome, entepersonale.CodiceFiscale,entepersonale.IdentePersonale "
        strsql = strsql & " HAVING (COUNT(DISTINCT entepersonaleruoli.IDRuolo) > 1)"

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            ControllaRuoliRisorse = 1
        Else
            ControllaRuoliRisorse = 0
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Private Function ProceduraDiModifica() As Byte
        '***Generata da Gianluigi Paesani in data:16/07/04
        '***Questa routine gestisce la modifica dell'istanza inserita(presentata)
        Dim booverifica As Boolean = False 'setto a default segnalazione dei processi della maschera

        lblMessaggio.Text = ""

        Dim booverifica1 As Boolean = True
        'verifico che almeno un elemento sia ceccato
        ProceduraDiModifica = 0 'setto a zero
        If Dgtattivita.Items.Count <> 0 Then
            For intj As Int32 = 0 To Dgtattivita.Items.Count - 1
                Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(intj).FindControl("chk")
                If chkoggetto.Checked = True Then
                    booverifica1 = False
                    Exit For
                End If
            Next
        End If

        If booverifica1 = True Then 'blocco comando se risultano non ckeccati tutti
            lblMessaggio.Text = "Attenzione, deve esistere almeno un progetto nell'Istanza di Presentazione"

            ProceduraDiModifica = 1 'setto per uscita se chiamata arriva da presentazioneistanza
            Exit Function
        End If
        'eseguo modifica
        For intj As Int32 = 0 To Dgtattivita.Items.Count - 1 'ciclo griglia attività(progetti)
            If Dgtattivita.Items(intj).Cells(4).Text <> "0" Then 'controllo se campo è valorizzato(prima flag si adesso flag no)
                Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(intj).FindControl("chk")
                If chkoggetto.Checked = False Then 'verifico lo stato delle check in griglia
                    'se il campo e valorizzato ho la chiave della tabella ma se è modificata proseguo 
                    Dim cmdinsert As Data.SqlClient.SqlCommand
                    'modifico prima la cronologia dell'attività
                    cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaAttività" & _
                    " (idattività,idstatoattività,datacronologia,idTipoCronologia," & _
                    " usernameaccreditatore)" & _
                    " select " & CInt(Dgtattivita.Items(intj).Cells(0).Text) & "," & _
                    " idstatoattività,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'" & _
                    " from attività where idattività=" & CInt(Dgtattivita.Items(intj).Cells(0).Text) & "", Session("conn"))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()
                    'modifico tabella attività progetti
                    cmdinsert = New Data.SqlClient.SqlCommand("update attività" & _
                    "  set idbandoattività=null," & _
                    " idstatoattività=(select idstatoattività from" & _
                    "  statiattività where defaultstato=1)" & _
                    " where idattività=" & CInt(Dgtattivita.Items(intj).Cells(0).Text) & "", Session("conn"))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose() 'modifico tabella dell'attività
                    booverifica = True 'setto variabile per modifica
                End If
            Else 'se non e valorizzato(prima flag no adesso flag si)
                Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(intj).FindControl("chk")
                If chkoggetto.Checked = True Then ' controllo valore check griglia
                    'se il valore chiave e a default (0) ed è stata eseguita la modifica eseguo comandi
                    Dim cmdinsert As Data.SqlClient.SqlCommand
                    'modifico prima la cronologia dell'attività
                    cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaAttività" & _
                    " (idattività,idstatoattività,datacronologia,idTipoCronologia," & _
                    " usernameaccreditatore)" & _
                    " select " & CInt(Dgtattivita.Items(intj).Cells(0).Text) & "," & _
                    " idstatoattività,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'" & _
                    " from attività where idattività=" & CInt(Dgtattivita.Items(intj).Cells(0).Text) & "", Session("conn"))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()
                    'modifico l'attivita
                    cmdinsert = New Data.SqlClient.SqlCommand("update attività" & _
                    " set idstatoattività=(select idstatoattività from statiattività" & _
                    " where Davalutare=1),idbandoattività=" & Request.QueryString("idBA") & "" & _
                    " where idattività=" & Dgtattivita.Items(intj).Cells(0).Text & "", Session("conn"))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose() 'modifica tabella attività
                    booverifica = True
                End If

            End If
        Next
        If booverifica = True Then 'se è stATA eseguita la modifica
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                Call VerificaCompetenze()
            End If
            lblMessaggio.Text = "L'Istanza è stata Modificata con successo"
            'cmdInserisci.Visible = False
            'cmdannulla.Visible = False
            'cmdmodifica.Visible = False
            'cmdPresentaIstanza.Visible = False

        Else
            'da decidere se segnalare la non avvenuta modifica
        End If
        'CaricaAttività()
        CaricaGriglia()
        'If ReturnRegioneCompetenzaBando(Request.QueryString("idBA")) <> 22 Then
        '    imgEsporta.Visible = False
        '    imgEsportaRiepilogo.Visible = False
        '    
        '    
        '    Dgtattivita.Columns(18).Visible = False
        '    Dgtattivita.Columns(17).Visible = False
        '    Dgtattivita.Columns(16).Visible = False
        'End If
    End Function

    Private Function ControllaDISABILI() As Byte
        Dim strsql As String
        Dim dt As DataTable
        Dim myRow As DataRow
        Dim dtrgenerico As SqlClient.SqlDataReader

        strsql = "SELECT DISTINCT "
        strsql = strsql & "attività.CodiceEnte, attività.Titolo, ISNULL(attività.NumeroPostiNoVittoNoAlloggio, 0) + ISNULL(attività.NumeroPostiVittoAlloggio, 0) "
        strsql = strsql & "+ ISNULL(attività.NumeroPostiVitto, 0) AS Volontari "
        strsql = strsql & "FROM bando INNER JOIN "
        strsql = strsql & "BandiAttività ON bando.IDBando = BandiAttività.IdBando INNER JOIN "
        strsql = strsql & "attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività INNER JOIN "
        strsql = strsql & "AssociaBandoTipiProgetto ON bando.IDBando = AssociaBandoTipiProgetto.IdBando LEFT OUTER JOIN "
        strsql = strsql & "AttivitàAccompagnamento ON attività.IDAttività = AttivitàAccompagnamento.IDAttività "
        strsql = strsql & "WHERE (bando.IDBando = '" & txtidbando.Text & "') AND (BandiAttività.IdEnte = '" & Session("IdEnte") & "') AND (AttivitàAccompagnamento.IdAttivitàAccompagnamento IS NULL) AND "
        strsql = strsql & "(attività.IdTipoProgetto = 3)"

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            ControllaDISABILI = 1
        Else
            ControllaDISABILI = 0
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Function

    Private Function TrovaRegioneCompetenzaBando(ByVal IDBando As Integer) As Integer
        Dim dtr As SqlClient.SqlDataReader
        Dim idReg As Integer
        strsql = "select IdRegioneCompetenza  from AssociaBandoRegioniCompetenze where IdBando =" & IDBando & ""
        dtr = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtr.HasRows = True Then
            dtr.Read()
            idReg = dtr("IdRegioneCompetenza")
        End If
        dtr.Close()
        dtr = Nothing
        Return idReg
    End Function

    Private Sub PerrsonalizzaTasti(ByVal defaultstato As String, ByVal davalutare As String, ByVal attivo As String, ByVal chiuso As String, ByVal inammissibile As String, ByVal cancellata As String)
        'Aggiunta da Alessandra Taballione il 02/02/2005
        'Visualizzazione dei tasti secondo lo stato
        If defaultstato = True Then 'Registrata
            If controlladata() = True Then
                cmdPresentaIstanza.Visible = True
                lblMessaggioPresenta.Visible = True
            Else
                cmdPresentaIstanza.Visible = False
                lblMessaggioPresenta.Visible = False
                lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè la data attuale non è compresa tra quella di inizio e fine Circolare presentazione progetti."

            End If
            cmddissaccredita.Visible = False
            cmdmodifica.Visible = True
            cmdaccredita.Visible = False
            cmdannulla.Visible = True
        End If
        If davalutare = True Then 'Proposta
            cmdPresentaIstanza.Visible = False
            lblMessaggioPresenta.Visible = False
            cmddissaccredita.Visible = True
            cmdmodifica.Visible = False
            cmdaccredita.Visible = True
            cmdannulla.Visible = False
            bloccaCheck()
        End If
        If attivo = True Then 'approvata
            cmdPresentaIstanza.Visible = False
            lblMessaggioPresenta.Visible = False
            cmddissaccredita.Visible = False
            cmdmodifica.Visible = False
            cmdaccredita.Visible = False
            cmdannulla.Visible = False
            cmdAnnullaPresentazione.Visible = False
            bloccaCheck()
        End If
        If chiuso = True And cancellata = True Then 'Cancellata
            cmdPresentaIstanza.Visible = False
            lblMessaggioPresenta.Visible = False
            cmddissaccredita.Visible = False
            cmdmodifica.Visible = False
            cmdaccredita.Visible = False
            cmdannulla.Visible = False
            cmdRipristina.Visible = False
            bloccaCheck()
        End If

        If chiuso = True And inammissibile = True Then 'Inammissibile
            If controlladata() = True Then
                cmdPresentaIstanza.Visible = True
                lblMessaggioPresenta.Visible = True
            Else
                cmdPresentaIstanza.Visible = False
                lblMessaggioPresenta.Visible = False
                lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè la data attuale non è compresa tra quella di inizio e fine Circolare presentazione progetti."

            End If
            cmddissaccredita.Visible = False
            cmdmodifica.Visible = True
            cmdaccredita.Visible = False
            cmdannulla.Visible = True
            'ripristino solo se non ho la data inizio bando volontari
            If ControlloDataInizioVolontari(Request.QueryString("idBA")) = True Then
                cmdRipristina.Visible = True
            Else
                cmdRipristina.Visible = False
            End If

            bloccaCheck()
        End If
    End Sub
    Function ControlloDataInizioVolontari(ByVal IdBandoAttivita As Integer) As Boolean
        Dim dtr As Data.SqlClient.SqlDataReader
        Dim blnVol As Boolean
        Dim strQuery As String
        If Not dtr Is Nothing Then
            dtr.Close()
            dtr = Nothing
        End If
        strQuery = " select b.*  from BandiAttività a" & _
                   " inner join bando b  on a.IdBando =b.IDBando " & _
                   " where IdBandoAttività = " & IdBandoAttivita & " And DataInizioVolontari Is null"
        dtr = ClsServer.CreaDatareader(strQuery, Session("conn"))
        blnVol = dtr.HasRows
        If Not dtr Is Nothing Then
            dtr.Close()
            dtr = Nothing
        End If

        Return blnVol
    End Function
    Function VerificaIstanzaInammissibile(ByVal IdBandoAttivita As Integer) As Boolean
        'creata da Simona Cordella 19/05/2017
        'controllo se l'istanza è nellop stato di INAMMISSIBILE
        Dim dtr As Data.SqlClient.SqlDataReader
        Dim blnVol As Boolean
        Dim strQuery As String
        If Not dtr Is Nothing Then
            dtr.Close()
            dtr = Nothing
        End If
        strQuery = " select * FROM BandiAttività " & _
                   " where IdBandoAttività = " & IdBandoAttivita & " and IdStatoBandoAttività=5"
        dtr = ClsServer.CreaDatareader(strQuery, Session("conn"))
        blnVol = dtr.HasRows
        If Not dtr Is Nothing Then
            dtr.Close()
            dtr = Nothing
        End If

        Return blnVol


    End Function




    Function controlladata() As Boolean
        'MODIFICATA DA SIMONA CORDELLA IL 08/07/2013
        'ESTRAGGO LE DATE DAL DB E CONTROLLO CON LA DATA ODIERNA


        'variabili che utilizzo per formattare la data di inizio bando
        'sulla quale farò i controlli per cui visualizzare o meno 
        'il pulsante di Presentazione di Istanza
        Dim strGiornoInizio As String
        Dim strMeseInizio As String
        Dim strAnnoInizio As String
        Dim strdataInizio As String

        'variabili che utilizzo per formattare la data di fine bando
        'sulla quale farò i controlli per cui visualizzare o meno 
        'il pulsante di Presentazione di Istanza
        Dim strGiornoFine As String
        Dim strMeseFine As String
        Dim strAnnoFine As String
        Dim strDataFine As String
        Dim strDataAttuale As String

        Dim rtsDataBando As SqlClient.SqlDataReader
        If Not rtsDataBando Is Nothing Then
            rtsDataBando.Close()
            rtsDataBando = Nothing
        End If

        strsql = " Select  DataInizioValidità, DataFineValidità ,GETDATE() AS DATAODIERNA  " & _
                 " FROM bando WHERE IdBando=" & txtidbando.Text & " "


        rtsDataBando = ClsServer.CreaDatareader(strsql, Session("conn"))
        If rtsDataBando.HasRows = True Then
            rtsDataBando.Read()
            'carico la data inversa di inizio rtsDataBando("DataInizioValidità")
            strGiornoInizio = IIf(Len(CStr(Day(rtsDataBando("DataInizioValidità")))) < 2, "0" & Day(rtsDataBando("DataInizioValidità")), Day(rtsDataBando("DataInizioValidità")))
            strMeseInizio = IIf(Len(CStr(Month(rtsDataBando("DataInizioValidità")))) < 2, "0" & Month(rtsDataBando("DataInizioValidità")), Month(rtsDataBando("DataInizioValidità")))
            strAnnoInizio = Year(rtsDataBando("DataInizioValidità"))
            strdataInizio = strAnnoInizio & strMeseInizio & strGiornoInizio

            'carico la data inversa di fine bando  rtsDataBando("DataFineValidità")
            strGiornoFine = IIf(Len(CStr(Day(rtsDataBando("DataFineValidità")))) < 2, "0" & Day(rtsDataBando("DataFineValidità")), Day(rtsDataBando("DataFineValidità")))
            strMeseFine = IIf(Len(CStr(Month(rtsDataBando("DataFineValidità")))) < 2, "0" & Month(rtsDataBando("DataFineValidità")), Month(rtsDataBando("DataFineValidità")))
            strAnnoFine = Year(rtsDataBando("DataFineValidità"))
            strDataFine = strAnnoFine & strMeseFine & strGiornoFine

            strDataAttuale = Year(rtsDataBando("DATAODIERNA")) & IIf(Len(CStr(Month(rtsDataBando("DATAODIERNA")))) < 2, "0" & Month(rtsDataBando("DATAODIERNA")), Month(rtsDataBando("DATAODIERNA"))) & IIf(Len(CStr(Day(rtsDataBando("DATAODIERNA")))) < 2, "0" & Day(rtsDataBando("DATAODIERNA")), Day(rtsDataBando("DATAODIERNA")))
        End If

        If Not rtsDataBando Is Nothing Then
            rtsDataBando.Close()
            rtsDataBando = Nothing
        End If


        If (CInt(strDataAttuale) >= CInt(strdataInizio) And CInt(strDataAttuale) <= CInt(strDataFine)) Then
            'se la data attuale è compresa imposto la funzione a true
            controlladata = True
        Else
            'altrimenti a false
            controlladata = False
        End If
        '    'variabili che utilizzo per formattare la data di inizio bando
        '    'sulla quale farò i controlli per cui visualizzare o meno 
        '    'il pulsante di Presentazione di Istanza
        '    Dim strGiornoInizio As String
        '    Dim strMeseInizio As String
        '    Dim strAnnoInizio As String
        '    Dim strdataInizio As String

        '    'variabili che utilizzo per formattare la data di fine bando
        '    'sulla quale farò i controlli per cui visualizzare o meno 
        '    'il pulsante di Presentazione di Istanza
        '    Dim strGiornoFine As String
        '    Dim strMeseFine As String
        '    Dim strAnnoFine As String
        '    Dim strDataFine As String

        '    'carico la data inversa di inizio
        '    strGiornoInizio = IIf(Len(CStr(Day(Request.QueryString("DataInizio")))) < 2, "0" & Day(Request.QueryString("DataInizio")), Day(Request.QueryString("DataInizio")))
        '    strMeseInizio = IIf(Len(CStr(Month(Request.QueryString("DataInizio")))) < 2, "0" & Month(Request.QueryString("DataInizio")), Month(Request.QueryString("DataInizio")))
        '    strAnnoInizio = Year(Request.QueryString("DataInizio"))
        '    strdataInizio = strAnnoInizio & strMeseInizio & strGiornoInizio

        '    'carico la data inversa di fine bando
        '    strGiornoFine = IIf(Len(CStr(Day(Request.QueryString("DataFine")))) < 2, "0" & Day(Request.QueryString("DataFine")), Day(Request.QueryString("DataFine")))
        '    strMeseFine = IIf(Len(CStr(Month(Request.QueryString("DataFine")))) < 2, "0" & Month(Request.QueryString("DataFine")), Month(Request.QueryString("DataFine")))
        '    strAnnoFine = Year(Request.QueryString("DataFine"))
        '    strDataFine = strAnnoFine & strMeseFine & strGiornoFine


        '    Dim strsql As String
        '    Dim dtrgenerico As SqlClient.SqlDataReader
        '    Dim strDataAttuale As String

        '    If Not dtrgenerico Is Nothing Then
        '        dtrgenerico.Close()
        '        dtrgenerico = Nothing
        '    End If
        '    strsql = "select GetDate() as DataAttuale"
        '    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        '    If dtrgenerico.HasRows = True Then
        '        dtrgenerico.Read()
        '        strDataAttuale = Year(dtrgenerico("DataAttuale")) & IIf(Len(CStr(Month(dtrgenerico("DataAttuale")))) < 2, "0" & Month(dtrgenerico("DataAttuale")), Month(dtrgenerico("DataAttuale"))) & IIf(Len(CStr(Day(dtrgenerico("DataAttuale")))) < 2, "0" & Day(dtrgenerico("DataAttuale")), Day(dtrgenerico("DataAttuale")))
        '    End If
        '    If Not dtrgenerico Is Nothing Then
        '        dtrgenerico.Close()
        '        dtrgenerico = Nothing
        '    End If

        '    If (CInt(strDataAttuale) >= CInt(strdataInizio) And CInt(strDataAttuale) <= CInt(strDataFine)) Then
        '        'se la data attuale è compresa imposto la funzione a true
        '        controlladata = True
        '    Else
        '        'altrimenti a false
        '        controlladata = False
        '    End If
        '    Return controlladata
    End Function

    Private Sub bloccaCheck()
        'Generato da Alessandra Taballione il 02/02/2005
        Dim item As DataGridItem
        For Each item In Dgtattivita.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chk"), CheckBox)
            check.Enabled = False
        Next
        chkSelDesel.Visible = False
        chkSelDesel2.Visible = False
        'lblseltutto.Visible = False
        'ImgSelezionaTutto.Visible = False
        'lblseltutto2.Visible = False
        'ImgSelezionaTutto2.Visible = False
    End Sub
   
    Private Sub DgdBando_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DgdBando.SelectedIndexChanged
        CaricaGriglia()
    End Sub

    Private Sub VerificaCompetenze()
        'SE UNSC DEVO AVVERTIRE SE CI SONO PROGETTI DI COMPETENZA REGIONALE
        bCompetenza = False
        Dim item As DataGridItem
        For Each item In Dgtattivita.Items
            If UCase(item.Cells(11).Text) <> "NAZIONALE" Then
                bCompetenza = True
                Exit For
            End If
        Next
    End Sub

    Private Sub PreparaInserimento2()
        Dim mydataset As DataSet
        Dim strsql As String
        cmdannulla.Visible = False
        cmdmodifica.Visible = False
        cmdPresentaIstanza.Visible = False
        lblMessaggioPresenta.Visible = False
        lblstato.Text = "NESSUNO"
        'carico sempre il bando di competenza nazionale
        'AND (bando.Riferimento = '1')" & _
        strsql = "SELECT distinct bando.IDBando, bando.Bando, AssociaBandoRegioniCompetenze.IdRegioneCompetenza " & _
                 "FROM bando INNER JOIN AssociaBandoRegioniCompetenze ON bando.IDBando = AssociaBandoRegioniCompetenze.IdBando INNER JOIN " & _
                 " RegioniCompetenze ON AssociaBandoRegioniCompetenze.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                 " WHERE     (AssociaBandoRegioniCompetenze.IdRegioneCompetenza = 22) " & _
                 " AND bando.idbando <> all (SELECT IDBANDO FROM BANDIATTIVITà a inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività WHERE IDENTE =" & Session("idente") & " and b.cancellata<>1) " & _
                 " AND bando.DataInizioValidità <= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)" & _
                 " AND bando.DataFineValidità >= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)"

        'seleziono bandi non collegati con attività per ente loggato
        strsql = strsql & " UNION select distinct a.idbando,a.bando,RegioniCompetenze.IdRegioneCompetenza  from bando a" & _
        " INNER JOIN AssociabandoTipiProgetto ab on a.idbando=ab.idbando " & _
        " INNER JOIN TipiProgetto ON ab.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
        " INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
        " INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "

        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strsql = strsql & " INNER JOIN STATIBANDO b on a.idstatobando=b.idstatobando"
        'AGGIUNTA JOIN PER RegioniCompetenze E AssociaBandoRegioniCompetenze
        strsql = strsql & " INNER JOIN AssociaBandoRegioniCompetenze ON a.IDBando = AssociaBandoRegioniCompetenze.IdBando" & _
        " INNER JOIN RegioniCompetenze ON AssociaBandoRegioniCompetenze.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
        '---------------------------------------------------------------------------------------------------------------------------
        strsql = strsql & " WHERE B.INVALUTAZIONE = 1" & _
        " AND a.DataInizioValidità <= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)" & _
        " AND a.DataFineValidità >= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)" & _
        " AND a.idbando <> all (SELECT IDBANDO FROM BANDIATTIVITà a inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività WHERE IDENTE =" & Session("idente") & " and b.cancellata<>1)" & _
        " and AssociaUtenteGruppo.Username='" & Session("Utente") & "'  "
        'AGGIUNTO FILTRO PER IdRegioneCompetenza SU VISTA CHE RACCOGLIE TUTTI GLI ENTI OPERATIVI E ACCREDITATI
        strsql = strsql & " AND  (RegioniCompetenze.IdRegioneCompetenza IN " & _
        " (SELECT idregionecompetenza FROM VW_ELENCO_ENTI_ACCREDITATI WHERE IDENTE = " & Session("idente") & "))"
        '---------------------------------------------------------------------------------------------------------------------------
        If Session("TipoUtente") = "E" Then
            strsql = strsql & " and isnull(a.enteabilitato,1) = 1 "
        End If
        strsql = strsql & " order by 1"
        ''''seleziono bandi non collegati con attività per ente loggato

        '''strsql = "SELECT distinct bando.IDBando, bando.Bando, AssociaBandoRegioniCompetenze.IdRegioneCompetenza " & _
        '''            "FROM bando INNER JOIN AssociaBandoRegioniCompetenze ON bando.IDBando = AssociaBandoRegioniCompetenze.IdBando INNER JOIN " & _
        '''            " RegioniCompetenze ON AssociaBandoRegioniCompetenze.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
        '''            " WHERE     (AssociaBandoRegioniCompetenze.IdRegioneCompetenza = 22) AND (bando.Riferimento = '1')"
        ''''seleziono bandi non collegati con attività per ente loggato
        '''strsql = strsql & " UNION select distinct a.idbando,a.bando,RegioniCompetenze.IdRegioneCompetenza  from bando a" & _
        '''" INNER JOIN AssociabandoTipiProgetto ab on a.idbando=ab.idbando " & _
        '''" INNER JOIN TipiProgetto ON ab.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
        '''" INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
        '''" INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo " & _
        '''" INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo " & _
        '''" INNER JOIN STATIBANDO b on a.idstatobando=b.idstatobando"
        ''''AGGIUNTA JOIN PER RegioniCompetenze E AssociaBandoRegioniCompetenze
        '''strsql = strsql & " INNER JOIN AssociaBandoRegioniCompetenze ON a.IDBando = AssociaBandoRegioniCompetenze.IdBando" & _
        '''" INNER JOIN RegioniCompetenze ON AssociaBandoRegioniCompetenze.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
        ''''---------------------------------------------------------------------------------------------------------------------------
        '''strsql = strsql & " WHERE B.INVALUTAZIONE = 1" & _
        '''" AND a.DataInizioValidità <= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)" & _
        '''" AND a.DataFineValidità >= Convert(Datetime,'" & Date.Today.ToString.Substring(0, 10) & "',103)" & _
        '''" AND a.idbando <> all (SELECT IDBANDO FROM BANDIATTIVITà a inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività WHERE IDENTE =" & Session("idente") & " and b.cancellata<>1)" & _
        '''" and AssociaUtenteGruppo.Username='" & Session("Utente") & "'  "
        ''''AGGIUNTO FILTRO PER IdRegioneCompetenza SU VISTA CHE RACCOGLIE TUTTI GLI ENTI OPERATIVI E ACCREDITATI
        '''strsql = strsql & " AND  (RegioniCompetenze.IdRegioneCompetenza IN " & _
        '''" (SELECT idregionecompetenza FROM VW_ELENCO_ENTI_ACCREDITATI WHERE IDENTE = " & Session("idente") & "))"
        ''''---------------------------------------------------------------------------------------------------------------------------
        '''If Session("TipoUtente") = "E" Then
        '''    strsql = strsql & " and isnull(a.enteabilitato,1) = 1 "
        '''End If
        '''strsql = strsql & " order by 1"
        mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
        DgdBando.DataSource = mydataset
        DgdBando.DataBind() 'valorizzo griglia bando


        If DgdBando.Items.Count = 0 Then
            lblMessaggio.Text = ""
            cmdInserisci.Visible = False
            DgdBando.Visible = False
            lblbando.Visible = True
        End If

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Call VerificaCompetenze()
        End If

        lblprogetto.Visible = True
        lblprogetto.Text = "Selezionare una circolare presentazione per visualizzare i progetti."
        Dgtattivita.Visible = False

    End Sub

    Private Function VerificaTutor(ByVal Idbando As Integer) As Boolean
        'Aggiunto da Simona Cordella il 16/11/2009
        'Controllo se il bando prevede la presentza dei TUTOR
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "select visualizzatutor from bando  " & _
        " where idbando=" & Idbando & ""

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        VerificaTutor = dtrgenerico("visualizzatutor")

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return VerificaTutor
    End Function

    Private Sub Dgtattivita_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles Dgtattivita.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Dim info As String
                Dim idattivita As Integer
                info = "Info"
                idattivita = e.Item.Cells(0).Text
                OpenWindow(info, idattivita)
                

            Case "Documenti"

                Response.Redirect("wfrmDocumentiProgetto.aspx?VengoDa=Istanza&IdAttivita=" & e.Item.Cells(0).Text & " &id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))

            Case "Applica"

                Response.Redirect("wfrmDocumentiProgetto_Applica.aspx?VengoDa=Istanza&IdAttivita=" & e.Item.Cells(0).Text & " &id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))
                
        End Select
    End Sub

    Private Function VisualizzaStampePerNazioneBase(ByVal IdBandoAttivita As Integer)
        '** Aggiunto il 26/01/2010 da Simona Cordella
        '** Verifico se l'idbadoattività ci sono progetti Italia e Estero
        '** Per i progetti Italia abilito BOX 16
        '** Per gli eseteri abilito BOX19
        '**  se ci sono entrambi abilito le stampe dei BOX 16 E 19

        Dim strSql As String
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strSql = " SELECT "
        strSql = strSql & " (SELECT count(TipiProgetto.NazioneBase) "
        strSql = strSql & "     FROM attività"
        strSql = strSql & "     INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività "
        strSql = strSql & "     INNER JOIN TipiProgetto ON attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto"
        strSql = strSql & "     WHERE TipiProgetto.NazioneBase = 1 and (BandiAttività.IdBandoAttività = " & IdBandoAttivita & "))as Italia,"
        strSql = strSql & " ( SELECT count(TipiProgetto.NazioneBase) "
        strSql = strSql & "     FROM attività "
        strSql = strSql & "     INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività "
        strSql = strSql & "     INNER JOIN TipiProgetto ON attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto"
        strSql = strSql & "     WHERE TipiProgetto.NazioneBase = 0 and (BandiAttività.IdBandoAttività = " & IdBandoAttivita & ") )  as Estero"

        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()

            If dtrgenerico("Italia") <> 0 Then
                Session("Sap") = True

            End If
            If dtrgenerico("Estero") <> 0 Then
                Session("SapEst") = True

            End If

        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Private Function ControllaCompletamentoRLEA(ByVal IDBandoAttivita As Integer) As String
        'Creata da Simona Cordella  il 16/02/2010
        'VERIFICA CHE L'RLEA SIA STATO INSERITO PER TUTTE LE SEDI NECESSARIE
        Dim intValore As Integer

        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_VERIFICA_COMPLETAMENTO_RLEA"
        'CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
        CustOrderHist.Connection = Session("Conn")

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IDBANDOATTIVITA"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)

        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@Esito"
        sparam1.SqlDbType = SqlDbType.VarChar
        sparam1.Size = 1000
        sparam1.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam1)

        Dim Reader As SqlClient.SqlDataReader
        CustOrderHist.Parameters("@IDBANDOATTIVITA").Value = IDBandoAttivita

        'Reader = CustOrderHist.ExecuteReader()
        CustOrderHist.ExecuteScalar()
        ' Insert code to read through the datareader.

        Return CustOrderHist.Parameters("@Esito").Value

        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If


    End Function

    Private Sub RegistraStampaAvvenuta()
        Dim cmdinsert As Data.SqlClient.SqlCommand
        ' CInt(txtidbandoAttivita.Text)
        'Funzione che tiene traccia delle stampe avvenute 
        Dim strSqlCommand As String = " INSERT INTO BandiAttivitàStampe " & vbCrLf
        strSqlCommand &= " VALUES ("
        strSqlCommand &= txtidbandoAttivita.Text & ","
        strSqlCommand &= "GETDATE(),"
        strSqlCommand &= "'" & ClsServer.NoApice(Session("utente")) & "',"
        strSqlCommand &= "1,"
        If Not IsNothing(Session("Sap")) Then
            If Session("Sap") = True Then
                strSqlCommand &= "1,"
            Else
                strSqlCommand &= "0,"
            End If
        Else
            strSqlCommand &= "0,"
        End If
        If Not IsNothing(Session("SapEst")) Then
            If Session("SapEst") = True Then
                strSqlCommand &= "1"
            Else
                strSqlCommand &= "0"
            End If
        Else
            strSqlCommand &= "0"
        End If
        strSqlCommand &= ")"

        Try
            cmdinsert = New Data.SqlClient.SqlCommand(strSqlCommand, Session("conn"))
            cmdinsert.ExecuteNonQuery()
            cmdinsert.Dispose()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub GestioneFascicolo(ByVal TipoUtente As String, ByVal idbando As String)
        'creato da simona cordella il 17/02/2011 
        'abilito la visualizzazione del fascicolo solo se sono un utente UNSC (bandiattività)
        'modificata da simona cordella il 23/05/2012 
        'i fascicoli vengono presi dalla tabella bandiattivitàfascicoli 
        If TipoUtente = "U" Then
            strsql = "SELECT CodiceFascicolopc, IDFascicolopc, DescrFascicolopc " & _
                     "FROM bandiattivitàfascicoli " & _
                     "WHERE idbando = '" & idbando & "' and idente = " & CInt(Session("IdEnte"))
            Try
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    TxtCodiceFascicolo.Text = "" & dtrgenerico("CodiceFascicolopc")
                    TxtIdFascicolo.Text = "" & dtrgenerico("IDFascicolopc")
                    txtDescFasc.Text = "" & dtrgenerico("DescrFascicolopc")
                End If
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                '*******************************************************************************
            Catch

            End Try
        Else
            LblNumFascicolo.Visible = False
            LblDescrFascicolo.Visible = False
            TxtCodiceFascicolo.Visible = False
            cmdSelFascicolo.Visible = False
            cmdSelProtocollo.Visible = False
            cmdFascCanc.Visible = False
            txtDescFasc.Visible = False
            cmdSalva.Visible = False
        End If
    End Sub

    Private Sub SalvaFascicolo()

        Dim strLocal As String
        Dim dtrCancellazione As SqlClient.SqlDataReader
        Dim mycommand As New SqlClient.SqlCommand
        Dim mydatatable As New DataTable

        mycommand.Connection = Session("conn")
        If (TxtCodiceFascicolo.Text = "" Or hddRicordaFascicolo.Value <> TxtCodiceFascicolo.Text) Then
            'cancella
            strLocal = "select IdBando FROM BandiAttività WHERE IdBandoAttività = " & Request.QueryString("idBA")
            Try
                mydatatable = ClsServer.CreaDataTable(strLocal, False, Session("conn"))

                Dim k As Int16

                For k = 0 To mydatatable.Rows.Count - 1
                    strLocal = "update cronologiaentidocumenti set dataprot =null, nprot = null where idente = " & CInt(Session("IdEnte")) & _
                                " and tipodocumento = 1 and idbando = " & mydatatable.Rows(k).Item("idbando")
                    mycommand.CommandText = strLocal
                    mycommand.ExecuteNonQuery()
                Next
                '        '*******************************************************************************
            Catch ex As Exception
                'Response.Write(ex.Message.ToString())
            End Try
        End If

        strLocal = " Update bandiattivitàfascicoli  SET CodiceFascicoloPc ='" & TxtCodiceFascicolo.Text & "', IdFascicoloPc='" & TxtIdFascicolo.Text & "', " & _
                   " DescrFascicoloPc='" & txtDescFasc.Text & "' WHERE IdBando = " & mydatatable.Rows(0).Item("idbando") & " and idente =" & CInt(Session("IdEnte")) & ""
        mycommand.CommandText = strLocal
        mycommand.ExecuteNonQuery()

    End Sub

    Private Sub cmdFascCanc_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCanc.Click
        TxtCodiceFascicolo.Text = ""
        TxtIdFascicolo.Text = ""
        txtDescFasc.Text = ""
        SalvaFascicolo()
    End Sub

    Private Sub CaricaProtocolli(ByVal IdBandoAttivita As String)
        Dim strSql As String
        Dim dataSet As New DataSet

        strSql = " SELECT  IdBandoAttività,NProt, dbo.FormatoData(DataProt) AS DataProt " & _
                 " FROM VW_ProtocolliIstanze where IdBandoAttività ='" & IdBandoAttivita & " '"

        dataSet = ClsServer.DataSetGenerico(strSql, Session("Conn"))
        dtgElencoProt.DataSource = dataSet
        dtgElencoProt.DataBind()

    End Sub

    Private Sub dtgElencoProt_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgElencoProt.ItemCommand
        'agg. il 15/05/2012 da s.c.
        Dim strMsgReturn As String = ""
        If e.CommandName = "Cancella" Then

            strMsgReturn = DisassociaProtocolloIstanza(Request.QueryString("idBA"), e.Item.Cells(1).Text, e.Item.Cells(2).Text, Session("Utente"))
            dtgElencoProt.CurrentPageIndex = 0
            CaricaProtocolli(Request.QueryString("IdAttivita"))

            If strMsgReturn <> "" Then
                If strMsgReturn = "OK" Then
                    lblMessaggio.Text = "Aggiornamento effettuato con successo."
                    CaricaProtocolli(Request.QueryString("idBA"))
                Else
                    lblMessaggio.Text = strMsgReturn
                End If
            End If
        End If
    End Sub

    Private Function AssociaProtocolloIstanza(ByVal IdBandoAttivita As Integer, ByVal NProt As String, ByVal DataProt As String, ByVal Username As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_AssociaProtocolloIstanza]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IdBandoAttivita", SqlDbType.Int).Value = IdBandoAttivita
            sqlCMD.Parameters.Add("@NProt", SqlDbType.VarChar).Value = NProt
            sqlCMD.Parameters.Add("@DataProt", SqlDbType.DateTime).Value = DataProt
            sqlCMD.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Esito").Value
            Return str
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Function DisassociaProtocolloIstanza(ByVal IdBandoAttivita As Integer, ByVal NProt As String, ByVal DataProt As String, ByVal Username As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[Sp_DisassociaProtocolloIstanza]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IdBandoAttivita", SqlDbType.Int).Value = IdBandoAttivita
            sqlCMD.Parameters.Add("@NProt", SqlDbType.VarChar).Value = NProt
            sqlCMD.Parameters.Add("@DataProt", SqlDbType.DateTime).Value = DataProt
            sqlCMD.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Esito").Value
            Return str
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Sub dtgElencoProt_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgElencoProt.PageIndexChanged
        dtgElencoProt.CurrentPageIndex = e.NewPageIndex
        CaricaProtocolli(Request.QueryString("idBA"))
        dtgElencoProt.SelectedIndex = -1
    End Sub

    Private Sub imgEsporta_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEsporta.Click
        Esportazione()
    End Sub

    Sub Esportazione()

        Dim i As Integer
        Dim arrParam(0) As SqlParameter
        Dim NomeFile As String

        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@IdBandoAttivita"
        arrParam(0).SqlDbType = SqlDbType.Int
        arrParam(0).Value = Request.QueryString("idBA")

        MyDataTable = New DataTable("DocumentiIstanza")
        MyDataTable = ExecuteDataTable("SP_EsportaDocumenti_Istanza", arrParam)
        MyDataSet.Tables.Add(MyDataTable)
        OutputXls(MyDataTable, "DocumentiIstanza", NomeFile)

        hlDw.NavigateUrl = "download" & "\" + NomeFile
        hlDw.Target = "_blank"

        hlDw.Visible = True
        imgEsporta.Visible = False
        lblperImgEsxport.Visible = False

    End Sub

    Public Function ExecuteDataTable(ByVal storedProcedureName As String, ByVal ParamArray arrParam() As SqlParameter) As DataTable
        Dim dt As DataTable

        ' Define the command 
        Dim cmd As New SqlCommand
        cmd.Connection = Session("Conn")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = storedProcedureName

        ' Handle the parameters 
        If Not arrParam.Length = 0 Then
            For Each param As SqlParameter In arrParam
                cmd.Parameters.Add(param)
            Next
        End If

        ' Define the data adapter and fill the dataset 
        Dim da As New SqlDataAdapter(cmd)
        dt = New DataTable
        da.Fill(dt)

        Return dt
    End Function

    Private Function OutputXls(ByVal Datasource As DataTable, ByVal Tipofile As String, ByRef NomeFile As String) As Boolean

        NomeFile = Session("Utente") & "_" & Tipofile & "_" & Format(DateTime.Now, "ddMMyyyyhhmmss") & "_" & ".csv"

        Dim stringWrite As System.IO.StringWriter = New System.IO.StringWriter

        If File.Exists(Server.MapPath("download") & "\" & NomeFile) Then
            File.Delete((Server.MapPath("download") & "\" & NomeFile))
        End If
        SaveTextToFile(MyDataTable, NomeFile)
        Return True
    End Function

    Function SaveTextToFile(ByVal DTBRicerca As DataTable, ByVal NomeFile As String)

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String

        'xPrefissoNome = Session("Utente")
        NomeUnivoco = NomeFile
        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco)
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        Writer.WriteLine(xLinea)
        xLinea = vbNullString
        'If DTBRicerca.Rows.Count = 0 Then
        '    'lblErr.Text = lblErr.Text & "La ricerca non ha prodotto nessun risultato."

        'Else


        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To DTBRicerca.Rows.Count - 1
            For j = 0 To intNumCol - 1
                If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next
            Writer.WriteLine(xLinea)
            xLinea = vbNullString
        Next
        ' End If
        Writer.Close()
        Writer = Nothing
    End Function

    Private Function GenerazioneBOX16_BOX19(ByVal IdBandoAttivita As Integer)

        'Dim localWS As New WS_Editor.WSMetodiDocumentazione
        Dim ds As DataSet
        Dim i As Integer
        Dim strCodiceProgetto As String
        Dim GeneraBOX As clsGeneraBox

        ''richiamo WSDocumentazione
        'localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")
        'localWS.Timeout = 1000000

        strsql = "Select idAttività,CodiceEnte,idtipoProgetto From Attività where IdBandoAttività = " & IdBandoAttivita
        ds = ClsServer.DataSetGenerico(strsql, Session("conn"))
        If ds.Tables(0).Rows.Count > 0 Then
            Dim NomeReport As String
            Dim strNomeFile As String
            For i = 0 To ds.Tables(0).Rows.Count - 1

                If ds.Tables(0).Rows.Item(i).Item("idtipoProgetto") = 2 Then 'estero
                    NomeReport = "crpElencoSAPEsteroProgetto.rpt"
                    strNomeFile = "Box19_"
                    strCodiceProgetto = ds.Tables(0).Rows.Item(i).Item("CodiceEnte")

                    'DecodeFile(localWS.getBox(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("utente"), NomeReport), Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                    Dim strFile As String
                    strFile = GeneraFile(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("utente"), NomeReport)  ',

                    DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                    clsGestioneDocumenti.CaricaDocumentoProgettoBOX(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("Utente"), strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), Session("conn"))

                    NomeReport = "crpElencoSAPEstero20Progetto.rpt"
                    strNomeFile = "Box20_"
                    strCodiceProgetto = ds.Tables(0).Rows.Item(i).Item("CodiceEnte")

                    'DecodeFile(localWS.getBox(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("utente"), NomeReport), Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))

                    strFile = GeneraFile(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("utente"), NomeReport)  ',

                    DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                    clsGestioneDocumenti.CaricaDocumentoProgettoBOX(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("Utente"), strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), Session("conn"))
                Else
                    NomeReport = "crpElencoSAPItaliaProgetto.rpt"
                    strNomeFile = "Box16_"
                    strCodiceProgetto = ds.Tables(0).Rows.Item(i).Item("CodiceEnte")

                    'DecodeFile(localWS.getBox(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("utente"), NomeReport), Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                    Dim strFile As String
                    strFile = GeneraFile(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("utente"), NomeReport)  ',

                    DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                    clsGestioneDocumenti.CaricaDocumentoProgettoBOX(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("Utente"), strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), Session("conn"))


                End If


                'strCodiceProgetto = ds.Tables(0).Rows.Item(i).Item("CodiceEnte")

                ''DecodeFile(localWS.getBox(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("utente"), NomeReport), Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                'Dim strFile As String
                'strFile = GeneraFile(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("utente"), NomeReport)  ',

                'DecodeFile(strFile, Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"))
                'clsGestioneDocumenti.CaricaDocumentoProgettoBOX(ds.Tables(0).Rows.Item(i).Item("idAttività"), Session("Utente"), strNomeFile & strCodiceProgetto & ".pdf", Server.MapPath("BOX/" & strNomeFile & strCodiceProgetto & ".pdf"), Session("conn"))
            Next
        End If
    End Function

    Private Sub GenerazioneBOX16_BOX19_Da_WSDocumentazione(ByVal IdBandoAttivita As Integer)

        Dim localWS As New WS_Editor.WSMetodiDocumentazione
        Dim ds As DataSet
        Dim i As Integer
        Dim strCodiceProgetto As String
        Dim ResultAsinc As IAsyncResult

        Dim cmdUp As SqlCommand
        'update InLavorazione a 1 
        'aggiunto flag LAVORAZIONE =1 da simona cordella il 16/10/2012
        cmdUp = New Data.SqlClient.SqlCommand(" UPDATE bandiattività" & _
                                              " SET InLavorazione= 1  " & _
                                              " WHERE idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdUp.ExecuteNonQuery()
        cmdUp.Dispose()

        'richiamo WSDocumentazione
        localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")
        localWS.Timeout = 1000000


        'localWS.GenerazioneBOX16_BOX19Async(IdBandoAttivita, Session("Utente"))


        ResultAsinc = localWS.BeginGenerazioneBOX16_BOX19(IdBandoAttivita, Session("Utente"), Nothing, "")


        'ResultAsinc = localWS.GenerazioneBOX16_BOX19(IdBandoAttivita, Session("Utente"))

        ' Commentata e messa quella sopra per farlo funzionare 26/05/2015
        'ResultAsinc = localWS.BeginGenerazioneBOX16_BOX19(IdBandoAttivita, Session("Utente"), Nothing, "")
    End Sub

    Function DecodeToByte(ByVal enc As String) As Byte()
        Dim bt() As Byte
        bt = System.Convert.FromBase64String(enc)
        Return bt
    End Function

    Sub DecodeFile(ByVal srcFile As String, ByVal destFile As String)
        'Dim src As String
        'Dim sr As New IO.StreamReader(srcFile)
        'src = sr.ReadToEnd
        'sr.Close()
        Dim bt64 As Byte() = DecodeToByte(srcFile)
        If IO.File.Exists(destFile) Then
            IO.File.Delete(destFile)
        End If

        Dim sw As New IO.FileStream(destFile, IO.FileMode.CreateNew)
        sw.Write(bt64, 0, bt64.Length)
        sw.Close()
    End Sub

    Private Function FileToBase64(ByVal fileName As String) As String
        Dim bFile() As Byte
        Dim fs As FileStream
        Dim _textB64 As String
        Try
            fs = New FileStream(Server.MapPath(fileName), FileMode.Open)
            ReDim bFile(fs.Length - 1)
            fs.Read(bFile, 0, fs.Length)
            _textB64 = Convert.ToBase64String(bFile)
        Catch ex As Exception
            'gestione eccezione 
            Return "ERRORE"
        Finally
            fs.Close()
        End Try
        Return _textB64
    End Function

    Private Function GeneraFile(ByVal idAttivita As Integer, ByVal username As String, ByVal NomeReport As String) As String
        Dim sDati As String
        Dim strEsito As String

        sDati = "IdAttivita," & idAttivita & ":"

        Try
            strEsito = CreatePdf(NomeReport, sDati, username)

            If strEsito = "ERRORE" Then
                Return strEsito
            Else
                Return FileToBase64(strEsito)
            End If

        Catch ex As Exception
            Return ex.Message '"ERRORE"
        End Try

    End Function

    Function CreatePdf(ByVal NomeReport As String, ByVal StrDati As String, ByVal strUserName As String, Optional ByVal SottoReport As String = "", Optional ByVal ReportStorico As Int16 = 1) As String
        '*************************************************************************************************
        'DESCRIZIONE: Genera il PDF nella directory Reports/Export del report selezionato
        'AUTORE: TESTA GUIDO    DATA: 04/10/2004
        '*************************************************************************************************
        Dim paramFieldDt As New ParameterField
        Dim discreteValDt As New ParameterDiscreteValue
        Dim myPath As New System.Web.UI.Page
        Dim crReportDocument As New ReportDocument
        Dim logOnInfo As New TableLogOnInfo
        Dim NameReportNew As String
        Dim i As Integer
        Dim sGruppo() As String         'matrice parametri/valori
        Dim sGruppo1() As String        'matrice sottoreport
        Dim sElemt() As String
        Dim GetPdfError As String
        Dim dbg As Char = ConfigurationSettings.AppSettings("debugApp")

        GetPdfError = ""

        NameReportNew = UCase(strUserName) & "-" & Format(Now, "dd-MM-yyyyhh-mm-ss")

        Try

            crReportDocument.Load(myPath.Server.MapPath("Reports\" & NomeReport))

            sGruppo = Split(StrDati, ":")
            '1)parametri report*****************************
            For i = 0 To UBound(sGruppo) - 1
                sElemt = Split(sGruppo(i), ",")
                paramFieldDt.ParameterFieldName = "@" & sElemt(0)       'nome campo
                discreteValDt.Value = sElemt(1)                           'valore campo
                paramFieldDt.CurrentValues.Add(discreteValDt)

                Dim paramFieldDefDt As ParameterFieldDefinition = crReportDocument.DataDefinition.ParameterFields.Item(sElemt(0))

                Dim ParameterValuesDt As ParameterValues = paramFieldDt.CurrentValues
                paramFieldDefDt.ApplyCurrentValues(ParameterValuesDt)
            Next i
            '******************************************
            If ReportStorico = 1 Then
                With logOnInfo
                    .ConnectionInfo.Password = ConfigurationSettings.AppSettings("connectionPassword")
                    .ConnectionInfo.ServerName = ConfigurationSettings.AppSettings("connectionServerName")
                    .ConnectionInfo.DatabaseName = ConfigurationSettings.AppSettings("PDFConnectionDatabaseNameStorico")
                    .ConnectionInfo.UserID = ConfigurationSettings.AppSettings("connectionUserid")
                End With
            Else
                With logOnInfo
                    .ConnectionInfo.Password = ConfigurationSettings.AppSettings("connectionPassword")
                    .ConnectionInfo.ServerName = ConfigurationSettings.AppSettings("connectionServerName")
                    .ConnectionInfo.DatabaseName = ConfigurationSettings.AppSettings("PDFConnectionDatabaseName")
                    .ConnectionInfo.UserID = ConfigurationSettings.AppSettings("connectionUserid")
                End With
            End If


            crReportDocument.Database.Tables(0).ApplyLogOnInfo(logOnInfo)
            '******************************************

            '3)gestione sotto report*********************
            sGruppo1 = Split(SottoReport, ":")
            i = 0
            For i = 0 To UBound(sGruppo1) - 1
                crReportDocument.OpenSubreport(sGruppo1(i)).Database.Tables(0).ApplyLogOnInfo(logOnInfo)
            Next i
            '******************************************

            '4)esportazione report in PDF***************
            Dim crDiskFileDestinationOptions As New CrystalDecisions.Shared.DiskFileDestinationOptions
            Dim crExportOptions As CrystalDecisions.Shared.ExportOptions


            crDiskFileDestinationOptions.DiskFileName = myPath.Server.MapPath("download/" & NameReportNew & ".pdf")
            crExportOptions = crReportDocument.ExportOptions
            crExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
            crExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
            crExportOptions.DestinationOptions = crDiskFileDestinationOptions

            crReportDocument.Export()
            crReportDocument.Close()

            Return "download/" & NameReportNew & ".pdf"

            '******************************************

        Catch ex As Exception
            GetPdfError = ex.Message '"ERRORE" '
        End Try

    End Function

    Private Function StatoDocumenti(ByVal idEnte As Integer) As Boolean
        'agg. da simoan cordella il 12/12/2012
        Dim strSql As String
        Dim dtrCount As SqlClient.SqlDataReader
        Dim blnReturn As Boolean
        strSql = " SELECT  NElaborazioniMancanti FROM LockDocumentiEnte l " & _
                 " WHERE IdEnte = " & idEnte
        dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))

        blnReturn = dtrCount.HasRows
        If Not dtrCount Is Nothing Then
            dtrCount.Close()
            dtrCount = Nothing
        End If

        Return blnReturn
    End Function

    Private Function ReturnRegioneCompetenzaBando(ByVal IdBA As Integer) As String
        Dim strSql As String
        Dim dtrReg As SqlClient.SqlDataReader
        Dim IdRegCompetenza As String = "0"
        strSql = "select abr.Idregionecompetenza from statibandiattività sba " & _
                " inner join bandiattività ba on ba.idstatobandoattività=sba.idstatobandoattività " & _
                " inner join bando b on b.idbando=ba.idbando " & _
                " inner join AssociaBandoRegioniCompetenze abr on abr.Idbando = b.idbando " & _
                " where idbandoattività=" & IdBA & ""
        dtrReg = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrReg.HasRows = True Then
            dtrReg.Read()
            IdRegCompetenza = dtrReg("Idregionecompetenza")
        End If
        If Not dtrReg Is Nothing Then
            dtrReg.Close()
            dtrReg = Nothing
        End If

        Return IdRegCompetenza
    End Function

    Private Sub CancellaBOX(ByVal IDBandoAttivita As Integer)
        Dim cmdDelete As SqlCommand
        cmdDelete = New Data.SqlClient.SqlCommand("Delete FROM AttivitàDocumenti  FROM AttivitàDocumenti  " & _
                        "INNER JOIN attività ON attività.IDAttività = AttivitàDocumenti.IdAttività  " & _
                        "INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività " & _
                        " WHERE  (LEFT(AttivitàDocumenti.FileName,3) = 'BOX') AND (BandiAttività.IdBandoAttività = " & IDBandoAttivita & ")", Session("conn"))
        cmdDelete.ExecuteNonQuery()
        cmdDelete.Dispose()
    End Sub

    Private Sub imgEsportaRiepilogo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEsportaRiepilogo.Click
        EsportazioneRiepilogoDocumenti()
    End Sub

    Sub EsportazioneRiepilogoDocumenti()

        Dim i As Integer
        Dim arrParam(0) As SqlParameter
        Dim NomeFile As String

        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@IdBandoAttivita"
        arrParam(0).SqlDbType = SqlDbType.Int
        arrParam(0).Value = Request.QueryString("idBA")

        MyDataTable = New DataTable("DocumentiIstanza")
        MyDataTable = ExecuteDataTable("SP_EsportaRiepilogoProgetti_TipoDocumento", arrParam)
        MyDataSet.Tables.Add(MyDataTable)
        OutputXls(MyDataTable, "RiepilogoDocumentiProgetto", NomeFile)

        hlDwRip.NavigateUrl = "download" & "\" + NomeFile
        hlDwRip.Target = "_blank"

        hlDwRip.Visible = True
        imgEsportaRiepilogo.Visible = False
        lblperImgEsxportRiepig.Visible = False
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        '***Generata da Gianluigi Paesani in data:13/07/04
        '***gestisco uscita wform a seconda del parametro
        Select Case Request.QueryString("Arrivo")
            Case Is = "WfrmRicIstanzadiPresentazione.aspx"
                Response.Redirect("" & Request.QueryString("Arrivo") & "")
            Case Is = "WfrmRicAccettazioneIstanzaUNSC.aspx"
                Response.Redirect("" & Request.QueryString("Arrivo") & "")
            Case Else
                Response.Redirect("WfrmMain.aspx")
        End Select

        'If Not Request.QueryString("Arrivo") Is Nothing Then
        '    Response.Redirect("" & Request.QueryString("Arrivo") & "")
        'Else
        '    Response.Redirect("WfrmMain.aspx")
        'End If
    End Sub

    Protected Sub cmdInserisci_Click(sender As Object, e As EventArgs) Handles cmdInserisci.Click
        '***Generata da Gianluigi Paesani in data:13/07/04
        '***Procedo con l'inserimento dell'istanza e modifico lo stato dell'attività
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtr As Data.SqlClient.SqlDataReader
        Dim bytverifica As Byte = 0
        Dim intIdbandoAttivita As Integer


        lblMessaggio.Text = ""

        If Not DgdBando.SelectedItem Is Nothing Then
            'Aggiunto da Alessandra Taballione il 06/05/2005
            'verifico se è stato selezionato almeno un progetto
            For i As Integer = 0 To Dgtattivita.Items.Count - 1
                'ciclo griglia check 
                Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(i).FindControl("chk")
                If chkoggetto.Visible = True Then
                    If chkoggetto.Checked = True Then
                        bytverifica = 1
                        Exit For
                    End If
                End If
            Next
            If bytverifica = 1 Then

                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    Call VerificaCompetenze()
                End If

                'inserisco la nuova istanza su BandiAttività
                cmdinsert = New Data.SqlClient.SqlCommand("insert into" & _
                " bandiattività(idstatobandoattività, idbando,idente," & _
                " UsernameInseritore, DataCreazioneRecord)" & _
                " select idstatobandoattività," & DgdBando.SelectedItem.Cells(1).Text & "," & Session("idente") & "," & _
                " '" & Session("Utente") & "',getdate()" & _
                " from statibandiattività where defaultstato=1", Session("conn"))
                cmdinsert.ExecuteNonQuery()
                cmdinsert.Dispose()

                'aggiunto il 28/10/2008 da jonsimo 
                strsql = "SELECT @@identity AS idbandoattività "
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                dtrgenerico.Read()
                intIdbandoAttivita = dtrgenerico("idbandoattività")
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If


                For i As Integer = 0 To Dgtattivita.Items.Count - 1
                    'ciclo griglia check 
                    Dim chkoggetto As CheckBox = Dgtattivita.Items.Item(i).FindControl("chk")
                    If chkoggetto.Checked = True Then
                        bytverifica = 1
                        'modifico prima la cronologia dell'attività
                        cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaAttività" & _
                        " (idattività,idstatoattività,datacronologia,idTipoCronologia," & _
                        " usernameaccreditatore)" & _
                        " select " & CInt(Dgtattivita.Items(i).Cells(0).Text) & "," & _
                        " idstatoattività,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'" & _
                        " from attività where idattività=" & Dgtattivita.Items(i).Cells(0).Text & "", Session("conn"))
                        cmdinsert.ExecuteNonQuery()
                        cmdinsert.Dispose()
                        'modifico l'attivita
                        'modificato il 28/10/2008 salvo l'id ricavato da @@identity
                        cmdinsert = New Data.SqlClient.SqlCommand("update attività" & _
                                " set idstatoattività=(select idstatoattività from statiattività" & _
                                " where Davalutare=1),idbandoattività= " & intIdbandoAttivita & "," & _
                                " dataultimostato=getdate()" & _
                                " where idattività=" & Dgtattivita.Items(i).Cells(0).Text & "", Session("conn"))
                        cmdinsert.ExecuteNonQuery()
                        cmdinsert.Dispose()
                        '" (select max(idbandoattività) from BandiAttività)," & _
                    End If
                Next
                lblMessaggio.Text = "E' stata creata l'istanza di presentazione per la Circolare presentazione progetti: " & DgdBando.SelectedItem.Cells(2).Text
                cmdInserisci.Visible = False
                Response.Redirect("WfrmIstanzaPresentazione.aspx?Verso=Mod" & "&id=" & DgdBando.SelectedItem.Cells(1).Text & "&idBA=" & intIdbandoAttivita & "&Stato=Registrata")
            Else
                lblMessaggio.Text = "Attenzione, selezionare un progetto prima di salvare"

            End If
            'If bytverifica = 1 Then 'caso istanza salvata correttamente
            '    lblMessaggio.Text = "E' stata creata l'istanza di presentazione per il bando: " & DgdBando.SelectedItem.Cells(2).Text
            '    cmdInserisci.Visible = False
            '    
            '    'dtr = ClsServer.CreaDatareader("select b.statobandoattività from bandiattività a" & _
            '    '" inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività" & _
            '    '" where a.idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
            '    'If dtr.HasRows = True Then
            '    '    Do While dtr.Read()
            '    '        lblstato.Text = dtr.GetValue(0)
            '    '    Loop
            '    'End If
            '    'dtr.Close()
            '    'dtr = Nothing
            'Else 'caso attività non ckeccata
            '    lblMessaggio.Text = "Attenzione, selezionare un progetto prima di salvare"
            '    
            'End If
        Else     'caso bando non selezionato

            lblMessaggio.Text = "Attenzione, e necessario selezionare una Circolare presentazione progetti prima di salvare"
        End If


    End Sub

    Protected Sub cmdmodifica_Click(sender As Object, e As EventArgs) Handles cmdmodifica.Click
        '***Generata da Gianluigi Paesani in data:16/07/04
        '***Questa routine richiama la procedura per la modifica
        ProceduraDiModifica()
    End Sub

    Protected Sub cmdannulla_Click(sender As Object, e As EventArgs) Handles cmdannulla.Click
        '***Generata da Gianluigi Paesani in data:16/07/04
        '***Questa routine annulla tutta l'istanza di presentazione inserita o modificata
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtr As SqlClient.SqlDataReader
        Dim booverifica As Boolean = False 'setto a zero parametri utenza

        lblMessaggio.Text = ""

        'inserisco in bandiattivitàrimossi
        cmdinsert = New Data.SqlClient.SqlCommand("insert into bandiattivitàrimossi" & _
        " (idente,idbando,idattività,UserName,DataRimozioneRecord)" & _
        " select " & Session("idente") & "," & Request.QueryString("id") & " , idattività,'" & ClsServer.NoApice(Session("Utente")) & "', getdate()" & _
        " from attività where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        'modifico prima la cronologia dell'attività
        cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaAttività" & _
        " (idattività,idstatoattività,datacronologia,idTipoCronologia," & _
        " usernameaccreditatore)" & _
        " select idattività," & _
        " idstatoattività,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'" & _
        " from attività where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        'eseguo modifica su tabella attività
        cmdinsert = New Data.SqlClient.SqlCommand("update attività" & _
        "  set idbandoattività=null," & _
        " idstatoattività=(select idstatoattività from" & _
        "  statiattività where defaultstato=1)" & _
        " where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        booverifica = True

        'elimino record in bandiattività
        cmdinsert = New Data.SqlClient.SqlCommand("delete from bandiattività" & _
        " where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        'If booverifica = True Then 'se è stato annullato qualcosa
        lblMessaggio.Text = "L'Istanza è stata eliminata con successo"
        lblstato.Text = "ELIMINATA"
        PerrsonalizzaTasti("0", "0", "0", "1", "0", "1")
        cmdInserisci.Visible = False
        'cmdannulla.Visible = False
        'cmdmodifica.Visible = False
        'cmdPresentaIstanza.Visible = False

        'Else
        'lblMessaggio.Text = "Attenzione, impossibile annullare l'Istanza"
        '
        'End If

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Call VerificaCompetenze()
        End If
    End Sub

    Protected Sub cmdPresentaIstanza_Click(sender As Object, e As EventArgs) Handles cmdPresentaIstanza.Click
        '***Generata da Gianluigi Paesani in data:19/07/04
        '***modulo che rendera i progetti associati al bando presentati e quindi non più
        '***modificabili
        Dim booverifica As Boolean = False 'setto a zero parametri utenza
        Dim intAnnoBreve As Integer

        lblMessaggio.Text = ""

        cmdPresentaIstanza.Visible = False

        'Aggiunto da Danilo Spagnulo il 12/12/2012
        If StatoDocumenti(Session("idente")) = True Then
            ImgAnteprimaStampa.Visible = False

            cmdPresentaIstanza.Visible = False
            lblMessaggioPresenta.Visible = False
            cmdannulla.Visible = False
            cmdmodifica.Visible = False
            lblMessaggio.Text = "Attenzione, è in corso l'applicazione di documenti a progetti. Si prega di attendere il completamento dell'operazione e riprovare."

            bloccaCheck()
            'Response.Write("<script language=""javascript"">" & vbCrLf)
            'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
            'Response.Write("</script>" & vbCrLf)
            cmdPresentaIstanza.Visible = False
            Exit Sub
        End If
        'Aggiunto da Alessandra Taballione il 21/09/2005
        If ControllaStatoChiuso() = True Then
            cmdPresentaIstanza.Visible = False
            lblMessaggioPresenta.Visible = False
            cmdannulla.Visible = False
            cmdmodifica.Visible = False
            lblMessaggio.Text = "Attenzione, non è possibile Modificare l'Istanza poichè la Circolare presentazione progetti risulta CHIUSA."

            bloccaCheck()
            'Response.Write("<script language=""javascript"">" & vbCrLf)
            'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
            'Response.Write("</script>" & vbCrLf)
            cmdPresentaIstanza.Visible = False
            Exit Sub
        End If
        If ProceduraDiModifica() = 1 Then
            'Response.Write("<script language=""javascript"">" & vbCrLf)
            'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
            'Response.Write("</script>" & vbCrLf)
            cmdPresentaIstanza.Visible = True
            Exit Sub 'se non ci sono flag interrompo comando
        End If
        'VERIFICA CHE L'RLEA SIA STATO INSERITO PER TUTTE LE SEDI NECESSARIE

        Dim strMsg As String = ControllaCompletamentoRLEA(CInt(txtidbandoAttivita.Text))
        If strMsg <> "" Then
            cmdPresentaIstanza.Visible = False
            lblMessaggioPresenta.Visible = False
            cmdannulla.Visible = False
            cmdmodifica.Visible = False
            lblMessaggio.Text = strMsg

            bloccaCheck()
            'Response.Write("<script language=""javascript"">" & vbCrLf)
            'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
            'Response.Write("</script>" & vbCrLf)
            cmdPresentaIstanza.Visible = True
            Exit Sub
        End If
        If Session("TipoUtente") = "E" Then
            'TOLTO CONTROLLO PER NUOVA PRESENTAZIONE 2020
            'If ControllaRISORSE() = 1 Then
            '    Response.Redirect("WFrmControlloProvincie.aspx?idbando=" & txtidbando.Text & "&Messaggio=Impossibile Presentare l'Istanza. E' necessario verificare le Risorse Richieste e Inserite per le seguenti Provincie.")
            '    Exit Sub
            'End If

            If ControllaOlpSedi() = 1 Then
                Response.Write("<script>" & vbCrLf)
                Response.Write("window.open(""risorsesedidiverse.aspx?idbando=" & txtidbando.Text & "&Messaggio=Impossibile Presentare l'Istanza. E' necessario verificare le seguenti anomalie sulle figure professionali."", """", ""width=700,height=620,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
                Response.Write("</script>")



                cmdPresentaIstanza.Visible = True
                '''lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè risulta associato lo stesso Olp per Sedi diverse."
                '''
                Exit Sub
            End If
            'agg. il 03/10/2006 da simona cordella
            'aggiungere funzione per il controllo compatibiliù dei ruoli di una risora
            If ControllaRuoliRisorse() = 1 Then
                'Response.Write("<script language=""javascript"">" & vbCrLf)
                'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
                'Response.Write("</script>" & vbCrLf)

                Response.Write("<script>" & vbCrLf)
                Response.Write("window.open(""risorsesedidiverse.aspx?idbando=" & txtidbando.Text & "&Messaggio=Impossibile Presentare l'Istanza. E' necessario verificare le seguenti anomalie sulle figure professionali."", """", ""width=700,height=620,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
                Response.Write("</script>")
                cmdPresentaIstanza.Visible = True
                Exit Sub
            End If
            'If ControllaDISABILI() = 1 Then
            '    'Response.Write("<script language=""javascript"">" & vbCrLf)
            '    'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
            '    'Response.Write("</script>" & vbCrLf)

            '    Response.Write("<script>" & vbCrLf)
            '    Response.Write("window.open(""controllodisabili.aspx?idbando=" & txtidbando.Text & """, """", ""width=600,height=350,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
            '    Response.Write("</script>")
            '    cmdPresentaIstanza.Visible = True
            '    Exit Sub
            'End If
        Else
            '16/02/2010 l'unsc e le regioni vengono bloccati come gli enti
            'TOLTO CONTROLLO PER NUOVA PRESENTAZIONE 2020
            'If ControllaRISORSE() = 1 Then
            '    Response.Redirect("WFrmControlloProvincie.aspx?idbando=" & txtidbando.Text & "&Messaggio=E' necessario verificare le Risorse Richieste e Inserite per le seguenti Provincie.")
            '    cmdPresentaIstanza.Visible = True
            '    Exit Sub
            'End If

            If ControllaOlpSedi() = 1 Then
                'Response.Write("<script language=""javascript"">" & vbCrLf)
                'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
                'Response.Write("</script>" & vbCrLf)

                Response.Write("<script>" & vbCrLf)
                Response.Write("window.open(""risorsesedidiverse.aspx?idbando=" & txtidbando.Text & "&Messaggio=E' necessario verificare le seguenti anomalie sulle figure professionali."", """", ""width=700,height=620,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
                Response.Write("</script>")
                '''lblMessaggio.Text = "Attenzione, non è possibile Presentare l'Istanza poichè risulta associato lo stesso Olp per Sedi diverse."
                '''
                cmdPresentaIstanza.Visible = True
                Exit Sub
            End If
            'agg. il 03/10/2006 da simona cordella
            'aggiungere funzione per il controllo compatibiliù dei ruoli di una risora
            If ControllaRuoliRisorse() = 1 Then
                'Response.Write("<script language=""javascript"">" & vbCrLf)
                'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
                'Response.Write("</script>" & vbCrLf)

                Response.Write("<script>" & vbCrLf)
                Response.Write("window.open(""risorsesedidiverse.aspx?idbando=" & txtidbando.Text & "&Messaggio=E' necessario verificare le seguenti anomalie sulle figure professionali."", """", ""width=700,height=620,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
                Response.Write("</script>")
                cmdPresentaIstanza.Visible = True
                Exit Sub
            End If

            'If ControllaDISABILI() = 1 Then
            '    'Response.Write("<script language=""javascript"">" & vbCrLf)
            '    'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
            '    'Response.Write("</script>" & vbCrLf)

            '    Response.Write("<script>" & vbCrLf)
            '    Response.Write("window.open(""controllodisabili.aspx?idbando=" & txtidbando.Text & """, """", ""width=600,height=350,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
            '    Response.Write("</script>")
            '    cmdPresentaIstanza.Visible = True
            '    Exit Sub
            'End If
        End If
        'If ProceduraDiModifica() = 1 Then
        '    Exit Sub 'se non ci sono flag interrompo comando
        'End If
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtr As Data.SqlClient.SqlDataReader
        'modifico stato istanza per non essere più modificabile
        cmdinsert = New Data.SqlClient.SqlCommand("update bandiattività" & _
        " set idstatobandoattività=(select idstatobandoattività" & _
        " from statibandiattività where Davalutare=1),usernamepresentazione='" & Session("Utente") & "',datapresentazione=getdate()" & _
        " where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()

        'UpDate dei Posti Richiesti + FAMI
        cmdinsert = New Data.SqlClient.SqlCommand("Update Attività Set NumeroPostiNoVittoNoAlloggioRic = NumeroPostiNoVittoNoAlloggio, NumeroPostiVittoAlloggioRic = NumeroPostiVittoAlloggio, NumeroPostiVittoRic = NumeroPostiVitto, NumeroPostiFamiRichiesti=NumeroPostiFami  Where IdBandoAttività = " & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()

        'Modifico la data di presentazione dei progetti sulle singole attività
        cmdinsert = New Data.SqlClient.SqlCommand("Update attività set DataPresentazioneProgetto=getdate() where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()

        'eseguo query per visualizzazione stato attuale
        dtr = ClsServer.CreaDatareader("select b.statobandoattività,b.defaultstato,b.davalutare,b.attivo,b.chiuso,b.inammissibile,b.cancellata,bando.annobreve  from bandiattività a" & _
        " inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività" & _
        " inner join bando on bando.idbando=a.idbando  " & _
        " where a.idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        If dtr.HasRows = True Then
            Do While dtr.Read()
                lblstato.Text = dtr.GetValue(0) 'visualizzo stato
                intAnnoBreve = dtr.GetValue(7)
                PerrsonalizzaTasti(dtr.GetValue(1), dtr.GetValue(2), dtr.GetValue(3), dtr.GetValue(4), dtr.GetValue(5), dtr.GetValue(6))
            Loop
        End If
        dtr.Close()
        dtr = Nothing
        'Eseguo StoreProcedure per codice progetto .
        ClsServer.EseguiStoreGeneraCodiciProgetto(txtidbandoAttivita.Text, "SP_GENERA_CODICI_PROGETTO", Session("conn"))

        '- IF PUNTAMENTO SERVER E DB ESCLUSA IL 16/06/2014 x messa in produzione progetti online
        'creo il fascicolo solo se sono sul server di produzione SQLHELIOS 
        'If UCase(ConfigurationSettings.AppSettings("DB_DATA_SOURCE")) = "SQLHELIOS" And UCase(ConfigurationSettings.AppSettings("DB_NAME")) = "UNSCPRODUZIONE" Then
        'modifica per generazione automatica fascicolo
        If TrovaRegioneCompetenzaBando(Request.QueryString("id")) = 22 Then
            Dim WSInterno As New WSHeliosInterno.HeliosInterno
            Dim wsOut As String

            Try
                WSInterno.Url = ConfigurationSettings.AppSettings("URL_WSHeliosInterno")
                wsOut = WSInterno.CreaFascicoloIstanza(Request.QueryString("id"), Session("idente"))
            Catch ex As Exception

            End Try
        End If

        '***** inizio simona 14/08/2012 *********
        ' richiamo routine per generazione e salvataggio box16 box19
        'visulizzo la stampa del box 16 solo per i bandi dal 2010 in poi
        If intAnnoBreve >= 10 Then
            'Il numero dei progetti è minore di 30 genero il box all'interno di helios
            If Dgtattivita.Items.Count < 1 Then
                GenerazioneBOX16_BOX19(txtidbandoAttivita.Text)
            Else
                'genero il box dal wsDocumentazione in modalità asincrona
                'richiama nuova pagina informativa 
                GenerazioneBOX16_BOX19_Da_WSDocumentazione(txtidbandoAttivita.Text)
                'reindirazzamento ad una pagine informativa finche il flag InLavorazione di Bando Attività nn ritorta a 0
                'SANDOKAN

                'Server.Transfer("WfrmInfoPresentazioneIstanza.aspx?IdBandoAttivita=" & txtidbandoAttivita.Text & "")

                Response.Redirect("WfrmInfoPresentazioneIstanza.aspx?IdBandoAttivita=" & txtidbandoAttivita.Text & "")
                Exit Sub
            End If
        End If
        'End If

        'setto wform
        lblMessaggio.Text = "L'Istanza è stata presentata con successo"
        cmdInserisci.Visible = False
        cmdannulla.Visible = False
        cmdmodifica.Visible = False
        cmdPresentaIstanza.Visible = False
        lblMessaggioPresenta.Visible = False

        'disabilito colonna info
        'Dgtattivita.Columns(16).Visible = False
        cmdAnnullaPresentazione.Visible = True
        Dgtattivita.Columns(18).Visible = False ' pulsante applica
        Dgtattivita.Columns(19).Visible = False ' colonna popup info
        ImgAnteprimaStampa.Visible = False

        'Aggiunto da Alessandra il 15/07/2005
        If Session("tipoutente") = "E" Then
            cmddissaccredita.Visible = False
            cmdaccredita.Visible = False
        End If

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Call VerificaCompetenze()
        End If

        Dim JScript As String

        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=16&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
        'If ReturnRegioneCompetenzaBando(txtidbandoAttivita.Text) = 22 Then
        '    JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=16&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
        'Else

        '    JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=37&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf


        '    imgStampaAll.Visible = True
        '    LblStampaAll.Visible = True
        '    'visulizzo la stampa del box 16 solo per i bandi dal 2010 in poi
        '    If intAnnoBreve >= 10 Then
        '        VisualizzaStampePerNazioneBase(CInt(txtidbandoAttivita.Text))
        '        If Not Session("Sap") Is Nothing Then
        '            If Session("Sap") = True Then
        '                JScript &= "myWin = window.open (""WfrmReportistica.aspx?sTipoStampa=32&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
        '            End If

        '        End If
        '        If Not Session("SapEst") Is Nothing Then
        '            If Session("SapEst") = True Then
        '                JScript &= "myWin = window.open (""WfrmReportistica.aspx?sTipoStampa=35&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")"
        '            End If
        '        End If
        '        'imgStampaSAP.Visible = True
        '        'LblStampaSAP.Visible = True
        '        'imgStampaSAPEstero.Visible = True
        '        'LblStampaSapEstero.Visible = True
        '    Else
        '        Session("Sap") = False
        '        Session("SapEst") = False
        '    End If
        'End If
        '  JScript &= ("window.close();" & vbCrLf)
        imgStampaAll.Visible = True

        'JScript &= ("</script>")
        ''    RegistraStampaAvvenuta()
        'Response.Write(JScript)

        'Response.Write("<script language=""javascript"">" & vbCrLf)
        'Response.Write("document.getElementById(""imgAttesa"").style.visibility = ""hidden"";" & vbCrLf)
        'Response.Write("</script>" & vbCrLf)

        RegistraStampaAvvenuta()
    End Sub

    Protected Sub cmdaccredita_Click(sender As Object, e As EventArgs) Handles cmdaccredita.Click
        '***Generata da Gianluigi Paesani in data:26/07/04
        '***Questa Routine accredita l'istanza presentata
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtr As Data.SqlClient.SqlDataReader
        Dim i As Integer

        If (Trim(lblStatoEnte.Text) = "Attivo" Or Trim(lblStatoEnte.Text) = "In Adeguamento" Or Trim(lblStatoEnte.Text) = "In Chiusura" Or Trim(lblStatoEnte.Text) = "Ex SCN con Prog") Then
            ''verifico se è stato indicato il numero di protocollo per il personale UNSC
            'If Session("TipoUtente") = "U" Then
            '    If dtgElencoProt.Items.Count = 0 Then
            '        lblMessaggio.Text = "E' necessario indicare almeno un protocollo per Accettare l'Istanza."
            '        Exit Sub
            '    End If
            'End If

            lblMessaggio.Text = ""

            'eseguo comando per cambio stato
            cmdinsert = New Data.SqlClient.SqlCommand("update bandiattività" & _
            " set idstatobandoattività=" & _
            " (select idstatobandoattività from statibandiattività where attivo=1)," & _
            " usernameAccreditatore='" & ClsServer.NoApice(Session("utente")) & "',DataAccettazione=getdate() where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
            cmdinsert.ExecuteNonQuery()
            cmdinsert.Dispose()

            'eseguo query per visualizzazione stato attuale
            dtr = ClsServer.CreaDatareader("select b.statobandoattività,b.defaultstato,b.davalutare,b.attivo,b.chiuso,b.inammissibile,b.cancellata from bandiattività a" & _
            " inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività" & _
            " where a.idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
            If dtr.HasRows = True Then
                Do While dtr.Read()
                    lblstato.Text = dtr.GetValue(0) 'visualizzo stato
                    PerrsonalizzaTasti(dtr.GetValue(1), dtr.GetValue(2), dtr.GetValue(3), dtr.GetValue(4), dtr.GetValue(5), dtr.GetValue(6))
                Loop
            End If
            dtr.Close()
            dtr = Nothing

            'setto wform

            lblMessaggio.Text = "L'istanza e' stata accettata con successo"
            cmdInserisci.Visible = False

            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                Call VerificaCompetenze()
            End If

        Else

            lblMessaggio.Text = "L'istanza non puo' essere accettata perche' lo stato dell'ente non e' Attivo."
            Exit Sub

        End If
    End Sub

    Protected Sub cmddissaccredita_Click(sender As Object, e As EventArgs) Handles cmddissaccredita.Click
        '***Generata da Gianluigi Paesani in data:26/07/04
        '***Questa Routine annulla l'accreditamento 
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtable As DataTable
        Dim myRow As DataRow

        lblMessaggio.Text = ""

        imgStampaAll.Visible = False

        Session("Sap") = False
        Session("SapEst") = False


        'eseguo comando per modifca stato bandoattività
        cmdinsert = New Data.SqlClient.SqlCommand("update bandiattività" & _
        " set idstatobandoattività=" & _
        " (select idstatobandoattività from statibandiattività where Inammissibile=1)," & _
        " usernameAccreditatore='" & ClsServer.NoApice(Session("utente")) & "',DataAccettazione=getdate() where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()

        '**** aggiunto il 17/05/2017 da s.c.
        'inserisco in cronologia e modifico lo stato del progetto in Archiviato
        cmdinsert = New Data.SqlClient.SqlCommand(" Insert into cronologiaattività " & _
                 " select idattività,idstatoattività,getdate(),0,'Archiviato per istanza inammissibile', " & _
                 " '" & ClsServer.NoApice(Session("utente")) & "',0 " & _
                 " FROM ATTIVITà A " & _
                 " INNER JOIN BANDIATTIVITà B ON A.IDBANDOATTIVITà = B.IDBANDOATTIVITà " & _
                 " WHERE a.IDBANDOattività =" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        cmdinsert = New Data.SqlClient.SqlCommand(" UPDATE ATTIVITà " & _
                " SET IDSTATOATTIVITà = 11,DATAULTIMOSTATO=GETDATE(),USERNAMESTATO = '" & ClsServer.NoApice(Session("utente")) & "', " & _
                " NOTESTATO='Archiviato per istanza inammissibile'  " & _
                " FROM ATTIVITà A " & _
                " INNER JOIN BANDIATTIVITà B ON A.IDBANDOATTIVITà = B.IDBANDOATTIVITà  " & _
                " WHERE a.IDBANDOATTIVITà = " & Request.QueryString("idBA") & "", Session("conn"))

        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        '******

            'eseguo query per visualizzazione stato attuale
            'Modificato da Alessandra Taballione il 02/02/2005
            'Agguinto controllo sullo stato per blocco Attività
        dtable = ClsServer.CreaDataTable("select b.statobandoattività,b.defaultstato,b.davalutare,b.attivo,b.chiuso,b.inammissibile,b.cancellata " & _
        " from bandiattività a" & _
        " inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività" & _
        " where a.idbandoattività=" & Request.QueryString("idBA") & "", False, Session("conn"))

        For Each myRow In dtable.Rows
            lblstato.Text = myRow.Item("statobandoattività") 'dtr.GetValue(0) 'visualizzo stato
            PerrsonalizzaTasti(myRow.Item("defaultstato"), myRow.Item("davalutare"), myRow.Item("attivo"), myRow.Item("chiuso"), myRow.Item("inammissibile"), myRow.Item("cancellata"))
        Next
            'visualizzo stato

        lblMessaggio.Text = "L'istanza e' stata respinta."
        cmdInserisci.Visible = False
            'Dgtattivita.Columns(16).Visible = True
        Dgtattivita.Columns(17).Visible = True 'colonna popup documenti

        Dgtattivita.Columns(18).Visible = True 'colonna pulcanse applica
        Dgtattivita.Columns(19).Visible = True 'colonna popup info 

            'cmdannulla.Visible = False
            'cmdmodifica.Visible = False
            'cmddissaccredita.Visible = False
            'cmdaccredita.Visible=False
            'Aggiunto da Alessandra Taballione il 02.02.2005
        CaricaGriglia()
    End Sub

    Protected Sub cmdAnnullaPresentazione_Click(sender As Object, e As EventArgs) Handles cmdAnnullaPresentazione.Click
        '***Generata da Gianluigi Paesani in data:26/07/04
        '***Questa Routine annulla l'accreditamento
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtable As DataTable
        Dim myRow As DataRow

        lblMessaggio.Text = ""

        imgStampaAll.Visible = False

        Session("Sap") = False
        Session("SapEst") = False

        CancellaBOX(Request.QueryString("idBA"))
        cmdAnnullaPresentazione.Visible = False
        'eseguo comando per modifca stato bandoattività
        cmdinsert = New Data.SqlClient.SqlCommand("update bandiattività" & _
        " set idstatobandoattività=" & _
        " (select idstatobandoattività from statibandiattività where defaultstato=1)," & _
        " usernameAccreditatore='" & ClsServer.NoApice(Session("utente")) & "',DataAccettazione=getdate() where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert = New Data.SqlClient.SqlCommand("update attività" & _
        " set codiceente='' where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        'eseguo query per visualizzazione stato attuale
        'Modificato da Alessandra Taballione il 02/02/2005
        'Agguinto controllo sullo stato per blocco Attività
        dtable = ClsServer.CreaDataTable("select b.statobandoattività,b.defaultstato,b.davalutare,b.attivo,b.chiuso,b.inammissibile,b.cancellata " & _
        " from bandiattività a" & _
        " inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività" & _
        " where a.idbandoattività=" & Request.QueryString("idBA") & "", False, Session("conn"))

        For Each myRow In dtable.Rows
            lblstato.Text = myRow.Item("statobandoattività") 'dtr.GetValue(0) 'visualizzo stato
            PerrsonalizzaTasti(myRow.Item("defaultstato"), myRow.Item("davalutare"), myRow.Item("attivo"), myRow.Item("chiuso"), myRow.Item("inammissibile"), myRow.Item("cancellata"))
        Next
        'visualizzo stato

        'lblMessaggio.Text = "L'istanza e' stata annullata."

        cmdInserisci.Visible = False
        Dgtattivita.Columns(16).Visible = True 'colonna numero documenti
        Dgtattivita.Columns(17).Visible = True 'colonna popup documenti
        Dgtattivita.Columns(18).Visible = True 'colonna popup applica ducumenti
        Dgtattivita.Columns(19).Visible = True 'colonna popup info
        'cmdannulla.Visible = False
        'cmdmodifica.Visible = False
        'cmddissaccredita.Visible = False
        'cmdaccredita.Visible=False
        'Aggiunto da Alessandra Taballione il 02.02.2005
        CaricaGriglia()

        'If booverifica = True Then 'se è stato annullato qualcosa
        lblMessaggio.Visible = True
        lblMessaggio.Text = "La presentazione è stata annullata con successo"
        'lblstato.Text = "ANNULLATA"
        PerrsonalizzaTasti("0", "0", "0", "1", "0", "1")
        cmdInserisci.Visible = False
        'cmdannulla.Visible = False
        'cmdmodifica.Visible = False
        'cmdPresentaIstanza.Visible = False


        imgStampaAll.Visible = False
       
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Call VerificaCompetenze()
        End If
    End Sub

    Protected Sub imgSalvaProt_Click(sender As Object, e As EventArgs) Handles imgSalvaProt.Click
        AssociaProtocolloIstanza(Request.QueryString("idBA"), txtNumProt.Text, txtDataProt.Text, Session("Utente"))
        dtgElencoProt.CurrentPageIndex = 0
        CaricaProtocolli(Request.QueryString("idBA"))
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        SalvaFascicolo()
    End Sub

    Private Sub imgCheckOLP_Command(sender As Object, e As System.Web.UI.WebControls.CommandEventArgs) Handles imgCheckOLP.Command
        If e.CommandName = "Olp" Then
            Dim info As String
            Dim idBando As Integer
            info = "Olp"
            idBando = txtidbando.Text
            OpenWindow(info, idBando)
        End If
    End Sub

    Private Sub ImgControllaProvincie_Command(sender As Object, e As System.Web.UI.WebControls.CommandEventArgs) Handles ImgControllaProvincie.Command
        If e.CommandName = "Provincia" Then
            Dim info As String
            Dim idBando As Integer
            info = "Provincia"
            idBando = txtidbando.Text
            OpenWindow(info, idBando)
        End If
    End Sub

    Protected Sub ImgSellProtollo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgSellProtollo.Click
        If txtNumProt.ReadOnly = False Then
            If TxtCodiceFascicolo.Text = "" Then
                lblMessaggio.Visible = True
                lblMessaggio.ForeColor = Color.Red
                lblMessaggio.Text = "Specificare il numero fascicolo!"
                Exit Sub
            Else
                Dim info As String
                Dim Nessuno As Integer
                info = "ImgSellProtollo"
                Nessuno = 0
                OpenWindow(info, Nessuno)
            End If

        End If
    End Sub

    Protected Sub cmdSelProtocollo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdSelProtocollo.Click

            If TxtCodiceFascicolo.Text = "" Then
                lblMessaggio.Visible = True
                lblMessaggio.ForeColor = Color.Red
                lblMessaggio.Text = "Specificare il numero fascicolo!"
                TxtCodiceFascicolo.Focus()
                Exit Sub
            Else
                Dim info As String
                Dim Nessuno As Integer
            info = "cmdSelProtocollo"
                Nessuno = 0
                OpenWindow(info, Nessuno)
            End If


    End Sub

    Protected Sub OpenWindow(quale, parametro)
        Select Case quale

            Case "Info"
                Dim url As String = "WfrmVerificaIstanzaAnomaliaProgetto.aspx?IdAttivita=" & parametro & "&VengoDa=" & 6
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
            Case "Olp"
                Dim url As String = "risorsesedidiverse.aspx?idbando=" & parametro & "&VengoDa=" & 7
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=700,left=100,top=100,resizable=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
            Case "Provincia"
                Response.Redirect("WFrmControlloProvincie.aspx?idBando=" & parametro & "&VengoDa=" & 8 & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))
                'Dim url As String = "WFrmControlloProvincie.aspx?idbando=" & parametro & "&VengoDa=" & 8
                'Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"
                'ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
            Case "ImgSellProtollo"
                Dim url As String = "WfrmSIGEDElencoDocumenti.aspx?TxtProt=" & txtNumProt.Text & "&TxtData=" & txtDataProt.Text & "&NumeroFascicolo=" & TxtIdFascicolo.Text & "&VengoDa=" & 9
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
            Case "cmdSelProtocollo"
                Dim url As String = "WfrmSIGEDElencoDocumenti.aspx?NumeroFascicolo=" & TxtIdFascicolo.Text & "&VengoDa=" & 10
                Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
        End Select

    End Sub

    Private Sub chkSelDesel_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelDesel.CheckedChanged
        If chkSelDesel.Checked = True Then
            chkSelDesel2.Checked = True
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = Dgtattivita.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = Dgtattivita.Items(x).Cells(3).FindControl("chk")
                If chkoggetto.Visible = True Then
                    chkoggetto.Checked = True
                Else
                    chkoggetto.Checked = False
                End If
            Next
        Else
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = Dgtattivita.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = Dgtattivita.Items(x).Cells(3).FindControl("chk")
                chkoggetto.Checked = False
            Next
            chkSelDesel2.Checked = False
        End If
    End Sub

    Private Sub chkSelDesel2_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelDesel2.CheckedChanged
        If chkSelDesel2.Checked = True Then
            chkSelDesel.Checked = True
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = Dgtattivita.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = Dgtattivita.Items(x).Cells(3).FindControl("chk")
                If chkoggetto.Visible = True Then
                    chkoggetto.Checked = True
                Else
                    chkoggetto.Checked = False
                End If

            Next

        Else
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = Dgtattivita.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = Dgtattivita.Items(x).Cells(3).FindControl("chk")
                chkoggetto.Checked = False
            Next
            chkSelDesel.Checked = False
        End If
    End Sub

    Protected Sub ImgAnteprimaStampa_Click(sender As Object, e As EventArgs) Handles ImgAnteprimaStampa.Click
        'Response.Redirect("WfrmReportistica.aspx?sTipoStampa=36&IdBandoAttivita=" & txtidbandoAttivita.Text)
        Dim JScript As String

        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=36&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
        JScript &= ("</script>")
        Response.Write(JScript)
    End Sub

    Protected Sub imgStampaAll_Click(sender As Object, e As EventArgs) Handles imgStampaAll.Click
        'Response.Redirect("WfrmReportistica.aspx?sTipoStampa=16&IdBandoAttivita=" & txtidbandoAttivita.Text)

        Dim JScript As String

        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmReportistica.aspx?sTipoStampa=16&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf

        'If Not Session("Sap") Is Nothing Then
        '    If Session("Sap") = True Then
        '        JScript &= "myWin = window.open (""WfrmReportistica.aspx?sTipoStampa=32&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
        '    End If
        'End If
        'If Not Session("SapEst") Is Nothing Then
        '    If Session("SapEst") = True Then
        '        JScript &= "myWin = window.open (""WfrmReportistica.aspx?sTipoStampa=35&IdBandoAttivita=" & txtidbandoAttivita.Text & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")"
        '    End If
        'End If
        JScript &= ("</script>")

        Response.Write(JScript)
        RegistraStampaAvvenuta()
    End Sub

    Protected Sub cmdRipristina_Click(sender As Object, e As EventArgs) Handles cmdRipristina.Click
        '***Generata da Simona Cordella il 19/05/2017
        '***Questa Routine rispristino istanza respinta  pulsante visiibile solo per IL DGSCN e REGIONE
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtable As DataTable
        Dim myRow As DataRow

        lblMessaggio.Text = ""



        Session("Sap") = False
        Session("SapEst") = False


        'eseguo comando per modifca stato bandoattività
        cmdinsert = New Data.SqlClient.SqlCommand("update bandiattività" & _
        " set idstatobandoattività=2, " & _
        " usernameAccreditatore='" & ClsServer.NoApice(Session("utente")) & "',DataAccettazione=getdate() where idbandoattività=" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()

        '**** aggiunto il 17/05/2017 da s.c.
        'inserisco in cronologia e modifico lo stato del progetto in Archiviato
        cmdinsert = New Data.SqlClient.SqlCommand(" Insert into cronologiaattività " & _
                 " select idattività,idstatoattività,getdate(),0,'Ripristino istanza inammissibile in Presentata', " & _
                 " '" & ClsServer.NoApice(Session("utente")) & "',0 " & _
                 " FROM ATTIVITà A " & _
                 " INNER JOIN BANDIATTIVITà B ON A.IDBANDOATTIVITà = B.IDBANDOATTIVITà " & _
                 " WHERE a.IDBANDOattività =" & Request.QueryString("idBA") & "", Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        cmdinsert = New Data.SqlClient.SqlCommand(" UPDATE ATTIVITà " & _
                " SET IDSTATOATTIVITà = 4,DATAULTIMOSTATO=GETDATE(),USERNAMESTATO = '" & ClsServer.NoApice(Session("utente")) & "', " & _
                " NOTESTATO='Ripristino istanza inammissibile in Presentata'  " & _
                " FROM ATTIVITà A " & _
                " INNER JOIN BANDIATTIVITà B ON A.IDBANDOATTIVITà = B.IDBANDOATTIVITà  " & _
                " WHERE a.IDBANDOATTIVITà = " & Request.QueryString("idBA") & "", Session("conn"))

        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        '******

        'eseguo query per visualizzazione stato attuale
        'Modificato da Alessandra Taballione il 02/02/2005
        'Agguinto controllo sullo stato per blocco Attività
        dtable = ClsServer.CreaDataTable("select b.statobandoattività,b.defaultstato,b.davalutare,b.attivo,b.chiuso,b.inammissibile,b.cancellata " & _
        " from bandiattività a" & _
        " inner join statibandiattività b on a.idstatobandoattività=b.idstatobandoattività" & _
        " where a.idbandoattività=" & Request.QueryString("idBA") & "", False, Session("conn"))

        For Each myRow In dtable.Rows
            lblstato.Text = myRow.Item("statobandoattività") 'dtr.GetValue(0) 'visualizzo stato
            PerrsonalizzaTasti(myRow.Item("defaultstato"), myRow.Item("davalutare"), myRow.Item("attivo"), myRow.Item("chiuso"), myRow.Item("inammissibile"), myRow.Item("cancellata"))
        Next
        'visualizzo stato

        lblMessaggio.Text = "L'istanza e' stata Ripristinata."
        cmdInserisci.Visible = False
        'Dgtattivita.Columns(16).Visible = True
        Dgtattivita.Columns(17).Visible = True 'colonna popup documenti

        Dgtattivita.Columns(18).Visible = True 'colonna pulcanse applica
        Dgtattivita.Columns(19).Visible = True 'colonna popup info 
        imgStampaAll.Visible = True
        cmdRipristina.Visible = False
        'cmdannulla.Visible = False
        'cmdmodifica.Visible = False
        'cmddissaccredita.Visible = False
        'cmdaccredita.Visible=False
        'Aggiunto da Alessandra Taballione il 02.02.2005
        CaricaGriglia()
    End Sub

    Protected Sub Dgtattivita_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Dgtattivita.SelectedIndexChanged

    End Sub
End Class