Public Class informazionientefiglio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        PopolaMaschera(Request.QueryString("IdEnteFiglio"))
        CaricaEntiSettori(Request.QueryString("IdEnteFiglio"))
    End Sub

    Private Sub PopolaMaschera(ByVal IdEnteFiglio As Integer)
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strsql As String
        Dim MyCommand As SqlClient.SqlDataAdapter
        Dim DsSedi As DataSet = New DataSet

        strsql = " SELECT day(datacontrolloemail)as ggDCEmail,month(datacontrolloemail)as monthDCEmail,year(datacontrolloemail)as yearDCEmail," & _
                "       day(datacontrollohttp)as ggDChttp,month(datacontrollohttp)as monthDChttp,year(datacontrollohttp)as yearDChttp, statienti.statoente," & _
                "       classiaccreditamento.classeaccreditamento,classiaccreditamento.EntiInPartenariato, " & _
                "       enti.Tipologia,enti.CodiceFiscale,isnull(enti.CodiceRegione,'Assente')as codiceregione," & _
                "       enti.Datacontrolloemail,enti.Datacontrollohttp," & _
                "       enti.dataCostituzione,enti.DataRicezioneCartacea,enti.Denominazione,enti.EstremiDeliberaStrutturaGestione," & _
                "       enti.http,enti.Datacontrollohttp,enti.httpvalido," & _
                "       enti.Email,enti.EmailCertificata,enti.Datacontrolloemail,enti.emailvalido,enti.PartitaIva,enti.NoteRichiestaRegistrazione," & _
                "       enti.TelefonoRichiestaRegistrazione," & _
                "       enti.PrefissoTelefonoRichiestaRegistrazione,enti.PrefissoFax,enti.Fax,enti.dataultimaClasseaccreditamento," & _
                "       enti.idclasseaccreditamentorichiesta,enti.idclasseaccreditamento,isnull(enti.CodiceFiscaleArchivio,'') as CodiceFiscaleArchivio, isnull(enti.PartitaIVAArchivio,'') as PartitaIVAArchivio, " & _
                "       entirelazioni.datascadenza,entirelazioni.datastipula,tipirelazioni.tipoRelazione " & _
                " FROM enti " & _
                " INNER JOIN classiaccreditamento on (classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamento)" & _
                " INNER JOIN statienti on (statienti.idstatoente=enti.idstatoente) " & _
                " INNER JOIN entirelazioni on enti.idente=entirelazioni.identefiglio " & _
                " INNER JOIN tipirelazioni on entirelazioni.idTipoRelazione =tipirelazioni.idTipoRelazione " & _
                " WHERE enti.idente=" & IdEnteFiglio
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        If Not IsDBNull(dtrGenerico("StatoEnte")) Then
            lblStato.Text = dtrGenerico("StatoEnte")
        End If

        lblCodiceFiscale.Text = IIf(Not IsDBNull(dtrGenerico("CodiceFiscale")), dtrGenerico("CodiceFiscale"), "")

        lblEnte.Text = IIf(Not IsDBNull(dtrGenerico("Denominazione")), dtrGenerico("Denominazione"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("CodiceRegione")), dtrGenerico("CodiceRegione"), "")
        If Not IsDBNull(dtrGenerico("http")) Then
            lblHTTP.Text = dtrGenerico("http")
        Else
            lblHTTP.Text = " "
        End If
        If Not IsDBNull(dtrGenerico("Email")) Then
            lblEmail.Text = dtrGenerico("Email")
        Else
            lblEmail.Text = " "
        End If

        'lblEmailCertificata.Text = IIf(Not IsDBNull(dtrGenerico("EmailCertificata")), dtrGenerico("EmailCertificata"), "")
        lblTelefono.Text = IIf(Not IsDBNull(dtrGenerico("PrefissoTelefonoRichiestaRegistrazione")), dtrGenerico("PrefissoTelefonoRichiestaRegistrazione"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("TelefonoRichiestaRegistrazione")), dtrGenerico("TelefonoRichiestaRegistrazione"), " ")
        lblFax.Text = IIf(Not IsDBNull(dtrGenerico("PrefissoFax")), dtrGenerico("PrefissoFax"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("Fax")), dtrGenerico("Fax"), " ")
        LblTipoRelazione.Text = IIf(Not IsDBNull(dtrGenerico("tipoRelazione")), dtrGenerico("tipoRelazione"), " ")
        lblTipologia.Text = IIf(Not IsDBNull(dtrGenerico("tipologia")), dtrGenerico("tipologia"), " ")
        'Aggiunto da Alessandra Taballione il 11/11/2005
        'Ricerca e popolamento dei dati relativi alla sede principale dell'Ente
        strsql = "SELECT  entisedi.identesede,entisedi.idcomune,entisedi.indirizzo, entisedi.civico,entisedi.DettaglioRecapito,entisedi.cap,comuni.denominazione as comune," & _
        " provincie.provincia " & _
        " FROM entisedi " & _
        " INNER JOIN statientisedi on statientisedi.idstatoentesede=entisedi.idstatoentesede " & _
        " INNER JOIN entiseditipi ON entisedi.IDEnteSede = entiseditipi.IDEnteSede " & _
        " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " & _
        " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " & _
        " WHERE entisedi.IDEnte = " & IdEnteFiglio & " And entiseditipi.idtiposede = 1 and (statientisedi.attiva=1 or statientisedi.DefaultStato=1)"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            lblIndirizzo.Text = dtrGenerico("Indirizzo")
            lblNumero.Text = dtrGenerico("civico")
            lblCAP.Text = dtrGenerico("cap")
            lblComune.Text = dtrGenerico("Comune")
            LblDettaglioRecapito.Text = IIf(Not IsDBNull(dtrGenerico("DettaglioRecapito")), dtrGenerico("DettaglioRecapito"), "")
            'txtprovincia.Text = dtrgenerico("provincia")
            'txtIdSede.Text = dtrgenerico("identeSede")
            'txtIDComunes.Text = dtrgenerico("idcomune")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Sub CaricaEntiSettori(ByVal IdEnteFiglio As Integer)
        'aggiunto il 19/08/2014 da Simona Cordella
        Dim dtsSettori As DataSet
        'variabile stringa locale per costruire la query per le aree
        Dim strSql As String
        Dim item As DataGridItem
        strSql = "SELECT macroambitiattività.IDMacroAmbitoAttività, macroambitiattività.MacroAmbitoAttività FROM macroambitiattività INNER JOIN EntiSettori ON macroambitiattività.IDMacroAmbitoAttività = EntiSettori.IdMacroAmbitoAttività WHERE EntiSettori.IdEnte = " & IdEnteFiglio
        dtsSettori = ClsServer.DataSetGenerico(strSql, Session("conn"))

        'controllo se ci sono dei record
        If dtsSettori.Tables(0).Rows.Count > 0 Then
            dtgSettori.DataSource = dtsSettori
            dtgSettori.DataBind()
        End If

    End Sub

End Class