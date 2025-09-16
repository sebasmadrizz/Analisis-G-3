$('#modalFormularioEditar').on('shown.bs.modal', function () {
    const selectProveedor = document.getElementById('proveedorSelect');
    $.ajax({
        url: `https://localhost:7266/api/Proveedor`,
        method: "GET",
        dataType: "json",
        success: function (proovedores) {
            console.log(proovedores)
            selectProveedor.length = 1;
            renderSelectProveedores(proovedores);
        }
    });
    function renderSelectProveedores(proovedores) {
        proovedores.forEach(proveedor => {
            const option = document.createElement('option');
            option.value = proveedor.proveedoR_ID;
            option.text = proveedor.nombre_PROVEEDOR;
            selectProveedor.appendChild(option);
        });

    }


});