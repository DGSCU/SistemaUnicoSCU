Imports System.Data.SqlClient
Public Class WfrmVerGestioneBandiRequisiti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            LoadBandiDisponibili()
            LoadVersioneAssociate(Request.QueryString("IDVersioneVerifiche"))
        End If
    End Sub
    '***************** function GESTIONE MASCHERA ****************************
    Sub LoadBandiDisponibili()
        Dim strSql As String
        Dim dtsBandoDisponibili As DataSet

        'strSql = "Select  BandoBreve,Gruppo,IDBando,anno from bando " & _
        '         " where IDVersioneVerifiche is null  order by anno desc "


        strSql = "SELECT DISTINCT  bando.BandoBreve,bando.Gruppo,bando.IDBando,bando.anno,bando.annobreve "
        strSql = strSql & " FROM bando"
        strSql = strSql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strSql = strSql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strSql = strSql & " WHERE IDVersioneVerifiche is null  and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strSql = strSql & " ORDER BY bando.annobreve desc"
        dtsBandoDisponibili = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dtgElencoBandiDisponibili.DataSource = dtsBandoDisponibili
        dtgElencoBandiDisponibili.DataBind()
        Session("DtBandiDisponibili") = dtsBandoDisponibili.Tables(0)
    End Sub

    Sub LoadVersioneAssociate(ByVal IDVersioneVerifiche As Integer)
        'richiamo la store per la visualizzazione dei bandi associati alla versione
        Dim dtBandoAssociato As DataTable


        Dim i As Integer
        Dim chkValore As CheckBox
        Dim dtBando As New DataTable
        Dim drBando As DataRow

        dtBandoAssociato = SP_VER_VERSIONEBANDI_ASSOCIATI(IDVersioneVerifiche)
        If Session("dtBando") Is Nothing Then 'creo la datatable
            Session("dtBando") = New DataTable
            Session("dtBando").Columns.Add(New DataColumn("BandoBreve", GetType(String)))
            Session("dtBando").Columns.Add(New DataColumn("Gruppo", GetType(String)))
            Session("dtBando").Columns.Add(New DataColumn("IdBando", GetType(String)))
            Session("dtBando").Columns.Add(New DataColumn("IDVersioneVerifiche", GetType(String)))
        End If


        For i = 1 To dtBandoAssociato.Rows.Count
            drBando = Session("dtBando").NewRow()
            drBando(0) = dtBandoAssociato.Rows(i - 1).Item(2) '"bandobreve"
            drBando(1) = dtBandoAssociato.Rows(i - 1).Item(3)   '"gruppo"
            drBando(2) = dtBandoAssociato.Rows(i - 1).Item(1)  '"idbando"
            drBando(3) = dtBandoAssociato.Rows(i - 1).Item(0)  'IDVersioneVerifiche
            Session("dtBando").Rows.Add(drBando)
        Next

        If dtBandoAssociato.Rows.Count > 0 Then
            dtgElencoBandiAssociati.DataSource = Session("dtBando")
            dtgElencoBandiAssociati.DataBind()
        End If

    End Sub

    Private Function SP_VER_VERSIONEBANDI_ASSOCIATI(ByVal IDVersioneVerifiche As Integer) As DataTable
        'eseguo store
        Dim sqlCMD As New SqlCommand
        Dim sqlAdapter As SqlClient.SqlDataAdapter
        Dim DsElencoReq As DataSet = New DataSet
        Dim DtElencoReq As DataTable
        Dim strNomeStore As String = "SP_VER_VERSIONEBANDI_ASSOCIATI"
        Try
            sqlAdapter = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlAdapter.SelectCommand.Parameters.Add("@IDVersione", SqlDbType.Int).Value = IDVersioneVerifiche
            sqlAdapter.SelectCommand.Parameters.Add("@MacroTipoProgetto", SqlDbType.VarChar).Value = Session("FiltroVisibilita")
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Versione"
            sparam2.Size = 400
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam2)

            sqlAdapter.Fill(DsElencoReq)
            DtElencoReq = DsElencoReq.Tables(0)
            If IsDBNull(sqlAdapter.SelectCommand.Parameters("@Versione").Value) Then
                txtDescrizione.Text = ""
            Else
                txtDescrizione.Text = sqlAdapter.SelectCommand.Parameters("@Versione").Value
            End If
            Return DtElencoReq
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Sub AssociaBando()
        'funzione che associa tutti i bandi
        'vengono riportati in automatio tutti i bandi che appartengono allo stesso gruppo
        Dim ContaCheck As Integer = 0
        Dim Item As DataGridItem
        Dim chkValore As CheckBox
        Dim dtBando As New DataTable
        Dim drBando As DataRow
        Dim strSql As String
        Dim dtrBandoGruppo As SqlDataReader
        Dim intGruppo As Integer = 0

        If Session("dtBando") Is Nothing Then 'creo la datatable
            Session("dtBando") = New DataTable
            Session("dtBando").Columns.Add(New DataColumn("BandoBreve", GetType(String)))
            Session("dtBando").Columns.Add(New DataColumn("Gruppo", GetType(String)))
            Session("dtBando").Columns.Add(New DataColumn("IdBando", GetType(String)))
            Session("dtBando").Columns.Add(New DataColumn("IDVersioneVerifiche", GetType(String)))
        End If
        ' salvo i data in un datatable e carico la griglia dei bandi associati
        For Each Item In dtgElencoBandiDisponibili.Items
            chkValore = Item.FindControl("chkbando")
            If chkValore.Checked = True Then
                ContaCheck = ContaCheck + 1
                If intGruppo <> Item.Cells(2).Text Then
                    intGruppo = Item.Cells(2).Text
                    strSql = "Select BandoBreve,Gruppo,IdBando from Bando where gruppo =" & Item.Cells(2).Text
                    dtrBandoGruppo = ClsServer.CreaDatareader(strSql, Session("Conn"))
                    If dtrBandoGruppo.HasRows = True Then
                        Do While dtrBandoGruppo.Read()
                            drBando = Session("dtBando").NewRow()
                            drBando(0) = "" & dtrBandoGruppo("BandoBreve")  '"bando"
                            drBando(1) = "" & dtrBandoGruppo("Gruppo")      '"gruppo"
                            drBando(2) = "" & dtrBandoGruppo("IdBando")     '"idbando"
                            drBando(3) = ""                                 '"idversioneverifiche"
                            Session("dtBando").Rows.Add(drBando)
                        Loop
                    End If
                    If Not dtrBandoGruppo Is Nothing Then
                        dtrBandoGruppo.Close()
                        dtrBandoGruppo = Nothing
                    End If
                End If
                'chkValore.Enabled = False
            End If
        Next
        If Not dtrBandoGruppo Is Nothing Then
            dtrBandoGruppo.Close()
            dtrBandoGruppo = Nothing
        End If
        If ContaCheck = 0 Then 'controllo se sono stati ceccati i valori nella lista
            lblmessaggio.Text = "Non è stato selezionato nessun bando da associare alla versione."
        Else
            dtgElencoBandiAssociati.DataSource = Session("dtBando")
            dtgElencoBandiAssociati.DataBind()
            dtgElencoBandiAssociati.SelectedIndex = -1
        End If

    End Sub

    Sub RemoveBandiAssociati()
        Dim i As Integer
        Dim bd As Integer
        Dim Item As DataGridItem
        Dim dtBando As New DataTable
        Dim drBando As DataRow
        Dim strSql As String


        If Session("dtBando") Is Nothing Then 'creo la datatable
            Session("dtBando") = New DataTable
            Session("dtBando").Columns.Add(New DataColumn("BandoBreve", GetType(String)))
            Session("dtBando").Columns.Add(New DataColumn("Gruppo", GetType(String)))
            Session("dtBando").Columns.Add(New DataColumn("IdBando", GetType(String)))
            Session("dtBando").Columns.Add(New DataColumn("IDVersioneVerifiche", GetType(String)))
        End If
        Dim intIdBando As Integer
        For bd = 1 To Session("DtBandiDisponibili").Rows.Count
            intIdBando = Session("DtBandiDisponibili").Rows(bd - 1).Item(2) '"idbando"
            For i = 1 To Session("dtBando").Rows.Count
                If intIdBando = Session("dtBando").Rows(i - 1).Item(2) Then
                    Session("DtBandiDisponibili").Rows(bd - 1).Delete()
                End If
            Next
        Next

        dtgElencoBandiDisponibili.DataSource = Session("DtBandiDisponibili")
        dtgElencoBandiDisponibili.DataBind()
        dtgElencoBandiDisponibili.SelectedIndex = -1
    End Sub

    Sub UpdateAssociazioneBando(ByVal IDVersioneVerifiche As Integer, ByVal IdBando As Integer)
        'update nella tabella Bando
        Dim cmdSalva As SqlCommand
        Dim strSql As String

        Try
            strSql = " UPDATE Bando SET  " & _
                     " IDVersioneVerifiche =" & IDVersioneVerifiche & " " & _
                     " WHERE IdBando = " & IdBando
            cmdSalva = ClsServer.EseguiSqlClient(strSql, Session("conn"))

            lblmessaggio.Text = "Salvataggio eseguito con successo."

        Catch ex As Exception
            lblmessaggio.Text = "Errore imprevisto.Contattare l'assistenza."
            'Response.Write(ex.Message.ToString())
        End Try
    End Sub
    Sub UpdateStatoVersione(ByVal IDVersioneVerifiche As Integer)
        'update nella tabella versione 'STATO ASSOCIATO
        Dim cmdSalva As SqlCommand
        Dim strSql As String

        Try
            strSql = " UPDATE TVERIFICHEVERSIONI SET  " & _
                     " Stato = 1 " & _
                     " WHERE IDVersioneVerifiche =" & IDVersioneVerifiche
            cmdSalva = ClsServer.EseguiSqlClient(strSql, Session("conn"))

            lblmessaggio.Text = "Salvataggio eseguito con successo."

        Catch ex As Exception
            lblmessaggio.Text = "Errore imprevisto.Contattare l'assistenza."
            'Response.Write(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub cmdAssocia_Click(sender As Object, e As EventArgs) Handles cmdAssocia.Click
        AssociaBando()
        RemoveBandiAssociati()
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim Item As DataGridItem
        For Each Item In dtgElencoBandiAssociati.Items
            UpdateAssociazioneBando(Request.QueryString("IDVersioneVerifiche"), Item.Cells(2).Text())
        Next
        UpdateStatoVersione(Request.QueryString("IDVersioneVerifiche"))
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'Session("dtsBandoDisponibili") = Nothing
        Session("dtBando") = Nothing
        Response.Redirect("WfrmVerVersioni.aspx")
    End Sub

  
End Class