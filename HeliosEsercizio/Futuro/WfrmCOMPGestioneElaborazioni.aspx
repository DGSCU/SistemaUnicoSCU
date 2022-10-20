<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCOMPGestioneElaborazioni.aspx.vb" Inherits="Futuro.WfrmCOMPGestioneElaborazioni" %>
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
                var IdData = CostruisciId('TxtDataValutaDal');
                var sharpIdData = "#" + IdData
                $("" + sharpIdData + "").datepicker();
            });
           </script>
           <script  type="text/javascript">
               $(function () {
                   var IdData = CostruisciId('TxtDataValutaAl');
                   var sharpIdData = "#" + IdData
                   $("" + sharpIdData + "").datepicker();
               });
           </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<fieldset class="ContornoPagina">
        <legend>Gestione Elaborazioni</legend>
        <asp:label id="lblmess"  runat="server" CssClass="msgErrore" Visible="False"></asp:label>
       <div class="wrapper" style="width:100%;border:0px">
                <div class="rowGroup" style="height:auto"> 
                    <div class="row" style="height:auto">                  
                        <div class="colHyperLink" style="width:49%">
                        </div>
                        <div class="colHyperLink" style="width:49%;float:right;text-align:right">
                            <asp:LinkButton ID="HplInserisciElaborazione" runat="server" style="cursor:pointer;" Text="Nuova Elaborazione" Visible="true"></asp:LinkButton>
                        </div>
                      
                     </div>
                </div>
            </div>
        <br />
        <div class="wrapper" style="width:100%">
            <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Ricerca Elaborazioni"></asp:Label></h2>
            </div>
          </div>
        <div class="wrapper" style="width:100%">
            <div class="RigaVuota" >&nbsp;</div>
            <div class="row">
                <div class="collable" style="width:15%">
                    <asp:Label ID="lblTipo" CssClass="label" AssociatedControlID="cboTipo" runat="server" Text="Tipo"></asp:Label>   
                </div> 
                <div class="colOggetti" style="width:35%">       
                         
                    <asp:DropDownList ID="cboTipo" runat="server">
                    </asp:DropDownList>
                         
                </div>
                <div class="collable" style="width:15%">

                    <asp:Label ID="lblStato" CssClass="label" AssociatedControlID="CboStato" 
                      runat="server" Text="Stato"></asp:Label>   

                </div> 
                <div class="colOggetti" style="width:35%">       
                         
                 <asp:DropDownList ID="CboStato" runat="server">
                    </asp:DropDownList>
                
                </div>
            </div>
            
            <div class="row">

              <div class="collable" style="width:15%">

                    <asp:Label ID="lblDataValutaDal" CssClass="label" AssociatedControlID="TxtDataValutaDal" runat="server" Text="Data Valuta Dal"></asp:Label>   

              </div>

               <div class="colOggetti" style="width:35%"> 
                
                    <asp:TextBox ID="TxtDataValutaDal" runat="server"></asp:TextBox>
                         
               </div>

                <div class="collable" style="width:15%">

                    <asp:Label ID="lblDataValutaAl" CssClass="label" 
                        AssociatedControlID="TxtDataValutaAl" runat="server" Text="Data Valuta Al"></asp:Label>   

                </div>
              <div class="colOggetti" style="width:35%">     
                    <asp:TextBox ID="TxtDataValutaAl" runat="server"></asp:TextBox>
                         
              </div>
          
            
        </div>
 </div>
 <br />
        <div class="RigaPulsanti" style="text-align:right">

              <asp:Button ID="CmdRicerca" runat="server" CssClass="Pulsante" Text="Ricerca" />&nbsp;<asp:Button ID="cmdChiudi" runat="server" CssClass="Pulsante" Text="Chiudi" />&nbsp;</div>

         <p style="text-align:center">
        <asp:label id="lblTitotloRicerca" runat="server" CssClass="bold" Text="Elenco Elaborazioni"></asp:label>
         </p>

         <asp:datagrid id="dgRisultatoRicercaElaborazioni" runat="server" CssClass="table"  
            Width="100%" ToolTip="Elenco Elaborazioni"  CellPadding="2"  
            AllowSorting="True" AutoGenerateColumns="False"   
            UseAccessibleHeader="True">
        <SelectedItemStyle BackColor="White"></SelectedItemStyle>
		<EditItemStyle></EditItemStyle>
		<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		<ItemStyle CssClass="tr" HorizontalAlign="Center"></ItemStyle>
		<HeaderStyle></HeaderStyle>
		<Columns>
            <asp:TemplateColumn> 
                <ItemTemplate>
					<asp:ImageButton ID="CmdModifica" CommandName="Modifica" ToolTip="Seleziona Elaborazione" AlternateText="Seleziona Elaborazione" runat="server" ImageURL="images/Icona_Progetto_small.png" CausesValidation="false"></asp:ImageButton>                         
                </ItemTemplate>
            </asp:TemplateColumn>
			<asp:BoundColumn Visible="true" DataField="idElaborazione" HeaderText="Cod.Elaborazione"
				DataFormatString="{0:d}">
				<HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center"></ItemStyle>
			</asp:BoundColumn>
           
			<asp:BoundColumn DataField="Tipo" HeaderText="Tipo">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione">
				<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>
			<asp:BoundColumn DataField="DataValuta" HeaderText="Data Valuta" DataFormatString="{0:dd/MM/yyyy}">
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
			<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundColumn>
			<asp:BoundColumn DataField="StatoElaborazione" HeaderText="Stato">
				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
			</asp:BoundColumn>    
			
		</Columns>
		<%--<PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" HorizontalAlign="Center" CssClass="linkPageGrid"  Mode="NumericPages"></PagerStyle>--%>
	</asp:datagrid>


   
        </fieldset>
         <div class="RigaPulsanti" style="text-align:right">
            
             &nbsp;&nbsp;&nbsp;&nbsp;
        </div>
</asp:Content>
