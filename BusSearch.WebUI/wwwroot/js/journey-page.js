document.addEventListener("DOMContentLoaded", function () {
    const storedData = localStorage.getItem("lastSearch");
    if (storedData) {
        const data = JSON.parse(storedData);
        document.getElementById("fromDisplay").innerText = data.originName;
        document.getElementById("toDisplay").innerText = data.destinationName;
    }
});
