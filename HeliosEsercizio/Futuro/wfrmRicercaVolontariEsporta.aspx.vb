Imports System.IO

Public Class wfrmRicercaVolontariEsporta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        lblTitolo.Text = "Ricerca Volontari da Esportare"

        If IsPostBack = False Then
            If Session("TipoUtente") = "E" Then
                DivEnte.Visible = False
            End If
            txtCodEnte.Text = Session("txtCodEnte")
        End If

    End Sub

    Sub CaricaGriglia()
        Dim strSql As String
        Dim strWhere As String
        Dim MyDataSet As DataSet
        'DESCRIZIONE: routine che carica la griglia con tutti i progetti con stato attività=1
        'AUTORE: Michele d'Ascenzio    
        'DATA: 03/11/2004
        Try
            strSql = "SELECT distinct enti.CodiceRegione, " & _
                     "entità.CodiceVolontario, " & _
                     "entità.DataInizioServizio, " & _
                     "entità.DataFineServizio, " & _
                     "entità.Cognome, " & _
                     "entità.Nome, " & _
                     "entità.Sesso, " & _
                     "comuni.Denominazione AS ComuneNascita, " & _
                     "entità.DataNascita, " & _
                     "comuni_2.Denominazione AS ComuneResidenza, " & _
                     "provincie_2.Provincia AS ProvinciaResidenza, " & _
                     "regioni_2.Regione AS RegioneResidenza, " & _
                     "entità.Indirizzo, " & _
                     "entità.NumeroCivico, " & _
                     "entità.CAP, " & _
                     "enti.CodiceRegione AS CodiceEnte, " & _
                     "enti.Denominazione, " & _
                     "attività.CodiceEnte AS CodiceProgetto, " & _
                     "attività.Titolo, " & _
                     "comuni_1.Denominazione AS ComuneSedeProgetto, " & _
                     "provincie_1.Provincia AS ProvinciaSedeProgetto, " & _
                     "regioni_1.Regione AS RegioneSedeProgetto, " & _
                     "entisedi.Indirizzo AS IndirizzoSedeProgetto, " & _
                     "entisedi.Civico, " & _
                     "entisedi.CAP AS CapSedeProgetto " & _
                     "FROM entità " & _
                     "INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                     "INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                     "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                     "INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
                     "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede " & _
                     "INNER JOIN comuni ON entità.IDComuneNascita = comuni.IDComune " & _
                     "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " & _
                     "INNER JOIN comuni comuni_1 ON entisedi.IDComune = comuni_1.IDComune " & _
                     "INNER JOIN provincie provincie_1 ON comuni_1.IDProvincia = provincie_1.IDProvincia " & _
                     "INNER JOIN comuni comuni_2 ON entità.IDComuneResidenza = comuni_2.IDComune " & _
                     "INNER JOIN provincie provincie_2 ON comuni_2.IDProvincia = provincie_2.IDProvincia " & _
                     "INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
                     "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione " & _
                     "INNER JOIN regioni regioni_1 ON provincie_1.IDRegione = regioni_1.IDRegione " & _
                     "INNER JOIN regioni regioni_2 ON provincie_2.IDRegione = regioni_2.IDRegione " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = Statientità.IdStatoEntità " & _
                     "INNER JOIN TIPIPROGETTO ON attività.idtipoprogetto=TIPIPROGETTO.idtipoprogetto " & _
                     "WHERE StatiEntità.InServizio = 1  and  entità.DataInizioServizio<getdate()  "
            If Session("TipoUtente") = "E" Then
                strSql = strSql & "AND Enti.IdEnte = " & Session("IdEnte") & " "
            End If
            If txtEnte.Text <> vbNullString Then
                strSql = strSql & "AND Enti.Denominazione like '" & Replace(txtEnte.Text, "'", "''") & "%' "
            End If
            If txtCodEnte.Text <> vbNullString Then
                strSql = strSql & "AND Enti.CodiceRegione like '" & Replace(txtCodEnte.Text, "'", "''") & "%' "
            End If
            If txtCognome.Text <> vbNullString Then
                strSql = strSql & "AND Entità.Cognome like '" & Replace(txtCognome.Text, "'", "''") & "%' "
            End If
            If txtNome.Text <> vbNullString Then
                strSql = strSql & "AND Entità.Nome like '" & Replace(txtNome.Text, "'", "''") & "%' "
            End If
            If txtProgetto.Text <> vbNullString Then
                strSql = strSql & "AND Attività.Titolo like '" & Replace(txtProgetto.Text, "'", "''") & "%' "
            End If
            If txtCodProgetto.Text <> vbNullString Then
                strSql = strSql & "AND Attività.CodiceEnte like '" & Replace(txtCodProgetto.Text, "'", "''") & "%'"
            End If
            If Session("FiltroVisibilita") <> Nothing Then
                strSql = strSql & " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            End If
            MyDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))

            'ANTONELLO PROVA BIN
           

            'assegno il dataset alla griglia del risultato
            dgVolontari.DataSource = MyDataSet
            Session("appDtsRisRicerca") = MyDataSet
            dgVolontari.DataBind()
            dgVolontari.Visible = True


            If dgVolontari.Items.Count = 0 Then
                CmdEsporta.Visible = False
                lblmessaggio.Text = "La ricerca non ha prodotto risultati."
            Else
                CmdEsporta.Visible = True
                lblmessaggio.Text = "Risultato " & lblTitolo.Text
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub dgVolontari_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgVolontari.PageIndexChanged
        'AUTORE: MIchele d'Ascenzio
        'DATA: 03/11/2004
        'Cambia pag della Griglia
        dgVolontari.CurrentPageIndex = e.NewPageIndex
        dgVolontari.DataSource = Session("appDtsRisRicerca")
        dgVolontari.DataBind()
        dgVolontari.SelectedIndex = -1
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        CaricaGriglia()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'chiamo homepage
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click

        EsportaVolontari()
      
    End Sub

    Private Sub EsportaVolontari()
        Dim StrSql As String
        Dim dtrVolontari As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try

            StrSql = "SELECT distinct enti.CodiceRegione, " & _
                     "entità.CodiceVolontario, " & _
                     "entità.DataInizioServizio, " & _
                     "entità.DataFineServizio, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Cognome, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Nome, " & _
                     "entità.Sesso, " & _
                     "entità.CodiceFiscale as CodiceFiscale, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(comuni.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ComuneNascita, " & _
                     "entità.DataNascita, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(comuni_2.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ComuneResidenza, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(provincie_2.Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ProvinciaResidenza, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(regioni_2.Regione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS RegioneResidenza, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Indirizzo, " & _
                     "entità.NumeroCivico, " & _
                     "entità.CAP, " & _
                     "enti.CodiceRegione AS CodiceEnte, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(enti.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Denominazione, " & _
                     "attività.CodiceEnte AS CodiceProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(attività.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Titolo, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(comuni_1.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ComuneSedeProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(provincie_1.Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ProvinciaSedeProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(regioni_1.Regione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS RegioneSedeProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi.Indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS IndirizzoSedeProgetto, " & _
                     "entisedi.Civico, " & _
                     "entisedi.CAP AS CapSedeProgetto, " & _
                     "entisediattuazioni.identesedeattuazione AS CodiceSedeProgetto, " & _
                     "isnull(entità.Email,'') AS Email " & _
                     "FROM entità " & _
                     "INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                     "INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                     "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                     "INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
                     "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede " & _
                     "INNER JOIN comuni ON entità.IDComuneNascita = comuni.IDComune " & _
                     "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " & _
                     "INNER JOIN comuni comuni_1 ON entisedi.IDComune = comuni_1.IDComune " & _
                     "INNER JOIN provincie provincie_1 ON comuni_1.IDProvincia = provincie_1.IDProvincia " & _
                     "INNER JOIN comuni comuni_2 ON entità.IDComuneResidenza = comuni_2.IDComune " & _
                     "INNER JOIN provincie provincie_2 ON comuni_2.IDProvincia = provincie_2.IDProvincia " & _
                     "INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
                     "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione " & _
                     "INNER JOIN regioni regioni_1 ON provincie_1.IDRegione = regioni_1.IDRegione " & _
                     "INNER JOIN regioni regioni_2 ON provincie_2.IDRegione = regioni_2.IDRegione " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = Statientità.IdStatoEntità " & _
                     "INNER JOIN TIPIPROGETTO ON attività.idtipoprogetto=TIPIPROGETTO.idtipoprogetto " & _
                     "WHERE StatiEntità.InServizio = 1 and  entità.DataInizioServizio<getdate() "

            If Session("TipoUtente") = "E" Then
                StrSql = StrSql & "AND Enti.IdEnte = " & Session("IdEnte") & " "
            End If
            If txtEnte.Text <> vbNullString Then
                StrSql = StrSql & " and enti.denominazione like '" & txtEnte.Text.Replace("'", "''") & "%'"
            End If
            If txtCodEnte.Text <> vbNullString Then
                StrSql = StrSql & " and enti.CodiceRegione = '" & txtCodEnte.Text.Replace("'", "''") & "'"
            End If
            If txtCognome.Text <> vbNullString Then
                StrSql = StrSql & " and Entità.Cognome LIKE '" & txtCognome.Text.Replace("'", "''") & "'"
            End If
            If txtNome.Text <> vbNullString Then
                StrSql = StrSql & " and  Entità.Nome LIKE '" & txtNome.Text.Replace("'", "''") & "'"
            End If
            If txtProgetto.Text <> vbNullString Then
                StrSql = StrSql & " and Attività.Titolo LIKE '" & txtProgetto.Text.Replace("'", "''") & "%'"
            End If
            If txtCodProgetto.Text <> vbNullString Then
                StrSql = StrSql & " and Attività.CodiceEnte LIKE '" & txtCodProgetto.Text.Replace("'", "''") & "%'"
            End If
            If Session("FiltroVisibilita") <> Nothing Then
                StrSql = StrSql & " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            End If
            dtrVolontari = ClsServer.CreaDatareader(StrSql, Session("conn"))

            NomeUnivoco = vbNullString

            If dtrVolontari.HasRows = False Then
                lblmessaggio.Text = "Nessun Volontario estratto."
            Else
                While dtrVolontari.Read
                    If NomeUnivoco = vbNullString Then
                        xPrefissoNome = Session("Utente")
                        NomeUnivoco = xPrefissoNome & "ExpVol" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        '---intestazioni
                        xLinea = "CodiceVolontario;DataInizioServizio;DataFineServizio;Cognome;Nome;Sesso;CodiceFiscale;ComuneNascita;DataNascita;ComuneResidenza;ProvinciaResidenza;RegioneResidenza;Indirizzo;NumeroCivico;CAP;CodiceEnte;Denominazione;CodiceProgetto;Titolo;ComuneSedeProgetto;ProvinciaSedeProgetto;RegioneSedeProgetto;IndirizzoSedeProgetto;Civico;CAP;CodiceSedeProgetto;Email;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo elemento (nome file)
                    If IsDBNull(dtrVolontari(1)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(dtrVolontari(1)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(2)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(2)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(3)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(3)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(4)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(4)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(5)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(5)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(6)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        If dtrVolontari(6) = "0" Then
                            xLinea = xLinea & "M;"
                        Else
                            xLinea = xLinea & "F;"
                        End If
                    End If
                    If IsDBNull(dtrVolontari(7)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(7)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(8)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(8)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(9)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(9)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(10)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(10)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(11)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(11)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(12)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(12)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(13)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(13)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(14)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(14)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(15)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(15)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(16)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(16)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(17)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(17)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(18)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(18)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(19)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(19)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(20)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(20)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(21)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(21)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(22)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(22)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(23)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(23)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(24)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(24)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(25)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(25)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(26)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(26)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(27)) = True Then 'email
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(27)) & ";"
                    End If
                    Writer.WriteLine(xLinea)
                End While
                ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

                ApriCSV1.Visible = True
                Writer.Close()
                Writer = Nothing
            End If


            dtrVolontari.Close()
            dtrVolontari = Nothing



        Catch ex As Exception
            lblMessaggiErrore.Text =  "Errore durante l'esportazione dei Volontari."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not dtrVolontari Is Nothing Then
                dtrVolontari.Close()
                dtrVolontari = Nothing
            End If
        End Try
    End Sub

End Class