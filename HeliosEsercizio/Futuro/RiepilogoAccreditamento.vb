Imports System.Collections.Generic

Namespace RiepilogoAccreditamento
	Public Class Ente
		Public Sub New()
			SettoriIntervento = New List(Of Settore)
			Accordi = New List(Of Accordo)
			Risorse = New List(Of Risorsa)
			Sedi = New List(Of Sede)
			Allegati = New List(Of Allegato)
			RuoliAntimafia = New List(Of RuoloAntimafia)
		End Sub

		Public Property Sezione As String
		Public Property IdClasseAccreditamento As Integer
		Public Property Privato As Boolean
		Public Property IdEnte As Integer
		Public Property CodiceEnte As String
		Public Property Denominazione As String
		Public Property Classe As String
		Public Property Tipo As String
		Public Property DataRichiesta As Date?
		Public Property NumeroFase As Integer
		Public Property CodiceFiscale As String
		Public Property DataNominaRappresentanteLegale As Date?
		Public Property Telefono As String
		Public Property DataCostituzione As Date?
		Public Property Fax As String
		Public Property Sito As String
		Public Property Email As String
		Public Property PEC As String
		Public Property SedeLegale As Sede
		Public Property NumeroSedi As Integer
		Public Property NumeroSediEstere As Integer
		Public Property DettaglioRecapito As String
		Public Property RappresentanteLegale As Risorsa
		Public Property SettoriIntervento As List(Of Settore)
		Public Property Accordi As List(Of Accordo)
		Public Property Risorse As List(Of Risorsa)
		Public Property Sedi As List(Of Sede)
		Public Property Allegati As List(Of Allegato)
		Public Property RuoliAntimafia As List(Of RuoloAntimafia)
		Public Property Formatori As Integer
		Public Property Selettori As Integer
		Public Property EspertiMonitoraggio As Integer
	End Class

	Public Class Accordo
		Inherits Ente
		Public Property Relazione As String
		Public Property DataStipula As Date
		Public Property DataScadenza As Date
	End Class
	Public Class Settore
		Public Sub New()
			Esperienze = New Dictionary(Of Integer, String)
			AreeIntervento = New List(Of AreaIntervento)
		End Sub

		Public Property IdEnte As Integer
		Public Property IdSettore As String
		Public Property Codifica As String
		Public Property Nome As String
		Public Property AreeDecoded As String
		Public Property Esperienze As Dictionary(Of Integer, String)
		Public Property AreeIntervento As List(Of AreaIntervento)
	End Class

	Public Class Sede
		Public Property IdEnte As Integer
		Public Property IdTipoSede As Integer
		Public Property Nome As String
		Public Property DenominazioneEnte As String
		Public Property NumeroVolontari As Integer
		Public Property Indirizzo As String
		Public Property DettaglioRecapito As String
		Public Property TitoloGiuridico As String
		Public Property Tipo As String
		Public Property Sito As String
		Public Property Email As String
		Public Property Nazionale As Boolean
		Public Property Esperienze As Dictionary(Of Integer, String)
		Public Property AreaIntervento As List(Of String)
	End Class

	Public Class Risorsa
		Public Sub New()
			Ruoli = New List(Of Ruolo)
		End Sub

		Public IdEnte As Integer
		Public Property Nome As String
		Public Property Titolo As String
		Public Property Posizione As String
		Public Property CodiceFiscale As String
		Public Property Email As String
		Public Property Telefono As String
		Public Property Fax As String
		Public Property Cellulare As String
		Public Property DataNascita As Date?
		Public Property LuogoNascita As String
		Public Property IndirizzoResidenza As String
		Public Property DettaglioRecapito As String
		Public Property Esperienza As String
		Public Property CorsoFormazione As String
		Public Property Ruoli As List(Of Ruolo)
	End Class

	Public Class RuoloAntimafia

		Public IdEnte As Integer
		Public Property Ruolo As String
		Public Property Nome As String
		Public Property Cognome As String
		Public Property CodiceFiscale As String
		Public Property CodiceFiscaleEnte As String
		Public Property Email As String
		Public Property Telefono As String
		Public Property Fax As String
		Public Property DataNascita As Date?
		Public Property LuogoNascita As String
	End Class



	Public Class AreaIntervento
		Public Property IdArea As String
		Public Property Codifica As String
		Public Property IdSettore As String
		Public Property Area As String

	End Class

	Public Class Ruolo
		Public Property IdRuolo As Integer
		Public Property CodiceFiscale As String
		Public Property Ruolo As String
	End Class

	Public Class Allegato
		Public Property Id As Integer
		Public Property NomeFile As String
		Public Property Tipologia As String
		Public Property Hash As String
	End Class


End Namespace