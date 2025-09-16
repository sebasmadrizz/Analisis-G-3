document.addEventListener('DOMContentLoaded', async () => {
    const sidebar = document.querySelector('.sidebar-nav nav');
    if (!sidebar) return;

    sidebar.innerHTML = ''; 

   
    async function fetchConRetry(url, intentos = 2) {
        for (let i = 0; i < intentos; i++) {
            try {
                const res = await fetch(url, { cache: 'no-store' });
                if (res.ok) return await res.json();
            } catch (err) {
                console.error(err);
            }
        }
        return null;
    }

    try {
        const padresData = await fetchConRetry('/Productos/Index?handler=ObtenerCategoriasPadres');
        const padres = padresData || [];

        if (!padres.length) return;

        for (const padre of padres) {
            const padreLink = document.createElement('a');
            padreLink.className = 'nav-link d-flex align-items-center p-3 rounded-3 mb-1 text-decoration-none text-dark';
            padreLink.href = '#';
            padreLink.dataset.padreId = padre.categoriasId;

            const icon = document.createElement('i');
            icon.className = 'fas fa-circle me-2';
            padreLink.appendChild(icon);

            const spanText = document.createElement('span');
            spanText.className = 'nav-text';
            spanText.textContent = padre.nombre;
            padreLink.appendChild(spanText);

            const hijasContainer = document.createElement('div');
            hijasContainer.className = 'hijas-container mt-1 ps-3';
            hijasContainer.style.display = 'none';

            padreLink.addEventListener('click', async (e) => {
                e.preventDefault();

                
                document.querySelectorAll('.hijas-container').forEach(div => {
                    if (div !== hijasContainer) div.style.display = 'none';
                });

                if (hijasContainer.style.display === 'none') {
                    
                    if (!hijasContainer.hasChildNodes()) {
                        const respuesta = await fetchConRetry(`/Productos/Index?handler=ObtenerCategoriasHijas&id=${padre.categoriasId}`);
                        if (respuesta?.tieneHijas && Array.isArray(respuesta.categorias)) {
                            const select = document.createElement('select');
                            select.className = 'form-select form-select-sm mb-2 shadow-sm border-0 bg-light';
                            select.innerHTML = `<option value="" disabled selected>—</option>`;

                            respuesta.categorias.forEach(hija => {
                                const option = document.createElement('option');
                                option.value = hija.categoriasId;
                                option.textContent = hija.nombre;
                                select.appendChild(option);
                            });

                            select.addEventListener('change', (e) => {
                                const subId = e.target.value;
                                if (subId) {
                                    window.location.href = `/Productos/ProductosxCategoria?categoriaId=${subId}`;
                                }
                            });

                            hijasContainer.appendChild(select);
                        } else {
                            
                            window.location.href = `/Productos/ProductosxCategoria?categoriaId=${padre.categoriasId}`;
                        }
                    }
                    hijasContainer.style.display = 'block';
                } else {
                    hijasContainer.style.display = 'none';
                }
            });

            sidebar.appendChild(padreLink);
            sidebar.appendChild(hijasContainer);
        }

        
        sidebar.addEventListener('mouseleave', () => {
            document.querySelectorAll('.hijas-container').forEach(div => {
                div.style.display = 'none';
            });
        });

    } catch (err) {
        console.error(err);
    }
});
