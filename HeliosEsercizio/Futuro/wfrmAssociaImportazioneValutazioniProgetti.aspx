<%@ Page Title="Definizione Modello Importazione Valutazioni Progetti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmAssociaImportazioneValutazioniProgetti.aspx.vb" Inherits="Futuro.wfrmAssociaImportazioneValutazioniProgetti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

    //<![CDATA[

        function isAlphaKey(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8);
        }

    //]]>

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Definizione Modello Importazione Valutazioni Progetti</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore"></asp:label>
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Definizione Modello Importazione Valutazioni Progetti"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTipoProgetto" CssClass="label" AssociatedControlID="ddlTipoProgetto" runat="server" Text="Tipo Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlTipoProgetto" runat="server" CssClass="ddlClass" AutoPostBack="true">
                        <asp:ListItem Value="2">Selezionare</asp:ListItem>
						<asp:ListItem Value="1">NAZIONALE</asp:ListItem>
						<asp:ListItem Value="0">ESTERO</asp:ListItem>
                    </asp:DropDownList>
               </div>
            </div>
        </div>
        <br />
        <asp:datagrid id="dtgAssociazioneImportazioneProgetti" runat="server" Width="100%" ToolTip="Associazione Importazione Progetti" CssClass="table" CellPadding="2" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	        <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="CodiceParametro" HeaderText="Codice Parametro"></asp:BoundColumn>
				<asp:BoundColumn DataField="Parametro" HeaderText="Parametro"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdCompetenza" HeaderText="IdCompetenza"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Nazionale" HeaderText="Nazionale"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Colonna">
					<ItemTemplate>
						<asp:TextBox onkeypress="return isAlphaKey(event)" id="txtColonna" MaxLength="2" Width="50px" runat="server" ToolTip="Colonna"></asp:TextBox>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="Colonna" HeaderText="ValoreColonna"></asp:BoundColumn>
			</Columns>
		</asp:datagrid>
        <div class="wrapper" style="width:100%; border-style:none">
            <div class="RigaVuota">&nbsp;</div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva"  />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
            </div>
        </div>
    </fieldset>
</asp:Content>
