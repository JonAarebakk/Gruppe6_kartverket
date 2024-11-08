//Sets the attribution to non-collapsible
const attribution = new ol.control.Attribution({
    collapsible: false,
});

//The source for the map
const raster = new ol.layer.Tile({
    source: new ol.source.OSM(), //OpenStreetMap gives ESPG:3857 projection by default
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

var featureCount = 0;

source.on("addfeature", function (evt) {
    featureCount = featureCount + 1;
    var feature = evt.feature;
    feature.setId(featureCount);
    console.log(feature.getId());
}); 

/*
source.on("change", function () {
    var features = source.getFeatures();
    console.log(features);
});*/

//The map object
const map = new ol.Map({
    layers: [raster, vector],
    target: 'map',
    view: new ol.View({
        projection: 'EPSG:3857', // Makes the map tilt a bit in north and south  ***NB! LOOK AT THAT*** 4326 or 3857
        constrainResolution: true,
        center: [890043.0200981781, 7998368.2036484955],  //[8.002189, 58.163703], // [lon, lat] Starts over Kristiansand (Maybe fetch from user?)
        zoom: 10,
    }),
});

const modify = new ol.interaction.Modify({ source: source });
map.addInteraction(modify);

const select = new ol.interaction.Select(); //Lets you select a feature on the map so we can delete it later
//map.addInteraction(select);

/*
modify.on('modifystart', function (evt) {
    //var myPrint = evt.get(features);
    //var myPrint = evt.features.getArray();
    var myPrint = evt.features;
    console.log(myPrint);
    var featureID = evt.features.getArray()[0].getId();
    console.log(featureID);
});*/


/** CAN NOT HAVE THIS ACTIVE WHILE ONE OF THE DRAW FUNCTIONS ARE ACTIVE. WILL GIVE ERROR */
select.on('select', function (evt) {  //NB! SE OM MAN KAN DESELCTE ETTERPÅ
    //Start med en if() som sjekker om featuren som er trykket på er en feature som ligger i source.getFeatures()
    console.log(evt);
    var selectedFeature = evt.selected[0];
    //console.log(selectedFeature);
    var featureID = evt.selected[0].getId();
    var selectedFeatureId = selectedFeature.getId();
    console.log(featureID + " " + selectedFeatureId + " 1 level deep");

    if (selectedFeatureId === undefined) {  //Legg inn sjekk ett level dypere //WHEN SELECT AND MODIFY ARE ACTIVE AT THE SAME TIME, THE SELECTED FEATURE WILL BE PLACED INSIDE ANOTHER FEATURE, SO THE ID HAS TO BE FOUND IN THE CHILD FEATURE
        console.log("it returned undefined");
        var selectedFeature2 = selectedFeature.get("features");
        var selectedFeature2Id = selectedFeature2[0].getId();
        //console.log(selectedFeature2);
        console.log(selectedFeature2Id + " 2 level deep");
    }
    select.getFeatures().clear();
    map.removeInteraction(select);
    map.addInteraction(modify);
    console.log("changed interactions");
});

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
            var transformedCoords = ol.proj.transform(coords, 'EPSG:3857', 'EPSG:4326'); // method to transform coordinates from EPSG:3857 to EPSG:4326 ***NB! Circle will crash this because it doesn't store coordinates the same way***
            let geoJsonGeometry = { type: typeSelect.value, coordinates: coords };
            drawCoords.push(geoJsonGeometry);
            //console.log(drawCoords);
            //console.log(transformedCoords);
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



document.querySelectorAll('.delete-feature').forEach(button => {
    button.addEventListener('click', function () {
        console.log("button pressed");
        //source.removeFeatures(source.getFeatures());
        //map.removeInteraction(modify);
        map.addInteraction(select);
    });
});
