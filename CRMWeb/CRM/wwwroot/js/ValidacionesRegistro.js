
$(document).ready(function () {
    
    $("form").submit(function (e) {
        var password = $("#password").val();
        var confirmar = $("#Confirmarpassword").val();
        var aceptado = $("#aceptar").is(":checked");
            $(".text-danger.custom-error").remove();
            var valido = true;
            if (password !== confirmar) {
                $("#Confirmarpassword").after('<div class="text-danger custom-error">Las contraseñas no coinciden.</div>');
                valido = false;
            }

            
            if (!aceptado) {
                $("#aceptar").parent().after('<div class="text-danger custom-error">Debe aceptar los términos y condiciones.</div>');
                valido = false;
            }

            if (!valido) {
                e.preventDefault(); 
            }
        });
    $("#Confirmarpassword").change(function (e) {
        $(".text-danger.custom-error, .text-success").remove();
        var password = $("#password").val();
        var confirmar = $("#Confirmarpassword").val();
            if (password !== confirmar) {
                $("#Confirmarpassword").after('<div class="text-danger custom-error">Las contraseñas no coinciden.</div>');
                
            } else {
                $("#Confirmarpassword").after('<div class="text-success d-flex align-items-center mt-1"><i class="bi bi-check-circle-fill me-1"></i> Las contraseñas coinciden.</div>');
            }

        });
    });

