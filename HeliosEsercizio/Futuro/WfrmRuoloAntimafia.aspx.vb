Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports Logger.Data
Imports System.Drawing
Imports System.Text.RegularExpressions.Regex

Public Class WfrmRuoloAntimafia
    Inherits SmartPage

    Private Enum tipoAzione
        Inserimento = 0
        Modifica = 1
        SolaLettura = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOut dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOut dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If

        '--- IMPORTANTE!!! INSERIRE CONTROLLO PERMESSI PER ACCESSO MASCHERA E PER EVENTUALI DATI IN QUERY STRING

        If Session("Denominazione") = "" Then
            lgContornoPagina.InnerText = "Ruolo Antimafia"
        Else
            lgContornoPagina.InnerText = "Ruolo Antimafia Ente - " & Session("Denominazione")
        End If

        If Page.IsPostBack = False Then

            Dim SelRuoloAntimafia As New clsRuoloAntimafia
            SelRuoloAntimafia.CaricaDdlEntiByIdEntePadre(ddlEnti, Session("IdEnte"), Session("Conn"), True)

            If Request.QueryString("FiltroEnte") <> Nothing Then
                ddlEnti.SelectedValue = Request.QueryString("FiltroEnte")
            End If

            Dim SelComune As New clsSelezionaComune
            Dim blnEstero As Boolean = False
            SelComune.CaricaProvinciaNazione(ddlProvinciaNascita, blnEstero, Session("Conn"))
            SelComune.CaricaProvinciaNazione(ddlProvinciaResidenza, blnEstero, Session("Conn"))

            'Solo ruoli per enti privati
            'Dim _privato = IsEntePrivato(ddlEnti.SelectedValue)
            Dim _privato = 1

            SelRuoloAntimafia = New clsRuoloAntimafia
            SelRuoloAntimafia.CaricaDdlRuoliAntimafia(ddlRuoliAntiMafia, _privato, Session("Conn"))

            'Controllo se mi è stato passato un idruoloantimafia valido e, se nel caso, lo uso per decidere
            'se la maschera è in inserimento o modifica mettendolo nel campo nascosto hIdRuoloAntiMafia
            'se il parametro passato non è valido la maschera passerà in inserimento
            If Request.QueryString("IdRuoloAntiMafia") <> Nothing Then
                Dim bid As Boolean
                Dim myid As Int64
                bid = Int64.TryParse(Request.QueryString("IdRuoloAntiMafia"), myid)
                If bid = False Then
                    'controllare e/o indicare eventuali messaggi di errore
                    hIdRuoloAntiMafia.Value = 0
                    Log.Warning(LogEvent.ANTIMAFIA_MODIFICA, "IdRuoloAntiMafia non valido", parameters:=Request.QueryString)
                ElseIf myid <> 0 Then
                    'controllo che sia stato passato un ruolo antimafia afferente al mio ente (mio ente o ente figlio)
                    Dim _ruoloAntimafia As clsRuoloAntimafia = New clsRuoloAntimafia()
                    If _ruoloAntimafia.IsRuoloAntimafiaInEnte(myid, Integer.Parse(Session("IdEnte")), Session("conn")) Then
                        hIdRuoloAntiMafia.Value = myid
                    Else
                        'controllare e/o indicare eventuali messaggi di errore
                        hIdRuoloAntiMafia.Value = 0
                        Log.Warning(LogEvent.ANTIMAFIA_MODIFICA, "IdRuoloAntiMafia non appartenente a ENTE", parameters:=Request.QueryString)
                    End If
                Else
                    hIdRuoloAntiMafia.Value = 0
                End If
            Else
                hIdRuoloAntiMafia.Value = 0
            End If

            'Controlli accesso/abilitazioni
            Dim _info As New clsRuoloAntimafia.InfoAdeguamentoAntimafia(Session("IdEnte"), Session("conn"), False)

            If Not _info.Trovato Then
                'errore nei dati, visualizzo solo un messaggio di errore
                lblMessaggio.Text = "ERRORE NEI DATI, ENTE NON TROVATO"
                divPrincipale.Visible = False
                Exit Sub
            ElseIf Not _info.isEntePrivato And Not _info.isEnteTitolare Then
                'la funzionalità non è abilitata per enti pubblici
                lblMessaggio.Text = "FUNZIONALITA' NON DISPONIBILE PER ENTI PUBBLICI NON TITOLARI"
                divPrincipale.Visible = False
                Exit Sub
            Else
                If _info.isAperto Then
                    AbilitaDisabilitaCampiMaschera(IIf(Integer.Parse(hIdRuoloAntiMafia.Value) = 0, tipoAzione.Inserimento, tipoAzione.Modifica))
                Else
                    AbilitaDisabilitaCampiMaschera(tipoAzione.SolaLettura)
                    lblMessaggio.Text = "ENTE DISABILITATO ALLA MODIFICA DEI DATI"
                End If
            End If
            hIdEnteFaseAntimafia.Value = _info.IdEnteFaseAntimafia

            If Integer.Parse(hIdRuoloAntiMafia.Value) <> 0 Then
                CaricaDatiRuolo(Integer.Parse(hIdRuoloAntiMafia.Value))
            End If

        End If
    End Sub

    Private Sub SvuotaCampi()
        txtcodicefiscale.Text = ""
        txtcognome.Text = ""
        txtnome.Text = ""
        If ddlRuoliAntiMafia.Items.Count > 1 Then ddlRuoliAntiMafia.SelectedIndex = 0
        txtDataNascita.Text = ""
        If ddlProvinciaNascita.Items.Count > 0 Then ddlProvinciaNascita.SelectedIndex = 0
        ChkEsteroNascita.Checked = False
        ddlComuneNascita.Items.Clear()
        If ddlProvinciaResidenza.Items.Count > 0 Then ddlProvinciaResidenza.SelectedIndex = 0
        ChkEsteroResidenza.Checked = False
        ddlComuneResidenza.Items.Clear()
        txtIndirizzo.Text = ""
        txtCivico.Text = ""
        txtCAP.Text = ""
        txtTelefono.Text = ""
        txtPEC.Text = ""
        txtEmail.Text = ""
    End Sub

    Private Sub AbilitaDisabilitaCampiMaschera(ByVal azione As Integer)

        If azione = tipoAzione.Inserimento Then
            cmdElimina.Visible = False
        ElseIf azione = tipoAzione.Modifica Then

        ElseIf azione = tipoAzione.SolaLettura Then
            cmdElimina.Visible = False
            cmdSalva.Visible = False

            ddlEnti.Enabled = False
            txtcodicefiscale.Enabled = False
            txtcodicefiscale.BackColor = Color.Gainsboro
            txtcognome.Enabled = False
            txtcognome.BackColor = Color.Gainsboro
            txtnome.Enabled = False
            txtnome.BackColor = Color.Gainsboro
            ddlRuoliAntiMafia.Enabled = False
            txtDataNascita.Enabled = False
            txtDataNascita.BackColor = Color.Gainsboro
            ddlProvinciaNascita.Enabled = False
            ChkEsteroNascita.Enabled = False
            ddlComuneNascita.Enabled = False
            ddlProvinciaResidenza.Enabled = False
            ChkEsteroResidenza.Enabled = False
            ddlComuneResidenza.Enabled = False
            txtIndirizzo.Enabled = False
            txtIndirizzo.BackColor = Color.Gainsboro
            txtCivico.Enabled = False
            txtCivico.BackColor = Color.Gainsboro
            txtCAP.Enabled = False
            txtCAP.BackColor = Color.Gainsboro
            txtTelefono.Enabled = False
            txtTelefono.BackColor = Color.Gainsboro
            txtPEC.Enabled = False
            txtPEC.BackColor = Color.Gainsboro
            txtEmail.Enabled = False
            txtEmail.BackColor = Color.Gainsboro
        End If

    End Sub

    Private Sub ddlProvinciaNascita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaNascita.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        SelComune.CaricaComuniNascita(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
    End Sub

    Private Sub ddlProvinciaResidenza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaResidenza.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        SelComune.CaricaComuni(ddlComuneResidenza, ddlProvinciaResidenza.SelectedValue, Session("Conn"))
    End Sub

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager.ConnectionStrings("unscproduzionenewConnectionString").ConnectionString

        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = " Select Top 30 CAP_INDIRIZZI.Indirizzo as CityName FROM  CAP_INDIRIZZI WHERE (CAP_INDIRIZZI.Indirizzo LIKE '%" + prefixText.Replace("'", "''") + "%') and idcomune=" & contextKey & "  ORDER BY CAP_INDIRIZZI.Indirizzo"
        cmd.Connection = conn
        conn.Open()

        Dim oReader As SqlDataReader = cmd.ExecuteReader
        Dim indirizzi As List(Of String) = New List(Of String)

        While oReader.Read
            indirizzi.Add(oReader.GetString(0))
        End While


        If Not oReader Is Nothing Then
            oReader.Close()
            oReader = Nothing
            conn.Close()
        End If

        Return indirizzi


    End Function

    Private Sub imgCap_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCap.Click

        Dim selCap As New clsSelezionaComune

        txtCAP.Text = selCap.RitornaCap(ddlComuneResidenza.SelectedValue, txtIndirizzo.Text.Trim, txtCivico.Text.Trim, Session("conn"))

    End Sub

    Private Function IsEntePrivato() As Integer
        'La funzione restituisce 1 se ente privato, 0 se ente pubblico, -1 negli altri casi
        Dim dtrTipologia As SqlClient.SqlDataReader
        Dim strSql As String
        Dim _ret As Integer = -1

        strSql = "Select t.privato from enti e inner join TipologieEnti t on e.tipologia =t.descrizione  where idente =" & Session("IdEnte")
        dtrTipologia = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If dtrTipologia.HasRows = True Then
            dtrTipologia.Read()
            'If dtrTipologia("Tipologia") = "Privato" Then
            If dtrTipologia("privato") = True Then
                _ret = 1
            ElseIf dtrTipologia("privato") = False Then
                _ret = 0
            End If
        End If
        dtrTipologia.Close()
        dtrTipologia = Nothing

        Return _ret

    End Function

    Private Function VerificaDatiInseriti() As Boolean

        Dim _datiPerCfOK As Boolean = True

        lblMessaggio.Text = ""
        lblMessaggio.CssClass = "msgErrore"

        'metto in uppercase CF, COGNOME, e NOME
        txtcognome.Text = txtcognome.Text.ToUpper
        txtnome.Text = txtnome.Text.ToUpper
        txtcodicefiscale.Text = txtcodicefiscale.Text.ToUpper

        If ddlEnti.Text.Trim = "" Or ddlEnti.Text.Trim() = "0" Then
            lblMessaggio.Text += "Ente obbligatorio" & "</br>"
        End If

        'verifico che i Ruoli AntiMafia siano caricati
        If ddlRuoliAntiMafia.Items.Count = 0 Then
            lblMessaggio.Text += "Impossibile determinare Tipologia Ente, elenco Ruoli Antimafia non caricato" & "</br>"
        End If

        If txtcodicefiscale.Text.Trim() = "" Then
            lblMessaggio.Text += "Codice Fiscale obbligatorio" & "</br>"
            _datiPerCfOK = False
        End If

        If ddlRuoliAntiMafia.Text.Trim() = "" Or ddlRuoliAntiMafia.Text.Trim() = "0" Then
            lblMessaggio.Text += "Ruolo Antimafia obbligatorio" & "</br>"
        End If

        If txtcognome.Text.Trim = "" Then
            lblMessaggio.Text += "Cognome obbligatorio" & "</br>"
            _datiPerCfOK = False
        End If

        If txtnome.Text.Trim = "" Then
            lblMessaggio.Text += "Nome obbligatorio" & "</br>"
            _datiPerCfOK = False
        End If

        If txtDataNascita.Text.Trim() = "" Then
            lblMessaggio.Text += "Data di Nascita obbligatoria" & "</br>"
            _datiPerCfOK = False
        Else
            Dim dataNascita As Date
            Dim data As String = txtDataNascita.Text.Trim()
            If Len(data) <> 10 And data <> "" Then 'il controllo sull'obbligatorietà è stato fatto sopra
                lblMessaggio.Text += "Data di nascita non valida. Inserire la data nel formato GG/MM/AAAA" & "</br>"
                _datiPerCfOK = False
            ElseIf (Date.TryParse(data, dataNascita) = False) Then
                lblMessaggio.Text += "Data di nascita non valida. Inserire la data nel formato GG/MM/AAAA" & "</br>"
                _datiPerCfOK = False
            End If
        End If

        If ddlProvinciaNascita.Text.Trim() = "" Or ddlProvinciaNascita.Text.Trim() = "0" Then
            lblMessaggio.Text += "Provincia di Nascita obbligatoria" & "</br>"
            _datiPerCfOK = False
        End If

        If ddlComuneNascita.Text.Trim() = "" Or ddlComuneNascita.Text.Trim() = "0" Then
            lblMessaggio.Text += "Comune di Nascita obbligatorio" & "</br>"
            _datiPerCfOK = False
        End If

        Dim regex As Regex

        'verifica correttezza codice fiscale: il sesso non fa parte dei campi quindi lo calcolo sia maschile che femminile
        If _datiPerCfOK Then 'il controllo sull'obbligatorietà è stato fatto sopra
            If txtcodicefiscale.Text.Length <> 16 Then
                lblMessaggio.Text += "Codice Fiscale non corretto" & "</br>"
                _datiPerCfOK = False
            Else
                regex = New Regex("^[a-zA-Z0-9]*$")

                If regex.Match(txtcodicefiscale.Text).Success = False Then
                    lblMessaggio.Text += "Il campo Codice Fiscale contiene caratteri non validi" & "</br>"
                    _datiPerCfOK = False
                Else
                    Dim strCodCatasto As String
                    Dim strCF_M As String
                    Dim strCF_F As String

                    strCodCatasto = ClsUtility.GetCodiceCatasto(Session("conn"), ddlComuneNascita.SelectedValue)
                    strCF_M = ClsUtility.CreaCF(Trim(Replace(txtcognome.Text, "'", "''")), Trim(Replace(txtnome.Text, "'", "''")), Trim(txtDataNascita.Text), strCodCatasto, "M")
                    strCF_F = ClsUtility.CreaCF(Trim(Replace(txtcognome.Text, "'", "''")), Trim(Replace(txtnome.Text, "'", "''")), Trim(txtDataNascita.Text), strCodCatasto, "F")

                    If txtcodicefiscale.Text.Trim() <> strCF_M And txtcodicefiscale.Text.Trim() <> strCF_F Then
                        'Verifico Omocodia
                        If ClsUtility.VerificaOmocodia(UCase(strCF_M), UCase(Trim(txtcodicefiscale.Text))) Or ClsUtility.VerificaOmocodia(UCase(strCF_F), UCase(Trim(txtcodicefiscale.Text))) Then
                            _datiPerCfOK = True
                        Else
                            lblMessaggio.Text += "Codice Fiscale non corretto" & "</br>"
                            _datiPerCfOK = False
                        End If
                    End If
                End If

            End If
        End If
        '-- fine correttezza codice fiscale

        Dim _datiPerCapOK As Boolean = True
        If ddlProvinciaResidenza.Text.Trim() = "" Or ddlProvinciaResidenza.Text.Trim() = "0" Then
            lblMessaggio.Text += "Provincia di Residenza obbligatoria" & "</br>"
            _datiPerCapOK = False
        End If

        If ddlComuneResidenza.SelectedValue.Trim() = "" Or ddlComuneResidenza.SelectedValue.Trim() = "0" Then
            lblMessaggio.Text += "Comune di Residenza obbligatorio" & "</br>"
            _datiPerCapOK = False
        End If

        If txtIndirizzo.Text.Trim() = "" Then
            lblMessaggio.Text += "Indirizzo di Residenza obbligatorio" & "</br>"
            _datiPerCapOK = False
        End If

        If txtCivico.Text.Trim() = "" Then
            lblMessaggio.Text += "Numero civico obbligatorio" & "</br>"
            _datiPerCapOK = False
        End If

        ' Controllo CAP
        If txtCAP.Text.Trim() = "" Then
            lblMessaggio.Text += "C.A.P. obbligatorio" & "</br>"
            _datiPerCapOK = False
        Else
            Dim cap As Int64
            Dim capInteger As Boolean
            capInteger = Int64.TryParse(txtCAP.Text.Trim(), cap)
            If capInteger = False Then
                lblMessaggio.Text += "CAP può contenere solo numeri" & "</br>"
                _datiPerCapOK = False
            End If
        End If


        Dim strMiaCausale As String = ""
        Dim bandiera As Boolean
        If _datiPerCapOK Then
            If ClsUtility.CAP_VERIFICA(Session("conn"), strMiaCausale, bandiera, Trim(txtCAP.Text), ddlComuneResidenza.SelectedItem.Value.ToString, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
                'Inserisco il Messaggio di Errore
                lblMessaggio.Text += strMiaCausale & "</br>"
                _datiPerCapOK = False
            End If
        End If
        '--- Fine controllo CAP

        If txtTelefono.Text.Trim() = "" Then
            'lblMessaggio.Text += "Telefono obbligatorio" & "</br>" 'rimossa obbligatorietà su richiesta dipartimento del 22/10/2021
        ElseIf txtTelefono.Text.Length > 11 Then
            lblMessaggio.Text += "Telefono può contenere massimo 11 caratteri numerici" & "</br>"
        Else
            Dim telefono As Int64
            Dim telefonoInteger As Boolean
            telefonoInteger = Int64.TryParse(txtTelefono.Text.Trim, telefono)
            If telefonoInteger = False Then
                lblMessaggio.Text += "Telefono può contenere solo numeri" & "</br>"
            End If
        End If

        regex = New Regex("^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")

        If txtPEC.Text.Trim() <> "" Then
            Dim match As Match = regex.Match(txtPEC.Text.Trim)
            If match.Success = False Then
                lblMessaggio.Text += "P.E.C. non valida" & "</br>"
            End If

        End If

        If txtEmail.Text.Trim() = "" Then
            ' lblMessaggio.Text += "Email obbligatoria" & "</br>" 'rimossa obbligatorietà su richiesta dipartimento del 22/10/2021
        Else
            Dim match As Match = regex.Match(txtEmail.Text.Trim)
            If match.Success = False Then
                lblMessaggio.Text += "Email non valida" & "</br>"
            End If
        End If

        'tolgo ultimo a capo se esiste
        If lblMessaggio.Text.Length > 5 Then
            If lblMessaggio.Text.Substring(lblMessaggio.Text.Length - 5) = "</br>" Then
                lblMessaggio.Text = lblMessaggio.Text.Substring(0, lblMessaggio.Text.Length - 5)
            End If
        End If
        If lblMessaggio.Text = "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If VerificaDatiInseriti() Then

            Dim _ruoloAntimafia As clsRuoloAntimafia = New clsRuoloAntimafia()
            Dim _newid As Integer = Integer.Parse(hIdRuoloAntiMafia.Value)
            Dim _res As String
            Try
                _res = _ruoloAntimafia.SalvaRuoloAntimafia(
                        _newid,
                        Integer.Parse(ddlEnti.SelectedValue),
                        txtcodicefiscale.Text.Trim(),
                        txtcognome.Text.Trim(),
                        txtnome.Text.Trim(),
                        Integer.Parse(ddlRuoliAntiMafia.SelectedValue),
                        Date.Parse(txtDataNascita.Text.Trim()),
                        Integer.Parse(ddlComuneNascita.SelectedValue),
                        Integer.Parse(ddlComuneResidenza.SelectedValue),
                        txtIndirizzo.Text.Trim(),
                        txtCivico.Text.Trim(),
                        txtCAP.Text.Trim(),
                        txtTelefono.Text.Trim(),
                        txtPEC.Text.Trim(),
                        txtEmail.Text.Trim(),
                        hIdEnteFaseAntimafia.Value,
                        Session("conn"))
            Catch ex As Exception
                Log.Error(IIf(_newid = 0, LogEvent.ANTIMAFIA_ERRORE_INSERIMENTO, LogEvent.ANTIMAFIA_ERRORE_MODIFICA), "Errore nel salvataggio", exception:=ex)
                lblMessaggio.Text = ex.Message
                Exit Sub
            End Try
            If _res <> "" Then
                Log.Warning(IIf(_newid = 0, LogEvent.ANTIMAFIA_INSERIMENTO, LogEvent.ANTIMAFIA_MODIFICA), _res, parameters:="IdRuoloAntimafia=" & _newid & ", IdEnte=" & Session("IdEnte") & ", CodiceFiscale=" & txtcodicefiscale.Text.Trim & ", IdElencoRuoliAntimafia=" & ddlRuoliAntiMafia.SelectedValue)
                lblMessaggio.Text = _res
            Else
                lblMessaggio.CssClass = "msgInfo"
                lblMessaggio.Text = "Operazione effettuata con successo"
                If Integer.Parse(hIdRuoloAntiMafia.Value) = 0 Then  'Se era in inserimento rimane in inserimento
                    SvuotaCampi()
                End If
            End If
        End If
    End Sub

    Protected Sub ChkEsteroNascita_CheckedChanged(sender As Object, e As EventArgs) Handles ChkEsteroNascita.CheckedChanged
        Dim SelComune As New clsSelezionaComune
        Dim blnEstero As Boolean = ChkEsteroNascita.Checked
        SelComune.CaricaProvinciaNazione(ddlProvinciaNascita, blnEstero, Session("Conn"))
        ddlComuneNascita.Items.Clear()
    End Sub

    Protected Sub ChkEsteroResidenza_CheckedChanged(sender As Object, e As EventArgs) Handles ChkEsteroResidenza.CheckedChanged
        Dim SelComune As New clsSelezionaComune
        Dim blnEstero As Boolean = ChkEsteroResidenza.Checked
        SelComune.CaricaProvinciaNazione(ddlProvinciaResidenza, blnEstero, Session("Conn"))
        ddlComuneResidenza.Items.Clear()
    End Sub

    Sub CaricaDatiRuolo(ByVal idRuoloAntiMafia As Integer)
        Dim _clsR As New clsRuoloAntimafia
        Dim _tRuolo As DataSet = _clsR.GetRuoloAntimafia(idRuoloAntiMafia, Session("conn"))

        If _tRuolo.Tables.Count = 1 AndAlso _tRuolo.Tables(0).Rows.Count = 1 Then
            Dim _ruolo As DataRow = _tRuolo.Tables(0).Rows(0)

            ddlEnti.SelectedValue = _ruolo("IdEnte")
            txtcodicefiscale.Text = _ruolo("CodiceFiscale").ToString
            txtcognome.Text = _ruolo("Cognome").ToString
            txtnome.Text = _ruolo("Nome").ToString
            ddlRuoliAntiMafia.SelectedValue = _ruolo("IdElencoRuoliAntimafia")
            txtDataNascita.Text = Date.Parse(_ruolo("DataNascita")).ToString("dd/MM/yyyy")

            Dim SelComune As New clsSelezionaComune
            If _ruolo("ComuneNazionaleNascita") = 0 Then
                ChkEsteroNascita.Checked = True
                SelComune.CaricaProvinciaNazione(ddlProvinciaNascita, ChkEsteroNascita.Checked, Session("Conn"))
            End If
            ddlProvinciaNascita.SelectedValue = _ruolo("IdProvinciaNascita")
            SelComune.CaricaComuniNascita(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
            ddlComuneNascita.SelectedValue = _ruolo("IdComuneNascita")

            If _ruolo("ComuneNazionaleResidenza") = 0 Then
                ChkEsteroResidenza.Checked = True
                SelComune.CaricaProvinciaNazione(ddlProvinciaResidenza, ChkEsteroNascita.Checked, Session("Conn"))
            End If
            ddlProvinciaResidenza.SelectedValue = _ruolo("IdProvinciaResidenza")
            SelComune.CaricaComuniNascita(ddlComuneResidenza, ddlProvinciaResidenza.SelectedValue, Session("Conn"))
            ddlComuneResidenza.SelectedValue = _ruolo("IdComuneResidenza")

            txtIndirizzo.Text = _ruolo("IndirizzoResidenza").ToString
            txtCivico.Text = _ruolo("NumeroCivicoResidenza").ToString
            txtCAP.Text = _ruolo("CAPResidenza").ToString
            txtTelefono.Text = _ruolo("Telefono").ToString
            txtPEC.Text = _ruolo("PEC").ToString
            txtEmail.Text = _ruolo("Email").ToString
        Else
            'errore nel recupero dei dati del ruolo
        End If
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        Response.Redirect("WfrmGestioneRuoliAntimafia.aspx?FiltroEnte=" & ddlEnti.SelectedValue)
    End Sub

    Protected Sub cmdElimina_Click(sender As Object, e As EventArgs) Handles cmdElimina.Click
        Dim _ruoloAntimafia As clsRuoloAntimafia = New clsRuoloAntimafia()
        Dim _ret As String
        Try
            _ret = _ruoloAntimafia.EliminaRuoloAntimafia(Integer.Parse(hIdRuoloAntiMafia.Value), hIdEnteFaseAntimafia.Value, Session("conn"))
        Catch ex As Exception
            Log.Error(LogEvent.ANTIMAFIA_ERRORE_ELIMINAZIONE, "Errore", exception:=ex)
            lblMessaggio.CssClass = "msgErrore"
            lblMessaggio.Text = ex.Message
            Exit Sub
        End Try
        If _ret = "" Then
            Response.Redirect("WfrmGestioneRuoliAntimafia.aspx?Messaggio=" & "Eliminazione effettuata con successo&FiltroEnte=" & ddlEnti.SelectedValue)
        Else
            lblMessaggio.CssClass = "msgErrore"
            lblMessaggio.Text = _ret
        End If

    End Sub

    Private Function IsEntePrivato(ByVal idEnte As Integer) As Integer
        'La funzione restituisce 1 se ente privato, 0 se ente pubblico, -1 negli altri casi o errore
        Dim dtrTipologia As SqlClient.SqlDataReader
        Dim strSql As String
        Dim _ret As Integer = -1

        Try
            strSql = "Select t.privato from enti e inner join TipologieEnti t on e.tipologia =t.descrizione  where idente =" & idEnte
            dtrTipologia = ClsServer.CreaDatareader(strSql, Session("Conn"))
            If dtrTipologia.HasRows = True Then
                dtrTipologia.Read()
                'If dtrTipologia("Tipologia") = "Privato" Then
                If dtrTipologia("privato") = True Then
                    _ret = 1
                ElseIf dtrTipologia("privato") = False Then
                    _ret = 0
                End If
            End If
            dtrTipologia.Close()
            dtrTipologia = Nothing

        Catch ex As Exception
            'viene restituito -1 in automatico
        End Try

        Return _ret

    End Function

    Private Sub ddlEnti_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlEnti.SelectedIndexChanged
        Dim _privato = IsEntePrivato(ddlEnti.SelectedValue)

        Dim SelRuoloAntimafia = New clsRuoloAntimafia
        SelRuoloAntimafia.CaricaDdlRuoliAntimafia(ddlRuoliAntiMafia, _privato, Session("Conn"))
    End Sub
End Class