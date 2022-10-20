<%@ Page Title="Creazione Utenze" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCreaUtenzeUNSC.aspx.vb" Inherits="Futuro.WfrmCreaUtenzeUNSC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

//<![CDATA[
////        var prefissoIdClient = "MainContent_";

////        function CostruisciId(IdServer) {
////            var IdClient = prefissoIdClient + IdServer
////            return IdClient
////        }

////        function ValidazioneClient() {
////            var idEmail = CostruisciId("txtEmail");
////            var txtEmail = document.getElementById(idEmail);
////            var i = new RegExp("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$");

////            if (txtEmail.disabled == false && txtEmail.value != '') {
////                if (!i.test(txtEmail.value)) {
////                    alert("Il formato Email non è valido!");
////                    txtEmail.focus();
////                    return false;
////                }
////            }

////            var IdDurataPassword = CostruisciId("TxtDurataPassword");
////            var txtDurataPassword = document.getElementById(IdDurataPassword);

////            if (txtDurataPassword != null) {
////                if (txtDurataPassword.disabled == false && txtDurataPassword.value != '') {
////                    if (parseInt(Number(txtDurataPassword.value)) != txtDurataPassword.value) {
////                        alert("Il campo 'Durata Password' può contenere solo numeri.");
////                        txtDurataPassword.focus();
////                        return false;
////                    }
////                }
////            }
////            
////            return true
////        }
 //]]>

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend id="lgTitoloPagina" runat="server"></legend>
        <div class="row" style="text-align:right">
            <asp:PlaceHolder ID="phAssociaArea" runat="server">
                <asp:ImageButton ID="ImgAssociaArea" runat="server" ImageUrl="Images/Icona_volontario_small.png" ToolTip="Associa Area" AlternateText="Associa Area" ImageAlign="Middle"/>
                <asp:Label ID="lblAssociaArea" runat="server"  AssociatedControlID="ImgAssociaArea" Text="Associa Area"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phResetPassword" runat="server" Visible="false">
                <asp:ImageButton id="ImgResetPassword" runat="server" ImageUrl="images/Icona_Progetto_small.png" ToolTip="Reset Password" AlternateText="Reset Password" ImageAlign="Middle"/>
                <asp:Label ID="lblResetPassword" runat="server"  AssociatedControlID="ImgResetPassword" Text="Reset Password"></asp:Label>
            </asp:PlaceHolder>
        </div>
        <asp:label id="lblmsgConf"  runat="server" CssClass="msgConferma" Visible="False"></asp:label>
        <asp:label id="lblMessaggio"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitoloPagina" runat="server"></asp:Label></h2>
            </div>
            <div class="RigaVuota">&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTipologiaUtenza" CssClass="label" runat="server" Text="Tipologia Utenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:RadioButton ID="optTipUteU" Text="U" GroupName="TipoUtenza" runat="server" Checked="True" AutoPostBack="true"/>
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:RadioButton ID="optTipUteR" Text="R" GroupName="TipoUtenza" runat="server" AutoPostBack="true"/>
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:RadioButton ID="optTipUteE" Text="E" GroupName="TipoUtenza" runat="server" Enabled="False" AutoPostBack="true"/>
                </div>
            </div>
        </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota">&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="(*)Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server" MaxLength="50"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="(*)Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server" MaxLength="50"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblUtente" CssClass="label" AssociatedControlID="txtUtente" runat="server" Text="(*)Utente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtTipoUtenza" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small" Width="5%" ReadOnly="True" ToolTip="Tipo Utenza"></asp:textbox>       
                    <asp:TextBox ID="txtUtente" CssClass="textbox" runat="server" Width="83%" MaxLength="9"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEmail" CssClass="label" AssociatedControlID="txtEmail" runat="server" Text="(*)E-Mail"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtEmail" CssClass="textbox" runat="server" MaxLength="255"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="Label1" CssClass="label" AssociatedControlID="TxtDominioAD" runat="server" Text="(*)Utenza Dominio"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtDominioAD" CssClass="textbox" runat="server" MaxLength="255"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDurataPassword" CssClass="label" AssociatedControlID="TxtDurataPassword" runat="server" Text="Durata Password"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtDurataPassword" CssClass="textbox" runat="server"  MaxLength="3" Width="30%" Text="180"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div id="dvCredenzialiEmail" runat="server" class="colOggetti" style="width:20%">
                    <asp:checkbox id="ChkCredenzialiEmail" runat="server"  Text="Invio Credenziali per EMail"></asp:checkbox>
                </div>
                <div class="colOggetti" style="width:30%">&nbsp;</div>
                <div class="colOggetti" id="dvStampaCredenziali" runat="server" style="width:50%">
                    <asp:checkbox id="ChkStampaCredenziali" runat="server"  Text="Stampa Credenziali"></asp:checkbox>
                </div>
            </div>
        </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota">&nbsp;</div>               
                <asp:datagrid id="dtgProfiliUtente" runat="server" CssClass="table"  Width="100%" ToolTip="Elenco Profili Utente"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
		        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
			        <asp:BoundColumn  DataField="idprofilo" Visible="False">
                    </asp:BoundColumn>
			        <asp:BoundColumn DataField="Descrizione" HeaderText="Tipo Profilo">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
			        <asp:TemplateColumn HeaderText="Sel.">
				        <HeaderStyle Width="20%" ></HeaderStyle>
				        <ItemTemplate>
					        <asp:CheckBox id="chkSeleziona" Text="Sel" TextAlign="Left" toolTip="Seleziona Profili" runat="server"></asp:CheckBox>
				        </ItemTemplate>
			        </asp:TemplateColumn>
		        </Columns>
	        </asp:datagrid>
                <br />
          <%--  <div class="row">
            <div class="colOggetti" style="width:15%">
                        <asp:RadioButton ID="optUtenzaProfilo" GroupName="Utenza" runat="server" Checked="True" Text="Tipo Profilo" AutoPostBack="true"/>
                    </div>
       
                <div class="colOggetti" style="width:15%">
                    <asp:RadioButton ID="optUtenzaProfiloa" GroupName="Utenza" runat="server" Checked="True" Text="Tipo Profilo" AutoPostBack="true"/>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="cboProfili" runat="server" CssClass="ddlClass" ToolTip="Tipo Profilo" Visible="false"></asp:dropdownlist>    
                </div>
                <div class="colOggetti" style="width:15%">
                    <asp:RadioButton ID="optUtenzaSimile" GroupName="Utenza" runat="server" Text="Profilo Simile" AutoPostBack="True"/>
                </div>
                <div class="colOggetti" style="width:35% ">
                    <asp:textbox id="txtTipoUtenzaSim" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small" Width="5%" ReadOnly="True" ToolTip="Profilo Simile"></asp:textbox>       
                    <asp:TextBox id="txtUtenzaSim" CssClass="textbox" runat="server" Width="75%" ToolTip="Profilo Simile"></asp:TextBox> 
                    <asp:imagebutton id="cmdRicerca" AlternateText="Ricerca Utenze" ToolTip="Ricerca Utenze" style="CURSOR: hand; vertical-align:middle" runat="server" ImageUrl="images/lenteIngrandimento_small.png" ></asp:imagebutton>        
                </div>
            </div>--%>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRegioneCompetenza" CssClass="label" AssociatedControlID="cboRegComp" runat="server" Text="Regione Competenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:dropdownlist id="cboRegComp" runat="server" CssClass="ddlClass"></asp:dropdownlist>         
                </div>
                <div class="colOggetti" style="width:50%">       
                    <asp:checkbox id="chkReadOnly" runat="server" Text="Solo Lettura"></asp:checkbox>         
                </div>
            </div>  
        </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota">&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTipoPrfiloRead" CssClass="label" AssociatedControlID="cboProfiliRead" runat="server" Text="Tipo Profilo Read"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:dropdownlist id="cboProfiliRead" runat="server" CssClass="ddlClass"></asp:dropdownlist>         
                </div>
            </div>
        </div>
        <div class="wrapper" style="width:100%; border-style:none" >
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaPulsanti">
                <asp:Button ID="imgSalva" CssClass="Pulsante" runat="server" Text="Salva"  /> 
                <asp:Button ID="cmdGenera" runat="server" CssClass="Pulsante" Text="Genera"  />
                <asp:Button ID="cmdAnnulla" CssClass="Pulsante" runat="server" Text="Annulla" />
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
            </div>
        </div>
    </fieldset>
</asp:Content>
