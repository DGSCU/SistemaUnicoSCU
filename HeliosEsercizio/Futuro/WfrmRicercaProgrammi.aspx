<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaProgrammi.aspx.vb" Inherits="Futuro.WfrmRicercaProgrammi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="firefox">
    <fieldset class="ContornoPaginaScroll">
    <legend >Ricerca Programmi</legend>
    <div class="wrapper" style="width:100%">
        <div class="headers"> 
           <h2>
          <asp:Label ID="Label5" runat="server"  Text="RICERCA PROGRAMMI"></asp:Label>
          </h2>
     </div>
        <div class="RigaVuota">
          &nbsp;
         </div>
        <div class="row" style="height:auto">
        <asp:label id="lblmessaggio" CssClass="msgErrore"  runat="server" ></asp:label>
        </div>
        <div class="RigaVuota">
          &nbsp;
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
          
            <asp:Label ID="lblTitoloprogramma" AssociatedControlID="txtTitoloProgramma"    runat="server"
                Text="Titolo Programma"></asp:Label>
        
            </div>
            <div class="colOggetti" style="width:35%" >
   
          <asp:TextBox ID="txtTitoloProgramma" 
               runat="server" autofocus="true"></asp:TextBox>
       
            </div>
            <div class="collable" style="width:15%" >
                <asp:Label ID="lblCodiceProgramma"  AssociatedControlID="TxtCodPog" runat="server"
                Text="Codice Programma"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%" >
                 <asp:TextBox ID="TxtCodPog" 
               runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="row" >
           <div class="collable" style="width:15%">
        <asp:Label ID="lblDenominazioneEnteProponente" AssociatedControlID="txtEnteProponente"    runat="server"
                Text="Ente Proponente"></asp:Label>
           </div>
           <div class="colOggetti" style="width:35%">
           <asp:TextBox ID="txtEnteProponente" 
               runat="server" autofocus="true"></asp:TextBox>
           </div>
           <div class="collable" style="width:15%">
          <asp:Label ID="lblCodiceEnteProponente" AssociatedControlID="txtCodiceEnteProponente"    runat="server"
                Text="Codice Ente Proponente"></asp:Label>
                         
           </div>
           <div class="colOggetti" style="width:35%">
           <asp:TextBox ID="txtCodiceEnteProponente" 
               runat="server" autofocus="true"></asp:TextBox>
           </div>
         
        </div>
        <div class="row" >
           <div class="collable" style="width:15%">
             <asp:Label ID="lblstaoProgramma" AssociatedControlID="ddlCoprogrammazione"    runat="server"
                Text="Stato Programma"></asp:Label>
           
          
           </div>
           <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="ddlStatoProgramma"   runat="server"></asp:DropDownList>
           
           </div>
           <div class="collable" style="width:15%">
        
                <asp:Label ID="lblCoprogrammazione" AssociatedControlID="ddlCoprogrammazione"    runat="server"
                Text="CoProgrammazione"></asp:Label>
         
           </div>
           <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="ddlCoprogrammazione"   runat="server">
                   	    <asp:ListItem Selected="True"></asp:ListItem>
						<asp:ListItem Value="0">No</asp:ListItem>
						<asp:ListItem Value="1">Si</asp:ListItem>
                 </asp:DropDownList>
           </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
              <asp:Label ID="lblDurataMesi" AssociatedControlID="ddlDurataMesi"    runat="server"
                Text="Durata Mesi"></asp:Label>
           </div>

            <div class="colOggetti" style="width:35%">
                 <asp:DropDownList ID="ddlDurataMesi"  runat="server" > </asp:DropDownList>
           </div>
            <div class="collable" style="width:15%">
          <asp:Label ID="lblObiettivo" AssociatedControlID="ddlObiettivo"    runat="server"
                Text="Obiettivo"></asp:Label>
         
           </div>
            <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="ddlObiettivo"  runat="server" > </asp:DropDownList>
           </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="lblAmbito"  AssociatedControlID="ddlAmbito" runat="server" Text="Ambito"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlAmbito"  runat="server"></asp:DropDownList>
           </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="lblTerritorio"  AssociatedControlID="ddlTerritorio" runat="server" Text="Territorio"></asp:Label>
           </div>
           <div class="colOggetti" style="width:35%">
           <asp:DropDownList ID="ddlTerritorio"  runat="server"></asp:DropDownList>
           </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
             <asp:Label ID="lblMacroAmbito"  AssociatedControlID="ddlMacroAmbito" runat="server" Text="Settore"></asp:Label>
           </div>
           <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="ddlMacroAmbito"  runat="server"></asp:DropDownList>
           </div>
            <div class="collable" style="width:15%">
             <asp:Label ID="lblReti"  AssociatedControlID="ddlReti" runat="server" Text="Reti"></asp:Label>
                </div>
          <div class="colOggetti" style="width:35%">
           <asp:DropDownList ID="ddlReti"  runat="server">
            <asp:ListItem Selected="True"></asp:ListItem>
						<asp:ListItem Value="0">No</asp:ListItem>
						<asp:ListItem Value="1">Si</asp:ListItem>
           </asp:DropDownList>
          </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
             <asp:Label ID="lblTipoProgramma"  AssociatedControlID="ddlTipoProgramma" runat="server" Text="Tipo Programma"></asp:Label>
           </div>
           <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="ddlTipoProgramma"  runat="server"></asp:DropDownList>
           </div>
            <div class="collable" style="width:15%">
             <asp:Label ID="lblCircolare" AssociatedControlID="DdlBando" runat="server" Text="Avviso"></asp:Label>
             </div>
             <div class="colOggetti" style="width:35%">
              <asp:dropdownlist id="DdlBando" runat="server"  AutoPostBack="False"></asp:dropdownlist>
              </div>
        </div>

        <%--MEV 224--%>
        <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label runat="server" ID="lblRegioni" AssociatedControlID="ddlRegioni" Text="Regione di attuazione del programma" />
            </div>
            <div class="colOggetti" style="width:35%">           
                <asp:DropDownList runat="server" ID="ddlRegioni" />
            </div>       
        </div>

        <div class="RigaPulsanti">
            <asp:Button ID="CmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
            <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />&nbsp;
            <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" ToolTip="Esporta elenco risultato ricerca" />
            <br />
            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>            
        </div>
        <div class="RigaVuotaPrint" >

                <br />
          &nbsp;
        </div>
    </div>
       <div class="row">
       <asp:Label CssClass="msgInfo" ID="lblInfoRicerca" runat="server"></asp:Label>
          </div>
       <asp:datagrid id="dgRisultatoRicerca" runat="server"  
            Caption="Risultato Ricerca Programmi" CaptionAlign="Top"  
            AllowPaging="True" CellPadding="1" Width="100%" CssClass="table"
            AutoGenerateColumns="False" UseAccessibleHeader="True">
        <FooterStyle></FooterStyle>
        <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
        <EditItemStyle></EditItemStyle>
        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
        <HeaderStyle></HeaderStyle>
        <Columns>
			<asp:TemplateColumn HeaderText="Selez.">
		    <ItemTemplate>           
                <asp:ImageButton ID="btnSeleziona" AlternateText="Selezione Programma"   runat="server" CommandName="Select"
                                ImageUrl="~/Images/Icona_Progetto_small.png"  ToolTip="Selezione Programma" />
            </ItemTemplate>         
            </asp:TemplateColumn>
             <asp:TemplateColumn Visible="False" HeaderText="Accet.">
		    <ItemTemplate>           
                <asp:ImageButton ID="btnaccettazione" AlternateText="Accettazione Programma"  runat="server" CommandName="accettazione"
                                ImageUrl="~/Images/valida_small.png"  ToolTip="Accettazione Programma" />
            </ItemTemplate>         
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="False" HeaderText="Valutaz.">
		    <ItemTemplate>           
                <asp:ImageButton ID="btnvalutazione" runat="server"   AlternateText="Valutazione Programma" CommandName="valutazione"
                                ImageUrl="~/Images/vincoli_small.png" ToolTip="Valutazione Programma" />
            </ItemTemplate>         
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="IdProgramma" Visible="false" HeaderText="IdProgramma"></asp:BoundColumn>
			<asp:BoundColumn DataField="CodiceEnteProponente" HeaderText="Codice Ente Proponente"></asp:BoundColumn>
			<asp:BoundColumn DataField="DenominazioneEnteProponente" HeaderText="Denominazione Ente Proponente"></asp:BoundColumn>
			<asp:BoundColumn DataField="CodiceProgramma" HeaderText="Codice Programma"></asp:BoundColumn>
			<asp:BoundColumn DataField="TitoloProgramma" HeaderText="Titolo Programma"></asp:BoundColumn>
			<asp:BoundColumn DataField="StatoProgramma" HeaderText="Stato Programma"></asp:BoundColumn>
		    <asp:BoundColumn DataField="IdEnteProponente" Visible="false" HeaderText="IdEnteProponente"></asp:BoundColumn>
            <asp:BoundColumn DataField="TipoProgramma" HeaderText="Tipo Programma">
            </asp:BoundColumn>
        </Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
			PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>

</fieldset>
</div>
</asp:Content>
