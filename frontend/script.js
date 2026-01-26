// Inicializa o mapa
const map = L.map('map').setView([20, 0], 2);

// Camada OpenStreetMap
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap'
}).addTo(map);

// Botão explorar
document.getElementById("exploreBtn").addEventListener("click", () => {
    alert("Clique em qualquer país no mapa 🌍");
});

// Clique no mapa
map.on('click', async (e) => {
    const lat = e.latlng.lat;
    const lng = e.latlng.lng;

    try {
        const response = await fetch(
            `http://localhost:5144/api/country?lat=${lat}&lng=${lng}`
        );

        if (!response.ok) {
            throw new Error("Erro ao consultar o backend");
        }

        const country = await response.json();

        if (!country) {
            alert("País não encontrado");
            return;
        }

        // Mostrar card
        document.getElementById("countryInfo").style.display = "block";

        document.getElementById("countryName").innerText = country.name;

        document.getElementById("countryFlag").src = country.flagUrl;
        document.getElementById("countryFlag").alt = `Bandeira de ${country.name}`;

        document.getElementById("countryDetails").innerText =
            `🌍 Região: ${country.region}
🏛 Capital: ${country.capital}
👥 População: ${country.population.toLocaleString()}
🗣 Idiomas: ${country.languages.join(", ")}
⏰ Fusos: ${country.timezones.join(", ")}`;

    } catch (error) {
        console.error(error);
        alert("Erro ao consultar o backend");
    }
});
