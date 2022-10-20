Imports System.Data.SqlClient

Public Class WfrmRicercaBandiQuestionario
    Inherits System.Web.UI.Page
    Dim dtsgenerico As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim azione As String
    Dim INSERIMENTO As String = "Inserimento"
    Dim query As String
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        azione = Request.QueryString("VengoDa")
        If (azione = INSERIMENTO) Then
            lblTitolo.Text = "Ricerca bandi per inserimento questionario"
        Else
            lblTitolo.Text = "Ricerca bandi per modifica questionario"
        End If

        If IsPostBack = False Then
            Call PopolaCombo()
        End If
    End Sub

#Region "Funzionalità"
    Private Sub PopolaCombo()
        Dim strSql As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        '        strSql = "SELECT 0 AS idbando, '' AS bandobreve from bando Where FormazioneGenerale=1 " & _
        '                 "UNION select idbando, bandobreve from bando Where FormazioneGenerale=1 ORDER BY idbando"
        strSql = "SELECT 0 AS idbando, '' AS bandobreve ,'2500' AS annobreve  "
        strSql = strSql & " UNION "
        strSql = strSql & " SELECT DISTINCT Bando.idBando,bando.bandobreve,bando.annobreve "
        strSql = strSql & " FROM bando"
        strSql = strSql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strSql = strSql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strSql = strSql & " WHERE Bando.FormazioneGenerale=1 and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strSql = strSql & " ORDER BY 3 desc"

        dtrLeggiDati = ClsServer.CreaDatareader(strSql, Session("conn"))
        ddlBando.DataSource = dtrLeggiDati
        ddlBando.DataValueField = "idbando"
        ddlBando.DataTextField = "bandobreve"
        ddlBando.DataBind()

        dtrLeggiDati.Close()
        dtrLeggiDati = Nothing

    End Sub

    Sub CaricaDataGrid(ByRef datagrid As DataGrid, ByRef dataset As DataSet)
        datagrid.DataSource = dataset
        Session("appDtsRisRicerca") = dataset
        datagrid.DataBind()
        datagrid.Visible = True
        If (dtsgenerico.Tables(0).Rows.Count > 0) Then
            datagrid.Caption = "Risultato Ricerca Bandi"
        Else
            datagrid.Caption = "La ricerca non ha prodotto risultati"
        End If
    End Sub
    Private Sub RicercaBandi()
        query = "select distinct bando.idbando,bando," & _
            " case isnull(importoStanziato,-1) when -1 then '0.00' else importoStanziato end as importostanziato ,statobando, " & _
            " '' as color, " & _
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
            " case AssociazioneAutomatica when 1 then 'Si'else 'No' end as AssociazioneAutomatica, bando.RevisioneFormazione " & _
            " FROM attivitàentisediattuazione INNER JOIN " & _
            " attività a ON attivitàentisediattuazione.IDAttività = a.IDAttività INNER JOIN " & _
            " BandiAttività ON a.IDBandoAttività = BandiAttività.IdBandoAttività INNER JOIN " & _
            " bando ON BandiAttività.IdBando = bando.IDBando INNER JOIN " & _
            " attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
            " entità ON attivitàentità.IDEntità = entità.IDEntità INNER JOIN " & _
            " statiBando ON bando.IDStatoBando = statiBando.IDStatoBando " & _
            " inner join attivitàformazionegenerale afg on a.idattività = afg.idattività" & _
            " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando" & _
            " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto " & _
            " Where isnull(afg.TipoFormazioneGenerale,1) =" & ddlTipoFormazioneGenerale.SelectedValue & "  and BandiAttività.idente=" & Session("idente") & " " & _
            " and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'   " & _
            " and (SELECT  ISNULL(SUM(entità.OreFormazione), 0)  " & _
            " FROM  attivitàentità INNER JOIN " & _
            " entità ON attivitàentità.IDEntità = entità.IDEntità INNER JOIN " & _
            " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
            " BandiAttività INNER JOIN " & _
            " attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
            " INNER JOIN AttivitàFormazioneGenerale ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività " & _
            " WHERE attività.IDAttività = a.IDAttività AND AttivitàFormazioneGenerale.StatoFormazione IN (1,2,3)) > 0 "

        If ddlBando.SelectedItem.Text <> "" Then
            query = query & " and bando.idBando = " & ddlBando.SelectedValue
        Else
            query = query & " and bando.idBando in (Select idbando From bando Where FormazioneGenerale=1)"
        End If

        'faccio vedere solo i bandi per i quali non è stato eseguito il questionario
        If Request.QueryString("VengoDa") = "Inserimento" Then
            query = query & " and dbo.Formazione_TerminiQuestionario_2(BandiAttività.IdBandoAttività, getdate()," & ddlTipoFormazioneGenerale.SelectedValue & ") = 'SI' " & _
                              " and BandiAttività.idbandoattività not in " & _
                              " (SELECT Q.IDBANDOATTIVITà  FROM QuestionarioStoricoProgetti q " & _
                              " INNER JOIN BandiAttività b ON B.IDBANDOATTIVITà = Q.IDBANDOATTIVITà " & _
                              " WHERE B.idente=" & Session("idente") & " and isnull(TipoFormazioneGenerale,1) =" & ddlTipoFormazioneGenerale.SelectedValue & " )"
        Else
            'faccio vedere solo i bandi per i quali  è stato eseguito il questionario
            query = query & " and BandiAttività.idbandoattività in " & _
                              " (SELECT Q.IDBANDOATTIVITà  FROM QuestionarioStoricoProgetti q " & _
                              " INNER JOIN BandiAttività b ON B.IDBANDOATTIVITà = Q.IDBANDOATTIVITà " & _
                              " WHERE B.idente=" & Session("idente") & " and isnull(TipoFormazioneGenerale,1) =" & ddlTipoFormazioneGenerale.SelectedValue & " )"
        End If
        ChiudiDataReader(dtrgenerico)
        dtsgenerico = ClsServer.DataSetGenerico(query, Session("conn"))

        CaricaDataGrid(dgRisultatoRicerca, dtsgenerico)


    End Sub

#End Region
#Region "Eventi"
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
   
    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        lblmessaggio.Text = ""
        If ddlTipoFormazioneGenerale.SelectedItem.Text = "" Then
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Selezionare una tipologia di Erogazione della Formazione."
            Exit Sub
        End If

        dgRisultatoRicerca.CurrentPageIndex = 0
        RicercaBandi()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand

        If e.CommandName = "Seleziona" Then
            Dim revisioneFormazione As String = e.Item.Cells(11).Text
            Session("idbando") = e.Item.Cells(5).Text
            'Context.Items.Add("idbando", e.Item.Cells(5).Text)
            Context.Items.Add("tipoazione", Request.QueryString("VengoDa"))
            If revisioneFormazione = "2" Or revisioneFormazione = "3" Then
                Response.Redirect("WfrmQuestionarioProgettoRev2.aspx?TipoFormazioneGenerale=" & ddlTipoFormazioneGenerale.SelectedValue & " &RevisioneFormazione=" & revisioneFormazione)
            Else
                If revisioneFormazione = "1" Then
                    Response.Redirect("WfrmQuestionarioProgettoSpec.aspx?TipoFormazioneGenerale=" & ddlTipoFormazioneGenerale.SelectedValue & " &RevisioneFormazione=" & revisioneFormazione)
                ElseIf revisioneFormazione = "0" Then
                    Response.Redirect("WfrmQuestionarioProgetto.aspx?TipoFormazioneGenerale=" & ddlTipoFormazioneGenerale.SelectedValue & " &RevisioneFormazione=" & revisioneFormazione)
                End If
            End If
        End If
    End Sub

#End Region

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        'dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dgRisultatoRicerca.DataBind()
        dgRisultatoRicerca.SelectedIndex = -1
        'RicercaBandi()
    End Sub
End Class