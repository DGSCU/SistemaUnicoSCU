<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCOMPGestioneCausaliPaghe.aspx.vb" Inherits="Futuro.WfrmCOMPGestioneCausaliPaghe" %>
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
                var IdData = CostruisciId('TxtDataValuta');
                var sharpIdData = "#" + IdData
                $("" + sharpIdData + "").datepicker();
            });
           </script>
          
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>Gestione Causali&nbsp; &nbsp;</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore"></asp:label>
     
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Inserimento Nuova Causale"></asp:Label>&nbsp;</h2>
            </div>
          </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblCodice" CssClass="label" AssociatedControlID="txtCodice" runat="server" Text="Codice"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                         
                    <asp:TextBox ID="txtCodice" MaxLength="2"  runat="server"></asp:TextBox>
                         
                </div>
                <div class="collable" style="width:15%">

                    <asp:Label ID="lbldescrizione" CssClass="label" AssociatedControlID="TxtDescrizione" 
                      runat="server" Text="Descrizione"></asp:Label>   

                </div> 
                <div class="colOggetti" style="width:35%">       
                          <asp:TextBox ID="TxtDescrizione" MaxLength="255" runat="server"></asp:TextBox>
                </div>
               
            </div>
            
            <div class="row">

              <div class="collable" style="width:15%">

                    <asp:Label ID="lbltipo" CssClass="label" AssociatedControlID="lblTipo" runat="server" Text="Tipo"></asp:Label>   

              </div>

               <div class="colOggetti" style="width:35%"> 
                
                  <asp:DropDownList ID="ddlTipo" runat="server" CssClass="ddlClass">
                       
						<asp:ListItem Value="-1">Debito</asp:ListItem>
						<asp:ListItem Value="1">Credito</asp:ListItem>   
                    </asp:DropDownList>
                   
               </div>

                <div class="collable" style="width:15%">

                </div>
              <div class="colOggetti" style="width:35%">     
                         
              </div>
          
            
        </div>
 </div>
        <div class="RigaPulsanti" style="text-align:right">
              &nbsp;
              <asp:Button ID="CmdInserisci" runat="server" CssClass="Pulsante" 
                  Text="Inserisci" />
              &nbsp;
              &nbsp;
              <asp:Button ID="Chiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />
        
        </div>
        </fieldset>
      
      
     
      <br />
         <fieldset class="ContornoPagina">
        <legend>Causali</legend>

        <p style="text-align:center">
        <asp:label id="lblabilitadisabilita" runat="server" CssClass="bold" Text="Elenco Causali"></asp:label>
    </p>
     <asp:datagrid id="dgAbilitazioni" runat="server" CssClass="table"  Width="100%" 
                 ToolTip="Elenco abilitazioni"  CellPadding="2"  AllowSorting="True" 
                 AutoGenerateColumns="False" AllowPaging="True" PageSize="100" 
                 UseAccessibleHeader="True">
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
          <asp:BoundColumn Visible="False" DataField="IdTipoElemento" HeaderText="IdTipoElemento"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
			
			
           
            <asp:BoundColumn DataField="Codice" HeaderText="Codice" ></asp:BoundColumn>
          
            <asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione" ></asp:BoundColumn>
           
            <asp:BoundColumn DataField="Stato" HeaderText="Stato" ></asp:BoundColumn>

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
         
         
        
			
		</Columns>
		<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>
	</asp:datagrid>

        </fieldset>
          <div class="RigaPulsanti" style="text-align:right">
              &nbsp;&nbsp;&nbsp;&nbsp;
        </div>
</asp:Content>