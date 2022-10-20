<%@ Page Title="Verifica controlli aggiuntivi sulla presentazione" Language="vb" AutoEventWireup="false" CodeBehind="WfrmVerificaCompletamentoAccreditamento.aspx.vb" Inherits="Futuro.WfrmVerificaCompletamentoAccreditamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">

<head id="Head1" runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Verifica controlli aggiuntivi sulla presentazione</title>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="fieldsetrasparente">
    <div class="wrapper" style="width:100%">
      <div class="headers" >
               <h2><asp:Label ID="lblTitolo" runat="server"  Text="Verifica controlli aggiuntivi sulla presentazione"></asp:Label></h2>
            </div>
         <div class="rowGroup" style="height:auto">
            <div class="RigaVuota"> &nbsp;</div>
		    <asp:label id="lblmessaggio"  runat="server" CssClass="msgErrore"></asp:label>
             <div class="RigaVuota">  &nbsp; </div>
		</div>

        <div class="row" >
    
       <div class="collable" style="width:15%" >
           <asp:Label ID="lblNote" AssociatedControlID="TxtNote"  runat="server" Text="Anomalie"></asp:Label>
       </div>
       <div class="colOggetti" style="width:85%">
           <asp:TextBox ID="TxtNote" CssClass="textbox"  Enabled="false" TextMode="MultiLine" Rows="10"  runat="server"></asp:TextBox>
       </div>
            
       </div>
       <br />
               <div class="row" >
        <div class="collable" style="width:15%">
          <h3> <asp:Label ID="labelNota" CssClass="labelDati" AssociatedControlID="lblNotaBene" runat="server" Text="Nota Bene:"></asp:Label>
       </h3></div>
       <div class="colOggetti" style="width:85%">
           <h3>   <asp:Label ID="lblNotaBene" CssClass="labelDati" runat="server" Text="Le eventuali anomalie qui riscontrate NON CONSIDERANO i vincoli visualizzati sull'albero. Controllare pertanto anche lo stato del MODELLO SEDI e 
					dei MODELLI relativi alle risorse presenti sull'albero stesso"></asp:Label></h3>
       </div>

       </div>
       <div class="RigaPulsanti">
            <asp:Button ID="imgchiudi" runat="server" CssClass="Pulsante" Text="Chiudi" OnClientClick="javascript:window.close();" />


        </div>
   </div>
   </fieldset>
   </form>
   </body>
   </html>
