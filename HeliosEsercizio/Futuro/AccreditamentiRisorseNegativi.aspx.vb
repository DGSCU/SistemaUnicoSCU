Imports System.IO

Public Class AccreditamentiRisorseNegativi
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strSQL As String
        Dim dtrPersonale As System.Data.SqlClient.SqlDataReader
        Dim mydataset As DataSet

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        strSQL = "SELECT Cognome, Nome, CodiceFiscale FROM EntePersonale WHERE IDEntePersonale = " & Request.QueryString("IDEntePersonale")
        dtrPersonale = ClsServer.CreaDatareader(strSQL, Session("conn"))
        With dtrPersonale
            If .HasRows = True Then
                .Read()
                txtCognome.Text = .Item("Cognome")
                txtNome.Text = .Item("Nome")
                txtCodiceFiscale.Text = .Item("CodiceFiscale")
            End If
            .Close()
        End With

        'Ricerca dati anagrafici Risorsa

        'strSQL = "SELECT b.IDRuolo, c.IDEnte, a.IDEntePersonaleRuolo, dbo.formatodata (b.DataAccreditamento) AS DataAccreditamento, e.Denominazione, r.Ruolo, dbo.formatodata (DataCronologia) as DataDelibera " & _
        '         "FROM CronologiaEntePersonaleRuoli a " & _
        '         "	INNER JOIN EntePersonaleRuoli b ON a.IDEntePersonaleRuolo = b.IDEntePersonaleRuolo " & _
        '         "	INNER JOIN EntePersonale c ON b.IDEntePersonale = c.IDEntePersonale " & _
        '         "	INNER JOIN Enti e ON c.IDEnte = e.IDEnte " & _
        '         "	INNER JOIN Ruoli r ON b.IDRuolo = r.IDRuolo " & _
        '            "WHERE a.Accreditato = -1 AND c.CodiceFiscale = '" & txtCodiceFiscale.Text & "' " & _
        '         "ORDER BY DataAccreditamento"
        strSQL = "SELECT b.IDRuolo, c.IDEnte,f.descrizione as Competenza,b.Usernameaccreditatore, b.IDEntePersonaleRuolo, dbo.formatodata (b.DataAccreditamento) AS DataAccreditamento, e.codiceregione, e.Denominazione, r.Ruolo, dbo.formatodata (b.DataAccreditamento) as DataValutazione " & _
                 "FROM  EntePersonaleRuoli b  " & _
                 "	INNER JOIN EntePersonale c ON b.IDEntePersonale = c.IDEntePersonale " & _
                 "	INNER JOIN Enti e ON c.IDEnte = e.IDEnte " & _
                 "	INNER JOIN regionicompetenze f ON e.IDregionecompetenza = f.idregionecompetenza " & _
                 "	INNER JOIN Ruoli r ON b.IDRuolo = r.IDRuolo " & _
                    "WHERE b.Accreditato = -1 AND c.CodiceFiscale = '" & txtCodiceFiscale.Text & "' " & _
                 "ORDER BY DataAccreditamento desc"

        mydataset = ClsServer.DataSetGenerico(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtgRicerca.DataSource = mydataset
        dtgRicerca.DataBind()
    End Sub

End Class