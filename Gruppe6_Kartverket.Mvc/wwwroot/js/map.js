//Sets the attribution to non-collapsible
const attribution = new ol.control.Attribution({
    collapsible: false,
});

const map = new ol.Map({
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM()
        })
    ],
    view: new ol.View({
        projection: 'EPSG:4326',
        constrainResolution: true,
        center: [8.002189, 58.163703, 0], // [lon, lat]
        zoom: 10
    }),
    target: 'map'
});