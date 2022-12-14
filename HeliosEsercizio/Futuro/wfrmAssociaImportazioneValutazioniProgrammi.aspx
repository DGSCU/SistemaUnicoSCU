<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmAssociaImportazioneValutazioniProgrammi.aspx.vb" Inherits="Futuro.wfrmAssociaImportazioneValutazioniProgrammi" %>
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
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Definizione Modello Importazione Valutazioni Programmi"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
      
        <asp:datagrid id="dtgAssociazioneImportazioneProgrammi" runat="server" Width="100%" ToolTip="Associazione Importazione Programmi" CssClass="table" CellPadding="2" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
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
       
      
        
    </div>
    <div class="RigaPulsanti" style="text-align:right">
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Salva"  />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
            </div>
    </fieldset>
</asp:Content>
