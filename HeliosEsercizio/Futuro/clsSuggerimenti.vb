Public Class clsSuggerimenti
    'SETTO LE VARIABILI IN INGRESSO
    Private _IdComune As Integer
    Private _Indirizzo As String
    Private _LocalConn As SqlClient.SqlConnection

    'PASSAGGIO VALORI PRaMETRI IN INGRESSO
    Public Sub New(ByVal Indirizzo As String, ByVal IdComune As Integer, ByVal LocalConn As SqlClient.SqlConnection)
        _Indirizzo = Replace(Indirizzo, "|", "''")
        _IdComune = IdComune
        _LocalConn = LocalConn
    End Sub

    'FUNZIONE CHE ESEGUE IL CONTROLLO DELL'ESISTENZA DELL'INDIRIZZO
    Private Function TrovaSuggerimenti() As DataSet
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String '= "SELECT top 1 indirizzo FROM VW_VALIDAZIONEINDIRIZZO INNER JOIN Comuni ON comuni.idcomune=VW_VALIDAZIONEINDIRIZZO.idcomune WHERE comuni.codiceISTAT = '" & _CodiceIstat & "'"
        Dim arrayIndirizzo As Array
        Dim intX As Integer

        strSql = "SELECT TOP 6 INDIRIZZO FROM CAP_INDIRIZZI "
        strSql = strSql & "WHERE IDCOMUNE  = '" & _IdComune & "' "

        arrayIndirizzo = Split(_Indirizzo, " ")

        For intX = 0 To UBound(arrayIndirizzo)
            Dim checkParola As New clsControllaParoleIndirizzi(arrayIndirizzo.GetValue(intX))
            strSql = strSql & "OR INDIRIZZO LIKE '%" & checkParola.CheckIndirizzo & "%' "
        Next

        strSql = strSql & "ORDER BY "
        strSql = strSql & "DIFFERENCE (INDIRIZZO, '" & _Indirizzo & "') DESC, "
        strSql = strSql & "DIFFERENCE(LEFT(INDIRIZZO,6), LEFT('" & _Indirizzo & "',6)) DESC "

        Dim CMD As New SqlClient.SqlDataAdapter(strSql, _LocalConn)
        Dim DstPrimario As New DataSet
        CMD.Fill(DstPrimario)
        TrovaSuggerimenti = DstPrimario
        DstPrimario = Nothing

        Return TrovaSuggerimenti

    End Function

    Private Function TrovaSuggerimentiPerParola() As DataSet
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String '= "SELECT top 1 indirizzo FROM VW_VALIDAZIONEINDIRIZZO INNER JOIN Comuni ON comuni.idcomune=VW_VALIDAZIONEINDIRIZZO.idcomune WHERE comuni.codiceISTAT = '" & _CodiceIstat & "'"
        Dim intX As Integer

        strSql = "SELECT TOP 4 INDIRIZZO FROM CAP_INDIRIZZI "
        strSql = strSql & "WHERE IDCOMUNE  = '" & _IdComune & "' "
        strSql = strSql & "AND INDIRIZZO LIKE '%" & Replace(_Indirizzo, "'", "''") & "%' "
        strSql = strSql & "ORDER BY INDIRIZZO"

        Dim CMD As New SqlClient.SqlDataAdapter(strSql, _LocalConn)
        Dim DstPrimario As New DataSet
        CMD.Fill(DstPrimario)
        TrovaSuggerimentiPerParola = DstPrimario
        DstPrimario = Nothing

        Return TrovaSuggerimentiPerParola

    End Function

    'PROPRIETA' CLASSE CHE RITORNA SE L'INDIRIZZO ESISTE O MENO
    Public ReadOnly Property IndirizziSimili() As DataSet
        Get
            Return TrovaSuggerimenti()
        End Get
    End Property

    'PROPRIETA' CLASSE CHE RITORNA SE L'INDIRIZZO ESISTE O MENO
    Public ReadOnly Property IndirizzoSimilePerParola() As DataSet
        Get
            Return TrovaSuggerimentiPerParola()
        End Get
    End Property

End Class
