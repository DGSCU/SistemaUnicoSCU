Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Imports System.Data.SqlClient
Public Class WfrmProgrammiRicIstanzadiPresentazione
    Inherits System.Web.UI.Page
    Dim mydataset As DataSet
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strquery As String
        Dim dtrGenerico As SqlClient.SqlDataReader
        If Not Session("LogIn") Is Nothing Then 'verifico validità log-in
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'testupdate
        If IsPostBack = False Then
           
            strquery = " SELECT Bando.idBando,bando.bandobreve,bando.annobreve  "
            strquery = strquery & " FROM bando"
            strquery = strquery & " WHERE Programmi =1 "
            strquery = strquery & " UNION "
            strquery = strquery & " SELECT  '0',' TUTTI ', 99  from bando "
            strquery = strquery & " ORDER BY Bando.annobreve desc"

            dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
            DdlBando.DataSource = dtrGenerico
            DdlBando.DataTextField = "bandobreve"
            DdlBando.DataValueField = "idbando"
            DdlBando.DataBind()

            ChiudiDataReader(dtrGenerico)

            Dim dtr As Data.SqlClient.SqlDataReader

            dtr = ClsServer.CreaDatareader("select ' Seleziona ' as Appoggio from bando" & _
            " union" & _
            " select distinct convert(varchar,(year(DataInizioValidità))) as Anno from bando where Programmi=1 order by 1", Session("conn"))
            If dtr.HasRows = True Then 'eseguo query per anni data bando
                Do While dtr.Read() 'popolo combo
                    ddlanno.Items.Add(Trim(dtr.GetValue(0)))
                Loop
                dtr.Close()
                dtr = Nothing
            Else
                dtr.Close()
                dtr = Nothing
            End If

        End If
    End Sub
    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata

        GridDaCaricare.DataSource = mydataset
        GridDaCaricare.DataBind()
       
        Dim NomeColonne(4) As String
        Dim NomiCampiColonne(4) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Avviso"
        NomeColonne(1) = "Data Inizio Avviso"
        NomeColonne(2) = "Data Fine Avviso"
        NomeColonne(3) = "Stato"
        NomeColonne(4) = "N. Programmi"


        NomiCampiColonne(0) = "Bando"
        NomiCampiColonne(1) = "DataInizio"
        NomiCampiColonne(2) = "DataFine"
        NomiCampiColonne(3) = "Stato"
        NomiCampiColonne(4) = "NProgrammi"


        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(mydataset, 4, NomeColonne, NomiCampiColonne)

        '***********************************************************************
        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "Attenzione, non sono presenti Istanze di Presentazione da modificare."
            dgRisultatoRicerca.Visible = False

            imgStampa.Visible = False
        Else
            imgStampa.Visible = True
        End If
    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Function StampaCSV(ByVal DTBRicerca As DataTable)

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        'Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False

        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpRicercaIstanza" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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

            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function

    Protected Sub imgStampa_Click(sender As Object, e As EventArgs) Handles imgStampa.Click
        imgStampa.Visible = False
        StampaCSV(Session("DtbRicerca"))
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        lblmessaggio.Text = ""
        dgRisultatoRicerca.Visible = True

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_PROGRAMMI_ISTANZA_RICERCA]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")
            If DdlBando.SelectedValue <> "TUTTI" Then
                sqlDAP.SelectCommand.Parameters.Add("@IDBando", SqlDbType.Int).Value = DdlBando.SelectedValue
            End If

            If ddlanno.SelectedValue <> "Seleziona" Then
                sqlDAP.SelectCommand.Parameters.Add("@Anno", SqlDbType.Int).Value = ddlanno.SelectedValue
            End If

            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar, 50).Value = Session("txtCodEnte")


            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            dgRisultatoRicerca.DataSource = dataSet
            dgRisultatoRicerca.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try

     

        'CaricaDataGrid(dgRisultatoRicerca)
        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "Attenzione, non sono presenti Istanze di Presentazione per i parametri inseriti."
            dgRisultatoRicerca.Visible = False
            imgStampa.Visible = False
        Else
            imgStampa.Visible = True
        End If
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Ricerca"
                Response.Redirect("WfrmRicercaProgrammi.aspx?idBP=" & e.Item.Cells(8).Text & "&VengoDa=RicIstanza&IdBando=" & e.Item.Cells(1).Text & "&Bando=" & e.Item.Cells(2).Text & "")
            Case "Select"
                Response.Redirect("WfrmProgrammiIstanza.aspx?idBP=" & e.Item.Cells(8).Text & "")
        End Select
    End Sub
End Class