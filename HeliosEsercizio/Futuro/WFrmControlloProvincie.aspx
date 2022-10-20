<%@ Page Title="Controllo Province" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WFrmControlloProvincie.aspx.vb" Inherits="Futuro.WFrmControlloProvincie" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="fieldsetrasparente">
     <div class="wrapper" style="width:100%;">
         <div class="headers" >
       <h2>
       <asp:Label ID="lblTitolo" runat="server"  Text="Controllo Province"></asp:Label>
       </h2>
       </div>
      <div class="RigaVuota">&nbsp;</div>
        <div class= "row" style="height:auto">  
	    <asp:label id="lblmessaggio" runat="server" CssClass="msgErrore" ></asp:label>
   </div>
    <asp:datagrid id="dgRisultatoRicerca" runat="server" Caption="Elenco Province"  CssClass="table" CellPadding="2"  AllowPaging="true" 	
        AllowSorting="false" AutoGenerateColumns="False" PageSize="10" UseAccessibleHeader="True" ShowFooter="false" Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
			<asp:BoundColumn DataField="IdProvincia" HeaderText="Provincia" Visible="False"></asp:BoundColumn>
			<asp:BoundColumn DataField="Provincia" HeaderText="Provincia">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="NVol" HeaderText="N&#176; Vol.">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="NRleaRic" HeaderText="RLEA Ric.">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="NRleaIns" HeaderText="RLEA Ins.">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="NTutorRic" HeaderText="Tutor Ric.">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="NTutorIns" HeaderText="Tutor Ins.">
			</asp:BoundColumn>
             <asp:TemplateColumn  > 
            <ItemTemplate >
			    <asp:ImageButton ID="ImageButton2"  style="cursor:pointer;" CommandName="Select" AlternateText="Elenco Progetti per Provincia" ToolTip="Elenco Progetti per Provincia" runat="server" ImageURL="images/vincoli_small.png"></asp:ImageButton>      
            </ItemTemplate>
        </asp:TemplateColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    <div class="RigaVuota">&nbsp;</div>
	    <div class="RigaPulsanti"> 
        <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
             <asp:Button ID="cmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" />&nbsp;
                <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
    </div>				
		</div>
        </fieldset>
</asp:Content>
