$(function () {
    let debounceTimer;
    let cache = {};
    let currentRequest = null;

    $("#buscador").on("input", function () {
        clearTimeout(debounceTimer);
        const query = $(this).val().trim();

        debounceTimer = setTimeout(function () {
            buscarProductos(query);
        }, 300);
    });

    function buscarProductos(query) {
        const $resultados = $("#resultadosBusqueda");

        if (query.length < 2) {
            $resultados.addClass("d-none").empty();
            return;
        }


        if (cache[query]) {
            renderResultados(cache[query]);
            return;
        }


        if (currentRequest) {
            currentRequest.abort();
        }

        currentRequest = $.ajax({
            url: `https://localhost:7266/api/Productos/Busqueda/${encodeURIComponent(query)}`,
            method: "GET",

            dataType: "json",
            success: function (productos) {
                cache[query] = productos;
                renderResultados(productos);
            },
            error: function (xhr, status) {
                if (status !== "abort") {
                    $resultados.html('<div class="p-2 text-danger">Error al buscar productos.</div>').removeClass("d-none");
                }
            }
        });
    }

    function renderResultados(productos) {
        const $resultados = $("#resultadosBusqueda");

        if (productos.length === 0) {

            return;
        }

        const html = productos.slice(0, 5).map(p => `
            <div class="resultado-item" onclick="window.location='/Productos/DetalleProducto/?IdProducto=${p.idProducto}'">
                <img src="${p.imagenUrl}" alt="${p.nombre}">
                <div class="resultado-nombre">${p.nombre}</div>
                <div class="resultado-precio text-primary">₡${p.precio.toLocaleString()}</div>
            </div>
        `).join('') + `
            <div class="resultado-ver-todos" onclick="window.location='/Productos?busqueda=${encodeURIComponent($('#buscador').val())}'">
                Ver todos los productos...
            </div>
        `;


        $resultados.html(html).removeClass("d-none");
    }
});

$(document).on("submit", "#BusquedaForm", function (e) {
    e.preventDefault();
    const $resultados = $("#resultadosBusqueda");
    $resultados.empty();

    const query = $(this).find("#buscador").val().trim();
    let $contenedor = $("#productos");

    $.ajax({
        url: `https://localhost:7266/api/Productos/Busqueda/${encodeURIComponent(query)}`,
        method: "GET",
        dataType: "json",
        success: function (productos) {
            renderProductosBuscados(productos);
        },
        error: function (xhr, status) {
            $contenedor.html('<div class="p-2 text-danger">Error al buscar productos.</div>');
        }
    });

    function renderProductosBuscados(productos) {
        let $contenedor = $("#productos");
        $contenedor.empty();
        $contenedor.innerHTML = "<div class='col-12 text-center'><span>Cargando...</span></div>";



        if (productos.length === 0) {
            $contenedor.html(`
            <div class="col-12">
                <div class="alert alert-warning w-100 text-center" role="alert">
                    Productos no disponibles
                </div>
            </div>
        `);
            return;
        }



        $.each(productos, function (i, p) {
            $contenedor.append(`
            <div class="col">
                <div class="card h-100 text-center shadow-sm border-0">
                    <img src="${p.imagenUrl}" alt="Producto"
                         onclick="window.location='/Productos/DetalleProducto/?IdProducto=${p.idProducto}'"
                         style="cursor:pointer;">
                    <div class="card-body d-flex flex-column">
                        <h6 class="card-title mb-1">${p.nombre}</h6>
                        <p class="fw-bold text-primary mb-4">$${p.precio}</p>
                        <button class="btn btn-outline-dark mt-auto">
                            <i class="fas fa-cart-plus"></i> Agregar
                        </button>
                    </div>
                </div>
            </div>
                    `);
        });

    }
});
$(document).ready(function () {

    $(document).on('click', function (event) {

        if (!$(event.target).closest('#buscador, #resultadosBusqueda').length) {
            $('#resultadosBusqueda').hide();
        }
    });


    $('#buscador').on('focus', function () {
        $('#resultadosBusqueda').show();
    });
});