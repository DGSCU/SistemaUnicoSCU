<%@ Page Title="Ricerca Utenze" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="disabilitautenze.aspx.vb" Inherits="Futuro.disabilitautenze" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Ricerca Utenze</legend>
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Utenze"></asp:Label></h2>
            </div>
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblTipologiaUtenza" CssClass="label" runat="server" Text="Tipologia Utenza"></asp:Label>
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:RadioButton ID="rdbU" Text="U" GroupName="TipoUtenza" runat="server" Checked="False" AutoPostBack="true" Enabled="false"/>
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:RadioButton ID="rdbR" Text="R" GroupName="TipoUtenza" runat="server" AutoPostBack="true" Enabled="False"/>
                </div>
                <div class="colOggetti" style="width:10%">
                    <asp:RadioButton ID="rdbTutti" Text="Tutti" GroupName="TipoUtenza" runat="server" Checked="False" Enabled="False" AutoPostBack="true"/>
                </div>
            </div>
        </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblCognome" CssClass="label" AssociatedControlID="txtCognome" runat="server" Text="Cognome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtCognome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblNome" CssClass="label" AssociatedControlID="txtNome" runat="server" Text="Nome"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtNome" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row">
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblUtente" CssClass="label" AssociatedControlID="txtutente" runat="server" Text="Utente"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:textbox id="txtTipo" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small" Width="5%" ReadOnly="True" ToolTip="Tipo Utente"></asp:textbox>       
                    <asp:TextBox ID="txtutente" CssClass="textbox" runat="server" Width="83%"></asp:TextBox>         
                </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblEmail" CssClass="label" AssociatedControlID="txtEmail" runat="server" Text="E-Mail"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="txtEmail" CssClass="textbox" runat="server"></asp:TextBox>         
                </div>
            </div>
            <div class="row" >
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlSelezionaStato" runat="server" Text="Stato"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">
                    <asp:DropDownList ID="ddlSelezionaStato" runat="server" CssClass="ddlClass">
                        <asp:ListItem Value="0" Selected="True">Selezionare</asp:ListItem>
						<asp:ListItem Value="1">Abilitato</asp:ListItem>
						<asp:ListItem Value="2">Disabilitato</asp:ListItem>   
                    </asp:DropDownList>
               </div>
                <div class="collable" style="width:15%">      
                    <asp:Label ID="lbldominio" CssClass="label" AssociatedControlID="TxtDominio" runat="server" Text="Dominio"></asp:Label>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:TextBox ID="TxtDominio" CssClass="textbox" runat="server" Enabled="false"></asp:TextBox>         
                </div>
            </div>
            <div class="RigaPulsanti">
                <asp:Button ID="imgRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />
                <asp:Button ID="imgChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
            </div>
        </div>
        <br />
        <span>
            <asp:label id="lblMess"  runat="server" CssClass="msgErrore"></asp:label>
        </span>
        <br />
        <br />
        <asp:datagrid id="dtgUtenze" runat="server" Width="100%" ToolTip="Elenco Volontari" CssClass="table" CellPadding="2"  AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnModifica" CommandName="Modifica" ToolTip="Modifica" AlternateText="Modifica" runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="IdUtente" HeaderText="IdUtente">
					<HeaderStyle Width="0%"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Nominativo" HeaderText="Nominativo">
					<HeaderStyle Width="40%"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="UserName" HeaderText="Utenza">
					<HeaderStyle Width="25%"></HeaderStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato">
					<HeaderStyle Width="15%"></HeaderStyle>
				</asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Abilita"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnAbilita" CommandName="Abilita" ToolTip="Abilita" AlternateText="Abilita" runat="server" ImageURL="images/selezionato_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="Disabilita"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnDisabilita" CommandName="Disabilita" ToolTip="Disabilita" AlternateText="Disabilita" runat="server" ImageURL="images/deselezionato_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="Inoltra" HeaderText="Inoltra Account">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="Tipo" HeaderText="Tipo"></asp:BoundColumn>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
        <asp:HiddenField ID="chkInoltro" runat="server" />
        <asp:HiddenField ID="idutente" runat="server" />
        <asp:HiddenField ID="tipoutente" runat="server" />
        <asp:HiddenField ID="nomeutente" runat="server" />
        <asp:HiddenField ID="cognomeutente" runat="server" />
    </fieldset>
</asp:Content>
