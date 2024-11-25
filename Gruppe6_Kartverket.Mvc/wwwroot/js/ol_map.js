//Sets the attribution to non-collapsible
document.addEventListener("DOMContentLoaded", function () {

    const attribution = new ol.control.Attribution({
        collapsible: false,
    });

    //The source for the map
    const raster = new ol.layer.Tile({
        source: new ol.source.OSM(), //OpenStreetMap gives ESPG:3857 projection by default
    });

    var geoJsonString = document.getElementById("CaseLocationGeoJSON").innerText;
    var geoJsonObject = JSON.parse(geoJsonString);

    console.log(geoJsonObject);

    var features = new ol.format.GeoJSON().readFeatures(geoJsonObject, {
        dataProjection: 'EPSG:4326',
        featureProjection: 'EPSG:3857'
    });

    //The source for the vector layer that will be drawn on the map
    const vectorSource = new ol.source.Vector({
        features: features,
    });


    
    const vector = new ol.layer.Vector({
        source: vectorSource,
        style: {
            'fill-color': 'rgba(255, 255, 255, 0.2)', // White with 20% opacity
            'stroke-color': '#5090ff',
            'stroke-width': 2,
        },
    });


    function getCenterCoordinates() {
        var coords = geoJsonObject.geometry.coordinates;
        var featureType = geoJsonObject.geometry.type;

        if (featureType == "Point") {
            coords = ol.proj.transform(coords, 'EPSG:4326', 'EPSG:3857');
        } else if (featureType == "LineString") {
            for (let i = 0; i < coords.length; i++) {
                coords[i] = ol.proj.transform(coords[i], 'EPSG:4326', 'EPSG:3857');
            }
        } else if (featureType == "Polygon") {
            for (let i = 0; i < coords.length; i++) {
                for (let j = 0; j < coords[i].length; j++) {
                    coords[i][j] = ol.proj.transform(coords[i][j], 'EPSG:4326', 'EPSG:3857');
                }
            }
        }


        var startingCoords = getCenterOfFeature(coords);
        return startingCoords;
    };



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


    var mapCenter = getCenterCoordinates();
    
    //Initializes the map
    const map = new ol.Map({
        layers: [raster, vector],
        target: 'map-container',
        view: new ol.View({
            projection: 'EPSG:3857', // Projects EPSG:3857 to match the OpenStreetMap tiles
            constrainResolution: true, // Set whether the view should allow intermediary zoom levels.
            center: mapCenter,  //[8.002189, 58.163703], // [lon, lat] Starts over Kristiansand (Maybe fetch from user?)
            zoom: 9,   // Initial zoom level
        }),
    });

    console.log(vectorSource.getFeatures());

});