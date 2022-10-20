<%@ Page Title="Richiesta Account e Password" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicAccount.aspx.vb" Inherits="Futuro.WfrmRicAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

    <style type="text/css"> .ui-datepicker {font-size: 11px;}</style>

    <script  type="text/javascript">

//<![CDATA[
        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };

        $(function () {
            var DataRicezione = CostruisciId("TxtDataRicezioneCarta");
            var readonly = '<%=TxtDataRicezioneCarta.ReadOnly%>';
            if (readonly == "False") {
                var sharpDataRicezione = "#" + DataRicezione
                $("" + sharpDataRicezione + "").datepicker();
            }
        });

        function ValidazioneClientSalva() {

            var idDenominazione = CostruisciId("txtdenominazione");
            var txtDenominazione = document.getElementById(idDenominazione);
            if (txtDenominazione.value == '') {
                alert("Inserire Denominazione!");
                txtDenominazione.focus();
                return false;
            }

            var idCodiceFiscale = CostruisciId("txtCodFis");
            var txtCodiceFiscale = document.getElementById(idCodiceFiscale);
            if (txtCodiceFiscale.value == '') {
                alert("Inserire il Codice Fiscale!");
                txtCodiceFiscale.focus();
                return false;
            }
            if (txtCodiceFiscale.value.length < 11) {
                alert("Attenzione.Il codice fiscale deve essere di almeno 11 caratteri!");
                txtCodiceFiscale.focus();
                return false;  
            }

            var idEmail = CostruisciId("txtEmail");
            var txtEmail = document.getElementById(idEmail);
            if (txtEmail.value == '') {
                alert("Inserire Email!");
                txtEmail.focus();
                return false;
            }

//            var i = new RegExp("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$");
//            if (!i.test(txtEmail.value)) {
//                alert("Il formato Email non è valido!");
//                txtEmail.focus();
//                return false;
//            }

            var idPrefisso = CostruisciId("txtprefisso");
            var txtPrefisso = document.getElementById(idPrefisso);
            if (txtPrefisso.value != '') {
                if (parseInt(Number(txtPrefisso.value)) != txtPrefisso.value) {
                    alert("Il Prefisso Telefono può contenere solo numeri.");
                    txtPrefisso.focus();
                    return false;
                }
            }

            var idTelefono = CostruisciId("txtTelefono");
            var txtTelefono = document.getElementById(idTelefono);
            if (txtTelefono.value != '') {
                if (parseInt(Number(txtTelefono.value)) != txtTelefono.value) {
                    alert("Il Telefono può contenere solo numeri.");
                    txtTelefono.focus();
                    return false;
                }
            }

            var idDataRicezione = CostruisciId("TxtDataRicezioneCarta");
            var txtDataRicezione = document.getElementById(idDataRicezione);
            if (txtDataRicezione.value == '') {
                alert("Inserire Data Ricezione Cartacea!");
                txtDataRicezione.focus();
                return false;
            }

            var idRicezione = CostruisciId("TxtDataRicezioneCarta");
            var txtRicezione = document.getElementById(idRicezione);
            if (txtRicezione.value != '') {
                if (isCorrectFormatDate(txtRicezione, "Data Ricezione") == false) {
                    return false;
                }
            }

            return true
        }

        function isCorrectFormatDate(txtDataRiferimento, nomeDataRiferimento) {
            
            //Checks for dd/mm/yyyy format.
            var currVal = txtDataRiferimento.value;
            var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
            var dtArray = currVal.match(rxDatePattern); // is format OK?
            if (dtArray == null) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }

            dtDay = dtArray[1];
            dtMonth = dtArray[3];
            dtYear = dtArray[5];

            if (dtMonth < 1 || dtMonth > 12) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }
            else if (dtDay < 1 || dtDay > 31) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }
            else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31) {
                alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                txtDataRiferimento.focus();
                return false;
            }
            else if (dtMonth == 2) {
                var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
                if (dtDay > 29 || (dtDay == 29 && !isleap)) {
                    alert("La '" + nomeDataRiferimento + "' non è valida. Inserire la data nel formato GG/MM/AAAA.")
                    txtDataRiferimento.focus();
                    return false;
                }
            }

            return true;
        }
//]]>

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Richiesta Account e Password</legend>
        <asp:label id="lblMessaggi"  runat="server" CssClass="msgErrore"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Richiesta Account e Password"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
             <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="ddlsuffisso" runat="server" Text="(*)Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:85%">       
                    <asp:dropdownlist id="ddlsuffisso" runat="server" CssClass="ddlClass" Width="30%" AutoPostBack="true"></asp:dropdownlist> 
                    <asp:textbox id="txtdenominazione" runat="server" CssClass="textbox_Maiuscolo" Width="67%" ToolTip="Denominazione" ></asp:textbox>        
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">
                    <asp:label id="lblCodiceFiscale" CssClass="label" runat="server" AssociatedControlID="txtCodFis" Text="(*)Cod. Fis."></asp:label>
                </div>
                <div class="colOggetti" style="width:50%">
                    <asp:textbox id="txtCodFis" runat="server" CssClass="textbox"></asp:textbox>
                </div>
             </div>
                 <div class="row" >
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEmail" CssClass="label" AssociatedControlID="txtEmail" runat="server" Text="(*)E-mail"></asp:Label>
                </div>
                <div class="colOggetti" style="width:80%">       
                     <asp:textbox id="txtEmail" runat="server" CssClass="textbox"></asp:textbox>      
                </div>
                 </div>
                 <div class="row" >
                <div class="collable" style="width:15%">
                    <asp:label id="lblPEC" CssClass="label" runat="server" AssociatedControlID="txtEmailCertificata" Text="PEC"></asp:label>
                </div>
                <div class="colOggetti" style="width:80%">
                    <asp:textbox id="txtEmailCertificata" runat="server" CssClass="textbox"></asp:textbox>
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRichiedente" CssClass="label" AssociatedControlID="txtRichiedente" runat="server" Text="Richiedente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:28%">       
                    <asp:textbox id="txtRichiedente" runat="server" CssClass="textbox_Maiuscolo"></asp:textbox>         
                </div>
                <div class="collable" style="width:6%">
                    <asp:label id="lblTelefono" CssClass="label" runat="server" AssociatedControlID="txtTelefono" Text="Telefono"></asp:label>
                </div>
                <div class="colOggetti" style="width:24%">
                    <asp:textbox id="txtprefisso" runat="server" CssClass="textbox" Width="20%" ToolTip="Prefisso Telefono"></asp:textbox>
                    <asp:textbox id="txtTelefono" runat="server" CssClass="textbox" Width="68%"></asp:textbox>
                </div>
                <div class="collable" style="width:12%">      
                    <asp:Label ID="lblDataRicezione" CssClass="label" AssociatedControlID="TxtDataRicezioneCarta" runat="server" Text="(*)Data Ricezione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:15%">       
                    <asp:TextBox ID="TxtDataRicezioneCarta" CssClass="textbox" runat="server" Width="85%" MaxLength="10"></asp:TextBox>         
                </div>
                <div class="RigaVuota" >&nbsp;</div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Conferma" OnClientClick="return ValidazioneClientSalva()"/>
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
        </div>  
    </fieldset>
    <asp:HiddenField ID="txtIDregcomp" runat="server"/>
</asp:Content>
