Public Class WfrmCOMPInfoPaghe
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtsGenerico As DataSet
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If


        If Request.QueryString("IdVol") = "" Then
            Response.Redirect("wfrmAnomaliaDati.aspx")
        End If


        'strsql = "select IDEnte,Denominazione from enti" & _
        '         " where CodiceRegione='" + Request.QueryString("Ente") + "'"

        strsql = "select e.idente, e.Denominazione from entità a " & _
            " inner join attivitàentità b on a.IDEntità = b.IDEntità and b.IdStatoAttivitàEntità = 1" & _
            " inner join attivitàentisediattuazione c on b.IDAttivitàEnteSedeAttuazione = c.IDAttivitàEnteSedeAttuazione" & _
            " inner join attività d on c.IDAttività = d.IDAttività " & _
            " inner join enti e on d.IDEntePresentante = e.idente " & _
            " where a.identità = " & Request.QueryString("IdVol")
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            Session("IdEnte") = dtrgenerico("IDEnte")
            Session("Denominazione") = dtrgenerico("Denominazione")
        End If


        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'CODICEENTE   Session("txtCodEnte")


        If IsPostBack = False Then
            If ClsUtility.ForzaCaricamentoPaghe(Session("Utente"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            caricadati()
            AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdVol"))
            CaricaGriglia()

            If Request.QueryString("IdPaga") <> Nothing Then
                lblRiferimentoIdPaga.Text = CInt(Request.QueryString("IdPaga"))

                CaricaGrigliaDettaglio(lblRiferimentoIdPaga.Text)
                cmdRicalcola.Visible = True
                CmdAnnulla.Visible = True
            End If
        Else

            lblmess.Visible = False
            lblmess.Text = ""
        End If
    End Sub

    Private Sub HplPresenzeEstero_Click(sender As Object, e As System.EventArgs) Handles HplPresenzeEstero.Click
        Response.Redirect("WfrmCOMPGestionePresenzeEstero.aspx?")
    End Sub

    Private Sub HplSommeResitituite_Click(sender As Object, e As System.EventArgs) Handles HplSommeResitituite.Click
        Response.Redirect("WfrmCOMPSommeRestituite.aspx?IdVol=" + Request.QueryString("IdVol") + "&IdAttivita=" & Request.QueryString("IdAttivita") + "&Proviene=" + CStr(Session("IdEnte")))
    End Sub

    Private Sub HplRimborsidecurtazioni_Click(sender As Object, e As System.EventArgs) Handles HplRimborsidecurtazioni.Click
        Response.Redirect("WfrmCOMPRimborsiDecurtazioni.aspx?IdVol=" + Request.QueryString("IdVol") + "&IdAttivita=" & Request.QueryString("IdAttivita") + "&Proviene=" + CStr(Session("IdEnte")))
    End Sub

    Private Sub HplGestioneAssenze_Click(sender As Object, e As System.EventArgs) Handles HplGestioneAssenze.Click
        Response.Redirect("assenzevolontari.aspx?IdAttivita=" + Request.QueryString("IdAttivita") + "&IdEntita=" + Request.QueryString("IdVol") + "&Proviene=" + CStr(Session("IdEnte")))
    End Sub


    Private Sub caricadati()

        strsql = "select entità.Identità,entità.cognome, entità.nome,entità.datachiusura, entità.datanascita,entità.idcomunenascita," & _
        " entità.codicefiscale,case entità.sesso when 1 then 'F' else 'M' end as sesso,entità.CodiceVolontario,entità.IBAN,entità.DataInizioServizio,entità.DataFineServizio, entità.DataInizioInterruzione,entità.DataFineInterruzione,DataRipresaServizio, DataFineServizioOriginaria " & _
        " from entità " & _
        " where entità.identità=" & CInt(Request.QueryString("IdVol")) & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()


            lblCognome.Text = IIf(Not IsDBNull(dtrgenerico("CodiceVolontario")), dtrgenerico("CodiceVolontario"), "")
            lblNome.Text = IIf(Not IsDBNull(dtrgenerico("Cognome")), dtrgenerico("Cognome"), "") & " " & IIf(Not IsDBNull(dtrgenerico("nome")), dtrgenerico("nome"), "")
            lblIbanVol.Text = IIf(Not IsDBNull(dtrgenerico("Iban")), dtrgenerico("Iban"), "")
            lblCodFis.Text = IIf(Not IsDBNull(dtrgenerico("codicefiscale")), dtrgenerico("codicefiscale"), "")
            lblsesso.Text = dtrgenerico("sesso")
            lbldataNascita.Text = dtrgenerico("DataNascita")
            lblDataInizio.Text = IIf(Not IsDBNull(dtrgenerico("DataInizioServizio")), dtrgenerico("DataInizioServizio"), "")
            lbldataFine.Text = IIf(Not IsDBNull(dtrgenerico("DataFineServizio")), dtrgenerico("DataFineServizio"), "")

            'se ci sono dateinterruzione
            If ((IsDBNull(dtrgenerico("DataInizioInterruzione")) = True) Or (IsDBNull(dtrgenerico("DataFineInterruzione")) = True)) = True Then
                dateIFinterruzione.Visible = False
                dateRipreseServizio.Visible = False
            Else
                dateIFinterruzione.Visible = True
                dateRipreseServizio.Visible = True
                lbldataInizioInterruzione.Text = IIf(Not IsDBNull(dtrgenerico("DataInizioInterruzione")), dtrgenerico("DataInizioInterruzione"), "")
                lbldataFineInterruzione.Text = IIf(Not IsDBNull(dtrgenerico("DataFineInterruzione")), dtrgenerico("DataFineInterruzione"), "")
                lblDataRipresaServizio.Text = IIf(Not IsDBNull(dtrgenerico("DataRipresaServizio")), dtrgenerico("DataRipresaServizio"), "")
                lblDataFineServizioOriginaria.Text = IIf(Not IsDBNull(dtrgenerico("DataFineServizioOriginaria")), dtrgenerico("DataFineServizioOriginaria"), "")
            End If

        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("IdElaborazione") <> "" Then
            Response.Redirect("WfrmCOMPModificaConsulta.aspx?IdElaborazione=" + Request.QueryString("IdElaborazione"))
        Else
            Response.Redirect("WfrmVolontari.aspx?IdVol=" + Request.QueryString("IdVol") + "&IdAttivita=" & Request.QueryString("IdAttivita"))
        End If

    End Sub

    Private Sub CaricaGriglia()
        strsql = "SELECT COMP_Paghe.IdPaga,COMP_Elaborazioni.IdElaborazione, COMP_Elaborazioni.DataValuta, COMP_Paghe.Importo, COMP_StatiPaghe.StatoPaga, COMP_StatiElaborazioni.StatoElaborazione,COMP_StatiPaghe.Modificabile FROM COMP_StatiPaghe INNER JOIN COMP_Paghe ON COMP_StatiPaghe.IdStatoPaga = COMP_Paghe.IdStatoPaga LEFT OUTER Join COMP_StatiElaborazioni RIGHT OUTER JOIN COMP_Elaborazioni ON COMP_StatiElaborazioni.IdStatoElaborazione = COMP_Elaborazioni.IdStatoElaborazione ON COMP_Paghe.IdElaborazione = COMP_Elaborazioni.IdElaborazione where IDEntità ='" & Request.QueryString("IdVol") & "' order by COMP_Elaborazioni.DataValuta desc"
        dgInfoPaghe.DataSource = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgInfoPaghe.DataBind()
        dgInfoPaghe.Visible = True
    End Sub

    Private Sub dgInfoPaghe_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgInfoPaghe.ItemCommand
        If e.CommandName = "Dettaglio" Then
            CaricaGrigliaDettaglio(e.Item.Cells(0).Text)
            lblRiferimentoIdPaga.Text = e.Item.Cells(0).Text
            CmdAnnulla.Visible = True
            cmdRicalcola.Visible = True
        End If
    End Sub

    Private Sub dgInfoPaghe_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgInfoPaghe.PageIndexChanged
        dgInfoPaghe.CurrentPageIndex = e.NewPageIndex
        CaricaGriglia()
        dgInfoPaghe.SelectedIndex = -1
    End Sub
    Private Sub CaricaGrigliaDettaglio(ByRef idPaga As Integer)
        strsql = "SELECT COMP_ElementiRetributivi.IdPaga, COMP_TipiElemento.Descrizione, COMP_ElementiRetributivi.Descrizione AS Descr, Case Segno WHEN 1 THEN '+' ELSE '-' END Segno, COMP_ElementiRetributivi.Importo, COMP_StatiElemento.StatoElemento,COMP_ElementiRetributivi.IdElementoRetributivo FROM COMP_ElementiRetributivi INNER JOIN COMP_StatiElemento ON COMP_ElementiRetributivi.IdStatoElemento = COMP_StatiElemento.IdStatoElemento INNER JOIN COMP_TipiElemento ON COMP_ElementiRetributivi.IdTipoElemento = COMP_TipiElemento.IdTipoElemento where IdPaga=" & idPaga & "  Order by COMP_TipiElemento.IdTipoElemento"
        dgDettaglioPaga.DataSource = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgDettaglioPaga.DataBind()
        dgDettaglioPaga.Visible = True

    End Sub
    Private Function AbilitaDisabilitaSospensionePulsante(ByVal entità As Integer)
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "select  case isnull(d.IdSospensioneVolontario,-1) when -1 then 'NO' else 'SI' End as SospensioneVolontario, case isnull(e.IdSospensioneProgetto,-1) when -1 then 'NO' else 'SI' End as SospensioneProgetto from entità a inner join attivitàentità b on a.IDEntità = b.IDEntità and IdStatoAttivitàEntità = 1 inner join attivitàentisediattuazione c on b.IDAttivitàEnteSedeAttuazione = c.IDAttivitàEnteSedeAttuazione left join COMP_SospensioneVolontari d on a.IDEntità = d.IdEntità and d.Valida = 1 left join COMP_SospensioneProgetti e on c.IDAttività = e.IdAttività and e.Valida = 1 where a.IdEntità = " & entità & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()

            If dtrgenerico("SospensioneVolontario") = "NO" Then
                cmdSospendiVol.Visible = True
                CmdRiattivaVol.Visible = False
            Else
                CmdRiattivaVol.Visible = True
                cmdSospendiVol.Visible = False
            End If
            If dtrgenerico("SospensioneProgetto") = "NO" Then
                cmdSospendiProg.Visible = True
                CmdRiattivaProg.Visible = False
            Else
                CmdRiattivaProg.Visible = True
                cmdSospendiProg.Visible = False
            End If

        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return True
    End Function

    Protected Sub CmdAnnulla_Click(sender As Object, e As EventArgs) Handles CmdAnnulla.Click
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_COMP_ANNULLAPAGA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdPaga", SqlDbType.Int).Value = lblRiferimentoIdPaga.Text
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
            lblmess.Visible = True
            AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdPaga"))
            CaricaGriglia()
            CaricaGrigliaDettaglio(lblRiferimentoIdPaga.Text)
        Catch ex As Exception
            lblmess.Visible = True
            lblmess.Text = ex.Message

        End Try


    End Sub

    Protected Sub cmdRicalcola_Click(sender As Object, e As EventArgs) Handles cmdRicalcola.Click
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_COMP_RICALCOLAPAGA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdPaga", SqlDbType.Int).Value = CInt(lblRiferimentoIdPaga.Text)
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
            lblmess.Visible = True
            AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdPaga"))
            CaricaGriglia()
            cmdRicalcola.Visible = True
            CmdAnnulla.Visible = True
            lblRiferimentoIdPaga.Text = ""
            dgDettaglioPaga.Visible = False
            'CaricaGrigliaDettaglio(lblRiferimentoIdPaga.Text)
        Catch ex As Exception
            lblmess.Visible = True
            lblmess.Text = ex.Message

        End Try
    End Sub

    Protected Sub cmdSospendiVol_Click(sender As Object, e As EventArgs) Handles cmdSospendiVol.Click

        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_COMP_SOSPENSIONEVOLONTARI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEntità", SqlDbType.Int).Value = CInt(Request.QueryString("IdVol"))
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
            lblmess.Visible = True
            AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdVol"))

        Catch ex As Exception
            lblmess.Visible = True
            lblmess.Text = ex.Message

        End Try
    End Sub

    Protected Sub CmdRiattivaVol_Click(sender As Object, e As EventArgs) Handles CmdRiattivaVol.Click
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_COMP_SOSPENSIONEVOLONTARI_RIPRISTINO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEntità", SqlDbType.Int).Value = CInt(Request.QueryString("IdVol"))
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
            lblmess.Visible = True
            AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdVol"))
        Catch ex As Exception
            lblmess.Visible = True
            lblmess.Text = ex.Message

        End Try
    End Sub

    Protected Sub cmdSospendiProg_Click(sender As Object, e As EventArgs) Handles cmdSospendiProg.Click
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_COMP_SOSPENSIONEPROGETTI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdAttività", SqlDbType.Int).Value = CInt(Request.QueryString("IdAttivita"))
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
            lblmess.Visible = True
            AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdVol"))
        Catch ex As Exception
            lblmess.Text = ex.Message
            lblmess.Visible = True
        End Try
    End Sub

    Protected Sub CmdRiattivaProg_Click(sender As Object, e As EventArgs) Handles CmdRiattivaProg.Click
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_COMP_SOSPENSIONEPROGETTI_RIPRISTINO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdAttività", SqlDbType.Int).Value = CInt(Request.QueryString("IdAttivita"))
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
            lblmess.Visible = True
            AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdVol"))
        Catch ex As Exception
            lblmess.Text = ex.Message
            lblmess.Visible = True
        End Try
    End Sub

    Private Sub dgDettaglioPaga_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dgDettaglioPaga.SelectedIndexChanged
        'lblRiferimentoIdPaga.Text = ""
    End Sub


End Class