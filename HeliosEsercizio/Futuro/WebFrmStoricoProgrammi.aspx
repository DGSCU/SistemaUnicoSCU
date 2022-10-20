﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebFrmStoricoProgrammi.aspx.vb" Inherits="Futuro.WebFrmStoricoProgrammi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Storico</legend>
<div class="wrapper" style="width:100%;border:0px">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server" CssClass="label"  Text="Storico Programma"></asp:Label>
      </h2>
      </div>
 <br />
  <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
                <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
            </div>
 

 </div>
<asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%"  Caption="Storico Programma"  
			AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table" 
            AllowSorting="True" AutoGenerateColumns="False" 
              UseAccessibleHeader="True">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White" Font-Bold="true"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
		<HeaderStyle></HeaderStyle>
        <Columns>
             <asp:TemplateColumn HeaderText="Selez." Visible="False"  > 
                <ItemTemplate >
	                <asp:ImageButton ID="SelProgramma" CommandName="Select" AlternateText="Seleziona Storico" ToolTip="Storico Programma" runat="server" ImageURL="images/cronologia_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                </asp:TemplateColumn>
			<asp:BoundColumn DataField="datainserimento" HeaderText="Data Inserimento">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Username" HeaderText="Utente">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="punteggiofinale" HeaderText="Punteggio finale">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="punteggiocp" HeaderText="Punteggio CP">
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="idstorico" HeaderText="idStorico">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="PunteggioCOE" HeaderText="Punteggio COE"></asp:BoundColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
   <div class="wrapper" style="width:100%;border:0px">
       <div class="RigaPulsanti">
          <asp:Button  ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;      
   </div>
   </div>
   <asp:HiddenField id="hf_IdProgramma" runat="server"/>
<asp:HiddenField id="hf_Pagina" runat="server" />

</fieldset>
</asp:Content>
