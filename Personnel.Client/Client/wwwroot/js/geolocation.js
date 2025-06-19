window.getGeoLocation = (dotnetHelper) => {
    if (!navigator.geolocation) {
        console.warn("Geolocalización no soportada");
        return;
    }

    navigator.geolocation.getCurrentPosition(
        position => {
            dotnetHelper.invokeMethodAsync("SetPosition", {
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
            });
        },
        error => {

            switch (error.code) {
                case error.PERMISSION_DENIED:
                    alert("Permiso denegado para obtener tu ubicación.");
                    break;
                case error.POSITION_UNAVAILABLE:
                    alert("No se pudo determinar tu ubicación.");
                    break;
                case error.TIMEOUT:
                    alert("La solicitud de ubicación tardó demasiado.");
                    break;
                default:
                    alert("Error desconocido al obtener ubicación.");
            }

            console.warn("Error geolocalización:", error.message);
            dotnetHelper.invokeMethodAsync("SetError", { message: error.message });
        },
        {
            enableHighAccuracy: true,
            timeout: 7000,
            maximumAge: 0
        }
    );

};
