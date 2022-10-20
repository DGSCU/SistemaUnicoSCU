<%@ Page Title="Elenco Profili" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmElencoProfili.aspx.vb" Inherits="Futuro.WfrmElencoProfili" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
    <legend>Profili</legend>

    <div class="wrapper" style="width:100%;border:0px">
        <div class="headers" >
            <h2>
            <asp:Label ID="lblTitolo" runat="server"  Text="Elenco Profili: "></asp:Label>
            <asp:Label ID="LblDescrTipoProgetto" runat="server"  Text="&nbsp;"></asp:Label>
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


       <asp:datagrid id="dgElencoProfili" CssClass="table" runat="server" CellPadding="2" Caption="Elenco Profili"  AllowPaging="false" 
                    AllowSorting="True" AutoGenerateColumns="False"  UseAccessibleHeader="True" Visible="true" ShowFooter="false"  Width="100%">
			    <FooterStyle></FooterStyle>
		        <SelectedItemStyle ></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle ></HeaderStyle>
                <Columns>
                <asp:TemplateColumn HeaderText="Sel./Desel.">
					<ItemStyle HorizontalAlign="Center"  ></ItemStyle>
						<ItemTemplate>
    						<asp:CheckBox id="check1"  DataField="Descrizione"  runat="server"  AutoPostBack="false" ToolTip="Seleziona Profilo"></asp:CheckBox>
						</ItemTemplate>
				</asp:TemplateColumn>
                <asp:BoundColumn DataField="Descrizione" HeaderText="Profili"></asp:BoundColumn>
				<asp:BoundColumn DataField="tipo" HeaderText="Tipo"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idprofilo" HeaderText="idprofili"></asp:BoundColumn>
                    </Columns>
                    </asp:datagrid>
                <div class="wrapper" style="width:100%;border:0px;">
            <div class="RigaPulsanti">
            <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" ToolTip="Salva" Text="Salva" />
            <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
         
            </div>
            </div>
        </fieldset>
        <asp:HiddenField ID="Hdd_chkFlag" runat="server" />
</asp:Content>
