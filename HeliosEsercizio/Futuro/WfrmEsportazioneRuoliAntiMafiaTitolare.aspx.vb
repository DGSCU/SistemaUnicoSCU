Imports Logger.Data
Public Class WfrmEsportazioneRuoliAntiMafiaTitolare
    Inherits SmartPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOut dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOut dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If

        '--- IMPORTANTE!!! INSERIRE CONTROLLO PERMESSI PER ACCESSO MASCHERA E PER EVENTUALI DATI IN QUERY STRING
        'Controlli accesso/abilitazioni
        Dim _info As New clsRuoloAntimafia.InfoAdeguamentoAntimafia(Session("IdEnte"), Session("conn"), False)

        If Not _info.Trovato Then
            'errore nei dati
            Exit Sub
        End If
        Dim _clsR As New clsRuoloAntimafia
        Dim _dt As New DataSet
        Try

            _dt = _clsR.GetRuoliAntimafiaEsportazioneTitolare(Session("IdEnte"), _info.IdEnteFaseAntimafia, Session("conn"))
            _clsR.ExportCSV(_dt, Session("Utente") & "_RuoliAntiMafiaEnteTitolare_" & Format(DateTime.Now, "ddMMyyyyhhmmss") & ".csv", Page)
        Catch ex As Exception
            Log.Error(LogEvent.ANTIMAFIA_ESPORTAZIONE_ENTE_TITOLARE_ERRORE, "Errore", exception:=ex)
        End Try
    End Sub

End Class