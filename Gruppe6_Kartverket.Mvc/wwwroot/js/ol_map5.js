﻿/* 
*************************************
**  Section 1: Setting up the map  **
**********************************************
**  To do list:                             **
**  - Change color for drawn features       **
**  - Implement fetch coordinates from user **
********************************************** 
*/ 

//Sets the attribution to non-collapsible
const attribution = new ol.control.Attribution({
    collapsible: false,
});

//The source for the map
const raster = new ol.layer.Tile({
    source: new ol.source.OSM(), //OpenStreetMap gives ESPG:3857 projection by default
});

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

//Initializes the map
const map = new ol.Map({
    layers: [raster, vector],
    target: 'map',
    view: new ol.View({
        projection: 'EPSG:3857', // Projects EPSG:3857 to match the OpenStreetMap tiles
        constrainResolution: true, // Set whether the view should allow intermediary zoom levels.
        center: [890043.0200981781, 7998368.2036484955],  //[8.002189, 58.163703], // [lon, lat] Starts over Kristiansand (Maybe fetch from user?)
        zoom: 10,   // Initial zoom level
    }),
});


/* 
*********************************************************
**  Section 2: Configure map buttons and interactions  **
*********************************************************
**  To do list:                                        **
**  - make maxFeaturesWarning() give a popUp           **
**  - make select.on to deactivate proparly            **
**  - make deleteFeature give popup for no more objects**
**  - remove console.log() debugging statements        **
**  - for cancel-case-button add a popUp for are you   **
**    sure? And if yes, clear the map                  **
*********************************************************
*/



//Enables modification of the vector layer i.e. the ability to move the drawn objects
const modify = new ol.interaction.Modify({ source: source });
map.addInteraction(modify);

const select = new ol.interaction.Select(); //Lets you select a feature on the map so we can delete it later


let draw, snap; // global so we can remove it later
var actionType = document.getElementById('actionType'); // What the user wants to do to
var drawnFeatures = 0;  //Counter for the amount of drawn objects

//Choose the interaction based on the actionType variable
function chooseInteractions() {
    const value = actionType.value;
    console.log(value);  //for debugging
    if (value !== 'None') {
        if (value == 'Delete') {
            deleteFeature();
        } else if (drawnFeatures < 10) { //Decide the max amount of drawn objects
            drawFeature();
        } else {
            console.log("Reached the end");  //for debugging
            maxFeaturesWarning(); //Give the message for max amount of drawings reached
        }
    } else {
        setActiveMapButton();
    }
};

// Function to delete a feature
function deleteFeature() { 
    if (drawnFeatures > 0) {
        map.removeInteraction(modify);
        map.addInteraction(select);
    } else {
        //Give a message that there are no features to delete
        /* Will be changed to give a popUp instead later */
        
        actionType.value = 'None';  //Set the actionType back to "None"
        resetInteractions();        //Reset the interactions
    }
}

//Function to draw a feature
function drawFeature() {
    draw = new ol.interaction.Draw({
        source: source,
        type: actionType.value,
    });
    draw.on('drawend', function () {       //What to do when a feature is drawn
        drawnFeatures = drawnFeatures + 1; //Increment the drawnFeatures variable
        resetInteractions();
    });

    map.addInteraction(draw);
    snap = new ol.interaction.Snap({ source: source });
    map.addInteraction(snap);
}


//Gives a warning when the user tries to draw more than 10 objects
function maxFeaturesWarning() { 
    if (source.getFeatures().length <= 10) {
        console.log("You can not draw more than 10 objects"); //for debugging
        /* Will be changed to give a popUp instead later */
    }
}

//Event listener for the actionType variable
//Removes all interactions and runs the chooseInteractions function
actionType.onchange = function () {
    setActiveMapButton();
    resetInteractions();
};

//Removes all interactions and runs the chooseInteractions function
function resetInteractions() {
    map.removeInteraction(draw);
    map.removeInteraction(snap);
    map.removeInteraction(select);
    map.addInteraction(modify);

    chooseInteractions();  //Runs the chooseInteractions to decide what to do
}


var featureCount = 0; //A counter to set IDs for new features

//Gives newly drawn features a unique ID from featureCount
source.on("addfeature", function (evt) {
    featureCount = featureCount + 1;
    var feature = evt.feature;
    feature.setId(featureCount);
});


//When a feature is selected, remove it from the source
select.on('select', function (evt) {  
    if ((evt.selected !== null) && (evt.selected.length > 0)) { //Check if a feature is selected
        var featureID = evt.selected[0].getId();        //Get the ID of the selected feature
        var feature = source.getFeatureById(featureID); //Uses the featureID to get the feature

        source.removeFeature(feature);    //Remove the feature from the source
        drawnFeatures = drawnFeatures - 1;  //Subtract to keep track of feature amount
        resetInteractions();
    }
});

//Changes the active map button for styling purposes
function setActiveMapButton() {
    var buttons = document.querySelectorAll('.map-button');
    buttons.forEach(button => {
        if (button.dataset.select == actionType.value) {
            button.classList.add('active');
        } else {
            button.classList.remove('active');
        }
    });
}

//Clears the source of all features and resets the drawnFeatures counter
function clearMapOfFunctions() { 
    source.clear();
    drawnFeatures = 0;
}

//Configures the buttons to change the type of interaction
document.querySelectorAll('.map-button').forEach(button => {
    button.addEventListener('click', function () {
        var selectValue = this.dataset.select;
        if (actionType.value == selectValue) {
            selectValue = 'None';
        }
        actionType.value = selectValue;

        const event = new Event('change', { bubbles: true });
        actionType.dispatchEvent(event); // Dispatches the event to the actionType variable
    });
});

// When the case is cancelled, set the actionType back to "None"
document.querySelectorAll('.cancel-case-button').forEach(button => {
    button.addEventListener('click', function () {
        console.log("Case Cancelled"); //for debugging
        actionType.value = 'None';     //Set the actionType back to "None"
        setActiveMapButton();          //Resets the map buttons
        clearMapOfFunctions();         //Clears the map of all features
    });
});

document.querySelectorAll('.submit-case').forEach(button => {
    button.addEventListener('click', function () {
        if (source.getFeatures().length > 0) {
            createFeatureCollection();
            clearMapOfFunctions();
        } else { 
            console.log("No features to submit"); //for debugging
        }
    });
});


/* 
*****************************************************************
**  Section 3: Takes drawn objects and make them into GeoJson  **
*****************************************************************
**  To do list:                                                **
**  - Send the made GeoJson to the backend                     **
**  - Add the API for county and municipality, and get the     **
**    data based on the coordinates of the drawn objects       **
*****************************************************************
*/

var geoJsonFeatures = []; //Array to store the GeoJson objects

/*  
*  Gets the features from the source and makes GeoJson objects from them
*  Transforms the coordinates from EPSG:3857 to EPSG:4326
*/
function featureToGeoJson() {
    if (source.getFeatures().length > 0) {
        source.getFeatures().forEach(function (feature) {
            var coords = feature.getGeometry().getCoordinates();
            var featureType = feature.getGeometry().getType();

            //Transforms the coordinates from EPSG:3857 to EPSG:4326
            //Split by featureType to handle different array depths
            if (featureType == "Point") {
                coords = ol.proj.transform(coords, 'EPSG:3857', 'EPSG:4326');
            } else if (featureType == "LineString") {
                for (let i = 0; i < coords.length; i++) {
                    coords[i] = ol.proj.transform(coords[i], 'EPSG:3857', 'EPSG:4326');
                }
            } else if (featureType == "Polygon") {
                for (let i = 0; i < coords.length; i++) {
                    for (let j = 0; j < coords[i].length; j++) {
                        coords[i][j] = ol.proj.transform(coords[i][j], 'EPSG:3857', 'EPSG:4326');
                    }
                }
            }

            console.log(coords); //for debugging
           
            let geoJsonFeature = {
                type: 'Feature',
                properties: {},
                geometry: {
                    coordinates: coords,
                    type: featureType
                }
            }
            geoJsonFeatures.push(geoJsonFeature); //Pushes the GeoJson object to the array
        }); 
        console.log(geoJsonFeatures); //for debugging 
    }
}



/* Collects the GeoJson objects into one FeatureCollection*/
function createFeatureCollection() {
    featureToGeoJson();
    let featureCollection = {
        type: 'FeatureCollection',
        features: geoJsonFeatures
    };
    console.log(featureCollection);
    var geoJsonString = JSON.stringify(featureCollection);
    console.log(geoJsonString);
}


/* Function 3: Pushes the GeoJson object somewhere*/