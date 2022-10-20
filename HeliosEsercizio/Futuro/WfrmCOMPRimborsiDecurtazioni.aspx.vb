Public Class WfrmCOMPRimborsiDecurtazioni
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


        If Request.QueryString("Proviene") <> "" Then
            Session("IdEnte") = Request.QueryString("Proviene")
        End If


        If IsPostBack = False Then
            If ClsUtility.ForzaCaricamentoPaghe(Session("Utente"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            CaricaCombo()
            caricadati()
            CaricaGriglia()

        Else

            lblmess.Visible = False
            lblmess.Text = ""
        End If
    End Sub
    Private Sub caricadati()
        
        strsql = "select entità.Identità,entità.cognome, entità.nome,entità.datachiusura, entità.datanascita,entità.idcomunenascita," & _
        " entità.codicefiscale,case entità.sesso when 1 then 'F' else 'M' end as sesso,entità.CodiceVolontario,entità.IBAN,entità.DataInizioServizio,entità.DataFineServizio  " & _
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

        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Private Sub CaricaCombo()
        strsql = "select idTipoElemento,Descrizione from COMP_TipiElemento where calcolato=0 and abilitato=1 ORDER BY 1 "
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        CboTipo.DataSource = dtrgenerico
        CboTipo.DataTextField = "descrizione"
        CboTipo.DataValueField = "idTipoElemento"
        CboTipo.DataBind()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Function controlliSalvataggioServer() As Boolean

        If txtImporto.Text.Trim = String.Empty Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire l'importo."
            txtImporto.Focus()
            Return False
        End If

        Dim importo As Double

        If (Double.TryParse(txtImporto.Text.Trim, importo) = False Or txtImporto.Text.Trim.Contains(",")) Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire un importo decimale utilizzando il punto come separatore."
            txtImporto.Focus()
            Return False
        ElseIf importo = 0 Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire l'importo."
            txtImporto.Focus()
            Return False
        End If
        If txtDescrizione.Text.Trim = String.Empty Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire una Descrizione."
            txtDescrizione.Focus()
            Return False
        End If
        Return True
    End Function

    Protected Sub CmdAnnulla_Click(sender As Object, e As EventArgs) Handles CmdAnnulla.Click
        puliscicampi()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("VengoDa") = "ESTERO" Then
            Response.Redirect("WfrmCOMPGestionePresenzeEstero.aspx?IdVol=" + Request.QueryString("IdVol") + "&IdAttivita=" & Request.QueryString("IdAttivita") + "&IdEnte=" & Request.QueryString("IdEnte") + "&Mensilita=" & Request.QueryString("Mensilita") + "&VadoA=ESTERO")
        Else
            Response.Redirect("WfrmCOMPInfoPaghe.aspx?IdVol=" + Request.QueryString("IdVol") + "&IdAttivita=" + Request.QueryString("IdAttivita") + "&Ente=" + Session("txtCodEnte"))
        End If
    End Sub

    Protected Sub cmdModifica_Click(sender As Object, e As EventArgs) Handles cmdModifica.Click
        If controlliSalvataggioServer() = True Then
            strsql = "Update COMP_ElementiRetributivi  set Importo=" & Replace(txtImporto.Text, ",", ".") & ", Descrizione='" & Replace(txtDescrizione.Text, "'", "''") & "',IdTipoElemento='" & CboTipo.SelectedValue & "', DataCreazioneRecord=getdate()  where IdElementoRetributivo=" & HdnIdSommaRestituita.Value & " "
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            puliscicampi()
            lblmess.Text = "MODIFICA EFFETTUATA."
            lblmess.Visible = True
            CaricaGriglia()
        Else
            Exit Sub
        End If
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If controlliSalvataggioServer() = True Then
            strsql = "Insert into COMP_ElementiRetributivi (IdPaga,IdEntità,IdTipoElemento,IdStatoElemento,Importo,Descrizione,UserNameCreazioneRecord,DataCreazioneRecord,UserNameStato,DataUltimoStato) values (NULL,'" & Request.QueryString("IdVol") & "'," & CboTipo.SelectedValue & ",1,'" & Replace(txtImporto.Text, ",", ".") & "','" & Replace(txtDescrizione.Text, "'", "''") & "','" & Session("Utente") & "', getdate(),'" & Session("Utente") & "', getdate())"
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            lblmess.Text = "INSERIMENTO EFFETTUATO."
            lblmess.Visible = True
            puliscicampi()
            CaricaGriglia()
        Else

            Exit Sub
        End If

    End Sub
    Private Sub CaricaGriglia()
        Dim dataSet As DataSet
        strsql = "Select IdElementoRetributivo,IdEntità,IdPaga,Importo,COMP_ElementiRetributivi.Descrizione,UserNameCreazioneRecord,DataCreazioneRecord,COMP_TipiElemento.Descrizione as Tipo from COMP_ElementiRetributivi inner join COMP_TipiElemento on COMP_ElementiRetributivi.idTipoElemento = COMP_TipiElemento.idTipoElemento   where IDEntità ='" & Request.QueryString("IdVol") & "' and calcolato=0"
        dataSet = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgRisultatoRicercaRimborsiDecurtazioni.DataSource = dataSet
        dgRisultatoRicercaRimborsiDecurtazioni.Visible = True
        Session("appDtsRisRicerca") = dataSet
        dgRisultatoRicercaRimborsiDecurtazioni.DataBind()

    End Sub

    Private Sub dgRisultatoRicercaRimborsiDecurtazioni_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicercaRimborsiDecurtazioni.ItemCommand
        Try
            If e.Item.ItemIndex <> -1 Then
                If e.Item.Cells(3).Text = "&nbsp;" Then
                    If e.CommandName = "Modifica" Then
                        cmdModifica.Visible = True
                        cmdSalva.Visible = False
                        strsql = "Select * from COMP_ElementiRetributivi where IdElementoRetributivo=" & e.Item.Cells(1).Text & " "
                        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        If dtrgenerico.HasRows = True Then
                            dtrgenerico.Read()
                            CboTipo.SelectedValue = dtrgenerico("IdTipoElemento")
                            txtDescrizione.Text = dtrgenerico("Descrizione")
                            txtImporto.Text = Replace(dtrgenerico("Importo"), ",", ".")
                            HdnIdSommaRestituita.Value = e.Item.Cells(1).Text
                        End If
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                    End If
                    If e.CommandName = "Elimina" Then

                        strsql = "Delete from COMP_ElementiRetributivi where IdElementoRetributivo=" & e.Item.Cells(1).Text & " "
                        myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                        lblmess.Text = "ELIMINAZIONE EFFETTUATA."
                        lblmess.Visible = True
                        puliscicampi()
                        CaricaGriglia()
                    End If
                Else
                    lblmess.Text = "IMPOSSIBILE EFFETTUARE MODIFICHE. L'ELEMENTO RETRIBUTIVO E' ASSOCIATO AD UNA PAGA"
                    lblmess.Visible = True
                End If
            End If
        Catch ex As Exception

        End Try
       

    End Sub

    Private Sub dgRisultatoRicercaRimborsiDecurtazioni_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicercaRimborsiDecurtazioni.PageIndexChanged
        dgRisultatoRicercaRimborsiDecurtazioni.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicercaRimborsiDecurtazioni.DataSource = Session("appDtsRisRicerca")
        dgRisultatoRicercaRimborsiDecurtazioni.DataBind()
        'dgRisultatoRicercaRimborsiDecurtazioni.CurrentPageIndex = e.NewPageIndex
        'CaricaGriglia()
        'dgRisultatoRicercaRimborsiDecurtazioni.SelectedIndex = -1
    End Sub
    Private Sub puliscicampi()
        txtImporto.Text = ""
        txtDescrizione.Text = ""
        HdnIdSommaRestituita.Value = ""
        cmdModifica.Visible = False
        cmdSalva.Visible = True
    End Sub
End Class