<Serializable()> _
Public Class clsRequisitoVerifiche

    Dim _IdRequisito As Integer
    Property IdRequisito() As Integer
        Get
            Return _IdRequisito
        End Get
        Set(ByVal Value As Integer)
            _IdRequisito = Value
        End Set
    End Property

    Dim _IdVersioneVerifiche As Integer
    Property IdVersioneVerifiche() As Integer
        Get
            Return _IdVersioneVerifiche
        End Get
        Set(ByVal Value As Integer)
            _IdVersioneVerifiche = Value
        End Set
    End Property

    Dim _Descrizione As String
    Property Descrizione() As String
        Get
            Return _Descrizione
        End Get
        Set(ByVal Value As String)
            _Descrizione = Value
        End Set
    End Property

    Dim _IdTipoRequisito As Integer
    Property IdTipoRequisito() As Integer
        Get
            Return _IdTipoRequisito
        End Get
        Set(ByVal Value As Integer)
            _IdTipoRequisito = Value
        End Set
    End Property

    Dim _IdSanzione As Integer
    Property IdSanzione() As Integer
        Get
            Return _IdSanzione
        End Get
        Set(ByVal Value As Integer)
            _IdSanzione = Value
        End Set
    End Property

    Dim _Abilitato As Integer
    Property Abilitato() As Integer
        Get
            Return _Abilitato
        End Get
        Set(ByVal Value As Integer)
            _Abilitato = Value
        End Set
    End Property

    Dim _Rationale As String
    Property Rationale() As String
        Get
            Return _Rationale
        End Get
        Set(ByVal Value As String)
            _Rationale = Value
        End Set
    End Property

    Dim _RiferimentoCircolare As String
    Property RiferimentoCircolare() As String
        Get
            Return Me._RiferimentoCircolare
        End Get
        Set(ByVal Value As String)
            Me._RiferimentoCircolare = Value
        End Set
    End Property

    Dim _Ordine As Integer
    Property Ordine() As Integer
        Get
            Return _Ordine
        End Get
        Set(ByVal Value As Integer)
            _Ordine = Value
        End Set
    End Property

    Dim _UserInseritore As String
    Property UserInseritore() As String
        Get
            Return _UserInseritore
        End Get
        Set(ByVal Value As String)
            _UserInseritore = Value
        End Set
    End Property

    Dim _UserUltimaModifica As String
    Property UserUltimaModifica() As String
        Get
            Return _UserUltimaModifica
        End Get
        Set(ByVal Value As String)
            _UserUltimaModifica = Value
        End Set
    End Property

    Dim _Note As String
    Property Note() As String
        Get
            Return _Note
        End Get
        Set(ByVal Value As String)
            _Note = Value
        End Set
    End Property

    Dim _DataInserimento As Date
    ReadOnly Property DataInserimento() As Date
        Get
            Return _DataInserimento
        End Get
    End Property

    Dim _DataUltimaModifica As Date
    ReadOnly Property DataUltimaModifica() As Date
        Get
            Return _DataUltimaModifica
        End Get
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

    Dim _IdEsiti As String
    Property IdEsiti() As String
        Get
            Return _IdEsiti
        End Get
        Set(ByVal Value As String)
            _IdEsiti = Value
        End Set
    End Property

    Public Shared Function RecuperaTipoRequisiti(ByVal ConnX As SqlClient.SqlConnection) As DataTable
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim SqlDa As New SqlClient.SqlDataAdapter(SqlCmd)
        Dim Dt As New DataTable
        SqlCmd.Connection = ConnX
        SqlCmd.CommandText = "SELECT IdTipoRequisito,TipoRequisito FROM TVerificheTipoRequisito WHERE Abilitato=0 "
        SqlDa.Fill(Dt)
        Return Dt
    End Function

    Public Shared Function RecuperaTipoSanzioni(ByVal ConnX As SqlClient.SqlConnection) As DataTable
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim SqlDa As New SqlClient.SqlDataAdapter(SqlCmd)
        Dim Dt As New DataTable
        SqlCmd.Connection = ConnX
        SqlCmd.CommandText = "SELECT IdSanzione,Descrizione FROM TVerificheSanzioni WHERE Abilitato=0 "
        SqlDa.Fill(Dt)
        Return Dt
    End Function


    Public Function Inserisci(ByVal ConnX As SqlClient.SqlConnection) As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_INS_REQUISITO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = ConnX
            SqlCmd.Parameters.Add("@IDVERSIONEVERIFICHE", Me.IdVersioneVerifiche)
            SqlCmd.Parameters.Add("@DESCRIZIONE", Me._Descrizione)
            SqlCmd.Parameters.Add("@IDTIPOREQUISITO", Me._IdTipoRequisito)
            SqlCmd.Parameters.Add("@IDSANZIONE", Me._IdSanzione)
            SqlCmd.Parameters.Add("@ABILITATO", Me._Abilitato)
            SqlCmd.Parameters.Add("@RATIONALE", clsConversione.DaVuotoInNull(Me._Rationale))
            SqlCmd.Parameters.Add("@RIFERIMENTOCIRCOLARE", clsConversione.DaVuotoInNull(Me._RiferimentoCircolare))
            SqlCmd.Parameters.Add("@ORDINE", Me._Ordine)
            SqlCmd.Parameters.Add("@USERINSERITORE", Me._UserInseritore)
            SqlCmd.Parameters.Add("@NOTE", clsConversione.DaVuotoInNull(Me._Note))
            SqlCmd.Parameters.Add("@IDREGCOMPETENZA", clsConversione.DaVuotoInNull(Me._IdRegCompetenza))
            SqlCmd.Parameters.Add("@IDESITI", Me.IdEsiti)

            SqlCmd.Parameters.Add("@IDRequisito", SqlDbType.Int)
            SqlCmd.Parameters("@IDRequisito").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            Me.IdRequisito = SqlCmd.Parameters("@IDRequisito").Value()
            Return True
        Catch ex As Exception
            Return False
        Finally
            'ConnX.Close()
        End Try
    End Function

    Public Function Modifica(ByVal ConnX As SqlClient.SqlConnection) As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_UPD_REQUISITO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = ConnX
            SqlCmd.Parameters.Add("@IDREQUISITO", Me._IdRequisito)
            SqlCmd.Parameters.Add("@IDVERSIONEVERIFICHE", Me.IdVersioneVerifiche)
            SqlCmd.Parameters.Add("@DESCRIZIONE", clsConversione.DaVuotoInNull(Me._Descrizione))
            SqlCmd.Parameters.Add("@IDTIPOREQUISITO", Me._IdTipoRequisito)
            SqlCmd.Parameters.Add("@IDSANZIONE", Me._IdSanzione)
            SqlCmd.Parameters.Add("@ABILITATO", Me._Abilitato)
            SqlCmd.Parameters.Add("@RATIONALE", clsConversione.DaVuotoInNull(Me._Rationale))
            SqlCmd.Parameters.Add("@RIFERIMENTOCIRCOLARE", clsConversione.DaVuotoInNull(Me._RiferimentoCircolare))
            SqlCmd.Parameters.Add("@ORDINE", Me._Ordine)
            SqlCmd.Parameters.Add("@USERULTIMAMODIFICA", clsConversione.DaVuotoInNull(Me._UserUltimaModifica))
            SqlCmd.Parameters.Add("@NOTE", Me._Note)
            SqlCmd.Parameters.Add("@IDREGCOMPETENZA", Me._IdRegCompetenza)
            SqlCmd.Parameters.Add("@IDESITI", Me.IdEsiti)
            'If ConnX.State = ConnectionState.Closed Then ConnX.Open()
            SqlCmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Return False
        Finally
            'ConnX.Close()
            'SqlCmd = Nothing
        End Try

        Return True
    End Function


    Public Shared Function RecuperaRequisitoVerifica(ByVal IdX As String, ByVal ConnX As SqlClient.SqlConnection) As clsRequisitoVerifiche
        Dim Req As clsRequisitoVerifiche
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim Dr As SqlClient.SqlDataReader
        SqlCmd.Connection = ConnX
        SqlCmd.CommandText = "SELECT * FROM TVERIFICHEREQUISITO  WHERE IDREQUISITO=" + IdX
        'If ConnX.State = ConnectionState.Closed Then ConnX.Open()
        Dr = SqlCmd.ExecuteReader()
        If Dr.Read Then
            Req = New clsRequisitoVerifiche
            Req._IdRequisito = IdX
            Req._Descrizione = clsConversione.DaNullInNothing(Dr("DESCRIZIONE"))
            Req._Rationale = clsConversione.DaNullInNothing(Dr("RATIONALE"))
            Req._RiferimentoCircolare = clsConversione.DaNullInNothing(Dr("RIFERIMENTOCIRCOLARE"))
            Req._Ordine = clsConversione.DaNullInNothing(Dr("ORDINE"))
            Req._Abilitato = clsConversione.DaBitAInteger(Dr("ABILITATO"))
            Req._IdTipoRequisito = clsConversione.DaNullInNothing(Dr("IDTIPOREQUISITO"))
            Req._IdSanzione = clsConversione.DaNullInNothing(Dr("IDSANZIONE"))
            Req._Note = clsConversione.DaNullInNothing(Dr("NOTE"))
            Req._IdRegCompetenza = clsConversione.DaNullInNothing(Dr("IdRegCompetenza"))
        End If
        Dr.Close()
        Return Req
    End Function

    Public Shared Function Ricerca(ByVal ConnX As SqlClient.SqlConnection, Optional ByVal DescrizioneX As String = "", Optional ByVal TipoRequisitoX As String = "", Optional ByVal SanzioneX As String = "", Optional ByVal AbilitatoX As String = "", Optional ByVal RiferimentoCircolareX As String = "", Optional ByVal IdRegCompetenza As Integer = 0) As DataTable
        Dim SqlCmd As SqlClient.SqlCommand
        Dim Dt As New DataTable
        Dim Da As New SqlClient.SqlDataAdapter(SqlCmd)
        SqlCmd = New SqlClient.SqlCommand
        SqlCmd.CommandText = "SP_SEL_REQUISITIVERIFICHE"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = ConnX
        SqlCmd.Parameters.Add("@DESCRIZIONE", clsConversione.DaVuotoInNull(DescrizioneX))
        SqlCmd.Parameters.Add("@ABILITATO", clsConversione.DaVuotoInNull(AbilitatoX))
        SqlCmd.Parameters.Add("@IDSANZIONE", clsConversione.DaVuotoInNull(SanzioneX))
        SqlCmd.Parameters.Add("@IDTIPOREQUISITO", clsConversione.DaVuotoInNull(TipoRequisitoX))
        SqlCmd.Parameters.Add("@RIFERIMENTOCIRCOLARE", clsConversione.DaVuotoInNull(RiferimentoCircolareX))
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

End Class
