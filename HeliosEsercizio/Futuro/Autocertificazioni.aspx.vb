Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Collections.Generic
Imports System.IO
Imports System.Security.Cryptography
'Imports System.Text.RegularExpressions.Regex
Imports Logger.Data
Public Class Autocertificazioni
    Inherits SmartPage
    Dim dtsGenerico As DataSet
    Dim dtrGenerico As System.Data.SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
        VerificaSessione()

        If IsPostBack = False Then
            'CaricaEntiFigli()

            Dim SelRuoloAntimafia As New clsRuoloAntimafia
            SelRuoloAntimafia.CaricaDdlEntiByIdEntePadre(ddlEntiFigli, Session("IdEnte"), Session("Conn"), False, True)
            ddlEntiFigli.SelectedValue = 0
            If ddlEntiFigli.Items.Count = 1 Then
                ddlEntiFigli.Visible = False
                lblEntefiglio.Visible = False
                imgOpenFigli.Visible = False
            End If

            PopolaMaschera()

            'Controlli accesso/abilitazioni
            Dim _info As New clsRuoloAntimafia.InfoAdeguamentoAntimafia(Session("IdEnte"), Session("conn"), False)
            If Not _info.Trovato Then
                'errore nei dati, visualizzo solo un messaggio di errore
                DisabilitaMaschera()
                msgErrore.Visible = True
                msgErrore.Text = "Non e' possibile effettuare modifiche ai  dati; fase Adeguamento dati Antimafia non trovata."
                Exit Sub
            ElseIf Not _info.isEntePrivato And Not _info.isEnteTitolare Then
                'la funzionalità non è abilitata per enti pubblici che non sono titolari
                DisabilitaMaschera()
                msgErrore.Visible = True
                msgErrore.Text = "FUNZIONALITA' NON DISPONIBILE PER ENTI PUBBLICI NON TITOLARI."
                Exit Sub
            Else
                If Not _info.isAperto Then
                    DisabilitaMaschera()
                    msgErrore.Visible = True
                    msgErrore.Text = "Non e' possibile effettuare modifiche ai  dati; fase Adeguamento dati Antimafia non avviata."
                    Exit Sub
                End If
            End If
        End If

    End Sub
    Private Function EntePubblico() As Boolean
        Dim pubblico As Boolean = False
        Dim strsql As String
        strsql = "select coalesce(t2.privato,0) privato from enti t1 inner join TipologieEnti t2 on t2.Descrizione=t1.Tipologia where t1.IDEnte=0" & Session("IdEnte")
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        If dtrGenerico.HasRows Then
            pubblico = (dtrGenerico("privato") & "" = "0")
        End If
        dtrGenerico.Close()
        Return pubblico
    End Function
    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        'Dim CampoObbligatorio As String = "il Campo {0} è obbligatorio<br/>"
        Dim conta As Integer = 0
        msgErrore.Text = ""
        If ChkDirettoreTecnico.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo Direttore tecnico è obbligatorio<br/>"
            conta = conta + 1
        End If
        If ChKCondirettoreTecnico.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo condirettore tecnico è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkResponsabileTecnico.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo responsabile tecnico è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkSocioResponsabileTecnico.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo socio responsabile tecnico è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkResponsabilePreposto.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo responsabile preposto è obbligatorio<br/>"
            conta = conta + 1
        End If

        If chkPrepostoGestioneTecnica.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo preposto gestione tecnica è obbligatorio<br/>"
            conta = conta + 1
        End If

        If chkPresidenteCollegio.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo Presidente e componenti del collegio sindacale o sindaco è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkSindaco.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo sindaco è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkSindacoProTempore.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo sindaco pro-tempore è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkSindacoSupplente.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo sindaco supplente è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkSoggettiVigilanza.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo Soggetti con compiti di vigilanza è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkConsigliereSorveglianza.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo consigliere di sorveglianza è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkConsigliereSorveglianzaSupplente.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo consigliere di sorveglianza supplente è obbligatorio<br/>"
            conta = conta + 1
        End If
        If chkPresidenteConsiglioSorveglianza.Checked = True Then
            'msgErrore.Text = msgErrore.Text + "Il campo presidente del consiglio di sorveglianza è obbligatorio<br/>"
            conta = conta + 1
        End If
        If conta > 0 Then
            Dim myQuerySql As String
            Dim Cmd As SqlClient.SqlCommand
            If hdTxtTipoOperazione.Text = "I" Then



                Try
                    myQuerySql = "INSERT INTO EntiAutocertificazioni (IDEnte ,DirettoreTecnico ,CondirettoreTecnico ,ResponsabileTecnico ,SocioResponsabileTecnico "
                    myQuerySql = myQuerySql + ",ResponsabilePreposto ,PrepostoGestioneTecnica ,PresidenteCollegio ,Sindaco ,SindacoProTempore ,SindacoSupplente "
                    myQuerySql = myQuerySql + ",SoggettiVigilanza ,ConsigliereSorveglianza ,ConsigliereSorveglianzaSupplente ,PresidenteConsiglioSorveglianza) "
                    myQuerySql = myQuerySql + "VALUES (" '& Session("IdEnte")
                    If ddlEntiFigli.SelectedValue = 0 Then
                        myQuerySql = myQuerySql + Session("IdEnte")
                    Else
                        myQuerySql = myQuerySql + ddlEntiFigli.SelectedValue
                    End If
                    myQuerySql = myQuerySql + " ," & CInt(ChkDirettoreTecnico.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(ChKCondirettoreTecnico.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkResponsabileTecnico.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkSocioResponsabileTecnico.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkResponsabilePreposto.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkPrepostoGestioneTecnica.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkPresidenteCollegio.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkSindaco.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkSindacoProTempore.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkSindacoSupplente.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkSoggettiVigilanza.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkConsigliereSorveglianza.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkConsigliereSorveglianzaSupplente.Checked)
                    myQuerySql = myQuerySql + " ," & CInt(chkPresidenteConsiglioSorveglianza.Checked) & ")"




                    'myQuerySql = myQuerySql + ",1 "
                    ',1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 ,1 
                    Cmd = New SqlClient.SqlCommand(myQuerySql, Session("conn"))
                    Cmd.ExecuteNonQuery()
                    hdTxtTipoOperazione.Text = "U"
                    msgConferma.Text = "Autocertificazioni Inserite Correttamente"
                    Log.Information(LogEvent.AUTOCERTIFICAZIONI_REGISTRAZIONE_CORRETTA)
                    Dim SelRuoloAntimafia As New clsRuoloAntimafia
                    SelRuoloAntimafia.CaricaDdlEntiByIdEntePadre(ddlEntiFigli, Session("IdEnte"), Session("Conn"), False, True) 'asterischi

                    'cmdSalva.Visible = False
                Catch ex As Exception
                    msgErrore.Visible = True
                    msgErrore.Text = "Errore nell'inserimmento delle Autocertificazioni- Contattare l'assistenza."
                    Log.Information(LogEvent.AUTOCERTIFICAZIONE_REGISTRAZIONE_ERRATA)
                End Try
            Else
                Try
                    myQuerySql = "UPDATE EntiAutocertificazioni "
                    'myQuerySql = myQuerySql + "SET IDEnte = " & Session("IdEnte")
                    myQuerySql = myQuerySql + "SET DirettoreTecnico = " & CInt(ChkDirettoreTecnico.Checked)
                    myQuerySql = myQuerySql + ",CondirettoreTecnico = " & CInt(ChKCondirettoreTecnico.Checked)
                    myQuerySql = myQuerySql + ",ResponsabileTecnico = " & CInt(chkResponsabileTecnico.Checked)
                    myQuerySql = myQuerySql + ",SocioResponsabileTecnico = " & CInt(chkSocioResponsabileTecnico.Checked)
                    myQuerySql = myQuerySql + ",ResponsabilePreposto = " & CInt(chkResponsabilePreposto.Checked)
                    myQuerySql = myQuerySql + ",PrepostoGestioneTecnica = " & CInt(chkPrepostoGestioneTecnica.Checked)
                    myQuerySql = myQuerySql + ",PresidenteCollegio = " & CInt(chkPresidenteCollegio.Checked)
                    myQuerySql = myQuerySql + ",Sindaco = " & CInt(chkSindaco.Checked)
                    myQuerySql = myQuerySql + ",SindacoProTempore = " & CInt(chkSindacoProTempore.Checked)
                    myQuerySql = myQuerySql + ",SindacoSupplente = " & CInt(chkSindacoSupplente.Checked)
                    myQuerySql = myQuerySql + ",SoggettiVigilanza = " & CInt(chkSoggettiVigilanza.Checked)
                    myQuerySql = myQuerySql + ",ConsigliereSorveglianza = " & CInt(chkConsigliereSorveglianza.Checked)
                    myQuerySql = myQuerySql + ",ConsigliereSorveglianzaSupplente = " & CInt(chkConsigliereSorveglianzaSupplente.Checked)
                    myQuerySql = myQuerySql + ",PresidenteConsiglioSorveglianza = " & CInt(chkPresidenteConsiglioSorveglianza.Checked)
                    If ddlEntiFigli.SelectedValue = 0 Then
                        myQuerySql = myQuerySql + " WHERE IDEnte = " & Session("IdEnte")
                    Else
                        myQuerySql = myQuerySql + " WHERE IDEnte = " & ddlEntiFigli.SelectedValue
                    End If
                    Cmd = New SqlClient.SqlCommand(myQuerySql, Session("conn"))
                    Cmd.ExecuteNonQuery()
                    msgConferma.Text = "Autocertificazioni Inserite Correttamente"
                    Log.Information(LogEvent.AUTOCERTIFICAZIONI_REGISTRAZIONE_CORRETTA)
                    Dim SelRuoloAntimafia As New clsRuoloAntimafia
                    SelRuoloAntimafia.CaricaDdlEntiByIdEntePadre(ddlEntiFigli, Session("IdEnte"), Session("Conn"), False)

                    'cmdSalva.Visible = False
                Catch ex As Exception
                    msgErrore.Visible = True
                    msgErrore.Text = "Errore nell'inserimmento delle Autocertificazioni- Contattare l'assistenza."
                    Log.Information(LogEvent.AUTOCERTIFICAZIONE_REGISTRAZIONE_ERRATA, "ID: " & hdTxtIDEntiAutocertificazioni.Text)
                End Try
            End If
        Else
            msgErrore.Text = "E&rsquo; obbligatorio selezionare almeno un ruolo"
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
    Private Sub PopolaMaschera()
        ChiudiDataReader(dtrGenerico)


        If ddlEntiFigli.Items.Count = 0 Then
            DisabilitaMaschera()
            msgErrore.Visible = True
            msgErrore.Text = "Nessun ente interessato."
        Else
            'popolamento della maschera dati DB
            Dim strsql As String
            strsql = "SELECT IDEntiAutocertificazioni ,DirettoreTecnico ,CondirettoreTecnico ,ResponsabileTecnico ,SocioResponsabileTecnico ,ResponsabilePreposto " &
                ",PrepostoGestioneTecnica ,PresidenteCollegio ,Sindaco ,SindacoProTempore ,SindacoSupplente ,SoggettiVigilanza ,ConsigliereSorveglianza " &
                ",ConsigliereSorveglianzaSupplente ,PresidenteConsiglioSorveglianza"
            If ddlEntiFigli.SelectedValue = 0 Then
                strsql += " FROM EntiAutocertificazioni where idente=" & Session("IdEnte")
            Else
                strsql += " FROM EntiAutocertificazioni where idente=" & ddlEntiFigli.SelectedValue
            End If

            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrGenerico.Read()

            If dtrGenerico.HasRows = True Then
                hdTxtTipoOperazione.Text = "U"
                hdTxtIDEntiAutocertificazioni.Text = dtrGenerico("IDEntiAutocertificazioni")
                ChkDirettoreTecnico.Checked = CBool(CInt(dtrGenerico("DirettoreTecnico")))
                ChKCondirettoreTecnico.Checked = CBool(CInt(dtrGenerico("CondirettoreTecnico")))
                chkResponsabileTecnico.Checked = CBool(CInt(dtrGenerico("ResponsabileTecnico")))
                chkSocioResponsabileTecnico.Checked = CBool(CInt(dtrGenerico("SocioResponsabileTecnico")))
                chkResponsabilePreposto.Checked = CBool(CInt(dtrGenerico("ResponsabilePreposto")))
                chkPrepostoGestioneTecnica.Checked = CBool(CInt(dtrGenerico("PrepostoGestioneTecnica")))
                chkPresidenteCollegio.Checked = CBool(CInt(dtrGenerico("PresidenteCollegio")))
                chkSindaco.Checked = CBool(CInt(dtrGenerico("Sindaco")))
                chkSindacoProTempore.Checked = CBool(CInt(dtrGenerico("SindacoProTempore")))
                chkSindacoSupplente.Checked = CBool(CInt(dtrGenerico("SindacoSupplente")))
                chkSoggettiVigilanza.Checked = CBool(CInt(dtrGenerico("SoggettiVigilanza")))
                chkConsigliereSorveglianza.Checked = CBool(CInt(dtrGenerico("ConsigliereSorveglianza")))
                chkConsigliereSorveglianzaSupplente.Checked = CBool(CInt(dtrGenerico("ConsigliereSorveglianzaSupplente")))
                chkPresidenteConsiglioSorveglianza.Checked = CBool(CInt(dtrGenerico("PresidenteConsiglioSorveglianza")))

                'chkPresidenteConsiglioSorveglianza.Enabled = False
                'chkConsigliereSorveglianzaSupplente.Enabled = False
                'chkConsigliereSorveglianza.Enabled = False
                'chkSoggettiVigilanza.Enabled = False
                'chkSindacoSupplente.Enabled = False
                'chkSindacoProTempore.Enabled = False
                'chkSindaco.Enabled = False
                'chkPresidenteCollegio.Enabled = False
                'chkPrepostoGestioneTecnica.Enabled = False
                'chkResponsabilePreposto.Enabled = False
                'chkSocioResponsabileTecnico.Enabled = False
                'chkResponsabileTecnico.Enabled = False
                'ChKCondirettoreTecnico.Enabled = False
                'ChkDirettoreTecnico.Enabled = False


                'cmdSalva.Visible = False

            Else
                hdTxtTipoOperazione.Text = "I"

                ChkDirettoreTecnico.Checked = False
                ChKCondirettoreTecnico.Checked = False
                chkResponsabileTecnico.Checked = False
                chkSocioResponsabileTecnico.Checked = False
                chkResponsabilePreposto.Checked = False
                chkPrepostoGestioneTecnica.Checked = False
                chkPresidenteCollegio.Checked = False
                chkSindaco.Checked = False
                chkSindacoProTempore.Checked = False
                chkSindacoSupplente.Checked = False
                chkSoggettiVigilanza.Checked = False
                chkConsigliereSorveglianza.Checked = False
                chkConsigliereSorveglianzaSupplente.Checked = False
                chkPresidenteConsiglioSorveglianza.Checked = False
            End If
            cmdAnnulla.Visible = True
            cmdSalva.Visible = True
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        End If
        
    End Sub
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        Response.Redirect("DettaglioFunzioni.aspx?IdVoceMenu=2")
    End Sub

    Protected Sub cmdCancella_Click(sender As Object, e As EventArgs) Handles cmdCancella.Click

        Dim myQuerySql As String
        Dim Cmd As SqlClient.SqlCommand
        Try
            If ddlEntiFigli.SelectedValue = 0 Then
                myQuerySql = "Delete From EntiAutocertificazioni where IDEnte = " & Session("IdEnte")
            Else
                myQuerySql = "Delete From EntiAutocertificazioni where IDEnte = " & ddlEntiFigli.SelectedValue
            End If

            Cmd = New SqlClient.SqlCommand(myQuerySql, Session("conn"))
            Cmd.ExecuteNonQuery()
            msgConferma.Text = "Cancellazione Avvenuta Correttamente"
            Log.Information(LogEvent.AUTOCERTIFICAZIONI_REGISTRAZIONE_CORRETTA)
            cmdSalva.Visible = False
            cmdCancella.Visible = False
            PopolaMaschera()
        Catch ex As Exception
            msgErrore.Visible = True
            msgErrore.Text = "Errore nella cancellazione delle Autocertificazioni- Contattare l'assistenza."
            Log.Information(LogEvent.AUTOCERTIFICAZIONE_REGISTRAZIONE_ERRATA, "id: " & hdTxtIDEntiAutocertificazioni.Text)
        End Try
    End Sub
    Sub DisabilitaMaschera()
        ChkDirettoreTecnico.Enabled = False
        ChKCondirettoreTecnico.Enabled = False
        chkResponsabileTecnico.Enabled = False
        chkSocioResponsabileTecnico.Enabled = False
        chkResponsabilePreposto.Enabled = False
        chkPrepostoGestioneTecnica.Enabled = False
        chkPresidenteCollegio.Enabled = False
        chkSindaco.Enabled = False
        chkSindacoProTempore.Enabled = False
        chkSindacoSupplente.Enabled = False
        chkSoggettiVigilanza.Enabled = False
        chkConsigliereSorveglianza.Enabled = False
        chkConsigliereSorveglianzaSupplente.Enabled = False
        chkPresidenteConsiglioSorveglianza.Enabled = False
        cmdCancella.Visible = False
        cmdSalva.Visible = False
    End Sub
    Sub CaricaEntiFigli()
        'codice generato da nahtanoj il 24?03?2005
        Dim strsql As String
        strsql = "select distinct entirelazioni.IdEnteFiglio, enti.Denominazione from entirelazioni "
        strsql = strsql & "inner join enti on enti.IdEnte=entirelazioni.IdEnteFiglio "
        strsql = strsql & "inner Join TipologieEnti on TipologieEnti.Descrizione=enti.Tipologia "
        strsql = strsql & "where entirelazioni.IdEntePadre=" & CInt(Session("IdEnte"))
        strsql = strsql & " and  entirelazioni.datafinevalidità is null and enti.idstatoente in (3,6) and TipologieEnti.privato=1 order by enti.Denominazione"
        ddlEntiFigli.Items.Clear()

        ddlEntiFigli.DataSource = MakeParentTable(strsql)
        ddlEntiFigli.DataTextField = "ParentItem"
        ddlEntiFigli.DataValueField = "id"
        ddlEntiFigli.DataBind()
        If ddlEntiFigli.Items.Count = 1 Then
            ddlEntiFigli.Visible = False
            lblEntefiglio.Visible = False
            imgOpenFigli.Visible = False
        End If
    End Sub
    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
        ' Create a new DataTable.
        Dim myDataTable As DataTable = New DataTable
        ' Declare variables for DataColumn and DataRow objects.
        Dim myDataColumn As DataColumn
        Dim myDataRow As DataRow
        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.Int64")
        myDataColumn.ColumnName = "id"
        myDataColumn.Caption = "id"
        myDataColumn.ReadOnly = True
        myDataColumn.Unique = True
        ' Add the Column to the DataColumnCollection.
        myDataTable.Columns.Add(myDataColumn)
        ' Create second column.
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "ParentItem"
        myDataColumn.AutoIncrement = False
        myDataColumn.Caption = "ParentItem"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False
        myDataTable.Columns.Add(myDataColumn)

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        myDataRow = myDataTable.NewRow()
        myDataRow("id") = 0
        myDataRow("ParentItem") = "Ente Titolare"
        myDataTable.Rows.Add(myDataRow)
        Do While dtrGenerico.Read
            myDataRow = myDataTable.NewRow()
            myDataRow("id") = dtrGenerico.GetValue(0)
            myDataRow("ParentItem") = dtrGenerico.GetValue(1)
            myDataTable.Rows.Add(myDataRow)
        Loop

        dtrGenerico.Close()
        dtrGenerico = Nothing

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Private Sub ddlEntiFigli_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEntiFigli.SelectedIndexChanged
        PopolaMaschera()
    End Sub

    Private Sub imgOpenFigli_Click(sender As Object, e As ImageClickEventArgs) Handles imgOpenFigli.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""WfrmPopUpEntiFigliPrivati.aspx?identita=1"", ""Visualizza"", ""width=670,height=300,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf)
        Response.Write("</script>")
    End Sub
End Class