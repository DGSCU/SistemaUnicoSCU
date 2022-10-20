<%@ Page Title="Presenze Volontario" Language="vb" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Presenze.aspx.vb" Inherits="Futuro.Presenze" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script  type="text/javascript">

            function infoPrefissi() {
                window.open("wfrmInfoPrefissiDocumentiEntita.aspx", "Elenco", "height=450,width=600,dependent=no,scrollbars=yes,status=no");
            }
		
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
<legend >GESTIONE PRESENZE/ASSENZE</legend>
<asp:Label ID="lblMessaggio" runat="server" Text=""></asp:Label>
            <div id="divConferma" class="rowGroup" style="height:auto" runat="server" visible="false">
                <fieldset class="ContornoPagina">
                <legend>Conferma assenze</legend>
                    <div class="wrapper" style="width:100%;border:0px">
                        <div class="row" style="height:auto">
                            <h3><asp:Label runat="server" ID="lblConferma" CssClass="labelDati"></asp:Label></h3>
                        </div>
                        <br />
                        <div class="RigaPulsanti" style="height:auto;text-align:center">
                            <asp:Button ID="cmdCertificatoUnico" CssClass="Pulsante" runat="server" ToolTip="Certificato Unico/Continuativo" Text="Certificato Unico/Continuativo" />
                            <asp:Button ID="cmdCertificatiDiversi" CssClass="Pulsante" runat="server" ToolTip="Certificati Diversi" Text="Certificati Diversi" />
                        </div>
                    </div>
                </fieldset>
            </div>
<fieldset class="ContornoPagina">
<legend>VOLONTARIO</legend>
<p style="width:100%">
 

    CODICE: <asp:Label ID="lblCodiceVolontario" runat="server" CssClass="labelDati"  Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
    NOME: <asp:Label ID="lblNome" runat="server" CssClass="labelDati"  Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
    COGNOME: <asp:Label ID="LblCognome" CssClass="labelDati"  runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
    INIZIO SERVIZIO: <asp:Label ID="lblInizioServizio" CssClass="labelDati" Font-Bold="true" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
    FINE SERVIZIO: <asp:Label ID="lblFineServizio" CssClass="labelDati" Font-Bold="true" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
   </p>
   <p style="text-align:right">        
     <strong><asp:LinkButton ID="imgStoricoNotifiche" runat="server" style="cursor:pointer;font-size:small;font-weight:bold;text-align:5px" Text="Storico Notifiche" Visible="false"></asp:LinkButton></strong>
    </p> 
 
</fieldset>
        <p style="width:100%">
            PROCEDURA: SELEZIONARE LA CAUSALE DI PRESENZA/ASSENZA DA APPLICARE E CLICCARE SUL GIORNO CORRISPONDENTE.
        </p>

      <div>
        <div style="float:right">
      
        <div class="firefox">
       <fieldset class="ContornoPagina" runat="server" id="fildGrigliaMese">
    <legend>RIEPILOGO  MESE</legend>
                <asp:datagrid ID="DtgRiepilogoPresenze" CssClass="table" Visible="true" FooterStyle-Height="25px" FooterStyle-BackColor="Red" Font-Size="small" runat="server" ToolTip="Riepilogo giorni Causali"  AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True"  CellPadding="0" 
												        Width="100%"	PageSize="31">
       	        <FooterStyle>
       
                </FooterStyle>
		        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		        <EditItemStyle ></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"></ItemStyle>
		        <HeaderStyle Height="30px"></HeaderStyle>	
                <Columns>
                <asp:BoundColumn Visible="True" DataField="Codice" ItemStyle-HorizontalAlign="Center"  HeaderText="Cod."></asp:BoundColumn>
		        <asp:BoundColumn Visible="True" DataField="Descrizione" HeaderText="Causale"></asp:BoundColumn>
                <asp:BoundColumn Visible="True" DataField="Totale" ItemStyle-HorizontalAlign="Center"  HeaderText="Tot."></asp:BoundColumn>
       
                </Columns>	
        		
                </asp:datagrid>
                <div class="bold" style="background-color:#FFFFFF;color:#3a4f63;width:100%;height:30px;text-align:right;"> TOTALE GIORNI N°
                    <asp:Label ID="lblTotale" CssClass="bold" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <div class="bold" style="background-color:#FFFFFF;color:#3a4f63;width:100%;height:20px;text-align:right;"> Conferma Mensile
                    <asp:Label ID="lblStatoConfermaMese" CssClass="bold" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <div class="bold" style="background-color:#FFFFFF;color:#3a4f63;width:100%;height:20px;text-align:right;"> Conferma Preliminare
                    <asp:Label ID="lblStatoConfermaPreliminare" CssClass="bold" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
        
       </fieldset>
        <fieldset class="ContornoPagina" runat="server" id="fildGrigliaInizioServizio">
           <legend>RIEPILOGO ASSENZE CUMULATE</legend>
        
            <asp:datagrid ID="DtgRiepilogoPresenzeinizioservizio" CssClass="table" Visible="true" FooterStyle-Height="25px" FooterStyle-BackColor="Red" Font-Size="small" runat="server" ToolTip="Riepilogo giorni Causali"  AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True"  CellPadding="0" 
												        Width="100%"	PageSize="31">
       	        <FooterStyle>
       
                </FooterStyle>
		        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		        <EditItemStyle ></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr"></ItemStyle>
		        <HeaderStyle Height="30px"></HeaderStyle>	
                <Columns>
                <asp:BoundColumn Visible="True" DataField="Codice" ItemStyle-HorizontalAlign="Center"  HeaderText="Cod."></asp:BoundColumn>
		        <asp:BoundColumn Visible="True" DataField="Descrizione" HeaderText="Causale"></asp:BoundColumn>
                <asp:BoundColumn Visible="True" DataField="Totale" ItemStyle-HorizontalAlign="Center"  HeaderText="Tot."></asp:BoundColumn>
       
                </Columns>	
        		
                </asp:datagrid>
           
       
          </fieldset> 
         </div>
      </div>
    </div> 
        <div> 
            <div style="width:100%;text-align:left;">
                    &nbsp;&nbsp;<asp:Label ID="lblCausale" AssociatedControlID="ddlMotivo" runat="server" Text=" CAUSALE PRESENZA/ASSENZA: "></asp:Label>
                <asp:DropDownList ID="ddlMotivo"  Width="45%" ForeColor="#3a4f63"  Visible="false" runat="server"> </asp:DropDownList>
                <p>
                </p>   
            </div>
        </div>
        <asp:Calendar ID="Calendar1" runat="server" DayStyle-Font-Size="Large" NextPrevStyle-ForeColor="White" TitleStyle-Font-Bold="true" TitleStyle-Font-Names="Verdana"  TitleFormat="MonthYear" TitleStyle-Font-Size="Large" TitleStyle-CssClass="IntestazioneCalendario"  TitleStyle-ForeColor="White" TitleStyle-BackColor="#3a4f63" DayStyle-Font-Bold="true" DayStyle-ForeColor="#3a4f63" ShowGridLines="True" Height="100%" Width="68%">
             <OtherMonthDayStyle   CssClass="Calendario"/>
             <SelectedDayStyle>
            </SelectedDayStyle>
        </asp:Calendar>
   
  
    <p></p>
    <div id="idupload" runat="server" style="width:99%;border-width:2px;border-color:#3a4f63;border-style:solid;">
        <div style="vertical-align:middle;">
            <p>
                <asp:ImageButton  id="imgPrefissiDocumenti"  runat="server" style="cursor:hand;margin-top:5px" ImageUrl="images/info_small.png" OnClientClick="javascript:infoPrefissi()"  ToolTip="Prefissi Documenti" AlternateText="Prefissi Documenti"/>
                <asp:label ID="LblSel" CssClass="label"  AssociatedControlID="txtSelFile" runat="server" Text="Selezionare il file per caricare il Foglio Presenze" ></asp:label>
                <input ID="txtSelFile" type="file" style="width:40%"   runat="server"  />
                <asp:Button  id="cmdUpload" runat="server"  CssClass="Pulsante"  Text="Upload File"  />
            </p>
        </div>
    </div>
    <p></p>
    <fieldset class="ContornoPaginaScrollT">
        <div class="firefox">

         <div class="wrapper" runat="server" id="uploadPresenze" style="width:99%;text-align:left;border-width:2px;border-color:#3a4f63;border-style:solid;">    
                <asp:GridView ID="gridPresenze" Width="100%" AutoGenerateColumns="False" runat="server">
                    <Columns>
                        <asp:ButtonField  HeaderText="Download"   ButtonType="Image" CommandName="Scarica" ImageUrl="~/Images/giu_small.png" HeaderStyle-Font-Bold="true" >
                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                        </asp:ButtonField>
                        <asp:BoundField DataField="idEntitàDocumento" ItemStyle-Font-Size="0px"  
                            ItemStyle-BorderWidth="0px" ItemStyle-Width="0px" HeaderStyle-Font-Size="0px"  
                            HeaderStyle-BorderWidth="0px" HeaderStyle-Width="0px"  Visible="true"  >
                            <HeaderStyle BorderWidth="0px" Font-Size="0px" Width="0px"></HeaderStyle>
                            <ItemStyle BorderWidth="0px" Font-Size="0px" Width="0px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField  DataField="FileName" HeaderText="Nome File"/>
                        <asp:BoundField DataField="dataInserimento" HeaderText="Data Inserimento"/>
                        <asp:BoundField DataField="usernameInserimento" HeaderText="User Name Inserimento"/>
                        <asp:BoundField DataField="HashValue" HeaderText="HashValue"/>
                        <asp:BoundField DataField="StatoDocumento" HeaderText="Stato Documento" />
                        <asp:ButtonField CommandName="Elimina" ButtonType="Image"  HeaderText="Rimuovi" ImageUrl="~/Images/canc_small.png" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </fieldset>
    <p>
        <asp:HyperLink ID="hlDw"  runat="server" Visible="false"></asp:HyperLink>
    </p>
    <div class="RigaPulsanti" style="text-align:right;">
   
        <asp:Button ID="CmdAnnullaValidazione" CssClass="Pulsante"  runat="server" Text="Annulla Validazione" Visible="false"  />
        <asp:Button ID="CmdConferma" CssClass="Pulsante"  runat="server" Text="Valido" Visible="false"  />
        <asp:Button ID="cmdRespingi" CssClass="Pulsante"  runat="server" Text="Non Valido" Visible="false"  />
        <asp:Button ID="cmdConfermaPreliminare" CssClass="Pulsante"  runat="server" Text="Conferma Preliminare"  />
        <asp:Button ID="cmdModuloPrecompilato" CssClass="Pulsante"  runat="server" Text="Genera Modulo"  />
        <asp:Button ID="CmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi"  />
  
   </div>
    
</fieldset>
</asp:Content>
