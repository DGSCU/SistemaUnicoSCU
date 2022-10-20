Option Strict Off
Option Explicit On
Public Class CheckBancari
    Private Const L_CONTO As Short = 12
    Private Const L_ABI As Short = 5
    Private Const L_CAB As Short = 5
    Private mAbi As String
    Private mCab As String
    Private mContoCorrente As String
    Private mCin As String
    Private mIBAN As String
    Private mBBAN As String
    Private mCheckDigitIBAN As String
    Private mPaese As String
    Private mDivisore As Short

    'UPGRADE_NOTE: Class_Initialize è stato aggiornato a Class_Initialize_Renamed. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Class_Initialize_Renamed()
        mContoCorrente = ""
        mCin = ""
        mIBAN = ""
        mBBAN = ""
        mCheckDigitIBAN = ""
        mPaese = "IT"
        mDivisore = 97
    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub


    Public Property Abi() As String
        Get
            Abi = mAbi
        End Get
        Set(ByVal Value As String)
            mAbi = Value
        End Set
    End Property


    Public Property Cab() As String
        Get
            Cab = mCab
        End Get
        Set(ByVal Value As String)
            mCab = Value
        End Set
    End Property


    Public Property ContoCorrente() As String
        Get
            ContoCorrente = mContoCorrente
        End Get
        Set(ByVal Value As String)
            mContoCorrente = Value
        End Set
    End Property


    Public Property Cin() As String
        Get
            Cin = mCin
        End Get
        Set(ByVal Value As String)
            mCin = Value
        End Set
    End Property


    Public Property IBAN() As String
        Get
            IBAN = mIBAN
        End Get
        Set(ByVal Value As String)
            mIBAN = Value
        End Set
    End Property


    Public Property BBAN() As String
        Get
            BBAN = mBBAN
        End Get
        Set(ByVal Value As String)
            mBBAN = Value
        End Set
    End Property


    Public Property CheckDigitIBAN() As String
        Get
            CheckDigitIBAN = mCheckDigitIBAN
        End Get
        Set(ByVal Value As String)
            mCheckDigitIBAN = Value
        End Set
    End Property


    Public Property Paese() As String
        Get
            Paese = mPaese
        End Get
        Set(ByVal Value As String)
            mPaese = Value
        End Set
    End Property


    Public Property Divisore() As Short
        Get
            Divisore = mDivisore
        End Get
        Set(ByVal Value As Short)
            mDivisore = Value
        End Set
    End Property
    Private Function NormalizzaDati(ByVal Codice As String, ByVal lunghezza As Short) As String
        Codice = Right(New String("0", lunghezza) & Trim(Codice), lunghezza)
        NormalizzaDati = Codice
    End Function

    Public Function NormalizzaContoCorrente(ByVal pContoCorrente As String) As String
        pContoCorrente = Replace(pContoCorrente, " ", "", 1, , CompareMethod.Text)
        NormalizzaContoCorrente = NormalizzaDati(pContoCorrente, L_CONTO)
    End Function

    Public Function VerificaCin(ByVal cinCode As String) As Boolean
        VerificaCin = (cinCode = CalcolaCin())
    End Function
    Public Function VerificaLetteraCin(ByVal CarattereDaControllare As String) As String
        '--------ANTONELLO DI CROCE-------------------SANDOKAN--------
        Dim stringadacontrollare As String
        Dim Stringachecontrolla As String
        Dim MyStr As String
        Dim esito As Integer

        stringadacontrollare = CarattereDaControllare
        Stringachecontrolla = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
        MyStr = stringadacontrollare


        If InStr(Stringachecontrolla, Mid(MyStr, 1, 1)) > 0 Then
            esito = 0
        Else
            esito = 1
        End If


        If esito = 1 Then
            Return 1
        Else
            Return 0
        End If
    End Function
    Public Function CalcolaCin() As String
        Const Numeri As String = "0123456789"
        Const Lettere As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ-. "
        Const Divisore As Short = 26
        Dim listaPari() As Object
        Dim ListaDispari() As Object
        Dim Codice, s As String
        Dim Somma As Short
        Dim k, i As Short
        'UPGRADE_WARNING: Array ha un nuovo comportamento. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        listaPari = New Object() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28}
        'UPGRADE_WARNING: Array ha un nuovo comportamento. Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        ListaDispari = New Object() {1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23, 27, 28, 26}
        mAbi = NormalizzaDati(mAbi, L_ABI)
        mCab = NormalizzaDati(mCab, L_CAB)
        ContoCorrente = NormalizzaContoCorrente(ContoCorrente)
        Codice = UCase(Abi & Cab & ContoCorrente)
        Somma = 0
        For k = 1 To (L_CONTO + L_ABI + L_CAB)
            s = Mid(Codice, k, 1)
            i = InStr(1, Numeri, s, CompareMethod.Text)
            If i = 0 Then
                i = InStr(1, Lettere, s, CompareMethod.Text)
            End If
            If i = 0 Then Exit Function
            i = i - 1
            If (k Mod 2) = 1 Then
                'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto ListaDispari(). Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Somma = Somma + CShort(ListaDispari(i))
            Else
                'UPGRADE_WARNING: Impossibile risolvere la proprietà predefinita dell'oggetto listaPari(). Fare clic per ulteriori informazioni: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Somma = Somma + CShort(listaPari(i))
            End If
        Next
        CalcolaCin = Mid(Lettere, Somma Mod Divisore + 1, 1)
    End Function

    Public Function CheckIBAN(Optional ByVal pIBAN As String = "") As Boolean
        Dim Codice As String
        Dim s As String
        Dim resto As Short
        If Not (pIBAN = "") Then
            Codice = pIBAN
        Else
            s = ""
            Codice = ""
            If BBAN <> "" Then
                Codice = BBAN
            End If
            If s = "" And Cin <> "" And Abi <> "" And Cab <> "" And ContoCorrente <> "" Then
                s = CalcolaCin()
                Codice = s & NormalizzaDati(mAbi, L_ABI) & NormalizzaDati(mCab, L_CAB) & NormalizzaContoCorrente(mContoCorrente)
            End If
            If Codice = "" Then
                CheckIBAN = False
                Exit Function
            End If
        End If
        Codice = NormalizzaIBAN(Codice)
        If Not CheckLength(Codice) Then
            CheckIBAN = False
            Exit Function
        End If
        Codice = Mid(Codice, 5) & Right(Codice, 4)
        Dim r() As String
        r = DivisioneIntera(AlfaToNumber(Codice), CStr(Divisore))
        resto = CShort(r(1))
        CheckIBAN = (resto = 1)
    End Function

    Public Function CalcolaBBAN() As String
        Dim Codice As String
        Dim s As String
        If Not (mIBAN = "") Then
            Codice = mIBAN
        Else
            s = mCin
            If s = "" Then
                s = CalcolaCin()
            End If
            Codice = s & NormalizzaDati(mAbi, L_ABI) & NormalizzaDati(mCab, L_CAB) & NormalizzaContoCorrente(mContoCorrente)
        End If
        CalcolaBBAN = Codice
    End Function

    Public Function CalcolaCheckIBAN(ByVal pPaese As String, ByVal pBBAN As String) As String
        CalcolaCheckIBAN = Mid(CalcolaIBAN(pPaese, pBBAN), 3, 2)
    End Function

    Public Function CalcolaIBAN(Optional ByVal pPaese As String = "", Optional ByVal pBBAN As String = "") As String
        Dim Codice, NumCode As String
        Dim r() As String
        If pPaese = "" Then pPaese = mPaese
        Codice = pBBAN
        If Codice = "" Then Codice = mBBAN
        If Codice = "" Then Codice = CalcolaBBAN()
        pBBAN = NormalizzaIBAN(Codice)
        Codice = pPaese & "00" & pBBAN
        Codice = Mid(Codice, 5) & Left(Codice, 4)
        NumCode = AlfaToNumber(Codice)
        r = DivisioneIntera(NumCode, CStr(Divisore))
        Dim resto As Short
        resto = CShort(r(1))
        resto = (Divisore + 1) - resto
        CalcolaIBAN = pPaese & Format(resto, "00") & pBBAN
    End Function

    Public Function NormalizzaIBAN(ByVal pCodice As String) As String
        Const alfanum As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim k, j As Short
        Dim Codice, s As String
        Codice = ""
        pCodice = UCase(pCodice)
        For k = 1 To Len(pCodice)
            s = Mid(pCodice, k, 1)
            j = InStr(1, alfanum, s, CompareMethod.Text)
            If j <> 0 Then Codice = Codice & s
        Next
        NormalizzaIBAN = Codice
    End Function

    Private Function CheckLength(ByVal pCodice As String) As Boolean
        CheckLength = True
    End Function

    Private Function AlfaToNumber(ByVal pCodice As String) As String
        Const alfachars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim s, Codice As String
        Dim k, i As Short
        Codice = ""
        For k = 1 To Len(pCodice)
            s = Mid(pCodice, k, 1)
            i = InStr(1, alfachars, s, CompareMethod.Text)
            If i <> 0 Then
                Codice = Codice & CStr(i + 9)
            Else
                Codice = Codice & s
            End If
        Next
        AlfaToNumber = Codice
    End Function

    Private Function DivisioneIntera(ByVal pDividendo As String, ByVal pDivisore As String) As String()
        Dim Intero As String
        Dim Resto, s As String
        Dim Divisore As Double
        Dim Dividendo As Double
        Dim x, Volte As Short
        Dim result(1) As String
        Divisore = CDbl(pDivisore)
        x = 0
        Intero = ""
        For x = 1 To Len(pDividendo)
            s = Mid(pDividendo, x, 1)
            Resto = Resto & s
            Dividendo = CDbl(Resto)
            Volte = 0
            While Dividendo >= Divisore
                Dividendo = Dividendo - Divisore
                Volte = Volte + 1
            End While
            Intero = Intero & Format(Volte, "0")
            Resto = Format(Dividendo, "0")
        Next
        result(1) = Resto
        result(0) = Intero
        While (Left(result(0), 1) = "0") And (Len(result(0)) > 1)
            result(0) = Mid(result(0), 2)
        End While
        If result(0) = "" Then
            result(0) = "0"
        End If
        DivisioneIntera = result 'System.Array.Copy(result)
    End Function
End Class