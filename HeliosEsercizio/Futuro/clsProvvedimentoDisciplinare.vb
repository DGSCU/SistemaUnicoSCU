
Imports System.Data.SqlClient
<Serializable()>
 Public Class clsProvvedimentoDisciplinare

#Region " PROPRIETA'"
    Dim _IDProvvedimentoDisciplinare As Integer
    Property IDProvvedimentoDisciplinare() As Integer
        Get
            Return Me._IDProvvedimentoDisciplinare
        End Get
        Set(ByVal Value As Integer)
            Me._IDProvvedimentoDisciplinare = Value
        End Set
    End Property

    Private _IDAttivitàEntità As Integer
    Property IDAttivitàEntità() As Integer
        Get
            Return Me._IDAttivitàEntità
        End Get
        Set(ByVal Value As Integer)
            Me._IDAttivitàEntità = Value
        End Set
    End Property

    Private _IDEntità As Integer
    Property IDEntità() As Integer
        Get
            Return Me._IDEntità
        End Get
        Set(ByVal Value As Integer)
            Me._IDEntità = Value
        End Set
    End Property
    Private _IDStatoProvvedimento As Integer
    Property IDStatoProvvedimento() As Integer
        Get
            Return Me._IDStatoProvvedimento
        End Get
        Set(ByVal Value As Integer)
            Me._IDStatoProvvedimento = Value
        End Set
    End Property

    Dim _CodiceFascicolo As String
    Property CodiceFascicolo() As String
        Get
            Return Me._CodiceFascicolo
        End Get
        Set(ByVal Value As String)
            Me._CodiceFascicolo = Value
        End Set
    End Property

    Dim _IDFascicolo As String
    Property IDFascicolo() As String
        Get
            Return Me._IDFascicolo
        End Get
        Set(ByVal Value As String)
            Me._IDFascicolo = Value
        End Set
    End Property

    Dim _DescFasicolo As String
    Property DescFasicolo() As String
        Get
            Return Me._DescFasicolo
        End Get
        Set(ByVal Value As String)
            Me._DescFasicolo = Value
        End Set
    End Property

    Dim _DataProtocolloComunicazione As String
    Property DataProtocolloComunicazione() As String
        Get
            Return Me._DataProtocolloComunicazione
        End Get
        Set(ByVal Value As String)
            Me._DataProtocolloComunicazione = Value
        End Set
    End Property

    Dim _NumeroProtocolloComunicazione As String
    Property NumeroProtocolloComunicazione() As String
        Get
            Return Me._NumeroProtocolloComunicazione
        End Get
        Set(ByVal Value As String)
            Me._NumeroProtocolloComunicazione = Value
        End Set
    End Property

    Dim _DataProtocolloAvvioProvvedimento As String
    Property DataProtocolloAvvioProvvedimento() As String
        Get
            Return Me._DataProtocolloAvvioProvvedimento
        End Get
        Set(ByVal Value As String)
            Me._DataProtocolloAvvioProvvedimento = Value
        End Set
    End Property

    Dim _NumeroProtocolloAvvioProvvedimento As String
    Property NumeroProtocolloAvvioProvvedimento() As String
        Get
            Return Me._NumeroProtocolloAvvioProvvedimento
        End Get
        Set(ByVal Value As String)
            Me._NumeroProtocolloAvvioProvvedimento = Value
        End Set
    End Property

    Dim _DataProtocolloControdeduzioni As String
    Property DataProtocolloControdeduzioni() As String
        Get
            Return Me._DataProtocolloControdeduzioni
        End Get
        Set(ByVal Value As String)
            Me._DataProtocolloControdeduzioni = Value
        End Set
    End Property

    Dim _NumeroProtocolloControdeduzioni As String
    Property NumeroProtocolloControdeduzioni() As String
        Get
            Return Me._NumeroProtocolloControdeduzioni
        End Get
        Set(ByVal Value As String)
            Me._NumeroProtocolloControdeduzioni = Value
        End Set
    End Property

    Dim _DataProtocolloChiusuraProvvedimento As String
    Property DataProtocolloChiusuraProvvedimento() As String
        Get
            Return Me._DataProtocolloChiusuraProvvedimento
        End Get
        Set(ByVal Value As String)
            Me._DataProtocolloChiusuraProvvedimento = Value
        End Set
    End Property

    Dim _NumeroProtocolloChiusuraProvvedimento As String
    Property NumeroProtocolloChiusuraProvvedimento() As String
        Get
            Return Me._NumeroProtocolloChiusuraProvvedimento
        End Get
        Set(ByVal Value As String)
            Me._NumeroProtocolloChiusuraProvvedimento = Value
        End Set
    End Property
    Dim _DataProtocolloSanzioni As String
    Property DataProtocolloSanzioni() As String
        Get
            Return Me._DataProtocolloSanzioni
        End Get
        Set(ByVal Value As String)
            Me._DataProtocolloSanzioni = Value
        End Set
    End Property

    Dim _NumeroProtocolloSanzioni As String
    Property NumeroProtocolloSanzioni() As String
        Get
            Return Me._NumeroProtocolloSanzioni
        End Get
        Set(ByVal Value As String)
            Me._NumeroProtocolloSanzioni = Value
        End Set
    End Property

    Dim _DataInserimento As String
    ReadOnly Property DataInserimento() As String
        Get
            Return Me._DataInserimento
        End Get
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

    Dim _DataUltimaModifica As Date
    ReadOnly Property DataUltimaModifica() As String
        Get
            Return Me._DataUltimaModifica
        End Get
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

    Dim _VengoDa As String
    Property VengoDa() As String
        Get
            Return Me._VengoDa
        End Get
        Set(ByVal Value As String)
            Me._VengoDa = Value
        End Set
    End Property
#End Region

    Public Function Inserisci(ByVal ConnX As SqlConnection, ByRef IdProvvDisciplinare As Integer) As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim mytrans As SqlClient.SqlTransaction
        Try
            SqlCmd.CommandText = "SP_VOLONTARI_PROVVEDIMENTO_DISCIPLINARE_INSERIMENTO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            ' mytrans = ConnX.BeginTransaction
            SqlCmd.Connection = ConnX
            'SqlCmd.Transaction = mytrans

            SqlCmd.Parameters.Add("@IDAttivitàEntità ", Me._IDAttivitàEntità)
            SqlCmd.Parameters.Add("@IDStatoProvvedimento", Me._IDStatoProvvedimento)
            SqlCmd.Parameters.Add("@CodiceFascicolo", Me._CodiceFascicolo)
            SqlCmd.Parameters.Add("@IDFascicolo", Me._IDFascicolo)
            SqlCmd.Parameters.Add("@DESCRFASCICOLO", Me._DescFasicolo)
            SqlCmd.Parameters.Add("@DataProtocolloComunicazione", clsConversione.DaVuotoInNull(Me._DataProtocolloComunicazione))
            SqlCmd.Parameters.Add("@NumeroProtocolloComunicazione", clsConversione.DaVuotoInNull(Me._NumeroProtocolloComunicazione))

            SqlCmd.Parameters.Add("@DataProtocolloAvvioProvvedimento", clsConversione.DaVuotoInNull(Me._DataProtocolloAvvioProvvedimento))
            SqlCmd.Parameters.Add("@NumeroProtocolloAvvioProvvedimento", clsConversione.DaVuotoInNull(Me._NumeroProtocolloAvvioProvvedimento))

            SqlCmd.Parameters.Add("@DataProtocolloControdeduzioni", clsConversione.DaVuotoInNull(Me._DataProtocolloControdeduzioni))
            SqlCmd.Parameters.Add("@NumeroProtocolloControdeduzioni", clsConversione.DaVuotoInNull(Me._NumeroProtocolloControdeduzioni))
            SqlCmd.Parameters.Add("@DataProtocolloChiusuraProvvedimento", clsConversione.DaVuotoInNull(Me._DataProtocolloChiusuraProvvedimento))
            SqlCmd.Parameters.Add("@NumeroProtocolloChiusuraProvvedimento", clsConversione.DaVuotoInNull(Me._NumeroProtocolloChiusuraProvvedimento))
            SqlCmd.Parameters.Add("@DataProtocolloSanzioni", clsConversione.DaVuotoInNull(Me._DataProtocolloSanzioni))
            SqlCmd.Parameters.Add("@NumeroProtocolloSanzioni", clsConversione.DaVuotoInNull(Me._NumeroProtocolloSanzioni))

            SqlCmd.Parameters.Add("@UserInseritore", Me._UserInseritore)
            SqlCmd.Parameters.Add("@NOTE", clsConversione.DaVuotoInNull(Me._Note))


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 4
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@messaggio"
            sparam2.Size = 1000
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@IDProvvDisciplinare"
            sparam3.SqlDbType = SqlDbType.Int
            sparam3.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam3)


            SqlCmd.ExecuteScalar()
            IdProvvDisciplinare = SqlCmd.Parameters("@IDProvvDisciplinare").Value
            'mytrans.Commit()
            Return True

        Catch ex As Exception

        End Try
    End Function

    Public Function Modifica(ByVal ConnX As SqlConnection) As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim mytrans As SqlClient.SqlTransaction
        Try
            SqlCmd.CommandText = "SP_VOLONTARI_PROVVEDIMENTO_DISCIPLINARE_MODIFICA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            ' mytrans = ConnX.BeginTransaction
            SqlCmd.Connection = ConnX
            'SqlCmd.Transaction = mytrans

            SqlCmd.Parameters.Add("@IDProvvedimentoDisciplinare", Me._IDProvvedimentoDisciplinare)
            SqlCmd.Parameters.Add("@IDStatoProvvedimento", Me._IDStatoProvvedimento)
            SqlCmd.Parameters.Add("@CodiceFascicolo", Me._CodiceFascicolo)
            SqlCmd.Parameters.Add("@IDFascicolo", Me._IDFascicolo)
            SqlCmd.Parameters.Add("@DescrFascicolo", Me._DescFasicolo)
            SqlCmd.Parameters.Add("@DataProtocolloComunicazione", clsConversione.DaVuotoInNull(Me._DataProtocolloComunicazione))
            SqlCmd.Parameters.Add("@NumeroProtocolloComunicazione", clsConversione.DaVuotoInNull(Me._NumeroProtocolloComunicazione))
            SqlCmd.Parameters.Add("@DataProtocolloAvvioProvvedimento", clsConversione.DaVuotoInNull(Me._DataProtocolloAvvioProvvedimento))
            SqlCmd.Parameters.Add("@NumeroProtocolloAvvioProvvedimento", clsConversione.DaVuotoInNull(Me._NumeroProtocolloAvvioProvvedimento))
            SqlCmd.Parameters.Add("@DataProtocolloControdeduzioni", clsConversione.DaVuotoInNull(Me._DataProtocolloControdeduzioni))
            SqlCmd.Parameters.Add("@NumeroProtocolloControdeduzioni", clsConversione.DaVuotoInNull(Me._NumeroProtocolloControdeduzioni))
            SqlCmd.Parameters.Add("@DataProtocolloChiusuraProvvedimento", clsConversione.DaVuotoInNull(Me._DataProtocolloChiusuraProvvedimento))
            SqlCmd.Parameters.Add("@NumeroProtocolloChiusuraProvvedimento", clsConversione.DaVuotoInNull(Me._NumeroProtocolloChiusuraProvvedimento))
            SqlCmd.Parameters.Add("@DataProtocolloSanzioni", clsConversione.DaVuotoInNull(Me._DataProtocolloSanzioni))
            SqlCmd.Parameters.Add("@NumeroProtocolloSanzioni", clsConversione.DaVuotoInNull(Me._NumeroProtocolloSanzioni))
            SqlCmd.Parameters.Add("@UserUltimaModifica", Me._UserUltimaModifica)
            SqlCmd.Parameters.Add("@Note", clsConversione.DaVuotoInNull(Me._Note))


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 4
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@messaggio"
            sparam2.Size = 1000
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam2)


            SqlCmd.ExecuteNonQuery()
            'mytrans.Commit()
            Return True

        Catch ex As Exception

        End Try
    End Function
End Class
