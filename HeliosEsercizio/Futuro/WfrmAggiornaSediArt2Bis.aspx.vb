Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Security.Cryptography
'Imports System.Text.RegularExpressions.Regex
Imports Logger.Data
Class WfrmAggiornaSediArt2Bis
    Inherits SmartPage
    Dim dtsGenerico As DataSet
    Dim strsql As String 'variabile stringa Che contiene stringa SQL
    Dim dtrGenerico As System.Data.SqlClient.SqlDataReader 'dichiarazione datareader
    Dim strNull As String 'variabile stringa che contiene valore NULL
    Dim ajaxHelper As AjaxHelper = New AjaxHelper()
    Dim bandiera As Integer
    Dim rstGenerico As SqlClient.SqlCommand
    Public IDESA As Integer
    Public strIdEnteSede As String
    Dim selComune As New clsSelezionaComune
    Dim helper As Helper = New Helper()
    Public Shared AlboEnte As String
    Public Shared idsedeInMod As String
    Public Shared DensedeInMod As String
    Public ShowPopUPControllo As String
    'Public AnomaliaIndirizzo, AnomaliaNome As Integer
    Public IndirizzoRicerca As String
    Dim indirizzoErratoHelios As Boolean = False
    Dim indirizzoOkGoogle As Boolean = False
    Dim procediGM As Boolean = False
    Dim strMiaCausale As String = ""
    Const INDEX_DGRISULTATORICERCA_ANOMALIE As Byte = 5
    Const INDEX_DGRISULTATORICERCA_ANOMALIA_NOME As Byte = 6
    Const INDEX_DGRISULTATORICERCA_ANOMALIA_INDIRIZZO As Byte = 7
    Const INDEX_DGRISULTATORICERCA_ANOMALIA_INDIRIZZO_GOOGLE As Byte = 8

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
        ShowPopUPControllo = ""
        IndirizzoRicerca = ""
        Session("Procedi") = 0
        Session("ProcediGM") = 0
        procediGM = False
        VerificaSessione()

        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

        If IsPostBack = False Then
            Session("LoadLSEId") = Nothing
            Session("LoadedLSE") = Nothing
            Session("AnomaliaNome") = 0
            Session("AnomaliaIndirizzo") = 0
            Session("AnomaliaIndirizzoGM") = 0


            If VerificaAbilitazione(Session("IdEnte"), Request.QueryString("identefase"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            lblRifFase.Text = Request.QueryString("identefase")
            ChiudiDataReader(dtrGenerico)
            dtrGenerico = ClsServer.CreaDatareader("select DataInizioFase,DataFineFase from entifasi where identefase=" & lblRifFase.Text, Session("Conn"))
            dtrGenerico.Read()

            If dtrGenerico.HasRows = True Then
                lbldal.Text = dtrGenerico("DataInizioFase")
                lblal.Text = dtrGenerico("DataFineFase")
            End If
            ChiudiDataReader(dtrGenerico)
            CaricoComboTitoloGiuridico(AlboEnte)

            Dim idEnteSede As String = Request.QueryString("identesede")
            lblidEnte.Value = Request.QueryString("idente")
            EvidenziaDatiModificati(idEnteSede)
            ChiudiDataReader(dtrGenerico)

        End If



    End Sub

#Region "Richiesta Modifica Sede"


    Private Sub AggiornaRichiestaModifica(ByVal AvvisoSede As Integer, ByVal IdEnteSede As Integer)
        Dim myQuerySql As String
        Dim Cmd As SqlClient.SqlCommand
        Try
            myQuerySql = "Update EntiSedi Set AvvisoSede =  " & AvvisoSede & " where IdEntesede=" & IdEnteSede
            Cmd = New SqlClient.SqlCommand(myQuerySql, Session("conn"))
            Cmd.ExecuteNonQuery()
            myQuerySql = "INSERT INTO CronologiaEntiSediRichiestaModifica (IdEntesede,AvvisoSede, UsernameRichiesta, DataRichiestaModifica)"
            myQuerySql &= " VALUES (" & IdEnteSede & "," & AvvisoSede & ",'" & Session("Utente") & "',getdate())"
            Cmd = New SqlClient.SqlCommand(myQuerySql, Session("conn"))
            Cmd.ExecuteNonQuery()


        Catch ex As Exception
            msgErrore.Visible = True
            msgErrore.Text = "Contattare l'assistenza."
        End Try

    End Sub


#End Region

#Region "Operazioni DB"


    Private Sub VerificaCap()

        'Effettuo Modifiche della sede
        Dim dtrTrovaProviciaDB As SqlClient.SqlDataReader
        Dim dtrTrovaProvinciaClient As SqlClient.SqlDataReader
        Dim IntProvinciaDB As Integer
        Dim IntComuneDB As Integer
        Dim ProvinciaDB As String
        Dim ComuneDB As String
        Dim blnProvincia As Boolean
        Dim IntComuneClient As Integer
        Dim IntProvinciaClient As Integer
        Dim ProvinciaClient As String
        Dim ComuneClient As String
        blnProvincia = False

        ChiudiDataReader(dtrGenerico)

        'Dim strMiaCausale As String = ""
        If ClsUtility.CAP_VERIFICA(Session("Conn"), _
                strMiaCausale, bandiera, Trim(txtCap.Text), ddlComune.SelectedItem.Value, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
            'Ripristino lo stato del tasto

            msgErrore.Text = strMiaCausale
            ChiudiDataReader(dtrGenerico)
            Exit Sub
        End If

    End Sub

#End Region

#Region "Eventi"


    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click

        Session("appDtsRisRicerca") = Nothing
        Response.Redirect("WfrmMain.aspx")

        Session("IdComune") = Nothing
    End Sub


#End Region

#Region "Fuzionalita"

    'Private Function MakeParentTable(ByVal strquery As String) As DataSet
    '    '***Generata da Gianluigi Paesani in data:05/07/04
    '    ' Create a new DataTable.
    '    Dim myDataTable As DataTable = New DataTable
    '    ' Declare variables for DataColumn and DataRow objects.
    '    Dim myDataColumn As DataColumn
    '    Dim myDataRow As DataRow
    '    ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
    '    myDataColumn = New DataColumn
    '    myDataColumn.DataType = System.Type.GetType("System.Int64")
    '    myDataColumn.ColumnName = "id"
    '    myDataColumn.Caption = "id"
    '    myDataColumn.ReadOnly = True
    '    myDataColumn.Unique = True
    '    ' Add the Column to the DataColumnCollection.
    '    myDataTable.Columns.Add(myDataColumn)
    '    ' Create second column.
    '    myDataColumn = New DataColumn
    '    myDataColumn.DataType = System.Type.GetType("System.String")
    '    myDataColumn.ColumnName = "ParentItem"
    '    myDataColumn.AutoIncrement = False
    '    myDataColumn.Caption = "ParentItem"
    '    myDataColumn.ReadOnly = False
    '    myDataColumn.Unique = False
    '    myDataTable.Columns.Add(myDataColumn)

    '    If Not dtrGenerico Is Nothing Then
    '        dtrGenerico.Close()
    '        dtrGenerico = Nothing
    '    End If
    '    dtrGenerico = ClsServer.CreaDatareader(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

    '    myDataRow = myDataTable.NewRow()
    '    myDataRow("id") = 0
    '    myDataRow("ParentItem") = "Sede Propria"
    '    myDataTable.Rows.Add(myDataRow)
    '    Do While dtrGenerico.Read
    '        myDataRow = myDataTable.NewRow()
    '        myDataRow("id") = dtrGenerico.GetValue(0)
    '        myDataRow("ParentItem") = dtrGenerico.GetValue(1)
    '        myDataTable.Rows.Add(myDataRow)
    '    Loop

    '    dtrGenerico.Close()
    '    dtrGenerico = Nothing

    '    MakeParentTable = New DataSet
    '    MakeParentTable.Tables.Add(myDataTable)
    'End Function


    Private Sub AbilitaCampiMaschera(ByVal isAbilitato As Boolean)
        txtCap.Enabled = isAbilitato
        txtCivico.Enabled = isAbilitato
        txtCodice.Value = isAbilitato
        ddlComune.Enabled = isAbilitato
        ddlProvincia.Enabled = isAbilitato

        txtfax.Enabled = isAbilitato

        txtIndirizzo.Enabled = isAbilitato
        TxtDettaglioRecapito.Enabled = isAbilitato
        TxtPalazzina.Enabled = isAbilitato
        TxtScala.Enabled = isAbilitato
        TxtPiano.Enabled = isAbilitato
        TxtInterno.Enabled = isAbilitato

        txtPrefFax.Enabled = isAbilitato
        txtprefisso.Enabled = isAbilitato
        txtTelefono.Enabled = isAbilitato



    End Sub

    Private Sub Clear()
        txtCap.Text = ""
        txtCivico.Text = ""
        txtCodice.Value = ""

        txtfax.Text = ""
        txtIndirizzo.Text = ""
        TxtDettaglioRecapito.Text = ""
        TxtPalazzina.Text = ""
        TxtScala.Text = ""
        TxtPiano.Text = ""
        TxtInterno.Text = ""

        txtPrefFax.Text = ""
        txtprefisso.Text = ""
        txtTelefono.Text = ""

    End Sub
    Private Function trovaindex(ByVal stringa As String, ByVal obj As Object)
        Dim i As Integer
        For i = 0 To obj.Items.Count
            If obj.Items(i).Text = stringa Then
                trovaindex = i
                Exit For
            End If
        Next
    End Function

#End Region

    'Funzione per controllare se sono presenti dei numeri all'interno di una stringa.(TRUE: c'è almeno un numero)
    Function checkNumeri(ByVal myString As String) As Boolean
        Dim maxIntX As Int16 = myString.Length - 1
        Dim intX As Int16 = 0
        For intX = 0 To maxIntX
            If IsNumeric(myString.Substring(intX, 1)) = True Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function

    Private Sub AbilitaDisabilitaTxtComune()
        'aggiunto il 10/10/2006 da simona cordella
        'Verifico se l'utente è abilitato al Menu' "Forza Modifica Comune" 
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
        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If
        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Modifica Comune'" & _
                 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlComune.Enabled = True
            ddlComune.BackColor = ddlComune.BackColor.White

        Else
            ddlComune.Enabled = False
            ddlComune.BackColor = ddlComune.BackColor.Gainsboro
        End If
        dtrGenerico.Close()
        dtrGenerico = Nothing
    End Sub

    Private Function VerificaModificaIndirizzoSede() As Boolean
        'Agg. da sc il 21/04/2009
        'Controllo se sono stati modificati INDIRIZZO, NUMERO CIVICO, CAP o COMUNE 
        Dim strSql As String
        Dim dtrVer As SqlClient.SqlDataReader

        If Not dtrVer Is Nothing Then
            dtrVer.Close()
            dtrVer = Nothing
        End If
        strSql = "SELECT  ISNULL(Indirizzo,'') AS Indirizzo, ISNULL(Civico,'') AS Civico ,ISNULL(IDComune,'') AS IDComune, ISNULL(CAP,'') AS CAP FROM entisedi where idEnteSede=" & txtidsede.Value & " and idente=" & lblidEnte.Value & ""
        dtrVer = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrVer.HasRows = True Then
            dtrVer.Read()
            If (dtrVer("Indirizzo") <> txtIndirizzo.Text) Or (dtrVer("Civico") <> txtCivico.Text) Or (dtrVer("IDComune") <> ddlComune.SelectedItem.Value) Or (dtrVer("CAP") <> txtCap.Text) Then
                If Not dtrVer Is Nothing Then
                    dtrVer.Close()
                    dtrVer = Nothing
                End If
                Return True
            Else
                If Not dtrVer Is Nothing Then
                    dtrVer.Close()
                    dtrVer = Nothing
                End If
                Return False
            End If
        End If
    End Function

    Private Function ControlloUtilizzoSede(Optional ByVal IdEnteSede As Integer = 0) As Boolean
        Dim StrSql As String
        Dim rstSede As SqlClient.SqlDataReader
        Dim strPalazzina As String
        Dim strScala As String
        Dim intPiano As Integer
        Dim strInterno As String
        Dim intIdEnteSede As Integer
        'agg. da Simona COrdella il 20/05/2009 per nuovo accreditamento Luglio 2009
        '1. Verifico se la denominazione della sede, l'indirizzo, il numero civico, il cap e il comune già esistono per l'ente
        '2. nel caso di modifica sede escludo la sede che sto modificando
        '3.
        If Not rstSede Is Nothing Then
            rstSede.Close()
            rstSede = Nothing
        End If
        StrSql = "Select identesede,denominazione,indirizzo,civico,idcomune,cap,isnull(palazzina,'') as palazzina,isnull(scala,'') as scala, isnull(piano,'') as piano,isnull(interno,'') as interno  from entisedi "
        StrSql = StrSql & " where denominazione ='" & txtdenominazione.Text.Replace("'", "''") & "' "
        StrSql = StrSql & " and indirizzo='" & txtIndirizzo.Text.Replace("'", "''") & "' "
        StrSql = StrSql & " and civico='" & txtCivico.Text.Replace("'", "''") & "' "
        StrSql = StrSql & " and idcomune =" & ddlComune.SelectedValue & "  and cap='" & txtCap.Text & "'"
        If IdEnteSede <> 0 Then 'fase modifica sede
            StrSql = StrSql & " and identesede <> " & IdEnteSede
        End If
        rstSede = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If rstSede.HasRows = False Then
            'caso in cui nn ci sono sedi con stessa denominazione,civico,cap e comune
            If Not rstSede Is Nothing Then
                rstSede.Close()
                rstSede = Nothing
            End If
            Return False
        Else ' se esistono sedi con stessa denominazione,civico,cap e comune
            rstSede.Read()
            txtdenominazione.Text = rstSede("Denominazione")
            intIdEnteSede = rstSede("identesede")
            strPalazzina = rstSede("palazzina")
            strScala = rstSede("scala")
            intPiano = rstSede("piano")
            strInterno = rstSede("interno")
            If Not rstSede Is Nothing Then
                rstSede.Close()
                rstSede = Nothing
            End If

            StrSql = "Select identesede,denominazione,indirizzo,civico,idcomune,cap,isnull(palazzina,'') as palazzina,isnull(scala,'') as scala, isnull(piano,'') as piano,isnull(interno,'') as interno  from entisedi "
            StrSql = StrSql & " where identesede <> " & intIdEnteSede
            rstSede = ClsServer.CreaDatareader(StrSql, Session("conn"))
            If rstSede.HasRows = False Then
                rstSede.Read()
                If (IsDBNull(rstSede("Palazzina")) Or rstSede("Palazzina") = strPalazzina) Then
                    Return False
                ElseIf (IsDBNull(rstSede("scala")) Or rstSede("scala") = strScala) Then
                    Return False
                ElseIf (IsDBNull(rstSede("piano")) Or rstSede("piano") = intPiano) Then
                    Return False
                ElseIf (IsDBNull(rstSede("interno")) Or rstSede("interno") = strInterno) Then
                    Return False
                End If
            End If
        End If
        If Not rstSede Is Nothing Then
            rstSede.Close()
            rstSede = Nothing
        End If
    End Function

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                CancellaMessaggi()
                If ClsUtility.ControlloRichiestaModificaSede(e.Item.Cells(0).Text, Session("Conn")) = True Then
                    msgErrore.Text = "Attenzione. E' gia' in corso una modifica sulla sede."
                    Exit Sub
                End If

                ChiudiDataReader(dtrGenerico)
                Clear()
                'PulisciMaschera()
                idSedeattuazione.Text = e.Item.Cells(1).Text
                FilModificaSede.Visible = True
                DensedeInMod = e.Item.Cells(4).Text
                idsedeInMod = e.Item.Cells(0).Text
                txtdenominazione.Text = DensedeInMod
                PopolaMaschera(e.Item.Cells(0).Text)
                GestioneMarcatore()
                If ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale() = False Then
                    msgErrore.Text = "Attenzione. sede non localizzata nell'ambito territoriale."
                    Exit Sub
                End If
                CaricaDatiEntiSediAttuazione(e.Item.Cells(0).Text)
                CaricaDatiEntiSediAttuazioneVariazione(e.Item.Cells(0).Text)
                'EvidenziaDatiModificati(e.Item.Cells(0).Text)  NO



                ' Response.Redirect("WfrmAnagraficaSedi.aspx?identesede=" & identesede & "&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&idente=" & idente & "&acquisita=" & acquisita & "&stato=" & stato)
        End Select
    End Sub

    Private Function GestioneMarcatore() As Boolean
        '** Agg. da simona cordella il 21/05/2009 per nuovo accrditamento
        '** Marcatore a 1 se modifico uno dei campi ( INDIRIZZO - NUMERO CIVICO- CAP - COMUNE)
        '** Mod. il 27/09/2013 da Simona Cordella
        '** nel controllo sono stati aggiunti i campo PALAZZINA,SCALA,PIANO,INTERNO, TIPOGIURIDICO E NUM. MAX VOLONTARI
        Dim blnMarcatore As Boolean = False
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strsql = " Select es.Denominazione, ISNULL(es.Indirizzo,'') AS Indirizzo, ISNULL(es.Civico,'') as Civico, ISNULL(es.IDComune,'') as IDComune, ISNULL(es.CAP,'') as CAP,es.marcatore, "
        strsql &= " ISNULL(es.Palazzina,'') as Palazzina, ISNULL(es.Scala,'')as Scala, ISNULL(CONVERT(VARCHAR(5),es.Piano),'') as Piano, ISNULL(es.Interno,'') as Interno, es.IdTitoloGiuridico, esa.NMaxVolontari"
        strsql &= " FROM entisedi es "
        strsql &= " INNER JOIN entisediAttuazioni esa on es.identesede = esa.identesede"
        strsql &= " WHERE es.IdEntesede=" & txtidsede.Value & " and isnull(es.marcatore,0)=0 "
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            If dtrGenerico("Marcatore") = False Then
                ''txtdenominazione.Text <> dtrGenerico("Denominazione") _ Or
                If (txtIndirizzo.Text <> dtrGenerico("Indirizzo") _
                        Or txtCivico.Text <> dtrGenerico("Civico") _
                        Or txtCap.Text <> dtrGenerico("CAP") _
                        Or ddlComune.SelectedItem.Value <> dtrGenerico("IDComune") _
                        Or TxtPalazzina.Text <> dtrGenerico("Palazzina") _
                        Or TxtPiano.Text <> dtrGenerico("Piano") _
                        Or TxtInterno.Text <> dtrGenerico("Interno") _
                        Or TxtScala.Text <> dtrGenerico("Scala")) Then

                    blnMarcatore = True
                End If
            Else
                blnMarcatore = False
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return blnMarcatore
    End Function



    Private Function ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale() As Boolean
        'Creata da Simona Cordella il 10/06/2009
        'Verifica la tipologia dell'ente padre o figlio
        'Se la tipologia dell'ente prevede OBBLIGOLOCALIZZAZIONE =TRUE 
        'possono inserire o modificare sedi, solo nei comuni che sono stati indicati nella tabella ENTIAMBITOTERRITORIALE

        'RITORNA TRUE se la sede appartiene al comune indicato con OBBLIGOLOCALIZZAZIONE = TRUE oppure se la tipologia dell'ente nn prevede l'obbligolocalizzazione
        'RITORNA FALSE se la tipologia dell'ente prevede OBBLIGOLOCALIZZAZIONE = TRUE ma il comune  non è stato indicato nella tabella ENTIAMBITOTERRITORIALE
        Dim dtrTerritorio As SqlClient.SqlDataReader
        Dim StrSql As String

        If Not dtrTerritorio Is Nothing Then
            dtrTerritorio.Close()
            dtrTerritorio = Nothing
        End If

        ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale = True

        'Ricavo la tipologia dell'Ente e flag ObbligoLocalizzazione
        StrSql = "Select enti.Tipologia,TipologieEnti.ObbligoLocalizzazione "
        StrSql = StrSql & " FROM enti "
        StrSql = StrSql & " INNER JOIN TipologieEnti ON enti.Tipologia = TipologieEnti.Descrizione"
        StrSql = StrSql & " WHERE idEnte = " & Session("idEnte")

        dtrTerritorio = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        If dtrTerritorio.HasRows = False Then
            ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale = True
        Else
            dtrTerritorio.Read()
            If dtrTerritorio("ObbligoLocalizzazione") = True Then
                If Not dtrTerritorio Is Nothing Then
                    dtrTerritorio.Close()
                    dtrTerritorio = Nothing
                End If
                'verifico se il comune della sede che è stato indicato in inserimento/modifica è presente nella tabella EntiAmbitoTerritoriale
                StrSql = "Select IdComune,IDEnte from EntiAmbitoTerritoriale  WHERE "
                'If Request.Form("txtIDComune") = "" Then
                StrSql = StrSql & " idComune ='" & ddlComune.SelectedItem.Value & "'"
                'Else
                '    StrSql = StrSql & " idComune ='" & Request.Form("txtIDComune") & "'"
                'End If
                StrSql = StrSql & " AND idEnte = " & Session("idEnte")

                dtrTerritorio = ClsServer.CreaDatareader(StrSql, Session("Conn"))
                If dtrTerritorio.HasRows = False Then
                    'comune nn presente in tabella
                    ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale = False
                Else
                    'comune presente in tabella
                    ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale = True
                End If
            End If
        End If
        If Not dtrTerritorio Is Nothing Then
            dtrTerritorio.Close()
            dtrTerritorio = Nothing
        End If
        Return ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale

    End Function


    Private Function ControlloDoppioneSedeNuovaGlobale(ByVal IdEnteSede As Integer, ByVal Indirizzo As String, ByVal Civico As String, ByVal IdComune As Integer, ByVal Palazzina As String, ByVal Scala As String, ByVal Piano As String, ByVal Interno As String) As Boolean
        'Aggiunto da Antonello il 10/01/2014
        'Richimo funzione sql che controlla se i dati in input per Sede sono già presenti nel db
        'la funzione sql ritorna 
        '       0 : se non esistono 
        '       1 : se esistono del db
        'la funzione ControlloDoppioneSedeNuovaGlobale ritorna 
        '       FALSE : se posso modificare la sede
        '       TRUE  : se non posso continuare la modifica(invio messaggio bloccante)

        Dim strSql As String
        Dim dtrNuovaSedeGlobale As SqlClient.SqlDataReader

        strSql = " Select dbo.DoppioneNuovaSedeGlobale(" & IdEnteSede & ",'" & Indirizzo.Replace("'", "''") & "', "
        strSql = strSql & " '" & Civico.Replace("'", "''") & "'," & IdComune & ",'" & Palazzina.Replace("'", "''") & "', "
        strSql = strSql & " '" & Scala.Replace("'", "''") & "','" & Piano.Replace("'", "''") & "','" & Interno & "') as DoppioneNuovaSedeGlobale"

        If Not dtrNuovaSedeGlobale Is Nothing Then
            dtrNuovaSedeGlobale.Close()
            dtrNuovaSedeGlobale = Nothing
        End If
        dtrNuovaSedeGlobale = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If dtrNuovaSedeGlobale.HasRows = True Then
            dtrNuovaSedeGlobale.Read()
            If dtrNuovaSedeGlobale("DoppioneNuovaSedeGlobale") = 1 Then
                ControlloDoppioneSedeNuovaGlobale = True
            Else
                ControlloDoppioneSedeNuovaGlobale = False
            End If

        End If
        If Not dtrNuovaSedeGlobale Is Nothing Then
            dtrNuovaSedeGlobale.Close()
            dtrNuovaSedeGlobale = Nothing
        End If
        Return ControlloDoppioneSedeNuovaGlobale
    End Function

    Private Sub CaricaDatiEntiSediAttuazione(ByVal IDEnteSede As Integer)
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'AGG DA SIMONA CORDELLA IL 19/05/2009 per nuovo accreditamento luglio 2009
        'visualizzo campi da  entisediattuazioni (NMaxVolontari,certificazione,datacertificazione ,usercertificazione,note)
        'verifico se la sede attuazione ha l'inidirizzo duplicato
        dtrGenerico = ClsServer.CreaDatareader("select NMaxVolontari,dbo.DoppioneSede(IdEnteSedeAttuazione) as DoppioneSede," & _
                        " Isnull(Entisediattuazioni.Certificazione,2) as Certificazione, Isnull(dbo.FormatoData(DataCertificazione),'') as DataCertificazione, Isnull(Entisediattuazioni.UserCertificazione,'') as UserCertificazione,Note,Isnull(Entisediattuazioni.Segnalazione,0) as Segnalazione " & _
                        " From entisediattuazioni " & _
                        " where identeSede=" & IDEnteSede & "", Session("Conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()

            'agg da simona cordella il 30/05/2009
            'visualizzazione e gestione del flag certificazione

            Session("intStatoPrecCertificazione") = dtrGenerico("Certificazione")

            If dtrGenerico("DoppioneSede") = 1 Then
                msgErrore.Text = "Attenzione. L'indirizzo indicato risulta utilizzato da altra Sede."
            End If

            If dtrGenerico("Segnalazione") = True Then
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If



            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Sub CaricaDatiEntiSediAttuazioneVariazione(ByVal IDEnteSede As Integer)
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'AGG DA SIMONA CORDELLA IL 19/05/2009 per nuovo accreditamento luglio 2009
        'visualizzo campi da  entisediattuazioni (NMaxVolontari,certificazione,datacertificazione ,usercertificazione,note)
        'verifico se la sede attuazione ha l'inidirizzo duplicato
        strsql = "select entisediattuazioni.NMaxVolontari,dbo.DoppioneSede(IdEnteSedeAttuazione) as DoppioneSede, " & _
                " Isnull(Entisediattuazioni.Certificazione,2) as Certificazione, " & _
                " Isnull(dbo.FormatoData(entisediattuazioni.DataCertificazione),'') as DataCertificazione," & _
                " Isnull(Entisediattuazioni.UserCertificazione,'') as UserCertificazione,entisediattuazioni.Note," & _
                " Isnull(Entisediattuazioni.RipristinoSegnalazione,0) as Segnalazione " & _
                " From Accreditamento_VariazioneSedi entisediattuazioni " & _
                " INNER JOIN entisediattuazioni esa on esa.IdEnteSede = entisediattuazioni.IdEnteSede " & _
                " WHERE entisediattuazioni.identeSede= " & IDEnteSede & " and entisediattuazioni.StatoVariazione =0 "
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()

            'agg da simona cordella il 30/05/2009
            'visualizzazione e gestione del flag certificazione

            Session("intStatoPrecCertificazione") = dtrGenerico("Certificazione")

            If dtrGenerico("DoppioneSede") = 1 Then
                msgErrore.Text = "Attenzione. L'indirizzo indicato risulta utilizzato da altra Sede."
            End If

            If UCase(dtrGenerico("Segnalazione")) = "SI" Then
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If



            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Function ControlloComuneEstero() As Boolean
        'Agg da Simona Cordella
        ''controllo se la sede è di tipo estero
        'FALSE : SEDE IN ITALIA
        'TRUE  : SEDE ESTERO
        ControlloComuneEstero = True

        strsql = "Select * from comuni where comunenazionale=1 and  "
        'If Request.Form("txtIDComune") = "" Then
        strsql = strsql & " idComune ='" & ddlComune.SelectedItem.Value & "'"
        'Else
        '    strsql = strsql & " idComune ='" & Request.Form("txtIDComune") & "'"
        'End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
        If dtrGenerico.HasRows = True Then
            ControlloComuneEstero = False
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return ControlloComuneEstero
    End Function

    Sub PopolaMaschera(ByVal IdEnteSede As Integer)
        ChiudiDataReader(dtrGenerico)
        Dim idProvincia As String
        Dim idComune As String
        Dim ObbligoScadenza As Boolean

        'popolamento della maschera dati DB
        dtrGenerico = ClsServer.CreaDatareader("select day(entisedi.datacontrolloemail)as ggDCEmail,month(entisedi.datacontrolloemail)as " & _
                    " monthDCEmail,year(entisedi.datacontrolloemail)as yearDCEmail," & _
                    " day(entisedi.datacontrollohttp)as ggDChttp,month(entisedi.datacontrollohttp)as monthDChttp," & _
                    " year(entisedi.datacontrollohttp)as yearDChttp," & _
                    " entisediTipi.idtiposede,comuni.idComune,comuni.denominazione as Comune,provincie.provincia,provincie.idprovincia,provincie.ProvinceNazionali ,StatiEntiSedi.statoentesede,* from entisedi " & _
                    " inner join StatiEntiSedi on (entisedi.IdStatoEnteSede=dbo.StatiEntiSedi.IdStatoEnteSede) " & _
                    " inner join comuni on (comuni.idcomune=entisedi.idcomune) " & _
                    " inner join provincie on (provincie.idprovincia=comuni.idprovincia)" & _
                    " left join entisediTipi on (entisediTipi.identesede=entisedi.idEntesede)" & _
                    " where entisedi.identeSede=" & IdEnteSede & "", Session("Conn"))
        dtrGenerico.Read()

        If dtrGenerico.HasRows = True Then
            'aggiunto il 05/03/2008 da s.c.
            ChkEstero.Checked = Not CBool(dtrGenerico("ProvinceNazionali"))

            If Not IsDBNull(dtrGenerico("denominazione")) Then

                txtidsede.Value = dtrGenerico("idEnteSede")

            End If
            If Not IsDBNull(dtrGenerico("identesede")) Then
                txtCodice.Value = dtrGenerico("identesede")
            End If
            If Not IsDBNull(dtrGenerico("indirizzo")) Then
                txtIndirizzo.Text = dtrGenerico("indirizzo")
            End If
            If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                TxtDettaglioRecapito.Text = dtrGenerico("DettaglioRecapito")
            End If

            If Not IsDBNull(dtrGenerico("Palazzina")) Then
                TxtPalazzina.Text = dtrGenerico("Palazzina")
            End If
            'Scala
            If Not IsDBNull(dtrGenerico("Scala")) Then
                TxtScala.Text = dtrGenerico("Scala")
            End If
            'Piano
            If Not IsDBNull(dtrGenerico("Piano")) Then
                TxtPiano.Text = dtrGenerico("Piano")
            End If
            ' Interno
            If Not IsDBNull(dtrGenerico("Interno")) Then
                TxtInterno.Text = dtrGenerico("Interno")
            End If

            If Not IsDBNull(dtrGenerico("Civico")) Then
                txtCivico.Text = dtrGenerico("Civico")
            End If
            If Not IsDBNull(dtrGenerico("Cap")) Then
                txtCap.Text = dtrGenerico("Cap")
            End If
            If Not IsDBNull(dtrGenerico("idprovincia")) Then
                idProvincia = dtrGenerico("idprovincia")
            End If
            If Not IsDBNull(dtrGenerico("idcomune")) Then
                idComune = dtrGenerico("idcomune")
            End If

            If Not IsDBNull(dtrGenerico("Telefono")) Then
                txtTelefono.Text = dtrGenerico("Telefono")
            End If
            If Not IsDBNull(dtrGenerico("prefissoTelefono")) Then
                txtprefisso.Text = dtrGenerico("prefissoTelefono")
            End If
            If Not IsDBNull(dtrGenerico("Fax")) Then
                txtfax.Text = dtrGenerico("Fax")
            End If
            If Not IsDBNull(dtrGenerico("prefissoFax")) Then
                txtPrefFax.Text = dtrGenerico("prefissoFax")
            End If
            'IdTitoloGiuridico
            If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
                ddlTitoloGiuridico.SelectedValue = dtrGenerico("IdTitoloGiuridico")
            End If
            'If Not IsDBNull(dtrGenerico("datacontrollohttp")) Then
            '    lblDataControlloHttp.Value = dtrGenerico("datacontrollohttp")
            'End If
            If Not IsDBNull(dtrGenerico("Datacontrolloemail")) Then
                lbldataControlloEmail.Value = dtrGenerico("ggDCemail") & "/" & IIf(CInt(dtrGenerico("monthDCemail")) < 10, "0" & dtrGenerico("monthDCemail"), dtrGenerico("monthDCemail")) & "/" & dtrGenerico("YearDCemail")
            End If
            'txtdataControlloEmail.Text = IIf(Not IsDBNull(dtrgenerico("Datacontrolloemail")), dtrgenerico("Datacontrolloemail"), "")
            If Not IsDBNull(dtrGenerico("Datacontrollohttp")) Then
                lblDataControlloHttp.Value = dtrGenerico("ggDChttp") & "/" & IIf(CInt(dtrGenerico("monthDChttp")) < 10, "0" & dtrGenerico("monthDChttp"), dtrGenerico("monthDChttp")) & "/" & dtrGenerico("yearDChttp")
            End If


            If ChkEstero.Checked = True Then
                If Not IsDBNull(dtrGenerico("CittaEstera")) Then
                    city.Visible = True
                    txtCity.Text = dtrGenerico("CittaEstera")
                End If
            Else
                city.Visible = False
            End If
            If Not IsDBNull(dtrGenerico("NormativaTutela")) Then
                If dtrGenerico("NormativaTutela") = True Then
                    ChkTutela.Checked = True
                Else
                    ChkTutela.Checked = False
                End If
            End If

            If Not IsDBNull(dtrGenerico("SoggettoEstero")) Then
                TxtSoggettoCapoSede.Text = dtrGenerico("SoggettoEstero")
            End If

            If Not IsDBNull(dtrGenerico("NonDisponibilitaSede")) Then
                If dtrGenerico("NonDisponibilitaSede") = True Then
                    rbSE2.Checked = True
                    rbSE1.Checked = False
                Else
                    rbSE2.Checked = False
                    rbSE1.Checked = True
                End If
            End If

            If Not IsDBNull(dtrGenerico("DisponibilitaSede")) Then
                If dtrGenerico("DisponibilitaSede") = True Then
                    rbNoSE1.Checked = False
                    rbNoSE2.Checked = True
                Else
                    rbNoSE1.Checked = True
                    rbNoSE2.Checked = False
                End If
            End If

            If Not IsDBNull(dtrGenerico("IdAllegatoLSE")) Then
                Session("LoadLSEId") = dtrGenerico("IdAllegatoLSE")
            Else
                Session("LoadLSEId") = Nothing
            End If

            If Not IsDBNull(dtrGenerico("Localita")) Then
                txtLocalita.Text = dtrGenerico("Localita")
            Else
                txtLocalita.Text = String.Empty
            End If

            If ChkEstero.Checked = True Then
                lblComune.Text = "Localit&agrave;"
                lblCap.Text = "Codice localit&agrave;"
                If Not IsDBNull(dtrGenerico("CittaEstera")) Then
                    city.Visible = True
                    txtCity.Text = dtrGenerico("CittaEstera")
                End If
                ddlComune.Visible = False
                txtLocalita.Visible = True
                DivItalia.Visible = False
                DivEstero2.Visible = True

                If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
                    If dtrGenerico("IdTitoloGiuridico") = 7 Then
                        DivSoggettoEstero.Visible = True
                        DivNoSoggettoEstero.Visible = False
                    Else
                        DivSoggettoEstero.Visible = False
                        DivNoSoggettoEstero.Visible = True
                    End If
                End If
                DivItalia.Visible = False
                If Session("LoadedLSE") Is Nothing Then
                    rowNoLSE.Visible = True
                    rowLSE.Visible = False
                Else
                    rowNoLSE.Visible = False
                    rowLSE.Visible = True
                End If
                DivEstero2.Visible = True

            Else
                DivItalia.Visible = True
                DivEstero2.Visible = False
                city.Visible = False
                ddlComune.Visible = True
                ddlComune.Enabled = True
                txtLocalita.Visible = False
            End If


            ChiudiDataReader(dtrGenerico)
            selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
            If Not idProvincia Is Nothing Then
                ddlProvincia.SelectedValue = idProvincia
            End If
            ChiudiDataReader(dtrGenerico)
            If Not idComune Is Nothing Then
                selComune.CaricaComuniDaProvincia(ddlComune, idProvincia, Session("Conn"))
                ddlComune.SelectedValue = idComune
                Session("IdComune") = idComune
            End If
            If ChkEstero.Checked = True Then
                city.Visible = True
            Else
                city.Visible = False
            End If



            ChiudiDataReader(dtrGenerico)
            CaricaLSE()

        End If
        CaricaDatiEntiSediAttuazione(IdEnteSede)
    End Sub

    Private Sub PopolaMascheraVariazione(ByVal IdEnteSede As Integer)
        ChiudiDataReader(dtrGenerico)
        Dim idProvincia As String
        Dim idComune As String
        Dim ObbligoScadenza As Boolean

        'popolamento della maschera dati DB
        dtrGenerico = ClsServer.CreaDatareader("select day(entisedi.datacontrolloemail)as ggDCEmail,month(entisedi.datacontrolloemail)as  " & _
                            " monthDCEmail,year(entisedi.datacontrolloemail)as yearDCEmail, " & _
                            " day(entisedi.datacontrollohttp)as ggDChttp,month(entisedi.datacontrollohttp)as monthDChttp, " & _
                            " year(entisedi.datacontrollohttp)as yearDChttp, " & _
                            "entisediTipi.idtiposede,comuni.idComune,comuni.denominazione as Comune,provincie.provincia,provincie.idprovincia,provincie.ProvinceNazionali ,StatiEntiSedi.statoentesede,* 	 " & _
                            "from Accreditamento_VariazioneSedi entisedi  " & _
                            "inner join entisedi es on es.identeSede =entisedi.identeSede " & _
                            "inner join StatiEntiSedi on (es.IdStatoEnteSede=dbo.StatiEntiSedi.IdStatoEnteSede)  " & _
                            "inner join comuni on (comuni.idcomune=entisedi.idcomune)  " & _
                            "inner join provincie on (provincie.idprovincia=comuni.idprovincia) " & _
                            "left join entisediTipi on (entisediTipi.identesede=entisedi.idEntesede) " & _
                            " where entisedi.identeSede=" & IdEnteSede & "  and entiSedi.StatoVariazione =0", Session("Conn"))
        dtrGenerico.Read()
        If dtrGenerico.HasRows = True Then

            ChkEstero.Checked = Not CBool(dtrGenerico("ProvinceNazionali"))


            If Not IsDBNull(dtrGenerico("denominazione")) Then
                'txtdenominazione.Text = dtrGenerico("denominazione")
                txtidsede.Value = dtrGenerico("idEnteSede")
            End If
            If Not IsDBNull(dtrGenerico("identesede")) Then
                txtCodice.Value = dtrGenerico("identesede")
            End If
            If Not IsDBNull(dtrGenerico("indirizzo")) Then
                txtIndirizzo.Text = dtrGenerico("indirizzo")
            End If
            If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                TxtDettaglioRecapito.Text = dtrGenerico("DettaglioRecapito")
            End If


            'palazzina
            If Not IsDBNull(dtrGenerico("Palazzina")) Then
                TxtPalazzina.Text = dtrGenerico("Palazzina")
            End If
            'Scala
            If Not IsDBNull(dtrGenerico("Scala")) Then
                TxtScala.Text = dtrGenerico("Scala")
            End If
            'Piano
            If Not IsDBNull(dtrGenerico("Piano")) Then
                TxtPiano.Text = dtrGenerico("Piano")
            End If
            ' Interno
            If Not IsDBNull(dtrGenerico("Interno")) Then
                TxtInterno.Text = dtrGenerico("Interno")
            End If

            If Not IsDBNull(dtrGenerico("Civico")) Then
                txtCivico.Text = dtrGenerico("Civico")
            End If
            If Not IsDBNull(dtrGenerico("Cap")) Then
                txtCap.Text = dtrGenerico("Cap")
            End If
            If Not IsDBNull(dtrGenerico("idprovincia")) Then
                idProvincia = dtrGenerico("idprovincia")
            End If
            If Not IsDBNull(dtrGenerico("idcomune")) Then
                idComune = dtrGenerico("idcomune")
            End If
            If Not IsDBNull(dtrGenerico("idtiposede")) Then

                txtTipologia.Value = dtrGenerico("idtiposede")
            End If
            If Not IsDBNull(dtrGenerico("Telefono")) Then
                txtTelefono.Text = dtrGenerico("Telefono")
            End If
            If Not IsDBNull(dtrGenerico("prefissoTelefono")) Then
                txtprefisso.Text = dtrGenerico("prefissoTelefono")
            End If
            If Not IsDBNull(dtrGenerico("Fax")) Then
                txtfax.Text = dtrGenerico("Fax")
            End If
            If Not IsDBNull(dtrGenerico("prefissoFax")) Then
                txtPrefFax.Text = dtrGenerico("prefissoFax")
            End If

            If Not IsDBNull(dtrGenerico("Datacontrolloemail")) Then
                lbldataControlloEmail.Value = dtrGenerico("ggDCemail") & "/" & IIf(CInt(dtrGenerico("monthDCemail")) < 10, "0" & dtrGenerico("monthDCemail"), dtrGenerico("monthDCemail")) & "/" & dtrGenerico("YearDCemail")
            End If
            If Not IsDBNull(dtrGenerico("Datacontrollohttp")) Then
                lblDataControlloHttp.Value = dtrGenerico("ggDChttp") & "/" & IIf(CInt(dtrGenerico("monthDChttp")) < 10, "0" & dtrGenerico("monthDChttp"), dtrGenerico("monthDChttp")) & "/" & dtrGenerico("yearDChttp")
            End If


            'città stera
            If ChkEstero.Checked = True Then
                If Not IsDBNull(dtrGenerico("CittaEstera")) Then
                    txtCity.Text = dtrGenerico("CittaEstera")
                End If
            End If

            If Not IsDBNull(dtrGenerico("NormativaTutela")) Then
                If dtrGenerico("NormativaTutela") = True Then
                    ChkTutela.Checked = True
                Else
                    ChkTutela.Checked = False
                End If
            End If

            If Not IsDBNull(dtrGenerico("SoggettoEstero")) Then
                TxtSoggettoCapoSede.Text = dtrGenerico("SoggettoEstero")
            End If

            If Not IsDBNull(dtrGenerico("NonDisponibilitaSede")) Then
                If dtrGenerico("NonDisponibilitaSede") = True Then
                    rbSE2.Checked = True
                    rbSE1.Checked = False
                Else
                    rbSE2.Checked = False
                    rbSE1.Checked = True
                End If
            End If

            If Not IsDBNull(dtrGenerico("DisponibilitaSede")) Then
                If dtrGenerico("DisponibilitaSede") = True Then
                    rbNoSE1.Checked = False
                    rbNoSE2.Checked = True
                Else
                    rbNoSE1.Checked = True
                    rbNoSE2.Checked = False
                End If
            End If

            If Not IsDBNull(dtrGenerico("IdAllegatoLSE")) Then
                Session("LoadLSEId") = dtrGenerico("IdAllegatoLSE")
            Else
                Session("LoadLSEId") = Nothing
            End If

            If Not IsDBNull(dtrGenerico("Localita")) Then
                txtLocalita.Text = dtrGenerico("Localita")
            Else
                txtLocalita.Text = String.Empty
            End If

            If ChkEstero.Checked = True Then
                lblComune.Text = "Localit&agrave;"
                lblCap.Text = "Codice localit&agrave;"
                If Not IsDBNull(dtrGenerico("CittaEstera")) Then
                    city.Visible = True
                    txtCity.Text = dtrGenerico("CittaEstera")
                End If
                ddlComune.Visible = False
                txtLocalita.Visible = True
                DivItalia.Visible = False
                DivEstero2.Visible = True

                If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
                    If dtrGenerico("IdTitoloGiuridico") = 7 Then
                        DivSoggettoEstero.Visible = True
                        DivNoSoggettoEstero.Visible = False
                    Else
                        DivSoggettoEstero.Visible = False
                        DivNoSoggettoEstero.Visible = True
                    End If
                End If
            Else
                city.Visible = False
                ddlComune.Visible = True
                txtLocalita.Visible = False
            End If


            ChiudiDataReader(dtrGenerico)


            ChiudiDataReader(dtrGenerico)
            selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
            If Not idProvincia Is Nothing Then
                ddlProvincia.SelectedValue = idProvincia
            End If
            ChiudiDataReader(dtrGenerico)
            If Not idComune Is Nothing Then
                selComune.CaricaComuniDaProvincia(ddlComune, idProvincia, Session("Conn"))
                ddlComune.SelectedValue = idComune
                Session("IdComune") = idComune
            End If
            ChiudiDataReader(dtrGenerico)

            CaricaLSE()
        End If
        CaricaDatiEntiSediAttuazioneVariazione(IdEnteSede)
    End Sub

    Private Sub ModificaSedeEnte()

        'Funzionalità: richiamo store SP_ACCREDITAMENTO_MODIFICAMASCHERA_SEDE per l'aggiornamento dell'anagrafica della sede
        Dim LSE As Allegato = Session("LoadedLSE")
        Dim hashValue As String = ""
        Dim idAllegatoOld As String = ""
        Dim sqlGetAllegato = "SELECT Top 1 e.idAllegatoLSE, FileName,HashValue,FileLength,BinData,DataInserimento from allegato a inner join entisedi e on a.idallegato = e.IdAllegatoLSE where e.identesede=@IdEnteSede"
        Dim AllegatoCommand As New SqlCommand(sqlGetAllegato, Session("conn"))
        AllegatoCommand.Parameters.AddWithValue("@idEnteSede", CInt(txtCodice.Value))
        Dim dtrAllegato As SqlDataReader = AllegatoCommand.ExecuteReader()
        If dtrAllegato.Read Then
            hashValue = dtrAllegato("HashValue")
            idAllegatoOld = dtrAllegato("idAllegatoLSE")
            dtrAllegato.Close()
            AllegatoCommand.Dispose()
        Else
            dtrAllegato.Close()
            ' ClearSessionLSE()
        End If
        If Session("LoadLSEId") = Nothing And ChkEstero.Checked And ddlTitoloGiuridico.SelectedValue = 7 Then

            If LSE Is Nothing Then
                msgErrore.Text = "L'allegato Lettera di accordo con sede estera è obbligatorio<br/>"
                Log.Warning(LogEvent.SEDI_ART2_MODIFICA_ERRATA, "L'allegato Lettera di accordo con sede estera è obbligatorio")
                Exit Sub
            End If
            If LSE.Hash <> hashValue Then
                If idAllegatoOld = String.Empty Then
                    Dim idAllegato As Integer = SalvaAllegato(LSE, TipoFile.LETTERA_SEDE_ESTERA, lblRifFase.Text)
                    Session("LoadLSEId") = idAllegato
                    Log.Information(LogEvent.SEDI_ART2_IMPORTAZIONE_LSE)

                Else
                    If CercaAllegato(LSE, lblRifFase.Text) Then
                        AggiornaAllegato(LSE, lblRifFase.Text)
                        Session("LoadLSEId") = idAllegatoOld
                        Log.Information(LogEvent.SEDI_IMPORTAZIONE_LSE, "Aggiornato Allegato ID: " & idAllegatoOld)
                    Else
                        Dim idAllegato As Integer = SalvaAllegato(LSE, TipoFile.LETTERA_SEDE_ESTERA, lblRifFase.Text)
                        Session("LoadLSEId") = idAllegato
                        Log.Information(LogEvent.SEDI_ART2_IMPORTAZIONE_LSE)
                    End If
                End If

            End If
        End If

        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_MODIFICAMASCHERA_SEDE_ART2"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEnteFase ", SqlDbType.Int).Value = lblRifFase.Text
            SqlCmd.Parameters.Add("@IdEnteSede ", SqlDbType.Int).Value = txtCodice.Value
            SqlCmd.Parameters.Add("@Denominazione", SqlDbType.VarChar).Value = txtdenominazione.Text 'denominazioneente

            SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = ddlComune.SelectedItem.Value
            SqlCmd.Parameters.Add("@CAP", SqlDbType.VarChar).Value = txtCap.Text ' CAP
            SqlCmd.Parameters.Add("@Indirizzo", SqlDbType.VarChar).Value = txtIndirizzo.Text ' indirizzo
            SqlCmd.Parameters.Add("@Civico", SqlDbType.VarChar).Value = txtCivico.Text ' Civico
            SqlCmd.Parameters.Add("@DettaglioRecapito", SqlDbType.VarChar).Value = TxtDettaglioRecapito.Text ' DettaglioRecapito
            SqlCmd.Parameters.Add("@PrefissoTelefono", SqlDbType.VarChar).Value = txtprefisso.Text  'Prefisso telefono
            SqlCmd.Parameters.Add("@Telefono", SqlDbType.VarChar).Value = txtTelefono.Text

            If Trim(TxtPalazzina.Text) = "" Then
                SqlCmd.Parameters.Add("@Palazzina", SqlDbType.VarChar).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@Palazzina", SqlDbType.VarChar).Value = TxtPalazzina.Text
            End If

            If Trim(TxtPiano.Text) = "" Then
                SqlCmd.Parameters.Add("@Piano", SqlDbType.SmallInt).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@Piano", SqlDbType.VarChar).Value = TxtPiano.Text
            End If
            If Trim(TxtScala.Text) = "" Then
                SqlCmd.Parameters.Add("@Scala", SqlDbType.VarChar).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@Scala", SqlDbType.VarChar).Value = TxtScala.Text
            End If
            If Trim(TxtInterno.Text) = "" Then
                SqlCmd.Parameters.Add("@Interno", SqlDbType.VarChar).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@Interno", SqlDbType.VarChar).Value = TxtInterno.Text
            End If
            If Trim(txtPrefFax.Text) = "" Then
                SqlCmd.Parameters.Add("@PrefissoFax", SqlDbType.VarChar).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@PrefissoFax", SqlDbType.VarChar).Value = txtPrefFax.Text
            End If
            If Trim(txtfax.Text) = "" Then
                SqlCmd.Parameters.Add("@Fax", SqlDbType.VarChar).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@Fax", SqlDbType.VarChar).Value = txtfax.Text
            End If

            If ChkEstero.Checked = True Then
                If Trim(txtCity.Text) = "" Then
                    SqlCmd.Parameters.Add("@CityEstero", SqlDbType.VarChar).Value = DBNull.Value
                Else
                    SqlCmd.Parameters.Add("@CityEstero", SqlDbType.VarChar).Value = txtCity.Text ' città estera
                End If

            End If
            SqlCmd.Parameters.Add("@IdTitoloGiuridico", SqlDbType.Int).Value = ddlTitoloGiuridico.SelectedItem.Value
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output
            'Nuovi Campi
            If ChkEstero.Checked And ddlTitoloGiuridico.SelectedValue = 7 Then
                SqlCmd.Parameters.Add("@IdAllegatoLSE", SqlDbType.Int).Value = Session("LoadLSEId")
            Else
                SqlCmd.Parameters.Add("@IdAllegatoLSE", SqlDbType.Bit).Value = DBNull.Value
            End If
            SqlCmd.Parameters.Add("@AnomaliaIndirizzo", SqlDbType.Bit).Value = Session("AnomaliaIndirizzo")
            SqlCmd.Parameters.Add("@AnomaliaNome", SqlDbType.Bit).Value = Session("AnomaliaNome")
            If ChkEstero.Checked = False Then
                If ChkTutela.Checked Then
                    SqlCmd.Parameters.Add("@NormativaTutela", SqlDbType.Bit).Value = 1
                Else
                    SqlCmd.Parameters.Add("@NormativaTutela", SqlDbType.Bit).Value = 0
                End If
            Else
                SqlCmd.Parameters.Add("@NormativaTutela", SqlDbType.Bit).Value = 0
            End If

            If ChkEstero.Checked Then
                If ddlTitoloGiuridico.SelectedValue = 7 Then
                    SqlCmd.Parameters.Add("@SoggettoEstero", SqlDbType.VarChar, 50).Value = TxtSoggettoCapoSede.Text
                    If rbSE2.Checked Then
                        SqlCmd.Parameters.Add("@NonDisponibilitaSede", SqlDbType.Bit).Value = 1
                    Else
                        SqlCmd.Parameters.Add("@NonDisponibilitaSede", SqlDbType.Bit).Value = 0
                    End If
                    SqlCmd.Parameters.Add("@DisponibilitaSede", SqlDbType.Bit).Value = DBNull.Value
                Else
                    SqlCmd.Parameters.Add("@SoggettoEstero", SqlDbType.VarChar, 50).Value = DBNull.Value
                    SqlCmd.Parameters.Add("@NonDisponibilitaSede", SqlDbType.Bit).Value = DBNull.Value
                    If rbNoSE1.Checked Then
                        SqlCmd.Parameters.Add("@DisponibilitaSede", SqlDbType.Bit).Value = 0
                    Else
                        SqlCmd.Parameters.Add("@DisponibilitaSede", SqlDbType.Bit).Value = 1
                    End If
                End If
                'SqlCmd.Parameters.Add("@SoggettoEstero", SqlDbType.VarChar, 50).Value = TxtSoggettoCapoSede.Text
                'If rbSE2.Checked Then
                '    SqlCmd.Parameters.Add("@NonDisponibilitaSede", SqlDbType.Bit).Value = 1
                'Else
                '    SqlCmd.Parameters.Add("@NonDisponibilitaSede", SqlDbType.Bit).Value = 0
                'End If
            Else
                SqlCmd.Parameters.Add("@SoggettoEstero", SqlDbType.VarChar, 50).Value = DBNull.Value
                SqlCmd.Parameters.Add("@NonDisponibilitaSede", SqlDbType.Bit).Value = DBNull.Value
                SqlCmd.Parameters.Add("@DisponibilitaSede", SqlDbType.Bit).Value = DBNull.Value
            End If
            SqlCmd.Parameters.Add("@AnomaliaIndirizzoGoogle", SqlDbType.Bit).Value = Session("AnomaliaIndirizzoGM")

            If ChkEstero.Checked = True Then
                SqlCmd.Parameters.Add("@Localita", SqlDbType.VarChar).Value = txtLocalita.Text
            Else
                SqlCmd.Parameters.Add("@Localita", SqlDbType.VarChar).Value = String.Empty
            End If
            SqlCmd.ExecuteNonQuery()
            msgConferma.Text = SqlCmd.Parameters("@messaggio").Value()
            msgConferma.Text = msgConferma.Text & "<br/>"
            Log.Information(LogEvent.SEDI_ART2_MODIFICA_CORRETTA, "idEntesede:" & txtCodice.Value)
        Catch ex As Exception
            msgErrore.Text = ex.Message
            Log.Critical(LogEvent.SEDI_ART2_MODIFICA_ERRATA, "idEntesede:" & txtCodice.Value)
        Finally

        End Try
    End Sub

#Region "Gestione indirizzi"
    Private Sub ChkEstero_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEstero.CheckedChanged
        Dim selComune As New clsSelezionaComune
        selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
        ddlComune.DataSource = Nothing
        ddlComune.Items.Add("")
        ddlComune.SelectedIndex = 0
        CancellaMessaggi()
        GestisciDivEstero()
        MaintainScrollPositionOnPostBack = True

        'If ChkEstero.Checked = True Then
        '    city.Visible = True
        '    ddlComune.Visible = False
        '    txtLocalita.Visible = True
        '    lblComune.Text = "Localit&agrave;"
        '    lblCap.Text = "Codice localit&agrave;"
        'Else
        '    ddlComune.Visible = True
        '    txtLocalita.Visible = False
        '    city.Visible = False
        '    lblComune.Text = "<strong>(*)</strong>Comune"
        '    lblCap.Text = "<strong>(*)</strong>C.A.P."
        'End If
    End Sub
    Private Sub ddlProvincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvincia.SelectedIndexChanged
        Dim selComune As New clsSelezionaComune
        ddlComune.Enabled = True
        ddlComune = selComune.CaricaComuni(ddlComune, ddlProvincia.SelectedValue, Session("Conn"))
    End Sub
    Protected Sub imgCap_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgCap.Click
        msgErrore.Text = String.Empty
        Dim selCap As New clsSelezionaComune
        If ddlComune.SelectedValue = String.Empty And txtCivico.Text = String.Empty And txtIndirizzo.Text = String.Empty Then
            msgErrore.Text = "Per ottenere il C.A.P. della residenza è necessario indicare almeno il comune e l'indirizzo di residenza."
        Else
            txtCap.Text = selCap.RitornaCap(ddlComune.SelectedValue, txtIndirizzo.Text, txtCivico.Text, Session("conn"))
        End If
    End Sub

#End Region

    Private Function VerificaValiditaCampi(ByVal AlboEnte As String) As Boolean
        Dim utility As ClsUtility = New ClsUtility()
        Dim campiValidi As Boolean = True
        Dim numero As Int32
        Dim dataTmp As Date
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"
        Dim campoObbligatorioSCU As String = "Il campo {0} è obbligatorio."
        Dim campoObbligatorioSCU1 As String = "Indicare {0} nel caso non sia previsto.<br/>"
        Dim numeroNonValido As String = "Il valore di '{0}' non è valido. Inserire solo numeri.<br/>"
        Dim emailNonValida As String = "Il valore di '{0}' non è valido. Inserire un indirizzo email valido.<br/>"
        Dim LungezzaErrata As String = "Il campo {0} può contenere massimo 5 caratteri.<br/>"
        Dim messaggioDataValida As String = "Il valore di '{0}' non è valido. Inserire la data nel formato gg/mm/aaaa.<br/>"
        Dim CittaEstera As String = "Il campo {0} è obbligatorio."
        Dim ListaAnomalie As String = ""
        Dim CheckIndirizzo As Boolean = True
        Dim regioneCompetenza As Integer = getRegioneCompetenzaEnte()



        If (ddlProvincia.SelectedItem.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
            campiValidi = False
            CheckIndirizzo = False
        End If
        If (ddlComune.SelectedItem.Text = String.Empty) Then
            'If ChkEstero.Checked Then
            '    msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Localit&agrave;")
            'Else
            If ChkEstero.Checked = False Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Comune")
                campiValidi = False
                CheckIndirizzo = False
            End If

        End If
        If (txtIndirizzo.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
            campiValidi = False
            CheckIndirizzo = False
        End If
        If (txtCivico.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
            campiValidi = False
            CheckIndirizzo = False
        End If
        If (txtCap.Text = String.Empty And ChkEstero.Checked = False) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
            campiValidi = False
            CheckIndirizzo = False
        End If

        'If (TxtPiano.Text <> String.Empty) Then
        '    If (Int32.TryParse(TxtPiano.Text, numero) = False) Then
        '        msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Piano")
        '        campiValidi = False
        '    End If
        'End If

        If Len(TxtPalazzina.Text) > 5 Then
            msgErrore.Text = msgErrore.Text + String.Format(LungezzaErrata, "Palazzina")
            campiValidi = False

        End If
        'If Len(TxtScala.Text) > 5 Then
        '    msgErrore.Text = msgErrore.Text + String.Format(LungezzaErrata, "Scala")
        '    campiValidi = False

        'End If
        'If Len(TxtInterno.Text) > 5 Then
        '    msgErrore.Text = msgErrore.Text + String.Format(LungezzaErrata, "Interno")
        '    campiValidi = False

        'End If
        If TxtScala.Text = String.Empty Then TxtScala.Text = "ND"
        If TxtScala.Text = String.Empty Then TxtPiano.Text = "0"
        If TxtInterno.Text = String.Empty Then TxtInterno.Text = "ND"


        If AlboEnte = "SCU" Then
            If (TxtPalazzina.Text = String.Empty) Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU, "Palazzina")
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU1, "ND")
                campiValidi = False
            End If
            If (TxtScala.Text = String.Empty) Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU, "Scala")
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU1, "ND")
                campiValidi = False
            End If
            If (TxtPiano.Text = String.Empty) Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU, "Piano")
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU1, "0")
                campiValidi = False
            End If
            If (TxtInterno.Text = String.Empty) Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU, "Interno")
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU1, "ND")
                campiValidi = False
            End If
        End If

        If (txtprefisso.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Prefisso Telefono")
            campiValidi = False
        Else
            If (Int32.TryParse(txtprefisso.Text, numero) = False) Then
                msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Prefisso Telefono")
                campiValidi = False
            End If
        End If
        If (txtTelefono.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Telefono")
            campiValidi = False
        Else
            Try
                If (Int64.TryParse(txtTelefono.Text, numero) = False) Then
                    msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Telefono")
                    campiValidi = False
                End If
            Catch ex As Exception

            End Try

        End If
        If (txtPrefFax.Text <> String.Empty) Then
            If ValidaInteri(txtPrefFax.Text, "Prefisso Fax") = False Then
                txtPrefFax.Text = ""
                'msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Prefisso Fax")
                'campiValidi = False
            End If
        End If
        If (txtfax.Text <> String.Empty) Then
            If ValidaInteri(txtfax.Text, "Fax") = False Then
                txtfax.Text = ""
                'msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Fax")
                'campiValidi = False
            End If

        End If

        If ChkEstero.Checked = True Then
            If (Trim(txtCity.Text) = String.Empty) Then

                msgErrore.Text = msgErrore.Text + String.Format(CittaEstera, "Città Estera")
                campiValidi = False

            End If
        End If
        Dim strMiaCausale As String = ""
        If ddlComune.SelectedValue <> "0" And CheckIndirizzo = True And Not indirizzoOkGoogle And Not procediGM Then


            'originale
            'If ClsUtility.CAP_VERIFICA(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")),
            '    strMiaCausale, bandiera, Trim(txtCap.Text), ddlComune.SelectedValue, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then

            'correzione
            If ClsUtility.CAP_VERIFICA(Session("Conn"), strMiaCausale, bandiera, Trim(txtCap.Text), ddlComune.SelectedValue, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then

                indirizzoErratoHelios = True
                'Ripristino lo stato del tasto
                'cmdSalva.Visible = True
                'Inserisco il Messaggio di Errore
                msgErrore.Text = strMiaCausale + "</br>"
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing

                End If
                Log.Warning(LogEvent.SEDI_ART2_MODIFICA_ERRATA, strMiaCausale)
                'campiValidi = False
            End If
        Else

            If ChkEstero.Checked And ddlComune.SelectedValue = False And ddlProvincia.SelectedValue = True Then
                ddlComune.ClearSelection()
                ddlComune.Items.FindByText(ddlProvincia.SelectedItem.Text).Selected = True
            End If

        End If
        If (ddlTitoloGiuridico.SelectedItem.Value <= 0) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Titolo di disponibilità")
            campiValidi = False
        End If
        If ChkEstero.Checked = False And ddlComune.SelectedValue <> 0 Then
            If regioneCompetenza <> 22 AndAlso regioneCompetenza <> getRegioneCompetenzaComune(ddlComune.SelectedValue) Then
                msgErrore.Text = msgErrore.Text + "Il comune non appartiene alla regione di competenza.</br>"
                campiValidi = False
            End If
        End If
        If ChkEstero.Checked Then
            If ddlTitoloGiuridico.SelectedValue = 7 Then
                Dim LSE As Allegato = Session("LoadedLSE")
                If LSE Is Nothing Then
                    msgErrore.Text = msgErrore.Text + "L'allegato Lettera di accordo con sede estera è obbligatorio<br/>"
                    campiValidi = False
                End If
                If TxtSoggettoCapoSede.Text = "" Then
                    msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Soggetto estero cui è in capo la Sede")
                    '   "Il campo Soggetto estero cui è in capo la Sede è Obbligatorio<br/>"
                    campiValidi = False
                End If
                If rbSE1.Checked = False And rbSE2.Checked = False Then
                    msgErrore.Text = msgErrore.Text + "La dichiarazione di Non Disponibilità della sede è obbligatoria<br/>"
                    campiValidi = False
                End If
            Else
                If rbNoSE1.Checked = False And rbNoSE2.Checked = False Then
                    msgErrore.Text = msgErrore.Text + "La dichiarazione di avere nella proria disponibilità la sede è obbligatoria<br/>"
                    campiValidi = False
                End If

            End If
        Else
            If ChkTutela.Checked = False Then
                msgErrore.Text = msgErrore.Text + "La dichiarazione di rispetto della normativa a tutela della salute è obbligatoria</br>"
                campiValidi = False
            End If
        End If
        Return campiValidi
    End Function

    Private Function ValidaInteri(ByVal valore As String, ByVal nomeCampo As String) As Boolean
        Dim numero As Int32
        Dim campiValidi As Boolean = True
        Dim messaggioNumeroNonValido As String = "Il valore di '{0}' non è valido. Inserire solo numeri.<br/>"

        If (Int32.TryParse(valore, numero) = False) Then
            msgErrore.Text = msgErrore.Text + String.Format(messaggioNumeroNonValido, nomeCampo)
            campiValidi = False
        End If
        Return campiValidi

    End Function

    Private Sub CancellaMessaggi()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager _
             .ConnectionStrings("unscproduzionenewConnectionString").ConnectionString


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

    Sub EvidenziaDatiModificati(ByVal IdEnteSede As Integer)
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strsql As String
        Dim DsSedi As DataSet = New DataSet
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'popolamento della maschera dati DB

        strsql = " SELECT comuni.idComune,comuni.denominazione as Comune,provincie.provincia,StatiEntiSedi.statoentesede," & _
                 " entisediattuazioni.idEnteSedeAttuazione,tipisedi.tiposede," & _
                 " entisedi.Denominazione, entisedi.Indirizzo,entisedi.DettaglioRecapito, entisedi.civico, entisedi.cap, " & _
                 " isnull(entisedi.prefissoTelefono,'') as prefissoTelefono, isnull(entisedi.Telefono,'') as telefono," & _
                 " entisedi.Palazzina,entisedi.Piano,entisedi.scala,entisedi.interno, " & _
                 " isnull(entisedi.prefissofax,'') as prefissoFax, isnull(entisedi.fax,'') as fax ,entisediattuazioni.NMaxVolontari," & _
                 " entisedi.http, entisedi.email, TitoliGiuridici.TitoloGiuridico, entisedi.AltroTitoloGiuridico, " & _
                 " isnull(dbo.FormatoData(entisedi.DataStipulaContratto),'') as DataStipulaContratto,   " & _
                 " isnull(dbo.FormatoData(entisedi.DataScadenzaContratto),'') as DataScadenzaContratto,entisedi.CittaEstera  " & _
                 " FROM entisedi " & _
                 " INNER JOIN entisediattuazioni on entisedi.identesede = entisediattuazioni.identesede " & _
                 " INNER JOIN StatiEntiSedi on (entisedi.IdStatoEnteSede=dbo.StatiEntiSedi.IdStatoEnteSede) " & _
                 " INNER JOIN comuni on (comuni.idcomune=entisedi.idcomune) " & _
                 " INNER JOIN provincie on (provincie.idprovincia=comuni.idprovincia)" & _
                 " LEFT JOIN entisediTipi on (entisediTipi.identesede=entisedi.idEntesede)" & _
                 " INNER JOIN tipiSedi on entisediTipi.idtiposede = tipiSedi.idtiposede" & _
                 " INNER JOIN TitoliGiuridici on TitoliGiuridici.IdTitoloGiuridico = entisedi.IdTitoloGiuridico" & _
                 " WHERE entisedi.identeSede = " & IdEnteSede & ""

        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
        dtrGenerico.Read()
        If dtrGenerico.HasRows = True Then
            If Not IsDBNull(dtrGenerico("denominazione")) Then
                txtdenominazione.Text = dtrGenerico("denominazione")
            End If

            If Not IsDBNull(dtrGenerico("idEnteSedeAttuazione")) Then

            End If
            If Not IsDBNull(dtrGenerico("indirizzo")) Then
                If Not (String.Equals(txtIndirizzo.Text, dtrGenerico("indirizzo"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtIndirizzo)
                Else
                    helper.RipristinaStyleDatiModificati(txtIndirizzo)
                End If
            End If
            If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                If Not (String.Equals(TxtDettaglioRecapito.Text, dtrGenerico("DettaglioRecapito"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtDettaglioRecapito)
                Else
                    helper.RipristinaStyleDatiModificati(TxtDettaglioRecapito)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Palazzina")) Then
                If Not (String.Equals(TxtPalazzina.Text, dtrGenerico("Palazzina"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtPalazzina)
                Else
                    helper.RipristinaStyleDatiModificati(TxtPalazzina)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Civico")) Then
                If Not (String.Equals(txtCivico.Text, dtrGenerico("Civico"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtCivico)
                Else
                    helper.RipristinaStyleDatiModificati(txtCivico)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Scala")) Then
                If Not (String.Equals(TxtScala.Text, dtrGenerico("Scala"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtScala)
                Else
                    helper.RipristinaStyleDatiModificati(TxtScala)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Piano")) Then
                If Not (String.Equals(TxtPiano.Text, dtrGenerico("Piano"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtPiano)
                Else
                    helper.RipristinaStyleDatiModificati(TxtPiano)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Interno")) Then
                If Not (String.Equals(TxtInterno.Text, dtrGenerico("Interno"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtInterno)
                Else
                    helper.RipristinaStyleDatiModificati(TxtInterno)
                End If
            End If




            If Not IsDBNull(dtrGenerico("Cap")) Then
                If Not (String.Equals(txtCap.Text, dtrGenerico("Cap"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtCap)
                Else
                    helper.RipristinaStyleDatiModificati(txtCap)
                End If
            End If
            If Not IsDBNull(dtrGenerico("prefissoTelefono")) Then
                If Not (String.Equals(txtprefisso.Text, dtrGenerico("prefissoTelefono"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtprefisso)
                Else
                    helper.RipristinaStyleDatiModificati(txtprefisso)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Telefono")) Then
                If Not (String.Equals(txtTelefono.Text, dtrGenerico("telefono"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtTelefono)
                Else
                    helper.RipristinaStyleDatiModificati(txtTelefono)
                End If
            End If
            If Not IsDBNull(dtrGenerico("prefissoFax")) Then
                If Not (String.Equals(txtPrefFax.Text, dtrGenerico("prefissoFax"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtPrefFax)
                Else
                    helper.RipristinaStyleDatiModificati(txtPrefFax)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Fax")) Then
                If Not (String.Equals(txtfax.Text, dtrGenerico("Fax"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtfax)
                Else
                    helper.RipristinaStyleDatiModificati(txtfax)
                End If
            End If


            If ChkEstero.Checked = True Then
                If Not IsDBNull(dtrGenerico("CittaEstera")) Then
                    If Not (String.Equals(txtCity.Text, dtrGenerico("CittaEstera"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(txtCity)
                    Else
                        helper.RipristinaStyleDatiModificati(txtCity)
                    End If
                End If
            End If
        End If
        ChiudiDataReader(dtrGenerico)

    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        CancellaMessaggi()
        FilModificaSede.Visible = False
        CaricaGriglia()
    End Sub

    Private Sub CaricaGriglia()
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_ELENCO_SEDI_ART2]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.VarChar).Value = Session("IdEnte")
            sqlDAP.SelectCommand.Parameters.Add("@idEnteFase", SqlDbType.VarChar).Value = lblRifFase.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceSede", SqlDbType.VarChar).Value = txtCodSedeAtt.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@ComuneSede", SqlDbType.VarChar).Value = txtComune.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@NomeEnteRif", SqlDbType.VarChar).Value = txtDenominazioneEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnteRif", SqlDbType.VarChar).Value = TxtCodiceRegione.Text.Replace("'", "''")


            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            dgRisultatoRicerca.DataSource = dataSet
            dgRisultatoRicerca.DataBind()
            GestisciAlert()

        Catch ex As Exception
        Response.Write(ex.Message.ToString())
        Exit Sub
        End Try
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'CaricaDataGrid(dgRisultatoRicerca)
        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dgRisultatoRicerca.DataBind()
        GestisciAlert()

    End Sub

    Protected Sub cmdModifica_Click(sender As Object, e As EventArgs) Handles cmdModifica.Click
        SalvaDati()

        'Dim ListaAnomalie As String


        'CancellaMessaggi()

        'If VerificaValiditaCampi(AlboEnte) = False Then
        '    'c'e un problema
        '    Exit Sub
        'Else
        '    VerificaCap()
        '    If msgErrore.Text <> "" Then
        '        Exit Sub
        '    End If
        '    If ControlloDoppioneSedeNuovaGlobale(txtidsede.Value, txtIndirizzo.Text, txtCivico.Text, ddlComune.SelectedItem.Value, Trim(TxtPalazzina.Text), Trim(TxtScala.Text), Trim(TxtPiano.Text), Trim(TxtInterno.Text)) = True Then
        '        msgErrore.Text = "L'indirizzo risulta gia' utilizzato dall'Ente in lavorazione o da altro Ente. Impossibile effettuare il salvataggio."
        '        Exit Sub
        '    End If
        '    Dim esito As Integer
        '    If Session("Procedi") = 0 And Session("ProcediGM") = 0 Then
        '        esito = ControllaNomeIndirizzoSede(ListaAnomalie)
        '        Select Case esito
        '            Case 0
        '                Session("AnomaliaNome") = 0
        '                Session("AnomaliaIndirizzo") = 0
        '            Case 1
        '                Session("AnomaliaNome") = 1
        '                Session("AnomaliaIndirizzo") = 0
        '            Case 2
        '                Session("AnomaliaNome") = 0
        '                Session("AnomaliaIndirizzo") = 1
        '            Case 3
        '                Session("AnomaliaNome") = 1
        '                Session("AnomaliaIndirizzo") = 1
        '            Case Else
        '                'inserire cosa fare se va in errore il controllo nome indirizzo
        '        End Select

        '        If esito = 1 Or esito = 3 Then 'se risultano anomalie per il nome
        '            ShowPopUPControllo = "1"
        '            ListaAnomalie = Replace(ListaAnomalie, "|", "<br/>")
        '            lblErroreControlloSede.Text = ListaAnomalie
        '            lblErroreControlloSede.Visible = True
        '            divSpiegazioni.Visible = True
        '            'If ChkEstero.Checked = True Then
        '            '    rowLSE.Visible = True
        '            'End If
        '            Exit Sub
        '        End If
        '        'If esito = 2 Or esito = 3 Then
        '        '    msgErrore.Text = msgErrore.Text + "Attenzione: a questo indirizzo risultano già presenti altre sedi<br/>"
        '        '    Exit Sub
        '        'End If
        '    Else
        '        ShowPopUPControllo = ""
        '        Session("Procedi") = 0
        '        lblErroreControlloSede.Text = ""
        '        lblErroreControlloSede.Visible = False
        '    End If

        '    '*****controlla l'indirizzo su googlemaps solo nel caso in cui viene rilevato un errore di indirizzo
        '    'If esito = 4 Or (Session("ProcediGM") = 0 And indirizzoErratoHelios) Then
        '    '    Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br>"
        '    '    'Dim MessaggioErrore As String = "{0} - Indirizzo trovato da Google: [{1}]"
        '    '    Dim MessaggioErroreNonTrovato As String = "Google non ha trovato questo indirizzo."
        '    '    Dim MessaggioErroreDiverso As String = "Google suggerisce questo altro indirizzo:<br/>{0}.<br/>"
        '    '    Dim MessaggioCorretto As String = "L'indirizzo appare corretto - Indirizzo trovato da Google: [{0}]"
        '    '    CancellaMessaggi()
        '    '    If (ddlProvincia.SelectedItem.Text = String.Empty) Then
        '    '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
        '    '    End If

        '    '    If (ddlComune.SelectedItem.Text = String.Empty And ChkEstero.Checked = False) Then
        '    '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Comune")
        '    '    End If
        '    '    If (txtIndirizzo.Text = String.Empty) Then
        '    '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
        '    '    End If
        '    '    If (txtCivico.Text = String.Empty) Then
        '    '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
        '    '    End If
        '    '    If (txtCap.Text = String.Empty And ChkEstero.Checked = False) Then
        '    '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
        '    '    End If

        '    '    Dim gm As New GoogleMaps(ConfigurationSettings.AppSettings("GoogleKey"))
        '    '    Dim stato As String
        '    '    If ChkEstero.Checked Then
        '    '        stato = RitornaCodiceStato(ddlProvincia.SelectedItem.Text)
        '    '    Else
        '    '        stato = "IT"
        '    '    End If

        '    '    Dim localita As String = ddlComune.SelectedItem.Text
        '    '    If localita = "" Then localita = txtCity.Text
        '    '    If ChkEstero.Checked Then localita = txtCity.Text
        '    '    Dim b As Boolean = gm.checkAddress(stato, localita, txtIndirizzo.Text, txtCivico.Text, txtCap.Text)

        '    '    '    'If b Then
        '    '    '    lblGeolocalizzazione.Text = String.Format(MessaggioCorretto, gm.GoogleFormattedAddress)
        '    '    '    lblErrGeolocalizzazione.Text = ""
        '    '    '    lblGeolocalizzazione.Visible = True
        '    '    '    lblErrGeolocalizzazione.Visible = False
        '    '    'Else
        '    '    If b = False Then
        '    '        'If gm.GoogleFormattedAddress = "" Then
        '    '        '    lblErrGeolocalizzazione.Text = gm.lastError
        '    '        'Else
        '    '        '    lblErrGeolocalizzazione.Text = gm.lastError & "<br>" & String.Format(MessaggioErroreDiverso, gm.GoogleFormattedAddress)
        '    '        'End If

        '    '        'lblGeolocalizzazione.Text = ""
        '    '        'lblGeolocalizzazione.Visible = False
        '    '        'lblErrGeolocalizzazione.Visible = True

        '    '        ShowPopUPControllo = 2
        '    '        Session("AnomaliaIndirizzoGM") = 1
        '    '        'If ChkEstero.Checked = True Then
        '    '        '    rowLSE.Visible = True
        '    '        'End If
        '    '        lblGeolocalizzazione.Text = ""
        '    '        lblGeolocalizzazione.Visible = False
        '    '        Dim messaggioGoogle As String = "<Strong>ATTENZIONE :</strong></br>L’indirizzo digitato non trova riscontro in Google Maps. Si prega di controllare la correttezza di quanto inserito.</br>In caso di permanenza dell’anomalia, il Dipartimento si riserva di effettuare tutti i successivi controlli di merito.</strong>"
        '    '        If gm.GoogleFormattedAddress <> "" Then messaggioGoogle &= "<br>Google Maps suggerisce: <b>" & gm.GoogleFormattedAddress & "</b>"
        '    '        lblErrGeolocalizzazione.Text = messaggioGoogle
        '    '        lblErrGeolocalizzazione.Visible = True
        '    '        Exit Sub
        '    '    Else
        '    '        lblErrGeolocalizzazione.Text = ""
        '    '        lblGeolocalizzazione.Text = ""
        '    '        lblGeolocalizzazione.Visible = True
        '    '        lblErrGeolocalizzazione.Visible = False
        '    '        ShowPopUPControllo = 0
        '    '        esito = 0
        '    '        Session("AnomaliaIndirizzoGM") = 0
        '    '        Me.indirizzoOkGoogle = True
        '    '    End If
        '    'Else
        '    '    indirizzoOkGoogle = True
        '    '    Session("ProcediGM") = 0
        '    '    lblErrGeolocalizzazione.Text = ""
        '    '    lblGeolocalizzazione.Text = ""
        '    '    lblGeolocalizzazione.Visible = False
        '    '    lblErrGeolocalizzazione.Visible = False
        '    '    ShowPopUPControllo = 0
        '    'End If

        '    If esito = 4 Or (Session("ProcediGM") = 0) Then
        '        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br>"
        '        'Dim MessaggioErrore As String = "{0} - Indirizzo trovato da Google: [{1}]"
        '        Dim MessaggioErroreNonTrovato As String = "Google non ha trovato questo indirizzo."
        '        Dim MessaggioErroreDiverso As String = "Google suggerisce questo altro indirizzo:<br/>{0}.<br/>"
        '        Dim MessaggioCorretto As String = "L'indirizzo appare corretto - Indirizzo trovato da Google: [{0}]"
        '        Dim GmError As Boolean = False
        '        CancellaMessaggi()
        '        If (ddlProvincia.SelectedItem.Text = String.Empty) Then
        '            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
        '        End If

        '        If (ddlComune.SelectedItem.Text = String.Empty And ChkEstero.Checked = False) Then
        '            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Comune")
        '        End If
        '        If (txtIndirizzo.Text = String.Empty) Then
        '            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
        '        End If
        '        If (txtCivico.Text = String.Empty) Then
        '            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
        '        End If
        '        If (txtCap.Text = String.Empty And ChkEstero.Checked = False) Then
        '            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
        '        End If

        '        Dim gm As New GoogleMaps(ConfigurationSettings.AppSettings("GoogleKey"))
        '        Dim stato As String
        '        If ChkEstero.Checked Then
        '            stato = RitornaCodiceStato(ddlProvincia.SelectedItem.Text)
        '        Else
        '            stato = "IT"
        '        End If

        '        Dim localita As String = ddlComune.SelectedItem.Text
        '        If localita = "" Then localita = txtCity.Text
        '        If ChkEstero.Checked Then localita = txtCity.Text
        '        Dim b As Boolean = gm.checkAddress(stato, localita, txtIndirizzo.Text, txtCivico.Text, txtCap.Text)

        '        '    'If b Then
        '        '    lblGeolocalizzazione.Text = String.Format(MessaggioCorretto, gm.GoogleFormattedAddress)
        '        '    lblErrGeolocalizzazione.Text = ""
        '        '    lblGeolocalizzazione.Visible = True
        '        '    lblErrGeolocalizzazione.Visible = False
        '        'Else
        '        If b = False Then
        '            If indirizzoErratoHelios = False And strMiaCausale.ToUpper.Trim = "CAP ESISTENTE PER COMUNE ED INDIRIZZO" And gm.lastError.ToUpper.Trim = "CIVICO NON ESISTENTE" Then
        '                lblGeolocalizzazione.Text = ""
        '                lblGeolocalizzazione.Visible = False
        '                lblErrGeolocalizzazione.Text = gm.lastError
        '                lblErrGeolocalizzazione.Visible = True
        '                GmError = True
        '            ElseIf indirizzoErratoHelios = False And (strMiaCausale.ToUpper.Trim = "CAP ESISTENTE PER COMUNE SENZA INDIRIZZI" _
        '                    Or strMiaCausale.ToUpper.Trim = "COMUNE ESTERO" Or strMiaCausale.ToUpper.Trim = "COMUNE NON TROVATO") Then
        '                lblGeolocalizzazione.Text = ""
        '                lblGeolocalizzazione.Visible = False
        '                Dim messaggioGoogle As String = "<Strong>ATTENZIONE :</strong></br>L’indirizzo digitato non trova riscontro in Google Maps. Si prega di controllare la correttezza di quanto inserito.</br>In caso di permanenza dell’anomalia, il Dipartimento si riserva di effettuare tutti i successivi controlli di merito.</strong>"
        '                If gm.GoogleFormattedAddress <> "" Then messaggioGoogle &= "<br>Google Maps suggerisce: <b>" & gm.GoogleFormattedAddress & "</b>"
        '                lblErrGeolocalizzazione.Text = messaggioGoogle
        '                lblErrGeolocalizzazione.Visible = True
        '                GmError = True
        '            End If
        '            'If gm.GoogleFormattedAddress = "" Then
        '            '    lblErrGeolocalizzazione.Text = gm.lastError
        '            'Else
        '            '    lblErrGeolocalizzazione.Text = gm.lastError & "<br>" & String.Format(MessaggioErroreDiverso, gm.GoogleFormattedAddress)
        '            'End If

        '            'lblGeolocalizzazione.Text = ""
        '            'lblGeolocalizzazione.Visible = False
        '            'lblErrGeolocalizzazione.Visible = True
        '            If GmError = True Then
        '                ShowPopUPControllo = 2
        '                Session("AnomaliaIndirizzoGM") = 1
        '                Exit Sub
        '            End If

        '            'If ChkEstero.Checked = True Then
        '            '    rowLSE.Visible = True
        '            'End If
        '            'lblGeolocalizzazione.Text = ""
        '            'lblGeolocalizzazione.Visible = False
        '            'Dim messaggioGoogle As String = "<Strong>ATTENZIONE :</strong></br>L’indirizzo digitato non trova riscontro in Google Maps. Si prega di controllare la correttezza di quanto inserito.</br>In caso di permanenza dell’anomalia, il Dipartimento si riserva di effettuare tutti i successivi controlli di merito.</strong>"
        '            'If gm.GoogleFormattedAddress <> "" Then messaggioGoogle &= "<br>Google Maps suggerisce: <b>" & gm.GoogleFormattedAddress & "</b>"
        '            'lblErrGeolocalizzazione.Text = messaggioGoogle
        '            'lblErrGeolocalizzazione.Visible = True

        '        Else
        '            lblErrGeolocalizzazione.Text = ""
        '            lblGeolocalizzazione.Text = ""
        '            lblGeolocalizzazione.Visible = True
        '            lblErrGeolocalizzazione.Visible = False
        '            ShowPopUPControllo = 0
        '            esito = 0
        '            Session("AnomaliaIndirizzoGM") = 0
        '            Me.indirizzoOkGoogle = True
        '        End If
        '    Else
        '        indirizzoOkGoogle = True
        '        Session("ProcediGM") = 0
        '        lblErrGeolocalizzazione.Text = ""
        '        lblGeolocalizzazione.Text = ""
        '        lblGeolocalizzazione.Visible = False
        '        lblErrGeolocalizzazione.Visible = False
        '        ShowPopUPControllo = 0
        '    End If

        '    ModificaSedeEnte()
        'End If
        'CaricaGriglia()
    End Sub
    Private Sub CaricoComboTitoloGiuridico(ByVal AlboEnte As String)
        'Agg. da Simona Cordella il 19/05/2009
        'Caricamento combo TitoloGiuridico (T
        Dim dtrTG As SqlClient.SqlDataReader

        If Not dtrTG Is Nothing Then
            dtrTG.Close()
            dtrTG = Nothing
        End If

        'strsql = " SELECT  0 AS IdTitoloGiuridico, '' AS TitoloGiuridico  "
        'strsql = strsql & " From TitoliGiuridici "
        'strsql = strsql & " Union "
        'strsql = strsql & " SELECT IdTitoloGiuridico, TitoloGiuridico "
        'strsql = strsql & " From TitoliGiuridici "
        strsql = " SELECT IdTitoloGiuridico, TitoloGiuridico "
        strsql = strsql & " From TitoliGiuridici "

        If AlboEnte = "" Then
            strsql &= " WHERE (ALBO='SCU' OR ALBO IS NULL) "
        Else
            strsql &= " WHERE (ALBO='" & AlboEnte & "' OR ALBO IS NULL) "
        End If

        strsql = strsql & " Order by IdTitoloGiuridico"

        'eseguo la query
        dtrTG = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        'assegno il datadearder alla combo caricando così descrizione e id
        ddlTitoloGiuridico.DataSource = dtrTG
        'ddlTitoloGiuridico.Items.Add("")
        ddlTitoloGiuridico.DataTextField = "TitoloGiuridico"
        ddlTitoloGiuridico.DataValueField = "IdTitoloGiuridico"
        ddlTitoloGiuridico.DataBind()
        If Not dtrTG Is Nothing Then
            dtrTG.Close()
            dtrTG = Nothing
        End If
    End Sub

    Private Function VerificaAbilitazione(ByVal IdEnte As String, ByVal IdEnteFase As String, ByVal Conn As SqlClient.SqlConnection) As Boolean

        '** Verifico se l'ente in sessione è coerente con la fase selezionata
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT identefase from entifasi where idente = " & IdEnte & " and identefase = " & IdEnteFase & " and getdate() between datainiziofase and datafinefase"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Conn)

        VerificaAbilitazione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return VerificaAbilitazione
    End Function

    Private Sub ddlTitoloGiuridico_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTitoloGiuridico.SelectedIndexChanged
        Dim blnObbligoScadenza As Boolean
        If AlboEnte = "SCN" Then
            If ddlTitoloGiuridico.SelectedItem.Text = "Altro" Then
                txtAltroTitoloGiuridicio.ReadOnly = False
            Else
                txtAltroTitoloGiuridicio.Text = ""
                txtAltroTitoloGiuridicio.ReadOnly = True
            End If
        Else

            ' blnObbligoScadenza = TitoloGiuridico_ObbligoScandeza(ddlTitoloGiuridico.SelectedValue)

        End If
        GestisciDivEstero()
        MaintainScrollPositionOnPostBack = True
    End Sub
    Sub GestisciDivEstero()
        If ChkEstero.Checked = True Then
            lblComune.Text = "Localit&agrave;"
            lblCap.Text = "Codice localit&agrave;"
            city.Visible = True
            ddlComune.Visible = False
            txtLocalita.Visible = True
            DivItalia.Visible = False
            DivEstero2.Visible = True
            If ddlTitoloGiuridico.SelectedValue = 7 Then
                DivSoggettoEstero.Visible = True
                DivNoSoggettoEstero.Visible = False
            Else
                DivSoggettoEstero.Visible = False
                DivNoSoggettoEstero.Visible = True
            End If
            DivItalia.Visible = False
            If Session("LoadedLSE") Is Nothing Then
                rowNoLSE.Visible = True
                rowLSE.Visible = False
            Else
                rowNoLSE.Visible = False
                rowLSE.Visible = True
            End If
            DivEstero2.Visible = True
        Else
            DivItalia.Visible = True
            DivEstero2.Visible = False
            city.Visible = False
            ddlComune.Visible = True
            txtLocalita.Visible = False
            lblComune.Text = "<strong>(*)</strong>Comune"
            lblCap.Text = "<strong>(*)</strong>C.A.P."
        End If



    End Sub

    Private Sub imgGoogleMaps_Click(sender As Object, e As ImageClickEventArgs) Handles imgGoogleMaps.Click
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br>"
        Dim MessaggioErrore As String = "{0} - Indirizzo trovato da Google: [{1}]"
        Dim MessaggioCorretto As String = "L'indirizzo appare corretto - Indirizzo trovato da Google: [{0}]"
        CancellaMessaggi()
        If (ddlProvincia.SelectedItem.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
        End If
        If (ddlComune.SelectedItem.Text = String.Empty And ChkEstero.Checked = False) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Comune")
        End If
        If (txtIndirizzo.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
        End If
        If (txtCivico.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
        End If
        If (txtCap.Text = String.Empty And ChkEstero.Checked = False) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
        End If
        Dim gm As New GoogleMaps(ConfigurationSettings.AppSettings("GoogleKey"))

        If msgErrore.Text = "" Then
            ShowPopUPControllo = 3
            IndirizzoRicerca = txtIndirizzo.Text & " " & txtCivico.Text & " " & txtCap.Text & " " & ddlComune.SelectedItem.Text & " " & ddlProvincia.SelectedItem.Text
            'Dim script As String = "<SCRIPT>" & vbCrLf & "window.open('" & lnk & "','_blank');" & vbCrLf & "</SCRIPT>"
            Dim script = "<script src=""https://maps.googleapis.com/maps/api/js?key=" & ConfigurationSettings.AppSettings("GoogleKey") & "&callback=initMap&libraries=places&v=weekly"" async></script>"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "IndirizzoGoogle", script)

        End If

    End Sub

    Private Sub MessaggiPopup(ByVal strMessaggio)
        lblErroreUpload.Visible = True
        lblErroreUpload.Text = strMessaggio
        popUpload.Show()
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
    Private Sub MessaggiSuccess(ByVal strMessaggio)
        msgErrore.Visible = True
        msgErrore.CssClass = "msgInfo"
        msgErrore.Text = strMessaggio
    End Sub

    Private Sub cmdAllega_Click(sender As Object, e As EventArgs) Handles cmdAllega.Click
        'Verifica se è stato inserito il file
        CancellaMessaggi()
        If fileLSE.PostedFile Is Nothing Or String.IsNullOrEmpty(fileLSE.PostedFile.FileName) Then
            MessaggiPopup("Non è stato scelto nessun file per il caricamento della Lettera di accordo con sede estera")
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileLSE) = False Then
            MessaggiPopup("Il formato file della Lettera di accordo con sede estera non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M")
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileLSE.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            MessaggiPopup("Attenzione. Impossibile caricare documento vuoto.")
            Exit Sub
        End If
        If iLen > 20971520 Then
            MessaggiPopup("Attenzione. La dimensione massima della Lettera di accordo con sede estera è di 20 MB.")
            Exit Sub
        End If
        Dim numBytesToRead As Integer = CType(fs.Length, Integer)
        Dim numBytesRead As Integer = 0

        While (numBytesToRead > 0)
            ' Read may return anything from 0 to numBytesToRead.
            Dim n As Integer = fs.Read(bBLOBStorage, numBytesRead, numBytesToRead)
            ' Break when the end of the file is reached.
            If (n = 0) Then
                Exit While
            End If
            numBytesRead = (numBytesRead + n)
            numBytesToRead = (numBytesToRead - n)

        End While

        fs.Close()
        Dim NomeSede As String = Regex.Replace(txtdenominazione.Text.Trim, "[\\ \/ \:  \* \?  \"" <> \|]", String.Empty).ToUpper
        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileLSE.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        Dim esito As Boolean
        esito = ControllaHV(hashValue)
        If esito Then
            Session("LoadLSEId") = Nothing
            'Salvo File In Sessione
            Dim LSE As New Allegato() With {
             .Id = Session("LoadLSEId"),
             .Updated = True,
             .Blob = bBLOBStorage,
             .Filename = "LDA_" & NomeSede & estensione,
             .Hash = hashValue,
             .Filesize = iLen,
            .DataInserimento = Date.Now
            }
            Session("LoadedLSE") = LSE
            'Se Il LSE è caricato in Sessione (Inserimento)
            rowNoLSE.Visible = False
            rowLSE.Visible = True
            txtLSEFilename.Text = LSE.Filename
            txtLSEHash.Text = LSE.Hash
            txtLSEData.Text = LSE.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
            MessaggiSuccess("Lettera di accordo con sede estera caricata correttamente")
        Else
            msgErrore.Text = "Attenzione. File già presente per questo ente"
        End If
    End Sub
    Function ControllaHV(hvControllo As String) As Boolean
        'ControllaHV =
        Dim esito As Integer = 0
        'Recupero File da DB
        Try
            Dim SqlCmd As SqlClient.SqlCommand


            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "Controlla_HashValue"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@Hashvalue", SqlDbType.VarChar, 100).Value = hvControllo
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")

            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@Esito"
            sparam.SqlDbType = SqlDbType.TinyInt
            sparam.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam)

            SqlCmd.ExecuteNonQuery()
            If SqlCmd.Parameters("@Esito").Value() = 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function getRegioneCompetenzaEnte() As Integer
        Dim strSql = "select t2.idRegioneCompetenza from enti t1 inner join sezionialboscu t2 on t2.idSezione = t1.idSezione where t1.idEnte=" & Session("IdEnte")
        Dim dtrLocal As SqlClient.SqlDataReader = ClsServer.CreaDatareader(strSql, Session("conn"))
        getRegioneCompetenzaEnte = 0
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            getRegioneCompetenzaEnte = dtrLocal("idRegioneCompetenza")
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
    End Function
    Private Function getRegioneCompetenzaComune(ByVal IDComune As Integer) As Integer
        ' IDComune = Replace(codiceIstat, "'", "")
        'Dim strSql = "select coalesce(IDRegione, -1) IdRegione from comuni t1 inner join provincie t2 on t2.idprovincia=t1.idprovincia where t1.IDComune='" & IDComune & "'"
        Dim strSql = "select coalesce(idRegioneCompetenza, -1) idRegioneCompetenza from comuni t1 inner join provincie t2 on t2.idprovincia=t1.idprovincia where t1.IDComune=" & IDComune
        Dim dtrLocal As SqlClient.SqlDataReader = ClsServer.CreaDatareader(strSql, Session("conn"))
        getRegioneCompetenzaComune = 0
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            getRegioneCompetenzaComune = dtrLocal("idRegioneCompetenza")
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
    End Function
    Protected Sub btnDownloadLSE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadLSE.Click
        If Session("LoadedLSE") Is Nothing Then
            msgErrore.Text = "Nessun File caricato"
            ClearSessionLSE()
            Log.Warning(LogEvent.SEDI_ART2_DOWNLOAD_LSE, "Nessun file Caricato")
            Exit Sub
        End If
        Log.Information(LogEvent.SEDI_ART2_DOWNLOAD_LSE)
        Dim LSE As Allegato = Session("LoadedLSE")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & LSE.Filename)
        Response.BinaryWrite(LSE.Blob)
        Response.End()
    End Sub
    Private Sub ClearSessionLSE()
        Session("LoadedLSE") = Nothing
        'Session("LoadLSEId") = Nothing
        rowNoLSE.Visible = True
        rowLSE.Visible = False
    End Sub

    Protected Sub btnEliminaLSE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaLSE.Click
        ClearSessionLSE()
    End Sub

    Private Function RitornaCodiceStato(Nome As String) As String
        Dim TCodici As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        TCodici = New DataTable
        RitornaCodiceStato = ""

        SqlCmd.CommandText = "SELECT Codice FROM nazioni WHERE  (Nazione = '" & Nome & "')"
        SqlCmd.CommandType = CommandType.Text
        SqlCmd.Connection = Session("conn")
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(TCodici)
        If TCodici.Rows.Count > 0 Then
            RitornaCodiceStato = TCodici(0).Item("Codice").ToString
        End If
    End Function
    Public Function ControllaNomeIndirizzoSede(ByRef Messaggio As String) As Integer
        Dim SqlCmd As SqlClient.SqlCommand
        Dim test2 As Integer
        Dim test As String
        test = Session("IdEnte")
        test2 = CInt(Session("IdEnte"))
        Messaggio = ""
        Try
            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "ControllaNomeIndirizzoSede"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = CInt(Session("IdEnte"))




            SqlCmd.Parameters.Add("@NomeSede", SqlDbType.VarChar, 50).Value = txtdenominazione.Text
            If lblPersonalizza.Value = "Inserimento" Then
                SqlCmd.Parameters.Add("@idEnteSede", SqlDbType.Int).Value = 0
            Else
                SqlCmd.Parameters.Add("@idEnteSede", SqlDbType.Int).Value = CInt(txtCodice.Value)
            End If
            SqlCmd.Parameters.Add("@Indirizzo", SqlDbType.VarChar, 255).Value = txtIndirizzo.Text
            SqlCmd.Parameters.Add("@Civico", SqlDbType.VarChar, 50).Value = txtCivico.Text
            SqlCmd.Parameters.Add("@Cap", SqlDbType.VarChar, 10).Value = txtCap.Text
            SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = ddlComune.SelectedValue
            SqlCmd.Parameters.Add("@Palazzina", SqlDbType.VarChar, 10).Value = TxtPalazzina.Text
            SqlCmd.Parameters.Add("@Scala", SqlDbType.VarChar, 10).Value = TxtScala.Text
            SqlCmd.Parameters.Add("@Piano", SqlDbType.Int).Value = TxtPiano.Text
            SqlCmd.Parameters.Add("@Interno", SqlDbType.VarChar, 10).Value = TxtInterno.Text
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.SqlDbType = SqlDbType.Int
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.SqlDbType = SqlDbType.VarChar
            sparam2.Size = 255
            sparam2.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam2)

            SqlCmd.ExecuteNonQuery()
            If SqlCmd.Parameters("@Esito").Value > 0 Then
                Messaggio = SqlCmd.Parameters("@Messaggio").Value
            End If
            Return SqlCmd.Parameters("@Esito").Value

        Catch ex As Exception
            Messaggio = "Errore nel controllo formale di nome e indirizzo"
            Return 4
        End Try
    End Function
    Private Sub GestisciAlert()

        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim img As ImageButton
        For Each item In dgRisultatoRicerca.Items
            img = DirectCast(item.FindControl("IdImgAlert"), ImageButton)
            img.Visible = False
            img.ToolTip = ""
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_ANOMALIA_NOME).Text = "True" Then
                img.Visible = True
                img.ToolTip = "Il nome presenta anomalie o è duplicato" + vbNewLine
            End If
            'If dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_ANOMALIA_INDIRIZZO).Text = "True" Then
            '    img.Visible = True
            '    img.ToolTip = img.ToolTip + "Indirizzo corrispondente ad altra sede" + vbNewLine
            'End If
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_ANOMALIA_INDIRIZZO_GOOGLE).Text = "True" Then
                img.Visible = True
                img.ToolTip += "Indirizzo non corrispondente a quello trovato da Google"
            End If
            img.Enabled = False
        Next
    End Sub

    Private Sub cmdProcediGeolocalizzazione_Click(sender As Object, e As EventArgs) Handles cmdProcediGeolocalizzazione.Click
        Session("ProcediGM") = 1
        procediGM = 1
        SalvaDati()
    End Sub

    Private Sub cmdProcedi_Click(sender As Object, e As EventArgs) Handles cmdProcedi.Click
        Session("Procedi") = 1
        SalvaDati()
    End Sub
    Sub SalvaDati()
        Dim ListaAnomalie As String


        CancellaMessaggi()

        If VerificaValiditaCampi(AlboEnte) = False Then
            'c'e un problema
            Exit Sub
        Else
            VerificaCap()
            If msgErrore.Text <> "" Then
                Exit Sub
            End If
            If ControlloDoppioneSedeNuovaGlobale(txtidsede.Value, txtIndirizzo.Text, txtCivico.Text, ddlComune.SelectedItem.Value, Trim(TxtPalazzina.Text), Trim(TxtScala.Text), Trim(TxtPiano.Text), Trim(TxtInterno.Text)) = True Then
                msgErrore.Text = "L'indirizzo risulta gia' utilizzato dall'Ente in lavorazione o da altro Ente. Impossibile effettuare il salvataggio."
                Exit Sub
            End If
            Dim esito As Integer
            If Session("Procedi") = 0 And Session("ProcediGM") = 0 Then
                esito = ControllaNomeIndirizzoSede(ListaAnomalie)
                Select Case esito
                    Case 0
                        Session("AnomaliaNome") = 0
                        Session("AnomaliaIndirizzo") = 0
                    Case 1
                        Session("AnomaliaNome") = 1
                        Session("AnomaliaIndirizzo") = 0
                    Case 2
                        Session("AnomaliaNome") = 0
                        Session("AnomaliaIndirizzo") = 1
                    Case 3
                        Session("AnomaliaNome") = 1
                        Session("AnomaliaIndirizzo") = 1
                    Case Else
                        'inserire cosa fare se va in errore il controllo nome indirizzo
                End Select

                If esito = 1 Or esito = 3 Then 'se risultano anomalie per il nome
                    ShowPopUPControllo = "1"
                    ListaAnomalie = Replace(ListaAnomalie, "|", "<br/>")
                    lblErroreControlloSede.Text = ListaAnomalie
                    lblErroreControlloSede.Visible = True
                    divSpiegazioni.Visible = True
                    'If ChkEstero.Checked = True Then
                    '    rowLSE.Visible = True
                    'End If
                    Exit Sub
                End If
                'If esito = 2 Or esito = 3 Then
                '    msgErrore.Text = msgErrore.Text + "Attenzione: a questo indirizzo risultano già presenti altre sedi<br/>"
                '    Exit Sub
                'End If
            Else
                ShowPopUPControllo = ""
                Session("Procedi") = 0
                lblErroreControlloSede.Text = ""
                lblErroreControlloSede.Visible = False
            End If

            '*****controlla l'indirizzo su googlemaps solo nel caso in cui viene rilevato un errore di indirizzo
            'If esito = 4 Or (Session("ProcediGM") = 0 And indirizzoErratoHelios) Then
            '    Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br>"
            '    'Dim MessaggioErrore As String = "{0} - Indirizzo trovato da Google: [{1}]"
            '    Dim MessaggioErroreNonTrovato As String = "Google non ha trovato questo indirizzo."
            '    Dim MessaggioErroreDiverso As String = "Google suggerisce questo altro indirizzo:<br/>{0}.<br/>"
            '    Dim MessaggioCorretto As String = "L'indirizzo appare corretto - Indirizzo trovato da Google: [{0}]"
            '    CancellaMessaggi()
            '    If (ddlProvincia.SelectedItem.Text = String.Empty) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
            '    End If

            '    If (ddlComune.SelectedItem.Text = String.Empty And ChkEstero.Checked = False) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Comune")
            '    End If
            '    If (txtIndirizzo.Text = String.Empty) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
            '    End If
            '    If (txtCivico.Text = String.Empty) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
            '    End If
            '    If (txtCap.Text = String.Empty And ChkEstero.Checked = False) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
            '    End If

            '    Dim gm As New GoogleMaps(ConfigurationSettings.AppSettings("GoogleKey"))
            '    Dim stato As String
            '    If ChkEstero.Checked Then
            '        stato = RitornaCodiceStato(ddlProvincia.SelectedItem.Text)
            '    Else
            '        stato = "IT"
            '    End If

            '    Dim localita As String = ddlComune.SelectedItem.Text
            '    If localita = "" Then localita = txtCity.Text
            '    If ChkEstero.Checked Then localita = txtCity.Text
            '    Dim b As Boolean = gm.checkAddress(stato, localita, txtIndirizzo.Text, txtCivico.Text, txtCap.Text)

            '    '    'If b Then
            '    '    lblGeolocalizzazione.Text = String.Format(MessaggioCorretto, gm.GoogleFormattedAddress)
            '    '    lblErrGeolocalizzazione.Text = ""
            '    '    lblGeolocalizzazione.Visible = True
            '    '    lblErrGeolocalizzazione.Visible = False
            '    'Else
            '    If b = False Then
            '        'If gm.GoogleFormattedAddress = "" Then
            '        '    lblErrGeolocalizzazione.Text = gm.lastError
            '        'Else
            '        '    lblErrGeolocalizzazione.Text = gm.lastError & "<br>" & String.Format(MessaggioErroreDiverso, gm.GoogleFormattedAddress)
            '        'End If

            '        'lblGeolocalizzazione.Text = ""
            '        'lblGeolocalizzazione.Visible = False
            '        'lblErrGeolocalizzazione.Visible = True

            '        ShowPopUPControllo = 2
            '        Session("AnomaliaIndirizzoGM") = 1
            '        'If ChkEstero.Checked = True Then
            '        '    rowLSE.Visible = True
            '        'End If
            '        lblGeolocalizzazione.Text = ""
            '        lblGeolocalizzazione.Visible = False
            '        Dim messaggioGoogle As String = "<Strong>ATTENZIONE :</strong></br>L’indirizzo digitato non trova riscontro in Google Maps. Si prega di controllare la correttezza di quanto inserito.</br>In caso di permanenza dell’anomalia, il Dipartimento si riserva di effettuare tutti i successivi controlli di merito.</strong>"
            '        If gm.GoogleFormattedAddress <> "" Then messaggioGoogle &= "<br>Google Maps suggerisce: <b>" & gm.GoogleFormattedAddress & "</b>"
            '        lblErrGeolocalizzazione.Text = messaggioGoogle
            '        lblErrGeolocalizzazione.Visible = True
            '        Exit Sub
            '    Else
            '        lblErrGeolocalizzazione.Text = ""
            '        lblGeolocalizzazione.Text = ""
            '        lblGeolocalizzazione.Visible = True
            '        lblErrGeolocalizzazione.Visible = False
            '        ShowPopUPControllo = 0
            '        esito = 0
            '        Session("AnomaliaIndirizzoGM") = 0
            '        Me.indirizzoOkGoogle = True
            '    End If
            'Else
            '    indirizzoOkGoogle = True
            '    Session("ProcediGM") = 0
            '    lblErrGeolocalizzazione.Text = ""
            '    lblGeolocalizzazione.Text = ""
            '    lblGeolocalizzazione.Visible = False
            '    lblErrGeolocalizzazione.Visible = False
            '    ShowPopUPControllo = 0
            'End If

            If esito = 4 Or (Session("ProcediGM") = 0) Then
                Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br>"
                'Dim MessaggioErrore As String = "{0} - Indirizzo trovato da Google: [{1}]"
                Dim MessaggioErroreNonTrovato As String = "Google non ha trovato questo indirizzo."
                Dim MessaggioErroreDiverso As String = "Google suggerisce questo altro indirizzo:<br/>{0}.<br/>"
                Dim MessaggioCorretto As String = "L'indirizzo appare corretto - Indirizzo trovato da Google: [{0}]"
                Dim GmError As Boolean = False
                CancellaMessaggi()
                If (ddlProvincia.SelectedItem.Text = String.Empty) Then
                    msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
                End If

                If (ddlComune.SelectedItem.Text = String.Empty And ChkEstero.Checked = False) Then
                    msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Comune")
                End If
                If (txtIndirizzo.Text = String.Empty) Then
                    msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
                End If
                If (txtCivico.Text = String.Empty) Then
                    msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
                End If
                If (txtCap.Text = String.Empty And ChkEstero.Checked = False) Then
                    msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
                End If

                Dim gm As New GoogleMaps(ConfigurationSettings.AppSettings("GoogleKey"))
                Dim stato As String
                If ChkEstero.Checked Then
                    stato = RitornaCodiceStato(ddlProvincia.SelectedItem.Text)
                Else
                    stato = "IT"
                End If

                Dim localita As String = ddlComune.SelectedItem.Text
                If localita = "" Then localita = txtCity.Text
                If ChkEstero.Checked Then localita = txtCity.Text
                Dim b As Boolean = gm.checkAddress(stato, localita, txtIndirizzo.Text, txtCivico.Text, txtCap.Text)

                '    'If b Then
                '    lblGeolocalizzazione.Text = String.Format(MessaggioCorretto, gm.GoogleFormattedAddress)
                '    lblErrGeolocalizzazione.Text = ""
                '    lblGeolocalizzazione.Visible = True
                '    lblErrGeolocalizzazione.Visible = False
                'Else
                If b = False Then
                    If indirizzoErratoHelios = False And strMiaCausale.ToUpper.Trim = "CAP ESISTENTE PER COMUNE ED INDIRIZZO" And gm.lastError.ToUpper.Trim = "CIVICO NON ESISTENTE" Then
                        lblGeolocalizzazione.Text = ""
                        lblGeolocalizzazione.Visible = False
                        lblErrGeolocalizzazione.Text = gm.lastError
                        lblErrGeolocalizzazione.Visible = True
                        GmError = True
                    ElseIf indirizzoErratoHelios = False And (strMiaCausale.ToUpper.Trim = "CAP ESISTENTE PER COMUNE SENZA INDIRIZZI" _
                            Or strMiaCausale.ToUpper.Trim = "COMUNE ESTERO" Or strMiaCausale.ToUpper.Trim = "COMUNE NON TROVATO") Then
                        lblGeolocalizzazione.Text = ""
                        lblGeolocalizzazione.Visible = False
                        Dim messaggioGoogle As String = "<Strong>ATTENZIONE :</strong></br>L’indirizzo digitato non trova riscontro in Google Maps. Si prega di controllare la correttezza di quanto inserito.</br>In caso di permanenza dell’anomalia, il Dipartimento si riserva di effettuare tutti i successivi controlli di merito.</strong>"
                        If gm.GoogleFormattedAddress <> "" Then messaggioGoogle &= "<br>Google Maps suggerisce: <b>" & gm.GoogleFormattedAddress & "</b>"
                        lblErrGeolocalizzazione.Text = messaggioGoogle
                        lblErrGeolocalizzazione.Visible = True
                        GmError = True
                    End If
                    'If gm.GoogleFormattedAddress = "" Then
                    '    lblErrGeolocalizzazione.Text = gm.lastError
                    'Else
                    '    lblErrGeolocalizzazione.Text = gm.lastError & "<br>" & String.Format(MessaggioErroreDiverso, gm.GoogleFormattedAddress)
                    'End If

                    'lblGeolocalizzazione.Text = ""
                    'lblGeolocalizzazione.Visible = False
                    'lblErrGeolocalizzazione.Visible = True
                    If GmError = True Then
                        ShowPopUPControllo = 2
                        Session("AnomaliaIndirizzoGM") = 1
                        Exit Sub
                    End If

                    'If ChkEstero.Checked = True Then
                    '    rowLSE.Visible = True
                    'End If
                    'lblGeolocalizzazione.Text = ""
                    'lblGeolocalizzazione.Visible = False
                    'Dim messaggioGoogle As String = "<Strong>ATTENZIONE :</strong></br>L’indirizzo digitato non trova riscontro in Google Maps. Si prega di controllare la correttezza di quanto inserito.</br>In caso di permanenza dell’anomalia, il Dipartimento si riserva di effettuare tutti i successivi controlli di merito.</strong>"
                    'If gm.GoogleFormattedAddress <> "" Then messaggioGoogle &= "<br>Google Maps suggerisce: <b>" & gm.GoogleFormattedAddress & "</b>"
                    'lblErrGeolocalizzazione.Text = messaggioGoogle
                    'lblErrGeolocalizzazione.Visible = True

                Else
                    lblErrGeolocalizzazione.Text = ""
                    lblGeolocalizzazione.Text = ""
                    lblGeolocalizzazione.Visible = True
                    lblErrGeolocalizzazione.Visible = False
                    ShowPopUPControllo = 0
                    esito = 0
                    Session("AnomaliaIndirizzoGM") = 0
                    Me.indirizzoOkGoogle = True
                End If
            Else
                indirizzoOkGoogle = True
                Session("ProcediGM") = 0
                lblErrGeolocalizzazione.Text = ""
                lblGeolocalizzazione.Text = ""
                lblGeolocalizzazione.Visible = False
                lblErrGeolocalizzazione.Visible = False
                ShowPopUPControllo = 0
            End If

            ModificaSedeEnte()
        End If
        CaricaGriglia()
    End Sub
    Private Sub CaricaLSE()
        '***Gestione LSE****
        If Session("LoadLSEId") IsNot Nothing Then
            'Se Il LSE è caricato nel DB
            'Recupero File da DB
            Dim sqlGetAllegato = "SELECT Top 1 FileName,HashValue,FileLength,BinData,DataInserimento From Allegato WHERE idAllegato = @idAllegato"
            Dim AllegatoCommand As New SqlCommand(sqlGetAllegato, Session("conn"))
            AllegatoCommand.Parameters.AddWithValue("@idAllegato", Session("LoadLSEId"))
            Dim dtrAllegato As SqlDataReader = AllegatoCommand.ExecuteReader()
            If dtrAllegato.Read Then
                Dim filename As String = dtrAllegato("FileName")
                Dim hashValue As String = dtrAllegato("HashValue")
                Dim filelength As Integer = CInt(dtrAllegato("FileLength"))
                Dim blob As Byte() = dtrAllegato("BinData")
                Dim dataInserimento As Date = dtrAllegato("DataInserimento")
                dtrAllegato.Close()
                AllegatoCommand.Dispose()
                rowNoLSE.Visible = False
                rowLSE.Visible = True
                txtLSEFilename.Text = filename
                txtLSEData.Text = dataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                Session("LoadedLSE") = New Allegato() With {
                 .Id = Session("LoadLSEId"),
                 .Blob = blob,
                 .Filename = filename,
                 .Hash = hashValue,
                 .Filesize = filelength,
                 .DataInserimento = dataInserimento
                }

            Else
                dtrAllegato.Close()
                ' ClearSessionLSE()
            End If
        End If
        Dim LSE As Allegato = Session("LoadedLSE")

        If LSE IsNot Nothing Then
            'Se Il LSE è caricato in Sessione (Inserimento)
            rowNoLSE.Visible = False
            rowLSE.Visible = True
            txtLSEFilename.Text = LSE.Filename
            txtLSEHash.Text = LSE.Hash
            txtLSEData.Text = LSE.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        Else
            'Se LSE non è ancora caricato
            rowNoLSE.Visible = True
            rowLSE.Visible = False
        End If
    End Sub
End Class