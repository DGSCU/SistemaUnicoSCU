
$(document).ready(function () {
    $('.it-date-datepicker').datepicker({
        inputFormat: ["dd/MM/yyyy"],
        outputFormat: 'dd/MM/yyyy',
	});
	$('[data-toggle="tooltip"]').tooltip();
	$('.input-time').on('input', ValidateTimeInput);
	$(".upload").change(ShowFilename);
});

/*$('form').submit(function () {
	controlloOK = true;
	$(this).find("input").each(function () {
		if (!checkField(this)) {
			if (controlloOK) {
				$(this).focus();
			}
			controlloOK = false;
		}
	});
	return controlloOK;
});*/
function ShowFilename(e) {
	var elem = $(e.currentTarget)
	var filename = elem.val().split('\\').pop();
	var id = elem.attr("id").replace(".","\\.");
	$("#"+id+"_Filename").text(filename);
	$("#" + id + "_Message").hide();

}

function checkField(self) {
	var elem = $(self);
	var errorMessage = "";
	var id = elem.attr("id");
	if (elem.attr('data-val-required')) {
		if (!elem.val()) {
			errorMessage=elem.attr('data-val-required');
		}
	}
	if (!errorMessage) {
		$("#" + id + "_Label").removeClass('text-danger');
		$("#" + id + "_Message").addClass('text-muted').removeClass('text-danger');
		return true;
	} else {
		$("#" + id + "_Label").addClass('text-danger');
		$("#" + id + "_Message").removeClass('text-muted').addClass('text-danger');
		$("#" + id + "_Message").text(errorMessage);
		return false;
	}
}


//Nascondo i campi upload per chi ha javascript
$(".upload").css("opacity","0");
$(".upload[type=file]+label").css("opacity", "1");

function isInt(value) {
	return /^\d+$/.test(value);
}

function ValidateTimeInput(e) {
	var element = $(e.target);
	var text = element.val();
	var fields = text.split(":");
	var hours = fields[0];
	if (hours.length > 2) {
		hours = hours.substring(0, 2);
		if (!isInt(hours)) {
			hours = hours.substring(0, 1);
			if (!isInt(hours)) {
				element.val("");
				return;
			}
		}
		element.val(hours);
		return;
	}
	if (hours.length === 1 && parseInt(hours) > 2) {
		element.val("");
		return;
	}
	if (parseInt(hours) >= 100) {
		element.val(hours.substring(0, 2));
		return;
	}
	if (parseInt(hours) >= 24) {
		element.val("2");
		return;
	}
	var minutes = fields[1];
	if (hours.length===2 && minutes === undefined) {
		element.val(hours + ":");
		return;
	}
	if (hours.length === 1 && fields.length>1) {
		element.val(hours);
		element.val(hours);
		return;
	}
	if (minutes.length > 2) {
		minutes = minutes.substring(0, 2);
		if (!isInt(minutes)) {
			minutes = minutes.substring(0, 1);
		}
		if (!isInt(minutes)) {
			element.val(hours + ":");
			return;
		}
		element.val(hours + ":" + minutes);
		return;
	}

	if (minutes.length === 1 && parseInt(minutes) > 6) {
		element.val(hours + ":");
		return;
	}
	if (parseInt(minutes) >= 100) {
		element.val(hours + ":" + minutes.substring(0, 2));
		return;
	}
	if (parseInt(minutes) > 60) {
		element.val(hours + ":6");
		return;
	}
	if (fields.length > 2) {
		element.val(hours + ":" + minutes);
		return;
	}
};
