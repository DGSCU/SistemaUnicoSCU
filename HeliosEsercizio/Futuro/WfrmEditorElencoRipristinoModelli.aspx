<%@ Page  Title="Cronologia Modello" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmEditorElencoRipristinoModelli.aspx.vb" Inherits="Futuro.WfrmEditorElencoRipristinoModelli" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >
<fieldset class="ContornoPagina">
<legend>Gestione Modello</legend>
     <div class="wrapper" style="width:100%">
      <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="Gestione Modello"></asp:Label>
            </h2>
        </div>
             <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server" ></asp:Label>
                <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
        </div>
        <br />

        <div class="row"  >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelNomedoc" CssClass="label" AssociatedControlID="txtSelFile" runat="server" Text="Nome del Documento in Lavorazione:"></asp:Label>
       </div>
         <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="txtSelFile" CssClass="labelDati"  runat="server"></asp:Label>
       </div>

       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelNomelogico" CssClass="label" AssociatedControlID="lblnomelogico" runat="server" Text="Nome Logico"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="lblnomelogico" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
       </div>
        <div class="row" >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelDescrizione" CssClass="label" AssociatedControlID="lbldescrizione"  runat="server" Text="Descrizione"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="lbldescrizione" CssClass="labelDati" runat="server"></asp:Label>
       </div>

       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelUltimamodifica" CssClass="label" AssociatedControlID="lblUltimamodifica" runat="server" Text="Ultima Modifica"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="lblUltimamodifica" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
       </div>
        <div class="row">
            <div class="collable" style="width:15%"  >
            <asp:Label ID="labelUserame" CssClass="label" AssociatedControlID="lblUsername"  runat="server" Text="Username"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">
            <asp:Label ID="lblUsername" CssClass="labelDati" runat="server"></asp:Label>
            </div>
         </div>
       <div class="row">
        <div class="collable" style="width:100%">
            <asp:hyperlink  id="urlDownload"  runat="server"  CssClass="linkStampa" Font-Bold="true" Font-Size="Small" ToolTip="Download File" Visible="false" Target="_blank"/> 
        </div>
       </div>

            <div class="RigaPulsanti" > 
        <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
    </div>
    </div>     
    <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Cronologia Documento"   
         AllowPaging="true" CellPadding="2"  CssClass="table" 	AllowSorting="True"  PageSize="10" 
         AutoGenerateColumns="False" UseAccessibleHeader="True" Caption="Cronologia Documento" Visible="true">
				<FooterStyle></FooterStyle>
				<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
				<EditItemStyle></EditItemStyle>
				<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
				<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
				<HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:BoundColumn Visible="False" DataField="idCronologiaModello" HeaderText="idCronologiaModello">
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="IdModelloCompetenza" HeaderText="IdModelloCompetenza">
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="IdArea" HeaderText="IdArea">
				    </asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="NomeLogico" HeaderText="Nome Logico">
					</asp:BoundColumn>
					<asp:BoundColumn DataField="NomeFisico" HeaderText="Nome Fisico">
					</asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="Path" HeaderText="Path">
					</asp:BoundColumn>
					<asp:BoundColumn DataField="UsernameProprietario" HeaderText="Username Proprietario">
					</asp:BoundColumn>
					<asp:BoundColumn DataField="DataCreazione" HeaderText="Data Creazione">
                    </asp:BoundColumn>
					<asp:BoundColumn DataField="DataStoricizzazione" HeaderText="Data Storicizzazione">
                    </asp:BoundColumn>
                    <asp:TemplateColumn Visible="False"  HeaderText="Ripristina"> 
                        <ItemTemplate>
						    <asp:ImageButton ID="ImgRipristina" style="cursor:pointer;"  CommandName="Select" AlternateText="Ripristina File" ToolTip="Ripristina File" runat="server" ImageURL="images/cronologia_small.png" CausesValidation="false"></asp:ImageButton>
                        </ItemTemplate>
                 </asp:TemplateColumn>
                          <asp:TemplateColumn  HeaderText="Visualizza"> 
                        <ItemTemplate>
						    <asp:ImageButton ID="ImgVisualizza"   CommandName="Documento" ImageUrl="images/documento_small.png" AlternateText="Visualizza File" ToolTip="Crea link per visualizzare il file" runat="server"   causesValidation="false"></asp:ImageButton>
                        </ItemTemplate>
                 </asp:TemplateColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;"   
				PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
   <asp:HiddenField ID="regionecompetenza" runat="server"  />
   <asp:HiddenField ID="area" runat="server"  />
   <asp:HiddenField ID="path" runat="server"  />
   <asp:HiddenField ID="IdRegioneCompetenza" runat="server"  />
      <asp:HiddenField ID="txtIdModello" runat="server" />
    </fieldset>
</asp:Content>
