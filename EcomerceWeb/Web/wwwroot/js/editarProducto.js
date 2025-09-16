
const modalEditar = $('#modalFormularioEditar');
$('#modalFormularioEditar').on('show.bs.modal', function (event) {
    var buttonEditar = $(event.relatedTarget);
    var urlEditar = buttonEditar.data('url');

    $.get(urlEditar, function (html) {
        $('#contenidoModalEditar').html(html);
    });
});


$(document).on('submit', '#formProductoEditar', function (e) {
    e.preventDefault();

    var $form = $(this);
    var url = $form.attr('action');
    var formData = new FormData(this);
    console.log(url);
    var antiForgeryToken = $form.find('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            'RequestVerificationToken': antiForgeryToken
        },
        success: function (response) {


            if (response.success) {

                Swal.fire({
                    title: "<strong>Producto editado</strong>",
                    icon: "success",
                    html: `
    <p>¡El producto fue editado correctamente!</p>
    <img src="/img/aprobado.jpg" alt="Producto editado" style="width: 150px; margin-top: 10px;" />
  `,
                    showCloseButton: true,
                    showCancelButton: false,
                    focusConfirm: false,
                    confirmButtonText: `
    <i class="fa fa-thumbs-up"></i> ¡Volver!
  `,
                    
                }).then((result) => {
                    if (result.isConfirmed) {
                        location.reload();
                    }
                });

            }
        }
    });
});