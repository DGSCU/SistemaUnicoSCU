<%@ Page Title="Pagina per la ricerca degli enti" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaEnti.aspx.vb" Inherits="Futuro.WfrmRicercaEnti" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent"  runat="server">
   
    <fieldset class="ContornoPagina">
<legend >Ricerca Ente</legend>
<div class="wrapper" style="width:100%">
    <div class="headers">
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="RICERCA ENTE"></asp:Label>
      </h2>
 </div>
    <div class="RigaVuota">
      
      &nbsp;</div>

    <div class="row" >
        <div class="collable" style="width:15%">
          
        <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione"   runat="server"
            Text="Denominazione:"></asp:Label>
        
        </div>
        <div class="colOggetti" style="width:35%">
         
       
      <asp:TextBox ID="txtdenominazione" autofocus="true" CssClass="textbox" 
           runat="server" ></asp:TextBox>
       
        </div>
        <div class="collable" style="width:15%">
         
        <asp:Label ID="lblAttribuita" CssClass="label" AssociatedControlID="ddlClasseAttribuita" runat="server"
            Text="Sezione Attribuita:"></asp:Label>
       
        </div>
        <div class="colOggetti" style="width:35%">
         
              <asp:DropDownList ID="ddlClasseAttribuita" CssClass="ddlClass"   runat="server" >
              </asp:DropDownList>
      
        </div>
    </div>
    <div class="row" >
       <div class="collable" style="width:15%">
          
            <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="txtCodEnte" ToolTip="codice dell'ente"   runat="server"
            Text="Cod. Ente:"></asp:Label>
         
       </div>
       <div class="colOggetti" style="width:35%">
          
      <asp:TextBox ID="txtCodEnte" CssClass="textbox" 
           runat="server" ></asp:TextBox>
        
       </div>
       <div class="collable" style="width:15%">
           
            <asp:Label ID="lblRichiesta" CssClass="label"  AssociatedControlID="ddlClasseRichiesta"   runat="server"
            Text="Sezione Richiesta:"></asp:Label>
         
       </div>
       <div class="colOggetti" style="width:35%">
           
               <asp:DropDownList ID="ddlClasseRichiesta" CssClass="ddlClass"   runat="server" >
               </asp:DropDownList>
        
       </div>
         
    </div>
    <div class="row" >
       <div class="collable" style="width:15%">
           
            <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="txtCodFis"   runat="server"
            Text="Codice Fiscale:"></asp:Label>
          
       </div>
       <div class="colOggetti" style="width:35%">
          
           <asp:TextBox ID="txtCodFis" CssClass="textbox" 
           runat="server" ></asp:TextBox>
       
           
       </div>
       <div class="collable" style="width:15%">
          
            <asp:Label ID="lblTipologia" CssClass="label" AssociatedControlID="ddlTipologia"   runat="server"
            Text="Tipologia:"></asp:Label>
         
       </div>
       <div class="colOggetti" style="width:35%">
           
               <asp:DropDownList ID="ddlTipologia" CssClass="ddlClass"   runat="server" >
               </asp:DropDownList>
         
       </div>
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">           
           <asp:Label ID="lblAlbo" AssociatedControlID="ddlAlbo"  CssClass="label" runat="server"
            Text="Albo:"></asp:Label>         
       </div>
        <div class="colOggetti" style="width:35%">          
               <asp:DropDownList ID="ddlAlbo" CssClass="ddlClass"   runat="server">
                   <asp:ListItem>Seleziona</asp:ListItem>
                   <asp:ListItem>SCN</asp:ListItem>
                   <asp:ListItem>SCU</asp:ListItem>
               </asp:DropDownList>        
       </div>

        <div class="collable" style="width:15%">          
            <asp:Label ID="lblStato" AssociatedControlID="ddlStato" CssClass="label" runat="server"
            Text="Stato:"></asp:Label>          
       </div>
        <div class="colOggetti" style="width:35%">          
               <asp:DropDownList ID="ddlStato" CssClass="ddlClass"   runat="server">
               </asp:DropDownList>        
       </div>  
    </div>
    <div class = "row">
       <div class="collable" style="width:15%">           
           <asp:Label ID="lblemail" AssociatedControlID="txtEmail"  CssClass="label" runat="server"
            Text="Email:"></asp:Label>         
       </div>
       <div class="colOggetti" style="width:35%">          
          <asp:TextBox ID="txtEmail" CssClass="textbox"  runat="server"></asp:TextBox>        
       </div>

       <div class="collable" style="width:15%">          
            <asp:Label ID="lblCausaleChiusura" AssociatedControlID="ddlCausaleChiusura" CssClass="label" runat="server"
            Text="Causale Chiusura:"></asp:Label>          
       </div>
        <div class="colOggetti" style="width:35%">          
               <asp:DropDownList ID="ddlCausaleChiusura" CssClass="ddlClass"   runat="server">
               </asp:DropDownList>        
       </div>  
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">
           
           <asp:Label ID="lblPec" AssociatedControlID="txtPEC"  CssClass="label"  runat="server"
            Text="Pec:"></asp:Label>
         
       </div>
        <div class="colOggetti" style="width:35%">
          
      <asp:TextBox ID="txtPEC" CssClass="textbox" 
           runat="server"></asp:TextBox>
         
       </div>
        <div class="collable" style="width:15%">
           
            <asp:Label ID="lblControlloPec" AssociatedControlID="ddlEsistenzaPEC" CssClass="label"  runat="server"
            Text="Esistenza Pec:"></asp:Label>
          
       </div>
        <div class="colOggetti" style="width:35%">
          
               <asp:DropDownList ID="ddlEsistenzaPEC" Width="30%"   runat="server">
               <asp:ListItem Text="" Value=""></asp:ListItem>
			   <asp:ListItem Text="SI" Value="1"></asp:ListItem>
			   <asp:ListItem Text="NO" Value="0"></asp:ListItem>
               </asp:DropDownList>
        
       </div>  
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">
           
           <asp:Label ID="lblCodFiscArch" AssociatedControlID="txtCodFisArchivio"  CssClass="label"  runat="server"
            Text="Codice Fiscale Archiviato:"></asp:Label>
         
       </div>
        <div class="colOggetti" style="width:35%">
          
      <asp:TextBox ID="txtCodFisArchivio" CssClass="textbox" 
           runat="server"></asp:TextBox>
         
       </div>
        <div class="collable" style="width:15%">
          
            <asp:Label ID="lblCompetenza" AssociatedControlID="CboCompetenza"  CssClass="label" runat="server"
            Text="Competenza:"></asp:Label>
          
       </div>
        <div class="colOggetti" style="width:35%">
          
               <asp:DropDownList ID="CboCompetenza" CssClass="ddlClass"   runat="server">
               </asp:DropDownList>
        
       </div>  
    </div>
    <div class="row" >
        <div class="collable" style="width:15%">           
           <asp:Label ID="lblVariazioni" AssociatedControlID="ddlRichistaVariazione" CssClass="label" runat="server"
            Text="Richiesta Variazione:"></asp:Label>         
        </div>
        <div class="colOggetti" style="width:35%">                    
               <asp:DropDownList ID="ddlRichistaVariazione" CssClass="ddlClass"  runat="server">
               <asp:ListItem Text="Tutti" Value=""></asp:ListItem>
			   <asp:ListItem Value="1">Variazione richieste</asp:ListItem>
			   <asp:ListItem Value="0">Senza variazione</asp:ListItem>
               </asp:DropDownList>       
        </div>
        <div class="collable" style="width:15%">          
            <asp:Label ID="lblFasiAperte" AssociatedControlID="ddlFasi" CssClass="label" runat="server"
            Text="Fasi Aperte:"></asp:Label>          
        </div>
        <div class="colOggetti" style="width:35%">          
               <asp:DropDownList ID="ddlFasi" CssClass="ddlClass"  runat="server">
                <asp:ListItem Text="Tutti" Value="0"></asp:ListItem>
				<asp:ListItem Text="Iscrizione" Value="1"></asp:ListItem>
				<asp:ListItem Text="Adeguamento" Value="2"></asp:ListItem>
				<asp:ListItem Text="Art.2" Value="3"></asp:ListItem>
				<asp:ListItem Text="Art.10" Value="4"></asp:ListItem>
               </asp:DropDownList>        
        </div>
     </div>
        <div class="row" >
            <div class="collable" style="width:15%">           
               <asp:Label ID="lblTitolare" AssociatedControlID="ddlTitolareAccoglienza" CssClass="label" runat="server"
                Text="Titolare/Accoglienza:"></asp:Label>         
            </div>
            <div class="colOggetti" style="width:35%">                    
                   <asp:DropDownList ID="ddlTitolareAccoglienza" CssClass="ddlClass"  runat="server">
                   <asp:ListItem Value="0" Text="Tutti"></asp:ListItem>
			       <asp:ListItem Value="1" Text="Titolare"></asp:ListItem>
			       <asp:ListItem Value="2" Text="Accoglienza"></asp:ListItem>
                   </asp:DropDownList>       
            </div>
            <div class="collable" style="width:15%">
           
            <asp:Label ID="lblAttesaAccreditamento" AssociatedControlID="ddlAttesaAccreditamento" CssClass="label"  runat="server"
            Text="In Attesa Decreto Iscrizione:"></asp:Label>
          
       </div>
        <div class="colOggetti" style="width:35%">
          
               <asp:DropDownList ID="ddlAttesaAccreditamento" Width="30%"   runat="server">
               <asp:ListItem Text="" Value=""></asp:ListItem>
			   <asp:ListItem Text="SI" Value="1"></asp:ListItem>
               </asp:DropDownList>

       </div>  
       </div>  

   
    
    

    <div class="RigaPulsanti">
        <asp:Button
          ID="CmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;<asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />

   </div>
    <div class="footers">
      <asp:Label ID="Label6" runat="server" Text="">&nbsp;</asp:Label>
 </div>
    <div class="RigaVuotaPrint" >
   <br />
        <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" 
            Text="Esporta CSV" Visible="False" />
            <br />
         <asp:HyperLink ID="ApriCSV1"   AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" 
        Text="DOWNLOAD CSV" runat="server" 
        ForeColor="#003399" Visible="False"></asp:HyperLink>
      &nbsp;
    </div>
</div>


<br />
<h3>
<asp:label id="lblmessaggio" runat="server" Text="Utilizzare il pulsante Ricerca per ottenere la lista dei risultati." AssociatedControlID="dgRisultatoRicerca"></asp:label>
</h3>
<br />

<asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%"  ToolTip="Risultato Ricerca Enti"   
          AllowPaging="True" CellPadding="2"  CssClass="table" 
        AutoGenerateColumns="False" UseAccessibleHeader="True">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
                                <asp:TemplateColumn  HeaderText="Selez.">
									<ItemTemplate>
                                        
                                         <asp:ImageButton ID="btnSeleziona" runat="server" AlternateText="Seleziona Ente" OnCommand="btnSeleziona_Click" 
                                                            CommandArgument='<%# Eval("IDente")%>' ImageUrl="~/Images/ente.png"  
                                                            ToolTip="Seleziona Ente" />
                                        </ItemTemplate>
                                       
                                        </asp:TemplateColumn>
									<asp:BoundColumn  DataField="codiceregione" HeaderText="Cod.Ente" >
										<HeaderStyle ></HeaderStyle>
										<ItemStyle  ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn  DataField="Denominazione" HeaderText="Denominazione">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="200px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Statoente" HeaderText="Stato"></asp:BoundColumn>
									<asp:BoundColumn DataField="tipologia" HeaderText="Tipologia">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="100px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ClasseRichiesta" 
                                        HeaderText="Sezione Richiesta">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle Width="80px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ClasseAttribuita" 
                                        HeaderText="Sezione Attribuita">
										<HeaderStyle  ></HeaderStyle>
										<ItemStyle Width="80px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Numerototalesedi" HeaderText="Nr.Sedi Attuazione">
										<HeaderStyle  ></HeaderStyle>
										<ItemStyle Width="70px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="http" HeaderText="http://">
										<HeaderStyle  ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CodiceFiscale" HeaderText="CodiceFiscale">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="idente" HeaderText="idEnte">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="datacontrollohttp" HeaderText="verHttp">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="datacontrolloemail" HeaderText="veremail">
										<HeaderStyle ></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Competenza" HeaderText="Competenza"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="EmailCertificata" HeaderText="PEC">
										<HeaderStyle ></HeaderStyle>
										<ItemStyle ></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="idClasseAccreditamentoRichiesta" HeaderText="ClasseAccreditamentoRichiesta"></asp:BoundColumn>
								</Columns>
								<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
<p>
    &nbsp;</p>

</fieldset>

</asp:Content>


