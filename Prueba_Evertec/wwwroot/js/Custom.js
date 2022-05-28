$.getJSON('https://api.ipify.org?format=json', function (data) {
    $("#wclip").val(data.ip);

});


$('#NumCelular').on('input', function () {
    var regex = /^[3][0-9]/
    if (!regex.test(this.value)) {
        $('#NumCelular').addClass("is-invalid")
    } else {
        $('#NumCelular').removeClass("is-invalid");
    }

    if (this.value.length > 10) {
        this.value = this.value.slice(0, 10);
    }

});

$('#ValorPago').on('input', function () {
    var regex = /^[1-9][0-9]/;
    if (!regex.test(this.value)) {
        $('#ValorPago').addClass("is-invalid")
    } else {
        $('#ValorPago').removeClass("is-invalid");
    }
});

$('#NumDoc').on('input', function () {

    var tip_doc = $("#TipoDocumento").val();
    var regex = /^[1-9][0-9]{3,9}$/;

    switch (tip_doc) {

        case "CC":
            if (!regex.test(this.value)) {
                $('#NumDoc').addClass("is-invalid");
            } else {
                $('#NumDoc').removeClass("is-invalid")
            }
            break;
        case "CE":
            regex = /^([a-zA-Z]{1,5})?[1-9][0-9]{3,7}$/;
            if (!regex.test(this.value)) {
                $('#NumDoc').addClass("is-invalid");
            } else {
                $('#NumDoc').removeClass("is-invalid")
            }
            break;
        case "NIT":
            regex = /^[1-9]\d{6,9}$/;
            if (!regex.test(this.value)) {
                $('#NumDoc').addClass("is-invalid");
            } else {
                $('#NumDoc').removeClass("is-invalid")
            }
            break;
        case "RUT":
            regex = /^[1-9]\d{6,9}$/;
            if (!regex.test(this.value)) {
                $('#NumDoc').addClass("is-invalid");
            } else {
                $('#NumDoc').removeClass("is-invalid")
            }
            break;
    }

});

$('#EnviarFactura').on('click', function () {

    let validar = document.querySelector(".form-control");

    if (!validar.classList.contains("is-invalid") ) {
        $("#FormFactura").submit();
    }
});


