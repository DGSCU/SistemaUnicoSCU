Imports System.Security.Cryptography
Imports Logger.Data
Imports System.Drawing
Imports System.Data.SqlClient

Public Class WfrmStrutturaOrganizzativaSistemi
    Inherits SmartPage

    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim codiceFiscaleEnte As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        checkSpid()
        codiceFiscaleEnte = GetCodiceFiscaleEnte()

        If IsPostBack = False Then
            'Il messaggio nel caricamento della pagina può, attualmente, solo essere errore (fase adeguamento chiusa prima che premessi salva)
            If Request.QueryString("Messaggio") <> Nothing Then
                lblMessaggio.Visible = True
                lblMessaggio.CssClass = "msgErrore"
                lblMessaggio.Text = Request.QueryString("Messaggio")
            End If
            LoadMaschera()
            CaricaGrigliaSistemi(Session("IDEnte"))

        End If
        If Session("TipoUTente") = "U" Then
            pdfFormazioneGenerale.Visible = True
            pdfMonitoraggio.Visible = True
            pdfSistemaComunicazione.Visible = True
            pdfSistemaSelezione.Visible = True
            dtgSistemi.Visible = True
        End If

    End Sub


    Sub LoadMaschera()
        'SOLO ENTI TITOLARI AGGIUNGERE CONTROLLO

        Dim abilitato As Integer
        Dim dati As Integer
        Dim annullamodifica As Integer
        Dim visualizzadatiaccreditati As Integer
        Dim annullacancellazione As Integer
        Dim messaggio As String


        'RICHIAMO STORE CHE VERIFICA L'ACCESSO MASCHERA DELL' ENTE
        Accesso_Maschera_Ente(Session("TipoUtente"), Session("IDEnte"), abilitato, dati, annullamodifica, annullacancellazione, visualizzadatiaccreditati, messaggio)

        If abilitato = 0 Then ' -- 0: maschera sola lettura        1: maschera in modifica
            BloccaMaschera(messaggio)
        Else
            If AbilitaSistemiAdeguamento() = False Then
                BloccaMascheraAdeguamento("NON E' POSSIBILE MODIFICARE I SISTEMI PER LA FASE DI ADEGUAMENTO APERTA")
            End If
        End If

        CaricaEntiSistema()
        'BloccaMaschera("Dati sola lettura. (TEST)")
    End Sub
    Private Function AbilitaSistemiAdeguamento() As Boolean
        Dim dtrGen As SqlClient.SqlDataReader
        Dim blnAdeguamento As Boolean
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        strsql = "select IdEnteFase from EntiFasi where TipoFase = 2 and GETDATE() between DataInizioFase and DataFineFase and IdEnte = " & Session("IdEnte")
        dtrGen = ClsServer.CreaDatareader(strsql, Session("Conn"))
        If dtrGen.HasRows = True Then
            blnAdeguamento = True
        Else
            blnAdeguamento = False
        End If
        dtrGen.Close()
        dtrGen = Nothing

        Dim blnAbilitaSistemi As Boolean = True
        strsql = "SELECT VALORE  FROM Configurazioni where Parametro='BLOCCO_ACCREDITAMENTO'"
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        dtrGen = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGen.Read()

        If dtrGen("Valore") = "SI" And Session("TipoUtente") = "E" And blnAdeguamento Then
            blnAbilitaSistemi = False
        End If
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        Return blnAbilitaSistemi

    End Function
    Function GetCodiceFiscaleEnte() As String
        Dim cf As String = ""
        dtrgenerico = ClsServer.CreaDatareader("select CodiceFiscale from enti where IdEnte=" & Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            cf = dtrgenerico("CodiceFiscale")
        End If
        dtrgenerico.Close()
        dtrgenerico = Nothing
        Return cf

    End Function
    Function GetNomeEnte() As String
        Dim cf As String = ""
        dtrgenerico = ClsServer.CreaDatareader("select Denominazione from enti where IdEnte=" & Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            cf = dtrgenerico("Denominazione")
        End If
        dtrgenerico.Close()
        dtrgenerico = Nothing
        Return cf

    End Function
    Sub BloccaMaschera(messaggio As String)
        TxtSistemaDiSelezione.ReadOnly = True
        TxtSistemaDiSelezione.BackColor = Color.Gainsboro
        TxtSistemaDiComunicazione.ReadOnly = True
        TxtSistemaDiComunicazione.BackColor = Color.Gainsboro
        TxtMonitoraggio.ReadOnly = True
        TxtMonitoraggio.BackColor = Color.Gainsboro
        TxtFormazioneGenerale.ReadOnly = True
        TxtFormazioneGenerale.BackColor = Color.Gainsboro

        cmdSalva.Visible = False
        cmdCaricaFileSdS.Enabled = False
        btnModificaSdS.Enabled = False
        btnEliminaSdS.Enabled = False
        cmdCaricaFileSO.Enabled = False
        btnModificaSO.Enabled = False
        btnEliminaSO.Enabled = False
        cmdCaricaFileAtD.Enabled = False
        btnModificaAtD.Enabled = False
        btnEliminaAtD.Enabled = False

        MessaggiAlert(messaggio)
    End Sub

    Sub BloccaMascheraAdeguamento(messaggio As String)

        TxtSistemaDiSelezione.ReadOnly = True
        TxtSistemaDiSelezione.BackColor = Color.Gainsboro
        TxtSistemaDiComunicazione.ReadOnly = True
        TxtSistemaDiComunicazione.BackColor = Color.Gainsboro
        TxtMonitoraggio.ReadOnly = True
        TxtMonitoraggio.BackColor = Color.Gainsboro
        TxtFormazioneGenerale.ReadOnly = True
        TxtFormazioneGenerale.BackColor = Color.Gainsboro

        'cmdSalva.Visible = False
        cmdCaricaFileSdS.Enabled = False
        btnModificaSdS.Enabled = False
        btnEliminaSdS.Enabled = False
        'cmdCaricaFileSO.Enabled = False
        'btnModificaSO.Enabled = False
        'btnEliminaSO.Enabled = False
        'cmdCaricaFileAtD.Enabled = False
        'btnModificaAtD.Enabled = False
        'btnEliminaAtD.Enabled = False

        MessaggiAlert(messaggio)
    End Sub

    Structure Sistemi
        'Corrisponde alla tabella sistemi, definita anche qui per non fare ulteriori queries
        Const StrutturaOrganizzativa = "S01"
        Const StrutturaOrganizzativaTipoAllegato = 9
        Const SistemaComunicazione = "S02"
        Const SistemaSelezione = "S03"
        Const SistemaSelezioneTipoAllegato = 10
        Const SistemaFormazioneGenerale = "S04"
        Const SistemaMonitoraggio = "S05"
        Const AttoDesignazione = "S06"
        Const AttoDesignazioneTipoAllegato = 3
    End Structure

    Sub CaricaEntiSistema()
        'Carica gli allegati
        Dim dtEntiSistema As DataTable
        Session("LoadedSO") = Nothing   'allegato sistema organizzativo
        Session("LoadedSdS") = Nothing  'allegato sistema di selezione
        Session("LoadedAtD") = Nothing  'allegato atto di designazione

        strsql = "select es.IDEnteSistema,s.Codice,es.Testo, a.IdEnteDocumento, a.IdTipoAllegato, FileName, HashValue, len(BinData)  FileLength, BinData, DataInserimento, IdEnteFase, Stato"
        strsql += " from entisistemi es"
        'tutte inner join: i campi sono TUTTI obbligatori
        strsql += " inner join sistemi s on s.IDSistema=es.IDSistema"
        strsql += " left join entidocumenti a on es.IdAllegato=a.IdEnteDocumento"
        strsql += " where s.Codice in('S01','S02','S03','S04','S05','S06') and es.idente=" & Session("IdEnte")

        'originale
        'dtEntiSistema = ClsServer.DataTableGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("conn")))
        'correzione
        dtEntiSistema = ClsServer.DataTableGenerico(strsql, Session("conn"))

        rowSdS.Visible = False
        rowNoSdS.Visible = True
        rowSO.Visible = False
        rowNoSO.Visible = True
        rowAtD.Visible = False
        rowNoAtD.Visible = True

        If dtEntiSistema IsNot Nothing And dtEntiSistema.Rows.Count > 0 Then

            'carico dati e metto in sessione gli allegati
            For Each dr As DataRow In dtEntiSistema.Rows
                If dr("Codice").ToString() = Sistemi.StrutturaOrganizzativa Then
                    If dr("IdEnteDocumento") IsNot DBNull.Value Then
                        CaricaAllegatoFromDatarow(dr, "LoadedSO", rowNoSO, rowSO, txtSOFilename, txtSOHash, txtSOData)
                        If False Then
                            Dim filename As String = dr("FileName")
                            Dim hashValue As String = dr("HashValue")
                            Dim filelength As Integer = CInt(dr("FileLength"))
                            Dim blob As Byte() = dr("BinData")
                            Dim dataInserimento As Date = dr("DataInserimento")
                            txtSOFilename.Text = filename
                            txtSOData.Text = dataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                            Session("LoadedSO") = New Allegato() With {
                         .Blob = blob,
                         .Filename = filename,
                         .Hash = hashValue,
                         .Filesize = filelength,
                         .DataInserimento = dataInserimento
                        }
                        End If
                    End If
                End If


                If dr("Codice").ToString() = Sistemi.SistemaComunicazione Then
                    TxtSistemaDiComunicazione.Text = dr("Testo") & ""
                End If
                If dr("Codice").ToString() = Sistemi.SistemaSelezione Then
                    TxtSistemaDiSelezione.Text = dr("Testo") & ""
                    If dr("IdEnteDocumento") IsNot DBNull.Value Then
                        CaricaAllegatoFromDatarow(dr, "LoadedSdS", rowNoSdS, rowSdS, txtSdSFilename, txtSdSHash, txtSdSData)
                        If False Then
                            Dim filename As String = dr("FileName")
                            Dim hashValue As String = dr("HashValue")
                            Dim filelength As Integer = CInt(dr("FileLength"))
                            Dim blob As Byte() = dr("BinData")
                            Dim dataInserimento As Date = dr("DataInserimento")
                            txtSdSFilename.Text = filename
                            txtSdSData.Text = dataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                            Session("LoadedSdS") = New Allegato() With {
                         .Blob = blob,
                         .Filename = filename,
                         .Hash = hashValue,
                         .Filesize = filelength,
                         .DataInserimento = dataInserimento
                        }
                        End If
                    End If
                End If
                If dr("Codice").ToString() = Sistemi.SistemaFormazioneGenerale Then
                    TxtFormazioneGenerale.Text = dr("Testo") & ""
                End If
                If dr("Codice").ToString() = Sistemi.SistemaMonitoraggio Then
                    TxtMonitoraggio.Text = dr("Testo") & ""
                End If
                If dr("Codice").ToString() = Sistemi.AttoDesignazione Then
                    If dr("IdEnteDocumento") IsNot DBNull.Value Then
                        CaricaAllegatoFromDatarow(dr, "LoadedAtD", rowNoAtD, rowAtD, txtAtDFilename, txtAtDHash, txtAtDData)
                        If False Then
                            Dim filename As String = dr("FileName")
                            Dim hashValue As String = dr("HashValue")
                            Dim filelength As Integer = CInt(dr("FileLength"))
                            Dim blob As Byte() = dr("BinData")
                            Dim dataInserimento As Date = dr("DataInserimento")
                            txtAtDFilename.Text = filename
                            txtAtDData.Text = dataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                            Session("LoadedAtD") = New Allegato() With {
                         .Blob = blob,
                         .Filename = filename,
                         .Hash = hashValue,
                         .Filesize = filelength,
                         .DataInserimento = dataInserimento
                        }
                        End If
                    End If
                End If
            Next
        End If

        If False Then
            If Session("LoadedSO") Is Nothing Then
                Dim allegatoSO = Session("LoadedSO")
                rowNoSO.Visible = True
                rowSO.Visible = False
            Else
                rowNoSO.Visible = False
                rowSO.Visible = True
            End If

            If Session("LoadedSdS") Is Nothing Then
                Dim allegatoSdS = Session("LoadedSdS")
                rowNoSdS.Visible = True
                rowSdS.Visible = False
            Else
                rowNoSdS.Visible = False
                rowSdS.Visible = True
            End If

            If Session("LoadedAtD") Is Nothing Then
                Dim allegatoAtD = Session("LoadedAtD")
                rowNoAtD.Visible = True
                rowAtD.Visible = False
            Else
                rowNoAtD.Visible = False
                rowAtD.Visible = True
            End If
        End If


    End Sub

    Private Sub cmdAllegaSO_Click(sender As Object, e As EventArgs) Handles cmdAllegaSO.Click
        'Verifica se è stato inserito il file
        If fileSO.PostedFile Is Nothing Or String.IsNullOrEmpty(fileSO.PostedFile.FileName) Then
            MessaggiPopup("Non è stato scelto nessun file per il caricamento della Struttura Organizzativa", lblErroreUploadSO, popUploadSO)
            Exit Sub
        End If
        'Controllo Tipo File
        If VerificaEstensioneFile(fileSO) = False Then
            MessaggiPopup("Il formato file della Struttura Organizzativa non è corretto. È possibile associare solo documenti nel formato .PDF", lblErroreUploadSO, popUploadSO)
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileSO.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length - 1)

        If iLen <= 0 Then
            MessaggiPopup("Attenzione. Impossibile caricare un file vuoto.", lblErroreUploadSO, popUploadSO)
            Exit Sub
        End If

        If iLen > 20971520 Then
            MessaggiPopup("Attenzione. La dimensione massima file è di 20 MB.", lblErroreUploadSO, popUploadSO)
            Exit Sub
        End If

        Dim bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileSO.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'confronto hash con quello (eventuale) SdS
        If Session("LoadedSdS") IsNot Nothing Then
            Dim SdS As Allegato = Session("LoadedSdS")
            If hashValue = SdS.Hash Then
                MessaggiPopup("Attenzione. Selezionare un file diverso dal PDF del SIstema di Selezione.", lblErroreUploadSO, popUploadSO)
                Exit Sub
            End If
        End If

        Dim AtD As Allegato = Session("LoadedAtD")
        If Not IsNothing(AtD) AndAlso hashValue = AtD.Hash Then
            MessaggiPopup("Attenzione. Selezionare un file diverso dal PDF dell'Atto di designazione.", lblErroreUploadAtD, popUploadAtD)
            Exit Sub
        End If

        'Salvo File In Sessione
        Dim SO As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "STRUTTORG_" & codiceFiscaleEnte & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now
        }
        Session("LoadedSO") = SO
        'Se Il CV è caricato in Sessione (Inserimento)
        rowNoSO.Visible = False
        rowSO.Visible = True
        txtSOFilename.Text = SO.Filename
        txtSOHash.Text = SO.Hash
        txtSOData.Text = SO.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiSuccess("Struttura Organizzativa Caricata Correttamente")
    End Sub

    Private Sub cmdAllegaSdS_Click(sender As Object, e As EventArgs) Handles cmdAllegaSdS.Click
        'Verifica se è stato inserito il file
        If fileSdS.PostedFile Is Nothing Or String.IsNullOrEmpty(fileSdS.PostedFile.FileName) Then
            MessaggiPopup("Non è stato scelto nessun file per il caricamento del Sistema di Selezione", lblErroreUploadSdS, popUploadSdS)
            Exit Sub
        End If
        'Controllo Tipo File
        If VerificaEstensioneFile(fileSdS) = False Then
            MessaggiPopup("Il formato file del Sistema di Selezione non è corretto. È possibile associare solo documenti nel formato .PDF", lblErroreUploadSdS, popUploadSdS)
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileSdS.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length - 1)

        If iLen <= 0 Then
            MessaggiPopup("Attenzione. Impossibile caricare un file vuoto.", lblErroreUploadSdS, popUploadSdS)
            Exit Sub
        End If

        If iLen > 20971520 Then
            MessaggiPopup("Attenzione. La dimensione massima del file è di 20 MB.", lblErroreUploadSdS, popUploadSdS)
            Exit Sub
        End If

        Dim bBLOBStorage = ClsServer.StreamToByte(fs)

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileSdS.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'confronto hash con quello (eventuale) SO
        If Session("LoadedSO") IsNot Nothing Then
            Dim SO As Allegato = Session("LoadedSO")
            If hashValue = SO.Hash Then
                MessaggiPopup("Attenzione. Selezionare un file diverso dal PDF della Struttura Organizzativa.", lblErroreUploadSdS, popUploadSdS)
                Exit Sub
            End If
        End If

        Dim AtD As Allegato = Session("LoadedAtD")
        If Not IsNothing(AtD) AndAlso hashValue = AtD.Hash Then
            MessaggiPopup("Attenzione. Selezionare un file diverso dal PDF dell'Atto di designazione.", lblErroreUploadAtD, popUploadAtD)
            Exit Sub
        End If

        'Salvo File In Sessione
        Dim SdS As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "SISTSEL_" & codiceFiscaleEnte & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
        .DataInserimento = Date.Now
        }
        Session("LoadedSdS") = SdS
        'Se Il CV è caricato in Sessione (Inserimento)
        rowNoSdS.Visible = False
        rowSdS.Visible = True
        txtSdSFilename.Text = SdS.Filename
        txtSdSHash.Text = SdS.Hash
        txtSdSData.Text = SdS.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiSuccess("Sistema di Selezione Caricato Correttamente")
    End Sub

    Private Sub cmdAllegaAtD_Click(sender As Object, e As EventArgs) Handles cmdAllegaAtD.Click
        'Verifica se è stato inserito il file
        If fileAtD.PostedFile Is Nothing Or String.IsNullOrEmpty(fileAtD.PostedFile.FileName) Then
            MessaggiPopup("Non è stato scelto nessun file per il caricamento dell'Atto di designazione", lblErroreUploadAtD, popUploadAtD)
            Exit Sub
        End If
        'Controllo Tipo File
        If VerificaEstensioneFile(fileAtD, True) = False Then
            MessaggiPopup("Il formato file dell'Atto di designazione non è corretto. È possibile associare solo documenti nel formato .PDF o .PDF.P7M", lblErroreUploadAtD, popUploadAtD)
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileAtD.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length - 1)

        If iLen <= 0 Then
            MessaggiPopup("Attenzione. Impossibile caricare un file vuoto.", lblErroreUploadAtD, popUploadAtD)
            Exit Sub
        End If

        If iLen > 20971520 Then
            MessaggiPopup("Attenzione. La dimensione massima del file è di 20 MB.", lblErroreUploadAtD, popUploadAtD)
            Exit Sub
        End If

        Dim bBLOBStorage = ClsServer.StreamToByte(fs)

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileAtD.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'confronto hash con quello (eventuale) SO
        If Session("LoadedSO") IsNot Nothing Then
            Dim SO As Allegato = Session("LoadedSO")
            If Not IsNothing(SO) AndAlso hashValue = SO.Hash Then
                MessaggiPopup("Attenzione. Selezionare un file diverso dal PDF della Struttura Organizzativa.", lblErroreUploadAtD, popUploadAtD)
                Exit Sub
            End If
        End If

        Dim SdS As Allegato = Session("LoadedSdS")
        If Not IsNothing(SdS) AndAlso hashValue = SdS.Hash Then
            MessaggiPopup("Attenzione. Selezionare un file diverso dal PDF del Sistema di Selezione.", lblErroreUploadAtD, popUploadAtD)
            Exit Sub
        End If

        'Salvo File In Sessione
        Dim AtD As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "ATTODESIGNAZIONE_" & codiceFiscaleEnte & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
        .DataInserimento = Date.Now
        }
        Session("LoadedAtD") = AtD
        'Se Il CV è caricato in Sessione (Inserimento)
        rowNoAtD.Visible = False
        rowAtD.Visible = True
        txtAtDFilename.Text = AtD.Filename
        txtAtDHash.Text = AtD.Hash
        txtAtDData.Text = AtD.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiSuccess("Atto di designazione caricato Correttamente")
    End Sub

    Private Sub btnEliminaSO_Click(sender As Object, e As ImageClickEventArgs) Handles btnEliminaSO.Click
        ClearSessionSO()
    End Sub
    Private Sub ClearSessionSO()
        Session("LoadedSO") = Nothing
        rowNoSO.Visible = True
        rowSO.Visible = False
    End Sub

    Private Sub btnEliminaSds_Click(sender As Object, e As ImageClickEventArgs) Handles btnEliminaSdS.Click
        ClearSessionSdS()
    End Sub
    Private Sub ClearSessionSdS()
        Session("LoadedSdS") = Nothing
        rowNoSdS.Visible = True
        rowSdS.Visible = False
    End Sub

    Private Sub btnEliminaAtD_Click(sender As Object, e As ImageClickEventArgs) Handles btnEliminaAtD.Click
        ClearSessionAtD()
    End Sub
    Private Sub ClearSessionAtD()
        Session("LoadedAtD") = Nothing
        rowNoAtD.Visible = True
        rowAtD.Visible = False
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("DettaglioFunzioni.aspx?IdVoceMenu=2")
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Try

            If ControllaCampi() = False Then
                Exit Sub
            End If

            'Il salvataggio dei 5 sistemi inclusi in questa maschera è sempre totale: se non esistono i record si creano altrimenti si va in update e la chiave di ricerca è l'idente
            Dim MyTransaction As System.Data.SqlClient.SqlTransaction
            Dim MyCommand = New SqlClient.SqlCommand
            Dim swErr As Boolean
            Dim _dt As DataTable
            Dim _idAllegato As Integer
            Dim _res As Integer
            Dim _idEnteFase As Integer = 0

            MyCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
            Try
                Dim _cE As New clsEnte
                _idEnteFase = _cE.RicavaIdEnteFase(Session("IdEnte"), MyCommand.Connection)
                If _idEnteFase = 0 Then Response.Redirect("WfrmStrutturaOrganizzativaSistemi.aspx?Messaggio=La Fase di Adeguamento risulta chiusa.")

                MyTransaction = MyCommand.Connection.BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
                MyCommand.Transaction = MyTransaction

                SalvaStrutturaOrganizzativa(MyCommand, Sistemi.StrutturaOrganizzativa, _idEnteFase)
                SalvaStrutturaOrganizzativa(MyCommand, Sistemi.SistemaComunicazione, _idEnteFase)
                SalvaStrutturaOrganizzativa(MyCommand, Sistemi.SistemaSelezione, _idEnteFase)
                SalvaStrutturaOrganizzativa(MyCommand, Sistemi.SistemaFormazioneGenerale, _idEnteFase)
                SalvaStrutturaOrganizzativa(MyCommand, Sistemi.SistemaMonitoraggio, _idEnteFase)
                SalvaStrutturaOrganizzativa(MyCommand, Sistemi.AttoDesignazione, _idEnteFase)

                MyTransaction.Commit()

            Catch exc As Exception

                MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
                swErr = True
                Log.Error(LogEvent.STRUTTURA_ORGANIZZATIVA_ERRORE, "Errore nel salvataggio", exception:=exc)
            End Try

            MyCommand.Dispose()

            If swErr = False Then
                'Esito positivo
                lblMessaggio.Visible = True
                lblMessaggio.CssClass = "msgInfo"
                lblMessaggio.Text = "Operazione di inserimento dei dati effettuata con successo."
            Else
                'Errore salvataggio
                lblMessaggio.Visible = True
                lblMessaggio.CssClass = "msgErrore"
                lblMessaggio.Text = "Errore durante l'operazione di inserimento dei dati."
            End If
        Catch exce As Exception
            Log.Error(LogEvent.STRUTTURA_ORGANIZZATIVA_ERRORE, "Errore nel cmdSalva_click", exception:=exce)
        End Try

    End Sub

    Sub SalvaStrutturaOrganizzativa(ByVal MyCommand As SqlClient.SqlCommand, ByVal CodiceSistema As String, ByVal IdEnteFase As Integer)
        Dim _dt As DataTable
        Dim _idAllegato As Integer = 0
        Dim _idTipoAllegato As Integer
        Dim _allegato As Allegato
        Dim _testo As String = ""

        'cerco l'eventuale record da updatare (in entisistemi)
        strsql = "select es.IDEnteSistema from entisistemi es inner join sistemi s on es.idsistema=s.idsistema where es.IDEnte=" & Session("IdEnte") & " and s.Codice='" & CodiceSistema & "'"

        _dt = ClsServer.CreaDataTableInTransazione(strsql, False, MyCommand.Connection, MyCommand.Transaction)
        If _dt Is Nothing OrElse _dt.Rows.Count <= 1 Then

            'salvo l'eventuale l'allegato
            If CodiceSistema = Sistemi.StrutturaOrganizzativa Or CodiceSistema = Sistemi.SistemaSelezione Or CodiceSistema = Sistemi.AttoDesignazione Then
                If CodiceSistema = Sistemi.StrutturaOrganizzativa Then
                    _allegato = Session("LoadedSO")
                    _idTipoAllegato = Sistemi.StrutturaOrganizzativaTipoAllegato
                ElseIf CodiceSistema = Sistemi.SistemaSelezione Then
                    _allegato = Session("LoadedSdS")
                    _idTipoAllegato = Sistemi.SistemaSelezioneTipoAllegato
                Else
                    _allegato = Session("LoadedAtD")
                    _idTipoAllegato = Sistemi.AttoDesignazioneTipoAllegato
                End If

                If _allegato IsNot Nothing Then
                    _idAllegato = SalvaAllegato(_allegato, _idTipoAllegato, IdEnteFase, MyCommand)
                End If
            End If

            'salvo il sistema
            Select Case CodiceSistema
                Case Sistemi.SistemaComunicazione
                    _testo = Left(TxtSistemaDiComunicazione.Text, 6000)
                Case Sistemi.SistemaSelezione
                    _testo = Left(TxtSistemaDiSelezione.Text, 15000)
                Case Sistemi.SistemaFormazioneGenerale
                    _testo = Left(TxtFormazioneGenerale.Text, 21000)
                Case Sistemi.SistemaMonitoraggio
                    _testo = Left(TxtMonitoraggio.Text, 12000)
                Case Else
                    _testo = ""
            End Select

            Dim _identesistema = 0
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then _identesistema = CInt(_dt.Rows(0)("IDEnteSistema").ToString())

            SalvaEnteSistema(_identesistema, CInt(Session("IdEnte")), Session("Account"), _testo, _idAllegato, CodiceSistema, IdEnteFase, MyCommand)
        Else
            Throw New System.Exception("Errore nei dati, più record presenti nella entisistemi per stesso ente e sistema")
        End If
    End Sub


    Sub SalvaEnteSistema(IdEnteSistema As Integer, IdEnte As Integer, Username As String, Testo As String, IdAllegato As Integer, CodiceSistema As String, IdEnteFase As Integer, MyCommand As SqlClient.SqlCommand)
        'insert o update con storicizzazione in entisistemistorico
        Dim strsql As String
        MyCommand.Parameters.Clear()
        MyCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
        MyCommand.Parameters.AddWithValue("@Username", Username)
        If Not String.IsNullOrEmpty(Testo) Then MyCommand.Parameters.AddWithValue("@Testo", Testo) Else MyCommand.Parameters.AddWithValue("@Testo", DBNull.Value)
        If IdAllegato > 0 Then MyCommand.Parameters.AddWithValue("@IdAllegato", IdAllegato) Else MyCommand.Parameters.AddWithValue("@IdAllegato", DBNull.Value)
        MyCommand.Parameters.AddWithValue("@CodiceSistema", CodiceSistema)
        If IdEnteSistema = 0 Then
            strsql = "INSERT INTO ENTISISTEMI OUTPUT INSERTED.IdEnteSistema select @IdEnte,s.IDSistema,@Username,getdate(),0,NULL,NULL,@Testo,@IdAllegato from sistemi s where Codice=@CodiceSistema"
            MyCommand.CommandText = strsql
            IdEnteSistema = CInt(MyCommand.ExecuteScalar)
            strsql = "INSERT INTO entisistemiStorico select @IdEnteSistema, @IdEnte,s.IDSistema,@Username,getdate(),0,NULL,NULL,@Testo,@IdAllegato,@IdEnteFase from sistemi s where Codice=@CodiceSistema"
            MyCommand.Parameters.AddWithValue("@IdEnteSistema", IdEnteSistema)
            MyCommand.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
            MyCommand.CommandText = strsql
            MyCommand.ExecuteNonQuery()
        Else
            strsql = "update entisistemi set IDEnte=@IdEnte,IDSistema=(select idsistema from sistemi where codice=@CodiceSistema),Username=@UserName,DataCreazioneRecord=GETDATE(),Accreditato=0,DataAccreditamento=null,UsernameAccreditatore=null,Testo=@Testo,IdAllegato=@IdAllegato where identesistema=@IdEnteSistema"
            MyCommand.Parameters.AddWithValue("@IdEnteSistema", IdEnteSistema)
            MyCommand.CommandText = strsql
            MyCommand.ExecuteNonQuery()
            strsql = "INSERT INTO entisistemiStorico select @IdEnteSistema, @IdEnte,s.IDSistema,@Username,getdate(),0,NULL,NULL,@Testo,@IdAllegato,@IdEnteFase from sistemi s where Codice=@CodiceSistema"
            MyCommand.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
            MyCommand.CommandText = strsql
            MyCommand.ExecuteNonQuery()
        End If
    End Sub
    Function checkNotEmpty(ByVal textbox As System.Web.UI.WebControls.TextBox, ByVal errMsg As String) As Boolean
        If textbox.Text.Trim = String.Empty Then
            lblMessaggio.Visible = True
            lblMessaggio.CssClass = "msgErrore"
            lblMessaggio.Text += errMsg + "<br>"
            Return False
        End If
        Return True
    End Function
    Function checkLength(ByVal textbox As System.Web.UI.WebControls.TextBox, ByVal nomeCampo As String, ByVal minimo As Integer, ByVal massimo As String) As Boolean
        If textbox.Text.Trim = String.Empty Then Return True 'i controlli sul campo vuoto sono eseguiti da altra funzione
        Dim lunghezza As Integer = textbox.Text.Trim.Length
        If lunghezza < minimo Or lunghezza > massimo Then
            lblMessaggio.Visible = True
            lblMessaggio.CssClass = "msgErrore"
            lblMessaggio.Text += String.Format("il campo {0} deve essere composto da un minimo di {1} ad un massimo di {2} caratteri. Attualmente ne sono stati inseriti {3}.<br>", _
                nomeCampo, minimo, massimo, lunghezza)
            Return False
        End If
        Return True
    End Function
    Function ControllaCampi() As Boolean
        Dim campiValidi As Boolean = True
        lblMessaggio.Visible = False
        lblMessaggio.Text = ""

        'Se l'ente titolare e' stato iscritto prima del 01/06/2021, ha sicuramente gia' comunicato i sistemi, quindi qui nessun controllo e' necessario
        If Not ClsServer.enteNuovo(Session("IdEnte"), Session("conn")) Then
            'controllare solo la lunghezza dei campi
            campiValidi = checkLength(TxtSistemaDiComunicazione, "Sistema di Comunicazione", 3000, 6000) _
            And checkLength(TxtSistemaDiSelezione, "Sistema di Selezione", 7500, 15000) _
            And checkLength(TxtFormazioneGenerale, "Sistema per la Formazione Generale degli Operatori Volontari e per la formazione delle figure dell’Ente", 10500, 21000) _
            And checkLength(TxtMonitoraggio, "Sistema di Monitoraggio", 6000, 12000)
            Return campiValidi
            End If

            campiValidi = checkNotEmpty(TxtSistemaDiComunicazione, "Sistema di Comunicazione obbligatorio.") _
            And checkNotEmpty(TxtSistemaDiSelezione, "Sistema di Selezione obbligatorio.") _
            And checkNotEmpty(TxtFormazioneGenerale, "Sistema per la Formazione Generale degli Operatori Volontari e per la formazione delle figure dell’Ente obbligatorio.") _
            And checkNotEmpty(TxtMonitoraggio, "Sistema di Monitoraggio obbligatorio.") _
            And checkLength(TxtSistemaDiComunicazione, "Sistema di Comunicazione", 3000, 6000) _
            And checkLength(TxtSistemaDiSelezione, "Sistema di Selezione", 7500, 15000) _
            And checkLength(TxtFormazioneGenerale, "Sistema per la Formazione Generale degli Operatori Volontari e per la formazione delle figure dell’Ente", 10500, 21000) _
            And checkLength(TxtMonitoraggio, "Sistema di Monitoraggio", 6000, 12000)

            If Session("LoadedSO") Is Nothing Then
                lblMessaggio.Visible = True
                lblMessaggio.CssClass = "msgErrore"
                lblMessaggio.Text += "Il file della Struttura Organizzativa è obbligatorio." + "</br>"
                campiValidi = False
            End If

            If False And Session("LoadedSdS") Is Nothing Then
                lblMessaggio.Visible = True
                lblMessaggio.CssClass = "msgErrore"
                lblMessaggio.Text += "Il file del Sistema di Selezione è obbligatorio." + "</br>"
                campiValidi = False
            End If

            Return campiValidi
    End Function

    Private Function GeneraHash(ByVal FileinByte() As Byte) As String
        Dim tmpHash() As Byte

        tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

        GeneraHash = ByteArrayToString(tmpHash)
        Return GeneraHash
    End Function
    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim i As Integer
        Dim sOutput As New StringBuilder(arrInput.Length)
        For i = 0 To arrInput.Length - 1
            sOutput.Append(arrInput(i).ToString("X2"))
        Next
        Return sOutput.ToString()
    End Function

    Private Sub MessaggiPopup(ByVal strMessaggio As String, ByRef _label As Label, ByRef _popup As AjaxControlToolkit.ModalPopupExtender)
        _label.Visible = True
        _label.Text = strMessaggio
        Log.Information(LogEvent.STRUTTURA_ORGANIZZATIVA_INFO, "Popup con messaggio", parameters:=strMessaggio)
        _popup.Show()
    End Sub

    Private Sub MessaggiSuccess(ByVal strMessaggio)
        lblMessaggio.Visible = True
        lblMessaggio.CssClass = "msgInfo"
        lblMessaggio.Text = strMessaggio
    End Sub

    Private Sub MessaggiAlert(ByVal strMessaggio)
        lblMessaggio.Visible = True
        lblMessaggio.CssClass = "msgErrore"
        lblMessaggio.Text = strMessaggio
    End Sub

    Function VerificaEstensioneFile(ByVal objPercorsoFile As HtmlInputFile, Optional ByVal permettiP7m As Boolean = False) As Boolean
        'sono accettati solo documento con estensione .pdf e .pdf.p7m
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        VerificaEstensioneFile = False

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeFile = Mid(objPercorsoFile.Value, i, 1) & NomeFile
        Next

        If UCase(Right(NomeFile, 4)) = ".PDF" Or (permettiP7m And UCase(Right(NomeFile, 8)) = ".PDF.P7M") Then
            '            Or UCase(Right(NomeFile, 8)) = ".PDF.P7M"         Then
            Return True
        Else
            Return False
        End If

    End Function

    Protected Sub btnDownloadSO_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadSO.Click
        If Session("LoadedSO") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearSessionSO()
            Exit Sub
        End If
        Dim SO As Allegato = Session("LoadedSO")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & SO.Filename)
        Response.BinaryWrite(SO.Blob)
        Response.End()
    End Sub

    Protected Sub btnDownloadSdS_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadSdS.Click
        If Session("LoadedSdS") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearSessionSdS()
            Exit Sub
        End If
        Dim SdS As Allegato = Session("LoadedSdS")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & SdS.Filename)
        Response.BinaryWrite(SdS.Blob)
        Response.End()
    End Sub

    Protected Sub btnDownloadAtD_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadAtD.Click
        If Session("LoadedAtD") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearSessionAtD()
            Exit Sub
        End If
        Dim AtD As Allegato = Session("LoadedAtD")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & AtD.Filename)
        Response.BinaryWrite(AtD.Blob)
        Response.End()
    End Sub

    Public Sub Accesso_Maschera_Ente(ByVal TipoUtente As String, ByVal IDEnte As Integer, ByRef abilitato As Integer, ByRef dati As Integer, ByRef annullamodifica As Integer, ByRef annullacancellazione As Integer, ByRef visualizzadatiaccreditati As Integer, ByRef messaggio As String)

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  25/07/2014
        'FUNZIONALITA': Verifica le condizioni di accreditamento e le eventuali modifiche in corso sui dati dell'ente
        '               e ritorna all'applicazione la configurazione da applicare alla maschera di gestion
        '               @abilitato                  -- 0: maschera sola lettura        1: maschera in modifica
        '               @dati                         -- 0: dati tabelle reali        1: dati tabelle variazioni
        '               @annullamodifica              -- 0: funzione non abilitata    1: funzione abilitata
        '               @visualizzadatiaccreditati     -- 0: funzione non abilitata    1: funzione abilitata
        '               @annullacancellazione       -- 0: funzione non abilitata    1: funzione abilitata
        '               @messaggio                  -- eventuale messaggio di ritorno da visualizzare all'utente



        Try
            Dim SqlCmd As SqlClient.SqlCommand
            '            Dim Dt As New DataTable
            '           Dim Da As New SqlClient.SqlDataAdapter(SqlCmd)

            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ACCESSOMASCHERA_ENTE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@TipoUtente", SqlDbType.VarChar).Value = TipoUtente
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IDEnte

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@abilitato"
            'sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.TinyInt
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@dati"
            'sparam1.Size = 100
            sparam2.SqlDbType = SqlDbType.TinyInt
            sparam2.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@annullamodifica"
            'sparam1.Size = 100
            sparam3.SqlDbType = SqlDbType.TinyInt
            sparam3.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam3)

            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@visualizzadatiaccreditati"
            'sparam1.Size = 100
            sparam4.SqlDbType = SqlDbType.TinyInt
            sparam4.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam4)

            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@annullacancellazione"
            'sparam1.Size = 100
            sparam5.SqlDbType = SqlDbType.TinyInt
            sparam5.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam5)


            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@messaggio"
            sparam6.Size = 1000
            sparam6.SqlDbType = SqlDbType.VarChar
            sparam6.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam6)

            SqlCmd.ExecuteNonQuery()


            abilitato = SqlCmd.Parameters("@abilitato").Value
            dati = SqlCmd.Parameters("@dati").Value
            annullamodifica = SqlCmd.Parameters("@annullamodifica").Value
            visualizzadatiaccreditati = SqlCmd.Parameters("@visualizzadatiaccreditati").Value
            annullacancellazione = SqlCmd.Parameters("@annullacancellazione").Value
            messaggio = SqlCmd.Parameters("@messaggio").Value
        Catch ex As Exception

            lblMessaggio.Text = ex.Message
        Finally
            'ConnX.Close()
            'SqlCmd = Nothing
        End Try

    End Sub
    Private Sub downloadCampo(ByVal nomeCampo As String, ByVal contenuto As String)
        Dim nomeEnte As String = GetNomeEnte()
        Dim codiceFiscale As String = GetCodiceFiscaleEnte()
        Dim oW As New AsposeWord()
        oW.open(Server.MapPath("download/Master/templateSistema.docx"))
        oW.addFieldValue("nomeEnte", nomeEnte)
        oW.addFieldValue("nomeCampo", nomeCampo)
        oW.addFieldValue("contenuto", contenuto)
        oW.merge()
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & codiceFiscale & "_" & Replace(nomeCampo, " ", "_") & ".pdf")
        Response.BinaryWrite(oW.pdfBytes)
        Response.End()
    End Sub
    Private Sub pdfFormazioneGenerale_Click(sender As Object, e As EventArgs) Handles pdfFormazioneGenerale.Click
        'genera il pdf con il contenuto del campo
        downloadCampo("Formazione Generale", TxtFormazioneGenerale.Text)
    End Sub

    Private Sub pdfMonitoraggio_Click(sender As Object, e As EventArgs) Handles pdfMonitoraggio.Click
        downloadCampo("Monitoraggio", TxtMonitoraggio.Text)
    End Sub

    Private Sub pdfSistemaComunicazione_Click(sender As Object, e As EventArgs) Handles pdfSistemaComunicazione.Click
        downloadCampo("Sistema di Comunicazione", TxtSistemaDiComunicazione.Text)
    End Sub

    Private Sub pdfSistemaSelezione_Click(sender As Object, e As EventArgs) Handles pdfSistemaSelezione.Click
        downloadCampo("Sistema di Selezione", TxtSistemaDiSelezione.Text)
    End Sub
    Function StatoEnte(ByVal IdEnte As Integer) As String
        Dim strStatoEnte As String
        If Not IsNothing(dtrgenerico) Then dtrgenerico.Close()
        Dim querySql As String = "  SELECT s.StatoEnte from enti e inner join statienti s on e.idstatoente=s.idstatoente where idente =" & IdEnte
        dtrgenerico = ClsServer.CreaDatareader(querySql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            strStatoEnte = dtrgenerico("StatoEnte")
        End If
        dtrgenerico.Close()
        Return strStatoEnte
    End Function
    Sub CaricaGrigliaSistemi(ByVal IdEnte As Integer)
        dtgSistemi.CurrentPageIndex = 0
        Dim querySql As String = ""
        querySql = "  SELECT s.IDSistema,S.Sistema ,Accreditato,"
        querySql &= " CASE WHEN ISNULL(Accreditato,0)=0 THEN 'Da Validare' WHEN ISNULL(Accreditato,0)='-1' THEN 'Non Iscritto'  ELSE 'Iscritto' END  AS STATO, "
        querySql &= " CASE WHEN ISNULL(Accreditato,0)=0 then 'SI' WHEN ISNULL(Accreditato,0)='-1' then 'SI' WHEN ISNULL(Accreditato,0)=1 then 'SI' END AS PulsanteConfermaValida,"
        querySql &= " CASE WHEN ISNULL(Accreditato,0)=0 then 'SI' WHEN ISNULL(Accreditato,0)='-1' then 'SI' WHEN ISNULL(Accreditato,0)=1 then 'SI' END AS PulsanteConfermaNONValida  "
        querySql &= " FROM sistemi s "
        querySql &= " LEFT join  entisistemi es on s.IDSistema=es.IDSistema and IdEnte = " & IdEnte
        querySql &= " WHERE (ALBO ='SCU' OR Albo IS NULL) and s.Codice in('S01','S02','S03','S04','S05') "

        Session("myDataSet") = ClsServer.DataSetGenerico(querySql, Session("conn"))

        dtgSistemi.DataSource = Session("myDataSet")
        dtgSistemi.DataBind()

        If Session("TipoUtente") = "E" Then
            dtgSistemi.Columns(5).Visible = False 'valida
            dtgSistemi.Columns(6).Visible = False 'non valida 
        End If
        If UCase(StatoEnte(IdEnte)) = "REGISTRATO" Then
            cmdConferma.Visible = True
        Else
            cmdConferma.Visible = False
        End If

    End Sub

    Protected Sub cmdConferma_Click(sender As Object, e As EventArgs) Handles cmdConferma.Click
        Dim item As DataGridItem
        Dim i As Integer
        Dim myCommand As System.Data.SqlClient.SqlCommand
        Dim strSql As String

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            strSql = "Delete entisistemi where IdEnte =" & Session("IdEnte")
            myCommand.CommandText = strSql
            myCommand.ExecuteNonQuery()

            For Each item In dtgSistemi.Items
                Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaSistema"), CheckBox)
                'controllo se è selezionato il check
                If check.Enabled = True Then
                    strSql = "Insert into entisistemi (idente,idsistema,Username,datacreazioneRecord) Values " &
                           "(" & Trim(Session("IdEnte")) & "," & item.Cells(1).Text & ",'" & Session("Utente") & "',getdate()) "
                    myCommand.CommandText = strSql
                    myCommand.ExecuteNonQuery()

                End If
            Next
            lblMessaggio.Text = "Aggiornamento effettuato con successo"
        Catch ex As Exception
            lblMessaggio.Text = "Errore imprevisto. Contattare l'assistenza."
            Log.Error(LogEvent.STRUTTURA_ORGANIZZATIVA_ERRORE, "Errore nella cancellazione", exception:=ex)
        End Try

    End Sub
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Sub CaricaSistemiEnte(ByVal IdEnte As Integer)
        Dim dtrSistemi As SqlDataReader
        'variabile stringa locale per costruire la query per le aree
        Dim strSql As String
        Dim item As DataGridItem
        Dim i As Integer
        ChiudiDataReader(dtrSistemi)
        strSql = "select IDSistema from entisistemi WHERE IdEnte = " & IdEnte
        dtrSistemi = ClsServer.CreaDatareader(strSql, Session("conn"))
        While dtrSistemi.Read()
            For Each item In dtgSistemi.Items
                Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaSistema"), CheckBox)
                If dtrSistemi("IDSistema") = item.Cells(1).Text() Then
                    check.Checked = True
                End If
            Next
        End While
        ChiudiDataReader(dtrSistemi)
    End Sub

    Private Function ConfermaSistema(ByVal Utente As String, ByVal IdEnte As Integer, ByVal Stato As Integer, ByVal IdSistema As Integer, Optional ByRef msg As String = "", Optional ByRef Esito As String = "") As String
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  24/07/2017
        'FUNZIONALITA': RICHIAMO STORE PER LA VALIDITA' DEL SISITEMA
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_VALIDA_SISTEMI]"


        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, Session("conn"))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Utente
            sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            sqlCMD.Parameters.Add("@Stato", SqlDbType.Int).Value = Stato
            sqlCMD.Parameters.Add("@IdSistema", SqlDbType.Int).Value = IdSistema

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            Dim str As String
            msg = sqlCMD.Parameters("@Messaggio").Value
            Esito = sqlCMD.Parameters("@Esito").Value
            'Return str

        Catch ex As Exception

            Exit Function
        End Try
    End Function

    Private Sub dtgSistemi_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgSistemi.ItemCommand

        Dim msg As String
        Dim Esito As String
        Select Case e.CommandName
            Case "Valida"
                ConfermaSistema(Session("Utente"), Session("IdEnte"), 1, e.Item.Cells(1).Text, msg, Esito)
                If Esito = "NEGATIVO" Then
                    lblMessaggio.Visible = True
                    lblMessaggio.Visible = False
                    lblMessaggio.Text = msg
                Else
                    lblMessaggio.Visible = False
                    lblMessaggio.Visible = True
                    lblMessaggio.Text = msg
                End If
                CaricaGrigliaSistemi(Session("IdEnte"))
            Case "NonValida"
                ConfermaSistema(Session("Utente"), Session("IdEnte"), 2, e.Item.Cells(1).Text, msg, Esito)
                If Esito = "NEGATIVO" Then
                    lblMessaggio.Visible = True
                    lblMessaggio.Visible = False
                    lblMessaggio.Text = msg
                Else
                    lblMessaggio.Visible = False
                    lblMessaggio.Visible = True
                    lblMessaggio.Text = msg
                End If
                CaricaGrigliaSistemi(Session("IdEnte"))
        End Select
    End Sub
End Class