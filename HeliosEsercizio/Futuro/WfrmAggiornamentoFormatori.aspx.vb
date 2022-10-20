Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Public Class WfrmAggiornamentoFormatori
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            'CARICA ANNO
            Table4.Visible = False
            CaricaAnno(ddlAnnoCorsoFormazione, 1)
        End If
    End Sub


    Private Sub CaricaGriglia()
        Dim strAnno As String
        Dim dtsGenerico As DataSet

        Select Case ddlAnnoCorsoFormazione.SelectedItem.Text
            Case "Selezionare"
                strAnno = "-1"
            Case "Nessuno"
                strAnno = 0
            Case Else
                strAnno = ddlAnnoCorsoFormazione.SelectedItem.Text
        End Select
        dtsGenerico = EseguiStoreReaderRicercaFormatori( _
                                                "SP_FORMAZIONE_RICERCA_FORMATORI", _
                                                txtCodiceEnte.Text, _
                                                txtdenominazioneEnte.Text, _
                                                txtCognome.Text, _
                                                txtNome.Text, _
                                                txtCodiceFisclae.Text, _
                                                ddlCorsoFormazione.SelectedValue, _
                                                strAnno, _
                                                Session("conn"))
        dgRisultatoRicerca.DataSource = dtsGenerico
        dgRisultatoRicerca.DataBind()
        If dgRisultatoRicerca.Items.Count > 0 Then
            Controlli(True)
            CaricaAnno(ddlAggiornaAnnoCorsoFor, 0)
        Else
            Controlli(False)
        End If
        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(7) As String
        Dim NomiCampiColonne(7) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Ente"
        NomeColonne(1) = "Competenza"
        NomeColonne(2) = "Formatore"
        NomeColonne(3) = "Data Nascita"
        NomeColonne(4) = "Comune"
        NomeColonne(5) = "Corso Formazione"
        NomeColonne(6) = "Anno Corso Formazione"

        NomiCampiColonne(0) = "Ente"
        NomiCampiColonne(1) = "Competenza"
        NomiCampiColonne(2) = "Formatore"
        NomiCampiColonne(3) = "DataNascita"
        NomiCampiColonne(4) = "Comune"
        NomiCampiColonne(5) = "CorsoFormazione"
        NomiCampiColonne(6) = "AnnoCorsoAggiornamento"

        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(dtsGenerico, 6, NomeColonne, NomiCampiColonne)
        '*********************************************************************************
    End Sub

    Private Function Controlli(ByVal Valore As Boolean)
        chkSelDesel.Visible = Valore
        LblAnnoCorsoForm.Visible = Valore
        ddlAggiornaAnnoCorsoFor.Visible = Valore
        LblCorsoAggFormazione.Visible = Valore
        ddlAggiornaCorsoFor.Visible = Valore
        imgAssegnaCorso.Visible = Valore
        cmdAssegna.Visible = Valore
        imgStampa.Visible = Valore
    End Function

    Public Shared Function EseguiStoreReaderRicercaFormatori( _
                                ByVal strNomeStore As String, _
                                ByVal CodEnte As String, _
                                ByVal DenominazioneEnte As String, _
                                ByVal Cognome As String, _
                                ByVal Nome As String, _
                                ByVal CodiceFiscale As String, _
                                ByVal Corso As Integer, _
                                ByVal AnnoCorso As String, _
                                ByVal conn As SqlClient.SqlConnection) As DataSet

        Dim MyCommand As New SqlClient.SqlDataAdapter(strNomeStore, conn)

        MyCommand.SelectCommand.CommandType = CommandType.StoredProcedure

        'PRIMO PARAMETRO CodEnte
        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@CodEnte"
        sparam.SqlDbType = SqlDbType.NVarChar
        MyCommand.SelectCommand.Parameters.Add(sparam)

        'SECONDO PARAMETRO DenominazioneEnte
        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@DenominazioneEnte"
        sparam1.SqlDbType = SqlDbType.NVarChar
        MyCommand.SelectCommand.Parameters.Add(sparam1)

        'SECONDO PARAMETRO Cognome
        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@Cognome"
        sparam2.SqlDbType = SqlDbType.NVarChar
        MyCommand.SelectCommand.Parameters.Add(sparam2)

        'TERZO PARAMETRO Nome
        Dim sparam3 As SqlClient.SqlParameter
        sparam3 = New SqlClient.SqlParameter
        sparam3.ParameterName = "@Nome"
        sparam3.SqlDbType = SqlDbType.NVarChar
        MyCommand.SelectCommand.Parameters.Add(sparam3)

        'QUARTO PARAMETRO CodiceFiscale
        Dim sparam4 As SqlClient.SqlParameter
        sparam4 = New SqlClient.SqlParameter
        sparam4.ParameterName = "@CodiceFiscale"
        sparam4.SqlDbType = SqlDbType.NVarChar
        MyCommand.SelectCommand.Parameters.Add(sparam4)

        'QUINTO PARAMETRO Corso
        Dim sparam5 As SqlClient.SqlParameter
        sparam5 = New SqlClient.SqlParameter
        sparam5.ParameterName = "@Corso"
        sparam5.SqlDbType = SqlDbType.Int
        MyCommand.SelectCommand.Parameters.Add(sparam5)

        'SESTO PARAMETRO AnnoCorso
        Dim sparam6 As SqlClient.SqlParameter
        sparam6 = New SqlClient.SqlParameter
        sparam6.ParameterName = "@AnnoCorso"
        sparam6.SqlDbType = SqlDbType.NVarChar
        MyCommand.SelectCommand.Parameters.Add(sparam6)

        Dim Reader As SqlClient.SqlDataReader
        MyCommand.SelectCommand.Parameters("@CodEnte").Value = CodEnte
        MyCommand.SelectCommand.Parameters("@DenominazioneEnte").Value = DenominazioneEnte
        MyCommand.SelectCommand.Parameters("@Cognome").Value = Cognome
        MyCommand.SelectCommand.Parameters("@Nome").Value = Nome
        MyCommand.SelectCommand.Parameters("@CodiceFiscale").Value = CodiceFiscale
        MyCommand.SelectCommand.Parameters("@Corso").Value = Corso
        MyCommand.SelectCommand.Parameters("@AnnoCorso").Value = AnnoCorso

        Dim DstPrimario As New DataSet
        MyCommand.Fill(DstPrimario)
        EseguiStoreReaderRicercaFormatori = DstPrimario
        DstPrimario = Nothing
    End Function

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaGriglia()
    End Sub

    Private Sub CaricaAnno(ByRef ddL As DropDownList, ByVal intRicerca As Byte)
        Dim intAnnoPartenza As Integer

        ddL.Items.Clear()
        ddL.Items.Add("Selezionare")
        If intRicerca = 1 Then ' mi ricorda se sto caricando la combo dell'anno come filtro di ricerca
            ddL.Items.Add("Nessuno")
        End If
        For intAnnoPartenza = 2009 To Year(Now)
            ddL.Items.Add(intAnnoPartenza)
        Next
    End Sub
    Private Function StoreAggiornaAnnoCorso(ByVal IdEntePersonale As Integer, ByVal AnnoCorso As Integer, ByVal UsernameAggiornamento As String)

        Try
            'Dim sReturnValue As String
            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_FORMAZIONE_AGGIORNA_ANNO_CORSO_FORMAZIONE"
            MyCommand.Connection = Session("conn")

            'PRIMO PARAMETRO IDENTEPERSONALE
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdEntePersonale"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            'SECONDO PARAMETRO ANNOCORSOAGGIORNAMENTO
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@AnnoCorsoAggiornamento"
            sparam1.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam1)

            'TERZO PARAMETRO USERAGGIORNAMENTO
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@UserAggiornamento"
            sparam2.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam2)

            MyCommand.Parameters("@IdEntePersonale").Value = IdEntePersonale
            MyCommand.Parameters("@AnnoCorsoAggiornamento").Value = AnnoCorso
            MyCommand.Parameters("@UserAggiornamento").Value = UsernameAggiornamento

            MyCommand.ExecuteNonQuery()

        Catch ex As Exception
            Return "ERRORE"
        End Try

    End Function
    Private Function StoreAggiornaCorsoFormazione(ByVal IdEntePersonale As Integer, ByVal CorsoAggiornamento As Integer)

        Try
            'Dim sReturnValue As String
            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_FORMAZIONE_AGGIORNA_CORSO_FORMAZIONE"
            MyCommand.Connection = Session("conn")

            'PRIMO PARAMETRO IDENTEPERSONALE
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdEntePersonale"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            'SECONDO PARAMETRO CORSOAGGIORNAMENTO
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@CorsoAggiornamento"
            sparam1.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam1)

            ''TERZO PARAMETRO USERAGGIORNAMENTO
            'Dim sparam2 As SqlClient.SqlParameter
            'sparam2 = New SqlClient.SqlParameter
            'sparam2.ParameterName = "@UserAggiornamento"
            'sparam2.SqlDbType = SqlDbType.NVarChar
            'MyCommand.Parameters.Add(sparam2)

            MyCommand.Parameters("@IdEntePersonale").Value = IdEntePersonale
            MyCommand.Parameters("@CorsoAggiornamento").Value = CorsoAggiornamento


            MyCommand.ExecuteNonQuery()

        Catch ex As Exception
            Return "ERRORE"
        End Try

    End Function


    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        'RICHIAMO FUNZIONE CARICAMENTO GRIGLIA
        lblmessaggio.Text = ""
        dgRisultatoRicerca.CurrentPageIndex = 0
        Table4.Visible = True
        CaricaGriglia()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub dgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
        Dim intConta As Integer = 0
        Dim item As DataGridItem

        If ddlAggiornaCorsoFor.SelectedItem.Text = "Selezionare" Then
            lblmessaggio.Text = "E' necessario selezionare il corso di aggiornamento."
        End If
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkCorso"), CheckBox)
            If check.Checked = True Then
                StoreAggiornaCorsoFormazione(dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text, ddlAggiornaCorsoFor.SelectedValue)
                intConta = intConta + 1
            End If
        Next
        If intConta = 0 Then
            lblmessaggio.Text = "Non è stato selezionato nessun Formatore da aggiornare."
        Else
            lblmessaggio.Text = "Salvataggio eseguito con successo."
            chkSelDesel.Checked = False
            CaricaGriglia()
        End If
    End Sub
    Private Sub chkSelDesel_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelDesel.CheckedChanged
        If chkSelDesel.Checked = True Then

            Dim nRighe As Integer
            Dim x As Integer
            nRighe = dgRisultatoRicerca.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = dgRisultatoRicerca.Items(x).Cells(0).FindControl("chkCorso")
                If chkoggetto.Visible = True Then
                    chkoggetto.Checked = True
                Else
                    chkoggetto.Checked = False
                End If
            Next
        Else
            Dim nRighe As Integer
            Dim x As Integer
            nRighe = dgRisultatoRicerca.Items.Count
            For x = 0 To nRighe - 1
                Dim chkoggetto As CheckBox = dgRisultatoRicerca.Items(x).Cells(0).FindControl("chkCorso")
                chkoggetto.Checked = False
            Next

        End If
    End Sub

    Protected Sub imgStampa_Click(sender As Object, e As EventArgs) Handles imgStampa.Click
        imgStampa.Visible = False
        StampaCSV(Session("DtbRicerca"))
    End Sub
    Function StampaCSV(ByVal DTBRicerca As DataTable)

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            'lblmessaggio.Text = lblmessaggio.Text & "La ricerca non ha prodotto nessun risultato."
            'lblStampa.Visible = False
            'hlCSVRicerca.Visible = False
            ApriCSV1.Visible = False
            imgStampa.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
            'Creazione dell'inntestazione del CSV
            Dim intNumCol As Int64 = DTBRicerca.Columns.Count
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            Writer.WriteLine(xLinea)
            xLinea = vbNullString

            'Scorro tutte le righe del datatable e riempio il CSV
            For i = 0 To DTBRicerca.Rows.Count - 1

                For j = 0 To intNumCol - 1
                    If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                        xLinea &= vbNullString & ";"
                    Else
                        xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                    End If
                Next

                Writer.WriteLine(xLinea)
                xLinea = vbNullString

            Next

            'lblStampa.Visible = True
            'hlCSVRicerca.Visible = True
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function
    Protected Sub imgAssegnaCorso_Click(sender As Object, e As EventArgs) Handles imgAssegnaCorso.Click
        Dim intConta As Integer = 0
        Dim item As DataGridItem

        If ddlAggiornaCorsoFor.SelectedItem.Text = "Selezionare" Then
            lblmessaggio.Text = "E' necessario selezionare il corso di aggiornamento."
            Exit Sub
        End If
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkCorso"), CheckBox)
            If check.Checked = True Then
                StoreAggiornaCorsoFormazione(dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text, ddlAggiornaCorsoFor.SelectedValue)
                intConta = intConta + 1
            End If
        Next
        If intConta = 0 Then
            lblmessaggio.Text = "Non è stato selezionato nessun Formatore da aggiornare."
            Exit Sub
        Else
            lblmessaggio.Text = "Salvataggio eseguito con successo."
            chkSelDesel.Checked = False
            CaricaGriglia()
        End If
    End Sub
    Protected Sub cmdAssegna_Click(sender As Object, e As EventArgs) Handles cmdAssegna.Click
        Dim intConta As Integer = 0
        Dim item As DataGridItem

        If ddlAggiornaAnnoCorsoFor.SelectedItem.Text = "Selezionare" Then
            lblmessaggio.Text = "E' necessario selezionare l'anno del corso di aggiornamento."
            Exit Sub
        End If
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkCorso"), CheckBox)
            If check.Checked = True Then
                StoreAggiornaAnnoCorso(dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text, ddlAggiornaAnnoCorsoFor.SelectedItem.Text, Session("Utente"))
                intConta = intConta + 1
            End If
        Next
        If intConta = 0 Then
            lblmessaggio.Text = "Non è stato selezionato nessun Formatore da aggiornare."
            Exit Sub
        Else
            lblmessaggio.Text = "Salvataggio eseguito con successo."
            chkSelDesel.Checked = False
            CaricaGriglia()
        End If
    End Sub
End Class