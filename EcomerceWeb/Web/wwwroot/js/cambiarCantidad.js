function cambiarCantidad(carritoProductoId, cambio) {
    const productoDiv = document.getElementById(`producto-${carritoProductoId}`);
    if (!productoDiv) return;

    const input = productoDiv.querySelector('.quantity-input');
    const valorAnterior = parseInt(input.value, 10);
    const nuevaCantidad = valorAnterior + cambio;

    if (nuevaCantidad <= 0) {
        eliminarProducto(carritoProductoId);
    } else {
        actualizarCantidad(carritoProductoId, nuevaCantidad, valorAnterior, cambio);
    }
}

window.cambiarCantidad = cambiarCantidad;