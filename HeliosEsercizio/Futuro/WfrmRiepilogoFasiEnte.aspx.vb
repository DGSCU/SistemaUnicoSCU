Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Public Class WfrmRiepilogoFasiEnte
    Inherits System.Web.UI.Page
    Dim myQuerySql As String
    Dim myDataReader As SqlClient.SqlDataReader

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
        VerificaSessione()

        If IsPostBack = False Then
            CaricaGriglia()
        End If
    End Sub
    Private Sub CaricaGriglia()

        myQuerySql = " Select  "
        myQuerySql &= " case EntiFasi.TipoFase "
        myQuerySql &= " when 1 then 'Iscrizione' when 2 then 'Adeguamento'	"
        myQuerySql &= " when 3 then 'Art2' + ISNULL(' ('  + case e.TipoFase when 1 then 'Accr.' when 2 then 'Adeg.' end + ' Rif: ' + convert(varchar,EntiFasi.IdEnteFaseRiferimento)+ ')','')"
        myQuerySql &= " when 4  then 'Art10' + ISNULL(' ('  + case e.TipoFase when 1 then 'Accr.' when 2 then 'Adeg.' end + ' Rif: ' + convert(varchar,EntiFasi.IdEnteFaseRiferimento)+ ')','') end as TipoFase,"
        myQuerySql &= " EntiFasi.IdEnteFase,	case EntiFasi.stato  When 1 then case when  GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase then 'Aperta' ELSE 'Scaduta' end  when 2 then 'Annullata' when 3 then  'Presentata'	when 4  then 'Valutata' end as  Stato, "
        myQuerySql &= " EntiFasi.DataInizioFase , EntiFasi.DataFineFase "
        myQuerySql &= " From EntiFasi "
        myQuerySql &= " left join EntiFasi e on EntiFasi.IdEnteFaseRiferimento = e.IdEnteFase"
        myQuerySql &= " Where EntiFasi.IdEnte = " & Session("IdEnte") & " "
        Select Case Request.QueryString("VengoDa")
            Case Costanti.VENGO_DA_ADEGUAMENTO_VARIAZIONE_ENTE
                myQuerySql &= " and EntiFasi.TipoFase in (1,2)"
            Case Costanti.VENGO_DA_ADEGUAMENTO_ADEGUAMENTO_OK
                myQuerySql &= " and EntiFasi.TipoFase in (1,2) and EntiFasi.Stato= 3 "
            Case Costanti.VENGO_DA_ADEGUAMENTO_ARTICOLO_2
                myQuerySql &= " and EntiFasi.TipoFase in (1,2) and EntiFasi.Stato= 3"
            Case Costanti.VENGO_DA_ADEGUAMENTO_ARTICOLO_10
                myQuerySql &= " and EntiFasi.TipoFase in (1,2) and EntiFasi.Stato= 3 "
            Case Costanti.VENGO_DA_ADEGUAMENTO_INSERISCI_DOCUMENTI
                myQuerySql &= " and EntiFasi.stato=1 and GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase "
            Case Costanti.VENGO_DA_ADEGUAMENTO_PRESENTA_ARTICOLO_2
                myQuerySql &= "  and EntiFasi.stato=1 and EntiFasi.TipoFase=3 and GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase "
            Case Costanti.VENGO_DA_ADEGUAMENTO_PRESENTA_ARTICOLO_10
                myQuerySql &= " and EntiFasi.stato=1 and EntiFasi.TipoFase=4 and GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase "
            Case Costanti.VENGO_DA_AGGIORNA_SEDI_ARTICOLO_2
                myQuerySql &= "  and EntiFasi.stato=1 and EntiFasi.TipoFase=3 and GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase "
        End Select
        myQuerySql &= "Order by EntiFasi.DataFineFase Desc"
        Session("dtsCarica") = ClsServer.DataSetGenerico(myQuerySql, Session("conn"))
        dtgElencoFasiEnte.DataSource = Session("dtsCarica")
        dtgElencoFasiEnte.DataBind()

        PersonalizzaGriglia(Request.QueryString("VengoDa"))

    End Sub

    Private Sub PersonalizzaGriglia(ByVal vengoDa As String)

        Select Case Request.QueryString("VengoDa")
            Case Costanti.VENGO_DA_ADEGUAMENTO_VARIAZIONE_ENTE
                msgInfo.Visible = False
                dtgElencoFasiEnte.Columns(0).Visible = True 'variazioni ente
                dtgElencoFasiEnte.Columns(6).Visible = False 'adeguamento
                dtgElencoFasiEnte.Columns(7).Visible = False 'art.2
                dtgElencoFasiEnte.Columns(8).Visible = False 'art.10
                dtgElencoFasiEnte.Columns(9).Visible = False 'Inserisci Documenti Ente
                dtgElencoFasiEnte.Columns(10).Visible = False 'Consulta Documenti
                dtgElencoFasiEnte.Columns(11).Visible = False 'Presenta art.2
                dtgElencoFasiEnte.Columns(12).Visible = False 'Presenta art.10
                dtgElencoFasiEnte.Columns(13).Visible = False 'Stampa copertine
                dtgElencoFasiEnte.Columns(14).Visible = False 'Aggiorna Indirizzo Sedi ART 2
            Case Costanti.VENGO_DA_ADEGUAMENTO_ADEGUAMENTO_OK
                msgInfo.Visible = True
                msgInfo.Text = "Selezionare la Fase di Adeguamento che si vuole dichiarare conclusa."
                dtgElencoFasiEnte.Columns(0).Visible = False 'variazioni ente
                dtgElencoFasiEnte.Columns(6).Visible = True 'adeguamento
                dtgElencoFasiEnte.Columns(7).Visible = False 'art.2
                dtgElencoFasiEnte.Columns(8).Visible = False 'art.10
                dtgElencoFasiEnte.Columns(9).Visible = False 'Inserisci Documenti Ente
                dtgElencoFasiEnte.Columns(10).Visible = False 'Consulta Documenti
                dtgElencoFasiEnte.Columns(11).Visible = False 'Presenta art.2
                dtgElencoFasiEnte.Columns(12).Visible = False 'Presenta art.10
                dtgElencoFasiEnte.Columns(13).Visible = False 'Stampa copertine
                dtgElencoFasiEnte.Columns(14).Visible = False 'Aggiorna Indirizzo Sedi ART 2
            Case Costanti.VENGO_DA_ADEGUAMENTO_ARTICOLO_2
                msgInfo.Visible = True
                msgInfo.Text = "Selezionare la Fase di Adeguamento per la quale si vuole iniziare una Fase di Art.2."
                dtgElencoFasiEnte.Columns(0).Visible = False 'variazioni ente
                dtgElencoFasiEnte.Columns(7).Visible = True 'articolo 2
                dtgElencoFasiEnte.Columns(6).Visible = False 'art.2
                dtgElencoFasiEnte.Columns(8).Visible = False 'art.10
                dtgElencoFasiEnte.Columns(9).Visible = False 'Inserisci Documenti Ente
                dtgElencoFasiEnte.Columns(10).Visible = False 'Consulta Documenti
                dtgElencoFasiEnte.Columns(11).Visible = False 'Presenta art.2
                dtgElencoFasiEnte.Columns(12).Visible = False 'Presenta art.10
                dtgElencoFasiEnte.Columns(13).Visible = False 'Stampa copertine
                dtgElencoFasiEnte.Columns(14).Visible = False 'Aggiorna Indirizzo Sedi ART 2
            Case Costanti.VENGO_DA_ADEGUAMENTO_ARTICOLO_10
                msgInfo.Visible = True
                msgInfo.Text = "Selezionare la Fase di Adeguamento per la quale si vuole iniziare una Fase di Art.10."
                dtgElencoFasiEnte.Columns(0).Visible = False 'variazioni ente
                dtgElencoFasiEnte.Columns(8).Visible = True 'articolo 10
                dtgElencoFasiEnte.Columns(6).Visible = False 'art.2
                dtgElencoFasiEnte.Columns(7).Visible = False 'art.10
                dtgElencoFasiEnte.Columns(9).Visible = False 'Inserisci Documenti Ente
                dtgElencoFasiEnte.Columns(10).Visible = False 'Consulta Documenti
                dtgElencoFasiEnte.Columns(11).Visible = False 'Presenta art.2
                dtgElencoFasiEnte.Columns(12).Visible = False 'Presenta art.10
                dtgElencoFasiEnte.Columns(13).Visible = False 'Stampa copertine
                dtgElencoFasiEnte.Columns(14).Visible = False 'Aggiorna Indirizzo Sedi ART 2
            Case Costanti.VENGO_DA_ADEGUAMENTO_INSERISCI_DOCUMENTI
                msgInfo.Visible = False
                dtgElencoFasiEnte.Columns(0).Visible = False 'variazioni ente
                dtgElencoFasiEnte.Columns(6).Visible = False 'adeguamento
                dtgElencoFasiEnte.Columns(7).Visible = False 'art.2
                dtgElencoFasiEnte.Columns(8).Visible = False 'art.10
                dtgElencoFasiEnte.Columns(9).Visible = True 'Inserisci Documenti Ente
                dtgElencoFasiEnte.Columns(10).Visible = False 'Consulta Documenti
                dtgElencoFasiEnte.Columns(11).Visible = False 'Presenta art.2
                dtgElencoFasiEnte.Columns(12).Visible = False 'Presenta art.10
                dtgElencoFasiEnte.Columns(13).Visible = False 'Stampa copertine
                dtgElencoFasiEnte.Columns(14).Visible = False 'Aggiorna Indirizzo Sedi ART 2
            Case Costanti.VENGO_DA_ADEGUAMENTO_CONSULTA_DOCUMENTI
                msgInfo.Visible = False
                dtgElencoFasiEnte.Columns(0).Visible = False 'variazioni ente
                dtgElencoFasiEnte.Columns(6).Visible = False 'adeguamento
                dtgElencoFasiEnte.Columns(7).Visible = False 'art.2
                dtgElencoFasiEnte.Columns(8).Visible = False 'art.10
                dtgElencoFasiEnte.Columns(9).Visible = False 'Inserisci Documenti Ente
                dtgElencoFasiEnte.Columns(10).Visible = True 'Consulta Documenti
                dtgElencoFasiEnte.Columns(11).Visible = False 'Presenta art.2
                dtgElencoFasiEnte.Columns(12).Visible = False 'Presenta art.10
                dtgElencoFasiEnte.Columns(13).Visible = True 'Stampa copertine
                dtgElencoFasiEnte.Columns(14).Visible = False 'Aggiorna Indirizzo Sedi ART 2
            Case Costanti.VENGO_DA_ADEGUAMENTO_PRESENTA_ARTICOLO_2
                msgInfo.Visible = True
                msgInfo.Text = "Selezionare la Fase di Art.2 che si vuole presentare."
                dtgElencoFasiEnte.Columns(0).Visible = False 'variazioni ente
                dtgElencoFasiEnte.Columns(6).Visible = False 'adeguamento
                dtgElencoFasiEnte.Columns(7).Visible = False 'art.2
                dtgElencoFasiEnte.Columns(8).Visible = False 'art.10
                dtgElencoFasiEnte.Columns(9).Visible = False 'Inserisci Documenti Ente
                dtgElencoFasiEnte.Columns(10).Visible = False 'Consulta Documenti
                dtgElencoFasiEnte.Columns(11).Visible = True 'Presenta art.2
                dtgElencoFasiEnte.Columns(12).Visible = False 'Presenta art.10
                dtgElencoFasiEnte.Columns(13).Visible = False 'Stampa copertine
                dtgElencoFasiEnte.Columns(14).Visible = False 'Aggiorna Indirizzo Sedi ART 2
            Case Costanti.VENGO_DA_ADEGUAMENTO_PRESENTA_ARTICOLO_10
                msgInfo.Visible = True
                msgInfo.Text = "Selezionare la Fase di Art.10 che si vuole presentare."
                dtgElencoFasiEnte.Columns(0).Visible = False 'variazioni ente
                dtgElencoFasiEnte.Columns(6).Visible = False 'adeguamento
                dtgElencoFasiEnte.Columns(7).Visible = False 'art.2
                dtgElencoFasiEnte.Columns(8).Visible = False 'art.10
                dtgElencoFasiEnte.Columns(9).Visible = False 'Inserisci Documenti Ente
                dtgElencoFasiEnte.Columns(10).Visible = False 'Consulta Documenti
                dtgElencoFasiEnte.Columns(11).Visible = False 'Presenta art.2
                dtgElencoFasiEnte.Columns(12).Visible = True 'Presenta art.10
                dtgElencoFasiEnte.Columns(13).Visible = False 'Stampa copertine
                dtgElencoFasiEnte.Columns(14).Visible = False 'Aggiorna Indirizzo Sedi ART 2

            Case Costanti.VENGO_DA_AGGIORNA_SEDI_ARTICOLO_2
                msgInfo.Visible = True
                msgInfo.Text = "Selezionare la Fase di Art.2 che si vuole modificare."
                dtgElencoFasiEnte.Columns(0).Visible = False 'variazioni ente
                dtgElencoFasiEnte.Columns(7).Visible = False 'articolo 2
                dtgElencoFasiEnte.Columns(6).Visible = False 'art.2
                dtgElencoFasiEnte.Columns(8).Visible = False 'art.10
                dtgElencoFasiEnte.Columns(9).Visible = False 'Inserisci Documenti Ente
                dtgElencoFasiEnte.Columns(10).Visible = False 'Consulta Documenti
                dtgElencoFasiEnte.Columns(11).Visible = False 'Presenta art.2
                dtgElencoFasiEnte.Columns(12).Visible = False 'Presenta art.10
                dtgElencoFasiEnte.Columns(13).Visible = False 'Stampa copertine
                dtgElencoFasiEnte.Columns(14).Visible = True 'Aggiorna Indirizzo Sedi ART 2

        End Select

    End Sub

    Private Sub Store_Verifica_Valutazione_Adeguamento(ByVal IdEnteFase As Integer, Optional ByRef msg As String = "", Optional ByRef Esito As String = "")
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_VERIFICA_VALUTAZIONE_ADEGUAMENTO]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEnteFase", SqlDbType.Int).Value = IdEnteFase

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Motivazione"
            sparam2.Size = 1000
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            msg = sqlCMD.Parameters("@Motivazione").Value
            Esito = sqlCMD.Parameters("@Esito").Value
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Private Sub Store_Adeguamento(ByVal IdEnteFase As Integer, ByVal FlagForzatura As Byte, Optional ByRef msg As String = "", Optional ByRef Esito As String = "")
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_ADEGUAMENTO_OK]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEnteFase", SqlDbType.Int).Value = IdEnteFase
            sqlCMD.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")
            sqlCMD.Parameters.Add("@FlagForzatura", SqlDbType.TinyInt).Value = FlagForzatura


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
            msg = sqlCMD.Parameters("@Messaggio").Value
            Esito = sqlCMD.Parameters("@Esito").Value
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try

    End Sub

    Private Sub PresentaArt2(ByVal IdEnteFase As Integer)
        'ChiudiFaseEnte(Session("IdEnte"))
        Dim strMg As String
        Dim esito As Integer
        ClsServer.ChiudiFaseEnte(IdEnteFase, Session("Utente"), Session("conn"), strMg, esito)
        If esito = 0 Then
            msgErrore.Text = strMg
            Exit Sub
        Else
            CaricaGriglia()
            If AppSettings("IsTest") <> "1" Then GenerazioneAllegato6_ElencoSedi(IdEnteFase)
            Response.Redirect("WfrmInfoPresentazioneAccreditamento.aspx?IDEnteFase=" & IdEnteFase & "")
            'StampaCopertina(IdEnteFase)
        End If
        'ClsServer.ChiudiFaseEnte(IdEnteFase, Session("Utente"), Session("conn"))

    End Sub

    Private Sub PresentaArt10(ByVal IdEnteFase As Integer)
        Dim strMg As String
        Dim esito As Integer
        ClsServer.ChiudiFaseEnte(IdEnteFase, Session("Utente"), Session("conn"), strMg, esito)
        If esito = 0 Then
            msgErrore.Text = strMg
            Exit Sub
        Else
            CaricaGriglia()
            If AppSettings("IsTest") <> "1" Then StampaCopertina(IdEnteFase)
        End If
        'ChiudiFaseEnte(Session("IdEnte"))
        ' ClsServer.ChiudiFaseEnte(IdEnteFase, Session("Utente"), Session("conn"))
       
    End Sub

    Sub StampaCopertina(ByVal idEnteFase As Integer)
        Dim numDocumenti As Integer
        Dim script As String

        myQuerySql = " Select count(isnull(identedocumento,0)) as NumDocumenti from EntiDocumenti where IdEnteFase=" & idEnteFase & ""
        ChiudiDataReader(myDataReader)
        myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
        myDataReader.Read()
        numDocumenti = myDataReader("NumDocumenti")

        ChiudiDataReader(myDataReader)
        script = "<script>" & vbCrLf
        If numDocumenti > 0 Then 'copertina con elenco documenti
            script &= "window.open(""WfrmReportistica.aspx?sTipoStampa=39&IDEnteFase=" & idEnteFase & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
        Else
            script &= "window.open(""WfrmReportistica.aspx?sTipoStampa=40&IDEnteFase=" & idEnteFase & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
        End If
        script &= ("</script>")
        Response.Write(script)
    End Sub

    Private Sub dtgElencoFasiEnte_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgElencoFasiEnte.ItemCommand
        Dim tipologia As String = Request.QueryString("tipologia")
        Select Case e.CommandName
            Case "Select"
                Response.Redirect("wfrmVariazioniEnti.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdEnteFase=" & e.Item.Cells(2).Text & "&tipologia=" & tipologia)
            Case "Adeguamento"
                If e.Item.Cells(3).Text <> "Presentata" Then
                    msgErrore.Text = "Impossibile adeguare la Fase perchè risulta nello stato di " & e.Item.Cells(3).Text & "."
                    Exit Sub
                Else
                    Dim strMsg As String
                    Dim esito, esitoAdeguamento As String
                    Store_Verifica_Valutazione_Adeguamento(e.Item.Cells(2).Text, strMsg, esito)
                    If esito = 1 Then
                        Store_Adeguamento(e.Item.Cells(2).Text, Request.QueryString("Forzatura"), strMsg, esitoAdeguamento)

                        If esitoAdeguamento = 0 Then
                            msgErrore.Text = strMsg
                        Else
                            msgConferma.Text = strMsg
                            CaricaGriglia()
                        End If
                    Else
                        msgErrore.Text = strMsg
                    End If
                End If
            Case "Articolo2"
                If e.Item.Cells(3).Text <> "Presentata" Then
                    msgErrore.Text = "Impossibile richiedere un Articolo2 per la Fase perchè risulta nello stato di " & e.Item.Cells(3).Text & "."
                    Exit Sub
                Else
                    Dim RitornoFaseRiferimento As String
                    If ControlloApeturaFase(e.Item.Cells(2).Text, 3, RitornoFaseRiferimento) = True Then
                        msgErrore.Text = "Attenzione. Per questa richiesta di Adeguamento è stata già aperta una Fase di Articolo2 (" & RitornoFaseRiferimento & ")."
                        Exit Sub
                    Else
                        InizioArticolo2(e.Item.Cells(2).Text)
                        CaricaGriglia()
                        msgErrore.Text = "La Fase Art.2 è stata aperta."
                    End If
                End If

            Case "Articolo10"
                If e.Item.Cells(3).Text <> "Presentata" Then
                    msgErrore.Text = "Impossibile richiedere un Articolo10 per la Fase perchè risulta nello stato di " & e.Item.Cells(3).Text & "."
                    Exit Sub
                Else
                    Dim RitornoFaseRiferimento As String
                    If ControlloApeturaFase(e.Item.Cells(2).Text, 4, RitornoFaseRiferimento) = True Then
                        msgErrore.Text = "Attenzione. Per questa richiesta di Adeguamento è stata già aperta una Fase di Articolo10 (" & RitornoFaseRiferimento & ")."
                        Exit Sub
                    Else
                        InizioArticolo10(e.Item.Cells(2).Text)
                        CaricaGriglia()
                        msgErrore.Text = "La Fase Art.10 è stata aperta."
                    End If
                End If
            Case "DocumentiEnte"
                If e.Item.Cells(3).Text <> "Aperta" Then
                    msgErrore.Text = "Impossibile caricare i Documenti per la Fase perchè risulta nello stato di " & e.Item.Cells(3).Text & "."
                    Exit Sub
                Else
                    Response.Redirect("wfrmDocumentiEnti.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdEnteFase=" & e.Item.Cells(2).Text & "&tipologia=" & tipologia)
                End If
            Case "ConsultaDocumenti"
                Response.Redirect("wfrmConsultaDocumenti.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdEnteFase=" & e.Item.Cells(2).Text & "&tipologia=" & tipologia)
            Case "PresentaArt2"
                If e.Item.Cells(3).Text <> "Aperta" Then
                    msgErrore.Text = "Impossibile presentare la Fase perchè risulta nello stato di " & e.Item.Cells(3).Text & "."
                    Exit Sub
                Else
                    PresentaArt2(e.Item.Cells(2).Text)
                    CaricaGriglia()
                    msgErrore.Text = "La Fase Art.2 è stata presentata. Si ricorda di inviare via Pec al Dipartimento il documento riepilogativo dell'avvenuta presentazione della fase."
                End If
            Case "PresentaArt10"
                If e.Item.Cells(3).Text <> "Aperta" Then
                    msgErrore.Text = "Impossibile presentare la Fase perchè risulta nello stato di " & e.Item.Cells(3).Text & "."
                    Exit Sub
                Else
                    PresentaArt10(e.Item.Cells(2).Text)
                    CaricaGriglia()
                    msgErrore.Text = "La Fase Art.10 è stata presentata. Si ricorda di inviare via Pec al Dipartimento il documento riepilogativo dell'avvenuta presentazione della fase."
                End If
            Case "Stampa" '1: Aperta 2: Annullata 3: Presentata 4: Valutata
                If (e.Item.Cells(3).Text = "Presentata" Or e.Item.Cells(3).Text = "Valutata") Then
                    StampaCopertina(e.Item.Cells(2).Text)
                Else 'aperta/annullata o scaduta
                    msgErrore.Text = "Impossibile Stampare il documento riassuntivo perchè la fase risulta nello stato di " & e.Item.Cells(3).Text & "."
                    Exit Sub
                End If

            Case "AggiornaIndirizzoSediART2"
                Response.Redirect("wfrmAggiornaSediArt2Bis.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdEnteFase=" & e.Item.Cells(2).Text & "&tipologia=" & tipologia)

        End Select
    End Sub
    Private Sub InizioArticolo2(ByVal IdEnteFaseRiferimento As Integer)
        Dim cmdAggiornaDB As Data.SqlClient.SqlCommand
        Dim valore As Integer

        ChiudiDataReader(myDataReader)
        myQuerySql = "select valore from configurazioni where parametro = 'DURATA_ART2'"
        myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
        myDataReader.Read()
        valore = myDataReader("valore")
        ChiudiDataReader(myDataReader)
        myQuerySql = "Insert Into EntiFasi (IdEnte,TipoFase,DataInizioFase,DataFineFase,Stato,UserNameInizioFase,IdEnteFaseRiferimento) " & _
                 " Values (" & Session("IdEnte") & ",3,GetDate(), DATEADD(S,-1,DATEADD(D,convert(int," & valore & ")+1,DBO.FORMATODATADT(GetDate()))),1,'" & Session("Utente") & "'," & IdEnteFaseRiferimento & ")"
        cmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        cmdAggiornaDB.ExecuteNonQuery()

    End Sub

    Private Sub InizioArticolo10(ByVal IdEnteFaseRiferimento As Integer)
        Dim cmdAggiornaDB As Data.SqlClient.SqlCommand
        Dim valore As Integer
        ChiudiDataReader(myDataReader)
        myQuerySql = "select valore from configurazioni where parametro = 'DURATA_ART10'"
        myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
        myDataReader.Read()
        valore = myDataReader("valore")

        ChiudiDataReader(myDataReader)
        myQuerySql = "Insert Into EntiFasi (IdEnte,TipoFase,DataInizioFase,DataFineFase,Stato,UserNameInizioFase,IdEnteFaseRiferimento) Values (" & Session("IdEnte") & ",4,GetDate(), DATEADD(S,-1,DATEADD(D,convert(int," & valore & ")+1,DBO.FORMATODATADT(GetDate()))),1,'" & Session("Utente") & "'," & IdEnteFaseRiferimento & ")"
        cmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        cmdAggiornaDB.ExecuteNonQuery()

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Dim tipologia As String = Request.QueryString("tipologia")
        Response.Redirect("WfrmAlbero.aspx?tipologia=" & tipologia)
    End Sub

    Private Sub dtgElencoFasiEnte_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgElencoFasiEnte.PageIndexChanged
        dtgElencoFasiEnte.SelectedIndex = -1
        dtgElencoFasiEnte.EditItemIndex = -1
        dtgElencoFasiEnte.CurrentPageIndex = e.NewPageIndex
        CaricaGriglia()
    End Sub

    Private Function ControlloApeturaFase(ByVal IdEnteFase As Integer, ByVal TipoFase As Integer, ByRef RitornoFaseRiferimento As String) As Boolean
        Dim cmdAggiornaDB As Data.SqlClient.SqlCommand
        Dim valore As Integer
        ChiudiDataReader(myDataReader)

        myQuerySql = "SELECT IdEnteFase " & _
                     " FROM EntiFasi WHERE  Stato=1 AND IdEnteFaseRiferimento = " & IdEnteFase & " and GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase AND TipoFase = " & TipoFase & " "
        myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
        If myDataReader.HasRows = False Then
            ControlloApeturaFase = False
        Else
            myDataReader.Read()
            RitornoFaseRiferimento = "Rif. Fase" & " " & myDataReader("IdEnteFase") & ""
            ControlloApeturaFase = True
        End If
        ChiudiDataReader(myDataReader)
    End Function
    Private Sub GenerazioneAllegato6_ElencoSedi(ByVal IdEnteFase As Integer)

        Dim localWS As New WS_Editor.WSMetodiDocumentazione
        Dim ds As DataSet
        Dim i As Integer
        Dim strCodiceProgetto As String
        Dim ResultAsinc As IAsyncResult

        Dim cmdUp As SqlCommand

        'update InLavorazione a 1 
        'aggiunto flag LAVORAZIONE =1 da simona cordella il 02/01/2018
        cmdUp = New Data.SqlClient.SqlCommand(" UPDATE EntiFasi" & _
                                              " SET InLavorazione= 1  " & _
                                              " WHERE IdEnteFase=" & IdEnteFase & "", Session("conn"))
        cmdUp.ExecuteNonQuery()
        cmdUp.Dispose()



        'richiamo WSDocumentazione
        localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")
        localWS.Timeout = 1000000




        ResultAsinc = localWS.BeginGenerazioneAllegato6_ElencoSedi(IdEnteFase, Session("Utente"), Nothing, "")



    End Sub
End Class