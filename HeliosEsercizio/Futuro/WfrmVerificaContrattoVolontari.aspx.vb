Imports System.IO

Public Class WfrmVerificaContrattoVolontari
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            ddlStatoContratto.SelectedValue = 1
            If Not Request.QueryString("CodiceVolontario") Is Nothing Then
                txtCodVolontario.Text = Request.QueryString("CodiceVolontario")
                ddlStatoContratto.SelectedValue = Request.QueryString("StatoContratto")
            End If
            LoadElencoContrattiVolontari()
        End If
    End Sub

    Private Sub LoadElencoContrattiVolontari()
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  12/09/2012
        'FUNZIONALITA': RICHIAMO STORE PER IL CARICAMENTO DELL'ELENCO DEI CONTRATTI DEI VOLONTARI
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_RITORNA_ELENCO_VOLONTARI_CONTRATTO]"
        lblmsg.Visible = False
        divDownload.Visible = False
        'lblTestoUP.Visible = False
        'hlScarica.Visible = False
        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = txtCognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txtNome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = txtCodVolontario.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceFiscale", SqlDbType.VarChar).Value = txtCodiceFiscale.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@NomeEnte", SqlDbType.VarChar).Value = txtNomeEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodiceEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@StatoContrattoVolontario", SqlDbType.Int).Value = ddlStatoContratto.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@TerminiScadenza", SqlDbType.Int).Value = ddlTerminiScaduti.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@FiltroVisibilita", SqlDbType.VarChar).Value = Session("FiltroVisibilita")
            sqlDAP.Fill(dataSet)

            dtgElencoContratti.DataSource = dataSet
            dtgElencoContratti.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Sub dtgElencoContratti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgElencoContratti.ItemCommand
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  12/09/2012
        'FUNZIONALITA': NELLA GRIGLIA SONO PRESENTI 4 PULSANTI, 
        '               "Download" PER IL DOWNLOAD DEL FILE 
        '               "Proroga" PER LA PROROGA DI 7 GG DOPO LA SCADENZA 
        '               "Approva" PER L'APPROVAZIONE DEL CONTRATTO IMPORTATO DAL VOLONTARIO (FUNZIONE CONSENTITA DAL SITO DEL SERVIZIO CIVILE)
        '               "Cancella" PER IL RIFIUTO DEL FILE IMPORTATO, QUESTA FUNZIONE CONSENTIRà AL VOLONTARIO SE ANCORA NEI TERMINI PREVISTI, DI POTER CARICARE NUOVAMENTE IL FILE
        lblmsg.Visible = False
        Select Case e.CommandName
            Case "Download"
                Select Case e.Item.Cells(7).Text
                    Case "DA CARICARE", "RESPINTO"
                        lblmsg.Visible = True
                        lblmsg.Text = "Impossibile scaricare il file. Il contratto non è presente."
                    Case "CARICATO", "APPROVATO"
                        lblmsg.Visible = False
                        DownloadDocumento(e.Item.Cells(8).Text, e.Item.Cells(2).Text, e.Item.Cells(15).Text)
                End Select
            Case "Proroga"
                If (e.Item.Cells(7).Text = "DA CARICARE" Or e.Item.Cells(7).Text = "RESPINTO") _
                    And e.Item.Cells(14).Text = 0 Then
                    ProrogaContratto(e.Item.Cells(13).Text) ' = False Then
                    dtgElencoContratti.CurrentPageIndex = 0
                    LoadElencoContrattiVolontari()
                Else
                    If (e.Item.Cells(7).Text = "CARICATO" Or e.Item.Cells(7).Text = "APPROVATO") Then
                        lblmsg.Text = "Impossibile effettuare la Proroga. Il contratto risulta gia caricato."
                    Else
                        lblmsg.Text = "Impossibile effettuare la Proroga. I termini non risultano scaduti."
                    End If
                    lblmsg.Visible = True
                End If
            Case "Approva"
                Select Case e.Item.Cells(7).Text
                    Case "DA CARICARE"
                        lblmsg.Visible = True
                        lblmsg.Text = "Impossibile Approvare. Il contratto non è stato Caricato."
                    Case "CARICATO"
                        lblmsg.Visible = False
                        ModificaStatoContratto(e.Item.Cells(13).Text, 2)
                        LoadElencoContrattiVolontari()
                    Case "APPROVATO"
                        lblmsg.Visible = True
                        lblmsg.Text = "Impossibile Approvare. Il contratto risulta essere già Approvato."
                    Case "RESPINTO"
                        lblmsg.Visible = True
                        lblmsg.Text = "Impossibile Approvare. Il contratto risulta essere Respinto."
                End Select
            Case "Cancella"
                Select Case e.Item.Cells(7).Text
                    Case "DA CARICARE"
                        lblmsg.Visible = True
                        lblmsg.Text = "Impossibile Respingere. Il contratto non è stato Caricato."
                    Case "CARICATO", "APPROVATO"
                        ModificaStatoContratto(e.Item.Cells(13).Text, 3)
                        LoadElencoContrattiVolontari()
                        lblTestoUP.Visible = False
                    Case "RESPINTO"
                        lblmsg.Visible = True
                        lblmsg.Text = "Impossibile Respingere. Il contratto risulta essere Respinto."
                End Select
        End Select
    End Sub

    Private Sub dtgElencoContratti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgElencoContratti.PageIndexChanged
        dtgElencoContratti.CurrentPageIndex = e.NewPageIndex
        LoadElencoContrattiVolontari()
        dtgElencoContratti.SelectedIndex = -1
        dtgElencoContratti.EditItemIndex = -1
    End Sub

    Private Sub DownloadDocumento(ByVal CodiceAllegato As String, ByVal Volontario As String, ByVal NomeFileContratto As String)
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
                lblmsg.Text = SIGED.Esito
                Exit Sub
            End If
            'Ottiene il percorso per recuperare il file
            PathServerSiged = "\\" & ConfigurationSettings.AppSettings("SERVER_SIGED") & "\" & ConfigurationSettings.AppSettings("CARTELLA_SIGED")  '& "\" & pNomeFile
            'NomeFile = "Contratto " & Volontario & ".pdf"

            wsOut = SIGED.SIGED_RestituisciDocumentoInterno(CodiceAllegato, "", PathServerSiged & "\" & Trim(NomeFileContratto))
            If SIGED.SIGED_Codice_Esito(wsOut.ESITO) = 0 Then
                PathLocale = myPath.Server.MapPath("download\") & wsOut.DATI.NOMEFILE  '& NomeFile
                If File.Exists(PathLocale) = True Then
                    File.Delete(PathLocale)
                End If
                File.Copy(PathServerSiged & "\" & wsOut.DATI.NOMEFILE, Trim(PathLocale))

                divDownload.Visible = True
                'lblTestoUP.Visible = True
                'hlScarica.Visible = True
                hlScarica.Text = wsOut.DATI.NOMEFILE
                hlScarica.NavigateUrl = "download\" & wsOut.DATI.NOMEFILE   'PathLocale
            Else
                lblmsg.Visible = True
                lblmsg.Text = Mid(wsOut.ESITO, 6, Len(wsOut.ESITO))
                divDownload.Visible = False
                'lblTestoUP.Visible = False
                'hlScarica.Visible = False
            End If

        Catch ex As Exception
            lblmsg.Text = "Errore imprevisto. Contattare l'assistenza."
            divDownload.Visible = True
            'lblTestoUP.Visible = True
            'hlScarica.Visible = True
        Finally
            SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
        End Try
    End Sub

    Private Sub ProrogaContratto(ByVal IdEntità As Integer)
        Dim myCommand As SqlClient.SqlCommand

        myCommand = New SqlClient.SqlCommand("UPDATE Entità set DataProrogaContratto= getdate() where IdEntità = " & IdEntità & "  ", Session("Conn"))
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()
    End Sub

    Private Function ModificaStatoContratto(ByVal IdEntita As Integer, ByVal StatoContratto As Integer)
        Dim StrSql As String
        lblmsg.Visible = True
        lblmsg.Text = ""
        StrSql = "UPDATE Entità set StatoContrattoVolontario =" & StatoContratto & " "
        StrSql = StrSql & " , UsernameValutazioneContratto = '" & Session("Utente") & "'"
        StrSql = StrSql & " , DataValutazioneContratto = getdate() "
        If StatoContratto = 3 Then
            StrSql = StrSql & " , RiferimentoContrattoVolontario = null"
        End If
        StrSql = StrSql & " where  IdEntità = " & IdEntita
        ClsServer.EseguiSqlClient(StrSql, Session("Conn"))
        CronologiaStatoContratto(IdEntita)
        lblmsg.Text = "Modifica effettuata con successo."
    End Function

    Private Function CronologiaStatoContratto(ByVal IdEntita As Integer)
        Dim StrSql As String

        StrSql = "INSERT INTO CronologiaEntitàContratto (IdEntità, RiferimentoContrattoVolontario, StatoContratto, NomeFileContratto, UserNameStato, DataCronologiaStato)"
        StrSql = StrSql & " (SELECT IDEntità, RiferimentoContrattoVolontario, StatoContrattoVolontario, NomeFileContratto, UsernameValutazioneContratto, DataValutazioneContratto "
        StrSql = StrSql & " FROM entità "
        StrSql = StrSql & " where IdEntità = " & IdEntita & " )"
        ClsServer.EseguiSqlClient(StrSql, Session("Conn"))

    End Function

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        dtgElencoContratti.CurrentPageIndex = 0
        LoadElencoContrattiVolontari()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub


End Class