Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Imports System.Data.SqlClient
Public Class WfrmProgrammi_AssociaRimuovi
    Inherits System.Web.UI.Page
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String
#Region "Utilità"

    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            CaricaCombo()

            Dim strSql As String
            strSql = "SELECT idprogramma,titolo from attività where idprogramma =  " & Request.QueryString("idProgramma")
            dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrGenerico.Read()
            If dtrGenerico.HasRows = True Then
                ddlAssocia.SelectedIndex = 2

            End If


            ChiudiDataReader(dtrGenerico)
            CaricaGriglia()

            strSql = "SELECT titolo from Programmi where idprogramma =  " & Request.QueryString("idProgramma")
            dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrGenerico.Read()
            If dtrGenerico.HasRows = True Then
                lblIntestazioneTitolo.Text = "RICERCA PROGETTI PER PROGRAMMA :" & " " & dtrGenerico("Titolo")
            End If
            ChiudiDataReader(dtrGenerico)


            If Session("TipoUtente") = "E" Then
                Dim abilitato As Integer
                abilitato = ClsUtility.LoadProgrammiAbilitaModificaEnte(Request.QueryString("idProgramma"), Session("Conn"))
                'blocco maschera se ente non abilitato o ente in sessione è coprogrammante
                If abilitato = 0 Or ClsUtility.ProgrammiLimitaFunzioniCoprogrammante(Request.QueryString("idProgramma"), Session("IdEnte"), Session("Conn")) Then
                    'bloccamaschera
                    dtgProgrammi_AssociaRimuovi.Columns(8).Visible = True
                    dtgProgrammi_AssociaRimuovi.Columns(9).Visible = False
                End If

            End If
            If Session("TipoUtente") = "R" Then
                dtgProgrammi_AssociaRimuovi.Columns(8).Visible = True
                dtgProgrammi_AssociaRimuovi.Columns(9).Visible = False
            End If
        Else

        End If
    End Sub

    Protected Sub CaricaGriglia()
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_PROGRAMMI_RICERCA_PROGETTI]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Session("Utente")
            sqlDAP.SelectCommand.Parameters.Add("@IdProgramma", SqlDbType.Int).Value = CInt(Request.QueryString("IdProgramma"))
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnteProponenteProgetto", SqlDbType.VarChar).Value = TxtCodPog.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@DenominazioneEnteProponenteProgetto", SqlDbType.VarChar).Value = txtEnteProponente.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@TitoloProgetto", SqlDbType.VarChar).Value = txtTitoloProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@IdMacroAmbitoAttività", SqlDbType.Int).Value = CInt(ddlMacroAmbito.SelectedValue)
            sqlDAP.SelectCommand.Parameters.Add("@DurataMesi", SqlDbType.Int).Value = CInt(ddlDurataMesi.SelectedValue)
            sqlDAP.SelectCommand.Parameters.Add("@ItaliaEstero", SqlDbType.VarChar).Value = ddlItaliaEstero.SelectedItem.Text
            sqlDAP.SelectCommand.Parameters.Add("@Regione", SqlDbType.VarChar).Value = txtRegione.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Provincia", SqlDbType.VarChar).Value = txtProvincia.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Comune", SqlDbType.VarChar).Value = txtComune.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Associato", SqlDbType.VarChar).Value = ddlAssocia.SelectedItem.Text

            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            dtgProgrammi_AssociaRimuovi.DataSource = dataSet
            dtgProgrammi_AssociaRimuovi.DataBind()





            'il numero 2 indica quante colonne iniziali non devono essere messe nel dataset per l'esportazione in csv
            'CaricaDataTablePerStampaDinamica(dataSet, 2)

            '******************************************************

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Sub CaricaComboMacroAmbitoAzione()
        Dim strSql As String
        strSql = "SELECT 0 as IDMacroAmbitoAttività,'' as MacroAmbitoAttività UNION " & _
                 "SELECT IDMacroAmbitoAttività,MacroAmbitoAttività from MacroAmbitiAttività order by 1 "
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlMacroAmbito.DataSource = dtrGenerico
            ddlMacroAmbito.DataTextField = "MacroAmbitoAttività"
            ddlMacroAmbito.DataValueField = "IDMacroAmbitoAttività"
            ddlMacroAmbito.DataBind()
        End If
        ChiudiDataReader(dtrGenerico)
    End Sub

    Private Sub CaricaComboDurata()
        Dim strSql As String

        ChiudiDataReader(dtrGenerico)
        strSql = " SELECT 0 as NumMesi,'' as nmesi UNION "
        strSql &= " SELECT nmesi as NumMesi ,convert(varchar,nmesi) as nmesi FROM TipiProgettoDettaglio "

        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlDurataMesi.DataSource = dtrGenerico
            ddlDurataMesi.DataTextField = "nmesi"
            ddlDurataMesi.DataValueField = "NumMesi"
            ddlDurataMesi.DataBind()
        End If
        ChiudiDataReader(dtrGenerico)
    End Sub
  
    Private Sub CaricaCombo()
        CaricaComboDurata()
        CaricaComboMacroAmbitoAzione()
        'if italia

    End Sub

    Protected Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        dtgProgrammi_AssociaRimuovi.CurrentPageIndex = 0
        lblmessaggio.Text = ""
        lblMessaggioConferma.Text = ""
        CaricaGriglia()
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmProgrammi.aspx?idProgramma=" & CInt(Request.QueryString("idProgramma")))
    End Sub
   
    Private Sub dtgProgrammi_AssociaRimuovi_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgProgrammi_AssociaRimuovi.ItemCommand
        Dim ArreyDiMessaggi() As String
        Dim ESITO As Integer = -1
        lblmessaggio.Text = ""
        lblMessaggioConferma.Text = ""
        If e.CommandName = "Includi" Then
            IdChekSelezionata.Value = dtgProgrammi_AssociaRimuovi.Items(e.Item.ItemIndex).Cells(0).Text
            'SP_PROGRAMMI_ASSOCIA_PROGETTO
            Dim SqlCmd As New SqlClient.SqlCommand

            Try
                SqlCmd.CommandText = "SP_PROGRAMMI_ASSOCIA_PROGETTO"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Connection = Session("Conn")

                SqlCmd.Parameters.Add("@IdProgramma ", SqlDbType.Int).Value = Request.QueryString("idProgramma")
                SqlCmd.Parameters.Add("@IdAttività ", SqlDbType.Int).Value = IdChekSelezionata.Value
                SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
                SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
                SqlCmd.Parameters("@Esito").Size = 10
                SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
                SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
                SqlCmd.Parameters("@messaggio").Size = 1000
                SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

                SqlCmd.ExecuteNonQuery()
                ESITO = SqlCmd.Parameters("@Esito").Value()
                If ESITO = 0 Then
                    MaintainScrollPositionOnPostBack = False
                    
                    ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(SqlCmd.Parameters("@messaggio").Value(), ":")

                    lblmessaggio.Text = ArreyDiMessaggi(0)
                    Dim MessErrore() As String
                    Dim NuovaStringa As String
                    For i = 0 To UBound(ArreyDiMessaggi)
                        If i = 0 Then
                            ArreyDiMessaggi.Clear(ArreyDiMessaggi, 0, 1)
                            'funziona

                            NuovaStringa = ArreyDiMessaggi(1)
                            ReDim MessErrore(0)
                            MessErrore(0) = NuovaStringa
                            ReDim Preserve MessErrore(0)
                      
                        End If
                    Next
                    ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(MessErrore(0), ".")
                    lblmessaggio.Text = lblmessaggio.Text & ":" & vbCrLf & "<br/>"
                    Dim rigadasplittare() As String

                    rigadasplittare = ClsUtility.CreaArrayMessaggi(MessErrore(0), "|")
                    For i = 0 To UBound(rigadasplittare) - 1
                        lblmessaggio.Text = lblmessaggio.Text & rigadasplittare(i) & vbCrLf & "<br/>"
                    Next
                    'lblmessaggio.Text = lblmessaggio.Text & ArreyDiMessaggi(0) & "|" & vbCrLf & "<br/>"
                Else
                    MaintainScrollPositionOnPostBack = True
                    lblMessaggioConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                    lblMessaggioConferma.Text = lblMessaggioConferma.Text & "<br/>"
                End If
            Catch ex As Exception
            lblmessaggio.Text = ex.Message
        Finally

        End Try


        End If
        If e.CommandName = "Escludi" Then
            IdChekSelezionata.Value = dtgProgrammi_AssociaRimuovi.Items(e.Item.ItemIndex).Cells(0).Text
            'SP_PROGRAMMI_RIMUOVI_PROGETTO
            Dim SqlCmd As New SqlClient.SqlCommand

            Try
                SqlCmd.CommandText = "SP_PROGRAMMI_RIMUOVI_PROGETTO"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Connection = Session("Conn")

                SqlCmd.Parameters.Add("@IdProgramma ", SqlDbType.Int).Value = Request.QueryString("idProgramma")
                SqlCmd.Parameters.Add("@IdAttività ", SqlDbType.Int).Value = IdChekSelezionata.Value
                SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
                SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
                SqlCmd.Parameters("@Esito").Size = 10
                SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
                SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
                SqlCmd.Parameters("@messaggio").Size = 1000
                SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

                SqlCmd.ExecuteNonQuery()

                ESITO = SqlCmd.Parameters("@Esito").Value()
                If ESITO = 0 Then
                    MaintainScrollPositionOnPostBack = False

                    ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(SqlCmd.Parameters("@messaggio").Value(), ":")

                    lblmessaggio.Text = ArreyDiMessaggi(0)
                    Dim MessErrore() As String
                    Dim NuovaStringa As String
                    For i = 0 To UBound(ArreyDiMessaggi)
                        If i = 0 Then
                            ArreyDiMessaggi.Clear(ArreyDiMessaggi, 0, 1)
                            'funziona

                            NuovaStringa = ArreyDiMessaggi(1)
                            ReDim MessErrore(0)
                            MessErrore(0) = NuovaStringa
                            ReDim Preserve MessErrore(0)

                        End If
                    Next
                    ArreyDiMessaggi = ClsUtility.CreaArrayMessaggi(MessErrore(0), ".")
                    lblmessaggio.Text = lblmessaggio.Text & ":" & vbCrLf & "<br/>"
                    Dim rigadasplittare() As String

                    rigadasplittare = ClsUtility.CreaArrayMessaggi(MessErrore(0), "|")
                    For i = 0 To UBound(rigadasplittare) - 1
                        lblmessaggio.Text = lblmessaggio.Text & rigadasplittare(i) & vbCrLf & "<br/>"
                    Next
                Else
                    MaintainScrollPositionOnPostBack = True
                    lblMessaggioConferma.Text = SqlCmd.Parameters("@messaggio").Value()
                    lblMessaggioConferma.Text = lblMessaggioConferma.Text & "<br/>"
                End If

            Catch ex As Exception
                lblmessaggio.Text = ex.Message
            Finally

            End Try

        End If
        CaricaGriglia()
    End Sub

    Private Sub dtgProgrammi_AssociaRimuovi_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgProgrammi_AssociaRimuovi.PageIndexChanged
        dtgProgrammi_AssociaRimuovi.SelectedIndex = -1
        dtgProgrammi_AssociaRimuovi.EditItemIndex = -1
        dtgProgrammi_AssociaRimuovi.CurrentPageIndex = e.NewPageIndex
        dtgProgrammi_AssociaRimuovi.DataSource = Session("appDtsRisRicerca")
        dtgProgrammi_AssociaRimuovi.DataBind()
    End Sub

   
End Class