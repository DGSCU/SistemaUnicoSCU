/* Visualizza la finestra modale di caricamento */
function ShowLoader() {
	$("#loader").show();
}
/* Nasconde la finestra modale di caricamento */
function HideLoader() {
	$("#loader").hide();
}


function InfoBox(text, title) {
	$("#popUpInfo .modal-title").html(title);
	$("#popUpInfo .modal-body").html(text);
	$("#popUpInfo").modal();
}