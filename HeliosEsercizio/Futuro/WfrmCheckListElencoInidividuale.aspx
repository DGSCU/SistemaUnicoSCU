﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCheckListElencoInidividuale.aspx.vb" Inherits="Futuro.WfrmCheckListElencoInidividuale" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>CHECK LIST ELENCO PAGHE INDIVIDUALI</legend>
<div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="CHECK LIST PAGHE INDIVIDUALI" 
                       ToolTip="I"></asp:Label>&nbsp;</h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodProgRend" CssClass="label" AssociatedControlID="CboCodProgRend" runat="server" Text="Codice Progetto Rendicontazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                    <asp:DropDownList ID="CboCodProgRend" runat="server">
                    
                    </asp:DropDownList>
                     
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblcompetenze" CssClass="label" AssociatedControlID="Cbocompetenze" runat="server" Text="Competenze"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    
                    <asp:DropDownList ID="Cbocompetenze" runat="server">
                   
                    </asp:DropDownList>
                     
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEsecuzionePagamento" CssClass="label" AssociatedControlID="CboEsecuzionePagamento" runat="server" Text="Esecuzione Pagamento"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                   
                     
                     <asp:DropDownList ID="CboEsecuzionePagamento" runat="server">
                    
                    </asp:DropDownList>
                    
                   
                     
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStatoChecklist" CssClass="label" AssociatedControlID="CboStatoChecklist" runat="server" Text="Stato Checklist"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                    <asp:DropDownList ID="CboStatoChecklist" runat="server">
                    
                    </asp:DropDownList>
                     
                </div>
            </div>
             <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblcodicefiscale" CssClass="label" AssociatedControlID="txtcodicefiscale" runat="server" Text="Codice Fiscale"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                   
                     
                    <asp:TextBox ID="txtcodicefiscale" runat="server"></asp:TextBox>
                    
                   
                     
                </div>
            <div class="collable" style="width:15%">      
                    <asp:Label ID="lblcodicevolontario" CssClass="label" AssociatedControlID="txtcodicevolontario" runat="server" Text="Codice Volontario"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                   
                     
                    <asp:TextBox ID="txtcodicevolontario" runat="server"></asp:TextBox>
                    
                   
                     
                </div>
                </div>
                 <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblcognome" CssClass="label" AssociatedControlID="txtcognome" runat="server" Text="Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                   
                     
                    <asp:TextBox ID="txtcognome" runat="server"></asp:TextBox>
                    
                   
                     
                </div>
            <div class="collable" style="width:15%">      
                    <asp:Label ID="lblnome" CssClass="label" AssociatedControlID="txtnome" runat="server" Text="Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                   
                     
                    <asp:TextBox ID="txtnome" runat="server"></asp:TextBox>
                    
                   
                     
                </div>
                </div>
               <div class="row" >
               <div class="collable" style="width:15%">
                 <asp:Label ID="lblcodicechecklist" CssClass="label" AssociatedControlID="txtcodicechecklist" runat="server" Text="Codice Check List"></asp:Label>
                </div>
                 <div class="colOggetti" style="width:35%">
                 <asp:TextBox ID="txtcodicechecklist" CssClass="textbox" runat="server"></asp:TextBox>  
               </div>
               </div>
            <div class="RigaPulsanti">
                <asp:Button ID="CmdEsporta" runat="server" Visible="false" CssClass="Pulsante" Text="Esporta CSV" />&nbsp;
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <br />
                <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
               
	           
            </div>
         </div>
         <p></p>
         <asp:datagrid id="dgRisultatoRicerca" runat="server" Caption="Risultato Ricerca Elenco Paghe Individuali"  CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True"  AutoGenerateColumns="False" UseAccessibleHeader="True"  Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
        <asp:BoundColumn Visible="False" DataField="IdCheckList" HeaderText="IdCheckList"></asp:BoundColumn>
        <asp:BoundColumn Visible="False" DataField="identità" HeaderText="IdEntita"></asp:BoundColumn>      
         <asp:TemplateColumn HeaderText="Lista"> 
             <ItemTemplate >
                 <asp:ImageButton ID="CmdImgVolontario" AlternateText="Seleziona" CommandName="Selezionato" ImageUrl="~/Images/vincoli_small.png" runat="server" />
             </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn DataField="codicechecklist"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Checklist"></asp:BoundColumn>
        <asp:BoundColumn DataField="CodiceVolontario"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Volontario"></asp:BoundColumn>
        <asp:BoundColumn DataField="Cognome"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Cognome"></asp:BoundColumn>
        <asp:BoundColumn DataField="Nome"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Nome"></asp:BoundColumn>
        <asp:BoundColumn DataField="CodiceFiscale"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Fiscale"></asp:BoundColumn>
         <asp:BoundColumn DataField="CodiceProgettoRendicontazione"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Progetto Rendicontazione"></asp:BoundColumn>
        <asp:BoundColumn DataField="Competenze" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Competenze"></asp:BoundColumn>
		<asp:BoundColumn DataField="EsecuzionePagamento"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Esecuzione Pagamento"></asp:BoundColumn>
        <asp:BoundColumn DataField="StatoCheckList"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Stato CheckList"></asp:BoundColumn>
        <asp:BoundColumn DataField="AnnoCompetenza"  HeaderText="AnnoCompetenza" Visible="false"></asp:BoundColumn>
        <asp:BoundColumn DataField="MeseCompetenza"  HeaderText="MeseCompetenza" Visible="false"></asp:BoundColumn>
    </Columns>
	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </fieldset>
</asp:Content>
