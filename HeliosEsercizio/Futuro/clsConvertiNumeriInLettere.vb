Public Class clsConvertiNumeriInLettere
    Dim Tabella_Nomi(19) As String
    Dim Tabella_Decine(9) As String

    Public Sub Auto_open()
        ' Inizializza tabella al caricamento del modulo
        Tabella_Nomi(1) = "uno"
        Tabella_Nomi(2) = "due"
        Tabella_Nomi(3) = "tre"
        Tabella_Nomi(4) = "quattro"
        Tabella_Nomi(5) = "cinque"
        Tabella_Nomi(6) = "sei"
        Tabella_Nomi(7) = "sette"
        Tabella_Nomi(8) = "otto"
        Tabella_Nomi(9) = "nove"
        Tabella_Nomi(10) = "dieci"
        Tabella_Nomi(11) = "undici"
        Tabella_Nomi(12) = "dodici"
        Tabella_Nomi(13) = "tredici"
        Tabella_Nomi(14) = "quattordici"
        Tabella_Nomi(15) = "quindici"
        Tabella_Nomi(16) = "sedici"
        Tabella_Nomi(17) = "diciassette"
        Tabella_Nomi(18) = "diciotto"
        Tabella_Nomi(19) = "diciannove"
        Tabella_Decine(1) = "dieci"
        Tabella_Decine(2) = "venti"
        Tabella_Decine(3) = "trenta"
        Tabella_Decine(4) = "quaranta"
        Tabella_Decine(5) = "cinquanta"
        Tabella_Decine(6) = "sessanta"
        Tabella_Decine(7) = "settanta"
        Tabella_Decine(8) = "ottanta"
        Tabella_Decine(9) = "novanta"
    End Sub

    Public Function NumToCars(ByVal Numero As Double) As String
        Dim Virgola As Integer
        Dim StrIntero As String
        Dim Decimale As String

        Auto_open()

        Virgola = InStr(1, Str$(Numero), ".", 0)
        StrIntero = Milioni_e_Migliaia(Int(Numero))
        If Virgola = 0 Then
            NumToCars = StrIntero & "/00"
        Else
            Decimale = Milioni_e_Migliaia _
            (Val(Mid$(Str$(Numero), Virgola + 1)))
            If Int(Numero) = 0 Then
                NumToCars = "zero/" & Decimale
            Else
                If (Decimale = "") Then
                    NumToCars = StrIntero
                Else
                    NumToCars = StrIntero & "/" & _
                    Decimale
                End If
            End If
        End If
    End Function

    Private Function Milioni_e_Migliaia(ByVal Numero As Double) As String
        Dim Assoluto As Double
        Dim NumMilioni As Double
        Dim Milioni As String
        Dim Var1 As Double, NumMigliaia As Double
        Dim Migliaia As String

        If Numero > 999999999 Then
            Milioni_e_Migliaia = "Numero troppo grande !"
            Exit Function
        End If

        Assoluto = Int(Numero)
        NumMilioni = Int(Assoluto / 1000000)
        If NumMilioni = 0 Then
            Milioni = ""
        ElseIf NumMilioni = 1 Then
            Milioni = "unmilione"
        Else
            Milioni = Centinaia(NumMilioni) & "milioni"
        End If
        Var1 = Assoluto Mod 1000000
        NumMigliaia = Int(Var1 / 1000)
        If NumMigliaia = 1 Then
            Migliaia = "Mille"
        Else
            If NumMigliaia <> 0 Then Migliaia = _
            Centinaia(NumMigliaia) & "mila"
        End If
        Milioni_e_Migliaia = Milioni & Migliaia & _
        Centinaia(Var1 Mod 1000)
    End Function

    Private Function Centinaia(ByVal Numero As Double) As String
        Dim NumCentinaia As Integer, StrCentinaia As String
        NumCentinaia = Int(Numero / 100)
        If NumCentinaia > 0 Then
            If NumCentinaia = 1 Then
                StrCentinaia = "cento"
            Else
                StrCentinaia = Tabella_Nomi(NumCentinaia) & _
                "cento"
            End If
        End If
        Centinaia = StrCentinaia & Decine_e_Unita(Numero - _
        (NumCentinaia * 100))
    End Function

    Private Function Decine_e_Unita(ByVal Numero As Double) As String
        Dim Decine As String, Unita As Integer

        If Numero = 0 Then
            Decine_e_Unita = ""
        Else
            If Numero < 20 Then
                Decine_e_Unita = Tabella_Nomi(Numero)
            Else
                Decine = Tabella_Decine(Int(Numero / 10))
                Unita = Numero Mod 10
                If Unita = 0 Then
                    Decine_e_Unita = Decine
                Else
                    Decine_e_Unita = Decine & _
                    Tabella_Nomi(Unita)
                End If
            End If
        End If
    End Function

End Class
