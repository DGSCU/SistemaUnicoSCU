Imports System.IO
Public Class WFrmReportisticaBase
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then
            CboBando.DataTextField = "BandoBreve"
            CboBando.DataValueField = "IdBando"
            'controllo se l'utente può vedere o meno i bandi attivi
            If Session("VisualizzaStatoProgetti") = True Then
                CboBando.DataSource = ClsServer.CreaDataTable("Select IdBando,BandoBreve From Bando", False, Session("Conn"))
            Else
                'controllo il bit VisualizzaStato su Bando per nascondere o meno lo stato del progetto di bandi in valutazione
                CboBando.DataSource = ClsServer.CreaDataTable("Select IdBando,BandoBreve From Bando WHERE VisualizzaStato=1", False, Session("Conn"))
            End If
            CboBando.DataBind()

            CboBando1.DataTextField = "BandoBreve"
            CboBando1.DataValueField = "IdBando"
            CboBando1.DataSource = ClsServer.CreaDataTable("Select IdBando,BandoBreve From Bando", False, Session("Conn"))
            CboBando1.DataBind()

            CboBando2.DataTextField = "BandoBreve"
            CboBando2.DataValueField = "IdBando"
            CboBando2.DataSource = ClsServer.CreaDataTable("Select IdBando,BandoBreve From Bando Where Anno <> 'Progetti 2005'", False, Session("Conn"))
            CboBando2.DataBind()

            CboBando3.DataTextField = "BandoBreve"
            CboBando3.DataValueField = "IdBando"
            CboBando3.DataSource = ClsServer.CreaDataTable("Select IdBando,BandoBreve From Bando Where Anno <> 'Progetti 2005'", False, Session("Conn"))
            CboBando3.DataBind()

            CboBando4.DataTextField = "BandoBreve"
            CboBando4.DataValueField = "IdBando"
            CboBando4.DataSource = ClsServer.CreaDataTable("Select IdBando,BandoBreve From Bando", False, Session("Conn"))
            CboBando4.DataBind()

            CboBando5.DataTextField = "BandoBreve"
            CboBando5.DataValueField = "IdBando"
            CboBando5.DataSource = ClsServer.CreaDataTable("Select IdBando,BandoBreve From Bando", False, Session("Conn"))
            CboBando5.DataBind()
        End If
    End Sub
    

    Private Sub VisualizzaLink(ByVal IntTipo As Int16)
        Select Case IntTipo
            Case 0
                hlEntiClassiRiep.Visible = True
            Case 1
                hlEntiClassi.Visible = True
            Case 2
                hlEntiRegioni.Visible = True
            Case 3
                hlEntiRegioniRiep.Visible = True
            Case 4
                hlEntiProgetti.Visible = True
            Case 5
                hlProgettiBando.Visible = True
            Case 6
                hlProgettiRegioneRiep.Visible = True
            Case 7
                hlProgettiRegione.Visible = True
            Case 8
                hlVolontari.Visible = True
            Case 9
                hlVolontariRiep.Visible = True
        End Select
    End Sub

    Private Sub NascondiLink()
        hlEntiClassiRiep.Visible = False
        hlEntiClassi.Visible = False
        hlEntiRegioni.Visible = False
        hlEntiRegioniRiep.Visible = False
        hlEntiProgetti.Visible = False
        hlProgettiBando.Visible = False
        hlProgettiRegioneRiep.Visible = False
        hlProgettiRegione.Visible = False
        hlVolontari.Visible = False
        hlVolontariRiep.Visible = False
    End Sub

    Private Sub EsportaEntiClassi()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            StrSql = "Select Enti.CodiceRegione,Enti.Denominazione,Enti.IdClasseAccreditamento,StatiEnti.Statoente,Enti.PartitaIva,Enti.CodiceFiscale," & _
                     "IsNull(Enti.PrefissoTelefonoRichiestaRegistrazione,'') + '/' + IsNull(Enti.TelefonoRichiestaRegistrazione,'') As Telefono," & _
                     "IsNull(Enti.PrefissoFax,'') + '/' + IsNull(Enti.Fax,'') As Fax,Enti.Email,Enti.Http,Enti.Tipologia," & _
                     "(SELECT (COUNT(*) + (SELECT  count(*) FROM EntiSedi " & _
                     "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                     "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                     "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                     "WHERE(StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                     "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) " & _
                     "AND (EntiSedi.IDEnte = Enti.IdEnte) And (substring(entisedi.usernamestato,1,1) <> 'N')))as NSedi " & _
                     "FROM EntiSedi " & _
                     "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                     "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                     "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                     "INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio " & _
                     "INNER JOIN AssociaEntiRelazioniSediAttuazioni " & _
                     "ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione AND " & _
                     "Entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione " & _
                     "WHERE(StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                     "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) " & _
                     "AND (entirelazioni.IDEntePadre = enti.idente) " & _
                     "AND (GETDATE() BETWEEN ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') " & _
                     "AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01') " & _
                     "AND (Substring(entisedi.usernamestato,1,1) <> 'N') )) as NumeroTotaleSedi, " & _
                     "(Select TOP 1 Entisedi.Indirizzo + Case IsNull(EntiSedi.Civico,'') WHEN '' THEN '' ELSE ',' + EntiSedi.Civico END + ' - ' + EntiSedi.Cap + ' ' + Comuni.denominazione + '(' + Provincie.DescrAbb + ')' " & _
                     "From EntiSedi " & _
                     "INNER JOIN EntiSediTipi ON EntiSedi.IdEnteSede = EntiSediTipi.IdEnteSede And EntiSediTipi.IdTipoSede = 1 " & _
                     "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                     "INNER JOIN Provincie ON Comuni.Idprovincia = Provincie.IdProvincia " & _
                     "WHERE EntiSedi.IdEnte = Enti.IdEnte) AS IndirtizzoLegale " & _
                     "From Enti " & _
                     "Inner Join StatiEnti on (statienti.idstatoente=enti.idstatoente) " & _
                     "Where StatiEnti.Chiuso <> 1 "

            If CboClasse.SelectedValue = 0 Then
                StrSql = StrSql & "AND Enti.IdClasseAccreditamento <= 4 "
            ElseIf CboClasse.SelectedValue = 1 Then
                StrSql = StrSql & "AND Enti.IdClasseAccreditamento = 1 "
            ElseIf CboClasse.SelectedValue = 2 Then
                StrSql = StrSql & "AND Enti.IdClasseAccreditamento = 2 "
            ElseIf CboClasse.SelectedValue = 3 Then
                StrSql = StrSql & "AND Enti.IdClasseAccreditamento = 3 "
            ElseIf CboClasse.SelectedValue = 4 Then
                StrSql = StrSql & "AND Enti.IdClasseAccreditamento = 4 "
            End If

            StrSql = StrSql & " Order by Enti.IdClasseAccreditamento,Enti.CodiceRegione,Enti.Denominazione"
            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpEntiClassi" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Codice;Denominazione;Classe;Stato;Partita Iva;Codice Fiscale;Telefono;Fax;Email;Http;Tipologia;Indirizzo Sede;Tot. Sedi"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If
                    If IsDBNull(DtrReport(3)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(3)) & ";"
                    End If
                    If IsDBNull(DtrReport(4)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(4)) & ";"
                    End If
                    If IsDBNull(DtrReport(5)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(5)) & ";"
                    End If
                    If IsDBNull(DtrReport(6)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(6)) & ";"
                    End If
                    If IsDBNull(DtrReport(7)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(7)) & ";"
                    End If
                    If IsDBNull(DtrReport(8)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(8)) & ";"
                    End If
                    If IsDBNull(DtrReport(9)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(9)) & ";"
                    End If
                    If IsDBNull(DtrReport(10)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(10)) & ";"
                    End If
                    If IsDBNull(DtrReport(12)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(12)) & ";"
                    End If
                    If IsDBNull(DtrReport(11)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(11)) & ";"
                    End If

                    Writer.WriteLine(xLinea)
                End While
                hlEntiClassi.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If


            DtrReport.Close()
            DtrReport = Nothing



        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione del report."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaEntiRegioni()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            'per la regione Trentino Alto Adige la query lavora con le province Bolzano o Trento
            If CboRegione.SelectedValue = "Bolzano - Bozen" Or CboRegione.SelectedValue = "Trento" Then

                StrSql = "Select '" & ClsServer.NoApice(CboRegione.SelectedItem.Text) & "',Enti.CodiceRegione,Enti.Denominazione,Enti.IdClasseAccreditamento,StatiEnti.Statoente,Enti.PartitaIva,Enti.CodiceFiscale," & _
                        "IsNull(Enti.PrefissoTelefonoRichiestaRegistrazione,'') + '/' + IsNull(Enti.TelefonoRichiestaRegistrazione,'') As Telefono," & _
                        "IsNull(Enti.PrefissoFax,'') + '/' + IsNull(Enti.Fax,'') As Fax,Enti.Email,Enti.Http,Enti.Tipologia," & _
                        "(SELECT (COUNT(*) + (SELECT  count(*) FROM EntiSedi " & _
                        "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                        "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                        "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                        "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                        "INNER JOIN Provincie ON Comuni.Idprovincia = Provincie.IdProvincia " & _
                        "INNER JOIN Regioni On Provincie.IdRegione = Regioni.IdRegione " & _
                        "WHERE provincie.provincia = '" & ClsServer.NoApice(CboRegione.SelectedItem.Text) & "' AND (StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                        "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) " & _
                        "AND (EntiSedi.IDEnte = Enti.IdEnte) And (substring(entisedi.usernamestato,1,1) <> 'N')))as NSedi " & _
                        "FROM EntiSedi " & _
                        "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                        "INNER JOIN Provincie ON Comuni.Idprovincia = Provincie.IdProvincia " & _
                        "INNER JOIN Regioni On Provincie.IdRegione = Regioni.IdRegione " & _
                        "INNER JOIN Entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                        "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                        "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                        "INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio " & _
                        "INNER JOIN AssociaEntiRelazioniSediAttuazioni " & _
                        "ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione AND " & _
                        "Entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione " & _
                        "WHERE provincie.provincia = '" & ClsServer.NoApice(CboRegione.SelectedItem.Text) & "' AND (StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                        "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) " & _
                        "AND (entirelazioni.IDEntePadre = enti.idente) " & _
                        "AND (GETDATE() BETWEEN ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') " & _
                        "AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01') " & _
                        "AND (Substring(entisedi.usernamestato,1,1) <> 'N') )) as NumeroTotaleSedi, " & _
                        "(Select TOP 1 Entisedi.Indirizzo + Case IsNull(EntiSedi.Civico,'') WHEN '' THEN '' ELSE ',' + EntiSedi.Civico END + ' - ' + EntiSedi.Cap + ' ' + Comuni.denominazione + '(' + Provincie.DescrAbb + ')' " & _
                        "From EntiSedi " & _
                        "INNER JOIN EntiSediTipi ON EntiSedi.IdEnteSede = EntiSediTipi.IdEnteSede And EntiSediTipi.IdTipoSede = 1 " & _
                        "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                        "INNER JOIN Provincie ON Comuni.Idprovincia = Provincie.IdProvincia " & _
                        "WHERE EntiSedi.IdEnte = Enti.IdEnte) AS IndirtizzoLegale " & _
                        "From Enti " & _
                        "Inner Join StatiEnti on (statienti.idstatoente=enti.idstatoente) " & _
                        "Where StatiEnti.Chiuso <> 1 And Enti.IdClasseAccreditamento <= 4 " & _
                        "Order by Enti.IdClasseAccreditamento,Enti.CodiceRegione,Enti.Denominazione"


            Else


                StrSql = "Select '" & ClsServer.NoApice(CboRegione.SelectedItem.Text) & "',Enti.CodiceRegione,Enti.Denominazione,Enti.IdClasseAccreditamento,StatiEnti.Statoente,Enti.PartitaIva,Enti.CodiceFiscale," & _
                         "IsNull(Enti.PrefissoTelefonoRichiestaRegistrazione,'') + '/' + IsNull(Enti.TelefonoRichiestaRegistrazione,'') As Telefono," & _
                         "IsNull(Enti.PrefissoFax,'') + '/' + IsNull(Enti.Fax,'') As Fax,Enti.Email,Enti.Http,Enti.Tipologia," & _
                         "(SELECT (COUNT(*) + (SELECT  count(*) FROM EntiSedi " & _
                         "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                         "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                         "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                         "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                         "INNER JOIN Provincie ON Comuni.Idprovincia = Provincie.IdProvincia " & _
                         "INNER JOIN Regioni On Provincie.IdRegione = Regioni.IdRegione " & _
                         "WHERE Regioni.Regione = '" & ClsServer.NoApice(CboRegione.SelectedItem.Text) & "' AND (StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                         "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) " & _
                         "AND (EntiSedi.IDEnte = Enti.IdEnte) And (substring(entisedi.usernamestato,1,1) <> 'N')))as NSedi " & _
                         "FROM EntiSedi " & _
                         "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                         "INNER JOIN Provincie ON Comuni.Idprovincia = Provincie.IdProvincia " & _
                         "INNER JOIN Regioni On Provincie.IdRegione = Regioni.IdRegione " & _
                         "INNER JOIN Entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                         "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                         "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                         "INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio " & _
                         "INNER JOIN AssociaEntiRelazioniSediAttuazioni " & _
                         "ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione AND " & _
                         "Entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione " & _
                         "WHERE Regioni.Regione = '" & ClsServer.NoApice(CboRegione.SelectedItem.Text) & "' AND (StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                         "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) " & _
                         "AND (entirelazioni.IDEntePadre = enti.idente) " & _
                         "AND (GETDATE() BETWEEN ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') " & _
                         "AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01') " & _
                         "AND (Substring(entisedi.usernamestato,1,1) <> 'N') )) as NumeroTotaleSedi, " & _
                         "(Select TOP 1 Entisedi.Indirizzo + Case IsNull(EntiSedi.Civico,'') WHEN '' THEN '' ELSE ',' + EntiSedi.Civico END + ' - ' + EntiSedi.Cap + ' ' + Comuni.denominazione + '(' + Provincie.DescrAbb + ')' " & _
                         "From EntiSedi " & _
                         "INNER JOIN EntiSediTipi ON EntiSedi.IdEnteSede = EntiSediTipi.IdEnteSede And EntiSediTipi.IdTipoSede = 1 " & _
                         "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                         "INNER JOIN Provincie ON Comuni.Idprovincia = Provincie.IdProvincia " & _
                         "WHERE EntiSedi.IdEnte = Enti.IdEnte) AS IndirtizzoLegale " & _
                         "From Enti " & _
                         "Inner Join StatiEnti on (statienti.idstatoente=enti.idstatoente) " & _
                         "Where StatiEnti.Chiuso <> 1 And Enti.IdClasseAccreditamento <= 4 " & _
                         "Order by Enti.IdClasseAccreditamento,Enti.CodiceRegione,Enti.Denominazione"
            End If

            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpEntiRegioni" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Regione;Codice;Denominazione;Classe;Stato;Partita Iva;Codice Fiscale;Telefono;Fax;Email;Http;Tipologia;Sede Legale;Tot. Sedi " & CboRegione.SelectedItem.Text
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If
                    If IsDBNull(DtrReport(3)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(3)) & ";"
                    End If
                    If IsDBNull(DtrReport(4)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(4)) & ";"
                    End If
                    If IsDBNull(DtrReport(5)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(5)) & ";"
                    End If
                    If IsDBNull(DtrReport(6)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(6)) & ";"
                    End If
                    If IsDBNull(DtrReport(7)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(7)) & ";"
                    End If
                    If IsDBNull(DtrReport(8)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(8)) & ";"
                    End If
                    If IsDBNull(DtrReport(9)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(9)) & ";"
                    End If
                    If IsDBNull(DtrReport(10)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(10)) & ";"
                    End If
                    If IsDBNull(DtrReport(11)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(11)) & ";"
                    End If
                    If IsDBNull(DtrReport(13)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(13)) & ";"
                    End If
                    If IsDBNull(DtrReport(12)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(12)) & ";"
                    End If
                    Writer.WriteLine(xLinea)
                End While
                hlEntiRegioni.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            DtrReport.Close()
            DtrReport = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione delle Sedi di Attuazione."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaEntiClassiRiepilogo()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            StrSql = "Select ClassiAccreditamento.IdClasseAccreditamento, " & _
                     "(Select Count(*) From Enti " & _
                     "Where(Enti.IdClasseAccreditamento = ClassiAccreditamento.IdClasseAccreditamento) " & _
                     "And Enti.IdStatoEnte IN (3,9)) As Enti, " & _
                     "(SELECT COUNT(*) + (SELECT COUNT(*) FROM EntiSedi " & _
                     "INNER JOIN Enti ON EntiSedi.IdEnte = Enti.IdEnte " & _
                     "INNER JOIN StatiEnti ON Enti.IdStatoEnte = StatiEnti.IdStatoEnte " & _
                     "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                     "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                     "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                     "WHERE(StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                     "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) " & _
                     "AND Enti.IdClasseAccreditamento = ClassiAccreditamento.IdClasseAccreditamento " & _
                     "AND Substring(entisedi.usernamestato,1,1) <> 'N' " & _
                     "AND StatiEnti.Chiuso <> 1) " & _
                     "FROM EntiSedi " & _
                     "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                     "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                     "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                     "INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio " & _
                     "INNER JOIN AssociaEntiRelazioniSediAttuazioni ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione AND Entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione " & _
                     "INNER JOIN Enti ON EntiRelazioni.IdEntePadre = Enti.IdEnte " & _
                     "INNER JOIN StatiEnti ON Enti.IdStatoEnte = StatiEnti.IdStatoEnte " & _
                     "WHERE(Enti.IdClasseAccreditamento = ClassiAccreditamento.IdClasseAccreditamento) " & _
                     "AND (StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                     "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) " & _
                     "AND (GETDATE() BETWEEN ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') " & _
                     "AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01') " & _
                     "AND Substring(entisedi.usernamestato,1,1) <> 'N' " & _
                     "AND StatiEnti.Chiuso <> 1)) AS Sedi " & _
                     "FROM ClassiAccreditamento " & _
                     "Where ClassiAccreditamento.IdClasseAccreditamento <= 4 " & _
                     "Group By ClassiAccreditamento.IdClasseAccreditamento " & _
                     "Order by ClassiAccreditamento.IdClasseAccreditamento"
            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                Dim LngTotEnti As Long = 0
                Dim LngTotSedi As Long = 0

                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpEntiClassiRiep" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Classe;N. Enti;N. Sedi;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngTotEnti = LngTotEnti + DtrReport(1)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngTotSedi = LngTotSedi + DtrReport(2)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If
                    Writer.WriteLine(xLinea)
                End While
                Writer.WriteLine("Totale;" & LngTotEnti.ToString & ";" & LngTotSedi.ToString & ";")
                hlEntiClassiRiep.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            DtrReport.Close()
            DtrReport = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione del report."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaEntiRegioniRiepilogo()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            StrSql = "Select A.Regione, " & _
                     "(SELECT COUNT(Distinct IdEnte) From VW_Elenco_Sedi Where VW_Elenco_Sedi.Regione = A.Regione And VW_Elenco_Sedi.[Classe Accreditamento] <= 4) as Enti, " & _
                     "(SELECT (COUNT(*) + (SELECT  count(*) " & _
                     "FROM EntiSedi " & _
                     "INNER JOIN Enti ON EntiSedi.IdEnte = Enti.IdEnte " & _
                     "INNER JOIN StatiEnti ON StatiEnti.IdStatoEnte = Enti.IdStatoEnte " & _
                     "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                     "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                     "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                     "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                     "INNER JOIN Provincie ON Comuni.Idprovincia = Provincie.IdProvincia " & _
                     "INNER JOIN Regioni On Provincie.IdRegione = Regioni.IdRegione " & _
                     "WHERE Regioni.Regione = A.Regione " & _
                     "AND StatiEnti.Chiuso <> 1 And Enti.IdClasseAccreditamento <= 4 " & _
                     "AND (StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                     "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) " & _
                     "AND (EntiSedi.IDEnte = Enti.IdEnte) And (substring(entisedi.usernamestato,1,1) <> 'N')))as NSedi " & _
                     "FROM EntiSedi " & _
                     "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                     "INNER JOIN Provincie ON Comuni.Idprovincia = Provincie.IdProvincia " & _
                     "INNER JOIN Regioni On Provincie.IdRegione = Regioni.IdRegione " & _
                     "INNER JOIN Entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
                     "INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                     "INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " & _
                     "INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio " & _
                     "INNER JOIN Enti ON EntiRelazioni.IdEntePadre = Enti.IdEnte " & _
                     "INNER JOIN StatiEnti ON StatiEnti.IdStatoEnte = Enti.IdStatoEnte " & _
                     "INNER JOIN AssociaEntiRelazioniSediAttuazioni ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione AND Entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione " & _
                     "WHERE Regioni.Regione = A.Regione " & _
                     "AND StatiEnti.Chiuso <> 1 And Enti.IdClasseAccreditamento <= 4 " & _
                     "AND (StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1) " & _
                     "AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1) AND (entirelazioni.IDEntePadre = enti.idente) " & _
                     "AND (GETDATE() BETWEEN ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') AND " & _
                     "ISNULL(entirelazioni.DataFineValidità,'2030-01-01') AND (Substring(entisedi.usernamestato,1,1) <> 'N') )) as Sedi " & _
                     "From Regioni As A " & _
                     "Where A.IdNazione = 1 And A.Regione <> 'ITALIA' " & _
                     "Order by A.Regione "
            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                Dim LngTotSedi As Long = 0
                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpEntiRegioniRiep" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Regione;N. Enti;N. Sedi;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngTotSedi = LngTotSedi + DtrReport(2)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If
                    Writer.WriteLine(xLinea)
                End While
                Writer.WriteLine("ITALIA;;" & LngTotSedi & ";")
                hlEntiRegioniRiep.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            DtrReport.Close()
            DtrReport = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione delle Sedi di Attuazione."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaEntiProgetti()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            StrSql = "Select Enti.CodiceRegione,Enti.Denominazione,Enti.IdClasseAccreditamento,StatiEnti.Statoente,Enti.PartitaIva,Enti.CodiceFiscale, " & _
                     "IsNull(Enti.PrefissoTelefonoRichiestaRegistrazione,'') + '/' + IsNull(Enti.TelefonoRichiestaRegistrazione,'') As Telefono, " & _
                     "IsNull(Enti.PrefissoFax,'') + '/' + IsNull(Enti.Fax,'') As Fax,Enti.Email,Enti.Http,Enti.Tipologia, " & _
                     "(Select Count(*) From Attività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "WHERE Attività.IdStatoAttività = 4 AND BandiAttività.IdEnte = Enti.IdEnte And BandiAttività.IdStatoBandoAttività IN (2,3,5) And BandiAttività.IdBando = " & CboBando.SelectedValue & ") As Proposti, " & _
                     "(Select Count(*) From Attività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "WHERE Attività.IdStatoAttività = 9 AND BandiAttività.IdEnte = Enti.IdEnte And BandiAttività.IdStatoBandoAttività IN (2,3,5) And BandiAttività.IdBando = " & CboBando.SelectedValue & ") As AttesaGraduatoria, " & _
                     "(Select Count(*) From Attività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "WHERE Attività.IdStatoAttività = 1 AND BandiAttività.IdEnte = Enti.IdEnte And BandiAttività.IdStatoBandoAttività IN (2,3,5) And BandiAttività.IdBando = " & CboBando.SelectedValue & ") As Attivi, " & _
                     "(Select Count(*) From Attività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "WHERE Attività.IdStatoAttività = 11 AND BandiAttività.IdEnte = Enti.IdEnte And BandiAttività.IdStatoBandoAttività IN (2,3,5) And BandiAttività.IdBando = " & CboBando.SelectedValue & ") As Sospesi, " & _
                     "(Select Count(*) From Attività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "WHERE Attività.IdStatoAttività = 2 AND BandiAttività.IdEnte = Enti.IdEnte And BandiAttività.IdStatoBandoAttività IN (2,3,5) And BandiAttività.IdBando = " & CboBando.SelectedValue & ") As Terminati, " & _
                     "(Select Count(*) From Attività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "WHERE Attività.IdStatoAttività = 6 AND BandiAttività.IdEnte = Enti.IdEnte And BandiAttività.IdStatoBandoAttività IN (2,3,5) And BandiAttività.IdBando = " & CboBando.SelectedValue & ") As Inammissibili, " & _
                     "(Select Count(*) From Attività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "WHERE Attività.IdStatoAttività = 7 AND BandiAttività.IdEnte = Enti.IdEnte And BandiAttività.IdStatoBandoAttività IN (2,3,5) And BandiAttività.IdBando = " & CboBando.SelectedValue & ") As Respinti " & _
                     "From Enti " & _
                     "Inner Join StatiEnti on statienti.idstatoente=enti.idstatoente " & _
                     "Where StatiEnti.Chiuso <> 1 and Enti.IdClasseAccreditamento <= 4 " & _
                     "Order By Enti.IdClasseAccreditamento,Enti.CodiceRegione"
            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                Dim LngProposti As Long = 0
                Dim LngAttesaGraduatoria As Long = 0
                Dim LngAttivi As Long = 0
                Dim LngSospesi As Long = 0
                Dim LngTerminati As Long = 0
                Dim LngInammissibili As Long = 0
                Dim LngRespinti As Long = 0

                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpEntiProgetti" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Codice;Denominazione;Classe;Stato;Partita Iva;Codice Fiscale;Telefono;Fax;Email;Http;Tipologia;Bando;Proposti;Attesa Graduatoria;Attivi;Sospesi;Terminati;Inammissibili;Respinti;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If
                    If IsDBNull(DtrReport(3)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(3)) & ";"
                    End If
                    If IsDBNull(DtrReport(4)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(4)) & ";"
                    End If
                    If IsDBNull(DtrReport(5)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(5)) & ";"
                    End If
                    If IsDBNull(DtrReport(6)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(6)) & ";"
                    End If
                    If IsDBNull(DtrReport(7)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(7)) & ";"
                    End If
                    If IsDBNull(DtrReport(8)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(8)) & ";"
                    End If
                    If IsDBNull(DtrReport(9)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(9)) & ";"
                    End If
                    If IsDBNull(DtrReport(10)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(10)) & ";"
                    End If

                    xLinea = xLinea & ClsUtility.FormatExport(CboBando.SelectedItem.Text) & ";"

                    If IsDBNull(DtrReport(11)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngProposti = LngProposti + DtrReport(11)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(11)) & ";"
                    End If
                    If IsDBNull(DtrReport(12)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngAttesaGraduatoria = LngAttesaGraduatoria + DtrReport(12)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(12)) & ";"
                    End If
                    If IsDBNull(DtrReport(13)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngAttivi = LngAttivi + DtrReport(13)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(13)) & ";"
                    End If
                    If IsDBNull(DtrReport(14)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngSospesi = LngSospesi + DtrReport(14)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(14)) & ";"
                    End If
                    If IsDBNull(DtrReport(15)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngTerminati = LngTerminati + DtrReport(15)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(15)) & ";"
                    End If
                    If IsDBNull(DtrReport(16)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngInammissibili = LngInammissibili + DtrReport(16)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(16)) & ";"
                    End If
                    If IsDBNull(DtrReport(17)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngRespinti = LngRespinti + DtrReport(17)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(17)) & ";"
                    End If
                    Writer.WriteLine(xLinea)
                End While
                Writer.WriteLine("Totale;;;;;;;;;;;" & LngProposti.ToString & ";" & LngAttesaGraduatoria.ToString & ";" & LngAttivi.ToString & ";" & LngSospesi.ToString & ";" & LngTerminati.ToString & ";" & LngInammissibili.ToString & ";" & LngRespinti.ToString & ";")
                hlEntiProgetti.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            DtrReport.Close()
            DtrReport = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione del report."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaProgettiBando()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String
        'variabile per stabilire se visualizzare il puntegio omeno
        Dim blnPunteggio As Boolean

        'Verifico le Abilitazione Dell'Utente
        StrSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu,VociMenu.descrizione, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                   " VociMenu.IdVoceMenuPadre" & _
                   " FROM VociMenu" & _
                   " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                   " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                   " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                   " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                   " WHERE (VociMenu.descrizione = 'Valutazione Qualità Progetti')" & _
                   " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        If Not DtrReport Is Nothing Then
            DtrReport.Close()
            DtrReport = Nothing
        End If

        DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

        'se c'è la necessaria abilitazione al menu mostro il punteggio
        blnPunteggio = DtrReport.HasRows

        If Not DtrReport Is Nothing Then
            DtrReport.Close()
            DtrReport = Nothing
        End If

        Try
            StrSql = "Select '" & CboBando1.SelectedItem.Text & "',Attività.CodiceEnte,Attività.Titolo,"
            If Session("VisualizzaStatoProgetti") = True Then
                StrSql = StrSql & "StatiAttività.StatoAttività, "
            Else
                'controllo il bit VisualizzaStato su Bando per nascondere o meno lo stato del progetto di bandi in valutazione
                StrSql = StrSql & "case isnull(bando.VisualizzaStato,1) when 1 then StatiAttività.StatoAttività else '' end as StatoAttività, "
            End If
            StrSql = StrSql & "Enti.CodiceRegione,Enti.IdClasseAccreditamento, " & _
                     "Attività.DataInizioPrevista,Attività.DataInizioAttività,Attività.DataFinePrevista,Attività.DataFineAttività, " & _
                     "Attività.NumeroPostiNoVittoNoAlloggio,Attività.NumeroPostiVittoAlloggio,Attività.NumeroPostiVitto, " & _
                     "MacroAmbitiAttività.Codifica + ' - ' + MacroAmbitiAttività.MacroAmbitoAttività, " & _
                     "AmbitiAttività.Codifica + ' - ' + AmbitiAttività.AmbitoAttività,TipiProgetto.Descrizione, "
            If blnPunteggio = True Then
                If Session("VisualizzaStatoProgetti") = True Then
                    StrSql = StrSql & "IsNull(Attività.PunteggioFinale,0) "
                Else
                    'controllo il bit VisualizzaStato su Bando per nascondere o meno lo stato del progetto di bandi in valutazione
                    StrSql = StrSql & "case isnull(bando.VisualizzaStato,1) when 1 then IsNull(Attività.PunteggioFinale,0) else 'Non Abilitato' end "
                End If
            Else
                StrSql = StrSql & "'Non Abilitato' "
            End If
            StrSql = StrSql & "From Attività  " & _
            "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività  " & _
            "INNER JOIN Bando ON bando.IdBando = BandiAttività.IdBando " & _
            "INNER JOIN Enti ON Attività.IdEntePresentante = Enti.IdEnte " & _
            "INNER JOIN StatiAttività ON Attività.IdStatoAttività = StatiAttività.IdStatoAttività " & _
            "INNER JOIN AmbitiAttività ON Attività.IdAmbitoAttività = AmbitiAttività.IdAmbitoAttività " & _
            "INNER JOIN MacroAmbitiAttività ON AmbitiAttività.IdMacroAmbitoAttività = MacroAmbitiAttività.IdMacroAmbitoAttività " & _
            "INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
            "WHERE BandiAttività.IdStatoBandoAttività IN (2,3,5) And BandiAttività.IdBando = " & CboBando1.SelectedValue & " and Attività.idattivitàcapofila is null " & _
            "Order by Attività.CodiceEnte,Attività.Titolo"
            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                Dim LngNoVittoNoAllooggio As Long = 0
                Dim LngVittoAlloggio As Long = 0
                Dim LngVitto As Long = 0


                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpProgettiBando" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Bando;Codice;Titolo;Stato;Codice Ente;Classe;Data Inizio Prev.;Data Inizio;Data Fine Prev.;Data Fine;Posti No Vitto No Alloggio;Posti Vitto Alloggio;Posti Vitto;Settore;Area;Tipo;Punteggio;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If
                    If IsDBNull(DtrReport(3)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(3)) & ";"
                    End If
                    If IsDBNull(DtrReport(4)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(4)) & ";"
                    End If
                    If IsDBNull(DtrReport(5)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(5)) & ";"
                    End If
                    If IsDBNull(DtrReport(6)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(6)) & ";"
                    End If
                    If IsDBNull(DtrReport(7)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(7)) & ";"
                    End If
                    If IsDBNull(DtrReport(8)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(8)) & ";"
                    End If
                    If IsDBNull(DtrReport(9)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(9)) & ";"
                    End If
                    If IsDBNull(DtrReport(10)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngNoVittoNoAllooggio = LngNoVittoNoAllooggio + DtrReport(10)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(10)) & ";"
                    End If
                    If IsDBNull(DtrReport(11)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngVittoAlloggio = LngVittoAlloggio + DtrReport(11)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(11)) & ";"
                    End If
                    If IsDBNull(DtrReport(12)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngVitto = LngVitto + DtrReport(12)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(12)) & ";"
                    End If
                    If IsDBNull(DtrReport(13)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(13)) & ";"
                    End If
                    If IsDBNull(DtrReport(14)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(14)) & ";"
                    End If
                    If IsDBNull(DtrReport(15)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(15)) & ";"
                    End If
                    If IsDBNull(DtrReport(16)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(16)) & ";"
                    End If
                    Writer.WriteLine(xLinea)
                End While
                Writer.WriteLine("TOTALE;;;;;;;;;;" & LngNoVittoNoAllooggio.ToString & ";" & LngVittoAlloggio.ToString & ";" & LngVitto.ToString & ";;;;;")
                hlProgettiBando.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            DtrReport.Close()
            DtrReport = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione del report."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaProgettiRegioneRiep()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            StrSql = "Select A.Regione, " & _
                     "(Select IsNull(SUM( " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0) + " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)),0) " & _
                     "From BandiAttività " & _
                     "INNER JOIN Attività ON BandiAttività.IdBandoAttività = Attività.IdBandoAttività " & _
                     "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                     "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = EntiSediAttuazioni.IdEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                     "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                     "INNER JOIN Provincie ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                     "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                     "WHERE Regioni.Regione = A.Regione And BandiAttività.IdStatoBandoAttività IN (2,3,5) And BandiAttività.IdBando = " & CboBando2.SelectedValue & ") As Richiesti, " & _
                     "(Select IsNull(SUM( " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0) + " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)),0) " & _
                     "From BandiAttività " & _
                     "INNER JOIN Attività ON BandiAttività.IdBandoAttività = Attività.IdBandoAttività " & _
                     "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                     "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = EntiSediAttuazioni.IdEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                     "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                     "INNER JOIN Provincie ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                     "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                     "WHERE Regioni.Regione = A.Regione And BandiAttività.IdStatoBandoAttività = 3 And Attività.IdStatoAttività IN (1,2) And BandiAttività.IdBando = " & CboBando2.SelectedValue & ") As Approvati " & _
                     "From Regioni AS A Where A.IdNazione = 1 And A.Regione <> 'ITALIA' " & _
                     "Order By A.Regione"
            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                Dim LngRichiesti As Long = 0
                Dim LngApprovati As Long = 0

                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpProgettiRegioniRiep" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Regione;Posti Richiesti;Posti Approvati;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngRichiesti = LngRichiesti + DtrReport(1)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngApprovati = LngApprovati + DtrReport(2)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If

                    Writer.WriteLine(xLinea)
                End While
                Writer.WriteLine("TOTALE;" & LngRichiesti.ToString & ";" & LngApprovati.ToString & ";")
                hlProgettiRegioneRiep.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            DtrReport.Close()
            DtrReport = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione del report."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaProgettiRegione()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String
        'variabile per stabilire se visualizzare il puntegio omeno
        Dim blnPunteggio As Boolean

        'Verifico le Abilitazione Dell'Utente
        StrSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu,VociMenu.descrizione, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                   " VociMenu.IdVoceMenuPadre" & _
                   " FROM VociMenu" & _
                   " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                   " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                   " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                   " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                   " WHERE (VociMenu.descrizione = 'Valutazione Qualità Progetti')" & _
                   " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        If Not DtrReport Is Nothing Then
            DtrReport.Close()
            DtrReport = Nothing
        End If

        DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

        'se c'è la necessaria abilitazione al menu mostro il punteggio
        blnPunteggio = DtrReport.HasRows

        If Not DtrReport Is Nothing Then
            DtrReport.Close()
            DtrReport = Nothing
        End If

        Try
            StrSql = "Select '" & CboBando3.SelectedItem.Text & "',B.CodiceEnte,B.Titolo,"
            If Session("VisualizzaStatoProgetti") = True Then
                StrSql = StrSql & "D.StatoAttività,"
            Else
                'controllo il bit VisualizzaStato su Bando per nascondere o meno lo stato del progetto di bandi in valutazione
                StrSql = StrSql & "case isnull(bando.VisualizzaStato,1) when 1 then D.StatoAttività else '' end as StatoAttività, "
            End If
            StrSql = StrSql & "C.CodiceRegione, C.IdClasseAccreditamento, " & _
                     "B.DataInizioPrevista,B.DataInizioAttività,B.DataFinePrevista,B.DataFineAttività, " & _
                     "F.Codifica + ' - ' + F.MacroAmbitoAttività,E.Codifica + ' - ' + E.AmbitoAttività,G.Descrizione, "
            If blnPunteggio = True Then
                If Session("VisualizzaStatoProgetti") = True Then
                    StrSql = StrSql & "IsNull(B.PunteggioFinale,0), "
                Else
                    'controllo il bit VisualizzaStato su Bando per nascondere o meno lo stato del progetto di bandi in valutazione
                    StrSql = StrSql & "case isnull(bando.VisualizzaStato,1) when 1 then IsNull(B.PunteggioFinale,0) else 'Non Abilitato' end, "
                End If
            Else
                StrSql = StrSql & "'Non Abilitato', "
            End If
            '"G.Descrizione,IsNull(B.PunteggioFinale,0), " & _

            'per la regione Trentino Alto Adige la query lavora con le province Bolzano o Trento
            If CboRegione1.SelectedValue = "Bolzano - Bozen" Or CboRegione1.SelectedValue = "Trento" Then

                StrSql = StrSql & "(Select IsNull(SUM( " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0) + " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)),0) " & _
                     "From Attività " & _
                     "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                     "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = EntiSediAttuazioni.IdEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                     "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                     "INNER JOIN Provincie ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                     "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                     "WHERE provincie.provincia  = '" & ClsServer.NoApice(CboRegione1.SelectedItem.Text) & "' And Attività.IdAttività = B.IdAttività) As Richiesti, " & _
                     "(Select IsNull(SUM( " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0) + " & _
                     "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)),0) " & _
                     "From Attività " & _
                     "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                     "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = EntiSediAttuazioni.IdEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                     "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                     "INNER JOIN Provincie ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                     "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                     "WHERE provincie.provincia  = '" & ClsServer.NoApice(CboRegione1.SelectedItem.Text) & "' And Attività.IdAttività = B.IdAttività And Attività.IdStatoAttività IN (1,2)) As Approvati " & _
                     "From BandiAttività AS A " & _
                     "INNER JOIN bando ON bando.IdBando = A.IdBando " & _
                     "INNER JOIN Attività AS B ON A.IdBandoAttività = B.IdBandoAttività " & _
                     "INNER JOIN Enti AS C ON B.IdEntePresentante = C.IdEnte " & _
                     "INNER JOIN StatiAttività AS D ON B.IdStatoAttività = D.IdStatoAttività " & _
                     "INNER JOIN AmbitiAttività AS E ON B.IdAmbitoAttività = E.IdAmbitoAttività " & _
                     "INNER JOIN MacroAmbitiAttività AS F ON E.IdMacroAmbitoAttività = F.IdMacroAmbitoAttività " & _
                     "INNER JOIN TipiProgetto AS G ON B.IdTipoProgetto = G.IdTipoProgetto " & _
                     "WHERE A.IdBando = " & CboBando3.SelectedValue & " AND A.IdStatoBandoAttività IN (2,3,5) and b.idattivitàcapofila is null " & _
                     "ORDER BY B.CodiceEnte,B.Titolo"

            Else

                StrSql = StrSql & "(Select IsNull(SUM( " & _
                   "IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + " & _
                   "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0) + " & _
                   "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)),0) " & _
                   "From Attività " & _
                   "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                   "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = EntiSediAttuazioni.IdEnteSedeAttuazione " & _
                   "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                   "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                   "INNER JOIN Provincie ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                   "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                   "WHERE Regioni.Regione = '" & ClsServer.NoApice(CboRegione1.SelectedItem.Text) & "' And Attività.IdAttività = B.IdAttività) As Richiesti, " & _
                   "(Select IsNull(SUM( " & _
                   "IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + " & _
                   "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0) + " & _
                   "IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)),0) " & _
                   "From Attività " & _
                   "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                   "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = EntiSediAttuazioni.IdEnteSedeAttuazione " & _
                   "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                   "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                   "INNER JOIN Provincie ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                   "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                   "WHERE Regioni.Regione = '" & ClsServer.NoApice(CboRegione1.SelectedItem.Text) & "' And Attività.IdAttività = B.IdAttività And Attività.IdStatoAttività IN (1,2)) As Approvati " & _
                   "From BandiAttività AS A " & _
                   "INNER JOIN bando ON bando.IdBando = A.IdBando " & _
                   "INNER JOIN Attività AS B ON A.IdBandoAttività = B.IdBandoAttività " & _
                   "INNER JOIN Enti AS C ON B.IdEntePresentante = C.IdEnte " & _
                   "INNER JOIN StatiAttività AS D ON B.IdStatoAttività = D.IdStatoAttività " & _
                   "INNER JOIN AmbitiAttività AS E ON B.IdAmbitoAttività = E.IdAmbitoAttività " & _
                   "INNER JOIN MacroAmbitiAttività AS F ON E.IdMacroAmbitoAttività = F.IdMacroAmbitoAttività " & _
                   "INNER JOIN TipiProgetto AS G ON B.IdTipoProgetto = G.IdTipoProgetto " & _
                   "WHERE A.IdBando = " & CboBando3.SelectedValue & " AND A.IdStatoBandoAttività IN (2,3,5) and b.idattivitàcapofila is null " & _
                   "ORDER BY B.CodiceEnte,B.Titolo"

            End If

            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                Dim LngRichiesti As Long = 0
                Dim LngApprovati As Long = 0

                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpProgettiRegioni" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        'controllo se occorre visualizzare il punteggio
                        xLinea = "Bando;Codice;Titolo;Stato;Codice Ente;Classe;Data Inizio Prev.;Data Inizio;Data Fine Prev.;Data Fine;Settore;Area;Tipo;Punteggio;Posti Richiesti in " & CboRegione1.SelectedItem.Text & ";Posti Approvati in " & CboRegione1.SelectedItem.Text & ";"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If
                    If IsDBNull(DtrReport(3)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(3)) & ";"
                    End If
                    If IsDBNull(DtrReport(4)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(4)) & ";"
                    End If
                    If IsDBNull(DtrReport(5)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(5)) & ";"
                    End If
                    If IsDBNull(DtrReport(6)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(6)) & ";"
                    End If
                    If IsDBNull(DtrReport(7)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(7)) & ";"
                    End If
                    If IsDBNull(DtrReport(8)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(8)) & ";"
                    End If
                    If IsDBNull(DtrReport(9)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(9)) & ";"
                    End If
                    If IsDBNull(DtrReport(10)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(10)) & ";"
                    End If
                    If IsDBNull(DtrReport(11)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(11)) & ";"
                    End If
                    If IsDBNull(DtrReport(12)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(12)) & ";"
                    End If
                    If IsDBNull(DtrReport(13)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(13)) & ";"
                    End If
                    If IsDBNull(DtrReport(14)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngRichiesti = LngRichiesti + DtrReport(14)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(14)) & ";"
                    End If
                    If IsDBNull(DtrReport(15)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngApprovati = LngApprovati + DtrReport(15)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(15)) & ";"
                    End If
                    Writer.WriteLine(xLinea)
                End While
                Writer.WriteLine("TOTALE;;;;;;;;;;;;;;" & LngRichiesti.ToString & ";" & LngApprovati.ToString & ";")
                hlProgettiRegione.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            DtrReport.Close()
            DtrReport = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione del report."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaVolontari()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            StrSql = "SELECT '" & CboBando4.SelectedItem.Text & "',Entità.CodiceVolontario,Entità.Cognome,Entità.Nome,Entità.DataNascita,CASE Entità.Sesso WHEN 0 THEN 'M' ELSE 'F' END, " & _
                     "Entità.Indirizzo + ' ' + Entità.NumeroCivico,Entità.CAP CapResidenzaVolontario,Comuni.Denominazione,IsNull(Provincie.DescrAbb,''), " & _
                     "Regioni.Regione,IsNull(Nazioni.Nazione,''),Entità.CodiceFiscale,Attività.CodiceEnte,Attività.Titolo, " & _
                     "IsNull(CASE Len(Day(Entità.DataInizioServizio)) WHEN 1 THEN '0' + CONVERT(varchar(20),Day(Entità.DataInizioServizio)) " & _
                     "ELSE CONVERT(varchar(20),Day(Entità.DataInizioServizio)) END + '/' + (CASE Len(Month(Entità.DataInizioServizio)) WHEN 1 THEN '0' + CONVERT(varchar(20), " & _
                     "Month(Entità.DataInizioServizio)) ELSE CONVERT(varchar(20), Month(Entità.DataInizioServizio)) END + '/' + CONVERT(varchar(20), " & _
                     "Year(Entità.DataInizioServizio))),''), " & _
                     "IsNull(CASE Len(Day(Entità.DataFineServizio)) WHEN 1 THEN '0' + CONVERT(varchar(20),Day(Entità.DataFineServizio)) " & _
                     "ELSE CONVERT(varchar(20),Day(Entità.DataFineServizio)) END + '/' + (CASE Len(Month(Entità.DataFineServizio)) WHEN 1 THEN '0' + CONVERT(varchar(20), " & _
                     "Month(Entità.DataFineServizio)) ELSE CONVERT(varchar(20), Month(Entità.DataFineServizio)) END + '/' + CONVERT(varchar(20), " & _
                     "Year(Entità.DataFineServizio))),''), " & _
                     "Enti.CodiceRegione,Enti.Denominazione, " & _
                     "EntiSedi.Denominazione,Comuni_1.Denominazione,Provincie_1.DescrAbb,Regioni_1.regione,Nazioni_1.nazione, " & _
                     "MacroAmbitiattività.Codifica + ' - ' + Macroambitiattività.Macroambitoattività, " & _
                     "Ambitiattività.Codifica + ' - ' + Ambitiattività.Ambitoattività,TipologiePosto.Descrizione,'" & CboStato.SelectedItem.Text & "' " & _
                     "FROM Entità " & _
                     "INNER JOIN GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità " & _
                     "INNER JOIN TipologiePosto ON GraduatorieEntità.IdTipologiaPosto = TipologiePosto.IdTipologiaPosto " & _
                     "INNER JOIN AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione " & _
                     "INNER JOIN Attività ON AttivitàSediAssegnazione.IDAttività = attività.IDAttività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "INNER JOIN Enti ON Attività.IDEntePresentante = Enti.IDEnte " & _
                     "INNER JOIN EntiSedi ON AttivitàSediAssegnazione.IDEnteSede = entisedi.IDEnteSede " & _
                     "INNER JOIN Comuni ON Entità.IDComuneResidenza = Comuni.IDComune " & _
                     "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia " & _
                     "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                     "INNER JOIN Nazioni ON Regioni.IdNazione = Nazioni.IdNazione " & _
                     "INNER JOIN Comuni Comuni_1 ON entisedi.IDComune = Comuni_1.IDComune " & _
                     "INNER JOIN Provincie Provincie_1 ON Comuni_1.IDProvincia = Provincie_1.IDProvincia " & _
                     "INNER JOIN Regioni Regioni_1 ON Provincie_1.IdRegione = Regioni_1.IdRegione " & _
                     "INNER JOIN Nazioni Nazioni_1 ON Regioni_1.IdNazione = Nazioni_1.IdNazione " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = StatiEntità.IdStatoEntità AND Entità.IdStatoEntità = " & CboStato.SelectedValue & " " & _
                     "INNER JOIN Ambitiattività ON Attività.IdAmbitoAttività = Ambitiattività.IdAmbitoAttività " & _
                     "INNER JOIN MacroAmbitiAttività ON AmbitiAttività.IdMacroAmbitoAttività = MacroAmbitiAttività.IdMacroAmbitoAttività " & _
                     "WHERE BandiAttività.IdBando = " & CboBando4.SelectedValue & " " & _
                     "ORDER BY 1"
            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                Dim LngRichiesti As Long = 0
                Dim LngApprovati As Long = 0

                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpVolontari" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Bando;Codice Volontario;Cognome;Nome;Data di Nascita;Sesso;Indirizzo;Cap;Comune;Provincia;Regione;Nazione;Codice Fiscale;Codice Progetto;Titolo;Data Inizio Servizio;Data Fine Servizio;Codice Ente;Ente;Nome Sede;Comune Sede;Provincia Sede;Regione Sede;Nazione Sede;Settore;Area;Tipo Posto;Stato;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If
                    If IsDBNull(DtrReport(3)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(3)) & ";"
                    End If
                    If IsDBNull(DtrReport(4)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(4)) & ";"
                    End If
                    If IsDBNull(DtrReport(5)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(5)) & ";"
                    End If
                    If IsDBNull(DtrReport(6)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(6)) & ";"
                    End If
                    If IsDBNull(DtrReport(7)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(7)) & ";"
                    End If
                    If IsDBNull(DtrReport(8)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(8)) & ";"
                    End If
                    If IsDBNull(DtrReport(9)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(9)) & ";"
                    End If
                    If IsDBNull(DtrReport(10)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(10)) & ";"
                    End If
                    If IsDBNull(DtrReport(11)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(11)) & ";"
                    End If
                    If IsDBNull(DtrReport(12)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(12)) & ";"
                    End If
                    If IsDBNull(DtrReport(13)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(13)) & ";"
                    End If
                    If IsDBNull(DtrReport(14)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(14)) & ";"
                    End If
                    If IsDBNull(DtrReport(15)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(15)) & ";"
                    End If
                    If IsDBNull(DtrReport(16)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(16)) & ";"
                    End If
                    If IsDBNull(DtrReport(17)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(17)) & ";"
                    End If
                    If IsDBNull(DtrReport(18)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(18)) & ";"
                    End If
                    If IsDBNull(DtrReport(19)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(19)) & ";"
                    End If
                    If IsDBNull(DtrReport(20)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(20)) & ";"
                    End If
                    If IsDBNull(DtrReport(21)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(21)) & ";"
                    End If
                    If IsDBNull(DtrReport(22)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(22)) & ";"
                    End If
                    If IsDBNull(DtrReport(23)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(23)) & ";"
                    End If
                    If IsDBNull(DtrReport(24)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(24)) & ";"
                    End If
                    If IsDBNull(DtrReport(25)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(25)) & ";"
                    End If
                    If IsDBNull(DtrReport(26)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(26)) & ";"
                    End If
                    If IsDBNull(DtrReport(27)) = True Then
                        xLinea = xLinea & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(27)) & ";"
                    End If
                    Writer.WriteLine(xLinea)
                End While
                hlVolontari.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            DtrReport.Close()
            DtrReport = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione del report."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaVolontariRiep()
        Dim StrSql As String
        Dim DtrReport As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            StrSql = "Select A.Regione," & _
                     "(SELECT Count(*) " & _
                     "FROM Entità " & _
                     "INNER JOIN GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità " & _
                     "INNER JOIN AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione " & _
                     "INNER JOIN Attività ON AttivitàSediAssegnazione.IDAttività = attività.IDAttività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "INNER JOIN Enti ON Attività.IDEntePresentante = Enti.IDEnte " & _
                     "INNER JOIN EntiSedi ON AttivitàSediAssegnazione.IDEnteSede = entisedi.IDEnteSede " & _
                     "INNER JOIN Comuni ON entisedi.IDComune = Comuni.IDComune " & _
                     "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia " & _
                     "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                     "INNER JOIN Nazioni ON Regioni.IdNazione = Nazioni.IdNazione " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = StatiEntità.IdStatoEntità AND Entità.IdStatoEntità = 3 " & _
                     "INNER JOIN Ambitiattività ON Attività.IdAmbitoAttività = Ambitiattività.IdAmbitoAttività " & _
                     "INNER JOIN MacroAmbitiAttività ON AmbitiAttività.IdMacroAmbitoAttività = MacroAmbitiAttività.IdMacroAmbitoAttività " & _
                     "WHERE BandiAttività.IdBando = " & CboBando5.SelectedValue & " And Regioni.Regione = A.Regione) As [In Servizio], " & _
                     "(SELECT Count(*) " & _
                     "FROM Entità " & _
                     "INNER JOIN GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità " & _
                     "INNER JOIN AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione " & _
                     "INNER JOIN Attività ON AttivitàSediAssegnazione.IDAttività = attività.IDAttività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "INNER JOIN Enti ON Attività.IDEntePresentante = Enti.IDEnte " & _
                     "INNER JOIN EntiSedi ON AttivitàSediAssegnazione.IDEnteSede = entisedi.IDEnteSede " & _
                     "INNER JOIN Comuni ON entisedi.IDComune = Comuni.IDComune " & _
                     "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia " & _
                     "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                     "INNER JOIN Nazioni ON Regioni.IdNazione = Nazioni.IdNazione " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = StatiEntità.IdStatoEntità AND Entità.IdStatoEntità = 4 " & _
                     "INNER JOIN Ambitiattività ON Attività.IdAmbitoAttività = Ambitiattività.IdAmbitoAttività " & _
                     "INNER JOIN MacroAmbitiAttività ON AmbitiAttività.IdMacroAmbitoAttività = MacroAmbitiAttività.IdMacroAmbitoAttività " & _
                     "WHERE BandiAttività.IdBando = " & CboBando5.SelectedValue & " And Regioni.Regione = A.Regione) As Rinunciatari, " & _
                     "(SELECT Count(*) " & _
                     "FROM Entità " & _
                     "INNER JOIN GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità " & _
                     "INNER JOIN AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione " & _
                     "INNER JOIN Attività ON AttivitàSediAssegnazione.IDAttività = attività.IDAttività " & _
                     "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                     "INNER JOIN Enti ON Attività.IDEntePresentante = Enti.IDEnte " & _
                     "INNER JOIN EntiSedi ON AttivitàSediAssegnazione.IDEnteSede = entisedi.IDEnteSede " & _
                     "INNER JOIN Comuni ON entisedi.IDComune = Comuni.IDComune " & _
                     "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia " & _
                     "INNER JOIN Regioni ON Provincie.IdRegione = Regioni.IdRegione " & _
                     "INNER JOIN Nazioni ON Regioni.IdNazione = Nazioni.IdNazione " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = StatiEntità.IdStatoEntità AND Entità.IdStatoEntità = 5 " & _
                     "INNER JOIN Ambitiattività ON Attività.IdAmbitoAttività = Ambitiattività.IdAmbitoAttività " & _
                     "INNER JOIN MacroAmbitiAttività ON AmbitiAttività.IdMacroAmbitoAttività = MacroAmbitiAttività.IdMacroAmbitoAttività " & _
                     "WHERE BandiAttività.IdBando = " & CboBando5.SelectedValue & " And Regioni.Regione = A.Regione) As [Chiusi in Servizio] " & _
                     "From Regioni As A Where A.IdNazione = 1 And A.Regione <> 'ITALIA' ORDER BY 1"
            DtrReport = ClsServer.CreaDatareader(StrSql, Session("Conn"))

            NomeUnivoco = vbNullString
            If DtrReport.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun dato da esportare."
            Else
                Dim LngServizio As Long = 0
                Dim LngRinunciatari As Long = 0
                Dim LngChiusi As Long = 0

                While DtrReport.Read
                    If NomeUnivoco = vbNullString Then
                        NomeUnivoco = Session("Utente") & "_ExpVolontariRiep" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Regione;In Servizio;Rinunciatari;Chiusi Durante Servizio;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    If IsDBNull(DtrReport(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(DtrReport(0)) & ";"
                    End If
                    If IsDBNull(DtrReport(1)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngServizio = LngServizio + DtrReport(1)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(1)) & ";"
                    End If
                    If IsDBNull(DtrReport(2)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngRinunciatari = LngRinunciatari + DtrReport(2)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(2)) & ";"
                    End If
                    If IsDBNull(DtrReport(3)) = True Then
                        xLinea = xLinea & "0;"
                    Else
                        LngChiusi = LngChiusi + DtrReport(3)
                        xLinea = xLinea & ClsUtility.FormatExport(DtrReport(3)) & ";"
                    End If
                    Writer.WriteLine(xLinea)
                End While
                Writer.WriteLine("TOTALE;" & LngServizio.ToString & ";" & LngRinunciatari.ToString & ";" & LngChiusi.ToString & ";")
                hlVolontariRiep.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            DtrReport.Close()
            DtrReport = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione del report."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not DtrReport Is Nothing Then
                DtrReport.Close()
                DtrReport = Nothing
            End If
        End Try

    End Sub

   
    Protected Sub cmdEsporta_Click(sender As Object, e As EventArgs) Handles cmdEsporta.Click
        NascondiLink()
        lblErr.Text = vbNullString
        If OptEntiClassiRiep.Checked = True Then
            EsportaEntiClassiRiepilogo()
            VisualizzaLink(0)
        ElseIf OptEntiClassi.Checked = True Then
            EsportaEntiClassi()
            VisualizzaLink(1)
        ElseIf OptEntiRegioni.Checked = True Then
            EsportaEntiRegioni()
            VisualizzaLink(2)
        ElseIf OptEntiRegioniRiep.Checked = True Then
            EsportaEntiRegioniRiepilogo()
            VisualizzaLink(3)
        ElseIf OptEntiProgetti.Checked = True Then
            EsportaEntiProgetti()
            VisualizzaLink(4)
        ElseIf OptProgettiBando.Checked = True Then
            EsportaProgettiBando()
            VisualizzaLink(5)
        ElseIf OptProgettiRegioneRiep.Checked = True Then
            EsportaProgettiRegioneRiep()
            VisualizzaLink(6)
        ElseIf OptProgettiRegione.Checked = True Then
            EsportaProgettiRegione()
            VisualizzaLink(7)
        ElseIf OptVolontari.Checked = True Then
            EsportaVolontari()
            VisualizzaLink(8)
        ElseIf OptVolontariRiep.Checked = True Then
            EsportaVolontariRiep()
            VisualizzaLink(9)
        End If
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WFrmMain.aspx")
    End Sub
End Class