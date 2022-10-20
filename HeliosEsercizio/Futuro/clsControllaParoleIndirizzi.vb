Public Class clsControllaParoleIndirizzi
    'SETTO LE VARIABILI IN INGRESSO
    Private _Termine As String

    'PASSAGGIO VALORI PRaMETRI IN INGRESSO
    Public Sub New(ByVal Termine As String)
        _Termine = Termine
    End Sub

    Private Function CheckParole() As String
        Dim strLocale As String = "calle, campiello, campo, carraia, carrarone, chiasso, circondario, circonvallazione, contrà, contrada, corso, diga, discesa, frazione, giardino, largo, località, lungoargine, lungolago, lungomare, maso, parallela, passeggiata, piazza, piazzale, piazzetta, rotonda, salita, strada, stradella, stradello, traversa, via, viale, vico, vicoletto, vicolo, vietta, viottolo, viuzza, viuzzo, piazza, della, delle, degli, del, santo, san, piazzale"
        If Len(_Termine) > 2 Then
            If InStr(LCase(strLocale), LCase(_Termine)) > 0 Then
                CheckParole = ""
            Else
                CheckParole = _Termine
            End If
        Else
            CheckParole = ""
        End If
        Return CheckParole
    End Function
    'PROPRIETA' CLASSE CHE RITORNA LA STRINGA EVENTUALE DA PASSARE ALLA QUERY
    Public ReadOnly Property CheckIndirizzo() As String
        Get
            Return CheckParole()
        End Get
    End Property
End Class
