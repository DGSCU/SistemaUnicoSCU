<%@ Page Title="Conferma Assenze Mensili" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="confermaassenzemensili.aspx.vb" Inherits="Futuro.confermaassenzemensili" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">

        function Conferma() {

            var rsp
            //rsp = confirm("                 " + "\n" +"    *** ATTENZIONE ***" + "\n\n" + "E' la tua 'decisione' definitiva ?" + "\n\n" + "La 'confermiamo' ?")
            rsp = confirm(unescape("%0D*** ATTENZIONE ***%0D%0DE%27 la tua %27decisione%27 definitiva %3F%0D%0DLa %27confermiamo%27 %3F%0D%0D%0D"))
            if (rsp == false) {
                return false;
            }		

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Conferma Assenze Mensili</legend>
        <asp:label id="lblNoVol"  runat="server" CssClass="msgErrore" Visible="False" Text="***ATTENZIONE - Impossibile procedere nessun volontario in servizio***"></asp:label>
        <br />
        <asp:Label ID="lblmessaggiosopra" runat="server" Visible="false" CssClass="msgConferma"></asp:Label>
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Conferma Assenze Mensili"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblEnteLabel" CssClass="label" AssociatedControlID="lblCodEnte" runat="server" Text="Codice Ente"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblCodEnte" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblEnte" CssClass="label" AssociatedControlID="lblDenominazioneEnte" runat="server" Text="Ente"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblDenominazioneEnte" CssClass="bold" runat="server"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblDataOdiernaLabel" CssClass="label" AssociatedControlID="lblDataOdierna" runat="server" Text="Data Odierna"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblDataOdierna" CssClass="bold" runat="server"></asp:label>         
                </div>
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblVisMeseSuccessivo" CssClass="label" AssociatedControlID="lblMeseSuccessivo" runat="server" Text="Mese da Confermare" Visible="false"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblMeseSuccessivo" CssClass="bold" runat="server" Visible="false"></asp:label>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblVisTotAssenze" CssClass="label" AssociatedControlID="lblTotAssenze" runat="server" Text="Totale Assenze" Visible="false"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                    <asp:label id="lblTotAssenze" CssClass="bold" runat="server" Visible="false"></asp:label>         
                </div>
            </div>
        </div>
        <br />
        <div ID="tblConferma" runat="server">
            <p style="text-align:center">
                <asp:Label id="lblAttenzione1" runat="server" CssClass="bold" Text="--- ATTENZIONE ---"></asp:Label>
                <br />
                <asp:Label Id ="lblAttenzione2" runat="server" CssClass="bold" Text="CONFERMA GENERALE ASSENZE/PRESENZE"></asp:Label>
                <br />
                 <asp:Label ID="lblAttenzione3" runat="server" CssClass="bold" Text="MESE/MESI"></asp:Label>&nbsp;<asp:Label id="lblMeseConferma" CssClass="bold" Visible="False" Runat="server"></asp:Label>
            </p>
        <asp:datagrid id="DtgMesiDaInserire" runat="server" CssClass="table" Width="100%" ToolTip="Elenco Assenze Mensili" CellPadding="2" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn DataField="mese1" HeaderText="Mese">
				</asp:BoundColumn>
				<asp:BoundColumn DataField="anno1" HeaderText="Anno">
				</asp:BoundColumn>
                 <asp:TemplateColumn HeaderText="Dettaglio"> 
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgSelect" CommandName="Select" ToolTip="Seleziona Mese" AlternateText="Seleziona Mese" runat="server" ImageURL="images/lenteIngrandimento_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="MeseNum" HeaderText="MeseNum"></asp:BoundColumn>
			</Columns>
		</asp:datagrid>
            <br />
            <p style="text-align:center">
                <asp:Label id="lblAvviso1" runat="server" CssClass="bold" Text="* Si confermano le assenze dei volontari gia' inserite."></asp:Label>
                <br />
                <asp:Label Id ="lblAvviso2" runat="server" CssClass="bold" Text="* Inoltre si confermano le presenze degli altri volontari "></asp:Label>
                <br />
                <asp:Label ID="lblAvviso3" runat="server" CssClass="bold" Text="(o con assenze non rilevanti ai fini dei pagamenti)."></asp:Label>
            </p>
            <div class="RigaPulsanti" style="text-align:right">
                <asp:Button ID="cmdConferma" CssClass="Pulsante" runat="server" Text="Conferma"  OnClientClick="return Conferma()"/>
            </div>
            <br />
       </div>
        <h3>
            <asp:Label Id ="lblRicerca" runat="server"  Text="Lista Mesi Confermati "></asp:Label>
        </h3>
        <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Mesi" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="5" AllowSorting="True" AutoGenerateColumns="False"  UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="False" DataField="IdEnteConfermaAssenze" HeaderText="IdEnteConfermaAssenze"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IdEnte" HeaderText="IdEnte"></asp:BoundColumn>
				<asp:BoundColumn DataField="Anno" HeaderText="Anno"></asp:BoundColumn>
				<asp:BoundColumn DataField="Mese" HeaderText="Mese"></asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato"></asp:BoundColumn>
                <asp:BoundColumn DataField="DataConferma" HeaderText="Data Ora Conferma"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <div class="RigaPulsanti" style="text-align:right">
            <asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        </div>
    </fieldset>
</asp:Content>
