Imports System.Drawing
Public Class WfrmVisualizzaElencoDocumentiformatori
    Inherits System.Web.UI.Page
    Dim dtrgenerico As SqlClient.SqlDataReader

    Const STATO_DA_VALIDARE As Byte = 0
    Const STATO_VALIDO As Byte = 1
    Const STATO_INVALIDO As Byte = 2
    Const INDEX_DGELENCODOCUMENTI_VALIDA_DOCUMENTO As Byte = 8
    Const INDEX_DGELENCODOCUMENTI_INVALIDA_DOCUMENTO As Byte = 9
    Const INDEX_DGELENCODOCUMENTI_STATO_DOCUMENTO As Byte = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        '--------------------INIZIO SICUREZZA-----------------------------------
        'IDENTE  Session("IdEnte")
        'CODICEENTE  NZ Session("codiceregione")  or Session("txtCodEnte")

        Dim strATTIVITA As Integer = -1
        Dim strBANDOATTIVITA As Integer = -1
        Dim strENTEPERSONALE As Integer = -1
        Dim strENTITA As Integer = -1
        Dim strIDENTE As Integer = -1

        If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdProg"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        Else
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Response.Redirect("wfrmAnomaliaDati.aspx")

        End If

        '---------------------FINE SICUREZZA---------------------------
        If Page.IsPostBack = False Then
            hlDownload.Visible = False
            CaricaDatiProgetto(Request.QueryString("IdProg"))
            CaricaComboTipoDocumenti()
            LoadGriglia()
        End If
    End Sub
    Private Sub CaricaDatiProgetto(ByVal IdAttività As Integer)
        Dim strSql As String
        Dim rstVol As SqlClient.SqlDataReader


        ddlTipoDocumenti.Items.Clear()
        strSql = "SELECT CodiceEnte, Titolo FROM Attività where IdAttività = " & IdAttività
        rstVol = ClsServer.CreaDatareader(strSql, Session("conn"))

        If rstVol.HasRows Then
            rstVol.Read()
            lblCodiceProg.Text = rstVol("CodiceEnte")
            lblTitolo.Text = rstVol("Titolo")
        End If

        If Not rstVol Is Nothing Then
            rstVol.Close()
            rstVol = Nothing
        End If
    End Sub

    Private Sub CaricaComboTipoDocumenti()
        Dim strSql As String
        Dim rstDoc As SqlClient.SqlDataReader


        ddlTipoDocumenti.Items.Clear()
        strSql = "SELECT idPrefisso as idPrefisso, Prefisso as Prefisso, ordine  FROM  PrefissiEntitàDocumenti where TipoInserimento=3 UNION Select 0, 'Seleziona',0  order by Ordine"
        rstDoc = ClsServer.CreaDatareader(strSql, Session("conn"))


        If rstDoc.HasRows Then
            ddlTipoDocumenti.DataSource = rstDoc
            ddlTipoDocumenti.DataTextField = "Prefisso"
            ddlTipoDocumenti.DataValueField = "IdPrefisso"
            ddlTipoDocumenti.DataBind()
        End If

        If Not rstDoc Is Nothing Then
            rstDoc.Close()
            rstDoc = Nothing
        End If
    End Sub

    Private Sub CaricaElencoDocumenti(ByVal IdAttività As Integer, TipoDocumento As String)

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_PROGETTI_ELENCO_DOCUMENTI_FORMATORI]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdAttività", SqlDbType.VarChar).Value = IdAttività
            sqlDAP.SelectCommand.Parameters.Add("@TipoDocumento", SqlDbType.VarChar).Value = TipoDocumento
            sqlDAP.Fill(dataSet)

            ' Session("appDtsRisRicerca") = dataSet
            dgElencoDocumenti.DataSource = dataSet
            dgElencoDocumenti.DataBind()

        Catch ex As Exception
            LblMsgFile.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            LblMsgFile.ForeColor = Color.Red
            Exit Sub
        End Try

    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        dgElencoDocumenti.CurrentPageIndex = 0
        hlDownload.Visible = False
        lblmessaggio.Visible = False
        LoadGriglia()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("ProVengoDa") = "6" Then
            Response.Redirect("WfrmCheckListDettaglioFormazione.aspx?idLista=" & Request.QueryString("IdLista") & "&IdAttivita=" & Request.QueryString("IdProg"))
        Else
            Response.Redirect("ricercaprogettiformatori.aspx")
        End If

    End Sub

    Private Sub dgElencoDocumenti_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgElencoDocumenti.ItemCommand
        Dim strSql As String
        Dim rstDoc As SqlClient.SqlDataReader
        Select Case e.CommandName
            Case "Download"
                hlDownload.Visible = True
                hlDownload.NavigateUrl = clsGestioneDocumenti.RecuperaDocumentoPresenzeFormatori(e.Item.Cells(0).Text, Session("conn"))
                hlDownload.Text = e.Item.Cells(2).Text
                hlDownload.Target = "_blank"
            Case "Elimina"
                strSql = " SELECT idPrefisso as idPrefisso, Prefisso as Prefisso, ordine  " & _
                         " FROM  PrefissiEntitàDocumenti where tipoinserimento=3 " & _
                         " and left(PrefissiEntitàDocumenti.prefisso, charindex('_',PrefissiEntitàDocumenti.Prefisso)-1) = '" & e.Item.Cells(5).Text & "'"
                rstDoc = ClsServer.CreaDatareader(strSql, Session("conn"))
                If rstDoc.HasRows = True Then
                    If Not rstDoc Is Nothing Then
                        rstDoc.Close()
                        rstDoc = Nothing
                    End If
                    'cancello la sede 
                    clsGestioneDocumenti.RimuoviDocumentoPresenzeFormatori(e.Item.Cells(0).Text, Session("Conn"))
                    'messaggio di conferma cancellazione
                    lblmessaggio.Text = "Cancellazione Effettuata"
                    lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                    LblMsgFile.Text = ""
                    'load griglia
                    LoadGriglia()
                Else
                    If Not rstDoc Is Nothing Then
                        rstDoc.Close()
                        rstDoc = Nothing
                    End If
                    LblMsgFile.Text = ""
                    lblmessaggio.Text = "Impossibile cancellare il documento selezionato."
                    lblmessaggio.ForeColor = Color.red
                End If
            Case "Valida"
                If VerificoEsistenzaCheckList(CInt(e.Item.Cells(0).Text)) = True Then
                    lblmessaggio.Text = "Impossibile modificare lo stato del documento perchè associato ad una CheckList Confermata."
                    lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#C00000")
                Else
                    clsGestioneDocumenti.AggiornaStatoAttivitaDocumentiFormazione(e.Item.Cells(0).Text, STATO_VALIDO, Session("Utente"), Session("Conn"))
                    LblMsgFile.Text = ""
                    LoadGriglia()
                End If
            Case "Invalida"
                If VerificoEsistenzaCheckList(CInt(e.Item.Cells(0).Text)) = True Then
                    lblmessaggio.Text = "Impossibile modificare lo stato del documento perchè associato ad una CheckList Confermata."
                    lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#C00000")
                Else
                    clsGestioneDocumenti.AggiornaStatoAttivitaDocumentiFormazione(e.Item.Cells(0).Text, STATO_INVALIDO, Session("Utente"), Session("Conn"))
                    LblMsgFile.Text = ""
                    LoadGriglia()
                End If
        End Select

    End Sub

    Protected Sub cmdUpload_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click
        Try
            Dim msg As String
            Dim PrefissoFile As String = ""
            LblMsgFile.Text = ""
            lblmessaggio.Text = ""
            'hlScarica.Visible = False
            hlDownload.Visible = False
            If txtSelFile.Value = "" Then
                LblMsgFile.Text = "E' necessario selezionare il file."
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If
            If clsGestioneDocumenti.VerificaEstensioneFileVolontario(txtSelFile) = False Then
                LblMsgFile.Text = "Il formato del file non è corretto.E' possibile associare documenti nel formato .PDF o .PDF.P7M"
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If
            If clsGestioneDocumenti.VerificaPrefissiDocumentoFormatori(txtSelFile, Session("conn"), PrefissoFile) = False Then
                LblMsgFile.Text = "Utilizzare uno dei prefissi consentiti per il nome del file."
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If
            msg = clsGestioneDocumenti.CaricaDocumentoPresenzeFormatori(Request.QueryString("IdProg"), Session("Utente"), txtSelFile, Session("conn"), PrefissoFile)
            If msg = "ok" Then
                LblMsgFile.Text = "Documento Associato"
                LblMsgFile.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
            Else
                LblMsgFile.Text = msg
                LblMsgFile.ForeColor = Color.Red
            End If

            CaricaElencoDocumenti(Request.QueryString("IdProg"), ddlTipoDocumenti.SelectedValue)

        Catch ex As Exception
            LblMsgFile.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            LblMsgFile.ForeColor = Color.Red
        Finally
            cmdUpload.Enabled = True
        End Try

    End Sub

    Sub LoadGriglia()
        CaricaElencoDocumenti(Request.QueryString("IdProg"), ddlTipoDocumenti.SelectedValue)
        If dgElencoDocumenti.Items.Count = 0 Then
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Nessun documento estratto."
        End If

        If Session("TipoUtente") = "U" Then
            dgElencoDocumenti.Columns(7).Visible = True
            dgElencoDocumenti.Columns(8).Visible = True
            dgElencoDocumenti.Columns(9).Visible = True
        Else
            dgElencoDocumenti.Columns(7).Visible = True
            dgElencoDocumenti.Columns(8).Visible = False
            dgElencoDocumenti.Columns(9).Visible = False
        End If
    End Sub
    Private Sub dgElencoDocumenti_ItemDataBound(ByVal source As Object, ByVal e As DataGridItemEventArgs) Handles dgElencoDocumenti.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            If String.Equals(e.Item.Cells(INDEX_DGELENCODOCUMENTI_STATO_DOCUMENTO).Text, STATO_VALIDO.ToString, StringComparison.InvariantCultureIgnoreCase) Then
                e.Item.Cells(INDEX_DGELENCODOCUMENTI_VALIDA_DOCUMENTO).Text = String.Empty
            ElseIf String.Equals(e.Item.Cells(INDEX_DGELENCODOCUMENTI_STATO_DOCUMENTO).Text, STATO_INVALIDO.ToString, StringComparison.InvariantCultureIgnoreCase) Then
                e.Item.Cells(INDEX_DGELENCODOCUMENTI_INVALIDA_DOCUMENTO).Text = String.Empty
            End If

        End If
    End Sub

    Private Function VerificoEsistenzaCheckList(ByVal IdAttivitàDocumentoFormazione As Integer) As Boolean

        Dim strsql As String
        Dim MyDataset As DataSet
        Dim blnVerifica As Boolean = False

        strsql = " Select * from VW_CHECKLIST_DOCUMENTI_FORMAZIONE where IdAttivitàDocumentoFormazione=" & IdAttivitàDocumentoFormazione
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
        If MyDataset.Tables(0).Rows.Count <> 0 Then
            blnVerifica = True
        End If
        MyDataset.Dispose()
        Return blnVerifica
    End Function

    Protected Sub dgElencoDocumenti_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgElencoDocumenti.SelectedIndexChanged

    End Sub
End Class