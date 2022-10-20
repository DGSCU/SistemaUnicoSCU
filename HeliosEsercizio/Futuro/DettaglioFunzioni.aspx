<%@ Page Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="DettaglioFunzioni.aspx.vb" Inherits="Futuro.DettaglioFunzioni" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent" >
  

      <fieldset class="FDSsottoMenu">
        <legend><asp:Label ID="lblsottomenudi" AssociatedControlID="MenuHelios" runat="server" Text=""></asp:Label></legend>

<div id="iscrizione_titolare" style="max-width: 800px;" runat="server">
    <b>Compilazione domanda di iscrizione all’Albo 
</b><p>
</p>
Per poter compilare la domanda di iscrizione è necessario inserire i dati nelle sezioni riportate a seguire.
<p>
</p>
    <div class="miomenu">
    
    <table class="menutable">
         <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaEnte.aspx?VediEnte=1">Sezione Ente Titolare</a></td>
        <td class="vocemenu">Inserire i dati anagrafici dell'Ente Titolare, completando le informazioni già inserite all'atto della registrazione. E’ richiesta la descrizione delle esperienze maturate dall’Ente nell’ultimo triennio nei settori del volontariato. E’ richiesta la scelta della sezione dell'Albo cui iscrivere l'Ente.
        <br />Tempo medio necessario per la compilazione: circa 10 minuti.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaEnteAccordo.aspx?azione=Ins&amp;VediEnte=1">Sezione Enti di Accoglienza</a></td>
        <td class="vocemenu">Inserire i dati anagrafici degli eventuali Enti di Accoglienza. E’ richiesta la descrizione delle esperienze maturate da ogni Ente di Accoglienza nell’ultimo triennio nei settori del volontariato .
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascun Ente di Accoglienza</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaSedi.aspx?tipoazione=Inserimento&amp;VediEnte=1">Sezione Sedi</a></td>
        <td class="vocemenu">Inserire i dati relativi alle Sedi di Attuazione. Ciascun Ente può avere una o più Sedi di Attuazione
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascuna sede</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="entepersonale.aspx?tipoazione=Inserimento&amp;VediEnte=1">Sezione Risorse</a></td>
        <td class="vocemenu">Inserire i dati relativi alle Risorse dell'Ente Titolare che ricoprono i ruoli di responsabili, formatori generali, selettori e esperti del monitoraggio.
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascuna risorsa</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmStrutturaOrganizzativaSistemi.aspx">Sezione Struttura Organizzativa<br />e Sistemi Funzionali</a></td>
        <td class="vocemenu">Inserire i dati relativi ai Sistemi Funzionali: Sistema di Comunicazione, Sistema di Selezione, Sistema di Formazione Generale, Sistema di Monitoraggio.  Caricare il documento che dettaglia la Struttura organizzativa e, eventualmente,  il Sistema di selezione 
        <br />Tempo medio necessario per la compilazione: circa 20 minuti</td>
        </tr>
       <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmGestioneRuoliAntiMafia.aspx">Sezione Ruoli Antimafia</a></td>
        <td class="vocemenu">Gli enti Titolari e di Accoglienza privati sono tenuti a inserire i dati anagrafici, completi della carica sociale rivestita dei 
        soggetti sottoposti al controllo antimafia, come previsto dal Codice Antimafia.
        <br />Tempo medio necessario per la compilazione: circa 3 minuti per ciascun ruolo.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="Autocertificazioni.aspx">Sezione Autocertificazione<br />e Consensi</a></td>
        <td class="vocemenu">Inserire per ciascun Ente le dichiarazione richieste sui ruoli non ricoperti.
        <br />Tempo medio necessario per la compilazione: circa 1 minuto per ciascun Ente </td>
        </tr>
        <tr valign="top" >
        <td class="titolo" colspan=2><br /><b>Funzioni di utilit&agrave;</b><br /></td>
        </tr>

        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmRicEnteinAccordo.aspx?azione=Mod&amp;VediEnte=1">Ricerca/Modifica Enti di Accoglienza</a></td>
        <td class="vocemenu">Visualizza la lista di Enti di Accoglienza inseriti; è possibile effettuare la ricerca degli Enti applicando filtri in base a 
        diversi parametri. La funzione permette di modificare dei dati relativi agli Enti visualizzati.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmRicercaSede.aspx?VediEnte=1">Ricerca/Modifica Sedi</a></td>
        <td class="vocemenu">Visualizza la lista di Sedi presenti; è possibile effettuare la ricerca delle Sedi applicando specifici filtri. La funzione permette di modificare i dati relativi alle Sedi visualizzate.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="ricercaentepersonale.aspx?VediEnte=1">Ricerca/Modifica Risorsa</a></td>
        <td class="vocemenu">Visualizza la lista delle Risorse e dei relativi ruoli svolti negli Enti; è possibile effettuare la ricerca delle Risorse
        applicando specifici filtri. La funzione permette di modificare i dati relativi alle Risorse visualizzate.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="DettaglioFunzioni.aspx?IdVoceMenu=115">Importazione da File</a></td>
        <td class="vocemenu">Permette "importazioni massive" di Enti, Sedi, Risorse e relativi documenti associati (Statuti, Atti costitutivi, curricula, ecc.) 
        tramite file di importazione in formato "CSV", facilmente realizzabili con i programmi di gestione fogli elettronici (ad esempio Excel)</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmGestioneDelegati.aspx">Gestione Delegati</a></td>
        <td class="vocemenu">Permette al Rappresentante Legale di delegare altri soggetti dotati di SPID all'inserimento dei dati necessari al completamento della
        Domanda di Iscrizione, fatta salva la firma digitale della Domanda che viene richiesta all'atto della presentazione.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu" nowrap><a class="level1 level1" href="WfrmRLEntiAccoglienza.aspx">Abilitazione alla registrazione<br />RL Enti di accoglienza</a></td>
        <td class="vocemenu">Permette al Rappresentante Legale dell’ente Titolare di abilitare il Rappresentante 
        legale di ciascun Ente di Accoglienza ad accedere al Sistema per  operare autonomamente sui propri dati. </td>
        </tr>

    </table>
    
  </div>
</div>

<div id="adeguamento_titolare" style="max-width: 800px;" runat="server">
    <b>Compilazione domanda di adeguamento dell'iscrizione all’Albo 
</b><p>
</p>
Per poter compilare la domanda di adeguamento è necessario inserire i dati nelle sezioni riportate a seguire.
<p>
</p>

      <div class="miomenu" >
    
    <table class="menutable">
         <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaEnte.aspx?VediEnte=1">Sezione Ente Titolare</a></td>
        <td class="vocemenu">Modificare i dati anagrafici dell'Ente Titolare, completando le informazioni già inserite all'atto della registrazione. E’ richiesta la descrizione delle esperienze maturate dall’Ente nell’ultimo triennio nei settori del volontariato. E’ richiesta la scelta della sezione dell'Albo cui iscrivere l'Ente.
        <br />Tempo medio necessario per la compilazione: circa 10 minuti.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaEnteAccordo.aspx?azione=Ins&amp;VediEnte=1">Sezione Enti di Accoglienza</a></td>
        <td class="vocemenu">Inserire i dati anagrafici di nuovi Enti di Accoglienza. E’ richiesta la descrizione delle esperienze maturate da ogni 
        nuovo Ente di Accoglienza nell’ultimo triennio nei settori del volontariato .
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascun Ente di Accoglienza</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaSedi.aspx?tipoazione=Inserimento&amp;VediEnte=1">Sezione Sedi</a></td>
        <td class="vocemenu">Inserire i dati relativi alle nuove Sedi di Attuazione.
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascuna sede</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="entepersonale.aspx?tipoazione=Inserimento&amp;VediEnte=1">Sezione Risorse</a></td>
        <td class="vocemenu">Inserire i dati relativi alle nuove Risorse dell'Ente Titolare che ricoprono i ruoli di responsabili, formatori generali, selettori e esperti del monitoraggio.
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascuna risorsa</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmStrutturaOrganizzativaSistemi.aspx">Sezione Struttura Organizzativa<br />e Sistemi Funzionali</a></td>
        <td class="vocemenu">Modificare i dati relativi ai Sistemi Funzionali: Sistema di Comunicazione, Sistema di Selezione, Sistema di Formazione Generale, Sistema di Monitoraggio.  Caricare il documento che dettaglia la Struttura organizzativa e, eventualmente,  il Sistema di selezione 
        <br />Tempo medio necessario per la compilazione: circa 20 minuti</td>
        </tr>
       <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmGestioneRuoliAntiMafia.aspx">Sezione Ruoli Antimafia</a></td>
        <td class="vocemenu">Gli enti Titolari e di Accoglienza privati sono tenuti a inserire i dati anagrafici, completi della carica sociale rivestita dei 
        soggetti sottoposti al controllo antimafia, come previsto dal Codice Antimafia.
        <br />Tempo medio necessario per la compilazione: circa 3 minuti per ciascun ruolo.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="Autocertificazioni.aspx">Sezione Autocertificazione<br />e Consensi</a></td>
        <td class="vocemenu">Inserire per ciascun Ente le dichiarazione richieste sui ruoli non ricoperti.
        <br />Tempo medio necessario per la compilazione: circa 1 minuto per ciascun Ente </td>
        </tr>
        <tr valign="top" >
        <td class="titolo" colspan=2><br /><b>Funzioni di utilit&agrave;</b><br /></td>
        </tr>

        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmRicEnteinAccordo.aspx?azione=Mod&amp;VediEnte=1">Ricerca/Modifica Enti di Accoglienza</a></td>
        <td class="vocemenu">Visualizza la lista di Enti di Accoglienza inseriti; è possibile effettuare la ricerca degli Enti applicando filtri in base a 
        diversi parametri. La funzione permette di modificare dei dati relativi agli Enti visualizzati.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmRicercaSede.aspx?VediEnte=1">Ricerca/Modifica Sedi</a></td>
        <td class="vocemenu">Visualizza la lista di Sedi presenti; è possibile effettuare la ricerca delle Sedi applicando specifici filtri. La funzione permette di modificare i dati relativi alle Sedi visualizzate.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="ricercaentepersonale.aspx?VediEnte=1">Ricerca/Modifica Risorsa</a></td>
        <td class="vocemenu">Visualizza la lista delle Risorse e dei relativi ruoli svolti negli Enti; è possibile effettuare la ricerca delle Risorse
        applicando specifici filtri. La funzione permette di modificare i dati relativi alle Risorse visualizzate.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="DettaglioFunzioni.aspx?IdVoceMenu=115">Importazione da File</a></td>
        <td class="vocemenu">Permette "importazioni massive" di Enti, Sedi, Risorse e relativi documenti associati (Statuti, Atti costitutivi, curricula, ecc.) 
        tramite file di importazione in formato "CSV", facilmente realizzabili con i programmi di gestione fogli elettronici (ad esempio Excel)</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmGestioneDelegati.aspx">Gestione Delegati</a></td>
        <td class="vocemenu">Permette al Rappresentante Legale di delegare altri soggetti dotati di SPID all'inserimento dei dati necessari al completamento della
        Domanda di Iscrizione, fatta salva la firma digitale della Domanda che viene richiesta all'atto della presentazione.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmRLEntiAccoglienza.aspx">Abilitazione alla registrazione<br />RL Enti di accoglienza</a></td>
        <td class="vocemenu">Permette al Rappresentante Legale dell’ente Titolare di abilitare il Rappresentante 
        legale di ciascun Ente di Accoglienza ad accedere al Sistema per  operare autonomamente sui propri dati. </td>
        </tr>

    </table>
    
  </div>
 </div>
 <div id="iscrizione_accoglienza" style="max-width: 800px;" runat="server">
    <b>Compilazione domanda di iscrizione all’Albo 
</b><p>
</p>
Per poter compilare la domanda di iscrizione è necessario inserire i dati nelle sezioni riportate a seguire.
<p>
</p>

      <div class="miomenu" >
    
    <table class="menutable">
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaEnteAccordo.aspx?azione=Ins&amp;VediEnte=1">Sezione Ente di Accoglienza</a></td>
        <td class="vocemenu">Inserire i dati anagrafici dell'Ente. E’ richiesta la descrizione delle esperienze maturate da ogni Ente di Accoglienza nell’ultimo triennio nei settori del volontariato .
        <br />Tempo medio necessario per la compilazione: circa 5 minuti</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaSedi.aspx?tipoazione=Inserimento&amp;VediEnte=1">Sezione Sedi</a></td>
        <td class="vocemenu">Inserire i dati relativi alle Sedi di Attuazione. Ciascun Ente può avere una o più Sedi di Attuazione
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascuna sede</td>
        </tr>
       <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmGestioneRuoliAntiMafia.aspx">Sezione Ruoli Antimafia</a></td>
        <td class="vocemenu">Gli enti Titolari e di Accoglienza privati sono tenuti a inserire i dati anagrafici, completi della carica sociale rivestita dei 
        soggetti sottoposti al controllo antimafia, come previsto dal Codice Antimafia.
        <br />Tempo medio necessario per la compilazione: circa 3 minuti per ciascun ruolo.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="Autocertificazioni.aspx">Sezione Autocertificazione<br />e Consensi</a></td>
        <td class="vocemenu">Inserire la dichiarazione richiesta sui ruoli non ricoperti.
        <br />Tempo medio necessario per la compilazione: circa 1 minuto per ciascun Ente </td>
        </tr>
        <tr valign="top" >
        <td class="titolo" colspan=2><br /><b>Funzioni di utilit&agrave;</b><br /></td>
        </tr>

        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmRicercaSede.aspx?VediEnte=1">Ricerca/Modifica Sedi</a></td>
        <td class="vocemenu">Visualizza la lista di Sedi presenti; è possibile effettuare la ricerca delle Sedi applicando specifici filtri. La funzione permette di modificare i dati relativi alle Sedi visualizzate.</td>
        </tr>
<!--
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="DettaglioFunzioni.aspx?IdVoceMenu=115">Importazione da File</a></td>
        <td class="vocemenu">Permette "importazioni massive" di Enti, Sedi, Risorse e relativi documenti associati (Statuti, Atti costitutivi, curricula, ecc.) 
        tramite file di importazione in formato "CSV", facilmente realizzabili con i programmi di gestione fogli elettronici (ad esempio Excel)</td>
        </tr>
-->
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmGestioneDelegati.aspx">Gestione Delegati</a></td>
        <td class="vocemenu">Permette al Rappresentante Legale di delegare altri soggetti dotati di SPID all'inserimento dei dati necessari al completamento della
        Domanda di Iscrizione, fatta salva la firma digitale della Domanda che viene richiesta all'atto della presentazione.</td>
        </tr>

    </table>
    
  </div>
</div>
 <div id="adeguamento_accoglienza" style="max-width: 800px;" runat="server">
    <b>Compilazione domanda di adeguamento dell'iscrizione all’Albo 
</b><p>
</p>
Per poter compilare la domanda di adeguamento è necessario inserire i dati nelle sezioni riportate a seguire.
<p>
</p>

      <div class="miomenu" >
    
     <table class="menutable">
        <!--
         <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaEnte.aspx?VediEnte=1">Sezione Ente Titolare</a></td>
        <td class="vocemenu">Modificare i dati anagrafici dell'Ente di Accoglienza. E’ richiesta la descrizione delle esperienze maturate dall’Ente nell’ultimo triennio nei settori del volontariato.
        <br />Tempo medio necessario per la compilazione: circa 10 minuti.</td>
        </tr>
        -->
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaEnteAccordo.aspx?azione=Ins&amp;VediEnte=1">Sezione Enti di Accoglienza</a></td>
        <td class="vocemenu">Modificare i dati anagrafici dell'Ente di Accoglienza. E’ richiesta la descrizione delle esperienze maturate nell’ultimo triennio nei settori del volontariato .
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascun Ente di Accoglienza</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmAnagraficaSedi.aspx?tipoazione=Inserimento&amp;VediEnte=1">Sezione Sedi</a></td>
        <td class="vocemenu">Inserire i dati relativi alle nuove Sedi di Attuazione.
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascuna sede</td>
        </tr>
        <!--
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="entepersonale.aspx?tipoazione=Inserimento&amp;VediEnte=1">Sezione Risorse</a></td>
        <td class="vocemenu">Inserire i dati relativi alle nuove Risorse dell'Ente Titolare che ricoprono i ruoli di responsabili, formatori generali, selettori e esperti del monitoraggio.
        <br />Tempo medio necessario per la compilazione: circa 5 minuti per ciascuna risorsa</td>
        </tr>

        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmStrutturaOrganizzativaSistemi.aspx">Sezione Struttura Organizzativa<br />e Sistemi Funzionali</a></td>
        <td class="vocemenu">Modificare i dati relativi ai Sistemi Funzionali: Sistema di Comunicazione, Sistema di Selezione, Sistema di Formazione Generale, Sistema di Monitoraggio.  Caricare il documento che dettaglia la Struttura organizzativa e, eventualmente,  il Sistema di selezione 
        <br />Tempo medio necessario per la compilazione: circa 20 minuti</td>
        </tr>
        -->
       <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmGestioneRuoliAntiMafia.aspx">Sezione Ruoli Antimafia</a></td>
        <td class="vocemenu">Gli enti Titolari e di Accoglienza privati sono tenuti a inserire i dati anagrafici, completi della carica sociale rivestita dei 
        soggetti sottoposti al controllo antimafia, come previsto dal Codice Antimafia.
        <br />Tempo medio necessario per la compilazione: circa 3 minuti per ciascun ruolo.</td>
        </tr>
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="Autocertificazioni.aspx">Sezione Autocertificazione<br />e Consensi</a></td>
        <td class="vocemenu">Inserire per ciascun Ente le dichiarazione richieste sui ruoli non ricoperti.
        <br />Tempo medio necessario per la compilazione: circa 1 minuto per ciascun Ente </td>
        </tr>
        <tr valign="top" >
        <td class="titolo" colspan=2><br /><b>Funzioni di utilit&agrave;</b><br /></td>
        </tr>
        <!--

        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmRicEnteinAccordo.aspx?azione=Mod&amp;VediEnte=1">Ricerca/Modifica Enti di Accoglienza</a></td>
        <td class="vocemenu">Visualizza la lista di Enti di Accoglienza inseriti; è possibile effettuare la ricerca degli Enti applicando filtri in base a 
        diversi parametri. La funzione permette di modificare dei dati relativi agli Enti visualizzati.</td>
        </tr>
        -->
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmRicercaSede.aspx?VediEnte=1">Ricerca/Modifica Sedi</a></td>
        <td class="vocemenu">Visualizza la lista di Sedi presenti; è possibile effettuare la ricerca delle Sedi applicando specifici filtri. La funzione permette di modificare i dati relativi alle Sedi visualizzate.</td>
        </tr>
        <!--
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="ricercaentepersonale.aspx?VediEnte=1">Ricerca/Modifica Risorsa</a></td>
        <td class="vocemenu">Visualizza la lista delle Risorse e dei relativi ruoli svolti negli Enti; è possibile effettuare la ricerca delle Risorse
        applicando specifici filtri. La funzione permette di modificare i dati relativi alle Risorse visualizzate.</td>
        </tr>

        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="DettaglioFunzioni.aspx?IdVoceMenu=115">Importazione da File</a></td>
        <td class="vocemenu">Permette "importazioni massive" di Enti, Sedi, Risorse e relativi documenti associati (Statuti, Atti costitutivi, curricula, ecc.) 
        tramite file di importazione in formato "CSV", facilmente realizzabili con i programmi di gestione fogli elettronici (ad esempio Excel)</td>
        </tr>
        -->
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmGestioneDelegati.aspx">Gestione Delegati</a></td>
        <td class="vocemenu">Permette al Rappresentante Legale di delegare altri soggetti dotati di SPID all'inserimento dei dati necessari al completamento della
        Domanda di Iscrizione, fatta salva la firma digitale della Domanda che viene richiesta all'atto della presentazione.</td>
        </tr>
        <!--
        <tr valign="top" >
        <td class="vocemenu"><a class="level1 level1" href="WfrmRLEntiAccoglienza.aspx">Abilitazione alla registrazione<br />RL Enti di accoglienza</a></td>
        <td class="vocemenu">Permette al Rappresentante Legale dell’ente Titolare di abilitare il Rappresentante 
        legale di ciascun Ente di Accoglienza ad accedere al Sistema per  operare autonomamente sui propri dati. </td>
        </tr>
        -->

    </table>
    
  </div>

</div>          
        <asp:Menu ID="MenuHelios" runat="server" CssClass="miomenu" 
            Font-names="Verdana, Gill Sans" orientation="Vertical" 
            staticdisplaylevels="5" staticsubmenuindent="5" SkipLinkText="" StaticMenuItemStyle-VerticalPadding="0.2em">
            <LevelMenuItemStyles>
                <asp:MenuItemStyle CssClass="level1" />
                <asp:MenuItemStyle CssClass="level2" />
                <asp:MenuItemStyle CssClass="level3" />
                <asp:MenuItemStyle CssClass="level4" />
                <asp:MenuItemStyle CssClass="level5" />
            </LevelMenuItemStyles>
            <StaticHoverStyle CssClass="hoverstyle" />
        </asp:Menu>
         <asp:Label ID="lblmsgerror" AssociatedControlID="MenuHelios" CssClass="msgErrore"  runat="server" Text=""></asp:Label>
     </fieldset>
    
       
  
 
</asp:Content>

