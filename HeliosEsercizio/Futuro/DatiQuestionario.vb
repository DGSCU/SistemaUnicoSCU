Public Class DatiQuestionario

    Private valoreid As String
    Public Property Id() As String
        Get
            Return valoreid
        End Get
        Set(ByVal value As String)
            valoreid = value
        End Set
    End Property

    Private valoreGruppo As String
    Public Property Gruppo() As String
        Get
            Return valoreGruppo
        End Get
        Set(ByVal value As String)
            valoreGruppo = value
        End Set
    End Property


    Private valoreTipoOggetto As TipoOggetto
    Public Property TipoOggetto() As TipoOggetto
        Get
            Return valoreTipoOggetto
        End Get
        Set(ByVal value As TipoOggetto)
            valoreTipoOggetto = value
        End Set
    End Property


    Private valoreDescrizione As String
    Public Property Descrizione() As String
        Get
            Return valoreDescrizione
        End Get
        Set(ByVal value As String)
            valoreDescrizione = value
        End Set
    End Property



End Class

Public Enum TipoOggetto
    CheckBox = 0
    RadioButton = 1

End Enum

Public Class DatiValutazioneQualita
    Inherits DatiQuestionario

End Class
