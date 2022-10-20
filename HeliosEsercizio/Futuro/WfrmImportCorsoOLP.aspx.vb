Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Public Class WfrmImportCorsoOLP
    Inherits System.Web.UI.Page
    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private xIdAttivita As Integer
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim myCommand As System.Data.SqlClient.SqlCommand
    '-----------------------------blocco variabili campi file csv-------------------
    '0
    Private VAR_COGNOME As String = ""
    '1
    Private VAR_NOME As String = ""
    '2
    Private VAR_ENTERIFERIMENTO As String = ""
    '3
    Private VAR_LUOGOSVOLGIMENTO As String = ""
    '4
    Private VAR_DATASVOLGIMENTOCORSO As String = ""
    '5
    Private VAR_NUMEROORE As String = ""
   

    '-------------------------------------------------------------------------------
   
    '-------------------------------------------------------------------------------
#Region "Routine Importazione Corso OLP SCN"

    Private Sub UpLoad()
        '--- salvataggio del file sul server
        Dim swErr As Boolean
        swErr = False
        If txtSelFile.PostedFile.FileName.ToString <> "" Then
            Try
                NomeUnivoco = "CorsoOLP" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
        strSql = "CREATE TABLE [#TEMP_CORSO_OLP] (" & _
                 "[Cognome] [nvarchar] (150) COLLATE DATABASE_DEFAULT, " & _
                 "[Nome]  [nvarchar] (150) COLLATE DATABASE_DEFAULT, " & _
                 "[EnteRiferimento]  [nvarchar] (200) COLLATE DATABASE_DEFAULT, " & _
                 "[LuogoSvolgimento]  [nvarchar] (255) COLLATE DATABASE_DEFAULT, " & _
                 "[DataSvolgimentoCorso]  [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                 "[NumeroOre] [tinyint])"
      
       
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
            strSql = "DROP TABLE [#TEMP_CORSO_OLP]"

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
     


        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)

        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")

        '--- intestazione

        xLinea = Reader.ReadLine()
        Writer.WriteLine("Note;" & xLinea)

        'ArrCampi = CreaArray(xLinea, 1)

        'If UBound(ArrCampi) < 5 Or UBound(ArrCampi) > 5 Then
        '    strNote = strNote & "Il numero delle colonne inserite è diverso da quello previsto."
        '    swErr = True
        '    TotKo = TotKo + 1
        '    'Writer.WriteLine("Note;" & strNote)
        '    Writer.WriteLine(strNote & ";")
        '    strNote = vbNullString
        'End If


        '--- scorro le righe
        xLinea = Reader.ReadLine()
        While (xLinea <> "")
            swErr = False
            Tot = Tot + 1
            ArrCampi = CreaArray(xLinea, 0)

            If UBound(ArrCampi) < 5 Then '21

                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
                'Exit While
            Else
                If UBound(ArrCampi) > 5 Then '21
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1
                    '    Exit While
                Else

                  
                    '----------------------------ASSEGNAZIONE VARIABILI-------------------ADC-------------

                    'Cognome 
                    Try
                        VAR_COGNOME = Trim(ArrCampi(0))
                    Catch ex As Exception
                        VAR_COGNOME = ""
                    End Try


                    'Nome
                    Try
                        VAR_NOME = Trim(ArrCampi(1))
                    Catch ex As Exception
                        VAR_NOME = ""
                    End Try

                    'CodiceFiscale
                    Try
                        VAR_ENTERIFERIMENTO = Trim(ArrCampi(2))
                    Catch ex As Exception
                        VAR_ENTERIFERIMENTO = ""
                    End Try

                    'DataNascita
                    Try
                        VAR_LUOGOSVOLGIMENTO = Trim(ArrCampi(3))
                    Catch ex As Exception
                        VAR_LUOGOSVOLGIMENTO = ""
                    End Try

                    'Sesso
                    Try
                        VAR_DATASVOLGIMENTOCORSO = Trim(ArrCampi(4))
                    Catch ex As Exception
                        VAR_DATASVOLGIMENTOCORSO = ""
                    End Try

                    'codice istat Comune Nascita
                    Try
                        VAR_NUMEROORE = Trim(ArrCampi(5))
                    Catch ex As Exception
                        VAR_NUMEROORE = ""
                    End Try



                    '----------------------------FINE  ASSEGNAZIONE VARIABILI  --------------ADC------------------



                    '---------------------------INIZIO CONTROLLI --------------ADC------------------

                    'Cognome 


                    If VAR_COGNOME = vbNullString Then
                        strNote = strNote & "Il campo Cognome e' un campo obbligatorio."
                        swErr = True
                    Else
                        If Len(VAR_COGNOME) > 150 Then
                            strNote = strNote & "Il campo Cognome puo' contenere massimo 150 caratteri."
                            swErr = True
                        End If
                    End If


                    'Nome

                    If VAR_NOME = vbNullString Then
                        strNote = strNote & "Il campo Nome e' un campo obbligatorio."
                        swErr = True
                    Else
                        If Len(VAR_NOME) > 150 Then
                            strNote = strNote & "Il campo Nome puo' contenere massimo 150 caratteri."
                            swErr = True
                        End If
                    End If


                    'EnteRiferimento

                    If VAR_ENTERIFERIMENTO = vbNullString Then
                        strNote = strNote & "Il campo EnteRiferimento e' un campo obbligatorio."
                        swErr = True
                    Else
                        If Len(VAR_ENTERIFERIMENTO) > 200 Then
                            strNote = strNote & "Il campo EnteRiferimento puo' contenere massimo 200 caratteri."
                            swErr = True
                        End If
                    End If

                    'LuogoSvolgimento
                    If VAR_LUOGOSVOLGIMENTO = vbNullString Then
                        strNote = strNote & "Il campo LuogoSvolgimento e' un campo obbligatorio."
                        swErr = True
                    Else
                        If Len(VAR_LUOGOSVOLGIMENTO) > 255 Then
                            strNote = strNote & "Il campo LuogoSvolgimento puo' contenere massimo 255 caratteri."
                            swErr = True
                        End If
                    End If


                    'DataSvolgimento
                    If VAR_DATASVOLGIMENTOCORSO = vbNullString Then
                        strNote = strNote & "Il campo DataSvolgimentoCorso e' un campo obbligatorio."
                        swErr = True
                    Else

                        If Len(VAR_DATASVOLGIMENTOCORSO) > 100 Then
                            strNote = strNote & "Il campo DataSvolgimentoCorso puo' contenere massimo 100 caratteri."
                            swErr = True
                        End If
                    End If

                    'NumeroOre
                    If VAR_NUMEROORE = vbNullString Then
                        strNote = strNote & "Il campo NumeroOre e' un campo obbligatorio."
                        swErr = True
                    Else
                        If IsNumeric(VAR_NUMEROORE) = False Then
                            strNote = strNote & "Il campo NumeroOre non e' nel formato corretto."
                            swErr = True
                        End If
                    End If


                    If swErr = False Then
                        ScriviTabTemp(ArrCampi)
                    Else
                        TotKo = TotKo + 1
                    End If


                End If
            End If
            Writer.WriteLine(strNote & ";" & xLinea)
            'Writer.WriteLine(Replace(Replace(Replace(strNote, "<a style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('", ""), "')" & """" & "><font color=" & """" & "blue" & """" & "><b>SUGGERIMENTO</b></font></a> .", ""), "|", "'") & ";" & xLinea)
            strNote = vbNullString
            xLinea = Reader.ReadLine()
        End While

      
     

        Reader.Close()
        Writer.Close()
        'If swErr = True Then
        '    lblMessaggioErrore.Visible = True
        '    lblMessaggioErrore.Text = strNote
        'Else
        Response.Redirect("WfrmRisultatoImportCorsoOlp.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
        'End If


        ''--- reindirizzo la pagina sottostante
        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportGraduatoriaVolontari.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
        'Response.Write("</script>")
    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            AbilitazioneCorsiOLP()
        End If

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

                UpLoad()


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

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Private Sub ScriviTabTemp(ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String


        Try
            strsql = "INSERT INTO #TEMP_CORSO_OLP " & _
                     "(Cognome, " & _
                     "Nome, " & _
                     "EnteRiferimento, " & _
                     "LuogoSvolgimento, " & _
                     "DataSvolgimentoCorso, " & _
                     "NumeroOre " & _
                     ") " & _
                     "values " & _
                     "('" & Trim(ClsServer.NoApice(VAR_COGNOME)) & "', " & _
                     "'" & Trim(ClsServer.NoApice(VAR_NOME)) & "', " & _
                     "'" & Trim(ClsServer.NoApice(VAR_ENTERIFERIMENTO)) & "', " & _
                     "'" & Trim(ClsServer.NoApice(VAR_LUOGOSVOLGIMENTO)) & "', " & _
                     "'" & Trim(ClsServer.NoApice(VAR_DATASVOLGIMENTOCORSO)) & "', " & _
                     "'" & Trim(ClsServer.NoApice(VAR_NUMEROORE)) & "') "

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
            If UBound(TmpArr) = 5 Then
                ReDim Preserve DefArr(UBound(DefArr) + (5 - UBound(TmpArr)))
            End If
        End If

        CreaArray = DefArr

    End Function
    Private Sub AbilitazioneCorsiOLP()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        If Session("TipoUtente") <> "U" Then
            Dim AbilitazioneCorsiOLP As Boolean
            strsql = "Select isnull(AbilitazioneCorsiOLP,0) as AbilitazioneCorsiOLP, isnull(idclasseaccreditamento,0) as idclasseaccreditamento, isnull(idregionecompetenza,0) as idregionecompetenza from enti where  idente=" & Session("IdEnte")
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrgenerico.Read()
            If dtrgenerico.HasRows = True Then

                If dtrgenerico("AbilitazioneCorsiOLP") = False Or dtrgenerico("idregionecompetenza") <> 22 Or dtrgenerico("idclasseaccreditamento") <> 1 Then
                    nonabilitato.Visible = False
                    lblNonAbilitato.Visible = True
                End If

            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub
End Class