Imports System.Data.SqlClient
<Serializable()>
Public Class clsVerificaSegnalazione

    Enum TipoVerifica
        Programmata = 1
        Su_Segnalazione
    End Enum
#Region " PROPRIETA'"
    Dim _IdVerifica As Integer
    Property IdVerifica() As Integer
        Get
            Return Me._IdVerifica
        End Get
        Set(ByVal Value As Integer)
            Me._IdVerifica = Value
        End Set
    End Property
    Private _IdSegnalazione As Integer
    Property IdSegnalazione() As Integer
        Get
            Return Me._IdSegnalazione
        End Get
        Set(ByVal Value As Integer)
            Me._IdSegnalazione = Value
        End Set
    End Property

    Dim _Tipologia As TipoVerifica
    Property Tipologia() As TipoVerifica
        Get
            Return Me._Tipologia
        End Get
        Set(ByVal Value As TipoVerifica)
            Me._Tipologia = Value
        End Set
    End Property

    Dim _IdAttivitaEnteSedeAttuazione As Integer
    Property IdAttivitaEnteSedeAttuazione() As Integer
        Get
            Return Me._IdAttivitaEnteSedeAttuazione
        End Get
        Set(ByVal Value As Integer)
            Me._IdAttivitaEnteSedeAttuazione = Value
        End Set
    End Property

    Dim _IdStatoVerifica As Integer
    Property IdStatoVerifica() As Integer
        Get
            Return Me._IdStatoVerifica
        End Get
        Set(ByVal Value As Integer)
            Me._IdStatoVerifica = Value
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

    Dim _CodiceFascicoloInterno As String
    Property CodiceFascicoloInterno() As String
        Get
            Return Me._CodiceFascicoloInterno
        End Get
        Set(ByVal Value As String)
            Me._CodiceFascicoloInterno = Value
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

    Dim _CodiceVerifica As String
    Property CodiceVerifica() As String
        Get
            Return Me._CodiceVerifica
        End Get
        Set(ByVal Value As String)
            Me._CodiceVerifica = Value
        End Set
    End Property

    Dim _DataInserimento As String
    Property DataInserimento() As String
        Get
            Return Me._DataInserimento
        End Get
        Set(ByVal Value As String)
            Me._DataInserimento = Value
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

    Dim _Note As String
    Property Note() As String
        Get
            Return Me._Note
        End Get
        Set(ByVal Value As String)
            Me._Note = Value
        End Set
    End Property

    Dim _Oggetto As String
    Property Oggetto() As String
        Get
            Return Me._Oggetto
        End Get
        Set(ByVal Value As String)
            Me._Oggetto = Value
        End Set
    End Property

    Dim _DataRicezioneSegnalazione As String
    Property DataRicezioneSegnalazione() As String
        Get
            Return Me._DataRicezioneSegnalazione
        End Get
        Set(ByVal Value As String)
            Me._DataRicezioneSegnalazione = Value
        End Set
    End Property

    Dim _DataProtSegnalazione As String
    Property DataProtSegnalazione() As String
        Get
            Return Me._DataProtSegnalazione
        End Get
        Set(ByVal Value As String)
            Me._DataProtSegnalazione = Value
        End Set
    End Property

    Dim _NProtSegnalazione As String
    Property NProtSegnalazione() As String
        Get
            Return Me._NProtSegnalazione
        End Get
        Set(ByVal Value As String)
            Me._NProtSegnalazione = Value
        End Set
    End Property

    Dim _Fonte As Integer
    Property Fonte() As Integer
        Get
            Return Me._Fonte
        End Get
        Set(ByVal Value As Integer)
            Me._Fonte = Value
        End Set
    End Property

    Dim _DataProtInvioLetteraInterlocutoria As String
    Property DataProtInvioLetteraInterlocutoria() As String
        Get
            Return Me._DataProtInvioLetteraInterlocutoria
        End Get
        Set(ByVal Value As String)
            Me._DataProtInvioLetteraInterlocutoria = Value
        End Set
    End Property

    Dim _NProtInvioLetteraInterlocutoria As String
    Property NProtInvioLetteraInterlocutoria() As String
        Get
            Return Me._NProtInvioLetteraInterlocutoria
        End Get
        Set(ByVal Value As String)
            Me._NProtInvioLetteraInterlocutoria = Value
        End Set
    End Property

    Dim _DataProtRispostaLetteraInterlocutoria As String
    Property DataProtRispostaLetteraInterlocutoria() As String
        Get
            Return Me._DataProtRispostaLetteraInterlocutoria
        End Get
        Set(ByVal Value As String)
            Me._DataProtRispostaLetteraInterlocutoria = Value
        End Set
    End Property

    Dim _NProtRispostaLetteraInterlocutoria As String
    Property NProtRispostaLetteraInterlocutoria() As String
        Get
            Return Me._NProtRispostaLetteraInterlocutoria
        End Get
        Set(ByVal Value As String)
            Me._NProtRispostaLetteraInterlocutoria = Value
        End Set
    End Property
    Private _IDVerificheAssociate As Integer
    Property IDVerificheAssociate() As Integer
        Get
            Return Me._IDVerificheAssociate
        End Get
        Set(ByVal Value As Integer)
            Me._IDVerificheAssociate = Value
        End Set
    End Property
    Dim _IdRegCompetenza As String
    Property IdRegCompetenza() As String
        Get
            Return Me._IdRegCompetenza
        End Get
        Set(ByVal Value As String)
            Me._IdRegCompetenza = Value
        End Set
    End Property

    Dim _DataInizioPrevista As String
    Property DataInizioPrevista() As String
        Get
            Return Me._DataInizioPrevista
        End Get
        Set(ByVal Value As String)
            Me._DataInizioPrevista = Value
        End Set
    End Property
    Dim _DataFinePrevista As String
    Property DataFinePrevista() As String
        Get
            Return Me._DataFinePrevista
        End Get
        Set(ByVal Value As String)
            Me._DataFinePrevista = Value
        End Set
    End Property
#End Region


    Public Function Inserisci(ByVal ConnX As SqlConnection, ByVal Str As String) As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim mytrans As SqlClient.SqlTransaction
        Try

            If Not Str <> "" Then
                SqlCmd.CommandText = "SP_INS_VERIFICASUSEGNALAZIONE"
                SqlCmd.CommandType = CommandType.StoredProcedure
                mytrans = ConnX.BeginTransaction
                SqlCmd.Connection = ConnX
                SqlCmd.Transaction = mytrans
                SqlCmd.Parameters.Add("@CODICEFASCICOLO", clsConversione.DaVuotoInNull(Me._CodiceFascicolo))
                SqlCmd.Parameters.Add("@IDFASCICOLO", clsConversione.DaVuotoInNull(Me._CodiceFascicoloInterno))
                SqlCmd.Parameters.Add("@DESCRFASCICOLO", clsConversione.DaVuotoInNull(Me._DescFasicolo))
                SqlCmd.Parameters.Add("@OGGETTO", clsConversione.DaVuotoInNull(Me._Oggetto))
                SqlCmd.Parameters.Add("@ESITOSEGNALAZIONE", clsConversione.DaVuotoInNull(Me._CodiceVerifica)) 'codiceverifica è esito segnalazione
                SqlCmd.Parameters.Add("@DATARICEZIONESEGNALAZIONE", clsConversione.DaVuotoInNull(Me._DataRicezioneSegnalazione))
                SqlCmd.Parameters.Add("@DATAPROTSEGNALAZIONE", clsConversione.DaVuotoInNull(Me._DataProtSegnalazione))
                SqlCmd.Parameters.Add("@NPROTSEGNALAZIONE", clsConversione.DaVuotoInNull(Me._NProtSegnalazione))
                SqlCmd.Parameters.Add("@FONTE", clsConversione.DaStringaInData(Me._Fonte))
                SqlCmd.Parameters.Add("@DATAPROTINVIOLETTERAINTERLOCUTORIA", clsConversione.DaStringaInData(Me._DataProtInvioLetteraInterlocutoria))
                SqlCmd.Parameters.Add("@NPROTINVIOLETTERAINTERLOCUTORIA", clsConversione.DaVuotoInNull(Me._NProtInvioLetteraInterlocutoria))
                SqlCmd.Parameters.Add("@DATAPROTRISPOSTALETTERAINTERLOCUTORIA", clsConversione.DaVuotoInNull(Me._DataProtRispostaLetteraInterlocutoria))
                SqlCmd.Parameters.Add("@NPROTRISPOSTALETTERAINTERLOCUTORIA", clsConversione.DaVuotoInNull(Me._NProtRispostaLetteraInterlocutoria))
                SqlCmd.Parameters.Add("@USERINSERITORE", clsConversione.DaStringaInData(Me._UserInseritore))
                SqlCmd.Parameters.Add("@NOTE", clsConversione.DaVuotoInNull(Me._Note))
                SqlCmd.Parameters.Add("@DATAINVIOFAX", DBNull.Value)
                SqlCmd.Parameters.Add("@DATAPROTINVIOFAX", DBNull.Value)
                SqlCmd.Parameters.Add("@NPROTINVIOFAX", DBNull.Value)
                SqlCmd.Parameters.Add("@IDREGCOMPETENZA", clsConversione.DaVuotoInNull(Me._IdRegCompetenza))
                SqlCmd.ExecuteNonQuery()
                mytrans.Commit()
                Return True
            Else
                SqlCmd.CommandText = "SP_INS_VERIFICASUSEGNALAZIONE_TOTALE"
                SqlCmd.CommandType = CommandType.StoredProcedure
                mytrans = ConnX.BeginTransaction
                SqlCmd.Connection = ConnX
                SqlCmd.Transaction = mytrans
                SqlCmd.Parameters.Add("@CODICEFASCICOLO", clsConversione.DaVuotoInNull(Me._CodiceFascicolo))
                SqlCmd.Parameters.Add("@IDFASCICOLO", clsConversione.DaVuotoInNull(Me._CodiceFascicoloInterno))
                SqlCmd.Parameters.Add("@DESCRFASCICOLO", clsConversione.DaVuotoInNull(Me._DescFasicolo))
                SqlCmd.Parameters.Add("@NOTE", clsConversione.DaVuotoInNull(Me._Note))
                SqlCmd.Parameters.Add("@OGGETTO", clsConversione.DaVuotoInNull(Me._Oggetto))
                SqlCmd.Parameters.Add("@ESITOSEGNALAZIONE", clsConversione.DaVuotoInNull(Me._CodiceVerifica))  'codiceverifica è esito segnalazione
                SqlCmd.Parameters.Add("@DATARICEZIONESEGNALAZIONE", clsConversione.DaVuotoInNull(Me._DataRicezioneSegnalazione))
                SqlCmd.Parameters.Add("@DATAPROTSEGNALAZIONE", clsConversione.DaVuotoInNull(Me._DataProtSegnalazione))
                SqlCmd.Parameters.Add("@NPROTSEGNALAZIONE", clsConversione.DaStringaInData(Me._NProtSegnalazione))
                SqlCmd.Parameters.Add("@FONTE", clsConversione.DaStringaInData(Me._Fonte))
                SqlCmd.Parameters.Add("@DATAPROTINVIOLETTERAINTERLOCUTORIA", clsConversione.DaVuotoInNull(Me._DataProtInvioLetteraInterlocutoria))
                SqlCmd.Parameters.Add("@NPROTINVIOLETTERAINTERLOCUTORIA", clsConversione.DaVuotoInNull(Me._NProtInvioLetteraInterlocutoria))
                SqlCmd.Parameters.Add("@DATAPROTRISPOSTALETTERAINTERLOCUTORIA", clsConversione.DaVuotoInNull(Me._DataProtRispostaLetteraInterlocutoria))
                SqlCmd.Parameters.Add("@NPROTRISPOSTALETTERAINTERLOCUTORIA", clsConversione.DaStringaInData(Me._NProtRispostaLetteraInterlocutoria))
                SqlCmd.Parameters.Add("@USERINSERITORE", clsConversione.DaVuotoInNull(Me._UserInseritore))
                SqlCmd.Parameters.Add("@DATAINVIOFAX", DBNull.Value)
                SqlCmd.Parameters.Add("@DATAPROTINVIOFAX", DBNull.Value)
                SqlCmd.Parameters.Add("@NPROTINVIOFAX", DBNull.Value)
                SqlCmd.Parameters.Add("@IDATTIVITAENTESEDEATTUAZIONE", clsConversione.DaVuotoInNull(Me._IdAttivitaEnteSedeAttuazione))
                SqlCmd.Parameters.Add("@IDREGCOMPETENZA", clsConversione.DaVuotoInNull(Me._IdRegCompetenza))
                SqlCmd.ExecuteNonQuery()
                mytrans.Commit()
                Return True
            End If


        Catch ex As Exception
            mytrans.Rollback()
            Return False
        End Try
    End Function

    Public Function Modifica(ByVal ConnX As SqlConnection, ByRef idritorna As Integer) As Boolean
        Dim SqlCmd As New SqlCommand
        Dim mytrans As SqlTransaction
        Dim sampParm As SqlParameter
        Try
            SqlCmd.CommandText = "SP_MOD_VERIFICASUSEGNALAZIONI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            mytrans = ConnX.BeginTransaction
            SqlCmd.Connection = ConnX
            SqlCmd.Transaction = mytrans
            sampParm = SqlCmd.Parameters.Add("@CODICEFASCICOLO", clsConversione.DaVuotoInNull(Me._CodiceFascicolo))
            sampParm = SqlCmd.Parameters.Add("@IDFASCICOLO", clsConversione.DaVuotoInNull(Me._CodiceFascicoloInterno))
            sampParm = SqlCmd.Parameters.Add("@DESCRFASCICOLO", clsConversione.DaVuotoInNull(Me._DescFasicolo))
            sampParm = SqlCmd.Parameters.Add("@NOTE", clsConversione.DaVuotoInNull(Me._Note))
            sampParm = SqlCmd.Parameters.Add("@IDSEGNALAZIONE", clsConversione.DaVuotoInNull(Me._IdSegnalazione))
            sampParm = SqlCmd.Parameters.Add("@IDVERIFICA", clsConversione.DaVuotoInNull(Me._IdVerifica))
            sampParm = SqlCmd.Parameters.Add("@OGGETTO", clsConversione.DaVuotoInNull(Me._Oggetto))
            sampParm = SqlCmd.Parameters.Add("@ESITOSEGNALAZIONE", clsConversione.DaVuotoInNull(Me._CodiceVerifica)) 'codiceverifica è esito segnalazione
            sampParm = SqlCmd.Parameters.Add("@DATARICEZIONESEGNALAZIONE", clsConversione.DaVuotoInNull(Me._DataRicezioneSegnalazione))
            sampParm = SqlCmd.Parameters.Add("@DATAPROTSEGNALAZIONE", clsConversione.DaVuotoInNull(Me._DataProtSegnalazione))
            sampParm = SqlCmd.Parameters.Add("@NPROTSEGNALAZIONE", clsConversione.DaStringaInData(Me._NProtSegnalazione))
            sampParm = SqlCmd.Parameters.Add("@FONTE", clsConversione.DaStringaInData(Me._Fonte))
            sampParm = SqlCmd.Parameters.Add("@DATAPROTINVIOLETTERAINTERLOCUTORIA", clsConversione.DaVuotoInNull(Me._DataProtInvioLetteraInterlocutoria))
            sampParm = SqlCmd.Parameters.Add("@NPROTINVIOLETTERAINTERLOCUTORIA", clsConversione.DaVuotoInNull(Me._NProtInvioLetteraInterlocutoria))
            sampParm = SqlCmd.Parameters.Add("@DATAPROTRISPOSTALETTERAINTERLOCUTORIA", clsConversione.DaVuotoInNull(Me._DataProtRispostaLetteraInterlocutoria))
            sampParm = SqlCmd.Parameters.Add("@NPROTRISPOSTALETTERAINTERLOCUTORIA", clsConversione.DaStringaInData(Me._NProtRispostaLetteraInterlocutoria))
            sampParm = SqlCmd.Parameters.Add("@USERINSERITORE", clsConversione.DaVuotoInNull(Me._UserInseritore))
            sampParm = SqlCmd.Parameters.Add("@DATAINVIOFAX", DBNull.Value)
            sampParm = SqlCmd.Parameters.Add("@DATAPROTINVIOFAX", DBNull.Value)
            sampParm = SqlCmd.Parameters.Add("@NPROTINVIOFAX", DBNull.Value)
            sampParm = SqlCmd.Parameters.Add("@IDATTIVITAENTESEDEATTUAZIONE", clsConversione.DaVuotoInNull(Me._IdAttivitaEnteSedeAttuazione))
            sampParm = SqlCmd.Parameters.Add("@IDVERIFICHEASSOCIATE", clsConversione.DaVuotoInNull(Me._IDVerificheAssociate))
            sampParm = SqlCmd.Parameters.Add("@IDREGCOMPETENZA", clsConversione.DaVuotoInNull(Me._IdRegCompetenza))
            sampParm = SqlCmd.Parameters.Add("@DATAPREVISTAVERIFICA", clsConversione.DaStringaInData(Me._DataInizioPrevista))
            sampParm = SqlCmd.Parameters.Add("@DATAFINEPREVISTAVERIFICA ", clsConversione.DaStringaInData(Me._DataFinePrevista))
            sampParm = SqlCmd.Parameters.Add("@RITORNAID", SqlDbType.Int)
            sampParm.Direction = ParameterDirection.Output
            SqlCmd.ExecuteNonQuery()
            mytrans.Commit()
            If Not IsDBNull(SqlCmd.Parameters("@RITORNAID").Value) Then idritorna = CType(SqlCmd.Parameters("@RITORNAID").Value, Integer)
            Return True

        Catch ex As Exception
            mytrans.Rollback()
            Return False
        End Try
    End Function

    Public Shared Function RecuperaAttivitaEnteSedeAttuazione(ByVal ConnX As SqlConnection, ByVal IDAESA As Integer) As DataTable
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim SqlDa As New SqlClient.SqlDataAdapter(SqlCmd)
        Dim Dt As New DataTable

        '   ",'<img src=images/Icona_Progetto_small.png onclick=VisualizzaVol(' + convert(varchar, idattivitàentesedeattuazione) + ',' + convert(varchar, idente) + ',' + convert(varchar, IDAttività) + ',' + convert(varchar, idattivitàentesedeattuazione) + ') STYLE=cursor:hand title= border=0>'  as LinkVol " & _
        SqlCmd.Connection = ConnX
        SqlCmd.CommandText = "SELECT *, EnteFiglio + ' (' + cast(IDEnteSedeAttuazione as nvarchar) + ')' as Unico," & _
                            " Comune +  ' (' + DescrAbb + ')' as comune1 " & _
                            "FROM ver_vw_ricercasedi WHERE IdAttivitàEnteSedeAttuazione=" + IDAESA.ToString
        SqlDa.Fill(Dt)
        Return Dt
    End Function

    Public Shared Function RecuperaFonte(ByVal ConnX As SqlConnection) As DataTable
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim SqlDa As New SqlClient.SqlDataAdapter(SqlCmd)
        Dim Dt As New DataTable
        SqlCmd.Connection = ConnX
        SqlCmd.CommandText = "SELECT IDFONTE,NOME FROM TVERIFICHEFONTI ORDER BY NOME"
        SqlDa.Fill(Dt)
        Return Dt
    End Function
    Public Shared Function Get_Select_Modifica(ByVal ConnX As SqlConnection, _
                                               ByVal IdSeg As Integer, _
                                               ByVal IdVer As Integer) As SqlDataReader

        Dim SqlCmd As New SqlCommand
        Dim sampParm As SqlParameter
        'Dim dataAdapter As New SqlDataAdapter(SqlCmd)
        'Dt = New DataTable

        'Try
        SqlCmd.CommandText = "SP_SEL_SEGNALAZIONI_SEDI"
        SqlCmd.CommandType = CommandType.StoredProcedure
        sampParm = SqlCmd.Parameters.Add("@IDSEGNALAZIONE", IdSeg)
        sampParm = SqlCmd.Parameters.Add("@IDVERIFICA", IdVer)
        SqlCmd.Connection = ConnX
        Get_Select_Modifica = SqlCmd.ExecuteReader()


        'dataAdapter.Fill(Dt)
        Return Get_Select_Modifica

        'Catch ex As Exception
        '    Dim pippo As String = ex.Message()
        'End Try
    End Function
    Public Shared Function Get_Select_Verificatori(ByVal ConnX As SqlConnection, _
                                                   ByVal IdVer As Integer, _
                                                   ByVal IdSeg As Integer, _
                                                   ByVal X As Byte) As SqlDataReader

        Dim SqlCmd As New SqlCommand
        Dim sampParm As SqlParameter
        'Dim dataAdapter As New SqlDataAdapter(SqlCmd)
        'Dt = New DataTable

        'Try
        SqlCmd.CommandText = "SP_VER_SELECT_VERIFICATORE"
        SqlCmd.CommandType = CommandType.StoredProcedure
        sampParm = SqlCmd.Parameters.Add("@IDVERIFICA", IdVer)
        sampParm = SqlCmd.Parameters.Add("@IDSEGNALAZIONE", IdSeg)
        sampParm = SqlCmd.Parameters.Add("@ISRECUPERODATA", X)
        SqlCmd.Connection = ConnX
        Get_Select_Verificatori = SqlCmd.ExecuteReader()


        'dataAdapter.Fill(Dt)
        Return Get_Select_Verificatori

        'Catch ex As Exception
        '    Dim pippo As String = ex.Message()
        'End Try
    End Function
    Public Shared Function Get_Select_Verificatori(ByVal ConnX As SqlConnection, _
                                                   ByVal IdVer As Integer) As SqlDataReader

        Dim SqlCmd As New SqlCommand
        Dim sampParm As SqlParameter
        'Dim dataAdapter As New SqlDataAdapter(SqlCmd)
        'Dt = New DataTable

        'Try
        SqlCmd.CommandText = "SP_SEL_RECUPERO_VERIFICATORE_AND_ACCREDITAMENTO"
        SqlCmd.CommandType = CommandType.StoredProcedure
        sampParm = SqlCmd.Parameters.Add("@IDVERIFICA", IdVer)
        SqlCmd.Connection = ConnX
        Get_Select_Verificatori = SqlCmd.ExecuteReader()


        'dataAdapter.Fill(Dt)
        Return Get_Select_Verificatori

        'Catch ex As Exception
        '    Dim pippo As String = ex.Message()
        'End Try
    End Function
    Public Shared Function SP_InserimentoModifica_Verificatore(ByVal ConnX As SqlConnection, _
                                                                ByVal idverifica As Integer, _
                                                                ByVal idverificaverificatori As Integer, _
                                                                ByVal segnalatore As Integer, _
                                                                ByVal datainizio As String, _
                                                                ByVal datafine As String, _
                                                                ByVal user As String) As Boolean
        Dim SqlCmd As New SqlCommand
        Dim mytrans As SqlTransaction
        Try
            SqlCmd.CommandText = "SP_INSERIMENTO_MODIFICA_VERIFICATORE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            mytrans = ConnX.BeginTransaction
            SqlCmd.Connection = ConnX
            SqlCmd.Transaction = mytrans
            SqlCmd.Parameters.Add("@IDVERIFICA", clsConversione.DaVuotoInNull(idverifica))
            SqlCmd.Parameters.Add("@IDVERIFICAVERIFICATORI", clsConversione.DaVuotoInNull(idverificaverificatori))
            SqlCmd.Parameters.Add("@IDVERIFICATORE", clsConversione.DaVuotoInNull(segnalatore))
            SqlCmd.Parameters.Add("@DATAINIZIO", clsConversione.DaVuotoInNull(datainizio))
            SqlCmd.Parameters.Add("@DATAFINE", clsConversione.DaVuotoInNull(datafine))
            SqlCmd.Parameters.Add("@USERMODIFICA", clsConversione.DaVuotoInNull(user))
            SqlCmd.ExecuteNonQuery()
            mytrans.Commit()
            Return True

        Catch ex As Exception
            mytrans.Rollback()
            Return False
        End Try
    End Function

    Public Shared Function Approvazione_Ver_Seg(ByVal ConnX As SqlConnection, _
                                                ByVal idVer As Integer, _
                                                ByVal user As String) As Boolean
        Dim SqlCmd As New SqlCommand
        Dim mytrans As SqlTransaction
        Try
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_SEGNALAZIONE_VERIFICA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            mytrans = ConnX.BeginTransaction
            SqlCmd.Connection = ConnX
            SqlCmd.Transaction = mytrans
            SqlCmd.Parameters.Add("@IDVERIFICA", clsConversione.DaVuotoInNull(idVer))
            SqlCmd.Parameters.Add("@USERULTIMAMODIFICA", clsConversione.DaVuotoInNull(user))
            SqlCmd.ExecuteNonQuery()
            mytrans.Commit()
            Return True

        Catch ex As Exception
            mytrans.Rollback()
            Return False
        End Try
    End Function
#Region "GESTIONE SCADENZARIO"
    Public Shared Function Get_Vista_SCADENZARIO_PER_VERIFICHE(ByVal ConnX As SqlConnection, ByVal NameStoredProcedure As String, ByVal IdRegCompetenza As Integer, ByVal MacroTipoProgetto As String) As DataSet
        '******************SCADENZARIO VERIFICHE********************
        '******************'SCADENZARIO RELAZIONI*******************
        '*************'SCADENZARIO INVIO CONTESTAZIONI*******************
        '*************'SCADENZARIO CHIUSURA VERIFICHE*******************
        '*************'SCADENZARIO APPLICAZIONI SANZIONI*******************

        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        Get_Vista_SCADENZARIO_PER_VERIFICHE = New DataSet

        SqlCmd.CommandText = NameStoredProcedure
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = ConnX
        SqlCmd.Parameters.Add("@IDREGCOMPETENZA", IdRegCompetenza)
        SqlCmd.Parameters.Add("@MacroTipoProgetto", MacroTipoProgetto)
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(Get_Vista_SCADENZARIO_PER_VERIFICHE)
        Return Get_Vista_SCADENZARIO_PER_VERIFICHE

    End Function
    Public Shared Function Get_Vista_SCADENZARIO_PER_VERIFICHE(ByVal ConnX As SqlConnection, ByVal NameStoredProcedure As String, ByVal IdVerificatore As Integer, ByVal IdRegCompetenza As Integer, ByVal MacroTipoProgetto As String) As DataSet
        '******************SCADENZARIO VERIFICHE********************
        '******************'SCADENZARIO RELAZIONI*******************
        'Quando l'utente è ispettore

        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        Get_Vista_SCADENZARIO_PER_VERIFICHE = New DataSet

        SqlCmd.CommandText = NameStoredProcedure
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = ConnX
        SqlCmd.Parameters.Add("@IDVERIFICATORE", IdVerificatore)
        SqlCmd.Parameters.Add("@IDREGCOMPETENZA", IdRegCompetenza)
        SqlCmd.Parameters.Add("@MacroTipoProgetto", MacroTipoProgetto)
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(Get_Vista_SCADENZARIO_PER_VERIFICHE)
        Return Get_Vista_SCADENZARIO_PER_VERIFICHE

    End Function

#End Region
End Class

