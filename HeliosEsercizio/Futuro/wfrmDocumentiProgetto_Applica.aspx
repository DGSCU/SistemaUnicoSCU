<%@ Page Title="Applicazione Documenti A Progetti Registrati" Language="vb"  AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmDocumentiProgetto_Applica.aspx.vb" Async="True" Inherits="Futuro.wfrmDocumentiProgetto_Applica" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript" language="javascript">
   /* <![CDATA[ */
      function toggle(Checkbox) {
          var GridVwHeaderChckbox = document.getElementById("<%=dtgProgetti.ClientID %>");
          for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
              GridVwHeaderChckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
          }
      }
        /* ]]> */
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend>Applicazione Documenti A Progetti Registrati</legend>
     <div class="wrapper" style="width:100%">
      <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="Documenti Progetto"></asp:Label>
            </h2>
        </div>
        <div class="RigaVuota" >&nbsp;
        </div>
     <div class="row" style="height:auto">
     <asp:Label id="lblmsg" runat="server" CssClass="msgErrore"></asp:Label>
    </div>
    <div class="row" >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labellblTitolo" CssClass="label" AssociatedControlID="lblTitoloProgetto" runat="server" Text="Titolo Progetto"></asp:Label>
       </div>
         <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="lblTitoloProgetto" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelLblCodice" CssClass="label" AssociatedControlID="LblCodice"  runat="server" Text="Codice"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="LblCodice" CssClass="labelDati" runat="server"></asp:Label>
       </div>

    
       </div>
    <div class="row" >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelLblStatoProgetto" CssClass="label" AssociatedControlID="LblStatoProgetto" runat="server" Text="Stato Progetto"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="LblStatoProgetto" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelLblStatoIstanza" CssClass="label" AssociatedControlID="LblStatoIstanza"  runat="server" Text="Stato Presentazione Istanza"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="LblStatoIstanza" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>

    </div>
    <div class="wrapper" style="width:100%;border:0px">
    <div class="RigaVuota" >&nbsp;
    </div>
    <div class="RigaPulsanti" > 
    <asp:Button id="cmdSalva" CssClass="Pulsante"  runat="server" Text="Salva" ></asp:Button> &nbsp;
        <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
    </div>
</div>
<div class="wrapper" style="width:100%;border:0px" id="divGestione" runat="server">
    <div class="RigaVuota" >&nbsp;
    </div>
    <asp:datagrid id="dtgElencoDocumentiProgetto" runat="server" Width="100%"  ToolTip="Elenco Documenti Progetto" 
				Caption="Elenco documenti esistenti" AllowPaging="false" CellPadding="2" Font-Size="Small"  CssClass="table"  
                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<FooterStyle></FooterStyle>
			<SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
			<EditItemStyle></EditItemStyle>
			<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
			<HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="False" DataField="Idattivit&#224;documento">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="FileName" HeaderText="Nome File">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="DataInserimento" HeaderText="Data Inserimento">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="hashvalue" HeaderText="Hash Value">
				</asp:BoundColumn>
				<asp:TemplateColumn>
					<ItemTemplate>
						<asp:CheckBox id="chkSel" runat="server"  AutoPostBack="false" ToolTip="Seleziona Tipo Documento"></asp:CheckBox>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>                              
		</asp:datagrid>
     <div class="RigaVuota" >&nbsp;
    </div>
     <div class="RigaVuota" >&nbsp;
    </div>
    <div class="wrapper" style="width:100%;border:0px">
    <div class="row" >
    <div class="collable"  style="width:15%" >
    <asp:Label ID="lblSede" runat="server" AssociatedControlID="ddlProvincia" CssClass="label" Text="Provincia Sede Progetto"></asp:Label>
    </div>
        <div class="colOggetti" style="width:35%" >
            <asp:DropDownList id="ddlProvincia" runat="server"  CssClass="ddlClass" AutoPostBack="true"   />
        </div>
    </div>
    </div>
    <asp:datagrid id="dtgProgetti" runat="server" Width="100%"  ToolTip="Elenco progetti registrati" 
				Caption="Elenco progetti registrati" AllowPaging="false" CellPadding="2" Font-Size="Small"  CssClass="table"  
                AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<FooterStyle></FooterStyle>
			<SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
			<EditItemStyle></EditItemStyle>
			<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
			<HeaderStyle></HeaderStyle>
            <Columns>
				<asp:BoundColumn Visible="False" DataField="IdAttivit&#224;" HeaderText="IdAttivit&#224;">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Titolo" HeaderText="Progetto">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="SettoreArea" HeaderText="Settore/Area">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="PostiRichiesti" HeaderText="Posti Richiesti">
				</asp:BoundColumn>
				<asp:TemplateColumn>
                <HeaderTemplate><asp:CheckBox id="chkSelDesel"   onclick="toggle(this)" runat="server" ToolTip="Seleziona Tutto"   /></HeaderTemplate>
					<ItemTemplate>
						<asp:CheckBox id="chkSelProg" runat="server" AutoPostBack="false" ToolTip="Seleziona Tipo Documento"></asp:CheckBox>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>                              
		</asp:datagrid>
</div>
    </fieldset>
</asp:Content>
