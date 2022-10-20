Imports System.Collections.Generic
Imports System.IO
Imports Aspose.Pdf
Imports Aspose.Pdf.Facades
''' <summary>
''' Classe Helper per l'uso semplificato di AsposePDF
''' G. de Nicolellis
''' ''' </summary>
''' <remarks></remarks>
Public Class AsposePDF
	Implements IDisposable
	Public sigNames As List(Of String)
	Public doc As Document
	Public Sub New()
		license() 'abilita il prodotto
	End Sub
	''' <summary>
	''' Crea un nuovo oggetto AsposePDF che rappresenta il file pdf su disco il cui path viene passato come argomento
	''' </summary>
	''' <param name="filepath">il path del file pdf da aprire</param>
	''' <remarks></remarks>
	Public Sub New(ByVal filepath As String)
		Me.New()
		Open(filepath)
	End Sub
	''' <summary>
	''' Crea un nuovo oggetto AsposePDF che rappresenta il file pdf contenuto nello stream passato come argomento
	''' </summary>
	''' <param name="stream"></param>
	''' <remarks></remarks>
	Public Sub New(ByVal stream As MemoryStream)
		Me.New()
		Open(stream)
	End Sub
	''' <summary>
	''' Crea un nuovo oggetto AsposePDF che rappresenta il file pdf codificato nell'array di byte passato come argomento
	''' </summary>
	''' <param name="byteArray"></param>
	''' <remarks></remarks>
	Public Sub New(ByVal byteArray As Byte())
		Me.New()
		Open(byteArray)
	End Sub
	''' <summary>
	''' Controlla se la licenza AsposePDF inglobata nella libreria é correntemente valida
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function Licensed() As Boolean
		Return Document.IsLicensed()
	End Function
	Private Sub license()
		Dim Key As String = "PExpY2Vuc2U+CiAgPERhdGE+CiAgICA8TGljZW5zZWRUbz5Mb2dpY2EgSW5mb3JtYXRpY2Egcy5yLmwuIChMaW5hKTwvTGljZW5zZWRUbz4KICAgIDxFbWFpbFRvPmwuZ2lhbmZvcmNoZXR0aUBsb2dpY2FpbmZvcm1hdGljYS5pdDwvRW1haWxUbz4KICAgIDxMaWNlbnNlVHlwZT5EZXZlbG9wZXIgT0VNPC9MaWNlbnNlVHlwZT4KICAgIDxMaWNlbnNlTm90ZT4xIERldmVsb3BlciBBbmQgVW5saW1pdGVkIERlcGxveW1lbnQgTG9jYXRpb25zPC9MaWNlbnNlTm90ZT4KICAgIDxPcmRlcklEPjIxMDMyMzE2NTkxMjwvT3JkZXJJRD4KICAgIDxVc2VySUQ+ODYzOTY2PC9Vc2VySUQ+CiAgICA8T0VNPlRoaXMgaXMgYSByZWRpc3RyaWJ1dGFibGUgbGljZW5zZTwvT0VNPgogICAgPFByb2R1Y3RzPgogICAgICA8UHJvZHVjdD5Bc3Bvc2UuVG90YWwgZm9yIC5ORVQ8L1Byb2R1Y3Q+CiAgICA8L1Byb2R1Y3RzPgogICAgPEVkaXRpb25UeXBlPlByb2Zlc3Npb25hbDwvRWRpdGlvblR5cGU+CiAgICA8U2VyaWFsTnVtYmVyPmI2ZGRhMjkyLTk1OWEtNGRmMS05ZDgyLWMyZDg2ZjE0NzJjODwvU2VyaWFsTnVtYmVyPgogICAgPFN1YnNjcmlwdGlvbkV4cGlyeT4yMDIyMDQwNzwvU3Vic2NyaXB0aW9uRXhwaXJ5PgogICAgPExpY2Vuc2VWZXJzaW9uPjMuMDwvTGljZW5zZVZlcnNpb24+CiAgICA8TGljZW5zZUluc3RydWN0aW9ucz5odHRwczovL3B1cmNoYXNlLmFzcG9zZS5jb20vcG9saWNpZXMvdXNlLWxpY2Vuc2U8L0xpY2Vuc2VJbnN0cnVjdGlvbnM+CiAgPC9EYXRhPgogIDxTaWduYXR1cmU+Z1ByM3FWaTBNYzBadVFhWmhNT0JKUUYyVWVBRVpvT3c2bEpKcU1hSnlYUGZiWk00ZzJZZ09xUUFCTFd5MjIyN2pnRjJrUjBYTjdBZUo5WkdVakVLZk4xMGdQNnoyUTlTeExVeWRFRUxHK2VrcW9NeXVyNU9UT3FhMFYwQkF3VXZUMFRPVmF6UUY2Rmg4WXliTDZJMU1nNEp2MG9idzlFUDdnWEkyN0RTUnBRPTwvU2lnbmF0dXJlPgo8L0xpY2Vuc2U+"
		Dim LStream As Stream = New MemoryStream(Convert.FromBase64String(Key))
		Dim license As New License()
		license.SetLicense(LStream)
	End Sub
	''' <summary>
	''' Apre un file pdf su disco
	''' </summary>
	''' <param name="filepath">il path del file pdf su disco</param>
	''' <remarks></remarks>
	Public Sub Open(ByVal filepath As String)
		doc = New Document(filepath)
	End Sub
	''' <summary>
	''' Apre un file pdf contenuto in un MemoryStream 
	''' </summary>
	''' <param name="stream"></param>
	''' <remarks></remarks>
	Public Sub Open(ByVal stream As MemoryStream)
		doc = New Document(stream)
	End Sub
	''' <summary>
	''' Apre un file pdf contenuto in un array di byte
	''' </summary>
	''' <param name="byteArray"></param>
	''' <remarks></remarks>
	Public Sub Open(ByVal byteArray As Byte())
		Dim stream As New MemoryStream(byteArray)
		Open(stream)
	End Sub
	''' <summary>
	'''  Controlla se il documento pdf caricato è correttamente firmato. Se ha piu' firme controlla che tutte le firme siano valide
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function IsSignedOK() As Boolean
		Dim ok As Boolean = False
		Using signature As New PdfFileSignature(doc)
			sigNames = signature.GetSignNames()
			If sigNames.Count < 1 Then Return False
			For i As Integer = 0 To sigNames.Count - 1
				If Not signature.VerifySignature(sigNames(i)) Then
					Exit For
				End If
			Next
			ok = True
		End Using
		Return ok
	End Function
	''' <summary>
	'''  Controlla se il documento pdf caricato è firmato, cioè se esiste almeno una firma. Non controlla la validità
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function IsSigned() As Boolean
		Dim ok As Boolean = False
		Using signature As New PdfFileSignature(doc)
			sigNames = signature.GetSignNames()
			Return (sigNames.Count > 0)
		End Using
	End Function
	Public Shared Function IsSigned(ByVal filepath As String) As Boolean
		Using pdf As New AsposePDF(filepath)
			Return pdf.IsSigned()
		End Using
	End Function
	Public Shared Function IsSigned(ByVal bytearray As Byte()) As Boolean
		Using pdf As New AsposePDF(bytearray)
			Return pdf.IsSigned()
		End Using
	End Function
	Public Shared Function IsSigned(ByVal stream As MemoryStream) As Boolean
		Using pdf As New AsposePDF(stream)
			Return pdf.IsSigned()
		End Using
	End Function

	Sub Dispose() Implements System.IDisposable.Dispose
		If Not IsNothing(doc) Then doc.Dispose()
		GC.SuppressFinalize(Me)
	End Sub

	Public Function GetSignitures() As Object
		Using signature As New PdfFileSignature(doc)
			Dim firme = signature.GetSignNames()
			For Each firma In firme
				Dim certificato = signature.ExtractCertificate(firma)
				Dim immagine = signature.ExtractImage(firma)
				Dim a = 1
			Next
		End Using
		Return Nothing
	End Function


End Class
