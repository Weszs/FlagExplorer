// Inicializa o mapa
const map = L.map('map').setView([20, 0], 2);

// Camada do mapa
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap'
}).addTo(map);

// Botão explorar
document.getElementById("exploreBtn").addEventListener("click", () => {
    alert("Clique em um país no mapa!");
});

// Evento de clique no mapa
map.on('click', async function (e) {
    const lat = e.latlng.lat;
    const lng = e.latlng.lng;

    try {
        const response = await fetch(`http://localhost:5144/api/country?lat=${lat}&lng=${lng}`);
        const data = await response.json();

        console.log("Dados recebidos do backend:", data); // Debug

        // Lida com retorno que pode ser array ou objeto
        const country = Array.isArray(data) ? data[0] : data;

        if (!country || !country.name) {
            alert("País não encontrado!");
            return;
        }

        // Exibe informações no painel
        document.getElementById("countryInfo").style.display = "block";
        document.getElementById("countryName").innerText = country.name.common || "N/A";
        document.getElementById("countryFlag").src = country.flags?.png || "";
        document.getElementById("countryDetails").innerText =
            `Capital: ${country.capital?.[0] || 'N/A'} | População: ${country.population?.toLocaleString() || 'N/A'}`;
    } catch (err) {
        console.error("Erro ao consultar o backend:", err);
        alert("Não foi possível obter informações do país.");
    }
});
