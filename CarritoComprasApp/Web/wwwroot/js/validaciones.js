document.querySelector("form")?.addEventListener("submit", function (e) {
    const password = document.querySelector("input[name='Clave']").value;
    if (password.length < 4) {
        e.preventDefault();
        alert("La contraseña debe tener al menos 4 caracteres.");
    }
});
function validarRegistro() {
    const nombres = document.getElementById("Nombres").value.trim();
    const direccion = document.getElementById("Direccion").value.trim();
    const telefono = document.getElementById("Telefono").value.trim();
    const usuario = document.getElementById("UsuarioLogin").value.trim();
    const identificacion = document.getElementById("Identificacion").value.trim();
    const clave = document.getElementById("Clave").value.trim();

    let errores = [];

    if (nombres.length < 3) errores.push("Nombre muy corto");
    if (direccion === "") errores.push("Dirección es obligatoria");
    if (!/^\d{7,}$/.test(telefono)) errores.push("Teléfono inválido");
    if (usuario.length < 3) errores.push("Usuario muy corto");
    if (!/^\d+$/.test(identificacion)) errores.push("Identificación inválida");
    if (clave.length < 5) errores.push("Contraseña muy corta");

    if (errores.length > 0) {
        alert("Errores:\n" + errores.join("\n"));
        return false;
    }

    return true;
}
function validarCompra() {
    const cantidad = parseInt(document.getElementById("Cantidad").value);
    const disponible = parseInt(document.getElementById("CantidadDisponible").value);

    if (isNaN(cantidad) || cantidad <= 0) {
        alert("Debe ingresar una cantidad válida.");
        return false;
    }

    if (cantidad > disponible) {
        return confirm(`Solo hay ${disponible} unidades disponibles. ¿Desea comprar esa cantidad?`);
    }

    return true;
}
function validarActualizarProducto() {
    const cantidad = parseInt(document.getElementById("CantidadDisponible").value);
    if (isNaN(cantidad) || cantidad < 0) {
        alert("Cantidad no válida");
        return false;
    }
    return true;
}
