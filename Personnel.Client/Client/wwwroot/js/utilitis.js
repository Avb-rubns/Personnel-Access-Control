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
        new QRCode(document.getElementById(id), options);

    }
}