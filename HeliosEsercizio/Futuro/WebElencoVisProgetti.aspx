<%@ Page Title="Dettaglio Progetti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebElencoVisProgetti.aspx.vb" Inherits="Futuro.WebElencoVisProgetti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend>Dettaglio Progetti</legend>
        <div class="wrapper" style="width:100%;">
          <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server" CssClass="label"  Text="Dettaglio Progetti"></asp:Label>
      </h2>
      <div class="RigaVuota">&nbsp;</div>
      </div>
               <div class="rowGroup" style="height:auto">

		    <asp:label id="lblConferma"  runat="server" CssClass="msgConferma"></asp:label>
            <asp:label id="lblErrore"  runat="server" CssClass="msgErrore"></asp:label>
		</div>

    <div class="RigaVuota">&nbsp;</div>
    <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" 
		    AllowPaging="true" CellPadding="2"  PageSize="10" CssClass="table" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White" Font-Bold="true" ></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
            <asp:TemplateColumn Visible="false"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImageButton1" CommandName="Select" ToolTip="Seleziona Progetto" AlternateText="Seleziona Progetto" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
             <asp:TemplateColumn Visible="False"> 
                <ItemTemplate  >
				    <asp:Image ID="AccProg" CommandName="accettazione" style="cursor:pointer;"  AlternateText="Accetta Progetto"  ToolTip='Accetta Progetto' runat="server"
                    ImageURL="images/valida_small.png"  >
                    </asp:Image>      
                </ItemTemplate>
            </asp:TemplateColumn>
             <asp:TemplateColumn Visible="False"> 
                <ItemTemplate >
				    <asp:Image ID="ValidaProg" CommandName="valutazione" style="cursor:pointer;"  AlternateText="Valutazione Qualit&#224; Progetto"  ToolTip='Valutazione Qualit&#224; Progetto' runat="server"
                    ImageURL="images/vincoli_small.png"  >
                    </asp:Image>      
                </ItemTemplate>
            </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="denominazione" HeaderText="Denominazione"></asp:BoundColumn>
				<asp:BoundColumn DataField="titolo" HeaderText="Titolo">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="bando" HeaderText="Bando">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="ambito" HeaderText="Settore / Area Intervento">
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="data" HeaderText="Data Ultimo Stato">
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idattivit&#224;" HeaderText="idattivit&#224;">
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idente" HeaderText="Idente"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="modificabile" HeaderText="modificabile"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdTipoProgetto" HeaderText="IdTipoProgetto"></asp:BoundColumn>
				<asp:BoundColumn DataField="statoattivit&#224;" HeaderText="Stato Progetto">
				</asp:BoundColumn>
			</Columns>
         <PagerStyle CssClass="linkPageGrid" HorizontalAlign="Center" 
                Mode="NumericPages" NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" />				
    </asp:datagrid>

    <div class="RigaVuota">&nbsp;</div>

      <div class="RigaPulsanti">
            <asp:Button ID="imgchiudi" runat="server" CssClass="Pulsante" Text="Chiudi"  />
            <asp:Button ID="imgForzaChiusuraEnte" runat="server"  CssClass="Pulsante" Text="Forza Chiusura Ente" Visible="False"/>
        </div>
    </div>
    </fieldset>
</asp:Content>
