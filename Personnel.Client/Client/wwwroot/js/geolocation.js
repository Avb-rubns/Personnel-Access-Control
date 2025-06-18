window.getGeoLocation = (dotnetHelper) => {
    if (!navigator.geolocation) {
        console.warn("Geolocalización no soportada");
        return;
    }

    navigator.geolocation.getCurrentPosition(
        pos => {
            dotnetHelper.invokeMethodAsync("SetPosition", {
                latitude: pos.coords.latitude,
                longitude: pos.coords.longitude
            });
        },
        err => {
            console.warn("Error geolocalización:", err.message);
            dotnetHelper.invokeMethodAsync("SetError", { message: err.message });
        },
        { enableHighAccuracy: true, timeout: 5000 }
    );
};
