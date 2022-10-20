Imports System.Data.SqlClient

Public Class WfrmVerificaCompletamentoAccreditamento
    Inherits System.Web.UI.Page
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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()

        '***** Aggiunto da Simona Cordella il 16/06/2009
        'Richiamo Store che verifica se l'ente ha completato tutte le operazioni necessarie per procedere all'acreditamento/adeguamento
        Dim strRitornoStore As String
        strRitornoStore = LeggiStoreVerificaCompletamentoAccreditamento(Session("IdEnte"))
        If strRitornoStore = "" Then
            TxtNote.Text = "Nessuna anomalia riscontrata."
        Else
            TxtNote.Text = strRitornoStore
        End If
    End Sub
    Private Function LeggiStoreVerificaCompletamentoAccreditamento(ByVal IDEnte As Integer) As String
        'Agg. da Simona Cordella il 16/06/2009
        'richiamo store che verifca se l'ente ha completato tutti gli inserimeni e gli aggiornamenti necessari per effettuare la presentazione della domanda di accrditamento /adeguamento
        Dim intValore As Integer

        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_VERIFICA_COMPLETAMENTO_ACCREDITAMENTO"
        CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdEnte"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)


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
        CustOrderHist.Parameters("@IdEnte").Value = IDEnte
        CustOrderHist.ExecuteScalar()

        intValore = CustOrderHist.Parameters("@Esito").Value
        LeggiStoreVerificaCompletamentoAccreditamento = CustOrderHist.Parameters("@Motivazione").Value


    End Function
End Class