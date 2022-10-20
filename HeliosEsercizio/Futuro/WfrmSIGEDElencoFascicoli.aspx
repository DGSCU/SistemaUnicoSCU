<%@ Page Language="vb" Title="Elenco Fascicoli" AutoEventWireup="false" CodeBehind="WfrmSIGEDElencoFascicoli.aspx.vb" Inherits="Futuro.WfrmSIGEDElencoFascicoli" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
	<head>
		<title>Elenco Fascicoli</title>
		 <link href="Styles/Site.css" rel="stylesheet"  type="text/css" media="screen" />
        <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	    <meta name="vs_defaultClientScript" content="JavaScript"/>
	    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
        <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }
        </style>
       <script  type="text/javascript">
         /* <![CDATA[ */
        var formId;
        var prefissoIdClient = "MainContent_";

	    
	    function ImpostaFormId() {
	        formId = document.forms[0].id;
	        return formId;
	    }
        function CostruisciId(IdServer) {
           var IdClient = prefissoIdClient + IdServer
           return IdClient
        }
          
      $(function () {
          $("#txtDataDa").datepicker();
      });

      $(function () {
          $("#txtDataA").datepicker();
      });

        function OpenWindow(NomeOggetto, NomeForm) {
            mywin = window.open("WfrmCalendario.aspx?strNomeOggetto=" + NomeOggetto + "&strNomeForm=" + NomeForm, "", "height=280,width=256,dependent=no,scrollbars=no,status=no");
        }
         function CheckCampiObbligatori() {
        		if (document.getElementById("cboTitolario").value=='')
				 { 
					alert("E' necessario indicare il titolario.");
					return false;
				 } 
                 else {
                   document.getElementById("cmdCerca").style.visibility="hidden";
                    return true;
                 }
        }

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
      /* ]]> */
	</script>

	</head>
	<body>

    <form id="formFascicoli" method="post" runat="server">
    <div class="fieldsetrasparente">
     <div class="wrapper" style="width:100%">
       <div class="headers" >
        <h2>
            <asp:label id="lblFascicolo" runat="server" Text="Elenco Fascicoli"></asp:label>
        </h2>
       </div>
       	 <div class="row" style="height:auto">
		    <asp:label id="lblmessaggio" runat="server" CssClass="msgErrore"></asp:label>
		</div>
        <div class="RigaVuota">
            &nbsp;
		</div>

       <div class="row">
        <div class="collable" style="width:15%">
            <asp:Label ID="lbltxtCodFasc" CssClass="label" AssociatedControlID="txtCodFasc" runat="server" Text="Num. Fascicolo:"></asp:Label>
        </div>
       <div class="colOggetti" style="width:35%">
            <asp:textbox id="txtCodFasc" CssClass="textbox" runat="server" ></asp:textbox>
       </div>
        <div class="collable" style="width:15%">
            <asp:Label ID="IdLbltxtDescFasc" CssClass="label" AssociatedControlID="txtDescFasc" runat="server" Text="Descrizione:"></asp:Label>
         </div>
         <div class="colOggetti" style="width:35%">
            <asp:textbox id="txtDescFasc" CssClass="textbox" runat="server" ></asp:textbox>
         </div>
       </div>
       <div class="row">
      
             <div class="collable" style="width:15%">
                <asp:Label ID="lblTxtNominativo" CssClass="label" AssociatedControlID="txtNominativo" runat="server" Text="Nominativo:"></asp:Label>
            </div>
          <div class="colOggetti" style="width:35%">
            <asp:textbox id="txtNominativo" CssClass="textbox" runat="server" ></asp:textbox>
         </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="lblCboTitolario" CssClass="label" AssociatedControlID="cboTitolario" runat="server" Text="Titolario:"></asp:Label>
            </div>
             <div class="colOggetti" style="width:35%">
                <asp:dropdownlist id="cboTitolario"   CssClass="ddlClass" runat="server" ></asp:dropdownlist>
            </div>
       </div>

            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCboAnno" CssClass="label" AssociatedControlID="cboAnno" runat="server" Text="Anno Titolario:"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:dropdownlist id="cboAnno" runat="server" CssClass="ddlClass" Width="100px" AutoPostBack="True"></asp:dropdownlist>
                </div>
            </div>
        <div class="row" style="visibility:hidden;height:auto" >

             <div class="collable" style="width:15%">
                <asp:Label ID="lblTxtDataDa" CssClass="label" Visible="false" AssociatedControlID="txtDataDa" runat="server" Text="Data Creazione Da:"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:textbox id="txtDataDa" CssClass="textbox" Visible="false" Width="100px" MaxLength="10" runat="server"></asp:textbox>
            </div>
       
               <div class="collable" style="width:15%">
                <asp:Label ID="lblTxtDataA" CssClass="label" Visible="false" AssociatedControlID="txtDataA" runat="server" Text="Data Creazione A:"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:textbox id="txtDataA" CssClass="textbox" Visible="false" Width="100px" MaxLength="10"  runat="server" ></asp:textbox>

            </div>
            </div>
       
       <div class="RigaPulsanti">
            <asp:Button ID="cmdCerca" CssClass="Pulsante" Visible="true" runat="server" OnClientClick="javascript: return CheckCampiObbligatori()" Text="Esegui Ricerca" />&nbsp;
             <asp:Button ID="imgBack" CssClass="Pulsante" Visible="false" runat="server"  Text="Indietro" />
        </div>
    <div class="RigaVuota">&nbsp;
    </div>
    <div class="row">
  <h3>
     <asp:label  id="lblRisultatoRicerca"   AssociatedControlID="dgRisultatoRicerca" Visible="false" Font-Bold="true"  runat="server" Text="Risultato ricerca" />
  </h3>
  
    <asp:datagrid id="dgRisultatoRicerca" runat="server" CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True" ShowFooter="false" Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
            <asp:TemplateColumn > 
              <ItemTemplate >
				<asp:ImageButton ID="ImageButton1" Width="20px" CommandName="Select1" STYLE="cursor:hand" 
                    AlternateText="Seleziona Fascicolo"  runat="server"  ToolTip="Seleziona Fascicolo" ImageURL="Images/cartella_small.png" CausesValidation="false">
                  </asp:ImageButton>                            
                </ItemTemplate>
                </asp:TemplateColumn>
			<asp:BoundColumn DataField="N&#176;. Fascicolo" HeaderText="N&#176;. Fascicolo">
                 <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>             
            </asp:BoundColumn>
			<asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" Width="55%"></ItemStyle>
            </asp:BoundColumn>
			<asp:BoundColumn DataField="Titolario" HeaderText="Titolario">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" Width="20%"></ItemStyle>
            </asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="Cod_Fasc" HeaderText="Cod_Fasc">
                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundColumn>
		    <asp:TemplateColumn> 
              <ItemTemplate >
				<asp:ImageButton ID="ImageButton2" Width="20px" CommandName="Navigazione" STYLE="cursor:hand" 
                    AlternateText="Navigazione" runat="server"  ToolTip="Navigazione"  ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false">
                  </asp:ImageButton>                            
                </ItemTemplate>
                </asp:TemplateColumn>
        </Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </div>
    </div>
    </div>
    </form>
</body>
</html>
