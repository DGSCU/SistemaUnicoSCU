Imports System.Data.SqlClient
Imports System.IO

Public Class ver_RicercaVerifichePerRequisiti
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    Dim DataSetRicerca As DataSet
    Public Sub TuttaPaginaSess()
        Session("TP") = True
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Call TuttaPaginaSess()
        If Not IsPostBack Then
            If Not Session("LogIn") Is Nothing Then
                If Session("LogIn") = False Then 'verifico validità log-in
                    Response.Redirect("LogOn.aspx")
                End If
            Else
                Response.Redirect("LogOn.aspx")
            End If
            If IsPostBack = False Then

                CaricaCombo()
                TrovaCompetenzaUtente()
                CaricaCompetenze()
                CaricaProgrammazione()
                CaricaVerificatore()

                ddlTipologiaVerifica.Items.Add("")
                ddlTipologiaVerifica.Items(0).Value = 0
                ddlTipologiaVerifica.Items.Add("Programmata")
                ddlTipologiaVerifica.Items(1).Value = 1
                ddlTipologiaVerifica.Items.Add("Segnalata")
                ddlTipologiaVerifica.Items(2).Value = 2
                ' If Session("TipoUtente") = "U" Then
                ddlTipologiaVerifica.Items.Add("Segnalata DGSCN")
                ddlTipologiaVerifica.Items(3).Value = 3
                'End If
                If Session("VengoDa") = "VerificaRequisiti" Then
                    EseguiRicerca()
                End If
            End If
        End If
    End Sub

    Private Function VerificaParametri() As Boolean

        VerificaParametri = False
        Dim data As Date
        Dim data1 As Date
        If txtDataDalPrevista.Text <> "" Then
            If Not Date.TryParse(txtDataDalPrevista.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prevista Verifica Dal deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataAlPrevista.Text <> "" Then
            If Not Date.TryParse(txtDataAlPrevista.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prevista Verifica Al deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataDalPrevista.Text <> "" And txtDataAlPrevista.Text <> "" Then
            ' verifico dal < al
            data = Convert.ToDateTime(txtDataDalPrevista.Text)
            data1 = Convert.ToDateTime(txtDataAlPrevista.Text)
            If (data > data1) Then

                lblErrore.Text = " La data prevista verifica DAL deve essere minore di quella AL. "
                Exit Function
            End If
        End If

        If txtDataDalInizio.Text <> "" Then
            If Not Date.TryParse(txtDataDalInizio.Text, data) Then
                lblErrore.Text = "  Il formato della Data Inizio Verifica Dal deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataAlInizio.Text <> "" Then
            If Not Date.TryParse(txtDataAlInizio.Text, data) Then
                lblErrore.Text = "  Il formato della Data Inizio Verifica Al deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataDalInizio.Text <> "" And txtDataAlInizio.Text <> "" Then

            ' verifico dal < al
            data = Convert.ToDateTime(txtDataDalInizio.Text)
            data1 = Convert.ToDateTime(txtDataAlInizio.Text)
            If (data > data1) Then

                lblErrore.Text = " La data inizio prevista DAL deve essere minore di quella AL."
                Exit Function
            End If
        End If

        If TxtInizioPog.Text <> "" Then
            If Not Date.TryParse(TxtInizioPog.Text, data) Then
                lblErrore.Text = "  Il formato della Data Inizio Progetti deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If TxtFinePog.Text <> "" Then
            If Not Date.TryParse(TxtFinePog.Text, data) Then
                lblErrore.Text = "  Il formato della Data Fine Progetti deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If

        VerificaParametri = True


    End Function


    Private Sub EseguiRicerca()
        Dim strSql As String
        Dim strWhere As String


        lblErrore.Text = ""  'Reset Errore




        'If Request.QueryString("VengoDa") = "VerificaRequisiti" Then
        If Session("VengoDa") = "VerificaRequisiti" Then
            strSql = Session("QUERY")
            Session("VengoDa") = ""
        Else
            strSql = "select distinct IdVerifica, CodiceFascicolo, StatoVerifiche,programmazione, " & _
                    " dbo.FormatoData(DataPrevistaVerifica) as DataPrevistaVerifica, dbo.FormatoData(DataFinePrevistaVerifica) as DataFinePrevistaVerifica, dbo.FormatoData(DataFineVerifica) as DataFineVerifica,tipoverificatore, " & _
                    " (Nominativo + '(' + case when tipoverificatore = 0 then 'Interno' when tipoverificatore = 1 then 'IGF' END + ')') as Nominativo, " & _
                    " (Denominazione + '(' + CodiceEnte + ')') as Denominazione, " & _
                    " (Titolo + '(' + CodiceProgetto + ')') as Titolo, dbo.FormatoData(DataInizioAttività) as DataInizioAttività, dbo.FormatoData(DataFineAttività) as DataFineAttività,  " & _
                    " case when tipologia = 1 then 'Programmata' when tipologia = 2 then 'Segnalata' when tipologia = 3 then 'Segnalata DGSCN' end as TipoVerifica,idprogrammazione,idente " & _
                    " from ver_vw_ricerca_verifiche  "
            '(EnteFiglio + '(' + convert(varchar,IDEnteSedeAttuazione)  + ')') As EnteFiglio, (Comune + '(' + DescrAbb + ')') as Comune, Regione,
            'where 1=1
            strWhere = "WHERE "


            If (VerificaParametri() = False) Then Exit Sub





            'If ddlCompetenza.SelectedValue <> 0 Then
            '    strSql = strSql & strWhere & " IdRegCompetenza ='" & ddlCompetenza.SelectedValue & "'"
            '    strWhere = "AND "
            'End If
            If ddlCompetenza.SelectedValue <> "" Then
                Select Case ddlCompetenza.SelectedValue
                    Case 0
                        strSql = strSql & ""
                    Case -1
                        strSql = strSql & strWhere & " IdRegCompetenza = 22 "
                        strWhere = "AND "
                    Case -2
                        strSql = strSql & strWhere & " IdRegCompetenza <> 22 And not IdRegCompetenza is null "
                        strWhere = "AND "
                    Case -3
                        strSql = strSql & strWhere & " IdRegCompetenza is null "
                        strWhere = "AND "
                    Case Else
                        strSql = strSql & strWhere & " IdRegCompetenza = '" & ddlCompetenza.SelectedValue & "'"
                        strWhere = "AND "
                End Select
            End If

            If txtCodiceFascicolo.Text <> "" Then
                strSql = strSql & strWhere & " CodiceFascicolo like '%" & Replace(txtCodiceFascicolo.Text, " '", "''") & "%'"
                strWhere = "AND "
            End If

            If ddlStatoVerifica.SelectedValue = 0 Then
                strSql = strSql & strWhere & " GestVerifiche = 1"
                strWhere = "AND "
            Else
                strSql = strSql & strWhere & " IDStatoVerifica = '" & ddlStatoVerifica.SelectedValue & "'"
                strWhere = "AND "
            End If
            If ddlVerificatoreInterno.SelectedValue <> 0 Then
                strSql = strSql & strWhere & " idverificatore = '" & ddlVerificatoreInterno.SelectedValue & "'"
                strWhere = "AND "
            End If
            'If ddlVerificatoreIGF.SelectedValue <> 0 Then
            'strSql = strSql & strWhere & " idIGF = '" & ddlVerificatoreIGF.SelectedValue & "'"
            'strWhere = "AND "
            'End If
            If ddlProgrammazione.SelectedValue <> "" Then
                If ddlProgrammazione.SelectedValue <> 9999 Then
                    strSql = strSql & strWhere & " idprogrammazione = '" & ddlProgrammazione.SelectedValue & "'"
                    strWhere = "AND "
                End If
            End If
            If ddlTipologiaVerifica.SelectedValue <> 0 Then
                strSql = strSql & strWhere & " tipologia = '" & ddlTipologiaVerifica.SelectedValue & "'"
                strWhere = "AND "
            End If
            If txtDataDalPrevista.Text <> "" And txtDataAlPrevista.Text <> "" Then
                strSql = strSql & strWhere & " DataPrevistaVerifica between '" & txtDataDalPrevista.Text & "' and '" & txtDataAlPrevista.Text & "'"
                strWhere = "AND "
            Else
                If txtDataDalPrevista.Text <> "" Then
                    strSql = strSql & strWhere & " DataPrevistaVerifica >= '" & txtDataDalPrevista.Text & "'"
                    strWhere = "AND "
                End If
                If txtDataAlPrevista.Text <> "" Then
                    strSql = strSql & strWhere & " DataPrevistaVerifica <= '" & txtDataAlPrevista.Text & "'"
                    strWhere = "AND "
                End If
            End If
            If txtDataDalInizio.Text <> "" And txtDataAlInizio.Text <> "" Then
                strSql = strSql & strWhere & " DataInizioVerifica between '" & txtDataDalInizio.Text & "' and '" & txtDataAlInizio.Text & "'"
                strWhere = "AND "
            Else
                If txtDataDalInizio.Text <> "" Then
                    strSql = strSql & strWhere & " DataInizioVerifica >= '" & txtDataDalInizio.Text & "'"
                    strWhere = "AND "
                End If
                If txtDataAlInizio.Text <> "" Then
                    strSql = strSql & strWhere & " DataInizioVerifica <= '" & txtDataAlInizio.Text & "'"
                    strWhere = "AND "
                End If
            End If
            If TxtCodPog.Text.Trim <> "" Then
                strSql = strSql & strWhere & " codiceprogetto like '" & TxtCodPog.Text & "%'"
                strWhere = "AND "
            End If
            If TxtDescPog.Text.Trim <> "" Then
                strSql = strSql & strWhere & " titolo like '" & Replace(TxtDescPog.Text, "'", "''") & "%'"
                strWhere = "AND "
            End If
            If DdlBando.SelectedValue > 0 Then
                strSql = strSql & strWhere & " idbando = " & DdlBando.SelectedValue
                strWhere = "AND "
            End If
            If ddlMaccCodAmAtt.SelectedValue > 0 Then
                strSql = strSql & strWhere & " idmacroambitoattività = " & ddlMaccCodAmAtt.SelectedValue
                strWhere = "AND "
            End If
            If ddlCodAmAtt.SelectedValue <> "" Then
                strSql = strSql & strWhere & " idambitoattività = " & ddlCodAmAtt.SelectedValue
                strWhere = "AND "
            End If
            If TxtCodEnte.Text.Trim <> "" Then
                strSql = strSql & strWhere & " codiceente = '" & TxtCodEnte.Text & "'"
                strWhere = "AND "
            End If
            If TxtDescrEnte.Text.Trim <> "" Then
                strSql = strSql & strWhere & " denominazione like '" & Replace(TxtDescrEnte.Text, "'", "''") & "%'"
                strWhere = "AND "
            End If
            If ddlClasse.SelectedValue <> "0" Then
                strSql = strSql & strWhere & " idclasseaccreditamento = " & ddlClasse.SelectedValue & ""
                strWhere = "AND "
            End If
            If ddlTipologia.SelectedValue <> "0" Then
                strSql = strSql & strWhere & " TipologiaEnte = '" & ddlTipologia.SelectedItem.Text & "'"
                strWhere = "AND "
            End If

            If TxtComune.Text.Trim <> "" Then
                strSql = strSql & strWhere & " comune like '" & Replace(TxtComune.Text, "'", "''") & "%'"
                strWhere = "AND "
            End If
            If TxtProvincia.Text.Trim <> "" Then
                strSql = strSql & strWhere & " provincia like '" & Replace(TxtProvincia.Text, "'", "''") & "%'"
                strWhere = "AND "
            End If
            If TxtRegione.Text.Trim <> "" Then
                strSql = strSql & strWhere & " regione like '" & Replace(TxtRegione.Text, "'", "''") & "%'"
                strWhere = "AND "
            End If
            If TxtInizioPog.Text.Trim <> "" Then
                strSql = strSql & strWhere & " datainizioattività = '" & TxtInizioPog.Text & "'"
                strWhere = "AND "
            End If
            If TxtFinePog.Text.Trim <> "" Then
                strSql = strSql & strWhere & " datafineattività = '" & TxtFinePog.Text & "'"
                strWhere = "AND "
            End If
            If TxtAlladata.Text.Trim <> "" Then
                strSql = strSql & strWhere & " datainizioattività <= '" & TxtAlladata.Text & "'"
                strWhere = "AND "
            End If


            ''controllo che se il tipo di utente è MASTER o ISPETTORE blocco o meno le due combo
            ''aggiunto da Jon Cruise il 14.06.2007
            Dim indIdVer As Integer
            indIdVer = ClsUtility.TrovaProfiloUtente(Me.TemplateSourceDirectory, Session("Utente"), Session("Conn"))

            If indIdVer <> 0 Then 'ispettore
                'strWhere = strWhere & " and IdRegioneCompetenza = " & ddlCompetenzaProgetto.SelectedValue
                strSql = strSql & strWhere & " IdVErificatore = '" & indIdVer & "'"
                strWhere = "AND "

            End If
            strSql = strSql & strWhere & " (MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' OR MacroTipoProgetto IS NULL) "
            strWhere = "AND "
            'If strWhere <> "" Then
            '    strSql &= " where " & strWhere
            'End If
            ' strSql = strSql & strWhere & " "
            'strSql = strSql.Replace("where  and", "where ")
            strSql &= " group by IdVerifica, CodiceFascicolo, StatoVerifiche, programmazione,Nominativo, Denominazione, Titolo, DataFineAttività, TipoVerificatore,Tipologia, DataPrevistaVerifica, DataFinePrevistaVerifica, DataInizioAttività, CodiceEnte, CodiceProgetto, DescrAbb, IDEnteSedeAttuazione, DataFineVerifica,idprogrammazione,idente "
            strSql &= " order by TipoVerificatore "
        End If
        Session("QUERY") = strSql

        'EnteFiglio, Comune, Provincia, Regione,
        Session("DataSetRicerca") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicerca)

    End Sub
    Private Sub CaricaDataGrid(ByRef GriddaCaricare As DataGrid)
        'assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = Session("DataSetRicerca")
        dgRisultatoRicerca.DataBind()
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(10) As String
        Dim NomiCampiColonne(10) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Fascicolo"
        NomeColonne(1) = "Stato Verifica"
        NomeColonne(2) = "Tipo Verifica"
        NomeColonne(3) = "Data Inizio Prevista Verifica"
        NomeColonne(4) = "Data Fine Prevista Verifica"
        NomeColonne(5) = "Data Chiusura Verifica"
        NomeColonne(6) = "Verificatore"
        NomeColonne(7) = "Ente Proponente"
        NomeColonne(8) = "Progetto"
        NomeColonne(9) = "Data Inizio Progetto"
        NomeColonne(10) = "Data Fine Progetto"
        'NomeColonne(11) = "Sede Attuazione"
        ''NomeColonne(11) = "Comune"

        NomiCampiColonne(0) = "CodiceFascicolo"
        NomiCampiColonne(1) = "StatoVerifiche"
        NomiCampiColonne(2) = "TipoVerifica"
        NomiCampiColonne(3) = "DataPrevistaVerifica"
        NomiCampiColonne(4) = "DataFinePrevistaVerifica"
        NomiCampiColonne(5) = "DataFineVerifica"
        NomiCampiColonne(6) = "nominativo"
        NomiCampiColonne(7) = "Denominazione"
        NomiCampiColonne(8) = "Titolo"
        NomiCampiColonne(9) = "DataInizioAttività"
        NomiCampiColonne(10) = "DataFineAttività"
        'NomiCampiColonne(11) = "entefiglio"
        'NomiCampiColonne(11) = "comune"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(Session("DataSetRicerca"), 10, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        GriddaCaricare.Visible = True
        If GriddaCaricare.Items.Count = 0 Then
            'GridDaCaricare.Visible = False
            ' lblmessaggio.Text = "Nessun Dato estratto."
            cmdEsporta.Visible = False
        Else
            'lblmessaggio.Text = "Helios - Elenco Enti."
            cmdEsporta.Visible = True
        End If
    End Sub
    ''funzione che controlla il profilo dell'utente loggato
    ''se l'utente loggato è MASTER può usare tutti i filtri di ricerca = TRUE
    ''se l'utente loggato è ISPETTORE gli blocco le combo degli ispettori = FALSE
    'Function TrovaProfiloUtente() As Integer
    '    Dim strLocal As String
    '    Dim dtrLocal As Data.SqlClient.SqlDataReader

    '    strLocal = "SELECT Descrizione FROM Profili "

    '    '============================================================================================================================
    '    '====================================================30/09/2008==============================================================
    '    '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
    '    '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
    '    '============================================================================================================================
    '    If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
    '        strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
    '    Else
    '        strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
    '    End If

    '    strLocal = strLocal & " WHERE AssociaUtenteGruppo.UserName='" & Session("Utente") & "'"

    '    dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))

    '    If dtrLocal.HasRows = True Then
    '        dtrLocal.Read()
    '        '*********************************************************************************************
    '        'MODIFICATO IL 12/11/2008
    '        'ANCHE IL PROFILO REGIONI MASTER DEVE VEDERE COMPORTARSI COME VERFICHE MASTER E COME UNSC MASTER
    '        '*********************************************************************************************
    '        If (dtrLocal("Descrizione") = "VERIFICHE MASTER" Or dtrLocal("Descrizione") = "UNSC MASTER" Or dtrLocal("Descrizione") = "REGIONI MASTER") Then
    '            'TrovaProfiloUtente = True
    '            TrovaProfiloUtente = 0
    '        Else
    '            'TrovaProfiloUtente = False
    '            If Not dtrLocal Is Nothing Then
    '                dtrLocal.Close()
    '                dtrLocal = Nothing
    '            End If
    '            strLocal = "SELECT idverificatore FROM TVerificatori INNER JOIN UtentiUnsc ON TVerificatori.IdUtente=UtentiUnsc.IdUtente WHERE UtentiUnsc.UserName='" & Session("Utente") & "'"
    '            dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))
    '            If dtrLocal.HasRows = True Then
    '                dtrLocal.Read()
    '                TrovaProfiloUtente = CInt(dtrLocal("idverificatore"))
    '            End If
    '            If Not dtrLocal Is Nothing Then
    '                dtrLocal.Close()
    '                dtrLocal = Nothing
    '            End If
    '        End If
    '    End If

    '    If Not dtrLocal Is Nothing Then
    '        dtrLocal.Close()
    '        dtrLocal = Nothing
    '    End If

    '    Return TrovaProfiloUtente

    'End Function

    'funzione che mi trova l'idutente dell'utente loggato
    'aggiunta da Jonah Lomu
    'il 14.06.2007
    Function TrovaIdVerificatore() As Integer
        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader

        strLocal = "SELECT IdVerificatore FROM TVerificatori INNER JOIN UtentiUNSC ON TVerificatori.IdUtente=UtentiUNSC.IdUtente WHERE UtentiUNSC.UserName='" & Session("Utente") & "'"

        dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            TrovaIdVerificatore = dtrLocal("IdVerificatore")
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        Return TrovaIdVerificatore

    End Function

    Sub CaricaCombo()
        Dim strsql As String
        '*****Carico Combo Verificatori Interni


        'strsql = "select IdVerificatore, (Cognome + ' ' + Nome) As Nome " & _
        '         " from TVerificatori WHERE Tipologia=0 AND Abilitato=0 "  
        'If ddlCompetenza.SelectedValue <> 22 Then
        '    strsql = strsql & " and IdRegCompetenza = " & ddlCompetenza.SelectedValue & " "
        'End If
        'ddlVerificatoreInterno.DataSource = MakeParentTable(strsql)
        'ddlVerificatoreInterno.DataTextField = "ParentItem"
        'ddlVerificatoreInterno.DataValueField = "id"
        'ddlVerificatoreInterno.DataBind()


        'If ddlCompetenza.SelectedValue = 22 Then
        '    ddlVerificatoreIGF.Enabled = True
        '    ''controllo che se il tipo di utente è MASTER o ISPETTORE blocco o meno le due combo
        '    ''aggiunto da Jon Cruise il 14.06.2007
        '    If TrovaProfiloUtente() <> 0 Then
        '        'aggiunto il 29/08/2007  da simona cordella
        '        'carico combo IGF a secondo del verificatoreinterno 
        '        '*****Carico Combo Verificatori IGF

        '        strsql = "SELECT V.IDVerificatore, V.Cognome + ' ' + V.Nome AS Nome " & _
        '                " FROM TVerificatori AS V " & _
        '                " INNER JOIN TVerificheAssociaUser AS VU ON V.IDVerificatore = VU.IDVerificatoreIGF " & _
        '                " WHERE  (V.Tipologia = 1) AND (V.Abilitato = 0) AND (V.GenericoIGF = 0) " & _
        '                " and vu.idverificatoreinterno = " & TrovaProfiloUtente() & "  and v.idRegCompetenza=22 "
        '        ddlVerificatoreIGF.DataSource = MakeParentTable(strsql)
        '        ddlVerificatoreIGF.DataTextField = "ParentItem"
        '        ddlVerificatoreIGF.DataValueField = "id"
        '        ddlVerificatoreIGF.DataBind()
        '        'ddlVerificatoreInterno.SelectedValue = TrovaIdVerificatore()
        '        ddlVerificatoreInterno.Enabled = False
        '        If ddlVerificatoreIGF.Items.Count = "1" Then 'riga vuota combo
        '            ddlVerificatoreIGF.Enabled = False
        '        End If

        '    Else
        '        '*****Carico Combo Verificatori IGF
        '        ddlVerificatoreIGF.DataSource = MakeParentTable("select IdVerificatore, (Cognome + ' ' + Nome) As Nome from TVerificatori WHERE Tipologia=1 AND Abilitato=0 and genericoIGF =0 and idRegCompetenza=22")
        '        ddlVerificatoreIGF.DataTextField = "ParentItem"
        '        ddlVerificatoreIGF.DataValueField = "id"
        '        ddlVerificatoreIGF.DataBind()
        '    End If
        'Else
        '    ddlVerificatoreIGF.Enabled = False
        'End If
        ''*****Carico Combo Progtrammazione
        'ddlProgrammazione.DataSource = MakeParentTable("select IdProgrammazione, Descrizione from TVerificheProgrammazione")
        'ddlProgrammazione.DataTextField = "ParentItem"
        'ddlProgrammazione.DataValueField = "id"
        'ddlProgrammazione.DataBind()


        '*****Carico Combo Stati Verificatori
        ddlStatoVerifica.DataSource = MakeParentTable("select IdStatoVerifiche, StatoVerifiche from TVerificheStati WHERE GestVerifiche=1 ")
        ddlStatoVerifica.DataTextField = "ParentItem"
        ddlStatoVerifica.DataValueField = "id"
        ddlStatoVerifica.DataBind()

        '*****Carico Combo Bandi
        'DdlBando.DataSource = MakeParentTable("select idbando, bandobreve from Bando")
        'Mod. il 14/10/2016 da simona cordella con il filtrovisibilità
        
        strsql = "SELECT DISTINCT Bando.idBando,bando.bandobreve,bando.annobreve "
        strsql = strsql & " FROM bando"
        strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql = strsql & " WHERE tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strsql = strsql & " ORDER BY bando.annobreve desc"
        
        DdlBando.DataSource = MakeParentTable(strsql)

        DdlBando.DataTextField = "ParentItem"
        DdlBando.DataValueField = "id"
        DdlBando.DataBind()

        '***Carico combo settore
        ddlMaccCodAmAtt.DataSource = MakeParentTable("select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività")
        ddlMaccCodAmAtt.DataTextField = "ParentItem"
        ddlMaccCodAmAtt.DataValueField = "id"
        ddlMaccCodAmAtt.DataBind()

        '***Carico combo area intervento
        ddlCodAmAtt.Items.Add("")
        ddlCodAmAtt.Enabled = False

        ddlClasse.DataSource = ClsServer.CreaDataTable("Select idclasseAccreditamento,classeAccreditamento from classiaccreditamento ", True, Session("conn"))
        ddlClasse.DataValueField = "idclasseAccreditamento"
        ddlClasse.DataTextField = "classeAccreditamento"
        ddlClasse.DataBind()

        ddlTipologia.DataSource = ClsServer.CreaDataTable("select idTipologieEnti,Descrizione from TipologieEnti", True, Session("conn"))
        ddlTipologia.DataValueField = "idTipologieEnti"
        ddlTipologia.DataTextField = "Descrizione"
        ddlTipologia.DataBind()



    End Sub

    'Sub CaricaCompetenze()
    '    'stringa per la query
    '    Dim strSQL As String
    '    'datareader che conterrà l'id 
    '    Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

    '    Try
    '        'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
    '        If Page.IsPostBack = False Then
    '            'preparo la query

    '            strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
    '            strSQL = strSQL & " union "
    '            strSQL = strSQL & " select '0',' TUTTI ','','A' "
    '            strSQL = strSQL & " union "
    '            strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
    '            strSQL = strSQL & " union "
    '            strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
    '            strSQL = strSQL & " union "
    '            strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
    '            strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
    '            'chiudo il datareader se aperto
    '            If Not dtrCompetenze Is Nothing Then
    '                dtrCompetenze.Close()
    '                dtrCompetenze = Nothing
    '            End If

    '            'eseguo la query
    '            dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
    '            'assegno il datadearder alla combo caricando così descrizione e id
    '            ddlCompetenza.DataSource = dtrCompetenze
    '            ddlCompetenza.Items.Add("")
    '            ddlCompetenza.DataTextField = "Descrizione"
    '            ddlCompetenza.DataValueField = "IDRegioneCompetenza"
    '            ddlCompetenza.DataBind()
    '            If Not dtrCompetenze Is Nothing Then
    '                dtrCompetenze.Close()
    '                dtrCompetenze = Nothing
    '            End If

    '            dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
    '            'ddlCompetenzaProgetto.DataSource = dtrCompetenze
    '            'ddlCompetenzaProgetto.Items.Add("")
    '            'ddlCompetenzaProgetto.DataTextField = "Descrizione"
    '            'ddlCompetenzaProgetto.DataValueField = "IDRegioneCompetenza"
    '            'ddlCompetenzaProgetto.DataBind()
    '            If Not dtrCompetenze Is Nothing Then
    '                dtrCompetenze.Close()
    '                dtrCompetenze = Nothing
    '            End If

    '            'chiudo il datareader se aperto
    '        End If
    '        'Controllo abilitazione scelta
    '        If Session("TipoUtente") = "U" Then
    '            ddlCompetenza.Enabled = True
    '            ddlCompetenza.SelectedIndex = 0

    '        Else
    '            'CboCompetenza.SelectedIndex = 1
    '            ddlCompetenza.Enabled = False
    '            'preparo la query
    '            strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
    '            strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
    '            strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
    '            'chiudo il datareader se aperto
    '            If Not dtrCompetenze Is Nothing Then
    '                dtrCompetenze.Close()
    '                dtrCompetenze = Nothing
    '            End If
    '            'controllo se utente o ente regionale
    '            'eseguo la query
    '            dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
    '            dtrCompetenze.Read()
    '            If dtrCompetenze.HasRows = True Then
    '                ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
    '                If dtrCompetenze("Heliosread") = True Then
    '                    ddlCompetenza.Enabled = True
    '                End If

    '            End If

    '        End If
    '    Catch ex As Exception
    '        Response.Write(ex.Message.ToString())
    '        If Not dtrCompetenze Is Nothing Then
    '            dtrCompetenze.Close()
    '            dtrCompetenze = Nothing
    '        End If
    '    End Try
    '    If Not dtrCompetenze Is Nothing Then
    '        dtrCompetenze.Close()
    '        dtrCompetenze = Nothing
    '    End If
    'End Sub

    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Inizializzo e carico datatable 
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        ' Create a new DataTable.
        Dim myDataTable As DataTable = New DataTable
        ' Declare variables for DataColumn and DataRow objects.
        Dim myDataColumn As DataColumn
        Dim myDataRow As DataRow
        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.Int64")
        myDataColumn.ColumnName = "id"
        myDataColumn.Caption = "id"
        myDataColumn.ReadOnly = True
        myDataColumn.Unique = True
        ' Add the Column to the DataColumnCollection.
        myDataTable.Columns.Add(myDataColumn)
        ' Create second column.
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "ParentItem"
        myDataColumn.AutoIncrement = False
        myDataColumn.Caption = "ParentItem"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False
        ' Add the column to the table.
        myDataTable.Columns.Add(myDataColumn)
        ' Make the ID column the primary key column. da verificare?????????
        'Dim PrimaryKeyColumns(0) As DataColumn
        'PrimaryKeyColumns(0) = myDataTable.Columns("id"))
        'myDataTable.PrimaryKey = PrimaryKeyColumns)
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        'Instantiate the DataSet variable.
        'mydataset = New DataSet
        ' Add the new DataTable to the DataSet.
        'mydataset.Tables.Add(myDataTable)
        myDataRow = myDataTable.NewRow()
        myDataRow("id") = 0
        myDataRow("ParentItem") = ""
        myDataTable.Rows.Add(myDataRow)
        Do While dtrgenerico.Read
            myDataRow = myDataTable.NewRow()
            myDataRow("id") = dtrgenerico.GetValue(0)
            myDataRow("ParentItem") = dtrgenerico.GetValue(1)
            myDataTable.Rows.Add(myDataRow)
        Loop

        dtrgenerico.Close()
        dtrgenerico = Nothing

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "seleziona"
                If e.Item.Cells(4).Text = "Segnalata DGSCN" Then
                    Response.Redirect("ver_VerificaSegnalataUNSC.aspx?StatoSegnalata=Modifica&IdEnte=" & e.Item.Cells(15).Text & "&IdVerifica=" & e.Item.Cells(1).Text)
                Else
                    Response.Redirect("verificarequisiti.aspx?VengoDa=VerificaRequisiti&IdEnte=" & e.Item.Cells(15).Text & "&IdProgrammazione=" & e.Item.Cells(14).Text & "&IdVerifica=" & e.Item.Cells(1).Text)
                End If


        End Select

    End Sub

    Private Sub ddlMaccCodAmAtt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMaccCodAmAtt.SelectedIndexChanged
        If ddlMaccCodAmAtt.SelectedItem.Text <> "" Then
            ddlCodAmAtt.Enabled = True
            ddlCodAmAtt.DataSource = MakeParentTable("select distinct a.idambitoattività," & _
                                                     " a.codifica + ' ' + a.AmbitoAttività as Ambito from ambitiattività a" & _
                                                     " inner join macroambitiattività b" & _
                                                     " on a.IDMacroAmbitoAttività=b.IDMacroAmbitoAttività" & _
                                                     " where a.IDMacroAmbitoAttività=" & ddlMaccCodAmAtt.SelectedValue & " order by 1")
            ddlCodAmAtt.DataTextField = "ParentItem"
            ddlCodAmAtt.DataValueField = "id"
            ddlCodAmAtt.DataBind()
        Else
            'popolo completamente combo aree di intervento
            ddlCodAmAtt.DataSource = Nothing
            ddlCodAmAtt.Items.Add("")
            ddlCodAmAtt.SelectedIndex = 0
            ddlCodAmAtt.Enabled = False
        End If
    End Sub


    Public Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        'Dim dtRicercaVerifiche As New DataSet
        'Dim strSql As String = "select *, case when tipoverificatore = 0 then 'Interno' when tipoverificatore = 1 then 'IGF' END as TipoVerificatoreEsteso, case when tipologia = 1 then 'Programmata' when tipologia = 2 then 'Segnalata' end as TipoVerifica from ver_vw_ricerca_verifiche"

        'dtRicercaVerifiche = ClsServer.DataSetGenerico(strSql, Session("conn"))
        'dgRisultatoRicerca.DataSource = dtRicercaVerifiche
        'dgRisultatoRicerca.DataBind()
        dgRisultatoRicerca.CurrentPageIndex = 0
        EseguiRicerca()
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub


    Public Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click

        Response.Redirect("WfrmMain.aspx")
    End Sub
    Private Sub RedirectPage()
        Response.Redirect("ver_RicercaVerifichePerRequisiti.aspx")
    End Sub
    Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String

        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                'strSQL = " Select IdRegioneCompetenza,case when Descrizione ='Nazionale' then UPPER(Descrizione) ELSE Descrizione end AS Descrizione,CodiceRegioneCompetenza "
                'strSQL = strSQL & " from RegioniCompetenze "
                'strSQL = strSQL & " ORDER BY CASE WHEN left(CodiceRegioneCompetenza,1)='N' then 1 else 2 end,descrizione "

                ''strSQL = strSQL & " union "
                ''trSQL = strSQL & " select '0',' TUTTI ','','A' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '',' NAZIONALE ','','B' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                ''strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "


                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                'strSQL = strSQL & " union "
                'strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                'strSQL = strSQL & "  from RegioniCompetenze "
                strSQL = strSQL & " order by left(CodiceRegioneCompetenza,1),descrizione "


                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlCompetenza.DataSource = dtrCompetenze
                ddlCompetenza.Items.Add("")
                ddlCompetenza.DataTextField = "Descrizione"
                ddlCompetenza.DataValueField = "IDRegioneCompetenza"
                ddlCompetenza.DataBind()

                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'chiudo il datareader se aperto
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenza.Enabled = True
                ddlCompetenza.SelectedIndex = 0
            Else
                'CboCompetenza.SelectedIndex = 1
                ddlCompetenza.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenza.Enabled = True
                    End If
                    'Session("IdRegCompetenza") = ddlCompetenza.SelectedValue
                End If

                If Session("TipoUtente") = "R" Then
                    ddlCompetenza.Enabled = False
                End If

            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub
    Private Sub TrovaCompetenzaUtente()
        'stringa per la query
        Dim strSQL As String

        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
        strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
        strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
        'chiudo il datareader se aperto
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
        'controllo se utente o ente regionale
        'eseguo la query
        dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrCompetenze.Read()
        If dtrCompetenze.HasRows = True Then
            Session("IdRegCompetenza") = dtrCompetenze("IdRegioneCompetenza")
            'If dtrCompetenze("Heliosread") = True Then
            '    ddlCompetenza.Enabled = True
            'End If
        End If
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub
    Private Sub CaricaProgrammazione()
        Dim strsql As String
        Dim dtrProg As SqlClient.SqlDataReader
        If Not dtrProg Is Nothing Then
            dtrProg.Close()
            dtrProg = Nothing
        End If


        strsql = " Select 9999  as idprogrammazione ,'' as Descrizione "

        'strsql = strsql & " union  Select idprogrammazione , tverificheprogrammazione.Descrizione "
        'strsql = strsql & " From tverificheprogrammazione "
        'strsql = strsql & " inner Join RegioniCompetenze on tverificheprogrammazione.IdRegCompetenza = RegioniCompetenze.IdRegioneCompetenza "
        strsql &= " union "
        strsql &= " SELECT p.idprogrammazione , p.descrizione  "
        strsql &= " from tverificheprogrammazione P"
        strsql &= " INNER JOIN bando b ON b.IDBando=p.IdBando"
        strsql &= " INNER join AssociaBandoTipiProgetto abtp on abtp.idbando =  b.idbando"
        strsql &= " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"


        strsql &= " WHERE  tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "


        'If ddlCompetenza.SelectedValue <> 22 Then
        '    strsql = strsql & " Where tverificheprogrammazione.IdRegCompetenza = " & ddlCompetenza.SelectedValue & " "
        'End If

        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case 0
                    strsql = strsql & ""
                Case -1
                    strsql = strsql & " AND p.IdRegCompetenza = 22"
                Case -2
                    strsql = strsql & " AND p.IdRegCompetenza <> 22 And not p.IdRegCompetenza is null "
                Case -3
                    strsql = strsql & " AND p.IdRegCompetenza is null "
                Case Else
                    strsql = strsql & " AND p.IdRegCompetenza = " & ddlCompetenza.SelectedValue
            End Select
        End If



        'strsql = strsql & "  union Select '0' as idprogrammazione ,'' as Descrizione From tverificheprogrammazione   "
        'strsql = strsql & "  inner Join RegioniCompetenze on tverificheprogrammazione.IdRegCompetenza = RegioniCompetenze.IdRegioneCompetenza  "

        ''If ddlCompetenza.SelectedValue <> 22 Then
        ''    strsql = strsql & " Where tverificheprogrammazione.IdRegCompetenza = " & ddlCompetenza.SelectedValue & " "
        ''End If

        'If ddlCompetenza.SelectedValue <> "" Then
        '    Select Case ddlCompetenza.SelectedValue
        '        Case 0
        '            strsql = strsql & ""
        '        Case -1
        '            strsql = strsql & " Where tverificheprogrammazione.IdRegCompetenza = 22"
        '        Case -2
        '            strsql = strsql & " Where tverificheprogrammazione.IdRegCompetenza <> 22 And not tverificheprogrammazione.IdRegCompetenza is null "
        '        Case -3
        '            strsql = strsql & " Where tverificheprogrammazione.IdRegCompetenza is null "
        '        Case Else
        '            strsql = strsql & " Where tverificheprogrammazione.IdRegCompetenza = " & ddlCompetenza.SelectedValue
        '    End Select
        'End If

        strsql = strsql & " Order by idprogrammazione desc "


        dtrProg = ClsServer.CreaDatareader(strsql, Session("conn"))

        ddlProgrammazione.DataSource = dtrProg

        ddlProgrammazione.DataValueField = "idprogrammazione"
        ddlProgrammazione.DataTextField = "descrizione"
        ddlProgrammazione.DataBind()

        'If ddlCompetenza.SelectedValue <> 22 Then
        '    ddlCompetenza.SelectedValue = Session("IdRegCompetenza")
        '    ddlCompetenza.Enabled = False
        'Else
        '    ddlCompetenza.Enabled = True
        '    ddlCompetenza.SelectedIndex = 0
        'End If


        If Not dtrProg Is Nothing Then
            dtrProg.Close()
            dtrProg = Nothing
        End If
        If dgRisultatoRicerca.Items.Count <> 0 Then
            dgRisultatoRicerca.DataSource = Nothing
            dgRisultatoRicerca.DataBind()
        End If

    End Sub

    Private Sub ddlCompetenza_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompetenza.SelectedIndexChanged
        CaricaProgrammazione()
        CaricaVerificatore()
    End Sub

    Private Sub TrovaCompetenzaProgrammazione(ByVal IDProg As Integer)
        Dim strSQL As String
        Dim dtrCompetenze As SqlClient.SqlDataReader

        strSQL = " Select TVerificheProgrammazione.IdRegCompetenza, RegioniCompetenze.Descrizione " & _
                 " from RegioniCompetenze " & _
                 " inner join TVerificheProgrammazione on TVerificheProgrammazione.IdRegCompetenza  = RegioniCompetenze.IdRegioneCompetenza  " & _
                 " Where IDProgrammazione  =" & IDProg & " "
        'chiudo il datareader se aperto
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
        'eseguo la query
        dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
        If dtrCompetenze.HasRows = True Then
            dtrCompetenze.Read()
            Session("IdRegCompetenza") = dtrCompetenze("IdRegCompetenza")
            If dtrCompetenze("IdRegCompetenza") = 22 Then
                ddlCompetenza.SelectedValue = -1
            Else
                ddlCompetenza.SelectedValue = dtrCompetenze("IdRegCompetenza")
            End If
            'If ddlCompetenza.SelectedValue <> "" Then
            '    Select Case ddlCompetenza.SelectedValue
            '        Case 0
            '            strSQL = strSQL & ""
            '        Case -1
            '            strSQL = strSQL & strWhere & " IdRegCompetenza = 22 "
            '            strWhere = "AND "
            '        Case -2
            '            strSQL = strSQL & strWhere & " IdRegCompetenza <> 22 And not IdRegCompetenza is null "
            '            strWhere = "AND "
            '        Case -3
            '            strSQL = strSQL & strWhere & " IdRegCompetenza is null "
            '            strWhere = "AND "
            '        Case Else
            '            strSQL = strSQL & strWhere & " IdRegCompetenza = '" & ddlCompetenza.SelectedValue & "'"
            '            strWhere = "AND "
            '    End Select
            'End If
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
            CaricaVerificatore()
        End If
        'chiudo il datareader se aperto
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub

    Private Sub ddlProgrammazione_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlProgrammazione.SelectedIndexChanged
        TrovaCompetenzaProgrammazione(ddlProgrammazione.SelectedValue)
    End Sub
    Private Sub CaricaVerificatore()
        Dim strsql As String
        '*****Carico Combo Verificatori Interni

        strsql = "select IdVerificatore, (Cognome + ' ' + Nome) As Nome " & _
                 " from TVerificatori WHERE Tipologia=0 AND Abilitato=0 "
        'If ddlCompetenza.SelectedValue <> 22 And ddlCompetenza.SelectedValue <> 0 Then
        '    strsql = strsql & " and IdRegCompetenza = " & ddlCompetenza.SelectedValue & " "
        'End If

        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case 0
                    strsql = strsql & ""
                Case -1
                    strsql = strsql & " and IdRegCompetenza = 22"
                Case -2
                    strsql = strsql & " and IdRegCompetenza <> 22 And not IdRegCompetenza is null "
                Case -3
                    strsql = strsql & " and IdRegCompetenza is null "
                Case Else
                    strsql = strsql & " and IdRegCompetenza = " & ddlCompetenza.SelectedValue
            End Select
        End If


        ddlVerificatoreInterno.DataSource = MakeParentTable(strsql)
        ddlVerificatoreInterno.DataTextField = "ParentItem"
        ddlVerificatoreInterno.DataValueField = "id"
        ddlVerificatoreInterno.DataBind()

        ''controllo che se il tipo di utente è MASTER o ISPETTORE blocco o meno le due combo
        ''aggiunto da Jon Cruise il 14.06.2007
        Dim IdVer As Integer
        IdVer = ClsUtility.TrovaProfiloUtente(Me.TemplateSourceDirectory, Session("Utente"), Session("Conn"))
        If IdVer <> 0 Then
            'aggiunto il 29/08/2007  da simona cordella
            'carico combo IGF a secondo del verificatoreinterno 
            '*****Carico Combo Verificatori IGF

            strsql = "SELECT V.IDVerificatore, V.Cognome + ' ' + V.Nome AS Nome " & _
                    " FROM TVerificatori AS V " & _
                    " INNER JOIN TVerificheAssociaUser AS VU ON V.IDVerificatore = VU.IDVerificatoreIGF " & _
                    " WHERE  (V.Tipologia = 1) AND (V.Abilitato = 0) AND (V.GenericoIGF = 0) " & _
                    " and vu.idverificatoreinterno = " & IdVer & "  and v.idRegCompetenza=22 "
            'ddlVerificatoreIGF.DataSource = MakeParentTable(strsql)
            'ddlVerificatoreIGF.DataTextField = "ParentItem"
            'ddlVerificatoreIGF.DataValueField = "id"
            'ddlVerificatoreIGF.DataBind()
            'ddlVerificatoreInterno.SelectedValue = TrovaIdVerificatore()
            ddlVerificatoreInterno.Enabled = False
            'If ddlVerificatoreIGF.Items.Count = "1" Then 'riga vuota combo
            '    ddlVerificatoreIGF.Enabled = False
            'End If

            'Else
            '    '*****Carico Combo Verificatori IGF
            '    ddlVerificatoreIGF.DataSource = MakeParentTable("select IdVerificatore, (Cognome + ' ' + Nome) As Nome from TVerificatori WHERE Tipologia=1 AND Abilitato=0 and genericoIGF =0 and idRegCompetenza=22")
            '    ddlVerificatoreIGF.DataTextField = "ParentItem"
            '    ddlVerificatoreIGF.DataValueField = "id"
            '    ddlVerificatoreIGF.DataBind()
        End If
        'If ddlCompetenza.SelectedValue <> 22 And ddlCompetenza.SelectedValue <> 0 Then
        '    ddlVerificatoreIGF.Enabled = False
        'Else
        '    ddlVerificatoreIGF.Enabled = True
        'End If
        'Select Case ddlCompetenza.SelectedValue
        '    Case 0
        '        ddlVerificatoreIGF.Enabled = True
        '    Case -1
        '        ddlVerificatoreIGF.Enabled = True
        '        ' strsql = strsql & " and IdRegCompetenza = 22"
        '    Case -2
        '        ddlVerificatoreIGF.Enabled = False
        '        'strsql = strsql & " and IdRegCompetenza <> 22 And not IdRegCompetenza is null "
        '    Case -3
        '        ddlVerificatoreIGF.Enabled = False
        '        'strsql = strsql & " and IdRegCompetenza is null "
        '    Case Else
        '        If ddlCompetenza.SelectedValue <> 22 And ddlCompetenza.SelectedValue <> 0 Then
        '            ddlVerificatoreIGF.Enabled = False
        '        Else
        '            ddlVerificatoreIGF.Enabled = True
        '        End If
        '        'ddlVerificatoreIGF.Enabled = True
        '        'strsql = strsql & " and IdRegCompetenza = " & ddlCompetenza.SelectedValue
        'End Select

    End Sub

    Private Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged

    End Sub


    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEsporta.Click
        cmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            cmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub
    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
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

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function

End Class