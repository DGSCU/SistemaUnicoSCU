<%@ Page Title="Elenco Manuali" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmManualiUtente.aspx.vb" Inherits="Futuro.WfrmManualiUtente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<fieldset class="ContornoPagina">
    <legend>Elenco Manuali Disponibili</legend>
        <div class="wrapper" style="width:100%;border:0px">
            <div class="RigaVuota" >&nbsp;</div>
                 <br />
                <h3>
                    <asp:label id="lblmessaggio" runat="server"></asp:label>
                </h3>
                <br />
                <asp:datagrid id="dgManuali" runat="server" Width="100%" 
                                ToolTip="Elenco Manuali" CssClass="table"  AllowPaging="False" 
                                AllowSorting="True" AutoGenerateColumns="False" 
                                UseAccessibleHeader="True">
	                <FooterStyle></FooterStyle>
		            <SelectedItemStyle ></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle ></HeaderStyle>
	                <Columns>
                        <asp:BoundColumn Visible="False" DataField="IdManuale" HeaderText="IdManuale"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Download"> 
                            <ItemTemplate>
	                            <asp:ImageButton ID="imgScaricaDoc" style="cursor:pointer;" CommandName="Download" alt="Scarica Documento" ToolTip='Scarica Documento' runat="server" ImageURL="images/giu_small.png" CausesValidation="false"></asp:ImageButton>  
                             </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateColumn>
	                    <asp:BoundColumn Visible="False" DataField="NomeApplicazione" HeaderText="Nome Applicazione"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="Manuale" HeaderText="Manuale">
                            <HeaderStyle Width="35%" />
                        </asp:BoundColumn>
	                    <asp:BoundColumn DataField="Versione" HeaderText="Versione">
                            <HeaderStyle Width="15%" />
                        </asp:BoundColumn>
	                    <asp:BoundColumn DataField="FileName" HeaderText="Nome File"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="HashValue" HeaderText="HashValue" Visible="False"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="Datainserimento" HeaderText="Data Inserimento" 
                            Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="UsernameInserimento" HeaderText="Utente Inseritore" 
                            Visible="False"></asp:BoundColumn>
                    </Columns>
	                <HeaderStyle></HeaderStyle>
                    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                </asp:datagrid>
            <div class="RigaVuota" >&nbsp;</div>
            <div style="vertical-align:middle;">
                <p>
                    <asp:HyperLink ID="hlDownload"  runat="server"  Visible="false"></asp:HyperLink>
                </p>
            </div>
            <br />
            <h3>
                <asp:label id="LblMsgFile" runat="server" CssClass="msgErrore"  ></asp:label>
            </h3>
            <div class="RigaPulsanti">
                 <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
            </div>              
        </div>
    </fieldset>
</asp:Content>
