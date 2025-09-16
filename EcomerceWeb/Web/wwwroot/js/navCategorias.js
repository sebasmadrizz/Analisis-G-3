document.addEventListener('DOMContentLoaded', async () => {
    const navUl = document.getElementById('navCategorias');
    if (!navUl) return;

    navUl.innerHTML = '';

    
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
        const padres = await fetchConRetry('/Productos/Index?handler=ObtenerCategoriasPadres') || [];

        
        const padresConHijas = [];
        for (const padre of padres) {
            const dataHijas = await fetchConRetry(`/Productos/Index?handler=ObtenerCategoriasHijas&id=${padre.categoriasId}`);
            if (dataHijas?.tieneHijas && Array.isArray(dataHijas.categorias) && dataHijas.categorias.length > 0) {
                padresConHijas.push({ padre, hijas: dataHijas.categorias });
            }
        }

        
        const primerosPadresConHijas = padresConHijas.slice(0, 4);

        
        for (const { padre, hijas } of primerosPadresConHijas) {
            const li = document.createElement('li');
            li.className = 'nav-item dropdown';

            const a = document.createElement('a');
            a.className = 'nav-link dropdown-toggle text-white';
            a.href = '#';
            a.id = `cat_${padre.categoriasId}`;
            a.setAttribute('role', 'button');
            a.setAttribute('data-bs-toggle', 'dropdown');
            a.setAttribute('aria-expanded', 'false');
            a.innerHTML = `${padre.nombre}`;

            li.appendChild(a);

            const ulDrop = document.createElement('ul');
            ulDrop.className = 'dropdown-menu';
            ulDrop.setAttribute('aria-labelledby', a.id);

            hijas.forEach(hija => {
                const liH = document.createElement('li');
                const aH = document.createElement('a');
                aH.className = 'dropdown-item';
                aH.href = `/Productos/ProductosxCategoria?categoriaId=${hija.categoriasId}`;
                aH.textContent = hija.nombre;
                liH.appendChild(aH);
                ulDrop.appendChild(liH);
            });

            li.appendChild(ulDrop);
            navUl.appendChild(li);
        }

    } catch (err) {
        console.error(err);
    }
});
