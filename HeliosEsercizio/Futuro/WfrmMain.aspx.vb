Imports System.Net
Imports System.Data.SqlClient
Public Class _WfrmMain
    Inherits SmartPage
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim strSql As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        lblNoSpid.Visible = Request.QueryString("nospid") = "1"

        Dim utente As String
        Dim denominazione As String
        Dim competenza As String
        Dim idente As String
        Dim codiceente As String
        utente = Session("Utente")
        denominazione = Session("Denominazione")
        SetSessionEntePadre() 'se non e' gia' stato fatto, pone in Session("IdEntePadre") il codice idEnte dell'Ente Titolare, se l'Ente corrente e' di Accoglienza; altrimenti 0
        Session("Competenza") = getSezione()
        competenza = Session("Competenza")
        idente = Session("IdEnte")
        codiceente = Session("txtCodEnte")
        'utente = utente.Remove(0, 1)
        If Session("NomeUtente") Is Nothing Then
            lblDenominazione.Text = "Salve, " + " " + denominazione
        Else
            lblDenominazione.Text = "Salve, " + " " + Session("NomeUtente").toUpper()
        End If

        If codiceente <> "&nbsp;" And codiceente <> "" Then
            lblCodice.Text = "il suo codice è: " + codiceente
        Else

            lblCodice.Visible = False
        End If
        lblCompetenza.Text = "la Sezione del suo Ente è: " + competenza
        lblChiso.Text = "Utente: " + utente
        CaricaGriglia(Session("Utente"))
    End Sub
    'valorizza la Session("IdEntePadre") per capire se si tratta di ente titolare o di accoglienza
    Sub SetSessionEntePadre()
        If IsNothing(Session("IdEntePadre")) Then
            Dim strSql As String = "select idEntePadre from entirelazioni where IDEnteFiglio=" & Session("IdEnte")
            Dim myDataReader As SqlDataReader
            myDataReader = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If myDataReader.Read Then
                'trovato l'ente padre: si tratta di ente di accoglienza
                Session("IdEntePadre") = myDataReader("idEntePadre")
            Else
                'non trovato l'ente padre: si tratta di ente titolare
                Session("IdEntePadre") = 0
            End If
            myDataReader.Close()
            myDataReader = Nothing

        End If
    End Sub
    Function getSezione() As String
        strSql = "select Sezione from enti t1 inner join SezioniAlboSCU t2 on t2.IdSezione=t1.IdSezione where t1.IDEnte=" & Session("IdEnte")
        Dim sezione = "non assegnata"
        Dim myDataReader As SqlDataReader
        myDataReader = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If myDataReader.Read Then
            sezione = myDataReader("Sezione")
        End If
        myDataReader.Close()
        myDataReader = Nothing
        Return sezione
    End Function
    Sub CaricaGriglia(ByVal chisei As String)

        If Session("Sistema") = "Futuro" Then
            Select Case Session("TipoUtente")

                Case "U"
                    strSql = "SELECT * FROM ComunicazioniFuturo order by DataComunicazione desc "
                    dtsGenerico = ClsServer.DataSetGenerico(strSql, Session("conn"))

                Case "R"
                    strSql = "SELECT * FROM ComunicazioniFuturo where visibilita <> 'U' order by DataComunicazione desc"
                    dtsGenerico = ClsServer.DataSetGenerico(strSql, Session("conn"))

                Case "E"
                    strSql = "SELECT * FROM ComunicazioniFuturo where visibilita = 'E' order by DataComunicazione desc"
                    dtsGenerico = ClsServer.DataSetGenerico(strSql, Session("conn"))

            End Select

        End If



        If Session("Sistema") = "Helios" Then
            Select Case Session("TipoUtente")

                Case "U"
                    strSql = "SELECT * FROM ComunicazioniHelios order by DataComunicazione desc "
                    dtsGenerico = ClsServer.DataSetGenerico(strSql, Session("conn"))

                Case "R"
                    strSql = "SELECT * FROM ComunicazioniHelios where visibilita <> 'U' order by DataComunicazione desc"
                    dtsGenerico = ClsServer.DataSetGenerico(strSql, Session("conn"))

                Case "E"
                    strSql = "SELECT * FROM ComunicazioniHelios where visibilita = 'E' order by DataComunicazione desc"
                    dtsGenerico = ClsServer.DataSetGenerico(strSql, Session("conn"))

            End Select

        End If



        Session("griglia") = dtsGenerico
        GridViewNew.DataSource = dtsGenerico
        GridViewNew.DataBind()

        dtsGenerico.Dispose()

    End Sub

    Private Sub GridViewNew_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewNew.PageIndexChanging
        Call CaricaGriglia(Session("TipoUtente"))
        GridViewNew.PageIndex = e.NewPageIndex
        GridViewNew.DataSource = Session("griglia")
        GridViewNew.DataBind()
        GridViewNew.SelectedIndex = -1
    End Sub

    Sub CaricaBoxInfoAdeguamentoAntimafia()

        Dim strSql As String = "select abilitato from SediPassword where  Abilitato = 1 and username ='" & Session("Utente") & "'"
        Dim myDataReader As SqlDataReader
        myDataReader = ClsServer.CreaDatareader(strSql, Session("Conn"))
        myDataReader.Read()
        If myDataReader.HasRows = True Then
            Response.Write("Gestore per gli Operatori Volontari della Sede")
            myDataReader.Close()
            myDataReader = Nothing
        Else
            myDataReader.Close()
            myDataReader = Nothing
            Dim _ra As New clsRuoloAntimafia()
            Response.Write(_ra.GetBoxInfoAntimafia(Session("IdEnte"), Session("conn"), True))
        End If

      

    End Sub
End Class