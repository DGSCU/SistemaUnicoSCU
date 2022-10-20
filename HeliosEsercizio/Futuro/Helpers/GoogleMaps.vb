Imports System.Net
Imports System.IO
Imports Newtonsoft.Json
Public Class GoogleMaps
    Private key As String = ""
    Public lastError As String = ""
    Public GoogleAddress As String = ""
    Public GoogleLocality As String = ""
    Public GoogleCivico As String = ""
    Public GoogleNazione As String = ""
    Public GoogleCap As String = ""
    Public GoogleFormattedAddress As String = ""
    Public okCivico As Boolean = False
    Public okCap As Boolean = False
    Public okLocality As Boolean = False
    Public okRoute As Boolean = False
    Public GoogleAdministrativeArea3 As String
    Public Sub New(ByVal GoogleKey As String)
        key = GoogleKey
    End Sub
    Public Shared Function LevenshteinDistance(ByVal s As String, ByVal t As String) As Integer
        Dim n As Integer = s.Length
        Dim m As Integer = t.Length
        Dim d(n + 1, m + 1) As Integer

        If n = 0 Then Return m
        If m = 0 Then Return n

        Dim i As Integer
        Dim j As Integer

        For i = 0 To n
            d(i, 0) = i
        Next

        For j = 0 To m
            d(0, j) = j
        Next

        For i = 1 To n
            For j = 1 To m

                Dim cost As Integer
                If t(j - 1) = s(i - 1) Then
                    cost = 0
                Else
                    cost = 1
                End If

                d(i, j) = Math.Min(Math.Min(d(i - 1, j) + 1, d(i, j - 1) + 1),
                                   d(i - 1, j - 1) + cost)
            Next
        Next

        Return d(n, m)
    End Function
    Public Shared Function NormalizeString(ByVal s As String) As String
        s = s.ToLower()
        For i As Integer = 1 To Len(s)
            Dim c As String = Mid(s, i)
            If c <= "a" Or c > "z" Then Mid(s, i, 1) = " "
        Next
        s = Replace(s, " ", "")
        Return s
    End Function
    Private Function URLEncode(ByVal Text As String) As String
        Dim URLencshort As String = ""
        Dim lngA As Long, strChar As String
        For lngA = 1 To Len(Text)
            strChar = Mid$(Text, lngA, 1)
            If strChar Like "[A-Za-z0-9]" Then
            ElseIf strChar = " " Then
                strChar = "+"
            Else
                strChar = "%" & Right$("0" & Hex$(Asc(strChar)), 2)
            End If
            URLencshort = URLencshort & strChar
        Next lngA
        Return URLencshort
    End Function
    Private Function geocode(ByVal url As String) As Object
        Dim inStream As StreamReader
        Dim webRequest As WebRequest
        Dim webresponse As WebResponse
        webRequest = webRequest.Create(url)
        webresponse = webRequest.GetResponse()
        inStream = New StreamReader(webresponse.GetResponseStream())
        Dim s As String = inStream.ReadToEnd()
        Dim jsobject = Newtonsoft.Json.JsonConvert.DeserializeObject(s)
        Return jsobject
    End Function
    Private Function isComponentType(ByVal component As Object, ByVal type As String) As Boolean
        Dim types As Object = component("types")
        For i As Integer = 0 To types.Count - 1
            If types(i) & "" = type Then Return True
        Next
        Return False
    End Function
    Private Function extractComponent(ByVal addr As Object, ByVal type As String) As String
        Dim longName As String = ""
        Dim shortName As String = ""
        For i As Integer = 0 To addr("results")(0)("address_components").Count - 1
            Dim componente As Object = addr("results")(0)("address_components")(i)
            If isComponentType(componente, type) Then
                Return addr("results")(0)("address_components")(i)("short_name") & ""
            End If
        Next
        Return ""
    End Function
    ''' <summary>
    ''' controlla un indirizzo su Google
    ''' </summary>
    ''' <param name="codiceNazione">Il codice ISO/web della nazione: Italia=IT, Francia=FR, ecc.</param>
    ''' <param name="localita"></param>
    ''' <param name="indirizzo"></param>
    ''' <param name="civico"></param>
    ''' <param name="CAP"></param>
    ''' <returns>true: l'indirizzo trovato su google e' (circa) uguale a quello cercato; false: qualcosa non va. 
    ''' Nella proprieta' lastError si trova la spiegazione dell'errore, e nella proprieta' GoogleFormattedAddress si trova
    ''' l'indirizzo suggerito da google </returns>
    ''' <remarks></remarks>
    Public Function checkAddress(ByVal codiceNazione As String, ByVal localita As String, ByVal indirizzo As String, ByVal civico As String, ByVal CAP As String) As Boolean
        localita = Trim(localita)
        indirizzo = Trim(indirizzo)
        civico = Trim(civico)
        CAP = Trim(CAP)
        Dim url1 As String = String.Format("https://maps.google.com/maps/api/geocode/json?key={0}&language=IT&country={1}&locality={2}&postal_code={3}&address={4}, {5}, {2}, {3}", _
            URLEncode(key), Uri.EscapeUriString(codiceNazione), Uri.EscapeUriString(localita), Uri.EscapeUriString(CAP), Uri.EscapeUriString(indirizzo), Uri.EscapeUriString(civico))
        Dim addr1
        Try
            addr1 = geocode(url1)
        Catch ex As Exception
            Return False
        End Try
        If addr1("status") & "" <> "OK" Then
            lastError = "Status non OK"
            Return False
        End If
        Me.GoogleCivico = extractComponent(addr1, "street_number")
        Me.okCivico = (GoogleCivico.ToLower = civico.ToLower)
        Me.GoogleNazione = extractComponent(addr1, "country")
        Me.GoogleCap = extractComponent(addr1, "postal_code")
        Me.GoogleAddress = extractComponent(addr1, "route")
        Me.GoogleFormattedAddress = addr1("results")(0)("formatted_address")
        Me.GoogleLocality = extractComponent(addr1, "locality")
        Me.GoogleAdministrativeArea3 = extractComponent(addr1, "administrative_area_level_3")

        If GoogleAddress = "" Then GoogleFormattedAddress = ""

        If GoogleNazione <> codiceNazione Or (codiceNazione.ToUpper() = "IT" And localita.ToUpper <> GoogleLocality.ToUpper And localita.ToUpper <> GoogleAdministrativeArea3.ToUpper) Then
            lastError = "Indirizzo non trovato su GoogleMaps: l'indirizzo risulta a " & GoogleLocality & " [" & GoogleNazione & "]"
            GoogleFormattedAddress = ""
            Return False
        End If

        If civico <> "" AndAlso GoogleCivico.ToLower <> civico.ToLower And civico.ToLower <> "snc" Then
            lastError = "civico non esistente"
            Return False
        End If

        If codiceNazione <> "" AndAlso GoogleNazione.ToLower <> codiceNazione.ToLower Then
            lastError = "Nazione errata"
            GoogleFormattedAddress = ""
            Return False
        End If


        If CAP <> "" AndAlso GoogleCap.ToLower <> CAP.ToLower And CAP.ToLower <> "ND" Then
            lastError = "CAP errato"
            Return False
        End If

        If IsNothing(GoogleLocality) Or GoogleAddress = "" Then
            lastError = "Località errata"
            Return False
        End If
        If localita <> "" AndAlso GoogleLocality.ToLower <> localita.ToLower AndAlso localita.ToLower <> GoogleAdministrativeArea3.ToLower Then
            lastError = "Località errata"
            Return False
        End If

        'calcola se l'indirizzo e' "uguale". Vista la difformita' tra indirizzi, per dichiarare "uguale" 
        'l'indirizzo trovato su google e quello originale, si calcola la distanza di Levensthein (quante modifiche servono 
        'per passare da una stringa all'altra) tra le due stringhe (normalizzate) (eliminando tutti i caratteri non [a,z])
        'si considera "buono" un valore minore di 5 (cosi' "VIA DI SAN LEO" e "VIA S. LEO" vengono considerate lo stesso indirizzo
        If LevenshteinDistance(NormalizeString(indirizzo), NormalizeString(GoogleAddress)) >= 5 Then
            lastError = "Indirizzo errato"
            Return False
        End If

        Return True
    End Function

End Class
