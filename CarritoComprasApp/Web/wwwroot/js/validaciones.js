document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const passwordInput = document.querySelector("input[name='Clave']");
    const cantidadInput = document.getElementById("CantidadDisponible");


    // Validación de contraseña con HTML5 (si existe el campo)
    if (passwordInput) {
        passwordInput.setAttribute("minlength", "4");
        passwordInput.setAttribute("required", "true");
    }

    // Validación de cantidad disponible con HTML5 (si existe el campo)
    if (cantidadInput) {
        cantidadInput.setAttribute("min", "0");
        cantidadInput.setAttribute("required", "true");

        // Mostrar mensajes de error con ARIA
        function mostrarError(elemento, mensaje) {
            let errorSpan = elemento.parentNode.querySelector(".error");
            if (!errorSpan) {
                errorSpan = document.createElement("span");
                errorSpan.classList.add("error");
                errorSpan.setAttribute("role", "alert");
                elemento.parentNode.appendChild(errorSpan);
            }
            errorSpan.textContent = mensaje;
        }

        // Validación en tiempo real para cantidad
        cantidadInput.addEventListener("input", function () {
            const cantidad = parseInt(cantidadInput.value);
            const errorSpan = cantidadInput.parentNode.querySelector(".error");

            if (isNaN(cantidad) || cantidad < 0) {
                mostrarError(cantidadInput, "Cantidad no válida");
            } else if (errorSpan) {
                errorSpan.remove();
            }
        });
    }

    // Validación al enviar el formulario
    if (form) {
        form.addEventListener("submit", function (e) {
            const errores = [];

            // Validación de contraseña
            if (passwordInput && passwordInput.value.length < 4) {
                errores.push("La contraseña debe tener al menos 4 caracteres.");
            }

            // Validación de cantidad
            if (cantidadInput) {
                const cantidad = parseInt(cantidadInput.value);
                if (isNaN(cantidad) || cantidad < 0) {
                    errores.push("Cantidad no válida");
                }
            }

            if (errores.length > 0) {
                e.preventDefault();
                alert("Errores:\n" + errores.join("\n"));
            }
        });
    }
});
