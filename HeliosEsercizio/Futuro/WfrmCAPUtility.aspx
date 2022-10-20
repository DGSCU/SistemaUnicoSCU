<%@ Page Title="Ricerca CAP/Indirizzi" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WfrmCAPUtility.aspx.vb" Inherits="Futuro.WfrmCAPUtility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ContornoPagina">
<legend >Ricerca CAP/Indirizzi</legend>
<div class="wrapper" style="width:100%">
<div class="headers" >
       <h2>
      <asp:Label ID="Label5" runat="server"  Text="Ricerca CAP/Indirizzi"></asp:Label>
      </h2>
 </div>

       <fieldset class="fieldsetrasparente">
    <div class="row" >
    
        <div class="collable" style="width:100px">
          
        
            <asp:Label ID="Lblidcomune" AssociatedControlID="txtComune"  runat="server"  Text="Comune"></asp:Label>
        
        </div>
        <div class="colOggetti" style="width:200px">
         
  
            <asp:TextBox ID="txtComune" autofocus="true"  runat="server"></asp:TextBox>
       
        </div>
        <div class="collable" style="width:150px">
         

            <asp:RadioButton ID="chkCompleto"    Text="Testo Completo"  GroupName="rd" Checked="true" runat="server" />
       
        </div>
        <div class="collable" style="width:150px">
         
            <asp:RadioButton ID="ChkParziale"  Text="Testo Parziale"  GroupName="rd" runat="server" />
        </div>
  
    </div>

    <div class="row" >
    
       <div class="collable" style="width:100px">

           <asp:Label ID="LblIdIndirizzo"  AssociatedControlID="txtIndirizzo" runat="server" Text="Indirizzo"></asp:Label>
         
       </div>
       <div class="colOggetti" style="width:200px">
          
            <asp:TextBox ID="txtIndirizzo"  runat="server" ></asp:TextBox>&nbsp; 
       </div>
       <div class="collable" style="width:150px">
       <asp:Button ID="CmdRicercaCap" runat="server" CssClass="Pulsante"  Text="Ricerca" />
        
       </div>
      
    
    </div>
        </fieldset>
    <fieldset>
    <div class="row"  >
     
       <div class="collable" style="width:100px">
          
           <asp:Label ID="LblIdCap"  AssociatedControlID="txtCap" runat="server" Text="Cap"></asp:Label>
          
       </div>
       <div class="colOggetti" style="width:200px">
          
           <asp:TextBox ID="txtCap"  runat="server"></asp:TextBox>
       
           
       </div>
          
<div class="collable" style="width:150px">
 <asp:Button ID="CmdRicerca" runat="server" CssClass="Pulsante"  Text="Ricerca" />
</div>
              
     
    </div>
    </fieldset>
    <div class="RigaPulsanti">
        &nbsp;<asp:Button 
            ID="CmdChiudi" CssClass="Pulsante" runat="server"
              Text="Chiudi"  />

   </div>
    <div class="footers">
      <asp:Label ID="Label6" runat="server" Text="">&nbsp;</asp:Label>
    </div>
    

</div>
 
 
 
 
 <fieldset class ="fieldsetrasparente" >
    <div class="RigaVuota" >
        <asp:Label ID="lblmess"  CssClass="msgInfo"  Visible="false" runat="server" Text=""></asp:Label>
    </div>
    
   </fieldset>
  <div>
<asp:datagrid id="dtgTrovaCap" runat="server" Visible="False" CssClass="table" AutoGenerateColumns="False" 
							 AllowSorting="True"  ToolTip="Risultato ricerca cap/inirizzo"
							 Width="100%">
							<FooterStyle ></FooterStyle>
							<SelectedItemStyle  BackColor="White"></SelectedItemStyle>
							<EditItemStyle ></EditItemStyle>
							<AlternatingItemStyle CssClass="tr"></AlternatingItemStyle>
							<ItemStyle CssClass="tr"></ItemStyle>
							<HeaderStyle></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Provincia"  HeaderText="Provincia" ItemStyle-Width="10%" ></asp:BoundColumn>
								<asp:BoundColumn DataField="Comune" HeaderText="Comune" ItemStyle-Width="20%" ></asp:BoundColumn>
								<asp:BoundColumn DataField="Indirizzo" HeaderText="Indirizzo" ItemStyle-Width="35%" ></asp:BoundColumn>
								<asp:BoundColumn DataField="Civici" HeaderText="N&#176;Civici" ItemStyle-Width="20%"   ></asp:BoundColumn>
								<asp:BoundColumn DataField="Cap" HeaderText="Cap" ItemStyle-Width="1%"  ></asp:BoundColumn>
							</Columns>
						</asp:datagrid>

</div>
<p>
    &nbsp;</p>



    
    </fieldset>
</asp:Content>
