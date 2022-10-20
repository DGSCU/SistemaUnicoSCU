'------------------------------------------------------------------------------
' <generato automaticamente>
'     Codice generato da uno strumento.
'
'     Le modifiche a questo file possono causare un comportamento non corretto e verranno perse se
'     il codice viene rigenerato. 
' </generato automaticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class WfrmGestioneSostituisciVolontari

    '''<summary>
    '''Controllo LblInfo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblInfo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblMessaggi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblMessaggi As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LblConfermaOperazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblConfermaOperazione As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo ImgDocumentazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ImgDocumentazione As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Controllo imgVolontari.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgVolontari As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Controllo CmdModChiVol.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdModChiVol As Global.System.Web.UI.WebControls.LinkButton

    '''<summary>
    '''Controllo txtCodiceVolontarioSubentrante.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtCodiceVolontarioSubentrante As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Controllo txtCodiceVolontarioVecchio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtCodiceVolontarioVecchio As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Controllo txtVolSubentarata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtVolSubentarata As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Controllo txtidattivitasedeassegnazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtidattivitasedeassegnazione As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Controllo txtIdentitaVecchia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtIdentitaVecchia As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Controllo txtidentitaSbentrante.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtidentitaSbentrante As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Controllo txtidattivita.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtidattivita As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Controllo txtdatalimite.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtdatalimite As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Controllo txtCodiceFiscale.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtCodiceFiscale As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''Controllo lblMsgStato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblMsgStato As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblStato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblStato As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblDataChiusuraEV.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblDataChiusuraEV As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtdatachiusuraEV.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtdatachiusuraEV As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo LabellblCognome.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabellblCognome As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblCognome.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblCognome As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabellblNome.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabellblNome As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblNome.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblNome As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabellbldataNascita.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabellbldataNascita As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lbldataNascita.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lbldataNascita As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabellblComuneNascita.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabellblComuneNascita As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblComuneNascita.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblComuneNascita As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabellblComuneResidenza.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabellblComuneResidenza As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblComuneResidenza.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblComuneResidenza As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabellblCodFis.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabellblCodFis As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblCodFis.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblCodFis As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabelLblDataInizioServizio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabelLblDataInizioServizio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LblDataInizioServizio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblDataInizioServizio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabelLblDataFineServizio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabelLblDataFineServizio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LblDataFineServizio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblDataFineServizio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LblAmmessoRecupero.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LblAmmessoRecupero As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabellblPreogetto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabellblPreogetto As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblPreogetto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblPreogetto As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabellblDataInizio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabellblDataInizio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblDataInizio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblDataInizio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo LabellbldataFine.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents LabellbldataFine As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lbldataFine.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lbldataFine As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblRisultato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblRisultato As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo dgRisultatoRicercaSedi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents dgRisultatoRicercaSedi As Global.System.Web.UI.WebControls.DataGrid

    '''<summary>
    '''Controllo Label1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo chkStessaSede.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkStessaSede As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Controllo dgRisultatoRicercaVolontari.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents dgRisultatoRicercaVolontari As Global.System.Web.UI.WebControls.DataGrid

    '''<summary>
    '''Controllo rigaLegenda.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents rigaLegenda As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''Controllo lnlLbgenda.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lnlLbgenda As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo Label11.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label11 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo Label12.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label12 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo dtgElencoVolontariAltroProgetto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents dtgElencoVolontariAltroProgetto As Global.System.Web.UI.WebControls.DataGrid

    '''<summary>
    '''Controllo lbltxtdataAvvio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lbltxtdataAvvio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtdataAvvio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtdataAvvio As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblTesto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblTesto As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo ImageButton1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ImageButton1 As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Controllo cmdSalva.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdSalva As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo cmdChiudi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdChiudi As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo ImgAnnullaChiusura.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ImgAnnullaChiusura As Global.System.Web.UI.WebControls.Button
End Class
