<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneUtenzeEnte.aspx.vb" Inherits="Futuro.WfrmGestioneUtenzeEnte" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
        <legend>Gestione utenze ente</legend>
            <asp:Panel ID="pnlInserimento" runat="server" Visible="true" Width="100%"> 
                <div class="wrapper" style="width:100%">
                    <div class="headers" >
                        <h2><asp:Label ID="lblTitoloPrincipale" runat="server"  Text="Ricerca utenze ente"></asp:Label></h2>
                    </div>
                    <div class="RigaVuota">&nbsp;</div>
                    <div class="row" style="height:auto">
                    <asp:Label ID="lblMessaggioConferma"  CssClass="msgConferma" runat="server"></asp:Label>
                    <asp:Label ID="lblerrore" CssClass="msgErrore" runat="server"></asp:Label>
                    </div>
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
                    <div class="row" >
                    <div class="collable" style="width:15%">      
                            <asp:Label ID="lblCodiceFiscale" CssClass="label" AssociatedControlID="txtCodiceFiscale" runat="server" Text="Codice Fiscale"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:TextBox ID="txtCodiceFiscale" CssClass="textbox" runat="server" MaxLength="16"></asp:TextBox>         
                        </div>
                        <div class="collable" style="width:15%">      
                            <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="ddlStato" runat="server" Text="Stato"></asp:Label>
                        </div>
                        <div class="colOggetti" style="width:35%">       
                            <asp:DropDownList ID="ddlStato" runat="server" CssClass="ddlClass">
                                <asp:ListItem Value=""> Selezionare... </asp:ListItem>
                                <asp:ListItem Value="0"> Disabilitato </asp:ListItem>
                                <asp:ListItem Value="1"> Abilitato </asp:ListItem>
                            </asp:DropDownList> 
                            <asp:TextBox ID="TxtStato" CssClass="textbox" runat="server" visible="false" ReadOnly="true"> </asp:TextBox>
                        </div>
                     </div>
                     <div class="row" >
                     <asp:TextBox ID="TxtHdTipoOperazione" CssClass="textbox" runat="server" visible="false" ReadOnly="true"> </asp:TextBox>
                     <asp:TextBox ID="TxtHdIdDelegato" CssClass="textbox" runat="server" visible="false" ReadOnly="true"> </asp:TextBox>
                     <asp:TextBox ID="TxtHdIdEnte" CssClass="textbox" runat="server" visible="false" ReadOnly="true"> </asp:TextBox>
                   
                     </div>
                </div>
                 </asp:Panel>
       
            <div class="RigaPulsanti" style="text-align:right">
                <asp:Button ID="cmdNuovo" runat="server" CssClass="Pulsante" Text="Nuovo" />&nbsp;
                 <asp:Button ID="imgRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;
                <asp:Button ID="CmdSalva" runat="server" CssClass="Pulsante" Text="Salva" Visible="False" />&nbsp
                <asp:Button ID="CmdElimina" runat="server" CssClass="Pulsante" Text="Elimina" Visible="False" />&nbsp
                <asp:Button ID="CmdAnnulla" runat="server" CssClass="Pulsante" Text="Annulla" Visible="False" />&nbsp
                 <asp:Button ID="CmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
            </div>
         <br />
          <%-- <div class="wrapper" style="width:100%">--%>
            <div class="RigaVuota">&nbsp;</div>               
                <asp:datagrid id="dtgProfiliUtente" runat="server" Caption="ELENCO PROFILI"  CssClass="table" Visible="false"  Width="100%" ToolTip="Elenco Profili Utente"  CellPadding="2"  AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
		        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		        <EditItemStyle></EditItemStyle>
		        <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		        <ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		        <HeaderStyle></HeaderStyle>
		        <Columns>
			        <asp:BoundColumn  DataField="idprofilo" Visible="False">
                    </asp:BoundColumn>
			        <asp:BoundColumn DataField="Descrizione" HeaderText="Tipo Profilo">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
			        <asp:TemplateColumn HeaderText="Sel.">
				        <HeaderStyle Width="20%" ></HeaderStyle>
				        <ItemTemplate>
					        <asp:CheckBox id="chkSeleziona" Text="Sel" TextAlign="Left" toolTip="Seleziona Profili" runat="server"></asp:CheckBox>
				        </ItemTemplate>
			        </asp:TemplateColumn>
		        </Columns>
	        </asp:datagrid>
                <br />
          <%--  <div class="row">
            <div class="colOggetti" style="width:15%">
                        <asp:RadioButton ID="optUtenzaProfilo" GroupName="Utenza" runat="server" Checked="True" Text="Tipo Profilo" AutoPostBack="true"/>
                    </div>
       
                <div class="colOggetti" style="width:15%">
                    <asp:RadioButton ID="optUtenzaProfiloa" GroupName="Utenza" runat="server" Checked="True" Text="Tipo Profilo" AutoPostBack="true"/>
                </div>
                <div class="colOggetti" style="width:35%">       
                    <asp:dropdownlist id="cboProfili" runat="server" CssClass="ddlClass" ToolTip="Tipo Profilo" Visible="false"></asp:dropdownlist>    
                </div>
                <div class="colOggetti" style="width:15%">
                    <asp:RadioButton ID="optUtenzaSimile" GroupName="Utenza" runat="server" Text="Profilo Simile" AutoPostBack="True"/>
                </div>
                <div class="colOggetti" style="width:35% ">
                    <asp:textbox id="txtTipoUtenzaSim" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Small" Width="5%" ReadOnly="True" ToolTip="Profilo Simile"></asp:textbox>       
                    <asp:TextBox id="txtUtenzaSim" CssClass="textbox" runat="server" Width="75%" ToolTip="Profilo Simile"></asp:TextBox> 
                    <asp:imagebutton id="cmdRicerca" AlternateText="Ricerca Utenze" ToolTip="Ricerca Utenze" style="CURSOR: hand; vertical-align:middle" runat="server" ImageUrl="images/lenteIngrandimento_small.png" ></asp:imagebutton>        
                </div>
            </div>--%>
          
      <%--  </div>--%>








          <span>
            <asp:label id="lblMess"  runat="server" CssClass="msgErrore"></asp:label>
        </span>
         <div class="row" id="DivGriglia" runat="server">

        <asp:datagrid id="dtgRisultatoRicerca" runat="server" Width="100%"  Caption="Elenco utenze ente" ToolTip="Elenco utenze ente" CssClass="table" CellPadding="2"  AllowPaging="True" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" UseAccessibleHeader="True">
			<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
		    <EditItemStyle></EditItemStyle>
		    <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		    <ItemStyle CssClass="tr" HorizontalAlign="Center" ></ItemStyle>
		    <HeaderStyle></HeaderStyle>
			<Columns>
                <asp:TemplateColumn HeaderText="Seleziona" ItemStyle-Width="10%"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgVolontario" CommandName="Select" ToolTip="Seleziona Nominativo" AlternateText="Seleziona Nominativo" runat="server" ImageURL="images/Icona_Volontario_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
				<asp:BoundColumn Visible="False" DataField="idEnteUtente" HeaderText="idEnteUtente"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="IDEnte" HeaderText="IDEnte"></asp:BoundColumn>
				<asp:BoundColumn DataField="Cognome" HeaderText="Cognome" ItemStyle-Width="18%"></asp:BoundColumn>
				<asp:BoundColumn DataField="Nome" HeaderText="Nome" ItemStyle-Width="18%">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="CodiceFiscale" HeaderText="Codice Fiscale" ItemStyle-Width="18%">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="Stato" HeaderText="Stato" ItemStyle-Width="18%">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:BoundColumn DataField="UltimoAccesso" HeaderText="Ultimo Accesso" ItemStyle-Width="18%">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
                <%-- <asp:TemplateColumn HeaderText="Abilita"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnAbilita" CommandName="Abilita" ToolTip="Abilita" AlternateText="Abilita" runat="server" ImageURL="images/selezionato_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>
                 <asp:TemplateColumn HeaderText="Disabilita"> 
                    <ItemTemplate>
					    <asp:ImageButton ID="ImgBtnDisabilita" CommandName="Disabilita" ToolTip="Disabilita" AlternateText="Disabilita" runat="server" ImageURL="images/deselezionato_small.png" CausesValidation="false"></asp:ImageButton>                         
                    </ItemTemplate>
                </asp:TemplateColumn>--%>
			</Columns>
			<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
		</asp:datagrid>
       </div>
        
         </fieldset>
</asp:Content>
