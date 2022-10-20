<%@ Page Title="Elenco Check List" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCheckListRicerca.aspx.vb" Inherits="Futuro.WfrmCheckListRicerca" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="firefox">
    <fieldset class="ContornoPaginaScroll"> 
        <legend>CHECK LIST ELENCO</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="CHECK LISTE PRESENZE"></asp:Label>&nbsp;</h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
                <div class="row" >
                <div class="collable" style="width:15%">  
                    <asp:Label ID="lblCodiceEnte" runat="server" AssociatedControlID="txtCodiceEnte" Text="Codice Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtCodiceEnte" runat="server"> </asp:TextBox>
                </div>
                <div class="collable" style="width:15%">  
                    <asp:Label ID="lblNomeEnte" runat="server" AssociatedControlID="txtNomeEnte" Text="Nome Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtNomeEnte" runat="server"> </asp:TextBox>
                </div>
                </div>
                <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodProgRend" CssClass="label" AssociatedControlID="txtCodProgRendi" runat="server" Text="Codice Progetto Rendicontazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">              
                    <asp:TextBox ID="txtCodProgRendi" runat="server"> </asp:TextBox> 
                    </div>
                    <div class="collable" style="width:15%"> 
                        <asp:Label ID="lblTitoloProg" AssociatedControlID="txtTitoloProg" runat="server" Text="Titolo Progetto"></asp:Label>     
                    </div>
                    <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtTitoloProg" runat="server"></asp:TextBox>
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
                    <asp:Label ID="lblcompetenze" CssClass="label" AssociatedControlID="Cbocompetenze" runat="server" Text="Competenze"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                        <asp:DropDownList ID="Cbocompetenze" runat="server"/>
                </div>
                <div class="collable" style="width:15%">  
                    <asp:Label ID="lblEsecuzionePagamento" CssClass="label" AssociatedControlID="CboEsecuzionePagamento" runat="server" Text="Esecuzione Pagamento"></asp:Label>    
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="CboEsecuzionePagamento" runat="server" />
                </div>
            </div>
                <div class="row" >
                <div class="collable" style="width:15%">     
                    <asp:Label ID="lblStatoChecklist" CssClass="label" AssociatedControlID="CboStatoChecklist" runat="server" Text="Stato Checklist"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="CboStatoChecklist" runat="server"/>
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblcodicechecklist" CssClass="label" AssociatedControlID="txtcodicechecklist" runat="server" Text="Codice Check List"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:TextBox ID="txtcodicechecklist" CssClass="textbox" runat="server"></asp:TextBox>  
                </div>
                </div>
                <div class="row" >
                <div class="collable" style="width:15%">     
                    <asp:Label ID="LblTipoCheckList" CssClass="label" AssociatedControlID="ddlTipoCheList" runat="server" Text="Tipo Checklist"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlTipoCheList" runat="server">
                        <asp:ListItem Value="0">Tutte</asp:ListItem>
					    <asp:ListItem Value="1">Collettive</asp:ListItem>
                        <asp:ListItem Value="2">Individuali</asp:ListItem>
                    </asp:DropDownList>
                </div>
                </div>
                <div class="RigaPulsanti">
                <asp:Button ID="CmdEsporta" runat="server" Visible="false" CssClass="Pulsante" 
                        Text="Esporta CSV" UseSubmitBehavior="False" />&nbsp;
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <br />
                <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca delle CheckList Collettive" CssClass="linkStampa" Text="Elenco CheckList Collettive" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
                <asp:HyperLink ID="ApriCSV1Indi" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca delle CheckList Individuali" CssClass="linkStampa" Text="Elenco CheckList Individuali" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
                <br />           
            </div>
            </div>
            <p></p>
            <asp:datagrid id="dgRisultatoRicerca" runat="server" 
            Caption="Risultato Ricerca Elenco CheckList Collettive"  CssClass="table" 
            CellPadding="2" PageSize = "5"  AllowPaging="True" 	
            AllowSorting="True"  AutoGenerateColumns="False" 
            UseAccessibleHeader="True"  Width="100%">
		    <FooterStyle></FooterStyle>
		    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
	        <Columns>
            <asp:BoundColumn Visible="False" DataField="IdCheckList" HeaderText="IdCheckList"></asp:BoundColumn>   
                <asp:TemplateColumn HeaderText="Lista"> 
                    <ItemTemplate >
                        <asp:ImageButton ID="CmdImgLista" AlternateText="Seleziona" CommandName="Selezionato" ImageUrl="~/Images/vincoli_small.png" runat="server" />
                    </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="codicechecklist"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Checklist"></asp:BoundColumn>
            <asp:BoundColumn DataField="CodiceProgettoRendicontazione"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Progetto Rendicontazione"></asp:BoundColumn>
            <asp:BoundColumn DataField="Titolo" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Titolo"></asp:BoundColumn>
            <asp:BoundColumn DataField="Ente" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Ente"></asp:BoundColumn>
            <asp:BoundColumn DataField="Competenze" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Comp."></asp:BoundColumn>
		    <asp:BoundColumn DataField="EsecuzionePagamento"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Esec. Pag."></asp:BoundColumn>
            <asp:BoundColumn DataField="StatoCheckList"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Stato CheckList"></asp:BoundColumn>
            <asp:BoundColumn DataField="AnnoCompetenza"  HeaderText="AnnoCompetenza" Visible="false"></asp:BoundColumn>
            <asp:BoundColumn DataField="MeseCompetenza"  HeaderText="MeseCompetenza" Visible="false"></asp:BoundColumn>
        </Columns>
	    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>
    </asp:datagrid>
            <p></p>
  
          <asp:datagrid id="dgRisultatoRicercaIndiduale" runat="server" 
            Caption="Risultato Ricerca Elenco CheckList Individuali"  CssClass="table" 
            CellPadding="2" PageSize = "5"  AllowPaging="True" 	
            AllowSorting="True"  AutoGenerateColumns="False" 
            UseAccessibleHeader="True"  Width="100%">
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
                <asp:BoundColumn DataField="codicechecklist"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Checklist">
                    <HeaderStyle CssClass="linkPageGridRidotto"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="CodiceProgettoRendicontazione"   HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Progetto Rendicontazione">
                    <HeaderStyle CssClass="linkPageGridRidotto" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="CodiceVolontario"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Volontario">
                    <HeaderStyle CssClass="linkPageGridRidotto"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Cognome"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Cognome">
                    <HeaderStyle CssClass="linkPageGridRidotto"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Nome"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Nome">
                    <HeaderStyle CssClass="linkPageGridRidotto"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="CodiceFiscale"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Codice Fiscale">
                    <HeaderStyle CssClass="linkPageGridRidotto"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Competenze" HeaderStyle-CssClass="linkPageGridRidotto"  HeaderText="Comp.">
                    <HeaderStyle CssClass="linkPageGridRidotto"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="EsecuzionePagamento"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Esec. Pag.">
                    <HeaderStyle CssClass="linkPageGridRidotto"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="StatoCheckList"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Stato CheckList">
                    <HeaderStyle CssClass="linkPageGridRidotto"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="AnnoCompetenza"  HeaderText="AnnoCompetenza" Visible="false"></asp:BoundColumn>
                <asp:BoundColumn DataField="MeseCompetenza"  HeaderText="MeseCompetenza" Visible="false"></asp:BoundColumn>
            </Columns>
            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>
        </asp:datagrid>
     
    </fieldset>
</div>
</asp:Content>
