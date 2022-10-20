Imports System.Data.SqlClient
Imports System.IO

Public Class WfrmRicercaBandi
    Inherits System.Web.UI.Page

    Dim dtsgenerico As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String

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
#End Region

#Region "PageLoad"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If dgRisultatoRicerca.Items.Count = 0 Then
            cmdEsporta.Visible = False
        Else
            cmdEsporta.Visible = True
        End If
        If IsPostBack = False Then
            CaricaCompetenze()
            If Not IsNothing(Context.Items("Ricerca")) Then
                lblRicerca.Value = Context.Items("Ricerca")
                lblpage.Value = Context.Items("page")
                RicercaBandi()
            End If
            strsql = "Select idstatobando,statobando from statibando " & _
                    " union " & _
                    " Select  '0',' Selezionare' from statibando order by statobando "
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            ddlStato.DataSource = dtrgenerico
            ddlStato.DataTextField = "statobando"
            ddlStato.DataValueField = "IdStatoBando"
            ddlStato.DataBind()
            CaricaBando()
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        If lblmessaggio.Text <> "" Then
            RicercaBandi()
        End If
    End Sub

    Private Sub CaricaBando()
        strsql = "SELECT Bando.idBando,bando.bandobreve,bando.annobreve  "
        strsql = strsql & " FROM bando"
        strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql = strsql & " WHERE tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strsql = strsql & " UNION "
        strsql = strsql & " SELECT  '0','','9999'  from bando "
        strsql = strsql & " ORDER BY Bando.annobreve desc ,Bando.idbando"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        ddlBando.DataSource = dtrgenerico
        ddlBando.DataTextField = "bandobreve"
        ddlBando.DataValueField = "IdBando"
        ddlBando.DataBind()
    End Sub
#End Region
#Region "Funzionalità"
    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata

        Dim appo As String
        GridDaCaricare.DataSource = dtsgenerico
        GridDaCaricare.DataBind()
        'Aggiunto da Alessandra Taballione il 29.03.2005
        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(7) As String
        Dim NomiCampiColonne(7) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Bando"
        NomeColonne(1) = "Stato"
        NomeColonne(2) = "Data Inizio"
        NomeColonne(3) = "Data Fine"
        NomeColonne(4) = "Data Inizio Volontari"
        NomeColonne(5) = "Data Fine Volontari"
        NomeColonne(6) = "Importo Stanziato"
        NomeColonne(7) = "Associazione Automatica"

        NomiCampiColonne(0) = "Bando"
        NomiCampiColonne(1) = "StatoBando"
        NomiCampiColonne(2) = "DataInizio"
        NomiCampiColonne(3) = "DataFine"
        NomiCampiColonne(4) = "DataInizioVolontari"
        NomiCampiColonne(5) = "DataFineVolontari"
        NomiCampiColonne(6) = "Importostanziato"
        NomiCampiColonne(7) = "AssociazioneAutomatica"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(dtsgenerico, 7, NomeColonne, NomiCampiColonne)

        '***********************************************************************
        GridDaCaricare.Visible = True
        If lblpage.Value <> "" And lblRicerca.Value <> "" Then
            GridDaCaricare.CurrentPageIndex = CInt(lblpage.Value)
        End If
        If GridDaCaricare.Items.Count = 0 Then
            dgRisultatoRicerca.Caption = "La Ricerca Non Ha Prodotto Risultati"
            cmdEsporta.Visible = False
        Else
            cmdEsporta.Visible = True
            dgRisultatoRicerca.Caption = "Risultato Ricerca Bandi"
            ColoraCelle()
        End If
    End Sub
    Private Sub RicercaBandi()
        If lblRicerca.Value = "" Then
            strsql = "select DISTINCT bando.idbando,bando.bandobreve,bando.bando,bando.dataInizioValidità ," & _
                " case isnull(importoStanziato,-1) when -1 then '0.00' else importoStanziato end as importostanziato ,statobando, " & _
                " case statibando.invalutazione when 1 then 'Khaki'else '' end + '' + " & _
                " case statibando.defaultstato when 1 then 'Gainsboro' else '' end + '' + " & _
                " case statibando.attivo when 1 then 'Lightgreen' else '' end + '' + " & _
                " case statibando.annullato when 1 then 'LightSalmon' else '' end " & _
                " as color, " & _
                " case len(day(datainiziovalidità)) when 1 then '0'+ convert(varchar(20),day(datainiziovalidità))" & _
                " else convert(varchar(20),day(datainiziovalidità))  end + '/'+ " & _
                " case len(month(datainiziovalidità)) when 1 then '0'+ convert(varchar(20),month(datainiziovalidità)) " & _
                " else convert(varchar(20),month(datainiziovalidità)) end + '/'+ convert(varchar(20),year(datainiziovalidità))" & _
                " as datainizio, case len(day(datafinevalidità)) when 1 then '0'+ convert(varchar(20),day(datafinevalidità))" & _
                " else convert(varchar(20),day(datafinevalidità))  end + '/'+ " & _
                " case len(month(datafinevalidità)) when 1 then '0'+ convert(varchar(20),month(datafinevalidità)) " & _
                " else convert(varchar(20),month(datafinevalidità)) end + '/'+ convert(varchar(20),year(datafinevalidità))" & _
                " as datafine ," & _
                " case len(day(DataInizioVolontari)) when 1 then '0'+ convert(varchar(20),day(DataInizioVolontari)) 	else convert(varchar(20),day(DataInizioVolontari))  end + '/'+  case len(month(DataInizioVolontari)) when 1 then '0'+ convert(varchar(20),month(DataInizioVolontari))  else convert(varchar(20),month(DataInizioVolontari)) end + '/'+convert(varchar(20),year(DataInizioVolontari)) as DataInizioVolontari," & _
                " case len(day(DataFineVolontari)) when 1 then '0'+ convert(varchar(20),day(DataFineVolontari))  else convert(varchar(20),day(DataFineVolontari))  end  + '/'+  case len(month(DataFineVolontari)) when 1 then '0'+ convert(varchar(20),month(DataFineVolontari))  else convert(varchar(20),month(DataFineVolontari)) end + '/'+convert(varchar(20),year(DataFineVolontari)) as DataFineVolontari  ," & _
                " case AssociazioneAutomatica when 1 then 'Si'else 'No' end as AssociazioneAutomatica ," & _
                " case EnteAbilitato when 1 then 'Si' else 'No' end as EnteAbilitato" & _
                " from bando  "
            strsql = strsql & "  inner join associaBandoRegioniCompetenze a on bando.idbando =a.idbando "
            strsql = strsql & "  inner join regionicompetenze r on a.idregionecompetenza = r.idregionecompetenza "
            strsql = strsql & "  INNER JOIN statibando on (statibando.idstatobando=bando.idstatobando) "
            strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
            strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
            strsql = strsql & " Where not bando.bando is null "
            If Trim(ddlBando.SelectedItem.Text) <> "" Then
                strsql = strsql & " and bandobreve = '" & Replace(ddlBando.SelectedItem.Text, "'", "''") & "'"
            End If
            If Trim(txtRiferimento.Text) <> "" Then
                strsql = strsql & " and riferimento like '" & Replace(txtRiferimento.Text, "'", "''") & "%'"
            End If
            If Trim(txtInizio.Text) <> "" And txtfine.Text = "" Then
                strsql = strsql & " and datainizioValidità >= '" & txtInizio.Text & "'"
            End If
            If Trim(txtfine.Text) <> "" And txtInizio.Text = "" Then
                strsql = strsql & " and datafineValidità <= '" & txtfine.Text & "'"
            End If
            If Trim(txtInizio.Text) <> "" And Trim(txtfine.Text) <> "" Then
                strsql = strsql & " and dataInizioValidità >= '" & txtInizio.Text & "' and datafineValidità <= '" & txtfine.Text & "'"
            End If
            If ddlStato.SelectedValue <> 0 Then
                strsql = strsql & " and bando.idstatobando = " & ddlStato.SelectedValue & ""
            End If
            If ddlCompetenze.SelectedValue <> "" Then
                Select Case ddlCompetenze.SelectedValue
                    Case 0
                        strsql = strsql & " "
                    Case -1
                        strsql = strsql & " And a.IdRegioneCompetenza = 22"
                    Case -2
                        strsql = strsql & " And a.IdRegioneCompetenza <> 22 And not a.IdRegioneCompetenza is null "
                    Case -3
                        strsql = strsql & " And a.IdRegioneCompetenza is null "
                    Case Else
                        strsql = strsql & " And a.IdRegioneCompetenza = " & ddlCompetenze.SelectedValue
                End Select

            End If
            strsql = strsql & " AND tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            strsql = strsql & "Order by  bando.dataInizioValidità desc, bando.bando, statobando "

        Else
            strsql = lblRicerca.Value
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        If txtRicerca.Value <> "" Then
            dgRisultatoRicerca.CurrentPageIndex = 0
            txtRicerca.Value = ""
        End If
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub
    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 15/06/04
        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer
        For Each item In dgRisultatoRicerca.Items
            For x = 0 To 11
                If dgRisultatoRicerca.Items(item.ItemIndex).Cells(6).Text = "&nbsp;" Then
                    color = Drawing.Color.LightSalmon
                Else
                    color = Drawing.Color.FromName(dgRisultatoRicerca.Items(item.ItemIndex).Cells(6).Text)
                End If
                dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
            Next
        Next
    End Sub
    Private Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlCompetenze.DataSource = dtrCompetenze
                ddlCompetenze.Items.Add("")
                ddlCompetenze.DataTextField = "Descrizione"
                ddlCompetenze.DataValueField = "IDRegioneCompetenza"
                ddlCompetenze.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenze.Enabled = True
                ddlCompetenze.SelectedIndex = 0

            Else
                ddlCompetenze.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    ddlCompetenze.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenze.Enabled = True
                    End If

                End If

            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub
#End Region
#Region "Eventi"
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        lblpage.Value = e.NewPageIndex
        RicercaBandi()
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub
    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        lblRicerca.Value = ""
        dgRisultatoRicerca.CurrentPageIndex = 0
        lblpage.Value = ""
        RicercaBandi()
    End Sub
    Private Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
        Dim idbando As String = dgRisultatoRicerca.SelectedItem.Cells(5).Text
        'Dim ricerca As String = strsql
        'Dim page As String = dgRisultatoRicerca.CurrentPageIndex
        'Context.Items.Add("idbando", dgRisultatoRicerca.SelectedItem.Cells(5).Text)
        'Context.Items.Add("Ricerca", strsql)
        'Context.Items.Add("page", dgRisultatoRicerca.CurrentPageIndex)
        'Server.Transfer("WfrmGestioneBandi.aspx?tipoazione=Modifica", )
        Response.Redirect("WfrmGestioneBandi.aspx?tipoazione=Modifica&idbando=" & idbando)
    End Sub
  
#End Region

    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            cmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub
    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
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

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function

    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEsporta.Click
        cmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub
End Class