document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const passwordInput = document.querySelector("input[name='Clave']");
    const cantidadInput = document.getElementById("CantidadDisponible");

    // Validación de contraseña con HTML5
    passwordInput.setAttribute("minlength", "4");
    passwordInput.setAttribute("required", "true");

    // Validación de cantidad disponible con HTML5
    cantidadInput.setAttribute("min", "0");
    cantidadInput.setAttribute("required", "true");

    // Mostrar mensajes de error con ARIA
    function mostrarError(elemento, mensaje) {
        const errorSpan = document.createElement("span");
        errorSpan.textContent = mensaje;
        errorSpan.classList.add("error");
        errorSpan.setAttribute("role", "alert");
        elemento.parentNode.appendChild(errorSpan);
    }

    // Validación en tiempo real
    cantidadInput.addEventListener("input", function () {
        const cantidad = parseInt(cantidadInput.value);
        if (isNaN(cantidad) || cantidad < 0) {
            mostrarError(cantidadInput, "Cantidad no válida");
        } else {
            const errorSpan = cantidadInput.parentNode.querySelector(".error");
            if (errorSpan) {
                errorSpan.remove();
            }
        }
    });

    // Validación al enviar el formulario
    form.addEventListener("submit", function (e) {
        let errores = [];

        // Validación de contraseña
        if (passwordInput.validity.tooShort) {
            errores.push("La contraseña debe tener al menos 4 caracteres.");
        }

        // Validación de cantidad disponible
        const cantidad = parseInt(cantidadInput.value);
        if (isNaN(cantidad) || cantidad < 0) {
            errores.push("Cantidad no válida");
        }

        if (errores.length > 0) {
            e.preventDefault();
            alert("Errores:\n" + errores.join("\n"));
        }
    });
});
