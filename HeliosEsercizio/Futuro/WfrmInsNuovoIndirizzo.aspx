<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmInsNuovoIndirizzo.aspx.vb" Inherits="Futuro.WfrmInsNuovoIndirizzo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>Inserimento Indirizzi</legend>
        
<div class="wrapper" style="width:100%">
<div class="headers">
               <h2>
               <asp:Label ID="Label1" runat="server" Text="Inserimento Nuovo Indirizzo"></asp:Label>
               </h2>
            </div>
                                <div class="RigaVuota" >
                                &nbsp;
                                </div>
				<div class="row">
					
						<asp:label id="lblmess" runat="server" CssClass="msgErrore"></asp:label>
                     
				</div>

				<div class="row">
					<div class="collable">
                    <asp:Label ID="Label2" AssociatedControlID="txtComune" runat="server" Text="Comune"></asp:Label>
                    
					</div>
					<div class="colOggetti" style="width:70%">
                    <asp:textbox id="txtComune" runat="server"></asp:textbox>
                    </div>
				</div>
				<div class="row">
					<div class="collable">
                    <asp:Label ID="Label3" AssociatedControlID="txtIndirizzo" runat="server" Text="Indirizzo"></asp:Label>
                    
					</div>
					<div class="colOggetti" style="width:70%">
                    <asp:textbox id="txtIndirizzo" runat="server"></asp:textbox>
                    </div>
					
				</div>
				<div class="row">
					<div class="collable">
               <asp:Label ID="Label4" AssociatedControlID="txtCap" runat="server" Text="Cap"></asp:Label>
                   
                    </div>
					<div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtCap" runat="server" MaxLength="5"></asp:textbox>
                    </div>
				</div>
				<div class="row" style="width:100%">
					
                    <asp:datagrid id="dtgTrovaCap" CssClass="table" runat="server" Width="100%"
							 AllowSorting="True" ToolTip="Risultato Ricerca Indirizzo" HorizontalAlign="Center"
							 AutoGenerateColumns="False" Visible="False" PageSize="5" UseAccessibleHeader="true">
							<FooterStyle></FooterStyle>
							<SelectedItemStyle BackColor="White"></SelectedItemStyle>
							<EditItemStyle></EditItemStyle>
							<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
							<ItemStyle CssClass="tr"></ItemStyle>
							<HeaderStyle></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Provincia" HeaderText="Provincia"></asp:BoundColumn>
								<asp:BoundColumn DataField="Comune" HeaderText="Comune"></asp:BoundColumn>
								<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo"></asp:BoundColumn>
								<asp:BoundColumn DataField="Civici" HeaderText="N&#176;Civici"></asp:BoundColumn>
								<asp:BoundColumn DataField="Cap" HeaderText="Cap"></asp:BoundColumn>
								<asp:ButtonColumn   Text="&lt;img src=images/canc_small.png '  title='Rimuovi' alt='Rimuovi' border=0&gt;"
									HeaderText="Rimuovi"  CommandName="Rimuovi">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:ButtonColumn>
							</Columns>
						</asp:datagrid>
                    </div>
				
				
			</div>
			    <div class="RigaPulsanti" style="text-align:right">
                    <asp:button  id="cmdRicerca" runat="server" BorderStyle="Outset" ToolTip="Ricerca" Text="Ricerca" CssClass="Pulsante"></asp:button>
                    <asp:button  id="ImgInserisci" runat="server" Text="inserisci" ToolTip="Inserimento Nuovo Indirizzo" CssClass="Pulsante"></asp:button>
                   <asp:button  id="CmdChiudi" runat="server" ToolTip="Chiudi" Text="Chiudi" CssClass="Pulsante"></asp:button>
                     </div>
			
            </fieldset>
</asp:Content>
