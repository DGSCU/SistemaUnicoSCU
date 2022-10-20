<%@ Page Title="Ricerca Progetti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ricercaprogetti.aspx.vb" Inherits="Futuro.ricercaprogetti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
           //<![CDATA[
            function VisualizzaSanzioneProg(IDAttività, IdEntePresentante) 
            {
                window.open("WfrmSedeSanzionata.aspx?IDAttivita=" + IDAttività + "&IdEntePresentante=" + IdEntePresentante, "ProgettoSanzionato", "width=950, height=600,  toolbar=no, location=no, menubar=no, scrollbars=yes");
            }
            //]]>
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="firefox">
    <fieldset class="ContornoPaginaScroll">
    <legend >Ricerca Progetti</legend>
    <div class="wrapper" style="width:100%">
        <div class="headers"> 
           <h2>
          <asp:Label ID="Label5" runat="server"  Text="RICERCA PROGETTI"></asp:Label>
          </h2>
     </div>
        <div class="RigaVuota">
          &nbsp;</div>
        <div class="row" style="height:auto">
        <asp:label id="lblmessaggio" CssClass="msgErrore"  runat="server" ></asp:label>
        </div>
       <div class="RigaVuota">
          &nbsp;</div>
        <div class="row" >
            <div class="collable" style="width:15%">
          
            <asp:Label ID="lblTitoloProgetto" AssociatedControlID="txtTitoloProgetto"    runat="server"
                Text="Titolo Progetto"></asp:Label>
        
            </div>
            <div class="colOggetti" style="width:35%" >
         
       
          <asp:TextBox ID="txtTitoloProgetto" 
               runat="server" autofocus="true"></asp:TextBox>
       
            </div>
            <div class="collable" style="width:15%" >
         
                &nbsp;
         
                <asp:Label ID="lblCodiceProgetto"  AssociatedControlID="TxtCodPog" runat="server"
                Text="Codice Progetto"></asp:Label>
       
            </div>
            <div class="colOggetti" style="width:35%" >
                 <asp:TextBox ID="TxtCodPog" 
               runat="server" ></asp:TextBox>
      
            </div>
        </div>
        <div class="row" >
           <div class="collable" style="width:15%">
          
                <asp:Label ID="lblTipiProgetto"  AssociatedControlID="DdlTipiProgetto" 
                    ToolTip="codice dell'ente"   runat="server"
                Text="Tipo Progetto"></asp:Label>
         
           </div>
           <div class="colOggetti" style="width:35%">
          
                   <asp:DropDownList ID="DdlTipiProgetto"  runat="server" >
                   </asp:DropDownList>
        
           </div>
           <div class="collable" style="width:15%">
           &nbsp;
                <asp:Label ID="lblStati"  AssociatedControlID="ddlStatoAttivita"   runat="server"
                Text="Stati Progetto"></asp:Label>
         
           </div>
           <div class="colOggetti" style="width:35%">
           
                   <asp:DropDownList ID="ddlStatoAttivita"  runat="server" >
                   </asp:DropDownList>
        
           </div>
         
        </div>
        <div class="row" >
           <div class="collable" style="width:15%">
           
                <asp:Label ID="lblCircolare" AssociatedControlID="DdlBando"   runat="server"
                Text="Circolare"></asp:Label>
          
           </div>
           <div class="colOggetti" style="width:35%">
          
                <asp:DropDownList ID="DdlBando"  runat="server" >
                   </asp:DropDownList>
       
           
           </div>
           <div class="collable" style="width:15%">
          &nbsp;
                  <asp:Label ID="lblCompetenza" AssociatedControlID="CboCompetenza"   runat="server"
                Text="Competenza"></asp:Label>
         
           </div>
           <div class="colOggetti" style="width:35%">
             <asp:DropDownList ID="CboCompetenza"   runat="server" >
                   </asp:DropDownList>
           </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="lblSettore"  AssociatedControlID="ddlMaccCodAmAtt"   runat="server"
                 Text="Settore"></asp:Label>
            
         
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlMaccCodAmAtt"   runat="server" >
                   </asp:DropDownList>
                
           </div>
            <div class="collable" style="width:15%">
           &nbsp;
                <asp:Label ID="lblEnteSecondario"  AssociatedControlID="txtDenominazioneEnteSecondario" runat="server"
                Text="Ente Secondario"></asp:Label>
         
           </div>
            <div class="colOggetti" style="width:35%">
               <asp:TextBox ID="txtDenominazioneEnteSecondario" 
               runat="server"></asp:TextBox>
         
           </div>
        </div>
         <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="lblRegione"  AssociatedControlID="txtRegione" runat="server" Text="Regione"></asp:Label>  
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtRegione"  runat="server"></asp:TextBox> 
           </div>
            <div class="collable" style="width:15%">
            &nbsp;
                <asp:Label ID="lblProvncia"  AssociatedControlID="txtProvincia" runat="server" Text="Provincia"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtProvincia" runat="server"></asp:TextBox>
           </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="lblComune"  AssociatedControlID="txtcomune" runat="server" Text="Comune"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtcomune"  runat="server"></asp:TextBox>
           </div>
            <div class="collable" style="width:15%">&nbsp;
                <asp:Label ID="lblDurataProgetto"  AssociatedControlID="ddlDurataProgetto" runat="server" Text="Durata (mesi)"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlDurataProgetto"  runat="server" > </asp:DropDownList>
           </div>
        </div>
        <%If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then%>
       <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="lblCodReg" AssociatedControlID="txtCodReg"  runat="server" Text="Cod Ente"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">
                 <asp:TextBox ID="txtCodReg"  runat="server"></asp:TextBox>
           </div>
            <div class="collable" style="width:15%">
            &nbsp;
                <asp:Label ID="lblDenEnte"  AssociatedControlID="txtDenominazioneEnte" runat="server" Text="Ente Presentante"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtDenominazioneEnte"  runat="server"></asp:TextBox>
           </div>
        </div>
       
       <% end if %>
       <div class="row">
            <div class="collable" style="width:15%">
                <asp:Label ID="lblGiovaniMinoriOp" AssociatedControlID="ddlGiovaniMinoriOp" runat="server" Text="Giovani Minori Opportunità"></asp:Label>
           </div>
            <div class="collable" style="width:35%">
                <asp:DropDownList ID="ddlGiovaniMinoriOp" runat="server">
                    <asp:ListItem Selected="True">Tutti</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                    <asp:ListItem Value="1">Si</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="collable" style="width:15%">&nbsp;
                <asp:Label ID="lblFami" AssociatedControlID="ddlFami" runat="server" Text="Fami"></asp:Label>
            </div>
            <div class="collable" style="width:35%">
                <asp:DropDownList ID="ddlFami" runat="server">
                    <asp:ListItem Selected="True">Tutti</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                    <asp:ListItem Value="1">Si</asp:ListItem>
                </asp:DropDownList>   
            </div>
        </div>
        <div class="row">
             <div class="collable" style="width:15%">
                <asp:Label ID="lblSanzione" AssociatedControlID="ddlSegnalazioneSanzione" runat="server" Text="Presenza Sanzione"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlSegnalazioneSanzione"   runat="server">
                   	    <asp:ListItem Selected="True">Tutti</asp:ListItem>
						<asp:ListItem Value="0">No</asp:ListItem>
						<asp:ListItem Value="1">Si</asp:ListItem>
                 </asp:DropDownList>
           </div>  
            <div class="collable" style="width:15%">
            &nbsp;
                <asp:Label ID="LblStatoValutazione"  AssociatedControlID="ddlStatoValutazione" 
                    runat="server" Text="Stato Valutazione" Visible="False"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">                  
                <asp:DropDownList ID="ddlStatoValutazione"  runat="server" 
                    Visible="False">
                    <asp:ListItem Selected="True">Tutti</asp:ListItem>
			        <asp:ListItem Value="0">Non Definito</asp:ListItem>
			        <asp:ListItem Value="1">Ammissibile</asp:ListItem>
			        <asp:ListItem Value="2">Non Ammissibile</asp:ListItem>
                    <asp:ListItem Value="3">Escluso</asp:ListItem>
                    <asp:ListItem Value="4">Non Valutabile</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtbando1" Width="0px" MaxLength="255" Runat="server" Visible="False"></asp:TextBox>  
                <asp:TextBox id="txtDenominazioneEnte1" Width="0px" MaxLength="200" Runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox id="ddlStatoAttivita1" Width="0px" Runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox id="ddlMaccCodAmAtt1" Width="0px" Runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox id="txtTitoloProgetto1" runat="server" Width="0px" MaxLength="255" Visible="False"></asp:TextBox>
            </div>  
        </div>
        <div class="row">
          <div class="collable" style="width:15%">
          <asp:Label ID="lbltitoloprogramma" AssociatedControlID="txtTitoloProgramma"    runat="server"
                Text="Titolo Programma"></asp:Label>
          </div>
          <div class="colOggetti" style="width:35%">
          <asp:TextBox ID="txtTitoloProgramma" 
               runat="server" autofocus="true"></asp:TextBox>
          </div>
          <div class="collable" style="width:15%">
           <asp:Label ID="lblcodiceProgramma"  AssociatedControlID="TxtCodProgramma" runat="server"
                Text="Codice Programma"></asp:Label>
          </div>
          <div class="colOggetti" style="width:35%">
          <asp:TextBox ID="TxtCodProgramma" 
               runat="server" ></asp:TextBox>
          </div>
          </div>
         <div class="row">
          <div class="collable" style="width:15%">
          <asp:Label ID="LblProgramma"  AssociatedControlID="txtProgramma" runat="server" Text="Codice Ente Referente Programma"></asp:Label>
          </div>
           <div class="colOggetti" style="width:35%">
           <asp:TextBox ID="txtProgramma"  runat="server" CssClass="textbox"  ></asp:TextBox>
           </div>
         </div>
        <div class="RigaPulsanti">
            <asp:Button ID="CmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
            <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />&nbsp;
            <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" ToolTip="Esporta elenco risultato ricerca" />
            <br />
            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa"                 Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>            
        </div>
<%--        <div class="footers">
            <asp:Label ID="Label6" runat="server" Text="">&nbsp;</asp:Label>
        </div>--%>
      <div class="RigaVuotaPrint" >

            <asp:Button ID="CmdEsportaElenco" runat="server" CssClass="Pulsante" 
                Text="Esporta CSV" Visible="False" 
              ToolTip="Esportazione elenco progetti" />
                <br />
             <asp:HyperLink ID="hlApriElencoProgetti"   AccessKey="S" 
              ToolTip="Link per l'esportazione docummenti progetto" CssClass="linkStampa" 
            Text="ELENCO PROGETTI" runat="server" 
            ForeColor="#003399" Visible="False"></asp:HyperLink>
             <asp:HyperLink ID="hlApriElencoSediProgetti"   AccessKey="S" 
              ToolTip="Link per l'esportazione elenco sedi progetto" CssClass="linkStampa" 
            Text="ELENCO SEDI PROGETTI" runat="server" 
            ForeColor="#003399" Visible="False"></asp:HyperLink>
            <asp:HyperLink ID="hlApriElencoRisorseProgetto"   AccessKey="S" 
              ToolTip="Link per l'esportazione delle risorse" CssClass="linkStampa" 
            Text="ELENCO RISORSE DI PROGETTO" runat="server" 
            ForeColor="#003399" Visible="False"></asp:HyperLink>
          &nbsp;
        </div>
    </div>


       <div class="row">
       <asp:Label CssClass="msgInfo" ID="lblInfoRicerca" runat="server"></asp:Label>
          </div>
    <asp:datagrid id="dgRisultatoRicerca" runat="server"  Caption="Risultato Ricerca Progetti" CaptionAlign="Top"  
            AllowPaging="True" CellPadding="1" Width="100%" CssClass="table"  PageSize ="10"
            AutoGenerateColumns="False" UseAccessibleHeader="True">
        <FooterStyle></FooterStyle>
        <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
        <EditItemStyle></EditItemStyle>
        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
        <HeaderStyle></HeaderStyle>
        <Columns>
			<asp:TemplateColumn HeaderText="Selez.">
		    <ItemTemplate>           
                <asp:ImageButton ID="btnSeleziona" AlternateText="Selezione Progetto"   runat="server" CommandName="Select"
                                ImageUrl="~/Images/Icona_Progetto_small.png"  ToolTip="Selezione Progetto" />
            </ItemTemplate>         
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="False" HeaderText="Accet.">
		    <ItemTemplate>           
                <asp:ImageButton ID="btnaccettazione" AlternateText="Accettazione Progetto"  runat="server" CommandName="accettazione"
                                ImageUrl="~/Images/valida_small.png"  ToolTip="Accettazione Progetto" />
            </ItemTemplate>         
            </asp:TemplateColumn>
            <asp:TemplateColumn Visible="False" HeaderText="Valutaz.">
		    <ItemTemplate>           
                <asp:ImageButton ID="btnvalutazione" runat="server"   AlternateText="Valutazione Progetto" CommandName="valutazione"
                                ImageUrl="~/Images/vincoli_small.png" ToolTip="Valutazione Progetto" />
            </ItemTemplate>         
            </asp:TemplateColumn>
			<asp:BoundColumn DataField="denominazione" HeaderText="Denominazione"></asp:BoundColumn>
			<asp:BoundColumn DataField="titolo" HeaderText="Titolo"></asp:BoundColumn>
			<asp:BoundColumn DataField="bando" HeaderText="Bando"></asp:BoundColumn>
			<asp:BoundColumn DataField="ambito" HeaderText="Settore / Area Intervento">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="data" HeaderText="Data Ultimo Stato">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="idattività" HeaderText="idattivit&#224;">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="idente" HeaderText="Idente"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="modificabile" HeaderText="modificabile"></asp:BoundColumn>
			<asp:BoundColumn Visible="False" DataField="IdTipoProgetto" HeaderText="IdTipoProgetto"></asp:BoundColumn>
			<asp:BoundColumn DataField="statoattivit&#224;" HeaderText="Stato Progetto">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="VolRic" HeaderText="N&#176; Volontari Concessi">
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundColumn>
			<asp:BoundColumn DataField="codiceente" HeaderText="Codice Progetto">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="RegioneCompetenza" HeaderText="Competenza"></asp:BoundColumn>
			<asp:BoundColumn  DataField="SegnalazioneSanzione" HeaderText="Presenza Sanzione">
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
         
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
			PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>

</fieldset>
</div>
        
</asp:Content>

