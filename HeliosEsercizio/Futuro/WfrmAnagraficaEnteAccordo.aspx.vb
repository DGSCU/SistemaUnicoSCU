Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Security.Cryptography
Imports System.Linq

Public Class WfrmAnagraficaEnteAccordo
    Inherits SmartPage

    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim cmdGenerico As SqlClient.SqlCommand
    Dim bandiera As Integer
    Dim bandierina As Integer 'la uso per sapere se devo mettere il marcatore a 1 nella function EntiVariazioniAccordi()
    Dim IdComuneRes As String
    Dim strsql As String
    Dim strQuery As String
    Dim blnCambiaClasse As Boolean 'variabile che identifica se la classe è stata modificata (=True)
    Dim MyRow As DataRow
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim helper As Helper = New Helper()
    Dim selComune As New clsSelezionaComune
    Dim LogParametri As New Hashtable
    Shared isVariazione As Boolean
    Shared flgForzaVariazione As Boolean
    Protected Const ANNIPRECEDENTI As Integer = 3
    Protected AnniCaricamento(2) As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA MODIFICA:  25/07/2014
        'FUNZIONALITA': GESTIONE LOAD MASCHERA ENTE ACCORDO

        Dim abilitato As Integer
        Dim dati As Integer
        Dim annullamodifica As Integer
        Dim visualizzadatiaccreditati As Integer
        Dim annullacancellazione As Integer
        Dim messaggio As String

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Request.Form("txtCancellaSede") = "DELETE" Then
            Session("AnnullaAccordo") = ""
            AnnullaAccordo()
        End If

        checkSpid()
        'Pulizia Dati
        lblErroreAttoCostitutivo.Text = ""
        lblErroreStatuto.Text = ""
        lblErroreDeliberaAdesione.Text = ""
        lblErroreImpegnoEtico.Text = ""
        lblErroreImpegno.Text = ""

        If IsPostBack = False Then

            'Session("EsperienzeAreeSettoreAccordo") = Nothing
            'Session("IDClasseAccreditamentoAccordo") = Nothing
            'Session("LoadedStatutoAccordo") = Nothing
            'Session("LoadedACAccordo") = Nothing
            'Session("LoadedDeliberaAdesioneAccordo") = Nothing
            'Session("LoadedCartaImpegnoEticoAccordo") = Nothing
            'Session("LoadedCartaImpegnoAccordo") = Nothing
            'Session("IdEnteFaseArt") = Nothing
            'Session("ElencoSettoriDBAccordo") = Nothing

            msgErrore.Text = String.Empty
            msgConferma.Text = String.Empty
            Dim AlboEnte As String

            AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
            CaricaCombo(AlboEnte)
            CaricaSettori(AlboEnte)

            LoadMaschera()

            Dim identefiglio As Integer = IIf(Request.QueryString("id") = Nothing, 0, Request.QueryString("id"))
            If identefiglio > 0 Then EvidenziaDatiModificati(identefiglio)

            If Request.QueryString("azione") = "Ins" Then
                Dim SelComune As New clsSelezionaComune
                'Dim blnEstero As Boolean = False
                SelComune.CaricaProvinciaNazione(ddlProvincia, ChkEstero.Checked, Session("Conn"))
                'VisibilitàCampiRiserva(False)
                divRiserva.Visible = False
            Else
                Dim strATTIVITA As Integer = -1
                Dim strBANDOATTIVITA As Integer = -1
                Dim strENTEPERSONALE As Integer = -1
                Dim strENTITA As Integer = -1
                Dim strIDENTE As Integer = Request.QueryString("id")
                If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 0 Then
                    Response.Redirect("wfrmAnomaliaDati.aspx")
                End If

                If Session("TipoUtente") = "U" Then

                    If AlboEnte = "SCN" Or LblDataRiserva.Text = "&nbsp;" Then
                        'VisibilitàCampiRiserva(False)
                        divRiserva.Visible = False
                    Else

                        divRiserva.Visible = True
                        If VerificaModIns() = True Then
                            DivAccreditaRiserva.Visible = True
                            BtnAssegna.Visible = True
                            CaricaCausaliRiserva()
                            CaricaRiservaSel()
                        End If

                        'VisibilitàCampiRiserva(True)
                    End If
                End If
            End If
        End If
        If ChkEstero.Checked Then
            lblComune.Text = "<strong>(*)</strong>Località"
        Else
            lblComune.Text = "<strong>(*)</strong>Comune"
        End If
        If ddlTipologia.SelectedValue = "Privato" Then
            divSenzaScopoLucro.Visible = True
        Else
            divSenzaScopoLucro.Visible = False
        End If

        If ddlGiuridica.SelectedItem IsNot Nothing AndAlso ddlGiuridica.SelectedItem.Text Like "Altro*" Or
           ddlGiuridica.SelectedItem IsNot Nothing AndAlso ddlGiuridica.SelectedItem.Text = "Altro ente" Then
            divAltraTipologia.Visible = True
        Else
            divAltraTipologia.Visible = False
        End If

        If Session("TipoUTente") = "U" Then
            cmdPdfSettori.Visible = True
        End If
    End Sub
    Private Sub VisibilitàCampiRiserva(ByVal valore As Boolean)
        LblRiserva.Visible = valore
        LblDataRiserva.Visible = valore
        LblNoRiserva.Visible = valore
        LblDataNORiserva.Visible = valore
    End Sub

    Private Function AnnullaAccordo()
        '***Generata da Gianluigi Paesani in data:07/05/04
        '***Questa routine gestisce l'eliminazione della relazione tramite la 
        '***cancellazione logica di essa (con la vorizzazione del campo datafinevalidità) lasciandone traccia.
        Dim mycomm As Data.SqlClient.SqlCommand 'esegue update
        Dim strquery As String 'variabile generica SQL
        msgErrore.Text = String.Empty
        msgConferma.Text = String.Empty

        '***********************************************************************
        '* Modificato il 16/09/2005 Da Federico Cicchinelli
        '* Controllo che non sia stata Generata Utenza e Password
        '***********************************************************************
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader("Select * From VW_Account_Utenti Where Identificativo = '" & lblIdEnte.Value & "' And Not Password Is Null", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = False Or (Mid(Session("Utente"), 1, 1) = "U" Or Mid(Session("Utente"), 1, 1) = "R") Then
            dtrgenerico.Close()
            dtrgenerico = Nothing

            '*****************************************************************************************
            'Modificato il 7/09/2005 da Amilcare Paolella
            '*****************************************************************************************
            'Prelevo il C.F. e la P.I. presenti nei campi CodiceFiscale e PartitaIVA dell'Ente da annullare
            strquery = "Select CodiceFiscale From Enti where IdEnte='" & lblIdEnte.Value & "'"
            Dim strCodFis As String  'Codice Fiscale
            'Dim strPartIva As String 'Partita IVA
            Dim dtrCodFis As Data.SqlClient.SqlDataReader
            dtrCodFis = ClsServer.CreaDatareader(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrCodFis.Read()
            strCodFis = Trim(IIf(IsDBNull(dtrCodFis.GetValue(0)), "", dtrCodFis.GetValue(0)))
            'strPartIva = Trim(IIf(IsDBNull(dtrCodFis.GetValue(1)), "", dtrCodFis.GetValue(1)))
            dtrCodFis.Close()
            dtrCodFis = Nothing
            '*****************************************************************************************
            'Inserire controllo sedi utilizzate per progetto


            'inizio controllo per constatare se vi siano sedi associate all'ente OK
            dtrgenerico = ClsServer.CreaDatareader("select idAssociaEntiRelazioniSedi from AssociaEntiRelazioniSedi a" & _
                        " inner join entirelazioni b on a.IdEnteRelazione=b.IDEnteRelazione" & _
                        " where b.identerelazione=" & Request.QueryString("identerelazione") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = False Then 'se non vi sono sedi eseguo eliminazione
                'eseguo comando dopo controllo formale
                dtrgenerico.Close()
                dtrgenerico = Nothing
                If ddlrelazione.SelectedItem.Text <> " Selezionare " Then

                    'Inserisco le informazioni del vecchio stato dell'accordo in cronologia **************************************
                    strquery = "INSERT INTO CronologiaEntiStati (IDEnte, IDStatoEnte, DataCronologia, UsernameAccreditatore, IDTipoCronologia) " & _
                               "SELECT enti.IDEnte, enti.IDStatoEnte,GETDATE(),'" & Session("Utente") & "',0 " & _
                               "FROM enti INNER JOIN entirelazioni ON enti.IDEnte = entirelazioni.IDEnteFiglio " & _
                               "WHERE (entirelazioni.IDEnteRelazione = " & Request.QueryString("identerelazione") & ")"
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    mycomm.Dispose()
                    '*************************************************************************************************************

                    strquery = "update entirelazioni set datafinevalidità=getdate() where identerelazione=" & Request.QueryString("identerelazione") & ""
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    mycomm.Dispose()
                    'Modifico stato a Sospeso codice fiscale e partita iva
                    'Controllo se il codice fiscale o la partita iva siano nulli; in tal caso non modifico il dato inn archivio
                    If strCodFis.ToString = "" Then
                        strquery = "update enti set idstatoente=(select idstatoente from statienti where sospeso=1)" & _
                                                                       " Where idente=" & lblIdEnte.Value & ""
                        'ElseIf strCodFis.ToString = "" And strPartIva.ToString <> "" Then
                        '    strquery = "update enti set idstatoente=(select idstatoente from statienti where sospeso=1)," & _
                        '                                                   "PartitaIVA=null,PartitaIVAArchivio='" & strPartIva.ToString & "'" & _
                        '                                                   " Where idente=" & lblIdEnte.Text & ""
                    Else
                        strquery = "update enti set idstatoente=(select idstatoente from statienti where sospeso=1)," & _
                                                                       "CodiceFiscale=null,CodiceFiscaleArchivio='" & strCodFis.ToString & "'" & _
                                                                       " Where idente=" & lblIdEnte.Value & ""
                        'Else
                        '    strquery = "update enti set idstatoente=(select idstatoente from statienti where sospeso=1)," & _
                        '                               "CodiceFiscale=null,CodiceFiscaleArchivio='" & strCodFis.ToString & "'," & _
                        '                               "PartitaIVA=null,PartitaIVAArchivio='" & strPartIva.ToString & "'" & _
                        '                               " Where idente=" & lblIdEnte.Text & ""
                    End If
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()

                    'Aggiunto il 10/03/2008 da Simona Cordella
                    'Creo Cronologia della sede modificata
                    CronologiaSedeAttuazione()
                    CronologiaEntiSedi()
                    'Aggiunto da Alessandra Taballione il 11/07/2005
                    ' Cancello sedi e sedi di attuazione
                    'sedi 
                    strquery = " Update entisedi set idstatoentesede=" & IIf(Session("okDelete") = "TRUE", 2, 3) & " " & _
                    " from entisedi " & _
                    " where(idente = " & lblIdEnte.Value & ")"
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    'sedi attuazioni Proprie
                    strquery = "Update entisediattuazioni set idstatoentesede=" & IIf(Session("okDelete") = "TRUE", 2, 3) & " " & _
                    " from entisediattuazioni " & _
                    " where identesede in (select identesede " & _
                    " from entisedi " & _
                    " where idente=" & lblIdEnte.Value & ")"
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    mycomm.Dispose()
                    '******** AGG. DA simona cordella il  18/08/2009**********
                    'verifica se esistono delle variazioni dell'accordo da valutare
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    strsql = "Select * from EntiVariazioniAccordi where identefiglio = " & lblIdEnte.Value & " AND LAVORATO =0"
                    dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    If dtrgenerico.HasRows = True Then
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        'cancello recorD IN ENTIVARIAZIONIACCORDI
                        strquery = " DELETE from EntiVariazioniAccordi where identefiglio = " & lblIdEnte.Value & " AND LAVORATO =0"
                        mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        mycomm.ExecuteNonQuery()
                    End If
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    '*************************************************

                    BloccaMaschera()
                    msgConferma.Text = "La relazione è stata eliminata con successo"
                    lblStatoAccordo.Text = "Annullato"
                    lblStato.Text = "Sospeso"
                    CmdSalva.Visible = False
                    cmdElimina.Visible = False
                    EliminaUtenza()
                End If
            Else 'se vi sono sedi avverto utente
                If ddlrelazione.SelectedItem.Text <> " Selezionare " Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing

                    'Inserisco le informazioni del vecchio stato dell'accordo in cronologia **************************************
                    strquery = "INSERT INTO CronologiaEntiStati (IDEnte, IDStatoEnte, DataCronologia, UsernameAccreditatore, IDTipoCronologia) " & _
                              "SELECT enti.IDEnte, enti.IDStatoEnte,GETDATE(),'" & Session("Utente") & "',0 " & _
                              "FROM enti INNER JOIN entirelazioni ON enti.IDEnte = entirelazioni.IDEnteFiglio " & _
                              "WHERE (entirelazioni.IDEnteRelazione = " & Request.QueryString("identerelazione") & ")"
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    mycomm.Dispose()

                    'Modificato da Alessandra taballione il 10/06/2005
                    'Elimino Inclusione sediAttuazione
                    strsql = "Delete from AssociaEntiRelazioniSediattuazioni  where identerelazione=" & Request.QueryString("identerelazione") & "  "
                    mycomm = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    mycomm.Dispose()
                    'Elimino Inclusione sedi
                    strsql = "Delete from AssociaEntiRelazioniSedi where identerelazione=" & Request.QueryString("identerelazione") & "  "
                    mycomm = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    mycomm.Dispose()
                    '
                    strquery = "update entirelazioni set datafinevalidità=getdate() where identerelazione=" & Request.QueryString("identerelazione") & ""
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    mycomm.Dispose()
                    'Modifico stato a Sospeso codice fiscale e partita iva
                    'Controllo se il codice fiscale o la partita iva siano nulli; in tal caso non modifico il dato inn archivio
                    If strCodFis.ToString = "" Then
                        strquery = "update enti set idstatoente=(select idstatoente from statienti where sospeso=1)" & _
                                                                       " Where idente=" & lblIdEnte.Value & ""

                    Else
                        strquery = "update enti set idstatoente=(select idstatoente from statienti where sospeso=1)," & _
                                                                       "CodiceFiscale=null,CodiceFiscaleArchivio='" & strCodFis.ToString & "'" & _
                                                                       " Where idente=" & lblIdEnte.Value & ""
                    End If
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()

                    'Aggiunto il 10/03/2008 da Simona Cordella
                    'Creo Cronologia della sede modificata
                    CronologiaSedeAttuazione()
                    CronologiaEntiSedi()

                    'Aggiunto da Alessandra Taballione il 11/07/2005
                    ' Cancello sedi e sedi di attuazione
                    'sedi 
                    strquery = " Update entisedi set idstatoentesede=" & IIf(Session("okDelete") = "TRUE", 2, 3) & " " & _
                    " from entisedi " & _
                    " where(idente = " & lblIdEnte.Value & ")"
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    'sedi attuazioni Proprie
                    strquery = "Update entisediattuazioni set idstatoentesede=" & IIf(Session("okDelete") = "TRUE", 2, 3) & " " & _
                    " from entisediattuazioni " & _
                    " where identesede in (select identesede " & _
                    " from entisedi " & _
                    " where idente=" & lblIdEnte.Value & ")"
                    mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    mycomm.ExecuteNonQuery()
                    mycomm.Dispose()

                    '******** AGG. DA simona cordella il  18/08/2009**********
                    'verifica se esistono delle variazioni dell'accordo da valutare
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    strsql = "Select * from EntiVariazioniAccordi where identefiglio = " & lblIdEnte.Value & " AND LAVORATO =0"
                    dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    If dtrgenerico.HasRows = True Then
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        'cancello recorD IN ENTIVARIAZIONIACCORDI
                        strquery = " DELETE from EntiVariazioniAccordi where identefiglio = " & lblIdEnte.Value & " AND LAVORATO =0"
                        mycomm = New SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        mycomm.ExecuteNonQuery()
                    End If
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    '*************************************************


                    BloccaMaschera()
                    msgConferma.Text = "La relazione è stata eliminata con successo"
                    lblStatoAccordo.Text = "Annullato"
                    lblStato.Text = "Sospeso"

                    'Modificato da s.c. per nuovo accreditamento
                    'cmdripristina.Visible = True
                    'cmdripristina.Visible = False
                    CmdSalva.Visible = False
                    cmdElimina.Visible = False
                    EliminaUtenza()
                    ''''''''''''''''''''''VediAssegnaUtenza()
                End If
                'Se sedi incluse elimino inclusione sedi ente da annullare accordo
                'MessaggiAlert("Impossibile eliminare la relazione, risultano sedi associate. Procedere prima con il rilascio di tali sedi")
                'Imgerrore.Visible = True
            End If
        Else
            dtrgenerico.Close()
            'Inserisco la richiesta di Svincolamento che dovrà essere confermata dall'ente figlio
            strsql = "Insert Into RescissioniAccordi (IdEnte,DataRichiesta,UtenteRichiesta,DataConferma,UtenteConferma,Stato) Values (" & _
                     "'" & lblIdEnte.Value & "',CONVERT(DateTime,GetDate(),103),'" & Session("Utente") & "',NULL,NULL,0)"
            ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            msgConferma.Text = "La richiesta di rescissione del vincolo è stata inviata con successo."
            cmdElimina.Visible = False
        End If
    End Function

    Sub CaricaCombo(ByVal AlboEnte As String)
        'Caricamento delle combo
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        ddlTipologia.Items.Add("")
        ddlTipologia.Items.Add("Pubblico")
        ddlTipologia.Items.Add("Privato")

        If AlboEnte = "SCN" Then
            strsql = "select '' descrizione , 0 idtipologieenti union select descrizione, idtipologieenti from tipologieenti where privato = 0 order by idtipologieenti"
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            ddlGiuridica.DataSource = dtrgenerico
            ddlGiuridica.DataValueField = "idtipologieenti"
            ddlGiuridica.DataTextField = "Descrizione"
            ddlGiuridica.DataBind()
        End If
        'ddlGiuridica.Items.Add("")
        'ddlGiuridica.Items.Add("Amministrazione Statale")
        'ddlGiuridica.Items.Add("Enti Locali")
        'ddlGiuridica.Items.Add("Altri Enti Pubblici")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'popolo combo relazioni
        'Aggiunto da Alessandra Taballione il 12/07/2005
        'Verifico de Ente di Prima o secondaClasse altrimenti No faccio vadere Accordo in Partenariato
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'modificato il 18/07/2017 classi /ALBO ENTE

        strsql = "select entiinpartenariato " & _
        " from classiaccreditamento " & _
        " inner join enti on enti.idclasseAccreditamentoRichiesta = classiaccreditamento.idclasseAccreditamento " & _
        " where idente=" & Session("Idente") & " "
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()
        If dtrgenerico("entiinpartenariato") = True Then
            strsql = "select idtiporelazione, tiporelazione from tipirelazioni WHERE"
        Else
            If Request.QueryString("azione") = "Ins" And txtmodificaqueristringdiINS.Value <> "MOD" Then
                strsql = " SELECT idtiporelazione, tiporelazione from tipirelazioni where idtiporelazione <> 4 AND "
            Else
                strsql = " SELECT idtiporelazione, tiporelazione from tipirelazioni where "
            End If
            'strsql = "select idtiporelazione, tiporelazione from tipirelazioni union select '0', 'Selezionare' from tipirelazioni"
            'strsql = "select idtiporelazione, tiporelazione from tipirelazioni where idtiporelazione <> 4  union select '0', 'Selezionare' from tipirelazioni"
        End If
        If AlboEnte = "" Then
            strsql &= " (Albo = 'SCU' OR Albo is null)"
        Else
            strsql &= " (Albo = '" & AlboEnte & "' OR Albo is null)"
        End If
        strsql &= " UNION select '0', 'Selezionare' from tipirelazioni"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        ddlrelazione.DataSource = dtrgenerico
        ddlrelazione.DataValueField = "idtiporelazione"
        ddlrelazione.DataTextField = "tiporelazione"
        ddlrelazione.DataBind()
        dtrgenerico.Close()
        dtrgenerico = Nothing
        'Generato da Alessandra Taballione il 26/02/2005
        'mi salvo la data odierna dal server
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Sub CaricaSettori(ByVal AlboEnte As String)
        'modificato il 18/07/2017  settori/ALBO ENTE
        'aggiunto il 19/08/2014 da Simona Cordella
        Dim dtsSettori As DataSet
        Dim strSql As String
        strSql = "select IDMacroAmbitoAttività, Codifica, Codifica + ' - ' + MacroAmbitoAttività as MacroAmbitoAttività, IDIperAmbitoAttività FROM macroambitiattività"
        'strSql = "select IDMacroAmbitoAttività, Codifica, MacroAmbitoAttività, IDIperAmbitoAttività FROM macroambitiattività"
        If AlboEnte = "" Then
            strSql &= " WHERE ALBO='SCU' OR ALBO IS NULL"
        Else
            strSql &= " WHERE ALBO='" & AlboEnte & "' OR ALBO IS NULL"
        End If
        strSql &= " order by Codifica"
        dtsSettori = ClsServer.DataSetGenerico(strSql, Session("conn"))
        'controllo se ci sono dei record
        If dtsSettori.Tables(0).Rows.Count > 0 Then
            dtgSettori.DataSource = dtsSettori
            dtgSettori.DataBind()
        End If
    End Sub

    Private Sub LoadMaschera()
        Dim abilitato As Integer
        Dim dati As Integer
        Dim annullamodifica As Integer
        Dim visualizzadatiaccreditati As Integer
        Dim annullacancellazione As Integer
        Dim pRiserva As Integer
        'Dim pTogliRiserva As Integer
        Dim messaggio As String
        Dim DA As New SqlDataAdapter
        Dim dtEnte As New DataTable

        Session("EsperienzeAreeSettoreAccordo") = Nothing
        Session("IDClasseAccreditamentoAccordo") = Nothing
        Session("LoadedStatutoAccordo") = Nothing
        Session("LoadedACAccordo") = Nothing
        Session("LoadedDeliberaAdesioneAccordo") = Nothing
        Session("LoadedCartaImpegnoEticoAccordo") = Nothing
        Session("LoadedCartaImpegnoAccordo") = Nothing
        Session("IdEnteFaseArt") = Nothing
        Session("ElencoSettoriDBAccordo") = Nothing

        '1. RICHIAMO STORE CHE VERIFICA L'ACCESSO MASCHERA DELL' ENTE),
        Accesso_Maschera_EnteAccordo(Session("TipoUtente"), Session("IdEnte"), IIf(Request.QueryString("id") = Nothing, 0, Request.QueryString("id")), abilitato, dati, annullamodifica, annullacancellazione, visualizzadatiaccreditati, pRiserva, messaggio)

        'Verifico se l'utente può effettuare un modifica senza effettuare un adeguamento

        If Request.QueryString("azione") = "Mod" Then

            Session("idEnteAccoglienza") = Request.QueryString("id")
            Session("idEnteRelazione") = Request.QueryString("identerelazione")
            'ADC 02/03/2022
            strsql = " SELECT IDENTE,IDSTATOENTE,CODICEREGIONE,isnull(flagforzaturaaccreditamento ,0) FLGABILITAZIONE,DATACREAZIONERECORD FROM ENTI WHERE IDENTE = @IdEnte "

            Dim SqlCmd As New SqlClient.SqlCommand
            Try
                SqlCmd.CommandText = strsql
                SqlCmd.CommandType = CommandType.Text
                SqlCmd.Connection = Session("Conn")
                SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("idEnteAccoglienza")
                DA.SelectCommand = SqlCmd
                DA.Fill(dtEnte)
            Catch ex As Exception
                msgErrore.Text = ex.Message
            End Try

            Dim CodRegione, idStatoEnte As String
            If dtEnte IsNot Nothing AndAlso dtEnte.Rows.Count > 0 Then
                Dim DataCreazioneEnte = CDate(dtEnte.Rows(0)("DATACREAZIONERECORD"))
                Session("DataCreazioneEnte") = DataCreazioneEnte
                If Not IsDBNull(dtEnte.Rows(0)("IDSTATOENTE")) Then
                    idStatoEnte = dtEnte.Rows(0)("IDSTATOENTE")
                    Select Case idStatoEnte
                        Case 3, 9 'Attivo o adeguamento
                            cmdModNoAdeguamento.Visible = True
                        Case 8 'Istruttoria
                            If Not IsDBNull(dtEnte.Rows(0)("CODICEREGIONE")) Then
                                CodRegione = dtEnte.Rows(0)("CODICEREGIONE")
                                If Not CodRegione Like ("CP%") Then
                                    cmdModNoAdeguamento.Visible = True
                                Else
                                    cmdModNoAdeguamento.Visible = False
                                End If
                            End If
                        Case Else
                            cmdModNoAdeguamento.Visible = False
                    End Select
                End If
            End If
        End If
        '2.PERSONALIZZO CAMPI NON VISIBILI SE SONO ENTE O UNSC/REGIONE
        PersonalizzoNoVisibilitàCampi()
        If Session("TipoUtente") <> "U" Then
            CmdInforScu1.Visible = False
            DIVdataaccr.Visible = False
        Else
            DIVdataaccr.Visible = True
        End If

        'CARICA DATI
        If Request.QueryString("id") <> Nothing Then 'modalità inserimento dati
            If dati = 0 Then ' -- 0: dati tabelle reali		1: dati tabelle variazioni
                isVariazione = False
                PopolaMaschera()
                CaricaGrid()
            Else
                isVariazione = True
                PopolaMascheraVariazione() 'PopolaMaschera dati variazione
                CaricaGrid()
            End If

            If messaggio <> "" Then
                msgErrore.Visible = True
                msgErrore.Text = messaggio
            End If

            If lblStatoAccordo.Text = "Attivo" And lblStato.Text = "Registrato" And abilitato <> 0 Then
                txtdenominazione.ReadOnly = False
                txtdenominazione.BackColor = Color.White
                txtCodFis.ReadOnly = False
                txtCodFis.BackColor = Color.White
            Else
                txtdenominazione.ReadOnly = True
                txtdenominazione.BackColor = Color.LightGray
                txtCodFis.ReadOnly = True
                txtCodFis.BackColor = Color.LightGray
            End If
        End If

        '3.ABILITO/DISABILITO MASCHERA SE E' MODIFICABILE O IN LETTERA
        If abilitato = 0 Then ' -- 0: maschera sola lettura		1: maschera in modifica
            BloccaMaschera()
            CmdSalva.Visible = False
            cmdElimina.Visible = False
            If messaggio <> "" Then
                msgErrore.Visible = True
                msgErrore.Text = messaggio
            End If
        Else
            SBloccaMaschera()
            Dim dtrGen As SqlClient.SqlDataReader
            Dim blnAdeguamento As Boolean
            If Not dtrGen Is Nothing Then
                dtrGen.Close()
                dtrGen = Nothing
            End If
            strsql = "select IdEnteFase from EntiFasi where TipoFase = 2 and GETDATE() between DataInizioFase and DataFineFase and IdEnte = " & Session("IdEnte")
            dtrGen = ClsServer.CreaDatareader(strsql, Session("Conn"))
            If dtrGen.HasRows = True Then
                blnAdeguamento = True
            Else
                blnAdeguamento = False
            End If
            dtrGen.Close()
            dtrGen = Nothing

            strsql = "SELECT VALORE  FROM Configurazioni where Parametro='BLOCCO_ACCREDITAMENTO'"
            If Not dtrGen Is Nothing Then
                dtrGen.Close()
                dtrGen = Nothing
            End If
            dtrGen = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrGen.Read()

            If dtrGen("Valore") = "SI" And Session("TipoUtente") = "E" And blnAdeguamento = False Then
                cmdElimina.Visible = False
            End If
            If Not dtrGen Is Nothing Then
                dtrGen.Close()
                dtrGen = Nothing
            End If
            'msgConferma.Visible = True
            'msgConferma.CssClass = "msgErrore"
        End If

        If annullamodifica = 0 Then '-- 0: funzione non abilitata	1: funzione abilitata
            'pulsante
            cmdAnnullaModifica.Visible = False
        Else
            cmdAnnullaModifica.Visible = True
        End If
        If annullacancellazione = 0 Then  '-- 0: funzione non abilitata	1: funzione abilitata
            'pulsante
            cmdAnnullaCancellazione.Visible = False
        Else
            cmdAnnullaCancellazione.Visible = True
        End If
        If visualizzadatiaccreditati = 0 Then '-- 0: funzione non abilitata	1: funzione abilitata
            'pulsante
            cmdVisualizzaDatiAccreditati.Visible = False
        Else
            cmdVisualizzaDatiAccreditati.Visible = True
        End If

        If pRiserva = 0 Then '-- 0: funzione non abilitata	1: funzione abilitata
            'pulsante
            btnRiserva.Visible = False
        Else
            btnRiserva.Visible = True
        End If

        If lblIdEnte.Value IsNot Nothing AndAlso lblIdEnte.Value.Trim() <> String.Empty Then
            'CaricaEntiSettori()
        End If
        'If pTogliRiserva = 0 Then '-- 0: funzione non abilitata	1: funzione abilitata
        '    'pulsante
        '    imgTogliRiserva.Visible = False
        'Else
        '    imgTogliRiserva.Visible = True
        'End If

        AttivaCronologia()
        If Request.QueryString("id") <> Nothing Then
            AttivaPulsantiModArt2Art10()
        End If
    End Sub

    Private Sub AttivaCronologia()
        For Each gRow As GridViewRow In dtgSettori.Rows

            Dim btnCronologia1 As Button = DirectCast(gRow.FindControl("btnCron1"), Button)
            Dim btnCronologia2 As Button = DirectCast(gRow.FindControl("btnCron2"), Button)
            Dim btnCronologia3 As Button = DirectCast(gRow.FindControl("btnCron3"), Button)
            Dim txtDesEsperienza3 As TextBox = DirectCast(gRow.FindControl("txtDesEsperienza3"), TextBox)
            Dim txtDesEsperienza2 As TextBox = DirectCast(gRow.FindControl("txtDesEsperienza2"), TextBox)
            Dim txtDesEsperienza1 As TextBox = DirectCast(gRow.FindControl("txtDesEsperienza1"), TextBox)

            If Not txtDesEsperienza3 Is Nothing AndAlso txtDesEsperienza3.Visible = True AndAlso txtDesEsperienza3.Text <> "" Then btnCronologia3.Visible = True And Not Session("TipoUtente") = "E" Else btnCronologia3.Visible = False
            If Not txtDesEsperienza2 Is Nothing AndAlso txtDesEsperienza2.Visible = True AndAlso txtDesEsperienza2.Text <> "" Then btnCronologia2.Visible = True And Not Session("TipoUtente") = "E" Else btnCronologia2.Visible = False
            If Not txtDesEsperienza1 Is Nothing AndAlso txtDesEsperienza1.Visible = True AndAlso txtDesEsperienza1.Text <> "" Then btnCronologia1.Visible = True And Not Session("TipoUtente") = "E" Else btnCronologia1.Visible = False

        Next
    End Sub

    Private Sub AttivaPulsantiModArt2Art10()
        'se utente di tipo dipartimento rende visibili in maschera i pulsanti per abilitare/disabilitare le modifiche Art2/Art10

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_ELENCO_ABILITAZIONI_MOD_ENTE_ART2_ART10_ENTE_FIGLIO]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = IIf(Request.QueryString("id") = Nothing, 0, Integer.Parse(Request.QueryString("id")))

            sqlDAP.SelectCommand.Parameters.Add("@Utente", SqlDbType.NVarChar, 10).Value = Session("Utente")
            sqlDAP.Fill(dataSet)

            If dataSet.Tables(0).Rows.Count > 0 Then
                'ciclo sulla griglia
                For Each gRow As GridViewRow In dtgSettori.Rows

                    Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                    Dim btnAbilita As Button = DirectCast(gRow.FindControl("btnAbArt2Art10"), Button)
                    Dim btnDisabilita As Button = DirectCast(gRow.FindControl("btnDisabArt2Art10"), Button)

                    'default
                    If btnAbilita IsNot Nothing Then btnAbilita.Visible = False
                    If btnDisabilita IsNot Nothing Then btnDisabilita.Visible = False

                    Dim _row As DataRow() = dataSet.Tables(0).Select("IdMacroAmbitoAttività=" + HFIdMacroAttivita.Value) 'ricerco il settore in quelli per cui abilitare/disabilitare le modifiche
                    If _row.Count > 0 Then
                        If _row(0).Item("abilitato") = "1" Then btnDisabilita.Visible = True Else btnAbilita.Visible = True
                    End If

                Next
            End If

        Catch ex As Exception
            msgErrore.ForeColor = Color.Red
            msgErrore.Text = "Errore nel recupero informazioni"
            Exit Sub
        End Try

    End Sub

    Private Sub CronologiaSedeAttuazione()
        Dim rstgenerico As SqlClient.SqlCommand
        'Aggiunto il 10/03/2008 da Simona Cordella
        'Prima di effettuare il cambio dello stato Inserisco il vecchio stato nella CronologiaEntiSedi
        strsql = "Insert into CronologiaEntiSediAttuazione (identeSedeAttuazione,idstatoentesede,dataCronologia,idtipocronologia,UsernameStato)" & _
                " Select esa.identeSedeAttuazione, esa.idstatoEnteSede,getdate(),0,'" & Session("Utente") & "' " & _
                " from entiSediAttuazioni esa " & _
                " INNER JOIN EntiSedi ON esa.IdEnteSede = entisedi.identesede " & _
                " where EntiSedi.idente = " & lblIdEnte.Value & ""

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        rstgenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
    End Sub

    Private Sub CronologiaEntiSedi()
        Dim rstgenerico As SqlClient.SqlCommand
        'Aggiunto il 10/03/2008 da Simona Cordella
        'Prima di effettuare il cambio dello stato Inserisco il vecchio stato nella CronologiaEntiSedi
        strsql = "insert into CronologiaEntiSedi (identeSede,idstatoentesede,dataCronologia,idtipocronologia,UsernameStato)" & _
            " select IdEnteSede, idstatoEnteSede,getdate(),0,'" & Session("Utente") & "' " & _
            " from entiSedi where idente = " & lblIdEnte.Value & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        rstgenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
    End Sub

    Private Sub BloccaMaschera()
        txtBlocco.Value = "TRUE"


        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim cmdModifica As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
            cmdModifica.Visible = False
            Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
            check.Enabled = False
        Next

        dtgSettori.Enabled = False

        txtCodFisArchivio.ReadOnly = True
        txtCodFisArchivio.BackColor = Color.Gainsboro
        txtdenominazione.ReadOnly = True
        txtdenominazione.BackColor = Color.Gainsboro
        txtCodFis.ReadOnly = True
        txtCodFis.BackColor = Color.Gainsboro
        ddlGiuridica.Enabled = False
        ddlTipologia.Enabled = False
        txtDataCostituzione.ReadOnly = True
        txtDataCostituzione.BackColor = Color.Gainsboro
        txtDataScadenza.ReadOnly = True
        txtDataScadenza.BackColor = Color.Gainsboro
        txtDataStipula.ReadOnly = True
        txtDataStipula.BackColor = Color.Gainsboro
        txtprefisso.ReadOnly = True
        txtprefisso.BackColor = Color.Gainsboro
        txtTelefono.ReadOnly = True
        txtTelefono.BackColor = Color.Gainsboro
        txthttp.ReadOnly = True
        txthttp.BackColor = Color.Gainsboro
        txtEmail.ReadOnly = True
        txtEmail.BackColor = Color.Gainsboro
        txtFax.ReadOnly = True
        txtFax.BackColor = Color.Gainsboro
        txtPrefissoFax.ReadOnly = True
        txtPrefissoFax.BackColor = Color.Gainsboro
        ddlrelazione.Enabled = False
        'Disabilitazioni campi dati sede principale
        txtIndirizzo.ReadOnly = True
        txtIndirizzo.BackColor = Color.Gainsboro
        TxtDettaglioRecapito.ReadOnly = True
        TxtDettaglioRecapito.BackColor = Color.Gainsboro
        txtCivico.ReadOnly = True
        txtCivico.BackColor = Color.Gainsboro
        ddlComune.Enabled = False
        ddlComune.BackColor = Color.Gainsboro
        txtCAP.ReadOnly = True
        txtCAP.BackColor = Color.Gainsboro
        txtAltraTipoEnte.ReadOnly = True
        txtAltraTipoEnte.BackColor = Color.Gainsboro

        txtDataNominaRL.Enabled = False
        txtEmailpec.Enabled = False
        txtCodFiscRL.Enabled = False
        ddlProvincia.Enabled = False
        ChkEstero.Enabled = False
        CmdSalva.Visible = False
        chkSenzaScopoLucro.Enabled = False
        chkFiniIstituzionali.Enabled = False
        chkAttivita.Enabled = False
        cmdAllegaAttoCostitutivo.Enabled = False
        cmdAllegaAttoCostitutivo.BackColor = Color.Gainsboro
        cmdAllegaStatuto.Enabled = False
        cmdAllegaStatuto.BackColor = Color.Gainsboro
        cmdAllegaImpegnoEtico.Enabled = False
        cmdAllegaImpegnoEtico.BackColor = Color.Gainsboro
        cmdAllegaDeliberaAdesione.Enabled = False
        cmdAllegaDeliberaAdesione.BackColor = Color.Gainsboro
        cmdAllegaImpegno.Enabled = False
        cmdAllegaImpegno.BackColor = Color.Gainsboro

        btnEliminaAttoCostitutitvo.Visible = False
        btnModificaAttoCostitutivo.Visible = False
        btnEliminaCI.Visible = False
        btnModificaCI.Visible = False
        btnEliminaCIE.Visible = False
        btnModificaCIE.Visible = False
        btnEliminaDeliberaAdesione.Visible = False
        btnModificaDeliberaAdesione.Visible = False
        btnEliminaStatuto.Visible = False
        btnModificaStatuto.Visible = False

        'controllo possibilità sostituzione art2/art10
        AbilitaSostituzioneArt2Art10()


    End Sub

    Private Sub AbilitaSostituzioneArt2Art10()
        Dim intIdEnte As Integer
        Dim intIdEnteAccoglienza As Integer
        Dim _settoriAbilitati As Boolean = False

        intIdEnte = Session("IdEnte")
        intIdEnteAccoglienza = IIf(Request.QueryString("id") = Nothing, 0, Request.QueryString("id"))

        'ABILITA SOSTITUZIONE ART2/ART10

        Try
            Dim SqlCmd As SqlClient.SqlCommand


            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_INTEGRAZIONE_ART2_ART10_ACCOGLIENZA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = intIdEnte
            SqlCmd.Parameters.Add("@IdEnteFiglio", SqlDbType.Int).Value = intIdEnteAccoglienza

            Dim sparamIdEnteFase As SqlClient.SqlParameter
            sparamIdEnteFase = New SqlClient.SqlParameter
            sparamIdEnteFase.ParameterName = "@IdEnteFase"
            'sparam1.Size = 100
            sparamIdEnteFase.SqlDbType = SqlDbType.Int
            sparamIdEnteFase.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparamIdEnteFase)

            Dim sparamIdEnteFaseArt_2_10 As SqlClient.SqlParameter
            sparamIdEnteFaseArt_2_10 = New SqlClient.SqlParameter
            sparamIdEnteFaseArt_2_10.ParameterName = "@IdEnteFaseArt_2_10"
            'sparam1.Size = 100
            sparamIdEnteFaseArt_2_10.SqlDbType = SqlDbType.Int
            sparamIdEnteFaseArt_2_10.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparamIdEnteFaseArt_2_10)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@AbilitaAttoCostitutivo"
            'sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.TinyInt
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@AbilitaCI"
            'sparam1.Size = 100
            sparam2.SqlDbType = SqlDbType.TinyInt
            sparam2.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@AbilitaCIE"
            'sparam1.Size = 100
            sparam3.SqlDbType = SqlDbType.TinyInt
            sparam3.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam3)

            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@AbilitaDeliberaAdesione"
            'sparam1.Size = 100
            sparam4.SqlDbType = SqlDbType.TinyInt
            sparam4.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam4)

            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@AbilitaStatuto"
            'sparam1.Size = 100
            sparam5.SqlDbType = SqlDbType.TinyInt
            sparam5.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam5)

            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@AbilitaSettori"
            sparam6.Size = 50
            sparam6.SqlDbType = SqlDbType.VarChar
            sparam6.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam6)

            SqlCmd.ExecuteNonQuery()

            Session("IdEnteFaseArt") = SqlCmd.Parameters("@IdEnteFaseArt_2_10").Value

            If SqlCmd.Parameters("@AbilitaAttoCostitutivo").Value = 1 Then
                btnModificaAttoCostitutivo.Visible = True
                rowAttoCostitutivo.Visible = True
                rowNoImpegnoEtico.Visible = False
            End If
            If SqlCmd.Parameters("@AbilitaCI").Value = 1 Then
                btnModificaCI.Visible = True
                rowCartaImpegno.Visible = True
                rowNoCartaImpegno.Visible = False
            End If
            If SqlCmd.Parameters("@AbilitaCIE").Value = 1 Then
                btnModificaCIE.Visible = True
                rowImpegnoEtico.Visible = True
                rowNoImpegnoEtico.Visible = False
            End If
            If SqlCmd.Parameters("@AbilitaDeliberaAdesione").Value = 1 Then
                btnModificaDeliberaAdesione.Visible = True
                rowDeliberaAdesione.Visible = True
                rowNoDeliberaAdesione.Visible = False
            End If
            If SqlCmd.Parameters("@AbilitaStatuto").Value = 1 Then
                btnModificaStatuto.Visible = True
                rowStatuto.Visible = True
                rowNoStatuto.Visible = False
            End If
            If Not String.IsNullOrEmpty(SqlCmd.Parameters("@AbilitaSettori").Value) Then
                _settoriAbilitati = AbilitaSettoriArt2Art10(SqlCmd.Parameters("@AbilitaSettori").Value)
            End If

            If SqlCmd.Parameters("@AbilitaAttoCostitutivo").Value = 1 Or
                SqlCmd.Parameters("@AbilitaCI").Value = 1 Or _
                SqlCmd.Parameters("@AbilitaDeliberaAdesione").Value = 1 Or _
                SqlCmd.Parameters("@AbilitaCIE").Value = 1 Or _
                SqlCmd.Parameters("@AbilitaStatuto").Value = 1 Or _
                _settoriAbilitati Then

                CmdSalvaDoc.Visible = True
                lblMessaggioArt2Art10.Text = "E' consentita l'integrazione dei dati/documenti richiesti dal Dipartimento con Art.2/Art.10"
                lblMessaggioArt2Art10.Visible = True
            Else
                CmdSalvaDoc.Visible = False
                lblMessaggioArt2Art10.Visible = False
            End If
        Catch ex As Exception

            msgErrore.Text = ex.Message

        End Try

    End Sub

    Function AbilitaSettoriArt2Art10(ElencoSettori As String) As Boolean

        Dim _ret As Boolean = False
        Dim _settori As String() = ElencoSettori.Split(",")

        If _settori.Length > 0 Then
            dtgSettori.Enabled = True
            For Each gRow As GridViewRow In dtgSettori.Rows
                Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                Dim divAree As HtmlGenericControl = gRow.Cells(2).FindControl("divArea")
                Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                If Array.Exists(_settori, Function(s) s = HFIdMacroAttivita.Value) Then
                    gRow.Enabled = True
                    'divAree.Style("display") = "block"
                    cmdInserisci.Visible = True
                    _ret = True
                Else
                    gRow.Enabled = False
                    'divAree.Style("display") = "none"
                    cmdInserisci.Visible = False
                End If
            Next
        End If

        Return _ret
    End Function

    Private Sub EliminaUtenza()
        Dim null As String
        null = "null"
        'modifica dell'Utenza dell'ente
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
        Try
            MyTransaction = CType(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), System.Data.SqlClient.SqlConnection).BeginTransaction(Session("IdEnte") & "_" & Session("IdEnte"))
            myCommand.Transaction = MyTransaction
            strsql = "Update Enti set "
            strsql = strsql & " password=" & null & " where idente=" & lblIdEnte.Value & ""
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()

            'Modificato da Federico Cicchinelli il 16/09/2005
            '*******************************************************************
            '* Elimina dalla Tabella EntiPassword
            '*******************************************************************
            'strsql = "Delete From EntiPassword Where IdEnte = '" & lblIdEnte.Text & "'"
            'myCommand.CommandText = strsql
            'myCommand.ExecuteNonQuery()
            '*******************************************************************

            'Alessandra Taballione il 08/03/2005
            'inserisco il Profilo del Nuovo Utente
            strsql = "delete from AssociaUtenteGruppo  where username='" & txtUtenza.Value & "'"
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
            MyTransaction.Commit()
        Catch ex As Exception
            Response.Write(strsql)
            Response.Write("<br>")
            Response.Write(ex.Message.ToString)
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("IdEnte"))
        End Try
        MyTransaction.Dispose()
        txtUtenza.Value = ""
        txtPassword.Value = ""
        ''''''''''''''''''''VediAssegnaUtenza()
        'Invio Comunicazione di elimininazione Utenza 
        'invioEmail("", txtemail.Text, "", "", "")
    End Sub

    Private Sub Accesso_Maschera_EnteAccordo(ByVal TipoUtente As String, ByVal IdEnte As Integer, ByVal IDEnteFiglio As Integer, ByRef abilitato As Integer, ByRef dati As Integer, ByRef annullamodifica As Integer, ByRef annullacancellazione As Integer, ByRef visualizzadatiaccreditati As Integer, ByRef pRiserva As Integer, ByRef messaggio As String)

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  25/07/2014
        'FUNZIONALITA': Verifica le condizioni di accreditamento e le eventuali modifiche in corso sui dati dell'ente
        '               e ritorna all'applicazione la configurazione da applicare alla maschera di gestion
        '               @abilitato  			    -- 0: maschera sola lettura		1: maschera in modifica
        '               @dati 					    -- 0: dati tabelle reali		1: dati tabelle variazioni
        '               @annullamodifica  			-- 0: funzione non abilitata	1: funzione abilitata
        '               @visualizzadatiaccreditati 	-- 0: funzione non abilitata	1: funzione abilitata
        '               @annullacancellazione       -- 0: funzione non abilitata	1: funzione abilitata
        '               @messaggio                  -- eventuale messaggio di ritorno da visualizzare all'utente



        Try
            Dim SqlCmd As SqlClient.SqlCommand
            '            Dim Dt As New DataTable
            '           Dim Da As New SqlClient.SqlDataAdapter(SqlCmd)

            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ACCESSOMASCHERA_ENTEFIGLIO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@TipoUtente", SqlDbType.VarChar).Value = TipoUtente
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            SqlCmd.Parameters.Add("@IdEnteFiglio", SqlDbType.Int).Value = IDEnteFiglio


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@abilitato"
            'sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.TinyInt
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@dati"
            'sparam1.Size = 100
            sparam2.SqlDbType = SqlDbType.TinyInt
            sparam2.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@annullamodifica"
            'sparam1.Size = 100
            sparam3.SqlDbType = SqlDbType.TinyInt
            sparam3.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam3)

            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@visualizzadatiaccreditati"
            'sparam1.Size = 100
            sparam4.SqlDbType = SqlDbType.TinyInt
            sparam4.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam4)

            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@annullacancellazione"
            'sparam1.Size = 100
            sparam5.SqlDbType = SqlDbType.TinyInt
            sparam5.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam5)


            Dim sparam7 As SqlClient.SqlParameter
            sparam7 = New SqlClient.SqlParameter
            sparam7.ParameterName = "@pRiserva"
            'sparam1.Size = 100
            sparam7.SqlDbType = SqlDbType.TinyInt
            sparam7.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam7)

       

            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@messaggio"
            sparam6.Size = 1000
            sparam6.SqlDbType = SqlDbType.VarChar
            sparam6.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam6)



            SqlCmd.ExecuteNonQuery()


            abilitato = SqlCmd.Parameters("@abilitato").Value
            dati = SqlCmd.Parameters("@dati").Value
            annullamodifica = SqlCmd.Parameters("@annullamodifica").Value
            visualizzadatiaccreditati = SqlCmd.Parameters("@visualizzadatiaccreditati").Value
            annullacancellazione = SqlCmd.Parameters("@annullacancellazione").Value
            pRiserva = SqlCmd.Parameters("@pRiserva").Value
            'pTogliRiserva = SqlCmd.Parameters("@pTogliRiserva").Value
            messaggio = SqlCmd.Parameters("@messaggio").Value
        Catch ex As Exception

            msgErrore.Text = ex.Message

        End Try

    End Sub

    Sub PersonalizzoNoVisibilitàCampi()
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'imgmes.Visible = False
        'msgErrore.Text = ""
        txtBlocco.Value = "FALSE"

        If Request.QueryString("azione") = "Ins" AndAlso txtmodificaqueristringdiINS.Value <> "MOD" Then
            'generato da Alessandra Taballione il 08/03/2005
            'Blocco della creazione Accordi su enti non abilitati
            dtrgenerico = ClsServer.CreaDatareader("select a.idclasseaccreditamento from enti a" &
                            " inner join classiaccreditamento b" &
                            " on a.idclasseaccreditamento=b.idclasseaccreditamento" &
                            " inner join classiaccreditamento c" &
                            " on a.idclasseaccreditamentorichiesta=c.idclasseaccreditamento" &
                            " where idente=" & Session("IdEnte") & "" &
                            " and (b.minsedi > 0 or c.minsedi> 0 )", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            'Modificata da Alessandra Taballione il 12/07/2005
            '" and (b.entiinpartenariato=1 or c.entiinpartenariato=1)", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = False Then
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    msgErrore.Text = "Attenzione l'Ente selezionato non può accedere alla gestione [ Accordo tra Enti ]"
                    msgErrore.Visible = True
                    CmdSalva.Visible = False
                    cmdElimina.Visible = False
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Exit Sub
                Else
                    msgErrore.Text = "Attenzione l'Ente non può accedere alla gestione [ Accordo tra Enti ]"
                    msgErrore.Visible = True
                    CmdSalva.Visible = False
                    cmdElimina.Visible = False
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Exit Sub
                End If
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
        dtrgenerico = ClsServer.CreaDatareader("select getdate() as dataOggi", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()
        txtdataoggi.Value = dtrgenerico("dataOggi")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        If Request.QueryString("azione") = "Ins" And txtmodificaqueristringdiINS.Value <> "MOD" Then
            lblTipoUtente.Value = Session("TipoUtente")
            PersonalizzaTasti()
            txtRichiedente.Value = Session("Utente")
            phStato.Visible = False
            lblClasseAccreditamento.Visible = False
            lblIdEnte.Value = "-1"
            dtgAttivitaEnte.Visible = False
            lblTitotloProgettiEnte.Visible = False
            '''''''''''''''''''''VediAssegnaUtenza()
        Else
            If Request.QueryString("Stato") = "Annullato" Then

                If lblStato.Text = "Chiuso" Then
                    dvCodiceFiscaleArchiviato.Visible = True

                End If
            End If

            PersonalizzaTasti()
            lblTipoUtente.Value = Session("TipoUtente")
            If (lblTipoUtente.Value = "U" Or lblTipoUtente.Value = "R") Then
                If lblIdEnte.Value = "" Then
                    lblIdEnte.Value = Request.QueryString("id")
                    Session("IdEnteAccoglienza") = Request.QueryString("id")
                End If
            Else
                If lblIdEnte.Value = "" Then
                    lblIdEnte.Value = Request.QueryString("id")
                    Session("IdEnteAccoglienza") = Request.QueryString("id")
                End If
                txtCodRegione.ReadOnly = True
                txtCodRegione.BackColor = Color.Gainsboro
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


        If lblTipoUtente.Value = "E" Then
            'Controllo che non sia gia stata fatta la richiesta di rescissione
            dtrgenerico = ClsServer.CreaDatareader("Select * from RescissioniAccordi Where IdEnte = '" & lblIdEnte.Value & "'", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = True Then
                cmdElimina.Visible = False
            End If
            dtrgenerico.Close()
        End If


        If lblStato.Text = "Attivo" Then
            txtdenominazione.ReadOnly = True
            txtdenominazione.BackColor = Color.Gainsboro
            txtCodFis.ReadOnly = True
            txtCodFis.BackColor = Color.Gainsboro

        End If
        If ddlTipologia.SelectedItem.Text = "Pubblico" Then
            'ddlGiuridica.Visible = True
            Response.Write("<input type=hidden name=chkGiuridica value=""true"">")
        Else
            'ddlGiuridica.Visible = False
            Response.Write("<input type=hidden name=chkGiuridica value=""false"">")
        End If

        'FZ controllo per disabilitare la maschera nel caso sia un'"R" che sta 
        'controllando i progetti di un' ente che nn ha la stessa regiojneee di competenza
        If ClsUtility.ControlloRegioneCompetenza(Session("TipoUtente"), Session("idEnte"), Session("Utente"), Session("IdStatoEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = False Then
            'BloccaMaschera()
            msgErrore.Text = "Attenzione, l'ente non è di propria competenza. Impossibile effettuare modifiche."
            CmdSalva.Visible = False
            'cmdripristina.Visible = False
            cmdElimina.Visible = False
        End If
        'FZ fine controllo
    End Sub

    Private Sub SBloccaMaschera()
        dtgSettori.Enabled = True

        'For Each gRow As GridViewRow In dtgSettori.Rows
        '    Dim cmdModifica As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
        '    cmdModifica.Visible = True
        'Next
        txtdenominazione.ReadOnly = False
        txtdenominazione.BackColor = Color.White
        txtCodFis.ReadOnly = False
        txtCodFis.BackColor = Color.White

        'imgCodFis.Visible = True
        'txtPartitaIva.ReadOnly = False
        'txtPartitaIva.BackColor = Color.White
        'imgPIVA.Visible = True
        ddlTipologia.Enabled = True
        txtDataCostituzione.ReadOnly = False
        txtDataCostituzione.BackColor = Color.White
        'imgcalendario.Visible = True
        txtDataScadenza.ReadOnly = False
        txtDataScadenza.BackColor = Color.White
        'imgCalScadenza.Visible = True
        txtDataStipula.ReadOnly = False
        txtDataStipula.BackColor = Color.White
        'imgCalStiplula.Visible = True
        txtprefisso.ReadOnly = False
        txtprefisso.BackColor = Color.White
        txtTelefono.ReadOnly = False
        txtTelefono.BackColor = Color.White
        txthttp.ReadOnly = False
        txthttp.BackColor = Color.White
        txtEmail.ReadOnly = False
        txtEmail.BackColor = Color.White
        txtFax.ReadOnly = False
        txtFax.BackColor = Color.White
        txtPrefissoFax.ReadOnly = False
        txtPrefissoFax.BackColor = Color.White
        ddlrelazione.Enabled = True
        ddlGiuridica.Enabled = True
        'Disabilitazioni campi dati sede principale
        txtIndirizzo.ReadOnly = False
        txtIndirizzo.BackColor = Color.White

        TxtDettaglioRecapito.ReadOnly = False
        TxtDettaglioRecapito.BackColor = Color.White

        txtCivico.ReadOnly = False
        txtCivico.BackColor = Color.White
        ddlComune.Enabled = True
        ddlComune.BackColor = Color.White
        'imgComune.Visible = True
        txtCAP.ReadOnly = False
        txtCAP.BackColor = Color.White

        txtDataNominaRL.Enabled = True
        txtEmailpec.Enabled = True
        txtCodFiscRL.Enabled = True
        ddlProvincia.Enabled = True
        ChkEstero.Enabled = True
        chkSenzaScopoLucro.Enabled = True
        chkFiniIstituzionali.Enabled = True
        chkAttivita.Enabled = True
        cmdAllegaAttoCostitutivo.Enabled = True
        cmdAllegaAttoCostitutivo.BackColor = Color.FromArgb(58, 79, 99)
        cmdAllegaStatuto.Enabled = True
        cmdAllegaStatuto.BackColor = Color.FromArgb(58, 79, 99)
        cmdAllegaImpegnoEtico.Enabled = True
        cmdAllegaImpegnoEtico.BackColor = Color.FromArgb(58, 79, 99)
        cmdAllegaDeliberaAdesione.Enabled = True
        cmdAllegaDeliberaAdesione.BackColor = Color.FromArgb(58, 79, 99)
        cmdAllegaImpegno.Enabled = True
        cmdAllegaImpegno.BackColor = Color.FromArgb(58, 79, 99)


    End Sub
    Private Sub CaricaDettaglioTipologiaEntiSCU(ByVal Tipologia As String, ByVal VengoDA As String)
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        If Tipologia = "" Then Exit Sub

        If VengoDA = "POPOLAMASCHERA" Then

            strsql = "select Privato,abilitata, isnull(ALBO,'SCU') as ALBO from tipologieenti where Descrizione= '" & Replace(Tipologia, "'", "''") & "' "

            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                If dtrgenerico("Privato") = True Then
                    strsql = "select '' descrizione , 0 idtipologieenti,null as Ordinamento union select descrizione, idtipologieenti,Ordinamento from tipologieenti where (privato =1 and abilitata=1 AND  isnull(ALBO,'SCU') ='SCU' and iscrizione = 1) or descrizione = (select tipologia from enti where IDEnte=0" & Request.QueryString("id") & ")  order by Ordinamento"
                Else
                    strsql = "select '' descrizione , 0 idtipologieenti,null as Ordinamento union select descrizione, idtipologieenti,Ordinamento from tipologieenti where (privato =0 and abilitata=1 AND  isnull(ALBO,'SCU') ='SCU' and iscrizione = 1) or descrizione = (select tipologia from enti where IDEnte=0" & Request.QueryString("id") & ")   order by Ordinamento"
                End If
            End If
        End If
        If VengoDA = "COMBOTIPOLOGIA" Then
            If Tipologia.ToUpper = "PRIVATO" Then
                strsql = "select '' descrizione , 0 idtipologieenti,null as Ordinamento union select descrizione, idtipologieenti,Ordinamento from tipologieenti where (privato =1 and abilitata=1 AND  isnull(ALBO,'SCU') ='SCU' and iscrizione = 1) or descrizione = (select tipologia from enti where IDEnte=0" & Request.QueryString("id") & ")  order by Ordinamento"
            Else
                strsql = "select '' descrizione , 0 idtipologieenti,null as Ordinamento union select descrizione, idtipologieenti,Ordinamento from tipologieenti where (privato =0 and abilitata=1 AND  isnull(ALBO,'SCU') ='SCU' and iscrizione = 1) or descrizione = (select tipologia from enti where IDEnte=0" & Request.QueryString("id") & ")   order by Ordinamento"
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        ddlGiuridica.DataSource = dtrgenerico
        ddlGiuridica.DataValueField = "idtipologieenti"
        ddlGiuridica.DataTextField = "Descrizione"
        ddlGiuridica.DataBind()

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


    End Sub

    Private Sub PopolaMaschera()
        Dim Tipologia As String = ""
        Dim AlboEnte As String = ""
        Dim privato As Integer
        Dim obbligatorio As String = "(*)"
        Dim chkSenzaLucro As Boolean?
        Dim AltroTipoEnte As String
        'Generata da Alessandra Taballione il 25/02/04
        'popolamento della Masche di Gestione dell'Ente
        'Stato Ente
        'Popolamento della Maschera
        'strsql = "Select * from Enti Where idEnte =" & Session("IdEnte") & ""
        strsql = ""
        strsql = "select day(datacontrolloemail)as ggDCEmail,month(datacontrolloemail)as monthDCEmail,year(datacontrolloemail)as yearDCEmail," &
                " day(datacontrollohttp)as ggDChttp,month(datacontrollohttp)as monthDChttp,year(datacontrollohttp)as yearDChttp, statienti.statoente," &
                " classiaccreditamento.classeaccreditamento as classeaccreditamentorichiesta ,classiaccreditamento.EntiInPartenariato, " &
                " dbo.formatodata(enti.DataRiserva) as DataRiserva,dbo.formatodata(enti.DataNoRiserva) as DataNoRiserva, enti.albo, isnull(t.privato,2) as privato, isnull(enti.StatoPec,0) as StatoPec, *" &
                " FROM Enti " &
                " INNER JOIN Classiaccreditamento on classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamentorichiesta" &
                " INNER JOIN Statienti on statienti.idstatoente=enti.idstatoente " &
                " left join tipologieenti t on enti.tipologia = t.descrizione " &
                " WHERE Enti.idente=" & Request.QueryString("id") & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()

        If Not IsDBNull(dtrgenerico("AttivitaSenzaLucro")) Then
            chkSenzaLucro = dtrgenerico("AttivitaSenzaLucro")
        End If

        If Not IsDBNull(dtrgenerico("AltroTipoEnte")) Then
            AltroTipoEnte = dtrgenerico("AltroTipoEnte")
        End If

        If Not IsDBNull(dtrgenerico("StatoEnte")) Then
            lblStato.Text = dtrgenerico("StatoEnte")
        End If
        AlboEnte = dtrgenerico("ALBO")
        privato = dtrgenerico("privato")
        If Not IsDBNull(dtrgenerico("Tipologia")) Then
            Tipologia = dtrgenerico("Tipologia")
        End If

        txtCodFisArchivio.Text = IIf(Not IsDBNull(dtrgenerico("CodiceFiscaleArchivio")), dtrgenerico("CodiceFiscaleArchivio"), "")
        txtCodFis.Text = IIf(Not IsDBNull(dtrgenerico("CodiceFiscale")), dtrgenerico("CodiceFiscale"), "")
        txtCodRegione.Text = IIf(Not IsDBNull(dtrgenerico("CodiceRegione")), dtrgenerico("CodiceRegione"), "")
        'Aggiunto da Alessandra Taballione il 28/02/2005
        'Implementazione del campo data di costituzione Ente
        txtDataCostituzione.Text = IIf(Not IsDBNull(dtrgenerico("dataCostituzione")), dtrgenerico("DataCostituzione"), "")
        txtdenominazione.Text = IIf(Not IsDBNull(dtrgenerico("Denominazione")), dtrgenerico("Denominazione"), "")
        'Modificato da Alessandra Taballione il 09/06/2004
        'non verrà piu inserito il numero delle sedi ma ci sarà una tabella
        'che conterrà le informazioni relative alla classe Richiesta
        txtRichiedente.Value = IIf(Not IsDBNull(dtrgenerico("NoteRichiestaRegistrazione")), dtrgenerico("NoteRichiestaRegistrazione"), "")
        txtTelefono.Text = IIf(Not IsDBNull(dtrgenerico("TelefonoRichiestaRegistrazione")), dtrgenerico("TelefonoRichiestaRegistrazione"), "")
        txtprefisso.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoTelefonoRichiestaRegistrazione")), dtrgenerico("PrefissoTelefonoRichiestaRegistrazione"), "")

        'controllo sulla password se non è null valorizzo l'utenza
        If Not IsDBNull(dtrgenerico("password")) Then
            txtUtenza.Value = IIf(Not IsDBNull(dtrgenerico("Username")), dtrgenerico("Username"), "")
            txtPassword.Value = dtrgenerico("password")
        End If
        lblClasse.Text = dtrgenerico("classeaccreditamentorichiesta")
        If lblClasse.Text = "Nessuna Classe" Then lblClasse.Text = "Nessuna Sezione"
        lblIdClasse.Value = dtrgenerico("idclasseaccreditamentorichiesta")

        lblInPartenariato.Value = dtrgenerico("EntiInPartenariato")
        txtEmail.Text = IIf(Not IsDBNull(dtrgenerico("email")), dtrgenerico("email"), "")
        txtEmailpec.Text = IIf(Not IsDBNull(dtrgenerico("EmailCertificata")), dtrgenerico("EmailCertificata"), "")
        txthttp.Text = IIf(Not IsDBNull(dtrgenerico("http")), dtrgenerico("http"), "")
        txtPrefissoFax.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoFax")), dtrgenerico("PrefissoFax"), "")
        txtFax.Text = IIf(Not IsDBNull(dtrgenerico("Fax")), dtrgenerico("Fax"), "")
        txtDataNominaRL.Text = IIf(Not IsDBNull(dtrgenerico("DataNominaRL")), dtrgenerico("DataNominaRL"), "")
        txtCodFiscRL.Text = IIf(Not IsDBNull(dtrgenerico("CodiceFiscaleRL")), dtrgenerico("CodiceFiscaleRL"), "")
        '*** aggiunto il 20/12/2017 riserva ***
        'ADC 02/03/2022
        If txtCodFiscRL.Text <> "" Then
            txtCDFRespLegal.Value = txtCodFiscRL.Text
        End If

        If txtEmailpec.Text = "" Then
            imgStatoPec.Visible = False
        Else
            If Session("TipoUtente") = "E" Then
                imgStatoPec.Visible = False
            Else
                imgStatoPec.Visible = True
                Select Case dtrgenerico("StatoPec")
                    Case 0 'davalutare
                        imgStatoPec.ImageUrl = "images/alert.png"
                        imgStatoPec.ToolTip = "PEC da verificare"
                        imgStatoPec.AlternateText = "PEC da verificata"
                    Case 1 'valida
                        imgStatoPec.ImageUrl = "images/vistabuona_small.png"
                        imgStatoPec.ToolTip = "PEC verificata"
                        imgStatoPec.AlternateText = "PEC verificata"
                    Case 2 'non valida
                        imgStatoPec.ImageUrl = "images/vistacattiva_small.png"
                        imgStatoPec.ToolTip = "PEC non valida: " & dtrgenerico("CausalePec")
                        imgStatoPec.AlternateText = "PEC non valida"
                End Select
            End If
        End If

        If Not IsDBNull(dtrgenerico("DataRiserva")) Then
            LblDataRiserva.Text = dtrgenerico("DataRiserva")
        End If
        If Not IsDBNull(dtrgenerico("DataNoRiserva")) Then
            LblDataNORiserva.Text = dtrgenerico("DataNoRiserva")
        End If

        If Not IsDBNull(dtrgenerico("AttivitaUltimiTreAnni")) Then
            If dtrgenerico("AttivitaUltimiTreAnni") = True Then
                chkAttivita.Checked = True
            Else
                chkAttivita.Checked = False
            End If
        End If

        If Not IsDBNull(dtrgenerico("AttivitaFiniIstituzionali")) Then
            If dtrgenerico("AttivitaFiniIstituzionali") = True Then
                chkFiniIstituzionali.Checked = True
            Else
                chkFiniIstituzionali.Checked = False
            End If
        End If

        '*****
        If Tipologia <> "" Then
            If AlboEnte = "SCN" Then
                If privato = -1 Then
                    ddlTipologia.SelectedIndex = 2
                    'trovaindex("PRIVATO", ddlTipologia)
                    ddlGiuridica.Visible = False
                Else
                    ddlTipologia.SelectedIndex = 1
                    ddlGiuridica.Visible = True
                    ddlGiuridica.SelectedIndex = trovaindex(Tipologia, ddlGiuridica)
                    txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
                End If
            Else
                Select Case privato
                    Case 0  'PUBBLICO
                        ddlTipologia.SelectedIndex = 1
                    Case -1 'PRIVATO
                        ddlTipologia.SelectedIndex = 2
                    Case Else 'NULL
                        ddlTipologia.SelectedIndex = 0
                End Select

                CaricaDettaglioTipologiaEntiSCU(Tipologia, "POPOLAMASCHERA")
                ddlGiuridica.Visible = True
                ddlGiuridica.SelectedIndex = trovaindex(Tipologia, ddlGiuridica)
                'txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
            End If
        End If

       

        If ddlTipologia.SelectedValue = "Privato" Then
            If chkSenzaLucro IsNot Nothing Then
                divSenzaScopoLucro.Visible = True
                If (chkSenzaLucro) Then
                    chkSenzaScopoLucro.Checked = True
                Else
                    chkSenzaScopoLucro.Checked = False
                End If
            End If
        Else
            divSenzaScopoLucro.Visible = False
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'popolo campi relazione
        strsql = "Select * from entirelazioni where identerelazione=" & Session("idEnteRelazione") & " "
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()

            txtDataScadenza.Text = IIf(Not IsDBNull(dtrgenerico("datascadenza")), dtrgenerico("datascadenza"), "")
            txtDataStipula.Text = IIf(Not IsDBNull(dtrgenerico("datastipula")), dtrgenerico("datastipula"), "")
            ddlrelazione.SelectedValue = dtrgenerico("idtiporelazione")

            If ddlrelazione.SelectedItem IsNot Nothing _
               AndAlso ddlrelazione.SelectedItem.Text <> "" Then
                If ddlrelazione.SelectedItem.Text = "Contratto" Then
                    If lblDataStipula.Text.IndexOf(obbligatorio) = -1 Then
                        lblDataStipula.Text = obbligatorio & lblDataStipula.Text
                    End If
                Else
                    If lblDataStipula.Text.IndexOf("(*)") <> -1 Then
                        lblDataStipula.Text = Mid(lblDataStipula.Text, obbligatorio.Length + 1, lblDataStipula.Text.Length)
                    End If
                End If
            End If

            If Not IsDBNull(dtrgenerico("datafinevalidità")) Then
                lblStatoAccordo.Text = "Annullato"
            Else
                lblStatoAccordo.Text = "Attivo"
            End If
        Else
            lblaccordo.Visible = False
            lblStatoAccordo.Visible = False
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
        'Ricerca e popolamento dei dati relativi alla sede principale dell'Ente Figlio
        strsql = "SELECT  entisedi.identesede,entisedi.idcomune,entisedi.indirizzo,entisedi.DettaglioRecapito, entisedi.civico,entisedi.cap,comuni.denominazione as comune," &
        " provincie.provincia, provincie.idprovincia, provincie.ProvinceNazionali " &
        " FROM entisedi " &
        " INNER JOIN statientisedi on statientisedi.idstatoentesede=entisedi.idstatoentesede " &
        " INNER JOIN entiseditipi ON entisedi.IDEnteSede = entiseditipi.IDEnteSede " &
        " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
        " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " &
        " WHERE entisedi.IDEnte = " & Request.QueryString("id") & " And entiseditipi.idtiposede = 1 and (statientisedi.attiva=1 or statientisedi.DefaultStato=1)"

        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            ChkEstero.Checked = Not CBool(dtrgenerico("ProvinceNazionali"))
            If ChkEstero.Checked = True Then
                lblComune.Text = "Località"
                lblCAP.Text = "Codice località"
            Else
                lblComune.Text = "Comune"
                lblCAP.Text = "CAP"
            End If

            txtIndirizzo.Text = dtrgenerico("Indirizzo")
            TxtDettaglioRecapito.Text = IIf(Not IsDBNull(dtrgenerico("DettaglioRecapito")), dtrgenerico("DettaglioRecapito"), "")
            txtCivico.Text = dtrgenerico("civico")
            'adc 21/12/2020
            txtCAP.Text = IIf(Not IsDBNull(dtrgenerico("cap")), dtrgenerico("cap"), "")

            'txtCAP.Text = dtrgenerico("cap")

            txtIdSede.Value = dtrgenerico("identeSede")
            txtIDComunes.Value = dtrgenerico("idcomune")
            txtIDComune.Value = dtrgenerico("idcomune")
            Session("IdComune") = dtrgenerico("idcomune")
            Dim idProvincia As String = dtrgenerico("idprovincia")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            If Not ddlProvincia.SelectedValue Is Nothing Then
                ddlProvincia.SelectedValue = idProvincia
            End If
            If Not Session("IdComune") Is Nothing Then
                selComune.CaricaComuniDaProvincia(ddlComune, idProvincia, Session("Conn"))
                ddlComune.SelectedValue = Session("IdComune")

            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'richiamo sub per caricare i Settori associati all'Ente
        If lblIdEnte.Value IsNot Nothing AndAlso lblIdEnte.Value.Trim() <> String.Empty Then
            CaricaEntiAllegati()
            CaricaEntiSettori()
        End If

        If ddlGiuridica.SelectedItem IsNot Nothing _
          AndAlso ddlGiuridica.SelectedItem.Text = "Altro ai sensi dell’Art. 1, comma 2, D.Lgs. 30-3-2001 n. 165 (specificare)" Then
            txtAltraTipoEnte.Text = AltroTipoEnte
            divAltraTipologia.Visible = True
        Else
            txtAltraTipoEnte.Text = String.Empty
            divAltraTipologia.Visible = True
        End If

        If Session("TipoUtente") = "U" Then
            CaricaDataAccredamento_Adeguamento()
        End If

    End Sub

    Private Function CaricaGrid()
        Dim dtsAttivitàEnteFiglio As New DataSet
        strsql = "SELECT distinct(attività.CodiceEnte), attività.Titolo, statiattività.StatoAttività " &
                 "FROM enti INNER JOIN entisedi ON enti.IDEnte = entisedi.IDEnte " &
                 "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
                 "INNER JOIN attivitàentisediattuazione ON entisediattuazioni.IDEnteSedeAttuazione = attivitàentisediattuazione.IDEnteSedeAttuazione " &
                 "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " &
                 "INNER JOIN statiattività ON attività.IDStatoAttività = statiattività.IDStatoAttività " &
                 "WHERE enti.IDEnte = " & lblIdEnte.Value
        dtsAttivitàEnteFiglio = ClsServer.DataSetGenerico(strsql, Session("Conn"))
        dtgAttivitaEnte.DataSource = dtsAttivitàEnteFiglio
        dtgAttivitaEnte.DataBind()
        dtgAttivitaEnte.Visible = True
        lblTitotloProgettiEnte.Visible = True
    End Function

    Private Sub PopolaMascheraVariazione()
        
        Dim Tipologia As String = ""
        Dim AlboEnte As String = ""
        Dim privato As Integer
        Dim IdComune As String
        Dim chkSenzaLucro As Boolean?
        Dim AltroTipoEnte As String
        Dim RapportAnnuale As Boolean?

        strsql = "SELECT day(datacontrolloemail)as ggDCEmail,month(datacontrolloemail)as monthDCEmail,year(datacontrolloemail)as yearDCEmail, " &
                 "        day(datacontrollohttp)as ggDChttp,month(datacontrollohttp)as monthDChttp,year(datacontrollohttp)as yearDChttp, " &
                 "        classiaccreditamento.classeaccreditamento as classeaccreditamentorichiesta ,classiaccreditamento.EntiInPartenariato, " &
                 " dbo.formatodata(e.DataRiserva) as DataRiserva,dbo.formatodata(e.DataNoRiserva) as DataNoRiserva, E.ALBO ,  isnull(t.privato,2) as privato,enti.EmailCertificata as PecVariazione,*  " &
                 " FROM Accreditamento_VariazioneEntiFiglio enti   " &
                 " INNER JOIN enti e on e.idente = enti.idente  " &
                 " INNER JOIN classiaccreditamento on (classiaccreditamento.idclasseaccreditamento=e.idclasseaccreditamentorichiesta)  " &
                 " inner join statienti on (statienti.idstatoente=e.idstatoente)" &
                 " left join tipologieenti t on enti.tipologia = t.descrizione " &
                 " WHERE enti.idente=" & lblIdEnte.Value & " AND enti.StatoVariazione = 0 "

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()

        'Aggiunto da Andrea Mondello il 26/04/2021 atto di nomina del Rappreesentante Legale inizio
        txtDataNominaRL.Text = IIf(Not IsDBNull(dtrgenerico("DataNominaRL")), dtrgenerico("DataNominaRL"), "")
        'Aggiunto da Andrea Mondello il 26/04/2021 atto di nomina del Rappreesentante Legale fine
        txtCodFis.Text = IIf(Not IsDBNull(dtrgenerico("CodiceFiscale")), dtrgenerico("CodiceFiscale"), "")

        txtCodFiscRL.Text = IIf(Not IsDBNull(dtrgenerico("CodiceFiscaleRL")), dtrgenerico("CodiceFiscaleRL"), "")


        'ADC 04/05/2022
        If txtDataNominaRL.Text <> "" Then
            hdDataNominaRL.Value = txtDataNominaRL.Text
        End If
        'ADC 02/03/2022
        If txtCodFiscRL.Text <> "" Then
            txtCDFRespLegal.Value = txtCodFiscRL.Text
        End If

        If Not IsDBNull(dtrgenerico("AttivitaFiniIstituzionali")) Then
            If dtrgenerico("AttivitaFiniIstituzionali") = True Then
                chkFiniIstituzionali.Checked = True
            Else
                chkFiniIstituzionali.Checked = False
            End If
        End If

        If Not IsDBNull(dtrgenerico("AttivitaSenzaLucro")) Then
            If dtrgenerico("AttivitaSenzaLucro") = True Then
                chkSenzaScopoLucro.Checked = True
            Else
                chkSenzaScopoLucro.Checked = True
            End If
        End If

        If Not IsDBNull(dtrgenerico("AttivitaUltimiTreAnni")) Then
            If dtrgenerico("AttivitaUltimiTreAnni") = True Then
                chkAttivita.Checked = True
            Else
                chkAttivita.Checked = True
            End If
        End If

        If Not IsDBNull(dtrgenerico("AltroTipoEnte")) Then
            AltroTipoEnte = dtrgenerico("AltroTipoEnte")
        End If

        If Not IsDBNull(dtrgenerico("StatoEnte")) Then
            lblStato.Text = dtrgenerico("StatoEnte")
        End If
        AlboEnte = dtrgenerico("ALBO")
        privato = dtrgenerico("privato")
        If Not IsDBNull(dtrgenerico("Tipologia")) Then
            Tipologia = dtrgenerico("Tipologia")
        End If

        ddlrelazione.SelectedValue = dtrgenerico("idtiporelazione")
        txtCodFisArchivio.Text = IIf(Not IsDBNull(dtrgenerico("CodiceFiscaleArchivio")), dtrgenerico("CodiceFiscaleArchivio"), "")
        txtCodFis.Text = IIf(Not IsDBNull(dtrgenerico("CodiceFiscale")), dtrgenerico("CodiceFiscale"), "")
        txtCodRegione.Text = IIf(Not IsDBNull(dtrgenerico("CodiceRegione")), dtrgenerico("CodiceRegione"), "")
        txtDataCostituzione.Text = IIf(Not IsDBNull(dtrgenerico("dataCostituzione")), dtrgenerico("DataCostituzione"), "")
        txtdenominazione.Text = IIf(Not IsDBNull(dtrgenerico("Denominazione")), dtrgenerico("Denominazione"), "")
        txtRichiedente.Value = IIf(Not IsDBNull(dtrgenerico("NoteRichiestaRegistrazione")), dtrgenerico("NoteRichiestaRegistrazione"), "")
        txtTelefono.Text = IIf(Not IsDBNull(dtrgenerico("TelefonoRichiestaRegistrazione")), dtrgenerico("TelefonoRichiestaRegistrazione"), "")
        txtprefisso.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoTelefonoRichiestaRegistrazione")), dtrgenerico("PrefissoTelefonoRichiestaRegistrazione"), "")
        'controllo sulla password se non è null valorizzo l'utenza
        If Not IsDBNull(dtrgenerico("password")) Then
            txtUtenza.Value = IIf(Not IsDBNull(dtrgenerico("Username")), dtrgenerico("Username"), "")
            txtPassword.Value = dtrgenerico("password")
        End If
        lblClasse.Text = dtrgenerico("classeaccreditamentorichiesta")
        If lblClasse.Text = "Nessuna Classe" Then lblClasse.Text = "Nessuna Sezione"
        lblIdClasse.Value = dtrgenerico("idclasseaccreditamentorichiesta")
        lblInPartenariato.Value = dtrgenerico("EntiInPartenariato")
        txtEmail.Text = IIf(Not IsDBNull(dtrgenerico("email")), dtrgenerico("email"), "")
        'ADC 24/03/2022
        txtEmailpec.Text = IIf(Not IsDBNull(dtrgenerico("PecVariazione")), dtrgenerico("PecVariazione"), "")
        'txtEmailpec.Text = IIf(Not IsDBNull(dtrgenerico("EmailCertificata")), dtrgenerico("EmailCertificata"), "")    'avevo fatto cosi ma secondo me e' sbagliato  ADC
        'FINE ADC
        txthttp.Text = IIf(Not IsDBNull(dtrgenerico("http")), dtrgenerico("http"), "")
        txtPrefissoFax.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoFax")), dtrgenerico("PrefissoFax"), "")
        txtFax.Text = IIf(Not IsDBNull(dtrgenerico("Fax")), dtrgenerico("Fax"), "")
        txtDataScadenza.Text = IIf(Not IsDBNull(dtrgenerico("datascadenza")), dtrgenerico("datascadenza"), "")
        txtDataStipula.Text = IIf(Not IsDBNull(dtrgenerico("datastipula")), dtrgenerico("datastipula"), "")

        '*** aggiunto il 20/12/2017 riserva ***
        If Not IsDBNull(dtrgenerico("DataRiserva")) Then
            LblDataRiserva.Text = dtrgenerico("DataRiserva")
        End If
        If Not IsDBNull(dtrgenerico("DataNoRiserva")) Then
            LblDataNORiserva.Text = dtrgenerico("DataNoRiserva")
        End If
        '*****
        If Tipologia <> "" Then
            If AlboEnte = "SCN" Then
                If privato = -1 Then
                    ddlTipologia.SelectedIndex = 2
                    ddlGiuridica.Visible = False
                Else
                    ddlTipologia.SelectedIndex = 1
                    ddlGiuridica.Visible = True
                    ddlGiuridica.SelectedIndex = trovaindex(Tipologia, ddlGiuridica)
                    txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
                End If
            Else
                Select Case privato
                    Case 0 'PUBBLICO
                        ddlTipologia.SelectedIndex = 1
                    Case -1 'PRIVATO
                        ddlTipologia.SelectedIndex = 2
                    Case 2 'NULL
                        ddlTipologia.SelectedIndex = 0
                End Select

                CaricaDettaglioTipologiaEntiSCU(Tipologia, "POPOLAMASCHERA")
                ddlGiuridica.Visible = True
                ddlGiuridica.SelectedIndex = trovaindex(Tipologia, ddlGiuridica)
                txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "Select * from entirelazioni where identerelazione=" & Request.QueryString("identerelazione") & " "
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            If Not IsDBNull(dtrgenerico("datafinevalidità")) Then
                lblStatoAccordo.Text = "Annullato"
            Else
                lblStatoAccordo.Text = "Attivo"
            End If
        Else
            lblaccordo.Visible = False
            lblStatoAccordo.Visible = False
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
        'Ricerca e popolamento dei dati relativi alla sede principale dell'Ente Figlio
        strsql = " SELECT  entisedi.identesede,entisedi.idcomune,entisedi.indirizzo,entisedi.DettaglioRecapito," &
                 "     entisedi.civico,entisedi.cap,comuni.denominazione as comune,provincie.provincia, provincie.idprovincia, provincie.ProvinceNazionali " &
                 " FROM Accreditamento_VariazioneEntiFiglio entisedi" &
                 " INNER JOIN entisedi es on entisedi.identesede =es.identesede" &
                 " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune" &
                 " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia" &
                 " WHERE entisedi.IDEnte = " & lblIdEnte.Value & " and entisedi.StatoVariazione=0 "
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            txtIndirizzo.Text = dtrgenerico("Indirizzo")
            TxtDettaglioRecapito.Text = IIf(Not IsDBNull(dtrgenerico("DettaglioRecapito")), dtrgenerico("DettaglioRecapito"), "")
            txtCivico.Text = dtrgenerico("civico")
            txtCAP.Text = dtrgenerico("cap")
            ChkEstero.Checked = Not CBool(dtrgenerico("ProvinceNazionali"))
            If ChkEstero.Checked = True Then
                lblComune.Text = "Località"
                lblCAP.Text = "Codice località"
            Else
                lblComune.Text = "Comune"
                lblCAP.Text = "CAP"
            End If

            txtIdSede.Value = dtrgenerico("identeSede")
            txtIDComunes.Value = dtrgenerico("idcomune")
            txtIDComune.Value = dtrgenerico("idcomune")
            Session("IdComune") = dtrgenerico("idcomune")
            Dim idProvincia As String = dtrgenerico("idprovincia")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            If Not ddlProvincia.SelectedValue Is Nothing Then
                ddlProvincia.SelectedValue = idProvincia
            End If
            If Not Session("IdComune") Is Nothing Then
                selComune.CaricaComuniDaProvincia(ddlComune, idProvincia, Session("Conn"))
                ddlComune.SelectedValue = Session("IdComune")
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


        'Carico gli allegati salvati e li metto in sessione
        CaricaEntiAllegatiVariazioni()

        'richiamo sub per caricare i Settori associati all'Ente
        CaricaEntiSettoriVariazione()

        If ddlGiuridica.SelectedItem IsNot Nothing _
          AndAlso ddlGiuridica.SelectedItem.Text = "Altro ai sensi dell’Art. 1, comma 2, D.Lgs. 30-3-2001 n. 165 (specificare)" Then
            txtAltraTipoEnte.Text = AltroTipoEnte
            divAltraTipologia.Visible = True
        Else
            txtAltraTipoEnte.Text = String.Empty
            divAltraTipologia.Visible = True
        End If

        If Session("TipoUtente") = "U" Then
            CaricaDataAccredamento_Adeguamento()
        End If

    End Sub


    Private Sub PersonalizzaTasti()
        If Request.QueryString("azione") = "Ins" And txtmodificaqueristringdiINS.Value <> "MOD" Then
            'cmdripristina.Visible = False
            cmdElimina.Visible = False

        Else
            If Request.QueryString("Stato") = "Annullato" Then
                CmdSalva.Visible = False
                cmdElimina.Visible = False
                'ANTONELLO ADEGUAMENTO DANIANTO-------------- da scommentare----------------------------
                'cmdripristina.Visible = True
                '--------------------------------------------------------------
            End If
            If Request.QueryString("Stato") = "Attivo" Then
                CmdSalva.Visible = True
                cmdElimina.Visible = True
            End If
        End If
    End Sub

    Private Function trovaindex(ByVal stringa As String, ByVal obj As Object)
        'Generata da Alessandra Taballione il 26/02/04
        'Ricerca nella DropDownList passata l'index Corrispondente al testo in essa selezionato.
        Dim i As Integer
        For i = 0 To obj.Items.Count - 1
            If UCase(obj.Items(i).Text) = UCase(stringa) Then
                trovaindex = i
                Exit For
            End If
        Next
    End Function

    Function ElencoSettoriEsperienzePrecedenti() As DataTable


        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataTable As New DataTable
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_GET_SETTORI_ESPERIENZE_PREC]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = IIf(Request.QueryString("id") = Nothing, 0, Integer.Parse(Request.QueryString("id")))

            sqlDAP.Fill(dataTable)

        Catch ex As Exception
            msgErrore.ForeColor = Color.Red
            msgErrore.Text = "Errore nel recupero informazioni"
            Exit Function
        End Try

        Return dataTable
    End Function


    Function ElencoVariazioniSettori() As DataTable
        'chiamata dalla CaricaEntiSettoriVariazione, legge da db le variazioni settori in corso indicando se sono nuovi settori (inseriti nella fase attuale)
        'sostituisce la query scolpita a codice presente precedentemente

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataTable As New DataTable
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_ELENCO_VARIAZIONI_SETTORI_ENTE_FIGLIO]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = IIf(Request.QueryString("id") = Nothing, 0, Integer.Parse(Request.QueryString("id")))

            sqlDAP.Fill(dataTable)

        Catch ex As Exception
            msgErrore.ForeColor = Color.Red
            msgErrore.Text = "Errore nel recupero informazioni"
            Exit Function
        End Try

        Return dataTable
    End Function


   Sub CaricaEntiSettoriVariazione()

        'aggiunto il 19/08/2014 da Simona Cordella
        'Dim dtrSettori As SqlClient.SqlDataReader
        Dim blnAbilitaSettori As Boolean
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim dtSettori As New DataTable

        Session("ElencoSettoriDBAccordo") = Nothing
        dtSettori = ElencoVariazioniSettori()
        Session("ElencoSettoriDBAccordo") = dtSettori

        blnAbilitaSettori = AbilitaSettori()

        'rendo tutti i cmdinserisci invisibili, poi vengono abilitati dopo se serve
        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
            If Not cmdInserisci Is Nothing Then
                cmdInserisci.Visible = False
            End If
        Next

        If dtSettori IsNot Nothing And dtSettori.Rows.Count > 0 Then 'Esistono settori legate ad esperienze quindi le carico in maschera

            For Each dr As DataRow In dtSettori.Rows
                Dim esperienzaArea As New clsEsperienzeAree
                Dim esperienze(2) As String
                Dim anniEsperienze(2) As Integer

                If IsDBNull(dr("IDSETTORE")) Then
                    For Each gRow As GridViewRow In dtgSettori.Rows
                        Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                        Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                        Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                        If dr("IDMACROATTIVITA").ToString() = HFIdMacroAttivita.Value Then
                            check.Checked = True
                            ChkChange(check, New EventArgs)
                            cmdInserisci.Visible = If(dr("NuovoInserimento") = "0", False, True)
                        End If

                        If blnAbilitaSettori = False Then
                            check.Enabled = blnAbilitaSettori
                        End If

                    Next
                    Continue For
                End If
                For i = 0 To 2 'Anni e Esperienza
                    Select Case i
                        Case 0
                            esperienze(i) = dr("I_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("I_ANNOESPERIENZA").ToString())
                        Case 1
                            esperienze(i) = dr("II_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("II_ANNOESPERIENZA").ToString())
                        Case 2
                            esperienze(i) = dr("III_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("III_ANNOESPERIENZA").ToString())
                    End Select
                Next
                esperienzaArea.IdSettore = CInt(dr("IDSETTORE").ToString())
                esperienzaArea.AnnoEsperienza = anniEsperienze.ToList()
                esperienzaArea.DescrizioneEsperienza = esperienze.ToList()
                esperienzaArea.AreeSelezionate = dr("AREEINTERVENTO").ToString().Split(",").ToList()

                EsperienzeAreeSettore.Add(esperienzaArea)
                Session("EsperienzeAreeSettoreAccordo") = EsperienzeAreeSettore

                For Each gRow As GridViewRow In dtgSettori.Rows
                    Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                    Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                    Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                    If dr("IDSETTORE").ToString() = HFIdMacroAttivita.Value Then
                        check.Checked = True
                        ChkChange(check, New EventArgs)
                        cmdInserisci.Visible = If(dr("NuovoInserimento") = "0", False, True)  'possono essere editate SOLO esperienze/aree inserite in questo adeguamento
                        hfRigaSelezionata.Value = gRow.RowIndex
                        AggiungiEsperienzaAmbiti()
                    Else
                        'cmdInserisci.Visible = False
                    End If

                    If blnAbilitaSettori = False Then
                        check.Enabled = blnAbilitaSettori
                    End If

                Next
            Next
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("azione") = "Ins" Then 'se sono in inserimento ritorno alla mail
            Session("IdComune") = Nothing
            Response.Redirect("WfrmMain.aspx")
        Else 'se sono in modifica ritorno alla Web form di ricerca
            Response.Redirect("WfrmRicEnteinAccordo.aspx?azione=Mod")
        End If
    End Sub

    Private Sub CmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalva.Click
        msgErrore.Text = String.Empty
        msgConferma.Text = String.Empty
        'Generato da Alessandra Taballione il 23/02/2004
        'Effettuo Controllo sulla Partita Iva 
        Dim comuni As Integer
        comuni = 0
        Dim AlboEnte As String
        Dim IDAllegatoDeliberaAdesione As Integer
        Dim IDAllegatoStatuto As Integer
        Dim IDAllegatoAttoCostitutivo As Integer
        Dim IDAllegatoImpegnoEtico As Integer
        Dim IDAllegatoImpegno As Integer

        messaggioNotificaRL.Visible = False  'ADC 02/03/2022
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
        msgErrore.Visible = True
        If ValidazioneServerSalva() = False Then
            Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_VALIDAZIONE_CAMPIINSERIMENTO_NONCORRETTA, "Valori inseriti non validi ")
            Exit Sub
        End If


        If txtCAP.Text = "" And ChkEstero.Checked = False Then
            msgErrore.Text = "Il CAP inserito non è congruo rispetto al comune selezionato."
            Exit Sub
        End If
        'controllo inserimento settore---------------- Antonello------------------------------------
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        ''controllo i settori
        'If VerificaCheck() = False Then
        '    msgErrore.Text = "E' necessario indicare almeno un Settore di Intervento."
        '    Exit Sub
        'End If
        ''Nel caso in cui sono in fase di inserimento non devo controllare i settori
        'If Request.QueryString("Id") <> "" Or lblIdEnte.Text <> "-1" Then

        'End If
        If AlboEnte = "SCN" Then
            If ddlTipologia.SelectedItem.Text = "Pubblico" Then
                If txtpianolocale.Value <> "Piano di Zona" And ddlGiuridica.SelectedItem.Text = "Piano di Zona" Then
                    msgErrore.Text = "Impossibile inserire la Tipologia Piano di Zona in quanto non e' piu' una tipologia Accreditabile"
                    Exit Sub
                End If
            Else
                txtpianolocale.Value = ""
            End If
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Dim strMiaCausale As String = ""
        If ClsUtility.CAP_VERIFICA(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")),
                strMiaCausale, bandiera, Trim(txtCAP.Text), IIf(Request.Form("txtIdComune") = "", ddlComune.SelectedValue, Request.Form("txtIdComune")), "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
            'Ripristino lo stato del tasto
            CmdSalva.Visible = True
            'Inserisco il Messaggio di Errore
            msgErrore.Text = strMiaCausale
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Exit Sub
        End If

        '*************************************************
        'AGGIUNTO DA Luigi Leucci il 14/05/2019
        ' Controllo validità codice fiscale italiano
        If Not ChkEstero.Checked Then
            If txtCodFis.Text.Length <> 11 Then
                msgErrore.Text = "Il campo Codice Fiscale deve essere composto di 11 numeri"
                Exit Sub
            End If

            If Not IsNumeric(txtCodFis.Text) Then
                msgErrore.Text = "Il campo Codice Fiscale deve essere composto di soli numeri"
                Exit Sub
            End If
        End If

        '*************************************************
        'AGGIUNTO DA SIMONA CORDELLA IL 18/10/2017
        ' richiamo store per controllo codicefiscale e demoniazione ente nell'albo
        Dim ESITO As String
        Dim strMsgEmail As String = ""
        Dim strMsg As String = ""
        Dim intIDE As Integer = 0

        If Request.QueryString("azione") <> "Ins" Then
            intIDE = lblIdEnte.Value
        End If

        ESITO = ClsUtility.STORE_VERIFICA_USABILITA_ENTE_ALBO(Replace(Trim(txtCodFis.Text), "'", "''"), Replace(Trim(txtdenominazione.Text), "'", "''"), AlboEnte, intIDE, strMsgEmail, strMsg, Session("conn"))
        If ESITO = "NO" Then
            msgErrore.Text = strMsg
            Exit Sub
        End If


        If ddlrelazione.SelectedItem.Text <> "" Then
            strsql = "Select idclasseaccreditamento from tipirelazioni where idtiporelazione=" & ddlrelazione.SelectedValue & ""
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrgenerico.Read()
            If dtrgenerico.HasRows = True Then
                lblIdClasse.Value = dtrgenerico("idclasseaccreditamento")
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


        If AlboEnte = "SCN" Then
            'ADC 02/02/2006 Query che controlla la compatibilita' sulle tipologie
            If (ddlGiuridica.SelectedValue = "0" And UCase(ddlTipologia.SelectedValue) = "PUBBLICO") Or UCase(ddlTipologia.SelectedValue) = "" Then
                msgErrore.Text = "Impossibile effetutare il salvataggio. Inserire il Tipo Ente."
                Exit Sub
            End If
        End If
        If AlboEnte = "SCU" Then
            If (ddlGiuridica.SelectedValue = "0") Or UCase(ddlTipologia.SelectedValue) = "" Then
                msgErrore.Text = "Impossibile effetutare il salvataggio. Inserire il Tipo Ente."
                Exit Sub
            End If
        End If

        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
        If AlboEnte = "SCN" Then
            If ddlGiuridica.SelectedValue = "2" And UCase(ddlTipologia.SelectedValue) <> "PRIVATO" Then
                strsql = "select a.idente from enti a " &
                        " inner join tipologieenti te1 on a.tipologia = te1.descrizione" &
                        " where a.idente = " & Session("idente") & " " &
                        " and te1.statale <> 1 "

                'eseguo la query per controllare compatibilità
                dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If dtrgenerico.HasRows = True Then
                    msgErrore.Text = "Impossibile salvare i dati. Il tipo ente selezionato risulta incompatibile con l'ente capofila."
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Exit Sub
                End If

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

            End If
        End If

        'controllo eliminato su richiesta Raggi 20/05/2021
        'If txtDataCostituzione.Text = String.Empty Then
        '   msgErrore.Text = msgErrore.Text & "La data di Costituzione dell'Ente in accordo é obbligatoria"
        '   msgErrore.Text = msgErrore.Text & " </br>"
        '   Exit Sub
        'End If


        Dim ListaAllegati As New List(Of Allegato)


        '***************
        If Request.QueryString("azione") = "Ins" And txtmodificaqueristringdiINS.Value <> "MOD" Then
            'generato da Papa Jotzinger il 31/03/2006
            'controllo su multi cap
            'vado a controllare se il cap che si vuole inserire è lo stesso o cmq della stessa terza cifra del comune selezionato
            If ClsUtility.ControllaEsistenzaCap(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), Trim(txtCAP.Text), IIf(Request.Form("txtIDComune") = "", txtIDComunes.Value, Request.Form("txtIdComune"))) = False Then

                msgErrore.Text = "Il CAP inserito non è congruo rispetto al comune selezionato."
                Exit Sub
            Else

                Dim IDAllegato As Integer
                Dim IdEnteFiglio As Integer
                Dim TipoOperazione As String = "I"

                ListaAllegati = CaricaEntiAllegatiDaDB(IdEnteFiglio)


                MyTransaction = CType(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), System.Data.SqlClient.SqlConnection).BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))

                If InserisciEnte(AlboEnte, IdEnteFiglio) Then
                    If Session("LoadedACAccordo") IsNot Nothing Then
                        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedACAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                            If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedACAccordo"), Allegato), IdEnteFiglio, TipoOperazione)) Then
                                IDAllegatoAttoCostitutivo = IDAllegato
                            Else
                                MyTransaction = Nothing
                                Exit Sub
                            End If
                        Else
                            IDAllegatoAttoCostitutivo = IDAllegato
                        End If
                    End If

                    If (Session("LoadedStatutoAccordo") IsNot Nothing) Then
                        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedStatutoAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                            If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedStatutoAccordo"), Allegato), IdEnteFiglio, TipoOperazione)) Then
                                IDAllegatoStatuto = IDAllegato
                            Else
                                MyTransaction = Nothing
                                Exit Sub
                            End If
                        Else
                            IDAllegatoStatuto = IDAllegato
                        End If
                    End If

                    If (Session("LoadedDeliberaAdesioneAccordo") IsNot Nothing) Then
                        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedDeliberaAdesioneAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                            If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedDeliberaAdesioneAccordo"), Allegato), IdEnteFiglio, TipoOperazione)) Then
                                IDAllegatoDeliberaAdesione = IDAllegato
                            Else
                                MyTransaction = Nothing
                                Exit Sub
                            End If
                        Else
                            IDAllegatoDeliberaAdesione = IDAllegato
                        End If
                    End If

                    If (Session("LoadedCartaImpegnoEticoAccordo") IsNot Nothing) Then
                        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedCartaImpegnoEticoAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                            If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedCartaImpegnoEticoAccordo"), Allegato), IdEnteFiglio, TipoOperazione)) Then
                                IDAllegatoImpegnoEtico = IDAllegato
                            Else
                                MyTransaction = Nothing
                                Exit Sub
                            End If
                        Else
                            IDAllegatoImpegnoEtico = IDAllegato
                        End If
                    End If


                    If (Session("LoadedCartaImpegnoAccordo") IsNot Nothing) Then
                        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedCartaImpegnoAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                            If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedCartaImpegnoAccordo"), Allegato), IdEnteFiglio, TipoOperazione)) Then
                                IDAllegatoImpegno = IDAllegato
                            Else
                                MyTransaction = Nothing
                                Exit Sub
                            End If
                        Else
                            IDAllegatoImpegnoEtico = IDAllegato
                        End If
                    End If

                    MyTransaction.Commit()
                    MyTransaction = Nothing
                    msgConferma.Text = "Operazione avvenuta correttamente Inserito nuovo Ente di Accoglienza"
                Else
                    If MyTransaction IsNot Nothing Then
                        MyTransaction = Nothing
                    End If
                    Exit Sub
                End If
            End If
        Else
            'generato da Papa Jotzinger il 31/03/2006
            'controllo su multi cap
            'vado a controllare se il cap che si vuole inserire è lo stesso o cmq della stessa terza cifra del comune selezionato
            '***************************CONTROLLO ESISTENZA CAP******************************
            'If ClsUtility.ControllaEsistenzaCap(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), Trim(txtCAP.Text), IIf(Request.Form("txtIdComune") = "", txtIDComunes.Value, Request.Form("txtIdComune")), "") = False Then
            'If ClsUtility.ControllaEsistenzaCap(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), Trim(txtCAP.Text), ddlComune.SelectedValue, "") = False Then



            If ClsUtility.CAP_VERIFICA(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")),
               strMiaCausale, bandiera, Trim(txtCAP.Text), ddlComune.SelectedValue, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
                'Session("conn"), strCausale, blnLocal, Trim(ArrCampi(7)), "0", "", Trim(ArrCampi(4)), Trim(ArrCampi(5)), Trim(ArrCampi(6))) = False Then
                msgErrore.Text = "Il CAP inserito non è congruo rispetto al comune selezionato."
                Exit Sub
                '***********************************************************
            Else
                Dim IDAllegato As Integer

                ListaAllegati = CaricaEntiAllegatiDaDB()

                MyTransaction = CType(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), System.Data.SqlClient.SqlConnection).BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
                If Session("LoadedACAccordo") IsNot Nothing Then
                    If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedACAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                        If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedACAccordo"), Allegato))) Then
                            IDAllegatoAttoCostitutivo = IDAllegato
                        Else
                            MyTransaction = Nothing
                            Exit Sub
                        End If
                    Else
                        IDAllegatoAttoCostitutivo = IDAllegato
                    End If
                End If

                If (Session("LoadedStatutoAccordo") IsNot Nothing) Then
                    If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedStatutoAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                        If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedStatutoAccordo"), Allegato))) Then
                            IDAllegatoStatuto = IDAllegato
                        Else
                            MyTransaction = Nothing
                            Exit Sub
                        End If
                    Else
                        IDAllegatoStatuto = IDAllegato
                    End If
                End If

                If (Session("LoadedDeliberaAdesioneAccordo") IsNot Nothing) Then
                    If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedDeliberaAdesioneAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                        If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedDeliberaAdesioneAccordo"), Allegato))) Then
                            IDAllegatoDeliberaAdesione = IDAllegato
                        Else
                            MyTransaction = Nothing
                            Exit Sub
                        End If
                    Else
                        IDAllegatoDeliberaAdesione = IDAllegato
                    End If
                End If

                If (Session("LoadedCartaImpegnoEticoAccordo") IsNot Nothing) Then
                    If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedCartaImpegnoEticoAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                        If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedCartaImpegnoEticoAccordo"), Allegato))) Then
                            IDAllegatoImpegnoEtico = IDAllegato
                        Else
                            MyTransaction = Nothing
                            Exit Sub
                        End If
                    Else
                        IDAllegatoImpegnoEtico = IDAllegato
                    End If
                End If

                If (Session("LoadedCartaImpegnoAccordo") IsNot Nothing) Then
                    If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedCartaImpegnoAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                        If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedCartaImpegnoAccordo"), Allegato))) Then
                            IDAllegatoImpegno = IDAllegato
                        Else
                            MyTransaction = Nothing
                            Exit Sub
                        End If
                    Else
                        IDAllegatoImpegno = IDAllegato
                    End If
                End If

                'EntiVariazioniAccordi()
                ModificaEnte(AlboEnte, IDAllegatoAttoCostitutivo, IDAllegatoStatuto, IDAllegatoDeliberaAdesione, IDAllegatoImpegnoEtico, IDAllegatoImpegno)
                LoadMaschera()
                'Aggiunto da Alessandra Taballione il 02/08/2005
                'Aggiorno ClasseRichiesta Ente
                strsql = "select ca.idclasseaccreditamento, classeaccreditamento" &
                " from enti " &
                " inner join classiaccreditamento ca on ca.idclasseAccreditamento=enti.idclasseaccreditamentoRichiesta " &
                " where idente=" & lblIdEnte.Value & " "
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                dtrgenerico.Read()
                lblIdClasse.Value = dtrgenerico("idclasseaccreditamento")
                lblClasse.Text = dtrgenerico("classeaccreditamento")
                If lblClasse.Text = "Nessuna Classe" Then lblClasse.Text = "Nessuna Sezione"
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

                'MessaggiConvalida("L'Accordo è stato modificato con successo")
            End If
        End If
        Session("IdComune") = Nothing
        Dim myidentefiglio As Integer = IIf(Request.QueryString("id") = Nothing, 0, Request.QueryString("id"))
        If myidentefiglio > 0 Then EvidenziaDatiModificati(myidentefiglio)
    End Sub

    Private Function VerificaCheck() As Boolean
        'controllo è  stata checcato almeno un settore per il salvataggio
        VerificaCheck = False
        Dim item As DataGridItem
        For Each gr As GridViewRow In dtgSettori.Rows
            Dim check As CheckBox = DirectCast(gr.FindControl("chkSeleziona"), CheckBox)
            If check.Checked = True Then
                VerificaCheck = True
            End If
        Next
        Return VerificaCheck
    End Function
    Private Function InserisciAllegati(trans As SqlClient.SqlTransaction, ByRef IDAllegato As Integer, allegato As Allegato, Optional ByVal IdEnteFiglio As Integer = 0, Optional TipoOperazione As String = "") As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand()
        LogParametri.Clear()
        Try
            'Prendo gli allegati salvati in sessione

            SqlCmd.CommandText = "sp_InsAllegato"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Transaction = trans
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Clear()
            If IdEnteFiglio = 0 Then
                IdEnteFiglio = lblIdEnte.Value
            End If
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnteFiglio
            LogParametri.Add("@IdEnte", IdEnteFiglio)
            'Tipo allegato Atto di nomina 

            SqlCmd.Parameters.Add("@IdTipoAllegato", SqlDbType.Int).Value = allegato.IdTipoAllegato
            LogParametri.Add("@IdTipoAllegato", allegato.IdTipoAllegato)
            'Blob
            SqlCmd.Parameters.Add("@BinData", SqlDbType.VarBinary, -1).Value = allegato.Blob
            LogParametri.Add("@BinData", "BlobAllegato")
            'FileName
            SqlCmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = allegato.Filename
            LogParametri.Add("@FileName", allegato.Filename)
            'Hash
            SqlCmd.Parameters.Add("@Hash", SqlDbType.VarChar, 100).Value = allegato.Hash
            LogParametri.Add("@Hash", allegato.Hash)
            'FileLength
            SqlCmd.Parameters.Add("@FileLength", SqlDbType.BigInt, 100).Value = allegato.Filesize
            LogParametri.Add("@FileLength", allegato.Filesize)

            'DataInserimento
            SqlCmd.Parameters.Add("@DataIns", SqlDbType.DateTime, 100).Value = allegato.DataInserimento
            LogParametri.Add("@DataIns", allegato.DataInserimento)

            'UserNAme
            SqlCmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = Session("Utente").ToString
            LogParametri.Add("@UserName", Session("Utente").ToString)

            If TipoOperazione <> String.Empty Then
                SqlCmd.Parameters.Add("@TipoOperazione", SqlDbType.Char, 1).Value = TipoOperazione
                LogParametri.Add("@TipoOperazione", TipoOperazione)
            Else
                SqlCmd.Parameters.Add("@TipoOperazione", SqlDbType.Char, 1).Value = DBNull.Value
                LogParametri.Add("@TipoOperazione", String.Empty)
            End If

            If Not Session("IdEnteFaseArt") Is Nothing Then
                'IdEnteFaseDestinazione
                SqlCmd.Parameters.Add("@IdEnteFaseDestinazione", SqlDbType.Int).Value = Session("IdEnteFaseArt").ToString
                LogParametri.Add("@IdEnteFaseDestinazione", Session("IdEnteFaseArt").ToString)
            End If

            SqlCmd.Parameters.Add("@IdAllegato", SqlDbType.Int)
            SqlCmd.Parameters("@IdAllegato").Direction = ParameterDirection.Output

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            If (SqlCmd.Parameters("@Esito").Value()) <> 0 Then ' inseriemento ok
                IDAllegato = CInt(SqlCmd.Parameters("@IdAllegato").Value().ToString())
                Dim entita As New Logger.Data.Entity
                entita.Id = IDAllegato
                Select Case allegato.IdTipoAllegato
                    Case 6
                        entita.Name = "DELIBERA_COSTITUTIVAENTE"
                    Case 7
                        entita.Name = "ATTO_COSTITUTIVO"
                    Case 8
                        entita.Name = "STATUTO"
                    Case 11
                        entita.Name = "CARTA_IMPEGNO_ETICO"
                    Case 12
                        entita.Name = "CARTA_IMPEGNO"
                End Select
                Log.Information(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_ALLEGATI_CORRETTA, SqlCmd.Parameters("@messaggio").Value, LogParametri, entita)
                InserisciAllegati = True
                Exit Function
            Else
                trans.Rollback()
                msgErrore.Text = SqlCmd.Parameters("@messaggio").Value()
                Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_ALLEGATI_ERRATA, SqlCmd.Parameters("@messaggio").Value, LogParametri)
                InserisciAllegati = False
                Exit Function
            End If
        Catch ex As Exception
            msgErrore.Text = ex.Message
            trans.Rollback()
            msgErrore.Text = ex.Message
            Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_ALLEGATI_ERRATA, ex.Message, ex, LogParametri)
            InserisciAllegati = False
            Exit Function
        End Try

    End Function
    Private Function InserisciEnte(ByVal strAlbo As String, ByRef IdEnteFiglio As Integer) As Boolean
        'Inserimento dell'ente
        'Inserimento dell'accordo
        'Dim strAlbo As String = "SCU"
        'strAlbo = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))
        LogParametri.Clear()

        Dim SqlCmd As New SqlClient.SqlCommand
        Try

            SqlCmd.CommandText = "SP_ACCREDITAMENTO_INSERIMENTO_ENTE_FIGLIO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Transaction = MyTransaction
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte") 'IDEnte
            LogParametri.Add("@IdEnte", Session("IdEnte"))

            SqlCmd.Parameters.Add("@Albo", SqlDbType.Char, 3).Value = "SCU" 'Albo
            LogParametri.Add("@Albo", "SCU")

            SqlCmd.Parameters.Add("@Denominazione", SqlDbType.NVarChar, 200).Value = Trim(txtdenominazione.Text)
            LogParametri.Add("@Denominazione", Trim(txtdenominazione.Text))

            SqlCmd.Parameters.Add("@CodiceFiscale", SqlDbType.NVarChar, 50).Value = Trim(txtCodFis.Text)
            LogParametri.Add("@CodiceFiscale", Trim(txtCodFis.Text))


            If strAlbo = "SCN" Then
                If ddlTipologia.SelectedItem IsNot Nothing _
                AndAlso ddlTipologia.SelectedItem.Text <> String.Empty AndAlso ddlTipologia.SelectedItem.Text = "Privato" Then
                    SqlCmd.Parameters.Add("@Tipologia", SqlDbType.NVarChar, 255).Value = Trim(ddlTipologia.SelectedItem.Text)
                    LogParametri.Add("@Tipologia", ddlTipologia.SelectedItem.Text)
                ElseIf ddlTipologia.SelectedItem IsNot Nothing _
                AndAlso ddlTipologia.SelectedItem.Text <> String.Empty AndAlso ddlTipologia.SelectedItem.Text <> "Privato" Then
                    SqlCmd.Parameters.Add("@Tipologia", SqlDbType.NVarChar, 255).Value = Trim(ddlGiuridica.SelectedItem.Text)
                    LogParametri.Add("@Tipologia", Trim(ddlGiuridica.SelectedItem.Text))
                Else
                    SqlCmd.Parameters.Add("@Tipologia", SqlDbType.NVarChar, 255).Value = DBNull.Value
                    LogParametri.Add("@Tipologia", String.Empty)
                End If
            End If

            If strAlbo = "SCU" Then
                If ddlGiuridica.SelectedItem IsNot Nothing _
                AndAlso ddlGiuridica.SelectedItem.Text <> String.Empty Then
                    SqlCmd.Parameters.Add("@Tipologia", SqlDbType.NVarChar, 255).Value = Trim(ddlGiuridica.SelectedItem.Text)
                    LogParametri.Add("@Tipologia", Trim(ddlGiuridica.SelectedItem.Text))
                Else
                    SqlCmd.Parameters.Add("@Tipologia", SqlDbType.NVarChar, 255).Value = DBNull.Value
                    LogParametri.Add("@Tipologia", ddlGiuridica.SelectedItem.Text)
                End If
            End If

            SqlCmd.Parameters.Add("@NoteRichiestaRegistrazione", SqlDbType.NVarChar, 100).Value = Trim(txtRichiedente.Value)
            LogParametri.Add("@NoteRichiestaRegistrazione", txtRichiedente.Value)

            SqlCmd.Parameters.Add("@PrefissoTelefonoRichiestaRegistrazione", SqlDbType.NVarChar, 10).Value = Trim(txtprefisso.Text)
            LogParametri.Add("@PrefissoTelefonoRichiestaRegistrazione", Trim(txtprefisso.Text))

            SqlCmd.Parameters.Add("@TelefonoRichiestaRegistrazione", SqlDbType.NVarChar, 60).Value = Trim(txtTelefono.Text)
            LogParametri.Add("@TelefonoRichiestaRegistrazione", Trim(txtTelefono.Text))

            If txtPrefissoFax.Text = String.Empty Then
                SqlCmd.Parameters.Add("@PrefissoFax", SqlDbType.NVarChar, 10).Value = DBNull.Value
                LogParametri.Add("@PrefissoFax", String.Empty)
            Else
                SqlCmd.Parameters.Add("@PrefissoFax", SqlDbType.NVarChar, 10).Value = Trim(txtPrefissoFax.Text)
                LogParametri.Add("@PrefissoFax", txtPrefissoFax.Text)
            End If

            If txtFax.Text = String.Empty Then
                SqlCmd.Parameters.Add("@Fax", SqlDbType.NVarChar, 60).Value = DBNull.Value
                LogParametri.Add("@Fax", String.Empty)
            Else
                SqlCmd.Parameters.Add("@Fax", SqlDbType.NVarChar, 60).Value = Trim(txtFax.Text)
                LogParametri.Add("@Fax", txtFax.Text)
            End If

            If txtDataCostituzione.Text = String.Empty Then
                SqlCmd.Parameters.Add("@DataCostituzione", SqlDbType.DateTime).Value = DBNull.Value
                LogParametri.Add("@DataCostituzione", String.Empty)
            Else
                SqlCmd.Parameters.Add("@DataCostituzione", SqlDbType.DateTime).Value = Trim(txtDataCostituzione.Text)
                LogParametri.Add("@DataCostituzione", txtDataCostituzione.Text)
            End If

            If Trim(txthttp.Text) = String.Empty Then
                SqlCmd.Parameters.Add("@Http", SqlDbType.NVarChar, 100).Value = DBNull.Value
                LogParametri.Add("@Http", String.Empty)
            Else
                SqlCmd.Parameters.Add("@Http", SqlDbType.NVarChar, 100).Value = Trim(txthttp.Text)
                LogParametri.Add("@Http", Trim(txthttp.Text))
            End If

            If Trim(txtEmail.Text) = String.Empty Then
                SqlCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = DBNull.Value
                LogParametri.Add("@Email", String.Empty)
            Else
                SqlCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = Trim(txtEmail.Text)
                LogParametri.Add("@Email", Trim(txtEmail.Text))
            End If


            If lblIdClasse.Value <> "" Then
                SqlCmd.Parameters.Add("@IdClasseAccreditamento", SqlDbType.Int).Value = lblIdClasse.Value
                LogParametri.Add("@IdClasseAccreditamento", lblIdClasse.Value)
            Else
                SqlCmd.Parameters.Add("@IdClasseAccreditamento", SqlDbType.Int).Value = DBNull.Value
                LogParametri.Add("@IdClasseAccreditamento", String.Empty)
            End If

            SqlCmd.Parameters.Add("@IdTipoRelazione", SqlDbType.Int).Value = ddlrelazione.SelectedValue
            LogParametri.Add("@IdTipoRelazione", ddlrelazione.SelectedValue)


            If Trim(txtDataStipula.Text) = String.Empty Then
                SqlCmd.Parameters.Add("@DataStipula", SqlDbType.DateTime).Value = DBNull.Value
                LogParametri.Add("@DataStipula", String.Empty)
            Else
                SqlCmd.Parameters.Add("@DataStipula", SqlDbType.DateTime).Value = Trim(txtDataStipula.Text)
                LogParametri.Add("@DataStipula", txtDataStipula.Text)
            End If

            If Trim(txtDataScadenza.Text) = String.Empty Then
                SqlCmd.Parameters.Add("@DataScadenza", SqlDbType.DateTime).Value = DBNull.Value
                LogParametri.Add("@DataScadenza", String.Empty)
            Else
                SqlCmd.Parameters.Add("@DataScadenza", SqlDbType.DateTime).Value = Trim(txtDataScadenza.Text)
                LogParametri.Add("@DataScadenza", Trim(txtDataScadenza.Text))
            End If

            If Trim(txtUtenza.Value) = String.Empty Then
                SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar, 10).Value = DBNull.Value
                LogParametri.Add("@UsernameRichiesta", String.Empty)
            Else
                SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar, 10).Value = Trim(txtUtenza.Value)
                LogParametri.Add("@UsernameRichiesta", Trim(txtUtenza.Value))
            End If

            If Trim(txtEmailpec.Text) = String.Empty Then
                SqlCmd.Parameters.Add("@Pec", SqlDbType.NVarChar, 100).Value = DBNull.Value
                LogParametri.Add("@Pec", String.Empty)
            Else
                SqlCmd.Parameters.Add("@Pec", SqlDbType.NVarChar, 100).Value = Trim(txtEmailpec.Text)
                LogParametri.Add("@Pec", Trim(txtEmailpec.Text))
            End If

            'adc(02/03/2022)  INSERIMENTO ma non serve
            'If txtCDFRespLegal.Value <> "" And txtCodFiscRL.Text <> "" Then
            '    If txtCDFRespLegal.Value <> txtCodFiscRL.Text Then
            '        messaggioNotificaRL.Visible = True
            '        messaggioNotificaRL.Text = "Attenzione! in caso di Inserimento del Rappresentante Legale è necessario fare l’aggiornamento antimafia"
            '    End If
            'End If
            SqlCmd.Parameters.Add("@CodiceFiscaleRL", SqlDbType.NVarChar, 50).Value = Trim(txtCodFiscRL.Text)
            LogParametri.Add("@CodiceFiscaleRL", Trim(txtCodFiscRL.Text))

            SqlCmd.Parameters.Add("@DataNominaRL", SqlDbType.DateTime).Value = Trim(txtDataNominaRL.Text)
            LogParametri.Add("@DataNominaRL", Trim(txtDataNominaRL.Text))

            SqlCmd.Parameters.Add("@IdAllegatoAttoCostitutivo", SqlDbType.Int).Value = DBNull.Value
            LogParametri.Add("@IdAllegatoAttoCostitutivo", String.Empty)

            SqlCmd.Parameters.Add("@IdAllegatoStatuto", SqlDbType.Int).Value = DBNull.Value
            LogParametri.Add("@IdAllegatoStatuto", String.Empty)

            SqlCmd.Parameters.Add("@IdAllegatoDeliberaAdesione", SqlDbType.Int).Value = DBNull.Value
            LogParametri.Add("@IdAllegatoDeliberaAdesione", String.Empty)

            SqlCmd.Parameters.Add("@IdAllegatoImpegnoEtico", SqlDbType.Int).Value = DBNull.Value
            LogParametri.Add("@IdAllegatoImpegnoEtico", String.Empty)

            If (chkAttivita.Checked) Then
                SqlCmd.Parameters.Add("@AttivitaUltimiTreAnni", SqlDbType.Bit).Value = 1
                LogParametri.Add("@AttivitaUltimiTreAnni", 1)
            Else
                SqlCmd.Parameters.Add("@AttivitaUltimiTreAnni", SqlDbType.Bit).Value = 0
                LogParametri.Add("@AttivitaUltimiTreAnni", 0)
            End If


            If (chkFiniIstituzionali.Checked) Then
                SqlCmd.Parameters.Add("@AttivitaFiniIstituzionali", SqlDbType.Bit).Value = 1
                LogParametri.Add("@AttivitaFiniIstituzionali", 1)
            Else
                SqlCmd.Parameters.Add("@AttivitaFiniIstituzionali", SqlDbType.Bit).Value = 0
                LogParametri.Add("@AttivitaFiniIstituzionali", 0)
            End If

            If ddlTipologia.SelectedValue = "Privato" Then
                If (chkSenzaScopoLucro.Checked) Then
                    SqlCmd.Parameters.Add("@AttivitaSenzaLucro", SqlDbType.Bit).Value = 1
                    LogParametri.Add("@AttivitaSenzaLucro", 1)
                Else
                    SqlCmd.Parameters.Add("@AttivitaSenzaLucro", SqlDbType.Bit).Value = 0
                    LogParametri.Add("@AttivitaSenzaLucro", 0)
                End If
            Else
                SqlCmd.Parameters.Add("@AttivitaSenzaLucro", SqlDbType.Bit).Value = 0
                LogParametri.Add("@AttivitaSenzaLucro", 0)
            End If

            SqlCmd.Parameters.Add("@IdAllegatoImpegno", SqlDbType.Int).Value = DBNull.Value
            LogParametri.Add("@IdAllegatoImpegno", String.Empty)

            If ddlGiuridica.SelectedItem.Text = "Altro ai sensi dell’Art. 1, comma 2, D.Lgs. 30-3-2001 n. 165 (specificare)" Then
                If txtAltraTipoEnte.Text <> String.Empty Then
                    SqlCmd.Parameters.Add("@AltroTipoEnte", SqlDbType.NVarChar, 255).Value = txtAltraTipoEnte.Text
                Else
                    SqlCmd.Parameters.Add("@AltroTipoEnte", SqlDbType.NVarChar, 255).Value = DBNull.Value
                End If
            Else
                SqlCmd.Parameters.Add("@AltroTipoEnte", SqlDbType.NVarChar, 255).Value = DBNull.Value
            End If

            SqlCmd.Parameters.Add("@IdEnteFiglio", SqlDbType.Int)
            SqlCmd.Parameters("@IdEnteFiglio").Direction = ParameterDirection.Output
            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            If SqlCmd.Parameters("@Esito").Value = 0 Then
                Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_ERRATO, SqlCmd.Parameters("@messaggio").Value, LogParametri)
                MyTransaction.Rollback()
                MyTransaction = Nothing
                InserisciEnte = False
            Else
                IdEnteFiglio = CInt(SqlCmd.Parameters("@IdEnteFiglio").Value)
                Dim entita = New Logger.Data.Entity()
                entita.Id = IdEnteFiglio
                entita.Name = "Ente Accoglienza " & Trim(txtdenominazione.Text)
                Log.Information(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_CORRETTO, SqlCmd.Parameters("@messaggio").Value, LogParametri, entita)
            End If

        Catch ex As Exception
            msgErrore.Text = ex.Message
            MyTransaction.Rollback()
            MyTransaction = Nothing
            InserisciEnte = False
            Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_ERRATO, ex.Message, ex)
            InserisciEnte = False
            Exit Function
        End Try

        'Aggiorno Aree d'intervento per i settori selezionati inizio
        If (Not AggiornaSettoriAreeIntervento(IdEnteFiglio)) Then
            MyTransaction = Nothing
            InserisciEnte = False
            Exit Function
        End If
        'Aggiorno Aree d'intervento per i settori selezionati fine


        'InserimentoSettori(idente)
        If (InserimentoSedePrincipale(IdEnteFiglio)) Then
            'MyTransaction = Nothing
            InserisciEnte = True
            Exit Function
        Else
            MyTransaction = Nothing
            InserisciEnte = False
            Exit Function
        End If

    End Function

    Private Sub ModificaEnte(ByVal AlboEnte As String, ByVal IdAllegatoAttoCostitutivo As Integer,
                                    ByVal IdAllegatoStatuto As Integer,
                                    ByVal IdAllegatoDeliberaAdesione As Integer,
                                    ByVal IDAllegatoImpegnoEtico As Integer,
                                    ByVal IDAllegatoImpegno As Integer)
        'Realizzata da : Simona Cordella
        'Creata il:		21/08/2014
        'Funzionalità: richiamo store SP_ACCREDITAMENTO_MODIFICAMASCHERA_ENTE_FIGLIO per l'aggiornamento dell'anagrafica dell'ente
        LogParametri.Clear()
        Dim SqlCmd As New SqlClient.SqlCommand
        Try

            SqlCmd.CommandText = "SP_ACCREDITAMENTO_MODIFICAMASCHERA_ENTE_FIGLIO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Transaction = MyTransaction

            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = lblIdEnte.Value 'Request.QueryString("id")
            LogParametri.Add("@IdEnte", lblIdEnte.Value)
            SqlCmd.Parameters.Add("@Denominazione", SqlDbType.VarChar).Value = txtdenominazione.Text 'denominazioneente
            LogParametri.Add("@Denominazione", txtdenominazione.Text)
            'Codice Fiscale
            If Trim(txtCodFis.Text) <> "" Then
                SqlCmd.Parameters.Add("@CodiceFiscale", SqlDbType.VarChar).Value = txtCodFis.Text 'codice fiscale
                LogParametri.Add("@CodiceFiscale", txtCodFis.Text)
            Else
                SqlCmd.Parameters.Add("@CodiceFiscale", SqlDbType.VarChar).Value = DBNull.Value 'codice fiscale
                LogParametri.Add("@CodiceFiscale", String.Empty)
            End If
            If AlboEnte = "SCN" Then
                If ddlTipologia.SelectedItem.Text = "Privato" Then
                    SqlCmd.Parameters.Add("@Tipologia", SqlDbType.VarChar).Value = ddlTipologia.SelectedItem.Text
                    LogParametri.Add("@Tipologia", ddlTipologia.SelectedItem.Text)
                Else
                    SqlCmd.Parameters.Add("@Tipologia", SqlDbType.VarChar).Value = ddlGiuridica.SelectedItem.Text
                    LogParametri.Add("@Tipologia", ddlGiuridica.SelectedItem.Text)
                End If
            Else
                SqlCmd.Parameters.Add("@Tipologia", SqlDbType.VarChar).Value = ddlGiuridica.SelectedItem.Text
                LogParametri.Add("@Tipologia", ddlGiuridica.SelectedItem.Text)
            End If

            SqlCmd.Parameters.Add("@NoteRichiestaRegistrazione", SqlDbType.VarChar).Value = txtRichiedente.Value
            LogParametri.Add("@NoteRichiestaRegistrazione", txtRichiedente.Value)

            SqlCmd.Parameters.Add("@PrefissoTelefonoRichiestaRegistrazione", SqlDbType.VarChar).Value = txtprefisso.Text  'Prefisso telefono
            LogParametri.Add("@PrefissoTelefonoRichiestaRegistrazione", txtprefisso.Text)

            SqlCmd.Parameters.Add("@TelefonoRichiestaRegistrazione", SqlDbType.VarChar).Value = txtTelefono.Text 'telefono
            LogParametri.Add("@TelefonoRichiestaRegistrazione", txtTelefono.Text)

            If Trim(txtPrefissoFax.Text) <> "" Then
                SqlCmd.Parameters.Add("@PrefissoFax", SqlDbType.VarChar).Value = txtPrefissoFax.Text
                LogParametri.Add("@PrefissoFax", txtPrefissoFax.Text)
            Else
                SqlCmd.Parameters.Add("@PrefissoFax", SqlDbType.VarChar).Value = DBNull.Value
                LogParametri.Add("@PrefissoFax", String.Empty)
            End If
            'fax
            If Trim(txtFax.Text) <> "" Then
                SqlCmd.Parameters.Add("@Fax", SqlDbType.VarChar).Value = txtFax.Text
                LogParametri.Add("@Fax", txtFax.Text)
            Else
                SqlCmd.Parameters.Add("@Fax", SqlDbType.VarChar).Value = DBNull.Value
                LogParametri.Add("@Fax", String.Empty)
            End If

            If Trim(txtDataCostituzione.Text) <> "" Then
                SqlCmd.Parameters.Add("@DataCostituzione", SqlDbType.DateTime).Value = txtDataCostituzione.Text
                LogParametri.Add("@DataCostituzione", txtDataCostituzione.Text)
            Else
                SqlCmd.Parameters.Add("@DataCostituzione", SqlDbType.DateTime).Value = DBNull.Value
                LogParametri.Add("@DataCostituzione", String.Empty)
            End If

            If Trim(txtDataStipula.Text) <> "" Then
                SqlCmd.Parameters.Add("@DataStipula", SqlDbType.DateTime).Value = txtDataStipula.Text
                LogParametri.Add("@DataStipula", txtDataStipula.Text)
            Else
                SqlCmd.Parameters.Add("@DataStipula", SqlDbType.DateTime).Value = DBNull.Value
                LogParametri.Add("@DataStipula", String.Empty)
            End If

            If Trim(txtDataScadenza.Text) <> "" Then
                SqlCmd.Parameters.Add("@DataScadenza", SqlDbType.DateTime).Value = txtDataScadenza.Text
                LogParametri.Add("@DataScadenza", txtDataScadenza.Text)
            Else
                SqlCmd.Parameters.Add("@DataScadenza", SqlDbType.DateTime).Value = DBNull.Value
                LogParametri.Add("@DataScadenza", String.Empty)
            End If

            SqlCmd.Parameters.Add("@IdTipoRelazione", SqlDbType.Int).Value = ddlrelazione.SelectedValue
            LogParametri.Add("@IdTipoRelazione", ddlrelazione.SelectedValue)

            If Trim(txthttp.Text) = "" Then
                SqlCmd.Parameters.Add("@Http", SqlDbType.VarChar).Value = DBNull.Value
                LogParametri.Add("@Http", String.Empty)
            Else
                SqlCmd.Parameters.Add("@Http", SqlDbType.VarChar).Value = txthttp.Text
                LogParametri.Add("@Http", txthttp.Text)
            End If


            If Trim(txtEmail.Text) = "" Then
                SqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = DBNull.Value
                LogParametri.Add("@Email", String.Empty)
            Else
                SqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = txtEmail.Text
                LogParametri.Add("@Email", txtEmail.Text)
            End If

            'SEDE
            If txtIdSede.Value <> "" Then
                SqlCmd.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = txtIdSede.Value ' IdEnteSede
                LogParametri.Add("@IdEnteSede", txtIdSede.Value)
            Else
                SqlCmd.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = 0 ' IdEnteSede
                LogParametri.Add("@IdEnteSede", 0)
            End If
            'If Trim(Request.Form("txtIDComune")) = "" Then
            '    SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = txtIDComunes.Value
            'Else
            '    SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = Request.Form("txtIDComune")
            'End If
            SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = ddlComune.SelectedValue
            LogParametri.Add("@IdComune", ddlComune.SelectedValue)

            SqlCmd.Parameters.Add("@CAP", SqlDbType.VarChar).Value = txtCAP.Text ' CAP
            LogParametri.Add("@CAP", txtCAP.Text)

            SqlCmd.Parameters.Add("@Indirizzo", SqlDbType.VarChar).Value = txtIndirizzo.Text ' indirizzo
            LogParametri.Add("@Indirizzo", txtIndirizzo.Text)

            SqlCmd.Parameters.Add("@Civico", SqlDbType.VarChar).Value = txtCivico.Text ' Civico
            LogParametri.Add("@Civico", txtCivico.Text)

            SqlCmd.Parameters.Add("@DettaglioRecapito", SqlDbType.VarChar).Value = TxtDettaglioRecapito.Text ' DettaglioRecapito
            LogParametri.Add("@DettaglioRecapito", TxtDettaglioRecapito.Text)

            'SETTORI INTERVENTO
            Dim check As CheckBox
            check = dtgSettori.Rows.Item(0).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_A", SqlDbType.Bit).Value = 1
                LogParametri.Add("@Settore_A", 1)
            Else
                SqlCmd.Parameters.Add("@Settore_A", SqlDbType.Bit).Value = 0
                LogParametri.Add("@Settore_A", 0)
            End If


            check = dtgSettori.Rows.Item(1).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_B", SqlDbType.Bit).Value = 1
                LogParametri.Add("@Settore_B", 1)
            Else
                SqlCmd.Parameters.Add("@Settore_B", SqlDbType.Bit).Value = 0
                LogParametri.Add("@Settore_B", 0)
            End If
            check = dtgSettori.Rows.Item(2).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_C", SqlDbType.Bit).Value = 1
                LogParametri.Add("@Settore_C", 1)
            Else
                SqlCmd.Parameters.Add("@Settore_C", SqlDbType.Bit).Value = 0
                LogParametri.Add("@Settore_C", 0)
            End If
            check = dtgSettori.Rows.Item(3).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_D", SqlDbType.Bit).Value = 1
                LogParametri.Add("@Settore_D", 1)
            Else
                SqlCmd.Parameters.Add("@Settore_D", SqlDbType.Bit).Value = 0
                LogParametri.Add("@Settore_D", 0)
            End If

            check = dtgSettori.Rows.Item(4).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_E", SqlDbType.Bit).Value = 1
                LogParametri.Add("@Settore_E", 1)
            Else
                SqlCmd.Parameters.Add("@Settore_E", SqlDbType.Bit).Value = 0
                LogParametri.Add("@Settore_E", 0)
            End If

            If dtgSettori.Rows.Count = 7 Then 'SCU 
                check = dtgSettori.Rows.Item(5).FindControl("chkSeleziona")
                If check.Checked = True Then
                    SqlCmd.Parameters.Add("@Settore_F", SqlDbType.Bit).Value = 1
                    LogParametri.Add("@Settore_F", 1)
                Else
                    SqlCmd.Parameters.Add("@Settore_F", SqlDbType.Bit).Value = 0
                    LogParametri.Add("@Settore_F", 0)
                End If

                check = dtgSettori.Rows.Item(6).FindControl("chkSeleziona")

                If check.Checked = True Then
                    SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 1
                    LogParametri.Add("@Settore_G", 1)
                Else
                    SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 0
                    LogParametri.Add("@Settore_G", 0)
                End If
            Else

                SqlCmd.Parameters.Add("@Settore_F", SqlDbType.Bit).Value = 0
                LogParametri.Add("@Settore_F", 0)

                check = dtgSettori.Rows.Item(5).FindControl("chkSeleziona")
                If check.Checked = True Then
                    SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 1
                    LogParametri.Add("@Settore_G", 1)
                Else
                    SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 0
                    LogParametri.Add("@Settore_G", 0)
                End If
            End If

            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")
            LogParametri.Add("@UsernameRichiesta", Session("Utente"))

            If Trim(txtEmailpec.Text) = String.Empty Then
                SqlCmd.Parameters.Add("@Pec", SqlDbType.NVarChar, 100).Value = DBNull.Value
                LogParametri.Add("@Pec", String.Empty)
            Else
                SqlCmd.Parameters.Add("@Pec", SqlDbType.NVarChar, 100).Value = Trim(txtEmailpec.Text)
                LogParametri.Add("@Pec", Trim(txtEmailpec.Text))
            End If
            'ADC 02/03/2022   MODIFICA
            If txtCodFiscRL.Text <> "" Then
                If txtCDFRespLegal.Value <> txtCodFiscRL.Text Then
                    messaggioNotificaRL.Visible = True
                    messaggioNotificaRL.Text = "Attenzione! in caso di variazione del Rappresentante Legale è necessario fare l'aggiornamento antimafia"
                End If
            End If
            SqlCmd.Parameters.Add("@CodiceFiscaleRL", SqlDbType.NVarChar, 50).Value = Trim(txtCodFiscRL.Text)
            LogParametri.Add("@CodiceFiscaleRL", Trim(txtCodFiscRL.Text))

            If Trim(txtDataNominaRL.Text) <> String.Empty Then
                SqlCmd.Parameters.Add("@DataNominaRL", SqlDbType.DateTime).Value = Trim(txtDataNominaRL.Text)
                LogParametri.Add("@DataNominaRL", Trim(txtDataNominaRL.Text))
            Else
                SqlCmd.Parameters.Add("@DataNominaRL", SqlDbType.DateTime).Value = DBNull.Value
                LogParametri.Add("@DataNominaRL", String.Empty)
            End If

            If IdAllegatoAttoCostitutivo <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoAttoCostitutivo", SqlDbType.Int).Value = IdAllegatoAttoCostitutivo
                LogParametri.Add("@IdAllegatoAttoCostitutivo", IdAllegatoAttoCostitutivo)
            Else
                SqlCmd.Parameters.Add("@IdAllegatoAttoCostitutivo", SqlDbType.Int).Value = DBNull.Value
                LogParametri.Add("@IdAllegatoAttoCostitutivo", String.Empty)
            End If

            If IdAllegatoStatuto <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoStatuto", SqlDbType.Int).Value = IdAllegatoStatuto
                LogParametri.Add("@IdAllegatoStatuto", IdAllegatoStatuto)
            Else
                SqlCmd.Parameters.Add("@IdAllegatoStatuto", SqlDbType.Int).Value = DBNull.Value
                LogParametri.Add("@IdAllegatoStatuto", String.Empty)
            End If

            If IdAllegatoDeliberaAdesione <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoDeliberaAdesione", SqlDbType.Int).Value = IdAllegatoDeliberaAdesione
                LogParametri.Add("@IdAllegatoDeliberaAdesione", IdAllegatoDeliberaAdesione)
            Else
                SqlCmd.Parameters.Add("@IdAllegatoDeliberaAdesione", SqlDbType.Int).Value = DBNull.Value
                LogParametri.Add("@IdAllegatoDeliberaAdesione", String.Empty)
            End If

            If IDAllegatoImpegnoEtico <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoImpegnoEtico", SqlDbType.Int).Value = IDAllegatoImpegnoEtico
                LogParametri.Add("@IdAllegatoImpegnoEtico", IDAllegatoImpegnoEtico)
            Else
                SqlCmd.Parameters.Add("@IdAllegatoImpegnoEtico", SqlDbType.Int).Value = DBNull.Value
                LogParametri.Add("@IdAllegatoImpegnoEtico", String.Empty)
            End If

            If (chkAttivita.Checked) Then
                SqlCmd.Parameters.Add("@AttivitaUltimiTreAnni", SqlDbType.Bit).Value = 1
                LogParametri.Add("@AttivitaUltimiTreAnni", 1)
            Else
                SqlCmd.Parameters.Add("@AttivitaUltimiTreAnni", SqlDbType.Bit).Value = 0
                LogParametri.Add("@AttivitaUltimiTreAnni", 0)
            End If


            If (chkFiniIstituzionali.Checked) Then
                SqlCmd.Parameters.Add("@AttivitaFiniIstituzionali", SqlDbType.Bit).Value = 1
                LogParametri.Add("@AttivitaFiniIstituzionali", 1)
            Else
                SqlCmd.Parameters.Add("@AttivitaFiniIstituzionali", SqlDbType.Bit).Value = 0
                LogParametri.Add("@AttivitaFiniIstituzionali", 0)
            End If

            If ddlTipologia.SelectedValue = "Privato" Then
                If (chkSenzaScopoLucro.Checked) Then
                    SqlCmd.Parameters.Add("@AttivitaSenzaLucro", SqlDbType.Bit).Value = 1
                    LogParametri.Add("@AttivitaSenzaLucro", 1)
                Else
                    SqlCmd.Parameters.Add("@AttivitaSenzaLucro", SqlDbType.Bit).Value = 0
                    LogParametri.Add("@AttivitaSenzaLucro", 0)
                End If
            Else
                SqlCmd.Parameters.Add("@AttivitaSenzaLucro", SqlDbType.Bit).Value = 0
                LogParametri.Add("@AttivitaSenzaLucro", 0)
            End If

            If IDAllegatoImpegno <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoImpegno", SqlDbType.Int).Value = IDAllegatoImpegno
                LogParametri.Add("@IdAllegatoImpegno", IDAllegatoImpegno)
            Else
                SqlCmd.Parameters.Add("@IdAllegatoImpegno", SqlDbType.Int).Value = DBNull.Value
                LogParametri.Add("@IdAllegatoImpegno", String.Empty)
            End If

            If ddlGiuridica.SelectedItem.Text = "Altro ai sensi dell’Art. 1, comma 2, D.Lgs. 30-3-2001 n. 165 (specificare)" Then
                If txtAltraTipoEnte.Text <> String.Empty Then
                    SqlCmd.Parameters.Add("@AltroTipoEnte", SqlDbType.NVarChar, 255).Value = txtAltraTipoEnte.Text
                Else
                    SqlCmd.Parameters.Add("@AltroTipoEnte", SqlDbType.NVarChar, 255).Value = DBNull.Value
                End If
            Else
                SqlCmd.Parameters.Add("@AltroTipoEnte", SqlDbType.NVarChar, 255).Value = DBNull.Value
            End If

            If flgForzaVariazione Then
                SqlCmd.Parameters.Add("@FlagForzaturaVariazione", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@FlagForzaturaVariazione", SqlDbType.Bit).Value = 0
            End If

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            If SqlCmd.Parameters("@Esito").Value = 0 Then
                msgErrore.Text = SqlCmd.Parameters("@messaggio").Value()
                MyTransaction.Rollback()
                MyTransaction = Nothing
                Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_MODIFICA_ERRATA, SqlCmd.Parameters("@messaggio").Value, LogParametri)
                Exit Sub
            End If

        Catch ex As Exception
            msgErrore.Text = ex.Message
            MyTransaction.Rollback()
            MyTransaction = Nothing
            Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_MODIFICA_ERRATA, ex.Message, ex, LogParametri)
            Exit Sub
        Finally
            If ddlTipologia.SelectedItem.Text = "Pubblico" Then
                txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
            Else
                txtpianolocale.Value = ""
            End If
        End Try

        'Aggiorno Aree d'intervento per i settori selezionati inizio
        If (AggiornaSettoriAreeIntervento(lblIdEnte.Value)) Then
            MyTransaction.Commit()
            MyTransaction = Nothing
            Exit Sub
        Else
            MyTransaction = Nothing
            Exit Sub
        End If
        'Aggiorno Aree d'intervento per i settori selezionati fine

    End Sub

    Private Sub InserimentoSettori(ByVal IDEnteFiglio As Integer, trans As SqlTransaction)
        Dim x As Integer
        Dim myCommand1 As SqlClient.SqlCommand
        myCommand1 = New SqlClient.SqlCommand
        myCommand1.Connection = Session("conn")
        For x = 0 To dtgSettori.Rows.Count - 1
            Dim check As CheckBox
            check = dtgSettori.Rows.Item(x).FindControl("chkSeleziona")
            If check.Checked = True Then

                strsql = "Insert into EntiSettori (idente,idMacroAmbitoAttività,UsernameInserimento,DataInserimento) Values " &
                        "(" & IDEnteFiglio & "," & dtgSettori.Rows(x).Cells(1).Text & ",'" & Session("Utente") & "',getdate()) "

                myCommand1.CommandText = strsql
                myCommand1.ExecuteNonQuery()
                myCommand1.Dispose()
            End If
        Next
    End Sub

    Private Function InserimentoSedePrincipale(Optional ByVal myIdEnte As String = "") As Boolean

        Dim SqlCmd As New SqlClient.SqlCommand
        LogParametri.Clear()

        SqlCmd.CommandText = "SP_INSERIMENTO_SEDE_PRINCIPALE"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("Conn")
        SqlCmd.Transaction = MyTransaction

        Try

            SqlCmd.Parameters.Add("@IdEnteFiglio", SqlDbType.Int).Value = CInt(myIdEnte) 'IDEnte
            LogParametri.Add("@IdEnteFiglio", myIdEnte)

            SqlCmd.Parameters.Add("@Denominazione", SqlDbType.NVarChar, 200).Value = Trim(txtdenominazione.Text)
            LogParametri.Add("@Denominazione", Trim(txtdenominazione.Text))

            SqlCmd.Parameters.Add("@Indirizzo", SqlDbType.NVarChar, 255).Value = Trim(txtIndirizzo.Text)
            LogParametri.Add("@Indirizzo", Trim(txtIndirizzo.Text))

            SqlCmd.Parameters.Add("@DettaglioRecapito", SqlDbType.NVarChar, 30).Value = Trim(TxtDettaglioRecapito.Text)
            LogParametri.Add("@DettaglioRecapito", Trim(TxtDettaglioRecapito.Text))

            SqlCmd.Parameters.Add("@FlagIndirizzoValido", SqlDbType.Bit).Value = bandiera
            LogParametri.Add("@FlagIndirizzoValido", bandiera)

            SqlCmd.Parameters.Add("@Civico", SqlDbType.NVarChar, 50).Value = Trim(txtCivico.Text)
            LogParametri.Add("@Civico", Trim(txtCivico.Text))

            SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = ddlComune.SelectedValue
            LogParametri.Add("@IdComune", ddlComune.SelectedValue)

            SqlCmd.Parameters.Add("@Cap", SqlDbType.NVarChar, 10).Value = txtCAP.Text
            LogParametri.Add("@Cap", txtCAP.Text)

            SqlCmd.Parameters.Add("@UserNameStato", SqlDbType.NVarChar, 10).Value = Session("Utente")
            LogParametri.Add("@UserNameStato", Session("Utente"))

            SqlCmd.Parameters.Add("@IdEnteSede", SqlDbType.Int)
            SqlCmd.Parameters("@IdEnteSede").Direction = ParameterDirection.Output

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato

            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            If SqlCmd.Parameters("@Esito").Value = 0 Then
                MyTransaction.Rollback()
                Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_SEDEPRINCIPALE_NONCORRETTA, SqlCmd.Parameters("@messaggio").Value, LogParametri)
                InserimentoSedePrincipale = False
            Else
                Log.Information(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_SEDEPRINCIPALE_CORRETTA, SqlCmd.Parameters("@messaggio").Value, LogParametri)
                InserimentoSedePrincipale = True
            End If
        Catch ex As Exception
            MyTransaction.Rollback()
            Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_SEDEPRINCIPALE_NONCORRETTA, ex.Message, ex, LogParametri)
            msgErrore.Text = SqlCmd.Parameters("@messaggio").Value + ex.Message
            InserimentoSedePrincipale = False
        End Try

        'Dim strNull As String
        'Dim CmdGenerico As SqlClient.SqlCommand
        'Dim strIdEnteSede As String
        'Dim strDenSede As String = "SEDE PRINC. - "

        'strNull = "null"
        'strDenSede = strDenSede & " " & txtdenominazione.Text
        'strsql = "Insert into entiSedi(idente,denominazione,Indirizzo,DettaglioRecapito,FlagIndirizzoValido, "
        'strsql = strsql & "civico,idcomune,cap, "
        'strsql = strsql & "idStatoenteSede,UsernameStato,datacreazionerecord) values ( "
        'strsql = strsql & IIf(Request.QueryString("id") <> "", Request.QueryString("id"), myIdEnte) & ",'" & Replace(strDenSede, "'", "''") & "'"
        'strsql = strsql & ",'" & Replace(txtIndirizzo.Text, "'", "''") & "','" & Replace(TxtDettaglioRecapito.Text, "'", "''") & "'," & bandiera & ",'" & Replace(txtCivico.Text, "'", "''") & "'"
        'strsql = strsql & "," & ddlComune.SelectedValue & ",'" & txtCAP.Text & "',"
        ''trovo stato dell'ente Padre e lo inserisco
        'If Not dtrgenerico Is Nothing Then
        '    dtrgenerico.Close()
        '    dtrgenerico = Nothing
        'End If
        'dtrgenerico = ClsServer.CreaDatareader(" SELECT idstatoentesede FROM STATientisedi  where defaultStato=1 ", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'dtrgenerico.Read()
        'strsql = strsql & " " & dtrgenerico("idstatoentesede") & ",'" & Replace(Session("Utente"), "'", "''") & "',getdate())"
        'If Not dtrgenerico Is Nothing Then
        '    dtrgenerico.Close()
        '    dtrgenerico = Nothing
        'End If
        'CmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        ''inserisco relazione EntisedeTipi - entisedi
        'If Not dtrgenerico Is Nothing Then
        '    dtrgenerico.Close()
        '    dtrgenerico = Nothing
        'End If
        'dtrgenerico = ClsServer.CreaDatareader("select @@identity as identesede from entisedi", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'dtrgenerico.Read()
        'strIdEnteSede = dtrgenerico("identesede")
        'txtIdSede.Value = strIdEnteSede
        'strsql = "insert into entiseditipi (identesede,idtiposede) values " & _
        '    " (" & strIdEnteSede & ",1)"
        ''se si tratta di un ente figlio assegno ad una variabile pubblica l'id della sede appena inserita
        ''così che lo passo successivamente in fase di inclusione delle sedi
        'If Not dtrgenerico Is Nothing Then
        '    dtrgenerico.Close()
        '    dtrgenerico = Nothing
        'End If
        'CmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
    End Function

    Private Sub cmdElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdElimina.Click
        'Dim dtrgenerico As Data.SqlClient.SqlDataReader
        Dim strMsg As String = ""
        Session("okDelete") = "TRUE"
        If Session("Tipoutente") = "U" Then
            If AccreditamentoVincoloCancellazione(Session("IdEnte"), lblIdEnte.Value, strMsg) = False Then
                'apro div per invio messaggio e per richiedere la conferma della cacellazione
                divChiusuraEnte.Visible = True
                lblChiudiEnte.Text = strMsg
                cmdElimina.Visible = False
            Else
                cmdElimina.Visible = True
                divChiusuraEnte.Visible = False
                AccreditamentoEliminaEnteFiglio(lblIdEnte.Value)
                LoadMaschera()
            End If
        Else

            cmdElimina.Visible = True
            divChiusuraEnte.Visible = False
            AccreditamentoEliminaEnteFiglio(lblIdEnte.Value)
            LoadMaschera()
        End If

    End Sub

    Private Sub AccreditamentoEliminaEnteFiglio(ByVal IdEnte As Integer)
        'Creata il:		26/08/2014
        'Funzionalità: richiamo store SP_VINCOLO_SEDI_VERIFICA_CANCELLAZIONE per richiedere la cancellazione dell'accordo


        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            MyTransaction = CType(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), System.Data.SqlClient.SqlConnection).BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ELIMINAMASCHERA_ENTEFIGLIO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Transaction = MyTransaction
            SqlCmd.Parameters.Add("@IdEnte ", SqlDbType.Int).Value = IdEnte
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            If SqlCmd.Parameters("@Esito").Value = 1 Then
                MyTransaction.Commit()
                MyTransaction = Nothing
                msgConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                Exit Sub
            Else
                MyTransaction.Rollback()
                MyTransaction = Nothing
                msgErrore.Text = SqlCmd.Parameters("@messaggio").Value()
                msgErrore.Visible = True
                Exit Sub
            End If

        Catch ex As Exception
            MyTransaction.Rollback()
            MyTransaction = Nothing
            msgErrore.Text = ex.Message
        End Try
    End Sub
    Private Function AccreditamentoVincoloCancellazione(ByVal IdEnte As Integer, ByVal IdEnteAccoglienza As Integer, ByRef mess As String) As Boolean
        'Creata il:		17/04/2018
        'Funzionalità: richiamo store SP_VINCOLO_CANCELLAZIONE_SEDI_CONGRUENZA_REGIONI per verificare se la cancellazione richiesta non faccia decadere l'accreditamento

        Dim blbEsito As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "[SP_VINCOLO_SEDI_VERIFICA_CANCELLAZIONE]"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            SqlCmd.Parameters.Add("@IdSoggetto", SqlDbType.Int).Value = IdEnteAccoglienza
            SqlCmd.Parameters.Add("@TipoSoggetto", SqlDbType.VarChar).Value = "ENTE"


            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Valore", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Valore").Direction = ParameterDirection.Output
            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@Messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            blbEsito = SqlCmd.Parameters("@Valore").Value()
            mess = SqlCmd.Parameters("@Messaggio").Value()
            Return blbEsito
        Catch ex As Exception
            msgErrore.Text = ex.Message
        Finally

        End Try
    End Function
    Private Sub dtgAttivitaEnte_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAttivitaEnte.PageIndexChanged
        dtgAttivitaEnte.CurrentPageIndex = e.NewPageIndex
        CaricaGrid()

        dtgAttivitaEnte.SelectedIndex = -1
    End Sub

    Private Sub cmdAnnullaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAnnullaModifica.Click
        '20/08/2014
        'annullo le modifiche effettua nell'anagrafica dell'ente
        Dim x As Integer
        Dim AlboEnte As String

        For x = 0 To dtgSettori.Rows.Count - 1
            Dim check As CheckBox
            check = dtgSettori.Rows.Item(x).FindControl("chkSeleziona")
            check.Checked = False
        Next
        hdGiuridica.Value = String.Empty

        AnnullaModificaDocumenti(Request.QueryString("id"))

        Dim StrSql As String
        StrSql = "UPDATE Accreditamento_VariazioneEntiFiglio SET StatoVariazione=2, UsernameCancellazione ='" & Session("Utente") & "', DataCancellazione= getdate() where IDEnte =" & Request.QueryString("id") & " And statovariazione = 0"
        cmdGenerico = ClsServer.EseguiSqlClient(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        StrSql = "UPDATE Enti SET RichiestaModifica =0,UsernameRichiestaModifica=null,DataRichiestaModifica=null where IDEnte =" & Request.QueryString("id") & " "
        cmdGenerico = ClsServer.EseguiSqlClient(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        ClearDeliberaAdesione()
        ClearSessionAttoCostitutivo()
        ClearSessionStatuto()
        ClearSessionCartaImpegno()
        ClearSessionImpegnoEtico()
        Session("EsperienzeAreeSettoreAccordo") = Nothing

        'PER L'ENTE VENGONO STORICIZZATE LE ARRE D'INTERVENTO DI OGNI SETTORE INIZIO

        Try
            Dim SqlCmd As New SqlCommand
            SqlCmd.CommandText = "SP_UPD_AREE_ESPERIENZE_SETTORE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Clear()
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = lblIdEnte.Value
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar, 10).Value = Session("Utente")
            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output
            SqlCmd.ExecuteNonQuery()
            MessaggiConvalida(SqlCmd.Parameters("@messaggio").Value())
            If SqlCmd.Parameters("@Esito").Value = 0 Then
                Exit Sub
            End If
        Catch ex As Exception
            Exit Sub
        End Try
        AlboEnte = ClsUtility.TrovaAlboEnte(lblIdEnte.Value, Session("Conn"))
        CaricaSettori(AlboEnte)

        msgConferma.Text = "Annullamento modifica effettuato con successo."
        LoadMaschera()
        Dim identefiglio As Integer = IIf(Request.QueryString("id") = Nothing, 0, Request.QueryString("id"))
        If identefiglio > 0 Then EvidenziaDatiModificati(identefiglio)
    End Sub

    Private Sub cmdAnnullaCancellazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAnnullaCancellazione.Click
        AccreditamentoAnnullaEliminaEnteFiglio(lblIdEnte.Value)
        LoadMaschera()
    End Sub

    Private Sub AccreditamentoAnnullaEliminaEnteFiglio(ByVal IdEnte As Integer)
        'Creata il:		26/08/2014
        'Funzionalità: richiamo store SP_ACCREDITAMENTO_ANNULLAELIMINAMASCHERA_ENTEFIGLIO per annullare la richiesta di cancellazione


        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ANNULLAELIMINAMASCHERA_ENTEFIGLIO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEnte ", SqlDbType.Int).Value = IdEnte
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            msgConferma.Text = SqlCmd.Parameters("@messaggio").Value()

        Catch ex As Exception
            msgErrore.Text = ex.Message
        Finally

        End Try
    End Sub

    Private Sub ddlProvincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvincia.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        SelComune.CaricaComuni(ddlComune, ddlProvincia.SelectedValue, Session("Conn"))
    End Sub

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager.ConnectionStrings("unscproduzionenewConnectionString").ConnectionString

        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = " Select Top 30 CAP_INDIRIZZI.Indirizzo as CityName FROM  CAP_INDIRIZZI WHERE (CAP_INDIRIZZI.Indirizzo LIKE '%" + prefixText.Replace("'", "''") + "%') and idcomune='" & contextKey & "'  ORDER BY CAP_INDIRIZZI.Indirizzo"
        cmd.Connection = conn
        conn.Open()

        Dim oReader As SqlDataReader = cmd.ExecuteReader
        Dim indirizzi As List(Of String) = New List(Of String)

        While oReader.Read
            indirizzi.Add(oReader.GetString(0))
        End While


        If Not oReader Is Nothing Then
            oReader.Close()
            oReader = Nothing
            conn.Close()
        End If

        Return indirizzi


    End Function

    Private Sub imgCap_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCap.Click

        Dim selCap As New clsSelezionaComune
        txtCAP.Text = selCap.RitornaCap(ddlComune.SelectedValue, txtIndirizzo.Text.Trim, txtCivico.Text.Trim, Session("conn"))

    End Sub

    Private Sub ChkEstero_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEstero.CheckedChanged

        Dim SelComune As New clsSelezionaComune
        Dim blnEstero As Boolean = ChkEstero.Checked
        SelComune.CaricaProvinciaNazione(ddlProvincia, blnEstero, Session("Conn"))
        ddlComune.Items.Clear()

        If blnEstero = True Then
            lblComune.Text = "Località"
            lblCAP.Text = "Codice località"
        Else
            lblComune.Text = "Comune"
            lblCAP.Text = "CAP"
        End If

    End Sub

    Private Sub ddlTipologia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipologia.SelectedIndexChanged

        Dim AlboEnte As String
        Dim Obbligatorio As String = "(*)"
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
        'If AlboEnte = "SCN" Then
        '    If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
        '        ddlGiuridica.Visible = True
        '        lblDDlGiuridica.Visible = True
        '    Else
        '        ddlGiuridica.Visible = False
        '        lblDDlGiuridica.Visible = False
        '    End If
        'Else
        '    ddlGiuridica.Visible = True
        '    lblDDlGiuridica.Visible = True
        '    CaricaDettaglioTipologiaEntiSCU(ddlTipologia.SelectedItem.Text, "COMBOTIPOLOGIA")
        'End If
        If AlboEnte = "SCN" Then
            If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
                ddlGiuridica.Visible = True
                lblDDlGiuridica.Visible = True
                rowNoDeliberaAdesione.Visible = True
                lblDeliberaAdesione.Text = Obbligatorio & lblDeliberaAdesione.Text
                If lblStatutoEnte.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblStatutoEnte.Text = Mid(lblStatutoEnte.Text, Obbligatorio.Length + 1, lblStatutoEnte.Text.Length)
                End If
                If lblAttoCostitutivoEnte.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblAttoCostitutivoEnte.Text = Mid(lblAttoCostitutivoEnte.Text, Obbligatorio.Length + 1, lblAttoCostitutivoEnte.Text.Length)
                End If
                divSenzaScopoLucro.Visible = False
                If ddlGiuridica.SelectedItem IsNot Nothing Then
                    If ddlGiuridica.SelectedItem.Text <> String.Empty AndAlso ddlGiuridica.SelectedItem.Text Like "Altro*" Then
                        divAltraTipologia.Visible = True
                    Else
                        divAltraTipologia.Visible = False
                    End If
                End If
            ElseIf ddlTipologia.SelectedValue.ToUpper = "PRIVATO" Then
                If Session("LoadedDeliberaAdesioneAccordo") Is Nothing Then
                    rowNoDeliberaAdesione.Visible = False
                Else
                    rowNoDeliberaAdesione.Visible = False
                    rowDeliberaAdesione.Visible = False
                    Session("LoadedDeliberaAdesioneAccordo") = Nothing
                End If
                divAltraTipologia.Visible = False
                txtAltraTipoEnte.Text = String.Empty
                divSenzaScopoLucro.Visible = True
                ddlGiuridica.Visible = False
                lblDDlGiuridica.Visible = False
                rowNoDeliberaAdesione.Visible = False
                lblAttoCostitutivoEnte.Text = Obbligatorio & lblAttoCostitutivoEnte.Text
                lblStatutoEnte.Text = Obbligatorio & lblStatutoEnte.Text
                If lblDeliberaAdesione.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblDeliberaAdesione.Text = Mid(lblDeliberaAdesione.Text, Obbligatorio.Length + 1, lblDeliberaAdesione.Text.Length)
                End If
            Else
                ddlGiuridica.Visible = False
                lblDDlGiuridica.Visible = False
                divAltraTipologia.Visible = False
                txtAltraTipoEnte.Text = String.Empty
            End If

        Else
            If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
                rowNoDeliberaAdesione.Visible = True
                ddlGiuridica.Visible = True
                lblDDlGiuridica.Visible = True
                lblDeliberaAdesione.Text = Obbligatorio & lblDeliberaAdesione.Text
                If lblStatutoEnte.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblStatutoEnte.Text = Mid(lblStatutoEnte.Text, Obbligatorio.Length + 1, lblStatutoEnte.Text.Length)
                End If
                If lblAttoCostitutivoEnte.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblAttoCostitutivoEnte.Text = Mid(lblAttoCostitutivoEnte.Text, Obbligatorio.Length + 1, lblAttoCostitutivoEnte.Text.Length)
                End If
                CaricaDettaglioTipologiaEntiSCU(ddlTipologia.SelectedItem.Text, "COMBOTIPOLOGIA")
                divSenzaScopoLucro.Visible = False
                If ddlGiuridica.SelectedItem IsNot Nothing Then
                    If ddlGiuridica.SelectedItem.Text <> String.Empty AndAlso ddlGiuridica.SelectedItem.Text Like "Altro*" Then
                        divAltraTipologia.Visible = True
                    Else
                        divAltraTipologia.Visible = False
                    End If
                End If
            ElseIf ddlTipologia.SelectedValue.ToUpper = "PRIVATO" Then
                If Session("LoadedDeliberaAdesioneAccordo") Is Nothing Then
                    rowNoDeliberaAdesione.Visible = False
                Else
                    rowNoDeliberaAdesione.Visible = False
                    rowDeliberaAdesione.Visible = False
                    Session("LoadedDeliberaAdesioneAccordo") = Nothing
                End If
                ddlGiuridica.Visible = True
                lblDDlGiuridica.Visible = True
                divAltraTipologia.Visible = False
                txtAltraTipoEnte.Text = String.Empty
                lblAttoCostitutivoEnte.Text = Obbligatorio & lblAttoCostitutivoEnte.Text
                lblStatutoEnte.Text = Obbligatorio & lblStatutoEnte.Text
                If lblDeliberaAdesione.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblDeliberaAdesione.Text = Mid(lblDeliberaAdesione.Text, Obbligatorio.Length + 1, lblDeliberaAdesione.Text.Length)
                End If
                CaricaDettaglioTipologiaEntiSCU(ddlTipologia.SelectedItem.Text, "COMBOTIPOLOGIA")
                divSenzaScopoLucro.Visible = True
            Else
                ddlGiuridica.Visible = False
                lblDDlGiuridica.Visible = False
                divAltraTipologia.Visible = False
                txtAltraTipoEnte.Text = String.Empty
            End If

        End If
        If ddlTipologia.SelectedValue = "Privato" Then
            chkSenzaScopoLucro.Visible = True
        Else
            chkSenzaScopoLucro.Checked = False
            chkSenzaScopoLucro.Visible = False
        End If

    End Sub

    Private Sub cmdVisualizzaDatiAccreditati_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdVisualizzaDatiAccreditati.Click

        Dim IdEnteFiglio As String
        IdEnteFiglio = Request.QueryString("id")

        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open('informazionientefiglio.aspx?IdEnteFiglio=" & IdEnteFiglio & "','info','height=600,width=800,scrollbars=yes,resizable=yes')")
        Response.Write("</script>")

    End Sub

    Function IsUnchangedSettoreEsperienze(RigaDati As DataRow, EsperienzeAreeSettore As List(Of clsEsperienzeAree)) As Boolean
        'Questa funzione restituisce true se il confronto tra i dati (in maschera contenuti nella EsperienzeAreeSettore solo se ci sono esperienze inserite) 
        'che verranno salvati sono uguali alla riga nel datatable
        Dim _idSettore As Integer

        If EsperienzeAreeSettore.Count = 0 Then 'Non c'è nessuna esperienza per nessun settore in maschera
            If RigaDati.Item("IDSETTORE") IsNot DBNull.Value Then
                Return False    'il settore aveva esperienze caricate
            Else
                Return True     'il settore non aveva esperienze caricate
            End If
        End If

        'se il settore aveva esperienze caricate
        If RigaDati.Item("IDSETTORE") IsNot DBNull.Value Then
            _idSettore = Integer.Parse(RigaDati.Item("IDSETTORE"))
            'se non esistono dati in maschera per il settore
            If Not EsperienzeAreeSettore.Exists(Function(s) s.IdSettore = Integer.Parse(RigaDati.Item("IDMACROATTIVITA"))) Then
                Return False
            Else
                'ci sono sia esperienze caricate che dati in maschera
                'bisogna testare campo per campo
            End If
        Else
            'il settore non aveva esperienze caricate
            If EsperienzeAreeSettore.Exists(Function(s) s.IdSettore = Integer.Parse(RigaDati.Item("IDMACROATTIVITA"))) Then
                'esistono i dati in maschera per il settore
                Return False
            Else
                'non esistono i dati in maschera per il settore
                Return True
            End If
        End If

        'rimangono da testare campo per campo i dati caricati con i dati in maschera
        Dim EsperienzaSettore = EsperienzeAreeSettore.Find((Function(esperienza As clsEsperienzeAree) esperienza.IdSettore = _idSettore))

        If RigaDati("AREEINTERVENTO") <> String.Join(",", EsperienzaSettore.AreeSelezionate) Then Return False 'settori di intervento diversi
        If Integer.Parse(RigaDati("III_ANNOESPERIENZA")) <> EsperienzaSettore.AnnoEsperienza(2) Then Return False
        If Integer.Parse(RigaDati("II_ANNOESPERIENZA")) <> EsperienzaSettore.AnnoEsperienza(1) Then Return False
        If Integer.Parse(RigaDati("I_ANNOESPERIENZA")) <> EsperienzaSettore.AnnoEsperienza(0) Then Return False
        If RigaDati("III_ESPERIENZA") <> EsperienzaSettore.DescrizioneEsperienza(2) Then Return False
        If RigaDati("II_ESPERIENZA") <> EsperienzaSettore.DescrizioneEsperienza(1) Then Return False
        If RigaDati("I_ESPERIENZA") <> EsperienzaSettore.DescrizioneEsperienza(0) Then Return False

        Return True
    End Function

    Function VerificaSettoriCancellatiReinseriti() As String
        'Questa funzione restituisce, se esistono, la lista dei settori cancellati e poi reinseriti in maschera nella stessa fase
        'La cosa non è ammessa e deve essere fatta in due fasi di adeguamento successive, la prima per cancellare, la seconda per reinserire
        'Il confronto viene fatto usando il datatable nella Session("ElencoSettoriDB") che contiene i settori/esperienze letti da db prima di inserirli sulla maschera
        'Ci sono due casi
        '   1) si è deselezionato il settore e poi, senza salvare, selezionato di nuovo
        '   2) si è deselezionato il settore, salvato, e poi selezionato di nuovo
        'In entrambi i casi è importante il campo aggiuntivo "NuovoInserimento" contenuto nel datatable che, per ogni settore caricato, indica se è un settore appena 
        'inserito nella eventuale fase attualmente aperta (che quindi può essere eliminato e/o modificato come si vuole)

        Dim _ret As String = ""
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim _settoriDaDb As DataTable

        If Session("EsperienzeAreeSettoreAccordo") IsNot Nothing Then
            EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettoreAccordo"), List(Of clsEsperienzeAree))
        Else
            EsperienzeAreeSettore.Clear()
        End If

        If Session("ElencoSettoriDBAccordo") IsNot Nothing Then
            _settoriDaDb = Session("ElencoSettoriDBAccordo")

            If _settoriDaDb.Rows.Count > 0 Then
                'ciclo su tutti i settori selezionati in griglia
                For Each gRow As GridViewRow In dtgSettori.Rows
                    Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                    'se è selezionato
                    If check.Checked Then
                        'cerco il settore in quelli che avevo caricato in maschera
                        Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                        If HFIdMacroAttivita IsNot Nothing Then
                            Dim _riga As DataRow() = _settoriDaDb.Select("IDMACROATTIVITA=" & HFIdMacroAttivita.Value)
                            'se l'ho trovato e non è un nuovo inserimento
                            If _riga.Count > 0 AndAlso _riga(0).Item("NuovoInserimento") = "0" Then
                                'se è cambiato allora è un errore (è stato cancellato e reinserito, l'unico modo di cambiarlo)
                                If Not IsUnchangedSettoreEsperienze(_riga(0), EsperienzeAreeSettore) Then
                                    If String.IsNullOrEmpty(_ret) Then
                                        _ret = _riga(0).Item("Codifica")
                                    Else
                                        _ret += "," + _riga(0).Item("Codifica")
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        End If

        Return _ret
    End Function

    Function GetIdStatoEnteAccordo(IdEnte As Integer) As Integer
        Dim DA As New SqlDataAdapter
        Dim dt As New DataTable
        'msgErrore.Text = ""
        Dim ret As Integer = 6
        strsql = "SELECT isnull(idstatoente,6) as IdStatoEnte from enti where idente=" & IdEnte

        Dim SqlCmd = New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = strsql
            SqlCmd.CommandType = CommandType.Text
            SqlCmd.Connection = Session("Conn")
            DA.SelectCommand = SqlCmd
            DA.Fill(dt)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                ret = dt.Rows(0)("IdStatoEnte")
            End If
        Catch ex As Exception
            'msgErrore.Text = ex.Message

        End Try

        Return ret
    End Function

    Function ValidazioneServerSalva() As Boolean
        Dim p As Hashtable
        Dim result As Boolean = True
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim isVecchioEnte As Boolean
        Dim dtConfigurazione As New DataTable
        Dim DataInizioNuovaIscrizione As Date
        Dim DataFineBloccoControlli As Date
        Dim DataCreazioneEnte As Date
        Dim DA As New SqlDataAdapter
        Dim VerificaDate As Boolean = True
        Dim regexData As Regex = New Regex("(^(((0[1-9]|1[0-9]|2[0-8])[\/](0[1-9]|1[012]))|((29|30|31)[\/](0[13578]|1[02]))|((29|30)[\/](0[4,6,9]|11)))[\/](18|19|[2-9][0-9])\d\d$)|(^29[\/]02[\/](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)")
        Dim regex As Regex = New Regex("^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")
        Dim enteNuovoDaSalvare = False
        msgErrore.Text = ""

        flgForzaVariazione = False
        p = New Hashtable()
        p.Add("IDEnte", Session("IdEnte"))

        msgErrore.Text = ""
        strsql = "SELECT PARAMETRO,VALORE FROM CONFIGURAZIONI WHERE PARAMETRO IN ('DATAINIZIONUOVAISCRIZIONE','DATAFINEBLOCCOCONTROLLI')"

        Dim SqlCmd = New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = strsql
            SqlCmd.CommandType = CommandType.Text
            SqlCmd.Connection = Session("Conn")
            DA.SelectCommand = SqlCmd
            DA.Fill(dtConfigurazione)
        Catch ex As Exception
            msgErrore.Text = ex.Message
        End Try

        For Each dr As DataRow In dtConfigurazione.Rows
            If dr("PARAMETRO") = "DATAINIZIONUOVAISCRIZIONE" Then
                DataInizioNuovaIscrizione = CDate(dr("VALORE").ToString())
            End If

            If dr("PARAMETRO") = "DATAFINEBLOCCOCONTROLLI" Then
                DataFineBloccoControlli = CDate(dr("VALORE").ToString())
            End If
        Next

        If Session("DataCreazioneEnte") Is Nothing Then
            DataCreazioneEnte = Now()
        Else
            DataCreazioneEnte = CDate(Session("DataCreazioneEnte"))
        End If

        If DataCreazioneEnte < DataInizioNuovaIscrizione Then
            isVecchioEnte = True
        Else
            isVecchioEnte = False
        End If

        If Request.QueryString("azione") IsNot Nothing AndAlso Request.QueryString("azione") = "Ins" Then
            enteNuovoDaSalvare = True
        Else
            enteNuovoDaSalvare = False
        End If


        ''''ADC 02/03/2022 SE ente prima del 01/06/2021  THEN
        Dim dataAccr = CDate("01/06/2021")

        If DataCreazioneEnte >= dataAccr.Date Then
            If (isVecchioEnte AndAlso Date.Now.Date >= DataFineBloccoControlli) OrElse isVecchioEnte = False OrElse enteNuovoDaSalvare Then
                Dim DataNomina As Date
                Dim matchDataNominaRL As Match = regexData.Match(txtDataNominaRL.Text)
                If txtDataNominaRL.Text.Trim = String.Empty Then
                    msgErrore.Text = msgErrore.Text & "La 'Data di Nomina' del Rappresentante Legale non è stata valorizzata. Inserire la data nel formato GG/MM/AAAA."
                    msgErrore.Text = msgErrore.Text & " </br>"
                    result = False
                Else
                    If matchDataNominaRL.Success = False Then
                        msgErrore.Text = msgErrore.Text & "Il campo 'Data di Nomina' non è nel formato valido gg/mm/aaaa"
                        msgErrore.Text = msgErrore.Text & " </br>"
                        result = False
                    Else
                        If Date.TryParse(txtDataNominaRL.Text, DataNomina) = False Then
                            msgErrore.Text = msgErrore.Text & "Il campo 'Data di Nomina' non è nel formato valido gg/mm/aaaa"
                            msgErrore.Text = msgErrore.Text & " </br>"
                            result = False
                        End If
                    End If
                End If

                Dim ListaHash As New Hashtable
                Dim HashDoppi As New List(Of String)

                'Dim ListaHash As New List(Of String)

                'If Session("LoadedACAccordo") IsNot Nothing Then
                '    ListaHash.Add(DirectCast(Session("LoadedACAccordo"), Allegato).Hash)
                'End If

                'If Session("LoadedStatutoAccordo") IsNot Nothing Then
                '    ListaHash.Add(DirectCast(Session("LoadedStatutoAccordo"), Allegato).Hash)
                'End If

                'If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Or ddlTipologia.SelectedValue = String.Empty Then
                '    If Session("LoadedDeliberaAdesioneAccordo") IsNot Nothing Then
                '        ListaHash.Add(DirectCast(Session("LoadedDeliberaAdesioneAccordo"), Allegato).Hash)
                '    End If
                'End If

                'If Session("LoadedCartaImpegnoEticoAccordo") IsNot Nothing Then
                '    ListaHash.Add(DirectCast(Session("LoadedCartaImpegnoEticoAccordo"), Allegato).Hash)
                'End If


                'If Session("LoadedCartaImpegnoAccordo") IsNot Nothing Then
                '    ListaHash.Add(DirectCast(Session("LoadedCartaImpegnoAccordo"), Allegato).Hash)
                'End If
                If Session("LoadedACAccordo") IsNot Nothing Then
                    ListaHash.Add("AC", DirectCast(Session("LoadedACAccordo"), Allegato).Hash)
                End If

                If Session("LoadedStatutoAccordo") IsNot Nothing Then
                    ListaHash.Add("ST", DirectCast(Session("LoadedStatutoAccordo"), Allegato).Hash)
                End If

                If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Or ddlTipologia.SelectedValue = String.Empty Then
                    If Session("LoadedDeliberaAdesioneAccordo") IsNot Nothing Then
                        ListaHash.Add("DAC", DirectCast(Session("LoadedDeliberaAdesioneAccordo"), Allegato).Hash)
                    End If
                End If

                If Session("LoadedCartaImpegnoEticoAccordo") IsNot Nothing Then
                    ListaHash.Add("CIE", DirectCast(Session("LoadedCartaImpegnoEticoAccordo"), Allegato).Hash)
                End If


                If Session("LoadedCartaImpegnoAccordo") IsNot Nothing Then
                    ListaHash.Add("CI", DirectCast(Session("LoadedCartaImpegnoAccordo"), Allegato).Hash)
                End If

                'Verifico se ci sono allegati duplicati in maschera inizio
                'Dim conta As Integer
                'For Each item In ListaHash
                '    conta = 0
                '    For i = 0 To ListaHash.Count - 1
                '        If item = ListaHash(i) Then
                '            conta = conta + 1
                '            If conta > 1 Then
                '                msgErrore.Text = msgErrore.Text & "Un allegato con codice Hash '" & item & "' è stato inserito più di una volta in maschera per questo Ente"
                '                msgErrore.Text = msgErrore.Text & " </br>"
                '                result = False
                '                Exit For
                '            End If
                '        End If
                '    Next
                '    If conta > 1 Then
                '        Exit For
                '    End If
                'Next
                Dim doppione As Boolean = False
                For Each item As DictionaryEntry In ListaHash
                    For i = 0 To ListaHash.Count - 1
                        If (item.Key <> ListaHash.Keys(i)) AndAlso (item.Value = ListaHash.Values(i)) Then
                            If Not (HashDoppi.Contains(item.Value)) Then
                                If (item.Key <> "ST" And item.Key <> "AC") Then
                                    msgErrore.Text = msgErrore.Text & "Un allegato con codice Hash '" & item.Value & "' è stato inserito più di una volta in maschera per questo Ente  </br>"
                                    HashDoppi.Add(item.Value)
                                    doppione = True
                                ElseIf (ListaHash.Keys(i) <> "ST" And ListaHash.Keys(i) <> "AC") Then
                                    HashDoppi.Add(item.Value)
                                    msgErrore.Text = msgErrore.Text & "Un allegato con codice Hash '" & item.Value & "' è stato inserito più di una volta in maschera per questo Ente  </br>"
                                    doppione = True
                                End If
                            End If
                        End If
                    Next
                Next
                If doppione Then result = False
                'Verifico se ci sono allegati duplicati in maschera fine

                If ddlTipologia.SelectedValue <> String.Empty Then
                    If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
                        If Session("LoadedDeliberaAdesioneAccordo") Is Nothing Then
                            msgErrore.Text = msgErrore.Text & "Allegare la Delibera dell'Ente in accordo"
                            msgErrore.Text = msgErrore.Text & " </br>"
                            result = False
                        End If
                        If Session("LoadedACAccordo") IsNot Nothing AndAlso Session("LoadedStatutoAccordo") Is Nothing Then
                            msgErrore.Text = msgErrore.Text & "Allegare lo Statuto dell'Ente in accordo"
                            msgErrore.Text = msgErrore.Text & " </br>"
                            result = False
                        End If
                        If Session("LoadedACAccordo") Is Nothing AndAlso Session("LoadedStatutoAccordo") IsNot Nothing Then
                            msgErrore.Text = msgErrore.Text & "Allegare l'Atto Costitutivo dell'Ente in accordo"
                            msgErrore.Text = msgErrore.Text & " </br>"
                            result = False
                        End If
                    End If
                    If ddlTipologia.SelectedValue.ToUpper = "PRIVATO" Then
                        'modificato in data 2021-10-08 
                        'in caso di Ente religioso civilmente riconosciuto è obbligatorio solo uno tra Atto Costitutivo e Statuto
                        If ddlGiuridica.SelectedValue = 48 Then
                            If Session("LoadedStatutoAccordo") Is Nothing AndAlso Session("LoadedACAccordo") Is Nothing Then
                                msgErrore.Text = msgErrore.Text & "Allegare lo Statuto dell'Ente in accordo o Allegare l'Atto Costitutivo dell'Ente in accordo"
                                msgErrore.Text = msgErrore.Text & " </br>"
                                result = False
                            End If
                        Else
                            If Session("LoadedStatutoAccordo") Is Nothing Then
                                msgErrore.Text = msgErrore.Text & "Allegare lo Statuto dell'Ente in accordo"
                                msgErrore.Text = msgErrore.Text & " </br>"
                                result = False
                            End If
                            If Session("LoadedACAccordo") Is Nothing Then
                                msgErrore.Text = msgErrore.Text & "Allegare l'Atto Costitutivo dell'Ente in accordo"
                                msgErrore.Text = msgErrore.Text & " </br>"
                                result = False
                            End If
                        End If
                    End If
                End If


                If Session("LoadedCartaImpegnoEticoAccordo") Is Nothing Then
                    msgErrore.Text = msgErrore.Text & "Allegare la Carta Impegno Etico dell'Ente in accordo"
                    msgErrore.Text = msgErrore.Text & " </br>"
                    result = False
                End If

                If Not chkAttivita.Checked Then
                    msgErrore.Text = msgErrore.Text & "La casella dichiarante lo svolgimento delle attività da almeno 3 anni non è stata selezionata"
                    msgErrore.Text = msgErrore.Text & " </br>"
                    result = False
                End If

                If Not chkFiniIstituzionali.Checked Then
                    msgErrore.Text = msgErrore.Text & "La casella dichiarante la conformità  tra gli scopi istituzionali e le finalità dell'ente non è stata selezionata"
                    msgErrore.Text = msgErrore.Text & " </br>"
                    result = False
                End If

                If ddlTipologia.SelectedValue = "Privato" AndAlso Not chkSenzaScopoLucro.Checked Then
                    msgErrore.Text = msgErrore.Text & "La casella dichiarante di essere senza scopo di lucro non è stata selezionata"
                    msgErrore.Text = msgErrore.Text & " </br>"
                    result = False
                End If

                If txtCodFiscRL.Text.Trim = String.Empty Then
                    msgErrore.Text = msgErrore.Text & "Inserire il Codice Fiscale del Rappresentante Legale."
                    msgErrore.Text = msgErrore.Text & " </br>"
                    result = False
                Else
                    If Not CheckCodFiscale() Then
                        msgErrore.Text += "Il campo Codice Fiscale del Rappresentante Legale è formalmente errato</br>"
                        msgErrore.Text = msgErrore.Text & " </br>"
                        result = False
                    End If
                End If
            Else
                If Session("IdStatoEnte") <> "6" AndAlso isVecchioEnte AndAlso enteNuovoDaSalvare = False Then 'Forzo l'applicativo alla variazione
                    flgForzaVariazione = True
                End If

                'controllo i settori
                If VerificaCheck() = False Then
                    msgErrore.ForeColor = Color.Red
                    msgErrore.Text = "E' necessario indicare almeno un Settore di Intervento."
                    msgErrore.Text = msgErrore.Text & " </br>"
                    Exit Function
                End If
            End If

        Else   'ADC 02/03/2022

            If Session("IdStatoEnte") <> "6" AndAlso isVecchioEnte AndAlso enteNuovoDaSalvare = False Then 'Forzo l'applicativo alla variazione
                flgForzaVariazione = True
            End If

            'controllo i settori
            If VerificaCheck() = False Then
                msgErrore.ForeColor = Color.Red
                msgErrore.Text = "E' necessario indicare almeno un Settore di Intervento."
                msgErrore.Text = msgErrore.Text & " </br>"
                Exit Function
            End If


        End If   'ADC 02/03/2022

        If txtdenominazione.Text.Trim = String.Empty Then
            msgErrore.Text = msgErrore.Text & "Inserire la Denominazione dell'Ente."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If txtCodFis.Text.Trim = String.Empty Then
            msgErrore.Text = msgErrore.Text & "Inserire il Codice Fiscale."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If


        If ddlTipologia.SelectedItem IsNot Nothing Then
            If ddlTipologia.SelectedItem.Text = String.Empty Then
                msgErrore.Text = msgErrore.Text & "Selezionare il Tipo Ente (Pubblico - Privato)"
                msgErrore.Text = msgErrore.Text & " </br>"
                result = False
            End If
        End If

        If ddlGiuridica.SelectedItem IsNot Nothing Then
            If ddlGiuridica.SelectedItem.Text = String.Empty Then
                msgErrore.Text = msgErrore.Text & "Selezionare la Denominazione tipologia (Università - Fondazione etc.)"
                msgErrore.Text = msgErrore.Text & " </br>"
                result = False
            Else
                If ddlGiuridica.SelectedItem.Text = "Altro ai sensi dell’Art. 1, comma 2, D.Lgs. 30-3-2001 n. 165 (specificare)" Then
                    If txtAltraTipoEnte.Text = String.Empty Then
                        msgErrore.Text = msgErrore.Text & "Inserire il campo 'Altra tipologia di ente'"
                        msgErrore.Text = msgErrore.Text & " </br>"
                        result = False
                    Else
                        If txtAltraTipoEnte.Text.Length < 2 Then
                            msgErrore.Text = msgErrore.Text & "Il campo 'Altra tipologia di ente' non può essere minore di due caratteri"
                            msgErrore.Text = msgErrore.Text & " </br>"
                            result = False
                        End If
                    End If
                End If
            End If
        Else
            msgErrore.Text = msgErrore.Text & "Selezionare il Tipo Ente per caricare l'elenco delle denominazioni delle tipologie "
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If txtprefisso.Text.Trim = String.Empty Then
            msgErrore.Text = msgErrore.Text & "Inserire il prefisso Telefonico."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        Dim prefissoTelefono As Integer
        Dim prefissoTelefonoInteger As Boolean
        prefissoTelefonoInteger = Integer.TryParse(txtprefisso.Text.Trim, prefissoTelefono)

        If prefissoTelefonoInteger = False Then
            msgErrore.Text = msgErrore.Text & "Il Prefisso Telefono puo' contenere solo numeri."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If txtTelefono.Text.Trim = String.Empty Then
            msgErrore.Text = msgErrore.Text & "Inserire il numero Telefonico."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        Dim numeroTelefono As Int64
        Dim numeroTelefonoInteger As Boolean
        numeroTelefonoInteger = Int64.TryParse(txtTelefono.Text.Trim, numeroTelefono)

        If numeroTelefonoInteger = False Then
            msgErrore.Text = msgErrore.Text & "Il Telefono puo' contenere solo numeri."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        Dim prefissoFax As Integer
        Dim prefissoFaxInteger As Boolean
        prefissoFaxInteger = Integer.TryParse(txtPrefissoFax.Text.Trim, prefissoFax)

        If txtPrefissoFax.Text.Trim <> String.Empty AndAlso prefissoFaxInteger = False Then
            msgErrore.Text = msgErrore.Text & "Il Prefisso Fax può contenere solo numeri."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        Dim numeroFax As Int64
        Dim numeroFaxInteger As Boolean
        numeroFaxInteger = Int64.TryParse(txtFax.Text.Trim, numeroFax)

        If txtFax.Text.Trim <> String.Empty AndAlso numeroFaxInteger = False Then
            msgErrore.Text = msgErrore.Text & "Il Fax puo' contenere solo numeri."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If


        Dim matchPEC As Match = regex.Match(txtEmailpec.Text.Trim)
        Dim match As Match = regex.Match(txtEmail.Text.Trim)

        Dim machDataCostituzione As Match = regexData.Match(txtDataCostituzione.Text)
        Dim machDataStipula As Match = regexData.Match(txtDataStipula.Text)
        Dim machDataScadenza As Match = regexData.Match(txtDataScadenza.Text)


        If txtEmail.Text.Trim <> String.Empty AndAlso match.Success = False Then
            msgErrore.Text = msgErrore.Text & "Il campo 'Email' non è valido."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If txtEmailpec.Text.Trim <> String.Empty AndAlso matchPEC.Success = False Then
            msgErrore.Text = msgErrore.Text & "Il campo 'Email PEC' non e' nel formato valido."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        Dim DataCostituzione As Date
        If txtDataCostituzione.Text.Trim <> String.Empty Then
            If machDataCostituzione.Success = False Then
                msgErrore.Text = msgErrore.Text & "Il campo 'Data Costituzione' non è nel formato valido gg/mm/aaaa"
                msgErrore.Text = msgErrore.Text & " </br>"
                VerificaDate = False
                result = False
            Else
                If Date.TryParse(txtDataCostituzione.Text, DataCostituzione) = False Then
                    msgErrore.Text = msgErrore.Text & "Il campo 'Data Costituzione' non è nel formato valido gg/mm/aaaa"
                    msgErrore.Text = msgErrore.Text & " </br>"
                    VerificaDate = False
                    result = False
                End If
            End If
        End If

        Dim DataStipula As Date
        If txtDataStipula.Text.Trim <> String.Empty Then
            If machDataStipula.Success = False Then
                msgErrore.Text = msgErrore.Text & "Il campo 'Data di Stipula' non è nel formato valido gg/mm/aaaa"
                msgErrore.Text = msgErrore.Text & " </br>"
                VerificaDate = False
                result = False
            Else
                If Date.TryParse(txtDataStipula.Text, DataStipula) = False Then
                    msgErrore.Text = msgErrore.Text & "Il campo 'Data di Stipula' non è nel formato valido gg/mm/aaaa"
                    msgErrore.Text = msgErrore.Text & " </br>"
                    VerificaDate = False
                    result = False
                End If
            End If
        End If

        Dim DataScadenza As Date
        If txtDataScadenza.Text.Trim <> String.Empty Then
            If machDataScadenza.Success = False Then
                msgErrore.Text = msgErrore.Text & "Il campo 'Data di Scadenza' non è nel formato valido gg/mm/aaaa"
                msgErrore.Text = msgErrore.Text & " </br>"
                VerificaDate = False
                result = False
            Else
                If Date.TryParse(txtDataScadenza.Text, DataScadenza) = False Then
                    msgErrore.Text = msgErrore.Text & "Il campo 'Data di Scadenza' non è nel formato valido gg/mm/aaaa"
                    msgErrore.Text = msgErrore.Text & " </br>"
                    VerificaDate = False
                    result = False
                End If
            End If
        End If


        If VerificaDate Then
            Dim msgErr As String = msgErrore.Text
            msgErr = LeggiData(result, msgErr)

            msgErrore.Text = msgErr
        End If

        If txtEmailpec.Text.Trim <> String.Empty _
           AndAlso txtEmail.Text.Trim <> String.Empty Then
            If txtEmail.Text.Trim = txtEmailpec.Text.Trim Then
                msgErrore.Text = msgErrore.Text & "Il campo 'Email e il campo Email PEC non possono essere uguali"
                msgErrore.Text = msgErrore.Text & " </br>"
                result = False
            End If
        End If

        If ddlrelazione.SelectedIndex <= 0 Then
            msgErrore.Text = msgErrore.Text & "Selezionare il Tipo di Relazione."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If txtIndirizzo.Text.Trim = String.Empty Then
            msgErrore.Text = msgErrore.Text & "Inserire l'indirizzo della sede Legale."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If txtCivico.Text.Trim = String.Empty Then
            msgErrore.Text = msgErrore.Text & "Inserire il civico dell'indirizzo della sede Legale."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If ddlProvincia.SelectedIndex <= 0 Then
            msgErrore.Text = msgErrore.Text & "Inserire la provincia della sede Legale."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If ddlComune.SelectedIndex <= 0 Then
            msgErrore.Text = msgErrore.Text & "Inserire il comune della sede Legale."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If txtCAP.Text.Trim = String.Empty And ChkEstero.Checked = False Then
            msgErrore.Text = msgErrore.Text & "Inserire il CAP della sede Legale."
            msgErrore.Text = msgErrore.Text & " </br>"
            result = False
        End If

        If ddlrelazione.SelectedItem IsNot Nothing Then
            If ddlrelazione.SelectedItem.Text = "Contratto" Then
                If txtDataStipula.Text = String.Empty Then
                    msgErrore.Text = msgErrore.Text & "La 'Data Stipula' nella relazione di tipo 'Contratto' è obbligatoria "
                    msgErrore.Text = msgErrore.Text & " </br>"
                    result = False
                End If
            End If
        End If

        'Lascio questo controllo in modo che sia valido per i vecchi e per i nuovi enti iscritti
        If enteNuovoDaSalvare OrElse GetIdStatoEnteAccordo(Request.QueryString("id")) = 6 Then 'Ente fase d'iscrizione
            'If Session("IdStatoEnte") = "6" Then 'Ente fase d'iscrizione
            If Session("EsperienzeAreeSettoreAccordo") Is Nothing Then
                msgErrore.Text = msgErrore.Text & "Non è stato selezionato nessun settore con le relative esperienze"
                msgErrore.Text = msgErrore.Text & " </br>"
                result = False
            Else
                EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettoreAccordo"), List(Of clsEsperienzeAree))
                If EsperienzeAreeSettore.Count = 0 Then
                    msgErrore.Text = msgErrore.Text & "Non è stato selezionato nessun settore con le relative esperienze"
                    msgErrore.Text = msgErrore.Text & " </br>"
                    result = False
                Else
                    'devo verificare che, per TUTTI i settori inseriti, ci siano esperienze inserite
                    For Each gRow As GridViewRow In dtgSettori.Rows
                        Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                        If check.Checked Then
                            Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                            If Not EsperienzeAreeSettore.Exists(Function(s) s.IdSettore = Integer.Parse(HFIdMacroAttivita.Value)) Then
                                msgErrore.Text = msgErrore.Text & "Sono stati selezionati dei settori di cui si richiede l'inserimento delle esperienze"
                                msgErrore.Text = msgErrore.Text & " </br>"
                                result = False
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If
        Else
            'devo verificare che, per TUTTI i settori NUOVI inseriti, ci siano esperienze inserite quindi testo anche la visibilità del pulsante inserisci
            EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettoreAccordo"), List(Of clsEsperienzeAree))
            For Each gRow As GridViewRow In dtgSettori.Rows
                Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                If check.Checked AndAlso cmdInserisci.Visible Then
                    If EsperienzeAreeSettore IsNot Nothing Then
                        Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                        If Not EsperienzeAreeSettore.Exists(Function(s) s.IdSettore = Integer.Parse(HFIdMacroAttivita.Value)) Then
                            msgErrore.Text = msgErrore.Text & "Sono stati selezionati dei settori di cui si richiede l'inserimento delle esperienze"
                            msgErrore.Text = msgErrore.Text & " </br>"
                            result = False
                            Exit For
                        End If
                    Else
                        msgErrore.Text = msgErrore.Text & "Sono stati selezionati dei settori di cui si richiede l'inserimento delle esperienze"
                        msgErrore.Text = msgErrore.Text & " </br>"
                        result = False
                        Exit For
                    End If
                End If
            Next
        End If

        Dim _settoriReinseriti As String = VerificaSettoriCancellatiReinseriti()
        If Not String.IsNullOrEmpty(_settoriReinseriti) Then
            If _settoriReinseriti.Length > 1 Then
                msgErrore.Text = msgErrore.Text & "I settori " & _settoriReinseriti & " sono stati cancellati e reinseriti. " & If(cmdAnnullaModifica.Visible, "Annullare le modifiche", "Impossibile salvare le modifiche")
                msgErrore.Text = msgErrore.Text & " </br>"
            Else
                msgErrore.Text = msgErrore.Text & "Il settore " & _settoriReinseriti & " è stato cancellato e reinserito. " & If(cmdAnnullaModifica.Visible, "Annullare le modifiche", "Impossibile salvare le modifiche")
                msgErrore.Text = msgErrore.Text & " </br>"
            End If
            result = False
        End If

        Return result

    End Function

    Function LeggiData(ByRef result As Boolean, ByRef msgErrore As String) As String

        Dim arrDataOggi As String()
        Dim arrDataStipula As String()
        Dim arrDataScadenza As String()
        Dim arrDataCostituzione As String()

        Dim dtOggi As Date
        Dim dtStipula As Date
        Dim dtScadenza As Date
        Dim dtCostituzione As Date

        Try

            arrDataOggi = txtdataoggi.Value.Split("/")
            dtOggi = New Date(arrDataOggi(2).Split(" ")(0), arrDataOggi(1), arrDataOggi(0))
            LogParametri.Add("DataOggi", txtdataoggi.Value)

            If txtDataStipula.Text <> String.Empty Then
                arrDataStipula = txtDataStipula.Text.Split("/")
                dtStipula = New Date(arrDataStipula(2), arrDataStipula(1), arrDataStipula(0))
                LogParametri.Add("DataStipula", txtDataStipula.Text)
            End If

            If txtDataScadenza.Text <> String.Empty Then
                arrDataScadenza = txtDataScadenza.Text.Split("/")
                dtScadenza = New Date(arrDataScadenza(2), arrDataScadenza(1), arrDataScadenza(0))
                LogParametri.Add("DataScadenza", txtDataScadenza.Text)
            End If

            If txtDataCostituzione.Text <> String.Empty Then
                arrDataCostituzione = txtDataCostituzione.Text.Split("/")
                dtCostituzione = New Date(arrDataCostituzione(2), arrDataCostituzione(1), arrDataCostituzione(0))
                LogParametri.Add("DataCostituzione", txtDataCostituzione)
            End If

            If (txtDataCostituzione.Text <> String.Empty And txtDataStipula.Text <> String.Empty) Then
                If (dtStipula < dtCostituzione) Then

                    msgErrore = msgErrore & "La data di Stipula non puo' essere antecedente alla data di Costituzione."
                    msgErrore = msgErrore & " </br>"
                    result = False
                End If
            End If

            If (txtDataStipula.Text <> String.Empty And txtDataScadenza.Text <> String.Empty) Then
                If (dtStipula >= dtScadenza) Then

                    msgErrore = msgErrore & "La data di Scadenza non puo' essere uguale o antecedente alla data di Stipula."
                    msgErrore = msgErrore & " </br>"
                    result = False
                End If
            End If

            If (txtDataStipula.Text = String.Empty And txtDataScadenza.Text <> String.Empty) Then

                msgErrore = msgErrore & "E' necessario inserire la data di Stipula se si vuole inserire la data di Scadenza."
                msgErrore = msgErrore & " </br>"
                result = False
            End If

            If (txtDataScadenza.Text <> String.Empty And dtScadenza < dtOggi) Then

                msgErrore = msgErrore & "La data di Scadenza non puo' essere antecedente alla data odierna."
                msgErrore = msgErrore & " </br>"
                result = False
            End If

        Catch ex As Exception
            Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_VALIDAZIONE_CAMPIINSERIMENTO_NONCORRETTA, ex.Message, ex, LogParametri)
            msgErrore = msgErrore & "Errore Generico"
            msgErrore = msgErrore & " </br>"
            result = False
        End Try

        Return msgErrore

    End Function
    Private Sub EvidenziaDatiModificati(ByVal IdEnteFiglio As Integer)
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strsql As String
        Dim MyCommand As SqlClient.SqlDataAdapter
        Dim DsSedi As DataSet = New DataSet
        Dim ALBOENTE As String = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

        strsql = " SELECT day(datacontrolloemail)as ggDCEmail,month(datacontrolloemail)as monthDCEmail,year(datacontrolloemail)as yearDCEmail," &
                "       day(datacontrollohttp)as ggDChttp,month(datacontrollohttp)as monthDChttp,year(datacontrollohttp)as yearDChttp, statienti.statoente," &
                "       classiaccreditamento.classeaccreditamento,classiaccreditamento.EntiInPartenariato, " &
                "       enti.Tipologia,enti.CodiceFiscale,isnull(enti.CodiceRegione,'Assente')as codiceregione," &
                "       enti.Datacontrolloemail,enti.Datacontrollohttp," &
                "       enti.dataCostituzione,enti.DataRicezioneCartacea,enti.Denominazione,enti.EstremiDeliberaStrutturaGestione," &
                "       enti.http,enti.Datacontrollohttp,enti.httpvalido," &
                "       enti.Email,enti.EmailCertificata,enti.Datacontrolloemail,enti.emailvalido,enti.PartitaIva,enti.NoteRichiestaRegistrazione," &
                "       enti.TelefonoRichiestaRegistrazione," &
                "       enti.DataNominaRL," &
                "       enti.CodiceFiscaleRL," &
                "       enti.PrefissoTelefonoRichiestaRegistrazione,enti.PrefissoFax,enti.Fax,enti.dataultimaClasseaccreditamento," &
                "       enti.idclasseaccreditamentorichiesta,enti.idclasseaccreditamento,isnull(enti.CodiceFiscaleArchivio,'') as CodiceFiscaleArchivio, isnull(enti.PartitaIVAArchivio,'') as PartitaIVAArchivio, " &
                "       entirelazioni.datascadenza,entirelazioni.datastipula,tipirelazioni.tipoRelazione " &
                " FROM enti " &
                " INNER JOIN classiaccreditamento on (classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamento)" &
                " INNER JOIN statienti on (statienti.idstatoente=enti.idstatoente) " &
                " INNER JOIN entirelazioni on enti.idente=entirelazioni.identefiglio " &
                " INNER JOIN tipirelazioni on entirelazioni.idTipoRelazione =tipirelazioni.idTipoRelazione " &
                " WHERE enti.idente=" & IdEnteFiglio
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        If dtrGenerico.HasRows = True Then
            If Not IsDBNull(dtrGenerico("denominazione")) Then
                If Not (String.Equals(txtdenominazione.Text, dtrGenerico("denominazione"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtdenominazione)
                Else
                    helper.RipristinaStyleDatiModificati(txtdenominazione)
                End If
            End If
            If ALBOENTE = "SCN" Then
                If Not IsDBNull(dtrGenerico("Tipologia")) Then
                    Dim tipologia As String = If(ddlTipologia.SelectedItem Is Nothing, "", ddlTipologia.SelectedItem.Text)
                    If (tipologia = "Privato") Then
                        If Not (String.Equals(tipologia, dtrGenerico("Tipologia"), StringComparison.InvariantCultureIgnoreCase)) Then
                            helper.ModificaStyleDatiModificati(ddlTipologia)
                            helper.ModificaStyleDatiModificati(ddlGiuridica)
                        Else
                            helper.RipristinaStyleDatiModificati(ddlTipologia)
                            helper.RipristinaStyleDatiModificati(ddlGiuridica)
                        End If
                    Else
                        Dim giuridica As String = If(ddlGiuridica.SelectedItem Is Nothing, "", ddlGiuridica.SelectedItem.Text)
                        If Not (String.Equals(giuridica, dtrGenerico("Tipologia"), StringComparison.InvariantCultureIgnoreCase)) Then
                            helper.ModificaStyleDatiModificati(ddlTipologia)
                            helper.ModificaStyleDatiModificati(ddlGiuridica)
                        Else
                            helper.RipristinaStyleDatiModificati(ddlTipologia)
                            helper.RipristinaStyleDatiModificati(ddlGiuridica)
                        End If
                    End If
                End If
            End If
            If ALBOENTE = "SCU" Then
                If Not IsDBNull(dtrGenerico("Tipologia")) Then
                    Dim giuridica As String = If(ddlGiuridica.SelectedItem Is Nothing, "", ddlGiuridica.SelectedItem.Text)
                    If Not (String.Equals(giuridica, dtrGenerico("Tipologia"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(ddlTipologia)
                        helper.ModificaStyleDatiModificati(ddlGiuridica)
                    Else
                        helper.RipristinaStyleDatiModificati(ddlTipologia)
                        helper.RipristinaStyleDatiModificati(ddlGiuridica)
                    End If
                End If
            End If

            If Not IsDBNull(dtrGenerico("CodiceFiscale")) Then
                If Not (String.Equals(txtCodFis.Text, dtrGenerico("CodiceFiscale"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtCodFis)
                Else
                    helper.RipristinaStyleDatiModificati(txtCodFis)
                End If
            End If
            If Not IsDBNull(dtrGenerico("CodiceFiscaleArchivio")) Then
                If Not (String.Equals(txtCodFisArchivio.Text, dtrGenerico("CodiceFiscaleArchivio"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtCodFisArchivio)
                Else
                    helper.RipristinaStyleDatiModificati(txtCodFisArchivio)
                End If
            End If
            If Not IsDBNull(dtrGenerico("dataCostituzione")) Then
                If Not (String.Equals(txtDataCostituzione.Text, dtrGenerico("dataCostituzione"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtDataCostituzione)
                Else
                    helper.RipristinaStyleDatiModificati(txtDataCostituzione)
                End If
            End If
            If Not IsDBNull(dtrGenerico("http")) Then
                If Not (String.Equals(txthttp.Text, dtrGenerico("http"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txthttp)
                Else
                    helper.RipristinaStyleDatiModificati(txthttp)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Email")) Then
                If Not (String.Equals(txtEmail.Text, dtrGenerico("Email"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtEmail)
                Else
                    helper.RipristinaStyleDatiModificati(txtEmail)
                End If
            End If
            'ADC MAIL PEC 24/03/2022
            If Not IsDBNull(dtrGenerico("EmailCertificata")) Then
                If Not (String.Equals(txtEmailpec.Text, dtrGenerico("EmailCertificata"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtEmailpec)
                Else
                    helper.RipristinaStyleDatiModificati(txtEmailpec)
                End If
            End If

            If Not IsDBNull(dtrGenerico("tipoRelazione")) Then
                If Not (String.Equals(ddlrelazione.SelectedItem.Text, dtrGenerico("tipoRelazione"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(ddlrelazione)
                Else
                    helper.RipristinaStyleDatiModificati(ddlrelazione)
                End If
            End If

            If Not IsDBNull(dtrGenerico("PrefissoTelefonoRichiestaRegistrazione")) Then
                Dim telefono As String = IIf(Not IsDBNull(dtrGenerico("PrefissoTelefonoRichiestaRegistrazione")), dtrGenerico("PrefissoTelefonoRichiestaRegistrazione"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("TelefonoRichiestaRegistrazione")), dtrGenerico("TelefonoRichiestaRegistrazione"), "")
                Dim telefonoVisualizzato As String = txtprefisso.Text & " - " & txtTelefono.Text
                If Not (String.Equals(telefonoVisualizzato, telefono, StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtTelefono)
                    helper.ModificaStyleDatiModificati(txtprefisso)
                Else
                    helper.RipristinaStyleDatiModificati(txtTelefono)
                    helper.RipristinaStyleDatiModificati(txtprefisso)
                End If
            End If

            If Not IsDBNull(dtrGenerico("PrefissoFax")) Then
                Dim fax As String = IIf(Not IsDBNull(dtrGenerico("PrefissoFax")), dtrGenerico("PrefissoFax"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("Fax")), dtrGenerico("Fax"), "")
                Dim faxVisualizzato As String = txtPrefissoFax.Text & " - " & txtFax.Text
                If Not (String.Equals(faxVisualizzato, fax, StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtFax)
                    helper.ModificaStyleDatiModificati(txtPrefissoFax)
                Else
                    helper.RipristinaStyleDatiModificati(txtFax)
                    helper.RipristinaStyleDatiModificati(txtPrefissoFax)
                End If
            End If

            If Not IsDBNull(dtrGenerico("datastipula")) Then
                If Not (String.Equals(txtDataStipula.Text, dtrGenerico("datastipula"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtDataStipula)
                Else
                    helper.RipristinaStyleDatiModificati(txtDataStipula)
                End If
            End If
            If Not IsDBNull(dtrGenerico("datascadenza")) Then
                If Not (String.Equals(txtDataScadenza.Text, dtrGenerico("datascadenza"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtDataScadenza)
                Else
                    helper.RipristinaStyleDatiModificati(txtDataScadenza)
                End If
            End If
            'ADC 03/05/2022 ----------------------- modifica raggi colorazione RL----------------------------------------------
            If IsDBNull(dtrGenerico("DataNominaRL")) Then   'se la data non c'e' nel db ma c'e' nelle variazioni
                If hdDataNominaRL.Value <> "" Then
                    helper.ModificaStyleDatiModificati(txtDataNominaRL)
                End If
            Else
                If Not IsDBNull(dtrGenerico("DataNominaRL")) Then ' se la data c'e' nel db e c'e' nelle variazioni lo confronto
                    If Not (String.Equals(txtDataNominaRL.Text, dtrGenerico("DataNominaRL"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(txtDataNominaRL)
                    Else
                        helper.RipristinaStyleDatiModificati(txtDataNominaRL)
                    End If

                End If
            End If

            If IsDBNull(dtrGenerico("CodiceFiscaleRL")) Then  'se il codicefiscaleRL non c'e' nel db ma c'e' nelle variazioni
                If txtCDFRespLegal.Value <> "" Then
                    helper.ModificaStyleDatiModificati(txtCodFiscRL)
                End If
            Else
                If Not IsDBNull(dtrGenerico("CodiceFiscaleRL")) Then  'se il codicefiscaleRL  c'e' nel db e c'e' nelle variazioni  lo confronto
                    If Not (String.Equals(txtCodFiscRL.Text, dtrGenerico("CodiceFiscaleRL"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(txtCodFiscRL)
                    Else
                        helper.RipristinaStyleDatiModificati(txtCodFiscRL)
                    End If
                End If
            End If


            '---------------------------------------------------------------------------------------------
        End If
            strsql = "SELECT  entisedi.identesede,entisedi.idcomune,entisedi.indirizzo, entisedi.civico,entisedi.DettaglioRecapito,entisedi.cap,comuni.denominazione as comune," &
            " provincie.provincia " &
            " FROM entisedi " &
            " INNER JOIN statientisedi on statientisedi.idstatoentesede=entisedi.idstatoentesede " &
            " INNER JOIN entiseditipi ON entisedi.IDEnteSede = entiseditipi.IDEnteSede " &
            " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
            " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " &
            " WHERE entisedi.IDEnte = " & IdEnteFiglio & " And entiseditipi.idtiposede = 1 and (statientisedi.attiva=1 or statientisedi.DefaultStato=1)"
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrGenerico.Read()
            If dtrGenerico.HasRows = True Then
                If Not IsDBNull(dtrGenerico("provincia")) Then
                    Dim provincia As String = If(ddlProvincia.SelectedItem Is Nothing, "", ddlProvincia.SelectedItem.Text)
                    If Not (String.Equals(provincia, dtrGenerico("provincia"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(ddlProvincia)
                    Else
                        helper.RipristinaStyleDatiModificati(ddlProvincia)
                    End If
                End If
                If Not IsDBNull(dtrGenerico("comune")) Then
                    Dim comune As String = If(ddlComune.SelectedItem Is Nothing, "", ddlComune.SelectedItem.Text)
                    If Not (String.Equals(comune, dtrGenerico("comune"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(ddlComune)
                    Else
                        helper.RipristinaStyleDatiModificati(ddlComune)
                    End If
                End If
                If Not IsDBNull(dtrGenerico("indirizzo")) Then
                    If Not (String.Equals(txtIndirizzo.Text, dtrGenerico("indirizzo"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(txtIndirizzo)
                    Else
                        helper.RipristinaStyleDatiModificati(txtIndirizzo)
                    End If
                End If
                If Not IsDBNull(dtrGenerico("Civico")) Then
                    If Not (String.Equals(txtCivico.Text, dtrGenerico("Civico"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(txtCivico)
                    Else
                        helper.RipristinaStyleDatiModificati(txtCivico)
                    End If
                End If
                If Not IsDBNull(dtrGenerico("Cap")) Then
                    If Not (String.Equals(txtCAP.Text, dtrGenerico("Cap"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(txtCAP)
                    Else
                        helper.RipristinaStyleDatiModificati(txtCAP)
                    End If
                End If
                If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                    If Not (String.Equals(TxtDettaglioRecapito.Text, dtrGenerico("DettaglioRecapito"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(TxtDettaglioRecapito)
                    Else
                        helper.RipristinaStyleDatiModificati(TxtDettaglioRecapito)
                    End If
                End If

            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            'Verifico Allegati Stefano Rossetti 05/07/2021
            Dim dtAllegati As DataTable
            Dim _allegato = New Allegato
            RipristinaPulsantiAllegati()

            strsql = String.Empty

            strsql += "SELECT coalesce(IdAllegatoAttoCostitutivo, 0) IdAllegatoAttoCostitutivo , coalesce(IdAllegatoStatuto,0) IdAllegatoStatuto ,"
            strsql += " coalesce(IdAllegatoDeliberaAdesione, 0) IdAllegatoDeliberaAdesione ,  coalesce(IdAllegatoImpegnoEtico, 0) IdAllegatoImpegnoEtico ,"
            strsql += " coalesce(IdAllegatoImpegno, 0) IdAllegatoImpegno"
            strsql += " From enti where IdEnte = " & IdEnteFiglio

        dtAllegati = ClsServer.DataTableGenerico(strsql, Session("Conn"))

            If dtAllegati IsNot Nothing _
            AndAlso dtAllegati.Rows.Count > 0 Then
                If Not IsNothing(Session("LoadedACAccordo")) Then
                    _allegato = Session("LoadedACAccordo")
                    If CInt(_allegato.Id.ToString()) <> CInt(dtAllegati.Rows(0)("IdAllegatoAttoCostitutivo")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadAttoCostitutivo)
                        helper.ModificaStyleDatiModificati(btnEliminaAttoCostitutitvo)
                        helper.ModificaStyleDatiModificati(btnModificaAttoCostitutivo)
                    End If
                End If

                If Not IsNothing(Session("LoadedStatutoAccordo")) Then
                    _allegato = Session("LoadedStatutoAccordo")
                    If CInt(_allegato.Id.ToString()) <> CInt(dtAllegati.Rows(0)("IdAllegatoStatuto")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadStatuto)
                        helper.ModificaStyleDatiModificati(btnEliminaStatuto)
                        helper.ModificaStyleDatiModificati(btnModificaStatuto)
                    End If
                End If

                If rowDeliberaAdesione.Visible Then
                    If Not IsNothing(Session("LoadedDeliberaAdesioneAccordo")) Then
                        _allegato = Session("LoadedDeliberaAdesioneAccordo")
                        If CInt(_allegato.Id.ToString()) <> CInt(dtAllegati.Rows(0)("IdAllegatoDeliberaAdesione")) Then
                            helper.ModificaStyleDatiModificati(btnDownloadDeliberaAdesione)
                            helper.ModificaStyleDatiModificati(btnEliminaDeliberaAdesione)
                            helper.ModificaStyleDatiModificati(btnModificaDeliberaAdesione)
                        End If
                    End If
                End If

                If Not IsNothing(Session("LoadedCartaImpegnoAccordo")) Then
                    _allegato = Session("LoadedCartaImpegnoAccordo")
                    If CInt(_allegato.Id.ToString()) <> CInt(dtAllegati.Rows(0)("IdAllegatoImpegno")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadCI)
                        helper.ModificaStyleDatiModificati(btnEliminaCI)
                        helper.ModificaStyleDatiModificati(btnModificaCI)
                    End If
                End If

                If Not IsNothing(Session("LoadedCartaImpegnoEticoAccordo")) Then
                    _allegato = Session("LoadedCartaImpegnoEticoAccordo")
                    If CInt(_allegato.Id.ToString()) <> CInt(dtAllegati.Rows(0)("IdAllegatoImpegnoEtico")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadCIE)
                        helper.ModificaStyleDatiModificati(btnEliminaCIE)
                        helper.ModificaStyleDatiModificati(btnModificaCIE)
                    End If
                End If

            End If

            ModificaStyleEntiSettori(IdEnteFiglio)
    End Sub

    Protected Sub ddlGiuridica_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGiuridica.SelectedIndexChanged
        hdGiuridica.Value = ddlGiuridica.SelectedValue

        If ddlGiuridica.SelectedItem IsNot Nothing And ddlGiuridica.SelectedItem.Text Like "Altro*" Or
          ddlGiuridica.SelectedItem IsNot Nothing And ddlGiuridica.SelectedItem.Text = "Altro ente" Then
            divAltraTipologia.Visible = True
        Else
            divAltraTipologia.Visible = False
            txtAltraTipoEnte.Text = String.Empty
        End If

    End Sub
    Sub ModificaStyleEntiSettori(IdEnteFiglio As Integer)
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree) 'contiene i dati caricati in maschera da confrontare con quelli reali 
        'NB NON contiene le macroaree che non hanno esperienze ma che sono selezionate (caso di enti preesistenti), vanno trovate in maschera

        If Session("EsperienzeAreeSettoreAccordo") IsNot Nothing Then EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettoreAccordo"), List(Of clsEsperienzeAree))

        Dim dtSettori As New DataTable
        'dtSettori = ClsServer.DataTableGenerico(strsql, Session("Conn"))

        Session("ElencoSettoriPrecedentiAccordo") = Nothing
        dtSettori = ElencoSettoriEsperienzePrecedenti()
        Session("ElencoSettoriPrecedentiAccordo") = dtSettori

        'devo ciclare la maschera per fare tutti i confronti
        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim chkMacroArea As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
            'individuo la macroarea in maschera
            Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
            'cerco la macroarea nei dati reali  il check precedente (dati reali) è sul campo IDMACROATTIVITA
            Dim _row As DataRow() = dtSettori.Select("IDMACROATTIVITA=" + HFIdMacroAttivita.Value)  'cerco il campo che indica che il settore era selezionato

            'controllo se è stata selezionata in maschera e confronto con il dato reale
            If (_row.Length = 0 And chkMacroArea.Checked) Or (_row.Length > 0 And Not chkMacroArea.Checked) Then
                helper.ModificaStyleDatiModificati(chkMacroArea)
            Else
                helper.RipristinaStyleDatiModificati(chkMacroArea)
            End If

            'uso il trucco di confrontare con le esperienze precedenti SOLO se si può modificarle cioè è abilitato il pulsante inserisci
            'se si dovrà abilitarlo sempre per utenti dipartimento si dovrà usare la Session("ElencoSettoriDB") e il campo NuovoInserimento
            Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)

            'se la macroarea è chekkata allora ci sono le textbox delle esperienze ed ha senso vedere se evidenziare eventuali modifiche
            If chkMacroArea.Checked And Not Session("TipoUtente") = "E" And Not cmdInserisci Is Nothing And cmdInserisci.Visible Then
                _row = dtSettori.Select("IDSETTORE=" + HFIdMacroAttivita.Value)
                'individuo le esperienze in maschera ed i bottoni della cronologia da rendere visibili nel caso serva
                Dim txtDesEsperienza3 As TextBox = gRow.Cells(2).FindControl("txtDesEsperienza3")
                Dim txtDesEsperienza2 As TextBox = gRow.Cells(2).FindControl("txtDesEsperienza2")
                Dim txtDesEsperienza1 As TextBox = gRow.Cells(2).FindControl("txtDesEsperienza1")
                Dim bntVariazioni3 As Button = gRow.Cells(2).FindControl("btnEsp3")
                Dim bntVariazioni2 As Button = gRow.Cells(2).FindControl("btnEsp2")
                Dim bntVariazioni1 As Button = gRow.Cells(2).FindControl("btnEsp1")

                'controllo esperienze eliminate/modificate
                ModificaStyleEsperienze(_row, "I_ESPERIENZA", txtDesEsperienza3, bntVariazioni3)
                ModificaStyleEsperienze(_row, "II_ESPERIENZA", txtDesEsperienza2, bntVariazioni2)
                ModificaStyleEsperienze(_row, "III_ESPERIENZA", txtDesEsperienza1, bntVariazioni1)
            End If
        Next
    End Sub

    Sub ModificaStyleEsperienze(_row As DataRow(), nomeCampoEsperienza As String, textboxEsperienza As TextBox, btnCronologia As Button)
        If _row.Length > 0 Then
            'si da per scontato che ci sia solo la prima riga
            If IsDBNull(_row(0)(nomeCampoEsperienza)) Then
                If String.IsNullOrEmpty(textboxEsperienza.Text) Then
                    helper.RipristinaStyleDatiModificati(textboxEsperienza)
                    btnCronologia.Visible = False
                Else
                    helper.ModificaStyleDatiModificati(textboxEsperienza)   'dati non esistenti come dati reali ma esistenti in maschera
                    btnCronologia.Visible = True And Not Session("TipoUtente") = "E"
                End If
            Else
                If String.IsNullOrEmpty(textboxEsperienza.Text) OrElse Not String.Equals(textboxEsperienza.Text, _row(0)(nomeCampoEsperienza).ToString, StringComparison.InvariantCultureIgnoreCase) Then
                    helper.ModificaStyleDatiModificati(textboxEsperienza)   'dati diversi tra maschera e dati reali (comprende il caso di dati cancellati in maschera)
                    btnCronologia.Visible = True And Not Session("TipoUtente") = "E"
                Else
                    helper.RipristinaStyleDatiModificati(textboxEsperienza)
                    btnCronologia.Visible = False
                End If
            End If
        Else
            If String.IsNullOrEmpty(textboxEsperienza.Text) Then
                helper.RipristinaStyleDatiModificati(textboxEsperienza)
                btnCronologia.Visible = False
            Else
                helper.ModificaStyleDatiModificati(textboxEsperienza)       'dati non esistenti come dati reali ma esistenti in maschera
                btnCronologia.Visible = True And Not Session("TipoUtente") = "E"
            End If
        End If
    End Sub

    Sub ModificaStyleEntiSettoriOLD(ByVal idEnte As Int32)
        ''aggiunto il 20/08/2014 da Simona Cordella
        'Dim dtsSettori As DataSet
        ''variabile stringa locale per costruire la query per le aree
        'Dim strSql As String
        'Dim result As New List(Of String)
        'strSql = "SELECT macroambitiattività.IDMacroAmbitoAttività  FROM macroambitiattività INNER JOIN EntiSettori ON macroambitiattività.IDMacroAmbitoAttività = EntiSettori.IdMacroAmbitoAttività WHERE  EntiSettori.IdEnte = " & idEnte

        'dtsSettori = ClsServer.DataSetGenerico(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))


        ''controllo se ci sono dei record
        'If dtsSettori.Tables(0).Rows.Count > 0 Then
        '    result = ConvertiDataTableInListaDiStringhe(dtsSettori)
        '    Dim check As CheckBox
        '    Dim idMacroAmbito As String
        '    Dim idMacroAmbitoVariazione As String
        '    For i As Integer = 0 To dtgSettori.Rows.Count - 1
        '        check = dtgSettori.Rows.Item(i).FindControl("chkSeleziona")
        '        idMacroAmbito = dtgSettori.Rows.Item(i).Cells(1).Text
        '        idMacroAmbitoVariazione = (From item In result Where item.ToString() = idMacroAmbito.ToString).FirstOrDefault
        '        If (((idMacroAmbito = idMacroAmbitoVariazione And check.Checked = False)) Or
        '            ((idMacroAmbitoVariazione Is Nothing Or idMacroAmbitoVariazione = String.Empty) And check.Checked = True)) Then
        '            helper.ModificaStyleDatiModificati(dtgSettori)
        '            Exit For
        '        Else
        '            helper.RipristinaStyleDatiModificati(dtgSettori)
        '        End If
        '    Next
        'End If
        Try
            Dim SqlCmd As SqlClient.SqlCommand
            '            Dim Dt As New DataTable
            '           Dim Da As New SqlClient.SqlDataAdapter(SqlCmd)

            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_VERIFICA_VARIAZIONE_SETTORI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = idEnte

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@VARIAZIONE"
            sparam1.SqlDbType = SqlDbType.TinyInt
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@messaggio"
            sparam6.Size = 1000
            sparam6.SqlDbType = SqlDbType.VarChar
            sparam6.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam6)

            SqlCmd.ExecuteNonQuery()

            If SqlCmd.Parameters("@VARIAZIONE").Value = 1 Then
                helper.ModificaStyleDatiModificati(dtgSettori)
            Else
                helper.RipristinaStyleDatiModificati(dtgSettori)
            End If

        Catch ex As Exception
            msgErrore.Visible = True
            msgErrore.Text = ex.Message
        Finally
            'ConnX.Close()
            'SqlCmd = Nothing
        End Try
    End Sub

    Private Function ConvertiDataTableInListaDiStringhe(info As DataSet) As List(Of String)
        Dim result As New List(Of String)
        For Each table As DataTable In info.Tables
            For Each row As DataRow In table.Rows
                For Each column As DataColumn In table.Columns
                    result.Add(row.Item(column))
                Next

            Next
        Next
        Return result
    End Function

    'Protected Sub imgApplicaRiserva_Click(sender As Object, e As EventArgs) Handles imgApplicaRiserva.Click

    '    Dim myQuerySql As String
    '    Dim CmdRiserva As SqlClient.SqlCommand
    '    Try
    '        myQuerySql = "Update Enti Set Riserva = 1, UsernameRiserva='" & Session("Utente") & "', DataRiserva= getdate() where IdEnte= " & Request.QueryString("id") & ""
    '        CmdRiserva = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
    '        CmdRiserva.ExecuteNonQuery()
    '        msgConferma.Visible = True
    '        msgConferma.Text = "Modifica effettuata con successo."
    '        imgApplicaRiserva.Visible = False
    '        imgTogliRiserva.Visible = True
    '    Catch ex As Exception
    '        msgErrore.Visible = True
    '        msgErrore.Text = "Contattare l'assistenza."
    '    End Try
    'End Sub

    'Private Sub imgTogliRiserva_Click(sender As Object, e As System.EventArgs) Handles imgTogliRiserva.Click
    '    Dim myQuerySql As String
    '    Dim CmdRiserva As SqlClient.SqlCommand
    '    Try
    '        myQuerySql = "Update Enti Set Riserva = 0, UsernameNoRiserva='" & Session("Utente") & "', DataNoRiserva= getdate() where IdEnte= " & Request.QueryString("id") & ""
    '        CmdRiserva = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
    '        CmdRiserva.ExecuteNonQuery()
    '        msgConferma.Visible = True
    '        msgConferma.Text = "Modifica effettuata con successo."
    '        imgTogliRiserva.Visible = False
    '        imgApplicaRiserva.Visible = True
    '    Catch ex As Exception
    '        msgErrore.Visible = True
    '        msgErrore.Text = "Contattare l'assistenza."
    '    End Try
    'End Sub

    Protected Sub CmdAnnullaRiservaSiNo_Click(sender As Object, e As EventArgs) Handles CmdAnnullaRiservaSiNo.Click
        DivAccreditaRiserva.Visible = False
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
            If VerificaCheck1() = False Then
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
                        mySql = "Insert into  EntiCausaliRiserva (IdEnte ,IdCausale, UsernameInserimento, DataInserimento)  Values (" & Request.QueryString("id") & ", '" & IDCausale & "', '" & Session("Utente") & "',  getdate() )"
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
                myQuerySql = "Update Enti Set Riserva = 1, UsernameRiserva='" & Session("Utente") & "', DataRiserva= getdate() where IdEnte= " & Request.QueryString("id") & ""
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
                        mySql = "Insert into  EntiCausaliRiserva (IdEnte ,IdCausale, UsernameInserimento, DataInserimento)  Values (" & Request.QueryString("id") & ", '" & IDCausale & "', '" & Session("Utente") & "',  getdate() )"
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


                        mySql = "Delete from EntiCausaliRiserva where IdEnte =" & Request.QueryString("id") & " and IdEnteCausaleRiserva = " & IdEnteCausaleRiserva & ""
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
                    myQuerySql = "Update Enti Set Riserva = 0, UsernameNoRiserva='" & Session("Utente") & "', DataNoRiserva= getdate() where IdEnte= " & Request.QueryString("id") & ""
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
                strSql = "select Riserva FROM enti where IdEnte = " & Request.QueryString("id") & " "
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
                    myQuerySql = "Update Enti Set Riserva = 1, UsernameRiserva='" & Session("Utente") & "', DataRiserva= getdate() where IdEnte= " & Request.QueryString("id") & ""
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
        strSql = "select Causali.IDCausale, Descrizione, IdEnteCausaleRiserva FROM Causali left join EntiCausaliRiserva a on Causali.IDCausale = a.IDCausale where tipo=11 and IdEnte = " & Request.QueryString("id") & " order by Causali.IDCausale "
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
    Sub CaricaCausaliRiserva()

        Dim dtsCausaliRiserva As DataSet
        Dim strSql As String

        strSql = "select Causali.IDCausale, Descrizione, IdEnteCausaleRiserva FROM Causali left join EntiCausaliRiserva a on Causali.IDCausale = a.IDCausale and IdEnte = " & Request.QueryString("id") & " where tipo=11 order by Causali.IDCausale"

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
        strSql = "select Causali.IDCausale, Descrizione, ISNULL(IdEnteCausaleRiserva,0) AS IdEnteCausaleRiserva FROM Causali left join EntiCausaliRiserva a on Causali.IDCausale = a.IDCausale and IdEnte = " & Request.QueryString("id") & " where tipo=11  order by Causali.IDCausale "
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

    Private Function VerificaCheck1() As Boolean
        'controllo è  stata checcato almeno una causale per il salvataggio
        VerificaCheck1 = False
        Dim item As DataGridItem
        For Each item In dtgCausaliRiserva.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkSeleziona"), CheckBox)
            If check.Checked = True Then
                VerificaCheck1 = True
            End If
        Next
        Return VerificaCheck1
    End Function

    Protected Sub btnRiserva_Click(sender As Object, e As EventArgs) Handles btnRiserva.Click
        DivAccreditaRiserva.Visible = True
        BtnAssegna.Visible = True
        CaricaCausaliRiserva()
        CaricaRiservaSel()
    End Sub
    Protected Sub OpenWindow()
        Dim codfis As String = txtCodFis.Text
        Dim url As String = "WfrmNavigaEnte.aspx?CodiceFiscale=" & codfis
        Dim s As String = "window.open('" & url + "', 'popup_window', 'width=1024,height=500,left=100,top=100,scrollbars=yes,resizable=yes');"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

    End Sub

    Protected Sub CmdInforScu1_Click(sender As Object, e As EventArgs) Handles CmdInforScu1.Click
        OpenWindow()
    End Sub

    Protected Sub cmdConfermaChiusura_Click(sender As Object, e As EventArgs) Handles cmdConfermaChiusura.Click
        AccreditamentoEliminaEnteFiglio(lblIdEnte.Value)
        LoadMaschera()
        msgConferma.Visible = True
        cmdElimina.Visible = False
        divChiusuraEnte.Visible = False
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divChiusuraEnte.Visible = False
        cmdElimina.Visible = True
    End Sub

    Sub CaricaDataAccredamento_Adeguamento()

        Dim dtrgenerico As SqlClient.SqlDataReader

        Dim strSql As String
        Dim item As DataGridItem
        strSql = "SELECT a.DataAccreditamento, c.tipofase, c.DataValutazione as DataValutazioneFase FROM enti a INNER JOIN EntiFasi_Enti b  ON a.idente=b.idente  and b.azione='Nuovo Ente'  inner join Entifasi c on b.identefase=c.identefase WHERE a.IDEnte = " & Request.QueryString("id")
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("Conn"))

        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            If Not IsDBNull(dtrgenerico("dataaccreditamento")) Then
                'codice generato
                If dtrgenerico("tipofase") = 1 Then
                    lblDataAccreditamento.Text = Mid(IIf(Not IsDBNull(dtrgenerico("DataAccreditamento")), dtrgenerico("DataAccreditamento"), ""), 1, 10)
                    lblFaseTesto.Text = "Accreditamento"
                    lblDataFase.Text = Mid(IIf(Not IsDBNull(dtrgenerico("DataValutazioneFase")), dtrgenerico("DataValutazioneFase"), ""), 1, 10)
                End If
                If dtrgenerico("tipofase") = 2 Then
                    lblDataAccreditamento.Text = Mid(IIf(Not IsDBNull(dtrgenerico("DataAccreditamento")), dtrgenerico("DataAccreditamento"), ""), 1, 10)
                    lblFaseTesto.Text = "Adeguamento"
                    lblDataFase.Text = Mid(IIf(Not IsDBNull(dtrgenerico("DataValutazioneFase")), dtrgenerico("DataValutazioneFase"), ""), 1, 10)
                End If
            Else
                DIVdataaccr.Visible = False
            End If
        Else
            DIVdataaccr.Visible = False
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub

    Protected Sub dtgSettori_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim url As String
        Dim istruzione As String
        Dim index As Integer


        Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)

        index = row.RowIndex
        Dim IDSettore = CStr(dtgSettori.DataKeys(index).Values(0))

        If e.CommandName = "ApriModifica" Then
            hfRigaSelezionata.Value = index
            'url = "WfrmDettaglioSettore.aspx?Settore=" & IDSettore
            'istruzione = "window.open('" & url + "', 'popup_window', 'width=1024,height=500,left=100,top=100,scrollbars=yes,resizable=yes');"
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "script", istruzione, True)
            mpe_EsperienzaAree.Show()
            CaricaAreeIntervento(IDSettore)
            CaricaAnniEsperienza()
            lblMsgAreeInervento.Text = String.Empty
        End If

        If e.CommandName = "MostraVariazioni1" Then
            MostraVariazioni(IDSettore, row.Cells(2).FindControl("txtDesEsperienza1"), "1", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza1"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza1").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If
        If e.CommandName = "MostraVariazioni2" Then
            MostraVariazioni(IDSettore, row.Cells(2).FindControl("txtDesEsperienza2"), "2", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza2"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza2").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If
        If e.CommandName = "MostraVariazioni3" Then
            MostraVariazioni(IDSettore, row.Cells(2).FindControl("txtDesEsperienza3"), "3", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza3"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza3").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If

        If e.CommandName = "MostraCronologia1" Then
            MostraCronologia(IDSettore, "1", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza1"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza1").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If
        If e.CommandName = "MostraCronologia2" Then
            MostraCronologia(IDSettore, "2", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza2"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza2").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If
        If e.CommandName = "MostraCronologia3" Then
            MostraCronologia(IDSettore, "3", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza3"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza3").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If

        If e.CommandName = "AbModArt2Art10" Then
            AbilitaModArt2Art10(IDSettore, True)
            AttivaPulsantiModArt2Art10()
        End If
        If e.CommandName = "DisabModArt2Art10" Then
            AbilitaModArt2Art10(IDSettore, False)
            AttivaPulsantiModArt2Art10()
        End If
    End Sub

    Sub AbilitaModArt2Art10(IdMacroAmbitoAttività As Integer, abilita As Boolean)
        Dim SqlCmd As New SqlClient.SqlCommand()
        Try
            SqlCmd.Connection = Session("Conn")
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ABILITA_MOD_ENTE_ART2_ART10"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Parameters.Clear()
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IIf(Request.QueryString("id") = Nothing, 0, Integer.Parse(Request.QueryString("id")))
            SqlCmd.Parameters.Add("@IdMacroAmbitoAttività", SqlDbType.Int).Value = IdMacroAmbitoAttività
            SqlCmd.Parameters.Add("@abilita", SqlDbType.Bit).Value = abilita
            SqlCmd.Parameters.Add("@Utente", SqlDbType.NVarChar, 10).Value = Session("Utente")
            SqlCmd.ExecuteNonQuery()
        Catch ex As Exception
            msgErrore.ForeColor = Color.Red
            msgErrore.Text = "Errore nell'aggiornamento"
            Exit Sub
        End Try
    End Sub

    Sub MostraCronologia(idSettore As String, indiceEsperienza As String, Anno As String, DescrSettore As String)
        'Carica da db i dati cronologici reali dell'esperienza e li mette in sessione
        'poi apre la maschera della cronologia
        Dim _dt As New DataTable
        Dim _strSQL As String = ""



        _strSQL = "Select convert(char(10), DataRiferimento, 103) as [Data Presentazione], TipoEvento as [Tipo Evento], "

        Select Case indiceEsperienza
            Case 1
                _strSQL += "III_Esperienza as Esperienza"
            Case 2
                _strSQL += "II_Esperienza as Esperienza"
            Case Else
                _strSQL += "I_Esperienza as Esperienza"
        End Select

        _strSQL += ", IdStorico as Id from Accreditamento_VariazioneEnteEsperienzaSettore_CONSISTENTI where IdEnte=" + IIf(Request.QueryString("id") = Nothing, "0", Request.QueryString("id")) + " and idsettore=" + idSettore
        _strSQL += " ORDER BY IdStorico Desc"

        _dt = ClsServer.DataTableGenerico(_strSQL, Session("Conn"))

        If _dt Is Nothing OrElse _dt.Rows.Count = 0 Then    'se non c'è niente esco, poi si farà + preciso e renderà invisibile il pulsante
            Exit Sub
        End If

        Session("TitoloXCronologia") = "Cronologia variazioni esperienza anno " & Anno & " settore " & DescrSettore
        Session("Cronologia") = _dt

        'stampo a pagina uno script che mi apre la popup della cronologia
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""Cronologia.aspx"", """", ""height=600,width=1250,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes"")" & vbCrLf)
        Response.Write("</script>")

    End Sub

    Sub MostraVariazioni(idSettore As String, txtEsperienza As TextBox, indiceEsperienza As String, Anno As String, DescrSettore As String)
        'Carica da db il dato reale dell'esperienza e lo mette in sessione insieme al dato in variazione (quello in maschera)
        'poi apre la maschera del compare
        Dim _dt As DataTable = Session("ElencoSettoriPrecedentiAccordo")

        Dim TestoPrecedente = ""
        Dim TestoAttuale = txtEsperienza.Text
        Dim _nomeCampo As String

        Select Case indiceEsperienza
            Case 1
                _nomeCampo = "III_Esperienza"
            Case 2
                _nomeCampo = "II_Esperienza"
            Case Else
                _nomeCampo = "I_Esperienza"
        End Select

        'Select Case indiceEsperienza
        '    Case 1
        '        _strSQL = "Select III_Esperienza as TestoPrecedente"
        '    Case 2
        '        _strSQL = "Select II_Esperienza as TestoPrecedente"
        '    Case Else
        '        _strSQL = "Select I_Esperienza as TestoPrecedente"
        'End Select

        '_strSQL += " FROM EnteEsperienzaSettore where idente=" + Session("IdEnte") + " and idsettore=" + idSettore + " and DataFineValidita is null"

        '_dt = ClsServer.DataTableGenerico(_strSQL, Session("Conn"))


        If Not _dt Is Nothing Then
            Dim _row As DataRow() = _dt.Select("IDSETTORE=" + idSettore)
            If _row.Length > 0 Then
                If Not IsDBNull(_row(0)(_nomeCampo)) Then
                    TestoPrecedente = _row(0)(_nomeCampo)
                End If
            End If
        End If

        Session("TestoPrecedenteXCompare") = TestoPrecedente
        Session("TestoAttualeXCompare") = TestoAttuale
        Session("TitoloXCompare") = "Variazione esperienza anno " & Anno & " settore " & DescrSettore

        'stampo a pagina uno script che mi apre la popup delle variazioni
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""compare.aspx"", """", ""height=500,width=900,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes"")" & vbCrLf)
        Response.Write("</script>")
    End Sub

    Private Sub CaricaAreeIntervento(IDSettore As String)
        Dim strSQl = ""
        Dim dtAmbiti As DataTable
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)

        strSQl = ""
        strSQl += "SELECT A.CODIFICA, (M.CODIFICA + A.CODIFICA  + ' - ' + A.AreaIntervento) AS AREAINTERVENTO,"
        strSQl += "A.IDSETTORE AS IDSETTORE, M.MACROAMBITOATTIVITÀ AS SETTORE, A.id AS IDAREA "
        strSQl += "FROM AREEESPERIENZA A "
        strSQl += "INNER JOIN MACROAMBITIATTIVITÀ M "
        strSQl += "ON A.IdSettore = M.IDMacroAmbitoAttività "
        strSQl += "WHERE A.IDSETTORE= " & IDSettore

        dtAmbiti = ClsServer.DataTableGenerico(strSQl, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        grdAmbiti.DataSource = dtAmbiti
        grdAmbiti.DataBind()

        If IDSettore <> "6" Then 'Estero 
            lblTitoloEsperienzaArea.Text = "Dettaglio Settore - " & dtAmbiti.Rows(0)("SETTORE").ToString()
        Else
            lblTitoloEsperienzaArea.Text = "Dettaglio Settore - Estero "
        End If


        'Recupero l'esperienze per quel settore specifico 
        If Session("EsperienzeAreeSettoreAccordo") IsNot Nothing Then
            EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettoreAccordo"), List(Of clsEsperienzeAree))
            Dim EsperienzaSettore = EsperienzeAreeSettore.Find((Function(esperienza As clsEsperienzeAree) esperienza.IdSettore = IDSettore))
            If EsperienzaSettore IsNot Nothing Then
                Dim Conta = 1
                For Each anno As Integer In EsperienzaSettore.AnnoEsperienza
                    Select Case Conta
                        Case 1
                            txtAnno3.Text = anno
                        Case 2
                            txtAnno2.Text = anno
                        Case 3
                            txtAnno1.Text = anno
                    End Select
                    Conta = Conta + 1
                Next
                Conta = 1
                For Each esperienza As String In EsperienzaSettore.DescrizioneEsperienza
                    Select Case Conta
                        Case 1
                            txtEsperienza3.Text = esperienza
                        Case 2
                            txtEsperienza2.Text = esperienza
                        Case 3
                            txtEsperienza1.Text = esperienza
                    End Select
                    Conta = Conta + 1
                Next

                For Each gr As GridViewRow In grdAmbiti.Rows
                    If gr.RowType = DataControlRowType.DataRow Then
                        For Each areaSelezionata As String In EsperienzaSettore.AreeSelezionate
                            Dim HFIDAREA = DirectCast(grdAmbiti.Rows(gr.RowIndex).Cells(4).FindControl("HFIDAREA"), HiddenField)
                            If (areaSelezionata = HFIDAREA.Value) Then
                                Dim chk As CheckBox = grdAmbiti.Rows(gr.RowIndex).Cells(2).FindControl("chkSeleziona")
                                chk.Checked = True
                            End If
                        Next
                    End If
                Next
            Else
                txtEsperienza3.Text = String.Empty
                txtEsperienza2.Text = String.Empty
                txtEsperienza1.Text = String.Empty
                txtAnno1.Text = String.Empty
                txtAnno2.Text = String.Empty
                txtAnno3.Text = String.Empty
            End If
        End If

    End Sub

    Private Function CaricaAnniEsperienza()

        For i = 1 To ANNIPRECEDENTI
            Select Case i
                Case 1
                    If Not IsNumeric(txtAnno3.Text) Then txtAnno3.Text = CStr(Year(Now.Date) - i)
                    txtAnno3.Enabled = False
                    AnniCaricamento(i - 1) = txtAnno3.Text
                Case 2
                    If Not IsNumeric(txtAnno2.Text) Then txtAnno2.Text = CStr(Year(Now.Date) - i)
                    txtAnno2.Enabled = False
                    AnniCaricamento(i - 1) = txtAnno2.Text
                Case 3
                    If Not IsNumeric(txtAnno1.Text) Then txtAnno1.Text = CStr(Year(Now.Date) - i)
                    txtAnno1.Enabled = False
                    AnniCaricamento(i - 1) = txtAnno1.Text
            End Select
        Next

    End Function

    Protected Sub ChkChange(sender As Object, e As EventArgs)
        Dim gr As GridViewRow
        Dim chkBox As CheckBox
        Dim url As String
        Dim istruzione As String
        'Dim EsperienzeSettore As New clsAmbitiEsperienza
        Dim contaRiga As Integer = 3
        Dim divAreaIntervento As HtmlGenericControl
        Dim IDSettore As String
        Dim divAree As HtmlGenericControl
        Dim lblAnno3 As Label
        Dim lblAnno2 As Label
        Dim lblAnno1 As Label
        Dim txtEsperienza3 As TextBox
        Dim txtEsperienza2 As TextBox
        Dim txtEsperienza1 As TextBox
        chkBox = CType(sender, CheckBox)
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)


        gr = chkBox.Parent.Parent
        IDSettore = CStr(dtgSettori.DataKeys(gr.RowIndex).Values(0))
        divAreaIntervento = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("divAreaIntervento") 'recupero la div annidata nella griglia

        lblAnno3 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("lblAnnoEsperienza3")
        lblAnno2 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("lblAnnoEsperienza2")
        lblAnno1 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("lblAnnoEsperienza1")
        txtEsperienza3 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("txtDesEsperienza3")
        txtEsperienza2 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("txtDesEsperienza2")
        txtEsperienza1 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("txtDesEsperienza1")
        divAree = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("divArea")
        Dim cmdInserisci As Button = DirectCast(gr.FindControl("cmdModifica"), Button)
        If chkBox.Checked Then
            divAreaIntervento.Style("display") = "block"
            divAree.Style("display") = "block"
            cmdInserisci.Visible = True
            CaricaAreeIntervento(IDSettore)
            CaricaAnniEsperienza()
        Else
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "confirm", "<script type=text/javascript>if (!confirm('Attenzione! Verranno cancellate tutte le esperienze del settore scelto.\r\n Si vuole procedere?')){false;}{</script>")
            cmdInserisci.Visible = False
            cmdInserisci.Text = "Inserisci"
            If Session("EsperienzeAreeSettoreAccordo") IsNot Nothing Then
                EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettoreAccordo"), List(Of clsEsperienzeAree))

                Dim EsperienzaSettore = EsperienzeAreeSettore.Find((Function(esperienza As clsEsperienzeAree) esperienza.IdSettore = IDSettore))

                'Se è stata modificata un esperienza la rimuovo per poi ricaricarla aggiornata
                If EsperienzaSettore IsNot Nothing Then
                    EsperienzeAreeSettore.Remove(EsperienzaSettore)
                    Session("EsperienzeAreeSettoreAccordo") = EsperienzeAreeSettore
                End If

            End If

            divAree.InnerHtml = ""
            divAree.Style("display") = "none"
            divAreaIntervento.Style("display") = "none"
            txtEsperienza1.Text = String.Empty
            txtEsperienza2.Text = String.Empty
            txtEsperienza3.Text = String.Empty
            lblAnno3.Text = "Non impostato"
            lblAnno2.Text = "Non impostato"
            lblAnno1.Text = "Non impostato"
        End If
    End Sub

    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim dtTipologiaEnte As DataTable
        Dim tipologiaEnte As String
        Dim naturaGiuridicaEnte As String
        Dim idCategoriaEntePubblico As Byte

        lblMsgAreeInervento.Text = String.Empty

        If ddlTipologia.SelectedItem IsNot Nothing Then
            tipologiaEnte = ddlTipologia.SelectedItem.Text
        End If

        If ddlGiuridica.SelectedItem IsNot Nothing Then
            naturaGiuridicaEnte = ddlGiuridica.SelectedItem.Text
        End If

        If tipologiaEnte = String.Empty Then
            lblMsgAreeInervento.Text = "Specificare la tipologia dell'ente prima di compilare il dettaglio di un settore"
            mpe_EsperienzaAree.Show()
            Exit Sub
        End If

        If naturaGiuridicaEnte = String.Empty Then
            lblMsgAreeInervento.Text = "Selezionare la denominazione della tipologia"
            mpe_EsperienzaAree.Show()
            Exit Sub
        End If


        If VerificaAree() Then
            AggiungiEsperienzaAmbiti()
        Else
            mpe_EsperienzaAree.Show()
        End If

    End Sub

    Private Function VerificaAree() As Boolean
        Dim Esperienza(ANNIPRECEDENTI - 1) As String
        Dim contaAmbitiSelezionati As Integer = 0


        For i = 0 To ANNIPRECEDENTI - 1
            Select Case i
                Case 0
                    Esperienza(0) = txtEsperienza3.Text.Trim
                Case 1
                    Esperienza(1) = txtEsperienza2.Text.Trim
                Case 2
                    Esperienza(2) = txtEsperienza1.Text.Trim
            End Select
        Next

        CaricaAnniEsperienza()

        For i = 0 To Esperienza.Length - 1
            If Esperienza(i) = String.Empty Then
                lblMsgAreeInervento.Text = "Esperienza anno " & AnniCaricamento(i) & " mancante"
                VerificaAree = False
                Exit Function
            Else
                If Esperienza(i).Length < 500 Or Esperienza(i).Length > 1500 Then
                    lblMsgAreeInervento.Text = "L' esperienza per l'anno " & AnniCaricamento(i) & " deve essere composta da un minimo di 500 ad un massimo di 1500 caratteri. Attualmente ne sono stati inseriti " & Esperienza(i).Length
                    VerificaAree = False
                    Exit Function
                End If
            End If
        Next


        For Each gr As GridViewRow In grdAmbiti.Rows
            If gr.RowType = DataControlRowType.DataRow Then
                Dim chk As CheckBox = gr.Cells(2).FindControl("chkSeleziona")
                If chk IsNot Nothing Then
                    If chk.Checked Then
                        contaAmbitiSelezionati = contaAmbitiSelezionati + 1
                    End If
                End If
            End If
        Next

        If contaAmbitiSelezionati = 0 Then
            lblMsgAreeInervento.Text = "È obbligatorio selezionare almeno un'area d'intervento "
            VerificaAree = False
            Exit Function
        End If

        VerificaAree = True

    End Function

    Private Sub AggiungiEsperienzaAmbiti()

        'Dim esperienzaAmbiti As New clsAmbitiEsperienza()
        Dim istruzione As String
        Dim rigaSelezionata As GridViewRow
        Dim rigsSelezionataAmbiti As GridViewRow
        Dim ContaRiga As Integer = 1
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim IDSettore As Integer
        Dim AreeSelezionate As New List(Of String)

        'rigaSelezionata.indiceRigaSelezionata = hfRigaSelezionata.Value

        If Session("EsperienzeAreeSettoreAccordo") IsNot Nothing Then
            EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettoreAccordo"), List(Of clsEsperienzeAree))
        Else
            EsperienzeAreeSettore.Clear()
        End If

        If Not hfRigaSelezionata.Value = String.Empty Then
            rigaSelezionata = dtgSettori.Rows(hfRigaSelezionata.Value)
        End If

        If Not hfRigaAmbiti.Value = String.Empty Then
            rigsSelezionataAmbiti = grdAmbiti.Rows(hfRigaAmbiti.Value)
        End If

        Dim EsperienzaArea As New clsEsperienzeAree()

        Dim lblAnnoEsperienza3 As Label = rigaSelezionata.Cells(2).FindControl("lblAnnoEsperienza3")
        Dim lblAnnoEsperienza2 As Label = rigaSelezionata.Cells(2).FindControl("lblAnnoEsperienza2")
        Dim lblAnnoEsperienza1 As Label = rigaSelezionata.Cells(2).FindControl("lblAnnoEsperienza1")

        Dim txtDesEsperienza3 As TextBox = rigaSelezionata.Cells(2).FindControl("txtDesEsperienza3")
        Dim txtDesEsperienza2 As TextBox = rigaSelezionata.Cells(2).FindControl("txtDesEsperienza2")
        Dim txtDesEsperienza1 As TextBox = rigaSelezionata.Cells(2).FindControl("txtDesEsperienza1")
        Dim Area As HtmlGenericControl = rigaSelezionata.Cells(2).FindControl("divArea")

        lblAnnoEsperienza3.Text = Trim(txtAnno3.Text)
        lblAnnoEsperienza2.Text = Trim(txtAnno2.Text)
        lblAnnoEsperienza1.Text = Trim(txtAnno1.Text)
        txtDesEsperienza3.Text = Trim(txtEsperienza3.Text)
        txtDesEsperienza2.Text = Trim(txtEsperienza2.Text)
        txtDesEsperienza1.Text = Trim(txtEsperienza1.Text)


        Area.InnerHtml = "<fieldset><legend>Aree d'intervento selezionate</legend>"
        Area.InnerHtml += "<table>"
        For Each gr As GridViewRow In grdAmbiti.Rows
            If gr.RowType = DataControlRowType.DataRow Then
                Dim chk As CheckBox = gr.Cells(2).FindControl("chkSeleziona")
                If chk IsNot Nothing Then
                    If chk.Checked Then
                        Dim HFIDAREA = DirectCast(grdAmbiti.Rows(gr.RowIndex).Cells(4).FindControl("HFIDAREA"), HiddenField)
                        Dim HFIDSettore = DirectCast(grdAmbiti.Rows(gr.RowIndex).Cells(3).FindControl("HFIDSettore"), HiddenField)
                        Area.InnerHtml += "<tr><td><label runat=""server"" ID =""lblValoreAreaIntervento_" & ContaRiga & """>" & grdAmbiti.Rows(gr.RowIndex).Cells(1).Text & "<label></td></tr>"
                        AreeSelezionate.Add(HFIDAREA.Value)
                        IDSettore = CInt(HFIDSettore.Value)
                        ContaRiga = ContaRiga + 1
                    End If
                End If
            End If
        Next

        Area.InnerHtml += "</table></fieldset>"

        'Carico l'entità EsperienzaArea inizio

        Dim EsperienzaSettore = EsperienzeAreeSettore.Find((Function(esperienza As clsEsperienzeAree) esperienza.IdSettore = IDSettore))

        'Se è stata modificata un esperienza la rimuovo per poi ricaricarla aggiornata
        If EsperienzaSettore IsNot Nothing Then
            EsperienzeAreeSettore.Remove(EsperienzaSettore)
        End If

        Dim AnniEsperienza As New List(Of Integer)
        Dim DescrizioneEsperienza As New List(Of String)

        EsperienzaArea.IdSettore = IDSettore
        AnniEsperienza.Insert(0, CInt(lblAnnoEsperienza3.Text))
        AnniEsperienza.Insert(1, CInt(lblAnnoEsperienza2.Text))
        AnniEsperienza.Insert(2, CInt(lblAnnoEsperienza1.Text))
        EsperienzaArea.AnnoEsperienza = AnniEsperienza

        DescrizioneEsperienza.Insert(0, txtDesEsperienza3.Text)
        DescrizioneEsperienza.Insert(1, txtDesEsperienza2.Text)
        DescrizioneEsperienza.Insert(2, txtDesEsperienza1.Text)

        EsperienzaArea.DescrizioneEsperienza = DescrizioneEsperienza
        EsperienzaArea.AreeSelezionate = AreeSelezionate
        EsperienzaArea.AreeCompetenza = Area.InnerHtml

        EsperienzeAreeSettore.Add(EsperienzaArea)

        'Carico l'entità EsperienzaArea fine

        'Metto tutto in sessione
        Session("EsperienzeAreeSettoreAccordo") = EsperienzeAreeSettore

        Dim cmdInserisci As Button = DirectCast(rigaSelezionata.FindControl("cmdModifica"), Button)
        If Not cmdInserisci Is Nothing Then
            cmdInserisci.Text = "Modifica"
        End If
        mpe_EsperienzaAree.Hide()

    End Sub


    Protected Sub ddlrelazione_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlrelazione.SelectedIndexChanged
        Dim obbligatorio As String = "(*)"
        If ddlrelazione.SelectedItem IsNot Nothing Then
            If ddlrelazione.SelectedItem.Text = "Contratto" Then
                If lblDataStipula.Text.IndexOf(obbligatorio) = -1 Then
                    lblDataStipula.Text = obbligatorio & lblDataStipula.Text
                End If
            Else
                If lblDataStipula.Text.IndexOf("(*)") <> -1 Then
                    lblDataStipula.Text = Mid(lblDataStipula.Text, obbligatorio.Length + 1, lblDataStipula.Text.Length)
                End If
            End If

        End If
    End Sub

    Private Function AggiornaSettoriAreeIntervento(IdEnteAccoglienza As Integer) As Boolean

        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim SqlCmd As New SqlClient.SqlCommand()
        Dim esito As Boolean = True
        Dim contaEsperienze As Integer
        LogParametri.Clear()

        If Session("IdStatoEnte") = "6" OrElse (Session("IdStatoEnte") <> "6" AndAlso Session("EsperienzeAreeSettoreAccordo") IsNot Nothing) Then
            If Session("EsperienzeAreeSettoreAccordo") IsNot Nothing Then
                EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettoreAccordo"), List(Of clsEsperienzeAree))
            Else
                EsperienzeAreeSettore.Clear()
                MyTransaction.Rollback()
                AggiornaSettoriAreeIntervento = False
                Exit Function
            End If

            Try

                'PER L'ENTE VENGONO STORICIZZATE LE ARRE D'INTERVENTO DI OGNI SETTORE INIZIO
                SqlCmd.CommandText = "SP_UPD_AREE_ESPERIENZE_SETTORE"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Transaction = MyTransaction
                SqlCmd.Connection = Session("Conn")
                SqlCmd.Parameters.Clear()
                SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = lblIdEnte.Value
                SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar, 10).Value = Session("Utente")
                'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
                SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
                SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

                SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
                SqlCmd.Parameters("@messaggio").Size = 1000
                SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output
                SqlCmd.ExecuteNonQuery()


                If SqlCmd.Parameters("@Esito").Value = 0 Then
                    esito = False
                    MyTransaction.Rollback()
                    Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_MODIFICA_SETTORI_ERRATA, SqlCmd.Parameters("@messaggio").Value)
                    AggiornaSettoriAreeIntervento = False
                    Exit Function
                End If

                'PER L'ENTE VENGONO STORICIZZATE LE ARRE D'INTERVENTO DI OGNI SETTORE FINE


                SqlCmd.CommandText = "SP_INS_AREE_ESPERIENZE_SETTORE"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Transaction = MyTransaction
                SqlCmd.Connection = Session("Conn")


                For Each EsperienzaArea As clsEsperienzeAree In EsperienzeAreeSettore
                    Dim areeEsperienze As String = String.Empty
                    LogParametri.Clear()
                    SqlCmd.Parameters.Clear()

                    SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnteAccoglienza
                    LogParametri.Add("@IdEnte", IdEnteAccoglienza)

                    SqlCmd.Parameters.Add("@IdSettore", SqlDbType.Int).Value = EsperienzaArea.IdSettore
                    LogParametri.Add("@IdSettore", EsperienzaArea.IdSettore)

                    SqlCmd.Parameters.Add("@I_Esperienza", SqlDbType.VarChar, 1500).Value = EsperienzaArea.DescrizioneEsperienza(0)
                    LogParametri.Add("@I_Esperienza", EsperienzaArea.DescrizioneEsperienza(0))

                    SqlCmd.Parameters.Add("@II_Esperienza", SqlDbType.VarChar, 1500).Value = EsperienzaArea.DescrizioneEsperienza(1)
                    LogParametri.Add("@II_Esperienza", EsperienzaArea.DescrizioneEsperienza(1))

                    SqlCmd.Parameters.Add("@III_Esperienza", SqlDbType.VarChar, 1500).Value = EsperienzaArea.DescrizioneEsperienza(2)
                    LogParametri.Add("@III_Esperienza", EsperienzaArea.DescrizioneEsperienza(2))

                    SqlCmd.Parameters.Add("@I_AnnoEsperienza", SqlDbType.Int).Value = EsperienzaArea.AnnoEsperienza(0)
                    LogParametri.Add("@I_AnnoEsperienza", EsperienzaArea.AnnoEsperienza(0))

                    SqlCmd.Parameters.Add("@II_AnnoEsperienza", SqlDbType.Int).Value = EsperienzaArea.AnnoEsperienza(1)
                    LogParametri.Add("@II_AnnoEsperienza", EsperienzaArea.AnnoEsperienza(1))

                    SqlCmd.Parameters.Add("@III_AnnoEsperienza", SqlDbType.Int).Value = EsperienzaArea.AnnoEsperienza(2)
                    LogParametri.Add("@III_AnnoEsperienza", EsperienzaArea.AnnoEsperienza(2))

                    For Each area In EsperienzaArea.AreeSelezionate
                        areeEsperienze += area + ","
                    Next

                    areeEsperienze = Left(areeEsperienze, areeEsperienze.Length - 1)

                    SqlCmd.Parameters.Add("@AreeIntervento", SqlDbType.VarChar, -1).Value = areeEsperienze
                    LogParametri.Add("@AreeIntervento", areeEsperienze)

                    SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar, 10).Value = Session("Utente")

                    'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
                    SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
                    SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

                    SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
                    SqlCmd.Parameters("@messaggio").Size = 1000
                    SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output


                    SqlCmd.ExecuteNonQuery()
                    '@messaggio varchar(1000) output
                    If SqlCmd.Parameters("@Esito").Value = 0 Then
                        esito = False
                        Exit For
                    Else
                        Log.Information(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_INSERIMENTO_CORRETTO, SqlCmd.Parameters("@messaggio").Value, LogParametri)
                    End If
                Next

                If esito = True Then
                    msgConferma.Text = SqlCmd.Parameters("@messaggio").Value
                    AggiornaSettoriAreeIntervento = True
                    Exit Function
                Else
                    MyTransaction.Rollback()
                    Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_MODIFICA_SETTORI_ERRATA, "Modifica Settore per ente di accordo non avvenuta", LogParametri)
                    AggiornaSettoriAreeIntervento = False
                    Exit Function
                End If
            Catch ex As Exception
                MyTransaction.Rollback()
                Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_MODIFICA_SETTORI_ERRATA, ex.Message, ex, LogParametri)
                AggiornaSettoriAreeIntervento = False
                Exit Function
            End Try
        Else
            MessaggiConvalida("OPERAZIONE AVVENUTA CON SUCCESSO")
            AggiornaSettoriAreeIntervento = True
            Exit Function
        End If

    End Function

    Private Sub MessaggiConvalida(ByVal strMessaggio)
        msgConferma.ForeColor = Color.Navy
        msgConferma.Text = strMessaggio
        Exit Sub
    End Sub

    Function ElencoSettori() As DataTable
        'chiamata dalla CaricaEntiSettori, legge da db i settori dalle tabelle reali indicando se sono nuovi settori (inseriti nella eventuale fase di iscrizione ancora aperta)
        'sostituisce la query scolpita a codice presente precedentemente

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataTable As New DataTable
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_ELENCO_SETTORI_ENTE_FIGLIO]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = IIf(Request.QueryString("id") = Nothing, 0, Integer.Parse(Request.QueryString("id")))

            sqlDAP.Fill(dataTable)

        Catch ex As Exception
            msgErrore.ForeColor = Color.Red
            msgErrore.Text = "Errore nel recupero informazioni"
            Exit Function
        End Try

        Return dataTable
    End Function

    Sub CaricaEntiSettori()
        Dim blnAbilitaSettori As Boolean
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim dtSettori As New DataTable
        'variabile stringa locale per costruire la query per le aree
        'Dim strSql As String

        Session("ElencoSettoriDBAccordo") = Nothing
        dtSettori = ElencoSettori()
        Session("ElencoSettoriDBAccordo") = dtSettori

        blnAbilitaSettori = AbilitaSettori()

        'rendo tutti i cmdinserisci invisibili, poi vengono abilitati dopo se serve
        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
            If Not cmdInserisci Is Nothing Then
                cmdInserisci.Visible = False
            End If
        Next

        If dtSettori IsNot Nothing AndAlso dtSettori.Rows.Count > 0 Then 'Esistono settori legate ad esperienze quindi le carico in maschera

            For Each dr As DataRow In dtSettori.Rows
                Dim esperienzaArea As New clsEsperienzeAree
                Dim esperienze(2) As String
                Dim anniEsperienze(2) As Integer
                If IsDBNull(dr("IDSETTORE")) Then
                    For Each gRow As GridViewRow In dtgSettori.Rows
                        Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                        Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                        Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                        If dr("IDMACROATTIVITA").ToString() = HFIdMacroAttivita.Value Then
                            check.Checked = True
                            ChkChange(check, New EventArgs)
                            cmdInserisci.Visible = If(dr("NuovoInserimento") = "0", False, True)
                        End If
                        If blnAbilitaSettori = False Then
                            check.Enabled = blnAbilitaSettori
                        End If
                    Next
                    Continue For
                End If
                For i = 0 To 2 'Anni e Esperienza
                    Select Case i
                        Case 0
                            esperienze(i) = dr("I_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("I_ANNOESPERIENZA").ToString())
                        Case 1
                            esperienze(i) = dr("II_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("II_ANNOESPERIENZA").ToString())
                        Case 2
                            esperienze(i) = dr("III_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("III_ANNOESPERIENZA").ToString())
                    End Select
                Next
                esperienzaArea.IdSettore = CInt(dr("IDMACROATTIVITA").ToString())
                esperienzaArea.AnnoEsperienza = anniEsperienze.ToList()
                esperienzaArea.DescrizioneEsperienza = esperienze.ToList()
                esperienzaArea.AreeSelezionate = dr("AREEINTERVENTO").ToString().Split(",").ToList()

                EsperienzeAreeSettore.Add(esperienzaArea)
                Session("EsperienzeAreeSettoreAccordo") = EsperienzeAreeSettore

                For Each gRow As GridViewRow In dtgSettori.Rows
                    Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                    Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                    Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                    If dr("IDMACROATTIVITA").ToString() = HFIdMacroAttivita.Value Then
                        check.Checked = True
                        ChkChange(check, New EventArgs)
                        cmdInserisci.Visible = If(dr("NuovoInserimento") = "0", False, True)  'possono essere editate SOLO esperienze/aree inserite in una fase aperta
                        hfRigaSelezionata.Value = gRow.RowIndex
                        AggiungiEsperienzaAmbiti()
                    End If

                    If blnAbilitaSettori = False Then
                        check.Enabled = blnAbilitaSettori
                    End If

                Next
            Next
        End If

    End Sub
    Private Function CheckCodFiscale() As Boolean
        'Controllo Codice Fiscale
        Dim RegPattern As String

        '*******Controllo Formale del codice fiscale
        'Controllo che sia formalmente corretto
        RegPattern = "^[A-Za-z]{6}[0-9]{2}[A-Za-z]{1}[0-9]{2}[A-Za-z]{1}[0-9]{3}[A-Za-z]{1}$"
        If Regex.Match(txtCodFiscRL.Text, RegPattern).Success = False Then
            CheckCodFiscale = False
            Exit Function
        Else
            CheckCodFiscale = True
        End If

    End Function

    Private Sub ClearSessionAttoCostitutivo()
        msgConferma.Text = String.Empty
        Session("LoadedACAccordo") = Nothing
        rowNoAttoCostitutivo.Visible = True
        rowAttoCostitutivo.Visible = False
        msgConferma.Text = "Atto Costitutivo dell'Ente eliminato correttamente"
    End Sub

    Private Sub ClearSessionStatuto()
        msgConferma.Text = String.Empty
        Session("LoadedStatutoAccordo") = Nothing
        rowNoStatuto.Visible = True
        rowStatuto.Visible = False
        msgConferma.Text = "Statuto dell'Ente eliminato correttamente"
    End Sub

    Private Sub ClearDeliberaAdesione()
        msgConferma.Text = String.Empty
        Session("LoadedDeliberaAdesioneAccordo") = Nothing
        rowNoDeliberaAdesione.Visible = True
        rowDeliberaAdesione.Visible = False
        msgConferma.Text = "Delibera eliminata correttamente"
    End Sub

    Private Sub ClearSessionImpegnoEtico()
        msgConferma.Text = String.Empty
        Session("LoadedCartaImpegnoEticoAccordo") = Nothing
        rowNoImpegnoEtico.Visible = True
        rowImpegnoEtico.Visible = False
        msgConferma.Text = "Carta Impegno Etico eliminata correttamente"
    End Sub

    Private Sub ClearSessionCartaImpegno()
        msgConferma.Text = String.Empty
        Session("LoadedCartaImpegnoAccordo") = Nothing
        rowNoCartaImpegno.Visible = True
        rowCartaImpegno.Visible = False
        msgConferma.Text = "Carta di Impegno eliminata correttamente"
    End Sub

    Protected Sub cmdAllegaAttoCostitutivo_Click(sender As Object, e As EventArgs) Handles cmdAllegaAttoCostitutivo.Click
        mpeAttoCostitutivo.Show()
    End Sub

    Protected Sub cmdAllegaStatuto_Click(sender As Object, e As EventArgs) Handles cmdAllegaStatuto.Click
        mpeStatuto.Show()
    End Sub

    Protected Sub cmdAllegaDeliberaAdesione_Click(sender As Object, e As EventArgs) Handles cmdAllegaDeliberaAdesione.Click
        mpeDeliberaAdesione.Show()
    End Sub

    Protected Sub cmdAllegaImpegnoEtico_Click(sender As Object, e As EventArgs) Handles cmdAllegaImpegnoEtico.Click
        mpeImpegnoEtico.Show()
    End Sub

    Protected Sub cmdAllegaAC_Click(sender As Object, e As EventArgs) Handles cmdAllegaAC.Click
        lblErroreAttoCostitutivo.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileAttoCostitutivo.PostedFile Is Nothing Or String.IsNullOrEmpty(fileAttoCostitutivo.PostedFile.FileName) Then
            lblErroreAttoCostitutivo.Text = "Non è stato scelto nessun file per il caricamento dell' atto costitutivo dell'ente"
            mpeAttoCostitutivo.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileAttoCostitutivo) = False Then
            lblErroreAttoCostitutivo.Text = "Il formato file  dell' atto costitutivo dell'ente non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeAttoCostitutivo.Show()
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileAttoCostitutivo.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreAttoCostitutivo.Text = "Attenzione. Impossibile caricare l'atto costitutivo dell'ente vuoto."
            mpeAttoCostitutivo.Show()
            Exit Sub
        End If
        If iLen > 20971520 Then
            lblErroreAttoCostitutivo.Text = "Attenzione. La dimensione massima dell atto di nomina del Rappresentante Legale è di 20 MB."
            mpeAttoCostitutivo.Show()
            Exit Sub
        End If
        bBLOBStorage = ClsServer.StreamToByte(fs)
        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileAttoCostitutivo.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim AC As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "ATTOCOSTITUTIVO_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
         .IdTipoAllegato = TipoFile.ATTO_COSTITUTIVO_ENTE
        }

        Session("LoadedACAccordo") = AC
        rowNoAttoCostitutivo.Visible = False
        rowAttoCostitutivo.Visible = True
        txtACFilename.Text = AC.Filename
        txtACHash.Text = AC.Hash
        txtACData.Text = AC.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("Atto Costitutivo dell'Ente caricato correttamente")
    End Sub

    Protected Sub cmdAllegaST_Click(sender As Object, e As EventArgs) Handles cmdAllegaST.Click
        lblErroreStatuto.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileStatuto.PostedFile Is Nothing Or String.IsNullOrEmpty(fileStatuto.PostedFile.FileName) Then
            lblErroreStatuto.Text = "Non è stato scelto nessun file per il caricamento dello Statuto dell'ente"
            mpeStatuto.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileStatuto) = False Then
            lblErroreStatuto.Text = "Il formato file  dello Statuto dell'ente non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeStatuto.Show()
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileStatuto.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreStatuto.Text = "Attenzione. Impossibile caricare lo Statuto dell'ente vuoto."
            mpeStatuto.Show()
            Exit Sub
        End If
        If iLen > 20971520 Then
            lblErroreStatuto.Text = "Attenzione. La dimensione massima dello Statuto è di 20 MB."
            mpeStatuto.Show()
            Exit Sub
        End If
        bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileStatuto.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim ST As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "STATUTO_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
         .IdTipoAllegato = TipoFile.STATUTO_ENTE
        }

        Session("LoadedStatutoAccordo") = ST
        'Se lo statuto è caricato in Sessione (Inserimento)
        rowNoStatuto.Visible = False
        rowStatuto.Visible = True
        txtStatutoFilename.Text = ST.Filename
        txtStatutoHash.Text = ST.Hash
        txtStatutoData.Text = ST.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("Statuto dell'ente caricato correttamente")
    End Sub

    Protected Sub cmdAllegaDA_Click(sender As Object, e As EventArgs) Handles cmdAllegaDA.Click
        msgErrore.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileDeliberaAdesione.PostedFile Is Nothing Or String.IsNullOrEmpty(fileDeliberaAdesione.PostedFile.FileName) Then
            lblErroreDeliberaAdesione.Text = "Non è stato scelto nessun file per il caricamento della delibera"
            mpeDeliberaAdesione.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileDeliberaAdesione) = False Then
            lblErroreDeliberaAdesione.Text = "Il formato file  della delibera di adesione non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeDeliberaAdesione.Show()
            Exit Sub
        End If

        'Controlli dimensioni del file
        Dim fs = fileDeliberaAdesione.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreDeliberaAdesione.Text = "Attenzione. Impossibile caricare la delibera di adesione dell'ente vuoto."
            mpeDeliberaAdesione.Show()
            Exit Sub
        End If

        If iLen > 20971520 Then
            lblErroreDeliberaAdesione.Text = "Attenzione. La dimensione massima della delibera di adesione è di 20 MB."
            mpeDeliberaAdesione.Show()
            Exit Sub
        End If

        bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileDeliberaAdesione.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim DA As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "DELIBERA_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
         .IdTipoAllegato = TipoFile.DELIBERA_COSTITUZIONE_ENTE
        }

        'If EsistenzaAllegatoEnteDB(DA.Hash) = False Then
        '    lblErroreImpegnoEtico.Text = "È stato inserito un allegato " & DA.Filename & " che è già presente in archivio per questo Ente"
        '    DA = Nothing
        '    mpeImpegnoEtico.Show()
        '    Exit Sub
        'End If

        Session("LoadedDeliberaAdesioneAccordo") = DA
        'Se lo statuto è caricato in Sessione (Inserimento)
        rowNoDeliberaAdesione.Visible = False
        rowDeliberaAdesione.Visible = True
        txtDeliberaAdesioneFileName.Text = DA.Filename
        txtDeliberaAdesioneHash.Text = DA.Hash
        txtDeliberaAdesioneData.Text = DA.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("Delibera caricata correttamente")
    End Sub

    Protected Sub cmdAllegaIE_Click(sender As Object, e As EventArgs) Handles cmdAllegaIE.Click
        msgErrore.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileImpegnoEtico.PostedFile Is Nothing Or String.IsNullOrEmpty(fileImpegnoEtico.PostedFile.FileName) Then
            lblErroreImpegnoEtico.Text = "Non è stato scelto nessun file per il caricamento della Carta d'Impegno Etico"
            mpeImpegnoEtico.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileImpegnoEtico) = False Then
            lblErroreImpegnoEtico.Text = "Il formato file  della della Carta d'Impegno Etico non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeImpegnoEtico.Show()
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileImpegnoEtico.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreImpegnoEtico.Text = "Attenzione. Impossibile caricare la Carta d'Impegno Etico dell'ente vuoto."
            mpeImpegnoEtico.Show()
            Exit Sub
        End If
        If iLen > 20971520 Then
            lblErroreImpegnoEtico.Text = "Attenzione. La dimensione massima della Carta d'Impegno Etico è di 20 MB."
            mpeImpegnoEtico.Show()
            Exit Sub
        End If
        bBLOBStorage = ClsServer.StreamToByte(fs)
        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileImpegnoEtico.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim CIE As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "CARTAIMPEGNOETICO_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
         .IdTipoAllegato = TipoFile.CARTA_IMPEGNO_ETICO
        }

        'If EsistenzaAllegatoEnteDB(CIE.Hash) = False Then
        '    lblErroreImpegnoEtico.Text = "È stato inserito un allegato " & CIE.Filename & " che è già presente in archivio per questo Ente"
        '    CIE = Nothing
        '    mpeImpegnoEtico.Show()
        '    Exit Sub
        'End If

        Session("LoadedCartaImpegnoEticoAccordo") = CIE
        'Se lo statuto è caricato in Sessione (Inserimento)
        rowNoImpegnoEtico.Visible = False
        rowImpegnoEtico.Visible = True
        txtCIEFilename.Text = CIE.Filename
        txtCIEHash.Text = CIE.Hash
        txtCIEData.Text = CIE.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("Carta Impegno Etico caricata correttamente")
    End Sub

    Protected Sub btnEliminaDeliberaAdesione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaDeliberaAdesione.Click
        ClearDeliberaAdesione()
    End Sub

    Protected Sub btnModificaDeliberaAdesione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaDeliberaAdesione.Click
        mpeDeliberaAdesione.Show()
    End Sub

    Protected Sub btnDownloadDeliberaAdesione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadDeliberaAdesione.Click
        If Session("LoadedDeliberaAdesioneAccordo") Is Nothing Then
            msgErrore.Text = "Nessun File caricato"
            ClearDeliberaAdesione()
            Exit Sub
        End If
        Dim DelAdesione As Allegato = Session("LoadedDeliberaAdesioneAccordo")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & DelAdesione.Filename)
        Response.BinaryWrite(DelAdesione.Blob)
        Response.End()
    End Sub

    Protected Sub btnEliminaCIE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaCIE.Click
        ClearSessionImpegnoEtico()
    End Sub

    Protected Sub btnModificaCIE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaCIE.Click
        mpeImpegnoEtico.Show()
    End Sub

    Protected Sub btnDownloadCIE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadCIE.Click
        If Session("LoadedCartaImpegnoEticoAccordo") Is Nothing Then
            msgErrore.Text = "Nessun File caricato"
            ClearSessionImpegnoEtico()
            Exit Sub
        End If
        Dim CIE As Allegato = Session("LoadedCartaImpegnoEticoAccordo")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & CIE.Filename)
        Response.BinaryWrite(CIE.Blob)
        Response.End()
    End Sub

    Protected Sub btnEliminaStatuto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaStatuto.Click
        ClearSessionStatuto()
    End Sub

    Protected Sub btnModificaStatuto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaStatuto.Click
        mpeStatuto.Show()
    End Sub

    Protected Sub btnDownloadStatuto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadStatuto.Click
        If Session("LoadedStatutoAccordo") Is Nothing Then
            msgErrore.Text = "Nessun File caricato"
            ClearSessionStatuto()
            Exit Sub
        End If
        Dim ST As Allegato = Session("LoadedStatutoAccordo")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & ST.Filename)
        Response.BinaryWrite(ST.Blob)
        Response.End()
    End Sub

    Protected Sub btnEliminaAttoCostitutitvo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaAttoCostitutitvo.Click
        ClearSessionAttoCostitutivo()
    End Sub

    Protected Sub btnModificaAttoCostitutivo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaAttoCostitutivo.Click
        mpeAttoCostitutivo.Show()
    End Sub

    Protected Sub btnDownloadAttoCostitutivo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadAttoCostitutivo.Click
        If Session("LoadedACAccordo") Is Nothing Then
            msgErrore.Text = "Nessun File caricato"
            ClearSessionAttoCostitutivo()
            Exit Sub
        End If
        Dim AC As Allegato = Session("LoadedACAccordo")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & AC.Filename)
        Response.BinaryWrite(AC.Blob)
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

    Private Sub CaricaEntiAllegati()
        Dim dtAllegati As DataTable
        Dim CaricaVisualizzaDeliberaAdesioneAccordo As Boolean = Not ddlTipologia.SelectedValue.ToUpper = "PRIVATO"

        Session("LoadedAN") = Nothing
        strsql = String.Empty
        strsql += "SELECT IdEnteDocumento, coalesce(IdTipoAllegato, 0) IdTipoAllegato, FileName, BinData, DataInserimento, len(BinData) FileLength, HashValue, IdEnteFase, Stato FROM EntiDocumenti A "
        strsql += "INNER JOIN ENTI E ON E.IdEnte= " & lblIdEnte.Value
        strsql += " WHERE IDEnteDocumento in (E.IdAllegatoDeliberaStrutturaGestione,  E.IdAllegatoDocumentoNomina,  "
        strsql += "E.IdAllegatoRTDPersonali ,  E.IdAllegatoAttoCostitutivo, E.IdAllegatoStatuto , E.IdAllegatoDeliberaAdesione, "
        strsql += "E.IdAllegatoImpegnoEtico, E.IdAllegatoImpegno)"
        dtAllegati = ClsServer.DataTableGenerico(strsql, Session("Conn"))

        If dtAllegati IsNot Nothing _
        AndAlso dtAllegati.Rows.Count > 0 Then
            'metto in sessione gli allegati

            For Each dr As DataRow In dtAllegati.Rows

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.ATTO_COSTITUTIVO_ENTE Then
                    CaricaAllegatoFromDatarow(dr, "LoadedACAccordo", rowNoAttoCostitutivo, rowAttoCostitutivo, txtACFilename, txtACHash, txtACData)
                    If False Then
                        Dim AllegatoAttoCostitutivo As New Allegato
                        AllegatoAttoCostitutivo.Filename = dr("FileName").ToString()
                        AllegatoAttoCostitutivo.Blob = DirectCast(dr("BinData"), Byte())
                        AllegatoAttoCostitutivo.DataInserimento = CDate(dr("DataInserimento").ToString())
                        AllegatoAttoCostitutivo.Filesize = CInt(dr("FileLength").ToString())
                        AllegatoAttoCostitutivo.Hash = dr("HashValue").ToString()
                        AllegatoAttoCostitutivo.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                        Session("LoadedACAccordo") = AllegatoAttoCostitutivo

                        If AllegatoAttoCostitutivo IsNot Nothing Then
                            'Se l'atto costitutivo è caricato in Sessione (Inserimento)
                            rowNoAttoCostitutivo.Visible = False
                            rowAttoCostitutivo.Visible = True
                            txtACFilename.Text = AllegatoAttoCostitutivo.Filename
                            txtACHash.Text = AllegatoAttoCostitutivo.Hash
                            txtACData.Text = AllegatoAttoCostitutivo.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                        Else
                            'Se l'atto costitutivo non è ancora caricato
                            rowNoAttoCostitutivo.Visible = True
                            rowAttoCostitutivo.Visible = False
                        End If

                    End If
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.STATUTO_ENTE Then
                    CaricaAllegatoFromDatarow(dr, "LoadedStatutoAccordo", rowNoStatuto, rowStatuto, txtStatutoFilename, txtStatutoHash, txtStatutoData)
                    If False Then
                        Dim AllegatoStatuto As New Allegato
                        AllegatoStatuto.Filename = dr("FileName").ToString()
                        AllegatoStatuto.Blob = DirectCast(dr("BinData"), Byte())
                        AllegatoStatuto.DataInserimento = CDate(dr("DataInserimento").ToString())
                        AllegatoStatuto.Filesize = CInt(dr("FileLength").ToString())
                        AllegatoStatuto.Hash = dr("HashValue").ToString()
                        AllegatoStatuto.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                        Session("LoadedStatutoAccordo") = AllegatoStatuto

                        If AllegatoStatuto IsNot Nothing Then
                            'Se l'atto costitutivo è caricato in Sessione (Inserimento)
                            rowNoStatuto.Visible = False
                            rowStatuto.Visible = True
                            txtStatutoFilename.Text = AllegatoStatuto.Filename
                            txtStatutoHash.Text = AllegatoStatuto.Hash
                            txtStatutoData.Text = AllegatoStatuto.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                        Else
                            'Se lo statuto non è ancora caricato
                            rowNoStatuto.Visible = True
                            rowStatuto.Visible = False
                        End If
                    End If

                End If


                If ddlTipologia.SelectedValue = String.Empty OrElse ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then

                    If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA_COSTITUZIONE_ENTE AndAlso CaricaVisualizzaDeliberaAdesioneAccordo Then
                        CaricaAllegatoFromDatarow(dr, "LoadedDeliberaAdesioneAccordo", rowNoDeliberaAdesione, rowDeliberaAdesione, txtDeliberaAdesioneFileName, txtDeliberaAdesioneHash, txtDeliberaAdesioneData)
                        If False Then
                            Dim AllegatoDeliberaAdedsione As New Allegato
                            AllegatoDeliberaAdedsione.Filename = dr("FileName").ToString()
                            AllegatoDeliberaAdedsione.Blob = DirectCast(dr("BinData"), Byte())
                            AllegatoDeliberaAdedsione.DataInserimento = CDate(dr("DataInserimento").ToString())
                            AllegatoDeliberaAdedsione.Filesize = CInt(dr("FileLength").ToString())
                            AllegatoDeliberaAdedsione.Hash = dr("HashValue").ToString()
                            AllegatoDeliberaAdedsione.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                            Session("LoadedDeliberaAdesioneAccordo") = AllegatoDeliberaAdedsione

                            If AllegatoDeliberaAdedsione IsNot Nothing Then
                                'Se la delibera è stata caricata in Sessione (Inserimento)
                                rowNoDeliberaAdesione.Visible = False
                                rowDeliberaAdesione.Visible = True
                                txtDeliberaAdesioneFileName.Text = AllegatoDeliberaAdedsione.Filename
                                txtDeliberaAdesioneHash.Text = AllegatoDeliberaAdedsione.Hash
                                txtDeliberaAdesioneData.Text = AllegatoDeliberaAdedsione.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                            Else
                                'Se la delibera non è ancora caricato
                                rowNoDeliberaAdesione.Visible = True
                                rowDeliberaAdesione.Visible = False
                            End If

                        End If

                    End If
                Else
                    rowNoDeliberaAdesione.Visible = False
                    rowDeliberaAdesione.Visible = False
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.CARTA_IMPEGNO_ETICO Then
                    CaricaAllegatoFromDatarow(dr, "LoadedCartaImpegnoEticoAccordo", rowNoImpegnoEtico, rowImpegnoEtico, txtCIEFilename, txtCIEHash, txtCIEData)
                    If False Then
                        Dim AllegatoImpegnoEtico As New Allegato
                        AllegatoImpegnoEtico.Filename = dr("FileName").ToString()
                        AllegatoImpegnoEtico.Blob = DirectCast(dr("BinData"), Byte())
                        AllegatoImpegnoEtico.DataInserimento = CDate(dr("DataInserimento").ToString())
                        AllegatoImpegnoEtico.Filesize = CInt(dr("FileLength").ToString())
                        AllegatoImpegnoEtico.Hash = dr("HashValue").ToString()
                        AllegatoImpegnoEtico.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                        Session("LoadedCartaImpegnoEticoAccordo") = AllegatoImpegnoEtico

                        If AllegatoImpegnoEtico IsNot Nothing Then
                            'Se l'atto di  è caricato in Sessione (Inserimento)
                            rowNoImpegnoEtico.Visible = False
                            rowImpegnoEtico.Visible = True
                            txtCIEFilename.Text = AllegatoImpegnoEtico.Filename
                            txtCIEHash.Text = AllegatoImpegnoEtico.Hash
                            txtCIEData.Text = AllegatoImpegnoEtico.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                        Else
                            'Se la delibera  non è ancora caricato
                            rowNoImpegnoEtico.Visible = True
                            rowImpegnoEtico.Visible = False
                        End If

                    End If

                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.CARTA_IMPEGNO Then
                    CaricaAllegatoFromDatarow(dr, "LoadedCartaImpegnoAccordo", rowNoCartaImpegno, rowCartaImpegno, txtCIFilename, txtCIHash, txtCIData)
                    If False Then
                        Dim AllegatoImpegno As New Allegato
                        AllegatoImpegno.Filename = dr("FileName").ToString()
                        AllegatoImpegno.Blob = DirectCast(dr("BinData"), Byte())
                        AllegatoImpegno.DataInserimento = CDate(dr("DataInserimento").ToString())
                        AllegatoImpegno.Filesize = CInt(dr("FileLength").ToString())
                        AllegatoImpegno.Hash = dr("HashValue").ToString()
                        AllegatoImpegno.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                        Session("LoadedCartaImpegnoAccordo") = AllegatoImpegno

                        If AllegatoImpegno IsNot Nothing Then
                            'Se l'atto di  è caricato in Sessione (Inserimento)
                            rowNoCartaImpegno.Visible = False
                            rowCartaImpegno.Visible = True
                            txtCIFilename.Text = AllegatoImpegno.Filename
                            txtCIHash.Text = AllegatoImpegno.Hash
                            txtCIData.Text = AllegatoImpegno.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                        Else
                            'Se la delibera  non è ancora caricato
                            rowNoCartaImpegno.Visible = True
                            rowCartaImpegno.Visible = False
                        End If
                    End If
                End If
            Next
        End If

        If Not CaricaVisualizzaDeliberaAdesioneAccordo Then
            rowNoDeliberaAdesione.Visible = False
            rowDeliberaAdesione.Visible = False
        End If
    End Sub
    Private Function EsistenzaAllegatoEnte() As Boolean
        EsistenzaAllegatoEnte = True
    End Function

    Protected Sub cmdAllegaCI_Click(sender As Object, e As EventArgs) Handles cmdAllegaCI.Click
        msgErrore.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileImpegno.PostedFile Is Nothing Or String.IsNullOrEmpty(fileImpegno.PostedFile.FileName) Then
            lblErroreImpegno.Text = "Non è stato scelto nessun file per il caricamento della Dichiarazione di Impegno "
            mpeImpegno.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileImpegno) = False Then
            lblErroreImpegno.Text = "Il formato file  della Dichiarazione di Impegno non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeImpegno.Show()
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileImpegno.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreImpegno.Text = "Attenzione. Impossibile caricare la Dichiarazione di Impegno dell'ente vuoto."
            mpeImpegno.Show()
            Exit Sub
        End If
        If iLen > 20971520 Then
            lblErroreImpegno.Text = "Attenzione. La dimensione massima della Dichiarazione di Impegno è di 20 MB."
            mpeImpegno.Show()
            Exit Sub
        End If
        bBLOBStorage = ClsServer.StreamToByte(fs)
        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileImpegno.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim CI As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "DICHIARAZIONEIMPEGNO_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
         .IdTipoAllegato = TipoFile.CARTA_IMPEGNO
        }

        Session("LoadedCartaImpegnoAccordo") = CI
        'Se lo statuto è caricato in Sessione (Inserimento)
        rowNoCartaImpegno.Visible = False
        rowCartaImpegno.Visible = True
        txtCIFilename.Text = CI.Filename
        txtCIHash.Text = CI.Hash
        txtCIData.Text = CI.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")

        MessaggiConvalida("Dichiarazione di Impegno caricata correttamente")
    End Sub

    Protected Sub btnDownloadCI_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadCI.Click
        If Session("LoadedCartaImpegnoAccordo") Is Nothing Then
            msgErrore.Text = "Nessun File caricato"
            ClearSessionAttoCostitutivo()
            Exit Sub
        End If
        Dim CI As Allegato = Session("LoadedCartaImpegnoAccordo")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & CI.Filename)
        Response.BinaryWrite(CI.Blob)
        Response.End()
    End Sub

    Protected Sub btnModificaCI_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaCI.Click
        mpeImpegno.Show()
    End Sub

    Protected Sub btnEliminaCI_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaCI.Click
        ClearSessionCartaImpegno()
    End Sub

    Protected Sub cmdAllegaImpegno_Click(sender As Object, e As EventArgs) Handles cmdAllegaImpegno.Click
        mpeImpegno.Show()
    End Sub

    Private Function CaricaEntiAllegatiDaDB(Optional IdEnteFiglio As Integer = 0) As List(Of Allegato)

        Dim dtAllegati As DataTable
        Dim ListaAllegato As New List(Of Allegato)
        Dim Tabella As String

        If IdEnteFiglio = 0 Then
            IdEnteFiglio = lblIdEnte.Value
        End If

        strsql = String.Empty
        strsql &= "SELECT IdEnteDocumento,coalesce(IdTipoAllegato, 0) IdTipoAllegato,FileName,HashValue FROM entidocumenti A"
        strsql &= " JOIN " & IIf(isVariazione, "Accreditamento_VariazioneEntiFiglio", "ENTI") & " E On E.IDENTE = " & lblIdEnte.Value
        strsql &= " And  IdEnteDocumento In (E.IdAllegatoAttoCostitutivo, E.IdAllegatoStatuto, E.IdAllegatoDeliberaAdesione, "
        strsql &= "E.IdAllegatoImpegnoEtico, E.IdAllegatoImpegno)"
        If isVariazione Then strsql &= " AND E.StatoVariazione = 0"
        dtAllegati = ClsServer.DataTableGenerico(strsql, Session("Conn"))

        If dtAllegati IsNot Nothing _
        AndAlso dtAllegati.Rows.Count > 0 Then
            For Each dr As DataRow In dtAllegati.Rows
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DOCUMENTO_NOMINA Then
                    Dim AllegatoAN As New Allegato
                    AllegatoAN.Id = dr("IdEnteDocumento")
                    AllegatoAN.Filename = CStr(dr("FileName"))
                    AllegatoAN.Blob = Nothing
                    AllegatoAN.DataInserimento = Nothing
                    AllegatoAN.Filesize = Nothing
                    AllegatoAN.Hash = dr("HashValue").ToString()
                    AllegatoAN.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoAN)
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA Then
                    Dim AllegatoDelibera As New Allegato
                    AllegatoDelibera.Id = dr("IdEnteDocumento")
                    AllegatoDelibera.Filename = dr("FileName").ToString()
                    AllegatoDelibera.Blob = Nothing
                    AllegatoDelibera.DataInserimento = Nothing
                    AllegatoDelibera.Filesize = Nothing
                    AllegatoDelibera.Hash = dr("HashValue").ToString()
                    AllegatoDelibera.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoDelibera)
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.ATTO_COSTITUTIVO_ENTE Then
                    Dim AllegatoAttoCostitutivo As New Allegato
                    AllegatoAttoCostitutivo.Id = dr("IdEnteDocumento")
                    AllegatoAttoCostitutivo.Filename = dr("FileName").ToString()
                    AllegatoAttoCostitutivo.Blob = Nothing
                    AllegatoAttoCostitutivo.DataInserimento = Nothing
                    AllegatoAttoCostitutivo.Filesize = Nothing
                    AllegatoAttoCostitutivo.Hash = dr("HashValue").ToString()
                    AllegatoAttoCostitutivo.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoAttoCostitutivo)
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.STATUTO_ENTE Then
                    Dim AllegatoStatuto As New Allegato
                    AllegatoStatuto.Id = dr("IdEnteDocumento")
                    AllegatoStatuto.Filename = dr("FileName").ToString()
                    AllegatoStatuto.Blob = Nothing
                    AllegatoStatuto.DataInserimento = Nothing
                    AllegatoStatuto.Filesize = Nothing
                    AllegatoStatuto.Hash = dr("HashValue").ToString()
                    AllegatoStatuto.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoStatuto)
                End If

                If ddlTipologia.SelectedValue = String.Empty OrElse ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
                    If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA_COSTITUZIONE_ENTE Then
                        Dim AllegatoDeliberaAdesione As New Allegato
                        AllegatoDeliberaAdesione.Id = dr("IdEnteDocumento")
                        AllegatoDeliberaAdesione.Filename = dr("FileName").ToString()
                        AllegatoDeliberaAdesione.Blob = Nothing
                        AllegatoDeliberaAdesione.DataInserimento = Nothing
                        AllegatoDeliberaAdesione.Filesize = Nothing
                        AllegatoDeliberaAdesione.Hash = dr("HashValue").ToString()
                        AllegatoDeliberaAdesione.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                        ListaAllegato.Add(AllegatoDeliberaAdesione)
                    End If
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.CARTA_IMPEGNO_ETICO Then
                    Dim AllegatoImpegnoEtico As New Allegato
                    AllegatoImpegnoEtico.Id = dr("IdEnteDocumento")
                    AllegatoImpegnoEtico.Filename = dr("FileName").ToString()
                    AllegatoImpegnoEtico.Blob = Nothing
                    AllegatoImpegnoEtico.DataInserimento = Nothing
                    AllegatoImpegnoEtico.Filesize = Nothing
                    AllegatoImpegnoEtico.Hash = dr("HashValue").ToString()
                    AllegatoImpegnoEtico.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoImpegnoEtico)
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.CARTA_IMPEGNO Then
                    Dim AllegatoImpegno As New Allegato
                    AllegatoImpegno.Id = dr("IdEnteDocumento")
                    AllegatoImpegno.Filename = dr("FileName").ToString()
                    AllegatoImpegno.Blob = Nothing
                    AllegatoImpegno.DataInserimento = Nothing
                    AllegatoImpegno.Filesize = Nothing
                    AllegatoImpegno.Hash = dr("HashValue").ToString()
                    AllegatoImpegno.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoImpegno)
                End If

            Next
        End If

        CaricaEntiAllegatiDaDB = ListaAllegato

    End Function
    Private Function ConfrontaAllegati(ListaAllegati As List(Of Allegato), allegatoSessione As Allegato, ByRef idAllegato As Integer) As Boolean
        Dim allegatoTrovato As Allegato

        allegatoTrovato = ListaAllegati.Find(Function(l) l.IdTipoAllegato = allegatoSessione.IdTipoAllegato)

        If allegatoTrovato IsNot Nothing Then
            If allegatoTrovato.Hash = allegatoSessione.Hash Then
                idAllegato = allegatoTrovato.Id
                ConfrontaAllegati = True
                Exit Function
            End If
        End If

        idAllegato = 0

    End Function

    Protected Sub cmdModNoAdeguamento_Click(sender As Object, e As EventArgs) Handles cmdModNoAdeguamento.Click
        Response.Redirect("WfrmAnagraficaEnteAccordoNoAdeguamento.aspx?id=" & Session("IdEnteAccoglienza") & "&identerelazione=" & Session("idEnteRelazione"))
    End Sub

    Private Sub CaricaEntiAllegatiVariazioni()
        Dim dtAllegati As DataTable
        Dim CaricaVisualizzaDeliberaAdesioneAccordo As Boolean = Not ddlTipologia.SelectedValue.ToUpper = "PRIVATO"

        Session("LoadedAN") = Nothing
        strsql = String.Empty
        strsql += "SELECT IdEnteDocumento, coalesce(IdTipoAllegato,0) IdTipoAllegato, FileName, BinData, len(BinData) FileLength, HashValue, Datainserimento, IdEnteFase, Stato FROM entidocumenti A"
        strsql += " JOIN Accreditamento_VariazioneEntiFiglio E ON E.IdEnte = " + lblIdEnte.Value
        strsql += " WHERE IdEnteDocumento in (E.IdAllegatoAttoCostitutivo,  E.IdAllegatoStatuto, E.IdAllegatoDeliberaAdesione, "
        strsql += " E.IdAllegatoImpegnoEtico,  E.IdAllegatoImpegno)"
        strsql += " AND E.StatoVariazione = 0"

        dtAllegati = ClsServer.DataTableGenerico(strsql, Session("Conn"))

        If dtAllegati IsNot Nothing _
        AndAlso dtAllegati.Rows.Count > 0 Then
            'metto in sessione gli allegati

            For Each dr As DataRow In dtAllegati.Rows
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.ATTO_COSTITUTIVO_ENTE Then
                    CaricaAllegatoFromDatarow(dr, "LoadedACAccordo", rowNoAttoCostitutivo, rowAttoCostitutivo, txtACFilename, txtACHash, txtACData)
                ElseIf CInt(dr("IdTipoAllegato").ToString()) = TipoFile.STATUTO_ENTE Then
                    CaricaAllegatoFromDatarow(dr, "LoadedStatutoAccordo", rowNoStatuto, rowStatuto, txtStatutoFilename, txtStatutoHash, txtACData)
                ElseIf CInt(dr("IdTipoAllegato").ToString()) = TipoFile.CARTA_IMPEGNO Then
                    CaricaAllegatoFromDatarow(dr, "LoadedCartaImpegnoAccordo", rowNoCartaImpegno, rowCartaImpegno, txtCIFilename, txtCIHash, txtCIData)
                ElseIf CInt(dr("IdTipoAllegato").ToString()) = TipoFile.CARTA_IMPEGNO_ETICO Then
                    CaricaAllegatoFromDatarow(dr, "LoadedCartaImpegnoEticoAccordo", rowNoImpegnoEtico, rowImpegnoEtico, txtCIEFilename, txtCIEHash, txtCIEData)
                ElseIf CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA_COSTITUZIONE_ENTE AndAlso CaricaVisualizzaDeliberaAdesioneAccordo Then
                    CaricaAllegatoFromDatarow(dr, "LoadedDeliberaAdesioneAccordo", rowNoDeliberaAdesione, rowDeliberaAdesione, txtDeliberaAdesioneFileName, txtDeliberaAdesioneHash, txtDeliberaAdesioneData)
                End If
            Next
        End If

        If Not CaricaVisualizzaDeliberaAdesioneAccordo Then
            rowNoDeliberaAdesione.Visible = False
            rowDeliberaAdesione.Visible = False
        End If
    End Sub
    Sub RipristinaPulsantiAllegati()
        helper.RipristinaStyleDatiModificati(btnDownloadAttoCostitutivo)
        helper.RipristinaStyleDatiModificati(btnEliminaAttoCostitutitvo)
        helper.RipristinaStyleDatiModificati(btnModificaAttoCostitutivo)

        helper.RipristinaStyleDatiModificati(btnDownloadCI)
        helper.RipristinaStyleDatiModificati(btnEliminaCI)
        helper.RipristinaStyleDatiModificati(btnModificaCI)

        helper.RipristinaStyleDatiModificati(btnDownloadCIE)
        helper.RipristinaStyleDatiModificati(btnEliminaCIE)
        helper.RipristinaStyleDatiModificati(btnModificaCIE)

        helper.RipristinaStyleDatiModificati(btnDownloadDeliberaAdesione)
        helper.RipristinaStyleDatiModificati(btnEliminaDeliberaAdesione)
        helper.RipristinaStyleDatiModificati(btnModificaDeliberaAdesione)

        helper.RipristinaStyleDatiModificati(btnDownloadStatuto)
        helper.RipristinaStyleDatiModificati(btnEliminaStatuto)
        helper.RipristinaStyleDatiModificati(btnModificaStatuto)
    End Sub
    Sub ModificaPulsantiAllegati()
        helper.ModificaStyleDatiModificati(btnDownloadAttoCostitutivo)
        helper.ModificaStyleDatiModificati(btnEliminaAttoCostitutitvo)
        helper.ModificaStyleDatiModificati(btnModificaAttoCostitutivo)

        helper.ModificaStyleDatiModificati(btnDownloadCI)
        helper.ModificaStyleDatiModificati(btnEliminaCI)
        helper.ModificaStyleDatiModificati(btnModificaCI)

        helper.ModificaStyleDatiModificati(btnDownloadCIE)
        helper.ModificaStyleDatiModificati(btnEliminaCIE)
        helper.ModificaStyleDatiModificati(btnModificaCIE)

        helper.ModificaStyleDatiModificati(btnDownloadDeliberaAdesione)
        helper.ModificaStyleDatiModificati(btnEliminaDeliberaAdesione)
        helper.ModificaStyleDatiModificati(btnModificaDeliberaAdesione)

        helper.ModificaStyleDatiModificati(btnDownloadStatuto)
        helper.ModificaStyleDatiModificati(btnEliminaStatuto)
        helper.ModificaStyleDatiModificati(btnModificaStatuto)
    End Sub

    Function getLabel(ByVal gRow As GridViewRow, ByVal contr As String) As String
        Dim label As Label = DirectCast(gRow.FindControl(contr), Label)
        Return Replace(label.Text, vbCrLf, "<br><br>")
    End Function
    Function getTextBox(ByVal gRow As GridViewRow, ByVal contr As String) As String
        Dim tb As TextBox = DirectCast(gRow.FindControl(contr), TextBox)
        Return Replace(tb.Text, vbCrLf, "<br><br>")
    End Function
    Function settoriHTML() As String
        Dim s As String = ""
        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
            If check.Checked Then
                s &= String.Format("<h1>{0}</h1>", getLabel(gRow, "lblSettoriIntervento"))
                s &= String.Format("<b>{0}</b>: {1}<br><br>", getLabel(gRow, "lblAnnoEsperienza3"), getTextBox(gRow, "txtDesEsperienza3"))
                s &= String.Format("<b>{0}</b>: {1}<br><br>", getLabel(gRow, "lblAnnoEsperienza2"), getTextBox(gRow, "txtDesEsperienza2"))
                s &= String.Format("<b>{0}</b>: {1}<br><br>", getLabel(gRow, "lblAnnoEsperienza1"), getTextBox(gRow, "txtDesEsperienza1"))

            End If
        Next
        Return s
    End Function
    Private Sub cmdPdfSettori_Click(sender As Object, e As EventArgs) Handles cmdPdfSettori.Click
        Dim oW As New AsposeWord()
        oW.open(Server.MapPath("download/Master/templateSettori.docx"))
        oW.addFieldValue("nomeEnte", txtdenominazione.Text)
        oW.addFieldHtml("htmlSettori", settoriHTML())
        oW.merge()
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & txtCodFis.Text & "_esperienze_settori.pdf")
        Response.BinaryWrite(oW.pdfBytes)
        Response.End()
    End Sub

    Private Function AbilitaSettori() As Boolean
        Dim dtrGen As SqlClient.SqlDataReader
        Dim blnAdeguamento As Boolean
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        strsql = "select IdEnteFase from EntiFasi where TipoFase = 2 and GETDATE() between DataInizioFase and DataFineFase and IdEnte = " & Session("IdEnte")
        dtrGen = ClsServer.CreaDatareader(strsql, Session("Conn"))
        If dtrGen.HasRows = True Then
            blnAdeguamento = True
        Else
            blnAdeguamento = False
        End If
        dtrGen.Close()
        dtrGen = Nothing

        Dim blnAbilitaSettori As Boolean = True
        strsql = "SELECT VALORE  FROM Configurazioni where Parametro='BLOCCO_ACCREDITAMENTO'"
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        dtrGen = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGen.Read()

        If dtrGen("Valore") = "SI" And Session("TipoUtente") = "E" And blnAdeguamento Then
            blnAbilitaSettori = False
        End If
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        Return blnAbilitaSettori

    End Function

    Private Sub CmdSalvaDoc_Click(sender As Object, e As System.EventArgs) Handles CmdSalvaDoc.Click
        msgErrore.Text = String.Empty
        msgConferma.Text = String.Empty
        'Generato da Alessandra Taballione il 23/02/2004
        'Effettuo Controllo sulla Partita Iva

        'If ValidazioneServerSalva() = False Then
        '    Exit Sub
        'End If

        Dim comuni As Integer
        comuni = 0
        Dim AlboEnte As String
        Dim IDAllegatoDeliberaAdesione As Integer
        Dim IDAllegatoStatuto As Integer
        Dim IDAllegatoAttoCostitutivo As Integer
        Dim IDAllegatoImpegnoEtico As Integer
        Dim IDAllegatoImpegno As Integer


        Dim ListaAllegati As New List(Of Allegato)


        Dim IDAllegato As Integer

        ListaAllegati = CaricaEntiAllegatiDaDB()

        MyTransaction = CType(Session("Conn"), System.Data.SqlClient.SqlConnection).BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
        If Session("LoadedACAccordo") IsNot Nothing Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedACAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedACAccordo"), Allegato))) Then
                    IDAllegatoAttoCostitutivo = IDAllegato
                Else
                    MyTransaction = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoAttoCostitutivo = IDAllegato
            End If
        End If

        If (Session("LoadedStatutoAccordo") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedStatutoAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedStatutoAccordo"), Allegato))) Then
                    IDAllegatoStatuto = IDAllegato
                Else
                    MyTransaction = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoStatuto = IDAllegato
            End If
        End If

        If (Session("LoadedDeliberaAdesioneAccordo") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedDeliberaAdesioneAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedDeliberaAdesioneAccordo"), Allegato))) Then
                    IDAllegatoDeliberaAdesione = IDAllegato
                Else
                    MyTransaction = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoDeliberaAdesione = IDAllegato
            End If
        End If

        If (Session("LoadedCartaImpegnoEticoAccordo") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedCartaImpegnoEticoAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedCartaImpegnoEticoAccordo"), Allegato))) Then
                    IDAllegatoImpegnoEtico = IDAllegato
                Else
                    MyTransaction = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoImpegnoEtico = IDAllegato
            End If
        End If

        If (Session("LoadedCartaImpegnoAccordo") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedCartaImpegnoAccordo"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(MyTransaction, IDAllegato, DirectCast(Session("LoadedCartaImpegnoAccordo"), Allegato))) Then
                    IDAllegatoImpegno = IDAllegato
                Else
                    MyTransaction = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoImpegno = IDAllegato
            End If
        End If

        If Session("IdStatoEnte") <> 8 Then ' se adeguamento
            ModificaEnte(AlboEnte, IDAllegatoAttoCostitutivo, IDAllegatoStatuto, IDAllegatoDeliberaAdesione, IDAllegatoImpegnoEtico, IDAllegatoImpegno)
        Else 'iscrizione
            Dim SqlCmd As New SqlClient.SqlCommand

            strsql = "UPDATE ENTI SET " & _
                    " IdAllegatoAttoCostitutivo = " & IDAllegatoAttoCostitutivo & " " & _
                    "	,IdAllegatoStatuto = " & IDAllegatoStatuto & " " & _
                    "	,IdAllegatoDeliberaAdesione = " & IDAllegatoDeliberaAdesione & " " & _
                    "	,IdAllegatoImpegnoEtico = " & IDAllegatoImpegnoEtico & " " & _
                    "	,IdAllegatoImpegno = " & IDAllegatoImpegno & " " & _
                    " WHERE idente = " & lblIdEnte.Value
            SqlCmd.CommandText = strsql
            SqlCmd.CommandType = CommandType.Text
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Transaction = MyTransaction

            SqlCmd.ExecuteNonQuery()

            If (AggiornaSettoriAreeIntervento(lblIdEnte.Value)) Then
                MyTransaction.Commit()
                MyTransaction = Nothing
                Exit Sub
            Else
                MyTransaction = Nothing
                Exit Sub
            End If

        End If
        LoadMaschera()
        msgConferma.Visible = True
        msgConferma.Text = "Salvataggio effettuato."

        Session("IdComune") = Nothing
        Dim identefiglio As Integer = IIf(Request.QueryString("id") = Nothing, 0, Request.QueryString("id"))
        If identefiglio > 0 Then EvidenziaDatiModificati(identefiglio)
    End Sub

    Sub AnnullaModificaDocumenti(IdEnte As String)
        'VA CHIAMATA PRIMA DELL'ANNULLAMENTO RECORD ACCREDITAMENTO
        Dim SqlCmd As New SqlCommand
        SqlCmd.CommandText = "SP_ACCREDITAMENTO_ANNULLA_MODIFICA_DOCUMENTI_ENTE_ACCOGLIENZA"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("Conn")
        SqlCmd.Parameters.Clear()
        SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte

        SqlCmd.ExecuteNonQuery()
    End Sub
End Class

