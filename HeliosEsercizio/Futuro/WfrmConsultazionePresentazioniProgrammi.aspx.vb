Imports Logger.Data
Public Class WfrmConsultazionePresentazioniProgrammi
    Inherits SmartPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then 'verifico validità log-in
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Not IsPostBack Then
            CaricaAvvisi()
        End If

    End Sub

    Sub CaricaAvvisi()
        Dim strquery As String
        Dim dtrCompetenze As SqlClient.SqlDataReader
        '*****Carico Combo Bandi Circolari

        strquery = " SELECT Bando.idBando,bando.bandobreve,bando.annobreve  "
        strquery = strquery & " FROM bando"
        strquery = strquery & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strquery = strquery & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strquery = strquery & " WHERE bando.programmi = 1 and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strquery = strquery & " UNION "
        strquery = strquery & " SELECT  '0',' TUTTI ', 99  from bando "
        strquery = strquery & " ORDER BY Bando.annobreve desc"

        dtrCompetenze = ClsServer.CreaDatareader(strquery, Session("conn"))
        DdlBando.DataSource = dtrCompetenze
        DdlBando.DataTextField = "bandobreve"
        DdlBando.DataValueField = "idbando"
        DdlBando.DataBind()
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub


    Sub Ricerca()
        lblMessaggio.Text = ""

        If Not String.IsNullOrEmpty(txtDataInizio.Text) AndAlso Not Date.TryParse(txtDataInizio.Text, Nothing) Then
            lblMessaggio.CssClass = "msgErrore"
            lblMessaggio.Text = "Data presentazione inizio non è una data valida. Inserire una data nel formato GG/MM/AAAA."
            Exit Sub
        End If

        If Not String.IsNullOrEmpty(txtDataFine.Text) AndAlso Not Date.TryParse(txtDataFine.Text, Nothing) Then
            lblMessaggio.CssClass = "msgErrore"
            lblMessaggio.Text = "Data presentazione fine non è una data valida. Inserire una data nel formato GG/MM/AAAA."
            Exit Sub
        End If

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_PROGRAMMI_PRESENTAZIONE_RICERCA]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 10).Value = Session("Utente")
            If Not String.IsNullOrEmpty(txtDenominazioneEnte.Text) Then sqlDAP.SelectCommand.Parameters.Add("@denominazioneente", SqlDbType.NVarChar).Value = txtDenominazioneEnte.Text
            If Not String.IsNullOrEmpty(txtCodEnte.Text) Then sqlDAP.SelectCommand.Parameters.Add("@codiceregione", SqlDbType.NVarChar).Value = txtCodEnte.Text
            If Not DdlBando.SelectedValue = "0" Then sqlDAP.SelectCommand.Parameters.Add("@idBando", SqlDbType.Int).Value = Integer.Parse(DdlBando.SelectedValue)

            If txtDataInizio.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@DataPresentazioneInizio", SqlDbType.VarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@DataPresentazioneInizio", SqlDbType.VarChar).Value = DataToISO(txtDataInizio.Text.Trim)
            End If

            If txtDataFine.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@DataPresentazioneFine", SqlDbType.VarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@DataPresentazioneFine", SqlDbType.VarChar).Value = DataToISO(txtDataFine.Text.Trim) + " 23:59:59"
            End If

            sqlDAP.Fill(dataSet)

            Session("dtsElencoPresentazioni") = dataSet
            dtgElencoPresentazioni.DataSource = dataSet
            dtgElencoPresentazioni.CurrentPageIndex = 0
            dtgElencoPresentazioni.DataBind()

            If dataSet.Tables(0).Rows.Count = 0 Then
                lblNoResult.Text = "Nessun risultato"
                divNoRicerche.Visible = True
                divRicerche.Visible = False
                CmdEsporta.Visible = False
            Else
                divNoRicerche.Visible = False
                divRicerche.Visible = True
                CmdEsporta.Visible = True
            End If

        Catch ex As Exception
            lblMessaggio.CssClass = "msgErrore"
            lblMessaggio.Text = ex.Message.ToString()
        End Try

    End Sub

    Function DataToISO(data As String) As String

        Return data.Substring(6, 4) + data.Substring(3, 2) + data.Substring(0, 2)

    End Function

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        Ricerca()
    End Sub

    Protected Sub dtgElencoPresentazioni_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgElencoPresentazioni.PageIndexChanged
        Dim dataSet As New DataSet
        dtgElencoPresentazioni.CurrentPageIndex = e.NewPageIndex
        dataSet = Session("dtsElencoPresentazioni")
        dtgElencoPresentazioni.DataSource = dataSet
        dtgElencoPresentazioni.DataBind()
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        Dim _dataSet As New DataSet
        _dataSet = Session("dtsElencoPresentazioni")

        'USATA SOLO PERCHE' LA ROUTINE DI EXPORT AL VOLO (SENZA LINK) E' LI, SI POTREBBE SPOSTARE...
        Dim _clsR As New clsRuoloAntimafia
        _clsR.ExportCSV(_dataSet, Session("Utente") & "_ProtocolliIstanzeProgrammi_" & Format(DateTime.Now, "ddMMyyyyhhmmss") & ".csv", Page)

    End Sub
End Class