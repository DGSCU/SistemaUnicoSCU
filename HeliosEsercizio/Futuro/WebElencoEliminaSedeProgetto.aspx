<%@ Page Title="Elimina Sedi Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebElencoEliminaSedeProgetto.aspx.vb" Inherits="Futuro.WebElencoEliminaSedeProgetto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="fieldsetrasparente">
<div class="wrapper" style="width:100%;border:0px">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblIntProgetto" runat="server"  Text="Elenco Personale Associato alla sede "></asp:Label>
      <asp:label  id="lblSede" style="vertical-align:middle"  Text="&nbsp;" runat="server"></asp:label>
      </h2>
    </div>
  
    <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
      &nbsp;</div>
         <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" ></asp:label>
         <asp:label id="lblmessaggioInfo"  runat="server" CssClass="msgInfo" ></asp:label>
          <asp:label id="lblConferma"  runat="server" CssClass="msgConferma" ></asp:label>
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
</div>
<asp:datagrid id="dgRisultatoRicerca" Caption="Elenco Personale Associato"   runat="server" Width="100%" ToolTip="Risorse Progetto" 
              AllowPaging="true" PageSize="10"  CssClass="table" AllowSorting="false"  AutoGenerateColumns="False" UseAccessibleHeader="True" 
              Visible="true">
	<FooterStyle></FooterStyle>
	<SelectedItemStyle  BackColor="White" ></SelectedItemStyle>
	<EditItemStyle></EditItemStyle>
	<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	<ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	<HeaderStyle></HeaderStyle>
    <Columns>
		<asp:BoundColumn Visible="false" DataField="img">
			<HeaderStyle HorizontalAlign="Center" Width="10px" VerticalAlign="Top"></HeaderStyle>
			<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="nominativo" HeaderText="Nominativo">
			<HeaderStyle HorizontalAlign="Center" Width="40px" VerticalAlign="Middle"></HeaderStyle>
			<ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="datanascita" HeaderText="Data Nascita" DataFormatString="{0:d}">
			<HeaderStyle Wrap="False" Width="10px"></HeaderStyle>
			<ItemStyle Wrap="False"></ItemStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="denominazione" HeaderText="Comune Nascita">
			<HeaderStyle Width="20px"></HeaderStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="telefono" HeaderText="Telefono">
			<HeaderStyle Width="10px"></HeaderStyle>
		</asp:BoundColumn>
		<asp:BoundColumn DataField="email" HeaderText="Email">
			<HeaderStyle Width="20px"></HeaderStyle>
		</asp:BoundColumn>
		<asp:TemplateColumn Visible="False">
			<HeaderStyle Width="40px"></HeaderStyle>
			<HeaderTemplate>
				<b></b>
			</HeaderTemplate>
			<ItemTemplate>
				<asp:CheckBox id="check1" onclick="javascript:check()" toolTip="Sede Attuazione Inclusa" AutoPostBack="False"
					runat="server"></asp:CheckBox>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:BoundColumn Visible="False" DataField="idass" HeaderText="idAss"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="accreditato" HeaderText="Accreditato"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="identePersonaleRuolo" HeaderText="identePersonaleRuolo"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="identesedeattuazione" HeaderText="idSedeAttuazione"></asp:BoundColumn>
		<asp:BoundColumn DataField="descrabb" HeaderText="Ruolo"></asp:BoundColumn>
	</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  
                    Mode="NumericPages"></PagerStyle>
</asp:datagrid>
<div class="wrapper" style="width:100%;border:0px">
<div class="RigaVuota">&nbsp;</div>
   <div class="RigaPulsanti"> 
        <asp:Button id="cmdConferma" CssClass="Pulsante"  runat="server" Text="Conferma" ></asp:Button>
        <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
</div>
         <asp:HiddenField ID="lblidsedeattuazione" runat="server" />
         <asp:HiddenField ID="lblidSede" runat="server" />
         <asp:HiddenField ID="lblidattEs" runat="server" />
    </div>
</fieldset>
</asp:Content>
