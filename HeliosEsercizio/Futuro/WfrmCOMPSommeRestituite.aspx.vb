Public Class WfrmCOMPSommeRestituite
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
    Private Sub CaricaGriglia()
        strsql = "Select * from COMP_SommeRestituite  where IDEntità ='" & Request.QueryString("IdVol") & "'"
        dgRisultatoRicercaRestituzioni.DataSource = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgRisultatoRicercaRestituzioni.DataBind()
        dgRisultatoRicercaRestituzioni.Visible = True
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If controlliSalvataggioServer() = True Then
            strsql = "Insert into COMP_SommeRestituite (IdEntità,Importo,Descrizione,DataRiferimento,UserNameCreazioneRecord,DataCreazioneRecord) values ('" & Request.QueryString("IdVol") & "','" & Replace(txtImporto.Text, ",", ".") & "','" & ClsServer.NoApice(txtDescrizione.Text) & "',CONVERT(Datetime,'" & TxtDataRiferimento.Text & "',103),'" & Session("Utente") & "', getdate())"
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            lblmess.Text = "INSERIMENTO EFFETTUATO."
            lblmess.Visible = True
            puliscicampi()
            CaricaGriglia()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub dgRisultatoRicercaRestituzioni_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicercaRestituzioni.ItemCommand
        If e.CommandName = "Modifica" Then
            cmdModifica.Visible = True
            cmdSalva.Visible = False
            strsql = "Select * from COMP_SommeRestituite where idSommaRestituita=" & e.Item.Cells(1).Text & " "
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                TxtDataRiferimento.Text = dtrgenerico("DataRiferimento")
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

            strsql = "Delete from COMP_SommeRestituite where idSommaRestituita=" & e.Item.Cells(1).Text & " "
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            lblmess.Text = "ELIMINAZIONE EFFETTUATA."
            lblmess.Visible = True
            CaricaGriglia()
        End If
       

    End Sub

    Protected Sub cmdModifica_Click(sender As Object, e As EventArgs) Handles cmdModifica.Click
        If controlliSalvataggioServer() = True Then
            strsql = "Update COMP_SommeRestituite  set Importo=" & Replace(txtImporto.Text, ",", ".") & ", Descrizione='" & ClsServer.NoApice(txtDescrizione.Text) & "',DataRiferimento=Convert(DateTime,'" & TxtDataRiferimento.Text & "',103),DataCreazioneRecord=getdate()  where idSommaRestituita=" & HdnIdSommaRestituita.Value & " "
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            cmdModifica.Visible = False
            cmdSalva.Visible = True
            TxtDataRiferimento.Text = ""
            txtDescrizione.Text = ""
            txtImporto.Text = ""
            HdnIdSommaRestituita.Value = ""
            lblmess.Text = "MODIFICA EFFETTUATA."
            lblmess.Visible = True
            puliscicampi()
            CaricaGriglia()
        Else
            Exit Sub
        End If
        
    End Sub
    Function controlliSalvataggioServer() As Boolean

        If TxtDataRiferimento.Text.Trim = String.Empty Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire la Data Riferimento."
            TxtDataRiferimento.Focus()
            Return False
        End If

        Dim dataRiferimento As Date
        If (Date.TryParse(TxtDataRiferimento.Text, dataRiferimento) = False) Then
            lblmess.Visible = True
            lblmess.Text = "Il formato della data è incorretto: il formato deve essere GG/MM/AAAA."
            TxtDataRiferimento.Focus()
            Return False
        End If
        If txtDescrizione.Text.Trim = String.Empty Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire una Descrizione."
            txtDescrizione.Focus()
            Return False
        End If
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
        Return True
    End Function

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmCOMPInfoPaghe.aspx?IdVol=" + Request.QueryString("IdVol") + "&IdAttivita=" + Request.QueryString("IdAttivita") + "&Ente=" + Session("txtCodEnte"))
    End Sub
    Protected Sub CmdAnnulla_Click(sender As Object, e As EventArgs) Handles CmdAnnulla.Click
        puliscicampi()
    End Sub

    Private Sub dgRisultatoRicercaRestituzioni_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicercaRestituzioni.PageIndexChanged
        dgRisultatoRicercaRestituzioni.CurrentPageIndex = e.NewPageIndex
        CaricaGriglia()
        dgRisultatoRicercaRestituzioni.SelectedIndex = -1
    End Sub
    Private Sub puliscicampi()
        txtImporto.Text = ""
        TxtDataRiferimento.Text = ""
        txtDescrizione.Text = ""
        HdnIdSommaRestituita.Value = ""
        cmdModifica.Visible = False
        cmdSalva.Visible = True
    End Sub
End Class