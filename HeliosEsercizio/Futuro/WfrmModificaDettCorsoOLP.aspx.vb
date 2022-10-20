Imports System.Data.SqlClient
Imports System.IO
Public Class WfrmModificaDettCorsoOLP
    Inherits System.Web.UI.Page
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String
    Dim myCommand As System.Data.SqlClient.SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If


        If Page.IsPostBack = False Then
            CaricaDati()

        End If

    End Sub
    Private Sub CaricaDati()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "select *  from CorsiFormazioneOLPDettaglio  Where IdCorsoFormazioneOLPDettaglio=" & Request.QueryString("idDettaglio")
        ' dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))

        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))


        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            TextCognome.Text = dtrgenerico.Item("Cognome").ToString
            TextNome.Text = dtrgenerico.Item("Nome").ToString
            TextEnteRiferimento.Text = dtrgenerico.Item("EnteRiferimento").ToString
            TextLuogoSvolgimento.Text = dtrgenerico.Item("LuogoSvolgimento").ToString
            TextDataSvolgimento.Text = dtrgenerico.Item("DataSvolgimentoCorso").ToString
            TextNumeroOre.Text = dtrgenerico.Item("NumeroOre").ToString
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub
    Protected Sub CmdSalva_Click(sender As Object, e As EventArgs) Handles CmdSalva.Click
        Dim messaggio As String
        Dim ErroreCampo As Boolean
        lblmessaggio.Text = ""
        If TextCognome.Text = "" Then
            ErroreCampo = True
            lblmessaggio.Text = "Il Campo Cognome e' Obbligatorio."
            messaggio = lblmessaggio.Text & " "
            'Exit Sub
        End If
        If TextNome.Text = "" Then
            lblmessaggio.Text = "Il Campo Nome e' Obbligatorio."
            ErroreCampo = True
            messaggio = messaggio & " " & lblmessaggio.Text & " "
            'Exit Sub
        End If
        If TextEnteRiferimento.Text = "" Then
            lblmessaggio.Text = "Il Campo Ente Riferimento e' Obbligatorio."
            messaggio = messaggio & " " & lblmessaggio.Text & " "
            ErroreCampo = True
            'Exit Sub
        End If
        If TextLuogoSvolgimento.Text = "" Then
            lblmessaggio.Text = "Il Campo Luogo Svolgimento e' Obbligatorio."
            ErroreCampo = True
            messaggio = messaggio & " " & lblmessaggio.Text & " "
            'Exit Sub
        End If
        If TextDataSvolgimento.Text = "" Then

            lblmessaggio.Text = "Il Campo Data Svolgimento e' Obbligatorio."
            ErroreCampo = True
            messaggio = messaggio & " " & lblmessaggio.Text & " "
            'Exit Sub
        End If
        If TextNumeroOre.Text = "" Then

            lblmessaggio.Text = "Il Campo Numero Ore e' Obbligatorio."
            ErroreCampo = True
            messaggio = messaggio & " " & lblmessaggio.Text & " "
            'Exit Sub
        End If

        If ErroreCampo = True Then

            lblmessaggio.Text = messaggio

        Else
            strsql = "update CorsiFormazioneOLPDettaglio set Cognome='" & TextCognome.Text.Replace("'", "''") & "', Nome='" & TextNome.Text.Replace("'", "''") & "',  EnteRiferimento='" & TextEnteRiferimento.Text.Replace("'", "''") & "', LuogoSvolgimento='" & TextLuogoSvolgimento.Text.Replace("'", "''") & "',DataSvolgimentoCorso='" & TextDataSvolgimento.Text & "',NumeroOre='" & TextNumeroOre.Text & "' where IdCorsoFormazioneOLPDettaglio=" & Request.QueryString("idDettaglio")

            'sql command che mi esegue la insert
            myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
            myCommand.ExecuteNonQuery()
            myCommand.Dispose()
            messaggio = "AGGIORNAMENTE AVVENUTO CON SUCCESSO"
            lblmessaggio.Text = messaggio
            CaricaDati()

        End If

      
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmVisualizzaDettaglioCorsoOLP.aspx?idCorso=" & Request.QueryString("IdCorso") & "&VengoDa=" & 2)
    End Sub
End Class