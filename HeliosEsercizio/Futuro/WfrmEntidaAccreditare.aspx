<%@ Page Title="Ricerca Enti da Valutare" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmEntidaAccreditare.aspx.vb" Inherits="Futuro.WfrmEntidaAccreditare" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Enti da Valutare</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
        <br />
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
                <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Enti da Valutare"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblDenominazione" CssClass="label" AssociatedControlID="txtdenominazione" runat="server" Text="Denominazione"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtdenominazione" CssClass="textbox" runat="server" MaxLength="200"></asp:TextBox>         
                </div>
                <div class="collable" style="width:13%">      
                    <asp:Label ID="lblCodiceEnte" CssClass="label" AssociatedControlID="txtCodiceEnte" runat="server" Text="Cod. Ente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:12%">       
                    <asp:TextBox ID="txtCodiceEnte" CssClass="textbox" runat="server" MaxLength="7"></asp:TextBox>         
                </div>
                <div class="collable" style="width:13%">      
                    <asp:Label ID="lblTipologia" CssClass="label" AssociatedControlID="ddlTipologia" runat="server" Text="Tipologia"></asp:Label>
                </div>
                <div class="colOggetti" style="width:12%">       
                    <asp:DropDownList ID="ddlTipologia" runat="server" CssClass="ddlClass"></asp:DropDownList>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblClasseAttribuita" CssClass="label" AssociatedControlID="ddlClasseAttribuita" runat="server" Text="Sezione Attribuita"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlClasseAttribuita" runat="server" CssClass="ddlClass"></asp:DropDownList>             
                </div>
                <div class="collable" style="width:13%">      
                    <asp:Label ID="lblClasseRichiesta" CssClass="label" AssociatedControlID="ddlClasseRichiesta" runat="server" Text="Sezione Richiesta"></asp:Label>
                </div>
                <div class="colOggetti" style="width:37%">       
                    <asp:DropDownList ID="ddlClasseRichiesta" runat="server" CssClass="ddlClass" Width="97%"></asp:DropDownList>             
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTipoRicerca" CssClass="label" AssociatedControlID="ddlTipoRicerca" runat="server" Text="Tipo Ricerca"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:DropDownList ID="ddlTipoRicerca" runat="server" CssClass="ddlClass"></asp:DropDownList>             
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="cmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi" />
                <asp:Button ID="CmdEsporta" runat="server" CssClass="Pulsante" Text="Esporta CSV" Visible="False" />
		        <br />
	            <asp:HyperLink ID="ApriCSV1" AccessKey="S" ToolTip="Link per la stampa del risultato della ricerca" CssClass="linkStampa" Text="DOWNLOAD CSV" runat="server" ForeColor="#003399" Visible="False"></asp:HyperLink>&nbsp;  
            </div>
        </div>
        <br />
        <asp:datagrid id="dgRisultatoRicerca" runat="server" Width="100%" ToolTip="Elenco Enti" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
		    <SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
		    <Columns>
                <asp:TemplateColumn HeaderText="Sel"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgEnte" CommandName="Select" ToolTip="Seleziona Ente" AlternateText="Seleziona Ente" runat="server" ImageURL="images/ente-mini.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
			    <asp:BoundColumn DataField="Codiceregione" HeaderText="Cod.Ente">
				    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="Denominazione" HeaderText="Denominazione">
				    <HeaderStyle HorizontalAlign="Center" Width="500px"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center"></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="tipologia" HeaderText="Tipologia">
				    <HeaderStyle HorizontalAlign="Center" Width="20px"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="ClasseAttribuita" HeaderText="Sezione Attribuita">
				    <HeaderStyle HorizontalAlign="Center" Width="20px"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="ClasseRichiesta" HeaderText="Sezione Richiesta">
				    <HeaderStyle HorizontalAlign="Center" Width="20px"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="Nsedi" HeaderText="Nr.Sedi">
				    <HeaderStyle HorizontalAlign="Center" Width="10px"></HeaderStyle>
				    <ItemStyle HorizontalAlign="Center"></ItemStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="http" HeaderText="http://">
				    <HeaderStyle HorizontalAlign="Center" Width="20px"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="email" HeaderText="Email">
				    <HeaderStyle HorizontalAlign="Center" Width="20px"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn Visible="False" DataField="idente" HeaderText="idEnte">
				    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			    </asp:BoundColumn>
			    <asp:BoundColumn DataField="PEC" HeaderText="Pec"></asp:BoundColumn>
		    </Columns>
		    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
        </asp:datagrid>
        <asp:HiddenField ID="txtdenominazione1" runat="server" />
        <asp:HiddenField ID="txttipologia1" runat="server" />
        <asp:HiddenField ID="Txtclasserichiesta1" runat="server" />
        <asp:HiddenField ID="Txtclasseattribuita1" runat="server" />
    </fieldset>
</asp:Content>
