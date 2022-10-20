<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmInserimentoMassivoRuoliAntimafia.aspx.vb" Inherits="Futuro.WfrmInserimentoMassivoRuoliAntimafia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
    //<![CDATA[


        function Nascondi() {
            document.all.MainContent_lblMessaggioErrore.style.visibility = 'visible';
            document.all.MainContent_lblMessaggioErrore.style.fontSize = '1.6em'
            document.all.MainContent_lblMessaggioErrore.style.fontWeight = 'bold'
            document.all.MainContent_lblMessaggioErrore.style.color = '#3a4f63'
            document.all.MainContent_lblMessaggioErrore.innerText = 'ATTENDERE........';
            document.all.MainContent_CmdElabora.style.visibility = 'hidden';
        }

        function toggleRuoliAntimafiaAmmessi() {
            var x = document.getElementById("divRuoliAntimafiaAmmessi");
            var y = document.getElementById("btnRuoliAntimafiaAmmessi");
            if (x.style.display === "none") {
                x.style.display = "block";
                y.innerHTML = "Nascondi elenco Ruoli Antimafia ammessi"
            } else {
                x.style.display = "none";
                y.innerHTML = "Visualizza elenco Ruoli Antimafia ammessi"
            }
        } 

    //]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend id="lgContornoPagina" runat="server">Inserimento Massivo Ruoli Antimafia</legend>
        <asp:Label ID="lblMessaggio" runat="server" CssClass="msgErrore" Visible="false"></asp:Label>
        <br />
        <div runat="server" id="divPrincipale">
            <div id="divrisultatoImportazione" runat="server">
                <div class="headers">
                    <h2>
                        <asp:Label ID="Label1" runat="server"  Text="Risultato Import Ruoli Antimafia"></asp:Label>
                    </h2>
                </div>
                <p>
                    <asp:label id="lblEsito"  CssClass="bold" runat="server">LA VERIFICA DEI DATI IMMESSI NEL FILE CSV RISULTA CORRETTA. PER SALVARE DEFINITIVAMENTE I DATI PREMERE IL TASTO CONFERMA.</asp:label>
                </p>
                <p>
                    <asp:label id="lblTotali" CssClass="bold" runat="server" ></asp:label>
                </p>
                <p>
                    <asp:linkbutton id="lnkScarica" CssClass="linkStampa" runat="server" Text="Scarica il file completo nel formato CSV" />  
                </p>
                <p class="RigaPulsanti">
                    <asp:Button ID="CmdConferma"  OnClientClick="Nascondi()" CssClass="Pulsante" runat="server" Text="Conferma"/>&nbsp;&nbsp;
                    <asp:Label ID="Label2" CssClass="msgErrore" runat="server" Text=""></asp:Label>
                    <asp:Button ID="Button1" CssClass="Pulsante" runat="server" Text="Chiudi"/>
                </p>
                <div id="divgriglia" runat="server">
                    <asp:datagrid id="dgRisultatoImportazione" runat="server" Width="100%" Caption="Risultato Elaborazione"
                            ToolTip="Risultato Elaborazione" CssClass="table"  AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" 
                            UseAccessibleHeader="True">
		                <EditItemStyle></EditItemStyle>
		                <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		                <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		                <HeaderStyle></HeaderStyle>
                        <Columns>
				            <asp:BoundColumn DataField="N.ro riga" HeaderText="N.ro riga"></asp:BoundColumn>
				            <asp:BoundColumn DataField="Errori" HeaderText="Errori" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
				            <asp:BoundColumn DataField="CodiceFiscaleEnte" HeaderText="CodiceFiscaleEnte"></asp:BoundColumn>
				            <asp:BoundColumn DataField="CodiceFiscale" HeaderText="CodiceFiscale"></asp:BoundColumn>
				            <asp:BoundColumn DataField="RuoloAntimafia" HeaderText="RuoloAntimafia"></asp:BoundColumn>
                        </Columns>
			            <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
		            </asp:datagrid>
                </div>
            </div>
            <div class="wrapper" style="width: 100%" id="inizio" runat="server">
                <div class="headers" >
                    <h2><asp:Label ID="lblTitolo" runat="server" Text="Import ruoli Antimafia"></asp:Label></h2>
                </div>
                    <div id="divImport" runat="server">
                        <div class="RigaVuota">&nbsp;<asp:Label ID="lblMessaggioErrore" CssClass="msgErroreBig" runat="server" Text=""></asp:Label></div>
                        <p> 
                            <asp:Label ID="lblTipoImport" runat="server"  Text="Scaricare il file dei Ruoli Antimafia cliccando qui"></asp:Label>&nbsp;&nbsp;&nbsp;
                            <asp:HyperLink ID="hplFileRicerca" NavigateUrl="~/WfrmEsportazioneRuoliAntimafiaTitolare.aspx" runat="server" Text="File RuoliAntimafia.csv"></asp:HyperLink> <br />
                            <asp:Label ID="LblUp" runat="server"  Text="Eseguire l'Upload del file completo"></asp:Label>
                            <asp:FileUpload ID="fupFile" runat="server" ToolTip="File di Upload" 
                                style="width:40%" />&nbsp;
                            <asp:Button ID="CmdElabora"  OnClientClick="Nascondi()" CssClass="Pulsante" runat="server" Text="Elabora" />
                            <asp:Button ID="btnChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                        </p>
                    </div>
                <br />
                <fieldset> 
                    <strong>Note:</strong>
                    <h3 class="text-danger">Attenzione! Non inserire il carattere <strong>;</strong> in nessun campo</h3><br />
                    <h3>Il campo RuoloAntimafia va compilato con uno dei valori previsti.</h3>
                    <div class="row">
                        <button type="button" id="btnRuoliAntimafiaAmmessi" class="collapsible" onclick="toggleRuoliAntimafiaAmmessi()">Visualizza elenco Ruoli Antimafia ammessi</button>
                        <div id="divRuoliAntimafiaAmmessi" style="display:none">
                            <% Call RuoliAntimafiaAmmessi()%>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="wrapper" style="width: 100%" id="divChiudi" visible="false" runat="server">
                <br /><asp:Label ID="lblDisabilitato" CssClass="msgErrore" runat="server" Text="" Enabled="false"></asp:Label><br />
                <div class="RigaPulsanti">
                    <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" ToolTip="Chiudi" Text="Chiudi" style="text-align:right;"/>
                </div>
            </div>
       </div>
       <asp:HiddenField ID="hdsRisultatoElaborazione" runat="server" />
       <asp:HiddenField ID="hIdEnteFaseAntimafia" runat="server" />
    </fieldset>
</asp:Content>
