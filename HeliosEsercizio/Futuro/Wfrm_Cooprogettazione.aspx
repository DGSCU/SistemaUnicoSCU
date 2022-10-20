<%@ Page Title="Gestione Enti Coprogettanti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Wfrm_Cooprogettazione.aspx.vb" Inherits="Futuro.Wfrm_Cooprogettazione" %>
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
<asp:Panel ID="Panel1" DefaultButton="ImgRicerca" runat="server">
<fieldset class="ContornoPagina">
    <div class="wrapper" style="width:100%;border:0px">
      <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestione Enti Coprogettanti"></asp:Label></h2>
            </div>
         <div class="rowGroup" style="height:auto">
            <div class="RigaVuota"> &nbsp;</div>
		    <asp:label id="LblErrore"  runat="server" CssClass="msgErrore"></asp:label>
  
             <div class="RigaVuota">  &nbsp; </div>
		</div>
        <div class="row" >
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="labelProgetto" AssociatedControlID="lblProgetto"  runat="server" Text="Progetto"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="lblProgetto" CssClass="labelDati"    runat="server"></asp:Label>
       </div>
             <div class="collable" style="width:15%"  >
           <asp:Label ID="labelCompetenza" CssClass="label" AssociatedControlID="lblCompetenza"  runat="server" Text="Competenza"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="lblCompetenza" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" >
        <div class="collable" style="width:15%">
           <asp:Label ID="labelSettore" CssClass="label" AssociatedControlID="lblSettore" runat="server" Text="Settore"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="lblSettore" CssClass="labelDati" runat="server"></asp:Label>
       </div>
          <div class="collable" style="width:15%">
           <asp:Label ID="labelArea" CssClass="label" AssociatedControlID="lblArea" runat="server" Text="Area"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="lblArea" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>
           <div class="RigaVuota">&nbsp;</div>

    <asp:datagrid id="DgEntiAccorpati" runat="server" Width="100%" Caption="Enti Coprogettanti"
		    AllowPaging="true" CellPadding="2"  PageSize="5" CssClass="table" AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
			<asp:BoundColumn Visible="False" DataField="IdAttivita" HeaderText="IdAttivita"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
			<asp:BoundColumn DataField="codiceente" HeaderText="Cod. Ente"></asp:BoundColumn>
			<asp:BoundColumn DataField="denominazione" HeaderText="Ente"></asp:BoundColumn>
			<asp:BoundColumn DataField="sedi" HeaderText="N&#176; Sedi su progetto">
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="volontari" HeaderText="N&#176; volontari richiesti">
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>	
            <asp:TemplateColumn HeaderText="Rimuovi"> 
                <ItemTemplate >
			        <asp:ImageButton ID="ImageButton2"  style="cursor:pointer;" CommandName="elimina" AlternateText="Elimina Ente" ToolTip='Elimina Ente' runat="server" ImageURL="images/canc_small.png" CausesValidation="false" Visible='<%# IIF(Eval("codiceente").ToString().Length = 7, True, False) %>'></asp:ImageButton>      
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
               
    	<% If Request.QueryString("popup") <> "1" And Request.QueryString("Modifica") = "0" Then%>
					<%else%>
                       <fieldset class="ContornoPagina">
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
<%end if%>
 </fieldset>
 </asp:Panel>

</asp:Content>
