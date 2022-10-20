<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmProgrammiCoprogrammazione.aspx.vb" Inherits="Futuro.WfrmProgrammiCoprogrammazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
            <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
    		<script type="text/javascript">
		 /* <![CDATA[ */
        function Ricerca(){
        var IdMsgErrore = CostruisciId("LblErrore");
        var validazioneOk = true;
        var messaggioDiErrore = "";
			if ((document.getElementById(CostruisciId("txtCodEnte")).value=='') & (document.getElementById(CostruisciId("txtDenominazione")).value==''))
			{
                messaggioDiErrore = "É obbligatorio valorizzare almeno uno dei campi di ricerca.";
				AggiornaErrore(messaggioDiErrore, IdMsgErrore);
                validazioneOk = false;
                TornaAdInizioPagina();
			}
            return validazioneOk;
            };
             /* ]]> */
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:Panel DefaultButton="ImgRicerca" runat="server">
<fieldset class="ContornoPagina">
    <div class="wrapper" style="width:100%;border:0px">
      <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestione Enti Coprogrammanti"></asp:Label></h2>
            </div>
         <div class="rowGroup" style="height:auto">
            <div class="RigaVuota"> &nbsp;</div>
		    <asp:label id="LblErrore"  runat="server" CssClass="msgErrore"></asp:label>
  
             <div class="RigaVuota">  &nbsp; </div>
		</div>
        <div class="row" >
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="labelProgramma" AssociatedControlID="lblProgramma"  runat="server" Text="Programma"></asp:Label>
       </div>
       <div class="colOggetti" style="width:85%">
           <asp:Label ID="lblProgramma" CssClass="labelDati"    runat="server"></asp:Label>
       </div>
         
       </div>
       

    <asp:datagrid id="DgEntiAccorpati" runat="server" Width="100%" Caption="Enti Coprogrammanti"
		    AllowPaging="True" CellPadding="2"  PageSize="5" CssClass="table" 
            AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
			<asp:BoundColumn Visible="False" DataField="IdProgramma" 
                HeaderText="IdProgramma"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
			<asp:BoundColumn DataField="codiceente" HeaderText="Cod. Ente"></asp:BoundColumn>
			<asp:BoundColumn DataField="denominazione" HeaderText="Ente"></asp:BoundColumn>
			<asp:BoundColumn DataField="progetti" HeaderText="N° Progetti su Programma">
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="volontari" HeaderText="N&#176; volontari richiesti">
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>	
            <asp:TemplateColumn HeaderText="Rimuovi"> 
                <ItemTemplate >
			        <asp:ImageButton ID="ImageButton2"  style="cursor:pointer;" CommandName="elimina" AlternateText="Elimina Ente" ToolTip='Elimina Ente' runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>      
                </ItemTemplate>
            <ItemStyle Width="5%" />
        </asp:TemplateColumn>
											</Columns>	
         <PagerStyle CssClass="linkPageGrid" HorizontalAlign="Center" 
                Mode="NumericPages" NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" />				
    </asp:datagrid>
      <div class="RigaVuota">&nbsp;</div>
                     <div class="RigaPulsanti">
        <asp:Button ID="ImgChiudi" runat="server" CssClass="Pulsante" Text="Chiudi"  />&nbsp;
   </div>
   </div>
               

 <fieldset class="ContornoPagina" id="flsRicercaEnti" runat="server">
 <legend>Ricerca Enti</legend>
                     <div class="wrapper" style="width:100%;border:0px;">
                    <div class="row" >
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="IdlblCausale" CssClass="label" AssociatedControlID="txtCodEnte" runat="server" Text="Codice Ente"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:TextBox id="txtCodEnte" CssClass="textbox" autofocus="true" runat="server"></asp:TextBox>
       </div>
       <div class="collable" style="width:15%">
           <asp:Label ID="IdlblDataChiusura" CssClass="label" AssociatedControlID="txtDenominazione"  runat="server" Text="Denominazione"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtDenominazione" CssClass="textbox"  runat="server"></asp:TextBox>
       </div>
        </div>
        <div class="RigaPulsanti">
        <asp:Button ID="ImgRicerca" runat="server" CssClass="Pulsante" OnClientClick="return Ricerca();" Text="Ricerca"  />&nbsp;
   </div>
   <asp:datagrid id="DgEnti" runat="server" Width="100%" Caption="Risultato Ricerca Enti"
		    AllowPaging="true" CellPadding="2"  PageSize="5" CssClass="table" AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
           <asp:TemplateColumn > 
                <ItemTemplate >
			        <asp:ImageButton ID="ImageButton2"  style="cursor:pointer;" CommandName="Select" AlternateText="Seleziona Ente" ToolTip='Seleziona Ente' runat="server" ImageURL="images/ente_small.png" CausesValidation="false"></asp:ImageButton>      
                </ItemTemplate>
        </asp:TemplateColumn>
			<asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"></asp:BoundColumn>
			<asp:BoundColumn DataField="codiceente" HeaderText="Cod. Ente"></asp:BoundColumn>
			<asp:BoundColumn DataField="denominazione" HeaderText="Ente"></asp:BoundColumn>
			<asp:BoundColumn DataField="classe" HeaderText="Sezione" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
		</Columns>
         <PagerStyle CssClass="linkPageGrid" HorizontalAlign="Center" 
                Mode="NumericPages" NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" />				
    </asp:datagrid>
        </div>
         <div class="RigaVuota"></div>&nbsp;
            </fieldset>

 </fieldset>
 </asp:Panel>
</asp:Content>
