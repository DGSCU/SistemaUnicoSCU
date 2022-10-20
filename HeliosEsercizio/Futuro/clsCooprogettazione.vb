Imports System.Data.SqlClient
Public Class clsCooprogettazione

#Region " COSTRUTTORI"
    Sub New(ByVal IDA As Integer)

        Me._idAttività = IDA

    End Sub

    Sub New(ByVal IDA As Integer, ByVal IDE As Integer)

        Me._idAttività = IDA
        Me._idEnte = IDE

    End Sub
#End Region

#Region " PROPRIETA'"
    Private _idAttività As Integer
    Public ReadOnly Property idAttività() As Integer
        Get
            Return Me._idAttività
        End Get
    End Property
    Private _idEnte As Integer
    Public ReadOnly Property idEnte() As Integer
        Get
            Return Me._idEnte
        End Get
    End Property

#End Region

#Region " METODI"
    Public Function SP_Popola_Griglia_Coprogetti(ByVal Connessione As SqlConnection) As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        SP_Popola_Griglia_Coprogetti = New DataTable

        SqlCmd.CommandText = "SP_COOPROG_SEL_RICERCA_ENTICOOPROGETTATI"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Connessione
        SqlCmd.Parameters.Add("@IDATTIVITA", Me._idAttività)
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(SP_Popola_Griglia_Coprogetti)
        Return SP_Popola_Griglia_Coprogetti

    End Function

    Public Function SP_Ricerca_Enti(ByVal Connessione As SqlConnection, ByVal Condizioni As String) As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        SP_Ricerca_Enti = New DataTable

        SqlCmd.CommandText = "SP_COOPROG_SEL_RICERCA_ENTI"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Connessione
        SqlCmd.Parameters.Add("@IDATTIVITA", Me._idAttività)
        SqlCmd.Parameters.Add("@CONDIZIONE", clsConversione.DaVuotoInNull(Condizioni))
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(SP_Ricerca_Enti)
        Return SP_Ricerca_Enti

    End Function
    Public Function SP_Inserimento_Elimina_EnteCoopr(ByVal ConnX As SqlConnection, _
                                                     ByVal IsInsert As Boolean) As Boolean
        Dim SqlCmd As New SqlCommand
        Dim mytrans As SqlTransaction

        Try
            SqlCmd.CommandText = "SP_COOPROG_INSERT_DELETE_ENTE_COOPROGETTO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            mytrans = ConnX.BeginTransaction
            SqlCmd.Connection = ConnX
            SqlCmd.Transaction = mytrans
            SqlCmd.Parameters.Add("@IDATTIVITA", Me._idAttività)
            SqlCmd.Parameters.Add("@IDENTE", Me._idEnte)
            SqlCmd.Parameters.Add("@Fase", IsInsert)
            SqlCmd.ExecuteNonQuery()
            mytrans.Commit()
            Return True

        Catch ex As Exception
            mytrans.Rollback()
            Return False
        End Try
    End Function
#End Region

End Class
