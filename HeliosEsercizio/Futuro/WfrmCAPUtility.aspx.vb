Public Class WfrmCAPUtility
    Inherits System.Web.UI.Page


    Dim dtsgenerico As New DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String
    Dim RECORD_DA_CARICARE As Integer = 100

    Protected WithEvents CmdRicercaCap As System.Web.UI.WebControls.Button
    Protected WithEvents txtCap As System.Web.UI.WebControls.TextBox
    Protected WithEvents CmdRicerca As System.Web.UI.WebControls.Button

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        dtgTrovaCap.Visible = False
    End Sub





    Protected Sub CmdRicercaCap_Click(sender As Object, e As EventArgs) Handles CmdRicercaCap.Click
        lblmess.Text = ""

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        If chkCompleto.Checked = True Then
            strsql = "SELECT top 1 indirizzo FROM CAP_ANAGRAFICACOMPLETA WITH(NOLOCK) WHERE Comune ='" & Replace(txtComune.Text, "'", "''") & "' "
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
            dtrgenerico.Read()
            If dtrgenerico.HasRows = True Then
                'If dtrgenerico("indirizzo") = "--" Then
                '    If txtIndirizzo.Text <> "" Then
                '        lblmess.Text = "Indirizzo Non Necessario"
                '        dtgTrovaCap.Visible = False
                '    Else
                '        If Not dtrgenerico Is Nothing Then
                '            dtrgenerico.Close()
                '            dtrgenerico = Nothing
                '        End If
                '        lblmess.Text = ""

                '        strsql = "SELECT top "
                '        strsql = strsql + RECORD_DA_CARICARE.ToString
                '        strsql = strsql + "Provincia,comune,indirizzo,civici,cap FROM CAP_ANAGRAFICACOMPLETA WITH(NOLOCK) WHERE Comune ='" & Replace(txtComune.Text, "'", "''") & "' ORDER BY Comune,Indirizzo,Civici"
                '        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
                '        dtgTrovaCap.DataSource = dtrgenerico
                '        dtgTrovaCap.DataBind()
                '        dtgTrovaCap.Visible = True
                '        If Not dtrgenerico Is Nothing Then
                '            dtrgenerico.Close()
                '            dtrgenerico = Nothing
                '        End If
                '    End If
                'Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                lblmess.Text = ""
                strsql = "SELECT top "
                strsql = strsql + RECORD_DA_CARICARE.ToString
                strsql = strsql + " Provincia,comune,indirizzo,civici,cap FROM CAP_ANAGRAFICACOMPLETA WHERE Comune ='" & Replace(txtComune.Text, "'", "''") & "' and indirizzo LIKE '%" & Replace(txtIndirizzo.Text, "'", "''") & "%' ORDER BY Comune,Indirizzo,Civici "
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
                dtgTrovaCap.DataSource = dtrgenerico
                dtgTrovaCap.DataBind()
                dtgTrovaCap.Visible = True
                If dtgTrovaCap.Items.Count = RECORD_DA_CARICARE Then
                    ImpostaMessaggioNumeroRecordVisualizzati(lblmess, RECORD_DA_CARICARE)

                ElseIf dtgTrovaCap.Items.Count = 0 Then
                    ImpostaMessaggioNessunRisultato(lblmess)
                    dtgTrovaCap.Visible = False
                End If
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
            'End If
        Else
            lblmess.Text = "Comune non trovato."
            lblmess.Visible = True
            dtgTrovaCap.Visible = False
        End If
        Else
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            strsql = "SELECT top "
            strsql = strsql + RECORD_DA_CARICARE.ToString
            strsql = strsql + " Provincia,comune,indirizzo,civici,cap FROM CAP_ANAGRAFICACOMPLETA WHERE indirizzo LIKE '%" & Replace(txtIndirizzo.Text, "'", "''") & "%' AND Comune LIKE '%" & Replace(txtComune.Text, "'", "''") & "%' ORDER BY Comune,Indirizzo,Civici"
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
            If dtrgenerico.HasRows = True Then
                dtgTrovaCap.DataSource = dtrgenerico
                dtgTrovaCap.DataBind()
                dtgTrovaCap.Visible = True
                txtIndirizzo.Visible = True
                If dtgTrovaCap.Items.Count = RECORD_DA_CARICARE Then
                    ImpostaMessaggioNumeroRecordVisualizzati(lblmess, RECORD_DA_CARICARE)
                ElseIf dtgTrovaCap.Items.Count = 0 Then
                    ImpostaMessaggioNessunRisultato(lblmess)
                    dtgTrovaCap.Visible = False
                End If
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
            Else
                ImpostaMessaggioNessunRisultato(lblmess)
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


    End Sub


    Protected Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        lblmess.Text = ""
        txtCap.Text = txtCap.Text.Trim
        If txtCap.Text <> "" And Len(txtCap.Text) = "5" Then
            strsql = "SELECT  Provincia,comune,indirizzo,civici,cap FROM CAP_ANAGRAFICACOMPLETA WHERE Cap ='" & Replace(txtCap.Text, "'", "''") & "' ORDER BY Comune,Indirizzo,Civici"
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))

            If dtrgenerico.HasRows = True Then
                dtgTrovaCap.DataSource = dtrgenerico
                dtgTrovaCap.DataBind()
                dtgTrovaCap.Visible = True
            Else
                ImpostaMessaggioNessunRisultato(lblmess)
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        Else
            ImpostaMessaggioNessunRisultato(lblmess)
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub


    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Dim IdEnte As String
        IdEnte = Session("IdEnte").ToString
        Dim ENTE_NON_COLLEGATO As String = "-1"

        If IdEnte = ENTE_NON_COLLEGATO Then
            Response.Redirect("WfrmRicercaEnti.aspx")
        Else
            Response.Redirect("WfrmMain.aspx")
        End If



    End Sub

    Private Sub ImpostaMessaggioNumeroRecordVisualizzati(label As Label, record As Integer)
        label.Visible = True
        label.Text = String.Format("Sono stati visualizzati solo i primi {0} elementi.", record)
    End Sub

    Private Sub ImpostaMessaggioNessunRisultato(label As Label)
        label.Visible = True
        label.Text = String.Format("La ricerca non ha prodotto risultati.")
    End Sub
End Class