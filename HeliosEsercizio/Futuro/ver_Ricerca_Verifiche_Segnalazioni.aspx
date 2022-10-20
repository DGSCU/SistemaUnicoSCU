<%@ Page Title="Ricerca Verifiche su Segnalazione" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ver_Ricerca_Verifiche_Segnalazioni.aspx.vb" Inherits="Futuro.ver_Ricerca_Verifiche_Segnalazioni" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function Stampa() {
            document.all.StampaPagina.style.visibility = 'hidden';
            window.print()
            window.close()
        }
    </script>
    <script type="text/javascript">
           /* <![CDATA[ */
            $(function () {
                var clientId = "#" + CostruisciId('txtDalSegnalazione');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDaSegnalazione');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataDalPrevista');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataAlPrevista');
                    $("" + clientId + "").datepicker();
            });

               $(function () {
                var clientId = "#" + CostruisciId('txtDataDalInizio');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtDataAlInizio');
                    $("" + clientId + "").datepicker();
            });
              /* ]]> */


                
    </script>
    <%--  <script type="text/javascript">
      
        function MM_findObj(n, d) { //v4.01
            var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
                d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
            }
            if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
            for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
            if (!x && d.getElementById) x = d.getElementById(n); return x;
        }

        function YY_checkform() { //v4.71
            //copyright (c)1998,2002 Yaromat.com
            var a = YY_checkform.arguments, oo = true, v = '', s = '', err = false, r, o, at, o1, t, i, j, ma, rx, cd, cm, cy, dte, at;
            for (i = 1; i < a.length; i = i + 4) {
                if (a[i + 1].charAt(0) == '#') { r = true; a[i + 1] = a[i + 1].substring(1); } else { r = false }
                o = MM_findObj(a[i].replace(/\[\d+\]/ig, ""));
                o1 = MM_findObj(a[i + 1].replace(/\[\d+\]/ig, ""));
                v = o.value; t = a[i + 2];
                if (o.type == 'text' || o.type == 'password' || o.type == 'hidden') {
                    if (r && v.length == 0) { err = true }
                    if (v.length > 0)
                        if (t == 1) { //fromto
                            ma = a[i + 1].split('_'); if (isNaN(v) || v < ma[0] / 1 || v > ma[1] / 1) { err = true }
                        } else if (t == 2) {
                            rx = new RegExp("^[\\w\.=-]+@[\\w\\.-]+\\.[a-zA-Z]{2,4}$"); if (!rx.test(v)) err = true;
                        } else if (t == 3) { // date
                            ma = a[i + 1].split("#"); at = v.match(ma[0]);
                            if (at) {
                                cd = (at[ma[1]]) ? at[ma[1]] : 1; cm = at[ma[2]] - 1; cy = at[ma[3]];
                                dte = new Date(cy, cm, cd);
                                if (dte.getFullYear() != cy || dte.getDate() != cd || dte.getMonth() != cm) { err = true };
                            } else { err = true }
                        } else if (t == 4) { // time
                            ma = a[i + 1].split("#"); at = v.match(ma[0]); if (!at) { err = true }
                        } else if (t == 5) { // check this 2
                            if (o1.length) o1 = o1[a[i + 1].replace(/(.*\[)|(\].*)/ig, "")];
                            if (!o1.checked) { err = true }
                        } else if (t == 6) { // the same
                            if (v != MM_findObj(a[i + 1]).value) { err = true }
                        }
                } else
                    if (!o.type && o.length > 0 && o[0].type == 'radio') {
                        at = a[i].match(/(.*)\[(\d+)\].*/i);
                        o2 = (o.length > 1) ? o[at[2]] : o;
                        if (t == 1 && o2 && o2.checked && o1 && o1.value.length / 1 == 0) { err = true }
                        if (t == 2) {
                            oo = false;
                            for (j = 0; j < o.length; j++) { oo = oo || o[j].checked }
                            if (!oo) { s += '* ' + a[i + 3] + '\n' }
                        }
                    } else if (o.type == 'checkbox') {
                        if ((t == 1 && o.checked == false) || (t == 2 && o.checked && o1 && o1.value.length / 1 == 0)) { err = true }
                    } else if (o.type == 'select-one' || o.type == 'select-multiple') {
                        if (t == 1 && o.selectedIndex / 1 == 0) { err = true }
                    } else if (o.type == 'textarea') {
                        if (v.length < a[i + 1]) { err = true }
                    }
                if (err) { s += '* ' + a[i + 3] + '\n'; err = false }
            }
            if (s != '') {
                //alert('Il formato della data è incorretto:\t\t\t\t\t\n\n'+s)
                alert(unescape("Il formato della data %E8 incorretto:\t\t\t\t\t\n\n" + s));
                o.focus();
                return false;
            }
            document.MM_returnValue = (s == '');

        }
        function Mid(str, start, len)
        /***
        IN: str - the string we are LEFTing
        start - our string's starting position (0 based!!)
        len - how many characters from start we want to get

        RETVAL: The substring from start to start+len
        ***/
        {
            // Make sure start and len are within proper bounds
            if (start < 0 || len < 0) return "";

            var iEnd, iLen = String(str).length;
            if (start + len > iLen)
                iEnd = iLen;
            else
                iEnd = start + len;

            return String(str).substring(start, iEnd);
        }
    </script>
   
  
    <script event="onclick" for="cmdRicerca">
			if ((document.all.txtDataDalPrevista.value!='')&&(document.all.txtDataAlPrevista.value!='')){
				/*blocco di variabili che costruiscono la data all'inverso per controllare
				che la data DAL sia minore di quella AL*/
				var giornoVerDAL=Mid(document.all.txtDataDalPrevista.value,0,2);
				var meseVerDAL=Mid(document.all.txtDataDalPrevista.value,3,2);
				var annoVerDAL=Mid(document.all.txtDataDalPrevista.value,6,4);
				var datainvertitaVerDAL=annoVerDAL+meseVerDAL+giornoVerDAL;
				
				var giornoVerAL=Mid(document.all.txtDataAlPrevista.value,0,2);
				var meseVerAL=Mid(document.all.txtDataAlPrevista.value,3,2);
				var annoVerAL=Mid(document.all.txtDataAlPrevista.value,6,4);
				var datainvertitaVerAL=annoVerAL+meseVerAL+giornoVerAL;

				if (datainvertitaVerDAL>datainvertitaVerAL){
					alert("La data prevista verifica AL deve essere maggiore o uguale  di quella DAL.");
					document.all.txtDataDalPrevista.focus()
					return false;
				}
			}
			if ((document.all.txtDataDalInizio.value!='')&&(document.all.txtDataAlInizio.value!='')){
				/*blocco di variabili che costruiscono la data all'inverso per controllare
				che la data DAL sia minore di quella AL*/
				var giornoDAL=Mid(document.all.txtDataDalInizio.value,0,2);
				var meseDAL=Mid(document.all.txtDataDalInizio.value,3,2);
				var annoDAL=Mid(document.all.txtDataDalInizio.value,6,4);
				var datainvertitaDAL=annoDAL+meseDAL+giornoDAL;
				
				var giornoAL=Mid(document.all.txtDataAlInizio.value,0,2);
				var meseAL=Mid(document.all.txtDataAlInizio.value,3,2);
				var annoAL=Mid(document.all.txtDataAlInizio.value,6,4);
				var datainvertitaAL=annoAL+meseAL+giornoAL;

				if (datainvertitaDAL>datainvertitaAL){
					alert("La data inizio verifica AL deve essere maggiore o uguale di quella DAL.");
					document.all.txtDataDalInizio.focus()
					return false;
				}
			}
			if ((document.all.txtDalSegnalazione .value!='')&&(document.all.txtDaSegnalazione.value!='')){
				var giornoDAL=Mid(document.all.txtDalSegnalazione .value,0,2);
				var meseDAL=Mid(document.all.txtDalSegnalazione .value,3,2);
				var annoDAL=Mid(document.all.txtDalSegnalazione .value,6,4);
				var datainvertitaDAL=annoDAL+meseDAL+giornoDAL;
				
				var giornoAL=Mid(document.all.txtDaSegnalazione.value,0,2);
				var meseAL=Mid(document.all.txtDaSegnalazione.value,3,2);
				var annoAL=Mid(document.all.txtDaSegnalazione.value,6,4);
				var datainvertitaAL=annoAL+meseAL+giornoAL;

				if (datainvertitaDAL>datainvertitaAL){
					alert("La data ricezione segnalazione AL deve essere maggiore o uguale di quella DAL.");
					document.all.txtDalSegnalazione.focus()
					return false;
				}
			}
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
        <fieldset class="ContornoPaginaScroll">
            <legend>Ricerca Verifiche su segnalazione</legend>
            <div class="wrapper" style="width: 100%">
                <div class="headers">
                    <h2>
                        <asp:Label ID="lblTitolo" runat="server" Text="Ricerca Verifiche su segnalazione"></asp:Label></h2>
                </div>
                <div class="rowGroup" style="height: auto">
                    <div class="RigaVuota">
                        &nbsp;
                    </div>
                </div>
                <fieldset class="ContornoPagina">
                    <legend>
                        <asp:Label ID="Label22" runat="server" Text="Caratteristiche Verifiche"></asp:Label></legend>
                    <div class="wrapper" style="width: 100%; border: 0px">
                        <div class="row" id="div6" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label25" CssClass="label" AssociatedControlID="txtCodiceFascicolo"
                                    runat="server" Text="Codice Fascicolo"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 25%">
                                <asp:TextBox ID="txtCodiceFascicolo" runat="server" CssClass="textbox">  </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="Label27" CssClass="label" AssociatedControlID="ddlStatoVerifica" runat="server"
                                    Text="Stato Verifica"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:DropDownList ID="ddlStatoVerifica" runat="server" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label5" CssClass="label" AssociatedControlID="ddEsistoSeg" runat="server"
                                    Text="Esito Segnalazione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:DropDownList ID="ddEsistoSeg" runat="server" AutoPostBack="False" CssClass="ddlClass">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="0">Registrata</asp:ListItem>
                                    <asp:ListItem Value="1">Infondata</asp:ListItem>
                                    <asp:ListItem Value="2">Trasmessa altro Servizio</asp:ListItem>
                                    <asp:ListItem Value="3">Archiviata</asp:ListItem>
                                    <asp:ListItem Value="4">Accettata</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row" id="div2" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label23" CssClass="label" AssociatedControlID="txtDalSegnalazione"
                                    runat="server" Text="Data Ricezione Segnalazione Dal"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:TextBox ID="txtDalSegnalazione" runat="server" CssClass="textboxData">  </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 5%">
                                <asp:Label ID="Label6" CssClass="label" AssociatedControlID="txtDaSegnalazione" runat="server"
                                    Text="Al"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:TextBox ID="txtDaSegnalazione" runat="server" CssClass="textboxData">  </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="Label7" CssClass="label" AssociatedControlID="ddFonte" runat="server"
                                    Text="Fonte"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:DropDownList ID="ddFonte" runat="server" AutoPostBack="False" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:CheckBox ID="chksegnalazione" runat="server" Text="Solo Segnalazioni"></asp:CheckBox>
                            </div>
                        </div>
                        <div class="row" id="div3" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label26" CssClass="label" AssociatedControlID="ddlVerificatoreInterno"
                                    runat="server" Text="Verificatore Interno"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 25%">
                                <asp:DropDownList ID="ddlVerificatoreInterno" runat="server" CssClass="ddlClass"
                                    AutoPostBack="False">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="Label29" CssClass="label" AssociatedControlID="ddlVerificatoreIGF"
                                    runat="server" Text="Verificatore IGF" Visible="False"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 25%">
                                <asp:DropDownList ID="ddlVerificatoreIGF" runat="server" CssClass="ddlClass" 
                                    Visible="False">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 10%">
                                <asp:Label ID="Label11" CssClass="label" AssociatedControlID="ddlCompetenza" runat="server"
                                    Text="Competenza"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 15%">
                                <asp:DropDownList ID="ddlCompetenza" runat="server" AutoPostBack="True" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row" id="div4" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label28" CssClass="label" AssociatedControlID="txtDataDalPrevista"
                                    runat="server" Text="Data prevista Verifica Dal"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:TextBox ID="txtDataDalPrevista" runat="server" CssClass="textboxData">  </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 5%">
                                <asp:Label ID="Label32" CssClass="label" AssociatedControlID="txtDataAlPrevista"
                                    runat="server" Text="Al"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:TextBox ID="txtDataAlPrevista" runat="server" CssClass="textboxData">  </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label30" CssClass="label" AssociatedControlID="txtDataDalInizio" runat="server"
                                    Text="Data Inizio Verifica Dal"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:TextBox ID="txtDataDalInizio" runat="server" CssClass="textboxData">  </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 5%">
                                <asp:Label ID="Label34" CssClass="label" AssociatedControlID="txtDataAlInizio" runat="server"
                                    Text="Al"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 10%">
                                <asp:TextBox ID="txtDataAlInizio" runat="server" CssClass="textboxData">  </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset class="ContornoPagina">
                    <legend>
                        <asp:Label ID="Label1" runat="server" Text="Caratteristiche Progetti"></asp:Label></legend>
                    <div class="wrapper" style="width: 100%; border: 0px">
                        <div class="row" id="div1" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="codpog" CssClass="label" AssociatedControlID="TxtCodPog" runat="server"
                                    Text="Codice Progetto"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtCodPog" runat="server" CssClass="textbox">  </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="nomepog" CssClass="label" AssociatedControlID="TxtDescPog" runat="server"
                                    Text="Titolo Progetto"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtDescPog" runat="server" CssClass="textbox">  </asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div7" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label2" CssClass="label" AssociatedControlID="DdlBando" runat="server"
                                    Text="Circolare"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="DdlBando" runat="server" CssClass="ddlClass" AutoPostBack="False">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label3" CssClass="label" AssociatedControlID="ddlMaccCodAmAtt" runat="server"
                                    Text="Settore"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlMaccCodAmAtt" runat="server" CssClass="ddlClass" AutoPostBack="False">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row" id="div8" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label4" CssClass="label" AssociatedControlID="ddlCodAmAtt" runat="server"
                                    Text="Area Intervento"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlCodAmAtt" runat="server" CssClass="ddlClass" AutoPostBack="False">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset class="ContornoPagina">
                    <legend>
                        <asp:Label ID="Label8" runat="server" Text="Caratteristiche Enti"></asp:Label></legend>
                    <div class="wrapper" style="width: 100%; border: 0px">
                        <div class="row" id="div5" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label20" CssClass="label" AssociatedControlID="TxtCodEnte" runat="server"
                                    Text="Cod. Ente"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtCodEnte" runat="server" CssClass="textbox">  </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label21" CssClass="label" AssociatedControlID="TxtDescrEnte" runat="server"
                                    Text="Descr. Ente"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtDescrEnte" runat="server" CssClass="textbox">  </asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div10" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label9" CssClass="label" AssociatedControlID="ddlClasse" runat="server"
                                    Text="Classe/Sezione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlClasse" runat="server" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label10" CssClass="label" AssociatedControlID="ddlTipologia" runat="server"
                                    Text="Tipologia"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:DropDownList ID="ddlTipologia" runat="server" CssClass="ddlClass">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <fieldset class="ContornoPagina">
                    <legend>
                        <asp:Label ID="Label12" runat="server" Text="Attuazione Geografica Progetti"></asp:Label></legend>
                    <div class="wrapper" style="width: 100%; border: 0px">
                        <div class="row" id="div9" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label13" CssClass="label" AssociatedControlID="TxtComune" runat="server"
                                    Text="Comune"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtComune" runat="server" CssClass="textbox">  </asp:TextBox>
                            </div>
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label14" CssClass="label" AssociatedControlID="TxtProvincia" runat="server"
                                    Text="Provincia"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtProvincia" runat="server" CssClass="textbox">  </asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="div11" runat="server">
                            <div class="collable" style="width: 15%">
                                <asp:Label ID="Label15" CssClass="label" AssociatedControlID="TxtRegione" runat="server"
                                    Text="Regione"></asp:Label>
                            </div>
                            <div class="colOggetti" style="width: 35%">
                                <asp:TextBox ID="TxtRegione" runat="server" CssClass="textbox">  </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <div class="RigaPulsanti">
                            <asp:Button ID="imgStampa" runat="server" ToolTip="Esporta" CssClass="Pulsante"
                        Text="Esporta CSV" Visible="False" />
                    <asp:Button ID="cmdRicerca" CssClass="Pulsante" runat="server" Text="Ricerca">
                    </asp:Button>
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
              


          <asp:HyperLink ID="ApriCSV1" runat="server" AccessKey="S" CssClass="linkStampa" ForeColor="#003399"
                        Text="DOWNLOAD CSV" ToolTip="Link per la stampa del risultato della ricerca"
                        Visible="False"></asp:HyperLink>
                </div>
                <asp:HiddenField ID="txtbando1" runat="server" />
                <asp:HiddenField ID="ddlCodAmAtt1" runat="server" />
                <asp:HiddenField ID="txtDenominazioneEnte1" runat="server" />
                <asp:HiddenField ID="ddlStatoAttivita1" runat="server" />
                <asp:HiddenField ID="txtTitoloProgetto1" runat="server" />
                <div class="rowGroup" style="height: auto">
                    <asp:Label ID="lblmessaggio" runat="server" CssClass="msgErrore"></asp:Label>
                    <div class="RigaVuota">
                        &nbsp;</div>
                </div>
                <div class="RigaPulsanti">
       <%--             <asp:ImageButton ID="" ImageUrl="images/printHELIOS.jpg" runat="server"
                        OnClientClick="Stampa()" Visible="False" Alt="Stampa"></asp:ImageButton>--%>
                </div>
                <asp:DataGrid ID="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Progetti"
                    AllowPaging="True" CellPadding="2" Font-Size="Small" CssClass="table" AllowSorting="True"
                    AutoGenerateColumns="False" UseAccessibleHeader="True">
                    <FooterStyle></FooterStyle>
                    <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
                    <EditItemStyle></EditItemStyle>
                    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
                    <HeaderStyle></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn> 
                            <ItemTemplate>
                                <asp:ImageButton ID="IMGseg" CommandName="seleziona" ToolTip="Dettaglio Segnalazione" AlternateText="Dettaglio Segnalazione" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>  
					        </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="Idverifica" HeaderText="Idverifica">
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Idsegnalazione" HeaderText="Idsegnalazione">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Codicefascicolo" HeaderText="Codice Fascicolo"></asp:BoundColumn>
                        <asp:BoundColumn DataField="statoverifiche" HeaderText="Stato Verifica"></asp:BoundColumn>
                        <asp:BoundColumn DataField="TipoVerifica" HeaderText="Tipo Verifica" Visible="False">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DataRicezioneSegnalazione" HeaderText="Data Segnalazione"
                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DataPrevistaVerifica" HeaderText="Data Inizio Prevista Verifica"
                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DataFinePrevistaVerifica" HeaderText="Data Fine Prevista Verifica"
                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                        <asp:BoundColumn DataField="DataFineVerifica" HeaderText="Data Chiusura Verifica"
                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                        <asp:BoundColumn DataField="nominativo" HeaderText="Verificatore"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Denominazione" HeaderText="Ente Proponente"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Titolo" HeaderText="Progetto">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="DataInizioAttivit&#224;" HeaderText="Data Inizio Progetto"
                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundColumn>
                        <asp:BoundColumn DataField="datafineattivit&#224;" HeaderText="Data Fine Progetto"
                            DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EnteFiglio" HeaderText="Sede Attuazione"></asp:BoundColumn>
                        <asp:BoundColumn DataField="comune" HeaderText="Comune"></asp:BoundColumn>
                        <asp:BoundColumn Visible="False" DataField="Regione" HeaderText="Regione"></asp:BoundColumn>
                    </Columns>
                    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center"
                    CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </div>
        </fieldset>
    </div>
</asp:Content>
