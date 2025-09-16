async function actualizarCantidad(carritoProductoId, cantidadNueva, valorAnterior, cambio) {
    const productoDiv = document.getElementById(`producto-${carritoProductoId}`);
    if (!productoDiv) return;

    const input = productoDiv.querySelector('.quantity-input');
    const btnIncrease = productoDiv.querySelector('.btn-increase');

    const response = await fetch(`${window.endpoints.actualizarProducto}${carritoProductoId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            cantidad: cantidadNueva,
            productosId: productoDiv.dataset.productosId
        })
    });

    if (!response.ok) {
        input.value = valorAnterior;
        if (btnIncrease) btnIncrease.disabled = true;
        return;
    }

    if (btnIncrease) btnIncrease.disabled = false;

    const carritoContainer = document.querySelector('.card-body[data-carrito-id]');
    if (!carritoContainer) return;
    const carritoId = carritoContainer.dataset.carritoId;

    const respCarrito = await fetch(`${window.endpoints.obtenerCarritoPorId}${carritoId}`);
    if (!respCarrito.ok) return;
    const carrito = await respCarrito.json();

    const producto = carrito.productos.find(p =>
        p.carritoProductoId.toLowerCase() === carritoProductoId.toLowerCase()
    );
    if (producto) {
        input.value = producto.cantidad;
        productoDiv.querySelector('.item-total').textContent = `₡${producto.totalLinea}`;
    } else {
        productoDiv.remove();
    }

    const totalElem = document.getElementById('total');
    if (totalElem && carrito.total != null) {
        totalElem.textContent = `₡${parseFloat(carrito.total).toFixed(2)}`;
    }
}

window.actualizarCantidad = actualizarCantidad;