Public Class WfrmCOMPGestioneCausaliPaghe
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtsGenerico As DataSet
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim myCommand As System.Data.SqlClient.SqlCommand
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

            CaricaGriglia()

       
        End If


    End Sub
    Private Sub CaricaGriglia()
        strsql = "Select IdTipoElemento,codice,descrizione, case Abilitato when 1 then 'Abilitato' else 'Disabilitato' end as Stato from COMP_TipiElemento"
        dgAbilitazioni.DataSource = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgAbilitazioni.DataBind()
        dgAbilitazioni.Visible = True
    End Sub

    Private Sub dgAbilitazioni_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAbilitazioni.ItemCommand
        'datareader locale che uso per legger i dati nella base dati

        Dim strsql As String
        'command che uso per fare update
        Dim myCommand As SqlClient.SqlCommand

        Select Case e.CommandName
            Case "Abilita"
                myCommand = New SqlClient.SqlCommand

                myCommand.Connection = Session("conn")

                strsql = "update COMP_TipiElemento set Abilitato=1 WHERE IdTipoElemento='" & e.Item.Cells(0).Text & "' and calcolato=0 "

                myCommand.CommandText = strsql

                myCommand.ExecuteNonQuery()

                lblmess.Visible = True
                lblmess.Text = "Elemento abilitato con successo."

                CaricaGriglia()
            Case "Disabilita"
                myCommand = New SqlClient.SqlCommand

                myCommand.Connection = Session("conn")

                strsql = "update COMP_TipiElemento set Abilitato=0 WHERE IdTipoElemento='" & e.Item.Cells(0).Text & "' and calcolato=0"

                myCommand.CommandText = strsql

                myCommand.ExecuteNonQuery()

                lblmess.Visible = True
                lblmess.Text = "Elemento disabilitato con successo."

                CaricaGriglia()
           
        End Select
    End Sub

    Protected Sub CmdInserisci_Click(sender As Object, e As EventArgs) Handles CmdInserisci.Click
        If VerificaInserimento() = True Then

            strsql = "Insert into COMP_TipiElemento (IdTipoElemento,Codice,Descrizione,Calcolato,Segno,Abilitato) values ('" & txtCodice.Text & "','" & txtCodice.Text & "','" & TxtDescrizione.Text & "',0," & ddlTipo.SelectedValue & ", 1 )"
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            lblmess.Text = "Inserimento effettuato."
            CaricaGriglia()
       
        End If
    End Sub
    Private Function VerificaInserimento() As Boolean

        Dim dtrLocale As SqlClient.SqlDataReader
        Dim strsql As String
        Dim codice As String
        Dim Descrizione As String
        'controllo e chiudo il datareader
        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If
        If txtCodice.Text = "" Then
            lblmess.Text = "il campo codice e' obbligatorio"
            Exit Function
        End If
        If IsNumeric(txtCodice.Text) = False Then
            lblmess.Text = "il campo codice deve essere numerico"
            Exit Function
        End If
        If TxtDescrizione.Text = "" Then
            lblmess.Text = "il campo Descrizione e' obbligatorio"
            Exit Function
        End If
        If Len(txtCodice.Text) < 2 Then
            lblmess.Text = "il campo codice deve contenere 2 caratteri"
            Exit Function
        End If

        strsql = "Select * from COMP_TipiElemento WHERE CODICE = '" & txtCodice.Text.Replace("'", "''") & "' OR DESCRIZIONE = '" & TxtDescrizione.Text.Replace("'", "''") & "'"


        dtrLocale = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrLocale.HasRows = True Then
            dtrLocale.Read()
            codice = dtrLocale("codice")
            Descrizione = dtrLocale("Descrizione")
            If Not dtrLocale Is Nothing Then
                dtrLocale.Close()
                dtrLocale = Nothing
            End If
            If txtCodice.Text = codice Then
                lblmess.Text = "il valore inserito risulta gia' presente"
                Exit Function
            End If
            If TxtDescrizione.Text = Descrizione Then
                lblmess.Text = "il valore inserito risulta gia' presente"
                Exit Function
            End If
        End If


        'controllo e chiudo il datareader
        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If
        Return True
    End Function

    Protected Sub Chiudi_Click(sender As Object, e As EventArgs) Handles Chiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class