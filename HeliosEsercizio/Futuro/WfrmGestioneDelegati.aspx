<%@ Page Title="Gestione Delegati del Rappresentante Legale" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneDelegati.aspx.vb" Inherits="Futuro.WfrmGestioneDelegati" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Gestione Delegati del Rappresentante Legale</legend>
            <asp:Panel ID="pnlInserimento" runat="server" Visible=false Width=100%> 
                <div class="wrapper" style="width:100%">
                    <div class="headers" >
                        <h2><asp:Label ID="lblTitoloPrincipale" runat="server"  Text="Inserimento Nuovo Delegato del Rappresentante Legale"></asp:Label></h2>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row" style="height:auto">
                    <asp:Label ID="lblMessaggioConferma"  CssClass="msgConferma" runat="server"></asp:Label>
                    <asp:Label ID="lblerrore" CssClass="msgErrore" runat="server"></asp:Label>
                    </div>
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
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlStato" runat="server" Text="Stato"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:DropDownList ID="ddlStato" runat="server" CssClass="ddlClass">
                                <asp:ListItem Value=""> Selezionare... </asp:ListItem>
                                <asp:ListItem Value="0"> Disabilitato </asp:ListItem>
                                <asp:ListItem Value="1"> Abilitato </asp:ListItem>
                            </asp:DropDownList> 
                            <asp:TextBox ID="TxtStato" CssClass="textbox" runat="server" visible=false ReadOnly> </asp:TextBox>
                        </div>
                     </div>
                     <div class="row" >
                     <asp:TextBox ID="TxtHdTipoOperazione" CssClass="textbox" runat="server" visible=false ReadOnly> </asp:TextBox>
                     <asp:TextBox ID="TxtHdIdDelegato" CssClass="textbox" runat="server" visible=false ReadOnly> </asp:TextBox>
                     <asp:TextBox ID="TxtHdIdEnte" CssClass="textbox" runat="server" visible=false ReadOnly> </asp:TextBox>
                   
                     </div>
                </div>
                 </asp:Panel>
       
            <div class="RigaPulsanti">
                <asp:Button ID="cmdNuovo" runat="server" CssClass="Pulsante" Text="Nuovo Delegato" />&nbsp;
                <asp:Button ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva" Visible="False" />&nbsp
                <asp:Button ID="CmdElimina" runat="server" CssClass="Pulsante" Text="Elimina" Visible="False" />&nbsp
                <asp:Button ID="CmdAnnulla" runat="server" CssClass="Pulsante" Text="Annulla" Visible="False" />&nbsp
            </div>
         <br />
        <h3>
            <asp:label id="lblPersonale" runat="server" Text="" AssociatedControlID="dtgRisultatoRicerca"></asp:label>
        </h3>
        <div class="row" id="DivGriglia" runat="server">

        <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  Caption="DELEGATI RAPPRESENTANTE LEGALE" ToolTip="Elenco Delegati Rappresentante Legale" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn HeaderText="Seleziona" ItemStyle-Width="10%"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgVolontario" CommandName="Select" ToolTip="Seleziona Nominativo" AlternateText="Seleziona Nominativo" runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IDDelegato" HeaderText="IDDelegato"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IDEnte" HeaderText="IDEnte"></asp:BoundColumn>
				<asp:BoundColumn DataField="Cognome" HeaderText="Cognome" ItemStyle-Width="18%"></asp:BoundColumn>
				<asp:BoundColumn DataField="Nome" HeaderText="Nome" ItemStyle-Width="18%">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Codice Fiscale" ItemStyle-Width="18%">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato Iscrizione" ItemStyle-Width="18%">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="UltimoAccesso" HeaderText="Ultimo Accesso" ItemStyle-Width="18%">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
       </div>
        <br />
      </fieldset>
</asp:Content>
