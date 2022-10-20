Imports System.Data.SqlClient
Public Class WfrmGestioneRequisiti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim intIdRequisito As Integer = 0
        If Not Me.Page.IsPostBack Then
            CaricaCompetenze()
            clsGui.CaricaDropDown(Me.ddSanzione, clsRequisitoVerifiche.RecuperaTipoSanzioni(Session("conn")), "Descrizione", "IdSanzione")
            clsGui.CaricaDropDown(Me.ddTipoRequisito, clsRequisitoVerifiche.RecuperaTipoRequisiti(Session("conn")), "TipoRequisito", "IdTipoRequisito")

            If Request.QueryString("TipoAzione") <> "Inserimento" Then
                intIdRequisito = Request.QueryString("IdRequisito")
            End If
            LoadMaschera(intIdRequisito)


            StatoVersione(Request.QueryString("StatoVersione"))

            If Request.QueryString("IdEsito") <> Nothing Then
                ImpostaCheckEsito(Request.QueryString("IdEsito"))
            End If
        End If
    End Sub
    Sub LoadMaschera(ByVal intIdRequisito As Integer)
        Dim dsReq As DataSet = New DataSet
        dsReq = SP_VER_REQUISITIESITI(intIdRequisito, Session("Utente"))
        dtgEsiti.DataSource = dsReq
        dtgEsiti.DataBind()
    End Sub
   
    Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query
                strSQL = " Select IdRegioneCompetenza,case when Descrizione ='Nazionale' then UPPER(Descrizione) ELSE Descrizione end AS Descrizione,CodiceRegioneCompetenza "
                strSQL = strSQL & " from RegioniCompetenze "
                strSQL = strSQL & " ORDER BY CASE WHEN left(CodiceRegioneCompetenza,1)='N' then 1 else 2 end,descrizione "
                ''strSQL = strSQL & " union "
                ''trSQL = strSQL & " select '0',' TUTTI ','','A' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '',' NAZIONALE ','','B' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                ''strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlCompetenza.DataSource = dtrCompetenze
                ddlCompetenza.Items.Add("")
                ddlCompetenza.DataTextField = "Descrizione"
                ddlCompetenza.DataValueField = "IDRegioneCompetenza"
                ddlCompetenza.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If

            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenza.Enabled = True
                ddlCompetenza.SelectedIndex = 0

            Else

                'CboCompetenza.SelectedIndex = 1
                'CboCompetenza.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenza.Enabled = True
                    End If
                End If

                If Session("TipoUtente") = "R" Then
                    ddlCompetenza.Enabled = False
                End If

            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub

    Private Function SP_VER_REQUISITIESITI(ByVal IdRequisito As Integer, ByVal Utente As String) As DataSet
        'eseguo store
        'Dim sqlCMD As New SqlCommand
        Dim sqlAdapter As SqlClient.SqlDataAdapter
        Dim DsElencoReq As DataSet = New DataSet
        'Dim DtElencoReq As DataTable
        Dim strNomeStore As String = "SP_VER_REQUISITIESITI"

        Try
            sqlAdapter = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlAdapter.SelectCommand.Parameters.Add("@IDRequisito", SqlDbType.Int).Value = IdRequisito
            sqlAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = Utente

            'REQUISITO
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Requisito"
            sparam1.Size = 400
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam1)

            'IDTIPOREQUISITO
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@IdTipoRequisito"
            sparam2.SqlDbType = SqlDbType.SmallInt
            sparam2.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam2)

            'TIPOREQUISITO
            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@TipoRequisito"
            sparam3.Size = 400
            sparam3.SqlDbType = SqlDbType.NVarChar
            sparam3.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam3)

            'RIFERIMENTO CIRCOLARE
            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@RiferimentoCircolare"
            sparam4.Size = 200
            sparam4.SqlDbType = SqlDbType.NVarChar
            sparam4.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam4)

            'ORDINE
            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@Ordine"
            sparam5.SqlDbType = SqlDbType.SmallInt
            sparam5.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam5)

            'NOTE
            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@Note"
            sparam6.Size = 1000
            sparam6.SqlDbType = SqlDbType.NVarChar
            sparam6.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam6)

            'RATIONALE
            Dim sparam7 As SqlClient.SqlParameter
            sparam7 = New SqlClient.SqlParameter
            sparam7.ParameterName = "@Rationale"
            sparam7.Size = 400
            sparam7.SqlDbType = SqlDbType.NVarChar
            sparam7.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam7)
            sqlAdapter.Fill(DsElencoReq)

            If Request.QueryString("TipoAzione") = "Dettaglio" Then
                txtDescrizione.Text = sqlAdapter.SelectCommand.Parameters("@Requisito").Value
                ddTipoRequisito.SelectedValue = sqlAdapter.SelectCommand.Parameters("@IdTipoRequisito").Value
                txtRiferimentoCircolare.Text = sqlAdapter.SelectCommand.Parameters("@RiferimentoCircolare").Value
                txtOrdine.Text = sqlAdapter.SelectCommand.Parameters("@Ordine").Value
                txtNote.Text = sqlAdapter.SelectCommand.Parameters("@Note").Value
                txtRationale.Text = sqlAdapter.SelectCommand.Parameters("@Rationale").Value
            End If

            Return DsElencoReq
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Sub StatoVersione(ByVal Stato As Integer)
        Select Case Stato
            Case 0 'registrata
                txtDescrizione.ReadOnly = False
                txtNote.ReadOnly = False
                txtOrdine.ReadOnly = False
                txtRationale.ReadOnly = False
                ddTipoRequisito.Enabled = True
                cmdConferma.Visible = True
                cmdInserimento.Visible = True
                'dtgEsiti.Enabled = True
                Blocca_SbloccaChec(True)
            Case 1 'associata 
                txtDescrizione.ReadOnly = True
                txtNote.ReadOnly = True
                txtOrdine.ReadOnly = True
                txtRationale.ReadOnly = True
                ddTipoRequisito.Enabled = False
                cmdConferma.Visible = False
                cmdInserimento.Visible = False
                'dtgEsiti.Enabled = False
                Blocca_SbloccaChec(False)
        End Select
    End Sub

    Private Function CheckEsito() As String
        'controllo è  stata checcato almeno un esito per l salvataggio
        Dim item As DataGridItem
        Dim Contackh As String = ""
        For Each item In dtgEsiti.Items
            If Request.Form("chkAssegna" & item.Cells(2).Text) = 1 Then
                Contackh &= item.Cells(2).Text & ";"
            End If
        Next
        Return Contackh
    End Function

    Private Function ImpostaCheckEsito(ByVal IdEsitoInserito As Integer)
        'controllo è  stata checcato almeno un esito per l salvataggio
        Dim item As DataGridItem
        Dim Contackh As String = ""

        For Each item In dtgEsiti.Items
            If item.Cells(2).Text = IdEsitoInserito Then
                If Request.Form("chkAssegna" & IdEsitoInserito) = 0 Then
                    item.Cells(0).Text = "<input type=checkbox  title='Selezione' onclick=javascript:ControllaCheckEsito('" & IdEsitoInserito & "')  value=1 name=chkAssegna" & IdEsitoInserito & "  id=chkAssegna" & IdEsitoInserito & " checked>"
                End If
            End If
        Next
    End Function

    Private Sub Blocca_SbloccaChec(ByVal Valore As Boolean)
        Dim item As DataGridItem

        For Each item In dtgEsiti.Items
            item.Cells(0).Enabled = Valore
        Next
    End Sub
   
    Protected Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        'If Request.QueryString("IdRequisito") = "" Then
        '    Response.Redirect("WfrmMain.aspx")
        'Else
        '    Response.Redirect("WfrmRicercaRequisitiVerifiche.aspx")
        'End If
        Response.Redirect("WfrmRicercaRequisitiVerifiche.aspx?TipoAzione=" + Request.QueryString("TipoAzione") + "&IDVersioneVerifiche=" + Request.QueryString("IDVersioneVerifiche") + "&StatoVersione=" + Request.QueryString("StatoVersione"))
    End Sub

    Protected Sub cmdInserimento_Click(sender As Object, e As EventArgs) Handles cmdInserimento.Click
        Response.Redirect("WfrmVerInserimentoEsiti.aspx?TipoAzione=" + Request.QueryString("TipoAzione") + "&&IDVersioneVerifiche=" + Request.QueryString("IDVersioneVerifiche") + "&IdRequisito=" + Request.QueryString("IdRequisito") + "&VengoDa=Requisito&StatoVersione=" & Request.QueryString("StatoVersione") & "")
    End Sub

    Protected Sub cmdConferma_Click(sender As Object, e As EventArgs) Handles cmdConferma.Click
        Dim Req As clsRequisitoVerifiche
        Dim Esito As String

        If txtDescrizione.Text = "" Then
            lblmessaggiosopra.Text = "Inserire la descrizione del requisito."
            lblmessaggiosopra.Visible = True
            txtDescrizione.Focus()
            Exit Sub
        End If
        If txtOrdine.Text = "" Then
            lblmessaggiosopra.Text = "Inserire il campo ordine."
            lblmessaggiosopra.Visible = True
            txtOrdine.Focus()
            Exit Sub
        End If
        If ddTipoRequisito.SelectedIndex = 0 Then
            lblmessaggiosopra.Text = "E' necessario indicare il Tipo Requisito."
            lblmessaggiosopra.Visible = True
            ddTipoRequisito.Focus()
            Exit Sub
        End If
        If txtRationale.Text = "" Then
            lblmessaggiosopra.Text = "E' necessario indicare il Rationale."
            lblmessaggiosopra.Visible = True
            txtRationale.Focus()
            Exit Sub
        End If
			

                    Req = Me.ViewState.Item("Requisito")

                    If Req Is Nothing Then
                        Req = New clsRequisitoVerifiche
                    End If
                    Req.Descrizione = Me.txtDescrizione.Text
                    If Me.ddAbilitato.SelectedItem.Text = "SI" Then
                        Req.Abilitato = Me.ddAbilitato.SelectedValue
                    Else
                        Req.Abilitato = Me.ddAbilitato.SelectedValue
                    End If
                    Req.IdVersioneVerifiche = Request.QueryString("IdVersioneVerifiche")
                    'If Me.ddSanzione.SelectedValue = "" Then
                    '    Response.Write("<script language=""javascript"">" & vbCrLf)
                    '    Response.Write("alert(""Inserire la Sanzione!!!"");" & vbCrLf)
                    '    Response.Write("</script>" & vbCrLf)
                    '    Exit Sub
                    'Else
                    '    Req.IdSanzione = Me.ddSanzione.SelectedValue
                    'End If
                    If Me.ddTipoRequisito.SelectedValue = "" Then
                        'Response.Write("<script language=""javascript"">" & vbCrLf)
                        'Response.Write("alert(""Inserire il Tipo Requisito!!!"");" & vbCrLf)
                        'Response.Write("</script>" & vbCrLf)
                        'Exit Sub
                    Else
                        Req.IdTipoRequisito = Me.ddTipoRequisito.SelectedValue
                    End If

                    Req.IdSanzione = 1
                    Req.Rationale = Me.txtRationale.Text
                    Req.RiferimentoCircolare = Me.txtRiferimentoCircolare.Text
                    Req.Note = Me.txtNote.Text
                    Req.Ordine = Me.txtOrdine.Text
                    Req.IdRegCompetenza = Me.ddlCompetenza.SelectedValue

                    Req.IdEsiti = CheckEsito()
                    If Req.IdEsiti = "" Then
                        Me.lblmessaggiosopra.Text = "E' necessario indicare almeno un Esito!!"
                        Me.lblmessaggiosopra.Visible = True
                        Exit Sub
                    End If
                    If Request.QueryString("TipoAzione") = "Dettaglio" Then
                        Req.UserUltimaModifica = Session("utente")
                        Req.IdRequisito = Request.QueryString("IdRequisito")
                        If Req.Modifica(Session("conn")) Then
                            'clsGui.SvuotaCampi(Me)
                            Me.cmdConferma.Visible = False
                            Me.lblmessaggiosopra.Text = "MODIFICA EFFETTUATA."
                            Me.lblmessaggiosopra.Visible = True
                            Me.ViewState.Remove("Requisito")
                            Me.ddlCompetenza.SelectedValue = Req.IdRegCompetenza
                            Me.LoadMaschera(Req.IdRequisito)
                        End If
                    Else
                        Req.UserInseritore = Session("utente")
                        If Req.Inserisci(Session("conn")) Then
                            clsGui.SvuotaCampi(Me)
                            Me.lblmessaggiosopra.Text = "INSERIMENTO EFFETTUATO."
                            Me.lblmessaggiosopra.Visible = True
                            Me.ddlCompetenza.SelectedValue = Req.IdRegCompetenza
                        End If
                    End If
    End Sub
End Class