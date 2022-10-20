Public Class WfrmRicercaUtenze
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
            If Request.QueryString("Utenza") <> "" Then
                txtUtenza.Text = Request.QueryString("Utenza")
                txtTipUtenza.Text = Request.QueryString("TipoUte")
                Ricerca()
            End If
        End If
    End Sub

    Private Sub Ricerca()
        If Trim(txtUtenza.Text) <> "" Then
            Dim strSql As String
            strSql = "SELECT R.Descrizione as Reg,Profili.IdProfilo, Profili.Descrizione, AssociaUtenteGruppo.UserName,UtentiUNSC.HeliosRead "
            strSql = strSql & "FROM Profili "
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
                strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            strSql = strSql & "LEFT JOIN UtentiUNSC On UtentiUNSC.UserName = AssociaUtenteGruppo.UserName "
            strSql = strSql & "LEFT JOIN Regionicompetenze R on r.idregionecompetenza = UtentiUNSC.idregionecompetenza "
            If Not Request.QueryString("IdRegioneCompetenza") Is Nothing Then
                strSql = strSql & "WHERE UtentiUNSC.IdRegioneCompetenza=" & Request.QueryString("IdRegioneCompetenza") & " AND (AssociaUtenteGruppo.UserName like '" & txtTipUtenza.Text & txtUtenza.Text & "%')"
            Else
                strSql = strSql & "WHERE (AssociaUtenteGruppo.UserName like '" & txtTipUtenza.Text & txtUtenza.Text & "%')"
            End If

            Dim myDTR As SqlClient.SqlDataReader
            myDTR = ClsServer.CreaDatareader(strSql, Session("conn"))
            If myDTR.Read = True Then
                txtUtenza.Text = CStr(myDTR.Item("UserName")).Substring(1)
                lblMessage.Text = myDTR.Item("Idprofilo") & " - " & myDTR.Item("Descrizione") & IIf(myDTR.Item("HeliosRead") = True, " - HeliosRead", "") & IIf(IsDBNull(myDTR.Item("Reg")), "", " - " & myDTR.Item("Reg"))
            Else
                lblMessage.Text = "L'utenza ricercata non è presente."
                txtUtenza.Text = ""
            End If
            myDTR.Close()
            myDTR = Nothing
        Else
            lblMessage.Text = "Valorizzare il campo Utenza."
        End If
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        Ricerca()
    End Sub

End Class