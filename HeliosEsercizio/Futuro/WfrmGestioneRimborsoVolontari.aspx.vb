Imports System.Drawing
Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Cryptography

Public Class WfrmGestioneRimborsoVolontari
    Inherits System.Web.UI.Page

    Dim strsql As String
    Dim dtsGenerico As DataSet
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim myCommand As System.Data.SqlClient.SqlCommand

    Enum Operation
        Insert = 1
        Update = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            txtIdentita.Value = Request.QueryString("identita")
            txtidattivita.Value = Request.QueryString("idattivita")
            strsql = "select idcausale,descrizione from causali where tipo=5"
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            ddlCausale.DataSource = dtrgenerico
            ddlCausale.DataTextField = "descrizione"
            ddlCausale.DataValueField = "idcausale"
            ddlCausale.DataBind()
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            PopolaMaschera()
            CaricaSediAttuazione()
            CaricaRimborsi()
            'End If
        Else
            lblmess.Visible = False
            lblmess.Text = ""
        End If
    End Sub

    Private Sub PopolaMaschera()
        'Generata da Alessandra Taballione il 22.11.2004
        'Effettua il Popolamento della maschera relativo 
        'alle informazioni del Volontario e del progetto
        'Volontario
        strsql = "select entità.cognome, entità.nome, entità.codicevolontario, entità.datachiusura,statientità.Statoentità, entità.datanascita,entità.idcomunenascita," & _
        " cn.denominazione as comuneNascita,cr.denominazione as comuneresidenza," & _
        " entità.idcomuneresidenza,entità.codicefiscale,case entità.sesso when 1 then 'F' else 'M' end as sesso, " & _
        " entità.DataInizioServizio, entità.DataFineServizio " & _
        " from entità " & _
        " inner join statientità on statientità.idstatoEntità=entità.idstatoentità " & _
        " inner join comuni cn on cn.idcomune=entità.idcomunenascita " & _
        " inner join comuni cr on cr.idcomune=entità.idcomuneresidenza " & _
        " where entità.identità=" & txtIdentita.Value & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            lblCognome.Text = IIf(Not IsDBNull(dtrgenerico("Cognome")), dtrgenerico("Cognome"), "")
            lblNome.Text = IIf(Not IsDBNull(dtrgenerico("nome")), dtrgenerico("nome"), "")
            lblComuneNascita.Text = IIf(Not IsDBNull(dtrgenerico("comuneNascita")), dtrgenerico("comuneNascita"), "")
            lblComuneResidenza.Text = IIf(Not IsDBNull(dtrgenerico("comuneresidenza")), dtrgenerico("comuneresidenza"), "")
            lblCodFis.Text = IIf(Not IsDBNull(dtrgenerico("codicefiscale")), dtrgenerico("codicefiscale"), "")
            lblsesso.Text = dtrgenerico("sesso")
            lbldataNascita.Text = dtrgenerico("DataNascita")
            lblStato.Text = dtrgenerico("Statoentità")
            lblCodiceVolontario.Text = IIf(Not IsDBNull(dtrgenerico("codicevolontario")), dtrgenerico("codicevolontario"), "")
            lblDataInizio.Text = IIf(Not IsDBNull(dtrgenerico("DataInizioServizio")), dtrgenerico("DataInizioServizio"), "")
            lbldataFine.Text = IIf(Not IsDBNull(dtrgenerico("DataFineServizio")), dtrgenerico("DataFineServizio"), "")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "Select titolo, datainizioattività,datafineattività,dateadd(day,90,datainizioattività) as dataLimite from attività where idattività=" & txtidattivita.Value & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            lblProgetto.Text = IIf(Not IsDBNull(dtrgenerico("titolo")), dtrgenerico("titolo"), "")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Private Sub CaricaSediAttuazione()
        strsql = "select '<img src=images/home3.gif Width=20 Height=20 border=0 >' as img, entisedi.denominazione as sedefisica, entisediattuazioni.denominazione as sedeAttuazione," & _
       " entisedi.indirizzo,Comuni.denominazione + '(' + provincie.provincia + ')' as Comune,attivitàentità.identità, " & _
       "attivitàentità.identità,attivitàentità.idattivitàentesedeattuazione,attivitàentità.datafineattivitàentità," & _
       " (select idstatoattivitàentità from statiattivitàentità where defaultstato=1) as statodefault," & _
       " attivitàentità.note,attivitàentità.percentualeutilizzo,attivitàentità.idtipologiaposto " & _
       " from attivitàentisediattuazione" & _
       " inner join attivitàentità on " & _
       " attivitàentità.idattivitàentesedeattuazione = attivitàentisediattuazione.idattivitàentesedeattuazione " & _
       " INNER JOIN StatiAttivitàEntità ON StatiAttivitàEntità.IdStatoAttivitàEntità = AttivitàEntità.IdStatoAttivitàEntità AND StatiAttivitàEntità.DefaultStato = 1 " & _
       " inner join entisediattuazioni on " & _
       " attivitàentisediattuazione.IdEnteSedeAttuazione = entisediattuazioni.IdEnteSedeAttuazione " & _
       " inner join entisedi on entisedi.identesede=entisediattuazioni.identesede " & _
       " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
       " inner join provincie on provincie.idprovincia=comuni.idprovincia" & _
       " inner join attività on attivitàentisediattuazione.IdAttività=attività.IdAttività " & _
       " where attivitàentità.identità=" & txtIdentita.Value & ""
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicercaSedi)
    End Sub

    Private Sub CaricaRimborsi()
        strsql = "Select EntitàRimborsi.idEntitàRimborso as idRimborso,EntitàRimborsi.identità,identitàrimborso,causali.idcausale," & _
        " causali.descrizione as causale,causali.idcausale,datariferimento,Importo, " & _
        " ImportoConfermato ," & _
        " case isnull(note,'-1')when '-1' then ' ' else note end as note,case stato " & _
        " when 1 then 'Proposto' when 2 then 'Confermato' when 3 then 'Respinto' end as Stato,case isnull(EntitàRimborsi.idAttivitàRimborso,-1) when -1 then 0 else EntitàRimborsi.idAttivitàRimborso end as idAttivitàRimborso " & _
        " from EntitàRimborsi " & _
        " inner join causali on causali.idcausale=entitàRimborsi.idcausale " & _
        " where idEntità=" & txtIdentita.Value & ""
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicercaRimborsi)
        ColoraCelle()
    End Sub

    Private Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            dgRisultatoRicercaRimborsi.Columns(8).Visible = True
            dgRisultatoRicercaRimborsi.Columns(9).Visible = True
            dgRisultatoRicercaRimborsi.Columns(14).Visible = False
        Else
            dgRisultatoRicercaRimborsi.Columns(8).Visible = False
            dgRisultatoRicercaRimborsi.Columns(9).Visible = False
            dgRisultatoRicercaRimborsi.Columns(14).Visible = True
        End If
    End Sub

    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 15/06/04
        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer
        For Each item In dgRisultatoRicercaRimborsi.Items
            For x = 0 To 14
                If dgRisultatoRicercaRimborsi.Items(item.ItemIndex).Cells(6).Text = "Confermato" Then
                    color = ColorTranslator.FromHtml("#99FF99") 'verde
                End If
                If dgRisultatoRicercaRimborsi.Items(item.ItemIndex).Cells(6).Text = "Proposto" Then
                    color = ColorTranslator.FromHtml("#FFFF99") 'Khaki
                End If
                If dgRisultatoRicercaRimborsi.Items(item.ItemIndex).Cells(6).Text = "Respinto" Then
                    color = ColorTranslator.FromHtml("#FF9966") 'LightSalmon
                End If
                dgRisultatoRicercaRimborsi.Items(item.ItemIndex).Cells(x).BackColor = color
            Next
        Next
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click

        If controlliSalvataggioServer(Operation.Insert) = True Then
            AggiornaDati()
            InsertFileInDB(Operation.Insert)
            PulisciMaschera()
        End If

    End Sub

    Private Sub AggiornaDati()
        Dim null As String
        Dim item As DataGridItem
        Dim newIDentitaRimborso As Int32 = 0

        null = "null"
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            If cmdSalva.Visible = True Then
                strsql = "Insert into EntitàRimborsi (Identità,idcausale,datariferimento," & _
                " Importo,usernameinseritore,datacreazione,note) values " & _
                " (" & txtIdentita.Value & "," & ddlCausale.SelectedValue & "," & _
                " '" & txtdataRiferimento.Text & "'," & Replace(txtImporto.Text, ",", ".") & ",'" & Replace(Session("Utente"), "'", "''") & "',getdate(),'" & Replace(txtNote.Text, "'", "''") & "')"
                strsql = strsql & " SELECT CAST(scope_identity() AS int) "
            Else
                strsql = " Update EntitàRimborsi set idcausale=" & ddlCausale.SelectedValue & ",datariferimento='" & txtdataRiferimento.Text & "'," & _
                " Importo=" & Replace(txtImporto.Text, ",", ".") & ",note='" & Replace(txtNote.Text, "'", "''") & "',usernameUltimaModifica='" & Session("Utente") & "'," & _
                " dataUltimaModifica=getdate()"
                If txtImportoConf.Text = "" Then txtImportoConf.Text = 0
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") And txtImportoConf.Text <> txtImporto.Text Then
                    If CDbl(txtImportoConf.Text) > 0 Then
                        strsql = strsql & " ,importoConfermato=" & Replace(txtImportoConf.Text, ",", ".") & " "
                    End If
                End If
                strsql = strsql & " where(idEntitàRimborso = " & txtidentitaRimborso.Value & ")"
            End If


            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = MyTransaction
            myCommand.CommandText = strsql

            If cmdSalva.Visible = True Then

                'Insert
                newIDentitaRimborso = Convert.ToInt32(myCommand.ExecuteScalar())
                txtidentitaRimborso.Value = newIDentitaRimborso
            Else

                'Update
                myCommand.ExecuteNonQuery()
            End If

            MyTransaction.Commit()

        Catch e As Exception
            Response.Write(strsql)
            Response.Write("<br>")
            Response.Write(e.Message.ToString)
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
        Finally
            cmdModifica.Visible = False
            cmdSalva.Visible = True
            PulisciMaschera()
            CaricaRimborsi()
        End Try
        MyTransaction.Dispose()
    End Sub

    Private Sub PulisciMaschera()
        txtdataRiferimento.Text = ""
        txtNote.Text = ""
        ddlCausale.SelectedIndex = 0
        txtImporto.Text = "0.00"
        txtImportoConf.Text = ""

        hlDw.Visible = False
        hlDw.NavigateUrl = String.Empty
        hlDw.Text = String.Empty

        'txtidentitaRimborso.Value = ""
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("ProVengoDa") = "7" Then
            Response.Redirect("WfrmCheckListDettaglioRimborsoViaggio.aspx?idLista=" & Request.QueryString("idLista") & "&Data=" & Request.QueryString("Data") & "&IdEntitaRimborso=" & Request.QueryString("IdEntitaRimborso") & "&VengoDa=" & 7)
        End If
        If Request.QueryString("VengoDa") = "ValidaDocumento" Then
            Response.Redirect("WfrmRiceraVolontariValidaDocumenti.aspx?Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Cognome=" & Request.QueryString("Cognome") & "&Nome=" & Request.QueryString("Nome") & "&CodVolontario=" & Request.QueryString("CodVolontario") & "&CodProgetto=" & Request.QueryString("CodProgetto") & "&CodSede=" & Request.QueryString("CodSede") & "&Doc=" & Request.QueryString("Doc") & "&StatoDoc=" & Request.QueryString("StatoDoc") & "&DataIS=" & Request.QueryString("DataIS") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdEntita=" & Request.QueryString("IdEntita") & "&IdRegione=" & Request.QueryString("IdRegione") & "&VengoDa=GestioneRimborso&Op=" & Request.QueryString("Op"))
        End If
        Response.Redirect("WfrmRicercaVolontariRimborsi.aspx?VengoDa=" & Request.QueryString("VengoDa") & "")
    End Sub

    Private Sub dgRisultatoRicercaRimborsi_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicercaRimborsi.PageIndexChanged
        dgRisultatoRicercaRimborsi.SelectedIndex = -1
        dgRisultatoRicercaRimborsi.EditItemIndex = -1
        dgRisultatoRicercaRimborsi.CurrentPageIndex = e.NewPageIndex
        CaricaRimborsi()
    End Sub

    Private Sub cmdModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModifica.Click

        'Nel caso di modifica di un record, non è obbligatorio inserire il file .pdf
        If controlliSalvataggioServer(Operation.Update) = True Then
            DeleteFileInDB()
            AggiornaDati()
            InsertFileInDB(Operation.Update)
        End If

    End Sub

    Private Sub dgRisultatoRicercaRimborsi_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicercaRimborsi.ItemCommand

        hlDw.Visible = False
        hlDw.NavigateUrl = String.Empty
        hlDw.Text = String.Empty

        If e.CommandName = "Conferma" Then
            If e.Item.Cells(6).Text = "Proposto" Then
                If e.Item.Cells(12).Text = "0" Then
                    strsql = "Update EntitàRimborsi set stato=2,"
                    If e.Item.Cells(4).Text <> "&nbsp;" Then
                        strsql = strsql & " ImportoConfermato=" & Replace(e.Item.Cells(4).Text, ",", ".") & ""
                    Else
                        strsql = strsql & " ImportoConfermato=" & Replace(e.Item.Cells(3).Text, ",", ".") & ""
                    End If
                    strsql = strsql & ", usernameApprovatore='" & Session("Utente") & "',dataApprovazione=getdate()  where idEntitàRimborso=" & e.Item.Cells(10).Text & " "
                    myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

                    strsql = "Update EntitàDocumenti set stato=1, usernameStato='" & Session("Utente") & "',dataStato=getdate()  where idEntitàRimborso=" & e.Item.Cells(10).Text & " "
                    myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))


                    CaricaRimborsi()
                    lblmess.Visible = False
                    lblmess.Text = ""
                Else
                    lblmess.Visible = True
                    lblmess.Text = "La Conferma di questo Rimborso deve essere effettuata a livello di Progetto."
                End If
            Else
                lblmess.Visible = True
                lblmess.Text = "Non è possibile Confermare il Rimborso perchè è stato " & e.Item.Cells(6).Text & "."
            End If
        End If
        If e.CommandName = "Respingi" Then
            If e.Item.Cells(6).Text = "Proposto" Then
                If e.Item.Cells(12).Text = "0" Then
                    strsql = "Update EntitàRimborsi set stato=3, usernameApprovatore='" & Session("Utente") & "',dataApprovazione=getdate()  where idEntitàRimborso=" & e.Item.Cells(10).Text & ""
                    myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

                    strsql = "Update EntitàDocumenti set stato=2, usernameStato='" & Session("Utente") & "',dataStato=getdate()  where idEntitàRimborso=" & e.Item.Cells(10).Text & " "
                    myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

                    CaricaRimborsi()
                    lblmess.Visible = False
                    lblmess.Text = ""
                Else
                    lblmess.Visible = True
                    lblmess.Text = "Il Rimborso deve essere Respinto a livello di Progetto."
                End If
            Else
                lblmess.Visible = True
                lblmess.Text = "Non è possibile Respingere il Rimborso perchè è stato " & e.Item.Cells(6).Text & "."
            End If
        End If
        If e.CommandName = "Modifica" Then
            If e.Item.Cells(6).Text = "Proposto" Then
                If e.Item.Cells(12).Text = "0" Then
                    lblmess.Visible = False
                    lblmess.Text = ""
                    cmdSalva.Visible = False
                    cmdModifica.Visible = True
                    txtdataRiferimento.Text = e.Item.Cells(1).Text
                    ddlCausale.SelectedValue = e.Item.Cells(11).Text
                    txtImporto.Text = Replace(e.Item.Cells(3).Text, ",", ".")
                    txtNote.Text = IIf(e.Item.Cells(5).Text = "&nbsp;", "", e.Item.Cells(5).Text)
                    txtidentitaRimborso.Value = e.Item.Cells(10).Text
                    txtImportoConf.Text = IIf(e.Item.Cells(4).Text = "&nbsp;", "", Replace(e.Item.Cells(4).Text, ",", "."))
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        txtImportoConf.ReadOnly = False
                        txtImportoConf.BackColor = Color.White
                    End If
                Else
                    lblmess.Visible = True
                    lblmess.Text = "La modifica di questo Rimborso deve essere effuttuata a livello di Progetto."
                End If
            Else
                lblmess.Visible = True
                lblmess.Text = "Non è possibile Modificare il Rimborso perchè è stato " & e.Item.Cells(6).Text & "."
            End If
        End If

        If e.CommandName = "Scarica" Then
            Dim strsql As String
            Dim MyDataset As DataSet
            Dim identitadocumento As Integer
            strsql = "SELECT * from EntitàDocumenti where IdEntitàRimborso=" & e.Item.Cells(10).Text
            MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

            If MyDataset.Tables(0).Rows.Count <> 0 Then

                hlDw.Visible = True
                identitadocumento = MyDataset.Tables(0).Rows(0).Item("IdEntitàDocumento")
                hlDw.NavigateUrl = RecuperaDocumentoRimborsi(identitadocumento, Session("conn"))
                hlDw.Text = MyDataset.Tables(0).Rows(0).Item("FileName")

            End If

            MyDataset.Dispose()

        End If

        If e.CommandName = "Elimina" Then
            If e.Item.Cells(6).Text = "Proposto" Then

                'Cancellazione File 
                strsql = "DELETE FROM EntitàDocumenti WHERE IdEntitàRimborso=" & e.Item.Cells(10).Text
                myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

                'Cancellazione Rimborso
                strsql = "DELETE FROM EntitàRimborsi WHERE idEntitàRimborso=" & e.Item.Cells(10).Text
                myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

                CaricaRimborsi()
                lblmess.Visible = False
                lblmess.Text = ""
            
            Else
                lblmess.Visible = True
                lblmess.Text = "Non è possibile Eliminare il Rimborso perchè è nello stato " & e.Item.Cells(6).Text & "."
            End If
        End If
    End Sub

    Private Sub dgRisultatoRicercaSedi_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicercaSedi.PageIndexChanged
        dgRisultatoRicercaSedi.SelectedIndex = -1
        dgRisultatoRicercaSedi.EditItemIndex = -1
        dgRisultatoRicercaSedi.CurrentPageIndex = e.NewPageIndex
        CaricaSediAttuazione()
    End Sub

    Function controlliSalvataggioServer(ByVal op As Operation) As Boolean

        If txtdataRiferimento.Text.Trim = String.Empty Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire la Data Riferimento."
            txtdataRiferimento.Focus()
            Return False
        End If

        Dim dataRiferimento As Date
        If (Date.TryParse(txtdataRiferimento.Text, dataRiferimento) = False) Then
            lblmess.Visible = True
            lblmess.Text = "Il formato della data è incorretto: il formato deve essere GG/MM/AAAA."
            txtdataRiferimento.Focus()
            Return False
        End If

        Dim DataInizioProgetto As Date
        Dim DataInizioProgettoGiorno As Integer
        Dim DataInizioProgettoMese As Integer
        Dim DataInizioProgettoAnno As Integer

        Dim DataFineProgetto As Date
        Dim DataFineProgettoGiorno As Integer
        Dim DataFineProgettoMese As Integer
        Dim DataFineProgettoAnno As Integer

        Dim DataRiferimentoInserita As Date
        Dim DataRiferimentoInseritaGiorno As Integer
        Dim DataRiferimentoInseritaMese As Integer
        Dim DataRiferimentoInseritaAnno As Integer

        DataInizioProgettoGiorno = lblDataInizio.Text.Split("/")(0)
        DataInizioProgettoMese = lblDataInizio.Text.Split("/")(1)
        DataInizioProgettoAnno = lblDataInizio.Text.Split("/")(2)
        DataInizioProgetto = New Date(DataInizioProgettoAnno, DataInizioProgettoMese, DataInizioProgettoGiorno)


        DataFineProgettoGiorno = lbldataFine.Text.Split("/")(0)
        DataFineProgettoMese = lbldataFine.Text.Split("/")(1)
        DataFineProgettoAnno = lbldataFine.Text.Split("/")(2)
        DataFineProgetto = New Date(DataFineProgettoAnno, DataFineProgettoMese, DataFineProgettoGiorno)

        DataRiferimentoInseritaGiorno = txtdataRiferimento.Text.Trim.Split("/")(0)
        DataRiferimentoInseritaMese = txtdataRiferimento.Text.Trim.Split("/")(1)
        DataRiferimentoInseritaAnno = txtdataRiferimento.Text.Trim.Split("/")(2)
        DataRiferimentoInserita = New Date(DataRiferimentoInseritaAnno, DataRiferimentoInseritaMese, DataRiferimentoInseritaGiorno)

        If DataRiferimentoInserita < DataInizioProgetto Then

            lblmess.Visible = True
            lblmess.Text = "La data di Riferimento non puo' essere antecedente alla data di inizio Progetto."
            txtdataRiferimento.Focus()
            Return False

        End If

        If DataRiferimentoInserita > DataFineProgetto Then

            lblmess.Visible = True
            lblmess.Text = "La data di Riferimento non puo' essere maggiore alla data di fine Progetto."
            txtdataRiferimento.Focus()
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

        If txtImportoConf.ReadOnly = False And txtImportoConf.Text.Trim <> String.Empty Then

            Dim importoConfermato As Double

            If (Double.TryParse(txtImportoConf.Text.Trim, importoConfermato) = False Or txtImportoConf.Text.Trim.Contains(",")) Then

                lblmess.Visible = True
                lblmess.Text = "E' necessario inserire un importo decimale."
                txtImportoConf.Focus()
                Return False

            End If

        End If

        If (op = Operation.Insert) Or (op = Operation.Update And txtSelFile.Value <> String.Empty) Then

            Dim PrefissoFile As String = ""

            If VerificaEstensioneFile(txtSelFile) = False Then
                lblmess.Visible = True
                lblmess.Text = "Il formato del file non è corretto.E' possibile associare documenti nel formato .PDF o .PDF.P7M"
                Return False
            End If
            If VerificaPrefissiDocumentiRimborsi(txtSelFile, Session("conn"), PrefissoFile) = False Then
                lblmess.Visible = True
                lblmess.Text = "Utilizzare uno dei prefissi consentiti per il nome del file."
                Return False
            End If


            Dim NomeUnivoco As String = ""
            Dim strPercorsoServer As String
            Dim i As Integer
            Dim myPath As New System.Web.UI.Page
            Dim msg As String = ""

            Dim annoCorrente As String
            Dim meseCorrente As String
            Dim giornoCorrente As String
            Dim objPercorsoFile As HtmlInputFile = txtSelFile

            annoCorrente = Year(Today).ToString

            If Month(Today) < 9 Then
                meseCorrente = "0" & Month(Today).ToString
            Else
                meseCorrente = Month(Today).ToString
            End If


            If Day(Today) < 9 Then
                giornoCorrente = "0" & Day(Today).ToString
            Else
                giornoCorrente = Day(Today).ToString
            End If

            'estraggo il nome del file 
            For i = Len(objPercorsoFile.Value) To 1 Step -1
                If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                    Exit For
                End If
                NomeUnivoco = Mid(objPercorsoFile.Value, i, 1) & NomeUnivoco
            Next

            Dim strNomeFile As String = annoCorrente & meseCorrente & giornoCorrente & "_" & Session("Utente")

            strPercorsoServer = myPath.Server.MapPath("upload") & "\" & strNomeFile & "_" & NomeUnivoco

            If File.Exists(strPercorsoServer) Then
                File.Delete(strPercorsoServer)
            End If

            objPercorsoFile.PostedFile.SaveAs(strPercorsoServer)

            Dim fs As New FileStream _
                    (strPercorsoServer, FileMode.Open, FileAccess.Read)
            Dim iLen As Integer = CInt(fs.Length - 1)
            'Dim iLen As Integer = CInt(fs.Length)
            Dim bBLOBStorage(iLen) As Byte

            If iLen < 0 Then
                File.Delete(strPercorsoServer)
                lblmess.Visible = True
                lblmess.Text = "Attenzione.Impossibile caricare un file vuoto."
                Return False
            Else
                If iLen > 20971520 Then
                    File.Delete(strPercorsoServer)
                    lblmess.Visible = True
                    lblmess.Text = "Attenzione.La dimensione massima è di 20 MB."
                    Return False
                Else
                    Dim numBytesToRead As Integer = CType(fs.Length, Integer)
                    Dim numBytesRead As Integer = 0

                    While (numBytesToRead > 0)
                        ' Read may return anything from 0 to numBytesToRead.
                        Dim n As Integer = fs.Read(bBLOBStorage, numBytesRead, _
                            numBytesToRead)
                        ' Break when the end of the file is reached.
                        If (n = 0) Then
                            Exit While
                        End If
                        numBytesRead = (numBytesRead + n)
                        numBytesToRead = (numBytesToRead - n)

                    End While
                    numBytesToRead = bBLOBStorage.Length

                    fs.Close()

                    Dim strHashValue As String
                    strHashValue = GeneraHash(bBLOBStorage)
                    Dim dtrHash As SqlDataReader

                    dtrHash = ClsServer.CreaDatareader("Select HashValue from EntitàDocumenti where IdEntità = " & Request.QueryString("IdEntita") & " and  HashValue = '" & strHashValue & "'", Session("conn"))
                    If dtrHash.HasRows = True Then
                        dtrHash.Close()
                        dtrHash = Nothing
                        File.Delete(strPercorsoServer)
                        lblmess.Visible = True
                        lblmess.Text = "Attenzione. Questo file è già presente."
                        Return False
                    Else
                        dtrHash.Close()
                        dtrHash = Nothing
                    End If

                End If

            End If

        End If

        

        Return True

    End Function

    Private Sub InsertFileInDB(ByVal op As Operation)

        Try

            If (op = Operation.Insert) Or (op = Operation.Update And txtSelFile.Value <> String.Empty) Then

                Dim PrefissoFile As String = ""

                CaricaDocumentoEntita(Request.QueryString("IdEntita"), Session("Utente"), txtSelFile, Session("conn"), PrefissoFile, Convert.ToInt32(txtidentitaRimborso.Value))

            End If
           

        Catch ex As Exception
            lblmess.Visible = True
            lblmess.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."

        End Try

    End Sub

    Private Sub CaricaDocumentoEntita(ByVal IdEntità As Integer, ByVal strUtente As String, ByVal objPercorsoFile As HtmlInputFile, ByRef cnLocal As SqlConnection, ByVal PrefissoFile As String, ByVal idEntitaRimborso As Integer)

        Dim NomeUnivoco As String = ""
        Dim strPercorsoServer As String
        Dim i As Integer
        Dim myPath As New System.Web.UI.Page

        Dim annoCorrente As String
        Dim meseCorrente As String
        Dim giornoCorrente As String

        annoCorrente = Year(Today).ToString

        If Month(Today) < 9 Then
            meseCorrente = "0" & Month(Today).ToString
        Else
            meseCorrente = Month(Today).ToString
        End If


        If Day(Today) < 9 Then
            giornoCorrente = "0" & Day(Today).ToString
        Else
            giornoCorrente = Day(Today).ToString
        End If

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeUnivoco = Mid(objPercorsoFile.Value, i, 1) & NomeUnivoco
        Next

        Dim strNomeFile As String = annoCorrente & meseCorrente & giornoCorrente & "_" & strUtente

        strPercorsoServer = myPath.Server.MapPath("upload") & "\" & strNomeFile & "_" & NomeUnivoco

        If File.Exists(strPercorsoServer) Then
            File.Delete(strPercorsoServer)
        End If

        objPercorsoFile.PostedFile.SaveAs(strPercorsoServer)

        Dim fs As New FileStream _
                (strPercorsoServer, FileMode.Open, FileAccess.Read)
        Dim iLen As Integer = CInt(fs.Length - 1)
        'Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        Dim numBytesToRead As Integer = CType(fs.Length, Integer)
        Dim numBytesRead As Integer = 0

        While (numBytesToRead > 0)
            ' Read may return anything from 0 to numBytesToRead.
            Dim n As Integer = fs.Read(bBLOBStorage, numBytesRead, _
                numBytesToRead)
            ' Break when the end of the file is reached.
            If (n = 0) Then
                Exit While
            End If
            numBytesRead = (numBytesRead + n)
            numBytesToRead = (numBytesToRead - n)

        End While
        numBytesToRead = bBLOBStorage.Length

        fs.Close()

        Dim strHashValue As String
        strHashValue = GeneraHash(bBLOBStorage)

        InserimentoDocumentoRimborso(IdEntità, NomeUnivoco, bBLOBStorage, strUtente, strHashValue, idEntitaRimborso, cnLocal)

        If File.Exists(strPercorsoServer) Then
            File.Delete(strPercorsoServer)
        End If

    End Sub

    Private Sub InserimentoDocumentoRimborso(ByVal IdEntità As Integer, ByVal NomeUnivoco As String, ByVal bBLOBStorage() As Byte, ByVal strUtente As String, ByVal strHashValue As String, ByVal idEntitaRimborso As Integer, ByVal cnLocal As SqlClient.SqlConnection)

        'sub che consente l'inserimento dei documenti da associare al progetto
        Dim cmd As SqlCommand = New SqlCommand _
        ("INSERT INTO EntitàDocumenti (IdEntità,BinData, FileName,DataInserimento,UsernameInserimento,HashValue,IdEntitàRimborso ) " _
            & "VALUES(@idEntità,@blob_data,@blob_filename ,getdate(),@utente,@hash_value,@IdEntitàRimborso)", cnLocal)
        cmd.CommandType = CommandType.Text
        cmd.Parameters.Add("@idEntità", SqlDbType.Int)
        cmd.Parameters("@idEntità").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_filename", SqlDbType.VarChar)
        cmd.Parameters("@blob_filename").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@blob_data", SqlDbType.Image) 'varbinary???
        cmd.Parameters("@blob_data").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@utente", SqlDbType.VarChar)
        cmd.Parameters("@utente").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@hash_value", SqlDbType.VarChar)
        cmd.Parameters("@hash_value").Direction = ParameterDirection.Input
        cmd.Parameters.Add("@IdEntitàRimborso", SqlDbType.Int)
        cmd.Parameters("@IdEntitàRimborso").Direction = ParameterDirection.Input


        cmd.Parameters("@idEntità").Value = IdEntità
        cmd.Parameters("@blob_filename").Value = NomeUnivoco
        cmd.Parameters("@blob_data").Value = bBLOBStorage
        cmd.Parameters("@utente").Value = strUtente
        cmd.Parameters("@hash_value").Value = strHashValue
        cmd.Parameters("@IdEntitàRimborso").Value = idEntitaRimborso

        cmd.ExecuteNonQuery()
        cmd.Dispose()
        cmd = Nothing
    End Sub

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

    Private Function VerificaEstensioneFile(ByVal objPercorsoFile As HtmlInputFile) As Boolean
        'sono accettati solo documento con estensione .pdf e .pdf.p7m
        Dim NomeFile As String = ""
        Dim i As Integer
        'Dim strEstensione As String
        'Dim strEstensione1 As String
        VerificaEstensioneFile = False

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeFile = Mid(objPercorsoFile.Value, i, 1) & NomeFile
        Next
        'strEstensione = Mid(NomeFile, InStr(NomeFile, "."), Len(NomeFile))

        'strEstensione = Right(NomeFile, 4)
        'strEstensione1 = Right(NomeFile, 8)
        If UCase(Right(NomeFile, 4)) = ".PDF" Or UCase(Right(NomeFile, 8)) = ".PDF.P7M" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function VerificaPrefissiDocumentiRimborsi(ByVal objPercorsoFile As HtmlInputFile, ByVal conn As SqlClient.SqlConnection, ByRef PrefissoFile As String) As Boolean
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        Dim StrSql As String
        Dim DsPrefisso As New DataSet
        VerificaPrefissiDocumentiRimborsi = False
        'carico in un dt i prefissi dei documenti
        StrSql = "Select Prefisso From PrefissiEntitàDocumenti where ModalitàInvio='FUTURO' and TipoInserimento=2 "
        DsPrefisso = ClsServer.DataSetGenerico(StrSql, conn)

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeFile = Mid(objPercorsoFile.Value, i, 1) & NomeFile
        Next
        Prefisso = Split(NomeFile, "_")
        'PrefissoFile = Prefisso & "_"
        If Prefisso.Length = 1 Then
            'errore
        Else
            For Each r As DataRow In DsPrefisso.Tables(0).Rows
                If r.Item("Prefisso") = UCase(Prefisso(0).ToString) & "_" Then
                    PrefissoFile = UCase(Prefisso(0).ToString) & "_"
                    VerificaPrefissiDocumentiRimborsi = True
                    Exit For
                End If
            Next
        End If
        Return VerificaPrefissiDocumentiRimborsi
    End Function

    Private Sub DeleteFileInDB()

        If (txtSelFile.Value <> String.Empty) Then

            Dim cnLocal As SqlClient.SqlConnection
            'sub che consente l'inserimento dei documenti da associare al progetto
            Dim cmd As SqlCommand = New SqlCommand _
            ("DELETE FROM dbo.EntitàDocumenti  " _
                & "WHERE IdEntitàRimborso = @IdEntitàRimborso", Session("conn"))

            cmd.CommandType = CommandType.Text

            cmd.Parameters.Add("@IdEntitàRimborso", SqlDbType.Int)
            cmd.Parameters("@IdEntitàRimborso").Direction = ParameterDirection.Input

            cmd.Parameters("@IdEntitàRimborso").Value = Convert.ToInt32(txtidentitaRimborso.Value)

            cmd.ExecuteNonQuery()
            cmd.Dispose()
            cmd = Nothing

        End If


    End Sub

    Private Function RecuperaDocumentoRimborsi(ByVal idEntitaDocumento As Integer, ByRef cnLocal As SqlConnection) As String
        Dim da As New SqlDataAdapter _
            ("SELECT BinData,FileName, HashValue, mese, anno, usernameinserimento FROM EntitàDocumenti WHERE idEntitàDocumento = " & idEntitaDocumento, cnLocal)
        Dim cb As SqlCommandBuilder = New SqlCommandBuilder(da)
        Dim ds As New DataSet
        Dim mese As String
        Dim anno As String
        Dim user As String
        Dim paht As String

        Try
            Dim oblLocalHLink As New HyperLink

            anno = Year(Today).ToString

            If Month(Today) < 9 Then
                mese = "0" & Month(Today).ToString
            Else
                mese = Month(Today).ToString
            End If

            da.Fill(ds, "_FileTest")
            Dim rw As DataRow
            rw = ds.Tables("_FileTest").Rows(0)

            ' Make sure you have some rows
            Dim i As Integer = ds.Tables("_FileTest").Rows.Count
            If i > 0 Then
                Dim bBLOBStorage() As Byte = _
                ds.Tables("_FileTest").Rows(0)("BinData")

                user = ds.Tables("_FileTest").Rows(0)("usernameinserimento")

                oblLocalHLink.Text = anno & mese & ds.Tables("_FileTest").Rows(0)("Filename")
                oblLocalHLink.NavigateUrl = FileByteToPathRimborsi(bBLOBStorage, anno & mese & "_" & user & "_" & ds.Tables("_FileTest").Rows(0)("Filename"), ds.Tables("_FileTest").Rows(0)("HashValue"))

                paht = FileByteToPathRimborsi(bBLOBStorage, anno & mese & "_" & user & "_" & ds.Tables("_FileTest").Rows(0)("Filename"), ds.Tables("_FileTest").Rows(0)("HashValue"))
            End If

            Return paht
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function

    Private Function FileByteToPathRimborsi(ByVal dataBuffer As Byte(), ByVal nomeFile As String, ByVal HashValue As String) As String
        'dichiaro una variabile byte che bufferizza (carica in memoria) il file template richiesto
        'e trasformato in base64
        Dim fs As FileStream
        Dim myPath As New System.Web.UI.Page

        If File.Exists(myPath.Server.MapPath("download") & "\" & nomeFile) Then
            File.Delete(myPath.Server.MapPath("download") & "\" & nomeFile)
        End If
        'passo il template al filestream
        fs = New FileStream(myPath.Server.MapPath("download") & "\" & nomeFile, FileMode.Create, FileAccess.Write)
        'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
        If (dataBuffer.Length > 0) Then
            fs.Write(dataBuffer, 0, dataBuffer.Length)
        End If

        'chiudo lo streaming
        fs.Close()
        Return "download\" & nomeFile
    End Function


End Class