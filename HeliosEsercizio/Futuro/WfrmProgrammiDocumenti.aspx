<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmProgrammiDocumenti.aspx.vb" Inherits="Futuro.WfrmProgrammiDocumenti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
        <script type="text/javascript">
            function disableUpload() {
                document.getElementById(CostruisciId("cmdUpload")).enabled = false;
            };
            function infoPrefissi() {
                window.open("wfrmProgrammiInfoPrefissi.aspx?idProgramma=" + <%=Request.QueryString("idProgramma")%> + "", "Elenco", "height=450,width=600,dependent=no,scrollbars=yes,status=no");
            }
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">

<legend>Documenti Programma</legend>
     <div class="wrapper" style="width:100%">
      <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="Documenti Programma"></asp:Label>
            </h2>
        </div>
        <div class="RigaVuota" >&nbsp;
        </div>
     <div class="row" style="height:auto">
     <asp:Label id="lblmsg" runat="server" CssClass="msgErrore"></asp:Label>
    </div>
    <div class="row" >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labellblTitolo" CssClass="label" AssociatedControlID="lblTitolo" runat="server" Text="Titolo"></asp:Label>
       </div>
         <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="lblTitolo" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelLblCodice" CssClass="label" AssociatedControlID="LblCodice"  runat="server" Text="Codice"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="LblCodice" CssClass="labelDati" runat="server"></asp:Label>
       </div>

    
       </div>
    <div class="row" >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelLblStatoProgramma" CssClass="label" AssociatedControlID="LblStatoProgramma" runat="server" Text="Stato Programma"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="LblStatoProgramma" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labelLblStatoIstanza" CssClass="label" AssociatedControlID="LblStatoIstanza"  runat="server" Text="Stato Presentazione Istanza"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="LblStatoIstanza" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" >
         <div class="collable" style="width:15%" >
            <asp:label ID="LblSel" CssClass="label" AssociatedControlID="txtSelFile" runat="server" Text="Selezionare il file" ></asp:label>
        </div>
        <div class="colOggetti" style="width:75%">
            <input ID="txtSelFile" type="file" style="width:95%"  runat="server" />
        </div>

     <div class="collable" style="width:5%" >
            <asp:ImageButton  id="imgPrefissiDocumenti" runat="server" style="cursor:hand" ImageUrl="images/info_small.png" OnClientClick="javascript:infoPrefissi()"  ToolTip="Info Prefissi Documenti" AlternateText="Prefissi Documenti"/>
        </div>
        
    <div class="collable" style="width:5%" >
            <asp:ImageButton  id="imgApplica" runat="server" style="cursor:hand" 
                ImageUrl="images/Icona_Progetto_small.png"  ToolTip="Applica Programma" 
                AlternateText="Applica Programma" Visible="False"/>
        </div>
        
       </div>
              <div class="RigaPulsanti">
            <asp:Button  id="cmdUpload" runat="server" CssClass="Pulsante"  Text="Upload File" OnClientClick="disableUpload()" />
            <asp:Button  id="imgEsporta" runat="server" CssClass="Pulsante"   Text="Esporta Elenco Documenti" />
            <br />
         <asp:hyperlink id="hlDw" runat="server" CssClass="linkStampa" ToolTip="Salva Elenco Documenti" Text="DOWNLOAD CSV" Visible="false" ></asp:hyperlink>

            
            &nbsp;
        </div>
       
       <div class="rowGroup" id="DivPrefissiDocumenti" runat="server" style="height:auto" visible="false">
       </div>
    </div>
    <div class="RigaVuota" >&nbsp;
    </div>
    <asp:datagrid id="dtgAttivitaDocumenti" runat="server" Width="100%"  ToolTip="Elenco Documenti Esistenti" 
								 Caption="Elenco Documenti Esistenti" AllowPaging="True" CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="5" 
                                 AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
								<FooterStyle></FooterStyle>
								<SelectedItemStyle Font-Bold="true" BackColor="White"></SelectedItemStyle>
								<EditItemStyle></EditItemStyle>
								<AlternatingItemStyle  CssClass="tr"></AlternatingItemStyle>
								<ItemStyle CssClass="tr"  HorizontalAlign="Center"  ></ItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
                                    <asp:TemplateColumn HeaderText="Download"> 
                                        <ItemTemplate  >
											<asp:ImageButton ID="ImageButton1"   style="cursor:pointer;" CommandName="Download" AlternateText="Scarica Documento" ToolTip='Scarica Documento' runat="server" ImageURL="images/giu_small.png" CausesValidation="false"></asp:ImageButton>      
                                         </ItemTemplate>
                                    </asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="IdProgrammaDocumento"></asp:BoundColumn>
									<asp:BoundColumn DataField="FileName" HeaderText="Nome File">
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DataInserimento" HeaderText="Data Inserimento">
									</asp:BoundColumn>
									<asp:BoundColumn DataField="hashvalue" HeaderText="Hash Value">
									</asp:BoundColumn>
                                       <asp:TemplateColumn HeaderText="Cancella"> 
                                        <ItemTemplate  >
											<asp:ImageButton ID="IdImgSelVolontari" style="cursor:pointer;" CommandName="Cancella" AlternateText="Cancella Documento" ToolTip='Cancella Documento' 
                                                            runat="server" ImageURL="images/canc_small.png" CausesValidation="false"></asp:ImageButton>      
                                         </ItemTemplate>
                                    </asp:TemplateColumn>
								</Columns>
                                    <PagerStyle NextPageText="&gt;&gt;"   
									PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
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
    <div class="RigaPulsanti" > 
    <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
</div>
</div>
    </fieldset>
</asp:Content>
