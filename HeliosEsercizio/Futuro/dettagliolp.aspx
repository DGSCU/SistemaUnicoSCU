<%@ Page Title="Controllo Risorse" Language="vb" AutoEventWireup="false" CodeBehind="dettagliolp.aspx.vb" Inherits="Futuro.dettagliolp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">

<head runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Controllo Risorse</title>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="fieldsetrasparente">
    <div class="wrapper" style="width:100%">
      <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Dettaglio OLP"></asp:Label></h2>
            </div>
         <div class="rowGroup" style="height:auto">
            <div class="RigaVuota"> &nbsp;</div>
		    <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore"></asp:label>
             <div class="RigaVuota">  &nbsp; </div>
		</div>
        <div class="row" >
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="lblNominativo" AssociatedControlID="lblNome"  runat="server" Text="Nome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="lblNome" CssClass="labelDati"  Enabled="false"   runat="server"></asp:Label>
       </div>
             <div class="collable" style="width:15%"  >
           <asp:Label ID="labelCognome" CssClass="label" AssociatedControlID="lblCognome"  runat="server" Text="Cognome"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
           <asp:Label ID="lblCognome" CssClass="labelDati" runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" >
        <div class="collable" style="width:15%">
           <asp:Label ID="labelCodiceFiscale" CssClass="label" AssociatedControlID="lblCodiceFiscale" runat="server" Text="Codice Fiscale"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%">
              <asp:Label ID="lblCodiceFiscale" CssClass="labelDati" runat="server"></asp:Label>
       </div>

       </div>
       <div class="RigaPulsanti">
            <asp:Button ID="imgchiudi" runat="server" CssClass="Pulsante" Text="Chiudi" OnClientClick="javascript:window.close();" />

            <asp:Button ID="cmdEsporta" runat="server" ToolTip="Esporta OLP su sedi diverse" CssClass="Pulsante" 
                Text="Esporta CSV" />
            &nbsp;
            <br />
            <asp:HyperLink ID="ApriCSV1" runat="server" AccessKey="S" CssClass="linkStampa" 
                ForeColor="#003399" Text="DOWNLOAD CSV" 
                ToolTip="Link per la stampa del risultato della ricerca" Visible="False"></asp:HyperLink>
        </div>
   </div>
    <div class="RigaVuota">&nbsp;</div>
    <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" Caption="Elenco risorse su sedi diverse"
		    AllowPaging="true" CellPadding="2"  PageSize="10" CssClass="table" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
	    <FooterStyle></FooterStyle>
	    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
	    <EditItemStyle></EditItemStyle>
	    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
	    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
	    <HeaderStyle></HeaderStyle>
        <Columns>
		    <asp:BoundColumn DataField="codiceente" HeaderText="Codice Progetto">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="titolo" HeaderText="Progetto">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="codicesedeente" HeaderText="Codice Sede">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="comune" HeaderText="Comune">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="indirizzo" HeaderText="Indirizzo">
		    </asp:BoundColumn>
		    <asp:BoundColumn DataField="NumVol" HeaderText="Numero Volontari" DataFormatString="{0:N}">
		    </asp:BoundColumn>
	    </Columns>	
         <PagerStyle CssClass="linkPageGrid" HorizontalAlign="Center" 
                Mode="NumericPages" NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" />				
    </asp:datagrid>
    <div class="RigaVuota">&nbsp;</div>

    <div class="RigaVuota">&nbsp;
    </div>
    </fieldset>
    </form>
</body>
</html>
