<%@ Page Title="Consultazione Documenti Ente" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmConsultaDocumenti.aspx.vb" Inherits="Futuro.wfrmConsultaDocumenti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/JHelper.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend>Documenti</legend>
     <div class="wrapper" style="width:100%">
      <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="Documenti Ente"></asp:Label>
            </h2>
        </div>
        <div class="RigaVuota" >&nbsp;
        </div>
     <div class="row" style="height:auto">
             <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
                 <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>

        </div>
    </div>
    <div class="row" >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labellblEnte" CssClass="label" AssociatedControlID="lblEnte" runat="server" Text="Codice Ente"></asp:Label>
       </div>
         <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="lblEnte" CssClass="labelDati"  runat="server"></asp:Label>
       </div> 
       </div>
    <div class="row" >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labellblDenominazioneEnte" CssClass="label" AssociatedControlID="lblDenominazioneEnte" runat="server" Text="Denominazione Ente"></asp:Label>
       </div>
       <div class="colOggetti" style="width:85%"  >
              <asp:Label ID="lblDenominazioneEnte" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" >
         <div class="collable" style="width:15%" >
            <asp:label ID="labelRifFase" CssClass="label" AssociatedControlID="LblRifFase" runat="server" Text="Riferimento Fase" ></asp:label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:Label ID="LblRifFase" CssClass="labelDati"  runat="server"></asp:Label>
        </div>
             <div class="collable" style="width:15%" >
            <asp:label ID="labelLblFase" CssClass="label" AssociatedControlID="LblFase" runat="server" Text="Fase" ></asp:label>
        </div>
        <div class="colOggetti" style="width:35%">
                 <asp:Label ID="LblFase" CssClass="labelDati"  runat="server"></asp:Label>
        </div>
       </div>
          <div class="row" >
         <div class="collable" style="width:15%" >
            <asp:label ID="label1" CssClass="label" AssociatedControlID="ddlTipoDoc" runat="server" Text="Tipo Documento" ></asp:label>
        </div>
        <div class="colOggetti" style="width:85%">
            <asp:DropDownList ID="ddlTipoDoc" CssClass="ddlClass"   runat="server"></asp:DropDownList>
        </div>

        </div>
        <div class="row" >
             <div class="collable" style="width:15%" >
                <asp:label ID="lblNomeFile" CssClass="label" AssociatedControlID="txtNomeFile" runat="server" Text="Nome File" ></asp:label>
            </div>
            <div class="colOggetti" style="width:85%">
                 <asp:TextBox ID="txtNomeFile" CssClass="textbox" runat="server" ></asp:TextBox>    
            </div>
        </div>
        <div class="row" >
             <div class="collable" style="width:15%" >
                <asp:label ID="lblSoggetto" CssClass="label" AssociatedControlID="txtSoggetto" runat="server" Text="Ente/Risorsa" ></asp:label>
            </div>
            <div class="colOggetti" style="width:85%">
                 <asp:TextBox ID="txtSoggetto" CssClass="textbox" runat="server" ></asp:TextBox>    
            </div>
        </div>
        <div class="row" id="divStatoDocumento" runat="server">
             <div class="collable" style="width:15%" >
                <asp:label ID="label2" CssClass="lblStatoDocumento" AssociatedControlID="ddlStatoDocumento" runat="server" Text="Stato Documento" ></asp:label>
            </div>
            <div class="colOggetti" style="width:85%">
                <asp:DropDownList ID="ddlStatoDocumento" CssClass="ddlClass"   runat="server">
                    <asp:ListItem Value="">TUTTI</asp:ListItem>
				    <asp:ListItem Value="0">Da Validare</asp:ListItem>
				    <asp:ListItem Value="1">Validato</asp:ListItem>
                    <asp:ListItem Value="2">Non Valido</asp:ListItem>
								
                </asp:DropDownList>
            </div>
        </div>
     <div class="RigaPulsanti" > 
       <asp:Button id="cmdRicerca" CssClass="Pulsante"  runat="server" Text="Ricerca" ></asp:Button>
    <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
</div>
    </div>
    <div class="RigaVuota" >&nbsp;
    </div>

    <asp:datagrid id="dtgConsultaDocumenti" runat="server" Width="100%"  Caption="Elenco Documenti" AllowPaging="True" 
                    CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="10" 
                    AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
	    <Columns>
            <asp:TemplateColumn HeaderText="Download"> 
                <ItemTemplate>
				    <asp:ImageButton ID="ImageButton1" style="cursor:pointer;" CommandName="Download" AlternateText="Scarica Documento" ToolTip='Scarica Documento' runat="server" ImageURL="images/giu_small.png" CausesValidation="false"></asp:ImageButton>      
                    </ItemTemplate>
            </asp:TemplateColumn>
		    <asp:BoundColumn Visible="False" DataField="IdEnteDocumento"/>
		    <asp:BoundColumn DataField="FileName" HeaderText="Nome File"/>
	        <asp:BoundColumn DataField="Soggetto" HeaderText="Ente/Risorsa"/>
		    <asp:BoundColumn DataField="DataInserimento" HeaderText="Data Inserimento"/>
		    <asp:BoundColumn DataField="hashvalue" HeaderText="Hash Value"/>
            <asp:BoundColumn DataField="TipoFase" HeaderText="Fase" Visible="false"/>
	        <asp:BoundColumn Visible="False" DataField="TipologiaDocumento" HeaderText="Tipo Documento"/>
            <asp:BoundColumn DataField="StatoDocumento" HeaderText="Stato Documento"/>
            <asp:TemplateColumn  Visible="False" HeaderText="Valido"> 
                <ItemTemplate>
				    <asp:ImageButton ID="imgValida"  CommandName="Valida" AlternateText="Documento Valido" 
                        ToolTip="Valida Documento" runat="server" ImageURL="Images/selezionato_small.png" Visible='<%# IIF(Eval("PulsanteConfermaValida").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>  
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="False" HeaderText="Non Valido"> 
                <ItemTemplate>
			    	<asp:ImageButton ID="imgNonValida"  CommandName="NonValida" AlternateText="Documento non Validato" 
                        ToolTip="Documento Non Valido" runat="server" ImageURL="Images/deselezionato_small.png" Visible='<%# IIF(Eval("PulsanteConfermaNONValida").ToString().Equals("NO"), False, True) %>'></asp:ImageButton>  
                </ItemTemplate>
            </asp:TemplateColumn>
	    </Columns>
           	<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
    </asp:datagrid>

<div class="wrapper" style="width:100%;border:0px">
    <div class="RigaVuota" >&nbsp;
    </div>
   <div class="rowGroup" runat="server" style="height:auto" Id="divDownloadFile" visible="false">
    <div class="RigaVuota" >&nbsp;
    </div>
    <div class="row" >
          <div class="colHyperLink" style="width:100%;" >
            <asp:hyperlink  id="hlScarica"  runat="server" Visible="false" Target="_blank"/>
        </div>
    </div>
</div>
</div>
    </fieldset>
</asp:Content>

