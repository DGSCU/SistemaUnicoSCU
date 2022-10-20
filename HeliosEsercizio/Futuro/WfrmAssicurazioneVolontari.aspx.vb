Imports System.Data.SqlClient
Imports System.IO
Public Structure TipoStato
    Dim SpName As String
    Dim Stato As String
End Structure
Public Class WfrmAssicurazioneVolontari
    Inherits System.Web.UI.Page
    Dim MyDataSet As New DataSet
    Dim MyDataTable As DataTable
    Dim statoAssicurazioni(2) As TipoStato
    Dim DataCombo As DateTime
    Dim ServerPath As String = ConfigurationSettings.AppSettings("PercorsoFileAssicurazioni")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Page.IsPostBack = False Then

            CaricaDataAvvio()

        End If
    End Sub
    Private Sub CaricaDataAvvio()
        Dim strsql As String
        Dim dtsData As DataSet


        strsql = " Select DISTINCT dbo.formatodata(datainizioservizio) as  datainizioservizio,datainizioservizio  as dis" & _
                 " FROM VW_ASSICURAZIONE_DATEAVVIO " & _
                 " WHERE MacroTipoProgetto = 'SCN' " & _
                 " order by dis desc "
        dtsData = ClsServer.DataSetGenerico(strsql, Session("conn"))

        ddlDataAvvio.DataSource = dtsData
        ddlDataAvvio.DataTextField = "datainizioservizio"
        ddlDataAvvio.DataBind()

    End Sub

    Private Sub hlCrono_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles hlCrono.Click
        'Dim js As String
        'js = "<script> myWin = window.open ('WfrmAssicurazioneVolontarioCrono.aspx','Elenco','width=800,height=400,toolbar=no,location=no,menubar=no,scrollbars=yes')</script>"
        'Response.Write(js)
        Response.Redirect("WfrmAssicurazioneVolontarioCrono.aspx")
    End Sub
  
    Protected Sub imgGenera_Click(sender As Object, e As EventArgs) Handles imgGenera.Click
        Panel1.Visible = True
        DataCombo = ddlDataAvvio.Text
        statoAssicurazioni(0).SpName = "SP_ASSICURAZIONE_ELENCOASSICURATI_V2"
        statoAssicurazioni(0).Stato = "ASSICURATI"
        statoAssicurazioni(1).SpName = "SP_ASSICURAZIONE_ELENCOSOSTITUITI_V2"
        statoAssicurazioni(1).Stato = "SOSTITUITI"
        statoAssicurazioni(2).SpName = "SP_ASSICURAZIONE_ELENCODEPENNATI_V2"
        statoAssicurazioni(2).Stato = "DEPENNATI"
        'con nazione base fissa
        Dim dtbind As New DataTable
        dtbind.Columns.Add(New DataColumn("Gruppo", GetType(String)))
        dtbind.Columns.Add(New DataColumn("Tipo", GetType(String)))
        dtbind.Columns.Add(New DataColumn("Download", GetType(String)))

        For i As Integer = 0 To statoAssicurazioni.Length - 1
            CreateDataSource(statoAssicurazioni(i).Stato, dtbind)
        Next
        GridViewGG.DataSource = dtbind
        GridViewGG.DataBind()
    End Sub
    Private Sub CreateDataSource(ByVal Tipo As String, ByRef dtbind As DataTable)

        Dim i As Integer
        Dim arrParam() As SqlParameter
        Dim NomeFile As String
        Dim strAppoStato As String = ""
        Dim strAppoSpName As String = ""
        Dim strAppoTipo As String = ""

        Dim dr As DataRow

        Select Case Tipo
            Case "ASSICURATI"
                ReDim arrParam(2)

                arrParam(0) = New SqlClient.SqlParameter
                arrParam(0).ParameterName = "@DataInizioServizio"
                arrParam(0).SqlDbType = SqlDbType.DateTime
                arrParam(0).Value = DataCombo


                'arrParam(1) = New SqlClient.SqlParameter
                'arrParam(1).ParameterName = "@NazioneBase"
                'arrParam(1).SqlDbType = SqlDbType.Bit
                'arrParam(1).Value = 1


                '
                arrParam(1) = New SqlClient.SqlParameter
                arrParam(1).ParameterName = "@Username"
                arrParam(1).SqlDbType = SqlDbType.VarChar
                arrParam(1).Value = Session("Utente")

                arrParam(2) = New SqlClient.SqlParameter
                arrParam(2).ParameterName = "@FiltroVisibilita"
                arrParam(2).SqlDbType = SqlDbType.VarChar
                arrParam(2).Value = Session("FiltroVisibilita")

                strAppoStato = statoAssicurazioni(0).Stato
                strAppoSpName = statoAssicurazioni(0).SpName

            Case "SOSTITUITI"
                ReDim arrParam(2)
                arrParam(0) = New SqlClient.SqlParameter
                arrParam(0).ParameterName = "@DataInizioServizio"
                arrParam(0).SqlDbType = SqlDbType.DateTime
                arrParam(0).Value = DataCombo

                arrParam(1) = New SqlClient.SqlParameter
                arrParam(1).ParameterName = "@Username"
                arrParam(1).SqlDbType = SqlDbType.VarChar
                arrParam(1).Value = Session("Utente")

                arrParam(2) = New SqlClient.SqlParameter
                arrParam(2).ParameterName = "@FiltroVisibilita"
                arrParam(2).SqlDbType = SqlDbType.VarChar
                arrParam(2).Value = Session("FiltroVisibilita")

                strAppoStato = statoAssicurazioni(1).Stato
                strAppoSpName = statoAssicurazioni(1).SpName

            Case "DEPENNATI"
                ReDim arrParam(2)

                arrParam(0) = New SqlClient.SqlParameter
                arrParam(0).ParameterName = "@DataInizioServizio"
                arrParam(0).SqlDbType = SqlDbType.DateTime
                arrParam(0).Value = DataCombo

                arrParam(1) = New SqlClient.SqlParameter
                arrParam(1).ParameterName = "@Username"
                arrParam(1).SqlDbType = SqlDbType.VarChar
                arrParam(1).Value = Session("Utente")

                arrParam(2) = New SqlClient.SqlParameter
                arrParam(2).ParameterName = "@FiltroVisibilita"
                arrParam(2).SqlDbType = SqlDbType.VarChar
                arrParam(2).Value = Session("FiltroVisibilita")

                strAppoStato = statoAssicurazioni(2).Stato
                strAppoSpName = statoAssicurazioni(2).SpName

        End Select

        MyDataTable = New DataTable(strAppoStato)

        ' Open the connection 
        Dim cnn As New SqlConnection
        cnn = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
        Dim dt As DataTable


        ' Define the command 
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = strAppoSpName

        ' Handle the parameters 
        If Not arrParam.Length = 0 Then
            For Each param As SqlParameter In arrParam
                cmd.Parameters.Add(param)
            Next
        End If

        ' Define the data adapter and fill the dataset 
        Dim da As New SqlDataAdapter(cmd)
        dt = New DataTable

        Dim ds As DataSet
        ds = New DataSet
        da.Fill(ds)

        Dim intx As Integer = 0
        For intx = 0 To ds.Tables.Count - 1

            dt = ds.Tables(intx)
            'da.Fill(dt)

            MyDataTable = dt


            'MyDataTable = New DataTable(statoAssicurazioni(0).Stato)
            'MyDataTable = ExecuteDataTable(statoAssicurazioni(0).SpName, arrParam)
            'MyDataSet.Tables.Add(MyDataTable)


            Select Case intx
                Case 0
                    strAppoTipo = "ITALIA"
                Case 1
                    strAppoTipo = "ESTERO"
                Case 2
                    strAppoTipo = "ITALIAUE"
                Case 3
                    strAppoTipo = "GG"
                Case 4
                    strAppoTipo = "PNRR"
                Case 5
                    strAppoTipo = "PNRRNG"
                Case 6
                    strAppoTipo = "DIGITALE"
            End Select

            'ciclo
            If OutputXls(MyDataTable, strAppoStato, strAppoTipo, NomeFile) Then

                'GridViewGG.DataSource = MyDataTable
                'GridViewGG.DataBind()





                'carico valori nel data rider 
                dr = dtbind.NewRow()
                dr(0) = strAppoTipo
                dr(1) = strAppoStato
                'dr(2) = NomeFile
                dr(2) = "download" & "\" & NomeFile

                dtbind.Rows.Add(dr)




                'hlAssIt.NavigateUrl = "download" & "\" + NomeFile
                'hlAssIt.Target = "_blank"
                'hlAssIt.Visible = True
                'Label1.Visible = False
            Else
                'hlAssIt.Visible = False
                'Label1.Text = "Non presenti in Archivio."
                'Label1.Visible = True
                'hlAssIt.NavigateUrl = ""
            End If
            'fine ciclo
        Next

    End Sub
    Private Function OutputXls(ByVal Datasource As DataTable, ByVal Tipofile As String, ByVal TipoAssicurazione As String, ByRef NomeFile As String) As Boolean

        Dim grid As New DataGrid
        ' chiamo la routine che imposta alcune proprietà di visualizzazione della griglia
        grid.ShowHeader = True
        grid.HeaderStyle.Font.Bold = True

        ' 1) imposto la sorgente dei dati per la griglia

        'grid.DataSource = Session("appDtsRisRicerca").Tables(0)
        Try

            If Datasource.Rows.Count = 0 Then
                Return False
            End If
        Catch ex As Exception

        End Try

        grid.DataSource = Datasource
        grid.AutoGenerateColumns = True

        grid.DataBind()

        NomeFile = "_" & Session("Utente") & "_" & Format(DataCombo, "yyyyMMdd") & "_" & Tipofile & TipoAssicurazione & "_" & Format(DateTime.Now, "ddMMyyyyHHmmss") & "_" & ".xls"
        'Response.Clear()
        'Response.AddHeader("content-disposition", "attachment;filename=" & NomeFile)
        'Response.Charset = ""
        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.ContentType = "application/vnd.xls"






        Dim stringWrite As System.IO.StringWriter = New System.IO.StringWriter
        '      Dim mio As String = stringWrite.Encoding
        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)




        '“AAAAMMGG_TIPOFILE_USERNAME_DataOraElaborazione.xls”
        'Dove:
        '•	AAAA: Anno della data selezionata in maschera
        '•	MM: Mese della data selezionata in maschera
        '•	GG: Giorno della data selezionata in maschera
        '•	TIPOFILE: tipologia del file prodotto (ASSICURATI/SOSTITUITI/DEPENNATI)
        '•	USERNAME: Utenza che ha effettuato l’elaborazione
        '•	DataOraElaborazione: Quando è stato prodotto il file
        '       NomeFile = Format(Convert.ToDateTime(ddlDataAvvio.SelectedItem.Text), "yyyyMMdd") & Tipofile & Session("Utente") & Format(DateTime.Now, "ddMMyyyyhhmmss") & ".xls"

        'Il percorso del repository sarà quello già utilizzato per l’archiviazione dei modelli documenti e determinazioni e la cartella preposta sarà la seguente:
        '\\appl\modhelios$


        grid.RenderControl(htmlWrite)

        Try
            'salvo in cartella server 
            'ed in cartella pubblica(downlad

            If SaveTextToFile(stringWrite.ToString(), ServerPath & NomeFile) = False Then

                Exit Function

            End If



            If File.Exists(Server.MapPath("download") & "\" & NomeFile) Then
                File.Delete((Server.MapPath("download") & "\" & NomeFile))
            End If
            If SaveTextToFile(stringWrite.ToString(), Server.MapPath("download") & "\" & NomeFile) = False Then

                Exit Function
            End If
        Catch ex As Exception

        End Try
        Return True
        'Response.Write(stringWrite.ToString())
        'Response.Flush()
        ' Response.Flush()
        ' Response.Close()


    End Function
    Public Function ExecuteDataTable(ByVal storedProcedureName As String, ByVal ParamArray arrParam() As SqlParameter) As DataTable
        Dim dt As DataTable

        ' Open the connection 
        Dim cnn As New SqlConnection
        cnn = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))


        ' Define the command 
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = storedProcedureName

        ' Handle the parameters 
        If Not arrParam.Length = 0 Then
            For Each param As SqlParameter In arrParam
                cmd.Parameters.Add(param)
            Next
        End If

        ' Define the data adapter and fill the dataset 
        Dim da As New SqlDataAdapter(cmd)
        dt = New DataTable
        da.Fill(dt)



        Return dt
    End Function
    Public Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean

        Dim Contents As String
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Try


            objReader = New StreamWriter(FullPath, False, System.Text.Encoding.Default)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message

        End Try
        Return bAns
    End Function

    Private Sub ddlDataAvvio_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDataAvvio.SelectedIndexChanged
        Panel1.Visible = False
    End Sub
End Class