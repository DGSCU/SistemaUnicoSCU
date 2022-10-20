Imports System.IO

Public Class elencodocumentazioneProgetti

    Inherits System.Web.UI.Page
    Public dtrLeggiDati As SqlClient.SqlDataReader
    Public TBLLeggiDati As DataTable
    Public row As TableRow
    Public myRow As DataRow
    Dim strsql As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'If (Session("IdEnte") > -1) And (Not Session("IdEnte") Is Nothing) Then
        '    lblDownolad.Text = "<a href=""" & CaricaFile(Session("IdEnte")) & """>Scarica File</a>"
        'End If
        'controllo se effettuato login
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        DivFasc.Visible = False
        If Request.QueryString("Tipo") = "Programmi" Then
            DivProgetti.Visible = False
        Else
            DivProgrammi.Visible = False
        End If

        If IsPostBack = False Then
            PopolaCombo()
            'mauro lanna
            If Session("TipoUtente") = "U" Then
                Call caricafascicolo()
                'Call CaricaProtocollo()
            Else
                TxtNumeroFascicolo.Visible = False
                cmdSelFascicolo.Visible = False
                cmdSelProtocollo.Visible = False
                cmdFascCanc.Visible = False
                txtDescFasc.Visible = False
                'Salva.Visible = False
                lblCodiceFascicoloP.Visible = False
                LblDescFasc.Visible = False
            End If
            '** fine

        End If
        'Prova(Session("IdEnte"), "provaonzo")

        'Modifica del 17/01/2006 di Amilcare Paolella ***************************
        'Ricavo le informazioni dell'utente per valorizzare la path dei documenti
        strsql = "SELECT RegioniCompetenze.CodiceRegioneCompetenza AS Path FROM UtentiUNSC INNER JOIN " & _
                 "RegioniCompetenze ON UtentiUNSC.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                 "WHERE UtentiUNSC.UserName ='" & Session("Utente") & "'"
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
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

    End Sub

    Private Sub PopolaCombo()

        strsql = "  SELECT DISTINCT bando.* "
        strsql &= " FROM bando "
        strsql &= " INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando "
        strsql &= " INNER JOIN statiBando ON statiBando.IDStatoBando = bando.IDStatoBando "
        strsql &= " INNER JOIN AssociaBandoRegioniCompetenze ON bando.IDBando = AssociaBandoRegioniCompetenze.IdBando "
        strsql &= " INNER JOIN UtentiUNSC ON AssociaBandoRegioniCompetenze.IdRegioneCompetenza = UtentiUNSC.IdRegioneCompetenza "
        strsql &= " INNER JOIN AssociaBandoTipiProgetto  on AssociaBandoTipiProgetto.IdBando =  bando.IDBando  "
        strsql &= " INNER JOIN TipiProgetto on TipiProgetto.IdTipoProgetto= AssociaBandoTipiProgetto.IdTipoProgetto "
        strsql &= " WHERE bandiattività.idente= " & Session("IdEnte") & " "
        strsql &= " AND UtentiUNSC.UserName = '" & Session("Utente") & "'   "
        strsql &= " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"

        If Request.QueryString("Tipo") = "Programmi" Then
            strsql &= " AND bando.programmi = 1 "
        End If

        strsql &= "order by bando.idbando desc "
        'strsql = "select distinct * from bando " & _
        '            " INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando " & _
        '            " inner join statibando on statibando.idstatobando=bando.idstatobando " & _
        '            " where bandiattività.idente= " & Session("IdEnte") & "  order by bando.idbando desc"
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrLeggiDati.HasRows = True Then
            ddlBando.DataSource = dtrLeggiDati
            ddlBando.DataValueField = "idbando"
            ddlBando.DataTextField = "bandobreve"
            ddlBando.DataBind()

            lblmessaggiosopra.Visible = False
            'Imgerrore.Visible = False
        Else

            lblmessaggiosopra.Visible = True

            lblmessaggiosopra.Text = "Non e' possibile procedere con la stampa dei documenti<br>in quanto non ci sono bandi disponibili per l'ente selezionato."
        End If

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
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
            strsql = strsql & "WHERE pluto.idtiposede = 1) "
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
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))
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
                strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',1)"
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

    Sub NuovaCronologia(ByVal strDocumento As String)
        'vado a fare la insert
        Dim cmdinsert As Data.SqlClient.SqlCommand
        strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento,idbando) "
        strsql = strsql & "values "
        strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & strDocumento & "',1," & ddlBando.SelectedValue & ")"
        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()

        cmdinsert.Dispose()
    End Sub


    Sub CaricaNprogApprovati()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "SELECT  Count(*) as Nprogetti from VW_MOD_PROGETTI_POSITIVI where idbando='" & ddlBando.SelectedItem.Value & "' and IdEnte=" & CInt(Session("IdEnte"))
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub
    Sub CaricaNprogApprovatiLIM()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "SELECT  Count(*) as Nprogetti from VW_MOD_PROGETTI_POSITIVI_LIMITATI where idbando='" & ddlBando.SelectedItem.Value & "' and IdEnte=" & CInt(Session("IdEnte"))
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub
    Sub CaricaNprogRespinti()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "Select Count(*) as Nprogetti from VW_MOD_PROGETTI_NEGATIVI "
        strsql = strsql & "where idbando='" & ddlBando.SelectedItem.Value & "' and IdEnte=" & CInt(Session("IdEnte"))
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub

    Function TRASMISSIONE_COMUNICAZIONE_DETERMINALIMITATAPLURIMA_DETERMINANEGATIVAPLURIMA(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
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
            Call VISTAdatiente()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))

                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

                If UCase(Left(Trim(NomeFile), 9)) = "DETERMINA" Then

                    Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                                    "Avverso il presente provvedimento e' ammesso ricorso al T.A.R. nei termini e nei modi previsti dalla legge n.1034/71, come modificata dalla legge n.205/2000 o, in alternativa, e' ammesso ricorso straordinario al Presidente della Repubblica nei termini e nei modi previsti dal D.P.R. n.1199/71, come modificato dalla legge n.205/2000.\par}}")
                End If
                'apro il template
                xLinea = Reader.ReadLine()


                Dim strDenominazioneEnte As String = dtrLeggiDati("DenominazioneEnte")
                Dim strClasseRichiesta As String = dtrLeggiDati("Classe") & "^"
                'Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")
                Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strNumeroCivico As String = dtrLeggiDati("NumeroCivico")
                Dim strCAP As String = dtrLeggiDati("CAP")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim strProvincia As String = dtrLeggiDati("provincia")
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceEnte")

                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<ClasseRichiesta>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<Indirizzo>", strIndirizzo)
                    xLinea = Replace(xLinea, "<NumeroCivico>", strNumeroCivico)
                    xLinea = Replace(xLinea, "<CAP>", strCAP)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Provincia>", strProvincia)
                    'xLinea = Replace(xLinea, "<CodiceRegione>", strCodiceRegione)
                    xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceRegione)

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

            TRASMISSIONE_COMUNICAZIONE_DETERMINALIMITATAPLURIMA_DETERMINANEGATIVAPLURIMA = "documentazione/" & strNomeFile
            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',1)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function
    Function DeterminaSingolaLIM(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

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
            Call VISTAdatiente()
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))

                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

                Writer.WriteLine("{\rtf1")

                'Write the page header and footer
                Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                                "AG01\par}}")
                'apro il template

                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                    "\par}}")

                xLinea = Reader.ReadLine()

                Dim strDenominazioneEnte As String = dtrLeggiDati("DenominazioneEnte")
                Dim strClasseRichiesta As String = dtrLeggiDati("Classe") & "^"
                'Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")
                Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strNumeroCivico As String = dtrLeggiDati("NumeroCivico")
                Dim strCAP As String = dtrLeggiDati("CAP")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim strProvincia As String = dtrLeggiDati("provincia")
                'Dim strCodiceRegione As String = dtrLeggiDati("CodiceRegione")
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceEnte")
                Dim strPunteggioFinale As String
                Dim strtitolo As String
                Dim stridAttivita As String
                '************************
                Dim strNoteProg As String
                '************************
                Dim NoteStorico As String 'Antonello

                Call VistaLimitazioni()

                If dtrLeggiDati.HasRows = True Then
                    dtrLeggiDati.Read()
                    strtitolo = dtrLeggiDati("Titolo")
                    strPunteggioFinale = dtrLeggiDati("Punteggio")

                    '************************
                    strNoteProg = dtrLeggiDati("Limitazioni")
                    'strNoteProg = Replace(strNoteProg, Chr(147), """")
                    'strNoteProg = Replace(strNoteProg, Chr(148), """")
                    'strNoteProg = Replace(strNoteProg, Chr(133), ".")
                    'strNoteProg = Replace(strNoteProg, Chr(150), "-")
                    'strNoteProg = Replace(strNoteProg, Chr(146), "'")
                    'strNoteProg = Replace(Replace(Replace(Replace(Replace(Replace(Replace(strNoteProg, "°", ""), "ì", "i'"), "é", "e'"), "à", "a'"), "ò", "o'"), "è", "e'"), "ù", "u'")
                    '************************
                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If
                Else
                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If

                End If
                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Classe>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<CodiceRegione>", strCodiceRegione)
                    xLinea = Replace(xLinea, "<Titolo>", strtitolo)
                    xLinea = Replace(xLinea, "<PunteggioFinale>", strPunteggioFinale)
                    xLinea = Replace(xLinea, "<Limitazioni>", IIf(strNoteProg <> "", strNoteProg, "Assenti."))
                    Dim intX As Integer
                    If InStr(xLinea, "<BreakPointLim>") > 0 Then
                        xLinea = Replace(xLinea, "<BreakPointLim>", "")
                        If dtrLeggiDati.HasRows = True Then
                            While dtrLeggiDati.Read
                                NoteStorico = dtrLeggiDati("limitazione")
                                'NoteStorico = Replace(NoteStorico, Chr(147), """")
                                'NoteStorico = Replace(NoteStorico, Chr(148), """")
                                'NoteStorico = Replace(NoteStorico, Chr(133), ".")
                                'NoteStorico = Replace(NoteStorico, Chr(150), "-")
                                'NoteStorico = Replace(NoteStorico, Chr(146), "'")
                                'NoteStorico = Replace(Replace(Replace(Replace(Replace(Replace(Replace(NoteStorico, "°", ""), "ì", "i'"), "é", "e'"), "à", "a'"), "ò", "o'"), "è", "e'"), "ù", "u'")
                                Writer.WriteLine(NoteStorico & "\par")
                                'Writer.WriteLine(dtrLeggiDati("limitazione") & "\par")
                                Writer.WriteLine(xLinea)
                            End While
                        Else
                            Writer.WriteLine("Limitazioni assenti.\par")
                        End If
                    End If
                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

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

            DeterminaSingolaLIM = "documentazione/" & strNomeFile

            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',1)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function
    Function DeterminaSingolaNegativa(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String

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
            Call VISTAdatiente()
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))

                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

                Writer.WriteLine("{\rtf1")

                'Write the page header and footer
                Writer.WriteLine("{\header\pard\qr{\fs18 " & _
                                "AG01\par}}")
                'apro il template

                Writer.WriteLine("{\footer\pard\ql{\fs16 " & _
                    "\par}}")

                xLinea = Reader.ReadLine()

                Dim strDenominazioneEnte As String = dtrLeggiDati("DenominazioneEnte")
                Dim strClasseRichiesta As String = dtrLeggiDati("Classe") & "^"
                Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strNumeroCivico As String = dtrLeggiDati("NumeroCivico")
                Dim strCAP As String = dtrLeggiDati("CAP")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim strProvincia As String = dtrLeggiDati("provincia")
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceEnte")
                Dim strPunteggioFinale As String
                Dim strtitolo As String
                Dim stridAttivita As String
                '************************
                Dim strNoteProg As String
                '************************
                Dim NoteStorico As String 'Antonello


                Call VISTAProgNegativiBis()

                If dtrLeggiDati.HasRows = True Then
                    dtrLeggiDati.Read()
                    strtitolo = dtrLeggiDati("Titolo")


                    stridAttivita = dtrLeggiDati("Idattività")


                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If
                Else
                    stridAttivita = 0
                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If
                End If
                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Classe>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<CodiceRegione>", strCodiceRegione)
                    xLinea = Replace(xLinea, "<Titolo>", strtitolo)
                    xLinea = Replace(xLinea, "<PunteggioFinale>", strPunteggioFinale)
                    xLinea = Replace(xLinea, "<Limitazioni>", IIf(strNoteProg <> "", strNoteProg, "Assenti."))
                    Dim intX As Integer
                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        Call VISTANegativiMotivazioni(stridAttivita)
                        xLinea = Replace(xLinea, "<BreakPoint>", "")
                        If dtrLeggiDati.HasRows = True Then
                            While dtrLeggiDati.Read
                                NoteStorico = dtrLeggiDati("Motivazione")
                                'NoteStorico = Replace(NoteStorico, Chr(147), """")
                                'NoteStorico = Replace(NoteStorico, Chr(148), """")
                                'NoteStorico = Replace(NoteStorico, Chr(133), ".")
                                'NoteStorico = Replace(NoteStorico, Chr(150), "-")
                                'NoteStorico = Replace(Replace(Replace(Replace(Replace(Replace(Replace(NoteStorico, "°", ""), "ì", "i'"), "é", "e'"), "à", "a'"), "ò", "o'"), "è", "e'"), "ù", "u'")
                                Writer.WriteLine(NoteStorico & "\par")
                                Writer.WriteLine(xLinea)
                            End While
                        Else
                            'Writer.WriteLine("Limitazioni assenti.\par")
                        End If
                    End If
                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

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

            DeterminaSingolaNegativa = "documentazione/" & strNomeFile
            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',1)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Function

    Function ElencoProgettiPositivi(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        ''''****************************************
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String
        Dim dtrcount As SqlClient.SqlDataReader
        Dim NoteStorico As String
        Dim AppoTitolo As String
        Dim NoteProg As String

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
            Call VISTAdatiente()
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))

                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

                'Writer.WriteLine("{\rtf1")
                'apro il template
                xLinea = Reader.ReadLine()

                Dim strDenominazioneEnte As String = dtrLeggiDati("DenominazioneEnte")
                Dim strClasseRichiesta As String = dtrLeggiDati("Classe")
                'Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")
                Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strNumeroCivico As String = dtrLeggiDati("NumeroCivico")
                Dim strCAP As String = dtrLeggiDati("CAP")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim strProvincia As String = dtrLeggiDati("provincia")
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceEnte")
                Dim strBando As String = ddlBando.SelectedItem.Text

                CaricaNprogApprovati()

                Dim strCount As String
                If dtrLeggiDati.HasRows = True Then
                    dtrLeggiDati.Read()
                    strCount = dtrLeggiDati("Nprogetti")
                Else
                    strCount = "0"
                End If

                Call VistaProgPositivi()

                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<Classe>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<Indirizzo>", strIndirizzo)
                    xLinea = Replace(xLinea, "<NumeroCivico>", strNumeroCivico)
                    xLinea = Replace(xLinea, "<CAP>", strCAP)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Provincia>", strProvincia)
                    xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceRegione)
                    xLinea = Replace(xLinea, "<BandoBreve>", strBando)
                    xLinea = Replace(xLinea, "<NProgetti>", strCount)

                    Dim intX As Integer
                    Dim num As Integer
                    Dim punteggio As String
                    Dim ciclovirgola As Integer

                    num = 0
                    If InStr(xLinea, "<BreakPoint>") > 0 Then

                        'xLinea = Replace(xLinea, "<BreakPoint>\fs20\par", "")
                        xLinea = Nothing

                        For Each myRow In TBLLeggiDati.Rows
                            num = num + 1

                            'Antonello
                            AppoTitolo = myRow.Item("Titolo")
                            'AppoTitolo = Replace(AppoTitolo, Chr(147), """")
                            'AppoTitolo = Replace(AppoTitolo, Chr(148), """")
                            'AppoTitolo = Replace(AppoTitolo, Chr(133), ".")
                            'AppoTitolo = Replace(AppoTitolo, Chr(150), "-")
                            'AppoTitolo = Replace(AppoTitolo, Chr(146), "'")
                            punteggio = myRow.Item("Punteggio")

                            For ciclovirgola = 1 To Len(punteggio)
                                If Mid(punteggio, ciclovirgola, 1) = "," Then
                                    'punteggio = Mid(myRow.Item("Punteggio"), 1, ciclo - 1)
                                    If Mid(myRow.Item("Punteggio"), ciclovirgola + 1, 2) = "00" Then
                                        'punteggio = Mid(punteggio, 1, ciclovirgola - 1)
                                        punteggio = CInt(punteggio)
                                    End If
                                End If
                            Next
                            xLinea = (Replace(xLinea, "<Nprogetti>", strCount))
                            Writer.WriteLine("\b " & num & ")" & Trim(AppoTitolo) & " - Punteggio: " & punteggio & " n.volontari " & myRow.Item("NVOLONTARI") & " " & myRow.Item("LIMITATO") & "\b0\par")

                            If Not dtrLeggiDati Is Nothing Then
                                dtrLeggiDati.Close()
                                dtrLeggiDati = Nothing
                            End If

                            'modifica *************************
                            'If num = 2 Then
                            '    xLinea = Nothing
                            'End If
                            '**********************************
                        Next
                        'End While
                        Writer.WriteLine("}")
                        Writer.WriteLine("\par")
                        'End If
                    End If
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
            ElencoProgettiPositivi = "documentazione/" & strNomeFile
            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',1)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        ''''''''''''''''''''''''''''''''''''''''''''''
    End Function
    Function ElencoProgettiPositiviLIM(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        ''''****************************************
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String
        Dim dtrcount As SqlClient.SqlDataReader
        Dim NoteStorico As String
        Dim AppoTitolo As String
        Dim NoteProg As String

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
            Call VISTAdatiente()
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))

                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

                'Writer.WriteLine("{\rtf1")
                'apro il template
                xLinea = Reader.ReadLine()

                Dim strDenominazioneEnte As String = dtrLeggiDati("DenominazioneEnte")
                Dim strClasseRichiesta As String = dtrLeggiDati("Classe")
                Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strNumeroCivico As String = dtrLeggiDati("NumeroCivico")
                Dim strCAP As String = dtrLeggiDati("CAP")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim strProvincia As String = dtrLeggiDati("provincia")
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceEnte")
                Dim strBando As String = ddlBando.SelectedItem.Text

                CaricaNprogApprovatiLIM()

                Dim strCount As String
                If dtrLeggiDati.HasRows = True Then
                    dtrLeggiDati.Read()
                    strCount = dtrLeggiDati("Nprogetti")
                Else
                    strCount = "0"
                End If



                Call VistaLimitazioniLIM()
                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<Classe>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<Indirizzo>", strIndirizzo)
                    xLinea = Replace(xLinea, "<NumeroCivico>", strNumeroCivico)
                    xLinea = Replace(xLinea, "<CAP>", strCAP)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Provincia>", strProvincia)
                    xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceRegione)
                    xLinea = Replace(xLinea, "<BandoBreve>", strBando)
                    xLinea = Replace(xLinea, "<NProgetti>", strCount)

                    Dim intX As Integer
                    Dim num As Integer
                    num = 0
                    If InStr(xLinea, "<BreakPoint>") > 0 Then


                        xLinea = Nothing

                        For Each myRow In TBLLeggiDati.Rows
                            num = num + 1

                            'Antonello
                            AppoTitolo = myRow.Item("Titolo")
                            'AppoTitolo = Replace(AppoTitolo, Chr(147), """")
                            'AppoTitolo = Replace(AppoTitolo, Chr(148), """")
                            'AppoTitolo = Replace(AppoTitolo, Chr(133), ".")
                            'AppoTitolo = Replace(AppoTitolo, Chr(150), "-")
                            'AppoTitolo = Replace(AppoTitolo, Chr(146), "'")

                            xLinea = (Replace(xLinea, "<Nprogetti>", strCount))
                            'Writer.WriteLine("\b " & num & ") " & AppoTitolo & " - Punteggio: " & myRow.Item("Punteggio") & " n.volontari " & myRow.Item("NVOLONTARI") & " " & myRow.Item("LIMITATO") & "\b0\par")
                            Writer.WriteLine("\b " & num & ") " & AppoTitolo & " - " & " n.volontari " & myRow.Item("NVOLONTARI") & " " & myRow.Item("LIMITATO") & "\b0\par")
                            'Writer.WriteLine("Note: " & IIf(myRow.Item("NoteProg") = "", "Assenti", NoteProg) & " \par")
                            'Writer.WriteLine("Motivazioni:\par")

                            NoteStorico = myRow.Item("Limitazioni")
                            'NoteStorico = Replace(NoteStorico, Chr(147), """")
                            'NoteStorico = Replace(NoteStorico, Chr(148), """")
                            'NoteStorico = Replace(NoteStorico, Chr(133), ".")
                            'NoteStorico = Replace(NoteStorico, Chr(150), "-")
                            'NoteStorico = Replace(NoteStorico, Chr(146), "'")
                            'NoteStorico = Replace(Replace(Replace(Replace(Replace(Replace(Replace(NoteStorico, "°", ""), "ì", "i'"), "é", "e'"), "à", "à"), "ò", "o'"), "è", "e'"), "ù", "u'")
                            Writer.WriteLine(NoteStorico & "\par")

                            If Not dtrLeggiDati Is Nothing Then
                                dtrLeggiDati.Close()
                                dtrLeggiDati = Nothing
                            End If

                            'modifica *************************
                            'If num = 2 Then
                            '    xLinea = Nothing
                            'End If
                            '**********************************
                        Next
                        'End While
                        Writer.WriteLine("}")
                        Writer.WriteLine("\par")
                        'End If
                    End If
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
            ElencoProgettiPositiviLIM = "documentazione/" & strNomeFile
            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',1)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        ''''''''''''''''''''''''''''''''''''''''''''''
    End Function
    Function ElencoProgettiNegativi(ByVal intIdEnte As Integer, ByVal NomeFile As String) As String
        ''''****************************************
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
            Call VISTAdatiente()
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/" & Session("Path") & NomeFile & ".rtf"))

                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

                'Writer.WriteLine("{\rtf1")
                'apro il template
                xLinea = Reader.ReadLine()

                Dim strDenominazioneEnte As String = dtrLeggiDati("DenominazioneEnte")
                Dim strClasseRichiesta As String = dtrLeggiDati("Classe")
                'Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")
                Dim strIndirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strNumeroCivico As String = dtrLeggiDati("NumeroCivico")
                Dim strCAP As String = dtrLeggiDati("CAP")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim strProvincia As String = dtrLeggiDati("provincia")
                'Dim strCodiceRegione As String = dtrLeggiDati("CodiceRegione")
                Dim strCodiceRegione As String = dtrLeggiDati("CodiceEnte")
                Dim strBando As String = ddlBando.SelectedItem.Text
                CaricaNprogRespinti()
                Dim strCount As String
                If dtrLeggiDati.HasRows = True Then
                    dtrLeggiDati.Read()
                    strCount = dtrLeggiDati("Nprogetti")
                Else
                    strCount = "0"
                End If
                Call VISTAProgNegativi()
                While xLinea <> ""

                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<Classe>", strClasseRichiesta)
                    xLinea = Replace(xLinea, "<Indirizzo>", strIndirizzo)
                    xLinea = Replace(xLinea, "<NumeroCivico>", strNumeroCivico)
                    xLinea = Replace(xLinea, "<CAP>", strCAP)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Provincia>", strProvincia)
                    xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceRegione)
                    xLinea = Replace(xLinea, "<BandoBreve>", strBando)
                    xLinea = Replace(xLinea, "<NProgetti>", strCount)
                    Dim intX As Integer
                    Dim num As Integer
                    Dim AppoTitolo As String
                    Dim NoteStorico As String
                    Dim pazzo As Integer

                    num = 0
                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        'xLinea = Replace(xLinea, "<BreakPoint>", "") & "\par"
                        xLinea = Nothing

                        For Each myRow In TBLLeggiDati.Rows
                            num = num + 1
                            'Antonello
                            AppoTitolo = myRow.Item("Titolo")
                            'AppoTitolo = Replace(AppoTitolo, Chr(147), """")
                            'AppoTitolo = Replace(AppoTitolo, Chr(148), """")
                            'AppoTitolo = Replace(AppoTitolo, Chr(133), ".")
                            'AppoTitolo = Replace(AppoTitolo, Chr(150), "-")
                            'AppoTitolo = Replace(AppoTitolo, Chr(146), "'")

                            'xLinea = (Replace(xLinea, "<Nprogetti>", myRow.Item("nprogetti")))
                            xLinea = (Replace(xLinea, "<Nprogetti>", strCount))
                            'strCount
                            Writer.WriteLine("\b " & num & ")     " & AppoTitolo & " \b0\par")
                            'Writer.WriteLine("Note: " & IIf(myRow.Item("NoteProg") = "", "Assenti", NoteProg) & " \par")
                            'Writer.WriteLine("Motivazioni:\par")

                            'Elenco limitazioni X progetto (flagAttività  valore=2)
                            VISTANegativiMotivazioni(myRow.Item("idattività"))
                            If dtrLeggiDati.HasRows = True Then
                                While dtrLeggiDati.Read
                                    NoteStorico = dtrLeggiDati("Motivazione")
                                    'NoteStorico = Replace(NoteStorico, Chr(147), """")
                                    'NoteStorico = Replace(NoteStorico, Chr(148), """")
                                    'NoteStorico = Replace(NoteStorico, Chr(133), ".")
                                    'NoteStorico = Replace(NoteStorico, Chr(150), "-")
                                    'NoteStorico = Replace(NoteStorico, Chr(146), "'")
                                    'NoteStorico = Replace(Replace(Replace(Replace(Replace(Replace(Replace(NoteStorico, "°", ""), "ì", "i'"), "é", "e'"), "à", "a'"), "ò", "o'"), "è", "e'"), "ù", "u'")
                                    Writer.WriteLine(NoteStorico & "\par")
                                    ' Writer.WriteLine(dtrLeggiDati("limitazione") & "\par")
                                    'Writer.WriteLine(xLinea)
                                End While
                            End If
                            If Not dtrLeggiDati Is Nothing Then
                                dtrLeggiDati.Close()
                                dtrLeggiDati = Nothing
                            End If
                            'Writer.WriteLine(xLinea)
                            'xLinea = Reader.ReadLine()
                        Next
                        Writer.WriteLine("\par")
                        'End If
                    End If

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

            ElencoProgettiNegativi = "documentazione/" & strNomeFile
            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento) "
            strsql = strsql & "values "
            strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & NomeFile & "',1)"
            cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
            cmdinsert.ExecuteNonQuery()

            cmdinsert.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        ''''''''''''''''''''''''''''''''''''''''''''''
    End Function

    Sub VISTAdatiente()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "select * from vw_mod_enti "
        strsql = strsql & "where IdEnte=" & Session("IdEnte")
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub
    Sub VISTAProgNegativi()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "Select * from VW_MOD_PROGETTI_NEGATIVI "
        strsql = strsql & "where idbando='" & ddlBando.SelectedItem.Value & "' and IdEnte=" & CInt(Session("IdEnte"))
        'eseguo la query e passo il risultato al datareader
        'dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
        TBLLeggiDati = ClsServer.CreaDataTable(strsql, False, Session("conn"))
    End Sub

    Sub VISTANegativiMotivazioni(ByVal idattivita As String)
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "SELECT * from VW_MOD_PROGETTI_NEGATIVI_MOTIVAZIONI WHERE IdAttività = " & idattivita
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub

    Sub VistaLimitazioni()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "Select * from VW_MOD_PROGETTI_POSITIVI_LIMITATI "
        strsql = strsql & "where idbando='" & ddlBando.SelectedItem.Value & "' and IdEnte=" & CInt(Session("IdEnte"))
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub
    Sub VistaProgPositivi()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "SELECT  * from VW_MOD_PROGETTI_POSITIVI where idbando='" & ddlBando.SelectedItem.Value & "' and IdEnte=" & CInt(Session("IdEnte"))
        'eseguo la query e passo il risultato al datareader
        TBLLeggiDati = ClsServer.CreaDataTable(strsql, False, Session("conn"))
    End Sub
    Sub VISTAProgNegativiBis() 'questa sub si differenzia solo per il fatto che viene usato un dtr
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "Select * from VW_MOD_PROGETTI_NEGATIVI "
        strsql = strsql & "where idbando='" & ddlBando.SelectedItem.Value & "' and IdEnte=" & CInt(Session("IdEnte"))
        'eseguo la query e passo il risultato al datareader
        'dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub
    Sub VistaLimitazioniLIM()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "Select * from VW_MOD_PROGETTI_POSITIVI_LIMITATI "
        strsql = strsql & "where idbando='" & ddlBando.SelectedItem.Value & "' and IdEnte=" & CInt(Session("IdEnte"))
        TBLLeggiDati = ClsServer.CreaDataTable(strsql, False, Session("conn"))
    End Sub
    '****************************************
    Private Sub cmdFascCanc_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCanc.Click
        TxtNumeroFascicolo.Text = ""
        'TxtCodiceFasc.Text = ""
        txtDescFasc.Text = ""
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
        'salva i dati vuoti

    End Sub
    Sub SalvaFascicolo()
        'Dim strsql As String
        ''vado a fare update ma si legge ;)

        'Dim cmdupdate As Data.SqlClient.SqlCommand
        'strsql = "update bandiattività set codicefascicolopc=' " & TxtNumeroFascicolo.Text & _
        '"', idfascicolopc ='" & TxtCodiceFasc.Text & "', descrfascicolopc='" & txtDescFasc.Text.Replace("'", "''") & "'"
        'strsql = strsql & " where idbando =" & ddlBando.SelectedValue & " and idente = " & Session("IdEnte")

        'cmdupdate = New SqlClient.SqlCommand(strsql, Session("conn"))
        'cmdupdate.ExecuteNonQuery()
        'cmdupdate.Dispose()

    End Sub

    Sub caricafascicolo()
        'Dim dtrUtilizzato As SqlClient.SqlDataReader
        'Dim strsql As String

        'strsql = "Select codicefascicolopc,idfascicolopc,descrfascicolopc  from "
        'strsql = strsql & "bandiattività where idbando ='" & ddlBando.SelectedValue & "' and idente = " & Session("IdEnte")
        'dtrUtilizzato = ClsServer.CreaDatareader(strsql, Session("conn"))

        'If dtrUtilizzato.HasRows = True Then
        '    dtrUtilizzato.Read()

        '    If IsDBNull(dtrUtilizzato("codicefascicolopc")) = False Then
        '        TxtNumeroFascicolo.Text = Trim(dtrUtilizzato("codicefascicolopc"))
        '        TxtNumFascicoloControllo.Text = Trim(dtrUtilizzato("codicefascicolopc"))
        '        TxtCodiceFasc.Text = dtrUtilizzato("idfascicolopc")
        '        txtDescFasc.Text = dtrUtilizzato("descrfascicolopc")
        '    Else
        '        TxtNumeroFascicolo.Text = ""
        '        TxtNumFascicoloControllo.Text = ""
        '        TxtCodiceFasc.Text = ""
        '        txtDescFasc.Text = ""
        '    End If
        'End If
        'If Not dtrUtilizzato Is Nothing Then
        '    dtrUtilizzato.Close()
        '    dtrUtilizzato = Nothing
        'End If
        ''********
    End Sub

    Sub cancella()
        'Dim strLocal As String
        'Dim dtrCancellazione As SqlClient.SqlDataReader
        'Dim mycommand As New SqlClient.SqlCommand
        'Dim mydatatable As New DataTable

        'mycommand.Connection = Session("conn")

        ''cancella
        ''strLocal = "select IDAttivitàSedeAssegnazione from attività inner join attivitàsediassegnazione on " & _
        ''"attività.idattività = attivitàsediassegnazione.idattività where(attività.idattività = " & Request.QueryString("IdAttivita") & ")"
        'Try
        '    'mydatatable = ClsServer.CreaDataTable(strLocal, False, Session("conn"))

        '    'Dim k As Int16

        '    'For k = 0 To mydatatable.Rows.Count - 1
        '    strLocal = "update cronologiaentidocumenti set dataprot =null,  nprot = null where idente = " & CInt(Session("IdEnte")) & _
        '    " and idbando =" & ddlBando.SelectedValue

        '    mycommand.CommandText = strLocal
        '    mycommand.ExecuteNonQuery()
        '    'Next
        '    '*******************************************************************************
        'Catch ex As Exception
        '    'Response.Write(ex.Message.ToString())
        'End Try

        ''strsql = "update bandiattività set codicefascicoloai=' " & TxtNumeroFascicolo.Text & _
        ''       "', idfascicoloai ='" & TxtCodiceFasc.Text & "', descrfascicoloai='" & txtDescFasc.Text
        ''strsql = strsql & "' where idbando =" & ddlBando.SelectedValue & " and idente = " & Session("IdEnte")

        ''mycommand.CommandText = strLocal
        '' mycommand.ExecuteNonQuery()

    End Sub


    Sub SalvaProtocolli(ByVal StrDataprot As String, ByVal StrNumProt As String, ByVal StringaDocumento As String)

        Dim strsql As String
        'vado a fare update
        Dim cmdupdate As Data.SqlClient.SqlCommand
        If StrDataprot = "" Then
            strsql = "update cronologiaentidocumenti set dataprot=null, nprot=null"
        Else
            strsql = "update cronologiaentidocumenti set dataprot='" & StrDataprot & "', nprot='" & StrNumProt & "'"
        End If
        strsql = strsql & " where idente = " & CInt(Session("IdEnte")) & _
        " and documento ='" & StringaDocumento & "' and idbando =" & ddlBando.SelectedValue

        cmdupdate = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdupdate.ExecuteNonQuery()
        cmdupdate.Dispose()
    End Sub

    'Sub CaricaProtocollo()
    '    Dim dtrUtilizzato As SqlClient.SqlDataReader
    '    Dim strsql As String

    '    strsql = "select * from cronologiaentidocumenti where idente = " & CInt(Session("IdEnte")) & _
    '    " and idbando ='" & ddlBando.SelectedValue & "'"

    '    dtrUtilizzato = ClsServer.CreaDatareader(strsql, Session("conn"))

    '    Gruppo1(False)
    '    Gruppo2(False)
    '    Gruppo3(False)
    '    Gruppo4(False)
    '    Gruppo5(False)
    '    Gruppo6(False)
    '    Gruppo7(False)
    '    Gruppo8(False)
    '    Gruppo9(False)

    '    Do While dtrUtilizzato.Read()
    '        Select Case dtrUtilizzato("Documento")

    '            Case "letteraditrasmissione1"
    '                Gruppo1(True)
    '                TxtNumProtocollo1.Text = "" & dtrUtilizzato("NProt")
    '                TxtDataProtocollo1.Text = "" & dtrUtilizzato("DataProt")

    '            Case "comunicazionepositiva1"
    '                Gruppo2(True)
    '                TxtNumProtocollo2.Text = "" & dtrUtilizzato("NProt")
    '                TxtDataProtocollo2.Text = "" & dtrUtilizzato("DataProt")

    '            Case "limitataplurima1"
    '                Gruppo3(True)
    '                TxtNumProtocollo3.Text = "" & dtrUtilizzato("NProt")
    '                TxtDataProtocollo3.Text = "" & dtrUtilizzato("DataProt")

    '            Case "limitatasingola1"
    '                Gruppo4(True)
    '                TxtNumProtocollo4.Text = "" & dtrUtilizzato("NProt")
    '                TxtDataProtocollo4.Text = "" & dtrUtilizzato("DataProt")

    '            Case "negativaplurima"
    '                Gruppo5(True)
    '                TxtNumProtocollo5.Text = "" & dtrUtilizzato("NProt")
    '                TxtDataProtocollo5.Text = "" & dtrUtilizzato("DataProt")

    '            Case "negativasingola1"
    '                Gruppo6(True)
    '                TxtNumProtocollo6.Text = "" & dtrUtilizzato("NProt")
    '                TxtDataProtocollo6.Text = "" & dtrUtilizzato("DataProt")

    '            Case "allegatonegativi1"
    '                Gruppo7(True)
    '                TxtNumProtocollo7.Text = "" & dtrUtilizzato("NProt")
    '                TxtDataProtocollo7.Text = "" & dtrUtilizzato("DataProt")

    '            Case "allegatopositivi1"
    '                Gruppo8(True)
    '                TxtNumProtocollo8.Text = "" & dtrUtilizzato("NProt")
    '                TxtDataProtocollo8.Text = "" & dtrUtilizzato("DataProt")

    '            Case "allegatopositivilimitati1"
    '                Gruppo9(True)
    '                TxtNumProtocollo9.Text = "" & dtrUtilizzato("NProt")
    '                TxtDataProtocollo9.Text = "" & dtrUtilizzato("DataProt")
    '        End Select
    '    Loop

    '    'mettere il visible sull'attivazione della freccietta per stampare

    '    If Not dtrUtilizzato Is Nothing Then
    '        dtrUtilizzato.Close()
    '        dtrUtilizzato = Nothing
    '    End If

    'End Sub
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
    '    If TxtNumProtocollo7.Visible = False Then
    '        TxtNumProtocollo7.Text = ""
    '        TxtDataProtocollo7.Text = ""
    '    End If
    '    LblNumProtocollo7.Visible = BlValore
    '    TxtNumProtocollo7.Visible = BlValore
    '    cmdSelProtocollo7.Visible = BlValore
    '    cmdScAllegati7.Visible = BlValore
    '    cmdNuovoFascicolo7.Visible = BlValore
    '    LblDataProtocollo7.Visible = BlValore
    '    TxtDataProtocollo7.Visible = BlValore

    'End Sub
    'Private Sub Gruppo8(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo8.Visible = False Then
    '        TxtNumProtocollo8.Text = ""
    '        TxtDataProtocollo8.Text = ""
    '    End If
    '    LblNumProtocollo8.Visible = BlValore
    '    TxtNumProtocollo8.Visible = BlValore
    '    cmdSelProtocollo8.Visible = BlValore
    '    cmdScAllegati8.Visible = BlValore
    '    cmdNuovoFascicolo8.Visible = BlValore
    '    LblDataProtocollo8.Visible = BlValore
    '    TxtDataProtocollo8.Visible = BlValore

    'End Sub
    'Private Sub Gruppo9(ByVal BlValore As Boolean)
    '    If TxtNumProtocollo9.Visible = False Then
    '        TxtNumProtocollo9.Text = ""
    '        TxtDataProtocollo9.Text = ""
    '    End If
    '    LblNumProtocollo9.Visible = BlValore
    '    TxtNumProtocollo9.Visible = BlValore
    '    cmdSelProtocollo9.Visible = BlValore
    '    cmdScAllegati9.Visible = BlValore
    '    cmdNuovoFascicolo9.Visible = BlValore
    '    LblDataProtocollo9.Visible = BlValore
    '    TxtDataProtocollo9.Visible = BlValore

    'End Sub
    Private Sub ddlBando_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlBando.SelectedIndexChanged
        Dim EvArgs As System.EventArgs
        If Session("TipoUtente") = "U" Then
            Call ResettaMaschera()
            Call caricafascicolo()
            'Call CaricaProtocollo()
        End If
    End Sub
    Sub ResettaMaschera()
        ChkLetteraPositivaNegativa.Enabled = True
        ChkLetteraPositivaNegativa.Checked = False
        chkletteraNegativa.Enabled = True
        chkletteraNegativa.Checked = False
        chkdetNegSingola.Enabled = True
        chkdetNegSingola.Checked = False
        chkDetNegMultipla.Enabled = True
        chkDetNegMultipla.Checked = False
        chkLetteraPositiva.Enabled = True
        chkLetteraPositiva.Checked = False
        chkDetNegMultipla.Enabled = True
        chkDetNegMultipla.Checked = False
        chkDetPosSingola.Enabled = True
        chkDetPosSingola.Checked = False
        chkElencoProgNeg.Enabled = True
        chkElencoProgNeg.Checked = False
        chkdetPosMultipla.Enabled = True
        chkdetPosMultipla.Checked = False
        chkElencoProgPos.Enabled = True
        chkElencoProgPos.Checked = False

        imgGeneraFile.Enabled = True

        hplPositivaNegativa.Visible = False
        hplletteraNegativa.Visible = False
        hpldetNegSingola.Visible = False
        hplLetteraPositiva.Visible = False
        hplDetNegMultipla.Visible = False
        hplDetPosSingola.Visible = False
        hplElencoProgNeg.Visible = False
        hpldetPosMultipla.Visible = False
        hplElencoProgPos.Visible = False
    End Sub

    'Private Sub Salva_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Salva.Click



    '    'Call Salvataggio()

    'End Sub
    ''Sub Salvataggio()
    ''If TxtNumeroFascicolo.Text <> "" Then

    '    If TxtNumFascicoloControllo.Text <> "" Then
    '        If Trim(TxtNumeroFascicolo.Text) <> Trim(TxtNumFascicoloControllo.Text) Then
    '            Call cancella()
    '        End If
    '    End If
    '    Call SalvaFascicolo()

    ''If TxtNumProtocollo1.Text <> "" Then
    '    Call SalvaProtocolli(TxtDataProtocollo1.Text, TxtNumProtocollo1.Text, "letteraditrasmissione1")
    ''End If
    ''If TxtNumProtocollo2.Text <> "" Then
    '    Call SalvaProtocolli(TxtDataProtocollo2.Text, TxtNumProtocollo2.Text, "comunicazionepositiva1")
    ''End If
    ''If TxtNumProtocollo3.Text <> "" Then
    '    Call SalvaProtocolli(TxtDataProtocollo3.Text, TxtNumProtocollo3.Text, "limitataplurima1")
    ''End If
    ''If TxtNumProtocollo4.Text <> "" Then
    '    Call SalvaProtocolli(TxtDataProtocollo4.Text, TxtNumProtocollo4.Text, "limitatasingola1")
    ''End If
    ''If TxtNumProtocollo5.Text <> "" Then
    '    Call SalvaProtocolli(TxtDataProtocollo5.Text, TxtNumProtocollo5.Text, "negativaplurima")
    ''End If
    ''If TxtNumProtocollo6.Text <> "" Then
    '    Call SalvaProtocolli(TxtDataProtocollo6.Text, TxtNumProtocollo6.Text, "negativasingola1")
    ''End If
    '    Call SalvaProtocolli(TxtDataProtocollo7.Text, TxtNumProtocollo7.Text, "allegatonegativi1")

    '    Call SalvaProtocolli(TxtDataProtocollo8.Text, TxtNumProtocollo8.Text, "allegatopositivi1")

    '    Call SalvaProtocolli(TxtDataProtocollo9.Text, TxtNumProtocollo9.Text, "allegatopositivilimitati1")

    ''Else
    ''messaggio - sicuro di cancellare?
    ''End If
    '    TxtNumFascicoloControllo.Text = Trim(TxtNumeroFascicolo.Text)
    '    hddModificaProtocollo.Value = 0
    'End Sub


    Protected Sub imgGeneraFile_Click(sender As Object, e As EventArgs) Handles imgGeneraFile.Click
        'controllo se è stato selezionato un ente
        If (Session("IdEnte") > -1) And (Not Session("IdEnte") Is Nothing) Then

            '*********************************************************************************************************************
            '*********************************************NUOVA GESTIONE MODELLI**************************************************
            '*********************************************************************************************************************
            'aggiunta nuova gestione della creazione dei documenti 
            'modifica aggiunta da Un, Due, Tre.....JONATHAN!!! il 25/01/2008
            'controllo se i due check sono flaggati e genero i doc da far scaricare

            If ChkLetteraPositivaNegativa.Checked = True Then
                hplPositivaNegativa.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplPositivaNegativa.NavigateUrl = Documento.PROG_letteraditrasmissione1(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("letteraditrasmissione1")
                If Session("TipoUtente") = "U" Then
                    ' Gruppo1(True)
                End If
            End If
            '2 COMUNICAZIONE POSITIVA
            If chkletteraNegativa.Checked = True Then
                hplletteraNegativa.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplletteraNegativa.NavigateUrl = Documento.PROG_comunicazionepositiva1(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("comunicazionepositiva1")
                If Session("TipoUtente") = "U" Then
                    ' Gruppo2(True)
                End If
            End If
            '3 DETERMINA LIMITATA PLURIMA
            If chkdetNegSingola.Checked = True Then
                hpldetNegSingola.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hpldetNegSingola.NavigateUrl = Documento.PROG_limitataplurima1(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("limitataplurima1")
                If Session("TipoUtente") = "U" Then
                    'Gruppo3(True)
                End If
            End If
            If chkLetteraPositiva.Checked = True Then
                hplLetteraPositiva.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplLetteraPositiva.NavigateUrl = Documento.PROG_limitatasingola1(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("limitatasingola1")
                If Session("TipoUtente") = "U" Then
                    ' Gruppo4(True)
                End If
            End If
            If chkDetNegMultipla.Checked = True Then
                hplDetNegMultipla.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDetNegMultipla.NavigateUrl = Documento.PROG_negativaplurima(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("negativaplurima")
                If Session("TipoUtente") = "U" Then
                    ' Gruppo5(True)
                End If
            End If
            If chkDetPosSingola.Checked = True Then
                hplDetPosSingola.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDetPosSingola.NavigateUrl = Documento.PROG_negativasingola1(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("negativasingola1")
                If Session("TipoUtente") = "U" Then
                    ' Gruppo6(True)
                End If
            End If
            If chkElencoProgNeg.Checked = True Then
                hplElencoProgNeg.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplElencoProgNeg.NavigateUrl = Documento.PROG_allegatonegativi1(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatonegativi1")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If
            If chkdetPosMultipla.Checked = True Then
                hpldetPosMultipla.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hpldetPosMultipla.NavigateUrl = Documento.PROG_allegatopositivi1(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatopositivi1")
                If Session("TipoUtente") = "U" Then
                    'Gruppo8(True)
                End If
            End If
            If chkElencoProgPos.Checked = True Then
                hplElencoProgPos.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplElencoProgPos.NavigateUrl = Documento.PROG_allegatopositivilimitati1(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("allegatopositivilimitati1")
                If Session("TipoUtente") = "U" Then
                    ' Gruppo9(True)
                End If

                'hplElencoProgPos.Visible = True
                'hplElencoProgPos.NavigateUrl = ElencoProgettiPositiviLIM(Session("IdEnte"), "allegatopositivilimitati1")
            End If
            If chkDecretoProgrammaEsclusoSingolo.Checked = True Then
                hplDecretoProgrammaEsclusoSingolo.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDecretoProgrammaEsclusoSingolo.NavigateUrl = Documento.PROG_DecretoProgrammaEsclusoSingolo(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DecretoProgrammaEsclusoSingolo")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If

            If chkDecretoProgrammaInammissibileSingolo.Checked = True Then
                hplDecretoProgrammaInammissibileSingolo.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDecretoProgrammaInammissibileSingolo.NavigateUrl = Documento.PROG_DecretoProgrammaInammissibileSingolo(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DecretoProgrammaInammissibileSingolo")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If

            If chkDecretoProgrammaPositivoconProgettiLimitatiSingolo.Checked = True Then
                hplDecretoProgrammaPositivoconProgettiLimitatiSingolo.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDecretoProgrammaPositivoconProgettiLimitatiSingolo.NavigateUrl = Documento.PROG_DecretoProgrammaPositivoconProgettiLimitatiSingolo(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DecretoProgrammaPositivoconProgettiLimitatiSingolo")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If
            If chkDecretoProgrammaRidottoSingolo.Checked = True Then
                hplDecretoProgrammaRidottoSingolo.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDecretoProgrammaRidottoSingolo.NavigateUrl = Documento.PROG_DecretoProgrammaRidottoSingolo(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DecretoProgrammaRidottoSingolo")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If
            If chkDecretoProgrammiEsclusiPlurimo.Checked = True Then
                hplDecretoProgrammiEsclusiPlurimo.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDecretoProgrammiEsclusiPlurimo.NavigateUrl = Documento.PROG_DecretoProgrammiEsclusiPlurimo(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DecretoProgrammiEsclusiPlurimo")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If
            If chkDecretoProgrammiInammissibiliPlurimo.Checked = True Then
                hplDecretoProgrammiInammissibiliPlurimo.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDecretoProgrammiInammissibiliPlurimo.NavigateUrl = Documento.PROG_DecretoProgrammiInammissibiliPlurimo(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DecretoProgrammiInammissibiliPlurimo")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If
            If chkDecretoProgrammiPositiviconProgettiLimitatiPlurimo.Checked = True Then
                hplDecretoProgrammiPositiviconProgettiLimitatiPlurimo.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDecretoProgrammiPositiviconProgettiLimitatiPlurimo.NavigateUrl = Documento.PROG_DecretoProgrammiPositiviconProgettiLimitatiPlurimo(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DecretoProgrammiPositiviconProgettiLimitatiPlurimo")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If
            If chkDecretoProgrammiRidottiPlurimo.Checked = True Then
                hplDecretoProgrammiRidottiPlurimo.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplDecretoProgrammiRidottiPlurimo.NavigateUrl = Documento.PROG_DecretoProgrammiRidottiPlurimo(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("DecretoProgrammiRidottiPlurimo")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If
            If chkLetteraTrasmissioneNegativi.Checked = True Then
                hplLetteraTrasmissioneNegativi.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplLetteraTrasmissioneNegativi.NavigateUrl = Documento.PROG_LetteraTrasmissioneNegativi(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("LetteraTrasmissioneNegativi")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If
            If chkLetteraTrasmissionePositivi.Checked = True Then
                hplLetteraTrasmissionePositivi.Visible = True
                Dim Documento As New GeneratoreModelli
                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                hplLetteraTrasmissionePositivi.NavigateUrl = Documento.PROG_LetteraTrasmissionePositivi(Session("IdEnte"), ddlBando.SelectedItem.Value, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                Documento.Dispose()

                NuovaCronologia("LetteraTrasmissionePositivi")
                If Session("TipoUtente") = "U" Then
                    'Gruppo7(True)
                End If
            End If
            ''1 LETTERA DI TRASMISSIONE
            'If ChkLetteraPositivaNegativa.Checked = True Then
            '    hplPositivaNegativa.Visible = True
            '    hplPositivaNegativa.NavigateUrl = TRASMISSIONE_COMUNICAZIONE_DETERMINALIMITATAPLURIMA_DETERMINANEGATIVAPLURIMA(Session("IdEnte"), "letteraditrasmissione1")
            'End If
            ''2 COMUNICAZIONE POSITIVA
            'If chkletteraNegativa.Checked = True Then
            '    hplletteraNegativa.Visible = True
            '    hplletteraNegativa.NavigateUrl = TRASMISSIONE_COMUNICAZIONE_DETERMINALIMITATAPLURIMA_DETERMINANEGATIVAPLURIMA(Session("IdEnte"), "comunicazionepositiva1")
            'End If
            ''3 DETERMINA LIMITATA PLURIMA
            'If chkdetNegSingola.Checked = True Then
            '    hpldetNegSingola.Visible = True
            '    hpldetNegSingola.NavigateUrl = TRASMISSIONE_COMUNICAZIONE_DETERMINALIMITATAPLURIMA_DETERMINANEGATIVAPLURIMA(Session("IdEnte"), "limitataplurima1")
            'End If
            ''4 DETERMINA LIMITATA SINGOLA
            'If chkLetteraPositiva.Checked = True Then
            '    hplLetteraPositiva.Visible = True
            '    hplLetteraPositiva.NavigateUrl = DeterminaSingolaLIM(Session("IdEnte"), "limitatasingola1")
            'End If
            '5 DETERMINA NEGATIVA PLURIMA
            'If chkDetNegMultipla.Checked = True Then
            '    hplDetNegMultipla.Visible = True
            '    hplDetNegMultipla.NavigateUrl = TRASMISSIONE_COMUNICAZIONE_DETERMINALIMITATAPLURIMA_DETERMINANEGATIVAPLURIMA(Session("IdEnte"), "negativaplurima")
            'End If
            '6 DETERMINA NEGATIVA SINGOLA
            'If chkDetPosSingola.Checked = True Then
            '    hplDetPosSingola.Visible = True
            '    hplDetPosSingola.NavigateUrl = DeterminaSingolaNegativa(Session("IdEnte"), "negativasingola1")
            'End If
            '7 ALLEGATO ELENCO PROGETTI NEGATIVI
            'If chkElencoProgNeg.Checked = True Then
            '    hplElencoProgNeg.Visible = True
            '    hplElencoProgNeg.NavigateUrl = ElencoProgettiNegativi(Session("IdEnte"), "allegatonegativi1")
            'End If
            '8 ALLEGATO ELENCO PROGETTI POSITIVI
            'If chkdetPosMultipla.Checked = True Then
            '    hpldetPosMultipla.Visible = True
            '    hpldetPosMultipla.NavigateUrl = ElencoProgettiPositivi(Session("IdEnte"), "allegatopositivi1")
            'End If
            '9 ALLEGATO ELENCO PROGETTI POSITIVI LIMITATI
            'If chkElencoProgPos.Checked = True Then
            '    hplElencoProgPos.Visible = True
            '    hplElencoProgPos.NavigateUrl = ElencoProgettiPositiviLIM(Session("IdEnte"), "allegatopositivilimitati1")
            'End If
            '*********************************************************************************************************************
            '*********************************************************************************************************************
            '*********************************************************************************************************************

            chkletteraNegativa.Enabled = False
            ChkLetteraPositivaNegativa.Enabled = False
            chkLetteraPositiva.Enabled = False
            chkdetNegSingola.Enabled = False
            chkDetNegMultipla.Enabled = False
            chkDetPosSingola.Enabled = False
            chkdetPosMultipla.Enabled = False
            chkElencoProgNeg.Enabled = False
            chkElencoProgPos.Enabled = False
            chkDecretoProgrammaEsclusoSingolo.Enabled = False
            chkDecretoProgrammaInammissibileSingolo.Enabled = False
            chkDecretoProgrammaPositivoconProgettiLimitatiSingolo.Enabled = False
            chkDecretoProgrammaRidottoSingolo.Enabled = False
            chkDecretoProgrammiEsclusiPlurimo.Enabled = False
            chkDecretoProgrammiInammissibiliPlurimo.Enabled = False
            chkDecretoProgrammiPositiviconProgettiLimitatiPlurimo.Enabled = False
            chkDecretoProgrammiRidottiPlurimo.Enabled = False
            chkLetteraTrasmissioneNegativi.Enabled = False
            chkLetteraTrasmissionePositivi.Enabled = False
            'ìimgGeneraFile.Enabled = False
        Else
            lblmessaggiosopra.Visible = True
            lblmessaggiosopra.Text = "Occorre selezionare un ente."
            'Imgerrore.Visible = True
        End If
    End Sub

    Protected Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        If (HdValoreSalva.Value = 1 Or hddModificaProtocollo.Value = 1) Then
            'Call Salvataggio()
        End If
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class