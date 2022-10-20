<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="informazioniente.aspx.vb" Inherits="Futuro.informazioniente" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="it" lang="it">
<head runat="server">
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <title>Informazioni Ente</title>
    <script language="javascript" type="text/javascript">
        function Stampa() {
            document.all.StampaPagina.style.visibility = 'hidden';
            window.print()
            window.close()
        }
		</script>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset class="ContornoPagina">
    <legend>DETTAGLI ENTE</legend>
    
    <div class="wrapper"  style="width:100%;border-width:0px">
          <div class="row" style="text-align:right">
           <img style="CURSOR: pointer" id="StampaPagina" onclick="javascript: Stampa()" name="StampaPagina"
							alt="Stampa" src="images/printHELIOS.jpg" 
    width="50" height="34"/>
          
          </div>  
             <div class="row" style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Stato Ente:</div>
                <div class="colOggetti" style="width:35%">
    
                    <asp:label id="lblStato" runat="server" Font-Bold="true"  Visible="true"></asp:label>
    
                </div>
                <div class="collable" style="width:15%">
    
                    Competenza:
                 </div>
                <div class="colOggetti" style="width:35%">
    
                   
								<asp:label  id="lblCompetenzaEnte"  Font-Bold="true"   runat="server"  
									Visible="true"></asp:label>
    
                </div>
            </div>
             <div class="row"   style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Ente: 
                 </div>
                <div style="width:85%">
    
                   <asp:label id="lblEnte" runat="server" Font-Bold="true"    Visible="true"></asp:label>
    
                </div>
                
    
             
            </div>
             <div  class="row"  style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Classe/Sezione Richiesta:
                </div>
                <div class="colOggetti" style="width:35%">
    
                    <asp:label id="lblClasseRichiesta"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
                    
                </div>
                <div class="collable" style="width:15%">
    
                   Classe/Sezione Attribuita:</div>
                <div class="colOggetti" style="width:35%">
    
                   
                        <asp:label id="lblClasseAttribuita"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
                        
                </div>
            </div>
             <div  class="row"  style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Codice Fiscale:</div>
                <div class="colOggetti" style="width:35%">
    
                  
                    
                    <asp:label id="lblCodiceFiscale"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
                    
                </div>
                <div class="collable" style="width:15%">
    
                    Data Costituzione:</div>
                <div class="colOggetti" style="width:35%">
    
                    
								<asp:label id="LblDataCostituzione" Font-Bold="true"    runat="server" Visible="true"></asp:label>
    
                </div>
            </div>
             <div class="row"   style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Richiedente Account:</div>
                <div class="colOggetti" style="width:35%">
    
                    
								<asp:label id="LblRichiedenteAccount"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
    
                </div>
                <div class="collable" style="width:15%">
    
                    Tipo:</div>
                <div class="colOggetti" style="width:35%">
    
                    
								<asp:label  id="lblTipo"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
    
                </div>
            </div>
             <div  class="row"  style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Telefono:</div>
                <div class="colOggetti" style="width:35%">
    
                    <asp:label id="lblTelefono"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
    
                </div>
                <div class="collable" style="width:15%">
    
                    Fax:</div>
                <div class="colOggetti" style="width:35%">
    
                   <asp:label id="lblPIVA" runat="server" Font-Bold="true"    Visible="False"></asp:label>
								<asp:label  id="lblFax" Font-Bold="true"    runat="server" Visible="true"></asp:label>
    
                </div>
            </div>
             <div  class="row"  style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Estremi Delibera:</div>
                <div class="colOggetti" style="width:85%">
    
                    <asp:label id="LblEstremiDelibera"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
    
                </div>
                
               
            </div>
             <div  class="row"  style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;HTTP:</div>
                <div class="colOggetti" style="width:35%">
    
                    <asp:label id="lblHTTP" Font-Bold="true"    runat="server" Visible="true"></asp:label>
    
                </div>
                <div class="collable" style="width:15%">
    
                    EMail</div>
                <div class="colOggetti" style="width:35%">
    
                    
								<asp:label  id="lblEmail"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
    
                </div>
            </div>
             <div  class="row"  style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;PEC:</div>
                <div class="colOggetti" style="width:35%">
    
                    
								<asp:label  id="lblEmailCertificata" Font-Bold="true"    runat="server" Visible="true"></asp:label>
    
                </div>
                <div class="collable" style="width:15%">
    
                    Firma:</div>
                <div class="colOggetti" style="width:35%">
    
                   
								<asp:label  id="LblFirma" runat="server"  Font-Bold="true"   Visible="true"></asp:label>
    
                </div>
            </div>
             <div  class="row"  style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Indirizzo:</div>
                <div class="colOggetti" style="width:35%">
    
                    <asp:label id="lblIndirizzo" Font-Bold="true"    runat="server" Visible="true"></asp:label>
    
                </div>
                <div class="collable" style="width:15%">
    
                    Numero:</div>
                <div class="colOggetti" style="width:35%">
    
                    
								<asp:label  id="lblNumero" runat="server"  Font-Bold="true"   Visible="true"></asp:label>
    
                </div>
            </div>
             <div  class="row"  style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Dettaglio Recapito:</div>
                <div class="colOggetti" style="width:85%">
    
                    <asp:label id="LblDettaglioRecapito"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
    
                </div>
               
              
            </div>
             <div  class="row"  style="height:30px">
                <div class="collable" style="width:15%">
    
                    &nbsp;Comune:</div>
                <div class="colOggetti" style="width:35%">
    
                   <asp:label id="lblComune"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
    
                </div>
                <div class="collable" style="width:15%">
    
                    Cap:</div>
                <div class="colOggetti" style="width:35%">
    
                   
								<asp:label  id="lblCAP"  Font-Bold="true"   runat="server" Visible="true"></asp:label>
    
                </div>
            </div>
             <div  class="row"  style="height:250px">
                <div class="collable" style="width:66%;text-align:center;margin-left:33%;">
    <asp:datagrid id="dtgSettori"  runat="server" Width="60%" Font-Names="verdana" Font-Size="Small"
										 ToolTip="Elenco Settori" AllowSorting="True" AutoGenerateColumns="False"
										PageSize="4" GridLines="Horizontal"  BorderWidth="1px">
										<HeaderStyle Font-Bold="true"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="MacroAmbitoAttivit&#224;" HeaderStyle-Font-Size="Medium" HeaderText="Settori di Intervento Accreditati"></asp:BoundColumn>
										</Columns>
										<PagerStyle  Mode="NumericPages"></PagerStyle>
									</asp:datagrid>
                </div>
                
               
            </div>

            <div class="row" style="text-align:right">
                <asp:Button ID="cmdChiudi" CssClass="Pulsante" runat="server" OnClientClick="javascript: window.close();" Text="Chiudi" />
           
            </div>
    
    </div>




    </fieldset>			

    </form>
</body>
</html>
