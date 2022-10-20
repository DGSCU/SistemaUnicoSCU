Imports System.Data.SqlClient
Imports System.Drawing

Public Class WfrmGestioneBandi
    Inherits System.Web.UI.Page

    Dim CmdGenerico As SqlClient.SqlCommand
    Dim intIdBando As Int32
    Dim MyTransaction As SqlClient.SqlTransaction
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim idBando As String

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
    Private Sub CancellaMessaggiInfo()
        lblErrore.Text = String.Empty
        lblInfo.Text = String.Empty
        lblConferma.Text = String.Empty
    End Sub
#End Region
#Region "PageLoad"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        idBando = Request.QueryString("idbando")
        If IsPostBack = False Then
            'Verifico se sto effettuando un inserimento o una modifica
            If Request.QueryString("Vengoda") = "ElencoProfili" Or Request.QueryString("Vengoda") = "ElencoAttivita" Then
                txtBando.Text = Trim(Request.QueryString("StrBando"))
                'agg. il 31/05/2006 simona cordella
                'gestione del nuovocampo descriizone abbreviata del bando
                txtBandoBreve.Text = Trim(Request.QueryString("StrBandoBreve"))
                txtriferimento.Text = Trim(Request.QueryString("StrRiferimento"))
                TxtAnnoRiferimento.Text = Trim(Request.QueryString("StrAnnoRif"))
                txtImportoStanziato.Text = Trim(Request.QueryString("StrImporto"))
                txtInizio.Text = Trim(Request.QueryString("StrDataInizio"))
                txtfine.Text = Trim(Request.QueryString("StrDataFine"))
                TxtDataInizioVolontari.Text = Trim(Request.QueryString("StrDataInizioVol"))
                TxtDataFineVolontari.Text = Trim(Request.QueryString("StrDataFineVol"))
                If Request.QueryString("strCheck") = True Then
                    ChkAssAutomatica.Checked = True
                End If
                TxtDataScadGrad.Text = Trim(Request.QueryString("strDataScadGrad"))

                TxtNMaxVolontariProgettoItalia.Text = Trim(Request.QueryString("strNMaxVolontariProgettoItalia"))
                TxtNMinVolontariProgettoItalia.Text = Trim(Request.QueryString("strNMinVolontariProgettoItalia"))
                TxtNMaxVolontariProgettoEstero.Text = Trim(Request.QueryString("strNMaxVolontariProgettoEstero"))
                TxtNMinVolontariProgettoEstero.Text = Trim(Request.QueryString("strNMinVolontariProgettoEstero"))
                TxtNMinVolontariSedeItalia.Text = Trim(Request.QueryString("strNMinVolontariSedeItalia"))
                TxtNMinVolontariSedeEstero.Text = Trim(Request.QueryString("strNMinVolontariSedeEstero"))
                TxtNMaxVolontariProgettoCoprogettato.Text = Trim(Request.QueryString("strNMaxVolontariProgettoCoprogettato"))


                TxtGruppo.Text = Request.QueryString("StrGruppo")
                lblIdBando.Value = Request.QueryString("StrIdBando")
                lblRicerca.Value = Session("Prov_Ricerca_Bando")
                lblpage.Value = Request.QueryString("Page")
                lblStato.Text = Trim(Request.QueryString("Stato"))
                Hdd_chkFlag.Value = Trim(Request.QueryString("Hdd_Check"))
                Hdd_dtiniziovol.Value = Trim(Request.QueryString("Hdd_DataIVol"))
                Hdd_dtfinevol.Value = Trim(Request.QueryString("Hdd_DataFVol"))
                Hdd_Rif.Value = Trim(Request.QueryString("Hdd_rif"))
            End If
            If Not (IsNothing(idBando) Or idBando = String.Empty) Then
                lblIdBando.Value = idBando
                lblAzione.Text = "Modifica"
                PopolaMaschera()
            End If
            If Not IsNothing(Request.QueryString("tipoazione")) Then
                lblAzione.Text = Request.QueryString("tipoazione")
            End If
            If Not IsNothing(Context.Items("Ricerca")) Then
                lblRicerca.Value = Context.Items("Ricerca")
                Session("Prov_Ricerca_Bando") = lblRicerca.Value
                lblpage.Value = Context.Items("page")
            End If
            'Richiamo la Personalizzazione della Maschera a seconda del tipo
            'di azione che andrò a svolgere
            Hdd_chkFlag.Value = ""

            CaricaDataGridTipoProgetto()
            CaricaGrigliaRegioneCompetenze()
            PersonalizzaMaschera()
            If Request.QueryString("Vengoda") = "ElencoProfili" Or Request.QueryString("Vengoda") = "ElencoAttivita" Then
                'carico i valori ricordati
                CaricaValori()
            End If
        End If
    End Sub


#End Region
#Region "Operazioni DB"
    Private Sub InserimentoBando()
        Dim null As String = "null"
        'Generato da Alesssandra Taballione il 30.06.2004
        'Sub che effettua l'inserimento nel DB del Bando
        'Mod. il 27/03/2006 da Simona Cordella
        'gestione nuovi campi per l'inserimento del bando (data inizio e fine volontari,ass. atuomatica, annorif, gruppo,annobreve)
        'Mod. il 28/08/2006 da simona cordella
        ' egstione del campo EnteAbilitato (si/no) defoult no
        'Mod. il 11/04/2007 da simona cordella
        'gestion del campo data scadenza graduatoria
        strsql = "Insert into bando (Bando,dataInizioValidità,datafineValidità,idstatobando," & _
        " UsernameInseritore,datacreazione,riferimento,importostanziato," & _
        " DataInizioVolontari,DataFineVolontari,Anno,AssociazioneAutomatica,Gruppo,AnnoBreve,BandoBreve,EnteAbilitato,DataScadenzaGraduatorie," & _
        " NMaxVolontariProgettoItalia,NMinVolontariProgettoItalia,NMinVolontariSedeItalia,NMinVolontariSedeEstero," & _
        " NMaxVolontariProgettoEstero,NMinVolontariProgettoEstero,NMaxVolontariProgettoCoprogettato  ) " & _
        " Select '" & Replace(txtBando.Text, "'", "''") & "','" & txtInizio.Text & "','" & txtfine.Text & "'," & _
        " idstatobando,'" & Replace(Session("Utente"), "'", "''") & "',getdate(),"
        If txtriferimento.Text <> "" Then
            strsql = strsql & "'" & Replace(txtriferimento.Text, "'", "''") & "', "
        Else
            strsql = strsql & "" & null & ","
        End If
        If txtImportoStanziato.Text <> "" Then
            strsql = strsql & "" & Replace(txtImportoStanziato.Text, ",", ".") & ","
            'from statibando where defaultStato=1"
        Else
            strsql = strsql & "" & null & ", "
            'from statibando where defaultStato=1"
        End If
        ''        strsql = strsql & "'" & TxtDataInizioVolontari.Text & "','" & TxtDataFineVolontari.Text & "',"

        If TxtDataInizioVolontari.Text <> "" Then
            strsql = strsql & "'" & TxtDataInizioVolontari.Text & "',"
        Else
            strsql = strsql & "" & null & ", "
        End If
        If TxtDataFineVolontari.Text <> "" Then
            strsql = strsql & "'" & TxtDataFineVolontari.Text & "',"
        Else
            strsql = strsql & "" & null & ", "
        End If
        strsql = strsql & "'" & TxtAnnoRiferimento.Text & "',"
        If ChkAssAutomatica.Checked = True Then
            strsql = strsql & "" & 1 & ","
        Else
            strsql = strsql & "" & 0 & ","
        End If
        strsql = strsql & "'" & TxtGruppo.Text & "',"
        strsql = strsql & "'" & Mid(TxtAnnoRiferimento.Text, 3, 2) & "',"
        'Aggiunto il 31/05/2006 da Simona Cordella
        'Gestione del campo Descrizione abbreviata del bando
        strsql = strsql & "'" & Replace(txtBandoBreve.Text, "'", "''") & "',"
        'Aggiunto il 28/08/2006
        'Gestione campo AbiltaEnte
        If ChkAbilita.Checked = True Then
            strsql = strsql & "" & 1 & ","
        Else
            strsql = strsql & "" & 0 & ","
        End If
        If TxtDataScadGrad.Text <> "" Then
            strsql = strsql & "'" & TxtDataScadGrad.Text & "',"
        Else
            strsql = strsql & "" & null & ", "
        End If
        strsql = strsql & TxtNMaxVolontariProgettoItalia.Text & ","
        strsql = strsql & TxtNMinVolontariProgettoItalia.Text & ","
        strsql = strsql & TxtNMinVolontariSedeItalia.Text & ","
        strsql = strsql & TxtNMinVolontariSedeEstero.Text & ","
        strsql = strsql & TxtNMaxVolontariProgettoEstero.Text & ","
        strsql = strsql & TxtNMinVolontariProgettoEstero.Text & ","
        strsql = strsql & TxtNMaxVolontariProgettoCoprogettato.Text

        strsql = strsql & " from statibando where defaultStato=1"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        'BloccaMaschera()
        MessaggiConvalida("Il bando è stato Inserito con successo!")
    End Sub

    Private Sub InserimentoTipiProgetto()
        Dim item As DataGridItem
        CmdGenerico = New System.Data.SqlClient.SqlCommand
        CmdGenerico.Connection = Session("conn")

        strsql = "Select idbando from bando where bando ='" & Replace(txtBando.Text, "'", "''") & "'"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        intIdBando = dtrgenerico("idBando")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        For Each item In dgTipiProgetto.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            If check.Checked = True Then
                strsql = "Insert into AssociaBandoTipiProgetto (IdBando,IdTipoProgetto) " & _
                        " Values ( " & intIdBando & "," & dgTipiProgetto.Items(item.ItemIndex).Cells(1).Text & ")"
                CmdGenerico.CommandText = strsql
                CmdGenerico.ExecuteNonQuery()
            End If
        Next

    End Sub

    Private Sub InserimentoRegioneCompetenza()
        'Generato il 11/07/2006 da SImona Cordella
        'Inserimento associazione regione di competenza
        Dim Item As DataGridItem
        CmdGenerico = New System.Data.SqlClient.SqlCommand
        CmdGenerico.Connection = Session("conn")
        strsql = "Select idbando from bando where bando ='" & Replace(txtBando.Text, "'", "''") & "'"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        intIdBando = dtrgenerico("idBando")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        For Each Item In dgRegioneCompetenza.Items
            Dim check As CheckBox = DirectCast(Item.FindControl("check2"), CheckBox)
            If check.Checked = True Then
                strsql = "Insert into AssociaBandoRegioniCompetenze (IdBando,IdRegioneCompetenza) " & _
                        " Values ( " & intIdBando & "," & dgRegioneCompetenza.Items(Item.ItemIndex).Cells(1).Text & ")"
                CmdGenerico.CommandText = strsql
                CmdGenerico.ExecuteNonQuery()
            End If
        Next

    End Sub

    Private Sub CancellaTipiProgetto()
        'Generato da Simona Cordella il 31/03/2006
        'Sub che effettua la cencellazione dei tipi progetto prima dell'inserimento
        ' viene usata solo in fase di modifica del bando
        CmdGenerico = New System.Data.SqlClient.SqlCommand
        CmdGenerico.Connection = Session("conn")

        strsql = "Delete from AssociaBandoTipiProgetto where idbando=" & lblIdBando.Value & ""
        CmdGenerico.CommandText = strsql
        CmdGenerico.ExecuteNonQuery()
    End Sub
    Private Sub CancellaRegioneCompetenza()
        'Generato da Simona Cordella il 11/07/2006
        'Sub che effettua la cencellazione delle associazione di reginoe competenza al bando
        ' viene usata solo in fase di modifica del bando
        CmdGenerico = New System.Data.SqlClient.SqlCommand
        CmdGenerico.Connection = Session("conn")

        strsql = "Delete from AssociaBandoRegioniCompetenze where idbando=" & lblIdBando.Value & ""
        CmdGenerico.CommandText = strsql
        CmdGenerico.ExecuteNonQuery()
    End Sub
    Private Sub ModificaBando()
        'Generato da Alesssandra Taballione il 30.06.2004
        'Sub che effettua la modifica nel DB del Bando
        Dim null As String = "null"
        strsql = "update bando set Bando='" & Replace(txtBando.Text, "'", "''") & "'," & _
                " dataInizioValidità='" & txtInizio.Text & "',datafineValidità='" & txtfine.Text & "'," & _
                " UsernameInseritore='" & Replace(Session("Utente"), "'", "''") & "',datacreazione=getdate(), "
        If Trim(txtriferimento.Text) <> "" Then
            strsql = strsql & "Riferimento='" & Replace(txtriferimento.Text, "'", "''") & "',"
        Else
            strsql = strsql & "Riferimento=" & null & ","
        End If
        If Trim(txtImportoStanziato.Text) <> "" Then
            strsql = strsql & " importostanziato=" & Replace(txtImportoStanziato.Text, ",", ".") & ","
        Else
            strsql = strsql & " importostanziato=" & null & ","
        End If

        If TxtDataInizioVolontari.Text <> "" Then
            strsql = strsql & " datainiziovolontari='" & TxtDataInizioVolontari.Text & "',"
        Else
            strsql = strsql & " datainiziovolontari=" & null & ", "
        End If
        If TxtDataFineVolontari.Text <> "" Then
            strsql = strsql & " datafinevolontari='" & TxtDataFineVolontari.Text & "',"
        Else
            strsql = strsql & " datafinevolontari=" & null & ", "
        End If
        ''strsql = strsql & " datainiziovolontari='" & TxtDataInizioVolontari.Text & "',datafinevolontari='" & TxtDataFineVolontari.Text & "',"
        strsql = strsql & "anno='" & TxtAnnoRiferimento.Text & "',"
        If ChkAssAutomatica.Checked = True Then
            strsql = strsql & "AssociazioneAutomatica = " & 1 & ","
        Else
            strsql = strsql & "AssociazioneAutomatica = " & 0 & ","
        End If
        strsql = strsql & " gruppo ='" & TxtGruppo.Text & "',"
        strsql = strsql & " Annobreve ='" & Mid(TxtAnnoRiferimento.Text, 3, 2) & "',"
        strsql = strsql & " BandoBreve ='" & Replace(txtBandoBreve.Text, "'", "''") & "',"
        'Aggiuinto il 28/08/2006 da Simona Cordella
        'gestione del campo Ente Abilitato
        If ChkAbilita.Checked = True Then
            strsql = strsql & "EnteAbilitato = " & 1 & ","
        Else
            strsql = strsql & "EnteAbilitato = " & 0 & ","
        End If
        'Aggiunto il 11/04/2007 da Simona Cordella
        'gestione del campo data scadenza graduatoria
        If TxtDataScadGrad.Text <> "" Then
            strsql = strsql & " DataScadenzaGraduatorie ='" & TxtDataScadGrad.Text & "',"
        Else
            strsql = strsql & " DataScadenzaGraduatorie =" & null & ", "
        End If

        'Aggiunti 6 campi sul numero dei volontari Italia e Estero -Ilaria Lombardi 19-10-2009


        strsql = strsql & " NMaxVolontariProgettoItalia =" & TxtNMaxVolontariProgettoItalia.Text & ","
        strsql = strsql & " NMinVolontariProgettoItalia =" & TxtNMinVolontariProgettoItalia.Text & ","
        strsql = strsql & " NMinVolontariSedeItalia =" & TxtNMinVolontariSedeItalia.Text & ","
        strsql = strsql & " NMinVolontariSedeEstero =" & TxtNMinVolontariSedeEstero.Text & ","
        strsql = strsql & " NMaxVolontariProgettoEstero =" & TxtNMaxVolontariProgettoEstero.Text & ","
        strsql = strsql & " NMaxVolontariProgettoCoprogettato =" & TxtNMaxVolontariProgettoCoprogettato.Text & ","
        strsql = strsql & " NMinVolontariProgettoEstero =" & TxtNMinVolontariProgettoEstero.Text


        strsql = strsql & " Where idbando=" & lblIdBando.Value & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        ' BloccaMaschera()
        MessaggiConvalida("Il Bando è stato Modificato con successo!")
    End Sub
    Private Sub CancellaBando()
        'Generato da Alesssandra Taballione il 30.06.2004
        'Sub che effettua l'eliminazione logica nel DB del Bando
        strsql = "update bando set idstatobando=(select idstatobando from statibando where annullato=1) " & _
        " Where idbando=" & lblIdBando.Value & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        BloccaMaschera()
        MessaggiConvalida("Il Bando è stato Annullato!")
        PopolaMaschera()
    End Sub
    Private Sub PubblicaBando()
        'Generato da Alesssandra Taballione il 30.06.2004
        'Sub che effettua l'eliminazione logica nel DB del Bando
        strsql = "update bando set idstatobando=(select idstatobando from statibando where invalutazione=1) " & _
        " Where idbando=" & lblIdBando.Value & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))

        MessaggiConvalida("Il Bando è stato Pubblicato!")
        PopolaMaschera()
        BloccaMaschera()
    End Sub
#End Region
#Region "Funzionalità"

    Sub CaricaValori()
        Dim intX As Integer = 0
        Dim item As DataGridItem

        For Each item In dgTipiProgetto.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            If Session("vIdRicordaCheck_Sim")(intX) = item.Cells(1).Text Then
                check.Checked = Session("vTipoRicordaCheck_Sim")(intX)
                intX = intX + 1
            End If
        Next
    End Sub

    Private Sub PersonalizzaMaschera()
        'Generato da Alessandra Taballione il 30.06.2004
        'Sub che personalizza la maschera di Gestione dei Bandi.
        If lblAzione.Text = "Inserimento" Then
            lblStato.Visible = True
            lblStatoBando.Visible = False
            cmdCancella.Visible = False
            cmdPubblica.Visible = False
        Else
            'verifico lo stato del bando e lo rendo modificabile solo 
            'se lo stato è "Registrato"
            strsql = "Select * from statibando where statobando='" & Replace(lblStato.Text, "'", "''") & "' "
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            'non è possibile Modificarlo
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                If dtrgenerico("defaultstato") = True Then
                    cmdPubblica.Visible = True
                Else
                    BloccaMaschera()
                    bloccaCheck()
                    bloccaCheck_RegioneCompetenza()
                End If
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            lblStato.Visible = True
            lblStatoBando.Visible = True
        End If
    End Sub
    Private Sub BloccaMaschera()
        'Generato da Alessandra Taballione il 30.06.2004
        'Sub che personalizza la maschera di Gestione dei Bandi
        'dopo aver effettuato operazioni nel db
        'Mod. il 27/03/2006 da Simona Cordella
        ' Gestione dei nuovi controlli inseriti in maschera
        txtBando.ReadOnly = True
        txtBando.BackColor = Color.Gainsboro
        'Aggiunto il 31/05/2006 da Simona Cordella
        'Gestione del nuov ocampo descrizione abbrviata bando
        txtBandoBreve.ReadOnly = True
        txtfine.Enabled = False
        txtInizio.Enabled = False
        txtriferimento.ReadOnly = True
        txtImportoStanziato.BackColor = Color.Gainsboro
        txtImportoStanziato.ReadOnly = True
        TxtAnnoRiferimento.ReadOnly = True
        TxtAnnoRiferimento.BackColor = Color.Gainsboro
        'Aggiunti i 6 campi riguardanti i volontari - Ilaria Lombardi 19-10-2009
        TxtNMaxVolontariProgettoEstero.ReadOnly = True
        TxtNMaxVolontariProgettoEstero.BackColor = Color.Gainsboro
        TxtNMinVolontariProgettoEstero.ReadOnly = True
        TxtNMinVolontariProgettoEstero.BackColor = Color.Gainsboro
        TxtNMaxVolontariProgettoItalia.ReadOnly = True
        TxtNMaxVolontariProgettoItalia.BackColor = Color.Gainsboro
        TxtNMinVolontariProgettoItalia.ReadOnly = True
        TxtNMinVolontariProgettoItalia.BackColor = Color.Gainsboro
        TxtNMinVolontariSedeEstero.ReadOnly = True
        TxtNMinVolontariSedeEstero.BackColor = Color.Gainsboro
        TxtNMinVolontariSedeItalia.ReadOnly = True
        TxtNMinVolontariSedeItalia.BackColor = Color.Gainsboro
        TxtNMaxVolontariProgettoCoprogettato.ReadOnly = True
        TxtNMaxVolontariProgettoCoprogettato.BackColor = Color.Gainsboro
        If lblStato.Text <> "Aperto" Then
            TxtDataFineVolontari.Enabled = False
            TxtDataInizioVolontari.Enabled = False
            TxtDataScadGrad.Enabled = False
            ChkAssAutomatica.Enabled = False

            ChkAbilita.Enabled = False
            TxtGruppo.BackColor = Color.Gainsboro
            hf_DisabilitaCalendar.Value = "1"
            cmdSalva.Visible = False
        Else
            cmdImpVirtuali.Visible = True
        End If
        cmdCancella.Visible = False
        cmdPubblica.Visible = False
        dgTipiProgetto.Enabled = False

        dgRegioneCompetenza.Enabled = False
        chkSelDesel.Visible = False

    End Sub
    Private Sub MessaggiAlert(ByVal strMessaggio)

        lblErrore.Text = lblErrore.Text & strMessaggio
        lblErrore.Text = lblErrore.Text & " <br/>"
    End Sub
    Private Sub MessaggiConvalida(ByVal strMessaggio)
        lblInfo.Text = strMessaggio
    End Sub

    Private Sub MessaggiConferma(ByVal strMessaggio)
        lblConferma.Text = strMessaggio
    End Sub
    Private Sub PopolaMaschera()
        'Generato da Alessandra Taballione il 01/07/2004
        'Private sub che popola la maschera di Gestione Bandi 
        'in Modifica
        strsql = "select bando,bandobreve,statobando,Riferimento,Importostanziato, case len(day(datainiziovalidità)) when 1 then '0'+ convert(varchar(20),day(datainiziovalidità))" & _
        " else convert(varchar(20),day(datainiziovalidità))  end + '/'+ " & _
        " case len(month(datainiziovalidità)) when 1 then '0'+ convert(varchar(20),month(datainiziovalidità)) " & _
        " else convert(varchar(20),month(datainiziovalidità)) end + '/'+ convert(varchar(20),year(datainiziovalidità))" & _
        " as datainiziovalidità, case len(day(datafinevalidità)) when 1 then '0'+ convert(varchar(20),day(datafinevalidità))" & _
        " else convert(varchar(20),day(datafinevalidità))  end + '/'+ " & _
        " case len(month(datafinevalidità)) when 1 then '0'+ convert(varchar(20),month(datafinevalidità)) " & _
        " else convert(varchar(20),month(datafinevalidità)) end + '/'+ convert(varchar(20),year(datafinevalidità))" & _
        " as datafinevalidità ,datainiziovolontari,datafinevolontari,anno,associazioneautomatica,gruppo,EnteAbilitato, " & _
        " DataScadenzaGraduatorie,NMaxVolontariProgettoItalia,NMinVolontariProgettoItalia,NMinVolontariSedeItalia, " & _
        " NMinVolontariSedeEstero,NMaxVolontariProgettoEstero,isnull(NMaxVolontariProgettoCoprogettato,0) as NMaxVolontariProgettoCoprogettato,NMinVolontariProgettoEstero " & _
        " from bando " & _
        " inner join statibando on (statibando.idstatobando=bando.idstatobando) " & _
        " where idBando = " & lblIdBando.Value & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        txtBando.Text = dtrgenerico("Bando")
        txtBandoBreve.Text = "" & dtrgenerico("BandoBreve")
        txtInizio.Text = dtrgenerico("dataInizioValidità")
        txtfine.Text = dtrgenerico("datafineValidità")
        lblStato.Text = dtrgenerico("statobando")
        'txtiniziod.Value = Year(txtInizio.Text)
        'txtiniziod.Value = txtiniziod.Value & IIf(Month(txtInizio.Text) < 10, "0" & Month(txtInizio.Text), Month(txtInizio.Text))
        'txtiniziod.Value = txtiniziod.Value & IIf(Day(txtInizio.Text) < 10, "0" & Day(txtInizio.Text), Day(txtInizio.Text))
        'txtfined.Value = Year(txtfine.Text)
        'txtfined.Value = txtfined.Value & IIf(Month(txtfine.Text) < 10, "0" & Month(txtfine.Text), Month(txtfine.Text))
        'txtfined.Value = txtfined.Value & IIf(Day(txtfine.Text) < 10, "0" & Day(txtfine.Text), Day(txtfine.Text))
        txtriferimento.Text = IIf(Not IsDBNull(dtrgenerico("Riferimento")), dtrgenerico("Riferimento"), "")
        txtImportoStanziato.Text = IIf(Not IsDBNull(dtrgenerico("importostanziato")), dtrgenerico("importostanziato"), "0")
        txtImportoStanziato.Text = Replace(txtImportoStanziato.Text, ",", ".")
        If InStr(txtImportoStanziato.Text, ".") = 0 Then
            txtImportoStanziato.Text = txtImportoStanziato.Text & ".00"
        End If
        TxtAnnoRiferimento.Text = dtrgenerico("Anno")
        Hdd_Rif.Value = (Mid(TxtAnnoRiferimento.Text, 6, 1))
        TxtDataInizioVolontari.Text = IIf(Not IsDBNull(dtrgenerico("DataInizioVolontari")), dtrgenerico("DataInizioVolontari"), "")
        TxtDataFineVolontari.Text = IIf(Not IsDBNull(dtrgenerico("DataFineVolontari")), dtrgenerico("DataFineVolontari"), "")
        If TxtDataInizioVolontari.Text <> "" Then
            Hdd_dtiniziovol.Value = Year(TxtDataInizioVolontari.Text)
            Hdd_dtiniziovol.Value = Hdd_dtiniziovol.Value & IIf(Month(TxtDataInizioVolontari.Text) < 10, "0" & Month(TxtDataInizioVolontari.Text), Month(TxtDataInizioVolontari.Text))
            Hdd_dtiniziovol.Value = Hdd_dtiniziovol.Value & IIf(Day(TxtDataInizioVolontari.Text) < 10, "0" & Day(TxtDataInizioVolontari.Text), Day(TxtDataInizioVolontari.Text))
        End If
        If TxtDataFineVolontari.Text <> "" Then
            Hdd_dtfinevol.Value = Year(TxtDataFineVolontari.Text)
            Hdd_dtfinevol.Value = Hdd_dtfinevol.Value & IIf(Month(TxtDataFineVolontari.Text) < 10, "0" & Month(TxtDataFineVolontari.Text), Month(TxtDataFineVolontari.Text))
            Hdd_dtfinevol.Value = Hdd_dtfinevol.Value & IIf(Day(TxtDataFineVolontari.Text) < 10, "0" & Day(TxtDataFineVolontari.Text), Day(TxtDataFineVolontari.Text))
        End If
        If dtrgenerico("AssociazioneAutomatica") = True Then
            ChkAssAutomatica.Checked = True
        End If
        Hdd_chkFlag.Value = ChkAssAutomatica.Checked
        'aggiunto il 28/08/2006 da simona cordella
        If dtrgenerico("EnteAbilitato") = True Then
            ChkAbilita.Checked = True
        End If
        TxtDataScadGrad.Text = IIf(Not IsDBNull(dtrgenerico("DataScadenzaGraduatorie")), dtrgenerico("DataScadenzaGraduatorie"), "")

        TxtGruppo.Text = dtrgenerico("gruppo")

        TxtNMaxVolontariProgettoItalia.Text = dtrgenerico("NMaxVolontariProgettoItalia")
        TxtNMinVolontariProgettoItalia.Text = dtrgenerico("NMinVolontariProgettoItalia")
        TxtNMinVolontariSedeItalia.Text = dtrgenerico("NMinVolontariSedeItalia")
        TxtNMinVolontariSedeEstero.Text = dtrgenerico("NMinVolontariSedeEstero")
        TxtNMaxVolontariProgettoEstero.Text = dtrgenerico("NMaxVolontariProgettoEstero")
        TxtNMinVolontariProgettoEstero.Text = dtrgenerico("NMinVolontariProgettoEstero")
        TxtNMaxVolontariProgettoCoprogettato.Text = dtrgenerico("NMaxVolontariProgettoCoprogettato")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Private Sub CaricaDataGridTipoProgetto()
        'Generato da Simona Cordella il 29/03/2006
        'Private sub che popola la data grid con i tipi progetto
        Dim dtsgenerico As DataSet
        Dim item As DataGridItem

        strsql = "SELECT IDTIPOPROGETTO,DESCRIZIONE "
        '            "'<img src=""images/xp1.gif"" onClick=""javascript: SelezionaTipoProgetto(''' + CONVERT(varchar, IDTIPOPROGETTO) + ''',''' + descrizione + ''') "" Style=""cursor:hand;"" border=""0"">' as Immagine 
        strsql = strsql & " FROM TIPIPROGETTO"
        'FiltroVisibilita 03/12/20104 da s.c.
        strsql = strsql & " where MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "

        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgTipiProgetto.DataSource = dtsgenerico
        dgTipiProgetto.DataBind()
        'Session("RisultatoRicercaTipiProgetto_Sim") = dtsgenerico
        If lblAzione.Text <> "Inserimento" Then
            strsql = " SELECT T.IDTIPOPROGETTO, T.DESCRIZIONE "
            strsql = strsql & " FROM TIPIPROGETTO T"
            strsql = strsql & " INNER JOIN ASSOCIABANDOTIPIPROGETTO A ON T.IDTIPOPROGETTO= A.IDTIPOPROGETTO "
            If Request.QueryString("Vengoda") = "ElencoProfili" Then
                strsql = strsql & " WHERE A.IDBANDO = " & Request.QueryString("strIdBando") & ""
            Else
                strsql = strsql & " WHERE A.IDBANDO = " & lblIdBando.Value & ""
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'FiltroVisibilita 03/12/20104 da s.c.
            strsql = strsql & " and MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "


            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            While dtrgenerico.Read()
                For Each item In dgTipiProgetto.Items
                    Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                    If dtrgenerico("IDTIPOPROGETTO") = dgTipiProgetto.Items(item.ItemIndex).Cells(1).Text Then
                        check.Checked = True
                    End If
                Next
            End While
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
    End Sub

    Private Sub CaricaGrigliaRegioneCompetenze()
        'Generato da Simona Cordella il 10/07/2006
        'Carico griglia per assegnazione regione comptetenza al bando
        Dim dtsgenerico As DataSet
        Dim item As DataGridItem

        strsql = "SELECT IDREGIONECOMPETENZA,CODICEREGIONECOMPETENZA,DESCRIZIONE FROM REGIONICOMPETENZE" & _
                " order by left(CODICEREGIONECOMPETENZA,1),descrizione"
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgRegioneCompetenza.DataSource = dtsgenerico
        dgRegioneCompetenza.DataBind()

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        If lblAzione.Text <> "Inserimento" Then
            strsql = " SELECT RegioniCompetenze.IdRegioneCompetenza, AssociaBandoRegioniCompetenze.IdRegioneCompetenza "
            strsql = strsql & " FROM RegioniCompetenze"
            strsql = strsql & " inner join  AssociaBandoRegioniCompetenze ON RegioniCompetenze.IdRegioneCompetenza = AssociaBandoRegioniCompetenze.IdRegioneCompetenza "
            If Request.QueryString("Vengoda") = "ElencoProfili" Then
                strsql = strsql & " WHERE AssociaBandoRegioniCompetenze.IDBANDO = " & Request.QueryString("strIdBando") & ""
            Else
                strsql = strsql & " WHERE AssociaBandoRegioniCompetenze.IDBANDO = " & lblIdBando.Value & ""
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            While dtrgenerico.Read()
                For Each item In dgRegioneCompetenza.Items
                    Dim check As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                    If dtrgenerico("IdRegioneCompetenza") = dgRegioneCompetenza.Items(item.ItemIndex).Cells(1).Text Then
                        check.Checked = True
                    End If
                Next
            End While
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
    End Sub
    Private Sub bloccaCheck()
        Dim item As DataGridItem
        For Each item In dgTipiProgetto.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            check.Enabled = False
        Next
    End Sub
    Private Sub bloccaCheck_RegioneCompetenza()
        Dim item As DataGridItem
        For Each item In dgRegioneCompetenza.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
            check.Enabled = False
        Next
    End Sub
    Private Sub RicordaCheck()
        ''Generato da Simona Cordella il 10/04/2006
        'ricorda i valori selezionati
        Dim item As DataGridItem

        ReDim Session("vIdRicordaCheck_Sim")(0)
        ReDim Session("vTipoRicordaCheck_Sim")(0)

        Session("vIdRicordaCheck_Sim")(0) = ""
        Session("vTipoRicordaCheck_Sim")(0) = ""


        For Each item In dgTipiProgetto.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            If Session("vIdRicordaCheck_Sim")(0) = "" Then
                Session("vIdRicordaCheck_Sim")(0) = item.Cells(1).Text
                Session("vTipoRicordaCheck_Sim")(0) = check.Checked
            Else
                ReDim Preserve Session("vIdRicordaCheck_Sim")(UBound(Session("vIdRicordaCheck_Sim")) + 1)
                ReDim Preserve Session("vTipoRicordaCheck_Sim")(UBound(Session("vTipoRicordaCheck_Sim")) + 1)

                Session("vIdRicordaCheck_Sim")(UBound(Session("vIdRicordaCheck_Sim"))) = item.Cells(1).Text
                Session("vTipoRicordaCheck_Sim")(UBound(Session("vTipoRicordaCheck_Sim"))) = check.Checked
            End If
        Next

    End Sub

    Private Function ControllaCheck_TipoProgetto() As Boolean
        'Generato da Simona CordelLa il 30/03/2006
        Dim item As DataGridItem
        ControllaCheck_TipoProgetto = False
        For Each item In dgTipiProgetto.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            If check.Checked = True Then
                ControllaCheck_TipoProgetto = True
                Exit For
            End If
        Next
        If ControllaCheck_TipoProgetto = False Then
            MessaggiAlert("Attenzione, è necessario selezionare almeno un tipo progetto per il Bando.")
        End If
    End Function

    Private Function ControllaCheck_RegioneCompetenza() As Boolean
        'Generato da Simona CordelLa il 30/03/2006
        Dim item As DataGridItem
        ControllaCheck_RegioneCompetenza = False
        For Each item In dgRegioneCompetenza.Items
            Dim check_Reg As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
            If check_Reg.Checked = True Then
                ControllaCheck_RegioneCompetenza = True
                Exit For
            End If
        Next
        If ControllaCheck_RegioneCompetenza = False Then
            MessaggiAlert("Attenzione, è necessario selezionare almeno una Regione di competenza.")
        End If
    End Function

    Private Function Controlli_Formali() As Boolean
        ' Generato da Simona Cordella il 03/04/2006
        Controlli_Formali = True

        'Verifico la validità della dataInizio e datafine Bando
        If CDate(txtInizio.Text) > CDate(txtfine.Text) Then
            MessaggiAlert("La data di inizio non può essere maggiore della data Fine Bando. Verificare l'esatta immissione dei dati. ")
            Controlli_Formali = False
        End If
        If CDate(txtInizio.Text) = CDate(txtfine.Text) Then
            MessaggiAlert("La data di Inizio non può essere uguale della data Fine Bando. Verificare l'esatta immissione dei dati. ")
            Controlli_Formali = False
        End If

        If TxtDataInizioVolontari.Text <> "" Then
            'Verifico la validità della dataInizio e datafine Volontari
            If CDate(txtfine.Text) > CDate(TxtDataInizioVolontari.Text) Then
                MessaggiAlert("La data di inizio Volontari deve essere maggiore della data fine del bando. Verificare l'esatta immissione dei dati. ")
                Controlli_Formali = False
            End If
            If CDate(txtfine.Text) = CDate(TxtDataInizioVolontari.Text) Then
                MessaggiAlert("La data di inizio Volontari deve essere maggiore della data fine del bando. Verificare l'esatta immissione dei dati. ")
                Controlli_Formali = False
            End If

            If TxtDataFineVolontari.Text <> "" Then
                If CDate(TxtDataInizioVolontari.Text) > CDate(TxtDataFineVolontari.Text) Then
                    MessaggiAlert("La data di inizio Volontari non può essere maggiore della data Fine Volontari. Verificare l'esatta immissione dei dati. ")
                    Controlli_Formali = False
                End If
                If CDate(TxtDataInizioVolontari.Text) = CDate(TxtDataFineVolontari.Text) Then
                    MessaggiAlert("La data di Inizio Volontari non può essere uguale della data Fine Volontari. Verificare l'esatta immissione dei dati. ")
                    Controlli_Formali = False
                End If
            Else
                MessaggiAlert("E' necessario inserire la Data Fine Volontari.")
                Controlli_Formali = False
            End If
        End If
        'agg. il 11/04/2007 da simona cordella 
        'Controllo sulla data scadenza graduatoria
        If TxtDataScadGrad.Text <> "" Then
            'Controllo se data fine volontari è null Ilaria Lombardi 27/10/2009
            If TxtDataInizioVolontari.Text = "" Or TxtDataFineVolontari.Text = "" Then
                MessaggiAlert("Non è possibile valorizzare la Data Scadenza Graduatoria se non sono valorizzate Data Inizio e Data Fine Volontari. ")
                Controlli_Formali = False
            End If

            If CDate(TxtDataScadGrad.Text) < CDate(TxtDataFineVolontari.Text) Then
                MessaggiAlert("La data Scadenza Graduatoria non può essere inferione alla data Fine Volontari. Verificare l'esatta immissione dei dati. ")
                Controlli_Formali = False
            End If
            If CDate(TxtDataScadGrad.Text) = CDate(TxtDataFineVolontari.Text) Then
                MessaggiAlert("La data Scadenza Graduatoria non può essere uguale della data Fine Volontari. Verificare l'esatta immissione dei dati. ")
                Controlli_Formali = False
            End If
        End If



    End Function

    Private Function VerificaAssociazioneAutomatica() As Boolean
        ' Generato da Simona Cordella il 03/04/2006ù
        'Controllo se è stato selezionato il flag per l'associazione automatica 
        'verifico se nel db risultano altri Bandi con il flag.
        VerificaAssociazioneAutomatica = False
        If ChkAssAutomatica.Checked = True Then
            strsql = "Select * from bando where AssociazioneAutomatica =1 "
            If lblAzione.Text <> "Inserimento" Then
                strsql = strsql & " and idbando<>  " & lblIdBando.Value & ""
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                MessaggiConferma("Esiste in archivio un bando con il flag Associazione automatica.")
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Hdd_chkFlag.Value = "true"
                VerificaAssociazioneAutomatica = True
            Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Hdd_chkFlag.Value = "false"
            End If
        Else
            Hdd_chkFlag.Value = "false"

        End If
    End Function

    Private Sub ModificaAssociazioneAutomatica(ByVal intIdBando As Integer)
        'Generato da Simona Cordella il 04/04/2006 
        'Aggiorno il flag Associazione Automatica a 0 
        strsql = "Update Bando set AssociazioneAutomatica=0 Where idbando<>" & intIdBando & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
    End Sub
#End Region
#Region "Eventi"

    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        CancellaMessaggiInfo()
        If (VerificaCampiObbligatori()) Then
            If lblAzione.Text = "Inserimento" Then
                'Verifico se il bando è già esistente
                strsql = "Select * from bando where bando='" & Replace(txtBando.Text, "'", "''") & "'"
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                If dtrgenerico.HasRows = True Then
                    MessaggiAlert("Il bando è già presente in archivio.")
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Exit Sub
                End If

                If Controlli_Formali() = False Then Exit Sub



                If ControllaCheck_TipoProgetto() = True Then
                    If ControllaCheck_RegioneCompetenza() = True Then
                        InserimentoBando()
                        ' inserisco i progetti selezionati
                        InserimentoTipiProgetto()
                        If ChkAssAutomatica.Checked = True Then
                            ModificaAssociazioneAutomatica(intIdBando)
                        End If
                        InserimentoRegioneCompetenza()
                        BloccaMaschera()
                        bloccaCheck()
                        dgTipiProgetto.Columns(3).Visible = False
                        dgTipiProgetto.Columns(4).Visible = False
                    End If
                End If
            Else
                'Verifico se il bando è già esistente
                strsql = "Select * from bando where bando='" & Replace(txtBando.Text, "'", "''") & "' and idbando<>" & lblIdBando.Value & ""
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                If dtrgenerico.HasRows = True Then
                    MessaggiAlert("Il bando è già presente in archivio.")
                    Exit Sub
                End If

                If lblStato.Text <> "Aperto" Then
                    If Controlli_Formali() = False Then Exit Sub
                    If ControllaCheck_TipoProgetto() = True Then
                        If ControllaCheck_RegioneCompetenza() = True Then
                            ModificaBando()
                            If ChkAssAutomatica.Checked = True Then
                                ModificaAssociazioneAutomatica(lblIdBando.Value)
                            End If
                            'If lblStato.Text <> "Aperto" Then
                            CancellaTipiProgetto()
                            InserimentoTipiProgetto()
                            ' aggiunto il 11/07/2006 da simona cordella
                            CancellaRegioneCompetenza()
                            InserimentoRegioneCompetenza()
                            'End If
                        End If
                    End If
                Else
                    ModificaBando()
                End If
            End If
        End If
    End Sub


    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        If lblAzione.Text = "Inserimento" Then
            Response.Redirect("WfrmMain.aspx")
        Else
            Server.Transfer("WfrmRicercaBandi.aspx")
        End If
    End Sub
    Private Sub cmdCancella_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdCancella.Click
        CancellaBando()
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
#End Region
    Private Sub cmdPubblica_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdPubblica.Click
        PubblicaBando()
    End Sub

    Private Sub ChkAssAutomatica_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkAssAutomatica.CheckedChanged
        If ChkAssAutomatica.Checked = True Then
            If lblAzione.Text = "Inserimento" Then
                VerificaAssociazioneAutomatica()
            Else
                VerificaAssociazioneAutomatica()
            End If
        End If
    End Sub

    Private Sub dgTipiProgetto_PageIndexChanged1(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgTipiProgetto.PageIndexChanged
        'dgTipiProgetto.CurrentPageIndex = e.NewPageIndex
        'dgTipiProgetto.DataSource = Session("RisultatoRicercaTipiProgetto_Sim")
        'dgTipiProgetto.DataBind()
    End Sub

    Private Sub dgTipiProgetto_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgTipiProgetto.ItemCommand
        Select Case e.CommandName
            Case "Profili"
                RicordaCheck()
                If lblAzione.Text = "Inserimento" Then
                    'Response.Redirect("WfrmElencoProfili.aspx?Hdd_rif=" & Hdd_Rif.Value & " &Hdd_DataIVol=" & Hdd_dtiniziovol.Value & " &Hdd_DataFVol=" & Hdd_dtfinevol.Value & " &Hdd_Check=" & Hdd_chkFlag.Value & " &TipoAzione=" & Request.QueryString("tipoazione") & "&Vengoda=ElencoProfili&strGruppo=" & TxtGruppo.Text & "&strCheck=" & ChkAssAutomatica.Checked & "&strDataFineVol=" & TxtDataFineVolontari.Text & "&strDataInizioVol=" & TxtDataInizioVolontari.Text & "&strDataFine=" & txtfine.Text & "&strDataInizio=" & txtInizio.Text & "&strImporto=" & txtImportoStanziato.Text & "&strAnnoRif=" & TxtAnnoRiferimento.Text & "&strRiferimento=" & txtriferimento.Text & "&strBandoBreve=" & txtBandoBreve.Text & "&strBando=" & txtBando.Text & "&strDataScadGrad=" & TxtDataScadGrad.Text & "&strTipoProgetto=" & e.Item.Cells(2).Text & "&strIdTipoProgetto=" & e.Item.Cells(1).Text)
                    Response.Redirect("WfrmElencoProfili.aspx?Hdd_rif=" & Hdd_Rif.Value & " &Hdd_DataIVol=" & Hdd_dtiniziovol.Value & " &Hdd_DataFVol=" & Hdd_dtfinevol.Value & " &Hdd_Check=" & Hdd_chkFlag.Value & " &TipoAzione=" & Request.QueryString("tipoazione") & "&Vengoda=ElencoProfili&strGruppo=" & TxtGruppo.Text & "&strCheck=" & ChkAssAutomatica.Checked & "&strDataFineVol=" & TxtDataFineVolontari.Text & "&strDataInizioVol=" & TxtDataInizioVolontari.Text & "&strDataFine=" & txtfine.Text & "&strDataInizio=" & txtInizio.Text & "&strImporto=" & txtImportoStanziato.Text & "&strAnnoRif=" & TxtAnnoRiferimento.Text & "&strRiferimento=" & txtriferimento.Text & "&strBandoBreve=" & txtBandoBreve.Text & "&strBando=" & txtBando.Text & "&strDataScadGrad=" & TxtDataScadGrad.Text & "&strNMaxVolontariProgettoItalia=" & TxtNMaxVolontariProgettoItalia.Text & "&strNMinVolontariProgettoItalia=" & TxtNMinVolontariProgettoItalia.Text & "&strNMaxVolontariProgettoEstero=" & TxtNMaxVolontariProgettoEstero.Text & "&strNMinVolontariProgettoEstero=" & TxtNMinVolontariProgettoEstero.Text & "&strNMinVolontariSedeItalia=" & TxtNMinVolontariSedeItalia.Text & "&strNMinVolontariSedeEstero=" & TxtNMinVolontariSedeEstero.Text & "&strTipoProgetto=" & e.Item.Cells(2).Text & "&strIdTipoProgetto=" & e.Item.Cells(1).Text & "&strNMaxVolontariProgettoCoprogettato=" & TxtNMaxVolontariProgettoCoprogettato.Text)
                Else
                    Response.Redirect("WfrmElencoProfili.aspx?Hdd_rif=" & Hdd_Rif.Value & " &Hdd_Check=" & Hdd_chkFlag.Value & " &&Hdd_DataIVol=" & Hdd_dtiniziovol.Value & " &Hdd_DataFVol=" & Hdd_dtfinevol.Value & " &Stato=" & lblStato.Text & "&Pagina=" & lblpage.Value & "&strIdBando= " & lblIdBando.Value & "&TipoAzione=" & Request.QueryString("tipoazione") & "&Vengoda=ElencoProfili&strGruppo=" & TxtGruppo.Text & "&strCheck=" & ChkAssAutomatica.Checked & "&strDataFineVol=" & TxtDataFineVolontari.Text & "&strDataInizioVol=" & TxtDataInizioVolontari.Text & "&strDataFine=" & txtfine.Text & "&strDataInizio=" & txtInizio.Text & "&strImporto=" & txtImportoStanziato.Text & "&strAnnoRif=" & TxtAnnoRiferimento.Text & "&strRiferimento=" & txtriferimento.Text & "&strBandoBreve=" & txtBandoBreve.Text & "&strBando=" & txtBando.Text & "&strDataScadGrad=" & TxtDataScadGrad.Text & "&strNMaxVolontariProgettoItalia=" & TxtNMaxVolontariProgettoItalia.Text & "&strNMinVolontariProgettoItalia=" & TxtNMinVolontariProgettoItalia.Text & "&strNMaxVolontariProgettoEstero=" & TxtNMaxVolontariProgettoEstero.Text & "&strNMinVolontariProgettoEstero=" & TxtNMinVolontariProgettoEstero.Text & "&strNMinVolontariSedeItalia=" & TxtNMinVolontariSedeItalia.Text & "&strNMinVolontariSedeEstero=" & TxtNMinVolontariSedeEstero.Text & "&strTipoProgetto=" & e.Item.Cells(2).Text & "&strIdTipoProgetto=" & e.Item.Cells(1).Text & "&strNMaxVolontariProgettoCoprogettato=" & TxtNMaxVolontariProgettoCoprogettato.Text)
                End If
            Case "AmbitiAttivita"
                RicordaCheck()
                If lblAzione.Text = "Inserimento" Then
                    Response.Redirect("WfrmElencoAmbitiAttivita.aspx?Hdd_rif=" & Hdd_Rif.Value & " &Hdd_DataIVol=" & Hdd_dtiniziovol.Value & " &Hdd_DataFVol=" & Hdd_dtfinevol.Value & " &Hdd_Check=" & Hdd_chkFlag.Value & " &TipoAzione=" & Request.QueryString("tipoazione") & "&Vengoda=ElencoAttivita&strGruppo=" & TxtGruppo.Text & "&strCheck=" & ChkAssAutomatica.Checked & "&strDataFineVol=" & TxtDataFineVolontari.Text & "&strDataInizioVol=" & TxtDataInizioVolontari.Text & "&strDataFine=" & txtfine.Text & "&strDataInizio=" & txtInizio.Text & "&strImporto=" & txtImportoStanziato.Text & "&strAnnoRif=" & TxtAnnoRiferimento.Text & "&strRiferimento=" & txtriferimento.Text & "&strBandoBreve=" & txtBandoBreve.Text & "&strBando=" & txtBando.Text & "&strDataScadGrad=" & TxtDataScadGrad.Text & "&strNMaxVolontariProgettoItalia=" & TxtNMaxVolontariProgettoItalia.Text & "&strNMinVolontariProgettoItalia=" & TxtNMinVolontariProgettoItalia.Text & "&strNMaxVolontariProgettoEstero=" & TxtNMaxVolontariProgettoEstero.Text & "&strNMinVolontariProgettoEstero=" & TxtNMinVolontariProgettoEstero.Text & "&strNMinVolontariSedeItalia=" & TxtNMinVolontariSedeItalia.Text & "&strNMinVolontariSedeEstero=" & TxtNMinVolontariSedeEstero.Text & "&strTipoProgetto=" & e.Item.Cells(2).Text & "&strIdTipoProgetto=" & e.Item.Cells(1).Text & "&strNMaxVolontariProgettoCoprogettato=" & TxtNMaxVolontariProgettoCoprogettato.Text)
                Else
                    Response.Redirect("WfrmElencoAmbitiAttivita.aspx?Hdd_rif=" & Hdd_Rif.Value & " &Hdd_Check=" & Hdd_chkFlag.Value & " &&Hdd_DataIVol=" & Hdd_dtiniziovol.Value & " &Hdd_DataFVol=" & Hdd_dtfinevol.Value & " &Stato=" & lblStato.Text & "&Pagina=" & lblpage.Value & "&strIdBando= " & lblIdBando.Value & "&TipoAzione=" & Request.QueryString("tipoazione") & "&Vengoda=ElencoAttivita&strGruppo=" & TxtGruppo.Text & "&strCheck=" & ChkAssAutomatica.Checked & "&strDataFineVol=" & TxtDataFineVolontari.Text & "&strDataInizioVol=" & TxtDataInizioVolontari.Text & "&strDataFine=" & txtfine.Text & "&strDataInizio=" & txtInizio.Text & "&strImporto=" & txtImportoStanziato.Text & "&strAnnoRif=" & TxtAnnoRiferimento.Text & "&strRiferimento=" & txtriferimento.Text & "&strBandoBreve=" & txtBandoBreve.Text & "&strBando=" & txtBando.Text & "&strDataScadGrad=" & TxtDataScadGrad.Text & "&strNMaxVolontariProgettoItalia=" & TxtNMaxVolontariProgettoItalia.Text & "&strNMinVolontariProgettoItalia=" & TxtNMinVolontariProgettoItalia.Text & "&strNMaxVolontariProgettoEstero=" & TxtNMaxVolontariProgettoEstero.Text & "&strNMinVolontariProgettoEstero=" & TxtNMinVolontariProgettoEstero.Text & "&strNMinVolontariSedeItalia=" & TxtNMinVolontariSedeItalia.Text & "&strNMinVolontariSedeEstero=" & TxtNMinVolontariSedeEstero.Text & "&strTipoProgetto=" & e.Item.Cells(2).Text & "&strIdTipoProgetto=" & e.Item.Cells(1).Text & "&strNMaxVolontariProgettoCoprogettato=" & TxtNMaxVolontariProgettoCoprogettato.Text)
                End If
        End Select

    End Sub

    Private Sub cmdCalcola_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdCalcola.Click
        GeneraProgressivoGruppo()
    End Sub

    Private Sub GeneraProgressivoGruppo()
        'Generato da Simona Cordella il 12/04/2006
        'Calcola il max valore del gruppo nella tabella Bandi
        strsql = "SELECT MAX(GRUPPO)+ 1 AS GRUPPO FROM BANDO"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            TxtGruppo.Text = dtrgenerico("Gruppo")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If

    End Sub

    Private Sub cmdImpVirtuali_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImpVirtuali.Click
        'Aggiunto da Simona Cordella il 15/05/2006
        'Richiamo store procedure SP_ASSEGNA_IMPORTI_VIRTUALI
        'RISULTATO STORE PROCEDURE DI MIGRAZIONE DATI: False=OK  True=ERR apro pupup con elnco errori
        StoreAssegnaImportiVirtuali(lblIdBando.Value, MyTransaction)
        Session("IdBando") = lblIdBando.Value
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""WfrmDettaglioAssegnazioneImportiVirtuali.aspx"","""",""width=600,height=400,scrollbars=si,toolbar=no,location=no,menubar=no"")" & vbCrLf)
        Response.Write("</script>")

    End Sub

    Private Function StoreAssegnaImportiVirtuali(ByVal IdBando As Long, ByVal Transazione As System.Data.SqlClient.SqlTransaction) As Boolean
        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_ASSEGNA_IMPORTI_VIRTUALI"
        CustOrderHist.Connection = Session("conn")
        CustOrderHist.Transaction = Transazione

        Dim paramEnte As SqlClient.SqlParameter
        paramEnte = New SqlClient.SqlParameter
        paramEnte.ParameterName = "@IdBando"
        paramEnte.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(paramEnte)

        Dim paramValore As SqlClient.SqlParameter
        paramValore = New SqlClient.SqlParameter
        paramValore.ParameterName = "@Valore"
        paramValore.SqlDbType = SqlDbType.Bit
        paramValore.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(paramValore)

        Dim Reader As SqlClient.SqlDataReader
        CustOrderHist.Parameters("@IdBando").Value = IdBando
        Reader = CustOrderHist.ExecuteReader()
        StoreAssegnaImportiVirtuali = CustOrderHist.Parameters("@Valore").Value
        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
    End Function

    Private Sub SelezionaCheck_RegioneCompetenza()
        'Generato da Simona CordelLa il 11/07/2006
        Dim item As DataGridItem
        If Hdd_chkFlag.Value = "" Then
            For Each item In dgRegioneCompetenza.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                check.Checked = True
                Hdd_chkFlag.Value = True
            Next
        Else
            For Each item In dgRegioneCompetenza.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check2"), CheckBox)
                check.Checked = False
                Hdd_chkFlag.Value = ""
            Next
        End If
    End Sub

    Private Sub cmdSeleziona_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles chkSelDesel.CheckedChanged
        Dim chkSelVol As CheckBox
        If chkSelDesel.Checked = True Then
            For i = 0 To dgRegioneCompetenza.Items.Count - 1
                chkSelVol = dgRegioneCompetenza.Items(i).FindControl("check2")
                chkSelVol.Checked = True
            Next
            chkSelDesel.Text = "Deseleziona tutto"
        Else
            For i = 0 To dgRegioneCompetenza.Items.Count - 1
                chkSelVol = dgRegioneCompetenza.Items(i).FindControl("check2")
                chkSelVol.Checked = False
            Next
            chkSelDesel.Text = "Seleziona tutto"
        End If
    End Sub


    Private Function VerificaCampiObbligatori() As Boolean
        Dim utility As ClsUtility = New ClsUtility()
        Dim data As Date
        Dim campiValidi As Boolean = True
        Dim dataValida As Boolean
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"
        Dim annoRiferimentoCorretto As String = "E' necessario inserire l'anno in modo corretto.<br/>"

        If (txtBando.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Bando")
            campiValidi = False
        End If
        If (txtBandoBreve.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Descr. Abbreviata Bando")
            campiValidi = False
        End If

        If (txtriferimento.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Num.Rif.")
            campiValidi = False
        End If
        If (TxtAnnoRiferimento.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Anno Rif.")
            campiValidi = False
        Else
            Hdd_Rif.Value = Mid(TxtAnnoRiferimento.Text, 6, 1)
            If InStr(TxtAnnoRiferimento.Text, "/") > 0 Then
                If Mid(TxtAnnoRiferimento.Text, 1, InStr(TxtAnnoRiferimento.Text, "/") - 1) > 1900 Then
                    If txtriferimento.Text <> "" Then

                        If Hdd_Rif.Value <> txtriferimento.Text Then
                            lblErrore.Text = lblErrore.Text + "Il numero dopo il carattere / deve essere uguale al campo numero riferimento. <br/>"
                            campiValidi = False
                            If Mid(TxtAnnoRiferimento.Text, 5, 1) <> "/" Then
                                TxtAnnoRiferimento.Text = Mid(TxtAnnoRiferimento.Text, 1, 5) & "/" & txtriferimento.Text
                            Else
                                TxtAnnoRiferimento.Text = Mid(TxtAnnoRiferimento.Text, 1, 5) & txtriferimento.Text
                            End If
                        End If
                    End If
                Else
                    lblErrore.Text = lblErrore.Text + annoRiferimentoCorretto
                    campiValidi = False
                End If
            Else
                lblErrore.Text = lblErrore.Text + annoRiferimentoCorretto
                campiValidi = False
            End If
        End If
        If (txtImportoStanziato.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Importo Stanziato")
            campiValidi = False
        End If
        If (txtInizio.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Data Inizio")
            campiValidi = False
        Else

            dataValida = ValidaData(txtInizio.Text, "Data Inizio")
            If Not (dataValida) Then
                campiValidi = False
            End If
        End If
        If (txtfine.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Data Fine")
            campiValidi = False
        Else
            dataValida = ValidaData(txtfine.Text, "Data Inizio")
            If Not (dataValida) Then
                campiValidi = False
            End If

        End If
        If (TxtNMaxVolontariProgettoItalia.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "N° Max Volontari Progetto Italia")
            campiValidi = False
        End If
        If (TxtNMaxVolontariProgettoEstero.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "N° Max Volontari Progetto Estero")
            campiValidi = False
        End If
        If (TxtNMinVolontariProgettoItalia.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "N° Min Volontari Progetto Italia")
            campiValidi = False
        End If
        If (TxtNMinVolontariProgettoEstero.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "N° Min Volontari Progetto Estero")
            campiValidi = False
        End If
        If (TxtNMinVolontariSedeItalia.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "N° Min Volontari Sede Italia")
            campiValidi = False
        End If
        If (TxtNMaxVolontariProgettoCoprogettato.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "N° Max Volontari Progetto Coprogettato")
            campiValidi = False
        End If
        If (TxtNMinVolontariSedeEstero.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "N° Min Volontari Sede Estero")
            campiValidi = False
        End If
        If (TxtGruppo.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Gruppo")
            campiValidi = False
        End If
        'if not(va
        Return campiValidi
    End Function

    Private Function ValidaData(ByVal data As String, ByVal nomeCampo As String) As Boolean
        Dim dataTmp As Date
        Dim dataValida As Boolean = True
        Dim messaggioDataValida As String = "Il valore di '{0}' non è valido. Inserire la data nel formato gg/mm/aaaa.<br/>"

        If (Date.TryParse(data, dataTmp) = False) Then
            lblErrore.Text = lblErrore.Text + String.Format(messaggioDataValida, nomeCampo)
            dataValida = False
        End If
        Return dataValida

    End Function

End Class