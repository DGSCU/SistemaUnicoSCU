Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Public Class WfrmRicercaGeneraleSedi
    Inherits System.Web.UI.Page

    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim strsql As String
    Dim blnRicerca As Boolean
    Public blnForza As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dtrGenericoLocal As SqlClient.SqlDataReader
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            Try
                '******* simona
                ddlTipologia.Items.Add("")
                If Not dtrGenericoLocal Is Nothing Then
                    dtrGenericoLocal.Close()
                    dtrGenericoLocal = Nothing
                End If

                strsql = "select 1 ordina, entisedi.identesede,entirelazioni.identefiglio,entirelazioni.identePadre " & _
                " from enti " & _
                " inner join entisedi on(entisedi.idente=enti.idEnte)  " & _
                " inner join statientisedi on(entisedi.idstatoentesede=statientisedi.idstatoEntesede)  " & _
                " inner join  Entiseditipi on (entisedi.identesede=Entiseditipi.idEntesede) " & _
                " inner join tipisedi on (tipisedi.idtiposede=Entiseditipi.idtiposede) " & _
                " inner join entirelazioni on (entirelazioni.identefiglio=enti.idente)  " & _
                " left join associaentirelazionisedi on (entisedi.identesede=associaentirelazionisedi.identesede  " & _
                " and entirelazioni.identerelazione=associaentirelazionisedi.identerelazione) " & _
                " where(entirelazioni.identePadre = " & Session("idEnte") & " " & _
                " and (statientisedi.attiva=1 or statientisedi.daaccreditare=1) And entirelazioni.datafinevalidità Is null And associaentirelazionisedi.idassociaentirelazionisedi Is null)"
                dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If dtrGenerico.HasRows = True Then
                    'chkIncludi.Visible = True
                Else
                    'chkIncludi.Visible = False
                End If
                If Not dtrGenericoLocal Is Nothing Then
                    dtrGenericoLocal.Close()
                    dtrGenericoLocal = Nothing
                End If
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                dtrGenericoLocal = ClsServer.CreaDatareader("Select tiposede,idtiposede from tipiSedi order by idtiposede desc ", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                Do While dtrGenericoLocal.Read()
                    ddlTipologia.Items.Add(dtrGenericoLocal("tiposede"))
                Loop
                ddlTipologia.SelectedIndex = 1
                If Not dtrGenericoLocal Is Nothing Then
                    dtrGenericoLocal.Close()
                    dtrGenericoLocal = Nothing
                End If
                ddlstato.Items.Add("")
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                dtrGenericoLocal = ClsServer.CreaDatareader("select replace(statoEnteSede, 'accreditata', 'Iscritta') statoEnteSede from StatiEntiSedi", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                Do While dtrGenericoLocal.Read()
                    ddlstato.Items.Add(dtrGenericoLocal("statoEnteSede"))
                Loop
                dtrGenericoLocal.Close()
                dtrGenericoLocal = Nothing
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                CaricaCompetenze()

            Catch ex As Exception
                ex.Message.ToString()
            End Try

        End If
        If Page.IsPostBack = True Then
            'RicercaPersonaliEnte()
            'RicercaServizi()
            'RicercaSedi()
        End If
        If dgRisultatoRicerca.Items.Count > 0 Then
            CmdEsporta.Visible = True
        Else
            CmdEsporta.Visible = False
        End If
        If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
            txtDenominazioneEnte.Text = Request.QueryString("DenominazioneEnte")
            RicercaSedi()
        End If
        '***  agg. da simona cordella il 07/07/2011
        'se l'utene NON è abilitato al menu FORZA PRESENZA SANZIONE 
        'nn visualizzo il filtro di ricerca (aspx) Presenza Sanzione
        blnForza = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        '***
    End Sub

    Protected Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        dgRisultatoRicerca.CurrentPageIndex = 0
        RicercaSedi()
    End Sub

    Function checkFiltroFase() As Boolean
        lblErroreFiltroFase.Visible = False
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) AndAlso Not Integer.TryParse(txtFiltroFase.Text, Nothing) Then
            lblErroreFiltroFase.Visible = True
            Return False
        End If
        Return True
    End Function

#Region "Funzionalita"
    Private Sub RicercaSedi()
        If Not checkFiltroFase() Then Exit Sub
        'adc
        Dim UtenteRegioneCompetenza As String
        Dim strSQL As String
        'datareader che conterrà l'la descrizione della regione
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader
        strSQL = "select a.Descrizione from RegioniCompetenze a "
        strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
        strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"

        'controllo se utente o ente regionale
        'eseguo la query
        dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrCompetenze.Read()
        If dtrCompetenze.HasRows = True Then
            UtenteRegioneCompetenza = dtrCompetenze("Descrizione")
        End If
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If

        strSQL = "select distinct 0 ordina,enti.codiceregione,regionicompetenze.descrizione as competenza,'" & Replace(UtenteRegioneCompetenza, "'", "''") & "' as UtenteCompetenza,enti.denominazione as ente,entisedi.denominazione as sede," &
        " 'propria' acquisita,enti.idente, " &
        " replace(statosedefisica.statoentesede, 'Accreditata', 'Iscritta') as stato," &
        " entisedi.idEntesede,tipisedi.tiposede, entisedi.CodicesedeEnte as " &
        " Codicesede,entisedi.indirizzo + ' ' + entisedi.Civico as Indirizzo, " &
        " entisedi.cap,comuni.denominazione +' ['+ provincie.provincia +']' as Comune," &
        " entisedi.prefissotelefono + '/' + entisedi.telefono as telefono, entisedi.Email," &
        " entisediattuazioni.identesedeattuazione as Nsedi, " &
        " statientisedi.ordine," &
        " case isnull(entisediattuazioni.Datainserimento,0) when 0 then entisedi.DataCreazioneRecord else entisediattuazioni.Datainserimento end as DataCreazioneRecord, " &
        " entisediattuazioni.identesedeattuazione, " &
        " Case isnull(entisediattuazioni.Certificazione,2) When 0 then 'No' When 1 then 'Si' when 2 then 'Da Valutare'  End as Certificazione,  " &
        " Case isnull(entisediattuazioni.Segnalazione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp onclick=VisualizzaSanzione('+ convert(varchar,entisediattuazioni.IDEnteSedeAttuazione) + ','+ convert(varchar, enti.IDEnte) + ') STYLE=cursor:hand title=Sanzione border=0>' End as Segnalazione,  " &
        " Case isnull(entisediattuazioni.SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica, " &
        " enti.idClasseAccreditamentoRichiesta " &
        " ,isnull(entisediattuazioni.nmaxvolontari,0) as nmaxvolontari " &
        " ,entiSedi.AnomaliaIndirizzo, entiSedi.AnomaliaNome, entiSedi.AnomaliaIndirizzoGoogle" &
        " from enti" &
        " inner join entisedi on(entisedi.idente=enti.idEnte) " &
        " left  join entisediattuazioni on entisediattuazioni.identesede=entisedi.identesede " &
        " left  join statientisedi statosedefisica on(entisediattuazioni.idstatoentesede=statosedefisica.idstatoEntesede) " &
        " inner  join statientisedi on(entisedi.idstatoentesede=statientisedi.idstatoEntesede) " &
        " inner join comuni on (entisedi.idcomune=Comuni.idcomune)  " &
        " inner join Provincie on (provincie.idprovincia=Comuni.idProvincia)  " &
        " INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione " &
        " inner join  Entiseditipi on (entisedi.identesede=Entiseditipi.idEntesede)  " &
        " inner join tipisedi on (tipisedi.idtiposede=Entiseditipi.idtiposede) " &
        " left join entirelazioni on (entirelazioni.identefiglio=enti.idente) " &
        " inner Join regionicompetenze on enti.idregionecompetenza = regionicompetenze.idregionecompetenza "

        'la validità del FiltroFase è stata controllata prima
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
            strSQL = strSQL & " left join EntiFasi_Sedi efs on efs.IdEnteSedeAttuazione=entisediattuazioni.IdEnteSedeAttuazione "
        End If

        strSQL = strSQL & " where entirelazioni.identefiglio is null and 1=1 and substring(entisedi.usernamestato,1,1) <> 'N' "
        If Trim(txtDenominazione.Text) <> "" Then
            strSQL = strSQL & " and entisedi.denominazione like '" & Replace(txtDenominazione.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodice.Text) <> "" Then
            strSQL = strSQL & " and entisedi.codiceSedeEnte='" & Replace(txtCodice.Text, "'", "''") & "'"
        End If
        If Not IsNothing(ddlTipologia.SelectedItem.Text) Then
            If Trim(ddlTipologia.SelectedItem.Text) <> "" Then
                strSQL = strSQL & " and tipisedi.tiposede='" & Replace(ddlTipologia.SelectedItem.Text, "'", "''") & "'"
            End If
        End If
        If ddlstato.SelectedItem.Text <> "" Then
            'strSQL = strSQL & " and  statientisedi.statoentesede='" & ddlstato.SelectedItem.Text & "'"
            strSQL = strSQL & " and replace(case isnull(statosedefisica.statoentesede,'vuoto') when 'vuoto' then statientisedi.statoentesede else  statosedefisica.statoentesede end, 'accreditata', 'Iscritta') ='" & ddlstato.SelectedItem.Text & "'"
        End If
        If Trim(txtregione.Text) <> vbNullString Then
            strSQL = strSQL & " and Regioni.Regione LIKE '" & Replace(txtregione.Text, "'", "''") & "%'"
        End If
        If Trim(txtProvincia.Text) <> vbNullString Then
            strSQL = strSQL & " and Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            strSQL = strSQL & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If
        If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
            strSQL = strSQL & " and enti.denominazione LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%'"
        End If

      
        Dim primo As Integer
        primo = Integer.TryParse(Trim(txtCodSedeAtt.Text), primo)
        If Trim(txtCodSedeAtt.Text) <> "" Then
            If primo = -1 Then
                strSQL = strSQL & " and EntiSediAttuazioni.identesedeattuazione=" & Trim(txtCodSedeAtt.Text) & ""
            Else
                lblmessaggio.Text = "Inserire Valore Numerico"
                primo = 1
            End If
        End If


        If Trim(txtCodRegione.Text) <> "" Then
            strSQL = strSQL & " and enti.Codiceregione ='" & Replace(txtCodRegione.Text, "'", "''") & "'"
        End If

        If CboCompetenza.SelectedValue <> "" Then
            Select Case CboCompetenza.SelectedValue
                Case 0
                    strSQL = strSQL & " "
                Case -1
                    strSQL = strSQL & " And enti.IdRegioneCompetenza = 22"
                Case -2
                    strSQL = strSQL & " And enti.IdRegioneCompetenza <> 22 And not enti.IdRegioneCompetenza is null "
                Case -3
                    strSQL = strSQL & " And enti.IdRegioneCompetenza is null "
                Case Else
                    strSQL = strSQL & " And enti.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            End Select

        End If
        If ddlCertificazione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and isnull(entisediattuazioni.Certificazione,2) =" & ddlCertificazione.SelectedValue
        End If
        'agg. da sc il 09/06/2009 trovo se si sono sedi con lo stesso indirizzo,civico, comune,cap 
        If ddlDuplicati.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and dbo.DoppioneSede(EntiSediAttuazioni.identesedeattuazione)=" & ddlDuplicati.SelectedValue
        End If
        'agg. da sc il 05/07/2011 combo su segnalazione della sede
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If
        If ddlLocalizzazione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and comuni.ComuneNazionale = " & ddlLocalizzazione.SelectedValue
        End If

        If ddlAnomalie.SelectedItem.Text <> "Qualsiasi" Then
            Select Case ddlAnomalie.SelectedValue
                Case 0
                    strSQL &= " and coalesce(entiSedi.AnomaliaNome,0) + coalesce(entiSedi.AnomaliaIndirizzoGoogle, 0) > 0"
                Case 1
                    strSQL &= " and coalesce(entiSedi.AnomaliaNome,0) + coalesce(entiSedi.AnomaliaIndirizzoGoogle, 0) < 1"
                Case 2
                    strSQL &= " and coalesce(entiSedi.AnomaliaIndirizzoGoogle,0) = 1"
                Case 3
                    strSQL &= " and coalesce(entiSedi.AnomaliaNome,0) = 1"
            End Select
        End If

        'la validità del FiltroFase è stata controllata prima
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
            strSQL = strSQL & " and efs.IdEnteFase =" & Trim(txtFiltroFase.Text)
        End If

        strSQL = strSQL & " union " &
                " select distinct 1 ordina, enti.codiceregione,regionicompetenze.descrizione as competenza,'" & Replace(UtenteRegioneCompetenza, "'", "''") & "' as UtenteCompetenza, " &
                " left(enti.denominazione,20) as ente,entisedi.denominazione as sede," &
                " case isnull(associaentirelazionisedi.idassociaentirelazionisedi,-1) when -1 then 'no' else convert(varchar(10),associaentirelazionisedi.idassociaentirelazionisedi) end acquisita," &
                " enti.idente, " &
                " replace(statosedefisica.statoentesede, 'Accreditata', 'Iscritta') as stato," &
                " entisedi.idEntesede,tipisedi.tiposede, entisedi.CodicesedeEnte as " &
                " Codicesede,entisedi.indirizzo + ' ' + entisedi.Civico as Indirizzo, " &
                " entisedi.cap,comuni.denominazione +' ['+ provincie.provincia +']' as Comune," &
                " entisedi.prefissotelefono + '/' + entisedi.telefono as telefono, entisedi.Email," &
                " entisediattuazioni.identesedeattuazione as Nsedi, " &
                " statientisedi.ordine, " &
                " case isnull(entisediattuazioni.Datainserimento,0) when 0 then entisedi.DataCreazioneRecord else entisediattuazioni.Datainserimento end as DataCreazioneRecord, " &
                " entisediattuazioni.identesedeattuazione, " &
                " Case isnull(entisediattuazioni.Certificazione,2) When 0 then 'No' When 1 then 'Si' when 2 then 'Da Valutare'  End as Certificazione,  " &
                " Case isnull(entisediattuazioni.Segnalazione,0) When 0 then 'No' When 1 then '<img src=images/documento_small.png onclick=VisualizzaSanzione('+ convert(varchar,entisediattuazioni.IDEnteSedeAttuazione) + ','+ convert(varchar, enti.IDEnte) + ') STYLE=cursor:hand title=Sanzione border=0>' End as Segnalazione,  " &
                " Case isnull(entisediattuazioni.SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica, " &
                " enti.idClasseAccreditamentoRichiesta " &
                " ,isnull(entisediattuazioni.nmaxvolontari,0) as nmaxvolontari " &
                " ,entiSedi.AnomaliaIndirizzo, entiSedi.AnomaliaNome, entiSedi.AnomaliaIndirizzoGoogle" &
                " from enti" &
                " inner join entisedi on(entisedi.idente=enti.idEnte) " &
                " left  join entisediattuazioni on entisediattuazioni.identesede=entisedi.identesede " &
                " left  join statientisedi statosedefisica on(entisediattuazioni.idstatoentesede=statosedefisica.idstatoEntesede) " &
                " inner  join statientisedi on(entisedi.idstatoentesede=statientisedi.idstatoEntesede) " &
                " inner join comuni on (entisedi.idcomune=Comuni.idcomune)  " &
                " inner join Provincie on (provincie.idprovincia=Comuni.idProvincia)  " &
                " INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione " &
                " inner join  Entiseditipi on (entisedi.identesede=Entiseditipi.idEntesede)" &
                " inner join tipisedi on (tipisedi.idtiposede=Entiseditipi.idtiposede)" &
                " inner join entirelazioni on (entirelazioni.identefiglio=enti.idente) " &
                " inner Join regionicompetenze on enti.idregionecompetenza = regionicompetenze.idregionecompetenza " &
                " left join associaentirelazionisedi on (entisedi.identesede=associaentirelazionisedi.identesede " &
                " and entirelazioni.identerelazione=associaentirelazionisedi.identerelazione)"

        'la validità del FiltroFase è stata controllata prima
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
            strSQL = strSQL & " left join EntiFasi_Sedi efs on efs.IdEnteSedeAttuazione=entisediattuazioni.IdEnteSedeAttuazione "
        End If

        strSQL = strSQL & " where substring(entisedi.usernamestato,1,1) <> 'N'" 'and entirelazioni.datafinevalidità is  null

        If Trim(txtDenominazione.Text) <> "" Then
            strSQL = strSQL & " and entisedi.denominazione like '" & Replace(txtDenominazione.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodice.Text) <> "" Then
            strSQL = strSQL & " and entisedi.codiceSedeEnte='" & Replace(txtCodice.Text, "'", "''") & "'"
        End If
        If Trim(ddlTipologia.SelectedItem.Text) <> "" Then
            strSQL = strSQL & " and tipisedi.tiposede='" & Replace(ddlTipologia.SelectedItem.Text, "'", "''") & "'"
        End If
        If ddlstato.SelectedItem.Text <> "" Then
            'strSQL = strSQL & " and  statientisedi.statoentesede='" & ddlstato.SelectedItem.Text & "'"
            strSQL = strSQL & " and case isnull(statosedefisica.statoentesede,'vuoto') when 'vuoto' then statientisedi.statoentesede else  statosedefisica.statoentesede end ='" & ddlstato.SelectedItem.Text & "'"
        End If
        If Trim(txtregione.Text) <> vbNullString Then
            strSQL = strSQL & " and Regioni.Regione LIKE '" & Replace(txtregione.Text, "'", "''") & "%'"
        End If
        If Trim(txtProvincia.Text) <> vbNullString Then
            strSQL = strSQL & " and Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            strSQL = strSQL & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If
        If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
            strSQL = strSQL & " and enti.denominazione LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%'"
        End If

        Dim secondo As Integer
        secondo = Integer.TryParse(Trim(txtCodSedeAtt.Text), secondo)
        If Trim(txtCodSedeAtt.Text) <> "" Then
            If secondo = -1 Then
                strSQL = strSQL & " and EntiSediAttuazioni.identesedeattuazione=" & Trim(txtCodSedeAtt.Text) & ""
            Else
                lblmessaggio.Text = "Inserire Valore Numerico"
                secondo = 1
            End If
        End If


        If Trim(txtCodRegione.Text) <> "" Then
            strSQL = strSQL & " and enti.Codiceregione LIKE '" & Replace(txtCodRegione.Text, "'", "''") & "%'"
        End If

        If CboCompetenza.SelectedValue <> "" Then
            Select Case CboCompetenza.SelectedValue
                Case 0
                    strSQL = strSQL & " "
                Case -1
                    strSQL = strSQL & " And enti.IdRegioneCompetenza = 22"
                Case -2
                    strSQL = strSQL & " And enti.IdRegioneCompetenza <> 22 And not enti.IdRegioneCompetenza is null "
                Case -3
                    strSQL = strSQL & " And enti.IdRegioneCompetenza is null "
                Case Else
                    strSQL = strSQL & " And enti.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            End Select

        End If
        If ddlCertificazione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and isnull(entisediattuazioni.Certificazione,2) =" & ddlCertificazione.SelectedValue
        End If
        If ddlLocalizzazione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and comuni.ComuneNazionale = " & ddlLocalizzazione.SelectedValue
        End If
        'agg. da sc il 05/07/2011 combo su segnalazione della sede
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If

        If ddlAnomalie.SelectedItem.Text <> "Qualsiasi" Then
            'Select Case ddlAnomalie.SelectedValue
            '    Case 0
            '        strSQL &= " and coalesce(entiSedi.AnomaliaIndirizzo,0) + coalesce(entiSedi.AnomaliaNome,0) + coalesce(entiSedi.AnomaliaIndirizzoGoogle, 0) > 0"
            '    Case 1
            '        strSQL &= " and coalesce(entiSedi.AnomaliaIndirizzo,0) + coalesce(entiSedi.AnomaliaNome,0) + coalesce(entiSedi.AnomaliaIndirizzoGoogle, 0) < 1"
            '    Case 2
            '        strSQL &= " and coalesce(entiSedi.AnomaliaIndirizzoGoogle,0) = 1"
            '    Case 3
            '        strSQL &= " and coalesce(entiSedi.AnomaliaNome,0) = 1"
            '    Case 4
            '        strSQL &= " and coalesce(entiSedi.AnomaliaIndirizzo,0) = 1"
            'End Select
            Select Case ddlAnomalie.SelectedValue
                Case 0
                    strSQL &= " and coalesce(entiSedi.AnomaliaNome,0) + coalesce(entiSedi.AnomaliaIndirizzoGoogle, 0) > 0"
                Case 1
                    strSQL &= " and coalesce(entiSedi.AnomaliaNome,0) + coalesce(entiSedi.AnomaliaIndirizzoGoogle, 0) < 1"
                Case 2
                    strSQL &= " and coalesce(entiSedi.AnomaliaIndirizzoGoogle,0) = 1"
                Case 3
                    strSQL &= " and coalesce(entiSedi.AnomaliaNome,0) = 1"
            End Select
        End If

        'la validità del FiltroFase è stata controllata prima
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
            strSQL = strSQL & " and efs.IdEnteFase =" & Trim(txtFiltroFase.Text)
        End If

        'agg. da sc il 09/06/2009 trovo se si sono sedi con lo stesso indirizzo,civico, comune,cap 
        If ddlDuplicati.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and dbo.DoppioneSede(EntiSediAttuazioni.identesedeattuazione)=" & ddlDuplicati.SelectedValue
            strSQL = strSQL & " order by comune,indirizzo "
        Else
            strSQL = strSQL & " order by ordina, STATIENTISEDI.ordine, ente,sede,acquisita"
        End If


        If (primo = -1 And secondo = -1) Or (primo = 0 And secondo = 0) Then

            dtsGenerico = ClsServer.DataSetGenerico(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            If txtRicerca.Text <> "" Then
                dgRisultatoRicerca.CurrentPageIndex = 0
                txtRicerca.Text = ""
            End If

            CaricaDataGrid(dgRisultatoRicerca)

        End If

    End Sub
    Private Sub RicercaSediStrsql(ByVal strsql As String)

        'Creazione della Stringa sql per la Ricerca Delle Sedi
        dtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If txtRicerca.Text <> "" Then
            dgRisultatoRicerca.CurrentPageIndex = 0
            txtRicerca.Text = ""
        End If

        CaricaDataGrid(dgRisultatoRicerca)

    End Sub
    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        'Verifoco l'abilitazione alla visualizzazione di determinate colonne
        If Session("TipoUtente") = "E" Then
            GridDaCaricare.Columns(16).Visible = False
        End If
        'AGG DA SIMONA CORDELLA IL 07/07/2011
        GridDaCaricare.Columns(21).Visible = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        GridDaCaricare.Columns(22).Visible = ClsUtility.ForzaSedeSottopostaVerifica(Session("Utente"), Session("conn"))

        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()


        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(11) As String
        Dim NomiCampiColonne(11) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Stato"
        NomeColonne(1) = "Ente Sede"
        NomeColonne(2) = "Ente"
        NomeColonne(3) = "Tipologia"
        NomeColonne(4) = "Cod.Sede Attuaz."
        NomeColonne(5) = "Indirizzo"
        NomeColonne(6) = "Comune"
        NomeColonne(7) = "Telefono"
        NomeColonne(8) = "Codice Nazionale"
        NomeColonne(9) = "Competenza"
        NomeColonne(10) = "NMaxVolontari"
        NomeColonne(11) = "Presenza Iscrizione"

        NomiCampiColonne(0) = "Stato"
        NomiCampiColonne(1) = "Sede"
        NomiCampiColonne(2) = "Ente"
        NomiCampiColonne(3) = "Tiposede"
        NomiCampiColonne(4) = "NSedi"
        NomiCampiColonne(5) = "Indirizzo"
        NomiCampiColonne(6) = "Comune"
        NomiCampiColonne(7) = "Telefono"
        NomiCampiColonne(8) = "codiceregione"
        NomiCampiColonne(9) = "Competenza"
        NomiCampiColonne(10) = "NMaxVolontari"
        NomiCampiColonne(11) = "Certificazione"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(dtsGenerico, 11, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        If GridDaCaricare.Items.Count = 0 Then
            '            GridDaCaricare.Visible = False
            'lblmessaggio.Text = "Nessun Dato estratto."

            CmdEsporta.Visible = False
            ApriCSV1.Visible = False
            dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato"

        Else
            GridDaCaricare.Visible = True
            lblmessaggio.Text = ""
            ColoraCelle()
            CmdEsporta.Visible = True
            ApriCSV1.Visible = False
            dgRisultatoRicerca.Caption = "Risultato Ricerca Sedi "

        End If




        'controllo se vengo da accettazione progetti per nascondere la colonna del pulsante
        If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
            GridDaCaricare.Columns(0).Visible = False
        End If

    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    Private Sub ColoraCelle()

        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim intConta As Integer
        Dim img As ImageButton
        For Each item In dgRisultatoRicerca.Items
            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(1).Text
                Case "Iscritta"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.LightGreen
                    Next

                    'aggiunto il 04/12/2008 da simona cordella
                    'se la sede è da valutare coloro il FONT DI VIOLA
                    Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text
                        Case "Da Valutare"
                            For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.FromArgb(201, 96, 185)
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).Font.Bold = True
                            Next
                        Case "No"
                            For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                                'dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).Font.Bold = True
                            Next
                    End Select
                    'ANTONELLO
                    ''''Case "Accreditata (*)"
                    ''''    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                    ''''        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
                    ''''        If intConta = 1 Then
                    ''''            dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                    ''''        End If
                    ''''    Next
                Case "Presentata"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Khaki
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Khaki
                    Next
                    'se la sede è da valutare coloro il FONT DI VIOLA
                    Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text
                        Case "Da Valutare"
                            For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.FromArgb(201, 96, 185)
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).Font.Bold = True
                            Next
                        Case "No"
                            For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                                'dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).Font.Bold = True
                            Next
                    End Select
                Case "Sospesa"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Gainsboro
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                    Next
                Case "Cancellata"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Gainsboro
                    Next
            End Select
            img = DirectCast(item.FindControl("IdImgAlert"), ImageButton)
            img.Visible = False
            img.ToolTip = ""
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(26).Text = "True" Then
                img.Visible = True
                img.ToolTip = "Il nome presenta anomalie o è duplicato" + vbNewLine
            End If
            'If dgRisultatoRicerca.Items(item.ItemIndex).Cells(27).Text = "True" Then
            '    img.Visible = True
            '    img.ToolTip = img.ToolTip + "Anomalia nell'indirizzo della sede" + vbNewLine
            'End If
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(28).Text = "True" Then
                img.Visible = True
                img.ToolTip = img.ToolTip + "Indirizzo non corrispondente a quello trovato da Google"
            End If
            img.Enabled = False
        Next
    End Sub
#End Region

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        'controllo se vengo dall'albero in gestione enti
        If Not Request.QueryString("VengoDa") Is Nothing Then
            'controllo se la variabile è valorizzata
            If Request.QueryString("VengoDa") <> "" Then
                'faccio la response.redirect verso l'albero
                Response.Redirect(Request.QueryString("VengoDa").ToString)
            End If
        End If
        'controllo se vengo dalla ricerca dei vincoli fra enti
        If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
            If Request.QueryString("CheckProvenienza") = "VisualizzazioneEntiInAccordo" Then
                Response.Redirect("WfrmElencoEntiAccordo.aspx?Pagina=" & Request.QueryString("Pagina") & "&CheckProvenienza=" & Request.QueryString("CheckProvenienza") & "&Denominazione=" & Request.QueryString("Denominazione") & "&idente=" & Request.QueryString("idente") & "&DenominazioneEnte=" & Request.QueryString("DenomonazioneEnte"))
            Else
                Response.Redirect("WfrmRicEnteinAccordo.aspx?Pagina=" & Request.QueryString("Pagina") & "&CheckProvenienza=" & Request.QueryString("CheckProvenienza") & "&Stato=" & Request.QueryString("Stato") & "&CF=" & Request.QueryString("CF") & "&ClasseAccreditamento=" & Request.QueryString("ClasseAccreditamento") & "&Tipologia=" & Request.QueryString("Tipologia") & "&CodiceRegione=" & Request.QueryString("CodicerRegione") & "&Denominazione=" & Request.QueryString("Denominazione") & "&VediEnte=1&DenominazioneEnte=" & Request.QueryString("DenomonazioneEnte"))
            End If
        Else
            Response.Redirect("WfrmMain.aspx")
        End If

    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Dim strSQL As String
        Dim ClasseAccreditamentoRichiesta As Integer
        Select Case e.CommandName
            Case "Select"
                Dim identesede As String = e.Item.Cells(12).Text
                Dim idente As String = e.Item.Cells(13).Text
                Dim acquisita As String = e.Item.Cells(14).Text
                Dim stato As String = e.Item.Cells(1).Text
                ClasseAccreditamentoRichiesta = e.Item.Cells(23).Text
                If ClasseAccreditamentoRichiesta = 5 Or ClasseAccreditamentoRichiesta = 7 Then
                    'trovo chi e' il padre e lo metto in sessione 
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If


                    strSQL = "Select  identepadre, enti.denominazione, isnull((Select descrizione from regionicompetenze where regionicompetenze.idregionecompetenza = enti.idregionecompetenza),'Nessuna') as regione FROM  entirelazioni "
                    strSQL = strSQL & " INNER JOIN enti ON entirelazioni.IDEntePadre = enti.IDEnte "
                    strSQL = strSQL & " where identefiglio='" & e.Item.Cells(13).Text & "'"

                    'controllo se utente o ente regionale
                    'eseguo la query'
                    dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
                    dtrGenerico.Read()

                End If


                If Session("TipoUtente") = "U" Then
                    Context.Items.Add("identesede", e.Item.Cells(12).Text)
                    Context.Items.Add("idente", e.Item.Cells(13).Text)
                    Context.Items.Add("Ente", e.Item.Cells(3).Text)
                    Context.Items.Add("acquisita", e.Item.Cells(14).Text)
                    Context.Items.Add("tipoazione", "Modifica")
                    Context.Items.Add("stato", e.Item.Cells(1).Text)
                    Context.Items.Add("page", dgRisultatoRicerca.CurrentPageIndex)
                    'Context.Items.Add("strsql", txtstrsql.Text)
                    'Mettere in sessione L'ente
                    If ClasseAccreditamentoRichiesta = 5 Or ClasseAccreditamentoRichiesta = 7 Then
                        Session("IdEnte") = dtrGenerico("identepadre")
                        Session("Denominazione") = dtrGenerico("denominazione")
                        Session("Competenza") = dtrGenerico("regione")

                    Else
                        Session("IdEnte") = e.Item.Cells(13).Text
                        Session("Denominazione") = e.Item.Cells(3).Text
                        Session("Competenza") = e.Item.Cells(18).Text
                    End If
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    'Response.Redirect("WfrmAnagraficaSedi.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti"))
                    Response.Redirect("WfrmAnagraficaSedi.aspx?identesede=" & identesede & "&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&idente=" & idente & "&acquisita=" & acquisita & "&stato=" & stato)
                    'Server.Transfer("WfrmAnagraficaSedi.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti"))
                Else

                    If e.Item.Cells(18).Text = e.Item.Cells(19).Text Then 'competenza e' uguale a competenza di chi si e' loggato allora
                        'richiamo la Pagina di Gestione della Sede inviando valori di Selezione
                        Context.Items.Add("identesede", e.Item.Cells(12).Text)
                        Context.Items.Add("idente", e.Item.Cells(13).Text)
                        Context.Items.Add("Ente", e.Item.Cells(3).Text)
                        Context.Items.Add("acquisita", e.Item.Cells(14).Text)
                        Context.Items.Add("tipoazione", "Modifica")
                        Context.Items.Add("stato", e.Item.Cells(1).Text)
                        Context.Items.Add("page", dgRisultatoRicerca.CurrentPageIndex)
                        'Context.Items.Add("strsql", txtstrsql.Text)
                        'Mettere in sessione L'ente
                        If ClasseAccreditamentoRichiesta = 5 Or ClasseAccreditamentoRichiesta = 7 Then
                            Session("IdEnte") = dtrGenerico("identepadre")
                            Session("Denominazione") = dtrGenerico("denominazione")
                            Session("Competenza") = dtrGenerico("regione")

                        Else
                            Session("IdEnte") = e.Item.Cells(13).Text
                            Session("Denominazione") = e.Item.Cells(3).Text
                            Session("Competenza") = e.Item.Cells(18).Text
                        End If

                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        'Response.Redirect("WfrmAnagraficaSedi.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti"))
                        Response.Redirect("WfrmAnagraficaSedi.aspx?identesede=" & identesede & "&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&idente=" & idente & "&acquisita=" & acquisita & "&stato=" & stato)
                        'Server.Transfer("WfrmAnagraficaSedi.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti"))
                    Else
                        lblmessaggio.Visible = True
                        lblmessaggio.Text = "Attenzione. La sede non è di propria competenza."

                        Exit Sub

                    End If
                End If
        End Select



    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        If Not checkFiltroFase() Then Exit Sub
        RicercaSedi()
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub

    Private Sub dgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
      
    End Sub
    Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                CboCompetenza.DataSource = dtrCompetenze
                CboCompetenza.Items.Add("")
                CboCompetenza.DataTextField = "Descrizione"
                CboCompetenza.DataValueField = "IDRegioneCompetenza"
                CboCompetenza.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                CboCompetenza.Enabled = True
                CboCompetenza.SelectedIndex = 0
            Else
                'CboCompetenza.SelectedIndex = 1
                'CboCompetenza.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    CboCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        CboCompetenza.Enabled = True
                    End If

                End If

            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub
    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To DTBRicerca.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function
End Class