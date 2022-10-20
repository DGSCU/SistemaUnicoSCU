/*! 2015-02-04
* DIVINA CIVITILLO */

var prefissoIdClient = "MainContent_";
/* INIZIO funzioni  per le pagine inglobate nella master*/

function CostruisciId(IdServer) {
    var IdClient = prefissoIdClient + IdServer;
    return IdClient;
};

/* FINE funzioni  per le pagine inglobate nella master*/
function PopUpOption() {
    var width = screen.width / 1.5;
    var height = screen.height / 1.5;
    var x = screen.width / 2 - width;
    var y = screen.height / 2 - height;
    var winOption = "dependent=no,scrollbars=yes,status=no,resizable=yes,width=" + width + ",height=" + height + ",top=" + y + ",left=" + x;
    return winOption
};

function AggiornaErrore(messaggio, IdMsgErrore) {
    var label = "#" + IdMsgErrore;
    $(label).text(messaggio);
    if (messaggio = "") {

        document.getElementById(IdMsgErrore).style.display = 'none';
    }
    else {
        document.getElementById(IdMsgErrore).style.display = 'block';
    }

};

function ConcatenaErrore(messaggio, IdMsgErrore) {
    var label = "#" + IdMsgErrore;
    $(label).html($(label).html() + messaggio + ' <br/>');

};

function ValidaNumeroIntero(IdValore, descrizione, IdMsgErrore) {
    var valoreCampo = document.getElementById(IdValore).value;
   if (valoreCampo != '') {
       if ((parseInt(Number(valoreCampo)) == valoreCampo)==false) {
            var errore = "Il campo '" + descrizione + "' può contenere solo numeri.";
            AggiornaErrore(errore, IdMsgErrore);
            document.getElementById(IdValore).value = "";
            document.getElementById(IdMsgErrore).scrollIntoView();
            return false;
        }
        else {
            AggiornaErrore("", IdMsgErrore);
            return true;
        }
    }
    else {
        AggiornaErrore("", IdMsgErrore);
        return true;
    }
};


function ValidaNumero(IdValore, descrizione, IdMsgErrore) {
    var valoreCampo = document.getElementById(IdValore).value;
    var numeroValido;
    if (valoreCampo != '') {
        valoreCampo = valoreCampo.replace(",", ".");
        if ((Number(valoreCampo) == valoreCampo) == false) {
            var errore = "Il campo '" + descrizione + "' può contenere solo numeri.";
            AggiornaErrore(errore, IdMsgErrore);
            document.getElementById(IdMsgErrore).style.display = 'block';
            document.getElementById(IdValore).value = "";
            document.getElementById(IdMsgErrore).scrollIntoView();
            numeroValido = false;
        }
        else {
            AggiornaErrore("", IdMsgErrore);
            numeroValido = true;
        }
    }
    else {
        AggiornaErrore("", IdMsgErrore);
        numeroValido = true;
    }
    return numeroValido;
};

function ValidaEmail(IdEmail, descrizione, IdMsgErrore) {
    var i = new RegExp("^.+\\@(\\[?)[a-zA-Z0-9\\-\\.]+\\.([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
    if (!i.test(document.getElementById(IdEmail).value)) {
        var errore = "Il campo '" + descrizione + "' non è valido.";
        AggiornaErrore(errore, IdMsgErrore);
        document.getElementById(IdEmail).value = "";
        document.getElementById(IdMsgErrore).scrollIntoView();
    }
    else {
        AggiornaErrore("", IdMsgErrore);
    }
};

function VerificaCampoObbligatorio(IdCampo, descrizione, IdMsgErrore) {
    if (document.getElementById(IdCampo).value == '') {
        var errore = "Il campo '" + descrizione + "' è obbligatorio.";
        ConcatenaErrore(errore, IdMsgErrore);
    }
};
function TornaAdInizioPagina() {
    $("html, body").animate({ scrollTop: 0 }, 'fast');
};
function CongruenzaCodiceFiscale(pCodiceFiscale, pCognome, pNome, pSesso, pDataNascita, IdMsgErrore) {

    var TutteLeLettere = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
    var TuttiINumeri = '0123456789'
    var TuttiGliOmocodici = 'LMNPQRSTUV'
    var TutteLeVocali = 'AEIOU'
    var TutteLeConsonanti = 'BCDFGHJKLMNPQRSTVWXYZ'
    var CodMese = 'ABCDEHLMPRST'
    var swErr = false
    var Vocali = ''
    var Consonanti = ''
    var xCodCognome = ''
    var xCodNome = ''
    var tmpGiornoNascita = 0

    pCodiceFiscale = pCodiceFiscale.toUpperCase()

    if (pCodiceFiscale.length < 16) {
        swErr = true
    }
    //--- cognome e nome stringa
    for (i = 0; i < 6; i++) {
        if (TutteLeLettere.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
            swErr = true
        }
    }

    //--- anno numerico
    for (i = 6; i < 8; i++) {
        if (TuttiINumeri.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
            if (TuttiGliOmocodici.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
                swErr = true
            }
        }
    }

    //--- mese stringa
    for (i = 8; i < 9; i++) {
        if (TutteLeLettere.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
            swErr = true
        }
    }

    //--- giorno numerico
    for (i = 9; i < 11; i++) {
        if (TuttiINumeri.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
            if (TuttiGliOmocodici.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
                swErr = true
            }
        }
    }

    //--- primo carattere comune stringa
    for (i = 11; i < 12; i++) {
        if (TutteLeLettere.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
            swErr = true
        }
    }

    //--- 3 caratteri comune numerico
    for (i = 12; i < 15; i++) {
        if (TuttiINumeri.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
            if (TuttiGliOmocodici.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
                swErr = true
            }
        }
    }

    //--- ultimo carattere di controllo stringa
    for (i = 15; i < 16; i++) {
        if (TutteLeLettere.indexOf(pCodiceFiscale.substr(i, 1)) == -1) {
            swErr = true
        }
    }

    //--- FINE CONTROLLO FORMALE
    //--- Controllo Cognome
    pCognome = pCognome.toUpperCase()
    for (i = 0; i <= pCognome.length; i++) {
        if (TutteLeVocali.indexOf(pCognome.substr(i, 1)) > -1) {
            Vocali = Vocali + pCognome.substr(i, 1)
        } else {
            if (TutteLeConsonanti.indexOf(pCognome.substr(i, 1)) > -1) {
                Consonanti = Consonanti + pCognome.substr(i, 1)
            }
        }
        if (Consonanti.length == 3) {
            break;
        }
    }
    if (Consonanti.length < 3) {
        Consonanti = Consonanti + Vocali.substr(0, 3 - Consonanti.length)
        for (i = Consonanti.length; i < 3; i++) {
            Consonanti = Consonanti.concat("X")
        }
    }
    xCodCognome = Consonanti

    if (xCodCognome != pCodiceFiscale.substr(0, 3)) {
        //errore sul cognome
        swErr = true
    }

    //--- Controllo Nome
    Consonanti = ''
    Vocali = ''
    pNome = pNome.toUpperCase()
    for (i = 0; i <= pNome.length; i++) {
        if (TutteLeVocali.indexOf(pNome.substr(i, 1)) > -1) {
            Vocali = Vocali + pNome.substr(i, 1)
        } else {
            if (TutteLeConsonanti.indexOf(pNome.substr(i, 1)) > -1) {
                Consonanti = Consonanti + pNome.substr(i, 1)
            }
        }
    }

    if (Consonanti.length >= 4) {
        Consonanti = Consonanti.substr(0, 1) + Consonanti.substr(2, 2)
    } else {
        if (Consonanti.length < 3) {
            Consonanti = Consonanti + Vocali.substr(0, 3 - Consonanti.length)
            for (i = Consonanti.length; i < 3; i++) {
                Consonanti = Consonanti.concat("X")
            }
        }
    }
    xCodNome = Consonanti

    if (xCodNome != pCodiceFiscale.substr(3, 3)) {
        swErr = true
    }

    //--- Controllo Anno	
    tmpValore = DecodificaOmocodici(pCodiceFiscale.substr(6, 1)) + DecodificaOmocodici(pCodiceFiscale.substr(7, 1))
    if (isNaN(tmpValore)) {
        swErr = true
    } else {
        if (tmpValore != pDataNascita.substr(8, 2)) {
            swErr = true
        }
    }

    //--- Controllo Mese				
    if (pCodiceFiscale.substr(8, 1) != CodMese.substr(parseInt(pDataNascita.substr(3, 2) - 1), 1)) {
        swErr = true
    }

    //--- Controllo Giorno
    pSesso = pSesso.toUpperCase()
    if (pSesso == "1") {
        if (pDataNascita.substr(0, 1) == '0') {
            tmpGiornoNascita = parseInt(pDataNascita.substr(1, 1)) + 40
        } else {
            tmpGiornoNascita = parseInt(pDataNascita.substr(0, 2)) + 40
        }
    } else {
        if (pDataNascita.substr(0, 1) == '0') {
            tmpGiornoNascita = parseInt(pDataNascita.substr(1, 1))
        } else {
            tmpGiornoNascita = parseInt(pDataNascita.substr(0, 2))
        }
    }
    tmpValore = DecodificaOmocodici(pCodiceFiscale.substr(9, 1)) + DecodificaOmocodici(pCodiceFiscale.substr(10, 1))
    if (isNaN(tmpValore)) {
        swErr = true
    } else {
        if (tmpValore != tmpGiornoNascita) {
            swErr = true
        }
    }

    //swErr=false
    if (swErr == true) {
        var errore = "Codice Fiscale errato.";
        ConcatenaErrore(errore, IdMsgErrore);
    }
};

function DecodificaOmocodici(pValore) {
    var TuttiGliOmocodici = 'LMNPQRSTUV'
    if (TuttiGliOmocodici.indexOf(pValore) > -1) {
        switch (pValore) {
            case "L":
                return 0;
                break;
            case "M":
                return 1;
                break;
            case "N":
                return 2;
                break;
            case "P":
                return 3;
                break;
            case "Q":
                return 4;
                break;
            case "R":
                return 5;
                break;
            case "S":
                return 6;
                break;
            case "T":
                return 7;
                break;
            case "U":
                return 8;
                break;
            case "V":
                return 9;
                break;
        }
    } else {
        return pValore;
    }
};


function VerificaDataValida(data, descrizione, IdMsgErrore)
{
  var currVal = data;
  var errore = "La data '" + descrizione + "' non è valida. Inserire la data nel formato gg/mm/aaaa";
  if (currVal == '') {
      return true;
  }
  //Declare Regex 
  var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
  var dtArray = currVal.match(rxDatePattern); // is format OK?
  if (dtArray == null) {
      ConcatenaErrore(errore, IdMsgErrore);
      return false;
  }

  //Checks for dd/mm/yyyy format.
  dtDay = dtArray[1];
  dtMonth = dtArray[3];
  dtYear = dtArray[5];

  if (dtMonth < 1 || dtMonth > 12) {
      ConcatenaErrore(errore, IdMsgErrore);
      return false;
  }
  else if (dtDay < 1 || dtDay > 31) {
      ConcatenaErrore(errore, IdMsgErrore);
      return false;
  }
  else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31) {
      ConcatenaErrore(errore, IdMsgErrore);
      return false;
  }
  else if (dtMonth == 2)
  {
     var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
     if (dtDay > 29 || (dtDay == 29 && !isleap)) {
         ConcatenaErrore(errore, IdMsgErrore);
         return false;
     }
 }
 return true;
};

