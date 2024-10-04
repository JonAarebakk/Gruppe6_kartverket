/*
*  A test file for troubleshooting maps
*  Don't think it works
*/

// Map views always need a projection.  Here we just want to map image
// coordinates directly to map coordinates, so we create a projection that uses
// the image extent in pixels.
const extent = [0, 0, 1024, 968];
const projection = new ol.source.Projection({
    code: 'xkcd-image',
    units: 'pixels',
    extent: extent,
});

const map = new ol.Map({
    layers: [
        //new ol.renderer.canvas.ImageLayer({
        new ol.layer.Image({
            source: new ol.source.Static({
                attributions: '© <a href="https://xkcd.com/license.html">xkcd</a>',
                url: 'https://imgs.xkcd.com/comics/online_communities.png',
                projection: projection,
                imageExtent: extent,
            }),
        }),
    ],
    target: 'map',
    view: new ol.View({
        projection: projection,
        center: ol.extent.getCenter(extent),
        zoom: 2,
        maxZoom: 8,
    }),
});
