Imports System.Data.SqlClient
Imports System.IO
Public Class wfrmConsultaDocumenti
    Inherits System.Web.UI.Page
    Dim dataReader As SqlClient.SqlDataReader
    Dim dtsRisRicerca As DataSet



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
    Private Sub CancellaMessaggiInfo()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim AlboEnte As String
        VerificaSessione()
        If Page.IsPostBack = False Then
            AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
            CaricaTipiDocumento(AlboEnte)
            RicercaDocumenti(Request.QueryString("IdEnteFase"))
            Dim StrSql As String
            ChiudiDataReader(dataReader)

            StrSql = "SELECT denominazione, ISNULL(CodiceRegione,'') as CodiceRegione,  " & _
                    " case ef.tipofase   when 1 then 'Iscrizione' when 2 then 'Adeguamento' when 3 then 'Art.2' when 4 then 'Art.10' end  + ' dal:' + dbo.formatodata (ef.datainiziofase) + ' al:' + dbo.formatodata (ef.datafinefase) as TipoFase " & _
                     " FROM  Enti e " & _
                     " INNER JOIN EntiFasi ef on e.idente = ef.idente " & _
                    " where e.idente= " & Session("IdEnte") & " and ef.identefase =" & Request.QueryString("IdEnteFase") & " "
            dataReader = ClsServer.CreaDatareader(StrSql, Session("conn"))
            dataReader.Read()

            If dataReader.HasRows Then
                lblDenominazioneEnte.Text = dataReader("Denominazione")
                lblEnte.Text = dataReader("CodiceRegione")
                LblRifFase.Text = Request.QueryString("IdEnteFase")
                LblFase.Text = dataReader("Tipofase")
            End If
            ChiudiDataReader(dataReader)
            'If Costanti.TIPOLOGIA_UTENTE_UNSC Then
            '    ' attivo sezione per la ricerca sullo stato dei documenti 
            '    'attivo pulsante che consente di validare tutti i documetni non validati
            'End If
            If Session("TipoUtente") = "U" Then
                divStatoDocumento.Visible = True
            Else
                divStatoDocumento.Visible = False
            End If
        End If
    End Sub
    Private Sub CaricaTipiDocumento(ByVal AlboEnte As String)
        Dim StrSql As String
        ChiudiDataReader(dataReader)
        ddlTipoDoc.Items.Clear()
        'StrSql = " SELECT idPrefisso as idPrefisso, Prefisso as Prefisso, ordine  FROM  PrefissiEntiDocumenti " & _
        '         " WHERE ModalitàInvio in ('HELIOS','AUTOMATICO') and ISNULL(albo,'" & AlboEnte & "')='" & AlboEnte & "' " & _
        '         " UNION Select 0, 'Seleziona',0  order by Ordine"

        StrSql = "select distinct idPrefisso, Prefisso, ordine from VW_DOCUMENTI where idEnteFase=" & Request.QueryString("IdEnteFase") & " UNION Select 0, 'Seleziona',0 order by ordine"
        dataReader = ClsServer.CreaDatareader(StrSql, Session("conn"))


        If dataReader.HasRows Then
            ddlTipoDoc.DataSource = dataReader
            ddlTipoDoc.DataTextField = "Prefisso"
            ddlTipoDoc.DataValueField = "IdPrefisso"
            ddlTipoDoc.DataBind()
        End If

        ChiudiDataReader(dataReader)
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        dtgConsultaDocumenti.CurrentPageIndex = 0
        RicercaDocumenti(Request.QueryString("IdEnteFase"))

    End Sub
    Private Sub RicercaDocumenti(ByVal IdEnteFase As Integer)
        Dim query As String
        CancellaMessaggiInfo()


        ChiudiDataReader(dataReader)
        Dim fileName As String = Request.QueryString("idDocumento") & ""
        If fileName <> "" Then
            txtNomeFile.Text = fileName
        End If

        query = " SELECT idente, IdEnteDocumento, FileName + case linkpagina when 'non definito' then '' else ' <a href=""'+linkpagina+'"" style=""float: left;"" title=""vedi questo documento nella maschera cui appartiene""><img src=""Images/lenteIngrandimento_small.png""></a>' end Filename, " 
        query &= "DataInserimento, hashvalue, TipoFase , TipologiaDocumento, Prefisso, StatoDocumento, PulsanteConfermaValida, "
        query &= "PulsanteConfermaNONValida, DatafineFase,  ordine, Soggetto "
        query &= "FROM VW_DOCUMENTI "
        query &= " WHERE idente =" & Session("IdEnte") & "  "
        query &= " And IdEnteFase = " & IdEnteFase & " And stato <> 2 "

        If ddlTipoDoc.SelectedValue <> "0" Then
            Dim prefisso As String = ddlTipoDoc.SelectedItem.ToString
            prefisso = Replace(prefisso, "'", "") 'sqlinjection
            prefisso = Replace(prefisso, "_", "[_]")
            query = query & " AND FileName like '" & prefisso & "%'"
        End If
        If txtNomeFile.Text <> "" Then
            query = query & " AND FileName like '%" & txtNomeFile.Text.Replace("'", "''") & "%'"
        End If
        If txtSoggetto.Text <> "" Then
            query = query & " AND Soggetto like '%" & txtSoggetto.Text.Replace("'", "''") & "%'"
        End If
        'If ddlStatoDocumento.SelectedValue <> "" Then
        '    query = query & "AND a.Stato =  " & ddlStatoDocumento.SelectedValue & ""
        'End If

        'modifica stefano rossetti 07/07/2021
        If ddlStatoDocumento.SelectedValue <> "" Then
            query = query & " AND StatoDocumento = '" & ddlStatoDocumento.SelectedItem.Text & "'"
        End If
        query = query & " order by datafinefase desc, ordine "

        dtsRisRicerca = ClsServer.DataSetGenerico(query, Session("conn"))


        Session("reader") = dtsRisRicerca
        
        dtgConsultaDocumenti.DataSource = dtsRisRicerca
        dtgConsultaDocumenti.DataBind()
        dtgConsultaDocumenti.Caption = "Elenco Documenti"

        'dtgConsultaDocumenti.Caption = "La Ricerca Non ha prodotto Risultati"
        'dtgConsultaDocumenti.DataBind()

        ChiudiDataReader(dataReader)
        If Session("TipoUtente") = "U" Then

            dtgConsultaDocumenti.Columns(5).Visible = False 'hashvalue
            If VerificaAbilitazioneMenuValidazioneDocumenti(Session("Utente")) = True Then
                dtgConsultaDocumenti.Columns(8).Visible = True  ' STATO DOCUMENTO"
                dtgConsultaDocumenti.Columns(9).Visible = True  'pulsante "valida documento"
                dtgConsultaDocumenti.Columns(10).Visible = True 'pulsante "respingi documento"
               
            Else
                dtgConsultaDocumenti.Columns(8).Visible = False ' STATO DOCUMENTO"
                dtgConsultaDocumenti.Columns(9).Visible = False 'pulsante "valida documento"
                dtgConsultaDocumenti.Columns(10).Visible = False 'pulsante "respingi documento"
                
            End If
        End If
        If Session("TipoUtente") = "E" Then

            dtgConsultaDocumenti.Columns(5).Visible = True 'hashvalue
            dtgConsultaDocumenti.Columns(8).Visible = False ' STATO DOCUMENTO"
        End If
    End Sub
    Private Sub dtgConsultaDocumenti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgConsultaDocumenti.ItemCommand
        Dim objHLink As HyperLink
        Dim msg As String
        Dim Esito As String
        msgErrore.Visible = False
        msgConferma.Visible = False
        Select Case e.CommandName

            Case "Download"
                objHLink = clsGestioneDocumentiAccreditamento.RecuperaDocumentoEnte(e.Item.Cells(1).Text, Session("Conn"))
                divDownloadFile.Visible = True
                hlScarica.Visible = True
                hlScarica.Text = objHLink.Text
                hlScarica.NavigateUrl = objHLink.NavigateUrl

            Case "Valida"
                
                msg = ConfermaDocumentoEnte(Session("Utente"), CInt(e.Item.Cells(1).Text), 1, e.Item.Cells(6).Text, Session("IdEnte"), Session("Conn"), msg, Esito)
                If Esito = "NEGATIVO" Then
                    msgErrore.Visible = True
                    msgErrore.Text = msg
                Else
                    msgConferma.Visible = True
                    msgConferma.Text = msg
                End If
                RicercaDocumenti(Request.QueryString("IdEnteFase"))
                'lblmessaggio.Text = msg
                ''lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                'lblmessaggio.Visible = True
                'LblMsgFile.Text = ""

                'LoadGriglia()

            Case "NonValida"
                msg = ConfermaDocumentoEnte(Session("Utente"), CInt(e.Item.Cells(1).Text), 2, e.Item.Cells(6).Text, Session("IdEnte"), Session("Conn"), msg, Esito)
                If Esito = "NEGATIVO" Then
                    msgErrore.Visible = True
                    msgErrore.Text = msg
                Else
                    msgConferma.Visible = True
                    msgConferma.Text = msg
                End If
                RicercaDocumenti(Request.QueryString("IdEnteFase"))
                'msg = ClsServer.ConfermaDocumento(Session("Utente"), e.Item.Cells(0).Text, 2, e.Item.Cells(6).Text, Session("IdEnte"), Session("Conn"))

                'If Esito = "NEGATIVO" Then
                '    lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#C00000")
                'Else
                '    lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                'End If

                'LoadGriglia()
                'lblmessaggio.Visible = True
                'lblmessaggio.Text = msg
                'lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                'LblMsgFile.Text = ""

        End Select
    End Sub
    Private Sub ddlTipoDoc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoDoc.SelectedIndexChanged
        CancellaMessaggiInfo()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Dim tipologia As String = Request.QueryString("tipologia")
        Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdEnteFse=" & Request.QueryString("IdEnteFase") & "&tipologia=" & tipologia)
    End Sub
    Private Sub dtgConsultaDocumenti_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgConsultaDocumenti.PageIndexChanged
        dtgConsultaDocumenti.SelectedIndex = -1
        dtgConsultaDocumenti.EditItemIndex = -1
        dtgConsultaDocumenti.CurrentPageIndex = e.NewPageIndex
        dtgConsultaDocumenti.DataSource = Session("reader")
        dtgConsultaDocumenti.DataBind()
    End Sub
    Private Function VerificaAbilitazioneMenuValidazioneDocumenti(ByVal Utente As String) As Boolean
        'Agg da  Simona Cordella il 30/04/2015
        'Verifico se l'utene U è autorizzato alla visualizzazione del menu ValidazioneDocumenti
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Validazione Documenti Ente'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

        VerificaAbilitazioneMenuValidazioneDocumenti = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Private Function ConfermaDocumentoEnte(ByVal Utente As String, ByVal IdEnteDocumento As Integer, ByVal Stato As Integer, ByVal Documento As String, ByVal IdEnte As Integer, ByVal connessione As SqlClient.SqlConnection, Optional ByRef msg As String = "", Optional ByRef Esito As String = "") As String
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  08/01/2018
        'FUNZIONALITA': RICHIAMO STORE PER LA VALIDAZIONE DEI DOCUMENTI
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_VALIDA_DOCUMENTI_ENTE]"


        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, connessione)
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Utente
            sqlCMD.Parameters.Add("@IdEnteDocumento", SqlDbType.Int).Value = IdEnteDocumento
            sqlCMD.Parameters.Add("@Stato", SqlDbType.Int).Value = Stato
            sqlCMD.Parameters.Add("@Documento", SqlDbType.VarChar).Value = Documento
            sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            Dim str As String
            msg = sqlCMD.Parameters("@Messaggio").Value
            Esito = sqlCMD.Parameters("@Esito").Value
            'Return str

        Catch ex As Exception

            Exit Function
        End Try
    End Function

End Class