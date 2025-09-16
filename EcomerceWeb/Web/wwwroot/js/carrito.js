(function () {
    function debounce(func, delay) {
        let timer;
        return function (...args) {
            clearTimeout(timer);
            timer = setTimeout(() => func.apply(this, args), delay);
        };
    }

    function actualizarTotalesLocal() {
        let totalGeneral = 0;

        document.querySelectorAll('.cart-item').forEach(productoDiv => {
            const input = productoDiv.querySelector('.quantity-input');
            const cantidad = parseInt(input?.value, 10) || 0;
            const precioUnit = parseFloat(productoDiv.dataset.precioUnit || 0);
            const itemTotalElem = productoDiv.querySelector('.item-total');

            const totalLinea = cantidad * precioUnit;
            if (itemTotalElem) itemTotalElem.textContent = `₡${totalLinea.toFixed(2)}`;

            totalGeneral += totalLinea;
        });

        const totalElem = document.getElementById('total');
        if (totalElem) totalElem.textContent = `₡${totalGeneral.toFixed(2)}`;
    }

    async function refrescarStockGlobal() {
        try {
            const resp = await fetch('/Carrito/Carrito?handler=Refrescar');
            if (!resp.ok) return null;
            const data = await resp.json();

            if (!data || !data.success || !Array.isArray(data.productos)) return null;

            document.querySelectorAll('.cart-item').forEach(productoDiv => {
                const input = productoDiv.querySelector('.quantity-input');
                if (!input) return;

                const carritoProductoId = productoDiv.dataset.carritoProductoId || productoDiv.id.replace('producto-', '');
                const cantidadAnterior = Number(input.value) || 0;

                const productoApi = data.productos.find(p => {
                    const pId = p.id != null ? p.id.toString() : null;
                    const pCarritoId = p.carritoProductoId != null ? p.carritoProductoId.toString() : null;
                    const cId = carritoProductoId != null ? carritoProductoId.toString() : null;
                    return pId === cId || pCarritoId === cId;
                });

                if (!productoApi) return;

                const stockApi = Number(productoApi.stock ?? productoApi.stockDisponible ?? productoApi.StockDisponible ?? 0);
                const nuevoMax = Math.max(0, stockApi + cantidadAnterior);
                input.max = nuevoMax;

                if ((Number(input.value) || 0) > nuevoMax) {
                    input.value = Math.min(cantidadAnterior, nuevoMax);
                }
            });

            actualizarTotalesLocal();
            return data;
        } catch (err) {
            console.error('Error refrescando stock global:', err);
            return null;
        }
    }


    const actualizarCantidadAPI = debounce(async (carritoProductoId, cantidadNueva, cantidadAnterior) => {
        try {
            const productoDiv = document.getElementById(`producto-${carritoProductoId}`);
            if (!productoDiv) return;

            const input = productoDiv.querySelector('.quantity-input');
            if (!input) return;

            const response = await fetch(`${window.endpoints.actualizarProducto}${carritoProductoId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    cantidad: cantidadNueva,
                    productosId: productoDiv.dataset.productosId
                })
            });

            if (!response.ok) {
                let errorMsg = 'Error al actualizar la cantidad';
                try {
                    const errData = await response.json();
                    if (errData && errData.mensaje) errorMsg = errData.mensaje;
                } catch (e) { }

                input.value = cantidadAnterior;

                const data = await refrescarStockGlobal();
                if (data && data.success) {
                    const productoApi = data.productos.find(p => {
                        const pId = p.id != null ? p.id.toString() : null;
                        const pCarritoId = p.carritoProductoId != null ? p.carritoProductoId.toString() : null;
                        const cId = carritoProductoId != null ? carritoProductoId.toString() : null;
                        return pId === cId || pCarritoId === cId;
                    });

                    if (productoApi) {
                        const stockApi = Number(productoApi.stock ?? productoApi.stockDisponible ?? productoApi.StockDisponible ?? 0);
                        const nuevoMax = Math.max(0, stockApi + cantidadAnterior);
                        input.max = nuevoMax;
                        input.value = Math.min(cantidadAnterior, nuevoMax);
                        input.setCustomValidity(errorMsg);
                        input.reportValidity();
                    } else {
                        input.setCustomValidity(errorMsg);
                        input.reportValidity();
                    }

                    actualizarTotalesLocal();
                }

                return;
            }

            if (input) input.setCustomValidity('');
        } catch (err) {
            console.error('Error actualizarCantidadAPI:', err);
        }
    }, 300);
    function actualizarCantidad(carritoProductoId, valorInput) {
        const productoDiv = document.getElementById(`producto-${carritoProductoId}`);
        if (!productoDiv) return;

        const input = productoDiv.querySelector('.quantity-input');
        if (!input) return;

        const cantidadAnterior = Number(input.value) || 0;
        let nuevaCantidad = Number(valorInput) || 0;
        const max = Number(input.max) || 0;

        if (nuevaCantidad > max) {
            nuevaCantidad = max;
            input.value = max;
            input.setCustomValidity(`La cantidad no puede ser mayor a ${max}`);
            input.reportValidity();
        } else {
            input.setCustomValidity('');
        }

        if (nuevaCantidad <= 0) {
            if (typeof window.eliminarProducto === 'function') {
                window.eliminarProducto(carritoProductoId);
            } else {
                productoDiv.remove();
                actualizarTotalesLocal();
            }
            return;
        }

        input.value = nuevaCantidad;
        actualizarTotalesLocal();

        actualizarCantidadAPI(carritoProductoId, nuevaCantidad, cantidadAnterior);
    }

    function cambiarCantidad(carritoProductoId, cambio) {
        const productoDiv = document.getElementById(`producto-${carritoProductoId}`);
        if (!productoDiv) return;
        const input = productoDiv.querySelector('.quantity-input');
        if (!input) return;
        const nuevaCantidad = (Number(input.value) || 0) + cambio;
        actualizarCantidad(carritoProductoId, nuevaCantidad);
    }

    function initInputValidation() {
        document.querySelectorAll('.quantity-input').forEach(input => {
            input.addEventListener('input', () => {
                const max = Number(input.max) || 0;
                const val = Number(input.value) || 0;
                if (val > max) {
                    input.setCustomValidity(`La cantidad no puede ser mayor a ${max}`);
                } else {
                    input.setCustomValidity('');
                }
                input.reportValidity();
            });
        });
    }

    document.addEventListener('DOMContentLoaded', () => {
        initInputValidation();
        window.actualizarTotalesLocal = actualizarTotalesLocal;
        window.refrescarStock = refrescarStockGlobal;
        window.actualizarCantidad = actualizarCantidad;
        window.cambiarCantidad = cambiarCantidad;
        window.refrescarStockGlobal = refrescarStockGlobal;
    });

})();