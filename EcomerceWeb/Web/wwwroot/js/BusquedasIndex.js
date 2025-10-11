document.addEventListener("DOMContentLoaded", () => {
    const buscador = document.getElementById("buscador");
    const resultados = document.getElementById("resultadosBusqueda");

    let debounceTimer;
    let cache = new Map();
    let abortController = null;

    buscador.addEventListener("input", () => {
        clearTimeout(debounceTimer);
        const query = buscador.value.trim();

        debounceTimer = setTimeout(() => {
            buscarProductos(query);
        }, 300);
    });

    async function buscarProductos(query) {
        if (query.length < 2) {
            resultados.classList.add("d-none");
            resultados.innerHTML = "";
            return;
        }

        // Si ya está cacheado
        if (cache.has(query)) {
            renderResultados(cache.get(query));
            return;
        }

        // Cancelar petición anterior si sigue activa
        if (abortController) abortController.abort();
        abortController = new AbortController();

        try {
            const url = `https://localhost:7266/api/Productos/Busquedas-index/1/10?searchTerm=${encodeURIComponent(query)}`;
            console.log("Buscando:", url);

            const response = await fetch(url, { signal: abortController.signal });
            if (!response.ok) throw new Error("Error en la red");

            const data = await response.json();
            console.log("Respuesta de la API:", data);

            const productos = data?.data?.items || [];
            const suggestion = data?.suggestion || "";

            // Asegurarse de que hay productos
            if (!Array.isArray(productos)) {
                console.warn("⚠️ 'data.items' no es un array:", productos);
                resultados.innerHTML = `<div class="p-2 text-danger">Respuesta inesperada del servidor.</div>`;
                resultados.classList.remove("d-none");
                return;
            }

            const resultadoFinal = { productos, suggestion };
            cache.set(query, resultadoFinal);
            renderResultados(resultadoFinal);

        } catch (error) {
            if (error.name !== "AbortError") {
                console.error("❌ Error al buscar:", error);
                resultados.innerHTML = `<div class="p-2 text-danger">Error al buscar productos.</div>`;
                resultados.classList.remove("d-none");
            }
        }
    }

    function renderResultados({ productos, suggestion }) {
        resultados.innerHTML = "";

        if ((!productos || productos.length === 0) && !suggestion) {
            resultados.classList.add("d-none");
            return;
        }

        let html = "";

        // Mostrar sugerencia si existe
        if (suggestion && suggestion.trim() !== "") {
            html += `
                <div class="p-2 text-muted fst-italic small">
                    ¿Quisiste decir:
                    <span class="text-primary fw-semibold sugerencia"
                          style="cursor:pointer"
                          onclick="document.getElementById('buscador').value='${suggestion}';
                                   document.getElementById('buscador').dispatchEvent(new Event('input'));">
                        ${suggestion}
                    </span>?
                </div>
            `;
        }

        // Mostrar productos
        if (productos.length > 0) {
            html += productos.slice(0, 2).map(p => `
                <div class="resultado-item d-flex align-items-center gap-2 p-2 border-bottom"
                     style="cursor:pointer"
                     onclick="window.location='/Productos/DetalleProducto/?IdProducto=${p.idProducto}'">
                    <img src="${p.imagenUrl}" 
                         alt="${p.nombre}" 
                         class="rounded" 
                         style="width:55px; height:55px; object-fit:cover;">
                    <div>
                        <div class="fw-semibold">${p.nombre}</div>
                        <div class="text-primary">₡${p.precio.toLocaleString()}</div>
                    </div>
                </div>
            `).join("");
        }

        html += `
            <div class="resultado-ver-todos text-center p-2 fw-semibold text-primary"
                 style="cursor:pointer"
                 onclick="window.location='/Productos?busqueda=${encodeURIComponent(buscador.value.trim())}'">
                Ver todos los productos...
            </div>
        `;

        resultados.innerHTML = html;
        resultados.classList.remove("d-none");
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