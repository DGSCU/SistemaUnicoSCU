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


Partial Public Class WfrmRicercaProgettiRicollocamento

    '''<summary>
    '''Controllo lblmess.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblmess As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblTitolo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblTitolo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblEnte.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblEnte As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtEnte.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtEnte As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblCodEnte.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblCodEnte As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtCodEnte.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtCodEnte As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblProgetto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblProgetto As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtProgetto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtProgetto As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblBando.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblBando As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtBando.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtBando As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblSettore.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblSettore As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo cboSettore.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cboSettore As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Controllo lblArea.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblArea As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo cboArea.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cboArea As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Controllo lblRegione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblRegione As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtRegione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtRegione As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblProvincia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblProvincia As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtProvincia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtProvincia As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblcodiceSede.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblcodiceSede As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo TxtCodiceSede.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents TxtCodiceSede As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo divlblSegnalazioneSanzione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divlblSegnalazioneSanzione As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''Controllo lblSegnalazioneSanzione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblSegnalazioneSanzione As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo divddlSegnalazioneSanzione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divddlSegnalazioneSanzione As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''Controllo ddlSegnalazioneSanzione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ddlSegnalazioneSanzione As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Controllo cmdRicerca.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdRicerca As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo cmdChiudi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdChiudi As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo lblmessaggio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblmessaggio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo dgRisultatoRicerca.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents dgRisultatoRicerca As Global.System.Web.UI.WebControls.DataGrid
End Class
