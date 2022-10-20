<%@ Page Title="Ricerca Generale Documenti Ente" Language="vb" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaGeneraleDocumentiEnte.aspx.vb" Inherits="Futuro.WfrmRicercaGeneraleDocumentiEnte" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script src="Scripts/jquery-ui-1.11.2/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.11.2/jquery-ui.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jquery-ui-1.11.2/jquery-ui.theme.css" rel="stylesheet" type="text/css"/>
    <script src="Scripts/JHelper.js" type="text/javascript"></script>
    <style type="text/css">
        .ui-datepicker {
            font-size: 11px;
        }
    </style>
    <script  type="text/javascript">
        $(function () {
            var IdData = CostruisciId('txtDataInsFileDal');
            var sharpIdData = "#" + IdData
            $("" + sharpIdData + "").datepicker();
        });
        $(function () {
            var IdData = CostruisciId('txtDataInsFileAl');
            var sharpIdData = "#" + IdData
            $("" + sharpIdData + "").datepicker();
        });
        $(function () {
            var IdData = CostruisciId('txtDataValidazioneFileDal');
            var sharpIdData = "#" + IdData
            $("" + sharpIdData + "").datepicker();
        });
        $(function () {
            var IdData = CostruisciId('txtDataValidazioneFileAl');
            var sharpIdData = "#" + IdData
            $("" + sharpIdData + "").datepicker();
        });
      </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<fieldset class="ContornoPagina" >
    <legend>Ricerca Generale Documenti</legend>
    <div class="wrapper" style="width:100%">
        <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="Ricerca Generale Documenti Ente"></asp:Label>
            </h2>
        </div>
        <div class="RigaVuota" >&nbsp;</div>
        <div class="row" style="height:auto">
             <div class="row" style="height:auto">
                <asp:Label ID="msgErrore" CssClass="msgErrore" runat="server"></asp:Label>
                 <asp:Label ID="msgInfo" CssClass="msgInfo" runat="server"></asp:Label>
                <asp:Label ID="msgConferma" CssClass="msgConferma" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%">
               <asp:Label ID="labellblDenominazioneEnte" CssClass="label" AssociatedControlID="TxtDenominazioneEnte" runat="server" Text="Denominazione Ente"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%"  >
                <asp:TextBox ID="TxtDenominazioneEnte" CssClass="textbox" runat="server" ></asp:TextBox>   
            </div>
            <div class="collable" style="width:15%"  >
               <asp:Label ID="labellblEnte" CssClass="label" AssociatedControlID="TxtCodiceEnte" runat="server" Text="Codice Ente"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%"  >
                <asp:TextBox ID="TxtCodiceEnte" CssClass="textbox" runat="server" ></asp:TextBox>   
            </div>
        </div>
        <div class="row">
            <div class="collable" style="width:15%" >
                <asp:label ID="label1" CssClass="label" AssociatedControlID="ddlTipoDoc" runat="server" Text="Tipo Documento" ></asp:label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlTipoDoc" CssClass="ddlClass"  runat="server"></asp:DropDownList>
            </div>
            <div class="collable" style="width:15%" >
                <asp:label ID="label2" CssClass="lblStatoDocumento" AssociatedControlID="ddlStatoDocumento" runat="server" Text="Stato Documento" ></asp:label>
            </div>
            <div class="colOggetti" style="width:35%">
                <asp:DropDownList ID="ddlStatoDocumento" CssClass="ddlClass"   runat="server">
                    <asp:ListItem Value="">TUTTI</asp:ListItem>
				    <asp:ListItem Value="0">Da Validare</asp:ListItem>
				    <asp:ListItem Value="1">Validato</asp:ListItem>
                    <asp:ListItem Value="2">Non Valido</asp:ListItem>		
                </asp:DropDownList>
            </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%" >
                <asp:label ID="labelRifFase" CssClass="label" AssociatedControlID="TxtRifFase" runat="server" Text="Riferimento Fase" ></asp:label>
            </div>
            <div class="colOggetti" style="width:35%">
                 <asp:TextBox ID="TxtRifFase" CssClass="textbox" runat="server" ></asp:TextBox>   
            </div>
            <div class="collable" style="width:15%" >
                <asp:label ID="labelLblFase" CssClass="label" AssociatedControlID="ddlFase" runat="server" Text="Fase" ></asp:label>
            </div>
            <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlFase" CssClass="ddlClass"   runat="server">
                        <asp:ListItem Value="0">TUTTI</asp:ListItem>
                        <asp:ListItem Value="1">Iscrizione</asp:ListItem>
                        <asp:ListItem Value="2">Adeguamento</asp:ListItem>
                        <asp:ListItem Value="3">Art.2</asp:ListItem>
                        <asp:ListItem Value="4">Art.10</asp:ListItem>		
                    </asp:DropDownList>
            </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%" >
                <asp:label ID="lblStatoFase" CssClass="label" AssociatedControlID="ddlStatoFase" runat="server" Text="Stato Fase" ></asp:label>
            </div>
            <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlStatoFase" CssClass="ddlClass"   runat="server">
                        <asp:ListItem Value="0">TUTTI</asp:ListItem>
                        <asp:ListItem Value="1">Aperta</asp:ListItem>
                        <asp:ListItem Value="-1">Scaduta</asp:ListItem>
                        <asp:ListItem Value="2">Annullata</asp:ListItem>
                        <asp:ListItem Value="3">Presentata</asp:ListItem>
                        <asp:ListItem Value="4">Valutata</asp:ListItem>		
                    </asp:DropDownList>
            </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="LblDataInsFileDal" AssociatedControlID="txtDataInsFileDal"  runat="server" Text="Data Inserimento File Dal"></asp:Label>
            </div>
            <div class="colOggetti" style="width:35%">    
                 <asp:TextBox ID="txtDataInsFileDal" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
           </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblDataInsFileAl" AssociatedControlID="txtDataInsFileAl"  runat="server" Text="Data Inserimento File Al"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">    
                 <asp:TextBox ID="txtDataInsFileAl" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
           </div>
        </div>
        <div class="row" >
            <div class="collable" style="width:15%">
                <asp:Label ID="LblDataValidazioneFileDal" AssociatedControlID="txtDataValidazioneFileDal"  runat="server" Text="Data Validazione File Dal"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">    
                 <asp:TextBox ID="txtDataValidazioneFileDal" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
           </div>
            <div class="collable" style="width:15%">
                <asp:Label ID="LblDataValidazioneFileAl" AssociatedControlID="txtDataValidazioneFileAl"  runat="server" Text="Data Validazione File Al"></asp:Label>
           </div>
            <div class="colOggetti" style="width:35%">    
                 <asp:TextBox ID="txtDataValidazioneFileAl" CssClass="textboxData" MaxLength="10" runat="server"></asp:TextBox>  
           </div>
        </div>
        <div class="RigaPulsanti"> 
            <asp:Button id="cmdRicerca" CssClass="Pulsante"  runat="server" Text="Ricerca"></asp:Button>
            <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi"></asp:Button>
            <asp:Button id="cmdEsportaCSV" CssClass="Pulsante"  runat="server" Text="Esporta CSV" Visible="false"></asp:Button>
             <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp
        </div>
 
        <div class="RigaVuota">&nbsp;</div>
    </div>
    
    <div class="firefox">
    <fieldset class="ContornoPaginaScrollT">
    <asp:datagrid id="dtgConsultaDocumenti" runat="server" 
                    Width="100%" Caption="Elenco Documenti" ToolTip="Elenco Documenti" 
                    AllowPaging="True" CellPadding="2"  CssClass="table" 
                    AutoGenerateColumns="False" UseAccessibleHeader="True">
                <FooterStyle></FooterStyle>
                <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
                <EditItemStyle></EditItemStyle>
                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
                <ItemStyle CssClass="tr"></ItemStyle>
                <HeaderStyle></HeaderStyle>
                <Columns>
                    <asp:TemplateColumn HeaderText="Download"> 
                        <ItemTemplate>
				            <asp:ImageButton ID="ImageButton1" style="cursor:pointer;" CommandName="Download" AlternateText="Scarica Documento" ToolTip='Scarica Documento' runat="server" ImageURL="images/giu_small.png" CausesValidation="false"></asp:ImageButton>      
                            </ItemTemplate>
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" />
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="idente" HeaderText="IdEnte" Visible="False"/>
                    <asp:BoundColumn Visible="False" DataField="IdEnteDocumento"/>
                    <asp:BoundColumn DataField="codiceregione" HeaderText="Codice Ente">
                        <HeaderStyle Width="2%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Denominazione" HeaderText="Ente">
                        <HeaderStyle Width="25%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="IdEnteFase" HeaderText="Rif. Fase">
                        <HeaderStyle Width="5%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="TipoFase" HeaderText="Fase">
                        <HeaderStyle Width="20%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="StatoFase" HeaderText="Stato Fase">
                        <HeaderStyle Width="8%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Prefisso" HeaderText="Tipologia Documento">
                        <HeaderStyle Width="10%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="StatoDocumento" HeaderText="Stato Documento">
                        <HeaderStyle Width="8%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="FileName" HeaderText="Nome File">
                        <HeaderStyle Width="20%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DataInserimento" HeaderText="Data Inserimento">
                        <HeaderStyle Width="5%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="DataStato" HeaderText="Data Validazione">
                        <HeaderStyle Width="5px" />
                    </asp:BoundColumn>
                </Columns>
                <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
            </asp:datagrid>
    </fieldset>
    </div>
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


