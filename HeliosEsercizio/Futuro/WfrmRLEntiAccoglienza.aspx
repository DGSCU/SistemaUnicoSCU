<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRLEntiAccoglienza.aspx.vb" Inherits="Futuro.WfrmRLEntiAccoglienza" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Abilitazione all'accesso di Rappresentanti Legali di Enti di accoglienza</legend>
            <asp:Panel ID="pnlInserimento" runat="server" Visible=false Width=100%> 
                <div class="wrapper" style="width:100%">
                    <div class="headers" >
                        <h2><asp:Label ID="lblTitoloPrincipale" runat="server"  Text="Inserimento Nuova Abilitazione"></asp:Label></h2>
                    </div>
                    <div class="row" style="height:auto">
                    <asp:Label ID="lblMessaggioConferma"  CssClass="msgConferma" runat="server"></asp:Label>
                    <asp:Label ID="lblerrore" CssClass="msgErrore" runat="server"></asp:Label>
                    </div>
                    <h4 >Dati Ente di Accoglienza</h4>
                    <div class="RigaVuota" >&nbsp;</div>           
                    <div class="row" >
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="Label1" CssClass="label" AssociatedControlID="txtCodFiscaleEnte" runat="server" Text="Codice Fiscale Ente"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCodFiscaleEnte" CssClass="textbox" runat="server" 
                                MaxLength="11"></asp:TextBox>         
                        </div>
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="Label2" CssClass="label" AssociatedControlID="chkEstero" runat="server" Text="Sede Estera"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                           <asp:CheckBox id="chkEstero" runat="server" AutoPostBack="True" ToolTip="Sede Estera"></asp:CheckBox>       
                        </div>
                    </div>
                     <h4>Dati Rappresentante Legale Ente di Accoglienza</h4>
                     <div class="RigaVuota" >&nbsp;</div>
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
                            <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="txtCodiceFiscale" runat="server" Text="Codice Fiscale"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCodiceFiscale" CssClass="textbox" runat="server" MaxLength="16"></asp:TextBox>         
                        </div>
                        <div class="collable" style="width:35%">      
                           &nbsp;
                        </div>
                       </div>
                     <div class ="row" >
                     <asp:TextBox ID="TxtHdTipoOperazione" CssClass="textbox" runat="server" visible=false ReadOnly> </asp:TextBox>
                     <asp:TextBox ID="TxtHdIdRlAccoglienza" CssClass="textbox" runat="server" visible=false ReadOnly> </asp:TextBox>
                     <asp:TextBox ID="TxtHdIdEnte" CssClass="textbox" runat="server" visible=false ReadOnly> </asp:TextBox>
                   
                     </div>
                   </div>
                 </asp:Panel>
       
            <div class="RigaPulsanti">
                <asp:Button ID="cmdNuovo" runat="server" CssClass="Pulsante" Text="Aggiungi Rappresentante Legale" />&nbsp;
                <asp:Button ID="CmdTorna" runat="server" CssClass="Pulsante" Text="Annulla" />&nbsp;
                <asp:Button ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva" Visible="False" />&nbsp
                <asp:Button ID="CmdElimina" runat="server" CssClass="Pulsante" Text="Elimina" Visible="False" />&nbsp
                <asp:Button ID="CmdAnnulla" runat="server" CssClass="Pulsante" Text="Annulla" Visible="False" />&nbsp
            </div>
         <br />
        <h3>
            <asp:label id="lblPersonale" runat="server" Text="" AssociatedControlID="dtgRisultatoRicerca"></asp:label>
        </h3>
        <div class ="row" id="DivGriglia" runat="server">
        <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  Caption="Elenco Abilitazioni Rappresentanti Legali" ToolTip="Elenco Abilitazioni Rappresentanti Legali" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn HeaderText="Selez." ItemStyle-Width="10%"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgDettaglioEnte" CommandName="Select" ToolTip="Seleziona Ente" AlternateText="Seleziona Ente" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IDRLEntiAccoglienza" HeaderText="IDRLEntiAccoglienza"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="SedeLegaleEstera" HeaderText="SedeLegaleEstera"></asp:BoundColumn>
                <asp:BoundColumn DataField="CodiceFiscaleEnte" HeaderText="Codice Fiscale Ente di Accoglienza"  ItemStyle-Width="15%"></asp:BoundColumn>
                <asp:BoundColumn DataField="RLCodiceFiscale" HeaderText="Codice Fiscale RL"  ItemStyle-Width="15%"></asp:BoundColumn>
				<asp:BoundColumn DataField="RLCognome" HeaderText="Cognome"  ItemStyle-Width="15%"></asp:BoundColumn>
				<asp:BoundColumn DataField="RLNome" HeaderText="Nome"  ItemStyle-Width="15%">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
                <asp:TemplateColumn ItemStyle-Width="15%">
					<HeaderTemplate>
						<strong>Sede Estera</strong>
					</HeaderTemplate>
					<ItemTemplate>
						&nbsp;&nbsp;
						<asp:CheckBox id="chkEsteraGrid" runat="server" AutoPostBack="True" ToolTip="Sede Legale Estera" readonly="True"></asp:CheckBox>
					</ItemTemplate>
				</asp:TemplateColumn>
                <asp:BoundColumn DataField="RLRegistrato" HeaderText="RL Registrato" ItemStyle-Width="15%"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        </div>
        <br />
        
      </fieldset>

</asp:Content>
