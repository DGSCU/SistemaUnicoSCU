Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Text.RegularExpressions

Public Class WfrmRicercaSede
    Inherits System.Web.UI.Page
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim strsql As String
    Dim blnRicerca As Boolean
    Public blnForza As Boolean
    Const INDEX_DGRISULTATORICERCA_STATO As Byte = 1
    Const INDEX_DGRISULTATORICERCA_SEDE As Byte = 2
    Const INDEX_DGRISULTATORICERCA_ENTE As Byte = 3
    Const INDEX_DGRISULTATORICERCA_TIPOSEDE As Byte = 4
    Const INDEX_DGRISULTATORICERCA_NSEDI As Byte = 5
    Const INDEX_DGRISULTATORICERCA_CODICESEDE As Byte = 6
    Const INDEX_DGRISULTATORICERCA_INDIRIZZO As Byte = 7
    Const INDEX_DGRISULTATORICERCA_CAP As Byte = 8
    Const INDEX_DGRISULTATORICERCA_COMUNE As Byte = 9
    Const INDEX_DGRISULTATORICERCA_TELEFONO As Byte = 10
    Const INDEX_DGRISULTATORICERCA_EMAIL As Byte = 11
    Const INDEX_DGRISULTATORICERCA_IDENTESEDE As Byte = 12
    Const INDEX_DGRISULTATORICERCA_IDENTE As Byte = 13
    Const INDEX_DGRISULTATORICERCA_ACQUISITA As Byte = 14
    Const INDEX_DGRISULTATORICERCA_INCLUSA As Byte = 15
    Const INDEX_DGRISULTATORICERCA_DATAINSERIMENTO As Byte = 16
    Const INDEX_DGRISULTATORICERCA_CERTIFICAZIONE As Byte = 17
    Const INDEX_DGRISULTATORICERCA_SANZIONE As Byte = 18
    Const INDEX_DGRISULTATORICERCA_VERIFICA As Byte = 19
    Const INDEX_DGRISULTATORICERCA_ANOMALIE As Byte = 20
    Const INDEX_DGRISULTATORICERCA_ANOMALIA_NOME As Byte = 21
    Const INDEX_DGRISULTATORICERCA_ANOMALIA_INDIRIZZO As Byte = 22
    Const INDEX_DGRISULTATORICERCA_ANOMALIA_INDIRIZZO_GOOGLE As Byte = 23



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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        Dim dtrGenericoLocal As SqlClient.SqlDataReader

        If IsPostBack = False Then
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                divCertificazione.Visible = True
            Else
                divCertificazione.Visible = False
            End If

            'visibilità filtroFase
            If Session("TipoUtente") = "U" Then
                lblFiltroFase.Visible = True
                txtFiltroFase.Visible = True
            Else
                lblFiltroFase.Visible = False
                txtFiltroFase.Visible = False
            End If

            Try
                'Ilaria Lombardi 10/11/09 gli passo anhce il codice regione quando vengo dalla maschera degli enti in accordo
                If Request.QueryString("codiceente") <> "" Then
                    TxtCodiceRegione.Text = Request.QueryString("codiceente")
                End If

                ddlTipologia.Items.Add("")

                ChiudiDataReader(dtrGenericoLocal)
                ChiudiDataReader(dtrGenerico)
                lblpage.Value = IIf(Not IsNothing(Context.Items("page")), Context.Items("page"), "")
                txtstrsql.Value = IIf(Not IsNothing(Context.Items("strsql")), Context.Items("strsql"), "")

                dtrGenericoLocal = ClsServer.CreaDatareader("Select tiposede,idtiposede from tipiSedi order by idtiposede desc ", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                Do While dtrGenericoLocal.Read()
                    ddlTipologia.Items.Add(dtrGenericoLocal("tiposede"))
                Loop

                ddlTipologia.SelectedIndex = 1
                ddlTipologia.Enabled = False
                ChiudiDataReader(dtrGenericoLocal)
                ddlstato.Items.Add("")
                dtrGenericoLocal = ClsServer.CreaDatareader("select statoEnteSede from StatiEntiSedi", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                Do While dtrGenericoLocal.Read()
                    ddlstato.Items.Add(dtrGenericoLocal("statoEnteSede"))
                Loop
                ChiudiDataReader(dtrGenericoLocal)


                If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
                    txtDenominazioneEnte.Text = Request.QueryString("DenominazioneEnte")
                    lblpage.Value = String.Empty
                    lblErrore.Text = String.Empty
                    RicercaSedi()
                End If
                If Request.QueryString("CheckProvenienza") = "RicercaEnteInAccordo" Then
                    txtDenominazioneEnte.Enabled = False
                    TxtCodiceRegione.Enabled = False
                End If
            Catch ex As Exception
                ex.Message.ToString()
            End Try
        End If
        'ADC-------------------------------------------------------
        'If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
        '    txtDenominazioneEnte.Text = Request.QueryString("DenominazioneEnte")
        '    lblpage.Value = String.Empty
        '    lblErrore.Text = String.Empty
        '    RicercaSedi()
        'End If
        '--------------------------------------------------------------
        '***  agg. da simona cordella il 07/07/2011
        'se l'utene NON è abilitato al menu FORZA PRESENZA SANZIONE 
        'nn visualizzo il filtro di ricerca (aspx) Presenza Sanzione
        blnForza = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        If (blnForza) Then
            divForzaSanzione.Visible = True
        Else
            divForzaSanzione.Visible = False
        End If
    End Sub

#Region "Eventi"
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click

        If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
            Response.Redirect("assegnazionevincoliprogetti.aspx?idattivita=" & Request.QueryString("idattivita") & "&tipologia=" & Request.QueryString("tipologia") & "&Nazionale=" & Request.QueryString("Nazionale"))
        Else
            If Not Request.QueryString("VengoDa") Is Nothing Then
                'controllo se la variabile è valorizzata
                If Request.QueryString("VengoDa") <> String.Empty Then
                    'faccio la response.redirect verso l'albero
                    Response.Redirect(Request.QueryString("VengoDa").ToString)
                End If
            End If
            If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
                If Request.QueryString("CheckProvenienza") = "VisualizzazioneEntiInAccordo" Then
                    Response.Redirect("WfrmElencoEntiAccordo.aspx?Pagina=" & Request.QueryString("Pagina") & "&CheckProvenienza=" & Request.QueryString("CheckProvenienza") & "&Denominazione=" & Request.QueryString("Denominazione") & "&idente=" & Request.QueryString("idente") & "&DenominazioneEnte=" & Request.QueryString("DenomonazioneEnte"))
                Else
                    Response.Redirect("WfrmRicEnteinAccordo.aspx?Pagina=" & Request.QueryString("Pagina") & "&CheckProvenienza=" & Request.QueryString("CheckProvenienza") & "&Stato=" & Request.QueryString("Stato") & "&CF=" & Request.QueryString("CF") & "&ClasseAccreditamento=" & Request.QueryString("ClasseAccreditamento") & "&Tipologia=" & Request.QueryString("Tipologia") & "&CodiceRegione=" & Request.QueryString("CodicerRegione") & "&Denominazione=" & Request.QueryString("Denominazione") & "&VediEnte=1&DenominazioneEnte=" & Request.QueryString("DenomonazioneEnte"))
                End If
            Else
                Response.Redirect("WfrmMain.aspx")
            End If
        End If
    End Sub
    Public Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        lblpage.Value = String.Empty
        lblErrore.Text = String.Empty
        RicercaSedi()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Dim identesede As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_IDENTESEDE).Text
                Dim idente As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_IDENTE).Text
                Dim acquisita As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_ACQUISITA).Text
                Dim stato As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_STATO).Text

                Context.Items.Add("Ente", e.Item.Cells(INDEX_DGRISULTATORICERCA_ENTE).Text)
                Context.Items.Add("tipoazione", "Modifica")
                Context.Items.Add("stato", e.Item.Cells(INDEX_DGRISULTATORICERCA_STATO).Text)
                Context.Items.Add("page", dgRisultatoRicerca.CurrentPageIndex)
                Response.Redirect("WfrmAnagraficaSedi.aspx?identesede=" & identesede & "&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&idente=" & idente & "&acquisita=" & acquisita & "&stato=" & stato)
        End Select
    End Sub

    Function checkFiltroFase() As Boolean
        lblErroreFiltroFase.Visible = False
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) AndAlso Not Integer.TryParse(txtFiltroFase.Text, Nothing) Then
            lblErroreFiltroFase.Visible = True
            Return False
        End If
        Return True
    End Function

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        If Not checkFiltroFase() Then Exit Sub
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        lblpage.Value = e.NewPageIndex
        'CaricaDataGrid(dgRisultatoRicerca)
        RicercaSedi()
        controllaChek()
    End Sub
#End Region
#Region "Funzionalita"
    Private Sub RicercaSedi()
        If Not checkFiltroFase() Then Exit Sub

        strsql = "select distinct 0 ordina,enti.denominazione as ente,enti.codiceregione as CodiceEnte,entisedi.denominazione as sede," &
        " 'propria' acquisita,enti.idente, " &
        " statosedefisica.statoentesede as stato," &
        " entisedi.idEntesede,tipisedi.tiposede, entisedi.CodicesedeEnte as " &
        " Codicesede,entisedi.indirizzo + ' ' + entisedi.Civico as Indirizzo, " &
        " entisedi.cap,case IsNull(comuni.ComuneNazionale,0) when 1 then comuni.denominazione +' ['+ provincie.provincia +']' when 0 then IsNull(entisedi.CittaEstera,'') +' ['+ provincie.provincia +']' end as Comune," &
        " entisedi.prefissotelefono + '/' + entisedi.telefono as telefono, entisedi.Email," &
        " entisediattuazioni.identesedeattuazione as Nsedi, " &
        " statientisedi.ordine, " &
        " case isnull(entisediattuazioni.Datainserimento,0) when 0 then entisedi.DataCreazioneRecord else entisediattuazioni.Datainserimento end as DataCreazioneRecord, " &
        " entisediattuazioni.identesedeattuazione," &
        " case isnull(entisediattuazioni.Certificazione,2) when 0 then 'No' when 1 then 'Si' when 2 then 'Da Valutare' end as Certificazione, " &
        " Case isnull(entisediattuazioni.Segnalazione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp onclick=VisualizzaSanzione('+ convert(varchar, entisediattuazioni.IDEnteSedeAttuazione) + ','+ convert(varchar, enti.IDEnte) + ') STYLE=cursor:hand title=Sanzione border=0>' End as Segnalazione,  " &
        " Case isnull(entisediattuazioni.SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica " &
        " ,isnull(entisediattuazioni.nmaxvolontari,0) as nmaxvolontari, entiSedi.AnomaliaIndirizzo, entiSedi.AnomaliaNome, entiSedi.AnomaliaIndirizzoGoogle" &
        " ,case isnull(entisedi.NormativaTutela,0) when 0 then 'No' when 1 then 'Si' end as Normativa81" &
        " ,case (comuni.ComuneNazionale) when 0 then case TitoliGiuridici.IdTitoloGiuridico when 7 then '' else Case isnull(entisedi.DisponibilitaSede,2) when 0 then 'CON' when 1 then 'ATT' when 2 then '' end end when 1 then '' End  as Conformita" &
        " ,ISNULL(entisedi.SoggettoEstero,'') as Soggettoestero" &
        " ,case (comuni.ComuneNazionale) when 0 then case IsNull(TitoliGiuridici.IdTitoloGiuridico,0) when 7 then case isnull(entisedi.NonDisponibilitaSede,2) when 0 then 'CON' when 1 then 'ATT' else '' end else '' end else '' end as DichSoggettoEstero" &
        " ,TitoliGiuridici.DescrizioneAbbreviata as TitoloPossedimento" &
        " from enti " &
        " inner join entisedi on(entisedi.idente=enti.idEnte) " &
        " left  join entisediattuazioni on entisediattuazioni.identesede=entisedi.identesede " &
        " left  join statientisedi statosedefisica on(entisediattuazioni.idstatoentesede=statosedefisica.idstatoEntesede) " &
        " inner  join statientisedi on(entisedi.idstatoentesede=statientisedi.idstatoEntesede) " &
        " inner join comuni on (entisedi.idcomune=Comuni.idcomune)  " &
        " inner join Provincie on (provincie.idprovincia=Comuni.idProvincia)  " &
        " INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione " &
        " inner join  Entiseditipi on (entisedi.identesede=Entiseditipi.idEntesede)  " &
        " inner join tipisedi on (tipisedi.idtiposede=Entiseditipi.idtiposede) " &
        " inner join TitoliGiuridici on TitoliGiuridici.IdTitoloGiuridico = entisedi.IdTitoloGiuridico"

        'la validità del FiltroFase è stata controllata prima
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
            strsql = strsql & " left join EntiFasi_Sedi efs on efs.IdEnteSedeAttuazione=entisediattuazioni.IdEnteSedeAttuazione "
        End If

        strsql = strsql & " where entisedi.idente = " & Session("idEnte") & " and substring(entisedi.usernamestato,1,1) <> 'N' "

        If Request.QueryString("CheckProvenienza") = "RicercaEnteInAccordo" Then

            strsql = strsql & " and enti.idente=" & Request.QueryString("IdEnteFiglio") & ""

        Else


            If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
                strsql = strsql & " and enti.denominazione LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%'"
            End If
            If Trim(TxtCodiceRegione.Text) <> vbNullString Then
                strsql = strsql & " and enti.codiceregione = '" & Replace(TxtCodiceRegione.Text, " '", "''") & "'"
            End If
        End If

        If Trim(txtdenominazione.Text) <> "" Then
            strsql = strsql & " and entisedi.denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodice.Value) <> "" Then
            strsql = strsql & " and entisedi.codiceSedeEnte='" & Replace(txtCodice.Value, "'", "''") & "'"
        End If
        If Not IsNothing(ddlTipologia.SelectedItem.Text) Then
            If Trim(ddlTipologia.SelectedItem.Text) <> "" Then
                strsql = strsql & " and tipisedi.tiposede='" & Replace(ddlTipologia.SelectedItem.Text, "'", "''") & "'"
            End If
        End If
        If ddlstato.SelectedItem.Text <> "" Then
            'strsql = strsql & " and  statientisedi.statoentesede='" & ddlstato.SelectedItem.Text & "'"
            strsql = strsql & " and case isnull(statosedefisica.statoentesede,'vuoto') when 'vuoto' then statientisedi.statoentesede else  statosedefisica.statoentesede end ='" & ddlstato.SelectedItem.Text & "'"
        End If
        If Trim(txtregione.Text) <> vbNullString Then
            strsql = strsql & " and Regioni.Regione LIKE '" & Replace(txtregione.Text, "'", "''") & "%'"
        End If
        If Trim(txtProvincia.Text) <> vbNullString Then
            strsql = strsql & " and Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            strsql = strsql & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If

        If Trim(txtIndirizzo.Text) <> vbNullString Then
            strsql = strsql & " and (entisedi.indirizzo + ' ' + entisedi.Civico) LIKE '" & Replace(txtIndirizzo.Text, "'", "''") & "%'"
        End If

        If chkRiferimentoRimborsi.Checked = True Then
            strsql = strsql & " and EntiSedi.RiferimentoRimborsi = 1"
        End If

        Dim primo As Integer
        primo = Integer.TryParse(Trim(txtCodSedeAtt.Text), primo)
        If Trim(txtCodSedeAtt.Text) <> "" Then
            If primo = -1 Then
                strsql = strsql & " and EntiSediAttuazioni.identesedeattuazione=" & Trim(txtCodSedeAtt.Text) & ""
            Else
                lblErrore.Text = "Inserire Valore Numerico"
                primo = 1
            End If
        End If


        If ddlCertificazione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and isnull(entisediattuazioni.Certificazione,2) =" & ddlCertificazione.SelectedValue
        End If
        If ddlDuplicati.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and dbo.DoppioneSede(EntiSediAttuazioni.identesedeattuazione)=" & ddlDuplicati.SelectedValue
        End If
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If
        If ddlLocalizzazione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and comuni.ComuneNazionale = " & ddlLocalizzazione.SelectedValue
        End If
        If ddlRichistaVariazione.SelectedValue <> "" Then
            strsql = strsql & " and isnull(entisedi.RichiestaModifica,0)=" & ddlRichistaVariazione.SelectedValue & " "
        End If

        'la validità del FiltroFase è stata controllata prima
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
            strsql = strsql & " and efs.IdEnteFase =" & Trim(txtFiltroFase.Text)
        End If

        strsql = strsql & " union " &
        " select distinct 1 ordina," &
        " enti.denominazione  as ente,enti.codiceregione as CodiceEnte,entisedi.denominazione as sede," &
        " case isnull(associaentirelazionisedi.idassociaentirelazionisedi,-1) when -1 then 'no' else convert(varchar(10),associaentirelazionisedi.idassociaentirelazionisedi) end acquisita," &
        " enti.idente, " &
        " statosedefisica.statoentesede  as stato," &
        " entisedi.idEntesede,tipisedi.tiposede, entisedi.CodicesedeEnte as " &
        " Codicesede,entisedi.indirizzo + ' ' + entisedi.Civico as Indirizzo, " &
        " entisedi.cap,case IsNull(comuni.ComuneNazionale,0) when 1 then comuni.denominazione +' ['+ provincie.provincia +']' when 0 then IsNull(entisedi.CittaEstera,'') +' ['+ provincie.provincia +']' end as Comune," &
        " entisedi.prefissotelefono + '/' + entisedi.telefono as telefono, entisedi.Email," &
        " entisediattuazioni.identesedeattuazione as Nsedi, " &
        " statientisedi.ordine, " &
        " case isnull(entisediattuazioni.Datainserimento,0) when 0 then entisedi.DataCreazioneRecord else entisediattuazioni.Datainserimento end as DataCreazioneRecord, " &
        " entisediattuazioni.identesedeattuazione,case isnull(entisediattuazioni.Certificazione,2) when 0 then 'No' when 1 then 'Si' when 2 then 'Da Valutare' end as Certificazione, " &
        " Case isnull(entisediattuazioni.Segnalazione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp onclick=VisualizzaSanzione('+ convert(varchar, entisediattuazioni.IDEnteSedeAttuazione) + ','+ convert(varchar, enti.IDEnte) + ') STYLE=cursor:hand title=Sanzione border=0>' End as Segnalazione,  " &
        " Case isnull(entisediattuazioni.SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica " &
        " ,isnull(entisediattuazioni.nmaxvolontari,0) as nmaxvolontari, entiSedi.AnomaliaIndirizzo, entiSedi.AnomaliaNome, entiSedi.AnomaliaIndirizzoGoogle" &
        " ,case isnull(entisedi.NormativaTutela,0) when 0 then 'No' when 1 then 'Si' end as Normativa81" &
        " ,case (comuni.ComuneNazionale) when 0 then case TitoliGiuridici.IdTitoloGiuridico when 7 then '' else Case isnull(entisedi.DisponibilitaSede,2) when 0 then 'CON' when 1 then 'ATT' when 2 then '' end end when 1 then '' End  as Conformita" &
        " ,ISNULL(entisedi.SoggettoEstero,'') as Soggettoestero" &
        " ,case (comuni.ComuneNazionale) when 0 then case IsNull(TitoliGiuridici.IdTitoloGiuridico,0) when 7 then case isnull(entisedi.NonDisponibilitaSede,2) when 0 then 'CON' when 1 then 'ATT' else '' end else '' end else '' end as DichSoggettoEstero" &
        " ,TitoliGiuridici.DescrizioneAbbreviata as TitoloPossedimento" &
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
        " left join associaentirelazionisedi on (entisedi.identesede=associaentirelazionisedi.identesede " &
        " and entirelazioni.identerelazione=associaentirelazionisedi.identerelazione)" &
        " inner join TitoliGiuridici on TitoliGiuridici.IdTitoloGiuridico = entisedi.IdTitoloGiuridico"

        'la validità del FiltroFase è stata controllata prima
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
            strsql = strsql & " left join EntiFasi_Sedi efs on efs.IdEnteSedeAttuazione=entisediattuazioni.IdEnteSedeAttuazione "
        End If

        strsql = strsql & " where entirelazioni.identePadre=" & Session("idEnte") & "  and substring(entisedi.usernamestato,1,1) <> 'N'" 'and entirelazioni.datafinevalidità is  null
        If Request.QueryString("CheckProvenienza") = "RicercaEnteInAccordo" Then

            strsql = strsql & " and enti.idente=" & Request.QueryString("IdEnteFiglio") & ""

        Else


            If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
                strsql = strsql & " and enti.denominazione LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%'"
            End If
            If Trim(TxtCodiceRegione.Text) <> vbNullString Then
                strsql = strsql & " and enti.codiceregione = '" & Replace(TxtCodiceRegione.Text, " '", "''") & "'"
            End If
        End If

        If Trim(txtdenominazione.Text) <> "" Then
            strsql = strsql & " and entisedi.denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodice.Value) <> "" Then
            strsql = strsql & " and entisedi.codiceSedeEnte='" & Replace(txtCodice.Value, "'", "''") & "'"
        End If
        If Trim(ddlTipologia.SelectedItem.Text) <> "" Then
            strsql = strsql & " and tipisedi.tiposede='" & Replace(ddlTipologia.SelectedItem.Text, "'", "''") & "'"
        End If
        If ddlstato.SelectedItem.Text <> "" Then
            '            strsql = strsql & " and  statientisedi.statoentesede='" & ddlstato.SelectedItem.Text & "'"
            strsql = strsql & " and case isnull(statosedefisica.statoentesede,'vuoto') when 'vuoto' then statientisedi.statoentesede else  statosedefisica.statoentesede end ='" & ddlstato.SelectedItem.Text & "'"
        End If
        If Trim(txtregione.Text) <> vbNullString Then
            strsql = strsql & " and Regioni.Regione LIKE '" & Replace(txtregione.Text, "'", "''") & "%'"
        End If
        If Trim(txtProvincia.Text) <> vbNullString Then
            strsql = strsql & " and Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            strsql = strsql & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If
        'If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
        '    If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
        '        strsql = strsql & " and enti.denominazione = '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "'"
        '    End If
        'Else
        '    If Trim(txtDenominazioneEnte.Text) <> vbNullString Then
        '        strsql = strsql & " and enti.denominazione LIKE '" & Replace(txtDenominazioneEnte.Text, "'", "''") & "%'"
        '    End If
        'End If
        'If Trim(TxtCodiceRegione.Text) <> vbNullString Then
        '    strsql = strsql & " and enti.codiceregione = '" & Replace(TxtCodiceRegione.Text, " '", "''") & "'"
        'End If
        If Trim(txtIndirizzo.Text) <> vbNullString Then
            strsql = strsql & " and (entisedi.indirizzo + ' ' + entisedi.Civico) LIKE '" & Replace(txtIndirizzo.Text, "'", "''") & "%'"
        End If
        If chkRiferimentoRimborsi.Checked = True Then
            strsql = strsql & " and EntiSedi.RiferimentoRimborsi = 1 "
        End If

        Dim secondo As Integer
        secondo = Integer.TryParse(Trim(txtCodSedeAtt.Text), secondo)
        If Trim(txtCodSedeAtt.Text) <> "" Then
            If secondo = -1 Then
                strsql = strsql & " and EntiSediAttuazioni.identesedeattuazione=" & Trim(txtCodSedeAtt.Text) & ""
            Else
                lblErrore.Text = "Inserire Valore Numerico"
                secondo = 1
            End If
        End If

        'agg. da sc il 05/12/2008
        If ddlCertificazione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and isnull(entisediattuazioni.Certificazione,0) =" & ddlCertificazione.SelectedValue
        End If
        If ddlLocalizzazione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and comuni.ComuneNazionale = " & ddlLocalizzazione.SelectedValue
        End If
        'agg. da sc il 05/07/2011 combo su segnalazione della sede
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue & ""
        End If
        '*** Aggiunto da Simona Cordella il 15/09/2014
        'filtro per RichiestaVariazione
        If ddlRichistaVariazione.SelectedValue <> "" Then
            strsql = strsql & " and isnull(entisedi.RichiestaModifica,0)=" & ddlRichistaVariazione.SelectedValue & " "
        End If
        '***

        'la validità del FiltroFase è stata controllata prima
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
            strsql = strsql & " and efs.IdEnteFase =" & Trim(txtFiltroFase.Text)
        End If

        'agg. da sc il 09/06/2009 trovo se si sono sedi con lo stesso indirizzo,civico, comune,cap 
        If ddlDuplicati.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and dbo.DoppioneSede(EntiSediAttuazioni.identesedeattuazione)=" & ddlDuplicati.SelectedValue
            strsql = strsql & " order by comune,indirizzo "
        Else
            strsql = strsql & " order by ordina,statientisedi.ordine, ente,sede,acquisita"
        End If


        If (primo = -1 And secondo = -1) Or (primo = 0 And secondo = 0) Then

            dtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If txtRicerca.Value <> "" Then
                dgRisultatoRicerca.CurrentPageIndex = 0
                txtRicerca.Value = ""
            End If
            If lblpage.Value <> "" Then
                dgRisultatoRicerca.CurrentPageIndex = CInt(lblpage.Value)
            Else
                dgRisultatoRicerca.CurrentPageIndex = 0
            End If

            'txtstrsql.Value = strsql
            CaricaDataGrid(dgRisultatoRicerca)
            If (dgRisultatoRicerca.Items.Count > 0) Then
                CmdEsporta.Visible = True
                ApriCSV1.Visible = False
                dgRisultatoRicerca.Caption = "Risultato Ricerca Sedi Ente"
            Else
                CmdEsporta.Visible = False
                ApriCSV1.Visible = False
                dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato"
            End If
            controllaChek()

        End If
    End Sub
    
    Sub CaricaDataGrid(ByRef datagrid As DataGrid) 'valorizzo la datagrid passata
        'Verifico l'abilitazione alla visualizzazione di determinate colonne
        Dim strMess As String
        If Session("TipoUtente") = "E" Then
            datagrid.Columns(16).Visible = False
            ' agg da simona cordella il 11/12/2008
            ' se sono un utente di tipo E rendo la colonna invisibile certificazione invisibile
            datagrid.Columns(17).Visible = False

        End If
        datagrid.Columns(18).Visible = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        datagrid.Columns(19).Visible = ClsUtility.ForzaSedeSottopostaVerifica(Session("Utente"), Session("conn"))

        datagrid.DataSource = dtsGenerico
        datagrid.DataBind()


        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(14) As String
        Dim NomiCampiColonne(14) As String
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
        NomeColonne(8) = "NMaxVolontari"
        NomeColonne(9) = "Titolo di Possedimento"
        NomeColonne(10) = "Normativa81"
        NomeColonne(11) = "Conformita"
        NomeColonne(12) = "Soggetto Estero"
        NomeColonne(13) = "Dichiarazione di Soggetto Estero"


        If Session("TipoUtente") <> "E" Then
            NomeColonne(14) = "Presenza Iscrizione"
        End If


        NomiCampiColonne(0) = "Stato"
        NomiCampiColonne(1) = "Sede"
        NomiCampiColonne(2) = "Ente"
        NomiCampiColonne(3) = "Tiposede"
        NomiCampiColonne(4) = "NSedi"
        NomiCampiColonne(5) = "Indirizzo"
        NomiCampiColonne(6) = "Comune"
        NomiCampiColonne(7) = "Telefono"
        NomiCampiColonne(8) = "NMaxVolontari"
        NomiCampiColonne(9) = "TitoloPossedimento"
        NomiCampiColonne(10) = "Normativa81"
        NomiCampiColonne(11) = "Conformita"
        NomiCampiColonne(12) = "Soggettoestero"
        NomiCampiColonne(13) = "DichSoggettoEstero"
        If Session("TipoUtente") <> "E" Then
            NomiCampiColonne(14) = "Certificazione"
        End If

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        If Session("TipoUtente") <> "E" Then
            CaricaDataTablePerStampa(dtsGenerico, 14, NomeColonne, NomiCampiColonne)
        Else
            CaricaDataTablePerStampa(dtsGenerico, 13, NomeColonne, NomiCampiColonne)
        End If
        'CaricaDataTablePerStampa(dtsGenerico, 8, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        If datagrid.Items.Count = 0 Then

            If Request.QueryString("esporta") = "si" Then
                CmdEsporta.Visible = False
            End If
        Else
            ColoraCelle()
            'ControllaSede()
            If Request.QueryString("esporta") = "si" Then
                CmdEsporta.Visible = True
                datagrid.Columns(0).Visible = False
            End If
        End If
        If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
            datagrid.Columns(0).Visible = False
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


    Private Sub controllaChek()
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(14).Text
                Case "no"
                    check.Checked = False
                Case "propria"
                    check.Visible = False
                Case Else
                    check.Checked = True
            End Select
            check.Enabled = False
        Next
    End Sub
    Private Sub ColoraCelle()

        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim intConta As Integer
        Dim img As ImageButton
        For Each item In dgRisultatoRicerca.Items
            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(1).Text
                Case "Accreditata"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.LightGreen
                    Next
                    'aggiunto il 04/12/2008 da simona cordella
                    'se sono un utene U o R coloro il font della griglio in viola sono le sede è da valutare
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        'se la sede è da valutare coloro il FONT DI VIOLA
                        Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text
                            Case "Da Valutare"
                                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.FromArgb(201, 96, 185)
                                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).Font.Bold = True
                                Next
                            Case "No"
                                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                                Next
                        End Select
                    End If

                Case "Presentata"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Khaki
                    Next
                    'aggiunto il 04/12/2008 da simona cordella
                    'se sono un utene U o R coloro il font della griglio in viola sono le sede è da valutare
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        'se la sede è da valutare coloro il FONT DI VIOLA
                        Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text
                            Case "Da Valutare"
                                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.FromArgb(201, 96, 185)
                                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).Font.Bold = True
                                Next
                            Case "No"
                                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                                Next
                        End Select
                    End If
                Case "Presentata (*)"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Khaki
                    Next
                Case "Sospesa"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Gainsboro
                    Next
                Case "Cancellata"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                    Next
                Case "Cancellata (*)"
                    For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                    Next
            End Select

            img = DirectCast(item.FindControl("IdImgAlert"), ImageButton)
            img.Visible = False
            img.ToolTip = ""
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_ANOMALIA_NOME).Text = "True" Then
                img.Visible = True
                img.ToolTip = "Il nome presenta anomalie o è duplicato" + vbNewLine
            End If
            'If dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_ANOMALIA_INDIRIZZO).Text = "True" Then
            '    img.Visible = True
            '    img.ToolTip = img.ToolTip + "Indirizzo corrispondente ad altra sede" + vbNewLine
            'End If
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_ANOMALIA_INDIRIZZO_GOOGLE).Text = "True" Then
                img.Visible = True
                img.ToolTip = img.ToolTip + "Indirizzo non corrispondente a quello trovato da Google"
            End If
            img.Enabled = False
        Next
    End Sub
#End Region

    Private Sub Includi_tutte_sedi()
        'Creata da Alessandra Taballione il 29/07/2004
        'Per facilitare l'Utente UNSC l'Inserimento dei dati 
        Dim cmdGenerico As SqlClient.SqlCommand
        Dim MyRow As DataRow
        Dim datat As DataTable
        Dim datat2 As DataTable
        Dim MyRow2 As DataRow
        'Realizzato da Alessandra Taballione il 10/03/2004
        'inclusione della sede
        'Inserimento della relazione PadreFiglio
        strsql = "select 1 ordina, entisedi.identesede,entirelazioni.identefiglio,entirelazioni.identePadre " & _
        " from enti " & _
        " inner join entisedi on(entisedi.idente=enti.idEnte)  " & _
        " inner join statientisedi on(entisedi.idstatoentesede=statientisedi.idstatoEntesede)  " & _
        " inner join  Entiseditipi on (entisedi.identesede=Entiseditipi.idEntesede) " & _
        " inner join tipisedi on (tipisedi.idtiposede=Entiseditipi.idtiposede) " & _
        " inner join entirelazioni on (entirelazioni.identefiglio=enti.idente)  " & _
        " left join associaentirelazionisedi on (entisedi.identesede=associaentirelazionisedi.identesede  " & _
        " and entirelazioni.identerelazione=associaentirelazionisedi.identerelazione) " & _
        " where(entirelazioni.identePadre = " & Session("idEnte") & " And entirelazioni.datafinevalidità Is null And associaentirelazionisedi.idassociaentirelazionisedi Is null)" & _
        " and (statientisedi.attiva=1 or statientisedi.daaccreditare=1) and tipisedi.idtiposede = 4 order by entisedi.identesede"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        datat = ClsServer.CreaDataTable(strsql, False, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        For Each MyRow In datat.Rows
            strsql = "insert into associaentirelazionisedi(idEnteSede,identeRelazione,DataCreazionerecord)" & _
            " select  " & MyRow.Item("identesede") & " ,identerelazione,getdate() from entirelazioni where identePadre= " & Session("IdEnte") & "  and identeFiglio=" & MyRow.Item("identeFiglio") & ""
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            'Aggiunto da Alessandra Taballione il 27.07.04
            'Insererimento della inclusione per tuute le sedi di attuazione della sede
            strsql = "Select entisediattuazioni.identeSedeAttuazione " & _
            " from entisediattuazioni " & _
            " inner join statientisedi on (entisediattuazioni.idstatoentesede=statientisedi.idstatoentesede)" & _
            " Where identesede=" & MyRow.Item("identesede") & " And (statientisedi.attiva = 1 Or statientisedi.Defaultstato = 1)"
            'strsql = "Select * from entisediattuazioni where identesede=" & MyRow.Item("identesede") & ""
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            datat2 = ClsServer.CreaDataTable(strsql, False, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            For Each MyRow2 In datat2.Rows
                strsql = "insert into associaentirelazionisediAttuazioni(idEnteSedeattuazione,identeRelazione,DataCreazionerecord)" & _
               " select  " & MyRow2.Item("identeSedeAttuazione") & " ,identerelazione,getdate() from entirelazioni where identePadre= " & Session("IdEnte") & "  and identeFiglio=" & MyRow.Item("identeFiglio") & ""
                cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            Next
        Next
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'If txtstrsql.Value <> "" Then
        '    RicercaSediStrsql(txtstrsql.Value)
        'Else
        RicercaSedi()
        'End If
    End Sub

    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
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
    Private Sub ControllaSede()
        Dim indirizzo1, indirizzo2, denominazione1, denominazione2, sede, comune, cap, strApp, strSql As String
        Dim chkIndirizzo, chkSede, chkRipetizione As Boolean
        Dim idEnte, idsede As Integer
        Dim strArray
        Dim messaggio As String = ""
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim tRisultati = New DataTable
        Dim Img As ImageButton
        Dim rgx As Regex = New Regex("[0-9]")
        'strsql = "Select IdEnteSede, entisedi.indirizzo + ' ' + entisedi.Civico As IndirizzoCompleto"
        'strsql += " from entisedi inner join comuni on (entisedi.idcomune=Comuni.idcomune) inner join Provincie on (provincie.idprovincia=Comuni.idProvincia) INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione"
        'strsql += "where (entisedi.indirizzo + ' ' + entisedi.Civico) = '" & indirizzo & "' and EntiSedi.CAP='" & cap & "' and (comuni.denominazione +' ['+ provincie.provincia +']')= '" & comune & "'"
        'strsql += " And IDEnteSede<>" & idSede & " And IDEnte<>" & idEnte

        strSql = "Select IdEnteSede, entisedi.denominazione, entisedi.indirizzo + ' ' + entisedi.Civico As Indirizzo, EntiSedi.CAP, comuni.denominazione +' ['+ provincie.provincia +']' as comune"
        strSql += " from entisedi inner join comuni on (entisedi.idcomune=Comuni.idcomune) inner join Provincie on (provincie.idprovincia=Comuni.idProvincia) INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione"
        strSql += " where idEnte = " & Session("IDEnte")

        SqlCmd.CommandText = strSql
        SqlCmd.CommandType = CommandType.Text
        SqlCmd.Connection = Session("conn")
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(tRisultati)

        For Each item In dgRisultatoRicerca.Items
            chkSede = False
            chkIndirizzo = False
            chkRipetizione = False
            sede = dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_SEDE).Text
            Img = DirectCast(dgRisultatoRicerca.Items(item.ItemIndex).FindControl("IdImgAlertNome"), ImageButton)

            If tRisultati.Rows.Count > 0 Then

                indirizzo1 = dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_INDIRIZZO).Text.Trim _
                    & dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_CAP).Text.Trim _
                    & dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_COMUNE).Text.Trim

                denominazione1 = rgx.Replace(sede, "").Trim

                For Each r As DataRow In tRisultati.Rows
                    If r("IdEnteSede") <> dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_IDENTESEDE).Text Then
                        indirizzo2 = r("indirizzo").ToString.Trim & r("CAP").ToString.Trim & r("Comune").ToString.Trim
                        If indirizzo2 = indirizzo1 Then chkIndirizzo = True
                        denominazione2 = rgx.Replace(r("Denominazione"), "").Trim
                        If denominazione2 = denominazione1 Then chkSede = True
                    End If

                Next
            Else

            End If

            If sede.Length > 4 Then
                For c = 0 To sede.Length - 4
                    If sede.Substring(c, 1) = sede.Substring(c + 1, 1) And sede.Substring(c, 1) = sede.Substring(c + 2, 1) And sede.Substring(c, 1) = sede.Substring(c + 3, 1) Then
                        chkRipetizione = True
                    End If
                Next
                For d = 0 To sede.Length - 2
                    strArray = ""
                    If sede.Substring(d, 1) <> sede.Substring(d + 1, 1) Then
                        strArray = sede.Split(sede.Substring(d, 2))
                        If UBound(strArray) > 2 Then
                            chkRipetizione = True
                        End If
                    End If
                Next
            Else
                messaggio = "Lunghezza denominazione sede inferiore a 4 caratteri<br>"
            End If
            If chkSede Then messaggio = "Nome sede corrispondente ad altra sede<br>"
            If chkIndirizzo Then messaggio = "Indirizzo corrispondente ad altra sede"
            'If messaggio <> String.Empty Then
            '    Img.Visible = True
            '    Img.ToolTip = messaggio
            '    Img.AlternateText = messaggio
            'Else
            '    Img.Visible = False
            'End If
        Next
    End Sub
End Class
