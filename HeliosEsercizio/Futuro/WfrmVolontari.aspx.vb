Imports System.IO
Imports System.Data.SqlClient
Imports System.Drawing

Public Class WfrmVolontari
    Inherits System.Web.UI.Page
    Public idComuneNasc As String
    Public idComuneRes As String
    Public idComuneDom As String
    Public idoneoSiNo As Integer
    'variabile che varrà 1 se si tratterà di una modifica(ora impostata volutamente a 1 per testare la modifica) e a 2 se si tratta di inserimento
    Public Shared intModificaInserimento As Integer
    'id ente in modifica
    Public Shared intEnteAssociato As Integer
    Protected WithEvents lblErr As System.Web.UI.WebControls.Label
    Protected WithEvents lblConferma As System.Web.UI.WebControls.Label
    'id relativo alla persona che si vuole modificare
    Public Shared intPersonaleEnteAssociato As Integer
    Protected WithEvents txtComuneDomicilio As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProvinciaDomicilio As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtConseguitoPresso As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnnoMedia As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtScuolaMedia As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAltriTitoli As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCorsi As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEsperienze As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAltreConoscenze As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSceltaProgetto As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnnoLaurea As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCorsoLaurea As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboUfficioPostale As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ImgDomicilio As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgSelezionaComuneResidenza As System.Web.UI.WebControls.Image
    Protected WithEvents hplDownload As System.Web.UI.WebControls.HyperLink
    Protected WithEvents hplDownloadB As System.Web.UI.WebControls.HyperLink
    Protected WithEvents imgGeneraDoc As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblGenera As System.Web.UI.WebControls.Label
    Protected WithEvents lblGeneraB As System.Web.UI.WebControls.Label
    Protected WithEvents imgChiusurainiziale As System.Web.UI.WebControls.LinkButton

    Protected WithEvents lblChiusuraIniziale As System.Web.UI.WebControls.LinkButton
    Protected WithEvents imgChiusuraServizio As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblChiusuraServizio As System.Web.UI.WebControls.Label
    Protected WithEvents imgSostituisciVol As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblSostitusciVol As System.Web.UI.WebControls.Label
    Protected WithEvents imgRiepilogoDocumentiVol As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblRiepilogoDocumentiVol As System.Web.UI.WebControls.Label
    Protected WithEvents ImgGrad As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImgAssocia As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblAssocia As System.Web.UI.WebControls.Label
    Public Shared intPersonaleEnteAcquisito As Integer
    Dim dtrgenerico As SqlClient.SqlDataReader
    Protected WithEvents HDDAltreInfo As System.Web.UI.HtmlControls.HtmlInputHidden
    Public dtrLeggiDati As SqlClient.SqlDataReader
    Protected WithEvents lblGeneraDuplica As System.Web.UI.WebControls.Label
    Protected WithEvents hplDownloadDuplicato As System.Web.UI.WebControls.HyperLink
    Protected WithEvents imgGeneraDocDuplicato As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblGeneraRitorno As System.Web.UI.WebControls.Label
    Protected WithEvents hplDownloadritorno As System.Web.UI.WebControls.HyperLink
    Protected WithEvents imgGeneraDocritorno As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgLettEsclusione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents HplLettEsclusione As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblLettEsclusione As System.Web.UI.WebControls.Label
    Protected WithEvents imgLettSubentro As System.Web.UI.WebControls.ImageButton
    Protected WithEvents HplLettSubentro As System.Web.UI.WebControls.HyperLink
    Protected WithEvents HplLettSubentroCopia As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblLettSubentro As System.Web.UI.WebControls.Label
    Protected WithEvents imgLetteraChiusura As System.Web.UI.WebControls.ImageButton
    Protected WithEvents HplLetteraChiusura As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblLetteraChiusura As System.Web.UI.WebControls.Label
    Protected WithEvents lblGraduatoria As System.Web.UI.WebControls.Label
    Protected WithEvents imgSelezionaComuneNascita As System.Web.UI.WebControls.Image
    Protected WithEvents txtTitolo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtComuneNascita As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtComuneResidenza As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProvinciaResidenza As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProvinciaNascita As System.Web.UI.WebControls.TextBox
    Protected WithEvents dtgRuoliScecondari As System.Web.UI.WebControls.DataGrid
    Protected WithEvents imgRuoli As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgCancella As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgRilascia As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblOperazione As System.Web.UI.WebControls.Label
    Dim CONTRATTO_DA_CARICARE As String = "DA CARICARE"
    Dim CONTRATTO_RESPINTO As String = "RESPINTO"
    Dim CONTRATTO_APPROVATO As String = "APPROVATO"
    Dim CONTRATTO_CARICATO As String = "CARICATO"
    Dim CONTRATTO_NON_PREVISTO As String = "NON PREVISTO"
    Dim PROGETTO_GARANZIA_GIOVANI As String = "4"
    Dim dtsAnagraficaEnti As System.Data.DataSet
    Dim dtaAnagraficaEnti As SqlClient.SqlDataAdapter
    Dim StrSql As String
    Dim bandiera As Integer
    Dim BandieraDomicilio As Integer
    Dim dtrgenerico1 As SqlClient.SqlDataReader
    Dim idcomunediresidenza As String
    Dim idcomunediDomicilio As String
    Dim dtgenerico As DataTable
    Public Shared StatoCivileEsistente As Boolean = False



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'predisposizione della form per inserimento o modifica di un volontario
        'AUTORE: TESTA GUIDO    DATA: 13/10/2004

        Dim dtR As DataRow
        Dim dtT As New DataTable
        'variabile contatore
        Dim i As Integer
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        GestioneNote_RigheNascoste.Visible = False
        DivContratto.Visible = False
        lblErr.Text = ""
        If Request.QueryString("VengoDA") = "SchedaVolontario" Then
            CaricaFascicolo(Request.QueryString("IdVol"))
        End If
        If ClsUtility.ForzaFascicoloInformaticoVolontari(Session("Utente"), Session("conn")) = True Then
            imgRiepilogoDocumentiVol.Visible = True
        End If
        If ClsUtility.ForzaCaricamentoPaghe(Session("Utente"), Session("conn")) = False Then
            hplInfoPaghe.Visible = False
        End If
        AbilitaDisabilitaTxtLibretto()
        AbilitaVerificaStatoCodiceFiscale()
        If Session("Sistema") = "Futuro" Then
            AbilitaGestioneDocumentiVolontario(Session("Utente"), Request.QueryString("IdVol"))
        End If

        If Page.IsPostBack = False Then

            gridSoricoNote.Visible = False


            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                ImgGrad.Visible = True
            Else
                ImgGrad.Visible = False
            End If
            If (Request.QueryString("IdVol") <> String.Empty) Then
                TxtDataDomanda.Enabled = False
                DivContratto.Visible = True
            Else
                CaricaComboProvinciaNazione("NASCITA", ChkEsteroNascita.Checked)
                CaricaComboProvinciaNazione("DOMICILIO", ChkEsteroNascita.Checked)
                CaricaComboProvinciaNazione("RESIDENZA", ChkEsteroNascita.Checked)
                DivContratto.Visible = False
                TxtDataDomanda.Enabled = True
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            StrSql = "SELECT RegioniCompetenze.CodiceRegioneCompetenza AS Path FROM UtentiUNSC INNER JOIN " & _
                     "RegioniCompetenze ON UtentiUNSC.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                     "WHERE UtentiUNSC.UserName ='" & Session("Utente") & "'"
            dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
            If dtrgenerico.Read = True Then
                Session("Path") = dtrgenerico("Path")
                Session("Path") &= "/"
                dtrgenerico.Close()
                dtrgenerico = Nothing
            Else
                dtrgenerico.Close()
                dtrgenerico = Nothing
                Response.Redirect("LogOn.aspx")
            End If

            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                If Request.QueryString("IdVol") <> vbNullString Then
                    StrSql = "SELECT  statientità.idstatoentità, statientità.statoentità, " & _
                            " isnull(entità.codicevolontario,'')as codicevolontario,inservizio, " & _
                            " GraduatorieEntità.Sostituito,attivitàentità.IDAttivitàEntità,GraduatorieEntità.Stato as idoneo,GraduatorieEntità.ammesso as Selezionato, entità.DataInizioServizio " & _
                            " FROM  StatiEntità " & _
                            " INNER Join entità ON StatiEntità.IDStatoEntità = entità.IDStatoEntità " & _
                            " INNER Join GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità " & _
                            " LEFT Join attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                            " WHERE entità.identità=" & Request.QueryString("IdVol") & ""
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))

                    Session("IdVol") = Request.QueryString("IdVol")
                    lblidVolontario.Value = Request.QueryString("IdVol")

                    dtrgenerico.Read()

                    If IsDBNull(dtrgenerico("DataInizioServizio")) Then
                        Session("DataInizio") = ""
                    Else
                        Session("DataInizio") = dtrgenerico("DataInizioServizio")
                    End If


                    CheckboxIdoneo.Visible = False
                    If dtrgenerico("Selezionato") <> 1 Then
                        If dtrgenerico("idoneo") = 1 Then
                            CheckboxIdoneo.Checked = True
                            CheckboxIdoneo.Visible = True
                            Bloccato.Visible = False
                        Else
                            CheckboxIdoneo.Checked = False
                            CheckboxIdoneo.Visible = True
                            Bloccato.Visible = False
                        End If
                    Else
                        If dtrgenerico("idoneo") = 1 Then
                            CheckboxIdoneo.Checked = True
                            CheckboxIdoneo.Visible = True
                            CheckboxIdoneo.Enabled = False
                            Bloccato.Visible = True
                        Else
                            CheckboxIdoneo.Checked = False
                            CheckboxIdoneo.Visible = True
                            CheckboxIdoneo.Enabled = False
                            Bloccato.Visible = True
                        End If
                    End If
                    'End If
                    lblIDAttivitaEntita.Value = "" & dtrgenerico("IDAttivitàEntità")

                    If dtrgenerico("inservizio") = True Then

                        Session("StatoVolontario") = "In Servizio"
                        StrSql = "SELECT VociMenu.VoceMenu "
                        StrSql = StrSql & " FROM VociMenu INNER JOIN"
                        StrSql = StrSql & " AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu INNER JOIN"
                        StrSql = StrSql & " Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo "
                        '============================================================================================================================
                        '====================================================30/09/2008==============================================================
                        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
                        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
                        '============================================================================================================================
                        If Session("Read") <> "1" Then
                            StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
                        Else
                            StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
                        End If
                        StrSql = StrSql & " WHERE (VociMenu.VoceMenu IN ('Chiusura In Servizio', 'Chiusura Iniziale')) "
                        StrSql = StrSql & " and username = '" & Session("Utente") & "'"
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
                        'Verifica se Volontario Estero o nazionale
                        If dtrgenerico.HasRows Then
                            While dtrgenerico.Read()
                                If dtrgenerico("VoceMenu") = "Chiusura In Servizio" Then
                                    imgChiusuraServizio.Visible = True
                                End If
                                If dtrgenerico("VoceMenu") = "Chiusura Iniziale" Then
                                    imgChiusurainiziale.Visible = True
                                End If
                            End While
                        End If
                    ElseIf (dtrgenerico("statoentità") = "Chiuso Durante Servizio" Or dtrgenerico("statoentità") = "Rinunciatario") And dtrgenerico("sostituito") = 0 Then
                        Session("StatoVolontario") = dtrgenerico("statoentità")


                        '** Aggiunto da Simona Cordella il 03/05/2006
                        '**Controllo se il volontario non è più in servizio e se non è stato sostituito

                        'controllo se l'utenè è abilitato a vedere i menù
                        StrSql = "SELECT VociMenu.VoceMenu "
                        StrSql = StrSql & " FROM VociMenu INNER JOIN"
                        StrSql = StrSql & " AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu INNER JOIN"
                        StrSql = StrSql & " Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo "
                        '============================================================================================================================
                        '====================================================30/09/2008==============================================================
                        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
                        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
                        '============================================================================================================================
                        If Session("Read") <> "1" Then
                            StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
                        Else
                            StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
                        End If
                        StrSql = StrSql & " WHERE (VociMenu.VoceMenu IN ('Ricerca Chiusi In Servizio')) "
                        StrSql = StrSql & " and username = '" & Session("Utente") & "'"
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
                        'Verifica se Volontario Estero o nazionale
                        If dtrgenerico.HasRows Then
                            While dtrgenerico.Read()
                                If dtrgenerico("VoceMenu") = "Ricerca Chiusi In Servizio" Then
                                    imgSostituisciVol.Visible = True
                                End If
                            End While
                        End If

                    Else
                        Session("StatoVolontario") = dtrgenerico("statoentità")
                    End If
                Else
                End If
            End If
            If Request.QueryString("provieneda") = "ChiusuraServizio" Or Request.QueryString("provieneda") = "ChiusuraIniziale" Then
                lblidAttivita.Value = CInt(Request.QueryString("IdProgetto"))
            Else
                lblidAttivita.Value = Request.QueryString("IdAttivita")
            End If
            lblidattivitasedeassegnazione.Value = Request.QueryString("idattivitaSedeAssegnazione")
            lblidEntesede.Value = Request.QueryString("IdEnteSede")
            lblpresenta.Value = Request.QueryString("presenta")
            If Request.QueryString("Mess") <> "" Then
                lblErr.Text = Request.QueryString("Mess")
            End If
            'SESSO
            cboSesso.Items.Clear()
            cboSesso.Items.Add("Uomo")
            cboSesso.Items(0).Value = 0
            cboSesso.Items.Add("Donna")
            cboSesso.Items(1).Value = 1
            'STATO CIVILE
            CaricaComboStatoCivile()
            'cboStatoCivile.Items.Clear()
            'cboStatoCivile.Items.Add("Stato Libero")
            'cboStatoCivile.Items(0).Value = "Stato Libero"
            'cboStatoCivile.Items.Add("Coniugato/a")
            'cboStatoCivile.Items(1).Value = "Coniugato/a"
            'cboStatoCivile.Items.Add("Vedovo/a")
            'cboStatoCivile.Items(2).Value = "Vedovo/a"


            CaricaComboCategoria()
            CaricaComboConseguimento()
            CaricaComboNazionalita()
            'carico combo GMO o FAMI in base al progetto
            CaricoParticolaritàEntità(lblidAttivita.Value)
            'carico eventuale sede secondaria del volontario
            CaricaComboSedeSecondaria(lblidAttivita.Value)
            CaricaComboStatoVerificaCF()
            If Request.Params("IdVol") <> "" Then
                Call CaricaVolontario(Request.Params("IdVol"))
            Else 'Inserimento  
                'Rendo editabile solo il Cod.Fis.
                txtCodiceFiscale.ReadOnly = False
                txtCodiceFiscale.BackColor = Color.White
                If Request.QueryString("codfis") <> "" Then
                    txtCodiceFiscale.Text = Request.QueryString("codFis")
                End If
                cmdSalva.Visible = True
            End If
            txtCodLibPost.Value = txtLibretto.Text

            '*******************************************************************************
            'Modifica di Amilcare Paolella del 29 Marzo 2006
            'Disabilitazione dei dati fondamentali della maschera per permettere all'ente di apportare
            'le modifiche al volontario
            ' Dim lblGraduatoria As New Label
            If Session("TipoUtente") = "E" Then
                txtCognome.Enabled = False
                txtNome.Enabled = False
                cboSesso.Enabled = False
                cboStatoCivile.Enabled = False
                txtCodiceFiscale.Enabled = False
                txtDataNascita.Enabled = False
                'txtComuneNascita.Enabled = False
                ddlComuneNascita.Enabled = False
                chkDisp1.Enabled = False
                chkDisp2.Enabled = False

                Session("truetrue") = ""
            Else
                Session("truetrue") = "ok"
            End If
            '*******************************************************************************
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            CaricaSedeAttuazione()
            If cboStatoCivile.Text = "0" Then
                StatoCivileEsistente = False
            Else
                StatoCivileEsistente = True
            End If

        End If

    End Sub
    Private Sub CaricoParticolaritàEntità(ByVal IdAttività As Integer)

        Dim blnFAMI As Boolean
        Dim blnGMO As Boolean

        Dim strMacrotipo As String

        Dim strSQL As String

        Dim dr As SqlDataReader
        If Not dr Is Nothing Then
            dr.Close()
            dr = Nothing
        End If

        strSQL = "Select ISNULL(GiovaniMinoriOpportunità,0) AS GMO ,ISNULL(FAMI,0) AS FAMI from attività where idattività = " & IdAttività
        dr = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dr.HasRows Then
            dr.Read()
            blnGMO = dr("GMO")
            blnFAMI = dr("FAMI")
        End If
        If Not dr Is Nothing Then
            dr.Close()
            dr = Nothing
        End If
        If blnGMO = False And blnFAMI = False Then
            'combo disabilitate
            ddlGMO.Enabled = False
            ddlFAMI.Enabled = False
        Else
            If blnGMO = True Then
                ddlGMO.Enabled = True
                ddlFAMI.Enabled = False


                strMacrotipo = "GMO"
            End If
            If blnFAMI = True Then
                ddlGMO.Enabled = False
                ddlFAMI.Enabled = True

                strMacrotipo = "FAMI"
            End If

            strSQL = " Select '' as Descrizione, '' as Codice From ParticolaritàEntità UNION "
            strSQL &= " Select Descrizione, Codice From ParticolaritàEntità "
            strSQL &= " WHERE  Macrotipo = '" & strMacrotipo & "'"
            strSQL &= " ORDER BY Codice"
            dr = ClsServer.CreaDatareader(strSQL, Session("conn"))


            'carico la combo delle sedi di attuazione
            If strMacrotipo = "GMO" Then
                ddlGMO.DataSource = dr
                ddlGMO.DataTextField = "Descrizione"
                ddlGMO.DataValueField = "Codice"
                ddlGMO.DataBind()
            Else
                ddlFAMI.DataSource = dr
                ddlFAMI.DataTextField = "Descrizione"
                ddlFAMI.DataValueField = "Codice"
                ddlFAMI.DataBind()
            End If
            If Not dr Is Nothing Then
                dr.Close()
                dr = Nothing
            End If
        End If
    End Sub
    Function LetteraAssegnazioneDuplicato(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Determina Adeguamento Positivo 
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

            'parla da sola
            StampaDuplicatoLetteraAssegnazione()
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")
                'apro il template
                xLinea = Reader.ReadLine()
                While xLinea <> ""
                    xLinea = Replace(xLinea, "<Ente>", dtrLeggiDati("Ente"))
                    xLinea = Replace(xLinea, "<IndirizzoEnte>", dtrLeggiDati("indirizzoEnte"))
                    xLinea = Replace(xLinea, "<ComuneEnte>", dtrLeggiDati("comune"))
                    xLinea = Replace(xLinea, "<CivicoEnte>", dtrLeggiDati("CivicoEnte"))
                    xLinea = Replace(xLinea, "<CapEnte>", dtrLeggiDati("capEnte"))
                    xLinea = Replace(xLinea, "<CodiceEnte>", dtrLeggiDati("codiceEnte"))
                    xLinea = Replace(xLinea, "<Nominativo>", dtrLeggiDati("nominativo"))
                    xLinea = Replace(xLinea, "<CodiceVolontario>", dtrLeggiDati("codicevolontario"))
                    xLinea = Replace(xLinea, "<sedeProgetto>", dtrLeggiDati("SedeProgetto"))
                    xLinea = Replace(xLinea, "<CodProgetto>", dtrLeggiDati("CodiceProgetto"))
                    xLinea = Replace(xLinea, "_", " ")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While
                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing
            End If
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraAssegnazioneDuplicato = "documentazione/" & strNomeFile

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function
    Private Sub ControllaVol(ByVal codicefiscale As String)
        'Esiste Record in GraduatorieEntità per stesso Volontario e Stesso Bando?
        'StrSql = "SELECT attività.titolo + '(' + attività.codiceente + ')' + enti.denominazione as dato " & _
        StrSql = "SELECT attività.titolo + ' (' + attività.codiceente + ') presso ' + enti.denominazione + ' - ' + entisedi.denominazione " & _
        " + ' - ' + comuni.denominazione as dato " & _
        " FROM graduatorieentità " & _
        " inner join entità on entità.identità = graduatorieentità.identità " & _
        " inner join attivitàsediassegnazione on " & _
        " (attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieentità.idattivitàsedeassegnazione)" & _
        " inner join entisedi on  entisedi.identesede=attivitàsediassegnazione.identesede " & _
        " inner join comuni on entisedi.idcomune=comuni.idcomune " & _
        " inner join attività on (attività.idattività=attivitàsediassegnazione.idattività)" & _
        " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
        " INNER join enti on enti.idente=attività.identepresentante " & _
        " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
        " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
        " where entità.codicefiscale='" & codicefiscale & "' and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.Gruppo ELSE BANDORICORSI.Gruppo END =(Select CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.Gruppo ELSE BANDORICORSI.Gruppo END from Attività " & _
        " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
        " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
        " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
        " where Attività.idattività=" & lblidAttivita.Value & ")"
        '" where graduatorieentità.identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(19).Text & " and bando.idbando =(Select bando.idBando from Attività " & _
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(StrSql, Session("conn"))
    End Sub
    Function LetteraEsclusione(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Determina Adeguamento Positivo 
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

            'parla da sola
            StampaEsclusioneVolo()
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")
                'apro il template
                xLinea = Reader.ReadLine()


                Dim strEnte As String = dtrLeggiDati("Ente")
                Dim strIndirizzoEnte As String = dtrLeggiDati("indirizzoEnte")
                Dim strComuneEnte As String = dtrLeggiDati("comune")
                Dim strCivicoEnte As String = dtrLeggiDati("CivicoEnte")
                Dim strCapEnte As String = dtrLeggiDati("capEnte")
                Dim strCodiceEnte As String = dtrLeggiDati("codiceEnte")
                Dim strNominativo As String = dtrLeggiDati("nominativo")
                Dim strCodiceVolontario As String = dtrLeggiDati("codicevolontario")
                Dim strsedeProgetto As String = dtrLeggiDati("SedeProgetto")
                Dim strindirizzoVol As String = dtrLeggiDati("indirizzoVol")
                Dim strcivicoVol As String = dtrLeggiDati("civicoVol")
                Dim strcomuneVol As String = dtrLeggiDati("comuneVol")
                Dim strprovinciaVol As String = dtrLeggiDati("provinciaVol")
                Dim strcapVol As String = dtrLeggiDati("capVol")
                Dim strdatainizio As String = dtrLeggiDati("datainizio")
                Dim strcf As String = dtrLeggiDati("codicefiscale")
                Dim strdatafine As String = dtrLeggiDati("datafine")
                Dim strNvolo As String = dtrLeggiDati("Nvolo")
                Dim strGazzetta As String = dtrLeggiDati("Gazzetta")


                While xLinea <> ""
                    xLinea = Replace(xLinea, "<Ente>", strEnte)
                    xLinea = Replace(xLinea, "<IndirizzoEnte>", strIndirizzoEnte)
                    xLinea = Replace(xLinea, "<ComuneEnte>", strComuneEnte)
                    xLinea = Replace(xLinea, "<CivicoEnte>", strCivicoEnte)
                    xLinea = Replace(xLinea, "<CapEnte>", strCapEnte)
                    xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceEnte)
                    xLinea = Replace(xLinea, "<Nominativo>", strNominativo)
                    xLinea = Replace(xLinea, "<CodiceVolontario>", strCodiceVolontario)
                    xLinea = Replace(xLinea, "<sedeProgetto>", strsedeProgetto)
                    xLinea = Replace(xLinea, "<indirizzoVol>", strindirizzoVol)
                    xLinea = Replace(xLinea, "<civicoVol>", strcivicoVol)
                    xLinea = Replace(xLinea, "<comuneVol>", strcomuneVol)
                    xLinea = Replace(xLinea, "<provinciaVol>", strprovinciaVol)
                    xLinea = Replace(xLinea, "<capVol>", strcapVol)
                    xLinea = Replace(xLinea, "<datainizio>", strdatainizio)
                    xLinea = Replace(xLinea, "<datafine>", strdatafine)
                    xLinea = Replace(xLinea, "<Nvolo>", strNvolo)
                    xLinea = Replace(xLinea, "<Gazzetta>", strGazzetta)
                    'xLinea = Replace(xLinea, "_", " ")
                    'Writer.WriteLine(xLinea)

                    'xLinea = Reader.ReadLine()
                    Dim intX As Integer
                    intX = 0
                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        ControllaVol(strcf)
                        xLinea = Replace(xLinea, "<BreakPoint>", "")
                        If dtrLeggiDati.HasRows = True Then
                            While dtrLeggiDati.Read
                                intX = intX + 1
                                Writer.WriteLine(intX & ".           " & dtrLeggiDati("dato") & "\par")
                                Writer.WriteLine(xLinea)
                            End While
                        End If
                    End If
                    Writer.WriteLine(xLinea)
                    xLinea = Reader.ReadLine()

                End While
                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing
            End If
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraEsclusione = "documentazione/" & strNomeFile

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


    End Function
    Sub CaricaSedeAttuazione()
        If Not Request.QueryString("IdEnteSede") Is Nothing Then
            StrSql = "SELECT IDEnteSedeAttuazione FROM entisediattuazioni where  identesede = " + Request.QueryString("IdEnteSede")
        Else
            StrSql = "SELECT TMPIdSedeAttuazione as IDEnteSedeAttuazione FROM entità where  identità = " + Request.QueryString("IdVol")
        End If

        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        CboSedeAtt.DataSource = dtrgenerico
        CboSedeAtt.DataTextField = "IDEnteSedeAttuazione"
        CboSedeAtt.DataValueField = "IDEnteSedeAttuazione"
        CboSedeAtt.DataBind()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Function LetteraAssegnazioneVOlontario(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Determina Adeguamento Positivo 
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String
        Dim passworddecript As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'parla da sola
            StampaAssegnazioneVolo()
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                passworddecript = dtrLeggiDati("Password")
                If passworddecript <> "" Then passworddecript = ClsUtility.ReadPsw(passworddecript)
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '               "AS04\par}}")
                'apro il template
                xLinea = Reader.ReadLine()
                While xLinea <> ""
                    xLinea = Replace(xLinea, "<Nominativo>", dtrLeggiDati("Nominativo"))
                    xLinea = Replace(xLinea, "<ComuneNas>", dtrLeggiDati("comuneNas"))
                    If NomeFile = "AssegnazioneVolontariNazionali" Or NomeFile = "AssegnazioneVolontariEstero" Or NomeFile = "SostituzioneVolontariNazionali" Or NomeFile = "SostituzioneVolontariEsteri" Then  'AssegnazioneVolontariNazionali SI TRATTA DEL LETTERA ASSEGNAZIONE
                        'If txtComuneDomicilio.Text = "" And txtIndirizzoDomicilio.Text = "" Then
                        If ddlComuneDomicilio.SelectedItem.Text = "" And txtIndirizzoDomicilio.Text = "" Then
                            'DATI RESIDENZA
                            xLinea = Replace(xLinea, "<ProvinciaRes>", "(" & dtrLeggiDati("provinciaRes") & ")")
                            xLinea = Replace(xLinea, "<ComuneRes>", dtrLeggiDati("comuneRes"))
                            xLinea = Replace(xLinea, "<CapRes>", dtrLeggiDati("capRes"))
                            xLinea = Replace(xLinea, "<IndirizzoRes>", dtrLeggiDati("indirizzoRes"))
                            xLinea = Replace(xLinea, "<CivicoRes>", dtrLeggiDati("civicoRes"))
                        Else
                            'DATI DOMICILIO
                            'xLinea = Replace(xLinea, "<ProvinciaRes>", "(" & txtProvinciaDomicilio.Text & ")") 'PROVINCIA DOMICILIO
                            'xLinea = Replace(xLinea, "<ComuneRes>", txtComuneDomicilio.Text) 'COMUNE DOMICILIO
                            xLinea = Replace(xLinea, "<ProvinciaRes>", "(" & ddlProvinciaDomicilio.SelectedItem.Text & ")") 'PROVINCIA DOMICILIO
                            xLinea = Replace(xLinea, "<ComuneRes>", ddlComuneDomicilio.SelectedItem.Text) 'COMUNE DOMICILIO
                            xLinea = Replace(xLinea, "<CapRes>", txtCapDomicilio.Text) 'CAP DOMICILIO
                            xLinea = Replace(xLinea, "<IndirizzoRes>", txtIndirizzoDomicilio.Text) 'INDIRIZZO DOMICILIO
                            xLinea = Replace(xLinea, "<CivicoRes>", txtCivicoDomicilio.Text) 'CIVICO DOMICILIO
                        End If
                    Else
                        'RESIDENZA
                        xLinea = Replace(xLinea, "<ProvinciaRes>", "(" & dtrLeggiDati("provinciaRes") & ")")
                        xLinea = Replace(xLinea, "<ComuneRes>", dtrLeggiDati("comuneRes"))
                        xLinea = Replace(xLinea, "<CapRes>", dtrLeggiDati("capRes"))
                        xLinea = Replace(xLinea, "<IndirizzoRes>", dtrLeggiDati("indirizzoRes"))
                        xLinea = Replace(xLinea, "<CivicoRes>", dtrLeggiDati("civicoRes"))
                    End If


                    xLinea = Replace(xLinea, "<DataOdierna>", dtrLeggiDati("DataOdierna"))
                    xLinea = Replace(xLinea, "<CodFis>", dtrLeggiDati("CodFis"))
                    xLinea = Replace(xLinea, "<CodiceVolontario>", dtrLeggiDati("Codicevolontario"))
                    xLinea = Replace(xLinea, "<Titolo>", dtrLeggiDati("Titolo"))
                    xLinea = Replace(xLinea, "<Ente>", dtrLeggiDati("Ente"))
                    xLinea = Replace(xLinea, "<dataFineProgetto>", dtrLeggiDati("dataFineProgetto"))
                    xLinea = Replace(xLinea, "<dataNascita>", dtrLeggiDati("dataNascita"))
                    xLinea = Replace(xLinea, "<IndirizzoSede>", dtrLeggiDati("indirizzoSede"))
                    xLinea = Replace(xLinea, "<CivicoSede>", dtrLeggiDati("CivicoSede"))
                    xLinea = Replace(xLinea, "<CapSede>", dtrLeggiDati("CapSede"))
                    xLinea = Replace(xLinea, "<ProvinciaSede>", "(" & dtrLeggiDati("provinciaSa") & ")")
                    'xLinea = Replace(xLinea, "<DescAbbre>", dtrLeggiDati("DescAbbre"))
                    xLinea = Replace(xLinea, "<DescAbbreSa>", dtrLeggiDati("DescAbbreSa"))
                    xLinea = Replace(xLinea, "<ComuneSede>", dtrLeggiDati("comuneSa"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Abbre>", dtrLeggiDati("Abbre"))
                    xLinea = Replace(xLinea, "<Indi>", dtrLeggiDati("Indi"))
                    xLinea = Replace(xLinea, "<Civ>", dtrLeggiDati("Civ"))
                    xLinea = Replace(xLinea, "<Deno>", dtrLeggiDati("Deno"))
                    xLinea = Replace(xLinea, "<Denom>", dtrLeggiDati("Denom"))
                    xLinea = Replace(xLinea, "<NZ>", dtrLeggiDati("NZ"))
                    xLinea = Replace(xLinea, "<NZpa>", dtrLeggiDati("NZpa"))
                    xLinea = Replace(xLinea, "<Nvolo>", dtrLeggiDati("Nvolo"))
                    xLinea = Replace(xLinea, "<Gazzetta>", dtrLeggiDati("Gazzetta"))
                    xLinea = Replace(xLinea, "<CodiceProgetto>", dtrLeggiDati("CodiceProgetto"))
                    xLinea = Replace(xLinea, "<DataApprovazioneGraduatoria>", dtrLeggiDati("DataApprovazioneGraduatoria"))
                    xLinea = Replace(xLinea, "<DataInizioServizio>", dtrLeggiDati("DataInizioServizio"))
                    xLinea = Replace(xLinea, "<GiorniP>", Session("intGGP"))
                    xLinea = Replace(xLinea, "<GiorniR>", Session("intGGR"))
                    xLinea = Replace(xLinea, "_", " ") 'DataApprovazioneGraduatoria,DataInizioServizio
                    xLinea = Replace(xLinea, "<Username>", dtrLeggiDati("UserName"))
                    xLinea = Replace(xLinea, "<Password>", passworddecript)

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While
                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing
            End If
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraAssegnazioneVOlontario = "documentazione/" & strNomeFile

            'vado a fare la insert
            'Dim cmdinsert As Data.SqlClient.SqlCommand
            'strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento, Documento) "
            'strsql = strsql & "values "
            'strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "')"
            'cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            'cmdinsert.ExecuteNonQuery()

            'cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


    End Function
    Private Sub StampaDuplicatoLetteraAssegnazione()
        StrSql = "select isnull(replace(replace(replace(replace(replace(replace(replace(e.cognome,'°',''),'ì','i''')" & _
        " ,'é','e'''),  'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') + ' '  + " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(e.Nome,'°','')" & _
        " ,'ì','i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Nominativo, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(e.Codicevolontario,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Codicevolontario," & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(en.denominazione,'°','')," & _
        " 'ì','i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Ente," & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(en.codiceregione,'°','')," & _
        " 'ì','i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceEnte," & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(esatt.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as SedeProgetto," & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(esEnte.Indirizzo,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as IndirizzoEnte," & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(esEnte.Civico,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CivicoEnte," & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(esEnte.cap,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as capEnte," & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(csE.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')+ '(' + " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(psE.provincia,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')+ ')' as comune, isnull(a.codiceEnte,'') as Codiceprogetto " & _
        " from entità e " & _
        " inner join graduatorieEntità ge on ge.identità=e.idEntità " & _
        " inner join AttivitàSediAssegnazione asa on asa.idattivitàSedeassegnazione=ge.idattivitàSedeassegnazione " & _
        " inner join attività a on a.idattività=asa.idattività " & _
        " inner join entisedi es on es.identesede=asa.identesede " & _
        " inner join entisediattuazioni esatt on es.identesede=esatt.identesede " & _
        " inner join Enti en on  en.idente=a.identepresentante " & _
        " inner join entisedi esEnte on esEnte.idente=en.idente " & _
        " inner join entiseditipi est on est.identesede=esEnte.identesede " & _
        " inner join comuni csE on csE.idcomune=esEnte.idcomune " & _
        " inner join provincie psE on psE.idprovincia=csE.idprovincia " & _
        " where(e.identità = " & Request.Params("IdVol") & " And idtiposede = 1) "
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(StrSql, Session("conn"))
    End Sub
    Private Sub StampaEsclusioneVolo()
        StrSql = "select isnull(replace(replace(replace(replace(replace(replace(replace(e.cognome,'°',''),'ì','i''') ,'é','e'''),  " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') + ' '  +  " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(e.Nome,'°','') ,'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Nominativo, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(e.Indirizzo,'°','') ,'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as indirizzoVol, " & _
         " isnull(replace(replace(replace(replace(replace(replace(replace(e.codicefiscale,'°','') ,'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Codicefiscale, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(e.numerocivico,'°','') ,'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as civicoVol, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(cRes.denominazione,'°','') ,'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as ComuneVol, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(pRes.provincia,'°','') ,'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as ProvinciaVol," & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(e.cap,'°',''),'ì','i'''),'é','e'''),  " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CapVol,  " & _
        " convert(varchar,e.datainizioservizio,103) as datainizio, " & _
        " convert(varchar,e.datafineservizio,103) as datafine, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(e.Codicevolontario,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Codicevolontario, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(en.denominazione,'°',''), 'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Ente, isnull(replace(replace(replace(replace(replace(replace " & _
        " (replace(en.codiceregione,'°',''), 'ì','i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceEnte," & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(esatt.Denominazione,'°',''),'ì','i'''),'é','e'''),  " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as SedeProgetto, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(esEnte.Indirizzo,'°',''),'ì','i'''),'é','e'''),  " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as IndirizzoEnte, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(esEnte.Civico,'°',''),'ì','i'''),'é','e'''),  " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CivicoEnte, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(esEnte.cap,'°',''),'ì','i'''),'é','e'''),  " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as capEnte, " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(csE.Denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')+ '(' +  isnull(replace(replace(replace(replace(replace(replace " & _
        " (replace(psE.provincia,'°',''),'ì','i'''),'é','e'''),  'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')+ ')' as comune," & _
        " isnull(a.codiceEnte,'') as Codiceprogetto,  " & _
        " isnull(replace(replace(replace(replace(replace(replace(replace(Bando.GazzettaUfficiale,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Gazzetta, " & _
        " isnull(bando.Volontari,'')as NVolo" & _
        " from entità e  " & _
        " inner join comuni cRes on cRes.idcomune=e.idcomuneResidenza  " & _
        " inner join provincie pRes on pRes.idprovincia=cRes.idprovincia " & _
        " inner join graduatorieEntità ge on ge.identità=e.idEntità  " & _
        " inner join AttivitàSediAssegnazione asa on asa.idattivitàSedeassegnazione=ge.idattivitàSedeassegnazione  " & _
        " inner join attività a on a.idattività=asa.idattività  " & _
        " inner join Bandiattività on a.idbandoattività=bandiattività.idbandoattività" & _
        " inner join bando on bandiattività.idbando=bando.idbando" & _
        " inner join entisedi es on es.identesede=asa.identesede  " & _
        " inner join entisediattuazioni esatt on es.identesede=esatt.identesede  " & _
        " inner join Enti en on  en.idente=a.identepresentante  " & _
        " inner join entisedi esEnte on esEnte.idente=en.idente  " & _
        " inner join entiseditipi est on est.identesede=esEnte.identesede  " & _
        " inner join comuni csE on csE.idcomune=esEnte.idcomune  " & _
        " inner join provincie psE on psE.idprovincia=csE.idprovincia " & _
        " where e.identità=" & Request.Params("IdVol") & " and est.idtiposede=1  "
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(StrSql, Session("conn"))
    End Sub
    Private Sub StampaAssegnazioneVolo()
        StrSql = "select isnull(replace(replace(replace(replace(replace(replace(replace(e.cognome,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') + ' ' " & _
        " + isnull(replace(replace(replace(replace(replace(replace(replace(e.Nome,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Nominativo,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as indirizzoRes,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.numerocivico,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoRes,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.cap,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capRes,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.CodiceFiscale,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CodFis,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(cRes.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneRes,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.provincia,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaRes,"
        StrSql = StrSql & "isnull(case len(day(getdate())) when 1 then '0' + " & _
        " convert(varchar(20),day(getdate())) else convert(varchar(20),day(getdate())) " & _
        " end + '/' + (case len(month(getdate())) when 1 then '0' + convert(varchar(20),month(getdate())) " & _
        " else convert(varchar(20),month(getdate()))  end + '/' + Convert(varchar(20), Year(getdate()))),'') as DataOdierna,"
        StrSql = StrSql & "isnull(case len(day(asa.dataUltimoStato)) when 1 then '0' +  convert(varchar(20),day(asa.dataUltimoStato)) " & _
        " else convert(varchar(20),day(asa.dataUltimoStato))  end + '/' + (case len(month(asa.dataUltimoStato)) " & _
        " when 1 then '0' + convert(varchar(20),month(asa.dataUltimoStato))  " & _
        " else convert(varchar(20),month(asa.dataUltimoStato))  end + '/' + Convert(varchar(20), " & _
        " Year(asa.dataUltimoStato))),'') as DataApprovazioneGraduatoria,"
        StrSql = StrSql & " isnull(case len(day(e.dataInizioservizio)) when 1 then '0' +  convert(varchar(20),day(e.dataInizioservizio)) " & _
        " else convert(varchar(20),day(e.dataInizioservizio))  end + '/' + (case len(month(e.dataInizioservizio)) " & _
        " when 1 then '0' + convert(varchar(20),month(e.dataInizioservizio))  " & _
        " else convert(varchar(20),month(e.dataInizioservizio))  end + '/' + Convert(varchar(20), " & _
        " Year(e.dataInizioservizio))),'') as DataInizioServizio,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.Codicevolontario,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Codicevolontario,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.titolo,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Titolo,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Ente,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.CodiceRegione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as NZ,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(es.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as indirizzoSede,"
        StrSql = StrSql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.civico,'°',''),'ì','i''')" & _
        ",'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoSede, "
        StrSql = StrSql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.cap,'°',''),'ì', " & _
        " 'i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capSede,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.codiceEnte,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceProgetto,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(cSa.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as comuneSa,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.DescrAbb,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as DescAbbre, "
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.provincia,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaSa, "
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.DescrAbb,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as DescAbbreSa, "
        StrSql = StrSql & "  isnull(case len(day(a.datafineattività)) when 1 then '0' + " & _
        " Convert(varchar(20), Day(a.datafineattività)) " & _
        " else convert(varchar(20),day(a.datafineattività))  " & _
        " end + '/' + (case len(month(a.datafineattività))  when 1 then '0' " & _
        " + convert(varchar(20),month(a.datafineattività))   else convert(varchar(20)," & _
        " month(a.datafineattività))  end + '/' + Convert(varchar(20),  Year(a.datafineattività))),'') " & _
        " as DatafineProgetto, "
        StrSql = StrSql & "  isnull(case len(day(e.datanascita)) when 1 then '0' + " & _
        " Convert(varchar(20), Day(e.datanascita)) " & _
        " else convert(varchar(20),day(e.datanascita))  " & _
        " end + '/' + (case len(month(e.datanascita))  when 1 then '0' " & _
        " + convert(varchar(20),month(e.datanascita))   else convert(varchar(20)," & _
        " month(e.datanascita))  end + '/' + Convert(varchar(20),  Year(e.datanascita))),'') " & _
        " as DataNascita, "
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(cNas.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneNas, "
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.Indirizzo,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Indi, "
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni_1.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Deno, "
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(Provincie.DescrAbb,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Abbre, "
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.Civico,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Civ, "
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi_1.CAP,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CAP, "
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(enti_1.Denominazione,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Denom, "
        StrSql = StrSql & "isnull(enti_1.CodiceRegione,'')as NZpa,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(Bando.GazzettaUfficiale,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Gazzetta, "
        StrSql = StrSql & "isnull(bando.Volontari,'')as NVolo,"
        StrSql = StrSql & "isnull(e.Username,'')as Username, "
        StrSql = StrSql & "isnull(e.Password,'')as Password, "
        StrSql = StrSql & "isnull(e.IdSedePrimaAssegnazione,'')as PrimaAss"
        StrSql = StrSql & " from entità e"
        StrSql = StrSql & " inner join comuni cRes on cRes.idcomune=e.idcomuneResidenza"
        StrSql = StrSql & " inner join comuni cNas on cNas.idcomune=e.idcomunenascita"
        StrSql = StrSql & " inner join provincie pRes on pres.idprovincia=cRes.idprovincia"
        StrSql = StrSql & " inner join graduatorieEntità ge on ge.identità=e.idEntità"
        StrSql = StrSql & " inner join AttivitàSediAssegnazione asa on asa.idattivitàSedeassegnazione=ge.idattivitàSedeassegnazione"
        StrSql = StrSql & " inner join attività a on a.idattività=asa.idattività"
        StrSql = StrSql & " inner join Bandiattività on a.idbandoattività=bandiattività.idbandoattività"
        StrSql = StrSql & " inner join bando on bandiattività.idbando=bando.idbando"
        StrSql = StrSql & " inner join entisedi es on es.identesede=asa.identesede"
        StrSql = StrSql & " inner join comuni cSa on cSa.idcomune=es.idcomune"
        StrSql = StrSql & " inner join provincie pSa on pSa.idprovincia=cSa.idprovincia"
        StrSql = StrSql & " inner join Enti en on  en.idente=es.idente"
        StrSql = StrSql & " left Join entisediattuazioni ON e.IdSedePrimaAssegnazione = entisediattuazioni.IDEnteSedeAttuazione"
        StrSql = StrSql & " left JOIN entisedi entisedi_1 ON entisediattuazioni.IDEnteSede = entisedi_1.IDEnteSede"
        StrSql = StrSql & " left join enti enti_1 ON entisedi_1.idente = enti_1.idente"
        StrSql = StrSql & " left JOIN comuni comuni_1 ON entisedi_1.IDComune = comuni_1.IDComune"
        StrSql = StrSql & " left JOIN provincie ON comuni_1.IDProvincia = provincie.IDProvincia"
        StrSql = StrSql & " where e.identità=" & Request.Params("IdVol") & ""
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(StrSql, Session("conn"))
    End Sub
    Private Sub ControlliInserimentoCodiceFiscale()
        Dim strCodFis As String
        Dim strIdEntita As String
        'Aggiunto da Alessandra Taballione il 04/11/2004
        'Controllo Volontario Su Codice Fiscale in Inserimento
        'Verifica Univocità Codice Fiscale
        If Not IsNothing(Request.QueryString("IdVol")) Then
            StrSql = "Select Codicefiscale,identità from entità where codicefiscale='" & Replace(txtCodiceFiscale.Text, "'", "''") & "' and identità <> " & Request.QueryString("IdVol") & " "
        Else
            StrSql = "Select Codicefiscale,identità from entità where codicefiscale='" & Replace(txtCodiceFiscale.Text, "'", "''") & "'"
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        'Se Esiste
        If dtrgenerico.HasRows = True Then
            'Esiste Record in AttivitàEntità con stato <> rinuncia?
            dtrgenerico.Read()
            lblidVolontario.Value = dtrgenerico("identità")
            StrSql = "Select distinct Entità.identità,Entità.cognome,Entità.nome," & _
            " statiattività.cancellata from Entità " & _
            " inner join AttivitàEntità on (Entità.identità=AttivitàEntità.identità)" & _
            " inner join statiattività on (statiattività.idstatoattività=AttivitàEntità.idstatoattivitàentità)" & _
            " where entità.identità=" & dtrgenerico("identità") & " and statiattività.cancellata=1"
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                'se esiste
                'Notifica Anomalia Servizio già effettuato
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                lblErr.Text = "Il Codice Fiscale inserito risulta appartenere ad un Volontario che ha già prestato servizio."
                'DisabilitaControlli()
                'txtCodiceFiscale.ReadOnly = False
                'txtCodiceFiscale.BackColor = Color.White
                Response.Redirect("WfrmVolontari.aspx?idattivitaSedeAssegnazione=" & lblidattivitasedeassegnazione.Value & "&presenta=" & Request.QueryString("presenta") & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&codFis=" & txtCodiceFiscale.Text & "&Mess=" & lblErr.Text & "&Disabilita=OK")
            Else
                'se non esiste
                'Esiste Record in GraduatorieEntità per stesso Volontario e Stesso Bando?
                StrSql = "SELECT graduatorieentità.identità FROM graduatorieentità " & _
                " inner join attivitàsediassegnazione on " & _
                " (attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieentità.idattivitàsedeassegnazione)" & _
                " inner join attività on (attività.idattività=attivitàsediassegnazione.idattività)" & _
                " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
                " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
                " where graduatorieentità.identità=" & lblidVolontario.Value & " and bando.idbando =(Select bando.idBando from Attività " & _
                " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
                " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
                " where Attività.idattività=" & Request.QueryString("IdAttivita") & ")"
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
                If dtrgenerico.HasRows = True Then
                    'Se Esiste

                    'Notifica Anomalia Domanda già presentata su altro progetto stesso bando
                    lblErr.Text = "Il Codice Fiscale inserito risulta appartenere ad un Volontario che ha già presentato domanda su un'altro Progetto."
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'DisabilitaControlli()
                    'txtCodiceFiscale.ReadOnly = False
                    'txtCodiceFiscale.BackColor = Color.White
                    Response.Redirect("WfrmVolontari.aspx?idattivitaSedeAssegnazione=" & lblidattivitasedeassegnazione.Value & "&presenta=" & Request.QueryString("presenta") & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&codFis=" & txtCodiceFiscale.Text & "&Mess=" & lblErr.Text & "&Disabilita=OK")
                Else
                    'Se non Esiste
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'Abilitazione campi
                    Response.Redirect("WfrmVolontari.aspx?idattivitaSedeAssegnazione=" & lblidattivitasedeassegnazione.Value & "&presenta=" & Request.QueryString("presenta") & "&IdEnteSede=" & lblidEntesede.Value & "&IdVol=" & lblidVolontario.Value & "&IdAttivita=" & lblidAttivita.Value & "&Associa=True")
                    'CaricaVolontario(lblidVolontario.Value)
                End If
            End If
        Else 'Se non esiste
            'Abilito Caricamento Dati
            'strCodFis = txtCodiceFiscale.Text
            'AbiltaPulisciControlli()
            'txtCodiceFiscale.Text = strCodFis
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            If txtCognome.ReadOnly = True Then
                Response.Redirect("WfrmVolontari.aspx?idattivitaSedeAssegnazione=" & lblidattivitasedeassegnazione.Value & "&presenta=" & Request.QueryString("presenta") & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&codFis=" & txtCodiceFiscale.Text & "")
            End If

        End If
    End Sub
    Private Sub PulisciDati()
        txtAltreInformazioni.Text = ""
        txtCAP.Text = ""
        txtCapDomicilio.Text = ""
        txtCapDomN.Value = ""
        txtCellulare.Text = ""
        txtCivico.Text = ""
        txtCivicoDomicilio.Text = ""
        txtCivicoDomN.Value = ""
        txtCodiceFiscale.Text = ""
        txtCodicevolontario.Text = ""
        txtCognome.Text = ""
        ddlComuneDomicilio.SelectedItem.Text = ""
        txtComuneDomN.Value = ""
        ddlComuneNascita.SelectedItem.Text = ""
        ddlComuneResidenza.SelectedItem.Text = ""
        txtDataNascita.Text = ""
        txtEmail.Text = ""
        txtFax.Text = ""
        txtIndirizzo.Text = ""
        txtDettaglioRecapitoResidenza.Text = ""
        txtIndirizzoDomicilio.Text = ""
        TxtDettaglioRecapitoDomicilio.Text = ""
        txtIndirizzoDomN.Value = ""
        txtLibretto.Text = ""
        txtNome.Text = ""
        ddlProvinciaDomicilio.SelectedItem.Text = ""
        TxtProvinciaDomN.Value = ""
        ddlProvinciaNascita.SelectedItem.Text = ""
        ddlProvinciaResidenza.SelectedItem.Text = ""
        txtTelefono.Text = ""
        txtTelefonoDomicilio.Text = ""
        txtTelefonoDomN.Value = ""
        CboSedeAtt.SelectedIndex = 0
        CboSedeAttSecondaria.SelectedIndex = 0
        txtPunteggio.Text = ""
        cboCategoria.SelectedIndex = 0
        CboConseguimentoTS.SelectedIndex = 0

        If ddlFAMI.Enabled = True Then
            ddlFAMI.SelectedItem.Text = ""
        End If
        If ddlGMO.Enabled = True Then
            ddlGMO.SelectedItem.Text = ""
        End If
    End Sub
    Private Function MAXID(ByVal myCommand As SqlCommand) As Integer
        'ritorna il MAxID della tabella Entità
        Dim strDtReader As String
        Dim dtrLocal As SqlDataReader
        Dim iMaxId As Integer
        strDtReader = "select @@identity as IDMAx"
        'controllo e chiudo se aperto il datareader
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        'eseguo la query
        myCommand.CommandText = strDtReader
        dtrLocal = myCommand.ExecuteReader
        'leggo il datareader
        dtrLocal.Read()
        'se ci sono dei record
        If dtrLocal.HasRows = True Then
            iMaxId = CInt(dtrLocal("IDMAx"))
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If
        End If
        'controllo e chiudo se aperto il datareader
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        Return iMaxId

    End Function
    Private Function CaricaUffici() As DataSet
        'carico gli uffici postale presi dalla tabella "ufficipostali"
        Dim StrSql As String
        Dim myDateset As New DataSet
        StrSql = "Select IDUfficioPostale,Descrizione From ufficipostali order by Descrizione"
        myDateset = ClsServer.DataSetGenerico(StrSql, Session("conn"))
        Return myDateset
    End Function
    Private Sub CaricaVolontario(ByVal idVolontario As Integer)
        Dim StrSql As String
        Dim myDateset As DataSet
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        StrSql = "SELECT entità.CodiceFascicolo, entità.IDFascicolo, entità.DescrFascicolo, entità.banca, " & _
                      "entità.cc, " & _
                      "entità.abi, " & _
                      "entità.cab, " & _
                      "entità.cin, " & _
                      "entità.IDUfficioPostale, " & _
                      "entità.CodiceLibrettoPostale, " & _
                      "entità.DisponibileStessoProg, " & _
                      "entità.DisponibileAltriProg, " & _
                      "entità.IDEntità, " & _
                      "entità.Cognome, " & _
                      "entità.Nome, " & _
                      "entità.Sesso, " & _
                      "entità.Indirizzo, " & _
                      "entità.DettaglioRecapitoResidenza, " & _
                      "entità.CAP, " & _
                      "entità.NumeroCivico, " & _
                      "entità.Telefono, " & _
                      "entità.Cellulare, " & _
                      "entità.Email, " & _
                      "entità.Fax, " & _
                      "entità.DataNascita, " & _
                      "entità.CodiceFiscale, " & _
                      "entità.StatoCivile, " & _
                      "entità.IndirizzoDomicilio, " & _
                      "entità.DettaglioRecapitoDomicilio, " & _
                      "entità.NumeroCivicoDomicilio, " & _
                      "entità.CAPDomicilio, " & _
                      "entità.EmailDomicilio, " & _
                      "entità.TelefonoDomicilio, " & _
                      "entità.TitoloStudio, " & _
                      "entità.IstitutoStudio, " & _
                      "entità.NumAnnoScuolaMedia, " & _
                      "entità.IstitutoMediaSuperiore, " & _
                      "entità.NumAnnoAccademico, " & _
                      "entità.TitoloLaurea, " & _
                      "entità.AltriTitoli, " & _
                      "entità.Corsi, " & _
                      "entità.Esperienze, " & _
                      "entità.AltreConoscenze, " & _
                      "entità.MotiviSceltaProgetto, " & _
                      "entità.AltreInformazioni, " & _
                      "entità.DisponibileStessoProg, " & _
                      "entità.DisponibileAltriProg, " & _
                      "entità.DataInizioServizio as Inizio, " & _
                      "entità.DataFineServizio as Fine, " & _
                      "entità.RitornoMittente, " & _
                      "entità.Username, " & _
                      "entità.IBAN, " & _
                      "entità.BIC_SWIFT, " & _
                      "entità.DataDomanda, " & _
                      "entità.IdNazionalita, " & _
                      "entità.IdTipoStatoCivile, " & _
                      "entità.CodiceFiscaleConiuge, " & _
                      "entità.GMO, " & _
                      "entità.FAMI, " & _
                      "isnull(entità.StatoContrattoVolontario,0) as StatoContrattoVolontario, " & _
                      "dbo.FN_VERIFICA_ABILITAZIONE_BANDO_CONTRATTO(" & idVolontario & ") AS AbilitaBandoContratto, " & _
                      "graduatorieentità.Punteggio, " & _
                      "Causali.Descrizione, " & _
                      "comuni_3.IDComune AS IdComuneNasc, " & _
                      "comuni_3.Denominazione AS DescComuneNAsc, " & _
                      "provincie_3.IDProvincia AS IdProvNasc, " & _
                      "provincie_3.Provincia AS DescProvNasc, " & _
                      "comuni_1.IDComune AS IdComuneResidenza, " & _
                      "comuni_1.Denominazione AS DescComuneResidenza, " & _
                      "provincie_1.IDProvincia AS IdProvResidenza, " & _
                      "provincie_1.Provincia AS DescProvResidenza, " & _
                      "comuni_2.IDComune AS IdComuneDomicilio, " & _
                      "comuni_2.Denominazione AS DescComuneDomicilio, " & _
                      "provincie_2.IDProvincia AS IdProvDomicilio, " & _
                      "provincie_2.Provincia AS DescProvDomicilio, " & _
                      "isnull(entità.codicevolontario,'')as codiceVolontario, " & _
                      " ISNULL(StatiAssicurativi.StatoAssicurativo,'NON DEFINITO') as statoassicurativo , " & _
                      "isnull(entità.tmpidsedeattuazione,'') as sedeattuazione, " & _
                      "isnull(entità.TMPIdSedeAttuazioneSecondaria,0) as sedeattuazionesecondario, " & _
                      "StatiEntità.StatoEntità, IsNull(GraduatorieEntità.Ammesso,0) As Ammesso, IsNull(GraduatorieEntità.Stato,0) As Stato, isnull(entità.IdCategoriaEntità,1) as IdCategoriaEntità, " & _
                      "isnull(entità.IdTitoloStudioConseguimento,1) as IdTitoloStudioConseguimento, " & _
                      "isnull(entità.AnomaliaCF,0) as AnomaliaCF, " & _
                      "entità.IdStatiVerificaCFEntità, " & _
                      "provincie_3.provincenazionali as ProvNazionaliNascita,provincie_1.provincenazionali as ProvNazionaliResidenza,provincie_2.provincenazionali as ProvNazionaliDomicilio  " & _
                      "FROM provincie provincie_2 " & _
                      "INNER JOIN  comuni comuni_2 ON provincie_2.IDProvincia = comuni_2.IDProvincia " & _
                      "RIGHT OUTER JOIN provincie provincie_1 " & _
                      "INNER JOIN comuni comuni_1 ON provincie_1.IDProvincia = comuni_1.IDProvincia " & _
                      "INNER JOIN entità " & _
                      "LEFT JOIN GraduatorieEntità ON entità.IdEntità = GraduatorieEntità.IdEntità " & _
                      "LEFT JOIN Causali ON entità.IDCausaleChiusura = Causali.IDCausale " & _
                      "INNER JOIN StatiEntità ON StatiEntità.IdStatoEntità = Entità.IdStatoEntità " & _
                      "INNER JOIN comuni comuni_3 ON entità.IDComuneNascita = comuni_3.IDComune " & _
                      "INNER JOIN provincie provincie_3 ON comuni_3.IDProvincia = provincie_3.IDProvincia " & _
                      "ON comuni_1.IDComune = entità.IDComuneResidenza " & _
                      "ON comuni_2.IDComune = entità.IDComuneDomicilio " & _
                      "LEFT JOIN StatiAssicurativi ON StatiAssicurativi.IdStatoAssicurativo= entità.idstatoassicurativo " & _
                      "LEFT JOIN CategorieEntità ON entità.IdCategoriaEntità = CategorieEntità.IdCategoriaEntità " & _
                      "LEFT JOIN TitoliStudioConseguimento ON entità.IdTitoloStudioConseguimento = TitoliStudioConseguimento.IdTitoloStudioConseguimento " & _
                      "WHERE entità.IDEntità = " & idVolontario
        '           '"(SELECT StatoAssicurativo From StatiAssicurativi where IdStatoAssicurativo= entità.idstatoassicurativo)as statoassicurativo, " & _
        myDateset = ClsServer.DataSetGenerico(StrSql, Session("conn"))
        If myDateset.Tables(0).Rows.Count > 0 Then
            If myDateset.Tables(0).Rows(0).Item("StatoEntità") = "Rinunciatario" Or myDateset.Tables(0).Rows(0).Item("StatoEntità") = "Chiuso Durante Servizio" Then
                lblStatoVol.Text = myDateset.Tables(0).Rows(0).Item("StatoEntità") & " - " & myDateset.Tables(0).Rows(0).Item("Descrizione")
            Else
                lblStatoVol.Text = myDateset.Tables(0).Rows(0).Item("StatoEntità") & " - " & myDateset.Tables(0).Rows(0).Item("Descrizione")
            End If
            If IsDBNull(myDateset.Tables(0).Rows(0).Item("Inizio")) = False Then
                If CStr(myDateset.Tables(0).Rows(0).Item("Inizio")) <> "" Then
                    lblInizio.Visible = True
                    LabelInizioServizio.Visible = True
                    lblFine.Visible = True
                    LabelDataFineServizio.Visible = True
                    IdDivDateServizio.Visible = True
                    DivDateServizio.Visible = True

                    If ControlloModificaDataInizioServizio() = True Then
                        imgModificaData.Visible = True
                        txtDataInizioVolontarioSubentrato.Visible = True

                        txtDataInizioVolontarioSubentrato.Text = myDateset.Tables(0).Rows(0).Item("Inizio")
                    Else
                        lblInizio.Text = myDateset.Tables(0).Rows(0).Item("Inizio")
                    End If

                    lblFine.Text = myDateset.Tables(0).Rows(0).Item("Fine")
                End If
            End If
            If IsDBNull(myDateset.Tables(0).Rows(0).Item("statoassicurativo")) = False Then
                If CStr(myDateset.Tables(0).Rows(0).Item("statoassicurativo")) <> "" Then
                    lblAss.Visible = True
                    lblTitAss.Visible = True
                    lblAss.Text = CStr(myDateset.Tables(0).Rows(0).Item("statoassicurativo"))

                End If
            End If
            Try
                TxtCodiceFascicolo.Text = myDateset.Tables(0).Rows(0).Item("CodiceFascicolo")
                HdControlloIdFascicolo.Value = TxtCodiceFascicolo.Text
                TxtIdFascicolo.Value = myDateset.Tables(0).Rows(0).Item("IDFascicolo")
                txtDescFasc.Text = myDateset.Tables(0).Rows(0).Item("DescrFascicolo")
            Catch es As Exception

            End Try



            Dim SelComune As New clsSelezionaComune

            txtCognome.Text = "" & myDateset.Tables(0).Rows(0).Item("Cognome")
            txtNome.Text = "" & myDateset.Tables(0).Rows(0).Item("Nome")
            cboSesso.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("Sesso")
            cboCategoria.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("IdCategoriaEntità")
            CboConseguimentoTS.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("IdTitoloStudioConseguimento")
            '*** agg.il 12/01/2015 da simona cordella ***
            If Not IsDBNull(myDateset.Tables(0).Rows(0).Item("IdNazionalita")) Then
                ddlNazionalita.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("IdNazionalita")
            End If
            TxtDataDomanda.Text = "" & myDateset.Tables(0).Rows(0).Item("DataDomanda")
            '***
            'Aggiunto da Alessandra Taballione il 10/06/2005
            txtCodicevolontario.Text = "" & myDateset.Tables(0).Rows(0).Item("codiceVolontario")
            cboStatoCivile.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("IdTipoStatoCivile")
            txtCodiceFiscale.Text = "" & myDateset.Tables(0).Rows(0).Item("CodiceFiscale")
            txtCodiceFiscaleConiuge.Text = "" & myDateset.Tables(0).Rows(0).Item("CodiceFiscaleConiuge")
            txtEmail.Text = "" & myDateset.Tables(0).Rows(0).Item("Email")
            txtTelefono.Text = "" & myDateset.Tables(0).Rows(0).Item("Telefono")
            txtCellulare.Text = "" & myDateset.Tables(0).Rows(0).Item("Cellulare")
            txtFax.Text = "" & myDateset.Tables(0).Rows(0).Item("Fax")
            txtDataNascita.Text = "" & myDateset.Tables(0).Rows(0).Item("DataNascita")
            If "" & myDateset.Tables(0).Rows(0).Item("ProvNazionaliNascita") = True Then
                ChkEsteroNascita.Checked = False
            Else
                ChkEsteroNascita.Checked = True
            End If
            CaricaComboProvinciaNazione("NASCITA", ChkEsteroNascita.Checked)

            ddlProvinciaNascita.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("IdProvNasc")
            If ddlProvinciaNascita.SelectedItem.Text <> "" Then
                ddlComuneNascita = SelComune.CaricaComuniNascita(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
                'ddlComuneNascita.SelectedItem.Text = "" & myDateset.Tables(0).Rows(0).Item("DescComuneNAsc")
                idComuneNasc = "" & myDateset.Tables(0).Rows(0).Item("IdComuneNasc")
                ddlComuneNascita.SelectedValue = idComuneNasc
                lblidComuneNascita.Value = "" & myDateset.Tables(0).Rows(0).Item("IdComuneNasc")
                ddlComuneNascita.Enabled = True
            End If

            'ottaviani 11-02-2021 - prima if
            If "" & myDateset.Tables(0).Rows(0).Item("ProvNazionaliResidenza") <> "" Then
                If "" & myDateset.Tables(0).Rows(0).Item("ProvNazionaliResidenza") = True Then
                    ChkEsteroResidenza.Checked = False
                Else
                    ChkEsteroResidenza.Checked = True
                End If
            End If

            CaricaComboProvinciaNazione("RESIDENZA", ChkEsteroResidenza.Checked)
            ddlProvinciaResidenza.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("IdProvResidenza")
            If ddlProvinciaResidenza.SelectedItem.Text <> "" Then
                ddlComuneResidenza = SelComune.CaricaComuni(ddlComuneResidenza, ddlProvinciaResidenza.SelectedValue, Session("Conn"))
                'ddlComuneResidenza.SelectedItem.Text = "" & myDateset.Tables(0).Rows(0).Item("DescComuneResidenza")
                idComuneRes = "" & myDateset.Tables(0).Rows(0).Item("IdComuneResidenza")
                ddlComuneResidenza.SelectedValue = idComuneRes
                lblidComuneResidenza.Value = "" & myDateset.Tables(0).Rows(0).Item("IdComuneResidenza")
                Session("IdComune") = "" & myDateset.Tables(0).Rows(0).Item("IdComuneResidenza")
                ddlComuneResidenza.Enabled = True
            End If


            txtIndirizzo.Text = "" & myDateset.Tables(0).Rows(0).Item("Indirizzo")
            txtCivico.Text = "" & myDateset.Tables(0).Rows(0).Item("NumeroCivico")
            txtCAP.Text = "" & myDateset.Tables(0).Rows(0).Item("CAP")
            txtDettaglioRecapitoResidenza.Text = "" & myDateset.Tables(0).Rows(0).Item("DettaglioRecapitoResidenza")

            txtUtenzaWeb.Text = "" & myDateset.Tables(0).Rows(0).Item("Username")

            If Not IsDBNull(myDateset.Tables(0).Rows(0).Item("ProvNazionaliDomicilio")) Then
                If "" & myDateset.Tables(0).Rows(0).Item("ProvNazionaliDomicilio") = True Then
                    ChkEsteroDomicilio.Checked = False
                Else
                    ChkEsteroDomicilio.Checked = True
                End If
            Else
                ChkEsteroDomicilio.Checked = False
            End If


            CaricaComboProvinciaNazione("DOMICILIO", ChkEsteroDomicilio.Checked)

            If "" & myDateset.Tables(0).Rows(0).Item("IdComuneDomicilio") <> "" Then
                ddlProvinciaDomicilio.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("IdProvDomicilio")
                ddlComuneDomicilio = SelComune.CaricaComuni(ddlComuneDomicilio, ddlProvinciaDomicilio.SelectedValue, Session("Conn"))
                idComuneDom = "" & myDateset.Tables(0).Rows(0).Item("IdComuneDomicilio")
                ddlComuneDomicilio.SelectedValue = idComuneDom
                Session("IdComuneDom") = "" & myDateset.Tables(0).Rows(0).Item("IdComuneDomicilio")
                lblidComuneDomicilio.Value = "" & myDateset.Tables(0).Rows(0).Item("IdComuneDomicilio")
                ddlProvinciaDomicilio.SelectedItem.Text = "" & myDateset.Tables(0).Rows(0).Item("DescProvDomicilio")

            End If
            If ddlProvinciaResidenza.SelectedItem.Text <> "" Then
                ddlComuneDomicilio.Enabled = True
            End If
            txtIndirizzoDomicilio.Text = "" & myDateset.Tables(0).Rows(0).Item("IndirizzoDomicilio")
            txtCivicoDomicilio.Text = "" & myDateset.Tables(0).Rows(0).Item("NumeroCivicoDomicilio")
            txtCapDomicilio.Text = "" & myDateset.Tables(0).Rows(0).Item("CAPDomicilio")
            TxtDettaglioRecapitoDomicilio.Text = "" & myDateset.Tables(0).Rows(0).Item("DettaglioRecapitoDomicilio")
            txtTelefonoDomicilio.Text = "" & myDateset.Tables(0).Rows(0).Item("TelefonoDomicilio")
            txtTitoloStudio.Text = "" & myDateset.Tables(0).Rows(0).Item("TitoloStudio")
            txtAltreInformazioni.Text = "" & myDateset.Tables(0).Rows(0).Item("AltreInformazioni")
            txtLibretto.Text = "" & myDateset.Tables(0).Rows(0).Item("CodiceLibrettoPostale")
            txtIBAN.Text = "" & myDateset.Tables(0).Rows(0).Item("IBAN")
            txtBicSwift.Text = "" & myDateset.Tables(0).Rows(0).Item("BIC_SWIFT")



            txtPunteggio.Text = myDateset.Tables(0).Rows(0).Item("Punteggio")
            'If ddlGMO.Enabled = True Then
            ddlGMO.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("GMO")

            'End If
            'If ddlFAMI.Enabled = True Then
            ddlFAMI.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("FAMI")
            'End If
            CboSedeAttSecondaria.SelectedValue = "" & myDateset.Tables(0).Rows(0).Item("sedeattuazionesecondario")

            If myDateset.Tables(0).Rows(0).Item("RitornoMittente") = 0 Then
                chkIndirizzoErrato.Checked = False
            Else
                chkIndirizzoErrato.Checked = True
            End If

            Session("idsedeattuazione") = "" & myDateset.Tables(0).Rows(0).Item("sedeattuazione")
            If myDateset.Tables(0).Rows(0).Item("DisponibileStessoProg") = 0 Then
                chkDisp1.Checked = False
            Else
                chkDisp1.Checked = True
            End If
            If myDateset.Tables(0).Rows(0).Item("DisponibileAltriProg") = 0 Then
                chkDisp2.Checked = False
            Else
                chkDisp2.Checked = True
            End If

            If myDateset.Tables(0).Rows(0).Item("AnomaliaCF") = True Then
                imgAnomaliaCF.Visible = True
                If Not IsDBNull(myDateset.Tables(0).Rows(0).Item("IdStatiVerificaCFEntità")) Then
                    cboStatoVerificaCF.SelectedValue = myDateset.Tables(0).Rows(0).Item("IdStatiVerificaCFEntità")
                Else
                    cboStatoVerificaCF.SelectedIndex = 1
                End If
            Else
                imgAnomaliaCF.Visible = False
                cboStatoVerificaCF.SelectedIndex = 0
            End If

            'aggiunto da simona cordella il 14/09/2012
            VisualizzaContratto(myDateset.Tables(0).Rows(0).Item("StatoContrattoVolontario"), myDateset.Tables(0).Rows(0).Item("AbilitaBandoContratto"))
            'Controllo la possibilità di inserire una persona in un determinato progetto
            If (Mid(Session("Utente"), 1, 1) = "U" Or Mid(Session("Utente"), 1, 1) = "R") Then
                If myDateset.Tables(0).Rows(0).Item("Ammesso") = 0 And myDateset.Tables(0).Rows(0).Item("Stato") = 1 Then
                    ImgAssocia.Visible = True
                End If
            End If
        Else
            lblErr.Text = "Errore nella ricerca.Riprovare"
        End If
    End Sub
    'funzione che controlla/se l'utente è abilitato a vedere la
    'txt della data inizio "FORZAINIZIOVOLONTARI"
    'la funzione controlla anche se il volontario è un subentrato
    'la data inizio del volotnario dev'essere diversa da quella di inizio attività
    'la data inizio del volontario dev'essere maggiore del getdate()
    'true faccio vedere la txt
    'false nascondo la txt
    Function ControlloModificaDataInizioServizio() As Boolean
        Dim strsql As String
        Dim strDataInizioVolontario As String
        Dim strDataInizioProgetto As String

        ControlloModificaDataInizioServizio = True

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        strsql = "SELECT "
        strsql = strsql & "isnull(case len(day(DataInizioServizio)) when 1 then '0' + convert(varchar(20),day(DataInizioServizio)) "
        strsql = strsql & "else convert(varchar(20),day(DataInizioServizio))  end + '/' + "
        strsql = strsql & "(case len(month(DataInizioServizio)) when 1 then '0' + convert(varchar(20),month(DataInizioServizio)) "
        strsql = strsql & "else convert(varchar(20),month(DataInizioServizio))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(DataInizioServizio))),'') as DataInizioVol "
        strsql = strsql & "FROM entità "
        strsql = strsql & "WHERE IdEntità='" & Request.QueryString("IdVol") & "'"

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()

        strDataInizioVolontario = dtrgenerico("DataInizioVol")

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        strsql = "SELECT "
        strsql = strsql & "isnull(case len(day(DataInizioAttività)) when 1 then '0' + convert(varchar(20),day(DataInizioAttività)) "
        strsql = strsql & "else convert(varchar(20),day(DataInizioAttività))  end + '/' + "
        strsql = strsql & "(case len(month(DataInizioAttività)) when 1 then '0' + convert(varchar(20),month(DataInizioAttività)) "
        strsql = strsql & "else convert(varchar(20),month(DataInizioAttività))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(DataInizioAttività))),'') as DataInizioProg "
        strsql = strsql & "FROM attività "
        strsql = strsql & "WHERE IdAttività=" & CInt(lblidAttivita.Value)

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()

        strDataInizioProgetto = dtrgenerico("DataInizioProg")

        'se la data inizio volontario è uguale a quella del progetto
        'non devo far vedere la txt della data inizio del volontario
        'SANDOKAN TEMPORANEO
        If strDataInizioProgetto <> "" Then

            If CDate(strDataInizioProgetto) = CDate(strDataInizioVolontario) Then
                ControlloModificaDataInizioServizio = False

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

                Return ControlloModificaDataInizioServizio
                'Exit Function
            End If

        End If
        'controllo se la data inizio del volontario è maggiore di quella attuale
        If CDate(strDataInizioVolontario) <= CDate(Session("dataserver")) Then
            ControlloModificaDataInizioServizio = False

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            Return ControlloModificaDataInizioServizio
            'Exit Function
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Verifica se l'uetnte puo modificare il libretto postale
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
        If Session("Read") <> "1" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Inizio Volontari'" & _
                 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = False Then
            ControlloModificaDataInizioServizio = False

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            Return ControlloModificaDataInizioServizio
            'Exit Function
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        Return ControlloModificaDataInizioServizio

    End Function
    Private Sub DisabilitaControlli()
        'disabilitazione controlli
        txtCognome.ReadOnly = True
        txtCognome.BackColor = Color.Gainsboro
        txtNome.ReadOnly = True
        txtNome.BackColor = Color.Gainsboro
        cboSesso.Enabled = False
        cboSesso.BackColor = Color.Gainsboro
        cboSesso.Font.Bold = True
        cboStatoCivile.Enabled = False
        cboStatoCivile.BackColor = Color.Gainsboro
        cboStatoCivile.Font.Bold = True
        txtCodiceFiscale.ReadOnly = True
        txtCodiceFiscale.BackColor = Color.Gainsboro
        txtEmail.ReadOnly = True
        txtEmail.BackColor = Color.Gainsboro
        txtTelefono.ReadOnly = True
        txtTelefono.BackColor = Color.Gainsboro
        txtCellulare.ReadOnly = True
        txtCellulare.BackColor = Color.Gainsboro
        txtFax.ReadOnly = True
        txtFax.BackColor = Color.Gainsboro
        txtDataNascita.ReadOnly = True
        txtDataNascita.BackColor = Color.Gainsboro
        'txtComuneNascita.ReadOnly = True
        'txtComuneNascita.BackColor = Color.Gainsboro
        'txtComuneResidenza.ReadOnly = True
        'txtComuneResidenza.BackColor = Color.Gainsboro
        ddlComuneResidenza.Enabled = False
        ddlComuneNascita.BackColor = Color.Gainsboro
        ddlComuneResidenza.Enabled = False
        ddlComuneResidenza.BackColor = Color.Gainsboro

        txtIndirizzo.ReadOnly = True
        txtIndirizzo.BackColor = Color.Gainsboro
        txtDettaglioRecapitoResidenza.ReadOnly = True
        txtDettaglioRecapitoResidenza.BackColor = Color.Gainsboro
        TxtDettaglioRecapitoDomicilio.ReadOnly = True
        TxtDettaglioRecapitoDomicilio.BackColor = Color.Gainsboro
        txtCivico.ReadOnly = True
        txtCivico.BackColor = Color.Gainsboro
        txtCAP.ReadOnly = True
        txtCAP.BackColor = Color.Gainsboro
        'txtComuneDomicilio.ReadOnly = True
        'txtComuneDomicilio.BackColor = Color.Gainsboro
        ddlComuneDomicilio.Enabled = False
        ddlComuneDomicilio.BackColor = Color.Gainsboro
        txtIndirizzoDomicilio.ReadOnly = True
        txtIndirizzoDomicilio.BackColor = Color.Gainsboro
        txtCivicoDomicilio.ReadOnly = True
        txtCivicoDomicilio.BackColor = Color.Gainsboro
        txtCapDomicilio.ReadOnly = True
        txtCapDomicilio.BackColor = Color.Gainsboro
        'txtEmailDomicilio.ReadOnly = True
        'txtEmailDomicilio.BackColor = Color.Gainsboro
        txtTelefonoDomicilio.ReadOnly = True
        txtTelefonoDomicilio.BackColor = Color.Gainsboro
        txtTitoloStudio.ReadOnly = True
        txtTitoloStudio.BackColor = Color.Gainsboro
        'txtConseguitoPresso.ReadOnly = True
        'txtConseguitoPresso.BackColor = Color.Gainsboro
        'txtAnnoMedia.ReadOnly = True
        'txtAnnoMedia.BackColor = Color.Gainsboro
        'txtScuolaMedia.ReadOnly = True
        'txtScuolaMedia.BackColor = Color.Gainsboro
        'txtAnnoLaurea.ReadOnly = True
        'txtAnnoLaurea.BackColor = Color.Gainsboro
        'txtCorsoLaurea.ReadOnly = True
        'txtCorsoLaurea.BackColor = Color.Gainsboro
        'txtAltriTitoli.ReadOnly = True
        'txtAltriTitoli.BackColor = Color.Gainsboro
        'txtCorsi.ReadOnly = True
        'txtCorsi.BackColor = Color.Gainsboro
        'txtEsperienze.ReadOnly = True
        'txtEsperienze.BackColor = Color.Gainsboro
        'txtAltreConoscenze.ReadOnly = True
        'txtAltreConoscenze.BackColor = Color.Gainsboro
        'txtSceltaProgetto.ReadOnly = True
        'txtSceltaProgetto.BackColor = Color.Gainsboro
        txtAltreInformazioni.ReadOnly = True
        txtAltreInformazioni.BackColor = Color.Gainsboro
        'txtUfficioPostale.ReadOnly = True
        'txtUfficioPostale.BackColor = Color.Gainsboro
        txtLibretto.ReadOnly = True
        txtLibretto.BackColor = Color.Gainsboro
        chkDisp1.Enabled = False
        chkDisp1.BackColor = Color.Gainsboro
        chkDisp2.Enabled = False
        chkDisp1.BackColor = Color.Gainsboro
        'Aggiunto da Alessandra Taballione il 29/10/2004
        'txtBanca.ReadOnly = True
        'txtBanca.BackColor = Color.Gainsboro
        'txtabi.ReadOnly = True
        'txtabi.BackColor = Color.Gainsboro
        'txtCAB.ReadOnly = True
        'txtCAB.BackColor = Color.Gainsboro
        'txtCCBancario.ReadOnly = True
        'txtCCBancario.BackColor = Color.Gainsboro
        'txtCIN.ReadOnly = True
        'txtCIN.BackColor = Color.Gainsboro
        'txtProvinciaDomicilio.ReadOnly = True
        'txtProvinciaDomicilio.BackColor = Color.Gainsboro
        'txtProvinciaNascita.ReadOnly = True
        'txtProvinciaNascita.BackColor = Color.Gainsboro
        'txtProvinciaResidenza.ReadOnly = True
        'txtProvinciaResidenza.BackColor = Color.Gainsboro
        ddlProvinciaDomicilio.Enabled = False
        ddlProvinciaDomicilio.BackColor = Color.Gainsboro
        ddlProvinciaNascita.Enabled = False
        ddlProvinciaNascita.BackColor = Color.Gainsboro
        ddlProvinciaResidenza.Enabled = False
        ddlProvinciaResidenza.BackColor = Color.Gainsboro


        'cmdnuovo.Visible = True
        'Aggiunto da Alessandra Taballione il 30.12.2004
        ' ImgDomicilio.Visible = False
        cboCategoria.Enabled = False
        cboCategoria.BackColor = Color.Gainsboro
        CboConseguimentoTS.Enabled = False
        CboConseguimentoTS.BackColor = Color.Gainsboro
    End Sub
    Private Sub AbiltaPulisciControlli()
        'disabilitazione controlli
        txtCognome.ReadOnly = False
        txtCognome.BackColor = Color.White
        txtCognome.Text = ""
        txtNome.ReadOnly = False
        txtNome.BackColor = Color.White
        txtNome.Text = ""
        cboSesso.Enabled = True
        cboSesso.BackColor = Color.White
        cboSesso.Font.Bold = False
        cboStatoCivile.Enabled = True
        cboStatoCivile.BackColor = Color.White
        cboStatoCivile.Font.Bold = False
        txtCodiceFiscale.ReadOnly = False
        txtCodiceFiscale.BackColor = Color.White
        txtCodiceFiscale.Text = ""
        txtEmail.ReadOnly = False
        txtEmail.BackColor = Color.White
        txtEmail.Text = ""
        txtTelefono.ReadOnly = False
        txtTelefono.BackColor = Color.White
        txtTelefono.Text = ""
        txtCellulare.ReadOnly = False
        txtCellulare.BackColor = Color.White
        txtCellulare.Text = ""
        txtFax.ReadOnly = False
        txtFax.BackColor = Color.White
        txtFax.Text = ""
        txtDataNascita.ReadOnly = False
        txtDataNascita.BackColor = Color.White
        txtDataNascita.Text = ""
        'txtComuneNascita.ReadOnly = False
        'txtComuneNascita.BackColor = Color.White
        'txtComuneNascita.Text = ""
        'txtComuneResidenza.ReadOnly = False
        'txtComuneResidenza.BackColor = Color.White
        'txtComuneResidenza.Text = ""
        ddlComuneNascita.Enabled = True
        ddlComuneNascita.BackColor = Color.White
        ddlComuneNascita.SelectedItem.Text = ""
        ddlComuneResidenza.Enabled = True
        ddlComuneResidenza.BackColor = Color.White
        ddlComuneResidenza.SelectedItem.Text = ""

        txtIndirizzo.ReadOnly = False
        txtIndirizzo.BackColor = Color.White
        txtIndirizzo.Text = ""
        txtDettaglioRecapitoResidenza.ReadOnly = False
        txtDettaglioRecapitoResidenza.BackColor = Color.White
        txtDettaglioRecapitoResidenza.Text = ""
        txtCivico.ReadOnly = False
        txtCivico.BackColor = Color.White
        txtCivico.Text = ""
        txtCAP.ReadOnly = False
        txtCAP.BackColor = Color.White
        txtCAP.Text = ""
        'txtComuneDomicilio.ReadOnly = False
        'txtComuneDomicilio.BackColor = Color.White
        'txtComuneDomicilio.Text = ""
        ddlComuneDomicilio.Enabled = True
        ddlComuneDomicilio.BackColor = Color.White
        ddlComuneDomicilio.SelectedItem.Text = ""
        txtIndirizzoDomicilio.ReadOnly = False
        txtIndirizzoDomicilio.BackColor = Color.White
        txtIndirizzoDomicilio.Text = ""
        TxtDettaglioRecapitoDomicilio.ReadOnly = False
        TxtDettaglioRecapitoDomicilio.BackColor = Color.White
        TxtDettaglioRecapitoDomicilio.Text = ""
        txtCivicoDomicilio.ReadOnly = False
        txtCivicoDomicilio.BackColor = Color.White
        txtCivicoDomicilio.Text = ""
        txtCapDomicilio.ReadOnly = False
        txtCapDomicilio.BackColor = Color.White
        txtCapDomicilio.Text = ""
        'txtEmailDomicilio.ReadOnly = False
        'txtEmailDomicilio.BackColor = Color.White
        'txtEmailDomicilio.Text = ""
        txtTelefonoDomicilio.ReadOnly = False
        txtTelefonoDomicilio.BackColor = Color.White
        txtTelefonoDomicilio.Text = ""
        txtTitoloStudio.ReadOnly = False
        txtTitoloStudio.BackColor = Color.White
        txtTitoloStudio.Text = ""
        'txtConseguitoPresso.ReadOnly = False
        'txtConseguitoPresso.BackColor = Color.White
        'txtConseguitoPresso.Text = ""
        'txtAnnoMedia.ReadOnly = False
        'txtAnnoMedia.BackColor = Color.White
        'txtAnnoMedia.Text = ""
        'txtScuolaMedia.ReadOnly = False
        'txtScuolaMedia.BackColor = Color.White
        'txtScuolaMedia.Text = ""
        'txtAnnoLaurea.ReadOnly = False
        'txtAnnoLaurea.BackColor = Color.White
        'txtAnnoLaurea.Text = ""
        'txtCorsoLaurea.ReadOnly = False
        'txtCorsoLaurea.BackColor = Color.White
        'txtCorsoLaurea.Text = ""
        'txtAltriTitoli.ReadOnly = False
        'txtAltriTitoli.BackColor = Color.White
        'txtAltriTitoli.Text = ""
        'txtCorsi.ReadOnly = False
        'txtCorsi.BackColor = Color.White
        'txtCorsi.Text = ""
        'txtEsperienze.ReadOnly = False
        'txtEsperienze.BackColor = Color.White
        'txtEsperienze.Text = ""
        'txtAltreConoscenze.ReadOnly = False
        'txtAltreConoscenze.BackColor = Color.White
        'txtAltreConoscenze.Text = ""
        'txtSceltaProgetto.ReadOnly = False
        'txtSceltaProgetto.BackColor = Color.White
        'txtSceltaProgetto.Text = ""
        txtAltreInformazioni.ReadOnly = False
        txtAltreInformazioni.BackColor = Color.White
        txtAltreInformazioni.Text = ""
        'txtUfficioPostale.ReadOnly = False
        'txtUfficioPostale.BackColor = Color.Gainsboro
        'txtUfficioPostale.Text = ""
        txtLibretto.ReadOnly = False
        txtLibretto.BackColor = Color.White
        txtLibretto.Text = ""
        chkDisp1.Enabled = True
        chkDisp1.BackColor = Color.Transparent
        chkDisp1.Checked = False
        chkDisp2.Enabled = True
        chkDisp2.BackColor = Color.Transparent
        chkDisp2.Checked = False
        'Aggiunto da Alessandra Taballione il 29/10/2004
        'txtBanca.ReadOnly = False
        'txtBanca.BackColor = Color.White
        'txtBanca.Text = ""
        'txtabi.ReadOnly = False
        'txtabi.BackColor = Color.White
        'txtabi.Text = ""
        'txtCAB.ReadOnly = False
        'txtCAB.BackColor = Color.White
        'txtCAB.Text = ""
        'txtCCBancario.ReadOnly = False
        'txtCCBancario.BackColor = Color.White
        'txtCCBancario.Text = ""
        'txtCIN.ReadOnly = False
        'txtCIN.BackColor = Color.White
        'txtCIN.Text = ""
        'txtUfficioPostale.ReadOnly = False
        'txtUfficioPostale.BackColor = Color.White
        'txtUfficioPostale.Text = ""
        'txtProvinciaNascita.ReadOnly = False
        'txtProvinciaNascita.BackColor = Color.White
        'txtProvinciaNascita.Text = ""
        'txtProvinciaResidenza.ReadOnly = False
        'txtProvinciaResidenza.BackColor = Color.White
        'txtProvinciaResidenza.Text = ""
        'txtProvinciaDomicilio.ReadOnly = False
        'txtProvinciaDomicilio.BackColor = Color.White
        'txtProvinciaDomicilio.Text = ""

        ddlProvinciaNascita.Enabled = True
        ddlProvinciaNascita.BackColor = Color.White
        ddlProvinciaNascita.SelectedItem.Text = ""
        ddlProvinciaResidenza.Enabled = True
        ddlProvinciaResidenza.BackColor = Color.White
        ddlProvinciaResidenza.SelectedItem.Text = ""
        ddlProvinciaDomicilio.Enabled = True
        ddlProvinciaDomicilio.BackColor = Color.White
        ddlProvinciaDomicilio.Text = ""

        lblErr.Text = ""
        cmdSalva.Visible = True
        'Aggiunto da Alessandra Taballione 30.12.2004
        'ImgDomicilio.Visible = True
        cboCategoria.BackColor = Color.White
        cboCategoria.Font.Bold = False
        CboConseguimentoTS.BackColor = Color.White
        CboConseguimentoTS.Font.Bold = False
    End Sub
    Private Sub txtCodiceFiscale_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodiceFiscale.TextChanged
        'If txtCodiceFiscale.Text <> "" Then
        '    ControlliInserimentoCodiceFiscale()
        '    txtCodiceFiscale.Text = UCase(txtCodiceFiscale.Text)
        'End If
    End Sub
    Private Sub GestisciDomicilio()
        If lblComuneDomicilio.Visible = False Then
            lblComuneDomicilio.Visible = True
            'txtComuneDomicilio.Visible = True
            ddlComuneDomicilio.Visible = True
            lblCapDom.Visible = True
            txtCapDomicilio.Visible = True
            lblIndirizzoDomicilo.Visible = True
            txtIndirizzoDomicilio.Visible = True
            TxtDettaglioRecapitoDomicilio.Visible = True
            lblDettaglioRecapitoDomicilio.Visible = True
            lblCivicoDomicilio.Visible = True
            txtCivicoDomicilio.Visible = True
            lblTelefonoDomicilio.Visible = True
            txtTelefonoDomicilio.Visible = True
            lblProvinciaDomicilio.Visible = True
            'txtProvinciaDomicilio.Visible = True
            ddlProvinciaDomicilio.Visible = True
            'imgSelezionaComuneResidenza.Visible = True
            dettrecapitodom.Visible = True
            imgCapDom.Visible = True
            infocivico.Visible = True
            txtvisibile.Value = "true"
        Else
            'PulisciDomicilio()
            lblComuneDomicilio.Visible = False
            'txtComuneDomicilio.Visible = False
            ddlComuneDomicilio.Visible = False
            lblCapDom.Visible = False
            txtCapDomicilio.Visible = False
            lblIndirizzoDomicilo.Visible = False
            txtIndirizzoDomicilio.Visible = False
            TxtDettaglioRecapitoDomicilio.Visible = False
            lblDettaglioRecapitoDomicilio.Visible = False
            lblCivicoDomicilio.Visible = False
            txtCivicoDomicilio.Visible = False
            lblTelefonoDomicilio.Visible = False
            txtTelefonoDomicilio.Visible = False
            lblProvinciaDomicilio.Visible = False
            'txtProvinciaDomicilio.Visible = False
            ddlProvinciaDomicilio.Visible = False
            'imgSelezionaComuneResidenza.Visible = False
            dettrecapitodom.Visible = False
            infocivico.Visible = False
            imgCapDom.Visible = False
            txtvisibile.Value = "false"
            'TxtProvinciaDomN.Value = txtProvinciaDomicilio.Text
            TxtProvinciaDomN.Value = ddlProvinciaDomicilio.SelectedItem.Text
            txtIndirizzoDomN.Value = txtIndirizzoDomicilio.Text
            txtCapDomN.Value = txtCapDomicilio.Text
            txtCivicoDomN.Value = txtCivicoDomicilio.Text
            txtTelefonoDomN.Value = txtTelefonoDomicilio.Text
            'txtComuneDomN.Value = txtComuneDomicilio.Text
            txtComuneDomN.Value = ddlComuneDomicilio.SelectedItem.Text
        End If
    End Sub
    Private Sub PulisciDomicilio()
        ddlComuneDomicilio.SelectedItem.Text = ""
        lblidComuneDomicilio.Value = ""
        ddlProvinciaDomicilio.SelectedItem.Text = ""
        txtIndirizzoDomicilio.Text = ""
        TxtDettaglioRecapitoDomicilio.Text = ""
        txtCivicoDomicilio.Text = ""
        txtCapDomicilio.Text = ""
        txtTelefonoDomicilio.Text = ""
    End Sub
    Private Sub ScriviCronogliaEntitaDettagli()
        Dim MyCommand As SqlClient.SqlCommand
        Dim strsql As String

        Try
            MyCommand = New SqlClient.SqlCommand
            MyCommand.Connection = Session("conn")
            '"Banca, " & _
            '"CC, " & _
            '"ABI, " & _
            '"CAB, " & _
            '"CIN, " & _
            '"Entità.Banca, " & _
            '"Entità.CC, " & _
            '"Entità.ABI, " & _
            '"Entità.CAB, " & _
            '"Entità.CIN, " & _


            strsql = "INSERT INTO CronologiaEntitàDettagli " & _
                    "(IdEntità, " & _
                    "Indirizzo, " & _
                    "CAP, " & _
                    "NumeroCivico, " & _
                    "IdComuneResidenza, " & _
                    "IdUfficioPostale, " & _
                    "CodiceLibrettoPostale, " & _
                    "IdComuneDomicilio, " & _
                    "IndirizzoDomicilio, " & _
                    "NumeroCivicoDomicilio, " & _
                    "CAPDomicilio, " & _
                    "DataInizioValidità, " & _
                    "DataFineValidità, " & _
                    "UserNameUltimaModifica, " & _
                    "DataUltimaModifica, " & _
                    "IBAN, " & _
                    "BIC_SWIFT, " & _
                    " GMO, FAMI, TMPIdSedeAttuazioneSecondaria ) " & _
                    "SELECT top 1 " & _
                    "Entità.IdEntità, " & _
                    "Entità.Indirizzo, " & _
                    "Entità.CAP, " & _
                    "Entità.NumeroCivico, " & _
                    "Entità.IdComuneResidenza, " & _
                    "Entità.IdUfficioPostale, " & _
                    "Entità.CodiceLibrettoPostale, " & _
                    "Entità.IdComuneDomicilio, " & _
                    "Entità.IndirizzoDomicilio, " & _
                    "Entità.NumeroCivicoDomicilio, " & _
                    "Entità.CAPDomicilio, " & _
                    "CASE WHEN CronologiaEntitàDettagli.DataFineValidità IS NULL THEN " & _
                    "Entità.DataUltimoStato " & _
                    "ELSE " & _
                    "CronologiaEntitàDettagli.DataFineValidità " & _
                    "END DataInizioValidità, " & _
                    "getdate(), '" & Session("Utente") & _
                    "',getdate() " & _
                    ",Entità.IBAN " & _
                    ",Entità.BIC_SWIFT " & _
                    ",Entità.GMO, Entità.FAMI, Entità.TMPIdSedeAttuazioneSecondaria " & _
                    "FROM Entità " & _
                    "LEFT JOIN CronologiaEntitàDettagli on Entità.IdEntità = CronologiaEntitàDettagli.IdEntità " & _
                     "WHERE Entità.IdEntità = " & Request.QueryString("IdVol") & " " & _
                    "order by CronologiaEntitàDettagli.datafinevalidità desc "

            MyCommand.CommandText = strsql
            MyCommand.ExecuteNonQuery()

        Catch es As Exception

        End Try

        MyCommand.Dispose()

    End Sub
    Private Sub AbilitaDisabilitaTxtLibretto()

        'Verifica se l'uetnte puo modificare il libretto postale
        StrSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu" & _
                 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"

        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If Session("Read") <> "1" Then
            StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        StrSql = StrSql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Libretto Postale'" & _
                 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            txtLibretto.Enabled = True
            txtLibretto.BackColor = Color.White
            'agg da simona cordella 30/09/209
            txtIBAN.Enabled = True
            txtBicSwift.Enabled = True
            txtIBAN.BackColor = Color.White
            txtBicSwift.BackColor = Color.White
        Else
            txtLibretto.Enabled = False
            txtLibretto.BackColor = Color.Gainsboro
            'agg da simona cordella 30/09/209
            txtIBAN.Enabled = False
            txtBicSwift.Enabled = False
            txtIBAN.BackColor = Color.Gainsboro
            txtBicSwift.BackColor = Color.Gainsboro
        End If
        dtrgenerico.Close()
        dtrgenerico = Nothing

    End Sub
    Private Sub ImgGrad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ImgGrad.Click
        'va alla graduatoria del volontario in base all'ID 
        'AUTORE: ANTONELLO DI CROCE    DATA: 12/09/2005
        'Danilo prima    Response.Redirect("WfrmVolontari.aspx?idattivitaSedeAssegnazione=" & lblidattivitasedeassegnazione.Value & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&presenta=" & Request.QueryString("presenta") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&Disabilita=OK")
        If Request.QueryString("IdVol") <> "" Then
            StrSql = " SELECT AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione, AttivitàSediAssegnazione.IDEnteSede"
            StrSql = StrSql & " FROM entità INNER JOIN"
            StrSql = StrSql & " GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità INNER JOIN"
            StrSql = StrSql & " AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione"
            StrSql = StrSql & " WHERE entità.IDEntità = " & Request.QueryString("IdVol")

            dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                Dim idattivitaSedeAssegnazione As String = dtrgenerico.Item("IDAttivitàSedeAssegnazione")
                Dim IdEnteSede As String = dtrgenerico.Item("IDEnteSede")
                dtrgenerico.Close()
                dtrgenerico = Nothing
                Response.Redirect("WfrmAssociaVolontari.aspx?idattivitaSedeAssegnazione=" & idattivitaSedeAssegnazione & "&IdEnteSede=" & IdEnteSede & "&presenta=" & 3 & "&IdAttivita=" & Request.QueryString("IdAttivita"))
            End If

        Else
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            If Request.QueryString("IdEnteSede") <> "" Then
                Response.Redirect("WfrmAssociaVolontari.aspx?IdEnteSede=" & Request.QueryString("IdEnteSede") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&presenta=" & Request.QueryString("presenta") & "&idattivitaSedeAssegnazione=" & Request.QueryString("idattivitaSedeAssegnazione") & "")
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Private Sub ImgAssocia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ImgAssocia.Click
        'Richiama la maschera per l'associazione del volontario ad una graduatoria
        Response.Redirect("WFrmVolontarioProgetto.aspx?Id=" & Request.Params("IdVol"))
    End Sub
    'Funzione creata da Amilcare Paolella il 1/12/2005
    'Funzione per controllare se il codice libretto inserito esiste: "" = non esiste il codice nel DB. Se esiste restituisce il nominativo
    Private Function CheckExistLibretto(ByVal strNumLib As String) As String
        Dim strNominativo As String = ""
        If Trim(strNumLib) = "" Then
            Return strNominativo
            Exit Function
        End If
        If StrComp(txtCodLibPost.Value, strNumLib) <> 0 Then
            StrSql = "Select Cognome + ' ' + Nome As Nominativo From Entità Where CodiceLibrettoPostale='" & strNumLib.Replace("'", "''") & "'"
            dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
            If dtrgenerico.HasRows = False Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            Else
                dtrgenerico.Read()
                strNominativo = dtrgenerico.Item("Nominativo")
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
        Return strNominativo
    End Function
    Private Sub imgChiusurainiziale_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgChiusurainiziale.Click
        Response.Redirect("dettaglivolontario.aspx?provieneda=ChiusuraIniziale&IdAttivita=" & CInt(lblidAttivita.Value) & "&IdVolontario=" & CInt(lblidVolontario.Value) & "&IdProgetto=" & CInt(lblidAttivita.Value) & "&Op=rinuncia")
    End Sub
    Private Sub imgChiusuraServizio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgChiusuraServizio.Click
        Response.Redirect("dettaglivolontario.aspx?provieneda=ChiusuraServizio&IdAttivita=" & CInt(lblidAttivita.Value) & "&IdVolontario=" & CInt(lblidVolontario.Value) & "&IdProgetto=" & CInt(lblidAttivita.Value) & "&Op=esclusione")
    End Sub
    Private Sub imgSostituisciVol_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgSostituisciVol.Click
        If lblStatoVol.Text Like "Chiuso Durante Servizio*" Then
            Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?CodiceFiscale=" & txtCodiceFiscale.Text & "&provieneda=Volontari&IdAttivita=" & CInt(lblidAttivita.Value) & "&IdEntita=" & CInt(lblidVolontario.Value) & "&VecchioIdAttivitaEntita=" & lblIDAttivitaEntita.Value & "&Op=esclusione")
        Else
            Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?CodiceFiscale=" & txtCodiceFiscale.Text & "&provieneda=Volontari&IdAttivita=" & CInt(lblidAttivita.Value) & "&IdEntita=" & CInt(lblidVolontario.Value) & "&VecchioIdAttivitaEntita=" & lblIDAttivitaEntita.Value & "&Op=rinuncia")
        End If
    End Sub
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
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(b.DettaglioRecapitoResidenza,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as DettaglioRecapitoResidenza, "
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni.denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Comune, "
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(provincie.provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Provincia, "
        strsql = strsql & "(SELECT isnull((SELECT COUNT(*) "
        strsql = strsql & "FROM EntiSediAttuazioni "
        strsql = strsql & "INNER JOIN StatiEntiSedi  "
        strsql = strsql & "ON EntiSediAttuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede "
        strsql = strsql & "INNER JOIN EntiSedi ON EntiSedi.IdEnteSede = EntiSediAttuazioni.IdEnteSede "
        strsql = strsql & "WHERE StatiEntiSedi.Attiva = 1 AND IdEnte = " & Session("IdEnte") & ") "
        strsql = strsql & "+ "
        strsql = strsql & "(SELECT COUNT(*) FROM EntiSediAttuazioni "
        strsql = strsql & "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede "
        strsql = strsql & "INNER JOIN AssociaEntiRelazioniSediAttuazioni "
        strsql = strsql & "ON AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione = EntiSediAttuazioni.IdEnteSedeAttuazione "
        strsql = strsql & "INNER JOIN EntiRelazioni ON EntiRelazioni.IdEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione "
        strsql = strsql & "INNER JOIN StatiEntiSedi ON EntiSediAttuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede "
        strsql = strsql & "WHERE StatiEntiSedi.Attiva = 1 AND EntiRelazioni.IdEntePadre = " & Session("IdEnte") & " ),0)) as sediacc  "
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
    Private Sub RecuperoInfoVecchioVolontario()
        StrSql = "select isnull(replace(replace(replace(replace(replace(replace(replace(e.cognome,'°',''),'ì','i'''),'é','e'''), " & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') + ' ' " & _
        " + isnull(replace(replace(replace(replace(replace(replace(replace(e.Nome,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as Nominativo,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as indirizzoRes,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.numerocivico,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoRes,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.cap,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capRes,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.CodiceFiscale,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as CodFis,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(cRes.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as comuneRes,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(pRes.provincia,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaRes,"
        StrSql = StrSql & "isnull(case len(day(getdate())) when 1 then '0' + " & _
        " convert(varchar(20),day(getdate())) else convert(varchar(20),day(getdate())) " & _
        " end + '/' + (case len(month(getdate())) when 1 then '0' + convert(varchar(20),month(getdate())) " & _
        " else convert(varchar(20),month(getdate()))  end + '/' + Convert(varchar(20), Year(getdate()))),'') as DataOdierna,"
        StrSql = StrSql & "isnull(case len(day(asa.dataUltimoStato)) when 1 then '0' +  convert(varchar(20),day(asa.dataUltimoStato)) " & _
        " else convert(varchar(20),day(asa.dataUltimoStato))  end + '/' + (case len(month(asa.dataUltimoStato)) " & _
        " when 1 then '0' + convert(varchar(20),month(asa.dataUltimoStato))  " & _
        " else convert(varchar(20),month(asa.dataUltimoStato))  end + '/' + Convert(varchar(20), " & _
        " Year(asa.dataUltimoStato))),'') as DataApprovazioneGraduatoria,"
        StrSql = StrSql & " isnull(case len(day(e.dataInizioservizio)) when 1 then '0' +  convert(varchar(20),day(e.dataInizioservizio)) " & _
        " else convert(varchar(20),day(e.dataInizioservizio))  end + '/' + (case len(month(e.dataInizioservizio)) " & _
        " when 1 then '0' + convert(varchar(20),month(e.dataInizioservizio))  " & _
        " else convert(varchar(20),month(e.dataInizioservizio))  end + '/' + Convert(varchar(20), " & _
        " Year(e.dataInizioservizio))),'') as DataInizioServizio,"
        StrSql = StrSql & " isnull(case len(day(e.dataFineservizio)) when 1 then '0' +  convert(varchar(20),day(e.dataFineservizio)) " & _
        " else convert(varchar(20),day(e.dataFineservizio))  end + '/' + (case len(month(e.dataFineservizio)) " & _
        " when 1 then '0' + convert(varchar(20),month(e.dataFineservizio))  " & _
        " else convert(varchar(20),month(e.dataFineservizio))  end + '/' + Convert(varchar(20), " & _
        " Year(e.dataFineservizio))),'') as DataFineServizio,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(e.Codicevolontario,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Codicevolontario,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.titolo,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Titolo,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(en.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Ente,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(es.indirizzo,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as indirizzoSede,"
        StrSql = StrSql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.civico,'°',''),'ì','i''')" & _
        ",'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as civicoSede, "
        StrSql = StrSql & " isnull(replace(replace(replace(replace(replace(replace(replace(es.cap,'°',''),'ì', " & _
        " 'i'''),'é','e'''), 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'')as capSede,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(a.codiceEnte,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceProgetto,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(cSa.denominazione,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as comuneSa,"
        StrSql = StrSql & "isnull(replace(replace(replace(replace(replace(replace(replace(pSa.provincia,'°',''),'ì','i'''),'é','e''')," & _
        " 'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as provinciaSa, "
        StrSql = StrSql & "  isnull(case len(day(a.datafineattività)) when 1 then '0' + " & _
        " Convert(varchar(20), Day(a.datafineattività)) " & _
        " else convert(varchar(20),day(a.datafineattività))  " & _
        " end + '/' + (case len(month(a.datafineattività))  when 1 then '0' " & _
        " + convert(varchar(20),month(a.datafineattività))   else convert(varchar(20)," & _
        " month(a.datafineattività))  end + '/' + Convert(varchar(20),  Year(a.datafineattività))),'') " & _
        " as DatafineProgetto,"
        StrSql = StrSql & "  isnull(case len(day(a.dataInizioattività)) when 1 then '0' + " & _
        " Convert(varchar(20), Day(a.dataInizioattività)) " & _
        " else convert(varchar(20),day(a.dataInizioattività))  " & _
        " end + '/' + (case len(month(a.dataInizioattività))  when 1 then '0' " & _
        " + convert(varchar(20),month(a.dataInizioattività))   else convert(varchar(20)," & _
        " month(a.dataInizioattività))  end + '/' + Convert(varchar(20),  Year(a.dataInizioattività))),'') " & _
        " as DataInizioProgetto "
        StrSql = StrSql & " from entità e"
        StrSql = StrSql & " inner join comuni cRes on cRes.idcomune=e.idcomuneResidenza"
        StrSql = StrSql & " inner join provincie pRes on pres.idprovincia=cRes.idprovincia"
        StrSql = StrSql & " inner join graduatorieEntità ge on ge.identità=e.idEntità"
        StrSql = StrSql & " inner join AttivitàSediAssegnazione asa on asa.idattivitàSedeassegnazione=ge.idattivitàSedeassegnazione"
        StrSql = StrSql & " inner join attività a on a.idattività=asa.idattività"
        StrSql = StrSql & " inner join entisedi es on es.identesede=asa.identesede"
        StrSql = StrSql & " inner join comuni cSa on cSa.idcomune=es.idcomune"
        StrSql = StrSql & " inner join provincie pSa on pSa.idprovincia=cSa.idprovincia"
        StrSql = StrSql & " inner join Enti en on  en.idente=es.idente"
        StrSql = StrSql & " where e.identità=" & lblidVolontario.Value & ""

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(StrSql, Session("conn"))
    End Sub
    Function Rinunciaserviziovolontario(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Lettera Adeguamento Positivo Negativo
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

            '---------------------------------------------------------------------------------
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'Dim x As New clsDocumentiRegioni
            'x.RecuperaDati(Session("IdVolSubentrante"), Session("conn"))
            Dim x As New clsDocumentiRegioni
            x.RecuperaDati(Request.QueryString("IdVol"), Session("conn"))

            '---------------------------------------------------------------------------

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '"AS02\par}}")
                'apro il template
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
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta"))
                    xLinea = Replace(xLinea, "<DataRinuncia>", Session("DataInizio"))
                    xLinea = Replace(xLinea, "<NomeVolontario>", txtCognome.Text & " " & txtNome.Text)
                    xLinea = Replace(xLinea, "<NomeVolontarioSostituito>", Session("VolSubentrante"))
                    xLinea = Replace(xLinea, "<CodiceVolontarioVecchio>", "(" & txtCodicevolontario.Text & ")")
                    xLinea = Replace(xLinea, "<CodiceVolontarioSubentrante>", "(" & Session("CodVolSubentrante") & ")")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")


                    xLinea = Replace(xLinea, "<Gazzetta>", x.Gazzetta)
                    xLinea = Replace(xLinea, "<NVol>", x.NVolontari)
                    xLinea = Replace(xLinea, "<IntestazioneRegione>", x.Intestazione)
                    xLinea = Replace(xLinea, "<SettoreRegione>", x.Settore)
                    xLinea = Replace(xLinea, "<IndirizzoRegione>", x.Indirizzo)
                    xLinea = Replace(xLinea, "<CapRegione>", x.Cap)
                    xLinea = Replace(xLinea, "<LocalitaRegione>", x.Località)



                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Rinunciaserviziovolontario = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',2)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function
    Private Sub imgModificaData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgModificaData.Click
        lblErr.Text = ""
        Dim blnCheckUpdate As Boolean

        blnCheckUpdate = True
        Dim DataInizioVolontarioSubentrante As Date
        If txtDataInizioVolontarioSubentrato.Text = "" Then
            lblErr.Text = "Attenzione. La Data Inizio Servizio deve essere valorizzata"
            blnCheckUpdate = False
        ElseIf Date.TryParse(txtDataInizioVolontarioSubentrato.Text, DataInizioVolontarioSubentrante) = False Then
            lblErr.Text = "Attenzione. La Data Inizio Servizio deve essere inserita in un formato valido (gg/mm/aaaa)."
            blnCheckUpdate = False
        ElseIf Date.Parse(txtDataInizioVolontarioSubentrato.Text) < Date.Parse(Session("dataserver")) Or Date.Parse(txtDataInizioVolontarioSubentrato.Text) > Date.Parse(lblFine.Text) Then
            lblErr.Text = "La Data Inizio Servizio dev'essere compresa fra la data odierna e la Data Fine."
            blnCheckUpdate = False
        End If



        If blnCheckUpdate = True Then
            'vado a fare l'update
            Dim cmdUpdate As Data.SqlClient.SqlCommand
            StrSql = "UPDATE entità "
            StrSql = StrSql & "set DataInizioServizio='" & txtDataInizioVolontarioSubentrato.Text & "', "
            StrSql = StrSql & "UserNameUltimaModifica='" & Session("Utente") & "', "
            StrSql = StrSql & "DataUltimaModifica=GetDate() "
            StrSql = StrSql & "WHERE IdEntità='" & Request.QueryString("IdVol") & "' "
            cmdUpdate = New SqlClient.SqlCommand(StrSql, Session("conn"))
            cmdUpdate.ExecuteNonQuery()

            cmdUpdate.Dispose()
            lblErr.Text = "L'aggiornamento è stato eseguito con successo"
        Else

            lblErr.Text = "Attenzione. La Data Inizio Servizio dev'essere compresa fra la data odierna e la Data Fine."
        End If


    End Sub
    Sub CaricaFascicolo(ByVal IdEntità As Integer)

        ' funzione che riporta in maschera i dati del fascicolo associato al volontario
        Dim StrSql As String
        Dim myDateset As DataSet
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'inseriti(campi) entità.CodiceFascicolo, entità.IDFascicolo, entità.DescrFascicolo, Mauro Lanna 11/08

        StrSql = "SELECT entità.CodiceFascicolo, entità.IDFascicolo, entità.DescrFascicolo " & _
                 "FROM entità " & _
                 "WHERE IDEntità = " & IdEntità
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            TxtCodiceFascicolo.Text = "" & dtrgenerico("CodiceFascicolo")
            HdControlloIdFascicolo.Value = TxtCodiceFascicolo.Text
            TxtIdFascicolo.Value = "" & dtrgenerico("IDFascicolo")
            txtDescFasc.Text = "" & dtrgenerico("DescrFascicolo")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Private Sub imgRiepilogoDocumentiVol_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Write("<script>")
        Response.Write("window.open('WfrmRiepilogoDocumentiVolontario.aspx?IdAttivita=" & CInt(lblidAttivita.Value) & "&IdEntita=" & CInt(lblidVolontario.Value) & "', 'RiepilogoDocumenti', 'width=900, height=600,dependent=no,scrollbars=yes,status=si')")
        Response.Write("</script>")
    End Sub
    Private Sub imgScaricaContratto_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgScaricaContratto.Click
        Dim rstVol As SqlDataReader

        'richiamo funzione di siged per scarica il contratto
        StrSql = "Select Cognome + ' ' + Nome AS Nominativo, RiferimentoContrattoVolontario, NomeFileContratto from entità where identità =" & CInt(lblidVolontario.Value) & " and  isnull(StatoContrattoVolontario,0) in(1,2) "
        rstVol = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If rstVol.HasRows = True Then
            rstVol.Read()
            Dim CodiceAllegato As String = rstVol("RiferimentoContrattoVolontario")
            Dim NomeFileContratto As String = rstVol("NomeFileContratto")
            Dim Volontario As String = rstVol("Nominativo")
            If Not rstVol Is Nothing Then
                rstVol.Close()
                rstVol = Nothing
            End If
            DownloadContratto(CodiceAllegato, Volontario, NomeFileContratto)
        End If
        If Not rstVol Is Nothing Then
            rstVol.Close()
            rstVol = Nothing
        End If
    End Sub
    Private Function DownloadContratto(ByVal CodiceAllegato As String, ByVal Volontario As String, ByVal NomeFileContratto As String)
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE: 15/05/2012
        'FUNZIONALITA': RECUPERA IL DOCUMENTO RICHIESTO DAL SISTEMA DOCUMENTALE.
        Dim SIGED As clsSiged
        Dim wsOut As WS_SIGeD.INDICE_ALLEGATO
        Dim objHLink As HyperLink
        Dim strNome As String
        Dim strCognome As String
        Dim strSQL As String
        Dim dsUser As DataSet
        Dim PathServerSiged As String
        Dim NomeFile As String
        Dim myPath As New System.Web.UI.Page
        Dim PathLocale As String
        Try
            'verifica che l'utente sia autorizzato all'accesso al sistema documentale
            strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & Session("Utente") & "'"
            dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))
            If dsUser.Tables(0).Rows.Count <> 0 Then
                strNome = dsUser.Tables(0).Rows(0).Item("Nome")
                strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
            End If
            SIGED = New clsSiged("", strNome, strCognome)

            If SIGED.Codice_Esito <> 0 Then
                Exit Function
            End If

            'Ottiene il percorso per recuperare il file
            PathServerSiged = "\\" & ConfigurationSettings.AppSettings("SERVER_SIGED") & "\" & ConfigurationSettings.AppSettings("CARTELLA_SIGED")  '& "\" & pNomeFile
            ' NomeFile = "Contratto " & Volontario & ".pdf"

            wsOut = SIGED.SIGED_RestituisciDocumentoInterno(CodiceAllegato, "", PathServerSiged & "\" & Trim(NomeFile))
            If SIGED.SIGED_Codice_Esito(wsOut.ESITO) = 0 Then
                PathLocale = myPath.Server.MapPath("download\") & NomeFileContratto  '& NomeFile
                If File.Exists(PathLocale) = True Then
                    File.Delete(PathLocale)
                End If
                File.Copy(PathServerSiged & "\" & wsOut.DATI.NOMEFILE, Trim(PathLocale))
                imgScaricaContratto.Visible = False
                HplScaricaContratto.Visible = True
                HplScaricaContratto.Text = "download\" & NomeFileContratto  'PathLocale
            Else
                'lblmsg.Text = Mid(wsOut.ESITO, 6, Len(wsOut.ESITO))
                imgScaricaContratto.Visible = False
                HplScaricaContratto.Visible = False
            End If

        Catch ex As Exception
            'lblmsg.Text = "Errore imprevisto. Contattare l'assistenza."
            imgScaricaContratto.Visible = False
            HplScaricaContratto.Visible = False
        Finally
            SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
        End Try
    End Function
    Sub VisualizzaContratto(ByVal StatoContratto As Integer, ByVal AbilitaBandoContratto As Integer)
        'bando non abilitato al caricamento del contratto
        If AbilitaBandoContratto = 0 Then
            LblContratto.Text = CONTRATTO_NON_PREVISTO
        Else
            'bando abilitato al caricamento del contratto dall'area riservate volontario (sito servizio civile)
            Select Case StatoContratto
                Case 0 'contratto DA CARICARE
                    LblContratto.Text = CONTRATTO_DA_CARICARE
                    TerminiCaricamentoContratto(CInt(lblidVolontario.Value))
                Case 1 'contratto CARICATO
                    LblContratto.Text = CONTRATTO_CARICATO
                    imgScaricaContratto.Visible = True
                    imgValutazioneContratto.Visible = True
                Case 2 'contratto APPROVATO
                    LblContratto.Text = CONTRATTO_APPROVATO
                    imgScaricaContratto.Visible = True
                    imgValutazioneContratto.Visible = True
                Case 3 'contratto RESPINTO
                    LblContratto.Text = CONTRATTO_RESPINTO
                    TerminiCaricamentoContratto(CInt(lblidVolontario.Value))
            End Select
            If (LblContratto.Text = String.Empty) Then
                DivContratto.Visible = False
            Else
                DivContratto.Visible = True
            End If
        End If

    End Sub
    Private Sub imgValutazioneContratto_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgValutazioneContratto.Click
        Dim intStatoContratto As Integer

        Select Case LblContratto.Text
            Case CONTRATTO_DA_CARICARE  '0
                intStatoContratto = 0
            Case CONTRATTO_CARICATO  '1contratto CARICATO
                intStatoContratto = 1
            Case CONTRATTO_APPROVATO  '2contratto APPROVATO
                intStatoContratto = 2
            Case CONTRATTO_RESPINTO  '3contratto RESPINTO
                intStatoContratto = 3
        End Select

        Response.Redirect("WfrmVerificaContrattoVolontari.aspx?StatoContratto=" & intStatoContratto & "&IdEntita=" & lblidVolontario.Value & "&CodiceVolontario=" & txtCodicevolontario.Text)
    End Sub
    Private Sub TerminiCaricamentoContratto(ByVal IdEntita As Integer)
        Dim strDataProroga As String = "0"
        'Dim strSql As String
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'verifico la scadenza dei termini per il caricamento del contratto
        StrSql = " Select dbo.FN_VERIFICA_TERMINI_SCADENZA_CONTRATTO(" & CInt(lblidVolontario.Value) & ") as TerminiScadenza, " & _
                 " ISNULL(CONVERT(varchar, DataProrogaContratto, 103), 0) as DataProrogaContratto " & _
                 " FROM entità WHERE identità= " & CInt(lblidVolontario.Value) & ""

        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrgenerico.HasRows Then
            dtrgenerico.Read()
            strDataProroga = dtrgenerico("DataProrogaContratto")
            If dtrgenerico("TerminiScadenza") = 0 Then
                imgProroga.Visible = True
                DivProrogaContratto.Visible = True
                DivContrattoProrogato.Visible = False
            Else
                If strDataProroga <> "0" Then
                    LblProroga.Visible = True
                    LabelProroga.Visible = True
                    LblProroga.Text = strDataProroga
                    imgProroga.Visible = False
                    DivContrattoProrogato.Visible = True
                    DivProrogaContratto.Visible = False
                End If
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Private Sub imgProroga_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgProroga.Click

        Dim myCommand As SqlClient.SqlCommand

        myCommand = New SqlClient.SqlCommand("UPDATE Entità set DataProrogaContratto= getdate() where IdEntità = " & lblidVolontario.Value & "  ", Session("Conn"))
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()
        'aggiunto da simona cordella il 19/10/2012
        VisualizzaContratto(0, 1)

    End Sub
    Private Sub CaricaComboCategoria()
        'INIZIO ANTONELLO 11/12/2013
        cboCategoria.Items.Clear()
        StrSql = " SELECT '' as IdCategoriaEntità, '' as CategoriaAbbreviata FROM CategorieEntità union " & _
                 " SELECT IdCategoriaEntità,CategoriaAbbreviata " & _
                 " FROM CategorieEntità Order by IdCategoriaEntità "

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrgenerico.HasRows Then
            'carico la combo delle sedi di attuazione
            cboCategoria.DataSource = dtrgenerico
            cboCategoria.DataTextField = "CategoriaAbbreviata"
            cboCategoria.DataValueField = "IdCategoriaEntità"
            cboCategoria.DataBind()

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If


    End Sub
    Private Sub CaricaComboStatoCivile()
        'INIZIO ANTONELLO 16/05/2016
        cboStatoCivile.Items.Clear()
        '" SELECT '' as IdCategoriaEntità, '' as CategoriaAbbreviata FROM CategorieEntità union " & _
        StrSql = " SELECT '' as IdTipoStatoCivile, '' as StatoCivile FROM TipiStatoCivile union " & _
        " SELECT IdTipoStatoCivile,StatoCivile " & _
        " FROM TipiStatoCivile Order by IdTipoStatoCivile "

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrgenerico.HasRows Then
            'carico la combo delle sedi di attuazione
            cboStatoCivile.DataSource = dtrgenerico
            cboStatoCivile.DataTextField = "StatoCivile"
            cboStatoCivile.DataValueField = "IdTipoStatoCivile"
            cboStatoCivile.DataBind()

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If


    End Sub
    Private Sub CaricaComboConseguimento()
        '--------------------------------------------------------------------------

        StrSql = "SELECT * "
        StrSql = StrSql & " FROM TitoliStudioConseguimento Order by IdTitoloStudioConseguimento "

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrgenerico.HasRows Then
            'carico la combo delle sedi di attuazione
            CboConseguimentoTS.DataSource = dtrgenerico
            CboConseguimentoTS.DataTextField = "TitoloStudioConseguimentoAbbreviato"
            CboConseguimentoTS.DataValueField = "IdTitoloStudioConseguimento"
            CboConseguimentoTS.DataBind()

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
        'Fine ANTONELLO
    End Sub
    Private Sub CaricaComboNazionalita()
        'aggiunto da simona cordella il 12/01/2015
        ddlNazionalita.Items.Clear()
        StrSql = " Select 1 as ordine,'' AS IdComune,'' AS denominazione  "
        StrSql = StrSql & " UNION "
        StrSql = StrSql & " Select 2 as ordine,'38715' AS IdComune,'ITALIA' AS denominazione  "
        StrSql = StrSql & " UNION "
        StrSql = StrSql & " Select 3 as ordine,IdComune,denominazione from Comuni"
        StrSql = StrSql & " where comunenazionale=0 and isnull(codiceistat,'')<>'' "
        StrSql = StrSql & " order by ordine,denominazione"


        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrgenerico.HasRows Then
            'carico la combo delle sedi di attuazione
            ddlNazionalita.DataSource = dtrgenerico
            ddlNazionalita.DataTextField = "denominazione"
            ddlNazionalita.DataValueField = "IdComune"
            ddlNazionalita.DataBind()

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If

    End Sub
    Private Sub CaricaComboSedeSecondaria(ByVal IDAttivita As Integer)
        'INIZIO SIMONA 30/07/2018
        CboSedeAttSecondaria.Items.Clear()

        StrSql = " SELECT '' as IdEnteSedeAttuazione,'' as Id UNION "
        StrSql = StrSql & " SELECT CONVERT(varchar,isnull(c.IDEnteSedeAttuazione,0)),c.IDEnteSedeAttuazione AS ID from attività a"
        StrSql = StrSql & " INNER JOIN TipiProgetto b on a.IdTipoProgetto = b.IdTipoProgetto "
        StrSql = StrSql & " INNER JOIN attivitàentisediattuazione c on a.IDAttività = c.IDAttività "
        StrSql = StrSql & " INNER JOIN entisediattuazioni d on c.IDEnteSedeAttuazione = d.IDEnteSedeAttuazione "
        StrSql = StrSql & " INNER JOIN entisedi e on d.IDEnteSede = e.IDEnteSede "
        StrSql = StrSql & " INNER JOIN comuni f on e.IDComune = f.IDComune "
        StrSql = StrSql & " WHERE a.IDAttività = " & IDAttivita & " and b.NazioneBase <> f.ComuneNazionale  "


        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrgenerico.HasRows Then
            'carico la combo delle sedi di attuazione
            CboSedeAttSecondaria.DataSource = dtrgenerico
            CboSedeAttSecondaria.DataTextField = "IdEnteSedeAttuazione"
            CboSedeAttSecondaria.DataValueField = "ID"
            CboSedeAttSecondaria.DataBind()

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If


    End Sub
    Private Sub CaricaComboStatoVerificaCF()
        'Aggiunta Luigi Lecci 05/03/2019
        Dim MyDataset As DataSet

        cboStatoVerificaCF.Items.Clear()
        StrSql = "SELECT '0' AS IDStatiVerificaCFEntità, '' AS Descrizione FROM StatiVerificaCFEntità  " & _
                 "UNION SELECT IDStatiVerificaCFEntità, Descrizione FROM StatiVerificaCFEntità"
        MyDataset = ClsServer.DataSetGenerico(StrSql, Session("conn"))
        cboStatoVerificaCF.DataSource = MyDataset
        cboStatoVerificaCF.DataValueField = "IDStatiVerificaCFEntità"
        cboStatoVerificaCF.DataTextField = "Descrizione"
        cboStatoVerificaCF.DataBind()
    End Sub
    Protected Sub cmdSalva_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSalva.Click
        lblErr.Text = String.Empty
        lblConferma.Text = String.Empty
        If VerificaCampiObbligatori() = False Then
            Exit Sub
        End If
        '*************************************************************************************************
        'DESCRIZIONE: Inserimento di un nuovo volontario (entità)
        'AUTORE: TESTA GUIDO    DATA: 13/10/2004
        '*************************************************************************************************
        Dim strSQL As String
        Dim sCognome As String
        Dim sNome As String
        Dim sSesso As String
        Dim sStatoCivile As String
        Dim sCodiceFiscale As String
        Dim sCodiceFiscaleConiuge As String
        Dim sCategoria As String
        Dim sEmail1 As String
        Dim sTele1 As String
        Dim sCell1 As String
        Dim sFax1 As String
        Dim sDataNasc As String
        Dim sIndirizzo1 As String
        Dim NCivic1 As String
        Dim NCap1 As String
        Dim sIndirizzo2 As String
        Dim NCivic2 As String
        Dim NCap2 As String
        Dim sEmail2 As String
        Dim sTele2 As String
        Dim sTitoloStud As String
        Dim sConseguimentoTitoloStudio As String
        Dim sConseguitoIn As String
        Dim sAnnoIscr1 As String
        Dim sScuolaSuperiore As String
        Dim sAnnoIscr2 As String
        Dim sCorsoLaureaIn As String
        Dim sAltriTitoli As String
        Dim sCorsi As String
        Dim sEsperienze As String
        Dim sConoscenze As String
        Dim sMotivi As String
        Dim sAltreinfo As String
        Dim sUffPostale As String
        Dim sDisp1 As String
        Dim sDisp2 As String
        Dim sCodiceLibretto As String
        Dim sIban As String
        Dim sBicSwift As String
        Dim sBanca As String
        Dim sCC As String
        Dim sAbi As String
        Dim sCab As String
        Dim sCin As String
        Dim strCodCatasto As String
        Dim strNewCF As String
        Dim strSesso As String


        Dim myDataSet As New DataSet
        Dim MyCommand As SqlCommand
        Dim iMaxId As Integer
        Dim sCodCatasto As String

        Dim sDettaglioDomicilio As String
        Dim sDettaglioResidenza As String
        Dim clsIban As New CheckBancari
        Dim sPunteggio As String
        Dim sCheckboxIdoneo As Integer
        Dim sDataDomanda As String
        Dim strCodiceProgetto As String
        '***********************************************************
        'Aggiunto da Amilcare Paolella il 1/12/2005 ****************
        'Controllo sull'esistenza del codice libretto postale ******
        Dim strCheckLib As String = CheckExistLibretto(txtLibretto.Text)
        If strCheckLib <> "" Then
            IdComuneNascita.Value = txtComuneNascita.Text
            IdComuneResidenza.Value = ddlComuneResidenza.SelectedItem.Text
            lblErr.Text = lblErr.Text & "Il codice di libretto postale inserito è già stato assegnato al volontario " & strCheckLib & ".<br/>"
            Exit Sub
        End If
        '***********************************************************

        'mod. il 12/05/2015 verifico se è stato modificata la residenza e/o il domicilio; aaggiorno le variabili che ricordano l'id del comune indicato
        If lblidComuneResidenza.Value <> ddlComuneResidenza.SelectedValue Then
            lblidComuneResidenza.Value = ddlComuneResidenza.SelectedValue
        End If
        If ddlProvinciaDomicilio.SelectedItem.Text <> "" Then
            If lblidComuneDomicilio.Value <> ddlComuneDomicilio.SelectedValue Then
                lblidComuneDomicilio.Value = ddlComuneDomicilio.SelectedValue
            End If
        End If

        'controllo codiceiban
        'Funzione che controlla l'autenticità del codice iban indicato
        'Mod. il 27/10/2009 controllo l'iban solo se il conto è in italia
        If Left(Trim(txtIBAN.Text), 2) = "IT" Then
            Dim ChkCalcolaIban As String = clsIban.CalcolaIBAN(Left(Trim(txtIBAN.Text), 2), Mid(Trim(txtIBAN.Text), 5))
            If clsIban.VerificaLetteraCin(Mid(Trim(txtIBAN.Text), 5, 1)) = "1" Then
                lblErr.Text = lblErr.Text & "Codice IBAN errato.<br/>"
                Exit Sub
            End If
            If ChkCalcolaIban <> Trim(txtIBAN.Text) Then
                lblErr.Text = lblErr.Text & "Codice IBAN errato.<br/>"
                Exit Sub
            Else
                If ClsUtility.AbiCab(UCase(Trim(txtIBAN.Text))) Then
                    lblErr.Text = lblErr.Text & "Il codice IBAN indicato non fa riferimento ad un conto corrente bancario.<br/>"
                    Exit Sub
                End If
            End If
        Else
            If Len(Trim(txtIBAN.Text)) > 31 Then
                lblErr.Text = lblErr.Text & "IBAN non valido, verificare la lunghezza del codice IBAN inserito.<br/>"
                Exit Sub
            End If
            If Trim(txtIBAN.Text) <> "" And Trim(txtBicSwift.Text) = "" Then
                lblErr.Text = lblErr.Text & "E' necessario indicare il campo Bic/Swift per il conto Estero.<br/>"
                Exit Sub
            End If
        End If
        If Trim(txtBicSwift.Text) <> "" And Trim(txtIBAN.Text) = "" Then
            lblErr.Text = lblErr.Text & "Indicare l'IBAN .<br/>"
            Exit Sub
        End If
        If Left(Trim(txtIBAN.Text), 2) = "IT" And Trim(txtBicSwift.Text) <> "" Then
            If (Len(Trim(txtBicSwift.Text)) <> 8 And Len(Trim(txtBicSwift.Text)) <> 11) Then
                lblErr.Text = lblErr.Text & "La lunghezza del codice Bic/Swift e' errata.<br/>"
                Exit Sub
            End If

        End If

        'If cboStatoCivile.s <> "" Then
        '    lblErr.Text = lblErr.Text & "La lunghezza del codice Bic/Swift e' errata.<br/>"
        '    Exit Sub
        'End If

        ' 18/02/82019 Luigi Leucci - Verifica Minori Opportunità e FAMI
        If Request.Params("IdVol") <> "" Then 'ADC
            strSQL = "SELECT DataInizioServizio, ISNULL(GMO, '') AS Gmo, ISNULL(FAMI, '') AS Fami FROM Entità WHERE IDEntità = " & Request.Params("IdVol")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            dtrgenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
            With dtrgenerico
                If .HasRows Then
                    .Read()
                    If Not IsDBNull(.Item("DataInizioServizio")) Then
                        If .Item("Gmo") <> "" And ddlGMO.SelectedValue = "" Then
                            lblErr.Text = lblErr.Text & "Impossibile togliere la classificazione GMO.<br/>"
                            Exit Sub
                        End If
                        If .Item("Fami") <> "" And ddlFAMI.SelectedValue = "" Then
                            lblErr.Text = lblErr.Text & "Impossibile togliere la classificazione Fami.<br/>"
                            Exit Sub
                        End If
                    End If
                End If
            End With
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        Dim strMiaCausale As String = ""
        If ClsUtility.CAP_VERIFICA_VOLONTARI(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), _
                strMiaCausale, bandiera, Trim(txtCAP.Text), IIf(Request.Form("txtIDComuneResidenza") = "", lblidComuneResidenza.Value, Request.Form("txtIDComuneResidenza")), "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
            'Ripristino lo stato del tasto
            cmdSalva.Visible = True
            'Inserisco il Messaggio di Errore
            lblErr.Text = strMiaCausale
            Response.Write("<input type=hidden name=IdComuneNascita value=""" & Request.Form("txtIDComuneNascita") & """>")
            Response.Write("<input type=hidden name=IdComuneResidenza value=""" & Request.Form("txtIDComuneResidenza") & """>")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Exit Sub
        End If


        If Request.Form("txtIDComuneDomicilio") <> "" Or lblidComuneDomicilio.Value <> "" Then
            Dim strMiaCausale2 As String = ""
            If ClsUtility.CAP_VERIFICA_VOLONTARI(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), _
                    strMiaCausale2, bandiera, Trim(txtCapDomicilio.Text), IIf(Request.Form("txtIDComuneDomicilio") = "", lblidComuneDomicilio.Value, Request.Form("txtIDComuneDomicilio")), "", "", txtIndirizzoDomicilio.Text, txtCivicoDomicilio.Text) = False Then
                'Ripristino lo stato del tasto
                cmdSalva.Visible = True
                'Inserisco il Messaggio di Errore
                lblErr.Text = strMiaCausale2
                Response.Write("<input type=hidden name=IdComuneNascita value=""" & Request.Form("txtIDComuneNascita") & """>")
                Response.Write("<input type=hidden name=IdComuneResidenza value=""" & Request.Form("txtIDComuneResidenza") & """>")
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Exit Sub
            End If
        End If


        idComuneNasc = lblidComuneNascita.Value
        If cboSesso.SelectedValue = 1 Then
            strSesso = "F"
        Else
            strSesso = "M"
        End If

        Dim intCodFisc As Integer = 0 ' 0=OK - 1=Lunghezza errata - 2=Non Congruente
        Dim ErrCodFisc As String = String.Empty

        'controllo codice fiscale
        ' -- Modificato da Luigi Leucci il 05/03/2019
        If txtCodiceFiscale.Text <> "" Then
            If Len(txtCodiceFiscale.Text) <> 16 Then
                intCodFisc = 1
            Else
                'ricavo il codice catastale del comune
                strCodCatasto = ClsUtility.GetCodiceCatasto(Session("conn"), idComuneNasc)
                'genero il CF con i dati anagrafici del volontario
                strNewCF = ClsUtility.CreaCF(Trim(Replace(txtCognome.Text, "'", "''")), Trim(Replace(txtNome.Text, "'", "''")), Trim(txtDataNascita.Text), strCodCatasto, Trim(strSesso))

                If UCase(Trim(txtCodiceFiscale.Text)) <> UCase(strNewCF) Then
                    'verifo eventuale OMOCODIA
                    If ClsUtility.VerificaOmocodia(UCase(strNewCF), UCase(Trim(txtCodiceFiscale.Text))) = False Then
                        intCodFisc = 2
                    End If
                End If
            End If
        End If

        imgAnomaliaCF.Visible = intCodFisc > 0

        Select Case intCodFisc
            Case 0 ' OK
                If cboStatoVerificaCF.SelectedValue > 0 Then ErrCodFisc = " Non è stato preso in considerazione il valore impostato in '" & LblStatoVerificaCF.Text & "' in quanto il Codice Fiscale risulta corretto."
                cboStatoVerificaCF.SelectedValue = 0

            Case 1 And Not cboStatoVerificaCF.Enabled
                ErrCodFisc = " Attenzione! Codice Fiscale non corretto."
                If cboStatoVerificaCF.SelectedValue < 1 Then cboStatoVerificaCF.SelectedValue = 1

            Case 2 And Not cboStatoVerificaCF.Enabled
                ErrCodFisc = " Attenzione! Codice Fiscale non congruente con i dati inseriti."
                If cboStatoVerificaCF.SelectedValue < 1 Then cboStatoVerificaCF.SelectedValue = 1

            Case 1 And cboStatoVerificaCF.Enabled
                If cboStatoVerificaCF.SelectedIndex < 1 Then
                    lblErr.Text = lblErr.Text & "Codice Fiscale non corretto. Selezionare lo stato di verifica del Codice Fiscale.<br/>"
                    Exit Sub
                End If

            Case 2 And cboStatoVerificaCF.Enabled
                If cboStatoVerificaCF.SelectedIndex < 1 Then
                    lblErr.Text = lblErr.Text & "Codice Fiscale non congruente con i dati inseriti. Selezionare lo stato di verifica del Codice Fiscale.<br/>"
                    Exit Sub
                End If
        End Select

        Dim BooCronologiaCF As Boolean = False
        If Request.QueryString("IdVol") <> vbNullString Then
            strSQL = "SELECT CodiceFiscale FROM Entità WHERE identità = " & Request.Params("IdVol")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                BooCronologiaCF = (dtrgenerico("CodiceFiscale") <> txtCodiceFiscale.Text)
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If

        Try
            sCognome = Replace(txtCognome.Text, "'", "''")
            sNome = Replace(txtNome.Text, "'", "''")
            sSesso = cboSesso.SelectedValue
            sStatoCivile = cboStatoCivile.SelectedValue



            Dim StrSql0 As String
            StrSql0 = "SELECT  CFConiugeObbligatorio FROM TipiStatoCivile where IdTipoStatoCivile = " & sStatoCivile
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(StrSql0, Session("conn"))
            Dim necessario As Boolean
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                If dtrgenerico("CFConiugeObbligatorio") = True Then
                    sCodiceFiscaleConiuge = Replace(txtCodiceFiscaleConiuge.Text, "'", "''")
                    If sCodiceFiscaleConiuge <> "" Then
                        If Len(sCodiceFiscaleConiuge) <> 16 Then
                            If Not dtrgenerico Is Nothing Then
                                dtrgenerico.Close()
                                dtrgenerico = Nothing
                            End If
                            lblErr.Text = lblErr.Text & "Codice Fiscale Coniuge non corretto.<br/>"
                            Exit Sub
                        Else
                            If Not dtrgenerico Is Nothing Then
                                dtrgenerico.Close()
                                dtrgenerico = Nothing
                            End If
                            If ClsUtility.VerificaValiditàCodiceFiscaleConiuge(Trim(sCodiceFiscaleConiuge)) = False Then
                                lblErr.Text = lblErr.Text & "Codice Fiscale Coniuge non risulta un codice valido.<br/>"
                                Exit Sub
                            End If

                        End If
                    Else
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        lblErr.Text = lblErr.Text & "Codice Fiscale Coniuge è obbligatorio.<br/>"
                        Exit Sub
                    End If
                Else
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    sCodiceFiscaleConiuge = ""
                End If


            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            '08/08/2018 da s.c. obbligo di inserire la sede secondaria (inserita in fase di caricamento della gradautoria)
            If CboSedeAttSecondaria.SelectedItem.Text = "" Then
                'se la sede è stata tolta ma nel db è presente un valore, è obbligatoriO indicare la sede secondaria
                If ControlloIndicazioneSedeSecondaria(Request.Params("IdVol")) = True Then
                    lblErr.Text = lblErr.Text & "E' necessario indicare la Sede Secondaria.<br/>"
                    Exit Sub
                End If
            End If





            sCodiceFiscale = Replace(txtCodiceFiscale.Text, "'", "''")
            sEmail1 = Replace(txtEmail.Text, "'", "''")
            sTele1 = Replace(txtTelefono.Text, "'", "''")
            sCell1 = Replace(txtCellulare.Text, "'", "''")
            sFax1 = Replace(txtFax.Text, "'", "''")
            sDataNasc = txtDataNascita.Text
            sDataDomanda = TxtDataDomanda.Text
            If Not IsNothing(Request.Params("txtIDComuneNascita")) Then
                If Request.Params("txtIDComuneNascita") <> "" Then
                    idComuneNasc = Request.Params("txtIDComuneNascita")
                Else
                    idComuneNasc = lblidComuneNascita.Value
                End If
            Else
                idComuneNasc = lblidComuneNascita.Value
            End If
            If Not IsNothing(Request.Params("txtIDComuneResidenza")) Then
                If Request.Params("txtIDComuneResidenza") <> "" Then
                    idComuneRes = Request.Params("txtIDComuneResidenza")
                Else
                    idComuneRes = lblidComuneResidenza.Value
                End If
            Else
                idComuneRes = lblidComuneResidenza.Value
            End If
            sIndirizzo1 = Replace(txtIndirizzo.Text, "'", "''")
            NCivic1 = Replace(txtCivico.Text, "'", "''")
            NCap1 = Replace(txtCAP.Text, "'", "''")
            sDettaglioResidenza = Replace(txtDettaglioRecapitoResidenza.Text, "'", "''")
            If Not IsNothing(Request.Params("txtIDComuneDomicilio")) Then
                If Request.Params("txtIDComuneDomicilio") <> "" Then
                    idComuneDom = Request.Params("txtIDComuneDomicilio")
                Else
                    idComuneDom = lblidComuneDomicilio.Value
                End If
            Else
                idComuneDom = lblidComuneDomicilio.Value
            End If
            sIndirizzo2 = Replace(txtIndirizzoDomicilio.Text, "'", "''")
            sDettaglioDomicilio = Replace(TxtDettaglioRecapitoDomicilio.Text, "'", "''")
            NCivic2 = Replace(txtCivicoDomicilio.Text, "'", "''")
            NCap2 = Replace(txtCapDomicilio.Text, "'", "''")
            sTele2 = Replace(txtTelefonoDomicilio.Text, "'", "''")
            sTitoloStud = Replace(txtTitoloStudio.Text, "'", "''")
            sAltreinfo = Replace(Request.Form("HDDAltreInfo"), "'", "''")
            txtAltreInformazioni.Text = sAltreinfo
            sCodiceLibretto = IIf(txtLibretto.Text <> "", "'" & Replace(txtLibretto.Text, "'", "''") & "'", "null")
            sIban = IIf(Trim(txtIBAN.Text) <> "", "'" & Replace(Trim(txtIBAN.Text), "'", "''") & "'", "null")
            sBicSwift = IIf(Trim(txtBicSwift.Text) <> "", "'" & Replace(Trim(txtBicSwift.Text), "'", "''") & "'", "null")

            If chkDisp1.Checked = True Then
                sDisp1 = "1"
            Else
                sDisp1 = "0"
            End If
            If chkDisp2.Checked = True Then
                sDisp2 = "1"
            Else
                sDisp2 = "0"
            End If

            If CheckboxIdoneo.Checked = True Then
                idoneoSiNo = 1
            Else
                idoneoSiNo = 2
            End If

            sPunteggio = Replace(txtPunteggio.Text, ",", ".")

            If Request.Params("IdVol") <> "" Then
                ' Aggiunta da Luigi Leucci 06/03/2019 
                If BooCronologiaCF Then
                    strSQL = "INSERT INTO CronologiaStatiVerificaCFEntità " & _
                             "(IDEntità, UserName, Data, VecchioIDStato, VecchioCodiceFiscale, NuovoCodiceFiscale, NuovoIDStato) " & _
                             "SELECT IDEntità, '" & _
                             Session("Utente") & _
                             "', GETDATE(), IDStatiVerificaCFEntità, CodiceFiscale, '" & _
                             txtCodiceFiscale.Text & "', " & _
                             IIf(cboStatoVerificaCF.SelectedIndex > 0, cboStatoVerificaCF.SelectedValue, " NULL") & _
                             " FROM Entità " & _
                             " WHERE IDEntità=" & Request.Params("IdVol")
                    MyCommand = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                End If
                ' Fine aggiunta

                strSQL = "insert into CronologiaEntità "
                strSQL = strSQL & "(IdEntità, IDStatoEntità, UserNameStato, DataChiusura, DataCronologia, NoteStato, IDCausaleChiusura) "
                strSQL = "select IdEntità,IdStatoEntità, UserNameStato, DataChiusura, DataUltimoStato, NoteStato, IdCausaleChiusura from entità "
                strSQL = strSQL & "where identità=" & CInt(Request.Params("IdVol"))
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                MyCommand = ClsServer.EseguiSqlClient(strSQL, Session("conn"))

                ScriviCronogliaEntitaDettagli()


                If HdControlloIdFascicolo.Value <> TxtCodiceFascicolo.Text Then 'è vuoto quindi è una nuova associazione
                    strSQL = "Update cronologiaentitàdocumenti SET dataprot = null , nprot='' where identità = " & Request.Params("IdVol")
                    MyCommand = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                End If

                strSQL = "Update entità SET Cognome='" & sCognome & "',Nome='" & sNome & "', Sesso='" & sSesso & "', "
                strSQL &= "Indirizzo='" & sIndirizzo1 & "',DettaglioRecapitoResidenza='" & sDettaglioResidenza & "',FlagIndirizzoValidoRes=" & bandiera & ",CAP='" & NCap1 & "',NumeroCivico='" & NCivic1 & "',IDComuneNascita=" & idComuneNasc & ", "
                strSQL &= "Telefono='" & sTele1 & "',Cellulare='" & sCell1 & "',Email='" & sEmail1 & "',Fax='" & sFax1 & "',DataNascita=Convert(DateTime,'" & sDataNasc & "',103), "
                strSQL &= "IDComuneResidenza=" & idComuneRes & ",CodiceFiscale='" & sCodiceFiscale & "',IDComuneDomicilio=" & IIf(idComuneDom = "", "null", idComuneDom) & ", "
                strSQL &= "IndirizzoDomicilio='" & sIndirizzo2 & "',DettaglioRecapitoDomicilio='" & sDettaglioDomicilio & "',FlagIndirizzoValidoDom=" & BandieraDomicilio & ",NumeroCivicoDomicilio='" & NCivic2 & "',CAPDomicilio='" & NCap2 & "',TelefonoDomicilio='" & sTele2 & "',EmailDomicilio='" & sEmail2 & "', "
                strSQL &= "TitoloStudio='" & sTitoloStud & "',IstitutoStudio='" & sConseguitoIn & "',NumAnnoScuolaMedia=" & IIf(sAnnoIscr1 = "", "null", sAnnoIscr1) & ",IstitutoMediaSuperiore='" & sScuolaSuperiore & "', "
                strSQL &= "NumAnnoAccademico=" & IIf(sAnnoIscr2 = "", "null", sAnnoIscr2) & ",TitoloLaurea='" & sCorsoLaureaIn & "',AltriTitoli='" & sAltriTitoli & "',Corsi='" & sCorsi & "',Esperienze='" & sEsperienze & "', "
                strSQL &= "AltreConoscenze='" & sConoscenze & "',MotiviSceltaProgetto='" & sMotivi & "',AltreInformazioni='" & sAltreinfo & "', DisponibileStessoProg=" & sDisp1 & ", "
                strSQL &= "DisponibileAltriProg=" & sDisp2 & ", CodiceLibrettoPostale=" & sCodiceLibretto & ", IBAN = " & sIban & ", BIC_SWIFT = " & sBicSwift & ", "
                strSQL &= " TMPIdSedeAttuazione='" & CboSedeAtt.SelectedItem.Text & "', TMPIdSedeAttuazioneSecondaria=" & IIf(CboSedeAttSecondaria.SelectedItem.Text = "", "null", CboSedeAttSecondaria.SelectedItem.Text) & ", "
                strSQL &= " RitornoMittente=" & IIf(chkIndirizzoErrato.Checked = True, 1, 0) & " "
                strSQL &= ", CodiceFascicolo ='" & TxtCodiceFascicolo.Text & "', IDFascicolo = '" & TxtIdFascicolo.Value & "' "
                strSQL &= ", DescrFascicolo = '" & Replace(txtDescFasc.Text, "'", "''") & "',IdCategoriaEntità='" & cboCategoria.SelectedValue & "',IdTitoloStudioConseguimento='" & CboConseguimentoTS.SelectedValue

                ' -- Modificato da Luigi Leucci il 05/03/2019
                strSQL &= "', AnomaliaCF =" & IIf(cboStatoVerificaCF.SelectedIndex > 0, 1, 0)
                strSQL &= ", IDStatiVerificaCFEntità = " & IIf(cboStatoVerificaCF.SelectedIndex > 0, cboStatoVerificaCF.SelectedValue, " NULL")
                ' -----

                strSQL &= " ,IdNazionalita='" & ddlNazionalita.SelectedValue & "',CodiceFiscaleConiuge=" & IIf(sCodiceFiscaleConiuge = "", "null", "'" & sCodiceFiscaleConiuge & "'") & " ,IdTipoStatoCivile=" & IIf(sStatoCivile = "0", "null", "'" & sStatoCivile & "'") & "  "

                If ddlGMO.SelectedValue = "" Then
                    strSQL &= " ,GMO= NULL "
                Else
                    strSQL &= " ,GMO='" & ddlGMO.SelectedValue & "' "
                End If
                If ddlFAMI.SelectedValue = "" Then
                    strSQL &= " ,FAMI=NULL "
                Else
                    strSQL &= " ,FAMI='" & ddlFAMI.SelectedValue & "' "
                End If
                '  " ,GMO='" & IIf(ddlGMO.SelectedValue = "", "null", ddlGMO.SelectedValue) & "' , FAMI='" & IIf(ddlFAMI.SelectedValue = "", "null", ddlFAMI.SelectedValue) & "'  "

                'Aggiornata da Danilo Spagnulo il 13/10/2006: controllo se deve essere aggiornata anche sede prima assegnazione
                Dim StrSql1 As String
                StrSql1 = "Select ISNULL(TMPIDSEDEATTUAZIONE,-1) as TMPIDSEDEATTUAZIONE, ISNULL(IDSEDEPRIMAASSEGNAZIONE,-1) as IDSEDEPRIMAASSEGNAZIONE FROM ENTITà where IDENTITà = " & Request.Params("IdVol")
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                dtrgenerico = ClsServer.CreaDatareader(StrSql1, Session("conn"))
                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    If dtrgenerico("IDSEDEPRIMAASSEGNAZIONE") <> -1 And dtrgenerico("IDSEDEPRIMAASSEGNAZIONE") <> dtrgenerico("TMPIDSEDEATTUAZIONE") Then

                    Else
                        strSQL = strSQL & ",IdSedePrimaAssegnazione= '" & CboSedeAtt.SelectedItem.Text & "' "
                    End If
                End If
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                'Fine Modifica Danilo Spagnulo
                strSQL = strSQL & " WHERE IDEntità=" & Request.Params("IdVol")
                MyCommand = ClsServer.EseguiSqlClient(strSQL, Session("conn"))


                'chi fa capanna ? Salvo il valore nell'idden dopo il salvataggio dei dati per uteriori controlli

                HdControlloIdFascicolo.Value = TxtCodiceFascicolo.Text

                '--- aggiunto da Michele d'Ascenzio
                'ScriviCronogliaEntitaDettagli()

                If Request.QueryString("Associa") = "True" Then
                    If Not IsNothing(Request.QueryString("idattivitaSedeAssegnazione")) Then
                        'Madificato da Alessandra Taballione il 25.10.2004
                        'Se provengo dalla Graduatoria oltre all'inserimento del volontario
                        'effettuo l'inserimento su AttivitàEntità per l'assegnazione del volontario alla sedeattuazione
                        strSQL = "insert into GraduatorieEntità (idEntità, idattivitàsedeassegnazione,Ordine,ammesso,stato,IdTipologiaPosto,Username,DataModifica,Punteggio)  " & _
                        " (Select " & Request.Params("IdVol") & "," & Request.QueryString("idattivitaSedeAssegnazione") & ",(select case isnull(max(ordine),-1) when -1 then 1 else max(ordine)+1  end from GraduatorieEntità where idattivitàsedeassegnazione=" & Request.QueryString("idattivitaSedeAssegnazione") & ")" & _
                        " ,0," & idoneoSiNo & ",(select IdTipologiaPosto from TipologiePosto where NoVittoNoAlloggio=1),'" & Session("Utente") & "',GetDate(),'" & sPunteggio & "' from entità where idEntità=" & Request.Params("IdVol") & ") "
                        MyCommand = ClsServer.EseguiSqlClient(strSQL, Session("conn"))


                        cmdSalva.Visible = True

                        Response.Redirect("WfrmVolontari.aspx?idattivitaSedeAssegnazione=" & lblidattivitasedeassegnazione.Value & "&IdEnteSede=" & lblidEntesede.Value & "&presenta=" & lblpresenta.Value & "&IdAttivita=" & lblidAttivita.Value & "&Disabilita=OK")
                    End If
                End If

                strSQL = "UPDATE GraduatorieEntità "
                strSQL = strSQL & "SET Punteggio='" & sPunteggio & "',Stato='" & idoneoSiNo & "'"
                strSQL = strSQL & "where identità=" & CInt(Request.Params("IdVol"))

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                MyCommand = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                lblConferma.Text = "L'aggiornamento è stato eseguito con successo." & ErrCodFisc

                sPunteggio = ""
            Else                                            'INSERMENTO

                'cerco lo stato di default dell'entità
                Dim intStatoEntita As Integer
                strSQL = "select IdStatoEntità from StatiEntità where defaultStato=1"
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                dtrgenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    intStatoEntita = CInt(dtrgenerico("IdStatoEntità"))
                End If
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                strCodiceProgetto = RicavaCodiceProgetto(lblidAttivita.Value)


                ' banca, cc, cab, abi, cin,IdUfficioPostale
                ' " & sUffPostale & ",sCab & "','" & sAbi & "','" & sCin & "'," &'" & sBanca & "','" & sCC & "',
                'aggiunti campi fascicolo
                strSQL = "Insert Into entità (Cognome,Nome,Sesso,Indirizzo, DettaglioRecapitoResidenza, FlagIndirizzoValidoRes,CAP,NumeroCivico,IDComuneNascita,Telefono,Cellulare,Email," & _
                        "Fax, DataNascita, IDComuneResidenza, CodiceFiscale, IdTipoStatoCivile,CodiceFiscaleConiuge, IDComuneDomicilio, IndirizzoDomicilio, DettaglioRecapitoDomicilio, FlagIndirizzoValidoDom," & _
                        "NumeroCivicoDomicilio,CAPDomicilio, TelefonoDomicilio, EmailDomicilio, TitoloStudio, IstitutoStudio, " & _
                        "NumAnnoScuolaMedia, IstitutoMediaSuperiore, NumAnnoAccademico, TitoloLaurea, AltriTitoli, Corsi, Esperienze, " & _
                        "AltreConoscenze, MotiviSceltaProgetto, AltreInformazioni,  DisponibileStessoProg, DisponibileAltriProg," & _
                        "CodiceLibrettoPostale, IBAN, BIC_SWIFT, IDStatoEntità, UserNameStato, DataUltimoStato, TMPCodiceProgetto, " & _
                        "TMPIdSedeAttuazione,TMPIdSedeAttuazioneSecondaria, IdSedePrimaAssegnazione, RitornoMittente, CodiceFascicolo, IDFascicolo, DescrFascicolo,IdCategoriaEntità,IdTitoloStudioConseguimento,GMO,FAMI, " & _
                        "AnomaliaCF, IDStatiVerificaCFEntità) " & _
                        "VALUES ('" & sCognome & "', '" & sNome & "', '" & sSesso & "', '" & sIndirizzo1 & "','" & sDettaglioResidenza & "'," & bandiera & ", '" & NCap1 & "','" & NCivic1 & _
                        "'," & idComuneNasc & ", '" & sTele1 & "','" & sCell1 & "','" & sEmail1 & "','" & sFax1 & "', Convert(DateTime,'" & _
                        sDataNasc & "',103)," & idComuneRes & ",'" & sCodiceFiscale & "','" & sStatoCivile & "', '" & sCodiceFiscaleConiuge & "' ," & IIf(idComuneDom = "", "null", idComuneDom) & _
                        ",'" & sIndirizzo2 & "','" & sDettaglioDomicilio & "'," & BandieraDomicilio & ", '" & NCivic2 & "','" & NCap2 & "','" & sTele2 & "', '" & sEmail2 & "','" & sTitoloStud & "', '" & sConseguitoIn & _
                        "', " & IIf(sAnnoIscr1 = "", "null", sAnnoIscr1) & ", '" & sScuolaSuperiore & "', " & IIf(sAnnoIscr2 = "", "null", sAnnoIscr2) & ",'" & _
                        sCorsoLaureaIn & "', '" & sAltriTitoli & "', '" & sCorsi & "', '" & sEsperienze & "','" & sConoscenze & "', '" & sMotivi & "', '" & _
                        sAltreinfo & "'," & sDisp1 & "," & sDisp2 & "," & sCodiceLibretto & ", " & sIban & ", " & sBicSwift & _
                        "," & intStatoEntita & ",'" & Session("Utente") & "',GetDate(),'" & strCodiceProgetto & _
                        "','" & CboSedeAtt.SelectedItem.Text & "', " & IIf(CboSedeAttSecondaria.SelectedItem.Text = "", "null", CboSedeAttSecondaria.SelectedItem.Text) & ", '" & CboSedeAtt.SelectedItem.Text & "', " & IIf(chkIndirizzoErrato.Checked = True, 1, 0) & _
                        ",'" & TxtCodiceFascicolo.Text & "', '" & TxtIdFascicolo.Value & "', '" & Replace(txtDescFasc.Text, "'", "''") & "','" & cboCategoria.SelectedValue & "','" & CboConseguimentoTS.SelectedValue & "'"

                If ddlGMO.SelectedValue = "" Then
                    strSQL &= " ,  NULL "
                Else
                    strSQL &= " , '" & ddlGMO.SelectedValue & "' "
                End If
                If ddlFAMI.SelectedValue = "" Then
                    strSQL &= " , NULL "
                Else
                    strSQL &= " ,'" & ddlFAMI.SelectedValue & "' "
                End If


                strSQL &= ", " & IIf(cboStatoVerificaCF.SelectedIndex > 0, 1, 0)
                strSQL &= ", " & IIf(cboStatoVerificaCF.SelectedIndex > 0, cboStatoVerificaCF.SelectedValue, " NULL")
                strSQL &= ")"


                MyCommand = New SqlCommand(strSQL, Session("conn"))

                'chi fa capanna ? Salvo il valore nell'idden dopo il salvataggio dei dati per uteriori controlli
                HdControlloIdFascicolo.Value = TxtCodiceFascicolo.Text


                If MyCommand.ExecuteNonQuery >= 1 Then
                    lblConferma.Text = "L'inserimento è stato eseguito con successo." & ErrCodFisc
                    PulisciDati()
                End If

                MyCommand.Dispose()
                iMaxId = MAXID(MyCommand)
                If Not IsNothing(Request.QueryString("idattivitaSedeAssegnazione")) Then
                    'Madificato da Alessandra Taballione il 25.10.2004
                    'Se provengo dalla Graduatoria oltre all'inserimento del volontario
                    'effettuo l'inserimento su AttivitàEntità per l'assegnazione del volontario alla sedeattuazione
                    strSQL = "insert into GraduatorieEntità (idEntità, idattivitàsedeassegnazione,Ordine,ammesso,stato,IdTipologiaPosto,Username,DataModifica,Punteggio)  " & _
                    "(Select @@identity as maxid," & Request.QueryString("idattivitaSedeAssegnazione") & ",(select case isnull(max(ordine),-1) when -1 then 1 else max(ordine)+1  end from GraduatorieEntità where idattivitàsedeassegnazione=" & Request.QueryString("idattivitaSedeAssegnazione") & ")" & _
                    " ,0," & idoneoSiNo & ",(select IdTipologiaPosto from TipologiePosto where NoVittoNoAlloggio=1),'" & Session("Utente") & "',GetDate(),'" & sPunteggio & "' from entità where idEntità=@@identity) "
                    MyCommand = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                    '''''''''''''''''DisabilitaControlli()
                    cmdSalva.Visible = True
                End If
                cmdnuovo.Visible = True


                strSQL = "SELECT bando.LOTUS "
                strSQL = strSQL & "FROM AttivitàSediAssegnazione "
                strSQL = strSQL & "INNER JOIN attività ON AttivitàSediAssegnazione.IDAttività = attività.IDAttività "
                strSQL = strSQL & "INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività "
                strSQL = strSQL & "INNER JOIN bando ON BandiAttività.IdBando = bando.IDBando "
                strSQL = strSQL & "WHERE attivitàsediassegnazione.IDAttivitàSedeAssegnazione='" & Request.QueryString("idattivitaSedeAssegnazione") & "'"

                Dim dtrLocal As SqlClient.SqlDataReader
                Dim myCommand2 As SqlClient.SqlCommand

                myCommand2 = New SqlClient.SqlCommand

                myCommand2.Connection = Session("conn")

                myCommand2.CommandText = strSQL

                dtrgenerico = myCommand2.ExecuteReader

                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    'se è un progetto a bando LOTUS allora vado direttamente alla schermata
                    If dtrgenerico("LOTUS") = True Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                        Response.Redirect("WFrmVolontarioProgetto.aspx?Id=" & iMaxId)
                    End If
                End If
                dtrgenerico.Close()
                dtrgenerico = Nothing
                sPunteggio = ""
            End If
        Catch ex As Exception
            lblErr.Text = "Contattare l'assistenza Garanzia Giovani"
            Throw (ex)
        End Try
        txtCodLibPost.Value = txtLibretto.Text
        Session("IdComune") = Nothing
        'imgAnomaliaCF.Visible = False
    End Sub
    Private Function VerificaCampiObbligatori() As Boolean
        Dim utility As ClsUtility = New ClsUtility()
        Dim data As Date
        Dim idTipoProgetto As String = utility.TipologiaProgettoDaIdAttivita(Request.QueryString("IdAttivita"), Session("conn"))
        Dim campiValidi As Boolean
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"
        Dim campoObbligatorioGaranziaGiovani As String = "Il campo {0} è obbligatorio per i volontari Garanzia Giovani .<br/>"
        Dim campoObbligatorioHelios As String = "Il campo {0} è obbligatorio per i volontari del Servizio Civile Nazionale .<br/>"
        If (txtCognome.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Cognome")
            campiValidi = False
        End If
        If (txtNome.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Nome")
            campiValidi = False
        End If

        If (txtCodiceFiscale.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Codice Fiscale")
            campiValidi = False
        End If
        If (txtDataNascita.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Data di nascita")
            campiValidi = False
        Else
            campiValidi = ValidaData(txtDataNascita.Text, "Data di nascita")
        End If
        If (ddlProvinciaNascita.SelectedItem.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Provincia/Nazione di nascita")
            campiValidi = False
        End If
        If (ddlComuneNascita.SelectedItem.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Comune di nascita")
            campiValidi = False
        End If

        If (ddlProvinciaResidenza.SelectedItem.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Provincia/Nazione di residenza")
            campiValidi = False
        End If
        If (ddlComuneResidenza.SelectedItem.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Comune di residenza")
            campiValidi = False
        End If
        If (txtIndirizzo.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Indirizzo residenza")
            campiValidi = False
        End If
        If (txtCivico.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "Numero Civico")
            campiValidi = False
        End If
        If (txtCAP.Text = String.Empty) Then
            lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "C.A.P.")
            campiValidi = False
        End If
        If (TxtDataDomanda.Text <> String.Empty) Then
            Dim dataValida = ValidaData(TxtDataDomanda.Text, "Data Domanda")
            If (dataValida = False) Then
                campiValidi = dataValida
            End If
        End If

        If StatoCivileEsistente = True Or Request.Params("IdVol") = Nothing Then
            If (cboStatoCivile.SelectedItem.Text = String.Empty) Then
                lblErr.Text = lblErr.Text + String.Format(campoObbligatorio, "StatoCivile")
                campiValidi = False


            End If


        End If
        If (idTipoProgetto = PROGETTO_GARANZIA_GIOVANI) Then
            If (ddlNazionalita.SelectedItem.Text = String.Empty) Then
                lblErr.Text = lblErr.Text + String.Format(campoObbligatorioGaranziaGiovani, "Nazionalità")
                campiValidi = False
            End If
            If (Request.Params("IdVol") <> "" And TxtDataDomanda.Text = String.Empty) Then
                lblErr.Text = lblErr.Text + String.Format(campoObbligatorioGaranziaGiovani, "Data Domanda")
                campiValidi = False
            End If
        Else
            If (cboCategoria.SelectedItem.Text = String.Empty) Then
                lblErr.Text = lblErr.Text + String.Format(campoObbligatorioHelios, "Categoria")
                campiValidi = False
            End If
        End If
        Return campiValidi
    End Function
    Private Function ValidaData(ByVal data As String, ByVal nomeCampo As String) As Boolean
        Dim dataTmp As Date
        Dim dataValida As Boolean = True
        Dim messaggioDataValida As String = "Il valore di '{0}' non è valido. Inserire la data nel formato gg/mm/aaaa.<br/>"

        If (Date.TryParse(data, dataTmp) = False) Then
            lblErr.Text = lblErr.Text + String.Format(messaggioDataValida, nomeCampo)
            dataValida = False
        End If
        Return dataValida

    End Function
    Protected Sub cmdnuovo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdnuovo.Click
        Response.Redirect("WfrmVolontari.aspx?idattivitaSedeAssegnazione=" & lblidattivitasedeassegnazione.Value & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&presenta=" & Request.QueryString("presenta") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&Disabilita=OK")
    End Sub
    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdChiudi.Click

        'se vengo dalle stampe degli attestati, torno alla ricerca delle stampe degli asttestati
        If Not IsNothing(Request.QueryString("vengoda")) Then
            If Request.QueryString("vengoda") = "stampe" Then
                Response.Redirect("WfrmRicercaVolontariStampa.aspx")
            End If
            If Request.QueryString("vengoda") = "attestati" Then
                Response.Redirect("WfrmRicercaVolontariStati.aspx")
            End If
        End If

        'Modificato da Alessandra Tballione il 26/10/2004
        'RIchamo la pagina dell'associa Volontari aggiornata
        If Not IsNothing(Request.QueryString("IdEnteSede")) Then
            If Request.QueryString("IdEnteSede") <> "" Then
                Response.Redirect("WfrmAssociaVolontari.aspx?IdEnteSede=" & Request.QueryString("IdEnteSede") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&presenta=" & Request.QueryString("presenta") & "&idattivitaSedeAssegnazione=" & Request.QueryString("idattivitaSedeAssegnazione") & "")
            Else
                If Request.Params("Ente") = "OK" Then
                    Response.Redirect("WfrmRicercaVolontari.aspx?Ente=OK")
                Else
                    Response.Redirect("WfrmRicercaVolontari.aspx")
                End If
            End If
        Else
            If Request.Params("Ente") = "OK" Then
                Response.Redirect("WfrmRicercaVolontari.aspx?Ente=OK")
            Else
                Response.Redirect("WfrmRicercaVolontari.aspx")
            End If
        End If
        Session("IdComune") = Nothing
    End Sub
    Private Sub CaricaComboProvinciaNazione(ByVal RichiamoProvincia As String, ByVal blnEsetero As Boolean)
        Dim SelComune As New clsSelezionaComune
        Select Case RichiamoProvincia
            Case "NASCITA"
                ddlProvinciaNascita = SelComune.CaricaProvinciaNazione(ddlProvinciaNascita, blnEsetero, Session("Conn"))
            Case "RESIDENZA"
                ddlProvinciaResidenza = SelComune.CaricaProvinciaNazione(ddlProvinciaResidenza, blnEsetero, Session("Conn"))
            Case "DOMICILIO"
                ddlProvinciaDomicilio = SelComune.CaricaProvinciaNazione(ddlProvinciaDomicilio, blnEsetero, Session("Conn"))
        End Select
    End Sub
    Private Sub ChkEsteroNascita_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEsteroNascita.CheckedChanged
        CaricaComboProvinciaNazione("NASCITA", ChkEsteroNascita.Checked)
        ddlComuneNascita.DataSource = Nothing
        ddlComuneNascita.Items.Add("")
        ddlComuneNascita.SelectedIndex = 0
    End Sub
    Private Sub ChkEsteroResidenza_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEsteroResidenza.CheckedChanged
        CaricaComboProvinciaNazione("RESIDENZA", ChkEsteroResidenza.Checked)
        ddlComuneResidenza.DataSource = Nothing
        ddlComuneResidenza.Items.Add("")
        ddlComuneResidenza.SelectedIndex = 0
    End Sub
    Private Sub ChkEsteroDomicilio_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEsteroDomicilio.CheckedChanged
        CaricaComboProvinciaNazione("DOMICILIO", ChkEsteroDomicilio.Checked)
        ddlComuneDomicilio.DataSource = Nothing
        ddlComuneDomicilio.Items.Add("")
        ddlComuneDomicilio.SelectedIndex = 0
    End Sub
    Private Sub ddlProvinciaNascita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaNascita.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        ddlComuneNascita.Enabled = True
        ddlComuneNascita = SelComune.CaricaComuniNascita(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
    End Sub
    Private Sub ddlProvinciaResidenza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaResidenza.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        ddlComuneResidenza.Enabled = True
        ddlComuneResidenza = SelComune.CaricaComuni(ddlComuneResidenza, ddlProvinciaResidenza.SelectedValue, Session("Conn"))
    End Sub
    Private Sub ddlProvinciaDomicilio_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaDomicilio.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        ddlComuneDomicilio.Enabled = True
        ddlComuneDomicilio = SelComune.CaricaComuni(ddlComuneDomicilio, ddlProvinciaDomicilio.SelectedValue, Session("Conn"))
    End Sub
    Protected Sub imgCap_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCap.Click
        lblErr.Text = ""
        Dim selCap As New clsSelezionaComune
        If ddlComuneResidenza.SelectedValue = String.Empty And txtCivico.Text = String.Empty And txtIndirizzo.Text = String.Empty Then
            lblErr.Text = "Per ottenere il C.A.P. della residenza è necessario indicare almeno il comune e l'indirizzo di residenza."
        Else
            txtCAP.Text = selCap.RitornaCap(ddlComuneResidenza.SelectedValue, txtIndirizzo.Text, txtCivico.Text, Session("conn"))
        End If
    End Sub
    Protected Sub imgCapDom_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCapDom.Click
        lblErr.Text = ""
        Dim selCap As New clsSelezionaComune
        If ddlComuneDomicilio.SelectedValue = String.Empty And txtCivicoDomicilio.Text = String.Empty And txtIndirizzoDomicilio.Text = String.Empty Then
            lblErr.Text = "Per ottenere il C.A.P. del domicilio è necessario indicare almeno il comune e l'indirizzo del domicilio."
        Else
            txtCapDomicilio.Text = selCap.RitornaCap(ddlComuneDomicilio.SelectedValue, txtIndirizzoDomicilio.Text, txtCivicoDomicilio.Text, Session("conn"))
        End If

    End Sub
    Protected Sub cmdInsNote_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdInsNote.Click
        If cmdInsNote.Text = "Inserisci Nota" Then
            GestioneNote_RigheNascoste.Visible = True
            'CaricaGrigliaNote(dtgStoricoNote)  'ADC
            cmdInsNote.Text = "Chiudi Nota"
        Else
            GestioneNote_RigheNascoste.Visible = False
            cmdInsNote.Text = "Inserisci Nota"
        End If
       
    End Sub
    Private Sub CaricaGrigliaNote(ByRef GridDaCaricare As DataGrid)
        StrSql = "Select * from CronologiaNoteEntità where IDEntità ='" & Session("IdVol") & "' Order by IDCronologiaNoteEntità desc"
        GridDaCaricare.DataSource = ClsServer.DataSetGenerico(StrSql, Session("conn"))
        GridDaCaricare.DataBind()
        GridDaCaricare.Visible = True
    End Sub
    Protected Sub cmdSalvaNota_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSalvaNota.Click
        If txtNuovaNota.Text.Trim <> "" Then
            StrSql = "Insert Into CronologiaNoteEntità (IDEntità,NoteEntità,DataNota,UserNameNota) Values ('" & Session("IdVol") & "','" & ClsServer.NoApice(txtNuovaNota.Text) & "', getdate(),'" & Session("Utente") & "')"
            Dim cmdNote As New SqlClient.SqlCommand
            cmdNote.CommandText = StrSql
            cmdNote.Connection = Session("Conn")
            cmdNote.ExecuteNonQuery()


            StrSql = "Update entità SET AltreInformazioni='" & ClsServer.NoApice(txtNuovaNota.Text) & "'"
            StrSql = StrSql & "  where  IDEntità='" & Session("IdVol") & "'"

            cmdNote.CommandText = StrSql
            cmdNote.Connection = Session("Conn")
            cmdNote.ExecuteNonQuery()

            CaricaGrigliaNote(dtgStoricoNote)
            txtAltreInformazioni.Text = txtNuovaNota.Text
            txtNuovaNota.Text = ""
            cmdInsNote.Text = "Inserisci Nota"
        End If
    End Sub
    Private Sub dtgStoricoNote_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgStoricoNote.ItemCommand
        Dim strUltimaNota As String
        Dim cmdGenerico As SqlClient.SqlCommand
        GestioneNote_RigheNascoste.Visible = True
        If e.CommandName = "Rimuovi" Then
            StrSql = "delete CronologiaNoteEntità where idCronologiaNoteEntità='" & e.Item.Cells(0).Text & "'"
            cmdGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))

            StrSql = "SELECT TOP 1 isnull(NoteEntità,'') AS NoteEntità FROM CronologiaNoteEntità  WHERE IDEntità='" & Session("IdVol") & "' ORDER BY DataNota DESC"
            dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                strUltimaNota = dtrgenerico("NoteEntità")

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

                'UPDATE CAMPO altreinformazioni su entità
                StrSql = "Update entità SET AltreInformazioni='" & ClsServer.NoApice(strUltimaNota) & "'"
                StrSql = StrSql & "  where  IDEntità='" & Session("IdVol") & "'"
                cmdGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))

                'strUltimoValore delle note
                'Response.Write("<input type=hidden id=strUltimoValore name=strUltimoValore value=""" & strUltimaNota & """>")
            Else
                'UPDATE A NULL CAMPO altreinformazioni su entità

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

                StrSql = "Update entità SET AltreInformazioni=NULL"
                StrSql = StrSql & "  where  IDEntità='" & Session("IdVol") & "'"
                cmdGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))

                'strUltimoValore delle note
                'Response.Write("<input type=hidden id=strUltimoValore name=strUltimoValore value="""">")
            End If
            dtgStoricoNote.CurrentPageIndex = 0
            CaricaGrigliaNote(dtgStoricoNote)
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Private Sub dtgStoricoNote_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgStoricoNote.PageIndexChanged
        GestioneNote_RigheNascoste.Visible = True
        dtgStoricoNote.SelectedIndex = -1
        dtgStoricoNote.EditItemIndex = -1
        dtgStoricoNote.CurrentPageIndex = e.NewPageIndex
        CaricaGrigliaNote(dtgStoricoNote)
    End Sub
    Protected Sub hlDocumentazione_Click(ByVal sender As Object, ByVal e As EventArgs) Handles hlDocumentazione.Click
        Response.Redirect("WfrmElencoDocumentazioneVolontario.aspx?CorrCodUnivoco=" + txtCodicevolontario.Text + "&VengoDA=SchedaVolontario&idattivita=" + Request.QueryString("idattivita") + "&VArUpdate=1&cancellaprotocolli=si&CodiceFiscale=" + txtCodiceFiscale.Text + "&IdVol=" + Request.QueryString("IdVol") + "&NumeroFascicolo=" + TxtIdFascicolo.Value + "&CodiceFascicolo=" + TxtCodiceFascicolo.Text + "&DescFascicolo=" + txtDescFasc.Text)
    End Sub
    Protected Sub cmdFascCanc_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCanc.Click
        TxtCodiceFascicolo.Text = String.Empty
        HdControlloIdFascicolo.Value = String.Empty
        TxtIdFascicolo.Value = String.Empty
        txtDescFasc.Text = String.Empty
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
    Protected Sub ddlComuneNascita_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlComuneNascita.SelectedIndexChanged
        lblidComuneNascita.Value = ddlComuneNascita.SelectedValue
    End Sub
    Protected Sub hlVisualizzaDocVolontario_Click(sender As Object, e As EventArgs) Handles hlVisualizzaDocVolontario.Click
        Response.Redirect("WfrmVisualizzaElencoDocumentiVolontario.aspx?IdVol=" + Request.QueryString("IdVol") + "&IdAttivita=" & Request.QueryString("IdAttivita"))
    End Sub
    Private Function AbilitaGestioneDocumentiVolontario(ByVal Utente As String, ByVal IdVolontario As Integer) As Boolean
        'Agg da Simona Cordella il 31/03/2015
        'Verifico se l'utente U è autorizzato alla visualizzazione del menu di Gestione Documenti Volontari
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'verifico lo stato del volontario (per vedere i documenti non deve essere registrato)
        strSql = "Select * from Entità where IdEntità = " & IdVolontario & " and idstatoentità<>1"

        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            strSql = " SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                     " VociMenu.IdVoceMenuPadre" & _
                     " FROM VociMenu " & _
                     " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                     " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                     " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                     " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                     " WHERE VociMenu.descrizione = 'Gestione Documenti Volontari'" & _
                     " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
            dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

            If dtrgenerico.HasRows = True Then
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                hlVisualizzaDocVolontario.Visible = True
            End If
        Else
            hlVisualizzaDocVolontario.Visible = False
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Function
    Private Function RicavaCodiceProgetto(ByVal idAttivita As Integer) As String
        'AGGIUNTO DA SIMONA CORDELLA IL 03/08/2018
        'RICAVO IL CODICE PROGETTO 
        'FUNCTION UTILIZZATA PER L'INSERIMENTO DEL VOLONTARIO

        'lblidAttivita.Value
        Dim strCodiceProgetto As String

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        StrSql = "SELECT CodiceEnte FROM attività WHERE IDAttività= " & idAttivita
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            strCodiceProgetto = dtrgenerico("CodiceEnte")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return strCodiceProgetto
    End Function
    Private Function ControlloIndicazioneSedeSecondaria(ByVal IdEntita As Integer) As Boolean
        'AGGIUNTO DA SIMONA CORDELLA IL 08/08/2018
        'CONTROLLO SE IN TABELLA E' STATO PRESENTE IL CODICESEDESECONDARIO ( TMPIdSedeAttuazioneSecondaria)

        Dim blnSedeSecondaria As Boolean

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        StrSql = "SELECT * FROM entità  WHERE IdEntità= " & IdEntita & " and ISNULL(TMPIdSedeAttuazioneSecondaria,'')<>'' "
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        blnSedeSecondaria = dtrgenerico.HasRows
 
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        Return blnSedeSecondaria
    End Function
    Private Sub hplInfoPaghe_Click(sender As Object, e As System.EventArgs) Handles hplInfoPaghe.Click

        Response.Redirect("WfrmCOMPInfoPaghe.aspx?IdVol=" + Request.QueryString("IdVol") + "&IdAttivita=" & Request.QueryString("IdAttivita"))
    End Sub
    Private Sub AbilitaVerificaStatoCodiceFiscale()

        'Verifica se l'uetnte puo modificare il libretto postale
        StrSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu" & _
                 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"

        If Session("Read") <> "1" Then
            StrSql += " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            StrSql += " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        StrSql += " LEFT JOIN RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                  " WHERE VociMenu.descrizione = 'Forza Codice Fiscale'" & _
                  " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        cboStatoVerificaCF.Enabled = dtrgenerico.HasRows
        dtrgenerico.Close()
        dtrgenerico = Nothing

    End Sub

    Protected Sub cmdStoricoNote_Click(sender As Object, e As EventArgs) Handles cmdStoricoNote.Click
        If cmdStoricoNote.Text = "Visualizza Note" Then
            gridSoricoNote.Visible = True
            CaricaGrigliaNote(dtgStoricoNote)
            dtgStoricoNote.Visible = True
            cmdStoricoNote.Text = "Nascondi Note"
        Else
            dtgStoricoNote.Visible = False
            cmdStoricoNote.Text = "Visualizza Note"
            gridSoricoNote.Visible = False
        End If
    End Sub
End Class