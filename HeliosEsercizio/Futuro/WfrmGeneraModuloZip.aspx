<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGeneraModuloZip.aspx.vb" Inherits="Futuro.WfrmGeneraModuloZip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>Genera Moduli Precompilati</legend>
         <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Volontari Per Generazione Moduli Precompilati"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" id="divTipoUtente" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="txtEnte" runat="server" Text="Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtEnte" CssClass="textbox" autofocus="true" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtCodEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
            <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodVolontario" CssClass="label" AssociatedControlID="TxtCodVolontario" runat="server" Text="Cod. Volontario"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtCodVolontario" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
       

                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodProgetto" CssClass="label" AssociatedControlID="txtCodProgetto" runat="server" Text="Cod. Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                   <asp:Label ID="LblCodiceSedeAttuazione" CssClass="label" AssociatedControlID="TxtCodiceSedeAttuazione" runat="server" Text="Cod. Sede"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtCodiceSedeAttuazione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
             </div>
             <div class="row" >
             <div class="collable" style="width:15%">  
                    <asp:Label ID="lblEsecuzionePagamento" CssClass="label" AssociatedControlID="CboAnnoMese" runat="server" Text="Anno/Mese"></asp:Label>    
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="CboAnnoMese" runat="server" />
                </div>
                 <div class="collable" style="width:15%">  
                 <asp:Label ID="lblStatoConfermaPreliminare" CssClass="label" AssociatedControlID="CboStatoConfermaPreliminare" runat="server" Text="Stato Conferma Preliminare"></asp:Label> 
                 </div>
                 <div class="colOggetti" style="width:35%">
                  <asp:DropDownList ID="CboStatoConfermaPreliminare" CssClass="ddlClass"  runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="DA CONFERMARE">DA CONFERMARE</asp:ListItem>
                        <asp:ListItem Value="CONFERMATA">CONFERMATA</asp:ListItem>
                    </asp:DropDownList>
                 </div>

              </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
         </div>

          <br />
    <h3>
        <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgVolontari"></asp:label>
    </h3>
    <br />


  <div>
     <div style="float:right">
        <asp:CheckBox id="chkSelectAll" Runat="server" Text="Seleziona tutti" Visible = "False" AutoPostBack="true"   ></asp:CheckBox>   
    </div>
   
    <asp:datagrid id="dgVolontari" runat="server" Width="100%" 
            ToolTip="Elenco Volontari" CssClass="table" 
            AllowSorting="True" AutoGenerateColumns="False"  Visible="False" 
            UseAccessibleHeader="True">
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		<HeaderStyle></HeaderStyle>
       
		<Columns>
			<asp:BoundColumn Visible="False" DataField="IdEntit&#224;" HeaderText="IdEntit&#224;"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IdAttivit&#224;" HeaderText="IdAttivit&#224;"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Selez."> 
                <ItemTemplate>
					<asp:ImageButton ID="ImageButton1" CommandName="Select" ToolTip="Seleziona Volontario" AlternateText="Seleziona Volontario" runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			<asp:BoundColumn DataField="codicevolontario" HeaderText="Codice Volontario"></asp:BoundColumn>
			<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Cod. Fiscale"></asp:BoundColumn>
			<asp:BoundColumn DataField="datanascita" HeaderText="Data Nascita" DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="ComuneNascita" HeaderText="Comune Nascita">
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Progetto" HeaderText="Progetto"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="SedeAttuazione" HeaderText="Sede Attuazione"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="Ente" HeaderText="Ente"></asp:BoundColumn>
			<asp:BoundColumn DataField="IdEnteSedeAttuazione" HeaderText="Codice Sede"></asp:BoundColumn>
             <asp:BoundColumn DataField="ConfermaPreliminare" 
                HeaderText="Stato Conferma Preliminare"></asp:BoundColumn>
             <asp:TemplateColumn HeaderText="Sel." >
				    <HeaderStyle Width="3%"></HeaderStyle>
					<ItemTemplate>
						<asp:CheckBox id="chkSelProg" toolTip="Seleziona" AutoPostBack="False" runat="server" Text="&nbsp;"></asp:CheckBox>
					</ItemTemplate>
			    </asp:TemplateColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    <p>&nbsp;</p>
     </div>
      
        <div class="RigaPulsanti" style="float:right">
             <asp:Button ID="CmdGenaraModuloZip" CssClass="Pulsante" Text="Genera Moduli Precompilati" Visible="false" runat="server"/>&nbsp;
       </div>    
    </fieldset>
      
</asp:Content>
