Imports System.IO

Public Class ver_RicercaAssociaVerifiche
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaIntestazione()
            txtIdVerificaPadre.Value = Request.QueryString("IdVerifica")

            ddlSettore.DataSource = ClsServer.CreaDataTable("select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività", True, Session("conn"))
            ddlSettore.DataValueField = "idmacroambitoattività"
            ddlSettore.DataTextField = "Macro"
            ddlSettore.DataBind()

            '***Carico combo area intervento
            ddlArea.Items.Add("")
            ddlArea.Enabled = False
        End If
    End Sub

    Private Sub CmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles CmdRicerca.Click
        CaricaElenco()
    End Sub
    Private Sub CaricaIntestazione()
        'Dim strsql As String
        'Dim dtrInt As SqlClient.SqlDataReader

        'strsql = "Select CodiceProgetto + ' - ' + Titolo as Progetto, Verificatore " & _
        '        " From ver_vw_dettaglioscenario " & _
        '        " where CodiceProgetto ='" & Request.QueryString("CodiceProgetto") & "' and idstatoverifica=5"
        'dtrInt = ClsServer.CreaDatareader(strsql, Session("conn"))
        'If dtrInt.HasRows = True Then
        '    dtrInt.Read()
        txtProgetto.Text = Request.QueryString("Progetto")
        TxtVerificatore.Text = Request.QueryString("Verificatore")
        TxtStatoVerifica.Text = Request.QueryString("StatoVerifica")
        TxtTipoVerifica.Text = Request.QueryString("TipoVerifica")
        'End If
        'If Not dtrInt Is Nothing Then
        '    dtrInt.Close()
        '    dtrInt = Nothing
        'End If
    End Sub
    Private Function TrovaTipologiaVerificatore(ByVal IdVerificatore) As Integer
        Dim strsql As String
        Dim dtrInt As SqlClient.SqlDataReader

        strsql = "Select Tipologia " & _
                " From Tverificatori " & _
                " where IdVerificatore =" & IdVerificatore & " "
        dtrInt = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrInt.HasRows = True Then
            dtrInt.Read()
            If dtrInt("Tipologia") = True Then
                TrovaTipologiaVerificatore = 1
            Else
                TrovaTipologiaVerificatore = 0
            End If
        End If
        If Not dtrInt Is Nothing Then
            dtrInt.Close()
            dtrInt = Nothing
        End If
        Return TrovaTipologiaVerificatore
    End Function

    Private Sub CaricaElenco()
        Dim dtElenco As New DataSet
        Dim strSql As String
        Dim TipologiaVer As Integer
        dgRisultatoRicerca.CurrentPageIndex = 0
        TipologiaVer = TrovaTipologiaVerificatore(Request.QueryString("idverificatore"))

        strSql = " select  IDAttivitàEnteSedeAttuazione, IDVerifica, IDStatoVerifica, " & _
                 " CodiceProgetto, Titolo as Progetto, IDEnteSedeAttuazione, Denominazione +  ' (' + CodiceEnte + ')' as EnteProponente , " & _
                 " Comune +  ' (' + DescrAbb + ')' as Comune, Regione, IDProgrammazione,Programmazione, " & _
                 " dbo.FormatoData(DataFineAttività) as DataFineAttività , " & _
                 " dbo.FormatoData(DataPrevistaVerifica) as DataPrevistaVerifica, " & _
                 " dbo.FormatoData(DataFinePrevistaVerifica) as DataFinePrevistaVerifica " & _
                 " from VER_VW_RICERCA_VERIFICHE "
        strSql &= " where idstatoverifica =5"
        strSql &= " and codiceprogetto= '" & Request.QueryString("codiceprogetto") & "'"
        If TipologiaVer = 0 Then
            strSql &= " and idverificatore =" & Request.QueryString("idverificatore") & " "
        Else
            strSql &= " and IdIgf =" & Request.QueryString("idverificatore") & " "
        End If
        strSql &= " and IDVerifica <> " & Request.QueryString("IDVerifica") & " "


        If TxtTipoVerifica.Text = "Programmata" Then
            strSql &= " and IDProgrammazione = " & Request.QueryString("IDProgrammazione") & " "
            strSql &= " and Tipologia =1 "
        Else
            strSql &= " and Tipologia =2 "
        End If

        If TxtCodEnte.Text.Trim <> "" Then
            strSql &= " and codiceente = '" & TxtCodEnte.Text & "'"
        End If
        If TxtDescrEnte.Text.Trim <> "" Then
            strSql &= " and denominazione like '" & Replace(TxtDescrEnte.Text, "'", "''") & "%'"
        End If
        If TxtComune.Text.Trim <> "" Then
            strSql &= " and comune like '" & Replace(TxtComune.Text, "'", "''") & "%'"
        End If
        If TxtProvincia.Text.Trim <> "" Then
            strSql &= " and provincia like '" & Replace(TxtProvincia.Text, "'", "''") & "%'"
        End If
        If TxtRegione.Text.Trim <> "" Then
            strSql &= " and regione like '" & Replace(TxtRegione.Text, "'", "''") & "%'"
        End If

        If ddlSettore.SelectedValue > 0 Then
            strSql &= " and idmacroambitoattività = " & ddlSettore.SelectedValue
        End If
        If ddlArea.SelectedValue <> "" Then
            If ddlArea.SelectedValue > 0 Then
                strSql &= " and idambitoattività = " & ddlArea.SelectedValue
            End If
        End If

        strSql &= " GROUP BY IDAttivitàEnteSedeAttuazione, IDVerifica, IDStatoVerifica, "
        strSql &= " CodiceProgetto, Titolo, IDEnteSedeAttuazione,"
        strSql &= " Denominazione, CodiceEnte, Comune , DescrAbb , Regione, IDProgrammazione,Programmazione, "
        strSql &= " DataFineAttività ,  DataPrevistaVerifica, DataFinePrevistaVerifica "
        strSql &= "  ORDER BY  idstatoverifica"
        Session("dtElenco") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicerca)

    End Sub
    Private Sub CaricaDataGrid(ByRef GriddaCaricare As DataGrid)
        'assegno il dataset alla griglia del risultato
        GriddaCaricare.DataSource = Session("dtElenco")
        GriddaCaricare.DataBind()
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(8) As String
        Dim NomiCampiColonne(8) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Ente Proponente"
        NomeColonne(1) = "Codice Progetto"
        NomeColonne(2) = "Progetto"
        NomeColonne(3) = "Data Fine Progetto"
        NomeColonne(4) = "Data Inizio Prevista Verifica"
        NomeColonne(5) = "Data Fine Prevista Verifica"
        NomeColonne(6) = "Sede Attuazione"
        NomeColonne(7) = "Comune"
        NomeColonne(8) = "Regione"
        'NomeColonne(10) = "Stato Verifica"


        NomiCampiColonne(0) = "EnteProponente"
        NomiCampiColonne(1) = "codiceprogetto"
        NomiCampiColonne(2) = "progetto"
        NomiCampiColonne(3) = "DataFineAttività"
        NomiCampiColonne(4) = "DataPrevistaVerifica"
        NomiCampiColonne(5) = "DataFinePrevistaVerifica"
        NomiCampiColonne(6) = "IDEnteSedeAttuazione"
        NomiCampiColonne(7) = "comune"
        NomiCampiColonne(8) = "regione"


        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(Session("dtElenco"), 8, NomeColonne, NomiCampiColonne)

        ''*********************************************************************************

        GriddaCaricare.Visible = True

        If GriddaCaricare.Items.Count = 0 Then
            imgStampa.Visible = False
            ApriCSV1.Visible = False
            lblmessaggi.Visible = True
            lblmessaggi.Text = "Nessuna informazione presente in archivio."
            GriddaCaricare.Visible = False
            cmdSalva.Visible = False
            chkSelDesel.Visible = False
            'cmdPresenta.Visible = False
        Else
            imgStampa.Visible = True
            ApriCSV1.Visible = True
            'lblMessaggio.Visible = False
            GriddaCaricare.Visible = True
            cmdSalva.Visible = True
            chkSelDesel.Visible = True
            'cmdPresenta.Visible = True
        End If

    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub

    Private Sub ddlSettore_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSettore.SelectedIndexChanged
        If ddlSettore.SelectedItem.Text <> "" Then
            ddlArea.Enabled = True
            ddlArea.DataSource = ClsServer.CreaDataTable("select distinct a.idambitoattività," & _
                                                     " a.codifica + ' ' + a.AmbitoAttività as Ambito from ambitiattività a" & _
                                                     " inner join macroambitiattività b" & _
                                                     " on a.IDMacroAmbitoAttività=b.IDMacroAmbitoAttività" & _
                                                     " where a.IDMacroAmbitoAttività=" & ddlSettore.SelectedValue & " order by 1", True, Session("conn"))
            ddlArea.DataTextField = "Ambito"
            ddlArea.DataValueField = "idambitoattività"
            ddlArea.DataBind()
        Else
            'popolo completamente combo aree di intervento
            ddlArea.DataSource = Nothing
            ddlArea.Items.Add("")
            ddlArea.SelectedIndex = 0
            ddlArea.Enabled = False
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("verificarequisiti.aspx?IDProgrammazione=" & Request.QueryString("IDProgrammazione") & "&IdVerifica=" & Request.QueryString("IdVerifica") & "&IdEnteSedeAttuazione=" & Request.QueryString("IdEnteSedeAttuazione") & "")
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        Dim sqlCommInsVer As New SqlClient.SqlCommand
        Dim IdVerifica As Integer
        Dim item As DataGridItem

        Dim IdScenario As Integer
        Dim IdProgrammazione As Integer

        If TxtTipoVerifica.Text = "Programmata" Then
            Dim drScenario As SqlClient.SqlDataReader = ClsServer.CreaDatareader("select IDScenario,IDPROGRAMMAZIONE from tverifiche where idverifica = " & txtIdVerificaPadre.Value, Session("conn"))
            drScenario.Read()
            IdScenario = drScenario("IdScenario")
            IdProgrammazione = drScenario("IdProgrammazione")
            drScenario.Close()
            drScenario = Nothing
        End If

        For Each item In dgRisultatoRicerca.Items
            IdVerifica = item.Cells(0).Text
            Dim check As CheckBox = DirectCast(item.FindControl("chkSel"), CheckBox)
            If check.Checked = True Then
                sqlCommInsVer.CommandType = CommandType.StoredProcedure
                sqlCommInsVer.CommandText = "SP_VER_INSERIMENTO_VERIFICHE_RAGGRUPPATE"
                sqlCommInsVer.Connection = Session("conn")
                sqlCommInsVer.Parameters.Add("@IDVERIFICA", IdVerifica)
                sqlCommInsVer.Parameters.Add("@IDSCENARIO", IdScenario)
                'sqlCommInsVer.Parameters.Add("@IDATTIVITAENTESEDEATTUAZIONE", Session("DataSetRicerca").Tables(0).Rows(i).Item(1))
                sqlCommInsVer.Parameters.Add("@IDSTATOVERIFICA", 5)
                sqlCommInsVer.Parameters.Add("@UTENTE", Session("Utente"))
                sqlCommInsVer.Parameters.Add("@IDPROGRAMMAZIONE", IdProgrammazione)
                sqlCommInsVer.Parameters.Add("@IDVerificaPadre", txtIdVerificaPadre.Value)
                sqlCommInsVer.ExecuteNonQuery()
                sqlCommInsVer.Parameters.Clear()
            End If
        Next
        'InserCom.Dispose()
        lblmessaggi.Text = "Inserimento eseguito correttamente"
        cmdSalva.Visible = False
    End Sub

    Protected Sub chkSelDesel_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelDesel.CheckedChanged
        If chkSelDesel.Checked = True Then

            Dim nRighe As Integer
            Dim x As Integer
            nRighe = dgRisultatoRicerca.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = dgRisultatoRicerca.Items(x).Cells(0).FindControl("chkSel")
                If chkoggetto.Visible = True Then
                    chkoggetto.Checked = True
                Else
                    chkoggetto.Checked = False
                End If
            Next
        Else
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = dgRisultatoRicerca.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = dgRisultatoRicerca.Items(x).Cells(0).FindControl("chkSel")
                chkoggetto.Checked = False
            Next

        End If
    End Sub




    Protected Sub imgStampa_Click(sender As Object, e As EventArgs) Handles imgStampa.Click
        imgStampa.Visible = False
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
            imgStampa.Visible = False
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