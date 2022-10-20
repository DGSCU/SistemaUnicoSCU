<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmProgrammidaValutare.aspx.vb" Inherits="Futuro.WfrmProgrammidaValutare" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="firefox">
<fieldset class="ContornoPaginaScroll">
        <legend>Ricerca Programmi da Valutare</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Programmi Da Valutare"></asp:Label></h2>
            </div>
              <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
                <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
            </div>

            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenEnte" CssClass="label" AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Den. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtDenominazioneEnte"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="txtCodEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodEnte" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTitoloProgetto"  AssociatedControlID="txtTitoloProgetto" 
                         Text="Programma" runat="server"  CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtTitoloProgetto"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblbando" CssClass="label" AssociatedControlID="txtbando" runat="server" Text="Bando"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="txtbando"  runat="server" CssClass="ddlClass"></asp:DropDownList>        
                </div>
            </div>
              <%-- <div  class="row">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblSettore" CssClass="label" AssociatedControlID="ddlMaccCodAmAtt" runat="server" Text="Settore" ></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlMaccCodAmAtt"  runat="server" CssClass="ddlClass" AutoPostBack="True"></asp:DropDownList>       
                </div>
               <div class="collable" style="width:15%">      
                    <asp:Label ID="lblddlCodAmAtt" CssClass="label" AssociatedControlID="ddlCodAmAtt" runat="server" Text="Area Intervento"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlCodAmAtt"  runat="server" CssClass="ddlClass"></asp:DropDownList>       
                </div>
            </div>--%>

            <div  class="row">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblddlStatoAttivita" CssClass="label" 
                         AssociatedControlID="ddlStatoAttivita" runat="server" Text="Stato Programma"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlStatoAttivita"  runat="server" CssClass="ddlClass"></asp:DropDownList>       
                </div>
               <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCboCompetenza" CssClass="label" AssociatedControlID="CboCompetenza" runat="server" Text="Competenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="CboCompetenza"  runat="server" CssClass="ddlClass"></asp:DropDownList>       
                </div>
            </div>
      <div  class="row">
      <div class="collable" style="width:15%">      
                    <asp:Label ID="LblStatoValutazione" CssClass="label" AssociatedControlID="ddlStatoValutazione" Visible="False" runat="server" Text="Stato Valutazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlStatoValutazione"  runat="server" CssClass="ddlClass" Visible="False">
                    	<asp:ListItem Selected="True">Tutti</asp:ListItem>
								<asp:ListItem Value="0">Non Definito</asp:ListItem>
								<asp:ListItem Value="1">Istruttoria</asp:ListItem>
								<asp:ListItem Value="2">Sottoposto alla Commissione</asp:ListItem>
                    </asp:DropDownList>       
                </div>
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione" Visible="false"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlSegnalazioneSanzione"  runat="server" CssClass="ddlClass" Visible="false">
                    <asp:ListItem Selected="True">Tutti</asp:ListItem>
								<asp:ListItem Value="0">No</asp:ListItem>
								<asp:ListItem Value="1">Si</asp:ListItem>
                    </asp:DropDownList>       
                </div>
               
            </div>

            <div class="RigaPulsanti">
            
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
    </div>
        <br />
            <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Risultato Ricerca Programmi"
                    ToolTip="Elenco Programmi" CssClass="table"  AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False"
                    UseAccessibleHeader="True">
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:TemplateColumn HeaderText="Selez."> 
                        <ItemTemplate>
	                        <asp:ImageButton ID="IdImgSelSedi" style="cursor:pointer;" CommandName="Select" AlternateText="Gestione Vincoli Programma" ToolTip="Gestione Vincoli" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
                         <asp:TemplateColumn Visible="False" HeaderText="Accett."> 
                        <ItemTemplate>
	                        <asp:ImageButton  ID="IdImgAccSedi" style="cursor:pointer;" CommandName="accettazione" AlternateText="Accettazione Programma" ToolTip='Accettazione' runat="server" ImageURL="images/valida_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
                         <asp:TemplateColumn Visible="False" HeaderText="Valut."> 
                        <ItemTemplate>
	                        <asp:ImageButton ID="IdImgValSedi" style="cursor:pointer;" CommandName="ValutazioneQual" AlternateText="Gestione Valutazione" ToolTip="Gestione Valutazione Programmi" runat="server" ImageURL="images/documento_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
				<asp:BoundColumn DataField="denominazione" HeaderText="Denominazione"></asp:BoundColumn>
					<asp:BoundColumn DataField="titolo" HeaderText="Titolo"></asp:BoundColumn>
					<asp:BoundColumn DataField="CodiceRegione" HeaderText="Cod. Ente"></asp:BoundColumn>
					<asp:BoundColumn DataField="bando" HeaderText="Bando"></asp:BoundColumn>
					<asp:BoundColumn DataField="data" HeaderText="Data Presentazione"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idprogramma" 
                        HeaderText="idprogramma"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idente" HeaderText="Idente"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idtipoprogramma" 
                        HeaderText="tipoProgramma"></asp:BoundColumn>
					<asp:BoundColumn DataField="Effettivi" HeaderText="Posti Effettivi">
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Richiesti" HeaderText="Posti Richiesti">
					</asp:BoundColumn>
				</Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	    </asp:datagrid>
       <asp:HiddenField ID="ddlMaccCodAmAtt1" runat="server" />
       <asp:HiddenField ID="Textbox2" runat="server" />
       <asp:HiddenField ID="txtTitoloProgetto1" runat="server" />
       <asp:HiddenField ID="txtbando1" runat="server" />
       <asp:HiddenField ID="txtDenominazioneEnte1" runat="server" />
       <asp:HiddenField ID="ddlCodAmAtt1" runat="server" />
       <asp:HiddenField ID="ddlStatoAttivita1" runat="server" />
       
       
    </fieldset>
    </div>
</asp:Content>