<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmProgrammi_AssociaRimuovi.aspx.vb" MaintainScrollPositionOnPostBack="true" Inherits="Futuro.WfrmProgrammi_AssociaRimuovi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="firefox">
    <fieldset class="ContornoPaginaScroll">
    <legend >Ricerca Progetti per Programma</legend>
      <div class="wrapper" id="Programmi_AssociaRimuovi" style="width:100%;border:0px">
        <div class="headers"> 
           <h2>
          <asp:Label ID="lblIntestazioneTitolo" runat="server"  Text=""></asp:Label>
          </h2>
     </div>
        <div class="RigaVuota">
          &nbsp;
         </div>
        <div class="row" style="height:auto">
        <asp:Label ID="lblMessaggioConferma"  CssClass="msgConferma" runat="server"></asp:Label>
        <asp:label id="lblmessaggio" CssClass="msgErrore"  runat="server" ></asp:label>
        </div>
        <div class="RigaVuota">
          &nbsp;
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
          
            <asp:Label ID="lblTitoloProgetto" AssociatedControlID="txtTitoloProgetto"    runat="server"
                Text="Titolo Progetto"></asp:Label>
        
            </div>
            <div class="colOggetti" style="width:35%" >
   
          <asp:TextBox ID="txtTitoloProgetto" 
               runat="server" autofocus="true"></asp:TextBox>
       
            </div>
            <div class="collable" style="width:15%">
                  <asp:Label ID="lblMacroAmbito"  AssociatedControlID="ddlMacroAmbito" runat="server" Text="Settore"></asp:Label>        
           </div>
           <div class="colOggetti" style="width:35%">
            <asp:DropDownList ID="ddlMacroAmbito"  runat="server"></asp:DropDownList>
           </div>
        </div>
        <div class="row" >
           <div class="collable" style="width:15%">
        <asp:Label ID="lblDenominazioneEnteProponenteProgetto" AssociatedControlID="txtEnteProponente"    runat="server"
                Text="Ente Proponente Progetto"></asp:Label>
           </div>
           <div class="colOggetti" style="width:35%">
           <asp:TextBox ID="txtEnteProponente" 
               runat="server" autofocus="true"></asp:TextBox>
           </div>
           <div class="collable" style="width:15%" >
                <asp:Label ID="lblCodiceEnteProponenteProgetto"  AssociatedControlID="TxtCodPog" runat="server"
                Text="Codice Ente Proponente Progetto"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%" >
                 <asp:TextBox ID="TxtCodPog" 
               runat="server" ></asp:TextBox>
            </div>
           
         
        </div>
        <div class="row" >
           <div class="collable" style="width:15%">
             <asp:Label ID="lblDurataMesi" AssociatedControlID="ddlDurataMesi"    runat="server"
                Text="Durata Mesi"></asp:Label>
           
          
           </div>
           <div class="colOggetti" style="width:35%">
           
                 <asp:DropDownList ID="ddlDurataMesi"  runat="server" > </asp:DropDownList>
           
           </div>
           <div class="collable" style="width:15%">
        
                <asp:Label ID="lblItaliaEstero" AssociatedControlID="ddlItaliaEstero"    runat="server"
                Text="ItaliaEstero"></asp:Label>
         
           </div>
           <div class="colOggetti" style="width:35%">
           <asp:DropDownList ID="ddlItaliaEstero"  runat="server">
            <asp:ListItem Selected="True"></asp:ListItem>
						<asp:ListItem Value="0">ESTERO</asp:ListItem>
						<asp:ListItem Value="1">ITALIA</asp:ListItem>
           </asp:DropDownList>
           </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
             
                <asp:Label ID="lblRegione"  AssociatedControlID="txtRegione" runat="server" Text="Regione"></asp:Label>
             
           </div>

            <div class="colOggetti" style="width:35%">
                        <asp:TextBox ID="txtRegione" runat="server" autofocus="true"></asp:TextBox>
           
           </div>
            <div class="collable" style="width:15%">
          <asp:Label ID="lblProvincia" AssociatedControlID="txtProvincia"    runat="server"
                Text="Provincia"></asp:Label>
         
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtProvincia" runat="server" autofocus="true"></asp:TextBox>
           </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="lblComune"  AssociatedControlID="txtComune" runat="server" Text="Comune"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">
                <asp:TextBox ID="txtComune" runat="server" autofocus="true"></asp:TextBox>
           </div>
            <div class="collable" style="width:15%">
            <asp:Label ID="lblAssocia"  AssociatedControlID="ddlAssocia" runat="server" Text="Associato"></asp:Label>
           </div>
            <div class="colOggetti" style="width:10%">
            <asp:DropDownList ID="ddlAssocia"  runat="server">
            <asp:ListItem Selected="True"></asp:ListItem>
						<asp:ListItem Value="0">NO</asp:ListItem>
						<asp:ListItem Value="1">SI</asp:ListItem>
           </asp:DropDownList>
            </div>
        </div>
       
        <div class="RigaPulsanti">
            <asp:Button ID="CmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
            <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />&nbsp;
            <br />
           
        </div>
        <div class="RigaVuotaPrint" >

                <br />
          &nbsp;
        </div>
      </div>
        <div  style="width:100%;">
       <h3>
            <asp:label id="lblProgrammi_AssociaRimuovi" runat="server" AssociatedControlID="dtgProgrammi_AssociaRimuovi"></asp:label>
        </h3>
        <br />
  <asp:datagrid id="dtgProgrammi_AssociaRimuovi" Caption="Risultato Ricerca Progetti per Programma"   
            runat="server" Width="100%" ToolTip="Progetti per Programma" 
                   AllowPaging="True"  CssClass="table" AllowSorting="True" 
                   AutoGenerateColumns="False" UseAccessibleHeader="True" PageSize ="5">
	<FooterStyle></FooterStyle>
	<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	<EditItemStyle></EditItemStyle>
	<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	<ItemStyle CssClass="tr" HorizontalAlign ="Center"></ItemStyle>
	<HeaderStyle></HeaderStyle>
    <Columns>
		<asp:BoundColumn Visible="False" DataField="IdAttività" HeaderText="Attiva"></asp:BoundColumn>
       <asp:BoundColumn DataField="CodiceEnteProponenteProgetto" HeaderText="Codice Ente Proponente Progetto"></asp:BoundColumn>
       <asp:BoundColumn DataField="DenominazioneEnteProponenteProgetto" HeaderText="Denominazione Ente Proponente Progetto"></asp:BoundColumn>
       <asp:BoundColumn DataField="TitoloProgetto" HeaderText="Titolo Progetto"></asp:BoundColumn>
       <asp:BoundColumn DataField="Settore" HeaderText="Settore"></asp:BoundColumn>
       <asp:BoundColumn DataField="DurataMesi" HeaderText="Durata Mesi"></asp:BoundColumn>
       <asp:BoundColumn DataField="ItaliaEstero" HeaderText="ItaliaEstero"></asp:BoundColumn>
       <asp:BoundColumn DataField="Coprogettazione" HeaderText="Coprogettazione"></asp:BoundColumn>
       <asp:BoundColumn DataField="Associato" Visible="False" HeaderText="Associato"></asp:BoundColumn>
        
       
       <asp:TemplateColumn HeaderText="Associa/Rimuovi"> 
             <ItemTemplate >
                 <asp:ImageButton ID="CmdSelezionato" ToolTip="Escludi" AlternateText="Escludi"  CommandName="Escludi" ImageUrl="~/Images/selezionato_small.png"  Visible='<%# IIF(Eval("Associato").ToString().Equals("SI"), True, False) %>' runat="server" />
                 <asp:ImageButton ID="CmdNonSelezionato" ToolTip="Includi" AlternateText="Includi" CommandName="Includi" ImageUrl="~/Images/deselezionato_small.png" Visible='<%# IIF(Eval("Associato").ToString().Equals("NO"), True, False) %>' runat="server" />  
             </ItemTemplate>
        </asp:TemplateColumn>
        
	</Columns>
		<PagerStyle NextPageText="&gt;&gt;"   
PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
</asp:datagrid>


  <asp:HiddenField runat="server" ID="IdChekSelezionata" />
</div>
    </fieldset>
</div>
</asp:Content>
