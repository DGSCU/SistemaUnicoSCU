<%@ Page Title="Elenco volontari progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="elencovolontariprogetto.aspx.vb" Inherits="Futuro.elencovolontariprogetto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Volontari Progetto</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:Label ID="lblTitolo" runat="server" CssClass="label"  Text="Ricerca Volontari Progetto ' "></asp:Label>
       <asp:Label ID="lblprogetto"  CssClass="label" runat="server"  Text=""></asp:Label>
       <asp:Label ID="Label1" runat="server" CssClass="label"  Text="'"></asp:Label>

      </h2>
      </div>
    <div class="row" style="height:auto" >
        <asp:Label id="lblmessaggio" CssClass="msgConferma" runat="server" ></asp:Label>
    <asp:Label id="lblerrore" CssClass="msgErrore"  runat="server"></asp:Label>
    </div>
    <div class="RigaVuota" >&nbsp;
    </div>

    <div class="row" >
       <div class="collable" style="width:15%">      
           <asp:Label ID="IdLblCognome" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">       
           <asp:TextBox ID="txtCognome" CssClass="textbox" autofocus="true" runat="server"></asp:TextBox>         
       </div>
       <div class="collable" style="width:15%">
            <asp:Label ID="IdLblNome" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="lblCodiceFiscale" AssociatedControlID="txtCodiceFiscale" runat="server" Text="Codice Fiscale"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">    
             <asp:TextBox ID="txtCodiceFiscale" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
           <div class="collable" style="width:15%">
            <asp:Label ID="IdLblCodProgetto" AssociatedControlID="ddlPosto" runat="server" Text="Tipo Posto"></asp:Label>
       </div>
        <div class="colOggetti" style="width:35%">
            <asp:dropdownlist id="ddlPosto" runat="server" CssClass="ddlClass" ></asp:dropdownlist>
       </div>
     
    </div>
     <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="lblSede" AssociatedControlID="txtSede" runat="server" Text="Sede"></asp:Label>
       </div>
        <div class="colOggetti" style="width:85%">    
             <asp:TextBox ID="txtSede" CssClass="textbox" runat="server"></asp:TextBox>  
       </div>
     
    </div>
 
    <div class="RigaPulsanti">
        <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
          <asp:Button  ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                   <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" />&nbsp;
                <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
      
   </div>
 </div>
 <div class="RigaVuota" >&nbsp;
    </div>
<asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%"  Caption="Risultato Ricerca Volontari Progetto"  
			AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" 
              UseAccessibleHeader="True">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White" Font-Bold="true"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
			<asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Codice Fiscale">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="StatoAttivit&#224;Entit&#224;" Visible="false" HeaderText="Stato Attivit&#224; Volontario">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="statoVol" HeaderText="Stato Volontario"></asp:BoundColumn>
			<asp:BoundColumn DataField="DataInizioServizio" HeaderText="Data Inizio Servizio">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataFineServizio" HeaderText="Data Fine Servizio">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DenominazioneSedeAttuazione" Visible="false" HeaderText="Sede Attuazione">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Sede" HeaderText="Sede">
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IDEntit&#224;" HeaderText="IdEntit&#224;">
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IdEnteSedeAttuazione" HeaderText="IdEnteSedeAttuazione">
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IDEnteSede" HeaderText="IdEnteSede">
			</asp:BoundColumn>
			<asp:BoundColumn DataField="posto" HeaderText="Tipo Posto"></asp:BoundColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
   

</fieldset>
</asp:Content>
