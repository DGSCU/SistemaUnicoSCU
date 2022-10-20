<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneRequisiti.aspx.vb" Inherits="Futuro.WfrmGestioneRequisiti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
   	<script type="text/javascript">
   	    function ControllaCheckEsito(IdEsito) {
   	        var NomeOggettoCheckAssegna;

   	        NomeOggettoCheckAssegna = 'chkAssegna' + IdEsito;
   	        //alert (document.getElementById(NomeOggettoCheckAssegna).checked);
   	        if (document.getElementById(NomeOggettoCheckAssegna).checked == true) {

   	            document.getElementById(NomeOggettoCheckAssegna).value = 1;
   	            document.getElementById(NomeOggettoCheckAssegna).checked = true;
   	            //alert (document.getElementById(NomeOggettoCheckAssegna).value);
   	        }
   	        else {
   	            //alert (document.getElementById(NomeOggettoCheckAssegna).checked);
   	            document.getElementById(NomeOggettoCheckAssegna).value = 0;
   	            document.getElementById(NomeOggettoCheckAssegna).checked = false;
   	            //alert (document.getElementById(NomeOggettoCheckAssegna).value);

   	        }
   	    }
   	    function SoloNumeri() {
   	        var evtobj = window.event;
   	        var keyascii = evtobj.keyCode;
   	        if (keyascii > 57 || keyascii < 48) {
   	            window.event.keyCode = 0;
   	        }
   	        else {
   	            window.event.keyCode = keyascii;
   	        }
   	    }				
		</script>
        <script type="text/javascript" language="javascript" >
        function Conferma() {
            if (document.all.MainContent_txtDescrizione.value == '') 
			 {
				alert("Inserire la descrizione del requisito.")
				document.all.MainContent_txtDescrizione.focus();
				return false
			 }
            if (document.all.MainContent_txtOrdine.value == '')
			 {
				alert("Inserire il campo ordine.")
				document.all.MainContent_txtOrdine.focus();
				return false;
			 }
        if (document.all.MainContent_ddTipoRequisito.value == "") {
            alert(document.all.MainContent_ddTipoRequisito.value);
				alert("E' necessario indicare il Tipo Requisito.")
				document.all.MainContent_ddTipoRequisito.focus();
				return false;
				}
        if (document.all.MainContent_txtRationale.value == '')
				{
				alert("E' necessario indicare il Rationale.")
				document.all.MainContent_txtRationale.focus();
				return false;
				} 
             }
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset id="Associa" class="ContornoPagina" runat="server">
        <legend>Gestione Requisito Verifica</legend>
        <asp:label id="lblmessaggiosopra" runat="server" Visible="false"></asp:label>
   <div class="wrapper" style="width: 100%">
             <div class="headers">
                <h2>
                    <asp:Label ID="lblTitoloPagina"  runat="server" Text="Gestione Requisito Verifica"></asp:Label>
                </h2>
            </div>

             <div class="RigaVuota">
                &nbsp;
            </div>

             <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtDescrizione" Text="Descrizione"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:85%">
                        <asp:TextBox ID="txtDescrizione" TextMode="MultiLine" Width="95%" runat="server"></asp:TextBox>
                    </div>
            </div>

             <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label ID="Label2" AssociatedControlID="txtOrdine" runat="server" Text="Ordine"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtOrdine" runat="server"></asp:TextBox>
                    </div>
                     <div class="collable" style="width:15%">
                         <asp:Label ID="Label3" runat="server" AssociatedControlID="ddTipoRequisito" Text="Tipo Requisito"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:DropDownList ID="ddTipoRequisito" runat="server">
                        </asp:DropDownList>
                    </div>

            </div>

             <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label ID="Label4" runat="server" AssociatedControlID="txtRationale" Text="Rationale:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:85%">
                        <asp:TextBox ID="txtRationale" TextMode="MultiLine" Width="95%" runat="server"></asp:TextBox>
                    </div>
            </div>

             <div class="row" >
                    <div class="collable" style="width:15%">
                        <asp:Label ID="Label5" runat="server" AssociatedControlID="txtNote" Text="Note:"></asp:Label>
                    </div>
                    <div class="colOggetti" style="width:85%">
                        <asp:TextBox ID="txtNote" TextMode="MultiLine" Width="95%" runat="server"></asp:TextBox>
                    </div>
            </div>

             <div class="RigaPulsanti"> 
                 <asp:Button ID="cmdInserimento" CssClass="Pulsante" Text="Aggiungi" runat="server" />
            </div>

             <div class="row" style="text-align:center">
            
            <asp:datagrid id="dtgEsiti" runat="server" Caption="ELENCO ESITI" ToolTip="Elenco esiti" CssClass="table"  AutoGenerateColumns="False"
								AllowSorting="True">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle CssClass="tr" ></AlternatingItemStyle>
								<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Assegna" HeaderText="Assegna">
										<HeaderStyle Width="4%"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="esito" HeaderText="Esito"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="idesito" HeaderText="idesito"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="IDRequisito" HeaderText="IDRequisito"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="abilitato"></asp:BoundColumn>
								</Columns>
								<PagerStyle NextPageText="&gt;&gt;" 
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
            </div>

             <div class="row" >
              <div class="colOggetti" style="width:33%">
              <asp:dropdownlist id="ddSanzione" runat="server" Visible="False" >
											<asp:ListItem Selected="True"></asp:ListItem>
											<asp:ListItem Value="0">Interno</asp:ListItem>
											<asp:ListItem Value="1">IGF</asp:ListItem>
										</asp:dropdownlist>
              </div>
               <div class="colOggetti" style="width:33%">
               <asp:dropdownlist id="ddAbilitato" runat="server" Visible="False"  AutoPostBack="True">
											<asp:ListItem Value="0">Si</asp:ListItem>
											<asp:ListItem Value="1">No</asp:ListItem>
										</asp:dropdownlist>
              </div>
               <div class="colOggetti" style="width:33%">
               <asp:dropdownlist id="ddlCompetenza" runat="server" Visible="False"  AutoPostBack="True"></asp:dropdownlist>
              </div>
             </div>

             <div class="row">

                    <div class="collable" style="width:15%">
                       
                        &nbsp;
                   </div>
                    <div class="colOggetti" style="width:85%">
                        <asp:TextBox ID="txtRiferimentoCircolare" Visible="false" TextMode="MultiLine" Width="95%" runat="server"></asp:TextBox>
                    </div>

             </div>

             <div class="RigaPulsanti"> 
                 <asp:Button ID="cmdConferma" CssClass="Pulsante" Text="Conferma" OnClientClick="return Conferma()" runat="server" />
                 <asp:Button ID="imgChiudi" CssClass="Pulsante" Text="Chiudi" runat="server"  />
             </div>


   </div>
</fieldset>
</asp:Content>
