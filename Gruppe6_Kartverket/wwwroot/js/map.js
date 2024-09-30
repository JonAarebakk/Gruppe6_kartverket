import Map from 'ol/Map';
import OSM from '~/lib/ol/source/OSM.js';
//import TileLayer from '~/lib/ol/layer/TileLayer.js';
//import View from 'ol/View';
//import TileJSON from '~ol/source/TileJSON.js';
//import ImageSource from '~/lib/ol/source/Image.js';
//import Attribution from 'ol/control/Attribution';
//import Tile from 'ol/layer/Tile';
//import * as olProj from '~/lib/ol/proj';

const key = 'fZSqzvs0JljaDaoMhFNh';
const source = new ol.source.TileJSON({
    url: `https://api.maptiler.com/maps/streets-v2/tiles.json?key=${key}`,
    tileSize: 512,
    crossOrigin: 'anonymous'
});

const attribution = new ol.control.Attribution({
    collapsible: false,
});

const source1 = new ol.renderer.canvas.ImageLayer({
    //url: 'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
    url: 'https://howtodrawforkids.com/wp-content/uploads/2021/05/How-to-draw-pikachu-for-kids.jpg',
    
    crossOrigin: ''
});


const map = new ol.Map({
    //target: 'map',
    layers: [
        new ol.layer.Image({
            source: source1
            //source: source
        }),
    ],
    target: 'map',
    view: new ol.View({
        constrainResolution: true,
        center: ol.proj.fromLonLat([16.62662018, 49.2125578]),
        zoom: 14
    }),
    //target: 'map',
});