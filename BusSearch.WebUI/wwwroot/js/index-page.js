document.addEventListener("DOMContentLoaded", function () {
    const errorBox = document.getElementById("formErrorMessage");
    const serverError = errorBox?.dataset.error;

    if (serverError) {
        errorBox.innerHTML = `<div>• ${serverError}</div>`;
        errorBox.classList.remove("d-none");
    }

    const storedData = localStorage.getItem("lastSearch");
    if (storedData) {
        const data = JSON.parse(storedData);

        updateLocationDisplay("origin", data.originName, data.originCityName);
        updateLocationDisplay("destination", data.destinationName, data.destinationCityName);

        document.getElementById("originId").value = data.originId;
        document.getElementById("originName").value = data.originName;
        document.getElementById("originCityName").value = data.originCityName;
        document.getElementById("destinationId").value = data.destinationId;
        document.getElementById("destinationName").value = data.destinationName;
        document.getElementById("destinationCityName").value = data.destinationCityName;

        document.querySelector("input[type='date']").value = data.departureDate;
    }

    // Tarih seçimi değiştiğinde validasyonu tetikle
    document.querySelector("input[type='date']").addEventListener("change", validateFormFields);
});

function updateLocationDisplay(prefix, name, city) {
    const line1 = document.getElementById(`${prefix}Line1`);
    const line2 = document.getElementById(`${prefix}Line2`);
    const placeholder = document.getElementById(`${prefix}Placeholder`);

    if (name) {
        line1.textContent = name;
        line1.classList.remove("d-none");

        if (city && city !== name) {
            line2.textContent = city;
            line2.classList.remove("d-none");
        } else {
            line2.classList.add("d-none");
        }

        placeholder.classList.add("d-none");
    } else {
        line1.classList.add("d-none");
        line2.classList.add("d-none");
        placeholder.classList.remove("d-none");
    }
}

function focusInput(inputId) {
    const input = document.getElementById(inputId);
    const display = document.getElementById(inputId.replace("Input", "Display"));
    const nameHidden = document.getElementById(inputId.replace("Input", "Name"));
    const cityHidden = document.getElementById(inputId.replace("Input", "CityName"));
    const line1 = document.getElementById(inputId.replace("Input", "Line1"));
    const line2 = document.getElementById(inputId.replace("Input", "Line2"));
    const placeholder = document.getElementById(inputId.replace("Input", "Placeholder"));

    display.classList.add("d-none");
    input.classList.remove("d-none");
    input.value = "";
    input.focus();

    function handleBlur() {
        if (!nameHidden.value) {
            line1.classList.add("d-none");
            line2.classList.add("d-none");
            placeholder.classList.remove("d-none");
        } else {
            updateLocationDisplay(inputId.replace("Input", ""), nameHidden.value, cityHidden.value);
        }

        input.classList.add("d-none");
        display.classList.remove("d-none");
    }

    input.addEventListener("blur", handleBlur, { once: true });
}

function fetchLocations(inputId, suggestionsId, hiddenInputId, hiddenNameId, hiddenCityId) {
    const input = document.getElementById(inputId);
    const suggestions = document.getElementById(suggestionsId);
    const hiddenInput = document.getElementById(hiddenInputId);
    const hiddenNameInput = document.getElementById(hiddenNameId);
    const hiddenCityInput = document.getElementById(hiddenCityId);

    async function loadSuggestions(keyword = "") {
        const response = await fetch(`/Home/SearchLocations?keyword=${keyword}`);
        const locations = await response.json();
        suggestions.innerHTML = "";

        locations.forEach(loc => {
            const item = document.createElement("button");
            item.type = "button";
            item.className = "list-group-item list-group-item-action";

            item.innerHTML = `
                <div class="fw-bold">${loc.name}</div>
                ${loc.parentName && loc.parentName !== loc.name
                    ? `<div class="text-muted small">${loc.parentName}</div>`
                    : ''}
            `;

            item.onclick = () => {
                hiddenInput.value = loc.id;
                hiddenNameInput.value = loc.name;
                hiddenCityInput.value = loc.parentName ?? "";
                updateLocationDisplay(hiddenInputId.replace("Id", ""), loc.name, loc.parentName);
                suggestions.innerHTML = "";
                input.classList.add("d-none");
                document.getElementById(inputId.replace("Input", "Display")).classList.remove("d-none");
                validateFormFields();
            };

            suggestions.appendChild(item);
        });
    }

    input.addEventListener("focus", async () => {
        await loadSuggestions();
    });

    input.addEventListener("input", async () => {
        await loadSuggestions(input.value.trim());
    });

    document.addEventListener("click", (event) => {
        if (!input.contains(event.target) && !suggestions.contains(event.target)) {
            suggestions.innerHTML = "";
        }
    });
}

function swapLocations() {
    const originId = document.getElementById("originId");
    const destinationId = document.getElementById("destinationId");
    const originName = document.getElementById("originName");
    const destinationName = document.getElementById("destinationName");
    const originCity = document.getElementById("originCityName");
    const destinationCity = document.getElementById("destinationCityName");

    [originId.value, destinationId.value] = [destinationId.value, originId.value];
    [originName.value, destinationName.value] = [destinationName.value, originName.value];
    [originCity.value, destinationCity.value] = [destinationCity.value, originCity.value];

    updateLocationDisplay("origin", originName.value, originCity.value);
    updateLocationDisplay("destination", destinationName.value, destinationCity.value);
}

function setToday() {
    const today = new Date();
    document.querySelector("input[type='date']").value = formatDateToInputValue(today);
    validateFormFields();
}

function setTomorrow() {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    document.querySelector("input[type='date']").value = formatDateToInputValue(tomorrow);
    validateFormFields();
}

function formatDateToInputValue(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

function validateFormFields() {
    const originId = document.getElementById("originId").value;
    const destinationId = document.getElementById("destinationId").value;
    const departureDate = document.querySelector("input[type='date']").value;
    const now = new Date();
    const today = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}-${String(now.getDate()).padStart(2, '0')}`;
    const errorBox = document.getElementById("formErrorMessage");

    let errors = [];

    if (originId === destinationId && originId !== "" && destinationId !== "") {
        errors.push("Kalkış ve varış noktaları aynı olamaz.");
    }

    if (departureDate < today) {
        errors.push("Geçmiş tarihli kalkış seçilemez. Lütfen bugünden sonraki bir tarih seçiniz.");
    }

    if (errors.length > 0) {
        errorBox.innerHTML = errors.map(err => `<div>• ${err}</div>`).join('');
        errorBox.classList.remove("d-none");
    } else {
        errorBox.classList.add("d-none");
    }
}

document.getElementById("searchForm").addEventListener("submit", function (e) {
    validateFormFields();
    const errorBox = document.getElementById("formErrorMessage");

    if (!errorBox.classList.contains("d-none")) {
        e.preventDefault();
        return;
    }

    const data = {
        originId: document.getElementById("originId").value,
        originName: document.getElementById("originName").value,
        originCityName: document.getElementById("originCityName").value,
        destinationId: document.getElementById("destinationId").value,
        destinationName: document.getElementById("destinationName").value,
        destinationCityName: document.getElementById("destinationCityName").value,
        departureDate: document.querySelector("input[type='date']").value
    };

    localStorage.setItem("lastSearch", JSON.stringify(data));
});


fetchLocations("originInput", "originSuggestions", "originId", "originName", "originCityName");
fetchLocations("destinationInput", "destinationSuggestions", "destinationId", "destinationName", "destinationCityName");
