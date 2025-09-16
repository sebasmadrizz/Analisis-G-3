$('#modalFormularioEditar').on('shown.bs.modal', function () {
    const selectCategorias = document.getElementById('categoriasSelect');
    $.ajax({
        url: `https://localhost:7266/api/Categorias`,
        method: "GET",
        dataType: "json",
        success: function (categorias) {
            
            selectCategorias.length = 1;
            renderSelectCategorias(categorias);
        }
    });
    function renderSelectCategorias(categorias) {
        categorias.forEach(categoria => {
            const option = document.createElement('option');
            option.value = categoria.categoriasId;
            option.text = categoria.nombre;
            selectCategorias.appendChild(option);
        });

    }


});