Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Collections.Generic
Public Class wfrmAnagraficaEnteAccordoNoAdeguamento
    Inherits SmartPage
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim AlboEnte As String
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Not IsPostBack Then
            PopolaMaschera()
            AbilitaMaschera(False)
            ddlComune.Enabled = False
            ddlProvincia.Enabled = False
            ChkEstero.Enabled = False
        Else
            lblMessaggio.Text = String.Empty
        End If
    End Sub

    Private Sub PopolaMaschera()

        Dim dtEnte As New DataTable
        Dim dtSede As New DataTable
        Dim cmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim isItaliano As Boolean

        Try
            strsql = "SELECT E.DENOMINAZIONE," &
                " E.PREFISSOTELEFONORICHIESTAREGISTRAZIONE," &
                " E.TELEFONORICHIESTAREGISTRAZIONE," &
                " E.EMAIL,E.EMAILCERTIFICATA,E.HTTP [HTTP]" &
                " FROM ENTI E" &
                " WHERE E.IDENTE = " & Session("IdEnteAccoglienza")
       
            cmd.Connection = Session("Conn")
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strsql
            da.SelectCommand = cmd
            da.Fill(dtEnte)

        Catch ex As Exception
            lblMessaggio.Text = ex.Message
        End Try

        If Not IsDBNull(dtEnte.Rows(0)("DENOMINAZIONE")) Then
            txtdenominazione.Text = dtEnte.Rows(0)("DENOMINAZIONE")
        End If

        If Not IsDBNull(dtEnte.Rows(0)("EMAIL")) Then
            txtemail.Text = dtEnte.Rows(0)("EMAIL")
        End If

        If Not IsDBNull(dtEnte.Rows(0)("EMAILCERTIFICATA")) Then
            txtEmailpec.Text = dtEnte.Rows(0)("EMAILCERTIFICATA")
        End If

        If Not IsDBNull(dtEnte.Rows(0)("PREFISSOTELEFONORICHIESTAREGISTRAZIONE")) Then
            txtprefisso.Text = dtEnte.Rows(0)("PREFISSOTELEFONORICHIESTAREGISTRAZIONE")
        End If

        If Not IsDBNull(dtEnte.Rows(0)("TELEFONORICHIESTAREGISTRAZIONE")) Then
            txtTelefono.Text = dtEnte.Rows(0)("TELEFONORICHIESTAREGISTRAZIONE")
        End If

        If Not IsDBNull(dtEnte.Rows(0)("HTTP")) Then
            txthttp.Text = dtEnte.Rows(0)("HTTP")
        End If


        Try

            strsql = "SELECT  entisedi.identesede,entisedi.idcomune,entisedi.indirizzo,entisedi.DettaglioRecapito, entisedi.civico, entisedi.cap, comuni.denominazione as comune," &
                     " provincie.provincia, provincie.idProvincia, provincie.ProvinceNazionali,comuni.ComuneNazionale " &
                     " FROM entisedi " &
                     " INNER JOIN statientisedi on statientisedi.idstatoentesede=entisedi.idstatoentesede " &
                     " INNER JOIN entiseditipi ON entisedi.IDEnteSede = entiseditipi.IDEnteSede " &
                     " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
                     " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " &
                     " WHERE entisedi.IDEnte = " & Session("IdEnteAccoglienza") & " And entiseditipi.idtiposede = 1 and (statientisedi.attiva=1 or statientisedi.DefaultStato=1)"


            cmd = New SqlCommand
            cmd.Connection = Session("Conn")
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strsql
            da.SelectCommand = cmd
            da.Fill(dtSede)


            If Not IsDBNull(dtSede.Rows(0)("ComuneNazionale")) Then
                isItaliano = CBool(dtSede.Rows(0)("ComuneNazionale"))
            End If

            HFIdSede.Value = dtSede.Rows(0)("identesede")

            If Not IsDBNull(dtSede.Rows(0)("indirizzo")) Then
                txtIndirizzo.Text = dtSede.Rows(0)("indirizzo")
            End If

            If Not IsDBNull(dtSede.Rows(0)("civico")) Then
                txtCivico.Text = dtSede.Rows(0)("civico")
            End If

            If Not IsDBNull(dtSede.Rows(0)("cap")) Then
                txtCAP.Text = dtSede.Rows(0)("cap")
            End If

            If Not IsDBNull(dtSede.Rows(0)("DettaglioRecapito")) Then
                TxtDettaglioRecapito.Text = dtSede.Rows(0)("DettaglioRecapito")
            End If


            CaricaProvince(isItaliano)

            If Not IsDBNull(dtSede.Rows(0)("idProvincia")) Then
                ddlProvincia.SelectedValue = dtSede.Rows(0)("idProvincia").ToString()
                CaricaComuni(dtSede.Rows(0)("idProvincia").ToString())
            End If

            ddlComune.SelectedValue = dtSede.Rows(0)("idComune").ToString()

        Catch ex As Exception
            lblMessaggio.Text = ex.Message
        End Try

    End Sub

    Private Sub AbilitaMaschera(ByVal abilita As Boolean)
        txtdenominazione.Enabled = abilita
        txtemail.Enabled = abilita
        txtEmailpec.Enabled = abilita
        txtprefisso.Enabled = abilita
        txtTelefono.Enabled = abilita
        txthttp.Enabled = abilita
        txtIndirizzo.Enabled = abilita
        txtCivico.Enabled = abilita
        txtCAP.Enabled = abilita
        TxtDettaglioRecapito.Enabled = abilita
        CmdSalva.Visible = abilita
        CmdModifica.Visible = Not abilita
        CmdAnnulla.Visible = abilita
        CmdChiudi.Visible = Not abilita
        ddlComune.Enabled = abilita
        ddlProvincia.Enabled = abilita
        ChkEstero.Enabled = abilita
        imgCap.Enabled = abilita

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Dim str = "Mod"
        Response.Redirect("WfrmAnagraficaEnteAccordo.aspx?azione=" & str & "&id=" & Session("IdEnteAccoglienza") & "&identerelazione=" & Session("idEnteRelazione"))
    End Sub

    Protected Sub CmdModifica_Click(sender As Object, e As EventArgs) Handles CmdModifica.Click
        AbilitaMaschera(True)
    End Sub

    Protected Sub CmdSalva_Click(sender As Object, e As EventArgs) Handles CmdSalva.Click
        If ValidazioneServerSalva() = True Then
            SalvaDatiEnteNoAdeguamento()
            AbilitaMaschera(False)
        End If

    End Sub

    Protected Sub CmdAnnulla_Click(sender As Object, e As EventArgs) Handles CmdAnnulla.Click
        PulisciCampi()
        PopolaMaschera()
        AbilitaMaschera(False)
    End Sub

    Private Sub PulisciCampi()
        txtdenominazione.Text = String.Empty
        txtemail.Text = String.Empty
        txtEmailpec.Text = String.Empty
        txtprefisso.Text = String.Empty
        txtTelefono.Text = String.Empty
        txthttp.Text = String.Empty
        txtIndirizzo.Text = String.Empty
        txtCivico.Text = String.Empty
        txtCAP.Text = String.Empty
        TxtDettaglioRecapito.Text = String.Empty
    End Sub

    Private Sub CaricaProvince(isItaliano As Boolean)
        Dim sel As New clsSelezionaComune
        If isItaliano Then
            ChkEstero.Checked = False
        Else
            ChkEstero.Checked = True
        End If
        sel.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))

    End Sub

    Private Sub CaricaComuni(ByVal idProvincia As String)
        Dim sel As New clsSelezionaComune
        sel.CaricaComuni(ddlComune, idProvincia, Session("Conn"))
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub


    Private Sub ddlProvincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvincia.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune

        SelComune.CaricaComuni(ddlComune, ddlProvincia.SelectedValue, Session("Conn"))

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager.ConnectionStrings("unscproduzionenewConnectionString").ConnectionString

        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = " Select Top 30 CAP_INDIRIZZI.Indirizzo as CityName FROM  CAP_INDIRIZZI WHERE (CAP_INDIRIZZI.Indirizzo LIKE '%" + prefixText.Replace("'", "''") + "%') and idcomune='" & contextKey & "'  ORDER BY CAP_INDIRIZZI.Indirizzo"
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

    Private Function ValidazioneServerSalva() As Boolean
        Dim regex As Regex = New Regex("^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")
        Dim matchPEC As Match = regex.Match(txtEmailpec.Text.Trim)
        Dim result As Boolean = True

        lblMessaggio.Text = String.Empty

        If txtdenominazione.Text.Trim() = String.Empty Then
            lblMessaggio.Text = lblMessaggio.Text & "La denominazione dell'Ente è obbligatoria."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        If txtprefisso.Text.Trim = String.Empty Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire il prefisso Telefonico."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        Dim prefissoTelefono As Integer
        Dim prefissoTelefonoInteger As Boolean
        prefissoTelefonoInteger = Integer.TryParse(txtprefisso.Text.Trim, prefissoTelefono)

        If prefissoTelefonoInteger = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Il Prefisso Telefono puo' contenere solo numeri."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        If txtTelefono.Text.Trim = String.Empty Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire il numero Telefonico."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        Dim numeroTelefono As Int64
        Dim numeroTelefonoInteger As Boolean
        numeroTelefonoInteger = Int64.TryParse(txtTelefono.Text.Trim, numeroTelefono)

        If numeroTelefonoInteger = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Il Telefono puo' contenere solo numeri."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        If txtemail.Text.Trim = String.Empty Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire il campo Email."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        Dim matchEmail As Match = regex.Match(txtemail.Text.Trim)
        If matchEmail.Success = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Il campo 'Email' non e' nel formato valido."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        If txtEmailpec.Text.Trim <> String.Empty AndAlso matchPEC.Success = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Il campo 'Email PEC' non e' nel formato valido."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        If txtIndirizzo.Text.Trim = String.Empty Then

            lblMessaggio.Text = lblMessaggio.Text & "Inserire l'indirizzo della sede Legale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        If txtCivico.Text.Trim = String.Empty Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire il numero civico della sede Legale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False

        End If

        If ddlProvincia.SelectedIndex <= 0 Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire la provincia della sede Legale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        If ddlComune.SelectedIndex <= 0 Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire il comune della sede Legale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
        End If


        If ChkEstero.Checked = False Then
            If txtCAP.Text.Trim = String.Empty Then
                lblMessaggio.Text = lblMessaggio.Text & "Inserire il CAP della sede Legale."
                lblMessaggio.Text = lblMessaggio.Text & " </br>"
                result = False
            Else
                Dim selCap As New clsSelezionaComune
                If ddlComune.SelectedIndex > 0 AndAlso _
                txtIndirizzo.Text <> String.Empty _
                AndAlso txtCivico.Text.Trim <> String.Empty Then
                    If txtCAP.Text <> selCap.RitornaCap(ddlComune.SelectedValue, txtIndirizzo.Text.Trim, txtCivico.Text.Trim, Session("conn")) Then
                        lblMessaggio.Text = lblMessaggio.Text & "Il CAP della sede Legale non è congruo con il comune selezionato o con l'indirizzo inserito"
                        lblMessaggio.Text = lblMessaggio.Text & " </br>"
                        result = False
                    End If
                End If
            End If
        End If

        If Not result Then
            MessaggiAlert(lblMessaggio.Text)
            Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_VALIDAZIONE_CAMPIINSERIMENTO_CORRETTA, "DATI INSERITI IN MASCHERA NON CORRETTI PER IL SALVATAGGIO")
        End If

        ValidazioneServerSalva = result

    End Function

    Private Sub ChkEstero_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEstero.CheckedChanged
        Dim SelComune As New clsSelezionaComune
        Dim blnEstero As Boolean = ChkEstero.Checked

        ddlComune.Items.Clear()
        SelComune.CaricaProvinciaNazione(ddlProvincia, blnEstero, Session("Conn"))


        If blnEstero = True Then
            lblComune.Text = "Località"
            lblCAP.Text = "Codice località"
        Else
            lblComune.Text = "Comune"
            lblCAP.Text = "CAP"
        End If
    End Sub

    Private Sub imgCap_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCap.Click
        Dim selCap As New clsSelezionaComune
        txtCAP.Text = selCap.RitornaCap(ddlComune.SelectedValue, txtIndirizzo.Text.Trim, txtCivico.Text.Trim, Session("conn"))
    End Sub

    Private Sub MessaggiAlert(ByVal strMessaggio)
        lblMessaggio.ForeColor = Color.Red
        lblMessaggio.Text = strMessaggio
        Exit Sub
    End Sub

    Private Function SalvaDatiEnteNoAdeguamento() As Boolean
        Dim esito As Boolean = True
        Dim LogParamentri As New Hashtable()

        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_MODIFICA_ENTE_SEDE_NO_ADEGUAMENTO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")


            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnteAccoglienza")
            LogParamentri.Add("@IdEnte", Session("IdEnteAccoglienza"))

            SqlCmd.Parameters.Add("@Denominazione", SqlDbType.VarChar).Value = txtdenominazione.Text 'denominazioneente
            LogParamentri.Add("@Denominazione", txtdenominazione.Text)

            SqlCmd.Parameters.Add("@PrefissoTelefonoRichiestaRegistrazione", SqlDbType.VarChar).Value = txtprefisso.Text  'Prefisso telefono
            LogParamentri.Add("@PrefissoTelefonoRichiestaRegistrazione", txtprefisso.Text)

            SqlCmd.Parameters.Add("@TelefonoRichiestaRegistrazione", SqlDbType.VarChar).Value = txtTelefono.Text 'telefono
            LogParamentri.Add("@TelefonoRichiestaRegistrazione", txtTelefono.Text)

            If Trim(txtemail.Text) = "" Then
                SqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = DBNull.Value
                LogParamentri.Add("@Email", String.Empty)
            Else
                SqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = txtemail.Text
                LogParamentri.Add("@Email", txtemail.Text)
            End If
            If Trim(txtEmailpec.Text) = "" Then
                SqlCmd.Parameters.Add("@EmailCertificata", SqlDbType.VarChar).Value = DBNull.Value
                LogParamentri.Add("@EmailCertificata", String.Empty)
            Else
                SqlCmd.Parameters.Add("@EmailCertificata", SqlDbType.VarChar).Value = txtEmailpec.Text
                LogParamentri.Add("@EmailCertificata", txtEmailpec.Text)
            End If

            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")
            LogParamentri.Add("@UsernameRichiesta", Session("Utente"))

            If Trim(txthttp.Text) = "" Then
                SqlCmd.Parameters.Add("@Http", SqlDbType.VarChar).Value = DBNull.Value
                LogParamentri.Add("@Http", String.Empty)
            Else
                SqlCmd.Parameters.Add("@Http", SqlDbType.VarChar).Value = txthttp.Text
                LogParamentri.Add("@Http", txthttp.Text)
            End If

            'SEDE
            If HFIdSede.Value <> "" Then
                SqlCmd.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = HFIdSede.Value ' IdEnteSede
                LogParamentri.Add("@IdEnteSede", HFIdSede.Value)
            Else
                SqlCmd.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = 0 ' IdEnteSede
                LogParamentri.Add("@IdEnteSede", 0)
            End If

            ' End If
            SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = ddlComune.SelectedValue
            LogParamentri.Add("@IdComune", ddlComune.SelectedValue)

            SqlCmd.Parameters.Add("@CAP", SqlDbType.VarChar).Value = txtCAP.Text ' CAP
            LogParamentri.Add("@CAP", txtCAP.Text)

            SqlCmd.Parameters.Add("@Indirizzo", SqlDbType.VarChar).Value = txtIndirizzo.Text ' indirizzo
            LogParamentri.Add("@Indirizzo", txtIndirizzo.Text)

            SqlCmd.Parameters.Add("@Civico", SqlDbType.VarChar).Value = txtCivico.Text ' Civico
            LogParamentri.Add("@Civico", txtCivico.Text)

            SqlCmd.Parameters.Add("@DettaglioRecapito", SqlDbType.VarChar).Value = TxtDettaglioRecapito.Text ' DettaglioRecapito
            LogParamentri.Add("@DettaglioRecapito", TxtDettaglioRecapito.Text)

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            If SqlCmd.Parameters("@Esito").Value = 0 Then
                MessaggiAlert(SqlCmd.Parameters("@messaggio").Value())
                Log.Information(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_MODIFICA_ERRATA, SqlCmd.Parameters("@messaggio").Value, LogParamentri)
                esito = False
            Else
                MessaggiConvalida(SqlCmd.Parameters("@messaggio").Value())
                Log.Information(Logger.Data.LogEvent.ANAGRAFICAENTEACCORDO_MODIFICA_CORRETTA, SqlCmd.Parameters("@messaggio").Value, LogParamentri)
            End If

            SalvaDatiEnteNoAdeguamento = esito

        Catch Sqlex As SqlException
            lblMessaggio.Text = Sqlex.Message
            SalvaDatiEnteNoAdeguamento = False
            'Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTE_MODIFICA_ERRATA, Sqlex.Message)
            Exit Function
        Catch ex As Exception
            lblMessaggio.Text = ex.Message
            SalvaDatiEnteNoAdeguamento = False
            'Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTE_MODIFICA_ERRATA, ex.Message)
            Exit Function
        End Try

    End Function

    Private Sub MessaggiConvalida(ByVal strMessaggio)
        'Realizzata da Alessandra Taballione 26/02/04
        'Private sub per la gestione dell'immagine e del messaggio
        'per eventuali comunicazione all'Utente

        lblMessaggio.ForeColor = Color.Navy
        lblMessaggio.Text = strMessaggio
        Exit Sub
    End Sub


End Class