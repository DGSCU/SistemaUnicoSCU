Imports System.Collections
Imports System.IO
Public Class WfrmRisultatoImportGraduatoriaVolontari
    Inherits System.Web.UI.Page
    Dim MyCommand As SqlClient.SqlCommand
    Dim DefArr As String()
    Dim ArrAggiorna As String()
    'Dim ArrTel As String()
    Dim ArrDatePreviste As String()
    Dim dtsLocal As DataSet



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then

            If Request.QueryString("MessGrad") <> "" Then

                Dim MESSAGGIODIRITORNOGRADUATORIEARREY() As String
                MESSAGGIODIRITORNOGRADUATORIEARREY = Request.QueryString("MessGrad").Split(";")

                Dim MessaggioFinale As String = ""
                For i = 0 To UBound(MESSAGGIODIRITORNOGRADUATORIEARREY) - 1
                    MessaggioFinale &= MESSAGGIODIRITORNOGRADUATORIEARREY(i) & "<br />"
                Next

                'CmdConferma.Visible = False
                'lblTotali.Visible = False
                'hlDownLoad.Visible = False
                AvvisoConferma.Visible = True
                avviso.Visible = True
                testoavviso.InnerHtml = "LA VERIFICA DEI DATI IMMESSI NEL FILE CSV NON RISULTA CORRETTA. IL NUMERO DEI VOLONTARI INSERITI E' DIVERSO DA QUELLI IN GRADUATORIA.VERIFICARE I PROGETTI E LE SEDI INDICATE (Progetto_Sede):" & "<br /> " & MessaggioFinale

                'Exit Sub
            End If

            CaricaGriglia()

            lblTotali.Text = "Sono state inviate " & Request.QueryString("Tot") & " righe. " & _
                             Request.QueryString("TotOk") & " con esito positivo. " & Request.QueryString("TotKo") & " con esito negativo."

            hlDownLoad.NavigateUrl = "download\" & Request.QueryString("NomeFile") & ".CSV"

            If CInt(Request.QueryString("TotKo")) > 0 Then
                CmdConferma.Visible = False
            Else
                AvvisoConferma.Visible = True
                avviso.Visible = True
                testoavviso.InnerHtml = "LA VERIFICA DEI DATI IMMESSI NEL FILE CSV RISULTA CORRETTA. PER SALVARE DEFINITIVAMENTE I DATI PREMERE IL TASTO CONFERMA."
            End If
        End If
    End Sub

    Private Sub CaricaGriglia()
        Dim dtCSV As DataTable = New DataTable
        Dim rwCSV As DataRow
        Dim clCSV As DataColumn

        Dim strSql As String
        Dim i As Integer

        Dim TmpArr() As String

        Dim Reader As StreamReader
        Dim xLinea As String


        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Note"
        clCSV.Caption = "Note"
        clCSV.ReadOnly = False
        clCSV.Unique = False

        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Cognome"
        clCSV.Caption = "Cognome"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Nome"
        clCSV.Caption = "Nome"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "CodiceFiscale"
        clCSV.Caption = "Codice Fiscale"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)


        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "EsitoSelezione"
        clCSV.Caption = "Esito Selezione"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)


        Reader = New StreamReader(Server.MapPath("download") & "\" & Request.QueryString("NomeFile") & ".CSV")

        xLinea = Reader.ReadLine()
        xLinea = Reader.ReadLine()


        While (xLinea <> "")

            DefArr = CreaArray(xLinea)
            rwCSV = dtCSV.NewRow

            rwCSV(0) = DefArr(0)

            If UBound(DefArr) = 0 Then
                rwCSV(1) = vbNullString
                rwCSV(2) = vbNullString
                rwCSV(3) = vbNullString
                rwCSV(4) = vbNullString
            Else
                rwCSV(1) = DefArr(1)
                If UBound(DefArr) = 1 Then
                    rwCSV(2) = vbNullString
                Else
                    rwCSV(2) = DefArr(2)
                    If UBound(DefArr) = 2 Then
                        rwCSV(3) = vbNullString
                    Else
                        rwCSV(3) = DefArr(3)
                        If UBound(DefArr) = 3 Then
                            rwCSV(4) = vbNullString
                        Else
                            rwCSV(4) = DefArr(6)

                        End If
                    End If
                End If

              
            End If




            'AGGIUGERE IN GRIGLIA CODICEESITO
            'vado a leggere 

            dtCSV.Rows.Add(rwCSV)
            xLinea = Reader.ReadLine()
        End While

        dtgCSV.DataSource = dtCSV
        dtgCSV.DataBind()


    End Sub

    'OK 1
    Private Function CreaArray(ByVal pLinea As String) As String()
        Dim TmpArr As String()

        Dim i As Integer
        Dim x As Integer
        Dim strValore As String

        TmpArr = Split(pLinea, ";")

        For i = 0 To UBound(TmpArr)
            If i = 0 Then
                ReDim DefArr(0)
            Else
                ReDim Preserve DefArr(UBound(DefArr) + 1)
            End If
            If Left(TmpArr(i), 1) = Chr(34) Then
                x = i
                Do While Right(TmpArr(x), 1) <> Chr(34)
                    If x = i Then
                        DefArr(UBound(DefArr)) = Mid(TmpArr(x), 2) & "; "
                    Else
                        DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & TmpArr(x) & "; "
                    End If
                    x = x + 1
                Loop
                i = x
            Else
                DefArr(UBound(DefArr)) = TmpArr(i)

            End If
            'strValore = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
            'DefArr(UBound(DefArr)) = strValore

            'If Left(TmpArr(i), 1) = Chr(34) Then
            '    x = i
            '    Do While Right(TmpArr(x), 1) <> Chr(34)
            '        If x = i Then
            '            DefArr(UBound(DefArr)) = Mid(TmpArr(x), 2) & "; "
            '        Else
            '            DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & TmpArr(x) & "; "
            '        End If
            '        x = x + 1
            '    Loop
            '    '***************************************DOMICILIO******************************************
            '    'vado a controllare l'indirizzo di domicilio da stampare nella datagrid
            '    'modifica aggiunta da jons friztgerald kennedy il 30 marzo 2009
            '    If InStr(DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1), "L'indirizzo del domicilio non e' valido.") > 0 Then
            '        strValore = Replace(DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1), "L'indirizzo del domicilio non e' valido.", Session("ArraySegnalazioneIndirizzoDomicilio")(0))
            '        Dim intX As Integer
            '        'Move all the items down
            '        If UBound(Session("ArraySegnalazioneIndirizzoDomicilio")) = 0 Then
            '            Session("ArraySegnalazioneIndirizzoDomicilio") = Nothing
            '        Else
            '            For intX = 0 To UBound(Session("ArraySegnalazioneIndirizzoDomicilio")) - 1
            '                Session("ArraySegnalazioneIndirizzoDomicilio")(intX) = Session("ArraySegnalazioneIndirizzoDomicilio")(intX + 1)
            '            Next
            '            'Redimension the array
            '            ReDim Preserve Session("ArraySegnalazioneIndirizzoDomicilio")(UBound(Session("ArraySegnalazioneIndirizzoDomicilio")) - 1)
            '        End If
            '    Else
            '        strValore = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
            '    End If
            '    '***************************************RESIDENZA******************************************
            '    'vado a controllare l'indirizzo di residenza da stampare nella datagrid
            '    'modifica aggiunta da jons friztgerald kennedy il 30 marzo 2009
            '    If InStr(strValore, "L'indirizzo di residenza non e' valido.") > 0 Then
            '        strValore = Replace(strValore, "L'indirizzo di residenza non e' valido.", Session("ArraySegnalazioneIndirizzoResidenza")(0))
            '        Dim intX As Integer
            '        'Move all the items down
            '        If UBound(Session("ArraySegnalazioneIndirizzoResidenza")) = 0 Then
            '            Session("ArraySegnalazioneIndirizzoResidenza") = Nothing
            '        Else
            '            For intX = 0 To UBound(Session("ArraySegnalazioneIndirizzoResidenza")) - 1
            '                Session("ArraySegnalazioneIndirizzoResidenza")(intX) = Session("ArraySegnalazioneIndirizzoResidenza")(intX + 1)
            '            Next
            '            'Redimension the array
            '            ReDim Preserve Session("ArraySegnalazioneIndirizzoResidenza")(UBound(Session("ArraySegnalazioneIndirizzoResidenza")) - 1)
            '        End If
            '    End If
            '    DefArr(UBound(DefArr)) = strValore
            '    i = x
            'Else
            '    '***************************************DOMICILIO******************************************
            '    If InStr(TmpArr(i), "L'indirizzo del domicilio non e' valido.") > 0 Then
            '        strValore = Replace(TmpArr(i), "L'indirizzo del domicilio non e' valido.", Session("ArraySegnalazioneIndirizzoDomicilio")(0))
            '        Dim intX As Integer
            '        'Move all the items down
            '        If UBound(Session("ArraySegnalazioneIndirizzoDomicilio")) = 0 Then
            '            Session("ArraySegnalazioneIndirizzoDomicilio") = Nothing
            '        Else
            '            For intX = 0 To UBound(Session("ArraySegnalazioneIndirizzoDomicilio")) - 1
            '                Session("ArraySegnalazioneIndirizzoDomicilio")(intX) = Session("ArraySegnalazioneIndirizzoDomicilio")(intX + 1)
            '            Next
            '            'Redimension the array
            '            ReDim Preserve Session("ArraySegnalazioneIndirizzoDomicilio")(UBound(Session("ArraySegnalazioneIndirizzoDomicilio")) - 1)
            '        End If
            '    Else
            '        strValore = TmpArr(i)
            '    End If
            '    '***************************************RESIDENZA******************************************
            '    If InStr(strValore, "L'indirizzo di residenza non e' valido.") > 0 Then
            '        strValore = Replace(strValore, "L'indirizzo di residenza non e' valido.", Session("ArraySegnalazioneIndirizzoResidenza")(0))
            '        Dim intX As Integer
            '        'Move all the items down
            '        If UBound(Session("ArraySegnalazioneIndirizzoResidenza")) = 0 Then
            '            Session("ArraySegnalazioneIndirizzoResidenza") = Nothing
            '        Else
            '            For intX = 0 To UBound(Session("ArraySegnalazioneIndirizzoResidenza")) - 1
            '                Session("ArraySegnalazioneIndirizzoResidenza")(intX) = Session("ArraySegnalazioneIndirizzoResidenza")(intX + 1)
            '            Next
            '            'Redimension the array
            '            ReDim Preserve Session("ArraySegnalazioneIndirizzoResidenza")(UBound(Session("ArraySegnalazioneIndirizzoResidenza")) - 1)
            '        End If
            '    End If
            '    DefArr(UBound(DefArr)) = strValore
            'End If

        Next

        CreaArray = DefArr

    End Function

    'OK 1
    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand

        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_GRADUATORIA_VOLONTARI]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub

    Private Sub CancellaTabellaTempGG()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand

        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_GRADUATORIA_VOLONTARIGG]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub

    'OK 1
    Private Sub ScriviEntitaGG()

        Dim strsql As String


        strsql = "INSERT INTO Entità " & _
                 "(Cognome, " & _
                 "Nome, " & _
                 "CodiceFiscale, " & _
                 "DataNascita, " & _
                 "Sesso, " & _
                 "IdComuneNascita, " & _
                 "IdComuneResidenza, " & _
                 "Indirizzo, " & _
                 "NumeroCivico, " & _
                 "Cap, " & _
                 "StatoCivile, " & _
                 "UserNameStato, " & _
                 "DataUltimoStato, " & _
                 "IdStatoEntità, " & _
                 "TmpCodiceProgetto, " & _
                 "TmpIdSedeAttuazione, " & _
                 "DisponibileStessoProg, " & _
                 "DisponibileAltriProg," & _
                 "Telefono,TitoloStudio,IdSedePrimaAssegnazione, " & _
                 "Email,IDComuneDomicilio,IndirizzoDomicilio, " & _
                 "NumeroCivicoDomicilio,CapDomicilio,DettaglioRecapitoResidenza,DettaglioRecapitoDomicilio,FlagIndirizzoValidoRes,FlagIndirizzoValidoDom,IdCategoriaEntità" & _
                 ",IdTitoloStudioConseguimento," & _
                 "AnomaliaCF, IDStatiVerificaCFEntità," & _
                 "IdNazionalita,DataDomanda,IdTipoStatoCivile,CodiceFiscaleConiuge) " & _
                 "SELECT " & _
                 "upper(a.Cognome), " & _
                 "upper(a.Nome), " & _
                 "upper(a.CodiceFiscale), " & _
                 "a.DataNascita, " & _
                 "a.Sesso, " & _
                 "b.IdComune, " & _
                 "c.IdComune, " & _
                 "a.Indirizzo, " & _
                 "a.NumeroCivico, " & _
                 "a.Cap, " & _
                 "'Stato Libero', " & _
                 "'" & Session("Utente") & "', " & _
                 "'" & Day(Now) & "/" & Month(Now) & "/" & Year(Now) & "', " & _
                 "1, " & _
                 "a.CodiceProgetto, " & _
                 "a.CodiceSede, " & _
                 "a.SubentroStessoProgetto, " & _
                 "a.SubentroAltriProgetti, " & _
                 "a.Telefono, a.TitoloStudio, a.CodiceSedePrimoGiorno, " & _
                 "a.Email," & _
                 "d.IDComune ," & _
                 "a.IndirizzoDomicilio, " & _
                 "a.NumeroCivicoDomicilio, " & _
                 "a.CapDomicilio,a.DettaglioRecapitoRes,a.DettaglioRecapitoDom,a.FlagIndirizzoValidoRes,a.FlagIndirizzoValidoDom, e.IdCategoriaEntità, " & _
                 "f.IdTitoloStudioConseguimento, " & _
                 "a.AnomaliaCF, a.IDStatiVerificaCFEntità, " & _
                 "isnull(cit.Idcomune,38715), a.Datadomanda,g.IdTipoStatoCivile,a.CodiceFiscaleConiuge " & _
                 "FROM #TEMP_GRADUATORIA_VOLONTARIGG a " & _
                 "INNER JOIN Comuni b ON a.CodiceISTATComuneNascita = b.CodiceISTAT " & _
                 "INNER JOIN Comuni c ON a.CodiceISTATComuneResidenza = c.CodiceISTAT " & _
                 "Left JOIN Comuni cit ON case isnull(a.Nazionalita,'') when '' then 'nonindicato' else a.Nazionalita end = cit.CodiceISTAT " & _
                 "LEFT JOIN CategorieEntità e ON a.Categoria = e.Codice " & _
                 "INNER JOIN TitoliStudioConseguimento f ON a.ConseguimentoTitoloStudio = f.Codice " & _
                 "INNER JOIN TipiStatoCivile g ON a.CodiceStatoCivile = g.Codice " & _
                 "LEFT JOIN Comuni  d ON CASE a.CodiceISTATComuneDomicilio WHEN '' THEN '-1' ELSE a.CodiceISTATComuneDomicilio END = d.CodiceISTAT  "

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

    End Sub

    'OK 1
    Private Sub ScriviGraduatorieGG()
        Dim cmdGraduatorie As SqlClient.SqlCommand
        Dim strsql As String

        strsql = "INSERT INTO GraduatorieEntità " & _
                 "(IdEntità, " & _
                 "IdAttivitàSedeAssegnazione, " & _
                 "Stato, " & _
                 "Ammesso, " & _
                 "Punteggio, " & _
                 "IdTipologiaPosto, " & _
                 "UserName, " & _
                 "DataModifica, " & _
                 "Ordine) " & _
                 "SELECT " & _
                 "b.IdEntità, " & _
                 "g.IdAttivitàSedeAssegnazione, " & _
                 "a.Idoneo, " & _
                 "a.Selezionato, " & _
                 "a.Punteggio, " & _
                 "a.TipoPosto, " & _
                 "'" & Session("Utente") & "', " & _
                 "GETDATE(), " & _
                 "'0' " & _
                 "FROM #TEMP_GRADUATORIA_VOLONTARIGG a " & _
                 "INNER JOIN Attività d ON a.CodiceProgetto = d.CodiceEnte " & _
                 "INNER JOIN EntiSediAttuazioni e ON e.IdEntesedeAttuazione = a.CodiceSede " & _
                 "INNER JOIN Entità b ON a.CodiceFiscale = b.CodiceFiscale and b.tmpcodiceprogetto = a.codiceprogetto and b.tmpidsedeattuazione = a.codicesede " & _
                 "INNER JOIN EntiSedi f ON f.IdEnteSede = e.IdenteSede " & _
                 "INNER JOIN AttivitàSediAssegnazione g ON g.IdEnteSede = f.IdEnteSede and g.idattività = d.idattività "

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

    End Sub

    'OK 1
    Private Function EseguiOrdinaGraduatoria() As Boolean
        Dim strsql As String
        Dim dtrId As SqlClient.SqlDataReader
        Dim ArrId() As Integer
        Dim i As Integer


        Try
            EseguiOrdinaGraduatoria = True
            For i = LBound(ArrAggiorna) To UBound(ArrAggiorna)
                ClsServer.EseguiStoreOrdina(ArrAggiorna(i), "SP_ORDINA_GRADUATORIA", Session("conn"))
            Next
        Catch
            If Not dtrId Is Nothing Then
                dtrId.Close()
                dtrId = Nothing
            End If
            EseguiOrdinaGraduatoria = False

        End Try

    End Function

    'OK 1
    Private Sub AggiornaStatoGraduatoria()
        Dim strSql As String
        Dim i As Integer
        Dim ArrId(1) As String

        For i = 0 To UBound(ArrAggiorna)
            ArrId = Split(ArrAggiorna(i), ",")

            strSql = "UPDATE AttivitàSediAssegnazione SET " & _
            "StatoGraduatoria = 2 " & _
            "WHERE " & _
            "IdAttivitàSedeAssegnazione = " & ArrId(0)

            MyCommand.CommandText = strSql
            MyCommand.ExecuteNonQuery()
        Next i

    End Sub

    'OK 1
    Private Function RecuperaDatiPerGraduatoriaGG() As Boolean
        Dim dtrAggiorna As SqlClient.SqlDataReader
        Dim strSql As String
        Dim i As Integer

        Try
            strSql = "SELECT " & _
                     "DISTINCT e.IdAttivitàSedeAssegnazione " & _
                     "FROM #TEMP_GRADUATORIA_VOLONTARIGG a " & _
                     "INNER JOIN Attività b ON a.CodiceProgetto = b.CodiceEnte " & _
                     "INNER JOIN EntiSediAttuazioni c ON a.CodiceSede = c.IdEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi d ON c.IdEnteSede = d.IdEnteSede " & _
                     "INNER JOIN AttivitàSediAssegnazione e ON d.IdEnteSede = e.IdEnteSede AND b.IdAttività = e.IdAttività"


            dtrAggiorna = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrAggiorna.HasRows = True Then
                i = 0
                While dtrAggiorna.Read

                    If i = 0 Then
                        ReDim ArrAggiorna(0)
                    Else
                        ReDim Preserve ArrAggiorna(UBound(ArrAggiorna) + 1)
                    End If
                    ArrAggiorna(i) = dtrAggiorna(0)
                    i = i + 1
                End While
            End If
            dtrAggiorna.Close()
            dtrAggiorna = Nothing
            RecuperaDatiPerGraduatoriaGG = True
        Catch
            If Not dtrAggiorna Is Nothing Then
                dtrAggiorna.Close()
                dtrAggiorna = Nothing
            End If
            RecuperaDatiPerGraduatoriaGG = False
        End Try

    End Function

    'OK 1
    Private Function RecuperaDatePrevisteGG() As Boolean
        Dim dtrDataPrevista As SqlClient.SqlDataReader
        Dim strSql As String
        Dim i As Integer
        Dim xId As Integer

        Try
            strSql = "SELECT " & _
                     "b.IdAttività, " & _
                     "a.DataInizioPrevista, " & _
                     "DateAdd(day, -1, DateAdd(month, b.NMesi, a.DataInizioPrevista)) as DataFinePrevista " & _
                     "FROM #TEMP_GRADUATORIA_VOLONTARIGG a " & _
                     "INNER JOIN Attività b ON a.CodiceProgetto = b.CodiceEnte "

            dtrDataPrevista = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrDataPrevista.HasRows = True Then
                i = -1
                xId = 0
                While dtrDataPrevista.Read
                    If xId <> dtrDataPrevista(0) Then
                        xId = dtrDataPrevista(0)
                        i = i + 1
                        If i = 0 Then
                            ReDim ArrDatePreviste(0)
                        Else
                            ReDim Preserve ArrDatePreviste(UBound(ArrDatePreviste) + 1)
                        End If
                    End If
                    ArrDatePreviste(i) = CStr(dtrDataPrevista(0)) & "," & CStr(dtrDataPrevista(1)) & "," & CStr(dtrDataPrevista(2))
                End While
            End If
            dtrDataPrevista.Close()
            dtrDataPrevista = Nothing
            RecuperaDatePrevisteGG = True
        Catch ex As Exception
            If Not dtrDataPrevista Is Nothing Then
                dtrDataPrevista.Close()
                dtrDataPrevista = Nothing
            End If
            RecuperaDatePrevisteGG = False
        End Try
    End Function

    'OK 1
    Private Sub AggiornaDatePreviste()
        Dim strSql As String
        Dim i As Integer
        Dim RigaDate(2) As String

        For i = 0 To UBound(ArrDatePreviste)
            RigaDate = Split(ArrDatePreviste(i), ",")

            strSql = "UPDATE Attività SET " & _
                     "DataInizioPrevista = '" & RigaDate(1) & "', " & _
                     "DataFinePrevista = '" & RigaDate(2) & "' " & _
                     "WHERE " & _
                     "IdAttività = " & RigaDate(0)

            MyCommand.CommandText = strSql
            MyCommand.ExecuteNonQuery()
        Next i
    End Sub


    Private Sub AggiornaTabellaTemporaneaTelefonoGG()
        'Antonello Di Croce 20/02/2008
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        'Dim i As Integer
        Dim Telefoni As String
        Dim Prefissi As String
        Dim zero As String
        Dim codfisc As String
        Dim LetturaTelefonoOld As String
        zero = "0"

        strSql = "Select Telefono,codicefiscale FROM #TEMP_GRADUATORIA_VOLONTARIGG"
        dtsLocal = ClsServer.DataSetGenerico(strSql, Session("conn"))
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(1) As String
        Dim NomiCampiColonne(1) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Telefono"
        NomeColonne(1) = "Codice Fiscale"


        NomiCampiColonne(0) = "Telefono"
        NomiCampiColonne(1) = "Codicefiscale"


        'carico i nomi delle colonne 
        For x = 0 To 1
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query 
        If dtsLocal.Tables(0).Rows.Count > 0 Then
            For i = 1 To dtsLocal.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 1
                    dr(x) = dtsLocal.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        Dim y As Integer
        For y = 0 To dt.Rows.Count - 1
            LetturaTelefonoOld = dt.Rows(y)(0)
            Telefoni = dt.Rows(y)(0)
            codfisc = dt.Rows(y)(1)
            Prefissi = Mid(Telefoni, 1, 3)
            Prefissi = Telefoni.Substring(0, 3)
            Select Case Prefissi
                'INIZIO TIM  -------------------------------------------
                Case "330"
                    Telefoni = dt.Rows(y)(0)
                Case "333"
                    Telefoni = dt.Rows(y)(0)
                Case "334"
                    Telefoni = dt.Rows(y)(0)
                Case "335"
                    Telefoni = dt.Rows(y)(0)
                Case "336"
                    Telefoni = dt.Rows(y)(0)
                Case "337"
                    Telefoni = dt.Rows(y)(0)
                Case "338"
                    Telefoni = dt.Rows(y)(0)
                Case "339"
                    Telefoni = dt.Rows(y)(0)
                Case "360"
                    Telefoni = dt.Rows(y)(0)
                    'Case "363"
                    'Telefoni = dt.Rows(y)(0)
                Case "366"
                    Telefoni = dt.Rows(y)(0)
                Case "368"
                    Telefoni = dt.Rows(y)(0)
                    'FINE TIM  -------------------------------------------

                    'INIZIO  VODAFONE  -------------------------------------------
                Case "340"
                    Telefoni = dt.Rows(y)(0)
                    'Case "343"
                    'Telefoni = dt.Rows(y)(0)
                Case "347"
                    Telefoni = dt.Rows(y)(0)
                    'Case "346"
                    'Telefoni = dt.Rows(y)(0)
                Case "348"
                    Telefoni = dt.Rows(y)(0)
                Case "349"
                    Telefoni = dt.Rows(y)(0)
                    'FINE VODAFONE  -------------------------------------------

                    'INIZIO H3G  -------------------------------------------
                    'Case "390"
                    'Telefoni = dt.Rows(y)(0)
                    'Case "391"
                    'Telefoni = dt.Rows(y)(0)
                    'Case "392"
                    'Telefoni = dt.Rows(y)(0)
                    'Case "393"
                    'Telefoni = dt.Rows(y)(0)
                    'FINE H3G  -------------------------------------------

                    'INIZIO WIND  -------------------------------------------
                Case "320"
                    Telefoni = dt.Rows(y)(0)
                    'Case "323"
                    'Telefoni = dt.Rows(y)(0)
                Case "327"
                    Telefoni = dt.Rows(y)(0)
                Case "328"
                    Telefoni = dt.Rows(y)(0)
                Case "329"
                    Telefoni = dt.Rows(y)(0)
                Case "380"
                    Telefoni = dt.Rows(y)(0)
                Case "383"
                    Telefoni = dt.Rows(y)(0)
                    'Case "388"
                    'Telefoni = dt.Rows(y)(0)
                Case "389"
                    Telefoni = dt.Rows(y)(0)
                    'FINE WIND  -------------------------------------------
                Case Else
                    Telefoni = zero & dt.Rows(y)(0)
            End Select


            strSql = "UPDATE #TEMP_GRADUATORIA_VOLONTARIGG SET Telefono ='" & Telefoni & "' where Telefono = '" & LetturaTelefonoOld & "' and  codicefiscale ='" & codfisc & "'"
            MyCommand.CommandText = strSql
            MyCommand.ExecuteNonQuery()

        Next
    End Sub

    Private Sub AggiornaRecuperoPostiGG()

        'recupero graduatorie relative a recupero posti
        Dim strSql As String
        Dim i As Integer
        Dim ArrId(1) As String
        Dim intAmmessoRecupero As Integer

        For i = 0 To UBound(ArrAggiorna)
            ArrId = Split(ArrAggiorna(i), ",")

            strSql = "SELECT isnull(AmmessoRecupero,0) as AmmessoRecupero from AttivitàSediAssegnazione WHERE " & _
            "IdAttivitàSedeAssegnazione = " & ArrId(0)


            MyCommand.CommandText = strSql
            intAmmessoRecupero = MyCommand.ExecuteScalar()

            If intAmmessoRecupero = 1 Then
                'aggiorno volontari relativi a graduatorie da recupero posti
                strSql = "update entità set ammessorecupero = 1 " & _
                "from entità a inner join graduatorieentità b on a.identità = b.identità " & _
                "inner join #TEMP_GRADUATORIA_VOLONTARIGG c ON c.CodiceFiscale = a.CodiceFiscale and c.codiceprogetto = a.tmpcodiceprogetto and c.codicesede = a.tmpidsedeattuazione " & _
                "where b.IdAttivitàSedeAssegnazione = " & ArrId(0)

                MyCommand.CommandText = strSql
                MyCommand.ExecuteNonQuery()

                'aggiorno graduatorie relative a recupero posti
                strSql = "update AttivitàSediAssegnazione set ammessorecupero = 2, statograduatoria = 3 where IdAttivitàSedeAssegnazione = " & ArrId(0)

                MyCommand.CommandText = strSql
                MyCommand.ExecuteNonQuery()
            End If

        Next i

    End Sub

    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        MyCommand = New SqlClient.SqlCommand
        MyCommand.Connection = Session("conn")
        CmdConferma.Visible = False

        Try
            'If Session("Sistema") = "Helios" Then
            'AggiornaTabellaTemporaneaTelefono()
            RecuperaDatiPerGraduatoria()
            RecuperaDatePreviste()

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            ScriviEntita()
            ScriviGraduatorie()
            AggiornaStatoGraduatoria()
            AggiornaDatePreviste()

            AggiornaRecuperoPosti()

            MyTransaction.Commit()
            'Else
            'AggiornaTabellaTemporaneaTelefonoGG()
            'RecuperaDatiPerGraduatoriaGG()
            'RecuperaDatePrevisteGG()

            'MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            'MyCommand.Transaction = MyTransaction

            'ScriviEntitaGG()
            'ScriviGraduatorieGG()
            'AggiornaStatoGraduatoria()
            'AggiornaDatePreviste()

            'AggiornaRecuperoPostiGG()

            'MyTransaction.Commit()
            'End If


        Catch exc As Exception

            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            swErr = True

        End Try

        MyCommand.Dispose()

        If swErr = False Then
            If EseguiOrdinaGraduatoria() = False Then
                'Errore EseguiOrdinaGraduatoria
                lblEsito.Text = "Errore durante l'operazione di ordinamento della graduatoria."
                lblEsito.Visible = True
                'CmdChiudi.Visible = True
            Else
                'Esito positivo
                lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."
                lblEsito.Visible = True
                'CmdChiudi.Visible = True
            End If
        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
            'CmdChiudi.Visible = True
        End If
        'If Session("Sistema") = "Helios" Then
        CancellaTabellaTemp()
        'Else
        'CancellaTabellaTempGG()
        'End If

    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        'If Session("Sistema") = "Helios" Then
        CancellaTabellaTemp()
        'Else
        'CancellaTabellaTempGG()
        'End If
        Response.Redirect("wfrmImportGraduatoriaVolontari.aspx")
    End Sub

    Private Sub dtgCSV_PreRender(sender As Object, e As System.EventArgs) Handles dtgCSV.PreRender
        'dtgCSV.Columns(0).ItemStyle.Width = Unit.Pixel(250)
        'dtgCSV.Columns(1).ItemStyle.Width = Unit.Pixel(250)
        'dtgCSV.Columns(2).ItemStyle.Width = Unit.Pixel(250)
        'dtgCSV.Columns(3).ItemStyle.Width = Unit.Pixel(250)
    End Sub
    'Private Sub AggiornaTabellaTemporaneaTelefono()
    '    'Antonello Di Croce 20/02/2008
    '    Dim strSql As String
    '    Dim dtrgenerico As SqlClient.SqlDataReader
    '    'Dim i As Integer
    '    Dim Telefoni As String
    '    Dim Prefissi As String
    '    Dim zero As String
    '    Dim codfisc As String
    '    Dim LetturaTelefonoOld As String
    '    zero = "0"

    '    strSql = "Select Telefono,codicefiscale FROM #TEMP_GRADUATORIA_VOLONTARI"
    '    dtsLocal = ClsServer.DataSetGenerico(strSql, Session("conn"))
    '    Dim dt As New DataTable
    '    Dim dr As DataRow
    '    Dim i As Integer
    '    Dim x As Integer

    '    Dim NomeColonne(1) As String
    '    Dim NomiCampiColonne(1) As String

    '    'nome della colonna 
    '    'e posizione nella griglia di lettura
    '    NomeColonne(0) = "Telefono"
    '    NomeColonne(1) = "Codice Fiscale"


    '    NomiCampiColonne(0) = "Telefono"
    '    NomiCampiColonne(1) = "Codicefiscale"


    '    'carico i nomi delle colonne 
    '    For x = 0 To 1
    '        dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
    '    Next

    '    'carico il datatable con il risultato della query 
    '    If dtsLocal.Tables(0).Rows.Count > 0 Then
    '        For i = 1 To dtsLocal.Tables(0).Rows.Count
    '            dr = dt.NewRow()
    '            For x = 0 To 1
    '                dr(x) = dtsLocal.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
    '            Next
    '            dt.Rows.Add(dr)
    '        Next
    '    End If

    '    Dim y As Integer
    '    For y = 0 To dt.Rows.Count - 1
    '        LetturaTelefonoOld = dt.Rows(y)(0)
    '        Telefoni = dt.Rows(y)(0)
    '        codfisc = dt.Rows(y)(1)
    '        Prefissi = Mid(Telefoni, 1, 3)
    '        Prefissi = Telefoni.Substring(0, 3)
    '        Select Case Prefissi
    '            'INIZIO TIM  -------------------------------------------
    '            Case "330"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "333"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "334"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "335"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "336"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "337"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "338"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "339"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "360"
    '                Telefoni = dt.Rows(y)(0)
    '                'Case "363"
    '                'Telefoni = dt.Rows(y)(0)
    '            Case "366"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "368"
    '                Telefoni = dt.Rows(y)(0)
    '                'FINE TIM  -------------------------------------------

    '                'INIZIO  VODAFONE  -------------------------------------------
    '            Case "340"
    '                Telefoni = dt.Rows(y)(0)
    '                'Case "343"
    '                'Telefoni = dt.Rows(y)(0)
    '            Case "347"
    '                Telefoni = dt.Rows(y)(0)
    '                'Case "346"
    '                'Telefoni = dt.Rows(y)(0)
    '            Case "348"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "349"
    '                Telefoni = dt.Rows(y)(0)
    '                'FINE VODAFONE  -------------------------------------------

    '                'INIZIO H3G  -------------------------------------------
    '                'Case "390"
    '                'Telefoni = dt.Rows(y)(0)
    '                'Case "391"
    '                'Telefoni = dt.Rows(y)(0)
    '                'Case "392"
    '                'Telefoni = dt.Rows(y)(0)
    '                'Case "393"
    '                'Telefoni = dt.Rows(y)(0)
    '                'FINE H3G  -------------------------------------------

    '                'INIZIO WIND  -------------------------------------------
    '            Case "320"
    '                Telefoni = dt.Rows(y)(0)
    '                'Case "323"
    '                'Telefoni = dt.Rows(y)(0)
    '            Case "327"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "328"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "329"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "380"
    '                Telefoni = dt.Rows(y)(0)
    '            Case "383"
    '                Telefoni = dt.Rows(y)(0)
    '                'Case "388"
    '                'Telefoni = dt.Rows(y)(0)
    '            Case "389"
    '                Telefoni = dt.Rows(y)(0)
    '                'FINE WIND  -------------------------------------------
    '            Case Else
    '                Telefoni = zero & dt.Rows(y)(0)
    '        End Select


    '        strSql = "UPDATE #TEMP_GRADUATORIA_VOLONTARI SET Telefono ='" & Telefoni & "' where Telefono = '" & LetturaTelefonoOld & "' and  codicefiscale ='" & codfisc & "'"
    '        MyCommand.CommandText = strSql
    '        MyCommand.ExecuteNonQuery()

    '    Next
    'End Sub
    Private Function RecuperaDatiPerGraduatoria() As Boolean
        Dim dtrAggiorna As SqlClient.SqlDataReader
        Dim strSql As String
        Dim i As Integer

        Try
            strSql = "SELECT " & _
                     "DISTINCT e.IdAttivitàSedeAssegnazione " & _
                     "FROM #TEMP_GRADUATORIA_VOLONTARI a " & _
                     "INNER JOIN Attività b ON a.CodiceProgetto = b.CodiceEnte " & _
                     "INNER JOIN EntiSediAttuazioni c ON  a.CodiceSede= c.IdEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi d ON c.IdEnteSede = d.IdEnteSede " & _
                     "INNER JOIN AttivitàSediAssegnazione e ON d.IdEnteSede = e.IdEnteSede AND b.IdAttività = e.IdAttività"

            'Originale sostituita da antonello 21/08/2019
            '"INNER JOIN EntiSediAttuazioni c ON CASE a.TipoSedePrincipale WHEN 'ITALIA' THEN a.CodiceSede ELSE a.CodiceSedeEstero END = c.IdEnteSedeAttuazione " & _

            dtrAggiorna = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrAggiorna.HasRows = True Then
                i = 0
                While dtrAggiorna.Read

                    If i = 0 Then
                        ReDim ArrAggiorna(0)
                    Else
                        ReDim Preserve ArrAggiorna(UBound(ArrAggiorna) + 1)
                    End If
                    ArrAggiorna(i) = dtrAggiorna(0)
                    i = i + 1
                End While
            End If
            dtrAggiorna.Close()
            dtrAggiorna = Nothing
            RecuperaDatiPerGraduatoria = True
        Catch
            If Not dtrAggiorna Is Nothing Then
                dtrAggiorna.Close()
                dtrAggiorna = Nothing
            End If
            RecuperaDatiPerGraduatoria = False
        End Try

    End Function
    Private Function RecuperaDatePreviste() As Boolean
        Dim dtrDataPrevista As SqlClient.SqlDataReader
        Dim strSql As String
        Dim i As Integer
        Dim xId As Integer

        Try
            strSql = "SELECT " & _
                     "b.IdAttività, " & _
                     "a.DataInizioPrevista, " & _
                     "DateAdd(day, -1, DateAdd(month, b.NMesi, a.DataInizioPrevista)) as DataFinePrevista " & _
                     "FROM #TEMP_GRADUATORIA_VOLONTARI a " & _
                     "INNER JOIN Attività b ON a.CodiceProgetto = b.CodiceEnte "

            dtrDataPrevista = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrDataPrevista.HasRows = True Then
                i = -1
                xId = 0
                While dtrDataPrevista.Read
                    If xId <> dtrDataPrevista(0) Then
                        xId = dtrDataPrevista(0)
                        i = i + 1
                        If i = 0 Then
                            ReDim ArrDatePreviste(0)
                        Else
                            ReDim Preserve ArrDatePreviste(UBound(ArrDatePreviste) + 1)
                        End If
                    End If
                    ArrDatePreviste(i) = CStr(dtrDataPrevista(0)) & "," & CStr(dtrDataPrevista(1)) & "," & CStr(dtrDataPrevista(2))
                End While
            End If
            dtrDataPrevista.Close()
            dtrDataPrevista = Nothing
            RecuperaDatePreviste = True
        Catch ex As Exception
            If Not dtrDataPrevista Is Nothing Then
                dtrDataPrevista.Close()
                dtrDataPrevista = Nothing
            End If
            RecuperaDatePreviste = False
        End Try
    End Function
    Private Sub ScriviEntita()

        Dim strsql As String

        'strsql = "INSERT INTO Entità " & _
        '         "(Cognome, Nome, CodiceFiscale, DataNascita, Sesso, IdComuneNascita, IdComuneResidenza, Indirizzo, " & _
        '         "NumeroCivico, Cap, StatoCivile, UserNameStato, DataUltimoStato, IdStatoEntità, TmpCodiceProgetto, " & _
        '         "TmpIdSedeAttuazione, TMPIdSedeAttuazioneSecondaria, DisponibileStessoProg, DisponibileAltriProg," & _
        '         "Telefono, TitoloStudio, IdSedePrimaAssegnazione, Email, IDComuneDomicilio, IndirizzoDomicilio, " & _
        '         "NumeroCivicoDomicilio, CapDomicilio, DettaglioRecapitoResidenza, DettaglioRecapitoDomicilio, " & _
        '         "FlagIndirizzoValidoRes, FlagIndirizzoValidoDom, IdCategoriaEntità, IdTitoloStudioConseguimento, " & _
        '         "AnomaliaCF, IDStatiVerificaCFEntità, IdNazionalita, IdTipoStatoCivile, CodiceFiscaleConiuge, GMO, FAMI) " & _
        '         "SELECT " & _
        '         "upper(a.Cognome), " & _
        '         "upper(a.Nome), " & _
        '         "upper(a.CodiceFiscale), " & _
        '         "a.DataNascita, " & _
        '         "a.Sesso, " & _
        '         "b.IdComune, " & _
        '         "c.IdComune, " & _
        '         "a.Indirizzo, " & _
        '         "a.NumeroCivico, " & _
        '         "a.Cap, " & _
        '         "'Stato Libero', " & _
        '         "'" & Session("Utente") & "', " & _
        '         "'" & Day(Now) & "/" & Month(Now) & "/" & Year(Now) & "', " & _
        '         "1, " & _
        '         "a.CodiceProgetto, " & _
        '         " CASE a.TipoSedePrincipale WHEN 'ITALIA' THEN a.CodiceSede ELSE a.CodiceSedeEstero END, " & _
        '         " CASE (CASE a.TipoSedePrincipale WHEN 'ITALIA' THEN a.CodiceSedeEstero ELSE a.CodiceSede END) WHEN 0 THEN NULL ELSE (CASE a.TipoSedePrincipale WHEN 'ITALIA' THEN a.CodiceSedeEstero ELSE a.CodiceSede END) END , " & _
        '         "a.SubentroStessoProgetto, " & _
        '         "a.SubentroAltriProgetti, " & _
        '         "a.Telefono, a.TitoloStudio, a.CodiceSedePrimoGiorno, " & _
        '         "a.Email," & _
        '         "d.IDComune ," & _
        '         "a.IndirizzoDomicilio, " & _
        '         "a.NumeroCivicoDomicilio, " & _
        '         "a.CapDomicilio, a.DettaglioRecapitoRes, a.DettaglioRecapitoDom, a.FlagIndirizzoValidoRes, a.FlagIndirizzoValidoDom, " & _
        '         "e.IdCategoriaEntità, f.IdTitoloStudioConseguimento, a.AnomaliaCF, a.IDStatiVerificaCFEntità, isnull(cit.Idcomune,38715), g.IdTipoStatoCivile, a.CodiceFiscaleConiuge," & _
        '         " CASE GMO WHEN '' THEN NULL ELSE GMO END, CASE FAMI WHEN '' THEN NULL ELSE FAMI END " & _
        '         "FROM #TEMP_GRADUATORIA_VOLONTARI a " & _
        '         "INNER JOIN Comuni b ON (a.CodiceISTATComuneNascita = b.CodiceISTAT OR a.CodiceISTATComuneNascita = b.CodiceISTATDismesso)" & _
        '         "INNER JOIN Comuni c ON a.CodiceISTATComuneResidenza = c.CodiceISTAT " & _
        '         "Left JOIN Comuni cit ON case isnull(a.Nazionalita,'') when '' then 'nonindicato' else a.Nazionalita end = cit.CodiceISTAT " & _
        '         "INNER JOIN CategorieEntità e ON a.Categoria = e.Codice " & _
        '         "INNER JOIN TitoliStudioConseguimento f ON a.ConseguimentoTitoloStudio = f.Codice " & _
        '         "INNER JOIN TipiStatoCivile g ON a.CodiceStatoCivile = g.Codice " & _
        '         "LEFT JOIN Comuni  d ON CASE a.CodiceISTATComuneDomicilio WHEN '' THEN '-1' ELSE a.CodiceISTATComuneDomicilio END = d.CodiceISTAT  "



        strsql = " UPDATE entità SET IdSedePrimaAssegnazione=a.CodiceSedePrimoGiorno, TMPIdSedeAttuazioneSecondaria=a.CodiceSedeSecondaria , UserNameStato='" & Session("Utente") & "', DataUltimoStato=getdate() " & _
                 " FROM #TEMP_GRADUATORIA_VOLONTARI a " & _
                 " inner join entità b on a.CodiceProgetto = b.TMPCodiceProgetto and a.CodiceFiscale = b.CodiceFiscale and a.CodiceSede = b.TMPIdSedeAttuazione " '& _
        '" inner join TipIStatoCivile as s on a.statocivile = s.codice "

       

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

    End Sub

    Private Sub ScriviGraduatorie()
        Dim cmdGraduatorie As SqlClient.SqlCommand
        Dim strsql As String

        'strsql = "INSERT INTO GraduatorieEntità " & _
        '         "(IdEntità, " & _
        '         "IdAttivitàSedeAssegnazione, " & _
        '         "Stato, " & _
        '         "Ammesso, " & _
        '         "Punteggio, " & _
        '         "IdTipologiaPosto, " & _
        '         "UserName, " & _
        '         "DataModifica, " & _
        '         "Ordine) " & _
        '         "SELECT " & _
        '         "b.IdEntità, " & _
        '         "g.IdAttivitàSedeAssegnazione, " & _
        '         "a.Idoneo, " & _
        '         "a.Selezionato, " & _
        '         "a.Punteggio, " & _
        '         "a.TipoPosto, " & _
        '         "'" & Session("Utente") & "', " & _
        '         "GETDATE(), " & _
        '         "'0' " & _
        '         "FROM #TEMP_GRADUATORIA_VOLONTARI a " & _
        '         "INNER JOIN Attività d ON a.CodiceProgetto = d.CodiceEnte " & _
        '         "INNER JOIN EntiSediAttuazioni e ON e.IdEntesedeAttuazione =  CASE a.TipoSedePrincipale WHEN 'ITALIA' THEN a.CodiceSede ELSE a.CodiceSedeEstero END " & _
        '         "INNER JOIN Entità b ON a.CodiceFiscale = b.CodiceFiscale and b.tmpcodiceprogetto = a.codiceprogetto and b.tmpidsedeattuazione = CASE a.TipoSedePrincipale WHEN 'ITALIA' THEN a.CodiceSede ELSE a.CodiceSedeEstero END  " & _
        '         " AND isnull(b.tmpidsedeattuazioneSecondaria,0) = CASE a.TipoSedePrincipale WHEN 'ITALIA' THEN isnull(a.CodiceSedeEstero,0) ELSE isnull(a.CodiceSede,0) END    " & _
        '         "INNER JOIN EntiSedi f ON f.IdEnteSede = e.IdenteSede " & _
        '         "INNER JOIN AttivitàSediAssegnazione g ON g.IdEnteSede = f.IdEnteSede and g.idattività = d.idattività "



        strsql = "UPDATE graduatorieentità  SET Punteggio=a.Punteggio, Ammesso=d.ammesso , Stato=d.stato ,IdTipoEsitoGraduatoriaEntità=d.IdTipoEsitoGraduatoriaEntità, IDTipologiaPosto=a.tipoposto, username='" & Session("Utente") & "',  DataModifica=getdate() " & _
                 " FROM #TEMP_GRADUATORIA_VOLONTARI a " & _
                 " inner join entità b on a.CodiceProgetto = b.TMPCodiceProgetto and a.CodiceFiscale = b.CodiceFiscale and a.CodiceSede = b.TMPIdSedeAttuazione " & _
                 " inner join graduatorieentità c on b.identità = c.identità " & _
                 " inner join TipiEsitiGraduatoriaEntità d on a.EsitoSelezione = d.codice "



        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

    End Sub
    Private Sub AggiornaRecuperoPosti()

        'recupero graduatorie relative a recupero posti
        Dim strSql As String
        Dim i As Integer
        Dim ArrId(1) As String
        Dim intAmmessoRecupero As Integer

        For i = 0 To UBound(ArrAggiorna)
            ArrId = Split(ArrAggiorna(i), ",")

            strSql = "SELECT isnull(AmmessoRecupero,0) as AmmessoRecupero from AttivitàSediAssegnazione WHERE " & _
            "IdAttivitàSedeAssegnazione = " & ArrId(0)


            MyCommand.CommandText = strSql
            intAmmessoRecupero = MyCommand.ExecuteScalar()

            If intAmmessoRecupero = 1 Then
                'aggiorno volontari relativi a graduatorie da recupero posti
                strSql = "update entità set ammessorecupero = 1 " & _
                "from entità a " & _
                "inner join graduatorieentità b on a.identità = b.identità " & _
                "inner join #TEMP_GRADUATORIA_VOLONTARI c on c.CodiceProgetto = a.TMPCodiceProgetto and c.CodiceFiscale = a.CodiceFiscale and c.CodiceSede = a.TMPIdSedeAttuazione " & _
                "where b.IdAttivitàSedeAssegnazione = " & ArrId(0)

                'Originale sostituita da antonello 21/08/2019
                '"inner join #TEMP_GRADUATORIA_VOLONTARI c ON c.CodiceFiscale = a.CodiceFiscale and c.codiceprogetto = a.tmpcodiceprogetto and CASE c.TipoSedePrincipale WHEN 'ITALIA' THEN c.CodiceSede ELSE c.CodiceSedeEstero END = a.tmpidsedeattuazione " & _

                MyCommand.CommandText = strSql
                MyCommand.ExecuteNonQuery()

                'aggiorno graduatorie relative a recupero posti
                strSql = "update AttivitàSediAssegnazione set ammessorecupero = 2, statograduatoria = 3 where IdAttivitàSedeAssegnazione = " & ArrId(0)

                MyCommand.CommandText = strSql
                MyCommand.ExecuteNonQuery()
            End If

        Next i

    End Sub
End Class