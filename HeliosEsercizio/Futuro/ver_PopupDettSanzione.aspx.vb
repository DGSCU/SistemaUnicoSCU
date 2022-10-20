Public Class ver_PopupDettSanzione
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dtgElenco As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            LblInfo.Visible = True
            dtgElenco.CurrentPageIndex = 0
            Select Case Request.QueryString("VengoDa")
                Case "Progetti"
                    CaricaProgetti()
                    LblInfo.Text = "Elenco Sedi del Progetto."
                Case "EnteCapofila"
                    CaricaEnteCapofila()
                    LblInfo.Text = "Elenco Progetto attivi dell'Ente."
                Case "EnteDipendente"
                    CaricaEnteDipendente()
                    LblInfo.Text = "Elenco Progetti attivi dell'Ente dipendete."
                Case "Sede" 'da fare
                    CaricaSede()
                    LblInfo.Text = "Elenco Progetti attivi della Sede."
                Case "SediAttuazione" 'dafare
                    CaricaSedeAttuazione()
                    LblInfo.Text = "Elenco Progetti attivi della Sede Attuazione."
                Case "SedeProgetti" 'da rivedere
                    'CaricaProgettiSedi()
                    LblInfo.Text = ""
            End Select
        End If
    End Sub

    Sub CaricaProgetti()

        Dim Strsql As String
        Dim DataSetPro As DataSet

        Strsql = "SELECT attività.Titolo + ' (' + attività.CodiceEnte + ')' AS Titolo, "
        Strsql = Strsql & " entisediattuazioni.Denominazione + ' (' + CONVERT(varchar, "
        Strsql = Strsql & " attivitàentisediattuazione.IDEnteSedeAttuazione) + ')' AS SedeAttuazione, "
        Strsql = Strsql & " entisedi.Denominazione AS Sede, ' ' AS indirizzo,"
        Strsql = Strsql & " (SELECT COUNT(DISTINCT ae.IDEntità) "
        Strsql = Strsql & "   FROM attivitàentità AE "
        Strsql = Strsql & "   INNER JOIN ENTITà E ON AE.IDEntità=E.IDEntità"
        Strsql = Strsql & "   WHERE(AE.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione)"
        Strsql = Strsql & "   AND  e.IDStatoEntità = 3) AS NumeroVolontari"
        Strsql = Strsql & " FROM attività "
        Strsql = Strsql & " INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività "
        Strsql = Strsql & " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
        Strsql = Strsql & " INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
        Strsql = Strsql & " WHERE(attività.IDAttività = " & Request.QueryString("IDAttivita") & ")"

        DataSetPro = ClsServer.DataSetGenerico(Strsql, Session("conn"))
        dtgElenco.DataSource = DataSetPro
        dtgElenco.DataBind()

        dtgElenco.Columns(0).Visible = False
        dtgElenco.Columns(1).Visible = False
        dtgElenco.Columns(3).Visible = False
    End Sub

    Sub CaricaEnteCapofila()
        Dim Strsql As String
        Dim DataSetPro As DataSet
        Strsql = " Select  attività.Titolo + ' (' + attività.CodiceEnte + ')' AS Titolo,'' as Sede, '' as SedeAttuazione,'' as Indirizzo,"
        Strsql = Strsql & " (SELECT COUNT(DISTINCT ae.IDEntità) "
        Strsql = Strsql & " FROM attività AS a "
        Strsql = Strsql & " INNER JOIN  attivitàentisediattuazione AS aesa ON a.IDAttività = aesa.IDAttività "
        Strsql = Strsql & " INNER JOIN attivitàentità AS ae ON ae.IDAttivitàEnteSedeAttuazione = aesa.IDAttivitàEnteSedeAttuazione "
        Strsql = Strsql & " INNER JOIN  entità AS e ON  ae.IDEntità = e.IDEntità"
        Strsql = Strsql & " WHERE e.IDStatoEntità = 3 and a.IDAttività = attività.IDAttività ) as NumeroVolontari"
        Strsql = Strsql & " FROM attività INNER JOIN"
        Strsql = Strsql & " enti ON attività.IDEntePresentante = enti.IDEnte"
        Strsql = Strsql & " WHERE(enti.IDEnte = " & Request.QueryString("IDEnte") & ")"
        Strsql = Strsql & " AND (attività.IDStatoAttività = 1) AND GETDATE() > attività.DataFineAttività"
        DataSetPro = ClsServer.DataSetGenerico(Strsql, Session("conn"))
        dtgElenco.DataSource = DataSetPro
        dtgElenco.DataBind()

        dtgElenco.Columns(0).Visible = False
        dtgElenco.Columns(1).Visible = False
        dtgElenco.Columns(2).Visible = False
    End Sub

    Sub CaricaEnteDipendente()
        Dim Strsql As String
        Dim DataSetPro As DataSet


        'Strsql = " SELECT  attività.Titolo + ' (' + attività.CodiceEnte + ')' AS Titolo,'' as Sede, '' as SedeAttuazione,'' as Indirizzo, "
        'Strsql = Strsql & " (SELECT COUNT(DISTINCT ae.IDEntità)   "
        'Strsql = Strsql & "    FROM attivitàentità AE    "
        'Strsql = Strsql & "    INNER JOIN ENTITà E ON AE.IDEntità=E.IDEntità   "
        'Strsql = Strsql & "    INNER JOIN dbo.attivitàentisediattuazione AES "
        'Strsql = Strsql & "    ON AES.IDAttivitàEnteSedeAttuazione= AE.IDAttivitàEnteSedeAttuazione"
        'Strsql = Strsql & "    WHERE(AES.IDAttività = attività.IDAttività)"
        'Strsql = Strsql & "    AND  e.IDStatoEntità = 3) as NumeroVolontari "
        'Strsql = Strsql & " FROM attività "
        'Strsql = Strsql & " WHERE  IDEntePresentante =" & Request.QueryString("IdEnteFiglio") & " "
        'Strsql = Strsql & " AND (IDStatoAttività = 1) AND (GETDATE() > DataFineAttività)"


        Strsql = " SELECT  DISTINCT attività.Titolo + ' (' + attività.CodiceEnte + ')' AS Titolo,'' as Sede, '' as SedeAttuazione,'' as Indirizzo, "
        Strsql = Strsql & " (SELECT COUNT(DISTINCT ae.IDEntità)   "
        Strsql = Strsql & "    FROM attivitàentità AE    "
        Strsql = Strsql & "    INNER JOIN ENTITà E ON AE.IDEntità=E.IDEntità   "
        Strsql = Strsql & "    INNER JOIN dbo.attivitàentisediattuazione AES "
        Strsql = Strsql & "    ON AES.IDAttivitàEnteSedeAttuazione= AE.IDAttivitàEnteSedeAttuazione"
        Strsql = Strsql & "    WHERE(AES.IDAttività = attività.IDAttività)"
        Strsql = Strsql & "    AND  e.IDStatoEntità = 3) as NumeroVolontari "
        Strsql = Strsql & " FROM attività"
        Strsql = Strsql & " INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività  "
        Strsql = Strsql & " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione  "
        Strsql = Strsql & " INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
        'Strsql = Strsql & "--inner join comuni on entisedi.idcomune =comuni.idcomune "
        'Strsql = Strsql & "--inner join provincie on provincie.idprovincia =comuni.idprovincia "
        Strsql = Strsql & " WHERE(attività.IDStatoAttività = 1)"
        Strsql = Strsql & " AND (GETDATE() > attività.DataFineAttività)  "
        Strsql = Strsql & " and entisedi.Idente=" & Request.QueryString("IdEnteFiglio") & ""


        DataSetPro = ClsServer.DataSetGenerico(Strsql, Session("conn"))
        dtgElenco.DataSource = DataSetPro
        dtgElenco.DataBind()

        dtgElenco.Columns(0).Visible = False
        dtgElenco.Columns(1).Visible = False
        dtgElenco.Columns(2).Visible = False

    End Sub

    Sub CaricaSede()
        Dim Strsql As String
        Dim DataSetPro As DataSet

        Strsql = "  SELECT  entisedi.denominazione as Sede ,"
        Strsql = Strsql & " entisedi.indirizzo + ' ' + entisedi.civico + ' ' + entisedi.cap + ' ' + comuni.denominazione + ' ('  +  provincie.descrabb + ')' as indirizzo,"
        Strsql = Strsql & " entisediattuazioni.denominazione as SedeAttuazione,attività.Titolo + ' (' + attività.CodiceEnte + ')' AS Titolo,"
        Strsql = Strsql & " (SELECT COUNT(DISTINCT ae.IDEntità) "
        Strsql = Strsql & "    FROM attivitàentità AE "
        Strsql = Strsql & "    INNER JOIN ENTITà E ON AE.IDEntità=E.IDEntità"
        Strsql = Strsql & "    WHERE(AE.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione)"
        Strsql = Strsql & "    AND  e.IDStatoEntità = 3) AS NumeroVolontari"
        Strsql = Strsql & " FROM attività "
        Strsql = Strsql & " INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività "
        Strsql = Strsql & " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
        Strsql = Strsql & " INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede"
        Strsql = Strsql & " inner join comuni on entisedi.idcomune =comuni.idcomune"
        Strsql = Strsql & " inner join provincie on provincie.idprovincia =comuni.idprovincia"
        Strsql = Strsql & " WHERE (attività.IDStatoAttività = 1)"
        Strsql = Strsql & " AND (GETDATE() > attività.DataFineAttività) "
        Strsql = Strsql & " AND (entisedi.IDEnteSede =" & Request.QueryString("IDEnteSede") & "  )"


        DataSetPro = ClsServer.DataSetGenerico(Strsql, Session("conn"))
        dtgElenco.DataSource = DataSetPro
        dtgElenco.DataBind()

        dtgElenco.Columns(0).Visible = False
        dtgElenco.Columns(1).Visible = False
        dtgElenco.Columns(2).Visible = False

    End Sub

    Sub CaricaSedeAttuazione()
        Dim Strsql As String
        Dim DataSetPro As DataSet

        Strsql = "  SELECT  entisedi.denominazione as Sede ,"
        Strsql = Strsql & " entisedi.indirizzo + ' ' + entisedi.civico + ' ' + entisedi.cap + ' ' + comuni.denominazione + ' ('  +  provincie.descrabb + ')' as indirizzo,"
        Strsql = Strsql & " entisediattuazioni.Denominazione + ' (' + CONVERT(varchar, "
        Strsql = Strsql & " attivitàentisediattuazione.IDEnteSedeAttuazione) + ')' AS SedeAttuazione, attività.Titolo + ' (' + attività.CodiceEnte + ')' AS Titolo,"
        Strsql = Strsql & " (SELECT COUNT(DISTINCT ae.IDEntità) "
        Strsql = Strsql & "    FROM attivitàentità AE "
        Strsql = Strsql & "    INNER JOIN ENTITà E ON AE.IDEntità=E.IDEntità"
        Strsql = Strsql & "    WHERE(AE.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione)"
        Strsql = Strsql & "    AND  e.IDStatoEntità = 3) AS NumeroVolontari"
        Strsql = Strsql & " FROM attività "
        Strsql = Strsql & " INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività "
        Strsql = Strsql & " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
        Strsql = Strsql & " INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede"
        Strsql = Strsql & " inner join comuni on entisedi.idcomune =comuni.idcomune"
        Strsql = Strsql & " inner join provincie on provincie.idprovincia =comuni.idprovincia"
        Strsql = Strsql & " WHERE (attività.IDStatoAttività = 1)"
        Strsql = Strsql & " AND (GETDATE() > attività.DataFineAttività) "
        Strsql = Strsql & " AND (entisediattuazioni.IDEnteSedeAttuazione =" & Request.QueryString("IDEnteSedeAttuazione") & "  )"



        DataSetPro = ClsServer.DataSetGenerico(Strsql, Session("conn"))
        dtgElenco.DataSource = DataSetPro
        dtgElenco.DataBind()

        dtgElenco.Columns(0).Visible = False
        dtgElenco.Columns(1).Visible = False


    End Sub


    Sub CaricaProgettiSedi()
        Dim Strsql As String
        Dim DataSetProSedi As DataSet

        dtgElenco.Items(1).Visible = False
        dtgElenco.Items(2).Visible = False
        dtgElenco.Items(3).Visible = False
        dtgElenco.Items(4).Visible = False

        Strsql = " SELECT attività.Titolo + ' (' + attività.CodiceEnte + ')' AS Titolo,"
        Strsql = Strsql & " (SELECT     COUNT(DISTINCT ae.IDEntità) "
        Strsql = Strsql & "   FROM attività AS a "
        Strsql = Strsql & "   INNER JOIN  attivitàentisediattuazione AS aesa ON a.IDAttività = aesa.IDAttività "
        Strsql = Strsql & "   INNER JOIN attivitàentità AS ae ON ae.IDAttivitàEnteSedeAttuazione = aesa.IDAttivitàEnteSedeAttuazione "
        Strsql = Strsql & "   INNER JOIN  entità AS e ON  ae.IDEntità = e.IDEntità"
        Strsql = Strsql & "   WHERE  e.IDStatoEntità = 3 "
        Strsql = Strsql & "    And ae.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione"
        Strsql = Strsql & "    and a.IDAttività =attività.IDAttività )AS NumeroVolontari"
        Strsql = Strsql & " FROM  attivitàentisediattuazione "
        Strsql = Strsql & " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività"
        Strsql = Strsql & " WHERE attività.IdStatoAttività=1 and getdate()>attività.DataFineAttività"
        Strsql = Strsql & " and attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione= " & Request.QueryString("IDAttivitàSedeAttuazione") & " "

        DataSetProSedi = ClsServer.DataSetGenerico(Strsql, Session("conn"))
        dtgElenco.DataSource = DataSetProSedi
        dtgElenco.DataBind()

    End Sub

    Private Sub dtgElenco_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgElenco.PageIndexChanged
        dtgElenco.SelectedIndex = -1
        dtgElenco.EditItemIndex = -1
        dtgElenco.CurrentPageIndex = e.NewPageIndex
        Select Case Request.QueryString("VengoDa")
            Case "Progetti"
                CaricaProgetti()
                LblInfo.Text = "Elenco Sedi del Progetto."
            Case "EnteCapofila"
                CaricaEnteCapofila()
                LblInfo.Text = "Elenco Progetto attivi per dell'Ente."
            Case "EnteDipendente"
                CaricaEnteDipendente()
                LblInfo.Text = "Elenco Progetti attivi per dell'Ente dipendete."
            Case "Sede" 'da fare
                CaricaSede()
                LblInfo.Text = "Elenco Progetti attivi della Sede."
            Case "SediAttuazione" 'dafare
                CaricaSedeAttuazione()
                LblInfo.Text = "Elenco Progetti attivi della Sede Attuazione."
            Case "SedeProgetti" 'da rivedere
                CaricaProgettiSedi()
                LblInfo.Text = ""
        End Select
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        'Response.Write("window.opener.location.reload();" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>")
    End Sub

    
End Class