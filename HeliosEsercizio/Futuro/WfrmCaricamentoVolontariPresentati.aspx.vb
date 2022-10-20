Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient

Public Class WfrmCaricamentoVolontariPresentati
    Inherits System.Web.UI.Page
    Private strIDselezionati As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        'If Page.IsPostBack = False Then
        '    CaricaPrima()
        '    dgRisultatoRicerca.Visible = False
        'End If
    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        For Each riga As DataGridItem In dgRisultatoRicerca.Items
            CType(riga.FindControl("chkSelProg"), CheckBox).Checked = chkSelectAll.Checked
        Next
    End Sub

    Protected Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        Dim strSQL As String
        Dim dtsLocal As Data.DataSet

        strSQL = "SELECT IDAttività, CodiceProgetto, Titolo, Bando, SettAmb, VolontariConcessi, Competenza, VolontariPresentati " & _
                 "FROM VW_CARICAMENTO_VOLONTARI_PRESENTATI " & _
                 "WHERE IdEntePresentante='" & Session("IdEnte") & "' AND MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "

        If txtCodiceProgetto.Text <> "" Then strSQL &= "AND CodiceProgetto like '" & ClsServer.NoApice(Trim(txtCodiceProgetto.Text)) & "%' "

        If txtTitoloProgetto.Text <> "" Then strSQL &= "AND Titolo like '" & ClsServer.NoApice(Trim(txtTitoloProgetto.Text)) & "%' "

        If ddlMaccCodAmAtt.SelectedIndex > 0 Then
            strSQL &= "AND idmacroambitoattività=" & ddlMaccCodAmAtt.SelectedValue & " "
            If ddlCodAmAtt.SelectedIndex > 0 Then strSQL &= "AND idambitoattività=" & ddlCodAmAtt.SelectedValue & " "
        End If

        Select Case ddlTipo.SelectedValue
            Case 1
                strSQL &= "AND VolontariPresentati is null "
            Case 2
                strSQL &= "AND not VolontariPresentati is null "
        End Select

        strSQL &= "ORDER BY Titolo "

        dtsLocal = ClsServer.DataSetGenerico(strSQL, Session("conn"))

        Session("dtsRisultatoRicerca") = dtsLocal
        dgRisultatoRicerca.DataSource = dtsLocal
        dgRisultatoRicerca.DataBind() 'valorizzo griglia
        dgRisultatoRicerca.Visible = True
        If dgRisultatoRicerca.Items.Count = 0 Then
            lblmessaggio.Text = "Nessun Dato estratto."
        End If

        chkSelectAll.Visible = dgRisultatoRicerca.Items.Count > 0
        chkSelectAll.Checked = False
        cmdEsporta.Visible = dgRisultatoRicerca.Items.Count > 0
        dtsLocal.Dispose()
    End Sub

    Private Sub CaricaPrima()
        Dim dtrSettori As SqlClient.SqlDataReader
        Dim strSQL As String

        ChiudiDataReader(dtrSettori)
        strSQL = "SELECT 0 ID , '' Descrizione UNION " & _
                 "select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività"

        dtrSettori = ClsServer.CreaDatareader(strSQL, Session("conn"))
        If dtrSettori.HasRows = True Then
            ddlMaccCodAmAtt.DataSource = dtrSettori
            ddlMaccCodAmAtt.DataTextField = "Descrizione"
            ddlMaccCodAmAtt.DataValueField = "ID"
            ddlMaccCodAmAtt.DataBind()
        End If
        ChiudiDataReader(dtrSettori)

        '***Carico combo area intervento
        ddlCodAmAtt.Enabled = False

    End Sub

    Private Sub ddlMaccCodAmAtt_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlMaccCodAmAtt.SelectedIndexChanged
        Dim dtrSettori As SqlClient.SqlDataReader
        Dim strSQL As String

        If ddlMaccCodAmAtt.SelectedItem.Text <> "" Then
            ChiudiDataReader(dtrSettori)
            strSQL = "SELECT 0 ID , '' Descrizione UNION " & _
                     "SELECT distinct a.idambitoattività ," & _
                     " a.codifica + ' ' + a.AmbitoAttività as Ambito from ambitiattività a" & _
                     " inner join macroambitiattività b" & _
                     " on a.IDMacroAmbitoAttività=b.IDMacroAmbitoAttività" & _
                     " where a.IDMacroAmbitoAttività=" & ddlMaccCodAmAtt.SelectedValue & " and attivo =1 order by 1"

            dtrSettori = ClsServer.CreaDatareader(strSQL, Session("conn"))
            If dtrSettori.HasRows = True Then
                ddlCodAmAtt.DataSource = dtrSettori
                ddlCodAmAtt.DataTextField = "Descrizione"
                ddlCodAmAtt.DataValueField = "ID"
                ddlCodAmAtt.DataBind()
            End If
            ChiudiDataReader(dtrSettori)
        Else
            'popolo completamente combo aree di intervento
            ddlCodAmAtt.DataSource = Nothing
        End If
        ddlCodAmAtt.Enabled = ddlMaccCodAmAtt.SelectedValue > 0
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("wfrmImportGraduatoriaVolontari.aspx")
    End Sub

    Private Sub CmdElabora_Click(sender As Object, e As System.EventArgs) Handles CmdElabora.Click
        Dim NomeFile As String

        lblMessaggioErrore.Visible = False
        lblMessaggioErrore.Text = ""

        If Not txtSelFile.HasFile Then
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = "Selezionare il file da inviare."
            Exit Sub
        End If
        If System.IO.Path.GetExtension(txtSelFile.FileName).ToLower() <> ".csv" Then
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = "Selezionare il file nel formato CSV."
            Exit Sub
        End If

        Try
            ' Copia del file sul server
            NomeFile = "DomandeRicevute" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeFile & ".csv")

            If Not CreaTabellaTemp() Then Exit Sub
            Call Importa(NomeFile)

        Catch exc As Exception
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = "Errore in fase di progettazione."

        End Try
    End Sub

    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEsporta.Click
        Dim strSQL As String
        Dim NomeFile As String
        Dim dtrProgetti As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter

        lblmessaggio.Text = ""

        ' lettura dei progetti selezionati
        For Each riga As DataGridItem In dgRisultatoRicerca.Items
            If CType(riga.FindControl("chkSelProg"), CheckBox).Checked Then
                strIDselezionati &= "," & riga.Cells(1).Text
            End If
        Next

        If strIDselezionati = "" Then
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista"
        Else
            strSQL = "SELECT CodiceEnte + ';' + " & _
                     "       REPLACE(REPLACE(Titolo, ';', ''), '""', '') + ';' + " & _
                     "       Convert(nvarchar, C.IDEnteSedeAttuazione) + ';' + " & _
                     "       REPLACE(REPLACE(D.Denominazione, ';', ''), '', '') + ';' + " & _
                     "       E.Denominazione + ';' + " & _
                     "       ISNULL(CONVERT(nvarchar, B.VolontariPresentati), '') + ''" & _
                     "FROM Attività A " & _
                     "INNER JOIN attivitàentisediattuazione B ON A.IDAttività = B.IDAttività " & _
                     "INNER JOIN entisediattuazioni C ON B.IDEnteSedeAttuazione = C.IDEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi D ON C.IDEnteSede = D.IDEnteSede " & _
                     "INNER JOIN Comuni E ON D.IDComune = E.IDComune " & _
                     "INNER JOIN TipiProgetto F ON A.IdTipoProgetto = F.IdTipoProgetto " & _
                     "WHERE E.ComuneNazionale = F.NazioneBase " & _
                     "AND A.IDAttività IN (" & strIDselezionati.Substring(1) & ") " & _
                     "ORDER BY CodiceEnte, C.IDEnteSedeAttuazione"
            Try
                dtrProgetti = ClsServer.CreaDatareader(strSQL, Session("conn"))
                NomeFile = Session("Utente") & "DomandeRicevute" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeFile & ".CSV", True, Encoding.Default)
                ' Riga di Intestazione
                Writer.WriteLine("CodiceProgetto;Titolo;CodiceSede;Sede;Comune;DomandeRicevute")

                While dtrProgetti.Read
                    Writer.WriteLine(dtrProgetti(0))
                End While
                Writer.Close()
                Writer = Nothing

                ChiudiDataReader(dtrProgetti)

                hlVolontari.Visible = True
                hlVolontari.NavigateUrl = "download\" & NomeFile & ".CSV"

            Catch ex As Exception
                lblmessaggio.Text = lblmessaggio.Text & "Errore durante l'esportazione dei Progetti."
                If Not Writer Is Nothing Then
                    Writer.Close()
                    Writer = Nothing
                End If
                ChiudiDataReader(dtrProgetti)
            End Try
        End If
    End Sub

    Private Function CreaTabellaTemp() As Boolean
        Dim strSQL As String
        Dim sqlTemp As SqlClient.SqlCommand

        DropTabellaTemp()

        Try
            strSQL = "CREATE TABLE [#TEMP_DomandeVolontari] (" & _
                     "[CodiceProgetto] [nvarchar] (22) COLLATE DATABASE_DEFAULT, " & _
                     "[Titolo] [nvarchar] (255) COLLATE DATABASE_DEFAULT, " & _
                     "[CodiceSede] [int], " & _
                     "[Sede] [nvarchar] (200) COLLATE DATABASE_DEFAULT, " & _
                     "[Comune] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                     "[NumeroDomande] [int]) "
            sqlTemp = New SqlClient.SqlCommand
            sqlTemp.CommandText = strSQL
            sqlTemp.Connection = Session("conn")
            sqlTemp.ExecuteNonQuery()
            sqlTemp.Dispose()
            Return True

        Catch e As Exception
            sqlTemp.Dispose()
            Return False

        End Try
    End Function

    Private Function Importa(ByVal vlNomeFile As String) As Boolean
        Dim strSQL As String
        Dim cmdTemp As New SqlClient.SqlCommand

        Dim strRiga As String
        Dim arrRiga As String()
        Dim rdrLettura As StreamReader
        Dim wrtNote As StreamWriter
        Dim strNote As String
        Dim booErrore As Boolean
        Dim intRigaOk As Integer = 0
        Dim intRiga As Integer = 0

        Dim clnCodiceProgetto = 0
        Dim clnTitolo = 1
        Dim clnCodiceSede = 2
        Dim clnSede = 3
        Dim clnComune = 4
        Dim clnNumeroDomande = 5

        'Apertura File di lettura e di scrittura
        rdrLettura = New StreamReader(Server.MapPath("upload") & "\" & vlNomeFile & ".CSV", System.Text.Encoding.UTF7, False)
        wrtNote = New StreamWriter(Server.MapPath("download") & "\" & vlNomeFile & ".CSV")

        ' Riga Intestazione
        strRiga = rdrLettura.ReadLine()
        wrtNote.WriteLine("Note;" & strRiga)

        arrRiga = Split(strRiga, ";")
        If UBound(arrRiga) <> 5 Then
            wrtNote.WriteLine("Il numero delle colonne inserite è diverso da quello previsto. " & ";" & strRiga)
        End If

        While Not rdrLettura.EndOfStream
            strRiga = rdrLettura.ReadLine()
            arrRiga = CreaArray(strRiga)
            strNote = ""
            intRiga += 1
            Select Case UBound(arrRiga)
                Case Is < 4
                    strNote = "Il numero delle colonne inserite è minore di quello richieste. "

                Case Is > 5
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste. "

                Case Else
                    ' Controllo i singoli campi inseriti

                    ' Verifica Codice Progetto
                    booErrore = CtrlVuoto(arrRiga(clnCodiceProgetto), "CodiceProgetto", strNote)
                    ' Verifica Codice Sede
                    If Not booErrore And Not CtrlNumeric(arrRiga(clnCodiceSede), "CodiceSede", strNote) Then
                        ' Verifica congruenza entrambi i codici
                        CTRLCodici(arrRiga(clnCodiceProgetto), arrRiga(clnCodiceSede), strNote)
                    End If

                    ' Verifica Numero Domande
                    If Not CtrlVuoto(arrRiga(clnNumeroDomande), "NumeroVolontari", strNote) Then
                        CtrlNumeric(arrRiga(clnNumeroDomande), "NumeroVolontari", strNote)
                    End If

                    If strNote = "" Then
                        strSQL = "INSERT INTO [#TEMP_DomandeVolontari] (" & _
                                 "CodiceProgetto, Titolo, CodiceSede, Comune, NumeroDomande" & _
                                 ") VALUES ('" & _
                                 arrRiga(clnCodiceProgetto) & "', '" & _
                                 arrRiga(clnTitolo).Replace("'", "''") & "', " & _
                                 arrRiga(clnCodiceSede) & ", '" & _
                                 arrRiga(clnComune).Replace("'", "''") & "', " & _
                                 arrRiga(clnNumeroDomande) & ")"
                        Try
                            cmdTemp.CommandText = strSQL
                            cmdTemp.Connection = Session("conn")
                            cmdTemp.ExecuteNonQuery()

                        Catch ex As Exception
                            strNote = "Errore generico. "

                        Finally
                            intRigaOk += 1

                        End Try

                        cmdTemp.Dispose()
                    End If
            End Select
            wrtNote.WriteLine(strNote & ";" & strRiga)
            strNote = vbNullString
        End While

        rdrLettura.Close()
        wrtNote.Close()

        Response.Redirect("WfrmRisultatoImportDomandeVolontari.aspx?NomeFile=" & vlNomeFile & "&Tot=" & intRiga & "&TotOk=" & intRigaOk & "&TotKo=" & intRiga - intRigaOk)
    End Function

    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Function DropTabellaTemp() As Boolean
        Dim strSQL As String
        Dim sqlTemp As SqlClient.SqlCommand
        Try
            '--- CANCELLO TAB TEMPORANEA
            strSQL = "DROP TABLE [#TEMP_DomandeVolontari]"

            sqlTemp = New SqlClient.SqlCommand
            sqlTemp.CommandText = strSQL
            sqlTemp.Connection = Session("conn")
            sqlTemp.ExecuteNonQuery()
            sqlTemp.Dispose()

            Return True

        Catch e As Exception
            sqlTemp.Dispose()
            Return False
        End Try

    End Function

    Private Function CtrlVuoto(ByVal vlValore As String, ByVal vlCampo As String, ByRef vlEsito As String) As Boolean
        If vlValore = vbNullString Then
            vlEsito &= "Il campo " & vlCampo & " e' un campo obbligatorio. "
            Return True
        End If
    End Function

    Private Function CTRLCodici(ByVal vlCodiceProgetto As String, ByVal vlCodiceSede As String, ByRef vlEsito As String) As Boolean
        Dim strSQL As String
        Dim dtrControllo As SqlClient.SqlDataReader
        Dim booErrore As Boolean

        ChiudiDataReader(dtrControllo)
        strSQL = "SELECT * " & _
                 "FROM Attività " & _
                 "WHERE IDEntePresentante = " & Session("IdEnte") & " AND CodiceEnte = '" & vlCodiceProgetto & "'"

        Try
            dtrControllo = ClsServer.CreaDatareader(strSQL, Session("conn"))
            If Not dtrControllo.HasRows Then
                vlEsito &= "Il valore del CodiceProgetto " & vlCodiceProgetto & " non esiste. "
                booErrore = True
            Else
                ChiudiDataReader(dtrControllo)
                strSQL = "SELECT * " & _
                         "FROM Attività A INNER JOIN AttivitàEntiSediAttuazione B ON A.IDAttività = B.IDAttività " & _
                         "INNER JOIN EntiSediAttuazioni C ON B.IDEnteSedeAttuazione = C.IDEnteSedeAttuazione " & _
                         "INNER JOIN EntiSedi D ON C.IDEnteSede = D.IDEnteSede " & _
                         "INNER JOIN Comuni E ON D.IDComune = E.IDComune " & _
                         "INNER JOIN TipiProgetto F ON A.IdTipoProgetto = F.IdTipoProgetto " & _
                         "WHERE E.ComuneNazionale = F.NazioneBase " & _
                         "AND CodiceEnte = '" & vlCodiceProgetto.Replace("'", "''") & "'" & _
                         "AND B.IDEnteSedeAttuazione = " & vlCodiceSede

                dtrControllo = ClsServer.CreaDatareader(strSQL, Session("conn"))
                If Not dtrControllo.HasRows Then
                    vlEsito &= "Il valore del CodiceSede " & vlCodiceSede & " non è corretto per il codiceProgetto " & vlCodiceProgetto & ". "
                    booErrore = True
                Else
                    ChiudiDataReader(dtrControllo)
                    strSQL = "SELECT IDEntità " & _
                             "FROM GraduatorieEntità a " & _
                             "INNER JOIN AttivitàSediAssegnazione b ON a.IdAttivitàSedeAssegnazione = b.IDAttivitàSedeAssegnazione " & _
                             "INNER JOIN Attività c ON b.IDAttività = c.IDAttività " & _
                             "INNER JOIN EntiSediAttuazioni d ON b.IDEnteSede = d.IDEnteSede " & _
                             "WHERE CodiceEnte = '" & vlCodiceProgetto.Replace("'", "''") & "'" & _
                             " AND IDEnteSedeAttuazione = " & vlCodiceSede
                    dtrControllo = ClsServer.CreaDatareader(strSQL, Session("conn"))
                    If dtrControllo.HasRows Then
                        vlEsito &= "Per la sede di progetto " & vlCodiceSede & " è già stata inserita la graduatoria. "
                        booErrore = True
                    End If
                End If
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message.ToString)
        End Try

        ChiudiDataReader(dtrControllo)
        Return booErrore
    End Function

    Private Function CtrlNumeric(ByVal vlValore As String, ByVal vlCampo As String, ByRef vlEsito As String) As Boolean
        If Not IsNumeric(vlValore) Then
            vlEsito &= "Il campo " & vlCampo & " non e' nel formato corretto."
            Return True
        End If
    End Function

    Private Function CreaArray(ByVal pLinea As String) As String()
        ' Se esiste un caratttere ";" all'interno di una cella va ricostruita l'array
        Dim arrTMP As String()

        Dim i As Integer = 0
        Dim x As Integer

        arrTMP = Split(pLinea, ";")

        Do While i <= UBound(arrTMP)
            If arrTMP(i).StartsWith(Chr(34)) Then ' Se il primo carattere è "
                If arrTMP(i).EndsWith(Chr(34)) Then ' Se l'ultimo carattere é "
                    ' Eliminazione degi doppi apici ad inizio e fine riga
                    arrTMP(i) = arrTMP(i).Substring(1, arrTMP(i).Length - 2)
                    i = i + 1
                Else
                    arrTMP(i) = arrTMP(i) & ";" & arrTMP(i + 1)
                    ' Ricerca dei doppi apici che terminano la cella
                    For x = i + 1 To UBound(arrTMP) - 1
                        arrTMP(x) = arrTMP(x + 1)
                    Next
                    ReDim Preserve arrTMP(UBound(arrTMP) - 1)
                End If
            Else
                i = i + 1
            End If
        Loop

        Return arrTMP
    End Function

End Class