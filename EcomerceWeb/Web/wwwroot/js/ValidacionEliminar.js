<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
document.addEventListener("DOMContentLoaded", function () {
    const contenedor = document.getElementById("productosInventario");

    contenedor.addEventListener("submit", function (e) {
        const form = e.target.closest(".form-eliminar");
        if (!form) return; 
        e.preventDefault(); 

        Swal.fire({
            title: '¿Está seguro?',
            text: "No podrá deshacer esta acción.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                form.submit(); 
            }
        });
    });
});


