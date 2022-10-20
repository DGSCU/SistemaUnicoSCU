<%@ Page Title="Elenco Ambiti Attivit&#224;" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmElencoAmbitiAttivita.aspx.vb" Inherits="Futuro.WfrmElencoAmbitiAttivita" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
    <legend>Profili</legend>

    <div class="wrapper" style="width:100%;border:0px">
        <div class="headers" >
            <h2>
            <asp:Label ID="lblTitolo" runat="server"  Text="Elenco Ambiti: "></asp:Label>
            <asp:Label ID="LblDescrAttivita" runat="server"  Text="&nbsp;"></asp:Label>
            </h2>
        </div>
        <div class="row">
            <asp:label  id="lblErrore" runat="server" CssClass="msgErrore"></asp:label>
             <asp:label id="lblConferma" runat="server" CssClass="msgConferma"></asp:label>
              <asp:label id="lblInfo" runat="server" CssClass="msgInfo" ></asp:label>
        </div>
 

                <div class="row">
          <div class="collable" style="width:100%;margin-right:2px">
           
           &nbsp;<asp:checkbox id="chkSelDesel"  runat="server" Font-Bold="True" 
								  TextAlign="Left" Text="Seleziona Tutto" AutoPostBack="True"></asp:checkbox>
          
       </div>
       </div>
        </div>


       <asp:datagrid id="dgElencoAttivita" CssClass="table" runat="server" CellPadding="2" Caption="Elenco Attivit&#224;"  AllowPaging="false" 
                    AllowSorting="True" AutoGenerateColumns="False"  UseAccessibleHeader="True" Visible="true" ShowFooter="false"  Width="100%">
			    <FooterStyle></FooterStyle>
		        <SelectedItemStyle ></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle ></HeaderStyle>
                <Columns>
                <asp:TemplateColumn HeaderText="Sel./Desel.">
					<ItemStyle HorizontalAlign="Center" ></ItemStyle>
						<ItemTemplate>
    						<asp:CheckBox id="check1"  runat="server"  AutoPostBack="false" ToolTip="Seleziona Ambito"></asp:CheckBox>
						</ItemTemplate>
				</asp:TemplateColumn>
                	<asp:BoundColumn DataField="Codifica" HeaderText="Attivita'"></asp:BoundColumn>
								<asp:BoundColumn DataField="MacroAmbitoAttivit&#224;" HeaderText="Settore"></asp:BoundColumn>
								<asp:BoundColumn DataField="AmbitoAttivit&#224;" HeaderText="Area Intervento"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IdAmbitoAttivit&#224;" HeaderText="idAmbitoAttivita"></asp:BoundColumn>
                    </Columns>
                    </asp:datagrid>
                <div class="wrapper" style="width:100%;border:0px;">
            <div class="RigaPulsanti">
            <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" ToolTip="Salva" Text="Salva" />
            <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
         
            </div>
            </div>
            <asp:HiddenField ID="Hdd_chkFlag" runat="server" />
        </fieldset>
    
</asp:Content>
