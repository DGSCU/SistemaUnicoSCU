Imports System.Drawing

Public Class WebInvioPassword
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrGenerico As SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'Generato da Alessandra Taballione il 19/10/2004
        'Inserimento del codice necessario per inizializzare la pagina
        Dim strPass As New ClsUtility
        Dim i As Integer 'Variabile Contatore
        Dim intLen As Integer 'Variabile intera che identifica la lungezza di una stringa
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        lblMessaggio.Visible = False
        If IsPostBack = False Then
            txtrisultatoricerca.Value = Context.Items("strsql")
            'personalizzazione Maschera Per l'Inoltro Della Password
            lblTitolo.Text = "Inoltro Password"
            lblEnte.Text = Trim(Context.Items("Ente").ToString)
            txtA.Text = Context.Items("Email").ToString
            txtDA.Text = "UNSC"
            'mod. da simona cordella il 11/09/2008
            'genero una nuova pw  anzichè inoltrare quella salvata nel db
            'txtPassword.Text = strPass.ReadPsw(Context.Items("password"))
            'txtPassword.Text = strPass.NuovaPasswordADC
            txtPassword.Attributes.Add("value", strPass.NuovaPasswordADC)

            txtUser.Text = Context.Items("username")
            txtOggetto.Text = "INOLTRO PASSWORD."
            txtmessaggio.Text = txtmessaggio.Text & "Di seguito vengono riportati  i dati da utilizzare per l'Accesso al Sistema: " & vbCrLf
            txtmessaggio.Text = txtmessaggio.Text & "Utente: " & txtUser.Text & "."
            'txtmessaggio.Text = txtmessaggio.Text & "Password: " & txtPassword.Text & "."
            txtmessaggio.Text = txtmessaggio.Text & vbCrLf & "Attenzione: la password è sensibile alle maiuscole e minuscole. "
        End If
    End Sub

    Private Sub cmdInvia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdInvia.Click
        'invio E-mail di avvenuta Registrazione
        'Call invioEmail(txtDA.Text, txtA.Text, txtCC.Text, txtOggetto.Text, txtmessaggio.Text)
        'modifica i dati nel db
        Dim rstGenerico As SqlClient.SqlCommand
        'mod. il 11/09/2008
        'agg. i campi nel db :  DataModificaPassword, DurataPassword, CambioPassword, PasswordDaInviare
        'password='" & Replace(txtPassword.Text, "'", "''")    txtPassword.Text
        strsql = " Update entipassword set " & _
                 " Password='" & Replace(ClsUtility.CreatePSW(ClsUtility.NuovaPasswordADC), "'", "''") & "', " & _
                 " DataModificaPassword =getdate()," & _
                 " CambioPassword=1, PasswordDaInviare=1 " & _
                 " where IdEnte ='" & Session("idente") & "' "

        rstGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))

        'agg. il 11/09/2008 da s.c.
        'Effettuo inserimento in CronologiaPasswordEnti
        strsql = "Insert into CronologiaPasswordEnti (Username, Password, DataCronologia) values " & _
                " ('" & Replace(txtUser.Text, "'", "''") & "','" & Replace(ClsUtility.CreatePSW(ClsUtility.NuovaPasswordADC), "'", "''") & "', getdate())"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))

        'messaggio Utente
        StoreInviaEmail(Session("IdEnte"), 3, "", "")
        lblMessaggio.Visible = True
        lblMessaggio.Text = "Inoltro Password Effettuato!"
        PulisciCampi()

    End Sub

    Private Function StoreInviaEmail(ByVal IdEnte As String, ByVal intTipologiaRichiesta As Integer, ByVal strCC As String, ByVal strNote As String) As Boolean
        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_MAIL_ACCOUNT_ENTI"
        CustOrderHist.Connection = Session("conn")

        '@IdEnte varchar(100),
        '@Tipo INT,
        '@CC NVARCHAR(100),
        '@NoteAggiuntive NVARCHAR(1200),

        Dim paramEnte As SqlClient.SqlParameter
        paramEnte = New SqlClient.SqlParameter
        paramEnte.ParameterName = "@IdEnte"
        paramEnte.SqlDbType = SqlDbType.VarChar
        CustOrderHist.Parameters.Add(paramEnte)

        Dim paramTipo As SqlClient.SqlParameter
        paramTipo = New SqlClient.SqlParameter
        paramTipo.ParameterName = "@Tipo"
        paramTipo.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(paramTipo)

        Dim paramCC As SqlClient.SqlParameter
        paramCC = New SqlClient.SqlParameter
        paramCC.ParameterName = "@CC"
        paramCC.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(paramCC)

        Dim paramNote As SqlClient.SqlParameter
        paramNote = New SqlClient.SqlParameter
        paramNote.ParameterName = "@NoteAggiuntive"
        paramNote.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(paramNote)

        Dim paramEsito As SqlClient.SqlParameter
        paramEsito = New SqlClient.SqlParameter
        paramEsito.ParameterName = "@Esito"
        paramEsito.SqlDbType = SqlDbType.Int
        paramEsito.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(paramEsito)

        Dim Reader As SqlClient.SqlDataReader
        CustOrderHist.Parameters("@IdEnte").Value = IdEnte
        CustOrderHist.Parameters("@Tipo").Value = intTipologiaRichiesta
        CustOrderHist.Parameters("@CC").Value = strCC
        CustOrderHist.Parameters("@NoteAggiuntive").Value = strNote & " "
        Reader = CustOrderHist.ExecuteReader()
        ' Insert code to read through the datareader.
        'If CustOrderHist.Parameters("@Valore").Value = 0 Then
        StoreInviaEmail = CustOrderHist.Parameters("@Esito").Value
        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
        'End If
        'connessione.Close()
    End Function

    Private Sub PulisciCampi()
        'Creato da Alessandra taballione il 19/10/2004
        'Effettuo il blocco degli oggetti della maschera 
        'dopo aver effettuato l'inoltro della password
        txtA.BackColor = Color.Gainsboro
        txtA.ReadOnly = True
        txtDA.BackColor = Color.Gainsboro
        txtDA.ReadOnly = True
        txtCC.BackColor = Color.Gainsboro
        txtCC.ReadOnly = True
        txtmessaggio.ReadOnly = True
        txtmessaggio.BackColor = Color.Gainsboro
        txtNote.ReadOnly = True
        txtNote.BackColor = Color.Gainsboro
        txtOggetto.ReadOnly = True
        txtOggetto.BackColor = Color.Gainsboro
        txtPassword.BackColor = Color.Gainsboro
        txtPassword.ReadOnly = True
        txtUser.ReadOnly = True
        txtUser.BackColor = Color.Gainsboro
        ChKAccodamess.Enabled = False
        cmdInvia.Enabled = False
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        'Generato da Alessandra taballione il 19/10/2004
        'Richiamo la Pagina di Risultato Richieste Account Inviado la Query di Partenza.
        Context.Items.Add("strsql", txtrisultatoricerca.Value)
        Context.Items.Add("VengoDa", "Password")
        Context.Items.Add("inviapassword", "si")
        Server.Transfer("WfrmElencoDomandeAccount.aspx")
    End Sub

    Private Sub ChKAccodamess_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChKAccodamess.CheckedChanged
        If ChKAccodamess.Checked = True And txtNote.Text.Trim <> String.Empty Then
            txtmessaggio.Text = txtmessaggio.Text & vbCrLf & "Note: " & txtNote.Text
        End If
    End Sub

End Class