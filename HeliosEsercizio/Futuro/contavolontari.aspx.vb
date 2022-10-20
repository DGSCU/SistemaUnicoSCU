Public Class contavolontari
    Inherits System.Web.UI.Page
#Region "Utility"


    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Session("JonListaIdVolontari") Is Nothing Then

            ReDim Session("JonListaIdVolontari")(0)
            Session("JonListaIdVolontari")(0) = ""

        End If
        If Page.IsPostBack = False Then

            If Request.Params("blocco") Is Nothing Then
                NumeroVolontariSelezionati.Text = ContaVol(Request.QueryString("strCheckStato"), Request.QueryString("strIdVol")).ToString
            Else    'se provengo dal refresh automatico
                NumeroVolontariSelezionati.Text = UBound(Session("JonListaIdVolontari")) + 1
            End If

        End If



    End Sub

    Function ContaVol(ByVal blnStato As Boolean, ByVal IdVol As Integer) As Integer
        Dim intX As Integer
        Dim strUltimoValore As String
        'controllo stato check


        'inserimento
        If blnStato = True Then
            'primo
            If Session("JonListaIdVolontari") Is Nothing Then
                ReDim Session("JonListaIdVolontari")(0)
                Session("JonListaIdVolontari")(0) = String.Empty
            End If
            If Session("JonListaIdVolontari")(0).ToString = String.Empty Then
                Session("JonListaIdVolontari")(0) = IdVol
            Else 'ridiminesiono il vettore
                ReDim Preserve Session("JonListaIdVolontari")(UBound(Session("JonListaIdVolontari")) + 1)
                Session("JonListaIdVolontari")(UBound(Session("JonListaIdVolontari"))) = IdVol
            End If
        Else 'cancellazione
            strUltimoValore = Session("JonListaIdVolontari")(UBound(Session("JonListaIdVolontari")))
            For intX = 0 To UBound(Session("JonListaIdVolontari"))
                If IdVol.ToString = Session("JonListaIdVolontari")(intX) Then
                    Session("JonListaIdVolontari")(intX) = strUltimoValore
                    ReDim Preserve Session("JonListaIdVolontari")(UBound(Session("JonListaIdVolontari")) - 1)
                    If UBound(Session("JonListaIdVolontari")) = -1 Then
                        Session("JonListaIdVolontari") = Nothing
                    End If
                    Exit For
                End If
            Next
        End If
        If Session("JonListaIdVolontari") Is Nothing Then
            ContaVol = 0
            Session("STRINGACHECKSEL") = ""
        Else
            Session("STRINGACHECKSEL") = ""
            For intX = 0 To UBound(Session("JonListaIdVolontari"))
                Session("STRINGACHECKSEL") = Session("STRINGACHECKSEL") & Session("JonListaIdVolontari")(intX) & "&"
            Next
            ContaVol = UBound(Session("JonListaIdVolontari")) + 1
        End If

        Return ContaVol

    End Function

End Class