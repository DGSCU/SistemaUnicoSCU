Imports System.Data.SqlClient

Public Class WfrmRicercaVolontariStati
    Inherits System.Web.UI.Page
    Public dtsRisRicerca As DataSet
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
    Private Sub CancellaMessaggi()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()

        If Page.IsPostBack = False Then
            Call CaricaDatiIniziali()
        End If

    End Sub


    Private Sub CaricaDatiIniziali()
        Dim strSql As String
        Dim MyDataset As DataSet
            cboSesso.Items.Add("")
            cboSesso.Items(0).Value = ""
            cboSesso.Items.Add("Uomo")
            cboSesso.Items(1).Value = 0
            cboSesso.Items.Add("Donna")
            cboSesso.Items(2).Value = 1

            cboStato.Items.Clear()
            strSql = "SELECT '0' as IdStatoEntità, '' as Statoentità FROM StatiEntità " & _
                     "UNION SELECT IdStatoEntità, StatoEntità  FROM StatiEntità"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            cboStato.DataSource = MyDataset
            cboStato.DataValueField = "IdStatoEntità"
            cboStato.DataTextField = "StatoEntità"
            cboStato.DataBind()

            cboStatiAttestato.Items.Clear()
            strSql = "SELECT '0' as IdStatoAttestato, '' as StatoAttestato FROM StatiAttestato " & _
                     "UNION SELECT IdStatoAttestato, StatoAttestato  FROM StatiAttestato  "
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            cboStatiAttestato.DataSource = MyDataset
            cboStatiAttestato.DataValueField = "IdStatoAttestato"
            cboStatiAttestato.DataTextField = "StatoAttestato"
            cboStatiAttestato.DataBind()

            cboStatiAttestatiSel.DataSource = MyDataset
            cboStatiAttestatiSel.DataValueField = "IdStatoAttestato"
            cboStatiAttestatiSel.DataTextField = "StatoAttestato"
            cboStatiAttestatiSel.DataBind()

    End Sub

    Sub CaricaGriglia()
        CancellaMessaggi()
        dtgRisultatoRicerca.CurrentPageIndex = 0
        Dim strSQL As String
        Dim strCondizione As String

        strSQL = "SELECT DISTINCT " & _
                 "Entità.IDEntità AS IDEntità, " & _
                 "isnull(Entità.AltreInformazioni,'') + case isnull(entità.notestato,'-1')when '-1' then '' " & _
                 " else 'note chiusura: ' end +  isnull(entità.notestato,'') AS AltreInfo,  " & _
                 "isnull(rtrim(ltrim(StatiAttestato.StatoAttestato)),'Indefinito') AS CLP, " & _
                 "Attività.IDAttività AS IdAttività, " & _
                 "Entità.Cognome + ' ' + Entità.Nome AS Nominativo, " & _
                 "Comuni.Denominazione + ' (' + isnull(Provincie.DescrAbb,Provincie.Provincia) + ') ' AS ComProv, " & _
                 "Comuni.Denominazione AS Comune, " & _
                 "Provincie.Provincia AS Provincia, " & _
                 "CASE Entità.Sesso WHEN 0 THEN 'UOMO' WHEN 1 THEN 'DONNA' END AS Sesso, " & _
                 "CASE Entità.Abilitato	WHEN 0 THEN 'NO' WHEN 1 THEN 'SI'  END AS Abilitato, " & _
                 "COUNT(Attività.Titolo) as NumeroAttivita, " & _
                 "CONVERT(varchar, Entità.DataInizioServizio, 103)as DataInizio, " & _
                 "CONVERT(varchar, Entità.DataFineServizio, 103) as DataFine, " & _
                 "StatiEntità.StatoEntità as Stato, " & _
                 "enti.CodiceRegione, " & _
                 "Enti.IdEnte as IdEnte, " & _
                 "Enti.Denominazione + ' (' + isnull(attività.CodiceEnte,'') + ') ' as Ente ,entità.codicevolontario, " & _
                 "Enti.Denominazione as Ente1,Entità.codicefiscale as cf, " & _
                 "isnull(case len(day(Entità.DataAttestato)) when 1 then '0' + convert(varchar(20),day(Entità.DataAttestato)) " & _
                 "else convert(varchar(20),day(Entità.DataAttestato))  end + '/' + " & _
                 "(case len(month(Entità.DataAttestato)) when 1 then '0' + convert(varchar(20),month(Entità.DataAttestato)) " & _
                 "else convert(varchar(20),month(Entità.DataAttestato))  end + '/' + " & _
                 "Convert(varchar(20), Year(Entità.DataAttestato))),'') as DataAttestato "


        strSQL = strSQL & "FROM AttivitàEntiSediAttuazione " & _
                 "INNER JOIN AttivitàEntità ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione = AttivitàEntità.IDAttivitàEnteSedeAttuazione " & _
                 "RIGHT OUTER JOIN Entità ON Attivitàentità.IDEntità = Entità.IDEntità " & _
                 "INNER JOIN StatiEntità ON Entità.IDStatoEntità = StatiEntità.IDStatoEntità " & _
                 "INNER JOIN GRADUATORIEENTITà on graduatorieentità.identità = entità.identità " & _
                 "INNER JOIN attivitàsediassegnazione on attivitàsediassegnazione.idattivitàsedeassegnazione = graduatorieentità.idattivitàsedeassegnazione " & _
                 "INNER JOIN Entisedi on attivitàsediassegnazione.identesede = entisedi.identesede " & _
                 "INNER JOIN Comuni ON Entisedi.IDComune = Comuni.IDComune " & _
                 "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia " & _
                 "INNER JOIN Regioni ON Provincie.IDRegione = Regioni.IDRegione " & _
                 "INNER JOIN attività on attività.idattività = attivitàsediassegnazione.idattività " & _
                 "INNER JOIN enti on attività.identepresentante = enti.idente " & _
                 "LEFT OUTER JOIN StatiAttestato on entità.IdStatoAttestato = StatiAttestato.IdStatoAttestato " & _
                 "INNER JOIN TIPIPROGETTO ON attività.idtipoprogetto=TIPIPROGETTO.idtipoprogetto "

        strCondizione = "WHERE "
        'se viene selezionato l'ente faccio il filtro su di lui
        If Request.Params("Ente") = "OK" Then
            strSQL = strSQL & strCondizione & "Attività.IDEntePresentante = " & Session("IdEnte") & " "
            strCondizione = "AND "
        End If
        'imposto le condizioni dinamicamente
        If cboSesso.SelectedIndex >= 1 Then
            strSQL = strSQL & strCondizione & "Sesso = '" & cboSesso.Items(cboSesso.SelectedIndex).Value.ToString & "' "
            strCondizione = "AND "
        End If
        If cboStato.SelectedIndex >= 1 Then
            strSQL = strSQL & strCondizione & "Entità.IDStatoEntità = '" & cboStato.Items(cboStato.SelectedIndex).Value.ToString & "' "
            strCondizione = "AND "
        End If

        If cboStatiAttestato.SelectedIndex >= 1 Then
            strSQL = strSQL & strCondizione & " isnull(rtrim(ltrim(StatiAttestato.IdStatoAttestato)),1) = '" & cboStatiAttestato.Items(cboStatiAttestato.SelectedIndex).Value.ToString & "' "
            strCondizione = "AND "
        End If


        If txtCognome.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Cognome LIKE '" & Replace(txtCognome.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtNome.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Nome LIKE '" & Replace(txtNome.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtRegione.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Regioni.Regione LIKE '" & Replace(txtRegione.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtProvincia.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtComune.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Comuni.Denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtDescEnte.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Enti.Denominazione LIKE '" & Replace(txtDescEnte.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtCodEnte.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Enti.CodiceRegione = '" & Replace(txtCodEnte.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        If txtProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Attività.Titolo LIKE '" & Replace(txtProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtCodProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Attività.CodiceEnte LIKE '" & Replace(txtCodProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If

        'Controllo Data Inizio Servizio
        If txtDataInizServ.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Entità.DataInizioServizio = '" & txtDataInizServ.Text & "'"
            strCondizione = "AND "
        End If

        If txtalladata.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "'" & txtalladata.Text & "' between Entità.datainizioservizio and Entità.datafineservizio and Entità.datainizioservizio <> Entità.datafineservizio "
            strCondizione = "AND "

        End If

        If txtCodVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & " entità.codicevolontario= '" & Replace(txtCodVolontario.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        'FiltroVisibilita 01/12/20104 da s.c.
        If Session("FiltroVisibilita") <> Nothing Then
            strSQL = strSQL & strCondizione & " TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            strCondizione = "AND "
        End If
        'eseguo la group by per il numero attività
        strSQL = strSQL & " GROUP BY " & _
                          "Entità.IDEntità, " & _
                          "Attività.IDAttività, " & _
                          "isnull(Entità.AltreInformazioni,'') + case isnull(entità.notestato,'-1')when '-1' then '' " & _
                          " else 'note chiusura: ' end +  isnull(entità.notestato,''), " & _
                          "Entità.Cognome + ' ' + Entità.Nome, " & _
                          "Comuni.Denominazione, " & _
                          "Provincie.Provincia,  " & _
                          "CASE Entità.Sesso WHEN 0 THEN 'UOMO' WHEN 1 THEN 'DONNA' END, " & _
                          "CASE Entità.Abilitato	WHEN 0 THEN 'NO' WHEN 1 THEN 'SI'  END, " & _
                          "CONVERT(varchar, Entità.DataInizioServizio, 103), " & _
                          "CONVERT(varchar, Entità.DataFineServizio, 103), " & _
                          "Attività.Titolo, " & _
                          "Enti.CodiceRegione, " & _
                          "StatiEntità.StatoEntità, " & _
                          "Enti.CodiceRegione, " & _
                          "Enti.IdEnte, " & _
                          "Enti.Denominazione, " & _
                          "attività.CodiceEnte, " & _
                          "StatiAttestato.StatoAttestato, " & _
                          "Provincie.DescrAbb, " & _
                          "entità.codicevolontario,Entità.codicefiscale,Entità.DataAttestato order by Entità.Cognome + ' ' + Entità.Nome "
        'eseguo la query
        dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dtgRisultatoRicerca.DataSource = dtsRisRicerca

        If dtsRisRicerca.Tables(0).Rows.Count > 0 Then
            dtgRisultatoRicerca.Caption = "Risultato Ricerca Volontari"
            fldsetDataGrid.Visible = True
        Else
            dtgRisultatoRicerca.Caption = "La ricerca non ha prodotto risultati."
            fldsetDataGrid.Visible = False
        End If

        Session("appDtsRisRicerca") = dtsRisRicerca
        dtgRisultatoRicerca.DataBind()
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        Call CaricaGriglia()
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dtgRisultatoRicerca.DataBind()
        dtgRisultatoRicerca.SelectedIndex = -1
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        'carico la home
        Session("appDtsRisRicerca") = Nothing
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dtgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand
        Dim idVolontario As String

        If e.CommandName = "SelezionaVolontario" Then
            idVolontario = e.Item.Cells(4).Text
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                If e.Item.Cells(12).Text = "&nbsp;" Or e.Item.Cells(12).Text = "" Or e.Item.Cells(12).Text = "&nbsp" Then
                    Session("IdEnte") = -1
                Else
                    Session("IdEnte") = e.Item.Cells(12).Text
                End If
                Session("Denominazione") = e.Item.Cells(13).Text
            End If
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                Response.Redirect("WfrmVolontari.aspx?Vengoda=attestati&IdVol=" & idVolontario & "&IdAttivita=" & e.Item.Cells(3).Text)
            Else
                Response.Redirect("WfrmVolontari.aspx?Vengoda=attestati&IdVol=" & idVolontario & "&IdAttivita=" & e.Item.Cells(3).Text & "&Ente=OK")
            End If
        ElseIf e.CommandName = "StoricoVolontario" Then
            idVolontario = e.Item.Cells(4).Text
            Response.Write("<script>" & vbCrLf)
            Response.Write("window.open(""WfrmAttivitaVolontari.aspx?IdVolontario=" & idVolontario & """, ""Visualizza"", ""width=670,height=300,dependent=no,resizable=yes,scrollbars=yes,status=no"")" & vbCrLf)
            Response.Write("</script>")
        End If
    End Sub

    'Applicare lo stato della combo a tutti i volontari selezionati
    'aggiorno lo stato dei progetti selezionati
    Private Sub imgConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgConferma.Click
        CancellaMessaggi()
        If (cboStatiAttestatiSel.SelectedValue = "0") Then
            msgErrore.Text = "Selezionare il nuovo stato da applicare ai volontari selezionati."
        Else
            Dim i As Integer
            Dim Mychk As CheckBox
            Dim strSQL As String
            Try

                For i = 0 To dtgRisultatoRicerca.Items.Count - 1
                    Mychk = dtgRisultatoRicerca.Items.Item(i).FindControl("chkSelVol")
                    If Mychk.Checked = True Then
                        'aggiorno la tabella entià
                        strSQL = "Update Entità Set IdStatoAttestato=" & cboStatiAttestatiSel.Items(cboStatiAttestatiSel.SelectedIndex).Value.ToString & ", DataAttestato=GetDate(),UsernameAttestato='" & Session("utente") & "' Where identità=" & dtgRisultatoRicerca.Items(i).Cells(4).Text
                        ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                        'inserisco la cronologia
                        strSQL = "Insert Into CronologiaStatiAttestato (IDEntità,IDStato,DataStato,UserNameStato) " & _
                                " VALUES (" & dtgRisultatoRicerca.Items(i).Cells(4).Text & "," & cboStatiAttestatiSel.Items(cboStatiAttestatiSel.SelectedIndex).Value.ToString & ",GetDate(),'" & Session("utente") & "')"
                        ClsServer.EseguiSqlClient(strSQL, Session("conn"))

                        dtgRisultatoRicerca.Items.Item(i).Cells(14).Text = cboStatiAttestatiSel.SelectedItem.Text

                    End If
                Next

                msgConferma.Text = "Aggiornamento completato con successo"

            Catch ex As Exception
                msgErrore.Text = "Errore imprevisto. Se il problema si ripete contattare l'assistenza Helios/Futuro."
            End Try

        End If
    End Sub
    Private Sub checkSelDesel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkSelDesel.CheckedChanged
        Dim i As Integer
        Dim Mychk As CheckBox

        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dtgRisultatoRicerca.Items.Count - 1
            Mychk = dtgRisultatoRicerca.Items.Item(i).FindControl("chkSelVol")

            If (Mychk.Enabled = True) Then
                Mychk.Checked = checkSelDesel.Checked
            End If
        Next i
        If (checkSelDesel.Checked) Then
            checkSelDesel.Text = "Deseleziona Tutto"

        Else
            checkSelDesel.Text = "Seleziona Tutto"
        End If

    End Sub
End Class