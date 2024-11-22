/*
*************************************
**  Section 1: Setting up the map  **
**********************************************
**  To do list:                             **
**  - Change color for drawn features       **
**  - Implement fetch coordinates from user **
**********************************************
*/
//#region Section 1 /*Lets Visual studio collapse this section*/

//Sets the attribution to non-collapsible
const attribution = new ol.control.Attribution({
    collapsible: false,
});

//The source for the map
const raster = new ol.layer.Tile({
    source: new ol.source.OSM(), //OpenStreetMap gives ESPG:3857 projection by default
});

//The source for the vector layer that will be drawn on the map
const vectorSource = new ol.source.Vector();
const vector = new ol.layer.Vector({
    source: vectorSource,
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

//#endregion
/*
*********************************************************
**  Section 2: Configure map buttons and interactions  **
*********************************************************
**  To do list:                                        **
**  - make select.on to deactivate proparly            **
**  - remove console.log() debugging statements        **
**  - for cancel-case-button add a popUp for are you   **
**    sure? And if yes, clear the map                  **
*********************************************************
*/
//#region Section 2 /*Lets Visual studio collapse this section*/

//Enables modification of the vector layer i.e. the ability to move the drawn objects
const modify = new ol.interaction.Modify({ source: vectorSource });
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
        actionType.value = 'None';  //Set the actionType back to "None"
        resetInteractions();        //Reset the interactions
        showPopup('delete-features'); //Gives a popup for no more features to delete
    }
}

//Function to draw a feature
function drawFeature() {
    draw = new ol.interaction.Draw({
        source: vectorSource,
        type: actionType.value,
    });
    draw.on('drawend', function () {       //What to do when a feature is drawn
        drawnFeatures = drawnFeatures + 1; //Increment the drawnFeatures variable
        resetInteractions();
    });

    map.addInteraction(draw);
    snap = new ol.interaction.Snap({ source: vectorSource });
    map.addInteraction(snap);
}

//Gives a warning when the user tries to draw more than 10 objects
function maxFeaturesWarning() {
    if (vectorSource.getFeatures().length <= 10) {
        showPopup('max-features'); //Gives a popup for no more features to delete
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
vectorSource.on("addfeature", function (evt) {
    featureCount = featureCount + 1;
    var feature = evt.feature;
    feature.setId(featureCount);
});

//When a feature is selected, remove it from the source
select.on('select', function (evt) {
    if ((evt.selected !== null) && (evt.selected.length > 0)) { //Check if a feature is selected
        var featureID = evt.selected[0].getId();        //Get the ID of the selected feature
        var feature = vectorSource.getFeatureById(featureID); //Uses the featureID to get the feature

        vectorSource.removeFeature(feature);    //Remove the feature from the source
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
    vectorSource.clear();
    drawnFeatures = 0;
}

//#endregion
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
//#region Section 3 /*Lets Visual studio collapse this section*/

var geoJsonFeatures = []; //Array to store the GeoJson objects

/*
*  Gets the features from the source and makes GeoJson objects from them
*  Transforms the coordinates from EPSG:3857 to EPSG:4326
*/
function featureToGeoJson() {
    if (vectorSource.getFeatures().length > 0) {
        vectorSource.getFeatures().forEach(function (feature) {
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

/* Function to get GeoJSON from the map */
function getGeoJsonFromMap() {
    var features = vectorSource.getFeatures();
    var geoJsonFormat = new ol.format.GeoJSON();
    var geoJsonData = geoJsonFormat.writeFeatures(features);
    return geoJsonData;
}

//#endregion

/*
**************************************************
**  Section 4: Configure overlays and popups    **
**************************************************
**  To do list:                                 **
**  - In showPopup(), differentiate between     **
**    the different overlays                    **
**************************************************
*/
//#region Section 4 /*Lets Visual studio collapse this section*/

//Toggle the elements that are displayed when a new case is started
function startNewCase() {
    //NB! Start with a if(logged in){do the things} else {give popup for log in/give number}

    let newCase = document.getElementById("new-case");
    let mapButtons = document.getElementById("map-button-group");
    let caseInput = document.getElementById("case-input-wrapper");

    newCase.style.display = "none";
    mapButtons.style.display = "flex";
    caseInput.style.display = "flex";
}

//Toggle the elements back to "default"
function cancelCase() {
    //Start with if(there are changes){give warning} else {cancelCase}

    let newCase = document.getElementById("new-case");
    let mapButtons = document.getElementById("map-button-group");
    let caseInput = document.getElementById("case-input-wrapper");

    newCase.style.display = "flex";
    mapButtons.style.display = "none";
    caseInput.style.display = "none";
    CaseForm.reset(); //Resets the form to default values
}

// A close function for the popups and overlays
function closePopupsAndOverlays() {
    let popup = document.getElementsByClassName("popup");
    for (let i = 0; i < popup.length; i++) {
        popup[i].style.display = "none";
    }
    let overlay = document.getElementsByClassName("overlay");
    for (let i = 0; i < overlay.length; i++) {
        overlay[i].style.display = "none";
    }
}

// Show the popup with the given ID //TODO: make the overlay different for the tlf popup
function showPopup(popupID) {
    closePopupsAndOverlays();
    if (popupID != "insert-phone-number") {
        let popupOverlay = document.getElementById("popup-overlay");
        popupOverlay.style.display = "block";
        let popup = document.getElementById(popupID);
        popup.style.display = "block";
    } else if (popupID == "insert-phone-number") {
        let popupOverlay = document.getElementById("insert-phone-number-overlay");
        popupOverlay.style.display = "block";
        let popup = document.getElementById(popupID);
        popup.style.display = "block";
    }
}

//#endregion
/*
**************************************************
**  Section 5: Give buttons functionality       **
****************************************************************
**  To do list:                                               **
**  - Make one general function that initializes all buttons  **
**  - Add if() on necCaseButton                               **
**  - Add if() on cancelCaseButton                            **
**  - Combine the two cancelCaseButton functions              **
**  - change querySelectorAll to querySelector on elements    **
**    where there is only one element                         **
**  - SubmitCaseConfirmation resets the .mapButtons proparly  **
**    and goes back to "default state"                        **
****************************************************************
*/
//#region Section 5 /*Lets Visual studio collapse this section*/

// Add event listener to the new-case-button
const newCaseButton = document.getElementById('new-case-button');
newCaseButton.addEventListener('click', function () {
    //if(logged in) {startNewCase()} else {give popup for log in/give number}
    startNewCase();
    showPopup('new-case-explanation');
});

const toTlfPopup = document.getElementById('to-tlf-popup');
toTlfPopup.addEventListener('click', function () {
    showPopup('insert-phone-number');
});

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

        cancelCase();                  //Resets the case input form
    });
});

document.querySelectorAll('.submit-case').forEach(button => {
    button.addEventListener('click', function () {
        if (vectorSource.getFeatures().length > 0) { //Maybe make a function for checking if any changes are made?
            showPopup('submit-case-popup');
            //createFeatureCollection();
            //clearMapOfFunctions();
        } else {
            console.log("No features to submit"); //for debugging //Make a popup for no features to submit maybe
        }
    });
});

const submitCaseConfirmation = document.querySelector('.submit-case-confirmation').addEventListener('click', function () {
    createFeatureCollection();
    clearMapOfFunctions();
    closePopupsAndOverlays();
    showPopup('case-submitted');
});

document.querySelectorAll('.popup-close').forEach(button => {
    button.addEventListener('click', function () {
        closePopupsAndOverlays();
    });
});

//#endregion