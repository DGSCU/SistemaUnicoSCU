<%@ Page Title="Assegnazione Vincoli Progetto" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="assegnazionevincoliprogetti.aspx.vb" Inherits="Futuro.assegnazionevincoliprogetti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
    function AlertSalvataggioDati() {
        /*var pulsanteSalvaVisibile = '<%= imgSalva.Visible  %>';
        if (pulsanteSalvaVisibile == 'True') {
        
            return confirm('Prima di essere reindirizzati alla pagina richiesta tutti i dati verranno salvati. CONTINUARE?');
        }*/
        return true;
    };
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPaginaScroll" style="width:auto;overflow:auto">
<legend >Valutazione Formale Progetto</legend>
<div class="firefox">
<div class="wrapper" style="width:100%;border:0px">
<div class="headers" >
    <h2>
    <asp:Label ID="LabelTitoloMaschera" runat="server"  Text="Valutazione Formale Progetto"></asp:Label>
    </h2>
</div>
    <div class="row" style="height:auto">
        <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
        <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
        <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>      
    </div>
  <br />
 <fieldset class="ContornoPagina" >
 <legend >Info Progetto</legend>
 <div class="wrapper" style="width:100%;border:0px">
    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="labelTitolo" CssClass="label" runat="server" AssociatedControlID="lblTitolo" Text="Titolo"></asp:Label>       
        </div>
        <div class="colOggetti" style="width:85%">
            <asp:Label ID="lblTitolo" CssClass="labelDati" Runat="server"></asp:Label>
        </div>
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">
            <asp:Label ID="labelCodiceProgetto" CssClass="label"   runat="server" AssociatedControlID="lblCodiceProgetto" Text="Codice Progetto"></asp:Label>       
        </div>
        <div class="colOggetti" style="width:85%">
            <asp:Label ID="lblCodiceProgetto" CssClass="labelDati" Runat="server"></asp:Label>
        </div>
    </div>
    </div>
  </fieldset>
 <fieldset class="ContornoPagina"  id="fsOperativita" runat="server">
            <legend>Operatività Progetto</legend>
            <div class="wrapper" style="width:100%;border:0px">
                <div class="rowGroup" style="height:auto"> 
                    <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="Prog" runat="server" style="cursor:pointer" Text="Visualizza Progetto" Visible="true"  OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
                        </div>
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="hlRicercaSedi" runat="server" style="cursor:pointer" Text="Visualizza Sedi" Visible="true" OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
                        </div>
                        </div>
                           <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="hlRicercaRisorse" runat="server" style="cursor:pointer" Text="Visualizza Risorse" Visible="true" OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
                        </div>
                     <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="imgElencoDocumentiProg" runat="server" style="cursor:pointer" Text="Visualizza Documenti Progetto" Visible="true" OnClientClick="return AlertSalvataggioDati();"></asp:LinkButton>
                        </div>
                        </div>
                        <div class="row" style="height:auto">                  
                           <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="imgAssociaDocumentiProg" runat="server" style="cursor:pointer" Text="Associa Documenti Progetto" Visible="false" ></asp:LinkButton>
                        </div>
                        </div>
                        </div>
                    </div>
        </fieldset>
  <br />
 <fieldset class="ContornoPagina" >
 <legend>Cause di inammissibilità del progetto...</legend>
    <asp:datagrid id="dtgVincoliProgetti"  runat="server" Width="100%"  AutoGenerateColumns="False" CssClass="table" 
                    AllowSorting="false"  CellPadding="3" UseAccessibleHeader="true" BorderStyle="Solid"  AllowPaging="false"
                    Caption="Inammissibilità">
			<FooterStyle ></FooterStyle>
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
			<EditItemStyle ></EditItemStyle>
			<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr"></ItemStyle>
			<HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="False" DataField="IdVincolo" HeaderText="IdVincolo"></asp:BoundColumn>
				<asp:BoundColumn DataField="Vincolo" HeaderText="Vincolo">
					<ItemStyle Wrap="true" Width="40%"></ItemStyle>
					<FooterStyle Wrap="True"></FooterStyle>
				</asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Esito">
					<HeaderStyle Wrap="True" HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle Wrap="True" Width="20%"></ItemStyle>
					<ItemTemplate>
                        <div class="collable" style="width:20%" >
                        <asp:Label ID="lblEsito" CssClass="label" runat="server" AssociatedControlID="ddlEsito" Text="Esito"></asp:Label>
                        </div>
                         <div class="colOggetti" style="width:80%">
						<asp:DropDownList ID="ddlEsito" CssClass="ddlClass"  Runat="server">
							<asp:ListItem Value="1">Positivo</asp:ListItem>
							<asp:ListItem Value="0">Negativo</asp:ListItem>
						</asp:DropDownList>
                    </div>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="Valore" HeaderText="Valore"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Note">
                <HeaderStyle Wrap="True" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Wrap="True" Width="40%"></ItemStyle>
					<ItemTemplate>
                    <div class="collable" style="width:15%" >
                        <asp:Label ID="lblNote" CssClass="label" runat="server" AssociatedControlID="txtNote" Text="Nota"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:85%">
						<asp:TextBox id="txtNote" runat="server" Rows="2" TextMode="MultiLine" CssClass="textbox"></asp:TextBox>
                        </div>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="ValoreNote" HeaderText="ValoreNote"></asp:BoundColumn>
			</Columns>
		</asp:datagrid>
  </fieldset>
 <fieldset class="ContornoPagina" >
 <legend >Cause di esclusione del progetto...</legend>
    <asp:datagrid id="dtgVincoliProgetti2" runat="server" Width="100%"  AutoGenerateColumns="False" CssClass="table" 
                    AllowSorting="false"  CellPadding="3" UseAccessibleHeader="true" BorderStyle="Solid"  AllowPaging="false"
                    Caption="Esclusione">
			<FooterStyle ></FooterStyle>
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
			<EditItemStyle ></EditItemStyle>
			<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
			<ItemStyle CssClass="tr"></ItemStyle>
			<HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="False" DataField="IdVincolo" HeaderText="IdVincolo"></asp:BoundColumn>

				<asp:BoundColumn DataField="Vincolo" HeaderText="Vincolo">
                	<ItemStyle Wrap="true" Width="40%"></ItemStyle>
				</asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Esito">
					<HeaderStyle Wrap="True" HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle Wrap="True" Width="20%"></ItemStyle>
					<ItemTemplate>
                        <div class="collable" style="width:20%" >
                        <asp:Label ID="lblEsito" AssociatedControlID="ddlEsito2" runat="server" Text="Esito"></asp:Label>
                        </div>
                         <div class="colOggetti" style="width:80%">
                       <asp:DropDownList ID="ddlEsito2" CssClass="ddlClass"  Runat="server">
							<asp:ListItem Value="1">Positivo</asp:ListItem>
							<asp:ListItem Value="0">Negativo</asp:ListItem>
						</asp:DropDownList>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="Valore" HeaderText="Valore"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Note">
                <HeaderStyle Wrap="True" HorizontalAlign="Center"></HeaderStyle>
                <ItemStyle Wrap="True" Width="40%"></ItemStyle>
					<ItemTemplate>
                    <div class="collable" style="width:15%" >
                        <asp:Label ID="lblNote2" runat="server" AssociatedControlID="txtNote2" Text="Nota"></asp:Label>
                          </div>
                        <div class="colOggetti" style="width:85%">
						<asp:TextBox id="txtNote2" runat="server" Rows="2" CssClass="textbox" TextMode="MultiLine" MaxLength="100"></asp:TextBox>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="ValoreNote" HeaderText="ValoreNote"></asp:BoundColumn>
			</Columns>
		</asp:datagrid>
  </fieldset>
 <div class="RigaPulsanti">
        <asp:Button  ID="imgAccetta" runat="server" CssClass="Pulsante" 
            Text="Ammissibile" />
        <asp:Button ID="imgAccettaConLimitazioni" CssClass="Pulsante" runat="server" 
            Text="Ammissibile Con Limitazioni"  />
        <asp:Button  ID="imgNonAmmissibile" runat="server" CssClass="Pulsante" 
            Text="Non Ammissibile" />
        <asp:Button  ID="imgEscluso" runat="server" CssClass="Pulsante" 
            Text="Escluso" />
        <asp:Button  ID="ImgRipristina" runat="server" CssClass="Pulsante" Text="Ripristina" Visible="False" />
  
   </div>
    <div class="RigaPulsanti">
             <asp:Button  ID="imgSalva" runat="server" CssClass="Pulsante" Text="Salva" />
        <asp:Button ID="imgChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
    </div>
    
   
</div>

</div>

</fieldset>
<asp:HiddenField id="txtidprogetto" runat="server"/>
<asp:HiddenField id="TxtTipoprog1" runat="server"/>
</asp:Content>
