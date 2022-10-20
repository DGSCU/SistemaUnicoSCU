Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient

Public Class wfrmImportGraduatoriaVolontari
    Inherits System.Web.UI.Page
    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private xIdAttivita As Integer

    '-----------------------------blocco variabili campi file csv-------------------
    '0
    Private VAR_NOME As String = ""
    '1
    Private VAR_COGNOME As String = ""
    '2
    Private VAR_CODICEFISCALE As String = ""
    '3
    Private VAR_CODICEPROGETTO As String = ""
    '4
    Private VAR_CODICESEDE As String = ""
    '5
    Private VAR_ESITOSELEZIONE As String = ""
    '6
    Private VAR_PUNTEGGIO As String = ""
    '7
    Private VAR_TIPOPOSTO As String = ""
    '8
    Private VAR_CODICESEDEPRIMOGIORNO As String = ""
    '9
    Private VAR_CODICESEDESECONDARIA As String = ""
    ''10
    'Private VAR_STATOCIVILE As String = ""
    ''11
    'Private VAR_CODICEFISCALECONIUGE As String = ""
    '12
    Private VAR_DATAINIZIOPREVISTA As String = ""
    'nd
    Private VAR_IDONEO As String = ""
    'nd
    Private VAR_SELEZIONATO As String = ""
    'nd
    Private VAR_GIOVANEMINORIOPPORTUNITA As String

    '-------------------------------------------------------------------------------
    'PER CSV GARANZIA GIOVANI 

    '8
    'data domanda
    'Private VAR_DATADOMANDA As String = ""

    'E TUTTI GLI ALTRI A SCALARE
    '-------------------------------------------------------------------------------
#Region "Routine Importazione Graduatoria SCN"

    Private Sub UpLoad()
        '--- salvataggio del file sul server
        Dim swErr As Boolean
        swErr = False
        If txtSelFile.PostedFile.FileName.ToString <> "" Then
            Try
                NomeUnivoco = "graduatoriavolontari" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                Dim file As String
                Dim estensione As String
                file = LCase(txtSelFile.FileName.ToString)
                estensione = file.Substring(file.Length - 4)
                If estensione <> ".csv" Then
                    lblMessaggioErrore.Visible = True
                    lblMessaggioErrore.Text = "Selezionare il file nel formato CSV."
                    Exit Sub
                End If

                txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")

                CreaTabTemp()

            Catch exc As Exception
                swErr = True
                CancellaTabellaTemp()
            End Try

            If swErr = False Then
                LeggiCSV()
            End If


        Else
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = "Selezionare il file da inviare."
            Exit Sub

        End If

    End Sub

    Private Sub CreaTabTemp()
        'Gestione nuovi campi nel file d'importazione 
        'email,CodiceIstatiComuneDomicilio,IndirizzoDomicilio,NumeroDomicilio,CapDomicilio

        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand

        CancellaTabellaTemp()

        '--- CREO TAB TEMPORANEA
        strSql = "CREATE TABLE [#TEMP_GRADUATORIA_VOLONTARI] (" & _
                 "[Nome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                 "[Cognome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceFiscale] [nvarchar] (16) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceProgetto] [nvarchar] (22) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceSede] [int], " & _
                 "[EsitoSelezione] [varchar] (100), " & _
                 "[Punteggio] [decimal] (18,2), " & _
                 "[TipoPosto] [int], " & _
                 "[CodiceSedePrimoGiorno]  [int], " & _
                 "[CodiceSedeSecondaria] [int], " & _
                 "[DataInizioPrevista] [datetime]) "
        '"[StatoCivile] [nvarchar] (10)  COLLATE DATABASE_DEFAULT NULL , " & _
        '"[CodiceFiscaleConiuge] [nvarchar] (50)  COLLATE DATABASE_DEFAULT NULL , " & _

       
        cmdCreateTempTable = New SqlClient.SqlCommand
        cmdCreateTempTable.CommandText = strSql
        cmdCreateTempTable.Connection = Session("conn")
        cmdCreateTempTable.ExecuteNonQuery()
        cmdCreateTempTable.Dispose()
    End Sub

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
    Private Sub LeggiCSV()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim AppoNote As String
        Dim strsqlvol As String
        Dim dtrcontrollocodfis As SqlClient.SqlDataReader
        Dim mycommand As SqlClient.SqlCommand
        Dim blnCheckComune As Boolean
        Dim strIdComune As String
        Dim strCodCatasto As String
        Dim strNewCF As String
        Dim blnControlloDom As Boolean = False 'controllo campi domicilio
        Dim strNoteCap As String
        Dim blnFlagCF As Integer = 0

        Dim PrevedeEstero As String = ""
        Dim SoloItalia As Boolean = True





        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)

        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")

        '--- intestazione

        xLinea = Reader.ReadLine()
        Writer.WriteLine("Note;" & xLinea)

        ArrCampi = CreaArray(xLinea, 1)
        'ArrCampi = CreaArray(xLinea, 0)


        If UBound(ArrCampi) <> 10 Then  'se base 0
            strNote = strNote & "Il numero delle colonne inserite è diverso da quello previsto."
            swErr = True
            TotKo = TotKo + 1
            'Writer.WriteLine("Note;" & strNote)
            Writer.WriteLine(strNote & ";")
            strNote = vbNullString
        End If

        'creo un array che metto in sessione per concatenarlo alle note in fase di segnalazione errori
        Dim vSegnalazioneIndirizzoResidenza() As String
        ReDim vSegnalazioneIndirizzoResidenza(0)

        'creo un array che metto in sessione per concatenarlo alle note in fase di segnalazione errori
        Dim vSegnalazioneIndirizzoDomicilio() As String
        ReDim vSegnalazioneIndirizzoDomicilio(0)

        '--- scorro le righe
        xLinea = Reader.ReadLine()
        While (xLinea <> "")
            swErr = False
            blnFlagCF = 0
            Tot = Tot + 1
            ArrCampi = CreaArray(xLinea, 0)
            ' If UBound(ArrCampi) < 31 Then '21
            If UBound(ArrCampi) < 1 Then '21
                'If UBound(ArrCampi) < 21 Then '21
                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            Else
                If UBound(ArrCampi) > 10 Then '21
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1
                Else


                    '----------------------------ASSEGNAZIONE VARIABILI-------------------ADC-------------
                    'Nome 0
                    Try
                        VAR_NOME = Trim(ArrCampi(0))
                    Catch ex As Exception
                        VAR_NOME = ""
                    End Try
                    'Cognome  1
                    Try
                        VAR_COGNOME = Trim(ArrCampi(1))
                    Catch ex As Exception
                        VAR_COGNOME = ""
                    End Try
                    'CodiceFiscale 2
                    Try
                        VAR_CODICEFISCALE = Trim(ArrCampi(2))
                    Catch ex As Exception
                        VAR_CODICEFISCALE = ""
                    End Try

                    'codice progetto 3
                    Try
                        VAR_CODICEPROGETTO = Trim(ArrCampi(3))
                    Catch ex As Exception
                        VAR_CODICEPROGETTO = ""
                    End Try
                    'codice sede 4
                    Try
                        VAR_CODICESEDE = Trim(ArrCampi(4))
                    Catch ex As Exception
                        VAR_CODICESEDE = ""
                    End Try
                    ' esito selezione 5
                    Try
                        VAR_ESITOSELEZIONE = Trim(ArrCampi(5))
                    Catch ex As Exception

                    End Try
                    'Punteggio 6
                    Try
                        VAR_PUNTEGGIO = Trim(ArrCampi(6))
                    Catch ex As Exception
                        VAR_PUNTEGGIO = ""
                    End Try

                    'Tipo Posto 7
                    Try
                        VAR_TIPOPOSTO = Trim(ArrCampi(7))
                    Catch ex As Exception
                        VAR_TIPOPOSTO = ""
                    End Try

                    'codice sede primo giorno 8
                    Try
                        VAR_CODICESEDEPRIMOGIORNO = Trim(ArrCampi(8))
                    Catch ex As Exception
                        VAR_CODICESEDEPRIMOGIORNO = ""
                    End Try
                    'codice sede secondaria 9
                    Try
                        VAR_CODICESEDESECONDARIA = Trim(ArrCampi(9))
                    Catch ex As Exception
                        VAR_CODICESEDESECONDARIA = ""
                    End Try
                    'stato civile 10
                    'Try
                    '    VAR_STATOCIVILE = Trim(ArrCampi(10))
                    'Catch ex As Exception
                    '    VAR_STATOCIVILE = ""
                    'End Try

                    ''codice fiscale coniuge 11
                    'Try
                    '    VAR_CODICEFISCALECONIUGE = Trim(ArrCampi(11))
                    'Catch ex As Exception
                    '    VAR_CODICEFISCALECONIUGE = ""
                    'End Try
                    'Data Inizio Prevista 12
                    'Data Inizio Prevista 10
                    Try
                        VAR_DATAINIZIOPREVISTA = Trim(ArrCampi(10))
                    Catch ex As Exception
                        VAR_DATAINIZIOPREVISTA = ""
                    End Try



                    '----------------------------FINE  ASSEGNAZIONE VARIABILI  --------------ADC------------------



                    '---------------------------INIZIO CONTROLLI --------------ADC------------------


                    'CodiceFiscale
                    'tabella  Entita   controllo se per quel progetto se per quella sede se per quel codicefiscale tira fuori un record




                    mycommand = New SqlClient.SqlCommand
                    mycommand.Connection = Session("conn")
                    strsqlvol = "select TMPCodiceProgetto , TMPIdSedeAttuazione, codicefiscale, isnull(GMO,'') as GMO from entità where TMPCodiceProgetto='" & VAR_CODICEPROGETTO & "' and TMPIdSedeAttuazione='" & VAR_CODICESEDE & "' and codicefiscale='" & VAR_CODICEFISCALE & "' "
                    mycommand.CommandText = strsqlvol
                    dtrcontrollocodfis = mycommand.ExecuteReader

                    If dtrcontrollocodfis.HasRows = False Then
                        VAR_GIOVANEMINORIOPPORTUNITA = ""

                        strNote = strNote & "CodiceFiscale non e' associato al progetto e alla sede indicata."
                        swErr = True
                    Else
                        dtrcontrollocodfis.Read()
                        VAR_GIOVANEMINORIOPPORTUNITA = dtrcontrollocodfis("GMO")
                    End If

                    If Not dtrcontrollocodfis Is Nothing Then
                        dtrcontrollocodfis.Close()
                        dtrcontrollocodfis = Nothing
                    End If


                    If VerificaProgetto(VAR_CODICEPROGETTO, PrevedeEstero, SoloItalia) = False Then
                        strNote = strNote & "Il CodiceProgetto indicato non e' tra i progetti in attesa di graduatoria."
                        swErr = True
                    Else
                        'DA AGGIUNGERE CONTROLLO CHE BANDO VOLONTARI SIA TERMINATO
                        If VerificaChiusuraBandoVolontari(VAR_CODICEPROGETTO) = False Then
                            strNote = strNote & "L'iscrizione al bando dei volontari e' ancora aperta pertanto non e' possibile caricare le graduatorie."
                            swErr = True
                        End If

                        If VerificaDataScadenzaGraduatoria(VAR_CODICEPROGETTO) = False Then
                            strNote = strNote & "Sono scaduti i termini per il caricamento della graduatoria."
                            swErr = True
                        End If
                    End If

                    'CodiceSede
                    If IsNumeric(VAR_CODICESEDE) = False Then
                        strNote = strNote & "Il campo CodiceSede non e' nel formato corretto."
                        swErr = True
                    End If
                    'Verifico che la sede di attuazione sia legata al progetto
                    'If VerificaAttivitaEnteSede(VAR_CODICESEDE, VAR_CODICEPROGETTO, "ITALIA") = False Then
                    '    strNote = strNote & "La Sede indicata non e' tra le sedi italiane del Progetto indicato."
                    '    swErr = True
                    'End If
                    'If VerificaVolontariPresentati(VAR_CODICESEDE) = False Then
                    '    strNote = strNote & "Per la sede di progetto non e' stato indicato il numero di volontari che hanno presentato domanda."
                    '    swErr = True
                    'End If
                    'Controllo che su quella sede non sia gia stata eseguito il caricamento di una graduatoria
                    If VerificaEsistenzaGraduatoria(VAR_CODICESEDE, VAR_CODICEPROGETTO) = False Then
                        strNote = strNote & "Per la Sede e il progetto indicati, e' gia stata caricata una graduatoria."
                        swErr = True
                    End If


                    '------------------------------------CONTROLLI CODICE SEDE SECONDARIA DA FARE  INIZIO-------------------ADC-----------------




                    'Se prevede estero la sede secondaria deve essere valorizzata
                    If PrevedeEstero = "SI" And VAR_CODICESEDESECONDARIA.Trim = "" Then
                        strNote = strNote & "Il campo CodiceSedeSecondaria deve essere valorizzato per i progetti ESTERO ed per i progetti ITALIA con periodo di servizio in un paese UE."
                        swErr = True
                    End If

                    'Verifico che la sede di attuazione sia legata al progetto
                    If VAR_CODICESEDESECONDARIA <> "" Then
                        If IsNumeric(VAR_CODICESEDESECONDARIA) = False Then
                            strNote = strNote & "Il campo CodiceSedeSecondaria non e' nel formato corretto."
                            swErr = True
                        End If
                        If VerificaAttivitaEnteSede(VAR_CODICESEDESECONDARIA, VAR_CODICEPROGETTO, "ESTERO") = False Then
                            strNote = strNote & "La Sede Secondaria indicata non è prevista dal Progetto indicato."
                            swErr = True
                        End If
                    End If

                    'If VerificaVolontariPresentati(VAR_CODICESEDESECONDARIA) = False Then
                    '    strNote = strNote & "Per la sede di progetto non e' stato indicato il numero di volontari che hanno presentato domanda."
                    '    swErr = True
                    'End If

                    'Controllo che su quella sede non sia gia stata eseguito il caricamento di una graduatoria
                    'If VerificaEsistenzaGraduatoria(VAR_CODICESEDESECONDARIA, VAR_CODICEPROGETTO) = False Then
                    '    strNote = strNote & "Per la Sede e il progetto indicati, e' gia stata caricata una graduatoria."
                    '    swErr = True
                    'End If

                    'If VAR_CODICESEDESECONDARIA <> vbNullString Then
                    '    strNote = strNote & "Il campo CodiceSedeEstero non deve essere valorizzato per il progetto indicato."
                    '    swErr = True
                    'End If

                    '---------------------------------------------SEDE SECONDARIA FINE--------------------------ADC---------------------------------



                    'CodiceSedePrimoGiorno

                    If UCase(VAR_CODICESEDEPRIMOGIORNO) <> vbNullString Then
                        If IsNumeric(VAR_CODICESEDEPRIMOGIORNO) = False Then
                            strNote = strNote & "Il campo Codice Sede Primo Giorno non e' nel formato corretto."
                            swErr = True
                        Else
                            If VerificaSedePrimo(VAR_CODICESEDEPRIMOGIORNO, VAR_CODICEPROGETTO) = False Then
                                strNote = strNote & "La Sede di prima convocazione indicata non e' tra le sedi attive dell'Ente."
                                swErr = True
                            End If
                        End If
                    Else
                        strNote = strNote & "Il campo Codice Sede Primo Giorno deve essere valorizzato."
                        swErr = True
                    End If

                    'DataInizioPrevista 

                    If IsDate(VAR_DATAINIZIOPREVISTA) = False Then
                        strNote = strNote & "La Data Inizio Prevista non e' nel formato corretto."
                        swErr = True
                    Else
                        If CDate(VAR_DATAINIZIOPREVISTA) < CDate(Now) Then
                            strNote = strNote & "La Data Inizio Prevista deve essere successiva alla data odierna."
                            swErr = True
                        Else
                            If Len(VAR_DATAINIZIOPREVISTA) = 8 Then

                                VAR_DATAINIZIOPREVISTA = Left(VAR_DATAINIZIOPREVISTA, 6) & "20" & Right(VAR_DATAINIZIOPREVISTA, 2)
                            End If
                        End If
                    End If

                    'ESITOSELEZIONE Slezionato
                    If VerificaEsitoSelezione(VAR_ESITOSELEZIONE, VAR_IDONEO, VAR_SELEZIONATO) = False Then
                        strNote = strNote & "Il campo EsitoSelezione non e' nel formato corretto."
                        swErr = True
                    End If

                    'Punteggio

                    If IsNumeric(VAR_PUNTEGGIO) = False Then
                        strNote = strNote & "Il campo Punteggio non e' nel formato corretto."
                        swErr = True
                    End If


                    'TipoPosto

                    If IsNumeric(VAR_TIPOPOSTO) = False Then
                        strNote = strNote & "Il campo TipoPosto non e' nel formato corretto."
                        swErr = True
                    Else
                        If VerificaTipoPosto(VAR_TIPOPOSTO) = False Then
                            strNote = strNote & "Il TipoPosto indicato non e' presente nella base dati."
                            swErr = True
                        End If
                    End If





                    'Controllo se il numero dei posti per sede di attuazione in base alla tipologia siano congruenti


                    If UCase(VAR_IDONEO) = "SI" And UCase(VAR_SELEZIONATO) = "SI" Then
                        If VerificaNumeroDiPosti(VAR_CODICESEDE, VAR_CODICEPROGETTO, VAR_TIPOPOSTO) = False Then
                            strNote = strNote & "E' stato superato il numero di volontari previsti per il progetto,la sede e la tipologia di posto indicati."
                            swErr = True
                        End If
                    End If

                    Dim messaggio As String = ""



                    'If VerificaStatoCivile(VAR_STATOCIVILE) = False Then

                    '    strNote = strNote & "Il Codice dello StatoCivile e' errato."
                    '    swErr = True

                    'Else

                    '    If VerificaOblligatorietàCodiceFiscaleConiuge(VAR_STATOCIVILE) = True Then

                    '        If VAR_CODICEFISCALECONIUGE <> "" Then

                    '            If VAR_CODICEFISCALE = VAR_CODICEFISCALECONIUGE Then
                    '                strNote = strNote & "Il Codice Fiscale Coniuge e' uguale al codicefiscale del volontario "
                    '                swErr = True

                    '            Else

                    '                If ClsUtility.VerificaValiditàCodiceFiscaleConiuge(VAR_CODICEFISCALECONIUGE) = False Then
                    '                    strNote = strNote & "Il Codice Fiscale Coniuge e' ERRATO "
                    '                    swErr = True
                    '                End If

                    '            End If

                    '        Else

                    '            strNote = strNote & "Per lo stato civile indicato il campo CodiceFiscaleConiuge e' un campo obbligatorio."
                    '            swErr = True
                    '        End If
                    '    Else
                    '        If VAR_CODICEFISCALECONIUGE <> "" Then

                    '            strNote = strNote & "Per lo stato civile indicato il campo CodiceFiscaleConiuge deve essere vuoto."
                    '            swErr = True
                    '        End If

                    '    End If

                    'End If

                    'End If
                    If VAR_GIOVANEMINORIOPPORTUNITA <> vbNullString Then
                        If ParticolaritàEntità("GMO", VAR_GIOVANEMINORIOPPORTUNITA) = False Then
                            strNote = strNote & "Il campo GiovaniMinoriOpportunità non è tra quelli codificati."
                            swErr = True
                        End If
                    End If

                    'controllo posti giovani minori opportunità
                    If VerificaGMOProgetto(VAR_CODICEPROGETTO, UCase(VAR_GIOVANEMINORIOPPORTUNITA), messaggio) = True Then
                        If messaggio <> "" Then
                            strNote = strNote & messaggio
                            swErr = True
                        End If
                    End If

                    'RIMOSSO CONTROLLO SU POSTI GMO RICHIESTA DIPARTIMENTO MARZO 2021
                    'If UCase(VAR_IDONEO) = "SI" And UCase(VAR_SELEZIONATO) = "SI" Then
                    '    If VerificaNumeroPostiGMO(VAR_CODICEPROGETTO, VAR_CODICESEDE, UCase(VAR_GIOVANEMINORIOPPORTUNITA), messaggio) = False Then
                    '        strNote = strNote & messaggio
                    '        swErr = True
                    '    End If
                    'End If






                    'swErr = False
                    'strNote = ""
                    If swErr = False Then
                        ScriviTabTemp(ArrCampi, blnFlagCF)
                    Else
                        'da rimuovere 
                        'ScriviTabTemp(ArrCampi, blnFlagCF)
                        TotKo = TotKo + 1
                    End If
                End If
            End If
            Writer.WriteLine(strNote & ";" & xLinea)
            'Writer.WriteLine(Replace(Replace(Replace(strNote, "<a style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('", ""), "')" & """" & "><font color=" & """" & "blue" & """" & "><b>SUGGERIMENTO</b></font></a> .", ""), "|", "'") & ";" & xLinea)
            strNote = vbNullString
            xLinea = Reader.ReadLine()
        End While

        'passo gli array che contentgono le segnalazioni 
        'sull'indirizzo del domicilio ad una sessione, 
        'così da controllare nella visualizzazione delle note
        'gli eventuali suggerimenti
        'If vSegnalazioneIndirizzoDomicilio(0) <> "" Then
        '    Session("ArraySegnalazioneIndirizzoDomicilio") = vSegnalazioneIndirizzoDomicilio
        'End If

        'passo gli array che contentgono le segnalazioni 
        'sull'indirizzo di residenza ad una sessione, 
        'così da controllare nella visualizzazione delle note
        'gli eventuali suggerimenti
        'If vSegnalazioneIndirizzoResidenza(0) <> "" Then
        '    Session("ArraySegnalazioneIndirizzoResidenza") = vSegnalazioneIndirizzoResidenza
        'End If
        Reader.Close()
        Writer.Close()


        If TotKo = 0 Then
            Dim messaggioritorno As String
            messaggioritorno = VerificaCompletamentoGraduatorie()
            If messaggioritorno <> "" Then
                TotKo = TotKo + 1

                Response.Redirect("WfrmRisultatoImportGraduatoriaVolontari.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "&MessGrad=" & messaggioritorno)
            End If
        End If




      
        Response.Redirect("WfrmRisultatoImportGraduatoriaVolontari.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
        ''--- reindirizzo la pagina sottostante
        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportGraduatoriaVolontari.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
        'Response.Write("</script>")
    End Sub
#End Region

#Region "Routine Importazione Graduatoria GG"
    'Private Sub UpLoadGG()
    '    '--- salvataggio del file sul server
    '    Dim swErr As Boolean
    '    swErr = False
    '    If txtSelFile.PostedFile.FileName.ToString <> "" Then
    '        Try
    '            NomeUnivoco = "graduatoriavolontariGG" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)

    '            Dim file As String
    '            Dim estensione As String
    '            file = LCase(txtSelFile.FileName.ToString)
    '            estensione = file.Substring(file.Length - 4)
    '            If estensione <> ".csv" Then
    '                lblMessaggioErrore.Visible = True
    '                lblMessaggioErrore.Text = "Selezionare il file nel formato CSV."
    '                Exit Sub
    '            End If

    '            txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")

    '            CreaTabTempGG()

    '        Catch exc As Exception
    '            swErr = True
    '            'Response.Write("<script>" & vbCrLf)
    '            'Response.Write("parent.fraUp.CambiaImmagine('filenoncaricato.jpg','0')" & vbCrLf)
    '            'Response.Write("</script>")
    '            CancellaTabellaTempGG()
    '        End Try

    '        If swErr = False Then
    '            LeggiCSVGG()

    '            'Response.Write("<script>" & vbCrLf)
    '            'Response.Write("parent.fraUp.CambiaImmagine('allegatocompletato.jpg','1')" & vbCrLf)
    '            'Response.Write("</script>")
    '        End If

    '    Else
    '        lblMessaggioErrore.Visible = True
    '        lblMessaggioErrore.Text = "Selezionare il file da inviare."
    '        Exit Sub

    '    End If

    'End Sub

    'OK 1
    'Private Sub CancellaTabellaTempGG()
    '    Dim strSql As String
    '    Dim cmdCanTempTable As SqlClient.SqlCommand
    '    Try
    '        '--- CANCELLO TAB TEMPORANEA
    '        strSql = "DROP TABLE [#TEMP_GRADUATORIA_VOLONTARIGG]"

    '        cmdCanTempTable = New SqlClient.SqlCommand
    '        cmdCanTempTable.CommandText = strSql
    '        cmdCanTempTable.Connection = Session("conn")
    '        cmdCanTempTable.ExecuteNonQuery()
    '    Catch e As Exception
    '    End Try

    '    cmdCanTempTable.Dispose()
    'End Sub

    'OK 1
    'Private Sub CreaTabTempGG()
    '    'Gestione nuovi campi nel file d'importazione 
    '    'email,CodiceIstatiComuneDomicilio,IndirizzoDomicilio,NumeroDomicilio,CapDomicilio

    '    Dim strSql As String
    '    Dim cmdCreateTempTable As SqlClient.SqlCommand

    '    CancellaTabellaTempGG()

    '    '--- CREO TAB TEMPORANEA
    '    strSql = "CREATE TABLE [#TEMP_GRADUATORIA_VOLONTARIGG] (" & _
    '             "[Cognome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
    '             "[Nome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
    '             "[CodiceFiscale] [nvarchar] (16) COLLATE DATABASE_DEFAULT, " & _
    '             "[DataNascita] [datetime], " & _
    '             "[Sesso] [nvarchar] (1) COLLATE DATABASE_DEFAULT, " & _
    '             "[CodiceISTATComuneNascita] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
    '             "[Categoria] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
    '             "[Nazionalita] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
    '             "[DataDomanda] [datetime], " & _
    '             "[CodiceISTATComuneResidenza] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
    '             "[Indirizzo] [nvarchar] (200) COLLATE DATABASE_DEFAULT, " & _
    '             "[NumeroCivico] [nvarchar] (10) COLLATE DATABASE_DEFAULT, " & _
    '             "[Cap] [nvarchar] (8) COLLATE DATABASE_DEFAULT, " & _
    '             "[CodiceProgetto] [nvarchar] (22) COLLATE DATABASE_DEFAULT, " & _
    '             "[DataInizioPrevista] [datetime], " & _
    '             "[CodiceSede] [int], " & _
    '             "[CodiceSedePrimoGiorno]  [int], " & _
    '             "[Idoneo] [bit], " & _
    '             "[Selezionato] [bit], " & _
    '             "[Punteggio] [decimal] (18,2), " & _
    '             "[TipoPosto] [int], " & _
    '             "[SubentroStessoProgetto] [bit], " & _
    '             "[SubentroAltriProgetti] [bit]," & _
    '             "[Telefono] [nvarchar] (20) COLLATE DATABASE_DEFAULT," & _
    '             "[TitoloStudio] [nvarchar] (100) COLLATE DATABASE_DEFAULT ," & _
    '             "[ConseguimentoTitoloStudio] [nvarchar] (100) COLLATE DATABASE_DEFAULT ," & _
    '             "[Email] [nvarchar] (100)  COLLATE DATABASE_DEFAULT NULL ," & _
    '             "[CodiceISTATComuneDomicilio] [nvarchar] (100) COLLATE DATABASE_DEFAULT NULL ," & _
    '             "[IndirizzoDomicilio] [nvarchar] (200)  COLLATE DATABASE_DEFAULT NULL , " & _
    '             "[NumeroCivicoDomicilio] [nvarchar] (10)  COLLATE DATABASE_DEFAULT NULL , " & _
    '             "[CapDomicilio] [nvarchar] (8)  COLLATE DATABASE_DEFAULT NULL , " & _
    '             "[DettaglioRecapitoRes] [nvarchar] (20)  COLLATE DATABASE_DEFAULT NULL , " & _
    '             "[DettaglioRecapitoDom] [nvarchar] (20)  COLLATE DATABASE_DEFAULT NULL , " & _
    '             "[CodiceStatoCivile] [nvarchar] (10)  COLLATE DATABASE_DEFAULT NULL , " & _
    '             "[CodiceFiscaleConiuge] [nvarchar] (50)  COLLATE DATABASE_DEFAULT NULL , " & _
    '             "[FlagIndirizzoValidoRes] [bit], " & _
    '             "[FlagIndirizzoValidoDom] [bit],AnomaliaCF int NULL, " & _
    '             "[IDStatiVerificaCFEntità] int NULL ) "
    '    '"[CodiceSedeConvocazione] [int], " & _
    '    '")"
    '    cmdCreateTempTable = New SqlClient.SqlCommand
    '    cmdCreateTempTable.CommandText = strSql
    '    cmdCreateTempTable.Connection = Session("conn")
    '    cmdCreateTempTable.ExecuteNonQuery()
    '    cmdCreateTempTable.Dispose()
    'End Sub


    'OK 1
    'Private Sub LeggiCSVGG()
    '    '--- lettura del file
    '    Dim Reader As StreamReader
    '    Dim xLinea As String
    '    Dim ArrCampi() As String
    '    Dim swErr As Boolean
    '    Dim AppoNote As String
    '    Dim strsqlvol As String
    '    Dim dtrcontrollocodfis As SqlClient.SqlDataReader
    '    Dim mycommand As SqlClient.SqlCommand
    '    Dim blnCheckComune As Boolean
    '    Dim strIdComune As String
    '    Dim strCodCatasto As String
    '    Dim strNewCF As String
    '    Dim blnControlloDom As Boolean = False 'controllo campi domicilio
    '    Dim strNoteCap As String
    '    Dim blnFlagCF As Integer = 0

    '    Dim PrevedeEstero As String = ""
    '    Dim SoloItalia As Boolean = True

    '    '--- Leggo il file di input e scrivo quello di output
    '    Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)

    '    Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")

    '    '--- intestazione

    '    xLinea = Reader.ReadLine()
    '    Writer.WriteLine("Note;" & xLinea)

    '    ArrCampi = CreaArrayGG(xLinea, 1)
    '    If UBound(ArrCampi) < 34 Or UBound(ArrCampi) > 34 Then
    '        strNote = strNote & "Il numero delle colonne inserite è diverso da quello previsto."
    '        swErr = True
    '        TotKo = TotKo + 1
    '        'Writer.WriteLine("Note;" & strNote)
    '        Writer.WriteLine(strNote & ";")
    '        strNote = vbNullString
    '    End If

    '    'creo un array che metto in sessione per concatenarlo alle note in fase di segnalazione errori
    '    Dim vSegnalazioneIndirizzoResidenza() As String
    '    ReDim vSegnalazioneIndirizzoResidenza(0)

    '    'creo un array che metto in sessione per concatenarlo alle note in fase di segnalazione errori
    '    Dim vSegnalazioneIndirizzoDomicilio() As String
    '    ReDim vSegnalazioneIndirizzoDomicilio(0)

    '    '--- scorro le righe
    '    xLinea = Reader.ReadLine()
    '    While (xLinea <> "")
    '        swErr = False
    '        blnFlagCF = 0
    '        Tot = Tot + 1
    '        ArrCampi = CreaArrayGG(xLinea, 0)
    '        If UBound(ArrCampi) < 1 Then '21
    '            'If UBound(ArrCampi) < 33 Then '21
    '            'If UBound(ArrCampi) < 21 Then '21
    '            '--- se i campi non sono tutti errore
    '            strNote = "Il numero delle colonne inserite è minore di quello richieste."
    '            swErr = True
    '            TotKo = TotKo + 1
    '        Else
    '            If UBound(ArrCampi) > 34 Then '21
    '                '--- se i campi sono troppi errore
    '                strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
    '                swErr = True
    '                TotKo = TotKo + 1
    '            Else


    '                '----------------------------ASSEGNAZIONE VARIABILI----GG---------------ADC-------------

    '                'Cognome 
    '                Try
    '                    VAR_COGNOME = Trim(ArrCampi(0))
    '                Catch ex As Exception
    '                    VAR_COGNOME = ""
    '                End Try


    '                'Nome
    '                Try
    '                    VAR_NOME = Trim(ArrCampi(1))
    '                Catch ex As Exception
    '                    VAR_NOME = ""
    '                End Try

    '                'CodiceFiscale
    '                Try
    '                    VAR_CODICEFISCALE = Trim(ArrCampi(2))
    '                Catch ex As Exception
    '                    VAR_CODICEFISCALE = ""
    '                End Try

    '                'DataNascita
    '                Try
    '                    VAR_DATANASCITA = Trim(ArrCampi(3))
    '                Catch ex As Exception
    '                    VAR_DATANASCITA = ""
    '                End Try

    '                'Sesso
    '                Try
    '                    VAR_SESSO = Trim(ArrCampi(4))
    '                Catch ex As Exception
    '                    VAR_SESSO = ""
    '                End Try

    '                'codice istat Comune Nascita
    '                Try
    '                    VAR_CODICEISTATCOMUNENASCITA = Trim(ArrCampi(5))
    '                Catch ex As Exception
    '                    VAR_CODICEISTATCOMUNENASCITA = ""
    '                End Try


    '                'Categoria
    '                Try
    '                    VAR_CATEGORIA = Trim(ArrCampi(6))
    '                Catch ex As Exception
    '                    VAR_CATEGORIA = ""
    '                End Try

    '                'Nazionalità
    '                Try
    '                    VAR_NAZIONALITA = Trim(ArrCampi(7))
    '                Catch ex As Exception
    '                    VAR_NAZIONALITA = ""
    '                End Try


    '                'Data Domanda
    '                Try
    '                    VAR_DATADOMANDA = Trim(ArrCampi(8))
    '                Catch ex As Exception
    '                    VAR_DATADOMANDA = ""
    '                End Try


    '                'CodiceIstatComuneResidenza
    '                Try
    '                    VAR_CODICEISTATCOMUNERESIDENZA = Trim(ArrCampi(9))
    '                Catch ex As Exception
    '                    VAR_CODICEISTATCOMUNERESIDENZA = ""
    '                End Try

    '                'indirizzo
    '                Try
    '                    VAR_INDIRIZZO = Trim(ArrCampi(10))
    '                Catch ex As Exception
    '                    VAR_INDIRIZZO = ""
    '                End Try

    '                'numero civico
    '                Try
    '                    VAR_NUMEROCIVICO = Trim(ArrCampi(11))
    '                Catch ex As Exception
    '                    VAR_NUMEROCIVICO = ""
    '                End Try

    '                'cap
    '                Try
    '                    VAR_CAP = Trim(ArrCampi(12))
    '                Catch ex As Exception
    '                    VAR_CAP = ""
    '                End Try

    '                'codice progetto
    '                Try
    '                    VAR_CODICEPROGETTO = Trim(ArrCampi(13))
    '                Catch ex As Exception
    '                    VAR_CODICEPROGETTO = ""
    '                End Try

    '                'Data Inizio Prevista
    '                Try
    '                    VAR_DATAINIZIOPREVISTA = Trim(ArrCampi(14))
    '                Catch ex As Exception
    '                    VAR_DATAINIZIOPREVISTA = ""
    '                End Try

    '                'codice sede
    '                Try
    '                    VAR_CODICESEDE = Trim(ArrCampi(15))
    '                Catch ex As Exception
    '                    VAR_CODICESEDE = ""
    '                End Try

    '                'codice sede primo giorno
    '                Try
    '                    VAR_CODICESEDEPRIMOGIORNO = Trim(ArrCampi(16))
    '                Catch ex As Exception
    '                    VAR_CODICESEDEPRIMOGIORNO = ""
    '                End Try
    '                'Idoneo
    '                Try
    '                    VAR_IDONEO = Trim(ArrCampi(17))
    '                Catch ex As Exception
    '                    VAR_IDONEO = ""
    '                End Try

    '                'selezionato
    '                Try
    '                    VAR_SELEZIONATO = Trim(ArrCampi(18))
    '                Catch ex As Exception

    '                End Try
    '                'Punteggio
    '                Try
    '                    VAR_PUNTEGGIO = Trim(ArrCampi(19))
    '                Catch ex As Exception
    '                    VAR_PUNTEGGIO = ""
    '                End Try

    '                'Tipo Posto
    '                Try
    '                    VAR_TIPOPOSTO = Trim(ArrCampi(20))
    '                Catch ex As Exception
    '                    VAR_TIPOPOSTO = ""
    '                End Try

    '                'subentro stesso progetto
    '                Try
    '                    VAR_SUBENTROSTESSOPROGETTO = Trim(ArrCampi(21))
    '                Catch ex As Exception
    '                    VAR_SUBENTROSTESSOPROGETTO = ""
    '                End Try

    '                'subentro altri progetti
    '                Try
    '                    VAR_SUBENTROALTRIPROGETTI = Trim(ArrCampi(22))
    '                Catch ex As Exception
    '                    VAR_SUBENTROALTRIPROGETTI = ""
    '                End Try

    '                'telefono
    '                Try
    '                    VAR_TELEFONO = Trim(ArrCampi(23))
    '                Catch ex As Exception
    '                    VAR_TELEFONO = ""
    '                End Try

    '                'titolo di studio
    '                Try
    '                    VAR_TITOLODISTUDIO = Trim(ArrCampi(24))
    '                Catch ex As Exception
    '                    VAR_TITOLODISTUDIO = ""
    '                End Try

    '                'conseguimento titolo di studio
    '                Try
    '                    VAR_CONSEGUIMENTOTITOLODISTUDIO = Trim(ArrCampi(25))
    '                Catch ex As Exception
    '                    VAR_CONSEGUIMENTOTITOLODISTUDIO = ""
    '                End Try

    '                'email
    '                Try
    '                    VAR_EMAIL = Trim(ArrCampi(26))
    '                Catch ex As Exception
    '                    VAR_EMAIL = ""
    '                End Try

    '                'codice istat comune domicilio
    '                Try
    '                    VAR_CODICEISTATCOMUNEDOMICILIO = Trim(ArrCampi(27))
    '                Catch ex As Exception
    '                    VAR_CODICEISTATCOMUNEDOMICILIO = ""
    '                End Try

    '                'indirizzo domicilio
    '                Try
    '                    VAR_INDIRIZZODOMICILIO = Trim(ArrCampi(28))
    '                Catch ex As Exception
    '                    VAR_INDIRIZZODOMICILIO = ""
    '                End Try

    '                'numero civoco domicilio
    '                Try
    '                    VAR_NUMEROCIVICODOMICILIO = Trim(ArrCampi(29))
    '                Catch ex As Exception
    '                    VAR_NUMEROCIVICODOMICILIO = ""
    '                End Try

    '                'cap domicilio
    '                Try
    '                    VAR_CAPDOMICILIO = Trim(ArrCampi(30))
    '                Catch ex As Exception
    '                    VAR_CAPDOMICILIO = ""
    '                End Try

    '                'dettaglio recapito residenza
    '                Try
    '                    VAR_DETTAGLIORECAPITORESIDENZA = Trim(ArrCampi(31))
    '                Catch ex As Exception
    '                    VAR_DETTAGLIORECAPITORESIDENZA = ""
    '                End Try

    '                'dettaglio recapito domicilio
    '                Try
    '                    VAR_DETTAGLIORECAPITODOMICILIO = Trim(ArrCampi(32))
    '                Catch ex As Exception
    '                    VAR_DETTAGLIORECAPITODOMICILIO = ""
    '                End Try

    '                'stato civile
    '                Try
    '                    VAR_STATOCIVILE = Trim(ArrCampi(33))
    '                Catch ex As Exception
    '                    VAR_STATOCIVILE = ""
    '                End Try

    '                'codice fiscale coniuge
    '                Try
    '                    VAR_CODICEFISCALECONIUGE = Trim(ArrCampi(34))
    '                Catch ex As Exception
    '                    VAR_CODICEFISCALECONIUGE = ""
    '                End Try

    '                '----------------------------FINE  ASSEGNAZIONE VARIABILI  --------------ADC------------------





    '                'Cognome    
    '                If VAR_COGNOME = vbNullString Then
    '                    strNote = strNote & "Il campo Cognome e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If Len(VAR_COGNOME) > 100 Then
    '                        strNote = strNote & "Il campo Cognome puo' contenere massimo 100 caratteri."
    '                        swErr = True
    '                    End If
    '                End If
    '                'Nome
    '                If VAR_NOME = vbNullString Then
    '                    strNote = strNote & "Il campo Nome e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If Len(VAR_NOME) > 100 Then
    '                        strNote = strNote & "Il campo Nome puo' contenere massimo 100 caratteri."
    '                        swErr = True
    '                    End If
    '                End If

    '                'DataNascita
    '                If VAR_DATANASCITA = vbNullString Then
    '                    strNote = strNote & "Il campo DataNascita e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If IsDate(VAR_DATANASCITA) = False Then
    '                        strNote = strNote & "Il campo DataNascita non e' nel formato corretto."
    '                        swErr = True
    '                    Else
    '                        If Len(VAR_DATANASCITA) = 8 Then
    '                            strNote = strNote & "Il campo DataNascita non e' nel formato corretto."
    '                            swErr = True
    '                            'ArrCampi(3) = Trim(ArrCampi(3))
    '                            'ArrCampi(3) = Left(ArrCampi(3), 6) & "19" & Right(ArrCampi(3), 2)
    '                        End If
    '                    End If
    '                End If

    '                'Sesso
    '                If VAR_SESSO = vbNullString Then
    '                    strNote = strNote & "Il campo Sesso e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If UCase(VAR_SESSO) <> "M" And UCase(VAR_SESSO) <> "F" Then
    '                        strNote = strNote & "Il campo Sesso non e' nel formato corretto."
    '                        swErr = True
    '                    End If
    '                End If



    '                If VAR_CODICEFISCALE = vbNullString Then
    '                    strNote = strNote & "Il campo CodiceFiscale e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If Not VAR_DATANASCITA = vbNullString Then
    '                        'se la lunghezza del CF è diversa da 16 è sicuramente errato
    '                        If Len(Replace(VAR_CODICEFISCALE, " ", "")) <> 16 Then
    '                            strNote = strNote & "CodiceFiscale non corretto."
    '                            swErr = True
    '                        Else
    '                            'ComuneNascita
    '                            If VAR_CODICEISTATCOMUNENASCITA = vbNullString Then
    '                                strNote = strNote & "Il campo CodiceISTATComuneNascita e' un campo obbligatorio."
    '                                swErr = True
    '                            Else
    '                                VerificaComuneNascita(VAR_CODICEISTATCOMUNENASCITA, strIdComune, blnCheckComune)

    '                                If blnCheckComune = False Then
    '                                    strNote = strNote & "Il CodiceISTATComuneNascita inserito non è un codice valido."
    '                                    swErr = True
    '                                Else    'SE VALIDO VERIFICO IL CF
    '                                    'ricavo il codice catastale del comune
    '                                    strCodCatasto = ClsUtility.GetCodiceCatasto(Session("conn"), strIdComune)
    '                                    'genero il CF con i dati anagrafici del volontario
    '                                    strNewCF = ClsUtility.CreaCF(VAR_COGNOME, VAR_NOME, VAR_DATANASCITA, strCodCatasto, VAR_SESSO)
    '                                    'lo confronto con il CF inserito nel CSV
    '                                    If UCase(Trim(Replace(VAR_CODICEFISCALE, " ", ""))) <> UCase(strNewCF) Then
    '                                        'verifo eventuale OMOCODIA
    '                                        If ClsUtility.VerificaOmocodia(UCase(strNewCF), UCase(Trim(Replace(VAR_CODICEFISCALE, " ", "")))) = False Then
    '                                            strNote = strNote & "CodiceFiscale non congruente con i dati inseriti."
    '                                            swErr = True
    '                                        End If
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                    'If UCase(Trim(ArrCampi(6))) = "ITA" Then
    '                    '    If Not Trim(ArrCampi(3)) = vbNullString Then
    '                    '        'se la lunghezza del CF è diversa da 16 è sicuramente errato
    '                    '        If Len(Replace(ArrCampi(2), " ", "")) <> 16 Then
    '                    '            strNote = strNote & "CodiceFiscale non corretto."
    '                    '            swErr = True
    '                    '        Else
    '                    '            'ComuneNascita
    '                    '            If Trim(ArrCampi(5)) = vbNullString Then
    '                    '                strNote = strNote & "Il campo CodiceISTATComuneNascita e' un campo obbligatorio."
    '                    '                swErr = True
    '                    '            Else
    '                    '                VerificaComune(Trim(ArrCampi(5)), strIdComune, blnCheckComune)

    '                    '                If blnCheckComune = False Then
    '                    '                    strNote = strNote & "Il CodiceISTATComuneNascita inserito non è un codice valido."
    '                    '                    swErr = True
    '                    '                Else    'SE VALIDO VERIFICO IL CF
    '                    '                    'ricavo il codice catastale del comune
    '                    '                    strCodCatasto = ClsUtility.GetCodiceCatasto(Session("conn"), strIdComune)
    '                    '                    'genero il CF con i dati anagrafici del volontario
    '                    '                    strNewCF = ClsUtility.CreaCF(Trim(ArrCampi(0)), Trim(ArrCampi(1)), Trim(ArrCampi(3)), strCodCatasto, Trim(ArrCampi(4)))
    '                    '                    'lo confronto con il CF inserito nel CSV
    '                    '                    If UCase(Trim(Replace(ArrCampi(2), " ", ""))) <> UCase(strNewCF) Then
    '                    '                        'verifo eventuale OMOCODIA
    '                    '                        If ClsUtility.VerificaOmocodia(UCase(strNewCF), UCase(Trim(Replace(ArrCampi(2), " ", "")))) = False Then
    '                    '                            strNote = strNote & "CodiceFiscale non congruente con i dati inseriti."
    '                    '                            swErr = True
    '                    '                        End If
    '                    '                    End If
    '                    '                End If
    '                    '            End If
    '                    '        End If
    '                    '    End If
    '                    'Else


    '                    '    If Not Trim(ArrCampi(3)) = vbNullString Then
    '                    '        'se la lunghezza del CF è diversa da 16 è sicuramente errato
    '                    '        If Len(Replace(ArrCampi(2), " ", "")) > 16 Then
    '                    '            strNote = strNote & "Lunghezza del CodiceFiscale superiore ai 16 caratteri."
    '                    '            swErr = True
    '                    '        Else
    '                    '            'ComuneNascita
    '                    '            If Trim(ArrCampi(5)) = vbNullString Then
    '                    '                strNote = strNote & "Il campo CodiceISTATComuneNascita e' un campo obbligatorio."
    '                    '                swErr = True
    '                    '            Else
    '                    '                VerificaComune(Trim(ArrCampi(5)), strIdComune, blnCheckComune)

    '                    '                If blnCheckComune = False Then
    '                    '                    strNote = strNote & "Il CodiceISTATComuneNascita inserito non è un codice valido."
    '                    '                    swErr = True
    '                    '                Else    'SE VALIDO VERIFICO IL CF
    '                    '                    'ricavo il codice catastale del comune
    '                    '                    strCodCatasto = ClsUtility.GetCodiceCatasto(Session("conn"), strIdComune)
    '                    '                    'genero il CF con i dati anagrafici del volontario
    '                    '                    strNewCF = ClsUtility.CreaCF(Trim(ArrCampi(0)), Trim(ArrCampi(1)), Trim(ArrCampi(3)), strCodCatasto, Trim(ArrCampi(4)))
    '                    '                    'lo confronto con il CF inserito nel CSV
    '                    '                    If UCase(Trim(Replace(ArrCampi(2), " ", ""))) <> UCase(strNewCF) Then
    '                    '                        'verifo eventuale OMOCODIA
    '                    '                        If ClsUtility.VerificaOmocodia(UCase(strNewCF), UCase(Trim(Replace(ArrCampi(2), " ", "")))) = False Then
    '                    '                            blnFlagCF = 1
    '                    '                        End If
    '                    '                    End If
    '                    '                End If
    '                    '            End If
    '                    '        End If
    '                    '    End If





    '                    '    If Len(Replace(ArrCampi(2), " ", "")) > 16 Then

    '                    '        strNote = strNote & "Lunghezza del CodiceFiscale superiore ai 16 caratteri."
    '                    '        swErr = True

    '                    '    End If


    '                    'End If
    '                End If


    '                mycommand = New SqlClient.SqlCommand
    '                mycommand.Connection = Session("conn")
    '                strsqlvol = "select codicefiscale from #TEMP_GRADUATORIA_VOLONTARIGG where codicefiscale='" & VAR_CODICEFISCALE & "' "
    '                mycommand.CommandText = strsqlvol
    '                dtrcontrollocodfis = mycommand.ExecuteReader

    '                If dtrcontrollocodfis.HasRows = True Then

    '                    strNote = strNote & "CodiceFiscale e' gia' presente nel file CSV."
    '                    swErr = True

    '                End If

    '                If Not dtrcontrollocodfis Is Nothing Then
    '                    dtrcontrollocodfis.Close()
    '                    dtrcontrollocodfis = Nothing
    '                End If
    '                'controllo categoria
    '                If VAR_CATEGORIA <> vbNullString Then
    '                    '    strNote = strNote & "Il campo Categoria e' un campo obbligatorio."
    '                    '    swErr = True
    '                    'Else

    '                    If VerificaCategoria(VAR_CATEGORIA) = False Then

    '                        strNote = strNote & "Il campo Categoria non è tra quelli codificati."
    '                        swErr = True

    '                    End If
    '                End If
    '                'Nazionalita
    '                'If UCase(Trim(ArrCampi(6))) <> "ITA" Then
    '                If VAR_NAZIONALITA = vbNullString Then
    '                    strNote = strNote & "Il campo Nazionalità e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    'vado a controllare l'esistenza del comune
    '                    'e nella funzione 

    '                    VerificaNazionalità(VAR_NAZIONALITA, strIdComune, blnCheckComune)

    '                    If blnCheckComune = False Then
    '                        strNote = strNote & "Il Campo Nazionalità inserito non è un codice valido."
    '                        swErr = True
    '                    End If
    '                End If
    '                'End If

    '                'datadomanda
    '                If VAR_DATADOMANDA = vbNullString Then
    '                    strNote = strNote & "Il campo DataDomanda e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If IsDate(VAR_DATADOMANDA) = False Then
    '                        strNote = strNote & "Il campo DataDomanda non e' nel formato corretto."
    '                        swErr = True
    '                    Else
    '                        If CDate(VAR_DATADOMANDA) > CDate(Now) Then
    '                            strNote = strNote & "La DataDomanda deve essere antecedente alla data odierna."
    '                            swErr = True
    '                        Else
    '                            If Len(VAR_DATADOMANDA) = 8 Then
    '                                strNote = strNote & "Il campo DataDomanda non e' nel formato corretto."
    '                                swErr = True
    '                                'ArrCampi(8) = Trim(ArrCampi(8))
    '                                'ArrCampi(8) = Left(ArrCampi(8), 6) & "19" & Right(ArrCampi(8), 2)
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '                'ComuneResidenza
    '                If VAR_CODICEISTATCOMUNERESIDENZA = vbNullString Then
    '                    strNote = strNote & "Il campo CodiceISTATComuneResidenza e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    'vado a controllare l'esistenza del comune
    '                    'e nella funzione 

    '                    VerificaComune(VAR_CODICEISTATCOMUNERESIDENZA, strIdComune, blnCheckComune)

    '                    If blnCheckComune = False Then
    '                        strNote = strNote & "Il CodiceISTATComuneResidenza inserito non è un codice valido."
    '                        swErr = True
    '                    Else
    '                        'se il comune esiste vado a controllare se il CAP inserito è
    '                        'un multi cap, quindi controllo se la terza cifra corrisponde
    '                        'If ClsUtility.ControllaMultiCAP(Session("conn"), Trim(ArrCampi(9)), strIdComune) = False Then
    '                        '    strNote = strNote & "Il CAP non risulta essere congruo con il comune."
    '                        '    swErr = True
    '                        'End If

    '                        '***************************CONTROLLO ESISTENZA CAP******************************
    '                        'If ClsUtility.ControllaEsistenzaCap(Session("conn"), Trim(ArrCampi(9)), strIdComune) = False Then
    '                        '    strNote = strNote & "Il CAP non risulta essere congruo con il comune inserito."
    '                        '    swErr = True
    '                        'End If
    '                        If ClsUtility.CAP_VERIFICA_VOLONTARI(Session("conn"), strNoteCap, True, VAR_CAP, strIdComune, "", "", VAR_INDIRIZZO, VAR_NUMEROCIVICO) = False Then
    '                            strNote = strNote & " " & strNoteCap & "(RESIDENZA) "
    '                            swErr = True
    '                        End If
    '                    End If
    '                End If

    '                'Indirizzo
    '                If VAR_INDIRIZZO = vbNullString Then
    '                    strNote = strNote & "Il campo Indirizzo e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If Len(VAR_INDIRIZZO) > 200 Then
    '                        strNote = strNote & "Il campo Indirizzo puo' contenere massimo 200 caratteri."
    '                        swErr = True
    '                    End If
    '                    'CONTROLLO ESISTENZA INDIRIZZO
    '                    'CONTROLLO AGGIUNTO DA THE GREAT SOUTHERN JONKILL
    '                    '26 marzo 2009
    '                    'VADO A PRENDERE LE INFORMAZIONI DEL COMUNE CHE DEVO CONTROLLARE (ID)
    '                    Dim informazioniComune As New clsInformazioniComune(Session("conn"), , VAR_CODICEISTATCOMUNERESIDENZA, )
    '                    Dim informazioniComunePerID As New clsInformazioniComune(Session("conn"), , , informazioniComune.getIdComune_byCodiceIstat)
    '                    'CONTROLLO SE HO UN IDCOMUNE CON INDIRIZZI, SE SI VADO A CONTROLLARE SE L'INDIRIZZO ESISTE
    '                    If informazioniComune.getIdComune_byCodiceIstat > 0 Then
    '                        'vado a prendere le informazioni dell'indirizzo inserito
    '                        ' '' ''Dim chkIndirizzo As New clsIndirizzi(ArrCampi(10), informazioniComune.getIdComune_byCodiceIstat, Session("conn"))
    '                        '' '' ''se il comune ha gli indirizzi vado a controllare se l'indirizzo inserito esiste
    '                        ' '' ''If informazioniComunePerID.checkEsistenzaIndirizzi = True Then
    '                        ' '' ''    'se l'indirizzo inserito non esiste informo l'utente
    '                        ' '' ''    If chkIndirizzo.CheckIndirizzo = False Then
    '                        ' '' ''        If vSegnalazioneIndirizzoResidenza(0) = "" Then
    '                        ' '' ''            vSegnalazioneIndirizzoResidenza(0) = "L'indirizzo di residenza non &egrave; valido: <a  id=" & """" & "suggerimento" & """" & "  rel=" & """" & "external" & """" & " style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('" & informazioniComune.getIdComune_byCodiceIstat & "','" & Replace(ArrCampi(10), "'", "|") & "')" & """" & "><strong>SUGGERIMENTO</strong></a> ."
    '                        ' '' ''        Else
    '                        ' '' ''            ReDim Preserve vSegnalazioneIndirizzoResidenza(UBound(vSegnalazioneIndirizzoResidenza) + 1)
    '                        ' '' ''            vSegnalazioneIndirizzoResidenza(UBound(vSegnalazioneIndirizzoResidenza)) = "L'indirizzo di residenza non &egrave; valido: <a  id=" & """" & "suggerimento" & """" & "    rel=" & """" & "external" & """" & " style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('" & informazioniComune.getIdComune_byCodiceIstat & "','" & Replace(ArrCampi(10), "'", "|") & "')" & """" & "><strong>SUGGERIMENTO</strong></a> ."
    '                        ' '' ''        End If
    '                        ' '' ''        strNote = strNote & "L'indirizzo di residenza non e' valido."
    '                        ' '' ''        swErr = True
    '                        ' '' ''    End If
    '                        ' '' ''End If

    '                    End If
    '                End If

    '                'NumeroCivico
    '                If VAR_NUMEROCIVICO = vbNullString Then
    '                    strNote = strNote & "Il campo NumeroCivico e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If Len(VAR_NUMEROCIVICO) > 10 Then
    '                        strNote = strNote & "Il campo NumeroCivico puo' contenere massimo 10 caratteri."
    '                        swErr = True
    '                    End If
    '                End If

    '                'CAP
    '                If VAR_CAP = vbNullString Then
    '                    strNote = strNote & "Il campo CAP e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If Len(VAR_CAP) > 8 Then
    '                        strNote = strNote & "Il campo CAP puo' contenere massimo 8 caratteri."
    '                        swErr = True
    '                    End If
    '                End If

    '                'CodiceProgetto
    '                If VAR_CODICEPROGETTO = vbNullString Then
    '                    strNote = strNote & "Il campo CodiceProgetto e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If VerificaProgetto(VAR_CODICEPROGETTO, PrevedeEstero, SoloItalia) = False Then
    '                        strNote = strNote & "Il CodiceProgetto indicato non e' tra i progetti in attesa di graduatoria."
    '                        swErr = True
    '                    Else

    '                        'cotrollo aggiunto da simona cordella il 30/04/2007
    '                        'controllo se la graduatoria rientra nei termini previsti 
    '                        'dalla datascadenzagradautoria definita nel bando
    '                        If VerificaDataScadenzaGraduatoria(VAR_CODICEPROGETTO) = False Then
    '                            strNote = strNote & "Sono scaduti i termini per il caricamento della graduatoria."
    '                            swErr = True
    '                        End If
    '                        'CodiceSede
    '                        If VAR_CODICESEDE = vbNullString Then
    '                            strNote = strNote & "Il campo CodiceSede e' un campo obbligatorio."
    '                            swErr = True
    '                        Else
    '                            If IsNumeric(VAR_CODICESEDE) = False Then
    '                                strNote = strNote & "Il campo CodiceSede non e' nel formato corretto."
    '                                swErr = True
    '                            Else
    '                                'Verifico che la sede di attuazione sia attiva e legata all'ente
    '                                'If VerificaSede(Trim(ArrCampi(12))) = False Then
    '                                '    strNote = strNote & "La Sede indicata non e' tra le sedi attive dell'Ente."
    '                                '    swErr = True
    '                                'Else
    '                                'Verifico che la sede di attuazione sia legata al progetto
    '                                If VerificaAttivitaEnteSede(VAR_CODICESEDE, VAR_CODICEPROGETTO, "ITALIA") = False Then
    '                                    strNote = strNote & "La Sede indicata non e' tra le sedi del Progetto indicato."
    '                                    swErr = True
    '                                Else
    '                                    'Controllo Presenza numero domande presentate per la sede
    '                                    'Luigi Leucci 15/10/2018
    '                                    If SoloItalia Then
    '                                        If VerificaVolontariPresentati(VAR_CODICESEDE) = False Then
    '                                            strNote = strNote & "Per la sede di progetto non e' stato indicato il numero di volontari che hanno presentato domanda."
    '                                            swErr = True
    '                                        End If
    '                                    End If

    '                                    'Controllo che su quella sede non sia gia stata eseguito il caricamento di una graduatoria
    '                                    If VerificaEsistenzaGraduatoria(VAR_CODICESEDE, VAR_CODICEPROGETTO) = False Then
    '                                        strNote = strNote & "Per la Sede e il progetto indicati, e' gia stata caricata una graduatoria."
    '                                        swErr = True
    '                                    End If
    '                                End If
    '                                'End If
    '                            End If
    '                        End If
    '                    End If
    '                End If

    '                'CodiceSedePrimoGiorno
    '                If UCase(VAR_CODICESEDEPRIMOGIORNO) <> vbNullString Then
    '                    If IsNumeric(VAR_CODICESEDEPRIMOGIORNO) = False Then
    '                        strNote = strNote & "Il campo CodiceSedePrimoGiorno non e' nel formato corretto."
    '                        swErr = True
    '                    Else
    '                        If VerificaSedePrimo(VAR_CODICESEDEPRIMOGIORNO, VAR_CODICEPROGETTO) = False Then
    '                            strNote = strNote & "La Sede di prima convocazione indicata non e' tra le sedi attive dell'Ente."
    '                            swErr = True
    '                        End If
    '                    End If
    '                Else
    '                    strNote = strNote & "Il campo CodiceSedePrimoGiorno e' un campo obbligatorio."
    '                End If

    '                'DataInizioPrevista 
    '                If VAR_DATAINIZIOPREVISTA = vbNullString Then
    '                    strNote = strNote & "Il campo Data Inizio Prevista e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If IsDate(VAR_DATAINIZIOPREVISTA) = False Then
    '                        strNote = strNote & "La Data Inizio Prevista non e' nel formato corretto."
    '                        swErr = True
    '                    Else
    '                        If CDate(VAR_DATAINIZIOPREVISTA) < CDate(Now) Then
    '                            strNote = strNote & "La Data Inizio Prevista deve essere successiva alla data odierna."
    '                            swErr = True
    '                        Else
    '                            If Len(VAR_DATAINIZIOPREVISTA) = 8 Then
    '                                strNote = strNote & "La Data Inizio Prevista non e' nel formato corretto."
    '                                swErr = True
    '                                'ArrCampi(14) = Trim(ArrCampi(14))
    '                                'ArrCampi(14) = Left(ArrCampi(14), 6) & "20" & Right(ArrCampi(14), 2)
    '                            End If
    '                        End If
    '                    End If
    '                End If

    '                'Idoneo
    '                If UCase(VAR_IDONEO) <> "SI" And UCase(VAR_IDONEO) <> "NO" Then
    '                    strNote = strNote & "Il campo Idoneo non e' nel formato corretto."
    '                    swErr = True
    '                End If

    '                'Slezionato
    '                If UCase(VAR_SELEZIONATO) <> "SI" And UCase(VAR_SELEZIONATO) <> "NO" Then
    '                    strNote = strNote & "Il campo Selezionato non e' nel formato corretto."
    '                    swErr = True
    '                Else
    '                    If UCase(VAR_SELEZIONATO) = "SI" And UCase(VAR_IDONEO) = "NO" Then
    '                        strNote = strNote & "Il Volontario per essere Selezionato deve essere Idoneo."
    '                        swErr = True
    '                    End If
    '                End If

    '                'Punteggio
    '                If VAR_PUNTEGGIO = vbNullString Then
    '                    strNote = strNote & "Il campo Punteggio e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If IsNumeric(VAR_PUNTEGGIO) = False Then
    '                        strNote = strNote & "Il campo Punteggio non e' nel formato corretto."
    '                        swErr = True
    '                    End If
    '                End If

    '                'TipoPosto
    '                If VAR_TIPOPOSTO = vbNullString Then
    '                    strNote = strNote & "Il campo TipoPosto e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If IsNumeric(VAR_TIPOPOSTO) = False Then
    '                        strNote = strNote & "Il campo TipoPosto non e' nel formato corretto."
    '                        swErr = True
    '                    Else
    '                        If VerificaTipoPosto(VAR_TIPOPOSTO) = False Then
    '                            strNote = strNote & "Il TipoPosto indicato non e' presente nella base dati."
    '                            swErr = True
    '                        End If
    '                    End If
    '                End If

    '                'Subentro stesso progetto
    '                If UCase(VAR_SUBENTROSTESSOPROGETTO) <> "SI" And UCase(VAR_SUBENTROSTESSOPROGETTO) <> "NO" Then
    '                    strNote = strNote & "Il campo SubentroStessoProgetto non e' nel formato corretto."
    '                    swErr = True
    '                End If

    '                'Subentro altro progetto
    '                If UCase(VAR_SUBENTROALTRIPROGETTI) <> "SI" And UCase(VAR_SUBENTROALTRIPROGETTI) <> "NO" Then
    '                    strNote = strNote & "Il campo SubentroAltriProgetti non e' nel formato corretto."
    '                    swErr = True
    '                End If

    '                'Telefono
    '                If UCase(VAR_TELEFONO) = vbNullString Then
    '                    strNote = strNote & "Il campo Telefono e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If IsNumeric(VAR_TELEFONO) = False Then
    '                        strNote = strNote & "Nel campo Telefono devono comparire solo valori numerici."
    '                        swErr = True
    '                    End If
    '                End If
    '                If CInt(Len(VAR_TELEFONO)) < 6 Then
    '                    strNote = strNote & "La lunghezza del campo telefono risulta troppo corta."
    '                    swErr = True
    '                End If
    '                'Controllo se il numero dei posti per sede di attuazione in base alla tipologia siano congruenti
    '                If UCase(VAR_IDONEO) = "SI" And UCase(VAR_SELEZIONATO) = "SI" Then
    '                    If VerificaNumeroDiPosti(VAR_CODICESEDE, VAR_CODICEPROGETTO, VAR_TIPOPOSTO) = False Then
    '                        strNote = strNote & "E' stato superato il numero di volontari previsti per il progetto,la sede e la tipologia di posto indicati."
    '                        swErr = True
    '                    End If
    '                End If

    '                'titolodistudio
    '                If VAR_TITOLODISTUDIO = vbNullString Then
    '                    strNote = strNote & "Il campo TitoloDiStudio e' un campo obbligatorio."
    '                    swErr = True
    '                Else
    '                    If IsNumeric(VAR_TITOLODISTUDIO) = False Then
    '                        strNote = strNote & "Il campo TitoloDiStudio non e' nel formato corretto."
    '                        swErr = True
    '                    Else
    '                        If VerificaTitoloStudio(VAR_TITOLODISTUDIO) = False Then
    '                            strNote = strNote & "Il campo TitoloDiStudio non è tra quelli codificati."
    '                            swErr = True
    '                        End If
    '                    End If
    '                End If

    '                'Controllo ConseguimentoTitolodiStudio
    '                If VAR_CONSEGUIMENTOTITOLODISTUDIO = vbNullString Then
    '                    strNote = strNote & "Il campo ConseguimentoTitoloDiStudio e' un campo obbligatorio."
    '                    swErr = True
    '                Else

    '                    If VerificaConseguimentoTitolo(VAR_CONSEGUIMENTOTITOLODISTUDIO) = False Then

    '                        strNote = strNote & "Il campo ConseguimentoTitoloDiStudio non è tra quelli codificati."
    '                        swErr = True

    '                    End If
    '                End If
    '                '05/06/2007 DA SIMONA CORDELLA
    '                'CONTROLLO IL FORMATO DELL'EMAIL

    '                ' If UBound(ArrCampi) >= 26 Then 'Antonello correzione controlli
    '                If VAR_EMAIL <> vbNullString Then
    '                    If CheckMail(VAR_EMAIL) = False Then
    '                        strNote = strNote & "L' Email inserita non è nel formato valido."
    '                        swErr = True
    '                    End If
    '                End If
    '                'controllidomicilio
    '                ' If UBound(ArrCampi) = 32 Then
    '                If VAR_CODICEISTATCOMUNEDOMICILIO <> vbNullString And VAR_INDIRIZZODOMICILIO <> vbNullString And VAR_NUMEROCIVICODOMICILIO <> vbNullString And VAR_CAPDOMICILIO <> vbNullString Then
    '                    blnControlloDom = True
    '                Else
    '                    blnControlloDom = False
    '                End If


    '                'CONTROLLO CODICE ISTAT DEL COMUNE DI DOMICILIO
    '                If VAR_CODICEISTATCOMUNEDOMICILIO <> vbNullString Then
    '                    If blnControlloDom = False Then
    '                        strNote = strNote & "Le informazioni relative al domicilio non sono complete."
    '                        swErr = True
    '                        blnControlloDom = True
    '                    Else
    '                        VerificaComune(VAR_CODICEISTATCOMUNEDOMICILIO, strIdComune, blnCheckComune)

    '                        If blnCheckComune = False Then
    '                            strNote = strNote & "Il CodiceISTATComuneDomicilio inserito non è un codice valido."
    '                            swErr = True
    '                        End If
    '                        If VAR_CAPDOMICILIO <> vbNullString Then
    '                            '***************************CONTROLLO ESISTENZA CAP******************************
    '                            'If ClsUtility.ControllaEsistenzaCap(Session("conn"), Trim(ArrCampi(26)), strIdComune) = False Then
    '                            '    strNote = strNote & "Il CAP non risulta essere congruo con il comune inserito."
    '                            '    swErr = True
    '                            'End If
    '                            If ClsUtility.CAP_VERIFICA_VOLONTARI(Session("conn"), strNoteCap, True, VAR_CAPDOMICILIO, strIdComune, "", "", VAR_INDIRIZZODOMICILIO, VAR_NUMEROCIVICODOMICILIO) = False Then
    '                                strNote = strNote & " " & strNoteCap & "(DOMICILIO) "
    '                                swErr = True
    '                            End If
    '                        End If
    '                    End If

    '                End If
    '                'indirizzodomicilio
    '                If VAR_INDIRIZZODOMICILIO <> vbNullString Then
    '                    If blnControlloDom = False Then
    '                        strNote = strNote & "Le informazioni relative al domicilio non sono complete."
    '                        swErr = True
    '                        blnControlloDom = True
    '                    End If
    '                    'CONTROLLO ESISTENZA INDIRIZZO
    '                    'CONTROLLO AGGIUNTO DA THE GREAT SOUTHERN JONKILL
    '                    '27 marzo 2009
    '                    'vado a prendere le informazioni del comune che si sta tentando di inserire
    '                    Dim informazioniComune As New clsInformazioniComune(Session("conn"), , VAR_CODICEISTATCOMUNEDOMICILIO, )
    '                    Dim informazioniComunePerID As New clsInformazioniComune(Session("conn"), , , informazioniComune.getIdComune_byCodiceIstat)
    '                    'controllo se l'indirizzo è valido, quindi ho un idcomune con indirizzi
    '                    If informazioniComune.getIdComune_byCodiceIstat > 0 Then
    '                        'dichiaro la classe contenente le informazioni dell'indirizzo
    '                        'Dim chkIndirizzo As New clsIndirizzi(ArrCampi(28), informazioniComune.getIdComune_byCodiceIstat, Session("conn"))
    '                        ''se il comune ha gli indirizzi vado a controllare se l'indirizzo esiste
    '                        'If informazioniComunePerID.checkEsistenzaIndirizzi = True Then
    '                        '    'se l'indirizzo che si sta inserendo non esiste mando un messaggio all'utente
    '                        '    If chkIndirizzo.CheckIndirizzo = False Then
    '                        '        If vSegnalazioneIndirizzoDomicilio(0) = "" Then
    '                        '            vSegnalazioneIndirizzoDomicilio(0) = "L'indirizzo del domicilio non &egrave; valido: <a style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('" & informazioniComune.getIdComune_byCodiceIstat & "','" & Replace(ArrCampi(28), "'", "|") & "')" & """" & "><font color=" & """" & "blue" & """" & "><b>SUGGERIMENTO</b></font></a> ."
    '                        '        Else
    '                        '            ReDim Preserve vSegnalazioneIndirizzoDomicilio(UBound(vSegnalazioneIndirizzoDomicilio) + 1)
    '                        '            vSegnalazioneIndirizzoDomicilio(UBound(vSegnalazioneIndirizzoDomicilio)) = "L'indirizzo del domicilio non &egrave; valido: <a style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('" & informazioniComune.getIdComune_byCodiceIstat & "','" & Replace(ArrCampi(28), "'", "|") & "')" & """" & "><font color=" & """" & "blue" & """" & "><b>SUGGERIMENTO</b></font></a> ."
    '                        '        End If
    '                        '        strNote = strNote & "L'indirizzo del domicilio non e' valido."
    '                        '        swErr = True
    '                        '    End If
    '                        'End If
    '                    End If


    '                End If

    '                'NumeroCivicoDomicilio
    '                If VAR_NUMEROCIVICODOMICILIO <> vbNullString Then
    '                    If blnControlloDom = False Then
    '                        strNote = strNote & "Le informazioni relative al domicilio non sono complete."
    '                        swErr = True
    '                        blnControlloDom = True
    '                    Else
    '                        If Len(VAR_NUMEROCIVICODOMICILIO) > 10 Then
    '                            strNote = strNote & "Il campo NumeroCivicoDomicilio puo' contenere massimo 10 caratteri."
    '                            swErr = True
    '                        End If
    '                    End If
    '                End If

    '                'CAP domicilio
    '                If VAR_CAPDOMICILIO <> vbNullString Then
    '                    If blnControlloDom = False Then
    '                        strNote = strNote & "Le informazioni relative al domicilio non sono complete."
    '                        swErr = True
    '                        blnControlloDom = True
    '                    Else
    '                        If Len(VAR_CAPDOMICILIO) > 8 Then
    '                            strNote = strNote & "Il campo CAPDomicilio puo' contenere massimo 8 caratteri."
    '                            swErr = True
    '                        End If
    '                    End If
    '                End If

    '                'DettaglioRecapitoRes
    '                If VAR_DETTAGLIORECAPITORESIDENZA <> vbNullString Then
    '                    'If blnControlloDom = False Then
    '                    '    strNote = strNote & "Le informazioni relative al domicilio non sono complete."
    '                    '    swErr = True
    '                    '    blnControlloDom = True
    '                    'Else
    '                    If Len(VAR_DETTAGLIORECAPITORESIDENZA) > 20 Then
    '                        strNote = strNote & "Il campo DettaglioRecapitoRes puo' contenere massimo 20 caratteri."
    '                        swErr = True
    '                    End If
    '                    'End If
    '                End If

    '                'DettaglioRecapitoCom
    '                If VAR_DETTAGLIORECAPITODOMICILIO <> vbNullString Then
    '                    'If blnControlloDom = False Then
    '                    '    strNote = strNote & "Le informazioni relative al domicilio non sono complete."
    '                    '    swErr = True
    '                    '    blnControlloDom = True
    '                    'Else
    '                    If Len(VAR_DETTAGLIORECAPITODOMICILIO) > 20 Then
    '                        strNote = strNote & "Il campo DettaglioRecapitoCom puo' contenere massimo 20 caratteri."
    '                        swErr = True
    '                    End If
    '                    'End If
    '                End If


    '                'ANTONELLO

    '                If VAR_STATOCIVILE = "" Then
    '                    strNote = strNote & "Il campo StatoCivile e' un campo obbligatorio."
    '                    swErr = True
    '                Else

    '                    If VerificaStatoCivile(VAR_STATOCIVILE) = False Then

    '                        strNote = strNote & "Il Codice dello StatoCivile e' errato."
    '                        swErr = True

    '                    Else

    '                        If VerificaOblligatorietàCodiceFiscaleConiuge(VAR_STATOCIVILE) = True Then


    '                            If VAR_CODICEFISCALECONIUGE <> "" Then

    '                                If VAR_CODICEFISCALE = VAR_CODICEFISCALECONIUGE Then
    '                                    strNote = strNote & "Il Codice Fiscale Coniuge e' uguale al codicefiscale del volontario "
    '                                    swErr = True

    '                                Else

    '                                    If ClsUtility.VerificaValiditàCodiceFiscaleConiuge(VAR_CODICEFISCALECONIUGE) = False Then
    '                                        strNote = strNote & "Il Codice Fiscale Coniuge e' ERRATO "
    '                                        swErr = True
    '                                    End If

    '                                End If

    '                            Else

    '                                strNote = strNote & "Il campo CodiceFiscaleConiuge e' un campo obbligatorio."
    '                                swErr = True
    '                            End If

    '                        Else

    '                            If VAR_CODICEFISCALECONIUGE <> "" Then

    '                                strNote = strNote & "Per lo stato civile indicato il campo CodiceFiscaleConiuge deve essere vuoto."
    '                                swErr = True
    '                            End If

    '                        End If

    '                    End If

    '                End If


    '                'End If
    '                'End If

    '                'swErr = False
    '                'strNote = ""
    '                If swErr = False Then
    '                    ScriviTabTempGG(ArrCampi, blnFlagCF)
    '                Else
    '                    TotKo = TotKo + 1
    '                End If
    '            End If
    '        End If
    '        Writer.WriteLine(strNote & ";" & xLinea)
    '        'Writer.WriteLine(Replace(Replace(Replace(strNote, "<a style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('", ""), "')" & """" & "><font color=" & """" & "blue" & """" & "><b>SUGGERIMENTO</b></font></a> .", ""), "|", "'") & ";" & xLinea)
    '        strNote = vbNullString
    '        xLinea = Reader.ReadLine()
    '    End While

    '    'passo gli array che contentgono le segnalazioni 
    '    'sull'indirizzo del domicilio ad una sessione, 
    '    'così da controllare nella visualizzazione delle note
    '    'gli eventuali suggerimenti
    '    If vSegnalazioneIndirizzoDomicilio(0) <> "" Then
    '        Session("ArraySegnalazioneIndirizzoDomicilio") = vSegnalazioneIndirizzoDomicilio
    '    End If

    '    'passo gli array che contentgono le segnalazioni 
    '    'sull'indirizzo di residenza ad una sessione, 
    '    'così da controllare nella visualizzazione delle note
    '    'gli eventuali suggerimenti
    '    If vSegnalazioneIndirizzoResidenza(0) <> "" Then
    '        Session("ArraySegnalazioneIndirizzoResidenza") = vSegnalazioneIndirizzoResidenza
    '    End If

    '    Reader.Close()
    '    Writer.Close()

    '    Response.Redirect("WfrmRisultatoImportGraduatoriaVolontari.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
    '    '--- reindirizzo la pagina sottostante
    '    'Response.Write("<script>" & vbCrLf)
    '    'Response.Write("parent.Naviga('WfrmRisultatoImportGraduatoriaVolontari.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
    '    'Response.Write("</script>")
    'End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub

    Public Sub CaricaTitoli()

        'DESCRIZIONE: caricamento dei Titoli Di Studio codificati in tabella
        'DATA: 14/06/2006

        Dim strSQL As String
        Dim dsTitoliStudio As SqlDataReader

        strSQL = "Select TitoloStudio, Codice From TitoliStudio Order by Codice"
        dsTitoliStudio = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsTitoliStudio.Read
            Response.Write("<li><strong>" & dsTitoliStudio.Item("Codice") & "</strong> per <strong>" & dsTitoliStudio.Item("TitoloStudio") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsTitoliStudio.Close()
        dsTitoliStudio = Nothing
    End Sub

    'Public Sub CaricaTipiStatoCivile()

    '    'DESCRIZIONE: caricamento dei Titoli Di Studio codificati in tabella
    '    'DATA: 14/06/2006

    '    Dim strSQL As String
    '    Dim dsTipiStatoCivile As SqlDataReader

    '    strSQL = "Select StatoCivile, Codice From TipiStatoCivile Order by IdTipoStatoCivile"
    '    dsTipiStatoCivile = ClsServer.CreaDatareader(strSQL, Session("conn"))
    '    Response.Write("<fieldset>")
    '    Response.Write("<ul>")
    '    Do While dsTipiStatoCivile.Read
    '        Response.Write("<li><strong>" & dsTipiStatoCivile.Item("Codice") & "</strong> per <strong>" & dsTipiStatoCivile.Item("StatoCivile") & "</strong></li>")
    '    Loop
    '    Response.Write("</ul>")
    '    Response.Write("</fieldset>")
    '    dsTipiStatoCivile.Close()
    '    dsTipiStatoCivile = Nothing
    'End Sub

    Public Sub CaricaCategoria()
        'AUTORE: ANTONELLO DI CROCE
        'DESCRIZIONE: caricamento CategorieEntità codificati in tabella
        'DATA: 10/12/2013

        Dim strSQL As String
        Dim dsCategoria As SqlDataReader

        strSQL = "Select Categoria, Codice From CategorieEntità where attiva = 1 Order by IdCategoriaEntità"
        dsCategoria = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsCategoria.Read
            Response.Write("<li><strong>" & dsCategoria.Item("Codice") & "</strong> per <strong>" & dsCategoria.Item("Categoria") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsCategoria.Close()
        dsCategoria = Nothing
    End Sub

    Public Sub CaricaConseguimento()
        'AUTORE: ANTONELLO DI CROCE
        'DESCRIZIONE: caricamento dei Titoli Studio Conseguimento codificati in tabella
        'DATA: 10/12/2013

        Dim strSQL As String
        Dim dsConseguimento As SqlDataReader

        strSQL = "Select TitoloStudioConseguimento, Codice From TitoliStudioConseguimento Order by IdTitoloStudioConseguimento"
        dsConseguimento = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsConseguimento.Read
            Response.Write("<li><strong>" & dsConseguimento.Item("Codice") & "</strong> per <strong>" & dsConseguimento.Item("TitoloStudioConseguimento") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsConseguimento.Close()
        dsConseguimento = Nothing
    End Sub

    Public Sub CaricaGMO()

        'DESCRIZIONE: caricamento ParticolaritàEntità -  GMO codificati in tabella
        'DATA: 10/07/2018

        Dim strSQL As String
        Dim dsParticolarità As SqlDataReader

        strSQL = "Select Descrizione, Codice From ParticolaritàEntità where Macrotipo = 'GMO' Order by IDParticolarità"
        dsParticolarità = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsParticolarità.Read
            Response.Write("<li><strong>" & dsParticolarità.Item("Codice") & "</strong> per <strong>" & dsParticolarità.Item("Descrizione") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsParticolarità.Close()
        dsParticolarità = Nothing
    End Sub

    Public Sub CaricaFAMI()

        'DESCRIZIONE: caricamento ParticolaritàEntità - FAMI codificati in tabella
        'DATA: 10/07/2018

        Dim strSQL As String
        Dim dsParticolarità As SqlDataReader

        strSQL = "Select Descrizione, Codice From ParticolaritàEntità where Macrotipo = 'FAMI' Order by IDParticolarità"
        dsParticolarità = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsParticolarità.Read
            Response.Write("<li><strong>" & dsParticolarità.Item("Codice") & "</strong> per <strong>" & dsParticolarità.Item("Descrizione") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsParticolarità.Close()
        dsParticolarità = Nothing
    End Sub

    'OK 1
    Private Function CreaArray(ByVal pLinea As String, ByVal Intestazione As Integer) As String()
        Dim TmpArr As String()
        Dim DefArr As String()
        Dim i As Integer
        Dim x As Integer

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
                DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
                i = x
            Else
                DefArr(UBound(DefArr)) = TmpArr(i)
            End If
        Next
        If Intestazione = 0 Then
            If UBound(TmpArr) >= 9 And UBound(TmpArr) < 10 Then
                ReDim Preserve DefArr(UBound(DefArr) + (10 - UBound(TmpArr)))
            End If
        End If

        'If UBound(TmpArr) = 22 Then
        '    ReDim Preserve DefArr(UBound(DefArr) + 6)
        'End If
        'If UBound(TmpArr) = 26 Then
        '    ReDim Preserve DefArr(UBound(DefArr) + 2)
        'End If
        CreaArray = DefArr

    End Function

    'Private Function CreaArrayGG(ByVal pLinea As String, ByVal Intestazione As Integer) As String()
    '    Dim TmpArr As String()
    '    Dim DefArr As String()
    '    Dim i As Integer
    '    Dim x As Integer

    '    TmpArr = Split(pLinea, ";")

    '    For i = 0 To UBound(TmpArr)
    '        If i = 0 Then
    '            ReDim DefArr(0)
    '        Else
    '            ReDim Preserve DefArr(UBound(DefArr) + 1)
    '        End If
    '        If Left(TmpArr(i), 1) = Chr(34) Then
    '            x = i
    '            Do While Right(TmpArr(x), 1) <> Chr(34)
    '                If x = i Then
    '                    DefArr(UBound(DefArr)) = Mid(TmpArr(x), 2) & "; "
    '                Else
    '                    DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & TmpArr(x) & "; "
    '                End If
    '                x = x + 1
    '            Loop
    '            DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
    '            i = x
    '        Else
    '            DefArr(UBound(DefArr)) = TmpArr(i)
    '        End If
    '    Next
    '    If Intestazione = 0 Then
    '        If UBound(TmpArr) >= 33 And UBound(TmpArr) < 34 Then
    '            ReDim Preserve DefArr(UBound(DefArr) + (34 - UBound(TmpArr)))
    '        End If
    '    End If
    '    CreaArrayGG = DefArr
    'End Function

    Private Sub ScriviTabTemp(ByVal pArray() As String, ByVal blnFlagCF As Integer)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String
        Dim NazioneBase As String

        Try

            NazioneBase = NazioneBaseProgetto(UCase(Trim(ClsServer.NoApice(VAR_CODICEPROGETTO))))

            strsql = "INSERT INTO #TEMP_GRADUATORIA_VOLONTARI " & _
                     "(Nome, " & _
                     "Cognome, " & _
                     "CodiceFiscale, " & _
                     "CodiceProgetto, " & _
                     "CodiceSede, " & _
                     "EsitoSelezione, " & _
                     "Punteggio, " & _
                     "TipoPosto, " & _
                     "CodiceSedePrimoGiorno, " & _
                     "CodiceSedeSecondaria, " & _
                     "DataInizioPrevista) " & _
                     "values " & _
                     "('" & Trim(ClsServer.NoApice(VAR_NOME)) & "', " & _
                     "'" & Trim(ClsServer.NoApice(VAR_COGNOME)) & "', " & _
                     "'" & UCase(Trim(ClsServer.NoApice(VAR_CODICEFISCALE))) & "', " & _
                     "'" & UCase(Trim(ClsServer.NoApice(VAR_CODICEPROGETTO))) & "', " & _
                     "" & Trim(ClsServer.NoApice(VAR_CODICESEDE)) & ", " & _
                     "'" & Trim(ClsServer.NoApice(VAR_ESITOSELEZIONE)) & "', " & _
                     "" & Replace(VAR_PUNTEGGIO, ",", ".") & ", " & _
                     "" & VAR_TIPOPOSTO & ", " & _
                    "" & IIf(VAR_CODICESEDEPRIMOGIORNO = "", "NULL", Trim(ClsServer.NoApice(VAR_CODICESEDEPRIMOGIORNO))) & ", " & _
                     "" & IIf(VAR_CODICESEDESECONDARIA = "", "NULL", Trim(ClsServer.NoApice(VAR_CODICESEDESECONDARIA))) & ", " & _
                     "'" & Trim(ClsServer.NoApice(VAR_DATAINIZIOPREVISTA)) & "' )"
            '"'" & VAR_STATOCIVILE & "', " & _
            '"'" & UCase(Trim(ClsServer.NoApice(VAR_CODICEFISCALECONIUGE))) & "', " & _


            cmdTemp = New SqlClient.SqlCommand
            cmdTemp.CommandText = strsql 'insert
            cmdTemp.Connection = Session("conn")
            cmdTemp.ExecuteNonQuery()
            cmdTemp.Dispose()
            TotOk = TotOk + 1

        Catch exc As Exception
            strNote = "Errore generico."
            TotKo = TotKo + 1

        End Try
    End Sub
    'OK 1
    'Private Sub ScriviTabTempGG(ByVal pArray() As String, ByVal blnFlagCF As Integer)
    '    '--- scrive nella tab temporanea
    '    Dim cmdTemp As SqlClient.SqlCommand
    '    Dim strsql As String


    '    Try
    '        strsql = "INSERT INTO #TEMP_GRADUATORIA_VOLONTARIGG " & _
    '                 "(Cognome, " & _
    '                 "Nome, " & _
    '                 "CodiceFiscale, " & _
    '                 "DataNascita, " & _
    '                 "Sesso, " & _
    '                 "CodiceISTATComuneNascita, " & _
    '                 "Categoria, " & _
    '                 "Nazionalita, " & _
    '                 "DataDomanda, " & _
    '                 "CodiceISTATComuneResidenza, " & _
    '                 "Indirizzo, " & _
    '                 "NumeroCivico, " & _
    '                 "CAP, " & _
    '                 "CodiceProgetto, " & _
    '                 "DataInizioPrevista, " & _
    '                 "CodiceSede, " & _
    '                 "CodiceSedePrimoGiorno, " & _
    '                 "Idoneo, " & _
    '                 "Selezionato, " & _
    '                 "Punteggio, " & _
    '                 "TipoPosto, " & _
    '                 "SubentroStessoProgetto, " & _
    '                 "SubentroAltriProgetti," & _
    '                 "Telefono, " & _
    '                 "TitoloStudio, " & _
    '                 "ConseguimentoTitoloStudio, " & _
    '                 "Email, " & _
    '                 "CodiceISTATComuneDomicilio, " & _
    '                 "IndirizzoDomicilio, " & _
    '                 "NumeroCivicoDomicilio, " & _
    '                 "CAPDomicilio, " & _
    '                 "DettaglioRecapitoRes, " & _
    '                 "DettaglioRecapitoDom, " & _
    '                 "FlagIndirizzoValidoRes, " & _
    '                 "FlagIndirizzoValidoDom, " & _
    '                 "AnomaliaCF, " & _
    '                 "IDStatiVerificaCFEntità, " & _
    '                 "CodiceStatoCivile, " & _
    '                 "CodiceFiscaleConiuge " & _
    '                 ") " & _
    '                 "values " & _
    '                 "('" & Trim(ClsServer.NoApice(VAR_COGNOME)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_NOME)) & "', " & _
    '                 "'" & UCase(Trim(ClsServer.NoApice(VAR_CODICEFISCALE))) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_DATANASCITA)) & "', "
    '        If UCase(VAR_SESSO) = "F" Then
    '            strsql = strsql & " 1, "
    '        Else
    '            strsql = strsql & " 0, "
    '        End If
    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_CODICEISTATCOMUNENASCITA)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_CATEGORIA)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_NAZIONALITA)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_DATADOMANDA)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_CODICEISTATCOMUNERESIDENZA)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_INDIRIZZO)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_NUMEROCIVICO)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_CAP)) & "', " & _
    '                 "'" & UCase(Trim(ClsServer.NoApice(VAR_CODICEPROGETTO))) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_DATAINIZIOPREVISTA)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_CODICESEDE)) & "', " & _
    '                 "'" & Trim(ClsServer.NoApice(VAR_CODICESEDEPRIMOGIORNO)) & "', "
    '        If UCase(VAR_IDONEO) = "SI" Then
    '            strsql = strsql & " 1, "
    '        Else
    '            strsql = strsql & " 0, "
    '        End If
    '        If UCase(VAR_SELEZIONATO) = "SI" Then
    '            strsql = strsql & " 1, "
    '        Else
    '            strsql = strsql & " 0, "
    '        End If
    '        strsql = strsql & Replace(VAR_PUNTEGGIO, ",", ".") & ", " & _
    '        VAR_TIPOPOSTO & ", "
    '        If UCase(VAR_SUBENTROSTESSOPROGETTO) = "SI" Then
    '            strsql = strsql & " 1, "
    '        Else
    '            strsql = strsql & " 0, "
    '        End If
    '        If UCase(VAR_SUBENTROALTRIPROGETTI) = "SI" Then
    '            strsql = strsql & " 1,"
    '        Else
    '            strsql = strsql & " 0,"
    '        End If
    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_TELEFONO)) & "','" & GetDescTitoloStudio(VAR_TITOLODISTUDIO) & "',"

    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_CONSEGUIMENTOTITOLODISTUDIO)) & "'," 'Conseguimento titolo di studio

    '        'Antonello correzione controlli

    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_EMAIL)) & "'," 'EMAIL


    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_CODICEISTATCOMUNEDOMICILIO)) & "'," 'CODICEISTATCOMUNEDOMICILIO
    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_INDIRIZZODOMICILIO)) & "'," 'INDIRIZZODOMICILIO
    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_NUMEROCIVICODOMICILIO)) & "'," 'NUMEROCIVICODOMICILIO
    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_CAPDOMICILIO)) & "',"  'CAPDOMICILIO

    '        'MODIFICA AGGIUNTA DA WHAT'S THE FREQUENCY JENNEDY
    '        '27 MARZO 2009
    '        'AGGIUNTI CAMPI DETTAGLIO SU INDIRIZZI
    '        'SE IL COMUNE INSERITO NON HA INDIRIZZI NELLA VISTA
    '        'METTO IL FLAG RElATIVO AL DETTAGLIO A 1
    '        'SE IL COMUNE INSERITO HA INDIRIZZI NELLA VISTA
    '        'METTO IL FLAG RElATIVO AL DETTAGLIO A 0
    '        'Dim chkFlagResidenza As New clsControllaFlagIndirizzo(Trim(pArray(6)), Session("conn"))
    '        'Dim chkFlagDomicilio As New clsControllaFlagIndirizzo(Trim(pArray(23)), Session("conn"))
    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_DETTAGLIORECAPITORESIDENZA)) & "',"  'DettaglioRecapitoRes
    '        strsql = strsql & "'" & Trim(ClsServer.NoApice(VAR_DETTAGLIORECAPITODOMICILIO)) & "',"  'DettaglioRecapitoDom
    '        Dim blnCHeckResidenza As Boolean
    '        ClsUtility.CAP_VERIFICA_VOLONTARI(Session("conn"), "", blnCHeckResidenza, Trim(ClsServer.NoApice(pArray(12))), "0", "", Trim(ClsServer.NoApice(pArray(9))), Trim(ClsServer.NoApice(pArray(10))), Trim(ClsServer.NoApice(pArray(11))))
    '        strsql = strsql & IIf(blnCHeckResidenza = False, 0, 1) & "," 'FlagIndirizzoValidoRes
    '        Dim blnCHeckDomicilio As Boolean
    '        ClsUtility.CAP_VERIFICA_VOLONTARI(Session("conn"), "", blnCHeckDomicilio, Trim(ClsServer.NoApice(pArray(30))), "0", "", Trim(ClsServer.NoApice(pArray(27))), Trim(ClsServer.NoApice(pArray(28))), Trim(ClsServer.NoApice(pArray(29))))
    '        strsql = strsql & IIf(blnCHeckDomicilio = False, 0, 1) 'FlagIndirizzoValidoDom

    '        ' -- Modificato da Luigi Leucci il 05/03/2019
    '        If blnFlagCF = 1 Then strsql = strsql & ", 1, 1" Else strsql = strsql & ", 0, Null"
    '        'strsql = strsql & "," & blnFlagCF
    '        ' --

    '        strsql = strsql & ",'" & VAR_STATOCIVILE & "'"
    '        strsql = strsql & ",'" & UCase(Trim(ClsServer.NoApice(VAR_CODICEFISCALECONIUGE))) & "'"
    '        strsql = strsql & ")"

    '        cmdTemp = New SqlClient.SqlCommand
    '        cmdTemp.CommandText = strsql 'insert
    '        cmdTemp.Connection = Session("conn")
    '        cmdTemp.ExecuteNonQuery()
    '        cmdTemp.Dispose()
    '        TotOk = TotOk + 1

    '    Catch exc As Exception
    '        strNote = "Errore generico."
    '        TotKo = TotKo + 1

    '    End Try
    'End Sub

    'OK 1
    Private Function CongruenzaCodiceFiscale(ByVal pCodiceFiscale As String, ByVal pCognome As String, ByVal pNome As String, ByVal pDataNascita As String, ByVal pSesso As String) As Boolean
        '--- verifica la coerenza tra codfis e cognome, nome, data nascita

        Dim TutteLeLettere As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim TuttiINumeri As String = "0123456789"
        Dim TuttiGliOmocodici As String = "LMNPQRSTUV"
        Dim TutteLeVocali As String = "AEIOU"
        Dim TutteLeConsonanti As String = "BCDFGHJKLMNPQRSTVWXYZ"
        Dim CodMese As String = "ABCDEHLMPRST"
        Dim swErr As Boolean = False
        Dim Vocali As String
        Dim Consonanti As String
        Dim xCodCognome As String
        Dim xCodNome As String
        Dim tmpGiornoNascitaM As Integer
        Dim tmpGiornoNascitaF As Integer
        Dim tmpValore As String
        Dim i As Integer
        CongruenzaCodiceFiscale = True
        pCodiceFiscale = UCase(pCodiceFiscale)
        If Len(pCodiceFiscale) <> 16 Then
            CongruenzaCodiceFiscale = False
            Exit Function
        End If

        '--- cognome e nome stringa
        For i = 1 To 6
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                'CongruenzaCodiceFiscale = "La sezione Cognome Nome non è nel formato corretto."
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Next i

        '--- anno numerico
        For i = 7 To 8
            If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
                If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
                    'CongruenzaCodiceFiscale = "La sezione Anno non è nel formato corretto."
                    CongruenzaCodiceFiscale = False
                    Exit Function
                End If
            End If
        Next i

        '--- mese stringa
        For i = 9 To 9
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                'CongruenzaCodiceFiscale = "La sezione Mese non è nel formato corretto."
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Next i

        '--- giorno numerico
        For i = 10 To 11
            If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
                If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
                    'CongruenzaCodiceFiscale = "La sezione Giorno non è nel formato corretto."
                    CongruenzaCodiceFiscale = False
                    Exit Function
                End If
            End If
        Next i

        '--- primo carattere comune stringa
        For i = 12 To 12
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                'CongruenzaCodiceFiscale = "La sezione Comune non è nel formato corretto."
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Next i

        '--- 3 caratteri comune numerico
        For i = 13 To 15
            If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
                If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
                    'CongruenzaCodiceFiscale = "La sezione Comune  non è nel formato corretto."
                    CongruenzaCodiceFiscale = False
                    Exit Function
                End If
            End If
        Next i

        '--- ultimo carattere di controllo stringa
        For i = 16 To 16
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                'CongruenzaCodiceFiscale = "La sezione Carattere di Controllo non è nel formato corretto."
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Next i

        '--- FINE CONTROLLO FORMALE
        '--- Controllo Cognome
        If pCognome = vbNullString Then

        End If
        pCognome = UCase(pCognome)
        For i = 1 To Len(pCognome)
            If InStr(TutteLeVocali, Mid(pCognome, i, 1)) > 0 Then
                Vocali = Vocali + Mid(pCognome, i, 1)
            Else
                If InStr(TutteLeConsonanti, Mid(pCognome, i, 1)) > 0 Then
                    Consonanti = Consonanti + Mid(pCognome, i, 1)
                End If
            End If
            If Len(Consonanti) = 3 Then
                Exit For
            End If
        Next i
        If Len(Consonanti) < 3 Then
            Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
            For i = Len(Consonanti) + 1 To 3
                Consonanti = Consonanti & "X"
            Next i
        End If
        xCodCognome = Consonanti

        If xCodCognome <> Mid(pCodiceFiscale, 1, 3) Then
            'errore sul cognome
            'CongruenzaCodiceFiscale = "La sezione Cognome non è congruente."
            CongruenzaCodiceFiscale = False
            Exit Function
        End If

        '--- Controllo Nome
        Consonanti = vbNullString
        Vocali = vbNullString
        pNome = UCase(pNome)
        For i = 1 To Len(pNome)
            If InStr(TutteLeVocali, Mid(pNome, i, 1)) > 0 Then
                Vocali = Vocali + Mid(pNome, i, 1)
            Else
                If InStr(TutteLeConsonanti, Mid(pNome, i, 1)) > 0 Then
                    Consonanti = Consonanti + Mid(pNome, i, 1)
                End If
            End If
        Next i

        If Len(Consonanti) >= 4 Then
            Consonanti = Mid(Consonanti, 1, 1) + Mid(Consonanti, 3, 2)
        Else
            If Len(Consonanti) < 3 Then
                Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
                For i = Len(Consonanti) + 1 To 3
                    Consonanti = Consonanti & "X"
                Next i
            End If
        End If
        xCodNome = Consonanti

        If xCodNome <> Mid(pCodiceFiscale, 4, 3) Then
            'CongruenzaCodiceFiscale = "La sezione Nome non è congruente."
            CongruenzaCodiceFiscale = False
            Exit Function
        End If

        '--- Controllo Anno	
        tmpValore = DecodificaOmocodici(Mid(pCodiceFiscale, 7, 1)) & DecodificaOmocodici(Mid(pCodiceFiscale, 8, 1))
        If IsNumeric(tmpValore) = True Then
            If tmpValore <> Mid(pDataNascita, 9, 2) Then
                'CongruenzaCodiceFiscale = "La sezione Anno non è congruente."
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Else
            CongruenzaCodiceFiscale = False
            Exit Function
        End If
        '--- Controllo Mese				
        If Mid(pCodiceFiscale, 9, 1) <> Mid(CodMese, Mid(pDataNascita, 4, 2), 1) Then
            'CongruenzaCodiceFiscale = "La sezione Mese non è congruente."
            CongruenzaCodiceFiscale = False
            Exit Function
        End If


        '--- Controllo Giorno
        tmpGiornoNascitaF = Mid(pDataNascita, 1, 2) + 40
        tmpGiornoNascitaM = Mid(pDataNascita, 1, 2)

        tmpValore = DecodificaOmocodici(Mid(pCodiceFiscale, 10, 1)) + DecodificaOmocodici(Mid(pCodiceFiscale, 11, 1))
        If UCase(Trim(pSesso)) = "F" Then
            If IsNumeric(tmpValore) = True Then
                If CInt(tmpValore) <> tmpGiornoNascitaF Then
                    'CongruenzaCodiceFiscale = "La sezione Giorno non è congruente."
                    CongruenzaCodiceFiscale = False
                    Exit Function
                End If
            Else
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Else
            If IsNumeric(tmpValore) = True Then
                If CInt(tmpValore) <> tmpGiornoNascitaM Then
                    'CongruenzaCodiceFiscale = "La sezione Giorno non è congruente."
                    CongruenzaCodiceFiscale = False
                    Exit Function
                End If
            Else
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        End If

    End Function

    'OK 1
    Private Function DecodificaOmocodici(ByVal pValore As String) As String
        Dim TuttiGliOmocodici As String = "LMNPQRSTUV"

        If InStr(TuttiGliOmocodici, pValore) > 0 Then

            Select Case pValore
                Case Is = "L"
                    DecodificaOmocodici = "0"

                Case "M"
                    DecodificaOmocodici = "1"

                Case "N"
                    DecodificaOmocodici = "2"

                Case "P"
                    DecodificaOmocodici = "3"

                Case "Q"
                    DecodificaOmocodici = "4"

                Case "R"
                    DecodificaOmocodici = "5"

                Case "S"
                    DecodificaOmocodici = "6"

                Case "T"
                    DecodificaOmocodici = "7"

                Case "U"
                    DecodificaOmocodici = "8"

                Case "V"
                    DecodificaOmocodici = "9"

            End Select
        Else
            DecodificaOmocodici = pValore
        End If

    End Function

    'OK 1
    Sub VerificaComune(ByVal pCodiceISTAT As String, Optional ByRef strIdComune As String = "", Optional ByRef blnVerifica As Boolean = False)
        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "Select IdComune from Comuni " & _
        "where CodiceISTAT= '" & ClsServer.NoApice(pCodiceISTAT) & "'"
        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        blnVerifica = dtrComuni.HasRows
        If dtrComuni.HasRows = True Then
            dtrComuni.Read()
            strIdComune = dtrComuni("IdComune")
        Else
            strIdComune = ""
        End If

        dtrComuni.Close()
        dtrComuni = Nothing

    End Sub

    Sub VerificaComuneNascita(ByVal pCodiceISTAT As String, Optional ByRef strIdComune As String = "", Optional ByRef blnVerifica As Boolean = False)
        '***********************
        'CREATA DA SIMONA CORDELLA IL 10/03/2017
        'FUNZIONE CHE RENDE VISIBILI I COMUNI DISMESSI SOLAMENTE PER IL CARICAMENTO DEI COMUNI DI NASCITA
        '***********************

        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = " Select IdComune from Comuni " & _
                 " where (CodiceISTAT= '" & ClsServer.NoApice(pCodiceISTAT) & "' OR CodiceIstatDismesso='" & ClsServer.NoApice(pCodiceISTAT) & "')"
        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        blnVerifica = dtrComuni.HasRows
        If dtrComuni.HasRows = True Then
            dtrComuni.Read()
            strIdComune = dtrComuni("IdComune")
        Else
            strIdComune = ""
        End If

        dtrComuni.Close()
        dtrComuni = Nothing

    End Sub

    Sub VerificaNazionalità(ByVal pCodiceISTAT As String, Optional ByRef strIdComune As String = "", Optional ByRef blnVerifica As Boolean = False)
        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String

        If UCase(pCodiceISTAT) = "ITALIA" Then
            blnVerifica = True
            strIdComune = "38715"
        Else
            strSql = "Select IdComune from Comuni " & _
            "where CodiceISTAT= '" & ClsServer.NoApice(pCodiceISTAT) & "' and comunenazionale=0"
            dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
            blnVerifica = dtrComuni.HasRows
            If dtrComuni.HasRows = True Then
                dtrComuni.Read()
                strIdComune = dtrComuni("IdComune")
            Else
                strIdComune = ""
            End If

            dtrComuni.Close()
            dtrComuni = Nothing
        End If
    End Sub

    'OK 1
    Private Function VerificaProgetto(ByVal pCodiceProgetto As String, ByRef PrevedeEstero As String, ByRef SoloItalia As Boolean) As Boolean
        'mod. il 27/11/20104 da s.c. FiltroVisibilita
        Dim dtrProgetto As SqlClient.SqlDataReader
        Dim strSql As String
        xIdAttivita = 0
        PrevedeEstero = ""
        SoloItalia = True

        strSql = " SELECT Attività.IdAttività, TipiProgetto.NazioneBase, isnull(Attività.EsteroUE,0) as EsteroUE  " & _
                 " FROM Attività " & _
                 " INNER JOIN StatiAttività ON Attività.IdStatoAttività = StatiAttività.IdStatoAttività " & _
                 " INNER JOIN TipiProgetto ON TipiProgetto.IdTipoProgetto = Attività.IdTipoProgetto " & _
                 " WHERE Attività.CodiceEnte= '" & pCodiceProgetto & "' " & _
                 " AND Attività.IdEntePresentante = " & Session("IdEnte") & " " & _
                 " AND StatiAttività.Attiva = 1 " & _
                 " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "

        dtrProgetto = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrProgetto.HasRows = True Then
            dtrProgetto.Read()
            xIdAttivita = dtrProgetto(0)

            SoloItalia = dtrProgetto("NazioneBase")

            If (dtrProgetto("NazioneBase") = False Or dtrProgetto("EsteroUE") = True) Then
                PrevedeEstero = "SI"
            Else
                PrevedeEstero = "NO"
            End If
        End If
        VerificaProgetto = dtrProgetto.HasRows

        dtrProgetto.Close()
        dtrProgetto = Nothing

    End Function

    'OK 1
    Private Function VerificaVolontariPresentati(ByVal pCodiceSede As Integer) As Boolean
        Dim dtrProgetto As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT ISNULL(VolontariPresentati, -1) FROM attivitàentisediattuazione a " & _
                 "WHERE IDAttività = " & xIdAttivita & _
                 " AND IDEnteSedeAttuazione = " & pCodiceSede
        dtrProgetto = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrProgetto.HasRows = True Then
            dtrProgetto.Read()
            If dtrProgetto(0) < 0 Then
                VerificaVolontariPresentati = False
            Else
                VerificaVolontariPresentati = True
            End If
        Else
            VerificaVolontariPresentati = False
        End If

        dtrProgetto.Close()
        dtrProgetto = Nothing

    End Function

    Private Function VerificaProroga(ByVal pCodiceProgetto As String) As Boolean
        Dim dtrDataProroga As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT datediff(dd,isnull(attività.DataProrogaGraduatorie,dateadd(dd,-10,getdate())),getdate()) as DiffGGAttività, datediff(dd,isnull(enti.DataProrogaGraduatorie,dateadd(dd,-10,getdate())),getdate()) as DiffGGEnte " & _
              " From Attività INNER JOIN " & _
              " Enti ON Attività.IdEntePresentante = Enti.IDEnte " & _
              " Where CodiceEnte = '" & pCodiceProgetto & "'"

        dtrDataProroga = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrDataProroga.Read()

        If dtrDataProroga("DiffGGAttività") > 7 Then
            If dtrDataProroga("DiffGGEnte") > 7 Then
                VerificaProroga = False              'proroga 7 giorni valida
            Else
                VerificaProroga = True               'proroga scaduta
            End If
        Else
            VerificaProroga = True               'proroga scaduta
        End If

        dtrDataProroga.Close()
        dtrDataProroga = Nothing

        Return VerificaProroga


    End Function

    Private Function VerificaDataScadenzaGraduatoria(ByVal pCodiceProgetto As String) As Boolean
        Dim dtrDataScadenza As SqlClient.SqlDataReader
        Dim strSql As String

        If VerificaProroga(pCodiceProgetto) Then
            VerificaDataScadenzaGraduatoria = True
        Else
            strSql = " Select CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.DataScadenzaGraduatorie ELSE BANDORICORSI.DataScadenzaGraduatorie END AS DataScadenzaGraduatorie, "
            strSql = strSql & "GETDATE() AS DataOdierna "
            strSql = strSql & "FROM Bando "
            strSql = strSql & "INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando "
            strSql = strSql & "INNER JOIN attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività "
            strSql = strSql & "LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO "
            strSql = strSql & "WHERE Attività.CodiceEnte= '" & pCodiceProgetto & "' "
            strSql = strSql & "AND Attività.IdEntePresentante = " & Session("IdEnte")

            dtrDataScadenza = ClsServer.CreaDatareader(strSql, Session("conn"))

            If dtrDataScadenza.HasRows = True Then
                dtrDataScadenza.Read()
                If IsDBNull(dtrDataScadenza("DataScadenzaGraduatorie")) = True Then
                    VerificaDataScadenzaGraduatoria = True
                Else
                    If dtrDataScadenza("Dataodierna") > dtrDataScadenza("DataScadenzaGraduatorie") Then
                        VerificaDataScadenzaGraduatoria = False
                    Else
                        VerificaDataScadenzaGraduatoria = True
                    End If
                End If

            End If

            dtrDataScadenza.Close()
            dtrDataScadenza = Nothing
        End If
    End Function
    Private Function VerificaChiusuraBandoVolontari(ByVal pCodiceProgetto As String) As Boolean
        Dim dtrBandoScadenzaDomande As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT bando.DataFineVolontari, GETDATE() AS DataOdierna " & _
                     "FROM bando " & _
                     "INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando " & _
                     "INNER JOIN attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività " & _
                     "WHERE  attività.CodiceEnte = '" & pCodiceProgetto & "' "
        dtrBandoScadenzaDomande = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrBandoScadenzaDomande.HasRows = True Then
            dtrBandoScadenzaDomande.Read()
            If dtrBandoScadenzaDomande("DataOdierna") > dtrBandoScadenzaDomande("DataFineVolontari") Then
                VerificaChiusuraBandoVolontari = True
            Else
                VerificaChiusuraBandoVolontari = False
            End If
        Else
            VerificaChiusuraBandoVolontari = False
        End If

        dtrBandoScadenzaDomande.Close()
        dtrBandoScadenzaDomande = Nothing

    End Function
    'OK 1
    Private Function VerificaTipoPosto(ByVal pIdTipoPosto As String) As Boolean
        Dim dtrTipoPosto As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "Select IdTipologiaPosto from TipologiePosto " & _
        "where IdTipologiaPosto= " & pIdTipoPosto
        dtrTipoPosto = ClsServer.CreaDatareader(strSql, Session("conn"))
        VerificaTipoPosto = dtrTipoPosto.HasRows

        dtrTipoPosto.Close()
        dtrTipoPosto = Nothing

    End Function

    'OK 1
    Private Function VerificaSede(ByVal pCodiceSede As String) As Boolean
        '--- verifico l'esistenza della sede di attuazione per l'ente in sessione

        Dim dtrSede As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT EntiSediAttuazioni.IdEnteSedeAttuazione FROM EntiSediAttuazioni " & _
                 "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                 "INNER JOIN StatiEntiSedi ON EntiSediAttuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                 "WHERE EntiSediAttuazioni.IdEnteSedeAttuazione = " & pCodiceSede & " " & _
                 "AND EntiSedi.IdEnte = " & Session("IdEnte") & _
                 "AND  (StatiEntiSedi.Attiva = 1 or StatiEntiSedi.idstatoentesede=3) " & _
                 "UNION " & _
                 "SELECT entisediattuazioni.IdEnteSedeAttuazione  " & _
                 " from enti" & _
                 " inner join entisedi on(entisedi.idente=enti.idEnte) " & _
                 " inner join entisediattuazioni on entisedi.identesede=entisediattuazioni.identesede " & _
                 " inner join statientisedi b on(entisediattuazioni.idstatoentesede=b.idstatoEntesede) " & _
                 " inner join entirelazioni on (entirelazioni.identefiglio=enti.idente) " & _
                 " inner join associaentirelazionisediattuazioni on (entisediattuazioni.identesedeattuazione=associaentirelazionisediattuazioni.identesedeattuazione " & _
                 " and entirelazioni.identerelazione=associaentirelazionisediattuazioni.identerelazione)" & _
                 " where entirelazioni.identePadre=" & Session("idEnte") & " and (entirelazioni.datafinevalidità is  null or b.idstatoentesede=3) " & _
                 " and EntiSediAttuazioni.IdEnteSedeAttuazione = " & pCodiceSede & " " & _
                 " AND (b.Attiva = 1 or b.idstatoentesede=3)"


        dtrSede = ClsServer.CreaDatareader(strSql, Session("conn"))
        VerificaSede = dtrSede.HasRows

        dtrSede.Close()
        dtrSede = Nothing

    End Function

    Private Function VerificaSedePrimo(ByVal pCodiceSede As String, ByVal pCodiceProgetto As String) As Boolean
        '--- verifico l'esistenza della sede di attuazione per l'ente in sessione

        Dim dtrSede As SqlClient.SqlDataReader
        Dim strSql As String


        strSql = "select identesedeattuazione from entisediattuazioni a inner join enti b on a.identecapofila=b.idente inner join statientisedi c on a.idstatoentesede = c.idstatoentesede " & _
        " where b.idente=" & Session("idEnte") & _
        " and  (c.statoentesede='Accreditata' or c.statoentesede='Sospesa' ) " & _
        " and a.identesedeattuazione=" & pCodiceSede & " " & _
        " UNION " & _
        " select identesedeattuazione from attivitàentisediattuazione a inner join attività b on a.idattività = b.idattività where b.codiceente = '" & pCodiceProgetto & "' and a.identesedeattuazione=" & pCodiceSede


        dtrSede = ClsServer.CreaDatareader(strSql, Session("conn"))

        VerificaSedePrimo = dtrSede.HasRows

        dtrSede.Close()
        dtrSede = Nothing

    End Function

    Private Function VerificaTitoloStudio(ByVal IdTitoloStudio As Integer) As Boolean
        Dim dtrTitoloStudio As SqlClient.SqlDataReader
        Dim strSql As String
        Dim sEsito As Boolean

        strSql = "Select * From TitoliStudio Where Codice=" & IdTitoloStudio
        dtrTitoloStudio = ClsServer.CreaDatareader(strSql, Session("conn"))
        sEsito = dtrTitoloStudio.HasRows

        dtrTitoloStudio.Close()
        dtrTitoloStudio = Nothing

        Return sEsito

    End Function

    Private Function GetDescTitoloStudio(ByVal IdTitoloStudio As Integer) As String
        Dim dtrTitoloStudio As SqlClient.SqlDataReader
        Dim strSql As String
        Dim sEsito As String

        strSql = "Select TitoloStudio From TitoliStudio Where Codice=" & IdTitoloStudio
        dtrTitoloStudio = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrTitoloStudio.Read()
        If dtrTitoloStudio.HasRows = True Then
            sEsito = CType(dtrTitoloStudio.Item("TitoloStudio"), String).Replace("'", "''")
        End If

        dtrTitoloStudio.Close()
        dtrTitoloStudio = Nothing

        Return sEsito

    End Function

    'OK 1
    Private Function VerificaAttivitaEnteSede(ByVal pCodiceSede As String, ByVal pCodiceAttivita As String, ByVal Nazione As String) As Boolean
        Dim dtrAttivitaEnteSede As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = " SELECT IdAttivitàEnteSedeAttuazione "
        strSql &= " FROM AttivitàEntiSediAttuazione "
        strSql &= " INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività "
        strSql &= " INNER JOIN entisediattuazioni esa ON esa.IDEnteSedeAttuazione=AttivitàEntiSediAttuazione.IDEnteSedeAttuazione"
        strSql &= " INNER JOIN entisedi es ON es.IDEnteSede =esa.IDEnteSede"
        strSql &= " INNER JOIN Comuni C ON C.IDComune =es.IDComune "
        strSql &= " INNER JOIN TipiProgetto D ON Attività.IdTipoProgetto =D.IDTipoProgetto "
        strSql &= " WHERE Attività.CodiceEnte = '" & pCodiceAttivita & "' "
        strSql &= " AND AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = " & pCodiceSede
        strSql &= " AND C.ComuneNazionale <> D.NazioneBase "

        dtrAttivitaEnteSede = ClsServer.CreaDatareader(strSql, Session("conn"))

        'Se esiste ritorna True 
        VerificaAttivitaEnteSede = dtrAttivitaEnteSede.HasRows

        dtrAttivitaEnteSede.Close()
        dtrAttivitaEnteSede = Nothing
    End Function

    'OK 1
    Private Function VerificaEsistenzaGraduatoria(ByVal pCodiceSede As String, ByVal pCodiceAttivita As String) As Boolean
        'Verifico che non sia gia stata caricata una graduatoria per quella sede / attività
        Dim DtrControllo As System.Data.SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT AttivitàSediAssegnazione.IdEnteSede FROM AttivitàSediAssegnazione " & _
                 "INNER JOIN Attività ON AttivitàSediAssegnazione.IdAttività = Attività.IdAttività " & _
                 "INNER JOIN EntiSedi ON AttivitàSediAssegnazione.IdEnteSede = EntiSedi.IdEnteSede " & _
                 "INNER JOIN EntiSediAttuazioni ON EntiSedi.IdEnteSede = EntiSediAttuazioni.IdEnteSede " & _
                 "WHERE Attività.CodiceEnte = '" & pCodiceAttivita & "' " & _
                 "AND EntiSediAttuazioni.IdEnteSedeAttuazione = " & pCodiceSede & " " & _
                 "AND (AttivitàSediAssegnazione.StatoGraduatoria = 1 or (AttivitàSediAssegnazione.StatoGraduatoria = 3 and isnull(AttivitàSediAssegnazione.AmmessoRecupero,0) = 1)) "

        DtrControllo = ClsServer.CreaDatareader(strSql, Session("conn"))

        'Ritorna False (Errore) se Esiste --- True se non Esiste (Corretto)
        VerificaEsistenzaGraduatoria = DtrControllo.HasRows

        DtrControllo.Close()
        DtrControllo = Nothing
    End Function

    'OK 1
    Private Function VerificaNumeroDiPosti(ByVal pCodiceSede As String, ByVal pCodiceAttivita As String, ByVal pIdTipoPosto As String) As Boolean
        Dim bnlComuneNazionale As Boolean
        'Prelevo il numero di posti specificati per il tipo di posto selezionato
        Dim DtrPosti As System.Data.SqlClient.SqlDataReader
        Dim StrSql As String
        Dim IntPostiProgetto As Integer
        'Controllo che il codice sede e il tipo posto siano numerici
        If IsNumeric(pCodiceSede) = False Or IsNumeric(pIdTipoPosto) = False Then
            Return False
            Exit Function
        End If

        'controllo se la sede è in ITALIA o ESTERO
        StrSql = "Select C.ComuneNazionale   "
        StrSql &= " From entisediattuazioni esa "
        StrSql &= " INNER JOIN entisedi es ON es.IDEnteSede =esa.IDEnteSede"
        StrSql &= " INNER JOIN Comuni C ON C.IDComune =es.IDComune "
        StrSql &= " WHERE IdEnteSedeAttuazione = " & pCodiceSede
        DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        If DtrPosti.HasRows = True Then
            DtrPosti.Read()
            bnlComuneNazionale = DtrPosti.Item("ComuneNazionale") 'true italia / false estero
        End If
        DtrPosti.Close()
        DtrPosti = Nothing

        'Prelevo il numero di posti specificati per il tipo di posto selezionato
        Select Case pIdTipoPosto
            Case 1 'Vitto Alloggio
                StrSql = "Select ISNULL(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0) As Posti From Attività " & _
                         "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                         "Where Attività.CodiceEnte = '" & pCodiceAttivita & "' And AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = " & pCodiceSede
            Case 2 'Solo Vitto
                StrSql = "Select ISNULL(AttivitàEntiSediAttuazione.NumeroPostiVitto,0) As Posti From Attività " & _
                         "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                         "Where Attività.CodiceEnte = '" & pCodiceAttivita & "' And AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = " & pCodiceSede
            Case 3 'Senza Vitto e Alloggio
                StrSql = "Select ISNULL(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) As Posti From Attività " & _
                         "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                         "Where Attività.CodiceEnte = '" & pCodiceAttivita & "' And AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = " & pCodiceSede
            Case Else
                Return False
                Exit Function
        End Select
        DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        If DtrPosti.HasRows = True Then
            DtrPosti.Read()
            IntPostiProgetto = DtrPosti.Item("Posti")
        Else
            IntPostiProgetto = 0
        End If
        DtrPosti.Close()
        DtrPosti = Nothing

        'Controllo il numero di posti gia definiti nell'importazione
        Dim IntPostiImportazione
        'If Session("Sistema") = "Helios" Then
        StrSql = "Select Count(*) As Posti From #TEMP_GRADUATORIA_VOLONTARI a INNER JOIN TipiEsitiGraduatoriaEntità b on a.esitoselezione = b.codice Where CodiceProgetto = '" & pCodiceAttivita & "'  "
        StrSql &= " And a.TipoPosto = " & pIdTipoPosto & " And b.idoneo = 'SI' And b.Selezionato = 'SI'"
        'If bnlComuneNazionale = True Then 'sede italia
        StrSql &= " And a.CodiceSede = " & pCodiceSede & ""
        'Else 'sede estera
        '    StrSql &= " And a.CodiceSedeEstero = " & pCodiceSede & ""
        'End If

        'Else
        '    StrSql = "Select Count(*) As Posti From #TEMP_GRADUATORIA_VOLONTARIGG Where CodiceProgetto = '" & pCodiceAttivita & "' And CodiceSede = " & pCodiceSede & " And TipoPosto = " & pIdTipoPosto & " And Idoneo = 1 And Selezionato = 1"
        'End If
        'StrSql = "Select Count(*) As Posti From #TEMP_GRADUATORIA_VOLONTARI Where CodiceProgetto = '" & pCodiceAttivita & "' And CodiceSede = " & pCodiceSede & " And TipoPosto = " & pIdTipoPosto & " And Idoneo = 1 And Selezionato = 1"
        DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        DtrPosti.Read()
        IntPostiImportazione = DtrPosti.Item("Posti")
        DtrPosti.Close()
        DtrPosti = Nothing

        If bnlComuneNazionale = True Then 'sede italia
            'verifico eventuali posti già avviati
            StrSql = "Select Count(*) As Posti From entità Where tmpCodiceProgetto = '" & pCodiceAttivita & "' And tmpidsedeattuazione = " & pCodiceSede & " And idTipoPosto = " & pIdTipoPosto & " And idstatoentità =3"
            DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
            DtrPosti.Read()
            IntPostiImportazione = IntPostiImportazione + DtrPosti.Item("Posti")
            DtrPosti.Close()
            DtrPosti = Nothing
        End If

        If IntPostiProgetto > IntPostiImportazione Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function VerificaFAMIProgetto(ByVal pCodiceAttivita As String, ByVal pFami As String, ByRef messaggio As String) As Boolean
        Dim DtrPosti As System.Data.SqlClient.SqlDataReader
        Dim StrSql As String
        Dim blnFAMI As Boolean = False
        messaggio = ""
        StrSql = "Select Fami AS FAMI From Attività " & _
               " Where Attività.CodiceEnte = '" & pCodiceAttivita & "'"
        'verifico se il progetto prevedere i fami
        DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        If DtrPosti.HasRows = True Then
            DtrPosti.Read()
            blnFAMI = DtrPosti.Item("FAMI")
        End If
        DtrPosti.Close()

        If blnFAMI = False Then 'FAMI NO SUL PROGETTO
            If pFami <> "" Then
                messaggio = "Il progetto non provede volontari FAMI."
                Return True
            Else
                messaggio = ""
                Return True
            End If
        Else 'FAMI SI SUL PROGETTO
            Return False
        End If
    End Function


    Private Function VerificaNumeroPostiFAMI(ByVal pCodiceSede As String, ByVal pCodiceAttivita As String, ByVal pFami As String, ByRef messaggio As String) As Boolean

        ''Prelevo il numero di posti specificati per la sede
        Dim DtrPosti As System.Data.SqlClient.SqlDataReader
        Dim StrSql As String
        Dim StrSql1 As String
        Dim IntPostiProgettoNOFAMI As Integer
        Dim IntPostiFAMI As Integer = 0

        StrSql = "Select (ISNULL(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0) + ISNULL(AttivitàEntiSediAttuazione.NumeroPostiVitto,0) + ISNULL(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0)) As Posti ," & _
                 " ISNULL(AttivitàEntiSediAttuazione.NumeroPostiFami,0) as PostiFami " & _
                 " From Attività " & _
                " INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                " Where Attività.CodiceEnte = '" & pCodiceAttivita & "' And AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = " & pCodiceSede
        DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        If DtrPosti.HasRows = True Then
            DtrPosti.Read()
            IntPostiProgettoNOFAMI = DtrPosti.Item("Posti") - DtrPosti.Item("PostiFAMI")
            IntPostiFAMI = DtrPosti.Item("PostiFAMI")
        Else
            IntPostiProgettoNOFAMI = 0
        End If
        DtrPosti.Close()
        Dim IntPostiImportazioneNOFAMI As Integer
        Dim IntPostiImportazioneFAMI As Integer
        ' If IntPostiFAMI <> 0 Then
        'Controllo il numero di posti gia definiti nell'importazione
        If Session("Sistema") = "Helios" Then
            StrSql = "Select Count(*) As Posti From #TEMP_GRADUATORIA_VOLONTARI Where CodiceProgetto = '" & pCodiceAttivita & "' And CodiceSede = " & pCodiceSede & " And Idoneo = 1 And Selezionato = 1 AND ISNULL(LTRIM(RTRIM(FAMI)),'')='' "
            'faMi
            StrSql1 = "Select Count(*) As PostiFAMI From #TEMP_GRADUATORIA_VOLONTARI Where CodiceProgetto = '" & pCodiceAttivita & "' And CodiceSede = " & pCodiceSede & " And Idoneo = 1 And Selezionato = 1 AND ISNULL(LTRIM(RTRIM(FAMI)),'')<>'' "
        End If
        DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        DtrPosti.Read()
        IntPostiImportazioneNOFAMI = DtrPosti.Item("Posti")
        DtrPosti.Close()
        DtrPosti = Nothing

        DtrPosti = ClsServer.CreaDatareader(StrSql1, Session("Conn"))
        DtrPosti.Read()
        IntPostiImportazioneFAMI = DtrPosti.Item("PostiFAMI")
        DtrPosti.Close()
        DtrPosti = Nothing

        If pFami <> "" Then
            If IntPostiFAMI > IntPostiImportazioneFAMI Then
                messaggio = ""

            Else ''""
                messaggio = messaggio & "E' stato superato il numero di volontari FAMI previsti per la sede."
            End If
        Else
            If IntPostiProgettoNOFAMI > IntPostiImportazioneNOFAMI Then
                'OK
            Else
                If IntPostiFAMI <> 0 Then
                    messaggio = messaggio & "E' stato superato il numero di volontari non FAMI previsti per la sede."
                End If
            End If
        End If
        If messaggio = "" Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function VerificaGMOProgetto(ByVal pCodiceAttivita As String, ByVal pGMO As String, ByRef messaggio As String) As Boolean
        Dim DtrPosti As System.Data.SqlClient.SqlDataReader
        Dim StrSql As String
        Dim blnGMO As Boolean = False
        messaggio = ""

        Return True

        'tolto controllo in quanto è possibile che un GMO copra un posto non GMO

        'StrSql = "Select GiovaniMinoriOpportunità AS GMO From Attività " & _
        '       " Where Attività.CodiceEnte = '" & pCodiceAttivita & "'"
        ''verifico se il progetto prevedere i fami
        'DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        'If DtrPosti.HasRows = True Then
        '    DtrPosti.Read()
        '    blnGMO = DtrPosti.Item("GMO")
        'End If
        'DtrPosti.Close()

        'If blnGMO = False Then 'GMO NO SUL PROGETTO
        '    If pGMO <> "" Then
        '        messaggio = "Il progetto non provede volontari Giovani Minore Opportunità."
        '        Return True
        '    Else
        '        messaggio = ""
        '        Return True
        '    End If
        'Else 'GMO SI SUL PROGETTO
        '    Return False
        'End If

    End Function

    Private Function VerificaCompletamentoGraduatorie() As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim dtsgenerico As DataSet
        Dim strsql As String
        Dim strcodiceprogetto As String
        Dim strcodicesede As String
        Dim nvolfile As Integer
        Dim nvolprevisti As Integer
        Dim strmessaggio As String
        strmessaggio = ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        strcodiceprogetto = ""
        strcodicesede = ""
        nvolfile = 0

        strsql = "select codiceprogetto, codicesede, count(*) nvolfile from  #TEMP_GRADUATORIA_VOLONTARI group by codiceprogetto, codicesede"
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("Conn"))
        Dim intX As Integer

        If dtsgenerico.Tables(0).Rows.Count > 0 Then

            For intX = 0 To dtsgenerico.Tables(0).Rows.Count - 1

                strcodiceprogetto = dtsgenerico.Tables(0).Rows(intX).Item("codiceprogetto")
                strcodicesede = dtsgenerico.Tables(0).Rows(intX).Item("codicesede")
                nvolfile = dtsgenerico.Tables(0).Rows(intX).Item("nvolfile")

                strsql = "select count(*) nvolprevisti from entità a where tmpCodiceProgetto = '" & strcodiceprogetto & "' and tmpIdSedeAttuazione = " & strcodicesede & ""
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    nvolprevisti = dtrgenerico("nvolprevisti")
                Else
                    nvolprevisti = 0
                End If
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                If nvolfile <> nvolprevisti Then
                    strmessaggio = strmessaggio & strcodiceprogetto & "_" & strcodicesede & ";"
                End If

            Next

        End If


        Return strmessaggio


    End Function

    Private Function VerificaNumeroPostiGMO(ByVal pCodiceAttivita As String, ByVal pCodiceSede As String, ByVal pGMO As String, ByRef messaggio As String) As Boolean

        'CONTROLLO PER I GIOVANI MINORI OPPORTUNITà

        ''Prelevo il numero di posti specificati per la sede
        Dim DtrPosti As System.Data.SqlClient.SqlDataReader
        Dim StrSql As String
        Dim StrSql1 As String
        Dim IntPostiProgettoNOGMO As Integer
        Dim IntPostiGMO As Integer = 0

        StrSql = "Select (ISNULL(attivitàentisediattuazione.NumeroPostiVittoAlloggio,0) + ISNULL(attivitàentisediattuazione.NumeroPostiVitto,0) + ISNULL(attivitàentisediattuazione.NumeroPostiNoVittoNoAlloggio,0)) As Posti," & _
                 " ISNULL(attivitàentisediattuazione.NumeroPostiGMO,0) as PostiGMO " & _
                 " From Attività Inner Join attivitàentisediattuazione on Attività.idattività = attivitàentisediattuazione.idattività " & _
                " Where Attività.CodiceEnte = '" & pCodiceAttivita & "' and attivitàentisediattuazione.identesedeattuazione = " & pCodiceSede
        DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        If DtrPosti.HasRows = True Then
            DtrPosti.Read()
            IntPostiProgettoNOGMO = DtrPosti.Item("Posti") - DtrPosti.Item("PostiGMO")
            IntPostiGMO = DtrPosti.Item("PostiGMO")
        Else
            IntPostiProgettoNOGMO = 0
        End If
        DtrPosti.Close()
        Dim IntPostiImportazioneNOGMO As Integer
        Dim IntPostiImportazioneGMO As Integer
        'If IntPostiGMO <> 0 Then
        'Controllo il numero di posti gia definiti nell'importazione

        StrSql = "Select Count(*) As Posti From #TEMP_GRADUATORIA_VOLONTARI as a INNER JOIN ENTITà as c ON a.CodiceFiscale = c.CodiceFiscale and a.codiceprogetto = c.tmpcodiceprogetto and a.codicesede = c.tmpidsedeattuazione INNER JOIN TipiEsitiGraduatoriaEntità b on a.esitoselezione = b.codice Where CodiceProgetto = '" & pCodiceAttivita & "' And CodiceSede = '" & pCodiceSede & "' And b.Idoneo = 'SI' And b.Selezionato = 'SI' AND ISNULL(LTRIM(RTRIM(c.gmo)),'')=''"

        StrSql1 = "Select Count(*) As PostiGMO From #TEMP_GRADUATORIA_VOLONTARI as  a INNER JOIN ENTITà as c ON a.CodiceFiscale = c.CodiceFiscale and a.codiceprogetto = c.tmpcodiceprogetto and a.codicesede = c.tmpidsedeattuazione INNER JOIN TipiEsitiGraduatoriaEntità b on a.esitoselezione = b.codice Where CodiceProgetto = '" & pCodiceAttivita & "' And CodiceSede = '" & pCodiceSede & "' And b.Idoneo = 'SI' And b.Selezionato = 'SI' AND ISNULL(LTRIM(RTRIM(c.gmo)),'')<>''"


        DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        DtrPosti.Read()
        IntPostiImportazioneNOGMO = DtrPosti.Item("Posti")
        DtrPosti.Close()
        DtrPosti = Nothing

        DtrPosti = ClsServer.CreaDatareader(StrSql1, Session("Conn"))
        DtrPosti.Read()
        IntPostiImportazioneGMO = DtrPosti.Item("PostiGMO")
        DtrPosti.Close()
        DtrPosti = Nothing

        ''verifico eventuali posti già avviati     And tmpidsedeattuazione = " & pCodiceSede & "
        'StrSql = "Select Count(*) As Posti From entità Where tmpCodiceProgetto = '" & pCodiceAttivita & "'  And idstatoentità =3"
        'DtrPosti = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        'DtrPosti.Read()
        'IntPostiImportazioneGMO = IntPostiImportazioneGMO + DtrPosti.Item("Posti")
        'DtrPosti.Close()
        'DtrPosti = Nothing
        If pGMO <> "" Then

            If IntPostiGMO > IntPostiImportazioneGMO Then
                messaggio = ""

            Else ''""
                messaggio = ""
                'rimosso controllo su giovani gmo che possono occupare anche posti "non gmo"
                ' messaggio = messaggio & "E' stato superato il numero di volontari Giovani Minori Opportunità previsti per il progetto."
            End If
        Else

            If IntPostiProgettoNOGMO > IntPostiImportazioneNOGMO Then
                'OK
            Else
                If IntPostiGMO <> 0 Then
                    messaggio = messaggio & "E' stato superato il numero di volontari non Giovani Minori Opportunità previsti per il progetto."
                End If
            End If
        End If
        If messaggio = "" Then
            Return True
        Else
            Return False
        End If

    End Function

    Function CheckMail(ByVal strMail) As Boolean
        Dim _pattern As String
        Dim r As Regex
        Dim m As Match
        Dim _bool As Boolean
        ' _pattern = "\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        _pattern = "^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
        r = New Regex(_pattern)
        m = r.Match(strMail)
        If m.Success Then
            CheckMail = True
        Else
            CheckMail = False
        End If
    End Function

    Private Function VerificaCategoria(ByVal Codicecategoria As String) As Boolean
        Dim dtrCategoria As SqlClient.SqlDataReader
        Dim strSql As String
        Dim sEsito As Boolean

        strSql = "Select * From CategorieEntità Where Codice='" & Replace(Codicecategoria, "'", "''") & "' and Attiva=1"
        dtrCategoria = ClsServer.CreaDatareader(strSql, Session("conn"))
        sEsito = dtrCategoria.HasRows

        dtrCategoria.Close()
        dtrCategoria = Nothing

        Return sEsito

    End Function
    Private Function VerificaEsitoSelezione(ByVal esitoselezionato As String, ByRef idoneo As String, ByRef selezionato As String) As Boolean
        Dim dtrCategoria As SqlClient.SqlDataReader
        Dim strSql As String
        Dim sEsito As Boolean

        strSql = "Select idoneo,selezionato From TipiEsitiGraduatoriaEntità Where Codice='" & Replace(esitoselezionato, "'", "''") & "' and Attiva=1"
        dtrCategoria = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrCategoria.HasRows Then
            sEsito = dtrCategoria.HasRows
            dtrCategoria.Read()
            idoneo = dtrCategoria("idoneo")
            selezionato = dtrCategoria("selezionato")
        Else
            idoneo = "NO"
            selezionato = "NO"
        End If
        dtrCategoria.Close()
        dtrCategoria = Nothing
        Return sEsito

    End Function

    Private Function VerificaConseguimentoTitolo(ByVal CodiceTitolo As String) As Boolean
        Dim dtrConseguimentoTitolo As SqlClient.SqlDataReader
        Dim strSql As String
        Dim sEsito As Boolean

        strSql = "Select * From TitoliStudioConseguimento Where Codice='" & Replace(CodiceTitolo, "'", "''") & "'"
        dtrConseguimentoTitolo = ClsServer.CreaDatareader(strSql, Session("conn"))
        sEsito = dtrConseguimentoTitolo.HasRows

        dtrConseguimentoTitolo.Close()
        dtrConseguimentoTitolo = Nothing

        Return sEsito

    End Function

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        'carico la home
        Response.Redirect("WfrmMain.aspx")
    End Sub


    Protected Sub CmdElabora_Click(sender As Object, e As EventArgs) Handles CmdElabora.Click
        lblMessaggioErrore.Visible = False
        lblMessaggioErrore.Text = ""
        If txtSelFile.FileName.ToString <> "" Then
            Dim file As String
            Dim estensione As String
            file = LCase(txtSelFile.FileName.ToString)
            estensione = file.Substring(file.Length - 4)
            If estensione = ".csv" Then
                If Session("Sistema") = "Helios" Then
                    UpLoad()
                Else
                    'UpLoadGG() temporaneamente tolta
                    UpLoad()
                End If
            Else
                lblMessaggioErrore.Visible = True
                lblMessaggioErrore.Text = "Selezionare il file nel formato CSV."
                Exit Sub
            End If
        Else
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = "Selezionare il file da inviare."
            Exit Sub
        End If

    End Sub

    'Private Function VerificaStatoCivile(ByVal CodiceStatoCivile As String) As Boolean
    '    Dim dtrStatoCivile As SqlClient.SqlDataReader
    '    Dim strSql As String
    '    Dim sEsito As Boolean

    '    strSql = "Select * From TipiStatoCivile Where Codice='" & Replace(CodiceStatoCivile, "'", "''") & "'"
    '    dtrStatoCivile = ClsServer.CreaDatareader(strSql, Session("conn"))
    '    sEsito = dtrStatoCivile.HasRows

    '    dtrStatoCivile.Close()
    '    dtrStatoCivile = Nothing

    '    Return sEsito

    'End Function

    'Private Function VerificaOblligatorietàCodiceFiscaleConiuge(ByVal CodiceStatoCivile As String) As Boolean
    '    Dim dtrStatoCivile As SqlClient.SqlDataReader
    '    Dim strSql As String
    '    Dim sEsito As Boolean

    '    strSql = "Select CFConiugeObbligatorio From TipiStatoCivile Where Codice='" & Replace(CodiceStatoCivile, "'", "''") & "'"
    '    dtrStatoCivile = ClsServer.CreaDatareader(strSql, Session("conn"))

    '    Dim necessario As Boolean
    '    If dtrStatoCivile.HasRows = True Then
    '        dtrStatoCivile.Read()
    '        sEsito = dtrStatoCivile("CFConiugeObbligatorio")
    '    End If
    '    dtrStatoCivile.Close()
    '    dtrStatoCivile = Nothing

    '    Return sEsito

    'End Function

    Private Function ParticolaritàEntità(ByVal Macrotipo As String, ByVal Codice As String) As Boolean
        Dim strSQL As String
        Dim dsParticolarità As SqlDataReader
        Dim sEsito As Boolean

        strSQL = "Select  * From ParticolaritàEntità where Macrotipo = '" & Macrotipo & "'  AND Codice='" & Codice & "'"
        dsParticolarità = ClsServer.CreaDatareader(strSQL, Session("conn"))

        sEsito = dsParticolarità.HasRows
        dsParticolarità.Close()
        dsParticolarità = Nothing

        Return sEsito

    End Function

    Private Function NazioneBaseProgetto(ByVal pCodiceProgetto As String) As String

        'mod. il 25/07/2018 da s.c. NazioneBase
        Dim dtrProgetto As SqlClient.SqlDataReader
        Dim strSql As String
        Dim NazioneBase As String

        strSql = " SELECT TipiProgetto.NazioneBase " & _
                 " FROM Attività " & _
                 " INNER JOIN TipiProgetto ON TipiProgetto.IdTipoProgetto = Attività.IdTipoProgetto " & _
                 " WHERE Attività.CodiceEnte= '" & pCodiceProgetto & "' "

        dtrProgetto = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrProgetto.HasRows = True Then
            dtrProgetto.Read()

            If dtrProgetto("NazioneBase") = True Then
                NazioneBase = "ITALIA"
            Else
                NazioneBase = "ESTERO"
            End If
        End If

        dtrProgetto.Close()
        dtrProgetto = Nothing

        Return NazioneBase
    End Function
    Public Sub CaricaEsitoSelezione()

        Dim strSQL As String
        Dim dstipoesito As SqlDataReader

        strSQL = "Select esito,codice From tipiesitigraduatoriaentità where attiva =1 Order by codice"
        dstipoesito = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dstipoesito.Read
            Response.Write("<li><strong>" & dstipoesito.Item("Codice") & "</strong> per <strong>" & dstipoesito.Item("esito") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dstipoesito.Close()
        dstipoesito = Nothing
    End Sub
End Class