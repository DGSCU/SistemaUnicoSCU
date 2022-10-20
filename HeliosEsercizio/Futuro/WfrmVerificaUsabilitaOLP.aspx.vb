Imports System.Data.SqlClient

Public Class WfrmVerificaUsabilitaOLP
    Inherits System.Web.UI.Page

    Dim dtrgenerico As SqlClient.SqlDataReader
#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()

        If Request.QueryString("IdAttivita") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then
                ChiudiDataReader(dtrgenerico)
            Else
                ChiudiDataReader(dtrgenerico)
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If


        End If
        '***** Aggiunto da Simona Cordella il 01/12/2009
        'Richiamo Store che verifica se l'opl è utilizzato dallo stesso ente su un'altra sede o da un ente diverso
        Dim strRitornoStore As String = String.Empty
        If Request.QueryString("tiporuolo") = "OLP" Then
            strRitornoStore = LeggiStoreVerificaUsabilitaOLP(Request.QueryString("IdAttivita"), Request.QueryString("IdAttivitaSedeAttuazione"), Request.QueryString("CodiceFiscale"))
        End If
        If Request.QueryString("tiporuolo") = "RLEA" Then
            strRitornoStore = LeggiStoreVerificaUsabilitaRLEA(Request.QueryString("IdAttivita"), Request.QueryString("IdAttivitaSedeAttuazione"), Request.QueryString("CodiceFiscale"))
        End If

        If strRitornoStore = "" Then
            TxtNote.Text = "Nessuna anomalia riscontrata."
        Else
            TxtNote.Text = strRitornoStore
        End If
        lblRuolo.Text = Request.QueryString("tiporuolo")
    End Sub
    Private Function LeggiStoreVerificaUsabilitaOLP(ByVal IdAttivita As Integer, ByVal IdAttivitaEnteSedeAttuazione As Integer, ByVal CodiceFiscale As String) As String
        'Agg. da Simona Cordella il 01/12/2009
        'Richiamo Store che verifica se l'opl è utilizzato dallo stesso ente su un'altra sede o da un ente diverso
        Dim intValore As Integer

        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_VERIFICA_USABILITA_OLP"
        CustOrderHist.Connection = Session("conn")

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdAttivita"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)


        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@IdAttivitàSedeAttuazione"
        sparam1.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@CodiceFiscale"
        sparam2.SqlDbType = SqlDbType.VarChar
        CustOrderHist.Parameters.Add(sparam2)


        Dim sparam3 As SqlClient.SqlParameter
        sparam3 = New SqlClient.SqlParameter
        sparam3.ParameterName = "@Esito"
        sparam3.SqlDbType = SqlDbType.Int
        sparam3.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam3)

        Dim sparam4 As SqlClient.SqlParameter
        sparam4 = New SqlClient.SqlParameter
        sparam4.ParameterName = "@Motivazione"
        sparam4.SqlDbType = SqlDbType.VarChar
        sparam4.Size = 1000
        sparam4.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam4)

        CustOrderHist.Parameters("@IdAttivita").Value = IdAttivita
        CustOrderHist.Parameters("@IdAttivitàSedeAttuazione").Value = IdAttivitaEnteSedeAttuazione
        CustOrderHist.Parameters("@CodiceFiscale").Value = CodiceFiscale
        CustOrderHist.ExecuteScalar()
        intValore = CustOrderHist.Parameters("@Esito").Value
        LeggiStoreVerificaUsabilitaOLP = CustOrderHist.Parameters("@Motivazione").Value

    End Function
    Private Function LeggiStoreVerificaUsabilitaRLEA(ByVal IdAttivita As Integer, ByVal IdAttivitaEnteSedeAttuazione As Integer, ByVal CodiceFiscale As String) As String
        'Agg. da Simona Cordella il 01/12/2009
        'Richiamo Store che verifica se l'opl è utilizzato dallo stesso ente su un'altra sede o da un ente diverso
        Dim intValore As Integer

        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_VERIFICA_USABILITA_RLEA"
        CustOrderHist.Connection = Session("conn")

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdAttivita"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)


        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@IdAttivitàSedeAttuazione"
        sparam1.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@CodiceFiscale"
        sparam2.SqlDbType = SqlDbType.VarChar
        CustOrderHist.Parameters.Add(sparam2)


        Dim sparam3 As SqlClient.SqlParameter
        sparam3 = New SqlClient.SqlParameter
        sparam3.ParameterName = "@Esito"
        sparam3.SqlDbType = SqlDbType.Int
        sparam3.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam3)

        Dim sparam4 As SqlClient.SqlParameter
        sparam4 = New SqlClient.SqlParameter
        sparam4.ParameterName = "@Motivazione"
        sparam4.SqlDbType = SqlDbType.VarChar
        sparam4.Size = 1000
        sparam4.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam4)

        CustOrderHist.Parameters("@IdAttivita").Value = IdAttivita
        CustOrderHist.Parameters("@IdAttivitàSedeAttuazione").Value = IdAttivitaEnteSedeAttuazione
        CustOrderHist.Parameters("@CodiceFiscale").Value = CodiceFiscale
        CustOrderHist.ExecuteScalar()
        intValore = CustOrderHist.Parameters("@Esito").Value
        LeggiStoreVerificaUsabilitaRLEA = CustOrderHist.Parameters("@Motivazione").Value


    End Function

End Class