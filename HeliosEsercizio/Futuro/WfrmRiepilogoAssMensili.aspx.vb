Public Class WfrmRiepilogoAssMensili
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'assegno alle lebel i valori e carico la griglia
        lblValEnte.Text = Session("txtCodEnte")
        lblValProg.Text = Session("Denominazione")


        If Page.IsPostBack = False Then
            CaricaGriglietta()
        End If

    End Sub

    Sub CaricaGriglietta()
        'Antonello Di Croce funzione generata il 11/11/2005
        'Creo una data teable
        Dim dtGriglietta1 As New DataTable
        Dim drGriglietta1 As DataRow
        Dim strMeseAttuale As String
        Dim strAnnoAttuale As String
        Dim strDataConferma As String
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

        'controllo del mese anno
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

        'con questa query capisco che: se NON ci sono righe devo inserire tutti i mesi mancanti,se invece CI sono righe devo inserire in un intervallo di mese o solo il mese da confermare.
        strsql3 = "SELECT enti.CodiceRegione AS codReg, EntiConfermaAssenze.Anno, EntiConfermaAssenze.Mese, EntiConfermaAssenze.DataConferma"
        strsql3 = strsql3 & " FROM EntiConfermaAssenze INNER JOIN"
        strsql3 = strsql3 & " enti ON EntiConfermaAssenze.IdEnte = enti.IDEnte"
        strsql3 = strsql3 & " WHERE(enti.CodiceRegione = '" & Session("txtCodEnte") & "') order by EntiConfermaAssenze.anno asc,EntiConfermaAssenze.Mese asc"
        dtrLeggiDati2 = ClsServer.CreaDatareader(strsql3, Session("conn"))
        dtrLeggiDati2.Read()

        If dtrLeggiDati2.HasRows = True Then
            A = CInt(dtrLeggiDati2("Anno"))
            M = CInt(dtrLeggiDati2("Mese"))
            '+ 1

            If M = 12 Then
                M = M + 1
            End If

            If M = 13 Then
                M = 1
                A = A + 1
            End If
            If Not dtrLeggiDati2 Is Nothing Then
                dtrLeggiDati2.Close()
                dtrLeggiDati2 = Nothing
            End If
            'Mauro Lanna 17/11/2008
            'creava problemi di visualizzazione es nz00931-nz00934 in quanto il mese iniziale era ottobre e veniva sbattuto fuori
            'If strMeseAttuale = M Then
            'Exit Sub
            'End If

        Else
            If Not dtrLeggiDati2 Is Nothing Then
                dtrLeggiDati2.Close()
                dtrLeggiDati2 = Nothing
            End If
            'SE NON CI SONO ASSENZE MAI CONFERMATE ALLORA.......eseguo questa query
            strsql4 = "SELECT  min(entità.DataInizioServizio) AS DataI"
            strsql4 = strsql4 & " FROM entità "
            strsql4 = strsql4 & " INNER Join attivitàentità ON entità.IDEntità = attivitàentità.IDEntità  "
            strsql4 = strsql4 & " INNER Join attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione  "
            strsql4 = strsql4 & " INNER join attività on attivitàentisediattuazione.idattività = attività.idattività "
            strsql4 = strsql4 & " INNER Join enti ON attività.IDEntepresentante = enti.IDEnte "
            strsql4 = strsql4 & " INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto  "
            strsql4 = strsql4 & " WHERE enti.CodiceRegione = '" & Session("txtCodEnte") & "' and entità.datainizioservizio <> entità.datafineservizio"
            strsql4 = strsql4 & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' and TipiProgetto.MacroTipoProgetto ='SCN' "
            dtrLeggiDati6 = ClsServer.CreaDatareader(strsql4, Session("conn"))
            dtrLeggiDati6.Read()
            If IsDBNull(dtrLeggiDati6("DataI")) Then
                DataInizio = ""
                If Not dtrLeggiDati6 Is Nothing Then
                    dtrLeggiDati6.Close()
                    dtrLeggiDati6 = Nothing
                End If
                lblNoVol.Visible = True
                Exit Sub
            Else
                DataInizio = dtrLeggiDati6("DataI")
            End If


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
        'Creo intestazione data teable
        dtGriglietta1.Columns.Add()
        dtGriglietta1.Columns.Add(New DataColumn("Mese1", GetType(String)))
        dtGriglietta1.Columns.Add(New DataColumn("Anno1", GetType(String)))
        dtGriglietta1.Columns.Add(New DataColumn("Stato", GetType(String)))
        dtGriglietta1.Columns.Add(New DataColumn("DataConferma", GetType(String)))
        dtGriglietta1.Columns.Add(New DataColumn("MeseNum", GetType(String)))

        Dim ControlloEsistenzaEntità As Boolean = True
        Do While ControlloEsistenzaEntità = True
            strMeseAttuale = IIf(Len(CStr(strMeseAttuale)) < 2, "0" & (CStr(strMeseAttuale)), (CStr(strMeseAttuale)))
            miadata = strAnnoAttuale & "-" & strMeseAttuale

            '-----------------------------------------------------------------------------------
            ''strsql2 = "SELECT "
            ''strsql2 = strsql2 & " entità.DataFineServizio AS DataF, "
            ''strsql2 = strsql2 & " entità.DataInizioServizio AS DataI, "
            ''strsql2 = strsql2 & " entità.IDEntità AS IdEntità, "
            ''strsql2 = strsql2 & " enti.CodiceRegione AS codReg "
            ''strsql2 = strsql2 & " FROM entità"
            ''strsql2 = strsql2 & " INNER Join attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
            ''strsql2 = strsql2 & " INNER Join attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
            ''strsql2 = strsql2 & " inner join attività on attivitàentisediattuazione.idattività = attività.idattività"
            ''strsql2 = strsql2 & " INNER Join enti ON attività.IDEntepresentante = enti.IDEnte"
            ''strsql2 = strsql2 & " WHERE (enti.CodiceRegione = '" & Session("txtCodEnte") & "')  "
            ''strsql2 = strsql2 & " AND ( '" & miadata & "'"
            ''strsql2 = strsql2 & " BETWEEN"
            ''strsql2 = strsql2 & " convert(varchar,year(entità.DataInizioServizio)) + '-' + REPLICATE('0',2-LEN(convert(varchar,month(entità.DataInizioServizio)))) + convert(varchar,month(entità.DataInizioServizio)) "
            ''strsql2 = strsql2 & " AND "
            ''strsql2 = strsql2 & " convert(varchar,year(entità.DataFineServizio)) + '-' + REPLICATE('0',2-LEN(convert(varchar,month(entità.DataFineServizio)))) + convert(varchar,month(entità.DatafINEServizio)))"
            '..........................................................................

            strsql2 = "SELECT entità.DataFineServizio AS DataF, entità.DataInizioServizio AS DataI, entità.IDEntità AS IdEntità, enti.CodiceRegione AS codReg,EntiConfermaAssenze.DataConferma, "
            strsql2 = strsql2 & " CASE WHEN CONVERT(varchar, EntiConfermaAssenze.IdEnteConfermaAssenze) IS NULL THEN 'Da Confermare' ELSE 'Confermato' END Stato"
            strsql2 = strsql2 & " FROM  entità INNER JOIN"
            strsql2 = strsql2 & " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN"
            strsql2 = strsql2 & " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN"
            strsql2 = strsql2 & " attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività INNER JOIN"
            strsql2 = strsql2 & " enti ON attività.IDEntePresentante = enti.IDEnte  INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto   LEFT OUTER JOIN"
            strsql2 = strsql2 & " EntiConfermaAssenze ON enti.IDEnte = EntiConfermaAssenze.IdEnte AND '" & miadata & "' = CONVERT(varchar, EntiConfermaAssenze.Anno) "
            strsql2 = strsql2 & " + '-' + REPLICATE('0', 2 - LEN(CONVERT(varchar, EntiConfermaAssenze.Mese))) + CONVERT(varchar, EntiConfermaAssenze.Mese)"
            strsql2 = strsql2 & " WHERE  TipiProgetto.MacroTipoProgetto ='SCN' and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' AND (enti.CodiceRegione = '" & Session("txtCodEnte") & "') AND ('" & miadata & "' BETWEEN CONVERT(varchar, YEAR(entità.DataInizioServizio)) + '-' + REPLICATE('0', "
            strsql2 = strsql2 & " 2 - LEN(CONVERT(varchar, MONTH(entità.DataInizioServizio)))) + CONVERT(varchar, MONTH(entità.DataInizioServizio)) AND CONVERT(varchar, "
            strsql2 = strsql2 & " YEAR(entità.DataFineServizio)) + '-' + REPLICATE('0', 2 - LEN(CONVERT(varchar, MONTH(entità.DataFineServizio)))) + CONVERT(varchar, "
            strsql2 = strsql2 & " MONTH(entità.DataFineServizio)))"



            '.......................................................................................
            If Not dtrLeggiDati1 Is Nothing Then
                dtrLeggiDati1.Close()
                dtrLeggiDati1 = Nothing
            End If

            dtrLeggiDati1 = ClsServer.CreaDatareader(strsql2, Session("conn"))
            dtrLeggiDati1.Read()
            'assegno il campo stato ad una variabile
            Dim stato As String
            Dim ControlloRighe As Integer
            If dtrLeggiDati1.HasRows = True Then
                If IsDBNull(dtrLeggiDati1("DataConferma")) Then
                    strDataConferma = ""
                Else
                    strDataConferma = dtrLeggiDati1("DataConferma")
                End If

                stato = dtrLeggiDati1("stato")
                ControlloRighe = 1
            Else
                ControlloRighe = 0
            End If
            If Not dtrLeggiDati1 Is Nothing Then
                dtrLeggiDati1.Close()
                dtrLeggiDati1 = Nothing
            End If
            If ControlloRighe = 1 Then
                If (CInt(strAnnoAttuale) = A) And (CInt(strMeseAttuale) < M) Then
                    ControlloEsistenzaEntità = False
                End If

                If (CInt(strAnnoAttuale) = A1) And (CInt(strMeseAttuale) < M1) Then
                    ControlloEsistenzaEntità = False
                End If

                'carico valori nel data rider 
                drGriglietta1 = dtGriglietta1.NewRow()
                drGriglietta1(1) = strMeseAttuale
                drGriglietta1(2) = strAnnoAttuale
                drGriglietta1(3) = stato
                drGriglietta1(4) = strDataConferma
                drGriglietta1(5) = strMeseAttuale
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

            'Controllo mese, anno
            If strMeseAttuale = "01" Then
                strAnnoAttuale = strAnnoAttuale - 1
                strMeseAttuale = "12"
            Else
                strAnnoAttuale = strAnnoAttuale
                strMeseAttuale = strMeseAttuale - 1
            End If
        Loop

        'creo una data table e gli passo un altra data table
        Dim dtGriglietta2 As New DataTable
        dtGriglietta2 = dtGriglietta1
        Dim x As Integer
        'ciclo la data table 
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
        'assegno alla data grid il datatable
        'DtgMesiDaInserire.DataSource = dtGriglietta1
        DtgMesiDaInserire.DataSource = dtGriglietta2
        DtgMesiDaInserire.DataBind()
        DtgMesiDaInserire.SelectedIndex = -1


        'FINE FASE NUOVA

        '-------------------------------------------------------------------------------------------


    End Sub

    Private Sub DtgMesiDaInserire_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DtgMesiDaInserire.ItemCommand

        If e.CommandName = "Select" Then
            Dim MeseSelezionato As Integer
            Dim AnnoSelezionato As Integer
            MeseSelezionato = CType(e.Item.Cells(5).Text, Integer)
            AnnoSelezionato = CType(e.Item.Cells(1).Text, Integer)

            Response.Write("<script>" & vbCrLf)
            Response.Write("window.open(""WfrmVisualizzaMeseVol.aspx?MeseSel=" & MeseSelezionato & "&AnnoSel=" & AnnoSelezionato & """, ""Visualizza"", ""width=900,height=400,dependent=no,scrollbars=yes,status=no"")" & vbCrLf)
            Response.Write("</script>")

        End If
       
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click

        Response.Redirect("WfrmMain.aspx")

    End Sub

End Class