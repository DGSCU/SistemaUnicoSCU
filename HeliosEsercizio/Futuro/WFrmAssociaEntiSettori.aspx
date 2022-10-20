<%@ Page Title="Gestione Settori" MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WFrmAssociaEntiSettori.aspx.vb" Inherits="Futuro.WFrmAssociaEntiSettori" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>Settori</legend>
        <div class="wrapper" style="width:100%;border:0px">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestione Settori"></asp:Label>
               </h2>
            </div>
            <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
                 <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
        </div>
        <br />
        <div class="row" style="height:auto">
        <div class="collable" style="width:48%">
            <asp:datagrid id="dtgAmbiti" runat="server" CssClass="table" Width="100%" Caption="Settori di intervento"  
                CellPadding="5"  AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" UseAccessibleHeader="True"        >
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns>
                        <asp:TemplateColumn HeaderText="Sel.">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="images/selezionato_small.png" ID="SelezioneSettore" runat="server" CommandName="Select" AlternateText="Seleziona Settore" ToolTip="Aggiungi Settore" />
                            </ItemTemplate>                 
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="IDMacroAmbitoAttivit&#224;" HeaderText="Settori di Intervento"></asp:BoundColumn>
					    <asp:BoundColumn DataField="MacroAmbitoAttivit&#224;" HeaderText="Settori di Intervento"></asp:BoundColumn>
				    </Columns>
					<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;"  HorizontalAlign="Right" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
        </div>
        <div class="collable" style="width:4%">&nbsp;
        </div>
        <div class="collable" style="width:48%">
        <asp:datagrid id="dtgAmbitiSelezionati" runat="server" CssClass="table" Width="100%" Caption="Settori di Intervento Associati"  
                CellPadding="5"  AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" UseAccessibleHeader="True"        >
					<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle></HeaderStyle>
					<Columns>
                       <asp:TemplateColumn HeaderText="Sel.">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="images/canc_small.png" ID="DeselezioneSettore" runat="server" CommandName="Select" ToolTip="Elimina Settore" AlternateText="Cancella Settore" />
                            </ItemTemplate>                 
                        </asp:TemplateColumn>
                        <asp:BoundColumn Visible="False" DataField="IDMacroAmbitoAttivit&#224;" HeaderText="Settori di Intervento Associati"></asp:BoundColumn>
					    <asp:BoundColumn DataField="MacroAmbitoAttivit&#224;" HeaderText="Settori di Intervento Associati"></asp:BoundColumn>
				    </Columns>
					<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;"  HorizontalAlign="Right" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
				</asp:datagrid>
        </div>
        </div>
            <br />
           <div class="RigaPulsanti" >    
         <asp:Button ID="cmdConferma" CssClass="Pulsante" runat="server"  Text="Conferma"  />
         <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server"  Text="Chiudi"  />
       </div>
        </div>
        </fieldset>
</asp:Content>
