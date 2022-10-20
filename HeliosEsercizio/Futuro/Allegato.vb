Public Class Allegato
    Private _Updated As Boolean = False
    Private _Id As Integer?
    Private _FileSize As Integer
    Private _Hash As String
    Private _Blob As Byte()
    Public _Filename As String
    Private _DataInserimento As Date
    Private _IdTipoAllegato As Integer
    Private _IdEnteFase As Integer
    Private _linkId As Integer 'l'eventuale link al record che lo contiene: ad esempi in entepersonale, l'IdEntePersonale del record che punta a questo documento

    Public Property Updated() As Boolean
        Get
            Return _Updated
        End Get
        Set(ByVal value As Boolean)
            _Updated = value
        End Set
    End Property

    Public Property Id() As Integer?
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer?)
            _Id = value
        End Set
    End Property

    Public Property Filesize() As Integer
        Get
            Return _FileSize
        End Get
        Set(ByVal value As Integer)
            _FileSize = value
        End Set
    End Property

    Public Property Blob() As Byte()
        Get
            Return _Blob
        End Get
        Set(ByVal value As Byte())
            _Blob = value
        End Set
    End Property

    Public Property Hash() As String
        Get
            Return _Hash
        End Get
        Set(ByVal value As String)
            _Hash = value
        End Set
    End Property

    Public Property Filename() As String
        Get
            Return _Filename
        End Get
        Set(ByVal value As String)
            _Filename = value
        End Set
    End Property

    Public Property DataInserimento() As Date
        Get
            Return _DataInserimento
        End Get
        Set(ByVal value As Date)
            _DataInserimento = value
        End Set
    End Property

    Public Property IdTipoAllegato As String
        Get
            Return _IdTipoAllegato
        End Get
        Set(ByVal value As String)
            _IdTipoAllegato = value
        End Set
    End Property

    Public Property IdEnteFase As String
        Get
            Return _IdEnteFase
        End Get
        Set(ByVal value As String)
            _IdEnteFase = value
        End Set
    End Property

    Public Property LinkId As Integer
        Get
            Return _linkId
        End Get
        Set(value As Integer)
            _linkId = value
        End Set
    End Property

End Class

