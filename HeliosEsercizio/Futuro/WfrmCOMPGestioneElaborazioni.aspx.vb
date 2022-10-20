Public Class WfrmCOMPGestioneElaborazioni
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtsGenerico As DataSet
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim dtsRisRicerca As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then
            If ClsUtility.ForzaCaricamentoPaghe(Session("Utente"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            CaricaComboTipo()
            CaricaComboStati()
            CaricaGriglia()

        Else
            
            lblmess.Visible = False
            lblmess.Text = ""
        End If
    End Sub
    Private Sub CaricaComboTipo()
        strsql = "select IdTipoPagamento,Descrizione from COMP_TipiPagamento UNION Select -1, 'Seleziona' order by 1 "
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        cboTipo.DataSource = dtrgenerico
        cboTipo.DataTextField = "Descrizione"
        cboTipo.DataValueField = "IdTipoPagamento"
        cboTipo.DataBind()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Private Sub CaricaComboStati()
        strsql = "select IdStatoElaborazione,StatoElaborazione from COMP_StatiElaborazioni UNION Select -1, 'Seleziona' order by 1 "
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        CboStato.DataSource = dtrgenerico
        CboStato.DataTextField = "StatoElaborazione"
        CboStato.DataValueField = "IdStatoElaborazione"
        CboStato.DataBind()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Private Sub CaricaGriglia()
        strsql = "SELECT COMP_Elaborazioni.IdElaborazione, COMP_TipiPagamento.Descrizione AS Tipo, COMP_StatiElaborazioni.StatoElaborazione, COMP_Elaborazioni.DataValuta, COMP_Elaborazioni.Descrizione FROM COMP_Elaborazioni INNER JOIN COMP_TipiPagamento ON COMP_Elaborazioni.IdTipoPagamento = COMP_TipiPagamento.IdTipoPagamento INNER JOIN COMP_StatiElaborazioni ON COMP_Elaborazioni.IdStatoElaborazione = COMP_StatiElaborazioni.IdStatoElaborazione where 1=1 order by COMP_Elaborazioni.IdElaborazione desc"
        dgRisultatoRicercaElaborazioni.DataSource = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgRisultatoRicercaElaborazioni.DataBind()
        If VerificaStatoElaborazione() = True Then
            dgRisultatoRicercaElaborazioni.Columns(0).Visible = False
            lblmess.Visible = True
            lblmess.Text = "Elaborazione in corso... In Lavorazione. Effettuare una nuova ricerca per aggiornare la pagina e controllare lo stato della elaborazione."
        End If
        dgRisultatoRicercaElaborazioni.Visible = True
    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Protected Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        If VerificaStatoElaborazione() = False Then
            dgRisultatoRicercaElaborazioni.Columns(0).Visible = True
        Else
            dgRisultatoRicercaElaborazioni.Columns(0).Visible = False

        End If

        Dim strsql As String
        strsql = "SELECT COMP_Elaborazioni.IdElaborazione, COMP_TipiPagamento.Descrizione AS Tipo, COMP_StatiElaborazioni.StatoElaborazione, COMP_Elaborazioni.DataValuta, COMP_Elaborazioni.Descrizione FROM COMP_Elaborazioni INNER JOIN COMP_TipiPagamento ON COMP_Elaborazioni.IdTipoPagamento = COMP_TipiPagamento.IdTipoPagamento INNER JOIN COMP_StatiElaborazioni ON COMP_Elaborazioni.IdStatoElaborazione = COMP_StatiElaborazioni.IdStatoElaborazione where 1=1 "
        If Trim(cboTipo.SelectedItem.Text) <> "Seleziona" Then
            strsql = strsql & " and COMP_TipiPagamento.Descrizione='" & cboTipo.SelectedItem.Text & "'"
        End If

       
        If TxtDataValutaDal.Text <> vbNullString And TxtDataValutaAl.Text <> vbNullString Then
            strsql = strsql & " AND COMP_Elaborazioni.DataValuta between '" & TxtDataValutaDal.Text & "' and '" & TxtDataValutaAl.Text & "' "
        Else
            If TxtDataValutaDal.Text <> vbNullString Then
                strsql = strsql & " AND COMP_Elaborazioni.DataValuta >= '" & TxtDataValutaDal.Text & "'"
            End If
            If TxtDataValutaAl.Text <> vbNullString Then
                strsql = strsql & " AND COMP_Elaborazioni.DataValuta <= '" & TxtDataValutaAl.Text & "'"
            End If

        End If


        If Trim(CboStato.SelectedItem.Text) <> "Seleziona" Then
            strsql = strsql & " and COMP_StatiElaborazioni.StatoElaborazione='" & CboStato.SelectedItem.Text & "'"
        End If

        strsql = strsql & " order by COMP_Elaborazioni.IdElaborazione desc"

        dtsRisRicerca = ClsServer.DataSetGenerico(strsql, Session("conn"))
        Session("Ricerca") = dtsRisRicerca
        dgRisultatoRicercaElaborazioni.DataSource = dtsRisRicerca
        dgRisultatoRicercaElaborazioni.DataBind()
    End Sub

    Private Sub dgRisultatoRicercaElaborazioni_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicercaElaborazioni.ItemCommand
        If e.CommandName = "Modifica" Then

            Response.Redirect("WfrmCOMPModificaConsulta.aspx?IdElaborazione=" + e.Item.Cells(1).Text)
          
        End If
    End Sub

    Private Sub HplInserisciElaborazione_Click(sender As Object, e As System.EventArgs) Handles HplInserisciElaborazione.Click
        Response.Redirect("WfrmCOMPModificaConsulta.aspx")
    End Sub

    Private Function VerificaStatoElaborazione() As Boolean
        Dim statoElaborazione As Integer
        strsql = "SELECT IdStatoElaborazione from COMP_Elaborazioni where IdStatoElaborazione=0"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            ''where idelaborazione=" & CInt(Request.QueryString("IdElaborazione")) & "
            'statoElaborazione = IIf(Not IsDBNull(CInt(dtrgenerico("IdStatoElaborazione"))), dtrgenerico("IdStatoElaborazione"), "")
            'If statoElaborazione = 0 Then
            Return True
            'End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return False
    End Function
End Class