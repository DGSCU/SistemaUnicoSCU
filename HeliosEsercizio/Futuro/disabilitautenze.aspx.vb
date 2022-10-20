Public Class disabilitautenze
    Inherits System.Web.UI.Page
    Dim CHKdiProvenienza As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

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
            If Session("TipoUtente") = "U" Then
                txtTipo.Text = ""
                'rdbTutti.Checked = True
                'Antonello 01/09/2008
                rdbU.Enabled = True
                rdbR.Enabled = True
                rdbU.Checked = True
                TxtDominio.Visible = True
                lbldominio.Visible = True
                TxtDominio.Enabled = True
                lbldominio.Enabled = True
                rdbTutti.Enabled = False
                If rdbU.Checked = True Then
                    txtTipo.Text = "U"
                End If

                '--------------
            End If
            If Session("TipoUtente") = "R" Then
                txtTipo.Text = "R"
                rdbR.Checked = True
                rdbR.Enabled = False
                rdbU.Enabled = False
                rdbTutti.Enabled = False
                TxtDominio.Visible = False
                lbldominio.Visible = False
                TxtDominio.Enabled = False
                lbldominio.Enabled = False
            End If
        Else
            If Session("TipoUtente") = "U" Then
                txtTipo.Text = ""
                'rdbTutti.Checked = True
                'Antonello 01/09/2008
                If rdbU.Checked = True Then
                    rdbU.Enabled = True
                    rdbR.Enabled = True
                    rdbU.Checked = True
                    TxtDominio.Visible = True
                    lbldominio.Visible = True
                    TxtDominio.Enabled = True
                    lbldominio.Enabled = True
                    rdbTutti.Enabled = False
                    txtTipo.Text = "U"

                End If
                If rdbR.Checked = True Then
                    rdbU.Enabled = True
                    rdbR.Enabled = True
                    rdbR.Checked = True
                    TxtDominio.Visible = True
                    lbldominio.Visible = True
                    TxtDominio.Enabled = False
                    lbldominio.Enabled = False
                    rdbTutti.Enabled = False
                    txtTipo.Text = "R"
                End If
                '--------------
            End If
            If Session("TipoUtente") = "R" Then
                txtTipo.Text = "R"
                rdbR.Checked = True
                rdbR.Enabled = False
                rdbU.Enabled = False
                rdbTutti.Enabled = False
                TxtDominio.Visible = False
                lbldominio.Visible = False
                TxtDominio.Enabled = False
                lbldominio.Enabled = False
            End If

            If chkInoltro.Value = "true" Then
                InoltroPWD()
            End If
        End If

    End Sub

    Sub InoltroPWD()
        If dtgUtenze.SelectedItem().Cells(8).Text = "U" Then           'utenza u no email 

            Response.Write("<script>")
            Response.Write("alert('Impossibile inoltrare password di tipo utenza U')")
            Response.Write("</script>")
        Else

            'Recupero l'id dell'utenza appena creata
            Dim cmdIdUtenza As New SqlClient.SqlCommand
            Dim intNewIdUtente As Int16

            intNewIdUtente = idutente.Value

            'Invio della mail di creazione dell'utenza
            Dim cmdMailUtenza As New SqlClient.SqlCommand
            cmdMailUtenza.CommandType = CommandType.StoredProcedure
            'cmdMailUtenza.CommandText = "SP_PROCEDURAMAIL_2"
            cmdMailUtenza.CommandText = "SP_PROCEDURAMAIL_ADC"
            cmdMailUtenza.Connection = Session("conn")

            Dim prmIdUtente As SqlClient.SqlParameter
            prmIdUtente = New SqlClient.SqlParameter
            prmIdUtente.ParameterName = "@IDUTENTE"
            prmIdUtente.SqlDbType = SqlDbType.Int
            prmIdUtente.Value = intNewIdUtente
            cmdMailUtenza.Parameters.Add(prmIdUtente)

            Dim prmTipo As SqlClient.SqlParameter
            prmTipo = New SqlClient.SqlParameter
            prmTipo.ParameterName = "@TIPO"
            prmTipo.SqlDbType = SqlDbType.Char
            prmTipo.Value = CStr(tipoutente.Value)
            cmdMailUtenza.Parameters.Add(prmTipo)

            cmdMailUtenza.ExecuteNonQuery()

            Response.Write("<script>")
            Response.Write("alert('Password inoltrata')")
            Response.Write("</script>")

            chkInoltro.Value = ""
            idutente.Value = ""
            tipoutente.Value = ""
            nomeutente.Value = ""
            cognomeutente.Value = ""
        End If
    End Sub

    Private Sub imgRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgRicerca.Click
        'nascondo la label del messaggio
        lblMess.Visible = False
        CaricaGriglia()
    End Sub

    Sub CaricaGriglia()

        'variabile stringa per generare il risultato della query
        Dim strLocal As String

        'dataset che uso per catricare i dati relativi alle sedi di attuazione e al progetto
        Dim dtsLocal As DataSet

        dtgUtenze.CurrentPageIndex = 0

        'preparo la query
        strLocal = "select IdUtente, Cognome + ' ' + Nome as Nominativo, isnull(Mail,'') as Email, UserName, case abilitato when 1 then 'Abilitato' else 'Disabilitato' end as Stato, '<img alt=Inoltra src=images/inoltropwd.jpg onclick=javascript:InoltraAccount(''' + convert(varchar,IdUtente) + ''',''' + LEFT(UserName, 1) + ''','''','''') style=cursor:hand border=0>' as Inoltra, LEFT(UserName, 1) AS Tipo,AccountAD from utentiunsc "
        strLocal = strLocal & "WHERE 1=1 "


        If txtCognome.Text <> "" Then
            strLocal = strLocal & "AND Cognome like '" & Replace(txtCognome.Text, "'", "''") & "%' "
        End If

        If txtEmail.Text <> "" Then
            strLocal = strLocal & "AND Mail like '" & Replace(txtEmail.Text, "'", "''") & "%' "
        End If

        If txtNome.Text <> "" Then
            strLocal = strLocal & "AND Nome like '" & Replace(txtNome.Text, "'", "''") & "%' "
        End If

        If txtutente.Text <> "" Then
            strLocal = strLocal & "AND Username like '" & txtTipo.Text & Replace(txtutente.Text, "'", "''") & "%' "
        End If
        If Session("TipoUtente") = "U" Then
            If TxtDominio.Text <> "" Then
                strLocal = strLocal & "AND AccountAD like '" & Replace(TxtDominio.Text, "'", "''") & "%' "
            End If
        End If
        If rdbR.Checked = True Then
            strLocal = strLocal & "AND left(UserName,1)='R' "
        End If

        If rdbU.Checked = True Then
            strLocal = strLocal & "AND left(UserName,1)='U' "
        End If

        If ddlSelezionaStato.SelectedValue <> 0 Then
            Select Case ddlSelezionaStato.SelectedValue
                Case 1
                    strLocal = strLocal & "AND Abilitato=1 "
                Case 2
                    strLocal = strLocal & "AND Abilitato=0 "
            End Select
        End If

        If Session("TipoUtente") = "R" Then
            strLocal = strLocal & "AND IdRegioneCompetenza=" & TrovaCompetenza() & " "
        End If

        strLocal = strLocal & "ORDER BY Username "

        dtsLocal = ClsServer.DataSetGenerico(strLocal, Session("conn"))

        dtgUtenze.DataSource = dtsLocal
        dtgUtenze.DataBind()

        Session("LocalDataSet") = dtsLocal
    End Sub

    Function TrovaCompetenza() As Integer
        'datareader locale che uso per legger i dati nella base dati
        Dim dtrLocale As SqlClient.SqlDataReader
        Dim strsql As String

        'controllo e chiudo il datareader
        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If

        strsql = "SELECT isnull(IdRegioneCompetenza,0) as IdRegioneCompetenza from UtentiUNSC WHERE username='" & Session("Utente") & "'"

        dtrLocale = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrLocale.HasRows = True Then
            dtrLocale.Read()
            TrovaCompetenza = dtrLocale("IdRegioneCompetenza")
        End If

        'controllo e chiudo il datareader
        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If

        Return TrovaCompetenza

    End Function

    Private Sub dtgUtenze_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgUtenze.ItemCommand
        'datareader locale che uso per legger i dati nella base dati
        Dim dtrLocale As SqlClient.SqlDataReader
        Dim strsql As String
        'command che uso per fare update
        Dim myCommand As SqlClient.SqlCommand

        Select Case e.CommandName
            Case "Abilita"
                myCommand = New SqlClient.SqlCommand

                myCommand.Connection = Session("conn")

                strsql = "update UtentiUNSC set Abilitato=1 WHERE IdUtente='" & e.Item.Cells(1).Text & "'"

                myCommand.CommandText = strsql

                myCommand.ExecuteNonQuery()

                lblMess.Visible = True
                lblMess.Text = "L'utenza di " & e.Item.Cells(2).Text & " e' stata abilitata con successo."

                CaricaGriglia()
            Case "Disabilita"
                myCommand = New SqlClient.SqlCommand

                myCommand.Connection = Session("conn")

                strsql = "update UtentiUNSC set Abilitato=0 WHERE IdUtente='" & e.Item.Cells(1).Text & "'"

                myCommand.CommandText = strsql

                myCommand.ExecuteNonQuery()

                lblMess.Visible = True
                lblMess.Text = "L'utenza di " & e.Item.Cells(2).Text & " e' stata disabilitata con successo."

                CaricaGriglia()
            Case "Modifica"
                'CHKdiProvenienza
                'strsql = "Select Username from UtentiUNSC where idUtente=" & e.Item.Cells(1).Text
                CHKdiProvenienza = Mid(e.Item.Cells(3).Text, 1, 1)
                Response.Redirect("WfrmCreaUtenzeUNSC.aspx?CHKProv=" & CHKdiProvenienza & "&IdUtente=" & e.Item.Cells(1).Text)
        End Select
    End Sub

    Private Sub dtgUtenze_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgUtenze.PageIndexChanged
        'Generato da Bagnani Jonathan il 07/12/06
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dtgUtenze.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dtgUtenze.DataSource = Session("LocalDataSet")
        dtgUtenze.DataBind()
    End Sub

    Private Sub imgChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub rdbU_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbU.CheckedChanged
        txtTipo.Text = "U"
        TxtDominio.Enabled = True
        TxtDominio.Visible = True
        lbldominio.Enabled = True
    End Sub

    Private Sub rdbR_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbR.CheckedChanged
        txtTipo.Text = "R"
        TxtDominio.Text = ""
        TxtDominio.Enabled = False
        TxtDominio.Visible = True
        lbldominio.Enabled = False
    End Sub

    Private Sub rdbTutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbTutti.CheckedChanged
        txtTipo.Text = ""
    End Sub
End Class