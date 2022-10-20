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


Partial Public Class WfrmCOMPGestionePresenzeEstero

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
    '''Controllo lblAnnoMese.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblAnnoMese As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo cboAnnoMeseEstero.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cboAnnoMeseEstero As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Controllo CmdDettaglio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdDettaglio As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Controllo lbldescrizione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lbldescrizione As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblConfermaAssenze.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblConfermaAssenze As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo CboConfermaAssenze.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CboConfermaAssenze As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Controllo lblCompletato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblCompletato As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo CboCompletato.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CboCompletato As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Controllo CmdRicerca.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdRicerca As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo CmdEsporta.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdEsporta As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo ApriCSV1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents ApriCSV1 As Global.System.Web.UI.WebControls.HyperLink

    '''<summary>
    '''Controllo lblMessaggio.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblMessaggio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo dgElencoEnti.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents dgElencoEnti As Global.System.Web.UI.WebControls.DataGrid

    '''<summary>
    '''Controllo filtriVolontari.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents filtriVolontari As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''Controllo lblCodiceVolontario.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblCodiceVolontario As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtCodiceVolontario.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtCodiceVolontario As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblNominativo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblNominativo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtNominativo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtNominativo As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblProgetto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblProgetto As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtCodiceProgetto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtCodiceProgetto As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblFascia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblFascia As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtFascia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtFascia As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo lblgiornivuoto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblgiornivuoto As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo CboVuoto.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CboVuoto As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Controllo CmdRicercaVol.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdRicercaVol As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo lblmessaggio1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblmessaggio1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo lblApplica.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblApplica As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Controllo txtApplica.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtApplica As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Controllo CmdApplica.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdApplica As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo CmdSalva.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdSalva As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo CmdCompleta.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdCompleta As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo dgvolontariestero.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents dgvolontariestero As Global.System.Web.UI.WebControls.DataGrid

    '''<summary>
    '''Controllo CmdSalva1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdSalva1 As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo CmdCompleta1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdCompleta1 As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Controllo CmdChiudi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents CmdChiudi As Global.System.Web.UI.WebControls.Button
End Class
