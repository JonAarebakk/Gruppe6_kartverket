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
            console.log(coords);
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

//Disabled now to save clutter
/*   
const popup = new ol.Overlay({
    element: document.getElementById('popup'),
});
map.addOverlay(popup);
const element = popup.getElement();
*/

//Prints the coordinates of the clicked point to the console
map.on('singleclick', function (evt) {
    
    const coordinate = evt.coordinate;
    const hdms = ol.coordinate.toStringHDMS(coordinate);
    console.log(coordinate); //for debugging
    console.log(hdms); //for debugging
    
    //Disabled now to save clutter
    /*popup.setPosition(coordinate);
    let popover = bootstrap.Popover.getInstance(element);
    if (popover) {
        popover.dispose();
    }
    popover = new bootstrap.Popover(element, {
        animation: false,
        container: element,
        content: '<p>The location you clicked was:</p><code>' + hdms + '</code>',
        html: true,
        placement: 'top',
        title: 'Welcome to OpenLayers',
    });
    popover.show();*/
});



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

vector.changed();
vector.addEventListener('change', function () {
    var visibility = vector.getVisible();
    var proporties = vector.getProperties();
    console.log(visibility);
    console.log(proporties);

});

