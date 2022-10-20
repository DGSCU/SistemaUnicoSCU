

Public Class WfrmSIGEDElencoAllegati
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents imgAttesa As System.Web.UI.WebControls.ImageButton

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Dim dt As New DataTable
    Dim SIGED As clsSiged

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblmessaggio.Text = ""
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        Try
            If Page.IsPostBack = False Then


                Dim pNumeroProtocollo As String = Request.QueryString("NumeroProtocollo")
                Dim pDataProtocollo As String = Request.QueryString("DataProtocollo")

                Dim strNome As String
                Dim strCognome As String
                Dim strSQL As String
                Dim dsUser As DataSet

                strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & Session("Utente") & "'"

                dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

                If dsUser.Tables(0).Rows.Count <> 0 Then
                    strNome = dsUser.Tables(0).Rows(0).Item("Nome")
                    strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
                End If

                CaricaAllegati(strCognome, strNome, Request.QueryString("NumeroProtocollo"), Request.QueryString("DataProtocollo"))

                ''                Dim strLocal As String = Year(CDate(pDataProtocollo)) & "-" & IIf(Len(Month(CDate(pDataProtocollo))) = 1, "0" & Month(CDate(pDataProtocollo)), Month(CDate(pDataProtocollo))) & "-" & IIf(Len(Day(CDate(pDataProtocollo))) = 1, "0" & Day(CDate(pDataProtocollo)), Day(CDate(pDataProtocollo)))

                'wsOUT = wsIN.INDICE_PROTOCOLLO(strCognome, strNome, pNumeroProtocollo, Year(CDate(pDataProtocollo))) 'pDataProtocollo)

                'sEsito = Left(wsOUT.ESITO, 4)

                'If sEsito = "0000" Then
                '    wsELENCO = wsOUT.ELENCO_DOCUMENTI

                '    dt.Columns.Add(New DataColumn("Codice Documento", GetType(String)))
                '    dt.Columns.Add(New DataColumn("Descrizione", GetType(String)))
                '    dt.Columns.Add(New DataColumn("Documento Firmato", GetType(String)))

                '    For riga = LBound(wsELENCO) To UBound(wsELENCO)
                '        dr = dt.NewRow
                '        dr(0) = wsELENCO(riga).CODICE_DOCUMENTO
                '        dr(1) = wsELENCO(riga).DESCRIZIONE
                '        dr(2) = wsELENCO(riga).FLAG_PRINCIPALE
                '        dt.Rows.Add(dr)
                '    Next

                '    dgRisultatoRicerca.DataSource = dt
                '    dgRisultatoRicerca.DataBind()
                '    Session("dtRisulatato") = dt
                'Else
                '    lblmessaggio.Text = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))
                'End If

            End If
        Catch ex As Exception
            lblmessaggio.Text = "Errore imprevisto: " & ex.Message
        End Try
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.Item.ItemIndex <> -1 Then

            Dim pNumeroProtocollo As String = Request.QueryString("NumeroProtocollo")
            Dim pDataProtocollo As String = Request.QueryString("DataProtocollo")
            Dim pIdentificativoDOC As String
            Dim pCodiceFascicolo As String = Request.QueryString("CodiceFascicolo")
            Dim pNomeFile As String
            pIdentificativoDOC = Replace(e.Item.Cells(1).Text, "#", "%23")
            'pNomeFile = Replace(e.Item.Cells(4).Text, "&", "")
            pNomeFile = Replace(Replace(Replace(Replace(e.Item.Cells(4).Text, "&", ""), "#", ""), "’", "'"), "+", " ")
            Response.Redirect("WfrmSIGEDDocumento.aspx?NomeFile=" & pNomeFile & "&NumeroProtocollo=" & pNumeroProtocollo & "&DataProtocollo=" & pDataProtocollo & "&CodiceFascicolo=" & pCodiceFascicolo & "&IdentificativoDOC=" & pIdentificativoDOC)

        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("dtRisulatato")
        dgRisultatoRicerca.DataBind()
    End Sub

    Private Sub CaricaAllegati(ByVal Cognome As String, ByVal Nome As String, ByVal NProtocollo As String, ByVal DataProtocollo As String)
        Dim wsOUT As New WS_SIGeD.INDICE_PROTOCOLLO
        Dim wsELENCO() As WS_SIGeD.ALLEGATO_DOCUMENTO_TROVATO
        Dim sEsito As String
        Dim sNumeroDoc As String
        Dim dr As DataRow
        Dim riga As Integer


        SIGED = New clsSiged("", Nome, Cognome)
        If SIGED.Codice_Esito <> 0 Then
            lblmessaggio.Text = SIGED.Esito
            Exit Sub
        End If


        wsOUT = SIGED.SIGED_IndiceProtocollo(SIGED.SIGED_CodiceProtocolloCompleto(Year(DataProtocollo), CInt(NProtocollo)))

        sEsito = Left(wsOUT.ESITO, 5)

        If sEsito = "00000" Then

            wsELENCO = wsOUT.ELENCO_DOCUMENTI

            If Not wsELENCO Is Nothing Then
                dt.Columns.Add(New DataColumn("iddocumento", GetType(String)))
                dt.Columns.Add(New DataColumn("Codice Documento", GetType(String)))
                dt.Columns.Add(New DataColumn("Descrizione", GetType(String)))
                dt.Columns.Add(New DataColumn("Nome File", GetType(String)))

                For riga = LBound(wsELENCO) To UBound(wsELENCO)
                    If wsELENCO(riga).NOMEFILE <> "" Then
                        dr = dt.NewRow
                        SIGED.NormalizzaCodice(wsELENCO(riga).CODICEALLEGATO)
                        dr(0) = wsELENCO(riga).CODICEALLEGATO 'es. di codice allegato 2011#PROTO#3436#ALLE#1 
                        dr(1) = SIGED.CodiceNormalizzato 'es. di codice allegato 2011#PROTO#3436#ALLE#1 
                        dr(2) = wsELENCO(riga).DESCRIZIONE
                        dr(3) = wsELENCO(riga).NOMEFILE
                        dt.Rows.Add(dr)
                    End If

                Next
                dgRisultatoRicerca.DataSource = dt
                dgRisultatoRicerca.DataBind()
                Session("dtRisulatato") = dt
            Else
                lblmessaggio.Text = "Nessun documento presente nel protocollo."
            End If
        Else
            lblmessaggio.Text = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))
        End If
        SIGED.SIGED_Chiudi_Autenticazione(Nome, Cognome)
    End Sub

    Private Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged

    End Sub
End Class
