Imports System.Data.SqlClient
Imports Ionic.Zip
Imports System.IO
Imports System.Web.UI
Public Class WfrmSigmaGenerazione
    Inherits System.Web.UI.Page
    Dim dataReader As SqlClient.SqlDataReader
    Dim dtsRisRicerca As DataSet
    Dim StrSql As String
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
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Page.IsPostBack = False Then

            CaricaComboMandati()
            ' RicercaDocumenti(ddlMandato.SelectedValue)
            RicercaDocumenti()
            ChiudiDataReader(dataReader)
        End If
    End Sub
    Private Sub RicercaDocumenti()
        Dim query As String
        CancellaMessaggiInfo()
        'ChiudiDataReader(dataReader)

        StrSql = "SELECT * " & _
            " FROM SIGMA_Generazione_File "

        StrSql = StrSql & "  order by SIGMA_Generazione_File.DataRichiesta desc "

        dtsRisRicerca = ClsServer.DataSetGenerico(StrSql, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dtgConsultaDocumenti.DataSource = dtsRisRicerca
        dtgConsultaDocumenti.DataBind()
        'dataReader = ClsServer.CreaDatareader(query, Session("conn"))
        Session("reader") = dtsRisRicerca


        If dtgConsultaDocumenti.Items.Count > 0 Then
            dtgConsultaDocumenti.DataSource = dtsRisRicerca
            dtgConsultaDocumenti.DataBind()
            dtgConsultaDocumenti.Caption = "Elenco Generazioni Effettuate"
        Else
            dtgConsultaDocumenti.Caption = "La Ricerca Non ha prodotto Risultati"
            dtgConsultaDocumenti.DataBind()

        End If
        
        'ChiudiDataReader(dataReader)

    End Sub
    Private Sub dtgConsultaDocumenti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgConsultaDocumenti.ItemCommand

        Select Case e.CommandName

            Case "Dettaglio"

                CaricagrigliaDettaglio(e.Item.Cells(1).Text)
                dtgFile.Caption = "Elenco File Generazione N.Gen: " & e.Item.Cells(1).Text & " Codice Locale Mandato: " & e.Item.Cells(2).Text
            Case "Zip"


                Dim zip As ZipFile = New ZipFile()
                Dim myTable As DataTable
                Dim query As String
                Dim dsTXT As New DataSet
                Dim dsCSV As New DataSet
                Dim rw As DataRow
                Dim daTXT As New SqlDataAdapter
                Dim daCSV As New SqlDataAdapter
                Dim cbTXT As SqlCommandBuilder = New SqlCommandBuilder(daTXT)
                Dim cbCSV As SqlCommandBuilder = New SqlCommandBuilder(daCSV)
                CancellaMessaggiInfo()
               
                query = "SELECT SIGMA_Generazione_File_Allegati.IdGenerazioneAllegato, SIGMA_Generazione_File_Allegati.Tabella, SIGMA_Generazione_File_Allegati.FileNameTXT ,SIGMA_Generazione_File_Allegati.FileNameCSV" & _
                    " FROM SIGMA_Generazione_File_Allegati " & _
                    " where SIGMA_Generazione_File_Allegati.IdGenerazione=" & e.Item.Cells(1).Text & ""
                query = query & "  order by SIGMA_Generazione_File_Allegati.Tabella  "


                myTable = ClsServer.CreaDataTable(query, False, Session("conn"))


                For Each myRow In myTable.Rows

                    '--------------------------------------------TXT
                    query = "SELECT BinDataTXT,FileNameTXT FROM SIGMA_Generazione_File_Allegati WHERE IdGenerazioneAllegato = " & myRow.Item(0).ToString
                    dsTXT = ClsServer.DataSetGenerico(query, Session("conn"))

                    daTXT = New SqlClient.SqlDataAdapter(query, CType(Session("conn"), SqlClient.SqlConnection))
                    daTXT.Fill(dsTXT, "_FileTest")

                    rw = dsTXT.Tables("_FileTest").Rows(0)

                    ' Make sure you have some rows
                    Dim i As Integer = dsTXT.Tables("_FileTest").Rows.Count
                    If i > 0 Then
                        Dim bBLOBStorage() As Byte = _
                        dsTXT.Tables("_FileTest").Rows(0)("BinDataTXT")
                        'oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("FileNameTXT")
                        FileByteToPath(bBLOBStorage, dsTXT.Tables("_FileTest").Rows(0)("FileNameTXT"))
                    End If

                    zip.AddFile(Server.MapPath("download") & "\" & dsTXT.Tables("_FileTest").Rows(0)("FileNameTXT"), "\")

                    '----------------------------------------CSV
                    query = "SELECT BinDataCSV,FileNameCSV FROM SIGMA_Generazione_File_Allegati WHERE IdGenerazioneAllegato = " & myRow.Item(0).ToString
                    dsCSV = ClsServer.DataSetGenerico(query, Session("conn"))

                    daCSV = New SqlClient.SqlDataAdapter(query, CType(Session("conn"), SqlClient.SqlConnection))
                    daCSV.Fill(dsCSV, "_FileTest")
                    rw = dsCSV.Tables("_FileTest").Rows(0)

                    ' Make sure you have some rows
                    Dim y As Integer = dsCSV.Tables("_FileTest").Rows.Count
                    If y > 0 Then
                        Dim bBLOBStorage() As Byte = _
                        dsCSV.Tables("_FileTest").Rows(0)("BinDataCSV")
                        'oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("FileNameCSV")
                        FileByteToPath(bBLOBStorage, dsCSV.Tables("_FileTest").Rows(0)("FileNameCSV"))
                    End If
                    zip.AddFile(Server.MapPath("download") & "\" & dsCSV.Tables("_FileTest").Rows(0)("FileNameCSV"), "\")
                    'zip.AddFile(Server.MapPath("download") & "\file1.CSV", "\")
                Next

                '-------------------------------------------------------------------------------------
                hlScarica1.Visible = True
                hlScarica1.Text = "ZIP" & e.Item.Cells(1).Text & ".rar"
                zip.Save(Server.MapPath("download") & "\" & hlScarica1.Text)
                hlScarica1.NavigateUrl = "download" & "\" & hlScarica1.Text


            Case "Si"
                ChiudiDataReader(dataReader)

                Dim strquery1 As String
                strquery1 = "Update  SIGMA_Generazione_File  set UsernameIndicazioneCaricamento = '" & Session("Utente") & "' , DataIndicazioneCaricamento = getdate(), Caricato = 1 from SIGMA_Generazione_File  where IdGenerazione  =" & e.Item.Cells(1).Text & " "
                Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, Session("Conn"))
                CmdUpDateSediAttuazione.ExecuteNonQuery()
                CmdUpDateSediAttuazione.Dispose()
                ChiudiDataReader(dataReader)
                RicercaDocumenti()

            Case "No"
                ChiudiDataReader(dataReader)
                Dim strquery1 As String
                strquery1 = "Update SIGMA_Generazione_File  set UsernameIndicazioneCaricamento = '" & Session("Utente") & "' , DataIndicazioneCaricamento = getdate(), Caricato = 0 from SIGMA_Generazione_File where IdGenerazione  =" & e.Item.Cells(1).Text & " "
                Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, Session("Conn"))
                CmdUpDateSediAttuazione.ExecuteNonQuery()
                CmdUpDateSediAttuazione.Dispose()
                ChiudiDataReader(dataReader)



                Dim strquery2 As String
                strquery2 = "UPDATE ENTITàDOCUMENTI SET TRASMESSO = 0, DATATRASMISSIONE =NULL from SIGMA_TABELLA_SD03 A INNER JOIN ENTITàDOCUMENTI B ON A.IDDOCUMENTO = B.IDENTITàDOCUMENTO where IdGenerazione  =" & e.Item.Cells(1).Text & " and TipoDocumento in ('CONTRATTO','PRESENZE','FORMGENVOL')"
                Dim CmdUpDateENTITADOCUMENTI As New Data.SqlClient.SqlCommand(strquery2, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, Session("Conn"))
                CmdUpDateENTITADOCUMENTI.ExecuteNonQuery()
                CmdUpDateENTITADOCUMENTI.Dispose()
                ChiudiDataReader(dataReader)



                Dim strquery3 As String
                strquery3 = "UPDATE attivitàDOCUMENTI SET TRASMESSO =0, DATATRASMISSIONE = NULL from SIGMA_TABELLA_SD03 A INNER JOIN attivitàDOCUMENTI B ON A.IDDOCUMENTO = B.IdAttivitàDocumento where IdGenerazione  =" & e.Item.Cells(1).Text & " and TipoDocumento in ('PROGETTO')"
                Dim CmdUpDateATTIVITADOCUMENTI As New Data.SqlClient.SqlCommand(strquery3, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'Dim CmdUpDateSediAttuazione As New Data.SqlClient.SqlCommand(strquery1, Session("Conn"))
                CmdUpDateATTIVITADOCUMENTI.ExecuteNonQuery()
                CmdUpDateATTIVITADOCUMENTI.Dispose()
                ChiudiDataReader(dataReader)


                RicercaDocumenti()

        End Select
    End Sub

    Private Sub dtgConsultaDocumenti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgConsultaDocumenti.PageIndexChanged
        dtgConsultaDocumenti.SelectedIndex = -1
        dtgConsultaDocumenti.EditItemIndex = -1
        dtgConsultaDocumenti.CurrentPageIndex = e.NewPageIndex
        dtgConsultaDocumenti.DataSource = Session("reader")
        dtgConsultaDocumenti.DataBind()
    End Sub
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub cmdElabora_Click(sender As Object, e As EventArgs) Handles cmdElabora.Click
        cmdElabora.Visible = False
        Dim sEsito As String = ""
        If ddlMandato.SelectedItem.Text = "" Then
            msgErrore.Text = "Selezionare un Mandato"
        Else
            sEsito = EseguiStoreSIGMA(ddlMandato.SelectedItem.Text, Session("Utente"), Session("conn"))
            If sEsito <> "" Then
                msgErrore.Text = sEsito
                cmdElabora.Visible = True
            Else
                RicercaDocumenti()
                msgInfo.Text = "GENERAZIONE COMPLETATA CON SUCCESSO"
                cmdElabora.Visible = True
            End If
        End If
    End Sub

    Private Sub CaricaComboMandati()
        ChiudiDataReader(dataReader)
        'INIZIO ANTONELLO 
        ddlMandato.Items.Clear()
        StrSql = " select '' as CodiceLocaleMandato, 0 as CodiceMandato union select CodiceLocaleMandato,CodiceMandato from VW_MANDATI order by CodiceMandato"

        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
        dataReader = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dataReader.HasRows Then
            'carico la combo delle sedi di attuazione
            ddlMandato.DataSource = dataReader
            ddlMandato.DataTextField = "CodiceLocaleMandato"
            ddlMandato.DataValueField = "CodiceMandato"
            ddlMandato.DataBind()

            If Not dataReader Is Nothing Then
                dataReader.Close()
                dataReader = Nothing
            End If
        End If
        ChiudiDataReader(dataReader)
    End Sub

    Private Sub dtgFile_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgFile.ItemCommand
        Dim objHLink As HyperLink

        Select Case e.CommandName

            Case "DownloadTxt"
                objHLink = clsGestioneDocumentiAccreditamento.RecuperaDocumentoSigmaTXT(e.Item.Cells(0).Text, Session("Conn"))
                divDownloadFile.Visible = True
                hlScarica.Visible = True
                hlScarica.Text = objHLink.Text
                hlScarica.NavigateUrl = objHLink.NavigateUrl
            Case "DownloadCsv"
                objHLink = clsGestioneDocumentiAccreditamento.RecuperaDocumentoSigmaCSV(e.Item.Cells(0).Text, Session("Conn"))
                divDownloadFile.Visible = True
                hlScarica.Visible = True
                hlScarica.Text = objHLink.Text
                hlScarica.NavigateUrl = objHLink.NavigateUrl
        End Select
    End Sub

    Sub CaricagrigliaDettaglio(ByVal IdGenSel As Integer)

        Dim query As String
        CancellaMessaggiInfo()
        ChiudiDataReader(dataReader)

        query = "SELECT SIGMA_Generazione_File_Allegati.IdGenerazioneAllegato, SIGMA_Generazione_File_Allegati.Tabella, SIGMA_Generazione_File_Allegati.FileNameTXT ,SIGMA_Generazione_File_Allegati.FileNameCSV" & _
            " FROM SIGMA_Generazione_File_Allegati " & _
            " where SIGMA_Generazione_File_Allegati.IdGenerazione=" & IdGenSel & " "
        query = query & "  order by SIGMA_Generazione_File_Allegati.Tabella  "

        dataReader = ClsServer.CreaDatareader(query, Session("conn"))
        Session("reader") = dataReader


        If dataReader.HasRows Then
            dtgFile.DataSource = dataReader
            dtgFile.DataBind()
            'dtgFile.Caption = "Elenco Ge"
            hlScarica.Visible = True
        Else
            dtgFile.Caption = "La Ricerca Non ha prodotto Risultati"
            dtgFile.DataBind()

        End If
        ChiudiDataReader(dataReader)



    End Sub
    Private Shared Function FileByteToPath(ByVal dataBuffer As Byte(), ByVal nomeFile As String) As String
        'dichiaro una variabile byte che bufferizza (carica in memoria) il file template richiesto
        'e trasformato in base64
        Dim fs As FileStream
        Dim myPath As New System.Web.UI.Page

        If File.Exists(myPath.Server.MapPath("download") & "\" & nomeFile) Then
            File.Delete(myPath.Server.MapPath("download") & "\" & nomeFile)
        End If
        'passo il template al filestream
        fs = New FileStream(myPath.Server.MapPath("download") & "\" & nomeFile, FileMode.Create, FileAccess.Write)
        'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
        If (dataBuffer.Length > 0) Then
            fs.Write(dataBuffer, 0, dataBuffer.Length)
        End If

        'chiudo lo streaming
        fs.Close()
        Return "download\" & nomeFile
    End Function

    Private Function EseguiStoreSIGMA(ByVal MANDATO As String, ByVal USERNAME As String, ByVal conn As SqlClient.SqlConnection) As String
        'AUTORE: Antonello
        'DESCRIZIONE: Produzione SIGMA
        'DATA: 25/01/2016



        Try
            Dim sReturnValue As String
            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandTimeout = 300
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_SIGMA_GENERAZIONE"
            MyCommand.Connection = conn


            'PRIMO PARAMEtrO IDATTIVITA'
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@CODICELOCALEMANDATO"
            sparam.Size = 50
            sparam.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam)

            'SECONDO PARAMEtrO ATTIVITÀSEDE
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@USERNAMERICHIESTA"
            sparam1.Size = 50
            sparam1.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam1)

            'TERZO PARAMEtrO OUTPUT
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@ESITO"
            sparam2.Size = 255
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam2)

            MyCommand.Parameters("@CODICELOCALEMANDATO").Value = MANDATO
            MyCommand.Parameters("@USERNAMERICHIESTA").Value = USERNAME

            MyCommand.ExecuteNonQuery()

            sReturnValue = MyCommand.Parameters("@ESITO").Value



            Return sReturnValue

        Catch ex As Exception

            Return "ERRORE" & ex.Message
        End Try
    End Function

  
End Class