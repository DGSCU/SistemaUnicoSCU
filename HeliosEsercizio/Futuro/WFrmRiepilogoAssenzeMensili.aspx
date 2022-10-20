<%@ Page Title="Riepilogo Assenze Mensili" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WFrmRiepilogoAssenzeMensili.aspx.vb" Inherits="Futuro.WFrmRiepilogoAssenzeMensili" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <fieldset class="ContornoPagina">
        <legend>Riepilogo Assenze DGSCN</legend>
        <div class="wrapper" style="width:99%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Progetti"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" >      
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="txtDenominazione" runat="server" Text="Ente"></asp:Label>
                </div>
                <div class="colOggetti" >       
                    <asp:TextBox ID="txtDenominazione" CssClass="textbox" autofocus="true" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" >      
                    <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtCodEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" >       
                    <asp:TextBox ID="txtCodEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" >      
                    <asp:Label ID="lblMesiRitardo" CssClass="label" AssociatedControlID="ddlmesiRitardo" runat="server" Text="Mesi Ritardo"></asp:Label>
                </div>
                <div class="colOggetti" >       
                     <asp:DropDownList ID="ddlmesiRitardo" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
            </div>
             <div class="RigaPulsanti">
                <asp:Button ID="cmdSalva" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
        </div>
         <br />
        <h3>
        <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgRisultatoRicerca"></asp:label>
    </h3>
        <br />
        <asp:datagrid id="dgRisultatoRicerca" runat="server"  ToolTip="Elenco Progetti" CssClass="table" Width="99%" CellPadding="2"  AllowPaging="True" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
		<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
			<asp:ButtonColumn Text="&lt;img src=images/statoente.jpg Width=20 Height=20 title='Seleziona Progetto' border=0&gt;" CommandName="Select" Visible="false"></asp:ButtonColumn>
			<asp:BoundColumn DataField="CodiceRegione" HeaderText="Cod. Ente"></asp:BoundColumn>
			<asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione"></asp:BoundColumn>
			<asp:BoundColumn DataField="Telefono" HeaderText="Telefono"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="Email" HeaderText="Email"></asp:BoundColumn>
			<asp:BoundColumn DataField="DataPartenza" HeaderText="Data Partenza"></asp:BoundColumn>
			<asp:BoundColumn DataField="DataUltimaConferma" HeaderText="Data Ultima Conferma">
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="UltimoMeseConfermato" HeaderText="Ultimo Mese Confermato"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="idente" HeaderText="Idente"></asp:BoundColumn>
			<asp:BoundColumn DataField="MesinonConfermati" HeaderText="Mesi non Confermati">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="volontarinonpagabili" HeaderText="Vol. non Pagabili">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="volontaripagabili" HeaderText="Vol. Pagabili">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo" Visible="False">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			</asp:BoundColumn>
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
        <br />
    </fieldset>
 
</asp:Content>
