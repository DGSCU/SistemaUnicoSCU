<%@ Page  Title="Gestione Modelli Documenti" Language="vb" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" CodeBehind="WfrmEditorModelliDownload.aspx.vb" Inherits="Futuro.WfrmEditorModelliDownload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
        <script type="text/javascript">
            function disableUpload() {
                document.getElementById(CostruisciId("imgConferma")).enabled = false;
            };
 
		</script>
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
   <fieldset class="ContornoPagina" id="fsOperativita" runat="server">
            <legend>Gestione</legend>
            <div class="wrapper" style="width:100%;border:0px">
                <div class="rowGroup" style="height:auto"> 
                    <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:50%">
                            <asp:LinkButton ID="CmdCronologia" runat="server" style="cursor:pointer" Text="Cronologia/Ripristina Modello " Visible="true" ></asp:LinkButton>
                        </div>
                        </div>
                        </div>
                    </div>
        </fieldset>
   <fieldset class="ContornoPagina">
    <legend>Info Documento</legend>
    <div class="wrapper" style="width:100%;border:0px">     
        <div class="row"  >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelNomedoc" CssClass="label" AssociatedControlID="lblNomedoc" runat="server" Text="Nome del Documento in Lavorazione:"></asp:Label>
       </div>
         <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="lblNomedoc" CssClass="labelDati"  runat="server"></asp:Label>
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
    </div>
 </fieldset>
  <div class="wrapper" style="width:100%;border:0px">     
  <div class="row" style="height:auto"> 
   <div class="collable" style="width:49%"  >       
   <fieldset class="ContornoPagina">
    <legend>Download Documento</legend>
       <div class="wrapper" style="width:100%;border:0px">
       <div class="row">
        <div class="collable" style="width:100%">
            <asp:hyperlink  id="urlDownload"  runat="server"  CssClass="linkStampa" Font-Bold="true" Font-Size="Small" Text="Download File" Visible="false" Target="_blank"/> 
        </div>
       </div>
           <div class="RigaPulsanti" > 
            <asp:Button id="imgScarica" CssClass="Pulsante"  runat="server" Text="Download File" ></asp:Button>
             <br />
        </div>
        </div>
    </fieldset>
    </div>
    <div class="collable" style="width:2%"  > &nbsp;
    </div>
    <div class="collable" style="width:49%"  >
   <fieldset class="ContornoPagina">
    <legend>Upload Documento</legend>
        <div class="wrapper" style="width:100%;border:0px">
            <div class="row" >
            <div class="collable" style="width:20%" >
                <asp:label ID="LblSel" CssClass="label" AssociatedControlID="txtSelFile" runat="server" Text="Selezionare il file" ></asp:label>
            </div>
            <div class="colOggetti" style="width:80%">
                <input ID="txtSelFile" type="file" style="width:95%"  runat="server" />
            </div>
            </div>
       
       <div class="RigaPulsanti" >
            <asp:Button  id="cmdUpload" runat="server" CssClass="Pulsante"  Text="Upload File" OnClientClick="disableUpload()" />
        </div>
       </div>
    </fieldset>
    </div>
    </div>
    </div>
    <div class="RigaPulsanti" > 
            <asp:Button  id="CmdEsporta" runat="server" CssClass="Pulsante"   Text="Esporta Elenco Tag" />
              <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
             <br />
         <asp:hyperlink id="ApriCSV1" runat="server" CssClass="linkStampa" ToolTip="Salva Elenco Documenti" Text="DOWNLOAD CSV" Visible="false" ></asp:hyperlink>
        </div>
   </div>

              <br />
        
    <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Elenco Tag"   
         AllowPaging="false" CellPadding="2"  CssClass="table" 	AllowSorting="True" 
         AutoGenerateColumns="False" UseAccessibleHeader="True" Caption="Elenco Tag" Visible="true">
				<FooterStyle></FooterStyle>
				<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
				<EditItemStyle></EditItemStyle>
				<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
				<ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
				<HeaderStyle></HeaderStyle>
		        <Columns>
                  <asp:TemplateColumn Visible="false" HeaderText="Selez."> 
                        <ItemTemplate>
						    <asp:ImageButton ID="imgModello" CommandName="Select" AlternateText="Seleziona Tag" ToolTip="Seleziona Tag" runat="server" ImageURL="images/documento_small.png" CausesValidation="false"></asp:ImageButton>
                        </ItemTemplate>
                 </asp:TemplateColumn>
                 <asp:BoundColumn DataField="gruppo" HeaderText="Gruppo">
                 <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                 </asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdTag" HeaderText="IdTag">
					
				</asp:BoundColumn>
				<asp:BoundColumn DataField="CodiceTag" ReadOnly="True" HeaderText="Codice Tag">
                <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione">
                <ItemStyle HorizontalAlign="Center" Width="70%"></ItemStyle>
                </asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="TipoTag" HeaderText="Tipo"></asp:BoundColumn>
				
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;"   
				PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
   <asp:HiddenField ID="regionecompetenza" runat="server"  />
   <asp:HiddenField ID="area" runat="server"  />
   <asp:HiddenField ID="path" runat="server"  />
   <asp:HiddenField ID="HiddenField4" runat="server"  />
    </fieldset>
</asp:Content>
