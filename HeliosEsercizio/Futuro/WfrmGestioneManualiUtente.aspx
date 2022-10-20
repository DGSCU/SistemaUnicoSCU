<%@ Page Title="Gestione Manuali Utente" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmGestioneManualiUtente.aspx.vb" Inherits="Futuro.WfrmGestioneManualiUtente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
     <fieldset class="ContornoPagina">
        <legend>Gestione Manuali</legend>
        <div class="wrapper" style="width:100%;border:0px">
            <div class="RigaVuota" >&nbsp;</div>
                <asp:datagrid id="dgManuali" runat="server" Width="100%" 
                                ToolTip="Elenco Manuali" CssClass="table"  AllowPaging="True" 
                                AllowSorting="True" AutoGenerateColumns="False" 
                                UseAccessibleHeader="True" >
	                <FooterStyle></FooterStyle>
		            <SelectedItemStyle ></SelectedItemStyle>
		            <EditItemStyle></EditItemStyle>
		            <AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
		            <ItemStyle CssClass="tr"  HorizontalAlign="Center"></ItemStyle>
		            <HeaderStyle ></HeaderStyle>
	                <Columns>
                        <asp:BoundColumn Visible="False" DataField="IdManuale" HeaderText="IdManuale"></asp:BoundColumn>
                      <asp:TemplateColumn HeaderText="Selez."> 
                            <ItemTemplate>
					            <asp:ImageButton ID="imgManuale" CommandName="Seleziona"  ToolTip="Seleziona Manuale" AlternateText="Seleziona Manuale" runat="server" ImageURL="images/documento_small.png" CausesValidation="false"></asp:ImageButton>                         
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
	                    <asp:BoundColumn Visible="False" DataField="NomeApplicazione" HeaderText="Nome Applicazione"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="Manuale" HeaderText="Manuale"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="Versione" HeaderText="Versione"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="FileName" HeaderText="Nome File"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="HashValue" HeaderText="HashValue"></asp:BoundColumn>
	                    <asp:BoundColumn DataField="Datainserimento" HeaderText="Data Inserimento"></asp:BoundColumn>
                        <asp:BoundColumn DataField="UsernameInserimento" HeaderText="Utente Inseritore"></asp:BoundColumn>
                    </Columns>
	                <HeaderStyle></HeaderStyle>
                    <PagerStyle NextPageText="&gt;&gt;" PrevPageText="&lt;&lt;" CssClass="linkPageGrid" Mode="NumericPages"></PagerStyle>
                </asp:datagrid>
            <div class="RigaVuota" >&nbsp;</div>
            <div style="vertical-align:middle;">
                <p>
                    <asp:label ID="LblSel" CssClass="label" AssociatedControlID="txtSelFile" runat="server" Text="Selezionare il file" ></asp:label>
                        <input ID="txtSelFile" type="file" style="width:70%"  runat="server" />
                   <asp:Button  id="cmdUpload" runat="server" CssClass="Pulsante"  Text="Upload File" />&nbsp;
                </p>
            </div>
            <br />
            <h3>
                <asp:label id="LblMsgFile" runat="server" text="Utilizza il pulsante Download per scaricare il manuale" ></asp:label>
            </h3>
             <br />
            <div class="RigaPulsanti">
                 
                 <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />&nbsp;
            </div>              
        </div>
    </fieldset>
</asp:Content>
