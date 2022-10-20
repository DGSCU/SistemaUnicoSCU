<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmSigmaGenerazione.aspx.vb" Inherits="Futuro.WfrmSigmaGenerazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
//<![CDATA[


    function Nascondi() {
        document.all.MainContent_msgInfo.style.visibility = 'visible';
        document.all.MainContent_msgInfo.style.fontSize = '1.6em'
        document.all.MainContent_msgInfo.style.fontWeight = 'bold'
        document.all.MainContent_msgInfo.style.color = '#3a4f63'
        document.all.MainContent_msgInfo.innerText = 'ATTENDERE........';
        document.all.MainContent_cmdElabora.style.visibility = 'hidden';
    }
 
//]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend>GESTIONE FILE SIGMA</legend>
 <fieldset class="ContornoPagina">
<legend>Generazione File</legend>
     <div class="wrapper" style="width:100%">
      <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="GENERAZIONE FILE SIGMA"></asp:Label>
            </h2>
        </div>
        <div class="RigaVuota" >
        &nbsp;

        </div>
     <div class="row" style="height:auto">
             <div class="row" style="height:auto">
                &nbsp;&nbsp;
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
               

        </div>
    </div>
    <div class="row" >
       <div class="collable" style="width:30%"  >
           <asp:Label ID="lblMandato" CssClass="label" AssociatedControlID="ddlMandato" runat="server" Text="Selezionare Mandato da Elaborare"></asp:Label>
       </div>
       <div class="colOggetti" style="width:60%"  >
            
             <asp:DropDownList ID="ddlMandato"
                 runat="server">
             </asp:DropDownList>
       </div> 
       </div>
   
      
     <div class="RigaPulsanti" >
   
     <asp:Label ID="msgConferma" runat="server" AssociatedControlID="msgInfo" CssClass="msgConferma"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
       <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
       <asp:Button id="cmdElabora" CssClass="Pulsante" OnClientClick="Nascondi()"  runat="server" Text="Elabora" ></asp:Button>
    <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
</div>
   </div>
   </fieldset>
 <div class="rowGroup" runat="server" style="height:auto" Id="divDownloadFile" visible="false">
    <div class="RigaVuota" >&nbsp;</div>
    
    </div>
 <fieldset class="ContornoPagina">
<legend>Elenco Generazioni</legend>
 
 <asp:datagrid id="dtgConsultaDocumenti" runat="server" Width="100%"   AllowSorting="True" ToolTip="GENERAZIONE FILE SIGMA" 
								 AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="5"  AutoGenerateColumns="False" UseAccessibleHeader="True">
								<FooterStyle></FooterStyle> 
        
								<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
								<HeaderStyle></HeaderStyle>
								
                                    <Columns>
            <asp:TemplateColumn HeaderText="Dettaglio"> 
                <ItemTemplate>
				    <asp:ImageButton ID="ImageButton1" style="cursor:pointer;" CommandName="Dettaglio" AlternateText="Dettaglio" ToolTip='Dettaglio' runat="server" ImageURL="images/ZoomIn_Small.png" CausesValidation="false"></asp:ImageButton>      
                    </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn Visible="true" HeaderText="N.Gen" DataField="IdGenerazione"></asp:BoundColumn>
             <asp:BoundColumn DataField="CodiceLocaleMandato" HeaderText="Codice Mandato">
		    </asp:BoundColumn>
            <asp:BoundColumn DataField="DataRichiesta" HeaderText="Data Generazione">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="UsernameRichiesta" HeaderText="UserName">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="Esito" HeaderText="Esito">
		    </asp:BoundColumn>
              <asp:TemplateColumn HeaderText="Caricamento"> 
                <ItemTemplate>
				    <asp:ImageButton ID="cmdSi" style="cursor:pointer;" CommandName="Si" AlternateText="Si" ToolTip='Si' runat="server" ImageURL="images/selezionato_small.png" Visible='<%# IIF(Eval("Caricato").ToString().Equals(""), True, False) %>'  CausesValidation="false"></asp:ImageButton>
                     <asp:ImageButton ID="cmdNo" style="cursor:pointer;" CommandName="No" AlternateText="No" ToolTip='No' runat="server" ImageURL="images/deselezionato_small.png" Visible='<%# IIF(Eval("Caricato").ToString().Equals(""), True, False) %>' CausesValidation="false"></asp:ImageButton>            
                    <asp:Label ID="lblStato" runat="server" Visible='<%# IIF(Eval("Caricato").ToString().Equals(""), False, True) %>' Text='<%# IIF(Eval("Caricato").ToString().Equals("1"), "Caricato", "Non Caricato") %>'></asp:Label>
                    </ItemTemplate>
            </asp:TemplateColumn>
               <asp:TemplateColumn HeaderText="Zip"> 
                <ItemTemplate>
				    <asp:ImageButton ID="ImageButton2" style="cursor:pointer;" CommandName="Zip" AlternateText="Scarica Zip" ToolTip='Scarica Zip' runat="server" ImageURL="images/Zip.jpg" Width="30px" Height="30px" CausesValidation="false"></asp:ImageButton>      
                    </ItemTemplate>
            </asp:TemplateColumn>
	        <asp:BoundColumn DataField="Caricato" HeaderText="Caricato" Visible="False">
            </asp:BoundColumn>
	    </Columns>

								<PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>
                   


    <div class="wrapper" style="width:100%;border:0px">
    
   <div class="rowGroup" runat="server" style="height:auto" Id="div1" visible="false">
    <div class="RigaVuota" >&nbsp;
      </div>
     
    
        </div>
        </div>
    <div class="row" >
          <div class="colHyperLink" style="width:100%;" >
            <asp:hyperlink  id="hlScarica1"  runat="server" Visible="false" Target="_blank"/>
        </div>
    </div>
</fieldset>
 <fieldset class="ContornoPagina">
<legend>Elenco File Generati</legend>
    <asp:datagrid id="dtgFile" runat="server" Width="100%"  Caption=""  
                    CellPadding="2" Font-Size="Small"  CssClass="table" 
                    AllowSorting="false" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
	    <Columns>
 <asp:BoundColumn Visible="false" DataField="IdGenerazioneAllegato"></asp:BoundColumn>
 <asp:BoundColumn DataField="Tabella" HeaderText="Tabella">
		    </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Download TXT"> 
                <ItemTemplate>
				    <asp:ImageButton ID="ImageButton1" style="cursor:pointer;" CommandName="DownloadTxt" AlternateText="Scarica Documento in Formato TXT" ToolTip='Scarica Documento in Formato TXT' runat="server" ImageURL="images/TXT.png" Height="30px" Width="30px" CausesValidation="false"></asp:ImageButton>      
                    </ItemTemplate>
            </asp:TemplateColumn>
           
               <asp:TemplateColumn HeaderText="Download CSV"> 
                <ItemTemplate>
				    <asp:ImageButton ID="ImageButton2" style="cursor:pointer;" CommandName="DownloadCsv" AlternateText="Scarica Documento in Formato CSV" ToolTip='Scarica Documento in Formato CSV' runat="server" ImageURL="images/csv.png" Height="30px" Width="30px" CausesValidation="false"></asp:ImageButton>      
                    </ItemTemplate>
            </asp:TemplateColumn>
	    </Columns>
            
    </asp:datagrid>
    <div class="row" >
          <div class="colHyperLink" style="width:100%;" >
            <asp:hyperlink  id="hlScarica"  runat="server" Visible="false" Target="_blank"/>
        </div>
    </div>

   </fieldset> 
   </fieldset>
</asp:Content>
