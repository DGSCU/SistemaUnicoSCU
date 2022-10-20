<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="Ver_GestioneProgrammazione.aspx.vb" Inherits="Futuro.Ver_GestioneProgrammazione" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="JavaScript" type="text/JavaScript">
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
                //alert('Il formato della data  incorretto:\t\t\t\t\t\n\n'+s)
                alert(unescape("Il formato della data %E8 incorretto:\t\t\t\t\t\n\n" + s));
                o.focus();
                o.value = ''
                return false;
            }
            document.MM_returnValue = (s == '');
        }


        function ContaPunti(x) {
            var i = new RegExp(".{2}");
            if (!i.test(x)) {
                alert(i.test(x));
                return true;
            } else {
                alert(i.test(x))
                return false;
            }
        }	 
    </script>
    <script language="vbscript">
			function leggidata(inizio,fine)
				dim datainizio 
				dim datafine 
				'data inizio 
				datainizio=Year(inizio)
				if len(Month(inizio))>1 then
					datainizio=datainizio & Month(inizio)
				else
					datainizio=datainizio & "0" & Month(inizio)
				end if
				if len(Day(inizio))>1 then
					datainizio=datainizio & Day(inizio)
				else
					datainizio=datainizio & "0" & Day(inizio)
				end if
				'datainizio = Year(inizio) & Month(inizio) & Day(inizio)
				'datafine = Year(fine) & Month(fine) & Day(fine)
				'data fine
				datafine=Year(fine)
				if len(Month(fine))>1 then
					datafine=datafine & Month(fine)
				else
					datafine=datafine & "0" & Month(fine)
				end if
				if len(Day(fine))>1 then
					datafine=datafine & Day(fine)
				else
					datafine=datafine & "0" & Day(fine)
				end if
				if cdbl(datainizio) > cdbl(datafine) then
				   leggidata=true
				end if
				if cdbl(datainizio) = cdbl(datafine) then
					leggidata=true
				end if
			end function
    </script>
    <script for="TxtDataInizio" event="onblur">
			YY_checkform('Form1','TxtDataInizio','^\([0-9][0-9]\)\/\([0-9][0-9]\)\/\([0-9]{4}\)$#1#2#3','3','Il formato deve essere GG/MM/AAAA.');
    </script>
    <script for="TxtDataFine" event="onblur">
			YY_checkform('Form1','TxtDataFine','^\([0-9][0-9]\)\/\([0-9][0-9]\)\/\([0-9]{4}\)$#1#2#3','3','Il formato deve essere GG/MM/AAAA.');
    </script>
    <script language="javascript">
        function apriCalendario1(obj) {
            if (obj == "TxtDataInizio") {
                window.open('WfrmCalendario.aspx?strNomeForm=Form1&strNomeOggetto=TxtDataInizio', 'Calendario', 'width=300,height=270,toolbar=no,location=no,menubar=no,scrollbars=no');
            }
            if (obj == "TxtDataFine") {
                window.open('WfrmCalendario.aspx?strNomeForm=Form1&strNomeOggetto=TxtDataFine', 'Calendario', 'width=300,height=270,toolbar=no,location=no,menubar=no,scrollbars=no');
            }
        }	
    </script>
    <script for="cmdSalva" event="onclick">
				var msg;
				if (document.Form1.TxtDescrizione.value=="") 
				{
					alert("Inserire la descrizione della Programmazione")
					document.Form1.TxtDescrizione.focus();
					return false;
				}
				if (document.Form1.TxtDataInizio.value=="") 
				{
					alert("Inserire la Data Inizio.")
					document.Form1.TxtDataInizio.focus();
					return false;
				}
				if (document.Form1.TxtDataFine.value=="") 
				{
					alert("Inserire la Data Fine.")
					document.Form1.TxtDataFine.focus();
					return false;
				}
				if (leggidata(document.Form1.TxtDataInizio.value,document.Form1.TxtDataFine.value)==true) 
				{
					alert(unescape("La data Inizio non pu%F2 essere maggiore o uguale alla data Fine."));
					return false;
				}
				
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Inserimento Programmazione</legend>
        <div class="wrapper" style="width: 100%">
            <div class="headers">
                <h2>
                    <asp:Label ID="lblTitolo" runat="server" Text="Inserimento Programmazione"></asp:Label></h2>
            </div>
            <div id="Div1" class="row" runat="server">
                <div class="collable" style="width: 20%">
                    <asp:Label ID="LblDescrizione" CssClass="label" AssociatedControlID="TxtDescrizione"
                        runat="server" Text="Programmazione" />
                </div>
                <div class="colOggetti" style="width: 80%">
                    <asp:TextBox ID="TxtDescrizione" runat="server" CssClass="textbox" MaxLength="50"></asp:TextBox>
                </div>
            </div>
            <div id="Div3" class="row" runat="server">
                <div class="collable" style="width: 20%">
                    <asp:Label ID="LblDataInizio" CssClass="label" AssociatedControlID="TxtDataInizio"
                        runat="server" Text="Data Inizio"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 20%">
                    <asp:TextBox ID="TxtDataInizio" runat="server" CssClass="textboxData">
                    </asp:TextBox>
                </div>
                <div class="collable" style="width: 20%">
                    <asp:Label ID="LblDataFine" CssClass="label" AssociatedControlID="TxtDataFine" runat="server"
                        Text="Data Fine"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 20%">
                    <asp:TextBox ID="TxtDataFine" runat="server" CssClass="textboxData">
                    </asp:TextBox>
                </div>
            </div>
            <div id="Div2" class="row" runat="server">
                <div class="collable" style="width: 20%">
                    <asp:Label ID="LblRegioneCompetenza" CssClass="label" AssociatedControlID="ddlCompetenza"
                        runat="server" Text="Competenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 40%">
                    <asp:DropDownList ID="ddlCompetenza" runat="server" CssClass="ddlClass" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
            </div>


              <div id="Div4" class="row" runat="server">
                <div class="collable" style="width: 20%">
                    <asp:Label ID="lblCircolare" CssClass="label" AssociatedControlID="ddlBando"
                        runat="server" Text="Circolare"></asp:Label>
                </div>
                <div class="colOggetti" style="width: 40%">
                    <asp:DropDownList ID="ddlBando" runat="server" CssClass="ddlClass" Enabled="False">
                    </asp:DropDownList>
                </div>
            </div>

                <div id="Div5" class="row" runat="server">
                <div class="collable" style="width: 20%">
                    <asp:Label ID="LblNote" CssClass="label" AssociatedControlID="TxtNote"
                        runat="server" Text="Note" />
                </div>
                <div class="colOggetti" style="width: 80%">
                    <asp:TextBox ID="TxtNote" runat="server" CssClass="textbox"  
                        MaxLength="1000"     TextMode="MultiLine" Width="99%"></asp:TextBox>
                </div>
            </div>



             <div class="RigaPulsanti">
                    <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva"></asp:Button>
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"></asp:Button>
                   
                </div>


        </div>
    </fieldset>
  
</asp:Content>
