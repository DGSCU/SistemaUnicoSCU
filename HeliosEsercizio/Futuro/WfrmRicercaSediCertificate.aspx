<%@ Page Title="Gestione Iscrizione Sedi Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaSediCertificate.aspx.vb" Inherits="Futuro.WfrmRicercaSediCertificate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script  type="text/javascript">

        var prefissoIdClient = "MainContent_";

        function CostruisciId(IdServer) {
            var IdClient = prefissoIdClient + IdServer;
            return IdClient;
        };


        function TogliCheck() {

            var idchkSelDesel = CostruisciId("chkSelDesel");
            var chkSelDesel = document.getElementById(idchkSelDesel);

            chkSelDesel.checked = false;

        }
	
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div class="firefox">
    <fieldset class="ContornoPaginaScroll">
        <legend>Gestione Iscrizione Sedi Progetto</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Gestione Iscrizione Sedi Progetto"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:85%">       
                    <asp:TextBox ID="txtdenominazione" CssClass="textbox" runat="server" Width="95%" MaxLength="200"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRegione" CssClass="label" AssociatedControlID="txtregione" runat="server" Text="Regione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtregione" CssClass="textbox" runat="server" MaxLength="100"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="txtComune" runat="server" Text="Comune"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtComune" CssClass="textbox" runat="server" MaxLength="100"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProvincia" CssClass="label" AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtProvincia" CssClass="textbox" runat="server" MaxLength="100"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblPresenzaCertificazione" CssClass="label" AssociatedControlID="ddlCertificazione" runat="server" Text="Presenza Iscrizione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlCertificazione" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblIndirizzo" CssClass="label" AssociatedControlID="txtIndirizzo" runat="server" Text="Indirizzo"></asp:Label>
                </div>
                <div class="colOggetti" style="width:85%">       
                    <asp:TextBox ID="txtIndirizzo" CssClass="textbox" runat="server" Width="95%" MaxLength="200"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodSedeAtt" CssClass="label" AssociatedControlID="txtCodSedeAtt" runat="server" Text="Cod. Sede"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodSedeAtt" CssClass="textbox" runat="server" MaxLength="8"></asp:TextBox>         
                </div>
                <asp:PlaceHolder ID="phCodiceEnte" runat="Server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodEnte" CssClass="label" AssociatedControlID="txtCodiceEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceEnte" CssClass="textbox" runat="server" MaxLength="100"></asp:TextBox>         
                </div>
                </asp:PlaceHolder>
            </div>
            <asp:PlaceHolder ID="phDenominazioneEnte" runat="server">
            <div class="row" > 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenomEnte" CssClass="label" AssociatedControlID="TxtDenomEnte" runat="server" Text="Denominazione Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:85%">       
                    <asp:TextBox ID="TxtDenomEnte" CssClass="textbox" runat="server" Width="95%" MaxLength="100"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCboCompetenza" CssClass="label" AssociatedControlID="CboCompetenza" runat="server" Text="Competenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="CboCompetenza" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlstato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlstato" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
            </div>
            </asp:PlaceHolder>
            <div class="row" >
                <asp:PlaceHolder ID="phPresenzaSanzione" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblPresenzaSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlSegnalazioneSanzione" runat="server" CssClass="ddlClass">
                        <asp:ListItem Selected="True">Tutti</asp:ListItem>
						<asp:ListItem Value="0">No</asp:ListItem>
						<asp:ListItem Value="1">Si</asp:ListItem>
                    </asp:DropDownList>         
                </div>
                </asp:PlaceHolder>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblRifFase" CssClass="label" AssociatedControlID="txtIDFaseEnte" runat="server" Text="Rif. Fase"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtIDFaseEnte" CssClass="textbox" runat="server" MaxLength="10"></asp:TextBox>         
                </div>     
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;
            </div>
             <br />
            <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore"></asp:label>
            <br />
            <br />
            <div class="row">
                <div class="colOggetti" style="width:50%">
                    <asp:checkbox id="chkSelDesel" runat="server" Text="Seleziona tutto" Visible="false" AutoPostBack="true"></asp:checkbox>
                </div>
            </div>

            <div class="RigaPulsanti" style="text-align:right">
                <asp:Button ID="cmdSalva" CssClass="Pulsante" runat="server" Text="Conferma" Visible="false"/>
            </div>

        </div>
       
        <div class="RigaVuota" >&nbsp;</div>
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Sedi" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:ButtonColumn Text="" Visible="false"></asp:ButtonColumn>
				<asp:BoundColumn DataField="Ente" HeaderText="Ente"></asp:BoundColumn>
				<asp:BoundColumn DataField="sedeFisica" HeaderText="Sede Fisica">
					<HeaderStyle Width="200px"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo">
					<HeaderStyle Width="200px"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Comune" HeaderText="Comune ">
					<HeaderStyle Width="30px"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="telefono" HeaderText="telefono"></asp:BoundColumn>
				<asp:BoundColumn DataField="sedeattuazione" HeaderText="Sede Attuazione">
					<HeaderStyle HorizontalAlign="Center" Width="200px" VerticalAlign="Middle"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="statoentesede" HeaderText="Stato"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idEntesede" HeaderText="idSede"></asp:BoundColumn>
				<asp:BoundColumn DataField="idEntesedeattuazione" HeaderText="Codice Sede"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idatt" HeaderText="idAttivitaES"></asp:BoundColumn>
				<asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Certificazione" HeaderText="Iscr"></asp:BoundColumn>
				<asp:TemplateColumn>
					<HeaderStyle HorizontalAlign="Center" Width="0%"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate>
				        <asp:CheckBox id="chkSedi" runat="server" checked='<%# DataBinder.Eval(Container, "DataItem.riccheck") %>' Text="&nbsp;"></asp:CheckBox>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="riccheck" HeaderText="ricchek"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="CheckCert"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idente"></asp:BoundColumn>
				<asp:BoundColumn DataField="codificaCert" HeaderText="Presenza Iscrizione">
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Presenza Sanzione">
	                <ItemTemplate>
		                <asp:Label ID="lblPresenzaSanzione" runat="server"></asp:Label>   
		                <asp:ImageButton ID="ImgVisualizzaSanzione" CommandName="VisualizzaSanzione" ToolTip="Visualizza Sanzione" AlternateText="Visualizza Sanzione" runat="server" ImageURL="~/Images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton> 
	                </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn DataField="verifica" HeaderText="Presenza Verifica">
					<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="IdEnteFase" HeaderText="Rif. Fase"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <asp:HiddenField ID="hdd_Check" runat="server" />
         
     </fieldset>
</div>


</asp:Content>
