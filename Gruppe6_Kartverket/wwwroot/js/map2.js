﻿//import * as map1 "ol/Map";

const key = 'fZSqzvs0JljaDaoMhFNh';
const source = new ol.source.TileJSON({
    url: `https://api.maptiler.com/maps/streets-v2/tiles.json?key=${key}`,
    tileSize: 512,
    crossOrigin: 'anonymous'
});


const attribution = new ol.control.Attribution({
    collapsible: false,
});

const map = new ol.Map({
    layers: [
        new ol.layer.Tile({
            source: source
        })
    ],
    target: 'map',
    view: new ol.View({
        constrainResolution: true,
        center: ol.proj.fromLonLat([16.62662018, 49.2125578]),
        zoom: 14
    })
});