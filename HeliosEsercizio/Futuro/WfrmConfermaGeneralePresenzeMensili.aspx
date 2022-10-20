<%@ Page Title="Conferma Presenze Mensili" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmConfermaGeneralePresenzeMensili.aspx.vb" Inherits="Futuro.WfrmConfermaGeneralePresenzeMensili" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <fieldset class="ContornoPagina">
        <legend>Conferma Presenze Mensili</legend>
        <asp:label id="lblNoVol"  runat="server" CssClass="msgErroreGrid" 
             Visible="False" 
             Text="***ATTENZIONE - Impossibile procedere nessun volontario in servizio***"></asp:label>
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Conferma Presenze Mensili"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:10%">
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="lblValEnte" runat="server" Text="Ente"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:15%">       
                    <asp:label id="lblValEnte" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblProg" CssClass="label" AssociatedControlID="lblValProg" runat="server" Text="Denominazione"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:55%">       
                    <asp:label id="lblValProg" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
        </div>
        <p style="text-align:center;">            <asp:Button ID="CmdConfermaPresenze" 
                runat="server" CssClass="Pulsante" Text="Conferma Generale Mensile" 
                Visible="False" />
         </p>    
        <asp:datagrid id="DtgMesiDaInserire" runat="server" CssClass="table" Width="100%" ToolTip="Elenco Presenze Mensili" CellPadding="2" AllowSorting="True" AutoGenerateColumns="false" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="mese" HeaderText="Mese" Visible="false">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="10%"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:BoundColumn DataField="mesedescr" HeaderText="Mese">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="10%"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="anno" HeaderText="Anno">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60%"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:BoundColumn DataField="DataConferma" HeaderText="Data Ora Conferma">
					<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60%"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Dettaglio"> 
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgSelect" CommandName="Select" ToolTip="Seleziona Mese" AlternateText="Seleziona Mese" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
			</Columns>
		</asp:datagrid>         
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        </div>
     </fieldset>
</asp:Content>
