Imports System.Data.SqlClient

Public Class WfrmAnnullaAccreditamento
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim cmdGenerico As SqlClient.SqlCommand
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
        If IsPostBack = False Then
            lblIdSede.Value = Context.Items("identesede")
            lblTipologia.Value = Context.Items("Tipologia")
            lblidEnte.Value = Context.Items("idente")
            lblEnte.Text = Context.Items("Ente")
            lblAcquisita.Value = Context.Items("acquisita")
            lblTipoAzione.Value = Context.Items("tipoazione")
            lblstatoSedeFisica.Value = Context.Items("StatoSedeFisica")
            If Not Context.Items("idSedeAttuazione") Is Nothing Then
                lblidSedeAtt.Value = Context.Items("idSedeAttuazione")
                lblsedeAtt.Visible = True
                lblSedeAttuazione.Text = Context.Items("SedeAtt")
                lblTitolo.Text = lblTitolo.Text & " Sede Attuazione"
            Else
                lblTitolo.Text = lblTitolo.Text & " Sede "
                lblsedeAtt.Visible = False
                lblSedeAttuazione.Visible = False
            End If
            strsql = "select denominazione from Enti where idente=" & lblidEnte.Value & ""
            ChiudiDataReader(dtrGenerico)
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrGenerico.Read()
            If dtrGenerico.HasRows = True Then
                lblEnte.Text = dtrGenerico("denominazione")
            End If
            'carico la lista degli stati diversi da attiva
            strsql = "select * from statiEntiSedi where attiva <> 1"
           ChiudiDataReader(dtrGenerico)
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            ddlStato.DataSource = dtrGenerico
            ddlStato.DataValueField = "idStatoEnteSede"
            ddlStato.DataTextField = "StatoEnteSede"
            ddlStato.DataBind()
            ChiudiDataReader(dtrGenerico)
            strsql = "Select * from EntiSedi Where identesede=" & lblIdSede.Value & ""
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrGenerico.Read()
            lblsede.Text = UCase(dtrGenerico("Denominazione"))
             ChiudiDataReader(dtrGenerico)
        End If
    End Sub
    Private Sub CronologiaStatiEntiSedi()
        'Realizzato da Alessandra Taballione il 21.06.2004
        'Prima di effettuare il cambio dello stato Inserisco il vecchio stato nella CronologiaEntiSedi
        strsql = "insert into CronologiaEntiSedi (identeSede,idstatoentesede,dataCronologia,idtipocronologia,UsernameStato)" & _
            " select " & lblIdSede.Value & ", idstatoEnteSede,getdate(),0,'" & Session("Utente") & "' " & _
            " from entiSedi where identesede= " & lblIdSede.Value & ""
        ChiudiDataReader(dtrGenerico)
        cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
    End Sub
    Private Sub CronologiaSedeAttuazione()
        'Prima di effettuare il cambio dello stato Inserisco il vecchio stato nella CronologiaEntiSedi
        strsql = "insert into CronologiaEntiSediAttuazione (identeSedeAttuazione,idstatoentesede,dataCronologia,idtipocronologia,UsernameStato)" & _
            " select " & lblidSedeAtt.Value & ", idstatoEnteSede,getdate(),0,'" & Session("Utente") & "' " & _
            " from entiSediAttuazioni where identesedeAttuazione= " & lblidSedeAtt.Value & ""
        ChiudiDataReader(dtrGenerico)
        cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
    End Sub
    Private Sub CronologiaStatiEntisediAttuazioni()
        'Realizzato da Alessandra Taballione il 21.06.2004
        'Prima di effettuare il cambio dello stato Inserisco il vecchio stato nella CronologiaEntiSediAttuazioni
        Dim sediAtt As New Collection
        Dim i As Integer
        strsql = "select * from entisediattuazioni where idEnteSede=" & lblIdSede.Value & " and idstatoEntesede=(Select idstatoentesede from statientisedi where attiva=1)"
         ChiudiDataReader(dtrGenerico)
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        Do While dtrGenerico.Read()
            sediAtt.Add(dtrGenerico("identesedeattuazione"))
        Loop
        For i = 1 To sediAtt.Count
            'Prima di effettuare il cambio dello stato Inserisco il vecchio stato nella CronologiaEntiSedi
            strsql = "insert into CronologiaEntiSediAttuazione (identeSedeAttuazione,idstatoentesede,dataCronologia,idtipocronologia,UsernameStato)" & _
                " select " & sediAtt.Item(i) & ", idstatoEnteSede,getdate(),0,'" & Session("Utente") & "' " & _
                " from entiSediAttuazioni where identesedeAttuazione= " & sediAtt.Item(i) & ""
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        Next
    End Sub
    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        If lblsedeAtt.Visible = True Then
            CronologiaSedeAttuazione()
            AnnullaAccreditamentoSedeAtt()
        Else
            CronologiaStatiEntiSedi()
            CronologiaStatiEntisediAttuazioni()
            AnnullaAccreditamentoSede()
        End If

    End Sub
    Private Sub AnnullaAccreditamentoSedeAtt()
        'Generato da Alessandra Taballione il 27/05/04
        'Eliminazione Logica Della Sede
        'Modificato da Simona Cordella il 12/12/2008
        'Gestione del campo Certificazione = 0(no) 
        strsql = "Update entiSediattuazioni set " & _
                 " idstatoenteSede=" & ddlStato.SelectedValue & ", " & _
                 " Certificazione =0 ," & _
                 " UserCertificazione ='" & Session("Utente") & "', " & _
                 " DataCertificazione =getdate()" & _
                 " Where identesedeattuazione=" & lblidSedeAtt.Value & " and identeSede=" & lblIdSede.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Context.Items.Add("identesede", lblIdSede.Value)
        Context.Items.Add("idente", lblidEnte.Value)
        Context.Items.Add("Ente", lblEnte.Text)
        Context.Items.Add("acquisita", lblAcquisita.Value)
        Context.Items.Add("tipoazione", "Modifica")
        Context.Items.Add("stato", lblstatoSedeFisica.Value)
        Server.Transfer("WfrmAnagraficaSedi.aspx")
    End Sub
    Private Sub AnnullaAccreditamentoSede()
        'Annulla Accreditamento della Sede 06/03/2004

        strsql = " update entisedi set idstatoenteSede =(select idstatoEnteSede from statiEntiSedi where idstatoEnteSede=" & ddlStato.SelectedValue & ") " & _
                            " where idEnteSede=" & lblIdSede.Value & " and idente=" & lblidEnte.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'Modificato da Simona Cordella il 12/12/2008
        'Gestione del campo Certificazione = 0(no) 
        strsql = "Update entiSediattuazioni set " & _
                 " idstatoenteSede=" & ddlStato.SelectedValue & ", " & _
                 " Certificazione =0 ," & _
                 " UserCertificazione ='" & Session("Utente") & "', " & _
                 " DataCertificazione =getdate()" & _
                 " Where identeSede=" & lblIdSede.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        cmdGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'Context.Items.Add("identesede", lblIdSede.Value)
        'Context.Items.Add("idente", lblidEnte.Value)
        'Context.Items.Add("Ente", lblEnte.Text)
        'Context.Items.Add("acquisita", lblAcquisita.Value)
        'Context.Items.Add("tipoazione", "Modifica")
        'Context.Items.Add("stato", ddlStato.SelectedItem.Text)
        'Server.Transfer("WfrmAnagraficaSedi.aspx")
        Response.Redirect("WfrmAnagraficaSedi.aspx?identesede=" & lblIdSede.Value & "&idente=" & lblidEnte.Value & "&acquisita=" & lblAcquisita.Value & "&stato=Accreditata")
    End Sub

    Public Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        'Context.Items.Add("identesede", lblIdSede.Value)
        'Context.Items.Add("idente", lblidEnte.Value)
        'Context.Items.Add("Ente", lblEnte.Text)
        'Context.Items.Add("acquisita", lblAcquisita.Value)
        'Context.Items.Add("tipoazione", "Modifica")
        'Context.Items.Add("stato", "Accreditata")
        ''Server.Transfer("WfrmAnagraficaSedi.aspx")
        Response.Redirect("WfrmAnagraficaSedi.aspx?identesede=" & lblIdSede.Value & "&idente=" & lblidEnte.Value & "&acquisita=" & lblAcquisita.Value & "&stato=Accreditata")
    End Sub

End Class