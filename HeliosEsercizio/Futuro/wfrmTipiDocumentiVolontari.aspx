<%@ Page Title="Classificazione Documenti Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmTipiDocumentiVolontari.aspx.vb" Inherits="Futuro.wfrmTipiDocumentiVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Classificazione Ddocumenti Volontari</legend>
        <asp:label id="lblmsg"  runat="server" CssClass="msgErrore"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
             <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Classificazione Documenti Volontari"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:label id="LblTipoDocumeto" runat="server" CssClass="label" AssociatedControlID="txtTipoDocumento" Text="Tipo Documento"></asp:label>
                </div>
                <div class="colOggetti" style="width:85%">
                    <asp:textbox id="txtTipoDocumento" runat="server" autofocus="true" CssClass="textbox" Width="96%" MaxLength="50"></asp:textbox>
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:label id="LblNote" runat="server" CssClass="label" AssociatedControlID="txtNote" Text="Note"></asp:label>
                </div>
                <div class="colOggetti" style="width:85%">
                    <asp:textbox id="txtNote" runat="server" TextMode="MultiLine" CssClass="textbox" Width="96%" MaxLength="250"></asp:textbox>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdInserisci" runat="server" CssClass="Pulsante" Text="Inserisci" />
            </div>
        </div>
        <br />
        <asp:datagrid id="dtgTipoDocumento" runat="server" CssClass="table"  Width="100%" ToolTip="Elenco esiti requisiti"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" AllowPaging="True" PageSize="10" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:ButtonColumn Text="" Visible="false"></asp:ButtonColumn>
				<asp:BoundColumn Visible="False" DataField="IdTipoDocumentoVolontario" HeaderText="IdTipoDocumentoVolontario"></asp:BoundColumn>
				<asp:BoundColumn DataField="TipoDocumento" HeaderText="Tipo Documento">
					<HeaderStyle Width="40%"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Note" HeaderText="Note">
					<HeaderStyle Width="40%"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Abilitato" HeaderText="Abilitato">
					<HeaderStyle Width="1px"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="NumeroDocumentiAssociati" HeaderText="Numero Documenti Associati">
					<HeaderStyle Width="1px"></HeaderStyle>
				</asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Abilita"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgbtnAbilita" CommandName="Abilita" ToolTip="Abilita" AlternateText="Abilita" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Disabilita"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgbtnDisabilita" CommandName="Disabilita" ToolTip="Disabilita" AlternateText="Disabilita" runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        </div>
    </fieldset>
</asp:Content>
