//Sets the attribution to non-collapsible
const attribution = new ol.control.Attribution({
    collapsible: false,
});

//The source for the map
const raster = new ol.layer.Tile({
    source: new ol.source.OSM(),
});

//Array with all the cords from the drawn objects
let drawCoords = [];

//The source for the vector layer that will be drawn on the map
const source = new ol.source.Vector();
const vector = new ol.layer.Vector({
    source: source,
    style: {
        'fill-color': 'rgba(255, 255, 255, 0.2)', // White with 20% opacity
        'stroke-color': '#ffcc33',  // (255,204,51) Sunglow
        'stroke-width': 2,
        'circle-radius': 7,
        'circle-fill-color': '#ffcc33',  // (255,204,51) Sunglow
    },
});


//The map object
const map = new ol.Map({
    layers: [raster, vector],
    target: 'map',
    view: new ol.View({
        projection: 'EPSG:4326',
        constrainResolution: true,
        center: [8.002189, 58.163703], // [lon, lat] Starts over Kristiansand (Maybe fetch from user?)
        zoom: 10,
    }),
});

//Makes the vector layer editable
const modify = new ol.interaction.Modify({ source: source });
map.addInteraction(modify);

let draw, snap; // global so we can remove it later
const typeSelect = document.getElementById('type');

//Function to add interactions to the map
function addInteractions() {
    const value = typeSelect.value;
    if (value !== 'None') {
        draw = new ol.interaction.Draw({
            source: source,
            type: typeSelect.value,
        });
        draw.on('drawend', function (evt) {
            var feature = evt.feature;
            var coords = feature.getGeometry().getCoordinates();
            let geoJsonGeometry = { type: typeSelect.value, coordinates: coords };
            drawCoords.push(geoJsonGeometry);
            console.log(drawCoords);
        });
        map.addInteraction(draw);
        snap = new ol.interaction.Snap({ source: source });
        map.addInteraction(snap);
    }
};

//Event listener for the typeSelect dropdown
typeSelect.onchange = function () {
    map.removeInteraction(draw);
    map.removeInteraction(snap);
    addInteractions();
};

addInteractions();

//Configures the buttons to change the type of interaction
/* 
* NB! Maybe change the buttosns to integrated controlls in the map via
* https://openlayers.org/en/latest/examples/custom-controls.html
* Will make the map view cleaner, but the current solutions works for now
*/
document.querySelectorAll('.map-button').forEach(button => {
    button.addEventListener('click', function () {
        var selectValue = this.dataset.select;
        if (typeSelect.value == selectValue) {
            selectValue = "None";
        }
        typeSelect.value = selectValue;

        const event = new Event('change', { bubbles: true });
        typeSelect.dispatchEvent(event);
    });
});

export function submitForm() {
    var responseObject = {
        point: [],
        linestring: [],
        polygon: []
    };

    var figures = drawCoords;
    figures.forEach((x) => {
        if (x.type === 'Point') {
            responseObject.point.push(x.coordinates);
        }
        if (x.type === 'LineString') {
            responseObject.linestring.push(x.coordinates);
        }
        if (x.type === 'Polygon') {
            responseObject.polygon.push(x.coordinates);
        }
    });

    var jsonValue = JSON.stringify(responseObject);
    console.log(jsonValue);

 //  document.getElementById("hiddenField").value = jsonValue;
 //  document.getElementById("mapInputForm").submit();  //Crashes because the mapInputForm and the MapData in HomeController do not match
}

