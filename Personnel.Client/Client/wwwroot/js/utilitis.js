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

import { QRCode } from '.qrcode.min.js'
function makeCode() {
    var qrcode = new QRCode("qrcode");
    var elText = document.getElementById("text");

    if (!elText.value) {
        alert("Input a text");
        elText.focus();
        return;
    }

    qrcode.makeCode(elText.value);
}
makeCode()