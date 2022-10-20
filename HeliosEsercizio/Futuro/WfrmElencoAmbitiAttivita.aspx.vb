Imports System.Data.SqlClient

Public Class WfrmElencoAmbitiAttivita
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim CmdGenerico As SqlClient.SqlCommand
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If IsPostBack = False Then
            ElencoAttivita()
            LblDescrAttivita.Text = (Request.QueryString("strTipoProgetto"))
            SelezionaAmbitiAttivita(Request.QueryString("strIdTipoProgetto"))

            If Request.QueryString("Stato") = "Aperto" Or Request.QueryString("Stato") = "Annullato" Then
                chkSelDesel.Visible = False
                cmdSalva.Visible = False
                bloccaCheck()
            End If
        End If
    End Sub

    Private Sub ElencoAttivita()
        Dim dtsgenerico As DataSet

        strsql = " SELECT macroambitiattività.Codifica + ambitiattività.Codifica AS Codifica,"
        strsql = strsql & " macroambitiattività.MacroAmbitoAttività,"
        strsql = strsql & " ambitiattività.AmbitoAttività,ambitiattività.IdAmbitoAttività"
        strsql = strsql & " FROM ambitiattività"
        strsql = strsql & "  INNER JOIN macroambitiattività ON ambitiattività.IDMacroAmbitoAttività = macroambitiattività.IDMacroAmbitoAttività"
        strsql = strsql & " ORDER BY  macroambitiattività.Codifica + ambitiattività.Codifica ,"
        strsql = strsql & " macroambitiattività.MacroAmbitoAttività "
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgElencoAttivita.DataSource = dtsgenerico
        Session("RisultatoElencoAttivita") = dtsgenerico
        dgElencoAttivita.DataBind()
    End Sub

    Private Sub SelezionaAmbitiAttivita(ByVal strIdTipoProgetto As String)
        Dim item As DataGridItem

        strsql = " SELECT aa.IdAmbitoAttività  " & _
         " FROM AssociaAmbitiTipiProgetto ap" & _
         " INNER JOIN AmbitiAttività aa on aa.IdAmbitoAttività = ap.IdAmbitoAttività " & _
         " WHERE Ap.IDTIPOPROGETTO = '" & strIdTipoProgetto & "'"

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        While dtrgenerico.Read()
            For Each item In dgElencoAttivita.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                If dtrgenerico("IdAmbitoAttività") = dgElencoAttivita.Items(item.ItemIndex).Cells(4).Text Then
                    check.Checked = True
                End If
            Next
        End While
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        CancellaAmbitiTipoProgetto(Request.QueryString("strIdTipoProgetto"))
        InserimentoAmbitiTipoProgetto(Request.QueryString("strIdTipoProgetto"))
        ClosePage()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        ClosePage()
    End Sub



    Private Sub ControllaCheck()
        'Generato da Simona CordelLa il 11/04/2006
        Dim item As DataGridItem
        If Hdd_chkFlag.Value = "" Then
            For Each item In dgElencoAttivita.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                check.Checked = True
                Hdd_chkFlag.Value = True
            Next
        Else
            For Each item In dgElencoAttivita.Items
                Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
                check.Checked = False
                Hdd_chkFlag.Value = ""
            Next
        End If
    End Sub

    Private Sub CancellaAmbitiTipoProgetto(ByVal IdProgetto As Integer)
        ' Generato da Simona Cordella il 11/04/2006
        'Elimino tutti i ambiti attività legati ad un certo tipo progetto
        CmdGenerico = New System.Data.SqlClient.SqlCommand
        CmdGenerico.Connection = Session("conn")

        strsql = "Delete from AssociaAmbitiTipiProgetto where idtipoProgetto=" & IdProgetto & ""
        CmdGenerico.CommandText = strsql
        CmdGenerico.ExecuteNonQuery()
    End Sub

    Private Sub InserimentoAmbitiTipoProgetto(ByVal IdProgetto As Integer)
        Dim item2 As DataGridItem
        ' Generato da Simona Cordella il 11/04/2006
        'Inserimento profili legati ad un certo tipo progetto
        CmdGenerico = New System.Data.SqlClient.SqlCommand
        CmdGenerico.Connection = Session("conn")
        For Each item2 In dgElencoAttivita.Items
            Dim check As CheckBox = DirectCast(item2.FindControl("check1"), CheckBox)
            If check.Checked = True Then
                strsql = "Insert into AssociaAmbitiTipiProgetto (IdTipoProgetto,IdAmbitoAttività) " & _
                        " Values ( " & IdProgetto & "," & dgElencoAttivita.Items(item2.ItemIndex).Cells(4).Text & ")"
                CmdGenerico.CommandText = strsql
                CmdGenerico.ExecuteNonQuery()
            End If
        Next
    End Sub

    Private Sub bloccaCheck()
        Dim item As DataGridItem
        For Each item In dgElencoAttivita.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            check.Enabled = False
        Next
    End Sub

    Sub ClosePage()
        'modificato il 31/05/2006 da simona cordella
        'gestione del nuovo campo descrizione abbreviata bando
        If Request.QueryString("TipoAzione") = "Inserimento" Then
            Response.Redirect("WfrmGestioneBandi.aspx?Hdd_rif=" & (Request.QueryString("Hdd_rif")) & " &Hdd_DataIVol=" & (Request.QueryString("Hdd_DataIVol")) & "&Hdd_DataFVol=" & (Request.QueryString("Hdd_DataFVol")) & "&Hdd_Check=" & (Request.QueryString("Hdd_Check")) & "&TipoAzione=" & (Request.QueryString("TipoAzione")) & "&Vengoda=" & (Request.QueryString("Vengoda")) & "&strGruppo=" & (Request.QueryString("strGruppo")) & "&strCheck=" & (Request.QueryString("strCheck")) & "&strDataFineVol= " & Request.QueryString("strDataFineVol") & "&strDataInizioVol=" & Request.QueryString("strDataInizioVol") & "&strDataFine= " & Request.QueryString("strDataFine") & "&strDataInizio=" & Request.QueryString("strDataInizio") & "&strImporto=" & Request.QueryString("strImporto") & "&strAnnoRif=" & Request.QueryString("strAnnoRif") & "&strRiferimento=" & Request.QueryString("strRiferimento") & "&strBandoBreve= " & Request.QueryString("strBandoBreve") & "&strBando=" & Request.QueryString("strBando") & "&strDataScadGrad=" & Request.QueryString("strDataScadGrad") & "&strNMaxVolontariProgettoItalia=" & Request.QueryString("strNMaxVolontariProgettoItalia") & "&strNMinVolontariProgettoItalia=" & Request.QueryString("strNMinVolontariProgettoItalia") & "&strNMaxVolontariProgettoEstero=" & Request.QueryString("strNMaxVolontariProgettoEstero") & "&strNMinVolontariProgettoEstero=" & Request.QueryString("strNMinVolontariProgettoEstero") & "&strNMinVolontariSedeItalia=" & Request.QueryString("strNMinVolontariSedeItalia") & "&strNMinVolontariSedeEstero=" & Request.QueryString("strNMinVolontariSedeEstero") & " ")
        Else
            Response.Redirect("WfrmGestioneBandi.aspx?Stato=" & (Request.QueryString("Stato")) & "&Pagina=" & (Request.QueryString("Pagina")) & " &strIdBando=" & (Request.QueryString("strIdBando")) & "&Hdd_rif=" & (Request.QueryString("Hdd_rif")) & " &Hdd_DataIVol=" & (Request.QueryString("Hdd_DataIVol")) & "&Hdd_DataFVol=" & (Request.QueryString("Hdd_DataFVol")) & "&Hdd_Check=" & (Request.QueryString("Hdd_Check")) & "&TipoAzione=" & (Request.QueryString("TipoAzione")) & "&Vengoda=" & (Request.QueryString("Vengoda")) & "&strGruppo=" & (Request.QueryString("strGruppo")) & "&strCheck=" & (Request.QueryString("strCheck")) & "&strDataFineVol= " & Request.QueryString("strDataFineVol") & "&strDataInizioVol=" & Request.QueryString("strDataInizioVol") & "&strDataFine= " & Request.QueryString("strDataFine") & "&strDataInizio=" & Request.QueryString("strDataInizio") & "&strImporto=" & Request.QueryString("strImporto") & "&strAnnoRif=" & Request.QueryString("strAnnoRif") & "&strRiferimento=" & Request.QueryString("strRiferimento") & "&strBandoBreve= " & Request.QueryString("strBandoBreve") & "&strBando=" & Request.QueryString("strBando") & "&strDataScadGrad=" & Request.QueryString("strDataScadGrad") & "&strNMaxVolontariProgettoItalia=" & Request.QueryString("strNMaxVolontariProgettoItalia") & "&strNMinVolontariProgettoItalia=" & Request.QueryString("strNMinVolontariProgettoItalia") & "&strNMaxVolontariProgettoEstero=" & Request.QueryString("strNMaxVolontariProgettoEstero") & "&strNMinVolontariProgettoEstero=" & Request.QueryString("strNMinVolontariProgettoEstero") & "&strNMinVolontariSedeItalia=" & Request.QueryString("strNMinVolontariSedeItalia") & "&strNMinVolontariSedeEstero=" & Request.QueryString("strNMinVolontariSedeEstero") & " ")
        End If
    End Sub
    Private Sub cmdSeleziona_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles chkSelDesel.CheckedChanged
        Dim chkSelVol As CheckBox
        If chkSelDesel.Checked = True Then
            For i = 0 To dgElencoAttivita.Items.Count - 1
                chkSelVol = dgElencoAttivita.Items(i).FindControl("check1")
                chkSelVol.Checked = True
            Next
            chkSelDesel.Text = "Deseleziona tutto"
        Else
            For i = 0 To dgElencoAttivita.Items.Count - 1
                chkSelVol = dgElencoAttivita.Items(i).FindControl("check1")
                chkSelVol.Checked = False
            Next
            chkSelDesel.Text = "Seleziona tutto"
        End If
    End Sub
End Class