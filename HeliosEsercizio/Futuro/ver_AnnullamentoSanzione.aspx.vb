Imports System.IO
Imports System.Drawing
Public Class ver_AnnullamentoSanzione
    Inherits System.Web.UI.Page
    Shared idVerifica As Integer
    Shared idEnte As Integer
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents LblTipoAnnullamento As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlTipoAnnullamento As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents LblAutoritaAnnullamento As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlAutoritaAnnullamento As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtNumProtCredenziali As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LblDataProtAnn As System.Web.UI.WebControls.Label
    'Protected WithEvents LblNumProtAnnullamento As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDataProtAnnullamento As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtNumProtAnnullamento As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlTipoRipristino As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents LblAutoritaRipristino As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlAutoritaRipristino As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents LblDataProtRipristino As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDataProtRipristino As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtNumProtRipristino As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ImgProtolloRipristino As System.Web.UI.WebControls.Image
    'Protected WithEvents ImgApriAllegatiRipristino As System.Web.UI.WebControls.Image
    'Protected WithEvents ImgProtocollazioneRipristino As System.Web.UI.WebControls.Image
    'Protected WithEvents LblNoteRipristino As System.Web.UI.WebControls.Label
    'Protected WithEvents LblTipoRipristino As System.Web.UI.WebControls.Label
    'Protected WithEvents LblNumProtRipristino As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdSalva As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents ImgProtocolloAnnullamento As System.Web.UI.WebControls.Image
    'Protected WithEvents ImgProtocollazioneAnnullamento As System.Web.UI.WebControls.Image
    'Protected WithEvents imgDataProtAnnullamento As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents ImgApriAllegatiAnnullamento As System.Web.UI.WebControls.Image
    'Protected WithEvents TxtCodiceFasc As System.Web.UI.WebControls.TextBox

    'Protected WithEvents TxtNumFascicolo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtDescrFascicolo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents imgDataProtRipristino As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents TxtSanzione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtTipoSanzione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtSoggettoSanzione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtStatoSanzione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtDataProtInvioSanzione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtNumProtInvioSanzione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtUtenteEsecutoreSanzione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtUserAnnullamento As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtDataRipristino As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtUserRipristino As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtDataAnnullamento As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label
    'Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    '** CREATA DA SIMONA CORDELLA IL 19/10/2011
    '** GESTIONE ANNULLAMENTE / RIPRISTINO SANZIONE

#Region "Funzioni e sub Maschera"
    Sub Intestazione()
        Visibilità_PulsantiSIGEDAnnullamento(Session("IdRegCompetenza"))
        Visibilità_PulsantiSIGEDRipristino(Session("IdRegCompetenza"))

        DataProtAnnullamento(False, Color.LightGray)
        DataProtRipristino(False, Color.LightGray)
        ImageProtocolloAnnullamento(False)
        ImageProtocolloRipristino(False)

        'If Session("IdRegCompetenza") = 22 Then
        '    Abilito_DisabilitoAnnullamento(True, Color.White)
        '    Abilito_DisabilitoRipristino(True, Color.White)
        'Else
        '    Abilito_DisabilitoAnnullamento(True, Color.White)
        '    Abilito_DisabilitoRipristino(True, Color.White)
        'End If
        
        CaricaInfoSanzione(idVerifica, Request.QueryString("IdA"), Request.QueryString("IdE"), Request.QueryString("IdES"), Request.QueryString("IdESA"), Request.QueryString("IdAESA"))

        Select Case TxtStatoSanzione.Text
            Case "Sanzione Valida"
                'annullamento abilitato
                'ripritsina disabilitato
                DataProtAnnullamento(True, Color.White)
                ImageProtocolloAnnullamento(True)
                AbilitoDisabilitoOggettiDropDownList(True, False)
                'Abilito_DisabilitoRipristino(False, Color.LightGray)
            Case "Sanzione Annullata"
                'annullamento disabilitato
                'ripritsina abilitato
                DataProtRipristino(True, Color.White)
                ImageProtocolloRipristino(True)
                AbilitoDisabilitoOggettiDropDownList(False, True)
            Case "Sanzione Ripristinata"
                cmdSalva.Visible = False

                AbilitoDisabilitoOggettiDropDownList(False, False)
                txtNote.Enabled = False
        End Select
        If Session("IdRegCompetenza") <> 22 Then
            ImageProtocolloAnnullamento(False)
            ImageProtocolloRipristino(False)
        End If

    End Sub
    Private Sub CaricaInfoSanzione(ByVal IdVerifica As Integer, ByVal IdA As String, ByVal IDE As String, ByVal IdES As String, ByVal IdESA As String, ByVal IdAESA As String)
        'Aggiunto il 21/10/2011 da Simona Cordella
        Dim Item As DataGridItem
        Dim StrSql As String
        Dim dtSanzione As New DataTable
        Dim drSanzione As DataRow
        Dim DtrSanzione As SqlClient.SqlDataReader

        If Not DtrSanzione Is Nothing Then
            DtrSanzione.Close()
            DtrSanzione = Nothing
        End If
        StrSql = " Select isnull(idfascicolo,'') as idfascicolo,isnull(codicefascicolo,'') as codicefascicolo,isnull(DescrFascicolo,'') as DescrFascicolo, " & _
                 " NProtEsecuzioneSanzione," & _
                 " dbo.FormatoData(DataProtEsecuzioneSanzione) as DataProtEsecuzioneSanzione from tverifiche " & _
                 " where IDVerifica = " & IdVerifica & ""
        DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        If DtrSanzione.HasRows = True Then
            DtrSanzione.Read()
            TxtNumFascicolo.Text = "" & Replace(DtrSanzione("codicefascicolo"), " ", "")
            hfTxtCodiceFasc.Value = "" & DtrSanzione("idfascicolo")
            TxtDescrFascicolo.Text = "" & DtrSanzione("DescrFascicolo")
            TxtDataProtInvioSanzione.Text = "" & DtrSanzione("DataProtEsecuzioneSanzione")
            TxtNumProtInvioSanzione.Text = "" & DtrSanzione("NProtEsecuzioneSanzione")
        End If
        If Not DtrSanzione Is Nothing Then
            DtrSanzione.Close()
            DtrSanzione = Nothing
        End If
        'CARICO DATI PROGETTO
        If IdA <> 0 Then
            StrSql = " SELECT Distinct attività.CodiceEnte, (attività.Titolo + ' (' + attività.CodiceEnte + ')') as Titolo, " & _
                    " TVerificheSanzioniProgetto.IDSanzioneProgetto, " & _
                    " TVerificheSanzioni.Descrizione as Sanzione,  " & _
                    " TVerificheSanzioniProgetto.UserAnnullaSanzione,dbo.FormatoData(TVerificheSanzioniProgetto.DataAnnullamentoSanzione) as DataAnnullamentoSanzione," & _
                    " TVerificheSanzioniProgetto.UserRipristinoSanzione,dbo.FormatoData(TVerificheSanzioniProgetto.DataRipristinoSanzione) as DataRipristinoSanzione,  " & _
                    " TVerificheSanzioniProgetto.UtenteEsecutore,isnull(TVerificheSanzioniProgetto.TipoAnnullamento,0) as TipoAnnullamento, " & _
                    " TVerificheSanzioniProgetto.NProtAnnullamentoSanzione,TVerificheSanzioniProgetto.DataProtAnnullamentoSanzione," & _
                    " isnull(TVerificheSanzioniProgetto.IdAutoritaAnnullamento,0) as IdAutoritaAnnullamento, " & _
                    " isnull(TVerificheSanzioniProgetto.TipoRipristino,0) as TipoRipristino, TVerificheSanzioniProgetto.NProtRipristinoSanzione, " & _
                    " TVerificheSanzioniProgetto.DataProtRipristinoSanzione,isnull(TVerificheSanzioniProgetto.IdAutoritaRipristino,0) as IdAutoritaRipristino, TVerificheSanzioniProgetto.Note, " & _
                    " (CASE isnull(TVerificheSanzioniProgetto.StatoSanzione,0) " & _
                    " WHEN 0 THEN 'Sanzione Valida' " & _
                    " WHEN 1 THEN 'Sanzione Annullata' " & _
                    " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                    " FROM TVerificheSanzioniProgetto " & _
                    " LEFT JOIN TVerificheSanzioni on TVerificheSanzioniProgetto.IDSanzione = TVerificheSanzioni.IDSanzione  " & _
                    " inner join attività on attività.idattività = TVerificheSanzioniProgetto.idattività " & _
                    " WHERE TVerificheSanzioniProgetto.IDVerifica = " & IdVerifica & " and TVerificheSanzioniProgetto.IdSanzione = " & Request.QueryString("IDSanzione") & ""
            DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
            If DtrSanzione.HasRows = True Then
                DtrSanzione.Read()

                'richiamo funzione che popola le text e le dropdownlist della maschera
                CaricaOggettiMaschera(DtrSanzione("Sanzione"), DtrSanzione("Titolo"), "Progetto", "" & DtrSanzione("StatoSanzione"), "" & DtrSanzione("DataAnnullamentoSanzione"), "" & DtrSanzione("UserAnnullaSanzione"), "" & DtrSanzione("UserRipristinoSanzione"), "" & DtrSanzione("DataRipristinoSanzione"), "" & DtrSanzione("UtenteEsecutore"), "" & DtrSanzione("TipoAnnullamento"), "" & DtrSanzione("NProtAnnullamentoSanzione"), "" & DtrSanzione("DataProtAnnullamentoSanzione"), "" & DtrSanzione("IdAutoritaAnnullamento"), "" & DtrSanzione("TipoRipristino"), "" & DtrSanzione("NProtRipristinoSanzione"), "" & DtrSanzione("DataProtRipristinoSanzione"), "" & DtrSanzione("IdAutoritaRipristino"), "" & DtrSanzione("Note"))
            End If
            If Not DtrSanzione Is Nothing Then
                DtrSanzione.Close()
                DtrSanzione = Nothing
            End If
        ElseIf IDE <> 0 Then
            'CARICO DATI ENTE
            StrSql = " SELECT  DISTINCT " & _
                    " enti.Denominazione  + ' (' + enti.CodiceRegione + ')'  AS Denominazione, " & _
                    " TVerificheSanzioniEnte.IDEnte, TVerificheSanzioniEnte.IDSanzioneEnte, TVerificheSanzioniEnte.IDSanzione,TVerificheSanzioni.Descrizione as Sanzione, " & _
                    " TVerificheSanzioniEnte.UserAnnullaSanzione,dbo.FormatoData(TVerificheSanzioniEnte.DataAnnullamentoSanzione) as DataAnnullamentoSanzione," & _
                    " TVerificheSanzioniEnte.UserRipristinoSanzione,dbo.FormatoData(TVerificheSanzioniEnte.DataRipristinoSanzione) as DataRipristinoSanzione,  " & _
                    " TVerificheSanzioniEnte.UtenteEsecutore,ISNULL(TVerificheSanzioniEnte.TipoAnnullamento,0) AS TipoAnnullamento, " & _
                    " TVerificheSanzioniEnte.NProtAnnullamentoSanzione,TVerificheSanzioniEnte.DataProtAnnullamentoSanzione," & _
                    " isnull(TVerificheSanzioniEnte.IdAutoritaAnnullamento,0) as IdAutoritaAnnullamento, " & _
                    " ISNULL(TVerificheSanzioniEnte.TipoRipristino,0) AS TipoRipristino, TVerificheSanzioniEnte.NProtRipristinoSanzione, " & _
                    " TVerificheSanzioniEnte.DataProtRipristinoSanzione, isnull(TVerificheSanzioniEnte.IdAutoritaRipristino,0) as IdAutoritaRipristino, TVerificheSanzioniEnte.Note, " & _
                    " (CASE isnull(TVerificheSanzioniEnte.StatoSanzione,0) " & _
                    " WHEN 0 THEN 'Sanzione Valida' " & _
                    " WHEN 1 THEN 'Sanzione Annullata' " & _
                    " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                    " FROM TVerificheSanzioniEnte " & _
                    " INNER JOIN TVerificheSanzioni on TVerificheSanzioniEnte.IDSanzione = TVerificheSanzioni.IDSanzione " & _
                    " INNER JOIN TVerifiche ON TVerifiche.IDVerifica=TVerificheSanzioniEnte.IDVerifica " & _
                    " INNER JOIN enti ON enti.IDEnte =TVerificheSanzioniEnte.IDENTE " & _
                    " WHERE TVerificheSanzioniEnte.IDVerifica = " & IdVerifica & " and TVerificheSanzioniEnte.IdSanzione = " & Request.QueryString("IDSanzione") & " "



            DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
            If DtrSanzione.HasRows = True Then
                DtrSanzione.Read()

                'richiamo funzione che popola le text e le dropdownlist della maschera
                CaricaOggettiMaschera(DtrSanzione("Sanzione"), DtrSanzione("Denominazione"), "Ente", "" & DtrSanzione("StatoSanzione"), "" & DtrSanzione("DataAnnullamentoSanzione"), "" & DtrSanzione("UserAnnullaSanzione"), "" & DtrSanzione("UserRipristinoSanzione"), "" & DtrSanzione("DataRipristinoSanzione"), "" & DtrSanzione("UtenteEsecutore"), "" & DtrSanzione("TipoAnnullamento"), "" & DtrSanzione("NProtAnnullamentoSanzione"), "" & DtrSanzione("DataProtAnnullamentoSanzione"), "" & DtrSanzione("IdAutoritaAnnullamento"), "" & DtrSanzione("TipoRipristino"), "" & DtrSanzione("NProtRipristinoSanzione"), "" & DtrSanzione("DataProtRipristinoSanzione"), "" & DtrSanzione("IdAutoritaRipristino"), "" & DtrSanzione("Note"))
            End If
            If Not DtrSanzione Is Nothing Then
                DtrSanzione.Close()
                DtrSanzione = Nothing
            End If
        ElseIf IdES <> 0 Then
            'CARICO DATI SEDE
            StrSql = " Select DISTINCT DenEnteSede  + ' (' + CONVERT(varchar, vista.identesede) + ')' as DenEnteSede, " & _
                    " vista.identesede,TVerificheSanzioniSede.IDSanzioneSede, TVerificheSanzioniSede.IDSanzione, " & _
                    " UserAnnullaSanzione,dbo.FormatoData(DataAnnullamentoSanzione) as DataAnnullamentoSanzione," & _
                    " TVerificheSanzioni.Descrizione as Sanzione, " & _
                    " TVerificheSanzioniSede.UserAnnullaSanzione,dbo.FormatoData(TVerificheSanzioniSede.DataAnnullamentoSanzione) as DataAnnullamentoSanzione," & _
                    " TVerificheSanzioniSede.UserRipristinoSanzione,dbo.FormatoData(TVerificheSanzioniSede.DataRipristinoSanzione) as DataRipristinoSanzione,  " & _
                    " TVerificheSanzioniSede.UtenteEsecutore,ISNULL(TVerificheSanzioniSede.TipoAnnullamento,0) AS TipoAnnullamento, " & _
                    " TVerificheSanzioniSede.NProtAnnullamentoSanzione,TVerificheSanzioniSede.DataProtAnnullamentoSanzione," & _
                    " isnull(TVerificheSanzioniSede.IdAutoritaAnnullamento,0) as IdAutoritaAnnullamento, " & _
                    " ISNULL(TVerificheSanzioniSede.TipoRipristino,0) AS TipoRipristino, TVerificheSanzioniSede.NProtRipristinoSanzione, " & _
                    " TVerificheSanzioniSede.DataProtRipristinoSanzione, isnull(TVerificheSanzioniSede.IdAutoritaRipristino,0) as IdAutoritaRipristino, TVerificheSanzioniSede.Note," & _
                    " (CASE isnull(TVerificheSanzioniSede.StatoSanzione,0) " & _
                    " WHEN 0 THEN 'Sanzione Valida' " & _
                    " WHEN 1 THEN 'Sanzione Annullata' " & _
                    " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                    " FROM ver_vw_ricerca_verifiche as vista " & _
                    " inner join TVerificheSanzioniSede on vista.identesede =TVerificheSanzioniSede.identesede " & _
                    " INNER JOIN TVerificheSanzioni on TVerificheSanzioniSede.IDSanzione = TVerificheSanzioni.IDSanzione " & _
                    " WHERE TVerificheSanzioniSede.idverifica = " & IdVerifica & " and TVerificheSanzioniSede.IdSanzione = " & Request.QueryString("IDSanzione") & " "

            DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
            If DtrSanzione.HasRows = True Then
                DtrSanzione.Read()

                'richiamo funzione che popola le text e le dropdownlist della maschera
                CaricaOggettiMaschera(DtrSanzione("Sanzione"), DtrSanzione("DenEnteSede"), "Sedi", "" & DtrSanzione("StatoSanzione"), "" & DtrSanzione("DataAnnullamentoSanzione"), "" & DtrSanzione("UserAnnullaSanzione"), "" & DtrSanzione("UserRipristinoSanzione"), "" & DtrSanzione("DataRipristinoSanzione"), "" & DtrSanzione("UtenteEsecutore"), "" & DtrSanzione("TipoAnnullamento"), "" & DtrSanzione("NProtAnnullamentoSanzione"), "" & DtrSanzione("DataProtAnnullamentoSanzione"), "" & DtrSanzione("IdAutoritaAnnullamento"), "" & DtrSanzione("TipoRipristino"), "" & DtrSanzione("NProtRipristinoSanzione"), "" & DtrSanzione("DataProtRipristinoSanzione"), "" & DtrSanzione("IdAutoritaRipristino"), "" & DtrSanzione("Note"))
            End If
            If Not DtrSanzione Is Nothing Then
                DtrSanzione.Close()
                DtrSanzione = Nothing
            End If
        ElseIf IdESA <> 0 Then
            'CARICO DATI SEDE ATTUAZIONE
            StrSql = " Select DISTINCT vista.identesedeattuazione, " & _
                    " entisediattuazioni.denominazione + ' (' + CONVERT(varchar, vista.identesedeattuazione) + ')' as denominazione, " & _
                    " TVerificheSanzioniSedeAttuazione.IDSanzioneSedeAttuazione, " & _
                    " TVerificheSanzioniSedeAttuazione.IDSanzione,TVerificheSanzioni.Descrizione as Sanzione,   " & _
                    " TVerificheSanzioniSedeAttuazione.UserAnnullaSanzione,dbo.FormatoData(TVerificheSanzioniSedeAttuazione.DataAnnullamentoSanzione) as DataAnnullamentoSanzione," & _
                    " TVerificheSanzioniSedeAttuazione.UserRipristinoSanzione,dbo.FormatoData(TVerificheSanzioniSedeAttuazione.DataRipristinoSanzione) as DataRipristinoSanzione,  " & _
                    " TVerificheSanzioniSedeAttuazione.UtenteEsecutore,ISNULL(TVerificheSanzioniSedeAttuazione.TipoAnnullamento,0) AS TipoAnnullamento, " & _
                    " TVerificheSanzioniSedeAttuazione.NProtAnnullamentoSanzione,TVerificheSanzioniSedeAttuazione.DataProtAnnullamentoSanzione," & _
                    " isnull(TVerificheSanzioniSedeAttuazione.IdAutoritaAnnullamento,0) as IdAutoritaAnnullamento, " & _
                    " ISNULL(TVerificheSanzioniSedeAttuazione.TipoRipristino,0) AS TipoRipristino, TVerificheSanzioniSedeAttuazione.NProtRipristinoSanzione, " & _
                    " TVerificheSanzioniSedeAttuazione.DataProtRipristinoSanzione, isnull(TVerificheSanzioniSedeAttuazione.IdAutoritaRipristino,0) as IdAutoritaRipristino,TVerificheSanzioniSedeAttuazione.Note, " & _
                    " (CASE isnull(TVerificheSanzioniSedeAttuazione.StatoSanzione,0) " & _
                    " WHEN 0 THEN 'Sanzione Valida' " & _
                    " WHEN 1 THEN 'Sanzione Annullata' " & _
                    " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                    " FROM ver_vw_ricerca_verifiche as vista   " & _
                    " INNER JOIN TVerificheSanzioniSedeAttuazione on  vista.identesedeattuazione =TVerificheSanzioniSedeAttuazione.identesedeattuazione " & _
                    " INNER JOIN entisediattuazioni  on entisediattuazioni.identesedeattuazione = vista.identesedeattuazione " & _
                    " INNER JOIN TVerificheSanzioni on TVerificheSanzioniSedeAttuazione.IDSanzione = TVerificheSanzioni.IDSanzione " & _
                    " WHERE TVerificheSanzioniSedeAttuazione.IDVerifica  = " & IdVerifica & " and TVerificheSanzioniSedeAttuazione.IdSanzione = " & Request.QueryString("IDSanzione") & " "
            DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
            If DtrSanzione.HasRows = True Then
                DtrSanzione.Read()

                'richiamo funzione che popola le text e le dropdownlist della maschera
                CaricaOggettiMaschera(DtrSanzione("Sanzione"), DtrSanzione("denominazione"), "Sedi Attuazione", "" & DtrSanzione("StatoSanzione"), "" & DtrSanzione("DataAnnullamentoSanzione"), "" & DtrSanzione("UserAnnullaSanzione"), "" & DtrSanzione("UserRipristinoSanzione"), "" & DtrSanzione("DataRipristinoSanzione"), "" & DtrSanzione("UtenteEsecutore"), "" & DtrSanzione("TipoAnnullamento"), "" & DtrSanzione("NProtAnnullamentoSanzione"), "" & DtrSanzione("DataProtAnnullamentoSanzione"), "" & DtrSanzione("IdAutoritaAnnullamento"), "" & DtrSanzione("TipoRipristino"), "" & DtrSanzione("NProtRipristinoSanzione"), "" & DtrSanzione("DataProtRipristinoSanzione"), "" & DtrSanzione("IdAutoritaRipristino"), "" & DtrSanzione("Note"))
            End If
            If Not DtrSanzione Is Nothing Then
                DtrSanzione.Close()
                DtrSanzione = Nothing
            End If
        ElseIf IdAESA <> 0 Then
            'CARICO DATI SEDE PROGETTO
            StrSql = "SELECT DISTINCT TVerificheSanzioniSedeProgetto.IDAttivitàSedeAttuazione, " & _
                    " attivitàentisediattuazione.IDEnteSedeAttuazione, " & _
                    " entisediattuazioni.Denominazione  + ' (' + CONVERT(varchar, entisediattuazioni.identesedeattuazione) + ')'  AS DenomSede," & _
                    " TVerificheSanzioniSedeProgetto.IDSanzione,TVerificheSanzioni.Descrizione as Sanzione, " & _
                    " Attività.Titolo + ' (' +  Attività.codiceEnte + ')' as progetto ,attività.idAttività,  " & _
                    " TVerificheSanzioniSedeProgetto.UserAnnullaSanzione,dbo.FormatoData(TVerificheSanzioniSedeProgetto.DataAnnullamentoSanzione) as DataAnnullamentoSanzione," & _
                    " TVerificheSanzioniSedeProgetto.UserRipristinoSanzione,dbo.FormatoData(TVerificheSanzioniSedeProgetto.DataRipristinoSanzione) as DataRipristinoSanzione,  " & _
                    " TVerificheSanzioniSedeProgetto.UtenteEsecutore,ISNULL(TVerificheSanzioniSedeProgetto.TipoAnnullamento,0) AS TipoAnnullamento, " & _
                    " TVerificheSanzioniSedeProgetto.NProtAnnullamentoSanzione,TVerificheSanzioniSedeProgetto.DataProtAnnullamentoSanzione," & _
                    " isnull(TVerificheSanzioniSedeProgetto.IdAutoritaAnnullamento,0) as IdAutoritaAnnullamento, " & _
                    " ISNULL(TVerificheSanzioniSedeProgetto.TipoRipristino,0) AS TipoRipristino, TVerificheSanzioniSedeProgetto.NProtRipristinoSanzione, " & _
                    " TVerificheSanzioniSedeProgetto.DataProtRipristinoSanzione, isnull(TVerificheSanzioniSedeProgetto.IdAutoritaRipristino,0) as IdAutoritaRipristino,TVerificheSanzioniSedeProgetto.Note, " & _
                    " (CASE isnull(TVerificheSanzioniSedeProgetto.StatoSanzione,0) " & _
                    " WHEN 0 THEN 'Sanzione Valida' " & _
                    " WHEN 1 THEN 'Sanzione Annullata' " & _
                    " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                    " FROM TVerificheSanzioniSedeProgetto  " & _
                    " INNER JOIN  attivitàentisediattuazione ON TVerificheSanzioniSedeProgetto.IDAttivitàSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione  " & _
                    " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione  " & _
                    " INNER JOIN Attività on attivitàentisediattuazione.idAttività = Attività.idAttività  " & _
                    " INNER JOIN TVerificheSanzioni on TVerificheSanzioniSedeProgetto.IDSanzione = TVerificheSanzioni.IDSanzione " & _
                    " WHERE TVerificheSanzioniSedeProgetto.IDVerifica = " & IdVerifica & " and TVerificheSanzioniSedeProgetto.IdSanzione = " & Request.QueryString("IDSanzione") & " "

            DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
            If DtrSanzione.HasRows = True Then
                DtrSanzione.Read()

                'richiamo funzione che popola le text e le dropdownlist della maschera
                CaricaOggettiMaschera(DtrSanzione("Sanzione"), DtrSanzione("progetto"), "Sedi di Progetto", "" & DtrSanzione("StatoSanzione"), "" & DtrSanzione("DataAnnullamentoSanzione"), "" & DtrSanzione("UserAnnullaSanzione"), "" & DtrSanzione("UserRipristinoSanzione"), "" & DtrSanzione("DataRipristinoSanzione"), "" & DtrSanzione("UtenteEsecutore"), "" & DtrSanzione("TipoAnnullamento"), "" & DtrSanzione("NProtAnnullamentoSanzione"), "" & DtrSanzione("DataProtAnnullamentoSanzione"), "" & DtrSanzione("IdAutoritaAnnullamento"), "" & DtrSanzione("TipoRipristino"), "" & DtrSanzione("NProtRipristinoSanzione"), "" & DtrSanzione("DataProtRipristinoSanzione"), "" & DtrSanzione("IdAutoritaRipristino"), "" & DtrSanzione("Note"))
            End If
            If Not DtrSanzione Is Nothing Then
                DtrSanzione.Close()
                DtrSanzione = Nothing
            End If
        End If
    End Sub
    Sub CaricaAutorità(ByVal objDDLAutorita As DropDownList)
        Dim dtrAutorita As System.Data.SqlClient.SqlDataReader
        Dim strSql As String
        Try
            objDDLAutorita.Items.Clear()
            strSql = "SELECT idAutorita,Autorita " & _
                    " FROM TLegaleAutorita " & _
                    " WHERE  Abilitato=0 " & _
                    " union SELECT 0 ,'' FROM TLegaleAutorita " & _
                    " WHERE  Abilitato=0 "
            dtrAutorita = ClsServer.CreaDatareader(strSql, Session("conn"))

            objDDLAutorita.DataSource = dtrAutorita
            objDDLAutorita.DataTextField = "Autorita"
            objDDLAutorita.DataValueField = "IDAutorita"
            objDDLAutorita.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        Finally
            If Not dtrAutorita Is Nothing Then
                dtrAutorita.Close()
                dtrAutorita = Nothing
            End If
        End Try
    End Sub


    Private Sub DataProtAnnullamento(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtAnnullamento.Enabled = blnValore
        txtNumProtAnnullamento.Enabled = blnValore

        txtDataProtAnnullamento.BackColor = Colore
        txtNumProtAnnullamento.BackColor = Colore
    End Sub
    Private Sub DataProtRipristino(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtRipristino.Enabled = blnValore
        txtNumProtRipristino.Enabled = blnValore

        txtDataProtRipristino.BackColor = Colore
        txtNumProtRipristino.BackColor = Colore
    End Sub

    Private Sub ImageProtocolloAnnullamento(ByVal blnValore As Boolean)
        ImgProtocolloAnnullamento.Visible = blnValore
        ImgApriAllegatiAnnullamento.Visible = blnValore
    End Sub
    Private Sub ImageProtocolloRipristino(ByVal blnValore As Boolean)
        ImgProtolloRipristino.Visible = blnValore
        ImgApriAllegatiRipristino.Visible = blnValore
    End Sub

    Sub Visibilità_PulsantiSIGEDAnnullamento(ByVal IdRegioneCompetenza As Integer)
        If IdRegioneCompetenza = 22 Then 'nazionale
            ImgApriAllegatiAnnullamento.Visible = True
            ImgProtocolloAnnullamento.Visible = True
            ''   ImgProtocollazioneAnnullamento.Visible = True
        Else 'regionale
            ImgApriAllegatiAnnullamento.Visible = False
            ImgProtocolloAnnullamento.Visible = False
            '' ImgProtocollazioneAnnullamento.Visible = False
        End If
    End Sub
    Sub Visibilità_PulsantiSIGEDRipristino(ByVal IdRegioneCompetenza As Integer)
        If IdRegioneCompetenza = 22 Then 'nazionale
            ImgApriAllegatiRipristino.Visible = True
            ImgProtolloRipristino.Visible = True
            ''  ImgProtocollazioneRipristino.Visible = True
        Else 'regionale
            ImgApriAllegatiRipristino.Visible = False
            ImgProtolloRipristino.Visible = False
            '' ImgProtocollazioneRipristino.Visible = False
        End If
    End Sub
    Sub AbilitoDisabilitoOggettiDropDownList(ByVal blnAnnulla As Boolean, ByVal blnRipristino As Boolean)

        ddlTipoAnnullamento.Enabled = blnAnnulla
        ddlAutoritaAnnullamento.Enabled = blnAnnulla
        If ddlTipoAnnullamento.SelectedValue = 2 Then
            ddlAutoritaAnnullamento.Enabled = blnAnnulla
        End If
        ddlTipoRipristino.Enabled = blnRipristino
        ddlAutoritaRipristino.Enabled = blnRipristino
        If ddlTipoRipristino.SelectedValue = 2 Then
            ddlAutoritaRipristino.Enabled = blnRipristino
        End If

    End Sub
    Private Function ControlliFormali() As Boolean
        ControlliFormali = True
        lblmessaggio.Visible = False
        lblerrore.Visible = True
        Select Case TxtStatoSanzione.Text
            Case "Sanzione Valida"

                If ddlTipoAnnullamento.SelectedValue = 0 Then
                    lblerrore.Text = "E' necessario indicare il Tipo Annullamento."
                    ControlliFormali = False
                Else
                    If ddlTipoAnnullamento.SelectedItem.Text = "Sentenza" Then
                        If ddlAutoritaAnnullamento.SelectedValue = 0 Then
                            lblerrore.Text = "E' necessario indicare l'autorità di Annullamento."
                            ControlliFormali = False
                        End If
                    End If
                End If
                If ddlAutoritaAnnullamento.SelectedValue <> 0 And ddlTipoAnnullamento.SelectedValue = 0 Then
                    lblerrore.Text = "E' necessario indicare il Tipo Annullamento."
                    ControlliFormali = False
                End If
            Case "Sanzione Annullata"
                If ddlTipoRipristino.SelectedValue = 0 Then
                    lblerrore.Text = "E' necessario indicare il Tipo Ripristino."
                    ControlliFormali = False
                Else
                    If ddlTipoRipristino.SelectedItem.Text = "Sentenza" Then
                        If ddlAutoritaRipristino.SelectedValue = 0 Then
                            lblerrore.Text = "E' necessario indicare l'autorità di Ripristino."
                            ControlliFormali = False
                        End If
                    End If
                End If
                If ddlAutoritaRipristino.SelectedValue <> 0 And ddlTipoRipristino.SelectedValue = 0 Then
                    lblerrore.Text = "E' necessario indicare il Tipo Ripristino."
                    ControlliFormali = False
                End If

        End Select
        Return ControlliFormali
    End Function
    Sub Update(ByVal objNomeTabella As String)
        Dim strNull As String = "Null"
        Dim strSql As String
        Dim CmdUpdate As SqlClient.SqlCommand

        strSql = " UPDATE " & objNomeTabella & " SET "
        strSql &= " Note = '" & txtNote.Text.Replace("'", "''") & "' , "
        If TxtStatoSanzione.Text = "Sanzione Valida" Then 'annullamento sanzione
            strSql &= " StatoSanzione = 1 , "
            strSql &= " TipoAnnullamento = " & ddlTipoAnnullamento.SelectedValue & " , "
            strSql &= " UserAnnullaSanzione='" & Session("Utente") & "', "
            strSql &= " DataAnnullamentoSanzione=getdate(), "

            If txtNumProtAnnullamento.Text <> "" Then
                strSql &= " NProtAnnullamentoSanzione ='" & txtNumProtAnnullamento.Text & "', "
            Else
                strSql &= " NProtAnnullamentoSanzione= " & strNull & ", "
            End If
            If txtDataProtAnnullamento.Text <> "" Then
                strSql &= " DataProtAnnullamentoSanzione='" & txtDataProtAnnullamento.Text & "', "
            Else
                strSql &= " DataProtAnnullamentoSanzione= " & strNull & ", "
            End If
            If ddlAutoritaAnnullamento.SelectedValue <> "" Then
                strSql &= " IdAutoritaAnnullamento = " & ddlAutoritaAnnullamento.SelectedValue & " "
            Else
                strSql &= " IdAutoritaAnnullamento = " & strNull & " "
            End If
        Else 'ripristino sanzione
            strSql &= " StatoSanzione = 2 , "
            strSql &= " TipoRipristino = " & ddlTipoRipristino.SelectedValue & " , "
            strSql &= " UserRipristinoSanzione='" & Session("Utente") & "', "
            strSql &= " DataRipristinoSanzione=getdate(), "

            If txtNumProtRipristino.Text <> "" Then
                strSql &= " NProtRipristinoSanzione ='" & txtNumProtRipristino.Text & "', "
            Else
                strSql &= " NProtRipristinoSanzione= " & strNull & ", "
            End If
            If txtDataProtRipristino.Text <> "" Then
                strSql &= " DataProtRipristinoSanzione ='" & txtDataProtRipristino.Text & "', "
            Else
                strSql &= " DataProtRipristinoSanzione= " & strNull & ", "
            End If
            If ddlAutoritaRipristino.SelectedValue <> "" Then
                strSql &= " IdAutoritaRipristino = " & ddlAutoritaRipristino.SelectedValue & " "
            Else
                strSql &= " IdAutoritaRipristino = " & strNull & " "
            End If
        End If
        strSql = strSql & " WHERE idverifica =" & idVerifica & " and IdSanzione =" & Request.QueryString("IDSanzione") & ""
        CmdUpdate = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub
    Sub CaricaOggettiMaschera(ByVal Sanzione As String, ByVal SoggettoSanzione As String, ByVal TipoSanzione As String, ByVal StatoSanzione As String, ByVal DataAnnullamentoSanzione As String, ByVal UserAnnullaSanzione As String, ByVal UserRipristinoSanzione As String, ByVal DataRipristinoSanzione As String, ByVal UtenteEsecutore As String, ByVal TipoAnnullamento As Integer, ByVal NProtAnnullamentoSanzione As String, ByVal DataProtAnnullamentoSanzione As String, ByVal IdAutoritaAnnullamento As Integer, ByVal TipoRipristino As Integer, ByVal NProtRipristinoSanzione As String, ByVal DataProtRipristinoSanzione As String, ByVal IdAutoritaRipristino As Integer, ByVal Note As String)
        'funzione che carica gli oggetti text e dropdownlist dal risultato di una query
        TxtSanzione.Text = Sanzione
        TxtSoggettoSanzione.Text = SoggettoSanzione
        TxtTipoSanzione.Text = TipoSanzione
        TxtStatoSanzione.Text = StatoSanzione
        TxtDataAnnullamento.Text = DataAnnullamentoSanzione
        TxtUserAnnullamento.Text = UserAnnullaSanzione
        TxtUserRipristino.Text = UserRipristinoSanzione
        TxtDataRipristino.Text = DataRipristinoSanzione
        TxtUtenteEsecutoreSanzione.Text = UtenteEsecutore
        ddlTipoAnnullamento.SelectedValue = TipoAnnullamento
        txtNumProtAnnullamento.Text = NProtAnnullamentoSanzione
        txtDataProtAnnullamento.Text = DataProtAnnullamentoSanzione
        ddlAutoritaAnnullamento.SelectedValue = IdAutoritaAnnullamento
        ddlTipoRipristino.SelectedValue = TipoRipristino
        txtNumProtRipristino.Text = NProtRipristinoSanzione
        txtDataProtRipristino.Text = DataProtRipristinoSanzione
        ddlAutoritaRipristino.SelectedValue = IdAutoritaRipristino
        txtNote.Text = Note
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            ddlTipoAnnullamento.Items.Add("")
            ddlTipoAnnullamento.Items(0).Value = 0
            ddlTipoAnnullamento.Items.Add("Ufficio")
            ddlTipoAnnullamento.Items(1).Value = 1
            ddlTipoAnnullamento.Items.Add("Sentenza")
            ddlTipoAnnullamento.Items(2).Value = 2


            ddlTipoRipristino.Items.Add("")
            ddlTipoRipristino.Items(0).Value = 0
            ddlTipoRipristino.Items.Add("Ufficio")
            ddlTipoRipristino.Items(1).Value = 1
            ddlTipoRipristino.Items.Add("Sentenza")
            ddlTipoRipristino.Items(2).Value = 2

            'ddlTipoAnnullamento.SelectedValue = 2
            'ddlTipoRipristino.SelectedValue = 2
            idEnte = Request.QueryString("IdE")
            idVerifica = Request.QueryString("IdVer")
            CaricaAutorità(ddlAutoritaAnnullamento)
            CaricaAutorità(ddlAutoritaRipristino)
            Intestazione()
        End If
    End Sub

    Private Sub ddlTipoAnnullamento_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoAnnullamento.SelectedIndexChanged
        If ddlTipoAnnullamento.SelectedItem.Text = "Sentenza" Then
            Visibilità_PulsantiSIGEDAnnullamento(Session("IdRegCompetenza"))
            If Session("IdRegCompetenza") = 22 Then
                '  Abilito_DisabilitoAnnullamento(True, Color.White)
            Else
                '  Abilito_DisabilitoAnnullamento(True, Color.White)
            End If

            ddlAutoritaAnnullamento.Enabled = True

        Else
            'Abilito_DisabilitoAnnullamento(True, "visibility: hidden", Color.LightGray)

            Visibilità_PulsantiSIGEDAnnullamento(Session("IdRegCompetenza"))
            If Session("IdRegCompetenza") = 22 Then
                ' Abilito_DisabilitoAnnullamento(True, Color.White)
            Else
                'Abilito_DisabilitoAnnullamento(True, Color.White)
            End If
            ddlAutoritaAnnullamento.SelectedValue = 0
            ddlAutoritaAnnullamento.Enabled = False
        End If
    End Sub

    Private Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        lblmessaggio.Text = ""
        If ControlliFormali() = False Then Exit Sub
        If Request.QueryString("IdA") <> 0 Then
            'ATTIVITA' - PROGETTO
            Update("TVerificheSanzioniProgetto")
        ElseIf Request.QueryString("IdE") <> 0 Then
            'ENTE
            Update("TVerificheSanzioniEnte")
        ElseIf Request.QueryString("IdES") <> 0 Then
            'SEDE
            Update("TVerificheSanzioniSede")
        ElseIf Request.QueryString("IdESA") <> 0 Then
            'SEDE ATTUAZIONE
            Update("TVerificheSanzioniSedeAttuazione")
            'RICHIAMO STORE CHE CONTROLLA SE ESISTONO ALTRE SANZIONI PER LA SEDE DI ATTUAZIONE
            Store_SP_VER_FLAG_SEDE_SEGNALATA(Request.QueryString("IdESA"))
        ElseIf Request.QueryString("IdAESA") <> 0 Then
            'SEDE PROGETTO
            Update("TVerificheSanzioniSedeProgetto")
        End If
        'lblmessaggio.Text = 
        Store_SP_VER_CONTROLLO_SANZIONI_VERIFICA(Request.QueryString("IdVer"))
        Intestazione()
        lblmessaggio.Visible = True
        lblerrore.Visible = False
        lblmessaggio.Text = "Salvataggio eseguito con successo."
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("ver_Sanzione.aspx?NumProtEsecSanzione= " & Trim(TxtNumProtInvioSanzione.Text) & "&DataProtEsecSanzione= " & Trim(TxtDataProtInvioSanzione.Text) & "&Segnalata=" & Request.QueryString("Segnalata") & "&idverifica=" & idVerifica & " &Idente=" & Trim(Request.QueryString("Idente")) & "&idprogrammazione=" & Trim(Request.QueryString("idprogrammazione")) & "&VengoDa=" & Session("VengoDa") & "&IdEnte=" & Request.QueryString("Idente"))
    End Sub

    Private Sub ddlTipoRipristino_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoRipristino.SelectedIndexChanged
        If ddlTipoRipristino.SelectedItem.Text = "Sentenza" Then
            Visibilità_PulsantiSIGEDRipristino(Session("IdRegCompetenza"))
            If Session("IdRegCompetenza") = 22 Then
                '  Abilito_DisabilitoRipristino(True, Color.White)
            Else
                '  Abilito_DisabilitoRipristino(True, Color.White)
            End If

            ddlAutoritaRipristino.Enabled = True
        Else
            'Abilito_DisabilitoRipristino(True, "visibility: hidden", Color.LightGray)
            Visibilità_PulsantiSIGEDRipristino(Session("IdRegCompetenza"))
            If Session("IdRegCompetenza") = 22 Then
                '  Abilito_DisabilitoRipristino(True, Color.White)
            Else
                '   Abilito_DisabilitoRipristino(True, Color.White)
            End If
            ddlAutoritaRipristino.SelectedValue = 0
            ddlAutoritaRipristino.Enabled = False
        End If
    End Sub

    Private Sub Store_SP_VER_FLAG_SEDE_SEGNALATA(ByVal IDESA As Integer)
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_VER_FLAG_SEDE_SEGNALATA]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IDEnteSedeAttuazione", SqlDbType.Int).Value = IDESA

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 50
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Function Store_SP_VER_CONTROLLO_SANZIONI_VERIFICA(ByVal IDVerifica As Integer) As String
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_VER_CONTROLLO_SANZIONI_VERIFICA]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IDVerifica", SqlDbType.Int).Value = IDVerifica

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 50
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Esito").Value
            Return str
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function


End Class