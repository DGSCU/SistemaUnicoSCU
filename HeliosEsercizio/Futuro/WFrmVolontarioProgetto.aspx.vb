Public Class WFrmVolontarioProgetto
    Inherits System.Web.UI.Page

    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim PROGETTO_GARANZIA_GIOVANI As String = "4"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Caricamento Della Combo
            CboTipoPosto.DataTextField = "Descrizione"
            CboTipoPosto.DataValueField = "IdTipologiaPosto"
            CboTipoPosto.DataSource = ClsServer.CreaDataTable("Select IdTipologiaPosto,Descrizione From TipologiePosto", False, Session("Conn"))
            CboTipoPosto.DataBind()

            'Carico i dati Relativi al Volontario
            Dim MyDtrVolontario As System.Data.SqlClient.SqlDataReader
            Dim StrSql As String
            StrSql = "Select Entità.Cognome,Entità.Nome,Entità.DataNascita,Entità.CodiceFiscale,Comuni.Denominazione,Provincie.Provincia," & _
                     "GraduatorieEntità.IdTipologiaPosto,AttivitàSediAssegnazione.IdAttività,Entità.TMPCodiceProgetto,Entità.TMPIdSedeAttuazione,entisediattuazioni.IdEnteSedeAttuazione,bando.LOTUS " & _
                     "From Entità " & _
                     "INNER JOIN Comuni ON Entità.IdComuneNascita = Comuni.IdComune " & _
                     "INNER JOIN Provincie ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                     "INNER JOIN GraduatorieEntità ON Entità.IdEntità = GraduatorieEntità.IdEntità " & _
                     "INNER JOIN AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IdAttivitàSedeAssegnazione " & _
                     "INNER JOIN entisediattuazioni on AttivitàSediAssegnazione.identesede = entisediattuazioni.identesede  " & _
                     "INNER JOIN Attività ON AttivitàSediAssegnazione.IdAttività = Attività.IdAttività " & _
                     "INNER JOIN BandiAttività ON BandiAttività.IDBandoAttività = attività.IDBandoAttività " & _
                     "INNER JOIN bando ON bando.IdBando = BandiAttività.IdBando " & _
                     "WHERE Entità.IdEntità = " & Request.QueryString("Id")
            MyDtrVolontario = ClsServer.CreaDatareader(StrSql, Session("Conn"))
            MyDtrVolontario.Read()
            txtLOTUS.Value = MyDtrVolontario.Item("LOTUS")
            LblCognome.Text = MyDtrVolontario.Item("Cognome")
            LblNome.Text = MyDtrVolontario.Item("Nome")
            LblCF.Text = MyDtrVolontario.Item("CodiceFiscale")
            LblData.Text = MyDtrVolontario.Item("DataNascita")
            LblLuogo.Text = MyDtrVolontario.Item("Denominazione") & " (" & MyDtrVolontario.Item("Provincia") & ")"
            CboTipoPosto.SelectedValue = MyDtrVolontario.Item("IdTipologiaPosto")
            HdyIdAttivita.Value = MyDtrVolontario.Item("IdAttività")
            'Precarico i Filtri di Ricerca
            TxtCodiceProgetto.Text = MyDtrVolontario.Item("TMPCodiceProgetto")
            TxtCodiceSede.Text = MyDtrVolontario.Item("IdEnteSedeAttuazione")
            MyDtrVolontario.Close()

            'Trovo il Bando di Competenza
            StrSql = "Select BandiAttività.IdBando " & _
                     "From Entità " & _
                     "INNER JOIN GraduatorieEntità ON Entità.IdEntità = GraduatorieEntità.IdEntità " & _
                     "INNER JOIN AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IdAttivitàSedeAssegnazione " & _
                     "INNER JOIN Attività ON AttivitàSediAssegnazione.IdAttività = Attività.IdAttività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "WHERE Entità.IdEntità = " & Request.QueryString("Id")
            MyDtrVolontario = ClsServer.CreaDatareader(StrSql, Session("Conn"))
            MyDtrVolontario.Read()
            HdyIdBando.Value = MyDtrVolontario.Item("IdBando")
            MyDtrVolontario.Close()
            MyDtrVolontario = Nothing

            'Pre Carico l'elenco dei progetti
            CaricaGriglia()
            'controllo se si tratta di utente legato a bando LOTUS
            If txtLOTUS.Value = True Then
                lblDataFine.Visible = True
                txtDataFine.Visible = True
                txtDataFine.Text = "01/01/2005"
                TxtDataInizio.Text = "01/01/2005"
                lblCodiceVolontario.Visible = True
                txtCodiceVolontario.Visible = True
            End If
        End If
    End Sub

    Private Sub CaricaGriglia()
        'Ricerca i Progetti
        Dim StrSql As String
        StrSql = "Select C.IdAttivitàEnteSedeAttuazione As IdAttivitaEnteSedeAttuazione,B.IdAttività AS IdAttivita,B.CodiceEnte,B.Titolo," & _
                 "isnull(i.datainiziodifferita,B.DataInizioAttività) As DataInizioAttivita, isnull(i.datafinedifferita,B.DataFineAttività) As DataFineAttivita,F.Denominazione As Comune,G.Provincia,E.Indirizzo,C.IdEnteSedeAttuazione, " & _
                 "B.NumeroPostiNoVittoNoAlloggio + B.NumeroPostiVittoAlloggio + B.NumeroPostiVitto AS Posti," & _
                 "c.NumeroPostiNoVittoNoAlloggio + c.NumeroPostiVittoAlloggio + c.NumeroPostiVitto AS PostiPrevistiSede, " & _
                 "(SELECT COUNT(*) FROM entità e1 INNER JOIN attivitàentità ae on e1.identità = ae.identità WHERE(ae.idattivitàentesedeattuazione = c.idattivitàentesedeattuazione) and ae.IdStatoAttivitàEntità=1 and e1.idstatoentità = 3) AS  PostiOccupati " & _
                 "From BandiAttività AS A " & _
                 "INNER JOIN bando ON A.IdBando = bando.IdBando " & _
                 "INNER JOIN Attività AS B ON A.IdBandoAttività = B.IdBandoAttività " & _
                 "INNER JOIN AttivitàEntiSediAttuazione AS C ON B.IdAttività = C.IdAttività " & _
                 "INNER JOIN EntiSediAttuazioni AS D ON C.IdEnteSedeAttuazione = D.IdEnteSedeAttuazione " & _
                 "INNER JOIN EntiSedi AS E ON D.IdEnteSede = E.IdEnteSede " & _
                 "INNER JOIN Comuni AS F ON E.IdComune = F.IdComune " & _
                 "INNER JOIN Provincie AS G ON F.IdProvincia = G.IdProvincia " & _
                 "INNER JOIN Enti AS H ON B.IdEntePresentante = H.IdEnte " & _
                 "INNER JOIN attivitàsediassegnazione AS I ON B.idattività = i.idattività and e.identesede = i.identesede " & _
                 "WHERE bando.gruppo in ( select gruppo from bando where IdBando = '" & HdyIdBando.Value & "') AND bando.LOTUS=" & IIf(txtLOTUS.Value = True, 1, 0) & " And B.IdStatoAttività = 1 "

        'Controllo Le Condizioni
        If TxtCodiceEnte.Text.Trim <> "" Then
            StrSql = StrSql & " And H.CodiceRegione = '" & ClsServer.NoApice(TxtCodiceEnte.Text) & "'"
        End If
        If TxtDescEnte.Text.Trim <> "" Then
            StrSql = StrSql & " And H.Denominazione Like '" & ClsServer.NoApice(TxtCodiceEnte.Text) & "%'"
        End If
        If TxtCodiceProgetto.Text.Trim <> "" Then
            StrSql = StrSql & " And B.CodiceEnte = '" & ClsServer.NoApice(TxtCodiceProgetto.Text) & "'"
        End If
        If TxtDescProgetto.Text.Trim <> "" Then
            StrSql = StrSql & " And B.Titolo Like '" & ClsServer.NoApice(TxtDescProgetto.Text) & "%'"
        End If
        If TxtCodiceSede.Text.Trim <> "" Then
            StrSql = StrSql & " And C.IdEnteSedeAttuazione = '" & ClsServer.NoApice(TxtCodiceSede.Text) & "'"
        End If
        If TxtComune.Text.Trim <> "" Then
            StrSql = StrSql & " And F.Denominazione LIKE '" & ClsServer.NoApice(TxtComune.Text) & "%'"
        End If
        StrSql = StrSql & " ORDER BY B.CodiceEnte,B.Titolo"
        DtgProgetti.DataSource = ClsServer.DataSetGenerico(StrSql, Session("conn"))
        Session("dtRisulatato") = DtgProgetti.DataSource
        DtgProgetti.SelectedIndex = -1
        DtgProgetti.CurrentPageIndex = 0
        DtgProgetti.DataBind()
        If DtgProgetti.Items.Count = 0 Then
            LblErrore.Text = "La ricerca non ha prodotto alcun risultato"
            Exit Sub
        End If

    End Sub

    Private Sub DtgProgetti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DtgProgetti.PageIndexChanged
        'utilizzo la session per memorizzare il dataset generato al momento della ricerca
        'DtgProgetti.CurrentPageIndex = e.NewPageIndex
        'CaricaGriglia()
        DtgProgetti.SelectedIndex = -1
        DtgProgetti.CurrentPageIndex = e.NewPageIndex
        DtgProgetti.DataSource = Session("dtRisulatato")
        DtgProgetti.DataBind()
        'DtgProgetti.SelectedIndex = -1
    End Sub

    Private Sub DtgProgetti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DtgProgetti.SelectedIndexChanged

        'INIZIO SANDOKAN 21/09/2011 aggiunto alert per ricollocamento su altro ente
        Dim enteSelezionato As String
        Dim enteInSessione As String
        'Dim stringa As String
        'Dim s() As String
        Dim i As Integer
        Dim MyDtrVolontario As System.Data.SqlClient.SqlDataReader
        Dim StrSqlADC As String
        enteSelezionato = DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(3).Text
        ''' pulire


        'FINE SANDOKAN

        StrSqlADC = "SELECT DISTINCT enti.CodiceRegione as CodReg " & _
                 "FROM attività " & _
                 "INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte  " & _
                 "WHERE attività.CodiceEnte = '" & enteSelezionato & "'"
        MyDtrVolontario = ClsServer.CreaDatareader(StrSqlADC, Session("Conn"))
        MyDtrVolontario.Read()
        enteSelezionato = MyDtrVolontario.Item("CodReg")
        MyDtrVolontario.Close()
        MyDtrVolontario = Nothing

        enteInSessione = Session("txtCodEnte")
        'SANDOKAN
        If enteSelezionato <> enteInSessione Then
            txtcodreg.Value = "True"
        Else
            txtcodreg.Value = ""
        End If
        'FINE SANDOKAN

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WFrmVolontari.aspx?IdVol=" & Request.QueryString("Id") & "&IdAttivita=" & HdyIdAttivita.Value)
    End Sub

    Private Sub cmdAssocia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAssocia.Click

        If controlliSalvataggioServer() = True Then
            AssociaGraduatoria()
        End If

    End Sub

    Private Sub AssociaGraduatoria()
        Dim strCronoUser As String
        Dim strCronoStato As String
        LblErrore.Text = ""
        Dim utility As New ClsUtility()
        Dim idTipoProgetto As String = utility.TipologiaProgettoDaIdAttivita(HdyIdAttivita.Value, Session("conn"))
        Dim dataInizio As Date = Date.Parse(TxtDataInizio.Text)
        Dim CodVolontario As String
        Dim DataAvvioProgetto As String

        'Controllo se ho selezionato almeno un elemento dalla griglia
        If DtgProgetti.SelectedIndex = -1 Then
            LblErrore.Text = "Selezionare almeno un elemento dall'elenco dei progetti."
            Exit Sub
        Else
            If DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(5).Text = "&nbsp;" Then
                LblErrore.Text = "La graduatoria selezionata non e' stata ancora confermata."
                Exit Sub
            End If
        End If

        'aggiungo il controllo sul titolario
        'se c'è vado avanti con la cvonferma della graduatoria
        'altrimenti informo l'utente che non è possibile procedere finchè 
        'non aggiunge il titolario per il bando
        If ClsUtility.TrovaTitolario(DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(1).Text, Session("conn")) = True Then

            'se si tratta di BANDO conLOTUS = 1, se è così non faccio il controllo sulle date
            If txtLOTUS.Value = True Then
                'controllo la lunghezza del codice volontario immesso perchè deve essere lungo11
                If Len(txtCodiceVolontario.Text) < 11 Then
                    LblErrore.Text = "Il codice volontario deve essere composto da 11 caratteri."
                    Exit Sub
                End If
                If checkFormatoCodiceVolontario() = False Then
                    LblErrore.Text = "Il codice volontario deve essere composto da 4 numeri, la lettera ""V"" e 6 numeri (1234V567890)."
                    Exit Sub
                Else
                    'vado a caontrollare se il codice volontario esiste già
                    'Controllo che i dati del volontario non siano gia stati aggiornati
                    Dim MyDtrControllo2 As System.Data.SqlClient.SqlDataReader
                    MyDtrControllo2 = ClsServer.CreaDatareader("Select codicevolontario From Entità Where codicevolontario = '" & txtCodiceVolontario.Text & "'", Session("Conn"))
                    If MyDtrControllo2.HasRows = True Then
                        MyDtrControllo2.Close()
                        LblErrore.Text = "Attenzione. Il Codice Volontario risulta essere gia' presente sul Sistema."
                        Exit Sub
                    End If
                    MyDtrControllo2.Close()
                End If
            End If

            'Controllo che i dati del volontario non siano gia stati aggiornati
            Dim MyDtrControllo As System.Data.SqlClient.SqlDataReader
            MyDtrControllo = ClsServer.CreaDatareader("Select Ammesso,Stato From GraduatorieEntità Where IdEntità = " & Request.QueryString("Id"), Session("Conn"))
            MyDtrControllo.Read()
            If MyDtrControllo.Item("Ammesso") = 1 Or MyDtrControllo.Item("Stato") = 0 Then
                MyDtrControllo.Close()
                LblErrore.Text = "Il Volontario non possiede più le condizioni necessarie per essere inserito in un progetto."
                Exit Sub
            End If
            MyDtrControllo.Close()

            'se si tratta di BANDO conLOTUS = 1, se è così non faccio il controllo sulle date
            If txtLOTUS.Value = False Then
                'Controllo che la data di inizio attività sia compresa nella data inizio e nella data fine del progetto
                If CDate(TxtDataInizio.Text) < CDate(DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(5).Text) Or _
                CDate(TxtDataInizio.Text) >= CDate(DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(6).Text) Then
                    LblErrore.Text = "La data di avvio al servizio non è compreso nel range di date che va dal " & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(5).Text & " al " & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(6).Text & "."
                    Exit Sub
                End If
            End If
            'se si tratta di BANDO conLOTUS = 1, se è così non faccio il controllo sulla capienza del progetto
            If txtLOTUS.Value = False Then
                'Controllo che il progetto non sia pieno
                MyDtrControllo = ClsServer.CreaDatareader("Select ISNULL(Count(IdGraduatoriaEntità),0) AS Volontari From Attività " & _
                                                          "INNER JOIN AttivitàSediAssegnazione ON Attività.IdAttività = AttivitàSediAssegnazione.IdAttività " & _
                                                          "INNER JOIN GraduatorieEntità ON AttivitàSediAssegnazione.IdAttivitàSedeAssegnazione = GraduatorieEntità.IdAttivitàSedeAssegnazione " & _
                                                          "INNER JOIN Entità ON GraduatorieEntità.IdEntità = Entità.IdEntità " & _
                                                          "Where Attività.IdAttività = " & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(1).Text & " And Entità.IdStatoEntità IN (1,3) And GraduatorieEntità.Stato = 1 And GraduatorieEntità.Ammesso = 1", Session("Conn"))
                MyDtrControllo.Read()
                If MyDtrControllo.Item("Volontari") >= DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(11).Text Then
                    MyDtrControllo.Close()
                    LblErrore.Text = "Impossibile associare il volontario perchè il progetto risulta completo."
                    Exit Sub
                End If
                MyDtrControllo.Close()
            End If

            'Trovo IdAttivitàSedeAssegnazione
            Dim LngAttivitàSedeAssegnazione As Long
            MyDtrControllo = ClsServer.CreaDatareader("Select IdAttivitàSedeAssegnazione From AttivitàSediAssegnazione " & _
                                                      "INNER JOIN EntiSedi ON AttivitàSediAssegnazione.IdEnteSede = EntiSedi.IdEnteSede " & _
                                                      "INNER JOIN EntiSediAttuazioni ON EntiSedi.IdEnteSede = EntiSediAttuazioni.IdEnteSede " & _
                                                      "WHERE EntiSediAttuazioni.IdEnteSedeAttuazione = " & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(10).Text & " And AttivitàSediAssegnazione.IdAttività = " & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(1).Text, Session("Conn"))
            MyDtrControllo.Read()
            LngAttivitàSedeAssegnazione = MyDtrControllo.Item("IdAttivitàSedeAssegnazione")
            MyDtrControllo.Close()

            'Controllo se la sede di assegnazione ha capienza per contenere il volontario.  ANTONELLO
            If Not CInt(DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(13).Text) < CInt(DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(12).Text) Then

                LblErrore.Text = "Impossibile associare il volontario perchè la sede selezionata del progetto risulta completa."
                Exit Sub
            End If


            'FINO ANTONELLO

            'Controllo se la graduatoria risulta confermata
            Dim IntStatoEntità As Int16 '1 = Registrato  3 = In Servizio
            MyDtrControllo = ClsServer.CreaDatareader("Select StatoGraduatoria From AttivitàSediAssegnazione Where IdAttivitàSedeAssegnazione = " & LngAttivitàSedeAssegnazione, Session("Conn"))
            MyDtrControllo.Read()
            'controllo se è registrata
            If MyDtrControllo.Item("StatoGraduatoria") = 1 Then 'Registrata
                LblErrore.Text = "La graduatoria di destinazione è priva di volontari e pertanto va prima confermata."
                MyDtrControllo.Close()
                MyDtrControllo = Nothing
                Exit Sub
            End If
            If MyDtrControllo.Item("StatoGraduatoria") = 2 Then 'Presentata
                IntStatoEntità = 1
            End If
            If MyDtrControllo.Item("StatoGraduatoria") = 3 Then 'Confermata
                IntStatoEntità = 3
            End If
            MyDtrControllo.Close()

            'CONTROLLO STATO RIATTIVAZIONE
            'idattivitàentesedeattuazione: DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text
            'identità: Request.QueryString("Id")
            'dataavvio: TxtDataInizio.Text

            Dim strmessaggio As String = String.Empty
            Dim blnRiattivazione As Boolean

            blnRiattivazione = ControllaRiattivazioni(Request.QueryString("Id"), TxtDataInizio.Text, DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text, strmessaggio)

            If blnRiattivazione = False Then
                LblErrore.Text = strmessaggio
                Exit Sub
            End If

            'FINE CONTROLLO STATO RIATTIVAZIONE

            'CONTROLLO NEET
            strmessaggio = ControlloDataNeet(LngAttivitàSedeAssegnazione, Request.QueryString("Id"), TxtDataInizio.Text)
            If strmessaggio <> "" Then
                LblErrore.Text = strmessaggio
                Exit Sub
            End If

            'FINE CONTROLLO NEET

            'DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text -> IDAttivitàEnteSedeAttuazione
            'richiamo la store per il controllo della graduatora
            If ControlloGraduatoria(DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text, DataAvvioProgetto) = "NEGATIVO" Then
                Response.Write("<script>" & vbCrLf)
                Response.Write("window.open(""WfrmControlliGraduatoria.aspx?IdAttivita=" & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(1).Text & "&DataAvvio=" & DataAvvioProgetto & "&IdAttivitaEnteSedeAttuazione=" & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text & """, """", ""width=950,height=600,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf)
                Response.Write("</script>")
                Exit Sub
            End If


            'controllo se esiste interruzione temporanea su sede
            Dim DataFineEffettiva As Date
            MyDtrControllo = ClsServer.CreaDatareader("select " & _
                    " b.DataFineAttività +  case when b.DataInizioAttività < '16/04/2020' then isnull(DATEDIFF(DAY,'16/04/2020',DataRipresaServizio),0) else 0 end as DataFineEffettiva " & _
                    " from attivitàentisediattuazione a " & _
                    " inner join attività b on a.IDAttività = b.IDAttività" & _
                    " where a.IDAttivitàEnteSedeAttuazione = " & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text, Session("Conn"))
            MyDtrControllo.Read()

            DataFineEffettiva = MyDtrControllo.Item("DataFineEffettiva")

            MyDtrControllo.Close()
            'idattivitàentesedeattuazione


            'preparo i dati per l'inserimento della cronologia
            MyDtrControllo = ClsServer.CreaDatareader("Select IdStatoEntità, UserNameStato From Entità Where IdEntità = " & Request.QueryString("Id"), Session("Conn"))
            MyDtrControllo.Read()
            strCronoUser = MyDtrControllo.Item("UserNameStato")
            strCronoStato = MyDtrControllo.Item("IdStatoEntità")
            MyDtrControllo.Close()


            Dim MyDtrControllo4 As System.Data.SqlClient.SqlDataReader
            MyDtrControllo4 = ClsServer.CreaDatareader("Select isnull(codicevolontario,'') as codicevolontario From Entità Where  IdEntità = " & Request.QueryString("Id"), Session("Conn"))
            If MyDtrControllo4.HasRows = True Then
                MyDtrControllo4.Read()
                CodVolontario = MyDtrControllo4("codicevolontario")
                MyDtrControllo4.Close()
            End If



            Dim MyQuery As New System.Collections.ArrayList

            'se si tratta di BANDO conLOTUS = 1, se è così non faccio il controllo sulle date
            If txtLOTUS.Value = False Then

            End If
            Dim strPreUpdateEntita As String
            Dim strUpdateEntita As String

            'Pre Update su entita per generare la cronologia dell'assegnazione del volontario
            'SANDOKAN


            strPreUpdateEntita = "Update Entità Set "
            strPreUpdateEntita = strPreUpdateEntita & "IDAttivitàSedeAssegnazioneOriginale = b.idattivitàsedeassegnazione, "
            strPreUpdateEntita = strPreUpdateEntita & "UserNameAssegnazioneAltraSede = '" & ClsServer.NoApice(Session("Utente")) & "', "
            strPreUpdateEntita = strPreUpdateEntita & "DataAssegnazioneAltraSede = CONVERT(Datetime,GetDate(),103), "
            strPreUpdateEntita = strPreUpdateEntita & "TMPIdSedeAttuazioneOriginale = A.TMPIdSedeAttuazione "
            strPreUpdateEntita = strPreUpdateEntita & "from entità a inner join graduatorieentità b on a.identità = b.identità "
            strPreUpdateEntita = strPreUpdateEntita & "where a.identità = " & Request.QueryString("Id") & " And b.idattivitàsedeassegnazione <> " & LngAttivitàSedeAssegnazione
            MyQuery.Add(strPreUpdateEntita)


            'FINE SANDOKAN

            '******************************************************************************************
            'Aggiorno i campi in Entità che devono essere SEMPRE aggiornati
            strUpdateEntita = "Update Entità Set "
            ''Agg.il 21/04/2008 da simona cordella campo POSTOOCCUPATO = 1
            strUpdateEntita = strUpdateEntita & "TMPCodiceProgetto = '" & ClsServer.NoApice(DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(3).Text) & "', "
            strUpdateEntita = strUpdateEntita & "TMPIdSedeAttuazione = '" & ClsServer.NoApice(DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(10).Text) & "', "
            If txtcodreg.Value = "True" Then
                strUpdateEntita = strUpdateEntita & "IdSedePrimaAssegnazione = '" & ClsServer.NoApice(DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(10).Text) & "', "
            End If
            strUpdateEntita = strUpdateEntita & "IdStatoEntità = " & IntStatoEntità & " "
            strUpdateEntita = strUpdateEntita & "Where IdEntità = " & Request.QueryString("Id")

            MyQuery.Add(strUpdateEntita)
            '******************************************************************************************
            If IntStatoEntità = 3 Then

                'Aggiorno i campi in Entità che devono essere aggiornati SOLO SE il volontario assume lo stato di IN SERVIZIO
                strUpdateEntita = "Update Entità Set "
                strUpdateEntita = strUpdateEntita & "DataInizioServizio = CONVERT(Datetime,'" & TxtDataInizio.Text & "',103), "
                'se si tratta di BANDO conLOTUS = 1, metto la data fine messa dall'utente
                'If (idTipoProgetto <> PROGETTO_GARANZIA_GIOVANI) Then
                '    If txtLOTUS.Value = True Then
                '        strUpdateEntita = strUpdateEntita & "DataFineServizio = CONVERT(Datetime,'" & txtDataFine.Text & "',103), "
                '    Else
                '        strUpdateEntita = strUpdateEntita & "DataFineServizio = CONVERT(Datetime,'" & DataFineEffettiva & "',103), "
                '    End If
                'Else
                '    strUpdateEntita = strUpdateEntita & "DataFineServizio = CONVERT(Datetime,'" & DateAdd(DateInterval.Year, 1, DateAdd(DateInterval.Day, -1, dataInizio)) & "',103), "
                'End If
                'tolta la specifica garanzia giovani il 03/03/2021 su richiesta del Dipartimento
                If txtLOTUS.Value = True Then
                    strUpdateEntita = strUpdateEntita & "DataFineServizio = CONVERT(Datetime,'" & txtDataFine.Text & "',103), "
                Else
                    strUpdateEntita = strUpdateEntita & "DataFineServizio = CONVERT(Datetime,'" & DataFineEffettiva & "',103), "
                End If
                ''Agg.il 21/04/2008 da simona cordella campo POSTOOCCUPATO = 1
                strUpdateEntita = strUpdateEntita & " POSTOOCCUPATO = 1, "
                strUpdateEntita = strUpdateEntita & "IdStatoEntità = " & IntStatoEntità & ", "
                strUpdateEntita = strUpdateEntita & "DataUltimoStato = CONVERT(Datetime,GetDate(),103), "
                strUpdateEntita = strUpdateEntita & "UserNameStato = '" & ClsServer.NoApice(Session("Utente")) & "', "
                'se si tratta di BANDO conLOTUS = 1, metto il codice volontario scelto dall'utente
                If txtLOTUS.Value = True Then
                    strUpdateEntita = strUpdateEntita & "CodiceVolontario = '" & UCase(txtCodiceVolontario.Text) & "' "
                Else
                    'strUpdateEntita = strUpdateEntita & "username=(SELECT 'V' + left(codicefiscale,3) + replicate('0',6-len(convert(varchar,identità))) + convert(varchar, identità) from entità where identità=" & Request.QueryString("Id") & "), "
                    strUpdateEntita = strUpdateEntita & "username=dbo.FN_CalcoloUsernameVolontario(" & Request.QueryString("Id") & " ), "
                    strUpdateEntita = strUpdateEntita & "password='" & ClsUtility.CriptaNuovaPass & "' "

                    If CodVolontario = "" Then
                        strUpdateEntita = strUpdateEntita & ", CodiceVolontario = (SELECT 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE())) +  "
                        strUpdateEntita = strUpdateEntita & "CONVERT(VARCHAR(6),REPLICATE('0', 6-LEN(CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) ) )) +  "
                        strUpdateEntita = strUpdateEntita & "CONVERT(VARCHAR(6),ISNULL(MAX(CONVERT(INT,RIGHT(CodiceVolontario,6))),0) + 1) "
                        strUpdateEntita = strUpdateEntita & "From Entità WHERE SUBSTRING(CodiceVolontario,1,5) = 'V'+ CONVERT(NVARCHAR(4),YEAR(GETDATE()))) "
                    End If

                End If

                strUpdateEntita = strUpdateEntita & "Where IdEntità = " & Request.QueryString("Id")

                MyQuery.Add(strUpdateEntita)
            End If

                'Aggiorno i campi in AttivitàEntità
            If IntStatoEntità = 3 Then
                'If (idTipoProgetto <> PROGETTO_GARANZIA_GIOVANI) Then
                '    MyQuery.Add("Insert Into AttivitàEntità (IdAttivitàEnteSedeAttuazione,IdEntità,DataInizioAttivitàEntità,DataFineAttivitàEntità,IdStatoAttivitàEntità,PercentualeUtilizzo,IdTipologiaPosto) Values (" & _
                '                "'" & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text & "','" & Request.QueryString("Id") & "',CONVERT(DateTime,'" & TxtDataInizio.Text & "',103)," & _
                '                "CONVERT(Datetime,'" & DataFineEffettiva & "',103),1,100," & CboTipoPosto.SelectedValue & ")")

                'Else
                '    MyQuery.Add("Insert Into AttivitàEntità (IdAttivitàEnteSedeAttuazione,IdEntità,DataInizioAttivitàEntità,DataFineAttivitàEntità,IdStatoAttivitàEntità,PercentualeUtilizzo,IdTipologiaPosto) Values (" & _
                '    "'" & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text & "','" & Request.QueryString("Id") & "',CONVERT(DateTime,'" & TxtDataInizio.Text & "',103)," & _
                '    "CONVERT(DateTime,'" & DateAdd(DateInterval.Year, 1, DateAdd(DateInterval.Day, -1, dataInizio)) & "',103),1,100," & CboTipoPosto.SelectedValue & ")")
                'End If
                'tolta la specifica garanzia giovani il 03/03/2021 su richiesta del Dipartimento
                MyQuery.Add("Insert Into AttivitàEntità (IdAttivitàEnteSedeAttuazione,IdEntità,DataInizioAttivitàEntità,DataFineAttivitàEntità,IdStatoAttivitàEntità,PercentualeUtilizzo,IdTipologiaPosto) Values (" & _
                            "'" & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text & "','" & Request.QueryString("Id") & "',CONVERT(DateTime,'" & TxtDataInizio.Text & "',103)," & _
                            "CONVERT(Datetime,'" & DataFineEffettiva & "',103),1,100," & CboTipoPosto.SelectedValue & ")")

                MyQuery.Add("update attivitàentisediattuazione set statoassegnazione=2 where IdAttivitàEnteSedeAttuazione='" & DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(2).Text & "'")


            End If

                'Aggiorno i Campi in GraduatorieEntità
                MyQuery.Add("Update GraduatorieEntità Set " & _
                            "IdAttivitàSedeAssegnazione = " & LngAttivitàSedeAssegnazione & "," & _
                            "Ammesso = 1," & _
                            "IdTipologiaPosto = " & CboTipoPosto.SelectedValue & "," & _
                            "Username = '" & ClsServer.NoApice(Session("Utente")) & "'," & _
                            "DataModifica = CONVERT(Datetime,GetDate(),103) " & _
                            "Where IdEntità = " & Request.QueryString("Id"))

                If IntStatoEntità = 3 Then
                    'Inserimento CronologiaEntità    
                    MyQuery.Add("Insert Into CronologiaEntità (IDEntità, IDStatoEntità, UserNameStato, DataCronologia) Values (" & _
                                    "'" & Request.QueryString("Id") & "'," & strCronoStato & ",'" & strCronoUser & "',CONVERT(varchar, getdate(), 103))")

                    MyQuery.Add("Exec SP_AVVIO_SINGOLO " & Request.QueryString("Id") & ",'" & Session("Utente") & "'")
                End If

                'Rigenero l'ordinamento della graduatoria
                MyQuery.Add("Exec SP_ORDINA_GRADUATORIA " & LngAttivitàSedeAssegnazione)

                Dim blnCheckFascicolazione As Boolean = False
                Dim strsql As String
                Dim dtrGenerico As SqlClient.SqlDataReader

                If ClsServer.EseguiQueryColl(MyQuery, Session.SessionID, Session("Conn")) = False Then
                    LblErrore.Text = "Si sono verificati errori durante il processo di inserimento dati."
                Else
                    If IntStatoEntità = 3 Then
                        '*** 09/12/2010 verifico esistenza del codice fascicolo
                        strsql = "Select isnull(CodiceFascicolo,'') as CodiceFascicolo,CodiceVolontario from Entità where identità = " & Request.QueryString("Id")
                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        dtrGenerico.Read()
                        Dim codicevolontario As String
                        codicevolontario = dtrGenerico("CodiceVolontario")
                        If dtrGenerico("CodiceFascicolo") = "" Then
                            If Not dtrGenerico Is Nothing Then
                                dtrGenerico.Close()
                                dtrGenerico = Nothing
                            End If
                            'esito fascicolazione

                            'loggo su logfascicolivolontari
                            strsql = "INSERT INTO LogFascicoliVolontari([Username],[Metodo],[IdEntità],[DataOraRichiesta],[DataOraEsecuzione],[Eseguito])"
                            strsql = strsql & " VALUES('" & Session("Utente") & "','GeneraFascicolo','" & Request.QueryString("Id") & "',getdate(),NULL,0)"
                            myCommand = New System.Data.SqlClient.SqlCommand
                            myCommand.Connection = Session("conn")
                            myCommand.CommandText = strsql
                            myCommand.ExecuteNonQuery()

                            '---recupero l'id appena inserito
                            Dim strID As String
                            strsql = "select @@identity as Id"
                            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                            dtrGenerico.Read()
                            strID = dtrGenerico("Id")
                            dtrGenerico.Close()
                            dtrGenerico = Nothing

                            If ClsUtility.GeneraFascicolo(Session("Utente"), Request.QueryString("Id"), Session("conn"), strID) <> "0" Then      'errore
                                Dim strErr As String
                                strErr = "<table align=""center"" width=""750"" style=""width: 750px"" cellSpacing=""1"" cellPadding=""1""><tr><td align=""750"">"
                                strErr = strErr & "<table align=""center"" cellSpacing=""1"" cellPadding=""1""><tr><td><img src=""http://futuro.serviziocivile.it/images/FUTURO-BLU.jpg"" border=""0""></td></tr></table>"
                                strErr = strErr & "<table align=""center"" bgcolor=""orange"" width=""750""><tr><td><b>Errore generazione fascicolo: Maschera WFrmVolontarioProgetto.aspx</b></td></tr>"
                                strErr = strErr & "<tr><td><b>Utente:</b> " & Session("Utente") & "</td></tr>"
                                strErr = strErr & "<tr><td><b>IP:</b> " & HttpContext.Current.Request.UserHostAddress & "</td></tr>"
                                strErr = strErr & "<tr><td><b>Codice Volontario: </b>" & codicevolontario & "</td></tr>"
                                strErr = strErr & "</table>"
                                strErr = strErr & "</td></tr></table>"

                                blnCheckFascicolazione = True
                            ClsUtility.invioEmail("futuroweb@serviziocivile.it", "futuroweb@serviziocivile.it", "d.spagnulo@logicainformatica.it;a.dicroce@logicainformatica.it;c.ottaviani@logicainformatica.it", "ERRORE GENERAZIONE FASCICOLO", strErr)


                            Else
                                strsql = "update LogFascicoliVolontari set DataOraEsecuzione = getdate(), Eseguito=1 where IdLogFascicoliVolontari = " & strID
                                myCommand.CommandText = strsql
                                myCommand.ExecuteNonQuery()

                            End If
                        End If
                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If
                        '***



                        HdyIdAttivita.Value = DtgProgetti.Items(DtgProgetti.SelectedIndex).Cells(1).Text
                    If blnCheckFascicolazione = True Then
                        AggiornaSessioni()
                        Response.Redirect("WFrmVolontari.aspx?ErroreFascicolazione=" & "Non e' stato possibile creare il fascicolo SIGED." & "&IdVol=" & Request.QueryString("Id") & "&IdAttivita=" & HdyIdAttivita.Value)
                        Exit Sub
                    End If
                    End If

                End If
            Else
                '=========================================
                'SE NON C'E' IL TITOLARIO INFORMO L'UTENTE
                '=========================================
                LblErrore.Text = "Attenzione per il bando di riferimento del progetto non è associato un titolario. Provvedere all'associazione e ripetere l'operazione."
                Exit Sub
        End If
        AggiornaSessioni()
        Response.Redirect("WFrmVolontari.aspx?IdVol=" & Request.QueryString("Id") & "&IdAttivita=" & HdyIdAttivita.Value)

    End Sub

    Private Sub AggiornaSessioni()
        Dim DtrEnte As System.Data.SqlClient.SqlDataReader
        Dim StrSql As String

        StrSql = " SELECT DISTINCT e.idente, e.denominazione" & _
                 " FROM entità a inner join graduatorieentità b on a.identità = b.identità" & _
                 " inner join attivitàsediassegnazione c on b.idattivitàsedeassegnazione = c.idattivitàsedeassegnazione" & _
                 " inner join attività d on c.idattività = d.idattività" & _
                 " inner join enti e on d.identepresentante = e.idente" & _
                 " where a.identità = " & Request.QueryString("Id")
        DtrEnte = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        DtrEnte.Read()
        Session("IdEnte") = DtrEnte.Item("idente")
        Session("Denominazione") = DtrEnte.Item("denominazione")

        DtrEnte.Close()
        DtrEnte = Nothing
    End Sub

    Function checkFormatoCodiceVolontario() As Boolean
        'controllo il formato immesso dall'utente del codicevolontario
        'che dev'essere composto da 
        '4 numeri, una 'V' e 6 numeri: 1234V567890
        If IsNumeric(Left(txtCodiceVolontario.Text, 4)) = True And Mid(UCase(txtCodiceVolontario.Text), 5, 1) = "V" And IsNumeric(Right(txtCodiceVolontario.Text, 6)) = True Then
            checkFormatoCodiceVolontario = True
        Else
            checkFormatoCodiceVolontario = False
        End If

        Return checkFormatoCodiceVolontario
    End Function

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        LblErrore.Text = ""
        CaricaGriglia()
    End Sub

    Function controlliSalvataggioServer() As Boolean

        If TxtDataInizio.Text.Trim = String.Empty Then
            LblErrore.Text = "Inserire la data di avvio al servizio del volontario."
            TxtDataInizio.Focus()
            Return False
        End If

        Dim dataInizio As Date
        If (Date.TryParse(TxtDataInizio.Text, dataInizio) = False) Then
            LblErrore.Text = "Inserire la data in formato gg/mm/aaaa."
            TxtDataInizio.Focus()
            Return False
        End If

        If txtDataFine.Visible = True Then

            If txtDataFine.Text.Trim = String.Empty Then
                LblErrore.Text = "Inserire la data di fine servizio del volontario."
                txtDataFine.Focus()
                Return False
            End If

            Dim dataFine As Date
            If (Date.TryParse(txtDataFine.Text, dataFine) = False) Then
                LblErrore.Text = "Inserire la data in formato gg/mm/aaaa."
                txtDataFine.Focus()
                Return False
            End If

        End If

        If txtCodiceVolontario.Visible = True Then
            If txtCodiceVolontario.Text.Trim = String.Empty Then
                LblErrore.Text = "Inserire il codice del volontario."
                txtCodiceVolontario.Focus()
                Return False
            End If
        End If

        Return True

    End Function

    Private Function ControllaRiattivazioni(ByVal IdVolontario As Integer, ByVal DataAvvio As Date, ByVal IdAttivitàEnteSedeAttuazione As Integer, ByRef messaggio As String) As Boolean

        Dim esito As Boolean
        Dim MySqlCommand As SqlClient.SqlCommand

        MySqlCommand = New SqlClient.SqlCommand
        MySqlCommand.CommandType = CommandType.StoredProcedure
        MySqlCommand.CommandText = "[SP_VERIFICA_SOSTITUISCI_VOLONTARIO]"
        MySqlCommand.Connection = Session("conn")

        Try

            MySqlCommand.Parameters.Add("@IdVolontario", SqlDbType.Int).Value = IdVolontario
            MySqlCommand.Parameters("@IdVolontario").Direction = ParameterDirection.Input

            MySqlCommand.Parameters.Add("@DataAvvio", SqlDbType.DateTime).Value = DataAvvio
            MySqlCommand.Parameters("@DataAvvio").Direction = ParameterDirection.Input

            MySqlCommand.Parameters.Add("@IdAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = IdAttivitàEnteSedeAttuazione
            MySqlCommand.Parameters("@IdAttivitàEnteSedeAttuazione").Direction = ParameterDirection.Input

            MySqlCommand.Parameters.Add("@Esito", SqlDbType.Bit)
            MySqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output

            MySqlCommand.Parameters.Add("@MESSAGGIO", SqlDbType.VarChar)
            MySqlCommand.Parameters("@MESSAGGIO").Direction = ParameterDirection.Output
            MySqlCommand.Parameters("@MESSAGGIO").Size = 8000

            MySqlCommand.ExecuteNonQuery()

            esito = MySqlCommand.Parameters("@Esito").Value
            messaggio = MySqlCommand.Parameters("@MESSAGGIO").Value

        Catch ex As Exception
            'Response.Write(ex.Message.ToString())
        End Try

        Return esito

    End Function

    Private Function ControlloDataNeet(ByVal IdAttivitaSedeAssegnazione As Integer, ByVal IdEntità As Integer, ByVal DataAvvio As String) As String

        Dim sqlDAP As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strEsito As String
        Dim strNomeStore As String = "[SP_GG_VERIFICA_DATE_CONTROLLI_NEET_SINGOLO]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdAttivitàSedeAssegnazione", SqlDbType.Int).Value = IdAttivitaSedeAssegnazione
            sqlDAP.SelectCommand.Parameters.Add("@IdEntità", SqlDbType.Int).Value = IdEntità
            sqlDAP.SelectCommand.Parameters.Add("@DataAvvio", SqlDbType.Date).Value = CDate(DataAvvio)

            Dim sparam0 As SqlClient.SqlParameter
            sparam0 = New SqlClient.SqlParameter
            sparam0.ParameterName = "@Esito"
            sparam0.Size = 100
            sparam0.SqlDbType = SqlDbType.Int
            sparam0.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam0)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Motivazione"
            sparam1.Size = 1000
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam1)

            sqlDAP.Fill(dataSet)
            strEsito = sqlDAP.SelectCommand.Parameters("@Motivazione").Value
            Return strEsito
        Catch ex As Exception
            'lblMessaggioAlert.Visible = True
            'lblMessaggioAlert.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            'Exit Function
        End Try


    End Function

    Private Function ControlloGraduatoria(ByVal IdAttivitaEnteSedeAttuazione As Integer, ByRef DataAvvioProgetto As String) As String

        Dim sqlDAP As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strEsito As String
        Dim strNomeStore As String = "[SP_GRADUATORIA_CONTROLLI_V3]"
        Dim IdAttivita As Integer

        Dim DtrAppo As System.Data.SqlClient.SqlDataReader
        Dim StrSql As String

        StrSql = " SELECT DISTINCT a.idattività, dbo.formatodata(a.datainizioattività) as DataAvvio" & _
                 " FROM attività a inner join attivitàentisediattuazione aesa on a.idattività = aesa.idattività" & _
                 " where aesa.idattivitàentesedeattuazione = " & IdAttivitaEnteSedeAttuazione
        DtrAppo = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        DtrAppo.Read()
        IdAttivita = DtrAppo.Item("idattività")
        DataAvvioProgetto = DtrAppo.Item("DataAvvio")

        DtrAppo.Close()
        DtrAppo = Nothing


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = IdAttivita
            sqlDAP.SelectCommand.Parameters.Add("@DataAvvio", SqlDbType.Date).Value = CDate(DataAvvioProgetto)
            sqlDAP.SelectCommand.Parameters.Add("@IDAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = IdAttivitaEnteSedeAttuazione

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam1)

            sqlDAP.Fill(dataSet)
            strEsito = sqlDAP.SelectCommand.Parameters("@Esito").Value
            Return strEsito
        Catch ex As Exception
            LblErrore.Visible = True
            LblErrore.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            Exit Function
        End Try


    End Function
End Class