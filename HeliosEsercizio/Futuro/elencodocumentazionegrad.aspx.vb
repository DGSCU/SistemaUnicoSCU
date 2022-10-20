Imports System.IO
Imports System.Data.SqlClient

Public Class elencodocumentazionegrad
    Inherits System.Web.UI.Page
    Public TBLLeggiDati As DataTable
    Dim DTProva As DataTable
    Public row As TableRow
    Protected WithEvents txtDescFasc As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdFascCanc As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdSelFascicolo As System.Web.UI.WebControls.Image
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumeroFascicolo As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtCodiceFasc As HtmlInputHidden
    Protected WithEvents cmdSelProtocollo As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo1 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo2 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo3 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo4 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo5 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo6 As System.Web.UI.WebControls.Image
    Protected WithEvents TxtNumProtocollo1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdNuovoFascicolo1 As System.Web.UI.WebControls.Image
    Protected WithEvents TxtDataProtocollo1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdScAllegati1 As System.Web.UI.WebControls.Image
    Protected WithEvents TxtNumProtocollo2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdScAllegati2 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdNuovoFascicolo2 As System.Web.UI.WebControls.Image
    Protected WithEvents TxtDataProtocollo2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtNumProtocollo3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdScAllegati3 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdNuovoFascicolo3 As System.Web.UI.WebControls.Image
    Protected WithEvents TxtDataProtocollo3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtNumProtocollo4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdScAllegati4 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdNuovoFascicolo4 As System.Web.UI.WebControls.Image
    Protected WithEvents TxtDataProtocollo4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtNumProtocollo5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdScAllegati5 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdNuovoFascicolo5 As System.Web.UI.WebControls.Image
    Protected WithEvents TxtDataProtocollo5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtNumProtocollo6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdScAllegati6 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdNuovoFascicolo6 As System.Web.UI.WebControls.Image
    Protected WithEvents TxtDataProtocollo6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNumProtocollo1 As System.Web.UI.WebControls.Label
    Protected WithEvents LblDataProtocollo1 As System.Web.UI.WebControls.Label
    Protected WithEvents LblNumProtocollo2 As System.Web.UI.WebControls.Label
    Protected WithEvents LblDataProtocollo2 As System.Web.UI.WebControls.Label
    Protected WithEvents LblNumProtocollo3 As System.Web.UI.WebControls.Label
    Protected WithEvents LblDataProtocollo3 As System.Web.UI.WebControls.Label
    Protected WithEvents LblNumProtocollo4 As System.Web.UI.WebControls.Label
    Protected WithEvents LblDataProtocollo4 As System.Web.UI.WebControls.Label
    Protected WithEvents LblNumProtocollo5 As System.Web.UI.WebControls.Label
    Protected WithEvents LblDataProtocollo5 As System.Web.UI.WebControls.Label
    Protected WithEvents LblNumProtocollo6 As System.Web.UI.WebControls.Label
    Protected WithEvents LblDataProtocollo6 As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumFascicoloControllo As HtmlInputHidden
    Protected WithEvents Salva As Button
    Protected WithEvents HdValoreSalva As HtmlInputHidden
    Protected WithEvents hddModificaProtocollo As HtmlInputHidden
    Protected WithEvents txtdatadal As System.Web.UI.WebControls.TextBox
    Public myRow As DataRow
    Dim PROGETTO_GARANZIA_GIOVANI As String = "4"
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
    Public dtrLeggiDati As SqlClient.SqlDataReader

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strsql As String

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then
            PopolaCombo()

            Call CaricaFascicolo()
            Call CaricaProtocollo()
        End If

        'Modifica del 17/01/2006 di Amilcare Paolella ***************************
        'Ricavo le informazioni dell'utente per valorizzare la path dei documenti
        strsql = "SELECT RegioniCompetenze.CodiceRegioneCompetenza AS Path FROM UtentiUNSC INNER JOIN " & _
                 "RegioniCompetenze ON UtentiUNSC.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                 "WHERE UtentiUNSC.UserName ='" & Session("Utente") & "'"
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrLeggiDati.Read = True Then
            Session("Path") = dtrLeggiDati("Path")
            Session("Path") &= "/"
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        Else
            'Non c'è corrispondenza è successo qualcosa di inusuale;esco e torno alla logon
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
            Response.Redirect("LogOn.aspx")
        End If

        Dim idTipoProgetto As String = TipologiaProgettoDaEnteEBando(ddlBando.SelectedValue, Session("IdEnte"))
        If (idTipoProgetto = PROGETTO_GARANZIA_GIOVANI) Then
            DisabilitaDocumentiHelios()
        End If
    End Sub

    Private Sub DisabilitaDocumentiHelios()
        chkLetteraApprovazioneGraduatoriaEstero.Enabled = False
        chkLetteraApprovazioneGraduatoriaEsteroNazionale.Enabled = False
        chkletteraAssegnazioneS.Enabled = False
        chkletteraAssegnazioneM.Enabled = False
        DivletteraAssegnazioneM.Visible = False
        DivletteraAssegnazioneS.Visible = False
        DivLetteraApprovazioneGraduatoriaEstero.Visible = False
        DivLetteraApprovazioneGraduatoriaEsteroNazionale.Visible = False

    End Sub

    Function CaricaFile(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

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

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(a.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Denominazione, "
            strsql = strsql & "isnull(a.CodiceRegione,'') as CodiceRegione, "
            strsql = strsql & "isnull(a.IdClasseAccreditamentoRichiesta,'') as ClasseRichiesta, "
            strsql = strsql & "isnull(case len(day(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),day(a.DataCostituzione)) "
            strsql = strsql & "else convert(varchar(20),day(a.DataCostituzione))  end + '/' + "
            strsql = strsql & "(case len(month(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),month(a.DataCostituzione)) "
            strsql = strsql & "else convert(varchar(20),month(a.DataCostituzione))  end + '/' + "
            strsql = strsql & "Convert(varchar(20), Year(a.DataCostituzione))),'') as DataCostituzione,"
            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(b.indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Indirizzo, "
            strsql = strsql & "isnull(b.Civico,'') as Civico, "
            strsql = strsql & "isnull(b.CAP,'') as CAP, "
            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni.denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Comune, "
            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(provincie.provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Provincia "
            strsql = strsql & "from enti as a "
            strsql = strsql & "left join entisedi as b on a.idente=b.idente "
            strsql = strsql & "and b.identesede = any (SELECT pippo.identesede FROM entisedi pippo "
            strsql = strsql & "INNER JOIN entiseditipi pluto ON pippo.identesede = pluto.identesede "
            strsql = strsql & "WHERE pluto.idtiposede = 1) "
            strsql = strsql & "left join comuni on b.idcomune=comuni.idcomune "
            strsql = strsql & "left join provincie on comuni.idprovincia=provincie.idprovincia "
            strsql = strsql & "left join entiseditipi as c on b.identesede=c.identesede "
            strsql = strsql & "left join tipisedi as d on c.idtiposede=d.idtiposede "
            strsql = strsql & "left join statientisedi as e on b.idstatoentesede=e.idstatoentesede "
            strsql = strsql & "where a.IdEnte=" & Session("IdEnte") & " and ((d.tiposede='Principale') or (b.identesede is null))"

            'eseguo la query e passo il risultato al datareader
            dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".rtf"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/" & NomeFile & ".rtf"))
                Writer = New StreamWriter(strPercorsoFile)

                xLinea = Reader.ReadLine()

                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")
                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'chiudo lo streaming in lettura
                Writer.Close()
                Writer = Nothing

                'chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

                CaricaFile = "documentazione/" & strNomeFile

                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If

                'vado a fare la insert
                Dim cmdinsert As Data.SqlClient.SqlCommand
                strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
                strsql = strsql & "values "
                strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',2)"
                cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
                cmdinsert.ExecuteNonQuery()

                cmdinsert.Dispose()

            Else
                CaricaFile = ""
            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Return CaricaFile

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Sub NuovaCronologia(ByVal strDocumento As String)
        Dim strsql As String
        Dim dtrUtilizzato As SqlClient.SqlDataReader

        'controllo esistenza dati protocollazione: se esistono per id ente e documento 
        'allora li riporto anche nella insert
        Dim DtDataprot As String
        Dim strnprot As String
        Dim blnesiste As Boolean = False
        Dim cmdinsert As Data.SqlClient.SqlCommand

        strsql = "Select dataprot,nprot  from CronologiaEntiDocumenti where idente =" & Session("IdEnte") & _
        " and documento ='" & strDocumento & "' and idbando=" & ddlBando.SelectedItem.Value & " and isnull(nprot,'') <> ''"
        dtrUtilizzato = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrUtilizzato.HasRows = True Then
            dtrUtilizzato.Read()
            DtDataprot = dtrUtilizzato("dataprot")
            strnprot = dtrUtilizzato("nprot")
            blnesiste = True
        End If
        If Not dtrUtilizzato Is Nothing Then
            dtrUtilizzato.Close()
            dtrUtilizzato = Nothing
        End If

        If blnesiste = True Then
            'vado a fare la insert
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento,dataprot,nprot,idbando) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & strDocumento & "',2,'" & DtDataprot & "','" & strnprot & "'," & ddlBando.SelectedItem.Value & ")"
        Else
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento,idbando) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & strDocumento & "',2," & ddlBando.SelectedItem.Value & ")"
        End If

        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()

        cmdinsert.Dispose()
    End Sub

    Private Function ValidaDati() As Boolean
        Dim datiValidi As Boolean = True
        Dim dataBando As Date
        If ddlBando.SelectedValue Is Nothing Or ddlBando.SelectedValue = "" Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text & "Selezionare un Bando. <br/>"
            datiValidi = False
        End If
        If (chkLetteraApprovazioneGraduatoria.Checked = False And chkLetteraApprovazioneGraduatoriaEstero.Checked = False And chkLetteraApprovazioneGraduatoriaEsteroNazionale.Checked = False And chkletteraAssegnazioneS.Checked = False And chkletteraAssegnazioneM.Checked = False) Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text & "Selezionare almeno un file da generare. <br/>"
            datiValidi = False
        End If
        If txtdatadal.Text <> String.Empty And Date.TryParse(txtdatadal.Text, dataBando) = False Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text & "Il Campo 'Data' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>"
            datiValidi = False
        End If
        Return datiValidi
    End Function
    Private Function ValidaDatiProgramma(ByRef IdProgramma As Integer, ByRef IdBando As Integer, ByRef IdEnte As Integer, ByRef TipoModello As String) As Boolean
        Dim datiValidi As Boolean = True
        Dim dataProgramma As Date
        If (chkLetteraApprovazioneGraduatoriaProgramma.Checked = False) Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text & "Selezionare il file da generare. <br/>"
            datiValidi = False
        End If
        If txtdatadal1.Text <> String.Empty And Date.TryParse(txtdatadal1.Text, dataProgramma) = False Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text & "Il Campo 'Data' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>"
            datiValidi = False
        End If
        If txtProgramma.Text <> String.Empty = False Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text & "Inserire codice Programma. <br/>"
            datiValidi = False
        End If

        Dim dtrLocale As SqlClient.SqlDataReader
        Dim strsql As String

        strsql = "Select a.idprogramma, b.idbando, b.idente, isnull(c.particolarità,0) as particolarità, a.idtipoprogramma, d.nazionebase from programmi a inner join bandiprogrammi b on a.idbandoprogramma = b.idbandoprogramma inner join bando c on b.idbando = c.idbando inner join territori d on a.idterritorio = d.idterritorio where  codiceprogramma = '" & Replace(txtProgramma.Text.Trim, "'", "''") & "'"
        dtrLocale = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrLocale.HasRows = True Then
            dtrLocale.Read()
            IdProgramma = dtrLocale("idprogramma")
            IdBando = dtrLocale("idbando")
            IdEnte = dtrLocale("idente")
            If dtrLocale("nazionebase") = 0 Then
                TipoModello = "EST"
            ElseIf dtrLocale("idtipoprogramma") = 2 Then
                TipoModello = "GG"
            ElseIf dtrLocale("particolarità") = 1 Then
                TipoModello = "SCD"
            Else
                TipoModello = "ORD"
            End If

        End If
        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If

        Return datiValidi
    End Function

    Private Sub imgGeneraFile_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgGeneraFile.Click

        lblmessaggiosopra.Text = ""
        If (ValidaDati() = True) Then
            imgGeneraFile.Visible = False
            Dim utility As ClsUtility = New ClsUtility()

            Dim Documento As New GeneratoreModelli()

            If (Session("IdEnte") > -1) And (Not Session("IdEnte") Is Nothing) Then
                Dim idTipoProgetto As String = TipologiaProgettoDaEnteEBando(ddlBando.SelectedValue, Session("IdEnte"))
                'controllo se i due check sono flaggati e genero i doc da far scaricare

                If chkLetteraApprovazioneGraduatoria.Checked = True Then
                    If (idTipoProgetto = PROGETTO_GARANZIA_GIOVANI) Then
                        hplLetteraApprovazioneGraduatoria.Visible = True
                        DivLetteraApprovazioneGraduatoria.Visible = True
                        hplLetteraApprovazioneGraduatoria.NavigateUrl = Documento.VOL_LetteraApprovazioneGraduatoriaGaranziaGiovani(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal.Text)
                        NuovaCronologia("LetteraApprovazioneGraduatoriaGaranziaGiovani")
                    Else
                        'hplLetteraApprovazioneGraduatoriaCopiaReg.Visible = True
                        hplLetteraApprovazioneGraduatoria.Visible = True
                        DivLetteraApprovazioneGraduatoria.Visible = True
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraApprovazioneGraduatoria.NavigateUrl = Documento.VOL_letteraapprovazionegraduatoria(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal.Text)
                        '-- hplLetteraApprovazioneGraduatoriaCopiaReg.NavigateUrl = Documento.VOL_letteraapprovazionegraduatoriaCopiaReg(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Documento.Dispose()

                        NuovaCronologia("letteraapprovazionegraduatoria")
                        'Gruppo1(True)
                        'NuovaCronologia("letteraapprovazionegraduatoriaCopiaReg")
                        'Gruppo2(True)
                    End If
                End If
                If chkLetteraApprovazioneGraduatoriaEstero.Checked = True Then
                    hplLetteraApprovazioneGraduatoriaEstero.Visible = True
                    DivLetteraApprovazioneGraduatoriaEstero.Visible = True
                    'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                    hplLetteraApprovazioneGraduatoriaEstero.NavigateUrl = Documento.VOL_letteraapprovazionegraduatoriaestero(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal.Text)

                    NuovaCronologia("letteraapprovazionegraduatoriaestero")
                    'Gruppo3(True)
                End If
                If chkLetteraApprovazioneGraduatoriaEsteroNazionale.Checked = True Then
                    hplLetteraApprovazioneGraduatoriaEsteroNazionale.Visible = True
                    DivLetteraApprovazioneGraduatoriaEsteroNazionale.Visible = True
                    'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                    hplLetteraApprovazioneGraduatoriaEsteroNazionale.NavigateUrl = Documento.VOL_letteraapprovazionegraduatoriaesteronazionale(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal.Text)

                    NuovaCronologia("letteraapprovazionegraduatoriaesteronazionale")
                    'Gruppo4(True)
                End If
                'Aggiunto da Alessandra Taballione il 04/10/2005
                'implementazione stampe assegnazione volontari dopo approvazione graduatoria
                If chkletteraAssegnazioneS.Checked = True Then
                    hplletteraAssegnazioneS.Visible = True
                    DivletteraAssegnazioneS.Visible = True
                    'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                    hplletteraAssegnazioneS.NavigateUrl = Documento.VOL_letteraAssegnazioneVolontariodopoapprovazionegraduatoriaS(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    NuovaCronologia("letteraAssegnazioneVolontariodopoapprovazionegraduatoriaS")
                    'Gruppo5(True)
                End If
                If chkletteraAssegnazioneM.Checked = True Then
                    hplletteraAssegnazioneM.Visible = True
                    DivletteraAssegnazioneM.Visible = True
                    'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                    hplletteraAssegnazioneM.NavigateUrl = Documento.VOL_letteraAssegnazioneVolontariodopoapprovazionegraduatoriaM(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    NuovaCronologia("letteraAssegnazioneVolontariodopoapprovazionegraduatoriaM")
                    'Gruppo6(True)
                End If
                chkLetteraApprovazioneGraduatoriaEstero.Enabled = False
                chkLetteraApprovazioneGraduatoriaEsteroNazionale.Enabled = False
                chkLetteraApprovazioneGraduatoria.Enabled = False
                chkletteraAssegnazioneS.Enabled = False
                chkletteraAssegnazioneM.Enabled = False
                imgGeneraFile.Enabled = False
            Else
                lblmessaggiosopra.Visible = True
                lblmessaggiosopra.Text = lblmessaggiosopra.Text & "Occorre selezionare un ente."
            End If
            Documento.Dispose()
        End If

    End Sub

    'Sub CaricaProgetti()
    '    Dim strsql As String

    '    'chiudo il datareader
    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If

    '    strsql = "Select distinct isnull(replace(replace(replace(replace(replace(replace(replace(replace(a.Titolo,'°',''),'ì','i'''),'é','e'''),  'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'’',''''),'') as Titolo, "
    '    strsql = strsql & "isnull(case len(day(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),day(a.DataInizioAttività)) else convert(varchar(20),day(a.DataInizioAttività))  end + '/' + (case len(month(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),month(a.DataInizioAttività)) else convert(varchar(20),month(a.DataInizioAttività))  end + '/' + Convert(varchar(20), Year(a.DataInizioAttività))),'xx/xx/xxxx') as DataInizioPrevista, "
    '    strsql = strsql & "isnull(a.CodiceEnte, 'Nessun Codice') as CodiceEnte "
    '    strsql = strsql & "from attività as a "
    '    strsql = strsql & "inner join bandiattività as ba on ba.idbandoattività = a.idbandoattività "
    '    strsql = strsql & "inner join attivitàsediassegnazione as b on a.idattività=b.idattività "
    '    strsql = strsql & "inner join enti as c on a.identepresentante=c.idente "
    '    strsql = strsql & "where ba.idbando = " & ddlBando.SelectedValue & " and b.statograduatoria=3 and a.IdEntePresentante=" & CInt(Session("IdEnte"))

    '    'eseguo la query e passo il risultato al datareader
    '    'dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    '    DTProva = ClsServer.CreaDataTable(strsql, False, Session("conn"))
    'End Sub
    'Sub CaricaProgettiVolontariVincolo()
    '    Dim strsql As String

    '    'chiudo il datareader
    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If

    '    strsql = "Select distinct isnull(replace(replace(replace(replace(replace(replace(replace(replace(a.Titolo,'°',''),'ì','i'''),'é','e'''),  'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'’',''''),'') as Titolo, "
    '    strsql = strsql & "isnull(case len(day(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),day(a.DataInizioAttività)) else convert(varchar(20),day(a.DataInizioAttività))  end + '/' + (case len(month(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),month(a.DataInizioAttività)) else convert(varchar(20),month(a.DataInizioAttività))  end + '/' + Convert(varchar(20), Year(a.DataInizioAttività))),'xx/xx/xxxx') as DataInizioPrevista, "
    '    strsql = strsql & "isnull(a.CodiceEnte, 'Nessun Codice') as CodiceEnte,vincoli.Vincolo, isnull(vincoli.descrizione,'') as Descrizione, entità.Cognome, entità.Nome, FlagEntità.Valore , isnull(flagEntità.notaStorico,'') as Note "
    '    strsql = strsql & "from attività as a "
    '    strsql = strsql & "inner join bandiattività as ba on ba.idbandoattività = a.idbandoattività "
    '    strsql = strsql & "inner join attivitàsediassegnazione as b on a.idattività=b.idattività "
    '    strsql = strsql & "inner join enti as c on a.identepresentante=c.idente "
    '    strsql = strsql & "INNER JOIN GraduatorieEntità ON b.IDAttivitàSedeAssegnazione = GraduatorieEntità.IdAttivitàSedeAssegnazione "
    '    strsql = strsql & "INNER JOIN entità ON GraduatorieEntità.IdEntità = entità.IDEntità "
    '    strsql = strsql & "INNER JOIN FlagEntità ON entità.IDEntità = FlagEntità.IdEntità "
    '    strsql = strsql & "INNER JOIN vincoli ON FlagEntità.IdVincolo = vincoli.IDVincolo "
    '    strsql = strsql & "where ba.idbando = " & ddlBando.SelectedValue & "  AND FlagEntità.Valore = 0 and b.statograduatoria=3 and a.IdEntePresentante=" & CInt(Session("IdEnte"))
    '    'CON DATA READER
    '    'eseguo la query e passo il risultato al datareader
    '    'dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    '    'CON DATA TABLE
    '    TBLLeggiDati = ClsServer.CreaDataTable(strsql, False, Session("conn"))
    'End Sub

    Sub CaricaDati()
        Dim strsql As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(a.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Denominazione, "
        strsql = strsql & "isnull(a.CodiceRegione,'') as CodiceRegione, "
        strsql = strsql & "isnull(a.IdClasseAccreditamentoRichiesta,'') as ClasseRichiesta, "
        strsql = strsql & "isnull(case len(day(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),day(a.DataCostituzione)) "
        strsql = strsql & "else convert(varchar(20),day(a.DataCostituzione))  end + '/' + "
        strsql = strsql & "(case len(month(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),month(a.DataCostituzione)) "
        strsql = strsql & "else convert(varchar(20),month(a.DataCostituzione))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(a.DataCostituzione))),'') as DataCostituzione,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(b.indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Indirizzo, "
        strsql = strsql & "isnull(b.Civico,'') as Civico, "
        strsql = strsql & "isnull(b.CAP,'') as CAP, "
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni.denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Comune, "
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(provincie.provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Provincia "
        strsql = strsql & "from enti as a "
        strsql = strsql & "left join entisedi as b on a.idente=b.idente "
        strsql = strsql & "and b.identesede = any (SELECT pippo.identesede FROM entisedi pippo "
        strsql = strsql & "INNER JOIN entiseditipi pluto ON pippo.identesede = pluto.identesede "
        strsql = strsql & "WHERE pluto.idtiposede = 1) "
        strsql = strsql & "left join comuni on b.idcomune=comuni.idcomune "
        strsql = strsql & "left join provincie on comuni.idprovincia=provincie.idprovincia "
        strsql = strsql & "left join entiseditipi as c on b.identesede=c.identesede "
        strsql = strsql & "left join tipisedi as d on c.idtiposede=d.idtiposede "
        strsql = strsql & "left join statientisedi as e on b.idstatoentesede=e.idstatoentesede "
        strsql = strsql & "where a.IdEnte=" & Session("IdEnte") & " and ((d.tiposede='Principale') or (b.identesede is null))"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub

    'Function LetteraApprovazioneGraduatoria(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

    '    Dim xStr As String
    '    Dim xLinea As String
    '    Dim Writer As StreamWriter
    '    Dim Reader As StreamReader
    '    Dim strPercorsoFile As String
    '    Dim strsql As String
    '    Dim strDataOdierna As String
    '    Dim strNomeFile As String

    '    Try
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        'prendo la data dal server
    '        dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
    '        dtrLeggiDati.Read()
    '        'passo la data odierna ad una variabile locale
    '        strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If


    '        Dim x As New clsDocumentiRegioni
    '        x.RecuperaDatiGraduatorie(CStr(Session("IdEnte")), ddlBando.SelectedValue, Session("conn"))

    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If




    '        'parla da sola
    '        CaricaDati()

    '        If dtrLeggiDati.HasRows = True Then
    '            dtrLeggiDati.Read()
    '            'creo il nome del file
    '            strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
    '            'creo il percorso del file da salvare
    '            strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

    '            'apro il file che fa da template
    '            Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.Default, False)

    '            Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

    '            xLinea = Reader.ReadLine()


    '            Dim strDenominazioneEnte As String = dtrLeggiDati("Denominazione")
    '            Dim strClasseRichiesta As String = dtrLeggiDati("ClasseRichiesta") & "^"
    '            Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")
    '            Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
    '            Dim strNumeroCivico As String = dtrLeggiDati("Civico")
    '            Dim strCAP As String = dtrLeggiDati("CAP")
    '            Dim strComune As String = dtrLeggiDati("Comune")
    '            Dim strProvincia As String = dtrLeggiDati("provincia")
    '            Dim strCodiceRegione As String = dtrLeggiDati("CodiceRegione")
    '            Dim blnSalta As Boolean
    '            CaricaProgetti()
    '            CaricaProgettiVolontariVincolo()
    '            While xLinea <> ""
    '                blnSalta = False
    '                xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
    '                xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
    '                xLinea = Replace(xLinea, "<ClasseRichiesta>", strClasseRichiesta)
    '                xLinea = Replace(xLinea, "<DataCostituzione>", strDataCostituzione)
    '                xLinea = Replace(xLinea, "<Indirizzo>", strIndirizzo)
    '                xLinea = Replace(xLinea, "<NumeroCivico>", strNumeroCivico)
    '                xLinea = Replace(xLinea, "<CAP>", strCAP)
    '                xLinea = Replace(xLinea, "<Comune>", strComune)
    '                xLinea = Replace(xLinea, "<Provincia>", strProvincia)
    '                xLinea = Replace(xLinea, "<CodiceRegione>", strCodiceRegione)
    '                'xLinea = Replace(xLinea, "<Gazzetta>", strGazzetta)
    '                'xLinea = Replace(xLinea, "<Nvolo>", strNVol)



    '                xLinea = Replace(xLinea, "<Gazzetta>", x.Gazzetta)
    '                xLinea = Replace(xLinea, "<NVol>", x.NVolontari)
    '                xLinea = Replace(xLinea, "<IntestazioneRegione>", x.Intestazione)
    '                xLinea = Replace(xLinea, "<SettoreRegione>", x.Settore)
    '                xLinea = Replace(xLinea, "<IndirizzoRegione>", x.Indirizzo)
    '                xLinea = Replace(xLinea, "<CapRegione>", x.Cap)
    '                xLinea = Replace(xLinea, "<LocalitaRegione>", x.Località)


    '                Try

    '                    If InStr(xLinea, "<BreakPoint>") > 0 Then
    '                        blnSalta = True
    '                        xLinea = Replace(xLinea, "<BreakPoint>", "") & "\par"
    '                        If DTProva.Rows.Count > 0 Then
    '                            For Each myRow In DTProva.Rows
    '                                Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
    '                                Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
    '                                Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
    '                                Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
    '                                Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                                Writer.WriteLine("\cellx3300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                                Writer.WriteLine("\cellx6670\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                                Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\b " & myRow.Item("Titolo") & "\cell " & myRow.Item("CodiceEnte") & "\cell " & myRow.Item("DatainizioPrevista") & "\b0\cell\row\pard\f2\fs20")
    '                                'xLinea = "\b " & dtrLeggiDati("Titolo") & "\tqc " & dtrLeggiDati("CodiceEnte") & "\tqc " & dtrLeggiDati("DatainizioPrevista") & "\b0\par"
    '                                'Writer.WriteLine(xLinea)
    '                            Next
    '                            Writer.WriteLine("\par")
    '                        End If
    '                    End If


    '                    'Writer.WriteLine(xLinea)

    '                    'xLinea = Reader.ReadLine()


    '                    'Antonello-------------VINCOLI--------------------------------------------------------------------



    '                    If InStr(xLinea, "<BreakPoint1>") > 0 Then
    '                        'blnSalta = True
    '                        xLinea = Replace(xLinea, "<BreakPoint1>", "") & "\par"
    '                        If TBLLeggiDati.Rows.Count > 0 Then
    '                            For Each myRow In TBLLeggiDati.Rows
    '                                'IntY = TBLLeggiDati.FieldCount
    '                                'While dtrLeggiDati.Read
    '                                Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
    '                                Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
    '                                Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
    '                                Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
    '                                Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                                'Writer.WriteLine("\cellx3300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                                Writer.WriteLine("\cellx4000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                                Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\b " & myRow.Item("Cognome") & "  " & myRow.Item("Nome") & "\cell " & IIf(myRow.Item("Descrizione") = "", myRow.Item("Note"), myRow.Item("Descrizione")) & "\b0\cell\row\pard\f2\fs20")
    '                                'xLinea = "\b " & dtrLeggiDati("Titolo") & "\tqc " & dtrLeggiDati("CodiceEnte") & "\tqc " & dtrLeggiDati("DatainizioPrevista") & "\b0\par"
    '                                'Writer.WriteLine(xLinea)
    '                            Next
    '                        End If
    '                        Writer.WriteLine("\par")
    '                    End If

    '                    '---------------------------------------------------------------------------------



    '                    Writer.WriteLine(xLinea)

    '                    xLinea = Reader.ReadLine()


    '                Catch ex As Exception
    '                    Dim pippo As String = ex.Message
    '                End Try

    '            End While

    '            Writer.Close()
    '            Writer = Nothing

    '            ''chiudo lo streaming in scrittura
    '            Reader.Close()
    '            Reader = Nothing

    '        End If

    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        LetteraApprovazioneGraduatoria = "documentazione/" & strNomeFile


    '        'vado a fare la insert
    '        Dim cmdinsert As Data.SqlClient.SqlCommand
    '        strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
    '        strsql = strsql & "values "
    '        strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',2)"
    '        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
    '        cmdinsert.ExecuteNonQuery()

    '        cmdinsert.Dispose()

    '    Catch ex As Exception
    '        Response.Write(ex.Message)
    '    End Try

    'End Function

    Function Prova() As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        strNomeFile = "Documento" & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
        'creo il percorso del file da salvare
        strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

        'apro il file che fa da template
        Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & "Documento" & ".rtf"))

        Writer = New StreamWriter(strPercorsoFile)

        'Writer.WriteLine("{\rtf1")

        'apro il template
        xLinea = Reader.ReadLine()
        While xLinea <> ""

            Writer.WriteLine(xLinea)

            xLinea = Reader.ReadLine()
        End While


        Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
        Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
        Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
        Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
        Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
        Writer.WriteLine("\cellx3300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
        Writer.WriteLine("\cellx6670\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
        Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj primo\cell secondo\cell terzo\cell\row\pard\f2\fs20")
        Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
        Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
        Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
        Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
        Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
        Writer.WriteLine("\cellx3300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
        Writer.WriteLine("\cellx6670\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
        Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj primo\cell secondo\cell terzo\cell\row\pard\f2\fs20")
        Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
        Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
        Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
        Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
        Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
        Writer.WriteLine("\cellx3300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
        Writer.WriteLine("\cellx6670\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
        Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj primo\cell secondo\cell terzo\cell\row\pard\f2\fs20\par")


        ''inserireblocco progetti

        'close the RTF string and file
        'Writer.WriteLine("}")
        Writer.Close()
        Writer = Nothing

        ''chiudo lo streaming in scrittura
        Reader.Close()
        Reader = Nothing

    End Function

    Private Sub imgChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgChiudi.Click
        If (HdValoreSalva.Value = "1" Or hddModificaProtocollo.Value = "1") Then
            Call Salvataggio()
        End If
        Response.Redirect("WfrmMain.aspx")

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Prova()
    End Sub

    Private Sub PopolaCombo()
        Dim strSql As String
        'strSql = "select distinct bando.* from bando " & _
        '            " INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando " & _
        '            " INNER JOIN attività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività " & _
        '            " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
        '            " inner join statibando on statibando.idstatobando=bando.idstatobando " & _
        '            " inner join AssociaBandoTipiProgetto on bando.idbando = AssociaBandoTipiProgetto.idbando " & _
        '            " inner join TipiProgetto on AssociaBandoTipiProgetto.idtipoprogetto = TipiProgetto.idtipoprogetto " & _
        '            " where TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' and bandiattività.idente= " & Session("IdEnte") & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataInizioVolontari ELSE BANDORICORSI.DataInizioVolontari END is not null order by bando.idbando desc"
        strSql = "select distinct bando.* from bando " & _
                    " INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando " & _
                    " INNER JOIN attività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività " & _
                    " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
                    " inner join statibando on statibando.idstatobando=bando.idstatobando " & _
                    " inner join AssociaBandoTipiProgetto on bando.idbando = AssociaBandoTipiProgetto.idbando " & _
                    " inner join TipiProgetto on attività.idtipoprogetto = TipiProgetto.idtipoprogetto " & _
                    " where TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' and attività.identepresentante= " & Session("IdEnte") & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataInizioVolontari ELSE BANDORICORSI.DataInizioVolontari END is not null order by bando.idbando desc"

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(strSql, Session("conn"))
        ddlBando.DataSource = dtrLeggiDati
        ddlBando.DataValueField = "idbando"
        ddlBando.DataTextField = "bandobreve"
        ddlBando.DataBind()
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
    End Sub

    Private Sub cmdFascCanc_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCanc.Click
        TxtNumeroFascicolo.Text = ""
        TxtCodiceFasc.Value = ""
        txtDescFasc.Text = ""
        TxtNumProtocollo1.Text = ""
        TxtDataProtocollo1.Text = ""
        TxtNumProtocollo2.Text = ""
        TxtDataProtocollo2.Text = ""
        TxtNumProtocollo3.Text = ""
        TxtDataProtocollo3.Text = ""
        TxtNumProtocollo4.Text = ""
        TxtDataProtocollo4.Text = ""
        TxtNumProtocollo5.Text = ""
        TxtDataProtocollo5.Text = ""
        TxtNumProtocollo6.Text = ""
        TxtDataProtocollo6.Text = ""
        'salva i dati vuoti
        Dim strsql As String
        'vado a fare update ma si legge ;)

        Dim cmdupdate As Data.SqlClient.SqlCommand
        strsql = "update enti set codicefascicolo= NULL, idfascicolo =NULL, descrfascicolo=NULL"
        cmdupdate = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdupdate.ExecuteNonQuery()
        cmdupdate.Dispose()
    End Sub
    Sub SalvaFascicolo()
        Dim strsql As String
        'vado a fare update ma si legge ;)

        Dim cmdupdate As Data.SqlClient.SqlCommand
        strsql = "update ENTI set codicefascicoloai=' " & TxtNumeroFascicolo.Text & _
        "', idfascicoloai ='" & TxtCodiceFasc.Value & "', descrfascicoloai='" & txtDescFasc.Text
        strsql = strsql & "' where  idente = " & Session("IdEnte")

        cmdupdate = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdupdate.ExecuteNonQuery()
        cmdupdate.Dispose()

    End Sub

    Sub CaricaFascicolo()
        Dim dtrUtilizzato As SqlClient.SqlDataReader
        Dim strsql As String

        strsql = "Select codicefascicoloai,idfascicoloai,descrfascicoloai  from "
        strsql = strsql & " ENTI where  idente = " & Session("IdEnte")
        dtrUtilizzato = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrUtilizzato.HasRows = True Then
            dtrUtilizzato.Read()

            If IsDBNull(dtrUtilizzato("codicefascicoloai")) = False Then
                TxtNumeroFascicolo.Text = Trim(dtrUtilizzato("codicefascicoloai"))
                TxtNumFascicoloControllo.Value = Trim(dtrUtilizzato("codicefascicoloai"))
                TxtCodiceFasc.Value = dtrUtilizzato("idfascicoloai")
                txtDescFasc.Text = dtrUtilizzato("descrfascicoloai")
            Else
                TxtNumeroFascicolo.Text = ""
                TxtNumFascicoloControllo.Value = ""
                TxtCodiceFasc.Value = ""
                txtDescFasc.Text = ""
            End If
        End If
        If Not dtrUtilizzato Is Nothing Then
            dtrUtilizzato.Close()
            dtrUtilizzato = Nothing
        End If
        '********
    End Sub

    Sub cancella()
        Dim strLocal As String
        Dim dtrCancellazione As SqlClient.SqlDataReader
        Dim mycommand As New SqlClient.SqlCommand
        Dim mydatatable As New DataTable

        mycommand.Connection = Session("conn")

        'cancella
        'strLocal = "select IDAttivitàSedeAssegnazione from attività inner join attivitàsediassegnazione on " & _
        '"attività.idattività = attivitàsediassegnazione.idattività where(attività.idattività = " & Request.QueryString("IdAttivita") & ")"
        Try
            'mydatatable = ClsServer.CreaDataTable(strLocal, False, Session("conn"))

            'Dim k As Int16

            'For k = 0 To mydatatable.Rows.Count - 1
            strLocal = "update cronologiaentidocumenti set dataprot =null,  nprot = null where idente = " & CInt(Session("IdEnte")) & _
            " and idbando =" & ddlBando.SelectedValue

            mycommand.CommandText = strLocal
            mycommand.ExecuteNonQuery()
            'Next
            '*******************************************************************************
        Catch ex As Exception
            'Response.Write(ex.Message.ToString())
        End Try

        'strsql = "update bandiattività set codicefascicoloai=' " & TxtNumeroFascicolo.Text & _
        '       "', idfascicoloai ='" & TxtCodiceFasc.Text & "', descrfascicoloai='" & txtDescFasc.Text
        'strsql = strsql & "' where idbando =" & ddlBando.SelectedValue & " and idente = " & Session("IdEnte")

        'mycommand.CommandText = strLocal
        ' mycommand.ExecuteNonQuery()

    End Sub


    Sub SalvaProtocolli(ByVal StrDataprot As String, ByVal StrNumProt As String, ByVal StringaDocumento As String)

        Dim strsql As String
        'vado a fare update
        Dim cmdupdate As Data.SqlClient.SqlCommand
        If StrDataprot = "" Then
            strsql = "update cronologiaentidocumenti set dataprot=null, nprot=null"
        Else
            strsql = "update cronologiaentidocumenti set dataprot='" & StrDataprot & "', nprot='" & StrNumProt & "'"
        End If
        strsql = strsql & " where idente = " & CInt(Session("IdEnte")) & _
        " and documento ='" & StringaDocumento & "' and idbando =" & ddlBando.SelectedValue

        cmdupdate = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdupdate.ExecuteNonQuery()
        cmdupdate.Dispose()
    End Sub

    Sub CaricaProtocollo()
        Dim dtrUtilizzato As SqlClient.SqlDataReader
        Dim strsql As String
        Dim strIDbando As String
        strIDbando = ddlBando.SelectedValue
        If strIDbando = "" Then
            strIDbando = "0"
        End If

        strsql = "select * from cronologiaentidocumenti where idente = " & CInt(Session("IdEnte")) & _
        " and idbando =" & strIDbando 'ddlBando.SelectedValue

        dtrUtilizzato = ClsServer.CreaDatareader(strsql, Session("conn"))

        Gruppo1(False)
        Gruppo2(False)
        Gruppo3(False)
        Gruppo4(False)
        Gruppo5(False)
        Gruppo6(False)


        Do While dtrUtilizzato.Read()
            Select Case dtrUtilizzato("Documento")

                Case "letteraapprovazionegraduatoria"
                    '  Gruppo1(True)
                    TxtNumProtocollo1.Text = "" & dtrUtilizzato("NProt")
                    TxtDataProtocollo1.Text = "" & dtrUtilizzato("DataProt")

                Case "letteraapprovazionegraduatoriaCopiaReg"
                    ' Gruppo2(True)
                    TxtNumProtocollo2.Text = "" & dtrUtilizzato("NProt")
                    TxtDataProtocollo2.Text = "" & dtrUtilizzato("DataProt")

                Case "letteraapprovazionegraduatoriaestero"
                    ' Gruppo3(True)
                    TxtNumProtocollo3.Text = "" & dtrUtilizzato("NProt")
                    TxtDataProtocollo3.Text = "" & dtrUtilizzato("DataProt")

                Case "letteraapprovazionegraduatoriaesteronazionale"
                    ' Gruppo4(True)
                    TxtNumProtocollo4.Text = "" & dtrUtilizzato("NProt")
                    TxtDataProtocollo4.Text = "" & dtrUtilizzato("DataProt")

                Case "letteraAssegnazioneVolontariodopoapprovazionegraduatoriaS"
                    'Gruppo5(True)
                    TxtNumProtocollo5.Text = "" & dtrUtilizzato("NProt")
                    TxtDataProtocollo5.Text = "" & dtrUtilizzato("DataProt")

                Case "letteraAssegnazioneVolontariodopoapprovazionegraduatoriaM"
                    'Gruppo6(True)
                    TxtNumProtocollo6.Text = "" & dtrUtilizzato("NProt")
                    TxtDataProtocollo6.Text = "" & dtrUtilizzato("DataProt")

            End Select
        Loop

        'mettere il visible sull'attivazione della freccietta per stampare

        If Not dtrUtilizzato Is Nothing Then
            dtrUtilizzato.Close()
            dtrUtilizzato = Nothing
        End If

    End Sub
    Private Sub Gruppo1(ByVal BlValore As Boolean)
        LblNumProtocollo1.Visible = BlValore
        If TxtNumProtocollo1.Visible = False Then
            TxtNumProtocollo1.Text = ""
            TxtDataProtocollo1.Text = ""
        End If
        TxtNumProtocollo1.Visible = BlValore
        cmdSelProtocollo1.Visible = BlValore
        cmdScAllegati1.Visible = BlValore
        cmdNuovoFascicolo1.Visible = BlValore
        LblDataProtocollo1.Visible = BlValore
        TxtDataProtocollo1.Visible = BlValore

    End Sub
    Private Sub Gruppo2(ByVal BlValore As Boolean)
        If TxtNumProtocollo2.Visible = False Then
            TxtNumProtocollo2.Text = ""
            TxtDataProtocollo2.Text = ""
        End If
        LblNumProtocollo2.Visible = BlValore
        TxtNumProtocollo2.Visible = BlValore
        cmdSelProtocollo2.Visible = BlValore
        cmdScAllegati2.Visible = BlValore
        cmdNuovoFascicolo2.Visible = BlValore
        LblDataProtocollo2.Visible = BlValore
        TxtDataProtocollo2.Visible = BlValore

    End Sub
    Private Sub Gruppo3(ByVal BlValore As Boolean)
        If TxtNumProtocollo3.Visible = False Then
            TxtNumProtocollo3.Text = ""
            TxtDataProtocollo3.Text = ""
        End If
        LblNumProtocollo3.Visible = BlValore
        TxtNumProtocollo3.Visible = BlValore
        cmdSelProtocollo3.Visible = BlValore
        cmdScAllegati3.Visible = BlValore
        cmdNuovoFascicolo3.Visible = BlValore
        LblDataProtocollo3.Visible = BlValore
        TxtDataProtocollo3.Visible = BlValore

    End Sub
    Private Sub Gruppo4(ByVal BlValore As Boolean)
        If TxtNumProtocollo4.Visible = False Then
            TxtNumProtocollo4.Text = ""
            TxtDataProtocollo4.Text = ""
        End If
        LblNumProtocollo4.Visible = BlValore
        TxtNumProtocollo4.Visible = BlValore
        cmdSelProtocollo4.Visible = BlValore
        cmdScAllegati4.Visible = BlValore
        cmdNuovoFascicolo4.Visible = BlValore
        LblDataProtocollo4.Visible = BlValore
        TxtDataProtocollo4.Visible = BlValore

    End Sub
    Private Sub Gruppo5(ByVal BlValore As Boolean)
        If TxtNumProtocollo5.Visible = False Then
            TxtNumProtocollo5.Text = ""
            TxtDataProtocollo5.Text = ""
        End If
        LblNumProtocollo5.Visible = BlValore
        TxtNumProtocollo5.Visible = BlValore
        cmdSelProtocollo5.Visible = BlValore
        cmdScAllegati5.Visible = BlValore
        cmdNuovoFascicolo5.Visible = BlValore
        LblDataProtocollo5.Visible = BlValore
        TxtDataProtocollo5.Visible = BlValore

    End Sub
    Private Sub Gruppo6(ByVal BlValore As Boolean)
        If TxtNumProtocollo6.Visible = False Then
            TxtNumProtocollo6.Text = ""
            TxtDataProtocollo6.Text = ""
        End If
        LblNumProtocollo6.Visible = BlValore
        TxtNumProtocollo6.Visible = BlValore
        cmdSelProtocollo6.Visible = BlValore
        cmdScAllegati6.Visible = BlValore
        cmdNuovoFascicolo6.Visible = BlValore
        LblDataProtocollo6.Visible = BlValore
        TxtDataProtocollo6.Visible = BlValore

    End Sub


    Sub ResettaMaschera()
        imgGeneraFile.Enabled = True
        imgGeneraFile.Visible = True
        Dim idTipoProgetto As String = TipologiaProgettoDaEnteEBando(ddlBando.SelectedValue, Session("IdEnte"))
        If (idTipoProgetto = PROGETTO_GARANZIA_GIOVANI) Then
            chkLetteraApprovazioneGraduatoria.Enabled = True
            chkLetteraApprovazioneGraduatoria.Checked = False
            hplLetteraApprovazioneGraduatoria.Visible = False
            DivLetteraApprovazioneGraduatoria.Visible = False


            chkLetteraApprovazioneGraduatoriaEstero.Enabled = False
            chkLetteraApprovazioneGraduatoriaEstero.Checked = False
            chkLetteraApprovazioneGraduatoriaEsteroNazionale.Enabled = False
            chkLetteraApprovazioneGraduatoriaEsteroNazionale.Checked = False
            chkletteraAssegnazioneS.Enabled = False
            chkletteraAssegnazioneS.Checked = False
            chkletteraAssegnazioneM.Enabled = False
            chkletteraAssegnazioneM.Checked = False
            hplLetteraApprovazioneGraduatoria.Visible = False
            'hplLetteraApprovazioneGraduatoriaCopiaReg.Visible = False
            hplLetteraApprovazioneGraduatoriaEstero.Visible = False
            hplLetteraApprovazioneGraduatoriaEsteroNazionale.Visible = False
            hplletteraAssegnazioneS.Visible = False
            hplletteraAssegnazioneM.Visible = False
        Else
            chkLetteraApprovazioneGraduatoria.Enabled = True
            chkLetteraApprovazioneGraduatoria.Checked = False
            chkLetteraApprovazioneGraduatoriaEstero.Enabled = True
            chkLetteraApprovazioneGraduatoriaEstero.Checked = False
            chkLetteraApprovazioneGraduatoriaEsteroNazionale.Enabled = True
            chkLetteraApprovazioneGraduatoriaEsteroNazionale.Checked = False
            chkletteraAssegnazioneS.Enabled = True
            chkletteraAssegnazioneS.Checked = False
            chkletteraAssegnazioneM.Enabled = True
            chkletteraAssegnazioneM.Checked = False
            hplLetteraApprovazioneGraduatoria.Visible = False
            'hplLetteraApprovazioneGraduatoriaCopiaReg.Visible = False
            hplLetteraApprovazioneGraduatoriaEstero.Visible = False
            hplLetteraApprovazioneGraduatoriaEsteroNazionale.Visible = False
            hplletteraAssegnazioneS.Visible = False
            hplletteraAssegnazioneM.Visible = False
            DivLetteraApprovazioneGraduatoria.Visible = False
            DivLetteraApprovazioneGraduatoriaEstero.Visible = False
            DivLetteraApprovazioneGraduatoriaEsteroNazionale.Visible = False
            DivletteraAssegnazioneM.Visible = False
            DivletteraAssegnazioneS.Visible = False
        End If
    End Sub


    Private Sub Salva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles Salva.Click

        Call Salvataggio()

    End Sub
    Sub Salvataggio()
        'If TxtNumeroFascicolo.Text <> "" Then
        If TxtNumFascicoloControllo.Value <> "" Then
            If Trim(TxtNumeroFascicolo.Text) <> Trim(TxtNumFascicoloControllo.Value) Then
                Call cancella()
            End If
        End If
        Call SalvaFascicolo()

        'If TxtNumProtocollo1.Text <> "" Then
        Call SalvaProtocolli(TxtDataProtocollo1.Text, TxtNumProtocollo1.Text, "letteraapprovazionegraduatoria")
        'End If
        'If TxtNumProtocollo2.Text <> "" Then
        Call SalvaProtocolli(TxtDataProtocollo2.Text, TxtNumProtocollo2.Text, "letteraapprovazionegraduatoriaCopiaReg")
        'End If
        'If TxtNumProtocollo3.Text <> "" Then
        Call SalvaProtocolli(TxtDataProtocollo3.Text, TxtNumProtocollo3.Text, "letteraapprovazionegraduatoriaestero")
        'End If
        'If TxtNumProtocollo4.Text <> "" Then
        Call SalvaProtocolli(TxtDataProtocollo4.Text, TxtNumProtocollo4.Text, "letteraapprovazionegraduatoriaesteronazionale")
        'End If
        'If TxtNumProtocollo5.Text <> "" Then
        Call SalvaProtocolli(TxtDataProtocollo5.Text, TxtNumProtocollo5.Text, "letteraAssegnazioneVolontariodopoapprovazionegraduatoriaS")
        'End If
        'If TxtNumProtocollo6.Text <> "" Then
        Call SalvaProtocolli(TxtDataProtocollo6.Text, TxtNumProtocollo6.Text, "letteraAssegnazioneVolontariodopoapprovazionegraduatoriaM")
        'End If
        'Else
        'messaggio - sicuro di cancellare?
        'End If
        TxtNumFascicoloControllo.Value = Trim(TxtNumeroFascicolo.Text)
        hddModificaProtocollo.Value = 0
    End Sub

    Function TipologiaProgettoDaEnteEBando(ByVal bando As String, ByVal idEnte As String) As String
        Dim query As String
        Dim idTipoProgetto As String = ""
        Dim dataReader As SqlDataReader
        'patch gestione tipi progetto diversi per stessa istanza 15/03/2021
        If Session("Sistema") = "Helios" Then
            idTipoProgetto = ""
        Else
            idTipoProgetto = "4"
        End If
        Return idTipoProgetto

        'prima:
        'query = "Select top(1) 4 as IdTipoProgetto " & _
        '"from bando " & _
        '"INNER JOIN AssociaBandoTipiProgetto ON bando.IDBando = AssociaBandoTipiProgetto.IdBando " & _
        ' "WHERE AssociaBandoTipiProgetto.IdTipoProgetto = 4 AND bando.IDBando = " & bando

        'dataReader = ClsServer.CreaDatareader(query, Session("conn"))
        'If dataReader.HasRows = True Then
        '    dataReader.Read()
        '    idTipoProgetto = dataReader("IdTipoProgetto")
        'End If
        'dataReader.Close()


        Return idTipoProgetto
    End Function



    Protected Sub ddlBando_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlBando.SelectedIndexChanged
        Call ResettaMaschera()
        Call CaricaFascicolo()
        Call CaricaProtocollo()
    End Sub
    Protected Sub imgGenerafileProgamma_Click(sender As Object, e As EventArgs) Handles imgGenerafileProgamma.Click
        Dim intIdProgramma As Integer
        Dim intIdBando As Integer
        Dim intIdEnte As Integer
        Dim strTipoModello As String

        lblmessaggiosopra.Text = ""
        If (ValidaDatiProgramma(intIdProgramma, intIdBando, intIdEnte, strTipoModello) = True) Then
            imgGenerafileProgamma.Visible = False
            Dim utility As ClsUtility = New ClsUtility()

            Dim Documento As New GeneratoreModelli()

            'If (Session("IdEnte") > -1) And (Not Session("IdEnte") Is Nothing) Then
            Dim idTipoProgetto As String = TipologiaProgettoDaEnteEBando(ddlBando.SelectedValue, Session("IdEnte"))
            'controllo se i due check sono flaggati e genero i doc da far scaricare

            If chkLetteraApprovazioneGraduatoriaProgramma.Checked = True Then
                'If (idTipoProgetto = PROGETTO_GARANZIA_GIOVANI) Then
                '    hplLetteraApprovazioneGraduatoriaProgramma.Visible = True
                '    'DivLetteraApprovazioneGraduatoria.Visible = True
                '    hplLetteraApprovazioneGraduatoriaProgramma.NavigateUrl = Documento.VOL_LetteraApprovazioneGraduatoriaProgrammaGaranziaGiovani(intIdEnte, intIdBando, intIdProgramma, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal1.Text)
                '    NuovaCronologia("LetteraApprovazioneGraduatoriaProgrammiGaranziaGiovani")
                'Else
                '    'hplLetteraApprovazioneGraduatoriaCopiaReg.Visible = True
                '    hplLetteraApprovazioneGraduatoriaProgramma.Visible = True
                '    'DivLetteraApprovazioneGraduatoria.Visible = True
                '    'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                '    hplLetteraApprovazioneGraduatoriaProgramma.NavigateUrl = Documento.VOL_LetteraApprovazioneGraduatoriaProgramma(intIdEnte, intIdBando, intIdProgramma, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal1.Text)
                '    '-- hplLetteraApprovazioneGraduatoriaCopiaReg.NavigateUrl = Documento.VOL_letteraapprovazionegraduatoriaCopiaReg(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                '    Documento.Dispose()
                '    NuovaCronologia("letteraapprovazionegraduatoriaprogramma")
                'End If
                Select Case strTipoModello
                    Case "ORD"
                        hplLetteraApprovazioneGraduatoriaProgramma.Visible = True
                        hplLetteraApprovazioneGraduatoriaProgramma.NavigateUrl = Documento.VOL_LetteraApprovazioneGraduatoriaProgramma(intIdEnte, intIdBando, intIdProgramma, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal1.Text)
                        NuovaCronologia("letteraapprovazionegraduatoriaprogramma")
                    Case "EST"
                        hplLetteraApprovazioneGraduatoriaProgramma.Visible = True
                        hplLetteraApprovazioneGraduatoriaProgramma.NavigateUrl = Documento.VOL_LetteraApprovazioneGraduatoriaProgrammaEstero(intIdEnte, intIdBando, intIdProgramma, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal1.Text)
                        NuovaCronologia("LetteraApprovazioneGraduatoriaProgrammiEstero")
                    Case "GG"
                        hplLetteraApprovazioneGraduatoriaProgramma.Visible = True
                        hplLetteraApprovazioneGraduatoriaProgramma.NavigateUrl = Documento.VOL_LetteraApprovazioneGraduatoriaProgrammaGaranziaGiovani(intIdEnte, intIdBando, intIdProgramma, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal1.Text)
                        NuovaCronologia("LetteraApprovazioneGraduatoriaProgrammiGaranziaGiovani")
                    Case "SCD"
                        hplLetteraApprovazioneGraduatoriaProgramma.Visible = True
                        hplLetteraApprovazioneGraduatoriaProgramma.NavigateUrl = Documento.VOL_LetteraApprovazioneGraduatoriaProgrammaSCD(intIdEnte, intIdBando, intIdProgramma, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"), txtdatadal1.Text)
                        NuovaCronologia("LetteraApprovazioneGraduatoriaProgrammiSCD")
                End Select
            End If
            imgGenerafileProgamma.Enabled = False
            Documento.Dispose()
        End If
    End Sub
End Class
