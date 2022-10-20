Imports System.Data.SqlClient

Public Class WebGestioneSediProgettoOlp

    Inherits System.Web.UI.Page
    Dim strsql As String

    Protected WithEvents txtprova As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSalva As System.Web.UI.WebControls.ImageButton
    Protected WithEvents txtPage As System.Web.UI.WebControls.Label
    Dim dataSet As DataSet
    Dim dataReader As SqlClient.SqlDataReader
    Protected WithEvents imgControllaProvincie As Button
    Protected WithEvents txtidbando As HiddenField
    Protected WithEvents imgCheckOLP As Button
    Dim cmdGenerico As SqlClient.SqlCommand
    Protected WithEvents txtRicerca As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtstrsql As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblcorruntpage As System.Web.UI.WebControls.Label
    Protected WithEvents lblpage As System.Web.UI.WebControls.Label
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Request.QueryString("IdAttivita") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then
                ChiudiDataReader(dataReader)
            Else
                ChiudiDataReader(dataReader)
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If


        End If


        If IsPostBack = False Then
            'Verifico se il Progetto può essere modificato.
            strsql = "select attività.idattività,attività.titolo,statiattività.statoattività,statiattività.defaultstato as defaultAttività," & _
            " case isnull(convert(smallint,statibandiAttività.defaultstato),-1)when -1 then '0'" & _
            " else convert(smallint,statibandiAttività.defaultstato)  end as defaultBando " & _
            " from attività " & _
            " inner join statiattività on (statiattività.idstatoattività=attività.idstatoattività) " & _
            " left join bandiAttività  on (attività.idbandoattività=bandiAttività.idbandoattività) " & _
            " left join statibandiAttività  " & _
            " on (bandiAttività.idstatobandoattività=statibandiAttività.idstatobandoattività) " & _
            " where attività.idattività=" & Request.QueryString("IdAttivita") & ""
            ChiudiDataReader(dataReader)
            dataReader = ClsServer.CreaDatareader(strsql, Session("conn"))
            dataReader.Read()
            'Se lo stato dei progetti=default o StatoIstanza=default effettuo le modifiche.
            If dataReader("defaultAttività") = False And dataReader("defaultBando") = False Then
                txtModifica.Value = "False"
            End If
            lblProgetto.Text = dataReader("titolo")
            ChiudiDataReader(dataReader)
            RicercaSedi()
        End If

        If Session("TipoUtente") = "U" Then
            ChiudiDataReader(dataReader)
            strsql = "SELECT enti.Denominazione, enti.IDEnte, attività.IDEntePresentante " & _
                     "FROM attività INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
                     "WHERE attività.IDAttività =" & Request.QueryString("IdAttivita")

            dataReader = ClsServer.CreaDatareader(strsql, Session("conn"))
            dataReader.Read()
            Session("Idente") = dataReader.Item("IDEnte")
            Session("Denominazione") = dataReader.Item("Denominazione")
            ChiudiDataReader(dataReader)
        End If
        ControlloProvince()

        If txtidbando.Value <> "" Then
            strsql = "SELECT IDBando, VisualizzaTutor FROM bando WHERE IDBando = '" & txtidbando.Value & "'"
            ChiudiDataReader(dataReader)
            dataReader = ClsServer.CreaDatareader(strsql, Session("conn"))
            dataReader.Read()
            If dataReader.Item("VisualizzaTutor") = "False" Then
                dgRisultatoRicerca.Columns(22).Visible = False
                dgRisultatoRicerca.Columns(23).Visible = False
            End If
            ChiudiDataReader(dataReader)
        Else
            dgRisultatoRicerca.Columns(22).Visible = False
            dgRisultatoRicerca.Columns(23).Visible = False
        End If
        If Request.QueryString("Nazionale") > 10 Or (Request.QueryString("Nazionale") = 3 And CInt(Request.QueryString("IdAttivita")) > 90000) Or (Request.QueryString("Nazionale") = 4 And CInt(Request.QueryString("IdAttivita")) > 90000) Then
            dgRisultatoRicerca.Columns(20).Visible = False
            dgRisultatoRicerca.Columns(21).Visible = False
        End If
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSalva.Click
        If dgRisultatoRicerca.CurrentPageIndex > 0 Then
            dgRisultatoRicerca.CurrentPageIndex = 0
        End If
        RicercaSedi()
    End Sub
    Private Sub RicercaSedi()
        Dim strappo As String
        'Dim strappo2 As String
        Dim strappo3 As String
        Dim strappo4 As String
        Dim Aggrla As String
        Dim AggTutor As String
        Dim NRlaIns As String
        Dim NTutorIns As String


        'strappo4 = "'<input readonly=""readonly"" style=""border:0px"" id=' + '""txtOlpR'+ Convert(varchar, entisediattuazioni.identesedeattuazione)+ '""' " & _
        '" + case statientisedi.statoentesede when 'Accreditata' then ' BACKGROUND-COLOR: #90ee90; ' when 'Presentata' then ' BACKGROUND-COLOR: #f0e68c; ' when 'Sospesa' then ' BACKGROUND-COLOR: #dcdcdc; 'else' BACKGROUND-COLOR: #ffa07a; 'end + '"" value=' + '""' " & _
        '" + convert(varchar,(select count(DISTINCT EPR.IDENTEPERSONALERUOLO) " & _
        '" from associaEntePersonaleRuoliAttivitàEntiSediAttuazione a " & _
        '" inner join entepersonaleruoli epr on (epr.identepersonaleruolo=a.identepersonaleruolo) " & _
        '" inner join attivitàentisediattuazione b on (a.idattivitàentesedeattuazione=b.idattivitàentesedeattuazione)" & _
        '" where b.idattività=attività.idattività and b.identesedeattuazione=entisediattuazioni.identesedeattuazione and idruolo=1))+ '""' + ';>' as nolpins,"

        strappo3 = "CEILING(convert(decimal(10,2),(select NumeroPostiNoVittoNoAlloggio  + NumeroPostiVittoAlloggio " & _
         " + NumeroPostiVitto  from attivitàentisediattuazione " & _
         " where idattività = attività.idattività " & _
         " and identesedeattuazione=entisediattuazioni.identesedeattuazione)) /"
        If Request.QueryString("Nazionale") = 3 Then 'per progetto straordinario rapporto fisso 1 olp per 4 volontari altrimenti leggo da tabella
            strappo3 = strappo3 & "4"
        Else
            strappo3 = strappo3 & _
             " (select iperambitiattività.MaxVolontariPerOLP " & _
             " from iperambitiattività " & _
             " inner join  macroambitiattività  on " & _
             " (macroambitiattività.idiperambitoattività=iperambitiattività.idiperambitiattività)" & _
             " inner join ambitiattività  on " & _
             " (ambitiattività.idmacroambitoattività=macroambitiattività.idmacroambitoattività) " & _
             " where ambitiattività.idambitoattività=attività.idambitoattività)"
        End If

        strappo3 = strappo3 & ")as NOlpRic,statientisedi.attiva,statientisedi.statoentesede,"

        NRlaIns = "  convert(varchar,(select count(DISTINCT EPR.IDENTEPERSONALERUOLO) " & _
        " from associaEntePersonaleRuoliAttivitàEntiSediAttuazione a " & _
        " inner join entepersonaleruoli epr on (epr.identepersonaleruolo=a.identepersonaleruolo) " & _
        " inner join attivitàentisediattuazione b on (a.idattivitàentesedeattuazione=b.idattivitàentesedeattuazione)" & _
        " where b.idattività=attività.idattività and b.identesedeattuazione=entisediattuazioni.identesedeattuazione and idruolo=6))  as nrla"



        NTutorIns = " convert(varchar,(select count(DISTINCT EPR.IDENTEPERSONALERUOLO) " & _
        " from associaEntePersonaleRuoliAttivitàEntiSediAttuazione a " & _
        " inner join entepersonaleruoli epr on (epr.identepersonaleruolo=a.identepersonaleruolo) " & _
        " inner join attivitàentisediattuazione b on (a.idattivitàentesedeattuazione=b.idattivitàentesedeattuazione)" & _
        " where b.idattività=attività.idattività and b.identesedeattuazione=entisediattuazioni.identesedeattuazione and idruolo=5)) as nTutor"

        
        'Aggrla = " <img src=""images/valida_small.png"" alt=""Valida"" onclick=""javascript:apri('" & _
        '" + convert(varchar,entisedi.identeSede) + ',' + convert(varchar,entisediattuazioni.identesedeattuazione)+ ',' + convert(varchar,attività.idattività)+ '," & _
        '"'+ case isnull(attivitàentisediattuazione.idattivitàEnteSedeAttuazione,-1)" & _
        '" when -1 then '0' else convert(varchar,attivitàentisediattuazione.idattivitàEnteSedeAttuazione) end + '," & _
        '"''RLEA'')"" " & _
        '"  title=""Aggiungi RLA""/>"

        'AggTutor = " <img src=""images/valida_small.png"" alt=""Valida"" onclick=""javascript:apri('" & _
        '" + convert(varchar,entisedi.identeSede) + ',' + convert(varchar,entisediattuazioni.identesedeattuazione)+ ',' + convert(varchar,attività.idattività)+ '," & _
        '"'+ case isnull(attivitàentisediattuazione.idattivitàEnteSedeAttuazione,-1)" & _
        '" when -1 then '0' else convert(varchar,attivitàentisediattuazione.idattivitàEnteSedeAttuazione) end + '," & _
        '" ''TUTOR'')"" " & _
        '" title=""Aggiungi Tutor""/>"

        strsql = " select distinct  " & strappo3 & strappo4 & "" & _
        " entisedi.identeSede,entisedi.Denominazione as sedeFisica,entisedi.Indirizzo,entisedi.Telefono," & _
        " comuni.denominazione as Comune,comuni.cap,Provincie.provincia,entisediattuazioni.identesedeattuazione," & _
        " entisediattuazioni.denominazione as sedeAttuazione,attivitàentisediattuazione.idattivitàentesedeattuazione as idatt,attività.idattività " & _
        " ,(select count(*) from attivitàentisediattuazione where idattività=attività.idattività " & _
        " and identesedeattuazione=entisediattuazioni.identesedeattuazione) as nProgAtt, " & _
         " case isnull(attivitàentisediattuazione.idattivitàEntesedeAttuazione,-1) when -1  then '""' else convert(varchar,(select NumeroPostiNoVittoNoAlloggio  + NumeroPostiVittoAlloggio + NumeroPostiVitto  from attivitàentisediattuazione where  idattività=attività.idattività  and identesedeattuazione=entisediattuazioni.identesedeattuazione  )) end  as NumeroPosti, " & _
         " case isnull(attivitàentisediattuazione.idattivitàEnteSedeAttuazione,-1)  when -1 then '0' else convert(varchar,attivitàentisediattuazione.idattivitàEnteSedeAttuazione) end as idattivitàEnteSedeAttuazione, " & _
        " (select count(DISTINCT EPR.IDENTEPERSONALERUOLO) from associaEntePersonaleRuoliAttivitàEntiSediAttuazione a " & _
        " inner join entepersonaleruoli epr on (epr.identepersonaleruolo=a.identepersonaleruolo) " & _
        " inner join attivitàentisediattuazione b on (a.idattivitàentesedeattuazione=b.idattivitàentesedeattuazione)" & _
        " where b.idattività=attività.idattività and b.identesedeattuazione=entisediattuazioni.identesedeattuazione and idruolo=1) as nolpins, " & _
        " entisediattuazioni.identecapofila,enti.CodiceRegione, "
        If Session("TipoUtente") = "E" And Request.QueryString("EnteCapoFila") = "False" Then
            strsql = strsql & " case WHEN entisediattuazioni.identecapofila = " & Session("IDEnte") & "  then 'SI' ELSE 'NO' END AS VISIBILE,"
        Else
            strsql = strsql & " 'SI' AS VISIBILE,"
        End If

        strsql = strsql & "CASE WHEN entisediattuazioni.identecapofila =  " & Session("IdEnte") & " THEN '" & Aggrla & "' ELSE '' END as aggrla , CASE WHEN entisediattuazioni.identecapofila =  " & Session("IdEnte") & " THEN '" & AggTutor & "' ELSE '' END as aggTutor ," & NRlaIns & "," & NTutorIns & "" & _
                " from attività " & _
                " inner join attivitàentisediattuazione on  " & _
                " (attivitàentisediattuazione.idattività=attività.idattività) " & _
                " LEFT OUTER JOIN AttivitàEntiCoprogettazione ON attività.IDAttività = AttivitàEntiCoprogettazione.IdAttività" & _
                " inner join entisediattuazioni on (entisediattuazioni.identesedeattuazione=attivitàentisediattuazione.identesedeattuazione) " & _
                " inner join entisedi  on entisediattuazioni.identesede =entisedi.identesede " & _
                " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
                " inner join Provincie on(Provincie.idProvincia=Comuni.idProvincia) " & _
                " inner join Regioni on (Regioni.Idregione=Provincie.IdRegione) " & _
                " inner join Nazioni on (Nazioni.IdNazione=Regioni.IdNazione) " & _
                " inner join statientisedi on (statientisedi.idstatoentesede=entisediattuazioni.idstatoentesede) " & _
                " inner join enti on (entisedi.idente=enti.idente) " & _
                " where attività.idattività=" & CInt(Request.QueryString("IdAttivita")) & " and not attivitàentisediattuazione.idattivitàentesedeattuazione is null "

        'modifica Antonello 05/09/2007
        'If Request.QueryString("CoProgettato") = "True" And Session("TipoUtente") = "E" Then

        '    If Request.QueryString("EnteCapoFila") = "False" Then

        '        'Aggiunto da Alessandra Taballione il 08/07/2005 inserimento rla e tutor
        '        strsql = strsql & "CASE WHEN entisediattuazioni.identecapofila =  " & Session("IdEnte") & " THEN '" & Aggrla & "' ELSE '' END as aggrla , CASE WHEN entisediattuazioni.identecapofila =  " & Session("IdEnte") & " THEN '" & AggTutor & "' ELSE '' END as aggTutor ," & NRlaIns & "," & NTutorIns & "" & _
        '        " from attività " & _
        '        " inner join attivitàentisediattuazione on  " & _
        '        " (attivitàentisediattuazione.idattività=attività.idattività) " & _
        '        " LEFT OUTER JOIN AttivitàEntiCoprogettazione ON attività.IDAttività = AttivitàEntiCoprogettazione.IdAttività" & _
        '        " inner join entisediattuazioni on (entisediattuazioni.identesedeattuazione=attivitàentisediattuazione.identesedeattuazione) " & _
        '        " inner join entisedi  on entisediattuazioni.identesede =entisedi.identesede " & _
        '        " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
        '        " inner join Provincie on(Provincie.idProvincia=Comuni.idProvincia) " & _
        '        " inner join Regioni on (Regioni.Idregione=Provincie.IdRegione) " & _
        '        " inner join Nazioni on (Nazioni.IdNazione=Regioni.IdNazione) " & _
        '        " inner join statientisedi on (statientisedi.idstatoentesede=entisediattuazioni.idstatoentesede) " & _
        '        " inner join enti on (entisedi.idente=enti.idente) " & _
        '        " where attività.idattività=" & CInt(Request.QueryString("IdAttivita")) & " and not attivitàentisediattuazione.idattivitàentesedeattuazione is null "
        '    Else
        '        strsql = strsql & " '" & Aggrla & "' as aggrla , '" & AggTutor & "' as aggTutor ," & NRlaIns & "," & NTutorIns & "" & _
        '         " from attività " & _
        '        " inner join attivitàentisediattuazione on  " & _
        '        " (attivitàentisediattuazione.idattività=attività.idattività) " & _
        '        " INNER JOIN AttivitàEntiCoprogettazione ON attività.IDAttività = AttivitàEntiCoprogettazione.IdAttività" & _
        '        " inner join entisediattuazioni on (entisediattuazioni.identesedeattuazione=attivitàentisediattuazione.identesedeattuazione) " & _
        '        " inner join entisedi  on entisediattuazioni.identesede =entisedi.identesede " & _
        '        " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
        '        " inner join Provincie on(Provincie.idProvincia=Comuni.idProvincia) " & _
        '        " inner join Regioni on (Regioni.Idregione=Provincie.IdRegione) " & _
        '        " inner join Nazioni on (Nazioni.IdNazione=Regioni.IdNazione) " & _
        '        " inner join statientisedi on (statientisedi.idstatoentesede=entisediattuazioni.idstatoentesede) " & _
        '        " inner join enti on (entisedi.idente=enti.idente) " & _
        '        " where attività.idattività=" & CInt(Request.QueryString("IdAttivita")) & " and not attivitàentisediattuazione.idattivitàentesedeattuazione is null "
        '    End If
        'Else
        '    strsql = strsql & "'" & Aggrla & "' as aggrla ,'" & AggTutor & "' as aggTutor ," & NRlaIns & "," & NTutorIns & "" & _
        '           " from attività " & _
        '           " inner join attivitàentisediattuazione on  " & _
        '           " (attivitàentisediattuazione.idattività=attività.idattività) " & _
        '           " inner join entisediattuazioni on (entisediattuazioni.identesedeattuazione=attivitàentisediattuazione.identesedeattuazione) " & _
        '           " inner join entisedi  on entisediattuazioni.identesede =entisedi.identesede " & _
        '           " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
        '           " inner join Provincie on(Provincie.idProvincia=Comuni.idProvincia) " & _
        '           " inner join Regioni on (Regioni.Idregione=Provincie.IdRegione) " & _
        '           " inner join Nazioni on (Nazioni.IdNazione=Regioni.IdNazione) " & _
        '           " inner join statientisedi on (statientisedi.idstatoentesede=entisediattuazioni.idstatoentesede) " & _
        '           " inner join enti on (entisedi.idente=enti.idente) " & _
        '           " where(attività.identePresentante=" & CInt(Session("IdEnte")) & " And attività.idattività=" & CInt(Request.QueryString("IdAttivita")) & ") and not attivitàentisediattuazione.idattivitàentesedeattuazione is null "

        'End If

        'If (Request.QueryString("Nazionale") = "2" Or Request.QueryString("Nazionale") = "5" Or Request.QueryString("Nazionale") = "6") Then
        strsql = strsql & " and nazioni.nazionebase=1 "
        'End If

        If Trim(txtSedefisica.Text) <> "" Then
            strsql = strsql & " and enti.CodiceRegione = '" & Replace(txtSedefisica.Text, "'", "''") & "'"
        End If
        If Trim(txtSedeAttuaz.Text) <> "" Then
            strsql = strsql & " and entisediattuazioni.Denominazione like '" & Replace(txtSedeAttuaz.Text, "'", "''") & "%'"
        End If
        If txtCodice.Text <> "" Then
            strsql = strsql & " and entisediattuazioni.IDENTESEDEATTUAZIONE like '" & Replace(txtCodice.Text, "'", "''") & "%'"
        End If
        dataSet = ClsServer.DataSetGenerico(strsql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicerca)
        Session("RisultatoGriglia") = dataSet
    End Sub
    Sub CaricaDataGrid(ByRef elencoRisorse As DataGrid) 'valorizzo la datagrid passata
        elencoRisorse.DataSource = dataSet
        elencoRisorse.DataBind()
        If (elencoRisorse.Items.Count > 0) Then
            ColoraCelle()
        End If

    End Sub
    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 15/06/04
        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=

        '*****la cella statoente è da mantenersi ultima al fine di permettere il corretto funzionamento di questa procedura****
        Dim item As DataGridItem
        Dim x As Integer
        Dim color As New System.Drawing.Color
        Dim numeroColonne As Int32 = dgRisultatoRicerca.Items(0).Cells.Count

        For Each item In dgRisultatoRicerca.Items
            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(numeroColonne - 1).Text
                Case "Accreditata"
                    For x = 0 To numeroColonne - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = VERDE
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = NERO
                    Next
                Case "Presentata"
                    For x = 0 To numeroColonne - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = GIALLO
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = NERO
                    Next
                Case "Sospesa"
                    For x = 0 To numeroColonne - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = GRIGIO
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = NERO
                    Next
                Case "Cancellata"
                    For x = 0 To numeroColonne - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = ROSSO
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = NERO
                    Next
            End Select
        Next
    End Sub
    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        RicercaSedi()
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'lblpage.Text = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub
    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
       

        If e.CommandName = "AggiungiModificaOlp" Then
            Dim abilitaLink As Boolean
            Dim idEntecapofila As String = e.Item.Cells(25).Text
            If Request.QueryString("CoProgettato") = "True" And Session("TipoUtente") = "E" Then
                If Request.QueryString("EnteCapoFila") = "False" Then
                    If (idEntecapofila = Session("IdEnte")) Then
                        abilitaLink = True
                    Else
                        abilitaLink = False

                    End If
                Else
                    abilitaLink = True
                End If

            Else
                abilitaLink = True
            End If
            If (abilitaLink) Then

                Dim coProgettato As String = Request.QueryString("CoProgettato")
                Dim idSede As String = e.Item.Cells(7).Text
                Dim idSedeAttuazione As String = e.Item.Cells(8).Text
                Dim idAttivita As String = Request.QueryString("IdAttivita")
                Dim idAttivitaEnteSedeAttuazione As String = e.Item.Cells(e.Item.Cells.Count - 3).Text
                Dim enteCapoFila As String = Request.QueryString("EnteCapoFila")
                Dim nazionale As String = Request.QueryString("Nazionale")


                Response.Redirect("WebElencoOlp.aspx?EnteCapoFila=" + enteCapoFila + "&CoProgettato=" + coProgettato + "&Nazionale=" + nazionale + "&Modifica=" & Request.QueryString("Modifica") & "&idSede=" + idSede + "&IdSedeAttuazione=" + idSedeAttuazione + "&IdAttivita=" + idAttivita + "&idattES=" + idAttivitaEnteSedeAttuazione + "&tiporuolo=OLP" + "&VengoDa=" & Request.QueryString("VengoDa"))
            End If
        End If
        If e.CommandName = "AggiungiModificaRlea" Then
            Dim abilitaLink As Boolean
            Dim idEntecapofila As String = e.Item.Cells(25).Text
            If Request.QueryString("CoProgettato") = "True" And Session("TipoUtente") = "E" Then
                If Request.QueryString("EnteCapoFila") = "False" Then
                    If (idEntecapofila = Session("IdEnte")) Then
                        abilitaLink = True
                    Else
                        abilitaLink = False

                    End If
                Else
                    abilitaLink = True
                End If

            Else
                abilitaLink = True
            End If
            If (abilitaLink) Then

                Dim coProgettato As String = Request.QueryString("CoProgettato")
                Dim idSede As String = e.Item.Cells(7).Text
                Dim idSedeAttuazione As String = e.Item.Cells(8).Text
                Dim idAttivita As String = Request.QueryString("IdAttivita")
                Dim idAttivitaEnteSedeAttuazione As String = e.Item.Cells(e.Item.Cells.Count - 3).Text
                Dim enteCapoFila As String = Request.QueryString("EnteCapoFila")
                Dim nazionale As String = Request.QueryString("Nazionale")


                Response.Redirect("WebElencoOlp.aspx?EnteCapoFila=" + enteCapoFila + "&CoProgettato=" + coProgettato + "&Nazionale=" + nazionale + "&Modifica=" & Request.QueryString("Modifica") & "&idSede=" + idSede + "&IdSedeAttuazione=" + idSedeAttuazione + "&IdAttivita=" + idAttivita + "&idattES=" + idAttivitaEnteSedeAttuazione + "&tiporuolo=RLEA" + "&VengoDa=" & Request.QueryString("VengoDa"))
            End If
        End If
        If e.CommandName = "AggiungiModificaTutor" Then
            Dim abilitaLink As Boolean
            Dim idEntecapofila As String = e.Item.Cells(25).Text
            If Request.QueryString("CoProgettato") = "True" And Session("TipoUtente") = "E" Then
                If Request.QueryString("EnteCapoFila") = "False" Then
                    If (idEntecapofila = Session("IdEnte")) Then
                        abilitaLink = True
                    Else
                        abilitaLink = False

                    End If
                End If

            Else
                abilitaLink = True
            End If
            If (abilitaLink) Then

                Dim coProgettato As String = Request.QueryString("CoProgettato")
                Dim idSede As String = e.Item.Cells(7).Text
                Dim idSedeAttuazione As String = e.Item.Cells(8).Text
                Dim idAttivita As String = Request.QueryString("IdAttivita")
                Dim idAttivitaEnteSedeAttuazione As String = e.Item.Cells(e.Item.Cells.Count - 3).Text
                Dim enteCapoFila As String = Request.QueryString("EnteCapoFila")
                Dim nazionale As String = Request.QueryString("Nazionale")


                Response.Redirect("WebElencoOlp.aspx?EnteCapoFila=" + enteCapoFila + "&CoProgettato=" + coProgettato + "&Nazionale=" + nazionale + "&Modifica=" & Request.QueryString("Modifica") & "&idSede=" + idSede + "&IdSedeAttuazione=" + idSedeAttuazione + "&IdAttivita=" + idAttivita + "&idattES=" + idAttivitaEnteSedeAttuazione + "&tiporuolo=Tutor")
            End If
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click

        If Request.QueryString("popup") <> "1" Then
            Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa") & "")
            'Response.Redirect("TabProgetti.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa") & "")
        Else
            Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?popup=" & Request.QueryString("popup") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa") & "")
            'Response.Redirect("TabProgetti.aspx?popup=" & Request.QueryString("popup") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa") & "")
        End If
    End Sub


    'Funzione per calcolare l'ID del bando - Generata da Amilcare Paolella il 15/11/2005
    Private Sub ControlloProvince()
        strsql = "select a.idbando, b.idbandoattività from bando a" & _
            " inner join bandiattività b on a.idbando=b.idbando" & _
            " inner join attività c on b.idbandoattività=c.idbandoattività" & _
            " inner join StatiBandiAttività d on d.idstatobandoattività=b.idstatobandoattività" & _
            " where c.IDAttività =" & Request.QueryString("IdAttivita") & _
            " group by b.idente,a.idbando, b.idbandoattività"
        dataReader = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dataReader.HasRows = True Then
            'imgControllaProvincie.Visible = True
            'imgCheckOLP.Visible = True
            dataReader.Read()
            txtidbando.Value = CInt(dataReader.Item(0))
            dataReader.Close()
            dataReader = Nothing
        Else
            'imgControllaProvincie.Visible = False
            'imgCheckOLP.Visible = False
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub


    Protected Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        If dgRisultatoRicerca.CurrentPageIndex > 0 Then
            dgRisultatoRicerca.CurrentPageIndex = 0
        End If
        RicercaSedi()
    End Sub

    Protected Sub imgCheckOLP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgCheckOLP.Click
        Response.Redirect("risorsesedidiverse.aspx?EnteCapoFila=" + Request.QueryString("EnteCapoFila") + "&CoProgettato=" + Request.QueryString("CoProgettato") + "&Nazionale=" + Request.QueryString("Nazionale") + "&Modifica=" & Request.QueryString("Modifica") & "&idSede=" + Request.QueryString("idSede") + "&IdSedeAttuazione=" + Request.QueryString("IdSedeAttuazione") + "&IdAttivita=" + Request.QueryString("IdAttivita") + "&idattES=" + Request.QueryString("idattES") + "&idbando=" + txtidbando.Value)
    End Sub

    Protected Sub imgControllaProvincie_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgControllaProvincie.Click
        Response.Redirect("WFrmControlloProvincie.aspx?EnteCapoFila=" + Request.QueryString("EnteCapoFila") + "&CoProgettato=" + Request.QueryString("CoProgettato") + "&Nazionale=" + Request.QueryString("Nazionale") + "&Modifica=" & Request.QueryString("Modifica") & "&idSede=" + Request.QueryString("idSede") + "&IdSedeAttuazione=" + Request.QueryString("IdSedeAttuazione") + "&IdAttivita=" + Request.QueryString("IdAttivita") + "&idattES=" + Request.QueryString("idattES") + "&idbando=" + txtidbando.Value)
    End Sub

    Protected Sub dgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged

    End Sub
End Class
