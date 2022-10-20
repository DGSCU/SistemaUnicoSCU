Imports System.IO
Public Class ver_ProgrammazioniApprovate
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents CmdStampaProgrammazione As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents lblCodiceFascicolo As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtCodiceFascicolo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtCodiceFasc As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSelFascicolo As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdSelProtocollo0 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdFascCanc As System.Web.UI.WebControls.Image
    Protected WithEvents LblDescrFascicolo As System.Web.UI.WebControls.Label
    Protected WithEvents txtDescFasc As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblDataProtPresenta As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDataProtocolloPresenta As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblNumProtPresenta As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtNumProtPresenta As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSc1SelProtocollo1 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdSc1Allegati1 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdNuovoFascioclo As System.Web.UI.WebControls.Image
    'Protected WithEvents lblDataProtApprova As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDataProtocolloApprova As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblNumProtApprova As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtNumProtApprova As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdSc1SelProtocollo2 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdSc1Allegati2 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdNuovoFascioclo1 As System.Web.UI.WebControls.Image
    'Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents CmdSalva As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents LblDescrProgrammazione As System.Web.UI.WebControls.Label
    'Protected WithEvents imgStampa As System.Web.UI.WebControls.ImageButton

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object
    Public dtrLeggiDati As SqlClient.SqlDataReader
    Public TBLLeggiDati As DataTable
    Public row As TableRow
    'Protected WithEvents lblMessaggio As System.Web.UI.WebControls.Label
    'Protected WithEvents ImgDataProtPres As System.Web.UI.WebControls.Image
    'Protected WithEvents ImgDataProtApp As System.Web.UI.WebControls.Image
    'Protected WithEvents LblCompetenza As System.Web.UI.WebControls.Label
    'Protected WithEvents btFascicola As System.Web.UI.WebControls.Button
    Public myRow As DataRow
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'creata il 27/09/2007 da simona cordella
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then
            CaricaMaschera(Request.QueryString("IdProgrammazione"))
            CaricaElenco(Request.QueryString("IdProgrammazione"))
            TrovaCompetenzaProgrammazione(Request.QueryString("IdProgrammazione"))

            PersonalizzaMaschera(Session("IdRegCompetenza"))
        End If
    End Sub


    Private Sub CaricaMaschera(ByVal IdProgrammazione)
        Dim strsql As String

        Dim dtrGenerico As SqlClient.SqlDataReader
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strsql = "Select  idstatoprogrammazione, dbo.formatodata(DataProtPresentazione)as DataProtPresentazione, NProtPresentazione,"
        strsql = strsql & " dbo.formatodata(DataProtApprovazione)as DataProtApprovazione,NProtApprovazione,"
        strsql = strsql & " CodiceFascicolo, IDFascicolo, DescrFascicolo,Descrizione, "
        strsql = strsql & " dbo.FormatoData(DataInizioValidità) AS DataInizioValidità, dbo.FormatoData(DataFineValidità) AS DataFineValidità"
        strsql = strsql & " from tverificheprogrammazione where idprogrammazione =" & IdProgrammazione & " "
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            LblDescrProgrammazione.Text = "" & dtrGenerico("descrizione")
            TxtCodiceFascicolo.Text = "" & dtrGenerico("CodiceFascicolo")
            ''TxtCodiceFasc.Text = "" & dtrGenerico("IDFascicolo")
            hfTxtCodiceFasc.Value = "" & dtrGenerico("IDFascicolo")
            txtDescFasc.Text = "" & dtrGenerico("DescrFascicolo")
            txtDataProtocolloPresenta.Text = "" & dtrGenerico("DataProtPresentazione")
            TxtNumProtPresenta.Text = "" & dtrGenerico("NProtPresentazione")
            txtDataProtocolloApprova.Text = "" & dtrGenerico("DataProtApprovazione")
            TxtNumProtApprova.Text = "" & dtrGenerico("NProtApprovazione")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Private Sub AggiornaProtocollo()
        Dim sqlComOp As New SqlClient.SqlCommand
        Dim strsql As String

        sqlComOp.Connection = Session("conn")
        sqlComOp.CommandType = CommandType.Text
        '  CodiceFascicolo , IDFascicolo, DescrFascicolo 
        strsql = " Update tverificheprogrammazione set  "
        strsql = strsql & " CodiceFascicolo='" & Replace(TxtCodiceFascicolo.Text, "'", "''") & "' , "
        '' strsql = strsql & " IDFascicolo ='" & Replace(TxtCodiceFasc.Text, "'", "''") & "' , "
        strsql = strsql & " IDFascicolo ='" & Replace(hfTxtCodiceFasc.Value, "'", "''") & "' , "
        strsql = strsql & " DescrFascicolo ='" & Replace(txtDescFasc.Text, "'", "''") & "', "
            If txtDataProtocolloPresenta.Text <> "" Then
        strsql = strsql & " DataProtPresentazione = '" & txtDataProtocolloPresenta.Text & "',"
            Else
        strsql = strsql & " DataProtPresentazione = null,"
            End If

        If TxtNumProtPresenta.Text <> "" Then
            strsql = strsql & " NProtPresentazione = '" & TxtNumProtPresenta.Text & "',"
        Else
            strsql = strsql & " NProtPresentazione = null,"
        End If
        If txtDataProtocolloApprova.Text <> "" Then
            strsql = strsql & " DataProtApprovazione = '" & txtDataProtocolloApprova.Text & "',"
        Else
            strsql = strsql & " DataProtApprovazione = null,"
        End If

        If TxtNumProtApprova.Text <> "" Then
            strsql = strsql & " NProtApprovazione = '" & TxtNumProtApprova.Text & "'"
        Else
            strsql = strsql & " NProtApprovazione = null "
        End If
        strsql = strsql & " Where idprogrammazione = " & Request.QueryString("IdProgrammazione")

        sqlComOp.CommandText = strsql
        sqlComOp.ExecuteNonQuery()
    End Sub
    Private Sub CaricaElenco(ByVal IdProgrammazione)
        Dim dtElenco As New DataSet
        Dim strSql As String
        Dim strApicetto As String = "'"
        dgRisultatoRicerca.CurrentPageIndex = 0
        strSql = "select   CodiceEnte,IDAttivitàEnteSedeAttuazione, IDVerifica, IDStatoVerifica,StatoVerifiche, Titolo +  ' (' + CodiceProgetto + ')' as Progetto, Ambito, dbo.FormatoData(DataFineAttività) as DataFineAttività , "
        strSql = strSql & " EnteFiglio + ' (' + convert(varchar, IDEnteSedeAttuazione)  +')'  as Sedeattuazione, EnteSede, Ente +  ' (' + CodiceEnte + ')' as Ente , Comune +  ' (' + DescrAbb + ')' as Comune,Regione  , numeroVolontari, IDProgrammazione, Verificatore,"
        strSql = strSql & " idclasseaccreditamento, dbo.FormatoData(datainizioattività) as datainizioattività, provincia, indirizzo"
        strSql = strSql & " from ver_vw_dettaglioscenario "
        strSql = strSql & " where idprogrammazione =" & IdProgrammazione & " "
        strSql = strSql & " and idstatoprogrammazione = 4 AND  IDStatoVerifica >= 5"

        strSql = strSql & " Group by IDAttivitàEnteSedeAttuazione, IDVerifica,  IDStatoVerifica, CodiceProgetto,  "
        strSql = strSql & " Titolo, Ambito, DataFineAttività,EnteFiglio, IDEnteSedeAttuazione,  StatoVerifiche,"
        strSql = strSql & " EnteSede,CodiceEnte, Ente, Comune, DescrAbb, Regione, numeroVolontari, "
        strSql = strSql & " idprogrammazione,verificatore,DataPrevistaVerifica ,idente ,"
        strSql = strSql & " IDAttività,idclasseaccreditamento,datainizioattività,provincia,indirizzo "
        strSql = strSql & " Order by Regione,Provincia,Comune"

        Session("dtElenco") = ClsServer.DataSetGenerico(strSql, Session("conn"))

        CaricaDataGrid(dgRisultatoRicerca)

    End Sub

    Private Sub CaricaDataGrid(ByRef GriddaCaricare As DataGrid)
        'assegno il dataset alla griglia del risultato
        GriddaCaricare.DataSource = Session("dtElenco")
        GriddaCaricare.DataBind()
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(13) As String
        Dim NomiCampiColonne(13) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Ente"
        NomeColonne(1) = "Ente"
        NomeColonne(2) = "Progetto"
        NomeColonne(3) = "Settore/ Ambito"
        'NomeColonne(4) = "Data Inizio Progetto"
        NomeColonne(4) = "Data Inizio Progetto"
        NomeColonne(5) = "Data Fine Progetto"
        NomeColonne(6) = "Classe/Sezione Accreditamento"
        NomeColonne(7) = "Sede Attuazione"
        NomeColonne(8) = "Num. Volontari"
        NomeColonne(9) = "Indirizzo"
        NomeColonne(10) = "Comune"
        NomeColonne(11) = "Provincia"
        NomeColonne(12) = "Regione"
        'NomeColonne(13) = "Stato Verifica"
        NomeColonne(13) = "Assegnato a"

        NomiCampiColonne(0) = "CodiceEnte"
        NomiCampiColonne(1) = "Ente"
        NomiCampiColonne(2) = "progetto"
        NomiCampiColonne(3) = "ambito"
        'NomiCampiColonne(3) = ""
        NomiCampiColonne(4) = "DataInizioAttività"
        NomiCampiColonne(5) = "DataFineAttività"
        NomiCampiColonne(6) = "idclasseaccreditamento"
        NomiCampiColonne(7) = "sedeattuazione"
        NomiCampiColonne(8) = "numerovolontari"
        NomiCampiColonne(9) = "indirizzo"
        NomiCampiColonne(10) = "comune"
        NomiCampiColonne(11) = "provincia"
        NomiCampiColonne(12) = "regione"
        'NomiCampiColonne(13) = "StatoVerifiche"
        NomiCampiColonne(13) = "verificatore"


        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(Session("dtElenco"), 13, NomeColonne, NomiCampiColonne)

        '*********************************************************************************
        If GriddaCaricare.Items.Count = 0 Then
            GriddaCaricare.Visible = False
            imgStampa.Visible = False
        Else
            imgStampa.Visible = True
            GriddaCaricare.Visible = True
        End If

    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("ver_RicercaProgrammazioneApprovate.aspx")
    End Sub

    Private Sub CmdStampaProgrammazione_Click(sender As Object, e As EventArgs) Handles CmdStampaProgrammazione.Click
        'Dim IDProgrammazione As Integer
        'IDProgrammazione = CInt(Request.QueryString("IdProgrammazione"))
        'Call stampaTrasmissioneProgrammazione(IDProgrammazione, "Trasmissioneprogrammazione")

        Dim Documento As New GeneratoreModelli
        Response.Write("<script type=""text/javascript"">" & vbCrLf)


        Response.Write("window.open('" & Documento.MON_Trasmissioneprogrammazione(Request.QueryString("IdProgrammazione"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</script>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()

    End Sub

    Private Function stampaTrasmissioneProgrammazione(ByVal IDProgrammazione As Integer, ByVal NomeFile As String) As String
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Dim datafineattività As String

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader("select top 1 datafineattività,titolo from attività where identepresentante = 14 order by datafineattività desc", Session("conn"))
        dtrLeggiDati.Read()
        datafineattività = dtrLeggiDati("datafineattività")
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

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

            Call DatiTrasProg()


            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/Naz/" & Session("Path") & NomeFile & ".rtf"))

                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

                'Writer.WriteLine("{\rtf1")


                xLinea = Reader.ReadLine()


                Dim ProgrammazioneVerifiche As String = dtrLeggiDati("Programmazione")

         
                While xLinea <> ""
                    xLinea = Replace(xLinea, "<Programmazione>", ProgrammazioneVerifiche)
                    Writer.WriteLine(xLinea)
                    xLinea = Reader.ReadLine()

                End While


                Writer.Close()
                Writer = Nothing


                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            stampaTrasmissioneProgrammazione = "documentazione/" & strNomeFile

        Catch ex As Exception
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Response.Write(ex.Message)
        End Try

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & stampaTrasmissioneProgrammazione & "');" & vbCrLf)
        Response.Write("</SCRIPT>")


    End Function
    Sub DatiTrasProg()
        Dim strsql As String
        Dim Programmazione As Integer
        Programmazione = CInt(Request.QueryString("IdProgrammazione"))
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "select * from VER_VW_PROGRAMMAZIONE_VERIFICHE "
        strsql = strsql & "where IDProgrammazione =" & Programmazione & " and idstatoverifica in ('4', '5')"
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub
 


    Private Sub PersonalizzaMaschera(ByVal IdRegCompetenza As Integer)

        '*** Creata il 15/11/2007 da Simona Cordella
        '*** Se competenza regionale <> 22 (Nazionale) disabiliti campi della protocollazione

        If IdRegCompetenza <> 22 Then
            'TxtCodiceFascicolo.ReadOnly = False
            'txtDescFasc.ReadOnly = False

            ' txtDataProtocolloPresenta.ReadOnly = False
            ' TxtNumProtPresenta.ReadOnly = False
            ' txtDataProtocolloApprova.ReadOnly = False
            ' TxtNumProtApprova.ReadOnly = False

            cmdSelFascicolo.Visible = False
            cmdSelProtocollo0.Visible = False
            cmdFascCanc.Visible = False
            cmdSc1SelProtocollo1.Visible = False
            cmdSc1Allegati1.Visible = False
            '' cmdNuovoFascioclo.Visible = False
            cmdSc1SelProtocollo2.Visible = False
            cmdSc1Allegati2.Visible = False
            ''   cmdNuovoFascioclo1.Visible = False
            'txtDataProtocolloPresenta.Enabled = True
            'txtDataProtocolloApprova.Enabled = True
            'ImgDataProtPres.Visible = True
            'ImgDataProtApp.Visible = True
        Else
            'TxtCodiceFascicolo.ReadOnly = True
            'txtDescFasc.ReadOnly = True

            'txtDataProtocolloPresenta.ReadOnly = True
            'TxtNumProtPresenta.ReadOnly = True
            'txtDataProtocolloApprova.ReadOnly = True
            'TxtNumProtApprova.ReadOnly = True

            cmdSelFascicolo.Visible = True
            cmdSelProtocollo0.Visible = True
            cmdFascCanc.Visible = True
            cmdSc1SelProtocollo1.Visible = True
            cmdSc1Allegati1.Visible = True
            ''  cmdNuovoFascioclo.Visible = True
            cmdSc1SelProtocollo2.Visible = True
            cmdSc1Allegati2.Visible = True
            ''   cmdNuovoFascioclo1.Visible = True
            'txtDataProtocolloPresenta.Enabled = False
            'txtDataProtocolloApprova.Enabled = False
            'ImgDataProtPres.Visible = False
            'ImgDataProtApp.Visible = False
        End If

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
            LblCompetenza.Visible = True
            LblCompetenza.Text = "Competenza:" & dtrCompetenze("Descrizione")
            Session("IdRegCompetenza") = dtrCompetenze("IdRegCompetenza")
        End If

        'chiudo il datareader se aperto
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub

    Private Sub btFascicola_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btFascicola.Click
        'creata da simona cordella il 07/05/2012 
        'funzione spot per la generazione dei fascicolo di evrifche aperte con programmazioni approvate
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strsql As String
        Dim item As DataGridItem
        Dim IdVerifica As Integer
        Dim strVerificatore As String

        Dim strIdVerifica As String
        For Each item In dgRisultatoRicerca.Items

            '*** 09/12/2010 verifico esistenza del codice fascicolo
            strsql = "Select isnull(CodiceFascicolo,'') as CodiceFascicolo from TVerifiche where IdVerifica = " & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrGenerico.Read()
            If dtrGenerico("CodiceFascicolo") = "" Then
                strIdVerifica = strIdVerifica & dgRisultatoRicerca.Items(item.ItemIndex).Cells(0).Text & "#"
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            '***
        Next
        If strIdVerifica <> "" Then
            lblMessaggio.Text = ClsUtility.GeneraFascicoloCumulatiVerifiche(Session("Utente"), Mid(strIdVerifica, 1, Len(strIdVerifica) - 1), Session("Conn"))
        End If
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


    Protected Sub imgStampa_Click(sender As Object, e As EventArgs) Handles imgStampa.Click
        imgStampa.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Protected Sub cmdFascCanc_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCanc.Click

        ''  TxtCodiceFascicolo.Text = ""
        hfTxtCodiceFasc.Value = ""
        txtDescFasc.Text = ""
        txtDataProtocolloPresenta.Text = ""
        TxtNumProtPresenta.Text = ""
        txtDataProtocolloApprova.Text = ""
        TxtNumProtApprova.Text = ""


    End Sub

    Public Function EseguiStoreCancellaProgrammazione(ByVal IdProgrammazione As Integer, ByVal conn As SqlClient.SqlConnection) As String

        Dim MySqlCommand As SqlClient.SqlCommand
        MySqlCommand = New SqlClient.SqlCommand
        MySqlCommand.CommandType = CommandType.StoredProcedure
        MySqlCommand.CommandText = "SP_ELIMINA_PROGRAMMAZIONE"
        MySqlCommand.Connection = conn

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IDProgrammazione"
        sparam.SqlDbType = SqlDbType.Int
        MySqlCommand.Parameters.Add(sparam)

        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@esito"
        sparam1.Size = 20
        sparam1.SqlDbType = SqlDbType.NVarChar
        sparam1.Direction = ParameterDirection.Output
        MySqlCommand.Parameters.Add(sparam1)

        Dim Reader As SqlClient.SqlDataReader
        MySqlCommand.Parameters("@IDProgrammazione").Value = IdProgrammazione
        Reader = MySqlCommand.ExecuteReader
        EseguiStoreCancellaProgrammazione = MySqlCommand.Parameters("@esito").Value
        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
    End Function

   
    Private Sub CmdSalva_Click(sender As Object, e As System.EventArgs) Handles CmdSalva.Click
        If txtDataProtocolloApprova.Text <> "" Then
            If txtDataProtocolloApprova.Text < txtDataProtocolloPresenta.Text Then
                lblMessaggio.Visible = True
                lblMessaggio.Text = "La data Approvazione deve essere maggione o uguale alla data Presentazione."
                Exit Sub
            Else
                lblMessaggio.Visible = False
            End If
        End If

        AggiornaProtocollo()
    End Sub

    Private Sub cmdCancella_Click(sender As Object, e As System.EventArgs) Handles cmdCancella.Click
        Dim strsql As String
        Dim dtrRic As SqlClient.SqlDataReader
        lblMessaggio.Visible = False
        Dim strEsito As String
        msgErrore.Visible = False
        lblMessaggio.Visible = False
        strsql = "Select * from TVerifiche where idprogrammazione = " & CInt(Request.QueryString("IdProgrammazione")) & "  AND IDStatoVerifica NOT IN (5,12,16)"
        dtrRic = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrRic.HasRows = True Then
            If Not dtrRic Is Nothing Then
                dtrRic.Close()
                dtrRic = Nothing
            End If
            msgErrore.Visible = True
            msgErrore.Text = "Non è possbile cancellare la programmazione in quanto sono presenti della Verifiche assegnate."
        Else
            If Not dtrRic Is Nothing Then
                dtrRic.Close()
                dtrRic = Nothing
            End If

            strEsito = EseguiStoreCancellaProgrammazione(CInt(Request.QueryString("IdProgrammazione")), Session("conn"))
            If strEsito = "OK" Then

                lblMessaggio.Visible = True
                lblMessaggio.Text = "Eliminazione eseguita con successo."
            Else
                msgErrore.Visible = True
                msgErrore.Text = "Problemi durante l' eliminazione della Programmazione."
            End If
            If Not dtrRic Is Nothing Then
                dtrRic.Close()
                dtrRic = Nothing
            End If
        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub
End Class