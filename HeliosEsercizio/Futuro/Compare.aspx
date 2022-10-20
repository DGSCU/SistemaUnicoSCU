<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Compare.aspx.vb" Inherits="Futuro.Compare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="fieldsetrasparente">
            <fieldset class="ContornoPagina">
                <legend id="legTitolo" runat="server"></legend>
                <div>
                    <div id="div1" runat="server" style="margin: auto;width:95%;overflow:auto">
                        <asp:RadioButton id="Radio1" Text="Mostra testo precedente" GroupName="RadioGroup1" runat="server" AutoPostBack="true"/><br />           
                        <asp:RadioButton id="Radio2" Text="Mostra testo attuale" Checked="True" GroupName="RadioGroup1" runat="server" AutoPostBack="true" Width="200px"/>
                        <asp:CheckBox ID="chkVariazioni" runat="server" Text="Mostra variazioni" checked="true" AutoPostBack="true" Width="200px"/>
                        &nbsp;<label ID="label1" runat="server" style="background-color:lightsalmon">Testo eliminato</label>
                        &nbsp;<label ID="label2" runat="server" style="background-color:lightgreen">Testo inserito</label>
                    </div>
                    <div id="divTesto2" runat="server" style="margin: auto;width:95%;height:300px;overflow:auto;border: solid 1px black;font-family: monospace;font-size:medium">
                    </div>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
