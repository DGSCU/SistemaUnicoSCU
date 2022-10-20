Imports System.IO
Public Class ver_Scadenzario
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents tabSchede As Microsoft.Web.UI.WebControls.TabStrip
    'Protected WithEvents mlpSchede As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents DG1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DG2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DG3 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DG4 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DG5 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents imgStampa As System.Web.UI.WebControls.Button
    Protected WithEvents imgChiudi As System.Web.UI.WebControls.Button


    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Public Property IdVerificatore() As Integer
        Get
            Return Me.ViewState("idverificatore")
        End Get
        Set(ByVal Value As Integer)
            Me.ViewState("idverificatore") = Value
        End Set
    End Property
    'funzione che controlla il profilo dell'utente loggato
    'MASTER  = TRUE
    'ISPETTORE  = FALSE
    Function TrovaProfiloUtente() As Boolean
        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader

        strLocal = "SELECT Descrizione FROM Profili "

        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strLocal = strLocal & " WHERE AssociaUtenteGruppo.UserName='" & Session("Utente") & "'"

        dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))

        If dtrLocal.HasRows Then
            dtrLocal.Read()
            If (dtrLocal("Descrizione") <> "VERIFICHE MASTER" And dtrLocal("Descrizione") <> "UNSC MASTER") Then

                dtrLocal.Close()
                dtrLocal = Nothing

                'tabSchede.Items(2).Enabled = False
                'tabSchede.Items(3).Enabled = False
                'tabSchede.Items(4).Enabled = False

                strLocal = "SELECT idverificatore FROM TVerificatori INNER JOIN UtentiUnsc ON TVerificatori.IdUtente=UtentiUnsc.IdUtente WHERE UtentiUnsc.UserName='" & Session("Utente") & "'"
                dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))

                If dtrLocal.HasRows Then
                    dtrLocal.Read()
                    If Not IsDBNull(dtrLocal("idverificatore")) Then
                        IdVerificatore = dtrLocal("idverificatore")
                    Else
                        dtrLocal.Close()
                        dtrLocal = Nothing
                        Return True
                    End If
                    dtrLocal.Close()
                    dtrLocal = Nothing
                    Return False
                End If
            End If
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        Return True
    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not IsPostBack Then
            TrovaCompetenzaUtente()
            InizializzaWebForm(TrovaProfiloUtente())
        End If

    End Sub
    Private Sub InizializzaWebForm(Optional ByVal IsMaster As Boolean = True)
        Dim D As DataSet
        Try

            'SCADENZARIO VERIFICHE
            D = New DataSet
            If IsMaster Then
                Me.DG1.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_VERIFICHE", Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            Else 'not master
                Me.DG1.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_VERIFICHE_UTENTE_VERIFICATORE", Me.IdVerificatore, Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            End If
            Me.DG1.DataBind()
            lblScadenzaVerifiche.Text = lblScadenzaVerifiche.Text & " (Estratti: " & Me.DG1.Items.Count & ")"
            Me.ViewState("Tab1") = Me.DG1.DataSource

            'SCADENZARIO RELAZIONI 
            D = New DataSet
            If IsMaster Then
                Me.DG2.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_RELAZIONI", Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            Else 'not master
                Me.DG2.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_RELAZIONI_UTENTE_VERIFICATORE", Me.IdVerificatore, Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            End If
            Me.DG2.DataBind()
            LblScadenzaRelazioni.Text = LblScadenzaRelazioni.Text & " (Estratti: " & Me.DG2.Items.Count & ")"
            Me.ViewState("Tab2") = Me.DG2.DataSource

            'SCADENZARIO CHIUSURA VERIFICHE
            D = New DataSet
            If IsMaster Then
                Me.DG3.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_CHIUSURA_VERIFICHE", Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            Else
                Me.DG3.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_CHIUSURA_VERIFICHE_UTENTE_VERIFICATORE", Me.IdVerificatore, Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            End If
            Me.DG3.DataBind()
            LblScadenzaChiusuraVerifiche.Text = LblScadenzaChiusuraVerifiche.Text & " (Estratti: " & Me.DG3.Items.Count & ")"
            Me.ViewState("Tab3") = Me.DG3.DataSource()

            'SCADENZARIO RISPOSTA CONTESTAZIONE
            D = New DataSet
            If IsMaster Then
                Me.DG4.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_CONTESTAZIONI", Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            Else
                Me.DG4.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_CONTESTAZIONI_UTENTE_VERIFICATORE", Me.IdVerificatore, Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            End If
            Me.DG4.DataBind()
            LblScadenzaContestazioni.Text = LblScadenzaContestazioni.Text & " (Estratti: " & Me.DG4.Items.Count & ")"
            Me.ViewState("Tab4") = Me.DG4.DataSource

            'SCADENZARIO APPLICAZIONE SANZIONE
            D = New DataSet
            If IsMaster Then
                Me.DG5.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_APPLICAZIONE_SANZIONI", Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            Else
                Me.DG5.DataSource = clsVerificaSegnalazione.Get_Vista_SCADENZARIO_PER_VERIFICHE(Session("conn"), "SP_VER_SCADENZA_APPLICAZIONE_SANZIONI_UTENTE_VERIFICATORE", Me.IdVerificatore, Session("IdRegCompetenza"), Session("FiltroVisibilita"))
            End If
            Me.DG5.DataBind()
            lblScadenzaApplicazioneSanzione.Text = lblScadenzaApplicazioneSanzione.Text & " (Estratti: " & Me.DG5.Items.Count & ")"
            Me.ViewState("Tab5") = Me.DG5.DataSource


            CaricaDTperStampa(0, 7)

        Catch ex As Exception
            Dim pippo As String = ex.Message
        End Try
    End Sub

    'Private Sub tabSchede_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabSchede.SelectedIndexChange

    Protected Sub imgStampa_Click(sender As Object, e As EventArgs) Handles imgStampa.Click

        '    'gestione per la stampa del tab selezionato
       imgStampa.Visible = False
        Dim dtbRicerca As DataTable

     

        Try
            imgStampa.Enabled = False

            
            CaricaDTperStampa(0, 7)
            dtbRicerca = Session("DtbRicerca")
            StampaCSV(dtbRicerca, ApriCSVScadenzaVerifiche, "SV")

            CaricaDTperStampa(1, 8)
            dtbRicerca = Session("DtbRicerca")
            StampaCSV(dtbRicerca, ApriCSVScadenzaRelazioni, "SR")

            CaricaDTperStampa(2, 7)
            dtbRicerca = Session("DtbRicerca")
            StampaCSV(dtbRicerca, ApriCSVScadenzaChiusuraVerifiche, "SCV")

            CaricaDTperStampa(3, 7)
            dtbRicerca = Session("DtbRicerca")
            StampaCSV(dtbRicerca, ApriCSVScadenzaContestazioni, "SC")

            CaricaDTperStampa(4, 8)
            dtbRicerca = Session("DtbRicerca")
            StampaCSV(dtbRicerca, ApriCSVScadenzaApplicazioneSanzione, "SAS")


        Catch ex As Exception

        Finally
            imgStampa.Enabled = True
        End Try



    End Sub
    Private Sub CaricaDTperStampa(ByVal index As Byte, ByVal NumCol As Byte)
        Dim NomeColonne(8) As String
        Dim NomiCampiColonne(8) As String
        Dim dtsGenerico As New DataSet

        Select Case index
            Case Is = 0
                NomiCampiColonne(0) = "Codicefascicolo" : NomiCampiColonne(1) = "statoverifiche" : NomiCampiColonne(2) = "Programmazione" : NomiCampiColonne(3) = "DataPrevistaVerifica"
                NomiCampiColonne(4) = "DataFinePrevistaVerifica" : NomiCampiColonne(5) = "nominativo" : NomiCampiColonne(6) = "DenominazioneEnteProponente"
                NomiCampiColonne(7) = "Titolo"

                NomeColonne(0) = "Numero Fascicolo" : NomeColonne(1) = "Stato Verifica" : NomeColonne(2) = "Programmazione"
                NomeColonne(3) = "Data Inizio Prevista Verifica" : NomeColonne(4) = "Data Fine Prevista Verifica" : NomeColonne(5) = "Verificatore"
                NomeColonne(6) = "Ente Proponente" : NomeColonne(7) = "Titolo"
                dtsGenerico = Me.ViewState("Tab1").copy
            Case Is = 1
                NomiCampiColonne(0) = "Codicefascicolo" : NomiCampiColonne(1) = "statoverifiche"
                NomiCampiColonne(2) = "Programmazione" : NomiCampiColonne(3) = "DataFineVerifica"
                NomiCampiColonne(4) = "dataprotincarico" : NomiCampiColonne(5) = "nominativo"
                NomiCampiColonne(6) = "DenominazioneEnteProponente" : NomiCampiColonne(7) = "Titolo"
                NomiCampiColonne(8) = "differenza"

                NomeColonne(0) = "Numero Fascicolo" : NomeColonne(1) = "Stato Verifica"
                NomeColonne(2) = "Programmazione" : NomeColonne(3) = "Data Chiusura Verifica"
                NomeColonne(4) = "Data Protocollo Incarico" : NomeColonne(5) = "Verificatore"
                NomeColonne(6) = "Ente Proponente" : NomeColonne(7) = "Titolo"
                NomeColonne(8) = "Scadenza in giorni"
                dtsGenerico = Me.ViewState("Tab2").copy
            Case Is = 2
                NomiCampiColonne(0) = "Codicefascicolo" : NomiCampiColonne(1) = "statoverifiche"
                NomiCampiColonne(2) = "Programmazione" : NomiCampiColonne(3) = "dataProtRelazione"
                NomiCampiColonne(4) = "nominativo" : NomiCampiColonne(5) = "DenominazioneEnteProponente"
                NomiCampiColonne(6) = "Titolo" : NomiCampiColonne(7) = "differenza"

                NomeColonne(0) = "Numero Fascicolo" : NomeColonne(1) = "Stato Verifica"
                NomeColonne(2) = "Programmazione" : NomeColonne(3) = "Data Protocollo Relazione"
                NomeColonne(4) = "Verificatore" : NomeColonne(5) = "Ente Proponente"
                NomeColonne(6) = "Progetto" : NomeColonne(7) = "Scadenza in giorni"
                dtsGenerico = Me.ViewState("Tab3").copy
            Case Is = 3
                NomiCampiColonne(0) = "Codicefascicolo" : NomiCampiColonne(1) = "statoverifiche"
                NomiCampiColonne(2) = "Programmazione" : NomiCampiColonne(3) = "DATAPROTINVIOLETTERACONTESTAZIONE"
                NomiCampiColonne(4) = "nominativo" : NomiCampiColonne(5) = "DenominazioneEnteProponente"
                NomiCampiColonne(6) = "Titolo" : NomiCampiColonne(7) = "differenza"

                NomeColonne(0) = "Numero Fascicolo" : NomeColonne(1) = "Stato Verifica"
                NomeColonne(2) = "Programmazione" : NomeColonne(3) = "Data Protocollo Invio Lettera Contestazione"
                NomeColonne(4) = "Verificatore" : NomeColonne(5) = "Ente Proponente"
                NomeColonne(6) = "Progetto" : NomeColonne(7) = "Scadenza in giorni"
                dtsGenerico = Me.ViewState("Tab4").copy
            Case Is = 4
                NomiCampiColonne(0) = "Codicefascicolo" : NomiCampiColonne(1) = "statoverifiche"
                NomiCampiColonne(2) = "Programmazione" : NomiCampiColonne(3) = "DataProtTrasmissioneSanzione"
                NomiCampiColonne(4) = "nominativo" : NomiCampiColonne(5) = "DenominazioneEnteProponente"
                NomiCampiColonne(6) = "Titolo" : NomiCampiColonne(7) = "RegioneOrUffico" : NomiCampiColonne(8) = "differenza"

                NomeColonne(0) = "Numero Fascicolo" : NomeColonne(1) = "Stato Verifica"
                NomeColonne(2) = "Programmazione" : NomeColonne(3) = "Data Protocollo Trasmissione Sanzione"
                NomeColonne(4) = "Verificatore" : NomeColonne(5) = "Ente Proponente"
                NomeColonne(6) = "Progetto" : NomeColonne(7) = "Regione/Servizio Esec. Sanzione" : NomeColonne(8) = "Scadenza in giorni"
                dtsGenerico = Me.ViewState("Tab5").copy
        End Select

        CaricaDataTablePerStampa(dtsGenerico, NumCol, NomeColonne, NomiCampiColonne)

    End Sub
    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub
    Private Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        Response.Redirect("WfrmMain.aspx")
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




    Private Sub StampaCSV(ByVal dtbRicerca As DataTable, ByRef ApriCSV1 As HyperLink, ByVal Nome As String)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False

        Else
            xPrefissoNome = Session("Utente") + Nome
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