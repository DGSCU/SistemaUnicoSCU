<%@ Page Title="Risorsa Ente" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="entepersonale.aspx.vb" Inherits="Futuro.entepersonale" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">
    /* <![CDATA[ */
        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

        $(function () {
            var DataNascita = CostruisciId("txtDataNascita");
            var readonly = '<%=txtDataNascita.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataNascita = "#" + DataNascita
                $("" + sharpDataNascita + "").datepicker();
            }
        });

        function SetContextKey() {
            $find('<%=AutoCompleteExtenderResidenza.ClientID%>').set_contextKey($get("<%=ddlComuneResidenza.ClientID %>").value);
        };
        function ShowPopUp() {
            $find('popUpControl').show();
			return false;
		}
       
        function ValidazioneClient() {

//            var idCognome = CostruisciId("txtCognome");
//            var txtCognome = document.getElementById(idCognome);
//            if (txtCognome.value == '') {
//                alert("Inserire il Cognome.");
//                txtCognome.focus();
//                return false;
//            }

//            var idNome = CostruisciId("txtNome");
//            var txtNome = document.getElementById(idNome);
//            if (txtNome.value == '') {
//                alert("Inserire il Nome.");
//                txtNome.focus();
//                return false;
//            }

//            var idCodiceFiscale = CostruisciId("txtCodiceFiscale");
//            var txtCodiceFiscale = document.getElementById(idCodiceFiscale);
//            if (txtCodiceFiscale.value == '') {
//                alert("Inserire il Codice Fiscale.");
//                txtCodiceFiscale.focus();
//                return false;
//            }
//            if (txtCodiceFiscale.value.length != 16) {
//                alert("Codice Fiscale non corretto.");
//                txtCodiceFiscale.focus();
//                return false;
//            }
            
//            var IdEmail = CostruisciId("txtEmail");
//            var txtEmail = document.getElementById(IdEmail);

//            if (txtEmail.value != '') {

//                var i = new RegExp("^.+\\@(\\[?)[a-zA-Z0-9\\-\\.]+\\.([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
//                if (!i.test(txtEmail.value)) {
//                    alert("Il campo 'Email' non è valido.");
//                    txtEmail.focus();
//                    return false;
//                }

//            }

//            var IdTelefono = CostruisciId("txtTelefono");
//            var txtTelefono = document.getElementById(IdTelefono);
//            if (txtTelefono.value != '') {
//                if (parseInt(Number(txtTelefono.value)) != txtTelefono.value) {
//                    alert("Il campo 'Telefono' può contenere solo numeri.");
//                    txtTelefono.focus();
//                    return false;
//                }
//            }

//            var IdFax = CostruisciId("txtFax");
//            var txtFax = document.getElementById(IdFax);
//            if (txtFax.value != '') {
//                if (parseInt(Number(txtFax.value)) != txtFax.value) {
//                    alert("Il campo 'Fax' può contenere solo numeri.");
//                    txtFax.focus();
//                    return false;
//                }
//            }

//            var IdCellulare = CostruisciId("txtCellulare");
//            var txtCellulare = document.getElementById(IdCellulare);
//            if (txtCellulare.value != '') {
//                if (parseInt(Number(txtCellulare.value)) != txtCellulare.value) {
//                    alert("Il campo 'Cellulare' può contenere solo numeri.");
//                    txtCellulare.focus();
//                    return false;
//                }
//            }

//            var IdDataNascita = CostruisciId("txtDataNascita");
//            var txtDataNascita = document.getElementById(IdDataNascita);
//            if (txtDataNascita.value == '') {
//                alert("Inserire la Data di Nascita.");
//                txtDataNascita.focus();
//                return false;
//            }

//            //Checks for dd/mm/yyyy format.
//            var currVal = txtDataNascita.value; 
//            var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
//            var dtArray = currVal.match(rxDatePattern); // is format OK?
//            if (dtArray == null) {
//                alert("La 'Data di nascita' non è valida. Inserire la data nel formato GG/MM/AAAA.")
//                txtDataNascita.focus();
//                return false;
//            }

//            dtDay = dtArray[1];
//            dtMonth = dtArray[3];
//            dtYear = dtArray[5];

//            if (dtMonth < 1 || dtMonth > 12) {
//                alert("La 'Data di nascita' non è valida. Inserire la data nel formato GG/MM/AAAA.")
//                txtDataNascita.focus();
//                return false;
//            }
//            else if (dtDay < 1 || dtDay > 31) {
//                alert("La 'Data di nascita' non è valida. Inserire la data nel formato GG/MM/AAAA.")
//                return false;
//            }
//            else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31) {
//                alert("La 'Data di nascita' non è valida. Inserire la data nel formato GG/MM/AAAA.")
//                txtDataNascita.focus();
//                return false;
//            }
//            else if (dtMonth == 2) {
//                var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
//                if (dtDay > 29 || (dtDay == 29 && !isleap)) {
//                    alert("La 'Data di nascita' non è valida. Inserire la data nel formato GG/MM/AAAA.")
//                    txtDataNascita.focus();
//                    return false;
//                }
//            }


//            var idComuneNascita = CostruisciId("ddlComuneNascita");
//            var ddlComuneNascita = document.getElementById(idComuneNascita);
//            var comuneNascitaSelectedIndex = ddlComuneNascita.selectedIndex;
//            if (comuneNascitaSelectedIndex <= 0) {
//                alert("Selezionare il Comune di Nascita.");
//                ddlComuneNascita.focus();
//                return false;
//            }
//            
//            var idCAP = CostruisciId("txtCAP");
//            var txtCAP = document.getElementById(idCAP);
//            var idComuneResidenza = CostruisciId("ddlComuneResidenza");
//            var ddlComuneResidenza = document.getElementById(idComuneResidenza);
//            var comuneResidenzaSelectedIndex = ddlComuneResidenza.selectedIndex;
//            var idIndirizzo = CostruisciId("txtIndirizzo");
//            var txtIndirizzo = document.getElementById(idIndirizzo);
//            var idCivico = CostruisciId("txtCivico");
//            var txtCivico = document.getElementById(idCivico);

//            if ((txtCAP.value != '') || (comuneResidenzaSelectedIndex > 0) || (txtIndirizzo.value != '') || (txtCivico.value != '')) {

//                if (comuneResidenzaSelectedIndex <= 0) {
//                    alert("Selezionare il Comune di Residenza.");
//                    ddlComuneResidenza.focus();
//                    return false;
//                }

//                if (txtIndirizzo.value == '') {
//                    alert("Compilare il campo Indirizzo.");
//                    txtIndirizzo.focus();
//                    return false;
//                }

//                if (txtCivico.value == '') {
//                    alert("Immettere il numero Civico.");
//                    txtCivico.focus();
//                    return false;
//                }

//                if (txtCAP.value == '') {
//                    alert("Inserire il Cap.");
//                    txtCAP.focus();
//                    return false;
//                }
//                               
//            }

////            if (txtCivico.value != '') {
////                if (parseInt(Number(txtCivico.value)) != txtCivico.value) {
////                    alert("Il campo 'Numero Civico' può contenere solo numeri.");
////                    txtCivico.focus();
////                    return false;
////                }
////            }

//            if (txtCAP.value != '') {
//                if (parseInt(Number(txtCAP.value)) != txtCAP.value) {
//                    alert("Il campo 'C.A.P.' può contenere solo numeri.");
//                    txtCAP.focus();
//                    return false;
//                }
//            }

//            return true;
       };

        function CodiceFiscaleUpperCase() {
            var idCodiceFiscale = CostruisciId("txtCodiceFiscale");
            var txtCodiceFiscale = document.getElementById(idCodiceFiscale);
            txtCodiceFiscale.value = txtCodiceFiscale.value.toUpperCase();
        };
          /* ]]> */

	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <fieldset class="ContornoPagina">
        <legend id="lgContornoPagina" runat="server">Risorsa Ente</legend>
        <asp:HyperLink ID="imgRisorsaVirtuale" NavigateUrl="~/WfrmGestioneServizi.aspx?Servizi=Risorsa" runat="server">Acquisizione Servizi</asp:HyperLink> <br />
        <br />
        <asp:label id="lblErr"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <p style="text-align:right">
            <asp:label id="lblInfoRisorsa"  runat="server" CssClass="bold" Visible="False"></asp:label>
        </p>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitoloPagina" runat="server"  Text="Risorsa Ente"></asp:Label></h2>
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="(*)Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="(*)Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div  class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTitolo" CssClass="label" AssociatedControlID="txtTitolo" runat="server" Text="Titolo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtTitolo" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblPosizione" CssClass="label" AssociatedControlID="txtPosizione" runat="server" Text="Posizione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtPosizione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div  class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="txtCodiceFiscale" runat="server" Text="(*)Codice Fiscale"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceFiscale" CssClass="textbox_Maiuscolo" runat="server" Onchange="CodiceFiscaleUpperCase()"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEmail" CssClass="label" AssociatedControlID="txtEmail" runat="server" Text="Email"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtEmail" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div  class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTelefono" CssClass="label" AssociatedControlID="txtTelefono" runat="server" Text="Telefono"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtTelefono" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblFax" CssClass="label" AssociatedControlID="txtFax" runat="server" Text="Fax"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtFax" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div  class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="Label1" CssClass="label" AssociatedControlID="txtCellulare" runat="server" Text="Cellulare"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCellulare" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDataNascita" CssClass="label" AssociatedControlID="txtDataNascita" runat="server" Text="(*)Data di nascita"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDataNascita" CssClass="textbox" runat="server" Width="75px" MaxLength="10"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProvinciaNascita" CssClass="label" AssociatedControlID="ddlProvinciaNascita" runat="server" Text="(*)Provincia/ Nazione di Nascita"></asp:Label>
                </div>
                <div class="colOggetti" style="width:25%">       
                    <asp:dropdownlist id="ddlProvinciaNascita" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>           
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:CheckBox ID="ChkEsteroNascita" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero Nazione di Nascita" runat="server" />
                </div>  
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblComuneNascita" CssClass="label" AssociatedControlID="ddlComuneNascita" runat="server" Text="(*)Comune di nascita"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="ddlComuneNascita" CssClass="ddlClass" runat="server"></asp:dropdownlist>         
                </div>    
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProvinciaResidenza" CssClass="label" AssociatedControlID="ddlProvinciaResidenza" runat="server" Text="Provincia/Nazione di Residenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:25%">       
                    <asp:dropdownlist id="ddlProvinciaResidenza" CssClass="ddlClass" AutoPostBack="true" runat="server"  ></asp:dropdownlist>           
                </div> 
                <div class="colOggetti" style="width:10%">
                    <asp:CheckBox ID="ChkEsteroResidenza" AutoPostBack="true" Text="Estero" ToolTip="Flag Estero Nazione di Nascita" runat="server" />
                </div> 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblComuneResidenza" CssClass="label" AssociatedControlID="ddlComuneResidenza" runat="server" Text="Comune di residenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="ddlComuneResidenza" CssClass="ddlClass" runat="server">
                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                    </asp:dropdownlist>         
                </div>    
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblIndirizzoResidenza" CssClass="label" runat="server" Text="Indirizzo di residenza" AssociatedControlID="txtIndirizzo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="textbox" onkeyup="javascript: SetContextKey();"></asp:TextBox>
                    <asp:AutoCompleteExtender 
                        ID="AutoCompleteExtenderResidenza" 
                        TargetControlID="txtIndirizzo" 
                        ContextKey ="ddlComuneResidenza"
                        CompletionListCssClass="ddl_Autocomplete"
                        UseContextKey="true"
                        CompletionInterval="100" EnableCaching="false" 
                        runat="server" MinimumPrefixLength="5" ServiceMethod="GetCompletionList" >
                    </asp:AutoCompleteExtender>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblNumeroCivico"  runat="server" Text="Numero civico" AssociatedControlID="txtCivico"></asp:Label>
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:TextBox ID="txtCivico" runat="server" CssClass="textbox" ></asp:TextBox>
                </div>
                <div class="collable" style="width:5%">
                     <asp:ImageButton ID="infocivicoRec" runat="server" AlternateText="Informazioni Civico di Residenza" 
                        ImageUrl="Images/info_small.png" style="width:20px;height:20px"  
                        ToolTip="NEL CAMPO CIVICO E' POSSIBILE INSERIRE SOLO I SEGUENTI FORMATI:
                                - 21
                                - 21/A 
                                - 21/A5 
                                - 21 BIS 
                                - KM 21,500 
                                OPPURE IL VALORE SNC" disabled="disabled"/>
                </div>
 
                <div class="collable" style="width:10%">
                    <asp:Label ID="lblCAP" CssClass="label" runat="server" Text="C.A.P." AssociatedControlID="txtCAP"  ></asp:Label>
                </div>
                <div class="colOggetti" style="width:5%">
                    <asp:TextBox ID="txtCAP" CssClass="textbox" runat="server"></asp:TextBox>

                </div>
                <div class="collable" style="width:5%">
                    <asp:ImageButton ID="imgCap" runat="server" title="Seleziona il Cap di Residenza" ImageUrl="Images/valida_small.png" AlternateText="Seleziona il Cap di Residenza"  style="width:20px;height:20px" />
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDettaglioRecapito" CssClass="label" runat="server" Text="Dettaglio recapito" AssociatedControlID="TxtDettaglioRecapito" ></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="TxtDettaglioRecapito" CssClass="textbox" runat="server" ToolTip="Dettagli aggiuntivi del recapito"></asp:TextBox>
                </div>
                <div class="colOggetti" style="width:15%">
                     <asp:Label ID="lblCorso" CssClass="label" runat="server" 
                         Text="Corso OLP da frequentare S/N" AssociatedControlID="chkCorsoOlp" 
                         Visible="False" ></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:checkbox id="chkCorsoOlp" runat="server" Visible="False"></asp:checkbox>
                 </div>
            </div>
            <div class="row">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEsperienzaServizioCivile" CssClass="label" AssociatedControlID="ddlEsperienzaServizioCivile" runat="server" Text="Esperienza di Servizio Civile"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="ddlEsperienzaServizioCivile" CssClass="ddlClass" runat="server">
                        <asp:ListItem Value="0" Selected="True">Selezionare</asp:ListItem>
						<asp:ListItem Value="1">Con esperienza</asp:ListItem>
						<asp:ListItem Value="2">Senza esperienza</asp:ListItem>
                    </asp:dropdownlist>           
                </div>  
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCorsoFormazione" CssClass="label" AssociatedControlID="ddlCorso" runat="server" Text="Corso di formazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="ddlCorso" CssClass="ddlClass" runat="server">
                        <asp:ListItem Value="0" Selected="True">Selezionare</asp:ListItem>
						<asp:ListItem Value="3">Non Necessario</asp:ListItem>
						<asp:ListItem Value="2">Da Fare</asp:ListItem>
						<asp:ListItem Value="1">Fatto</asp:ListItem>
                    </asp:dropdownlist>         
                </div>
            </div>
<%--            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDichiarazioneRequisitiMinimi" CssClass="label" AssociatedControlID="chkDichiarazioneRequisitiMinimi" runat="server" Text="(*)Requisiti minimi"></asp:Label>
                </div>
                <div class="colOggetti" style="width:85%">
                    <asp:CheckBox ID="chkDichiarazioneRequisitiMinimi" Text="La risorsa soddisfa i requisiti minimi per i ruoli che ricopre" ToolTip="Spuntando la casella si dichiara che la risorsa soddisfa i requisiti minimi per i ruoli che ricopre" runat="server" />
                </div>  
            </div>--%>
			<div class="row" runat="server" id="rowNoCV">
				<div class="collable" style="width: 15%">
					<asp:Label ID="lblCV" CssClass="label" AssociatedControlID="fileCV" runat="server" Text="Curriculum Vitae"></asp:Label>
				</div>
                <div class="colOggetti" style="width: 35%">
                    <asp:Button ID="cmdCaricaFile" runat="server" OnClientClick="return ShowPopUp()" CssClass="Pulsante" Text="Carica File"/> 
				</div>
			</div>
			<div class="row" runat="server" id="rowCV">
				<table class="table" cellspacing="0" cellpadding="2" rules="all" border="1" id="MainContent_dtgConsultaDocumenti" style="font-size: Small; width: 100%; border-collapse: collapse;">
					<caption>
						Curriculum Vitae
					</caption>
					<tbody>
						<tr>
							<th scope="col">Download</th>
							<th scope="col">Nome File</th>
							<th scope="col">Data Inserimento</th>
							<th scope="col">Hash Value</th>
							<th scope="col"></th>
						</tr>
						<tr class="tr" align="center" runat="server" id="rowAllegato">
							<td>
								<asp:ImageButton runat="server" ID="btnDownloadCV" ImageUrl="images/giu_small.png" AlternateText="Scarica Curriculum Vitae" ToolTip="Scarica Curriculum Vitae" />
							</td>
							<td>
								<asp:Literal runat="server" ID="txtCVFilename"></asp:Literal>
							</td>
							<td>
								<asp:Literal runat="server" ID="txtCVData"></asp:Literal></td>
							<td>
								<asp:Literal runat="server" ID="txtCVHash"></asp:Literal></td>
							<td>
								<asp:ImageButton runat="server" ID="btnModificaCV" OnClientClick="return ShowPopUp()" ImageUrl="images/ripristina_small.png" AlternateText="Modifica Curriculum Vitae" ToolTip="Modifica Curriculum Vitae" />
								<asp:ImageButton runat="server" ID="btnEliminaCV" ImageUrl="images/canc_small.png" AlternateText="Elimina Curriculum Vitae" ToolTip="Elimina Curriculum Vitae" />
							</td>
						</tr>
					</tbody>
				</table>

			</div>

			<br />
            <p style="text-align:center">
                <asp:label id="LblAnnoCorso" runat="server" CssClass="bold" Text=" " 
                    AssociatedControlID="dtgRuoli"></asp:label>
            </p>
            <%--<asp:datagrid id="dtgRuoloPrincipale" runat="server" CssClass="table" Width="100%" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="4" UseAccessibleHeader="True">
			    <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle></HeaderStyle>
			    <Columns>
                    <asp:BoundColumn Visible="False" DataField="IDRuolo" HeaderText="IDComune"></asp:BoundColumn>
				    <asp:BoundColumn DataField="Ruolo" HeaderText="Ruolo"></asp:BoundColumn>
				    <asp:BoundColumn DataField="Principale" HeaderText="Principale">
					    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					    <ItemStyle HorizontalAlign="Center"></ItemStyle>
				    </asp:BoundColumn>
				    <asp:BoundColumn DataField="Accreditato" HeaderText="Accreditamento">
					    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					    <ItemStyle HorizontalAlign="left"></ItemStyle>
				    </asp:BoundColumn>
				    <asp:BoundColumn DataField="Visibilit&#224;" HeaderText="Visibilit&#224;">
					    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					    <ItemStyle HorizontalAlign="Center"></ItemStyle>
				    </asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Dettagli Accreditamento"> 
                        <ItemTemplate>
					        <asp:ImageButton ID="ImgDettagliAccreditamento" CommandName="Sostituzione" ToolTip="Visualizza Dettagli Risorsa" AlternateText="Visualizza Dettagli Risorsa" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Dettagli Risorsa"> 
                        <ItemTemplate>
					        <asp:ImageButton ID="ImgDettagliRisorsa" CommandName="Dettagli" ToolTip="Visualizza Cronologia Risorsa" AlternateText="Visualizza Cronologia Risorsa" runat="server" ImageURL="images/cronologia_small.png" CausesValidation="false"></asp:ImageButton>                         
                        </ItemTemplate>
                    </asp:TemplateColumn>
			    </Columns>
			    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		    </asp:datagrid>--%>
            <asp:datagrid id="dtgRuoli" runat="server" CssClass="table" Width="100%" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	            <SelectedItemStyle BackColor="White"></SelectedItemStyle>
	            <EditItemStyle></EditItemStyle>
	            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	            <ItemStyle CssClass="tr" HorizontalAlign="center"></ItemStyle>
	            <HeaderStyle></HeaderStyle>
	            <Columns>
                    <asp:BoundColumn Visible="False" DataField="IDRuolo" HeaderText="IDRuolo"></asp:BoundColumn>
		            <asp:BoundColumn Visible="False" DataField="Accreditato" HeaderText="Accreditato"></asp:BoundColumn>
		            <asp:BoundColumn Visible="False" DataField="identepersonaleruolo" HeaderText="entepersonaleruoli.identepersonale"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Ruolo" HeaderText="Ruolo">
                        <ItemStyle HorizontalAlign="left"/>
                    </asp:BoundColumn>
		            <asp:BoundColumn DataField="Stato" HeaderText="Iscrizione"></asp:BoundColumn>
		            <asp:BoundColumn Visible="False" DataField="ruoloaccreditamento" HeaderText="ruoloaccreditamento"></asp:BoundColumn>
		            <asp:BoundColumn Visible="False" DataField="DataFineValidit&#224;" HeaderText="DataFineValidit&#224;"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Assegna Ruolo">
	                    <ItemTemplate>
		                    <asp:CheckBox id="chkAssegnaRuolo" Text="Sel"  TextAlign="Left"  toolTip="Assegna Ruolo" AlternateText="Assegna Ruolo" runat="server" OnCheckedChanged="ChkAssegnaRuoloChange" AutoPostBack="true"></asp:CheckBox>
	                    </ItemTemplate>
                    </asp:TemplateColumn>
		            <asp:TemplateColumn HeaderText="Dettagli Iscrizione"> 
			            <ItemTemplate>
				            <asp:ImageButton ID="ImgDettagliAccreditamento" CommandName="Sostituzione" ToolTip="Visualizza Dettagli Risorsa" AlternateText="Visualizza Dettagli Risorsa" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
			            </ItemTemplate>
		            </asp:TemplateColumn>
		            <asp:TemplateColumn HeaderText="Dettagli Risorsa"> 
			            <ItemTemplate>
				            <asp:ImageButton ID="ImgDettagliRisorsa" CommandName="Dettagli" ToolTip="Visualizza Cronologia Risorsa" AlternateText="Visualizza Cronologia Risorsa" runat="server" ImageURL="images/cronologia_small.png" CausesValidation="false"></asp:ImageButton>                         
			            </ItemTemplate>
		            </asp:TemplateColumn>
	            </Columns>
            </asp:datagrid>
            <div class="RigaPulsanti">
                <%--<asp:Button ID="imgRuoli" runat="server" CssClass="Pulsante" Text="Ruoli" />--%>
                <asp:Button ID="cmdAnnullaCancellazione" runat="server" CssClass="Pulsante" Text="Annulla Cancellazione" />
                <asp:Button ID="cmdSalva" runat="server" CssClass="Pulsante" Text="Salva"/> 
                <asp:Button ID="cmdSalvaDoc" runat="server" CssClass="Pulsante" Text="Salva Documenti" Visible="false"/> 
                <asp:Button ID="imgRilascia" runat="server" CssClass="Pulsante" Text="Rilascia" />
                <asp:Button ID="imgCancella" runat="server" CssClass="Pulsante" Text="Cancella" />
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
            <asp:HiddenField ID="IdModVal1" runat="server" />
            <asp:HiddenField ID="txtIDComunes" runat="server" />

            <asp:HiddenField ID="IdVal1" runat="server" />
            
            <asp:HiddenField ID="chkRuoli" runat="server" />
            <asp:HiddenField ID="IdEntePersonale" runat="server" />
            <asp:HiddenField ID="chkStatoEnte" runat="server" />
        </div>
    </fieldset>
    <%--POPUP File--%>
    <asp:ModalPopupExtender runat="server" ID="popUpload" TargetControlID="hfPopup" 
        PopupControlID="pnlUpload" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        PopupDragHandleControlID="pnlEsperienzaArea" BehaviorID="popUpControl" DynamicServicePath="" Enabled="True" CancelControlID="btnCloseUpload"> 
    </asp:ModalPopupExtender>
	<asp:Panel ID="pnlUpload" runat="server" Style="background-color: White; padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
        <asp:HiddenField runat="server" ID="hfPopup" />
        <asp:label id="lblErroreUpload"  runat="server" CssClass="msgErrore" Visible="False" AssociatedControlID="cmdAllega"></asp:label>
        <p class="msgInfo">Caricamento Curriculum Vitae</p>
        <fieldset>
			<input id="fileCV" type="file" runat="server" />
            <div style="margin-top: 1em;"></div>
            <asp:Button ID="cmdAllega" runat="server" CssClass="Pulsante" Text="Carica File"/>
            <asp:Button ID="btnCloseUpload" runat="server" CssClass="Pulsante" Text="Chiudi"/>
        </fieldset>
	</asp:Panel>
    <!--POPUP --ALERT Inizio-->
        <asp:HiddenField ID="HFALERT" runat="server" />		
    <asp:ModalPopupExtender runat="server" ID="mpeALERT" TargetControlID="HFALERT" 
        PopupControlID="pnlUploadALERT" BackgroundCssClass="modalBackgroundPopUp" DropShadow="True"
        PopupDragHandleControlID="pnlEsperienzaArea" DynamicServicePath="" Enabled="True"
        CancelControlID="btnCloseALERT"> 
    </asp:ModalPopupExtender>
        <asp:Panel ID="pnlUploadALERT" runat="server" Style="background-color: White;
        padding: 20px; width: 100%; overflow-y: auto; max-height: 90vh">
        <p class="msgInfo" id="alertTitolo" runat="server">ALERT</p>
        <fieldset>
            <asp:Label ID="lblAlert" runat="server" CssClass="bold" Text="prova"></asp:Label>
            <div style="margin-top: 1em;text-align:center"><asp:Button ID="btnCloseALERT" runat="server" CssClass="Pulsante" Text="OK" style="text-align:center"/></div>
        </fieldset>
    </asp:Panel>
    <!--POPUP --ALERT Fine-->
</asp:Content>
