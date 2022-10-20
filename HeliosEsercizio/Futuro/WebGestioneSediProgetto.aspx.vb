Imports System.Data.SqlClient

Public Class WebGestioneSediProgetto
    Inherits System.Web.UI.Page
    Dim dataset As DataSet
    Dim dataReader As SqlClient.SqlDataReader
    Dim sqlCommand As SqlClient.SqlCommand
    Dim query As String
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_CODICE_REGIONE As Integer = 0
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_SEDE_FISICA As Integer = 1
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_INDIRIZZO As Integer = 2
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_COMUNE As Integer = 3
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_TELEFONO As Integer = 4
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_SEDE_ATTUAZIONE As Integer = 5
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_STATO_ENTE_SEDE As Integer = 6
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_NUMERO_PROGETTI_ATTIVI As Integer = 7
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_ID_SEDE As Integer = 8
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_CODICE_SEDE As Integer = 9
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_NUMERO_VOLONTARI As Integer = 10
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_N_VOL As Integer = 11
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_ID_ATTIVITA_ENTE_SEDE As Integer = 12
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_ATTIVA As Integer = 13
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_DATA_INSERIMENTO As Integer = 14
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_PRESENZA_SANZIONE As Integer = 15
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_PRESENZA_VERIFICA As Integer = 16
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_STATO_ENTE As Integer = 17
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_MODIFICA_SEDE As Integer = 18
    Private Const INDEX_DATAGRID_DGRISULTATORICERCA_RIMUOVI_SEDE As Integer = 19

    Dim ROSSO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FF9966")
    Dim GIALLO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FFFF99")
    Dim VERDE As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#99FF99")
    Dim NERO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#000000")
    Dim BIANCO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")
    Dim GRIGIO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#CCCCCC")





#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        ChiudiDataReader(dataReader)
        If Request.QueryString("IdAttivita") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1
            If Not ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
        End If

        If IsPostBack = False Then
            Dim Modifica As Integer
            If Session("TipoUtente") = "E" Then
                Modifica = CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
                If Modifica = "0" Then txtModifica.Value = "False"
            Else
                Modifica = CInt(Request.QueryString("Modifica"))
            End If

            query = "select attività.idattività,attività.titolo,attività.esteroue,statiattività.statoattività,statiattività.defaultstato as defaultAttività," & _
            " case isnull(convert(smallint,statibandiAttività.defaultstato),-1)when -1 then '0'" & _
            " else convert(smallint,statibandiAttività.defaultstato)  end as defaultBando " & _
            " from attività " & _
            " inner join statiattività on (statiattività.idstatoattività=attività.idstatoattività) " & _
            " left join bandiAttività  on (attività.idbandoattività=bandiAttività.idbandoattività) " & _
            " left join statibandiAttività  " & _
            " on (bandiAttività.idstatobandoattività=statibandiAttività.idstatobandoattività) " & _
            " where attività.idattività=" & Request.QueryString("IdAttivita") & ""
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()


            'progetto EsteroUE
            If dataReader("esteroue") = True Then
                imgAggiungi.Visible = True
                chkSediNonIscritte.Visible = True
            End If


            If Modifica = "0" Then
                If dataReader("defaultAttività") = False And dataReader("defaultBando") = False Then
                    txtModifica.Value = "False"
                    lblmessaggio.Text = "Impossibile modificare le sedi perchè il progetto è Attivo o in Valutazione."
                End If
            End If
            lblProgetto.Text = dataReader("Titolo")
            ChiudiDataReader(dataReader)


            query = "Select idattivitàentesedeattuazione  from attivitàentisediattuazione where idattività=" & CInt(Request.QueryString("IdAttivita")) & ""
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            If dataReader.HasRows = True Then
                chkSediProgetto.Checked = True
            Else
                chkSediProgetto.Checked = False
            End If
            ChiudiDataReader(dataReader)

            'se coprogrammante non coprogettante allora blocco chksediprogetto (visualizza sempre solo le sedi selezionate)
            Dim strsqlstato As String
            Dim dtrStato As System.Data.SqlClient.SqlDataReader
            strsqlstato = "select identepresentante from attività where identepresentante = " & Session("IdEnte") & " and idattività = " & CInt(Request.QueryString("IdAttivita")) & " union select idente from AttivitàEntiCoprogettazione where idente = " & Session("IdEnte") & " and idattività = " & CInt(Request.QueryString("IdAttivita"))

            'controllo se ente proponente o coprogettante
            dtrStato = ClsServer.CreaDatareader(strsqlstato, Session("conn"))
            dtrStato.Read()
            If dtrStato.HasRows = False Then
                'ente non proponente non coprogettante quindi coprogrammante
                chkSediProgetto.Checked = True
                chkSediProgetto.Enabled = False
                dgRisultatoRicerca.Columns(18).Visible = False
                dgRisultatoRicerca.Columns(19).Visible = False
            End If
            ChiudiDataReader(dtrStato)

            'se Regione e competenza progetto 22 (nazionale) allora blocco modifiche

            If Session("TipoUtente") = "R" Then
                strsqlstato = "select idregionecompetenza from attività where idattività = " & CInt(Request.QueryString("IdAttivita")) & " "

                dtrStato = ClsServer.CreaDatareader(strsqlstato, Session("conn"))
                dtrStato.Read()
                If dtrStato("idregionecompetenza") = 22 Then
                    'regione che consulta progetto competenza nazionale
                    chkSediProgetto.Checked = True
                    chkSediProgetto.Enabled = False
                    dgRisultatoRicerca.Columns(18).Visible = False
                    dgRisultatoRicerca.Columns(19).Visible = False
                End If
                ChiudiDataReader(dtrStato)
            End If
           

            lblmessaggio.Text = ""
            If Not (ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))) Then
                lblPresenzaSanzione.Visible = False
                ddlSegnalazioneSanzione.Visible = False

            End If
            controllacolonnepulsanteGriglia()
            RicercaSedi()

        End If

    End Sub

    Private Sub controllacolonnepulsanteGriglia()
        'SedeSottopostaVerifica dataReader era aperto òlo devo chiudere
        'getdate() between  DataInizioVolontari and DataFineVolontari and 
        Dim strsql As String
        If Request.QueryString("Nazionale") > 10 Then

        End If
        strsql = "SELECT isnull(bando.DataFineVolontari,GETDATE()) DataFineVolontari, GETDATE() AS DataOdierna FROM bando " & _
            "INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando " & _
            "INNER JOIN attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività " & _
            "where  idattività=" & CInt(Request.QueryString("IdAttivita")) & ""
        dataReader = ClsServer.CreaDatareader(strsql, Session("conn"))
        dataReader.Read()
        If dataReader.HasRows = True Then
            If dataReader("DataOdierna") < dataReader("DataFineVolontari") Then
                'dgRisultatoRicerca.Columns(17).Visible = False
                dgRisultatoRicerca.Columns(19).Visible = False
                'Else
                '    dgRisultatoRicerca.Columns(17).Visible = True
                '    dgRisultatoRicerca.Columns(18).Visible = True
            End If
       
        End If

            ''    dgRisultatoRicerca.Columns(17).Visible = True
            ''    dgRisultatoRicerca.Columns(18).Visible = True
            ''Else
            ''    dgRisultatoRicerca.Columns(17).Visible = False
            ''    dgRisultatoRicerca.Columns(18).Visible = False
            ''    'If (dataReader("DataOdierna") > dataReader("DataInizioAttività")) And (dataReader("DataOdierna") < dataReader("DataFineAttività")) Then
            ''    '    dgRisultatoRicerca.Columns(17).Visible = True
            ''    '    dgRisultatoRicerca.Columns(18).Visible = True
            ''    'Else
            ''    '    dgRisultatoRicerca.Columns(17).Visible = False
            ''    '    dgRisultatoRicerca.Columns(18).Visible = False
            ''    'End If
        'End If
        ChiudiDataReader(dataReader)

    End Sub


    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        lblmessaggio.Text = String.Empty
        If dgRisultatoRicerca.CurrentPageIndex > 0 Then
            dgRisultatoRicerca.CurrentPageIndex = 0
        End If
        If txtCodiceSede.Text <> "" Then
            Dim CodiceSede As Integer
            CodiceSede = Integer.TryParse(txtCodiceSede.Text.Trim, CodiceSede)
            If CodiceSede = False Then
                lblmessaggio.Text = "Il Codice Sede puo' contenere solo numeri."
                lblmessaggio.Visible = True
                Exit Sub
            End If
        End If

        RicercaSedi()
    End Sub


    Private Sub RicercaSedi()

       


        'modificato il 03/07/2018 è stato sostituita l'apertua della popup sedesanzionata non la dicitura SI se la sde è sanzionata
        If Session("TipoUtente") = "E" Then
            'Data inserimento
            dgRisultatoRicerca.Columns(INDEX_DATAGRID_DGRISULTATORICERCA_DATA_INSERIMENTO).Visible = False
        End If

        If Request.QueryString("EnteCapoFila") = "True" Then


            '   query = " Select *, " & _
            '" Case isnull(Segnalazione,0) When 0 then 'No' When 1 then '<img src=""images/Anomalie.bmp"" onclick=""VisualizzaSanzione('+ convert(varchar, IDEnteSedeAttuazione) + ','+ convert(varchar, IDEnte) + ')"" STYLE=cursor:hand title=""Sanzione"" >' End as PresenzaSanzione,  " & _
            '" Case isnull(SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica " & _
            '" ,Case isnull(NumeroPostifami,0) when 0 THEN convert(varchar,NumeroPosti) else convert(varchar,NumeroPosti) + ' (Fami:' + convert(varchar,NumeroPostifami) + ')' end totNumeroPosti   " & _
            '" from VW_ELENCO_SEDI_PROGETTO_FUTURO " & _
            '" WHERE idattività=" & CInt(Request.QueryString("IdAttivita"))
            query = " Select *, " & _
                     " Case isnull(Segnalazione,0) When 0 then 'No' When 1 then 'Si' End as PresenzaSanzione,  " & _
                     " Case isnull(SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica " & _
                     " ,Case isnull(NumeroPostifami,0) when 0 THEN convert(varchar,NumeroPosti) else convert(varchar,NumeroPosti) + ' (Fami:' + convert(varchar,NumeroPostifami) + ')' end totNumeroPosti   " & _
                     " from VW_ELENCO_SEDI_PROGETTO_FUTURO " & _
                     " WHERE idattività=" & CInt(Request.QueryString("IdAttivita"))
        Else

            Dim strsqlstato As String
            Dim dtrStato As System.Data.SqlClient.SqlDataReader
            strsqlstato = "select idente from AttivitàEntiCoprogettazione where idente = " & Session("IdEnte") & " and idattività = " & CInt(Request.QueryString("IdAttivita"))

            'controllo se ente coprogettante
            dtrStato = ClsServer.CreaDatareader(strsqlstato, Session("conn"))
            dtrStato.Read()
            If dtrStato.HasRows = False Then
                'ente non coprogettante quindi coprogrammante
                query = " Select *, " & _
                    " Case isnull(Segnalazione,0) When 0 then 'No' When 1 then 'Si' End as PresenzaSanzione,  " & _
                    " Case isnull(SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica " & _
                    " ,Case isnull(NumeroPostifami,0) when 0 THEN convert(varchar,NumeroPosti) else convert(varchar,NumeroPosti) + ' (Fami:' + convert(varchar,NumeroPostifami) + ')' end totNumeroPosti   " & _
                    " from VW_ELENCO_SEDI_PROGETTO_ENTI_COPROGRAMMANTI_FUTURO " & _
                    " WHERE idattività=" & CInt(Request.QueryString("IdAttivita"))
            Else
                query = " Select *, " & _
                    " Case isnull(Segnalazione,0) When 0 then 'No' When 1 then 'Si' End as PresenzaSanzione,  " & _
                    " Case isnull(SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica " & _
                    " ,Case isnull(NumeroPostifami,0) when 0 THEN convert(varchar,NumeroPosti) else convert(varchar,NumeroPosti) + ' (Fami:' + convert(varchar,NumeroPostifami) + ')' end totNumeroPosti   " & _
                    " from VW_ELENCO_SEDI_PROGETTO_ENTI_COPROGETTANTI_FUTURO " & _
                    " WHERE identecoprogettante=" & Session("IdEnte") & " and idattività=" & CInt(Request.QueryString("IdAttivita"))
            End If
            ChiudiDataReader(dtrStato)

        End If

        If Session("TipoUtente") = "E" And CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn"))) = True Then

            query = query & " and attiva=1  "
        End If
        If Trim(txtSedefisica.Text) <> "" Then 'AMILCARE
            query = query & " and CodiceRegione = '" & Replace(txtSedefisica.Text, "'", "''") & "'"
        End If
        If Trim(txtSedeAttuaz.Text) <> "" Then
            query = query & " and sedeattuazione like '" & Replace(txtSedeAttuaz.Text, "'", "''") & "%'"
        End If
        If chkSediProgetto.Checked = True Then
            query = query & " and idatt <>0"
        End If
        If chkSediNonIscritte.Checked = True Then
            query = query & " and noaccreditamento=1"
            ' query = query & " and attiva=1"
        End If
        If chkSediNonIscritte.Visible = False Then
            query = query & " and ISNULL(noaccreditamento,0)=0"
        End If
        If Trim(txtRegione.Text) <> vbNullString Then
            query = query & " and Regione LIKE '" & Replace(txtRegione.Text, "'", "''") & "%'"
        End If
        If Trim(txtprovincia.Text) <> vbNullString Then
            query = query & " and Provincia LIKE '" & Replace(txtprovincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            query = query & " and Comune LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodiceSede.Text) <> vbNullString Then
            query = query & " and identesedeattuazione ='" & txtCodiceSede.Text & "'"
        End If
        'agg. da sc il 07/07/2011 combo su segnalazione della sede
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            query = query & " and isnull(Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If
        dataset = ClsServer.DataSetGenerico(query, Session("conn"))
        Session("SessionSediProgetto") = dataset
        CaricaDataGrid(dgRisultatoRicerca)

        ControllaPulsanti()

    End Sub

    Sub ControllaPulsanti()
        Dim Indice As DataGridItem
        For Each Indice In dgRisultatoRicerca.Items
            'If dgRisultatoRicerca.Items(Indice.ItemIndex).Cells(INDEX_DATAGRID_DGRISULTATORICERCA_N_VOL).Text = "&nbsp;" Then
            '    dgRisultatoRicerca.Items(Indice.ItemIndex).Cells(INDEX_DATAGRID_DGRISULTATORICERCA_ID_ATTIVITA_ENTE_SEDE).Text = "&nbsp;"
            'End If
            If dgRisultatoRicerca.Items(Indice.ItemIndex).Cells(INDEX_DATAGRID_DGRISULTATORICERCA_MODIFICA_SEDE).Text = "NO" Then
                dgRisultatoRicerca.Items(Indice.ItemIndex).Cells(INDEX_DATAGRID_DGRISULTATORICERCA_RIMUOVI_SEDE).Text = ""
            End If
        Next
    End Sub

    Private Sub RicercaSedi_1()

       

        If Session("TipoUtente") = "E" Then
            dgRisultatoRicerca.Columns(19).Visible = False
        End If


        Dim strappo As String
        Dim strappo2 As String

        strappo = " <input id=' + '" & """" & "txt'" & "+ Convert(varchar, entisediattuazioni.identesedeattuazione)+ '" & """' + '  type=""text"" readOnly=""true"" style=""BACKGROUND-COLOR: Gainsboro""" & _
        " value='+ case isnull(attivitàentisediattuazione.idattivitàEntesedeAttuazione,-1)when -1 " & _
        " then '""""' else '""' + convert(varchar,(select NumeroPostiNoVittoNoAlloggio " & _
        " + NumeroPostiVittoAlloggio + NumeroPostiVitto " & _
        " from attivitàentisediattuazione where idattività=attività.idattività  and identesedeattuazione=entisediattuazioni.identesedeattuazione " & _
        " ))+ '""' end + ';>' as text,statientisedi.statoentesede,"

        strappo2 = "'<img src=""images/sedi_small.png"">' as img ,statientisedi.attiva,"

        strappo = strappo & "' <img src=""images/valida_small.png"" style=""CURSOR: hand"" title=""Aggiungi Sede""  onclick=javascript:apri('" & _
                            " + convert(varchar,entisedi.identeSede) + ',' + convert(varchar,entisediattuazioni.identesedeattuazione)+ ',' + convert(varchar,attività.idattività)+ '," & _
                            "'+ case isnull(attivitàentisediattuazione.idattivitàEnteSedeAttuazione,-1)" & _
                            " when -1 then '0' else convert(varchar,attivitàentisediattuazione.idattivitàEnteSedeAttuazione) end + '," & _
                            "'+ 'txt'+ Convert(varchar, entisediattuazioni.identesedeattuazione) + ',' + case statientisedi.Attiva when '1' then '1' else case statientisedi.DaAccreditare when '1' then '2' else case statientisedi.idstatoentesede when '3' then '3' else '0' end  end end+ ') " & _
                            "  title=""Aggiungi Sede"" >"

        query = " select '" & strappo & "' as prova," & strappo2 & "" & _
                 " entisedi.identeSede,entisedi.Denominazione as sedeFisica,entisedi.Indirizzo,entisedi.Telefono," & _
                 " comuni.denominazione as Comune,comuni.cap,Provincie.provincia,entisediattuazioni.identesedeattuazione," & _
                 " entisediattuazioni.denominazione as sedeAttuazione, " & _
                 " case isnull(attivitàentisediattuazione.idattivitàentesedeattuazione,-1)when -1 then 0 else attivitàentisediattuazione.idattivitàentesedeattuazione end as idatt,attività.idattività " & _
                 " ,(select count(*) from attivitàentisediattuazione where idattività=attività.idattività " & _
                 " and identesedeattuazione=entisediattuazioni.identesedeattuazione) as nProgAtt, entisedi.DataCreazioneRecord " & _
                 " from entisedi " & _
                 " inner join comuni on(comuni.idcomune=entisedi.idcomune) " & _
                 " inner join Provincie on(Provincie.idProvincia=Comuni.idProvincia)" & _
                 " INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione " & _
                 " inner join entisediattuazioni on (entisediattuazioni.identesede=entisedi.identesede) " & _
                 " inner join statientisedi on (statientisedi.idstatoentesede=entisediattuazioni.idstatoentesede) " & _
                 " inner join attività on (attività.identePresentante=entisedi.idente) " & _
                 " left join attivitàentisediattuazione on  (attivitàentisediattuazione.idattività=attività.idattività  " & _
                 " and attivitàentisediattuazione.identesedeattuazione=entisediattuazioni.identesedeattuazione ) " & _
                 " where(entisedi.idente=" & CInt(Session("IdEnte")) & " And attività.idattività=" & CInt(Request.QueryString("IdAttivita")) & ")"
        If Session("TipoUtente") = "E" Then
            query = query & " and statientisedi.attiva=1"
        End If
        If Trim(txtSedefisica.Text) <> "" Then
            query = query & " and entisedi.Denominazione like '" & Replace(txtSedefisica.Text, "'", "''") & "%'"
        End If
        If Trim(txtSedeAttuaz.Text) <> "" Then
            query = query & " and entisediattuazioni.Denominazione like '" & Replace(txtSedeAttuaz.Text, "'", "''") & "%'"
        End If
        If chkSediProgetto.Checked = True Then
            query = query & " and not attivitàentisediattuazione.idattivitàentesedeattuazione is null "
        End If
        If Trim(txtRegione.Text) <> vbNullString Then
            query = query & " and Regioni.Regione LIKE '" & Replace(txtRegione.Text, "'", "''") & "%'"
        End If
        If Trim(txtprovincia.Text) <> vbNullString Then
            query = query & " and Provincie.Provincia LIKE '" & Replace(txtprovincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            query = query & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodiceSede.Text) <> vbNullString Then
            query = query & " and entisediattuazioni.identesedeattuazione ='" & txtCodiceSede.Text & "'"
        End If

        query = query & "union "

        strappo = " <input id=' + '" & """" & "txt'" & "+ Convert(varchar, entisediattuazioni.identesedeattuazione)+ '" & """' + '  type=""text"" readOnly=""true"" style=""BACKGROUND-COLOR: Gainsboro""" & _
                  " value='+ case isnull(attivitàentisediattuazione.idattivitàEntesedeAttuazione,-1)when -1 " & _
                  " then '""""' else '""' + convert(varchar,(select NumeroPostiNoVittoNoAlloggio " & _
                  " + NumeroPostiVittoAlloggio + NumeroPostiVitto " & _
                  " from attivitàentisediattuazione where  idattività=attività.idattività  and identesedeattuazione=entisediattuazioni.identesedeattuazione " & _
                  " ))+ '""' end + ';>' as text,statientisedi.statoentesede, "

        strappo2 = "'<img src=""images/ente_small.png"" >' as img ,statientisedi.attiva,"

        strappo = strappo & "' <img src=""images/valida_small.png"" style=""CURSOR: hand"" title=""Aggiungi Sede"" onclick=javascript:apri('" & _
                            " + convert(varchar,entisedi.identeSede) + ',' + convert(varchar,entisediattuazioni.identesedeattuazione)+ ',' + convert(varchar,attività.idattività)+ '," & _
                            "'+ case isnull(attivitàentisediattuazione.idattivitàEnteSedeAttuazione,-1)" & _
                            " when -1 then '0' else convert(varchar,attivitàentisediattuazione.idattivitàEnteSedeAttuazione) end + '," & _
                            "'+ 'txt'+ Convert(varchar, entisediattuazioni.identesedeattuazione) + ',' + case statientisedi.Attiva when '1' then '1' else case statientisedi.DaAccreditare when '1' then '2' else case statientisedi.idstatoentesede when '3' then '3' else '0' end end end + ') " & _
                            "  title=""Aggiungi Sede""> "

        query = query & " select '" & strappo & "' as prova," & strappo2 & " " & _
                          " entisedi.identeSede,entisedi.Denominazione as sedeFisica,entisedi.Indirizzo,entisedi.Telefono," & _
                          " comuni.denominazione as Comune,comuni.cap,Provincie.provincia,entisediattuazioni.identesedeattuazione," & _
                          " entisediattuazioni.denominazione as sedeAttuazione, " & _
                          " case isnull(attivitàentisediattuazione.idattivitàentesedeattuazione,-1)when -1 then 0 else attivitàentisediattuazione.idattivitàentesedeattuazione end as idatt,attività.idattività " & _
                          " ,(select count(*) from attivitàentisediattuazione where idattività=attività.idattività " & _
                          " and identesedeattuazione=entisediattuazioni.identesedeattuazione) as nProgAtt,entisedi.DataCreazioneRecord " & _
                          " from enti " & _
                          " inner join entisedi on(entisedi.idente=enti.idEnte)  " & _
                          " inner join entisediattuazioni on (entisediattuazioni.identesede=entisedi.identesede) " & _
                          " inner join statientisedi on (statientisedi.idstatoentesede=entisediattuazioni.idstatoentesede) " & _
                          " inner join entirelazioni on (entirelazioni.identefiglio=enti.idente)  " & _
                          " left join associaentirelazionisediattuazioni on (entisediattuazioni.identesedeattuazione=associaentirelazionisediattuazioni.identesedeattuazione  " & _
                          " and entirelazioni.identerelazione=associaentirelazionisediattuazioni.identerelazione) " & _
                          " inner join attività on (attività.identePresentante=entirelazioni.identepadre) " & _
                          " left join attivitàentisediattuazione on  (attivitàentisediattuazione.idattività=attività.idattività  " & _
                          " and attivitàentisediattuazione.identesedeattuazione=entisediattuazioni.identesedeattuazione ) " & _
                          " inner join comuni on (entisedi.idcomune=Comuni.idcomune)" & _
                          " inner join Provincie on (provincie.idprovincia=Comuni.idProvincia) " & _
                          " INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione " & _
                          " where(entirelazioni.identePadre=" & CInt(Session("IdEnte")) & " And attività.idattività=" & CInt(Request.QueryString("IdAttivita")) & ")"
        If Session("TipoUtente") = "E" Then
            query = query & " and statientisedi.attiva=1"
        End If
        If Trim(txtSedefisica.Text) <> "" Then
            query = query & " and enti.Denominazione like '" & Replace(txtSedefisica.Text, "'", "''") & "%'"
        End If
        If Trim(txtSedeAttuaz.Text) <> "" Then
            query = query & " and entisediattuazioni.Denominazione like '" & Replace(txtSedeAttuaz.Text, "'", "''") & "%'"
        End If
        If chkSediProgetto.Checked = True Then
            query = query & " and not attivitàentisediattuazione.idattivitàentesedeattuazione is null "
        End If
        If Trim(txtRegione.Text) <> vbNullString Then
            query = query & " and Regioni.Regione LIKE '" & Replace(txtRegione.Text, "'", "''") & "%'"
        End If
        If Trim(txtprovincia.Text) <> vbNullString Then
            query = query & " and Provincie.Provincia LIKE '" & Replace(txtprovincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            query = query & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodiceSede.Text) <> vbNullString Then
            query = query & " and entisediattuazioni.identesedeattuazione ='" & txtCodiceSede.Text & "'"
        End If
        dataset = ClsServer.DataSetGenerico(query, Session("conn"))

        CaricaDataGrid(dgRisultatoRicerca)
    End Sub

    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dataset
        GridDaCaricare.DataBind()


        GridDaCaricare.Columns(INDEX_DATAGRID_DGRISULTATORICERCA_PRESENZA_SANZIONE).Visible = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        GridDaCaricare.Columns(INDEX_DATAGRID_DGRISULTATORICERCA_PRESENZA_VERIFICA).Visible = ClsUtility.ForzaSedeSottopostaVerifica(Session("Utente"), Session("conn"))

        ColoraCelle()
        Dim Modifica As Integer
        If Session("TipoUtente") = "E" Then
            Modifica = CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
        Else
            Modifica = CInt(Request.QueryString("Modifica"))
        End If
        If Modifica = "0" Then
            If txtModifica.Value = "False" Then
                'AggiungiModificaSede
                dgRisultatoRicerca.Columns(INDEX_DATAGRID_DGRISULTATORICERCA_MODIFICA_SEDE).Visible = False
                'Rimuovi Sede
                dgRisultatoRicerca.Columns(INDEX_DATAGRID_DGRISULTATORICERCA_RIMUOVI_SEDE).Visible = False
            End If
        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("SessionSediProgetto")
        dgRisultatoRicerca.DataBind()


        dgRisultatoRicerca.Columns(INDEX_DATAGRID_DGRISULTATORICERCA_PRESENZA_SANZIONE).Visible = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        dgRisultatoRicerca.Columns(INDEX_DATAGRID_DGRISULTATORICERCA_PRESENZA_VERIFICA).Visible = ClsUtility.ForzaSedeSottopostaVerifica(Session("Utente"), Session("conn"))

        ColoraCelle()
        Dim Modifica As Integer
        If Session("TipoUtente") = "E" Then
            Modifica = Not CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
        Else
            Modifica = CInt(Request.QueryString("Modifica"))
        End If

        If Modifica = "0" Then
            If txtModifica.Value = "False" Then
                'AggiungiModificaSede
                dgRisultatoRicerca.Columns(INDEX_DATAGRID_DGRISULTATORICERCA_MODIFICA_SEDE).Visible = False
                'Rimuovi Sede
                dgRisultatoRicerca.Columns(INDEX_DATAGRID_DGRISULTATORICERCA_RIMUOVI_SEDE).Visible = False
            End If
        End If

        ControllaPulsanti()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        'variabile che uso per controllare l'id della regione di competenza dell'ente
        Dim intIdRegioneCompetenza As Integer
        'variabile che uso per controllare se è Competenza Nazionale
        Dim strIdRegioneCompetenza As String
        Dim myCommand As New System.Data.SqlClient.SqlCommand
        Dim blnTipologiaProgetto As Boolean
        ChiudiDataReader(dataReader)


        If e.CommandName = "Rimuovi" Then
            '********************************************************************************
            'controllo se è l'ultima sede, se è l'ultima sede procedo con la cancellazione
            'solo se lo stato dell'attività è "registrato", altrimenti blocco la rimozione
            '********************************************************************************
            Dim idAttivitaEnteSedeAttuazione As String = e.Item.Cells(INDEX_DATAGRID_DGRISULTATORICERCA_ID_ATTIVITA_ENTE_SEDE).Text
            Dim idSedeAttuazione As String = e.Item.Cells(INDEX_DATAGRID_DGRISULTATORICERCA_CODICE_SEDE).Text
            Dim idSede As String = e.Item.Cells(INDEX_DATAGRID_DGRISULTATORICERCA_ID_SEDE).Text
            Dim idAttivita As String = Request.QueryString("IdAttivita")


            '************************************************************
            'Verifica la presenza di volontari sulla sede di progetto
            If VolontariSuSedeProgetto(idAttivitaEnteSedeAttuazione) = True Then
                lblmessaggio.Text = "Non è possibile cancellare la sede in quanto ci sono Volontari in Servizio."
                Exit Sub
            End If
            '************************************************************

            '************************************************************
            'Verifica la presenza di graduatoria sulla sede di progetto
            If VolontariSuGraduatoria(idAttivita, idSede) = True Then
                lblmessaggio.Text = "Non è possibile cancellare la sede in quanto ci sono Graduatorie Volontari caricate non confermate."
                Exit Sub
            End If
            '************************************************************
            'controllo se la regione di competenza dell'attività è diveras da nazionale
            'in questo caso procedo con i controlli sullo stato del progetto

            query = "select attività.IdRegioneCompetenza, isnull(RegioniCompetenze.CodiceRegioneCompetenza, '') as CodiceRegioneCompetenza from attività LEFT JOIN RegioniCompetenze ON RegioniCompetenze.IdRegioneCompetenza=attività.IdRegioneCompetenza where attività.idattività='" & Request.QueryString("IdAttivita") & "'"

            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()
            If dataReader("CodiceRegioneCompetenza") <> "NAZ" Then
                ChiudiDataReader(dataReader)

                'controllo il numero delle sedi sul progetto in questione
                query = "select COUNT(*) AS NSedi from attivitàentisediattuazione where idattività='" & Request.QueryString("IdAttivita") & "'"
                dataReader = ClsServer.CreaDatareader(query, Session("conn"))

                'se ci sono righe controllo il numero
                If dataReader.HasRows = True Then
                    dataReader.Read()
                    'se è rimasta una sede soltanto e il progetto non è di competenza nazionale
                    'controllo lo stato del progetto
                    'proseguo con la cancellazione SOLO se è REGISTRATO
                    If dataReader("NSedi") = 1 Then
                        ChiudiDataReader(dataReader)

                        query = "SELECT statiattività.StatoAttività AS StatoAttività "
                        query = query & "FROM attività "
                        query = query & "INNER JOIN statiattività ON statiattività.IDStatoAttività = attività.IDStatoAttività "
                        query = query & "WHERE attività.idattività='" & idAttivita & "'"
                        dataReader = ClsServer.CreaDatareader(query, Session("conn"))

                        If dataReader.HasRows = True Then
                            dataReader.Read()

                            'se lo stato del progetto è diverso da registrato blocco la cancellazione
                            If dataReader("StatoAttività") <> "Registrato" Then
                                lblmessaggio.Text = "Attenzione. Il progetto è già legato a Istanza. Per cambiare competenza rimuoverlo dall'Istanza."
                                ChiudiDataReader(dataReader)
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
            ChiudiDataReader(dataReader)
            Dim Modifica As Integer
            If Session("TipoUtente") = "E" Then
                Modifica = Not CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
            Else
                Modifica = CInt(Request.QueryString("Modifica"))
            End If

            query = "select attività.idattività,statiattività.statoattività,statiattività.defaultstato as defaultAttività," & _
            " case isnull(convert(smallint,statibandiAttività.defaultstato),-1)when -1 then '0'" & _
            " else convert(smallint,statibandiAttività.defaultstato)  end as defaultBando " & _
            " from attività " & _
            " inner join statiattività on (statiattività.idstatoattività=attività.idstatoattività)" & _
            " left join bandiAttività  on (attività.idbandoattività=bandiAttività.idbandoattività)" & _
            " left join statibandiAttività  on (bandiAttività.idstatobandoattività=statibandiAttività.idstatobandoattività)" & _
            " where attività.idattività=" & Request.QueryString("IdAttivita") & ""

            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()


            If Modifica = "0" Then
                If dataReader("defaultAttività") = False And dataReader("defaultBando") = False Then
                    Dim attiva As Int32
                    Int32.TryParse(e.Item.Cells(INDEX_DATAGRID_DGRISULTATORICERCA_ATTIVA).Text, attiva)
                    If attiva <> 0 Then
                        lblmessaggio.Text = "Non è possibile modificare la sede perchè il progetto è in stato di: " & dataReader("statoattività") & "."
                    End If
                    ChiudiDataReader(dataReader)
                    Exit Sub
                End If
            End If
            ChiudiDataReader(dataReader)
            '************************************************************
            ' aggiunto da simona cordella il 25/11/2008
            'controllo se la sede è sottoposta a verifica se è <=5 cancello la sede altrimenti invio messaggio e blocco l'eliminazione
            If SedeSottopostaVerifica(idAttivitaEnteSedeAttuazione) = True Then
                lblmessaggio.Text = "La sede selezionata per la rimozione risulta sottoposta a verifica."
                Exit Sub
            End If
            '************************************************************
            query = "select * from Associaentepersonaleruoliattivitàentisediattuazione where idattivitàentesedeattuazione=" & idAttivitaEnteSedeAttuazione & " "
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            If dataReader.HasRows = False Then
                ChiudiDataReader(dataReader)
                query = "delete attivitàentisediattuazione where idattività='" & idAttivita & "' and identesedeattuazione=" & idSedeAttuazione & ""
                sqlCommand = ClsServer.EseguiSqlClient(query, Session("conn"))

                '*********************************************************************************************
                Dim dtrlocal As SqlClient.SqlDataReader
                Dim intPostiVittoAlloggio As Integer
                Dim intPostiNovittoNoAlloggio As Integer
                Dim intPostiVitto As Integer
                Dim intPostiFami As Integer
                Dim intPostiGMO As Integer
                Dim intIdTipoProgetto As Integer
                ChiudiDataReader(dataReader)

                query = "select NazioneBase, TipiProgetto.idtipoprogetto, year(attività.datacreazionerecord) as Anno from TipiProgetto "
                query = query & "inner join attività on attività.idtipoprogetto=tipiprogetto.idtipoprogetto "
                query = query & "where attività.idattività='" & idAttivita & "'"
                myCommand.Connection = Session("conn")
                myCommand.CommandText = query
                dtrlocal = myCommand.ExecuteReader
                dtrlocal.Read()
                If dtrlocal.HasRows = True Then
                    blnTipologiaProgetto = dtrlocal("NazioneBase")
                    intIdTipoProgetto = dtrlocal("idtipoprogetto")
                    'forzo i progetti gg inseriti dal bando 2020 in poi
                    If intIdTipoProgetto = 4 And CInt(dtrlocal("Anno")) >= 2019 Then
                        intIdTipoProgetto = 11
                    End If
                End If
                ChiudiDataReader(dtrlocal)

                'se è nazionale 
                '1nazionale
                '2estero
                If blnTipologiaProgetto = True Then
                    'Area - Settore
                    query = "select sum(a.NumeroPostiNovittoNoAlloggio) as NoVittoNoAloggio, "
                    query = query & "sum(a.NumeroPostivittoAlloggio) as VittoAloggio, "
                    query = query & "sum(a.NumeroPostivitto) as Vitto, "
                    query = query & "sum(a.NumeroPostiFami) as NumeroPostiFami, "
                    query = query & "sum(a.NumeroPostiGMO) as NumeroPostiGMO "
                    query = query & "from attivitàentisediattuazione as a "
                    query = query & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
                    query = query & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
                    query = query & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
                    query = query & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
                    query = query & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
                    query = query & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
                    query = query & "where a.idattività='" & idAttivita & "' AND (nazioni.NazioneBase = 1)"
                Else
                    query = "SELECT SUM(a.NumeroPostiNoVittoNoAlloggio) AS NoVittoNoAloggio, "
                    query = query & "SUM(a.NumeroPostiVittoAlloggio) AS VittoAloggio, "
                    query = query & "SUM(a.NumeroPostiVitto) AS Vitto, "
                    query = query & "sum(a.NumeroPostiFami) as NumeroPostiFami, "
                    query = query & "sum(a.NumeroPostiGMO) as NumeroPostiGMO "
                    query = query & "FROM attivitàentisediattuazione a "
                    query = query & "INNER JOIN entisediattuazioni ON a.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
                    query = query & "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
                    query = query & "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune "
                    query = query & "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
                    query = query & "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione "
                    query = query & "INNER JOIN nazioni ON regioni.IDNazione = nazioni.IDNazione "
                    query = query & "WHERE (a.IDAttività='" & idAttivita & "') AND (nazioni.NazioneBase = 0)"
                End If
                myCommand.CommandText = query
                dtrlocal = myCommand.ExecuteReader
                dtrlocal.Read()

                If dtrlocal.HasRows = True Then
                    If IsDBNull(dtrlocal("NoVittoNoAloggio")) = False Then
                        intPostiNovittoNoAlloggio = CInt(dtrlocal("NoVittoNoAloggio"))
                    Else
                        intPostiNovittoNoAlloggio = 0
                    End If
                    If IsDBNull(dtrlocal("VittoAloggio")) = False Then
                        intPostiVittoAlloggio = CInt(dtrlocal("VittoAloggio"))
                    Else
                        intPostiVittoAlloggio = 0
                    End If
                    If IsDBNull(dtrlocal("Vitto")) = False Then
                        intPostiVitto = CInt(dtrlocal("Vitto"))
                    Else
                        intPostiVitto = 0
                    End If
                    If IsDBNull(dtrlocal("NumeroPostiFami")) = False Then
                        intPostiFami = CInt(dtrlocal("NumeroPostiFami"))
                    Else
                        intPostiFami = 0
                    End If
                    If IsDBNull(dtrlocal("NumeroPostiGMO")) = False Then
                        intPostiGMO = CInt(dtrlocal("NumeroPostiGMO"))
                    Else
                        intPostiGMO = 0
                    End If
                End If
                ChiudiDataReader(dtrlocal)

                query = "Update attività set NumeroPostiNoVittoNoAlloggio=" & intPostiNovittoNoAlloggio & ", "
                query = query & "NumeroPostiVittoAlloggio=0" & intPostiVittoAlloggio & ", "
                query = query & "NumeroPostiVitto=" & intPostiVitto & ", "
                query = query & "NumeroPostiFami=" & intPostiFami & " "
                If intIdTipoProgetto >= 11 Then
                    query = query & ",NumeroGiovaniMinoriOpportunità=" & intPostiGMO & " "
                End If
                query = query & "where idattività=" & idAttivita

                sqlCommand = ClsServer.EseguiSqlClient(query, Session("conn"))
                ChiudiDataReader(dataReader)
                ChiudiDataReader(dtrlocal)

                Dim myCommandSP As New System.Data.SqlClient.SqlCommand
                myCommandSP.CommandText = "SP_PROGRAMMI_AGGIORNA_POSTI_da_progetto"
                myCommandSP.CommandType = CommandType.StoredProcedure

                myCommandSP.Connection = Session("conn")
                myCommandSP.Parameters.AddWithValue("@IdAttività", idAttivita)
                myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
                myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
                myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
                myCommandSP.Parameters("@messaggio").Size = 1000
                myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
                myCommandSP.ExecuteNonQuery()

                'Dim myCommandSP As New System.Data.SqlClient.SqlCommand
                myCommandSP.CommandText = "SP_PROGETTI_COPROGETTAZIONE_ACCOGLIENZA"
                myCommandSP.CommandType = CommandType.StoredProcedure
                myCommandSP.Connection = Session("conn")
                myCommandSP.Parameters.Clear()
                myCommandSP.Parameters.AddWithValue("@IdAttività", idAttivita)
                myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
                myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
                myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
                myCommandSP.Parameters("@messaggio").Size = 1000
                myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
                myCommandSP.ExecuteNonQuery()

                '*********************************************************************************************
                'Aggiunto Da Alessandra Taballione il 15/09/2005
                'Se non esistono sedi attuazione per quella sede assegnazione effettuo Eliminazione 
                'Sede Assegnazione
                '******************
                query = "select count(distinct attivitàentisediattuazione.IDEnteSedeAttuazione) as sediatt " & _
                " FROM entisediattuazioni " & _
                " INNER JOIN attivitàentisediattuazione " & _
                " ON entisediattuazioni.IDEnteSedeAttuazione = attivitàentisediattuazione.IDEnteSedeAttuazione " & _
                " inner JOIN AttivitàSediAssegnazione " & _
                " on AttivitàSediAssegnazione.identesede=entisediattuazioni.identesede AND attivitàsediassegnazione.idattività = attivitàentisediattuazione.idattività " & _
                " where AttivitàSediAssegnazione.identesede=" & idSede & " " & _
                " and AttivitàSediAssegnazione.idattività=" & idAttivita
                dataReader = ClsServer.CreaDatareader(query, Session("conn"))
                If dataReader.HasRows = True Then
                    dataReader.Read()
                    If dataReader("sediatt") = 0 Then
                        ChiudiDataReader(dataReader)
                        query = "delete AttivitàSediAssegnazione where idattività='" & idAttivita & "' and identesede=" & idSede & ""
                        sqlCommand = ClsServer.EseguiSqlClient(query, Session("conn"))
                        ChiudiDataReader(dataReader)
                    End If
                End If
                ChiudiDataReader(dataReader)
                RicercaSedi()
            Else
                lblmessaggio.Text = "Non è possibile rimuovere la sede perchè è presente del personale associato."

                ChiudiDataReader(dataReader)
                Response.Redirect("WebElencoEliminaSedeProgetto.aspx?EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&idTipoProg=" & Request.QueryString("idTipoProg") & "&idSede=" & idSede & "&IdSedeAttuazione=" & idSedeAttuazione & "&IdAttivita=" & idAttivita & "&idattES=" & idAttivitaEnteSedeAttuazione & "&blnVisualizzaVolontari=" & Request.QueryString("blnVisualizzaVolontari") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale"))
            End If


            '****************************************************************************
            'modifica effettuata da Jodel Castro il 19/07/2006
            '****************************************************************************
            'controllo se per questa attività non ci sono più sedi di attuazione
            'qualora non ci fossero e l'ente non è di competenza nazionale
            'vado a fare l'update sul campo IdRegioneCompetenza su attività 
            'rimettendolo a null (nessuna competenza)
            '*'controllo se l'ente è di competenza nazionale
            '*'nel caso lo sia evito di fare i controlli sulle sedi di attuazione inserite
            '*'perchè l'idregionecompetenza resterebbe lo stesso
            ChiudiDataReader(dataReader)
            query = "select enti.IdRegioneCompetenza, RegioniCompetenze.CodiceRegioneCompetenza from enti " & _
                    " INNER JOIN RegioniCompetenze ON RegioniCompetenze.IdRegioneCompetenza=enti.IdRegioneCompetenza " & _
                    " where enti.idente='" & Session("IdEnte") & "'"
            'eseguo la query
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            'leggo il datareader
            If dataReader.HasRows = True Then
                dataReader.Read()
                intIdRegioneCompetenza = dataReader("IdRegioneCompetenza")
                strIdRegioneCompetenza = dataReader("CodiceRegioneCompetenza")
                'MODIFICA: Se si stà inserendo un progetto estero e l'ente è regionale, lo tratto come un ente di competenza nazionale

                If CInt(Request.QueryString("Nazionale")) = 2 And strIdRegioneCompetenza <> "NAZ" Then       'PROGETTO ESTERO / ENTE REGIONALE
                    intIdRegioneCompetenza = 22
                    strIdRegioneCompetenza = "NAZ"
                End If


            End If
            ChiudiDataReader(dataReader)

            'se l'ente non è di competenza nazionale allora vado a controllare se esistono 
            'ancora sedi di attuazione legate al progetto
            If strIdRegioneCompetenza <> "NAZ" Then
                query = "select * from attivitàentisediattuazione where idattività='" & idAttivita & "'"

                dataReader = ClsServer.CreaDatareader(query, Session("conn"))

                'se non ci sono righe fare l'update su attività e mettere a null il campo IdRegioneCompetenza
                If dataReader.HasRows = False Then
                    ChiudiDataReader(dataReader)
                    query = "Update attività set IdRegioneCompetenza=null "
                    query = query & "where idRegioneCompetenza<>22 and idattività=" & idAttivita
                    sqlCommand = ClsServer.EseguiSqlClient(query, Session("conn"))
                End If
                ChiudiDataReader(dataReader)

            End If

        End If
        If e.CommandName = "AggiungiModificaSede" Then
            Dim idAttivitaEnteSedeAttuazione As String = e.Item.Cells(INDEX_DATAGRID_DGRISULTATORICERCA_ID_ATTIVITA_ENTE_SEDE).Text
            Dim idSedeAttuazione As String = e.Item.Cells(INDEX_DATAGRID_DGRISULTATORICERCA_CODICE_SEDE).Text
            Dim idSede As String = e.Item.Cells(INDEX_DATAGRID_DGRISULTATORICERCA_ID_SEDE).Text
            Dim idAttivita As String = Request.QueryString("IdAttivita")
            Dim tipoProgetto As String = Request.QueryString("Nazionale")
            Dim numeroVolontari As String = e.Item.Cells(INDEX_DATAGRID_DGRISULTATORICERCA_NUMERO_VOLONTARI).Text
            Dim statoEnteSede As Int32
            Int32.TryParse(e.Item.Cells(INDEX_DATAGRID_DGRISULTATORICERCA_STATO_ENTE).Text, statoEnteSede)
            If (statoEnteSede <= 0) Then
                lblmessaggio.Text = "Non è possibile modificare la sede progetto."
            End If
            Response.Redirect("WebAggiungiSedeProgetto.aspx?EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&idTipoProg=" & tipoProgetto & "&idSede=" & idSede & "&IdSedeAttuazione=" & idSedeAttuazione & "&IdAttivita=" & idAttivita & "&idattES=" & idAttivitaEnteSedeAttuazione & "&blnVisualizzaVolontari=" & Request.QueryString("blnVisualizzaVolontari") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&VengoDa=" & Request.QueryString("VengoDa") & "")
        End If
        ControllaPulsanti()
    End Sub

    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 15/06/04
        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer
        Dim txt As String
        For Each item In dgRisultatoRicerca.Items
            Dim statoentesede = dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text
            Select Case statoentesede
                Case "Accreditata"
                    If InStr(dgRisultatoRicerca.Items(item.ItemIndex).Cells(1).Text, "(*)") > 0 Then
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(1).ForeColor = color.Red
                    End If
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = VERDE
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = NERO
                    Next
                Case "Presentata"
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = GIALLO
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = NERO
                    Next
                Case "Sospesa"
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = GRIGIO
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = NERO
                    Next
                Case "Cancellata"
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = ROSSO
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = NERO
                    Next
            End Select
        Next
    End Sub

    'Private Sub imgAggiungi_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAggiungi.Click
    '    Response.Redirect("inserimentosediprogetto.aspx?strTitoloProgetto=" & Request.QueryString("strTitoloProgetto") & "&blnVisualizzaVolontari=" & Request.QueryString("blnVisualizzaVolontari") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&VengoDa=" & Request.QueryString("VengoDa"))
    'End Sub

    Private Function VolontariSuSedeProgetto(ByVal IdAttivitàEnteSedeAttuazione As Integer) As Boolean
        'Aggiunto il 26/11/2008 da Simona Cordella
        'Controllo la presenza di volontari sulla sede che sto cancellando
        ' Se ci sono invio un messaggio e blocco la cancellazione della sede di progetto
        Dim strsql As String
        Dim dataReaderVolontario As SqlClient.SqlDataReader

        strsql = " SELECT  attivitàentità.IDEntità " & _
                 " FROM attivitàentisediattuazione  " & _
                 " INNER JOIN attivitàentità  ON  attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione " & _
                 " WHERE attivitàentità.IDAttivitàEnteSedeAttuazione = " & IdAttivitàEnteSedeAttuazione & " "
        dataReaderVolontario = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dataReaderVolontario.HasRows = True Then
            dataReaderVolontario.Read()
            ChiudiDataReader(dataReaderVolontario)
            Return True 'nn posso cancellare la sede
        Else
            ChiudiDataReader(dataReaderVolontario)
            Return False 'nn posso cancellare la sede
        End If
    End Function

    Private Function SedeSottopostaVerifica(ByVal IdAttivitàEnteSedeAttuazione As Integer) As Boolean
        'Aggiunto il 25/11/2008 da Simona Cordella
        'Controllo se la sede di progetto che voglio rimuovere è sottoposta a verifica
        ' Se lo stato della verifica è <=5 : cancello la sede
        ' Se lo stato della verifica è >5: invio messaggio e nn permetto la cancellazione
        Dim strsql As String
        Dim dtrSede As SqlClient.SqlDataReader
        Dim dtSede As DataTable
        Dim i As Integer
        strsql = " SELECT TVerifiche.IDVerifica " & _
                 " FROM TVerifiche " & _
                 " LEFT JOIN TVerificheAssociate ON TVerifiche.IDVerifica = TVerificheAssociate.IDVerifica " & _
                 " WHERE TVerificheAssociate.IDAttivitàEnteSedeAttuazione = " & IdAttivitàEnteSedeAttuazione & " and TVerifiche.IDStatoVerifica>5"
        dtrSede = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrSede.HasRows = True Then
            dtrSede.Read()
            If Not dtrSede Is Nothing Then
                dtrSede.Close()
                dtrSede = Nothing
            End If
            Return True 'nn posso cancellare la sede
        Else
            If Not dtrSede Is Nothing Then
                dtrSede.Close()
                dtrSede = Nothing
            End If
            strsql = " SELECT TVerifiche.IDVerifica " & _
                     " FROM TVerifiche " & _
                     " LEFT JOIN TVerificheAssociate ON TVerifiche.IDVerifica = TVerificheAssociate.IDVerifica " & _
                     " WHERE TVerificheAssociate.IDAttivitàEnteSedeAttuazione = " & IdAttivitàEnteSedeAttuazione & ""
            dtSede = ClsServer.CreaDataTable(strsql, False, Session("conn"))
            For i = 0 To dtSede.Rows.Count - 1

                'delete in TVerificheCronologiaStati
                strsql = " delete TVerificheCronologiaStati where IdVerifica = " & dtSede.Rows(i).Item(0)
                sqlCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                'delete in TVerificheVerificatori
                strsql = " delete TVerificheVerificatori where IdVerifica = " & dtSede.Rows(i).Item(0)
                sqlCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                'delete in TVerificheAssociate
                strsql = " Delete from TVerificheAssociate " & _
                         " Where IDAttivitàEnteSedeAttuazione = " & IdAttivitàEnteSedeAttuazione & " And idVerifica = " & dtSede.Rows(i).Item(0)
                sqlCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                ' delete in TVerifiche
                strsql = " Delete from TVerifiche" & _
                         " Where IdVerifica = " & dtSede.Rows(i).Item(0)
                sqlCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            Next
            Return False 'posso cancellare la sede
        End If
    End Function

    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click

        Dim Modifica As Integer
        If Session("TipoUtente") = "E" Then
            Modifica = CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
        Else
            Modifica = CInt(Request.QueryString("Modifica"))
        End If
        If Request.QueryString("popup") <> "1" Then
            Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?Modifica=" & Modifica & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa") & "")
            'Response.Redirect("TabProgetti.aspx?Modifica=" & Modifica & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa") & "")
        Else
            Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?popup=" & Request.QueryString("popup") & "&Modifica=" & Modifica & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa") & "")
            ' Response.Redirect("TabProgetti.aspx?popup=" & Request.QueryString("popup") & "&Modifica=" & Modifica & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa") & "")
        End If
    End Sub

    Private Function VolontariSuGraduatoria(ByVal IdAttività As Integer, ByVal IdSede As Integer) As Boolean
        'Aggiunto il 25/08/2016 da Simona Cordella
        'Controllo la presenza di graduatorie sulla sede che sto cancellando
        ' Se ci sono invio un messaggio e blocco la cancellazione della sede di progetto
        Dim strsql As String
        Dim dataReaderVolontario As SqlClient.SqlDataReader

        strsql = " SELECT graduatorieentità.IDEntità " & _
                 " FROM attivitàsediassegnazione  " & _
                 " INNER JOIN graduatorieentità  ON  attivitàsediassegnazione.IDAttivitàSedeAssegnazione = graduatorieentità.IDAttivitàSedeAssegnazione " & _
                 " WHERE attivitàsediassegnazione.IdAttività = " & IdAttività & " and attivitàsediassegnazione.IdEnteSede =" & IdSede
        dataReaderVolontario = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dataReaderVolontario.HasRows = True Then
            dataReaderVolontario.Read()
            ChiudiDataReader(dataReaderVolontario)
            Return True 'nn posso cancellare la sede
        Else
            ChiudiDataReader(dataReaderVolontario)
            Return False 'posso cancellare la sede
        End If
    End Function

   
    Protected Sub imgAggiungi_Click(sender As Object, e As EventArgs) Handles imgAggiungi.Click
        'Response.Redirect("WebAggiungiSedeProgettoEU.aspx?IdAttivita=" & Request.QueryString("IdAttivita") & "&Nazionale=" & Request.QueryString("Nazionale") & "&VengoDa=" & Request.QueryString("VengoDa"))
        Dim idAttivitaEnteSedeAttuazione As String = ""
        Dim idSedeAttuazione As String = ""
        Dim idSede As String = ""
        Dim idAttivita As String = Request.QueryString("IdAttivita")
        Dim tipoProgetto As String = Request.QueryString("Nazionale")
        Dim numeroVolontari As String = ""
        Response.Redirect("WebAggiungiSedeProgettoEU.aspx?EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&idTipoProg=" & tipoProgetto & "&idSede=" & idSede & "&IdSedeAttuazione=" & idSedeAttuazione & "&IdAttivita=" & idAttivita & "&idattES=" & idAttivitaEnteSedeAttuazione & "&blnVisualizzaVolontari=" & Request.QueryString("blnVisualizzaVolontari") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&VengoDa=" & Request.QueryString("VengoDa") & "")
    End Sub

    'Private Sub chkSediNonIscritte_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSediNonIscritte.CheckedChanged
    '    chkSediProgetto.Checked = False
    'End Sub
End Class
