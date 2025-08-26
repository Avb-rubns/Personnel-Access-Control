function detectColorScheme() {
    var theme = "light";

    //local storage is used to override OS theme settings
    if (localStorage.getItem("theme"))
    {
        if (localStorage.getItem("theme") == "dark") {
            var theme = "dark";
        }
    } else if (!window.matchMedia) {
        return false;
    } else if (window.matchMedia("(prefers-color-scheme: dark)").matches) {
        var theme = "dark";
    }

    localStorage.setItem("theme", "dark");
}
function createQR(id, options) {
    const element = document.getElementById(id);
    if (element) {
        element.innerHTML = "";
        new QRCode(element, options);

    }
}
function downloadQRcode(id, name) {
    const element = document.getElementById(id);
    if (!element) {
        return;
    }
    let canvas = element.querySelector('canvas');
    if (!canvas) {
        return;
    }
    const image = canvas.toDataURL("image/png");
    const link = document.createElement('a');
    link.href = image;
    link.download = name + ".png";
    link.click();
}

//function shareQRCode(id) {
//    // Verifica si la API de Web Share está disponible en el navegador.
//    if (navigator.share) {

//        console.log('Web Share es compatible en este navegador.');
//        // Configura los datos que se van a compartir.
//        const shareData = {
//            title: 'Mi increíble sitio web',
//            text: 'Echa un vistazo a este sitio web que encontré!',
//            url: 'https://www.ejemplo.com',
//        };

//        try {
//            // Intenta compartir.
//            await navigator.share(shareData);
//            console.log('Contenido compartido con éxito.');
//        } catch (err) {
//            // Maneja errores, como cuando el usuario cancela.
//            console.error('Error al compartir:', err);
//       }
//    } else {
//        // Si la API no está disponible, proporciona una alternativa.
//       // Por ejemplo, mostrar un modal con enlaces a redes sociales.
//        console.log('Web Share no es compatible en este navegador.');
//    }
//}

function width(){
    return window.innerWidth
}

async function copyText(text) {
    try {
        await navigator.clipboard.writeText(text);
        return true; // Éxito
    } catch (error) {
        return false; // Fallo
    }
}
