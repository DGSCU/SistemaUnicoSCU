'CLASSE CHE CONTROLLA L'ESISTENZA DI UN INDIRIZZO 
Public Class clsIndirizzi
    'VARIABILI LOCALI UTILIZZATE NELLA CLASSE CHE PRENDONO I PARAMETRI IN INGRESSO
    Private _Indirizzo As String
    Private _IdComune As Integer
    Private _LocalConn As SqlClient.SqlConnection

    'PASSO ALLE VARIABILI LOCALI I VALORI DI QUELLE IN INGRESSo
    Public Sub New(ByVal Indirizzo As String, ByVal IdComune As Integer, ByVal LocalConn As SqlClient.SqlConnection)
        _Indirizzo = Indirizzo
        _IdComune = IdComune
        _LocalConn = LocalConn
    End Sub

    'FUNZIONE CHE CONTROLLA L'ESISTENZA DI UN INDIRIZZO 
    'PARAMETRI=> INDIRIZZO + IDCOMUNE
    Private Function TrovaIndirizzo() As Boolean
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String = "SELECT DISTINCT indirizzo FROM CAP_INDIRIZZI WHERE indirizzo = '" & Replace(_Indirizzo, "'", "''") & "' AND IdComune=" & _IdComune & " order BY indirizzo"
        Dim comando As SqlClient.SqlCommand

        comando = New SqlClient.SqlCommand(strSql, _LocalConn)
        comando.CommandTimeout = 300
        dtrLocal = comando.ExecuteReader()
        TrovaIndirizzo = dtrLocal.HasRows
        dtrLocal.Close()
        Return TrovaIndirizzo
    End Function

    'PROPRIETA' CHE RESTITUISCE L'ESITO DELLA RICERCA DELL'INDIRIZZO
    Public ReadOnly Property CheckIndirizzo() As Boolean
        Get
            Return TrovaIndirizzo()
        End Get
    End Property

End Class