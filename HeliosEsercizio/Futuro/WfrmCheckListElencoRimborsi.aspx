<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCheckListElencoRimborsi.aspx.vb" Inherits="Futuro.WfrmCheckListElencoRimborsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }
        </style>
          
       <script  type="text/javascript">

           var formId;
           var prefissoIdClient = "MainContent_";

           function ImpostaFormId() {
               formId = document.forms[0].id;
               return formId;
           }
           function CostruisciId(IdServer) {
               var IdClient = prefissoIdClient + IdServer
               return IdClient
           }


           $(function () {
               var IdData = CostruisciId('txtdataDal');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });

           $(function () {
               var IdData = CostruisciId('txtdataAl');
               var sharpIdData = "#" + IdData
               $("" + sharpIdData + "").datepicker();
           });

         



	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>CHECK LIST ELENCO RIMBORSI INDIVIDUALI</legend>
<div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="CHECK LISTE RIMBORSI INDIVIDUALI" 
                       ToolTip="I"></asp:Label>&nbsp;</h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodProgRend" CssClass="label" AssociatedControlID="txtCodProgRendi" runat="server" Text="Codice Progetto Rendicontazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%"> 
                      
                    <asp:TextBox ID="txtCodProgRendi" runat="server"></asp:TextBox>
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
                    <asp:Label ID="lblRimborsoDal" CssClass="label" AssociatedControlID="txtdataDal" runat="server" Text="Rimborso Dal:"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                    <asp:TextBox ID="txtdataDal" MaxLength="10" ssClass="textboxData" runat="server"></asp:TextBox>
                    

                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRimborsoAl" CssClass="label"  AssociatedControlID="txtdataAl" runat="server" Text="Rimborso Al:"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    
                    <asp:TextBox ID="txtdataAl" MaxLength="10" ssClass="textboxData" runat="server"></asp:TextBox>
                    
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
                <br />
           
	           
            </div>
         </div>
         <p></p>
         <asp:datagrid id="dgRisultatoRicerca" runat="server" Caption="Risultato Ricerca Elenco Rimborsi Individuali"  CssClass="table" CellPadding="2" PageSize = "10"  AllowPaging="true" 	
        AllowSorting="True"  AutoGenerateColumns="False" UseAccessibleHeader="True"  Width="100%">
		<FooterStyle></FooterStyle>
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
	    <Columns>
        <asp:BoundColumn Visible="False" DataField="IdCheckList" HeaderText="IdCheckList"></asp:BoundColumn>
        <asp:BoundColumn Visible="False" DataField="IdentitàRimborso" HeaderText="IdEntitaRimborso"></asp:BoundColumn>      
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
       <asp:BoundColumn DataField="Importo"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Importo Euro"></asp:BoundColumn>
       <asp:BoundColumn DataField="DataRiferimento" DataFormatString="{0:d}"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Data"></asp:BoundColumn>
       <asp:BoundColumn DataField="StatoCheckList"  HeaderStyle-CssClass="linkPageGridRidotto" HeaderText="Stato"></asp:BoundColumn>
    </Columns>
	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGridRidotto"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </fieldset>
</asp:Content>
