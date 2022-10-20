'CLASSE CHE RESTITUISCE LE INFORMAZIONI RELATIVE AL COMUNE
Public Class clsInformazioniComune
    'VARIABILE IN INGRESSO CODICEISTAT
    Private _CodiceIstat As String
    'VARIABILE IN INGRESSO IDCOMUNE
    Private _IdComune As Integer
    'VARBIABILE IN INGRESSo CAP
    Private _CAP As String
    'CONNESSIONE
    Private _LocalConn As SqlClient.SqlConnection

    'PASSO ALLE VARIABILI LOCALI I VALORI DICHIARATI NEL NEW
    Public Sub New(ByVal LocalConn As SqlClient.SqlConnection, Optional ByVal CAP As String = "", Optional ByVal CodiceIstat As String = "", Optional ByVal IdComune As Integer = 0)
        _CodiceIstat = CodiceIstat
        _IdComune = IdComune
        _CAP = CAP
        _LocalConn = LocalConn
    End Sub

    'FUNZIONE CHE RESTITUISCE LA DENOMINAZIONE DEL COMUNE TRAMITE L'ID DEL COMUNE
    Private Function TrovaComune_byIdComune() As String
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String = "SELECT Denominazione FROM Comuni WHERE IdComune = " & _IdComune
        Dim comando As SqlClient.SqlCommand

        comando = New SqlClient.SqlCommand(strSql, _LocalConn)
        comando.CommandTimeout = 300
        dtrLocal = comando.ExecuteReader()
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            TrovaComune_byIdComune = dtrLocal("Denominazione")
        Else
            TrovaComune_byIdComune = ""
        End If
        dtrLocal.Close()
        Return TrovaComune_byIdComune
    End Function

    'FUNZIONE CHE RESTITUISCE LA DENOMINAZIONE DEL COMUNE TRAMITE CODICE ISTAT
    Private Function TrovaComune_byCodiceIstat() As String
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String = "SELECT Denominazione FROM Comuni WHERE CodiceISTAT = '" & _CodiceIstat & "'"
        Dim comando As SqlClient.SqlCommand

        comando = New SqlClient.SqlCommand(strSql, _LocalConn)
        comando.CommandTimeout = 300
        dtrLocal = comando.ExecuteReader()
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            TrovaComune_byCodiceIstat = dtrLocal("Denominazione")
        Else
            TrovaComune_byCodiceIstat = ""
        End If
        dtrLocal.Close()
        Return TrovaComune_byCodiceIstat
    End Function

    'FUNZIONE CHE RESTITUISCE L'ID DEL COMUNE TRAMITE CODICE ISTAT
    Private Function TrovaIdComune_byCodiceIstat() As Integer
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim comando As SqlClient.SqlCommand
        Dim strSql As String

        strSql = "SELECT IdComune FROM Comuni WHERE CodiceISTAT = '" & Replace(_CodiceIstat, "'", "''") & "'"
        comando = New SqlClient.SqlCommand(strSql, _LocalConn)
        comando.CommandTimeout = 300
        dtrLocal = comando.ExecuteReader()
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            TrovaIdComune_byCodiceIstat = dtrLocal("IdComune")
        Else
            TrovaIdComune_byCodiceIstat = 0
        End If
        dtrLocal.Close()
      
        'Dim strSql As String = "SELECT IdComune FROM Comuni WHERE CodiceISTAT = '" & _CodiceIstat & "'"

        Return TrovaIdComune_byCodiceIstat
    End Function

    'FUNZIONE CHE CONTROLLA L'ESISTENZA DEGLI INDIRIZZI PER UN IDCOMUNE
    Private Function ControllaEsistenzaIndirizzi() As Boolean
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String = "SELECT TOP 1 indirizzo FROM CAP_INDIRIZZI WHERE IdComune=" & _IdComune
        Dim comando As SqlClient.SqlCommand

        comando = New SqlClient.SqlCommand(strSql, _LocalConn)
        comando.CommandTimeout = 300
        dtrLocal = comando.ExecuteReader()
        ControllaEsistenzaIndirizzi = dtrLocal.HasRows
        dtrLocal.Close()
        Return ControllaEsistenzaIndirizzi
    End Function

    'PROPRIETA' DELLA CLASSE CHE RESTITUISCE LA DENOMINAZIONE DEL COMUNE PER ID
    Public ReadOnly Property getDenominazioneComune_byIDComune() As String
        Get
            Return TrovaComune_byIdComune()
        End Get
    End Property

    'PROPRIETA' DELLA CLASSE CHE RESTITUISCE LA DENOMINAZIONE DEL COMUNE PER CODICE ISTAT
    Public ReadOnly Property getDenominazioneComune_byCodiceIstat() As String
        Get
            Return TrovaComune_byCodiceIstat()
        End Get
    End Property

    'PROPRIETA' DELLA CLASSE CHE L'ID DEL COMUNE PER CODICE ISTAT
    Public ReadOnly Property getIdComune_byCodiceIstat() As Integer
        Get
            Return TrovaIdComune_byCodiceIstat()
        End Get
    End Property

    'PROPRIETA' DELLA CLASSE CHE RESTITUISCE SE L'INDIRIZZO ESISTE PER UN ID
    Public ReadOnly Property checkEsistenzaIndirizzi() As Boolean
        Get
            Return ControllaEsistenzaIndirizzi()
        End Get
    End Property

End Class
