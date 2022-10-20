Public Class RicollocaVolontario
    Inherits System.Web.UI.Page

    Dim strsql As String
    Dim dtsGenerico As DataSet
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim dtrgenerico As SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'Generato da Bagnani Jonathan il 29/10/04
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'se si tratta del primo
        If Page.IsPostBack = False Then
            If Request.QueryString("Pippo") = 1 Then
                CaricaDatiVolontario()
                CaricaSediAttuazioneLegateAlVolontario()
                CaricaSediAttuazione()
            Else
                Session("idattività") = Request.QueryString("IdProgetto")
                CaricaDatiVolontario()
                CaricaSediAttuazioneLegateAlVolontario()
            End If
        End If
    End Sub

    Private Sub CaricaDatiVolontario()
        'Generata da BagnaniJonathan 29.11.2004
        'carico la pagina con le informazioni del Volontario e del progetto
        'Session("idattività") = Request.QueryString("IdProgetto")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "select entità.cognome, entità.nome,entità.datachiusura,statientità.Statoentità, entità.datanascita,entità.idcomunenascita," & _
        " cn.denominazione as comuneNascita,cr.denominazione as comuneresidenza," & _
        " entità.idcomuneresidenza,entità.codicefiscale,case entità.sesso when 1 then 'F' else 'M' end as sesso " & _
        " from entità " & _
        " inner join statientità on statientità.idstatoEntità=entità.idstatoentità " & _
        " inner join comuni cn on cn.idcomune=entità.idcomunenascita " & _
        " inner join comuni cr on cr.idcomune=entità.idcomuneresidenza " & _
        " where entità.identità=" & Request.QueryString("IdVolontario") & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            lblCognome.Text = IIf(Not IsDBNull(dtrgenerico("Cognome")), dtrgenerico("Cognome"), "")
            lblNome.Text = IIf(Not IsDBNull(dtrgenerico("nome")), dtrgenerico("nome"), "")
            lblComuneNascita.Text = IIf(Not IsDBNull(dtrgenerico("comuneNascita")), dtrgenerico("comuneNascita"), "")
            lblComuneResidenza.Text = IIf(Not IsDBNull(dtrgenerico("comuneresidenza")), dtrgenerico("comuneresidenza"), "")
            lblCodFis.Text = IIf(Not IsDBNull(dtrgenerico("codicefiscale")), dtrgenerico("codicefiscale"), "")
            lblsesso.Text = dtrgenerico("sesso")
            lbldataNascita.Text = dtrgenerico("DataNascita")
            lblStato.Text = dtrgenerico("Statoentità")
            txtdatachiusuraEV.Text = IIf(IsDBNull(dtrgenerico("datachiusura")) = True, "", dtrgenerico("datachiusura"))
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        If Request.QueryString("pippo") = 1 Then

            strsql = "Select titolo, datainizioattività,datafineattività,dateadd(day,90,datainizioattività) as dataLimite from attività where idattività=" & Request.QueryString("IdProgetto")
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                lblProgetto.Text = IIf(Not IsDBNull(dtrgenerico("titolo")), dtrgenerico("titolo"), "")
                lblDataInizio.Text = IIf(Not IsDBNull(dtrgenerico("datainizioattività")), dtrgenerico("datainizioattività"), "")
                lbldataFine.Text = IIf(Not IsDBNull(dtrgenerico("datafineattività")), dtrgenerico("datafineattività"), "")
                txtdatalimite.Value = IIf(Not IsDBNull(dtrgenerico("datalimite")), dtrgenerico("datalimite"), "")
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

        Else
            strsql = "Select titolo, datainizioattività,datafineattività,dateadd(day,90,datainizioattività) as dataLimite from attività where idattività=" & Session("idattività")
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                lblProgetto.Text = IIf(Not IsDBNull(dtrgenerico("titolo")), dtrgenerico("titolo"), "")
                lblDataInizio.Text = IIf(Not IsDBNull(dtrgenerico("datainizioattività")), dtrgenerico("datainizioattività"), "")
                lbldataFine.Text = IIf(Not IsDBNull(dtrgenerico("datafineattività")), dtrgenerico("datafineattività"), "")
                txtdatalimite.Value = IIf(Not IsDBNull(dtrgenerico("datalimite")), dtrgenerico("datalimite"), "")
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
    End Sub

    Private Sub CaricaSediAttuazioneLegateAlVolontario()
        'Generato da Bagnani Jonathan il 29/10/04
        If Request.QueryString("Pippo") = 1 Then

            lblTitotloElencoVolontari.Visible = True

            strsql = "select '<img src=images/home3.gif Width=20 Height=20 border=0 >' as img, entisedi.denominazione as sedefisica, entisediattuazioni.denominazione as sedeAttuazione," & _
                 " entisedi.indirizzo,Comuni.denominazione + '(' + provincie.provincia + ')' as Comune,attivitàentità.IDAttivitàEntità, " & _
                 "attivitàentità.identità,attivitàentità.idattivitàentesedeattuazione,attivitàentità.datafineattivitàentità," & _
                 "case len(day(attivitàentità.datainizioattivitàentità)) when 1 then '0' + convert(varchar(20),day(attivitàentità.datainizioattivitàentità)) " & _
                 "else convert(varchar(20),day(attivitàentità.datainizioattivitàentità))  end + '/' + " & _
                 "(case len(month(attivitàentità.datainizioattivitàentità)) when 1 then '0' + convert(varchar(20),month(attivitàentità.datainizioattivitàentità)) " & _
                 "else convert(varchar(20),month(attivitàentità.datainizioattivitàentità))  end + '/' + " & _
                 "Convert(varchar(20), Year(attivitàentità.datainizioattivitàentità))) as DataInizio," & _
                 " (select idstatoattivitàentità from statiattivitàentità where defaultstato=1) as statodefault," & _
                 " attivitàentità.note,attivitàentità.percentualeutilizzo,attivitàentità.percentualeutilizzo as Percentuale,attivitàentità.idtipologiaposto,attività.identepresentante as idente" & _
                 " from attivitàentisediattuazione" & _
                 " inner join attivitàentità on " & _
                 " attivitàentità.idattivitàentesedeattuazione = attivitàentisediattuazione.idattivitàentesedeattuazione " & _
                 " inner join statiattivitàentità on statiattivitàentità.IDStatoAttivitàEntità=attivitàentità.IDStatoAttivitàEntità " & _
                 " inner join entisediattuazioni on " & _
                 " attivitàentisediattuazione.IdEnteSedeAttuazione = entisediattuazioni.IdEnteSedeAttuazione " & _
                 " inner join entisedi on entisedi.identesede=entisediattuazioni.identesede " & _
                 " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
                 " inner join provincie on provincie.idprovincia=comuni.idprovincia" & _
                 " inner join attività on attivitàentisediattuazione.IdAttività=attività.IdAttività " & _
                 " where statiattivitàentità.defaultstato=1 and attivitàentisediattuazione.IdAttività=" & Session("idattività") & " and attivitàentità.identità=" & Request.QueryString("IdVolontario") & ""
            dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
            CaricaDataGrid(dgRisultatoRicercaSedi)

        Else

            strsql = "select '<img src=images/home3.gif Width=20 Height=20 border=0 >' as img, entisedi.denominazione as sedefisica, entisediattuazioni.denominazione as sedeAttuazione," & _
                 " entisedi.indirizzo,Comuni.denominazione + '(' + provincie.provincia + ')' as Comune,attivitàentità.IDAttivitàEntità, " & _
                 "attivitàentità.identità,attivitàentità.idattivitàentesedeattuazione,attivitàentità.datafineattivitàentità," & _
                 "case len(day(attivitàentità.datainizioattivitàentità)) when 1 then '0' + convert(varchar(20),day(attivitàentità.datainizioattivitàentità)) " & _
                 "else convert(varchar(20),day(attivitàentità.datainizioattivitàentità))  end + '/' + " & _
                 "(case len(month(attivitàentità.datainizioattivitàentità)) when 1 then '0' + convert(varchar(20),month(attivitàentità.datainizioattivitàentità)) " & _
                 "else convert(varchar(20),month(attivitàentità.datainizioattivitàentità))  end + '/' + " & _
                 "Convert(varchar(20), Year(attivitàentità.datainizioattivitàentità))) as DataInizio," & _
                 " (select idstatoattivitàentità from statiattivitàentità where defaultstato=1) as statodefault," & _
                 " attivitàentità.note,attivitàentità.percentualeutilizzo,attivitàentità.percentualeutilizzo as Percentuale,attivitàentità.idtipologiaposto,attività.identepresentante as idente" & _
                 " from attivitàentisediattuazione" & _
                 " inner join attivitàentità on " & _
                 " attivitàentità.idattivitàentesedeattuazione = attivitàentisediattuazione.idattivitàentesedeattuazione " & _
                 " inner join statiattivitàentità on statiattivitàentità.IDStatoAttivitàEntità=attivitàentità.IDStatoAttivitàEntità " & _
                 " inner join entisediattuazioni on " & _
                 " attivitàentisediattuazione.IdEnteSedeAttuazione = entisediattuazioni.IdEnteSedeAttuazione " & _
                 " inner join entisedi on entisedi.identesede=entisediattuazioni.identesede " & _
                 " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
                 " inner join provincie on provincie.idprovincia=comuni.idprovincia" & _
                 " inner join attività on attivitàentisediattuazione.IdAttività=attività.IdAttività " & _
                 " where statiattivitàentità.defaultstato=1 and attivitàentisediattuazione.IdAttività=" & Request.QueryString("IdProgetto") & " and attivitàentità.identità=" & Request.QueryString("IdVolontario") & ""
            dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
            CaricaDataGrid(dgRisultatoRicercaSedi)
        End If
    End Sub

    Private Sub CaricaSediAttuazione()
        'Generato da Bagnani Jonathan il 29/10/04
        If Request.QueryString("Pippo") = 1 Then
            strsql = "select '<img src=images/sedeassegnazione.gif Width=20 Height=20 border=0 >' as img, entisedi.denominazione as sedefisica, entisediattuazioni.denominazione as sedeAttuazione," & _
                  " entisedi.indirizzo,Comuni.denominazione + '(' + provincie.provincia + ')' as Comune, '' identità, " & _
                  "'' identità,idattivitàentesedeattuazione, '' datafineattivitàentità," & _
                  " '' as statodefault," & _
                  " '' note,'' percentualeutilizzo,'' idtipologiaposto , attività.identepresentante as idente " & _
                  " from attivitàentisediattuazione" & _
                  " inner join entisediattuazioni on " & _
                  " attivitàentisediattuazione.IdEnteSedeAttuazione = entisediattuazioni.IdEnteSedeAttuazione " & _
                  " inner join entisedi on entisedi.identesede=entisediattuazioni.identesede " & _
                  " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
                  " inner join provincie on provincie.idprovincia=comuni.idprovincia" & _
                  " inner join attività on attivitàentisediattuazione.IdAttività=attività.IdAttività " & _
                  " where attivitàentisediattuazione.IdAttività=" & Request.QueryString("IdProgetto") & _
                  " And  attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione=" & Request.QueryString("IdAttivitaEnteSedeAttuazione")

            dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
            CaricaDataGrid(dtgElencoSedi)

        Else
            strsql = "select '<img src=images/sedeassegnazione.gif Width=20 Height=20 border=0 >' as img, entisedi.denominazione as sedefisica, entisediattuazioni.denominazione as sedeAttuazione," & _
                             " entisedi.indirizzo,Comuni.denominazione + '(' + provincie.provincia + ')' as Comune, '' identità, " & _
                             "'' identità,attivitàentisediattuazione.idattivitàentesedeattuazione, '' datafineattivitàentità," & _
                             " '' as statodefault," & _
                             " '' note,'' percentualeutilizzo,'' idtipologiaposto, attività.identepresentante as idente " & _
                             " from attivitàentisediattuazione" & _
                             " inner join entisediattuazioni on " & _
                             " attivitàentisediattuazione.IdEnteSedeAttuazione = entisediattuazioni.IdEnteSedeAttuazione " & _
                             " inner join entisedi on entisedi.identesede=entisediattuazioni.identesede " & _
                             " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
                             " inner join provincie on provincie.idprovincia=comuni.idprovincia" & _
                             " inner join attività on attivitàentisediattuazione.IdAttività=attività.IdAttività " & _
                             " where attivitàentisediattuazione.IdAttività=" & Request.QueryString("IdProgetto") & ""

            dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
            CaricaDataGrid(dtgElencoSedi)
        End If

    End Sub

    Private Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()
    End Sub

    Private Sub cmdConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConferma.Click

        If controlliSalvataggioServer() = True Then
            SalvaTrasferimenti()
        End If

    End Sub

    Private Sub SalvaTrasferimenti()
        'Generato da Bagnani Jonathan il 30/10/04
        'Valorizzazione nella dataGrid del Check delle sedi di Assegnazione
        Dim item As DataGridItem
        Dim intIdStatoSospeso As Integer
        Dim intIdStatoDefault As Integer
        Dim intIdTipoPosto As Integer
        Dim strDataFine As String
        Dim blnAggiorna As Boolean = False
        Dim intIDAttivitàSedeAssegnazione As Integer
        Dim intIDAttivitàSedeAssegnazioneOLD As Integer
        Dim intIdEnteOld As Integer 'salvo l'idente priam delal nuova ricollocazione del volontario
        Dim bytEscludiFormazione As Integer '0: se il volontario è stato assegnato ad un progetto di un'altro ente
        ' 1: se il volontario è stato assegnato ad un progetto dello stesso ente )in questo caso viene escluso dalal formazione)
        Try
            'variabili che uso localmente per creare le stringhe delle date inverse per usarle 
            'per controllare che la data di trasferimento sia compresa 
            ''''Dim strDataImmessa As String
            ''''Dim strDataInizioInversa As String
            ''''Dim strDataFineInversa As String
            ''''Dim strGiornoInizioInverso As String
            ''''Dim strMeseInizioInverso As String
            ''''Dim strAnnoInizioInverso As String
            ''''Dim strGiornoFineInverso As String
            ''''Dim strMeseFineInverso As String
            ''''Dim strAnnoFineInverso As String

            Dim strDataFineAttivitaEntita As String

            lblMessaggi.Visible = False

            If dtgElencoSedi.Visible = True Then
                If dtgElencoSedi.Items.Count = 0 Then
                    lblMessaggi.Visible = True
                    lblMessaggi.Text = "Selezionare la sede per il nuovo collocamento."
                    Exit Sub
                End If
            Else
                Exit Sub
            End If

            'verifica congruenza date
            If VerificaDate(strDataFineAttivitaEntita) = False Then
                lblMessaggi.Visible = True
                Exit Sub
            End If

            If VerificaStatoGraduatoria(Request.QueryString("IdAttivitaEnteSedeAttuazione")) = False Then
                lblMessaggi.Visible = True
                lblMessaggi.Text = "E' necessario confermare la graduatoria di destinazione prima di procedere con il ricollocamento del volontario."
                Exit Sub
            End If

            ''If lbldataFine.Text <> "" Then
            ''    strDataFineInversa = Format(CDate(lbldataFine.Text), "yyyyMMdd")
            ''End If


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ''If Not dtrgenerico Is Nothing Then
            ''    dtrgenerico.Close()
            ''    dtrgenerico = Nothing
            ''End If

            '''controllo limite inferiore 
            ''strsql = "select max(DataInizioAttivitàEntità) as MinDataTrasferimento from AttivitàEntità "
            ''strsql = strsql & "inner join attivitàentisediattuazione on attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione=AttivitàEntità.IDAttivitàEnteSedeAttuazione "
            ''strsql = strsql & "where attivitàentisediattuazione.idattività=" & CInt(Session("idattività")) & " And  attivitàentità.identità=" & Request.QueryString("IdVolontario")

            ''dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            ''If dtrgenerico.HasRows = True Then
            ''    dtrgenerico.Read()
            ''    If Not IsDBNull(dtrgenerico("MinDataTrasferimento")) Then
            ''        strGiornoInizioInverso = IIf(Len(CStr(Day(dtrgenerico("MinDataTrasferimento")))) < 2, "0" & CStr(Day(dtrgenerico("MinDataTrasferimento"))), CStr(Day(dtrgenerico("MinDataTrasferimento"))))
            ''        strMeseInizioInverso = IIf(Len(CStr(Month(dtrgenerico("MinDataTrasferimento")))) < 2, "0" & CStr(Month(dtrgenerico("MinDataTrasferimento"))), CStr(Month(dtrgenerico("MinDataTrasferimento"))))
            ''        strAnnoInizioInverso = Year(dtrgenerico("MinDataTrasferimento"))
            ''        strDataInizioInversa = strAnnoInizioInverso & strMeseInizioInverso & strGiornoInizioInverso
            ''    End If
            ''End If

            ''strDataImmessa = Year(txtdataAvvio.Text) & IIf(Len(CStr(Month(txtdataAvvio.Text))) < 2, "0" & CStr(Month(txtdataAvvio.Text)), CStr(Month(txtdataAvvio.Text))) & IIf(Len(CStr(Day(txtdataAvvio.Text))) < 2, "0" & CStr(Day(txtdataAvvio.Text)), CStr(Day(txtdataAvvio.Text)))

            ''If CDbl(strDataImmessa) > CDbl(strDataInizioInversa) And CDbl(strDataImmessa) < CDbl(strDataFineInversa) Then
            ''Else
            ''    ImgAlert.Visible = True
            ''    lblMessaggi.Visible = True
            ''    lblMessaggi.Text = "La data di Trasferimento deve essere compresa tra il " & IIf(Len(CStr(Day(dtrgenerico("MinDataTrasferimento")))) < 2, "0" & CStr(Day(dtrgenerico("MinDataTrasferimento"))), CStr(Day(dtrgenerico("MinDataTrasferimento")))) & "/" & IIf(Len(CStr(Month(dtrgenerico("MinDataTrasferimento")))) < 2, "0" & CStr(Month(dtrgenerico("MinDataTrasferimento"))), CStr(Month(dtrgenerico("MinDataTrasferimento")))) & "/" & Year(dtrgenerico("MinDataTrasferimento")) & " e il " & lbldataFine.Text & "."
            ''    If Not dtrgenerico Is Nothing Then
            ''        dtrgenerico.Close()
            ''        dtrgenerico = Nothing
            ''    End If
            ''    Exit Sub
            ''End If

            'fine verifica congruenza date

            ''If Not dtrgenerico Is Nothing Then
            ''    dtrgenerico.Close()
            ''    dtrgenerico = Nothing
            ''End If

            strsql = "select IDStatoAttivitàEntità from StatiAttivitàEntità where Sospeso=1"
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                intIdStatoSospeso = CInt(dtrgenerico("IDStatoAttivitàEntità"))
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'stati di default
            strsql = "select IDStatoAttivitàEntità from StatiAttivitàEntità where DefaultStato=1"
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                intIdStatoDefault = CInt(dtrgenerico("IDStatoAttivitàEntità"))
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            For Each item In dgRisultatoRicercaSedi.Items

                '**Imposto la data fine attività del progetto di partenza con la data trasferimento
                strsql = "update attivitàentità set DataFineAttivitàEntità=dateadd(day,-1,convert(datetime,'" & IIf(Len(CStr(Day(txtdataAvvio.Text))) < 2, "0" & CStr(Day(txtdataAvvio.Text)), CStr(Day(txtdataAvvio.Text))) & "/" & IIf(Len(CStr(Month(txtdataAvvio.Text))) < 2, "0" & CStr(Month(txtdataAvvio.Text)), CStr(Month(txtdataAvvio.Text))) & "/" & Year(txtdataAvvio.Text) & "',103)), "
                strsql = strsql & "IdStatoAttivitàEntità=" & intIdStatoSospeso & " "
                strsql = strsql & " where IDAttivitàEntità=" & CInt(dgRisultatoRicercaSedi.Items(item.ItemIndex).Cells(16).Text())

                Dim cmdupdate As Data.SqlClient.SqlCommand
                cmdupdate = New SqlClient.SqlCommand(strsql, Session("conn"))
                cmdupdate.ExecuteNonQuery()
                cmdupdate.Dispose()

                'BLOCCO per prendermi l'id della tipologia del  posto per l'inserimento della nuova cronologia
                strsql = "select IDTipologiaPosto from attivitàentità "
                strsql = strsql & " where (IDTipologiaPosto is not null) and IDAttivitàEntità=" & CInt(dgRisultatoRicercaSedi.Items(item.ItemIndex).Cells(16).Text())

                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    intIdTipoPosto = CInt(dtrgenerico("IDTipologiaPosto"))
                End If

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                intIdEnteOld = CInt(dgRisultatoRicercaSedi.Items(item.ItemIndex).Cells(17).Text())
            Next

            Dim intCheckPerc As Integer
            For Each item In dtgElencoSedi.Items

                Dim txtPerc As TextBox = DirectCast(item.FindControl("txtPercentualeUtilizzo"), TextBox)
                Dim txtNot As TextBox = DirectCast(item.FindControl("txtNote"), TextBox)

                If txtPerc.Text = "" Then
                    intCheckPerc = 0
                Else
                    intCheckPerc = CInt(txtPerc.Text)
                End If
                If intIdEnteOld = CInt(dtgElencoSedi.Items(item.ItemIndex).Cells(16).Text()) Then
                    bytEscludiFormazione = 1
                Else
                    bytEscludiFormazione = 0
                End If
                If intCheckPerc > 0 Then
                    'preparo la insert per le nuove assegnazioni
                    strsql = "insert into attivitàentità "
                    strsql = strsql & "(IDAttivitàEnteSedeAttuazione, IDEntità, DataInizioAttivitàEntità, "
                    strsql = strsql & "DataFineAttivitàEntità, IdStatoAttivitàEntità, Note, "
                    strsql = strsql & "PercentualeUtilizzo, IDTipologiaPosto,UsernameRicollocamento,EscludiFormazione ) "
                    strsql = strsql & "values "
                    strsql = strsql & "(" & CInt(dtgElencoSedi.Items(item.ItemIndex).Cells(10).Text()) & ", "
                    strsql = strsql & CInt(Request.QueryString("IdVolontario")) & ", "
                    strsql = strsql & "convert(datetime, '" & txtdataAvvio.Text & "', 103), "
                    strsql = strsql & "convert(datetime, '" & strDataFineAttivitaEntita & "', 103), "
                    strsql = strsql & intIdStatoDefault & ", "
                    strsql = strsql & "'" & Replace(txtNot.Text, "'", "''") & "', "
                    strsql = strsql & intCheckPerc & ", "
                    strsql = strsql & intIdTipoPosto & ","
                    strsql = strsql & "'" & Session("Utente") & "',"
                    strsql = strsql & bytEscludiFormazione & ")"

                    'altrimenti vado a fare la insert delle nuove sedi
                    Dim cmdinsert As Data.SqlClient.SqlCommand
                    cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()

                    ''Implementato da Guido Testa il 15/05/2006
                    ''Aggiornamento graduatoria e inserimento cronologia

                    'estraggo IdAttivitàSedeAssegnazione presente in graduatoria per inserirlo nella tabella CronologiaEntitàAssegnazione                    
                    intIDAttivitàSedeAssegnazioneOLD = GetIdSedeOld(Request.QueryString("IdVolontario"))
                    'fine estrazione ID

                    'inserisco il vecchio IdAttivitàSedeAssegnazione nella tabella CronologiaEntitàAssegnazione
                    Call InsertCronologia(CInt(Request.QueryString("IdVolontario")), CInt(intIDAttivitàSedeAssegnazioneOLD))
                    'fine inserimento cronologia

                    'estraggo il nuovo IdAttivitàSedeAssegnazione
                    intIDAttivitàSedeAssegnazione = GetIdSede(CInt(dtgElencoSedi.Items(item.ItemIndex).Cells(10).Text))
                    'fine estrazione ID   

                    'aggiorno la graduatoria
                    Call UpdateGraduatoria(CInt(Request.QueryString("IdVolontario")), CInt(intIDAttivitàSedeAssegnazione))
                    'fine aggiornamento


                    'Aggiorno i campi IdSedePrimaAssegnazione e TMPIdSedeAttuazione della tabella ENTITA'
                    'idattivitàentisediattuazione = CInt(dtgElencoSedi.Items(item.ItemIndex).Cells(10).Text())
                    'idvolontario = CInt(Request.QueryString("IdVolontario"))
                    Dim IDEnteSedeAttuazione As Integer = 0
                    strsql = "SELECT IDEnteSedeAttuazione FROM attivitàentisediattuazione " & _
                             "WHERE IDAttivitàEnteSedeAttuazione = " & CInt(dtgElencoSedi.Items(item.ItemIndex).Cells(10).Text())
                    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

                    If dtrgenerico.HasRows = True Then
                        dtrgenerico.Read()
                        IDEnteSedeAttuazione = CInt(dtrgenerico("IDEnteSedeAttuazione"))
                    End If
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    If IDEnteSedeAttuazione <> 0 Then
                        strsql = "UPDATE entità set IdSedePrimaAssegnazione = " & IDEnteSedeAttuazione & ", " & _
                                 "TMPIdSedeAttuazione = " & IDEnteSedeAttuazione & " " & _
                                 "WHERE identità = " & CInt(Request.QueryString("IdVolontario"))
                        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
                        cmdinsert.ExecuteNonQuery()
                        cmdinsert.Dispose()

                    End If
                    'FINE 'Aggiorno i campi IdSedePrimaAssegnazione e TMPIdSedeAttuazione della tabella ENTITA'


                End If
            Next
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Operazione effetuata con successo."
            blnAggiorna = True

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Private Function VerificaDate(Optional ByRef strDataFineAttivitaEntita As String = "") As Boolean
        Dim strDataInizioProgNew As String      'data inizio progetto nuovo
        Dim strDataFineProgNew As String        'data fine progetto nuovo
        Dim strDataInizioProgOld As String      'data inizio progetto vecchio
        Dim strDataFineProgOld As String        'data fine progetto vecchio
        Dim strDataInizioAttVolo As String      'data inizio attività volonatrio
        Dim strDataTrasferimento As String      'data trasferimento indicata

        VerificaDate = True

        'predo le date del progetto di partenza
        strsql = "Select DataInizioAttività, DataFineAttività From Attività Where idattività=" & CInt(Session("idattività"))
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()

        'date progetto di partenza
        strDataInizioProgOld = Format(CDate(dtrgenerico("DataInizioAttività")), "yyyyMMdd")
        strDataFineProgOld = Format(CDate(dtrgenerico("DataFineAttività")), "yyyyMMdd")

        dtrgenerico.Close()
        dtrgenerico = Nothing
        '----------------------------------------------------------------------------------------------
        'date progetto di destinazione
        strDataInizioProgNew = Format(CDate(lblDataInizio.Text), "yyyyMMdd")
        strDataFineProgNew = Format(CDate(lbldataFine.Text), "yyyyMMdd")
        '----------------------------------------------------------------------------------------------
        'data inizio attività volontario
        strsql = "SELECT max(DataInizioAttivitàEntità) as MinDataTrasferimento from AttivitàEntità " & _
                 "INNER JOIN attivitàentisediattuazione on attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione=AttivitàEntità.IDAttivitàEnteSedeAttuazione " & _
                 "WHERE attivitàentisediattuazione.idattività=" & CInt(Session("idattività")) & " AND  attivitàentità.identità=" & Request.QueryString("IdVolontario")
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        dtrgenerico.Read()
        strDataInizioAttVolo = Format(CDate(dtrgenerico("MinDataTrasferimento")), "yyyyMMdd")
        dtrgenerico.Close()
        dtrgenerico = Nothing
        '----------------------------------------------------------------------------------------------
        strDataTrasferimento = Year(txtdataAvvio.Text) & IIf(Len(CStr(Month(txtdataAvvio.Text))) < 2, "0" & CStr(Month(txtdataAvvio.Text)), CStr(Month(txtdataAvvio.Text))) & IIf(Len(CStr(Day(txtdataAvvio.Text))) < 2, "0" & CStr(Day(txtdataAvvio.Text)), CStr(Day(txtdataAvvio.Text)))
        '----------------------------------------------------------------------------------------------

        '***data trasferimento antecedente alla data inizio nuovo progetto***
        If CDbl(strDataTrasferimento) < CDbl(strDataInizioProgNew) Then
            lblMessaggi.Text = "La data di Trasferimento non può essere antecedente alla data inizio del nuovo progetto (" & Right(strDataInizioProgNew, 2) & "/" & Mid(strDataInizioProgNew, 5, 2) & "/" & Left(strDataInizioProgNew, 4) & ")."
            Return False
            Exit Function
        End If

        '***data trasferimento antecedente alla data inizio attività volontario***
        If CDbl(strDataTrasferimento) < CDbl(strDataInizioAttVolo) Then
            lblMessaggi.Text = "La data di Trasferimento non può essere antecedente alla data inizio progetto del volontario (" & Right(strDataInizioAttVolo, 2) & "/" & Mid(strDataInizioAttVolo, 5, 2) & "/" & Left(strDataInizioAttVolo, 4) & ")."
            Return False
            Exit Function
        End If

        '***Data Fine progetto di partenza MINORE Data Fine progetto di destinazione
        If CDbl(strDataFineProgOld) < CDbl(strDataFineProgNew) Then
            strDataFineAttivitaEntita = Right(strDataFineProgOld, 2) & "/" & Mid(strDataFineProgOld, 5, 2) & "/" & Left(strDataFineProgOld, 4)
            '*** la data trasferimento non deve essere maggiore della data fine progetto di partenza***
            If CDbl(strDataTrasferimento) > CDbl(strDataFineProgOld) Then
                lblMessaggi.Text = "La data di Trasferimento non può essere successiva alla data fine progetto di partenza (" & strDataFineAttivitaEntita & ")."
                Return False
                Exit Function
            End If
            '***Data Fine progetto di partenza MAGGIORE Data Fine progetto di destinazione
        ElseIf CDbl(strDataFineProgOld) > CDbl(strDataFineProgNew) Then
            strDataFineAttivitaEntita = Right(strDataFineProgNew, 2) & "/" & Mid(strDataFineProgNew, 5, 2) & "/" & Left(strDataFineProgNew, 4)
            '*** la data trasferimento non deve essere maggiore della data fine progetto di destinazione***
            If CDbl(strDataTrasferimento) > CDbl(strDataFineProgNew) Then
                lblMessaggi.Text = "La data di Trasferimento non può essere successiva alla data fine progetto di destinazione (" & strDataFineAttivitaEntita & ")."
                Return False
                Exit Function
            End If
        Else
            strDataFineAttivitaEntita = Right(strDataFineProgNew, 2) & "/" & Mid(strDataFineProgNew, 5, 2) & "/" & Left(strDataFineProgNew, 4)
            '***Data Fine progetto di partenza UGUALE Data Fine progetto di destinazione
            If CDbl(strDataTrasferimento) > CDbl(strDataFineProgNew) Then
                lblMessaggi.Text = "La data di Trasferimento non può essere successiva alla data fine progetto di destinazione (" & strDataFineAttivitaEntita & ")."
                Return False
                Exit Function
            End If
        End If

    End Function

    Private Function VerificaStatoGraduatoria(ByVal IdAttivitaEnteSedeAttuazione As Integer) As Boolean
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Dim intStatoGraduatoria As Integer
        strsql = " Select AttivitàSediAssegnazione.StatoGraduatoria " & _
        " FROM attività " & _
        " INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività " & _
        " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
        " INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede " & _
        " INNER JOIN AttivitàSediAssegnazione ON attività.IDAttività = AttivitàSediAssegnazione.IDAttività " & _
        "            AND entisedi.IDEnteSede = AttivitàSediAssegnazione.IDEnteSede" & _
        " WHERE attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = " & IdAttivitaEnteSedeAttuazione & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            intStatoGraduatoria = dtrgenerico("StatoGraduatoria")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        If intStatoGraduatoria <> 3 Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Function GetIdSedeOld(ByVal intIdVolontario As Integer) As Integer
        'Estraggo IdAttivitàSedeAssegnazione presente in graduatoria per salvarlo in cronologia prima di aggiornarlo con la nuova sede
        Dim IntSede As Integer
        strsql = ""
        strsql = "Select IdAttivitàSedeAssegnazione " & _
                " FROM GraduatorieEntità  WHERE IdEntità = " & CInt(Request.QueryString("IdVolontario"))
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            IntSede = dtrgenerico("IdAttivitàSedeAssegnazione")
        End If
        dtrgenerico.Close()
        dtrgenerico = Nothing

        Return IntSede

    End Function

    Private Sub InsertCronologia(ByVal intIdVolontario As Integer, ByVal intIdSede As Integer)
        'Inserisco il vecchio IDSede nella tabelle delle cronologie
        Dim cmdinsert As Data.SqlClient.SqlCommand
        strsql = "Insert Into CronologiaEntitàAssegnazione (IDEntità,IDAttivitàSedeAssegnazione,DataCronologia,UserNameCrono) " & _
                            " VALUES (" & intIdVolontario & "," & intIdSede & ",getdate(),'" & Session("Utente") & "')"
        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()

    End Sub

    Private Function GetIdSede(ByVal intAttivitàEnteSedeAttuazione As Integer) As Integer
        'Estraggo il nuovo IdAttivitàSedeAssegnazione
        Dim IntSede As Integer
        strsql = "Select AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione " & _
                                " FROM attivitàentisediattuazione INNER JOIN " & _
                                " entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione INNER JOIN " & _
                                " attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività INNER JOIN " & _
                                " AttivitàSediAssegnazione ON attività.IDAttività = AttivitàSediAssegnazione.IDAttività INNER JOIN " & _
                                " entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede AND AttivitàSediAssegnazione.IDEnteSede = entisedi.IDEnteSede " & _
                                " WHERE attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione =" & intAttivitàEnteSedeAttuazione

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            IntSede = CInt(dtrgenerico("IDAttivitàSedeAssegnazione"))
        End If
        dtrgenerico.Close()
        dtrgenerico = Nothing

        Return IntSede
    End Function

    Private Sub UpdateGraduatoria(ByVal intIdVolontario As Integer, ByVal intIDAttivitàSedeAssegnazione As Integer)
        'Aggiorno la graduatoria
        Dim cmdUpdate As Data.SqlClient.SqlCommand

        strsql = "Update GraduatorieEntità Set IdAttivitàSedeAssegnazione = " & intIDAttivitàSedeAssegnazione & " Where IdEntità = " & intIdVolontario

        cmdUpdate = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdUpdate.ExecuteNonQuery()
        cmdUpdate.Dispose()

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click

        Dim parameterOp As String = Request.Params("Op")
        Response.Redirect("WfrmRicercaVolontariInServizio.aspx?Op=" + parameterOp)

    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        Response.Redirect("WfrmRicercaProgettiRicollocamento.aspx?IdProgetto=" & Request.QueryString("IdProgetto") & "&IdVolontario=" & Request.QueryString("IdVolontario") & "&Op=" & Request.QueryString("Op"))
    End Sub

    Function controlliSalvataggioServer() As Boolean

        If txtdataAvvio.Text.Trim = String.Empty Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Selezionare la Data di Trasferimento."
            Return False
        End If

        Dim dataTrasferimento As Date
        If (Date.TryParse(txtdataAvvio.Text, dataTrasferimento) = False) Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Il formato della data è incorretto: il formato deve essere GG/MM/AAAA."
            Return False
        End If
        Return True

    End Function



End Class