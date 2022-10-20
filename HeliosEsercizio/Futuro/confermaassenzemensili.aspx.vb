Public Class confermaassenzemensili
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'controllo se effettuato login
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            Session("LocalDataSet") = Nothing
            lblDenominazioneEnte.Text = Session("Denominazione")
            lblCodEnte.Text = Session("CodiceRegioneEnte")

            'vado a calcolarmi il totale delle assenze da confermare
            lblTotAssenze.Text = TotaleAssenze()

            'If lblTotAssenze.Text <> "0" Then
            If CheckMese() = False Then
                cmdConferma.Visible = False
                tblConferma.Visible = False
                lblMeseSuccessivo.Text = "Nessuno"
                lblTotAssenze.Text = "0"
                lblmessaggiosopra.Visible = True
                lblmessaggiosopra.Text = "Non ci sono assenze da confermare."
            Else
                Dim dtrLeggiDati4 As SqlClient.SqlDataReader
                Dim strMeseAttuale As String
                Dim strAnnoAttuale As String

                'chiudo il datareader
                If Not dtrLeggiDati4 Is Nothing Then
                    dtrLeggiDati4.Close()
                    dtrLeggiDati4 = Nothing
                End If

                'prendo la data dal server
                dtrLeggiDati4 = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
                dtrLeggiDati4.Read()
                'passo la data odierna ad una variabile locale
                strMeseAttuale = IIf(Len(CStr(Month(dtrLeggiDati4("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati4("dataOggi"))), CStr(Month(dtrLeggiDati4("dataOggi"))))
                strAnnoAttuale = Year(dtrLeggiDati4("dataOggi"))
                'controllo il mese e l'anno
                If strMeseAttuale = "01" Then
                    strAnnoAttuale = strAnnoAttuale - 1
                    strMeseAttuale = "12"
                Else
                    strAnnoAttuale = strAnnoAttuale
                    strMeseAttuale = strMeseAttuale - 1
                End If
                'chiudo il datareader
                If Not dtrLeggiDati4 Is Nothing Then
                    dtrLeggiDati4.Close()
                    dtrLeggiDati4 = Nothing
                End If

                tblConferma.Visible = True
                cmdConferma.Visible = True

                'controllo se ci sono volontari in servizio 
                Dim dtrLeggiDati As SqlClient.SqlDataReader
                Dim strsql1 As String
                strsql1 = "SELECT  entità.DataFineServizio AS DataF, entità.DataInizioServizio AS DataI, entità.IDEntità AS IdEntità, enti.CodiceRegione AS codReg"
                strsql1 = strsql1 & " FROM entità "
                strsql1 = strsql1 & " INNER Join attivitàentità ON entità.IDEntità = attivitàentità.IDEntità"
                strsql1 = strsql1 & " INNER Join attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione"
                strsql1 = strsql1 & " inner join attività on attivitàentisediattuazione.idattività = attività.idattività"
                strsql1 = strsql1 & " INNER Join enti ON attività.IDEntepresentante = enti.IDEnte"
                strsql1 = strsql1 & " INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
                strsql1 = strsql1 & " WHERE (enti.CodiceRegione = '" & Session("txtCodEnte") & "') "
                strsql1 = strsql1 & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' AND TipiProgetto.MacroTipoProgetto ='SCN' "
                'Autore : Testa Guido
                'Data : 18-09-2006
                'Modifica : Il controllo temporale viene baipassato se si tratta di UNSC o Regione
                If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
                    strsql1 = strsql1 & " AND ( '" & strAnnoAttuale & "-" & IIf(Len(strMeseAttuale) < 2, "0" & strMeseAttuale, strMeseAttuale) & "'"
                    strsql1 = strsql1 & " BETWEEN"
                    strsql1 = strsql1 & " convert(varchar,year(entità.DataInizioServizio)) + '-' + REPLICATE('0',2-LEN(convert(varchar,month(entità.DataInizioServizio)))) + convert(varchar,month(entità.DataInizioServizio)) "
                    strsql1 = strsql1 & " AND "
                    strsql1 = strsql1 & " convert(varchar,year(entità.DataFineServizio)) + '-' + REPLICATE('0',2-LEN(convert(varchar,month(entità.DataFineServizio)))) + convert(varchar,month(entità.DatafINEServizio)))"
                End If
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                dtrLeggiDati = ClsServer.CreaDatareader(strsql1, Session("conn"))
                dtrLeggiDati.Read()

                If dtrLeggiDati.HasRows = True Then
                    cmdConferma.Visible = True
                    tblConferma.Visible = True
                    lblNoVol.Visible = False
                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If

                    CaricaGriglietta()
                Else
                    cmdConferma.Visible = False
                    tblConferma.Visible = False
                    lblNoVol.Visible = True
                End If
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If
            End If
            'Else
            '    imgConferma.Visible = False
            '    lblVisMeseSuccessivo.Visible = False
            '    lblVisTotAssenze.Visible = False
            '    lblTotAssenze.Visible = False
            '    lblMeseSuccessivo.Visible = False
            '    Imgerrore.Visible = True
            '    lblmessaggiosopra.Visible = True
            '    lblmessaggiosopra.Text = "Non ci sono assenze da confermare."
            'End If

            'carico comunque le assenze confermate

            CaricaAssenzeConfermate()

        End If
    End Sub

    Function TotaleAssenze() As String

        Dim strsql As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim strMeseAttuale As String
        Dim strAnnoAttuale As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'prendo la data dal server
        dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
        dtrLeggiDati.Read()
        'passo la data odierna ad una variabile locale
        strMeseAttuale = IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi"))))
        strAnnoAttuale = Year(dtrLeggiDati("dataOggi"))

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(sum(isnull(giorni,0)),0) as TotaleGiorniInseriti "
        strsql = strsql & "FROM enti "
        strsql = strsql & "inner JOIN attività ON enti.IDEnte = attività.IDEntePresentante "
        strsql = strsql & "inner JOIN statiattività on attività.idstatoattività=statiattività.idstatoattività "
        strsql = strsql & "inner join attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività "
        strsql = strsql & "inner JOIN attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàenteSedeAttuazione "
        strsql = strsql & "inner JOIN entità ON attivitàentità.IDEntità = entità.IDEntità "
        strsql = strsql & "inner JOIN EntitàAssenze ON entità.IDEntità = EntitàAssenze.IDEntità "
        strsql = strsql & " INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
        strsql = strsql & "WHERE "
        If strMeseAttuale = "01" Then
            strsql = strsql & "entitàassenze.anno='" & CInt(strAnnoAttuale) - 1 & "' "
            strsql = strsql & "and entitàassenze.Mese=12 "
        Else
            strsql = strsql & "entitàassenze.anno='" & CInt(strAnnoAttuale) & "' "
            strsql = strsql & "and entitàassenze.Mese='" & CInt(strMeseAttuale) - 1 & "' "
        End If
        strsql = strsql & "and (enti.idente = '" & Session("IdEnte") & "') "
        strsql = strsql & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'  AND TipiProgetto.MacroTipoProgetto ='SCN' "

        'strsql = "select count(*) as ConteggioAssenze , attività.datainizioattività, attività.datafineattività "
        'strsql = strsql & "FROM enti "
        'strsql = strsql & "INNER JOIN attività ON enti.IDEnte = attività.IDEntePresentante "
        'strsql = strsql & "INNER JOIN statiattività on attività.idstatoattività=statiattività.idstatoattività "
        'strsql = strsql & "inner join attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività "
        'strsql = strsql & "INNER JOIN attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàenteSedeAttuazione "
        'strsql = strsql & "INNER JOIN entità ON attivitàentità.IDEntità = entità.IDEntità "
        'strsql = strsql & "INNER JOIN EntitàAssenze ON entità.IDEntità = EntitàAssenze.IDEntità "
        'If strMeseAttuale = "01" Then
        '    strsql = strsql & "WHERE ('01/'+convert(varchar,month(getdate()-1))+'/'+convert(varchar,year(getdate())) between attività.datainizioattività and attività.datafineattività) and (statiattività.attiva=1) and (entitàassenze.anno='" & CInt(strAnnoAttuale) - 1 & "') and (entitàassenze.Mese=12 and (entitàassenze.stato=1) and (enti.idente = '" & Session("IdEnte") & "') "
        'Else
        '    strsql = strsql & "WHERE ('01/'+convert(varchar,month(getdate()-1))+'/'+convert(varchar,year(getdate())) between attività.datainizioattività and attività.datafineattività) and (statiattività.attiva=1) and (entitàassenze.anno='" & strAnnoAttuale & "') and (entitàassenze.Mese=" & CInt(strMeseAttuale) - 1 & ") and (entitàassenze.stato=1) and (enti.idente = '" & Session("IdEnte") & "') "
        'End If
        'strsql = strsql & "group by attività.datainizioattività, attività.datafineattività"


        'prendo la data dal server
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrLeggiDati.HasRows = True Then
            dtrLeggiDati.Read()
            TotaleAssenze = dtrLeggiDati("TotaleGiorniInseriti")
        Else
            TotaleAssenze = ""
        End If

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
    End Function

    Function CheckMese() As Boolean 'true deve inserire, false sola visualizzazione
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim strsql As String
        'datareader locale 
        Dim dtsLocal As DataSet
        Dim strMeseAttuale As String
        Dim strAnnoAttuale As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'prendo la data dal server
        dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
        dtrLeggiDati.Read()
        'passo la data odierna ad una variabile locale
        strMeseAttuale = IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi"))))
        strAnnoAttuale = Year(dtrLeggiDati("dataOggi"))

        Select Case CInt(strMeseAttuale) - 1
            Case 0
                lblMeseSuccessivo.Text = "Dicembre"
                lblMeseConferma.Text = UCase("Dicembre") & " " & CInt(strAnnoAttuale) - 1
            Case 1
                lblMeseSuccessivo.Text = "Gennaio"
                lblMeseConferma.Text = UCase("Gennaio") & " " & strAnnoAttuale
            Case 2
                lblMeseSuccessivo.Text = "Febbraio"
                lblMeseConferma.Text = UCase("Febbraio") & " " & strAnnoAttuale
            Case 3
                lblMeseSuccessivo.Text = "Marzo"
                lblMeseConferma.Text = UCase("Marzo") & " " & strAnnoAttuale
            Case 4
                lblMeseSuccessivo.Text = "Aprile"
                lblMeseConferma.Text = UCase("Aprile") & " " & strAnnoAttuale
            Case 5
                lblMeseSuccessivo.Text = "Maggio"
                lblMeseConferma.Text = UCase("Maggio") & " " & strAnnoAttuale
            Case 6
                lblMeseSuccessivo.Text = "Giugno"
                lblMeseConferma.Text = UCase("Giugno") & " " & strAnnoAttuale
            Case 7
                lblMeseSuccessivo.Text = "Luglio"
                lblMeseConferma.Text = UCase("Luglio") & " " & strAnnoAttuale
            Case 8
                lblMeseSuccessivo.Text = "Agosto"
                lblMeseConferma.Text = UCase("Agosto") & " " & strAnnoAttuale
            Case 9
                lblMeseSuccessivo.Text = "Settembre"
                lblMeseConferma.Text = UCase("Settembre") & " " & strAnnoAttuale
            Case 10
                lblMeseSuccessivo.Text = "Ottobre"
                lblMeseConferma.Text = UCase("Ottobre") & " " & strAnnoAttuale
            Case 11
                lblMeseSuccessivo.Text = "Novembre"
                lblMeseConferma.Text = UCase("Novembre") & " " & strAnnoAttuale
        End Select

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'si tratta di gennaio, quindi passo a dicembre dell'anno prima e all'anno prima
        If CInt(strMeseAttuale) - 1 = 0 Then
            strMeseAttuale = "12"
            strAnnoAttuale = CInt(strAnnoAttuale) - 1
        Else ' altrimenti devo fare il mese = mese - 1
            strMeseAttuale = CInt(strMeseAttuale) - 1
        End If

        strsql = "select Mese, Anno from EntiConfermaAssenze where (Anno=" & CInt(strAnnoAttuale) & ") and IdEnte='" & Session("IdEnte") & "' and (Mese=" & CInt(strMeseAttuale) & " ) "
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

        'se risulta essere già stato confermato il mese corrente vado in sola visualizzazione
        'altrimenti mostro il tasto di conferma
        If dtrLeggiDati.HasRows = True Then
            CheckMese = False
        Else
            CheckMese = True
        End If

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

    End Function

    Sub CaricaGriglietta()
        'Antonello Di Croce 11/11/2005
        Dim dtGriglietta1 As New DataTable
        Dim drGriglietta1 As DataRow
        Dim strMeseAttuale As String
        Dim strAnnoAttuale As String
        'INIZIO FASE NUOVA
        '---------------------------------------------------------------------------------------------

        Dim strsql9 As String
        Dim dtrLeggiDati9 As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati9 Is Nothing Then
            dtrLeggiDati9.Close()
            dtrLeggiDati9 = Nothing
        End If

        'prendo la data dal server
        dtrLeggiDati9 = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
        dtrLeggiDati9.Read()
        'passo la data odierna ad una variabile locale

        strMeseAttuale = IIf(Len(CStr(Month(dtrLeggiDati9("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati9("dataOggi"))), CStr(Month(dtrLeggiDati9("dataOggi"))))
        strAnnoAttuale = Year(dtrLeggiDati9("dataOggi"))
        If strMeseAttuale = "01" Then
            strAnnoAttuale = strAnnoAttuale - 1
            strMeseAttuale = "12"
        Else
            strAnnoAttuale = strAnnoAttuale
            strMeseAttuale = strMeseAttuale - 1
        End If


        'chiudo il datareader
        If Not dtrLeggiDati9 Is Nothing Then
            dtrLeggiDati9.Close()
            dtrLeggiDati9 = Nothing
        End If

        Dim dtrLeggiDati1 As SqlClient.SqlDataReader
        Dim strsql2 As String
        Dim miadata As String

        Dim dtrLeggiDati2 As SqlClient.SqlDataReader
        Dim strsql3 As String
        Dim A As Integer 'anno 
        Dim M As Integer 'mese

        Dim dtrLeggiDati6 As SqlClient.SqlDataReader
        Dim strsql4 As String
        Dim A1 As Integer 'anno
        Dim M1 As String 'mese
        Dim DataInizio As String

        'con questa query capisco che se non ci sono righe devo inserire tuti i mesi mancanti,se invece ci sono righe devo inserire in un intervallo di mese o solo il mese da confermare.
        strsql3 = "SELECT enti.CodiceRegione AS codReg, EntiConfermaAssenze.Anno, EntiConfermaAssenze.Mese, EntiConfermaAssenze.DataConferma"
        strsql3 = strsql3 & " FROM EntiConfermaAssenze INNER JOIN"
        strsql3 = strsql3 & " enti ON EntiConfermaAssenze.IdEnte = enti.IDEnte"
        strsql3 = strsql3 & " WHERE(enti.CodiceRegione = '" & Session("txtCodEnte") & "') "
        strsql3 &= "ORDER BY Anno DESC, Mese DESC, DataConferma DESC"
        dtrLeggiDati2 = ClsServer.CreaDatareader(strsql3, Session("conn"))
        dtrLeggiDati2.Read()

        If dtrLeggiDati2.HasRows = True Then
            A = CInt(dtrLeggiDati2("Anno"))
            M = CInt(dtrLeggiDati2("Mese")) + 1

            If M = 13 Then
                M = 1
                A = A + 1
            End If
            If Not dtrLeggiDati2 Is Nothing Then
                dtrLeggiDati2.Close()
                dtrLeggiDati2 = Nothing
            End If

            'modificata IF da Danilo il 09/08/2012. vecchia IF:
            'If IIf(strMeseAttuale = 12, 1, strMeseAttuale + 1) = M Then
            If IIf(strMeseAttuale = 12, 1, strMeseAttuale + 1) = M And IIf(strMeseAttuale = 12, strAnnoAttuale + 1, strAnnoAttuale) = A Then
                Exit Sub
            End If

        Else
            If Not dtrLeggiDati2 Is Nothing Then
                dtrLeggiDati2.Close()
                dtrLeggiDati2 = Nothing
            End If
            'SE NON CI SONO ASSENZE MAI CONFERMATE ALLORA

            strsql4 = "SELECT  min(entità.DataInizioServizio) AS DataI"
            strsql4 = strsql4 & " FROM entità "
            strsql4 = strsql4 & " INNER Join attivitàentità ON entità.IDEntità = attivitàentità.IDEntità  "
            strsql4 = strsql4 & " INNER Join attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione  "
            strsql4 = strsql4 & " inner join attività on attivitàentisediattuazione.idattività = attività.idattività "
            strsql4 = strsql4 & " INNER Join enti ON attività.IDEntepresentante = enti.IDEnte "
            strsql4 = strsql4 & " INNER Join TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
            strsql4 = strsql4 & " WHERE TipiProgetto.MacroTipoProgetto ='SCN' and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'  AND enti.CodiceRegione = '" & Session("txtCodEnte") & "' and entità.datainizioservizio <> entità.datafineservizio"
            dtrLeggiDati6 = ClsServer.CreaDatareader(strsql4, Session("conn"))
            dtrLeggiDati6.Read()
            DataInizio = dtrLeggiDati6("DataI")

            If dtrLeggiDati6.HasRows = True Then

                A1 = DataInizio.Substring(6, 4)
                M1 = DataInizio.Substring(3, 2)

                If Not dtrLeggiDati6 Is Nothing Then
                    dtrLeggiDati6.Close()
                    dtrLeggiDati6 = Nothing
                End If

            End If

        End If
        If Not dtrLeggiDati2 Is Nothing Then
            dtrLeggiDati2.Close()
            dtrLeggiDati2 = Nothing
        End If
        If Not dtrLeggiDati6 Is Nothing Then
            dtrLeggiDati6.Close()
            dtrLeggiDati6 = Nothing
        End If
        dtGriglietta1.Columns.Add()
        dtGriglietta1.Columns.Add(New DataColumn("Mese1", GetType(String)))
        dtGriglietta1.Columns.Add(New DataColumn("Anno1", GetType(String)))
        dtGriglietta1.Columns.Add(New DataColumn("MeseNum", GetType(String)))

        ' Aggiunta il 31/01/2019 
        ' Luigi Leucci
        ' Caso in cui l'ente non abbia mesi da confermare
        If A = 0 And M = 0 And A1 = Now.Year And Val(M1) = Val(Now.Month) Then
            cmdConferma.Visible = False
            tblConferma.Visible = False
            lblNoVol.Visible = True
            Exit Sub
        End If
        ' -----------------------

        Dim ControlloEsistenzaEntità As Boolean = True
        Do While ControlloEsistenzaEntità = True
            strMeseAttuale = IIf(Len(CStr(strMeseAttuale)) < 2, "0" & (CStr(strMeseAttuale)), (CStr(strMeseAttuale)))
            miadata = strAnnoAttuale & "-" & strMeseAttuale
            strsql2 = "SELECT "
            strsql2 = strsql2 & " entità.DataFineServizio AS DataF, "
            strsql2 = strsql2 & " entità.DataInizioServizio AS DataI, "
            strsql2 = strsql2 & " entità.IDEntità AS IdEntità, "
            strsql2 = strsql2 & " enti.CodiceRegione AS codReg "
            strsql2 = strsql2 & " FROM entità"
            strsql2 = strsql2 & " INNER Join attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
            strsql2 = strsql2 & " INNER Join attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
            strsql2 = strsql2 & " inner join attività on attivitàentisediattuazione.idattività = attività.idattività"
            strsql2 = strsql2 & " INNER Join enti ON attività.IDEntepresentante = enti.IDEnte"
            strsql2 = strsql2 & " INNER Join TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
            strsql2 = strsql2 & " WHERE TipiProgetto.MacroTipoProgetto ='SCN' and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'  AND (enti.CodiceRegione = '" & Session("txtCodEnte") & "')  "
            'Autore : Testa Guido
            'Data : 18-09-2006
            'Modifica : Il controllo temporale viene baipassato se si tratta di UNSC o Regione
            If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
                strsql2 = strsql2 & " AND ( '" & miadata & "'"
                strsql2 = strsql2 & " BETWEEN"
                strsql2 = strsql2 & " convert(varchar,year(entità.DataInizioServizio)) + '-' + REPLICATE('0',2-LEN(convert(varchar,month(entità.DataInizioServizio)))) + convert(varchar,month(entità.DataInizioServizio)) "
                strsql2 = strsql2 & " AND "
                strsql2 = strsql2 & " convert(varchar,year(entità.DataFineServizio)) + '-' + REPLICATE('0',2-LEN(convert(varchar,month(entità.DataFineServizio)))) + convert(varchar,month(entità.DatafINEServizio)))"
            End If

            If Not dtrLeggiDati1 Is Nothing Then
                dtrLeggiDati1.Close()
                dtrLeggiDati1 = Nothing
            End If

            dtrLeggiDati1 = ClsServer.CreaDatareader(strsql2, Session("conn"))
            dtrLeggiDati1.Read()
            Dim ControlloRighe As Integer
            If dtrLeggiDati1.HasRows = True Then
                ControlloRighe = 1
            Else
                ControlloRighe = 0
            End If
            If Not dtrLeggiDati1 Is Nothing Then
                dtrLeggiDati1.Close()
                dtrLeggiDati1 = Nothing
            End If
            If ControlloRighe = 1 Then

                If (CInt(strAnnoAttuale) = A) And (CInt(strMeseAttuale) = M) Then
                    ControlloEsistenzaEntità = False
                End If

                If (CInt(strAnnoAttuale) = A1) And (CInt(strMeseAttuale) = M1) Then
                    ControlloEsistenzaEntità = False
                End If

                'carico valori nel data rider 
                drGriglietta1 = dtGriglietta1.NewRow()
                drGriglietta1(1) = strMeseAttuale
                drGriglietta1(2) = strAnnoAttuale
                drGriglietta1(3) = strMeseAttuale
                dtGriglietta1.Rows.Add(drGriglietta1)


                If Not dtrLeggiDati1 Is Nothing Then
                    dtrLeggiDati1.Close()
                    dtrLeggiDati1 = Nothing
                End If
            Else
                ControlloEsistenzaEntità = False
                If Not dtrLeggiDati1 Is Nothing Then
                    dtrLeggiDati1.Close()
                    dtrLeggiDati1 = Nothing
                End If
            End If '-----------------

            If strMeseAttuale = "01" Then
                strAnnoAttuale = strAnnoAttuale - 1
                strMeseAttuale = "12"
            Else
                strAnnoAttuale = strAnnoAttuale
                strMeseAttuale = strMeseAttuale - 1
            End If


        Loop
        Dim dtGriglietta2 As New DataTable
        dtGriglietta2 = dtGriglietta1
        Dim x As Integer

        'assegno alla data grid il datatable
        For x = 0 To dtGriglietta1.Rows.Count - 1
            Select Case dtGriglietta1.Rows(x).Item("Mese1")
                Case Is = "01"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Gennaio"
                Case Is = "02"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Febbraio"
                Case Is = "03"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Marzo"
                Case Is = "04"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Aprile"
                Case Is = "05"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Maggio"
                Case Is = "06"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Giugno"
                Case Is = "07"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Luglio"
                Case Is = "08"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Agosto"
                Case Is = "09"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Settembre"
                Case Is = "10"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Ottobre"
                Case Is = "11"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Novembre"
                Case Is = "12"
                    dtGriglietta2.Rows(x).Item("Mese1") = "Dicembre"
            End Select
        Next
        'DtgMesiDaInserire.DataSource = dtGriglietta1
        DtgMesiDaInserire.DataSource = dtGriglietta2
        DtgMesiDaInserire.DataBind()
        DtgMesiDaInserire.SelectedIndex = -1


        'FINE FASE NUOVA

        '-------------------------------------------------------------------------------------------
    End Sub

    Sub CaricaAssenzeConfermate()
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim strsql As String
        'datareader locale 
        Dim dtsLocal As DataSet

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'prendo la data dal server
        dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
        dtrLeggiDati.Read()
        'passo la data odierna ad una variabile locale
        lblDataOdierna.Text = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select IdEnteConfermaAssenze,DataConferma, "
        strsql = strsql & "IdEnte, "
        strsql = strsql & "Anno, EntiConfermaAssenze.mese as Mesi, "
        strsql = strsql & "case Mese "
        strsql = strsql & "when 1 then 'Gennaio' "
        strsql = strsql & "when 2 then 'Febbraio' "
        strsql = strsql & "when 3 then 'Marzo' "
        strsql = strsql & "when 4 then 'Aprile' "
        strsql = strsql & "when 5 then 'Maggio' "
        strsql = strsql & "when 6 then 'Giugno' "
        strsql = strsql & "when 7 then 'Luglio' "
        strsql = strsql & "when 8 then 'Agosto' "
        strsql = strsql & "when 9 then 'Settembre' "
        strsql = strsql & "when 10 then 'Ottobre' "
        strsql = strsql & "when 11 then 'Novembre' "
        strsql = strsql & "when 12 then 'Dicembre' "
        strsql = strsql & "end as Mese, "
        strsql = strsql & "'Confermato' as Stato "
        strsql = strsql & "from EntiConfermaAssenze "
        strsql = strsql & "where IdEnte=" & Session("IdEnte") & " order by anno desc, Mesi desc"

        'eseguo la query e passo il risultato al datareader
        dtsLocal = ClsServer.DataSetGenerico(strsql, Session("conn"))

        'controllo se ci sono sedi di attuazione assegnate al volontario selezionato
        If dtsLocal.Tables(0).Rows.Count > 0 Then
            Session("LocalDataSet") = dtsLocal
            dtgRisultatoRicerca.DataSource = dtsLocal
            dtgRisultatoRicerca.DataBind()
        Else
            lblmessaggiosopra.Visible = True
            lblmessaggiosopra.Text = "Nessuna Assenza Confermata."
            'PulisciDataGrid(dtgRisultatoRicerca)
        End If

        dtsLocal.Dispose()
        dtsLocal = Nothing

    End Sub

    Sub PulisciDataGrid(ByVal GridDaPulire As DataGrid)
        Dim dtRigheVuote As New DataTable
        Dim drRigheVuote As DataRow
        Dim i As Integer
        dtRigheVuote.Columns.Add()
        dtRigheVuote.Columns.Add(New DataColumn("IdEnteConfermaAssenze", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("IdEnte", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("Anno", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("Mese", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("Stato", GetType(String)))
        drRigheVuote = dtRigheVuote.NewRow()
        drRigheVuote(1) = ""
        drRigheVuote(2) = ""
        drRigheVuote(3) = "Nessuna Assenza Confermata."
        drRigheVuote(4) = ""
        drRigheVuote(5) = ""
        dtRigheVuote.Rows.Add(drRigheVuote)
        For i = 1 To 3
            drRigheVuote = dtRigheVuote.NewRow()
            drRigheVuote(1) = ""
            drRigheVuote(2) = ""
            drRigheVuote(3) = ""
            drRigheVuote(4) = ""
            drRigheVuote(5) = ""
            dtRigheVuote.Rows.Add(drRigheVuote)
        Next
        GridDaPulire.DataSource = dtRigheVuote
        GridDaPulire.DataBind()
        GridDaPulire.SelectedIndex = -1
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub cmdConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConferma.Click
        'AggiornaDati()
        ConfermaAssenze()
    End Sub

    Sub ConfermaAssenze()
        'modificata il 05/04/2012 da simona cordella
        'routine per il salvataggio delle assenze mensili
        Dim strsql As String
        Dim myCommand As System.Data.SqlClient.SqlCommand
        'transazione che gestirà il rollback in caso di errore
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim strMeseAttuale As String
        Dim strAnnoAttuale As String
        Dim strGiornoAttuale As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        Dim item As DataGridItem

        Try

            myCommand = New System.Data.SqlClient.SqlCommand
            myCommand.Connection = Session("conn")

            'Se è stata definita la transazione
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            myCommand.Transaction = MyTransaction

            'assegno alla data grid il datatable

            For Each item In DtgMesiDaInserire.Items

                strMeseAttuale = item.Cells(3).Text
                strAnnoAttuale = item.Cells(1).Text
                strsql = "Update EntitàAssenze set stato=2, usernameConferma='" & Session("Utente") & "',dataConferma=getdate() "
                strsql = strsql & "FROM enti INNER JOIN "
                strsql = strsql & "attività ON enti.IDEnte = attività.IDEntePresentante INNER JOIN "
                strsql = strsql & "attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività INNER JOIN "
                strsql = strsql & "attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione INNER JOIN "
                strsql = strsql & "entità ON attivitàentità.IDEntità = entità.IDEntità INNER JOIN "
                strsql = strsql & "EntitàAssenze ON entità.IDEntità = EntitàAssenze.IDEntità "
                strsql = strsql & "INNER JOIN  TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
                strsql = strsql & "WHERE TipiProgetto.MacroTipoProgetto ='SCN'  and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'  AND (entitàassenze.anno='" & strAnnoAttuale & "') and (entitàassenze.Mese=" & CInt(strMeseAttuale) & ") and (entitàassenze.stato=1) and (enti.idente = '" & Session("IdEnte") & "')"

                myCommand.CommandText = strsql
                myCommand.ExecuteNonQuery()

                strsql = "insert into EntiConfermaAssenze (IdEnte,Anno,Mese,DataConferma,UserNameConferma) values (" & Session("IdEnte") & "," & CInt(strAnnoAttuale) & "," & CInt(strMeseAttuale) & ",getDate(),'" & Session("Utente") & "')"

                myCommand.CommandText = strsql
                myCommand.ExecuteNonQuery()

            Next
            MyTransaction.Commit()
            'vado a calcolarmi il totale delle assenze da confermare
            lblTotAssenze.Text = TotaleAssenze()
            If CheckMese() = False Then
                cmdConferma.Visible = False
                tblConferma.Visible = False
                lblMeseSuccessivo.Text = "Nessuno"
                lblTotAssenze.Text = "0"
                lblmessaggiosopra.Visible = True
                lblmessaggiosopra.Text = "Non ci sono assenze da confermare."
            Else
                tblConferma.Visible = True
                cmdConferma.Visible = True
            End If
            'carico comunque le assenze confermate
            CaricaAssenzeConfermate()

            lblmessaggiosopra.Visible = True
            lblmessaggiosopra.Text = "Assenze confermate con successo."
        Catch ex As Exception
            Response.Write(ex.Message.ToString)
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
        End Try
    End Sub

    Private Sub DtgMesiDaInserire_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DtgMesiDaInserire.ItemCommand

        Dim MeseSelezionato As Integer
        Dim AnnoSelezionato As Integer
        MeseSelezionato = CType(e.Item.Cells(3).Text, Integer)
        AnnoSelezionato = CType(e.Item.Cells(1).Text, Integer)

        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""WfrmVisualizzaMeseVol.aspx?MeseSel=" & MeseSelezionato & "&AnnoSel=" & AnnoSelezionato & """, ""Visualizza"", ""width=670,height=300,dependent=no,scrollbars=yes,status=no"")" & vbCrLf)
        Response.Write("</script>")

    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dtgRisultatoRicerca.DataSource = Session("LocalDataSet")
        dtgRisultatoRicerca.DataBind()
    End Sub

End Class