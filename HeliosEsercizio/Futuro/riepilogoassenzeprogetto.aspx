<%@ Page Title="Riepilogo Assenze Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="riepilogoassenzeprogetto.aspx.vb" Inherits="Futuro.riepilogoassenzeprogetto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">

<div class="wrapper" style="width:100%;">
    <div class="headers" >
       <h2>
       <asp:Label ID="lblTitolo" runat="server"  Text="Riepilogo Assenze Progetto"></asp:Label>
       </h2>
    </div>
    <div class="RigaVuota">&nbsp;</div>
        <div class="row" >
           <div class="collable" style="width:15%">
           <asp:Label ID="labelProgetto" CssClass="label" AssociatedControlID="lblProgetto" runat="server" Text="Progetto"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="lblProgetto" CssClass="labelDati" runat="server"></asp:Label>
       </div>
   <%--    <div class="collable" style="width:15%" >
           <asp:Label ID="labelStato" AssociatedControlID="lblStato"  runat="server" Text="Stato Progetto"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="lblStato" CssClass="labelDati"  Enabled="false"   runat="server"></asp:Label>
       </div>--%>

       </div>
       <div class="row" id="divData" runat="server">
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelDataInizio" CssClass="label" AssociatedControlID="lblDataInizio" runat="server" Text="Data Inizio"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="lblDataInizio" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
    
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labeldataFine" CssClass="label" AssociatedControlID="lbldataFine"  runat="server" Text="Data Fine"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="lbldataFine" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>

    <div class="RigaPulsanti">
          <asp:Button   ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
   </div>
   
</div>
<div class="wrapper" style="width:100%;border:0px;">
<div class="RigaVuota">&nbsp;</div>
          <div class="row"  >
            <div class="collable"  style="width:48%">
    <asp:datagrid id="dtgPeriodoAssenze" runat="server" Width="100%" Caption="Giorni Assenza per Periodo"
		    AllowPaging="true" CellPadding="2"  CssClass="table" AllowSorting="false" PageSize="10" ShowFooter="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
			<asp:BoundColumn DataField="Anno"  HeaderText="Anno">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="MeseTesto"  HeaderText="Mese">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Registrate"  HeaderText="Registrate">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Confermate" HeaderText="Confermate">
			</asp:BoundColumn>
		</Columns>
                                           <PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>		
    </asp:datagrid>
    </div>
     <div class="collable"  style="width:2%">&nbsp;</div>
    <div class="collable"  style="width:48%">
    <asp:datagrid id="dtgCausaliAssenze" runat="server" Width="100%" Caption="Giorni Assenza per Causale"
		    AllowPaging="true" CellPadding="2"  CssClass="table" AllowSorting="false" PageSize="10" AutoGenerateColumns="False" ShowFooter="false" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
			<asp:BoundColumn DataField="Causale"  HeaderText="Causale">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Registrate"  HeaderText="Registrate">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Confermate"  HeaderText="Confermate">
			</asp:BoundColumn>
		</Columns>
                                           <PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>		
    </asp:datagrid>
    </div>
       </div>
    <div class="RigaVuota">&nbsp;
    </div>
        <div class="row" >
           <div class="collable" style="width:20%">
           <asp:Label ID="labelTotRegistrate" CssClass="label" AssociatedControlID="lblTotRegistrate" runat="server" Text="Totale Assenze Registrate"></asp:Label>
       </div>
       <div class="colOggetti" style="width:30%">
              <asp:Label ID="lblTotRegistrate" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       <div class="collable" style="width:20%" >
           <asp:Label ID="labelTotConfermate" AssociatedControlID="lblTotConfermate"  runat="server" Text="Totale Assenze Confermate"></asp:Label>
       </div>
       <div class="colOggetti" style="width:30%">
           <asp:Label ID="lblTotConfermate" CssClass="labelDati"  Enabled="false"   runat="server"></asp:Label>
       </div>

       </div>
    
</div>
</fieldset>
</asp:Content>
