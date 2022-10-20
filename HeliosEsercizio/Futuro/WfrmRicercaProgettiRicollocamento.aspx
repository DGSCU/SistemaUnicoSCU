<%@ Page Title="Ricerca Progetti Ricollocamento" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaProgettiRicollocamento.aspx.vb" Inherits="Futuro.WfrmRicercaProgettiRicollocamento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script type="text/javascript">

         function VisualizzaSanzione(IDEnteSedeAttuazione, IdEnte) {
             window.open("WfrmSedeSanzionata.aspx?IDEnteSedeAttuazione=" + IDEnteSedeAttuazione + "&IdEnte=" + IdEnte, "SedeSanzionata", "width=950, height=600, toolbar=no, location=no, menubar=no, scrollbars=yes");
         }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Progetti Ricollocamento</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Progetti Ricollocamento"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="txtEnte" runat="server" Text="Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtEnte" autofocus="true" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtCodEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProgetto" CssClass="label" AssociatedControlID="txtProgetto" runat="server" Text="Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProgetto" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblBando" CssClass="label" AssociatedControlID="txtBando" runat="server" Text="Bando"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtBando" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblSettore" CssClass="label" AssociatedControlID="cboSettore" runat="server" Text="Settore"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="cboSettore" CssClass="ddlClass" AutoPostBack="True" Runat="server"></asp:dropdownlist>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblArea" CssClass="label" AssociatedControlID="cboArea" runat="server" Text="Area"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="cboArea" runat="server" CssClass="label" Enabled="False">
                        <asp:ListItem></asp:ListItem>
                    </asp:dropdownlist>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRegione" CssClass="label" AssociatedControlID="txtRegione" runat="server" Text="Regione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtRegione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProvincia" CssClass="label" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProvincia" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblcodiceSede" CssClass="label" AssociatedControlID="TxtCodiceSede" runat="server" Text="Codice Sede"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtCodiceSede" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%" id="divlblSegnalazioneSanzione" runat="server">      
                    <asp:Label ID="lblSegnalazioneSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%" id="divddlSegnalazioneSanzione" runat="server">
                    <asp:DropDownList ID="ddlSegnalazioneSanzione" runat="server" CssClass="ddlClass">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
					    <asp:ListItem Value="0">No</asp:ListItem>
					    <asp:ListItem Value="1">Si</asp:ListItem>
                    </asp:DropDownList>
               </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante"  Text="Chiudi"  />&nbsp;
            </div>
        </div>
         <br />
    <h3>
        <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgRisultatoRicerca"></asp:label>
    </h3>
    <br />
    <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Progetti" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="10" AllowSorting="True" AutoGenerateColumns="False"  Visible="False" UseAccessibleHeader="True" DataKeyField="DataFields">
        <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
            <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="ImageButton1" CommandName="Select" ToolTip="Seleziona Progetto" AlternateText="Seleziona Progetto" runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			<asp:BoundColumn DataField="CodEnte" HeaderText="Cod Ente"></asp:BoundColumn>
			<asp:BoundColumn DataField="TitoloProgetto" HeaderText="Progetto"></asp:BoundColumn>
			<asp:BoundColumn DataField="SettoreAmbito" HeaderText="Settore/ Ambito">
				<HeaderStyle Width="50px"></HeaderStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataInizio" HeaderText="Data Inizio" DataFormatString="{0:d}">
				<HeaderStyle Width="50px"></HeaderStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="SedeFisica" HeaderText="Sede Fisica"></asp:BoundColumn>
			<asp:BoundColumn DataField="SedeAttuazione" HeaderText="Sede Attuazione"></asp:BoundColumn>
			<asp:BoundColumn DataField="Codice" HeaderText="Codice Sede Attuazione"></asp:BoundColumn>
			<asp:BoundColumn DataField="Comune" HeaderText="Comune"></asp:BoundColumn>
			<asp:BoundColumn DataField="NumPostiAssegnati" HeaderText="N. Posti Disponibili">
				<HeaderStyle Width="50px"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IdAttivit&#224;" HeaderText="IDProgetto"></asp:BoundColumn>
			<asp:BoundColumn DataField="Segnalazione" HeaderText="Presenza Sanzione">
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="verifica" HeaderText="Presenza Verifica">
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
    </fieldset>
   
</asp:Content>
