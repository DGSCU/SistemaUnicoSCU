<Serializable()> _
Public Class clsVerificatore

    Dim _IdVerificatore As Integer
    Property IdVerificatore() As Integer
        Get
            Return Me._IdVerificatore
        End Get
        Set(ByVal Value As Integer)
            Me._IdVerificatore = Value
        End Set
    End Property

    Dim _Cognome As String
    Property Cognome() As String
        Get
            Return Me._Cognome
        End Get
        Set(ByVal Value As String)
            Me._Cognome = Value
        End Set
    End Property


    Dim _Nome As String
    Property Nome() As String
        Get
            Return Me._Nome
        End Get
        Set(ByVal Value As String)
            Me._Nome = Value
        End Set
    End Property

    Dim _IdCircoscrizione As Integer
    Property IdCircoscrizione() As Integer
        Get
            Return Me._IdCircoscrizione
        End Get
        Set(ByVal Value As Integer)
            Me._IdCircoscrizione = Value
        End Set
    End Property

    Dim _Abilitato As Integer
    Property Abilitato() As Integer
        Get
            Return Me._Abilitato
        End Get
        Set(ByVal Value As Integer)
            Me._Abilitato = Value
        End Set
    End Property

    Dim _Tipologia_Old As Integer
    Private Property Tipologia_Old() As Integer
        Get
            Return Me._Tipologia_Old
        End Get
        Set(ByVal Value As Integer)
            Me._Tipologia_Old = Value
        End Set
    End Property

    Dim _Tipologia As Integer
    Property Tipologia() As Integer
        Get
            Return Me._Tipologia
        End Get
        Set(ByVal Value As Integer)
            If Not Me._Tipologia = Value Then
                Me._Tipologia_Old = Me._Tipologia
            End If
            Me._Tipologia = Value
        End Set
    End Property

    Dim _Riferimento As String
    Property Riferimento() As String
        Get
            Return Me._Riferimento
        End Get
        Set(ByVal Value As String)
            Me._Riferimento = Value
        End Set
    End Property

    Dim _Email As String
    Property Email() As String
        Get
            Return Me._Email
        End Get
        Set(ByVal Value As String)
            Me._Email = Value
        End Set
    End Property

    Dim _TelInterno As String
    Property TelInterno() As String
        Get
            Return Me._TelInterno
        End Get
        Set(ByVal Value As String)
            Me._TelInterno = Value
        End Set
    End Property

    Dim _TelCell As String
    Property TelCell() As String
        Get
            Return Me._TelCell
        End Get
        Set(ByVal Value As String)
            Me._TelCell = Value
        End Set
    End Property


    Dim _UserInseritore As String
    Property UserInseritore() As String
        Get
            Return Me._UserInseritore
        End Get
        Set(ByVal Value As String)
            Me._UserInseritore = Value
        End Set
    End Property

    Dim _UserUltimaModifica As String
    Property UserUltimaModifica() As String
        Get
            Return Me._UserUltimaModifica
        End Get
        Set(ByVal Value As String)
            Me._UserUltimaModifica = Value
        End Set
    End Property

    Dim _Note As String
    Property Note() As String
        Get
            Return Me._Note
        End Get
        Set(ByVal Value As String)
            Me._Note = Value
        End Set
    End Property

    Dim _VerificatoreInterno_Old As Integer
    Private Property VerificatoreInterno_Old() As Integer
        Get
            Return Me._VerificatoreInterno_Old
        End Get
        Set(ByVal Value As Integer)
            Me._VerificatoreInterno_Old = Value
        End Set
    End Property

    Dim _VerificatoreInterno As Integer
    Property VerificatoreInterno() As Integer
        Get
            Return Me._VerificatoreInterno
        End Get
        Set(ByVal Value As Integer)
            If Not Me._VerificatoreInterno = Value Then
                Me._VerificatoreInterno_Old = Me._VerificatoreInterno
            End If
            Me._VerificatoreInterno = Value
        End Set
    End Property

    Dim _IdRegCompetenza As Integer
    Property IdRegCompetenza() As Integer
        Get
            Return Me._IdRegCompetenza
        End Get
        Set(ByVal Value As Integer)
            Me._IdRegCompetenza = Value
        End Set
    End Property

    Dim _IdUser As Integer
    Property IdUser() As Integer
        Get
            Return Me._IdUser
        End Get
        Set(ByVal Value As Integer)
            Me._IdUser = Value
        End Set
    End Property

    Public Shared Function RecuperaVerificatoriInterni(ByVal Connx As SqlClient.SqlConnection, ByVal IdRegCompetenza As Integer) As DataTable
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim SqlDa As New SqlClient.SqlDataAdapter(SqlCmd)
        Dim Dt As New DataTable
        Dim strSql As String

        SqlCmd.Connection = Connx
        strSql = "SELECT IDVERIFICATORE,COGNOME + ' ' + NOME AS NOMECOMPLETO,COGNOME,NOME FROM TVERIFICATORI WHERE TIPOLOGIA=0 and IdRegCompetenza=" & IdRegCompetenza & ""


        SqlCmd.CommandText = strSql
        SqlDa.Fill(Dt)
        Return Dt
    End Function

    Public Function Inserisci(ByVal ConnX As SqlClient.SqlConnection) As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand
        SqlCmd.CommandText = "SP_INS_VERIFICATORE"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = ConnX
        SqlCmd.Parameters.Add("@COGNOME", clsConversione.DaVuotoInNull(Me._Cognome))
        SqlCmd.Parameters.Add("@NOME", clsConversione.DaVuotoInNull(Me._Nome))
        SqlCmd.Parameters.Add("@IDCIRCOSCRIZIONE", clsConversione.DaVuotoInNull(Me._IdCircoscrizione))
        SqlCmd.Parameters.Add("@ABILITATO", clsConversione.DaVuotoInNull(Me._Abilitato))
        SqlCmd.Parameters.Add("@TIPOLOGIA", clsConversione.DaVuotoInNull(Me._Tipologia))
        SqlCmd.Parameters.Add("@RIFERIMENTO", clsConversione.DaVuotoInNull(Me._Riferimento))
        SqlCmd.Parameters.Add("@EMAIL", clsConversione.DaVuotoInNull(Me._Email))
        SqlCmd.Parameters.Add("@TELINTERNO", clsConversione.DaVuotoInNull(Me._TelInterno))
        SqlCmd.Parameters.Add("@TELCELL", clsConversione.DaVuotoInNull(Me._TelCell))
        SqlCmd.Parameters.Add("@USERINSERITORE", clsConversione.DaVuotoInNull(Me._UserInseritore))
        SqlCmd.Parameters.Add("@NOTE", clsConversione.DaVuotoInNull(Me._Note))
        SqlCmd.Parameters.Add("@IDVERIFICATOREINTERNO", clsConversione.DaZeroInNull(Me._VerificatoreInterno))
        SqlCmd.Parameters.Add("@IDREGCOMPETENZA", clsConversione.DaZeroInNull(Me._IdRegCompetenza))
        SqlCmd.Parameters.Add("@IDUSER", clsConversione.DaZeroInNull(Me._IdUser))
        SqlCmd.ExecuteNonQuery()
        Return True
    End Function

    Public Function Modifica(ByVal ConnX As SqlClient.SqlConnection) As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand
        SqlCmd.CommandText = "SP_UPD_VERIFICATORE"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = ConnX
        SqlCmd.Parameters.Add("@IDVERIFICATORE_OLD", Me._IdVerificatore)
        SqlCmd.Parameters.Add("@COGNOME", clsConversione.DaVuotoInNull(Me._Cognome))
        SqlCmd.Parameters.Add("@NOME", clsConversione.DaVuotoInNull(Me._Nome))
        SqlCmd.Parameters.Add("@IDCIRCOSCRIZIONE", clsConversione.DaVuotoInNull(Me._IdCircoscrizione))
        SqlCmd.Parameters.Add("@ABILITATO", clsConversione.DaVuotoInNull(Me._Abilitato))
        SqlCmd.Parameters.Add("@TIPOLOGIA", clsConversione.DaVuotoInNull(Me._Tipologia))
        SqlCmd.Parameters.Add("@RIFERIMENTO", clsConversione.DaVuotoInNull(Me._Riferimento))
        SqlCmd.Parameters.Add("@EMAIL", clsConversione.DaVuotoInNull(Me._Email))
        SqlCmd.Parameters.Add("@TELINTERNO", clsConversione.DaVuotoInNull(Me._TelInterno))
        SqlCmd.Parameters.Add("@TELCELL", clsConversione.DaVuotoInNull(Me._TelCell))
        SqlCmd.Parameters.Add("@USERULTIMAMODIFICA", clsConversione.DaVuotoInNull(Me._UserUltimaModifica))
        SqlCmd.Parameters.Add("@NOTE", clsConversione.DaVuotoInNull(Me._Note))
        SqlCmd.Parameters.Add("@IDVERIFICATOREINTERNO", clsConversione.DaZeroInNull(Me._VerificatoreInterno))
        SqlCmd.Parameters.Add("@IDVERIFICATOREINTERNO_OLD", clsConversione.DaZeroInNull(Me._VerificatoreInterno_Old))
        SqlCmd.Parameters.Add("@TIPOLOGIA_OLD", Me._Tipologia_Old)
        SqlCmd.Parameters.Add("@IDREGCOMPETENZA", clsConversione.DaZeroInNull(Me._IdRegCompetenza))
        SqlCmd.Parameters.Add("@IDUSER", clsConversione.DaZeroInNull(Me._IdUser))
        SqlCmd.ExecuteNonQuery()
        Return True
    End Function

    Public Shared Function RecuperaVerificatore(ByVal IdX As Integer, ByVal ConnX As SqlClient.SqlConnection, Optional ByVal IdVI As Integer = 0) As clsVerificatore
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim Dr As SqlClient.SqlDataReader
        Dim Ver As clsVerificatore
        Dim strSql As String


        SqlCmd.Connection = ConnX
        strSql = "SELECT A.*,B.IDVERIFICATOREINTERNO FROM TVERIFICATORI A LEFT JOIN TVERIFICHEASSOCIAUSER B ON A.IDVERIFICATORE =B.IDVERIFICATOREIGF " & _
                 " WHERE IDVERIFICATORE=" + IdX.ToString + " "
        If IdVI.ToString <> 0 Then
            strSql = strSql & " and  B.IDVERIFICATOREINTERNO = " + IdVI.ToString + " "
        End If
        SqlCmd.CommandText = strSql
        Dr = SqlCmd.ExecuteReader()
        If Dr.Read() Then
            Ver = New clsVerificatore
            Ver._IdVerificatore = IdX
            Ver._Cognome = clsConversione.DaNullInNothing(Dr("COGNOME"))
            Ver._Nome = clsConversione.DaNullInNothing(Dr("NOME"))
            Ver._IdCircoscrizione = clsConversione.DaNullInNothing(Dr("IDCIRCOSCRIZIONE"))
            Ver._Riferimento = clsConversione.DaNullInNothing(Dr("RIFERIMENTO"))
            Ver._Abilitato = clsConversione.DaBitAInteger(Dr("ABILITATO"))
            Ver._Tipologia = clsConversione.DaBitAInteger(Dr("TIPOLOGIA"))
            'Ver._Tipologia_Old = clsConversione.DaBitAInteger(Dr("TIPOLOGIA"))
            Ver._Email = clsConversione.DaNullInNothing(Dr("EMAIL"))
            Ver._TelInterno = clsConversione.DaNullInNothing(Dr("TELINTERNO"))
            Ver._TelCell = clsConversione.DaNullInNothing(Dr("TELCELL"))
            Ver._Note = clsConversione.DaNullInNothing(Dr("NOTE"))
            Ver._VerificatoreInterno = clsConversione.DaNullInNothing(Dr("IDVERIFICATOREINTERNO"))
            Ver._IdRegCompetenza = clsConversione.DaNullInNothing(Dr("IDREGCOMPETENZA"))
            Ver._IdUser = clsConversione.DaNullInNothing(Dr("IDUTENTE"))
            'Ver._VerificatoreInterno_Old = clsConversione.DaNullInNothing(Dr("IDVERIFICATOREINTERNO"))
        End If
        Dr.Close()
        Return Ver
    End Function

    Public Shared Function Ricerca(ByVal ConnX As SqlClient.SqlConnection, Optional ByVal CognomeX As String = "", Optional ByVal NomeX As String = "", Optional ByVal IdCircoscrizioneX As String = "", Optional ByVal IdVerificatoreInternoX As String = "", Optional ByVal TipologiaX As String = "", Optional ByVal AbilitatoX As String = "", Optional ByVal IdRegCompetenza As Integer = 0) As DataTable
        Dim SqlCmd As SqlClient.SqlCommand
        Dim Dt As New DataTable
        Dim Da As New SqlClient.SqlDataAdapter(SqlCmd)
        SqlCmd = New SqlClient.SqlCommand
        SqlCmd.CommandText = "SP_SEL_VERIFICATORI"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = ConnX
        SqlCmd.Parameters.Add("@COGNOME", clsConversione.DaVuotoInNull(CognomeX))
        SqlCmd.Parameters.Add("@NOME", clsConversione.DaVuotoInNull(NomeX))
        SqlCmd.Parameters.Add("@IDCIRCOSCRIZIONE", clsConversione.DaVuotoInNull(IdCircoscrizioneX))
        SqlCmd.Parameters.Add("@ABILITATO", clsConversione.DaVuotoInNull(AbilitatoX))
        SqlCmd.Parameters.Add("@IDVERIFICATOREINTERNO", clsConversione.DaVuotoInNull(IdVerificatoreInternoX))
        SqlCmd.Parameters.Add("@TIPOLOGIA", clsConversione.DaVuotoInNull(TipologiaX))
        SqlCmd.Parameters.Add("@IDREGCOMPETENZA", clsConversione.DaVuotoInNull(IdRegCompetenza))
        SqlCmd.ExecuteNonQuery()
        Da.SelectCommand = SqlCmd
        'If ConnX.State = ConnectionState.Closed Then ConnX.Open()
        Da.Fill(Dt)
        'ConnX.Close()
        Dt.Columns.Add("DescrAbilitato")
        Dt.Columns("DescrAbilitato").Expression = "IIF(Abilitato=0,'SI','NO')"
        Return Dt
    End Function

    Public Shared Function RecuperaCircoscrizioni(ByVal Connx As SqlClient.SqlConnection, ByVal IdRegCompetenza As Integer) As DataTable
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim SqlDa As New SqlClient.SqlDataAdapter(SqlCmd)
        Dim Dt As New DataTable
        Dim strsql As String
        SqlCmd.Connection = Connx

        strsql = "SELECT IDCIRCOSCRIZIONE,CIRCOSCRIZIONE FROM TVERIFICHECIRCOSCRIZIONI WHERE ABILITATO=0"
        If IdRegCompetenza = 22 Then
            strsql = strsql & " and CIRCOSCRIZIONE<> 'Regionale'"
        Else
            strsql = strsql & " and CIRCOSCRIZIONE ='Regionale'"
        End If
        SqlCmd.CommandText = strsql
        SqlDa.Fill(Dt)
        Return Dt
    End Function

End Class
