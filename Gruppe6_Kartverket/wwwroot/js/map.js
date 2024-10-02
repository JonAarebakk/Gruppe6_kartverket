//Puts OpenStrretMap attribution in the bottom right corner
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
        constrainResolution: true,
        center: ol.proj.fromLonLat([7.995611, 58.146722]),
        zoom: 14
    }),
    target: 'map'
});

console.log(2);