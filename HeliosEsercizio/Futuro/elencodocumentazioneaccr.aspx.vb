Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Data.SqlClient
Public Class elencodocumentazioneaccr
    Inherits System.Web.UI.Page
    Dim strSql As String
    Public TBLLeggiDati As DataTable
    Public row As TableRow
    Public myRow As DataRow
    Public dtrLeggiDati As SqlClient.SqlDataReader
#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

#End Region
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'controllo se effettuato login
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CheckStatoProgetto()
            'mauro lanna

            'mauro lanna
            If Session("TipoUtente") = "U" Then
                Call caricafascicolo()
                Call CaricaProtocollo()
            Else
                TxtNumeroFascicolo.Visible = False
                cmdSelFascicolo.Visible = False
                cmdSelProtocollo.Visible = False
                cmdFascCanc.Visible = False
                txtDescFasc.Visible = False
                'Salva.Visible = False
                Label11.Visible = False
                Label31.Visible = False
            End If
            '** fine
            'carico la combo con le fasi valutate
            CaricaFasiEnte(Session("IdEnte"))
            AbilitaStampeAdeguamentiTotale(Session("IdEnte"))
        End If

        'Modifica del 17/01/2006 di Amilcare Paolella ***************************
        'Ricavo le informazioni dell'utente per valorizzare la path dei documenti
        strSql = "SELECT RegioniCompetenze.CodiceRegioneCompetenza AS Path FROM UtentiUNSC INNER JOIN " & _
                 "RegioniCompetenze ON UtentiUNSC.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                 "WHERE UtentiUNSC.UserName ='" & Session("Utente") & "'"
        dtrLeggiDati = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrLeggiDati.Read = True Then
            Session("Path") = dtrLeggiDati("Path")
            Session("Path") &= "/"
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        Else
            'Non c'è corrispondenza è successo qualcosa di inusuale;esco e torno alla logon
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
            Response.Redirect("LogOn.aspx")
        End If
        '************************************************************************

        'FZ controllo per disabilitare la maschera nel caso sia un'"R" che sta 
        'controllando i progetti di un' ente che nn ha la stessa regiojneee di competenza
        If ClsUtility.ControlloRegioneCompetenza(Session("TipoUtente"), Session("idEnte"), Session("Utente"), Session("IdStatoEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = False Then
            imgGeneraFile.Visible = False
            lblmessaggiosopra.ForeColor = Color.Red
            lblmessaggiosopra.Text = "Attenzione, l'ente non è di propria competenza. Impossibile effettuare modifiche."
            lblmessaggiosopra.Visible = True
            'Imgerrore.Visible = True
            'Imgerrore.ImageUrl = "Images/alert3.gif"
        End If
        'FZ fine controllo

    End Sub
    Sub CheckStatoProgetto()
        'Try
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'Dim cmdinsert As Data.SqlClient.SqlCommand
        'strSql = "insert into _log values ('entro',getdate()) "
        'cmdinsert = New SqlClient.SqlCommand(strSql, Session("conn"))
        'cmdinsert.ExecuteNonQuery()

        'preparo la query che verifica lo stato dell'ente
        strSql = "select StatoEnte, isnull(enti.FlagAccreditamentoCompleto,0) as FlagAccreditamentoCompleto from statienti "
        strSql = strSql & "inner join enti on statienti.idstatoente=enti.idstatoente "
        strSql = strSql & "where enti.idente='" & Session("IdEnte") & "'"

        'eseguo la query
        dtrLeggiDati = ClsServer.CreaDatareader(strSql, Session("conn"))
        'se ci sono righe vado a controllare lo stato
        If dtrLeggiDati.HasRows = True Then
            dtrLeggiDati.Read()
            Select Case dtrLeggiDati("StatoEnte")

                Case "Istruttoria" 'blocco tutte le stampede dell'adeguamento anche combo fase
                    ddlFasi.Enabled = False

                    chkComAvvioProdAdeg.Enabled = False
                    chkLetteraCompleDocuAdeg.Enabled = False
                    chkRicClassAdeg.Enabled = False
                    lblRicClassAdeg.Enabled = False
                    ChkArt2adeg.Enabled = False
                    LblArt2adeg.Enabled = False
                    chkArt10adeg.Enabled = False
                    lblart10adeg.Enabled = False
                    chkLettAdegPosNeg.Enabled = False
                    lblLettAdegPosNeg.Enabled = False
                    chkDetAdegPos.Enabled = False
                    lblDetAdegPos.Enabled = False
                    ChkDetAdegPosArt10.Enabled = False
                    lblDetAdegPosArt10.Enabled = False
                    chkDetAdegPosLim.Enabled = False
                    lblDetAdegPosLim.Enabled = False
                    chkDetAdegNeg.Enabled = False
                    lblDetAdegNeg.Enabled = False
                    chkAllegatoA1.Enabled = False
                    lblAllegatoA1.Enabled = False
                    chkAllegatoA2.Enabled = False
                    lblAllegatoA2.Enabled = False
                    chkAllegatoB.Enabled = False
                    lblAllegatoB.Enabled = False

                    'se è attivo 
                Case "Attivo"
                    chkDetAccPos.Enabled = True
                    chkDetAccrPosArt10.Enabled = True
                    lblDetAccrPosArt10.Enabled = True
                    lblDetAccPos.Enabled = True
                    chkdetAccPosLim.Enabled = True
                    lbldetAccPosLim.Enabled = True
                    chkdetAccPosLimSediFigure.Enabled = True
                    lbldetAccPosLimSediFigure.Enabled = True
                    chkDetAdegPos.Enabled = True
                    ChkDetAdegPosArt10.Enabled = True
                    lblDetAdegPosArt10.Enabled = True
                    lblDetAdegPos.Enabled = True
                    chkDetAdegPosLim.Enabled = True
                    lblDetAdegPosLim.Enabled = True
                    'chkdetRevPos.Enabled = True
                    'chkDetRevPosArt10.Enabled = True
                    'lblDetRevPosArt10.Enabled = True
                    'lbldetRevPos.Enabled = True
                    'chkdetRevPosLim.Enabled = True
                    'lbldetRevPosLim.Enabled = True
                    chkDetAdegNeg.Enabled = True
                Case "Chiuso"
                    lblLetAccPos.Enabled = False
                    chkLetAccPos.Enabled = False
                    'chkDetNegAccr.Enabled = True
                    'lblDetNegAccr.Enabled = True
                    'chkDetNegASegRispEnte.Enabled = True
                    'lblDetNegASegRispEnte.Enabled = True
                    'chkDetNegSenzaRispEnte.Enabled = True
                    'lblDetNegSenzaRispEnte.Enabled = True
                    chkDetAccNeg.Enabled = True
                    lblDetAccNeg.Enabled = True
                    chkDetAdegNeg.Enabled = True
                    lblDetAdegNeg.Enabled = True
                    'chkDetRevNeg.Enabled = True
                    'lblDetRevNeg.Enabled = True
                Case "In Adeguamento"
                    'doc accreditamento
                    ' Leucci Luigi 28-02-2019
                    lblart2IIstep.Enabled = False
                    ChkArt2IIstep.Enabled = False
                    lblart10IIstep.Enabled = False
                    chkart10IIstep.Enabled = False
                    ' Fine 28-02-2019

                    lblLetAccPos.Enabled = False
                    chkLetAccPos.Enabled = False
                    lblLetAccNeg.Enabled = False
                    chkLetAccNeg.Enabled = False
                    lblAllegatoA1IIStep.Enabled = False
                    chkAllegatoA1IIStep.Enabled = False
                    lblAllegatoA2IIStep.Enabled = False
                    chkAllegatoA2IIStep.Enabled = False
                    lblAllegatoBIIStep.Enabled = False
                    chkAllegatoBIIStep.Enabled = False
                    'doc adeguaento mod. il 07/09/2015 
                    lblLettAdegPosNeg.Enabled = True
                    chkLettAdegPosNeg.Enabled = True
                    lblAllegatoA1.Enabled = True
                    chkAllegatoA1.Enabled = True
                    lblAllegatoA2.Enabled = True
                    chkAllegatoA2.Enabled = True
                    lblAllegatoB.Enabled = True
                    chkAllegatoB.Enabled = True
                    chkDetAdegPos.Enabled = True
                    lblDetAdegPos.Enabled = True
                    ChkDetAdegPosArt10.Enabled = True
                    lblDetAdegPosArt10.Enabled = True
                    chkDetAdegPosLim.Enabled = True
                    lblDetAdegPosLim.Enabled = True

                    'lblLettAdegPosNeg.Enabled = False
                    'chkLettAdegPosNeg.Enabled = False
                    'lblAllegatoA1.Enabled = False
                    'chkAllegatoA1.Enabled = False
                    'lblAllegatoA2.Enabled = False
                    'chkAllegatoA2.Enabled = False
                    'lblAllegatoB.Enabled = False
                    'chkAllegatoB.Enabled = False

            End Select

            If dtrLeggiDati("StatoEnte") = "Istruttoria" And dtrLeggiDati("FlagAccreditamentoCompleto") Then
                chkDetAccPos.Enabled = True
                chkDetAccrPosArt10.Enabled = True
                lblDetAccrPosArt10.Enabled = True
                lblDetAccPos.Enabled = True
                chkdetAccPosLim.Enabled = True
                lbldetAccPosLim.Enabled = True
                chkdetAccPosLimSediFigure.Enabled = True
                lbldetAccPosLimSediFigure.Enabled = True
            End If
        End If

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        ''Dim cmdinsert As Data.SqlClient.SqlCommand
        'strSql = "insert into _log values ('esco',getdate()) "
        'cmdinsert = New SqlClient.SqlCommand(strSql, Session("conn"))
        'cmdinsert.ExecuteNonQuery()

        'Catch ex As Exception
        '    If Not dtrLeggiDati Is Nothing Then
        '        dtrLeggiDati.Close()
        '        dtrLeggiDati = Nothing
        '    End If
        '    Dim cmdinsert As Data.SqlClient.SqlCommand
        '    strSql = "insert into _log values ('" & ex.Message.Replace("'", "''") & "',getdate()) "
        '    cmdinsert = New SqlClient.SqlCommand(strSql, Session("conn"))
        '    cmdinsert.ExecuteNonQuery()
        '    Response.Write(ex.Message)
        'End Try
    End Sub

    Function CaricaFile(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(a.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Denominazione, "
            strsql = strsql & "isnull(a.CodiceRegione,'') as CodiceRegione, "
            strsql = strsql & "isnull(a.IdClasseAccreditamentoRichiesta,'') as ClasseRichiesta, "
            strsql = strsql & "isnull(case len(day(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),day(a.DataCostituzione)) "
            strsql = strsql & "else convert(varchar(20),day(a.DataCostituzione))  end + '/' + "
            strsql = strsql & "(case len(month(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),month(a.DataCostituzione)) "
            strsql = strsql & "else convert(varchar(20),month(a.DataCostituzione))  end + '/' + "
            strsql = strsql & "Convert(varchar(20), Year(a.DataCostituzione))),'') as DataCostituzione,"
            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(b.indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Indirizzo, "
            strsql = strsql & "isnull(b.Civico,'') as Civico, "
            strsql = strsql & "isnull(b.CAP,'') as CAP, "
            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni.denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Comune, "
            strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(provincie.provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Provincia "
            strsql = strsql & "from enti as a "
            strsql = strsql & "left join entisedi as b on a.idente=b.idente "
            strsql = strsql & "and b.identesede = any (SELECT pippo.identesede FROM entisedi pippo "
            strsql = strsql & "INNER JOIN entiseditipi pluto ON pippo.identesede = pluto.identesede "
            strsql = strsql & "inner join statientisedi on statientisedi.idstatoentesede=pippo.idstatoentesede "
            strsql = strsql & "WHERE pluto.idtiposede = 1 and statientisedi.attiva=1) "
            strsql = strsql & "left join comuni on b.idcomune=comuni.idcomune "
            strsql = strsql & "left join provincie on comuni.idprovincia=provincie.idprovincia "
            strsql = strsql & "left join entiseditipi as c on b.identesede=c.identesede "
            strsql = strsql & "left join tipisedi as d on c.idtiposede=d.idtiposede "
            strsql = strsql & "left join statientisedi as e on b.idstatoentesede=e.idstatoentesede "
            strsql = strsql & "where a.IdEnte=" & Session("IdEnte") & " and ((d.tiposede='Principale') or (b.identesede is null))"

            'eseguo la query e passo il risultato al datareader
            dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".rtf"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/" & NomeFile & ".rtf"))
                Writer = New StreamWriter(strPercorsoFile)

                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")
                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'chiudo lo streaming in lettura
                Writer.Close()
                Writer = Nothing

                'chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

                CaricaFile = "documentazione/" & strNomeFile

                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If

                'vado a fare la insert
                Dim cmdinsert As Data.SqlClient.SqlCommand
                strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
                strsql = strsql & "values "
                strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
                cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
                cmdinsert.ExecuteNonQuery()

                cmdinsert.Dispose()

            Else
                CaricaFile = ""
            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If
            Return CaricaFile
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function

    Sub NuovaCronologia(ByVal strDocumento As String, ByVal IDEnteFase As Integer)
        'vado a fare la insert
        Dim cmdinsert As Data.SqlClient.SqlCommand
        strSql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento,IDEnteFase) "
        strSql = strSql & "values "
        strSql = strSql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & strDocumento & "',0, " & IDEnteFase & ")"
        cmdinsert = New SqlClient.SqlCommand(strSql, Session("conn"))
        cmdinsert.ExecuteNonQuery()

        cmdinsert.Dispose()
    End Sub
    Function Attribuzionecodice(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'caricaprogetti
            CaricaProgetti()
            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '"IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\qc\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Attribuzionecodice = "documentazione/" & strNomeFile


            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function Articolo2(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'Carico i Dati per l'Intestazione del documento
            strsql = "Select IsNull(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Enti.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Denominazione, " & _
                     "IsNull(Replace(Replace(Replace(Replace(Replace(Replace(Replace(EntiSedi.Indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Indirizzo, " & _
                     "IsNull(Replace(Replace(Replace(Replace(Replace(Replace(Replace(EntiSedi.Civico,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Civico, " & _
                     "IsNull(EntiSedi.Cap,'') as Cap, " & _
                     "IsNull(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Comuni.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Comune, " & _
                     "IsNull(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Provincie.DescrAbb,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Provincia " & _
                     "From Enti " & _
                     "INNER JOIN EntiSedi ON Enti.IdEnte = EntiSedi.IdEnte " & _
                     "INNER JOIN StatiEntiSedi ON EntiSedi.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                     "INNER JOIN EntiSediTipi ON EntiSedi.IdEnteSede = EntiSediTipi.IdEnteSede AND EntiSediTipi.IdTipoSede = 1 " & _
                     "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                     "INNER JOIN Provincie ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                     "Where Enti.IdEnte = " & Session("IdEnte") & " Order BY StatiEntiSedi.Ordine ASC"
            'eseguo la query e passo il risultato al datareader
            dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)
                'Writer.WriteLine("{\rtf1")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<Civico>", IIf(dtrLeggiDati("Civico") = "", dtrLeggiDati("Civico"), ", " & dtrLeggiDati("Civico")))
                    xLinea = Replace(xLinea, "<Cap>", dtrLeggiDati("Cap"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", IIf(dtrLeggiDati("Provincia") = "", dtrLeggiDati("Provincia"), "(" & dtrLeggiDati("Provincia") & ")"))
                    xLinea = Replace(xLinea, "_", " ")
                    Writer.WriteLine(xLinea)
                    xLinea = Reader.ReadLine()
                End While

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                'chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing
            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Articolo2 = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            If NomeFile = "art2adeguamento" Then
                strsql = "Insert Into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) Values " & _
                         "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'Articolo 2 Adeguamento',0)"
            ElseIf NomeFile = "art2accreditamento" Then
                strsql = "Insert Into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) Values " &
                                  "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'Articolo 2 Iscrizione',0)"
            End If
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()
            cmdinsert.Dispose()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function Art10accreditamento(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\qc\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Art10accreditamento = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function Detnegaccraseguitorispostaente(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                                "IST003\par}}")
                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Detnegaccraseguitorispostaente = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function Detnegaccrsenzarispostaente(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                                "IST003\par}}")
                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Detnegaccrsenzarispostaente = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function Detnegaccrtreanni(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                                "IST003\par}}")
                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Detnegaccrtreanni = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function Letteratrasmissioneperdeterminanegtreanni(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST003\par}}")
                'Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                '                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Letteratrasmissioneperdeterminanegtreanni = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function LetteraAdeguamentoPositivoNegativo(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Lettera Adeguamento Positivo Negativo
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")
                '------------------------------------------------
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '"AA02\par}}")
                '----------------------------------------------------------
                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraAdeguamentoPositivoNegativo = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function LetteraAvvioProcedimento(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Lettera Adeguamento Positivo Negativo
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")
                '-------------------------------------------------------------------------
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '"AA02\par}}")
                '--------------------------------------------------------------------------
                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraAvvioProcedimento = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function LetteraCompleDocu(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Lettera Adeguamento Positivo Negativo
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")
                '---------------------------------------------------------------------------
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '"AA02\par}}")
                '------------------------------------------------------------------------------
                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraCompleDocu = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function LetteraAvvioProcedimentoAdeg(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Lettera Adeguamento Positivo Negativo
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")
                '------------------------------------------------------------------------------
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '"AA02\par}}")
                '--------------------------------------------------------------------------------
                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraAvvioProcedimentoAdeg = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function LetteraCompleDocuAdeg(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Lettera Adeguamento Positivo Negativo
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")
                '-------------------------------------------------------------------------
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '"AA02\par}}")
                '---------------------------------------------------------------------------------------
                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraCompleDocuAdeg = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function DeterminaAdeguamentoPositivo(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Determina Adeguamento Positivo 
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                                "AA02A\par}}")
                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "<Sediacc>", dtrLeggiDati("sediacc"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            DeterminaAdeguamentoPositivo = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function DeterminaAdeguamentoPositivoLimitazioni(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Determina Adeguamento Positivo con Limitazioni
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                                "AA02B\par}}")
                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "<Sediacc>", dtrLeggiDati("sediacc"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            DeterminaAdeguamentoPositivoLimitazioni = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function DeterminaAdeguamentoNegativo(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Determina Adeguamento Negativo
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                                "AA02A\par}}")
                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "_", " ")
                    xLinea = Replace(xLinea, "-", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            DeterminaAdeguamentoNegativo = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function FalseSedi(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "<NClasse>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "_", " ")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            FalseSedi = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Sub CaricaProgetti()
        Dim strsql As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(upper(replace(replace(replace(replace(replace(replace(replace(a.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Titolo, "
        strsql = strsql & "isnull(case len(day(a.DataInizioPrevista)) when 1 then '0' + convert(varchar(20),day(a.DataInizioPrevista)) else convert(varchar(20),day(a.DataInizioPrevista))  end + '/' + (case len(month(a.DataInizioPrevista)) when 1 then '0' + convert(varchar(20),month(a.DataInizioPrevista)) else convert(varchar(20),month(a.DataInizioPrevista))  end + '/' + Convert(varchar(20), Year(a.DataInizioPrevista))),'xx/xx/xxxx') as DataInizioPrevista, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace(a.CodiceEnte,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as CodiceEnte "
        strsql = strsql & "from attività as a "
        strsql = strsql & "inner join attivitàsediassegnazione as b on a.idattività=b.idattività "
        strsql = strsql & "inner join enti as c on a.identepresentante=c.idente "
        strsql = strsql & "where b.statograduatoria=3 and a.IdEntePresentante=" & CInt(Session("IdEnte"))

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub

    Sub CaricaElencoSediAbilitate()
        Dim strsql As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(upper(replace(replace(replace(replace(replace(replace(replace([Nome Sede Fisica],'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Nome, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace([Indirizzo],'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Indirizzo, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace([Civico],'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Civico, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace([CAP],'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as CAP, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace([Comune],'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Comune, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace([Provincia],'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Provincia, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace([Codice Sede Attuazione],'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Codice "
        strsql = strsql & "FROM VW_ELENCO_SEDI "
        strsql = strsql & "WHERE ([idente] = '" & Session("IdEnte") & "') "
        strsql = strsql & "group by [Nome Sede Fisica], [Indirizzo], [Civico], [CAP], [Comune], [Provincia], [Codice Sede Attuazione]"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub

    Sub CaricaElencoRisorseAccreditate()
        Dim strsql As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(upper(replace(replace(replace(replace(replace(replace(replace(EntePersonale.Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Nome, " & _
                 "isnull(upper(replace(replace(replace(replace(replace(replace(replace(EntePersonale.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Cognome, " & _
                 "isnull(upper(replace(replace(replace(replace(replace(replace(replace(EntePersonale.CodiceFiscale,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as CodiceFiscale, " & _
                 "EntePersonale.DataNascita, " & _
                 "isnull(upper(replace(replace(replace(replace(replace(replace(replace(Comuni.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Comune, " & _
                 "isnull(upper(replace(replace(replace(replace(replace(replace(replace(Ruoli.Ruolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Ruolo " & _
                 "FROM EntePersonale " & _
                 "INNER JOIN Comuni ON EntePersonale.IdComuneNascita = Comuni.IdComune " & _
                 "INNER JOIN EntePersonaleRuoli ON EntePersonale.IdEntePersonale = EntePersonaleRuoli.IdEntePersonale " & _
                 "INNER JOIN Ruoli ON EntePersonaleRuoli.IdRuolo = Ruoli.IdRuolo " & _
                 "WHERE  entepersonale.datafinevalidità is null and entepersonaleruoli.datafinevalidità is null and  Ruoli.RuoloAccreditamento = 1 and  Ruoli.Nascosto = 0 And EntePersonale.IdEnte = '" & Session("IdEnte") & "' And EntePersonaleRuoli.Accreditato = 1 " & _
                 "Order By Cognome, Nome, Ruolo"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub

    Sub CaricaElencoElencoEntiEsclusi()
        Dim strsql As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT entisedi.CAP as CAP, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace(entisedi.indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Indirizzo, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace(comuni.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Comune, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace(provincie.Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Provincia, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace(enti_2.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Nome, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace(enti_2.CodiceRegione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Codice "
        strsql = strsql & "FROM entirelazioni "
        strsql = strsql & "INNER JOIN enti enti_2 ON entirelazioni.IDEnteFiglio = enti_2.IDEnte "
        strsql = strsql & "INNER JOIN statienti statienti_2 ON enti_2.IDStatoEnte = statienti_2.IDStatoEnte "
        strsql = strsql & "LEFT JOIN entisedi ON entisedi.IDEnte = enti_2.IDEnte "
        strsql = strsql & "AND ENTISEDI.IDENTESEDE  = ANY (SELECT A.IDENTESEDE FROM ENTISEDI A INNER JOIN "
        strsql = strsql & "ENTISEDITIPI ON ENTISEDITIPI.IDENTESEDE = A.IDENTESEDE  WHERE  A.IDENTE = '" & Session("IdEnte") & "' AND ENTISEDITIPI.IDTIPOSEDE = 1)"
        strsql = strsql & "LEFT JOIN comuni ON entisedi.IDComune = comuni.IDComune "
        strsql = strsql & "LEFT JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
        strsql = strsql & "WHERE (statienti_2.Sospeso = 1) AND ENTIrelazioni.IDENTEpadre = '" & Session("IdEnte") & "' "
        strsql = strsql & "AND enti_2.datadeterminazionenegativa>= (select max(datainizio) from processitemporali) "
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub

    Sub CaricaElencoElencoRisorseEsclusi()
        Dim strsql As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "select isnull(upper(replace(replace(replace(replace(replace(replace(replace(entepersonale.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Cognome, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace(entepersonale.Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Nome, "
        strsql = strsql & "isnull(case len(day(entepersonale.DataNascita)) when 1 then '0' + convert(varchar(20),day(entepersonale.DataNascita)) "
        strsql = strsql & "else convert(varchar(20),day(entepersonale.DataNascita))  end + '/' + "
        strsql = strsql & "(case len(month(entepersonale.DataNascita)) when 1 then '0' + convert(varchar(20),month(entepersonale.DataNascita)) "
        strsql = strsql & "else convert(varchar(20),month(entepersonale.DataNascita))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(entepersonale.DataNascita))),'') as DataNascita,"
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace(comuni.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Comune, "
        strsql = strsql & "isnull(upper(replace(replace(replace(replace(replace(replace(replace(ruoli.Ruolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u''')), '') as Ruolo "
        strsql = strsql & "FROM entepersonaleruoli "
        strsql = strsql & "INNER JOIN entepersonale ON entepersonaleruoli.IDEntePersonale = entepersonale.IDEntePersonale "
        strsql = strsql & "INNER JOIN enti ON entepersonale.IDEnte = enti.IDEnte "
        strsql = strsql & "INNER JOIN comuni ON entepersonale.IDComuneNascita = comuni.IDComune "
        strsql = strsql & "INNER JOIN ruoli ON entepersonaleruoli.IDRuolo = ruoli.IDRuolo "
        strsql = strsql & "WHERE (entepersonale.datafinevalidità is null ) and (entepersonaleruoli.datafinevalidità is null ) and (entepersonaleruoli.Accreditato = - 1) AND (enti.IDEnte = '" & Session("IdEnte") & "') "
        strsql = strsql & "AND entepersonaleruoli.dataaccreditamento >= (select max(datainizio) from processitemporali) "
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub

    Sub CaricaDati()
        Dim strsql As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(replace(replace(replace(replace(replace(replace(replace(a.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Denominazione, "
        strsql = strsql & "isnull(a.CodiceRegione,'') as CodiceRegione, "
        strsql = strsql & "isnull(a.IdClasseAccreditamentoRichiesta,'') as ClasseRichiesta, "
        strsql = strsql & "isnull(case len(day(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),day(a.DataCostituzione)) "
        strsql = strsql & "else convert(varchar(20),day(a.DataCostituzione))  end + '/' + "
        strsql = strsql & "(case len(month(a.DataCostituzione)) when 1 then '0' + convert(varchar(20),month(a.DataCostituzione)) "
        strsql = strsql & "else convert(varchar(20),month(a.DataCostituzione))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(a.DataCostituzione))),'') as DataCostituzione,"
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(b.indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Indirizzo, "
        strsql = strsql & "isnull(b.Civico,'') as Civico, "
        strsql = strsql & "isnull(b.CAP,'') as CAP, "
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(comuni.denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Comune, "
        strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(provincie.provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Provincia, "
        strsql = strsql & "(select count(*) from vw_elenco_sedi where vw_elenco_sedi.idente = " & Session("IdEnte") & ") as sediacc "
        strsql = strsql & "from enti as a "
        strsql = strsql & "left join entisedi as b on a.idente=b.idente "
        strsql = strsql & "and b.identesede = any (SELECT pippo.identesede FROM entisedi pippo "
        strsql = strsql & "INNER JOIN entiseditipi pluto ON pippo.identesede = pluto.identesede "
        strsql = strsql & "inner join statientisedi on statientisedi.idstatoentesede=pippo.idstatoentesede "
        strsql = strsql & "WHERE pluto.idtiposede = 1) " 'and statientisedi.attiva=1) "
        strsql = strsql & "left join comuni on b.idcomune=comuni.idcomune "
        strsql = strsql & "left join provincie on comuni.idprovincia=provincie.idprovincia "
        strsql = strsql & "left join entiseditipi as c on b.identesede=c.identesede "
        strsql = strsql & "left join tipisedi as d on c.idtiposede=d.idtiposede "
        strsql = strsql & "left join statientisedi as e on b.idstatoentesede=e.idstatoentesede "
        strsql = strsql & "where a.IdEnte=" & Session("IdEnte") & " and ((d.tiposede='Principale') or (b.identesede is null)) "
        strsql = strsql & "order by e.ordine"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub

    Function AllegatoA1determinazione(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)

                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                'apro il template
                xLinea = Reader.ReadLine()

                Dim strDenominazioneEnte As String = dtrLeggiDati("Denominazione")
                Dim strClasseRichiesta As String = dtrLeggiDati("ClasseRichiesta") & "^"
                Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")
                Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strNumeroCivico As String = dtrLeggiDati("Civico")
                Dim strCAP As String = dtrLeggiDati("CAP")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim strProvincia As String = dtrLeggiDati("provincia")
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceRegione")
                Dim strNumeroSedi As String

                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If

                'prendo la data dal server
                dtrLeggiDati = ClsServer.CreaDatareader("select count(*) as ConteggioSedi FROM VW_ELENCO_SEDI WHERE ([idente] = '" & Session("IdEnte") & "')", Session("conn"))
                If dtrLeggiDati.HasRows = True Then
                    dtrLeggiDati.Read()
                    'conteggio sedi 0 se non ci sono
                    strNumeroSedi = dtrLeggiDati("ConteggioSedi")
                Else
                    'conteggio sedi 0 se non ci sono
                    strNumeroSedi = "0"
                End If

                CaricaElencoSediAbilitate()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<CodiceRegione>", strCodiceRegione)
                    xLinea = Replace(xLinea, "<NumeroSedi>", strNumeroSedi)

                    Dim intX As Integer

                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        xLinea = Replace(xLinea, "<BreakPoint>", "")
                        If dtrLeggiDati.HasRows = True Then
                            While dtrLeggiDati.Read
                                Writer.WriteLine(dtrLeggiDati("Nome") & " - " & dtrLeggiDati("Indirizzo") & " " & dtrLeggiDati("Civico") & dtrLeggiDati("CAP") & " " & dtrLeggiDati("Comune") & " (" & dtrLeggiDati("Provincia") & ") - " & dtrLeggiDati("Codice") & "\par")
                            End While
                        End If
                    End If
                    Writer.WriteLine(xLinea)
                    xLinea = Reader.ReadLine()
                End While

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            AllegatoA1determinazione = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function AllegatoA2determinazione(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'Carico i Dati per l'Intestazione del documento
            strsql = "Select isnull(replace(replace(replace(replace(replace(replace(replace(Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Denominazione, " & _
                     "IsNull(CodiceRegione,'') as CodiceRegione, IsNull(IdClasseAccreditamentoRichiesta,'') as ClasseRichiesta " & _
                     "From Enti Where IdEnte = " & Session("IdEnte")
            'eseguo la query e passo il risultato al datareader
            dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)

                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                'apro il template
                xLinea = Reader.ReadLine()

                Dim strDenominazioneEnte As String = dtrLeggiDati("Denominazione")
                Dim strClasseRichiesta As String = dtrLeggiDati("ClasseRichiesta") & "^"
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceRegione")
                Dim strNumeroRisorse As String

                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If

                'prendo la data dal server
                dtrLeggiDati = ClsServer.CreaDatareader("select Count(*) As ConteggioRisorse FROM EntePersonale " & _
                                                        "INNER JOIN Comuni ON EntePersonale.IdComuneNascita = Comuni.IdComune " & _
                                                        "INNER JOIN EntePersonaleRuoli ON EntePersonale.IdEntePersonale = EntePersonaleRuoli.IdEntePersonale " & _
                                                        "INNER JOIN Ruoli ON EntePersonaleRuoli.IdRuolo = Ruoli.IdRuolo " & _
                                                        "WHERE entepersonale.datafinevalidità is null and entepersonaleruoli.datafinevalidità is null and Ruoli.RuoloAccreditamento = 1 and  Ruoli.Nascosto = 0 And EntePersonale.IdEnte = '" & Session("IdEnte") & "' And EntePersonaleRuoli.Accreditato = 1", Session("conn"))
                If dtrLeggiDati.HasRows = True Then
                    dtrLeggiDati.Read()
                    'conteggio sedi 0 se non ci sono
                    strNumeroRisorse = dtrLeggiDati("ConteggioRisorse")
                Else
                    'conteggio sedi 0 se non ci sono
                    strNumeroRisorse = "0"
                End If

                CaricaElencoRisorseAccreditate()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<CodiceRegione>", strCodiceRegione)
                    xLinea = Replace(xLinea, "<NumeroRisorse>", strNumeroRisorse)

                    Dim intX As Integer

                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        xLinea = Replace(xLinea, "<BreakPoint>", "")
                        If dtrLeggiDati.HasRows = True Then
                            While dtrLeggiDati.Read
                                Writer.WriteLine(dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " - " & dtrLeggiDati("CodiceFiscale") & " " & dtrLeggiDati("DataNascita") & " " & dtrLeggiDati("Comune") & " - " & dtrLeggiDati("Ruolo") & "\par")
                            End While
                        End If
                    End If

                    Dim num As Integer
                    num = 0
                    If InStr(xLinea, "<BreakPoint1>") > 0 Then
                        xLinea = Replace(xLinea, "<BreakPoint1>", "") & "\par"
                        Call Servizi()
                        For Each myRow In TBLLeggiDati.Rows
                            num = num + 1
                            Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                            Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                            Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                            Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                            Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            'Writer.WriteLine("\cellx700\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx3000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx6000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx8000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            'Writer.WriteLine("\cellx9000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\f2\fs15\ " & myRow.Item("Sistema") & "\cell " & myRow.Item("Denominazione") & "\cell " & myRow.Item("CodiceRegione") & "\cell " & myRow.Item("Stato") & "\cell\row\pard\f2\fs15")
                        Next
                        Writer.WriteLine("\par")
                    End If

                    Writer.WriteLine(xLinea)
                    xLinea = Reader.ReadLine()

                End While

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            AllegatoA2determinazione = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function AllegatoBdeterminazione(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)

                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                'apro il template
                xLinea = Reader.ReadLine()

                Dim strDenominazioneEnte As String = dtrLeggiDati("Denominazione")
                Dim strClasseRichiesta As String = dtrLeggiDati("ClasseRichiesta") & "^"
                Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")
                Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strNumeroCivico As String = dtrLeggiDati("Civico")
                Dim strCAP As String = dtrLeggiDati("CAP")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim strProvincia As String = dtrLeggiDati("provincia")
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceRegione")

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<DataCostituzione>", strDataCostituzione)
                    xLinea = Replace(xLinea, "<Indirizzo>", strIndirizzo)
                    xLinea = Replace(xLinea, "<NumeroCivico>", strNumeroCivico)
                    xLinea = Replace(xLinea, "<CAP>", strCAP)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Provincia>", strProvincia)
                    xLinea = Replace(xLinea, "<CodiceRegione>", strCodiceRegione)

                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        CaricaElencoElencoEntiEsclusi()
                        xLinea = Replace(xLinea, "<BreakPoint>", "")
                        If dtrLeggiDati.HasRows = True Then
                            While dtrLeggiDati.Read
                                Writer.WriteLine(dtrLeggiDati("Nome") & " " & dtrLeggiDati("Codice") & " " & dtrLeggiDati("CAP") & " " & dtrLeggiDati("Indirizzo") & " " & dtrLeggiDati("Comune") & " (" & dtrLeggiDati("Provincia") & ") " & "\par")
                                'Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                                'Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                                'Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                                'Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                                'Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                'Writer.WriteLine("\cellx3300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                'Writer.WriteLine("\cellx6670\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                'Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\b \cell " & dtrLeggiDati("Nome") & " - " & dtrLeggiDati("Indirizzo") & " " & dtrLeggiDati("Civico") & dtrLeggiDati("CAP") & " " & dtrLeggiDati("Comune") & " (" & dtrLeggiDati("Provincia") & ") - " & dtrLeggiDati("Codice") & "\b0\cell\row\pard\f2\fs20")
                                'xLinea = "\b " & dtrLeggiDati("Titolo") & "\tqc " & dtrLeggiDati("CodiceEnte") & "\tqc " & dtrLeggiDati("DatainizioPrevista") & "\b0\par"
                                'Writer.WriteLine(xLinea)
                            End While
                            'Writer.WriteLine("\par")
                        End If
                    End If

                    If InStr(xLinea, "<BreakPoint2>") > 0 Then
                        CaricaElencoElencoRisorseEsclusi()
                        xLinea = Replace(xLinea, "<BreakPoint2>", "")
                        If dtrLeggiDati.HasRows = True Then
                            While dtrLeggiDati.Read
                                Writer.WriteLine(dtrLeggiDati("Nome") & " " & dtrLeggiDati("Cognome") & " " & dtrLeggiDati("DataNascita") & " " & dtrLeggiDati("Comune") & " " & dtrLeggiDati("Ruolo") & "\par")
                                'Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                                'Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                                'Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                                'Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                                'Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                'Writer.WriteLine("\cellx3300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                'Writer.WriteLine("\cellx6670\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                'Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\b \cell " & dtrLeggiDati("Nome") & " - " & dtrLeggiDati("Indirizzo") & " " & dtrLeggiDati("Civico") & dtrLeggiDati("CAP") & " " & dtrLeggiDati("Comune") & " (" & dtrLeggiDati("Provincia") & ") - " & dtrLeggiDati("Codice") & "\b0\cell\row\pard\f2\fs20")
                                'xLinea = "\b " & dtrLeggiDati("Titolo") & "\tqc " & dtrLeggiDati("CodiceEnte") & "\tqc " & dtrLeggiDati("DatainizioPrevista") & "\b0\par"
                                'Writer.WriteLine(xLinea)
                            End While
                            'Writer.WriteLine("\par")
                        End If
                    End If

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            AllegatoBdeterminazione = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function LetteraApprovazioneGraduatoria(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)

                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                '------------------------------------------------------------------------
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '"AG01\par}}")
                '-----------------------------------------------------------------------
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\qc\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'apro il template
                xLinea = Reader.ReadLine()

                Dim strDenominazioneEnte As String = dtrLeggiDati("Denominazione")
                Dim strClasseRichiesta As String = dtrLeggiDati("ClasseRichiesta") & "^"
                Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")
                Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strNumeroCivico As String = dtrLeggiDati("Civico")
                Dim strCAP As String = dtrLeggiDati("CAP")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim strProvincia As String = dtrLeggiDati("provincia")
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceRegione")

                CaricaProgetti()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<DataCostituzione>", strDataCostituzione)
                    xLinea = Replace(xLinea, "<Indirizzo>", strIndirizzo)
                    xLinea = Replace(xLinea, "<NumeroCivico>", strNumeroCivico)
                    xLinea = Replace(xLinea, "<CAP>", strCAP)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Provincia>", strProvincia)
                    xLinea = Replace(xLinea, "<CodiceRegione>", strCodiceRegione)

                    Dim intX As Integer

                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        xLinea = Replace(xLinea, "<BreakPoint>", "") & "\par"
                        If dtrLeggiDati.HasRows = True Then
                            intX = dtrLeggiDati.FieldCount
                            While dtrLeggiDati.Read
                                Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                                Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                                Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                                Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                                Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                Writer.WriteLine("\cellx3300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                Writer.WriteLine("\cellx6670\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\b " & dtrLeggiDati("Titolo") & "\cell " & dtrLeggiDati("CodiceEnte") & "\cell " & dtrLeggiDati("DatainizioPrevista") & "\b0\cell\row\pard\f2\fs20")
                                'xLinea = "\b " & dtrLeggiDati("Titolo") & "\tqc " & dtrLeggiDati("CodiceEnte") & "\tqc " & dtrLeggiDati("DatainizioPrevista") & "\b0\par"
                                'Writer.WriteLine(xLinea)
                            End While
                            Writer.WriteLine("\par")
                        End If
                    End If

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                ''inserireblocco progetti
                'CaricaProgetti()
                'If dtrLeggiDati.HasRows = True Then
                '    While dtrLeggiDati.Read
                '        xLinea = (dtrLeggiDati("Titolo") & vbTab & dtrLeggiDati("CodiceEnte") & vbTab & dtrLeggiDati("DatainizioPrevista")) & vbCrLf

                '        Writer.WriteLine(xLinea)
                '    End While
                'End If

                ''apro il secondo template
                'xLinea = Reader_SecondaParte.ReadLine()

                'While xLinea <> ""

                '    Writer.WriteLine(xLinea)

                '    xLinea = Reader_SecondaParte.ReadLine()
                'End While



                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraApprovazioneGraduatoria = "documentazione/" & strNomeFile


            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function LetteraAccreditamentoPositivo(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Lettera Adeguamento Positivo Negativo
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "AA02\par}}")
                'Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                '                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraAccreditamentoPositivo = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function

    Function LetteraAccreditamentoNegativo(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Lettera Adeguamento Positivo Negativo
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                'Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '               "AA02\par}}")
                'Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                '                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            LetteraAccreditamentoNegativo = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function

    Function DeterminaAccreditamentoPositivoconLimiti(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Determina Adeguamento Positivo 
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "AA02A\par}}")
                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "<Sediacc>", dtrLeggiDati("sediacc"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            DeterminaAccreditamentoPositivoconLimiti = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function

    Function DeterminaAccreditamentoNegativo(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Determina Adeguamento Positivo 
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "AA02A\par}}")
                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "<Sediacc>", dtrLeggiDati("sediacc"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            DeterminaAccreditamentoNegativo = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function

    Function DeterminaAccreditamentoPositivo(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        'Implementazione stampa generata da Alessandra Taballione il 16/06/2005
        'Determina Adeguamento Positivo 
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            'parla da sola
            CaricaDati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"), System.Text.Encoding.UTF7, False)
                Writer = New StreamWriter(strPercorsoFile)

                Writer.WriteLine("{\rtf1")

                ''Write the color table (for use in background and foreground colors)
                'xLinea = "{\colortbl;\red0\green0\blue0;\red0\green0\blue255;" & _
                '       "\red0\green255\blue255;\red0\green255\blue0;" & _
                '       "\red255\green0\blue255;\red255\green0\blue0;" & _
                '       "\red255\green255\blue0;\red255\green255\blue255;}"
                'Writer.WriteLine(xLinea)

                ''Write the title and author for the document properties
                'Writer.WriteLine("{\info{\title Sample RTF Document}" & _
                '                 "{\author Microsoft Developer Support}}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "IST004\par}}")
                '"INST003\par}{\fs18\chdate\par}\par\par}")
                'Writer.WriteLine("{\footer\pard\ql\brdrt\brdrs\brdrw10\brsp100" & _
                '                 "\fs18 Page " & _
                '                 "{\field{\*\fldinst PAGE}{\fldrslt 1}} of " & _
                '                 "{\field{\*\fldinst NUMPAGES}{\fldrslt 1}} \par}")

                'Write the page header and footer
                'Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                '                "AA02A\par}}")
                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")

                'apro il template
                xLinea = Reader.ReadLine()

                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", dtrLeggiDati("Denominazione"))
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", dtrLeggiDati("ClasseRichiesta") & "^")
                    xLinea = Replace(xLinea, "<DataCostituzione>", dtrLeggiDati("DataCostituzione"))
                    xLinea = Replace(xLinea, "<Indirizzo>", dtrLeggiDati("Indirizzo"))
                    xLinea = Replace(xLinea, "<NumeroCivico>", dtrLeggiDati("Civico"))
                    xLinea = Replace(xLinea, "<CAP>", dtrLeggiDati("CAP"))
                    xLinea = Replace(xLinea, "<Comune>", dtrLeggiDati("Comune"))
                    xLinea = Replace(xLinea, "<Provincia>", dtrLeggiDati("provincia"))
                    xLinea = Replace(xLinea, "<CodiceRegione>", dtrLeggiDati("CodiceRegione"))
                    xLinea = Replace(xLinea, "<Sediacc>", dtrLeggiDati("sediacc"))
                    xLinea = Replace(xLinea, "_", " ")

                    ''Write a sentence in the first paragraph of the document
                    'Writer.WriteLine("\par\fs24\cf2 This is a sample \b RTF \b0 " & _
                    '                 "document created with ASP.\cf0")

                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While


                'Writer.WriteLine("}") 'End Table

                'Add a page break and then a new paragraph
                'Writer.WriteLine("\par \page")
                'Writer.WriteLine("\pard\fs18\cf2\qc " & _
                '                 "This sample provided by Microsoft Developer Support.")

                'close the RTF string and file
                Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            DeterminaAccreditamentoPositivo = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',0)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Function
    Sub SalvaFascicolo()
        Dim strsql As String
        'vado a fare update ma si legge ;)

        Dim cmdupdate As Data.SqlClient.SqlCommand
        strsql = "update enti set codicefascicolo=' " & TxtNumeroFascicolo.Text & _
        "', idfascicolo ='" & TxtCodiceFasc.Value & "', descrfascicolo='" & txtDescFasc.Text.Replace("'", "''") & "'"
        strsql = strsql & "  where idente = " & Session("IdEnte")

        cmdupdate = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdupdate.ExecuteNonQuery()
        cmdupdate.Dispose()

    End Sub
    Sub Servizi()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "SELECT sistemi.idsistema,"
        strsql = strsql & " sistemi.sistema,"
        strsql = strsql & " Enti.denominazione,"
        strsql = strsql & " Enti.Codiceregione,"
        strsql = strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato, "
        strsql = strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then '<img src=images/canc.jpg title=""Cancella"" style=""cursor: hand"" onclick=""javascript: CancellaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' end as Elimina "
        strsql = strsql & " FROM entisistemi "
        strsql = strsql & " inner join sistemi on sistemi.idsistema=entisistemi.idsistema "
        strsql = strsql & " inner join Enti on enti.idente=entisistemi.idente "
        strsql = strsql & " INNER Join "
        strsql = strsql & " EntiAcquisizioneServizi ON entisistemi.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema "
        strsql = strsql & " WHERE   (EntiAcquisizioneServizi.idEnteSecondario = " & Session("idEnte") & ")"
        strsql = strsql & " UNION "
        strsql = strsql & " SELECT 0,"
        strsql = strsql & " 'Formazione' as sistema,"
        strsql = strsql & " 'Regione' as denominazione,"
        strsql = strsql & " 'Regione' as Codiceregione,"
        strsql = strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato, "
        strsql = strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then '<img src=images/canc.jpg title=""Cancella"" style=""cursor: hand"" onclick=""javascript: CancellaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' end as Elimina "
        strsql = strsql & " FROM Enti "
        strsql = strsql & " INNER Join "
        strsql = strsql & " EntiAcquisizioneServizi ON Enti.IDEnte = EntiAcquisizioneServizi.IdEnteSecondario "
        strsql = strsql & " INNER Join "
        strsql = strsql & " statiEnti ON enti.IDstatoEnte=statienti.idstatoente "
        strsql = strsql & " WHERE EntiAcquisizioneServizi.IdEnteSistema IS NULL AND (EntiAcquisizioneServizi.idEnteSecondario = '" & Session("idEnte") & "')"
        TBLLeggiDati = ClsServer.CreaDataTable(strsql, False, Session("conn"))


    End Sub

    Private Sub imgChiudi_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgChiudi.Click
        If (HdValoreSalva.Value = 1 Or hddModificaProtocollo.Value = 1) Then
            Call Salvataggio()
        End If
        'Response.Redirect("WfrmMain.aspx")
    End Sub
    Sub caricafascicolo()
        Dim dtrUtilizzato As SqlClient.SqlDataReader
        Dim strsql As String

        strsql = "Select codicefascicolo,idfascicolo,descrfascicolo  from "
        strsql = strsql & "enti where idente = " & Session("IdEnte")
        dtrUtilizzato = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrUtilizzato.HasRows = True Then
            dtrUtilizzato.Read()

            If IsDBNull(dtrUtilizzato("codicefascicolo")) = False Then
                TxtNumeroFascicolo.Text = Trim(dtrUtilizzato("codicefascicolo"))
                'txtNumFasc.Value = Trim(dtrUtilizzato("codicefascicolo"))
                TxtNumFascicoloControllo.Text = Trim(dtrUtilizzato("codicefascicolo"))

                TxtCodiceFasc.Value = dtrUtilizzato("idfascicolo")
                txtDescFasc.Text = dtrUtilizzato("descrfascicolo")
                'TxtDesFasc.Value = dtrUtilizzato("descrfascicolo")
            Else
                TxtNumeroFascicolo.Text = ""
                TxtNumFascicoloControllo.Text = ""
                TxtCodiceFasc.Value = ""
                txtDescFasc.Text = ""
            End If
        End If
        If Not dtrUtilizzato Is Nothing Then
            dtrUtilizzato.Close()
            dtrUtilizzato = Nothing
        End If
        '********
    End Sub

    Sub cancella()
        Dim strLocal As String
        Dim dtrCancellazione As SqlClient.SqlDataReader
        Dim mycommand As New SqlClient.SqlCommand
        Dim mydatatable As New DataTable

        mycommand.Connection = Session("conn")

        'cancella
        'strLocal = "select IDAttivitàSedeAssegnazione from attività inner join attivitàsediassegnazione on " & _
        '"attività.idattività = attivitàsediassegnazione.idattività where(attività.idattività = " & Request.QueryString("IdAttivita") & ")"
        Try
            'mydatatable = ClsServer.CreaDataTable(strLocal, False, Session("conn"))

            'Dim k As Int16

            'For k = 0 To mydatatable.Rows.Count - 1
            strLocal = "update cronologiaentidocumenti set dataprot =null,  nprot = null where idente = " & CInt(Session("IdEnte")) & _
            " and tipodocumento = 0"


            mycommand.CommandText = strLocal
            mycommand.ExecuteNonQuery()
            'Next
            '*******************************************************************************
        Catch ex As Exception
            'Response.Write(ex.Message.ToString())
        End Try

        'strsql = "update bandiattività set codicefascicoloai=' " & TxtNumeroFascicolo.Text & _
        '       "', idfascicoloai ='" & TxtCodiceFasc.Text & "', descrfascicoloai='" & txtDescFasc.Text
        'strsql = strsql & "' where idbando =" & ddlBando.SelectedValue & " and idente = " & Session("IdEnte")

        'mycommand.CommandText = strLocal
        ' mycommand.ExecuteNonQuery()

    End Sub


    Sub SalvaProtocolli(ByVal StrDataprot As String, ByVal StrNumProt As String, ByVal StringaDocumento As String)


        Dim strsql As String
        'vado a fare update
        Dim cmdupdate As Data.SqlClient.SqlCommand
        If StrDataprot = "" Then
            strsql = "update cronologiaentidocumenti set dataprot= null, nprot= null"
        Else
            strsql = "update cronologiaentidocumenti set dataprot='" & StrDataprot & "', nprot='" & StrNumProt & "'," & "UserName ='" & Session("Utente") & "' "
        End If
        strsql = strsql & " WHERE (idente ='" & CInt(Session("IdEnte")) & _
            "') and (documento ='" & StringaDocumento & "')"

        cmdupdate = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdupdate.ExecuteNonQuery()
        cmdupdate.Dispose()


        'Dim strsql As String
        'Dim mydatatable As New DataTable
        'Dim mycommand As New SqlClient.SqlCommand

        ''vado a fare update
        'Dim cmdupdate As Data.SqlClient.SqlCommand
        'If StrDataprot = "" Then
        '    Exit Sub
        'End If

        'strsql = "SELECT MAX(IdCronologiaEnteDocumento) AS Expr1 FROM(CronologiaEntiDocumenti) " & _
        '"WHERE (UserName ='" & Session("Utente") & "') AND (idente ='" & CInt(Session("IdEnte")) & _
        '"') and (documento ='" & StringaDocumento & "')"

        'Try
        '    mydatatable = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        '    If mydatatable.Rows.Count <> 0 Then
        '        ' strsql = "INSERT INTO CronologiaProtocolliAccreditamento (IdCronologiaEnteDocumento , DataProt , NProt , username) " & _
        '        '"VALUES (" & mydatatable.Rows(0).Item("Expr1") & "," & StrDataprot & "," & StrNumProt & "'" & Session("Utente") & "')"

        '        mycommand.CommandText = strsql
        '        mycommand.ExecuteNonQuery()
        '    End If
        '    '*******************************************************************************
        'Catch ex As Exception
        '    'Response.Write(ex.Message.ToString())
        'End Try

    End Sub

    Sub CaricaProtocollo()

        Dim dtrUtilizzato As SqlClient.SqlDataReader
        Dim strsql As String
        strsql = "select distinct documento,dataprot,nprot from cronologiaentidocumenti where idente = " & CInt(Session("IdEnte"))

        dtrUtilizzato = ClsServer.CreaDatareader(strsql, Session("conn"))

        'Gruppo1(False)
        'Gruppo2(False)
        'Gruppo3(False)
        'Gruppo4(False)
        'Gruppo5(False)
        'Gruppo6(False)
        'Gruppo7(False)
        'Gruppo8(False)
        'Gruppo9(False)
        'Gruppo10(False)
        'Gruppo11(False)
        'Gruppo12(False)
        'Gruppo13(False)
        'Gruppo14(False)
        'Gruppo15(False)
        'Gruppo16(False)
        'Gruppo17(False)
        'Gruppo18(False)
        'Gruppo19(False)
        'Gruppo20(False)
        'Gruppo21(False)
        'Gruppo22(False)
        'Gruppo23(False)
        'Gruppo24(False)
        'Gruppo25(False)
        'Gruppo26(False)
        'Gruppo27(False)


        'Do While dtrUtilizzato.Read()
        '    Select Case dtrUtilizzato("Documento")

        '        Case "letteraavvioprocedimento"
        '            Gruppo1(True)
        '            TxtNumProtocollo1.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo1.Text = "" & dtrUtilizzato("DataProt")

        '        Case "LetteraCompleDocu"
        '            Gruppo2(True)
        '            TxtNumProtocollo2.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo2.Text = "" & dtrUtilizzato("DataProt")

        '        Case "letteraavvioprocedimentoAdeg"
        '            Gruppo15(True)
        '            TxtNumProtocollo15.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo15.Text = "" & dtrUtilizzato("DataProt")

        '        Case "LetteraCompleDocuAdeg"
        '            Gruppo16(True)
        '            TxtNumProtocollo16.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo16.Text = "" & dtrUtilizzato("DataProt")

        '        Case "letteraadegpositivoenegativo"
        '            Gruppo20(True)
        '            TxtNumProtocollo20.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo20.Text = "" & dtrUtilizzato("DataProt")

        '        Case "DeterminaAdeguamentoPositivo"
        '            Gruppo21(True)
        '            TxtNumProtocollo21.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo21.Text = "" & dtrUtilizzato("DataProt")


        '        Case "DeterminaAccreditamentoPositivoArt10"
        '            Gruppo9(True)
        '            TxtNumProtocollo9.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo9.Text = "" & dtrUtilizzato("DataProt")

        '        Case "DeterminaAdeguamentoPositivoArt10"
        '            Gruppo22(True)
        '            TxtNumProtocollo22.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo22.Text = "" & dtrUtilizzato("DataProt")

        '        Case "DeterminaAdeguamentoPositivoconLimiti"
        '            Gruppo23(True)
        '            TxtNumProtocollo23.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo23.Text = "" & dtrUtilizzato("DataProt")

        '        Case "DeterminaAdeguamentoNegativo"
        '            Gruppo24(True)
        '            TxtNumProtocollo24.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo24.Text = "" & dtrUtilizzato("DataProt")

        '        Case "allegatoA1Adeguamento"
        '            Gruppo25(True)
        '            TxtNumProtocollo25.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo25.Text = "" & dtrUtilizzato("DataProt")

        '        Case "allegatoA2Adeguamento"
        '            Gruppo26(True)
        '            TxtNumProtocollo26.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo26.Text = "" & dtrUtilizzato("DataProt")

        '        Case "allegatobAdeguamento"
        '            Gruppo27(True)
        '            TxtNumProtocollo27.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo27.Text = "" & dtrUtilizzato("DataProt")

        '        Case "letteraaccreditamentopositivo"
        '            Gruppo6(True)
        '            TxtNumProtocollo6.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo6.Text = "" & dtrUtilizzato("DataProt")

        '        Case "letteraaccreditamentonegativo"
        '            Gruppo7(True)
        '            TxtNumProtocollo7.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo7.Text = "" & dtrUtilizzato("DataProt")

        '        Case "determinaaccreditamentopositivoconlimiti"
        '            Gruppo10(True)
        '            TxtNumProtocollo10.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo10.Text = "" & dtrUtilizzato("DataProt")

        '        Case "determinaaccreditamentonegativo"
        '            Gruppo11(True)
        '            TxtNumProtocollo11.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo11.Text = "" & dtrUtilizzato("DataProt")

        '        Case "determinaaccreditamentopositivo"
        '            Gruppo8(True)
        '            TxtNumProtocollo8.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo8.Text = "" & dtrUtilizzato("DataProt")

        '        Case "Articolo 2 Accreditamento"
        '            Gruppo4(True)
        '            TxtNumProtocollo4.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo4.Text = "" & dtrUtilizzato("DataProt")


        '        Case "art10accreditamento "
        '            Gruppo5(True)
        '            TxtNumProtocollo5.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo5.Text = "" & dtrUtilizzato("DataProt")

        '        Case "Articolo 2 Adeguamento"
        '            Gruppo18(True)
        '            TxtNumProtocollo18.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo18.Text = "" & dtrUtilizzato("DataProt")

        '        Case "art10adeguamento"
        '            Gruppo19(True)
        '            TxtNumProtocollo19.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo19.Text = "" & dtrUtilizzato("DataProt")

        '        Case "allegatoA1"
        '            Gruppo12(True)
        '            TxtNumProtocollo12.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo12.Text = "" & dtrUtilizzato("DataProt")

        '        Case "allegatoA2"
        '            Gruppo13(True)
        '            TxtNumProtocollo13.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo13.Text = "" & dtrUtilizzato("DataProt")

        '        Case "allegatob"
        '            Gruppo14(True)
        '            TxtNumProtocollo14.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo14.Text = "" & dtrUtilizzato("DataProt")

        '        Case "FalseSedi"
        '            Gruppo3(True)
        '            TxtNumProtocollo3.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo3.Text = "" & dtrUtilizzato("DataProt")

        '        Case "FalseSediAdeg"
        '            Gruppo17(True)
        '            TxtNumProtocollo17.Text = "" & dtrUtilizzato("NProt")
        '            TxtDataProtocollo17.Text = "" & dtrUtilizzato("DataProt")
        '    End Select
        'Loop

        ''mettere il visible sull'attivazione della freccietta per stampare

        If Not dtrUtilizzato Is Nothing Then
            dtrUtilizzato.Close()
            dtrUtilizzato = Nothing
        End If

    End Sub


    'Private Sub Gruppo1(ByVal BlValore As Boolean)
    '    LblNumProtocollo1.Visible = BlValore
    '    If TxtNumProtocollo1.Visible = False Then
    '        TxtNumProtocollo1.Text = ""
    '        TxtDataProtocollo1.Text = ""
    '    End If
    '    TxtNumProtocollo1.Visible = BlValore
    '    cmdSelProtocollo1.Visible = BlValore
    '    cmdScAllegati1.Visible = BlValore
    '    cmdNuovoFascicolo1.Visible = BlValore
    '    LblDataProtocollo1.Visible = BlValore
    '    TxtDataProtocollo1.Visible = BlValore

    'End Sub
    'Private Sub Gruppo2(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo2.Visible = False Then
    '        TxtNumProtocollo2.Text = ""
    '        TxtDataProtocollo2.Text = ""
    '    End If
    '    LblNumProtocollo2.Visible = BlValore
    '    TxtNumProtocollo2.Visible = BlValore
    '    cmdSelProtocollo2.Visible = BlValore
    '    cmdScAllegati2.Visible = BlValore
    '    cmdNuovoFascicolo2.Visible = BlValore
    '    LblDataProtocollo2.Visible = BlValore
    '    TxtDataProtocollo2.Visible = BlValore

    'End Sub
    'Private Sub Gruppo3(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo3.Visible = False Then
    '        TxtNumProtocollo3.Text = ""
    '        TxtDataProtocollo3.Text = ""
    '    End If
    '    LblNumProtocollo3.Visible = BlValore
    '    TxtNumProtocollo3.Visible = BlValore
    '    cmdSelProtocollo3.Visible = BlValore
    '    cmdScAllegati3.Visible = BlValore
    '    cmdNuovoFascicolo3.Visible = BlValore
    '    LblDataProtocollo3.Visible = BlValore
    '    TxtDataProtocollo3.Visible = BlValore

    'End Sub
    'Private Sub Gruppo4(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo4.Visible = False Then
    '        TxtNumProtocollo4.Text = ""
    '        TxtDataProtocollo4.Text = ""
    '    End If
    '    LblNumProtocollo4.Visible = BlValore
    '    TxtNumProtocollo4.Visible = BlValore
    '    cmdSelProtocollo4.Visible = BlValore
    '    cmdScAllegati4.Visible = BlValore
    '    cmdNuovoFascicolo4.Visible = BlValore
    '    LblDataProtocollo4.Visible = BlValore
    '    TxtDataProtocollo4.Visible = BlValore

    'End Sub
    'Private Sub Gruppo5(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo5.Visible = False Then
    '        TxtNumProtocollo5.Text = ""
    '        TxtDataProtocollo5.Text = ""
    '    End If
    '    LblNumProtocollo5.Visible = BlValore
    '    TxtNumProtocollo5.Visible = BlValore
    '    cmdSelProtocollo5.Visible = BlValore
    '    cmdScAllegati5.Visible = BlValore
    '    cmdNuovoFascicolo5.Visible = BlValore
    '    LblDataProtocollo5.Visible = BlValore
    '    TxtDataProtocollo5.Visible = BlValore

    'End Sub
    'Private Sub Gruppo6(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo6.Visible = False Then
    '        TxtNumProtocollo6.Text = ""
    '        TxtDataProtocollo6.Text = ""
    '    End If
    '    LblNumProtocollo6.Visible = BlValore
    '    TxtNumProtocollo6.Visible = BlValore
    '    cmdSelProtocollo6.Visible = BlValore
    '    cmdScAllegati6.Visible = BlValore
    '    cmdNuovoFascicolo6.Visible = BlValore
    '    LblDataProtocollo6.Visible = BlValore
    '    TxtDataProtocollo6.Visible = BlValore

    'End Sub
    'Private Sub Gruppo7(ByVal BlValore As Boolean)
    '    LblNumProtocollo7.Visible = BlValore
    '    If TxtNumProtocollo7.Visible = False Then
    '        TxtNumProtocollo7.Text = ""
    '        TxtDataProtocollo7.Text = ""
    '    End If
    '    TxtNumProtocollo7.Visible = BlValore
    '    cmdSelProtocollo7.Visible = BlValore
    '    cmdScAllegati7.Visible = BlValore
    '    cmdNuovoFascicolo7.Visible = BlValore
    '    LblDataProtocollo7.Visible = BlValore
    '    TxtDataProtocollo7.Visible = BlValore

    'End Sub

    'Private Sub Gruppo8(ByVal BlValore As Boolean)
    '    LblNumProtocollo8.Visible = BlValore
    '    If TxtNumProtocollo8.Visible = False Then
    '        TxtNumProtocollo8.Text = ""
    '        TxtDataProtocollo8.Text = ""
    '    End If
    '    TxtNumProtocollo8.Visible = BlValore
    '    cmdSelProtocollo8.Visible = BlValore
    '    cmdScAllegati8.Visible = BlValore
    '    cmdNuovoFascicolo8.Visible = BlValore
    '    LblDataProtocollo8.Visible = BlValore
    '    TxtDataProtocollo8.Visible = BlValore

    'End Sub
    'Private Sub Gruppo9(ByVal BlValore As Boolean)
    '    LblNumProtocollo9.Visible = BlValore
    '    If TxtNumProtocollo9.Visible = False Then
    '        TxtNumProtocollo9.Text = ""
    '        TxtDataProtocollo9.Text = ""
    '    End If
    '    TxtNumProtocollo9.Visible = BlValore
    '    cmdSelProtocollo9.Visible = BlValore
    '    cmdScAllegati9.Visible = BlValore
    '    cmdNuovoFascicolo9.Visible = BlValore
    '    LblDataProtocollo9.Visible = BlValore
    '    TxtDataProtocollo9.Visible = BlValore

    'End Sub

    'Private Sub Gruppo10(ByVal BlValore As Boolean)
    '    LblNumProtocollo10.Visible = BlValore
    '    If TxtNumProtocollo10.Visible = False Then
    '        TxtNumProtocollo10.Text = ""
    '        TxtDataProtocollo10.Text = ""
    '    End If
    '    TxtNumProtocollo10.Visible = BlValore
    '    cmdSelProtocollo10.Visible = BlValore
    '    cmdScAllegati10.Visible = BlValore
    '    cmdNuovoFascicolo10.Visible = BlValore
    '    LblDataProtocollo10.Visible = BlValore
    '    TxtDataProtocollo10.Visible = BlValore

    'End Sub

    'Private Sub Gruppo11(ByVal BlValore As Boolean)
    '    LblNumProtocollo11.Visible = BlValore
    '    If TxtNumProtocollo11.Visible = False Then
    '        TxtNumProtocollo11.Text = ""
    '        TxtDataProtocollo11.Text = ""
    '    End If
    '    TxtNumProtocollo11.Visible = BlValore
    '    cmdSelProtocollo11.Visible = BlValore
    '    cmdScAllegati11.Visible = BlValore
    '    cmdNuovoFascicolo11.Visible = BlValore
    '    LblDataProtocollo11.Visible = BlValore
    '    TxtDataProtocollo11.Visible = BlValore

    'End Sub

    'Private Sub Gruppo12(ByVal BlValore As Boolean)
    '    LblNumProtocollo12.Visible = BlValore
    '    If TxtNumProtocollo12.Visible = False Then
    '        TxtNumProtocollo12.Text = ""
    '        TxtDataProtocollo12.Text = ""
    '    End If
    '    TxtNumProtocollo12.Visible = BlValore
    '    cmdSelProtocollo12.Visible = BlValore
    '    cmdScAllegati12.Visible = BlValore
    '    cmdNuovoFascicolo12.Visible = BlValore
    '    LblDataProtocollo12.Visible = BlValore
    '    TxtDataProtocollo12.Visible = BlValore

    'End Sub

    'Private Sub Gruppo13(ByVal BlValore As Boolean)
    '    LblNumProtocollo13.Visible = BlValore
    '    If TxtNumProtocollo13.Visible = False Then
    '        TxtNumProtocollo13.Text = ""
    '        TxtDataProtocollo13.Text = ""
    '    End If
    '    TxtNumProtocollo13.Visible = BlValore
    '    cmdSelProtocollo13.Visible = BlValore
    '    cmdScAllegati13.Visible = BlValore
    '    cmdNuovoFascicolo13.Visible = BlValore
    '    LblDataProtocollo13.Visible = BlValore
    '    TxtDataProtocollo13.Visible = BlValore

    'End Sub

    'Private Sub Gruppo14(ByVal BlValore As Boolean)
    '    LblNumProtocollo14.Visible = BlValore
    '    If TxtNumProtocollo14.Visible = False Then
    '        TxtNumProtocollo14.Text = ""
    '        TxtDataProtocollo14.Text = ""
    '    End If
    '    TxtNumProtocollo14.Visible = BlValore
    '    cmdSelProtocollo14.Visible = BlValore
    '    cmdScAllegati14.Visible = BlValore
    '    cmdNuovoFascicolo14.Visible = BlValore
    '    LblDataProtocollo14.Visible = BlValore
    '    TxtDataProtocollo14.Visible = BlValore

    'End Sub
    'Private Sub Gruppo15(ByVal BlValore As Boolean)
    '    LblNumProtocollo15.Visible = BlValore
    '    If TxtNumProtocollo15.Visible = False Then
    '        TxtNumProtocollo15.Text = ""
    '        TxtDataProtocollo15.Text = ""
    '    End If
    '    TxtNumProtocollo15.Visible = BlValore
    '    cmdSelProtocollo15.Visible = BlValore
    '    cmdScAllegati15.Visible = BlValore
    '    cmdNuovoFascicolo15.Visible = BlValore
    '    LblDataProtocollo15.Visible = BlValore
    '    TxtDataProtocollo15.Visible = BlValore

    'End Sub
    'Private Sub Gruppo16(ByVal BlValore As Boolean)
    '    LblNumProtocollo16.Visible = BlValore
    '    If TxtNumProtocollo16.Visible = False Then
    '        TxtNumProtocollo16.Text = ""
    '        TxtDataProtocollo16.Text = ""
    '    End If
    '    TxtNumProtocollo16.Visible = BlValore
    '    cmdSelProtocollo16.Visible = BlValore
    '    cmdScAllegati16.Visible = BlValore
    '    cmdNuovoFascicolo16.Visible = BlValore
    '    LblDataProtocollo16.Visible = BlValore
    '    TxtDataProtocollo16.Visible = BlValore

    'End Sub
    'Private Sub Gruppo17(ByVal BlValore As Boolean)
    '    LblNumProtocollo17.Visible = BlValore
    '    If TxtNumProtocollo17.Visible = False Then
    '        TxtNumProtocollo17.Text = ""
    '        TxtDataProtocollo17.Text = ""
    '    End If
    '    TxtNumProtocollo17.Visible = BlValore
    '    cmdSelProtocollo17.Visible = BlValore
    '    cmdScAllegati17.Visible = BlValore
    '    cmdNuovoFascicolo17.Visible = BlValore
    '    LblDataProtocollo17.Visible = BlValore
    '    TxtDataProtocollo17.Visible = BlValore

    'End Sub
    'Private Sub Gruppo18(ByVal BlValore As Boolean)
    '    LblNumProtocollo18.Visible = BlValore
    '    If TxtNumProtocollo18.Visible = False Then
    '        TxtNumProtocollo18.Text = ""
    '        TxtDataProtocollo18.Text = ""
    '    End If
    '    TxtNumProtocollo18.Visible = BlValore
    '    cmdSelProtocollo18.Visible = BlValore
    '    cmdScAllegati18.Visible = BlValore
    '    cmdNuovoFascicolo18.Visible = BlValore
    '    LblDataProtocollo18.Visible = BlValore
    '    TxtDataProtocollo18.Visible = BlValore

    'End Sub
    'Private Sub Gruppo19(ByVal BlValore As Boolean)
    '    LblNumProtocollo19.Visible = BlValore
    '    If TxtNumProtocollo19.Visible = False Then
    '        TxtNumProtocollo19.Text = ""
    '        TxtDataProtocollo19.Text = ""
    '    End If
    '    TxtNumProtocollo19.Visible = BlValore
    '    cmdSelProtocollo19.Visible = BlValore
    '    cmdScAllegati19.Visible = BlValore
    '    cmdNuovoFascicolo19.Visible = BlValore
    '    LblDataProtocollo19.Visible = BlValore
    '    TxtDataProtocollo19.Visible = BlValore

    'End Sub
    'Private Sub Gruppo20(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo20.Visible = False Then
    '        TxtNumProtocollo20.Text = ""
    '        TxtDataProtocollo20.Text = ""
    '    End If
    '    LblNumProtocollo20.Visible = BlValore
    '    TxtNumProtocollo20.Visible = BlValore
    '    cmdSelProtocollo20.Visible = BlValore
    '    cmdScAllegati20.Visible = BlValore
    '    cmdNuovoFascicolo20.Visible = BlValore
    '    LblDataProtocollo20.Visible = BlValore
    '    TxtDataProtocollo20.Visible = BlValore

    'End Sub
    'Private Sub Gruppo21(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo21.Visible = False Then
    '        TxtNumProtocollo21.Text = ""
    '        TxtDataProtocollo21.Text = ""
    '    End If
    '    LblNumProtocollo21.Visible = BlValore
    '    TxtNumProtocollo21.Visible = BlValore
    '    cmdSelProtocollo21.Visible = BlValore
    '    cmdScAllegati21.Visible = BlValore
    '    cmdNuovoFascicolo21.Visible = BlValore
    '    LblDataProtocollo21.Visible = BlValore
    '    TxtDataProtocollo21.Visible = BlValore

    'End Sub
    'Private Sub Gruppo22(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo22.Visible = False Then
    '        TxtNumProtocollo22.Text = ""
    '        TxtDataProtocollo22.Text = ""
    '    End If
    '    LblNumProtocollo22.Visible = BlValore
    '    TxtNumProtocollo22.Visible = BlValore
    '    cmdSelProtocollo22.Visible = BlValore
    '    cmdScAllegati22.Visible = BlValore
    '    cmdNuovoFascicolo22.Visible = BlValore
    '    LblDataProtocollo22.Visible = BlValore
    '    TxtDataProtocollo22.Visible = BlValore

    'End Sub
    'Private Sub Gruppo23(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo23.Visible = False Then
    '        TxtNumProtocollo23.Text = ""
    '        TxtDataProtocollo23.Text = ""
    '    End If
    '    LblNumProtocollo23.Visible = BlValore
    '    TxtNumProtocollo23.Visible = BlValore
    '    cmdSelProtocollo23.Visible = BlValore
    '    cmdScAllegati23.Visible = BlValore
    '    cmdNuovoFascicolo23.Visible = BlValore
    '    LblDataProtocollo23.Visible = BlValore
    '    TxtDataProtocollo23.Visible = BlValore

    'End Sub
    'Private Sub Gruppo24(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo24.Visible = False Then
    '        TxtNumProtocollo24.Text = ""
    '        TxtDataProtocollo24.Text = ""
    '    End If
    '    LblNumProtocollo24.Visible = BlValore
    '    TxtNumProtocollo24.Visible = BlValore
    '    cmdSelProtocollo24.Visible = BlValore
    '    cmdScAllegati24.Visible = BlValore
    '    cmdNuovoFascicolo24.Visible = BlValore
    '    LblDataProtocollo24.Visible = BlValore
    '    TxtDataProtocollo24.Visible = BlValore

    'End Sub
    'Private Sub Gruppo25(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo25.Visible = False Then
    '        TxtNumProtocollo25.Text = ""
    '        TxtDataProtocollo25.Text = ""
    '    End If
    '    LblNumProtocollo25.Visible = BlValore
    '    TxtNumProtocollo25.Visible = BlValore
    '    cmdSelProtocollo25.Visible = BlValore
    '    cmdScAllegati25.Visible = BlValore
    '    cmdNuovoFascicolo25.Visible = BlValore
    '    LblDataProtocollo25.Visible = BlValore
    '    TxtDataProtocollo25.Visible = BlValore

    'End Sub
    'Private Sub Gruppo26(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo26.Visible = False Then
    '        TxtNumProtocollo26.Text = ""
    '        TxtDataProtocollo26.Text = ""
    '    End If
    '    LblNumProtocollo26.Visible = BlValore
    '    TxtNumProtocollo26.Visible = BlValore
    '    cmdSelProtocollo26.Visible = BlValore
    '    cmdScAllegati26.Visible = BlValore
    '    cmdNuovoFascicolo26.Visible = BlValore
    '    LblDataProtocollo26.Visible = BlValore
    '    TxtDataProtocollo26.Visible = BlValore

    'End Sub
    'Private Sub Gruppo27(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo27.Visible = False Then
    '        TxtNumProtocollo27.Text = ""
    '        TxtDataProtocollo27.Text = ""
    '    End If
    '    LblNumProtocollo27.Visible = BlValore
    '    TxtNumProtocollo27.Visible = BlValore
    '    cmdSelProtocollo27.Visible = BlValore
    '    cmdScAllegati27.Visible = BlValore
    '    cmdNuovoFascicolo27.Visible = BlValore
    '    LblDataProtocollo27.Visible = BlValore
    '    TxtDataProtocollo27.Visible = BlValore

    'End Sub

    Sub Salvataggio()
        'If TxtNumeroFascicolo.Text <> "" Then
        lblmessaggiosopra.Text = ""
        Call SalvaFascicolo()

        ''If TxtNumProtocollo1.Text <> "" Then
        'Call SalvaProtocolli(TxtDataProtocollo1.Text, TxtNumProtocollo1.Text, "letteraavvioprocedimento")
        ''End If
        ''If TxtNumProtocollo2.Text <> "" Then
        'Call SalvaProtocolli(TxtDataProtocollo2.Text, TxtNumProtocollo2.Text, "LetteraCompleDocu")
        ''End If
        ''If TxtNumProtocollo3.Text <> "" Then
        'Call SalvaProtocolli(TxtDataProtocollo3.Text, TxtNumProtocollo3.Text, "FalseSedi")
        ''End If
        ''If TxtNumProtocollo4.Text <> "" Then
        'Call SalvaProtocolli(TxtDataProtocollo4.Text, TxtNumProtocollo4.Text, "Articolo 2 Accreditamento")
        ''End If
        ''If TxtNumProtocollo5.Text <> "" Then
        'Call SalvaProtocolli(TxtDataProtocollo5.Text, TxtNumProtocollo5.Text, "art10accreditamento")
        ''End If
        ''If TxtNumProtocollo6.Text <> "" Then
        'Call SalvaProtocolli(TxtDataProtocollo6.Text, TxtNumProtocollo6.Text, "letteraaccreditamentopositivo")
        ''End If
        ''Else
        ''messaggio - sicuro di cancellare?
        ''End If
        'Call SalvaProtocolli(TxtDataProtocollo7.Text, TxtNumProtocollo7.Text, "letteraaccreditamentonegativo")

        'Call SalvaProtocolli(TxtDataProtocollo8.Text, TxtNumProtocollo8.Text, "determinaaccreditamentopositivo")

        'Call SalvaProtocolli(TxtDataProtocollo9.Text, TxtNumProtocollo9.Text, "DeterminaAccreditamentoPositivoArt10")

        'Call SalvaProtocolli(TxtDataProtocollo10.Text, TxtNumProtocollo10.Text, "determinaaccreditamentopositivoconlimiti")

        'Call SalvaProtocolli(TxtDataProtocollo11.Text, TxtNumProtocollo11.Text, "determinaaccreditamentonegativo")

        'Call SalvaProtocolli(TxtDataProtocollo12.Text, TxtNumProtocollo12.Text, "allegatoA1")

        'Call SalvaProtocolli(TxtDataProtocollo13.Text, TxtNumProtocollo13.Text, "allegatoA2")

        'Call SalvaProtocolli(TxtDataProtocollo14.Text, TxtNumProtocollo14.Text, "allegatob")

        'Call SalvaProtocolli(TxtDataProtocollo15.Text, TxtNumProtocollo15.Text, "letteraavvioprocedimentoAdeg")

        'Call SalvaProtocolli(TxtDataProtocollo16.Text, TxtNumProtocollo16.Text, "LetteraCompleDocuAdeg")

        'Call SalvaProtocolli(TxtDataProtocollo17.Text, TxtNumProtocollo17.Text, "FalseSediAdeg")

        'Call SalvaProtocolli(TxtDataProtocollo18.Text, TxtNumProtocollo18.Text, "Articolo 2 Adeguamento")

        'Call SalvaProtocolli(TxtDataProtocollo19.Text, TxtNumProtocollo19.Text, "art10adeguamento")

        'Call SalvaProtocolli(TxtDataProtocollo20.Text, TxtNumProtocollo20.Text, "letteraadegpositivoenegativo")

        'Call SalvaProtocolli(TxtDataProtocollo21.Text, TxtNumProtocollo21.Text, "DeterminaAdeguamentoPositivo")

        'Call SalvaProtocolli(TxtDataProtocollo22.Text, TxtNumProtocollo22.Text, "DeterminaAdeguamentoPositivoArt10")

        'Call SalvaProtocolli(TxtDataProtocollo23.Text, TxtNumProtocollo23.Text, "DeterminaAdeguamentoPositivoconLimiti")

        'Call SalvaProtocolli(TxtDataProtocollo24.Text, TxtNumProtocollo24.Text, "DeterminaAdeguamentoNegativo")

        'Call SalvaProtocolli(TxtDataProtocollo25.Text, TxtNumProtocollo25.Text, "allegatoA1Adeguamento")

        'Call SalvaProtocolli(TxtDataProtocollo26.Text, TxtNumProtocollo26.Text, "allegatoA2Adeguamento")

        'Call SalvaProtocolli(TxtDataProtocollo27.Text, TxtNumProtocollo27.Text, "allegatobAdeguamento")

        TxtNumFascicoloControllo.Text = Trim(TxtNumeroFascicolo.Text)
        hddModificaProtocollo.Value = 0
    End Sub

    Private Sub cmdFascCanc_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCanc.Click
        lblmessaggiosopra.Text = ""
        TxtNumeroFascicolo.Text = ""
        TxtCodiceFasc.Value = ""
        txtDescFasc.Text = ""
        Dim strsql As String
        'vado a fare update ma si legge ;)

        Dim cmdupdate As Data.SqlClient.SqlCommand
        strsql = "update enti set codicefascicolo= NULL, idfascicolo =NULL, descrfascicolo=NULL"
        strsql = strsql & "  where idente = " & Session("IdEnte")

        cmdupdate = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdupdate.ExecuteNonQuery()
        cmdupdate.Dispose()
        'TxtNumProtocollo1.Text = ""
        'TxtDataProtocollo1.Text = ""
        'TxtNumProtocollo2.Text = ""
        'TxtDataProtocollo2.Text = ""
        'TxtNumProtocollo3.Text = ""
        'TxtDataProtocollo3.Text = ""
        'TxtNumProtocollo4.Text = ""
        'TxtDataProtocollo4.Text = ""
        'TxtNumProtocollo5.Text = ""
        'TxtDataProtocollo5.Text = ""
        'TxtNumProtocollo6.Text = ""
        'TxtDataProtocollo6.Text = ""
        'TxtNumProtocollo7.Text = ""
        'TxtDataProtocollo7.Text = ""
        'TxtNumProtocollo8.Text = ""
        'TxtDataProtocollo8.Text = ""
        'TxtNumProtocollo9.Text = ""
        'TxtDataProtocollo9.Text = ""
        'TxtNumProtocollo10.Text = ""
        'TxtDataProtocollo10.Text = ""
        'TxtNumProtocollo11.Text = ""
        'TxtDataProtocollo11.Text = ""
        'TxtNumProtocollo12.Text = ""
        'TxtDataProtocollo12.Text = ""
        'TxtNumProtocollo13.Text = ""
        'TxtDataProtocollo13.Text = ""
        'TxtNumProtocollo14.Text = ""
        'TxtDataProtocollo14.Text = ""
        'TxtNumProtocollo15.Text = ""
        'TxtDataProtocollo15.Text = ""
        'TxtNumProtocollo16.Text = ""
        'TxtDataProtocollo16.Text = ""
        'TxtNumProtocollo17.Text = ""
        'TxtDataProtocollo17.Text = ""
        'TxtNumProtocollo18.Text = ""
        'TxtDataProtocollo18.Text = ""
        'TxtNumProtocollo19.Text = ""
        'TxtDataProtocollo19.Text = ""
        'TxtNumProtocollo20.Text = ""
        'TxtDataProtocollo20.Text = ""
        'TxtNumProtocollo21.Text = ""
        'TxtDataProtocollo21.Text = ""
        'TxtNumProtocollo22.Text = ""
        'TxtDataProtocollo22.Text = ""
        'TxtNumProtocollo23.Text = ""
        'TxtDataProtocollo23.Text = ""
        'TxtNumProtocollo24.Text = ""
        'TxtDataProtocollo24.Text = ""
        'TxtNumProtocollo25.Text = ""
        'TxtDataProtocollo25.Text = ""
        'TxtNumProtocollo26.Text = ""
        'TxtDataProtocollo26.Text = ""
        'TxtNumProtocollo27.Text = ""
        'TxtDataProtocollo27.Text = ""

        'salva i dati vuoti

    End Sub
    Sub CaricaFasiEnte(ByVal IdEnte As Integer)
        '** Aggiunto da Simona Cordella il 07/09/2015
        '** carico la combo con l'elenco delle fasi Valutate
        Dim strSql As String
        Dim dtsFase As DataSet

        strSql = " select 0 as identefase , '' Tipofase,'31/12/2050' as datafinefase"
        strSql &= " union "
        strSql &= " SELECT identefase,case TipoFase when 2 then 'Adeguamento:' end  + ' Rif. Fase n°.' + convert(varchar(10),IdEnteFase) + ' dal:' + dbo.formatodata (datainiziofase) + ' al:' + dbo.formatodata (datafinefase) as TipoFase, "
        strSql &= " datafinefase"
        strSql &= " FROM EntiFasi "
        strSql &= " WHERE IdEnte = " & IdEnte & "  and tipofase = 2 and stato in (3,4) "
        strSql &= " ORDER BY datafinefase desc"
        dtsFase = ClsServer.DataSetGenerico(strSql, Session("conn"))

        ddlFasi.DataSource = dtsFase

        ddlFasi.DataValueField = "identefase"
        ddlFasi.DataTextField = "TipoFase"
        ddlFasi.DataBind()

    End Sub

    Sub AbilitaStampeAdeguamentiTotale(ByVal IdEnte As Integer)
        '** Aggiunto da Simona Cordella il 30/11/2015
        '** controllo le fasi
        Dim strSql As String
        Dim dtRFase As SqlDataReader

        strSql = " SELECT identefase"
        strSql &= " FROM EntiFasi "
        strSql &= " WHERE IdEnte = " & IdEnte & " and tipofase = 2  "
        strSql &= " and case stato  When 1 then case when  GETDATE() between DataInizioFase and DataFineFase then 'Aperta' ELSE 'Scaduta' end  when 2 then 'Annullata' when 3 then  'Presentata'	when 4  then 'Valutata' end IN ('Aperta','Presentata')"
        'and tipofase = 2 and stato in (1,3) "
        dtRFase = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtRFase.HasRows = True Then
            ChiudiDataReader(dtRFase)
            StampeAdeguamentoTotali(False)
        Else
            ChiudiDataReader(dtRFase)
            'StampeAdeguamentoTotali(True)

            'preparo la query che verifica lo stato dell'ente
            strSql = "select StatoEnte from statienti "
            strSql = strSql & "inner join enti on statienti.idstatoente=enti.idstatoente "
            strSql = strSql & "where enti.idente='" & Session("IdEnte") & "'"

            'eseguo la query
            dtrLeggiDati = ClsServer.CreaDatareader(strSql, Session("conn"))
            'se ci sono righe vado a controllare lo stato
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                Select Case dtrLeggiDati("StatoEnte")
                    'se è attivo 
                    Case "Istruttoria"
                        StampeAdeguamentoTotali(False)
                    Case "Chiuso"
                        StampeAdeguamentoTotali(False)

                    Case Else
                        StampeAdeguamentoTotali(True)
                End Select
            End If
            ChiudiDataReader(dtrLeggiDati)


        End If
    End Sub

    Sub StampeAdeguamentoTotali(ByVal blnValore As Boolean)
        chkDetAdegPosTot.Enabled = blnValore
        lblDetAdegNegTot.Enabled = blnValore
        ChkDetAdegPosArt10Tot.Enabled = blnValore
        lblDetAdegPosArt10Tot.Enabled = blnValore
        chkDetAdegPosLimTot.Enabled = blnValore
        lblDetAdegPosLimTot.Enabled = blnValore
        chkDetAdegNegTot.Enabled = blnValore
        lblDetAdegNegTot.Enabled = blnValore
        chkAllegatoA1Tot.Enabled = blnValore
        lblAllegatoA1Tot.Enabled = blnValore
        chkAllegatoA2Tot.Enabled = blnValore
        lblAllegatoA2Tot.Enabled = blnValore
        chkAllegatoBTot.Enabled = blnValore
        lblAllegatoBTot.Enabled = blnValore
    End Sub

    'Private Function ControlloSelezioneFase() As Boolean

    '    If ddlFasi.SelectedValue = 0 Then
    '        lblmessaggiosopra.Visible = True
    '        lblmessaggiosopra.Text = "E' necessario selezionare una fase."
    '        lblmessaggiosopra.ForeColor = Color.Red
    '        Return False
    '    Else
    '        lblmessaggiosopra.ForeColor = Color.Black
    '        lblmessaggiosopra.Visible = False
    '        Return True
    '    End If
    'End Function

    Private Function ControlloSelezioneFase(Optional ByVal vlStato As Boolean = False) As Boolean
        ' Modificata da Luigi Leucci 28-02-2019
        Dim strSql As String
        Dim dtrFase As SqlDataReader
        Dim Ritorno As Boolean = False

        If ddlFasi.SelectedValue = 0 Then
            lblmessaggiosopra.Visible = True
            lblmessaggiosopra.Text = "E' necessario selezionare una fase."
            lblmessaggiosopra.ForeColor = Color.Red
        Else
            lblmessaggiosopra.ForeColor = Color.Black
            lblmessaggiosopra.Visible = False

            If vlStato Then
                strSql = "SELECT CASE Stato " & _
                         "           WHEN 1 THEN CASE " & _
                         "                          WHEN GETDATE() BETWEEN DataInizioFase and DataFineFase THEN 'Aperta' " & _
                         "                            ELSE 'Scaduta' " & _
                         "                          END " & _
                         "           WHEN 2 THEN 'Annullata' " & _
                         "           WHEN 3 THEN 'Presentata' " & _
                         "           WHEN 4 THEN 'Valutata'" & _
                         "      END AS Descrizione, Stato " & _
                         "FROM EntiFasi WHERE IDEnteFase = " & ddlFasi.SelectedValue
                ChiudiDataReader(dtrFase)
                dtrFase = ClsServer.CreaDatareader(strSql, Session("conn"))

                If dtrFase.HasRows = True Then
                    dtrFase.Read()
                    If dtrFase("Stato") <> 3 Then
                        lblmessaggiosopra.Visible = True
                        lblmessaggiosopra.Text = "La fase selezionata è " & dtrFase("Descrizione")
                        lblmessaggiosopra.ForeColor = Color.Red
                    Else
                        Ritorno = True
                    End If
                End If
                ChiudiDataReader(dtrFase)
            Else
                Ritorno = True
            End If
        End If
        Return Ritorno
    End Function
    Private Function RicavoIDEnteFaseAccreditamento(ByVal IdEnte As Integer) As Integer
        '** Aggiunto da Simona Cordella il 23/04/2018
        '** ricavo l'identefase dell'accredimante dell'ente per la registrazione nella cronologiaentidocumenti

        Dim strSql As String
        Dim dtrFase As SqlDataReader
        Dim IDEnteFase As Integer
        ChiudiDataReader(dtrFase)
        strSql = " SELECT identefase"
        strSql &= " FROM EntiFasi "
        strSql &= " WHERE IdEnte = " & IdEnte & " and tipofase = 1  "

        dtrFase = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrFase.HasRows = True Then
            dtrFase.Read()
            IDEnteFase = dtrFase("IDEnteFase")
        End If
        ChiudiDataReader(dtrFase)
        Return IDEnteFase
    End Function

    Protected Sub imgGeneraFile_Click(sender As Object, e As EventArgs) Handles imgGeneraFile.Click
        lblmessaggiosopra.Text = ""
        'controllo se è stato selezionato un ente
        If (Session("IdEnte") > -1) And (Not Session("IdEnte") Is Nothing) Then
            '******************************************************************************************************************************
            '******************************************************************************************************************************
            '******************************************************************************************************************************
            If chkComAvvioProd.Checked = True Then '1
                hpComAvvioProd.Visible = True
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))

                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hpComAvvioProd.NavigateUrl = Documento.ACCR_letteraavvioprocedimento(IdEnteFaseAcc, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("letteraavvioprocedimento", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo1(True)
                'End If
            End If

            If chkLetteraCompleDocu.Checked = True Then '2
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hpLetteraCompleDocu.Visible = True
                hpLetteraCompleDocu.NavigateUrl = Documento.ACCR_LetteraCompleDocu(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                'hpLetteraCompleDocu.NavigateUrl = LetteraCompleDocu(Session("IdEnte"), "LetteraCompleDocu")
                Documento.Dispose()

                NuovaCronologia("LetteraCompleDocu", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo2(True)
                'End If

            End If

            If chkComAvvioProdAdeg.Checked = True Then '15
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hpComAvvioProdAdeg.Visible = True
                'hpComAvvioProdAdeg.NavigateUrl = LetteraAvvioProcedimentoAdeg(Session("IdEnte"), "letteraavvioprocedimentoAdeg")
                hpComAvvioProdAdeg.NavigateUrl = Documento.ACCR_letteraavvioprocedimentoAdeg(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("letteraavvioprocedimentoAdeg", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo15(True)
                'End If
            End If

            If chkLetteraCompleDocuAdeg.Checked = True Then '16
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hpLetteraCompleDocuAdeg.Visible = True
                hpLetteraCompleDocuAdeg.NavigateUrl = Documento.ACCR_LetteraCompleDocuAdeg(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("LetteraCompleDocuAdeg", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo16(True)
                'End If
            End If

            'Aggiunto da Alessandra taballione il 16/06/2005
            'Aggiunta di altre 4 stampe
            'Lettera adeguamento positivo e negativo
            If chkLettAdegPosNeg.Checked = True Then '20
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplLettAdegPosNeg.Visible = True
                hplLettAdegPosNeg.NavigateUrl = Documento.ACCR_letteraadegpositivoenegativo(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("letteraadegpositivoenegativo", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo20(True)
                'End If
            End If
            'Determina Adeguamento Posistivo 
            If chkDetAdegPos.Checked = True Then '21
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplDetAdegPos.Visible = True
                hplDetAdegPos.NavigateUrl = Documento.ACCR_DeterminaAdeguamentoPositivo(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DeterminaAdeguamentoPositivo", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo21(True)
                'End If
            End If
            'Determina acrreditamento Positivo art 10
            'Aggiunto da Alessandra Taballione il 07/11/2005
            If chkDetAccrPosArt10.Checked = True Then '9 
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hplDetAccrPosArt10.Visible = True
                hplDetAccrPosArt10.NavigateUrl = Documento.ACCR_DeterminaAccreditamentoPositivoArt10(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DeterminaAccreditamentoPositivoArt10", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo9(True)
                'End If
            End If
            'Aggiunto da Alessandra Taballione il 07/11/2005
            'Determina Adeguamento Posistivo Art 10
            If ChkDetAdegPosArt10.Checked = True Then '22
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli

                HplDetAdegPosArt10.Visible = True
                HplDetAdegPosArt10.NavigateUrl = Documento.ACCR_DeterminaAdeguamentoPositivoArt10(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DeterminaAdeguamentoPositivoArt10", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo22(True)
                'End If
            End If
            'Determina Adeguamento Positivo con Limitazioni
            If chkDetAdegPosLim.Checked = True Then '23
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplDetAdegPosLim.Visible = True
                hplDetAdegPosLim.NavigateUrl = Documento.ACCR_DeterminaAdeguamentoPositivoconLimiti(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DeterminaAdeguamentoPositivoconLimiti", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo23(True)
                'End If
            End If
            'Determina Adeguamneto Negativo
            If chkDetAdegNeg.Checked = True Then '24
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplDetAdegNeg.Visible = True
                hplDetAdegNeg.NavigateUrl = Documento.ACCR_DeterminaAdeguamentoNegativo(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DeterminaAdeguamentoNegativo", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo24(True)
                'End If

            End If
            'allegato a1
            If chkAllegatoA1.Checked = True Then '25
                If ControlloSelezioneFase() = False Then Exit Sub
                hplAllegatoA1.Visible = True
                'hplAllegatoA1.NavigateUrl = AllegatoA1determinazione(Session("IdEnte"), "allegatoA1Adeguamento")
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplAllegatoA1.NavigateUrl = Documento.ACCR_allegatoA1Adeguamento(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatoA1Adeguamento", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo25(True)
                'End If

            End If
            'allegato a2
            If chkAllegatoA2.Checked = True Then '26
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplAllegatoA2.Visible = True
                hplAllegatoA2.NavigateUrl = Documento.ACCR_allegatoA2Adeguamento(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatoA2Adeguamento", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo26(True)
                'End If
            End If
            'allegato b
            If chkAllegatoB.Checked = True Then '27
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplAllegatoB.Visible = True
                hplAllegatoB.NavigateUrl = Documento.ACCR_allegatobAdeguamento(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatobAdeguamento", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo27(True)
                'End If
            End If
            'Aggiunto da Alessandra Taballione il 22/06/2005
            'Lettera accreditamento Positivo
            If chkLetAccPos.Checked = True Then '6 

                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hplLetAccPos.Visible = True
                hplLetAccPos.NavigateUrl = Documento.ACCR_letteraaccreditamentopositivo(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("letteraaccreditamentopositivo", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo6(True)
                'End If
            End If
            'Lettera accreditamento Negativo
            If chkLetAccNeg.Checked = True Then '7 
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hplLetAccNeg.Visible = True
                hplLetAccNeg.NavigateUrl = Documento.ACCR_letteraaccreditamentonegativo(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("letteraaccreditamentonegativo", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo7(True)
                'End If
            End If
            'Determina acrreditamento positivo con limiti sedi e figure
            If chkdetAccPosLim.Checked = True Then '10
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hpldetAccPosLim.Visible = True
                hpldetAccPosLim.NavigateUrl = Documento.ACCR_determinaaccreditamentopositivoconlimiti(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("determinaaccreditamentopositivoconlimiti", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo10(True)
                'End If
            End If
            'Determina acrreditamento positivo con limiti sedi o figure
            If chkdetAccPosLimSediFigure.Checked = True Then '10
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hpldetAccPosLimSediFigure.Visible = True
                hpldetAccPosLimSediFigure.NavigateUrl = Documento.ACCR_determinaaccreditamentopositivoconlimitiSedioFigure(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("determinaaccreditamentopositivoconlimitisediofigure", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo10(True)
                'End If
            End If
            'Determina acrreditamento Negativo
            If chkDetAccNeg.Checked = True Then '11
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hplDetAccNeg.Visible = True
                hplDetAccNeg.NavigateUrl = Documento.ACCR_determinaaccreditamentonegativo(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("determinaaccreditamentonegativo", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo11(True)
                'End If
            End If
            'Determina acrreditamento Positivo
            If chkDetAccPos.Checked = True Then '8
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hplDetAccPos.Visible = True
                hplDetAccPos.NavigateUrl = Documento.ACCR_determinaaccreditamentopositivo(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("determinaaccreditamentopositivo", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo8(True)
                'End If
            End If
            'articolo 2 II Step
            If ChkArt2IIstep.Checked = True Then '4 
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                HplArt2IIstep.Visible = True
                HplArt2IIstep.NavigateUrl = Documento.ACCR_art2accreditamento(IdEnteFaseAcc, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("Articolo 2 Iscrizione", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo4(True)
                'End If

            End If
            'articolo 10 II Step
            If chkart10IIstep.Checked = True Then '5
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hplart10IIstep.Visible = True
                hplart10IIstep.NavigateUrl = Documento.ACCR_art10accreditamento(IdEnteFaseAcc, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("art10accreditamento", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo5(True)
                'End If
            End If
            'articolo 2 Adeguamento
            If ChkArt2adeg.Checked = True Then '18
                If ControlloSelezioneFase(True) = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                HplArt2adeg.Visible = True
                HplArt2adeg.NavigateUrl = Documento.ACCR_art2adeguamento(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("Articolo 2 Adeguamento", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo18(True)
                'End If
            End If
            'articolo 10 Adeguamento
            If chkArt10adeg.Checked = True Then '19
                If ControlloSelezioneFase(True) = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplart10adeg.Visible = True
                hplart10adeg.NavigateUrl = Documento.ACCR_art10adeguamento(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("art10adeguamento", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo19(True)
                'End If

            End If
            'Aggiunto da Alessandra Taballione il 07/07/2005
            'Allegato a1
            If chkAllegatoA1IIStep.Checked = True Then '12
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hplAllegatoA1IIStep.Visible = True
                hplAllegatoA1IIStep.NavigateUrl = Documento.ACCR_allegatoA1(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatoA1", IdEnteFaseAcc)

                'If Session("TipoUtente") = "U" Then
                '    Gruppo12(True)
                'End If
            End If
            'Allegato a2
            If chkAllegatoA2IIStep.Checked = True Then '13
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hplAllegatoA2IIStep.Visible = True
                hplAllegatoA2IIStep.NavigateUrl = Documento.ACCR_allegatoA2(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatoA2", IdEnteFaseAcc)

                'If Session("TipoUtente") = "U" Then
                '    Gruppo13(True)
                'End If
            End If
            'Allegato b
            If chkAllegatoBIIStep.Checked = True Then '14
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hplAllegatoBIIStep.Visible = True
                hplAllegatoBIIStep.NavigateUrl = Documento.ACCR_allegatob(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatob", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo14(True)
                'End If

            End If
            'False sedi (II step, Revisione, Adeguamento)
            If chkRicClassIIstep.Checked = True Then '3
                Dim Documento As New GeneratoreModelli
                Dim IdEnteFaseAcc As Integer = RicavoIDEnteFaseAccreditamento(Session("IdEnte"))
                hpRicClassIIstep.Visible = True
                hpRicClassIIstep.NavigateUrl = Documento.ACCR_FalseSedi(Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("FalseSedi", IdEnteFaseAcc)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo3(True)
                'End If

            End If
            If chkRicClassAdeg.Checked = True Then '17
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hpRicClassAdeg.Visible = True
                hpRicClassAdeg.NavigateUrl = Documento.ACCR_FalseSediAdeg(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("FalseSediAdeg", ddlFasi.SelectedValue)

                'If Session("TipoUtente") = "U" Then
                '    Gruppo17(True)
                'End If
            End If
            'chkDetAdegPosTot
            If ChkArt2adeg.Checked = True Then '18
                If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                HplArt2adeg.Visible = True
                HplArt2adeg.NavigateUrl = Documento.ACCR_art2adeguamento(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("Articolo 2 Adeguamento", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo18(True)
                'End If
            End If
            '30/11/2015 nuovi report
            'ChkDetAdegPosArt10Tot
            'Determina Adeguamento Posistivo 
            If chkDetAdegPosTot.Checked = True Then '217
                'If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplDetAdegPosTot.Visible = True
                hplDetAdegPosTot.NavigateUrl = Documento.ACCR_DeterminaAdeguamentoPositivoTot(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DeterminaAdeguamentoPositivoTotale", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo21(True)
                'End If
            End If

            'Determina Adeguamento Posistivo Art 10
            If ChkDetAdegPosArt10Tot.Checked = True Then '22
                'If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                HplDetAdegPosArt10Tot.Visible = True
                HplDetAdegPosArt10Tot.NavigateUrl = Documento.ACCR_DeterminaAdeguamentoPositivoArt10Tot(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DeterminaAdeguamentoPositivoArt10Totale", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo22(True)
                'En
            End If

            'chkDetAdegPosLimTot
            'Determina Adeguamento Positivo con Limitazioni
            If chkDetAdegPosLimTot.Checked = True Then '218
                'If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplDetAdegPosLimTot.Visible = True
                hplDetAdegPosLimTot.NavigateUrl = Documento.ACCR_DeterminaAdeguamentoPositivoconLimitiTot(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DeterminaAdeguamentoPositivoconLimitiTotale", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo23(True)
                'End If
            End If
            'chkDetAdegNegTot
            'Determina Adeguamneto Negativo
            If chkDetAdegNegTot.Checked = True Then '24
                ' If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplDetAdegNegTot.Visible = True
                hplDetAdegNegTot.NavigateUrl = Documento.ACCR_DeterminaAdeguamentoNegativo(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DeterminaAdeguamentoNegativoTotale", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo24(True)
                'End If

            End If
            'allegato a1
            If chkAllegatoA1Tot.Checked = True Then '25
                'If ControlloSelezioneFase() = False Then Exit Sub
                hplAllegatoA1Tot.Visible = True
                'hplAllegatoA1.NavigateUrl = AllegatoA1determinazione(Session("IdEnte"), "allegatoA1Adeguamento")
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplAllegatoA1Tot.NavigateUrl = Documento.ACCR_allegatoA1AdeguamentoTot(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatoA1AdeguamentoTotale", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo25(True)
                'End If

            End If
            'allegato a2
            If chkAllegatoA2Tot.Checked = True Then '26
                ' If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplAllegatoA2Tot.Visible = True
                hplAllegatoA2Tot.NavigateUrl = Documento.ACCR_allegatoA2AdeguamentoTot(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatoA2AdeguamentoTotale", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo26(True)
                'End If
            End If
            'allegato b
            If chkAllegatoBTot.Checked = True Then '27
                ' If ControlloSelezioneFase() = False Then Exit Sub
                Dim Documento As New GeneratoreModelli
                hplAllegatoBTot.Visible = True
                hplAllegatoBTot.NavigateUrl = Documento.ACCR_allegatobAdeguamentoTot(ddlFasi.SelectedValue, Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatobAdeguamentoTotale", ddlFasi.SelectedValue)
                'If Session("TipoUtente") = "U" Then
                '    Gruppo27(True)
                'End If
            End If

            'chkAllegatoA1Tot
            'chkAllegatoA2Tot
            'chkAllegatoBTot



            '******************************************************************************************************************************
            '******************************************************************************************************************************
            '******************************************************************************************************************************


            'If chkRicClassRev.Checked = True Then
            '    hpRicClassRev.Visible = True
            '    hpRicClassRev.NavigateUrl = FalseSedi(Session("IdEnte"), "FalseSedi")
            'End If


            '****************************************************************************************

            'aggiunti da alessandra taballione il 23/06/2005
            'chkLetrevPosNeg.Enabled = False
            'chkdetRevPosLim.Enabled = False
            'chkDetRevNeg.Enabled = False
            'chkdetRevPos.Enabled = False
            'chkAllegatoA1rev.Enabled = False
            'chkAllegatoA2rev.Enabled = False
            'chkAllegatobrev.Enabled = False
            ChkArt2IIstep.Enabled = False
            chkart10IIstep.Enabled = False
            'chkart10revisioni.Enabled = False
            ChkArt2adeg.Enabled = False
            chkArt10adeg.Enabled = False

            chkComAvvioProd.Enabled = False
            chkLetteraCompleDocu.Enabled = False
            chkComAvvioProdAdeg.Enabled = False
            chkLetteraCompleDocuAdeg.Enabled = False

            chkLetAccPos.Enabled = False
            chkLetAccNeg.Enabled = False
            chkdetAccPosLim.Enabled = False
            chkdetAccPosLimSediFigure.Enabled = False
            chkDetAccNeg.Enabled = False
            chkDetAccPos.Enabled = False
            chkAllegatoA1.Enabled = False
            chkAllegatoA2.Enabled = False
            chkAllegatoB.Enabled = False
            'chkLetteratrasmissioneperdeterminanegtreanni.Enabled = False
            'chkArticolo10.Enabled = False
            'chkAttribuzioneCodice.Enabled = False
            'chkDetNegAccr.Enabled = False
            'chkDetNegASegRispEnte.Enabled = False
            'chkDetNegSenzaRispEnte.Enabled = False
            chkLettAdegPosNeg.Enabled = False
            chkDetAdegPos.Enabled = False
            chkDetAdegPosLim.Enabled = False
            chkDetAdegNeg.Enabled = False
            chkAllegatoA1IIStep.Enabled = False
            chkAllegatoA2IIStep.Enabled = False
            chkAllegatoBIIStep.Enabled = False
            imgGeneraFile.Enabled = False
            chkRicClassIIstep.Enabled = False
            chkRicClassAdeg.Enabled = False
            'chkRicClassRev.Enabled = False
            'aggiunto da Alessandra Taballione il 08/11/2005
            chkDetAccrPosArt10.Enabled = False
            ChkDetAdegPosArt10.Enabled = False
            'chkDetRevPosArt10.Enabled = False
        Else
            lblmessaggiosopra.Visible = True
            lblmessaggiosopra.Text = "Occorre selezionare un ente."
            'Imgerrore.Visible = True
        End If
    End Sub

    Protected Sub imgChiudi_Click1(sender As Object, e As EventArgs) Handles imgChiudi.Click
        If (HdValoreSalva.Value = 1 Or hddModificaProtocollo.Value = 1) Then
            Call Salvataggio()
        End If
        Response.Redirect("WfrmMain.aspx")
    End Sub


    'Protected Sub Salva_Click1(sender As Object, e As EventArgs) Handles Salva.Click
    '    Call Salvataggio()
    'End Sub

    Protected Sub chkDetAdegPosTot_CheckedChanged(sender As Object, e As EventArgs) Handles chkDetAdegPosTot.CheckedChanged

    End Sub
End Class