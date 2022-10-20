<%@ Page Title="Ricerca Progetti per Caricamento Presenze Formatori" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ricercaprogettiformatori.aspx.vb" Inherits="Futuro.ricercaprogettiformatori" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="ContornoPagina">
        <legend>Ricerca Progetti per Caricamento Presenze Formatori</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Progetti per Caricamento Presenze Formatori"></asp:Label></h2>
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
                    <asp:Label ID="lblCodProgetto" CssClass="label" AssociatedControlID="txtCodProgetto" runat="server" Text="Cod. Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>

                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                       <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>

             <div  class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblDoc" CssClass="label" AssociatedControlID="ddlPrefissiDocumenti" runat="server" Text="Progetti senza documenti caricati:"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlPrefissiDocumenti" runat="server">
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
        <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgProgetti"></asp:label>
    </h3>
    <br />
    <asp:datagrid id="dgProgetti" runat="server" Width="100%" 
            ToolTip="Elenco Progetti" CssClass="table"  AllowPaging="True" 
            AllowSorting="True" AutoGenerateColumns="False"  Visible="False" 
            UseAccessibleHeader="True">
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
			<asp:BoundColumn Visible="False" DataField="IdAttività" HeaderText="IdAttività"></asp:BoundColumn>
			<asp:TemplateColumn>
                <ItemTemplate>
					<asp:ImageButton ID="imgDocProg" CommandName="Consulta" ToolTip="Consulta Documenti Presenze Formatori" AlternateText="Consulta Documenti Presenze Formatori" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>            
            </asp:TemplateColumn>
			<asp:BoundColumn DataField="codiceProgetto" HeaderText="Codice Progetto"></asp:BoundColumn>
			<asp:BoundColumn DataField="TitoloProgetto" HeaderText="Titolo Progetto">
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="totaledocumentipresenti" 
                HeaderText="Totale Documenti"></asp:BoundColumn>
		    <asp:BoundColumn DataField="idente" HeaderText="idente" Visible="False">
            </asp:BoundColumn>
            <asp:BoundColumn DataField="ente" HeaderText="Denominazione" Visible="False">
            </asp:BoundColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    <p>&nbsp;</p>
    </fieldset>
</asp:Content>
