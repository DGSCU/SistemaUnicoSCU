<%@ Page Title="RicercaUtenzaSede" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmRicercaUtenzeSedi.aspx.vb" Inherits="Futuro.WfrmRicercaUtenzeSedi" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Profilo: gestore degli operatori volontari per sede</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca sedi"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" id="divSede" runat="server">
                 <div class="collable" style="width:15%">      
                    <asp:Label ID="LblDenominazione" CssClass="label" AssociatedControlID="TxtDenSede" runat="server" Text="Denominazione Sede"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtDenSede" autofocus="true" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblIdEnteSedeAtt" CssClass="label" AssociatedControlID="txtIdEnteSedeAttuazione" runat="server" Text="Cod. Sede"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtIdEnteSedeAttuazione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" id="divComune" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblComune" CssClass="label" AssociatedControlID="txtComune" runat="server" Text="Comune"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtComune" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblProvincia" CssClass="label" AssociatedControlID="TxtProvincia" runat="server" Text="Provincia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtProvincia" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" id="divReg" runat="server">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblRegione" CssClass="label" AssociatedControlID="TxtRegione" runat="server" Text="Regione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtRegione" CssClass="textbox" runat="server"></asp:TextBox>         
                </div> 
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblStatoUtenza" CssClass="label" AssociatedControlID="ddlStatoUtenza" runat="server" Text="Stato Utenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlStatoUtenza" runat="server">
                        <asp:ListItem Value="-1">Tutti</asp:ListItem>
                        <asp:ListItem Value="0">Non Attiva</asp:ListItem>
                        <asp:ListItem Value="1">Attiva</asp:ListItem>
                    </asp:DropDownList>
                </div> 
            </div>
            <div class="row" id="divNominativo" runat="server" visible="false">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="LblNome" CssClass="label" AssociatedControlID="TxtNome" runat="server" Text="Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>
              &nbsp;
            </div>
        </div>
        <br />
        <h3>
            <asp:label id="lblmessaggio" runat="server" AssociatedControlID="dgSedi"></asp:label>
        </h3>
        <br />
            <asp:datagrid id="dgSedi" runat="server" Width="100%" 
                    ToolTip="Elenco Sedi" CssClass="table"  AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False"  Visible="False" 
                    UseAccessibleHeader="True">
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
                    <asp:TemplateColumn HeaderText="Selez."> 
                    <ItemTemplate>
	                    <asp:ImageButton ID="IdImgSelSedi" style="cursor:pointer;" CommandName="Select" alt="Seleziona Sede" ToolTip='Seleziona Sede' runat="server" ImageURL="images/sedi_small.png" CausesValidation="false"></asp:ImageButton>      
                        </ItemTemplate>
                    </asp:TemplateColumn>
        		    <asp:BoundColumn DataField="identesedeattuazione" HeaderText="Codice Sede">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Denominazionesede" HeaderText="Sede">
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Comune" HeaderText="Comune"></asp:BoundColumn>
                    <asp:BoundColumn DataField="regione" HeaderText="Regione"></asp:BoundColumn>

        		    <asp:BoundColumn DataField="StatoUtenza" HeaderText="Stato Utenza">
                    </asp:BoundColumn>

        		</Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
	    </asp:datagrid>
        <p>&nbsp;</p>
    </fieldset>
</asp:Content>
