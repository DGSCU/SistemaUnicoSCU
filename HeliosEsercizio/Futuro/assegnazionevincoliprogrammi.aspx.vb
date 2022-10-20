Imports System.Data.SqlClient
Public Class assegnazionevincoliprogrammi
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
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

    Private Sub CancellaMessaggi()
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
            CaricaDatiProgramma(Request.QueryString("IdProgramma"))
            CaricaComboVincoliprogrammi(Request.QueryString("IdProgramma"))
            DisabilitaTasti(Request.QueryString("IdProgramma"))
        End If
        txtidprogramma.Value = Request.QueryString("IdProgramma")
        strsql = "select IdProgramma, TipiProgramma.IdTipoProgramma from Programmi " & _
                                        " inner join TipiProgramma  on (TipiProgramma.idtipoprogramma=Programmi.idtipoprogramma) " & _
                                        " where IdProgramma=" & txtidprogramma.Value & ""
        ChiudiDataReader(dtrgenerico)

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()

        TxtTipoprog1.Value = dtrgenerico("IdTipoProgramma")

        ChiudiDataReader(dtrgenerico)

        'Antonello questa forse va tolta 30/07/2020------------------------------------------

        'strsql = "SELECT isnull(UserVerificaBox, 'Nessuno') as UserVerificaBox, idregionecompetenza FROM Programmi " & _
        '         "WHERE IdProgramma=" & txtidprogramma.Value & ""
        'dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        'dtrgenerico.Read()

        'Dim strappocomp As String = dtrgenerico("idregionecompetenza")

        'ChiudiDataReader(dtrgenerico)

        '-------------------------------------------------------------------------------------

        'Dim strappocomp As String = "22"
        'If ClsUtility.ForzaFascicoloInformaticoProgetti(Session("Utente"), Session("conn")) = True And strappocomp = "22" Then
        '    imgAssociaDocumentiProg.Visible = True
        'End If
        If Session("TipoUtente") = "U" Then
            NascondiMenuLaterale()
        End If
    End Sub
    Sub CaricaDatiProgramma(ByVal IdProgramma As String)
        '***Generata da Bagnani Jonathan in data:04/11/04
        Dim strLocal As String
        'datareader locale 
        Dim dtrLocal As SqlClient.SqlDataReader

        strLocal = "select a.titolo, a.CodiceProgramma "
        strLocal = strLocal & "from Programmi as a "
        strLocal = strLocal & "where a.idProgramma='" & IdProgramma & "'"

        'eseguo la query e passo il risultato al datareader
        dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))

        'controllo se ci sono sedi di attuazione assegnate al volontario selezionato

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            lblTitolo.Text = dtrLocal("titolo")
            lblCodiceProgetto.Text = dtrLocal("CodiceProgramma")
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

    End Sub

    Sub CaricaComboVincoliprogrammi(ByVal IdProgramma As String)

        Dim strLocal As String
        'datareader locale 
        Dim dtsLocal As DataSet

        strLocal = "select a.IdVincolo, a.Vincolo, isnull(b.Valore,1) as Valore, b.NotaStorico as ValoreNote from Vincoli as a "
        strLocal = strLocal & "inner join bando on bando.IdRevisioneAccettazioneProgrammi = a.IdRevisioneProgrammi "
        strLocal = strLocal & "inner join BandiProgrammi on bando.idbando = BandiProgrammi.idbando "
        strLocal = strLocal & "inner join programmi on BandiProgrammi.IdBandoProgramma = Programmi.IdBandoProgramma "
        strLocal = strLocal & "left join FlagProgrammi as b on a.IDVincolo=b.IDVincolo and b.IdProgramma=Programmi.IdProgramma "
        strLocal = strLocal & "where a.Programmi=1 and Programmi.IdProgramma = " & IdProgramma
        strLocal = strLocal & " order by a.ordine "

        dtsLocal = ClsServer.DataSetGenerico(strLocal, Session("conn"))

        dtgVincoliProgetti.DataSource = dtsLocal
        dtgVincoliProgetti.DataBind()

        Dim dtgItem As DataGridItem

        For Each dtgItem In dtgVincoliProgetti.Items
            Dim ddlEsitoVincolo As DropDownList = DirectCast(dtgItem.FindControl("ddlEsito"), DropDownList)
            Dim txtNotaStorico As TextBox = DirectCast(dtgItem.FindControl("txtNote"), TextBox)
            Select Case dtgVincoliProgetti.Items(dtgItem.ItemIndex).Cells(3).Text()
                Case "1"
                    ddlEsitoVincolo.SelectedValue = 1
                Case "0"
                    ddlEsitoVincolo.SelectedValue = 0
                Case "2"
                    ddlEsitoVincolo.SelectedValue = 2
            End Select
            If dtgVincoliProgetti.Items(dtgItem.ItemIndex).Cells(5).Text() = "&nbsp;" Then
                txtNotaStorico.Text = vbNullString
            Else
                txtNotaStorico.Text = dtgVincoliProgetti.Items(dtgItem.ItemIndex).Cells(5).Text()
            End If
        Next


        strLocal = "select a.IdVincolo, a.Vincolo, isnull(b.Valore,1) as Valore, b.NotaStorico as ValoreNote from Vincoli as a "
        strLocal = strLocal & "inner join bando on bando.IdRevisioneAccettazioneProgrammi = a.IdRevisioneProgrammi "
        strLocal = strLocal & "inner join BandiProgrammi on bando.idbando = BandiProgrammi.idbando "
        strLocal = strLocal & "inner join programmi on BandiProgrammi.IdBandoProgramma = Programmi.IdBandoProgramma "
        strLocal = strLocal & "left join FlagProgrammi as b on a.IDVincolo=b.IDVincolo and b.IdProgramma=Programmi.IdProgramma "
        strLocal = strLocal & "where a.Programmi=2 and Programmi.IdProgramma = " & IdProgramma
        strLocal = strLocal & " order by a.ordine "

        dtsLocal = ClsServer.DataSetGenerico(strLocal, Session("conn"))

        dtgVincoliProgetti2.DataSource = dtsLocal
        dtgVincoliProgetti2.DataBind()

        Dim dtgItem2 As DataGridItem

        For Each dtgItem2 In dtgVincoliProgetti2.Items
            Dim ddlEsitoVincolo2 As DropDownList = DirectCast(dtgItem2.FindControl("ddlEsito2"), DropDownList)
            Dim txtNotaStorico2 As TextBox = DirectCast(dtgItem2.FindControl("txtNote2"), TextBox)
            Select Case dtgVincoliProgetti2.Items(dtgItem2.ItemIndex).Cells(3).Text()
                Case "1"
                    ddlEsitoVincolo2.SelectedValue = 1
                Case "0"
                    ddlEsitoVincolo2.SelectedValue = 0
                Case "2"
                    ddlEsitoVincolo2.SelectedValue = 2
            End Select
            If dtgVincoliProgetti2.Items(dtgItem2.ItemIndex).Cells(5).Text() = "&nbsp;" Then
                txtNotaStorico2.Text = vbNullString
            Else
                txtNotaStorico2.Text = dtgVincoliProgetti2.Items(dtgItem2.ItemIndex).Cells(5).Text()
            End If
        Next

    End Sub

    Sub AggiornaVincoli(ByVal IdProgramma As String)
        '***Generata da Bagnani Jonathan in data:04/11/04
        Dim strLocal As String
        Try
            strLocal = "delete from FlagProgrammi where IdProgramma='" & IdProgramma & "'"

            'cancello i vincoli esistenti
            Dim cmdDelete As Data.SqlClient.SqlCommand
            cmdDelete = New SqlClient.SqlCommand(strLocal, Session("conn"))
            cmdDelete.ExecuteNonQuery()
            cmdDelete.Dispose()

            Dim dtgItem As DataGridItem
            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            For Each dtgItem In dtgVincoliProgetti.Items
                Dim ddlEsitoVincolo As DropDownList = DirectCast(dtgItem.FindControl("ddlEsito"), DropDownList)
                Dim txtNotaStorico As TextBox = DirectCast(dtgItem.FindControl("txtNote"), TextBox)
                strLocal = "insert into FlagProgrammi (IdProgramma,IdVincolo,Valore,DataModiFICA,NotaStorico) "
                strLocal = strLocal & "values "
                strLocal = strLocal & "('" & IdProgramma & "'," & CInt(dtgVincoliProgetti.Items(dtgItem.ItemIndex).Cells(0).Text()) & "," & ddlEsitoVincolo.SelectedValue & ",GetDate(),'" & Trim(ClsServer.NoApice(txtNotaStorico.Text)) & "')"
                cmdinsert = New SqlClient.SqlCommand(strLocal, Session("conn"))
                cmdinsert.ExecuteNonQuery()
            Next
            cmdinsert.Dispose()

            Dim dtgItem2 As DataGridItem
            'vado a fare la insert
            Dim cmdinsert2 As Data.SqlClient.SqlCommand
            For Each dtgItem2 In dtgVincoliProgetti2.Items
                Dim ddlEsitoVincolo2 As DropDownList = DirectCast(dtgItem2.FindControl("ddlEsito2"), DropDownList)
                Dim txtNotaStorico2 As TextBox = DirectCast(dtgItem2.FindControl("txtNote2"), TextBox)
                strLocal = "insert into FlagProgrammi (IdProgramma,IdVincolo,Valore,DataModiFICA,NotaStorico) "
                strLocal = strLocal & "values "
                strLocal = strLocal & "('" & IdProgramma & "'," & CInt(dtgVincoliProgetti2.Items(dtgItem2.ItemIndex).Cells(0).Text()) & "," & ddlEsitoVincolo2.SelectedValue & ",GetDate(),'" & Trim(ClsServer.NoApice(txtNotaStorico2.Text)) & "')"
                cmdinsert2 = New SqlClient.SqlCommand(strLocal, Session("conn"))
                cmdinsert2.ExecuteNonQuery()
            Next
            cmdinsert2.Dispose()

            msgConferma.Text = "I dati sono stati correttamente salvati. <br/>"
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub imgAmmissibile_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgAmmissibile.Click
        CancellaMessaggi()
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        Dim intstatoattività As Integer

        AggiornaVincoli(Request.QueryString("IdProgramma"))
        ChiudiDataReader(dtrgenerico)
        '*********controllo se l'attività è in graduatoria ed è stata confermata
        'dtrgenerico = ClsServer.CreaDatareader("select idgraduatoriaprogetto" & _
        '" from graduatorieprogetti where statograduatoria=1" & _
        '" and idattività=" & CInt(Request.QueryString("idattivita")) & "", Session("conn"))
        'If dtrgenerico.HasRows = True Then
        '    CancellaMessaggi()
        '    msgErrore.Text = "Il progetto è già in Graduatoria. Impossibile procedere con l'accettazione."
        '    ChiudiDataReader(dtrgenerico)
        '    Exit Sub
        'End If
        'ChiudiDataReader(dtrgenerico)

        '*********fine
        dtrgenerico = ClsServer.CreaDatareader("select a.IdProgramma,a.titolo," & _
                " d.idstatoPROGRAMMA,d.statoPROGRAMMA,b.idbandoProgramma," & _
                " b.idbando,c.idstatobandoPROGRAMMA,c.statobandoPROGRAMMA from Programmi a" & _
                " inner join BandiProgrammi b on a.idbandoProgramma=b.idbandoProgramma" & _
                " inner join statiBandiProgrammi c on b.idstatobandoProgramma=c.idstatobandoProgramma" & _
                " inner join StatiProgrammi d on a.idstatoProgramma=d.idstatoProgramma" & _
                " where c.attivo = 1 And (d.idstatoprogramma=2) And a.IdProgramma='" & Request.QueryString("IdProgramma") & "'", Session("conn"))
        'Verifico stato attuale dell'attività
        If dtrgenerico.HasRows = True Then 'entro in modifica per accreditare
            dtrgenerico.Read()
            intstatoattività = dtrgenerico.GetValue(2)
            dtrgenerico.Close()
            dtrgenerico = Nothing
            Dim CmdModifica As Data.SqlClient.SqlCommand
            'inserisco nella cronologia
            CmdModifica = New Data.SqlClient.SqlCommand("insert into CronologiaProgrammi" & _
            " (idprogramma,idstatoprogramma,datacronologia,idtipocronologia," & _
            " usernameaccreditatore)" & _
            " select " & Request.QueryString("IdProgramma") & ",idstatoprogramma,getdate(),0,'" & Session("Utente") & "'" & _
            " from programmi where IdProgramma='" & Request.QueryString("IdProgramma") & "'", Session("conn"))
            CmdModifica.ExecuteNonQuery()
            CmdModifica.Dispose()
            'modifico programma
            CmdModifica = New SqlClient.SqlCommand("update Programmi set" & _
            " idstatoProgramma=3,dataultimostato=getdate(), usernamestato = '" & Session("Utente") & "' where IdProgramma=" & Request.QueryString("IdProgramma") & "", Session("conn"))
            CmdModifica.ExecuteNonQuery()
            CmdModifica.Dispose()

            msgConferma.Text = "Operazione effettuata con successo."
            DisabilitaTasti(Request.QueryString("IdProgramma"))
        Else  'chiudo se non è possibile modificare
            ChiudiDataReader(dtrgenerico)
            CancellaMessaggi()
            msgErrore.Text = "Impossibile valutare il programma prima dell'accettazione dell'Istanza di Presentazione!"
        End If
    End Sub

    Private Sub imgSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgSalva.Click
        CancellaMessaggi()
        AggiornaVincoli(Request.QueryString("IdProgramma"))
    End Sub

    Private Sub imgNonAmmissibile_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgNonAmmissibile.Click
        CancellaMessaggi()
        Dim dtrgenerico As Data.SqlClient.SqlDataReader

        AggiornaVincoli(Request.QueryString("IdProgramma"))

        '*********controllo se l'attività è in graduatoria ed è stata confermata
        'dtrgenerico = ClsServer.CreaDatareader("select idgraduatoriaprogetto" & _
        '    " from graduatorieprogetti where statograduatoria=1" & _
        '    " and idattività='" & Request.QueryString("idattivita") & "'", Session("conn"))
        'If dtrgenerico.HasRows = True Then
        '    CancellaMessaggi()
        '    msgErrore.Text = "Il progetto è già in Graduatoria. Impossibile respingere il progetto."
        '    ChiudiDataReader(dtrgenerico)
        '    Exit Sub
        'End If
        'ChiudiDataReader(dtrgenerico)
        '*********fine
        'prima di lanciare form di gestione verifico accreditamento 
        dtrgenerico = ClsServer.CreaDatareader("select a.IdProgramma,a.titolo," & _
                " d.idstatoPROGRAMMA,d.statoPROGRAMMA,b.idbandoProgramma," & _
                " b.idbando,c.idstatobandoPROGRAMMA,c.statobandoPROGRAMMA from Programmi a" & _
                " inner join BandiProgrammi b on a.idbandoProgramma=b.idbandoProgramma" & _
                " inner join statiBandiProgrammi c on b.idstatobandoProgramma=c.idstatobandoProgramma" & _
                " inner join StatiProgrammi d on a.idstatoProgramma=d.idstatoProgramma" & _
                " where c.attivo = 1 And (d.idstatoprogramma=2) And a.IdProgramma='" & Request.QueryString("IdProgramma") & "'", Session("conn"))
        'solo se il progetto risulta accreditato lancio form
        If dtrgenerico.HasRows = True Then
            ChiudiDataReader(dtrgenerico)

            Dim Cmdinsert As Data.SqlClient.SqlCommand
            'inserimento in cronologiaprogrammi
            Cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaProgrammi" & _
            " (idprogramma,idstatoprogramma,datacronologia,idtipocronologia," & _
            " usernameaccreditatore)" & _
            " select " & Request.QueryString("IdProgramma") & ",idstatoprogramma,getdate(),0,'" & Session("Utente") & "'" & _
            " from programmi where IdProgramma='" & Request.QueryString("IdProgramma") & "'", Session("conn"))
            Cmdinsert.ExecuteNonQuery()
            Cmdinsert.Dispose()
            'mofico stato programma
            Cmdinsert = New Data.SqlClient.SqlCommand("update Programmi" & _
            "  set idstatoProgramma=4,DataUltimoStato=getdate(), usernamestato = '" & Session("Utente") & "'" & _
            " where IdProgramma=" & Request.QueryString("IdProgramma") & "", Session("conn"))
            Cmdinsert.ExecuteNonQuery()
            Cmdinsert.Dispose()
            DisabilitaTasti(Request.QueryString("IdProgramma"))

            ArchiviaProgetti(Request.QueryString("IdProgramma"))
            '*****************************************************************************
        Else
            CancellaMessaggi()
            msgErrore.Text = "Impossibile valutare il programma prima dell'accettazione dell'Istanza di Presentazione."
            ChiudiDataReader(dtrgenerico)
        End If
    End Sub

    Private Sub imgEscluso_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgEscluso.Click
        CancellaMessaggi()
        Dim dtrgenerico As Data.SqlClient.SqlDataReader

        AggiornaVincoli(Request.QueryString("IdProgramma"))

        '*********controllo se l'attività è in graduatoria ed è stata confermata
        'dtrgenerico = ClsServer.CreaDatareader("select idgraduatoriaprogetto" & _
        '    " from graduatorieprogetti where statograduatoria=1" & _
        '    " and idattività='" & Request.QueryString("idattivita") & "'", Session("conn"))
        'If dtrgenerico.HasRows = True Then
        '    CancellaMessaggi()
        '    msgErrore.Text = "Il progetto è già in Graduatoria. Impossibile respingere il progetto."
        '    ChiudiDataReader(dtrgenerico)
        '    Exit Sub
        'End If
        'ChiudiDataReader(dtrgenerico)
        '*********fine
        'prima di lanciare form di gestione verifico accreditamento 
        dtrgenerico = ClsServer.CreaDatareader("select a.IdProgramma,a.titolo," & _
                " d.idstatoPROGRAMMA,d.statoPROGRAMMA,b.idbandoProgramma," & _
                " b.idbando,c.idstatobandoPROGRAMMA,c.statobandoPROGRAMMA from Programmi a" & _
                " inner join BandiProgrammi b on a.idbandoProgramma=b.idbandoProgramma" & _
                " inner join statiBandiProgrammi c on b.idstatobandoProgramma=c.idstatobandoProgramma" & _
                " inner join StatiProgrammi d on a.idstatoProgramma=d.idstatoProgramma" & _
                " where c.attivo = 1 And (d.idstatoprogramma=2) And a.IdProgramma='" & Request.QueryString("IdProgramma") & "'", Session("conn"))

        'solo se il progetto risulta accreditato lancio form
        If dtrgenerico.HasRows = True Then
            ChiudiDataReader(dtrgenerico)

            Dim Cmdinsert As Data.SqlClient.SqlCommand
            'inserimento in cronologiaprogrammi
            Cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaProgrammi" & _
            " (idprogramma,idstatoprogramma,datacronologia,idtipocronologia," & _
            " usernameaccreditatore)" & _
            " select " & Request.QueryString("IdProgramma") & ",idstatoprogramma,getdate(),0,'" & Session("Utente") & "'" & _
            " from programmi where IdProgramma='" & Request.QueryString("IdProgramma") & "'", Session("conn"))
            Cmdinsert.ExecuteNonQuery()
            Cmdinsert.Dispose()
            'mofico stato programma
            Cmdinsert = New Data.SqlClient.SqlCommand("update Programmi" & _
            "  set idstatoProgramma=5,DataUltimoStato=getdate(), usernamestato = '" & Session("Utente") & "'" & _
            " where IdProgramma=" & Request.QueryString("IdProgramma") & "", Session("conn"))
            Cmdinsert.ExecuteNonQuery()
            Cmdinsert.Dispose()
            DisabilitaTasti(Request.QueryString("IdProgramma"))

            ArchiviaProgetti(Request.QueryString("IdProgramma"))

            '*****************************************************************************
        Else
            CancellaMessaggi()
            msgErrore.Text = "Impossibile valutare il programma prima dell'accettazione dell'Istanza di Presentazione."
            ChiudiDataReader(dtrgenerico)
        End If
    End Sub

    Private Sub imgChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgChiudi.Click
        If Request.Params("tipologia") = "Programmi" Then
            Response.Redirect("WfrmRicercaProgrammi.aspx?VengoDa=" & Request.Params("VengoDa"))
        Else
            Response.Redirect("WfrmProgrammidaValutare.aspx?VengoDa=Valutare&VediEnte=0")
        End If
    End Sub

    'true se non è modificabile, false se è modificabile
    Function DisabilitaTasti(ByVal IdProgramma As String) As Boolean
        Dim dataReader As Data.SqlClient.SqlDataReader
        Dim tastiDisabilitati As Boolean
        Dim query As String


        query = "select StatiProgrammi.IdStatoProgramma ,StatiProgrammi.StatoProgramma ,bando.datainiziovolontari "
        query = query & "from StatiProgrammi "
        query = query & "inner join Programmi on StatiProgrammi.IDStatoProgramma = Programmi.IDStatoProgramma "
        query = query & "inner join bandiProgrammi on Programmi.IdBandoProgramma=bandiProgrammi.IdBandoProgramma "
        query = query & "inner join bando on bando.idbando=bandiProgrammi.idbando "
        query = query & "where Programmi.IdProgramma='" & IdProgramma & "' "
        query = query & "and bando.idstatobando=1"

        dataReader = ClsServer.CreaDatareader(query, Session("conn"))
        If dataReader.HasRows = True Then
            dataReader.Read()
            If dataReader("IdStatoProgramma") <> 2 Then
                BloccaControlliDataGrid()
                imgAmmissibile.Visible = False
                imgAccettaConLimitazioni.Visible = False
                imgNonAmmissibile.Visible = False
                imgEscluso.Visible = False
                imgSalva.Visible = False
                msgErrore.Text = "Il programma non puo' essere modificato perche' risulta essere " & dataReader("StatoProgramma")
                If IsDBNull(dataReader("datainiziovolontari")) = True Then
                    ImgRipristina.Visible = True
                Else
                    ImgRipristina.Visible = False
                End If

                tastiDisabilitati = True
            Else
                tastiDisabilitati = False
            End If
        End If
        ChiudiDataReader(dataReader)
        Return tastiDisabilitati

    End Function

    Sub BloccaControlliDataGrid()
        Dim item As DataGridItem
        For Each item In dtgVincoliProgetti.Items
            Dim ddlEsitoVincolo As DropDownList = DirectCast(item.FindControl("ddlEsito"), DropDownList)
            Dim txtNotaStorico As TextBox = DirectCast(item.FindControl("txtNote"), TextBox)
            ddlEsitoVincolo.Enabled = False
            txtNotaStorico.Enabled = False
        Next
        Dim item2 As DataGridItem
        For Each item2 In dtgVincoliProgetti2.Items
            Dim ddlEsitoVincolo2 As DropDownList = DirectCast(item2.FindControl("ddlEsito2"), DropDownList)
            Dim txtNotaStorico2 As TextBox = DirectCast(item2.FindControl("txtNote2"), TextBox)
            ddlEsitoVincolo2.Enabled = False
            txtNotaStorico2.Enabled = False
        Next
    End Sub
    Sub SBloccaControlliDataGrid()
        Dim item As DataGridItem
        For Each item In dtgVincoliProgetti.Items
            Dim ddlEsitoVincolo As DropDownList = DirectCast(item.FindControl("ddlEsito"), DropDownList)
            Dim txtNotaStorico As TextBox = DirectCast(item.FindControl("txtNote"), TextBox)
            ddlEsitoVincolo.Enabled = True
            txtNotaStorico.Enabled = True
        Next
        Dim item2 As DataGridItem
        For Each item2 In dtgVincoliProgetti2.Items
            Dim ddlEsitoVincolo2 As DropDownList = DirectCast(item2.FindControl("ddlEsito2"), DropDownList)
            Dim txtNotaStorico2 As TextBox = DirectCast(item2.FindControl("txtNote2"), TextBox)
            ddlEsitoVincolo2.Enabled = True
            txtNotaStorico2.Enabled = True
        Next
    End Sub

    Private Sub imgAccettaConLimitazioni_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgAccettaConLimitazioni.Click
        'CancellaMessaggi()
        'Dim dtrgenerico As Data.SqlClient.SqlDataReader
        'Dim statoAttivita As Integer

        'AggiornaVincoli(Request.QueryString("IdProgramma"))
        'ChiudiDataReader(dtrgenerico)

        ''*********controllo se l'attività è in graduatoria ed è stata confermata
        ''dtrgenerico = ClsServer.CreaDatareader("select idgraduatoriaprogetto" & _
        ''" from graduatorieprogetti where statograduatoria=1" & _
        ''" and idattività=" & CInt(Request.QueryString("idattivita")) & "", Session("conn"))
        ''If dtrgenerico.HasRows = True Then
        ''    CancellaMessaggi()
        ''    msgErrore.Text = "Il progetto è già in Graduatoria. Impossibile procedere con l'accettazione con limitazioni."
        ''    ChiudiDataReader(dtrgenerico)
        ''    Exit Sub
        ''End If

        ''ChiudiDataReader(dtrgenerico)

        ''*********fine
        'dtrgenerico = ClsServer.CreaDatareader("select a.IdProgramma,a.titolo," & _
        '        " d.idstatoProgramma,d.statoProgramma,b.idbandoProgramma," & _
        '        " b.idbando,c.idstatobandoProgramma,c.statobandoProgramma from Programmi a" & _
        '        " inner join BandiProgrammi b on a.idbandoProgramma=b.idbandoProgramma" & _
        '        " inner join statiBandiProgrammi c on b.idstatobandoProgramma=c.idstatobandoProgramma" & _
        '        " inner join StatiProgrammi d on a.idstatoProgramma=d.idstatoProgramma" & _
        '        " where c.attivo = 1 And (d.DaValutare = 1 or d.chiusa=1) And a.IdProgramma='" & Request.QueryString("IdProgramma") & "'", Session("conn"))
        ''Verifico stato attuale dell'attività
        'If dtrgenerico.HasRows = True Then 'entro in modifica per accreditare
        '    dtrgenerico.Read()
        '    statoAttivita = dtrgenerico.GetValue(2)
        '    dtrgenerico.Close()
        '    dtrgenerico = Nothing
        '    Dim CmdModifica As Data.SqlClient.SqlCommand
        '    'modifico attività
        '    CmdModifica = New SqlClient.SqlCommand("update Programmi set" & _
        '    " idstatoProgramma=(select idstatoProgramma from StatiProgrammi" & _
        '    " where DaGraduare=1),dataultimostato=getdate(), Limitazioni=1 where IdProgramma=" & Request.QueryString("IdProgramma") & "", Session("conn"))
        '    CmdModifica.ExecuteNonQuery()
        '    CmdModifica.Dispose()
        '    'inserisco nella cronologia
        '    CmdModifica = New SqlClient.SqlCommand("insert into CronologiaAttività" & _
        '    " (idattività,idstatoattività,datacronologia,UsernameAccreditatore,idTipoCronologia)" & _
        '    " values(" & Request.QueryString("IdProgramma") & "," & _
        '    " " & statoAttivita & ", getdate(),'" & Session("Utente") & "',0)", Session("conn"))
        '    CmdModifica.ExecuteNonQuery()
        '    CmdModifica.Dispose()
        '    msgConferma.Text = "Operazione effettuata con successo."
        '    DisabilitaTasti(Request.QueryString("IdProgramma"))
        'Else  'chiudo se non è possibile modificare
        '    ChiudiDataReader(dtrgenerico)
        '    CancellaMessaggi()
        '    msgErrore.Text = "Impossibile valutare il programma prima dell'accettazione dell'Istanza di Presentazione."
        'End If
    End Sub


    Protected Sub Prog_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Prog.Click
        CancellaMessaggi()
        If (imgSalva.Visible = True) Then
            AggiornaVincoli(Request.QueryString("IdProgramma"))
        End If
        If (msgErrore.Text = String.Empty) Then

            Response.Redirect("WfrmProgrammi.aspx?VengoDa=AccettazioneProgrammi&popup=1&Modifica=1&IdProgramma=" & Request.QueryString("IdProgramma") & "&Nazionale=" & TxtTipoprog1.Value)
            'Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdProgramma"), Session("Conn")) & "?VengoDa=AccettazioneProgetti&popup=1&Modifica=1&idattivita=" & Request.QueryString("idattivita") & "&Nazionale=" & TxtTipoprog1.Value)
            'Response.Redirect("TabProgetti.aspx?VengoDa=AccettazioneProgetti&popup=1&Modifica=1&idattivita=" & Request.QueryString("idattivita") & "&Nazionale=" & TxtTipoprog1.Value)
        End If


    End Sub

    'Protected Sub hlRicercaSedi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles hlRicercaSedi.Click
    '    CancellaMessaggi()
    '    If (imgSalva.Visible = True) Then
    '        AggiornaVincoli(Request.QueryString("IdAttivita"))
    '    End If
    '    If (msgErrore.Text = String.Empty) Then
    '        Response.Redirect("WfrmRicercaSede.aspx?VengoDaProgetti=AccettazioneProgetti&VediEnte=1" & "&Nazionale=" & TxtTipoprog1.Value & "&idattivita=" & Request.QueryString("idattivita"))
    '    End If

    'End Sub

    'Protected Sub hlRicercaRisorse_Click(ByVal sender As Object, ByVal e As EventArgs) Handles hlRicercaRisorse.Click
    '    CancellaMessaggi()
    '    If (imgSalva.Visible = True) Then
    '        AggiornaVincoli(Request.QueryString("IdAttivita"))
    '    End If
    '    If (msgErrore.Text = String.Empty) Then
    '        Response.Redirect("ricercaentepersonale.aspx?VengoDaProgetti=AccettazioneProgetti&VediEnte=1" & "&Nazionale=" & TxtTipoprog1.Value & "&idattivita=" & Request.QueryString("idattivita"))
    '    End If
    'End Sub

    Protected Sub imgElencoDocumentiProg_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgElencoDocumentiProg.Click
        CancellaMessaggi()
        If (imgSalva.Visible = True) Then
            AggiornaVincoli(Request.QueryString("IdProgramma"))
        End If

        If (msgErrore.Text = String.Empty) Then
            Response.Redirect("WfrmProgrammiDocumenti.aspx?VengoDa=AccettazioneProgramma&IdProgramma=" & Request.QueryString("IdProgramma") & "&Nazionale=" & TxtTipoprog1.Value)
        End If
    End Sub

    Protected Sub imgRiepilogoDocumentiVol_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Response.Write("<script>")
        'Response.Write("window.open('WfrmAssociaDocumentiProgetti.aspx?VengoDa=AccettazioneProgetti&IdAttivita=" & Request.QueryString("idattivita") & "', 'AssociaDocumenti', 'width=900, height=600,dependent=no,scrollbars=yes,status=si')")
        'Response.Write("</script>")
    End Sub

    Protected Sub ImgRipristina_Click(sender As Object, e As EventArgs) Handles ImgRipristina.Click
        '*** Aggiunto il 07/03/2008 da Simona Cordella
        '*** Controllo se ci sono volontari sul progetto****
        Dim dtrVol As SqlClient.SqlDataReader
        CancellaMessaggi()

        strsql = " SELECT Isnull(Count(entità.identità),0) AS totvol"
        strsql = strsql & " FROM  entità INNER JOIN"
        strsql = strsql & " GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità INNER JOIN"
        strsql = strsql & " AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione INNER JOIN"
        strsql = strsql & " attività ON AttivitàSediAssegnazione.IDAttività = attività.IDAttività inner join"
        strsql = strsql & " programmi ON attività.idprogramma = programmi.idprogramma"
        strsql = strsql & " WHERE  programmi.idprogramma =" & Request.QueryString("IdProgramma") & ""
        dtrVol = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrVol.HasRows = True Then
            dtrVol.Read()
            If dtrVol("totvol") = 0 Then
                ChiudiDataReader(dtrVol)
                Dim CmdModifica As Data.SqlClient.SqlCommand

                'Storico Attività'inserisco nella cronologia
                Dim Cmdinsert As Data.SqlClient.SqlCommand
                'inserimento in cronologiaattività
                Cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaProgrammi" & _
                " (idprogramma,idstatoprogramma,datacronologia,idtipocronologia," & _
                " usernameaccreditatore)" & _
                " select " & Request.QueryString("IdProgramma") & ",idstatoprogramma,getdate(),0,'" & Session("Utente") & "'" & _
                " from programmi where idprogramma='" & Request.QueryString("IdProgramma") & "'", Session("conn"))
                Cmdinsert.ExecuteNonQuery()
                Cmdinsert.Dispose()
                'mofico stato attività a proposto e ConfermaValutazione=0
                Cmdinsert = New Data.SqlClient.SqlCommand("update programmi" & _
                "  set idstatoprogramma=2,DataUltimoStato=getdate(),ConfermaValutazione=0, USERNAMESTATO = '" & Session("Utente") & "'" & _
                " where IDPROGRAMMA=" & Request.QueryString("IdProgramma") & "", Session("conn"))
                Cmdinsert.ExecuteNonQuery()
                Cmdinsert.Dispose()

                RipristinaProgetti(Request.QueryString("IdProgramma"))

                Response.Redirect("assegnazionevincoliprogrammi.aspx?idprogramma=" & CInt(Request.QueryString("IdProgramma")))
            Else
                msgErrore.Text = "Il programma è stato già attivato.Impossibile ripristinarlo."
            End If
        End If
        ChiudiDataReader(dtrVol)
    End Sub
    Private Sub ArchiviaProgetti(ByVal IntIdProgramma As Integer)
        Dim Cmdinsert As Data.SqlClient.SqlCommand
        'inserimento in cronologiaattività
        Cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaAttività" & _
        " (idattività,idstatoattività,datacronologia,idtipocronologia," & _
        " usernameaccreditatore)" & _
        " select idattività,idstatoattività,getdate(),0,'" & Session("Utente") & "'" & _
        " from attività where idprogramma=" & IntIdProgramma & "", Session("conn"))
        Cmdinsert.ExecuteNonQuery()
        Cmdinsert.Dispose()
        'mofico stato attività
        Cmdinsert = New Data.SqlClient.SqlCommand("update attività" & _
        "  set idstatoattività=11, DataUltimoStato=getdate(), statovalutazione = 4" & _
        " where idprogramma=" & IntIdProgramma & "", Session("conn"))
        Cmdinsert.ExecuteNonQuery()
        Cmdinsert.Dispose()
    End Sub

    Private Sub RipristinaProgetti(ByVal IntIdProgramma As Integer)
        Dim Cmdinsert As Data.SqlClient.SqlCommand
        'inserimento in cronologiaattività
        Cmdinsert = New Data.SqlClient.SqlCommand("insert into CronologiaAttività" & _
        " (idattività,idstatoattività,datacronologia,idtipocronologia," & _
        " usernameaccreditatore)" & _
        " select idattività,idstatoattività,getdate(),0,'" & Session("Utente") & "'" & _
        " from attività where idprogramma=" & IntIdProgramma & "", Session("conn"))
        Cmdinsert.ExecuteNonQuery()
        Cmdinsert.Dispose()
        'mofico stato attività
        Cmdinsert = New Data.SqlClient.SqlCommand("update attività" & _
        "  set idstatoattività=4, DataUltimoStato=getdate(), statovalutazione = null" & _
        " where idprogramma=" & IntIdProgramma & "", Session("conn"))
        Cmdinsert.ExecuteNonQuery()
        Cmdinsert.Dispose()
    End Sub
End Class