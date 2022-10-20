Public Class WfrmInsNuovoIndirizzo
    Inherits System.Web.UI.Page
    Dim sEsito As String
    Dim dtsgenerico As New DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
   
    Public Sub puliscicampi()
        txtCap.Text = ""
        txtComune.Text = ""
        txtIndirizzo.Text = ""
    End Sub
   
    Private Sub dtgTrovaCap_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgTrovaCap.ItemCommand
        lblmess.Visible = False
        lblmess.Text = ""
        If e.CommandName = "Rimuovi" Then
            Dim MyCommand As System.Data.SqlClient.SqlCommand
            Dim Risultato As String

            'Eseguo la Store Procedure SP_CAP_AGGIUNGI_NUOVO_INDIRIZZO
            MyCommand = New System.Data.SqlClient.SqlCommand
            MyCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_CAP_ELIMINA_INDIRIZZO"

            Try

                'Variabile - @COMUNE - In
                Dim sparam1 As SqlClient.SqlParameter
                sparam1 = New SqlClient.SqlParameter
                sparam1.ParameterName = "@COMUNE"
                sparam1.SqlDbType = SqlDbType.NVarChar
                sparam1.Size = 200
                MyCommand.Parameters.Add(sparam1)

                'Variabile - @INDIRIZZO - In
                Dim sparam2 As SqlClient.SqlParameter
                sparam2 = New SqlClient.SqlParameter
                sparam2.ParameterName = "@INDIRIZZO"
                sparam2.SqlDbType = SqlDbType.NVarChar
                sparam2.Size = 200
                MyCommand.Parameters.Add(sparam2)

                'Variabile - @CAP - In
                Dim sparam3 As SqlClient.SqlParameter
                sparam3 = New SqlClient.SqlParameter
                sparam3.ParameterName = "@CAP"
                sparam3.SqlDbType = SqlDbType.NVarChar
                sparam3.Size = 5
                MyCommand.Parameters.Add(sparam3)

                'Variabile - @ESITO - Out
                Dim sparam4 As SqlClient.SqlParameter
                sparam4 = New SqlClient.SqlParameter
                sparam4.ParameterName = "@ESITO"
                sparam4.SqlDbType = SqlDbType.NVarChar
                sparam4.Size = 100
                sparam4.Direction = ParameterDirection.Output
                MyCommand.Parameters.Add(sparam4)

                MyCommand.Parameters("@COMUNE").Value = e.Item.Cells(1).Text
                MyCommand.Parameters("@INDIRIZZO").Value = e.Item.Cells(2).Text
                MyCommand.Parameters("@CAP").Value = e.Item.Cells(4).Text

                MyCommand.ExecuteNonQuery()

                Risultato = MyCommand.Parameters("@ESITO").Value
                lblmess.Visible = True
                lblmess.Text = Risultato


            Catch ex As Exception
                lblmess.Text = "SI E' VERIFICATO UN ERRORE CONTATTARE ANTONELLO"
            End Try

        End If

        Call Ricerca(e.Item.Cells(1).Text)
        puliscicampi()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub
    Private Sub Ricerca(Optional ByVal comune As String = "")
        If comune = "" Then

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            strsql = "SELECT Provincia,comune,indirizzo,civici,cap FROM CAP_ANAGRAFICACOMPLETA_AGGIUNTA WHERE indirizzo LIKE '%" & Replace(txtIndirizzo.Text, "'", "''") & "%' AND Cap LIKE '%" & Replace(txtCap.Text, "'", "''") & "%' AND Comune LIKE '%" & Replace(txtComune.Text, "'", "''") & "%'"
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
            'dtrgenerico.Read()
            dtgTrovaCap.DataSource = dtrgenerico
            dtgTrovaCap.DataBind()
            'Session("UltimaRicerca") = dtrgenerico

            dtgTrovaCap.Visible = True

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

        Else

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            strsql = "SELECT Provincia,comune,indirizzo,civici,cap FROM CAP_ANAGRAFICACOMPLETA_AGGIUNTA WHERE indirizzo LIKE '%" & Replace(txtIndirizzo.Text, "'", "''") & "%' AND Cap LIKE '%" & Replace(txtCap.Text, "'", "''") & "%' AND Comune LIKE '%" & Replace(comune, "'", "''") & "%'"
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
            'dtrgenerico.Read()
            dtgTrovaCap.DataSource = dtrgenerico
            dtgTrovaCap.DataBind()
            'Session("UltimaRicerca") = dtrgenerico

            dtgTrovaCap.Visible = True

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If


        End If

    End Sub
    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        Call Ricerca()
    End Sub

    Protected Sub ImgInserisci_Click(sender As Object, e As EventArgs) Handles ImgInserisci.Click
        lblmess.Visible = False
        lblmess.Text = ""
        Dim MyCommand As System.Data.SqlClient.SqlCommand
        Dim Risultato As String


        If txtCap.Text = "" Or txtIndirizzo.Text = "" Or txtComune.Text = "" Then
            lblmess.Visible = True
            lblmess.Text = "Compilare tutti i Campi"
            Exit Sub
        End If


        'Eseguo la Store Procedure SP_CAP_AGGIUNGI_NUOVO_INDIRIZZO
        MyCommand = New System.Data.SqlClient.SqlCommand
        MyCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
        MyCommand.CommandType = CommandType.StoredProcedure
        MyCommand.CommandText = "SP_CAP_AGGIUNGI_NUOVO_INDIRIZZO"


        Try

            'Variabile - @COMUNE - In
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@COMUNE"
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Size = 200
            MyCommand.Parameters.Add(sparam1)

            'Variabile - @INDIRIZZO - In
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@INDIRIZZO"
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Size = 200
            MyCommand.Parameters.Add(sparam2)

            'Variabile - @CAP - In
            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@CAP"
            sparam3.SqlDbType = SqlDbType.NVarChar
            sparam3.Size = 5
            MyCommand.Parameters.Add(sparam3)

            'Variabile - @ESITO - Out
            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@ESITO"
            sparam4.SqlDbType = SqlDbType.NVarChar
            sparam4.Size = 100
            sparam4.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam4)

            MyCommand.Parameters("@COMUNE").Value = txtComune.Text
            MyCommand.Parameters("@INDIRIZZO").Value = txtIndirizzo.Text
            MyCommand.Parameters("@CAP").Value = txtCap.Text

            MyCommand.ExecuteNonQuery()

            Risultato = MyCommand.Parameters("@ESITO").Value
            lblmess.Visible = True
            lblmess.Text = Risultato
            Call Ricerca(txtComune.Text)
            puliscicampi()

        Catch ex As Exception
            lblmess.Text = "SI E' VERIFICATO UN ERRORE CONTATTARE ANTONELLO"
        End Try

    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class