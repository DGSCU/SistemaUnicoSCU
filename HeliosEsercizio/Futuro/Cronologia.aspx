<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Cronologia.aspx.vb" Inherits="Futuro.Cronologia" EnableEventValidation = "false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend id="legTitolo" runat="server"></legend>
                 <div class="wrapper" style="width: 100%">
                    <asp:GridView ID="gvCronologia" runat="server" CssClass="table" CellPadding="4" 
                         ForeColor="#333333" GridLines="None">
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>
                        <EditRowStyle BackColor="#999999"></EditRowStyle>
                        <AlternatingRowStyle CssClass="tr" BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle CssClass="tr" HorizontalAlign="Center" BackColor="#F7F6F3" 
                            ForeColor="#333333"></RowStyle>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Sel.">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSel" runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkSel" runat="server" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
<%--                    <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>--%>
                    <div class="RigaPulsanti">
                        <asp:Button ID="cmdConfronta" runat="server" CssClass="Pulsante" Text="Confronta" />
                        <br />
                        <asp:Label ID="lblMessaggio" runat="server" CssClass="msgErrore" Visible="false"></asp:Label>
                    </div>
                 </div>               
            </fieldset>
        </div>
    </form>
</body>
</html>
