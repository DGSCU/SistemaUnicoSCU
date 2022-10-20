<%@ Page Title="Ricerca Bandi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaBandi.aspx.vb" Inherits="Futuro.WfrmRicercaBandi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
      <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
        <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
        <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <style type="text/css">
            .ui-datepicker {
                font-size: 11px;
            }

        </style>
        <script type="text/javascript" >
           /* <![CDATA[ */
            $(function () {
                var clientId = "#" + CostruisciId('txtInizio');
                    $("" + clientId + "").datepicker();
            });
               $(function () {
                var clientId = "#" + CostruisciId('txtfine');
                    $("" + clientId + "").datepicker();
            });

                 /* ]]> */
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Ricerca Bandi</legend>
<div class="wrapper" style="width:100%">
    <div class="headers" >
       <h2>
      <asp:label  id="lblTitolo" style="vertical-align:middle"  Text="Ricerca Bandi" runat="server"></asp:label>
      </h2>
    </div>
  
    <div class="rowGroup" style="height:auto">
        <div class="RigaVuota">
      &nbsp;</div>
         <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore" ></asp:label>
        <div class="RigaVuota">
      &nbsp;</div>
    </div>
    

    <div class="row" > 
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblBando" CssClass="label" AssociatedControlID="ddlBando"  runat="server" Text="Bando"></asp:Label>
        </div>
        <div class="colOggetti" style="width:85%" >
           <asp:dropdownlist id="ddlBando" runat="server" CssClass="ddlClass"  >
			</asp:dropdownlist>
        </div>

    </div>
        <div class="row" > 
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="Label1" CssClass="label" AssociatedControlID="txtRiferimento"  runat="server" Text="Riferimento"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
           <asp:TextBox id="txtRiferimento" runat="server" CssClass="textbox"  >
			</asp:TextBox>
        </div>

    </div>
        <div class="row" > 
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlStato"  runat="server" Text="Stato"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
           <asp:dropdownlist id="ddlStato" runat="server" CssClass="ddlClass" >
			</asp:dropdownlist>
        </div>
             <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblCompetenze" CssClass="label" AssociatedControlID="ddlCompetenze"  runat="server" Text="Competenze"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
           <asp:dropdownlist id="ddlCompetenze" runat="server" CssClass="ddlClass" >
			</asp:dropdownlist>
        </div>
    </div>
        <div class="row" > 
        <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblInizio" CssClass="label" AssociatedControlID="txtInizio"  runat="server" Text="Data Inizio"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
           <asp:TextBox id="txtInizio" runat="server" CssClass="textboxData"  >
			</asp:TextBox>
        </div>
             <div class="colOggetti" style="width:15%">
            <asp:Label ID="lblfine" CssClass="label" AssociatedControlID="txtfine"  runat="server" Text="Data Fine"></asp:Label>
        </div>
        <div class="colOggetti" style="width:35%" >
           <asp:TextBox id="txtfine" runat="server" CssClass="textboxData"  >
			</asp:TextBox>
        </div>

    </div>
    <div class="RigaPulsanti" > 
       <asp:Button id="cmdRicerca" CssClass="Pulsante"  runat="server" Text="Ricerca"  ></asp:Button>
    <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server"  Text="Chiudi" ></asp:Button>
      <asp:Button ID="cmdEsporta" runat="server" ToolTip="Esporta" CssClass="Pulsante" 
                Text="Esporta CSV" />
        <br />
            <asp:HyperLink ID="ApriCSV1" runat="server" AccessKey="S" CssClass="linkStampa" 
                ForeColor="#003399" Text="DOWNLOAD CSV" 
                ToolTip="Link per la stampa del risultato della ricerca" Visible="False"></asp:HyperLink>

</div>
</div>
<br />
  <asp:datagrid id="dgRisultatoRicerca" Caption="Risultato Ricerca Bandi"   runat="server" Width="100%"
                   AllowPaging="true" PageSize="10"  CssClass="table" AllowSorting="True" 
                   AutoGenerateColumns="False" UseAccessibleHeader="True" Visible="true">
	<FooterStyle></FooterStyle>
	<SelectedItemStyle  BackColor="White" Font-Bold="true"></SelectedItemStyle>
	<EditItemStyle></EditItemStyle>
	<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	<ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	<HeaderStyle></HeaderStyle>
    <Columns>
        <asp:TemplateColumn HeaderText="Selez." > 
            <ItemTemplate >
				<asp:ImageButton ID="linkQuestionario" style="cursor:pointer;" AlternateText="Bando" CommandName="Select" ToolTip="Seleziona Bando" CausesValidation="false" runat="server" ImageUrl="images/vincoli_small.png" />  
                </ItemTemplate>
        </asp:TemplateColumn>
		<asp:BoundColumn DataField="bando" HeaderText="Bando">
		</asp:BoundColumn>
		<asp:BoundColumn  DataField="StatoBando" HeaderText="Stato">
		</asp:BoundColumn>
		<asp:BoundColumn  DataField="DataInizio" HeaderText="Data Inizio">
		</asp:BoundColumn>
		<asp:BoundColumn  DataField="DataFine" HeaderText="Data Fine">
		</asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="idbando" HeaderText="idbando"></asp:BoundColumn>
		<asp:BoundColumn Visible="False" DataField="color" HeaderText="Color"></asp:BoundColumn>
		<asp:BoundColumn  DataField="DataInizioVolontari" HeaderText="Data Inizio Vol."></asp:BoundColumn>
		<asp:BoundColumn  DataField="DataFineVolontari" HeaderText="Data Fine Vol."></asp:BoundColumn>
		<asp:BoundColumn  DataField="importoStanziato" HeaderText="Importo Stanziato" DataFormatString="{0:N2}">
		</asp:BoundColumn>
		<asp:BoundColumn  DataField="AssociazioneAutomatica" HeaderText="Ass. Automatica">
		</asp:BoundColumn>
		<asp:BoundColumn  DataField="EnteAbilitato" HeaderText="Ente Abilitato"></asp:BoundColumn>
	</Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
</asp:datagrid>
<asp:HiddenField  runat="server" ID="lblRicerca"/>
<asp:HiddenField  runat="server" ID="lblpage"/>
<asp:HiddenField  runat="server" ID="txtRicerca"/>
</fieldset>
</asp:Content>

