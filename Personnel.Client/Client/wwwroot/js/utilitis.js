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

window.shareHelper = {
    shareQR: async function (id, title, text, name) {
        try {

            const element = document.getElementById(id);
            if (!element) {
                return false;
            }
            let canvas = element.querySelector('canvas');
            if (!canvas) {
                return;
            }

            // Convertir canvas a blob
            const dataUrl = canvas.toDataURL("image/png");
            const response = await fetch(dataUrl);
            const blob = await response.blob();
            const file = new File([blob], name, { type: "image/png" });

            if (navigator.canShare && navigator.canShare({ files: [file] })) {
                await navigator.share({
                    title: title,
                    text: text,
                    files: [file]
                });
                return true;
            } else {
                console.warn("Web Share API no soporta archivos en este navegador.");
                return false;
            }
        } catch (err) {
            console.error("Error al compartir canvas:", err);
            return false;
        }
    }
};


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
