<%@ Page Title="Ricerca Progetti Per Valutazione Qualit&#224;" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmProgettidaValQualita.aspx.vb" Inherits="Futuro.WfrmProgettidaValQualita" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="firefox">
<fieldset class="ContornoPaginaScroll">
        <legend>Ricerca Progetti da Valutare</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Progetti Per Valutazione Qualit&#224;"></asp:Label></h2>
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
                    <asp:Label ID="lblTitoloProgetto"  AssociatedControlID="txtTitoloProgetto" Text="Progetto" runat="server"  CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtTitoloProgetto"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="LblCodiceProgetto"  AssociatedControlID="txtCodiceProgetto" Text="Codice Progetto" runat="server"  CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceProgetto"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                 </div>
                  <div class="row">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblProgramma"  AssociatedControlID="txtTitoloProgramma" Text="Programma" runat="server"  CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtTitoloProgramma"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCodiceProgramma"  AssociatedControlID="txtCodiceProgramma" Text="Codice Programma" runat="server"  CssClass="label"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCodiceProgramma"  CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                 </div>
               <div  class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblbando" CssClass="label" AssociatedControlID="txtbando" runat="server" Text="Bando"></asp:Label>
                </div>
                <div class="colOggetti" style="width:85%">       
                    <asp:DropDownList id="txtbando" Width="96%"  runat="server" CssClass="ddlClass"></asp:DropDownList>        
                </div>
            </div>
               <div  class="row">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblSettore" CssClass="label" AssociatedControlID="ddlMaccCodAmAtt" runat="server" Text="Settore"></asp:Label>
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
            </div>

            <div  class="row">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblddlStatoAttivita" CssClass="label" AssociatedControlID="ddlStatoAttivita" runat="server" Text="Stato Progetto"></asp:Label>
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
                    <asp:Label ID="Label1" CssClass="label" AssociatedControlID="ddlFiltro" runat="server" Text="Tipo Ricerca"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlFiltro"  runat="server" CssClass="ddlClass">
                        <asp:ListItem Value="0" Selected="True">Tutti</asp:ListItem>
						<asp:ListItem Value="1">Da Confermare</asp:ListItem>
						<asp:ListItem Value="2">Confermati</asp:ListItem>
                    </asp:DropDownList>       
                </div>
               <div class="collable" style="width:15%">      
                    <asp:Label ID="Label2" CssClass="label" AssociatedControlID="ddlTipiProgetto" runat="server" Text="Tipo Progetto"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlTipiProgetto"  runat="server" CssClass="ddlClass" >
                    </asp:DropDownList>       
                </div>
            </div>
      <div  class="row">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="lblSanzione" CssClass="label" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlSegnalazioneSanzione"  runat="server" CssClass="ddlClass">
                    <asp:ListItem Selected="True">Tutti</asp:ListItem>
								<asp:ListItem Value="0">No</asp:ListItem>
								<asp:ListItem Value="1">Si</asp:ListItem>
                    </asp:DropDownList>       
                </div>
               <div class="collable" style="width:15%">      
                    <asp:Label ID="LblStatoValutazione" CssClass="label" AssociatedControlID="ddlStatoValutazione" Visible="False" runat="server" Text="Stato Valutazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList id="ddlStatoValutazione"  runat="server" CssClass="ddlClass" Visible="False">
                    	<asp:ListItem Selected="True">Tutti</asp:ListItem>
								<asp:ListItem Value="0">Non Definito</asp:ListItem>
			                    <asp:ListItem Value="1" Text="Ammissibile"></asp:ListItem>
			                    <asp:ListItem Value="2" Text="Non Ammissibile"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Escluso"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Non Valutabile"></asp:ListItem>
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
            <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Risultato Ricerca Progetti"
                    cssClass="table"  AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False"  PageSize="10"
                    UseAccessibleHeader="True">
		     	        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:TemplateColumn HeaderText="Selez."  > 
                        <ItemTemplate >
	                        <asp:ImageButton ID="SelPrgetti" CommandName="Select" AlternateText="Valutazione Qualità Progetto" ToolTip="Seleziona Progetto" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    			<asp:BoundColumn DataField="denominazione" HeaderText="Denominazione"></asp:BoundColumn>
									<asp:BoundColumn DataField="codiceente" Visible="false" HeaderText="Codice Progetto"></asp:BoundColumn>
									<asp:BoundColumn DataField="titolo" HeaderText="Titolo"></asp:BoundColumn>
									<asp:BoundColumn DataField="CodiceRegione" Visible="false" HeaderText="Cod. Ente"></asp:BoundColumn>
									<asp:BoundColumn DataField="bando" HeaderText="Bando"></asp:BoundColumn>
									<asp:BoundColumn DataField="ambito" HeaderText="Settore / Area Intervento"></asp:BoundColumn>
									<asp:BoundColumn DataField="data" Visible="false" HeaderText="Data Presentazione">
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="idattivit&#224;" HeaderText="idattivit&#224;"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="idente" HeaderText="Idente"></asp:BoundColumn>
									<asp:BoundColumn DataField="davalutare" HeaderText="Valutazioni Confermate">
									</asp:BoundColumn>
									<asp:BoundColumn DataField="punteggio" HeaderText="Punteggio">
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Effettivi" HeaderText="Posti Effettivi">
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Richiesti" HeaderText="Posti Richiesti">
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="statoattivit&#224;"></asp:BoundColumn>
									<asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
									<asp:BoundColumn DataField="SegnalazioneSanzione" HeaderText="Presenza Sanzione">
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Limitazioni" HeaderText="Limitazioni"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="denominazione" Visible="false" HeaderText="DenominazioneNascosta"></asp:BoundColumn>
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
