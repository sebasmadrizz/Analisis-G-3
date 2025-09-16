let productosActuales = [];
function renderProductosBuscados(data) {
    productosActuales = data;
    let $contenedor = $("#productosInventario");
    $contenedor.empty();
    $contenedor.html("<div class='col-12 text-center'></div>");
    const productos = Array.isArray(data) ? data : [data];

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
        let lowStockHtml = "";

        if (p.stock < 15) {
            lowStockHtml = `
            <div class="low-stock-indicator position-absolute top-0 end-0 m-2" title="Stock bajo">
                <span class="badge bg-danger rounded-circle p-2 shadow">
                    <i class="bi bi-exclamation"></i>
                </span>
            </div>
        `;
        }

        let toggleBtnHtml = p.estado === "Activo"
            ? `<button class="btn btn-outline-success rounded-circle p-2 shadow-sm toggle-btn" title="Activo" data-id="${p.idProducto}">
                    <i class="bi bi-toggle-on"></i>
               </button>`
            : `<button class="btn btn-outline-danger rounded-circle p-2 shadow-sm toggle-btn" title="Inactivo" data-id="${p.idProducto}">
                    <i class="bi bi-toggle-off"></i>
               </button>`;

        $contenedor.append(`
    <div class="col">
        <div class="card h-100 shadow-sm rounded-4 overflow-hidden position-relative">
            <img src="${p.imagenUrl}"
                 alt="Imagen de ${p.nombre}"
                 class="card-img-top producto-img"
                 style="cursor:pointer;" />
            <div class="card-body d-flex flex-column">
                <h5 class="card-title mb-1 fw-semibold text-truncate" title="${p.nombre}">${p.nombre}</h5>
                <p class="card-text fw-bold text-primary mb-1">₡${p.precio.toLocaleString()}</p>
                <p class="text-muted mb-3 stock-info">Stock: ${p.stock}</p>
                <div class="mt-auto d-flex justify-content-between gap-2">
             ${toggleBtnHtml}
                    <button data-bs-toggle="modal"
                            data-bs-target="#modalFormularioEditar"
                            data-url="/Productos/Inventario?handler=FormularioModalEditar&idProducto=${p.idProducto}"
                            class="btn btn-outline-secondary rounded-circle">
                        <i class="bi bi-pencil"></i>
                    </button>

                </div>
            </div>
            ${lowStockHtml}
        </div>
    </div>
`);
    });


    const token = document.querySelector('#__RequestVerificationToken').value;

    $contenedor.find(".toggle-btn").on("click", function (e) {
        e.preventDefault();
        const idProducto = $(this).data("id");

        $.ajax({
            url: "/Productos/Inventario?handler=EliminarProducto", 
            type: "POST",
            data: {
                idProducto: idProducto,
                __RequestVerificationToken: token
            },
            success: function () {
                productosActuales = productosActuales.map(p => {
                    if (p.idProducto === idProducto) {
                        return {
                            ...p,
                            estado: p.estado === "Activo" ? "Inactivo" : "Activo"
                        };
                    }
                    return p;
                });
                renderProductosBuscados(productosActuales);
            },
            error: function (err) {
                console.error("Error al actualizar:", err);
            }
        });
    });
}



$(document).on("submit", "#BusquedaForm", function (e) {
    e.preventDefault();
    const $resultados = $("#resultadosBusqueda");
    $resultados.empty();

    const query = $(this).find("#buscador").val().trim();
    let $contenedor = $("#productosInventario");

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

    
});


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
            $resultados.html('<div class="p-2 text-muted">No se encontraron productos.</div>').removeClass("d-none");
            return;
        }

        const html = productos.slice(0, 5).map((p, i) => `
        <div class="resultado-item" >
            <img src="${p.imagenUrl}" alt="${p.nombre}">
            <div class="resultado-nombre">${p.nombre}</div>
            <div class="resultado-precio text-primary">₡${p.precio.toLocaleString()}</div>
        </div>
    `);

        $resultados.html(html.join("")).removeClass("d-none");
        
        
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