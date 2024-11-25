/*
*************************************
**  Section 1: Setting up the map  **
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
        'stroke-color': '#5090ff',
        'stroke-width': 2
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
        zoom: 8,   // Initial zoom level
    }),
});

//#endregion
/*
*********************************************************
**  Section 2: Configure map buttons and interactions  **
*********************************************************
*/
//#region Section 2 /*Lets Visual studio collapse this section*/

//Enables modification of the vector layer i.e. the ability to move the drawn objects
const modify = new ol.interaction.Modify({ source: vectorSource });
map.addInteraction(modify);


let draw, snap; // global so we can remove it later
var actionType = document.getElementById('actionType'); // What the user wants to do to
var drawnFeature = false;  //Counter for the amount of drawn objects
var featureId = 0; //Used to set feature ID

//Choose the interaction based on the actionType variable
function chooseInteractions() {
    const value = actionType.value;

    if (value !== 'None') {
        if (value == 'Delete') {
            deleteFeature();
        } else {
            drawFeature();
        }
    } else {
        deselectMapButtons();
    }
};


function deleteFeature() {
    if (drawnFeature = true) {
        actionType.value = 'None'
        clearMapOfFunctions();
        resetInteractions();
    }
}

//Function to draw a feature
function drawFeature() {
    draw = new ol.interaction.Draw({
        source: vectorSource,
        type: actionType.value,
    });
    draw.on('drawend', function () {       //What to do when a feature is drawn
        resetInteractions();
    });



    map.addInteraction(draw);
    snap = new ol.interaction.Snap({ source: vectorSource });
    map.addInteraction(snap);
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

    chooseInteractions();  //Runs the chooseInteractions to decide what to do
}


//Makes sure that only one feature is drawn at a time
vectorSource.on("addfeature", function (evt) {
    if (drawnFeature == true) {
        var obsoleteFeature = vectorSource.getFeatureById(featureId);
        vectorSource.removeFeature(obsoleteFeature);
    }
    drawnFeature = true;
    featureId = featureId + 1;
    var feature = evt.feature;
    feature.setId(featureId);
});


//Changes the active map button for styling and functionality
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

//Sets all map buttons to inactive for styling and functionality
function deselectMapButtons() {
    var buttons = document.querySelectorAll('.map-button');
    buttons.forEach(button => {
        if (button.classList.contains('active')) {
            button.classList.remove('active');
            actionType.value = 'None';
            map.removeInteraction(draw);
            map.removeInteraction(snap);
        }
    });
}



function clearMapOfFunctions() {
    vectorSource.clear();
    drawnFeature = false;
}

//#endregion
/*
*****************************************************************
**  Section 3: Takes drawn objects and make them into GeoJson  **
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

            var centerOfFeature = getCenterOfFeature(coords);

            let geoJsonFeature = {
                type: 'Feature',
                properties: {},
                geometry: {
                    type: featureType,
                    coordinates: coords
                }
            }
            var geoJsonString = JSON.stringify(geoJsonFeature);
            document.getElementById("geoJsonInput").value = geoJsonString;

            document.getElementById("centerLongitudeInput").value = centerOfFeature[0];
            document.getElementById("centerLatitudeInput").value = centerOfFeature[1];
        });
    }
}

//Loops through the coordinates and calculates the center of the feature
function getCenterOfFeature(coords) {
    let totalLon = 0;
    let totalLat = 0;
    let count = 0;

    function calculateCenter(coordinates) {
        if (Array.isArray(coordinates[0])) {
            for (let i = 0; i < coordinates.length; i++) {
                calculateCenter(coordinates[i]);
            }
        } else {
            totalLon += coordinates[0];
            totalLat += coordinates[1];
            count++;
        }
    }

    calculateCenter(coords);

    const centerLon = totalLon / count;
    const centerLat = totalLat / count;

    return [centerLon, centerLat];
}


//#endregion

/*
**************************************************
**  Section 4: Configure overlays and popups    **
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
    let newCase = document.getElementById("new-case");
    let mapButtons = document.getElementById("map-button-group");
    let caseInput = document.getElementById("case-input-wrapper");

    newCase.style.display = "flex";
    mapButtons.style.display = "none";
    caseInput.style.display = "none";
    CaseForm.reset();
}


//Checks if all form inputs have changed from their initial values
function isFormValid() {
    if (!CaseForm.CaseTitle.value) {
        return false;
    } else if (!CaseForm.Kategori.value) {
        return false;
    } else if (!CaseForm.Beskrivelse.value) {
        return false;
    } else if (drawnFeature == false) {
        return false;
    } else {
        return true;
    }
}

//Checks if any form inputs have changed from their initial values
function hasFormChanged() {
    if (CaseForm.CaseTitle.value) {
        return true;
    } else if (CaseForm.Kategori.value) {
        return true;
    } else if (CaseForm.Beskrivelse.value) {
        return true;
    } else if (drawnFeature == true) {
        return true;
    } else {
        return false;
    }
}


function submitCase() {
    var formValidity = isFormValid();

    if (formValidity) {
        featureToGeoJson();
        clearMapOfFunctions();
        closePopupsAndOverlays();

        document.getElementById("CaseForm").submit();
        CaseForm.reset();

        //showPopup('case-submitted');
    } else {
        closePopupsAndOverlays();
        showPopup('case-not-valid');
    }
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
**************************************************
*/
//#region Section 5 /*Lets Visual studio collapse this section*/

// Add event listener to the new-case-button
const newCaseButton = document.getElementById('new-case-button');
newCaseButton.addEventListener('click', function () {
    var isUserLoggedIn = document.getElementById('loggedIn').textContent;

    if (isUserLoggedIn == "loggedIn") {
        startNewCase();
    } else {
        showPopup('new-case-explanation');
    }
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
        var formChanged = hasFormChanged();

        if (formChanged) {
            showPopup('avbryt-case');
        }
    });
});

document.querySelectorAll('.cancel-case-confirmation').forEach(button => {
    button.addEventListener('click', function () {

        actionType.value = 'None';
        setActiveMapButton();          //Resets the map buttons
        clearMapOfFunctions();
        closePopupsAndOverlays();

        cancelCase();                  //Resets the case input form
    });
});


document.querySelectorAll('.submit-case').forEach(button => {
    button.addEventListener('click', function () {
        var caseValidity = isFormValid();
        if (caseValidity) {
            showPopup('submit-case-popup');
        } else {
            showPopup('case-not-valid');
        }
    });
});

const submitCaseConfirmation = document.querySelector('.submit-case-confirmation').addEventListener('click', function () {
    submitCase();
});

document.querySelectorAll('.popup-close').forEach(button => {
    button.addEventListener('click', function () {
        closePopupsAndOverlays();
    });
});

const deactiveMapButtons = document.querySelector('.case-input-wrapper').addEventListener('click', function () {
    deselectMapButtons();
});

//#endregion
