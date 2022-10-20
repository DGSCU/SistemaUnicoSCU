Imports System.Security.Cryptography
Imports Logger.Data
Imports Futuro.RiepilogoAccreditamento

Public Class WfrmIstanzaSostituzioniOLP
    Inherits SmartPage

    Private Const PATH_DOC_ISTANZA As String = "download\Master\Progetti\IstanzaSostituzioniOLP.docx"
    Private Const PATH_DOC_ISTANZA_COORDINATORE As String = "download\Master\Progetti\IstanzaSostituzioniOLPCOORDINATORE.docx"
    Private Const TIPO_RAPPRESENTANTE_LEGALE As String = "Rappresentante Legale"
    Private Const TIPO_COORDINATORE_RESPONSABILE As String = "Coordinatore Responsabile del Servizio Civile Universale"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOut dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOut dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If

        If Page.IsPostBack = False Then
            Dim IdIstanzaSostituzioneOLP As Integer = 0

            Integer.TryParse(Request.QueryString("IdIstanzaSostituzioneOLP"), IdIstanzaSostituzioneOLP)

            If Not CaricaMaschera(IdIstanzaSostituzioneOLP) Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If

            If Request.QueryString("Messaggio") <> Nothing Then
                lblMessSalva.Visible = True
                lblMessSalva.CssClass = "msgInfo"
                lblMessSalva.Text = Request.QueryString("Messaggio")
            Else
                lblMessSalva.Visible = False
            End If
        End If
    End Sub


    Sub BloccaMaschera()
        btnElimina.Visible = False
        btnPresenta.Visible = False
        btnSalva.Visible = False
        cmdInserisci.Visible = False
        btnModificaISTANZA.Visible = False
        btnEliminaISTANZA.Visible = False
        dgElencoSostituzioniOLP.Columns(15).Visible = False 'Tasto elimina sostituzione
    End Sub

    Function CaricaMaschera(IdIstanzaSostituzioneOLP As Integer) As Boolean
        Dim dataSet As New DataSet

        'carico sempre (anche se vuoto) per dare la forma al DataSet
        Dim IdEnte As Integer = 0
        Integer.TryParse(Session("IdEnte"), IdEnte)
        CaricaIstanzaSostituzioniOLP(IdIstanzaSostituzioneOLP, IdEnte, Session("Utente"))

        dataSet = Session("DtsIstanzaSostituzioniOLP")

        If IdIstanzaSostituzioneOLP > 0 AndAlso dataSet.Tables(1).Rows.Count = 0 Then Return False

        dgElencoSostituzioniOLP.DataSource = dataSet.Tables(1)
        dgElencoSostituzioniOLP.CurrentPageIndex = 0
        dgElencoSostituzioniOLP.DataBind()
        lblErroreSelezioneSostituzioneOlp.Visible = False

        CaricaRicercaSostituzioniOLP(GetElencoIdSostituzioniOLPinGriglia(), False)

        txtIdIstanza.Text = IdIstanzaSostituzioneOLP.ToString()

        If IdIstanzaSostituzioneOLP = 0 Then
            'se sono in creazione precarico tutte le sostituzioni disponibili e poi ricarico(vuota) la griglia di ricerca
            'poichè il datatable della ricerca è identico al secondo datatable dell'istanza è facile e non devo ripetere ricerche
            Dim dataset1 As DataSet = Session("DtsRicercaSostituzioniOLP")
            Dim _dt As DataTable = dataset1.Tables(0).Copy()
            _dt.TableName = dataSet.Tables(1).TableName
            dataSet.Tables.RemoveAt(1)
            dataSet.Tables.Add(_dt)
            Session("DtsIstanzaSostituzioniOLP") = dataSet
            dgElencoSostituzioniOLP.DataSource = dataSet.Tables(1)
            dgElencoSostituzioniOLP.CurrentPageIndex = 0
            dgElencoSostituzioniOLP.DataBind()
            CaricaRicercaSostituzioniOLP(GetElencoIdSostituzioniOLPinGriglia(), False)
            ClearSessionISTANZA()
            txtStatoIstanza.Text = "IN CREAZIONE"
        Else
            txtStatoIstanza.Text = dataSet.Tables(0).Rows(0)("StatoIstanza").ToString()
            If dataSet.Tables(0).Rows(0)("Stato").ToString = "1" Then
                btnElimina.Visible = True
                divNoPresentata.Visible = True
            Else
                btnElimina.Visible = False
                divNoPresentata.Visible = False
            End If
            'Carico in maschera gli eventuali altri dati
            If dataSet.Tables(0).Rows(0)("Domanda") IsNot DBNull.Value Then
                Dim _dom As Byte() = DirectCast(dataSet.Tables(0).Rows(0)("Domanda"), Byte())
                Session("IstanzaScaricata") = _dom
            End If

            If dataSet.Tables(0).Rows(0)("DomandaFirmata") IsNot DBNull.Value Then
                rowNoISTANZA.Visible = False
                rowISTANZA.Visible = True

                Dim ISTANZA As New Allegato() With {
                 .Updated = False,
                 .Blob = DirectCast(dataSet.Tables(0).Rows(0)("DomandaFirmata"), Byte()),
                 .Filename = dataSet.Tables(0).Rows(0)("FilenameDomandaFirmata"),
                 .Hash = dataSet.Tables(0).Rows(0)("HashDomandaFirmata"),
                 .Filesize = DirectCast(dataSet.Tables(0).Rows(0)("DomandaFirmata"), Byte()).Length,
                 .DataInserimento = dataSet.Tables(0).Rows(0)("DataCreazioneRecord")
                }
                Session("LoadedISTANZAFIRMATA") = ISTANZA
                txtISTANZAFilename.Text = ISTANZA.Filename
                txtISTANZAHash.Text = ISTANZA.Hash
                txtISTANZAData.Text = ISTANZA.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
            Else
                ClearSessionISTANZA()
            End If

            If dataSet.Tables(0).Rows(0)("Stato").ToString <> "1" Then
                BloccaMaschera()
            End If

            If Session("TipoUtente") = "U" Then
                dgElencoSostituzioniOLP.Columns(9).Visible = True   'colonna Stato
                dgRisultatoRicerca.Columns(8).Visible = True        'colonna Stato

                'verifico se abilitare la generazione della lettera di risposta
                If dataSet.Tables(0).Rows(0)("Stato").ToString = "2" Then       'se lo stato è presentata
                    Dim _row As DataRow() = dataSet.Tables(1).Select("Stato='Presentata'") 'cerco eventuali sostituzioni ancora in stato: presentata
                    If _row.Count = 0 Then btnGeneraRisposta.Visible = True
                End If
            Else
                If dataSet.Tables(0).Rows(0)("Stato").ToString = "3" Then dgElencoSostituzioniOLP.Columns(9).Visible = True 'se lo stato dell'istanza è valutata abilito la colona stato anche per gli enti
            End If
        End If

        Return True
    End Function

    Private Sub CaricaIstanzaSostituzioniOLP(IdIstanzaSostituzioneOLP As Integer, IdEnte As Integer, Username As String)

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_ISTANZA_SOSTITUZIONE_OLP_GET_DATI]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))
            sqlDAP.SelectCommand.Parameters.Add("@IdIstanzaSostituzioneOLP", SqlDbType.Int).Value = IdIstanzaSostituzioneOLP
            sqlDAP.SelectCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 10).Value = Session("Utente")

            sqlDAP.Fill(dataSet)

            Session("DtsIstanzaSostituzioniOLP") = dataSet

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Function GetElencoIdSostituzioniOLPinGriglia() As String
        Dim _ret As String = "0"
        Dim dataSet As New DataSet

        dataSet = Session("DtsIstanzaSostituzioniOLP")

        For Each _row As DataRow In dataSet.Tables(1).Rows
            _ret = _ret + "," + _row("IdSostituzioneOLP").ToString
        Next

        Return _ret
    End Function

    Protected Sub btnRicercaSostituzioniOLP_Click(sender As Object, e As EventArgs) Handles btnRicercaSostituzioniOLP.Click
        CaricaRicercaSostituzioniOLP(GetElencoIdSostituzioniOLPinGriglia(), True)
    End Sub

    Private Sub CaricaRicercaSostituzioniOLP(ElencoIdSostituzioniOLPdaEscludere As String, mostraPopup As Boolean)

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_ISTANZA_RICERCA_SOSTITUZIONE_OLP]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))
            sqlDAP.SelectCommand.Parameters.Add("@ElencoIdSostituzioniOLPdaEscludere", SqlDbType.VarChar, -1).Value = ElencoIdSostituzioniOLPdaEscludere
            If Not String.IsNullOrEmpty(txtTitoloProgetto.Text) Then sqlDAP.SelectCommand.Parameters.Add("@titoloprogetto", SqlDbType.NVarChar).Value = txtTitoloProgetto.Text
            If Not String.IsNullOrEmpty(TxtCodProg.Text) Then sqlDAP.SelectCommand.Parameters.Add("@codiceprogetto", SqlDbType.NVarChar).Value = TxtCodProg.Text
            If Not String.IsNullOrEmpty(txtNomeSostRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@nomesostituito", SqlDbType.NVarChar).Value = txtNomeSostRicerca.Text
            If Not String.IsNullOrEmpty(txtCognomeSostRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@cognomesostituito", SqlDbType.NVarChar).Value = txtCognomeSostRicerca.Text
            If Not String.IsNullOrEmpty(txtNomeSubRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@nomesubentrante", SqlDbType.NVarChar).Value = txtNomeSostRicerca.Text
            If Not String.IsNullOrEmpty(txtCognomeSubRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@cognomesubentrante", SqlDbType.NVarChar).Value = txtCognomeSostRicerca.Text

            sqlDAP.Fill(dataSet)

            Session("DtsRicercaSostituzioniOLP") = dataSet
            dgRisultatoRicerca.DataSource = dataSet
            dgRisultatoRicerca.CurrentPageIndex = 0
            dgRisultatoRicerca.DataBind()

            If mostraPopup Then popSelezioneSostituzioneOlp.Show()
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Sub AggiungiSostituzione(e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim dataSet As New DataSet

        dataSet = Session("DtsIstanzaSostituzioniOLP")

        Dim _row As DataRow = dataSet.Tables(1).NewRow()

        _row("Titolo") = e.Item.Cells(0).Text
        _row("Codice") = e.Item.Cells(1).Text
        _row("IdEnteSedeAttuazione") = e.Item.Cells(2).Text
        _row("CognomeSost") = e.Item.Cells(3).Text
        _row("NomeSost") = e.Item.Cells(4).Text
        _row("Motivazione") = e.Item.Cells(5).Text
        _row("CognomeSub") = e.Item.Cells(6).Text
        _row("NomeSub") = e.Item.Cells(7).Text
        _row("IdSostituzioneOLP") = Integer.Parse(e.Item.Cells(9).Text)
        _row("IdAttivita") = Integer.Parse(e.Item.Cells(10).Text)
        _row("IdAttivitaSedeAttuazione") = Integer.Parse(e.Item.Cells(11).Text)
        _row("IdEntePersonaleRuolo") = Integer.Parse(e.Item.Cells(12).Text)
        _row("IdEnteSedeAttuazione") = Integer.Parse(e.Item.Cells(13).Text)
        _row("Stato") = e.Item.Cells(8).Text

        dataSet.Tables(1).Rows.Add(_row)

        Session("DtsIstanzaSostituzioniOLP") = dataSet
        dgElencoSostituzioniOLP.DataSource = dataSet.Tables(1)
        dgElencoSostituzioniOLP.CurrentPageIndex = 0
        dgElencoSostituzioniOLP.DataBind()
        ClearSessionISTANZA()
    End Sub

    Protected Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName

            Case "SelezionaOLP"

                If Not ControllaSelezione(e) Then
                    popSelezioneSostituzioneOlp.Show()
                    Exit Sub
                End If

                AggiungiSostituzione(e)

                'svuoto campi ricerca
                txtTitoloProgetto.Text = ""
                TxtCodProg.Text = ""
                txtNomeSostRicerca.Text = ""
                txtCognomeSostRicerca.Text = ""
                txtNomeSubRicerca.Text = ""
                txtCognomeSubRicerca.Text = ""

                'svuoto griglia
                CaricaRicercaSostituzioniOLP(GetElencoIdSostituzioniOLPinGriglia(), False)

                lblErroreSelezioneSostituzioneOlp.Visible = False
        End Select
    End Sub

    Function ControllaSelezione(e As System.Web.UI.WebControls.DataGridCommandEventArgs) As Boolean
        Dim dataSet As New DataSet

        dataSet = Session("DtsIstanzaSostituzioniOLP")
        Dim _row As DataRow() = dataSet.Tables(1).Select("IdSostituzioneOLP=" + e.Item.Cells(9).Text)

        If _row.Length > 0 Then
            lblErroreSelezioneSostituzioneOlp.Text = "Sostituzione già presente nell'istanza."
            lblErroreSelezioneSostituzioneOlp.CssClass = "msgErrore"
            lblErroreSelezioneSostituzioneOlp.Visible = True
            Return False
        End If
        Return True
    End Function

    Protected Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Dim dataSet As New DataSet
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dataSet = Session("DtsRicercaSostituzioniOLP")
        dgRisultatoRicerca.DataSource = dataSet
        dgRisultatoRicerca.DataBind()
        popSelezioneSostituzioneOlp.Show()
    End Sub

    Sub EliminaSostituzione(e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim dataSet As New DataSet

        dataSet = Session("DtsIstanzaSostituzioniOLP")

        Dim _row As DataRow() = dataSet.Tables(1).Select("IdSostituzioneOLP=" + e.Item.Cells(10).Text)

        If _row IsNot Nothing AndAlso _row.Length = 1 Then
            dataSet.Tables(1).Rows.Remove(_row(0))
            dataSet.Tables(1).AcceptChanges()

            Session("DtsIstanzaSostituzioniOLP") = dataSet
            dgElencoSostituzioniOLP.DataSource = dataSet.Tables(1)
            dgElencoSostituzioniOLP.CurrentPageIndex = 0
            dgElencoSostituzioniOLP.DataBind()

            CaricaRicercaSostituzioniOLP(GetElencoIdSostituzioniOLPinGriglia(), False)
            ClearSessionISTANZA()
        End If
    End Sub

    Protected Sub dgElencoSostituzioniOLP_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgElencoSostituzioniOLP.ItemCommand
        Select Case e.CommandName

            '_row("IdAttivita") = Integer.Parse(e.Item.Cells(8).Text)
            '_row("IdAttivitaSedeAttuazione") = Integer.Parse(e.Item.Cells(9).Text)
            '_row("IdEntePersonaleRuolo") = Integer.Parse(e.Item.Cells(10).Text)
            '_row("IdEnteSedeAttuazione") = Integer.Parse(e.Item.Cells(11).Text)

            Case "EliminaSostituzione"
                EliminaSostituzione(e)

            Case "Select"
                Response.Redirect("WfrmSostituzioneOlp.aspx?IdAttivita=" & e.Item.Cells(11).Text & "&IdAttivitaSedeAttuazione=" & e.Item.Cells(12).Text & "&IdEntePersonaleRuolo=" & e.Item.Cells(13).Text & "&IdEnteSedeAttuazione=" & e.Item.Cells(14).Text & "&IdSostituzioneOLP=" & e.Item.Cells(10).Text)
        End Select

    End Sub

    Function ControllaSalvataggio(Presenta As Boolean) As Boolean
        Dim dataSet As New DataSet
        lblMessSalva.Visible = False
        dataSet = Session("DtsIstanzaSostituzioniOLP")

        If dataSet Is Nothing OrElse dataSet.Tables.Count <> 2 OrElse dataSet.Tables(1).Rows.Count = 0 Then
            lblMessSalva.Text = "Nessuna sostituzione OLP nell'istanza."
            lblMessSalva.CssClass = "msgErrore"
            lblMessSalva.Visible = True
            Return False
        End If

        If Presenta Then
            Dim _i As Allegato = Session("LoadedISTANZAFIRMATA")
            If _i Is Nothing Then
                lblMessSalva.Text = "E' obbligatorio caricare l'Istanza firmata."
                lblMessSalva.CssClass = "msgErrore"
                lblMessSalva.Visible = True
                Return False
            End If
        End If

        Return True
    End Function

    Protected Sub btnSalva_Click(sender As Object, e As EventArgs) Handles btnSalva.Click
        If Not ControllaSalvataggio(False) Then
            Exit Sub
        End If

        Try
            Dim _errore As String
            Dim _idIstanzaSostituzioniOLP As Integer = 0
            Dim _elencoId As String = GetElencoIdSostituzioniOLPinGriglia()
            Dim _istanzaFirmata As Byte() = Nothing
            Dim _istanzaFirmataHash As String = Nothing
            Dim _istanzaScaricata As Byte() = Nothing
            Dim _istanzaFirmataFilename As String = Nothing
            Dim _i As Allegato = Session("LoadedISTANZAFIRMATA")

            If _elencoId = "0" Then Throw New Exception("Nessuna sostituzione selezionata") 'solo per sicurezza in quanto il controllo deve essere fatto prima nella ControllaSalvataggio

            If Not String.IsNullOrEmpty(txtIdIstanza.Text) Then _idIstanzaSostituzioniOLP = Integer.Parse(txtIdIstanza.Text)

            If Not _i Is Nothing Then
                _istanzaFirmata = _i.Blob
                _istanzaFirmataHash = _i.Hash
                _istanzaScaricata = Session("IstanzaScaricata")
                _istanzaFirmataFilename = _i.Filename
            End If


            _errore = SalvaIstanzaSostituzioniOLP(_idIstanzaSostituzioniOLP,
                                                Integer.Parse(Session("IdEnte")),
                                                _elencoId, Session("Utente"),
                                                _istanzaFirmata,
                                                _istanzaFirmataHash,
                                                _istanzaFirmataFilename,
                                                _istanzaScaricata
                                                )

            If String.IsNullOrEmpty(_errore) Then
                'Redirect a se stesso per mettere in query string l'id dell'istanza
                Response.Redirect("WfrmIstanzaSostituzioniOLP.aspx?IdIstanzaSostituzioneOLP=" & _idIstanzaSostituzioniOLP.ToString() & "&Messaggio=Salvataggio effettuato correttamente")
            Else
                lblMessSalva.Visible = True
                lblMessSalva.CssClass = "msgErrore"
                lblMessSalva.Text = _errore
            End If

        Catch ex As Exception
            lblMessSalva.Visible = True
            lblMessSalva.CssClass = "msgErrore"
            lblMessSalva.Text = "Errore nel salvataggio"
        End Try
    End Sub


    Function SalvaIstanzaSostituzioniOLP(
                                ByRef IdIstanzaSostituzioniOLP As Integer,
                                IdEnte As Integer,
                                ElencoIdSostituzioniOLP As String,
                                UsernameCreazioneRecord As String,
                                IstanzaFirmata As Byte(),
                                IstanzaFirmataHash As String,
                                IstanzaFirmataFilename As String,
                                IstanzaScaricata As Byte()
                                ) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ISTANZA_SOSTITUZIONI_OLP_SALVA]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdIstanzaSostituzioniOLP", SqlDbType.Int).Value = IdIstanzaSostituzioniOLP
        sqlCMD.Parameters("@IdIstanzaSostituzioniOLP").Direction = ParameterDirection.InputOutput
        sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
        sqlCMD.Parameters.Add("@ElencoIdSostituzioniOLP", SqlDbType.VarChar, -1).Value = ElencoIdSostituzioniOLP
        sqlCMD.Parameters.Add("@UsernameCreazioneRecord", SqlDbType.VarChar, 50).Value = UsernameCreazioneRecord

        If IstanzaFirmata Is Nothing Then
            sqlCMD.Parameters.Add("@IstanzaFirmata", SqlDbType.VarBinary).Value = DBNull.Value
            sqlCMD.Parameters.Add("@IstanzaFirmataHash", SqlDbType.VarChar, 100).Value = DBNull.Value
            sqlCMD.Parameters.Add("@IstanzaFirmataFilename", SqlDbType.VarChar, 255).Value = DBNull.Value
            sqlCMD.Parameters.Add("@IstanzaScaricata", SqlDbType.VarBinary).Value = DBNull.Value
        Else
            sqlCMD.Parameters.Add("@IstanzaFirmata", SqlDbType.VarBinary).Value = IstanzaFirmata
            sqlCMD.Parameters.Add("@IstanzaFirmataHash", SqlDbType.VarChar, 100).Value = IstanzaFirmataHash
            sqlCMD.Parameters.Add("@IstanzaFirmataFilename", SqlDbType.VarChar, 255).Value = IstanzaFirmataFilename
            sqlCMD.Parameters.Add("@IstanzaScaricata", SqlDbType.VarBinary).Value = IstanzaScaricata
        End If

        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, -1)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()
        IdIstanzaSostituzioniOLP = sqlCMD.Parameters("@IdIstanzaSostituzioniOLP").Value
        Return sqlCMD.Parameters("@Errore").Value
    End Function

    Protected Sub btnChiudi_Click(sender As Object, e As EventArgs) Handles btnChiudi.Click
        Response.Redirect("WfrmRicercaIstanzeSostituzioniOLP.aspx")
    End Sub

    Protected Sub btnElimina_Click(sender As Object, e As EventArgs) Handles btnElimina.Click
        Try
            Dim _errore As String
            _errore = EliminaIstanzaSostituzioniOLP(Integer.Parse(txtIdIstanza.Text), Integer.Parse(Session("IdEnte")))

            If String.IsNullOrEmpty(_errore) Then
                Response.Redirect("WfrmRicercaIstanzeSostituzioniOLP.aspx?Messaggio=Eliminazione Istanza effettuata correttamente")
            Else
                lblMessSalva.Visible = True
                lblMessSalva.CssClass = "msgErrore"
                lblMessSalva.Text = "Errore nell'eliminazione"
            End If

        Catch ex As Exception
            lblMessSalva.Visible = True
            lblMessSalva.CssClass = "msgErrore"
            lblMessSalva.Text = "Errore nell'eliminazione"
        End Try

    End Sub

    Function EliminaIstanzaSostituzioniOLP(
                                IdIstanzaSostituzioneOLP As Integer,
                                IdEnte As Integer) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ISTANZA_SOSTITUZIONE_OLP_ELIMINA]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdIstanzaSostituzioneOLP", SqlDbType.Int).Value = IdIstanzaSostituzioneOLP
        sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, -1)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()
        Return sqlCMD.Parameters("@Errore").Value
    End Function

    Protected Sub btnScaricaIstanza_Click(sender As Object, e As EventArgs) Handles btnScaricaIstanza.Click
        Dim documento As New AsposeWord
        Dim responsabile As Risorsa
        Dim managerEnte = New clsEnte()

        Dim dataSet As New DataSet
        lblMessSalva.Visible = False
        dataSet = Session("DtsIstanzaSostituzioniOLP")

        If dataSet Is Nothing OrElse dataSet.Tables.Count <> 2 OrElse dataSet.Tables(1).Rows.Count = 0 Then
            lblMessSalva.Text = "Nessuna sostituzione OLP nell'istanza."
            lblMessSalva.CssClass = "msgErrore"
            lblMessSalva.Visible = True
            Exit Sub
        End If


        Try
            Dim isRappresentanteLegale As Boolean = ddlTipoRappresentante.SelectedValue = TIPO_RAPPRESENTANTE_LEGALE
            If isRappresentanteLegale Then
                documento.open(Server.MapPath(PATH_DOC_ISTANZA))
                responsabile = managerEnte.GetRappresentanteLegale(Session("IdEnte"), Session("conn"))
            Else
                documento.open(Server.MapPath(PATH_DOC_ISTANZA_COORDINATORE))
                responsabile = managerEnte.GetCoordinatoreResponsabile(Session("IdEnte"), Session("conn"))
            End If
        Catch ex As Exception
            'Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Template non valido", ex) bisogna aggiungere i log
            lblMessSalva.Text = "Errore nella generazione dell'Istanza Sostituzioni OLP"
            lblMessSalva.CssClass = "msgErrore"
            lblMessSalva.Visible = True
            Exit Sub
        End Try

        Dim ente = managerEnte.GetDatiEnte(Session("IdEnte"), Session("conn"))
        documento.addFieldValue("NomeEnte", ente.Denominazione)
        documento.addFieldValue("CodiceFiscaleEnte", ente.CodiceFiscale)
        documento.addFieldValue("TipoRappresentante", ddlTipoRappresentante.SelectedValue)

        If responsabile IsNot Nothing Then
            documento.addFieldValue("RappresentanteLegaleNome", responsabile.Nome.ToUpper())
            documento.addFieldValue("RappresentanteLegaleLuogoNascita", responsabile.LuogoNascita)
            Dim indirizzo As String = ""
            If Not String.IsNullOrEmpty(responsabile.IndirizzoResidenza) Then
                indirizzo = " e residente in " + responsabile.IndirizzoResidenza
            End If
            documento.addFieldValue("RappresentanteLegaleIndirizzo", indirizzo)
            If responsabile.DataNascita.HasValue Then
                documento.addFieldValue("RappresentanteLegaleDataNascita", responsabile.DataNascita.Value.ToString("dd/MM/yyyy"))
            End If
        End If

        If ente.SedeLegale IsNot Nothing Then
            documento.addFieldValue("IndirizzoEnte", ente.SedeLegale.Indirizzo)
        End If
        ente.SedeLegale = managerEnte.GetSedeLegale(Session("IdEnte"), Session("conn"))

        If ente.SedeLegale IsNot Nothing Then
            documento.addFieldValue("IndirizzoEnte", ente.SedeLegale.Indirizzo)
        End If

        'ente.RappresentanteLegale = managerEnte.GetRappresentanteLegale(Session("IdEnte"), Session("conn"))
        'If ente.RappresentanteLegale IsNot Nothing Then
        '    documento.addFieldValue("RappresentanteLegaleNome", ente.RappresentanteLegale.Nome.ToUpper())
        '    documento.addFieldValue("RappresentanteLegaleLuogoNascita", ente.RappresentanteLegale.LuogoNascita)
        '    Dim indirizzo As String = ""
        '    If Not String.IsNullOrEmpty(ente.RappresentanteLegale.IndirizzoResidenza) Then
        '        indirizzo = " e residente in " + ente.RappresentanteLegale.IndirizzoResidenza
        '    End If
        '    documento.addFieldValue("RappresentanteLegaleIndirizzo", indirizzo)
        '    If ente.RappresentanteLegale.DataNascita.HasValue Then
        '        documento.addFieldValue("RappresentanteLegaleDataNascita", ente.RappresentanteLegale.DataNascita.Value.ToString("dd/MM/yyyy"))
        '    End If
        'End If
        documento.addFieldValue("Data", Date.Today.ToString("dd/MM/yyyy"))
        documento.addFieldValue("CodiceEnte", ente.CodiceEnte)

        Dim html As New StringBuilder
        html.Append("<style>")
        html.Append("table {width:100%; border-collapse: collapse; font-size:10pt; margin-bottom:1em;}")
        html.Append("table, th, td {border: 1px solid lightgray;}")
        html.Append("table tr:nth-child(even) {background-color:#eee}")
        html.Append("td {padding:1pt; font-family:'courier new';}")
        html.Append(".space {width:50%;height:1em;}")
        html.Append("</style>")

        html.Append("<table><tbody>")
        html.Append("<tr style='fort-weight:bold'>")
        html.Append("<th>Titolo Progetto</th>")
        html.Append("<th>Codice Progetto</th>")
        html.Append("<th>Codice Sede</th>")
        html.Append("<th>Cognome sostituito</th>")
        html.Append("<th>Nome sostituito</th>")
        html.Append("<th>Motivo sostituzione</th>")
        html.Append("<th>Cognome subentrante</th>")
        html.Append("<th>Nome subentrante</th>")
        html.Append("</tr>")

        For Each _row As DataRow In dataSet.Tables(1).Rows
            html.Append("<tr>")
            html.Append("<td>" & _row("Titolo") & "</td>")
            html.Append("<td>" & _row("Codice") & "</td>")
            html.Append("<td>" & _row("IdEnteSedeAttuazione") & "</td>")
            html.Append("<td>" & _row("CognomeSost") & "</td>")
            html.Append("<td>" & _row("NomeSost") & "</td>")
            html.Append("<td>" & _row("Motivazione") & "</td>")
            html.Append("<td>" & _row("CognomeSub") & "</td>")
            html.Append("<td>" & _row("NomeSub") & "</td>")
            html.Append("</tr>")
        Next

        html.Append("</tbody></table>")
        html.Append("</br>")

        documento.addFieldHtml("htmlSostituzioniOLP", html.ToString)

        Try
            documento.merge()

            Session("IstanzaScaricata") = documento.pdfBytes
            Session("DtsIstanzaSostituzioniOLP") = dataSet

        Catch ex As Exception
            'Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Scrittura template", ex) bisogna aggiungere i log
            lblMessSalva.Text = "Errore nella generazione dell'Istanza Sostituzioni OLP"
            lblMessSalva.CssClass = "msgErrore"
            lblMessSalva.Visible = True
            Exit Sub
        End Try
        'Log.Information(LogEvent.PRESENTAZIONE_RIEPILOGO)
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & "IstanzaSostituzioniOLP.pdf")
        Response.BinaryWrite(documento.pdfBytes)
        Response.End()
    End Sub

    Private Sub MessaggiPopup(ByVal strMessaggio As String, ByRef _label As Label, ByRef _popup As AjaxControlToolkit.ModalPopupExtender)
        _label.Visible = True
        _label.Text = strMessaggio
        'Log.Information(LogEvent.STRUTTURA_ORGANIZZATIVA_INFO, "Popup con messaggio", parameters:=strMessaggio)
        _popup.Show()
    End Sub

    Function VerificaEstensioneFile(ByVal objPercorsoFile As HtmlInputFile, Optional ByVal permettiP7m As Boolean = False) As Boolean
        'sono accettati solo documento con estensione .pdf e .pdf.p7m
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        VerificaEstensioneFile = False

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeFile = Mid(objPercorsoFile.Value, i, 1) & NomeFile
        Next

        If UCase(Right(NomeFile, 4)) = ".PDF" Or (permettiP7m And UCase(Right(NomeFile, 8)) = ".PDF.P7M") Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function GeneraHash(ByVal FileinByte() As Byte) As String
        Dim tmpHash() As Byte

        tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

        GeneraHash = ByteArrayToString(tmpHash)
        Return GeneraHash
    End Function
    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim i As Integer
        Dim sOutput As New StringBuilder(arrInput.Length)
        For i = 0 To arrInput.Length - 1
            sOutput.Append(arrInput(i).ToString("X2"))
        Next
        Return sOutput.ToString()
    End Function

    Protected Sub cmdAllegaISTANZA_Click(sender As Object, e As EventArgs) Handles cmdAllegaISTANZA.Click
        'Verifica se è stata scaricata l'istanza da firmare
        If Session("IstanzaScaricata") Is Nothing Then
            MessaggiPopup("Non è stata scaricata l'istanza da firmare.", lblErroreUploadISTANZA, popUploadISTANZA)
            Exit Sub
        End If

        'Verifica se è stato inserito il file
        If fileISTANZA.PostedFile Is Nothing Or String.IsNullOrEmpty(fileISTANZA.PostedFile.FileName) Then
            MessaggiPopup("Non è stato scelto nessun file per il caricamento dell'Istanza firmata", lblErroreUploadISTANZA, popUploadISTANZA)
            Exit Sub
        End If
        'Controllo Tipo File
        If VerificaEstensioneFile(fileISTANZA, True) = False Then
            MessaggiPopup("Il formato file dell'Istanza firmata non è corretto. È possibile associare solo documenti nel formato .PDF o PDF.P7M", lblErroreUploadISTANZA, popUploadISTANZA)
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileISTANZA.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length - 1)

        If iLen <= 0 Then
            MessaggiPopup("Attenzione. Impossibile caricare un file vuoto.", lblErroreUploadISTANZA, popUploadISTANZA)
            Exit Sub
        End If

        If iLen > 20971520 Then
            MessaggiPopup("Attenzione. La dimensione massima file è di 20 MB.", lblErroreUploadISTANZA, popUploadISTANZA)
            Exit Sub
        End If

        Dim bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim managerEnte = New clsEnte()
        If Not ConfigurationSettings.AppSettings("DisabilitaFirma") = "true" Then

            Dim rappresentanteLegale = managerEnte.GetRappresentanteLegale(Session("IdEnte"), Session("conn"))
            Dim coordinatore = managerEnte.GetCoordinatoreResponsabile(Session("IdEnte"), Session("conn"))
            If rappresentanteLegale Is Nothing Then
                MessaggiPopup("Non è presente il Rappresentante Legale dell'Ente.", lblErroreUploadISTANZA, popUploadISTANZA)
                Exit Sub
            End If
            Dim cfCoordinatore = ""
            If coordinatore IsNot Nothing Then
                cfCoordinatore = coordinatore.CodiceFiscale
            End If
            Dim sc As New SignChecker(bBLOBStorage)
            If Not sc.checkSignature(rappresentanteLegale.CodiceFiscale, cfCoordinatore) Then
                Log.Warning(LogEvent.FIRMA_NON_VALIDA, sc.getLog())
                MessaggiPopup("Il documento non è firmato digitalmente o non è firmato dal Rappresentante Legale o dal Coordinatore Responsabile del Servizio Civile Universale.", lblErroreUploadISTANZA, popUploadISTANZA)
                Exit Sub
            End If

            Dim _documentoDaFirmare As Byte() = DirectCast(Session("IstanzaScaricata"), Byte())

            If Not sc.compareSignedNotSigned(bBLOBStorage, _documentoDaFirmare) Then
                Log.Warning(LogEvent.FIRMA_NON_VALIDA, message:="Il file firmato non corrisponde alla Istanza Sostituzione OLP da firmare ed allegare")
                MessaggiPopup("Il file firmato non corrisponde alla Istanza da firmare e allegare.", lblErroreUploadISTANZA, popUploadISTANZA)
                Exit Sub
            End If
        End If

        Dim ente = managerEnte.GetDatiEnte(Session("IdEnte"), Session("conn"))

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileISTANZA.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim ISTANZAFIRMATA As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "ISTANZA_SOSTITUZIONI_OLP_" & ente.CodiceFiscale & "_" & txtIdIstanza.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
        .DataInserimento = Date.Now
        }
        Session("LoadedISTANZAFIRMATA") = ISTANZAFIRMATA
        lblErroreUploadISTANZA.Text = ""
        lblErroreUploadISTANZA.Visible = False
        rowNoISTANZA.Visible = False
        rowISTANZA.Visible = True
        txtISTANZAFilename.Text = ISTANZAFIRMATA.Filename
        txtISTANZAHash.Text = ISTANZAFIRMATA.Hash
        txtISTANZAData.Text = ISTANZAFIRMATA.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
    End Sub

    Protected Sub btnDownloadISTANZA_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadISTANZA.Click
        Dim SO As Allegato = Session("LoadedISTANZAFIRMATA")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & SO.Filename)
        Response.BinaryWrite(SO.Blob)
        Response.End()
    End Sub

    Private Sub ClearSessionISTANZA()
        Session("LoadedISTANZAFIRMATA") = Nothing
        Session("IstanzaScaricata") = Nothing
        rowNoISTANZA.Visible = True
        rowISTANZA.Visible = False
    End Sub

    Protected Sub btnEliminaISTANZA_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaISTANZA.Click
        ClearSessionISTANZA()
    End Sub

    Protected Sub btnCloseUploadISTANZA_Click(sender As Object, e As EventArgs) Handles btnCloseUploadISTANZA.Click
        lblErroreUploadISTANZA.Text = ""
        lblErroreUploadISTANZA.Visible = False
    End Sub

    Function PresentaIstanzaSostituzioniOLP(
                                IdIstanzaSostituzioneOLP As Integer,
                                IdEnte As Integer,
                                UsernamePresentazione As String
                                ) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ISTANZA_SOSTITUZIONE_OLP_PRESENTA]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdIstanzaSostituzioneOLP", SqlDbType.Int).Value = IdIstanzaSostituzioneOLP
        sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
        sqlCMD.Parameters.Add("@UsernamePresentazione", SqlDbType.VarChar, 50).Value = UsernamePresentazione

        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, -1)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()
        Return sqlCMD.Parameters("@Errore").Value
    End Function

    Protected Sub btnPresenta_Click(sender As Object, e As EventArgs) Handles btnPresenta.Click
        If Not ControllaSalvataggio(True) Then
            Exit Sub
        End If

        Try
            Dim _errore As String
            Dim _idIstanzaSostituzioniOLP As Integer = 0
            Dim _elencoId As String = GetElencoIdSostituzioniOLPinGriglia()
            Dim _istanzaFirmata As Byte() = Nothing
            Dim _istanzaFirmataHash As String = Nothing
            Dim _istanzaScaricata As Byte() = Nothing
            Dim _istanzaFirmataFilename As String = Nothing
            Dim _i As Allegato = Session("LoadedISTANZAFIRMATA")

            If Not String.IsNullOrEmpty(txtIdIstanza.Text) Then _idIstanzaSostituzioniOLP = Integer.Parse(txtIdIstanza.Text)

            _istanzaFirmata = _i.Blob
            _istanzaFirmataHash = _i.Hash
            _istanzaScaricata = Session("IstanzaScaricata")
            _istanzaFirmataFilename = _i.Filename

            _errore = SalvaIstanzaSostituzioniOLP(_idIstanzaSostituzioniOLP,
                                                Integer.Parse(Session("IdEnte")),
                                                _elencoId, Session("Utente"),
                                                _istanzaFirmata,
                                                _istanzaFirmataHash,
                                                _istanzaFirmataFilename,
                                                _istanzaScaricata
                                                )

            If Not String.IsNullOrEmpty(_errore) Then
                lblMessSalva.Visible = True
                lblMessSalva.CssClass = "msgErrore"
                lblMessSalva.Text = _errore
                Exit Sub
            End If

            _errore = PresentaIstanzaSostituzioniOLP(_idIstanzaSostituzioniOLP, Integer.Parse(Session("IdEnte")), Session("Utente"))

            If String.IsNullOrEmpty(_errore) Then
                CaricaMaschera(_idIstanzaSostituzioniOLP)
                lblMessSalva.Visible = True
                lblMessSalva.CssClass = "msgInfo"
                lblMessSalva.Text = "Presentazione effettuata correttamente"
            Else
                lblMessSalva.Visible = True
                lblMessSalva.CssClass = "msgErrore"
                lblMessSalva.Text = _errore
            End If

        Catch ex As Exception
            lblMessSalva.Visible = True
            lblMessSalva.CssClass = "msgErrore"
            lblMessSalva.Text = "Errore nella presentazione"
        End Try
    End Sub

    Protected Sub ddlTipoRappresentante_SelectedIndexChanged(sender As Object, e As EventArgs)
        ClearSessionISTANZA()
    End Sub

    Protected Sub btnGeneraRisposta_Click(sender As Object, e As EventArgs) Handles btnGeneraRisposta.Click
        lblMessSalva.Visible = False

        'verifico che lo stato sia corretto e che tutte le sostituzioni contenute siano valutate
        'il controllo dovrebbe già essere stato fatto ma viene ripetuto
        Dim dataSet As New DataSet
        dataSet = Session("DtsIstanzaSostituzioniOLP")

        If dataSet.Tables(0).Rows(0)("Stato").ToString = "2" Then       'se lo stato è presentata
            Dim _row As DataRow() = dataSet.Tables(1).Select("Stato='Presentata'") 'cerco eventuali sostituzioni ancora in stato: presentata
            If _row.Count > 0 Then
                lblMessSalva.Visible = True
                lblMessSalva.CssClass = "msgErrore"
                lblMessSalva.Text = "Ci sono sostituzioni non ancora valutate."
                Exit Sub
            End If
        Else
            lblMessSalva.Visible = True
            lblMessSalva.CssClass = "msgErrore"
            lblMessSalva.Text = "L'istanza non è in stato presentata."
            Exit Sub
        End If


        ' AGGIUNGERE QUI LA GENERAZIONE DELLA LETTERA DI RISPOSTA

        'lblGenera.Text = "Scarica File  "
        hplDownload.Visible = True

        Dim Documento As New GeneratoreModelli
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Dim utility As New ClsUtility()

        hplDownload.NavigateUrl = Documento.PROG_SostituzioniOLP(Session("IdEnte"), txtIdIstanza.Text, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
        NuovaCronologia("RispostaSostituzioneOLP")

        btnGeneraRisposta.Visible = False
        hplDownload.Target = "_blank"
    End Sub
    Sub NuovaCronologia(ByVal strDocumento As String, Optional ByRef DataProt As String = "", Optional ByRef NProt As String = "")
        'vado a fare la insert
        Dim strsql As String = ""
        Dim cmdinsert As Data.SqlClient.SqlCommand
        strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento,IDAttivitàSedeAssegnazione,DataProt,NProt) "
        strsql = strsql & "values "
        strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & strDocumento & "',2,null,"

        If DataProt = "" Then
            strsql = strsql & " null,"
        Else
            strsql = strsql & " '" & DataProt & "',"
        End If
        If NProt = "" Then
            strsql = strsql & " null"
        Else
            strsql = strsql & "'" & NProt & "'"
        End If
        strsql = strsql & ")"

        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()

        cmdinsert.Dispose()

    End Sub

    Protected Sub dgElencoSostituzioniOLP_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgElencoSostituzioniOLP.PageIndexChanged
        Dim dataSet As New DataSet
        dgElencoSostituzioniOLP.CurrentPageIndex = e.NewPageIndex
        dataSet = Session("DtsIstanzaSostituzioniOLP")
        dgElencoSostituzioniOLP.DataSource = dataSet.Tables(1)
        dgElencoSostituzioniOLP.DataBind()
    End Sub
End Class