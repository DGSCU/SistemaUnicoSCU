Public Class WfrmProgrammiVerificaIstanzaAnomaliaProgramma
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'testupdate
        If Not IsPostBack Then
            'Richiamo Store che verifica se ci sono anomalie sul singolo progetto
            Dim strRitornoStore As String
            strRitornoStore = LeggiStoreVerificaIstanzaProgramma(Request.QueryString("IdProgramma"), Request.QueryString("IdBando"))
            If strRitornoStore = "" Then
                TxtNote.Text = "Nessuna anomalia riscontrata."
            Else
                TxtNote.Text = strRitornoStore
            End If
        End If

    End Sub
    Private Function LeggiStoreVerificaIstanzaProgramma(ByVal IdProgramma As Integer, ByVal IdBando As Integer) As String

        'richiamo store che verifca  se ci sono anomalie sul progetto durante l'istnza di presentazione
        Dim intValore As Integer

        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_ISTANZA_DETTAGLIO_ANOMALIA_PROGRAMMA"
        CustOrderHist.Connection = Session("conn")
        'IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdProgramma"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)

        Dim sparamb As SqlClient.SqlParameter
        sparamb = New SqlClient.SqlParameter
        sparamb.ParameterName = "@IdBando"
        sparamb.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparamb)

        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@Esito"
        sparam1.SqlDbType = SqlDbType.Int
        sparam1.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@Motivazione"
        sparam2.SqlDbType = SqlDbType.VarChar
        sparam2.Size = -1
        sparam2.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam2)

        Dim Reader As SqlClient.SqlDataReader
        CustOrderHist.Parameters("@IdProgramma").Value = IdProgramma
        CustOrderHist.Parameters("@IdBando").Value = IdBando
        'Reader = CustOrderHist.ExecuteReader()
        CustOrderHist.ExecuteScalar()
        ' Insert code to read through the datareader.
        intValore = CustOrderHist.Parameters("@Esito").Value
        LeggiStoreVerificaIstanzaProgramma = CustOrderHist.Parameters("@Motivazione").Value

        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
    End Function

    Protected Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        Response.Write("<script>window.close();</" + "script>")
        Response.End()
    End Sub
End Class