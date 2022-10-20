Imports System.Data.SqlClient

Public Class Wfrm_Cooprogettazione
    Inherits System.Web.UI.Page
    Dim dtrgenerico As SqlClient.SqlDataReader
#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
#End Region
    Private Sub DataGridSource(ByVal DtAppoggio As DataTable)
        If DtAppoggio.Rows.Count <> 0 Then
            If Not IsDBNull(DtAppoggio.Rows(0).Item("progetto")) Then Me.lblProgetto.Text = DtAppoggio.Rows(0).Item("progetto") Else Me.lblProgetto.Text = ""
            Me.lblCompetenza.Text = DtAppoggio.Rows(0).Item("Competenza")
            Me.lblSettore.Text = DtAppoggio.Rows(0).Item("settore")
            Me.lblArea.Text = DtAppoggio.Rows(0).Item("area")
            DgEntiAccorpati.CurrentPageIndex = 0
            If Not IsDBNull(DtAppoggio.Rows(0).Item("idente")) Then
                DgEntiAccorpati.Visible = True
                DgEntiAccorpati.DataSource = DtAppoggio
                DgEntiAccorpati.DataBind()
            Else
                DgEntiAccorpati.Visible = False
            End If
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Request.QueryString("IdAttivita") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then

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


        End If
        If Not IsPostBack Then
            LblErrore.Text = ""
            DgEnti.Visible = False

            Dim cls As New clsCooprogettazione(CInt(Request.QueryString("IdAttivita")))
            Dim DtAppoggio As DataTable = cls.SP_Popola_Griglia_Coprogetti(Session("conn"))
            DataGridSource(DtAppoggio)
            'mod il 23/01/2013 rendo in lettura la maschera se provendo dalla scheda progetto (no accettazione, no valutazione) 
            If Request.QueryString("popup") <> "1" And Request.QueryString("Modifica") = "0" Then
                DgEntiAccorpati.Columns(6).Visible = False
            End If

        End If
    End Sub
#Region " METODI FUNZIONALI"
    Private Sub ImgRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgRicerca.Click
        LblErrore.Text = String.Empty
        If (txtCodEnte.Text = String.Empty And txtDenominazione.Text = String.Empty) Then
            LblErrore.Text = "E' obbligatorio valorizzare almeno uno dei campi di ricerca."
        Else

            Dim cls As New clsCooprogettazione(CInt(Request.QueryString("IdAttivita")))
            Dim condizione As String
            DgEnti.Visible = True
            If Me.txtCodEnte.Text <> "" Then condizione += " and Codiceregione='" + Replace(Me.txtCodEnte.Text, "'", "''") + "'"
            If Me.txtDenominazione.Text <> "" Then condizione += " and Denominazione like '" + Replace(Me.txtDenominazione.Text, "'", "''") + "%'"

            DgEnti.CurrentPageIndex = 0
            DgEnti.DataSource = cls.SP_Ricerca_Enti(Session("conn"), condizione)
            DgEnti.DataBind()
            Session("DTENTI") = DgEnti.DataSource
        End If
    End Sub
    Private Sub ImgChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgChiudi.Click
        'Response.Redirect("WfrmRicercaEnti.aspx")

        If Request.QueryString("popup") <> "1" Then
            Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
            'Response.Redirect("TabProgetti.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        Else
            Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?popup=" & Request.QueryString("popup") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
            'Response.Redirect("TabProgetti.aspx?popup=" & Request.QueryString("popup") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        End If
    End Sub
#End Region
#Region " EVENTI DATAGRID"
    Private Sub DgEnti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DgEnti.PageIndexChanged

        DgEnti.CurrentPageIndex = e.NewPageIndex
        DgEnti.DataSource = Session("DTENTI").copy
        DgEnti.DataBind()

    End Sub

    Private Sub DgEntiAccorpati_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DgEntiAccorpati.PageIndexChanged
        Dim cls As New clsCooprogettazione(CInt(Request.QueryString("IdAttivita")))
        Me.DgEntiAccorpati.CurrentPageIndex = e.NewPageIndex
        DgEntiAccorpati.DataSource = cls.SP_Popola_Griglia_Coprogetti(Session("conn"))
        DgEntiAccorpati.DataBind()

    End Sub

    Private Sub DgEnti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DgEnti.ItemCommand
        If e.CommandName = "Select" Then
            Dim cls As New clsCooprogettazione(CInt(Request.QueryString("IdAttivita")), CInt(e.Item.Cells(1).Text))
            If cls.SP_Inserimento_Elimina_EnteCoopr(Session("conn"), True) Then
                DgEntiAccorpati.CurrentPageIndex = 0
                Dim DTappo As DataTable = cls.SP_Popola_Griglia_Coprogetti(Session("conn"))
                DataGridSource(DTappo)
                DTappo.Clear()
                DgEnti.Visible = False
            Else
                LblErrore.Text = "Inserimento fallito."
            End If

        End If
    End Sub

    Private Sub DgEntiAccorpati_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DgEntiAccorpati.ItemCommand
        LblErrore.Text = ""
        Dim dtrLocale As SqlClient.SqlDataReader
        Dim STRSQL As String
        ChiudiDataReader(dtrLocale)

        If e.CommandName = "elimina" Then
            Dim cls As New clsCooprogettazione(CInt(Request.QueryString("IdAttivita")), CInt(e.Item.Cells(1).Text))
            If e.Item.Cells(4).Text = 0 Or Len(e.Item.Cells(2).Text) > 7 Then
                'CONTROLLO SE ESISTONO FIGLI COPROGETTANTI
              
                STRSQL = "SELECT * FROM AttivitàEntiCoprogettazione A INNER JOIN ENTI B ON A.IdEnte = B.IDENTE INNER JOIN entirelazioni C ON B.IDEnte = C.IDEnteFiglio  WHERE IdAttività =" & (CInt(Request.QueryString("IdAttivita")) & " AND C.IDEntePadre =" & CInt(e.Item.Cells(1).Text))
                dtrLocale = ClsServer.CreaDatareader(STRSQL, Session("conn"))
                dtrLocale.Read()
                If dtrLocale.HasRows = True Then
                    LblErrore.Text = "Per Rimuovere l'Ente è necessario eliminare prima i propri enti di accoglienza già presenti sul progetto."
                    ChiudiDataReader(dtrLocale)
                    Exit Sub
                End If
                ChiudiDataReader(dtrLocale)
                If cls.SP_Inserimento_Elimina_EnteCoopr(Session("conn"), False) Then
                    DgEntiAccorpati.CurrentPageIndex = 0
                    Dim DTappo As DataTable = cls.SP_Popola_Griglia_Coprogetti(Session("conn"))
                    DataGridSource(DTappo)
                Else
                    LblErrore.Text = "Eliminazione fallita."
                End If
            Else
                LblErrore.Text = "Per Rimuovere l'ente è necessario escludere le sedi già presenti."
            End If
        End If
    End Sub
#End Region

   
End Class