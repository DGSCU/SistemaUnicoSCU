Public Class clsDocumentiRegioni
    Private MyIntestazione As String
    Private MySettore As String
    Private MyIndirizzo As String
    Private MyCap As String
    Private MyLocalit‡ As String
    Private MyGazzetta As String
    Private MyNvol As String



    Public Function RecuperaDati(ByVal IdEntit‡ As String, ByVal conn As SqlClient.SqlConnection)
        Dim strsql As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        strsql = "select "
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Bando.GazzettaUfficiale,'∞',''),'Ï','i'''),'È','e'''), " & _
        " '‡','a'''),'Ú','o'''),'Ë','e'''),'˘','u'''),'')as Gazzetta, "
        strsql = strsql & "isnull(bando.Volontari,'')as NVol,"
        strsql = strsql & "isnull(regionicompetenze.IntestazioneDocumenti,'')as IntestazioneDocumenti,"
        strsql = strsql & "isnull(regionicompetenze.SettoreDocumenti,'')as SettoreDocumenti,"
        strsql = strsql & "isnull(regionicompetenze.IndirizzoDocumenti,'')as IndirizzoDocumenti,"
        strsql = strsql & "isnull(regionicompetenze.CapDocumenti,'')as CapDocumenti,"
        strsql = strsql & "isnull(regionicompetenze.Localit‡Documenti,'')as Localit‡Documenti"
        strsql = strsql & " from entit‡ e"
        strsql = strsql & " inner join graduatorieEntit‡ ge on ge.identit‡=e.idEntit‡"
        strsql = strsql & " inner join Attivit‡SediAssegnazione asa on asa.idattivit‡Sedeassegnazione=ge.idattivit‡Sedeassegnazione"
        strsql = strsql & " inner join attivit‡ a on a.idattivit‡=asa.idattivit‡"
        strsql = strsql & " inner join Bandiattivit‡ on a.idbandoattivit‡=bandiattivit‡.idbandoattivit‡"
        strsql = strsql & " inner join bando on bandiattivit‡.idbando=bando.idbando"
        strsql = strsql & " inner join regionicompetenze on a.idregionecompetenza =regionicompetenze.idregionecompetenza"
        strsql = strsql & " where e.identit‡=" & IdEntit‡ & ""

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        dtrLeggiDati = ClsServer.CreaDatareader(strsql, conn)
        dtrLeggiDati.Read()
        MyIntestazione = dtrLeggiDati("IntestazioneDocumenti")
        MySettore = dtrLeggiDati("SettoreDocumenti")
        MyIndirizzo = dtrLeggiDati("IndirizzoDocumenti")
        MyCap = dtrLeggiDati("CapDocumenti")
        MyLocalit‡ = dtrLeggiDati("Localit‡Documenti")
        MyNvol = dtrLeggiDati("NVol")
        MyGazzetta = dtrLeggiDati("Gazzetta")

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

    End Function
    'Public Function RecuperaDatiGraduatorie(ByVal IdEnte As String, ByVal IdBando As String, ByVal conn As SqlClient.SqlConnection)
    '    Dim strsql As String
    '    Dim dtrLeggiDati As SqlClient.SqlDataReader

    '    strsql = "select "
    '    strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Bando.GazzettaUfficiale,'∞',''),'Ï','i'''),'È','e'''), " & _
    '    " '‡','a'''),'Ú','o'''),'Ë','e'''),'˘','u'''),'')as Gazzetta, "
    '    strsql = strsql & "isnull(bando.Volontari,'')as NVol,"
    '    strsql = strsql & "isnull(regionicompetenze.IntestazioneDocumenti,'')as IntestazioneDocumenti,"
    '    strsql = strsql & "isnull(regionicompetenze.SettoreDocumenti,'')as SettoreDocumenti,"
    '    strsql = strsql & "isnull(regionicompetenze.IndirizzoDocumenti,'')as IndirizzoDocumenti,"
    '    strsql = strsql & "isnull(regionicompetenze.CapDocumenti,'')as CapDocumenti,"
    '    strsql = strsql & "isnull(regionicompetenze.Localit‡Documenti,'')as Localit‡Documenti"
    '    strsql = strsql & " from attivit‡ a "
    '    strsql = strsql & " inner join Bandiattivit‡ on a.idbandoattivit‡=bandiattivit‡.idbandoattivit‡"
    '    strsql = strsql & " inner join bando on bandiattivit‡.idbando=bando.idbando"
    '    strsql = strsql & " inner join regionicompetenze on a.idregionecompetenza =regionicompetenze.idregionecompetenza"
    '    strsql = strsql & " where Bandiattivit‡.idEnte=" & IdEnte & "  and bandiattivit‡.IdBando= " & IdBando

    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If

    '    dtrLeggiDati = ClsServer.CreaDatareader(strsql, conn)
    '    dtrLeggiDati.Read()

    '    MyIntestazione = dtrLeggiDati("IntestazioneDocumenti")
    '    MySettore = dtrLeggiDati("SettoreDocumenti")
    '    MyIndirizzo = dtrLeggiDati("IndirizzoDocumenti")
    '    MyCap = dtrLeggiDati("CapDocumenti")
    '    MyLocalit‡ = dtrLeggiDati("Localit‡Documenti")
    '    MyNvol = dtrLeggiDati("NVol")
    '    MyGazzetta = dtrLeggiDati("Gazzetta")

    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If

    'End Function

    Public Property Intestazione()
        Get
            Return MyIntestazione
        End Get
        Set(ByVal Value)

        End Set
    End Property

    Public Property Settore()
        Get
            Return MySettore
        End Get
        Set(ByVal Value)

        End Set
    End Property
    Public Property Indirizzo()
        Get
            Return MyIndirizzo

        End Get
        Set(ByVal Value)

        End Set
    End Property

    Public Property Cap()
        Get
            Return MyCap
        End Get
        Set(ByVal Value)

        End Set
    End Property

    Public Property Localit‡()
        Get
            Return MyLocalit‡
        End Get
        Set(ByVal Value)

        End Set
    End Property
    Public Property Gazzetta()
        Get
            Return MyGazzetta
        End Get
        Set(ByVal Value)

        End Set
    End Property
    Public Property NVolontari()
        Get
            Return MyNvol
        End Get
        Set(ByVal Value)

        End Set
    End Property
End Class
