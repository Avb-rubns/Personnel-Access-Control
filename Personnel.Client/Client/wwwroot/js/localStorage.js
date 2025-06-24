function setToLocalStorage(key, value) {
    window.localStorage.setItem(key, value)
}

function getFromLocalStorage(key) {
    const value = window.localStorage.getItem(key)

    if (value) {
        return value
    }
}

function deleteLocalStorage(key) {
    window.localStorage.removeItem(key)
}