<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"  CodeBehind="WfrmStatoApplicazioneDocumentiProgetti.aspx.vb" Inherits="Futuro.WfrmStatoApplicazioneDocumentiProgetti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<meta http-equiv="refresh" content="2"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <fieldset class="ContornoPagina">
<legend >Ricerca Volontari</legend>
<div class="wrapper" style="width:100%">
    <div class="headers">
       <h2>
      <asp:Label ID="lblTitolo" runat="server"  Text="Applicazione Documenti - Progetti "></asp:Label>
      </h2>
      </div>

    <div class="RigaVuota">
       
    &nbsp;

    </div>
   
							
								
	          
					<div class="row" >
						<div class="colOggetti" style="width:35%">
							<img id="imgAttesa" src="images/wait2.gif" width="220" height="220"/>
						</div>
						<div class="colOggetti" style="width:35%">
							<asp:label id="LblInfo" runat="server"> Registrazione Documenti in corso. Si prega di attendere il completamento dell'operazione.</asp:label></td>
					</div>
                    </div>
					<div class="row" >
                    <div class="colOggetti" style="width:35%">
						<asp:label id="LblNumProgRimasti" runat="server">Documenti in lavorazione:</asp:label>
							
                        </div> 
                        <div class="colOggetti" style="width:35%">   
                            <asp:label id="LblTotElab" runat="server" 
								ForeColor="red"></asp:label>
						</div>
					</div>



					<div class="RigaPulsanti">
                                                              <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" Text="Chiudi"  />
		            
                    </div>
		</div>
                </fieldset>
</asp:Content>
