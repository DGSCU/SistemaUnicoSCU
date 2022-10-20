
Option Explicit On
Imports System.Collections.Generic
Public Class clsEsperienzeAree
	Private _idSettore As Integer
	Private _annoEsperienza As List(Of Integer)
	Private _descrizioneEsperienza As List(Of String)
    Private _areeSelezionate As List(Of String)
	Private _areeCompetenza As String

	Public Property IdSettore As Integer
		Get
			Return _idSettore
		End Get
		Set(ByVal value As Integer)
			_idSettore = value
		End Set
	End Property

	Public Property AnnoEsperienza As List(Of Integer)
		Get
			Return _annoEsperienza
		End Get
		Set(ByVal value As List(Of Integer))
			_annoEsperienza = value
		End Set
	End Property


	Public Property DescrizioneEsperienza As List(Of String)
		Get
			Return _descrizioneEsperienza
		End Get
		Set(ByVal value As List(Of String))
			_descrizioneEsperienza = value
		End Set
	End Property

    Public Property AreeSelezionate As List(Of String)
        Get
            Return _areeSelezionate
        End Get
        Set(ByVal value As List(Of String))
            _areeSelezionate = value
        End Set
    End Property

	Public Property AreeCompetenza As String
		Get
			Return _areeCompetenza
		End Get
		Set(ByVal value As String)
			_areeCompetenza = value
		End Set
	End Property

End Class
