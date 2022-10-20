Imports Logger.Data
Imports System.IO
Imports System.Data.SqlClient

Public Class WfrmInserimentoMassivoRuoliAntimafia
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

        If Session("Denominazione") = "" Then
            lgContornoPagina.InnerText = "Inserimento Massivo Ruoli Antimafia"
        Else
            lgContornoPagina.InnerText = "Inserimento Massivo Ruoli Antimafia - " & Session("Denominazione")
        End If

        If Page.IsPostBack Then
            If lblMessaggioErrore.Text = "" Then    'usato solo per sapere se l'elaborazione è andata a buon fine
                divrisultatoImportazione.Visible = True
                inizio.Visible = False
            End If
        Else
            'Controlli accesso/abilitazioni
            Dim _info As New clsRuoloAntimafia.InfoAdeguamentoAntimafia(Session("IdEnte"), Session("conn"), False)

            If Not _info.Trovato Then
                'errore nei dati, visualizzo solo un messaggio di errore
                lblMessaggio.Visible = True
                lblMessaggio.Text = "ERRORE NEI DATI, ENTE NON TROVATO"
                divPrincipale.Visible = False
                Exit Sub
            ElseIf Not _info.isEnteTitolare Then
                'la funzionalità non è abilitata per enti non titolari
                lblMessaggio.Visible = True
                lblMessaggio.Text = "FUNZIONALITA' NON DISPONIBILE PER ENTI NON TITOLARI"
                divPrincipale.Visible = False
                Exit Sub
            Else
                If Not _info.isAperto Then
                    divrisultatoImportazione.Visible = False
                    inizio.Visible = False
                    divChiudi.Visible = True
                    lblDisabilitato.Text = "Non e' possibile effettuare l'import dei dati; l'ente non e' attualmente abilitato."
                    Exit Sub
                End If
            End If

            hdsRisultatoElaborazione.Value = Guid.NewGuid().ToString
            hIdEnteFaseAntimafia.Value = _info.IdEnteFaseAntimafia.ToString
            divrisultatoImportazione.Visible = False
            inizio.Visible = True
            lblEsito.Visible = False
            CmdConferma.Visible = False
        End If
    End Sub

    Protected Sub CmdElabora_Click(sender As Object, e As EventArgs) Handles CmdElabora.Click
        Session(hdsRisultatoElaborazione.Value) = Nothing
        If fupFile.HasFile Then
            Dim _clsAM As New clsRuoloAntimafia
            Dim _ret As String = ""
            Dim risultatoElaborazione = New clsRuoloAntimafia.ControlloCSV
            risultatoElaborazione.Nomefile = fupFile.FileName
            Try
                _ret = _clsAM.ImportMassivoRuoliAntiMafia(fupFile.PostedFile.InputStream, Session("IdEnte"), risultatoElaborazione, Session("conn"))
            Catch ex As Exception
                Log.Error(LogEvent.ANTIMAFIA_IMPORTAZIONE_MASSIVA_ERRORE, "Errore in elaborazione file", exception:=ex)
                _ret = ex.Message
            End Try

            lblTotali.Text = "Sono state inviate " & risultatoElaborazione.RigheCSV.Count & " righe. " & _
                             risultatoElaborazione.RigheCSV.Count - risultatoElaborazione.ErroriCSV.Count & " con esito positivo. " & risultatoElaborazione.ErroriCSV.Count & " con esito negativo."
            If _ret = "" Then
                Session(hdsRisultatoElaborazione.Value) = risultatoElaborazione
                dgRisultatoImportazione.DataSource = GeneraDatasetErrori(risultatoElaborazione)
                dgRisultatoImportazione.DataBind()
                inizio.Visible = False
                divrisultatoImportazione.Visible = True
                If risultatoElaborazione.ErroriCSV.Count = 0 Then
                    CmdConferma.Visible = True
                    lblEsito.Visible = True
                    divgriglia.Visible = False
                Else
                    CmdConferma.Visible = False
                    lblEsito.Visible = False
                    divgriglia.Visible = True
                End If
                lblMessaggioErrore.Text = ""
            Else
                lblMessaggioErrore.CssClass = "msgErroreBig"
                lblMessaggioErrore.Text = _ret
                divrisultatoImportazione.Visible = False
                inizio.Visible = True
                lblEsito.Visible = False
                CmdConferma.Visible = False
            End If
        Else
            lblMessaggioErrore.CssClass = "msgErroreBig"
            lblMessaggioErrore.Text = "Selezionare il file da importare"
            divrisultatoImportazione.Visible = False
            inizio.Visible = True
            lblEsito.Visible = False
            CmdConferma.Visible = False
        End If
    End Sub

    Function GeneraDatasetErrori(risultato As clsRuoloAntimafia.ControlloCSV) As DataSet
        Dim _ret As New DataSet

        Dim Err As DataTable = New DataTable("Errors")
        Err.Columns.Add("N.ro riga")
        Err.Columns.Add("Errori")
        Err.Columns.Add("CodiceFiscaleEnte")
        Err.Columns.Add("CodiceFiscale")
        Err.Columns.Add("RuoloAntimafia")


        For Each e In risultato.ErroriCSV

            Dim errori As String
            For Each ec In e.Errori
                If ec.NomeCampo = "Errore generale" Then
                    errori += ec.Errore + "<br>"
                Else
                    errori += "Campo <strong>" + ec.NomeCampo + "</strong> (=""" + ec.Valore + """) " + ec.Errore + "<br>"
                End If
            Next

            'trovo la riga corrispondente dei campi decodificati
            Dim _rVal As clsRuoloAntimafia.RigaRuoloDaInserire = risultato.RigheCSV.Find(Function(r) r.Riga = e.Riga)

            'La riga (che nella struttura dati è ad indice iniziale 0) è incrementata di 1 per riflettere il numero di riga del file .csv
            Err.Rows.Add(e.Riga + 1, errori, _rVal.EnteCSV, _rVal.CodiceFiscale, _rVal.RuoloAntimafiaCSV)
            errori = ""
        Next

        _ret.Tables.Add(Err)

        Return _ret
    End Function

    Protected Sub lnkScarica_Click(sender As Object, e As EventArgs) Handles lnkScarica.Click
        Dim risultatoelaborazione As clsRuoloAntimafia.ControlloCSV = Session(hdsRisultatoElaborazione.Value)
        If risultatoelaborazione Is Nothing Or risultatoelaborazione.File Is Nothing Or String.IsNullOrEmpty(risultatoelaborazione.Nomefile) Then
            lblEsito.Text = "Errore nei dati."
            lblEsito.Visible = True
            Return
        End If
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" + risultatoelaborazione.Nomefile)
        Response.ContentType = "text/csv"
        Response.BinaryWrite(risultatoelaborazione.File)
        Response.End()
        Response.Flush()
    End Sub

    Protected Sub btnChiudi_Click(sender As Object, e As EventArgs) Handles btnChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dgRisultatoImportazione_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoImportazione.PageIndexChanged
        dgRisultatoImportazione.CurrentPageIndex = e.NewPageIndex
        Dim risultatoelaborazione As clsRuoloAntimafia.ControlloCSV = Session(hdsRisultatoElaborazione.Value)
        dgRisultatoImportazione.DataSource = GeneraDatasetErrori(risultatoelaborazione)
        dgRisultatoImportazione.DataBind()
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("Wfrminserimentomassivoruoliantimafia.aspx")
    End Sub

    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        Dim risultatoelaborazione As clsRuoloAntimafia.ControlloCSV = Session(hdsRisultatoElaborazione.Value)
        If risultatoelaborazione Is Nothing Or risultatoelaborazione.ErroriCSV.Count > 0 Then
            lblEsito.Text = "Errore nei dati."
            lblEsito.Visible = True
            Return
        End If

        Dim MyCommand = New SqlClient.SqlCommand
        MyCommand.Connection = Session("conn")
        CmdConferma.Visible = False
        Try
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            ImportaRuoliAntimafia(risultatoelaborazione, MyCommand)

            MyTransaction.Commit()

        Catch exc As Exception

            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            swErr = True
            Log.Error(LogEvent.ANTIMAFIA_IMPORTAZIONE_MASSIVA_ERRORE, "Errore in inserimento", exception:=exc)
        End Try

        MyCommand.Dispose()

        If swErr = False Then
            'Esito positivo
            lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."
            lblEsito.Visible = True
            '            CmdChiudi_Click.ImageUrl = "images/chiudi.jpg"
        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
            'imgAnnulla.ImageUrl = "images/annulla.jpg"
        End If

    End Sub

    Sub ImportaRuoliAntimafia(risultatoElaborazione As clsRuoloAntimafia.ControlloCSV, myCommand As SqlClient.SqlCommand)

        'cancello totalmente i vecchi ruoli antimafia dal mio ente e dagli enti in accordo
        Dim strSql = "delete from RuoliAntimafia where idente in"
        strSql += "(select " + Session("IdEnte") + " as idente union"
        strSql += " select idente from enti e inner join entirelazioni er on er.IDEnteFiglio=e.idente"
        strSql += " where er.IDEntePadre= " + Session("IdEnte") + " And er.datafinevalidità Is null) and IdEnteFaseAntimafia=" & hIdEnteFaseAntimafia.Value

        myCommand.CommandText = strSql
        myCommand.ExecuteNonQuery()

        'ciclo su tutte le righe da inserire e le inserisco
        strSql = "INSERT INTO [dbo].[RuoliAntimafia] ([IdEnte] ,[CodiceFiscale] ,[Cognome] ,[Nome] ,[IdElencoRuoliAntimafia] ,[DataNascita] ,[IdComuneNascita] ,[IdComuneResidenza] ,[IndirizzoResidenza] ,[NumeroCivicoResidenza] ,[CAPResidenza] ,[Telefono] ,[PEC] ,[Email],[IdEnteFaseAntimafia],[UltimaEsportazioneDati])"
        strSql += " VALUES (@IdEnte,@CodiceFiscale,@Cognome,@Nome,@IdElencoRuoliAntimafia,@DataNascita,@IdComuneNascita,@IdComuneResidenza,@IndirizzoResidenza,@NumeroCivicoResidenza,@CAPResidenza,@Telefono,@PEC,@Email,@IdEnteFaseAntimafia,case @UltimaEsportazioneDati when '01-01-1900' then NULL else @UltimaEsportazioneDati end)"
        myCommand.CommandText = strSql
        For Each r As clsRuoloAntimafia.RigaRuoloDaInserire In risultatoElaborazione.RigheCSV

            myCommand.Parameters.AddWithValue("@IdEnte", r.IdEnte)
            myCommand.Parameters.AddWithValue("@CodiceFiscale", r.CodiceFiscale)
            myCommand.Parameters.AddWithValue("@Cognome", r.Cognome)
            myCommand.Parameters.AddWithValue("@Nome", r.Nome)
            myCommand.Parameters.AddWithValue("@IdElencoRuoliAntimafia", r.IdElencoRuoliAntimafia)
            myCommand.Parameters.AddWithValue("@DataNascita", r.DataNascita)
            myCommand.Parameters.AddWithValue("@IdComuneNascita", r.IdComuneNascita)
            myCommand.Parameters.AddWithValue("@IdComuneResidenza", r.IdComuneResidenza)
            myCommand.Parameters.AddWithValue("@IndirizzoResidenza", r.IndirizzoResidenza)
            myCommand.Parameters.AddWithValue("@NumeroCivicoResidenza", r.NumeroCivicoResidenza)
            myCommand.Parameters.AddWithValue("@CAPResidenza", r.CAPResidenza)
            myCommand.Parameters.AddWithValue("@Telefono", r.Telefono.ToString)
            myCommand.Parameters.AddWithValue("@PEC", r.PEC)
            myCommand.Parameters.AddWithValue("@Email", r.Email.ToString)
            myCommand.Parameters.AddWithValue("@IdEnteFaseAntimafia", hIdEnteFaseAntimafia.Value)
            myCommand.Parameters.AddWithValue("@UltimaEsportazioneDati", r.UltimaEsportazioneDati)

            myCommand.ExecuteNonQuery()
            myCommand.Parameters.Clear()
        Next

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Public Sub RuoliAntimafiaAmmessi()
        'ROUTINE CHE CARICA il div divRuoliAntimafiaAmmessi

        Dim _elencoRuoliAntimafiaAmmessi As clsRuoloAntimafia.LElencoRuoliAntiMafia = New clsRuoloAntimafia.LElencoRuoliAntiMafia(Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        For Each _r As clsRuoloAntimafia.ElencoRuoliAntimafia In _elencoRuoliAntimafiaAmmessi.ListaElencoRuoloAntiMafia
            Response.Write("<li><strong>" & _r.Denominazione & "</strong></li>")
        Next
        Response.Write("</ul>")
        Response.Write("</fieldset>")

    End Sub

End Class