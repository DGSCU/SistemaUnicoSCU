<%@ Page Title="Log Fascicoli Volontari" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmLogFascicoliVolontari.aspx.vb" Inherits="Futuro.WfrmLogFascicoliVolontari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Log Fascicoli Volontari</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Log Fascicoli Volontari"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:150px">
                    <asp:RadioButton ID="RadioButton1" Text="Solo Negativi"  GroupName="rd" 
                        runat="server" AutoPostBack="True" Checked="True"/>
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:150px">
                    <asp:RadioButton ID="RadioButton2" Text="Tutte"  GroupName="rd" runat="server" 
                        AutoPostBack="True"/>
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Label ID="lblrieseguilog" CssClass="label" AssociatedControlID="TxtNumLog" runat="server" Text="Riesegui prime N°"></asp:Label>&nbsp;
                <asp:textbox ID="TxtNumLog" runat="server" CssClass="textbox" Width="50px" 
                    Text="5" MaxLength="6"></asp:textbox>&nbsp;
                <asp:Button ID="cmdRieseguiLog" CssClass="Pulsante" runat="server" Text="Elabora" />&nbsp;
            </div>
        </div>
        <br />
        <asp:datagrid id="dtgEventi" runat="server" CssClass="table"  Width="100%" 
            CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" 
            AllowPaging="True" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White" Font-Bold="true"></SelectedItemStyle>
            <EditItemStyle></EditItemStyle>
            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn HeaderText="Sel"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImageButton1" CommandName="Select" ToolTip="Seleziona Dettaglio" AlternateText="Seleziona Dettaglio" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IdLogFascicoliVolontari" HeaderText="Id"></asp:BoundColumn>
		        <asp:BoundColumn DataField="username" HeaderText="User Name"></asp:BoundColumn>
		        <asp:BoundColumn DataField="metodo" HeaderText="Metodo"></asp:BoundColumn>
		        <asp:BoundColumn Visible="False" DataField="identit&#224;" HeaderText="Id Volontario"></asp:BoundColumn>
		        <asp:BoundColumn DataField="Dataorarichiesta" HeaderText="Data Ora Richiesta"></asp:BoundColumn>
		        <asp:BoundColumn DataField="dataoraesecuzione" HeaderText="Data Ora Esecuzione"></asp:BoundColumn>
		        <asp:BoundColumn DataField="IdEntità" HeaderText="IdEntità">
                </asp:BoundColumn>
		        <asp:BoundColumn DataField="eseguito" HeaderText="Eseguito"></asp:BoundColumn>
			</Columns>
            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="CmdEsportaEventi" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		    <br />
	        <asp:HyperLink ID="ApriCSV1Eventi" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
            &nbsp;
        </div>
        <asp:datagrid id="DtgDettagli" runat="server" CssClass="table"  Width="100%" CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
            <EditItemStyle></EditItemStyle>
            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
            <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
            <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="username" HeaderText="User Name"></asp:BoundColumn>
				<asp:BoundColumn DataField="metodo" HeaderText="Metodo"></asp:BoundColumn>
				<asp:BoundColumn DataField="parametri" HeaderText="Parametri">
					<HeaderStyle Width="300px"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="dataorarichiesta" HeaderText="Data Ora Richiesta"></asp:BoundColumn>
				<asp:BoundColumn DataField="dataoraesecuzione" HeaderText="Data Ora Esecuzione"></asp:BoundColumn>
				<asp:BoundColumn DataField="eseguito" HeaderText="Eseguito"></asp:BoundColumn>
			</Columns>
		</asp:datagrid>
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="CmdEsportaDettagli" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		    <br />
	        <asp:HyperLink ID="ApriCSV1Dettagli" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
            &nbsp;
        </div>
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
        </div>
    </fieldset>
</asp:Content>
