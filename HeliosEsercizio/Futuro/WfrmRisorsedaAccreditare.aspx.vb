Imports System.IO

Public Class WfrmRisorsedaAccreditare
    Inherits System.Web.UI.Page

    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim mydataset As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '***Generata da Gianluigi Paesani in data:09/06/04
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then 'controlli formali per la session utente
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then

            'If Not Server.HtmlDecode(Request.Cookies("InfoRif")("txtCodiceEnte")) Is Nothing Then
            '    If Session("txtCodEnte") = Request.Cookies("InfoRif")("txtCodiceEnte") Then
            '        RecuperaCookie()
            '    End If
            'End If
            'Dim appCookie As New HttpCookie("InfoRif")
            If Not Request.Cookies("InfoRif") Is Nothing Then
                If Session("txtCodEnte") = Request.Cookies("InfoRif")("txtCodiceEnte") Then
                    RecuperaCookie()
                End If
            End If
            CaricaRuoli()
            ddlTipoRicerca.Items.Add("")
            ddlTipoRicerca.Items.Add("Nuovi Accreditamenti")
            ddlTipoRicerca.Items.Add("Adeguamenti")

            If Session("CodiceRegioneEnte") <> "" Then
                txtCodiceEnte.Text = Session("txtCodEnte")
            End If
            If Not Request.QueryString("Provenienza") Is Nothing Then
                If Request.QueryString("Provenienza") = "Conferma" Then
                    ConfermaServizio(Request.QueryString("strIDEnteAcquisizione"))
                Else
                    RespingiServizio(Request.QueryString("strIDEnteAcquisizione"))
                End If
                PopolaGriglia(0)
                RicercaServizi()
            End If
            If Not Request.QueryString("Accetta") Is Nothing Then
                If Request.QueryString("Accetta") = "si" Then
                    ConfermaRuolo(Request.QueryString("strIDRuolo"), Request.QueryString("strIdEntePersonale"))
                Else
                    RespingiRuolo(Request.QueryString("strIDRuolo"), Request.QueryString("strIdEntePersonale"))
                End If
                PopolaGriglia(0)
                RicercaServizi()
            End If
            'PopolaGriglia(0)
            'RicercaServizi()

        End If
        If dtgRisultatoRicerca.Items.Count = 0 Then
            CmdEsportaRisorse.Visible = False
            lblRisorse.Visible = False
        Else
            CmdEsportaRisorse.Visible = True
            lblRisorse.Visible = True
        End If

        If dgServizi.Items.Count = 0 Then
            CmdEsportaServizi.Visible = False
            lblServizi.Visible = False
        Else
            CmdEsportaServizi.Visible = True
            lblServizi.Visible = True
        End If
    End Sub

    'carico la combo dei ruoli
    Sub CaricaRuoli()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id del tizio inserito
        Dim dtrRuoloPrincipale As System.Data.SqlClient.SqlDataReader
        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query
                'lafaccio union con '0' e 'Selezionare' perchè così aggiungo una riga vuota
                strSQL = "select IDRuolo,Ruolo from ruoli where ruoli.ruoloaccreditamento=1 and  Ruoli.Nascosto = 0 "
                strSQL = strSQL & "union "
                strSQL = strSQL & "select '0',' Selezionare ' from ruoli where ruoli.ruoloaccreditamento=1 and  Ruoli.Nascosto = 0 order by ruolo"
                'chiudo il datareader se aperto
                If Not dtrRuoloPrincipale Is Nothing Then
                    dtrRuoloPrincipale.Close()
                    dtrRuoloPrincipale = Nothing
                End If
                'eseguo la query
                dtrRuoloPrincipale = ClsServer.CreaDatareader(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlRuolo.DataSource = dtrRuoloPrincipale
                ddlRuolo.Items.Add("")
                ddlRuolo.DataTextField = "Ruolo"
                ddlRuolo.DataValueField = "IDRuolo"
                ddlRuolo.DataBind()
                'chiudo il datareader se aperto
                If Not dtrRuoloPrincipale Is Nothing Then
                    dtrRuoloPrincipale.Close()
                    dtrRuoloPrincipale = Nothing
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try

    End Sub

    'routine che uso per Confermare il servizio
    Sub ConfermaServizio(ByVal strIDEnteAcquisizione As String)
        'stringa per la delete
        Dim strsql As String

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
        Try
            strsql = "update EntiAcquisizioneServizi set StatoRichiesta=1, DataConferma=GetDate(), UserNameConferma='" & Session("Utente") & "' where IdEnteAcquisizione='" & strIDEnteAcquisizione & "'"
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    'routine che uso per RespingiServizio il servizio
    Sub RespingiServizio(ByVal strIDEnteAcquisizione As String)
        'stringa per la delete
        Dim strsql As String

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
        Try
            strsql = "update EntiAcquisizioneServizi set StatoRichiesta=3, DataConferma=GetDate(), UserNameConferma='" & Session("Utente") & "' where IdEnteAcquisizione='" & strIDEnteAcquisizione & "'"
            myCommand.CommandText = strsql
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub PopolaGriglia(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        '***Generata da Gianluigi Paesani in data:09/06/04
        '***Esegue query per ricerca
        'verifico chiamata da dove viene
        'datareader che conterrà l'id regione compentenza
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader
        'per le regioni
        Dim strsqlReg As String

        If bytVerifica = 1 Then dtgRisultatoRicerca.CurrentPageIndex = bytpage

        'Dim strquery As String = "SELECT b.idente,b.CodiceRegione, a.identepersonale, a.cognome + ' ' + a.nome as Nominativo, d.ruolo,b.denominazione" & _
        '            " as denominazione,convert(varchar,c.datainiziovalidità,103) as data,d.idruolo," & _
        '            " '<img src=images/mini_conferma.jpg title=""Conferma"" style=""cursor: hand"" onclick=""javascript: ConfServRuolo(' + convert(varchar,d.idruolo) + ',' + convert(varchar,a.identepersonale) + ')"" border=0>' as Conferma, " & _
        '            " '<img src=images/pers.jpg title=""Seleziona Risorsa"" border=0>' as Immo, " & _
        '            " '<img src=images/canc.jpg title=""Respingi"" style=""cursor: hand"" onclick=""javascript: RespServRuolo(' + convert(varchar,d.idruolo) + ',' + convert(varchar,a.identepersonale) + ')"" border=0>' as Respingi, ef.IdEnteFase, " & _
        '            " CodFis " & _
        '            " FROM entepersonale a INNER JOIN" & _
        '            " enti b ON a.IDEnte = b.IDEnte" & _
        '            " inner join classiaccreditamento ca on ca.idclasseaccreditamento=b.idclasseaccreditamento " & _
        '            " inner join statienti on statienti.idstatoente=b.idstatoente " & _
        '            " INNER JOIN entepersonaleruoli c ON a.IDEntePersonale = c.IDEntePersonale" & _
        '            " INNER JOIN ruoli d ON c.IDRuolo = d.IDRuolo " & _
        '            " INNER JOIN EntiFasi_Risorse efr on efr.IdEntePersonaleRuolo = c.IDEntePersonaleRuolo " & _
        '            " INNER JOIN EntiFasi ef on efr.IdEnteFase = ef.IdEnteFase " & _
        '            "	LEFT JOIN (SELECT CodiceFiscale AS CodFis " & _
        '            "               FROM CronologiaEntePersonaleRuoli a " & _
        '            "                INNER JOIN entepersonaleruoli b ON a.IDEntePersonaleRuolo = b.IDEntePersonaleRuolo " & _
        '            "                INNER JOIN entepersonale c ON b.IDEntePersonale = c.IDEntePersonale " & _
        '            "               WHERE a.Accreditato = -1) AS Storico ON a.CodiceFiscale = Storico.CodFis " & _
        '            " WHERE c.datafinevalidità is null and c.accreditato=0 and statienti.istruttoria=1 and d.ruoloaccreditamento=1 and  d.Nascosto = 0  and ef.Stato =3"
        Dim strquery As String = "SELECT distinct b.idente,b.CodiceRegione, a.identepersonale, a.cognome + ' ' + a.nome as Nominativo, d.ruolo,b.denominazione" & _
                    " as denominazione,convert(varchar,c.datainiziovalidità,103) as data,d.idruolo," & _
                    " '<img src=images/mini_conferma.jpg title=""Conferma"" style=""cursor: hand"" onclick=""javascript: ConfServRuolo(' + convert(varchar,d.idruolo) + ',' + convert(varchar,a.identepersonale) + ')"" border=0>' as Conferma, " & _
                    " '<img src=images/pers.jpg title=""Seleziona Risorsa"" border=0>' as Immo, " & _
                    " '<img src=images/canc.jpg title=""Respingi"" style=""cursor: hand"" onclick=""javascript: RespServRuolo(' + convert(varchar,d.idruolo) + ',' + convert(varchar,a.identepersonale) + ')"" border=0>' as Respingi, ef.IdEnteFase, " & _
                    " CodFis,year(c.datainiziovalidità),MONTH(c.datainiziovalidità),DAY(c.datainiziovalidità),a.cognome " & _
                    " FROM entepersonale a INNER JOIN" & _
                    " enti b ON a.IDEnte = b.IDEnte" & _
                    " inner join classiaccreditamento ca on ca.idclasseaccreditamento=b.idclasseaccreditamento " & _
                    " inner join statienti on statienti.idstatoente=b.idstatoente " & _
                    " INNER JOIN entepersonaleruoli c ON a.IDEntePersonale = c.IDEntePersonale" & _
                    " INNER JOIN ruoli d ON c.IDRuolo = d.IDRuolo " & _
                    " INNER JOIN EntiFasi_Risorse efr on efr.IdEntePersonaleRuolo = c.IDEntePersonaleRuolo " & _
                    " INNER JOIN EntiFasi ef on efr.IdEnteFase = ef.IdEnteFase " & _
                    "	LEFT JOIN (SELECT CodiceFiscale AS CodFis, b.idruolo " & _
                    "               FROM entepersonaleruoli b  " & _
                    "                INNER JOIN entepersonale c ON b.IDEntePersonale = c.IDEntePersonale " & _
                    "               WHERE b.Accreditato = -1) AS Storico ON a.CodiceFiscale = Storico.CodFis and c.idruolo = Storico.idruolo " & _
                    " WHERE c.datafinevalidità is null and c.accreditato=0 and statienti.istruttoria=1 and d.ruoloaccreditamento=1 and  d.Nascosto = 0  and ef.Stato =3"

        '                    " '<img src=images/info_xsmall.jpg title=""Esito"" style=""cursor: hand"" onclick=""javascript: Info(' + convert(varchar,d.idruolo) + ',' + convert(varchar,a.identepersonale) + ')"" border=0>' As InfoRespinto " & _

        'imposto eventuali parametri
        If txtdenomina.Text <> "" Then
            strquery = strquery & " and b.denominazione like '" & ClsServer.NoApice(txtdenomina.Text) & "%'"
        End If
        If txtCodiceEnte.Text <> "" Then
            strquery = strquery & " and b.CodiceRegione = '" & ClsServer.NoApice(txtCodiceEnte.Text) & "'"
        End If
        If txtNome.Text <> "" Then
            strquery = strquery & " and a.nome like '" & ClsServer.NoApice(txtNome.Text) & "%'"
        End If

        If txtCognome.Text <> "" Then
            strquery = strquery & " and a.cognome like '" & ClsServer.NoApice(txtCognome.Text) & "%'"
        End If
        If ddlRuolo.SelectedValue <> 0 Then
            strquery = strquery & " and d.idruolo=" & ddlRuolo.SelectedValue & ""
        End If
        'Aggiunto da Alessandra Taballione il 20/06/2005
        'implementazione dei filtri di ricerca
        If ddlTipoRicerca.SelectedItem.Text = "Nuovi Accreditamenti" Then
            strquery = strquery & " and ca.DefaultClasse=1"
        End If
        If ddlTipoRicerca.SelectedItem.Text = "Adeguamenti" Then
            strquery = strquery & " and (ca.defaultClasse <> 1  and ca.minSedi > 0) "
        End If
        If txtIDFaseEnte.Text <> "" Then
            strquery = strquery & " and EF.IdEnteFase = '" & txtIDFaseEnte.Text & "'"
        End If
        ''filtro per le regioni
        If Session("TipoUtente") = "R" Then
            'preparo la query
            strsqlReg = "select b.IdRegioneCompetenza from RegioniCompetenze a "
            strsqlReg = strsqlReg & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
            strsqlReg = strsqlReg & "where b.username = '" & Session("Utente") & "'"
            'chiudo il datareader se aperto
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
            'eseguo la query
            dtrCompetenze = ClsServer.CreaDatareader(strsqlReg, Session("conn"))

            If dtrCompetenze.HasRows = True Then
                dtrCompetenze.Read()
                'If dtrCompetenze("Heliosread") = True Then
                strquery = strquery & " And b.IdRegioneCompetenza = " & dtrCompetenze("IdRegioneCompetenza") & " "
                'End If
            End If

            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End If

        strquery = strquery & " and a.datafinevalidità is null order by year(c.datainiziovalidità), MONTH(c.datainiziovalidità), DAY(c.datainiziovalidità), b.denominazione, a.cognome"
        mydataset = ClsServer.DataSetGenerico(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtgRisultatoRicerca.DataSource = mydataset

        CaricaDataTablePerStampa(mydataset)

        dtgRisultatoRicerca.DataBind()
        'controllo risulato ricerca
        If dtgRisultatoRicerca.Items.Count = 0 Then
            dtgRisultatoRicerca.Visible = False
            CmdEsportaRisorse.Visible = False
            lblRisorse.Visible = False
        Else
            dtgRisultatoRicerca.Visible = True
            CmdEsportaRisorse.Visible = True
            lblRisorse.Visible = True
        End If

    End Sub

    Sub RicercaServizi()
        'datareader che conterrà l'id regione compentenza
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader
        'per le regioni
        Dim strsqlReg As String

        Dim Strsql As String
        Dim dtsServizi As DataSet
        '<img src=images/canc.jpg title='Cancella' border=0>
        Strsql = "SELECT sistemi.idsistema,"
        Strsql = Strsql & " sistemi.sistema,"
        Strsql = Strsql & " Entefor.denominazione,"
        Strsql = Strsql & " Entefor.Codiceregione,"
        Strsql = Strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato, "
        Strsql = Strsql & " '<img src=images/mini_conferma.jpg title=""Conferma"" style=""cursor: hand"" onclick=""javascript: ConfermaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' as Conferma, "
        Strsql = Strsql & " '<img src=images/canc.jpg title=""Respingi"" style=""cursor: hand"" onclick=""javascript: RespingiServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' as Respingi, "
        Strsql = Strsql & " EntiAcquisizioneServizi.IdEnteAcquisizione,"
        Strsql = Strsql & " Enteric.codiceregione as codreg,"
        Strsql = Strsql & " Enteric.denominazione as denominazioneentesecondario,EF.IdEnteFase"
        Strsql = Strsql & " FROM entisistemi "
        Strsql = Strsql & " inner join sistemi on sistemi.idsistema=entisistemi.idsistema "
        Strsql = Strsql & " inner join Enti entefor on entefor.idente=entisistemi.idente "
        Strsql = Strsql & " INNER Join "
        Strsql = Strsql & " EntiAcquisizioneServizi ON entisistemi.IDEnteSistema=EntiAcquisizioneServizi.idEnteSistema "
        Strsql = Strsql & " INNER Join "
        Strsql = Strsql & " Enti as enteric ON enteric.IDEnte=EntiAcquisizioneServizi.idEntesecondario "
        Strsql = Strsql & " INNER Join "
        Strsql = Strsql & " statiEnti ON enteric.IDstatoEnte=statienti.idstatoente "
        Strsql = Strsql & " INNER JOIN EntiFasi_ServiziAcquisiti EFSA ON EFSA.idEnteAcquisizione =EntiAcquisizioneServizi.idEnteAcquisizione "
        Strsql = Strsql & " INNER JOIN EntiFasi EF ON EF.IdEnteFase = EFSA.IDEnteFase"
        Strsql = Strsql & " WHERE Sistemi.Nascosto=0 and NOT EntiAcquisizioneServizi.IdEnteSistema IS NULL and StatoRichiesta =0 "
        Strsql = Strsql & " and statienti.istruttoria=1 "

        'imposto eventuali parametri
        If txtdenomina.Text <> "" Then
            Strsql = Strsql & " and enteric.denominazione like '" & ClsServer.NoApice(txtdenomina.Text) & "%'"
        End If
        If txtCodiceEnte.Text <> "" Then
            Strsql = Strsql & " and enteric.CodiceRegione = '" & ClsServer.NoApice(txtCodiceEnte.Text) & "'"
        End If
        If txtIDFaseEnte.Text <> "" Then
            Strsql = Strsql & " and EF.IdEnteFase = '" & txtIDFaseEnte.Text & "'"
        End If
        ''filtro per le regioni
        If Session("TipoUtente") = "R" Then
            'preparo la query
            strsqlReg = "select b.IdRegioneCompetenza from RegioniCompetenze a "
            strsqlReg = strsqlReg & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
            strsqlReg = strsqlReg & "where b.username = '" & Session("Utente") & "'"
            'chiudo il datareader se aperto
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
            'eseguo la query
            dtrCompetenze = ClsServer.CreaDatareader(strsqlReg, Session("conn"))

            If dtrCompetenze.HasRows = True Then
                dtrCompetenze.Read()
                'If dtrCompetenze("Heliosread") = True Then
                Strsql = Strsql & " And enteric.IdRegioneCompetenza = " & dtrCompetenze("IdRegioneCompetenza") & " "
                'End If
            End If

            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End If

        Strsql = Strsql & " UNION "
        Strsql = Strsql & " SELECT 0,"
        Strsql = Strsql & " 'Formazione' as sistema,"
        Strsql = Strsql & " 'Regione' as denominazione,"
        Strsql = Strsql & " 'Regione' as Codiceregione,"
        Strsql = Strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato, "
        'Strsql = Strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then '<img src=images/canc.jpg title=""Cancella"" style=""cursor: hand"" onclick=""javascript: CancellaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' end as Elimina "
        Strsql = Strsql & " '<img src=images/mini_conferma.jpg title=""Conferma"" style=""cursor: hand"" onclick=""javascript: ConfermaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' as Conferma, "
        Strsql = Strsql & " '<img src=images/canc.jpg title=""Respingi"" style=""cursor: hand"" onclick=""javascript: RespingiServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' as Respingi, "
        Strsql = Strsql & " EntiAcquisizioneServizi.IdEnteAcquisizione,"
        Strsql = Strsql & " Enti.codiceregione as codreg,"
        Strsql = Strsql & " Enti.denominazione as denominazioneentesecondario,EF.IdEnteFase"
        Strsql = Strsql & " FROM Enti "
        Strsql = Strsql & " INNER Join "
        Strsql = Strsql & " EntiAcquisizioneServizi ON Enti.IDEnte = EntiAcquisizioneServizi.IdEnteSecondario "
        Strsql = Strsql & " INNER Join "
        Strsql = Strsql & " statiEnti ON enti.IDstatoEnte=statienti.idstatoente "
        Strsql = Strsql & " INNER JOIN EntiFasi_ServiziAcquisiti EFSA ON EFSA.idEnteAcquisizione =EntiAcquisizioneServizi.idEnteAcquisizione "
        Strsql = Strsql & " INNER JOIN EntiFasi EF ON EF.IdEnteFase = EFSA.IDEnteFase"
        Strsql = Strsql & " WHERE EntiAcquisizioneServizi.IdEnteSistema IS NULL and StatoRichiesta =0 and statienti.istruttoria=1 "

        'imposto eventuali parametri
        If txtdenomina.Text <> "" Then
            Strsql = Strsql & " and enti.denominazione like '" & ClsServer.NoApice(txtdenomina.Text) & "%'"
        End If
        If txtCodiceEnte.Text <> "" Then
            Strsql = Strsql & " and enti.CodiceRegione = '" & ClsServer.NoApice(txtCodiceEnte.Text) & "'"
        End If
        If txtIDFaseEnte.Text <> "" Then
            Strsql = Strsql & " and EF.IdEnteFase = '" & txtIDFaseEnte.Text & "'"
        End If
        ''filtro per le regioni
        If Session("TipoUtente") = "R" Then
            'preparo la query
            strsqlReg = "select b.IdRegioneCompetenza from RegioniCompetenze a "
            strsqlReg = strsqlReg & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
            strsqlReg = strsqlReg & "where b.username = '" & Session("Utente") & "'"
            'chiudo il datareader se aperto
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
            'eseguo la query
            dtrCompetenze = ClsServer.CreaDatareader(strsqlReg, Session("conn"))

            If dtrCompetenze.HasRows = True Then
                dtrCompetenze.Read()
                'If dtrCompetenze("Heliosread") = True Then
                Strsql = Strsql & " And enti.IdRegioneCompetenza = " & dtrCompetenze("IdRegioneCompetenza") & " "
                'End If
            End If

            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End If

        dtsServizi = ClsServer.DataSetGenerico(Strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dgServizi.DataSource = dtsServizi
        dgServizi.DataBind()

        CaricaDataTablePerStampa1(dtsServizi)

        If dgServizi.Items.Count = 0 Then
            dgServizi.Visible = False
            CmdEsportaServizi.Visible = False
            lblServizi.Visible = False
        Else
            dgServizi.Visible = True
            CmdEsportaServizi.Visible = True
            lblServizi.Visible = True
        End If
    End Sub

    'Sub per l'accreditamento positivo del ruolo
    Sub ConfermaRuolo(ByVal strMyIdRuolo As String, ByVal strMyIdEntePersonale As String)
        Dim CmdInsCronologia As Data.SqlClient.SqlCommand
        Dim intidentepersonaleruolo As Int32
        Dim intValoreAccredito As Int32
        Dim bytForzaturaRuolo, bytFRStorico As Byte
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        'verifico se è stato già accreditato
        dtrgenerico = ClsServer.CreaDatareader("select identepersonaleruolo," & _
        " accreditato,forzatura from entepersonaleruoli where" & _
        " idruolo=" & strMyIdRuolo & " and" & _
        " identepersonale=" & strMyIdEntePersonale & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()
        If IsDBNull(dtrgenerico.GetValue(0)) = False Then
            If dtrgenerico.GetValue(1) <> 1 Then
                intidentepersonaleruolo = dtrgenerico.GetValue(0)
                intValoreAccredito = dtrgenerico.GetValue(1)
                bytFRStorico = dtrgenerico.GetValue(2)
                bytForzaturaRuolo = 0
                dtrgenerico.Close()
                dtrgenerico = Nothing 'popolo cronologia
                CmdInsCronologia = New SqlClient.SqlCommand("insert into" & _
                " cronologiaentepersonaleruoli" & _
                " (identepersonaleruolo,accreditato,datacronologia," & _
                " idtipocronologia,usernameaccreditatore,forzatura)" & _
                " values (" & intidentepersonaleruolo & "," & intValoreAccredito & ",getdate(),0," & _
                " '" & ClsServer.NoApice(Session("Utente")) & "'," & bytFRStorico & ")", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                CmdInsCronologia.ExecuteNonQuery()
                CmdInsCronologia.Dispose()
                Dim ComUpdRuoli As Data.SqlClient.SqlCommand 'eseguo comando di modifica su entepersonaleruoli
                ComUpdRuoli = New SqlClient.SqlCommand("update entepersonaleruoli" & _
                " set accreditato=1,dataaccreditamento=getdate()," & _
                " usernameaccreditatore='" & ClsServer.NoApice(Session("Utente")) & "',forzatura=" & bytForzaturaRuolo & "" & _
                " where idruolo=" & strMyIdRuolo & " and identepersonale=" & strMyIdEntePersonale & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                ComUpdRuoli.ExecuteNonQuery()
                ComUpdRuoli.Dispose()
            Else
                dtrgenerico.Close() 'chiudo
                dtrgenerico = Nothing
            End If
        Else 'se non è possibile accreditare chiudo solamente dataREader
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    'Sub per l'accreditamento negativo del ruolo
    Sub RespingiRuolo(ByVal strMyIdRuolo As String, ByVal strMyIdEntePersonale As String)
        Dim intidentepersonaleruolo As Int64
        Dim bytForzaturaRuolo As Byte
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        'verifico se è possibile disaccreditare verificando solamente se sia già disaccreditato
        dtrgenerico = ClsServer.CreaDatareader("select identepersonaleruolo," & _
        " accreditato,forzatura from entepersonaleruoli where" & _
        " idruolo=" & strMyIdRuolo & " and" & _
        " identepersonale=" & strMyIdEntePersonale & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            If dtrgenerico.GetValue(1) = -1 Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
                Exit Sub
            Else
                intidentepersonaleruolo = dtrgenerico.GetValue(0)
                bytForzaturaRuolo = dtrgenerico.GetValue(2)
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Dim cmdInsCronologia As Data.SqlClient.SqlCommand
            cmdInsCronologia = New SqlClient.SqlCommand("insert into cronologiaentepersonaleruoli" & _
            " (identepersonaleruolo,accreditato,datacronologia,idtipocronologia," & _
            " usernameaccreditatore,forzatura)" & _
            " select identepersonaleruolo,accreditato,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'," & bytForzaturaRuolo & "" & _
            " from entepersonaleruoli where identepersonaleruolo=" & intidentepersonaleruolo & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            cmdInsCronologia.ExecuteNonQuery()
            cmdInsCronologia.Dispose()
            Dim ComUpdRuoli As Data.SqlClient.SqlCommand 'eseguo comando di modifica
            ComUpdRuoli = New SqlClient.SqlCommand("update entepersonaleruoli set accreditato=-1,dataaccreditamento=getdate(),usernameaccreditatore='" & ClsServer.NoApice(Session("Utente")) & "'" & _
            " where idruolo=" & strMyIdRuolo & " and identepersonale=" & strMyIdEntePersonale & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            ComUpdRuoli.ExecuteNonQuery()
            ComUpdRuoli.Dispose()

        Else 'se non è possibile accreditare chiudo solamente dataREader
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(4) As String
        Dim NomiCampiColonne(4) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Nominativo"
        NomeColonne(1) = "Ruolo"
        NomeColonne(2) = "Cod. Ente"
        NomeColonne(3) = "Ente"
        NomeColonne(4) = "Data Inserimento"

        NomiCampiColonne(0) = "Nominativo"
        NomiCampiColonne(1) = "Ruolo"
        NomiCampiColonne(2) = "Codiceregione"
        NomiCampiColonne(3) = "denominazione"
        NomiCampiColonne(4) = "Data"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 4
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 4
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa1(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(5) As String
        Dim NomiCampiColonne(5) As String

        'nome della colonna 
        'e posizione nella griglia di lettura

        NomeColonne(0) = "Ente Richiedente"
        NomeColonne(1) = "Codice Ente Richiedente"
        NomeColonne(2) = "Servizi Acquisiti"
        NomeColonne(3) = "Ente Fornitore"
        NomeColonne(4) = "Codice Ente Fornitore"
        NomeColonne(5) = "Stato"

        NomiCampiColonne(0) = "denominazioneentesecondario"
        NomiCampiColonne(1) = "CodReg"
        NomiCampiColonne(2) = "Sistema"
        NomiCampiColonne(3) = "denominazione"
        NomiCampiColonne(4) = "Codiceregione"
        NomiCampiColonne(5) = "Stato"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 5
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 5
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca1") = dt

    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        '***Generata da Gianluigi Paesani in data:09/06/04
        '***salvo parametri per link 
        txtdenomina1.Value = txtdenomina.Text
        txtCognome1.Value = txtCognome.Text
        Txtnome1.Value = txtNome.Text
        dtgRisultatoRicerca.CurrentPageIndex = 0
        dgServizi.CurrentPageIndex = 0

        ApriCSV1Risorse.Visible = False
        ApriCSV1Servizi.Visible = False

        AssegnaCookies()
        'richiamo sub 
        PopolaGriglia(0)
        RicercaServizi()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        '***Generata da Gianluigi Paesani in data:09/06/04
        '***chiamo homepage
        Response.Redirect("wfrmMain.aspx")
    End Sub

    Private Sub dtgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand

        Dim idRuolo As String
        Dim idEntePersonale As String

        If e.CommandName = "Conferma" Then
            idRuolo = e.Item.Cells(12).Text
            idEntePersonale = e.Item.Cells(3).Text
            Response.Redirect("WfrmRisorsedaAccreditare.aspx?Accetta=si&strIDRuolo=" & idRuolo & "&strIdEntePersonale=" & idEntePersonale)
        End If

        If e.CommandName = "Respingi" Then
            idRuolo = e.Item.Cells(12).Text
            idEntePersonale = e.Item.Cells(3).Text
            Response.Redirect("WfrmRisorsedaAccreditare.aspx?Accetta=no&strIDRuolo=" & idRuolo & "&strIdEntePersonale=" & idEntePersonale)
        End If

        If e.CommandName = "InfoRespinto" Then
            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("window.open ('AccreditamentiRisorseNegativi.aspx?IDEntePersonale=" & e.Item.Cells(3).Text & "','info','height=500,Width=1000,menubar=no,scrollbars=yes,resizable=yes')")
            Response.Write("</SCRIPT>")
        End If

    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        '***Generata da Gianluigi Paesani in data:09/06/04
        '***
        txtdenomina.Text = txtdenomina1.Value
        txtCognome.Text = txtCognome1.Value
        txtNome.Text = Txtnome1.Value
        'passo valore pagina
        PopolaGriglia(1, e.NewPageIndex)
    End Sub

    Private Sub dgServizi_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgServizi.ItemCommand

        Dim IdEnteAcquisizione As String

        If e.CommandName = "Conferma" Then
            IdEnteAcquisizione = e.Item.Cells(10).Text
            Response.Redirect("WfrmRisorsedaAccreditare.aspx?Provenienza=Conferma&strIDEnteAcquisizione=" & IdEnteAcquisizione)
        End If

        If e.CommandName = "Respingi" Then
            IdEnteAcquisizione = e.Item.Cells(10).Text
            Response.Redirect("WfrmRisorsedaAccreditare.aspx?Provenienza=Respingi&strIDEnteAcquisizione=" & IdEnteAcquisizione)
        End If

    End Sub

    Private Sub dgServizi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgServizi.SelectedIndexChanged
        'Apro la maschera di dettaglio dei Servizi acquisiti
        If dgServizi.SelectedItem.Cells(1).Text <> "0" Then
            Response.Write("<script>")
            Response.Write("window.open('ricercaentepersonaleacquisibile.aspx?IdSist=" & dgServizi.SelectedItem.Cells(1).Text & "&CodEnte=" & dgServizi.SelectedItem.Cells(6).Text & "','ricerca','height=450,width=450,dependent=yes,scrollbars=yes,status=no,resizable=no')")
            Response.Write("</script>")
        End If
    End Sub

    Private Sub dgServizi_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgServizi.PageIndexChanged
        dgServizi.SelectedIndex = -1
        dgServizi.EditItemIndex = -1
        dgServizi.CurrentPageIndex = e.NewPageIndex
        RicercaServizi()
    End Sub

    Private Sub CmdEsportaRisorse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsportaRisorse.Click
        CmdEsportaRisorse.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSVRisorse(dtbRicerca)
    End Sub

    Private Sub StampaCSVRisorse(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1Risorse.Visible = False
            CmdEsportaRisorse.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1Risorse.Visible = True
            ApriCSV1Risorse.NavigateUrl = url
        End If

    End Sub

    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim Reader As StreamReader
        Dim url As String
        NomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        Writer = New StreamWriter(mapPath & "\" & NomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        Writer.WriteLine(xLinea)
        xLinea = vbNullString

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
        url = "download\" & NomeUnivoco & ".CSV"

        Writer.Close()
        Writer = Nothing
        Return url
    End Function

    Private Sub CmdEsportaServizi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsportaServizi.Click
        CmdEsportaServizi.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca1")
        StampaCSVServizi(dtbRicerca)
    End Sub

    Private Sub StampaCSVServizi(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1Servizi.Visible = False
            CmdEsportaServizi.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1Servizi.Visible = True
            ApriCSV1Servizi.NavigateUrl = url
        End If

    End Sub
    Sub AssegnaCookies()
        'CREATA IL 28/12/2016 ADC
        Response.Cookies("InfoRif")("txtCodiceEnte") = txtCodiceEnte.Text
        Response.Cookies("InfoRif")("txtIDFaseEnte") = txtIDFaseEnte.Text
        Response.Cookies("InfoRif").Expires = DateTime.Now.AddDays(1)
    End Sub
    Sub RecuperaCookie()
        'CREATA IL 28/12/2016 ADC
        If Not Server.HtmlDecode(Request.Cookies("InfoRif")("txtCodiceEnte")) Is Nothing Then
            txtCodiceEnte.Text = Server.HtmlDecode(Request.Cookies("InfoRif")("txtCodiceEnte")).ToString

        End If
        If Not Server.HtmlDecode(Request.Cookies("InfoRif")("txtIDFaseEnte")) Is Nothing Then
            txtIDFaseEnte.Text = Server.HtmlDecode(Request.Cookies("InfoRif")("txtIDFaseEnte")).ToString
        End If

    End Sub
End Class