(function () {

    function actualizarItemCountYVista() {
        const items = document.querySelectorAll('.cart-item');
        const countElem = document.getElementById('item-count');
        if (countElem) countElem.textContent = items.length;

        const emptyCartElem = document.getElementById('empty-cart');
        const cartWithItems = document.getElementById('cart-with-items');
        if (items.length === 0) {
            if (emptyCartElem) emptyCartElem.classList.remove('d-none');
            if (cartWithItems) cartWithItems.classList.add('d-none');
        } else {
            if (emptyCartElem) emptyCartElem.classList.add('d-none');
            if (cartWithItems) cartWithItems.classList.remove('d-none');
        }
    }

    async function eliminarProducto(carritoProductoId) {
        const productoDiv = document.getElementById(`producto-${carritoProductoId}`);
        if (!productoDiv) return;

        const controles = productoDiv.querySelectorAll('button, input');
        controles.forEach(c => c.disabled = true);

        try {
            const resp = await fetch(`${window.endpoints.eliminarProducto}${carritoProductoId}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' }
            });

            if (resp.ok) {
                productoDiv.remove();

                actualizarItemCountYVista();
                    window.actualizarTotalesLocal();

            } 
        } catch (err) {
            console.error('Error en la solicitud de eliminación:', err);
            controles.forEach(c => c.disabled = false);
        }
    }

    window.eliminarProducto = eliminarProducto;
})();