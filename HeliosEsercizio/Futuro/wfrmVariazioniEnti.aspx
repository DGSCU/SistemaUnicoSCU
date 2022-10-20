<%@ Page Title="Variazioni Ente" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="wfrmVariazioniEnti.aspx.vb" Inherits="Futuro.wfrmVariazioniEnti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend>Variazioni Ente</legend>
     <div class="wrapper" style="width:100%">
      <div class="headers" >
            <h2>
            <asp:Label ID="Label5" runat="server"  Text="Riepilogo Variazioni Accreditamento Ente"></asp:Label>
            </h2>
        </div>

         <br />

          <div class="row" style="height:auto">
        <asp:label id="lblmessaggio" CssClass="msgErrore"  runat="server" ></asp:label>
        </div>
    <div class="row" >
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labellblEnte" CssClass="label" AssociatedControlID="lblEnte" runat="server" Text="Codice Ente"></asp:Label>
       </div>
         <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="lblEnte" CssClass="labelDati"  runat="server"></asp:Label>
       </div> 
 
       <div class="collable" style="width:15%"  >
           <asp:Label ID="labellblDenominazioneEnte" CssClass="label" AssociatedControlID="lblDenominazioneEnte" runat="server" Text="Denominazione Ente"></asp:Label>
       </div>
       <div class="colOggetti" style="width:35%"  >
              <asp:Label ID="lblDenominazioneEnte" CssClass="labelDati"  runat="server"></asp:Label>
       </div>
       </div>
       <div class="row" >
         <div class="collable" style="width:15%" >
            <asp:label ID="labelRifFase" CssClass="label" AssociatedControlID="lblRifFase" runat="server" Text="Riferimento Fase" ></asp:label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:Label ID="lblRifFase" CssClass="labelDati"  runat="server"></asp:Label>
        </div>
             <div class="collable" style="width:15%" >
            <asp:label ID="labelLblFase" CssClass="label" AssociatedControlID="LblFase" runat="server" Text="Fase" ></asp:label>
        </div>
        <div class="colOggetti" style="width:35%">
                 <asp:Label ID="LblFase" CssClass="labelDati"  runat="server"></asp:Label>
        </div>
       </div>
       <div class="row" >
         <div class="collable" style="width:15%" >
            <asp:label ID="labellblDataInizioFase" CssClass="label" AssociatedControlID="LblStatoFase" runat="server" Text="Stato Fase" ></asp:label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:Label ID="LblStatoFase" CssClass="labelDati"  runat="server"></asp:Label>
        </div>
        <div class="collable" style="width:15%" >
            <asp:label ID="labelDichiarazioneUnica" CssClass="label" AssociatedControlID="DichiarazioneUnica" runat="server" Text="Dich. Impegno Unica" ></asp:label>
        </div>
        <div class="colOggetti" style="width:35%">
            <asp:Label ID="DichiarazioneUnica" CssClass="labelDati"  runat="server"></asp:Label>
        </div>
    </div>

    </div>
    <div class="RigaVuota" >&nbsp;
    </div>
    <asp:GridView id="dtgELENCOVARIAZIONIPadre" runat="server" Width="100%"  Caption="Variazioni Ente Titolare"  AllowPaging="True" ShowHeaderWhenEmpty="true" 
                    CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="5" 
                    AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
	<FooterStyle></FooterStyle>
		<SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		<EditRowStyle></EditRowStyle>
		<AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		<RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		<HeaderStyle></HeaderStyle>
        <PagerStyle CssClass="linkPageGrid"  />
      <PagerSettings  Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	</asp:GridView>
<br />
<asp:GridView id="dtgELENCOVARIAZIONIFIGLI" runat="server" Width="100%"  Caption="Variazioni Enti di Accoglienza"  AllowPaging="True" ShowHeaderWhenEmpty="true" 
                    CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="5" 
                    AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
	<FooterStyle></FooterStyle>
		<SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		<EditRowStyle></EditRowStyle>
		<AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		<RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		<HeaderStyle></HeaderStyle>
        <PagerStyle CssClass="linkPageGrid"  />
      <PagerSettings  Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	</asp:GridView>
 <br />
<asp:GridView id="DtgELENCOVARIAZIONISEDI" runat="server" Width="100%"  Caption="Variazioni Sedi"  AllowPaging="True" ShowHeaderWhenEmpty="true"
                    CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="5" 
                    AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
		<FooterStyle></FooterStyle>
		<SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		<EditRowStyle></EditRowStyle>
		<AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		<RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		<HeaderStyle></HeaderStyle>
        <PagerStyle CssClass="linkPageGrid"  />
      <PagerSettings Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	</asp:GridView>
    <br />
<asp:GridView id="DtgELENCOVARIAZIONIRISORSE" runat="server" Width="100%"  Caption="Variazioni Risorse" AllowPaging="True" ShowHeaderWhenEmpty="true" 
                    CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="5" 
                    AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
			<FooterStyle></FooterStyle>
		<SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		<EditRowStyle></EditRowStyle>
		<AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		<RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		<HeaderStyle></HeaderStyle>
        <PagerStyle CssClass="linkPageGrid"  />
      <PagerSettings Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings >
	</asp:GridView>
        <br />
<asp:GridView id="DtgELENCOVARIAZIONIServizi" runat="server" Width="100%"  Caption="Variazioni Servizi Acquisiti"  ShowHeaderWhenEmpty="true"  AllowPaging="True" 
                    CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="5" 
                    AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
			<FooterStyle></FooterStyle>
		<SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		<EditRowStyle></EditRowStyle>
		<AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		<RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		<HeaderStyle></HeaderStyle>
        <PagerStyle CssClass="linkPageGrid"  />
      <PagerSettings Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings>
	</asp:GridView>
    <br />
<asp:GridView id="DtgELENCOVARIAZIONISistemi" runat="server" Width="100%"  Caption="Variazioni Sistemi"  ShowHeaderWhenEmpty="true"  AllowPaging="True" 
                    CellPadding="2" Font-Size="Small"  CssClass="table" PageSize="5" 
                    AllowSorting="false" AutoGenerateColumns="true" UseAccessibleHeader="True">
			<FooterStyle></FooterStyle>
		<SelectedRowStyle Font-Bold="true" BackColor="White"></SelectedRowStyle>
		<EditRowStyle></EditRowStyle>
		<AlternatingRowStyle  CssClass="tr"></AlternatingRowStyle>
		<RowStyle CssClass="tr"   HorizontalAlign="Center"  ></RowStyle>
		<HeaderStyle></HeaderStyle>
        <PagerStyle CssClass="linkPageGrid"  />
      <PagerSettings Mode="NumericFirstLast" Visible="true" pagebuttoncount="10" position="Bottom"></PagerSettings>
	</asp:GridView>
    <br />
    <div id="esporta" style="text-align:right">
     <asp:Button ID="CmdEsportaElenco" runat="server" CssClass="Pulsante" 
                Text="Esporta CSV"  
              ToolTip="Esportazione elenco variazioni"   />
    
    </div>
   

<div class="RigaVuotaPrint" id="rigaprint" visible="false" runat="server" style="text-align:right">

            
                <br />
             <asp:HyperLink ID="hlApriPadre"   AccessKey="S" 
              ToolTip="Link per l'esportazione delle variazioni Ente Titolare" CssClass="linkStampa" 
            Text="Variazione Ente Titolare" runat="server" 
            ForeColor="#003399" Visible="False"></asp:HyperLink>

             <asp:HyperLink ID="hlApriFigli"   AccessKey="S" 
              ToolTip="Link per l'esportazione delle variazioni Enti di Accoglienza" CssClass="linkStampa" 
            Text="Variazioni Enti di Accoglienza" runat="server" 
            ForeColor="#003399" Visible="False"></asp:HyperLink>

            <asp:HyperLink ID="hlApriSedi"   AccessKey="S" 
              ToolTip="Link per l'esportazione Sedi" CssClass="linkStampa" 
            Text="Variazioni Sedi" runat="server" 
            ForeColor="#003399" Visible="False"></asp:HyperLink>

             <asp:HyperLink ID="hlApriRisorse"   AccessKey="S" 
              ToolTip="Link per l'esportazione delle Risorse" CssClass="linkStampa" 
            Text="Variazione Risorse" runat="server" 
            ForeColor="#003399" Visible="False"></asp:HyperLink>

            <asp:HyperLink ID="hlApriServiziAcquisiti"   AccessKey="S" 
              ToolTip="Link per l'esportazione Servizi Acquisiti" CssClass="linkStampa" 
            Text="Variazione Servizi Acquisiti" runat="server" 
            ForeColor="#003399" Visible="False"></asp:HyperLink>

            <asp:HyperLink ID="hlApriSistemi"   AccessKey="S" 
              ToolTip="Link per l'esportazione Sistemi" CssClass="linkStampa" 
            Text="Variazione Sistemi" runat="server" 
            ForeColor="#003399" Visible="False"></asp:HyperLink>
          &nbsp;
        </div>

<div class="wrapper" style="width:100%;border:0px">
    <div class="RigaPulsanti" > 
    <asp:Button id="cmdChiudi" CssClass="Pulsante"  runat="server" Text="Chiudi" ></asp:Button>
</div>

</div>
    </fieldset>
</asp:Content>
